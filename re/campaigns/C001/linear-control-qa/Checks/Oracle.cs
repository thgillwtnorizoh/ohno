// Reused Work1 Issue19 scalar/schedule helpers; no Work2 expected values.
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Selection;
using Ohno.CorpusAudit;
partial class Program {
 static List<object> checks=new(),cases=new(),consumers=new(),probes=new(),stress=new();
 static int failures,audio,errors,prepared,refused,added,lost;static string temp="";
 static void C(bool value,string name){checks.Add(new{pass=value,name});if(!value){failures++;Console.WriteLine("FAIL "+name);}}
 static string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes));
 static string? Refuse(Action f){try{f();return null;}catch(InvalidDataException e){return e.Message;}}
 static object? Call(string name,params object?[] args){try{return typeof(Beatmap).GetMethod(name,BindingFlags.Static|BindingFlags.NonPublic)!.Invoke(null,args);}catch(TargetInvocationException e)when(e.InnerException is not null){throw e.InnerException;}}
 static Vector2 V(JsonElement e)=>new(e[0].GetSingle(),e[1].GetSingle());
 static SliderGeometry Prediction(JsonElement c){var p=c.GetProperty("prediction");SliderSegment S(JsonElement e)=>new(V(e[0]),V(e[1]),true,true);
  var raw=p.GetProperty("raw").EnumerateArray().Select(S).ToArray();var seg=p.GetProperty("segments").EnumerateArray().Select(S).ToArray();
  var points=new[]{raw[0].From}.Concat(raw.Select(s=>s.To)).ToArray();
  return new(points,raw,seg,p.GetProperty("cumulative").EnumerateArray().Select(e=>e.GetDouble()).ToArray(),p.GetProperty("unadjusted").GetDouble(),c.GetProperty("requested").GetDouble()){SourceKind=SliderPathKind.Linear,PreparedKind=SliderPathKind.Linear,SourceControls=points};
 }
 static string GeometryHash(SliderGeometry g)=>Hash(JsonSerializer.SerializeToUtf8Bytes(new{raw=g.RawSegments.Select(s=>new[]{s.From.X,s.From.Y,s.To.X,s.To.Y}),segments=g.Segments.Select(s=>new[]{s.From.X,s.From.Y,s.To.X,s.To.Y}),g.CumulativeLengths,g.UnadjustedLength,g.RequestedLength}));
 static SliderDefinition Expand(SliderGeometry g,int n,double velocity,double q,CancellationToken token=default,long moves=250000,long cps=100000,long segs=100000,Action<string,int,int>? observe=null)
  =>(SliderDefinition)Call("PrepareSliderGeometry",new CircleDefinition(new(128,192),2000),g,g.RequestedLength,n,velocity,q,token,moves,cps,segs,observe)!;
 static void Schedule(SliderDefinition s,string id){
  double t=2000;int k=0;bool exact=true;
  for(int tr=0;tr<s.Traversals;tr++)for(int j=0;j<s.Geometry.Segments.Count;j++){
   int ix=tr%2==0?j:s.Geometry.Segments.Count-1-j;float d=(float)(s.Geometry.CumulativeLengths[ix]-(ix==0?0:s.Geometry.CumulativeLengths[ix-1]));double end=t+(double)(1000f*d)/s.TravelRate;
   var m=s.Movements[k++];var seg=s.Geometry.Segments[ix];exact&=m.StartTime==(int)t&&m.EndTime==(int)end&&m.From==(tr%2==0?seg.From:seg.To)&&m.To==(tr%2==0?seg.To:seg.From);t=end;
  }
  C(exact&&s.EndTime==(int)t&&k==s.Movements.Count,id+" independent per-piece timing/order");
  C(s.EndPosition==(s.Traversals%2==0?new Vector2(128,192):s.Geometry.Segments[^1].To),id+" endpoint parity");
  C(s.Checkpoints.Count>=s.Traversals&&s.Checkpoints.Count<=4096,id+" finite checkpoint ownership");
  bool positions=true;foreach(var cp in s.Checkpoints.Where(c=>c.Kind!=SliderCheckpointKind.Interior))positions&=cp.Position==s.TraversalEndMovement(cp.Traversal).To;
  C(positions,id+" endpoint checkpoint consumer");
 }
 static void OriginalSchedule(SliderDefinition s,JsonElement c,string id){
  var predicted=c.GetProperty("schedule");C(s.EndTime==predicted.GetProperty("end").GetInt32(),id+" independent end prediction");
  C(s.Checkpoints.Select(x=>$"{x.Time}/{x.Kind}/{x.Traversal}").SequenceEqual(predicted.GetProperty("checkpoints").EnumerateArray().Select(x=>$"{x[0].GetInt32()}/{x[1].GetString()}/{x[2].GetInt32()}")),id+" independent full checkpoint schedule");
 }

}
