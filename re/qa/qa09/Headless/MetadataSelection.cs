using System.Numerics;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA09;
public static partial class Tests
{
    static void MetadataCases()
    {
        foreach(int raw in new[]{0,1,2,4,8,14,15,31,-1,int.MinValue}) Add($"M01 raw sound {raw} versus constructor bits","E037 M45eb/M45ec; raw column retained",()=>{
            var map=Map($"64,128,2000,1,{raw},3:2:7:33:kick.ogg\n400,192,4000,1,0");var m=map.CircleSounds!.Circles[0];
            Eq(m.RawSound,raw);Eq(m.StoredMask,raw&14);Eq(m.BaseSet,3);Eq(m.AdditionSet,2);Eq(m.Index,7);Eq(m.Volume,33);Eq(m.Filename,"kick.ogg");
            Yes(ReferenceEquals(m.Object,map.Circles[0]),"exact parsed identity");Eq(m.Object.Position,new Vector2(64,128));Eq(m.Object.StartTime,2000);
        });
        foreach(var x in new (string? Payload,int Base,int Add,int Index,int Volume,string File)[]{
            (null,0,0,0,0,""),("",0,0,0,0,""),("0:0",0,0,0,0,""),("2:3:0:0",2,3,0,0,""),
            ("1:0:2:100:a.wav",1,0,2,100,"a.wav"),("3:1:-1:-10",3,1,-1,0,""),("2:0:3:101",2,0,3,100,"")})
            Add($"M02 payload {x.Payload??"ABSENT"}","E037 fields/defaults/clamp; empty containment ENGINEERING",()=>{
                var m=Meta(14,x.Payload);Eq(m.Error,null);Eq(m.RawPayload,x.Payload);Eq(m.BaseSet,x.Base);Eq(m.AdditionSet,x.Add);Eq(m.Index,x.Index);Eq(m.Volume,x.Volume);Eq(m.Filename,x.File);
            });
        foreach(string payload in new[]{"1",":0","x:1","1:0:","1:0:1:","1:0:2147483648","1:0:0:2147483648"})
            Add("M03 malformed payload "+payload,"R11 documented ENGINEERING containment, not original exception equivalence",()=>{
                var m=Map($"128,192,2000,1,0,{payload}\n128,192,4000,1,0");Yes(m.CircleSounds!.Circles[0].Error is not null,"explicit metadata error");
                var s=Fresh(m);s.Press(new(128,192),2000);Eq(s.Circles[0].Result,Judgement.Hit300);
            });
        Add("M04 timing source order set-zero normalization and volume clamp","E037 M28ff/M0b73",()=>{
            var m=Map(timing:"2006,500,4,3,9,101,1,0\n2005,-100,4,0,2,-5,0,0\n1999,500,4,1,1,44,1,0");var t=m.CircleSounds!.Timing;
            Seq(t.Select(p=>p.Time),2006d,2005d,1999d);Seq(t.Select(p=>p.Set),3,2,1);Seq(t.Select(p=>p.Index),9,2,1);Seq(t.Select(p=>p.Volume),100,0,44);
            Seq(t.Select(p=>p.SourceOrder),0,1,2);Seq(t.Select(p=>p.Uninherited),true,false,true);
            Yes(ReferenceEquals(CircleSampleSelection.Timing(t,2000),t[2]),"last supplied qualifying record, not greatest time");
        });
        foreach(string timing in new[]{"0,500","0,500,4,1,0","0,500,4,x,0,100,1,0","0,500,4,1,0,bad,1,0"})
            Add("M05 unavailable sample timing "+timing,"R11 explicit unsupported defaults/containment; slider timing remains admitted",()=>{
                var m=Map(timing:timing);Yes(m.CircleSounds!.TimingError is not null,"unavailable reason retained");Eq(m.Circles.Count,2);var s=Fresh(m);s.Press(new(128,192),2000);Eq(s.Ledger.N300,(ushort)1);
            });
        Add("M06 unsorted circle source keeps exact identities","E037 object metadata ownership; R10 separate registration guard",()=>{
            var m=Map("64,192,4000,1,2,1:3:2:45:late.wav\n448,192,2000,1,8,2:1:0:67:early.wav");var side=m.CircleSounds!.Circles;
            Eq(side[0].Object.StartTime,4000);Eq(side[1].Object.StartTime,2000);Yes(ReferenceEquals(side[0].Object,m.Circles[1]),"source late identity after circle sorting");
            Yes(ReferenceEquals(side[1].Object,m.Circles[0]),"source early identity after circle sorting");Eq(Fresh(m).Health,null);
        });
        Add("M07 no sample metadata changes slider duration or R10 flags","E005 unchanged timing; E036 final grouping",()=>{
            string rows="128,192,2000,1,0\n128,192,4000,2,14,L|228:192,1,100\n128,192,6000,1,0";
            var a=Map(rows,timing:"0,500,4,1,0,100,1,0");var b=Map(rows,timing:"0,500,4,3,7,15,1,0");
            Eq(a.Sliders[0].EndTime,4500);Eq(b.Sliders[0].EndTime,4500);Seq(a.Sliders[0].Checkpoints.Select(x=>x.Time),4464);Seq(b.Sliders[0].Checkpoints.Select(x=>x.Time),4464);
            var sa=Fresh(a);var sb=Fresh(b);Seq(sa.Health!.Chart.NewComboProvenance.Select(p=>p.FinalFlag),true,false,false);Seq(sb.Health!.Chart.NewComboProvenance.Select(p=>p.FinalFlag),true,false,false);
            Eq(sa.Health.Calibration.Rate,sb.Health.Calibration.Rate);Eq(sa.Health.Calibration.B,sb.Health.Calibration.B);Eq(sa.Health.Calibration.G,sb.Health.Calibration.G);Eq(sa.Health.Calibration.Trials,sb.Health.Calibration.Trials);
        });
    }
    static void SelectionCases()
    {
        Add("S01 null and empty timing","E037 M1be6 null/empty",()=>{Eq(CircleSampleSelection.Timing(null,2000),null);Eq(CircleSampleSelection.Timing([],2000),null);var f=new ResourcesFixture();var sink=new Sink();CircleSampleSelection.Request(Meta(),[],50,f.R,Policy(),sink);Eq(sink.Requests.Count,0);Eq(f.Trace.Count,0);});
        foreach(int time in new[]{2004,2005,2006}) Add($"S02 query start+5 record {time}","E037 M24bb:0012-0014; M1be6 <=",()=>{
            SampleTiming[] t=[Point(time:0),Point(set:3,time:time)];Eq(CircleSampleSelection.Timing(t,2000)!.Set,time<=2005?3:1);
        });
        Add("S03 first supplied when none qualify","E037 M1be6 first fallback",()=>{SampleTiming[] t=[Point(3,time:3000),Point(2,time:2500)];Yes(ReferenceEquals(CircleSampleSelection.Timing(t,2000),t[0]),"first supplied, not minimum-time point");});
        Add("S04 supplied-order last qualifying","E037 no sort in timing lookup",()=>{SampleTiming[] t=[Point(1,time:2005),Point(3,time:1990),Point(2,time:2006)];Yes(ReferenceEquals(CircleSampleSelection.Timing(t,2000),t[1]),"last qualifying record remains1990");});
        Add("S05 unchecked start+5 overflow artificial operand","E037 integer add before Double conversion",()=>{SampleTiming[] t=[Point(1,time:-2147483644d),Point(3,time:0)];Eq(CircleSampleSelection.Timing(t,int.MaxValue)!.Set,1);});
        Add("S06 zero inheritance versus explicit selectors","E037 M24ba field tests",()=>{
            var a=CircleSampleSelection.Select(Meta(2,"0:0:0:0"),Point(3,7,40),Policy());Eq(a,new SelectedCircleSample(2,3,0,7,40));
            var b=CircleSampleSelection.Select(Meta(14,"1:2:2:25"),Point(3,7,40),Policy());Eq(b,new SelectedCircleSample(14,1,2,2,25));
        });
        Add("S07 zero selected base discards addition artificial","E037 M24ba:003f-0047",()=>{var a=CircleSampleSelection.Select(Meta(2,"0:3:0:0"),Point(0),Policy());Eq(a.AdditionSet,0);});
        var gates=new (string Name,CircleSoundPolicy P,bool Normal)[]{("all true",Policy(true),true),("no skin",Policy(true) with{HasSkin=false},false),("layer off",Policy(false),false),("no selected map",Policy(true) with{HasSelectedMap=false},false),("mode3",Policy(true) with{SelectedMode=3},false)};
        foreach(var g in gates) Add("S08 nonzero mask gate "+g.Name,"E037 M44a2 exact predicate",()=>{Eq(CircleSampleSelection.Select(Meta(2),Point(),g.P).Mask,g.Normal?3:2);Eq(CircleSampleSelection.Select(Meta(0),Point(),g.P).Mask,1);});
        foreach(var x in new[]{(0,new[]{1}),(2,new[]{2}),(4,new[]{4}),(8,new[]{8}),(14,new[]{4,2,8}),(15,new[]{4,2,8})})
            Add($"S09 normalized mask {x.Item1} ordered layers","E037 M3b32 Normal/Finish/Whistle/Clap",()=>{Seq(Requests(Meta(x.Item1)).Requests.Select(r=>r.Layer),x.Item2);});
        var volumes=new[]{(0,6,8,6),(1,6,8,6),(7,6,8,6),(8,6,8,6),(9,7,9,7),(10,8,10,8),(19,15,19,16),(99,79,99,84),(100,80,100,85)};
        foreach(var v in volumes) Add($"S10 standard finite volume {v.Item1}","E037 Double multipliers and conv.i4; literal independent expected tuple",()=>{
            var sink=Requests(Meta(14),policy:Policy(true),timingVolume:v.Item1);Seq(sink.Requests.Select(r=>r.Layer),1,4,2,8);Seq(sink.Requests.Select(r=>r.Volume),v.Item2,v.Item3,v.Item4,v.Item4);
        });
        foreach(int custom in new[]{0,777}) Add($"S11 custom handle {custom} bypasses floor/layers","E037 custom !=-1; zero is queued selection operand",()=>{var r=Requests(Meta(14),custom,Policy(true),0).Requests;Eq(r.Count,1);Eq(r[0].Handle,custom);Eq(r[0].Layer,0);Eq(r[0].Volume,0);});
        Add("S12 custom -1 returns standard layers","E037 M24ba distinct sentinel",()=>Seq(Requests(Meta(14),-1,Policy(true)).Requests.Select(r=>r.Layer),1,4,2,8));
        foreach(var x in new[]{(0,-.4f),(64,-.3f),(256,0f),(448,.3f),(512,.4f)}) Add($"S13 circle X pan {x.Item1}","E037 M24bd Single equation",()=>{float a=Requests(Meta(x:x.Item1),50).Requests.Single().Pan;Near(a,x.Item2,2e-8);});
        foreach(var rate in new[]{(75,true,.75f),(100,true,1f),(150,true,1.5f),(150,false,1f)}) Add($"S14 factor {rate.Item1} flag{rate.Item2}","E037 M3b31 bounded rate branch",()=>Near(Requests(Meta(),50,Policy() with{SongRate=rate.Item1,RateMatching=rate.Item2}).Requests.Single().Factor,rate.Item3));
        Add("S15 gain integer global product then Single divisions","E037 M3b40/M1404, managed operands only",()=>{
            var r=Requests(Meta(4,"0:0:0:37"),policy:Policy() with{VolumeEffect=63,VolumeUniversal=71}).Requests.Single();Eq(r.Volume,37);Near(r.Gain,.165501,2e-8);
        });
        Add("S16 observer before submit; prepared lookup has no provider I/O","E037 M24ba observer; E038 dictionary/table judgement boundary",()=>{
            var f=new ResourcesFixture();f.PrepareDefaults();var order=new List<string>();var sink=new OrderSink(order);CircleSampleSelection.Request(Meta(14),[Point()],-1,f.R,Policy(true),sink,_=>order.Add("observer"));Seq(order,"observer","1","4","2","8");Eq(f.Trace.Count,0);
        });
        Add("S17 distinct base/addition banks versus addition-zero inheritance","E037 M24ba/M3b32 separate layer set operands",()=>{
            var f=new ResourcesFixture();foreach(var x in new[]{("normal-hitnormal",11),("normal-hitfinish",14),("normal-hitwhistle",12),("normal-hitclap",18),("drum-hitfinish",34),("drum-hitwhistle",32),("drum-hitclap",38)})f.R.Caches.Embedded[x.Item1]=x.Item2;
            f.R.Prepare([],[]);var a=new Sink();CircleSampleSelection.Request(Meta(14,"1:3:0:25"),[Point()],-1,f.R,Policy(true),a);Seq(a.Requests.Select(r=>r.Handle),11,34,32,38);
            var b=new Sink();CircleSampleSelection.Request(Meta(14,"1:0:0:25"),[Point()],-1,f.R,Policy(true),b);Seq(b.Requests.Select(r=>r.Handle),11,14,12,18);
        });
    }
    sealed class OrderSink(List<string> order):ISamplePlaybackSink { public void Enqueue(SamplePlaybackRequest request)=>order.Add(request.Layer.ToString()); }
}
