using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Windows.Threading;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;

internal static class Program
{
    static int groups;
    static void Yes(bool value) { if (!value) throw new Exception("R14-Q01 group " + (groups + 1)); }
    static void Eq<T>(T a, T b) => Yes(EqualityComparer<T>.Default.Equals(a, b));
    static void Pass(string label) => Console.WriteLine($"PASS R14-Q01 {++groups:00} {label}");
    static long Count(string json, string key) { using var d = JsonDocument.Parse(json); return d.RootElement.GetProperty("counts").TryGetProperty(key, out var v) ? v.GetInt64() : 0; }
    static string Field(string json, string key) { using var d = JsonDocument.Parse(json); return d.RootElement.GetProperty("fields").GetProperty(key).GetString()!; }
    static void Set(object o, string f, object v) => o.GetType().GetField(f, BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(o,v);
    static void Pump() { var frame = new DispatcherFrame(); Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => frame.Continue = false), DispatcherPriority.ApplicationIdle); Dispatcher.PushFrame(frame); }
    static void Stage(WindowsCircleEffects effects, SpinnerState s, SpinnerSoundStage stage) => typeof(WindowsCircleEffects).GetMethod("OnSpinner", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(effects, [s,stage]);
    static PreparedCircleEffects Prepare(HitSoundLibrary lib, Beatmap m) => lib.PrepareAsync(m,CancellationToken.None).GetAwaiter().GetResult();
    [STAThread] static int Main(string[] args)
    {
        string repo = args[0], pack = args[1];
        string temp = Path.Combine(Path.GetTempPath(), "ohno-q01-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(temp);
        try
        {
            string config = HitSoundSettings.ConfigurationPath(temp); Eq(config, Path.Combine(temp,"hitsounds.json"));
            Eq(HitSoundSettings.ConfigurationPath(),Path.Combine(AppContext.BaseDirectory,"hitsounds.json"));
            var absent = HitSoundSettings.Read(config); Eq(absent.DefaultPackDirectory,null);
            Pass("D01 executable-relative configuration path and absent-file defaults");
            File.WriteAllText(config, JsonSerializer.Serialize(new HitSoundSettings { DefaultPackDirectory = pack }));
            var configured = HitSoundSettings.Read(config); Eq(configured.DefaultPackDirectory,pack);
            var d = new EffectAudioDiagnostics(configured,config); Eq(Field(d.Json(),"configurationExists"),"True");
            Pass("D02 explicit configured pack read from selected settings file");
            string fixture = Path.Combine(repo,"reconstructed/CircleGame/Fixtures/spinner-practice.osu");
            string mapPath = Path.Combine(temp,"practice.osu"); File.Copy(fixture,mapPath); var map = Beatmap.Load(mapPath);
            using (var lib = new HitSoundLibrary(new() { DefaultPackDirectory = Path.Combine(temp,"wrong") }))
            using (var data = Prepare(lib,map)) { Eq(data.SpinHandle,-1); Eq(data.BonusHandle,-1); Eq(Field(data.Diagnostics.Json(),"defaultPackDirectoryExists"),"False"); }
            Pass("D03 wrong directory remains missing, gameplay parser succeeds");
            string circleOnly = Path.Combine(temp,"circles"); Directory.CreateDirectory(circleOnly);
            File.Copy(Path.Combine(pack,"normal-hitnormal.wav"),Path.Combine(circleOnly,"normal-hitnormal.wav"));
            using (var lib = new HitSoundLibrary(new() { DefaultPackDirectory = circleOnly }))
            using (var data = Prepare(lib,map)) { Yes(data.Resources.Resolve(1,1,0)>0); Eq(data.SpinHandle,-1); Eq(data.BonusHandle,-1); }
            Pass("D04 valid circle pack does not imply either spinner resource");
            using var fullLib = new HitSoundLibrary(configured); using var full = Prepare(fullLib,map);
            Yes(full.SpinHandle>0 && full.BonusHandle>0); Eq(full.SpinnerLimitation,null);
            Eq(full.Samples[full.SpinHandle].Origin,SampleOrigin.Default); Eq(full.Samples[full.SpinHandle].Name,"spinnerspin");
            Eq(full.Samples[full.SpinHandle].Loop,true); Eq(full.Samples[full.BonusHandle].Loop,false);
            Pass("D05 correct independent pack maps positive separate loop/bonus handles and provenance");
            string v15 = Path.Combine(temp,"v15.osu"); File.WriteAllText(v15,File.ReadAllText(mapPath).Replace("format v14","format v15"));
            bool rejected=false;try { Beatmap.Load(v15); } catch(InvalidDataException) { rejected=true; } Yes(rejected);
            string v13=Path.Combine(temp,"v13.osu");File.WriteAllText(v13,File.ReadAllText(mapPath).Replace("format v14","format v13"));
            rejected=false;try { Beatmap.Load(v13); } catch(InvalidDataException) { rejected=true; } Yes(rejected);
            // Artificial bounded metadata variant isolates the defensive audio gate.
            // Normal parser never produces a non-v14 spinner. No playable-v15 claim.
            var unsupported=map with { CircleSounds=map.CircleSounds! with { Format=15 }, HealthSource=map.HealthSource! with { } };
            unsupported.HealthSource!.BindFreshOwner(unsupported);Yes(unsupported.HealthSource!.IsFreshFor(unsupported));
            Yes(HitSoundLibrary.SpinnerAdmission(unsupported)!.Contains("format14"));
            using (var data = Prepare(fullLib,unsupported)) { Eq(data.Spinners.Count,0); Yes(Field(data.Diagnostics.Json(),"spinnerAdmission").Contains("format14")); }
            Pass("D06 non14 parser rejection and artificial defensive audio-format rejection");
            File.Copy(Path.Combine(pack,"spinnerspin.wav"),Path.Combine(temp,"spinnerspin.wav"));
            using (var data = Prepare(fullLib,map)) { Eq(data.Samples[data.SpinHandle].Origin,SampleOrigin.Map); Eq(data.Samples[data.SpinHandle].Name,"spinnerspin.wav"); }
            File.Delete(Path.Combine(temp,"spinnerspin.wav"));
            Pass("D07 map unprefixed index0 resource precedes default pack");
            using (var skinLib = new HitSoundLibrary(configured with { SkinName="Independent",SkinDirectory=pack }))
            using (var data = Prepare(skinLib,map)) { Eq(data.Samples[data.SpinHandle].Origin,SampleOrigin.Skin); }
            Pass("D08 admitted skin precedes default resource");
            using (var cached = new HitSoundLibrary(new() { DefaultPackDirectory = circleOnly }))
            {
                using (var missingBefore=Prepare(cached,map)) Eq(missingBefore.SpinHandle,-1);
                File.Copy(Path.Combine(pack,"spinnerspin.wav"),Path.Combine(circleOnly,"spinnerspin.wav"));
                using (var reload=Prepare(cached,map)) Eq(reload.SpinHandle,-1);
                using var freshLib = new HitSoundLibrary(new() { DefaultPackDirectory = circleOnly });
                using var fresh = Prepare(freshLib,map); Yes(fresh.SpinHandle>0);
            }
            Pass("D09 cached default miss survives map reload; fresh library/app reads added file");
            using var loops = new WindowsSliderLoops { Diagnostics = d };
            Eq(loops.IsReady(full.SpinHandle),false); Eq(loops.Acquire(full.SpinHandle,false),0);
            full.Diagnostics.Resource("probe",full.SpinHandle,full.Samples);
            Eq(Field(full.Diagnostics.Json(),"probe.catalogued"),"True"); Eq(Field(full.Diagnostics.Json(),"probe.decoder"),"NOT OPENED");
            Pass("D10 actual catalogued resource distinct from empty Windows decoder readiness; no media opened");
            var ctor = typeof(WindowsCircleEffects).GetConstructors(BindingFlags.NonPublic|BindingFlags.Instance).Single();
            using var effects=(WindowsCircleEffects)ctor.Invoke([full,configured,Dispatcher.CurrentDispatcher]);
            var backend=new RecordingSampleChannels();
            var global=new SharedSliderChannels(new GlobalSampleTiming(full.Source.Timing),backend,(_,_,_)=>0);
            var spin=global.AttachSpinner(backend,()=>full.SpinHandle); Set(effects,"continuing",global);Set(effects,"spinnerChannel",spin);
            var session=new CircleSession(map); effects.Attach(session);effects.Resume();global.Clock(3000);
            var state=session.Spinners[0];Eq(state.Required,15);
            state.SampleAngle(0,3001,20,true);state.SampleAngle(.6,3021,20,true);state.Integrate(3021,20);
            Yes(state.SmoothedMotion!=0);Yes(global.Pending>0);Yes(!backend.Operations.Any(o=>o.Name=="Play"));
            Pass("D11 nonzero U reaches actual session callback and queues continuing start");
            Pump(); Yes(backend.Operations.Any(o=>o.Name=="Play" && o.Channel==full.SpinHandle && o.Value==0));
            Yes(Count(effects.DiagnosticsJson,"shared.flush.executed")>0);
            Pass("D12 actual WPF dispatcher drains deferred Play(false) with recording backend");
            backend.Operations.Clear();Stage(effects,state,SpinnerSoundStage.Motion);Stage(effects,state,SpinnerSoundStage.ZeroMotion);Pump();
            Yes(backend.Operations.Where(o=>o.Name is "Play" or "Pause").Select(o=>o.Name).SequenceEqual(new[]{"Play","Pause"}));
            Yes(Count(effects.DiagnosticsJson,"spinner.pause.submitted")>0);
            Pass("D13 start then zero-motion Pause remains distinct and observable");
            backend.Operations.Clear();Stage(effects,state,SpinnerSoundStage.Motion);effects.Restart(new CircleSession(map));Pump();
            Yes(!backend.Operations.Any(o=>o.Name=="Play"));Yes(Count(effects.DiagnosticsJson,"shared.flush.cancelled")>0);
            Pass("D14 restart generation cancels stale callback and reports cancellation");
            loops.Play(full.SpinHandle,false);Eq(loops.Ready,0);Eq(Count(d.Json(),"loop.start.missing"),1L);
            Pass("D15 unavailable backend channel reports failed start without media");
            var obs=new SpinnerProgressObservation();int bonuses=0;
            for(int i=1;i<=19;i++) obs.Observe(i,15,null,()=>bonuses++);Eq(bonuses,0);obs.Observe(20,15,null,()=>bonuses++);Eq(bonuses,1);
            Yes(SpinnerSampleSelection.BonusAllowed(20,15,0));Yes(!SpinnerSampleSelection.BonusAllowed(20,15,4194304));
            Pass("D16 actual observed C20 first normal bonus for N15; modifier separate");
            var s16=new SpinnerState(map.Spinners[0],5);for(int i=1;i<=16;i++)s16.Observation.Observe(i,15);
            int completion=0;s16.Sound+=(_,e)=>{if(e==SpinnerSoundStage.PositiveCompletion)completion++;};s16.Complete(6001);Eq(s16.Result,Judgement.Hit300);Eq(completion,1);
            Pass("D17 C16 positive300 completion is independent of C20 bonus");
            var current=new CircleSession(map);effects.Restart(current);var s=current.Spinners[0];
            for(int i=1;i<=20;i++)s.Observation.Observe(i,15);
            Stage(effects,s,SpinnerSoundStage.Bonus);Stage(effects,s,SpinnerSoundStage.PositiveCompletion);Pump();
            Yes(Count(effects.DiagnosticsJson,"SpinnerBonus.request")>0);Yes(Count(effects.DiagnosticsJson,"SpinnerCompletion.request")>0);
            Yes(Count(effects.DiagnosticsJson,"SpinnerBonus.voiceMissing")>0);Eq(Count(effects.DiagnosticsJson,"SpinnerBonus.MediaPlayer.Play"),0L);
            Pass("D18 actual Windows bonus/completion request identities and missing bonus voice; no fallback claim");
            var actions=new Queue<Action>();var failure=new CircleSoundPlayback(_=>throw new IOException("controlled bonus failure"),a=>actions.Enqueue(a),()=>{});failure.Resume();
            var policy=new CircleSoundPolicy(true,true,true,0,100,false,80,50);
            SpinnerSampleSelection.Bonus(s.Definition,-1,100,policy,failure);actions.Dequeue()();Eq(failure.Error,null);
            SpinnerSampleSelection.Bonus(s.Definition,777,100,policy,failure);actions.Dequeue()();Eq(failure.Error,"controlled bonus failure");failure.Dispose();
            Pass("D19 actual playback invalid bonus sentinel and contained injected failure; not a decoder test");
            backend.Operations.Clear();effects.Enabled=false;Stage(effects,s,SpinnerSoundStage.Motion);Eq(global.Pending,0);effects.Enabled=true;effects.Resume();
            global.SettingsChanged(0,100);Stage(effects,s,SpinnerSoundStage.Motion);Pump();
            Yes(backend.Operations.Any(o=>o.Name=="Attribute" && o.Attribute==2 && o.Value==0));
            Yes(Count(effects.DiagnosticsJson,"spinner.rejected")>0);
            Pass("D20 effects disable gate and zero gain preserved and observable");
            string before=JsonSerializer.Serialize(new{current.Ledger.Score,current.Ledger.EventCount,current.Health?.State.H,s.Progress,s.Observation.Count,s.Result});
            Stage(effects,s,SpinnerSoundStage.Motion);Stage(effects,s,SpinnerSoundStage.Cleanup);Pump();
            Eq(JsonSerializer.Serialize(new{current.Ledger.Score,current.Ledger.EventCount,current.Health?.State.H,s.Progress,s.Observation.Count,s.Result}),before);
            backend.Operations.Clear();global.Channel0=101;global.Channel1=202;global.Submit(2,1,1,true,256);global.Drain();
            Yes(backend.Operations.Count(o=>o.Name=="Play" && o.Value==1)==2);
            Pass("D21 audio diagnostics leave accounting/state and distinct shared slider Play(true) intact");
            for(int i=0;i<1000;i++){d.Count("bounded",i);d.Set("sample",new string('x',2000));}Eq(Count(d.Json(),"bounded"),1000L);Eq(Field(d.Json(),"sample").Length,1600);
            Eq(loops.FrequencyCapability,FrequencyCapability.Unsupported);Eq(loops.Length(1,0),null);
            Pass("D22 bounded aggregate diagnostics and honest unsupported frequency capability");
            string source=File.ReadAllText(Path.Combine(repo,"reconstructed/CircleGame/Platform/WindowsCircleEffects.cs"));
            Yes(source.Contains("oneShot.media.failed"));Yes(source.Contains("oneShot.decoder.failed"));
            Yes(File.ReadAllText(Path.Combine(repo,"reconstructed/CircleGame/Program.cs")).Contains("Save audio diagnostics"));
            Pass("D23 SOURCE INSPECTION ONLY nonempty decoder/MediaFailed instrumentation and owner export wiring");
            Console.WriteLine($"R14-Q01 DEVELOPMENT: {groups}/23 groups passed (22 executed component, 1 source inspection); no media opened/played, no gameplay window");
            Console.WriteLine("R14-Q01 DIAGNOSTICS " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(effects.DiagnosticsJson)));
            return 0;
        }
        finally { Directory.Delete(temp,true); }
    }
}
