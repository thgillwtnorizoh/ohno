using System.Numerics;
using System.Security.Cryptography;
using System.Reflection;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Platform;
using Ohno.CorpusAudit;

class Program {
 static List<object> tests=new();static List<object> dispositions=new();
 static void C(bool ok,string name){tests.Add(new{name,pass=ok});if(!ok)Console.WriteLine("FAIL "+name);}
 static bool Reject(Action action){try{action();return false;}catch(InvalidDataException){return true;}}
 static string Hash(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p)));
 static async Task Main(string[] args){
 string repo=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);Directory.CreateDirectory(output);
 string research=Path.Combine(repo,"re/campaigns/C001/versioned-spinners"),qa=Path.Combine(repo,"re/campaigns/C001/versioned-spinner-qa");
 var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(research,"manifest.json"))).RootElement;
 var pin=JsonDocument.Parse(File.ReadAllText(Path.Combine(qa,"expected-sources.json"))).RootElement;
 var audit=AuditRunner.Run(Path.Combine(research,"fixtures"));
 C(audit.SourceCommit=="2a2e29177d5f1173c711cf3d12d70be2f1759e09"&&audit.SourceState=="clean","exact clean compiled source identity");
 foreach(var p in pin.GetProperty("hashes").EnumerateObject().Where(p=>p.Name!="LocalHitSoundAssets.cs"))C(audit.SourceHashes.GetValueOrDefault(p.Name)==p.Value.GetString(),"compiled hash "+p.Name);
 C(Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().Single(a=>a.Key=="AssetHash").Value==pin.GetProperty("hashes").GetProperty("LocalHitSoundAssets.cs").GetString(),"compiled asset integration hash");
 C(audit.Complete&&audit.Charts.Count==33,"complete actual audit 33 immutable occurrences");
 int admissions=0;var rejected=new HashSet<string>{"v05-fractional","v13-fractional","v13-end-before-start","v13-zero-duration","v09-invalid-end","v14-missing-end","negative-format-3","negative-format-128","negative-mode1-v9","negative-mode1-v14"};
 foreach(var fixture in manifest.GetProperty("cases").EnumerateArray()){
  string id=fixture.GetProperty("id").GetString()!,path=Path.Combine(research,fixture.GetProperty("path").GetString()!);var e=fixture.GetProperty("expected");int version=fixture.GetProperty("version").GetInt32();
  C(Hash(path).Equals(fixture.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable hash");
  var cat=LocalChartCatalog.Index(path);Beatmap? map=null,direct=null;string? failure=null;
  try{map=cat.LoadCanonicalGameplay();}catch(InvalidDataException x){failure=x.Message;}
  try{direct=Beatmap.LoadCanonical(path);}catch(InvalidDataException){}
  bool expected=!rejected.Contains(id);C((map is not null)==expected&&(direct is not null)==expected,id+" catalog/direct canonical disposition");
  if(expected){
   admissions++;var m=map!;var spinner=m.Spinners.Single();var provenance=spinner.Preparation!;var definition=direct!.Spinners.Single();
   C(cat.Availability==ChartAvailability.Indexed&&cat.Metadata!.Format==version,id+" indexed original format");
   C(m.Document!.FormatVersion==version&&m.HealthSource!.Version==version&&m.CircleSounds!.Format==version,id+" retained source versions");
   C(spinner.StartTime==2000&&spinner.EndTime==5000&&spinner.Center==new Vector2(256,192)&&definition==spinner,id+" integer construction independent of raw XY");
   C(provenance.ConstructorFlags==(version<=8?12:8)&&provenance.ParserFlags==(fixture.GetProperty("fileBit4").GetBoolean()?12:8)&&provenance.RawType==provenance.ParserFlags,id+" constructor then parser overwrite");
   C(provenance.EndpointPolicy==SpinnerEndpointPolicy.EngineeringUncappedHostLengthUnavailable&&provenance.CompatibilityPolicy==SpinnerCompatibilityPolicy.ModernWithoutRecordedSource,id+" explicit uncapped modern policy");
   if(id=="v13-conditional-end-cap")C(e.GetProperty("end").GetInt32()==3950&&spinner.EndTime==5000,id+" conditional original cap distinct from actual uncapped endpoint");
   var payload=m.CircleSounds!.Spinners.Single();var sample=e.GetProperty("storedSample");
   C(ReferenceEquals(payload.Object,spinner)&&payload.Payload.Object.StartTime==5000&&payload.Payload.StoredMask==2&&spinner.HitSound==2,id+" raw sound and end-time sample ownership");
   C(payload.Payload.BaseSet==sample.GetProperty("base").GetInt32()&&payload.Payload.AdditionSet==sample.GetProperty("addition").GetInt32()&&payload.Payload.Index==sample.GetProperty("index").GetInt32()&&payload.Payload.Volume==sample.GetProperty("volume").GetInt32()&&payload.Payload.Filename==sample.GetProperty("filename").GetString(),id+" independently stored sample operands");
   C(HitSoundLibrary.SpinnerAdmission(m) is null,id+" actual audio admission");
   var state=new SpinnerState(spinner,m.OverallDifficulty);C(state.Required==15&&Math.Abs(state.Acceleration-27.0/25000)<1e-15,id+" rational 3s OD5 requirement and acceleration");
   state.SampleAngle(1.25,2000,17,true);C(state.TargetMotion==0&&state.PreviousAngle==1.25,id+" old version retains modern first-angle reference");
   var score=ScoreConfiguration.FromBeatmap(m);var session=CircleSession.CreateFreshOrdinary(m,score);
   var health=HealthChart.FromMap(m,session.Approach,HealthPreparationPolicy.E036FreshOrdinary);
   C(m.HealthSource!.IsFreshFor(m)&&ReferenceEquals(session.Map,m)&&ReferenceEquals(session.Spinners.Single().Definition,spinner)&&session.Health is not null,id+" calibrated real fresh session identity");
   C(health.NewComboProvenance.Select(p=>p.FinalFlag).SequenceEqual(e.GetProperty("finalNoBreakFlags").EnumerateArray().Select(x=>x.GetBoolean())),id+" original-derived final NewCombo sequence");
   var hp=health.NewComboProvenance.Single(p=>p.RawType is 8 or 12);C(hp.ConstructorFlag==(version<=8)&&hp.ParserFlag==fixture.GetProperty("fileBit4").GetBoolean(),id+" real health constructor/parser provenance");
   C(score.Policy==ScorePolicy.PlainLegacy&&double.IsFinite(score.K)&&score.MultiplierProvenance.StartsWith("AUTO E034")&&health.Objects.Single(o=>o.Kind==ScoreObjectKind.Spinner).SpinnerRequirement==15,id+" score and health prepared spinner count");
   // Force no input: completion must miss, update actual score source and deliver cleanup.
   var miss=CircleSession.CreateFreshOrdinary(m,score,false);var events=new List<SpinnerSoundStage>();miss.SpinnerSound+=(s,stage)=>{C(ReferenceEquals(s.Definition,spinner),id+" event prepared identity");events.Add(stage);};miss.Advance(5001);
   C(miss.Spinners.Single().Result==Judgement.Miss&&events.Contains(SpinnerSoundStage.Cleanup),id+" no-input result and cleanup hook");
   if(version<14)C(Reject(()=>Beatmap.Load(path)),id+" historical string loader remains excluded");else C(Beatmap.Load(path).Spinners.Count==1,id+" historical v14 spinner remains admitted");
  }else C(map is null&&failure is not null,id+" finite engineering exclusion retained");
  dispositions.Add(new{id,index=cat.Availability.ToString(),prepared=expected,actualPrepared=map is not null,failure});
  C(Hash(path).Equals(fixture.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" source unchanged after consumers");
 }
 C(admissions==23,"23 finite admissions / 10 exclusions");
 string temp=Path.Combine(Path.GetTempPath(),"ohno-independent-spinner-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
 try{
 string source=File.ReadAllText(Path.Combine(research,"fixtures/v09-single.osu"));
 void Boundary(string id,string text,bool index,bool canonical){string path=Path.Combine(temp,id+".osu");File.WriteAllText(path,text);var cat=LocalChartCatalog.Index(path);C((cat.Availability==ChartAvailability.Indexed)==index,id+" index boundary");C(!Reject(()=>cat.LoadCanonicalGameplay())==canonical,id+" canonical boundary");}
 foreach(int v in new[]{7,8,11})Boundary("additional-version-"+v,source.Replace("format v9","format v"+v),true,true);
 var edge=Beatmap.LoadCanonical(Path.Combine(temp,"additional-version-8.osu")).Spinners.Single();C(edge.Preparation is {ConstructorFlags:12,ParserFlags:8},"v8 constructor boundary overwritten before registration");
 Boundary("explicit-stack07",source.Replace("StackLeniency:0","StackLeniency:0.7"),false,false);
 Boundary("implicit-stack07",source.Replace("StackLeniency:0\n",""),false,false);
 Boundary("break",source.Replace("[HitObjects]","[Events]\n2,500,1000\n[HitObjects]"),true,false);
 Boundary("malformed-sample",source.Replace(",8,2,5000",",8,2,5000,abc:0"),true,false);
 Boundary("short-sample",source.Replace(",8,2,5000",",8,2,5000,2"),true,false);
 Boundary("empty-sample",source.Replace(",8,2,5000",",8,2,5000,"),true,false);
 Boundary("missing-timing-operands",source.Replace("0,500,4,1,0,100,1,0","0,500"),true,false);
 Boundary("overlap",source+"64,64,3000,1,0\n",true,false);
 Boundary("shared-end",source+"64,64,5000,1,0\n",true,false);
 Boundary("registration-order",source.Replace("11,23,2000,8,2,5000","64,64,6000,1,0\n11,23,2000,8,2,5000"),true,false);
 Boundary("start-zero",source.Replace("2000,8,2","0,8,2"),true,false);
 // Actual end-time lookup selects the later timing row; keep original start row unchanged.
 string timed=source.Replace("[HitObjects]","4999,500,4,3,2,37,1,0\n[HitObjects]");string soundPath=Path.Combine(temp,"sound.osu");File.WriteAllText(soundPath,timed);var soundMap=Beatmap.LoadCanonical(soundPath);var spin=soundMap.Spinners.Single();
 string pack=Path.Combine(temp,"pack");Directory.CreateDirectory(pack);
 foreach(string name in new[]{"spinnerspin","spinnerbonus","drum-hitnormal","drum-hitwhistle"}){using var w=new BinaryWriter(File.Create(Path.Combine(pack,name+".wav")));w.Write("RIFF"u8.ToArray());w.Write(292);w.Write("WAVEfmt "u8.ToArray());w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(8000);w.Write(16000);w.Write((short)2);w.Write((short)16);w.Write("data"u8.ToArray());w.Write(256);w.Write(new byte[256]);}
 using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});using var effects=await library.PrepareAsync(soundMap,default);
 C(effects.SpinnerLimitation is null&&effects.Spinners.ContainsKey(spin)&&ReferenceEquals(effects.PreparedMap,soundMap)&&effects.SpinHandle>0&&effects.BonusHandle>0,"actual finite old spinner resource admission and identity");
 var sink=new Sink();SelectedCircleSample? selection=null;var policy=new CircleSoundPolicy(true,true,true,0,100,false,80,100);
 SpinnerSampleSelection.Completion(effects.Spinners[spin],soundMap.CircleSounds!.Timing,new(256,192),effects.Resources,policy,sink,s=>selection=s);
 C(selection is {BaseSet:3,Index:2,Volume:37},"completion selects stored end timing row rather than start");
 C(sink.Requests.Count>0&&sink.Requests.All(r=>ReferenceEquals(r.Spinner,spin)&&r.SpinnerKind==SoundEventKind.SpinnerCompletion),"completion requests retain spinner identity");
 var channels=new RecordingSampleChannels();var queue=new Queue<Action>();var shared=new SharedSpinnerChannel(channels,()=>effects.SpinHandle,()=>1f,()=>1f,a=>queue.Enqueue(a));shared.Refresh();
 var live=CircleSession.CreateFreshOrdinary(soundMap,ScoreConfiguration.FromBeatmap(soundMap),false);var stages=new List<SpinnerSoundStage>();live.SpinnerSound+=(s,stage)=>{stages.Add(stage);if(stage==SpinnerSoundStage.Motion)shared.Continue(null);if(stage==SpinnerSoundStage.Cleanup)shared.QueuePause();};
 for(int t=1980;t<=5020;t+=20){double angle=(t-1980)*0.04;live.Advance(t,new(256+80*(float)Math.Cos(angle),192+80*(float)Math.Sin(angle)),1);while(queue.Count>0)queue.Dequeue()();}
 C(stages.Contains(SpinnerSoundStage.Motion)&&channels.Operations.Any(o=>o.Name=="Play")&&channels.Operations.Any(o=>o.Name=="Pause"),"session input movement reaches real recording channel hooks");
 C(live.Spinners.Single().Result==Judgement.Hit300&&live.JudgedCount==1&&live.Ledger.LastEvent is {Kind:ScoreEventKind.SpinnerFinal,Raw:1024},"input completion reaches actual score ledger/result");
 C(live.Spinners.Single().PreviousAngle is not null&&live.Spinners.Single().Progress>0,"actual session input accumulates spinner progress");
 }finally{Directory.Delete(temp,true);}
 int fail=tests.Count(x=>!JsonSerializer.SerializeToElement(x).GetProperty("pass").GetBoolean());
 File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{source=audit.SourceCommit,sourceState=audit.SourceState,sourceHashes=audit.SourceHashes,runtime=Environment.Version.ToString(),passed=tests.Count-fail,failed=fail,admissions,exclusions=33-admissions,tests,dispositions,originalRuntime="NOT RUN",audibleOutput="NOT RUN"},new JsonSerializerOptions{WriteIndented=true}));
 Console.WriteLine($"Independent versioned spinner checks: {tests.Count-fail}/{tests.Count} PASS; {fail} FAIL");Environment.ExitCode=fail==0?0:1;
 }
 sealed class Sink:ISamplePlaybackSink{public List<SamplePlaybackRequest> Requests=new();public void Enqueue(SamplePlaybackRequest request)=>Requests.Add(request);}
}
