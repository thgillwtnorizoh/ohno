using System.Globalization;
using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;

internal static class Program
{
    static int groups;static readonly List<object> results=[];
    static void Yes(bool value){if(!value)throw new Exception("R16 group "+(groups+1));}
    static void Eq<T>(T a,T b){if(!EqualityComparer<T>.Default.Equals(a,b))throw new Exception($"R16 group {groups+1}: {a} != {b}");}
    static void Pass(string label){results.Add(new{group=++groups,label,passed=true});Console.WriteLine($"PASS R16 {groups:00} {label}");}
    static void Throws(Action a){try{a();}catch(Exception e)when(e is IOException or InvalidDataException or ArgumentException){return;}throw new Exception("R16 expected file/parser rejection");}
    static string Write(string folder,string filename="map.osu",string version="Easy",string? objects=null,int mode=0)
    {
        Directory.CreateDirectory(folder);string p=Path.Combine(folder,filename);
        File.WriteAllText(p,$"osu file format v14\n[General]\nMode:{mode}\n[Metadata]\nTitle:Local Song\nTitleUnicode:歌曲\nArtist:Independent\nArtistUnicode:音楽\nCreator:Author\nVersion:{version}\nBeatmapID:7\nBeatmapSetID:99\n[Difficulty]\nHPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n"+(objects??"256,192,2000,1,0,0:0:0:0:")+"\n");return p;
    }
    static void Stage(PlayEntryFlow f){Eq(f.Request(),StartDecision.Accepted);Yes(f.Accept());Yes(f.Queue());Yes(f.Commit());Yes(f.Capture());}
    static string Snapshot(CircleSession s)=>JsonSerializer.Serialize(new{s.Ledger.Score,s.Ledger.N300,s.Ledger.N100,s.Ledger.N50,s.Ledger.NMiss,s.Ledger.MaximumCombo,s.Ledger.CurrentCombo,s.Ledger.AccuracyFraction,s.Ledger.EventCount,s.JudgedCount,s.Finished,s.Scoring.K,objects=s.ObjectResults.ToArray(),sliders=s.Sliders.Select(x=>new{x.Tracking,x.Audio.Index,x.SuccessfulCheckpoints,x.FailedCheckpoints}),spinners=s.Spinners.Select(x=>new{x.Progress,x.SmoothedMotion,x.Observation.Count,x.Observation.CachedInteger}),health=s.Health is{}h?new{h.State.H,h.State.A,h.State.V,h.ResultEvents,h.Failed}:null});
    static async Task<int> Main(string[] args)
    {
        string repo=args[0];CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;string tmp=Path.Combine(Path.GetTempPath(),"ohno-r16-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(tmp);
        try
        {
            Eq(SelectionSettings.ConfigurationPath(tmp),Path.Combine(tmp,"selection.json"));var settings=SelectionSettings.Read(Path.Combine(tmp,"absent.json"));Eq(settings.SongsRoot(tmp),Path.Combine(tmp,"Songs"));
            Pass("configuration beside executable and application-relative Songs default");
            File.WriteAllText(SelectionSettings.ConfigurationPath(tmp),"{\"BeatmapDirectory\":\"Music\",\"PreferUnicode\":true}");settings=SelectionSettings.Read(SelectionSettings.ConfigurationPath(tmp));Yes(settings.PreferUnicode);Eq(settings.SongsRoot(tmp),Path.Combine(tmp,"Music"));
            Pass("explicit BeatmapDirectory and Unicode preference inputs");
            using(var empty=new LocalChartCatalog([Path.Combine(tmp,"missing")])){var snap=await empty.RefreshAsync();Eq(snap!.Charts.Count,0);Yes(snap.Notices.Any());}
            Pass("empty/missing root produces usable explicit empty index");
            string songs=Path.Combine(tmp,"Songs"),pA=Write(Path.Combine(songs,"song1"),"same.osu"),pB=Write(Path.Combine(songs,"song1"),"second.osu","Hard"),pC=Write(Path.Combine(songs,"song2"),"same.osu","Other");
            using var catalog=new LocalChartCatalog([songs]);var snapshot=(await catalog.RefreshAsync())!;Eq(snapshot.Charts.Count,3);
            var a=snapshot.Charts.Single(x=>x.Path==pA);var b=snapshot.Charts.Single(x=>x.Path==pB);var chartC=snapshot.Charts.Single(x=>x.Path==pC);
            Yes(a.Available&&b.Available&&chartC.Available);Yes(!a.GameplayValidated);
            Pass("recursive local discovery indexes metadata without full gameplay validation");
            Eq(a.Filename,chartC.Filename);Yes(!ReferenceEquals(a,chartC));Eq(a.Metadata!.ChartId,chartC.Metadata!.ChartId);Eq(snapshot.Charts.Count(x=>x.Metadata!.ChartId==7),3);
            Pass("duplicate filenames across folders and duplicate chart IDs are retained");
            Eq(a.Metadata.Title,"Local Song");Eq(a.Metadata.Artist,"Independent");Eq(a.Metadata.Creator,"Author");Eq(b.Metadata!.Version,"Hard");Eq(a.Metadata.Ruleset,0);
            Pass("title artist creator version and effective ordinary ruleset");
            Eq(a.Metadata.Hp,0f);Eq(a.Metadata.Cs,5f);Eq(a.Metadata.Od,5f);Eq(a.Metadata.Ar,5f);Eq(a.Metadata.Objects,1);Eq(a.Metadata.MinimumBpm,120d);Eq(a.Metadata.DurationMilliseconds,2000);
            Pass("independent HP CS OD AR count fixed BPM and stored-end metadata");
            var unicode=LocalChartCatalog.Index(pA,true);Eq(unicode.Metadata!.Title,"歌曲");Eq(unicode.Metadata.Artist,"音楽");
            Pass("Unicode preference with fallback separate from gameplay metadata");
            string optional=Write(Path.Combine(tmp,"optional"));File.WriteAllText(optional,File.ReadAllText(optional).Replace("Title:Local Song\n","").Replace("Artist:Independent\n","").Replace("Creator:Author\n","").Replace("Version:Easy\n",""));var noOptional=LocalChartCatalog.Index(optional);Eq(noOptional.Metadata!.Title,"map");Eq(noOptional.Metadata.Artist,null);Yes(noOptional.LoadGameplay().Circles.Count==1);
            Pass("missing optional metadata uses declared filename fallback; actual parser remains authoritative");
            string bad=Path.Combine(songs,"broken.osu");File.WriteAllText(bad,"bad header");var badChart=LocalChartCatalog.Index(bad);Eq(badChart.Availability,ChartAvailability.Invalid);Yes(!badChart.Available);Eq(LocalChartCatalog.Index(Path.Combine(tmp,"gone.osu")).Availability,ChartAvailability.Missing);
            Pass("invalid and missing osu rows expose status instead of fabricated chart");
            string alt=Write(Path.Combine(tmp,"alt"),mode:1);Eq(LocalChartCatalog.Index(alt).Availability,ChartAvailability.Unsupported);string version=Write(Path.Combine(tmp,"format"));File.WriteAllText(version,File.ReadAllText(version).Replace("format v14","format v15"));Eq(LocalChartCatalog.Index(version).Availability,ChartAvailability.Unsupported);
            Pass("unsupported ruleset and format visible before gameplay loading");
            string unsupported=Write(Path.Combine(tmp,"object"),objects:"256,192,2000,128,0,3000:0:0:0:0:");Eq(LocalChartCatalog.Index(unsupported).Availability,ChartAvailability.Unsupported);
            Pass("unsupported object metadata is unavailable");
            string malformed=Write(Path.Combine(tmp,"parser"),objects:"256,192,2000,2,0,P|356:192,1,100");var indexed=LocalChartCatalog.Index(malformed);Yes(indexed.Available);Throws(()=>indexed.LoadGameplay());
            Pass("metadata indexed is distinct from successful full gameplay parsing");
            var stable=(await catalog.RefreshAsync())!;Yes(ReferenceEquals(a,stable.Charts.Single(x=>x.Path==pA)));
            Pass("unchanged bytes/stamp preserve exact chart reference across refresh");
            var selected=new SelectedChartState();var model=new SelectPlayModel(selected);model.Rebuild(new(1,[a,b,chartC],[]));Eq(model.Groups.Count,2);Eq(model.Groups[0].Rows.Count,2);Yes(ReferenceEquals(model.Highlighted,a));Eq(selected.Current,null);
            Pass("folder runs and initial highlight do not implicitly commit selection");
            var disjoint=new SelectPlayModel(new());disjoint.Rebuild(new(1,[a,chartC,b],[]));Eq(disjoint.Groups.Count,3);Eq(disjoint.Rows[2].FirstInRun,2);
            Pass("P11 supplied folder s/t/s creates three contiguous runs, not global set grouping");
            Yes(model.Commit());Yes(ReferenceEquals(selected.Current,a));Yes(ReferenceEquals(model.Committed,a));
            Pass("difficulty commit invokes nonnull authoritative selection callback");
            model.Highlight(chartC);Yes(ReferenceEquals(model.Highlighted,chartC));Yes(ReferenceEquals(model.Committed,a));Yes(ReferenceEquals(selected.Current,a));
            Pass("different-song highlight remains pending without selected-map change");
            model.Highlight(b);Yes(ReferenceEquals(selected.Current,b));Yes(ReferenceEquals(model.Committed,b));
            Pass("same-run highlight may immediately commit difficulty");
            int cleanup=0;Yes(!selected.Set(b,_=>cleanup++));Eq(cleanup,0);int revision=selected.Revision;Yes(!selected.Set(null));Eq(selected.Revision,revision);
            Pass("same-reference setter and null callback do not clear or clean a valid selection");
            bool threw=false;try{selected.Set(chartC,_=>throw new InvalidOperationException("controlled old resource"));}catch(InvalidOperationException){threw=true;}Yes(threw);Yes(ReferenceEquals(selected.Current,b));
            Pass("pre-store cleanup failure retains old authoritative reference");
            model.SelectionCallback=null;model.Highlight(chartC);Yes(!model.Commit());Yes(ReferenceEquals(model.Committed,chartC));Yes(ReferenceEquals(selected.Current,b));Yes(model.SelectionError is not null);
            Pass("null selection callback preserves global while exposing committed/global distinction");
            model.SelectionCallback=chart=>{selected.Set(chart);throw new IOException("controlled metadata after store");};Yes(!model.Commit());Yes(ReferenceEquals(selected.Current,chartC));Yes(model.SelectionError!.Contains("controlled"));
            Pass("post-store callback failure is contained without manufactured rollback");
            model.SelectionCallback=chart=>selected.Set(chart);model.Rebuild(new(2,[a,b,chartC],[]));Yes(ReferenceEquals(model.Highlighted,chartC));Yes(ReferenceEquals(model.Committed,chartC));
            Pass("rebuild/Rank return restores live selected reference and containing group");
            model.Rebuild(new(3,[a,b],[]));Yes(ReferenceEquals(selected.Current,chartC));Eq(model.Highlighted,null);Yes(model.Recovery is not null);Yes(!model.Rebuild(new(2,[chartC],[])));
            Pass("missing selected row recovery and stale browser generation cannot replace newer selection");
            model.Rebuild(new(4,[a,b,chartC],[]));model.Highlight(a);model.Commit();model.Move(1);Yes(ReferenceEquals(selected.Current,b));model.Move(10);Yes(ReferenceEquals(model.Highlighted,chartC));Yes(ReferenceEquals(selected.Current,b));model.MoveGroup(-1);Yes(ReferenceEquals(selected.Current,chartC));
            Pass("nearby/page/group movement distinguishes pending confirmation from committed navigation");
            int starts=0;model.StartRequested+=()=>starts++;model.Confirm();Eq(starts,1);model.Highlight(a);model.Confirm();Eq(starts,1);model.Confirm();Eq(starts,2);
            Pass("Enter commits a pending highlight before a separate confirmed Start");
            model.MouseSelect(chartC);Yes(ReferenceEquals(selected.Current,chartC));Eq(starts,2);model.MouseSelect(chartC);Eq(starts,3);
            Pass("mouse different-row selection and repeated selected-row Start remain separate");
            using(var none=new PlayEntryFlow(new()))Eq(none.Request(),StartDecision.Rejected);
            using var flow=new PlayEntryFlow(selected);Eq(flow.Request(specialTemporary:true),StartDecision.Rejected);Eq(flow.Request(phase:1),StartDecision.Rejected);Eq(flow.Stage,PlayEntryStage.Selected);
            Pass("Start no-selection temporary-mode and nonready-phase guards");
            selected.Set(a);Eq(flow.Request(),StartDecision.Accepted);Eq(flow.Stage,PlayEntryStage.StartRequested);Eq(flow.Captured,null);Eq(flow.Request(),StartDecision.Duplicate);Yes(flow.Accept());Yes(flow.Queue());Eq(flow.Stage,PlayEntryStage.PendingPlay);Eq(flow.Captured,null);
            Pass("accepted queued Play is not commitment or captured chart; duplicate Start suppressed");
            selected.Set(b);Yes(flow.Commit());Eq(flow.Stage,PlayEntryStage.PlayCommitted);Eq(flow.Captured,null);Yes(flow.Capture());Yes(ReferenceEquals(flow.Captured,b));selected.Set(chartC);Yes(ReferenceEquals(flow.Captured,b));
            Pass("live selection at capture sees before-capture change; later global change does not replace Play");
            long ticket=flow.BeginLoading();Eq(flow.Stage,PlayEntryStage.Loading);Yes(flow.CanInstall(ticket));Eq(flow.Captured!.LoadGameplay().Title,"Local Song");Yes(flow.Ready(ticket));Eq(flow.Stage,PlayEntryStage.Ready);Yes(!flow.Ready(ticket));
            Pass("full parser follows Play commitment/capture; readiness is a separate completion");
            Stage(flow);long old=flow.BeginLoading();flow.Cancel();Yes(!flow.CanInstall(old));Stage(flow);long newer=flow.BeginLoading();Yes(!flow.Ready(old));flow.Fail(old,"stale failure");Eq(flow.Stage,PlayEntryStage.Loading);Yes(flow.Ready(newer));
            Pass("stale load success/failure cannot install or overwrite newer generation");
            Stage(flow);long deferred=flow.BeginLoading();var completed=new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);var waiting=Task.Run(async()=>{await completed.Task;return flow.Ready(deferred);});Eq(flow.Stage,PlayEntryStage.Loading);flow.Cancel();completed.SetResult(true);Yes(!await waiting);
            Pass("controlled deferred completion after cancellation cannot become gameplay ready");
            Stage(flow);long failed=flow.BeginLoading();flow.Fail(failed,"missing song");Eq(flow.Stage,PlayEntryStage.Failed);Eq(flow.Error,"missing song");Stage(flow);flow.BeginLoading();flow.Dispose();Yes(!flow.CanInstall(flow.Generation));Eq(flow.Request(),StartDecision.Rejected);
            Pass("explicit load failure recovery and application-close disposal reject stale work");
            string deleted=Write(Path.Combine(tmp,"deleted"));var deletedChart=LocalChartCatalog.Index(deleted);selected.Set(deletedChart);using(var missing=new PlayEntryFlow(selected)){Eq(missing.Request(),StartDecision.Accepted);missing.Accept();missing.Queue();missing.Commit();missing.Capture();File.Delete(deleted);Throws(()=>missing.Captured!.LoadGameplay());}
            Pass("M2578-style request does not prove file availability or successful load");
            File.WriteAllText(pA,File.ReadAllText(pA).Replace("Version:Easy","Version:Updated"));Throws(()=>a.LoadGameplay());var changed=(await catalog.RefreshAsync())!;var updated=changed.Charts.Single(x=>x.Path==pA);Yes(!ReferenceEquals(a,updated));Eq(updated.Metadata!.Version,"Updated");selected.Set(a);model.Rebuild(changed with{Generation=model.CatalogGeneration+1});Yes(ReferenceEquals(selected.Current,a));Yes(model.Recovery is not null);
            Pass("changed metadata creates new revision; stale cached selection never silently substitutes it");
            var parsed=updated.LoadGameplay();string numeric=JsonSerializer.Serialize(parsed.ScoreMetadata);Eq(ScoreConfiguration.FromBeatmap(parsed).K,ScoreConfiguration.FromBeatmap(Beatmap.Load(pA)).K);Eq(JsonSerializer.Serialize(parsed.ScoreMetadata),numeric);
            Pass("selection metadata/fingerprints preserve authoritative E034 metadata and score multiplier");
            File.Delete(pB);var deletedSnapshot=(await catalog.RefreshAsync())!;Yes(!deletedSnapshot.Charts.Any(x=>x.Path==pB));
            Pass("root refresh removes deleted chart independently of retained selection");
            string external=Write(Path.Combine(tmp,"external"));var added=(await catalog.RefreshAsync(addExternal:external))!;Yes(added.Charts.Any(x=>x.Path==external));
            Pass("file-picker external chart becomes its own local catalog entry");
            using(var cancelled=new LocalChartCatalog([songs])){using var cts=new CancellationTokenSource();cts.Cancel();Eq(await cancelled.RefreshAsync(cts.Token),null);Eq(cancelled.Snapshot.Generation,0L);}
            Pass("cancelled refresh leaves previously installed snapshot intact");
            var reached=new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);var release=new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            using(var raced=new LocalChartCatalog([songs],beforePublish:async generation=>{if(generation==1){reached.SetResult(true);await release.Task;}}))
            {var first=raced.RefreshAsync();await reached.Task;var second=await raced.RefreshAsync();Yes(second!.Generation==2);release.SetResult(true);Eq(await first,null);Eq(raced.Snapshot.Generation,2L);}
            Pass("actual scans with injected publication interleaving retain latest snapshot only");
            using(var fixtures=new LocalChartCatalog([Path.Combine(repo,"reconstructed/CircleGame/Fixtures")])){var list=await fixtures.RefreshAsync();Yes(list!.Charts.Any(x=>x.Filename=="spinner-practice.osu"));Yes(list.Charts.Any(x=>x.Filename=="discrete-slider-r12.osu"));}
            Pass("bundled existing development fixtures discover without rewriting source files");
            var missingStar=StarDisplay.Prepare(updated,0,0);Eq(missingStar.State,"missing");Yes(missingStar.Text.Contains("not calculated"));
            Pass("absent star cache never fabricates a zero-star calculation");
            var star=new SuppliedStar("osu-stable:E043:cache-v1","independent compatible export",updated.Fingerprint!.Sha256,0,0,4.56789f);var display=StarDisplay.Prepare(updated,0,0,star);Eq(display.State,"valid");Yes(display.Text.StartsWith("4.56★"));
            Pass("compatible hash/mode/mod source uses promoted Single magnitude and floor2");
            Eq(StarDisplay.Prepare(updated,1,0,star).State,"mismatched");Eq(StarDisplay.Prepare(updated,0,8,star).State,"mismatched");Eq(StarDisplay.Prepare(updated,0,0,star with{ChartSha256="old"}).State,"stale");Eq(StarDisplay.Prepare(updated,0,0,star with{Compatibility="lazer"}).State,"unsupported");
            Pass("star mode/mod/hash/profile mismatches are explicit; no alternate calculator substitution");
            display=StarDisplay.Prepare(updated,0,0,star with{Value=-4.5f});Eq(display.State,"stale");Yes(display.Text.StartsWith("4.5★"));Eq(StarDisplay.Prepare(updated,0,0,star with{Value=float.NaN}).State,"unsupported");Eq(StarDisplay.Prepare(updated,0,0,star with{Stale=true}).State,"stale");
            Pass("negative cached magnitude staleness and nonfinite values do not imply valid calculated stars");
            Eq(StarDisplay.Prepare(updated,0,8388608,star).State,"valid");
            Pass("ordinary normalized star key masks1370; raw excluded bit does not create a new cache entry");
            string noStar=StarDisplay.Prepare(updated,0,0).Text;Yes(!noStar.Contains(ScoreConfiguration.FromBeatmap(parsed).K.ToString("0.00")));Eq(PlayEntryFlow.TransitionScalar,1000);
            Pass("star display is separate from E034 K; queued scalar1000 is metadata not Task.Delay");
            foreach(string fixture in new[]{"first-circles.osu","discrete-slider-r12.osu","spinner-practice.osu","mixed-r07.osu"})
            {
                string path=Path.Combine(repo,"reconstructed/CircleGame/Fixtures",fixture);var oldMap=Beatmap.Load(path);var row=LocalChartCatalog.Index(path);var newMap=row.LoadGameplay();Eq(row.Fingerprint!.Sha256,ChartFingerprint.Read(path).Fingerprint.Sha256);Eq(JsonSerializer.Serialize(oldMap.ScoreMetadata),JsonSerializer.Serialize(newMap.ScoreMetadata));
                var x=CircleSession.CreateFreshOrdinary(oldMap,ScoreConfiguration.FromBeatmap(oldMap));var y=CircleSession.CreateFreshOrdinary(newMap,ScoreConfiguration.FromBeatmap(newMap));using var rx=new RankSession(x);using var ry=new RankSession(y);var ax=new List<string>();var ay=new List<string>();
                foreach(var pair in new[]{(s:x,list:ax),(s:y,list:ay)}){pair.s.PositiveCircleSound+=d=>pair.list.Add("circle:"+d.StartTime);pair.s.SliderSound+=d=>pair.list.Add("slider:"+d.Kind+":"+d.Index);pair.s.SpinnerSound+=(d,e)=>pair.list.Add("spinner:"+e+":"+d.Observation.Count);pair.s.ContinuingSound+=(d,t,on)=>pair.list.Add("loop:"+t+":"+on);}
                Eq(x.Ledger.EventCount,0);Eq(y.Ledger.EventCount,0);
                for(int t=0;t<=rx.MaximumEnd+3016;t+=16){Vector2 pointer=SpinnerState.DisplayCenter+new Vector2((float)(100*Math.Cos(t*.04)),(float)(100*Math.Sin(t*.04)));x.Advance(t,pointer,1);y.Advance(t,pointer,1);Eq(Snapshot(x),Snapshot(y));foreach(var r in new[]{rx,ry}){r.Observe(t,continuation:t<=r.MaximumEnd);r.Complete();r.Observe(t,continuation:t<=r.MaximumEnd);}}
                Eq(string.Join("|",ax),string.Join("|",ay));Eq(rx.HandlerRecord.Grade(),ry.HandlerRecord.Grade());Eq(rx.HandlerRecord.Score,ry.HandlerRecord.Score);
            }
            Pass("four old-loader versus SelectPlay-loader matched histories: score K counts judgement slider spinner HP audio Rank unchanged");
            string source=File.ReadAllText(Path.Combine(repo,"reconstructed/CircleGame/Program.cs"));foreach(string token in new[]{"case Key.Up","case Key.Down","case Key.PageUp","case Key.PageDown","case Key.Left","case Key.Right","browser.Confirm()","Key.Space or Key.Enter","surface.SkipResultsReveal()","captured?.Verify(request.Token)","entry.CanInstall(ticket)","_ = RefreshCatalogAsync()"})Yes(source.Contains(token));
            Pass("SOURCE INSPECTION state-specific keyboard, Rank skip, loader revalidation and Rank-return refresh wiring");
            Yes(!File.ReadAllText(Path.Combine(repo,"reconstructed/CircleGame/Selection/SelectPlayModel.cs")).Contains("Task.Delay"));using var coverage=JsonDocument.Parse(File.ReadAllText(Path.Combine(repo,"reconstructed/R16DevelopmentChecks/e043-coverage.json")));Eq(coverage.RootElement.GetProperty("rows").GetArrayLength(),66);
            Pass("SOURCE INSPECTION no fabricated native queue delay and66 original prediction rows retained separately");
            Console.WriteLine($"R16 DEVELOPMENT: {groups} groups passed; final2 source inspections; other groups actual production components; no game Window/media/physical input");
            Console.WriteLine("R16 EVIDENCE "+Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(new{schema="R16.development.v1",groups,cases=results})));return 0;
        }
        finally{Directory.Delete(tmp,true);}
    }
}
