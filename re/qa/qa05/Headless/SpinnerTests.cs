// Linked production components, finite explicit arguments; no application or OS input.
using System.Numerics;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA05;
public static class SpinnerTests
{
    private static readonly List<(string Name,Action Body)> Cases=[];
    private static string root="", scratch=""; private static int file;
    private static SpinnerState Fresh(int duration=3000,float od=5)=>new(new SpinnerDefinition(2000,2000+duration,0,null),od);
    private static void Eq<T>(T a,T b){if(!EqualityComparer<T>.Default.Equals(a,b))throw new InvalidOperationException($"expected={b}; actual={a}");}
    private static void Near(double a,double b,double eps=1e-12){if(!double.IsFinite(a)||Math.Abs(a-b)>eps)throw new InvalidOperationException($"expected near {b}; actual={a}");}
    private static void Yes(bool b,string why){if(!b)throw new InvalidOperationException(why);}
    private static void Reject(Action a){try{a();}catch(InvalidDataException){return;}throw new InvalidOperationException("expected InvalidDataException");}
    private static void Add(string n,Action a)=>Cases.Add((n,a));
    private static SpinnerState Moving(double theta=1){var s=Fresh();s.SampleAngle(0,2000,0,true);s.SampleAngle(theta,2001,0,true);return s;}
    private static void SeedCount(SpinnerState s,int count){for(int i=1;i<=count;i++)s.Observation.Observe(i,s.Required);}
    private static Beatmap Parse(string rows="256,192,2000,8,0,5000",int version=14)
    {
        string p=Path.Combine(scratch,$"{file++}.osu");
        File.WriteAllText(p,$"osu file format v{version}\n[General]\nMode:0\n[Difficulty]\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,1000,4,1,0,100,1,0\n[HitObjects]\n{rows}\n");return Beatmap.Load(p);
    }
    private static SpinnerState Spin16()
    {
        var s=Fresh();s.SampleAngle(0,2000,16,true);
        for(int i=1;i<=16;i++){s.SampleAngle(Math.Atan2(Math.Sin(i),Math.Cos(i)),2000+i*100,16,true);s.Integrate(2000+i*100,100);}
        return s;
    }
    public static int Main(string[] args)
    {
        if(args.Length!=1)throw new ArgumentException("Pass repository root.");root=Path.GetFullPath(args[0]);scratch=Path.Combine(Path.GetTempPath(),"ohno-qa05-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        try{Register();int failed=0;foreach(var c in Cases){try{c.Body();Console.WriteLine("PASS "+c.Name);}catch(Exception e){failed++;Console.WriteLine($"FAIL {c.Name}: {e.GetType().Name}: {e.Message}");}}
            Console.WriteLine($"QA05 SPINNER TESTS: {Cases.Count-failed} passed, {failed} failed, {Cases.Count} executed");return failed==0?0:1;
        }finally{Directory.Delete(scratch,true);}
    }
    private static void Register()
    {
        Add("A01 atan2 Y X with translated supplied center",()=>{var s=Fresh();s.SampleCursor(new(10,24),new(10,20),2000,0,true);Near(s.PreviousAngle!.Value,Math.PI/2);Eq(s.TargetMotion,0.0);});
        Add("A02 modern first sample establishes reference only",()=>{var s=Fresh();s.SampleAngle(-2,2000,0,true);Eq(s.PreviousAngle,-2.0);Eq(s.Progress,0f);Eq(s.Observation.Count,0);Eq(s.StationarySamples,1);});
        Add("A03 opposite signed motions give opposite targets",()=>{Near(Moving(.5).TargetMotion,.03);Near(Moving(-.5).TargetMotion,-.03);});
        Add("A04 wrap in negative angular crossing",()=>{var s=Fresh();s.SampleAngle(3,2000,0,true);s.SampleAngle(-3,2001,0,true);Near(s.TargetMotion,(2*Math.PI-6)/(50.0/3));});
        Add("A05 wrap in positive angular crossing",()=>{var s=Fresh();s.SampleAngle(-3,2000,0,true);s.SampleAngle(3,2001,0,true);Near(s.TargetMotion,-(2*Math.PI-6)/(50.0/3));});
        Add("A06 exact positive antipode rejected but reference advances",()=>{var s=Moving(Math.PI);Eq(s.TargetMotion,0.0);Eq(s.PreviousAngle,Math.PI);s.SampleAngle(Math.PI-.1,2002,0,true);Near(s.TargetMotion,-.006);});
        Add("A07 exact negative antipode rejected",()=>{var s=Moving(-Math.PI);Eq(s.TargetMotion,0.0);Eq(s.PreviousAngle,-Math.PI);});
        Add("A08 just inside antipodal magnitude accepted",()=>{var s=Moving(double.BitDecrement(Math.PI));Yes(s.TargetMotion>0,"inside antipode rejected");});
        Add("A09 plus pi to minus pi is zero wrapped motion",()=>{var s=Fresh();s.SampleAngle(Math.PI,2000,0,true);s.SampleAngle(-Math.PI,2001,0,true);Eq(s.TargetMotion,0.0);Eq(s.StationarySamples,2);});
        Add("A10 radius does not scale angular target",()=>{var a=Fresh();var b=Fresh();a.SampleCursor(new(1,0),Vector2.Zero,2000,0,true);b.SampleCursor(new(1000,0),Vector2.Zero,2000,0,true);a.SampleCursor(new(0,1),Vector2.Zero,2001,0,true);b.SampleCursor(new(0,1000),Vector2.Zero,2001,0,true);Eq(a.TargetMotion,b.TargetMotion);Near(a.TargetMotion,Math.PI/2/(50.0/3));});
        Add("A11 center atan2 has no invented dead zone",()=>{var s=Moving(.5);s.SampleCursor(Vector2.Zero,Vector2.Zero,2002,0,true);Eq(s.PreviousAngle,0.0);Near(s.TargetMotion,-.03);});
        Add("A12 released movement zeros target and updates reference",()=>{var s=Moving(.5);s.SampleAngle(1,2002,0,false);Eq(s.TargetMotion,0.0);Eq(s.PreviousAngle,1.0);s.SampleAngle(1.25,2003,0,true);Near(s.TargetMotion,.015);});
        Add("A13 stationary first decay then zero despite release and late time",()=>{var s=Moving(.5);s.SampleAngle(.5,6000,0,false);Near(s.TargetMotion,.01);Eq(s.StationarySamples,1);s.SampleAngle(.5,6001,0,false);Eq(s.TargetMotion,0.0);Eq(s.StationarySamples,2);});
        Add("A14 sample at exact start and end is eligible",()=>{var s=Fresh();s.SampleAngle(0,1999,0,true);s.SampleAngle(.5,2000,0,true);Near(s.TargetMotion,.03);s.SampleAngle(1,5000,0,true);Near(s.TargetMotion,.03);});
        Add("A15 movement outside live interval updates only reference target zero",()=>{var s=Fresh();s.SampleAngle(0,1900,0,true);s.SampleAngle(.5,1999,0,true);Eq(s.TargetMotion,0.0);Eq(s.PreviousAngle,.5);s.SampleAngle(1,5001,0,true);Eq(s.TargetMotion,0.0);Eq(s.PreviousAngle,1.0);});
        Add("A16 negative sample interval leaves sample history unchanged",()=>{var s=Moving(.5);double ema=s.IntervalEma;s.SampleAngle(1,2002,-1,false);Eq(s.PreviousAngle,.5);Eq(s.IntervalEma,ema);Near(s.TargetMotion,.03);});
        Add("B01 EMA first and second samples use real supplied interval",()=>{var s=Fresh();s.SampleAngle(0,2000,1,true);double e=.999*(50.0/3)+(1-.999);Near(s.IntervalEma,e);s.SampleAngle(.5,2020,20,true);double w=Math.Pow(.999,20);Near(s.IntervalEma,w*e+(1-w)*20);});
        Add("B02 zero interval keeps EMA and selects fixed divisor",()=>{var s=Moving(.5);Eq(s.IntervalEma,50.0/3);Near(s.TargetMotion,.03);});
        Add("B03 adaptive divisor uses sample interval after EMA threshold",()=>{var s=Fresh();s.SampleAngle(0,2000,100,true);Yes(s.IntervalEma>17.333332697550457,"adaptive threshold not reached");s.SampleAngle(.5,2100,100,true);Near(s.TargetMotion,.005);});
        Add("B04 adaptive zero divisor yields zero target",()=>{var s=Fresh();s.SampleAngle(0,2000,100,true);s.SampleAngle(.5,2001,0,true);Eq(s.TargetMotion,0.0);});
        Add("B05 acceleration and Single progress conversion",()=>{var s=Moving();s.Integrate(2001,1);Near(s.SmoothedMotion,.00108);Eq(s.Progress,Math.Abs((float)((double)(float).00108/Math.PI)));});
        Add("B06 both motion clamps and per-update progress cap",()=>{var a=Moving();var b=Moving(-1);a.Integrate(2100,100);b.Integrate(2100,100);Eq(a.SmoothedMotion,.05);Eq(b.SmoothedMotion,-.05);Eq(a.Progress,1f);Eq(a.Progress,b.Progress);});
        Add("B07 direction reversal decelerates without resetting progress",()=>{var s=Moving();s.Integrate(2100,100);s.SampleAngle(0,2101,0,true);s.Integrate(2101,1);Near(s.SmoothedMotion,.04892);Yes(s.Progress>1,"reversal erased progress");s.Integrate(2200,100);Eq(s.SmoothedMotion,-.05);});
        Add("B08 strict integration start end and positive interval",()=>{var s=Moving();foreach(var t in new[]{1999,2000,5000,5001})s.Integrate(t,100);s.Integrate(2100,0);s.Integrate(2100,-1);Eq(s.Progress,0f);Eq(s.SmoothedMotion,0.0);});
        Add("B09 released target still permits residual progress",()=>{var s=Moving();s.Integrate(2100,100);s.SampleAngle(1.2,2101,0,false);Eq(s.TargetMotion,0.0);s.Integrate(2101,1);Near(s.SmoothedMotion,.04892);Yes(s.Progress>1,"residual contribution missing");});
        Add("B10 sufficient deceleration can stop immediately",()=>{var s=Moving();s.Integrate(2001,1);float p=s.Progress;s.SampleAngle(1.1,2002,0,false);s.Integrate(2002,1);Eq(s.SmoothedMotion,0.0);Eq(s.Progress,p);});
        Add("C01 fractional progress leaves integer cache unchanged",()=>{var o=new SpinnerProgressObservation();Eq(o.Observe(float.BitDecrement(1f),15),0);Eq(o.Count,0);Eq(o.CachedInteger,0);});
        Add("C02 first exact integer change has no component result",()=>{var o=new SpinnerProgressObservation();Eq(o.Observe(1,15),0);Eq(o.Count,1);Eq(o.CachedInteger,1);});
        Add("C03 skipped integers count as one observed event",()=>{var o=new SpinnerProgressObservation();Eq(o.Observe(12,15),0);Eq(o.Count,1);Eq(o.Observe(50,15),8192);Eq(o.Count,2);Eq(o.CachedInteger,50);});
        Add("C04 ordinary changed observations alternate raw identities",()=>{var o=new SpinnerProgressObservation();Eq(o.Observe(1,15),0);Eq(o.Observe(2,15),8192);Eq(o.Observe(3,15),4096);Eq(o.Observe(4,15),8192);});
        Add("C05 bonus predicate begins only at qualifying even offset",()=>{var o=new SpinnerProgressObservation();for(int i=1;i<=18;i++)o.Observe(i,15);Eq(o.Observe(19,15),4096);Eq(o.Observe(20,15),16384);Eq(o.Observe(21,15),4096);Eq(o.Observe(22,15),16384);});
        Add("C06 unchanged integer never repeats event",()=>{var o=new SpinnerProgressObservation();o.Observe(1,15);o.Observe(2,15);Eq(o.Observe(2.9f,15),0);Eq(o.Observe(2,15),0);Eq(o.Count,2);Eq(o.CachedInteger,2);});
        Add("C07 inequality includes artificial downward integer observation",()=>{var o=new SpinnerProgressObservation();o.Observe(5,15);Eq(o.Observe(2,15),8192);Eq(o.Count,2);Eq(o.CachedInteger,2);});
        Add("D01 OD5 duration3000 prepares N15 and acceleration",()=>{var s=Fresh();Eq(s.Required,15);Near(s.Acceleration,.00108);});
        Add("D02 OD endpoints prepare distinct truncated requirements",()=>{Eq(Fresh(3000,0).Required,9);Eq(Fresh(3000,10).Required,22);Eq(Fresh(399,5).Required,1);});
        foreach(var pair in new[]{(0,-131072),(2,-131072),(3,256),(13,256),(14,512),(15,512),(16,1024)})
        {var q=pair;Add($"D03 N15 count {q.Item1} threshold",()=>Eq(SpinnerState.ClassifyModern(q.Item1,15),q.Item2));}
        Add("D04 zero requirement special and small N division",()=>{Eq(Fresh(1).Required,0);Eq(SpinnerState.ClassifyModern(0,0),1024);Eq(SpinnerState.ClassifyModern(0,3),256);Eq(SpinnerState.ClassifyModern(0,4),-131072);});
        Add("D05 exact end does not complete and later completion is once",()=>{var s=Fresh();Eq(s.Complete(5000),false);Eq(s.Result,Judgement.Pending);Eq(s.Complete(5001),true);Eq(s.FinalRaw,-131072);Eq(s.Complete(5002),false);});
        Add("D06 accepted progress can finish before last observation",()=>{var s=Spin16();Eq(s.Progress,16f);Eq(s.Observation.Count,15);Eq(s.Observation.CachedInteger,15);Eq(s.Complete(5001),true);Eq(s.FinalRaw,512);Eq(s.Observation.Count,15);Eq(s.Observation.CachedInteger,15);});
        Add("D07 explicit last sample changes C then final grade",()=>{var s=Spin16();s.SampleAngle(s.PreviousAngle!.Value,5000,16,false);Eq(s.Observation.Count,16);s.Complete(5001);Eq(s.FinalRaw,1024);});
        Add("D08 high P with one observation grades from C not P",()=>{var s=Fresh();s.Observation.Observe(100,15);s.Complete(5001);Eq(s.FinalRaw,-131072);Eq(s.Observation.Count,1);});
        Add("D09 evaluated spinner stops later integration",()=>{var s=Moving();s.Integrate(2100,100);s.Complete(5001);s.Integrate(2200,100);Eq(s.Progress,1f);});
        Add("E01 spinner collection excluded from heads and head press",()=>{var s=new CircleSession(Parse());Eq(s.Heads.Count,0);Eq(s.ObjectCount,1);s.Press(SpinnerState.DisplayCenter,2000);Eq(s.Spinners[0].Result,Judgement.Pending);Eq(s.JudgedCount,0);});
        Add("E02 live session components do not add combo",()=>{var s=new CircleSession(Parse());for(int i=0;i<=15;i++){double a=i;s.Advance(2000+i*100,SpinnerState.DisplayCenter+new Vector2((float)(100*Math.Cos(a)),(float)(100*Math.Sin(a))),1);}Yes(s.Spinners[0].Observation.Count>=2,"did not reach component observations");Eq(s.Combo,0);Eq(s.JudgedCount,0);});
        Add("E03 final success increments combo and accuracy only once",()=>{var s=new CircleSession(Parse());SeedCount(s.Spinners[0],16);s.Advance(5000);Eq(s.JudgedCount,0);s.Advance(5001);Eq(s.Spinners[0].FinalRaw,1024);Eq(s.Combo,1);Eq(s.JudgedCount,1);Eq(s.HitValueTotal,300);s.Advance(5002);Eq(s.Combo,1);Eq(s.JudgedCount,1);});
        Add("E04 failure resets earlier circle combo",()=>{var s=new CircleSession(Parse("0,0,1000,1,0\n256,192,2000,8,0,5000"));s.Press(Vector2.Zero,1000);Eq(s.Combo,1);s.Advance(5001);Eq(s.Combo,0);Eq(s.JudgedCount,2);Eq(s.Spinners[0].Result,Judgement.Miss);});
        Add("E05 fresh sessions do not share observation or motion state",()=>{var map=Parse();var a=new CircleSession(map);var b=new CircleSession(map);SeedCount(a.Spinners[0],4);Eq(b.Spinners[0].Progress,0f);Eq(b.Spinners[0].Observation.Count,0);Eq(b.Spinners[0].Observation.CachedInteger,0);Eq(b.Spinners[0].PreviousAngle,null);});
        Add("E06 nominal input qualification is separate from integration",()=>{var s=new CircleSession(Parse());s.Advance(2000,new(300,192),1);s.Advance(2008,new(256,236),1);Eq(s.Spinners[0].PreviousAngle,0.0);s.Advance(2017,new(256,236),1);Near(s.Spinners[0].PreviousAngle!.Value,Math.PI/2);});
        Add("E07 supplied mapped spinner pointer separate from head pointer",()=>{var s=new CircleSession(Parse());s.Advance(2000,Vector2.Zero,1,new(256,292));Near(s.Spinners[0].PreviousAngle!.Value,Math.PI/2);});
        Add("E08 mixed circle slider spinner sequence counted once",()=>{
            var map=Parse("0,0,1000,1,0\n0,0,2000,2,0,L|100:0,1,100\n256,192,4000,8,0,7000\n0,0,8000,1,0\n0,0,9000,2,0,L|100:0,1,100");var s=new CircleSession(map);Eq(s.ObjectCount,5);Eq(s.Heads.Count,4);
            s.Press(Vector2.Zero,1000);s.Press(Vector2.Zero,2000);s.Advance(2000,Vector2.Zero,1);s.Advance(2964,new(96.4f,0),1);s.Advance(3000,new(100,0),1);
            SeedCount(s.Spinners[0],16);Eq(s.Combo,3);s.Advance(7001);Eq(s.Combo,4);s.Press(Vector2.Zero,8000);s.Press(Vector2.Zero,9000);s.Advance(9000,Vector2.Zero,1);s.Advance(9964,new(96.4f,0),1);s.Advance(10000,new(100,0),1);
            Eq(s.Combo,7);Eq(s.JudgedCount,5);Eq(s.ObjectResults.Count(),5);Yes(s.ObjectResults.All(x=>x==Judgement.Hit300),"mixed final results differ");Eq(s.Accuracy,100.0);
        });
        Add("F01 all three R07 fixtures load under current parser",()=>{foreach(var n in new[]{"spinner-practice","circle-spinner","mixed-r07"})Eq(Beatmap.Load(Path.Combine(root,"reconstructed/CircleGame/Fixtures",n+".osu")).Spinners.Count,1);});
        Add("F02 valid optional hit sample retained",()=>{var d=Parse("99,88,2000,8,7,5000,1:2:3:4:local.wav").Spinners[0];Eq(d.StartTime,2000);Eq(d.EndTime,5000);Eq(d.HitSound,7);Eq(d.HitSample,"1:2:3:4:local.wav");Eq(Parse().Spinners[0].HitSample,null);});
        Add("F03 malformed and empty hitSound reject",()=>{Reject(()=>Parse("0,0,2000,8,no,5000"));Reject(()=>Parse("0,0,2000,8,,5000"));});
        Add("F04 malformed and absent end reject",()=>{Reject(()=>Parse("0,0,2000,8,0,no"));Reject(()=>Parse("0,0,2000,8,0"));});
        Add("F05 spinner unsupported versions reject",()=>{Reject(()=>Parse(version:13));Reject(()=>Parse(version:5));});
        Add("F06 zero negative and overlong durations reject",()=>{foreach(var e in new[]{2000,1999,62001})Reject(()=>Parse($"0,0,2000,8,0,{e}"));});
        Add("F07 duration1 and duration60000 accepted",()=>{Eq(Parse("0,0,2000,8,0,2001").Spinners[0].EndTime,2001);Eq(Parse("0,0,2000,8,0,62000").Spinners[0].EndTime,62000);});
        Add("F08 maximum end bound equality and rejection",()=>{Eq(Parse("0,0,3540000,8,0,3600000").Spinners[0].EndTime,3600000);Reject(()=>Parse("0,0,3540001,8,0,3600001"));});
        Add("F09 overlapping and shared endpoint objects reject",()=>{Reject(()=>Parse("0,0,2000,8,0,5000\n0,0,4000,1,0"));Reject(()=>Parse("0,0,2000,8,0,5000\n0,0,5000,1,0"));Reject(()=>Parse("0,0,1000,2,0,L|200:0,1,200\n0,0,2000,8,0,5000"));});
        Add("F10 old circle slider fixtures remain admitted",()=>{foreach(var n in new[]{"first-slider","slider-repeat","bezier-quadratic"}){var m=Beatmap.Load(Path.Combine(root,"reconstructed/CircleGame/Fixtures",n+".osu"));Eq(m.Spinners.Count,0);Yes(m.Sliders.Count>0,"slider missing");}});
    }
}
