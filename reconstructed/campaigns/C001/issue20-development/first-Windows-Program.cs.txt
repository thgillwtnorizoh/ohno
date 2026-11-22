using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows.Media;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Rendering;
internal static class Program {
 [STAThread] static int Main(string[] args){
  var checks=new List<object>();int failed=0,prepared=0,added=0;void C(bool value,string name){checks.Add(new{pass=value,name});if(!value){failed++;Console.WriteLine("FAIL "+name);}}
  string root=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);Directory.CreateDirectory(output);
  var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,"re/campaigns/C001/control-count/manifest.json"))).RootElement;
  var build=typeof(PlayfieldView).GetMethod("BuildPath",BindingFlags.NonPublic|BindingFlags.Static)!;
  C(ReferenceEquals(typeof(Beatmap).Assembly,typeof(PlayfieldView).Assembly),"actual linked WPF assembly defines production Beatmap and rendering");
  var timer=Stopwatch.StartNew();int lines=0;
  foreach(var c in manifest.GetProperty("cases").EnumerateArray()){
   string id=c.GetProperty("id").GetString()!;if(!(c.GetProperty("futureCandidate").GetBoolean()||c.GetProperty("current").GetProperty("prepared").GetBoolean()))continue;
   string path=Path.Combine(root,"re/campaigns/C001/control-count",c.GetProperty("path").GetString()!);var map=Beatmap.LoadCanonical(path);var s=map.Sliders.Single();prepared++;if(!c.GetProperty("current").GetProperty("prepared").GetBoolean())added++;
   var geometry=(StreamGeometry)build.Invoke(null,new object[]{s.Geometry})!;C(geometry.IsFrozen,id+" actual BuildPath returns frozen geometry");
   var flattened=geometry.GetFlattenedPathGeometry();var points=flattened.Figures.SelectMany(f=>new[]{f.StartPoint}.Concat(f.Segments.OfType<PolyLineSegment>().SelectMany(p=>p.Points)).Concat(f.Segments.OfType<LineSegment>().Select(p=>p.Point))).ToArray();
   C(points.Length>0&&double.IsFinite(geometry.Bounds.Width)&&double.IsFinite(geometry.Bounds.Height),id+" finite nonempty WPF path");
   C(points[0].X==s.Geometry.Segments[0].From.X&&points[0].Y==s.Geometry.Segments[0].From.Y&&points[^1].X==s.Geometry.Segments[^1].To.X&&points[^1].Y==s.Geometry.Segments[^1].To.Y,id+" actual prepared WPF endpoints");
   lines+=s.Geometry.Segments.Count;var session=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));C(ReferenceEquals(session.Sliders[0].Definition,s)&&session.Health is not null&&map.HealthSource!.IsFreshFor(map),id+" actual WPF-linked registered session score health identity");
  }
  timer.Stop();C(prepared==112&&added==74,"actual WPF-linked112/74 profile");
  var dll=typeof(Beatmap).Assembly.Location;
  using var pdbStream=File.OpenRead(Path.ChangeExtension(dll,".pdb"));using var pdb=MetadataReaderProvider.FromPortablePdbStream(pdbStream);var reader=pdb.GetMetadataReader();
  var documents=new List<object>();
  foreach(var h in reader.Documents){var d=reader.GetDocument(h);string name=reader.GetString(d.Name).Replace('\\','/');if(!name.EndsWith("/Gameplay/Beatmap.cs",StringComparison.Ordinal))continue;
   string sourceHash=Convert.ToHexString(reader.GetBlobBytes(d.Hash));string currentHash=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,"reconstructed/CircleGame/Gameplay/Beatmap.cs"))));C(sourceHash==currentHash,"actual WPF DLL portable PDB compiled Beatmap SHA256 matches checkout");documents.Add(new{name,sourceHash});
  }
  C(documents.Count==1,"one actual compiled Beatmap PDB identity");var hash=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(dll)));
  File.WriteAllText(Path.Combine(output,"windows-linked.json"),JsonSerializer.Serialize(new{source=Environment.GetEnvironmentVariable("GITHUB_SHA"),actualAssembly=dll,assemblySha256=hash,documents,prepared,added,pathSegments=lines,milliseconds=timer.Elapsed.TotalMilliseconds,passed=checks.Count-failed,failed,checks,appLaunch="NOT RUN",media="NOT RUN"},new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine($"Issue20 WPF linked {checks.Count-failed}/{checks.Count} PASS;{prepared} owners;{lines} actual BuildPath segments");return failed==0?0:1;
 }
}
