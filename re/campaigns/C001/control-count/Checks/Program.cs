using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Selection;
using Ohno.CorpusAudit;
partial class Program {
 static List<object> checks=new(),cases=new(),consumers=new(),probes=new(),stress=new();
 static int failures,audio,errors,prepared,refused;static string temp="";
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
 static async Task Main(string[] args){
  string root=Path.GetFullPath(args[0]),home=Path.Combine(root,"re/campaigns/C001/control-count"),output=Path.GetFullPath(args[1]);Directory.CreateDirectory(output);
  temp=Path.Combine(Path.GetTempPath(),"ohno-control19-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
  var all=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"manifest.json"))).RootElement.GetProperty("cases");
  var audit=AuditRunner.Run(Path.Combine(home,"fixtures"));C(audit.SourceCommit=="6a152648c72773eb33d37f8ac01f18ca35ab1f41"&&audit.SourceState=="clean","exact clean compiled source");
  var pins=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"expected-sources.json"))).RootElement.GetProperty("hashes");foreach(var p in pins.EnumerateObject())
   C(p.Name=="LocalHitSoundAssets.cs"?Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().Single(a=>a.Key=="AssetHash").Value==p.Value.GetString():audit.SourceHashes.GetValueOrDefault(p.Name)==p.Value.GetString(),"compiled source "+p.Name);
  string pack=Path.Combine(temp,"pack");Pack(pack);using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});
  try{foreach(var c in all.EnumerateArray()){
   string id=c.GetProperty("id").GetString()!,path=Path.Combine(home,c.GetProperty("path").GetString()!);byte[] bytes=File.ReadAllBytes(path);var stamp=File.GetLastWriteTimeUtc(path);
   C(Hash(bytes).Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable fixture");
   var entry=LocalChartCatalog.Index(path);bool indexExpected=c.GetProperty("version").GetInt32() is >=5 and <=14&&c.GetProperty("mode").GetInt32()==0;
   C(entry.Availability==(indexExpected?ChartAvailability.Indexed:ChartAvailability.Unsupported),id+" catalog separate profile");
   Beatmap? map=null;string? error=Refuse(()=>map=Beatmap.LoadCanonical(path));var want=c.GetProperty("current");
   C(error==want.GetProperty("error").GetString(),id+" exact direct canonical stage "+want.GetProperty("stage").GetString());
   C((map is not null)==want.GetProperty("prepared").GetBoolean(),id+" direct disposition");if(map is not null)prepared++;else refused++;
   Beatmap? indexed=null;string? indexedError=Refuse(()=>indexed=entry.LoadCanonicalGameplay());C((indexed is not null)==(map is not null),id+" indexed canonical disposition");
   var row=audit.Charts.Single(x=>x.EntryKey==Hash(Encoding.UTF8.GetBytes(id+".osu")));C((row.Canonical.Status=="success")== (map is not null),id+" actual audit disposition");
   if(c.GetProperty("current").GetProperty("stage").GetString()=="raw-cell-gate"&&indexExpected)C(row.Canonical.Code=="slider-control-limit",id+" broad raw-cell family");
   if(c.GetProperty("current").GetProperty("stage").GetString()=="helper-supplied-gate-after-head")C(row.Canonical.Code=="production-profile",id+" distinct normalized helper family");
   string? legacyError=Refuse(()=>Beatmap.Load(path));
   if(c.GetProperty("kind").GetString()=="L"&&c.GetProperty("bad").ValueKind==JsonValueKind.Null&&c.GetProperty("spans").GetInt32()<=16)
    C(legacyError==error,id+" preserved historical string profile");
   if(c.TryGetProperty("prediction",out _) ){
    var g=Prediction(c);C(g.Controls.Count==c.GetProperty("normalized").GetInt32()&&g.RawSegments.Count==g.Controls.Count-1,id+" normalized count and adjacent pairs");
    var supplied=c.GetProperty("firstEqualsHead").GetBoolean()?g.Controls:g.Controls.Skip(1).ToArray();
    SliderGeometry? historical=null;string? helperError=Refuse(()=>historical=SliderGeometryPreparation.Prepare(new(128,192),supplied,SliderPathKind.Linear,g.RequestedLength,c.GetProperty("version").GetInt32()));
    string? expectedHelper=supplied.Count>64?"Path exceeds the 64-control engineering limit.":g.Controls.Count>64?"R06 allows at most 64 normalized controls per slider.":null;
    C(helperError==expectedHelper,id+" historical helper supplied versus normalized guard");
    if(historical is not null)C(GeometryHash(g)==GeometryHash(historical),id+" scalar oracle versus unchanged actual geometry");
    if(map is not null){C(GeometryHash(map.Sliders.Single().Geometry)==GeometryHash(g),id+" actual canonical geometry");Schedule(map.Sliders.Single(),id);OriginalSchedule(map.Sliders.Single(),c,id);}
    if(g.Controls.Count<=96){
     double rate=id=="timing-inherited-r32"?400:200;double q=Math.Min(id=="timing-inherited-r32"?200:100/c.GetProperty("tick").GetDouble(),g.RequestedLength);
     // This invokes actual unchanged scheduling on an independent predicted geometry.
     // It neither bypasses nor modifies actual canonical geometry admission.
     long before=GC.GetAllocatedBytesForCurrentThread();var probe=Expand(g,c.GetProperty("spans").GetInt32(),rate,q);long allocated=GC.GetAllocatedBytesForCurrentThread()-before;
     Schedule(probe,"ENGINEERING isolated "+id);OriginalSchedule(probe,c,"ENGINEERING isolated "+id);C(probe.Movements.Count<=95*32,"finite proposed movement bound "+id);
     probes.Add(new{id,normalized=g.Controls.Count,segments=g.Segments.Count,raw=g.RawSegments.Count,probe.EndTime,movements=probe.Movements.Count,checkpoints=probe.Checkpoints.Count,allocatedBytes=allocated,canonicalAdmitted=map is not null,geometryHash=GeometryHash(g)});
    }
   }
   if(map is not null){C(map.Document!.SourceSha256==Hash(bytes)&&map.Document.CopySourceBytes()!.SequenceEqual(bytes),id+" canonical byte owner");await Connected(map,library,id);}
   C(Hash(File.ReadAllBytes(path))==Hash(bytes)&&File.GetLastWriteTimeUtc(path)==stamp,id+" read-only fixture");
   cases.Add(new{id,error,index=entry.Availability.ToString(),indexedError,legacyError,stage=want.GetProperty("stage").GetString(),audit=row.Canonical,prepared=map is not null,raw=c.GetProperty("rawCells").GetInt32(),normalized=c.GetProperty("normalized").GetInt32()});
  }
  await Budgets(all,library);C(prepared==38&&refused==130,"finite baseline38 prepared130 refused");
  }finally{Directory.Delete(temp,true);File.WriteAllText(Path.Combine(output,"partial-checks.json"),JsonSerializer.Serialize(checks));}
  File.WriteAllText(Path.Combine(output,"audit.json"),AuditRunner.Json(audit));
  File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{source=audit.SourceCommit,state=audit.SourceState,runtime=Environment.Version.ToString(),passed=checks.Count-failures,failed=failures,prepared,refused,audio,errors,checks,cases,probes,consumers,stress},new JsonSerializerOptions{WriteIndented=true}));
  Console.WriteLine($"Issue19 {checks.Count-failures}/{checks.Count} PASS; {prepared} actual canonical preparations/{refused} refusals; {probes.Count} isolated scheduling probes; {audio} audio-resource consumers");Environment.ExitCode=failures==0?0:1;
 }
}
