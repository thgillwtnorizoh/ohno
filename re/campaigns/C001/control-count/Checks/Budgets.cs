using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
partial class Program {
 sealed class CancelControls(CancellationTokenSource cts):IReadOnlyList<Vector2>{
  public int Count=>64;public Vector2 this[int i]=>new(128+2*i,192);
  public IEnumerator<Vector2> GetEnumerator(){for(int i=0;i<Count;i++){if(i==32)cts.Cancel();yield return this[i];}}
  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()=>GetEnumerator();
 }
 static async Task Budgets(JsonElement all,HitSoundLibrary library){
  var c=all.EnumerateArray().Single(c=>c.GetProperty("id").GetString()=="v14-raw95-insert");var g=Prediction(c);
  C(g.Controls.Count==96&&g.Segments.Count==95,"proposed maximum real adjacent-pair prediction96/95");
  C(SliderSoundMetadata.Parse(2,16,14,0,null,null,null).EffectiveTraversals==16,"public metadata16 retained");
  C(Refuse(()=>SliderSoundMetadata.Parse(2,17,14,0,null,null,null))=="R12 retains the ENGINEERING 1–16 traversal admission bound.","public metadata17 refusal retained");
  var s=Expand(g,32,20,190d/128);C(s.Movements.Count==3040&&s.Checkpoints.Count==4096,"isolated96-control exact3040 movement4096 checkpoint plan");
  foreach(var budget in new[]{(3039L,4096L,95L),(3040L,4095L,95L),(3040L,4096L,94L)}){
   var stages=new List<string>();string? e=Refuse(()=>Expand(g,32,20,190d/128,moves:budget.Item1,cps:budget.Item2,segs:budget.Item3,observe:(a,b,d)=>stages.Add(a)));
   C(e is not null&&!stages.Contains("movement-allocation"),"remaining-budget before movement allocation "+budget);
  }
  var stages2=new List<string>();C(Refuse(()=>Expand(g,32,20,1,observe:(a,b,d)=>stages2.Add(a)))=="R05 accepts at most 4096 checkpoints per slider."&&!stages2.Contains("movement-allocation"),"parent checkpoint overrun before movement allocation");
  foreach(string stage in new[]{"preflight","traversal","segment","tick","movement-allocation","movement-traversal","movement-segment","parent","position","complete"}){
   using var cts=new CancellationTokenSource();bool reached=false,cancelled=false,returned=false;
   try{Expand(g,32,20,190d/128,cts.Token,observe:(a,b,d)=>{if(a==stage&&(a is not ("traversal" or "segment" or "tick" or "movement-traversal" or "movement-segment")||b==4)&&(a is not ("segment" or "tick" or "movement-segment")||d==3)){reached=true;cts.Cancel();}});returned=true;}catch(OperationCanceledException){cancelled=true;}
   C(reached&&cancelled&&!returned,"isolated96-control during-work cancellation "+stage);stress.Add(new{kind="ENGINEERING isolated predicted geometry, actual unchanged scheduler",stage,reached,cancelled,returned});
  }
  using(var cts=new CancellationTokenSource()){
   bool cancelled=false;SliderGeometry? partial=null;try{partial=SliderGeometryPreparation.Prepare(new(128,192),new CancelControls(cts),SliderPathKind.Linear,126,14,cts.Token);}catch(OperationCanceledException){cancelled=true;}
   C(cancelled&&partial is null,"actual geometry cancellation after normalization work publishes no geometry");
  }
  using(var cts=new CancellationTokenSource()){
   cts.Cancel();bool cancelled=false;try{Beatmap.LoadCanonical("does-not-exist",cts.Token);}catch(FileNotFoundException){/* FileInfo length precedes cancellation: retained current ordering. */}catch(OperationCanceledException){cancelled=true;}
   stress.Add(new{kind="missing-file ordering",cancelled,fileStatBeforeCancellation=true});
   string path=Path.Combine(temp,"precancel.osu");File.WriteAllText(path,"osu file format v14\n");cancelled=false;Beatmap? partial=null;
   try{partial=Beatmap.LoadCanonical(path,cts.Token);}catch(OperationCanceledException){cancelled=true;}C(cancelled&&partial is null,"existing-file canonical pre-cancel no map");
  }
  foreach(var a in new[]{(32L,95L,3040L),(32L,95L,3039L),(long.MaxValue,95L,250000L)}){
   int? n=null;var e=Refuse(()=>n=(int)Call("PreflightSliderMovements",a.Item1,a.Item2,a.Item3,CancellationToken.None)!);
   C(a.Item1==32&&a.Item3==3040?n==3040:e is not null,"unchanged checked movement preflight "+a);
  }
  // Genuine admitted <=64 canonical resource cancellation. No fabricated >64 owner.
  string path2=Path.Combine(temp,"resource.osu");File.WriteAllText(path2,"osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n128,192,2000,2,2,L|254:192,1,126,,,0:0:0:100:owned.wav\n");
  byte[] custom=File.ReadAllBytes(Path.Combine(temp,"pack/normal-hitnormal.wav"));Array.Resize(ref custom,2048);File.WriteAllBytes(Path.Combine(temp,"owned.wav"),custom);var map=Beatmap.LoadCanonical(path2);
  using(var cts=new CancellationTokenSource()){cts.Cancel();bool cancelled=false;PreparedCircleEffects? partial=null;try{partial=await library.PrepareAsync(map,cts.Token);}catch(OperationCanceledException){cancelled=true;}C(cancelled&&partial is null,"actual resource preparation precancel publishes no owner");}
  await Partial(map);
 }
}
