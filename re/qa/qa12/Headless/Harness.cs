// QA12 independently derives expectations from E024-E026/E037-E041.
// Linked production is the subject; no development assertions are imported.
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using System.IO.Compression;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
namespace Ohno.QA12;
public static partial class Tests
{
    record Check(string Id,string Authority,string Class,Action Body);
    static readonly List<Check> checks=[];
    static readonly List<object> results=[], evidence=[];
    static string root="",scratch="",packA="",packB="",fixture="";
    static void Add(string id,string authority,Action body,string classification="Executed finite component")=>checks.Add(new(id,authority,classification,body));
    static void Eq<T>(T actual,T expected){evidence.Add(new{expected,actual});if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception($"expected {expected}, actual {actual}");}
    static void Seq<T>(IEnumerable<T> actual,params T[] expected){var a=actual.ToArray();evidence.Add(new{expected,actual=a});if(!a.SequenceEqual(expected))throw new Exception($"expected [{string.Join(',',expected)}], actual [{string.Join(',',a)}]");}
    static void Yes(bool actual,string label){evidence.Add(new{expected=true,actual,label});if(!actual)throw new Exception(label);}
    static void Bits(float actual,uint expected)=>Eq(unchecked((uint)BitConverter.SingleToInt32Bits(actual)),expected);
    static void Near(double actual,double expected,double epsilon=1e-12){evidence.Add(new{expected,actual,epsilon});if(!double.IsFinite(actual)||Math.Abs(actual-expected)>epsilon)throw new Exception($"expected {expected:R}, actual {actual:R}");}
    static void Throws<T>(Action a)where T:Exception{try{a();}catch(T e){evidence.Add(new{expected=typeof(T).Name,actual=e.GetType().Name,e.Message});return;}throw new Exception("Expected "+typeof(T).Name);}
    static string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
    static string Read(string p)=>File.ReadAllText(Path.Combine(root,p));
    static readonly CircleSoundPolicy Policy=new(true,false,true,0,100,false,80,50);
    const string Timing="0,600,4,1,1,23,1,0\n1200,600,4,2,2,41,1,0\n6200,600,4,3,7,67,1,0\n6203,600,4,1,9,89,1,0";
    static Beatmap Map(string rows="71,89,2200,8,65551,6200,2:3:7:61:violet.wav",string timing=Timing)
    {
        string text=$"osu file format v14\n[General]\nMode:0\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n{timing}\n[HitObjects]\n{rows}\n";
        fixture=Hash(System.Text.Encoding.UTF8.GetBytes(text))+".osu";
        string path=Path.Combine(scratch,fixture);File.WriteAllText(path,text);
        string output=Path.Combine(root,"re/qa/qa12/Inputs",fixture);if(!File.Exists(output))File.WriteAllText(output,text);
        return Beatmap.Load(path);
    }
    static SpinnerState State(int duration=4000)=>new(new(2200,2200+duration,65551,null),5);
    static void Count(SpinnerState s,int n){for(int i=1;i<=n;i++)s.Observation.Observe(i,s.Required);}
    public sealed record Op(string Name,int Handle,int Attr=0,float Value=0,bool? Flag=null);
    // Independent backend: no inferred state mutation, playback, decoder or native imports.
    public sealed class Backend:ISpinnerSampleChannels
    {
        public readonly List<Op> Ops=[];public readonly Dictionary<int,int> States=[];
        public Func<int,bool,int> Acquisition=(_,_)=>733;
        public Func<int,int,long?> Query=(_,_)=>8193;
        public Action<Op>? Enter;
        public FrequencyCapability FrequencyCapability{get;set;}=FrequencyCapability.Supported;
        void Record(Op o){Ops.Add(o);Enter?.Invoke(o);}
        public int Acquire(int h,bool f){Record(new("Acquire",h,Flag:f));return Acquisition(h,f);}
        public int Activity(int h){Record(new("Activity",h));return States.GetValueOrDefault(h);}
        public long? Length(int h,int mode){Record(new("Length",h,mode));return Query(h,mode);}
        public void Attribute(int h,int a,float v)=>Record(new("Attribute",h,a,v));
        public void Play(int h,bool r)=>Record(new("Play",h,Flag:r));
        public void Pause(int h)=>Record(new("Pause",h));public void Stop(int h)=>Record(new("Stop",h));
        public void Dispose()=>Record(new("Dispose",0));
    }
    sealed class Rig
    {
        public readonly Backend B=new();public readonly SharedSliderChannels Global;public readonly SharedSpinnerChannel Spin;
        public int Sample=719;public Action? Resolve;
        public Rig(bool warm=true)
        {
            Global=new(new GlobalSampleTiming([new(0,1,1,25,true,0),new(100,2,17,73,false,1)]),B,(r,_,_)=>r==0?101:202){EffectVolume=80,UniversalVolume=50};
            B.Acquisition=(sample,_)=>sample==719?733:sample;
            Spin=Global.AttachSpinner(B,()=>{B.Ops.Add(new("Resolve",Sample));Resolve?.Invoke();return Sample;});
            if(warm)Global.Clock(0);B.Ops.Clear();
        }
        public void Trace()=>evidence.Add(new{Global.CurrentBase,Global.CurrentIndex,Global.Channel0,Global.Channel1,Spin.Channel,Global.Pending,ops=B.Ops.ToArray()});
    }
    sealed class Sink:ISamplePlaybackSink
    {public readonly List<SamplePlaybackRequest> Requests=[];public Action<SamplePlaybackRequest>? Enter;public void Enqueue(SamplePlaybackRequest r){Requests.Add(r);Enter?.Invoke(r);}}
    sealed class Files(string label,List<string> trace):ISampleFiles
    {
        public readonly Dictionary<string,byte[]> Data=[];
        public bool Exists(string n){trace.Add(label+".exists:"+n);return Data.ContainsKey(n);}
        public byte[]? Read(string n){trace.Add(label+".read:"+n);return Data.GetValueOrDefault(n);}
    }
    sealed class Defaults(List<string> trace):IDefaultSamples
    {public readonly Dictionary<string,byte[]> U=[],G=[];public byte[]? Ui(string k){trace.Add("ui:"+k);return U.GetValueOrDefault(k);}public byte[]? Gameplay(string k){trace.Add("default:"+k);return G.GetValueOrDefault(k);}}
    sealed class Loader:ISampleLoader
    {public readonly List<SampleBytes> Loaded=[];public readonly List<int> Freed=[];public Func<SampleBytes,int>? Result;public int Load(SampleBytes b){Loaded.Add(b);return Result?.Invoke(b)??(1000+Loaded.Count);}public void Free(int h)=>Freed.Add(h);}
    static byte[] Marker(byte v)=>Enumerable.Repeat(v,1057).ToArray();
    sealed class Resources
    {
        public readonly List<string> Trace=[];public readonly Files Map,Skin;public readonly Defaults Defaults;public readonly Loader Loader=new();public readonly CircleSampleResources R;
        public Resources(SampleResourcePolicy? policy=null){Map=new("map",Trace);Skin=new("skin",Trace);Defaults=new(Trace);R=new(Map,Skin,Defaults,Loader,policy??new(14,false,false,true,true,"Private",true,CultureInfo.InvariantCulture));}
        public void Standards(){foreach(string bank in new[]{"normal","soft","drum"})foreach(string name in new[]{"hitnormal","hitfinish","hitwhistle","hitclap"})Defaults.G[bank+"-"+name]=Marker((byte)(Defaults.G.Count+1));R.Prepare([1,2,7,9],[]);}
    }
    public static int Main(string[] args)
    {
        CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;root=Path.GetFullPath(args[0]);packA=args.Length>2?args[2]:"";packB=args.Length>3?args[3]:"";
        scratch=Path.Combine(Path.GetTempPath(),"ohno-qa12-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        Metadata();Channels();Frequency();Observations();Motion();Deferred();Bonus();Completion();Sessions();Audits();Assets();
        int failed=0;
        foreach(var c in checks){evidence.Clear();fixture="";string? error=null;try{c.Body();}catch(Exception e){failed++;error=e.ToString();}results.Add(new{id=c.Id,authority=c.Authority,classification=c.Class,passed=error is null,fixture,observations=evidence.ToArray(),error});Console.WriteLine($"{(error is null?"PASS":"FAIL")} {c.Id}"+(error is null?"":" "+error));}
        var manifest=Directory.GetFiles(Path.Combine(root,"reconstructed/CircleGame"),"*.cs",SearchOption.AllDirectories).Where(p=>!p.Contains(Path.DirectorySeparatorChar+"obj"+Path.DirectorySeparatorChar)).Concat(Directory.GetFiles(Path.Combine(root,"re/qa/qa12/Headless"),"*.cs")).ToDictionary(p=>Path.GetRelativePath(root,p).Replace('\\','/'),p=>Hash(File.ReadAllBytes(p)));
        byte[] bytes=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA12.v1",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-preflight",runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,executed=checks.Count,passed=checks.Count-failed,failed,cases=results,manifest},new JsonSerializerOptions{WriteIndented=true});File.WriteAllBytes(args[1],bytes);
        Console.WriteLine($"QA12: {checks.Count-failed} passed, {failed} failed, {checks.Count} executed");Console.WriteLine("QA12 SHA256 "+Hash(bytes));using var ms=new MemoryStream();using(var gz=new GZipStream(ms,CompressionLevel.SmallestSize,true))gz.Write(bytes);Console.WriteLine("QA12 EVIDENCE "+Convert.ToBase64String(ms.ToArray()));Directory.Delete(scratch,true);return failed==0?0:1;
    }
}
