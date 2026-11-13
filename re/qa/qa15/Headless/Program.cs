using System.Globalization;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Difficulty;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using CircleDefinition=Ohno.CircleGame.Difficulty.CircleDefinition;

internal static class Program
{
    static readonly Evidence E=new("QA15 HEADLESS");
    static string root="",temp="",baseline="";
    static int comparisons;
    static void Eq<T>(T a,T b,string note)=>E.Check(EqualityComparer<T>.Default.Equals(a,b),note,a,b);
    static TaskCompletionSource<bool> Signal()=>new(TaskCreationOptions.RunContinuationsAsynchronously);
    static async Task Until(Func<bool> ready){using var timeout=new CancellationTokenSource(TimeSpan.FromSeconds(10));while(!ready()){timeout.Token.ThrowIfCancellationRequested();await Task.Yield();}}
    static CatalogChart Copy(string name,string? text=null){string p=Path.Combine(temp,name+".osu");File.WriteAllText(p,text??baseline);return LocalChartCatalog.Index(p);}
    static void Unsupported(Action a){bool refused=false;try{a();}catch(UnsupportedStarProfileException){refused=true;}E.Check(refused,"Explicit unsupported exception",true,refused);}
    static object Pair(DifficultyPair p)=>new{index=p.Index,time=p.Time,delta=p.Delta,strain=p.Strain,position=new[]{(double)p.Position.X,(double)p.Position.Y},previous_position=new[]{(double)p.PreviousPosition.X,(double)p.PreviousPosition.Y},radius=(double)p.Preparation.Radius,preempt=p.Preparation.Preempt,window=p.Preparation.FullWindow,gap=p.Gap,jump=p.Jump,angle=p.Angle,axis=p.Axis};
    static object[] Patterns(IReadOnlyList<PatternSnapshot> p)=>p.Select(x=>(object)x.Values).ToArray();
    static object Trace(StarCalculationResult result)
    {
        var t=result.Trace!;
        return new{pairs=t.Pairs.Select(Pair).ToArray(),trace=t.Rows.Select((r,i)=>new{
            raw0=r.Movement.Composite,rhythm_multiplier=r.Rhythm.Multiplier,
            rhythm=new{seed=r.Rhythm.Seed,capacity=r.Rhythm.Capacity,tolerance=r.Rhythm.Tolerance,events=r.Rhythm.Steps.Select(x=>new{k=x.K,pair=x.Pair,dt=x.Dt,prior=x.Prior,difference=x.Difference,history_weight=x.HistoryWeight,group=x.Group.Values,previous_group=x.PreviousGroup.Values,saved=Patterns(x.Saved),active=x.Active,value=x.Value,sum=x.Sum,flags=x.Flags,before_saved=Patterns(x.BeforeSaved)}).ToArray(),unattenuated=r.Rhythm.Sum,attenuation=r.Rhythm.Attenuation},
            visibility=i==0?(object)new{raw=r.Visibility.Raw}:new{past=r.Visibility.Past.Select(x=>new[]{(double)x.Index,x.Value}).ToArray(),future=r.Visibility.Future.Select(x=>new[]{(double)x.Index,x.Value}).ToArray(),angles=r.Visibility.Angles.Select(x=>new[]{(double)x.Index,x.Value,x.Cap,x.Operand}).ToArray(),angle=r.Visibility.Angle,backward=r.Visibility.Backward,forward=r.Visibility.Forward,next_jump=r.Visibility.NextJump,forward_root=r.Visibility.ForwardRoot,velocity=r.Visibility.Velocity,movement=r.Visibility.Movement, @short=r.Visibility.Short,raw=r.Visibility.Raw},
            raw1=r.IntervalRaw,states=r.States,outputs=r.Outputs,movement_terms=i>1?new[]{r.Movement.Movement,r.Movement.Jump,r.Movement.VelocityAngle}:new[]{r.Movement.Movement,r.Movement.VelocityAngle}}).ToArray(),
            outputs=t.ComponentOutputs,aggregates=t.Aggregates,processed_segments=t.Envelope.Processed.Select(x=>new[]{x.Value,x.Duration}).ToArray(),component2_transformed=t.Component2Transformed,
            final=new{@double=result.CalculatedDouble,single=(double)result.CachedSingle,single_bits_le=result.SingleBitsLittleEndian,display=double.Parse(result.Display.TrimEnd('★'),CultureInfo.InvariantCulture)}};
    }
    static void Compare(JsonElement expected,JsonElement actual,string path="")
    {
        comparisons++;
        if(expected.ValueKind==JsonValueKind.Object)
        {
            if(actual.ValueKind!=JsonValueKind.Object)throw new Exception(path+" expected object");
            var keys=expected.EnumerateObject().Select(x=>x.Name).Order().ToArray();
            if(!keys.SequenceEqual(actual.EnumerateObject().Select(x=>x.Name).Order()))throw new Exception(path+" key mismatch");
            foreach(var p in expected.EnumerateObject())Compare(p.Value,actual.GetProperty(p.Name),path+"/"+p.Name);
        }
        else if(expected.ValueKind==JsonValueKind.Array)
        {
            if(actual.ValueKind!=JsonValueKind.Array||expected.GetArrayLength()!=actual.GetArrayLength())throw new Exception(path+" array length mismatch");
            for(int i=0;i<expected.GetArrayLength();i++)Compare(expected[i],actual[i],path+"/"+i);
        }
        else if(expected.ValueKind==JsonValueKind.Number)
        {
            double want=expected.GetDouble(),got=actual.GetDouble();
            bool exact=expected.TryGetInt64(out _)||path.EndsWith("/final/single")||path.EndsWith("/final/display");
            bool ok=double.IsFinite(got)&&(exact?want==got:Math.Abs(want-got)<=Math.Max(5e-13,Math.Abs(want)*5e-12));
            if(!ok)throw new Exception($"{path}: expected {want:R}; actual {got:R}; exact={exact}");
        }
        else if(expected.ToString()!=actual.ToString())throw new Exception(path+": expected "+expected+" actual "+actual);
    }
    static async Task<int> Main(string[] args)
    {
        root=args[0];CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
        temp=Path.Combine(Path.GetTempPath(),"qa15-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        string qa=Path.Combine(root,"re/qa/qa15"),b=Path.Combine(qa,"beatmaps");
        using var zipped=new GZipStream(new MemoryStream(Convert.FromBase64String(File.ReadAllText(Path.Combine(qa,"verification/expected.json.gz.b64")))),CompressionMode.Decompress);
        using var expect=JsonDocument.Parse(zipped);using var manifest=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(b,"manifest.json")));
        var rows=manifest.RootElement.GetProperty("files").EnumerateArray().ToArray();
        baseline=File.ReadAllText(Path.Combine(b,rows[2].GetProperty("path").GetString()!));
        try
        {
            foreach(var item in expect.RootElement.GetProperty("rows").EnumerateArray())
            {
                string id=item.GetProperty("id").GetString()!;
                await E.Run("numerical/"+id,()=>{
                    StarCalculationResult actual;
                    if(item.TryGetProperty("path",out var path))
                    {
                        string p=Path.Combine(b,path.GetString()!);var row=rows.Single(x=>x.GetProperty("id").GetString()==id);
                        var bytes=File.ReadAllBytes(p);Eq(row.GetProperty("sha256").GetString(),Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant(),"Packaged exact SHA256");
                        var chart=LocalChartCatalog.Index(p);E.Check(chart.Available,"Actual catalog admits packaged bytes",true,chart.Available);
                        Eq(row.GetProperty("circle_count").GetInt32(),chart.LoadGameplay().Circles.Count,"Actual gameplay parser sees same circles");
                        var admission=StarAdmission.Read(chart);var prm=item.GetProperty("params");Eq(prm[0].GetSingle(),admission.Preparation.Radius,"Original-derived Single radius");Eq(prm[1].GetDouble(),admission.Preparation.Preempt,"Original-derived AR preempt");Eq(prm[2].GetDouble(),admission.Preparation.FullWindow,"Original-derived full OD window");
                        actual=CircleStarCalculator.Calculate(chart,capture:true);
                        Eq(row.GetProperty("expected_cached_single_bits_le").GetString(),actual.SingleBitsLittleEndian,"Manifest exact cached Single bits");Eq(row.GetProperty("expected_r17_display").GetString(),actual.Display,"Manifest display floor");
                    }
                    else
                    {
                        var circles=item.GetProperty("circles").EnumerateArray().Select(x=>new CircleDefinition(x[0].GetInt32(),x[1].GetSingle(),x[2].GetSingle())).ToArray();var p=item.GetProperty("params");
                        actual=CircleStarCalculator.CalculatePrepared(circles,new(p[0].GetSingle(),p[1].GetDouble(),p[2].GetDouble()));
                    }
                    var normalized=JsonSerializer.SerializeToElement(Trace(actual),Evidence.JsonOptions);int before=comparisons;
                    E.Charts.Add(new{id,expectedAuthority="verification/expected.json.gz.b64#"+id,actual=normalized});
                    Compare(item.GetProperty("expected"),normalized);
                    E.Check(true,"Full independent trace: geometry, movement, rhythm identities/events, visibility histories, three states, duration, final/cache/display",new{reference=id},new{comparedNodes=comparisons-before});
                });
            }
            var mutations=new Dictionary<string,string>{
                ["format"]=baseline.Replace("format v14","format v13"),["mode"]=baseline.Replace("Mode:0","Mode:1"),["missing-mode"]=baseline.Replace("Mode:0",""),
                ["stacking"]=baseline.Replace("StackLeniency:0","StackLeniency:0.7"),["missing-stacking"]=baseline.Replace("StackLeniency:0",""),["CS"]=baseline.Replace("CircleSize:5","CircleSize:4"),["OD"]=baseline.Replace("OverallDifficulty:5","OverallDifficulty:6"),["AR"]=baseline.Replace("ApproachRate:5","ApproachRate:7"),["NaN"]=baseline.Replace("CircleSize:5","CircleSize:NaN"),["duplicate-key"]=baseline.Replace("Mode:0","Mode:0\nMode:0"),
                ["slider"]=baseline.Replace(",1,0\n",",2,0\n"),["spinner"]=baseline.Replace(",1,0\n",",8,0\n"),["sound"]=baseline.Replace(",1,0\n",",1,2\n"),["sample-columns"]=baseline.Replace(",1,0\n",",1,0,0:0:0:0:\n"),["no-objects"]=baseline[..baseline.IndexOf("[HitObjects]",StringComparison.Ordinal)]};
            foreach(var(name,text) in mutations)await E.Run("admission/"+name,()=>{E.Check(text!=baseline,"Mutation changed actual source");Unsupported(()=>StarAdmission.Parse(text));});
            string header=baseline[..(baseline.IndexOf("[HitObjects]",StringComparison.Ordinal)+"[HitObjects]".Length)]+"\n";
            foreach(var(name,text) in new Dictionary<string,string>{["one"]="0,0,0,1,0\n",["equal"]="0,0,100,1,0\n1,1,100,1,0\n",["reverse"]="0,0,101,1,0\n1,1,100,1,0\n",["negative"]="0,0,-1,1,0\n1,1,100,1,0\n",["overflow"]="0,0,0,1,0\n1,1,2147483648,1,0\n",["coordinate"]="0,0,0,1,0\n513,1,100,1,0\n",["decimal"]="0,0,0,1,0\n1.1,1,100,1,0\n",["type"]="0,0,0,1,0\n1,1,100,21,0\n"})
                await E.Run("admission/rows-"+name,()=>Unsupported(()=>StarAdmission.Parse(header+text)));
            foreach(var(name,profile) in new[]{("mods",new StarProfile(Mods:8)),("width",new StarProfile(Width:1024)),("height",new StarProfile(Height:768)),("shift",new StarProfile(TimeShift:1)),("rate",new StarProfile(Rate:1.5))})
                await E.Run("admission/profile-"+name,()=>Unsupported(()=>StarAdmission.Parse(baseline,profile)));
            await E.Run("admission/limit",()=>Unsupported(()=>StarAdmission.Parse(header+string.Join('\n',Enumerable.Range(0,4097).Select(i=>$"0,0,{i},1,0")))));
            var first=Copy("first");var second=Copy("second",File.ReadAllText(Path.Combine(b,rows[0].GetProperty("path").GetString()!)));
            await E.Run("cache/real-reuse",async()=>{using var cache=new CalculatedStarCache();var a=await cache.GetAsync(first);var next=await cache.GetAsync(first);Eq(StarCalculationState.Calculated,a.State,"First real calculation");Eq("0.22★",a.Result!.Display,"Four-circle reference");E.Check(ReferenceEquals(a,next),"Same cached object returned");Eq(1,cache.CompletedCount,"One cache entry");});
            await E.Run("cache/content-key",async()=>{var copy=Copy("same-bytes");using var cache=new CalculatedStarCache();var a=await cache.GetAsync(first);var other=await cache.GetAsync(copy);Eq(CalculatedStarCache.Key(first),CalculatedStarCache.Key(copy),"Identical content/profile identity across paths");E.Check(ReferenceEquals(a,other),"Identical bytes share completed result");var key=CalculatedStarCache.Key(first)!;Eq(0,key.Mods,"Cache mods");Eq(0,key.Ruleset,"Cache ruleset");E.Check(key.PreparationProfile.Contains("512x384")&&key.CalculatorProfile.Contains("E044-E046"),"Preparation/calculator profiles in cache identity");});
            await E.Run("cache/cancel-before",async()=>{using var cts=new CancellationTokenSource();cts.Cancel();using var cache=new CalculatedStarCache();var a=await cache.GetAsync(first,cts.Token);Eq(StarCalculationState.Cancelled,a.State,"Pre-cancel request");Eq(0,cache.CompletedCount,"No cache entry from cancellation");});
            await E.Run("calculator/cooperative-cancel",()=>{using var cts=new CancellationTokenSource();cts.Cancel();bool stopped=false;try{CircleStarCalculator.Calculate(first,cts.Token);}catch(OperationCanceledException){stopped=true;}E.Check(stopped,"Actual production calculation honors cancellation");Unsupported(()=>new CalculationBudget(maximumSteps:0).Step());});
            await E.Run("cache/coalesce-and-duplicate-cancel",async()=>{var enter=Signal();var release=Signal();int calls=0;using var cache=new CalculatedStarCache(async(c,t)=>{Interlocked.Increment(ref calls);enter.SetResult(true);await release.Task;return CircleStarCalculator.Calculate(c,t);});var a=cache.GetAsync(first);await enter.Task;using var cancel=new CancellationTokenSource();var duplicate=cache.GetAsync(first,cancel.Token);await Until(()=>cache.JoinedPendingRequests==1);cancel.Cancel();Eq(StarCalculationState.Cancelled,(await duplicate).State,"Duplicate caller cancellation");release.SetResult(true);Eq(StarCalculationState.Calculated,(await a).State,"Owner continues");Eq(1,calls,"One actual computation");});
            await E.Run("cache/owner-cancel-then-retry",async()=>{var enter=Signal();var release=Signal();using var cts=new CancellationTokenSource();int calls=0;using var cache=new CalculatedStarCache(async(c,t)=>{if(Interlocked.Increment(ref calls)==1){enter.SetResult(true);await release.Task;}return CircleStarCalculator.Calculate(c,t);});var a=cache.GetAsync(first,cts.Token);await enter.Task;cts.Cancel();Eq(StarCalculationState.Cancelled,(await a).State,"Owner cancellation returned");var retry=cache.GetAsync(first);release.SetResult(true);Eq(StarCalculationState.Calculated,(await retry).State,"Retry creates live job");Eq(2,calls,"Cancelled job not reused");});
            await E.Run("cache/changed-same-size-same-time",async()=>{var c=Copy("changed");using var cache=new CalculatedStarCache();await cache.GetAsync(c);var stamp=File.GetLastWriteTimeUtc(c.Path);File.WriteAllText(c.Path,baseline.Replace("100,192,1000","101,192,1000"));File.SetLastWriteTimeUtc(c.Path,stamp);Eq(c.Fingerprint!.Length,new FileInfo(c.Path).Length,"Same byte length");E.Check(File.ReadAllText(c.Path)!=baseline,"Actual content mutation");Eq(StarCalculationState.Stale,(await cache.GetAsync(c)).State,"Old fingerprint refuses cache");var fresh=LocalChartCatalog.Index(c.Path);E.Check(CalculatedStarCache.Key(fresh)!=CalculatedStarCache.Key(c),"Hash identity changed");Eq(StarCalculationState.Calculated,(await cache.GetAsync(fresh)).State,"New revision calculated");});
            await E.Run("cache/changed-in-flight",async()=>{var c=Copy("inflight");var enter=Signal();var release=Signal();using var cache=new CalculatedStarCache(async(row,t)=>{var r=CircleStarCalculator.Calculate(row,t);enter.SetResult(true);await release.Task;return r;});var pending=cache.GetAsync(c);await enter.Task;File.AppendAllText(c.Path,"\n// changed\n");release.SetResult(true);Eq(StarCalculationState.Stale,(await pending).State,"Post-computation revision recheck");Eq(0,cache.CompletedCount,"Stale job cannot cache");});
            await E.Run("cache/missing-file",async()=>{var c=Copy("deleted");File.Delete(c.Path);using var cache=new CalculatedStarCache();Eq(StarCalculationState.Stale,(await cache.GetAsync(c)).State,"Missing file has no stars");Eq(0,cache.CompletedCount,"Missing cannot cache");});
            await E.Run("selection/real-switch-refresh-return",async()=>{using var cache=new CalculatedStarCache();using var controller=new SelectPlayStarController(cache);var selected=new SelectedChartState();var browser=new SelectPlayModel(selected);browser.Rebuild(new(1,[first,second],[]));browser.MouseSelect(first);controller.Highlight(first,1);await controller.Completion;Eq("0.22★",controller.StateFor(first).Result!.Display,"First rating");browser.MouseSelect(second);controller.Highlight(second,1);await controller.Completion;Eq("0.11★",controller.StateFor(second).Result!.Display,"Second rating");Eq(StarCalculationState.NotCalculated,controller.StateFor(first).State,"Other row cannot inherit rating");browser.MouseSelect(first);controller.Highlight(first,1);await controller.Completion;var saved=controller.StateFor(first).Result;controller.Invalidate();browser.Rebuild(new(2,[first,second],[]));E.Check(browser.Restore(),"Selection restoration after refresh");controller.Highlight(browser.Highlighted,2);await controller.Completion;E.Check(ReferenceEquals(saved,controller.StateFor(first).Result),"Completed calculation reused after refresh");});
            await E.Run("selection/stale-async-result",async()=>{var enter=Signal();var release=Signal();using var cache=new CalculatedStarCache(async(c,t)=>{if(c==first){enter.SetResult(true);await release.Task;}return CircleStarCalculator.Calculate(c,t);});using var controller=new SelectPlayStarController(cache);controller.Highlight(first,1);await enter.Task;var old=controller.Completion;controller.Highlight(second,2);Eq(StarCalculationState.Pending,controller.StateFor(second).State,"New pending reference");release.SetResult(true);await old;await controller.Completion;Eq("0.11★",controller.StateFor(second).Result!.Display,"Late old result did not publish 0.22");Eq(StarCalculationState.NotCalculated,controller.StateFor(first).State,"Old row inactive");controller.Highlight(first,1);Eq("0.11★",controller.StateFor(second).Result!.Display,"Older catalog epoch ignored");});
            await E.Run("selection/dispose-inflight",async()=>{var enter=Signal();var release=Signal();var cache=new CalculatedStarCache(async(c,t)=>{enter.SetResult(true);await release.Task;return CircleStarCalculator.Calculate(c,t);});var controller=new SelectPlayStarController(cache);controller.Highlight(first,1);await enter.Task;int late=0;controller.Changed+=()=>late++;controller.Dispose();cache.Dispose();release.SetResult(true);await controller.Completion;await Until(()=>cache.PendingCount==0);Eq(0,late,"No late disposal notification");Eq(0,cache.CompletedCount,"No disposed cache publication");});
            await E.Run("cache/error-states",async()=>{using var io=new CalculatedStarCache((c,t)=>throw new IOException("QA15 controlled read"));Eq(StarCalculationState.Stale,(await io.GetAsync(first)).State,"IO failure typed stale");using var bug=new CalculatedStarCache((c,t)=>throw new InvalidOperationException("QA15 controlled error"));Eq(StarCalculationState.Failed,(await bug.GetAsync(first)).State,"Calculator failure typed failed");Eq(0,bug.CompletedCount,"Failure not cached");});
            await E.Run("cache/bounded-completed",async()=>{using var cache=new CalculatedStarCache();for(int i=0;i<65;i++)await cache.GetAsync(Copy("bounded"+i,baseline+"\n//"+i));Eq(64,cache.CompletedCount,"Completed cache bound enforced");});
            foreach(string name in new[]{"first-slider.osu","spinner-practice.osu"})await E.Run("unsupported/playable-"+name,async()=>{var chart=LocalChartCatalog.Index(Path.Combine(root,"reconstructed/CircleGame/Fixtures",name));var map=chart.LoadGameplay();using var cache=new CalculatedStarCache();var state=await cache.GetAsync(chart);Eq(StarCalculationState.Unsupported,state.State,"Star profile unsupported");E.Check(state.Result is null&&chart.Available,"Unrated chart remains available");E.Check(map.Sliders.Count+map.Spinners.Count>0,"Actual excluded object loaded");});
            foreach(string path in new[]{first.Path,Path.Combine(root,"reconstructed/CircleGame/Fixtures/first-slider.osu"),Path.Combine(root,"reconstructed/CircleGame/Fixtures/spinner-practice.osu")})
                await E.Run("gameplay/rank-retry-"+Path.GetFileName(path),async()=>{
                    var chart=LocalChartCatalog.Index(path);using var cache=new CalculatedStarCache();await cache.GetAsync(chart);var map=chart.LoadGameplay();var plain=Beatmap.Load(path);
                    var a=CircleSession.CreateFreshOrdinary(plain,ScoreConfiguration.FromBeatmap(plain));var c=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));using var ra=new RankSession(a);using var rb=new RankSession(c);
                    for(int t=0;t<=ra.MaximumEnd+3016;t+=16){a.Advance(t);c.Advance(t);if(t%128==0)E.Check(State(a)==State(c),"Matched gameplay snapshots",State(a),State(c));ra.Observe(t,continuation:t<=ra.MaximumEnd);rb.Observe(t,continuation:t<=rb.MaximumEnd);ra.Complete();rb.Complete();}
                    Eq(ra.HandlerRecord.Score,rb.HandlerRecord.Score,"Rank score preserved");Eq(ra.HandlerRecord.Grade(),rb.HandlerRecord.Grade(),"Rank grade preserved");var retry=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));Eq(0,retry.Ledger.EventCount,"Fresh Retry ledger");E.Check(!ReferenceEquals(retry,c),"Fresh Retry session identity");
                });
        }
        finally{Directory.Delete(temp,true);}
        return E.Finish(root,args.Length>1?args[1]:"");
    }
    static string State(CircleSession s)=>JsonSerializer.Serialize(new{s.Ledger.Score,s.Ledger.N300,s.Ledger.N100,s.Ledger.N50,s.Ledger.NMiss,s.Ledger.CurrentCombo,s.Ledger.MaximumCombo,s.Ledger.AccuracyFraction,s.Ledger.EventCount,s.JudgedCount,s.Finished,health=s.Health is{} h?new{h.State.H,h.State.A,h.State.V,h.Failed}:null});
}
