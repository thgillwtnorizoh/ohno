// Bounded E039 discrete S controls, direct actual production calls only.
// No game window, OS input, native media, listening or original execution.
using System.Numerics;
using System.Globalization;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;

if (args.Length != 2) throw new ArgumentException("Pass repository root and independent15-file sample directory.");
string scratch = Path.Combine(Path.GetTempPath(), "ohno-r12-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(scratch);
int groups = 0, serial = 0;
var policy = new CircleSoundPolicy(true, false, true, 0, 100, false, 100, 100);
try
{
    var map = Map(); var def = map.Sliders.Single(); var metadata = def.Samples!;
    Eq(metadata.RawSound,0); Eq(metadata.RawTraversals,1); Eq(metadata.EffectiveTraversals,1); Eq(metadata.SourceOrder,0);
    Check(metadata.Simple); Seq(metadata.Schedule,2500,3000,3464); Seq(metadata.EdgeSubset);
    Check(ReferenceEquals(map.CircleSounds!.RegisteredOverrides[0].Slider,def));
    Seq(def.Checkpoints.Select(c=>c.Time),2500,3000,3464);
    Pass("S01 source identity, raw/effective traversal count, actual mixed schedule and subset provenance");
    var explicitMap=Map(2,2,"4","1:2"); var data=explicitMap.Sliders[0].Samples!;
    Seq(data.EdgeSounds,4,2,2); Seq(data.Bases,1,0,0); Seq(data.Additions,2,0,0); Check(!data.Simple);
    var excess=Map(1,2,"2|4|8|14","1:2|2:3|3:1").Sliders[0].Samples!;
    Seq(excess.EdgeSounds,2,4); Seq(excess.Bases,1,2); Seq(excess.Additions,2,3);
    Pass("S23 independently padded/truncated raw+1 EdgeSounds and zero-aligned EdgeSets");
    var invalid=Map(2,14,"bad||8",null).Sliders[0].Samples!; Seq(invalid.EdgeSounds,0,0,8); Check(invalid.Error is null);
    var simple=Map(2,14,null,"3:2|2:1|1:3").Sliders[0]; Check(simple.Samples!.Simple); Seq(simple.Samples.EdgeSounds);
    var info=SliderSampleSelection.Edge(simple,simple.Samples,map.CircleSounds!.Timing,policy,1);
    Eq(info.BaseSet,1); Eq(info.AdditionSet,1); Eq(info.Mask,14);
    Pass("S22/S24 ignored TryParse failure becomes0; EdgeSets alone cannot activate explicit mode");
    foreach(string edgeSet in new[]{"1","bad:2","1:bad"})
    {
        var malformed=Map(2,0,"0|0|0",edgeSet).Sliders[0]; Check(malformed.Samples!.Error is not null);
        Throws<InvalidDataException>(()=>SliderSampleSelection.Edge(malformed,malformed.Samples,null,policy,0));
    }
    Check(Map(payload:"").Sliders[0].Samples!.Error is not null);
    Throws<InvalidDataException>(()=>Map(0)); Throws<InvalidDataException>(()=>Map(17));
    Pass("S25 malformed edge/payload retained as unavailable; existing1–16 traversal bound unchanged");
    var stored=Map(mask:32).Sliders[0]; Eq(stored.Samples!.RawSound,32);
    var prep=PreparedSliderSamples.Create(stored,map.CircleSounds.Timing,policy,99);
    Eq(prep.Head.RawSound,32); Eq(prep.Head.StoredMask,0); Eq(SliderSampleSelection.Edge(stored,stored.Samples,map.CircleSounds.Timing,policy,0).Mask,32);
    Pass("raw parent sound is not circle-normalized; embedded head keeps only2/4/8");
    foreach(int version in new[]{13,14})
    {
        var versionMap=Map(2,14,"2|4|8","0:0|0:0|0:0","3:1:9:90:unique.wav",version,
            "0,500,4,2,7,25,1,0"); var parent=versionMap.Sliders[0];
        var prepared=PreparedSliderSamples.Create(parent,versionMap.CircleSounds!.Timing,policy,123);
        int expected=version==13?3:2; Eq(prepared.Head.BaseSet,expected); Eq(prepared.Head.AdditionSet,expected);
        Eq(prepared.Head.Index,0); Eq(prepared.Head.Volume,0); Eq(prepared.Head.Filename,""); Eq(prepared.ParentCustomHandle,123);
        Check(ReferenceEquals(prepared.Head.Object,parent.Head)); Eq(prepared.Head.StoredMask,2);
        var next=SliderSampleSelection.Edge(parent,parent.Samples!,versionMap.CircleSounds.Timing,policy,1); Eq(next.BaseSet,2); Eq(next.Index,7); Eq(next.Volume,25);
    }
    Pass("S26/S27 format13/14 head-base distinction; only base/addition copied, never index/volume/custom");
    foreach(int delta in new[]{0,50,100,150})
    {
        var session=Session(map);int calls=0;session.SliderSound+=e=>
        {
            if(e.Kind!=SliderSoundKind.Head)return; calls++; Eq(session.Ledger.EventCount,0);Eq(session.Sliders[0].Head.JudgedAt,0);
            Eq(session.Sliders[0].Head.Result,delta==0?Judgement.Hit300:delta==50?Judgement.Hit100:Judgement.Hit50);
        };
        session.Press(def.Head.Position,2000+delta);session.Press(def.Head.Position,2000+delta);
        Eq(calls,delta==150?0:1);Eq(session.Ledger.EventCount,1);Eq(session.Ledger.LastEvent!.Value.Raw,delta==150?-262144:64);
    }
    Pass("S02/S03 positive300/100/50 head hook once before parent64; missed head silent");
    var progress=new SliderAudioProgress();Eq(progress.Index,0);Check(progress.Advance(def,2500,true,1) is null);Eq(progress.Index,0);
    Eq(progress.Advance(def,3000,true,1)!.Index,1);progress.Reset();Eq(progress.Index,0);
    Check(progress.Advance(def,1999,false,1) is null);Eq(progress.Index,-1);Eq(progress.Advance(def,2500,true,1)!.Index,0);
    Pass("S42 constructor/reset0 versus reached pre-start−1; direct first deadline can be skipped");
    progress=Pre(def);Eq(progress.Advance(def,2500,true,1),new SliderAudioSelection(SliderSoundKind.Tick,0,0));
    Check(progress.Advance(def,2500,true,1) is null);Eq(progress.Advance(def,3000,true,1)!.Index,1);
    Check(progress.Advance(def,3464,true,1) is null);Eq(progress.Index,1);
    Pass("S01/S09/S15 timely ticks, same-time suppression and final timestamp exclusion");
    progress=Pre(def);Eq(progress.Advance(def,3100,true,1),new SliderAudioSelection(SliderSoundKind.Tick,1,1));Eq(progress.Index,1);
    Check(progress.Advance(def,3100,true,1) is null);
    Pass("S08/S09 two overdue deadlines yield latest tick only, never replay skipped index0");
    var repeat=Map(2).Sliders[0];Seq(repeat.Samples!.Schedule,2500,3000,3500,4000,4500,4964);Seq(repeat.Samples.EdgeSubset,3500);
    progress=Pre(repeat);Eq(progress.Advance(repeat,3500,true,1),new SliderAudioSelection(SliderSoundKind.RepeatEdge,1,2));
    progress=Pre(repeat);Eq(progress.Advance(repeat,4000,true,1),new SliderAudioSelection(SliderSoundKind.Tick,3,3));
    Pass("S10/S13/S14 repeat quotient selection and jump past repeat to a later interior tick");
    progress=Pre(repeat);Check(progress.Advance(repeat,3500,false,1) is null);Eq(progress.Index,2);
    Check(progress.Advance(repeat,3501,true,1) is null);Eq(progress.Advance(repeat,4000,true,1)!.Index,3);
    Pass("S12/S41 tracking loss consumes due audio index; late reentry without a deadline does not replay");
    progress=Pre(def);Check(progress.Advance(def,2500,true,0) is null);Eq(progress.Index,-1);
    Eq(progress.Advance(def,2500,true,1)!.Index,0);Check(progress.Advance(def,1900,true,1) is null);Eq(progress.Index,-1);
    Eq(progress.Advance(def,2500,true,1)!.Index,0);Check(progress.Advance(def,3501,true,1) is null);Eq(progress.Index,-1);
    Pass("S40/S43 numeric-state1 and inclusive active range; outside rewind resets−1 (artificial direct lifecycle)");
    var nonmonotone=def with { Checkpoints=[def.Checkpoints[0] with { Time=3000 },def.Checkpoints[1] with { Time=2500 },def.Checkpoints[2]] };
    progress=Pre(nonmonotone);Check(progress.Advance(nonmonotone,2600,true,1) is null);Eq(progress.Index,-1);
    Pass("prefix scan stops at first future timestamp; actual prepared order is never sorted for sound");
    var f=new Resources();f.R.Prepare([],[],includeTicks:true);
    var selected=PreparedSliderSamples.Create(def,map.CircleSounds.Timing,policy,-1);var sink=new Sink();int observers=0;
    SliderSampleSelection.Request(new(def,SliderSoundKind.Tick,0,0,2500,0),selected,
        [new(2501,1,1,90,true,0),new(2502,2,7,0,false,1),new(2503,3,1,100,true,2)],f.R,policy,sink,_=>observers++);
    Eq(observers,0);Eq(sink.Requests.Count,1);Eq(sink.Requests[0].Volume,0);Eq(sink.Requests[0].Layer,0);
    Eq(sink.Requests[0].Kind,SoundEventKind.SliderTick);Eq(f.Name(sink.Requests[0].Handle),"soft-slidertick");
    Eq(SliderSampleSelection.Tick(metadata with { RawSound=14,AdditionSet=3 },[new(0,2,7,7,true,0)],0),new SelectedCircleSample(14,2,3,7,7));
    Pass("S28/S31 tick deadline+2, mode1, direct volume0/7, no Normal augmentation/observer/layer expansion");
    sink.Requests.Clear();SliderSampleSelection.Request(new(def,SliderSoundKind.Tick,0,0,3000,256),selected,
        [new(0,1,0,1,true,0)],f.R,policy,sink);Eq(sink.Requests.Single().Volume,1);
    Pass("dedicated tick volume1 bypasses standard minimum8 and attenuation");
    var edgeMap=Map(2,14,"14|14|14","0:0|0:3|0:0","1:1:9:90:",14,
        "0,500,4,1,1,100,1,0\n3505,500,4,2,7,25,0,0\n5005,500,4,3,2,60,0,0");
    var edgeDef=edgeMap.Sliders[0];var edges=PreparedSliderSamples.Create(edgeDef,edgeMap.CircleSounds!.Timing,policy,123);
    info=SliderSampleSelection.Edge(edgeDef,edgeDef.Samples!,edgeMap.CircleSounds.Timing,policy,1);
    Eq(info,new SelectedCircleSample(14,2,3,7,25));
    Eq(SliderSampleSelection.Edge(edgeDef,edgeDef.Samples!,edgeMap.CircleSounds.Timing,policy,2).Volume,60);
    sink.Requests.Clear();SliderSampleSelection.Request(new(edgeDef,SliderSoundKind.RepeatEdge,1,2,3500,512),edges,edgeMap.CircleSounds.Timing,f.R,policy,sink,_=>{Eq(sink.Requests.Count,0);observers++;});
    Seq(sink.Requests.Select(r=>r.Layer),4,2,8);Seq(sink.Requests.Select(r=>r.Volume),25,21,21);
    Check(sink.Requests.All(r=>r.Kind==SoundEventKind.SliderRepeatEdge && ReferenceEquals(r.Event!.Parent,edgeDef)));
    Pass("S27/S31 intermediate subset+5 and final end+5 use timing index/volume, edge observer before standard order");
    sink.Requests.Clear();SliderSampleSelection.Request(new(edgeDef,SliderSoundKind.RepeatEdge,1,2,3500,256),edges,
        [new(0,1,0,100,true,0)],f.R,policy with { LayeredHitSounds=true },sink);
    Seq(sink.Requests.Select(r=>r.Layer),1,4,2,8);Seq(sink.Requests.Select(r=>r.Volume),80,100,85,85);
    Pass("S48 edge Normal/Finish/Whistle/Clap order and conditional attenuation reuse verified circle machinery");
    var headSink=new Sink();SliderSampleSelection.Request(new(def,SliderSoundKind.Head,0,-1,2000,512),selected,map.CircleSounds.Timing,f.R,policy,headSink);
    Near(headSink.Requests.Single().Pan,-.2f);
    foreach(float x in new[]{0f,256f,512f})
    {sink.Requests.Clear();SliderSampleSelection.Request(new(def,SliderSoundKind.Tick,0,0,2500,x),selected,map.CircleSounds.Timing,f.R,policy,sink);Near(sink.Requests.Single().Pan,(x/512f-.5f)*.8f);}
    Pass("S35 separate head X128 and explicit stored parent presentation X0/256/512 pan operands");
    var fallback=SliderSampleSelection.Edge(def,metadata,null,policy,-1);Eq(fallback,new SelectedCircleSample(1,1,1,0,100));
    Throws<InvalidDataException>(()=>SliderSampleSelection.Tick(metadata,null,0));
    sink.Requests.Clear();SliderSampleSelection.Request(new(def,SliderSoundKind.Head,0,-1,2000,128),PreparedSliderSamples.Create(def,null,policy,-1),null,f.R,policy,sink);Eq(sink.Requests.Count,0);
    Pass("S29/S30 no-timing edge defaults distinct from throwing tick and inherited head early return");
    var shortLists=edgeDef.Samples! with { EdgeSounds=[4],Bases=[],Additions=[] };
    info=SliderSampleSelection.Edge(edgeDef,shortLists,[new(0,2,3,40,true,0)],policy,2);Eq(info.Mask,14);Eq(info.BaseSet,1);Eq(info.AdditionSet,1);
    Check(SliderAudioProgress.AggregateEdge(2000,5000,0,3500)==0);Eq(SliderAudioProgress.AggregateEdge(2000,5000,0,3501),-1);
    Eq(SliderAudioProgress.AggregateEdge(2000,5000,3,5000),2);
    Pass("exact independent nonhead list fallbacks and M1466 midpoint/count−1 including simple empty list");
    var sessionLate=Session(map);var events=new List<SliderSoundEvent>();sessionLate.SliderSound+=events.Add;
    sessionLate.Advance(1999);sessionLate.Press(def.Head.Position,2000);sessionLate.Advance(2501,def.BallPosition(2501),1);
    Eq(sessionLate.Sliders[0].Checkpoints[0].RawResult,-262144);Eq(sessionLate.Sliders[0].Audio.Index,0);
    Seq(events.Select(e=>e.Kind),SliderSoundKind.Head,SliderSoundKind.Tick);
    Pass("S05 gameplay entry-deadline failure still permits later tick with stored tracking true");
    var repeatMap=Map(2);var repeatState=Session(repeatMap);events.Clear();repeatState.SliderSound+=events.Add;repeatState.Advance(1999);
    repeatState.Advance(3501,repeat.BallPosition(3501),1);Eq(repeatState.Sliders[0].ProcessedCheckpoints,1);Eq(repeatState.Sliders[0].Checkpoints[0].RawResult,-262144);
    Eq(events.Single().Kind,SliderSoundKind.RepeatEdge);Eq(events[0].Index,1);Eq(repeatState.Sliders[0].Audio.Index,2);
    Pass("S11 audio repeat edge independent of failed gameplay item identity and successes/failures");
    var independentState=Session(map).Sliders[0];independentState.Audio.Advance(def,1999,false,1);
    Check(independentState.Audio.Advance(def,2500,true,1) is not null);Eq(independentState.ProcessedCheckpoints,0);
    var silent=Session(map);silent.NumericSliderAudioState=0;int sounds=0;silent.SliderSound+=e=>{if(e.Kind==SliderSoundKind.Tick)sounds++;};silent.Advance(1999);silent.Advance(2500,def.BallPosition(2500),1);
    Eq(silent.Sliders[0].ProcessedCheckpoints,1);Eq(sounds,0);
    Pass("S07 gameplay and audio can progress independently; explicit state0 consumes gameplay without audio producer");
    var lost=Session(repeatMap);events.Clear();lost.SliderSound+=events.Add;lost.Advance(1999);lost.Advance(3500,new(float.NaN,float.NaN),0);
    Eq(lost.Sliders[0].Audio.Index,2);lost.Advance(3501,repeat.BallPosition(3501),1);Eq(events.Count,0);
    Pass("S06/S12 tracking-false consumption and actual-session late reentry do not replay a repeat");
    var terminal=Session(map);events.Clear();terminal.SliderSound+=events.Add;terminal.Advance(1999);terminal.Press(def.Head.Position,2000);
    terminal.Advance(2500,def.BallPosition(2500),1);terminal.Advance(3000,def.BallPosition(3000),1);
    int beforeFinal=events.Count;terminal.Advance(3464,new(float.NaN,float.NaN),0);Eq(events.Count,beforeFinal);Eq(terminal.Sliders[0].Checkpoints[^1].RawResult,-524288);
    terminal.SliderSound+=e=>{if(e.Kind==SliderSoundKind.AggregateEdge){Check(terminal.Sliders[0].CompletionStarted);Eq(terminal.Sliders[0].Result,Judgement.Pending);Eq(terminal.Ledger.TotalWholeResults,0);}};
    terminal.Advance(3500);Eq(events.Count,beforeFinal+1);Eq(events[^1].Kind,SliderSoundKind.AggregateEdge);Eq(events[^1].Index,-1);
    terminal.Advance(3501);Eq(events.Count,beforeFinal+1);Check(terminal.Sliders[0].Result!=Judgement.Miss);
    Pass("S15/S16/S19 terminal failure has no sound; positive aggregate hooks once before grading/accounting");
    var pending=Session(map);events.Clear();pending.SliderSound+=events.Add;pending.Advance(1999);pending.Press(def.Head.Position,2000);pending.Advance(3500);
    Eq(pending.Sliders[0].ProcessedCheckpoints,1);Eq(pending.Sliders[0].Result,Judgement.Hit50);Eq(events.Last().Kind,SliderSoundKind.AggregateEdge);
    var zero=Session(map);int none=0;zero.SliderSound+=_=>none++;zero.Advance(1999);zero.Press(def.Head.Position,2150);zero.Advance(3500);
    Eq(none,0);Eq(zero.Sliders[0].Result,Judgement.Miss);
    Pass("S17/S18 positive aggregate with pending components versus zero ratio suppression, no backlog drain");
    var missed=Session(map);events.Clear();missed.SliderSound+=events.Add;missed.Advance(1999);missed.Press(def.Head.Position,2150);
    foreach(int t in new[]{2500,3000,3464,3500})missed.Advance(t,def.BallPosition(t),1);
    Check(events.All(e=>e.Kind!=SliderSoundKind.Head));Check(events.Any(e=>e.Kind==SliderSoundKind.Tick));Eq(events.Last().Kind,SliderSoundKind.AggregateEdge);
    Check(missed.Sliders[0].Result!=Judgement.Miss);
    Pass("S03 missed head still allows component-driven ticks and positive final aggregate");
    var triplet=Map(3).Sliders[0];progress=Pre(triplet);var families=new List<string>();
    foreach(int t in new[]{2500,3000,3500,4000,4500,5000,5500,6000,6464})
        if(progress.Advance(triplet,t,true,1) is {} next)families.Add((next.Kind==SliderSoundKind.Tick?"t":"e")+next.Index);
    Seq(families,"t0","t1","e1","t3","t4","e2","t6","t7");
    Pass("S20 three-traversal discrete prefix sequence, final entry excluded");
    var dynamic=new Resources();dynamic.Files.Data["soft-slidertick7.wav"]=Bytes();dynamic.R.Prepare([7],[],includeTicks:true);
    int h=dynamic.R.ResolveTick(2,7);Eq(dynamic.Name(h),"soft-slidertick7.wav");int probes=dynamic.Files.Reads.Count;
    Check(dynamic.R.ResolveTick(2,99)>0);Eq(dynamic.Files.Reads.Count,probes);
    dynamic.R.Caches.Map["soft-slidertick99"]=0;Eq(dynamic.R.ResolveTick(2,99),0);dynamic.R.Caches.Map["soft-slidertick99"]=-1;
    Eq(dynamic.R.ResolveTick(2,99),dynamic.R.ResolveTick(2,0));
    Pass("S36 dedicated dynamic tick cache lookup,0 retention and missing/−1 plane0 fallback without I/O");
    var matrix=new Resources();
    foreach(int bank in new[]{1,2,3})
    {
        string name=(bank==1?"normal":bank==2?"soft":"drum")+"-slidertick";
        matrix.Files.Data[name+".wav"]=Bytes();matrix.Files.Data[name+"2.wav"]=Bytes();
    }
    matrix.R.Prepare([],[],includeTicks:true);
    foreach(int bank in new[]{1,2,3})
    {
        string name=(bank==1?"normal":bank==2?"soft":"drum")+"-slidertick";
        Eq(matrix.Name(matrix.R.ResolveTick(bank,0)),name);
        Eq(matrix.Name(matrix.R.ResolveTick(bank,1)),name+".wav");
        Eq(matrix.Name(matrix.R.ResolveTick(bank,2)),name+"2.wav");
        Check(new[]{matrix.R.ResolveTick(bank,0),matrix.R.ResolveTick(bank,1),matrix.R.ResolveTick(bank,2)}.Distinct().Count()==3);
    }
    var ordering=new Resources();ordering.Files.Data["normal-slidertick.mp3"]=Bytes();ordering.Files.Data["normal-slidertick.ogg"]=Bytes();ordering.R.StandardTick(1,1);
    Seq(ordering.Files.Reads,"normal-slidertick.wav","normal-slidertick.mp3");ordering.Files.Reads.Clear();ordering.R.StandardTick(1,0);Eq(ordering.Files.Reads.Count,0);
    Pass("mode1 three-bank/three-plane dedicated table, standard extension precedence and index0 map exclusion");
    var absent=new Resources(false);absent.R.Prepare([],[],includeTicks:true);sink.Requests.Clear();
    SliderSampleSelection.Request(new(def,SliderSoundKind.Tick,0,0,2500,128),selected,map.CircleSounds.Timing,absent.R,policy,sink);Eq(sink.Requests.Count,0);
    absent.R.Caches.Map["normal-slidertick7"]=0;sink.Requests.Clear();SliderSampleSelection.Request(new(def,SliderSoundKind.Tick,0,0,2500,128),selected,[new(0,1,7,100,true,0)],absent.R,policy,sink);
    Eq(sink.Requests.Single().Handle,0);Eq(sink.Requests.Single().Kind,SoundEventKind.SliderTick);
    progress=Pre(def);Check(progress.Advance(def,2500,true,1) is not null);Eq(progress.Index,0);
    Pass("S37/S38 missing−1 submits nothing, invalid0 still requests; audio index does not roll back");
    var throwSession=Session(map);throwSession.Advance(1999);throwSession.SliderSound+=_=>throw new IOException("optional slider sink");throwSession.Press(def.Head.Position,2000);
    Eq(throwSession.Ledger.Score,30);throwSession.Advance(2500,def.BallPosition(2500),1);Eq(throwSession.Sliders[0].Audio.Index,0);
    Eq(throwSession.Sliders[0].SuccessfulCheckpoints,1);Check(throwSession.SliderSoundError!.Contains("optional slider sink"));
    Pass("S39 optional helper failures after progression store do not undo index, scoring or component judgement");
    var panSession=Session(map);events.Clear();panSession.SliderSound+=events.Add;panSession.Advance(1999);panSession.PublishSliderPresentationX(def,17);
    panSession.Advance(2500,def.BallPosition(2500),1);Eq(events.Single().PresentationX,17f);
    Throws<ArgumentOutOfRangeException>(()=>panSession.PublishSliderPresentationX(def,float.NaN));
    Pass("ENGINEERING last-published ball pan is distinct from head/current pointer/new required position");
    var customMap=Map(2,14,"2|4|8","1:2|2:3|3:1","3:1:9:90:parent.wav");
    File.WriteAllBytes(Path.Combine(customMap.CircleSounds.Directory,"parent.wav"),File.ReadAllBytes(Path.Combine(args[1],"normal-hitnormal.wav")));
    using(var library=new HitSoundLibrary(new(){DefaultPackDirectory=args[1],LayeredHitSounds=false}))
    using(var assets=await library.PrepareAsync(customMap,CancellationToken.None))
    {
        var pd=assets.Sliders[customMap.Sliders[0]];Check(pd.ParentCustomHandle>0);Eq(pd.Head.Index,0);Eq(pd.Head.Volume,0);Eq(pd.Head.Filename,"");
        var recorded=new Sink();SliderSampleSelection.Request(new(pd.Parent,SliderSoundKind.Head,0,-1,2000,128),pd,assets.Source.Timing,assets.Resources,policy,recorded);
        SliderSampleSelection.Request(new(pd.Parent,SliderSoundKind.RepeatEdge,1,2,3500,512),pd,assets.Source.Timing,assets.Resources,policy,recorded);
        SliderSampleSelection.Request(new(pd.Parent,SliderSoundKind.Tick,0,0,2500,256),pd,assets.Source.Timing,assets.Resources,policy,recorded);
        Check(recorded.Requests.All(r=>r.Handle!=pd.ParentCustomHandle));Check(recorded.Requests.Any(r=>r.Kind==SoundEventKind.SliderHead));Check(recorded.Requests.Any(r=>r.Kind==SoundEventKind.SliderTick));
        Check(assets.Samples.Values.Any(s=>s.Name=="normal-slidertick"));
    }
    Pass("S21/S32/S33 actual local preparation keeps parent handle without copying/dispatching it; original15-name pack supplies discrete families");
    foreach(string name in new[]{"my-slider.wav","slider.wav","my-slidertick.wav"})
    {
        var cm=Map(payload:"0:0:0:0:"+name);Eq(cm.Sliders[0].Samples!.CustomLoopFlag,name=="my-slider.wav");
        File.WriteAllBytes(Path.Combine(cm.CircleSounds!.Directory,name),File.ReadAllBytes(Path.Combine(args[1],"normal-hitnormal.wav")));
        using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=args[1]});using var assets=await lib.PrepareAsync(cm,CancellationToken.None);
        var ps=assets.Sliders[cm.Sliders[0]];Eq(assets.Samples[ps.ParentCustomHandle].Loop,name=="my-slider.wav");
    }
    Pass("S34 parent loader loop-flag operands retained as metadata only, never a discrete loop voice command");
    using(var library=new HitSoundLibrary(new(){DefaultPackDirectory=args[1]}))
    {
        var colliding=Map(payload:"0:0:0:0:normal-hitnormal.wav");await ThrowsAsync<InvalidDataException>(()=>library.PrepareAsync(colliding,CancellationToken.None));
        var plain=Map();using var first=await library.PrepareAsync(plain,CancellationToken.None);
        var cloned=plain with {Title="clone"};await ThrowsAsync<InvalidDataException>(()=>library.PrepareAsync(cloned,CancellationToken.None));
    }
    Pass("explicit ambiguous parent-cache and unsupported cloned registration rejected without guessed resource consequences");
    var queue=new Queue<Action>();var played=new List<SamplePlaybackRequest>();var playback=new CircleSoundPlayback(played.Add,queue.Enqueue,()=>{});playback.Resume();
    sink.Requests.Clear();SliderSampleSelection.Request(new(def,SliderSoundKind.Tick,0,0,2500,256),selected,map.CircleSounds.Timing,f.R,policy,sink);
    var request=sink.Requests.Single();playback.Enqueue(request);playback.Pause();queue.Dequeue()();Eq(played.Count,0);
    playback.Resume();playback.Enqueue(request);playback.Restart();queue.Dequeue()();Eq(played.Count,0);
    playback.Enqueue(request);queue.Dequeue()();Eq(played.Count,1);Eq(played[0].Kind,SoundEventKind.SliderTick);
    playback.Enqueue(request);playback.Dispose();queue.Dequeue()();Eq(played.Count,1);
    var reset=Session(map);Eq(reset.Sliders[0].Audio.Index,0);Eq(reset.Ledger.Score,0);reset.Advance(0);Eq(reset.Sliders[0].Audio.Index,-1);
    Pass("shared R11 pause/restart/replacement/dispose generation policy applies to tagged slider requests; fresh audio0 remains explicit");
    var enabled=Session(map,true);var disabled=Session(map,true);int captured=0;enabled.SliderSound+=e=>{captured++;SliderSampleSelection.Request(e,selected,map.CircleSounds.Timing,f.R,policy,new Sink());};
    Eq(captured,0);enabled.Advance(1999);disabled.Advance(1999);enabled.Press(def.Head.Position,2000);disabled.Press(def.Head.Position,2000);
    foreach(int t in new[]{2501,3000,3464,3500,4000})
    {
        enabled.Advance(t,def.BallPosition(t),1);disabled.Advance(t,def.BallPosition(t),1);
        Eq(enabled.Ledger.Score,disabled.Ledger.Score);Eq(enabled.Combo,disabled.Combo);Eq(enabled.MaxCombo,disabled.MaxCombo);Eq(enabled.Accuracy,disabled.Accuracy);
        Check(enabled.ObjectResults.SequenceEqual(disabled.ObjectResults));Eq(enabled.Ledger.EventCount,disabled.Ledger.EventCount);
        Eq(enabled.Health!.State.H,disabled.Health!.State.H);Eq(enabled.Health.State.V,disabled.Health.State.V);Eq(enabled.Health.State.A,disabled.Health.State.A);
        Eq(enabled.Health.ResultEvents,disabled.Health.ResultEvents);Seq(enabled.Sliders[0].Checkpoints.Select(c=>c.RawResult),disabled.Sliders[0].Checkpoints.Select(c=>c.RawResult).ToArray());
    }
    Check(captured>0);Eq(enabled.Scoring.K,disabled.Scoring.K);
    Pass("private HP calibration silent; matched actual sound selection leaves score/accuracy/combo/judgement/HP stream unchanged");
    var circleMap=Beatmap.Load(Path.Combine(args[0],"reconstructed/CircleGame/Fixtures/first-circles.osu"));var circles=Session(circleMap);int circleHooks=0,sliderHooks=0;
    circles.PositiveCircleSound+=_=>circleHooks++;circles.SliderSound+=_=>sliderHooks++;
    foreach(var c in circleMap.Circles)circles.Press(c.Position,c.StartTime);
    Eq(circleHooks,circleMap.Circles.Count);Eq(sliderHooks,0);
    var fixture=Beatmap.Load(Path.Combine(args[0],"reconstructed/CircleGame/Fixtures/discrete-slider-r12.osu"));
    using(var lib=new HitSoundLibrary(new(){DefaultPackDirectory=args[1]}))using(var prepared=await lib.PrepareAsync(fixture,CancellationToken.None))Eq(prepared.Sliders.Count,1);
    Pass("ordinary circle hooks unchanged; independently authored R12 mixed fixture prepares using optional local15-file pack");
    Console.WriteLine($"R12 SLIDER SOUND CHECKS: {groups} passed (bounded E039 discrete controls; no continuous audio, native playback or game window)");
}
finally { Directory.Delete(scratch,true); }

Beatmap Map(int spans=1,int mask=0,string? edges=null,string? sets=null,string? payload=null,int version=14,string timing="0,500,4,1,1,100,1,0")
{
    string dir=Path.Combine(scratch,"map"+serial++);Directory.CreateDirectory(dir);
    string row=$"128,192,2000,2,{mask},L|428:192,{spans},300";
    if(edges is not null||sets is not null||payload is not null)row+=","+(edges??"");
    if(sets is not null||payload is not null)row+=","+(sets??"");
    if(payload is not null)row+=","+payload;
    string path=Path.Combine(dir,"practice.osu");File.WriteAllText(path,$"osu file format v{version}\n[General]\nMode:0\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n{timing}\n[HitObjects]\n{row}\n");return Beatmap.Load(path);
}
static CircleSession Session(Beatmap map,bool hp=false)=>CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map),hp);
static SliderAudioProgress Pre(SliderDefinition definition){var p=new SliderAudioProgress();p.Advance(definition,definition.Head.StartTime-1,false,1);return p;}
static byte[] Bytes()=>new byte[2048];
void Pass(string name){groups++;Console.WriteLine("PASS R12 "+name);}
static void Check(bool value){if(!value)throw new Exception("R12 bounded assertion failed.");}
static void Eq<T>(T actual,T expected){if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception($"Expected {expected}; got {actual}");}
static void Seq<T>(IEnumerable<T> actual,params T[] expected){if(!actual.SequenceEqual(expected))throw new Exception($"Expected [{string.Join(',',expected)}]; got [{string.Join(',',actual)}]");}
static void Near(float actual,float expected){if(Math.Abs(actual-expected)>1e-7f)throw new Exception($"Expected {expected}; got {actual}");}
static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
static async Task ThrowsAsync<T>(Func<Task> action)where T:Exception{try{await action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
sealed class Sink:ISamplePlaybackSink{public List<SamplePlaybackRequest> Requests {get;}=[];public void Enqueue(SamplePlaybackRequest r)=>Requests.Add(r);}
sealed class Files:ISampleFiles
{
    public Dictionary<string,byte[]> Data {get;}=new(StringComparer.Ordinal);public List<string> Reads {get;}=[];
    public bool Exists(string n)=>Data.ContainsKey(n);public byte[]? Read(string n){Reads.Add(n);return Data.GetValueOrDefault(n);}
}
sealed class Default(bool all):IDefaultSamples{public byte[]? Ui(string k)=>null;public byte[]? Gameplay(string k)=>all?new byte[2048]:null;}
sealed class Loader:ISampleLoader
{
    public Dictionary<int,SampleBytes> Loaded {get;}=[];private int next;
    public int Load(SampleBytes b){int h=++next;Loaded[h]=b;return h;}public void Free(int h)=>Loaded.Remove(h);
}
sealed class Resources
{
    public Files Files {get;}=new();public Loader Loader {get;}=new();public CircleSampleResources R {get;}
    public Resources(bool all=true){R=new(Files,null,new Default(all),Loader,new(14,false,false,true,true,"Default",true,CultureInfo.InvariantCulture));}
    public string Name(int h)=>Loader.Loaded[h].Name;
}
