using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;

static class Program
{
    static string Git(string root,params string[] args) {
        var si=new ProcessStartInfo("git"){WorkingDirectory=root,RedirectStandardOutput=true};
        foreach(var arg in args)si.ArgumentList.Add(arg);
        using var p=Process.Start(si)!;var result=p.StandardOutput.ReadToEnd().Trim();p.WaitForExit();
        if(p.ExitCode!=0)throw new Exception("git failed");return result;
    }
    static string Hash(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p))).ToLowerInvariant();
    static int Main(string[] args) {
        string root=Path.GetFullPath(args[0]),dir=Path.Combine(root,"re/campaigns/C001/stacking");
        using var doc=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(dir,"manifest.json")));
        int checks=0,gaps=0,loaded=0;var results=new List<object>();
        // Explicit E045 no-mod viewport operands, independently specified in manifest.
        float radius=32.013118743896484f,step=radius/10f;
        foreach(var test in doc.RootElement.GetProperty("cases").EnumerateArray()) {
            string id=test.GetProperty("id").GetString()!,path=Path.Combine(dir,test.GetProperty("path").GetString()!);
            if(Hash(path)!=test.GetProperty("sha256").GetString())throw new Exception("fixture changed: "+id);
            var observed=new List<object>();int localGaps=0;string? error=null;
            try {
                var map=Beatmap.LoadCanonical(path);loaded++;
                var heads=map.Circles.Select(c=>(c.StartTime,c.Position)).Concat(map.Sliders.Select(s=>(s.Head.StartTime,s.Head.Position)))
                    .Concat(map.Spinners.Select(s=>(StartTime:s.StartTime,Position:new Vector2(256,192)))).OrderBy(v=>v.StartTime).ToArray();
                var raw=test.GetProperty("original_heads").EnumerateArray().ToArray();
                var heights=test.GetProperty("expected_heights").EnumerateArray().Select(x=>x.GetInt32()).ToArray();
                if(heads.Length!=raw.Length)throw new Exception("object count differs");
                for(int i=0;i<raw.Length;i++) {
                    if((int.Parse(test.GetProperty("rows")[i].GetString()!.Split(',')[3])&8)!=0) {
                        observed.Add(new{index=i,skipped="Spinner has no prepared-position field in production API"});continue;
                    }
                    var original=new Vector2(raw[i][0].GetSingle(),raw[i][1].GetSingle());
                    var expected=original-heights[i]*new Vector2(step,step);
                    checks++;bool pass=Vector2.Distance(expected,heads[i].Position)<.00003f;
                    if(!pass){gaps++;localGaps++;}
                    observed.Add(new{index=i,start=heads[i].StartTime,heightPrediction=heights[i],expected=new[]{expected.X,expected.Y},actual=new[]{heads[i].Position.X,heads[i].Position.Y},pass});
                }
            } catch(Exception e)when(e is InvalidDataException or FormatException or OverflowException) {
                error=e.GetType().Name+": "+e.Message;gaps++;localGaps++;
            }
            results.Add(new{id,sha256=Hash(path),error,observed,gaps=localGaps});Console.WriteLine($"{id}: {localGaps} gaps");
        }
        var sourceFiles=new[]{"reconstructed/CircleGame/Gameplay/Beatmap.cs","reconstructed/CircleGame/Gameplay/CircleSession.cs"};
        var packet=new {source=Git(root,"rev-parse","HEAD"),productionSource=Git(root,"log","-1","--format=%H","--","reconstructed/CircleGame"),production=sourceFiles.ToDictionary(p=>p,p=>Hash(Path.Combine(root,p))),runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,os=System.Runtime.InteropServices.RuntimeInformation.OSDescription,fixtures=results.Count,loaded,checks,gaps,radius,step,originalExecuted=false,humanQualification="PENDING",results};
        File.WriteAllText(args[1],JsonSerializer.Serialize(packet,new JsonSerializerOptions{WriteIndented=true})+"\n");
        Console.WriteLine($"C001 stacking actual production: {loaded}/{results.Count} loaded; {checks} head checks; {gaps} gaps");
        return gaps==0||args.Contains("--audit")?0:1;
    }
}
