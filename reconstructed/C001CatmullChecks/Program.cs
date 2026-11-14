using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using StarAdmission=Ohno.CircleGame.Difficulty.StarAdmission;
class Program
{
 static List<object> checks=new();static List<object> cases=new();static string id="";
 static void C(bool p,string n){checks.Add(new{id,name=n,pass=p});if(!p)throw new Exception(id+": "+n);}
 static bool Near(double a,double b)=>Math.Abs(a-b)<=.0001;
 static Vector2 V(JsonElement e)=>new(e[0].GetSingle(),e[1].GetSingle());
 static bool Near(Vector2 a,Vector2 b)=>Near(a.X,b.X)&&Near(a.Y,b.Y);
 static void Reject(Action a,string label){try{a();}catch(InvalidDataException){C(true,label);return;}throw new Exception("Expected engineering rejection "+id);}
 static string Map(string rows,float stack=0)=>$"osu file format v14\n[General]\nMode:0\nStackLeniency:{stack.ToString(System.Globalization.CultureInfo.InvariantCulture)}\n[Metadata]\nTitle:C development\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n{rows}\n";
 static int Main(string[] args){string root=Path.GetFullPath(args[0]),outFile=args[1],folder=Path.Combine(root,"re/campaigns/C001/catmull"),temp=Path.Combine(Path.GetTempPath(),"catmull-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
 try{
 using var manifest=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(folder,"manifest.json")));
 foreach(var f in manifest.RootElement.GetProperty("cases").EnumerateArray()){
 id=f.GetProperty("id").GetString()!;string file=Path.Combine(folder,f.GetProperty("path").GetString()!);var e=f.GetProperty("expected");
 C(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(file))).Equals(f.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),"unchanged Work1 fixture hash");
 if(id is "all-coincident" or "singleton" or "bare-marker" or "vanishing-request" or "malformed-coordinate" or "precision-diagnostic") {Reject(()=>Beatmap.LoadCanonical(file),"explicit finite engineering rejection");cases.Add(new{id,admitted=false,reason="engineering finite admission; not original runtime"});continue;}
 var map=Beatmap.LoadCanonical(file);var slider=map.Sliders.Single();var g=slider.Geometry;
 C(map.Document!.CopySourceBytes()!.SequenceEqual(File.ReadAllBytes(file))&&slider.RawPath!.StartsWith("C|"),"raw C bytes and marker retained");
 C(g.SourceKind==SliderPathKind.Catmull&&g.PreparedKind==SliderPathKind.Catmull&&g.SourcePerfectCircle is null,"original C classification without P/B fallback");
 C(g.SourceControls.Count==e.GetProperty("normalizedControls").GetArrayLength()&&g.SourceControls.Zip(e.GetProperty("normalizedControls").EnumerateArray()).All(p=>Near(p.First,V(p.Second))),"normalization and head insertion");
 var raw=e.GetProperty("rawSegments");C(g.RawSegments.Count==raw.GetArrayLength()&&g.RawSegments.Count==50*(g.SourceControls.Count-1),"50 pieces per pair including duplicates");
 for(int i=0;i<g.RawSegments.Count;i++){var r=g.RawSegments[i];C(Near(r.From,V(raw[i].GetProperty("start")))&&Near(r.To,V(raw[i].GetProperty("end"))),"independent endpoint pair #"+i);C(r.IsSubcurveEnd==raw[i].GetProperty("isSubcurveEnd").GetBoolean()&&!r.IsLinear,"original span flag #"+i);}
 var prepared=e.GetProperty("preparedSegments");C(g.Segments.Count==prepared.GetArrayLength()&&g.Segments.Zip(prepared.EnumerateArray()).All(p=>Near(p.First.From,V(p.Second.GetProperty("start")))&&Near(p.First.To,V(p.Second.GetProperty("end")))&&p.First.IsSubcurveEnd==p.Second.GetProperty("isSubcurveEnd").GetBoolean()),"shared trim/extension and retained flags");
 C(Near(g.UnadjustedLength,e.GetProperty("unadjustedChordLength").GetDouble())&&g.CumulativeLengths.Count==e.GetProperty("cumulativeLengths").GetArrayLength()&&g.CumulativeLengths.Zip(e.GetProperty("cumulativeLengths").EnumerateArray()).All(p=>Near(p.First,p.Second.GetDouble())),"ordered chord and cumulative lengths");
 C(Near(slider.Tail,V(e.GetProperty("preparedEnd")))&&slider.Movements.Count==g.Segments.Count*slider.Traversals,"prepared endpoint/movement count");
 for(int k=0;k<slider.Movements.Count;k++){int span=k/g.Segments.Count,i=k%g.Segments.Count;var seg=g.Segments[span%2==0?i:g.Segments.Count-1-i];C(Near(slider.Movements[k].From,span%2==0?seg.From:seg.To)&&Near(slider.Movements[k].To,span%2==0?seg.To:seg.From),"repeat movement direction #"+k);}
 C(Near(slider.EndPosition,slider.Traversals%2==0?slider.Head.Position:slider.Tail),"final traversal endpoint");
 C(slider.Checkpoints.All(c=>Near(c.Position,c.Kind==SliderCheckpointKind.Interior?slider.RequiredPosition(c.Time):slider.TraversalEndMovement(c.Traversal).To))&&slider.Samples!.Schedule.SequenceEqual(slider.Checkpoints.Select(c=>c.Time)),"checkpoint/input/sound schedule coherency");
 var health=HealthChart.FromMap(map,1200,HealthPreparationPolicy.E036FreshOrdinary);C(map.HealthSource!.IsFreshFor(map)&&ReferenceEquals(health.NewComboProvenance.Single().Identity,slider)&&ReferenceEquals(map.CircleSounds!.RegisteredOverrides.Single().Slider,slider),"health/NewCombo/sound exact identities");
 var session=new CircleSession(map,ScoreConfiguration.EngineeringPractice,false);var sounds=new List<SliderSoundEvent>();session.SliderSound+=sounds.Add;session.Press(slider.Head.Position,1000);for(int t=1000;t<=slider.EndTime+50;t++)session.Advance(t,slider.BallPosition(t),1);
 C(session.Sliders[0].Head.Result==Judgement.Hit300&&sounds.Count>0&&sounds.All(x=>ReferenceEquals(x.Parent,slider)),"actual session head and sound parent");C(Beatmap.LoadCanonical(file).Sliders.Single().Movements.SequenceEqual(slider.Movements),"reload stable");
 cases.Add(new{id,admitted=true,raw=g.RawSegments.Count,prepared=g.Segments.Count,slider.EndTime,checkpointTimes=slider.Checkpoints.Select(c=>c.Time)});
 }
 id="consumer-controls";string path=Path.Combine(temp,"test.osu");string row="100,100,1000,2,0,C|100:100|200:100|200:200,1,200";
 File.WriteAllText(path,Map(row));var baseline=Beatmap.LoadCanonical(path);var a=baseline.Sliders[0];
 int x=(int)a.Tail.X,y=(int)a.Tail.Y;string follower=$"{x},{y},2100,1,0";File.WriteAllText(path,Map(row+"\n"+follower,.5f));var tail=Beatmap.LoadCanonical(path);C(Near(tail.Circles.Single().Position,new Vector2(x,y)+new Vector2(3.2013118267059326f)),"negative C stack uses adjusted endpoint");
 File.WriteAllText(path,Map(row+"\n100,100,1100,1,0",.5f));var stacked=Beatmap.LoadCanonical(path);var b=stacked.Sliders[0];var delta=b.Head.Position-a.Head.Position;C(delta!=Vector2.Zero,"C receives positive stack displacement");C(b.Geometry.SourceControls.SequenceEqual(a.Geometry.SourceControls)&&b.Geometry.RawSegments.SequenceEqual(a.Geometry.RawSegments)&&b.Movements.Zip(a.Movements).All(p=>Near(p.First.From,p.Second.From+delta)&&Near(p.First.To,p.Second.To+delta)&&p.First.StartTime==p.Second.StartTime),"C raw/prepared separation and movement translation");
 C(b.Checkpoints.Zip(a.Checkpoints).All(p=>Near(p.First.Position,p.Second.Position+delta)&&p.First.Time==p.Second.Time)&&ReferenceEquals(stacked.CircleSounds!.RegisteredOverrides.Single(o=>o.Slider is not null).Slider,b),"stack checkpoint/sound identity");
 File.WriteAllText(path,Map(row));Reject(()=>Beatmap.Load(path),"historical loader still excludes C");bool starRejected=false;try{StarAdmission.Parse(File.ReadAllText(path));}catch(Ohno.CircleGame.Difficulty.UnsupportedStarProfileException){starRejected=true;}C(starRejected,"R17 remains restricted");using var cts=new CancellationTokenSource();cts.Cancel();bool cancelled=false;try{Beatmap.LoadCanonical(path,cts.Token);}catch(OperationCanceledException){cancelled=true;}C(cancelled,"cancellation");
 // Follow a curve with sufficient deflection to discriminate it from its chord.
 File.WriteAllText(path,Map("100,100,1000,2,0,C|100:100|300:400|500:100,1,720"));var large=Beatmap.LoadCanonical(path);var def=large.Sliders[0];var good=new CircleSession(large,ScoreConfiguration.EngineeringPractice,false);var wrong=new CircleSession(large,ScoreConfiguration.EngineeringPractice,false);foreach(var s in new[]{good,wrong}){s.Press(def.Head.Position,1000);s.Advance(1000,def.Head.Position,1);}int mid=1000+(def.EndTime-1000)/2;good.Advance(mid,def.BallPosition(mid),1);wrong.Advance(mid,new(300,100),1);C(good.Sliders[0].FollowEligible&&!wrong.Sliders[0].FollowEligible,"actual C follow is not head-tail chord");
 C(File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Rendering/PlayfieldView.cs")).Contains("foreach (var s in source.Segments)"),"SOURCE render consumes shared prepared segments; GUI not executed");
 Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outFile))!);File.WriteAllText(outFile,JsonSerializer.Serialize(new{classification="Work2 development, independent Work1 QA pending",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local hashes",runtime=Environment.Version.ToString(),productionHash=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,"reconstructed/CircleGame/Gameplay/Beatmap.cs")))),checks=checks.Count,fixtureCount=cases.Count,cases,results=checks,originalRuntime="NOT RUN"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine($"C001 CATMULL DEVELOPMENT: {checks.Count}/{checks.Count} PASS; {cases.Count} dispositions");return 0;
 }finally{Directory.Delete(temp,true);}}
}
