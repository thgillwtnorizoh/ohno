using System.Numerics;
using System.Reflection;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
internal sealed class NearSink : ISamplePlaybackSink { public List<SamplePlaybackRequest> Requests {get;}=[]; public void Enqueue(SamplePlaybackRequest request)=>Requests.Add(request); }
partial class Program
{
 static string Geometry(Beatmap m)=>JsonSerializer.Serialize(m.Sliders,new JsonSerializerOptions{IncludeFields=true});
 static string History(Beatmap m)
 {
  var session=CircleSession.CreateFreshOrdinary(m,ScoreConfiguration.FromBeatmap(m),false);
  var backend=new RecordingSampleChannels{QueryLength=(_,_)=>8193};
  var channel=new SharedSpinnerChannel(backend,()=>1,()=>1,()=>1,a=>a());channel.Refresh();
  var events=new List<string>();session.SpinnerSound+=(s,e)=>{events.Add(e+":"+s.Definition.StartTime+":"+s.Observation.Count);if(e==SpinnerSoundStage.ObservedChange)channel.Observe(s.Observation.Count,s.Required,true);else if(e==SpinnerSoundStage.Motion)channel.Continue((float)s.Observation.Count/s.Required);else if(e==SpinnerSoundStage.Cleanup)channel.QueuePause();};
  int end=Math.Max(m.Spinners.Max(s=>s.EndTime),m.Sliders.Max(s=>s.EndTime))+300;
  var times=new SortedSet<int>(Enumerable.Range(0,end/17+2).Select(i=>i*17));foreach(var s in m.Sliders){times.Add(s.Head.StartTime);times.Add(s.EndTime+1);foreach(var cp in s.Checkpoints)times.Add(cp.Time);}foreach(var s in m.Spinners){times.Add(s.StartTime);times.Add(s.EndTime+1);}
  foreach(int t in times){foreach(var s in m.Sliders.Where(s=>s.Head.StartTime==t))session.Press(s.Head.Position,t);var slider=m.Sliders.FirstOrDefault(s=>t>=s.Head.StartTime&&t<=s.EndTime);var spin=m.Spinners.FirstOrDefault(s=>t>=s.StartTime&&t<=s.EndTime)??m.Spinners[0];float a=(t-spin.StartTime)*.02f;session.Advance(t,slider?.BallPosition(t)??new Vector2(64,64),1,spin.Center+new Vector2(100*MathF.Cos(a),100*MathF.Sin(a)));}
  C(session.JudgedCount==m.Spinners.Count+m.Sliders.Count,"actual whole-result object denominator");
  C(events.Any(e=>e.StartsWith("Cleanup:")),"real spinner cleanup hook");
  return JsonSerializer.Serialize(new{session.JudgedCount,session.Ledger.Score,session.Ledger.EventCount,events,ops=backend.Operations.Select(x=>x.Name)});
 }
 static void Boundaries(string temp,string template)
 {
  Beatmap Load(string name,string text){string p=Path.Combine(temp,name+".osu");File.WriteAllText(p,text);return Beatmap.LoadCanonical(p);}
  bool Reject(string name,string text,string contains){try{Load(name,text);return false;}catch(System.IO.InvalidDataException e){return e.Message.Contains(contains,StringComparison.Ordinal);}}
  string prefix=template[..(template.IndexOf("[HitObjects]",StringComparison.Ordinal)+"[HitObjects]".Length)]+"\n";
  string Many(int count,bool slider)=>prefix+string.Join('\n',Enumerable.Range(0,count-(slider?1:0)).Select(i=>$"7,11,{1000+3*i},8,2,{1001+3*i}"))+(slider?"\n64,64,10000,2,0,L|164:64,1,100":"");
  C(Load("pair-under",Many(1414,true)).Spinners.Count==1413,"all-object1414 under1m with exempt spinner pairs admitted");
  C(Reject("pair-over",Many(1415,true),"pair work limit"),"all-object1415 exceeds1m despite spinner exemptions");
  C(Reject("all-spinner-over",Many(1415,false),"pair work limit"),"all-spinner cannot bypass pair cap");
  C(Reject("all-spinner",Many(2,false),"zero-displacement"),"no-slider all-spinner profile retains certificate");
  string addCircle=template+"\n64,64,4000,1,0\n";
  C(Reject("circle-bearing",addCircle,"zero-displacement"),"far registered circle disables exemption anywhere");
  using(var pre=new CancellationTokenSource()){pre.Cancel();bool cancelled=false;try{Beatmap.LoadCanonical(Path.Combine(temp,"pair-under.osu"),pre.Token);}catch(OperationCanceledException){cancelled=true;}C(cancelled,"qualified map precancel honored");}
  var method=typeof(Beatmap).GetMethod("CertifyLegacySpinnerZeroDisplacementCore",BindingFlags.NonPublic|BindingFlags.Static)!;
  var operands=Enumerable.Range(0,1414).Select(i=>new SamplePreparationOperand(new(new Vector2(8*(i%64),8*(i/64)),100+i),0,"",null)).ToList();
  using var stop=new CancellationTokenSource();stop.CancelAfter(1);bool interrupted=false;
  var timeout=System.Diagnostics.Stopwatch.StartNew();
  do{try{method.Invoke(null,new object[]{operands,stop.Token,true});}catch(TargetInvocationException e)when(e.InnerException is OperationCanceledException){interrupted=true;}}while(!interrupted&&timeout.Elapsed<TimeSpan.FromSeconds(5));
  C(interrupted,"bounded proof responds to asynchronously requested cancellation");
  var spinner=Load("isolated",prefix+"7,11,1000,8,2,1100").CircleSounds!.RegisteredOverrides.Single();
  using var cancelledToken=new CancellationTokenSource();cancelledToken.Cancel();bool allCancelled=false;try{method.Invoke(null,new object[]{Enumerable.Repeat(spinner,1414).ToList(),cancelledToken.Token,true});}catch(TargetInvocationException e)when(e.InnerException is OperationCanceledException){allCancelled=true;}C(allCancelled,"all-exempt spinner scan cannot bypass cancellation");
 }
}
