using System.Numerics;
using System.Reflection;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Platform;
using Ohno.CorpusAudit;
partial class Program {
 static readonly HashSet<string> Added=new(){"v05-spinner-slider","v05-slider-only-B","v05-slider-only-P","v05-slider-only-C","v05-slider-only-cs5-ar5-default","v05-slider-only-cs5-ar5-explicit","v05-slider-only-cs4-ar8-default","v05-slider-only-cs4-ar8-explicit","v05-slider-only-cs7-ar3-default","v05-slider-only-cs7-ar3-explicit","v05-slider-only-two-spinners","v05-slider-only-far-before"};
 static bool Positive(JsonElement c)=>c.GetProperty("current").GetProperty("prepared").GetBoolean()||Added.Contains(c.GetProperty("id").GetString()!);
 static async Task Extra(string temp,string fixtures,HitSoundLibrary library){
  string template=File.ReadAllText(Path.Combine(fixtures,"v05-spinner-slider.osu"));
  Beatmap Load(string id,string text){string p=Path.Combine(temp,id+".osu");File.WriteAllText(p,text);return Beatmap.LoadCanonical(p);}
  bool Refuse(string id,string text,string reason="zero-displacement certificate"){try{Load(id,text);return false;}catch(InvalidDataException e){return e.Message.Contains(reason,StringComparison.Ordinal);}}
  C(Refuse("far-circle",template+"400,300,3000,1,0\n"),"a far registered circle disables exemption");
  foreach(int v in new[]{4,6,9,12,13}){
   string text=template.Replace("format v5","format v"+v).Replace("CircleSize:5","CircleSize:4");
   C(Refuse("version-"+v,text,v==4?"format":"zero-displacement certificate"),"outside version5 retained RouteZ guard "+v);
  }
  foreach(string scalar in new[]{"0.1","0.33","1"}){var m=Load("scalar"+scalar,template.Replace("Mode:0\n","Mode:0\nStackLeniency:"+scalar+"\n"));C(m.MetadataSettings!.StackLeniency==float.Parse(scalar,System.Globalization.CultureInfo.InvariantCulture)&&m.Circles.Count==0&&m.Sliders.Count==1,"genuine positive scalar "+scalar);}
  string slider="256,192,1200,2,0,L|356:192,1,100";
  string noSlider=template.Replace(slider,"7,11,1200,8,2,1300");C(Refuse("all-spin",noSlider),"no-slider multi-spinner profile stays excluded");
  C(Load("single-spin",template.Replace(slider+"\n","")).Spinners.Count==1,"isolated single-spinner old RouteZ admission");
  // Retained earlier-slider comparisons: raw head, geometric endpoint and even
  // traversal endpoint. All timestamps are disjoint; spinner file xy is irrelevant.
  string prefix=template[..(template.IndexOf("[HitObjects]",StringComparison.Ordinal)+13)]+"\n";
  foreach(var (id,row) in new[]{("head","256,192,1000,2,0,L|356:192,1,100"),("tail","128,192,1000,2,0,L|256:192,1,128"),("even-geo","128,192,1000,2,0,L|256:192,2,128"),("even-final","256,192,1000,2,0,L|356:192,2,100")})
   C(Refuse(id,prefix+row+"\n7,11,3000,8,2,3100\n"),"earlier slider retained "+id);
  foreach(var (label,x,y,accept) in new[]{("inside",254,190,false),("equal",253,192,true),("outside",253,191,true)}){
   string text=prefix+$"128,128,1000,2,0,L|{x}:{y},1,{Math.Sqrt((x-128)*(x-128)+(y-128)*(y-128)).ToString("R",System.Globalization.CultureInfo.InvariantCulture)}\n7,11,3000,8,2,3100\n";
   C(Refuse("distance-"+label,text)==!accept,"retained endpoint strict distance "+label);
  }
  // Actual all-object work boundaries; almost all pairs would be exempt. A far
  // slider comes last, so it cannot introduce an ordinary-origin relationship.
  string Many(int count)=>prefix+string.Join('\n',Enumerable.Range(0,count-1).Select(i=>$"7,11,{100+3*i},8,0,{101+3*i}"))+"\n64,64,10000,2,0,L|114:64,1,50\n";
  C(Load("pairs-under",Many(1414)).Spinners.Count==1413,"1414 objects998991 pairs with exemptions");
  C(Refuse("pairs-over",Many(1415),"pair work limit"),"1415 objects1000405 pairs counted before exemptions");
  var core=typeof(Beatmap).GetMethod("CertifyLegacySpinnerZeroDisplacementCore",BindingFlags.NonPublic|BindingFlags.Static)!;
  var operands=Enumerable.Range(0,1414).Select(i=>new SamplePreparationOperand(new(new(8*(i%64),8*(i/64)),100+i),0,"",null)).ToList();
  using var cancel=new System.Threading.CancellationTokenSource();cancel.CancelAfter(1);bool cancelled=false;
  try{core.Invoke(null,new object[]{operands,cancel.Token,true});}catch(TargetInvocationException e)when(e.InnerException is OperationCanceledException){cancelled=true;}
  C(cancelled,"core observes cancellation during bounded inner scan");
  using var pre=new System.Threading.CancellationTokenSource();pre.Cancel();cancelled=false;try{core.Invoke(null,new object[]{new List<SamplePreparationOperand>(),pre.Token,true});}catch(TargetInvocationException e)when(e.InnerException is OperationCanceledException){cancelled=true;}C(cancelled,"empty/exempt core checks entry cancellation");
  // Whole real session with recording channels on actual new .7 map, not control.
  var liveMap=Load("live",template);using var fx=await library.PrepareAsync(liveMap,default);
  var backend=new RecordingSampleChannels();var queue=new Queue<Action>();var channel=new SharedSpinnerChannel(backend,()=>fx.SpinHandle,()=>1,()=>1,a=>queue.Enqueue(a));channel.Refresh();
  var live=CircleSession.CreateFreshOrdinary(liveMap,ScoreConfiguration.FromBeatmap(liveMap),false);
  live.SpinnerSound+=(s,k)=>{if(k==SpinnerSoundStage.Motion)channel.Continue(null);if(k==SpinnerSoundStage.Cleanup)channel.QueuePause();};
  for(int t=980;t<=1800;t+=5){double a=.1*(t-980);live.Advance(t,new(256+70*(float)Math.Cos(a),192+70*(float)Math.Sin(a)),1);if(t==1200)live.Press(new(256,192),t);while(queue.Count>0)queue.Dequeue()();}
  C(live.Sliders[0].Head.Result==Judgement.Hit300&&live.Spinners[0].Result!=Judgement.Pending,"actual new source session spinner final and slider head");
  C(backend.Operations.Any(x=>x.Name=="Play")&&backend.Operations.Any(x=>x.Name=="Pause"),"actual spinner motion cleanup recording hooks");
  var completion=prefix+"7,11,1000,8,2,1500\n256,192,1700,2,0,L|356:192,1,100\n";
  completion=completion.Replace("[HitObjects]","1499,-50,4,1,0,37,0,0\n[HitObjects]");var endMap=Load("end-sample",completion);using var endFx=await library.PrepareAsync(endMap,default);SelectedCircleSample? selected=null;var sink=new Sink();
  SpinnerSampleSelection.Completion(endFx.Spinners[endMap.Spinners[0]],endMap.CircleSounds!.Timing,new(256,192),endFx.Resources,new(true,true,true,0,100,false,80,100),sink,x=>selected=x);
  C(selected is {Volume:37}&&sink.Requests.Count>0&&sink.Requests.All(x=>ReferenceEquals(x.Spinner,endMap.Spinners[0])),"stored end sample selection and real owner tags");
  string root=Path.GetFullPath(Path.Combine(fixtures,"../../../../.."));
  var historical=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,"re/campaigns/C001/legacy-spinner-profile/manifest.json"))).RootElement.GetProperty("cases");
  foreach(var c in historical.EnumerateArray()){
   string id=c.GetProperty("id").GetString()!,path=Path.Combine(root,"re/campaigns/C001/legacy-spinner-profile",c.GetProperty("path").GetString()!);
   bool expected=c.GetProperty("timingColumns").GetInt32()>=6&&!c.GetProperty("breaks").GetBoolean()&&c.GetProperty("sourceOrder").GetBoolean()&&c.GetProperty("shape").GetString()!="overlap"&&!(c.GetProperty("version").GetInt32()==5&&new[]{"circle-pair","spinner-before","spinner-after","even-tail"}.Contains(c.GetProperty("shape").GetString()));
   bool ok=true;try{var m=Beatmap.LoadCanonical(path);CircleSession.CreateFreshOrdinary(m,ScoreConfiguration.FromBeatmap(m));}catch(InvalidDataException){ok=false;}
   C(ok==expected,"Issue9 original finite contract "+id);C(Hash(path).Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),"Issue9 original immutable "+id);
  }
  var excluded=new HashSet<string>{"v05-fractional","v13-fractional","v13-end-before-start","v13-zero-duration","v09-invalid-end","v14-missing-end","negative-format-3","negative-format-128","negative-mode1-v9","negative-mode1-v14"};
  foreach(var c in JsonDocument.Parse(File.ReadAllText(Path.Combine(root,"re/campaigns/C001/versioned-spinners/manifest.json"))).RootElement.GetProperty("cases").EnumerateArray()){
   string id=c.GetProperty("id").GetString()!,path=Path.Combine(root,"re/campaigns/C001/versioned-spinners",c.GetProperty("path").GetString()!);bool ok=true;try{Beatmap.LoadCanonical(path);}catch(InvalidDataException){ok=false;}
   C(ok==!excluded.Contains(id),"Issue7 retained individual admission "+id);C(Hash(path).Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),"Issue7 immutable "+id);
  }
  // Source-unchanged Issue11 fixed type/message taxonomy, including adversaries.
  var failure=typeof(AuditRunner).GetMethod("Failure",BindingFlags.Static|BindingFlags.NonPublic)!;
  foreach(var (message,code) in new[]{("Legacy spinner stack profile lacks a zero-displacement certificate.","stack-certificate-unproven"),("Legacy spinner zero-displacement certificate exceeds pair work limit.","stack-certificate-work-limit")}){
   var outcome=(Outcome)failure.Invoke(null,new object[]{new InvalidDataException(message)})!;C(outcome.Code==code&&outcome.ExceptionType=="InvalidDataException","Issue11 precise mapping "+code);
   foreach(var extra in new[]{" ","x","\n"}){var other=(Outcome)failure.Invoke(null,new object[]{new InvalidDataException(message+extra)})!;C(other.Code!=code,"Issue11 no suffix spoof "+code+extra);}
  }
  C(Refuse("string-old",template,"never-match")==false,"canonical positive remains available");
  bool legacy=false;try{Beatmap.Load(Path.Combine(fixtures,"v05-spinner-slider.osu"));}catch(InvalidDataException){legacy=true;}C(legacy,"historical string loader v5 exclusion unchanged");
 }
 sealed class Sink:ISamplePlaybackSink{public List<SamplePlaybackRequest> Requests=new();public void Enqueue(SamplePlaybackRequest r)=>Requests.Add(r);}
}
