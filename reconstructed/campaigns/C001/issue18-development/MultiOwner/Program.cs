using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CorpusAudit;
string root=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);
string temp=Path.Combine(Path.GetTempPath(),"i18-multi-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
var checks=new List<string>();void Check(bool ok,string name){if(!ok)throw new Exception(name);checks.Add(name);}
try{
 string template=File.ReadAllText(Path.Combine(root,"re/campaigns/C001/traversal-profile/fixtures/v14-L-r17.osu"));
 string text=template[..(template.IndexOf("[HitObjects]",StringComparison.Ordinal)+"[HitObjects]".Length)]+"\n128,192,2000,2,2,L|428:192,17,300\n128,192,30000,2,2,L|428:192,32,300\n128,192,80000,2,2,L|428:192,24,300\n";
 string path=Path.Combine(temp,"multi.osu");File.WriteAllText(path,text);var map=Beatmap.LoadCanonical(path);var score=ScoreConfiguration.FromBeatmap(map);var calibrated=CircleSession.CreateFreshOrdinary(map,score);
 Check(calibrated.Health is not null&&map.HealthSource!.IsFreshFor(map)&&map.Sliders.Select(s=>s.Traversals).SequenceEqual(new[]{17,32,24}),"actual fresh multi-parent score/health/source");
 string pack=Path.Combine(temp,"pack");Directory.CreateDirectory(pack);
 foreach(string bank in new[]{"normal","soft","drum"})foreach(string family in new[]{"hitnormal","hitwhistle","hitfinish","hitclap","slidertick","sliderslide","sliderwhistle"}){using var w=new BinaryWriter(File.Create(Path.Combine(pack,bank+"-"+family+".wav")));w.Write("RIFF"u8.ToArray());w.Write(292);w.Write("WAVEfmt "u8.ToArray());w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(8000);w.Write(16000);w.Write((short)2);w.Write((short)16);w.Write("data"u8.ToArray());w.Write(256);w.Write(new byte[256]);}
 using var library=new HitSoundLibrary(new(){DefaultPackDirectory=pack});using var fx=await library.PrepareAsync(map,default);
 Check(ReferenceEquals(fx.PreparedMap,map)&&fx.Sliders.Count==3&&map.Sliders.All(s=>ReferenceEquals(fx.Sliders[s].Parent,s)&&calibrated.Sliders.Any(state=>ReferenceEquals(state.Definition,s))&&map.CircleSounds!.RegisteredOverrides.Any(o=>ReferenceEquals(o.Slider,s))),"all three actual registered session/resource owners");
 var live=CircleSession.CreateFreshOrdinary(map,score,false);
 for(int t=0;t<=map.Sliders[^1].EndTime+10;t+=5){foreach(var s in map.Sliders.Where(s=>s.Head.StartTime==t))live.Press(s.Head.Position,t);var active=map.Sliders.FirstOrDefault(s=>t>=s.Head.StartTime&&t<=s.EndTime);live.Advance(t,active?.RequiredPosition(t)??Vector2.Zero,1);}
 Check(live.JudgedCount==3&&live.Sliders.All(s=>s.Head.Result==Judgement.Hit300&&s.Result!=Judgement.Pending),"actual multi-parent input/results registration");
 var audit=AuditRunner.Run(temp,new(MaxDepth:0));Check(audit.SourceCommit=="6a152648c72773eb33d37f8ac01f18ca35ab1f41"&&audit.SourceState=="clean","exact compiled production source");
 File.WriteAllText(output,JsonSerializer.Serialize(new{source=audit.SourceCommit,state=audit.SourceState,sourceHashes=audit.SourceHashes,runtime=Environment.Version.ToString(),checks=checks.Count,passed=checks.Count,failed=0,assertions=checks,counts=map.Sliders.Select(s=>s.Traversals),results=live.Sliders.Select(s=>s.Result.ToString()),originalRuntime="NOT RUN"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine($"Supplemental multi-parent ownership {checks.Count}/{checks.Count} PASS");
}finally{Directory.Delete(temp,true);}
