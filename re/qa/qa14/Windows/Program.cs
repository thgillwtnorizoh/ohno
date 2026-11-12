using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Rendering;
using Ohno.CircleGame.Selection;

namespace Ohno.QA14.Windows;
internal static class Program
{
    static readonly List<object> observations=[],results=[];
    static string root="",scratch="",work="";
    static int failed;
    static string Hash(byte[] x)=>Convert.ToHexString(SHA256.HashData(x)).ToLowerInvariant();
    static void Eq<T>(T actual,T expected,string label=""){observations.Add(new{label,expected,actual});if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception(label+" expected="+expected+" actual="+actual);}
    static void Same(object? a,object? b)=>Eq(ReferenceEquals(a,b),true,"reference identity");
    static T Get<T>(object o,string name)=>(T)o.GetType().GetField(name,BindingFlags.NonPublic|BindingFlags.Instance)!.GetValue(o)!;
    static IEnumerable<T> Desc<T>(DependencyObject obj) where T:DependencyObject {if(obj is T t)yield return t;foreach(var child in LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>())foreach(var item in Desc<T>(child))yield return item;}
    static string Text(string version="A")=>$"osu file format v14\n[General]\nMode:0\n[Metadata]\nTitle:QA14 Windows\nArtist:Work1\nVersion:{version}\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,600,4,1,1,55,1,0\n[HitObjects]\n173,141,1700,1,2,0:0:0:0:\n";
    static CatalogChart Chart(string name="s/a.osu",string? text=null){string p=Path.Combine(work,name);Directory.CreateDirectory(Path.GetDirectoryName(p)!);File.WriteAllText(p,text??Text());return LocalChartCatalog.Index(p);}
    static SelectPlayModel Model(params CatalogChart[] charts){var m=new SelectPlayModel(new());m.Rebuild(new(1,charts,[]));return m;}
    static Button Button(SelectPlayView view,string label)=>Desc<Button>(view).Single(b=>b.Content is string s&&s.StartsWith(label,StringComparison.Ordinal));
    static void Click(Button button)=>button.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
    static void Run(string id,Action body)
    {
        work=Path.Combine(scratch,id);Directory.CreateDirectory(work);observations.Clear();string? error=null;try{body();}catch(Exception e){error=e.ToString();failed++;}
        results.Add(new{id,classification="EXECUTED WINDOWS COMPONENT",authority="E043 reference/selection/Rank model plus R16 declared unshown WPF engineering contract",passed=error is null,observations=observations.ToArray(),error});Console.WriteLine($"{(error is null?"PASS":"FAIL")} QA14 WINDOWS {id} {error}");
    }
    static void FrameResults(PlayfieldView v)
    {
        if(!v.HasRankControls&&v.Rank?.Stage!=RankStage.ChartSelection)throw new InvalidOperationException("QA14 refuses gameplay Frame/input sampling");
        typeof(PlayfieldView).GetMethod("Frame",BindingFlags.NonPublic|BindingFlags.Instance)!.Invoke(v,[null,EventArgs.Empty]);
    }
    static void Rank(PlayfieldView v)
    {
        v.Session!.Press(new(173,141),1700);v.Session.Advance(4700);v.Rank!.Observe(1901);Eq(v.Rank.Complete(),true);v.Rank.Observe(4700);Eq(v.Rank.Stage,RankStage.RankAdmitted);FrameResults(v);FrameResults(v);FrameResults(v);Eq(v.Rank.Stage,RankStage.ResultsPresented);
    }
    [STAThread] static int Main(string[] args)
    {
        root=Path.GetFullPath(args[0]);scratch=Path.Combine(Path.GetTempPath(),"ohno-qa14-windows-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        Run("W01-construction",()=>{var m=Model(Chart());using var v=new SelectPlayView(m);Eq(Desc<ListBox>(v).Count(),2);Eq(Desc<Button>(v).Count(),4);Eq(m.Selection.Current,null);Eq(m.Committed,null);Eq(v.RowDefinitions.Count,3);});
        Run("W02-empty",()=>{var m=Model();using var v=new SelectPlayView(m);Eq(Get<ListBox>(v,"songs").Items.Count,0);Eq(Get<Button>(v,"play").IsEnabled,false);Eq(Desc<TextBlock>(v).Any(t=>t.Text.Contains("No highlighted difficulty")),true);});
        Run("W03-song-difficulty-binding",()=>{var a=Chart("s/a.osu");var b=Chart("s/b.osu",Text("B"));var c=Chart("t/c.osu",Text("C"));var m=Model(a,b,c);using var v=new SelectPlayView(m);Eq(Get<ListBox>(v,"songs").Items.Count,2);Eq(Get<ListBox>(v,"difficulties").Items.Count,2);Same(Get<ListBox>(v,"difficulties").SelectedItem,a);});
        Run("W04-highlight-versus-selected-label",()=>{var a=Chart("s/a.osu");var b=Chart("t/b.osu",Text("B"));var m=Model(a,b);m.Commit();using var v=new SelectPlayView(m);m.Highlight(b);Same(Get<ListBox>(v,"difficulties").SelectedItem,b);Same(m.Selection.Current,a);Eq(Get<Button>(v,"play").Content,"Commit difficulty");string text=Get<TextBlock>(v,"state").Text;Eq(text.Contains("Highlight: B"),true);Eq(text.Contains("Selected: a.osu"),true);});
        Run("W05-commit-button",()=>{var a=Chart();var m=Model(a);int starts=0;m.StartRequested+=()=>starts++;using var v=new SelectPlayView(m);Click(Get<Button>(v,"play"));Same(m.Selection.Current,a);Eq(starts,0);Eq(Get<Button>(v,"play").Content,"Play selected");});
        Run("W06-play-button",()=>{var m=Model(Chart());m.Commit();int starts=0;m.StartRequested+=()=>starts++;using var v=new SelectPlayView(m);Click(Get<Button>(v,"play"));Eq(starts,1);});
        Run("W07-row-first-repeat",()=>{var a=Chart();var m=Model(a);int n=0;m.StartRequested+=()=>n++;using var v=new SelectPlayView(m);v.SelectRow(a);Eq(n,0);Same(m.Selection.Current,a);v.SelectRow(a);Eq(n,1);});
        Run("W08-refresh-callback",()=>{using var v=new SelectPlayView(Model());int n=0;v.RefreshRequested+=()=>n++;Click(Button(v,"Refresh"));Eq(n,1);});
        Run("W09-open-callback-no-dialog",()=>{using var v=new SelectPlayView(Model());int n=0;v.OpenRequested+=()=>n++;Click(Button(v,"Open"));Eq(n,1);});
        Run("W10-back-callback",()=>{using var v=new SelectPlayView(Model());int n=0;v.BackRequested+=()=>n++;Click(Button(v,"Back"));Eq(n,1);});
        Run("W11-status",()=>{using var v=new SelectPlayView(Model());v.SetStatus("independent status");Eq(Get<TextBlock>(v,"status").Text,"independent status");});
        Run("W12-unavailable-row",()=>{var a=Chart(text:"invalid");var m=Model(a);using var v=new SelectPlayView(m);v.SelectRow(a);Eq(Get<Button>(v,"play").IsEnabled,false);Eq(m.Selection.Current,null);Eq(((SongRun)Get<ListBox>(v,"songs").Items[0]).Id,-1);});
        Run("W13-no-star-fabrication",()=>{using var v=new SelectPlayView(Model(Chart()));Eq(Get<TextBlock>(v,"detail").Text.Contains("Stars not calculated · no compatible cache"),true);});
        Run("W14-same-folder-list-event",()=>{var a=Chart("s/a.osu");var b=Chart("s/b.osu",Text("B"));var m=Model(a,b);m.Commit();using var v=new SelectPlayView(m);Get<ListBox>(v,"difficulties").SelectedItem=b;Same(m.Selection.Current,b);Same(m.Committed,b);});
        Run("W15-callback-error-visible",()=>{var m=Model(Chart());m.SelectionCallback=_=>throw new IOException("controlled callback");using var v=new SelectPlayView(m);v.Confirm();Eq(Get<TextBlock>(v,"state").Text.Contains("controlled callback"),true);Eq(m.Selection.Current,null);});
        Run("W16-stale-generation",()=>{var a=Chart("s/a.osu");var b=Chart("t/b.osu");var m=Model(a);using var v=new SelectPlayView(m);m.Rebuild(new(5,[b],[]));Eq(m.Rebuild(new(4,[a],[])),false);Same(Get<ListBox>(v,"difficulties").SelectedItem,b);Eq(m.CatalogGeneration,5L);});
        Run("W17-disposed-supported-callbacks",()=>{var a=Chart();var m=Model(a);var v=new SelectPlayView(m);int starts=0;m.StartRequested+=()=>starts++;v.Dispose();v.Dispose();v.SelectRow(a);v.Confirm();Eq(m.Selection.Revision,0);Eq(starts,0);Eq(v.Children.Count,0);});
        Run("W18-model-event-unregistered",()=>{var a=Chart("s/a.osu");var b=Chart("t/b.osu");var m=Model(a,b);var v=new SelectPlayView(m);var oldText=Get<TextBlock>(v,"state");string before=oldText.Text;v.Dispose();m.Highlight(b);m.Commit();Eq(oldText.Text,before);Eq(v.Children.Count,0);});
        Run("W19-rebind-no-feedback-loop",()=>{var a=Chart();var m=Model(a);using var v=new SelectPlayView(m);int starts=0;m.StartRequested+=()=>starts++;v.Bind();v.Bind();v.Bind();Eq(m.Selection.Revision,0);Eq(starts,0);});
        Run("W20-retained-reference-group",()=>{var a=Chart("s/a.osu");var b=Chart("t/b.osu");var m=Model(a,b);m.Highlight(b);m.Commit();using var v=new SelectPlayView(m);m.Rebuild(new(2,[a,b],[]));Same(Get<ListBox>(v,"difficulties").SelectedItem,b);Eq(((SongRun)Get<ListBox>(v,"songs").SelectedItem).Folder,b.Folder);});
        Run("W21-missing-reference-recovery",()=>{var a=Chart("s/a.osu");var b=Chart("t/b.osu");var m=Model(a);m.Commit();using var v=new SelectPlayView(m);m.Rebuild(new(2,[b],[]));Eq(m.Highlighted,null);Eq(Get<Button>(v,"play").IsEnabled,false);Eq(Get<TextBlock>(v,"state").Text.Contains("missing, changed or unavailable"),true);Same(m.Selection.Current,a);});
        Run("W22-new-revision-explicit-commit",()=>{var a=Chart();var m=Model(a);m.Commit();using var v=new SelectPlayView(m);File.AppendAllText(a.Path,"\n// new revision");var b=LocalChartCatalog.Index(a.Path);m.Rebuild(new(2,[b],[]));Same(m.Selection.Current,a);Eq(m.Committed,null);v.SelectRow(b);Same(m.Selection.Current,b);});
        Run("W23-browser-does-not-update-play",()=>{var a=Chart();var map=a.LoadGameplay();using var surface=new PlayfieldView();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));var s=surface.Session!;var m=Model(a);using var v=new SelectPlayView(m);v.SelectRow(a);v.Bind();Eq(s.Ledger.EventCount,0);Eq(surface.CurrentTime,0);Eq(s.Health!.ResultEvents,0);Same(surface.Session,s);});
        Run("W24-real-rank-back",()=>{var a=Chart();var map=a.LoadGameplay();var m=Model(a);m.Commit();using var v=new SelectPlayView(m);using var surface=new PlayfieldView();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));Rank(surface);int score=surface.Session!.Ledger.Score;surface.BackFromResults();FrameResults(surface);Eq(surface.Rank!.Stage,RankStage.ChartSelection);Eq(m.Restore(),true);Same(m.Highlighted,a);Eq(surface.Session.Ledger.Score,score);});
        Run("W25-real-fresh-retry-captured-map",()=>{var a=Chart("s/a.osu");var b=Chart("t/b.osu");var map=a.LoadGameplay();var m=Model(a,b);m.Commit();using var surface=new PlayfieldView();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));Rank(surface);var old=surface.Session;var oldHp=old!.Health;m.Highlight(b);m.Commit();surface.RetryResults();FrameResults(surface);Eq(ReferenceEquals(surface.Session,old),false);Same(surface.Session!.Map,map);Same(m.Selection.Current,b);Eq(surface.Session.Ledger.Score,0);Eq(surface.Session.Ledger.CurrentCombo,0);Eq(surface.Session.Ledger.EventCount,0);Eq(ReferenceEquals(surface.Session.Health,oldHp),false);surface.Pause();});
        Run("W26-final-validation-retains-host",()=>{var a=Chart();var map=a.LoadGameplay();using var surface=new PlayfieldView();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));var retained=surface.Session;var state=new SelectedChartState();state.Set(a);using var f=new PlayEntryFlow(state);f.Request();f.Accept();f.Queue();f.Commit();f.Capture();long ticket=f.BeginLoading();_ = f.Captured!.LoadGameplay();File.AppendAllText(a.Path,"\n// changed after parse");bool rejected=false;try{a.Verify();}catch(InvalidDataException e){rejected=true;f.Fail(ticket,e.Message);}Eq(rejected,true);Eq(f.Stage,PlayEntryStage.Failed);Same(surface.Session,retained);});
        Run("W27-missing-required-audio",()=>{var a=Chart(text:Text().Replace("Mode:0","Mode:0\nAudioFilename:missing.wav"));var map=a.LoadGameplay();bool rejected=false;try{using var audio=new BeatmapAudio(map.AudioPath);}catch(FileNotFoundException){rejected=true;}Eq(rejected,true);});
        Run("W28-empty-effects",()=>{var a=Chart();var map=a.LoadGameplay();using var library=new HitSoundLibrary(new HitSoundSettings{Enabled=false});using var effects=WindowsCircleEffects.PrepareAsync(map,library,Dispatcher.CurrentDispatcher,CancellationToken.None).GetAwaiter().GetResult();Eq(effects.Status.Contains("no playable effects"),true);effects.Pause();effects.Enabled=false;effects.Dispose();effects.Dispose();Eq(effects.DiagnosticsJson.Contains("disposed"),true);});
        Run("W29-results-do-not-score",()=>{var map=Chart().LoadGameplay();using var s=new PlayfieldView();s.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));Rank(s);int count=s.Session!.Ledger.EventCount,score=s.Session.Ledger.Score;double hp=s.Session.Health!.State.H;int time=s.CurrentTime;s.SkipResultsReveal();FrameResults(s);FrameResults(s);Eq(s.Session.Ledger.EventCount,count);Eq(s.Session.Ledger.Score,score);Eq(s.Session.Health.State.H,hp);Eq(s.CurrentTime,time);});
        Run("W30-bound-detail-metadata",()=>{var a=Chart();using var v=new SelectPlayView(Model(a));string detail=Get<TextBlock>(v,"detail").Text;Eq(detail.Contains("QA14 Windows"),true);Eq(detail.Contains("Creator: unavailable"),true);Eq(detail.Contains("full gameplay validation"),true);Eq(detail.Contains(a.Path),true);});
        var sources=Directory.GetFiles(Path.Combine(root,"reconstructed/CircleGame"),"*.cs",SearchOption.AllDirectories).Where(p=>!p.Contains(Path.DirectorySeparatorChar+"obj"+Path.DirectorySeparatorChar)&&!p.Contains(Path.DirectorySeparatorChar+"bin"+Path.DirectorySeparatorChar)).Concat(Directory.GetFiles(Path.Combine(root,"re/qa/qa14/Windows"),"*.cs"));
        var manifest=sources.ToDictionary(p=>Path.GetRelativePath(root,p).Replace('\\','/'),p=>Hash(File.ReadAllBytes(p)));
        byte[] data=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA14.windows.v1",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-preflight",production="19bb644e01c646ca8850f0751ec03f7ef881d766",runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,named=results.Count,passed=results.Count-failed,failed,cases=results,manifest},new JsonSerializerOptions{WriteIndented=true});
        File.WriteAllBytes(args[1],data);Console.WriteLine($"QA14 WINDOWS: {results.Count-failed} passed, {failed} failed, {results.Count} named controls; unshown components only");Console.WriteLine("QA14 WINDOWS SHA256 "+Hash(data));using var stream=new MemoryStream();using(var gzip=new GZipStream(stream,CompressionLevel.SmallestSize,true))gzip.Write(data);Console.WriteLine("QA14 WINDOWS EVIDENCE "+Convert.ToBase64String(stream.ToArray()));
        if(!Path.GetFullPath(scratch).StartsWith(Path.GetFullPath(Path.GetTempPath()),StringComparison.OrdinalIgnoreCase))throw new Exception("Unsafe test cleanup");Directory.Delete(scratch,true);return failed==0?0:1;
    }
}
