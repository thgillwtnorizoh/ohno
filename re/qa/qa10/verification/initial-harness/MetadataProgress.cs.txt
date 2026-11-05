using System.Numerics;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
namespace Ohno.QA10;
public static partial class Tests
{
    static void Metadata()
    {
        Add("M01 raw parent and exact parsed identity","E039 A/E;S01",()=>{var m=SM(mask:47);var s=m.Sliders.Single();var d=Data(s);Eq(d.RawSound,47);Eq(d.RawTraversals,1);Eq(d.EffectiveTraversals,1);Eq(d.SourceOrder,0);Yes(ReferenceEquals(m.CircleSounds!.RegisteredOverrides[0].Slider,s),"registered operand exact parent identity");Yes(m.HealthSource!.IsFreshFor(m),"fresh parser ownership");});
        foreach(var pair in new[]{(Name:"missing",Value:(string?)null),(Name:"empty",Value:(string?)"")})
            Add("M02 "+pair.Name+" EdgeSounds simple","E039 E;S22",()=>{var d=Data(SM(sounds:pair.Value).Sliders[0]);Eq(d.Simple,true);Eq(d.RawEdgeSounds,pair.Value);Seq(d.EdgeSounds);});
        foreach(int count in new[]{1,2,3})
            Add($"M03 explicit list raw+1 padding R{count}","E039 M28f7:185d/1895;S23",()=>{var d=Data(SM(count,8,"2","1:3").Sliders[0]);Eq(d.Simple,false);Seq(d.EdgeSounds,new[]{2}.Concat(Enumerable.Repeat(8,count)).ToArray());Seq(d.Bases,new[]{1}.Concat(Enumerable.Repeat(0,count)).ToArray());Seq(d.Additions,new[]{3}.Concat(Enumerable.Repeat(0,count)).ToArray());});
        Add("M04 excess arrays truncated independently","E039 E;S23",()=>{var d=Data(SM(1,0,"2|4|8|14","1:2|2:3|3:1|1:1").Sliders[0]);Seq(d.EdgeSounds,2,4);Seq(d.Bases,1,2);Seq(d.Additions,2,3);});
        foreach(string raw in new[]{"bad|4|8","|4|8","2147483648|4|8","-2|0|31"})
            Add("M05 EdgeSounds TryParse "+raw,"E039 M28f7:1874;S24",()=>{var d=Data(SM(2,2,raw).Sliders[0]);Eq(d.Error,null);Seq(d.EdgeSounds,raw.StartsWith("-2",StringComparison.Ordinal)?-2:0,raw.StartsWith("-2",StringComparison.Ordinal)?0:4,raw.StartsWith("-2",StringComparison.Ordinal)?31:8);});
        foreach(string bad in new[]{"1","a:2","1:b","2147483648:0",""+"1:2|"})
            Add("M06 invalid EdgeSets retained unavailable "+bad,"E039 M28f7:190c-1936;S25 ENGINEERING containment",()=>{var m=SM(2,0,"0|0|0",bad);var d=Data(m.Sliders[0]);Yes(d.Error is not null,"conversion/index rejection recorded");Throws<System.IO.InvalidDataException>(()=>SliderSampleSelection.Edge(m.Sliders[0],d,m.CircleSounds!.Timing,Policy(),1));Eq(Fresh(m).Sliders.Count,1);});
        Add("M07 missing EdgeSets padded zero","E039 E;S23",()=>{var d=Data(SM(2,4,"2|4|8").Sliders[0]);Seq(d.Bases,0,0,0);Seq(d.Additions,0,0,0);});
        Add("M08 EdgeSets without sounds retained but ignored","E039 M142d simple mode;S22",()=>{var m=SM(2,4,null,"3:2|2:1|1:3","1:2");var s=m.Sliders[0];var d=Data(s);Eq(d.Simple,true);Seq(d.Bases,3,2,1);Seq(d.Additions,2,1,3);Eq(SliderSampleSelection.Edge(s,d,m.CircleSounds!.Timing,Policy(),1),new SelectedCircleSample(4,1,2,1,100));});
        Add("M09 empty sounds with EdgeSets stays simple","E039 E;S22",()=>{var d=Data(SM(2,2,"","3:2").Sliders[0]);Eq(d.Simple,true);Seq(d.Bases,3);Seq(d.Additions,2);});
        foreach(int volume in new[]{-9,0,1,99,101})
            Add($"M10 payload volume{volume}","E039 M28f7:1a47-1a54",()=>{var d=Data(SM(payload:$"3:2:17:{volume}:custom.wav").Sliders[0]);Eq(d.BaseSet,3);Eq(d.AdditionSet,2);Eq(d.Index,17);Eq(d.Volume,volume<0?0:volume>100?100:volume);Eq(d.Filename,"custom.wav");Eq(d.Error,null);});
        Add("M11 minimal payload preserves absent optionals","E039 E",()=>{var d=Data(SM(payload:"2:3").Sliders[0]);Eq(d.BaseSet,2);Eq(d.AdditionSet,3);Eq(d.Index,0);Eq(d.Volume,0);Eq(d.Filename,"");Eq(d.RawPayload,"2:3");});
        foreach(string payload in new[]{"","1","x:2","1:2:x","1:2:0:x"})
            Add("M12 malformed payload "+payload,"E039 E;ENGINEERING unavailable",()=>{Yes(Data(SM(payload:payload).Sliders[0]).Error is not null,"malformed metadata not guessed defaults");});
        foreach(int version in new[]{13,14})
            Add($"M13 format{version} edge0 inheritance and head fields","E039 M146a:0067-00ab/M48e0:005f-0082;S26",()=>{var m=SM(2,14,"2|4|8","0:0|0:0|0:0","3:1:9:91:private.wav",version,"0,500,4,2,7,23,1,0");var s=m.Sliders[0];var p=PreparedSliderSamples.Create(s,m.CircleSounds!.Timing,Policy(),8123);Eq(p.Head.BaseSet,version==13?3:2);Eq(p.Head.AdditionSet,version==13?3:2);Eq(p.Head.RawSound,2);Eq(p.Head.StoredMask,2);Eq(p.Head.Index,0);Eq(p.Head.Volume,0);Eq(p.Head.Filename,"");Eq(p.ParentCustomHandle,8123);Yes(ReferenceEquals(p.Head.Object,s.Head),"exact embedded head definition");Eq(SliderSampleSelection.Edge(s,Data(s),m.CircleSounds.Timing,Policy(),1).BaseSet,2);});
        foreach(int spans in new[]{1,2,3})
            Add($"M14 independently derived schedule R{spans}","E016/E021/E039 S01/S20: S shifted2000ms, length300,V200,Q100",()=>{var s=SM(spans).Sliders[0];int[] expected=spans==1?[4500,5000,5464]:spans==2?[4500,5000,5500,6000,6500,6964]:[4500,5000,5500,6000,6500,7000,7500,8000,8464];Seq(Data(s).Schedule,expected);Seq(s.Checkpoints.Select(c=>c.Time),expected);Seq(Data(s).EdgeSubset,spans==1?[]:spans==2?[5500]:[5500,7000]);Eq(s.EndTime,spans==1?5500:spans==2?7000:8500);});
        Add("M15 source order retained across circle and slider","E039 provenance;ENGINEERING fresh source",()=>{var m=Map("32,192,1000,1,0\n"+Row()+"\n480,192,9000,1,0");var s=m.Sliders[0];Eq(Data(s).SourceOrder,1);Yes(ReferenceEquals(m.CircleSounds!.RegisteredOverrides[1].Slider,s),"source index maps exact prepared parent");});
        foreach(int spans in new[]{0,-1,17})Add($"M16 bounded traversal{spans} rejected","R12 ENGINEERING1-16, E039 raw-vs-effective boundary",()=>Throws<System.IO.InvalidDataException>(()=>SM(spans)));
        Add("M17 mismatched event parent rejected","E039 exact source identity;ENGINEERING",()=>{var m=SM();var s=m.Sliders[0];var p=PreparedSliderSamples.Create(s,m.CircleSounds!.Timing,Policy(),-1);var f=new RF(true);f.Prepare();Throws<System.IO.InvalidDataException>(()=>SliderSampleSelection.Request(new(s with{},SliderSoundKind.Tick,0,0,4500,256),p,m.CircleSounds.Timing,f.R,Policy(),new Sink()));});
    }
    static void Progression()
    {
        Add("A01 fresh0 and first due skip","E039 M142d default;S42",()=>{var s=new SliderState(SM().Sliders[0]);Eq(s.Audio.Index,0);Eq(Audio(s,4500),null);Eq(s.Audio.Index,0);Eq(Audio(s,5000),new SliderAudioSelection(SliderSoundKind.Tick,1,1));Eq(s.ProcessedCheckpoints,0);});
        Add("A02 explicit Reset0","E039 M1431:0019;S42",()=>{var s=Pre(SM().Sliders[0]);Audio(s,5000);s.Audio.Reset();Eq(s.Audio.Index,0);Eq(Audio(s,4500),null);});
        Add("A03 prestart sets minus1","E039 M1449:0226;S40",()=>{var s=new SliderState(SM().Sliders[0]);Eq(Audio(s,3999,false),null);Eq(s.Audio.Index,-1);});
        foreach(var row in new[]{(T:4000,Expected:-1,Index:-1),(T:4499,Expected:-1,Index:-1),(T:4500,Expected:0,Index:0),(T:4501,Expected:0,Index:0),(T:5000,Expected:1,Index:1),(T:5464,Expected:1,Index:1),(T:5500,Expected:1,Index:1),(T:5501,Expected:-1,Index:-1)})
            Add($"A04 inclusive range/due time{row.T}","E039 M1449 prefix/range;S01/S08/S15/S40",()=>{var s=Pre(SM().Sliders[0]);var e=Audio(s,row.T);Eq(e?.Index??-1,row.Expected);Eq(s.Audio.Index,row.Index);Eq(s.ProcessedCheckpoints,0);});
        foreach(int numeric in new[]{-1,0,2})Add($"A05 numeric audio state{numeric}","E039 M1449:0107-010d;S40",()=>{var s=Pre(SM().Sliders[0]);Eq(Audio(s,4500,true,numeric),null);Eq(s.Audio.Index,-1);Eq(Audio(s,4500),new SliderAudioSelection(SliderSoundKind.Tick,0,0));});
        Add("A06 same-time no duplicate","E039 S09",()=>{var s=Pre(SM().Sliders[0]);Audio(s,4500);Eq(Audio(s,4500),null);Eq(Audio(s,4501),null);Eq(s.Audio.Index,0);});
        Add("A07 latest due skips earlier sounds","E039 S08",()=>{var s=Pre(SM().Sliders[0]);Eq(Audio(s,5100),new SliderAudioSelection(SliderSoundKind.Tick,1,1));Eq(Audio(s,5100),null);Eq(s.ProcessedCheckpoints,0);});
        Add("A08 tracking false consumes then reentry silent","E039 S06/S41",()=>{var s=Pre(SM().Sliders[0]);Eq(Audio(s,4500,false),null);Eq(s.Audio.Index,0);Eq(Audio(s,4501),null);Eq(Audio(s,5000),new SliderAudioSelection(SliderSoundKind.Tick,1,1));});
        Add("A09 false latest consumes multiple deadlines","E039 S41",()=>{var s=Pre(SM().Sliders[0]);Audio(s,5100,false);Eq(s.Audio.Index,1);Eq(Audio(s,5101),null);});
        Add("A10 outside rewind permits repeat","E039 S43 artificial direct lifecycle",()=>{var s=Pre(SM().Sliders[0]);Audio(s,4500);Audio(s,3990);Eq(s.Audio.Index,-1);Eq(Audio(s,4500)?.Index,0);});
        Add("A11 in-range rewind does not undo index","E039 M1449 latest<=old gate",()=>{var s=Pre(SM().Sliders[0]);Audio(s,5000);Eq(Audio(s,4500),null);Eq(s.Audio.Index,1);});
        Add("A12 first future blocks later due artificial list","E039 prefix rule;artificial prepared order",()=>{var d=SM().Sliders[0];d=d with{Checkpoints=[d.Checkpoints[0] with{Time=5000},d.Checkpoints[1] with{Time=4500},d.Checkpoints[2]]};var s=Pre(d);Eq(Audio(s,4600),null);Eq(s.Audio.Index,-1);});
        Add("A13 only final list entry never produces audio","E039 S15;single short traversal",()=>{var d=SM(length:20).Sliders[0];Seq(Data(d).Schedule,4064);var s=Pre(d);Eq(Audio(s,4064),null);Eq(Audio(s,4100),null);Eq(s.Audio.Index,-1);});
        Add("A14 two spans reversal quotient","E039 S10/S13",()=>{var s=Pre(SM(2).Sliders[0]);Eq(Audio(s,5500),new SliderAudioSelection(SliderSoundKind.RepeatEdge,1,2));Eq(s.Audio.Index,2);});
        Add("A15 jump across reversal picks later tick","E039 S14",()=>{var s=Pre(SM(2).Sliders[0]);Eq(Audio(s,6000),new SliderAudioSelection(SliderSoundKind.Tick,3,3));});
        Add("A16 reversal consumed while tracking lost","E039 S12",()=>{var s=Pre(SM(2).Sliders[0]);Audio(s,5500,false);Eq(s.Audio.Index,2);Eq(Audio(s,5501),null);});
        Add("A17 three span complete discrete sequence","E039 S20; independently shifted producer",()=>{var s=Pre(SM(3).Sliders[0]);var e=new List<string>();foreach(int t in new[]{4500,5000,5500,6000,6500,7000,7500,8000,8464})if(Audio(s,t)is{} next)e.Add($"{next.Kind}:{next.Index}:{next.ScheduleIndex}");Seq(e,"Tick:0:0","Tick:1:1","RepeatEdge:1:2","Tick:3:3","Tick:4:4","RepeatEdge:2:5","Tick:6:6","Tick:7:7");Eq(s.ProcessedCheckpoints,0);});
        Add("A18 producer labels cannot select sound","E039 M1449 arithmetic independent of raw/kind",()=>{var d=SM(2).Sliders[0];d=d with{Checkpoints=d.Checkpoints.Select(c=>c with{Kind=SliderCheckpointKind.Final}).ToArray()};Eq(Audio(Pre(d),4500)?.Kind,SliderSoundKind.Tick);Eq(Audio(Pre(d),5500)?.Kind,SliderSoundKind.RepeatEdge);});
        foreach(var item in new[]{(T:5499,C:3,E:0),(T:5500,C:3,E:0),(T:5501,C:3,E:2),(T:7000,C:3,E:2),(T:5501,C:0,E:-1),(T:5501,C:1,E:0)})
            Add($"A19 aggregate midpoint t{item.T} count{item.C}","E039 M1466 midpoint/count;S16/S42 artificial selector operands",()=>Eq(SliderAudioProgress.AggregateEdge(4000,7000,item.C,item.T),item.E));
    }
}
