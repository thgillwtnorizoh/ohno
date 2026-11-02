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

public sealed class CircleSession
{
    public Beatmap Map { get; }
    public IReadOnlyList<CircleState> Circles { get; }
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

    public void Advance(int time)
    {
        if (Finished) return;
        foreach (CircleState circle in Circles)
        {
            // E12 M28e4: strict > at timeout, not >=.
            if (circle.Result == Judgement.Pending && time > circle.Definition.StartTime + Window50)
                Judge(circle, time);
        }
        // R02's own result overlay uses E27's B+3000 horizon, not its mode machine.
        if (time >= Map.Circles[^1].StartTime + 3000) Finished = true;
    }

    public void Press(Vector2 pointer, int time)
    {
        if (Finished) return;
        // E11/E13: first spatial/time success in ordered candidates, no nearest sort.
        // Full E07 candidate storage/opaque gates are not implemented in R02.
        foreach (CircleState circle in Circles)
        {
            int start = circle.Definition.StartTime;
            if (circle.Result != Judgement.Pending || time < start - Approach || time > start + Window50) continue;
            if (Vector2.DistanceSquared(pointer, circle.Definition.Position) > HitRadius * HitRadius) continue;
            // E11 earlier unevaluated-object admission gate, ordinary distinct times.
            bool blocked = Circles.TakeWhile(c => !ReferenceEquals(c, circle)).Any(c =>
                c.Result == Judgement.Pending && c.Definition.StartTime + 3 < start);
            if (blocked || Math.Abs(time - start) >= 400)
            {
                Feedback = new(circle.Definition.Position, blocked ? "Earlier circle first" : "Too early", time, Judgement.Pending);
                return;
            }
            Judge(circle, time);
            return;
        }
    }

    private void Judge(CircleState circle, int time)
    {
        int error = Math.Abs(time - circle.Definition.StartTime);
        // E12 M1884: equality drops to the next quality; no extra ms tolerance.
        circle.Result = error < Window300 ? Judgement.Hit300 : error < Window100 ?
            Judgement.Hit100 : error < Window50 ? Judgement.Hit50 : Judgement.Miss;
        circle.JudgedAt = time;
        JudgedCount++;
        int value = circle.Result switch { Judgement.Hit300 => 300, Judgement.Hit100 => 100, Judgement.Hit50 => 50, _ => 0 };
        // Deliberately simple R02 feedback counters, NOT stable's score/HP formula.
        HitValueTotal += value;
        Combo = value == 0 ? 0 : Combo + 1;
        MaxCombo = Math.Max(MaxCombo, Combo);
        Feedback = new(circle.Definition.Position, value == 0 ? "MISS" : value.ToString(), time, circle.Result);
    }
}
