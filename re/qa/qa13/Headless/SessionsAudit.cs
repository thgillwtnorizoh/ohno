using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA13;
public static partial class Tests
{
    static string Snapshot(CircleSession s)=>JsonSerializer.Serialize(new{s.Scoring,s.Ledger.Score,s.Ledger.EventCount,s.Ledger.LastEvent,s.Ledger.LastOutcome,s.Ledger.N300,s.Ledger.N100,s.Ledger.N50,s.Ledger.NMiss,s.Ledger.CurrentCombo,s.Ledger.MaximumCombo,s.Ledger.ComboSnapshot,s.Accuracy,s.JudgedCount,s.Finished,s.HealthPolicy,
        circles=s.Circles.Select(c=>new{c.Result,c.JudgedAt}),sliders=s.Sliders.Select(x=>new{x.Head.Result,parent=x.Result,x.Tracking,x.FollowEligible,x.EntryTime,x.ProcessedCheckpoints,x.SuccessfulCheckpoints,x.FailedCheckpoints,x.TerminalSucceeded,x.CompletionStarted,audio=x.Audio.Index,checkpoints=x.Checkpoints.Select(c=>new{c.RawResult,c.Succeeded,c.JudgedAt})}),
        spinners=s.Spinners.Select(x=>new{x.Progress,x.Observation.CachedInteger,x.Observation.Count,x.Required,x.TargetMotion,x.SmoothedMotion,x.LastComponentRaw,x.FinalRaw,x.Result,x.CompletionStarted}),
        health=s.Health is{}h?new{h.State,h.Calibration,h.ResultEvents,h.LastPassiveLoss,h.Failed,h.Recovering,h.RecoveryCount,grouping=h.Chart.Objects.Select(o=>o.NewCombo)}:null});
    static void ObserveAudio(CircleSession s,List<string> log)
    {
        s.PositiveCircleSound+=c=>log.Add($"circle:{c.Position}:{c.StartTime}");
        s.SliderSound+=e=>log.Add($"slider:{e.Kind}:{e.Index}:{e.ScheduleIndex}");
        s.SpinnerSound+=(x,e)=>log.Add($"spinner:{e}:{x.Observation.Count}:{x.Required}");
        s.ContinuingSound+=(x,t,on)=>log.Add($"loop:{x.Definition.Head.StartTime}:{t}:{on}:{x.Audio.Index}");
        s.GlobalSoundTiming+=t=>log.Add("global:"+t);s.ContinuingManagerTail+=v=>log.Add("tail:"+v);
    }
    static void Sessions()
    {
        var rows=new Dictionary<string,string>{
            ["circles"]="64,192,1000,1,2,0:0:0:0:\n448,192,2200,1,4,0:0:0:0:",
            ["straight"]="64,192,1000,2,14,L|264:192,1,200,2|8,1:2|3:1,0:0:0:0:\n448,192,5000,1,0,0:0:0:0:",
            ["repeat"]="64,192,1000,2,14,L|264:192,2,200,2|8|4,1:2|3:1|2:3,0:0:0:0:\n448,192,5000,1,0,0:0:0:0:",
            ["bezier"]="64,192,1000,2,2,B|164:64|264:192,1,200,,,0:0:0:0:\n448,192,5000,1,0,0:0:0:0:",
            ["spinner"]="256,192,1000,8,2,5000,0:0:0:0:\n448,192,6000,1,0,0:0:0:0:"};
        foreach(var (name,data) in rows)foreach(bool reduced in new[]{false,true})Add($"S/matched-{name}-reduced{reduced}","E027/E033-E036/E037-E041: Rank must not mutate active gameplay",()=>{
            var map=Map(data);var a=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));var b=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));using var rank=new RankSession(a);
            var aa=new List<string>();var bb=new List<string>();ObserveAudio(a,aa);ObserveAudio(b,bb);var trace=new List<object>();int comparisons=0;
            int previousEvents=-1;
            void Compare(string label,int t){string av=Snapshot(a),bv=Snapshot(b);Eq(Hash(System.Text.Encoding.UTF8.GetBytes(av)),Hash(System.Text.Encoding.UTF8.GetBytes(bv)));Yes(aa.SequenceEqual(bb),"ordered audio histories equal");if(previousEvents!=a.Ledger.EventCount||label=="press"){trace.Add(new{label,time=t,actual=JsonSerializer.Deserialize<JsonElement>(av),expectedSha=Hash(System.Text.Encoding.UTF8.GetBytes(bv)),audioCount=aa.Count});previousEvents=a.Ledger.EventCount;}comparisons++;}
            Compare("fresh-calibration-silent",0);Eq(a.Ledger.EventCount,0);Eq(aa.Count,0);Eq(a.Health!.ResultEvents,0);
            int end=rank.MaximumEnd;
            for(int t=0;t<=end+3200;t+=20)
            {
                foreach(var head in a.Heads.Where(h=>h.Definition.StartTime+(reduced?100:0)==t))
                {if(reduced&&head==a.Heads.Last())continue;a.Press(head.Definition.Position,t);b.Press(head.Definition.Position,t);Compare("press",t);}
                Vector2 pointer=new(448,192);var slider=a.Sliders.FirstOrDefault(s=>t>=s.Definition.Head.StartTime&&t<=s.Definition.EndTime);
                if(slider is not null){var move=slider.Definition.Movements.FirstOrDefault(m=>t>=m.StartTime&&t<=m.EndTime);if(move is not null)pointer=move.Position(t);}
                var spin=a.Spinners.FirstOrDefault(s=>t>=s.Definition.StartTime&&t<=s.Definition.EndTime);
                if(spin is not null){double angle=t*.045;pointer=SpinnerState.DisplayCenter+new Vector2((float)(110*Math.Cos(angle)),(float)(110*Math.Sin(angle)));}
                int held=reduced&&t%240>=160?0:1;
                a.Advance(t,pointer,held);b.Advance(t,pointer,held);
                rank.Observe(t,continuation:t<=end);rank.Complete();rank.Observe(t,continuation:t<=end);Compare("advance-with-rank",t);
            }
            Yes(comparisons>100,"intermediate comparisons");string frozen=Snapshot(a);
            if(rank.Stage==RankStage.RankAdmitted){rank.Commit(()=>{});rank.Initialize(0);rank.Present();rank.Skip(1);rank.Observe(end+9999);Eq(Snapshot(a),frozen);}
            Eq(Snapshot(a),Snapshot(b));Seq(aa,bb);observations.Add(new{comparisons,rank.Stage,trace,audio=aa.ToArray(),ledgerObservationBoundary="LastEvent/EventCount at every supplied action; arbitrary internal multi-event stream not exposed"});
        });
        Add("S/fresh-retry-identities","E042 Retry2; engineering fresh kernel and record",()=>{var map=Map();var old=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));old.Press(new(256,192),2000);old.SetHeldButtons(3);var record=new SessionRankRecord(old);var fresh=CircleSession.CreateFreshOrdinary(map,old.Scoring);var next=new SessionRankRecord(fresh);Yes(!ReferenceEquals(old.Ledger,fresh.Ledger),"ledger fresh");Yes(!ReferenceEquals(old.Health,fresh.Health),"health fresh");Yes(!ReferenceEquals(record.SessionIdentity,next.SessionIdentity),"record fresh");Yes(ReferenceEquals(record.MapIdentity,next.MapIdentity),"same map");Eq(fresh.Ledger.EventCount,0);Eq(fresh.Circles[0].Result,Judgement.Pending);Eq(fresh.HealthPolicy,HealthPreparationPolicy.E036FreshOrdinary);});
        Add("S/failing-audio-and-rank","E037-E041 error containment; Rank read-only accounting",()=>{var map=Map();var a=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));var b=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));using var rank=new RankSession(a);a.PositiveCircleSound+=_=>throw new IOException("QA13 controlled missing effects sink");a.Press(new(256,192),2000);b.Press(new(256,192),2000);Eq(Snapshot(a),Snapshot(b));Yes(a.CircleSoundError is not null,"effects failure contained");rank.Observe(2201);rank.Complete();rank.Observe(5000);rank.Commit(()=>{});rank.Initialize(0);rank.Present();Eq(Snapshot(a),Snapshot(b));});
        Add("S/recovery-guard-rank-readonly","R09-C01 recovery guard; R10 fresh grouping retained",()=>{var m=Map();var s=CircleSession.CreateFreshOrdinary(m,ScoreConfiguration.FromBeatmap(m));var h=s.Health!;h.State.Assign(160);h.State.ConfigureFill(3,.03,false);h.SupplyRecoveryCount(1);h.BeginRecovery();var r=new SessionRankRecord(s);r.Success=true;string before=Snapshot(s);RankPresentation.Capture(r);Eq(Snapshot(s),before);Eq(h.State.H,160d);Eq(h.State.Fill,false);});
    }
    static void Audits()
    {
        void Audit(string id,string authority,Action body)=>Add(id,authority,body,"SOURCE INSPECTION");
        Audit("A/keyboard-routing","E042 M32d1; ENGINEERING R key, no physical input",()=>{string s=Read("reconstructed/CircleGame/Program.cs");string block=s[s.IndexOf("if (surface.HasRankControls)")..s.IndexOf("switch (e.Key)")];foreach(string t in new[]{"heldKeys.Clear(); PublishHeldKeys()","Key.Space or Key.Enter","surface.SkipResultsReveal()","Key.Escape","surface.BackFromResults()","Key.R","surface.RetryResults()","UpdateHud(); return;"})Yes(block.Contains(t),t);});
        Audit("A/clock-and-accounting-freeze","E027 lifecycle plus ENGINEERING safety",()=>{string s=Read("reconstructed/CircleGame/Rendering/PlayfieldView.cs");int branch=s.IndexOf("if (Rank is { } rank &&");int game=s.IndexOf("Session.Advance(CurrentTime");Yes(branch<game,"results branch before gameplay");Yes(s[branch..game].Contains("FrameUpdated?.Invoke(); return;"),"results return");Yes(s.Contains("rank.Commit(() => { Pause(); Session.SetHeldButtons(0); ResultsCommitted?.Invoke(); })"),"cleanup exact boundary");Yes(s.Contains("System.Diagnostics.Stopwatch rankClock"),"separate host clock");});
        Audit("A/effects-pause-generation","ENGINEERING Windows cancellation",()=>{string view=Read("reconstructed/CircleGame/Rendering/PlayfieldView.cs"),fx=Read("reconstructed/CircleGame/Platform/WindowsCircleEffects.cs");Yes(view.Contains("effects?.Pause()"),"pause effects");Yes(fx.Contains("loopGeneration++"),"cancel loop generation");Yes(fx.Contains("continuing?.Cancel()"),"clear pending continuing");Yes(fx.Contains("loops.PauseAll()"),"pause persistent channels");});
        Audit("A/no-results-audio-or-assets","E042 optional applause unsupported; ENGINEERING typography",()=>{string r=Read("reconstructed/CircleGame/Gameplay/RankResults.cs"),v=Read("reconstructed/CircleGame/Rendering/RankResultsView.cs");foreach(string t in new[]{"MediaPlayer",".Enqueue(","File.","applause","menuback","menuhit"})Yes(!r.Contains(t),"Rank model no "+t);Yes(v.Contains("silent results"),"silent UI");Yes(v.Contains("Segoe UI"),"engineering typeface");});
        Audit("A/no-invented-statistics","E042 unavailable decorated counts/player/date",()=>{string v=Read("reconstructed/CircleGame/Rendering/RankResultsView.cs");foreach(string t in new[]{"presentation.N300","presentation.N100","presentation.N50","presentation.NMiss","presentation.MaximumCombo","presentation.Valid"})Yes(v.Contains(t),t);foreach(string t in new[]{"N300g","N100k","PlayerName","DateTime.Now"})Yes(!v.Contains(t),"no invented "+t);});
        Audit("A/renderer-results-wiring","ENGINEERING view eligibility and cleanup",()=>{string s=Read("reconstructed/CircleGame/Program.cs");foreach(string t in new[]{"rank.Stage is RankStage.ResultsPresented or RankStage.Unavailable","rankView.Bind(rank.Presentation","else rankView.Hide()","rankView.BackRequested +=","rankView.RetryRequested +=","rankView.SkipRequested +=","surface.ResultsCommitted +="})Yes(s.Contains(t),t);});
        Audit("A/host-horizon-scope","E027 distinct q/K/horizon, bounded ENGINEERING projection",()=>{string s=Read("reconstructed/CircleGame/Rendering/PlayfieldView.cs");Yes(s.Contains("no timed storyboard-event horizon"),"empty horizon disclosed");Yes(s.Contains("results.Observe(CurrentTime, continuation: CurrentTime <= results.MaximumEnd)"),"separate model observation");Yes(s.Contains("Session.Advance remains authoritative"),"no accounting substitution");});
        Audit("A/R15-initial-failure-retained","R15 historical failure is oracle conflict, not production fix",()=>{string initial=Read("reconstructed/R15DevelopmentChecks/verification/initial-Program.cs.txt");string final=Read("reconstructed/R15DevelopmentChecks/Program.cs");Yes(initial!=final,"initial harness retained separately");Yes(Read("reconstructed/R15DevelopmentChecks/verification/initial-provenance.json").Contains("27c89de"),"initial source");Yes(Read("reconstructed/R15DevelopmentChecks/verification/windows-provenance.json").Contains("3f9a8f5"),"final source");});
    }
}
