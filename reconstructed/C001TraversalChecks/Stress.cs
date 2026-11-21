using System.Reflection;
using Ohno.CircleGame.Gameplay;
partial class Program
{
 static void Stress(string root,string tmp)
 {
  string template=File.ReadAllText(Path.Combine(root,"re/campaigns/C001/traversal-profile/fixtures/v14-L-r17.osu"));
  bool Refused(Action a,string phrase){try{a();return false;}catch(InvalidDataException e){return e.Message.Contains(phrase,StringComparison.Ordinal);}}
  C(Refused(()=>Beatmap.Load(Path.Combine(root,"re/campaigns/C001/traversal-profile/fixtures/v14-L-r17.osu")),"1–16"),"legacy string loading retains17 refusal");
  var preflight=typeof(Beatmap).GetMethod("PreflightSliderMovements",BindingFlags.Static|BindingFlags.NonPublic)!;
  int Budget(long r,long s,long remain){try{return (int)preflight.Invoke(null,new object[]{r,s,remain,default(CancellationToken)})!;}catch(TargetInvocationException e)when(e.InnerException is not null){throw e.InnerException;}}
  C(Budget(16,8192,250000)==131072,"legacy maximum geometry movement preflight");
  C(Refused(()=>Budget(32,8192,250000),"Movement"),"32*8192 authentic movement work refusal before expansion");
  C(Budget(32,7812,250000)==249984&&Refused(()=>Budget(32,7813,250000),"Movement"),"250000 movement boundary checked before allocation");
  C(Refused(()=>Budget(long.MaxValue,2,250000),"Int64"),"checked Int64 multiplication overflow translated to bounded refusal");
  string prefix=template[..(template.IndexOf("[HitObjects]",StringComparison.Ordinal)+"[HitObjects]".Length)]+"\n";
  Beatmap Load(string name,string text){string f=Path.Combine(tmp,name+".osu");File.WriteAllText(f,text);return Beatmap.LoadCanonical(f);}
  string dense="C|"+string.Join('|',Enumerable.Repeat("128:192",63).Append("4224:192"));
  var denseMap=Load("dense32",prefix+$"128,192,2000,2,2,{dense},32,4096\n");var first=denseMap.Sliders.Single();
  int segments=first.Geometry.Segments.Count;
  C(first.Geometry.RawSegments.Count==50*(first.Geometry.Controls.Count-1)&&segments>3000&&first.Movements.Count==32*segments,"actual canonical dense Catmull geometry and repeated work");
  int remaining=250000,rowTime=2000;var movementRows=new System.Text.StringBuilder();
  while(remaining>=segments){int r=Math.Min(32,remaining/segments);movementRows.Append($"128,192,{rowTime},2,2,{dense},{r},4096\n");remaining-=r*segments;rowTime+=first.EndTime-first.Head.StartTime+10;}
  var aggregate=Load("movement-near",prefix+movementRows);int used=aggregate.Sliders.Sum(x=>x.Movements.Count);
  C(used<=250000&&250000-used<segments,"actual multi-slider near250000 movement budget");
  C(Refused(()=>Load("movement-over",prefix+movementRows+$"128,192,{rowTime},2,2,{dense},1,4096\n"),"Movement"),"actual multi-slider remaining movement preflight rejects overbudget");
  string fast=prefix.Replace("0,500,4,1,0,100,1,0","0,100,4,1,0,100,1,0").Replace("SliderTickRate:1","SliderTickRate:8");
  string Row(int i,double length=1600)=>$"128,192,{2000+52000*i},2,2,L|1741:192,32,{length.ToString(System.Globalization.CultureInfo.InvariantCulture)}\n";
  // Exact geometric1600 path avoids suppression-estimate fitting.
  string exact=Row(0).Replace("1741:192","1728:192");var cp=Load("checkpoints-exact",fast+exact);
  C(cp.Sliders[0].Checkpoints.Count==4096,"actual canonical exact4096 per-slider checkpoints");
  C(Refused(()=>Load("checkpoints-over",fast+Row(0,1612.5)),"4096 checkpoints"),"actual canonical per-slider checkpoint limit");
  string rows24=string.Concat(Enumerable.Range(0,24).Select(i=>Row(i).Replace("1741:192","1728:192")));
  var cpMap=Load("map-cp-near",fast+rows24);C(cpMap.Sliders.Sum(x=>x.Checkpoints.Count)==98304,"actual map checkpoint near100000");
  C(Refused(()=>Load("map-cp-over",fast+rows24+Row(24).Replace("1741:192","1728:192")),"100000 scheduled checkpoints"),"actual aggregate checkpoint budget before repeated movement expansion");
  budgets.Add(new{kind="actual canonical aggregate",segments,rawSegments=first.Geometry.RawSegments.Count,movements=used,sliders=aggregate.Sliders.Count,mapCheckpoints=cpMap.Sliders.Sum(x=>x.Checkpoints.Count)});
  string segmentRows=string.Concat(Enumerable.Range(0,31).Select(i=>$"128,192,{2000+i*30000},2,2,{dense},1,4096\n"));
  var segmentMap=Load("segments-near",prefix+segmentRows);C(segmentMap.Sliders.Sum(x=>x.Geometry.Segments.Count)==31*segments,"actual map near100000 prepared segments");
  C(Refused(()=>Load("segments-over",prefix+segmentRows+$"128,192,{2000+31*30000},2,2,{dense},1,4096\n"),"100000 prepared segments"),"actual aggregate segment budget preserved");
  var arc=Load("geometry-max-raw",prefix+"128,192,2000,2,2,P|20990:21054|41852:192,1,4096\n");
  C(arc.Sliders[0].Geometry.RawSegments.Count==8192,"actual canonical geometry8192 raw segment boundary");
  var arc32=Load("geometry-max-raw32",prefix+"128,192,2000,2,2,P|20990:21054|41852:192,32,4096\n");
  C(arc32.Sliders[0].Movements.Count==32*arc32.Sliders[0].Geometry.Segments.Count&&arc32.Sliders[0].Geometry.Segments.Count<8192,"movement preflight uses adjusted segments, not raw8192 false exclusion");
  C(Refused(()=>Load("geometry-over",prefix+"128,192,2000,2,2,P|32896:32960|65664:192,1,4096\n"),"8192-segment"),"actual raw geometry budget rejects before large allocation");
  var core=typeof(Beatmap).GetMethod("PrepareSliderGeometry",BindingFlags.Static|BindingFlags.NonPublic)!;
  var baseMap=Beatmap.LoadCanonical(Path.Combine(root,"re/campaigns/C001/traversal-profile/fixtures/v14-L-r17.osu"));var source=baseMap.Sliders[0];
  SliderDefinition Expand(int n,long moves,long cps,long segs,CancellationToken token,Action<string,int,int>? observe=null){try{return (SliderDefinition)core.Invoke(null,new object?[]{source.Head,source.Geometry,300d,n,200d,100d,token,moves,cps,segs,observe})!;}catch(TargetInvocationException e)when(e.InnerException is not null){throw e.InnerException;}}
  var pieces=Enumerable.Range(0,8192).Select(i=>new SliderSegment(new(128+.5f*i,192),new(128+.5f*(i+1),192),true,true)).ToArray();
  var maxGeometry=new SliderGeometry(source.Geometry.Controls,pieces,pieces,Enumerable.Range(1,8192).Select(i=>.5*i).ToArray(),4096,4096);
  var maxStages=new List<string>();bool maxRejected=false;
  try{core.Invoke(null,new object?[]{source.Head,maxGeometry,4096d,32,200d,100d,default(CancellationToken),250000L,100000L,100000L,(Action<string,int,int>)((name,_,_)=>maxStages.Add(name))});}catch(TargetInvocationException e)when(e.InnerException is InvalidDataException error&&error.Message.Contains("Movement",StringComparison.Ordinal)){maxRejected=true;}
  C(maxRejected&&!maxStages.Contains("movement-allocation"),"bounded geometry upper-limit8192 times32 refuses before expansion (component stress, not canonical owner)");
  foreach(string stage in new[]{"preflight","traversal","segment","tick","movement-allocation","movement-traversal","movement-segment","parent","position","complete"}){
   using var stop=new CancellationTokenSource();SliderDefinition? published=null;bool cancelled=false,observed=false;int ticks=0;
   Action<string,int,int> observe=(name,t,k)=>{if(name==stage&&(name is not ("traversal" or "segment" or "movement-traversal" or "movement-segment")||t==1)&&(name!="tick"||++ticks==2)){observed=true;stop.Cancel();}};
   try{published=Expand(32,250000,100000,100000,stop.Token,observe);}catch(OperationCanceledException){cancelled=true;}
   C(observed&&cancelled&&published is null,"deterministic cancellation at real "+stage+" boundary publishes no partial parent");
  }
  var phases=new List<string>();bool budget=Refused(()=>Expand(32,31,100000,100000,default,(name,_,_)=>phases.Add(name)),"Movement");
  C(budget&&!phases.Contains("movement-allocation"),"movement budget refuses BEFORE repeated list allocation");
  phases.Clear();budget=Refused(()=>Expand(32,250000,95,100000,default,(name,_,_)=>phases.Add(name)),"100000 scheduled");
  C(budget&&!phases.Contains("movement-allocation"),"exact checkpoint preflight refuses BEFORE repeated list allocation");
  using var pre=new CancellationTokenSource();pre.Cancel();bool stopped=false;try{Beatmap.LoadCanonical(Path.Combine(tmp,"movement-near.osu"),pre.Token);}catch(OperationCanceledException){stopped=true;}C(stopped,"actual canonical pre-cancelled map");

 }
}
