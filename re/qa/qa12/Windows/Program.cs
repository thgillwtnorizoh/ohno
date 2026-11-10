using System.IO;
using System.Reflection;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows.Threading;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.QA12;
internal static class Program
{
    static readonly List<object> results=[],observations=[];
    static int failed;
    static void Eq<T>(T actual,T expected){observations.Add(new{expected,actual});if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception($"expected {expected}, actual {actual}");}
    static void Run(string id,Action body,string classification="Executed Windows empty-resource component"){observations.Clear();string? error=null;try{body();}catch(Exception e){failed++;error=e.ToString();}results.Add(new{id,classification,passed=error is null,observations=observations.ToArray(),error});Console.WriteLine($"{(error is null?"PASS":"FAIL")} {id} {error}");}
    static void Set(object obj,string field,object value)=>obj.GetType().GetField(field,BindingFlags.NonPublic|BindingFlags.Instance)!.SetValue(obj,value);
    static T Get<T>(object obj,string field)=>(T)obj.GetType().GetField(field,BindingFlags.NonPublic|BindingFlags.Instance)!.GetValue(obj)!;
    [STAThread] static int Main(string[] args)
    {
        using var empty=new WindowsSliderLoops();
        Run("WE01 empty preparation no resources",()=>{empty.PrepareAsync(new Dictionary<int,SampleBytes>(),"",CancellationToken.None).GetAwaiter().GetResult();Eq(empty.Ready,0);Eq(empty.Error,null);});
        Run("WE02 accurate unsupported frequency and no length",()=>{Eq(empty.FrequencyCapability,FrequencyCapability.Unsupported);foreach(int h in new[]{-1,0,733}){Eq(empty.Length(h,0),null);empty.Attribute(h,1,60000);}Eq(empty.Error,null);Eq(empty.FrequencyStatus.Contains("Unsupported"),true);});
        Run("WE03 missing identities remain unavailable",()=>{foreach(int h in new[]{-1,0,733}){Eq(empty.Acquire(h,false),0);Eq(empty.Activity(h),0);empty.Play(h,false);empty.Pause(h);empty.Stop(h);}Eq(empty.Ready,0);Eq(empty.Error,null);});
        Run("WE04 pause restart-style calls and disposal",()=>{empty.PauseAll();empty.Dispose();empty.Dispose();empty.Play(733,false);empty.Attribute(733,1,60000);Eq(empty.Ready,0);Eq(empty.Acquire(733,false),0);Eq(empty.Error,null);});
        string temp=Path.Combine(Path.GetTempPath(),"qa12-win-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        try{
            string path=Path.Combine(temp,"fixture.osu");File.WriteAllText(path,"osu file format v14\n[General]\nMode:0\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n[TimingPoints]\n0,600,4,1,1,25,1,0\n[HitObjects]\n71,89,2200,8,14,9200,0:0:0:0:\n");
            var map=Beatmap.Load(path);using var lib=new HitSoundLibrary(new());var data=lib.PrepareAsync(map,CancellationToken.None).GetAwaiter().GetResult();Eq(data.Samples.Count,0);
            var ctor=typeof(WindowsCircleEffects).GetConstructors(BindingFlags.NonPublic|BindingFlags.Instance).Single();using var effects=(WindowsCircleEffects)ctor.Invoke([data,new HitSoundSettings(),Dispatcher.CurrentDispatcher]);
            var backend=new Tests.Backend();var global=new SharedSliderChannels(new GlobalSampleTiming(data.Source.Timing),backend,(_,_,_)=>0);var spin=global.AttachSpinner(backend,()=>719);spin.Channel=733;
            Set(effects,"continuing",global);Set(effects,"spinnerChannel",spin);var session=new CircleSession(map);effects.Attach(session);effects.Resume();
            var handler=typeof(WindowsCircleEffects).GetMethod("OnSpinner",BindingFlags.NonPublic|BindingFlags.Instance)!;void Stage(SpinnerSoundStage stage)=>handler.Invoke(effects,[session.Spinners[0],stage]);
            Run("WI01 actual observed-change callback",()=>{backend.Ops.Clear();session.Spinners[0].Observation.Observe(1,35);Stage(SpinnerSoundStage.ObservedChange);Eq(backend.Ops[0].Name,"Length");Eq(backend.Ops[^1].Attr,1);Eq(spin.LastRatio,1f/35f);},"Executed Windows callback with recording backend");
            Run("WI02 actual motion callback defers Play",()=>{backend.Ops.Clear();Stage(SpinnerSoundStage.Motion);Eq(backend.Ops.Any(x=>x.Name=="Play"),false);Eq(global.Pending,1);global.Drain();Eq(backend.Ops[^1].Name,"Play");Eq(backend.Ops[^1].Flag,false);},"Executed Windows callback with recording backend");
            Run("WI03 actual zero-U cleanup current target",()=>{global.Cancel();backend.Ops.Clear();Stage(SpinnerSoundStage.ZeroMotion);spin.Channel=919;global.Drain();Eq(backend.Ops.Single().Name,"Pause");Eq(backend.Ops.Single().Handle,919);},"Executed Windows callback with recording backend");
            Run("WI04 actual completion cleanup before positive hook",()=>{global.Cancel();backend.Ops.Clear();var s=session.Spinners[0];for(int i=2;i<=36;i++)s.Observation.Observe(i,35);s.Complete(9201);Eq(s.Result,Judgement.Hit300);Eq(global.Pending,1);global.Drain();Eq(backend.Ops.Single().Name,"Pause");},"Executed Windows callback with recording backend");
            Run("WI05 settings disable suppresses callback and reenable permits",()=>{backend.Ops.Clear();effects.Enabled=false;Stage(SpinnerSoundStage.Motion);Eq(global.Pending,0);effects.Enabled=true;effects.Resume();Stage(SpinnerSoundStage.Motion);Eq(global.Pending,1);global.Cancel();},"Executed Windows callback with recording backend");
            Run("WI06 generation cancellation on restart",()=>{Stage(SpinnerSoundStage.Motion);Eq(global.Pending,1);int before=Get<int>(effects,"loopGeneration");effects.Restart(new CircleSession(map));Eq(global.Pending,0);Eq(Get<int>(effects,"loopGeneration")>before,true);Eq(global.SameMapIdentity,true);},"Executed Windows callback with recording backend");
            Run("WI07 backend failure observable and contained",()=>{backend.Enter=o=>{if(o.Name=="Length")throw new IOException("QA12 length failure");};Stage(SpinnerSoundStage.ObservedChange);Eq(effects.Status.Contains("QA12 length failure"),true);backend.Enter=null;},"Executed Windows callback with recording backend");
            Run("WI08 pause teardown and repeat disposal",()=>{effects.Pause();Eq(global.Pending,0);effects.Resume();effects.Dispose();effects.Dispose();Eq(global.Pending,0);},"Executed Windows empty-resource component");
        }finally{Directory.Delete(temp,true);}
        byte[] b=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA12.Windows.v1",source=Environment.GetEnvironmentVariable("GITHUB_SHA"),executed=results.Count,passed=results.Count-failed,failed,cases=results,limit="Empty actual Windows resources; injected recording backend for callback interleavings. No MediaPlayer construction/Open/listening/window/device test."},new JsonSerializerOptions{WriteIndented=true});File.WriteAllBytes(args[0],b);Console.WriteLine($"QA12 WINDOWS: {results.Count-failed} passed, {failed} failed");Console.WriteLine("QA12 WINDOWS SHA256 "+Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant());using var ms=new MemoryStream();using(var gz=new GZipStream(ms,CompressionLevel.SmallestSize,true))gz.Write(b);Console.WriteLine("QA12 WINDOWS EVIDENCE "+Convert.ToBase64String(ms.ToArray()));return failed==0?0:1;
    }
}
