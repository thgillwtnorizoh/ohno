using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Platform;
using Ohno.CorpusAudit;
partial class Program {
 static List<object> checks=new(),outcomes=new();
 static void C(bool p,string n){checks.Add(new{name=n,pass=p});if(!p)Console.WriteLine("FAIL "+n);}
 static string Hash(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p)));
 static readonly Dictionary<string,string> Guards=new(){["certificate"]="Legacy spinner stack profile lacks a zero-displacement certificate.",["break"]="Versioned spinner profile requires no breaks.",["order"]="Versioned spinner requires strictly increasing registered source order.",["interval"]="Versioned spinner first profile requires separated object intervals.",["timing"]="Versioned spinner requires six or more explicit finite timing/sample fields.",["resource"]="AudioFilename must stay within the beatmap folder."};
 static void Coordinates(Beatmap m,JsonElement c,string id){
  var actual=m.Circles.Select(x=>(t:x.StartTime,p:x.Position)).Concat(m.Sliders.Select(x=>(t:x.Head.StartTime,p:x.Head.Position))).Concat(m.Spinners.Select(x=>(t:x.StartTime,p:x.Center))).OrderBy(x=>x.t).ToArray();
  var target=c.GetProperty("original").GetProperty("preparedHeads");
  C(actual.Length==target.GetArrayLength()&&actual.Select((x,i)=>Vector2.Distance(x.p,new(target[i][0].GetSingle(),target[i][1].GetSingle()))).All(x=>x<.00004f),id+" original-derived ordinary prepared geometry");
 }
 static async Task Main(string[] args){
 string root=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]),home=Path.Combine(root,"re/campaigns/C001/near-spinner"),fixtures=Path.Combine(home,"fixtures");Directory.CreateDirectory(output);
 var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"manifest.json"))).RootElement.GetProperty("cases");
 var pins=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,"re/campaigns/C001/near-spinner-qa/expected-sources.json"))).RootElement;
 var audit=AuditRunner.Run(fixtures);C(audit.SourceCommit==pins.GetProperty("source").GetString()&&audit.SourceState=="clean","compiled exact clean source stamp");
 foreach(var p in pins.GetProperty("hashes").EnumerateObject().Where(p=>p.Name!="LocalHitSoundAssets.cs"))C(audit.SourceHashes.GetValueOrDefault(p.Name)==p.Value.GetString(),"compiled source "+p.Name);
 C(Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().Single(x=>x.Key=="AssetHash").Value==pins.GetProperty("hashes").GetProperty("LocalHitSoundAssets.cs").GetString(),"compiled actual audio resource adapter");
 C(audit.Complete&&audit.Charts.Count==manifest.GetArrayLength(),"complete synthetic denominator");C(AuditRunner.Json(audit)==AuditRunner.Json(AuditRunner.Run(fixtures)),"deterministic audit");
 string temp=Path.Combine(Path.GetTempPath(),"ohno-i13-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
 int prepared=0,rejected=0,controls=0;
 try {
  string pack=Path.Combine(temp,"samples");Directory.CreateDirectory(pack);
  foreach(string name in new[]{"spinnerspin","spinnerbonus","normal-hitnormal","normal-hitwhistle"}){using var w=new BinaryWriter(File.Create(Path.Combine(pack,name+".wav")));w.Write("RIFF"u8.ToArray());w.Write(292);w.Write("WAVEfmt "u8.ToArray());w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(8000);w.Write(16000);w.Write((short)2);w.Write((short)16);w.Write("data"u8.ToArray());w.Write(256);w.Write(new byte[256]);}
  using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});
  async Task Consume(Beatmap m,JsonElement c,string id,bool compare){
   var score=ScoreConfiguration.FromBeatmap(m);var session=CircleSession.CreateFreshOrdinary(m,score);var health=HealthChart.FromMap(m,session.Approach,HealthPreparationPolicy.E036FreshOrdinary);
   C(session.Health is not null&&m.HealthSource!.IsFreshFor(m)&&health.Objects.Count==session.ObjectCount,id+" real fresh score health session");
   C(m.Spinners.All(s=>s.Center==new Vector2(256,192)&&session.Spinners.Any(x=>ReferenceEquals(x.Definition,s))&&m.CircleSounds!.Spinners.Any(x=>ReferenceEquals(x.Object,s))),id+" fixed spinner center and shared sample/session identity");
   C(m.Circles.All(s=>m.CircleSounds!.Circles.Any(x=>ReferenceEquals(x.Object,s)))&&m.Sliders.All(s=>m.CircleSounds!.RegisteredOverrides.Any(x=>ReferenceEquals(x.Slider,s))),id+" sound owners match prepared ordinary objects");
   C(m.Spinners.All(s=>s.Preparation!.CompatibilityPolicy==SpinnerCompatibilityPolicy.ModernWithoutRecordedSource&&s.Preparation.EndpointPolicy==SpinnerEndpointPolicy.EngineeringUncappedHostLengthUnavailable),id+" source policy and host cap remain explicit");
   // E036 fresh no-break flags derive from original registration type transitions.
   var flags=new List<bool>();string? previous=null;
   foreach(var k in c.GetProperty("kinds").EnumerateArray().Select(x=>x.GetString()!)){flags.Add(k!="spinner"&&(previous is null||previous=="spinner"));previous=k;}
   C(health.NewComboProvenance.Select(x=>x.FinalFlag).SequenceEqual(flags),id+" independent no-break registration NewCombo");
   C(HitSoundLibrary.SpinnerAdmission(m) is null,id+" actual audio admission");
   using var effects=await library.PrepareAsync(m,default);
   C(ReferenceEquals(effects.PreparedMap,m)&&effects.Spinners.Count==m.Spinners.Count&&effects.Sliders.Count==m.Sliders.Count&&effects.SpinHandle>0&&effects.BonusHandle>0,id+" real resource preparation with synthetic WAV stubs");
   foreach(var s in m.Sliders)C(effects.Sliders.ContainsKey(s)&&s.Geometry.SourceControls.Count>0&&s.Movements.All(x=>float.IsFinite(x.From.X)&&float.IsFinite(x.To.X)),id+" slider path and resource owner");
   if(compare)Coordinates(m,c,id);
  }
  foreach(var c in manifest.EnumerateArray()){
   string id=c.GetProperty("id").GetString()!,p=Path.Combine(fixtures,id+".osu");string before=Hash(p);var stamp=File.GetLastWriteTimeUtc(p);
   C(before.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable fixture hash");
   var index=LocalChartCatalog.Index(p);C(index.Availability==ChartAvailability.Indexed,id+" catalog candidate separate from canonical");
   Beatmap? map=null;string? message=null;try{map=index.LoadCanonicalGameplay();}catch(InvalidDataException e){message=e.Message;}
   bool wanted=Positive(c);C(wanted==c.GetProperty("handoff").GetProperty("prepared").GetBoolean(),id+" independent versioned expectation");C(!c.GetProperty("current").GetProperty("prepared").GetBoolean()||wanted,id+" no previous admission loss");C((map is not null)==wanted,id+" current canonical expected disposition");
   Beatmap? direct=null;try{direct=Beatmap.LoadCanonical(p);}catch(InvalidDataException){}C((direct is not null)==wanted,id+" direct canonical agrees");
   string guard=c.GetProperty("current").GetProperty("guard").GetString()!;if(!wanted)C(message==Guards[guard],id+" exact reached guard "+guard+" observed "+message);
   var row=audit.Charts.Single(x=>x.EntryKey==Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(id+".osu"))))!;
   C((row.Canonical.Status=="success")==wanted,id+" audit canonical agrees");if(!wanted&&guard=="certificate")C(row.Canonical.Code=="stack-certificate-unproven"&&row.Canonical.ExceptionType=="InvalidDataException",id+" actual RouteZ classifier");
   if(map is not null){prepared++;C(map.Document!.SourceSha256==before&&map.MetadataSettings!.StackLeniency==c.GetProperty("setting").GetSingle(),id+" retain source and leniency");await Consume(map,c,id,true);}else rejected++;
   // These temporary controls qualify connected consumers ONLY for literal no-displacement
   // handoff positives. No original fixture edit, no algorithm patch, no compatibility claim.
   bool target=c.GetProperty("handoff").GetProperty("extension").GetBoolean();
   if(target){string text=File.ReadAllText(p);text=text.Contains("StackLeniency:")?text.Replace("StackLeniency:0.7","StackLeniency:0"):text.Replace("Mode:0\n","Mode:0\nStackLeniency:0\n");string cp=Path.Combine(temp,id+".osu");File.WriteAllText(cp,text);var control=LocalChartCatalog.Index(cp).LoadCanonicalGameplay();await Consume(control,c,id+" zero-setting consumer control",true);C(map is not null&&JsonSerializer.Serialize(map.Sliders,new JsonSerializerOptions{IncludeFields=true})==JsonSerializer.Serialize(control.Sliders,new JsonSerializerOptions{IncludeFields=true}),id+" full prepared slider equality with zero control");controls++;}
   C(Hash(p)==before&&File.GetLastWriteTimeUtc(p)==stamp,id+" read-only bytes and timestamp");
   outcomes.Add(new{id,index=index.Availability.ToString(),prepared=map is not null,message,route=c.GetProperty("route").GetString(),audit=row.Canonical,original=c.GetProperty("original").Clone(),handoff=c.GetProperty("handoff").Clone(),temporaryConsumerControl=target});
  }
  // Actual admitted RouteM input uses displaced geometry; no original-runtime claim.
  var inputMap=Beatmap.LoadCanonical(Path.Combine(fixtures,"v06-pair-after.osu"));
  var live=new CircleSession(inputMap,ScoreConfiguration.EngineeringPractice,false);
  live.Advance(1150,Vector2.Zero,0);var head=live.Circles[0];
  var pointer=head.Definition.Position-new Vector2(live.HitRadius-.01f,0);
  C(Vector2.Distance(pointer,new(128,128))>live.HitRadius,"input probe discriminates raw vs displaced head");
  live.Press(pointer,head.Definition.StartTime);C(head.Result==Judgement.Hit300,"real input reaches translated RouteM circle");
  // Spinner-first positive has a slider target: after spinner expires the real
  // parent/head session accepts its head. Before expiry it remains blocked.
  string safeText=File.ReadAllText(Path.Combine(fixtures,"v05-spinner-slider.osu"));
  string safePath=Path.Combine(temp,"safe-input.osu");File.WriteAllText(safePath,safeText);
  var safe=Beatmap.LoadCanonical(safePath);var sliderLive=new CircleSession(safe,ScoreConfiguration.EngineeringPractice,false);
  sliderLive.Press(new(256,192),1090);C(sliderLive.Sliders[0].Head.Result==Judgement.Pending,"actual positive-control slider head blocked while spinner pending");
  sliderLive.Advance(1150,Vector2.Zero,0);sliderLive.Press(new(256,192),1200);C(sliderLive.Sliders[0].Head.Result==Judgement.Hit300,"actual positive-control slider head accepts after spinner end");
  await Extra(temp,fixtures,library);
  // Actual source fingerprint and local resource protections remain active.
  string baseText=File.ReadAllText(Path.Combine(fixtures,"v05-no-neighbor.osu")),changed=Path.Combine(temp,"changed.osu");File.WriteAllText(changed,baseText);var stale=LocalChartCatalog.Index(changed);File.AppendAllText(changed,"\n// mutation\n");bool mutation=false;try{stale.LoadCanonicalGameplay();}catch(Exception e){mutation=e.Message.Contains("changed since indexing",StringComparison.Ordinal);}C(mutation,"actual catalog fingerprint guard");
  var files=new LocalSampleFiles(temp);bool traversal=false;try{files.Resolve("../escape.wav");}catch(InvalidDataException){traversal=true;}C(traversal&&files.Read("missing.wav") is null,"actual sample traversal and missing-file boundaries");
  using var cancelled=new CancellationTokenSource();cancelled.Cancel();bool cancel=false;try{Beatmap.LoadCanonical(Path.Combine(fixtures,"v05-no-neighbor.osu"),cancelled.Token);}catch(OperationCanceledException){cancel=true;}C(cancel,"actual canonical cancellation");
 }finally{Directory.Delete(temp,true);}
 int failed=checks.Count(x=>!JsonSerializer.SerializeToElement(x).GetProperty("pass").GetBoolean());
 File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{source=audit.SourceCommit,state=audit.SourceState,sourceHashes=audit.SourceHashes,runtime=Environment.Version.ToString(),passed=checks.Count-failed,failed,fixtures=manifest.GetArrayLength(),prepared,rejected,zeroSettingConsumerControls=controls,checks,outcomes,originalRuntime="NOT RUN",audibleOutput="NOT RUN",qualification="independent actual new-production component QA"},new JsonSerializerOptions{WriteIndented=true}));
 File.WriteAllText(Path.Combine(output,"audit.json"),AuditRunner.Json(audit));Console.WriteLine($"Issue15 independent QA {checks.Count-failed}/{checks.Count} PASS; {prepared} prepared; {rejected} rejected; {controls} consumer controls");Environment.ExitCode=failed==0?0:1;
 }
}
