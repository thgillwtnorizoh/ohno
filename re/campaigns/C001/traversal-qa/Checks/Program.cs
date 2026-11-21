using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Globalization;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Platform;
using Ohno.CorpusAudit;
partial class Program {
 static List<object> checks=new(),cases=new(),consumers=new(),stress=new();static Dictionary<string,string> shapes=new();
 static string root="",home="",temp="";static int failures,prepared,rejected,added,lost,audio,errors;static bool baseline;
 static void C(bool pass,string name){checks.Add(new{pass,name});if(!pass){failures++;Console.WriteLine("FAIL "+name);}}
 static string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b));
 static string H(string path)=>Hash(File.ReadAllBytes(path));
 static object? Call(string method,params object?[] args){try{return typeof(Beatmap).GetMethod(method,BindingFlags.NonPublic|BindingFlags.Static)!.Invoke(null,args);}catch(TargetInvocationException e)when(e.InnerException is not null){throw e.InnerException;}}
 static string? Refuse(Action action){try{action();return null;}catch(InvalidDataException e){return e.Message;}}
 static string Shape(SliderDefinition s)=>Hash(JsonSerializer.SerializeToUtf8Bytes(new{
 s.Traversals,s.EndTime,s.TravelRate,s.Tail,s.Geometry.SourceKind,s.Geometry.PreparedKind,s.Geometry.UnadjustedLength,
 controls=s.Geometry.SourceControls.Select(p=>new[]{p.X,p.Y}),raw=s.Geometry.RawSegments.Select(p=>new{from=new[]{p.From.X,p.From.Y},to=new[]{p.To.X,p.To.Y},p.IsLinear,p.IsSubcurveEnd}),
 segments=s.Geometry.Segments.Select(p=>new{from=new[]{p.From.X,p.From.Y},to=new[]{p.To.X,p.To.Y},p.IsLinear,p.IsSubcurveEnd}),lengths=s.Geometry.CumulativeLengths,
 moves=s.Movements.Select(p=>new{from=new[]{p.From.X,p.From.Y},to=new[]{p.To.X,p.To.Y},p.StartTime,p.EndTime}),
 checkpoints=s.Checkpoints.Select(p=>new{p.Time,p.Kind,p.Traversal,position=new[]{p.Position.X,p.Position.Y}})}));
 static SliderDefinition OldComponent(JsonElement c,string path){
  var lines=File.ReadAllText(path).Split('\n');int i=Array.IndexOf(lines,"[HitObjects]")+1;var row=lines[i].Split(',');row[6]="1";lines[i]=string.Join(',',row);string cp=Path.Combine(temp,"control.osu");File.WriteAllText(cp,string.Join('\n',lines));var m=Beatmap.LoadCanonical(cp);var s=m.Sliders.Single();
  var timing=c.GetProperty("timing").GetString()!.Split('\n').Select(x=>x.Split(',')).Select(x=>new TimingPoint(double.Parse(x[0],CultureInfo.InvariantCulture),double.Parse(x[1],CultureInfo.InvariantCulture),x[6]=="1")).ToList();
  return (SliderDefinition)Call("PrepareSlider",s.Head,s.Geometry.SourceControls.ToArray(),s.Geometry.SourceKind,c.GetProperty("length").GetDouble(),c.GetProperty("rawTraversals").GetInt32(),c.GetProperty("version").GetInt32(),1d,c.GetProperty("tickRate").GetDouble(),timing,CancellationToken.None)!;
 }
 static void Original(SliderDefinition s,JsonElement c,string id){
  int n=c.GetProperty("rawTraversals").GetInt32();C(s.Traversals==n&&s.Movements.Count==n*s.Geometry.Segments.Count,id+" repeated work");C(s.EndPosition==(n%2==0?s.Head.Position:s.Tail),id+" endpoint parity");
  for(int t=0;t<n;t++)C(s.TraversalEndMovement(t).To==(t%2==0?s.Tail:s.Head.Position),id+" traversal endpoint "+t);
  var expected=c.GetProperty("original");if(expected.GetProperty("schedule").ValueKind==JsonValueKind.Array){C(s.Checkpoints.Select(x=>x.Time).SequenceEqual(expected.GetProperty("schedule").EnumerateArray().Select(x=>x.GetInt32())),id+" original-derived straight schedule");C(s.EndTime==expected.GetProperty("end").GetInt32(),id+" original-derived end");}
  if(id.StartsWith("fraction-duration"))C(s.EndTime==(int)(2000+501.25*n),id+" fractional absolute accumulation");
  var distances=s.Geometry.CumulativeLengths.Select((x,i)=>(float)(x-(i==0?0:s.Geometry.CumulativeLengths[i-1]))).ToArray();double clock=s.Head.StartTime;int k=0;bool times=true;
  for(int t=0;t<n;t++)for(int j=0;j<distances.Length;j++){int ix=t%2==0?j:distances.Length-1-j;double next=clock+(double)(1000f*distances[ix])/s.TravelRate;var m=s.Movements[k++];times&=m.StartTime==(int)clock&&m.EndTime==(int)next;clock=next;}
  C(times&&s.EndTime==(int)clock,id+" Single multiply/Double accumulation/final truncation");
  var data=s.Samples!;int stride=s.Checkpoints.Count/n;var subset=s.Checkpoints.Take(s.Checkpoints.Count-1).Where((_,i)=>(i+1)%stride==0).Select(x=>x.Time);
  C(data.Schedule.SequenceEqual(s.Checkpoints.Select(x=>x.Time))&&data.EdgeSubset.SequenceEqual(subset),id+" N/R floor subset");C(data.RawTraversals==n&&data.EffectiveTraversals==n,id+" raw/effective operands");
  if(data.Error is null){string e=c.GetProperty("edge").GetString()!,sets=c.GetProperty("sets").GetString()!;var masks=e.Length==0?new List<int>():e.Split('|').Take(n+1).Select(x=>int.TryParse(x,out int v)?v:0).ToList();while(e.Length>0&&masks.Count<n+1)masks.Add(2);
   C(data.Simple==(e.Length==0)&&data.EdgeSounds.SequenceEqual(masks),id+" raw edge masks ignored-TryParse/pad/truncate");
   if(e.Length>0){var pairs=sets.Length==0?new List<int[]>():sets.Split('|').Select(x=>x.Split(':').Select(int.Parse).ToArray()).Take(n+1).ToList();while(pairs.Count<n+1)pairs.Add(new[]{0,0});C(data.Bases.SequenceEqual(pairs.Select(x=>x[0]))&&data.Additions.SequenceEqual(pairs.Select(x=>x[1])),id+" raw+1 edge selectors");}}
 }
 sealed class Sink:ISamplePlaybackSink{public List<SamplePlaybackRequest> Requests=new();public void Enqueue(SamplePlaybackRequest r)=>Requests.Add(r);}
 static void Pack(string p){Directory.CreateDirectory(p);foreach(string bank in new[]{"normal","soft","drum"})foreach(string family in new[]{"hitnormal","hitwhistle","hitfinish","hitclap","slidertick","sliderslide","sliderwhistle"}){using var w=new BinaryWriter(File.Create(Path.Combine(p,bank+"-"+family+".wav")));w.Write("RIFF"u8);w.Write(292);w.Write("WAVEfmt "u8);w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(8000);w.Write(16000);w.Write((short)2);w.Write((short)16);w.Write("data"u8);w.Write(256);w.Write(new byte[256]);}}
 static async Task Connected(Beatmap map,HitSoundLibrary library,string id,bool fixture=true){
  var config=ScoreConfiguration.FromBeatmap(map);var session=CircleSession.CreateFreshOrdinary(map,config);var hc=HealthChart.FromMap(map,session.Approach,HealthPreparationPolicy.E036FreshOrdinary);
  C(map.HealthSource!.IsFreshFor(map)&&session.Health is not null&&hc.Objects.Count==map.Sliders.Count,id+" true fresh calibrated health owner");C(hc.NewComboProvenance.Count==map.Sliders.Count&&hc.NewComboProvenance.All(x=>x.FinalFlag),id+" real NewCombo provenance");
  using var fx=await library.PrepareAsync(map,default);C(ReferenceEquals(fx.PreparedMap,map)&&fx.Sliders.Keys.All(x=>map.Sliders.Any(s=>ReferenceEquals(x,s))),id+" true canonical resource owner");
  if(map.Sliders.Any(s=>s.Samples!.Error is not null)){if(fixture)errors++;C(fx.Error is not null,id+" stored sample error remains unavailable");return;}
  if(fixture)audio++;C(fx.Error is null&&fx.Samples.Count>0,id+" independent WAV bytes loaded");var policy=new CircleSoundPolicy(true,true,true,0,100,false,80,100);var sink=new Sink();using var recording=new RecordingSampleChannels();var channels=new SharedSliderChannels(new GlobalSampleTiming(map.CircleSounds!.Timing),recording,(kind,bank,index)=>fx.Resources.ResolveContinuing(kind,bank,index));bool loops=fx.ContinuousLimitation is null,parentIdentity=true;int events=0,continuing=0;
  session.SliderSound+=e=>{events++;parentIdentity&=map.Sliders.Any(s=>ReferenceEquals(s,e.Parent));SliderSampleSelection.Request(e,fx.Sliders[e.Parent],map.CircleSounds.Timing,fx.Resources,policy,sink);};
  session.ContinuingSound+=(state,time,active)=>{continuing++;if(!loops)return;channels.Clock(time);var d=state.Definition.Samples!;if(active)channels.Submit(d.RawSound,d.BaseSet,d.AdditionSet,true,state.Definition.Head.Position.X);else channels.QueuePause();channels.Drain();};
  foreach(var s in map.Sliders){session.Press(s.Head.Position,s.Head.StartTime);for(int t=s.Head.StartTime;t<=s.EndTime+10;t+=5)session.Advance(t,s.RequiredPosition(Math.Min(t,s.EndTime)),1);}
  foreach(var st in session.Sliders){double ratio=(st.SuccessfulCheckpoints+1d)/(st.Checkpoints.Count+1);var grade=ratio==1?Judgement.Hit300:ratio>=.5?Judgement.Hit100:ratio>0?Judgement.Hit50:Judgement.Miss;C(st.Head.Result==Judgement.Hit300&&st.Result==grade,id+" E17 successes/head aggregate including pending");}
  C(parentIdentity&&events>map.Sliders.Sum(s=>s.Traversals)&&sink.Requests.Count>0,id+" actual input/discrete hooks");C(sink.Requests.All(r=>fx.Samples.ContainsKey(r.Handle)&&r.Event is not null&&map.Sliders.Any(s=>ReferenceEquals(s,r.Event.Parent))),id+" owned bytes and request parents");
  if(loops){channels.QueuePause();channels.Drain();C(recording.Operations.Any(x=>x.Name=="Play")&&recording.Operations.Any(x=>x.Name=="Pause")&&recording.Operations.Where(x=>x.Name=="Acquire").All(x=>fx.Samples.ContainsKey(x.Channel)),id+" actual continuing handle records");channels.Submit(2,1,1,true,null);channels.Cancel();C(channels.Pending==0,id+" cancelled queued audio");}else C(map.Document!.FormatVersion<14,id+" legacy continuing limitation retained");
  foreach(var s in map.Sliders){var d=s.Samples!;var timing=new[]{new SampleTiming(0,1,0,100,true,0),new SampleTiming(s.EndTime+4,3,2,37,false,1)};for(int i=0;i<=Math.Max(s.Traversals,d.EdgeSubset.Count);i++)C(SliderSampleSelection.Edge(s,d,timing,policy,i).Volume==(i<s.Traversals?100:37),id+" original edge query "+i);}
  C(session.Ledger.Score>0&&session.Ledger.TotalWholeResults==map.Sliders.Count&&session.Ledger.IsSupported&&session.Scoring.MultiplierProvenance.StartsWith("AUTO E034")&&session.Health!.Calibration.Trials>0,id+" live automatic score and calibrated health");fx.Dispose();C(fx.Resources.Caches.Map.Count==0,id+" released map resource cache");
  consumers.Add(new{id,fixture,parents=map.Sliders.Count,requests=sink.Requests.Count,continuing,loops,results=session.Sliders.Select(s=>new{result=s.Result.ToString(),s.SuccessfulCheckpoints,s.FailedCheckpoints,pending=s.Checkpoints.Count-s.ProcessedCheckpoints})});
 }
 static async Task Main(string[] args){
  root=Path.GetFullPath(args[0]);string output=Path.GetFullPath(args[1]);baseline=args.Contains("baseline");home=Path.Combine(root,"re/campaigns/C001/traversal-qa");Directory.CreateDirectory(output);temp=Path.Combine(Path.GetTempPath(),"i18qa-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
  var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,"re/campaigns/C001/traversal-profile/manifest.json"))).RootElement.GetProperty("cases");var audit=AuditRunner.Run(Path.Combine(root,"re/campaigns/C001/traversal-profile/fixtures"));string source=baseline?"22a403505f13461477343b9c6a251980716dd90f":"6a152648c72773eb33d37f8ac01f18ca35ab1f41";C(audit.SourceCommit==source&&audit.SourceState=="clean","exact compiled clean source");
  var pins=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,baseline?"old-sources.json":"expected-sources.json"))).RootElement;foreach(var p in pins.GetProperty("hashes").EnumerateObject().Where(x=>x.Name!="LocalHitSoundAssets.cs"))C(audit.SourceHashes.GetValueOrDefault(p.Name)==p.Value.GetString(),"compiled source "+p.Name);
  C(Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().Single(x=>x.Key=="AssetHash").Value==pins.GetProperty("hashes").GetProperty("LocalHitSoundAssets.cs").GetString(),"linked resource source");
  var reference=baseline?new Dictionary<string,string>():JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(Path.Combine(home,"baseline/shapes.json")))!;string pack=Path.Combine(temp,"pack");Pack(pack);using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});
  try{foreach(var c in manifest.EnumerateArray()){
   string id=c.GetProperty("id").GetString()!,path=Path.Combine(root,"re/campaigns/C001/traversal-profile",c.GetProperty("path").GetString()!);string hash=H(path);var stamp=File.GetLastWriteTimeUtc(path);C(hash.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable bytes");var entry=LocalChartCatalog.Index(path);C(entry.Availability.ToString()==c.GetProperty("current").GetProperty("index").GetString(),id+" actual index");Beatmap? map=null;string? error=null;try{map=entry.LoadCanonicalGameplay();}catch(InvalidDataException e){error=e.Message;}
   bool old=c.GetProperty("current").GetProperty("prepared").GetBoolean(),expected=baseline?old:c.GetProperty("futureCandidate").GetBoolean();C((map is not null)==expected,id+" actual canonical admission");if(map is not null){prepared++;if(!old)added++;}else{rejected++;if(old)lost++;}
   var row=audit.Charts.Single(x=>x.EntryKey==Hash(Encoding.UTF8.GetBytes(id+".osu")));C((row.Canonical.Status=="success")==expected,id+" actual audit disposition");
   if(!expected){string guard=c.GetProperty("current").GetProperty("guard").GetString()!;string message=guard=="duration"?"R05 requires each traversal >= 1 ms and parent end <= 3600000 ms.":guard=="checkpoints"?"R05 accepts at most 4096 checkpoints per slider.":baseline?"R05 supports 1–16 slider traversals (span count includes the first traversal).":"Canonical supports 1–32 slider traversals (span count includes the first traversal).";C(error==message,id+" exact typed rejection");C(row.Canonical.ExceptionType=="InvalidDataException"&&row.Canonical.Code==(guard=="checkpoints"?"checkpoint-profile":"slider-repeat-profile"),id+" retained classifier family");}
   if(map is not null){shapes[id]=Shape(map.Sliders.Single());if(!baseline){C(reference[id]==shapes[id],id+" exact previous production geometry/movement/schedule digest");Original(map.Sliders[0],c,id);C(map.Document!.SourceSha256==hash&&map.Document.CopySourceBytes()!.SequenceEqual(File.ReadAllBytes(path)),id+" real canonical byte owner");await Connected(map,library,id);}}
   else if(baseline&&c.GetProperty("probe").GetBoolean())shapes[id]=Shape(OldComponent(c,path));
   if(c.GetProperty("rawTraversals").ValueKind==JsonValueKind.Number){int n=c.GetProperty("rawTraversals").GetInt32();if(n<1||n>16)C(Refuse(()=>SliderSoundMetadata.Parse(2,n,14,0,null,null,null))=="R12 retains the ENGINEERING 1–16 traversal admission bound.",id+" public metadata legacy guard");}
   C(H(path)==hash&&File.GetLastWriteTimeUtc(path)==stamp,id+" fixture remains read-only");cases.Add(new{id,sha256=hash,prepared=map is not null,previous=old,added=map is not null&&!old,error,audit=row.Canonical,sampleError=map?.Sliders[0].Samples?.Error});
  }
  if(!baseline){C((prepared,rejected,added,lost,audio,errors)==(86,20,45,0,82,4),"complete finite fixture contract");await Adversaries(library);}
  }catch(Exception e){File.WriteAllText(Path.Combine(output,"exception.txt"),e.ToString());throw;}finally{Directory.Delete(temp,true);File.WriteAllText(Path.Combine(output,"partial-checks.json"),JsonSerializer.Serialize(checks));}
  File.WriteAllText(Path.Combine(output,"shapes.json"),JsonSerializer.Serialize(shapes,new JsonSerializerOptions{WriteIndented=true}));File.WriteAllText(Path.Combine(output,"audit.json"),AuditRunner.Json(audit));File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{source,state=audit.SourceState,sourceHashes=audit.SourceHashes,runtime=Environment.Version.ToString(),passed=checks.Count-failures,failed=failures,prepared,rejected,added,lost,audio,errors,baseline,checks,cases,consumers,stress},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine($"Independent recovery Issue18 {checks.Count-failures}/{checks.Count} PASS;{prepared}/{rejected};new{added},lost{lost};audio{audio},errors{errors};baseline={baseline}");Environment.ExitCode=failures==0?0:1;
 }
}
