using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Reflection;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Platform;
using Ohno.CorpusAudit;
partial class Program {
 static List<object> checks=new(),outcomes=new(),probes=new(),budgets=new();
 static void C(bool pass,string name){checks.Add(new{pass,name});if(!pass)Console.WriteLine("FAIL "+name);}
 static string Hash(string p)=>Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(p)));
 static void Geometry(SliderDefinition s,JsonElement c,int n,string label){
  C(s.Traversals==n&&s.Movements.Count==n*s.Geometry.Segments.Count,label+" R times prepared-segment work");
  C(s.Checkpoints.Count>=n&&s.Checkpoints.Count/s.Traversals>0,label+" nonzero N/R");
  C(s.EndPosition==(n%2==1?s.Tail:s.Head.Position),label+" odd/even final endpoint");
  for(int k=0;k<n;k++){var m=s.TraversalEndMovement(k);C(m.To==(k%2==0?s.Tail:s.Head.Position),label+" traversal endpoint "+k);}
  var expected=c.GetProperty("original").GetProperty("schedule");
  if(expected.ValueKind==JsonValueKind.Array)C(s.Checkpoints.Select(x=>x.Time).SequenceEqual(expected.EnumerateArray().Select(x=>x.GetInt32()))&&s.EndTime==c.GetProperty("original").GetProperty("end").GetInt32(),label+" independent straight300 schedule/end");
  if(c.GetProperty("id").GetString()!.StartsWith("fraction-duration",StringComparison.Ordinal))C(s.EndTime==(int)(2000+501.25*n),label+" rational fractional accumulation (not per-span truncate)");
  C(s.TerminalTime==Math.Max(s.Head.StartTime+(s.EndTime-s.Head.StartTime)/2,s.Checkpoints[^1].Time),label+" terminal time retains finite midpoint bound");
 }
 sealed class Sink:ISamplePlaybackSink{public List<SamplePlaybackRequest> Requests=new();public void Enqueue(SamplePlaybackRequest r)=>Requests.Add(r);}
 static async Task Main(string[] args){
  string root=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]),home=Path.Combine(root,"re/campaigns/C001/traversal-profile"),fixtures=Path.Combine(home,"fixtures");Directory.CreateDirectory(output);
  var cases=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"manifest.json"))).RootElement.GetProperty("cases");
  var baseline=JsonDocument.Parse(File.ReadAllText(Path.Combine(home,"results/results.json"))).RootElement.GetProperty("probes").EnumerateArray().ToDictionary(x=>x.GetProperty("id").GetString()!,x=>x.Clone());
  var audit=AuditRunner.Run(fixtures);if(Environment.GetEnvironmentVariable("GITHUB_SHA") is {} sha)C(audit.SourceCommit==sha&&audit.SourceState=="clean","exact clean compiled CI source");
  C(audit.Complete&&audit.Charts.Count==106,"complete actual corpus scan");C(AuditRunner.Json(audit)==AuditRunner.Json(AuditRunner.Run(fixtures)),"deterministic read-only audit");
  string tmp=Path.Combine(Path.GetTempPath(),"i18-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(tmp);int admitted=0,rejected=0,added=0;
  try{
   string pack=Path.Combine(tmp,"pack");Directory.CreateDirectory(pack);
   foreach(string bank in new[]{"normal","soft","drum"})foreach(string family in new[]{"hitnormal","hitwhistle","hitfinish","hitclap","slidertick","sliderslide","sliderwhistle"}){using var w=new BinaryWriter(File.Create(Path.Combine(pack,bank+"-"+family+".wav")));w.Write("RIFF"u8.ToArray());w.Write(292);w.Write("WAVEfmt "u8.ToArray());w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(8000);w.Write(16000);w.Write((short)2);w.Write((short)16);w.Write("data"u8.ToArray());w.Write(256);w.Write(new byte[256]);}
   using var library=new HitSoundLibrary(new HitSoundSettings{DefaultPackDirectory=pack});
   foreach(var c in cases.EnumerateArray()){
    string id=c.GetProperty("id").GetString()!,path=Path.Combine(home,c.GetProperty("path").GetString()!);string hash=Hash(path);var stamp=File.GetLastWriteTimeUtc(path);C(hash.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable bytes");
    var indexed=LocalChartCatalog.Index(path);C(indexed.Availability.ToString()==c.GetProperty("current").GetProperty("index").GetString(),id+" index unchanged");
    Beatmap? map=null;string? message=null;try{map=indexed.LoadCanonicalGameplay();}catch(InvalidDataException e){message=e.Message;}
    bool expected=c.GetProperty("futureCandidate").GetBoolean();C((map is not null)==expected,id+" actual new canonical disposition");
    var row=audit.Charts.Single(x=>x.EntryKey==Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(id+".osu"))))!;C((row.Canonical.Status=="success")==expected,id+" actual audit agrees");
    if(!expected){string guard=c.GetProperty("current").GetProperty("guard").GetString()!;string error=guard=="duration"?"R05 requires each traversal >= 1 ms and parent end <= 3600000 ms.":guard=="checkpoints"?"R05 accepts at most 4096 checkpoints per slider.":"Canonical supports 1–32 slider traversals (span count includes the first traversal).";C(message==error&&row.Canonical.ExceptionType=="InvalidDataException",id+" exact guard type/message");rejected++;}
    if(map is not null){admitted++;if(!c.GetProperty("current").GetProperty("prepared").GetBoolean())added++;
     var s=map.Sliders.Single();int n=s.Traversals;Geometry(s,c,n,id);var data=s.Samples!;
     C(map.Document!.SourceSha256==hash&&data.RawTraversals==n&&data.EffectiveTraversals==n,id+" genuine canonical raw/effective provenance");
     C(data.EdgeSubset.Count==(s.Checkpoints.Count-1)/(s.Checkpoints.Count/n)&&data.Schedule.SequenceEqual(s.Checkpoints.Select(x=>x.Time)),id+" exact N/R floor and complete schedule");
     C((data.Error is not null)==id.Contains("bad-",StringComparison.Ordinal),id+" malformed metadata Error versus canonical preparation");
     if(!data.Simple&&data.Error is null){var raw=c.GetProperty("edge").GetString()!.Split('|').Take(n+1).Select(x=>int.TryParse(x,out int value)?value:0).ToList();while(raw.Count<n+1)raw.Add(2);C(data.EdgeSounds.SequenceEqual(raw)&&data.Bases.Count==n+1,id+" exact ignored-TryParse and raw+1 short/full/long arrays");}
     if(baseline.TryGetValue(id,out var prior)){C(s.Checkpoints.Select(x=>x.Time).SequenceEqual(prior.GetProperty("schedule").EnumerateArray().Select(x=>x.GetInt32()))&&s.EndTime==prior.GetProperty("end").GetInt32(),id+" unchanged schedule versus sealed old-source component observations");}
     var score=ScoreConfiguration.FromBeatmap(map);var calibrated=CircleSession.CreateFreshOrdinary(map,score);var health=HealthChart.FromMap(map,calibrated.Approach,HealthPreparationPolicy.E036FreshOrdinary);
     C(calibrated.Health is not null&&health.Objects.Count==1&&health.NewComboProvenance[0].FinalFlag&&map.HealthSource!.IsFreshFor(map),id+" actual fresh score calibrated health NewCombo");
     C(ReferenceEquals(calibrated.Sliders[0].Definition,s)&&map.CircleSounds!.RegisteredOverrides.Any(x=>ReferenceEquals(x.Slider,s)),id+" actual registered session/sample identities");
     if(data.Error is null){using var fx=await library.PrepareAsync(map,default);C(ReferenceEquals(fx.PreparedMap,map)&&fx.Sliders.ContainsKey(s),id+" actual canonical audio-resource owner");
      var timing=map.CircleSounds!.Timing;var policy=new CircleSoundPolicy(true,true,true,0,100,false,80,100);var samples=fx.Sliders[s];
      foreach(int edge in new[]{0,n-1,n,n+1}){var selected=SliderSampleSelection.Edge(s,data,timing,policy,edge);int query=edge==0?s.Head.StartTime+5:edge>=n?s.EndTime+5:data.EdgeSubset[edge-1]+5;var expectedPoint=timing.LastOrDefault(x=>x.Time<=query)??timing[0];C(selected.Volume==expectedPoint.Volume&&selected.Index==expectedPoint.Index,id+" exact selected edge timing operands "+edge);}
      var live=CircleSession.CreateFreshOrdinary(map,score,false);
      var sink=new Sink();using var recording=new RecordingSampleChannels();var channels=new SharedSliderChannels(new GlobalSampleTiming(timing),recording,fx.Resources.ResolveContinuing){SelectedFormat=data.Format,SongFormat=data.Format};
      int events=0;live.SliderSound+=e=>{events++;SliderSampleSelection.Request(e,samples,timing,fx.Resources,policy,sink);};live.ContinuingSound+=(state,t,active)=>{if(fx.ContinuousLimitation is not null)return;channels.Clock(t);if(active)channels.Submit(2,1,1,true,state.Definition.Head.Position.X);else channels.QueuePause();channels.Drain();};
      live.Press(s.Head.Position,s.Head.StartTime);
      for(int t=s.Head.StartTime;t<=s.EndTime+10;t+=5)live.Advance(t,s.RequiredPosition(Math.Min(t,s.EndTime)),1);channels.QueuePause();channels.Drain();
      C(sink.Requests.Count>0&&sink.Requests.All(x=>ReferenceEquals(x.Event?.Parent,s))&&recording.Operations.Where(x=>x.Name=="Acquire").All(x=>fx.Samples.ContainsKey(x.Channel))&&
       (fx.ContinuousLimitation is null?recording.Operations.Any(x=>x.Name=="Play")&&recording.Operations.Any(x=>x.Name=="Pause"):data.Format<14&&!recording.Operations.Any(x=>x.Name=="Play")),id+" canonical discrete and admitted continuing channels resolve actual owned resources; legacy limitation retained");
      var state=live.Sliders[0];double ratio=(state.SuccessfulCheckpoints+1d)/(state.Checkpoints.Count+1d);var grade=ratio==1?Judgement.Hit300:ratio>=.5?Judgement.Hit100:ratio>0?Judgement.Hit50:Judgement.Miss;
      C(state.Head.Result==Judgement.Hit300&&state.Result==grade&&state.ProcessedCheckpoints<=state.Checkpoints.Count,id+" E17 actual success aggregate without pending drain");
      if(baseline.TryGetValue(id,out prior))C(state.Result.ToString()==prior.GetProperty("sessionResult").GetString()&&state.Checkpoints.Count-state.ProcessedCheckpoints==prior.GetProperty("pending").GetInt32(),id+" sealed observed result/pending preserved");
      channels.Submit(2,1,1,true,null);channels.Cancel();C(channels.Pending==0,id+" audio queued cancellation cleanup");
      using(var cancelled=new CancellationTokenSource()){cancelled.Cancel();PreparedCircleEffects? partial=null;bool cancelledOwner=false;try{partial=await library.PrepareAsync(map,cancelled.Token);}catch(OperationCanceledException){cancelledOwner=true;}partial?.Dispose();C(cancelledOwner&&partial is null,id+" cancelled real resource preparation publishes no partial owner");}
      probes.Add(new{id,n,s.EndTime,checkpoints=s.Checkpoints.Count,movements=s.Movements.Count,result=state.Result.ToString(),pending=state.Checkpoints.Count-state.ProcessedCheckpoints,events});
     }else{bool unavailable=false;try{using var fx=await library.PrepareAsync(map,default);unavailable=fx.Error is not null;}catch(InvalidDataException){unavailable=true;}C(unavailable,id+" sample Error preserves prepared/not-audio-ready distinction");}
    }
    if(c.GetProperty("rawTraversals").ValueKind==JsonValueKind.Number){int n=c.GetProperty("rawTraversals").GetInt32();if(n>=17){bool metadata=false;try{SliderSoundMetadata.Parse(2,n,c.GetProperty("version").GetInt32(),0,null,null,null);}catch(InvalidDataException e){metadata=e.Message=="R12 retains the ENGINEERING 1–16 traversal admission bound.";}C(metadata,id+" legacy public metadata guard preserved");}}
    C(Hash(path)==hash&&File.GetLastWriteTimeUtc(path)==stamp,id+" immutable after consumers");outcomes.Add(new{id,fixtureSha256=hash,previouslyPrepared=c.GetProperty("current").GetProperty("prepared").GetBoolean(),prepared=map is not null,message,audit=row.Canonical,sampleError=map?.Sliders[0].Samples!.Error});
   }
   C(admitted==86&&rejected==20&&added==45,"all106 individual targets;45 additions and zero old losses");
   Stress(root,tmp);
  }finally{Directory.Delete(tmp,true);}
  int failed=checks.Count(x=>!JsonSerializer.SerializeToElement(x).GetProperty("pass").GetBoolean());File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{actor="Work2 development; independent Work1 QA pending",source=audit.SourceCommit,state=audit.SourceState,sourceHashes=audit.SourceHashes,runtime=Environment.Version.ToString(),passed=checks.Count-failed,failed,fixtures=106,admitted,rejected,added,checks,outcomes,probes,budgets,originalRuntime="NOT RUN"},new JsonSerializerOptions{WriteIndented=true}));Console.WriteLine($"Issue18 development {checks.Count-failed}/{checks.Count} PASS;{admitted} prepared/{rejected} excluded;{added} additions");Environment.ExitCode=failed==0?0:1;
 }
}
