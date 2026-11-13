using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Difficulty;
using Ohno.CircleGame.Selection;

internal static class Program
{
    static string Git(string root, params string[] args)
    {
        var start=new ProcessStartInfo("git") { WorkingDirectory=root, RedirectStandardOutput=true };
        foreach(string arg in args)start.ArgumentList.Add(arg);
        using var p=Process.Start(start)!; string text=p.StandardOutput.ReadToEnd();p.WaitForExit();
        if(p.ExitCode!=0)throw new Exception("git provenance failed");return text.Trim();
    }
    static string Hash(string path)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
    static Dictionary<string,object?> Project(Beatmap map)=>new()
    {
        ["Circles"]=map.Circles.Count,["Sliders"]=map.Sliders.Count,["Spinners"]=map.Spinners.Count,
        ["CircleSize"]=map.CircleSize,["OverallDifficulty"]=map.OverallDifficulty,["ApproachRate"]=map.ApproachRate
    };
    static bool Equal(JsonElement expected,JsonElement actual)=>expected.ValueKind==JsonValueKind.Number&&actual.ValueKind==JsonValueKind.Number
        ? expected.GetDouble()==actual.GetDouble():expected.ToString()==actual.ToString();
    static int Main(string[] args)
    {
        string root=Path.GetFullPath(args[0]),folder=Path.Combine(root,"re/campaigns/C001"),output=Path.GetFullPath(args[1]);
        bool audit=args.Contains("--audit");var results=new List<object>();int checks=0,matches=0,gaps=0,probes=0;
        using var manifest=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(folder,"corpus/manifest.json")));
        foreach(var row in manifest.RootElement.GetProperty("cases").EnumerateArray())
        {
            string id=row.GetProperty("id").GetString()!,path=Path.Combine(folder,"corpus",row.GetProperty("path").GetString()!);
            if(Hash(path)!=row.GetProperty("sha256").GetString())throw new Exception("Fixture bytes changed: "+id);
            var catalog=LocalChartCatalog.Index(path);JsonElement actualMeta=JsonSerializer.SerializeToElement(catalog.Metadata);
            JsonElement? actualPrepared=null;string? gameplayError=null,starError=null;
            try { actualPrepared=JsonSerializer.SerializeToElement(Project(Beatmap.LoadCanonical(path))); }
            catch(Exception e)when(e is InvalidDataException or FormatException or OverflowException or ArgumentException) {gameplayError=e.GetType().Name+": "+e.Message;}
            try { StarAdmission.Read(catalog); }
            catch(Exception e)when(e is InvalidDataException or UnsupportedStarProfileException or FormatException or ArgumentException) {starError=e.GetType().Name+": "+e.Message;}
            var mismatches=new List<object>();
            void Compare(string stage,JsonElement expect,JsonElement? actual)
            {
                foreach(var prop in expect.EnumerateObject())
                {
                    checks++;bool found=actual is {ValueKind:JsonValueKind.Object} x&&x.TryGetProperty(prop.Name,out _);
                    JsonElement? got=found?actual!.Value.GetProperty(prop.Name):null;
                    if(got is {} v&&Equal(prop.Value,v))matches++;
                    else {gaps++;mismatches.Add(new{stage,field=prop.Name,expected=prop.Value,actual=got});}
                }
            }
            Compare("metadata",row.GetProperty("expected_metadata"),actualMeta);
            var expectedPrepared=row.GetProperty("expected_prepared");
            if(expectedPrepared.ValueKind==JsonValueKind.Object)Compare("prepared",expectedPrepared,actualPrepared);
            if(!row.GetProperty("expected_metadata").EnumerateObject().Any()&&expectedPrepared.ValueKind==JsonValueKind.Null)probes++;
            results.Add(new{id,sha256=Hash(path),catalogStatus=catalog.Availability.ToString(),catalogError=catalog.Error,
                metadata=actualMeta,prepared=actualPrepared,gameplayError,starError,mismatches,originalRuntime="NOT RUN"});
            Console.WriteLine($"C001 {id}: {mismatches.Count} original-contract gaps; gameplay={(gameplayError is null?"loaded":"rejected")}");
        }
        // Real revision and cancellation controls, independent of original semantic expectations.
        string temp=Path.Combine(Path.GetTempPath(),"c001-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        int engineering=0;
        try
        {
            string source=Path.Combine(folder,"corpus/fixtures/control.osu"),target=Path.Combine(temp,"revision.osu");File.Copy(source,target);
            var before=LocalChartCatalog.Index(target);var stamp=File.GetLastWriteTimeUtc(target);long size=new FileInfo(target).Length;
            File.WriteAllText(target,File.ReadAllText(target).Replace("Title:C001 control","Title:C001 changed"));File.SetLastWriteTimeUtc(target,stamp);
            if(new FileInfo(target).Length!=size)throw new Exception("revision control must preserve size");
            bool refused=false;try {before.LoadGameplay();}catch(InvalidDataException){refused=true;}
            if(!refused)throw new Exception("stale chart accepted");engineering++;
            var after=LocalChartCatalog.Index(target);if(after.Fingerprint==before.Fingerprint||after.LoadGameplay().Title!="C001 changed")throw new Exception("fresh revision not bound");engineering++;
            using var cancelled=new CancellationTokenSource();cancelled.Cancel();refused=false;
            try {LocalChartCatalog.Index(target,token:cancelled.Token);}catch(OperationCanceledException){refused=true;}
            if(!refused)throw new Exception("catalog cancellation ignored");engineering++;
            refused=false;try {StarAdmission.Read(after,cancelled.Token);}catch(OperationCanceledException){refused=true;}
            if(!refused)throw new Exception("star cancellation ignored");engineering++;
        }
        finally {Directory.Delete(temp,true);}
        var paths=new[]{"reconstructed/CircleGame/Gameplay/Beatmap.cs","reconstructed/CircleGame/Selection/LocalChartCatalog.cs","reconstructed/CircleGame/Difficulty/CircleDifficultyPreparation.cs"};
        var packet=new {campaign="C001",source=Git(root,"log","-1","--format=%H","--","reconstructed/CircleGame"),production=paths.ToDictionary(x=>x,x=>Hash(Path.Combine(root,x))),
            runtime=Environment.Version.ToString(),os=System.Runtime.InteropServices.RuntimeInformation.OSDescription,
            originalExecuted=false,humanQualification="PENDING",mode=audit?"DIAGNOSTIC AUDIT - NOT CONFORMANCE PASS":"ORIGINAL CONTRACT CONFORMANCE",
            fixtures=results.Count,diagnosticOnly=probes,checks,matches,gaps,engineeringControls=engineering,results};
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);string encoded=JsonSerializer.Serialize(packet);File.WriteAllText(output,encoded+"\n");
        Console.WriteLine("C001 RESULTS "+encoded);
        Console.WriteLine($"C001: {matches}/{checks} original-contract fields match; {gaps} gaps; {engineering}/4 engineering controls PASS; {probes} diagnostic-only probes; original runtime NOT RUN");
        return gaps==0||audit?0:1;
    }
}
