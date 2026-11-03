using System.Numerics;

namespace Ohno.CircleGame.Gameplay;

public enum Judgement { Pending, Hit300, Hit100, Hit50, Miss }

public sealed class CircleState(CircleDefinition definition)
{
    public CircleDefinition Definition { get; } = definition;
    public Judgement Result { get; internal set; }
    public int JudgedAt { get; internal set; }
}

public sealed record HitFeedback(Vector2 Position, string Text, int Time, Judgement Result);

// E25: observed integer changes are independent of accumulated P and final grade.
public sealed class SpinnerProgressObservation
{
    public int CachedInteger { get; private set; }
    public int Count { get; private set; }
    public int Observe(float progress, int required)
    {
        if (!float.IsFinite(progress) || progress <= int.MinValue || (double)progress >= 2147483648.0)
            throw new ArgumentOutOfRangeException(nameof(progress), "Finite in-range Single progress required.");
        int observed = (int)progress;
        if (observed == CachedInteger) return 0;
        Count = unchecked(Count + 1); // One increment, even if many integers were skipped.
        int raw = Count > unchecked(required + 3) && unchecked(Count - unchecked(required + 3)) % 2 == 0 ? 16384 :
            Count > 1 ? (Count % 2 == 0 ? 8192 : 4096) : 0;
        // Original cache follows selection/helpers and a second P read. This serial
        // model has no opaque/reentrant hooks; its unchanged P is the second value.
        CachedInteger = (int)progress;
        return raw; // No score, bonus payout or combo operation.
    }
}

public sealed class SpinnerState
{
    public const double InitialInterval = 16.666666666666668;
    public static readonly Vector2 DisplayCenter = new(256, 192); // ENGINEERING prepared-center substitute.
    public SpinnerDefinition Definition { get; }
    public int Required { get; }
    public double Acceleration { get; }
    public double? PreviousAngle { get; private set; }
    public double IntervalEma { get; private set; } = InitialInterval;
    public int StationarySamples { get; private set; }
    public double TargetMotion { get; private set; }
    public double SmoothedMotion { get; private set; }
    public float Progress { get; private set; }
    public SpinnerProgressObservation Observation { get; } = new();
    public bool InputHeld { get; private set; }
    public double VisualRotation { get; private set; } // Independent primitive presentation.
    public int LastComponentRaw { get; private set; }
    public int FinalRaw { get; private set; }
    public Judgement Result { get; private set; }

    public SpinnerState(SpinnerDefinition definition, float overallDifficulty)
    {
        Definition = definition;
        int duration = definition.EndTime - definition.StartTime;
        if (duration <= 0 || duration > 60_000 || !float.IsFinite(overallDifficulty) || overallDifficulty < 0 || overallDifficulty > 10)
            throw new ArgumentOutOfRangeException(nameof(definition), "R07 requires positive bounded duration and finite OD0–10.");
        double od = overallDifficulty;
        double preparedRate = od > 5 ? 5 + (7.5 - 5) * (od - 5) / 5 : 5 - (5 - 3) * (5 - od) / 5;
        Required = (int)((double)((float)duration / 1000f) * preparedRate); // E26 conversion sequence.
        Acceleration = 0.00008 + Math.Max(0.0, ((5000.0 - duration) / 1000.0) / 2000.0);
    }

    public int SampleCursor(Vector2 pointer, Vector2 center, int time, double sampleInterval, bool held)
    {
        Vector2 relative = pointer - center; // Single subtraction, Y then X to Double atan2.
        return SampleAngle(Math.Atan2((double)relative.Y, (double)relative.X), time, sampleInterval, held);
    }

    public int SampleAngle(double theta, int time, double sampleInterval, bool held)
    {
        if (!double.IsFinite(theta) || !double.IsFinite(sampleInterval)) throw new ArgumentOutOfRangeException(nameof(theta));
        if (sampleInterval < 0) return 0;
        // Modern/no-record compatibility policy only: first reference is theta,
        // not E24's historical pre-20190510 source-record reference of zero.
        double previous = PreviousAngle ?? theta;
        double delta = theta - previous;
        if (theta - previous < -Math.PI) delta = 2 * Math.PI + theta - previous;
        else if (previous - theta < -Math.PI) delta = -2 * Math.PI - previous + theta;
        double weight = Math.Pow(0.999, sampleInterval);
        IntervalEma = weight * IntervalEma + (1 - weight) * sampleInterval;
        InputHeld = held;
        if (delta == 0)
        {
            int old = StationarySamples;
            StationarySamples = unchecked(old + 1);
            TargetMotion = old >= 1 ? 0 : TargetMotion / 3.0;
        }
        else
        {
            StationarySamples = 0;
            if (!held || time < Definition.StartTime || time > Definition.EndTime) delta = 0;
            TargetMotion = Math.Abs(delta) >= Math.PI ? 0 : IntervalEma > 17.333332697550457 ?
                (sampleInterval > 0 ? delta / sampleInterval : 0) : delta / InitialInterval;
        }
        PreviousAngle = theta; // Also after movement/hold/time rejection.
        LastComponentRaw = Observation.Observe(Progress, Required); // Old P before later integration.
        return LastComponentRaw;
    }

    public void Integrate(int time, int updateInterval)
    {
        if (Result != Judgement.Pending || time <= Definition.StartTime || time >= Definition.EndTime || updateInterval <= 0) return;
        double allowance = Acceleration * (double)updateInterval; // Plain M28ae: no mods/Relax/Auto.
        SmoothedMotion += TargetMotion > SmoothedMotion ? Math.Min(TargetMotion - SmoothedMotion, allowance) :
            Math.Max(TargetMotion - SmoothedMotion, -allowance);
        SmoothedMotion = Math.Max(-0.05, Math.Min(SmoothedMotion, 0.05));
        float step = (float)(SmoothedMotion * (double)updateInterval);
        VisualRotation += step;
        Progress = (float)(Progress + Math.Min(1f, Math.Abs((float)((double)step / Math.PI))));
    }

    public static int ClassifyModern(int count, int required)
    {
        if (required == 0) return 1024;
        if (count < Math.Max(0, required / 4)) return -131072;
        if (count > required) return 1024;
        return count > unchecked(required - 2) ? 512 : 256;
    }

    public bool Complete(int time)
    {
        if (Result != Judgement.Pending || time <= Definition.EndTime) return false;
        // E26: no Observe call, P drain, counter increment or cache repair here.
        FinalRaw = ClassifyModern(Observation.Count, Required);
        Result = FinalRaw switch { 1024 => Judgement.Hit300, 512 => Judgement.Hit100, 256 => Judgement.Hit50, _ => Judgement.Miss };
        return true;
    }
}

public sealed class SliderCheckpointState(SliderCheckpoint definition)
{
    public SliderCheckpoint Definition { get; } = definition;
    public bool? Succeeded { get; internal set; } // null is pending, distinct from failure.
    public int RawResult { get; internal set; }
    public int? JudgedAt { get; internal set; }
}

public sealed class SliderState(SliderDefinition definition)
{
    public SliderDefinition Definition { get; } = definition;
    public CircleState Head { get; } = new(definition.Head);
    public Judgement Result { get; internal set; }
    public bool Tracking { get; internal set; }
    public bool FollowEligible { get; internal set; }
    public int EntryTime { get; internal set; }
    public IReadOnlyList<SliderCheckpointState> Checkpoints { get; } = definition.Checkpoints.Select(c => new SliderCheckpointState(c)).ToArray();
    public int SuccessfulCheckpoints { get; internal set; }
    public int FailedCheckpoints { get; internal set; }
    public int ProcessedCheckpoints => SuccessfulCheckpoints + FailedCheckpoints;
    public bool? TerminalSucceeded => Checkpoints[^1].Succeeded;
    internal int RememberedMask;
}

// E15's logical-side/history projection; the WPF wrapper publishes real Z/X state.
internal sealed class HeldInput
{
    private int current, previous, older;
    private bool leftNew, rightNew;
    public void Publish(int mask)
    {
        mask &= 3;
        if (mask == current) return;
        leftNew = (mask & 1) != 0 && (current & 1) == 0;
        rightNew = (mask & 2) != 0 && (current & 2) == 0;
        older = previous; previous = current; current = mask;
    }
    public bool For(SliderState slider)
    {
        bool auxiliary = current != 0 && (previous != 3 || older != current);
        bool held = false;
        if (current != 0)
        {
            if (slider.RememberedMask == 0 || (current != 3 && auxiliary))
            {
                slider.RememberedMask = leftNew ? 1 : rightNew ? 2 : current;
                held = true;
            }
            else held = (current & slider.RememberedMask) > 0;
        }
        else slider.RememberedMask = 0;
        return held || auxiliary; // No Relax branch in this no-mod client.
    }
    public void ConsumeEdges() { leftNew = false; rightNew = false; }
}

public sealed class CircleSession
{
    public Beatmap Map { get; }
    public IReadOnlyList<CircleState> Circles { get; }
    public IReadOnlyList<SliderState> Sliders { get; }
    public IReadOnlyList<SpinnerState> Spinners { get; }
    public IReadOnlyList<CircleState> Heads { get; }
    private readonly List<(CircleState Head, SliderState? Slider)> targets;
    private readonly HeldInput input = new();
    private int previousUpdateTime, previousQualifyingInputTime;
    public int ObjectCount => Circles.Count + Sliders.Count + Spinners.Count;
    public IEnumerable<Judgement> ObjectResults => Circles.Select(c => c.Result).Concat(Sliders.Select(s => s.Result)).Concat(Spinners.Select(s => s.Result));
    public int Approach { get; }
    public int Window300 { get; }
    public int Window100 { get; }
    public int Window50 { get; }
    public float DiameterAtScaleOne { get; }
    public float HitRadius { get; }
    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }
    public int JudgedCount { get; private set; }
    public int HitValueTotal { get; private set; }
    public HitFeedback? Feedback { get; private set; }
    public bool Finished { get; private set; }
    public double Accuracy => JudgedCount == 0 ? 100 : 100.0 * HitValueTotal / (JudgedCount * 300);

    public CircleSession(Beatmap map)
    {
        Map = map;
        Circles = map.Circles.Select(c => new CircleState(c)).ToArray();
        Sliders = map.Sliders.Select(s => new SliderState(s)).ToArray();
        Spinners = map.Spinners.Select(s => new SpinnerState(s, map.OverallDifficulty)).ToArray();
        targets = Circles.Select(c => (Head: c, Slider: (SliderState?)null))
            .Concat(Sliders.Select(s => (Head: s.Head, Slider: (SliderState?)s)))
            .OrderBy(t => t.Head.Definition.StartTime).ToList();
        Heads = targets.Select(t => t.Head).ToArray();
        // E07 M28be/M28b3: no-mod finite approach mapping, truncation toward zero.
        Approach = MapDifficulty(map.ApproachRate, 1800, 1200, 450);
        // E12 M28be widths, read in 300/100/50 classification order below.
        Window300 = MapDifficulty(map.OverallDifficulty, 80, 50, 20);
        Window100 = MapDifficulty(map.OverallDifficulty, 140, 100, 60);
        Window50 = MapDifficulty(map.OverallDifficulty, 200, 150, 100);
        // E13 M28be at a supplied scale-one playfield W=512, s=1.
        DiameterAtScaleOne = (float)(64.0 * (1 - 0.699999988079071 * ((map.CircleSize - 5.0) / 5.0)));
        HitRadius = (DiameterAtScaleOne / 2f) * 1.0004099607467651f;
    }

    private static int MapDifficulty(double d, double a, double b, double c) =>
        (int)(d > 5 ? b + (c - b) * (d - 5) / 5 : d < 5 ? b - (b - a) * (5 - d) / 5 : b);

    public void SetHeldButtons(int buttons) => input.Publish(buttons);

    // Existing headless/circle API: no supplied live input means no held tracking.
    public void Advance(int time) => Advance(time, new Vector2(float.NaN, float.NaN), 0);

    public void Advance(int time, Vector2 pointer, int buttons, Vector2? spinnerPointer = null)
    {
        if (Finished) return;
        input.Publish(buttons);
        int updateInterval = unchecked(time - previousUpdateTime);
        double sampleInterval = previousQualifyingInputTime > 0 ? (double)unchecked(time - previousQualifyingInputTime) : SpinnerState.InitialInterval;
        Vector2 spinPointer = spinnerPointer ?? pointer;
        foreach (SpinnerState spinner in Spinners)
        {
            if (spinner.Result != Judgement.Pending || time < spinner.Definition.StartTime) continue;
            // ENGINEERING: every running UI callback is a qualifying sample. The
            // two interval histories remain distinct; original timing-tail gates are not reproduced.
            if (float.IsFinite(spinPointer.X) && float.IsFinite(spinPointer.Y))
                spinner.SampleCursor(spinPointer, SpinnerState.DisplayCenter, time, sampleInterval, (buttons & 3) != 0);
            spinner.Integrate(time, updateInterval);
            if (spinner.Complete(time))
            {
                RecordResult(spinner.Result, true);
                Feedback = new(SpinnerState.DisplayCenter, "SPINNER " + Label(spinner.Result), time, spinner.Result);
            }
        }
        previousUpdateTime = time;
        previousQualifyingInputTime = time; // Global frame history, including passes before spinner activation.
        foreach (SliderState slider in Sliders)
        {
            if (slider.Result == Judgement.Pending && time >= slider.Definition.Head.StartTime) Track(slider, pointer, time);
            if (slider.Head.Result == Judgement.Pending && time > slider.Definition.Head.StartTime + Window50)
                JudgeHead(slider, time);
            // E17: tracking/component decision precedes inclusive parent completion.
            if (slider.Result == Judgement.Pending && time >= slider.Definition.EndTime)
            {
                int successes = (Positive(slider.Head.Result) ? 1 : 0) + slider.SuccessfulCheckpoints;
                double ratio = successes / (double)(slider.Checkpoints.Count + 1);
                // E17/E23 legacy policy (no ScoreV2/mods), not head-sensitive grading.
                slider.Result = ratio == 1 ? Judgement.Hit300 : ratio >= 0.5 ? Judgement.Hit100 :
                    ratio > 0 ? Judgement.Hit50 : Judgement.Miss;
                RecordResult(slider.Result, false); // Whole-object accuracy, no duplicate combo award.
                Feedback = new(slider.Definition.EndPosition, "SLIDER " + Label(slider.Result), time, slider.Result);
            }
        }
        input.ConsumeEdges();
        foreach (CircleState circle in Circles)
        {
            // E12 M28e4: strict > at timeout, not >=.
            if (circle.Result == Judgement.Pending && time > circle.Definition.StartTime + Window50)
                Judge(circle, time);
        }
        // R02's own result overlay uses E27's B+3000 horizon, not its mode machine.
        int lastEnd = targets.Select(t => t.Slider?.Definition.EndTime ?? t.Head.Definition.StartTime)
            .Concat(Spinners.Select(s => s.Definition.EndTime)).Max();
        if (time >= lastEnd + 3000) Finished = true;
    }

    public void Press(Vector2 pointer, int time, int side = 1)
    {
        if (Finished) return;
        if (Spinners.Any(s => s.Result == Judgement.Pending && time >= s.Definition.StartTime)) return;
        // E11/E13: first spatial/time success in ordered candidates, no nearest sort.
        // Full E07 candidate storage/opaque gates are not implemented in R02.
        foreach (var target in targets)
        {
            CircleState circle = target.Head;
            int start = circle.Definition.StartTime;
            if (circle.Result != Judgement.Pending || target.Slider is { Result: not Judgement.Pending } ||
                time < start - Approach || time > start + Window50) continue;
            // QA01-D01: ordered acceptance rejects NaN while retaining finite equality.
            if (!(Vector2.DistanceSquared(pointer, circle.Definition.Position) <= HitRadius * HitRadius)) continue;
            // E11 earlier unevaluated-object admission gate, ordinary distinct times.
            bool blocked = Spinners.Any(s => s.Result == Judgement.Pending && s.Definition.EndTime + 3 < start) ||
                targets.TakeWhile(t => !ReferenceEquals(t.Head, circle)).Any(t =>
                (t.Slider?.Result ?? t.Head.Result) == Judgement.Pending &&
                (t.Slider?.Definition.EndTime ?? t.Head.Definition.StartTime) + 3 < start);
            if (blocked || Math.Abs(time - start) >= 400)
            {
                Feedback = new(circle.Definition.Position, blocked ? "Earlier circle first" : "Too early", time, Judgement.Pending);
                return;
            }
            if (target.Slider is { } slider)
            {
                slider.RememberedMask = side == 2 ? 2 : 1;
                JudgeHead(slider, time);
            }
            else Judge(circle, time);
            return;
        }
    }

    private void Judge(CircleState circle, int time)
    {
        circle.Result = Classify(time - circle.Definition.StartTime);
        circle.JudgedAt = time;
        RecordResult(circle.Result, true);
        Feedback = new(circle.Definition.Position, Label(circle.Result), time, circle.Result);
    }

    private Judgement Classify(int delta)
    {
        int error = Math.Abs(delta);
        // E12/E15: strict windows, equality drops to the next branch.
        return error < Window300 ? Judgement.Hit300 : error < Window100 ?
            Judgement.Hit100 : error < Window50 ? Judgement.Hit50 : Judgement.Miss;
    }

    private static bool Positive(Judgement result) => result is Judgement.Hit300 or Judgement.Hit100 or Judgement.Hit50;
    private static string Label(Judgement result) => result switch
    { Judgement.Hit300 => "300", Judgement.Hit100 => "100", Judgement.Hit50 => "50", _ => "MISS" };

    private void JudgeHead(SliderState slider, int time)
    {
        slider.Head.Result = Classify(time - slider.Definition.Head.StartTime);
        slider.Head.JudgedAt = time;
        if (Positive(slider.Head.Result)) AddCombo(); else Combo = 0;
        Feedback = new(slider.Definition.Head.Position, "HEAD " + Label(slider.Head.Result), time, slider.Head.Result);
        // Head evaluation does not finish the parent and does not enter tracking.
    }

    private void Track(SliderState slider, Vector2 pointer, int time)
    {
        float radius = HitRadius * (slider.Tracking ? 2.4000000953674316f : 1f);
        bool follow = input.For(slider) && Vector2.DistanceSquared(pointer, slider.Definition.RequiredPosition(time)) < radius * radius;
        slider.FollowEligible = follow;
        if (follow && !slider.Tracking) { slider.EntryTime = time; slider.Tracking = true; }
        // E16/E21: leading due-prefix only, in original producer order. The final
        // adjustment can be nonmonotone. One reached call processes at most ONE.
        int raw = 0, k = 0, count = slider.Checkpoints.Count;
        while (k < count && slider.Checkpoints[k].Definition.Time <= time) k++;
        if (slider.ProcessedCheckpoints < k)
        {
            var checkpoint = slider.Checkpoints[slider.ProcessedCheckpoints];
            bool success = follow && slider.EntryTime <= checkpoint.Definition.Time;
            if (success)
            {
                slider.SuccessfulCheckpoints++;
                // k, NOT the processed index or producer role, selects success code.
                raw = k == count ? 128 : k % (count / slider.Definition.Traversals) == 0 ? 64 : 8;
            }
            else
            {
                slider.FailedCheckpoints++;
                raw = slider.ProcessedCheckpoints == count ? -524288 : -262144;
            }
            checkpoint.Succeeded = success;
            checkpoint.RawResult = raw;
            checkpoint.JudgedAt = time;
            string role = checkpoint.Definition.Kind switch
            { SliderCheckpointKind.Interior => "TICK", SliderCheckpointKind.Repeat => "REPEAT", _ => "TAIL" };
            Feedback = new(checkpoint.Definition.Position, role + (success ? " HIT" : " MISSED"), time,
                success ? Judgement.Hit300 : Judgement.Miss);
        }
        if (!follow && slider.Tracking && slider.ProcessedCheckpoints < count) slider.Tracking = false;
        // E18/E21/E22: leave precedes the immediate combo consumer. Positive
        // component increments; ordinary negative resets; final negative does not.
        // No full score/HP consumer or original helper exceptions are reproduced.
        if (raw > 0) AddCombo(); else if (raw == -262144) Combo = 0;
    }

    private void AddCombo() { Combo++; MaxCombo = Math.Max(MaxCombo, Combo); }

    private void RecordResult(Judgement result, bool updateCombo)
    {
        JudgedCount++;
        int value = result switch { Judgement.Hit300 => 300, Judgement.Hit100 => 100, Judgement.Hit50 => 50, _ => 0 };
        // Deliberately simple R02 feedback counters, NOT stable's score/HP formula.
        HitValueTotal += value;
        if (updateCombo) { if (value == 0) Combo = 0; else AddCombo(); }
    }
}
