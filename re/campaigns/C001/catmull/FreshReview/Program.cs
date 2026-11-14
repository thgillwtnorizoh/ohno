using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
class Program {
 static List<object> checks=new();
 static void Check(bool ok,string name){checks.Add(new{name,pass=ok});Console.WriteLine((ok?"PASS ":"FAIL ")+name);}
 static void Reject(Action action,string name){try{action();Check(false,name);}catch(InvalidDataException){Check(true,name);}}
 static SliderGeometry P(Vector2[] p,double n=200)=>SliderGeometryPreparation.Prepare(p[0],p,SliderPathKind.Catmull,n);
 static Vector2 Hermite(Vector2 a,Vector2 b,Vector2 c,Vector2 d,double t) {
  // Independent cubic Hermite basis: endpoint tangents (c-a)/2 and (d-b)/2.
  double h00=2*t*t*t-3*t*t+1,h10=t*t*t-2*t*t+t,h01=-2*t*t*t+3*t*t,h11=t*t*t-t*t;
  return new((float)(h00*b.X+h10*(c.X-a.X)/2+h01*c.X+h11*(d.X-b.X)/2),
   (float)(h00*b.Y+h10*(c.Y-a.Y)/2+h01*c.Y+h11*(d.Y-b.Y)/2));
 }
 static bool Near(Vector2 a,Vector2 b)=>Vector2.Distance(a,b)<.0001;
 static int Main(string[] args){
  Vector2[] p=[new(0,0),new(16,-8),new(16,-8),new(-16,24)];var g=P(p);
  Check(g.RawSegments.Count==150,"leading head and interior duplicate retained:150 pairs");
  double maxError=0;for(int i=0;i<3;i++)for(int j=0;j<50;j++){
   var a=p[Math.Max(0,i-1)];var b=p[i];var c=p[i+1];var d=i+2<p.Length?p[i+2]:2*c-b;var r=g.RawSegments[i*50+j];
   maxError=Math.Max(maxError,Vector2.Distance(r.From,Hermite(a,b,c,d,j/50.0)));
   maxError=Math.Max(maxError,Vector2.Distance(r.To,Hermite(a,b,c,d,(j+1)/50.0)));
  }
  Check(maxError<.0001,"all300 signed/duplicate endpoints vs independent Hermite basis");
  Check(g.RawSegments.Select((s,i)=>!s.IsLinear&&s.IsSubcurveEnd==(i%50==49)).All(x=>x),"flags at49/99/149 only");
  Check(g.RawSegments[74].To!=p[1],"duplicate span has curved interior rather than zero shortcut");
  var lead=P([new(0,0),new(0,0),new(16,0)]);
  Check(lead.RawSegments.Count==100&&lead.RawSegments[24].To.X==-1,"leading duplicate lobe midpoint=-1");
  Vector2[] line=[new(0,0),new(200,0)];var full=P(line);double last=SliderGeometryPreparation.Length(full.RawSegments[^1]);
  var remove=P(line,full.UnadjustedLength-last+.000099);var retain=P(line,full.UnadjustedLength-last+.000101);
  Check(remove.Segments.Count==49&&!remove.Segments[^1].IsSubcurveEnd,"trim inside1e-4 threshold removes final marked pair");
  Check(retain.Segments.Count==50&&retain.Segments[^1].IsSubcurveEnd,"trim outside1e-4 threshold retains tiny marked pair");
  var controls=Enumerable.Range(0,64).Select(i=>new Vector2(i,0)).ToArray();
  Check(P(controls,63).RawSegments.Count==3150,"64 normalized controls produce3150 pairs");
  Reject(()=>P(Enumerable.Range(0,65).Select(i=>new Vector2(i,0)).ToArray()),"65 controls reject");
  Reject(()=>SliderGeometryPreparation.Prepare(new(-1,0),controls,SliderPathKind.Catmull,63),"head insertion exceeding64 rejects");
  Check(P([new(0,0),new(131072,0)]).RawSegments.Count==50,"inclusive positive coordinate admission boundary");
  Check(P([new(0,0),new(-131072,0)]).RawSegments.Count==50,"inclusive negative coordinate admission boundary");
  Reject(()=>P([new(0,0),new(float.BitIncrement(131072f),0)]),"one float beyond coordinate bound rejects");
  Reject(()=>P([new(0,0),new(float.NaN,0)]),"nonfinite controls reject");
  using(var cts=new CancellationTokenSource()){cts.Cancel();try{SliderGeometryPreparation.Prepare(line[0],line,SliderPathKind.Catmull,200,14,cts.Token);Check(false,"cancelled preparation throws");}catch(OperationCanceledException){Check(true,"cancelled preparation throws");}}
  string source=File.ReadAllText(Path.Combine(args[0],"re/campaigns/C001/catmull/fixtures/two-controls.osu"));
  string file=Path.Combine(Path.GetTempPath(),"fresh-c-"+Guid.NewGuid().ToString("N")+".osu");
  try{
   var text=source.Replace("100,100,1000,2,0,C|100:100|200:100,1,100","0,0,1000,2,0,C|0:0|200:0,3,200");File.WriteAllText(file,text);var map=Beatmap.LoadCanonical(file);var s=map.Sliders.Single();
   int[] times=[1500,2000,2500,3000,3500,3964];
   Check(s.Checkpoints.Select(x=>x.Time).SequenceEqual(times),"three traversals keep tick phase across50 pieces:1500/2000/2500/3000/3500/3964");
   Check(s.Checkpoints.Select(x=>x.Kind).SequenceEqual(new[]{SliderCheckpointKind.Interior,SliderCheckpointKind.Repeat,SliderCheckpointKind.Interior,SliderCheckpointKind.Repeat,SliderCheckpointKind.Interior,SliderCheckpointKind.Final}),"three traversal checkpoint kinds");
   Console.WriteLine("MIDPOINT OBSERVATIONS "+string.Join(";",new[]{1500,2500,3500}.Select(t=>s.RequiredPosition(t).ToString())));
   Check(new[]{1500,2500,3500}.All(t=>Math.Abs(s.RequiredPosition(t).X-100)<.4&&s.RequiredPosition(t).Y==0),"forward/reverse midpoint positions within two integer-clock milliseconds");
   Check(Math.Abs(s.EndTime-4000)<=1&&Near(s.EndPosition,new(200,0)),"three traversal duration and odd endpoint");
   File.WriteAllText(file,text.Replace("C|0:0|200:0","C|0:0|-16.9:8.9|32:0"));var fraction=Beatmap.LoadCanonical(file).Sliders.Single();
   Check(fraction.Geometry.SourceControls[1]==new Vector2(-16,8),"signed fractional parsing truncates toward zero");
   File.WriteAllText(file,text.Replace("C|","c|"));Reject(()=>Beatmap.LoadCanonical(file),"lowercase c excluded by explicit marker engineering profile");
   File.WriteAllText(file,text);Reject(()=>Beatmap.Load(file),"historical Load still excludes C");
  }finally{File.Delete(file);}
  int failed=checks.Count(c=>!JsonSerializer.SerializeToElement(c).GetProperty("pass").GetBoolean());
  File.WriteAllText(args[1],JsonSerializer.Serialize(new{testedProduction="2d776dac175c608e213dbd523c9d237c9411ec8c",runtime=Environment.Version.ToString(),separateReviewer=true,originalRuntime="NOT RUN",maxHermiteEndpointError=maxError,checks=checks.Count,failed,results=checks},new JsonSerializerOptions{WriteIndented=true}));return failed==0?0:1;
 }
}
