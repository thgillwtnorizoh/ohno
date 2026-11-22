using System.Numerics;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
partial class Program {
 static string Prefix(double tick=.5,double beat=500)=>$"osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:{tick.ToString(CultureInfo.InvariantCulture)}\n[TimingPoints]\n0,{beat.ToString(CultureInfo.InvariantCulture)},4,1,0,100,1,0\n[HitObjects]\n";
 static string Path96(bool longLast=false,int last=1728)=>"L|"+string.Join('|',Enumerable.Range(longLast?0:1,longLast?96:95).Select(i=>longLast?(i==95?$"{last}:192":"128:192"):$"{128+2*i}:192"));
 static string Row(int start,int n=32,double length=190,string? path=null)=>$"128,192,{start},6,2,{path??Path96()},{n},{length.ToString(CultureInfo.InvariantCulture)},2|8,1:0|2:0,0:0:0:100:\n";
 static Beatmap Load(string name,string text){string path=System.IO.Path.Combine(temp,name+".osu");File.WriteAllText(path,text);File.WriteAllText(System.IO.Path.Combine(generated,name+".osu"),text);return Beatmap.LoadCanonical(path);}
 static Beatmap ObserveLoad(string path,CancellationToken token,Action<string,int,int> observe)=>(Beatmap)Call("LoadCore",path,token,true,observe)!;
 static async Task Stress(string root,HitSoundLibrary library){
  C(SliderGeometryPreparation.MaxControls==64,"shared public MaxControls remains64");
  var ninetySix=Enumerable.Range(0,96).Select(i=>new Vector2(128+2*i,192)).ToArray();
  C(Refuse(()=>SliderGeometryPreparation.FlattenBezier(ninetySix))=="R06 flattener requires 1–64 finite controls.","public Bezier flattener unchanged64");
  C(Refuse(()=>SliderSoundMetadata.Parse(2,17,14,0,null,null,null))=="R12 retains the ENGINEERING 1–16 traversal admission bound.","public metadata17 refusal");
  foreach(var kind in new[]{"B","P","C"})foreach(int count in new[]{64,65,96}){
   string p=kind+"|"+string.Join('|',Enumerable.Range(1,count).Select(i=>$"{128+2*i}:192"));string? e=Refuse(()=>Load(kind+count,Prefix()+Row(2000,1,126,p)));
   C(e==(count>64?"R06 allows at most 64 normalized controls per slider.":"Path exceeds the 64-control engineering limit."),kind+" source supplied/normalized64 guard even if fallback");
  }
  foreach(int n in new[]{16,17,32,33}){
   string file=System.IO.Path.Combine(temp,"r"+n+".osu");File.WriteAllText(file,Prefix()+Row(2000,n));
   C((Refuse(()=>Beatmap.LoadCanonical(file)) is null)==(n<=32),"new control profile canonical traversal "+n);
   C(Refuse(()=>Beatmap.Load(file))=="R06 allows at most 64 normalized controls per slider.","legacy raw count before traversal "+n);
  }
  var three=Load("multi-consumer",Prefix()+Row(2000,17)+Row(20000,32)+Row(52000,24));await Connected(three,library,"real newly admitted three-parent17/32/24",false);
  C(three.Sliders.All(s=>s.Geometry.SourceControls.Count==96&&s.Geometry.RawSegments.Count==95)&&three.CircleSounds!.RegisteredOverrides.Count==3,"multi-parent fresh canonical ownership");
  var max=Load("max",Prefix(8,100)+Row(2000,32,1600,Path96(true)));var sx=max.Sliders[0];C(sx.Geometry.Controls.Count==96&&sx.Movements.Count==3040&&sx.Checkpoints.Count==4096,"real96 control3040 movements4096 checkpoints");
  C(Refuse(()=>Load("cp-over",Prefix(8,100)+Row(2000,32,1612.5,Path96(true,1741))))=="R05 accepts at most 4096 checkpoints per slider.","real per-parent4096 one-over");
  var timer=Stopwatch.StartNew();long allocation=GC.GetAllocatedBytesForCurrentThread();for(int k=0;k<5;k++)foreach(var cp in sx.Checkpoints)_=sx.RequiredPosition(cp.Time);allocation=GC.GetAllocatedBytesForCurrentThread()-allocation;timer.Stop();
  stress.Add(new{kind="actual RequiredPosition measured CPU work; not frame-time qualification",sx.Movements.Count,checkpoints=sx.Checkpoints.Count,queries=5*sx.Checkpoints.Count,milliseconds=timer.Elapsed.TotalMilliseconds,allocatedBytes=allocation,conservativePreparationComparisons=4096L*3040,exclusiveNewProfileMapBound=100000L*3040});
  string moveRows=string.Concat(Enumerable.Range(0,82).Select(i=>Row(2000+31000*i)))+Row(2000+31000*82,8,180);
  var moves=Load("moves-exact",Prefix()+moveRows);C(moves.Sliders.Sum(s=>s.Movements.Count)==250000,"actual new multi-parent exact250000 movements; final trim90");
  C(Refuse(()=>Load("moves-one-over",Prefix()+moveRows+Row(2000+31000*83,1,2)))=="R06 map geometry exceeds 100000 prepared segments / 250000 Movement records.","actual map movement one-over deterministic");
  string segmentRows=string.Concat(Enumerable.Range(0,1052).Select(i=>Row(2000+1100*i,1)))+Row(2000+1100*1052,1,120);
  var segments=Load("segments-exact",Prefix()+segmentRows);C(segments.Sliders.Sum(s=>s.Geometry.Segments.Count)==100000&&segments.Sliders[^1].Geometry.RawSegments.Count==95&&segments.Sliders[^1].Geometry.Segments.Count==60,"actual100000 segments counts prepared trim60 not raw95");
  C(Refuse(()=>Load("segments-one-over",Prefix()+segmentRows+Row(2000+1100*1053,1,2)))=="R06 map geometry exceeds 100000 prepared segments / 250000 Movement records.","actual map prepared segments one-over");
  string checkpointRows=string.Concat(Enumerable.Range(0,24).Select(i=>Row(2000+52000*i,32,1600,Path96(true))));
  var near=Load("map-cp-near",Prefix(8,100)+checkpointRows);C(near.Sliders.Sum(s=>s.Checkpoints.Count)==98304,"real98304 map checkpoints");
  string extra=Row(2000+52000*24,32,662.5,Path96(true,791));var cpx=Load("map-cp-exact",Prefix(8,100)+checkpointRows+extra);
  C(cpx.Sliders.Sum(s=>s.Checkpoints.Count)==100000,"real exact100000 checkpoints");
  C(Refuse(()=>Load("map-cp-one-over",Prefix(8,100)+checkpointRows+extra+Row(2000+52000*25,1,2)))=="R05 accepts at most 100000 scheduled checkpoints per map.","real map checkpoints one-over");
  stress.Add(new{kind="genuine canonical newly admitted multi-parent budgets",movements=moves.Sliders.Sum(s=>s.Movements.Count),segments=segments.Sliders.Sum(s=>s.Geometry.Segments.Count),checkpoints=cpx.Sliders.Sum(s=>s.Checkpoints.Count),movementParents=moves.Sliders.Count,segmentParents=segments.Sliders.Count,checkpointParents=cpx.Sliders.Count});
  // Full real loader instrumentation, never substituted geometry or borrowed owners.
  string cancelPath=System.IO.Path.Combine(temp,"cancel-new.osu");File.WriteAllText(cancelPath,Prefix(8,100)+Row(2000,32,1600,Path96(true)));
  foreach(string stage in new[]{"coordinate","row-head-normalization","normalize","head-normalization","piece","raw-length","trim","cumulative","geometry-complete","preflight","traversal","segment","tick","movement-allocation","movement-traversal","movement-segment","parent","position","complete","map-publication"}){
   using var cts=new CancellationTokenSource();bool reached=false,cancelled=false;Beatmap? partial=null;int seen=0;
   try{partial=ObserveLoad(cancelPath,cts.Token,(a,b,d)=>{if(a==stage&&++seen==(stage is "coordinate" or "normalize" or "piece" or "segment" or "tick" or "movement-segment" or "position"?3:1)){reached=true;cts.Cancel();}});}catch(OperationCanceledException){cancelled=true;}
   C(reached&&cancelled&&partial is null,"full canonical new96 cancellation "+stage);stress.Add(new{kind="actual canonical full loader instrumented",stage,reached,cancelled,partialPublished=partial is not null});
  }
  using(var cts=new CancellationTokenSource()){cts.Cancel();bool cancelled=false;try{Beatmap.LoadCanonical(cancelPath,cts.Token);}catch(OperationCanceledException){cancelled=true;}C(cancelled,"actual96 map precancel");}
  var resourceMap=Load("new-resource",Prefix()+Row(2000,17).Replace("0:0:0:100:","0:0:0:100:owned.wav"));
  byte[] custom=File.ReadAllBytes(System.IO.Path.Combine(temp,"pack/normal-hitnormal.wav"));Array.Resize(ref custom,2048);File.WriteAllBytes(System.IO.Path.Combine(temp,"owned.wav"),custom);await Partial(resourceMap);
  // Genuine source-L96 variants: sample errors remain stored, not admission failures.
  foreach(string edge in new[]{"2","2|8|4|2|8|4|2|8|4|2|8|4|2|8|4|2|8|4|2|8|4|2|8|4|2|8|4|2|8|4|2|8|4|2|8"}){
   var e=Load("edge-"+edge.Length,Prefix()+Row(2000).Replace(",2|8,",","+edge+","));var d=e.Sliders[0].Samples!;
   var expected=edge.Split('|').Take(33).Select(int.Parse).ToList();while(expected.Count<33)expected.Add(2);C(d.EdgeSounds.SequenceEqual(expected)&&d.Bases.Count==33,"new96 short/long raw+1 sound edge arrays "+edge.Length);await Connected(e,library,"new96 edge payload "+edge.Length,false);
  }
  foreach(string payload in new[]{"x:0:0:100:","0:0:x:100:"}){
   var e=Load("bad-sample-"+payload.GetHashCode(),Prefix()+Row(2000,17).Replace("0:0:0:100:",payload));C(e.Sliders[0].Samples!.Error is not null,"new96 malformed sample stores Error");using var fx=await library.PrepareAsync(e,default);C(fx.Error is not null,"new96 malformed sample unavailable resources");
  }
  var stack=Load("stack-source",Prefix().Replace("StackLeniency:0","StackLeniency:0.7")+Row(2000,2,20)+Row(2300,2,20));
  var original=Load("stack-untranslated",Prefix()+Row(2000,2,20)+Row(2300,2,20));
  C(stack.Sliders[0].Head.Position!=original.Sliders[0].Head.Position,"actual new96 modern stack translation occurs");
  foreach(var pair in stack.Sliders.Zip(original.Sliders)){
   var a=pair.First;var b=pair.Second;Vector2 delta=a.Head.Position-b.Head.Position;
   C(a.Geometry.SourceControls.SequenceEqual(b.Geometry.SourceControls)&&a.Geometry.RawSegments.SequenceEqual(b.Geometry.RawSegments)&&a.Geometry.CumulativeLengths.SequenceEqual(b.Geometry.CumulativeLengths)&&a.Geometry.Controls.SequenceEqual(b.Geometry.Controls.Select(p=>p+delta)),"new96 stack preserves source controls/raw and translated prepared identity");
   C(stack.CircleSounds!.RegisteredOverrides.Any(o=>ReferenceEquals(o.Slider,a))&&CircleSession.CreateFreshOrdinary(stack,ScoreConfiguration.FromBeatmap(stack)).Sliders.Any(o=>ReferenceEquals(o.Definition,a)),"new96 translated sample/session registration identity");
  }
  foreach(var a in new[]{(32L,95L,3040L),(32L,95L,3039L),(long.MaxValue,95L,250000L)}){
   int? n=null;var e=Refuse(()=>n=(int)Call("PreflightSliderMovements",a.Item1,a.Item2,a.Item3,CancellationToken.None)!);C(a.Item1==32&&a.Item3==3040?n==3040:e is not null,"checked Int64 preallocation component "+a);
  }
 }
}
