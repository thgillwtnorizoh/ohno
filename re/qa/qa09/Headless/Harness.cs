// QA09: independent original-static oracle, actual linked R11 components only.
// No original binary, WPF window, MediaPlayer, native audio or physical input.
using System.Globalization;
using System.IO.Compression;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
namespace Ohno.QA09;

public static partial class Tests
{
    static readonly List<(string Name, string Authority, Func<Task> Run)> cases = [];
    static readonly List<object> outcomes = [], inputs = [], observations = [];
    static string root = "", scratch = "", packA = "", packB = "";
    static int serial;
    static void Add(string name, string authority, Action run) => cases.Add((name, authority, () => { run(); return Task.CompletedTask; }));
    static void AddAsync(string name, string authority, Func<Task> run) => cases.Add((name, authority, run));
    static void Obs(object value) => observations.Add(value);
    static void Eq<T>(T actual, T expected)
    {
        Obs(new { expected, actual });
        if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"expected={expected}; actual={actual}");
    }
    static void Yes(bool condition, string reason) { Obs(new { condition, reason }); if (!condition) throw new Exception(reason); }
    static void Seq<T>(IEnumerable<T> actual, params T[] expected)
    { var a = actual.ToArray(); Obs(new { expected, actual = a }); if (!a.SequenceEqual(expected)) throw new Exception($"expected=[{string.Join(',',expected)}]; actual=[{string.Join(',',a)}]"); }
    static void Near(double actual, double expected, double tolerance = 1e-10)
    { Obs(new { expected, actual, tolerance }); if (!double.IsFinite(actual) || Math.Abs(actual-expected)>tolerance) throw new Exception($"expected={expected:R}; actual={actual:R}"); }
    static void Throws<T>(Action action) where T:Exception
    { try { action(); } catch(T e) { Obs(new { rejected = typeof(T).Name, e.Message }); return; } throw new Exception("expected "+typeof(T).Name); }
    static async Task ThrowsAsync<T>(Func<Task> action) where T:Exception
    { try { await action(); } catch(T e) { Obs(new { rejected = typeof(T).Name, e.Message }); return; } throw new Exception("expected "+typeof(T).Name); }
    static string Hash(byte[] bytes) => Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    static string NewDirectory(string label)
    { string dir=Path.Combine(scratch, $"{serial++:D4}-{label}");Directory.CreateDirectory(dir);return dir; }
    static Beatmap Map(string rows="128,192,2000,1,0\n384,192,4000,1,0", string timing="0,500,4,1,0,100,1,0", string events="",int hp=0,int version=14,string? directory=null)
    {
        string text=$"osu file format v{version}\n[General]\nMode:0\n[Metadata]\nTitle:QA09 independent finite input\n[Difficulty]\nHPDrainRate:{hp}\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n{timing}\n[Events]\n{events}\n[HitObjects]\n{rows}\n";
        string path=Path.Combine(directory??NewDirectory("map"),"control.osu");File.WriteAllText(path,text,new UTF8Encoding(false));
        inputs.Add(new { name=$"control-{inputs.Count:D3}.osu",text,sha256=Hash(File.ReadAllBytes(path)) });
        return Beatmap.Load(path);
    }
    static CircleSession Fresh(Beatmap map,bool hp=true) => CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map),hp);
    static CircleSampleMetadata Meta(int raw=0,string? payload=null,int x=256,int start=2000) => CircleSampleMetadata.Parse(new(new(x,192),start),raw,payload);
    static CircleSoundPolicy Policy(bool layered=false) => new(true,layered,true,0,100,false,80,100);
    static SampleTiming Point(int set=1,int index=0,int volume=100,double time=0) => new(time,set,index,volume,true,0);
    static byte[] Bytes(int marker=1,int count=1024) { byte[] b=new byte[count];if(count>0)b[0]=(byte)marker;return b; }
    static readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = false, IncludeFields = true };
    public static async Task<int> Main(string[] args)
    {
        root=Path.GetFullPath(args[0]);packA=Path.GetFullPath(args[1]);packB=Path.GetFullPath(args[2]);
        CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;CultureInfo.CurrentUICulture=CultureInfo.InvariantCulture;
        scratch=Path.Combine(Path.GetTempPath(),"ohno-qa09-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        try
        {
            MetadataCases(); SelectionCases(); ResourceCases(); LifecycleCases(); IntegrationCases(); AssetCases(); SourceBoundaries();
            Yes(cases.Select(c=>c.Name).Distinct().Count()==cases.Count,"Unique independent names");
            int failed=0;
            foreach(var c in cases)
            {
                inputs.Clear();observations.Clear();
                try { await c.Run();Console.WriteLine("PASS "+c.Name);outcomes.Add(new { c.Name,c.Authority,passed=true,inputs=inputs.ToArray(),observations=observations.ToArray() }); }
                catch(Exception e) {failed++;Console.WriteLine($"FAIL {c.Name}: {e}");outcomes.Add(new { c.Name,c.Authority,passed=false,error=e.ToString(),inputs=inputs.ToArray(),observations=observations.ToArray() });}
            }
            var paths=new[]{"reconstructed/CircleGame/Gameplay/Beatmap.cs","reconstructed/CircleGame/Gameplay/CircleSession.cs","reconstructed/CircleGame/Gameplay/CircleHitSounds.cs","reconstructed/CircleGame/Platform/LocalHitSoundAssets.cs","reconstructed/CircleGame/Platform/WindowsCircleEffects.cs","reconstructed/CircleGame/Program.cs","reconstructed/CircleGame/Rendering/PlayfieldView.cs","reconstructed/GenerateIndependentHitSounds.ps1","re/evidence/37/circle-hitsound.json","re/evidence/38/hitsound-resources.json"};
            var manifest=paths.Concat(Directory.GetFiles(Path.Combine(root,"re/qa/qa09/Headless"),"*.cs").Select(p=>Path.GetRelativePath(root,p))).ToDictionary(p=>p.Replace('\\','/'),p=>Hash(File.ReadAllBytes(Path.Combine(root,p))));
            byte[] data=JsonSerializer.SerializeToUtf8Bytes(new { schema="QA09.v1",runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,os=System.Runtime.InteropServices.RuntimeInformation.OSDescription,process=System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),sourceSha=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-preflight",passed=cases.Count-failed,failed,executed=cases.Count,sourceHashes=manifest,cases=outcomes,limits="Finite linked components only. No game/MediaPlayer/native/listening execution. WPF lifecycle binding source-inspected, not dynamically executed." },jsonOptions);
            string output=args.Length>3?Path.GetFullPath(args[3]):Path.Combine(scratch,"results.json");File.WriteAllBytes(output,data);
            Console.WriteLine($"QA09 HITSOUND TESTS: {cases.Count-failed} passed, {failed} failed, {cases.Count} executed");
            Console.WriteLine("QA09 RESULTS SHA256 "+Hash(data));
            // Chunked compressed evidence survives CI line limits; no audio bytes in it.
            using var memory=new MemoryStream();using(var zip=new GZipStream(memory,CompressionLevel.SmallestSize,true))zip.Write(data);
            string encoded=Convert.ToBase64String(memory.ToArray());int n=(encoded.Length+5999)/6000;
            for(int i=0;i<n;i++)Console.WriteLine($"QA09 EVIDENCE {i+1}/{n} "+encoded.Substring(i*6000,Math.Min(6000,encoded.Length-i*6000)));
            return failed==0?0:1;
        }
        finally
        {
            string temp=Path.GetFullPath(Path.GetTempPath());string target=Path.GetFullPath(scratch);
            if(!target.StartsWith(temp,StringComparison.OrdinalIgnoreCase)||!Path.GetFileName(target).StartsWith("ohno-qa09-",StringComparison.Ordinal))throw new Exception("Refuse cleanup outside own temporary directory");
            Directory.Delete(target,true);
        }
    }

    sealed class Files(string tag,List<string> trace):ISampleFiles
    {
        public Dictionary<string,byte[]> Data {get;}=new(StringComparer.Ordinal);
        public HashSet<string> ExistsOnly {get;}=new(StringComparer.Ordinal);
        public bool Exists(string name) {trace.Add(tag+" exists "+name);return Data.ContainsKey(name)||ExistsOnly.Contains(name);}
        public byte[]? Read(string name) {trace.Add(tag+" read "+name);return Data.GetValueOrDefault(name);}
    }
    sealed class Defaults(List<string> trace):IDefaultSamples
    {
        public Dictionary<string,byte[]> U {get;}=new();
        public Dictionary<string,byte[]> G {get;}=new();
        public bool SupplyAll {get;set;}
        public byte[]? Ui(string key) {trace.Add("ui "+key);return U.GetValueOrDefault(key);}
        public byte[]? Gameplay(string key) {trace.Add("game "+key);return G.GetValueOrDefault(key)??(SupplyAll?Bytes():null);}
    }
    sealed class Loader:ISampleLoader
    {
        public List<SampleBytes> Loaded {get;}=[];public List<int> Freed {get;}=[];
        public Func<SampleBytes,int>? Result {get;set;}
        public int Load(SampleBytes sample){Loaded.Add(sample);return Result?.Invoke(sample)??(100+Loaded.Count);}
        public void Free(int handle)=>Freed.Add(handle);
    }
    sealed class Sink:ISamplePlaybackSink
    { public List<SamplePlaybackRequest> Requests {get;}=[];public void Enqueue(SamplePlaybackRequest request)=>Requests.Add(request); }
    sealed class ResourcesFixture
    {
        public List<string> Trace {get;}=[];public Files Map {get;}public Files Skin {get;}public Defaults Defaults {get;}public Loader Loader {get;}=new();public CircleSampleResources R {get;}
        public ResourcesFixture(int format=14,bool suppressed=false,bool early=false,bool hasMap=true,bool hasSkin=true,string name="Default",bool skinSamples=true,SampleCaches? caches=null)
        {
            Map=new("map",Trace);Skin=new("skin",Trace);Defaults=new(Trace);
            R=new(Map,Skin,Defaults,Loader,new(format,suppressed,early,hasMap,hasSkin,name,skinSamples,CultureInfo.InvariantCulture),caches);
            Obs(new ResourceObservation(Trace,Loader));
        }
        public void PrepareDefaults(){Defaults.SupplyAll=true;R.Prepare([],[]);Trace.Clear();Loader.Loaded.Clear();}
    }
    sealed class ResourceObservation(List<string> trace,Loader loader)
    {
        public List<string> Trace=>trace;
        public object Loads=>loader.Loaded.Select(s=>new{s.Name,s.Origin,s.Mode,bytes=s.Bytes.Length,sha256=Hash(s.Bytes)}).ToArray();
        public List<int> Freed=>loader.Freed;
    }
    static Sink Requests(CircleSampleMetadata meta,int custom=-1,CircleSoundPolicy? policy=null,int timingVolume=100,ResourcesFixture? f=null)
    {
        f??=new();if(f.R.Caches.Embedded.Count==0)f.PrepareDefaults();var sink=new Sink();Obs(sink.Requests);
        CircleSampleSelection.Request(meta,[Point(volume:timingVolume)],custom,f.R,policy??Policy(),sink);return sink;
    }
}
