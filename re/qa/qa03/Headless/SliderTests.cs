// Direct calls on linked R05 components only. No window, OS input or game entry point.
using System.Numerics;
using Ohno.CircleGame.Gameplay;

namespace Ohno.QA03;

public static class SliderTests
{
    private static readonly List<(string Name, Action Body)> Cases = [];
    private static string root = "";
    private static string scratch = "";
    private static Beatmap Load(string name) => Beatmap.Load(Path.Combine(root,
        "reconstructed/CircleGame/Fixtures", name + ".osu"));
    // Isolate a real parsed definition; exclude unrelated earlier-circle admission.
    private static CircleSession One(string name = "slider-repeat", int index = 0)
    {
        var map = Load(name);
        return new(map with { Circles = Array.Empty<CircleDefinition>(), Sliders = new[] { map.Sliders[index] } });
    }
    private static SliderState S(CircleSession s) => s.Sliders.Single();
    private static void At(CircleSession s, int time, int mask = 1) =>
        s.Advance(time, S(s).Definition.RequiredPosition(time), mask);
    private static CircleSession Started(string name = "slider-repeat")
    {
        var s = One(name); var d = S(s).Definition;
        s.Press(d.Head.Position, d.Head.StartTime);
        At(s, d.Head.StartTime);
        return s;
    }
    private static CircleSession Origin()
    {
        // Separately parsed, translated finite fixture. Origin avoids addition rounding
        // when testing exact squared-radius equality. No production state is forced.
        string text = File.ReadAllText(Path.Combine(root, "reconstructed/CircleGame/Fixtures/slider-ticks.osu"))
            .Replace("106,192,3000", "0,0,3000").Replace("406:192", "300:0");
        string path = Path.Combine(scratch, "origin.osu");
        File.WriteAllText(path, text);
        return new(Beatmap.Load(path));
    }
    private static void Eq<T>(T actual, T expected)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            throw new InvalidOperationException($"expected={expected}; actual={actual}");
    }
    private static void Yes(bool value, string label)
    {
        if (!value) throw new InvalidOperationException(label);
    }
    private static void Near(Vector2 actual, Vector2 expected)
    {
        if (Vector2.Distance(actual, expected) > 0.0001f)
            throw new InvalidOperationException($"expected near {expected}; actual={actual}");
    }
    private static void Schedule(SliderDefinition d, params int[] expected) =>
        Eq(string.Join(",", d.Checkpoints.Select(c => c.Time)), string.Join(",", expected));
    private static void All(CircleSession s, bool success = true)
    {
        foreach (var c in S(s).Checkpoints) At(s, c.Definition.Time, success ? 1 : 0);
    }
    private static void Add(string name, Action body) => Cases.Add((name, body));

    public static int Main(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException("Pass repository root only.");
        root = Path.GetFullPath(args[0]);
        scratch = Path.Combine(Path.GetTempPath(), "ohno-qa03-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(scratch);
        try
        {
            Register(); int failed = 0;
            foreach (var c in Cases)
            {
                try { c.Body(); Console.WriteLine("PASS " + c.Name); }
                catch (Exception e) { failed++; Console.WriteLine($"FAIL {c.Name}: {e.GetType().Name}: {e.Message}"); }
            }
            Console.WriteLine($"QA03 SLIDER TESTS: {Cases.Count - failed} passed, {failed} failed, {Cases.Count} executed");
            return failed == 0 ? 0 : 1;
        }
        finally { Directory.Delete(scratch, true); }
    }

    private static void Register()
    {
        Add("A01 tick fixture schedule and end", () => {
            var d = S(One("slider-ticks")).Definition;
            Schedule(d, 4000, 5000, 5964); Eq(d.EndTime, 6000); Eq(d.Traversals, 1);
        });
        Add("A02 repeat fixture schedule and end", () => {
            var d = S(One()).Definition;
            Schedule(d, 4000, 5000, 6000, 7000, 8000, 8964); Eq(d.EndTime, 9000); Eq(d.Traversals, 2);
        });
        Add("A03 three-span carry schedule and end", () => {
            var d = S(One("mixed-sliders", 2)).Definition;
            Schedule(d, 22000, 23000, 23600, 24200, 25200, 26200, 27200, 28200, 28764);
            Eq(d.EndTime, 28800); Eq(d.Traversals, 3);
        });
        Add("A04 producer roles retain ordered traversal identity", () => {
            var d = S(One()).Definition;
            Eq(string.Join(",", d.Checkpoints.Select(c => c.Kind)), "Interior,Interior,Repeat,Interior,Interior,Final");
            Eq(string.Join(",", d.Checkpoints.Select(c => c.Traversal)), "0,0,0,1,1,1");
        });
        Add("A05 directed movement endpoints and intervals", () => {
            var d = S(One()).Definition; Eq(d.Movements.Count, 2);
            Eq(d.Movements[0], new SliderMovement(new(106,192), new(406,192), 3000,6000));
            Eq(d.Movements[1], new SliderMovement(new(406,192), new(106,192), 6000,9000));
            Eq(d.EndPosition, new Vector2(106,192));
        });
        Add("A06 three spans preserve nonzero carry and alternate endpoints", () => {
            var d = S(One("mixed-sliders", 2)).Definition;
            Eq(d.Movements.Count, 3);
            Eq(d.Movements[0], new SliderMovement(new(106,128), new(366,128),21000,23600));
            Eq(d.Movements[1], new SliderMovement(new(366,128), new(106,128),23600,26200));
            Eq(d.Movements[2], new SliderMovement(new(106,128), new(366,128),26200,28800));
            // Not a reset tick clock at24600/27200: first reverse interior is24200.
            Eq(d.Checkpoints[3].Time - d.Movements[1].StartTime, 600);
            Eq(d.Checkpoints[6].Time - d.Movements[2].StartTime, 1000);
        });
        Add("B01 forward literal geometry before reversal", () => {
            var d = S(One()).Definition;
            Near(d.RequiredPosition(4500), new(256,192)); Near(d.RequiredPosition(5999), new(405.9f,192));
        });
        Add("B02 exact reversal selects first inclusive record", () => {
            var d = S(One()).Definition; Eq(d.MovementIndex(6000),0); Eq(d.RequiredPosition(6000),new Vector2(406,192));
        });
        Add("B03 reverse immediately after boundary", () => {
            var d = S(One()).Definition; Eq(d.MovementIndex(6001),1);
            Near(d.RequiredPosition(6001),new(405.9f,192)); Near(d.RequiredPosition(7500),new(256,192));
        });
        Add("B04 reverse returns to initial endpoint", () => {
            var d = S(One()).Definition; Eq(d.RequiredPosition(9000),d.Head.Position);
        });
        Add("B05 third traversal continues forward", () => {
            var d = S(One("mixed-sliders",2)).Definition; Eq(d.MovementIndex(26200),1);
            Eq(d.MovementIndex(26201),2); Near(d.RequiredPosition(27500),new(236,128));
            Eq(d.RequiredPosition(28800),new Vector2(366,128));
        });
        Add("B06 reversal alone preserves active entry and held side", () => {
            var s = Started(); At(s,4000); At(s,5000); At(s,6000); At(s,6001);
            Yes(S(s).Tracking && S(s).FollowEligible,"tracking lost at reversal");
            Eq(S(s).EntryTime,3000); Eq(S(s).RememberedMask,1); Eq(S(s).Checkpoints[2].RawResult,64);
        });
        Add("C01 no component before due and inclusive equality", () => {
            var s=Started(); At(s,3999); Eq(S(s).ProcessedCheckpoints,0);
            Yes(S(s).Checkpoints.All(c=>c.Succeeded is null),"pending changed");
            At(s,4000); Eq(S(s).ProcessedCheckpoints,1); Eq(S(s).Checkpoints[0].RawResult,8);
        });
        Add("C02 one outstanding component per invocation", () => {
            var s=Started(); At(s,6000); Eq(S(s).ProcessedCheckpoints,1);
            Eq(S(s).Checkpoints[0].RawResult,64); Eq(S(s).Checkpoints[1].Succeeded,null);
        });
        Add("C03 identical-time calls can drain prefix then stop", () => {
            var s=Started(); At(s,6000); At(s,6000); At(s,6000);
            Eq(S(s).ProcessedCheckpoints,3); Eq(s.Combo,4);
            At(s,6000); Eq(S(s).ProcessedCheckpoints,3); Eq(s.Combo,4);
        });
        Add("C04 delayed interior gets repeat code from due prefix", () => {
            var s=Started(); At(s,6000);
            Eq(S(s).Checkpoints[0].Definition.Kind,SliderCheckpointKind.Interior);
            Eq(S(s).Checkpoints[0].RawResult,64);
        });
        Add("C05 delayed repeat can get interior code", () => {
            var s=Started(); At(s,4000); At(s,5000); At(s,7000);
            Eq(S(s).Checkpoints[2].Definition.Kind,SliderCheckpointKind.Repeat);
            Eq(S(s).Checkpoints[2].RawResult,8);
        });
        Add("C06 all-due prefix gives earlier item terminal success code", () => {
            var s=Started(); At(s,8964); Eq(S(s).ProcessedCheckpoints,1);
            Eq(S(s).Checkpoints[0].RawResult,128); Eq(S(s).Result,Judgement.Pending);
        });
        Add("C07 failed and pending slots remain distinct", () => {
            var s=Started(); At(s,4000,0);
            Eq(S(s).FailedCheckpoints,1); Eq(S(s).SuccessfulCheckpoints,0);
            Eq(S(s).Checkpoints[0].Succeeded,false); Eq(S(s).Checkpoints[1].Succeeded,null);
        });
        Add("C08 missed head permits later checkpoint success", () => {
            var s=One(); At(s,3200,0); Eq(S(s).Head.Result,Judgement.Miss);
            At(s,3500); At(s,4000); Eq(S(s).Checkpoints[0].Succeeded,true);
            Eq(S(s).Head.Result,Judgement.Miss); Eq(S(s).Result,Judgement.Pending);
        });
        Add("D01 entry equality succeeds", () => {
            var s=One(); At(s,4000); Eq(S(s).EntryTime,4000); Eq(S(s).Checkpoints[0].Succeeded,true);
        });
        Add("D02 late entry fails while tracking remains active", () => {
            var s=One(); At(s,4001); Eq(S(s).EntryTime,4001);
            Eq(S(s).Checkpoints[0].Succeeded,false); Yes(S(s).Tracking,"late entry unexpectedly left tracking");
        });
        Add("D03 held input alone is insufficient", () => {
            var s=One(); s.Advance(3000,new(1000,1000),1); Yes(!S(s).Tracking,"outside acquired");
            s.Advance(4000,new(1000,1000),1); Eq(S(s).Checkpoints[0].Succeeded,false);
        });
        Add("D04 cursor alone is insufficient", () => {
            var s=One(); At(s,3000,0); At(s,4000,0);
            Yes(!S(s).Tracking,"unheld acquired"); Eq(S(s).Checkpoints[0].Succeeded,false);
        });
        Add("D05 entry radius equality rejected", () => {
            var s=Origin(); s.Advance(3000,new(s.HitRadius,0),1); Yes(!S(s).Tracking,"entry equality accepted");
        });
        Add("D06 just inside entry radius accepted", () => {
            var s=Origin(); s.Advance(3000,new(float.BitDecrement(s.HitRadius),0),1); Yes(S(s).Tracking,"inside rejected");
        });
        Add("D07 active radius equality rejects and leaves", () => {
            var s=Origin(); At(s,3000);
            s.Advance(3000,new(s.HitRadius*2.4000000953674316f,0),1);
            Yes(!S(s).Tracking && !S(s).FollowEligible,"active equality accepted");
        });
        Add("D08 active hysteresis accepts outside entry disk", () => {
            var s=Origin(); At(s,3000); s.Advance(3000,new(s.HitRadius*2,0),1);
            Yes(S(s).Tracking && S(s).FollowEligible,"hysteresis rejected"); Eq(S(s).EntryTime,3000);
        });
        Add("D09 release and timely re-entry renew entry", () => {
            var s=Started(); At(s,3500,0); Yes(!S(s).Tracking,"release retained active before terminal");
            Eq(S(s).RememberedMask,0); At(s,3900); At(s,4000);
            Eq(S(s).EntryTime,3900); Eq(S(s).Checkpoints[0].Succeeded,true);
        });
        Add("D10 release and late re-entry cannot rescue deadline", () => {
            var s=Started(); At(s,3500,0); At(s,4001);
            Eq(S(s).Checkpoints[0].Succeeded,false); Yes(S(s).Tracking,"re-entry failed to activate");
        });
        Add("D11 both held retains remembered side and no synthetic edge needed", () => {
            var s=Started(); At(s,3500,3); Eq(S(s).RememberedMask,1);
            At(s,4000,3); Eq(S(s).Checkpoints[0].Succeeded,true);
        });
        Add("D12 replacing logical side follows original history branch", () => {
            var s=Started(); At(s,3500,2); Eq(S(s).RememberedMask,2);
            At(s,4000,2); Eq(S(s).Checkpoints[0].Succeeded,true);
        });
        Add("D13 history veto differs from any-held shortcut", () => {
            // Explicit public component arguments, not a physical-input claim:
            // create previous=both, older=current=left, then remember right.
            var s=One(); s.SetHeldButtons(1); s.SetHeldButtons(3); s.SetHeldButtons(1);
            s.Press(S(s).Definition.Head.Position,3000,2); At(s,3000,1);
            Eq(S(s).RememberedMask,2); Yes(!S(s).Tracking,"history veto replaced by any-held");
            At(s,3500,2); At(s,4000,2); Eq(S(s).Checkpoints[0].Succeeded,true);
        });
        Add("D14 NaN follow coordinates reject", () => {
            var s=Started(); s.Advance(4000,new(float.NaN,192),1);
            Eq(S(s).Checkpoints[0].Succeeded,false); Yes(!S(s).FollowEligible,"NaN followed");
        });
        Add("E01 head and components credit combo without parent duplicate", () => {
            var s=Started(); All(s); Eq(s.Combo,7); Eq(s.JudgedCount,0);
            At(s,9000); Eq(s.Combo,7); Eq(s.JudgedCount,1); Eq(S(s).Result,Judgement.Hit300);
        });
        Add("E02 ordinary interior failure resets positive combo", () => {
            var s=Started(); At(s,4000); Eq(s.Combo,2); At(s,5000,0);
            Eq(S(s).Checkpoints[1].RawResult,-262144); Eq(s.Combo,0); Eq(s.MaxCombo,2);
        });
        Add("E03 repeat success increments combo", () => {
            var s=Started(); At(s,4000); At(s,5000); At(s,6000);
            Eq(S(s).Checkpoints[2].RawResult,64); Eq(s.Combo,4);
        });
        Add("E04 repeat failure uses ordinary negative reset", () => {
            var s=Started(); At(s,4000); At(s,5000); At(s,6000,0);
            Eq(S(s).Checkpoints[2].RawResult,-262144); Eq(s.Combo,0);
        });
        Add("E05 sequential final success increments combo before completion", () => {
            var s=Started(); All(s); Eq(S(s).Checkpoints[5].RawResult,128);
            Eq(s.Combo,7); Eq(S(s).Result,Judgement.Pending); Eq(S(s).Head.Result,Judgement.Hit300);
        });
        Add("E06 final failure preserves combo and guarded active flag", () => {
            var s=Started(); foreach(var t in new[]{4000,5000,6000,7000,8000}) At(s,t);
            Eq(s.Combo,6); At(s,8964,0); Eq(S(s).Checkpoints[5].RawResult,-524288);
            Eq(S(s).Checkpoints[5].Succeeded,false); Eq(s.Combo,6);
            Yes(S(s).Tracking && !S(s).FollowEligible,"terminal leave guard flattened");
        });
        Add("E08 terminal entry equality after previous failures succeeds", () => {
            var s=Started(); foreach(var t in new[]{4000,5000,6000,7000,8000}) At(s,t,0);
            At(s,8964); Eq(S(s).EntryTime,8964); Eq(S(s).Checkpoints[5].RawResult,128);
            Eq(S(s).SuccessfulCheckpoints,1); Eq(S(s).FailedCheckpoints,5);
        });
        Add("E09 terminal entry after deadline fails while active", () => {
            var s=Started(); foreach(var t in new[]{4000,5000,6000,7000,8000}) At(s,t,0);
            At(s,8965); Eq(S(s).EntryTime,8965); Eq(S(s).Checkpoints[5].RawResult,-524288);
            Yes(S(s).Tracking,"terminal late entry lost active state"); Eq(S(s).Result,Judgement.Pending);
        });
        Add("E07 all due failure is nonterminal until processed count reaches N", () => {
            var s=Started(); At(s,8964,0); Eq(S(s).Checkpoints[0].RawResult,-262144);
            Eq(S(s).ProcessedCheckpoints,1); Eq(S(s).TerminalSucceeded,null);
        });
        Add("F01 inclusive parent end distinct from terminal", () => {
            var s=Started(); All(s); At(s,8999); Eq(S(s).Result,Judgement.Pending);
            At(s,9000); Eq(S(s).Result,Judgement.Hit300);
        });
        Add("F02 delayed parent completion leaves backlog untouched", () => {
            var s=Started(); At(s,9000); Eq(S(s).ProcessedCheckpoints,1);
            Eq(S(s).Checkpoints[0].RawResult,128); Eq(S(s).Result,Judgement.Hit50);
            Eq(S(s).Checkpoints.Count(c=>c.Succeeded is null),5);
        });
        Add("F03 evaluated parent suppresses later component processing", () => {
            var s=Started(); At(s,9000); int combo=s.Combo;
            At(s,9000); At(s,9001); Eq(S(s).ProcessedCheckpoints,1); Eq(s.JudgedCount,1); Eq(s.Combo,combo);
        });
        Add("F04 terminal failure still permits positive parent aggregate", () => {
            var s=Started(); foreach(var t in new[]{4000,5000,6000,7000,8000}) At(s,t);
            At(s,8964,0); At(s,9000,0); Eq(S(s).Result,Judgement.Hit100); Eq(s.Combo,6);
        });
        Add("F05 legacy full successes with low positive head still best", () => {
            var s=One(); var d=S(s).Definition;
            s.Press(d.Head.Position,d.Head.StartTime+s.Window100); Eq(S(s).Head.Result,Judgement.Hit50);
            At(s,3200); All(s); At(s,9000); Eq(S(s).Result,Judgement.Hit300);
        });
        Add("F06 missing head and all components successful gives middle", () => {
            var s=One(); At(s,3200,0); At(s,3500); All(s); At(s,9000);
            Eq(S(s).Head.Result,Judgement.Miss); Eq(S(s).Result,Judgement.Hit100);
        });
        Add("F07 all failed including head gives miss", () => {
            var s=One(); All(s,false); At(s,9000,0); Eq(S(s).Result,Judgement.Miss); Eq(s.Combo,0);
        });
        Add("F08 below half and above half use recorded successes", () => {
            foreach(var count in new[]{2,3}) {
                var s=Started(); var checkpoints=S(s).Checkpoints;
                for(int i=0;i<checkpoints.Count;i++) At(s,checkpoints[i].Definition.Time,i<count?1:0);
                At(s,9000,0); Eq(S(s).Result,count==2?Judgement.Hit50:Judgement.Hit100);
            }
        });
        Add("F09 pending versus failed equal numerator but different histories", () => {
            var pending=Started(); At(pending,4000); At(pending,9000,0);
            var failed=Started(); At(failed,4000);
            foreach(var t in new[]{5000,6000,7000,8000,8964}) At(failed,t,0);
            At(failed,9000,0); Eq(S(pending).Result,S(failed).Result);
            Eq(S(pending).Result,Judgement.Hit50); Eq(S(pending).FailedCheckpoints,1);
            Eq(S(failed).FailedCheckpoints,5); Eq(S(pending).Checkpoints.Count(c=>c.Succeeded is null),4);
        });
        Add("R04-01 unchanged single-span fixture preparation", () => {
            var d=S(One("first-slider")).Definition; Schedule(d,6564); Eq(d.EndTime,6600);
            Eq(d.Movements.Single(),new SliderMovement(new(160,192),new(320,192),5000,6600));
        });
        Add("R04-02 head and terminal independently produce best legacy grade", () => {
            var s=Started("first-slider"); At(s,6564); Eq(S(s).TerminalSucceeded,true);
            Eq(S(s).Result,Judgement.Pending); Eq(s.Combo,2); At(s,6600); Eq(S(s).Result,Judgement.Hit300); Eq(s.Combo,2);
        });
        Add("R04-03 head only exactly half grades middle", () => {
            var s=Started("first-slider"); At(s,6564,0); At(s,6600,0);
            Eq(S(s).TerminalSucceeded,false); Eq(S(s).Result,Judgement.Hit100); Eq(s.Combo,1);
        });
        Add("R04-04 timely release and re-entry preserves terminal", () => {
            var s=Started("first-slider"); At(s,6000,0); At(s,6500); At(s,6564); At(s,6600);
            Eq(S(s).EntryTime,6500); Eq(S(s).Result,Judgement.Hit300);
        });
    }
}
