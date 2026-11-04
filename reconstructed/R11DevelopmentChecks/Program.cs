// Bounded E037 H01–H37 /E038 K01–K42 development controls, not independent QA09.
// Explicit production component calls only; no Window, player, clock or OS input.
using System.Globalization;
using System.Numerics;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;

if (args.Length != 1) throw new ArgumentException("Pass repository root.");
string scratch = Path.Combine(Path.GetTempPath(), "ohno-r11-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(scratch);
int groups = 0, files = 0;
try
{
    var circle = new CircleDefinition(new(256, 192), 2000);
    foreach (int raw in new[] { 0, 1, 2, 4, 8, 14, 15, -1, 1024 })
    { var data = CircleSampleMetadata.Parse(circle, raw, null); Eq(data.RawSound, raw); Eq(data.StoredMask, raw & 14); Eq(data.Index, 0); Check(data.Error is null); }
    Pass("H11 constructor keeps2/4/8, raw0/1 stored0; raw sound retained");
    var absent = CircleSampleMetadata.Parse(circle, 0, null); var empty = CircleSampleMetadata.Parse(circle, 0, "");
    var zero = CircleSampleMetadata.Parse(circle, 0, "0:0:0:0:");
    Check(absent.RawPayload is null && empty.RawPayload == "" && zero.RawPayload == "0:0:0:0:");
    Eq(zero.Volume, 0); Eq(zero.Index, 0); Check(CircleSampleMetadata.Parse(circle, 0, "0").Error is not null);
    Check(CircleSampleMetadata.Parse(circle, 0, "bad:0").Error is not null);
    Eq(CircleSampleMetadata.Parse(circle, 0, "1:3:7:101:kick.wav").Volume, 100);
    Eq(CircleSampleMetadata.Parse(circle, 0, "1:3:-3:-2:").Volume, 0);
    Pass("payload absence/empty/zero/malformed kept distinct, finite overrides and volume clamps");
    var map = Map("128,192,2000,1,14,1:3:7:25:kick.wav");
    Check(ReferenceEquals(map.CircleSounds!.Circles[0].Object, map.Circles[0]));
    Eq(map.CircleSounds.Circles[0].Filename, "kick.wav"); Eq(map.CircleSounds.Circles[0].Index, 7);
    Eq(map.CircleSounds.Timing[0].Set, 2); Eq(map.CircleSounds.Timing[0].Index, 1);
    Eq(map.CircleSounds.Timing[0].Volume, 40); Eq(map.CircleSounds.Timing[0].Uninherited, true);
    var bad = Map("128,192,2000,1,0,0"); Check(bad.CircleSounds!.Circles[0].Error is not null);
    var shortTiming = Map("128,192,2000,1,0", "0,500"); Check(shortTiming.CircleSounds!.TimingError is not null);
    Pass("actual additive parser metadata and long-row Soft2 default; malformed/short unsupported status");
    var timing = new SampleTiming[] { new(2004, 1, 1, 20, true, 0), new(2005, 2, 2, 40, false, 1), new(2006, 3, 3, 60, true, 2) };
    Eq(CircleSampleSelection.Timing(timing, 2000), timing[1]);
    Eq(CircleSampleSelection.Timing(timing, 1000), timing[0]); Check(CircleSampleSelection.Timing([], 2000) is null);
    var unsorted = new SampleTiming[] { new(100, 1, 0, 100, true, 0), new(90, 2, 0, 40, true, 1) };
    Eq(CircleSampleSelection.Timing(unsorted, 100), unsorted[1]);
    Eq(CircleSampleSelection.Timing([new(double.NaN, 1, 0, 100, true, 0), timing[1]], 2000), timing[1]);
    Pass("H06–H10 supplied-order scan, start+5 inclusive, no nearest-time repair and first fallback");
    var policy = Policy(); var inherited = CircleSampleSelection.Select(zero, timing[1], policy);
    Eq(inherited, new SelectedCircleSample(1, 2, 0, 2, 40));
    var overrides = CircleSampleMetadata.Parse(circle, 14, "1:3:2:25:");
    Eq(CircleSampleSelection.Select(overrides, timing[1], policy), new SelectedCircleSample(14, 1, 3, 2, 25));
    Eq(CircleSampleSelection.Select(overrides with { Index = -3 }, timing[1], policy).Index, -3);
    Pass("H17/H18/H23 base/index/volume inheritance and addition operands");
    var (r, mf, sf, df, loader) = Resources(); df.Data["normal-hitnormal"] = Bytes(); df.Data["normal-hitfinish"] = Bytes(); df.Data["normal-hitwhistle"] = Bytes(); df.Data["normal-hitclap"] = Bytes(); r.Prepare([], []);
    var sink = new Recording();
    CircleSampleSelection.Request(overrides with { AdditionSet = 1, Index = 0, Volume = 100 }, [new(0,1,0,100,true,0)], -1, r, policy, sink);
    Eq(string.Join(',', sink.Requests.Select(p => p.Layer)), "4,2,8"); Eq(string.Join(',', sink.Requests.Select(p => p.Volume)), "100,85,85");
    sink.Requests.Clear(); CircleSampleSelection.Request(overrides with { AdditionSet = 1, Index = 0, Volume = 100 }, [new(0,1,0,100,true,0)], -1, r, policy with { LayeredHitSounds = true }, sink);
    Eq(string.Join(',', sink.Requests.Select(p => p.Layer)), "1,4,2,8"); Eq(string.Join(',', sink.Requests.Select(p => p.Volume)), "80,100,85,85");
    Pass("H12–H16 K05/K06 all layer identities and exact Normal/Finish/Whistle/Clap request order");
    foreach (var p in new[] { policy with { LayeredHitSounds = true, HasSkin = false }, policy with { LayeredHitSounds = true, HasSelectedMap = false }, policy with { LayeredHitSounds = true, SelectedMode = 3 } })
        Eq(CircleSampleSelection.Select(overrides, timing[1], p).Mask, 14);
    Eq(CircleSampleSelection.Select(zero, timing[1], policy with { HasSkin = false, HasSelectedMap = false }).Mask, 1);
    Pass("conditional layered predicate requires all applicable state; zero mask always augments");
    sink.Requests.Clear(); CircleSampleSelection.Request(overrides with { BaseSet = 1, AdditionSet = 1, Index = 0, Volume = 0 }, [new(0,1,0,0,true,0)], -1, r, policy with { LayeredHitSounds = true }, sink);
    Eq(string.Join(',', sink.Requests.Select(p => p.Volume)), "6,8,6,6");
    sink.Requests.Clear(); CircleSampleSelection.Request(overrides with { Volume = 0 }, [new(0,1,0,0,true,0)], 99, r, policy, sink);
    Eq(sink.Requests.Count, 1); Eq(sink.Requests[0].Volume, 0); Eq(sink.Requests[0].Layer, 0);
    Pass("H19/H20 standard minimum8 before attenuation; custom bypasses floor and all layers");
    sink.Requests.Clear(); CircleSampleSelection.Request(zero, [new(0,1,0,100,true,0)], 0, r, policy, sink);
    Eq(sink.Requests.Count, 1); Eq(sink.Requests[0].Handle, 0);
    sink.Requests.Clear(); CircleSampleSelection.Request(zero, [], 99, r, policy, sink); Eq(sink.Requests.Count, 0);
    bool observed = false; Reject(() => CircleSampleSelection.Request(zero, timing, 99, r, policy, sink, _ => { observed = true; throw new InvalidDataException("observer"); }));
    Check(observed); Eq(sink.Requests.Count, 0);
    Pass("H06/H26/H35 custom0 still submits, null record exits and observer precedes dispatch");
    foreach (float x in new[] { 0f, 64f, 256f, 448f, 512f })
    {
        sink.Requests.Clear(); var data = zero with { Object = new(new(x,192),2000) };
        CircleSampleSelection.Request(data, [new(0,1,0,100,true,0)], 10, r, policy with { VolumeEffect = 80, VolumeUniversal = 50, SongRate = 150, RateMatching = true }, sink);
        Near(sink.Requests[0].Pan, (x / 512f - .5f) * .8f); Eq(sink.Requests[0].Factor, 1.5f); Eq(sink.Requests[0].Gain, .4f);
    }
    sink.Requests.Clear(); CircleSampleSelection.Request(zero, timing, 10, r, policy with { SongRate = 150, RateMatching = false }, sink); Eq(sink.Requests[0].Factor, 1f);
    Pass("H30/H31 horizontal Single pan, rate branch, exact integer global volume product and request gain");
    foreach (int delta in new[] { 0, 50, 100, 150 })
    {
        var s = new CircleSession(new Beatmap("one",5,5,5,[circle])); int calls = 0;
        s.PositiveCircleSound += source => { calls++; Check(ReferenceEquals(source,circle)); Eq(s.Ledger.EventCount,0); Eq(s.Circles[0].JudgedAt,0); Check(s.Circles[0].Result != Judgement.Pending); };
        s.Press(circle.Position,2000+delta); s.Press(circle.Position,2000+delta); s.Advance(3000);
        Eq(calls,delta==150?0:1); Eq(s.JudgedCount,1);
    }
    Pass("H01–H04 ordinary300/100/50 hook once before completion/accounting; miss and later updates silent");
    var broken = new CircleSession(new Beatmap("sink failure",5,5,5,[circle])); broken.PositiveCircleSound += _ => throw new IOException("sink unavailable"); broken.Press(circle.Position,2000);
    Eq(broken.Ledger.Score,300); Eq(broken.JudgedCount,1); Check(broken.CircleSoundError!.Contains("sink unavailable"));
    Pass("ENGINEERING optional hook failure containment preserves result/accounting");
    foreach (int set in new[] { 1,2,3 }) foreach (int bit in new[] { 1,2,4,8 }) foreach (int index in new[] { 0,1,2 })
        Eq(r.Key(bit,set,index), (set==1?"normal":set==2?"soft":"drum")+"-"+CircleSampleResources.LayerName(bit)+(index==2?"2":""));
    Pass("K07–K12 36 standard naming controls, index1 unsuffixed and index2 suffix2");
    var a = Resources(); a.Map.Data["normal-hitnormal.wav"] = Bytes(); Eq(a.R.Standard(1,1,0),-1); Eq(a.Map.Reads.Count,0);
    a.Map.Data["normal-hitnormal.mp3"] = Bytes(); a.Map.Data["normal-hitnormal.ogg"] = Bytes(); Check(a.R.Standard(1,1,1)>0); Eq(string.Join(',',a.Map.Reads),"normal-hitnormal.wav");
    Pass("K10/K11/K27 index0 skips map; standard first nonnull wav stops probing");
    a = Resources(); a.Map.Data["soft-hitwhistle.mp3"] = Bytes(); a.Map.Data["soft-hitwhistle.ogg"] = Bytes(); Check(a.R.Standard(2,2,1)>0);
    Eq(string.Join(',',a.Map.Reads),"soft-hitwhistle.wav,soft-hitwhistle.mp3");
    a = Resources(); a.Map.Data["kick.ogg"] = Bytes(); a.Map.Data["kick.mp3"] = Bytes(); Check(a.R.Custom("kick")>0);
    Eq(string.Join(',',a.Map.Reads),"kick.wav,kick.ogg");
    Pass("K02/K18/K20/K21 standard wav/mp3/ogg versus custom wav/ogg/mp3");
    a = Resources(); a.Map.Existing.Add("kick.ogg"); a.Map.Data["kick.wav"] = Bytes(); Eq(a.R.Custom("kick.ogg"),-1); Eq(string.Join(',',a.Map.Reads),"kick.ogg");
    a = Resources(); a.Map.Data[".kick.wav"] = Bytes(); Check(a.R.Custom(".kick")>0); Eq(a.Map.Reads[0],".kick.wav");
    Pass("K22/K23 existing but unreadable exact custom does not probe; first-dot0 uses suffix arm");
    a = Resources(); a.Map.Data["kick.wav"] = Bytes(); int firstHandle = a.R.Custom("kick.wav"); Check(firstHandle>0);
    int reads = a.Map.Reads.Count; Eq(a.R.Custom("kick.ogg"),firstHandle); Eq(a.Map.Reads.Count,reads);
    int replaced = a.R.Custom("kick.wav",force:true); Check(replaced!=firstHandle); Check(a.Loader.Freed.Contains(firstHandle));
    a.R.EvictMap("kick"); Check(a.Loader.Freed.Contains(replaced));
    Pass("K16/K24/K26 custom cache shares stem, force and explicit eviction release prior IDs");
    a = Resources(); Eq(a.R.Custom("missing"),-1); reads=a.Map.Reads.Count; a.Map.Data["missing.wav"]=Bytes(); Eq(a.R.Custom("missing"),-1); Eq(a.Map.Reads.Count,reads);
    Eq(a.R.Custom("missing",acceptingSetter:false),-1); Eq(a.Map.Reads.Count,reads); a.R.ClearMap(); Check(a.R.Custom("missing")>0);
    Pass("K17/K25/K36 negative caches retain misses until explicit map invalidation");
    a = Resources(); a.Map.Data["normal-hitnormal.wav"]=Bytes(); a.Map.Data["normal-hitnormal.mp3"]=Bytes(); a.Loader.Return = s => s.Origin==SampleOrigin.Map ? -1 : 77;
    a.Default.Data["normal-hitnormal"]=Bytes(); Eq(a.R.Standard(1,1,1),77); Eq(string.Join(',',a.Map.Reads),"normal-hitnormal.wav");
    a = Resources(); a.Map.Data["normal-hitnormal.wav"]=Bytes(); a.Loader.Return=_=>0; Eq(a.R.Standard(1,1,1),0); Eq(a.Default.Reads.Count,0);
    a = Resources(); a.Default.Data["normal-hitnormal"]=Bytes(); a.Loader.Return=_=>0; Eq(a.R.Standard(1,1,0),-1);
    Pass("K33–K35 nonnull bytes terminate even load-1; map0 retained versus embedded0 normalized");
    a = Resources(skin:true); a.Skin.Data["normal-hitnormal.mp3"]=Bytes(); a.Default.Data["normal-hitnormal"]=Bytes(); Check(a.R.Standard(1,1,1)>0);
    Eq(a.Loader.Loaded.Single().Origin,SampleOrigin.Skin); Eq(string.Join(',',a.Skin.Probes),"normal-hitnormal.wav,normal-hitnormal.mp3"); Eq(a.Default.Reads.Count,0);
    var missingSkin = Resources(skin:true); missingSkin.Default.Data["normal-hitnormal"]=Bytes(); Check(missingSkin.R.Standard(1,1,1)>0); Eq(missingSkin.Loader.Loaded.Single().Origin,SampleOrigin.Default);
    Pass("K29/K31 provider map→admitted skin→UI/default gameplay ordering");
    a = Resources(skin:true, skinName:"Default"); a.Skin.Data["normal-hitnormal.wav"]=Bytes(); Eq(a.R.Standard(1,1,0),-1); Eq(a.Skin.Probes.Count,0);
    a = Resources(skin:true, skinSamples:false); Eq(a.R.Standard(1,1,0),-1); Eq(a.Skin.Probes.Count,0);
    var nullSkin = new CircleSampleResources(new Files(),null,new Defaults(),new Loader(),new(14,false,false,true,false,"other",true,CultureInfo.InvariantCulture)); Reject(()=>nullSkin.Standard(1,1,0));
    a = Resources(early:true); Eq(a.R.Standard(1,1,1),-1); Eq(a.Map.Reads.Count,0); Eq(a.Default.Reads.Count,0);
    Pass("K30/K37/K41 explicit skin/process gates and unsupported null skin state");
    a = Resources(); a.Map.Data["normal-hitnormal.wav"]=Bytes(); a.Default.UiData["normal-hitnormal"]=Bytes(); a.Default.Data["normal-hitnormal"]=Bytes();
    Check(a.R.Standard(1,1,0)>0); Eq(string.Join(',',a.Default.Reads),"ui:normal-hitnormal");
    Pass("K31/K32 UI Byte[] precedes gameplay pack, absence stays missing");
    a = Resources(format:4); a.Map.Data["hitnormal.wav"]=Bytes(); Check(a.R.Standard(1,1,1)>0); Eq(string.Join(',',a.Map.Reads),"normal-hitnormal.wav,hitnormal.wav");
    a = Resources(format:5); a.Map.Data["hitnormal.wav"]=Bytes(); Eq(a.R.Standard(1,1,1),-1); Check(!a.Map.Reads.Contains("hitnormal.wav"));
    Pass("K28 original pre-v5 unprefixed fallback projection, without expanding actual parser formats");
    a = Resources(); a.Map.Data["normal-hitnormal7.wav"]=Bytes(); a.Default.Data["normal-hitnormal"]=Bytes(); a.R.Prepare([7],[7]);
    int dynamicHandle=a.R.Resolve(1,1,7); Check(dynamicHandle>0); Eq(dynamicHandle,a.R.Caches.Map["normal-hitnormal7"]);
    reads=a.Map.Reads.Count; Check(a.R.Resolve(1,1,99)>0); Eq(a.Map.Reads.Count,reads);
    a.R.Caches.Map["normal-hitnormal99"]=0; Eq(a.R.Resolve(1,1,99),0); a.R.Caches.Map["normal-hitnormal99"]=-1; Check(a.R.Resolve(1,1,99)>0);
    Eq(a.R.Resolve(1,1,-3),a.R.Resolve(1,1,0));
    Pass("H21–H23 K13–K15 dynamic preload/dictionary-only resolve,0 and plane0 fallback");
    a = Resources(); a.Map.Data["normal-hitnormal.wav"]=Bytes(); int aliased=a.R.Custom("normal-hitnormal.wav"); a.R.Prepare([],[]); Eq(a.R.Resolve(1,1,1),aliased);
    Eq(a.Loader.Loaded.Count(x=>x.Origin==SampleOrigin.Map),1);
    Pass("K40 custom stem and standard map share the same case-sensitive cache identity");
    a = Resources(skin:true); a.Map.Data["normal-hitnormal.wav"]=Bytes(); a.Skin.Data["normal-hitnormal.wav"]=Bytes(); a.Default.Data["normal-hitnormal"]=Bytes();
    int mh=a.R.Standard(1,1,1), sh=a.R.Standard(1,1,0); a.R.ClearSkin(); int dh=a.R.Standard(1,1,0,1);
    Check(mh!=sh && sh!=dh); Check(a.R.Caches.Map.Count>0 && a.R.Caches.Embedded.Count>0);
    a.R.ClearMap(); Check(a.R.Caches.Embedded.Count>0); a.R.ClearAll(); Eq(a.R.Caches.Embedded.Count,0);
    Pass("K42 distinct map/skin/default caches and independent map/skin/all invalidation");
    var actions=new Queue<Action>(); var played=new List<SamplePlaybackRequest>(); int stops=0;
    var lifetime=new CircleSoundPlayback(played.Add,actions.Enqueue,()=>stops++); var req=new SamplePlaybackRequest(circle,10,1,80,0,1,.8f);
    lifetime.Resume(); lifetime.Enqueue(req); lifetime.Pause(); actions.Dequeue()(); Eq(played.Count,0);
    lifetime.Resume(); lifetime.Enqueue(req); actions.Dequeue()(); Eq(played.Count,1);
    lifetime.Enqueue(req); lifetime.Restart(); actions.Dequeue()(); Eq(played.Count,1);
    lifetime.Enqueue(req with { Handle=0 }); actions.Dequeue()(); Eq(played.Count,1);
    lifetime.Enqueue(req); lifetime.Dispose(); actions.Dequeue()(); Eq(played.Count,1); Check(stops>=3);
    Pass("H26/H29 pause/restart/replacement generations discard old queued work and sample0; stop/dispose isolated");
    var failureSink=new CircleSoundPlayback(_=>throw new IOException("decoder"),a=>a(),()=>{}); failureSink.Resume(); failureSink.Enqueue(req); Eq(failureSink.Error,"decoder");
    var failureQueue=new CircleSoundPlayback(_=>{},_=>throw new IOException("queue"),()=>{}); failureQueue.Resume(); failureQueue.Enqueue(req); Eq(failureQueue.Error,"queue");
    Pass("H36 optional decoder/sink/dispatch failures contained without pretending audio success");
    var local=new LocalSampleFiles(scratch); File.WriteAllBytes(Path.Combine(scratch,"safe.wav"),Bytes()); Check(local.Exists("safe.wav")); Check(local.Read("safe.wav")!.Length==2048);
    foreach(string path in new[]{"../escape.wav","sub/../../escape.wav","C:\\outside.wav","/outside.wav","https://example.invalid/a.wav","sub\\..\\escape.wav"}) Reject(()=>local.Resolve(path));
    Pass("K39 ENGINEERING admitted local roots reject absolute/network/parent traversal");
    var actualLibrary=new HitSoundLibrary(new HitSoundSettings { SkinSamples=false });
    var freshMap=Map("128,192,2000,1,0\n384,192,4000,1,0");
    using(var prepared=await actualLibrary.PrepareAsync(freshMap,CancellationToken.None))
    { Eq(prepared.Circles.Count,2); Eq(prepared.Samples.Count,0); Eq(prepared.Resources.Resolve(1,2,0),-1); Check(prepared.Error is null); }
    using(var replacedMap=await actualLibrary.PrepareAsync(Map("256,192,3000,1,0"),CancellationToken.None)) Eq(replacedMap.Circles.Count,1);
    actualLibrary.Dispose();
    Pass("real loose-file preparation remains playable without assets; map replacement keeps identities isolated");
    var shortCatalog=new EffectSampleCatalog(); Eq(shortCatalog.Load(new("short.wav",new byte[20],SampleOrigin.Map,SampleLoadMode.MapShortCreate)),0); Check(shortCatalog.Errors.Count>0);
    Pass("native short-byte sample creation explicitly unsupported, no fabricated audible fallback");
    var mixed=Beatmap.Load(Path.Combine(args[0],"reconstructed/CircleGame/Fixtures/health-r09.osu"));
    var on=CircleSession.CreateFreshOrdinary(mixed,ScoreConfiguration.FromBeatmap(mixed)); var off=CircleSession.CreateFreshOrdinary(mixed,ScoreConfiguration.FromBeatmap(mixed));
    int soundCalls=0; var actualRequests = new Recording();
    Eq(actualRequests.Requests.Count,0); // Calibration cannot emit a sound request.
    on.PositiveCircleSound+=source=>
    {
        soundCalls++;
        CircleSampleSelection.Request(mixed.CircleSounds!.Circles.Single(m=>ReferenceEquals(m.Object,source)),
            mixed.CircleSounds.Timing,10,r,policy,actualRequests);
    };
    foreach(var c in mixed.Circles) { on.Press(c.Position,c.StartTime); off.Press(c.Position,c.StartTime); on.Advance(c.StartTime+200); off.Advance(c.StartTime+200); }
    Eq(on.Ledger.Score,off.Ledger.Score); Eq(on.Combo,off.Combo); Eq(on.Accuracy,off.Accuracy); Check(on.ObjectResults.SequenceEqual(off.ObjectResults));
    Eq(on.Health!.State.H,off.Health!.State.H); Eq(on.Health.State.A,off.Health.State.A); Eq(on.Health.ResultEvents,off.Health.ResultEvents); Check(soundCalls>0); Eq(actualRequests.Requests.Count,soundCalls);
    Eq(CircleSession.CreateFreshOrdinary(mixed,ScoreConfiguration.FromBeatmap(mixed)).Ledger.EventCount,0);
    Pass("actual hook preserves score/accuracy/combo/judgements/HP; fresh session resets state and calibration stays silent");
    Console.WriteLine($"R11 HITSOUND CHECKS: {groups} passed (E037/E038 finite production-component controls; no native playback, listening or game window)");
}
finally { Directory.Delete(scratch,true); }

Beatmap Map(string rows,string timing="0,1000,4,0,1,40,1,0")
{
    string path=Path.Combine(scratch,$"map{files++}.osu");
    File.WriteAllText(path,$"osu file format v14\n[General]\nMode:0\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n{timing}\n[HitObjects]\n{rows}\n"); return Beatmap.Load(path);
}
static CircleSoundPolicy Policy()=>new(true,false,true,0,100,false,100,100);
static byte[] Bytes()=>new byte[2048];
static (CircleSampleResources R, Files Map, Files Skin, Defaults Default, Loader Loader) Resources(bool skin=false,string skinName="custom",bool skinSamples=true,int format=14,bool early=false)
{
    var map=new Files();var skinFiles=new Files();var pack=new Defaults();var loader=new Loader();
    return(new(map,skinFiles,pack,loader,new(format,false,early,true,skin,skinName,skinSamples&&skin,CultureInfo.InvariantCulture)),map,skinFiles,pack,loader);
}
void Pass(string name){groups++;Console.WriteLine("PASS R11 "+name);}
static void Check(bool condition){if(!condition)throw new Exception("Bounded hitsound assertion failed.");}
static void Eq<T>(T actual,T expected){if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception($"Expected {expected}; got {actual}");}
static void Near(float actual,float expected){if(Math.Abs(actual-expected)>.00001f)throw new Exception($"Expected {expected}; got {actual}");}
static void Reject(Action action){try{action();}catch(InvalidDataException){return;}throw new Exception("Expected explicit unsupported/error result.");}
sealed class Files : ISampleFiles
{
    public Dictionary<string,byte[]> Data {get;}=new(StringComparer.Ordinal);
    public HashSet<string> Existing {get;}=[];
    public List<string> Reads {get;}=[];public List<string> Probes {get;}=[];
    public bool Exists(string name){Probes.Add(name);return Existing.Contains(name)||Data.ContainsKey(name);}
    public byte[]? Read(string name){Reads.Add(name);return Data.GetValueOrDefault(name);}
}
sealed class Defaults : IDefaultSamples
{
    public Dictionary<string,byte[]> Data {get;}=[];public Dictionary<string,byte[]> UiData {get;}=[];public List<string> Reads {get;}=[];
    public byte[]? Ui(string key){Reads.Add("ui:"+key);return UiData.GetValueOrDefault(key);}
    public byte[]? Gameplay(string key){Reads.Add("game:"+key);return Data.GetValueOrDefault(key);}
}
sealed class Loader : ISampleLoader
{
    public List<SampleBytes> Loaded {get;}=[];public List<int> Freed {get;}=[];public Func<SampleBytes,int>? Return {get;set;}
    public int Load(SampleBytes sample){Loaded.Add(sample);return Return?.Invoke(sample)??Loaded.Count;}
    public void Free(int id)=>Freed.Add(id);
}
sealed class Recording : ISamplePlaybackSink
{public List<SamplePlaybackRequest> Requests {get;}=[];public void Enqueue(SamplePlaybackRequest request)=>Requests.Add(request);}
