using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using System.IO.Compression;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA13;
public static partial class Tests
{
    sealed record Control(string Id,string Authority,string Classification,Action Body);
    static readonly List<Control> controls=[];
    static readonly List<object> observations=[],results=[];
    static string root="",scratch="",fixture="";
    static void Add(string id,string authority,Action body,string classification="EXECUTED COMPONENT")=>controls.Add(new(id,authority,classification,body));
    static void Eq<T>(T actual,T expected){observations.Add(new{expected,actual});if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception($"expected {expected}, actual {actual}");}
    static void Yes(bool actual,string label){observations.Add(new{expected=true,actual,label});if(!actual)throw new Exception(label);}
    static void Seq<T>(IEnumerable<T> actual,IEnumerable<T> expected){var a=actual.ToArray();var e=expected.ToArray();observations.Add(new{expected=e,actual=a});if(!a.SequenceEqual(e))throw new Exception("sequence mismatch");}
    static void Throws<T>(Action body)where T:Exception{try{body();}catch(T e){observations.Add(new{expected=typeof(T).Name,actual=e.GetType().Name,e.Message});return;}throw new Exception("expected "+typeof(T).Name);}
    static string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
    static string Read(string f)=>File.ReadAllText(Path.Combine(root,f));
    // Independent mutable record control, never used as the grade oracle.
    sealed class Record:RankRecord
    {
        public object Identity=new();public RankMapMetadata? Data=new("QA13 title","QA13 difficulty","QA13 artist");
        public int Numeric,Max,Mods;public ushort C300=10,C100,C50,Miss;
        public override object MapIdentity=>Identity;public override RankMapMetadata? Metadata=>Data;
        public override int Score=>Numeric;public override int MaximumCombo=>Max;
        public override ushort N300=>C300;public override ushort N100=>C100;public override ushort N50=>C50;public override ushort NMiss=>Miss;
        public override int Modifiers=>Mods;public void Prepare(int n)=>PreparedCombo=n;
    }
    static RankSession Flow(Record? r=null,Func<bool?>? failed=null,Func<bool>? pending=null)=>new(r??new Record(),1000,10,failed??(()=>false),pending);
    static void Finish(RankSession f){f.Observe(1201);Yes(f.Complete(),"finite completion");}
    static void Enter(RankSession f){Finish(f);Eq(f.RequestRank(),TransitionDecision.Accepted);Yes(f.Commit(()=>{}),"commit");Yes(f.Initialize(1000),"initialize");Yes(f.Present(),"presentation eligibility");}
    static Beatmap Map(string rows="256,192,2000,1,0,0:0:0:0:",string metadata="Title:QA13 chart\nVersion:finite variant\nArtist:Independent",string timing="0,600,4,1,1,55,1,0",int format=14)
    {
        string text=$"osu file format v{format}\n[General]\nMode:0\n[Metadata]\n{metadata}\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n{timing}\n[HitObjects]\n{rows}\n";
        fixture=Hash(Encoding.UTF8.GetBytes(text))+".osu";string p=Path.Combine(scratch,fixture);File.WriteAllText(p,text);
        string saved=Path.Combine(root,"re/qa/qa13/Inputs",fixture);Directory.CreateDirectory(Path.GetDirectoryName(saved)!);if(!File.Exists(saved))File.WriteAllText(saved,text);
        return Beatmap.Load(p);
    }
    public static int Main(string[] args)
    {
        root=Path.GetFullPath(args[0]);CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
        scratch=Path.Combine(Path.GetTempPath(),"ohno-qa13-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        Grades();Records();Policies();Completion();Transitions();Presentation();Reveal();Sessions();Audits();
        int failed=0;
        foreach(var c in controls){observations.Clear();fixture="";string? error=null;try{c.Body();}catch(Exception e){error=e.ToString();failed++;}results.Add(new{id=c.Id,authority=c.Authority,classification=c.Classification,passed=error is null,fixture,observations=observations.ToArray(),error});Console.WriteLine($"{(error is null?"PASS":"FAIL")} QA13 {c.Id}"+(error is null?"":" "+error));}
        var files=Directory.GetFiles(Path.Combine(root,"reconstructed/CircleGame"),"*.cs",SearchOption.AllDirectories).Where(p=>!p.Contains("\\obj\\")&&!p.Contains("\\bin\\"))
            .Concat(Directory.GetFiles(Path.Combine(root,"re/qa/qa13/Headless"),"*.cs"));
        var manifest=files.ToDictionary(p=>Path.GetRelativePath(root,p).Replace('\\','/'),p=>Hash(File.ReadAllBytes(p)));
        byte[] data=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA13.v1",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-preflight",production="3f9a8f58d29480729d1d305f075791769163f46d",runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            named=controls.Count,passed=controls.Count-failed,failed,classifications=controls.GroupBy(c=>c.Classification).ToDictionary(g=>g.Key,g=>g.Count()),cases=results,manifest},new JsonSerializerOptions{WriteIndented=true});
        File.WriteAllBytes(args[1],data);Console.WriteLine($"QA13 HEADLESS: {controls.Count-failed} passed, {failed} failed, {controls.Count} named controls");
        Console.WriteLine("QA13 SHA256 "+Hash(data));using var ms=new MemoryStream();using(var gz=new GZipStream(ms,CompressionLevel.SmallestSize,true))gz.Write(data);Console.WriteLine("QA13 EVIDENCE "+Convert.ToBase64String(ms.ToArray()));
        Directory.Delete(scratch,true);return failed==0?0:1;
    }
}
