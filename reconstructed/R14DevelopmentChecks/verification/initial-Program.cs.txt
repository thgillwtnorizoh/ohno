using System.Numerics;
using System.Globalization;
using System.Text;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
int groups = 0;
void Yes(bool value) { if (!value) throw new Exception("R14 assertion failed at group " + (groups + 1)); }
void Eq<T>(T actual, T expected) => Yes(EqualityComparer<T>.Default.Equals(actual, expected));
void Near(float actual, float expected) => Yes(Math.Abs(actual - expected) < .000001f);
void Pass(string label) => Console.WriteLine($"PASS R14 {++groups:00} {label}");
CircleSoundPolicy policy = new(true, false, true, 0, 100, false, 100, 100);
var backend = new RecordingSampleChannels { QueryLength = (_, _) => 8193 };
var queue = new Queue<Action>();
float replacementGain = .25f, startGain = .5f;
var spin = new SharedSpinnerChannel(backend, () => 777, () => replacementGain, () => startGain, a => queue.Enqueue(a));
void Drain() { while (queue.TryDequeue(out var a)) a(); }
Eq(spin.Channel, 0); spin.Refresh(); Eq(spin.Channel, 777);
Yes(backend.Operations.Select(o => o.Name).SequenceEqual(new[] { "Acquire", "Attribute", "Attribute" }));
Yes(backend.Operations.Where(o => o.Name == "Attribute").Select(o => o.Attribute).SequenceEqual(new[] { 11, 2 }));
Eq(backend.Operations[0].Value, 0f); Near(backend.Operations[^1].Value, .25f);
Pass("A39 spinner default0, unprefixed resolution, Acquire(false), attribute11 then gain");
foreach(int state in new[] { 0, 1, 2, 3, -1 })
{
    backend.States[spin.Channel] = state; backend.Operations.Clear(); spin.Refresh();
    Eq(backend.Operations.Count(o => o.Name == "Stop"), state == 1 ? 1 : 0);
}
Pass("conditional prior active1 Stop, other numeric states still replace");
backend.Operations.Clear(); spin.Channel = 0; spin.Refresh(); Yes(!backend.Operations.Any(o => o.Name is "Activity" or "Stop"));
Pass("old channel0 skips activity/Stop, without repairing native sentinels");
foreach(int sample in new[] {-1,0})
{
 var invalid = new SharedSpinnerChannel(backend, () => sample, () => .25f, () => .5f, a => queue.Enqueue(a));
 invalid.Refresh(); Eq(invalid.Channel, sample);
}
Pass("missing minus1 and invalid0 remain opaque in pure acquisition model");
var globalBackend = new RecordingSampleChannels();
var global = new SharedSliderChannels(new GlobalSampleTiming([new(0,1,1,25,true,0),new(10,2,7,50,false,1)]),globalBackend,(row,set,index)=>101+row);
var globalSpin = global.AttachSpinner(globalBackend,()=>777); global.Clock(0);
Eq(globalSpin.Channel,777); Eq(global.Channel0,101); Eq(global.Channel1,102);
globalBackend.Operations.Clear(); global.Clock(0); Eq(globalBackend.Operations.Count,0);
global.Clock(20); Eq(globalSpin.Channel,777); Eq(globalBackend.Operations.Count(o=>o.Name=="Acquire"),1);
Pass("A39/A40 admitted global bank/index refresh always reacquires same spinner sample separately from sliders");
globalBackend.Operations.Clear();global.ApplicationState=9;global.Refresh(3,false);Eq(globalBackend.Operations.Count,0);
global.Refresh(3,true);Yes(globalBackend.Operations.Any(o=>o.Name=="Acquire"&&o.Channel==777));global.ApplicationState=1;
Pass("unchanged/application and forced global refresh gates retained");
global.Channel0=101;global.Channel1=202;globalSpin.Channel=303;globalBackend.Operations.Clear();
global.Timing.Reset();global.Clock(0);globalBackend.Operations.Clear(); // global point volume25
// Prior write mutates settings, later write mutates spinner target. One gain across all three.
globalBackend.OnOperation=o=>{if(o.Name=="Attribute"&&o.Channel==101)global.UniversalVolume=100;if(o.Name=="Attribute"&&o.Channel==202)globalSpin.Channel=404;};
global.SettingsChanged(80,50);
Yes(globalBackend.Operations.Select(o=>o.Channel).SequenceEqual(new[]{101,202,404}));
foreach(var o in globalBackend.Operations)Near(o.Value,.1f);globalBackend.OnOperation=null;
Pass("A65 R13-C01 single gain across slider0/slider1/spinner with fresh spinner target");
foreach(var item in new[]{(0f,20000f),(.5f,40000f),(1f,60000f),(2f,100000f),(3f,100000f),(-1f,-20000f)})
{backend.Operations.Clear();spin.Channel=777;spin.Modulate(item.Item1);Eq(backend.Operations.Count,2);Eq(backend.Operations[1].Attribute,1);Eq(backend.Operations[1].Value,item.Item2);}
Pass("A18-A23 finite Single frequency0/.5/1/2/3/-1; upper cap only, no lower floor");
foreach(long length in new[]{long.MinValue,-1L,0L,8191L,8192L,8193L,long.MaxValue})
{backend.QueryLength=(_,_)=>length;backend.Operations.Clear();spin.Modulate(1f);Eq(backend.Operations.Count,length>8192?2:1);Eq(backend.Operations[0].Attribute,0);}
Pass("A28-A30 signed Int64 native length: strict >8192, mode0");
backend.QueryLength=(_,_)=>8193;spin.Channel=101;
backend.OnOperation=o=>{if(o.Name=="Length")spin.Channel=303;};backend.Operations.Clear();spin.Modulate(1);
Eq(backend.Operations[0].Channel,101);Eq(backend.Operations[1].Channel,303);Yes(!backend.Operations.Any(o=>o.Name=="Activity"));backend.OnOperation=null;
Pass("A31/A32 length query old target then fresh frequency target; no activity gate");
foreach(float r in new[]{float.NaN,float.PositiveInfinity,float.NegativeInfinity,float.MaxValue})
{backend.Operations.Clear();spin.Modulate(r);Eq(spin.FrequencyStatus,FrequencyCapability.Unsupported);Yes(backend.Operations.All(o=>o.Name=="Length"));}
Pass("A24-A27 unsupported conversion produces no invented CLR frequency");
backend.QueryLength=(_,_)=>null;spin.Continue(.5f);Eq(spin.FrequencyStatus,FrequencyCapability.Unsupported);Eq(queue.Count,1);Drain();backend.QueryLength=(_,_)=>8193;
Pass("unknown native length is explicit unsupported modulation; continuing start remains available");
backend.FrequencyCapability=FrequencyCapability.Unsupported;backend.Operations.Clear();spin.Modulate(.5f);Eq(spin.FrequencyStatus,FrequencyCapability.Unsupported);Eq(backend.Operations.Count,1);backend.FrequencyCapability=FrequencyCapability.Supported;
Pass("explicit unsupported frequency capability is not a successful no-op attribute setter");
backend.OnOperation=o=>{if(o.Name=="Length")throw new IOException("length failure");};bool threw=false;try{spin.Modulate(.5f);}catch(IOException){threw=true;}Yes(threw);Eq(spin.FrequencyStatus,FrequencyCapability.Failed);backend.OnOperation=null;
Pass("frequency failure status and pure-model exception propagation");
backend.Operations.Clear();spin.Channel=101;spin.Continue(.5f);Yes(backend.Operations.Select(o=>o.Name).SequenceEqual(new[]{"Length","Attribute"}));Eq(queue.Count,1);
spin.Channel=303;startGain=.75f;backend.Operations.Clear();Drain();Eq(backend.Operations[0].Channel,303);Near(backend.Operations[0].Value,.75f);Eq(backend.Operations[1].Name,"Play");Eq(backend.Operations[1].Value,0f);
Pass("A33-A35 synchronous frequency before queued start; fresh gain/channel and Play(false)");
spin.Continue(null);spin.Continue(null);Eq(queue.Count,2);backend.Operations.Clear();Drain();Eq(backend.Operations.Count(o=>o.Name=="Play"),2);
Pass("repeated start submissions have no native-active or pending-start suppression");
spin.Channel=101;spin.Continue(null);backend.OnOperation=o=>{if(o.Name=="Attribute")spin.Channel=402;};backend.Operations.Clear();Drain();Eq(backend.Operations[0].Channel,101);Eq(backend.Operations[1].Channel,402);backend.OnOperation=null;
Pass("start callback rereads spinner field between gain assignment and Play");
spin.QueuePause();spin.Channel=501;backend.Operations.Clear();Drain();Eq(backend.Operations.Single().Name,"Pause");Eq(backend.Operations.Single().Channel,501);
Pass("A60 queued Pause targets replacement spinner channel, not Stop/release");
foreach(int invalidHandle in new[]{0,-1}){spin.Channel=invalidHandle;backend.Operations.Clear();spin.Modulate(1f);Eq(backend.Operations[1].Channel,invalidHandle);Eq(backend.Operations[1].Value,60000f);}
Pass("A33 artificial invalid channel does not invent a frequency validity/activity guard");
var interrupted=new SpinnerProgressObservation();backend.OnOperation=o=>{if(o.Name=="Length")throw new IOException("observed change helper");};
threw=false;try{interrupted.Observe(1,15,()=>spin.Observe(interrupted.Count,15,true));}catch(IOException){threw=true;}Yes(threw);Eq(interrupted.Count,1);Eq(interrupted.CachedInteger,0);backend.OnOperation=null;
Pass("A35 pure observed-change exception leaves advanced C and stale cache before component selection");
var observation = new SpinnerProgressObservation();int changes=0;observation.Observe(8,10,()=>{changes++;Eq(observation.Count,1);Eq(observation.CachedInteger,0);});Eq(observation.Count,1);Eq(changes,1);
observation.Observe(8.9f,10,()=>changes++);Eq(changes,1);
Pass("A09/A11 jumped integer changes C once before raw0 classification/cache; unchanged integer silent");
backend.Operations.Clear();spin.Observe(1,10,true);Eq(backend.Operations[1].Value,24000f);backend.Operations.Clear();spin.Observe(1,10,false);Eq(backend.Operations.Count,0);
Pass("first C1/raw0 frequency possible; disabled setting suppresses observation modulation only");
spin.Observe(1,0,true);Eq(spin.FrequencyStatus,FrequencyCapability.Unsupported);
Pass("N0 modulation unsupported independently of modern N0 final grade");
var moving=new SpinnerState(new(1000,3000,0,null),5);var stages=new List<SpinnerSoundStage>();float oldProgress=0;
moving.Sound+=(s,e)=>{stages.Add(e);if(e==SpinnerSoundStage.Motion)Eq(s.Progress,oldProgress);};
moving.SampleAngle(0,1000,20,true);moving.SampleAngle(.3,1020,20,true);oldProgress=moving.Progress;moving.Integrate(1020,20);Yes(stages.Contains(SpinnerSoundStage.Motion));Yes(moving.Progress>oldProgress);
Pass("A02 nonzero smoothed U helper occurs before P accumulation");
stages.Clear();moving.SampleAngle(.6,1040,20,false);oldProgress=moving.Progress;moving.Integrate(1040,20);Yes(stages.Contains(SpinnerSoundStage.Motion));Yes(!moving.InputHeld);
Pass("A04 residual smoothed motion after release still produces continuation");
var stationary=new SpinnerState(new(1000,3000,0,null),5);stages.Clear();stationary.Sound+=(_,e)=>stages.Add(e);
stationary.Integrate(1000,20);stationary.Integrate(3000,20);stationary.Integrate(999,20);stationary.Integrate(3001,20);stationary.Integrate(1020,0);Eq(stages.Count,0);
stationary.Integrate(1020,20);Yes(stages.SequenceEqual(new[]{SpinnerSoundStage.ZeroMotion}));
Pass("A01/A05/A06/A08 strict interval/positive step; zero U cleanup only inside gate");
var bonusObservation=new SpinnerProgressObservation();int bonuses=0;
for(int c=1;c<=17;c++){int expectedC=c;int raw=bonusObservation.Observe(c,10,bonus:()=>{bonuses++;Eq(bonusObservation.Count,expectedC);Eq(bonusObservation.CachedInteger,expectedC-1);});Eq(raw==16384,c==15||c==17);}
Eq(bonuses,2);Yes(!SpinnerSampleSelection.BonusAllowed(13,10,0));Yes(!SpinnerSampleSelection.BonusAllowed(14,10,0));Yes(SpinnerSampleSelection.BonusAllowed(15,10,0));Yes(!SpinnerSampleSelection.BonusAllowed(15,10,4194304));
Pass("A12-A16 bonus threshold/parity, pre-cache producer and numeric4194304 sound-only gate");
var sink=new Sink();var def=new SpinnerDefinition(1000,3000,14,null);var registration=new List<SpinnerBonusPlayer>();
var player=SpinnerSampleSelection.Bonus(def,33,25,policy,sink,p=>{Eq(sink.Requests.Count,0);registration.Add(p);});
Eq(registration.Count,1);Eq(player.InitialVolume,.25f);Yes(player.Restart&&!player.Secondary);var request=sink.Requests.Single();Eq(request.Kind,SoundEventKind.SpinnerBonus);Eq(request.Pan,0f);Eq(request.Factor,1f);Eq(request.Volume,25);Near(request.Gain,.25f);Yes(ReferenceEquals(request.BonusPlayer,player));
Pass("bonus independent player registration before queue, volume/100f, true/false, no pan/layers/attenuation");
foreach(int handle in new[]{-1,0}){sink.Requests.Clear();var p=SpinnerSampleSelection.Bonus(def,handle,0,policy,sink);Eq(p.Sample,handle);Eq(sink.Requests.Single().Handle,handle);Eq(p.InitialVolume,0f);}
Pass("A48 missing/0 bonus still registers logical player; actual worker validity separate");
var localQueue=new Queue<Action>();int played=0;var playback=new CircleSoundPlayback(_=>played++,a=>localQueue.Enqueue(a),()=>{});playback.Resume();
SpinnerSampleSelection.Bonus(def,-1,25,policy,playback);SpinnerSampleSelection.Bonus(def,0,25,policy,playback);while(localQueue.TryDequeue(out var action))action();Eq(played,0);
Pass("missing bonus handles are safely rejected by production dispatch without scoring impact");
int eventBonuses=0;var rawSpinner=new SpinnerState(def,5);rawSpinner.Sound+=(_,e)=>{if(e==SpinnerSoundStage.Bonus)eventBonuses++;};rawSpinner.Observation.Observe(1,rawSpinner.Required);rawSpinner.Observation.Observe(2,rawSpinner.Required);rawSpinner.SampleAngle(0,1020,20,true);Eq(eventBonuses,0);
Pass("ordinary raw4096/8192 have no dedicated bonus hook");
var files=new Files();var defaults=new Defaults();var loader=new Loader();var resources=new CircleSampleResources(files,null,defaults,loader,new(14,false,false,true,true,"Default",true,CultureInfo.InvariantCulture));
int spinHandle=resources.SpinnerResource(true),bonusHandle=resources.SpinnerResource(false);Yes(spinHandle>0&&bonusHandle>0&&spinHandle!=bonusHandle);
Eq(loader.Samples[spinHandle-1].Name,"spinnerspin.wav");Yes(loader.Samples[spinHandle-1].Loop);Yes(!loader.Samples[bonusHandle-1].Loop);
Yes(files.Reads.SequenceEqual(new[]{"spinnerspin.wav","spinnerbonus.wav"}));
Pass("A41/A42 unprefixed unsuffixed map index0 resources; loop spin only; standard extensions");
files.Reads.Clear();Eq(resources.SpinnerResource(true),spinHandle);Eq(files.Reads.Count,0);
Pass("named cache lookup before providers, no loop/index cache invention");
resources.Prepare([1,2,7],[],true);resources.PrepareContinuing([7]);Yes(resources.ResolveContinuing(0,1,0)>0&&resources.ResolveTick(1,0)>0&&resources.Resolve(1,1,0)>0);
Pass("mode0/mode1/mode2 resource tables remain separate from named spinner resources");
var failLoader=new Loader{Result=0};var zeros=new CircleSampleResources(new Files(),null,new Defaults(),failLoader,new(14,false,false,true,true,"Default",true,CultureInfo.InvariantCulture));Eq(zeros.SpinnerResource(true),0);
var absent=new CircleSampleResources(new Missing(),null,new MissingDefaults(),loader,new(14,false,false,true,true,"Default",true,CultureInfo.InvariantCulture));Eq(absent.SpinnerResource(true),-1);
Pass("A43-A47 named map0 preserved versus missing minus1; bytes do not prove decoding");
var fallback=new CircleSampleResources(new Missing(),null,new Defaults(),loader,new(14,false,false,true,true,"Default",true,CultureInfo.InvariantCulture));int defaultSpin=fallback.SpinnerResource(true);Eq(loader.Samples[defaultSpin-1].Origin,SampleOrigin.Default);Yes(loader.Samples[defaultSpin-1].Loop);
var skinResource=new CircleSampleResources(new Missing(),new Files(),new Defaults(),loader,new(14,false,false,true,true,"Private",true,CultureInfo.InvariantCulture));int skinSpin=skinResource.SpinnerResource(true);Eq(loader.Samples[skinSpin-1].Origin,SampleOrigin.Skin);
Pass("A45 unprefixed map-to-admitted-skin-to-independent-default hierarchy");
var firstBytes=new Files();var failedLoader=new Loader{Result=-1};var termination=new CircleSampleResources(firstBytes,null,new Defaults(),failedLoader,new(14,false,false,true,true,"Default",true,CultureInfo.InvariantCulture));Eq(termination.SpinnerResource(true),-1);Yes(firstBytes.Reads.SequenceEqual(new[]{"spinnerspin.wav"}));Eq(failedLoader.Samples.Count,2);
Pass("A48 first nonnull map bytes terminate extension probes even load minus1; default stage remains separate");
foreach(int missingSample in new[]{-1,0}){var unavailableBackend=new RecordingSampleChannels{GetChannel=(_,_)=>0};var unavailable=new SharedSpinnerChannel(unavailableBackend,()=>missingSample,()=>1,()=>1,a=>queue.Enqueue(a));unavailable.Refresh();Eq(unavailable.Channel,0);Eq(unavailableBackend.Operations.First().Channel,missingSample);}
Pass("A46 native sample sentinels versus acquired-channel0 remain distinct injected operands");
foreach(var (c,result) in new[]{(16,Judgement.Hit300),(14,Judgement.Hit100),(3,Judgement.Hit50),(2,Judgement.Miss)}){var originalCase=new SpinnerState(new(2000,5000,0,null),5);Eq(originalCase.Required,15);for(int j=1;j<=c;j++)originalCase.Observation.Observe(j,15);int positiveCalls=0;originalCase.Sound+=(_,e)=>{if(e==SpinnerSoundStage.PositiveCompletion)positiveCalls++;};originalCase.Complete(5001);Eq(originalCase.Result,result);Eq(positiveCalls,result==Judgement.Miss?0:1);}
Pass("A49-A52 literal N15 C16/14/3/2 positive minimum and later quality outcomes");
foreach(var (count,grade) in new[]{(0,Judgement.Miss),(2,Judgement.Hit50),(9,Judgement.Hit100),(11,Judgement.Hit300)})
{
 var state=new SpinnerState(def,5);for(int i=1;i<=count;i++)state.Observation.Observe(i,state.Required);
 var order=new List<SpinnerSoundStage>();state.Sound+=(s,e)=>{order.Add(e);if(e==SpinnerSoundStage.PositiveCompletion){Eq(s.Result,Judgement.Pending);Eq(s.FinalRaw,0);Yes(s.CompletionStarted);}};
 Yes(!state.Complete(3000));Yes(state.Complete(3001));Eq(state.Result,grade);Eq(order[0],SpinnerSoundStage.Cleanup);Eq(order.Contains(SpinnerSoundStage.PositiveCompletion),count>0);
}
Pass("A49-A52 strict end, cleanup then positive pre-quality hook for50/100/300; MISS silent");
var shortState=new SpinnerState(new(1000,1100,0,null),5);int positive=0;shortState.Sound+=(_,e)=>{if(e==SpinnerSoundStage.PositiveCompletion)positive++;};shortState.Complete(1101);Eq(positive,1);Eq(shortState.Result,Judgement.Hit300);
Pass("N0 modern positive completion is independent of unsupported modulation");
var sample=new CircleSampleMetadata(new(new(256,192),3000),14,14,0,0,0,0,"",null,null);
var prepared=new PreparedSpinnerSamples(new(def,0,sample),-1);var timing=new[]{new SampleTiming(0,1,1,25,true,0),new SampleTiming(3000,2,2,50,false,1),new SampleTiming(3003,3,7,80,false,2)};
sink.Requests.Clear();SelectedCircleSample? selected=null;SpinnerSampleSelection.Completion(prepared,timing,new(256,192),resources,policy,sink,i=>selected=i);
Eq(selected!.BaseSet,2);Eq(selected.Index,2);Eq(selected.Volume,50);Yes(sink.Requests.Select(r=>r.Layer).SequenceEqual(new[]{4,2,8}));Yes(sink.Requests.All(r=>r.Pan==0&&r.Kind==SoundEventKind.SpinnerCompletion));
Pass("A53/A54 end lookup without+5 selects end bank/index, not start or end+3");
sink.Requests.Clear();SpinnerSampleSelection.Completion(prepared,timing,new(512,192),resources,policy with{LayeredHitSounds=true},sink);Yes(sink.Requests.Select(r=>r.Layer).SequenceEqual(new[]{1,4,2,8}));Near(sink.Requests[0].Pan,.4f);Yes(sink.Requests.Select(r=>r.Volume).SequenceEqual(new[]{40,50,42,42}));
Pass("A55/A56 conditional Normal expansion order/attenuation and fresh stored center pan");
sink.Requests.Clear();SpinnerSampleSelection.Completion(prepared with{Custom=0},timing,new(0,192),resources,policy,sink);Eq(sink.Requests.Count,1);Eq(sink.Requests[0].Handle,0);Eq(sink.Requests[0].Layer,0);Eq(sink.Requests[0].Volume,50);
SpinnerSampleSelection.Completion(prepared,null,new(256,192),resources,policy,sink);Eq(sink.Requests.Count,1);
Pass("A57-A59 completion custom0 bypasses layers; absent timing emits nothing");
var overridden=prepared with { Metadata=prepared.Metadata with { Payload=sample with{BaseSet=3,AdditionSet=1,Index=7,Volume=1} } };
sink.Requests.Clear();SpinnerSampleSelection.Completion(overridden,timing,new(256,192),resources,policy,sink,i=>selected=i);Eq(selected!.BaseSet,3);Eq(selected.Index,7);Eq(selected.Volume,1);Yes(sink.Requests.Select(r=>r.Volume).SequenceEqual(new[]{8,6,6}));
sink.Requests.Clear();SpinnerSampleSelection.Completion(overridden with{Custom=33},timing,new(256,192),resources,policy,sink);Eq(sink.Requests.Single().Volume,1);
Pass("object overrides preserved; standard floor8 versus custom no floor");
string scratch=Path.Combine(Path.GetTempPath(),"r14-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
try
{
 string path=Path.Combine(scratch,"map.osu");
 File.WriteAllText(path,"osu file format v14\n[General]\nMode:0\n[Metadata]\nTitle:R14 finite spinner\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,500,4,1,1,25,1,0\n3000,-100,4,2,2,50,0,0\n[HitObjects]\n100,200,1000,8,17,3000,3:2:7:35:private.wav\n");
 var map=Beatmap.Load(path);var metadata=map.CircleSounds!.Spinners.Single();Eq(metadata.Payload.RawSound,17);Eq(metadata.Payload.StoredMask,17);Eq(metadata.SourceOrder,0);Eq(metadata.Payload.Object.StartTime,3000);Eq(metadata.Payload.Object.Position,new Vector2(256,192));Eq(metadata.Payload.BaseSet,3);Eq(metadata.Payload.AdditionSet,2);Eq(metadata.Payload.Index,7);Eq(metadata.Payload.Volume,35);Eq(metadata.Payload.Filename,"private.wav");Yes(ReferenceEquals(metadata.Object,map.Spinners.Single()));Yes(ReferenceEquals(map.CircleSounds.RegisteredOverrides[0].Spinner,metadata.Object));
 Pass("spinner metadata exact identity/source order/end/center/raw mask17 and complete payload without circle normalization");
 Eq(HitSoundLibrary.SpinnerAdmission(map),null);Yes(HitSoundLibrary.SpinnerAdmission(map with{Title="clone"}) is not null);
 Pass("fresh spinner admission versus unsupported cloned map");
 var a=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.EngineeringPractice);var b=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.EngineeringPractice);int hooks=0;
 a.SpinnerSound+=(s,e)=>{hooks++;throw new IOException("optional audio backend");};
 for(int time=1000;time<=3020;time+=20)
 {float theta=(time-1000)*.02f;Vector2 pointer=new(256+100*MathF.Cos(theta),192+100*MathF.Sin(theta));a.Advance(time,pointer,1);b.Advance(time,pointer,1);Eq(a.Ledger.Score,b.Ledger.Score);Eq(a.Combo,b.Combo);Eq(a.Accuracy,b.Accuracy);Eq(a.Spinners[0].Observation.Count,b.Spinners[0].Observation.Count);Eq(a.Spinners[0].Progress,b.Spinners[0].Progress);Eq(a.Spinners[0].Result,b.Spinners[0].Result);Eq(a.Health?.State.H,b.Health?.State.H);}
 Yes(hooks>0);Pass("executed sessions preserve spinner score/accuracy/combo/P/C/grade/HP under throwing audio callbacks");
 int silent=0;var calibration=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.EngineeringPractice);calibration.SpinnerSound+=(_,_)=>silent++;Eq(silent,0);
 Pass("private health preparation does not dispatch spinner audio");
 int cleanups=0;calibration.SpinnerSound+=(_,e)=>{if(e==SpinnerSoundStage.Cleanup)cleanups++;};calibration.CleanupSpinnerAudio();Eq(cleanups,1);
 Pass("explicit full-list/session cleanup bridge reaches spinner cleanup independently of completion");
 using var library=new HitSoundLibrary(new(){DefaultPackDirectory=args[1]});using var assets=await library.PrepareAsync(map,CancellationToken.None);Yes(assets.Spinners.ContainsKey(metadata.Object));Yes(assets.SpinHandle>0&&assets.BonusHandle>0);Yes(assets.Samples[assets.SpinHandle].Loop&&!assets.Samples[assets.BonusHandle].Loop);
 Pass("actual local provider/library prepares independent spinner resources and metadata association");
 foreach(string stem in new[]{"spinnerspin","spinnerbonus"})
 {
   File.WriteAllText(path,File.ReadAllText(path).Replace("private.wav",stem+".wav").Replace("spinnerspin.wav",stem+".wav"));
   var collision=Beatmap.Load(path);Yes(HitSoundLibrary.SpinnerAdmission(collision)?.Contains("collision")==true);
   using var limited=await library.PrepareAsync(collision,CancellationToken.None);Yes(limited.SpinnerLimitation is not null);Eq(limited.Spinners.Count,0);
 }
 Pass("fresh named/custom collisions explicitly disable optional spinner audio without rejecting playable map");
}finally{Directory.Delete(scratch,true);}
backend.Operations.Clear();globalBackend.Operations.Clear();spin.Channel=999;spin.Modulate(.5f);Yes(backend.Operations.All(o=>o.Channel==999));Eq(globalBackend.Operations.Count,0);
Pass("frequency never targets the separate slider channel pair");
int playedAfterReset=0;var pending=new Queue<Action>();var life=new CircleSoundPlayback(_=>playedAfterReset++,a=>pending.Enqueue(a),()=>{});life.Resume();SpinnerSampleSelection.Bonus(def,33,25,policy,life);life.Restart();while(pending.TryDequeue(out var action))action();Eq(playedAfterReset,0);life.Dispose();
Pass("bonus dispatcher restart/disposal generation cancels stale spinner player requests");
var failBackend=new RecordingSampleChannels{GetChannel=(_,_)=>throw new IOException("acquire")};var partial=new SharedSpinnerChannel(failBackend,()=>777,()=>.5f,()=>.5f,a=>queue.Enqueue(a)){Channel=101};failBackend.States[101]=1;threw=false;try{partial.Refresh();}catch(IOException){threw=true;}Yes(threw);Eq(partial.Channel,101);Yes(failBackend.Operations.Any(o=>o.Name=="Stop"));
Pass("A66 old Stop survives failed replacement; no fabricated transactional rollback");
string root=args[0];string windows=File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Platform/WindowsCircleEffects.cs"));Yes(windows.Contains("session.SpinnerSound += OnSpinner")&&windows.Contains("old.SpinnerSound -= OnSpinner")&&windows.Contains("epoch == loopGeneration")&&windows.Contains("spinnerChannel?.QueuePause()"));
Pass("SOURCE INSPECTION spinner attach/detach, pause and shared generation guard");
string native=File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Platform/WindowsSliderLoops.cs"));Yes(native.Contains("FrequencyCapability.Unsupported")&&native.Contains("Length(int channel, int mode) => null")&&!native.Contains("attribute == 1) c.Player.SpeedRatio"));
Pass("SOURCE INSPECTION Windows frequency and native length unavailable, no invented speed mapping");
foreach(var (name,count) in new[]{("spinnerspin",22050),("spinnerbonus",13230)}){byte[] bytes=File.ReadAllBytes(Path.Combine(args[1],name+".wav"));Eq(bytes.Length,44+2*count);Eq(Encoding.ASCII.GetString(bytes,0,4),"RIFF");Eq(BitConverter.ToInt32(bytes,40),2*count);Eq(BitConverter.ToInt32(bytes,24),44100);Yes(bytes.Skip(44).Any(b=>b!=0));}
Pass("independent two-file spinner PCM structures (not decoding, modulation or audible proof)");
Console.WriteLine($"R14 SPINNER AUDIO CHECKS: {groups} passed; source inspections explicitly labelled; no media/window/original execution");
sealed class Sink:ISamplePlaybackSink{public List<SamplePlaybackRequest> Requests{get;}=[];public void Enqueue(SamplePlaybackRequest r)=>Requests.Add(r);}
sealed class Files:ISampleFiles{public List<string> Reads{get;}=[];public bool Exists(string n)=>true;public byte[]? Read(string n){Reads.Add(n);return new byte[1024];}}
sealed class Missing:ISampleFiles{public bool Exists(string n)=>false;public byte[]? Read(string n)=>null;}
sealed class Defaults:IDefaultSamples{public byte[]? Ui(string k)=>null;public byte[]? Gameplay(string k)=>Encoding.ASCII.GetBytes(k);}
sealed class MissingDefaults:IDefaultSamples{public byte[]? Ui(string k)=>null;public byte[]? Gameplay(string k)=>null;}
sealed class Loader:ISampleLoader{public List<SampleBytes> Samples{get;}=[];public int? Result{get;init;}public int Load(SampleBytes b){Samples.Add(b);return Result??Samples.Count;}public void Free(int h){}}
