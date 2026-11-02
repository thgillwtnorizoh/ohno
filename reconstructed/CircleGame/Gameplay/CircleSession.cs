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

public sealed class SliderState(SliderDefinition definition)
{
    public SliderDefinition Definition { get; } = definition;
    public CircleState Head { get; } = new(definition.Head);
    public Judgement Result { get; internal set; }
    public bool Tracking { get; internal set; }
    public bool FollowEligible { get; internal set; }
    public int EntryTime { get; internal set; }
    public bool? TerminalSucceeded { get; internal set; }
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
    public IReadOnlyList<CircleState> Heads { get; }
    private readonly List<(CircleState Head, SliderState? Slider)> targets;
    private readonly HeldInput input = new();
    public int ObjectCount => Circles.Count + Sliders.Count;
    public IEnumerable<Judgement> ObjectResults => Circles.Select(c => c.Result).Concat(Sliders.Select(s => s.Result));
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

    public void Advance(int time, Vector2 pointer, int buttons)
    {
        if (Finished) return;
        input.Publish(buttons);
        foreach (SliderState slider in Sliders)
        {
            if (slider.Result == Judgement.Pending && time >= slider.Definition.Head.StartTime) Track(slider, pointer, time);
            if (slider.Head.Result == Judgement.Pending && time > slider.Definition.Head.StartTime + Window50)
                JudgeHead(slider, time);
            // E17: tracking/component decision precedes inclusive parent completion.
            if (slider.Result == Judgement.Pending && time >= slider.Definition.EndTime)
            {
                int successes = (Positive(slider.Head.Result) ? 1 : 0) + (slider.TerminalSucceeded == true ? 1 : 0);
                double ratio = successes / 2.0;
                // E17/E23 legacy policy (no ScoreV2/mods), not head-sensitive grading.
                slider.Result = ratio == 1 ? Judgement.Hit300 : ratio >= 0.5 ? Judgement.Hit100 :
                    ratio > 0 ? Judgement.Hit50 : Judgement.Miss;
                RecordResult(slider.Result, false); // Whole-object accuracy, no duplicate combo award.
                Feedback = new(slider.Definition.Tail, "SLIDER " + Label(slider.Result), time, slider.Result);
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
        int lastEnd = targets.Max(t => t.Slider?.Definition.EndTime ?? t.Head.Definition.StartTime);
        if (time >= lastEnd + 3000) Finished = true;
    }

    public void Press(Vector2 pointer, int time, int side = 1)
    {
        if (Finished) return;
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
            bool blocked = targets.TakeWhile(t => !ReferenceEquals(t.Head, circle)).Any(t =>
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
        // E16/E22: one scheduled terminal, inclusive deadline; entry must be timely.
        if (slider.TerminalSucceeded is null && time >= slider.Definition.TerminalTime)
        {
            slider.TerminalSucceeded = follow && slider.EntryTime <= slider.Definition.TerminalTime;
            if (slider.TerminalSucceeded == true) AddCombo();
            // E22 terminal failure (-524288) has comboEnabled=false; no reset here.
            Feedback = new(slider.Definition.Tail, slider.TerminalSucceeded == true ? "TAIL HIT" : "TAIL MISSED", time,
                slider.TerminalSucceeded == true ? Judgement.Hit300 : Judgement.Miss);
        }
        if (!follow && slider.Tracking && slider.TerminalSucceeded is null) slider.Tracking = false;
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
