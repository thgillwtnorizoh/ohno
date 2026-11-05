// Independent QA11: published E040 operands, actual linked production components.
// No original binary, game window, physical input, native decoder or audio output.
using System.Globalization;
using System.IO.Compression;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
namespace Ohno.QA11;

public static partial class Tests
{
    sealed record Case(string Name,string Authority,string Classification,Func<Task> Run);
    static readonly List<Case> cases=[];
    static readonly List<object> outcomes=[],inputs=[],observations=[];
    static string root="",scratch="",packA="",packB="";
    static int serial;
    const string DefaultTiming="0,600,4,1,1,100,1,0";
    static void Add(string name,string authority,Action run,string classification="EXECUTED COMPONENT") =>
        cases.Add(new(name,authority,classification,()=>{run();return Task.CompletedTask;}));
    static void Async(string name,string authority,Func<Task> run)=>cases.Add(new(name,authority,"EXECUTED COMPONENT",run));
    static void Obs(object value)=>observations.Add(value);
    static void Eq<T>(T actual,T expected){Obs(new{expected,actual});if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception($"expected={expected}; actual={actual}");}
    static void Seq<T>(IEnumerable<T> actual,params T[] expected){var a=actual.ToArray();Obs(new{expected,actual=a});if(!a.SequenceEqual(expected))throw new Exception($"expected=[{string.Join(',',expected)}]; actual=[{string.Join(',',a)}]");}
    static void Yes(bool actual,string reason){Obs(new{expected=true,actual,reason});if(!actual)throw new Exception(reason);}
    static void Near(double actual,double expected,double tolerance=1e-7){Obs(new{expected,actual,tolerance});if(!double.IsFinite(actual)||Math.Abs(actual-expected)>tolerance)throw new Exception($"expected={expected:R}; actual={actual:R}");}
    static void Throws<T>(Action action)where T:Exception{try{action();}catch(T e){Obs(new{rejection=typeof(T).Name,e.Message});return;}throw new Exception("expected "+typeof(T).Name);}
    static async Task ThrowsAsync<T>(Func<Task> action)where T:Exception{try{await action();}catch(T e){Obs(new{rejection=typeof(T).Name,e.Message});return;}throw new Exception("expected "+typeof(T).Name);}
    static string Hash(byte[] data)=>Convert.ToHexString(SHA256.HashData(data)).ToLowerInvariant();
    static string Dir(string label){string p=Path.Combine(scratch,$"{serial++:D4}-{label}");Directory.CreateDirectory(p);return p;}
    static Beatmap Map(string rows,string timing=DefaultTiming,int version=14,string events="",string? directory=null,int hp=0)
    {
        string text=$"osu file format v{version}\n[General]\nMode:0\n[Metadata]\nTitle:QA11 independent input\n[Difficulty]\nHPDrainRate:{hp}\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n{timing}\n[Events]\n{events}\n[HitObjects]\n{rows}\n";
        string file=Path.Combine(directory??Dir("map"),"input.osu");File.WriteAllText(file,text,new UTF8Encoding(false));
        inputs.Add(new{name=$"input-{inputs.Count:D3}.osu",text,sha256=Hash(File.ReadAllBytes(file))});return Beatmap.Load(file);
    }
    static string Row(int spans=1,int mask=0,string? sounds=null,string? sets=null,string? payload=null,int start=6000,int length=240)
    {
        string s=$"96,192,{start},2,{mask},L|{96+length}:192,{spans},{length}";
        if(sounds is not null||sets is not null||payload is not null)s+=","+(sounds??"");
        if(sets is not null||payload is not null)s+=","+(sets??"");
        if(payload is not null)s+=","+payload;return s;
    }
    static Beatmap SM(int spans=1,int mask=0,string? sounds=null,string? sets=null,string? payload=null,int version=14,string timing=DefaultTiming,int length=240)=>
        Map(Row(spans,mask,sounds,sets,payload,length:length),timing,version);
    static SliderSoundMetadata Data(SliderDefinition s)=>s.Samples??throw new Exception("Missing parser sample metadata");
    static CircleSession Fresh(Beatmap m,bool hp=false)=>CircleSession.CreateFreshOrdinary(m,ScoreConfiguration.FromBeatmap(m),hp);
    static CircleSoundPolicy Policy(bool layered=false)=>new(true,layered,true,0,100,false,100,100);
    static SampleTiming Point(int set=1,int index=1,int volume=100,double time=0)=>new(time,set,index,volume,true,0);
    static SliderState Pre(SliderDefinition s){var state=new SliderState(s);state.Audio.Advance(s,3999,false,1);return state;}
    static SliderAudioSelection? Audio(SliderState s,int time,bool tracking=true,int numeric=1)
    {
        int before=s.Audio.Index;var e=s.Audio.Advance(s.Definition,time,tracking,numeric);
        Obs(new{time,tracking,numeric,before,after=s.Audio.Index,gameplay=s.ProcessedCheckpoints,success=s.SuccessfulCheckpoints,failure=s.FailedCheckpoints,selected=e});return e;
    }
    static void Events(List<SliderSoundEvent> events,params string[] expected)=>Seq(events.Select(e=>$"{e.Kind}:{e.Index}:{e.ScheduleIndex}"),expected);
    static object EventView(SliderSoundEvent e)=>new{kind=e.Kind.ToString(),e.Index,e.ScheduleIndex,e.Time,e.PresentationX,parentStart=e.Parent.Head.StartTime};
    static object RequestView(SamplePlaybackRequest r)=>new{kind=r.Kind.ToString(),r.Handle,r.Layer,r.Volume,r.Pan,r.Factor,r.Gain,sourceStart=r.Source.StartTime,identity=r.Event is null?null:new{r.Event.Index,r.Event.ScheduleIndex,r.Event.UpdateTime,parentStart=r.Event.Parent.Head.StartTime}};
    static byte[] Bytes(int marker=1,int count=1536){byte[] b=new byte[count];b[0]=(byte)marker;return b;}
    static readonly JsonSerializerOptions options=new(){IncludeFields=true};
    public static async Task<int> Main(string[] args)
    {
        root=Path.GetFullPath(args[0]);packA=Path.GetFullPath(args[1]);packB=Path.GetFullPath(args[2]);
        CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;CultureInfo.CurrentUICulture=CultureInfo.InvariantCulture;
        scratch=Path.Combine(Path.GetTempPath(),"ohno-qa11-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        try
        {
            TimingCases();ChannelCases();ResourceCases();SessionCases();AssetCases();Bindings();
            if(cases.Select(c=>c.Name).Distinct().Count()!=cases.Count)throw new Exception("Duplicate case names");
            int failed=0;
            foreach(var c in cases)
            {
                inputs.Clear();observations.Clear();
                try{await c.Run();outcomes.Add(new{c.Name,c.Authority,c.Classification,passed=true,inputs=inputs.ToArray(),observations=observations.ToArray()});Console.WriteLine("PASS "+c.Name);}
                catch(Exception e){failed++;outcomes.Add(new{c.Name,c.Authority,c.Classification,passed=false,error=e.ToString(),inputs=inputs.ToArray(),observations=observations.ToArray()});Console.WriteLine("FAIL "+c.Name+": "+e);}
            }
            string[] paths=["reconstructed/CircleGame/Gameplay/Beatmap.cs","reconstructed/CircleGame/Gameplay/CircleSession.cs","reconstructed/CircleGame/Gameplay/CircleHitSounds.cs","reconstructed/CircleGame/Platform/LocalHitSoundAssets.cs","reconstructed/CircleGame/Platform/WindowsCircleEffects.cs","reconstructed/CircleGame/Rendering/PlayfieldView.cs","reconstructed/CircleGame/Program.cs","reconstructed/GenerateIndependentSliderTicks.ps1","reconstructed/GenerateIndependentHitSounds.ps1","re/evidence/39/slider-hitsounds.json","re/evidence/37/circle-hitsound.json","re/evidence/38/hitsound-resources.json","reconstructed/R13DevelopmentChecks/verification/initial-r13-failure.log","reconstructed/R13DevelopmentChecks/verification/windows-provenance.json","reconstructed/CircleGame/Platform/WindowsSliderLoops.cs","reconstructed/GenerateIndependentSliderLoops.ps1","re/evidence/40/slider-continuous-audio.json"];
            var hashes=paths.Concat(Directory.GetFiles(Path.Combine(root,"re/qa/qa11/Headless"),"*.cs").Select(p=>Path.GetRelativePath(root,p))).ToDictionary(p=>p.Replace('\\','/'),p=>Hash(File.ReadAllBytes(Path.Combine(root,p))));
            byte[] data=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA11.v1",sourceSha=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-preflight",productionBaseline="8bd0433528cbceb0bffcaa4bf29119776deaf0e9",runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,os=System.Runtime.InteropServices.RuntimeInformation.OSDescription,executed=cases.Count,passed=cases.Count-failed,failed,sourceInspected=cases.Count(c=>c.Classification=="SOURCE INSPECTION"),assetControls=cases.Count(c=>c.Classification=="ASSET CONTROL"),sourceHashes=hashes,cases=outcomes,limits="Actual linked components; artificial operands labelled. No original binary/input/playback/listening. WPF media paths source-only; empty Windows checks separate. Conditional E040 coverage is not original runtime."},options);
            File.WriteAllBytes(args[3],data);Console.WriteLine($"QA11 CONTINUOUS TESTS: {cases.Count-failed} passed, {failed} failed, {cases.Count} executed");Console.WriteLine("QA11 RESULTS SHA256 "+Hash(data));
            using var memory=new MemoryStream();using(var gzip=new GZipStream(memory,CompressionLevel.SmallestSize,true))gzip.Write(data);
            string b64=Convert.ToBase64String(memory.ToArray());int count=(b64.Length+5999)/6000;
            for(int i=0;i<count;i++)Console.WriteLine($"QA11 EVIDENCE {i+1}/{count} "+b64.Substring(i*6000,Math.Min(6000,b64.Length-i*6000)));
            return failed==0?0:1;
        }
        finally
        {
            string temp=Path.GetFullPath(Path.GetTempPath());string target=Path.GetFullPath(scratch);
            if(!target.StartsWith(temp,StringComparison.OrdinalIgnoreCase)||!Path.GetFileName(target).StartsWith("ohno-qa11-",StringComparison.Ordinal))throw new Exception("Refuse cleanup outside own temporary directory");
            Directory.Delete(target,true);
        }
    }
    sealed class Files(string label,List<string> trace):ISampleFiles
    {
        public Dictionary<string,byte[]> Data{get;}=new(StringComparer.Ordinal);
        public HashSet<string> Existing{get;}=new(StringComparer.Ordinal);
        public bool Exists(string n){trace.Add(label+" exists "+n);return Data.ContainsKey(n)||Existing.Contains(n);}
        public byte[]? Read(string n){trace.Add(label+" read "+n);return Data.GetValueOrDefault(n);}
    }
    sealed class Defaults(List<string> trace):IDefaultSamples
    {
        public bool All{get;set;}public Dictionary<string,byte[]> U{get;}=[];public Dictionary<string,byte[]> G{get;}=[];
        public byte[]? Ui(string n){trace.Add("ui "+n);return U.GetValueOrDefault(n);}
        public byte[]? Gameplay(string n){trace.Add("default "+n);return G.GetValueOrDefault(n)??(All?Bytes(33):null);}
    }
    sealed class Loader:ISampleLoader
    {
        public List<SampleBytes> Loaded{get;}=[];public List<int> Freed{get;}=[];public Func<SampleBytes,int>? Return{get;set;}
        public int Load(SampleBytes b){Loaded.Add(b);return Return?.Invoke(b)??(1000+Loaded.Count);}
        public void Free(int h)=>Freed.Add(h);
    }
    sealed class Sink:ISamplePlaybackSink
    {
        public List<SamplePlaybackRequest> Requests{get;}=[];public Action<SamplePlaybackRequest>? Before{get;set;}
        public void Enqueue(SamplePlaybackRequest r){Before?.Invoke(r);Requests.Add(r);Obs(RequestView(r));}
    }
    sealed class RF
    {
        public List<string> Trace{get;}=[];public Files Map{get;}public Files Skin{get;}public Defaults Defaults{get;}public Loader Loader{get;}=new();public CircleSampleResources R{get;}
        public RF(bool all=false,bool suppressed=false,bool hasMap=true,string skinName="Default",bool skinSamples=true,bool early=false)
        {Map=new("map",Trace);Skin=new("skin",Trace);Defaults=new(Trace){All=all};R=new(Map,Skin,Defaults,Loader,new(14,suppressed,early,hasMap,true,skinName,skinSamples,CultureInfo.InvariantCulture));}
        public void Prepare(){R.Prepare([],[],true);Trace.Clear();}
        public void Record()=>Obs(new{trace=Trace.ToArray(),loads=Loader.Loaded.Select(b=>new{b.Name,b.Origin,b.Mode,b.Loop,bytes=b.Bytes.Length,sha256=Hash(b.Bytes)}).ToArray(),freed=Loader.Freed.ToArray()});
    }
    static Sink Request(SliderDefinition parent,SliderSoundKind kind,int index=0,int scheduleIndex=0,float x=256,IReadOnlyList<SampleTiming>? timing=null,RF? fixture=null,CircleSoundPolicy? policy=null,int custom=-1,Action<SelectedCircleSample>? observer=null)
    {
        fixture??=new(true);if(fixture.R.Caches.Embedded.Count==0)fixture.Prepare();timing??=[Point()];policy??=Policy();
        var prepared=PreparedSliderSamples.Create(parent,timing,policy,custom);var sink=new Sink();
        SliderSampleSelection.Request(new(parent,kind,index,scheduleIndex,9999,x),prepared,timing,fixture.R,policy,sink,observer);fixture.Record();return sink;
    }
}
