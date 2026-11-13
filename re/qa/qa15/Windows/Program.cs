using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Ohno.CircleGame.Difficulty;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Rendering;
using Ohno.CircleGame.Selection;

internal static class Program
{
    static readonly Evidence E=new("QA15 WINDOWS");
    static IEnumerable<DependencyObject> Tree(DependencyObject x){yield return x;foreach(var y in LogicalTreeHelper.GetChildren(x))if(y is DependencyObject d)foreach(var z in Tree(d))yield return z;}
    static string Detail(SelectPlayView view)=>Tree(view).OfType<TextBlock>().Select(x=>x.Text).First(x=>x.Contains("Stars")||x.Contains("★"));
    static void Run(string id,Action body,string classification="EXECUTED UNSHOWN WINDOWS COMPONENT")=>E.Run(id,body,classification).GetAwaiter().GetResult();
    static void FrameAtRank(PlayfieldView view)
    {
        E.Check(view.HasRankControls||view.Rank?.Stage==RankStage.ChartSelection,"Frame restricted to Rank; no gameplay OS sampling");
        typeof(PlayfieldView).GetMethod("Frame",BindingFlags.NonPublic|BindingFlags.Instance)!.Invoke(view,[null,EventArgs.Empty]);
    }
    static void FinishRank(PlayfieldView v){v.Session!.Press(new(256,192),2000);v.Session.Advance(5000);v.Rank!.Observe(2201);v.Rank.Complete();v.Rank.Observe(5000);FrameAtRank(v);FrameAtRank(v);FrameAtRank(v);E.Check(v.Rank.Stage==RankStage.ResultsPresented,"Unshown Rank reached");}
    [STAThread]static int Main(string[] args)
    {
        string root=args[0],qa=Path.Combine(root,"re/qa/qa15"),temp=Path.Combine(Path.GetTempPath(),"qa15-window-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        using var m=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(qa,"beatmaps/manifest.json")));var rows=m.RootElement.GetProperty("files").EnumerateArray().ToArray();
        var charts=rows.Select(x=>LocalChartCatalog.Index(Path.Combine(qa,"beatmaps",x.GetProperty("path").GetString()!))).ToArray();
        var selected=new SelectedChartState();var browser=new SelectPlayModel(selected);browser.Rebuild(new(1,charts,[]));using var view=new SelectPlayView(browser);using var cache=new CalculatedStarCache();using var stars=new SelectPlayStarController(cache);
        view.CalculatedStars=row=>stars.StateFor(row).Text;
        try
        {
            for(int i=0;i<rows.Length;i++)
            {
                int n=i;Run("binding/"+rows[n].GetProperty("id").GetString(),()=>{
                    var chart=charts[n];browser.Highlight(chart);stars.Highlight(chart,1);stars.Completion.GetAwaiter().GetResult();view.Bind();string expected=rows[n].GetProperty("expected_r17_display").GetString()!,actual=Detail(view);
                    E.Check(actual.Contains(expected+" · original-derived restricted calculation"),"Actual production view text for independently verified packaged bytes",expected,actual);
                    E.Check(actual.Contains(chart.Path),"Displayed path belongs to highlighted file",chart.Path,actual);
                    E.Check(stars.StateFor(chart).State==StarCalculationState.Calculated,"Actual cache/controller calculation state",StarCalculationState.Calculated,stars.StateFor(chart).State);
                });
            }
            Run("binding/pending-real-completion",()=>{var gate=new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);using var pendingCache=new CalculatedStarCache(async(c,t)=>{await gate.Task;return CircleStarCalculator.Calculate(c,t);});using var pending=new SelectPlayStarController(pendingCache);using var v=new SelectPlayView(browser);v.CalculatedStars=c=>pending.StateFor(c).Text;browser.Highlight(charts[0]);pending.Highlight(charts[0],2);v.Bind();E.Check(Detail(v).Contains("pending")&&!Detail(v).Contains("★"),"Pending text contains no invented rating");gate.SetResult(true);pending.Completion.GetAwaiter().GetResult();v.Bind();E.Check(Detail(v).Contains("0.11★"),"Pending becomes actual calculation", "0.11★",Detail(v));});
            Run("binding/return-cache-refresh",()=>{view.SelectRow(charts[0]);stars.Highlight(charts[0],1);stars.Completion.GetAwaiter().GetResult();var previous=stars.StateFor(charts[0]).Result;browser.Highlight(charts[1]);stars.Highlight(charts[1],1);stars.Completion.GetAwaiter().GetResult();view.SelectRow(charts[0]);stars.Highlight(charts[0],1);stars.Completion.GetAwaiter().GetResult();E.Check(ReferenceEquals(previous,stars.StateFor(charts[0]).Result),"Return reuses real completed result");stars.Invalidate();browser.Rebuild(new(2,charts,[]));E.Check(browser.Restore(),"Real model restores selected row");stars.Highlight(browser.Highlighted,2);stars.Completion.GetAwaiter().GetResult();view.Bind();E.Check(Detail(view).Contains("0.11★"),"Refresh restores correct rating", "0.11★",Detail(view));});
            Run("binding/missing-changed-recovery",()=>{string path=Path.Combine(temp,"revision.osu");File.Copy(charts[2].Path,path);var old=LocalChartCatalog.Index(path);browser.Rebuild(new(3,[old],[]));view.SelectRow(old);stars.Highlight(old,3);stars.Completion.GetAwaiter().GetResult();File.AppendAllText(path,"\n// revision\n");stars.Invalidate();stars.Highlight(old,4);stars.Completion.GetAwaiter().GetResult();view.Bind();E.Check(Detail(view).Contains("stale")&&!Detail(view).Contains("★"),"Changed file cannot show old rating");var fresh=LocalChartCatalog.Index(path);browser.Rebuild(new(5,[fresh],[]));E.Check(browser.Highlighted is null&&browser.Recovery is not null,"Explicit commit needed after revision change");view.SelectRow(fresh);stars.Highlight(fresh,5);stars.Completion.GetAwaiter().GetResult();view.Bind();E.Check(Detail(view).Contains("0.22★"),"Fresh revision calculated after explicit commit");File.Delete(path);stars.Invalidate();stars.Highlight(fresh,6);stars.Completion.GetAwaiter().GetResult();view.Bind();E.Check(Detail(view).Contains("stale"),"Missing-file rating recovery is nonnumeric");});
            foreach(string name in new[]{"first-slider.osu","spinner-practice.osu"})Run("binding/unsupported-"+name,()=>{var chart=LocalChartCatalog.Index(Path.Combine(root,"reconstructed/CircleGame/Fixtures",name));browser.Rebuild(new(browser.CatalogGeneration+1,[chart],[]));view.SelectRow(chart);stars.Highlight(chart,browser.CatalogGeneration);stars.Completion.GetAwaiter().GetResult();view.Bind();E.Check(Detail(view).Contains("unsupported")&&!Detail(view).Contains("★"),"Actual unshown view explicitly unrated", "unsupported",Detail(view));E.Check(chart.Available&&chart.LoadGameplay().Circles.Count+chart.LoadGameplay().Sliders.Count+chart.LoadGameplay().Spinners.Count>0,"Unsupported difficulty stays playable");});
            string single=Path.Combine(temp,"rank.osu");File.WriteAllText(single,"osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Metadata]\nTitle:QA15 unshown Rank control\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n256,192,2000,1,0\n");var map=Beatmap.Load(single);
            Run("rank/back-to-rated-selection",()=>{browser.Rebuild(new(10,charts,[]));view.SelectRow(charts[2]);stars.Highlight(charts[2],10);stars.Completion.GetAwaiter().GetResult();using var surface=new PlayfieldView();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));int events=surface.Session!.Ledger.EventCount;view.Bind();E.Check(surface.Session.Ledger.EventCount==events&&surface.CurrentTime==0,"Star binding does not mutate gameplay time or score");FinishRank(surface);surface.BackFromResults();FrameAtRank(surface);E.Check(surface.Rank!.Stage==RankStage.ChartSelection,"Actual Rank Back transitions");E.Check(browser.Restore(),"Actual selected chart restored");stars.Highlight(browser.Highlighted,10);stars.Completion.GetAwaiter().GetResult();view.Bind();E.Check(Detail(view).Contains("0.22★"),"Rank return retains four-circle rating");});
            Run("rank/fresh-retry",()=>{using var surface=new PlayfieldView();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));FinishRank(surface);var old=surface.Session;surface.RetryResults();FrameAtRank(surface);E.Check(!ReferenceEquals(old,surface.Session)&&ReferenceEquals(surface.Session!.Map,map),"Actual host Retry creates fresh session for captured map");E.Check(surface.Session!.Ledger.EventCount==0,"Actual Retry score ledger reset",0,surface.Session.Ledger.EventCount);surface.Pause();});
            Run("binding/supplied-cache-separation",()=>{using var legacy=new SelectPlayView(browser);E.Check(Detail(legacy).Contains("no compatible cache"),"Unconfigured view retains supplied-cache contract");var chart=charts[0];var supplied=StarDisplay.Prepare(chart,0,0,new("osu-stable:E043:cache-v1","QA15 independent supplied operand",chart.Fingerprint!.Sha256,0,0,1.15f));E.Check(supplied.Text.Contains("1.14★")&&supplied.Text.Contains("supplied cache"),"Supplied Single floor remains distinct", "1.14★ supplied cache",supplied.Text);});
            string host=File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Program.cs"));
            Run("source/dispatcher-binding",()=>E.Check(host.Contains("browserView.CalculatedStars = chart => stars.StateFor(chart).Text")&&host.Contains("if (!closing && selectionVisible) browserView.Bind()"),"SOURCE ONLY actual GameWindow dispatcher binding"),"SOURCE INSPECTION");
            Run("source/selection-generation",()=>E.Check(host.Contains("stars.Highlight(browser.Highlighted, browser.CatalogGeneration)")&&host.Contains("stars.Invalidate();"),"SOURCE ONLY highlight/catalog invalidation wiring"),"SOURCE INSPECTION");
            Run("source/captured-play-and-retry",()=>E.Check(host.Contains("entry.Capture()")&&host.Contains("LoadMapAsync(entry.Captured!.Path, entry.Captured, ticket)")&&host.Contains("surface.RetryResults()"),"SOURCE ONLY captured Play and Retry routing"),"SOURCE INSPECTION");
            Run("source/disposal",()=>E.Check(host.Contains("stars.Dispose(); starCache.Dispose()")&&host.Contains("browserView.Dispose()"),"SOURCE ONLY host resource disposal"),"SOURCE INSPECTION");
        }
        finally{Directory.Delete(temp,true);}
        return E.Finish(root,args.Length>1?args[1]:"");
    }
}
