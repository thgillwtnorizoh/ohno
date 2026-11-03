// Independent finite assertions on linked production methods. No GUI or OS input.
using System.Numerics;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA04;
public static class GeometryTests
{
    private static readonly List<(string Name, Action Body)> Cases=[];
    private static readonly Vector2[] Q=[new(0,0),new(1,1),new(2,0)];
    private static readonly Vector2[] QV=[new(0,0),new(.5f,.375f),new(1,.5f),new(1.5f,.375f),new(2,0)];
    private static readonly Vector2[] C=[new(0,0),new(1,1),new(2,1),new(3,0)];
    private static readonly Vector2[] CV=[new(0,0),new(.5f,.421875f),new(1,.671875f),new(1.5f,.75f),new(2,.671875f),new(2.5f,.421875f),new(3,0)];
    private static string scratch="";
    private static int file;
    private static void Eq<T>(T a,T b) { if(!EqualityComparer<T>.Default.Equals(a,b)) throw new InvalidOperationException($"expected={b}; actual={a}"); }
    private static void Yes(bool b,string why) { if(!b) throw new InvalidOperationException(why); }
    private static void Near(double a,double b,double eps=1e-6) { if(!double.IsFinite(a)||Math.Abs(a-b)>eps) throw new InvalidOperationException($"expected near {b}; actual={a}"); }
    private static void Vec(Vector2 a,Vector2 b) { Near(a.X,b.X);Near(a.Y,b.Y); }
    private static void Same(IEnumerable<Vector2> a,IEnumerable<Vector2> b) { Yes(a.SequenceEqual(b),"vertex sequence mismatch: "+string.Join(";",a)); }
    private static void Reject(Action a) { try {a();} catch(InvalidDataException) {return;} throw new InvalidOperationException("expected InvalidDataException"); }
    private static SliderGeometry G(Vector2[] p,double length)=>SliderGeometryPreparation.Prepare(p[0],p,true,length);
    private static IEnumerable<Vector2> Vertices(IReadOnlyList<SliderSegment> s)=>new[]{s[0].From}.Concat(s.Select(x=>x.To));
    private static Beatmap Map(string path="B|60:0|60:0|60:80",string length="140",int spans=2,int version=14,string head="0,0")
    {
        string p=Path.Combine(scratch,$"{file++}.osu");
        File.WriteAllText(p,$"osu file format v{version}\n[General]\nMode:0\n[Difficulty]\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,1000,4,1,0,100,1,0\n[HitObjects]\n{head},3000,2,0,{path},{spans},{length}\n");
        return Beatmap.Load(p);
    }
    private static SliderDefinition D()=>Map().Sliders.Single();
    private static CircleSession Session()
    {
        var s=new CircleSession(Map());s.Press(Vector2.Zero,3000);s.Advance(3000,Vector2.Zero,1);return s;
    }
    private static SliderState S(CircleSession s)=>s.Sliders.Single();
    private static void Add(string name,Action a)=>Cases.Add((name,a));
    public static int Main(string[] args)
    {
        if(args.Length!=1)throw new ArgumentException("Pass repository root.");
        scratch=Path.Combine(Path.GetTempPath(),"ohno-qa04-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        try { Register();int fail=0;foreach(var c in Cases)try {c.Body();Console.WriteLine("PASS "+c.Name);}catch(Exception e){fail++;Console.WriteLine($"FAIL {c.Name}: {e.GetType().Name}: {e.Message}");}
            Console.WriteLine($"QA04 GEOMETRY TESTS: {Cases.Count-fail} passed, {fail} failed, {Cases.Count} executed");return fail==0?0:1;
        } finally {Directory.Delete(scratch,true);}
    }
    private static void Register()
    {
        Add("A01 E32 quadratic exact raw vertices",()=>Same(SliderGeometryPreparation.FlattenBezier(Q),QV));
        Add("A02 E32 cubic weighted vertices rather than uniform t",()=>{
            var v=SliderGeometryPreparation.FlattenBezier(C);Same(v,CV);Eq(v[1],new Vector2(.5f,.421875f));Yes(v[1].Y!=5f/12f,"analytic uniform point substituted");
        });
        Add("A03 exact squared flatness equality accepts leaf",()=>{
            Same(SliderGeometryPreparation.FlattenBezier([new(0,0),new(1,.25f),new(2,0)]),[new(0,0),new(1,.125f),new(2,0)]);
        });
        Add("A04 just above flatness boundary subdivides",()=>{
            Eq(SliderGeometryPreparation.FlattenBezier([new(0,0),new(1,float.BitIncrement(.25f)),new(2,0)]).Count,5);
        });
        Add("A05 flatness is second difference not chord distance",()=>{
            // Collinear but unevenly spaced: second difference=1, squared=1.
            Eq(SliderGeometryPreparation.FlattenBezier([new(0,0),new(1,0),new(3,0)]).Count,5);
        });
        Add("A06 asymmetric half split preserves polygon order",()=>{
            Same(SliderGeometryPreparation.FlattenBezier([new(0,0),new(1,1),new(2,1)]),
                [new(0,0),new(.5f,.4375f),new(1,.75f),new(1.5f,.9375f),new(2,1)]);
        });
        Add("A07 source controls are not mutated",()=>{var copy=C.ToArray();_ = SliderGeometryPreparation.FlattenBezier(copy);Same(copy,C);});
        Add("A08 singleton retains duplicate final endpoint",()=>Same(SliderGeometryPreparation.FlattenBezier([new(3,4)]),[new(3,4),new(3,4)]));
        Add("B01 repeated control joins two quadratic ranges",()=>{
            var p=new Vector2[]{new(0,0),new(1,1),new(2,0),new(2,0),new(3,-1),new(4,0)};
            var g=G(p,4);Same(Vertices(g.RawSegments),QV.Concat(new Vector2[]{new(2.5f,-.375f),new(3,-.5f),new(3.5f,-.375f),new(4,0)}));
            Eq(g.Controls.Count,6);Eq(g.RawSegments.Count,8);Eq(g.RawSegments.Count(x=>x.IsSubcurveEnd),2);Eq(g.RawSegments[4].From,new Vector2(2,0));
        });
        Add("B02 final equal pair is not a separator",()=>{var g=G([new(0,0),new(1,1),new(2,0),new(2,0)],2);Eq(g.Controls.Count,4);Eq(g.RawSegments.Count(x=>x.IsSubcurveEnd),1);});
        Add("B03 two-control subcurve is a marked straight piece",()=>{
            var g=G([new(0,0),new(3,4)],5);Eq(g.RawSegments.Single(),new SliderSegment(new(0,0),new(3,4),true,true));
        });
        Add("B04 leading duplicate retained as zero raw piece",()=>{
            var g=G([new(0,0),new(0,0),new(3,0)],3);Eq(g.Controls.Count,3);Eq(g.RawSegments.Count,2);Eq(g.RawSegments[0].From,g.RawSegments[0].To);
        });
        Add("B05 head insertion and no second head insertion",()=>{
            var a=SliderGeometryPreparation.Prepare(Vector2.Zero,[new(1,1),new(2,0)],true,2);
            Same(a.Controls,Q);Same(G(Q,2).Controls,Q);
        });
        Add("C01 unadjusted raw length uses E32 Single lengths",()=>{
            var g=G(Q,2);double expected=1.25+2*(double)(float)Math.Sqrt(.265625);Eq(g.UnadjustedLength,expected);Eq(g.RequestedLength,2.0);Same(Vertices(g.RawSegments),QV);
        });
        Add("C02 shorten last piece without changing raw snapshot",()=>{
            var g=G(Q,2);Eq(g.Segments.Count,4);Eq(g.RawSegments[^1].To,new Vector2(2,0));
            Vec(g.Segments[^1].To,new(1.7753788f,.1684659f));
            Eq(g.Segments[0],g.RawSegments[0]);Eq(g.Segments[2],g.RawSegments[2]);
        });
        Add("C03 remove trailing piece at exact requested boundary",()=>{
            var g=G([new(0,0),new(3,0),new(3,0),new(3,4)],3);Eq(g.RawSegments.Count,2);Eq(g.Segments.Count,1);Vec(g.Segments[0].To,new(3,0));
        });
        Add("C04 removal tolerance can erase tiny requested remainder",()=>{
            var g=G([new(0,0),new(3,0),new(3,0),new(3,4)],3.00005);Eq(g.Segments.Count,1);Near(g.Segments[0].To.X,3.00005,1e-6);
        });
        Add("C05 extend nonzero tail direction",()=>{
            var g=G([new(0,0),new(3,4)],10);Vec(g.Segments.Single().To,new(6,8));Eq(g.UnadjustedLength,5.0);Near(g.CumulativeLengths.Single(),10);
        });
        Add("C06 zero final direction blocks requested extension",()=>{
            var g=SliderGeometryPreparation.Prepare(Vector2.Zero,[new(0,0),new(3,0),new(3,0)],false,10);Eq(g.RawSegments.Count,2);Eq(g.Segments.Count,2);
            Eq(g.Segments[^1].From,g.Segments[^1].To);Eq(g.CumulativeLengths[^1],3.0);Eq(g.RequestedLength,10.0);
        });
        Add("C07 entirely erased path rejected by engineering policy",()=>Reject(()=>G([new(0,0),new(3,0)],.00001)));
        Add("C08 cumulative lengths recomputed from adjusted endpoints",()=>{
            var g=G(Q,2);Eq(g.CumulativeLengths.Count,4);Eq(g.CumulativeLengths[0],.625);
            Eq(g.CumulativeLengths[1],.625+(double)(float)Math.Sqrt(.265625));
            double sum=0;for(int i=0;i<g.Segments.Count;i++){sum+=SliderGeometryPreparation.Length(g.Segments[i]);Eq(g.CumulativeLengths[i],sum);}
        });
        Add("D01 joined bend forward and reverse segment times",()=>{
            var d=D();Eq(d.Movements.Count,4);
            Eq(d.Movements[0],new SliderMovement(new(0,0),new(60,0),3000,3600));Eq(d.Movements[1],new SliderMovement(new(60,0),new(60,80),3600,4400));
            Eq(d.Movements[2],new SliderMovement(new(60,80),new(60,0),4400,5200));Eq(d.Movements[3],new SliderMovement(new(60,0),new(0,0),5200,5800));Eq(d.EndTime,5800);
        });
        Add("D02 inclusive first matching segment and reversal",()=>{
            var d=D();Eq(d.MovementIndex(3600),0);Eq(d.MovementIndex(4400),1);Eq(d.MovementIndex(5200),2);
            Vec(d.RequiredPosition(3600),new(60,0));Vec(d.RequiredPosition(4400),new(60,80));Vec(d.RequiredPosition(5800),Vector2.Zero);
        });
        Add("D03 path follows pieces instead of whole-curve chord",()=>{
            var d=D();Vec(d.RequiredPosition(3300),new(30,0));Vec(d.RequiredPosition(4000),new(60,40));Vec(d.RequiredPosition(4800),new(60,40));Vec(d.RequiredPosition(5500),new(30,0));
        });
        Add("D04 true quadratic uses individual segment time conversions",()=>{
            var d=Map("B|1:1|2:0","2",1).Sliders.Single();Eq(d.Movements.Count,4);
            Eq(d.Movements[0],new SliderMovement(new(0,0),new(.5f,.375f),3000,3006));
            Eq(d.Movements[1],new SliderMovement(new(.5f,.375f),new(1,.5f),3006,3011));
            Eq(d.Movements[2],new SliderMovement(new(1,.5f),new(1.5f,.375f),3011,3016));Vec(d.RequiredPosition(3003),new(.25f,.1875f));
        });
        Add("E01 checkpoint carry across pieces and reversal",()=>{
            var d=D();Eq(string.Join(",",d.Checkpoints.Select(x=>x.Time)),"4000,4400,4800,5764");
            Vec(d.Checkpoints[0].Position,new(60,40));Vec(d.Checkpoints[1].Position,new(60,80));Vec(d.Checkpoints[2].Position,new(60,40));Vec(d.Checkpoints[3].Position,Vector2.Zero);
        });
        Add("E02 one outstanding entry per delayed call",()=>{
            var s=Session();s.Advance(4800,new(60,40),1);Eq(S(s).ProcessedCheckpoints,1);Eq(S(s).Checkpoints[0].RawResult,8);
            s.Advance(4800,new(60,40),1);Eq(S(s).ProcessedCheckpoints,2);Eq(S(s).Checkpoints[2].Succeeded,null);
        });
        Add("E03 curved segment boundary preserves held tracking",()=>{
            var s=new CircleSession(Map("B|1:1|2:0","2",1));s.Press(Vector2.Zero,3000);s.Advance(3000,Vector2.Zero,1);s.Advance(3006,new(.5f,.375f),1);
            Yes(S(s).Tracking,"segment transition lost tracking");Eq(S(s).EntryTime,3000);Eq(S(s).ProcessedCheckpoints,0);
        });
        Add("E04 reversal alone preserves entry and tracking",()=>{
            var s=Session();s.Advance(4000,new(60,40),1);s.Advance(4400,new(60,80),1);s.Advance(4401,new(60,79.9f),1);
            Yes(S(s).Tracking,"reversal reset");Eq(S(s).EntryTime,3000);Eq(S(s).Checkpoints[1].RawResult,64);
        });
        Add("E05 loss and timely re-entry across pieces",()=>{
            var s=Session();s.Advance(3600,new(60,0),0);Yes(!S(s).Tracking,"release ignored");s.Advance(3900,new(60,30),1);s.Advance(4000,new(60,40),1);
            Eq(S(s).EntryTime,3900);Eq(S(s).Checkpoints[0].Succeeded,true);Eq(S(s).Head.Result,Judgement.Hit300);Eq(S(s).Result,Judgement.Pending);
        });
        Add("E06 independent head miss then valid later checkpoint",()=>{
            var s=new CircleSession(Map());s.Advance(3200,Vector2.Zero,0);s.Advance(3900,new(60,30),1);s.Advance(4000,new(60,40),1);
            Eq(S(s).Head.Result,Judgement.Miss);Eq(S(s).Checkpoints[0].Succeeded,true);Eq(S(s).Result,Judgement.Pending);
        });
        Add("E07 parent end separate from checkpoint and no duplicate combo",()=>{
            var s=Session();s.Advance(4000,new(60,40),1);s.Advance(4400,new(60,80),1);s.Advance(4800,new(60,40),1);s.Advance(5764,new(3.6f,0),1);
            Eq(S(s).ProcessedCheckpoints,4);Eq(s.Combo,5);s.Advance(5799,new(.1f,0),1);Eq(S(s).Result,Judgement.Pending);
            s.Advance(5800,Vector2.Zero,1);Eq(S(s).Result,Judgement.Hit300);Eq(s.Combo,5);Eq(s.JudgedCount,1);
        });
        Add("E08 late re-entry cannot rescue due checkpoint",()=>{
            var s=Session();s.Advance(3600,new(60,0),0);s.Advance(4001,new(60,40.1f),1);Eq(S(s).Checkpoints[0].Succeeded,false);Yes(S(s).Tracking,"late entry did not activate");
        });
        Add("F01 v13 Bezier rejected",()=>Reject(()=>Map(version:13)));
        Add("F02 unsupported Perfect and Catmull types rejected",()=>{Reject(()=>Map("P|60:0|60:80"));Reject(()=>Map("C|60:0|60:80"));});
        Add("F03 malformed control rejected",()=>Reject(()=>Map("B|broken|60:80")));
        Add("F04 nonfinite control rejected",()=>{Reject(()=>Map("B|NaN:0|60:80"));Reject(()=>Map("B|Infinity:0|60:80"));});
        Add("F05 degenerate geometry rejected",()=>Reject(()=>Map("B|0:0|0:0")));
        Add("F06 control boundary accepts64 rejects65",()=>{
            Eq(SliderGeometryPreparation.FlattenBezier(Enumerable.Range(0,64).Select(i=>new Vector2(i,0)).ToArray()).Count,64);
            Reject(()=>SliderGeometryPreparation.FlattenBezier(Enumerable.Range(0,65).Select(i=>new Vector2(i,0)).ToArray()));
        });
        Add("F07 parser normalized controls limit",()=>Reject(()=>Map("B|"+string.Join("|",Enumerable.Range(1,64).Select(i=>$"{i}:0")))));
        Add("F08 empty nonfinite and overflow flatness defensive checks",()=>{
            Reject(()=>SliderGeometryPreparation.FlattenBezier([]));Reject(()=>SliderGeometryPreparation.FlattenBezier([new(float.NaN,0)]));
            Reject(()=>SliderGeometryPreparation.FlattenBezier([new(0,0),new(float.MaxValue,0),new(0,0)]));
        });
        Add("F09 declared segment and work limits retained",()=>{Eq(SliderGeometryPreparation.MaxSegments,8192);Eq(SliderGeometryPreparation.MaxSubdivisionWork,50000);});
        Add("F10 parser truncates Double controls before Single storage",()=>{
            var d=Map("B|1.9:1.9|2.9:0","2",1).Sliders.Single();Same(d.Geometry.Controls,Q);
        });
    }
}
