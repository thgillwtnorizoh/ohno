using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Difficulty;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using CircleDefinition = Ohno.CircleGame.Difficulty.CircleDefinition;

internal static class Program
{
    static int groups,comparisons;static readonly List<object> results=[];static readonly List<object> charts=[];
    static void Pass(string label){results.Add(new{group=++groups,label,passed=true});Console.WriteLine($"PASS R17 {groups:000} {label}");}
    static void Yes(bool ok,string path=""){if(!ok)throw new Exception($"R17 group{groups+1}: {path}");}
    static void Eq<T>(T expected,T actual,string path=""){Yes(EqualityComparer<T>.Default.Equals(expected,actual),$"{path}: expected{expected} actual{actual}");}
    static void Near(double expected,double actual,string path="")
    {comparisons++;Yes(double.IsFinite(actual)&&Math.Abs(expected-actual)<=Math.Max(5e-13,Math.Abs(expected)*5e-12),$"{path}: expected{expected:R} actual{actual:R}");}
    static void Number(JsonElement e,double actual,string path)=>Near(e.GetDouble(),actual,path);
    static void Numbers(JsonElement e,IEnumerable<double> actual,string path){var a=actual.ToArray();Eq(e.GetArrayLength(),a.Length,path+" length");for(int i=0;i<a.Length;i++)Number(e[i],a[i],path+"/"+i);}
    static void Pattern(JsonElement e,PatternSnapshot actual){Eq(e.GetArrayLength(),4);for(int i=0;i<4;i++)Eq(e[i].GetInt32(),actual.Values[i],"pattern identity/content");}
    static void Patterns(JsonElement e,IReadOnlyList<PatternSnapshot> actual){Eq(e.GetArrayLength(),actual.Count);for(int i=0;i<actual.Count;i++)Pattern(e[i],actual[i]);}
    static void Reject(Action action){bool rejected=false;try{action();}catch(UnsupportedStarProfileException){rejected=true;}Yes(rejected,"expected explicit unsupported profile");}
    static async Task Until(Func<bool> ready)
    {var deadline=System.Diagnostics.Stopwatch.StartNew();while(!ready()){if(deadline.ElapsedMilliseconds>10000)throw new Exception("Controlled asynchronous boundary timeout");await Task.Yield();}}
    static string fixture="",repo="",temp="";
    static string Write(string name,string? content=null){string path=Path.Combine(temp,name+".osu");File.WriteAllText(path,content??fixture);return path;}
    static CircleDefinition[] Circles(JsonElement input)=>input.EnumerateArray().Select(x=>new CircleDefinition(x[0].GetInt32(),x[1].GetSingle(),x[2].GetSingle())).ToArray();
    static void Compare(JsonElement expected,StarCalculationResult actual,JsonElement terms)
    {
        var trace=actual.Trace!;var ep=expected.GetProperty("pairs");Eq(ep.GetArrayLength(),trace.Pairs.Count);
        for(int i=0;i<trace.Pairs.Count;i++)
        {
            var p=trace.Pairs[i];var e=ep[i];Eq(e.GetProperty("index").GetInt32(),p.Index);
            foreach(var(k,v)in new[]{("time",p.Time),("delta",p.Delta),("strain",p.Strain),("gap",p.Gap),("jump",p.Jump),("radius",(double)p.Preparation.Radius),("preempt",p.Preparation.Preempt),("window",p.Preparation.FullWindow)})Number(e.GetProperty(k),v,"pair/"+i+"/"+k);
            Numbers(e.GetProperty("position"),[p.Position.X,p.Position.Y],"position");Numbers(e.GetProperty("previous_position"),[p.PreviousPosition.X,p.PreviousPosition.Y],"previous position");
            foreach(var(k,v)in new[]{("angle",p.Angle),("axis",p.Axis)}){var n=e.GetProperty(k);if(n.ValueKind==JsonValueKind.Null)Eq(v,null,k);else Number(n,v!.Value,k);}
            var row=trace.Rows[i];var t=expected.GetProperty("trace")[i];Number(t.GetProperty("raw0"),row.Movement.Composite,"raw0");Number(t.GetProperty("raw1"),row.IntervalRaw,"raw1");
            if(i>1){Number(terms[i][0],row.Movement.Movement,"movement");Number(terms[i][1],row.Movement.Jump,"jump channel");Number(terms[i][2],row.Movement.VelocityAngle,"velocity1.45 channel");}
            else{Eq(row.Movement.Movement,0d);Eq(row.Movement.VelocityAngle,0d);}
            Numbers(t.GetProperty("states"),row.States,"component states");Numbers(t.GetProperty("outputs"),row.Outputs,"outputs");
            Number(t.GetProperty("rhythm_multiplier"),row.Rhythm.Multiplier,"rhythm");var rh=t.GetProperty("rhythm");Eq(rh.GetProperty("seed").GetInt32(),row.Rhythm.Seed);Eq(rh.GetProperty("capacity").GetInt32(),row.Rhythm.Capacity);
            Number(rh.GetProperty("tolerance"),row.Rhythm.Tolerance,"tolerance");Number(rh.GetProperty("unattenuated"),row.Rhythm.Sum,"rhythm sum");Number(rh.GetProperty("attenuation"),row.Rhythm.Attenuation,"count attenuation");
            var events=rh.GetProperty("events");Eq(events.GetArrayLength(),row.Rhythm.Steps.Count,"event count");
            for(int j=0;j<row.Rhythm.Steps.Count;j++)
            {
                var step=row.Rhythm.Steps[j];var want=events[j];Eq(want.GetProperty("k").GetInt32(),step.K);Eq(want.GetProperty("pair").GetInt32(),step.Pair);
                foreach(var(k,v)in new[]{("dt",step.Dt),("prior",step.Prior),("difference",step.Difference),("history_weight",step.HistoryWeight),("value",step.Value),("sum",step.Sum)})Number(want.GetProperty(k),v,"rhythm step/"+k);
                Pattern(want.GetProperty("group"),step.Group);Pattern(want.GetProperty("previous_group"),step.PreviousGroup);Patterns(want.GetProperty("saved"),step.Saved);Patterns(want.GetProperty("before_saved"),step.BeforeSaved);
                Eq(want.GetProperty("active").GetBoolean(),step.Active);Eq(string.Join('|',want.GetProperty("flags").EnumerateArray().Select(x=>x.GetString())),string.Join('|',step.Flags),"reached group branches");
            }
            var vis=t.GetProperty("visibility");Number(vis.GetProperty("raw"),row.Visibility.Raw,"visibility raw");
            if(i==0)continue; // Original early return: oracle's diagnostic-only dead-history expansion is not a reached path.
            foreach(var(k,v)in new[]{("backward",row.Visibility.Backward),("forward",row.Visibility.Forward),("angle",row.Visibility.Angle),("forward_root",row.Visibility.ForwardRoot),("velocity",row.Visibility.Velocity),("movement",row.Visibility.Movement),("short",row.Visibility.Short)})Number(vis.GetProperty(k),v,"visibility/"+k);
            if(vis.GetProperty("next_jump").ValueKind==JsonValueKind.Null)Eq(row.Visibility.NextJump,null);else Number(vis.GetProperty("next_jump"),row.Visibility.NextJump!.Value,"following jump");
            foreach(var(k,list)in new[]{("past",row.Visibility.Past),("future",row.Visibility.Future)}){var want=vis.GetProperty(k);Eq(want.GetArrayLength(),list.Count,"visibility count");for(int j=0;j<list.Count;j++){Eq(want[j][0].GetInt32(),list[j].Index);Number(want[j][1],list[j].Value,k);}}
            var angular=vis.GetProperty("angles");Eq(angular.GetArrayLength(),row.Visibility.Angles.Count,"angular count");
            for(int j=0;j<row.Visibility.Angles.Count;j++){var a=row.Visibility.Angles[j];Eq(angular[j][0].GetInt32(),a.Index);Number(angular[j][1],a.Value,"angular contribution");Number(angular[j][2],a.Cap,"angle cap");Number(angular[j][3],a.Operand,"angle operand");}
        }
        Numbers(expected.GetProperty("aggregates"),trace.Aggregates,"q0/q1/q2");Numbers(expected.GetProperty("component2_transformed"),trace.Component2Transformed,"first minute before sort");
        var segments=expected.GetProperty("processed_segments");Eq(segments.GetArrayLength(),trace.Envelope.Processed.Count,"mutable prefix segment count");
        for(int i=0;i<trace.Envelope.Processed.Count;i++)Numbers(segments[i],[trace.Envelope.Processed[i].Value,trace.Envelope.Processed[i].Duration],"duration segment");
        var final=expected.GetProperty("final");Number(final.GetProperty("double"),actual.CalculatedDouble,"final Double");
        Eq(final.GetProperty("single_bits_le").GetString(),actual.SingleBitsLittleEndian,"exact cached Single bits");Eq(BitConverter.SingleToInt32Bits(final.GetProperty("single").GetSingle()),BitConverter.SingleToInt32Bits(actual.CachedSingle));
        Eq(final.GetProperty("display").GetDouble().ToString("0.##",CultureInfo.InvariantCulture)+"★",actual.Display,"floor display");
    }
    static string Snapshot(CircleSession s)=>JsonSerializer.Serialize(new{s.Ledger.Score,s.Ledger.N300,s.Ledger.N100,s.Ledger.N50,s.Ledger.NMiss,s.Ledger.MaximumCombo,s.Ledger.CurrentCombo,s.Ledger.AccuracyFraction,s.Ledger.EventCount,s.JudgedCount,s.Finished,s.Scoring.K,objects=s.ObjectResults.ToArray(),sliders=s.Sliders.Select(x=>new{x.Tracking,x.Audio.Index,x.SuccessfulCheckpoints,x.FailedCheckpoints}),spinners=s.Spinners.Select(x=>new{x.Progress,x.SmoothedMotion,x.Observation.Count,x.Observation.CachedInteger}),health=s.Health is{} h?new{h.State.H,h.State.A,h.State.V,h.ResultEvents,h.Failed}:null});
    static async Task<int> Main(string[] args)
    {
        repo=args[0];CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;temp=Path.Combine(Path.GetTempPath(),"r17-dev-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        fixture=File.ReadAllText(Path.Combine(repo,"re/evidence/46/fixtures/combined-repeat-visibility.osu")).Replace("\r\n","\n");
        try
        {
            using var input=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(repo,"reconstructed/R17DevelopmentChecks/reference-inputs.json")));
            byte[] reference=File.ReadAllBytes(Path.Combine(repo,"re/evidence/46/results.json"));Eq(Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(System.Text.Encoding.UTF8.GetString(reference).Replace("\r\n","\n")))).ToLowerInvariant(),input.RootElement.GetProperty("authoritySha256").GetString());
            using var expected=JsonDocument.Parse(reference);Eq(expected.RootElement.GetProperty("charts").GetArrayLength(),28);Pass("pinned independent E046 expected reference;28 are fixtures, not automatic passing checks");
            foreach(var row in input.RootElement.GetProperty("rows").EnumerateArray())
            {
                string id=row.GetProperty("id").GetString()!;var p=row.GetProperty("params");var result=CircleStarCalculator.CalculatePrepared(Circles(row.GetProperty("circles")),new(p[0].GetSingle(),p[1].GetDouble(),p[2].GetDouble()));
                var want=expected.RootElement.GetProperty("charts").EnumerateArray().Single(x=>x.GetProperty("id").GetString()==id).GetProperty("expected");Compare(want,result,row.GetProperty("movement_terms"));
                charts.Add(new{id,result.CalculatedDouble,result.CachedSingle,result.SingleBitsLittleEndian,result.Display,aggregates=result.Trace!.Aggregates});
                Console.WriteLine($"R17 CHART {id}: Double={result.CalculatedDouble:R} SingleLE={result.SingleBitsLittleEndian} display={result.Display}");
                if(row.GetProperty("kind").GetString()!.StartsWith("BOUNDED RAW",StringComparison.Ordinal))
                {var chart=LocalChartCatalog.Index(Path.Combine(repo,"re/evidence/46/fixtures",id+".osu"));var raw=CircleStarCalculator.Calculate(chart,capture:true);Near(result.CalculatedDouble,raw.CalculatedDouble);Eq(result.SingleBitsLittleEndian,raw.SingleBitsLittleEndian);}
                Pass("executed full independent intermediate/chart comparison "+id);
            }
            using var e45=JsonDocument.Parse(File.ReadAllBytes(Path.Combine(repo,"re/evidence/45/controls.json")));
            var controls=e45.RootElement.GetProperty("cases").EnumerateArray().ToArray();
            for(int n=2;n<=4;n++)
            {
                int i=132+(n-2)*3;JsonElement C(int k)=>controls.Single(x=>x.GetProperty("id").GetString()=="H"+k);
                var admission=StarAdmission.Parse(C(i).GetProperty("input").GetProperty("text").GetString()!);var r=CircleStarCalculator.CalculatePrepared(admission.Circles,admission.Preparation);
                Number(C(i).GetProperty("expected"),r.CalculatedDouble,"E045 independent scalar");Eq(C(i+1).GetProperty("expected").GetString(),r.SingleBitsLittleEndian);Eq(C(i+2).GetProperty("expected").GetDouble().ToString("0.##")+"★",r.Display);
                Pass("E045 independently closed "+n+"-circle raw fixture Double/bits/display");
            }
            var admitted=StarAdmission.Parse(fixture);Eq(admitted.Preparation.Radius,32.013118743896484f);Eq(admitted.Preparation.Preempt,450d);Eq(admitted.Preparation.FullWindow,99d);Eq(CircleDifficultyPreparation.Operands(5,5,5).Preempt,1200d);Pass("original Single radius / AR interpolation / floored OD preparation");
            var pairs=CircleDifficultyPreparation.Prepare(admitted.Circles,admitted.Preparation);Yes(pairs[0].Angle is null&&pairs[1].Angle is null&&pairs[2].Angle is not null);Yes(ReferenceEquals(pairs[2].Previous(1),pairs[0]));Yes(ReferenceEquals(pairs[0].Following(),pairs[1]));Pass("registered N-1 pairs retain predecessor/following identity before sort and nullable early angles");
            var shortPairs=CircleDifficultyPreparation.Prepare([new(0,0,0),new(24,100,100)],new(50,1200,99));Eq(shortPairs[0].Delta,24d);Eq(shortPairs[0].Strain,25d);var wrapped=CircleDifficultyPreparation.Prepare([new(int.MaxValue,0,0),new(int.MinValue,0,0)],new(50,1200,99));Eq(wrapped[0].Delta,1d);Pass("unchecked Int32 subtraction before Double and separate Max25 strain");
            var anchor=pairs[^1];var rhythm=RhythmProcessor.Calculate(anchor);Near(1.065012169146155,rhythm.Multiplier);Near(.5652670333869401,rhythm.Sum);var insert=rhythm.Steps.First(s=>s.Flags.Contains("saved-insert"));Eq(insert.PreviousGroup.Identity,insert.Saved[0].Identity);Eq(string.Join(',',rhythm.Steps.Where(s=>s.Flags.Contains("saved-insert")||s.Flags.Contains("repeat-increment")).Select(s=>s.Saved[0].Repeats)),"1,2,3,4");Pass("closed repeated-group scalar and actual saved/last alias; repeats mutate1/2/3/4");
            var copied=insert.Saved[0] with{Identity=999};Eq(copied.Interval,insert.Saved[0].Interval);Eq(copied.Count,insert.Saved[0].Count);Yes(copied.Identity!=insert.PreviousGroup.Identity);var nonDiscriminator=expected.RootElement.GetProperty("cases").EnumerateArray().Single(x=>x.GetProperty("name").GetString()=="deep-copy scalar alone cannot establish reference identity");Yes(nonDiscriminator.GetProperty("passed").GetBoolean());Pass("identity graph discriminates equal-valued copy; retained oracle confirms scalar is a non-discriminator");
            foreach(var(dt,count)in new[]{(499,5),(500,4),(501,4),(600,4)})
            {var square=new[]{(0f,0f),(200f,0f),(200f,200f),(0f,200f)};var q=CircleDifficultyPreparation.Prepare(Enumerable.Range(0,13).Select(i=>new CircleDefinition(i*dt,square[i%4].Item1,square[i%4].Item2)).ToArray(),new(50,1200,99));var vis=VisibilityProcessor.Calculate(q[8]);Eq(vis.Angles.Count,count);Near(Math.Clamp(2/(count*(1-(dt-200d)/1800)),.2,1),vis.Angle);}
            Pass("visibility2000ms tests prior elapsed then processes crossing term;499/500/501/600");
            foreach(var(last,count)in new[]{(999,5),(1000,4),(1001,4)})
            {var ts=Enumerable.Range(0,11).Select(i=>i*1000).Append(10000+last);var q=CircleDifficultyPreparation.Prepare(ts.Select(t=>new CircleDefinition(t,256,192)).ToArray(),new(50,1200,99));var r=RhythmProcessor.Calculate(q[^1]);Eq(r.Steps.Count,count);Eq(r.Multiplier,1d);}
            Pass("strict5000ms seed boundary yields5/4/4 comparisons independently of neutral scalar");
            foreach(var(n,count)in new[]{(32,28),(33,29),(34,30),(35,30)}){var q=CircleDifficultyPreparation.Prepare(Enumerable.Range(0,n).Select(i=>new CircleDefinition(i,256,192)).ToArray(),new(50,1200,99));Eq(RhythmProcessor.Calculate(q[^1]).Steps.Count,count);}Pass("capacity32 is not32 synthetic comparisons;28/29/30/30 controls");
            Near(14.583333333333332,DifficultyAggregation.Ordered([10,5],20));Near(13,DifficultyAggregation.Ordered([10,5],1));Yes(DifficultyAggregation.Ordered([10,10],1)>10);Pass("ordered i^.9 s20/s1 weights and repeated values not deduplicated");
            var minute=CircleDifficultyPreparation.Prepare([new(0,0,0),new(100,0,0),new(60100,0,0),new(60101,0,0)],new(50,1200,99));var(values,minuteCount)=DifficultyAggregation.FirstMinute([10,0,20],minute);Eq(minuteCount,2);Eq(values[0],0d);Near(20*Math.Log10(5.5),values[1]);Pass("first-minute equality and earliest positive preprocessing precede descending sort");
            var envPairs=CircleDifficultyPreparation.Prepare([new(0,0,0),new(1000,0,0),new(1400,0,0),new(1401,0,0)],new(50,1200,99));var env=new DurationEnvelope();int calls=0;env.Process(envPairs[0],0,()=>{calls++;return 1;});env.Process(envPairs[1],1,()=>{calls++;return .5;});Eq(env.Finish().Trace.Segments.Count,1);Eq(calls,2);env=new();foreach(var p in envPairs)env.Process(p,1,()=>1);Yes(env.Finish().Trace.Segments.Count>=2);Pass("duration envelope first-pair origin, strict400 boundary, single callback per pair");
            DifficultyPair AtTime(DifficultyPair p,double time)=>new(){Index=p.Index,Time=time,Delta=p.Delta,Strain=p.Strain,Position=p.Position,PreviousPosition=p.PreviousPosition,Preparation=p.Preparation,Jump=p.Jump,Angle=p.Angle,Axis=p.Axis,History=p.History};
            foreach(var(span,stored)in new[]{(200.5,200d),(201.5,202d)}){var fractional=new DurationEnvelope();fractional.Process(AtTime(envPairs[0],1000.5),0,()=>1);fractional.Process(AtTime(envPairs[1],1000.5+span),1,()=>2);var finish=fractional.Finish();Yes(finish.Trace.Segments.Any(s=>s.Value==1&&s.Duration==stored));Near(span+400,finish.Trace.Retained);}Pass("ARTIFICIAL envelope tie-to-even storage remains separate from unrounded retained duration");
            Reject(()=>new CalculationBudget(maximumSteps:0).Step());using(var cancelled=new CancellationTokenSource()){cancelled.Cancel();bool stopped=false;try{CircleStarCalculator.CalculatePrepared(admitted.Circles,admitted.Preparation,cancelled.Token);}catch(OperationCanceledException){stopped=true;}Yes(stopped);}Pass("bounded work and cooperative cancellation never manufacture a valid zero");
            Reject(()=>CircleDifficultyPreparation.Prepare([new(0,float.NaN,0),new(100,0,0)],new(50,1200,99)));Reject(()=>CircleDifficultyPreparation.Prepare([new(0,float.MaxValue,0),new(100,-float.MaxValue,0)],new(50,1200,99)));Pass("nonfinite input or Single spacing rejected explicitly");
            var rejections=new Dictionary<string,string>{["format"]=fixture.Replace("format v14","format v15"),["mode"]=fixture.Replace("Mode:0","Mode:1"),["missing mode"]=fixture.Replace("Mode:0", ""),["slider"]=fixture.Replace(",1,0\n",",2,0\n"),["spinner"]=fixture.Replace(",1,0\n",",8,0\n"),["missing stacking"]=fixture.Replace("StackLeniency:0",""),["nonzero stacking"]=fixture.Replace("StackLeniency:0","StackLeniency:.7"),["invalid scalar"]=fixture.Replace("CircleSize:5.0","CircleSize:NaN"),["unverified CS"]=fixture.Replace("CircleSize:5.0","CircleSize:4"),["unverified OD"]=fixture.Replace("OverallDifficulty:5.0","OverallDifficulty:7"),["unverified AR"]=fixture.Replace("ApproachRate:10.0","ApproachRate:7"),["incomplete"]=fixture[..fixture.IndexOf("[HitObjects]",StringComparison.Ordinal)], ["extra sample operands"]=fixture.Replace(",1,0\n",",1,0,0:0:0:0:\n")};
            foreach(var(name,text)in rejections){Yes(text!=fixture,"mutant changed input "+name);Reject(()=>StarAdmission.Parse(text));Pass("strict raw admission rejects "+name);}
            const string header="osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Difficulty]\nCircleSize:5\nApproachRate:5\nOverallDifficulty:5\n[HitObjects]\n";
            foreach(var(name,objects)in new[]{("equal time","0,0,0,1,0\n10,10,0,1,0\n"),("negative time","0,0,-1,1,0\n10,10,100,1,0\n"),("out-of-order","0,0,100,1,0\n10,10,99,1,0\n"),("coordinate","0,0,0,1,0\n513,10,100,1,0\n"),("noninteger","0,0,0,1,0\nNaN,10,100,1,0\n"),("overflow time","0,0,0,1,0\n10,10,2147483648,1,0\n"),("one object","0,0,0,1,0\n")}){Reject(()=>StarAdmission.Parse(header+objects));Pass("strict raw admission rejects "+name);}
            foreach(var p in new[]{new StarProfile(Mods:8),new StarProfile(Width:1024),new StarProfile(Height:768),new StarProfile(TimeShift:1),new StarProfile(Rate:1.5)})Reject(()=>StarAdmission.Parse(fixture,p));Pass("modifiers viewport shift and rate outside independently verified raw profile rejected");
            var good=LocalChartCatalog.Index(Write("good"));var actualGood=CircleStarCalculator.Calculate(good);Near(5.119475874072683,actualGood.CalculatedDouble);Eq(actualGood.SingleBitsLittleEndian,"bfd2a340");Eq(actualGood.Display,"5.11★");Pass("combined raw operational discriminator Double/binary32/floor display");
            using(var cache=new CalculatedStarCache()){var missing=LocalChartCatalog.Index(Path.Combine(temp,"missing.osu"));Eq((await cache.GetAsync(missing)).State,StarCalculationState.Stale);var vanished=LocalChartCatalog.Index(Write("vanished"));File.Delete(vanished.Path);Eq((await cache.GetAsync(vanished)).State,StarCalculationState.Stale);}Pass("missing file is unavailable rather than zero stars");
            int calculations=0;using(var cache=new CalculatedStarCache((chart,ct)=>{Interlocked.Increment(ref calculations);return Task.FromResult(actualGood);}))
            {var r=await cache.GetAsync(good);Eq(r.State,StarCalculationState.Calculated);var again=await cache.GetAsync(good);Eq(calculations,1);Yes(ReferenceEquals(r,again));Yes(CalculatedStarCache.Key(good)!.CalculatorProfile.Contains("E044-E046"));}Pass("successful process-local content/profile cache reuses unchanged result and preserves provenance");
            var entered=new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);var release=new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);calculations=0;
            using(var cache=new CalculatedStarCache(async(chart,ct)=>{Interlocked.Increment(ref calculations);entered.TrySetResult(true);await release.Task;return actualGood;}))
            {var first=cache.GetAsync(good);await entered.Task;var duplicate=cache.GetAsync(good);await Until(()=>cache.JoinedPendingRequests==1);release.SetResult(true);Eq((await first).State,StarCalculationState.Calculated);Eq((await duplicate).State,StarCalculationState.Calculated);Eq(calculations,1);}Pass("duplicate requests coalesce to one active calculator operation");
            var two=LocalChartCatalog.Index(Write("two",header+"0,192,0,1,0\n100,192,1000,1,0\n"));
            using(var cache=new CalculatedStarCache())using(var controller=new SelectPlayStarController(cache))
            {controller.Highlight(good,1);await controller.Completion;Eq(controller.StateFor(good).State,StarCalculationState.Calculated);controller.Highlight(two,1);Eq(controller.StateFor(good).State,StarCalculationState.NotCalculated);await controller.Completion;Eq(controller.StateFor(two).Result!.Display,"0.11★");controller.Invalidate();Eq(controller.StateFor(two).State,StarCalculationState.Cancelled);controller.Highlight(two,2);await controller.Completion;Eq(controller.StateFor(two).State,StarCalculationState.Calculated);}Pass("actual asynchronous highlight completion, different-chart isolation and unchanged revision after refresh");
            entered=new(TaskCreationOptions.RunContinuationsAsynchronously);release=new(TaskCreationOptions.RunContinuationsAsynchronously);
            using(var cache=new CalculatedStarCache(async(chart,ct)=>{entered.TrySetResult(true);await release.Task;return actualGood;}))using(var controller=new SelectPlayStarController(cache))
            {controller.Highlight(good,1);await entered.Task;var old=controller.Completion;controller.Invalidate();controller.Highlight(two,2);release.SetResult(true);await old;await controller.Completion;Eq(controller.StateFor(good).State,StarCalculationState.NotCalculated);Eq(controller.StateFor(two).State,StarCalculationState.Calculated);}Pass("obsolete completion after refresh/selection switch cannot publish into newer chart reference");
            entered=new(TaskCreationOptions.RunContinuationsAsynchronously);release=new(TaskCreationOptions.RunContinuationsAsynchronously);
            using(var cache=new CalculatedStarCache(async(chart,ct)=>{entered.TrySetResult(true);await release.Task;return actualGood;}))using(var controller=new SelectPlayStarController(cache))
            {controller.Highlight(good,1);await entered.Task;int notifications=0;controller.Changed+=()=>notifications++;controller.Dispose();cache.Dispose();release.SetResult(true);await controller.Completion;await Until(()=>cache.PendingCount==0);Eq(notifications,0);Eq(cache.CompletedCount,0);}Pass("stale completion after controller/cache disposal cannot notify or install resources");
            string changedPath=Write("changed");var changed=LocalChartCatalog.Index(changedPath);using(var cache=new CalculatedStarCache()){await cache.GetAsync(changed);File.AppendAllText(changedPath,"\n// revision\n");Eq((await cache.GetAsync(changed)).State,StarCalculationState.Stale);var replacement=LocalChartCatalog.Index(changedPath);Yes(CalculatedStarCache.Key(changed)!=CalculatedStarCache.Key(replacement));Eq((await cache.GetAsync(replacement)).State,StarCalculationState.Calculated);}Pass("same path changed content invalidates old cache identity; new revision can calculate");
            entered=new(TaskCreationOptions.RunContinuationsAsynchronously);release=new(TaskCreationOptions.RunContinuationsAsynchronously);var during=LocalChartCatalog.Index(Write("during"));
            using(var cache=new CalculatedStarCache(async(chart,ct)=>{entered.TrySetResult(true);await release.Task;return actualGood;})){var pending=cache.GetAsync(during);await entered.Task;File.AppendAllText(during.Path,"\n// changed during calculation\n");release.SetResult(true);Eq((await pending).State,StarCalculationState.Stale);Eq(cache.CompletedCount,0);}Pass("revision changed while actual cache job is held fails post-calculation verification and cannot cache numeric result");
            using(var cache=new CalculatedStarCache())using(var controller=new SelectPlayStarController(cache)){controller.Highlight(good,5);await controller.Completion;controller.Highlight(two,4);Eq(controller.StateFor(good).State,StarCalculationState.Calculated);controller.Dispose();controller.Highlight(two,6);Eq(controller.StateFor(two).State,StarCalculationState.NotCalculated);}Pass("stale catalog epoch and disposed controller reject publication");
            using(var cache=new CalculatedStarCache((chart,ct)=>throw new IOException("controlled load failure")))Eq((await cache.GetAsync(good)).State,StarCalculationState.Stale);using(var cache=new CalculatedStarCache((chart,ct)=>throw new InvalidOperationException("controlled calculator failure")))Eq((await cache.GetAsync(good)).State,StarCalculationState.Failed);Pass("file/calculator exceptions contained as typed nonnumeric states");
            using(var cache=new CalculatedStarCache())
            {var playable=LocalChartCatalog.Index(Path.Combine(repo,"reconstructed/CircleGame/Fixtures/discrete-slider-r12.osu"));Eq((await cache.GetAsync(playable)).State,StarCalculationState.Unsupported);Yes(playable.Available);Yes(playable.LoadGameplay().Sliders.Count>0);}Pass("unsupported star chart remains playable without metadata/accounting mutation");
            using(var cache=new CalculatedStarCache((chart,ct)=>Task.FromResult(actualGood)))
            {for(int i=0;i<CalculatedStarCache.MaximumEntries+1;i++){var row=LocalChartCatalog.Index(Write("bounded"+i,fixture+"\n//"+i));await cache.GetAsync(row);}Eq(cache.CompletedCount,CalculatedStarCache.MaximumEntries);}Pass("completed cache eviction is bounded to64 process-local entries");
            foreach(string filename in new[]{"first-circles.osu","discrete-slider-r12.osu","spinner-practice.osu","mixed-r07.osu","combined-repeat-visibility.osu"})
            {
                string path=Path.Combine(repo,filename=="combined-repeat-visibility.osu"?"re/evidence/46/fixtures":"reconstructed/CircleGame/Fixtures",filename);var oldMap=Beatmap.Load(path);var chart=LocalChartCatalog.Index(path);var map=chart.LoadGameplay();using(var cache=new CalculatedStarCache()){var rating=await cache.GetAsync(chart);if(filename=="combined-repeat-visibility.osu")Eq(rating.State,StarCalculationState.Calculated);}
                var a=CircleSession.CreateFreshOrdinary(oldMap,ScoreConfiguration.FromBeatmap(oldMap));var b=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));using var ra=new RankSession(a);using var rb=new RankSession(b);var ax=new List<string>();var bx=new List<string>();
                foreach(var(s,list)in new[]{(a,ax),(b,bx)}){s.PositiveCircleSound+=d=>list.Add("circle:"+d.StartTime);s.SliderSound+=d=>list.Add("slider:"+d.Kind+":"+d.Index);s.SpinnerSound+=(d,e)=>list.Add("spinner:"+e+":"+d.Observation.Count);s.ContinuingSound+=(d,t,on)=>list.Add("loop:"+t+":"+on);}
                for(int t=0;t<=ra.MaximumEnd+3016;t+=16){var pointer=SpinnerState.DisplayCenter+new Vector2((float)(100*Math.Cos(t*.04)),(float)(100*Math.Sin(t*.04)));a.Advance(t,pointer,1);b.Advance(t,pointer,1);Eq(Snapshot(a),Snapshot(b));foreach(var r in new[]{ra,rb}){r.Observe(t,continuation:t<=r.MaximumEnd);r.Complete();r.Observe(t,continuation:t<=r.MaximumEnd);}}
                Eq(string.Join('|',ax),string.Join('|',bx));Eq(ra.HandlerRecord.Grade(),rb.HandlerRecord.Grade());Eq(ra.HandlerRecord.Score,rb.HandlerRecord.Score);
            }
            Pass("five matched before/after metadata calculation sessions including an admitted calculator chart preserve parser K events judgement HP audio Rank histories");
            string host=File.ReadAllText(Path.Combine(repo,"reconstructed/CircleGame/Program.cs"));Yes(host.Contains("stars.Highlight(browser.Highlighted, browser.CatalogGeneration)"));Yes(host.Contains("stars.Invalidate()"));Yes(host.Contains("stars.Dispose(); starCache.Dispose()"));Yes(host.Contains("Dispatcher.BeginInvoke(new Action(() => { if (!closing && selectionVisible) browserView.Bind(); }))"));Pass("SOURCE INSPECTION host highlight/refresh/Play/close generations and UI dispatcher publication");
            Console.WriteLine($"R17 DEVELOPMENT: {groups} groups passed; {comparisons} finite scalar comparisons; last1 source inspection; research controls not added to independent QA");Console.WriteLine("R17 EVIDENCE "+Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(new{groups,comparisons,results,charts})));return 0;
        }
        finally{Directory.Delete(temp,true);}
    }
}
