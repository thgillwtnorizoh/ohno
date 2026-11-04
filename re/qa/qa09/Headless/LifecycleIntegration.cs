using System.Numerics;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
namespace Ohno.QA09;
public static partial class Tests
{
    static void LifecycleCases()
    {
        SamplePlaybackRequest Req(int id=50)=>new(new(new(128,192),2000),id,1,80,-.2f,1,.64f);
        Add("L01 fresh paused queue then active consecutive requests","R11 ENGINEERING CircleSoundPlayback boundary",()=>{
            var q=new List<Action>();var played=new List<SamplePlaybackRequest>();int stops=0;var p=new CircleSoundPlayback(played.Add,q.Add,()=>stops++);Obs(new{played});
            p.Enqueue(Req());Eq(q.Count,0);p.Resume();p.Enqueue(Req(51));p.Enqueue(Req(52));Eq(played.Count,0);Eq(q.Count,2);q.ForEach(a=>a());Seq(played.Select(x=>x.Handle),51,52);Eq(stops,0);
        });
        Add("L02 four simultaneous logical layers retain distinct requests","E037 layer requests plus ENGINEERING queue",()=>{
            var q=new List<Action>();var layers=new List<int>();var p=new CircleSoundPlayback(r=>layers.Add(r.Layer),q.Add,()=>{});p.Resume();foreach(int bit in new[]{1,4,2,8})p.Enqueue(Req() with{Layer=bit});q.ForEach(a=>a());Seq(layers,1,4,2,8);
        });
        Add("L03 pause invalidates queued and stops; resume does not replay","ENGINEERING policy, not original queued pause rule",()=>{
            var q=new List<Action>();int count=0,stops=0;var p=new CircleSoundPlayback(_=>count++,q.Add,()=>stops++);p.Resume();p.Enqueue(Req());p.Pause();p.Enqueue(Req());p.Resume();q.ForEach(a=>a());Eq(count,0);Eq(stops,1);p.Enqueue(Req());q[^1]();Eq(count,1);
        });
        Add("L04 restart generation rejects old queued work","ENGINEERING generation isolation",()=>{
            var q=new List<Action>();var played=new List<int>();var p=new CircleSoundPlayback(r=>played.Add(r.Handle),q.Add,()=>{});p.Resume();p.Enqueue(Req(51));p.Restart();p.Enqueue(Req(52));q.ForEach(a=>a());Seq(played,52);
        });
        Add("L05 disposal and repeated disposal reject pending/future work","ENGINEERING lifetime",()=>{
            var q=new List<Action>();int played=0,stops=0;var p=new CircleSoundPlayback(_=>played++,q.Add,()=>stops++);p.Resume();p.Enqueue(Req());p.Dispose();p.Dispose();p.Resume();p.Restart();p.Enqueue(Req());q.ForEach(a=>a());Eq(played,0);Eq(stops,1);Eq(p.Disposed,true);Eq(p.Paused,true);
        });
        foreach(int id in new[]{-1,0})Add($"L06 invalid queued handle {id}","E037 sample invalidity, engineering opaque ID boundary",()=>{
            var q=new List<Action>();int count=0;var p=new CircleSoundPlayback(_=>count++,q.Add,()=>{});p.Resume();p.Enqueue(Req(id));Eq(q.Count,1);q[0]();Eq(count,0);
        });
        Add("L07 disabled sink drops enqueue and queued dispatch","ENGINEERING Enabled gate",()=>{
            var q=new List<Action>();int n=0;var p=new CircleSoundPlayback(_=>n++,q.Add,()=>{});p.Resume();p.Enabled=false;p.Enqueue(Req());Eq(q.Count,0);p.Enabled=true;p.Enqueue(Req());p.Enabled=false;q[0]();Eq(n,0);
        });
        Add("L08 dispatch submission failure contained","ENGINEERING not original helper exception semantics",()=>{var p=new CircleSoundPlayback(_=>{},_=>throw new IOException("submit sentinel"),()=>{});p.Resume();p.Enqueue(Req());Eq(p.Error,"submit sentinel");});
        Add("L09 backend request failure contained","ENGINEERING sink failure; no actual decoder",()=>{var p=new CircleSoundPlayback(_=>throw new IOException("backend sentinel"),a=>a(),()=>{});p.Resume();p.Enqueue(Req());Eq(p.Error,"backend sentinel");});
        Add("L10 stop failure contained across pause/disposal","ENGINEERING failure boundary",()=>{var p=new CircleSoundPlayback(_=>{},a=>a(),()=>throw new IOException("stop sentinel"));p.Pause();Eq(p.Error,"stop sentinel");p.Dispose();Eq(p.Disposed,true);});
        Add("L11 replacement old callbacks cannot act on new generation","ENGINEERING queue objects, not WPF concurrency",()=>{
            var q=new List<Action>();int old=0,current=0;var a=new CircleSoundPlayback(_=>old++,q.Add,()=>{});a.Resume();a.Enqueue(Req());a.Dispose();var b=new CircleSoundPlayback(_=>current++,q.Add,()=>{});b.Resume();b.Enqueue(Req());q.ForEach(f=>f());Eq(old,0);Eq(current,1);
        });
        foreach(string path in new[]{"../x.wav","a/../x.wav","a/./x.wav","/root.wav","C:\\x.wav","\\\\server\\x.wav","https://x.test/x.wav"})
            Add("P01 local resource path rejected "+path,"ENGINEERING admission, not original unrestricted access",()=>{var f=new LocalSampleFiles(NewDirectory("paths"));Throws<InvalidDataException>(()=>f.Resolve(path));});
        Add("P02 local missing versus readable bytes","R11 actual local provider",()=>{string dir=NewDirectory("files");var f=new LocalSampleFiles(dir);Eq(f.Read("missing.wav"),null);File.WriteAllBytes(Path.Combine(dir,"a.wav"),Bytes(71));Eq(f.Exists("a.wav"),true);Eq(f.Read("a.wav")![0],(byte)71);});
        Add("P03 size guard refuses larger than8MiB","ENGINEERING bound",()=>{string dir=NewDirectory("limit");using(var file=File.Create(Path.Combine(dir,"big.wav")))file.SetLength(8*1024*1024+1);Throws<InvalidDataException>(()=>new LocalSampleFiles(dir).Read("big.wav"));});
        Add("P04 native short sample creation is logical0 with diagnostic","E038 boundary; ENGINEERING unsupported native operation",()=>{var c=new EffectSampleCatalog();Eq(c.Load(new("short.wav",Bytes(count:1023),SampleOrigin.Map,SampleLoadMode.MapShortCreate)),0);Yes(c.Errors.Any(e=>e.Contains("short-sample",StringComparison.Ordinal)),"unsupported status");Eq(c.Snapshot([0]).Count,0);});
        Add("P05 catalog ID mapping and unsupported extension","ENGINEERING opaque IDs not native loader results",()=>{var c=new EffectSampleCatalog();int id=c.Load(new("a.ogg",Bytes(),SampleOrigin.Map,SampleLoadMode.MapBytes));Yes(id>0,"positive owned ID");Eq(c.Snapshot([id])[id].Name,"a.ogg");c.Free(id);c.Free(id);Eq(c.Snapshot([id]).Count,0);Eq(c.Load(new("x.exe",Bytes(),SampleOrigin.Map,SampleLoadMode.MapBytes)),0);});
        Add("P06 catalog64-resource cap","ENGINEERING bounded preparation",()=>{var c=new EffectSampleCatalog();for(int i=0;i<64;i++)c.Load(new("a.wav",Bytes(),SampleOrigin.Map,SampleLoadMode.MapBytes));Throws<InvalidDataException>(()=>c.Load(new("a.wav",Bytes(),SampleOrigin.Map,SampleLoadMode.MapBytes)));});
        Add("P07 missing settings is explicit practice configuration","ENGINEERING default, not original installed settings",()=>{var s=HitSoundSettings.Read(Path.Combine(NewDirectory("config"),"hitsounds.json"));Eq(s.Enabled,true);Eq(s.LayeredHitSounds,true);Eq(s.DefaultPackDirectory,null);Eq(s.VolumeEffect,80);Eq(s.VolumeUniversal,100);});
        foreach(string json in new[]{"{","null","{\"VolumeEffect\":101}","{\"VolumeUniversal\":-1}","{\"SkinName\":\" \"}"})
            Add("P08 malformed settings "+json,"ENGINEERING validation without fallback inside settings API",()=>{
                string path=Path.Combine(NewDirectory("badconfig"),"hitsounds.json");File.WriteAllText(path,json);if(json=="{")Throws<JsonException>(()=>HitSoundSettings.Read(path));else Throws<InvalidDataException>(()=>HitSoundSettings.Read(path));
            });
        Add("P09 explicit disabled settings differ from missing/malformed","ENGINEERING configuration",()=>{string path=Path.Combine(NewDirectory("offconfig"),"hitsounds.json");File.WriteAllText(path,"{\"Enabled\":false,\"LayeredHitSounds\":false,\"SkinSamples\":false,\"VolumeEffect\":31,\"VolumeUniversal\":47}");var s=HitSoundSettings.Read(path);Eq(s.Enabled,false);Eq(s.LayeredHitSounds,false);Eq(s.SkinSamples,false);Eq(s.VolumeEffect,31);Eq(s.VolumeUniversal,47);});
        Add("P10 oversized settings rejected","ENGINEERING16384-byte bound",()=>{string p=Path.Combine(NewDirectory("bigconfig"),"hitsounds.json");File.WriteAllText(p,new string(' ',16385));Throws<InvalidDataException>(()=>HitSoundSettings.Read(p));});
        AddAsync("P11 library no independent pack remains playable silence","R11 optional provider integration",async()=>{
            using var lib=new HitSoundLibrary(new());var m=Map();using var p=await lib.PrepareAsync(m,CancellationToken.None);Eq(p.Samples.Count,0);Yes(p.MissingLayers>=2,"explicit unresolved layers");var s=Fresh(m);s.Press(m.Circles[0].Position,2000);Eq(s.Ledger.N300,(ushort)1);
        });
        AddAsync("P12 library independent pack actual preparation","E038 naming model + ENGINEERING pack mapping key.wav",async()=>{
            using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=packA});var m=Map();using var p=await lib.PrepareAsync(m,CancellationToken.None);Eq(p.Samples.Count,12);Eq(p.Error,null);Eq(p.MissingLayers,0);Yes(ReferenceEquals(p.Circles.Keys.First(),m.Circles[0]),"exact dictionary object identity");Yes(!p.Circles.ContainsKey(m.Circles[0] with{}),"cloned value identity not admitted");
        });
        AddAsync("P13 library native-short custom preserves zero bypass","E037/E038 custom zero; ENGINEERING no native create",async()=>{
            string dir=NewDirectory("shortsample");File.WriteAllBytes(Path.Combine(dir,"tiny.wav"),Bytes(count:1));var m=Map("128,192,2000,1,14,0:0:0:0:tiny.wav\n128,192,4000,1,0",directory:dir);
            using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=packA});using var p=await lib.PrepareAsync(m,CancellationToken.None);Eq(p.Circles[m.Circles[0]].Custom,0);Yes(p.Error?.Contains("short-sample",StringComparison.Ordinal)==true,"unsupported create diagnostic");var sink=new Sink();var x=p.Circles[m.Circles[0]];CircleSampleSelection.Request(x.Metadata,p.Source.Timing,x.Custom,p.Resources,Policy(true),sink);Eq(sink.Requests.Count,1);Eq(sink.Requests[0].Handle,0);Eq(sink.Requests[0].Layer,0);
        });
        AddAsync("P14 library dynamic indexes loaded before requests","E038 M3b18 production library binding",async()=>{
            string dir=NewDirectory("index7");File.Copy(Path.Combine(packA,"soft-hitwhistle.wav"),Path.Combine(dir,"soft-hitwhistle7.wav"));var m=Map("128,192,2000,1,2,2:0:7:40\n128,192,4000,1,0",directory:dir);
            using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=packA,LayeredHitSounds=false});using var p=await lib.PrepareAsync(m,CancellationToken.None);int id=p.Resources.Resolve(2,2,7);Yes(id>0,"map indexed ID");Eq(p.Samples[id].Origin,SampleOrigin.Map);Eq(p.Samples[id].Name,"soft-hitwhistle7.wav");
        });
        AddAsync("P15 map replacement invalidates map IDs retains default IDs","R11 actual library, E038 cache ownership",async()=>{
            string one=NewDirectory("mapone"),two=NewDirectory("maptwo");File.Copy(Path.Combine(packA,"normal-hitnormal.wav"),Path.Combine(one,"normal-hitnormal.wav"));File.Copy(Path.Combine(packA,"normal-hitfinish.wav"),Path.Combine(two,"normal-hitnormal.wav"));
            var m1=Map(timing:"0,500,4,1,1,100,1,0",directory:one);var m2=Map(timing:"0,500,4,1,1,100,1,0",directory:two);using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=packA});var p1=await lib.PrepareAsync(m1,CancellationToken.None);int old=p1.Resources.Resolve(1,1,1),shared=p1.Resources.Resolve(1,1,0);p1.Dispose();using var p2=await lib.PrepareAsync(m2,CancellationToken.None);int fresh=p2.Resources.Resolve(1,1,1);Yes(fresh!=old,"new map ID");Eq(p2.Resources.Resolve(1,1,0),shared);Eq(Hash(p2.Samples[fresh].Bytes),Hash(File.ReadAllBytes(Path.Combine(two,"normal-hitnormal.wav"))));Yes(!p2.Samples.ContainsKey(old),"old map resource absent from new snapshot");
        });
        AddAsync("P16 canceled and disposed library preparation","ENGINEERING supported cancellation boundary, no media open",async()=>{
            var lib=new HitSoundLibrary(new());var m=Map();using var cts=new CancellationTokenSource();cts.Cancel();await ThrowsAsync<OperationCanceledException>(()=>lib.PrepareAsync(m,cts.Token));lib.Dispose();lib.Dispose();await ThrowsAsync<ObjectDisposedException>(()=>lib.PrepareAsync(m,CancellationToken.None));
        });
        AddAsync("P17 unsupported timing and malformed registered metadata explicit","R11 containment, not invented timing defaults",async()=>{
            using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=packA});using var a=await lib.PrepareAsync(Map(timing:"0,500"),CancellationToken.None);Yes(a.Source.TimingError is not null,"unsupported timing reason");using var b=await lib.PrepareAsync(Map("128,192,2000,1,2,bad\n128,192,4000,1,0"),CancellationToken.None);Yes(b.Source.TimingError is not null,"registered operand error blocks optional effects");
        });
        Add("P18 dynamic-index bound before resource work","ENGINEERING64 distinct indexes",()=>{var f=new ResourcesFixture();Throws<InvalidDataException>(()=>f.R.Prepare(Enumerable.Range(3,65),[]));Eq(f.Trace.Count,0);});
        Add("P19 catalog64MiB budget and release","ENGINEERING bounded memory ownership",()=>{var c=new EffectSampleCatalog();byte[] large=Bytes(count:8*1024*1024);int id=0;for(int i=0;i<8;i++)id=c.Load(new("x.wav",large,SampleOrigin.Map,SampleLoadMode.MapBytes));Throws<InvalidDataException>(()=>c.Load(new("x.wav",large,SampleOrigin.Map,SampleLoadMode.MapBytes)));c.Free(id);Yes(c.Load(new("x.wav",large,SampleOrigin.Map,SampleLoadMode.MapBytes))>0,"free releases byte budget");});
        AddAsync("P20 configured but empty independent pack remains explicit missing","ENGINEERING no fabricated default content",async()=>{using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=NewDirectory("emptypack")});using var p=await lib.PrepareAsync(Map(),CancellationToken.None);Eq(p.Samples.Count,0);Yes(p.MissingLayers>=2,"missing pack entries counted");});
    }

    static object Snapshot(CircleSession s)=>new {s.Ledger.Score,s.Ledger.CurrentCombo,s.Ledger.MaximumCombo,s.Ledger.ComboSnapshot,s.Ledger.N300,s.Ledger.N100,s.Ledger.N50,s.Ledger.NMiss,s.Ledger.AccuracyPercent,s.Ledger.EventCount,s.Ledger.LastEvent,s.Ledger.LastOutcome,s.JudgedCount,s.Finished,
        results=s.ObjectResults.ToArray(),circles=s.Circles.Select(c=>new{c.Result,c.JudgedAt}).ToArray(),
        sliders=s.Sliders.Select(x=>new{x.Result,HeadResult=x.Head.Result,x.SuccessfulCheckpoints,x.FailedCheckpoints,raw=x.Checkpoints.Select(c=>c.RawResult).ToArray()}).ToArray(),
        spinners=s.Spinners.Select(x=>new{x.Result,x.FinalRaw,x.Observation.Count}).ToArray(),
        hp=s.Health is{} h?new{h.State.H,h.State.A,h.State.V,h.State.Fill,h.State.FillRate,h.State.Rate,h.State.B,h.State.G,h.ResultEvents,h.Failed,h.Recovering,h.RecoveryCount,h.Marker,h.LastPassiveLoss}:null};
    static void Matched(CircleSession a,CircleSession b)
    {string x=JsonSerializer.Serialize(Snapshot(a)),y=JsonSerializer.Serialize(Snapshot(b));Obs(new{enabled=Snapshot(a),disabled=Snapshot(b)});Eq(x,y);}
    static void IntegrationCases()
    {
        foreach(var x in new[]{(0,Judgement.Hit300,1),(50,Judgement.Hit100,1),(100,Judgement.Hit50,1),(150,Judgement.Miss,0)})
            Add($"I01 circle result offset{x.Item1} positive hook order","E012 M1884 /E037 pre-completion sound",()=>{
                var m=Map();var s=Fresh(m);int hooks=0;s.PositiveCircleSound+=c=>{hooks++;Yes(ReferenceEquals(c,m.Circles[0]),"own evaluated identity");Eq(s.Circles[0].Result,x.Item2);Eq(s.Circles[0].JudgedAt,0);Eq(s.JudgedCount,0);Eq(s.Ledger.EventCount,0);Eq(s.Health!.ResultEvents,0);};
                s.Press(m.Circles[0].Position,2000+x.Item1);Eq(hooks,x.Item3);Eq(s.Circles[0].Result,x.Item2);Eq(s.JudgedCount,1);Eq(s.Ledger.EventCount,1);Eq(s.Health!.ResultEvents,1);s.Press(m.Circles[0].Position,2000+x.Item1);s.Advance(2200);Eq(hooks,x.Item3);
            });
        Add("I02 late hit selects stored start+5 not judgement timestamp","E037 M24bb; actual production positive event",()=>{
            var m=Map("128,192,2000,1,2\n128,192,4000,1,4",timing:"0,500,4,1,0,10,1,0\n2050,500,4,3,0,100,1,0");var s=Fresh(m);var f=new ResourcesFixture();f.PrepareDefaults();var sink=new Sink();s.PositiveCircleSound+=c=>CircleSampleSelection.Request(m.CircleSounds!.Circles.Single(p=>ReferenceEquals(p.Object,c)),m.CircleSounds.Timing,-1,f.R,Policy(),sink);
            s.Press(new(128,192),2100);Eq(s.Circles[0].Result,Judgement.Hit50);Eq(sink.Requests.Count,1);Eq(sink.Requests[0].Layer,2);Eq(sink.Requests[0].Volume,8);Near(sink.Requests[0].Pan,-.2,2e-8);
        });
        Add("I03 consecutive circles use own sidecar metadata","E037 object fields; actual event source identity",()=>{
            var m=Map("64,192,2000,1,2,1:0:0:20\n448,192,4000,1,4,3:0:0:90");var f=new ResourcesFixture();f.PrepareDefaults();var sink=new Sink();var s=Fresh(m);s.PositiveCircleSound+=c=>{var meta=m.CircleSounds!.Circles.Single(p=>ReferenceEquals(p.Object,c));CircleSampleSelection.Request(meta,m.CircleSounds.Timing,-1,f.R,Policy(),sink);};s.Press(new(64,192),2000);s.Press(new(448,192),4000);Seq(sink.Requests.Select(r=>r.Layer),2,4);Seq(sink.Requests.Select(r=>r.Volume),17,90);Yes(ReferenceEquals(sink.Requests[1].Source,m.Circles[1]),"second circle source retained");
        });
        Add("I04 missing samples never change valid judgement","E037 no audible guarantee; ENGINEERING failure containment",()=>{
            var m=Map();var a=Fresh(m);var b=Fresh(m);var f=new ResourcesFixture();f.R.Prepare([],[]);var sink=new Sink();a.PositiveCircleSound+=c=>CircleSampleSelection.Request(m.CircleSounds!.Circles.Single(x=>ReferenceEquals(x.Object,c)),m.CircleSounds.Timing,-1,f.R,Policy(true),sink);a.Press(new(128,192),2000);b.Press(new(128,192),2000);Eq(sink.Requests.Count,0);Matched(a,b);
        });
        Add("I05 optional sink exception cannot become MISS","R11 ENGINEERING exception containment after classification",()=>{
            var m=Map();var a=Fresh(m);var b=Fresh(m);a.PositiveCircleSound+=_=>throw new IOException("effect sentinel");a.Press(new(128,192),2000);b.Press(new(128,192),2000);Eq(a.Circles[0].Result,Judgement.Hit300);Yes(a.CircleSoundError?.Contains("effect sentinel",StringComparison.Ordinal)==true,"diagnostic kept");Matched(a,b);
        });
        Add("I06 calibration private notifications have no sound or score","E035 private1024/1028 boundary preserved",()=>{
            var m=Map();var s=Fresh(m);int count=0;s.PositiveCircleSound+=_=>count++;var c=new HealthCalibration();c.Trial(s.Health!.Chart);Eq(count,0);Eq(s.Ledger.EventCount,0);Eq(s.Health.ResultEvents,0);Eq(s.JudgedCount,0);
        });
        Add("I07 enabled versus disabled matched circle event stream","E033/E034/E035/E036 isolation at each exposed step",()=>{
            var m=Map("128,192,2000,1,14\n128,192,4000,1,14\n128,192,6000,1,14\n128,192,8000,1,14\n128,192,10000,1,14");var a=Fresh(m);var b=Fresh(m);var f=new ResourcesFixture();f.PrepareDefaults();var q=new List<Action>();var played=new List<SamplePlaybackRequest>();var p=new CircleSoundPlayback(played.Add,q.Add,()=>{});p.Resume();
            a.PositiveCircleSound+=c=>CircleSampleSelection.Request(m.CircleSounds!.Circles.Single(x=>ReferenceEquals(x.Object,c)),m.CircleSounds.Timing,-1,f.R,Policy(true),p);Eq(a.Scoring.K,b.Scoring.K);Matched(a,b);
            foreach(int t in new[]{2000,4050,6100,8150}){a.Press(new(128,192),t);b.Press(new(128,192),t);Matched(a,b);q.ForEach(run=>run());q.Clear();Matched(a,b);}
            foreach(int t in new[]{10151,12000,14000}){a.Advance(t);b.Advance(t);Matched(a,b);}Eq(played.Count,12);Eq(a.Ledger.N300,(ushort)1);Eq(a.Ledger.N100,(ushort)1);Eq(a.Ledger.N50,(ushort)1);Eq(a.Ledger.NMiss,(ushort)2);
        });
        Add("I08 mixed slider spinner gameplay produces circle hooks only","R11 explicit circle-only scope; matched production noncircle streams",()=>{
            var m=Map("128,192,2000,1,0\n128,192,4000,2,0,L|228:192,1,100\n256,192,6000,8,0,7000\n128,192,9000,1,0");var a=Fresh(m);var b=Fresh(m);int hooks=0;a.PositiveCircleSound+=_=>hooks++;
            a.Press(new(128,192),2000);b.Press(new(128,192),2000);Matched(a,b);a.Press(new(128,192),4000);b.Press(new(128,192),4000);Matched(a,b);
            foreach(int t in new[]{4464,4500,6000,7001}){a.Advance(t);b.Advance(t);Matched(a,b);}a.Press(new(128,192),9000);b.Press(new(128,192),9000);Matched(a,b);Eq(hooks,2);
        });
        foreach(double health in new[]{159d,160d,200d})Add($"I09 C01 recovery threshold H{health}","R09-C01 correction retained with sound event subscription",()=>{
            var s=Fresh(Map());int hooks=0;s.PositiveCircleSound+=_=>hooks++;var hp=s.Health!;hp.State.Assign(health);hp.State.ConfigureFill(0,.02,false);hp.SupplyRecoveryCount(1);hp.BeginRecovery();Eq(hooks,0);Eq(hp.Recovering,true);Eq(hp.RecoveryCount,0);Eq(hp.State.Fill,health<160);Near(hp.State.H,health<160?200:health);Near(hp.State.A,health<160?319:health);
        });
        Add("I10 HistoricalR09 versus E036FreshOrdinary split unchanged","R09/R10 explicit compatibility boundary",()=>{
            var m=Map();var historic=new CircleSession(m,ScoreConfiguration.FromBeatmap(m));var fresh=Fresh(m);Eq(historic.HealthPolicy,HealthPreparationPolicy.HistoricalR09);Eq(historic.Health,null);Eq(fresh.HealthPolicy,HealthPreparationPolicy.E036FreshOrdinary);Yes(fresh.Health is not null,"fresh unmarked circles supported");
        });
        Add("I11 restart fresh session allows one new hook","R11 generation component plus production fresh session",()=>{
            var m=Map();int hooks=0;var old=Fresh(m);old.PositiveCircleSound+=_=>hooks++;old.Press(new(128,192),2000);old.Press(new(128,192),2000);Eq(hooks,1);var fresh=Fresh(m);fresh.PositiveCircleSound+=_=>hooks++;Eq(fresh.Ledger.EventCount,0);fresh.Press(new(128,192),2000);Eq(hooks,2);Eq(fresh.Ledger.N300,(ushort)1);
        });
        AddAsync("I12 independent parsed fixture reaches original-derived requests","E037 start+5 and E038 dynamic fallback through actual production library",async()=>{
            string path=Path.Combine(root,"re/qa/qa09/Inputs/ordinary-samples.osu");var m=Beatmap.Load(path);inputs.Add(new{name="ordinary-samples.osu",text=File.ReadAllText(path),sha256=Hash(File.ReadAllBytes(path))});using var lib=new HitSoundLibrary(new(){DefaultPackDirectory=packA,LayeredHitSounds=false});using var data=await lib.PrepareAsync(m,CancellationToken.None);var sink=new Sink();var s=Fresh(m);s.PositiveCircleSound+=c=>{var x=data.Circles[c];CircleSampleSelection.Request(x.Metadata,data.Source.Timing,x.Custom,data.Resources,Policy(false),sink);};
            foreach(var c in m.Circles)s.Press(c.Position,c.StartTime);Seq(sink.Requests.Select(r=>r.Layer),2,4,8);Seq(sink.Requests.Select(r=>r.Volume),51,25,76);Seq(sink.Requests.Select(r=>data.Samples[r.Handle].Name),"drum-hitwhistle","normal-hitfinish","soft-hitclap");Eq(s.Ledger.N300,(ushort)3);Eq(s.JudgedCount,3);
        });
    }
}
