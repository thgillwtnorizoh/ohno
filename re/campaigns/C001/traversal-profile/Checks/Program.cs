using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using System.Globalization;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Platform;
using Ohno.CorpusAudit;
class Program {
 static List<object> checks=new(),outcomes=new(),probes=new();
 static void C(bool pass,string name){checks.Add(new{pass,name});if(!pass)Console.WriteLine("FAIL "+name);}
 static string Hash(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p)));
 static readonly MethodInfo Prepare=typeof(Beatmap).GetMethod("PrepareSlider",BindingFlags.Static|BindingFlags.NonPublic)!;
 static readonly string Repeat="R05 supports 1–16 slider traversals (span count includes the first traversal).";
 static double D(string s)=>double.Parse(s,CultureInfo.InvariantCulture);
 static SliderDefinition Component(Beatmap control,JsonElement c,int n,CancellationToken token=default){
  var s=control.Sliders.Single();var timing=c.GetProperty("timing").GetString()!.Split('\n').Select(l=>l.Split(',')).Select(a=>new TimingPoint(D(a[0]),D(a[1]),a[6]=="1")).ToList();
  try{return (SliderDefinition)Prepare.Invoke(null,new object[]{s.Head,s.Geometry.SourceControls.ToArray(),s.Geometry.SourceKind,c.GetProperty("length").GetDouble(),n,c.GetProperty("version").GetInt32(),1d,c.GetProperty("tickRate").GetDouble(),timing,token})!;}
  catch(TargetInvocationException e)when(e.InnerException is not null){throw e.InnerException;}
 }
 static SliderSoundMetadata SoundOperands(JsonElement c,int n){
  string edge=c.GetProperty("edge").GetString()!,sets=c.GetProperty("sets").GetString()!;bool simple=edge.Length==0;
  List<int> sounds=simple?new():edge.Split('|').Take(n+1).Select(x=>int.TryParse(x,out int v)?v:0).ToList();while(!simple&&sounds.Count<n+1)sounds.Add(2);
  var bs=new List<int>();var adds=new List<int>();
  if(sets.Length>0)foreach(var x in sets.Split('|')){var p=x.Split(':');bs.Add(int.Parse(p[0],CultureInfo.InvariantCulture));adds.Add(int.Parse(p[1],CultureInfo.InvariantCulture));}
  if(!simple){bs=bs.Take(n+1).ToList();adds=adds.Take(n+1).ToList();while(bs.Count<n+1)bs.Add(0);while(adds.Count<n+1)adds.Add(0);}
  return new(2,n,n,c.GetProperty("version").GetInt32(),0,edge,sets,null,simple,sounds,bs,adds,0,0,0,100,"",null);
 }
 static void Geometry(SliderDefinition s,JsonElement c,int n,string label){
  C(s.Traversals==n&&s.Movements.Count==n*s.Geometry.Segments.Count,label+" R times prepared-segment work");
  C(s.Checkpoints.Count>=n&&s.Checkpoints.Count/s.Traversals>0,label+" nonzero N/R");
  C(s.EndPosition==(n%2==1?s.Tail:s.Head.Position),label+" odd/even final endpoint");
  for(int k=0;k<n;k++){var m=s.TraversalEndMovement(k);C(m.To==(k%2==0?s.Tail:s.Head.Position),label+" traversal endpoint "+k);}
  var expected=c.GetProperty("original").GetProperty("schedule");
  if(expected.ValueKind==JsonValueKind.Array)C(s.Checkpoints.Select(x=>x.Time).SequenceEqual(expected.EnumerateArray().Select(x=>x.GetInt32()))&&s.EndTime==c.GetProperty("original").GetProperty("end").GetInt32(),label+" independent straight300 schedule/end");
  if(c.GetProperty("id").GetString()!.StartsWith("fraction-duration",StringComparison.Ordinal))C(s.EndTime==(int)(2000+501.25*n),label+" rational fractional accumulation (not per-span truncate)");
  C(s.TerminalTime==Math.Max(s.Head.StartTime+(s.EndTime-s.Head.StartTime)/2,s.Checkpoints[^1].Time),label+" terminal time retains finite midpoint bound");
 }
 sealed class Sink:ISamplePlaybackSink{public List<SamplePlaybackRequest> Requests=new();public void Enqueue(SamplePlaybackRequest r)=>Requests.Add(r);}
 static async Task Main(string[] args){
  string root=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]),home=Path.Combine(root,"re/campaigns/C001/traversal-profile"),fixtures=Path.Combine(home,"fixtures");Directory.CreateDirectory(output);
  var cases=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"manifest.json"))).RootElement.GetProperty("cases");var pins=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"expected-sources.json"))).RootElement;
  var audit=AuditRunner.Run(fixtures);C(audit.SourceCommit==pins.GetProperty("source").GetString()&&audit.SourceState=="clean","exact clean compiled source");
  foreach(var p in pins.GetProperty("hashes").EnumerateObject().Where(x=>x.Name!="LocalHitSoundAssets.cs"))C(audit.SourceHashes.GetValueOrDefault(p.Name)==p.Value.GetString(),"compiled "+p.Name);
  C(Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().Single(x=>x.Key=="AssetHash").Value==pins.GetProperty("hashes").GetProperty("LocalHitSoundAssets.cs").GetString(),"actual linked resource source");
  C(audit.Complete&&audit.Charts.Count==cases.GetArrayLength(),"complete actual corpus scan");C(AuditRunner.Json(audit)==AuditRunner.Json(AuditRunner.Run(fixtures)),"deterministic read-only audit");
  string tmp=Path.Combine(Path.GetTempPath(),"i17-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(tmp);int admitted=0,rejected=0,componentProbes=0,guard2=0;var budgets=new List<object>();
  try{
   string pack=Path.Combine(tmp,"pack");Directory.CreateDirectory(pack);
   foreach(string bank in new[]{"normal","soft","drum"})foreach(string family in new[]{"hitnormal","hitwhistle","hitfinish","hitclap","slidertick","sliderslide","sliderwhistle"}){using var w=new BinaryWriter(File.Create(Path.Combine(pack,bank+"-"+family+".wav")));w.Write("RIFF"u8.ToArray());w.Write(292);w.Write("WAVEfmt "u8.ToArray());w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(8000);w.Write(16000);w.Write((short)2);w.Write((short)16);w.Write("data"u8.ToArray());w.Write(256);w.Write(new byte[256]);}
   using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});
   foreach(var c in cases.EnumerateArray()){
    string id=c.GetProperty("id").GetString()!,path=Path.Combine(home,c.GetProperty("path").GetString()!);string hash=Hash(path);var stamp=File.GetLastWriteTimeUtc(path);C(hash.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable bytes");
    var indexed=LocalChartCatalog.Index(path);C(indexed.Availability.ToString()==c.GetProperty("current").GetProperty("index").GetString(),id+" index stage independent of runtime cap");
    Beatmap? map=null;string? message=null;try{map=indexed.LoadCanonicalGameplay();}catch(InvalidDataException e){message=e.Message;}
    bool expected=c.GetProperty("current").GetProperty("prepared").GetBoolean();C((map is not null)==expected,id+" actual canonical disposition");var row=audit.Charts.Single(x=>x.EntryKey==Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(id+".osu"))))!;C((row.Canonical.Status=="success")==expected,id+" actual audit canonical stage");
    string guard=c.GetProperty("current").GetProperty("guard").GetString()!;if(guard=="duration")C(message=="R05 requires each traversal >= 1 ms and parent end <= 3600000 ms.",id+" exact duration bound");if(guard=="checkpoints")C(message=="R05 accepts at most 4096 checkpoints per slider.",id+" exact checkpoint work bound");if(guard=="repeat-guard")C(message==Repeat&&row.Canonical.Code=="slider-repeat-profile"&&row.Canonical.ExceptionType=="InvalidDataException",id+" exact traversal throw/type/classifier");
    if(map is not null){admitted++;int n=map.Sliders.Single().Traversals;Geometry(map.Sliders[0],c,n,id);var score=ScoreConfiguration.FromBeatmap(map);var session=CircleSession.CreateFreshOrdinary(map,score);var health=HealthChart.FromMap(map,session.Approach,HealthPreparationPolicy.E036FreshOrdinary);
     C(session.Health is not null&&health.Objects.Count==1&&health.NewComboProvenance[0].FinalFlag&&map.HealthSource!.IsFreshFor(map),id+" actual fresh health/score/NewCombo");
     C(map.Sliders[0].Samples!.RawTraversals==n&&map.Sliders[0].Samples!.EffectiveTraversals==n&&session.Sliders[0].Definition==map.Sliders[0],id+" raw/effective and consumer identity");
     if(map.Sliders[0].Samples!.Error is null){using var fx=await library.PrepareAsync(map,default);C(ReferenceEquals(fx.PreparedMap,map)&&fx.Sliders.ContainsKey(map.Sliders[0]),id+" actual audio resource owner");}else{bool unavailable=false;try{using var fx=await library.PrepareAsync(map,default);unavailable=fx.Error is not null;}catch(InvalidDataException){unavailable=true;}C(unavailable,id+" malformed samples stay unavailable, not invented defaults");}
    }else rejected++;
    if(c.GetProperty("rawTraversals").ValueKind==JsonValueKind.Number){int n=c.GetProperty("rawTraversals").GetInt32();if(n is >=1 and <=33){try{SliderSoundMetadata.Parse(2,n,c.GetProperty("version").GetInt32(),0,null,null,null);C(n<=16,id+" independent metadata guard permits supported count");}catch(InvalidDataException e){guard2++;C(n>16&&e.Message=="R12 retains the ENGINEERING 1–16 traversal admission bound.",id+" separate sound metadata guard");}}}
    bool canProbe=c.GetProperty("probe").GetBoolean()&&c.GetProperty("id").GetString()!.IndexOf("bad-",StringComparison.Ordinal)<0;
    if(canProbe){int n=c.GetProperty("rawTraversals").GetInt32();string text=File.ReadAllText(path);var lines=text.Split('\n');int p=Array.IndexOf(lines,"[HitObjects]")+1;var cells=lines[p].Split(',');cells[6]="1";lines[p]=string.Join(',',cells);string controlPath=Path.Combine(tmp,id+"-one.osu");File.WriteAllText(controlPath,string.Join('\n',lines));var control=Beatmap.LoadCanonical(controlPath);
     long before=GC.GetAllocatedBytesForCurrentThread();var timer=Stopwatch.StartNew();var s=Component(control,c,n);timer.Stop();long bytes=GC.GetAllocatedBytesForCurrentThread()-before;componentProbes++;Geometry(s,c,n,id+" ISOLATED COMPONENT");
     var data=SoundOperands(c,n).Prepare(s);s=s with {Samples=data};C(data.Schedule.Count==s.Checkpoints.Count&&data.EdgeSubset.Count==(s.Checkpoints.Count-1)/(s.Checkpoints.Count/n),id+" actual N/R sound subset floor semantics");
     var timing=new[]{new SampleTiming(0,1,0,100,true,0),new SampleTiming(s.EndTime+4,3,2,37,false,1)};var policy=new CircleSoundPolicy(true,true,true,0,100,false,80,100);var prepared=PreparedSliderSamples.Create(s,timing,policy,-1);
     for(int edge=0;edge<=Math.Max(n,data.EdgeSubset.Count);edge++){var selected=SliderSampleSelection.Edge(s,data,timing,policy,edge);C(edge<n?selected.Volume==100:selected.Volume==37,id+" edge index/query "+edge);}
     if(data.Simple)C(data.EdgeSounds.Count==0,id+" empty sound list retained for aggregate edge-1");else C(data.EdgeSounds.Count==n+1&&data.Bases.Count==n+1,id+" raw+1 bounded sound arrays");
     // Explicit synthetic component map. It has NO canonical document/fresh-owner
     // proof; never rebind or fabricate canonical health/resource provenance.
     var probeMap=new Beatmap("isolated component",5,5,5,Array.Empty<CircleDefinition>()){Sliders=new[]{s}};var live=new CircleSession(probeMap,ScoreConfiguration.EngineeringPractice,false);live.Press(s.Head.Position,s.Head.StartTime);
     using var resourceControl=await library.PrepareAsync(control,default);var sink=new Sink();using var recording=new RecordingSampleChannels();var channels=new SharedSliderChannels(new GlobalSampleTiming(timing),recording,(kind,bank,index)=>100+kind+bank*10);
     int events=0;live.SliderSound+=e=>{events++;SliderSampleSelection.Request(e,prepared,timing,resourceControl.Resources,policy,sink);};live.ContinuingSound+=(state,t,active)=>{channels.Clock(t);if(active)channels.Submit(2,1,1,true,state.Definition.Head.Position.X);else channels.QueuePause();channels.Drain();};
     for(int t=s.Head.StartTime;t<=s.EndTime+10;t+=5)live.Advance(t,s.RequiredPosition(Math.Min(t,s.EndTime)),1);channels.QueuePause();channels.Drain();
     C(sink.Requests.Count>0&&recording.Operations.Any(x=>x.Name=="Play")&&recording.Operations.Any(x=>x.Name=="Pause"),id+" actual discrete requests and continuing recording hooks (component resources)");channels.Submit(2,1,1,true,null);channels.Cancel();C(channels.Pending==0,id+" queued audio cancellation");
     var state=live.Sliders[0];double ratio=(state.SuccessfulCheckpoints+1d)/(state.Checkpoints.Count+1d);var grade=ratio==1?Judgement.Hit300:ratio>=.5?Judgement.Hit100:ratio>0?Judgement.Hit50:Judgement.Miss;C(state.Head.Result==Judgement.Hit300&&state.Result==grade&&state.ProcessedCheckpoints<=state.Checkpoints.Count,id+" E17 actual aggregate from successes without pending drain");C(events>n,id+" distinct actual discrete audio progression");
     var hc=new HealthChart(5,1200,c.GetProperty("version").GetInt32(),new[]{new PreparedHealthObject(s.Head.StartTime,s.EndTime,ScoreObjectKind.Slider,true,data.EdgeSubset.Count,s.Checkpoints.Count)},Array.Empty<HealthBreak>());hc.Validate();var calibration=new HealthCalibration().Run(hc);C(double.IsFinite(calibration.Rate)&&calibration.Trials>0,id+" explicit constructed health component calibrates");
     bool cancel=false;using var cts=new CancellationTokenSource();cts.Cancel();try{Component(control,c,n,cts.Token);}catch(OperationCanceledException){cancel=true;}C(cancel,id+" preparation observes precancel at geometry entry");
     probes.Add(new{id,count=n,classification="ISOLATED PRODUCTION COMPONENT; canonical guard unchanged; not original runtime or admitted higher-count chart",end=s.EndTime,schedule=s.Checkpoints.Select(x=>x.Time),endpoints=new[]{s.EndPosition.X,s.EndPosition.Y},segments=s.Geometry.Segments.Count,movements=s.Movements.Count,checkpoints=s.Checkpoints.Count,subset=data.EdgeSubset.Count,sessionResult=state.Result.ToString(),state.SuccessfulCheckpoints,state.FailedCheckpoints,processed=state.ProcessedCheckpoints,pending=state.Checkpoints.Count-state.ProcessedCheckpoints,ratio,allocatedBytes=bytes,elapsedTicks=timer.ElapsedTicks,singleDistances=s.Geometry.CumulativeLengths.Select((x,i)=>(float)(x-(i==0?0:s.Geometry.CumulativeLengths[i-1]))),rate=s.TravelRate});
    }
    C(Hash(path)==hash&&File.GetLastWriteTimeUtc(path)==stamp,id+" original read-only after consumers/probes");outcomes.Add(new{id,prepared=map is not null,index=indexed.Availability.ToString(),message,audit=row.Canonical,original=c.GetProperty("original").Clone()});
   }
  }finally{Directory.Delete(tmp,true);}
  int failed=checks.Count(x=>!JsonSerializer.SerializeToElement(x).GetProperty("pass").GetBoolean());File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{source=audit.SourceCommit,state=audit.SourceState,sourceHashes=audit.SourceHashes,runtime=Environment.Version.ToString(),passed=checks.Count-failed,failed,fixtures=cases.GetArrayLength(),admitted,rejected,componentProbes,secondGuardRefusals=guard2,checks,outcomes,probes,originalRuntime="NOT RUN",largeCountSimulation="NOT RUN",higherCountCanonical="NOT IMPLEMENTED"},new JsonSerializerOptions{WriteIndented=true}));File.WriteAllText(Path.Combine(output,"audit.json"),AuditRunner.Json(audit));Console.WriteLine($"Issue17 {checks.Count-failed}/{checks.Count} PASS; {admitted} preparations/{rejected} refusals; {componentProbes} isolated components");Environment.ExitCode=failed==0?0:1;
 }
}
