using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

internal sealed class Evidence(string label)
{
    private readonly List<object> cases=[];
    public readonly List<object> Charts=[];
    private int failures;
    private List<object> notes=[];
    public static readonly JsonSerializerOptions JsonOptions=new(){WriteIndented=false,IncludeFields=true};
    public void Check(bool value,string observation,object? expected=null,object? actual=null)
    {
        notes.Add(new{observation,expected,actual,passed=value});
        if(!value)throw new InvalidOperationException(observation+": expected "+JsonSerializer.Serialize(expected)+" actual "+JsonSerializer.Serialize(actual));
    }
    public async Task Run(string id,Func<Task> body,string classification="EXECUTED COMPONENT")
    {
        notes=[];string? error=null;
        try{await body().WaitAsync(TimeSpan.FromSeconds(30));}catch(Exception e){error=e.ToString();failures++;}
        cases.Add(new{id,classification,passed=error is null,observations=notes.ToArray(),error});
        Console.WriteLine($"{(error is null?"PASS":"FAIL")} {label} {id}"+(error is null?"":" "+error));
    }
    public Task Run(string id,Action body,string classification="EXECUTED COMPONENT")=>Run(id,()=>{body();return Task.CompletedTask;},classification);
    public int Finish(string repo,string path)
    {
        string source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"LOCAL UNCOMMITTED";
        var sources=Directory.GetFiles(Path.Combine(repo,"reconstructed/CircleGame"),"*.cs",SearchOption.AllDirectories).Where(p=>!p.Contains(Path.DirectorySeparatorChar+"obj"+Path.DirectorySeparatorChar));
        var manifest=sources.Concat(Directory.GetFiles(Path.Combine(repo,"re/qa/qa15"),"*.cs",SearchOption.AllDirectories).Where(p=>!p.Contains(Path.DirectorySeparatorChar+"obj"+Path.DirectorySeparatorChar)))
            .ToDictionary(p=>Path.GetRelativePath(repo,p).Replace('\\','/'),p=>Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(File.ReadAllText(p).Replace("\r\n","\n")))).ToLowerInvariant());
        byte[] json=JsonSerializer.SerializeToUtf8Bytes(new{schema="QA15.executed.v1",source,os=System.Runtime.InteropServices.RuntimeInformation.OSDescription,runtime=Environment.Version.ToString(),named=cases.Count,passed=cases.Count-failures,failed=failures,cases,charts=Charts,manifest,humanGate="PENDING HUMAN TEST",originalExecuted=false},JsonOptions);
        if(path.Length>0)File.WriteAllBytes(path,json);
        using var packed=new MemoryStream();using(var gzip=new GZipStream(packed,CompressionLevel.SmallestSize,true))gzip.Write(json);
        Console.WriteLine(label+" EVIDENCE "+Convert.ToBase64String(packed.ToArray()));
        Console.WriteLine($"{label}: {cases.Count-failures} passed, {failures} failed");return failures==0?0:1;
    }
}
