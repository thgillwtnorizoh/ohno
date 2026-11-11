using System.Globalization;

namespace Ohno.CircleGame.Gameplay;

// E042 enum identities. N is a sentinel, never a calculated ordinary grade.
public enum RankGrade { XH = 0, SH = 1, X = 2, S = 3, A = 4, B = 5, C = 6, D = 7, F = 8, N = 9 }
public interface IRankGradeRecord
{
    ushort N300 { get; }
    ushort N100 { get; }
    ushort N50 { get; }
    ushort NMiss { get; }
    int WholeTotal { get; }
    bool Success { get; }
    int Modifiers { get; }
}
public static class OrdinaryRankGrade
{
    // Stored binary32 quotients, then binary64 comparisons; original x86 JIT margins unverified.
    public static RankGrade Calculate(IRankGradeRecord record)
    {
        float p300 = (float)record.N300 / (float)record.WholeTotal;
        float p50 = (float)record.N50 / (float)record.WholeTotal; // Independent virtual total read.
        if (!record.Success) return RankGrade.F;
        if (p300 == 1f) return Silver(record) ? RankGrade.XH : RankGrade.X;
        if ((double)p300 > .9 && (double)p50 <= .01 && record.NMiss == 0)
            return Silver(record) ? RankGrade.SH : RankGrade.S;
        if (((double)p300 > .8 && record.NMiss == 0) || (double)p300 > .9) return RankGrade.A;
        if (((double)p300 > .7 && record.NMiss == 0) || (double)p300 > .8) return RankGrade.B;
        return (double)p300 > .6 ? RankGrade.C : RankGrade.D;
    }
    private static bool Silver(IRankGradeRecord r) => (r.Modifiers & 8) > 0 || (r.Modifiers & 1024) > 0;
}
public sealed record RankMapMetadata(string Title, string? Difficulty, string? Artist);

// Mutable original-derived flags around a live record; grade is never stored here.
public abstract class RankRecord : IRankGradeRecord
{
    public object SessionIdentity { get; } = new();
    public abstract object? MapIdentity { get; }
    public abstract RankMapMetadata? Metadata { get; }
    public abstract int Score { get; }
    public abstract ushort N300 { get; }
    public abstract ushort N100 { get; }
    public abstract ushort N50 { get; }
    public abstract ushort NMiss { get; }
    public abstract int MaximumCombo { get; }
    public virtual int Modifiers => 0;
    public virtual int WholeTotal => unchecked(N50 + N100 + N300 + NMiss);
    public virtual float AccuracyFraction => WholeTotal <= 0 ? 1f :
        (float)unchecked(50 * N50 + 100 * N100 + 300 * N300) / (float)unchecked(300 * WholeTotal);
    public bool Success { get; set; }
    public bool Valid { get; set; } = true;
    public bool? Perfect { get; set; }
    public int? PreparedCombo { get; protected set; }
    public virtual string? UnsupportedReason => null;
    public RankGrade Grade() => OrdinaryRankGrade.Calculate(this);
    public void Complete(bool failed, int expectedWholeCount)
    {
        Success = !failed; // Never inferred from accuracy, validity or final judgement.
        if (WholeTotal != expectedWholeCount) Valid = false; // M2b5a does not rewrite score/veto completion.
        Perfect = PreparedCombo is { } prepared ? !(unchecked(prepared - MaximumCombo) > 0) : null;
    }
}
// Explicit inputs to the scoped ordinary factory, not inferred from a no-mod label.
public sealed record RankPolicyInputs(int Ruleset = 0, int Modifiers = 0, bool FreshCreation = true,
    bool RecordedInput = false, bool SourceRecord = false, bool Restored = false,
    bool AlternateRecord = false, bool Accumulator = false);
public static class RankAdmission
{
    public static string? Check(Beatmap map, ScoreLedger ledger, RankPolicyInputs policy)
    {
        if (policy.Ruleset != 0 || policy.Modifiers != 0 || !policy.FreshCreation || policy.RecordedInput ||
            policy.SourceRecord || policy.Restored || policy.AlternateRecord || policy.Accumulator)
            return "Rank supports fresh no-mod ordinary base records only; replay/restore/alternate policies unsupported.";
        if (map.HealthSource is null || !map.HealthSource.IsFreshFor(map) || map.ScoreMetadata is null)
            return "Rank requires exact fresh parsed map and score provenance.";
        if (map.ScoreMetadata.RowCount >= 7000)
            return "E023 automatic ScoreV2 boundary: 7000 or more source rows; base legacy Rank unsupported.";
        if (!ledger.IsSupported || ledger.Configuration.Policy != ScorePolicy.PlainLegacy)
            return ledger.UnsupportedReason ?? "Unsupported numeric record policy.";
        return null;
    }
    public static int PreparedCombo(HealthChart chart)
    {
        chart.Validate();
        int count = 0;
        foreach (var o in chart.Objects)
        {
            if (o.Kind == ScoreObjectKind.Slider)
            {
                int a = unchecked(o.RepeatSubsetCount + 2);
                int b = unchecked(o.ScheduleCount - o.RepeatSubsetCount - 1);
                count = unchecked(count + unchecked(a + b - 1)); // M1150 trial scalar, not live scoring.
            }
            count = unchecked(count + 1);
        }
        return count;
    }
}
public sealed class SessionRankRecord : RankRecord
{
    public CircleSession Session { get; }
    private readonly RankPolicyInputs policy;
    private readonly string? admission;
    public SessionRankRecord(CircleSession session, RankPolicyInputs? policy = null)
    {
        Session = session; this.policy = policy ?? new();
        admission = RankAdmission.Check(session.Map, session.Ledger, this.policy);
        if (session.HealthPolicy == HealthPreparationPolicy.E036FreshOrdinary && session.Health is { } hp)
            PreparedCombo = RankAdmission.PreparedCombo(hp.Chart); // Completed E035 calibration and R10 provenance.
    }
    public override object MapIdentity => Session.Map;
    public override RankMapMetadata Metadata => new(Session.Map.Title,
        Session.Map.DisplayMetadata?.Difficulty, Session.Map.DisplayMetadata?.Artist);
    public override int Score => Session.Ledger.Score;
    public override ushort N300 => Session.Ledger.N300;
    public override ushort N100 => Session.Ledger.N100;
    public override ushort N50 => Session.Ledger.N50;
    public override ushort NMiss => Session.Ledger.NMiss;
    public override int MaximumCombo => Session.Ledger.MaximumCombo;
    public override float AccuracyFraction => Session.Ledger.AccuracyFraction;
    public override int Modifiers => policy.Modifiers;
    public override string? UnsupportedReason => admission ?? Session.Ledger.UnsupportedReason;
}
public enum RankMode { Play = 2, SelectPlay = 5, Rank = 7 }
public enum TransitionDecision { Accepted, Duplicate, Rejected, Deferred }
// ENGINEERING bounded subset of E003: no opaque helpers, original fade manager or worker scheduling.
public sealed class RankTransitions
{
    public RankMode Current { get; private set; } = RankMode.Play;
    public RankMode? Pending { get; private set; }
    public int Phase { get; private set; } = 2;
    public bool GateOpen { get; set; } = true;
    public bool Allow { get; set; } = true;
    public bool RestrictRankRetry { get; set; } // E003 conditional F2c48 gate; host false.
    public double RetryOperand { get; set; } = 10000;
    public TransitionDecision Request(RankMode target)
    {
        if (!Enum.IsDefined(target)) return TransitionDecision.Rejected;
        if (!GateOpen) return TransitionDecision.Deferred; // Caller must resubmit through fresh guards.
        if (!Allow || (RestrictRankRetry && Current == RankMode.Rank && target == RankMode.Play && RetryOperand < 10000))
            return TransitionDecision.Rejected;
        if (target == Current || target == Pending) return TransitionDecision.Duplicate;
        Pending = target; Phase = 0; return TransitionDecision.Accepted;
    }
    public bool Commit()
    {
        if (Pending is not { } target || Current == target) return false;
        Current = target; return true; // Pending survives until initialization completes.
    }
    public void Initialized()
    { if (Pending == Current) Pending = null; Phase = 1; }
}
public sealed record RankGradeResources(RankGrade First, string FirstKey, RankGrade Eligibility, string? SecondaryKey)
{
    public static RankGradeResources Read(Func<RankRecord> selected, Action<int>? afterRead = null)
    {
        RankGrade first = selected().Grade(); afterRead?.Invoke(1);
        RankGrade later = selected().Grade(); afterRead?.Invoke(2);
        string? secondary = (int)later < 6 ? "ranking-" + selected().Grade() : null;
        if (secondary is not null) afterRead?.Invoke(3);
        return new(first, "ranking-" + first, later, secondary);
    }
}
// ENGINEERING immutable text capture at Initialize, while SelectedRecord remains mutable/live.
public sealed record RankPresentation(object RecordIdentity, RankMapMetadata Map, string Score, string Accuracy,
    ushort N300, ushort N100, ushort N50, ushort NMiss, int MaximumCombo, bool Success, bool Valid,
    bool? Perfect, RankGradeResources Grade, string Provenance)
{
    public static RankPresentation Capture(RankRecord record, Action<int>? afterGradeRead = null, int? scoreOverride = null)
    {
        if (record.UnsupportedReason is { } reason) throw new InvalidOperationException(reason);
        var map = record.Metadata ?? throw new InvalidOperationException("Rank map metadata unavailable.");
        string score = (scoreOverride ?? record.Score).ToString("00000000", CultureInfo.InvariantCulture);
        string accuracy = (record.AccuracyFraction * 100f).ToString("0.00", CultureInfo.InvariantCulture) + "%";
        return new(record.SessionIdentity, map, score, accuracy, record.N300, record.N100, record.N50, record.NMiss,
            record.MaximumCombo, record.Success, record.Valid, record.Perfect,
            RankGradeResources.Read(() => record, afterGradeRead),
            "E042 base legacy · invariant formatting · four whole-result counters · engineering typography/capture; player/date/decorated counts unavailable");
    }
}
public sealed class RankScoreReveal(string capturedScore, int hostTime)
{
    public string CapturedScore { get; } = capturedScore;
    public int Start { get; private set; } = unchecked(hostTime + 300);
    public bool Skipped { get; private set; }
    public string Text(int now)
    {
        int prefix = unchecked(now - Start) / 500;
        return new string(CapturedScore.Select((c, i) => i < prefix ? c : '·').ToArray()); // Deterministic replacement, not M2333 random.
    }
    public bool Skip(int now)
    {
        if (unchecked(now - Start) < 5000 && !Skipped)
        { Skipped = true; Start = unchecked(now - 7000); return false; }
        return true;
    }
}
public enum RankStage { Active, FinalizationEligible, RecordCompleted, RankRequested, RankAdmitted,
    TransitionCommitted, ResultsInitialized, ResultsPresented, DepartureRequested, ChartSelection, Unavailable, Disposed }
public sealed class RankSession : IDisposable
{
    public RankRecord HandlerRecord { get; }
    public RankRecord? GlobalRecord { get; set; }
    public RankRecord? SelectedRecord { get; private set; }
    public RankPresentation? Presentation { get; private set; }
    public RankScoreReveal? Reveal { get; private set; }
    public RankTransitions Transitions { get; } = new();
    public RankStage Stage { get; private set; }
    public RankGrade? SelectorGrade { get; private set; }
    public string? Limitation { get; private set; }
    public int MaximumEnd { get; }
    public bool Finalized { get; private set; }
    private readonly object? mapIdentity;
    private readonly int expectedWhole;
    private readonly Func<bool?> failed;
    private readonly Func<bool> pendingAccounting;
    public RankSession(CircleSession session) : this(new SessionRankRecord(session),
        session.Circles.Select(c => c.Definition.StartTime).Concat(session.Sliders.Select(s => s.Definition.EndTime))
        .Concat(session.Spinners.Select(s => s.Definition.EndTime)).Max(), session.ObjectCount,
        () => session.Health?.Failed, () => session.ObjectResults.Any(r => r == Judgement.Pending)) { }
    public RankSession(RankRecord record, int maximumEnd, int expectedWhole, Func<bool?> failed, Func<bool>? pendingAccounting = null)
    {
        HandlerRecord = record; GlobalRecord = record; mapIdentity = record.MapIdentity;
        MaximumEnd = maximumEnd; this.expectedWhole = expectedWhole; this.failed = failed;
        this.pendingAccounting = pendingAccounting ?? (() => false);
    }
    public void Observe(int time, bool continuation = false, int clockState = 1, bool suspended = false,
        int timedEventCount = 0, int eventHorizon = 0)
    {
        if (Stage is not (RankStage.Active or RankStage.FinalizationEligible or RankStage.RecordCompleted or RankStage.RankRequested)) return;
        // E027 outer failure return; never pretend a failed-grade getter implies Rank entry.
        if (failed() != false) { Limitation = failed() is null ? "Success provenance unavailable: health/failure model unsupported." : "Failure latched: ordinary automatic Rank route bypassed."; return; }
        if (!Finalized && !continuation && !suspended && ((time > 0 && clockState == 0) || time > unchecked(MaximumEnd + 200)))
            Stage = RankStage.FinalizationEligible;
        if (Finalized && unchecked(MaximumEnd + 3000) <= time && (timedEventCount == 0 || unchecked(eventHorizon - 500) <= time)) RequestRank();
    }
    public bool Complete()
    {
        if (Stage != RankStage.FinalizationEligible || Finalized) return false;
        // ENGINEERING safety: defer rather than fabricate/suppress pending kernel accounting.
        if (pendingAccounting()) { Limitation = "Pending kernel results; finalization deferred by engineering safety policy."; return false; }
        HandlerRecord.Complete(failed() != false, expectedWhole);
        Finalized = true; Stage = RankStage.RecordCompleted; return true;
    }
    public TransitionDecision RequestRank()
    {
        if (!Finalized || Stage is RankStage.Disposed or RankStage.ChartSelection) return TransitionDecision.Rejected;
        if (Stage == RankStage.Unavailable) return TransitionDecision.Rejected;
        if (Stage is RankStage.RankAdmitted or RankStage.TransitionCommitted or RankStage.ResultsInitialized or RankStage.ResultsPresented or RankStage.DepartureRequested)
            return TransitionDecision.Duplicate;
        Stage = RankStage.RankRequested;
        if (GlobalRecord is not { } record || record.UnsupportedReason is not null)
        { Limitation = recordReason(); Stage = RankStage.Unavailable; return TransitionDecision.Rejected; }
        SelectorGrade = record.Grade();
        RankMode target = SelectorGrade == RankGrade.F ? RankMode.SelectPlay : RankMode.Rank;
        if (target == RankMode.SelectPlay) record.Valid = false;
        var decision = Transitions.Request(target);
        if (decision == TransitionDecision.Accepted) Stage = target == RankMode.Rank ? RankStage.RankAdmitted : RankStage.DepartureRequested;
        return decision;
        string recordReason() => GlobalRecord?.UnsupportedReason ?? "Global results record unavailable.";
    }
    public bool Commit(Action cleanup)
    {
        if (Stage is not (RankStage.RankAdmitted or RankStage.DepartureRequested) || !Transitions.Commit()) return false;
        Stage = RankStage.TransitionCommitted;
        cleanup(); // Earlier current-mode mutation survives an exception. No rollback.
        if (Transitions.Current == RankMode.SelectPlay)
        { GlobalRecord = null; Transitions.Initialized(); Stage = RankStage.ChartSelection; }
        return true;
    }
    public bool Initialize(int hostTime)
    {
        if (Stage != RankStage.TransitionCommitted || Transitions.Current != RankMode.Rank) return false;
        SelectedRecord = GlobalRecord; // Delayed selection of current reference, never request-time snapshot.
        if (SelectedRecord is null || !ReferenceEquals(SelectedRecord.MapIdentity, mapIdentity))
        { Limitation = "Missing or mismatched selected results map/record; original restoration unsupported."; Stage = RankStage.Unavailable; return false; }
        try
        {
            Presentation = RankPresentation.Capture(SelectedRecord); Reveal = new(Presentation.Score, hostTime);
            Stage = RankStage.ResultsInitialized; Transitions.Initialized(); return true;
        }
        catch (InvalidOperationException e) { Limitation = e.Message; Stage = RankStage.Unavailable; return false; }
    }
    public bool Present()
    { if (Stage != RankStage.ResultsInitialized || Presentation is null) return false; Stage = RankStage.ResultsPresented; return true; }
    public bool Skip(int hostTime) => Reveal?.Skip(hostTime) ?? false;
    public TransitionDecision Back() => Depart(RankMode.SelectPlay);
    public TransitionDecision Retry(int hostTime) { Reveal?.Skip(hostTime); return Depart(RankMode.Play); }
    private TransitionDecision Depart(RankMode target)
    {
        if (Stage is RankStage.DepartureRequested) return TransitionDecision.Duplicate;
        if (Stage is not (RankStage.ResultsPresented or RankStage.ResultsInitialized or RankStage.Unavailable)) return TransitionDecision.Rejected;
        var decision = Transitions.Request(target);
        if (decision == TransitionDecision.Accepted) Stage = RankStage.DepartureRequested;
        return decision;
    }
    public void UnsupportedCompletion()
    { if (!Finalized) { Stage = RankStage.Unavailable; Limitation ??= HandlerRecord.UnsupportedReason ?? "Ordinary results completion unavailable."; } }
    public void Dispose() { if (Stage == RankStage.Disposed) return; GlobalRecord = null; Reveal = null; Stage = RankStage.Disposed; }
}
