using System.Numerics;

namespace Ohno.CircleGame.Gameplay;

public enum Judgement { Pending, Hit300, Hit100, Hit50, Miss }

public enum ScorePolicy { PlainLegacy, ScoreV2 }
public sealed record ScoreConfiguration(double K, ScorePolicy Policy, string MultiplierProvenance)
{
    // ENGINEERING chart preset, not recovered automatic density preparation.
    public static ScoreConfiguration EngineeringPractice { get; } = new(1.0, ScorePolicy.PlainLegacy, "ENGINEERING preset");

    public static ScoreConfiguration FromBeatmap(Beatmap map)
    {
        LegacyScoreMetadata metadata = map.ScoreMetadata ?? throw new System.IO.InvalidDataException(
            "Automatic scoring requires fresh .osu metadata; synthetic maps must supply an explicit engineering configuration.");
        LegacyScoreMultiplier prepared = metadata.Prepare();
        // E034 ordinary no-mod factor is1.0; do not infer modifiers/cache state.
        return new((double)prepared.K * 1.0, ScorePolicy.PlainLegacy,
            $"AUTO E034 fresh .osu N{metadata.RowCount}/T{metadata.WholeSeconds}s/B{metadata.BreakMilliseconds}ms");
    }
}
public enum ScoreObjectKind { Circle, Slider, Spinner }
public enum ScoreEventKind { CircleWhole, SliderHead, SliderInterior, SliderRepeatOrTerminal, SliderAggregate, SpinnerProgress, SpinnerFinal }
public readonly record struct ScoreSource(int ObjectId, ScoreObjectKind Kind, int StartTime, int EndTime);
public readonly record struct ScoreEvent(ScoreSource Source, int Sequence, ScoreEventKind Kind, int Raw,
    bool AccountingEnabled, bool ObjectHasSliderType, ScorePolicy Policy, SliderCheckpointKind? ProducerRole = null);
public enum ScoreAccountingStatus { Applied, RecordSuppressed, NoEvent, Unsupported }
public readonly record struct ScoreAccountingOutcome(ScoreAccountingStatus Status, int BaseAmount, int CalculatedDelta,
    int AppliedDelta, int ComboBefore);

// Dedicated E33 ledger component, independent of session/UI. Kept in this linked
// gameplay source so unchanged QA01–QA05 projects compile the actual dependency.
public sealed class ScoreLedger
{
    public ScoreConfiguration Configuration { get; }
    public int Score { get; private set; }
    public int CurrentCombo { get; private set; }
    public int MaximumCombo { get; private set; }
    public ushort ComboSnapshot { get; private set; }
    public ushort N300 { get; private set; }
    public ushort N100 { get; private set; }
    public ushort N50 { get; private set; }
    public ushort NMiss { get; private set; }
    public int EventCount { get; private set; } // ENGINEERING audit count, not original score.
    public ScoreEvent? LastEvent { get; private set; }
    public ScoreAccountingOutcome LastOutcome { get; private set; } = new(ScoreAccountingStatus.NoEvent, 0, 0, 0, 0);
    public int LastBaseAmount { get; private set; }
    public string? UnsupportedReason { get; private set; }
    public bool IsSupported => UnsupportedReason is null;
    public int TotalWholeResults => unchecked(N50 + N100 + N300 + NMiss);
    public int WeightedHitTotal => unchecked(50 * N50 + 100 * N100 + 300 * N300);
    public float AccuracyFraction => TotalWholeResults <= 0 ? 1f : (float)WeightedHitTotal / (float)unchecked(300 * TotalWholeResults);
    public float AccuracyPercent => AccuracyFraction * 100f;
    public string PolicyLabel => $"{(Configuration.Policy == ScorePolicy.PlainLegacy ? "Plain legacy" : "Unsupported policy")} · K={Configuration.K:G} ({Configuration.MultiplierProvenance})";

    public ScoreLedger(ScoreConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        if (!double.IsFinite(configuration.K) || configuration.K < 0 || configuration.K > 10 ||
            string.IsNullOrWhiteSpace(configuration.MultiplierProvenance))
            throw new ArgumentOutOfRangeException(nameof(configuration), "R08 requires finite K0–10 and explicit provenance.");
        Configuration = configuration;
        if (configuration.Policy != ScorePolicy.PlainLegacy) UnsupportedReason = "Only the plain null-ScoreV2 legacy policy is implemented.";
    }

    public ScoreAccountingOutcome Apply(ScoreEvent e) => Apply(e, null);

    public ScoreAccountingOutcome Apply(ScoreEvent e, Action? beforeLegacyWrite)
    {
        if (e.Raw == 0) return new(ScoreAccountingStatus.NoEvent, 0, 0, 0, CurrentCombo);
        if (!IsSupported) return new(ScoreAccountingStatus.Unsupported, 0, 0, 0, CurrentCombo);
        bool whole = e.Kind is ScoreEventKind.CircleWhole or ScoreEventKind.SliderAggregate or ScoreEventKind.SpinnerFinal;
        ScoreObjectKind expectedSource = e.Kind switch
        {
            ScoreEventKind.CircleWhole => ScoreObjectKind.Circle,
            ScoreEventKind.SpinnerProgress or ScoreEventKind.SpinnerFinal => ScoreObjectKind.Spinner,
            _ => ScoreObjectKind.Slider
        };
        bool validRaw = e.Kind switch
        {
            ScoreEventKind.CircleWhole or ScoreEventKind.SliderAggregate or ScoreEventKind.SpinnerFinal => e.Raw is 1024 or 512 or 256 or -131072,
            ScoreEventKind.SliderHead => e.Raw is 64 or -262144,
            ScoreEventKind.SliderInterior or ScoreEventKind.SliderRepeatOrTerminal => e.Raw is 8 or 64 or 128 or -262144 or -524288,
            ScoreEventKind.SpinnerProgress => e.Raw is 4096 or 8192 or 16384,
            _ => false
        };
        bool validRole = e.Kind switch
        {
            ScoreEventKind.SliderInterior => e.ProducerRole == SliderCheckpointKind.Interior,
            ScoreEventKind.SliderRepeatOrTerminal => e.ProducerRole is SliderCheckpointKind.Repeat or SliderCheckpointKind.Final,
            _ => e.ProducerRole is null
        };
        if (e.Policy != ScorePolicy.PlainLegacy || !validRaw || !validRole || e.Source.Kind != expectedSource ||
            e.ObjectHasSliderType != (expectedSource == ScoreObjectKind.Slider) || e.Source.ObjectId < 0 || e.Sequence < 0)
            return Unsupported("Unsupported policy, raw/decorated result or provenance/flags.");

        int b = e.Raw switch { 1024 => 300, 512 => 100, 256 => 50, 64 or 128 => 30, 8 => 10, 8192 => 100, 16384 => 1100, _ => 0 };
        bool scaled = whole && e.Raw > 0;
        int oldCombo = CurrentCombo;
        // E33 order: captured flag gates counters, then object-local base amount,
        // old-combo delta, combo/max/snapshot, and finally admitted legacy score.
        if (whole && e.AccountingEnabled)
        {
            switch (e.Raw)
            {
                case 1024: N300 = unchecked((ushort)(N300 + 1)); break;
                case 512: N100 = unchecked((ushort)(N100 + 1)); break;
                case 256: N50 = unchecked((ushort)(N50 + 1)); break;
                case -131072: NMiss = unchecked((ushort)(NMiss + 1)); break;
            }
        }
        LastBaseAmount = b;
        double bonus = scaled ? (double)Math.Max(0, unchecked(oldCombo - 1)) * (double)(b / 25) * Configuration.K : 0;
        // Outside this finite contract, earlier count/base mutations can remain.
        // Do not invent target-CLR overflow conversion output or saturate it.
        if (!double.IsFinite(bonus) || bonus < int.MinValue || bonus >= 2147483648.0)
            return Unsupported("Combo bonus is outside the supported finite Int32 conversion domain.");
        int delta = unchecked(b + (scaled ? (int)bonus : 0));
        bool comboEnabled = e.Kind != ScoreEventKind.SpinnerProgress && e.Raw != -524288 &&
            !(e.Kind == ScoreEventKind.SliderAggregate && e.Raw > 0);
        if (comboEnabled)
        {
            if (e.Raw > 0)
            {
                CurrentCombo = unchecked(CurrentCombo + 1);
                MaximumCombo = Math.Max(MaximumCombo, CurrentCombo);
                ComboSnapshot = unchecked((ushort)CurrentCombo);
            }
            else { CurrentCombo = 0; ComboSnapshot = 0; } // Includes parent whole MISS.
        }
        beforeLegacyWrite?.Invoke(); // E035 HP site after combo, before late record write; no arithmetic change.
        // E33 A: late admission is distinct from the captured counter flag.
        // Supported negative identities also carry this bit and admit a zero write.
        bool recordAdmitted = e.AccountingEnabled || (e.Raw & 8388608) > 0;
        int appliedDelta = recordAdmitted ? delta : 0;
        if (recordAdmitted) Score = unchecked(Score + delta);
        EventCount++;
        LastEvent = e;
        LastOutcome = new(recordAdmitted ? ScoreAccountingStatus.Applied : ScoreAccountingStatus.RecordSuppressed,
            b, delta, appliedDelta, oldCombo);
        return LastOutcome;

        ScoreAccountingOutcome Unsupported(string reason)
        {
            UnsupportedReason = reason;
            LastEvent = e;
            LastOutcome = new(ScoreAccountingStatus.Unsupported, 0, 0, 0, CurrentCombo);
            return LastOutcome; // ENGINEERING: preserve supported prefix; hide it as a final score.
        }
    }
}

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
    private readonly Dictionary<object, ScoreSource> scoreSources = new(ReferenceEqualityComparer.Instance);
    public ScoreLedger Ledger { get; }
    public ScoreConfiguration Scoring => Ledger.Configuration;
    public OrdinaryHealth? Health { get; }
    public string HealthStatus { get; }
    public HealthCalibrationResult? HealthPreparation => Health?.Calibration;
    private int previousUpdateTime, previousQualifyingInputTime;
    private bool hasQualifyingInputTime;
    public int ObjectCount => Circles.Count + Sliders.Count + Spinners.Count;
    public IEnumerable<Judgement> ObjectResults => Circles.Select(c => c.Result).Concat(Sliders.Select(s => s.Result)).Concat(Spinners.Select(s => s.Result));
    public int Approach { get; }
    public int Window300 { get; }
    public int Window100 { get; }
    public int Window50 { get; }
    public float DiameterAtScaleOne { get; }
    public float HitRadius { get; }
    public int Combo => Ledger.CurrentCombo;
    public int MaxCombo => Ledger.MaximumCombo;
    public int JudgedCount { get; private set; }
    public int HitValueTotal => Ledger.WeightedHitTotal; // Compatibility view, NOT legacy numeric score.
    public HitFeedback? Feedback { get; private set; }
    public bool Finished { get; private set; }
    public double Accuracy => Ledger.AccuracyPercent;

    // Historical component API uses the documented ENGINEERING preset. The UI
    // and scoring callers explicitly supply configuration through the overload.
    public CircleSession(Beatmap map) : this(map, ScoreConfiguration.EngineeringPractice) { }

    public CircleSession(Beatmap map, ScoreConfiguration scoring) : this(map, scoring, true) { }

    public CircleSession(Beatmap map, ScoreConfiguration scoring, bool enableHealth)
    {
        Map = map;
        Ledger = new ScoreLedger(scoring);
        Circles = map.Circles.Select(c => new CircleState(c)).ToArray();
        Sliders = map.Sliders.Select(s => new SliderState(s)).ToArray();
        Spinners = map.Spinners.Select(s => new SpinnerState(s, map.OverallDifficulty)).ToArray();
        targets = Circles.Select(c => (Head: c, Slider: (SliderState?)null))
            .Concat(Sliders.Select(s => (Head: s.Head, Slider: (SliderState?)s)))
            .OrderBy(t => t.Head.Definition.StartTime).ToList();
        Heads = targets.Select(t => t.Head).ToArray();
        var sources = Circles.Select(c => (State: (object)c, Kind: ScoreObjectKind.Circle, Start: c.Definition.StartTime, End: c.Definition.StartTime))
            .Concat(Sliders.Select(s => (State: (object)s, Kind: ScoreObjectKind.Slider, Start: s.Definition.Head.StartTime, End: s.Definition.EndTime)))
            .Concat(Spinners.Select(s => (State: (object)s, Kind: ScoreObjectKind.Spinner, Start: s.Definition.StartTime, End: s.Definition.EndTime)))
            .OrderBy(s => s.Start).ToArray();
        for (int i = 0; i < sources.Length; i++)
            scoreSources.Add(sources[i].State, new(i, sources[i].Kind, sources[i].Start, sources[i].End));
        // E07 M28be/M28b3: no-mod finite approach mapping, truncation toward zero.
        Approach = MapDifficulty(map.ApproachRate, 1800, 1200, 450);
        // E12 M28be widths, read in 300/100/50 classification order below.
        Window300 = MapDifficulty(map.OverallDifficulty, 80, 50, 20);
        Window100 = MapDifficulty(map.OverallDifficulty, 140, 100, 60);
        Window50 = MapDifficulty(map.OverallDifficulty, 200, 150, 100);
        // E13 M28be at a supplied scale-one playfield W=512, s=1.
        DiameterAtScaleOne = (float)(64.0 * (1 - 0.699999988079071 * ((map.CircleSize - 5.0) / 5.0)));
        HitRadius = (DiameterAtScaleOne / 2f) * 1.0004099607467651f;
        if (enableHealth && scoring.Policy == ScorePolicy.PlainLegacy)
        {
            try
            {
                HealthChart chart = HealthChart.FromMap(map, Approach);
                Health = new(chart, new HealthCalibration().Run(chart), fillOffset: 0);
                HealthStatus = "E035 calibrated · bounded host · fill offset0 ENGINEERING";
            }
            catch (System.IO.InvalidDataException error) { HealthStatus = error.Message; }
        }
        else HealthStatus = "Health unavailable: explicitly disabled or unsupported score policy.";
    }

    private static int MapDifficulty(double d, double a, double b, double c) =>
        (int)(d > 5 ? b + (c - b) * (d - 5) / 5 : d < 5 ? b - (b - a) * (5 - d) / 5 : b);

    public void SetHeldButtons(int buttons) => input.Publish(buttons);

    // Existing headless/circle API: no supplied live input means no held tracking.
    public void Advance(int time) => Advance(time, new Vector2(float.NaN, float.NaN), 0);

    public void Advance(int time, Vector2 pointer, int buttons, Vector2? spinnerPointer = null, HealthHostTiming? healthTiming = null)
    {
        if (Finished) return;
        input.Publish(buttons);
        // ENGINEERING current-object substitute: capture before object evaluation.
        // The original uses its old candidate selection, not this interval proxy.
        bool healthSpinner = Spinners.Any(s => s.Result == Judgement.Pending && time >= s.Definition.StartTime && time <= s.Definition.EndTime);
        int healthPreviousTime = previousUpdateTime;
        int updateInterval = unchecked(time - previousUpdateTime);
        double sampleInterval = previousQualifyingInputTime > 0 ? (double)unchecked(time - previousQualifyingInputTime) : SpinnerState.InitialInterval;
        // E24 has a nominal 16.666... timing-tail qualification gate. This UI
        // approximation prevents applying its nominal divisor to every high-Hz
        // rendering sample; it is not the original marker/record/provider scheduler.
        bool qualifyInput = !hasQualifyingInputTime || unchecked(time - previousQualifyingInputTime) >= SpinnerState.InitialInterval;
        Vector2 spinPointer = spinnerPointer ?? pointer;
        foreach (SpinnerState spinner in Spinners)
        {
            if (spinner.Result != Judgement.Pending || time < spinner.Definition.StartTime) continue;
            if (qualifyInput && float.IsFinite(spinPointer.X) && float.IsFinite(spinPointer.Y))
            {
                int raw = spinner.SampleCursor(spinPointer, SpinnerState.DisplayCenter, time, sampleInterval, (buttons & 3) != 0);
                // Only this actual observation returns an event; never replay LastComponentRaw on other frames.
                Emit(spinner, ScoreEventKind.SpinnerProgress, raw, time, spinner.Observation.Count);
            }
            spinner.Integrate(time, updateInterval);
            if (spinner.Complete(time))
            {
                RecordWholeResult(spinner, ScoreEventKind.SpinnerFinal, spinner.Result, time, spinner.FinalRaw);
                Feedback = new(SpinnerState.DisplayCenter, "SPINNER " + Label(spinner.Result), time, spinner.Result);
            }
        }
        previousUpdateTime = time;
        if (qualifyInput)
        {
            previousQualifyingInputTime = time; // Global history, including passes before spinner activation.
            hasQualifyingInputTime = true;
        }
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
                RecordWholeResult(slider, ScoreEventKind.SliderAggregate, slider.Result, time);
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
        if (Health is { } hp)
        {
            // Candidate proxy is the stable prepared list (including evaluated objects),
            // NOT a claim to reproduce F17fd attention. Break/queue arithmetic is separate.
            var eligibility = new HealthEligibility(time, healthPreviousTime, hp.Chart.Objects[0].Start,
                hp.Chart.Objects[^1].End, Approach, hp.Marker, 1, hp.Chart.Format, hp.QueueHead);
            double hostElapsed = healthTiming?.ElapsedMilliseconds ?? updateInterval;
            double hostScale = healthTiming?.Scale ?? updateInterval / (1000.0 / 60.0);
            hp.FrameWithPreparedCandidates(new(time, healthPreviousTime, updateInterval, hostElapsed, hostScale, 1,
                ClearFill: true, SelectedSpinner: healthSpinner, Paused: false, eligibility));
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
        RecordWholeResult(circle, ScoreEventKind.CircleWhole, circle.Result, time);
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
        Emit(slider, ScoreEventKind.SliderHead, Positive(slider.Head.Result) ? 64 : -262144, time);
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
        int raw = 0, k = 0, count = slider.Checkpoints.Count, sequence = -1;
        SliderCheckpointState? processed = null;
        while (k < count && slider.Checkpoints[k].Definition.Time <= time) k++;
        if (slider.ProcessedCheckpoints < k)
        {
            sequence = slider.ProcessedCheckpoints;
            var checkpoint = slider.Checkpoints[sequence];
            processed = checkpoint;
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
        // Leave precedes E33 accounting. Role is producer metadata; actual raw
        // still comes from the unchanged due-prefix selector above.
        if (processed is not null)
            Emit(slider, processed.Definition.Kind == SliderCheckpointKind.Interior ? ScoreEventKind.SliderInterior :
                ScoreEventKind.SliderRepeatOrTerminal, raw, time, sequence, processed.Definition.Kind);
    }

    private void RecordWholeResult(object source, ScoreEventKind kind, Judgement result, int time, int? raw = null)
    {
        JudgedCount++;
        int code = raw ?? (result switch { Judgement.Hit300 => 1024, Judgement.Hit100 => 512, Judgement.Hit50 => 256, _ => -131072 });
        Emit(source, kind, code, time);
    }

    private void Emit(object source, ScoreEventKind kind, int raw, int time, int sequence = 0, SliderCheckpointKind? role = null)
    {
        if (raw == 0) return; // E33: not an accounting event (C1 can still advance observation state).
        ScoreSource identity = scoreSources[source];
        var e = new ScoreEvent(identity, sequence, kind, raw, AccountingEnabled: true,
            ObjectHasSliderType: identity.Kind == ScoreObjectKind.Slider, Policy: Scoring.Policy, ProducerRole: role);
        bool hpReached = false;
        Ledger.Apply(e, Health is null ? null : () => { Health.ApplyResultHp(raw, clearFill: true); hpReached = true; });
        // E035/E019: after HP mutation AND record continuation, only on a reached result.
        // No-mod app supplies no suppression/recoveries; latch is diagnostic, no invented departure.
        if (hpReached) Health!.CheckFailure(time);
    }
}
// E035 finite ordinary health components. Independent of WPF and score arithmetic.
// Kept in this historically linked source so Work1 QA projects stay untouched.
public sealed class HealthState
{
    public double H { get; private set; }
    public double V { get; private set; }
    public double A { get; private set; }
    public double Rate { get; }
    public double B { get; }
    public double G { get; }
    public bool Fill { get; private set; } = true;
    public int FillFrom { get; private set; }
    public double FillRate { get; private set; } = .02;

    public HealthState(double rate = 0, double b = 1, double g = 1) { Rate = rate; B = b; G = g; }
    public void Reset() { H = 200; A = 200; V = 0; } // M297d: no other resets.
    public void Assign(double value) { H = Math.Max(0, Math.Min(200, value)); A = value; }
    public void SetPresentation(double value) => V = value;
    public void ConfigureFill(int from, double slope, bool enabled) { FillFrom = from; FillRate = slope; Fill = enabled; }
    public void Subtract(double amount, bool clearFill = false)
    {
        if (clearFill && Fill) Fill = false;
        A = Math.Max(0, A - amount);
        H = Math.Max(0, H - amount); // Deliberately NO upper clamp for negative subtraction.
    }
    public void Add(double amount, bool clearFill = false)
    {
        if (clearFill && Fill) Fill = false;
        A += amount;
        H = Math.Max(0, Math.Min(200, H + amount));
    }
    public static double Phi(double hp, double a, double b, double c) => hp > 5 ?
        b + (c - b) * (hp - 5) / 5 : hp < 5 ? b - (b - a) * (5 - hp) / 5 : b;
    public double PlainDelta(int raw, double hp) => raw switch
    {
        1024 => 6 * G, 512 => G * Phi(hp, 17.6, 2.2, 2.2), 256 => G * Phi(hp, 3.2, .4, .4),
        -131072 => Phi(hp, -6, -25, -40), 64 or 128 => 4 * G, 8 => 3 * G,
        -262144 or -524288 => Phi(hp, -4, -15, -28), 4096 or 8192 => 1.7 * G,
        16384 => 2 * G, 0 => 0,
        _ => throw new System.IO.InvalidDataException("Unsupported plain health result; decorations/mods are not inferred.")
    };
    public bool ApplyPlain(int raw, double hp, bool clearFill = false)
    {
        if (raw == 0) return false;
        ApplyDelta(PlainDelta(raw, hp), clearFill);
        return true;
    }
    public void ApplyCalibration(int raw, double hp)
    {
        // Separate sink: this explicit trial1028 never enters the live ledger.
        double delta = raw == 1028 ? 14 * B + 6 * G : PlainDelta(raw, hp);
        ApplyDelta(delta, false);
    }
    private void ApplyDelta(double delta, bool clearFill)
    {
        if (delta > 0) Add(delta, clearFill); else Subtract(-delta, clearFill);
    }
    public void Smooth(int time, double correctedDelta, double hostElapsed, double hostScale, int clockState, bool recovering)
    {
        if (V < H)
        {
            if (Fill)
            {
                if (FillFrom < time && (recovering || clockState == 1))
                    V = Math.Min(200, V + FillRate * (recovering ? hostElapsed : correctedDelta));
            }
            else V = Math.Min(200, V + Math.Abs(H - V) / 4 * hostScale);
        }
        else if (V > H)
        {
            Fill = false;
            V = Math.Max(0, V - Math.Abs(V - H) / 6 * hostScale);
        }
    }
}

public sealed record PreparedHealthObject(int Start, int End, ScoreObjectKind Kind, bool? NewCombo,
    int RepeatSubsetCount = 0, int ScheduleCount = 0, int SpinnerRequirement = 0);
public sealed record HealthChart(double Hp, int Approach, int Format, IReadOnlyList<PreparedHealthObject> Objects,
    IReadOnlyList<HealthBreak> Breaks)
{
    public static HealthChart FromMap(Beatmap map, int approach)
    {
        HealthMapSource source = map.HealthSource ?? throw Unsupported("Missing fresh parser identity/group/break metadata.");
        LegacyScoreMetadata metadata = map.ScoreMetadata ?? throw Unsupported("Missing fresh HP metadata.");
        int count = map.Circles.Count + map.Sliders.Count + map.Spinners.Count;
        if (source.Rows.Count != count || metadata.RowCount != count) throw Unsupported("Prepared count differs from raw source; no inferred identities.");
        var objects = new List<PreparedHealthObject>();
        foreach (HealthObjectSource row in source.Rows)
        {
            ScoreObjectKind kind = (row.RawType & 8) != 0 ? ScoreObjectKind.Spinner :
                (row.RawType & 2) != 0 ? ScoreObjectKind.Slider : ScoreObjectKind.Circle;
            // E04 M2902 OR file bit: false circle/slider file bits do NOT prove false.
            bool? combo = (row.RawType & 4) != 0 || (kind == ScoreObjectKind.Spinner && source.Version <= 8) ? true :
                kind == ScoreObjectKind.Spinner ? false : null;
            if (kind == ScoreObjectKind.Circle)
            {
                if (!map.Circles.Any(c => c.StartTime == row.Start)) throw Unsupported("Circle identity missing.");
                objects.Add(new(row.Start, row.Start, kind, combo));
            }
            else if (kind == ScoreObjectKind.Slider)
            {
                SliderDefinition s = map.Sliders.Single(s => s.Head.StartTime == row.Start);
                int n = s.Checkpoints.Count, h = n / s.Traversals, subset = 0;
                for (int i = 0; i < n - 1 && h > 0; i++) if ((i + 1) % h == 0) subset++; // E21 F0bf7, not producer labels.
                objects.Add(new(row.Start, s.EndTime, kind, combo, subset, n));
            }
            else
            {
                SpinnerDefinition s = map.Spinners.Single(s => s.StartTime == row.Start);
                objects.Add(new(s.StartTime, s.EndTime, kind, combo, SpinnerRequirement: new SpinnerState(s, map.OverallDifficulty).Required));
            }
        }
        var chart = new HealthChart(metadata.Hp, approach, source.Version, objects.AsReadOnly(), source.Breaks);
        chart.Validate();
        return chart;
    }
    public void Validate()
    {
        if (Objects.Count == 0 || Objects.Count > 10000 || Breaks.Count > 4096 || Format < 5 || Format > 14 || !double.IsFinite(Hp) || Hp < 0 || Hp > 10 || Approach < 0)
            throw Unsupported("Health calibration needs nonempty finite prepared inputs.");
        for (int i = 0; i < Objects.Count; i++)
        {
            var o = Objects[i];
            if (o.End < o.Start || (i > 0 && o.Start <= Objects[i - 1].End))
                throw Unsupported("R09 needs ordered nonoverlapping prepared objects; no sorting/repair.");
            if (i > 0 && o.NewCombo is null) throw Unsupported("Original M2902 NewCombo input is unresolved for a following circle/slider without explicit bit4.");
            if (o.RepeatSubsetCount < 0 || o.ScheduleCount < o.RepeatSubsetCount + (o.Kind == ScoreObjectKind.Slider ? 1 : 0) || o.SpinnerRequirement < 0)
                throw Unsupported("Inconsistent prepared component counts.");
        }
        int previousBreakEnd = int.MinValue;
        foreach (var b in Breaks)
        {
            if (b.End <= b.Start || b.Start < previousBreakEnd ||
                !Enumerable.Range(1, Objects.Count - 1).Any(i => b.Start >= Objects[i - 1].End && b.End <= Objects[i].Start))
                throw Unsupported("R09 supports ordered positive breaks contained in prepared inter-object gaps only.");
            previousBreakEnd = b.End;
        }
    }
    private static System.IO.InvalidDataException Unsupported(string reason) => new("Health unavailable: " + reason);
}

public enum HealthRetry { None, PreObject, DurationOverflow, ThirdLowGroup, FinalTarget, Balance }
public sealed record HealthCalibrationResult(double Rate, double B, double G, int Trials,
    IReadOnlyList<double> Snapshots, int PreRetries, int GroupRetries, int EndRetries, int BalanceRetries);
public sealed class HealthCalibration
{
    public const int MaxTrials = 10000, MaxWork = 2000000; // ENGINEERING safety, NOT original limits.
    public double Rate { get; private set; } = .05;
    public double B { get; private set; } = 1;
    public double G { get; private set; } = 1;
    public int Trials { get; private set; }
    public double LastH { get; private set; }
    public double LastA { get; private set; }
    public double LastMean { get; private set; }
    public HealthRetry LastRetry { get; private set; }
    public double LastGapLoss { get; private set; }
    public int LastExcludedGap { get; private set; }
    public int LastBreaksConsumed { get; private set; }
    public IReadOnlyList<double> Snapshots => Array.AsReadOnly(snapshots);
    private double[] snapshots = [];
    private HealthChart? activeChart;
    private int work, preRetries, groupRetries, endRetries, balanceRetries;
    public HealthRetry Trial(HealthChart chart)
    {
        chart.Validate();
        if (activeChart is not null && !ReferenceEquals(activeChart, chart)) throw new System.IO.InvalidDataException("Reuse calibration only for the same prepared chart.");
        if (activeChart is null) { activeChart = chart; snapshots = Enumerable.Repeat(double.NaN, chart.Objects.Count).ToArray(); }
        Trials++;
        var state = new HealthState(Rate, B, G); state.Reset();
        double l = HealthState.Phi(chart.Hp, 195, 160, 60), u = HealthState.Phi(chart.Hp, 198, 170, 80);
        double e = HealthState.Phi(chart.Hp, 198, 180, 80), w = HealthState.Phi(chart.Hp, 8, 4, 0);
        int previousEnd = unchecked(chart.Objects[0].Start - chart.Approach), breakIndex = 0, low = 0;
        HealthRetry retry = HealthRetry.None;
        for (int i = 0; i < chart.Objects.Count; i++)
        {
            Work(); var o = chart.Objects[i]; int excluded = 0;
            if (breakIndex < chart.Breaks.Count)
            {
                var b = chart.Breaks[breakIndex];
                if (b.Start >= previousEnd && b.End <= o.Start)
                {
                    excluded = chart.Format >= 8 ? unchecked(b.End - previousEnd) : unchecked(b.End - b.Start);
                    breakIndex++; // At most ONE, including multiple breaks in the gap.
                }
            }
            LastExcludedGap = excluded; LastBreaksConsumed = breakIndex;
            LastGapLoss = Rate * (double)unchecked(unchecked(o.Start - previousEnd) - excluded);
            state.Subtract(LastGapLoss);
            previousEnd = o.End;
            if (state.H <= l) { Rate *= .96; preRetries++; retry = HealthRetry.PreObject; break; }
            double d = Rate * (double)unchecked(o.End - o.Start), overflow = Math.Max(0, d - state.H);
            state.Subtract(d);
            if (o.Kind == ScoreObjectKind.Slider)
            {
                for (int k = 0; k < o.RepeatSubsetCount + 2; k++) { Work(); state.ApplyCalibration(64, chart.Hp); }
                for (int k = 0; k < o.ScheduleCount - o.RepeatSubsetCount - 1; k++) { Work(); state.ApplyCalibration(8, chart.Hp); }
            }
            else if (o.Kind == ScoreObjectKind.Spinner)
                for (int k = 0; k < o.SpinnerRequirement; k++) { Work(); state.ApplyCalibration(8192, chart.Hp); }
            if (overflow > 0 && state.H - overflow <= l) { Rate *= .96; preRetries++; retry = HealthRetry.DurationOverflow; break; }
            bool boundary = i == chart.Objects.Count - 1 || chart.Objects[i + 1].NewCombo == true;
            state.ApplyCalibration(boundary ? 1028 : 1024, chart.Hp);
            if (boundary && state.H < u && ++low > 2)
            {
                B *= 1.07; G *= 1.03; groupRetries++; retry = HealthRetry.ThirdLowGroup; break;
            }
            snapshots[i] = state.H; // Preserve old untouched suffix across retries, like F1529.
        }
        if (retry == HealthRetry.None && state.H < e)
        {
            Rate *= .94; B *= 1.01; G *= 1.01; endRetries++; retry = HealthRetry.FinalTarget;
        }
        LastMean = (state.A - 200) / (double)chart.Objects.Count; // Still computed on retry.
        if (retry == HealthRetry.None && LastMean < w)
        {
            Rate *= .96; B *= 1.02; G *= 1.01; balanceRetries++; retry = HealthRetry.Balance;
        }
        LastH = state.H; LastA = state.A; LastRetry = retry;
        if (!double.IsFinite(Rate) || !double.IsFinite(B) || !double.IsFinite(G) || Rate <= 0)
            throw new System.IO.InvalidDataException("Health calibration left finite supported domain.");
        return retry;
    }
    public HealthCalibrationResult Run(HealthChart chart, int trialLimit = MaxTrials)
    {
        for (int n = 0; n < trialLimit; n++)
            if (Trial(chart) == HealthRetry.None)
                return new(Rate, B, G, Trials, Array.AsReadOnly((double[])snapshots.Clone()), preRetries, groupRetries, endRetries, balanceRetries);
        throw new System.IO.InvalidDataException("Health calibration exceeded ENGINEERING trial guard; no fallback rate/gains.");
    }
    private void Work()
    {
        if (++work > MaxWork) throw new System.IO.InvalidDataException("Health calibration exceeded ENGINEERING work guard; no fallback.");
    }
}

public readonly record struct HealthEligibility(int Time, int PreviousTime, int FirstStart, int LastEnd, int Approach,
    int Marker, int ClockState, int Format, int? QueueHead = null, bool Special = false, int SpecialUntil = 0,
    bool Recorded = false, int RecordedUntil = 0)
{
    public bool Evaluate()
    {
        if (Special && PreviousTime <= SpecialUntil) return false;
        if (Recorded && RecordedUntil >= PreviousTime) return false;
        if (Time < unchecked(FirstStart - Approach)) return false;
        if (Time > LastEnd) return false;
        if (Marker >= 0 || ClockState == 0) return false;
        return QueueHead is null || Time <= QueueHead.Value || Format < 8;
    }
}
public enum HealthFailure { NotChecked, Healthy, Suppressed, Recovery, Latched, AlreadyLatched }
public readonly record struct HealthFailureGates(bool NoFail = false, bool Relax = false, bool Relax2 = false,
    bool Recorded = false, bool RecordedContinuation = false, bool Special = false)
{
    public bool Suppress => NoFail || Relax || Relax2 || (Recorded && !RecordedContinuation) || Special;
}
public readonly record struct HealthFrame(int Time, int PreviousTime, double CorrectedDelta, double HostElapsed,
    double HostScale, int ClockState, bool ClearFill, bool SelectedSpinner, bool Paused, HealthEligibility LateEligibility);
public readonly record struct HealthHostTiming(double ElapsedMilliseconds, double Scale);

public sealed class OrdinaryHealth
{
    public HealthState State { get; }
    public HealthCalibrationResult Calibration { get; }
    public HealthChart Chart { get; }
    public bool Eligible { get; private set; }
    public bool Failed { get; private set; }
    public int? FailureTime { get; private set; }
    public bool Recovering { get; private set; }
    public int RecoveryCount { get; private set; }
    public HealthFailure LastFailure { get; private set; }
    public int ResultEvents { get; private set; }
    public double LastPassiveLoss { get; private set; }
    public int Marker { get; private set; } = -1;
    public int? QueueHead => queue.Count == 0 ? null : queue.Peek();
    private readonly Queue<int> queue = new();
    public OrdinaryHealth(HealthChart chart, HealthCalibrationResult calibration, int fillOffset = 0)
    {
        Chart = chart; Calibration = calibration;
        State = new(calibration.Rate, calibration.B, calibration.G);
        int first = chart.Objects[0].Start, from = fillOffset > 0 ? -fillOffset : 0;
        double slope = .02;
        if (first * slope < 200)
        {
            int denominator = unchecked(first + fillOffset);
            if (denominator <= 0) throw new System.IO.InvalidDataException("Unsupported finite initial fill operand; F0fc4 must be supplied explicitly.");
            slope = 200 / (double)denominator;
        }
        else if (first > 10000) from = unchecked(first - 10000);
        State.ConfigureFill(from, slope, true); State.Reset(); // Reset retains prepared coefficients/fill.
        int[] ends = chart.Objects.Select(o => o.End).ToArray();
        foreach (var b in chart.Breaks)
        {
            int midpoint = unchecked(b.Start + unchecked(b.End - b.Start) / 2);
            int index = Array.BinarySearch(ends, midpoint); if (index < 0) index = ~index;
            index--;
            if (index < 0) throw new System.IO.InvalidDataException("Unsupported original break predecessor index; no repair.");
            queue.Enqueue(ends[index]);
        }
    }
    public void ApplyResultHp(int raw, bool clearFill)
    {
        if (State.ApplyPlain(raw, Chart.Hp, clearFill)) ResultEvents++;
    }
    public HealthFailure CheckFailure(int time, HealthFailureGates gates = default)
    {
        if (State.H > 0 || double.IsNaN(State.H)) return LastFailure = HealthFailure.Healthy;
        if (gates.Suppress) return LastFailure = HealthFailure.Suppressed;
        if (RecoveryCount > 0) { BeginRecovery(); return LastFailure = HealthFailure.Recovery; }
        if (Failed) return LastFailure = HealthFailure.AlreadyLatched;
        // Supported state boundary only: original candidate cleanup/departure is NOT emulated.
        Failed = true; FailureTime = time;
        return LastFailure = HealthFailure.Latched;
    }
    public void SupplyRecoveryCount(int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        RecoveryCount = count; // Explicit component input; ordinary no-mod app uses zero.
    }
    public void BeginRecovery()
    {
        if (RecoveryCount <= 0) throw new InvalidOperationException("No supplied recovery count.");
        if (State.H < 160) State.Add(160, true);
        State.ConfigureFill(State.FillFrom, .07999999821186066, true);
        Recovering = true; RecoveryCount--;
        // Native playback toggle is an unresolved boundary, not a clock rewrite here.
    }
    public void Frame(HealthFrame frame)
    {
        if (frame.Paused) return;
        AdvanceState(frame);
        Eligible = frame.LateEligibility.Evaluate();
    }
    public void FrameWithPreparedCandidates(HealthFrame frame)
    {
        if (frame.Paused) return;
        AdvanceState(frame);
        UpdateBreakState(frame.Time, Chart.Objects.Select(o => o.Start));
        Eligible = (frame.LateEligibility with { Marker = Marker, QueueHead = QueueHead }).Evaluate();
    }
    private void AdvanceState(HealthFrame frame)
    {
        LastPassiveLoss = 0;
        if (Eligible)
        {
            LastPassiveLoss = (State.Rate / 1.0) * (double)unchecked(frame.Time - frame.PreviousTime);
            LastPassiveLoss *= frame.SelectedSpinner ? (double).25f : (double)1f;
            State.Subtract(LastPassiveLoss, frame.ClearFill);
        }
        State.Smooth(frame.Time, frame.CorrectedDelta, frame.HostElapsed, frame.HostScale, frame.ClockState, Recovering);
        if (Recovering && State.V > State.H - 5) Recovering = false;
        // No passive-drain failure callback. Only reached result completion invokes it.
    }
    public void UpdateBreakState(int time, IEnumerable<int> candidateStarts)
    {
        bool active = Chart.Breaks.Any(b => b.Start <= time && time <= b.End);
        if (active)
        {
            Marker = time;
            // E27 supplies the guard omitted from a standalone queue shorthand: only while active.
            while (queue.Count > 0 && queue.Peek() < time) queue.Dequeue();
        }
        if (Marker >= 0 && candidateStarts.Any(s => s <= time && s >= Marker)) Marker = -1;
    }
}
