using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Selection;
using Ohno.CorpusAudit;
partial class Program {
 const string Target="5dda483348e21e79c8354bf6ea322d50b7fb3e72";
 static readonly CultureInfo Inv=CultureInfo.InvariantCulture;
 // New Work1 oracle: admission is computed from the bytes and finite handoff,
 // not Work2 outcomes or fixture-name/futureCandidate labels.
 static (int raw,int normalized,Vector2[] points,string? error) Inspect(string text,bool canonical=true){
  var lines=text.Split('\n');int version=int.Parse(lines[0].Trim()[17..],Inv);
  var r=lines.Last(x=>x.StartsWith("128,192,2000,",StringComparison.Ordinal)).Trim().Split(',');
  var cells=r[5].Split('|');int raw=cells.Length-1,limit=canonical&&cells[0]=="L"?96:64;
  if(version is <5 or >14)return(raw,-1,[],"Use an osu file format v5–v14 supported circle/Linear-slider map.");
  if(raw>limit)return(raw,-1,[],limit==64?"R06 allows at most 64 normalized controls per slider.":"Canonical source L allows at most 96 raw coordinate cells before head insertion.");
  var points=new List<Vector2>();
  foreach(string cell in cells.Skip(1)){
   var xy=cell.Split(':');if(xy.Length!=2)return(raw,-1,[],"Invalid slider control; expected x:y.");
   var values=new double[2];for(int i=0;i<2;i++){
    if(!double.TryParse(xy[i],NumberStyles.Float,Inv,out values[i])||!double.IsFinite(values[i]))return(raw,-1,[],"Invalid finite number for Bézier control coordinate.");
    if(values[i]<int.MinValue||values[i]>int.MaxValue)return(raw,-1,[],"R06 Bézier controls must fit finite Int32 coordinates.");
   }
   points.Add(new((float)(int)values[0],(float)(int)values[1]));
  }
  if(points.Count==0||points[0]!=new Vector2(128,192))points.Insert(0,new(128,192));
  int spans=int.Parse(r[6],Inv);double length=double.Parse(r[7],Inv);
  string? e=spans>(canonical?32:16)?(canonical?"Canonical supports 1–32 slider traversals (span count includes the first traversal).":"R05 supports 1–16 slider traversals (span count includes the first traversal)."):
   length<=.0001||length>4096?"R05 slider length must be greater than 0.0001 and at most 4096 pixels.":
   !lines.Any(x=>x.Trim()=="Mode:0")?"R02 requires explicit Mode: 0 (osu!standard).":
   points.Count>limit?(limit==96?"Canonical source L exceeds 96 normalized controls after head insertion.":"Path exceeds the 64-control engineering limit."):null;
  return(raw,points.Count,points.ToArray(),e);
 }
 static string AuditCode(JsonElement c,string? error){
  if(error is null)return "canonical-prepared";
  if(c.GetProperty("version").GetInt32() is <5 or >14)return "format-profile";
  if(c.GetProperty("mode").GetInt32()!=0)return "mode-profile";
  if(error.StartsWith("Invalid finite"))return "numeric-syntax";
  if(error.StartsWith("Invalid slider control"))return "slider-control-syntax";
  if(error.StartsWith("R05 slider length"))return "slider-length-profile";
  if(error.StartsWith("R05 requires each traversal"))return "slider-repeat-profile";
  if(error.Contains("checkpoints per slider"))return "checkpoint-profile";
  return error=="R06 allows at most 64 normalized controls per slider."||error=="Canonical source L exceeds 96 normalized controls after head insertion."?"slider-control-limit":"production-profile";
 }
 static async Task Main(string[] args){
  string root=Path.GetFullPath(args[0]),home=Path.Combine(root,"re/campaigns/C001/control-count"),output=Path.GetFullPath(args[1]);Directory.CreateDirectory(output);
  temp=Path.Combine(Path.GetTempPath(),"ohno-independent21-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
  var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"manifest.json"))).RootElement.GetProperty("cases");
  var audit=AuditRunner.Run(Path.Combine(home,"fixtures"));
  C(audit.SourceCommit==Target&&audit.SourceState=="clean"&&audit.Complete&&audit.Charts.Count==168,"exact immutable compiled source and complete audit");
  string pack=Path.Combine(temp,"pack");Pack(pack);using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});
  try{foreach(var c in manifest.EnumerateArray()){
   string id=c.GetProperty("id").GetString()!,path=Path.Combine(home,c.GetProperty("path").GetString()!);byte[] bytes=File.ReadAllBytes(path);var stamp=File.GetLastWriteTimeUtc(path);string hash=Hash(bytes),text=Encoding.UTF8.GetString(bytes);
   C(hash.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable bytes");
   var expected=Inspect(text);string? want=expected.error;
   // Remaining semantic gates follow the sealed original-static duration/G oracle.
   if(want is null&&c.GetProperty("bad").GetString()=="duration")want="R05 requires each traversal >= 1 ms and parent end <= 3600000 ms.";
   if(want is null&&c.GetProperty("bad").GetString()=="checkpoint-real")want="R05 accepts at most 4096 checkpoints per slider.";
   C(expected.raw==c.GetProperty("rawCells").GetInt32(),id+" raw cells from bytes excluding marker");
   if(expected.normalized>=0)C(expected.normalized==c.GetProperty("normalized").GetInt32(),id+" independent converted normalized count");
   var entry=LocalChartCatalog.Index(path);bool supported=c.GetProperty("version").GetInt32() is >=5 and <=14&&c.GetProperty("mode").GetInt32()==0;
   C(entry.Availability==(supported?ChartAvailability.Indexed:ChartAvailability.Unsupported),id+" catalog admission independent of geometry");
   Beatmap? map=null,indexed=null;string? error=Refuse(()=>map=Beatmap.LoadCanonical(path));string? indexedError=Refuse(()=>indexed=entry.LoadCanonicalGameplay());
   C(error==want,id+" direct exact diagnostic expected="+want+" got="+error);C((map is not null)==(want is null),id+" direct admission");
   C((indexed is not null)==(want is null),id+" actual catalog gameplay admission");if(supported)C(indexedError==want,id+" indexed diagnostic precedence");
   var row=audit.Charts.Single(x=>x.EntryKey==Hash(Encoding.UTF8.GetBytes(id+".osu")));
   C(row.Canonical.Code==AuditCode(c,want)&&(row.Canonical.Status=="success")== (want is null),id+" independent AuditRunner diagnostic family");
   C(want is null?row.Audio.Code=="no-audio-declared"&&row.Consumers.Count==4&&row.Consumers.Values.All(x=>x.Status=="success"):row.Audio.Code=="canonical-not-prepared",id+" audit resource readiness and connected structural consumers");
   bool old=c.GetProperty("current").GetProperty("prepared").GetBoolean();
   if(map is null){refused++;if(old)lost++;}else{prepared++;if(!old)added++;}
   // Legacy frozen expectations are Work1 Issue19 baseline, with retained 16-span precedence.
   string? legacy=Refuse(()=>Beatmap.Load(path));
   if(c.GetProperty("kind").GetString()=="L"){
    string? legacyWant=c.GetProperty("current").GetProperty("error").GetString();
    if(c.GetProperty("spans").GetInt32()>16&&expected.raw<=64&&c.GetProperty("bad").GetString() is not ("malformed" or "arity" or "marker" or "nonfinite" or "out-of-range"))legacyWant="R05 supports 1–16 slider traversals (span count includes the first traversal).";
    C(legacy==legacyWant,id+" historical string admission and error precedence");
   }
   if(c.TryGetProperty("prediction",out _)){
    var oracle=Prediction(c);var supplied=c.GetProperty("firstEqualsHead").GetBoolean()?oracle.Controls:oracle.Controls.Skip(1).ToArray();
    string? publicError=supplied.Count>64?"Path exceeds the 64-control engineering limit.":oracle.Controls.Count>64?"R06 allows at most 64 normalized controls per slider.":null;
    C(Refuse(()=>SliderGeometryPreparation.Prepare(new(128,192),supplied,SliderPathKind.Linear,oracle.RequestedLength,c.GetProperty("version").GetInt32()))==publicError,id+" public kind API historical64");
    C(Refuse(()=>SliderGeometryPreparation.Prepare(new(128,192),supplied,false,oracle.RequestedLength,c.GetProperty("version").GetInt32()))==publicError,id+" public bool API historical64");
    if(map is not null){var s=map.Sliders.Single();
     C(s.Geometry.SourceControls.SequenceEqual(expected.points)&&s.ControlPoints.SequenceEqual(expected.points),id+" actual converted controls");
     C(GeometryHash(s.Geometry)==GeometryHash(oracle),id+" sealed Work1 raw/prepared/cumulative length digest");
     C(s.Geometry.RawSegments.Count==expected.normalized-1&&s.Geometry.RawSegments.All(x=>x.IsSubcurveEnd&&x.IsLinear),id+" N-1 including duplicate zero pieces and flags");
     Schedule(s,id);OriginalSchedule(s,c,id);
     var d=s.Samples!;int n=s.Checkpoints.Count,repeat=n/s.Traversals;
     C(d.Schedule.SequenceEqual(s.Checkpoints.Select(x=>x.Time))&&d.EdgeSubset.Count==(n-1)/repeat,id+" floor N/R raw plus one sample owners");
    }
   }
   if(map is not null){C(map.Document!.SourceSha256==hash&&map.Document.CopySourceBytes()!.SequenceEqual(bytes),id+" actual canonical immutable document");await Connected(map,library,id);}
   C(Hash(File.ReadAllBytes(path))==hash&&File.GetLastWriteTimeUtc(path)==stamp,id+" immutable after every consumer");
   cases.Add(new{id,hash,raw=expected.raw,normalized=expected.normalized,want,error,index=entry.Availability.ToString(),indexedError,legacy,audit=row.Canonical,old,prepared=map is not null});
  }
  C(prepared==112&&refused==56&&added==74&&lost==0,"independently derived112/56 plus74 zero lost");
  await Adversaries(library);
  }finally{File.WriteAllText(Path.Combine(output,"partial-checks.json"),JsonSerializer.Serialize(checks));Directory.Delete(temp,true);}
  File.WriteAllText(Path.Combine(output,"audit.json"),AuditRunner.Json(audit));
  File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{actor="Work1 independent Issue21",source=audit.SourceCommit,state=audit.SourceState,sourceHashes=audit.SourceHashes,runtime=Environment.Version.ToString(),passed=checks.Count-failures,failed=failures,prepared,refused,added,lost,audio,errors,checks,cases,consumers,stress},new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine($"Issue21 independent {checks.Count-failures}/{checks.Count} PASS; prepared{prepared} excluded{refused} gained{added} lost{lost}; audio{audio}");Environment.ExitCode=failures==0?0:1;
 }
}
