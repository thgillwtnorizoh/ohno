using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
partial class Program {
 static string Header(double tick=.5,double beat=500)=>$"osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Metadata]\nTitle:Independent Issue21\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:{tick.ToString(Inv)}\n[TimingPoints]\n0,{beat.ToString(Inv)},4,1,0,100,1,0\n[HitObjects]\n";
 static string Chain(int controls=96)=>"L|"+string.Join('|',Enumerable.Range(1,controls-1).Select(i=>$"{128+2*i}:192"));
 static string Vertical()=>"L|"+string.Join('|',Enumerable.Range(0,96).Select(i=>i==95?"128:1792":"128:192"));
 static string Row(int start,int spans=1,double length=190,string? path=null)=>$"128,192,{start},6,2,{path??Chain()},{spans},{length.ToString(Inv)},2|8,1:0|2:0,0:0:0:100:\n";
 static Beatmap Load(string name,string text){string p=Path.Combine(temp,name+".osu");File.WriteAllText(p,text);return Beatmap.LoadCanonical(p);}
 static Beatmap Observed(string file,CancellationToken token,Action<string,int,int> observer)=>(Beatmap)Call("LoadCore",file,token,true,observer)!;
 static async Task Adversaries(HitSoundLibrary library){
  C(SliderGeometryPreparation.MaxControls==64&&SliderGeometryPreparation.MaxSegments==8192,"historical public constants64/8192");
  var a=Enumerable.Range(0,65).Select(i=>new Vector2(128+2*i,192)).ToArray();
  C(Refuse(()=>SliderGeometryPreparation.FlattenBezier(a))=="R06 flattener requires 1–64 finite controls.","public FlattenBezier65 fence");
  C(SliderSoundMetadata.Parse(2,16,14,0,null,null,null).EffectiveTraversals==16&&Refuse(()=>SliderSoundMetadata.Parse(2,17,14,0,null,null,null))=="R12 retains the ENGINEERING 1–16 traversal admission bound.","public metadata16/17 fence");
  foreach(string marker in new[]{"B","P","C"})foreach(int count in new[]{64,65,96}){
   string p=marker+"|"+string.Join('|',Enumerable.Range(0,count).Select(i=>$"{128+2*i}:192"));
   string? e=Refuse(()=>Load(marker+count,Header()+Row(2000,1,126,p)));
   C(count==64?e is null:e=="R06 allows at most 64 normalized controls per slider.","literal source "+marker+" equal first count"+count);
  }
  var fallback=Load("P-linear-fallback",Header()+Row(2000,1,126,Chain(64).Replace("L|","P|"))).Sliders[0];
  C(fallback.Geometry.SourceKind==SliderPathKind.Perfect&&fallback.Geometry.PreparedKind==SliderPathKind.Linear,"actual P prepared Linear remains source P");
  C(Refuse(()=>Load("P-fallback65",Header()+Row(2000,1,128,Chain(65).Replace("L|","P|"))))=="Path exceeds the 64-control engineering limit.","P Linear fallback normalized65 has historical diagnostic");
  foreach(int v in Enumerable.Range(5,10))foreach(int n in new[]{16,17,32,33}){
   string text=(Header()+Row(2000,n)).Replace("format v14","format v"+v);
   C((Refuse(()=>Load($"v{v}-r{n}",text)) is null)==(n<=32),$"literal L96 version{v} traversal{n}");
  }
  C(Refuse(()=>Load("raw97",Header()+Row(2000,1,190,Chain(98))))=="Canonical source L allows at most 96 raw coordinate cells before head insertion.","raw97 precise new diagnostic");
  // Competing errors: raw -> coordinate -> spans -> length -> mode -> normalized.
  foreach(var pair in new[]{
   (Row(2000,33,4097,Chain(98)).Replace("130:192","bad:192"),"Canonical source L allows at most 96 raw coordinate cells before head insertion."),
   (Row(2000,33,4097).Replace("130:192","bad:192"),"Invalid finite number for Bézier control coordinate."),
   (Row(2000,33,4097),"Canonical supports 1–32 slider traversals (span count includes the first traversal)."),
   (Row(2000,32,4097),"R05 slider length must be greater than 0.0001 and at most 4096 pixels."),
   (Row(2000,1,190,Chain(97)),"R02 requires explicit Mode: 0 (osu!standard).")}){
   C(Refuse(()=>Load("precedence"+checks.Count,Header().Replace("Mode:0","Mode:1")+pair.Item1))==pair.Item2,"new source multiple-error precedence "+pair.Item2);
  }
  // A genuine newly admitted multi-parent map with different spans than Work2's map.
  var multi=Load("multi",Header()+Row(2000,19)+Row(23000,27)+Row(52000,31));
  C(multi.Sliders.Select(s=>s.Traversals).SequenceEqual(new[]{19,27,31})&&multi.Sliders.All(s=>s.Geometry.SourceControls.Count==96),"independent genuine three-parent19/27/31");await Connected(multi,library,"new three-parent19/27/31",false);
  string moveRows=string.Concat(Enumerable.Range(0,82).Select(i=>Row(2000+i*32000,32)))+Row(2000+82*32000,7)+Row(2000+83*32000,1,110);
  var moves=Load("movement-exact",Header()+moveRows);C(moves.Sliders.Sum(s=>s.Movements.Count)==250000&&moves.Sliders[^1].Geometry.RawSegments.Count==95&&moves.Sliders[^1].Geometry.Segments.Count==55,"actual exact250000 using independently composed84 parents and trimmed55 tail");
  C(Refuse(()=>Load("movement-over",Header()+moveRows+Row(2000+84*32000,1,2)))=="R06 map geometry exceeds 100000 prepared segments / 250000 Movement records.","actual250001 movements refusal");
  // Last remaining allowance60: a raw95 chain must first trim, then count60.
  string segments=string.Concat(Enumerable.Range(0,1052).Select(i=>Row(2000+i*3000)))+Row(2000+1052*3000,1,120);
  var seg=Load("segment-exact",Header()+segments);C(seg.Sliders.Sum(s=>s.Geometry.Segments.Count)==100000&&seg.Sliders[^1].Geometry.RawSegments.Count==95&&seg.Sliders[^1].Geometry.Segments.Count==60,"actual prepared100000 budget after raw95 trim60");
  C(Refuse(()=>Load("segment-over",Header()+segments+Row(2000+1053*3000,1,2)))=="R06 map geometry exceeds 100000 prepared segments / 250000 Movement records.","actual100001 final prepared segments refusal");
  var max=Load("max-vertical",Header(8,100)+Row(2000,32,1600,Vertical()));
  C(max.Sliders[0].Movements.Count==3040&&max.Sliders[0].Checkpoints.Count==4096&&max.Sliders[0].Geometry.RawSegments.Count(x=>x.From==x.To)==94,"genuine96/95 vertical duplicates3040 movement4096 plan");
  C(Refuse(()=>Load("parent-checkpoint-over",Header(8,100)+Row(2000,32,1612.5,Vertical().Replace("1792","1805"))))=="R05 accepts at most 4096 checkpoints per slider.","actual one additional checkpoint per traversal over4096");
  string cps=string.Concat(Enumerable.Range(0,24).Select(i=>Row(2000+i*53000,32,1600,Vertical())));
  cps+=Row(2000+24*53000,32,662.5,Vertical().Replace("1792","855"));
  var cpmap=Load("cp-exact",Header(8,100)+cps);C(cpmap.Sliders.Sum(s=>s.Checkpoints.Count)==100000,"actual exact100000 map checkpoints independently vertical geometry");
  C(Refuse(()=>Load("cp-over",Header(8,100)+cps+Row(2000+25*53000,1,2)))=="R05 accepts at most 100000 scheduled checkpoints per map.","actual100001 map checkpoints refusal");
  stress.Add(new{kind="actual canonical limits",movementParents=moves.Sliders.Count,movements=moves.Sliders.Sum(s=>s.Movements.Count),preparedSegments=seg.Sliders.Sum(s=>s.Geometry.Segments.Count),checkpoints=cpmap.Sliders.Sum(s=>s.Checkpoints.Count)});
  foreach(var b in new[]{(1L,250000L,250000L,250000),(32L,95L,3040L,3040),(32L,95L,3039L,-1),(long.MaxValue,2L,250000L,-2),(long.MinValue,-1L,250000L,-2),(1L,1L,-1L,-1)}){
   object? count=null;string? e=Refuse(()=>count=Call("PreflightSliderMovements",b.Item1,b.Item2,b.Item3,CancellationToken.None));
   C(b.Item4>=0?e is null&&(int)count! ==b.Item4:b.Item4==-2?e=="Slider Movement work exceeds checked Int64 resource limit.":e=="R06 map geometry exceeds 100000 prepared segments / 250000 Movement records.","checked Int64 preflight "+b);
  }
  // Cooperative cancellation calls the full parser/geometry/planner and returns no owner.
  string file=Path.Combine(temp,"cancel.osu");File.WriteAllText(file,Header(8,100)+Row(2000,32,1600,Vertical()));
  foreach(string stage in new[]{"coordinate","row-head-normalization","normalize","head-normalization","piece","raw-length","trim","cumulative","geometry-complete","preflight","traversal","segment","tick","movement-allocation","movement-traversal","movement-segment","parent","position","complete","map-publication"}){
   using var cts=new CancellationTokenSource();int seen=0;bool cancelled=false,reached=false;Beatmap? partial=null;
   try{partial=Observed(file,cts.Token,(s,t,j)=>{if(s==stage&&++seen==(stage is "coordinate" or "piece" or "normalize" or "movement-segment" or "position"?7:1)){reached=true;cts.Cancel();}});}catch(OperationCanceledException){cancelled=true;}
   C(reached&&cancelled&&partial is null,"actual full loader cancellation "+stage);stress.Add(new{kind="actual full loader cancellation",stage,reached,cancelled,published=partial is not null});
  }
  using(var cts=new CancellationTokenSource()){cts.Cancel();bool cancelled=false;try{Beatmap.LoadCanonical(file,cts.Token);}catch(OperationCanceledException){cancelled=true;}C(cancelled,"canonical96 pre-cancel");}
  byte[] wav=File.ReadAllBytes(Path.Combine(temp,"pack/normal-hitnormal.wav"));File.WriteAllBytes(Path.Combine(temp,"owned.wav"),wav);
  var custom=Load("custom",Header()+Row(2000,19).Replace("0:0:0:100:","0:0:0:100:owned.wav"));
  using(var fx=await library.PrepareAsync(custom,default)){C(fx.Error is null&&fx.Resources.Caches.Map.Count>0,"real valid custom WAV bytes owned");fx.Dispose();C(fx.Resources.Caches.Map.Count==0,"real custom WAV cache released after disposal");}
  await Partial(custom);
  // Stack translation and raw geometry provenance, using independently known CS5 step3.2.
  var stacked=Load("stack",Header().Replace("StackLeniency:0","StackLeniency:0.7")+Row(2000,2,10)+Row(2200,2,10));
  var plain=Load("plain",Header()+Row(2000,2,10)+Row(2200,2,10));
  C(stacked.Sliders[0].Head.Position==new Vector2(128,192)-new Vector2(3.2f),"new96 exact CS5 negative stack step");
  foreach(var pair in stacked.Sliders.Zip(plain.Sliders)){var s=pair.First;var o=pair.Second;var d=s.Head.Position-o.Head.Position;
   C(s.Geometry.SourceControls.SequenceEqual(o.Geometry.SourceControls)&&s.Geometry.RawSegments.SequenceEqual(o.Geometry.RawSegments)&&s.Geometry.CumulativeLengths.SequenceEqual(o.Geometry.CumulativeLengths),"new96 source raw/cumulative preserved under stack");
   C(s.Geometry.Controls.SequenceEqual(o.Geometry.Controls.Select(p=>p+d))&&s.Geometry.Segments.SequenceEqual(o.Geometry.Segments.Select(x=>x with{From=x.From+d,To=x.To+d})),"new96 prepared path translates once");
  }
  await Connected(stacked,library,"new96 exact stacked registered input",false);
  var nonmonotone=Load("tail",Header(1)+Row(2000,17,206));var slider=nonmonotone.Sliders[0];
  C(slider.EndTime==19510&&slider.Checkpoints[^1].Time==19474&&slider.Checkpoints[^2].Time==19480,"new96 original nonmonotone final deadline retained");
  await Connected(nonmonotone,library,"new96 nonmonotone pending result",false);
  string legacyFile=Path.Combine(temp,"legacy-two.osu");File.WriteAllText(legacyFile,Header()+Row(2000,17,10,"L|138:192"));
  C(Refuse(()=>Beatmap.Load(legacyFile))=="R05 supports 1–16 slider traversals (span count includes the first traversal).","small legacy path public16 remains independent of count");
  // Resource errors are stored on a genuine new source map, not geometry refusals.
  var bad=Load("bad-audio",Header()+Row(2000,19).Replace("0:0:0:100:","0:0:bad:100:"));using(var fx=await library.PrepareAsync(bad,default))C(bad.Sliders[0].Samples!.Error is not null&&fx.Error is not null,"new96 malformed samples admit geometry but resources unavailable");
  C(Refuse(()=>Load("too-large",Header()+new string(' ',2*1024*1024)+Row(2000)))=="R02 accepts beatmaps up to 2 MB.","2MiB file bound and error precedence");
  C(Refuse(()=>Load("too-many",Header()+string.Concat(Enumerable.Range(0,10001).Select(i=>$"128,192,{2000+i},1,0\n"))))=="R07 accepts at most 10000 objects.","10001 source objects refuse before health");
 }
}
