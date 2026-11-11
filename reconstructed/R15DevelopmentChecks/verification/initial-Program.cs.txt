using System.Globalization;
using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;

internal static class Program
{
    static int groups;
    static readonly List<object> evidence = [];
    static void Yes(bool b) { if (!b) throw new Exception("R15 assertion group " + (groups+1)); }
    static void Eq<T>(T actual,T expected) { if (!EqualityComparer<T>.Default.Equals(actual,expected)) throw new Exception($"R15 group {groups+1}: {actual} != {expected}"); }
    static void Pass(string label) { groups++;Console.WriteLine($"PASS R15 {groups:00} {label}"); evidence.Add(new {group=groups,label,passed=true}); }
    static RankSession Flow(FakeRecord? record=null) => new(record ?? new FakeRecord(),1000,10,()=>false);
    static void Complete(RankSession f) { f.Observe(1201);Eq(f.Stage,RankStage.FinalizationEligible);Yes(f.Complete()); }
    static void Enter(RankSession f) { Complete(f);f.Observe(4000);Eq(f.Stage,RankStage.RankAdmitted);Yes(f.Commit(()=>{}));Yes(f.Initialize(100));Yes(f.Present()); }
    static Beatmap Map(string dir,string name="fixture",int count=1)
    {
        string path=Path.Combine(dir,name+".osu");
        File.WriteAllText(path,"osu file format v14\n[General]\nMode:0\n[Metadata]\nTitle:Rank fixture\nArtist:Independent\nVersion:Control\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n"+string.Join("\n",Enumerable.Range(0,count).Select(i=>$"256,192,{2000+i*100},5,0,0:0:0:0:")));
        return Beatmap.Load(path);
    }
    static string Snapshot(CircleSession s) => JsonSerializer.Serialize(new{s.Ledger.Score,s.Ledger.N300,s.Ledger.N100,s.Ledger.N50,s.Ledger.NMiss,s.Ledger.MaximumCombo,s.Ledger.CurrentCombo,s.Ledger.AccuracyFraction,s.Ledger.EventCount,s.JudgedCount,s.Finished,objects=s.ObjectResults.ToArray(),sliders=s.Sliders.Select(x=>new{x.Tracking,x.Audio.Index,x.SuccessfulCheckpoints,x.FailedCheckpoints}),spinners=s.Spinners.Select(x=>new{x.Progress,x.SmoothedMotion,x.Observation.Count,x.Observation.CachedInteger}),health=s.Health is {}h?new{h.State.H,h.State.A,h.State.V,h.ResultEvents,h.Failed}:null});
    static int Main(string[] args)
    {
        string root=args[0];CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
        using var json=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,"re/evidence/42/rank-results.json")));
        foreach(var row in json.RootElement.GetProperty("reference_cases").EnumerateArray().Where(c=>c.GetProperty("kind").GetString()=="grade"))
        {
            var init=row.GetProperty("initial_state");int[] n=init.GetProperty("counts").EnumerateArray().Select(x=>x.GetInt32()).ToArray();
            var r=new FakeRecord{C300=(ushort)n[0],C100=(ushort)n[1],C50=(ushort)n[2],Miss=(ushort)n[3],Success=init.GetProperty("F1978").GetBoolean(),Bits=init.GetProperty("mods").GetInt32()};
            Eq((int)r.Grade(),row.GetProperty("expected").GetProperty("grade").GetInt32());
            Eq(r.WholeTotal,n.Sum());Pass(row.GetProperty("id").GetString()+" E042 literal grade vector through actual production getter");
        }
        Eq(string.Join(",",Enum.GetValues<RankGrade>().Select(v=>(int)v)),"0,1,2,3,4,5,6,7,8,9");Eq(RankGrade.N.ToString(),"N");
        Pass("enum identities and sentinel");
        var fresh=new FakeRecord{C300=ushort.MaxValue,C100=ushort.MaxValue,C50=ushort.MaxValue,Miss=ushort.MaxValue,Success=true};Eq(fresh.WholeTotal,262140);Eq(fresh.Grade(),RankGrade.D);
        Pass("UInt16 fresh fields sum in Int32 without decorated counts");
        fresh=new FakeRecord{C300=80,C100=20,C50=2,Success=true};int reads=0;
        fresh.Total=()=>{reads++;return reads==1?100:200;};Eq(fresh.Grade(),RankGrade.A);Eq(reads,2);
        Pass("separate virtual total reads for p300 and p50");
        fresh=new FakeRecord{Success=true};reads=0;fresh.Total=()=>{reads++;if(reads==2)fresh.Success=false;return 0;};Eq(fresh.Grade(),RankGrade.F);Eq(reads,2);
        Pass("failed success flag read after both proportion operations");
        fresh=new FakeRecord{Success=true};Eq(fresh.Grade(),RankGrade.X);fresh.Success=false;Eq(fresh.Grade(),RankGrade.F);fresh.Success=true;fresh.C300=9;fresh.Miss=1;Eq(fresh.Grade(),RankGrade.A);
        Pass("R29/R30 repeated getters after success/count mutation");
        fresh=new FakeRecord{Success=true,Valid=false,Numeric=123456789};Eq(fresh.Grade(),RankGrade.X);Eq(fresh.Score,123456789);fresh.Perfect=true;Eq(fresh.Grade(),RankGrade.X);
        Pass("R27/R28 validity/score/perfect independent of grade");
        fresh=new FakeRecord{C300=0,Success=true};Eq(fresh.AccuracyFraction,1f);Eq(fresh.Grade(),RankGrade.D);
        Pass("R55 artificial successful empty accuracy100 and gradeD");
        var keys=RankGradeResources.Read(()=>fresh);Eq(keys.FirstKey,"ranking-D");Eq(keys.SecondaryKey,null);
        fresh.C300=10;keys=RankGradeResources.Read(()=>fresh,i=>{if(i==1)fresh.Success=false;});Eq(keys.FirstKey,"ranking-X");Eq(keys.Eligibility,RankGrade.F);Eq(keys.SecondaryKey,null);
        Pass("R57-R59 independent first/eligibility grade reads and keys");
        fresh.Success=true;fresh.C300=10;keys=RankGradeResources.Read(()=>fresh,i=>{if(i==2){fresh.C300=0;fresh.C100=10;}});Eq(keys.SecondaryKey,"ranking-D");
        Pass("third grade read for optional secondary key");
        fresh=new FakeRecord{C300=10,Success=true,Numeric=0};var text=RankPresentation.Capture(fresh);Eq(text.Score,"00000000");Eq(text.Accuracy,"100.00%");
        fresh.Numeric=123456789;text=RankPresentation.Capture(fresh);Eq(text.Score,"123456789");
        Pass("R49/R50 eight-digit minimum, large score not truncated");
        text=RankPresentation.Capture(fresh,scoreOverride:123);Eq(text.Score,"00000123");Eq(fresh.Score,123456789);
        Pass("R51 optional presentation score leaves record unchanged");
        fresh=new FakeRecord{C300=1,C100=1,Success=true};text=RankPresentation.Capture(fresh);Eq(text.Accuracy,"66.67%");
        CultureInfo.CurrentCulture=CultureInfo.GetCultureInfo("fr-FR");Eq(RankPresentation.Capture(fresh).Accuracy,"66.67%");CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
        Pass("R53/R54 Single accuracy percent with explicit invariant culture");
        fresh=new FakeRecord{C300=9,C100=2,C50=1,Miss=3,Max=123,Success=true};text=RankPresentation.Capture(fresh);Eq(text.N300,(ushort)9);Eq(text.N100,(ushort)2);Eq(text.N50,(ushort)1);Eq(text.NMiss,(ushort)3);Eq(text.MaximumCombo,123);
        Pass("R56 four ordinary whole counters and maximum combo only");
        fresh.Numeric=999;fresh.Success=false;Eq(text.Score,"00000000");Eq(text.Grade.First,RankGrade.C);Eq(fresh.Grade(),RankGrade.F);
        Pass("captured presentation remains distinct from later live record getters");
        fresh=new FakeRecord{C300=10,Max=1};fresh.Prepare(2);fresh.Complete(false,10);Eq(fresh.Perfect,false);Eq(fresh.Grade(),RankGrade.X);
        fresh.C300=9;fresh.Miss=1;fresh.Max=2;fresh.Complete(false,10);Eq(fresh.Perfect,true);Yes(fresh.AccuracyFraction<1f);
        Pass("prepared perfect scalar rather than100 percent/no misses");
        fresh=new FakeRecord();fresh.Complete(false,10);Eq(fresh.Perfect,null);fresh.Prepare(int.MinValue);fresh.Max=1;fresh.Complete(false,10);Eq(fresh.Perfect,false);
        Pass("unavailable perfect provenance and artificial unchecked subtraction");
        using(var f=Flow()){f.Observe(1200);Eq(f.Stage,RankStage.Active);f.Observe(1201);Eq(f.Stage,RankStage.FinalizationEligible);Yes(!f.Finalized);Yes(f.Complete());Yes(f.Finalized);Eq(f.Stage,RankStage.RecordCompleted);Yes(!f.Complete());}
        Pass("strict B+200, separate finalization latch and completed record");
        using(var f=Flow()){f.Observe(1100,clockState:0);Eq(f.Stage,RankStage.FinalizationEligible);}
        using(var f=Flow()){f.Observe(1201,suspended:true);Eq(f.Stage,RankStage.Active);f.Observe(1201,continuation:true);Eq(f.Stage,RankStage.Active);}
        Pass("conditional q0 shortcut, suspension and continuation gates");
        using(var f=Flow()){Complete(f);f.Observe(3999);Eq(f.Stage,RankStage.RecordCompleted);f.Observe(4000,timedEventCount:1,eventHorizon:5001);Eq(f.Stage,RankStage.RecordCompleted);f.Observe(4501,timedEventCount:1,eventHorizon:5001);Eq(f.Stage,RankStage.RankAdmitted);Eq(f.Transitions.Current,RankMode.Play);}
        Pass("B+3000 equality and separate event-horizon equality");
        bool pending=true;using(var f=new RankSession(new FakeRecord(),1000,10,()=>false,()=>pending)){f.Observe(1201);Yes(!f.Complete());pending=false;Yes(f.Complete());}
        Pass("ENGINEERING defer pending accounting without fabricating results");
        using(var f=new RankSession(new FakeRecord{C300=9},1000,10,()=>false)){Complete(f);Eq(f.HandlerRecord.Valid,false);Eq(f.HandlerRecord.Success,true);Eq(f.HandlerRecord.Grade(),RankGrade.X);Yes(f.Finalized);}
        Pass("R76 count mismatch invalidates but still finalizes and grades");
        using(var f=new RankSession(new FakeRecord(),1000,10,()=>true)){f.Observe(9999);Eq(f.Stage,RankStage.Active);Yes(!f.Finalized);f.UnsupportedCompletion();Eq(f.Stage,RankStage.Unavailable);}
        using(var f=new RankSession(new FakeRecord(),1000,10,()=>null)){f.Observe(9999);Yes(f.Limitation!.Contains("provenance"));}
        Pass("R77 failed caller bypass and unavailable success provenance; no fabricated Rank");
        using(var f=Flow()){Complete(f);f.Transitions.GateOpen=false;Eq(f.RequestRank(),TransitionDecision.Deferred);Eq(f.Transitions.Pending,null);f.Transitions.GateOpen=true;Eq(f.RequestRank(),TransitionDecision.Accepted);Eq(f.Stage,RankStage.RankAdmitted);Eq(f.Transitions.Current,RankMode.Play);Eq(f.RequestRank(),TransitionDecision.Duplicate);}
        Pass("R33-R35 deferred guarded resubmission, accepted pending not committed");
        var transition=new RankTransitions{Allow=false};Eq(transition.Request(RankMode.Rank),TransitionDecision.Rejected);transition.Allow=true;Eq(transition.Request(RankMode.Rank),TransitionDecision.Accepted);Yes(transition.Commit());Yes(!transition.Commit());Eq(transition.Pending,RankMode.Rank);transition.Initialized();Eq(transition.Pending,null);
        Pass("guard rejection, commitment and initialization are separate, repeated commit suppressed");
        transition=new();transition.Request(RankMode.Rank);transition.Commit();transition.Request(RankMode.SelectPlay);transition.Initialized();Eq(transition.Pending,RankMode.SelectPlay);
        Pass("R39 new pending request survives initialization mismatch");
        using(var f=Flow()){Complete(f);f.RequestRank();bool cleanup=false;f.Commit(()=>{cleanup=true;Eq(f.Stage,RankStage.TransitionCommitted);Eq(f.Transitions.Current,RankMode.Rank);Eq(f.Presentation,null);});Yes(cleanup);Eq(f.SelectedRecord,null);Yes(f.Initialize(0));Eq(f.Stage,RankStage.ResultsInitialized);Yes(f.Present());Eq(f.Stage,RankStage.ResultsPresented);Yes(!f.Initialize(1));}
        Pass("R37/R41 commit, cleanup, selection, initialization, presentation boundaries");
        fresh=new();using(var f=Flow(fresh)){Complete(f);f.RequestRank();var later=new FakeRecord{Map=fresh.Map,Numeric=456,Success=true};f.GlobalRecord=later;f.Commit(()=>{});f.Initialize(0);Yes(ReferenceEquals(f.SelectedRecord,later));Yes(ReferenceEquals(f.HandlerRecord,fresh));Eq(f.Presentation!.Score,"00000456");}
        Pass("R40 delayed Rank binding observes replacement global reference");
        fresh=new();using(var f=Flow(fresh)){Complete(f);f.RequestRank();f.Commit(()=>fresh.Success=false);f.Initialize(0);Eq(f.SelectorGrade,RankGrade.X);Eq(f.Presentation!.Grade.First,RankGrade.F);}
        Pass("R69 selector grade can differ from Rank after synchronous cleanup mutation");
        fresh=new();using(var f=Flow(fresh)){Complete(f);f.RequestRank();f.Commit(()=>{});f.Initialize(0);var captured=f.Presentation!;fresh.Success=false;Eq(captured.Grade.First,RankGrade.X);Eq(f.SelectedRecord!.Grade(),RankGrade.F);}
        Pass("R38 partial: deferred cleanup after initialization changes live record, not captured text");
        fresh=new();using(var f=Flow(fresh)){Complete(f);f.GlobalRecord=new FakeRecord{Map=fresh.Map,Success=false};Eq(f.RequestRank(),TransitionDecision.Accepted);Eq(f.Transitions.Pending,RankMode.SelectPlay);Yes(f.HandlerRecord.Success);Yes(!f.GlobalRecord.Valid);}
        Pass("R31/R32 handler alias distinct from replaced global selector record");
        using(var f=Flow()){Complete(f);f.RequestRank();f.Commit(()=>{});f.GlobalRecord=null;Yes(!f.Initialize(0));Eq(f.Stage,RankStage.Unavailable);Eq(f.Presentation,null);}
        using(var f=Flow()){Complete(f);f.RequestRank();f.Commit(()=>{});f.GlobalRecord=new FakeRecord();Yes(!f.Initialize(0));Yes(f.Limitation!.Contains("mismatched"));}
        Pass("R43/R45 bounded missing/mismatched record-map rejection, no restoration claim");
        using(var f=Flow()){Complete(f);f.RequestRank();try{f.Commit(()=>throw new IOException("cleanup"));throw new Exception("expected");}catch(IOException){}Eq(f.Stage,RankStage.TransitionCommitted);Eq(f.Presentation,null);}
        Pass("cleanup exception retains prior commitment; no fabricated transactional rollback");
        var reveal=new RankScoreReveal("12345678",1000);Eq(reveal.Start,1300);Eq(reveal.Text(1300),"········");Eq(reveal.Text(1800),"1·······");Eq(reveal.Text(5300),"12345678");Eq(reveal.Text(6300),"12345678");
        Pass("R62 elapsed/500 reveal uses separate captured text and +300 host start");
        Eq(reveal.Skip(1300),false);Eq(reveal.Text(1300),"12345678");Eq(reveal.Skip(1300),true);var boundary=new RankScoreReveal("00000000",0);Eq(boundary.Skip(5300),true);Eq(boundary.Skipped,false);
        Pass("R63/R64 first skip retimes by7000, repeated and5000 boundary behavior");
        using(var f=Flow()){Enter(f);object identity=f.Presentation!.RecordIdentity;f.Skip(100);f.Skip(100);Eq(f.Transitions.Pending,null);Yes(ReferenceEquals(identity,f.HandlerRecord.SessionIdentity));Eq(f.Stage,RankStage.ResultsPresented);}
        Pass("Space/Enter reveal skip does not retry or prepare record again");
        using(var f=Flow()){Enter(f);Eq(f.Back(),TransitionDecision.Accepted);Eq(f.Transitions.Pending,RankMode.SelectPlay);Eq(f.Back(),TransitionDecision.Duplicate);f.Commit(()=>{});Eq(f.Stage,RankStage.ChartSelection);Eq(f.GlobalRecord,null);}
        Pass("R65 Back requests5 then commits to ENGINEERING chart selection");
        using(var f=Flow()){Enter(f);f.Transitions.RestrictRankRetry=true;f.Transitions.RetryOperand=9999;Eq(f.Retry(100),TransitionDecision.Rejected);f.Transitions.RetryOperand=10000;Eq(f.Retry(100),TransitionDecision.Accepted);Eq(f.Transitions.Pending,RankMode.Play);Eq(f.Retry(100),TransitionDecision.Duplicate);}
        Pass("R66 retry independent of reveal Boolean; conditional E00310000 guard/equality");
        transition=new();transition.Request(RankMode.Rank);transition.Commit();transition.Initialized();transition.RestrictRankRetry=true;transition.RetryOperand=double.NaN;Eq(transition.Request(RankMode.Play),TransitionDecision.Accepted);
        Pass("E003 conditional unordered retry operand is not ordered-less10000");
        using(var f=Flow()){Enter(f);f.Dispose();f.Dispose();Eq(f.Stage,RankStage.Disposed);Eq(f.GlobalRecord,null);Eq(f.Retry(0),TransitionDecision.Rejected);}
        Pass("deterministic disposal rejects later operations, retained aliases stay distinct");
        string tmp=Path.Combine(Path.GetTempPath(),"ohno-r15-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(tmp);
        try
        {
            var map=Map(tmp);var session=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));var adapter=new SessionRankRecord(session);
            Eq(adapter.UnsupportedReason,null);Yes(ReferenceEquals(adapter.MapIdentity,map));Eq(adapter.Metadata.Difficulty,"Control");Eq(adapter.PreparedCombo,1);Eq(adapter.Score,session.Ledger.Score);
            session.Press(new(256,192),2000);Eq(adapter.N300,(ushort)1);adapter.Complete(false,1);Eq(adapter.Perfect,true);Yes(ReferenceEquals(adapter.SessionIdentity,adapter.SessionIdentity));
            Pass("fresh policy, live ledger adapter, identity, prepared metadata and E035 perfect scalar");
            foreach(var p in new[]{new RankPolicyInputs(Modifiers:536870912),new(Ruleset:1),new(RecordedInput:true),new(SourceRecord:true),new(Restored:true),new(Accumulator:true),new(AlternateRecord:true),new(FreshCreation:false)}) Yes(RankAdmission.Check(map,session.Ledger,p) is not null);
            Pass("explicit unsupported modifiers/rulesets/source/replay/restored/alternate/accumulator guards");
            var map6999=Map(tmp,"6999",6999);var map7000=Map(tmp,"7000",7000);var ledger=new ScoreLedger(ScoreConfiguration.EngineeringPractice);
            Eq(RankAdmission.Check(map6999,ledger,new()),null);Yes(RankAdmission.Check(map7000,ledger,new())!.Contains("7000"));
            Pass("E023 automatic ScoreV2 source-row boundary6999/7000 independent of no-mod label");
            var clone=map with{};Yes(RankAdmission.Check(clone,ledger,new())!.Contains("fresh"));
            Pass("cloned/reused map does not inherit fresh results provenance");
            var chart=new HealthChart(0,1200,14,[new(2000,3000,ScoreObjectKind.Slider,true,1,6),new(4000,5000,ScoreObjectKind.Spinner,false,SpinnerRequirement:15)],[]);
            Eq(RankAdmission.PreparedCombo(chart),8); // slider schedule6 +2 completed objects, not spinner observation count.
            Pass("E035 F05fd a+b-1 then per-object1; spinner count not extra combo");
            var noHealth=new CircleSession(map,ScoreConfiguration.FromBeatmap(map),false);var noPerfect=new SessionRankRecord(noHealth);Eq(noPerfect.PreparedCombo,null);noPerfect.Complete(false,1);Eq(noPerfect.Perfect,null);
            Pass("perfect decoration unavailable without completed health-preparation provenance");
            var retry=CircleSession.CreateFreshOrdinary(map,session.Scoring);Eq(retry.Ledger.Score,0);Eq(retry.Ledger.EventCount,0);Eq(retry.Circles[0].Result,Judgement.Pending);Yes(!ReferenceEquals(retry.Ledger,session.Ledger));Yes(!ReferenceEquals(new SessionRankRecord(retry).SessionIdentity,adapter.SessionIdentity));
            Pass("fresh retry session independent ledger/judgement/health/record identities");
            foreach(string file in new[]{"first-circles.osu","discrete-slider-r12.osu","spinner-practice.osu","mixed-r07.osu"})
            {
                var m=Beatmap.Load(Path.Combine(root,"reconstructed/CircleGame/Fixtures",file));var a=CircleSession.CreateFreshOrdinary(m,ScoreConfiguration.FromBeatmap(m));var b=CircleSession.CreateFreshOrdinary(m,ScoreConfiguration.FromBeatmap(m));using var rank=new RankSession(a);var aa=new List<string>();var bb=new List<string>();
                foreach(var pair in new[]{(s:a,list:aa),(s:b,list:bb)}){pair.s.PositiveCircleSound+=c=>pair.list.Add("circle"+c.StartTime);pair.s.SliderSound+=e=>pair.list.Add("slider"+e.Kind+":"+e.Index);pair.s.SpinnerSound+=(s,e)=>pair.list.Add("spinner"+e+":"+s.Observation.Count);pair.s.ContinuingSound+=(s,t,on)=>pair.list.Add("loop"+t+":"+on);}
                int end=rank.MaximumEnd+3000;
                for(int t=0;t<=end+16;t+=16){Vector2 p=SpinnerState.DisplayCenter+new Vector2((float)(100*Math.Cos(t*.04)),(float)(100*Math.Sin(t*.04)));a.Advance(t,p,1);b.Advance(t,p,1);rank.Observe(t,continuation:t<=rank.MaximumEnd);rank.Complete();rank.Observe(t,continuation:t<=rank.MaximumEnd);Eq(Snapshot(a),Snapshot(b));}
                Eq(string.Join("|",aa),string.Join("|",bb));string before=Snapshot(a);if(rank.Stage==RankStage.RankAdmitted){rank.Commit(()=>{});rank.Initialize(0);rank.Present();rank.Skip(1);Eq(Snapshot(a),before);}
            }
            Pass("four matched finite sessions: all intermediate judgement/score/combo/health/audio histories unchanged");
            adapter.Complete(false,1);string stable=Snapshot(session);RankPresentation.Capture(adapter);Eq(Snapshot(session),stable);
            Pass("presentation construction leaves authoritative completed ledger and health unchanged");
        }
        finally{Directory.Delete(tmp,true);}
        fresh=new FakeRecord{MapData=null,Success=true};bool missingMetadata=false;
        try{RankPresentation.Capture(fresh);}catch(InvalidOperationException){missingMetadata=true;}Yes(missingMetadata);
        Pass("missing metadata does not fabricate map/player/date fields");
        var unavailableLedger=new ScoreLedger(new(1,ScorePolicy.ScoreV2,"explicit unsupported policy"));
        Yes(!unavailableLedger.IsSupported);
        Pass("unsupported alternate numeric policy is explicit, no legacy rewrite");
        using(var coverage=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,"reconstructed/R15DevelopmentChecks/e042-coverage.json"))))
        {var rows=coverage.RootElement.GetProperty("rows").EnumerateArray().ToArray();Eq(rows.Length,78);Eq(rows.Select(r=>r.GetProperty("id").GetString()).Distinct().Count(),78);Yes(rows.All(r=>r.GetProperty("originalRuntime").GetString()=="UNVERIFIED ORIGINAL-RUNTIME"));}
        Pass("E042 coverage inventory78 retained IDs/classifications, not78 executed tests");
        string win=File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Rendering/PlayfieldView.cs"));string program=File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Program.cs"));
        Yes(win.Contains("rank.Commit(() => { Pause(); Session.SetHeldButtons(0); ResultsCommitted?.Invoke(); })"));Yes(win.Contains("Rank?.Dispose(); Rank = new(replacement)"));Yes(program.Contains("Key.Space or Key.Enter"));Yes(program.Contains("surface.BackFromResults()"));Yes(!win.Contains("Practice complete"));
        Pass("SOURCE INSPECTION Windows cleanup/key clearing/dedicated results wiring; no window execution");
        Yes(!File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Gameplay/RankResults.cs")).Contains("Task.Delay"));Yes(File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Rendering/RankResultsView.cs")).Contains("independent WPF graphics"));
        Pass("SOURCE INSPECTION engineering typography, no fabricated native transition delay or Rank audio");
        Console.WriteLine($"R15 DEVELOPMENT: {groups} groups passed; last2 source-inspected; other groups executed production components; no media/window/original runtime");
        Console.WriteLine("R15 EVIDENCE "+Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(new {schema="R15.development.v1",groups,cases=evidence})));
        return 0;
    }
    sealed class FakeRecord : RankRecord
    {
        public object Map=new();public RankMapMetadata? MapData=new("Test",null,null);public int Numeric,Max,Bits;public ushort C300=10,C100,C50,Miss;public Func<int>? Total;
        public override object MapIdentity=>Map;public override RankMapMetadata? Metadata=>MapData;
        public override int Score=>Numeric;public override int MaximumCombo=>Max;public override ushort N300=>C300;public override ushort N100=>C100;public override ushort N50=>C50;public override ushort NMiss=>Miss;public override int Modifiers=>Bits;
        public override int WholeTotal=>Total?.Invoke()??base.WholeTotal;
        public void Prepare(int n)=>PreparedCombo=n;
    }
}
