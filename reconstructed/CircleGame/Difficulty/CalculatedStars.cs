using System.IO;
using Ohno.CircleGame.Selection;

namespace Ohno.CircleGame.Difficulty;

public enum StarCalculationState { NotCalculated,Pending,Calculated,Unsupported,Failed,Stale,Cancelled }
public sealed record CalculatedStarState(StarCalculationState State,string Reason,StarCalculationResult? Result=null)
{
    public string Text=>State==StarCalculationState.Calculated&&Result is{} result ? result.Display+" · "+result.Provenance :
        "Stars not calculated · "+(State==StarCalculationState.Pending?"calculation pending":State==StarCalculationState.NotCalculated?"restricted calculation not requested":State.ToString().ToLowerInvariant()+" · "+Reason);
}
public sealed record StarCacheKey(string ChartSha256,string CalculatorProfile,int Ruleset,int Mods,string PreparationProfile);

// ENGINEERING bounded process cache / one worker. No database or native scheduling claim.
public sealed class CalculatedStarCache : IDisposable
{
    public const int MaximumEntries=64,MaximumPending=32;
    private sealed class Work(long id,CatalogChart chart,CancellationTokenSource cancellation)
    {
        internal readonly long Id=id;internal readonly CatalogChart Chart=chart;internal readonly CancellationTokenSource Cancellation=cancellation;
        internal readonly TaskCompletionSource<CalculatedStarState> Completion=new(TaskCreationOptions.RunContinuationsAsynchronously);
    }
    private readonly object sync=new();
    private readonly Dictionary<StarCacheKey,CalculatedStarState> completed=[];
    private readonly Queue<StarCacheKey> insertionOrder=[];
    private readonly Dictionary<StarCacheKey,Work> pending=[];
    private readonly SemaphoreSlim worker=new(1,1);
    private readonly CancellationTokenSource lifetime=new();
    private readonly Func<CatalogChart,CancellationToken,Task<StarCalculationResult>> calculate;
    private bool disposed,resourcesDisposed;private long sequence,joined;private int jobs;
    public long JoinedPendingRequests {get{lock(sync)return joined;}}
    public int CompletedCount {get{lock(sync)return completed.Count;}}
    public int PendingCount {get{lock(sync)return pending.Count;}}
    public CalculatedStarCache(Func<CatalogChart,CancellationToken,Task<StarCalculationResult>>? calculate=null)
    {this.calculate=calculate??((chart,token)=>Task.Run(()=>CircleStarCalculator.Calculate(chart,token),token));}
    public static StarCacheKey? Key(CatalogChart chart)=>chart.Fingerprint is{} fp?new(fp.Sha256,StarAdmission.CalculatorProfile,0,0,"CS5:OD5:AR5-or10:512x384:zero-stack:shift0:rate1"):null;
    public Task<CalculatedStarState> GetAsync(CatalogChart chart,CancellationToken token=default)
    {
        // All file reads, including a cache-hit revision check, occur off the UI thread.
        return Task.Run<CalculatedStarState>(async()=>
        {
            try{token.ThrowIfCancellationRequested();chart.Verify(token);}
            catch(OperationCanceledException){return new(StarCalculationState.Cancelled,"Request cancelled.");}
            catch(Exception e){return new(StarCalculationState.Stale,e.Message);}
            Work job;StarCacheKey? key=Key(chart);
            if(key is null)return new(StarCalculationState.Unsupported,"No complete chart fingerprint.");
            lock(sync)
            {
                if(disposed)return new(StarCalculationState.Cancelled,"Cache disposed.");
                if(completed.TryGetValue(key,out var hit))return hit;
                if(pending.TryGetValue(key,out var existing)&&!existing.Cancellation.IsCancellationRequested){job=existing;joined++;}
                else
                {
                    if(jobs>=MaximumPending)return new(StarCalculationState.Failed,"Bounded calculation queue full.");
                    job=new(++sequence,chart,CancellationTokenSource.CreateLinkedTokenSource(lifetime.Token,token));pending[key]=job;jobs++;
                    _=RunAsync(key,job);
                }
            }
            try{return await job.Completion.Task.WaitAsync(token).ConfigureAwait(false);}
            catch(OperationCanceledException){return new(StarCalculationState.Cancelled,"Request cancelled.");}
        });
    }
    private async Task RunAsync(StarCacheKey key,Work job)
    {
        // First owner's token owns this shared pending job; duplicates await it with their own cancellation.
        CalculatedStarState result;bool acquired=false;
        try
        {
            await worker.WaitAsync(job.Cancellation.Token).ConfigureAwait(false);acquired=true;
            var numeric=await calculate(job.Chart,job.Cancellation.Token).ConfigureAwait(false);
            job.Cancellation.Token.ThrowIfCancellationRequested();job.Chart.Verify(job.Cancellation.Token);
            if(!double.IsFinite(numeric.CalculatedDouble)||!float.IsFinite(numeric.CachedSingle))throw new UnsupportedStarProfileException("Nonfinite result cannot be cached.");
            result=new(StarCalculationState.Calculated,"",numeric);
        }
        catch(OperationCanceledException){result=new(StarCalculationState.Cancelled,"Calculation cancelled.");}
        catch(UnsupportedStarProfileException e){result=new(StarCalculationState.Unsupported,e.Message);}
        catch(Exception e)when(e is IOException or InvalidDataException){result=new(StarCalculationState.Stale,e.Message);}
        catch(Exception e){result=new(StarCalculationState.Failed,e.Message);}
        finally{if(acquired)worker.Release();}
        lock(sync)
        {
            jobs--;if(pending.TryGetValue(key,out var current)&&current.Id==job.Id)pending.Remove(key);
            if(!disposed&&!job.Cancellation.IsCancellationRequested&&result.State is StarCalculationState.Calculated or StarCalculationState.Unsupported)
            {
                if(!completed.ContainsKey(key))insertionOrder.Enqueue(key);completed[key]=result;
                while(completed.Count>MaximumEntries)completed.Remove(insertionOrder.Dequeue());
            }
            if(disposed)result=new(StarCalculationState.Cancelled,"Cache disposed before publication.");
            ReleaseIfFinished();
        }
        job.Cancellation.Dispose();job.Completion.TrySetResult(result);
    }
    private void ReleaseIfFinished()
    {if(disposed&&jobs==0&&!resourcesDisposed){resourcesDisposed=true;worker.Dispose();lifetime.Dispose();}}
    public void Dispose()
    {lock(sync){if(disposed)return;disposed=true;lifetime.Cancel();completed.Clear();insertionOrder.Clear();ReleaseIfFinished();}}
}

// A controller retains chart references and catalog generation, never authoritative selection ownership.
public sealed class SelectPlayStarController(CalculatedStarCache cache) : IDisposable
{
    private readonly object sync=new();private CancellationTokenSource? cancellation;
    private CatalogChart? chart;private long catalogGeneration=-1,requestGeneration;private bool disposed;
    private CalculatedStarState state=new(StarCalculationState.NotCalculated,"");
    public event Action? Changed;
    public Task Completion {get;private set;}=Task.CompletedTask;
    public CalculatedStarState StateFor(CatalogChart row)
    {lock(sync)return ReferenceEquals(chart,row)?state:new(StarCalculationState.NotCalculated,"");}
    public void Highlight(CatalogChart? candidate,long catalogEpoch)
    {
        long request;CancellationToken token;
        lock(sync)
        {
            if(disposed||catalogEpoch<catalogGeneration)return;
            if(ReferenceEquals(chart,candidate)&&catalogEpoch==catalogGeneration&&state.State!=StarCalculationState.Cancelled)return;
            cancellation?.Cancel();cancellation?.Dispose();chart=candidate;catalogGeneration=catalogEpoch;request=++requestGeneration;
            if(candidate is null){state=new(StarCalculationState.NotCalculated,"");Completion=Task.CompletedTask;return;}
            cancellation=new();token=cancellation.Token;state=new(StarCalculationState.Pending,"");
            Completion=PublishAsync(candidate,catalogEpoch,request,token);
        }
        Changed?.Invoke();
    }
    private async Task PublishAsync(CatalogChart target,long epoch,long request,CancellationToken token)
    {
        var result=await cache.GetAsync(target,token).ConfigureAwait(false);
        lock(sync)
        {
            if(disposed||token.IsCancellationRequested||epoch!=catalogGeneration||request!=requestGeneration||!ReferenceEquals(target,chart))return;
            state=result;
        }
        Changed?.Invoke();
    }
    public void Invalidate()
    {
        lock(sync){if(disposed)return;cancellation?.Cancel();requestGeneration++;state=new(StarCalculationState.Cancelled,"Catalog/lifetime generation cancelled.");}
        Changed?.Invoke();
    }
    public void Dispose(){lock(sync){if(disposed)return;disposed=true;cancellation?.Cancel();cancellation?.Dispose();requestGeneration++;}}
}
