using System.Globalization;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;

namespace Ohno.QA14;
public static partial class Tests
{
    sealed record Control(string Id,string Authority,Func<Task> Body,string Classification);
    static readonly List<Control> controls=[];
    static readonly List<object> observations=[],results=[];
    static string root="",scratch="",work="";
    static void Add(string id,string authority,Action body,string classification="EXECUTED COMPONENT") => controls.Add(new(id,authority,()=>{body();return Task.CompletedTask;},classification));
    static void Async(string id,string authority,Func<Task> body) => controls.Add(new(id,authority,body,"EXECUTED COMPONENT"));
    static void Eq<T>(T actual,T expected,string label="") {observations.Add(new{label,expected,actual});if(!EqualityComparer<T>.Default.Equals(actual,expected))throw new Exception(label+" expected="+expected+" actual="+actual);}
    static void Yes(bool actual,string label)=>Eq(actual,true,label);
    static void Same(object? actual,object? expected,string label="reference identity")=>Eq(ReferenceEquals(actual,expected),true,label);
    static void Different(object? actual,object? expected)=>Eq(ReferenceEquals(actual,expected),false,"distinct reference");
    static void Seq<T>(IEnumerable<T> actual,IEnumerable<T> expected,string label="sequence") {var a=actual.ToArray();var e=expected.ToArray();observations.Add(new{label,expected=e,actual=a});if(!a.SequenceEqual(e))throw new Exception(label+" differs");}
    static void Throws<T>(Action body) where T:Exception {try{body();}catch(T e){observations.Add(new{expected=typeof(T).Name,actual=e.GetType().Name,e.Message});return;}throw new Exception("Expected "+typeof(T).Name);}
    static string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
    static string Read(string p)=>File.ReadAllText(Path.Combine(root,p));
    const string Circle="173,141,1700,1,2,0:0:0:0:";
    static string Text(string rows=Circle,string metadata="Title:Independent QA14\nArtist:Finite artist\nCreator:Work1\nVersion:Control",string timing="0,600,4,1,1,55,1,0",string general="Mode:0",int format=14,string difficulty="HPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1") => $"osu file format v{format}\n[General]\n{general}\n[Metadata]\n{metadata}\n[Difficulty]\n{difficulty}\n[TimingPoints]\n{timing}\n[HitObjects]\n{rows}\n";
    static string Put(string relative="song/chart.osu",string? text=null,Encoding? encoding=null)
    {
        string p=Path.Combine(work,relative);Directory.CreateDirectory(Path.GetDirectoryName(p)!);text??=Text();encoding??=new UTF8Encoding(false);File.WriteAllText(p,text,encoding);
        byte[] bytes=File.ReadAllBytes(p);observations.Add(new{fixture=relative,sha256=Hash(bytes),length=bytes.Length,encoding=encoding.WebName});
        if(bytes.Length<=16384){string saved=Path.Combine(root,"re/qa/qa14/Inputs",Hash(bytes)+".osu");Directory.CreateDirectory(Path.GetDirectoryName(saved)!);if(!File.Exists(saved))File.WriteAllBytes(saved,bytes);}
        return p;
    }
    static CatalogChart Chart(string name="song/chart.osu",string? text=null)=>LocalChartCatalog.Index(Put(name,text));
    static CatalogSnapshot Snap(long n,params CatalogChart[] charts)=>new(n,charts,[]);
    static SelectPlayModel Browser(params CatalogChart[] charts){var m=new SelectPlayModel(new());m.Rebuild(Snap(1,charts));return m;}
    static PlayEntryFlow Pending(SelectedChartState s){var f=new PlayEntryFlow(s);Eq(f.Request(),StartDecision.Accepted);Yes(f.Accept(),"accept");Yes(f.Queue(),"queue");return f;}
    static PlayEntryFlow Loading(SelectedChartState s){var f=Pending(s);Yes(f.Commit(),"commit");Yes(f.Capture(),"capture");f.BeginLoading();return f;}
    static TaskCompletionSource<bool> Barrier()=>new(TaskCreationOptions.RunContinuationsAsynchronously);
    static async Task Wait(Task t)=>await t.WaitAsync(TimeSpan.FromSeconds(30));
    public static async Task<int> Main(string[] args)
    {
        root=Path.GetFullPath(args[0]);CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;CultureInfo.CurrentUICulture=CultureInfo.InvariantCulture;
        scratch=Path.Combine(Path.GetTempPath(),"ohno-qa14-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        Catalog();Identity();Grouping();Selection();Entry();Races();Metadata();Stars();Sessions();Audits();
        int failed=0;
        foreach(var c in controls)
        {
            observations.Clear();work=Path.Combine(scratch,c.Id.Replace('/','_'));Directory.CreateDirectory(work);string? error=null;
            try{await c.Body().WaitAsync(TimeSpan.FromMinutes(3));}catch(Exception e){error=e.ToString();failed++;}
            results.Add(new{id=c.Id,authority=c.Authority,classification=c.Classification,passed=error is null,observations=observations.ToArray(),error});
            Console.WriteLine($"{(error is null?"PASS":"FAIL")} QA14 {c.Id}"+(error is null?"":" "+error));
        }
        var sources=Directory.GetFiles(Path.Combine(root,"reconstructed/CircleGame"),"*.cs",SearchOption.AllDirectories).Where(p=>!p.Contains(Path.DirectorySeparatorChar+"obj"+Path.DirectorySeparatorChar)&&!p.Contains(Path.DirectorySeparatorChar+"bin"+Path.DirectorySeparatorChar))
            .Concat(Directory.GetFiles(Path.Combine(root,"re/qa/qa14/Headless"),"*.cs"));
        var manifest=sources.ToDictionary(p=>Path.GetRelativePath(root,p).Replace('\\','/'),p=>Hash(File.ReadAllBytes(p)));
        byte[] data=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA14.v1",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-preflight",production="19bb644e01c646ca8850f0751ec03f7ef881d766",runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            named=controls.Count,passed=controls.Count-failed,failed,classifications=controls.GroupBy(c=>c.Classification).ToDictionary(g=>g.Key,g=>g.Count()),cases=results,manifest},new JsonSerializerOptions{WriteIndented=true});
        File.WriteAllBytes(args[1],data);Console.WriteLine($"QA14 HEADLESS: {controls.Count-failed} passed, {failed} failed, {controls.Count} named controls");
        Console.WriteLine("QA14 SHA256 "+Hash(data));using var ms=new MemoryStream();using(var gz=new GZipStream(ms,CompressionLevel.SmallestSize,true))gz.Write(data);Console.WriteLine("QA14 EVIDENCE "+Convert.ToBase64String(ms.ToArray()));
        // Owned isolated test tree only. No original/reconstruction fixture deletion.
        if(!Path.GetFullPath(scratch).StartsWith(Path.GetFullPath(Path.GetTempPath()),StringComparison.OrdinalIgnoreCase))throw new Exception("Unsafe scratch cleanup");
        Directory.Delete(scratch,true);return failed==0?0:1;
    }
}
