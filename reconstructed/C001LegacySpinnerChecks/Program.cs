using System.Reflection;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Platform;


class Program {
 static List<object> tests=new();static List<object> outcomes=new();
 static void Check(bool ok,string name){tests.Add(new{name,pass=ok});if(!ok)Console.WriteLine("FAIL "+name);}
 static string Hash(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p)));
 static string Site(Exception e)=>e.Message switch {
 "Versioned spinner requires full explicit finite timing/sample rows."=>"explicit-timing",
 "Versioned spinner first profile requires no stacking or breaks."=>"stack-setting-or-break",
 "Versioned spinner requires strictly increasing registered source order."=>"registered-order",
 "Versioned spinner first profile requires separated object intervals."=>"separated-intervals",
 _=>"OTHER: "+e.GetType().Name+": "+e.Message};
 static void Main(string[] args){
 string repo=Path.GetFullPath(args[0]),outdir=Path.GetFullPath(args[1]),folder=Path.Combine(repo,"re/campaigns/C001/legacy-spinner-profile");Directory.CreateDirectory(outdir);
 var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(folder,"manifest.json"))).RootElement.GetProperty("cases");
 var targets=JsonDocument.Parse(File.ReadAllText(Path.Combine(folder,"handoff-targets.json"))).RootElement.GetProperty("cases").EnumerateArray().ToDictionary(c=>c.GetProperty("id").GetString()!,c=>c.GetProperty("proposedFinitePrepared").GetBoolean());
 string temp=Path.Combine(Path.GetTempPath(),"ohno-i9-probe-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
 int admissions=0,zeroControls=0;
 try { foreach(var c in manifest.EnumerateArray()){
  string id=c.GetProperty("id").GetString()!,path=Path.Combine(folder,c.GetProperty("path").GetString()!);int version=c.GetProperty("version").GetInt32();bool wanted=targets[id];
  Check(Hash(path).Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable hash");
  var settings=OsuMetadataSettings.Prepare(OsuDocument.Read(File.ReadAllBytes(path)));
  Check(settings.Error is null&&settings.StackLeniency==(float)c.GetProperty("expectedSetting").GetDouble(),id+" actual metadata scalar distinct from displacement");
  var chart=LocalChartCatalog.Index(path);Check(chart.Availability==ChartAvailability.Indexed,id+" catalog candidate independent of canonical qualification");
  Beatmap? map=null;string guard="none";try{map=chart.LoadCanonicalGameplay();}catch(InvalidDataException e){guard=Site(e);}
  Beatmap? direct=null;try{direct=Beatmap.LoadCanonical(path);}catch(InvalidDataException){}
  Check((map is not null)==wanted&&(direct is not null)==wanted,id+" independent current canonical disposition");
  object? consumers=null;
  if(map is not null){admissions++;var score=ScoreConfiguration.FromBeatmap(map);var session=CircleSession.CreateFreshOrdinary(map,score);var health=HealthChart.FromMap(map,session.Approach,HealthPreparationPolicy.E036FreshOrdinary);
   Check(map.HealthSource!.IsFreshFor(map)&&session.Health is not null&&health.Objects.Count==map.HealthSource.Rows.Count,id+" fresh calibrated health and registration");
   var flags=c.GetProperty("original").GetProperty("finalNoBreakFlags");if(flags.ValueKind==JsonValueKind.Array)Check(health.NewComboProvenance.Select(x=>x.FinalFlag).SequenceEqual(flags.EnumerateArray().Select(x=>x.GetBoolean())),id+" original-derived registration NewCombo sequence");
   Check(map.Spinners.All(s=>map.CircleSounds!.Spinners.Any(p=>ReferenceEquals(p.Object,s))&&session.Spinners.Any(p=>ReferenceEquals(p.Definition,s))),id+" real sample/session spinner identities");
   Check(HitSoundLibrary.SpinnerAdmission(map) is null,id+" actual spinner audio admission");
   Check(map.Spinners.All(s=>s.Center==new Vector2(256,192)&&s.Preparation!.CompatibilityPolicy==SpinnerCompatibilityPolicy.ModernWithoutRecordedSource&&s.Preparation.EndpointPolicy==SpinnerEndpointPolicy.EngineeringUncappedHostLengthUnavailable),id+" fixed center and disclosed policies");
   var rawPositions=c.GetProperty("original").GetProperty("preparedHeads");
   if(rawPositions.ValueKind==JsonValueKind.Array){var heads=map.Circles.Select(x=>(time:x.StartTime,pos:x.Position)).Concat(map.Sliders.Select(x=>(time:x.Head.StartTime,pos:x.Head.Position))).Concat(map.Spinners.Select(x=>(time:x.StartTime,pos:x.Center))).OrderBy(x=>x.time).ToArray();var target=rawPositions.EnumerateArray().ToArray();Check(heads.Length==target.Length&&heads.Select((x,i)=>Vector2.Distance(x.pos,new(target[i][0].GetSingle(),target[i][1].GetSingle()))).All(x=>x<0.00004f),id+" original-derived prepared head coordinates");}
   consumers=new{score.K,healthObjects=health.Objects.Count,flags=health.NewComboProvenance.Select(p=>p.FinalFlag),sessionHealth=session.Health is not null,preparedHeads=map.Circles.Select(x=>new{x.StartTime,x.Position.X,x.Position.Y}),spinnerCenters=map.Spinners.Select(x=>new{x.StartTime,x.Center.X,x.Center.Y}),audio=HitSoundLibrary.SpinnerAdmission(map)??"admitted"};
  }
  Check(Hash(path).Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" unchanged source after consumers");
  outcomes.Add(new{id,index=chart.Availability.ToString(),reachedGuard=guard,prepared=map is not null,consumers,originalPrediction=c.GetProperty("original").Clone(),handoff=c.GetProperty("handoff").Clone()});
 }}finally{Directory.Delete(temp,true);}
 int failed=tests.Count(x=>!JsonSerializer.SerializeToElement(x).GetProperty("pass").GetBoolean());File.WriteAllText(Path.Combine(outdir,"results.json"),JsonSerializer.Serialize(new{actor="Work2 new-production development; independent QA pending",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local candidate; source hashes in source.json",runtime=Environment.Version.ToString(),passed=tests.Count-failed,failed,fixtures=manifest.GetArrayLength(),admissions,rejections=manifest.GetArrayLength()-admissions,zeroControls,tests,outcomes,originalRuntime="NOT RUN"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine($"Issue9 new development: {tests.Count-failed}/{tests.Count} PASS; {admissions} prepared; {zeroControls} isolated scalar controls");Environment.ExitCode=failed==0?0:1;
 }
}
