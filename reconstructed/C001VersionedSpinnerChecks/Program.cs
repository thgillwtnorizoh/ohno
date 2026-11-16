using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Difficulty;

internal sealed class Sink : ISamplePlaybackSink { public List<SamplePlaybackRequest> Requests { get; }=[];public void Enqueue(SamplePlaybackRequest request)=>Requests.Add(request); }
internal static class Program
{
    private static readonly List<string> checks=[];
    private static void C(bool ok,string name){if(!ok)throw new Exception("CHECK FAILED: "+name);checks.Add(name);}
    private static void Reject(Action run,string why){try{run();throw new Exception("Unexpected admission: "+why);}catch(InvalidDataException){C(true,why);}}
    private static string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes));
    private static void Wave(string file)
    {
        using var s=File.Create(file);using var b=new BinaryWriter(s);int size=1600;
        b.Write("RIFF"u8);b.Write(36+size);b.Write("WAVEfmt "u8);b.Write(16);b.Write((short)1);b.Write((short)1);b.Write(8000);b.Write(16000);b.Write((short)2);b.Write((short)16);b.Write("data"u8);b.Write(size);b.Write(new byte[size]);
    }
    private static string Play(Beatmap map)
    {
        var score=ScoreConfiguration.FromBeatmap(map);var session=CircleSession.CreateFreshOrdinary(map,score,false);var stages=new List<string>();session.SpinnerSound+=(s,e)=>stages.Add(e+":"+s.Observation.Count);
        var times=new SortedSet<int>(Enumerable.Range(0,460).Select(i=>i*17));
        foreach(var c in map.Circles)times.Add(c.StartTime);
        foreach(var s in map.Sliders){times.Add(s.Head.StartTime);times.Add(s.EndTime+1);foreach(var cp in s.Checkpoints)times.Add(cp.Time);}
        foreach(var s in map.Spinners){times.Add(s.StartTime);times.Add(s.EndTime);times.Add(s.EndTime+1);}
        foreach(int t in times)
        {
            foreach(var c in map.Circles.Where(c=>c.StartTime==t))session.Press(c.Position,t);
            foreach(var s in map.Sliders.Where(s=>s.Head.StartTime==t))session.Press(s.Head.Position,t);
            Vector2 pointer=new(64,64);var slide=map.Sliders.FirstOrDefault(s=>t>=s.Head.StartTime&&t<=s.EndTime);if(slide is not null)pointer=slide.BallPosition(t);
            var spinner=map.Spinners.Single();double theta=(t-spinner.StartTime)*.02;var spinPointer=spinner.Center+new Vector2((float)Math.Cos(theta)*100,(float)Math.Sin(theta)*100);
            session.Advance(t,pointer,1,spinPointer);
        }
        C(session.Spinners.Single().Result!=Judgement.Pending,"actual session completes spinner");
        C(stages.Any(x=>x.StartsWith("ObservedChange:",StringComparison.Ordinal))&&stages.Any(x=>x.StartsWith("Cleanup:",StringComparison.Ordinal)),"actual session input emits progress and cleanup hooks");
        C(session.JudgedCount==map.Circles.Count+map.Sliders.Count+map.Spinners.Count,"actual whole-result registration count");
        return JsonSerializer.Serialize(new{session.Ledger.Score,session.Ledger.CurrentCombo,session.Ledger.MaximumCombo,session.Ledger.AccuracyFraction,session.Ledger.EventCount,session.JudgedCount,spinners=session.Spinners.Select(s=>new{s.Progress,s.Observation.Count,s.Observation.CachedInteger,s.Required,s.Acceleration,s.FinalRaw,s.Result}),stages});
    }
    private static async Task<int> Main(string[] args)
    {
        string root=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);var results=new List<object>();string temp=Path.Combine(Path.GetTempPath(),"ohno-versioned-spinner-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        try
        {
            string packet=Path.Combine(root,"re/campaigns/C001/versioned-spinners");var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(packet,"manifest.json"))).RootElement;
            string pack=Path.Combine(temp,"pack");Directory.CreateDirectory(pack);
            foreach(string bank in new[]{"normal","soft","drum"})foreach(string family in new[]{"hitnormal","hitwhistle","hitfinish","hitclap","slidertick","sliderslide","sliderwhistle"})Wave(Path.Combine(pack,bank+"-"+family+".wav"));
            foreach(string family in new[]{"spinnerspin","spinnerbonus"})Wave(Path.Combine(pack,family+".wav"));
            Wave(Path.Combine(temp,"synthetic.wav"));
            int admitted=0,rejected=0;
            foreach(var c in manifest.GetProperty("cases").EnumerateArray())
            {
                string id=c.GetProperty("id").GetString()!,source=Path.Combine(packet,c.GetProperty("path").GetString()!);byte[] bytes=File.ReadAllBytes(source);string hash=Hash(bytes);C(hash.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" authority fixture SHA");
                int version=c.GetProperty("version").GetInt32(),mode=c.GetProperty("mode").GetInt32();var e=c.GetProperty("expected");
                bool expected=mode==0&&version is >=5 and <=14&&e.TryGetProperty("duration",out var duration)&&duration.GetInt32()>0&&!id.Contains("fractional",StringComparison.Ordinal);
                string copy=Path.Combine(temp,id+".osu");File.WriteAllBytes(copy,bytes);var indexed=LocalChartCatalog.Index(copy);
                if(!expected)
                {
                    Reject(()=>indexed.LoadCanonicalGameplay(),id+" explicit engineering or malformed exclusion");rejected++;
                    results.Add(new{id,hash,admitted=false,index=indexed.Availability.ToString(),classification="engineering/negative control, not original-runtime rejection"});continue;
                }
                var map=indexed.LoadCanonicalGameplay();admitted++;C(indexed.Availability==ChartAvailability.Indexed,id+" catalog admitted");var def=map.Spinners.Single();var p=def.Preparation!;
                C(p is not null&&p.Format==version&&p.ConstructorFlags==(version<=8?12:8)&&p.ParserFlags==(c.GetProperty("fileBit4").GetBoolean()?12:8),id+" explicit constructor then parser overwrite");
                C(def.Center==new Vector2(256,192)&&def.StartTime==2000&&def.EndTime==5000&&def.HitSound==2,id+" constructor operands and raw sound");
                C(p!.EndpointPolicy==SpinnerEndpointPolicy.EngineeringUncappedHostLengthUnavailable&&p.CompatibilityPolicy==SpinnerCompatibilityPolicy.ModernWithoutRecordedSource,id+" honest uncapped/source-policy classification");
                if(id=="v13-conditional-end-cap")C(e.GetProperty("end").GetInt32()==3950&&def.EndTime==5000,"conditional original end-cap stays unintegrated without provider");
                var payload=map.CircleSounds!.Spinners.Single();var sample=e.GetProperty("storedSample");
                C(ReferenceEquals(payload.Object,def)&&payload.Payload.Object.StartTime==def.EndTime&&payload.Payload.Object.Position==def.Center,id+" end-time sample identity");
                C(payload.Payload.BaseSet==sample.GetProperty("base").GetInt32()&&payload.Payload.AdditionSet==sample.GetProperty("addition").GetInt32()&&payload.Payload.Index==sample.GetProperty("index").GetInt32()&&payload.Payload.Volume==sample.GetProperty("volume").GetInt32()&&payload.Payload.Filename==sample.GetProperty("filename").GetString()&&payload.Payload.StoredMask==2,id+" all sample fields and unnormalized raw sound");
                var score=ScoreConfiguration.FromBeatmap(map);var session=CircleSession.CreateFreshOrdinary(map,score);C(session.Health is not null,id+" actual calibrated ordinary health available");var chart=session.Health!.Chart;
                C(chart.Format==version&&map.HealthSource!.IsFreshFor(map)&&ReferenceEquals(chart.NewComboProvenance.Single(n=>n.RuntimeType==OriginalRegisteredType.SpinnerBase).Identity,def),id+" actual health source format/identity");
                var combo=chart.NewComboProvenance.Single(n=>n.RuntimeType==OriginalRegisteredType.SpinnerBase);C(combo.ConstructorFlag==((p.ConstructorFlags&4)!=0)&&combo.ParserFlag==((p.ParserFlags&4)!=0),id+" actual NewCombo consumes matching transition");
                C(chart.NewComboProvenance.Select(n=>n.FinalFlag).SequenceEqual(e.GetProperty("finalNoBreakFlags").EnumerateArray().Select(n=>n.GetBoolean())),id+" following circle/slider NewCombo");
                C(session.Spinners.Single().Required==15&&Math.Abs(session.Spinners.Single().Acceleration-.00108)<1e-12,id+" duration preparation independent of file version");
                if(version<14)Reject(()=>Beatmap.Load(source),id+" historical loader remains restricted");
                string current=Path.Combine(temp,"control.osu");File.WriteAllText(current,File.ReadAllText(source).Replace("format v"+version,"format v14",StringComparison.Ordinal));var modern=Beatmap.LoadCanonical(current);
                C(Play(map)==Play(modern),id+" matched modern-policy input/score/result/audio histories");
                using var library=new HitSoundLibrary(new(){DefaultPackDirectory=pack});using var assets=await library.PrepareAsync(map,CancellationToken.None);
                C(assets.SpinnerLimitation is null&&assets.Spinners.ContainsKey(def)&&assets.SpinHandle>0&&assets.BonusHandle>0,id+" actual sample library admits versioned spinner");
                C(assets.Samples[assets.SpinHandle].Loop&&!assets.Samples[assets.BonusHandle].Loop,id+" independent continuing and bonus resources");
                if(version<14)C(assets.ContinuousLimitation is not null,id+" existing old slider-loop exclusion retained");
                var backend=new RecordingSampleChannels{QueryLength=(_,_)=>8193};var queue=new Queue<Action>();var channel=new SharedSpinnerChannel(backend,()=>assets.SpinHandle,()=>1,()=>1,a=>queue.Enqueue(a));channel.Refresh();
                var sink=new Sink();SelectedCircleSample? selected=null;SpinnerSampleSelection.Completion(assets.Spinners[def],map.CircleSounds.Timing,def.Center,assets.Resources,new(true,false,true,0,100,false,100,100),sink,x=>selected=x);
                C(selected is not null&&sink.Requests.Count>0&&sink.Requests.All(r=>ReferenceEquals(r.Spinner,def)&&r.Kind==SoundEventKind.SpinnerCompletion),id+" actual end-time completion sample hook");
                var input=new SpinnerState(def,map.OverallDifficulty);input.Sound+=(_,stage)=>{if(stage==SpinnerSoundStage.ObservedChange)channel.Observe(input.Observation.Count,input.Required,true);else if(stage==SpinnerSoundStage.Motion)channel.Continue((float)input.Observation.Count/input.Required);else if(stage==SpinnerSoundStage.Cleanup)channel.QueuePause();};
                for(int t=2000;t<=5001;t+=17){input.SampleCursor(def.Center+new Vector2(100*MathF.Cos((t-2000)*.02f),100*MathF.Sin((t-2000)*.02f)),def.Center,t,17,true);input.Integrate(t,17);}input.Complete(5001);while(queue.TryDequeue(out var action))action();
                C(backend.Operations.Any(x=>x.Name=="Play")&&backend.Operations.Any(x=>x.Name=="Pause"),id+" state audio hooks reach recording persistent backend");
                C(Beatmap.LoadCanonical(source).Spinners.Single()==def,id+" stable reload construction provenance");C(File.ReadAllBytes(source).SequenceEqual(bytes),id+" authority bytes unchanged after consumers");
                results.Add(new{id,hash,admitted=true,version,index=indexed.Availability.ToString(),p.ConstructorFlags,p.ParserFlags,endpoint=p.EndpointPolicy.ToString(),sourcePolicy=p.CompatibilityPolicy.ToString(),scoreK=score.K,session.Spinners.Single().Required,healthFormat=chart.Format});
            }
            C(admitted==23&&rejected==10,"33 authority dispositions:23 admitted10 explicitly excluded");
            string text=File.ReadAllText(Path.Combine(packet,"fixtures/v05-single.osu")),path=Path.Combine(temp,"negative.osu");
            foreach(var pair in new[]{("sample","5000,0"),("empty sample","5000,"),("break","5000")})
            {
                string variant=pair.Item1=="break"?text.Replace("[HitObjects]","[Events]\n2,100,200\n[HitObjects]"):text.Replace("5000",pair.Item2);File.WriteAllText(path,variant);Reject(()=>Beatmap.LoadCanonical(path),"finite legacy exclusion "+pair.Item1);
            }
            foreach(var pair in new[]{("short timing",text.Replace("0,500,4,1,0,100,1,0","0,500")),("start zero",text.Replace("2000,8","0,8"))}){File.WriteAllText(path,pair.Item2);Reject(()=>Beatmap.LoadCanonical(path),"finite legacy exclusion "+pair.Item1);}
            foreach(var variant in new[]{text.Replace("StackLeniency:0","StackLeniency:0.7"),text.Replace("StackLeniency:0",string.Empty)})
            {
                File.WriteAllText(path,variant);var candidate=LocalChartCatalog.Index(path);var qualified=candidate.LoadCanonicalGameplay();
                C(candidate.Availability==ChartAvailability.Indexed&&qualified.MetadataSettings!.StackLeniency==.7f,"superseded scalar exclusion: genuine .7 RouteZ candidate");
                C(Beatmap.LoadCanonical(path).Spinners.Count==1&&CircleSession.CreateFreshOrdinary(qualified,ScoreConfiguration.FromBeatmap(qualified)).Health is not null,"RouteZ direct canonical and calibrated consumers");
                C(HitSoundLibrary.SpinnerAdmission(qualified) is null&&qualified.CircleSounds!.Spinners.Any(x=>ReferenceEquals(x.Object,qualified.Spinners.Single())),"RouteZ sample/audio identities");
            }
            File.WriteAllText(path,text.Replace("StackLeniency:0","StackLeniency:0.7").Replace("[HitObjects]","[HitObjects]\n256,192,1000,1,0"));Reject(()=>Beatmap.LoadCanonical(path),"RouteZ constructor-head proximity excluded");
            File.WriteAllText(path,text.Replace("8,2","136,2"));Reject(()=>Beatmap.LoadCanonical(path),"legacy mixed factory flags excluded");
            foreach(int format in new[]{7,8,11}){File.WriteAllText(path,text.Replace("format v5","format v"+format));var map=Beatmap.LoadCanonical(path);C(map.Spinners.Single().Preparation!.ConstructorFlags==(format<=8?12:8),"additional format boundary "+format);}
            File.WriteAllText(path,text.Replace("8,2,5000","8,17,5000"));var rawMap=Beatmap.LoadCanonical(path);
            C(rawMap.Spinners.Single().HitSound==17&&rawMap.CircleSounds!.Spinners.Single().Payload.StoredMask==17,"old spinner raw sound is not circle-normalized");
            File.WriteAllText(path,text.Replace("0,500,4,1,0,100,1,0","0,500,4,1,0,100,1,0\n5000,600,4,3,7,36,1,0\n5002,600,4,2,9,66,1,0"));var endMap=Beatmap.LoadCanonical(path);
            using(var library=new HitSoundLibrary(new(){DefaultPackDirectory=pack}))using(var effects=await library.PrepareAsync(endMap,CancellationToken.None))
            {
                var sink=new Sink();SelectedCircleSample? endSelection=null;var spinner=endMap.Spinners.Single();
                SpinnerSampleSelection.Completion(effects.Spinners[spinner],endMap.CircleSounds!.Timing,spinner.Center,effects.Resources,new(true,false,true,0,100,false,100,100),sink,s=>endSelection=s);
                C(endSelection is { Index:7,Volume:36,BaseSet:3 }&&sink.Requests.Count>0,"actual old spinner completion selects stored end, not start+5/end+5");
            }
            File.WriteAllText(path,File.ReadAllText(Path.Combine(packet,"fixtures/v05-mixed.osu"))+"\n300,300,7100,1,0\n");Reject(()=>Beatmap.LoadCanonical(path),"legacy profile rejects nonspinner interval overlap too");
            using var cts=new CancellationTokenSource();cts.Cancel();bool cancelled=false;try{Beatmap.LoadCanonical(path,cts.Token);}catch(OperationCanceledException){cancelled=true;}C(cancelled,"cancelled canonical preparation");
            bool starRejected=false;try{StarAdmission.Parse(text);}catch(UnsupportedStarProfileException){starRejected=true;}C(starRejected,"R17 remains restricted");
            Directory.CreateDirectory(output);File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{actor="Work2 development, independent Work1 QA pending",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local source hashes",runtime=Environment.Version.ToString(),checks=checks.Count,admitted,rejected,fixtureCount=results.Count,cases=results,results=checks,hostLengthCap="NOT INTEGRATED; ENGINEERING uncapped provider-unavailable endpoint",originalRuntime="NOT RUN",sourceHashes=new[]{"Gameplay/Beatmap.cs","Selection/LocalChartCatalog.cs","Platform/LocalHitSoundAssets.cs"}.ToDictionary(p=>p,p=>Hash(File.ReadAllBytes(Path.Combine(root,"reconstructed/CircleGame",p))))},new JsonSerializerOptions{WriteIndented=true})+"\n");Console.WriteLine($"VERSIONED SPINNER DEVELOPMENT: {checks.Count}/{checks.Count} PASS;23 admissions/10 exclusions;33 immutable fixtures");return 0;
        }
        finally{Directory.Delete(temp,true);}
    }
}
