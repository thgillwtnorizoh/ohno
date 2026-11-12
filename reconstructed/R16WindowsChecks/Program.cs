using System.IO;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Rendering;
using Ohno.CircleGame.Selection;

internal static class Program
{
    static int groups;static void Yes(bool b){if(!b)throw new Exception("R16 Windows group "+(groups+1));}
    static void Pass(string text)=>Console.WriteLine($"PASS R16 WINDOWS {++groups:00} {text}");
    static IEnumerable<DependencyObject> Walk(DependencyObject o){yield return o;foreach(object child in LogicalTreeHelper.GetChildren(o))if(child is DependencyObject d)foreach(var nested in Walk(d))yield return nested;}
    static void ResultsFrame(PlayfieldView v){if(!v.HasRankControls&&v.Rank?.Stage!=RankStage.ChartSelection)throw new Exception("No gameplay Frame/input sampling authorized");typeof(PlayfieldView).GetMethod("Frame",BindingFlags.NonPublic|BindingFlags.Instance)!.Invoke(v,[null,EventArgs.Empty]);}
    static void Rank(PlayfieldView v){v.Session!.Press(new Vector2(256,192),2000);v.Session.Advance(5000);v.Rank!.Observe(2201);v.Rank.Complete();v.Rank.Observe(5000);ResultsFrame(v);ResultsFrame(v);ResultsFrame(v);Yes(v.Rank.Stage==RankStage.ResultsPresented);}
    [STAThread]static int Main(string[] args)
    {
        string tmp=Path.Combine(Path.GetTempPath(),"r16-win-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(tmp);
        try
        {
            string p=Path.Combine(tmp,"a.osu");File.WriteAllText(p,"osu file format v14\n[General]\nMode:0\n[Metadata]\nTitle:Local Windows\nVersion:A\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n256,192,2000,1,0,0:0:0:0:\n");
            string q=Path.Combine(tmp,"b.osu");File.WriteAllText(q,File.ReadAllText(p).Replace("Version:A","Version:B"));var a=LocalChartCatalog.Index(p);var b=LocalChartCatalog.Index(q);
            var selected=new SelectedChartState();var model=new SelectPlayModel(selected);model.Rebuild(new(1,[a,b],[]));using var view=new SelectPlayView(model);
            Yes(Walk(view).OfType<ListBox>().Count()==2);Yes(Walk(view).OfType<Button>().Count()==4);Yes(selected.Current is null);
            Pass("unshown browser widget construction creates song/difficulty/detail/actions without gameplay session");
            view.SelectRow(a);Yes(ReferenceEquals(selected.Current,a));Yes(ReferenceEquals(model.Committed,a));
            Pass("actual view row callback commits authoritative chart");
            model.Highlight(b);view.Bind();Yes(ReferenceEquals(selected.Current,b));Yes(Walk(view).OfType<TextBlock>().Any(t=>t.Text.Contains("Stars not calculated")));
            Pass("same-folder rebind selects difficulty and exposes unavailable star provenance");
            int starts=0;model.StartRequested+=()=>starts++;view.SelectRow(b);Yes(starts==1);view.Confirm();Yes(starts==2);
            Pass("repeated selected-row and confirm callbacks request Start separately from selection");
            int scoreEvents=0;using var surface=new PlayfieldView();var map=b.LoadGameplay();surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));scoreEvents=surface.Session!.Ledger.EventCount;model.Highlight(a);view.Bind();Yes(surface.Session.Ledger.EventCount==scoreEvents&&surface.CurrentTime==0);
            Pass("browser initialization/navigation does not create score events or advance gameplay clock");
            Rank(surface);surface.BackFromResults();ResultsFrame(surface);Yes(surface.Rank!.Stage==RankStage.ChartSelection);Yes(model.Restore());Yes(ReferenceEquals(model.Highlighted,a));Yes(ReferenceEquals(selected.Current,a));
            Pass("actual Rank Back and reference restoration preserve live selection independently of captured Play map");
            surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));Rank(surface);var previous=surface.Session;surface.RetryResults();ResultsFrame(surface);Yes(!ReferenceEquals(previous,surface.Session));Yes(surface.Session!.Ledger.EventCount==0);Yes(ReferenceEquals(surface.Session.Map,map));Yes(ReferenceEquals(selected.Current,a));surface.Pause();
            Pass("existing fresh Retry is independent of browser selection and retains loaded map");
            var flow=new PlayEntryFlow(selected);Yes(flow.Request()==StartDecision.Accepted);flow.Accept();flow.Queue();flow.Commit();flow.Capture();long ticket=flow.BeginLoading();_ = flow.Captured!.LoadGameplay();var retained=surface.Session;File.AppendAllText(p,"\n// changed after preparation\n");bool changed=false;try{flow.Captured.Verify();}catch(InvalidDataException){changed=true;}Yes(changed);flow.Fail(ticket,"changed during load");Yes(ReferenceEquals(surface.Session,retained));Yes(flow.Stage==PlayEntryStage.Failed);
            Pass("changed file after preparation is rejected before replacing previous valid host session");
            string missing=Path.Combine(tmp,"missing-audio.osu");File.WriteAllText(missing,File.ReadAllText(q).Replace("Mode:0","Mode:0\nAudioFilename:absent.wav"));var missingMap=LocalChartCatalog.Index(missing).LoadGameplay();bool failed=false;try{using var audio=new BeatmapAudio(missingMap.AudioPath);}catch(FileNotFoundException){failed=true;}Yes(failed);Yes(ReferenceEquals(surface.Session,retained));
            Pass("missing declared song fails before MediaPlayer construction; previous gameplay remains retained");
            var unavailable=LocalChartCatalog.Index(Path.Combine(tmp,"deleted.osu"));model.Rebuild(new(2,[unavailable],[]));view.Bind();Yes(model.Recovery is not null);Yes(Walk(view).OfType<TextBlock>().Any(t=>t.Text.Contains("missing")));
            Pass("missing Rank-return chart displays explicit recovery and unavailable rows");
            view.Dispose();int revision=selected.Revision;view.SelectRow(b);view.Confirm();Yes(selected.Revision==revision);flow.Dispose();Yes(!flow.CanInstall(ticket));surface.Dispose();surface.Dispose();
            Pass("disposed browser/entry/host reject stale callbacks with no media or Window shown");
            string program=File.ReadAllText(Path.Combine(args[0],"reconstructed/CircleGame/Program.cs"));Yes(program.Contains("if (selectionVisible)"));Yes(program.Contains("private bool HandleBrowserKey"));Yes(program.Contains("Key.Space or Key.Enter"));Yes(program.Contains("if (captured is not null && !entry.CanInstall(ticket)) return"));Yes(program.Contains("browser.Restore(); _ = RefreshCatalogAsync()"));
            Pass("SOURCE INSPECTION keyboard isolation, exact install guard and Rank-return generation refresh");
            Console.WriteLine($"R16 WINDOWS: {groups}/12 groups passed; final1 source inspection; unshown components only; no Window/show/render/OS input/nonempty media");return 0;
        }
        finally{Directory.Delete(tmp,true);}
    }
}
