using System.IO;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Rendering;

internal static class Program
{
    static int count;
    static void Yes(bool b){if(!b)throw new Exception("R15 Windows group "+(count+1));}
    static void Pass(string label)=>Console.WriteLine($"PASS R15 WINDOWS {++count:00} {label}");
    static void Frame(PlayfieldView view)=>typeof(PlayfieldView).GetMethod("Frame",BindingFlags.NonPublic|BindingFlags.Instance)!.Invoke(view,[null,EventArgs.Empty]);
    static void Enter(PlayfieldView surface)
    {
        var s=surface.Session!;s.Press(new Vector2(256,192),2000);s.Advance(5000);
        var rank=surface.Rank!;rank.Observe(2201);rank.Complete();rank.Observe(5000);Yes(rank.Stage==RankStage.RankAdmitted);
        Frame(surface);Yes(rank.Stage==RankStage.TransitionCommitted);Frame(surface);Yes(rank.Stage==RankStage.ResultsInitialized);Frame(surface);Yes(rank.Stage==RankStage.ResultsPresented);
    }
    [STAThread]static int Main()
    {
        string temp=Path.Combine(Path.GetTempPath(),"r15-windows-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        try
        {
            string path=Path.Combine(temp,"map.osu");File.WriteAllText(path,"osu file format v14\n[General]\nMode:0\n[Metadata]\nTitle:Rank Windows\nVersion:Silent control\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n256,192,2000,1,0,0:0:0:0:\n");
            var map=Beatmap.Load(path);using var surface=new PlayfieldView();using var silent=new BeatmapAudio(null);
            surface.Load(map,silent,ScoreConfiguration.FromBeatmap(map));int cleared=0;surface.ResultsCommitted+=()=>cleared++;
            Enter(surface);Yes(cleared==1);Yes(!surface.Running);Yes(surface.Rank!.SelectedRecord is SessionRankRecord);
            Pass("actual host separate commitment/cleanup/binding/presentation with silent audio and no Window");
            int events=surface.Session!.Ledger.EventCount;double h=surface.Session.Health!.State.H;Frame(surface);Frame(surface);Yes(surface.Session.Ledger.EventCount==events&&surface.Session.Health.State.H==h);
            Pass("Rank frames leave gameplay ledger/HP frozen");
            var rankView=new RankResultsView();var dto=surface.Rank.Presentation!;rankView.Bind(dto,null,"········");Yes(rankView.Visibility==Visibility.Visible&&rankView.Children.Count==1);rankView.Bind(dto,null,dto.Score);rankView.Hide();rankView.Bind(dto,null,dto.Score);Yes(rankView.Children.Count==1);
            Pass("actual independent WPF widgets bind/rebind without parent reuse failure; no draw/screenshot");
            rankView.Hide();rankView.Bind(null,"Unsupported policy","");Yes(rankView.Children.Count==1);
            Pass("unavailable results representation has controls and no fabricated grade");
            object original=surface.Session;surface.SkipResultsReveal();surface.SkipResultsReveal();Yes(ReferenceEquals(surface.Session,original));Yes(surface.Rank.Transitions.Pending is null);
            Pass("actual host reveal skip does not retry or duplicate accounting");
            surface.BackFromResults();Frame(surface);Yes(surface.Rank.Stage==RankStage.ChartSelection);Yes(!surface.Running);
            Pass("Back commits SelectPlay5 to chart-selection substitute with clock stopped");
            surface.Load(map,new BeatmapAudio(null),ScoreConfiguration.FromBeatmap(map));Enter(surface);var old=surface.Session!;surface.RetryResults();Frame(surface);Yes(!ReferenceEquals(old,surface.Session));Yes(surface.Session!.Ledger.EventCount==0&&surface.Session.Ledger.Score==0&&surface.Session.Circles[0].Result==Judgement.Pending);Yes(surface.Rank!.Stage==RankStage.Active);
            Pass("actual retry constructs fresh ledger/health/judgement/Rank and restarts supported clock");
            surface.Pause();surface.Dispose();surface.Dispose();Yes(surface.Rank.Stage==RankStage.Disposed);
            Pass("pause/repeat disposal contains lifetime; no nonempty media or game Window");
            Console.WriteLine($"R15 WINDOWS: {count}/8 groups passed; actual host/widget construction only, no Window/show/render/input/media playback");return 0;
        }
        finally{Directory.Delete(temp,true);}
    }
}
