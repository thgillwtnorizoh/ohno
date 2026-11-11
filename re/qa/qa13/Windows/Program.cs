using System.IO;
using System.Reflection;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Text.Json;
using System.Security.Cryptography;
using System.IO.Compression;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Rendering;
internal static class Program
{
    static readonly List<object> results=[],observations=[];static int failed;
    static void Eq<T>(T a,T e){observations.Add(new{expected=e,actual=a});if(!EqualityComparer<T>.Default.Equals(a,e))throw new Exception($"expected {e}, actual {a}");}
    static void Run(string id,Action body){observations.Clear();string? error=null;try{body();}catch(Exception e){error=e.ToString();failed++;}results.Add(new{id,classification="EXECUTED WINDOWS COMPONENT",passed=error is null,observations=observations.ToArray(),error});Console.WriteLine($"{(error is null?"PASS":"FAIL")} QA13 WINDOWS {id} {error}");}
    static T Get<T>(object o,string f)=>(T)o.GetType().GetField(f,BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(o)!;
    static void Set(object o,string f,object v)=>o.GetType().GetField(f,BindingFlags.Instance|BindingFlags.NonPublic)!.SetValue(o,v);
    static void Frame(PlayfieldView v){if(!v.HasRankControls&&v.Rank?.Stage!=RankStage.ChartSelection)throw new Exception("QA13 refuses gameplay Frame/input sampling");typeof(PlayfieldView).GetMethod("Frame",BindingFlags.NonPublic|BindingFlags.Instance)!.Invoke(v,[null,EventArgs.Empty]);}
    static void Admitted(PlayfieldView v)
    {v.Session!.Press(new(173,141),1700);v.Session.Advance(4700);v.Rank!.Observe(1901);Eq(v.Rank.Complete(),true);v.Rank.Observe(4700);Eq(v.Rank.Stage,RankStage.RankAdmitted);}
    static void Presented(PlayfieldView v){Admitted(v);Frame(v);Frame(v);Frame(v);Eq(v.Rank!.Stage,RankStage.ResultsPresented);}
    static IEnumerable<T> Desc<T>(DependencyObject o)where T:DependencyObject
    {if(o is T t)yield return t;foreach(var child in LogicalTreeHelper.GetChildren(o).OfType<DependencyObject>())foreach(var c in Desc<T>(child))yield return c;}
    sealed class Backend:ISpinnerSampleChannels
    {
        public List<string> Ops=[];public FrequencyCapability FrequencyCapability=>FrequencyCapability.Unsupported;
        public int Acquire(int h,bool f){Ops.Add($"Acquire:{h}:{f}");return h;}
        public int Activity(int h)=>0;public long? Length(int h,int m)=>null;
        public void Attribute(int h,int a,float v)=>Ops.Add($"Attribute:{h}:{a}");public void Play(int h,bool r)=>Ops.Add($"Play:{h}:{r}");
        public void Pause(int h)=>Ops.Add("Pause:"+h);public void Stop(int h)=>Ops.Add("Stop:"+h);public void Dispose()=>Ops.Add("Dispose");
    }
    [STAThread]static int Main(string[] args)
    {
        string dir=Path.Combine(Path.GetTempPath(),"ohno-qa13-windows-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(dir);
        try
        {
            string file=Path.Combine(dir,"control.osu");File.WriteAllText(file,"osu file format v14\n[General]\nMode:0\n[Metadata]\nTitle:Independent Windows Rank\nVersion:Silent control\nArtist:QA13\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,600,4,1,1,55,1,0\n[HitObjects]\n173,141,1700,1,0,0:0:0:0:\n");
            Beatmap map=Beatmap.Load(file);PlayfieldView Host(){var v=new PlayfieldView();v.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));return v;}
            using var host=Host();int committed=0;host.ResultsCommitted+=()=>committed++;
            Run("W01 fresh actual host alias",()=>{Eq(ReferenceEquals(((SessionRankRecord)host.Rank!.HandlerRecord).Session,host.Session),true);Eq(host.Rank.Stage,RankStage.Active);Eq(host.HasRankControls,false);});
            Run("W02 admitted not presented",()=>{Admitted(host);Eq(host.Rank!.Transitions.Current,RankMode.Play);Eq(host.Rank.Presentation,null);Eq(host.HasRankControls,true);});
            Run("W03 commit cleanup first frame",()=>{Frame(host);Eq(host.Rank!.Stage,RankStage.TransitionCommitted);Eq(host.Rank.SelectedRecord,null);Eq(host.Running,false);Eq(committed,1);});
            Run("W04 initialize second frame",()=>{Frame(host);Eq(host.Rank!.Stage,RankStage.ResultsInitialized);Eq(ReferenceEquals(host.Rank.SelectedRecord,host.Rank.HandlerRecord),true);Eq(host.Rank.Presentation!.Map.Title,"Independent Windows Rank");});
            Run("W05 third frame permits view",()=>{Frame(host);Eq(host.Rank!.Stage,RankStage.ResultsPresented);Eq(committed,1);});
            Run("W06 results frames freeze gameplay",()=>{int score=host.Session!.Ledger.Score,events=host.Session.Ledger.EventCount,time=host.CurrentTime;double h=host.Session.Health!.State.H;for(int i=0;i<8;i++)Frame(host);Eq(host.CurrentTime,time);Eq(host.Session.Ledger.Score,score);Eq(host.Session.Ledger.EventCount,events);Eq(host.Session.Health.State.H,h);Eq(host.Running,false);});
            var view=new RankResultsView();
            Run("W07 unshown view initially collapsed",()=>Eq(view.Visibility,Visibility.Collapsed));
            Run("W08 actual presentation binds independently",()=>{view.Bind(host.Rank!.Presentation,null,"········");Eq(view.Visibility,Visibility.Visible);Eq(view.Children.Count,1);Eq(Desc<TextBlock>(view).Any(t=>t.Text=="X"),true);Eq(Desc<TextBlock>(view).Any(t=>t.Text=="100.00%"||t.Text=="Accuracy  100.00%"),true);});
            Run("W09 same reference reveal updates",()=>{var p=host.Rank!.Presentation!;view.Bind(p,null,p.Score);Eq(Desc<TextBlock>(view).Any(t=>t.Text==p.Score),true);Eq(view.Children.Count,1);});
            Run("W10 hide and rebind same score widget",()=>{view.Hide();Eq(view.Children.Count,0);view.Bind(host.Rank!.Presentation,null,"12345678");Eq(view.Children.Count,1);Eq(Desc<TextBlock>(view).Any(t=>t.Text=="12345678"),true);});
            Run("W11 replacement presentation frozen fields",()=>{var p=host.Rank!.Presentation! with{Score="87654321",Valid=false,Perfect=null,Map=new("Second title",null,null)};view.Bind(p,null,p.Score);Eq(Desc<TextBlock>(view).Any(t=>t.Text=="Second title"),true);Eq(Desc<TextBlock>(view).Any(t=>t.Text=="INVALID"),true);Eq(Desc<TextBlock>(view).Any(t=>t.Text.Contains("Perfect decoration unavailable")),true);});
            Run("W12 unavailable view no invented grade",()=>{view.Bind(null,"QA13 unsupported","");Eq(Desc<TextBlock>(view).Any(t=>t.Text=="RESULTS UNAVAILABLE"),true);Eq(Desc<TextBlock>(view).Any(t=>t.Text=="X"),false);Eq(Desc<Button>(view).Count(),2);});
            Run("W13 supplied callbacks component event only",()=>{int back=0,retry=0,skip=0;view.BackRequested+=()=>back++;view.RetryRequested+=()=>retry++;view.SkipRequested+=()=>skip++;view.Bind(host.Rank!.Presentation,null,host.Rank.Presentation!.Score);foreach(var button in Desc<Button>(view))button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));Eq(back,1);Eq(retry,1);Eq(skip,1);});
            Run("W14 host reveal skip no restart",()=>{var old=host.Session;host.SkipResultsReveal();host.SkipResultsReveal();Eq(ReferenceEquals(old,host.Session),true);Eq(host.Rank!.Transitions.Pending,null);});
            Run("W15 held buttons suppressed in Rank",()=>{host.SetHeldButtons(3);Eq(host.HasRankControls,true);Eq(Get<int>(Get<object>(host.Session!,"input"),"current"),0);Eq(host.Session!.Ledger.EventCount,1);});
            Run("W16 actual Back commits selection",()=>{host.BackFromResults();Eq(host.Rank!.Transitions.Pending,RankMode.SelectPlay);Frame(host);Eq(host.Rank.Stage,RankStage.ChartSelection);Eq(host.Running,false);Eq(host.Rank.GlobalRecord,null);});
            using var retryHost=Host();retryHost.SetHeldButtons(3);Presented(retryHost);var previous=retryHost.Session!;var oldRank=retryHost.Rank!;
            Run("W17 rejected Retry creates no session",()=>{retryHost.Rank!.Transitions.Allow=false;retryHost.RetryResults();Eq(retryHost.Rank.Transitions.Pending,null);Eq(ReferenceEquals(previous,retryHost.Session),true);retryHost.Rank.Transitions.Allow=true;});
            Run("W18 actual fresh Retry",()=>{retryHost.RetryResults();retryHost.RetryResults();Eq(retryHost.Rank!.Transitions.Pending,RankMode.Play);Frame(retryHost);Eq(ReferenceEquals(previous,retryHost.Session),false);Eq(ReferenceEquals(previous.Ledger,retryHost.Session!.Ledger),false);Eq(ReferenceEquals(previous.Health,retryHost.Session.Health),false);Eq(retryHost.Session.Ledger.EventCount,0);Eq(retryHost.Session.Circles[0].Result,Judgement.Pending);var input=Get<object>(retryHost.Session,"input");foreach(string f in new[]{"current","previous","older"})Eq(Get<int>(input,f),0);Eq(Get<bool>(input,"leftNew"),false);Eq(Get<bool>(input,"rightNew"),false);Eq(retryHost.Rank.Stage,RankStage.Active);Eq(oldRank.Stage,RankStage.Disposed);retryHost.Pause();});
            Run("W19 map replacement discards Rank",()=>{var old=host.Rank!;string other=Path.Combine(dir,"replacement.osu");File.WriteAllText(other,File.ReadAllText(file).Replace("Independent Windows Rank","Replacement map").Replace("173,141,1700","311,217,2700"));var next=Beatmap.Load(other);host.Load(next,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(next));Eq(old.Stage,RankStage.Disposed);Eq(host.Rank!.Stage,RankStage.Active);Eq(ReferenceEquals(map,host.Session!.Map),false);Eq(host.Session.Map.Title,"Replacement map");Eq(host.Session.Ledger.Score,0);Eq(host.HasRankControls,false);});
            Run("W20 repeat host disposal",()=>{host.Dispose();host.Dispose();Eq(host.Rank!.Stage,RankStage.Disposed);Eq(host.Running,false);});
            using var lib=new HitSoundLibrary(new());var prepared=lib.PrepareAsync(map,CancellationToken.None).GetAwaiter().GetResult();Eq(prepared.Samples.Count,0);
            var ctor=typeof(WindowsCircleEffects).GetConstructors(BindingFlags.NonPublic|BindingFlags.Instance).Single();using var effects=(WindowsCircleEffects)ctor.Invoke([prepared,new HitSoundSettings(),Dispatcher.CurrentDispatcher]);
            var backend=new Backend();var shared=new SharedSliderChannels(new GlobalSampleTiming(prepared.Source.Timing),backend,(_,_,_)=>0);var spinner=shared.AttachSpinner(backend,()=>0);spinner.Channel=911;shared.Channel0=101;shared.Channel1=202;
            Set(effects,"continuing",shared);Set(effects,"spinnerChannel",spinner);
            using var fxHost=new PlayfieldView();fxHost.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map),effects);effects.Resume();
            Run("W21 effects empty resource preparation",()=>{Eq(prepared.Samples.Count,0);Eq(effects.Status.Contains("unavailable",StringComparison.OrdinalIgnoreCase)||effects.Status.Contains("0"),true);});
            Run("W22 pending shared starts cancelled at results",()=>{shared.Submit(2,1,1,true,256f);spinner.Continue(null);Eq(shared.Pending>0,true);shared.QueuePause();int before=Get<int>(effects,"loopGeneration");Admitted(fxHost);Frame(fxHost);Eq(shared.Pending,0);Eq(Get<int>(effects,"loopGeneration")>before,true);backend.Ops.Clear();shared.Drain();Eq(backend.Ops.Count,0);Eq(fxHost.Running,false);});
            Run("W23 no duplicate score through effects cleanup",()=>{int n=fxHost.Session!.Ledger.EventCount;Frame(fxHost);Frame(fxHost);Frame(fxHost);Eq(fxHost.Session.Ledger.EventCount,n);Eq(fxHost.Session.Ledger.N300,(ushort)1);});
            Run("W24 disposed effects remain bounded",()=>{fxHost.Dispose();effects.Dispose();effects.Dispose();Eq(shared.Pending,0);Eq(fxHost.Rank!.Stage,RankStage.Disposed);});
        }
        finally{Directory.Delete(dir,true);}
        byte[] bytes=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA13.Windows.v1",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-preflight",named=results.Count,passed=results.Count-failed,failed,cases=results,limit="Unshown components; direct callback/routed component events, no physical input, no gameplay Frame sampling, no Window.Show, no media Open or playback"},new JsonSerializerOptions{WriteIndented=true});File.WriteAllBytes(args[0],bytes);Console.WriteLine($"QA13 WINDOWS: {results.Count-failed} passed, {failed} failed, {results.Count} named controls");Console.WriteLine("QA13 WINDOWS SHA256 "+Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant());using var ms=new MemoryStream();using(var gz=new GZipStream(ms,CompressionLevel.SmallestSize,true))gz.Write(bytes);Console.WriteLine("QA13 WINDOWS EVIDENCE "+Convert.ToBase64String(ms.ToArray()));return failed==0?0:1;
    }
}
