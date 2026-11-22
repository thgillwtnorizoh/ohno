using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Selection;
using Ohno.CorpusAudit;
// Work2 development. Reuses published Work1 scalar predictions; not independent QA.
partial class Program {
 static List<object> checks=new(),cases=new(),consumers=new(),probes=new(),stress=new();
 static int failures,audio,errors,prepared,refused;static string temp="",generated="";
 static void C(bool value,string name){checks.Add(new{pass=value,name});if(!value){failures++;Console.WriteLine("FAIL "+name);}}
 static string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes));
 static string? Refuse(Action f){try{f();return null;}catch(InvalidDataException e){return e.Message;}}
 static object? Call(string name,params object?[] args){try{return typeof(Beatmap).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic)!.Invoke(null,args);}catch(TargetInvocationException e)when(e.InnerException is not null){throw e.InnerException;}}
 static Vector2 V(JsonElement e)=>new(e[0].GetSingle(),e[1].GetSingle());
 static SliderGeometry Prediction(JsonElement c){var p=c.GetProperty("prediction");SliderSegment S(JsonElement e)=>new(V(e[0]),V(e[1]),true,true);
  var raw=p.GetProperty("raw").EnumerateArray().Select(S).ToArray();var seg=p.GetProperty("segments").EnumerateArray().Select(S).ToArray();
  var points=new[]{raw[0].From}.Concat(raw.Select(s=>s.To)).ToArray();
  return new(points,raw,seg,p.GetProperty("cumulative").EnumerateArray().Select(e=>e.GetDouble()).ToArray(),p.GetProperty("unadjusted").GetDouble(),c.GetProperty("requested").GetDouble()){SourceKind=SliderPathKind.Linear,PreparedKind=SliderPathKind.Linear,SourceControls=points};
 }
 static string GeometryHash(SliderGeometry g)=>Hash(JsonSerializer.SerializeToUtf8Bytes(new{raw=g.RawSegments.Select(s=>new[]{s.From.X,s.From.Y,s.To.X,s.To.Y}),segments=g.Segments.Select(s=>new[]{s.From.X,s.From.Y,s.To.X,s.To.Y}),g.CumulativeLengths,g.UnadjustedLength,g.RequestedLength}));
 static SliderDefinition Expand(SliderGeometry g,int n,double velocity,double q,CancellationToken token=default,long moves=250000,long cps=100000,long segs=100000,Action<string,int,int>? observe=null)
  =>(SliderDefinition)Call("PrepareSliderGeometry",new CircleDefinition(new(128,192),2000),g,g.RequestedLength,n,velocity,q,token,moves,cps,segs,observe)!;
 static void Schedule(SliderDefinition s,string id){
  double t=2000;int k=0;bool exact=true;
  for(int tr=0;tr<s.Traversals;tr++)for(int j=0;j<s.Geometry.Segments.Count;j++){
   int ix=tr%2==0?j:s.Geometry.Segments.Count-1-j;float d=(float)(s.Geometry.CumulativeLengths[ix]-(ix==0?0:s.Geometry.CumulativeLengths[ix-1]));double end=t+(double)(1000f*d)/s.TravelRate;
   var m=s.Movements[k++];var seg=s.Geometry.Segments[ix];exact&=m.StartTime==(int)t&&m.EndTime==(int)end&&m.From==(tr%2==0?seg.From:seg.To)&&m.To==(tr%2==0?seg.To:seg.From);t=end;
  }
  C(exact&&s.EndTime==(int)t&&k==s.Movements.Count,id+" independent per-piece timing/order");
  C(s.EndPosition==(s.Traversals%2==0?new Vector2(128,192):s.Geometry.Segments[^1].To),id+" endpoint parity");
  C(s.Checkpoints.Count>=s.Traversals&&s.Checkpoints.Count<=4096,id+" finite checkpoint ownership");
  bool positions=true;foreach(var cp in s.Checkpoints.Where(c=>c.Kind!=SliderCheckpointKind.Interior))positions&=cp.Position==s.TraversalEndMovement(cp.Traversal).To;
  C(positions,id+" endpoint checkpoint consumer");
 }
 static void OriginalSchedule(SliderDefinition s,JsonElement c,string id){
  var predicted=c.GetProperty("schedule");C(s.EndTime==predicted.GetProperty("end").GetInt32(),id+" independent end prediction");
  C(s.Checkpoints.Select(x=>$"{x.Time}/{x.Kind}/{x.Traversal}").SequenceEqual(predicted.GetProperty("checkpoints").EnumerateArray().Select(x=>$"{x[0].GetInt32()}/{x[1].GetString()}/{x[2].GetInt32()}")),id+" independent full checkpoint schedule");
 }
 static string? Expected(JsonElement c){
  if(c.GetProperty("current").GetProperty("prepared").GetBoolean()||c.GetProperty("futureCandidate").GetBoolean())return null;
  if(c.GetProperty("version").GetInt32() is <5 or >14||c.GetProperty("kind").GetString()!="L")return c.GetProperty("current").GetProperty("error").GetString();
  if(c.GetProperty("rawCells").GetInt32()>96)return "Canonical source L allows at most 96 raw coordinate cells before head insertion.";
  string? bad=c.GetProperty("bad").GetString();
  return bad switch {
   "arity" or "marker"=>"Invalid slider control; expected x:y.",
   "malformed" or "nonfinite"=>"Invalid finite number for Bézier control coordinate.",
   "out-of-range"=>"R06 Bézier controls must fit finite Int32 coordinates.",
   "length"=>"R05 slider length must be greater than 0.0001 and at most 4096 pixels.",
   "duration"=>"R05 requires each traversal >= 1 ms and parent end <= 3600000 ms.",
   "checkpoint-real"=>"R05 accepts at most 4096 checkpoints per slider.",
   _=>c.GetProperty("mode").GetInt32()!=0?"R02 requires explicit Mode: 0 (osu!standard).":"Canonical source L exceeds 96 normalized controls after head insertion."
  };
 }
 static void ControlsFromBytes(string path,JsonElement c,SliderDefinition? s,string id){
  string line=File.ReadAllLines(path).Last(x=>x.StartsWith("128,192,2000,",StringComparison.Ordinal));var row=line.Split(',');var cells=row[5].Split('|');
  C(cells.Length-1==c.GetProperty("rawCells").GetInt32(),id+" byte-derived raw cells excludes marker");
  if(c.GetProperty("bad").ValueKind!=JsonValueKind.Null)return;
  var pts=cells.Skip(1).Select(x=>x.Split(':')).Select(x=>new Vector2((float)(int)double.Parse(x[0],System.Globalization.CultureInfo.InvariantCulture),(float)(int)double.Parse(x[1],System.Globalization.CultureInfo.InvariantCulture))).ToList();
  bool eq=pts[0]==new Vector2(128,192);if(!eq)pts.Insert(0,new(128,192));
  C(eq==c.GetProperty("firstEqualsHead").GetBoolean()&&pts.Count==c.GetProperty("normalized").GetInt32(),id+" exact converted equality and normalized count from bytes");
  if(s is not null){C(s.Geometry.SourceControls.SequenceEqual(pts),id+" original source control identity");C(s.Geometry.Controls.SequenceEqual(pts)&&s.ControlPoints.SequenceEqual(pts),id+" unstacked prepared controls");}
 }
 static async Task Main(string[] args){
  string root=Path.GetFullPath(args[0]),home=Path.Combine(root,"re/campaigns/C001/control-count"),output=Path.GetFullPath(args[1]);Directory.CreateDirectory(output);generated=Path.Combine(output,"generated-fixtures");Directory.CreateDirectory(generated);
  temp=Path.Combine(Path.GetTempPath(),"ohno-issue20-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
  var all=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"manifest.json"))).RootElement.GetProperty("cases");
  var audit=AuditRunner.Run(Path.Combine(home,"fixtures"));
  if(Environment.GetEnvironmentVariable("GITHUB_SHA") is {} sha)C(audit.SourceCommit==sha&&audit.SourceState=="clean","exact clean compiled Windows source");
  C(audit.Complete&&audit.Charts.Count==168,"complete168 actual read-only audit");
  string pack=Path.Combine(temp,"pack");Pack(pack);using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});int added=0,lost=0;
  try{foreach(var c in all.EnumerateArray()){
   string id=c.GetProperty("id").GetString()!,path=Path.Combine(home,c.GetProperty("path").GetString()!);byte[] bytes=File.ReadAllBytes(path);var stamp=File.GetLastWriteTimeUtc(path);string hash=Hash(bytes);
   C(hash.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable fixture");
   var entry=LocalChartCatalog.Index(path);bool indexExpected=c.GetProperty("version").GetInt32() is >=5 and <=14&&c.GetProperty("mode").GetInt32()==0;
   C(entry.Availability==(indexExpected?ChartAvailability.Indexed:ChartAvailability.Unsupported),id+" catalog separate profile");
   Beatmap? map=null;string? error=Refuse(()=>map=Beatmap.LoadCanonical(path));string? expected=Expected(c);
   bool old=c.GetProperty("current").GetProperty("prepared").GetBoolean(),want=old||c.GetProperty("futureCandidate").GetBoolean();
   C(error==expected,id+" exact direct diagnostic expected="+expected+" actual="+error);C((map is not null)==want,id+" direct canonical disposition");
   if(map is not null){prepared++;if(!old)added++;}else{refused++;if(old)lost++;}
   Beatmap? indexed=null;string? indexedError=Refuse(()=>indexed=entry.LoadCanonicalGameplay());C((indexed is not null)==(map is not null),id+" indexed canonical disposition");
   var row=audit.Charts.Single(x=>x.EntryKey==Hash(Encoding.UTF8.GetBytes(id+".osu")));C((row.Canonical.Status=="success")==want,id+" audit canonical disposition");
   if(indexExpected)C(row.Canonical.ExceptionType==(map is null?"InvalidDataException":null),id+" audit typed message matches direct loader");
   string auditCode=want?"canonical-prepared":c.GetProperty("version").GetInt32() is <5 or >14?"format-profile":c.GetProperty("mode").GetInt32()!=0?"mode-profile":c.GetProperty("bad").GetString() switch {
    "malformed" or "nonfinite"=>"numeric-syntax","arity" or "marker"=>"slider-control-syntax","out-of-range"=>"production-profile","duration"=>"slider-repeat-profile","length"=>"slider-length-profile","checkpoint-real"=>"checkpoint-profile",_=>expected=="Canonical source L allows at most 96 raw coordinate cells before head insertion."||expected=="Path exceeds the 64-control engineering limit."?"production-profile":"slider-control-limit"};
   C(row.Canonical.Code==auditCode,id+" explicit per-case audit code "+auditCode);
   string? legacyError=Refuse(()=>Beatmap.Load(path));
   if(c.GetProperty("kind").GetString()=="L")C(legacyError==c.GetProperty("current").GetProperty("error").GetString()||(c.GetProperty("spans").GetInt32()>16&&legacyError=="R05 supports 1–16 slider traversals (span count includes the first traversal)."),id+" historical loader frozen baseline");
   ControlsFromBytes(path,c,map?.Sliders.Single(),id);
   if(c.TryGetProperty("prediction",out _) ){
    var g=Prediction(c);var supplied=c.GetProperty("firstEqualsHead").GetBoolean()?g.Controls:g.Controls.Skip(1).ToArray();
    string? expectedHelper=supplied.Count>64?"Path exceeds the 64-control engineering limit.":g.Controls.Count>64?"R06 allows at most 64 normalized controls per slider.":null;
    C(Refuse(()=>SliderGeometryPreparation.Prepare(new(128,192),supplied,SliderPathKind.Linear,g.RequestedLength,c.GetProperty("version").GetInt32()))==expectedHelper,id+" public kind helper64 retained");
    C(Refuse(()=>SliderGeometryPreparation.Prepare(new(128,192),supplied,false,g.RequestedLength,c.GetProperty("version").GetInt32()))==expectedHelper,id+" public bool helper64 retained");
    if(map is not null){var s=map.Sliders.Single();C(GeometryHash(s.Geometry)==GeometryHash(g),id+" scalar raw/trim/cumulative/G/requested geometry");C(s.Geometry.RawSegments.All(x=>x.IsSubcurveEnd&&x.IsLinear),id+" Linear flags");Schedule(s,id);OriginalSchedule(s,c,id);
     var d=s.Samples!;C(d.Schedule.SequenceEqual(s.Checkpoints.Select(x=>x.Time))&&d.EdgeSubset.Count==(s.Checkpoints.Count-1)/(s.Checkpoints.Count/s.Traversals),id+" raw+1 and N/R original sample schedule");
     C(d.EdgeSounds.SequenceEqual(new[]{2,8}.Concat(Enumerable.Repeat(2,Math.Max(0,s.Traversals-1))))&&d.Bases.Count==s.Traversals+1,id+" short edge list padding");
    }
   }
   if(map is not null){C(map.Document!.SourceSha256==hash&&map.Document.CopySourceBytes()!.SequenceEqual(bytes),id+" real canonical byte owner");await Connected(map,library,id);}
   C(Hash(File.ReadAllBytes(path))==hash&&File.GetLastWriteTimeUtc(path)==stamp,id+" read-only fixture after consumers");
   cases.Add(new{id,sha256=hash,previouslyPrepared=old,prepared=map is not null,added=map is not null&&!old,error,index=entry.Availability.ToString(),indexedError,legacyError,audit=row.Canonical,raw=c.GetProperty("rawCells").GetInt32(),normalized=c.GetProperty("normalized").GetInt32(),sampleError=map?.Sliders[0].Samples!.Error});
  }
  await Stress(root,library);C(prepared==112&&refused==56&&added==74&&lost==0,"168 target112/56 plus74 zero losses");
  }finally{File.WriteAllText(Path.Combine(output,"partial-checks.json"),JsonSerializer.Serialize(checks));Directory.Delete(temp,true);}
  File.WriteAllText(Path.Combine(output,"audit.json"),AuditRunner.Json(audit));
  File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{actor="Work2 development, NOT independent QA",source=audit.SourceCommit,state=audit.SourceState,sourceHashes=audit.SourceHashes,assetHash=Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().Single(a=>a.Key=="AssetHash").Value,runtime=Environment.Version.ToString(),passed=checks.Count-failures,failed=failures,prepared,refused,added,lost,audio,errors,checks,cases,consumers,stress},new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine($"Issue20 development {checks.Count-failures}/{checks.Count} PASS; {prepared}/{refused}; +{added} lost{lost}");Environment.ExitCode=failures==0?0:1;
 }
}
