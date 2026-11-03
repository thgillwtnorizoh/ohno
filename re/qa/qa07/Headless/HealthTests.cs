// Independent finite E035 oracles; only actual production components are executed.
using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA07;

public static class HealthTests
{
    private static readonly List<(string Name, Action Body)> Cases = [];
    private static string root = "", scratch = "", input = "";
    private static int file;
    private static void Add(string name, Action body) => Cases.Add((name, body));
    private static void Eq<T>(T actual, T expected)
    { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"expected={expected}; actual={actual}"); }
    private static void Near(double actual, double expected, double tolerance = 1e-10)
    { if (!double.IsFinite(actual) || Math.Abs(actual - expected) > tolerance) throw new Exception($"expected={expected:R}; actual={actual:R}; tolerance={tolerance}"); }
    private static void Yes(bool value, string why) { if (!value) throw new Exception(why); }
    private static void Reject<T>(Action body) where T : Exception
    { try { body(); } catch (T) { return; } throw new Exception("expected " + typeof(T).Name); }
    private static PreparedHealthObject C(int t, bool? group = true) => new(t,t,ScoreObjectKind.Circle,group);
    private static PreparedHealthObject S(int t, int end, int subset = 0, int schedule = 1, bool? group = true) => new(t,end,ScoreObjectKind.Slider,group,subset,schedule);
    private static HealthChart Chart(double hp = 5, int approach = 0, PreparedHealthObject[]? objects = null, HealthBreak[]? breaks = null, int format = 14) => new(hp,approach,format,objects ?? [C(2000)],breaks ?? []);
    // Explicit already-prepared operands isolate runtime APIs from calibration; NOT an automatic fallback.
    private static OrdinaryHealth Live(double rate = .05, double hp = 5, double b = 1, double g = 1, HealthChart? chart = null) => new(chart ?? Chart(hp),new(rate,b,g,1,[200],0,0,0,0));
    private static HealthEligibility Eligible(int time) => new(time,time,0,int.MaxValue,0,-1,1,14);
    private static HealthFrame Frame(int t, int p, bool spin = false) => new(t,p,0,0,0,1,false,spin,false,Eligible(t));
    private static Beatmap Map(string rows, string events = "", string hp = "0")
    {
        input=$"osu file format v14\n[General]\nMode:0\n[Difficulty]\nHPDrainRate:{hp}\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,1000,4,1,0,100,1,0\n[Events]\n{events}\n[HitObjects]\n{rows}\n";
        string path=Path.Combine(scratch,$"control{file++}.osu");File.WriteAllText(path,input);return Beatmap.Load(path);
    }
    private static Beatmap Fixture(string name) => Beatmap.Load(Path.Combine(root,"reconstructed/CircleGame/Fixtures",name+".osu"));
    private static CircleSession Session(Beatmap m, bool hp = true) => new(m,ScoreConfiguration.FromBeatmap(m),hp);
    private static ScoreEvent Event(ScoreEventKind kind, int raw, SliderCheckpointKind? role = null, bool enabled = true)
    {
        ScoreObjectKind k=kind switch { ScoreEventKind.CircleWhole=>ScoreObjectKind.Circle, ScoreEventKind.SpinnerFinal or ScoreEventKind.SpinnerProgress=>ScoreObjectKind.Spinner, _=>ScoreObjectKind.Slider };
        return new(new(0,k,2000,5000),0,kind,raw,enabled,k==ScoreObjectKind.Slider,ScorePolicy.PlainLegacy,role);
    }
    public static int Main(string[] args)
    {
        if(args.Length!=1) throw new ArgumentException("Pass repository root.");
        root=Path.GetFullPath(args[0]);scratch=Path.Combine(Path.GetTempPath(),"ohno-qa07-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        try
        {
            StateCases(); CalibrationCases(); ResultCases(); DrainCases(); SmoothFailureCases(); IntegrationCases();
            int failed=0;
            foreach(var c in Cases)
            {
                input="";
                try { c.Body(); Console.WriteLine("PASS "+c.Name); }
                catch(Exception e) { failed++; Console.WriteLine($"FAIL {c.Name}: {e}"); if(input.Length>0) Console.WriteLine("FAILED INPUT "+JsonSerializer.Serialize(input)); }
            }
            Console.WriteLine($"QA07 HEALTH TESTS: {Cases.Count-failed} passed, {failed} failed, {Cases.Count} executed");
            return failed==0?0:1;
        }
        finally { Directory.Delete(scratch,true); }
    }
    private static void StateCases()
    {
        Add("H01 managed defaults versus live reset",()=>{var h=new HealthState();Eq(h.H,0.0);Eq(h.A,0.0);Eq(h.V,0.0);Eq(h.Fill,true);Eq(h.FillRate,.02);h.Reset();Eq(h.H,200.0);Eq(h.A,200.0);Eq(h.V,0.0);});
        Add("H02 selective reset retains coefficients and fill operands",()=>{var h=new HealthState(.012,2,3);h.Assign(210);h.Subtract(120);h.SetPresentation(50);h.ConfigureFill(123,.7,false);h.Reset();Eq(h.H,200.0);Eq(h.A,200.0);Eq(h.V,0.0);Eq(h.Rate,.012);Eq(h.B,2.0);Eq(h.G,3.0);Eq(h.Fill,false);Eq(h.FillFrom,123);Eq(h.FillRate,.7);});
        foreach(var v in new[]{(-1.0,0.0),(0.0,0.0),(200.0,200.0),(250.0,200.0)})
            Add($"H03 absolute argument {v.Item1}",()=>{var h=new HealthState();h.SetPresentation(45);h.Assign(v.Item1);Eq(h.H,v.Item2);Eq(h.A,v.Item1);Eq(h.V,45.0);});
        Add("H04 uncapped balance and separately capped target",()=>{var h=new HealthState();h.Assign(200);h.Add(11);h.Subtract(1);h.SetPresentation(31);h.Add(6);Eq(h.H,200.0);Eq(h.A,216.0);Eq(h.V,31.0);});
        Add("H05 negative subtraction has no upper cap",()=>{var h=new HealthState();h.Assign(199);h.Subtract(-6);Eq(h.H,205.0);Eq(h.A,205.0);});
        Add("H06 subtraction floors balance independently",()=>{var h=new HealthState();h.Assign(-10);h.Subtract(-5);Eq(h.H,5.0);Eq(h.A,0.0);h.Subtract(8);Eq(h.H,0.0);Eq(h.A,0.0);});
        Add("H07 addition lower clamp does not floor balance",()=>{var h=new HealthState();h.Assign(1);h.Add(-3);Eq(h.H,0.0);Eq(h.A,-2.0);});
        foreach(bool clear in new[]{false,true})
            Add($"H08 zero subtraction fill guard {clear}",()=>{var h=new HealthState();h.Reset();h.Subtract(0,clear);Eq(h.Fill,!clear);Eq(h.A,200.0);Eq(h.H,200.0);});
        Add("H09 V-only setter does not replace target",()=>{var h=new HealthState();h.Assign(50);h.SetPresentation(-20);Eq(h.V,-20.0);Eq(h.A,50.0);Eq(h.H,50.0);});
        Add("H10 finite extrema assign without normalization",()=>{var h=new HealthState();h.Assign(double.MaxValue);Eq(h.H,200.0);Eq(h.A,double.MaxValue);h.Assign(-double.MaxValue);Eq(h.H,0.0);Eq(h.A,-double.MaxValue);});
    }
    private static void CalibrationCases()
    {
        Add("C01 initial rate and independent HP0 acceptance anchor",()=>{
            var c=new HealthCalibration();Eq(c.Rate,.05);Eq(c.B,1.0);Eq(c.G,1.0);
            var chart=Chart(0,1200);for(int i=0;i<61;i++){Eq(c.Trial(chart),HealthRetry.PreObject);Eq(c.Trials,i+1);Eq(c.B,1.0);Eq(c.G,1.0);Yes(double.IsNaN(c.Snapshots[0]),"no successful object snapshot yet");}
            Near(c.Rate,.004144911095285020,1e-17);Eq(c.Trial(chart),HealthRetry.None);Eq(c.Trials,62);
            Near(c.LastA,215.02610668565798);Eq(c.LastH,200.0);Eq(c.Snapshots[0],200.0);Near(c.LastMean,15.02610668565798);
        });
        Add("C02 accepted calibration reset is not accepted balance",()=>{var c=new HealthCalibration();var chart=Chart(0,1200);var p=c.Run(chart);Eq(p.PreRetries,61);Eq(p.GroupRetries,0);Eq(p.EndRetries,0);Eq(p.BalanceRetries,0);var live=new OrdinaryHealth(chart,p);Eq(live.State.H,200.0);Eq(live.State.A,200.0);Eq(live.State.V,0.0);Eq(live.State.Rate,p.Rate);Eq(live.ResultEvents,0);});
        foreach(var v in new[]{(0.0,100,195.0),(2.5,450,177.5),(5.0,800,160.0),(7.5,1800,110.0),(10.0,2800,60.0)})
            Add($"C03 pre-threshold equality HP{v.Item1}",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(v.Item1,v.Item2)),HealthRetry.PreObject);Eq(c.LastH,v.Item3);Eq(c.Rate,.048);Eq(c.B,1.0);Eq(c.G,1.0);});
        Add("C04 HP5 approach consumed before any group",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(5,1200)),HealthRetry.PreObject);Eq(c.LastH,140.0);Eq(c.LastA,140.0);Eq(c.LastMean,-60.0);});
        Add("C05 HP10 balance retry independent arithmetic",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(10,1200)),HealthRetry.Balance);Eq(c.LastH,160.0);Eq(c.LastA,160.0);Eq(c.LastMean,-40.0);Eq(c.Rate,.048);Eq(c.B,1.02);Eq(c.G,1.01);});
        Add("C06 slider subset versus schedule counts",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(10,0,[S(2000,2100,1,3)])),HealthRetry.None);Eq(c.LastH,200.0);Eq(c.LastA,230.0);Eq(c.Snapshots[0],200.0);}); // 200-5+3*4+1*3+20
        Add("C07 spinner prepared N notifications not live observed C",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(10,0,[new(2000,2100,ScoreObjectKind.Spinner,false,SpinnerRequirement:4)])),HealthRetry.None);Near(c.LastA,221.8);Eq(c.LastH,200.0);});
        foreach(bool group in new[]{false,true})
            Add($"C08 next prepared NewCombo selects1028 {group}",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(10,0,[C(2000),C(2100,group)])),HealthRetry.None);Eq(c.LastA,group?235.0:221.0);Eq(c.LastMean,group?17.5:10.5);Eq(c.Snapshots[0],200.0);});
        Add("C09 duration overflow safety occurs after components",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(10,0,[S(2000,10200)])),HealthRetry.DurationOverflow);Eq(c.LastH,8.0);Eq(c.LastA,8.0);Eq(c.Rate,.048);Eq(c.B,1.0);Yes(double.IsNaN(c.Snapshots[0]),"snapshot not committed");});
        Add("C10 final target retry retains completed snapshot",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(5,0,[S(2000,4200)])),HealthRetry.FinalTarget);Eq(c.LastH,118.0);Eq(c.LastA,118.0);Eq(c.Snapshots[0],118.0);Eq(c.Rate,.047);Eq(c.B,1.01);Eq(c.G,1.01);});
        Add("C11 third nonconsecutive low group retains rate",()=>{
            var chart=Chart(10,0,[S(1000,4000),C(4001),S(4002,5002),C(5003),S(5004,5904)]);
            var c=new HealthCalibration();Eq(c.Trial(chart),HealthRetry.ThirdLowGroup);Eq(c.Rate,.05);Eq(c.B,1.07);Eq(c.G,1.03);
            Near(c.LastH,78.8);Near(c.Snapshots[0],78);Near(c.Snapshots[1],97.95);Near(c.Snapshots[2],75.9);Near(c.Snapshots[3],95.85);Yes(double.IsNaN(c.Snapshots[4]),"third low group no snapshot");
            c.Trial(chart);Yes(c.LastH>78.8,"new trial must use retained gains, not constructor1");Yes(c.B>=1.07&&c.G>=1.03,"coefficients retained");
        });
        foreach(var v in new[]{(7,1000,50.0),(8,2000,0.0),(14,2000,0.0)})
            Add($"C12 break leading-gap treatment format{v.Item1}",()=>{var c=new HealthCalibration();c.Trial(Chart(10,0,[C(1000),C(3000)],[new(2000,3000)],v.Item1));Eq(c.LastExcludedGap,v.Item2);Eq(c.LastGapLoss,v.Item3);Eq(c.LastBreaksConsumed,1);});
        Add("C13 two breaks in one gap are not unioned",()=>{var c=new HealthCalibration();c.Trial(Chart(10,0,[C(1000),C(6000)],[new(2000,3000),new(4000,5000)]));Eq(c.LastExcludedGap,2000);Eq(c.LastGapLoss,150.0);Eq(c.LastBreaksConsumed,1);});
        Add("C14 gap measured from preceding end",()=>{var c=new HealthCalibration();c.Trial(Chart(10,0,[S(1000,1100),C(1200)]));Eq(c.LastGapLoss,5.0);});
        Add("C15 rejected unordered objects are not sorted",()=>Reject<InvalidDataException>(()=>new HealthCalibration().Run(Chart(0,0,[C(3000),C(2000)]))));
        Add("C16 explicit retry guard does not assign fallback",()=>{var c=new HealthCalibration();Reject<InvalidDataException>(()=>c.Run(Chart(0,1200),1));Eq(c.Trials,1);Eq(c.Rate,.048);});
        Add("C17 malformed counts and empty chart rejected",()=>{foreach(var chart in new[]{Chart(objects:[]),Chart(objects:[S(1000,2000,2,1)]),Chart(objects:[new(1000,2000,ScoreObjectKind.Spinner,false,SpinnerRequirement:-1)]),Chart(double.NaN)})Reject<InvalidDataException>(()=>new HealthCalibration().Run(chart));});
        foreach(var v in new[]{(960,HealthRetry.Balance,180.0),(961,HealthRetry.FinalTarget,179.95)})
            Add($"C19 final-target boundary duration{v.Item1}",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(5,0,[S(2000,2000+v.Item1)])),v.Item2);Near(c.LastH,v.Item3);});
        foreach(var v in new[]{(2960,HealthRetry.FinalTarget),(2961,HealthRetry.ThirdLowGroup)})
            Add($"C20 group equality excludes first low occurrence duration{v.Item1}",()=>{int e=1000+v.Item1;var c=new HealthCalibration();Eq(c.Trial(Chart(10,0,[S(1000,e),S(e+1,e+561),S(e+562,e+1122)])),v.Item2);});
        foreach(var v in new[]{(400,HealthRetry.None,0.0),(401,HealthRetry.Balance,-.05)})
            Add($"C21 balance equality approach{v.Item1}",()=>{var c=new HealthCalibration();Eq(c.Trial(Chart(10,v.Item1)),v.Item2);Near(c.LastMean,v.Item3);});
        Add("C18 calibrator cannot silently replace its prepared chart",()=>{var c=new HealthCalibration();c.Trial(Chart(0,1200));Reject<InvalidDataException>(()=>c.Trial(Chart(0,1200)));});
    }
    private static void ResultCases()
    {
        // Literal E035 M0ad2 branch table, not production Phi/switch as expected oracle.
        var rows=new (int Raw,double H0,double H5,double H10)[]{(1024,6,6,6),(512,17.6,2.2,2.2),(256,3.2,.4,.4),(-131072,-6,-25,-40),(64,4,4,4),(8,3,3,3),(128,4,4,4),(-262144,-4,-15,-28),(-524288,-4,-15,-28),(4096,1.7,1.7,1.7),(8192,1.7,1.7,1.7),(16384,2,2,2)};
        foreach(var row in rows)foreach(int hp in new[]{0,5,10})
            Add($"J01 raw{row.Raw} HP{hp} independent HP table",()=>{var h=Live(0,hp,g:2.5);h.State.Assign(100);h.State.SetPresentation(37);double expected=hp==0?row.H0:hp==5?row.H5:row.H10;if(row.Raw>0)expected*=2.5;h.ApplyResultHp(row.Raw,true);Near(h.State.H,100+expected);Near(h.State.A,100+expected);Eq(h.State.V,37.0);Eq(h.ResultEvents,1);});
        Add("J02 intermediate HP2.5 whole losses and gains",()=>{var h=new HealthState(0,7,2);Near(h.PlainDelta(512,2.5),19.8);Near(h.PlainDelta(256,2.5),3.6);Near(h.PlainDelta(-131072,2.5),-15.5);Near(h.PlainDelta(-262144,7.5),-21.5);});
        Add("J03 calibration1028 uses independent B and G",()=>{var h=new HealthState(0,2,3);h.Assign(100);h.ApplyCalibration(1028,5);Eq(h.H,146.0);Eq(h.A,146.0);});
        Add("J04 calibration plain1024 excludes B",()=>{var h=new HealthState(0,9,2);h.Assign(100);h.ApplyCalibration(1024,5);Eq(h.H,112.0);});
        Add("J05 raw0 does not clear fill or emit HP event",()=>{var h=Live();h.State.Assign(100);h.ApplyResultHp(0,true);Eq(h.State.H,100.0);Eq(h.State.Fill,true);Eq(h.ResultEvents,0);Eq(h.LastFailure,HealthFailure.NotChecked);});
        Add("J06 unsupported live decoration explicitly rejected",()=>{var h=Live();h.State.Assign(100);foreach(int raw in new[]{257,513,1025,514,1026,1028})Reject<InvalidDataException>(()=>h.ApplyResultHp(raw,true));Eq(h.State.H,100.0);Eq(h.ResultEvents,0);});
        Add("J07 score suppression does not suppress HP hook",()=>{var l=new ScoreLedger(ScoreConfiguration.EngineeringPractice);var h=Live();h.State.Assign(100);l.Apply(Event(ScoreEventKind.CircleWhole,1024,enabled:false),()=>{Eq(l.CurrentCombo,1);Eq(l.Score,0);h.ApplyResultHp(1024,true);});Eq(l.Score,0);Eq(l.N300,(ushort)0);Eq(h.State.H,106.0);});
        Add("J08 terminal versus interior miss combo separation",()=>{var l=new ScoreLedger(ScoreConfiguration.EngineeringPractice);var h=Live();h.State.Assign(100);l.Apply(Event(ScoreEventKind.SliderHead,64),()=>h.ApplyResultHp(64,true));l.Apply(Event(ScoreEventKind.SliderRepeatOrTerminal,-524288,SliderCheckpointKind.Final),()=>h.ApplyResultHp(-524288,true));Eq(l.CurrentCombo,1);Eq(h.State.H,89.0);l.Apply(Event(ScoreEventKind.SliderInterior,-262144,SliderCheckpointKind.Interior),()=>h.ApplyResultHp(-262144,true));Eq(l.CurrentCombo,0);Eq(h.State.H,74.0);});
        Add("J09 spinner components and final rewards are separate",()=>{var l=new ScoreLedger(ScoreConfiguration.EngineeringPractice);var h=Live();h.State.Assign(100);foreach(int raw in new[]{4096,8192,16384})l.Apply(Event(ScoreEventKind.SpinnerProgress,raw),()=>h.ApplyResultHp(raw,true));Eq(l.CurrentCombo,0);Near(h.State.H,105.4);l.Apply(Event(ScoreEventKind.SpinnerFinal,1024),()=>h.ApplyResultHp(1024,true));Near(h.State.H,111.4);Eq(l.CurrentCombo,1);Eq(l.N300,(ushort)1);Eq(h.ResultEvents,4);});
        Add("J10 HP exception leaves earlier combo but prevents score continuation",()=>{var l=new ScoreLedger(ScoreConfiguration.EngineeringPractice);Reject<InvalidOperationException>(()=>l.Apply(Event(ScoreEventKind.CircleWhole,1024),()=>throw new InvalidOperationException("intentional test callback")));Eq(l.CurrentCombo,1);Eq(l.N300,(ushort)1);Eq(l.Score,0);Eq(l.EventCount,0);});
    }
    private static void DrainCases()
    {
        foreach(var v in new[]{(1010,1000,false,.5),(1010,1000,true,.125),(1000,1000,false,0.0),(900,1000,false,-5.0),(100000,1000,false,4950.0)})
            Add($"D01 consumed T{v.Item1} prev{v.Item2} spinner{v.Item3}",()=>{var h=Live();h.State.Assign(199);h.Frame(Frame(1000,1000));Eq(h.State.H,199.0);h.Frame(Frame(v.Item1,v.Item2,v.Item3));Eq(h.LastPassiveLoss,v.Item4);Eq(h.State.H,Math.Max(0,199-v.Item4));Eq(h.State.A,Math.Max(0,199-v.Item4));Eq(h.Failed,false);});
        Add("D02 signed subtraction wraps before Double conversion",()=>{var h=Live();h.State.Assign(100);h.Frame(Frame(1,1));h.Frame(Frame(int.MinValue,int.MaxValue));Eq(h.LastPassiveLoss,.05);Eq(h.State.H,99.95);});
        Add("D03 late eligibility is not retroactive",()=>{var h=Live();h.State.Assign(100);h.Frame(Frame(1000,990));Eq(h.State.H,100.0);var f=Frame(1010,1000) with {LateEligibility=Eligible(1010) with {Marker=0}};h.Frame(f);Eq(h.State.H,99.5);Eq(h.Eligible,false);h.Frame(Frame(1020,1010));Eq(h.State.H,99.5);Eq(h.Eligible,true);});
        Add("D04 repeated reached pass does not consume delta marker",()=>{var h=Live();h.Frame(Frame(1000,1000));h.Frame(Frame(1010,1000));h.Frame(Frame(1010,1000));Eq(h.State.H,199.0);});
        Add("D05 zero dt still clears fill on eligible subtract",()=>{var h=Live();h.Frame(Frame(1000,1000));h.Frame(Frame(1000,1000) with {ClearFill=true});Eq(h.State.Fill,false);Eq(h.State.A,200.0);});
        Add("D06 pause suppresses drain smoothing and late state writes",()=>{var h=Live();h.State.Assign(100);h.State.SetPresentation(20);h.Frame(Frame(1000,1000));h.Frame(Frame(2000,1000) with {Paused=true,CorrectedDelta=1000,HostElapsed=1000,HostScale=9,LateEligibility=Eligible(2000) with {ClockState=0}});Eq(h.State.H,100.0);Eq(h.State.V,20.0);Eq(h.Eligible,true);h.Frame(Frame(1000,1000));Eq(h.State.H,100.0);h.Frame(Frame(1020,1000));Eq(h.State.H,99.0);});
        Add("D07 gameplay and host operands remain separate",()=>{var h=Live(.01);h.Frame(Frame(1000,1000));h.Frame(Frame(1010,1000) with {HostElapsed=900,HostScale=0,CorrectedDelta=0});Eq(h.LastPassiveLoss,.1);Eq(h.State.V,0.0);h.Frame(Frame(1010,1010) with {CorrectedDelta=10});Eq(h.LastPassiveLoss,0.0);Eq(h.State.V,1.0);}); // first-start2000 -> fill slope .1
        var basis=new HealthEligibility(800,799,2000,8000,1200,-1,1,14,3000);
        foreach(var v in new (string,HealthEligibility,bool)[]{("lower before",basis with {Time=799},false),("lower equal",basis,true),("upper equal",basis with {Time=8000,QueueHead=null},true),("upper after",basis with {Time=8001,QueueHead=null},false),("queue equal",basis with {Time=3000},true),("queue after",basis with {Time=3001},false),("old queue after",basis with {Time=3001,Format=7},true),("marker zero",basis with {Marker=0},false),("clock zero",basis with {ClockState=0},false),("clock two",basis with {ClockState=2},true),("special equality",basis with {Special=true,SpecialUntil=799},false),("special after",basis with {Special=true,SpecialUntil=798},true),("recorded equality",basis with {Recorded=true,RecordedUntil=799},false),("recorded before",basis with {Recorded=true,RecordedUntil=798},true)})
            Add("D08 eligibility "+v.Item1,()=>Eq(v.Item2.Evaluate(),v.Item3));
        Add("D09 inclusive break marker and candidate equality",()=>{var chart=Chart(0,0,[C(1000),C(3000),C(8000)],[new(4000,6000)]);var h=Live(chart:chart);Eq(h.QueueHead,(int?)3000);h.UpdateBreakState(3000,[]);Eq(h.QueueHead,(int?)3000);h.UpdateBreakState(3001,[]);Eq(h.Marker,-1);Eq(h.QueueHead,(int?)3000);h.UpdateBreakState(4000,[]);Eq(h.Marker,4000);Eq(h.QueueHead,(int?)null);h.UpdateBreakState(6000,[]);Eq(h.Marker,6000);h.UpdateBreakState(6001,[5999]);Eq(h.Marker,6000);h.UpdateBreakState(8000,[8000]);Eq(h.Marker,-1);});
        Add("D10 break entry consumes prior eligible then disables next drain",()=>{var chart=Chart(0,0,[C(1000),C(3000),C(8000)],[new(4000,6000)]);var h=Live(chart:chart);h.FrameWithPreparedCandidates(Frame(3000,3000));Eq(h.Eligible,true);h.FrameWithPreparedCandidates(Frame(4000,3000));Eq(h.State.H,150.0);Eq(h.Eligible,false);h.FrameWithPreparedCandidates(Frame(5000,4000));Eq(h.State.H,150.0);h.FrameWithPreparedCandidates(Frame(8000,5000));Eq(h.Eligible,true);Eq(h.State.H,150.0);});
    }
    private static void SmoothFailureCases()
    {
        Add("V01 ordinary rise independent of milliseconds",()=>{var h=new HealthState();h.Assign(200);h.SetPresentation(100);h.ConfigureFill(0,.02,false);h.Smooth(1,999,999,1,1,false);Eq(h.V,125.0);Eq(h.H,200.0);Eq(h.A,200.0);});
        Add("V02 fall clears fill and uses sixth",()=>{var h=new HealthState();h.Assign(40);h.SetPresentation(100);h.Smooth(1,0,0,1,0,false);Eq(h.V,90.0);Eq(h.Fill,false);Eq(h.H,40.0);});
        Add("V03 fill timestamp equality then normal source",()=>{var h=new HealthState();h.Reset();h.ConfigureFill(100,.03,true);h.Smooth(100,50,1000,10,1,false);Eq(h.V,0.0);h.Smooth(101,50,1000,10,1,false);Eq(h.V,1.5);});
        Add("V04 clock guard versus recovery host elapsed",()=>{var h=new HealthState();h.Reset();h.ConfigureFill(0,.02,true);h.Smooth(1,20,100,1,0,false);Eq(h.V,0.0);h.Smooth(1,20,100,1,2,false);Eq(h.V,0.0);h.Smooth(1,20,100,1,0,true);Eq(h.V,2.0);});
        Add("V05 exponential rise overshoots target",()=>{var h=new HealthState();h.Assign(110);h.SetPresentation(100);h.ConfigureFill(0,.02,false);h.Smooth(1,0,0,8,0,false);Eq(h.V,120.0);Eq(h.H,110.0);});
        Add("V06 fall overshoots target but lower clamps zero",()=>{var h=new HealthState();h.Assign(90);h.SetPresentation(100);h.Smooth(1,0,0,12,1,false);Eq(h.V,80.0);h.Assign(0);h.Smooth(2,0,0,12,1,false);Eq(h.V,0.0);});
        Add("V07 fill can overshoot H but caps200",()=>{var h=new HealthState();h.Assign(10);h.ConfigureFill(0,.5,true);h.Smooth(1,100,1,1,1,false);Eq(h.V,50.0);h.Assign(200);h.Smooth(2,1000,1,1,1,false);Eq(h.V,200.0);});
        Add("V08 equality does not clear fill",()=>{var h=new HealthState();h.Assign(100);h.SetPresentation(100);h.Smooth(1,1000,1000,10,1,false);Eq(h.V,100.0);Eq(h.Fill,true);});
        Add("F01 target zero fails while V positive",()=>{var h=Live();h.State.Assign(0);h.State.SetPresentation(100);Eq(h.CheckFailure(123),HealthFailure.Latched);Eq(h.Failed,true);Eq(h.FailureTime,(int?)123);Eq(h.State.V,100.0);Eq(h.CheckFailure(456),HealthFailure.AlreadyLatched);Eq(h.FailureTime,(int?)123);});
        Add("F02 positive target ignores zero V",()=>{var h=Live();h.State.Assign(double.Epsilon);h.State.SetPresentation(0);Eq(h.CheckFailure(1),HealthFailure.Healthy);Eq(h.Failed,false);});
        foreach(var gate in new[]{new HealthFailureGates(NoFail:true),new HealthFailureGates(Relax:true),new HealthFailureGates(Relax2:true),new HealthFailureGates(Recorded:true),new HealthFailureGates(Special:true)})
            Add("F03 suppression "+gate,()=>{var h=Live();h.State.Assign(0);Eq(h.CheckFailure(1,gate),HealthFailure.Suppressed);Eq(h.Failed,false);Eq(h.State.H,0.0);});
        Add("F04 recorded continuation explicitly permits failure",()=>{var h=Live();h.State.Assign(0);Eq(h.CheckFailure(1,new(Recorded:true,RecordedContinuation:true)),HealthFailure.Latched);});
        Add("F05 result heal before failure read",()=>{var h=Session(Map("256,192,2000,1,0"));h.Health!.State.Assign(0);h.Press(new(256,192),2000);Eq(h.Health.State.H,6.0);Eq(h.Health.LastFailure,HealthFailure.Healthy);Eq(h.Health.Failed,false);});
        Add("F06 result loss before failure read",()=>{var h=Session(Map("256,192,2000,1,0"));h.Health!.State.Assign(1);h.Health.State.SetPresentation(100);h.Advance(2151);Eq(h.Health.State.H,0.0);Eq(h.Health.Failed,true);Eq(h.Health.FailureTime,(int?)2151);Eq(h.Ledger.NMiss,(ushort)1);});
        Add("F07 passive zero does not invent eager failure",()=>{var h=Live();h.State.Assign(1);h.Frame(Frame(1000,1000));h.Frame(Frame(2000,1000));Eq(h.State.H,0.0);Eq(h.Failed,false);Eq(h.LastFailure,HealthFailure.NotChecked);h.ApplyResultHp(1024,true);Eq(h.CheckFailure(2000),HealthFailure.Healthy);Eq(h.State.H,6.0);});
        Add("F08 recovery precedes already-failed gate",()=>{var h=Live();h.State.Assign(0);h.CheckFailure(1);h.SupplyRecoveryCount(1);Eq(h.CheckFailure(2),HealthFailure.Recovery);Eq(h.State.H,160.0);Eq(h.State.A,160.0);Eq(h.State.Fill,true);Eq(h.State.FillRate,.07999999821186066);Eq(h.RecoveryCount,0);Eq(h.FailureTime,(int?)1);Eq(h.Failed,true);});
        Add("F09 recovery adds160 rather than assigns160",()=>{var h=Live();h.State.Assign(100);h.SupplyRecoveryCount(2);h.BeginRecovery();Eq(h.State.H,200.0);Eq(h.State.A,260.0);Eq(h.RecoveryCount,1);Eq(h.Recovering,true);});
        Add("F10 recovery release uses strict V target margin",()=>{var h=Live();h.State.Assign(0);h.SupplyRecoveryCount(1);h.CheckFailure(1);h.State.SetPresentation(155);h.Frame(Frame(1,1));Eq(h.Recovering,true);h.State.SetPresentation(156);h.Frame(Frame(1,1));Eq(h.Recovering,false);});
        Add("F11 state reset does not reset separate failure latch",()=>{var h=Live();h.State.Assign(0);h.CheckFailure(1);h.State.Reset();Eq(h.Failed,true);Eq(h.State.H,200.0);Eq(h.CheckFailure(2),HealthFailure.Healthy);Eq(h.FailureTime,(int?)1);});
        foreach(double target in new[]{160.0,200.0})
            Add($"F13 original recovery guard retains fill at H{target}",()=>{var h=Live();h.State.Assign(target);h.State.ConfigureFill(123,.03,false);h.SupplyRecoveryCount(1);h.BeginRecovery();Console.WriteLine($"OBS recovery H={h.State.H:R} A={h.State.A:R} fill={h.State.Fill} slope={h.State.FillRate:R} from={h.State.FillFrom} recovering={h.Recovering} count={h.RecoveryCount}");Eq(h.State.H,target);Eq(h.State.A,target);Eq(h.State.Fill,false);Eq(h.State.FillRate,.03);Eq(h.State.FillFrom,123);Eq(h.Recovering,true);Eq(h.RecoveryCount,0);});
        Add("F12 recovery count and missing count reject",()=>{var h=Live();Reject<ArgumentOutOfRangeException>(()=>h.SupplyRecoveryCount(-1));Reject<InvalidOperationException>(()=>h.BeginRecovery());});
    }
    private static void IntegrationCases()
    {
        Add("I01 fresh supported map calibration creates no ledger events",()=>{var s=Session(Fixture("health-r09"));Yes(s.Health is not null,"supported explicit groups");Eq(s.Health!.ResultEvents,0);Eq(s.Ledger.EventCount,0);Eq(s.Ledger.Score,0);Eq(s.Ledger.CurrentCombo,0);Eq(s.Ledger.TotalWholeResults,0);Eq(s.Health.State.A,200.0);Eq(s.Health.State.V,0.0);});
        Add("I02 parser-derived slider counts and spinner requirement",()=>{var m=Map("128,192,2000,6,0,L|428:192,2,300\n256,192,9000,8,0,12000");var chart=HealthChart.FromMap(m,1200);Eq(chart.Objects[0].ScheduleCount,6);Eq(chart.Objects[0].RepeatSubsetCount,1);Eq(chart.Objects[1].SpinnerRequirement,15);Eq(chart.Objects[0].End,8000);}); // beat1000, length300, speed.1 -> 2*3000
        Add("I03 supported first unknown group does not require M2902",()=>{var s=Session(Map("256,192,2000,1,0"));Yes(s.Health is not null,"first NewCombo unused by calibration");Eq(s.Health!.Calibration.Trials,62);});
        Add("I04 following unresolved NewCombo stays playable and scorable",()=>{var m=Map("128,192,2000,1,0\n256,192,4000,1,0");var s=Session(m);Eq(s.Health,(OrdinaryHealth?)null);Yes(s.HealthStatus.Contains("M2902"),"explicit dependency");s.Press(new(128,192),2000);s.Press(new(256,192),4000);Eq(s.Ledger.N300,(ushort)2);Eq(s.Combo,2);Eq(s.Accuracy,100.0);Yes(s.Ledger.Score>0,"no fallback health must not suppress score");});
        Add("I05 unavailable and disabled health preserve identical events",()=>{var m=Map("128,192,2000,1,0\n256,192,4000,1,0");var a=Session(m);var b=Session(m,false);a.Press(new(128,192),2000);b.Press(new(128,192),2000);a.Advance(4151);b.Advance(4151);SameScore(a,b);Eq(a.Circles[1].Result,Judgement.Miss);});
        Add("I06 explicit groups establish different supported admission",()=>{var s=Session(Map("128,192,2000,1,0\n256,192,4000,5,0"));Yes(s.Health is not null,"bit4 proves next group");});
        Add("I07 fresh restart isolates HP failure progress and ledger",()=>{var m=Map("256,192,2000,1,0");var a=Session(m);a.Health!.State.Assign(0);a.Health.CheckFailure(1);a.Press(new(256,192),2000);var b=Session(m);Yes(!ReferenceEquals(a.Health,b.Health),"new health instance");Eq(b.Health!.Failed,false);Eq(b.Health.State.H,200.0);Eq(b.Health.ResultEvents,0);Eq(b.Ledger.Score,0);Eq(b.Circles[0].Result,Judgement.Pending);});
        Add("I08 supported-unavailable-supported replacement isolates state",()=>{var supported=Map("256,192,2000,1,0");var unknown=Map("128,192,2000,1,0\n256,192,4000,1,0");var a=Session(supported);a.Health!.State.Assign(1);var b=Session(unknown);Eq(b.Health,(OrdinaryHealth?)null);var c=Session(supported);Eq(c.Health!.State.H,200.0);Eq(c.Ledger.Score,0);});
        Add("I09 missing health metadata is explicit not fallback",()=>{var m=new Beatmap("synthetic",5,5,5,[new(new(256,192),2000)]);var s=new CircleSession(m);Eq(s.Health,(OrdinaryHealth?)null);Yes(s.HealthStatus.Contains("Missing"),"missing provenance");s.Press(new(256,192),2000);Eq(s.Combo,1);});
        Add("I10 break metadata and original input order remain separate",()=>{var m=Map("256,192,4000,5,0\n128,192,2000,5,0");var s=Session(m);Eq(s.Health,(OrdinaryHealth?)null);Yes(s.HealthStatus.Contains("ordered"),"do not repair source order");Eq(m.Circles[0].StartTime,2000);});
        Add("I11 private trials preserve prepared gameplay state",()=>{var m=Fixture("health-r09");var a=Session(m);var b=Session(m);Eq(a.Ledger.EventCount,0);foreach(var c in a.Heads)Eq(c.Result,Judgement.Pending);foreach(var s in a.Sliders){Eq(s.ProcessedCheckpoints,0);Eq(s.Tracking,false);}foreach(var s in a.Spinners){Eq(s.Progress,0f);Eq(s.Observation.Count,0);}Eq(a.Health!.State.Rate,b.Health!.State.Rate);});
        Add("I12 once-only slider terminal HP without duplicate combo",()=>{
            var m=Map("128,192,2000,2,0,L|428:192,1,300");var a=Session(m);var b=Session(m,false);a.Press(new(128,192),2000);b.Press(new(128,192),2000);
            a.Advance(2000,new(128,192),1);b.Advance(2000,new(128,192),1);
            foreach(int t in new[]{3000,4000}){a.Advance(t,m.Sliders[0].RequiredPosition(t),1);b.Advance(t,m.Sliders[0].RequiredPosition(t),1);}
            int combo=a.Combo;a.Health!.State.Assign(100);a.Advance(4964,new(float.NaN,float.NaN),0);b.Advance(4964,new(float.NaN,float.NaN),0);
            Eq(a.Sliders[0].Checkpoints[^1].RawResult,-524288);Eq(a.Combo,combo);Near(a.Health.State.H,96-a.Health.State.Rate*964);
            int events=a.Health.ResultEvents;a.Advance(4964);Eq(a.Health.ResultEvents,events);a.Advance(5000);b.Advance(5000);Eq(a.Sliders[0].Result,Judgement.Hit100);Eq(a.Combo,combo);SameScore(a,b);Eq(a.Health.ResultEvents,a.Ledger.EventCount);
        });
        Add("I13 full mixed fixture HP enable does not change scoring or judgements",()=>{
            var map=Fixture("health-r09");var a=Session(map);var b=Session(map,false);
            for(int t=0;t<=18000;t+=20){Vector2 p=new(256,192);int held=0;var slider=map.Sliders.Single();if(t>=slider.Head.StartTime&&t<=slider.EndTime){p=slider.RequiredPosition(t);held=1;}
                foreach(var c in map.Circles.Where(c=>c.StartTime==t)){a.Press(c.Position,t);b.Press(c.Position,t);}if(t==slider.Head.StartTime){a.Press(p,t);b.Press(p,t);}a.Advance(t,p,held);b.Advance(t,p,held);SameScore(a,b);}
            Eq(a.Health!.ResultEvents,a.Ledger.EventCount);Eq(a.JudgedCount,5);Eq(a.Spinners[0].Result,Judgement.Miss);int n=a.Health.ResultEvents;a.Advance(20000);Eq(a.Health.ResultEvents,n);
        });
        Add("I14 judgement HP survives caps but never duplicates a press",()=>{var a=Session(Map("256,192,2000,1,0"));a.Press(new(256,192),2000);a.Press(new(256,192),2000);Eq(a.Health!.State.H,200.0);Eq(a.Health.State.A,206.0);Eq(a.Health.ResultEvents,1);Eq(a.Ledger.EventCount,1);Eq(a.Ledger.N300,(ushort)1);});
        Add("I15 unavailable calibration rejects unknown groups directly",()=>{var chart=Chart(0,0,[C(2000),C(4000,null)]);Reject<InvalidDataException>(()=>new HealthCalibration().Run(chart));});
        Add("I16 supported calibration validates break domain without repair",()=>{foreach(var b in new[]{new HealthBreak(3000,2500),new HealthBreak(1000,3000),new HealthBreak(2500,5000)})Reject<InvalidDataException>(()=>new HealthCalibration().Run(Chart(0,0,[C(2000),C(4000)],[b])));});
    }
    private static void SameScore(CircleSession a, CircleSession b)
    {
        Eq(a.Ledger.Score,b.Ledger.Score);Eq(a.Combo,b.Combo);Eq(a.MaxCombo,b.MaxCombo);Eq(a.Ledger.N300,b.Ledger.N300);Eq(a.Ledger.N100,b.Ledger.N100);Eq(a.Ledger.N50,b.Ledger.N50);Eq(a.Ledger.NMiss,b.Ledger.NMiss);Eq(a.Accuracy,b.Accuracy);Eq(a.JudgedCount,b.JudgedCount);Eq(a.Ledger.EventCount,b.Ledger.EventCount);Yes(a.ObjectResults.SequenceEqual(b.ObjectResults),"health must not change judgement");
    }
}
