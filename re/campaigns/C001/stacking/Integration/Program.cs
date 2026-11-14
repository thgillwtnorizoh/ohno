using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;

static class Program
{
    static readonly List<object> Results = new();
    static string Id="";
    static void Check(bool pass,string contract) { Results.Add(new{id=Id,contract,pass}); if(!pass)Console.WriteLine($"FAIL {Id}: {contract}"); }
    static bool Near(Vector2 a,Vector2 b)=>Vector2.Distance(a,b)<.00003f;
    static string Git(string root,params string[] args) {
        var si=new ProcessStartInfo("git"){WorkingDirectory=root,RedirectStandardOutput=true};
        foreach(var a in args)si.ArgumentList.Add(a);using var p=Process.Start(si)!;
        var s=p.StandardOutput.ReadToEnd().Trim();p.WaitForExit();if(p.ExitCode!=0)throw new Exception("git failed");return s;
    }
    static int Main(string[] args) {
        string root=Path.GetFullPath(args[0]),folder=Path.Combine(root,"re/campaigns/C001/stacking");
        using var manifest=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(folder,"manifest.json")));
        foreach(var fixture in manifest.RootElement.GetProperty("cases").EnumerateArray()) {
            Id=fixture.GetProperty("id").GetString()!;
            string path=Path.Combine(folder,fixture.GetProperty("path").GetString()!);
            var bytes=File.ReadAllBytes(path);Check(Convert.ToHexString(SHA256.HashData(bytes)).Equals(fixture.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),"unchanged fixture bytes");
            // Historical adapter supplies the preserved nonstacking geometry. It
            // is a translation control, not an independent original stack oracle.
            var raw=Beatmap.Load(path);var map=Beatmap.LoadCanonical(path);var again=Beatmap.LoadCanonical(path);
            Check(map.Document!.CopySourceBytes()!.SequenceEqual(bytes),"raw source bytes retained");
            Check(map.HealthSource!.IsFreshFor(map),"fresh health owner bound after translation");
            Check(map.ScoreMetadata==raw.ScoreMetadata,"score metadata invariant under stacking");
            var registered=map.Circles.Cast<object>().Concat(map.Sliders).Concat(map.Spinners)
                .OrderBy(o=>o switch {CircleDefinition c=>c.StartTime,SliderDefinition s=>s.Head.StartTime,SpinnerDefinition s=>s.StartTime,_=>0}).ToArray();
            try {
                var health=HealthChart.FromMap(map,1200,HealthPreparationPolicy.E036FreshOrdinary);
                Check(health.NewComboProvenance.Count==registered.Length,"NewCombo count preserved");
                for(int i=0;i<registered.Length;i++)Check(ReferenceEquals(registered[i],health.NewComboProvenance[i].Identity),"NewCombo exact registered identity #"+i);
            } catch(InvalidDataException e) {
                string? baselineError=null;
                try { HealthChart.FromMap(raw,1200,HealthPreparationPolicy.E036FreshOrdinary); }
                catch(InvalidDataException old) { baselineError=old.Message; }
                Check(e.Message==baselineError,"retained health admission limitation: "+e.Message);
            }
            foreach(var c in map.Circles) {
                Check(ReferenceEquals(map.CircleSounds!.Circles.Single(x=>x.Object.StartTime==c.StartTime).Object,c),"circle sound sidecar identity #"+c.StartTime);
                Check(ReferenceEquals(map.CircleSounds!.RegisteredOverrides.Single(x=>x.Circle?.StartTime==c.StartTime).Circle,c),"circle resource operand identity #"+c.StartTime);
                Check(c.Position==again.Circles.Single(x=>x.StartTime==c.StartTime).Position,"reload has no cumulative displacement #"+c.StartTime);
            }
            foreach(var s in map.Sliders) {
                var b=raw.Sliders.Single(x=>x.Head.StartTime==s.Head.StartTime);var d=s.Head.Position-b.Head.Position;
                Check(ReferenceEquals(map.CircleSounds!.RegisteredOverrides.Single(x=>x.Slider?.Head.StartTime==s.Head.StartTime).Slider,s),"slider resource operand identity");
                Check(s.Geometry.RawSegments.SequenceEqual(b.Geometry.RawSegments),"original raw geometry segments unchanged");
                Check(s.Geometry.CumulativeLengths.SequenceEqual(b.Geometry.CumulativeLengths)&&s.Geometry.UnadjustedLength==b.Geometry.UnadjustedLength&&s.Geometry.RequestedLength==b.Geometry.RequestedLength,"geometry lengths unchanged");
                Check(s.EndTime==b.EndTime&&s.Traversals==b.Traversals&&s.TravelRate==b.TravelRate&&s.PixelLength==b.PixelLength,"slider duration and traversal operands unchanged");
                Check(s.Samples!.Schedule.SequenceEqual(b.Samples!.Schedule)&&s.Samples.EdgeSubset.SequenceEqual(b.Samples.EdgeSubset),"sound schedule unchanged");
                Check(s.ControlPoints.Count==b.ControlPoints.Count&&s.ControlPoints.Zip(b.ControlPoints).All(p=>Near(p.First,p.Second+d)),"all public controls translated");
                Check(s.Geometry.Controls.Count==b.Geometry.Controls.Count&&s.Geometry.Controls.Zip(b.Geometry.Controls).All(p=>Near(p.First,p.Second+d)),"all geometry controls translated");
                Check(s.Geometry.Segments.Count==b.Geometry.Segments.Count&&s.Geometry.Segments.Zip(b.Geometry.Segments).All(p=>Near(p.First.From,p.Second.From+d)&&Near(p.First.To,p.Second.To+d)&&p.First.IsLinear==p.Second.IsLinear&&p.First.IsSubcurveEnd==p.Second.IsSubcurveEnd),"all render path segments translated");
                Check(s.Movements.Count==b.Movements.Count&&s.Movements.Zip(b.Movements).All(p=>Near(p.First.From,p.Second.From+d)&&Near(p.First.To,p.Second.To+d)&&p.First.StartTime==p.Second.StartTime&&p.First.EndTime==p.Second.EndTime),"all input movement segments translated, times retained");
                Check(s.Checkpoints.Count==b.Checkpoints.Count&&s.Checkpoints.Zip(b.Checkpoints).All(p=>Near(p.First.Position,p.Second.Position+d)&&p.First.Time==p.Second.Time&&p.First.Kind==p.Second.Kind&&p.First.Traversal==p.Second.Traversal),"all checkpoint positions translated, metadata retained");
                Check(Near(s.Tail,b.Tail+d)&&Near(s.EndPosition,b.EndPosition+d),"far tail and final traversal endpoint translated");
                int mid=s.Head.StartTime+(s.EndTime-s.Head.StartTime)/2;
                Check(Near(s.RequiredPosition(mid),b.RequiredPosition(mid)+d)&&Near(s.BallPosition(mid),b.BallPosition(mid)+d),"input and presentation interpolation translated");
                Check(s.Head.Position==again.Sliders.Single(x=>x.Head.StartTime==s.Head.StartTime).Head.Position,"slider reload displacement stable");
            }
            var session=new CircleSession(map,ScoreConfiguration.EngineeringPractice,false);
            Check(session.Circles.All(c=>map.Circles.Any(d=>ReferenceEquals(d,c.Definition)))&&session.Sliders.All(s=>map.Sliders.Any(d=>ReferenceEquals(d,s.Definition))),"session uses exact prepared definitions");
            if(session.Heads.Count>0) {
                var first=session.Heads[0];var old=raw.Circles.Concat(raw.Sliders.Select(s=>s.Head)).Single(c=>c.StartTime==first.Definition.StartTime);
                if(first.Definition.Position!=old.Position) {
                    Vector2 pointer=first.Definition.Position-new Vector2(session.HitRadius-.01f,0);
                    Check(Vector2.Distance(pointer,old.Position)>session.HitRadius,"input probe outside original head radius");
                    session.Press(pointer,first.Definition.StartTime);
                    Check(first.Result==Judgement.Hit300,"actual input accepts translated head at discriminating edge");
                }
            }
        }
        string production=Git(root,"log","-1","--format=%H","--","reconstructed/CircleGame");
        int failed=Results.Count(x=>JsonSerializer.SerializeToElement(x).GetProperty("pass").GetBoolean()==false);
        string productionHash=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,"reconstructed/CircleGame/Gameplay/Beatmap.cs"))));
        File.WriteAllText(args[1],JsonSerializer.Serialize(new{source=Git(root,"rev-parse","HEAD"),productionSource=production,productionHash,runtime=System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,checks=Results.Count,failed,originalExecuted=false,ownerQualification="PENDING",results=Results},new JsonSerializerOptions{WriteIndented=true})+"\n");
        Console.WriteLine($"C001 independent integration: {Results.Count-failed}/{Results.Count} PASS; {failed} failures");return failed==0?0:1;
    }
}
