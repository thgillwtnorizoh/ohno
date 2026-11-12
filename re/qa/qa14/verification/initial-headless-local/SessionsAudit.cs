using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
namespace Ohno.QA14;
public static partial class Tests
{
    static string Snapshot(CircleSession s)=>JsonSerializer.Serialize(new{s.Scoring,s.Ledger.Score,s.Ledger.EventCount,s.Ledger.LastEvent,s.Ledger.LastOutcome,s.Ledger.N300,s.Ledger.N100,s.Ledger.N50,s.Ledger.NMiss,s.Ledger.CurrentCombo,s.Ledger.MaximumCombo,s.Ledger.ComboSnapshot,s.Accuracy,s.JudgedCount,s.Finished,s.HealthPolicy,
        circles=s.Circles.Select(c=>new{c.Result,c.JudgedAt}),sliders=s.Sliders.Select(x=>new{x.Head.Result,parent=x.Result,x.Tracking,x.FollowEligible,x.EntryTime,x.ProcessedCheckpoints,x.SuccessfulCheckpoints,x.FailedCheckpoints,x.TerminalSucceeded,x.CompletionStarted,audio=x.Audio.Index,checkpoints=x.Checkpoints.Select(c=>new{c.RawResult,c.Succeeded,c.JudgedAt})}),
        spinners=s.Spinners.Select(x=>new{x.Progress,x.Observation.CachedInteger,x.Observation.Count,x.Required,x.TargetMotion,x.SmoothedMotion,x.LastComponentRaw,x.FinalRaw,x.Result,x.CompletionStarted}),
        health=s.Health is{}h?new{h.State,h.Calibration,h.ResultEvents,h.LastPassiveLoss,h.Failed,h.Recovering,h.RecoveryCount,grouping=h.Chart.Objects.Select(o=>o.NewCombo)}:null});
    static string Shape(Beatmap m)=>JsonSerializer.Serialize(new{m.Title,m.CircleSize,m.OverallDifficulty,m.ApproachRate,m.ScoreMetadata,circles=m.Circles.Select(c=>new{x=c.Position.X,y=c.Position.Y,c.StartTime}),sliders=m.Sliders.Select(s=>new{x=s.Head.Position.X,y=s.Head.Position.Y,s.Head.StartTime,s.EndTime,s.Traversals,s.PixelLength,s.TravelRate,
        moves=s.Movements.Select(p=>new{ax=p.From.X,ay=p.From.Y,bx=p.To.X,by=p.To.Y,p.StartTime,p.EndTime}),checkpoints=s.Checkpoints.Select(c=>new{c.Time,c.Kind,c.Traversal,x=c.Position.X,y=c.Position.Y}),s.Geometry.CumulativeLengths}),m.Spinners});
    sealed class MarkerFiles:ISampleFiles,IDefaultSamples,ISampleLoader
    {
        readonly Dictionary<string,int> handles=new(StringComparer.Ordinal);
        public bool Exists(string name)=>true;
        public byte[]? Read(string name)=>new byte[1024];
        public byte[]? Ui(string name)=>new byte[1024];
        public byte[]? Gameplay(string name)=>null;
        public int Load(SampleBytes data){if(!handles.TryGetValue(data.Name,out int h))handles[data.Name]=h=handles.Count+100;return h;}
        public void Free(int h){}
    }
    sealed class Channels:ISpinnerSampleChannels
    {
        public readonly List<string> Ops=[];
        public FrequencyCapability FrequencyCapability=>FrequencyCapability.Supported;
        public int Acquire(int sample,bool fresh){Ops.Add($"acquire:{sample}:{fresh}");return sample+1000;}
        public int Activity(int channel){Ops.Add("active:"+channel);return 0;}
        public long? Length(int channel,int mode){Ops.Add($"length:{channel}:{mode}");return 10000;}
        public void Attribute(int c,int a,float value)=>Ops.Add($"attribute:{c}:{a}:{value:R}");
        public void Play(int c,bool restart)=>Ops.Add($"play:{c}:{restart}");
        public void Pause(int c)=>Ops.Add("pause:"+c);
        public void Stop(int c)=>Ops.Add("stop:"+c);
        public void Dispose()=>Ops.Add("dispose");
    }
    sealed class SoundProbe:ISamplePlaybackSink
    {
        public readonly List<string> Events=[],Requests=[];
        public readonly Channels Backend=new();
        readonly CircleSampleResources resources;
        readonly CircleSoundPolicy policy=new(false,false,true,0,100,true,80,100);
        readonly SharedSliderChannels shared;
        readonly SharedSpinnerChannel spinner;
        public SoundProbe(CircleSession session)
        {
            var map=session.Map;var files=new MarkerFiles();var info=map.CircleSounds!;
            resources=new(files,null,files,files,new(14,false,false,true,false,"",false,CultureInfo.InvariantCulture));
            resources.Prepare(info.Timing.Select(p=>p.Index),[],true);resources.PrepareContinuing(info.Timing.Select(p=>p.Index));
            shared=new(new GlobalSampleTiming(info.Timing),Backend,(row,bank,index)=>resources.ResolveContinuing(row,bank,index));
            spinner=shared.AttachSpinner(Backend,()=>resources.SpinnerResource(true));
            session.PositiveCircleSound+=c=>{Events.Add("circle:"+c.StartTime);CircleSampleSelection.Request(info.Circles.Single(s=>ReferenceEquals(s.Object,c)),info.Timing,-1,resources,policy,this);};
            session.SliderSound+=e=>{Events.Add($"slider:{e.Kind}:{e.Index}:{e.ScheduleIndex}:{e.Time}");SliderSampleSelection.Request(e,PreparedSliderSamples.Create(e.Parent,info.Timing,policy,-1),info.Timing,resources,policy,this);};
            session.GlobalSoundTiming+=t=>{Events.Add("global:"+t);shared.Clock(t);};
            session.ContinuingSound+=(s,t,active)=>{Events.Add($"continuing:{s.Definition.Head.StartTime}:{t}:{active}:{s.Audio.Index}");if(active)shared.Submit(s.Definition.Samples!.RawSound,s.Definition.Samples.BaseSet,s.Definition.Samples.AdditionSet,false,s.PresentationBallX);else shared.QueuePause();shared.Drain();};
            session.ContinuingManagerTail+=s=>{Events.Add("tail:"+s);shared.ManagerTail(s);shared.Drain();};
            session.SpinnerSound+=(s,stage)=>
            {
                Events.Add($"spinner:{stage}:{s.Observation.Count}:{s.Required}");
                if(stage==SpinnerSoundStage.ObservedChange)spinner.Observe(s.Observation.Count,s.Required,true);
                else if(stage==SpinnerSoundStage.Motion)spinner.Continue(null);
                else if(stage is SpinnerSoundStage.ZeroMotion or SpinnerSoundStage.Cleanup)spinner.QueuePause();
                else if(stage==SpinnerSoundStage.Bonus&&SpinnerSampleSelection.BonusAllowed(s.Observation.Count,s.Required,0))SpinnerSampleSelection.Bonus(s.Definition,resources.SpinnerResource(false),55,policy,this);
                else if(stage==SpinnerSoundStage.PositiveCompletion)SpinnerSampleSelection.Completion(new(info.Spinners.Single(m=>ReferenceEquals(m.Object,s.Definition)),-1),info.Timing,SpinnerState.DisplayCenter,resources,policy,this);
                shared.Drain();
            };
        }
        public void Enqueue(SamplePlaybackRequest r)=>Requests.Add($"{r.Kind}:{r.Source.StartTime}:{r.Handle}:{r.Layer}:{r.Volume}:{r.Pan:R}:{r.Factor:R}:{r.Gain:R}:{r.Event?.Index}:{r.Event?.ScheduleIndex}");
    }
    static void Sessions()
    {
        const string A="E043 P36/P37/P42/P50; E033/E034/E035/E036/E042 retained kernels; matched production parser/session/selection/audio components";
        var variants=new Dictionary<string,string>{
            ["circles"]="64,192,1000,1,2,0:0:0:0:\n448,192,2600,1,4,0:0:0:0:",
            ["straight"]="64,192,1000,2,14,L|264:192,1,200,2|8,1:2|3:1,0:0:0:0:\n448,192,6000,1,0,0:0:0:0:",
            ["repeat"]="64,192,1000,2,14,L|264:192,2,200,2|8|4,1:2|3:1|2:3,0:0:0:0:\n448,192,6000,1,0,0:0:0:0:",
            ["bezier"]="64,192,1000,2,2,B|164:64|264:192,1,200,,,0:0:0:0:\n448,192,6000,1,0,0:0:0:0:",
            ["joined-bezier"]="64,192,1000,2,2,B|164:64|264:192|264:192|364:64|464:192,1,400,,,0:0:0:0:\n448,192,6000,1,0,0:0:0:0:",
            ["spinner"]="256,192,1000,8,2,5000,0:0:0:0:\n448,192,6000,1,0,0:0:0:0:"};
        foreach(var (name,rows) in variants)foreach(bool reduced in new[]{false,true})Add($"P/matched-{name}-reduced{reduced}",A,()=>
        {
            string path=Put(text:Text(rows:rows));var compatibility=Beatmap.Load(path);var chart=LocalChartCatalog.Index(path);var selection=new SelectedChartState();selection.Set(chart);using var entry=Loading(selection);var browserMap=entry.Captured!.LoadGameplay();
            Eq(Shape(browserMap),Shape(compatibility),"parsed geometry/timing/score metadata identical");
            var x=CircleSession.CreateFreshOrdinary(compatibility,ScoreConfiguration.FromBeatmap(compatibility));var y=CircleSession.CreateFreshOrdinary(browserMap,ScoreConfiguration.FromBeatmap(browserMap));using var rankX=new RankSession(x);using var rankY=new RankSession(y);var ax=new SoundProbe(x);var ay=new SoundProbe(y);
            int comparisons=0,lastCount=-1;var trace=new List<object>();
            void Compare(string action,int time)
            {
                string sx=Snapshot(x),sy=Snapshot(y);Eq(Hash(Encoding.UTF8.GetBytes(sy)),Hash(Encoding.UTF8.GetBytes(sx)),action+" at"+time);Seq(ay.Events,ax.Events,"gameplay audio hooks");Seq(ay.Requests,ax.Requests,"actual production sample request operands");Seq(ay.Backend.Ops,ax.Backend.Ops,"actual shared-channel operations with injected native operands");
                if(lastCount!=x.Ledger.EventCount||action=="press"){trace.Add(new{action,time,expected=JsonSerializer.Deserialize<JsonElement>(sx),actualHash=Hash(Encoding.UTF8.GetBytes(sy)),sampleRequests=ay.Requests.Count,channelOperations=ay.Backend.Ops.Count});lastCount=x.Ledger.EventCount;}comparisons++;
            }
            Eq(x.Ledger.EventCount,0);Eq(y.Ledger.EventCount,0);Eq(ax.Events.Count,0);Eq(ay.Events.Count,0);Eq(ax.Requests.Count,0);Compare("private-calibration-silent",0);int end=rankX.MaximumEnd;
            for(int t=0;t<=end+3200;t+=20)
            {
                foreach(var head in x.Heads.Where(h=>h.Definition.StartTime+(reduced?100:0)==t)){if(reduced&&ReferenceEquals(head,x.Heads.Last()))continue;x.Press(head.Definition.Position,t);y.Press(head.Definition.Position,t);Compare("press",t);}
                Vector2 pointer=new(448,192);var sliding=x.Sliders.FirstOrDefault(s=>t>=s.Definition.Head.StartTime&&t<=s.Definition.EndTime);
                if(sliding is not null)pointer=sliding.Definition.RequiredPosition(t);
                if(x.Spinners.Any(s=>t>=s.Definition.StartTime&&t<=s.Definition.EndTime)){double angle=t*.047;pointer=SpinnerState.DisplayCenter+new Vector2((float)(110*Math.Cos(angle)),(float)(110*Math.Sin(angle)));}
                int held=reduced&&t%240>=160?0:1;x.Advance(t,pointer,held);y.Advance(t,pointer,held);rankX.Observe(t,continuation:t<=end);rankY.Observe(t,continuation:t<=end);rankX.Complete();rankY.Complete();rankX.Observe(t,continuation:t<=end);rankY.Observe(t,continuation:t<=end);Compare("advance",t);
            }
            Eq(rankY.Stage,rankX.Stage);Eq(rankY.HandlerRecord.Grade(),rankX.HandlerRecord.Grade());string frozen=Snapshot(y);
            if(rankX.Stage==RankStage.RankAdmitted){rankX.Commit(()=>{});rankY.Commit(()=>{});rankX.Initialize(0);rankY.Initialize(0);rankX.Present();rankY.Present();Eq(rankY.Presentation!.Score,rankX.Presentation!.Score);Eq(rankY.Presentation.Accuracy,rankX.Presentation.Accuracy);rankY.Skip(1);Eq(Snapshot(y),frozen);}
            Same(entry.Captured,chart);observations.Add(new{comparisons,trace,requests=ay.Requests,channelOperations=ay.Backend.Ops,classification="Injected resources/channels; no media/native audio execution. LastEvent is observed at every supplied action, not an arbitrary internal event subscription."});
        });
        foreach(string mutation in new[]{"unchanged","deleted","changed","unavailable"}) Async("P/rank-return-"+mutation,A,async()=>{string p=Put();using var catalog=new LocalChartCatalog([work]);var model=new SelectPlayModel(new());model.Rebuild((await catalog.RefreshAsync())!);model.Commit();var selected=model.Selection.Current!;var map=selected.LoadGameplay();var s=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));s.Press(new(173,141),1700);s.Advance(4700);using var rank=new RankSession(s);rank.Observe(1901);Yes(rank.Complete(),"whole record completed");rank.Observe(4700);Yes(rank.Commit(()=>{}),"Rank commit");Yes(rank.Initialize(0),"init");rank.Present();Eq(rank.Back(),TransitionDecision.Accepted);rank.Commit(()=>{});Eq(rank.Stage,RankStage.ChartSelection);
            if(mutation=="deleted")File.Delete(p);if(mutation=="changed")File.AppendAllText(p,"\n// after Rank");if(mutation=="unavailable")selected.ReportGameplay("unavailable now");model.Rebuild((await catalog.RefreshAsync())!);Same(model.Selection.Current,selected);if(mutation=="unchanged"){Same(model.Highlighted,selected);Eq(model.Recovery,null);}else{Eq(model.Highlighted,null);Eq(model.Committed,null);Yes(model.Recovery is not null,"no arbitrary first-row recovery");}});
        Add("P/retry-captured-not-browser",A,()=>{var oldChart=Chart("s/played.osu");var nextChart=Chart("t/highlight.osu");var selection=new SelectedChartState();selection.Set(oldChart);using var entry=Loading(selection);var map=entry.Captured!.LoadGameplay();var old=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));old.Press(new(173,141),1700);old.SetHeldButtons(3);selection.Set(nextChart);var retry=CircleSession.CreateFreshOrdinary(map,old.Scoring);Same(entry.Captured,oldChart);Same(retry.Map,map);Different(retry.Ledger,old.Ledger);Different(retry.Health,old.Health);Eq(retry.Ledger.Score,0);Eq(retry.Ledger.CurrentCombo,0);Eq(retry.Ledger.MaximumCombo,0);Eq(retry.Ledger.EventCount,0);Eq(retry.Circles[0].Result,Judgement.Pending);Eq(retry.HealthPolicy,HealthPreparationPolicy.E036FreshOrdinary);Same(selection.Current,nextChart);});
        Add("P/health-recovery-and-grouping-preserved",A,()=>{var a=Chart();var map=a.LoadGameplay();var s=CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map));s.Health!.State.Assign(160);s.Health.State.ConfigureFill(3,.03,false);s.Health.SupplyRecoveryCount(1);s.Health.BeginRecovery();string before=Snapshot(s);var b=Browser(a);b.Commit();_ = a.Metadata!.Details;_ = StarDisplay.Prepare(a,0,0);Eq(Snapshot(s),before);Eq(s.Health.State.Fill,false);Eq(s.Health.State.H,160d);});
    }
    static void Audits()
    {
        void Audit(string id,string authority,Action body)=>Add(id,authority,body,"SOURCE INSPECTION");
        Audit("A/queued-host-capture","E043 M46ae/M2237; R16 dispatcher engineering",()=>{string s=Read("reconstructed/CircleGame/Program.cs");int start=s.IndexOf("private void StartSelected()");int end=s.IndexOf("private void SaveAudioDiagnostics()");string block=s[start..end];foreach(string t in new[]{"entry.Request(browser.SpecialTemporarySelection)","entry.Accept(); entry.Queue()","queuedGeneration != entry.Generation","!entry.Commit() || !entry.Capture()","LoadMapAsync(entry.Captured!.Path, entry.Captured, ticket)"})Yes(block.Contains(t),t);Yes(!block.Contains("Task.Delay"),"no fabricated1000ms scheduler");});
        Audit("A/validation-install-order","R16 fingerprint/loading ENGINEERING contract",()=>{string s=Read("reconstructed/CircleGame/Program.cs");int start=s.IndexOf("private async Task LoadMapAsync");int end=s.IndexOf("private void KeyPressed");string block=s[start..end];int verify=block.IndexOf("captured?.Verify(request.Token)"),parse=block.IndexOf("Beatmap.Load(path)"),audio=block.IndexOf("await candidateAudio.PrepareAsync"),effects=block.IndexOf("await WindowsCircleEffects.PrepareAsync"),last=block.LastIndexOf("captured?.Verify(request.Token)"),guard=block.IndexOf("!entry.CanInstall(ticket)"),install=block.IndexOf("surface.Load(map");Yes(verify<parse&&parse<audio&&audio<effects&&effects<last&&last<guard&&guard<install,"ordered checks and awaited preparation precede replacement");Yes(block.Contains("!ReferenceEquals(loadCancellation, request)"),"obsolete request guard");Yes(block.Contains("closing || request.IsCancellationRequested"),"shutdown/cancellation guard");});
        Audit("A/failure-retention","R16 prior-session and optional-effects ENGINEERING",()=>{string s=Read("reconstructed/CircleGame/Program.cs");string b=s[s.IndexOf("private async Task LoadMapAsync")..s.IndexOf("private void KeyPressed")];Yes(b.Contains("Resource preparation failed:"),"optional effects containment");Yes(b.Contains("captured.ReportGameplay(error.Message); entry.Fail(ticket"),"failed revision and flow status");Yes(b.Contains("candidateAudio?.Dispose()"),"candidate song cleanup");Yes(b.Contains("candidateEffects?.Dispose()"),"candidate effects cleanup");Yes(!b.Contains("surface.Dispose()"),"failure does not dispose previous valid surface");});
        Audit("A/browser-rank-key-isolation","E043 commands/E042 reveal; ENGINEERING key routing",()=>{string s=Read("reconstructed/CircleGame/Program.cs");int selection=s.IndexOf("if (selectionVisible)\n"),rank=s.IndexOf("if (surface.HasRankControls)");Yes(selection>=0&&selection<rank,"browser keys before gameplay");foreach(string t in new[]{"case Key.Up: browser.Move(-1)","case Key.Down: browser.Move(1)","case Key.PageUp: browser.Move(-10)","case Key.PageDown: browser.Move(10)","case Key.Left: browser.MoveGroup(-1)","case Key.Right: browser.MoveGroup(1)","case Key.Enter: if (!repeated) browser.Confirm()","Key.Space or Key.Enter) surface.SkipResultsReveal()","Key.Escape) surface.BackFromResults()","Key.R) surface.RetryResults()"})Yes(s.Contains(t),t);});
        Audit("A/refresh-back-close-cancellation","R16 generation/shutdown ENGINEERING",()=>{string s=Read("reconstructed/CircleGame/Program.cs");Yes(s.Contains("entry.Cancel(); loadCancellation?.Cancel()"),"refresh/back cancels entry/load");Yes(s.Contains("entry.Dispose(); catalog.Dispose(); browserView.Dispose()"),"close disposes model/resources");Yes(s.Contains("heldKeys.Clear(); PublishHeldKeys(); selectionVisible = true"),"selection entry clears input");Yes(s.Contains("browser.Restore(); _ = RefreshCatalogAsync()"),"Rank return restore then refresh");});
        Audit("A/silent-no-calculator","E043 bounded producer boundary; R16 no preview/star engine",()=>{string s=Read("reconstructed/CircleGame/Selection/LocalChartCatalog.cs"),v=Read("reconstructed/CircleGame/Rendering/SelectPlayView.cs");foreach(string t in new[]{"Stars not calculated","osu-stable:E043:cache-v1","mods & 1370"})Yes(s.Contains(t),t);foreach(string t in new[]{"MediaPlayer","M08d9","Task.Delay","BASS_"})Yes(!s.Contains(t)&&!v.Contains(t),"not introduced "+t);});
        Audit("A/windows-view-wiring","R16 unshown view events ENGINEERING; no physical input",()=>{string s=Read("reconstructed/CircleGame/Rendering/SelectPlayView.cs");foreach(string t in new[]{"songs.SelectionChanged +=","difficulties.SelectionChanged +=","difficulties.PreviewMouseLeftButtonDown +=","e.Handled = true; SelectRow(chart)","model.Changed += ModelChanged","model.Changed -= ModelChanged"})Yes(s.Contains(t),t);});
        Audit("A/production-history","R16 chronology: no invented failed development gate",()=>{string s=Read("reconstructed/R16-original-derived-selectplay.md");foreach(string t in new[]{"19bb644e01c646ca8850f0751ec03f7ef881d766","43153555528","52/52","12/12","No initial failing gate"})Yes(s.Contains(t),t);});
    }
}
