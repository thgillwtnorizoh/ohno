using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Ohno.CircleGame.Difficulty;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Rendering;
using Ohno.CircleGame.Selection;

internal static class Program
{
    static int groups;static void Yes(bool value){if(!value)throw new Exception("R17 Windows group"+(groups+1));}
    static void Pass(string text)=>Console.WriteLine($"PASS R17 WINDOWS {++groups:00} {text}");
    static IEnumerable<DependencyObject> Walk(DependencyObject o){yield return o;foreach(var x in LogicalTreeHelper.GetChildren(o))if(x is DependencyObject child)foreach(var y in Walk(child))yield return y;}
    static string Details(SelectPlayView v)=>Walk(v).OfType<TextBlock>().Select(x=>x.Text).First(x=>x.Contains("Stars")||x.Contains("★"));
    static void Frame(PlayfieldView v){if(!v.HasRankControls&&v.Rank?.Stage!=RankStage.ChartSelection)throw new Exception("No gameplay frame/input sampling authorized");typeof(PlayfieldView).GetMethod("Frame",BindingFlags.NonPublic|BindingFlags.Instance)!.Invoke(v,[null,EventArgs.Empty]);}
    static void Rank(PlayfieldView v){v.Session!.Press(new(256,192),2000);v.Session.Advance(5000);v.Rank!.Observe(2201);v.Rank.Complete();v.Rank.Observe(5000);Frame(v);Frame(v);Frame(v);Yes(v.Rank.Stage==RankStage.ResultsPresented);}
    [STAThread]static int Main(string[] args)
    {
        string repo=args[0],tmp=Path.Combine(Path.GetTempPath(),"r17-win-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(tmp);
        try
        {
            var chart=LocalChartCatalog.Index(Path.Combine(repo,"re/evidence/46/fixtures/combined-repeat-visibility.osu"));var selection=new SelectedChartState();var browser=new SelectPlayModel(selection);browser.Rebuild(new(1,[chart],[]));using var view=new SelectPlayView(browser);var release=new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);using var cache=new CalculatedStarCache(async(row,token)=>{await release.Task.ConfigureAwait(false);return CircleStarCalculator.Calculate(row,token);});using var stars=new SelectPlayStarController(cache);
            view.CalculatedStars=row=>stars.StateFor(row).Text;view.Bind();Yes(Details(view).Contains("not requested"));Yes(selection.Current is null);Pass("unshown actual browser accepts calculated provenance adapter without committing chart");
            stars.Highlight(chart,1);view.Bind();Yes(Details(view).Contains("pending"));release.SetResult(true);stars.Completion.GetAwaiter().GetResult();view.Bind();Yes(Details(view).Contains("5.11★"));Yes(Details(view).Contains("original-derived restricted calculation"));Yes(!Details(view).Contains("supplied cache"));Pass("actual Windows calculator/cache completion displays correct E0465.11 and calculated provenance");
            string text="osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Metadata]\nTitle:R17 Windows\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n256,192,2000,1,0\n256,192,3000,1,0\n";string p=Path.Combine(tmp,"other.osu");File.WriteAllText(p,text);var other=LocalChartCatalog.Index(p);browser.Rebuild(new(2,[chart,other],[]));browser.Highlight(other);stars.Highlight(other,2);view.Bind();Yes(!Details(view).Contains("5.11★"));stars.Completion.GetAwaiter().GetResult();view.Bind();Yes(Details(view).Contains("0.11★"));Pass("switching difficulty replaces rating with that chart's own result and reference");
            var slider=LocalChartCatalog.Index(Path.Combine(repo,"reconstructed/CircleGame/Fixtures/discrete-slider-r12.osu"));browser.Rebuild(new(3,[slider],[]));browser.Highlight(slider);stars.Highlight(slider,3);stars.Completion.GetAwaiter().GetResult();view.Bind();Yes(Details(view).Contains("unsupported"));Yes(slider.Available);Yes(slider.LoadGameplay().Sliders.Count>0);Pass("unsupported numerical profile remains selectable/playable and receives no calculated number");
            browser.Rebuild(new(4,[other],[]));view.SelectRow(other);stars.Highlight(other,4);stars.Completion.GetAwaiter().GetResult();var old=stars.StateFor(other).Result;File.AppendAllText(p,"\n// changed\n");stars.Invalidate();stars.Highlight(other,5);stars.Completion.GetAwaiter().GetResult();view.Bind();Yes(Details(view).Contains("stale"));Yes(stars.StateFor(other).Result is null);Yes(old is not null);Pass("changed file revision rejects cached rating and exposes stale status");
            var replacement=LocalChartCatalog.Index(p);browser.Rebuild(new(6,[replacement],[]));view.SelectRow(replacement);stars.Highlight(replacement,6);stars.Completion.GetAwaiter().GetResult();view.Bind();Yes(Details(view).Contains("0.11★"));Pass("explicit replacement revision can calculate without changing authoritative selection semantics");
            string single=Path.Combine(tmp,"single.osu");File.WriteAllText(single,text.Replace("256,192,3000,1,0\n",""));var map=Beatmap.Load(single);using var surface=new PlayfieldView();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));var session=surface.Session!;int events=session.Ledger.EventCount;double hp=session.Health!.State.H;view.Bind();Yes(session.Ledger.EventCount==events&&session.Health.State.H==hp&&surface.CurrentTime==0);Pass("metadata binding does not advance gameplay time, health or score");
            Rank(surface);surface.BackFromResults();Frame(surface);Yes(surface.Rank!.Stage==RankStage.ChartSelection);Yes(browser.Restore());Yes(ReferenceEquals(browser.Highlighted,replacement));stars.Highlight(replacement,6);stars.Completion.GetAwaiter().GetResult();view.Bind();Yes(Details(view).Contains("0.11★"));Pass("real unshown Rank Back preserves selected chart and its calculated cache identity");
            surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));Rank(surface);var previous=surface.Session;surface.RetryResults();Frame(surface);Yes(!ReferenceEquals(previous,surface.Session));Yes(surface.Session!.Ledger.EventCount==0);Yes(ReferenceEquals(surface.Session.Map,map));surface.Pause();Pass("existing actual Retry remains fresh and independent of later browser rating selection");
            using var oldView=new SelectPlayView(browser);Yes(Details(oldView).Contains("no compatible cache"));var supplied=StarDisplay.Prepare(replacement,0,0,new("osu-stable:E043:cache-v1","independent export",replacement.Fingerprint!.Sha256,0,0,1.15f));Yes(supplied.Text.StartsWith("1.14★"));Yes(supplied.Text.Contains("supplied cache"));Pass("legacy unconfigured widget and supplied cache compatibility remain semantically separate");
            stars.Dispose();view.Dispose();browser.Highlight(chart);view.Bind();Yes(view.Children.Count==0);surface.Dispose();surface.Dispose();Pass("disposed widget/controller/host safely ignore supported late callbacks; no Window/media opened");
            string host=File.ReadAllText(Path.Combine(repo,"reconstructed/CircleGame/Program.cs"));Yes(host.Contains("browserView.CalculatedStars = chart => stars.StateFor(chart).Text"));Yes(host.Contains("if (!closing && selectionVisible) browserView.Bind()"));Yes(host.Contains("stars.Dispose(); starCache.Dispose()"));Pass("SOURCE INSPECTION GameWindow dispatcher and close wiring; no Window instantiated");
            Console.WriteLine($"R17 WINDOWS: {groups}/12 groups passed; last1 source inspection; other controls unshown components; no Window/show/render/OS input/nonempty media");return 0;
        }
        finally{Directory.Delete(tmp,true);}
    }
}
