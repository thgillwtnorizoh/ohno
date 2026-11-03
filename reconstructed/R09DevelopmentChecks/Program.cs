// Bounded E035 finite controls, actual linked components; no original game or OS input.
using System.Numerics;
using Ohno.CircleGame.Gameplay;

if (args.Length != 1) throw new ArgumentException("Pass repository root.");
string fixtures = Path.Combine(args[0], "reconstructed", "CircleGame", "Fixtures");
int groups = 0;
var fresh = new HealthState(.05, 2, 3);
Eq(fresh.H, 0.0); Eq(fresh.A, 0.0); Eq(fresh.V, 0.0); Eq(fresh.Fill, true); Eq(fresh.FillRate, .02);
fresh.Assign(90); fresh.SetPresentation(50); fresh.ConfigureFill(123, .7, false); fresh.Reset();
Eq(fresh.H, 200.0); Eq(fresh.A, 200.0); Eq(fresh.V, 0.0); Eq(fresh.Fill, false);
Eq(fresh.FillFrom, 123); Eq(fresh.FillRate, .7); Eq(fresh.Rate, .05); Eq(fresh.B, 2.0); Eq(fresh.G, 3.0);
Pass("fresh constructor versus selective200/200/0 reset, coefficients/fill retained");
var state = new HealthState(); state.SetPresentation(44); state.Assign(250);
Eq(state.H, 200.0); Eq(state.A, 250.0); Eq(state.V, 44.0);
state.Assign(-1); Eq(state.H, 0.0); Eq(state.A, -1.0);
state.Assign(199); state.Add(6); Eq(state.H, 200.0); Eq(state.A, 205.0);
state.Subtract(-5); Eq(state.H, 205.0); Eq(state.A, 210.0);
state.Assign(200); state.Add(11); state.Subtract(1); state.ApplyPlain(1024, 5);
Eq(state.H, 200.0); Eq(state.A, 216.0);
state.Subtract(1000); Eq(state.H, 0.0); Eq(state.A, 0.0); Eq(state.V, 44.0);
state.ConfigureFill(0, .02, true); state.Subtract(0, true); Eq(state.Fill, false);
Pass("absolute/add/subtract ordering, distinct clamps/accumulator and zero-loss fill guard");
foreach (double hp in new[] { 0.0, 5.0, 10.0 })
{
    double hundred = hp == 0 ? 17.6 : 2.2, fifty = hp == 0 ? 3.2 : .4;
    double miss = hp == 0 ? -6 : hp == 5 ? -25 : -40;
    double componentMiss = hp == 0 ? -4 : hp == 5 ? -15 : -28;
    foreach (var (raw, delta) in new[] { (1024, 6.0), (512, hundred), (256, fifty), (-131072, miss),
        (64, 4.0), (8, 3.0), (128, 4.0), (-262144, componentMiss), (-524288, componentMiss),
        (4096, 1.7), (8192, 1.7), (16384, 2.0) })
    {
        var h = new HealthState(); h.Assign(100); h.SetPresentation(37);
        Near(h.PlainDelta(raw, hp), delta); h.ApplyPlain(raw, hp); Near(h.H, 100 + delta); Near(h.A, 100 + delta); Eq(h.V, 37.0);
    }
}
var gains = new HealthState(0, 1, 2); Eq(gains.PlainDelta(8, 5), 6.0); Eq(gains.PlainDelta(64, 5), 8.0); Eq(gains.PlainDelta(128, 5), 8.0);
var trialOnly = new HealthState(); trialOnly.Assign(100); trialOnly.ApplyCalibration(1028, 5); Eq(trialOnly.H, 120.0);
Reject(() => trialOnly.ApplyPlain(1028, 5)); Eq(trialOnly.ApplyPlain(0, 5), false); Eq(trialOnly.H, 120.0);
Pass("all plain HP classes at difficulty anchors, calibrated G and separate trial1028/raw0");

var control = Chart(0, 1200, [Circle(2000)]); var calibration = new HealthCalibration(); var prepared = calibration.Run(control);
double rate = .05; for (int i = 0; i < 61; i++) rate *= .96;
Eq(prepared.Trials, 62); Eq(prepared.PreRetries, 61); Eq(prepared.Rate, rate); Eq(prepared.B, 1.0); Eq(prepared.G, 1.0);
Eq(prepared.Snapshots.Count, 1); Eq(prepared.Snapshots[0], 200.0); Near(calibration.LastA, 220 - 1200 * rate);
var live = new OrdinaryHealth(control, prepared); Eq(live.State.H, 200.0); Eq(live.State.A, 200.0); Eq(live.State.V, 0.0); Eq(live.ResultEvents, 0);
Pass("independently derived HP0 one-circle control:61 reductions/62 trials, live selective reset and no live events");
var hp5 = new HealthCalibration(); Eq(hp5.Trial(Chart(5, 1200, [Circle(2000)])), HealthRetry.PreObject);
var equality = new HealthCalibration(); Eq(equality.Trial(Chart(5, 800, [Circle(2000)])), HealthRetry.PreObject); Eq(equality.LastH, 160.0);
var overflowTrial = new HealthCalibration(); Eq(overflowTrial.Trial(Chart(10, 0, [Slider(2000, 10000)])), HealthRetry.DurationOverflow);
Eq(hp5.Rate, .05 * .96); Eq(hp5.B, 1.0); Eq(hp5.G, 1.0); Eq(hp5.LastH, 140.0);
var hp10 = new HealthCalibration(); var hardChart = Chart(10, 1200, [Circle(2000)]);
Eq(hp10.Trial(hardChart), HealthRetry.Balance); Eq(hp10.Rate, .05 * .96); Eq(hp10.B, 1.02); Eq(hp10.G, 1.01);
Eq(hp10.LastH, 160.0); Eq(hp10.LastMean, -40.0);
hp10.Trial(hardChart); Check(hp10.B >= 1.02 && hp10.G >= 1.01);
var end = new HealthCalibration(); Eq(end.Trial(Chart(5, 0, [Slider(2000, 4000)])), HealthRetry.FinalTarget);
Eq(end.Rate, .05 * .94); Eq(end.B, 1.01); Eq(end.G, 1.01); Eq(end.LastH, 128.0);
var grouped = Chart(10, 0, [Slider(1000, 4160), Circle(4161), Slider(4162, 5120), Circle(5121), Slider(5122, 6080)]);
var group = new HealthCalibration(); Eq(group.Trial(grouped), HealthRetry.ThirdLowGroup);
Eq(group.Rate, .05); Eq(group.B, 1.07); Eq(group.G, 1.03); Check(double.IsNaN(group.Snapshots[4]));
group.Trial(grouped); Check(group.LastH > 70 && group.G >= 1.03);
Reject(() => new HealthCalibration().Run(control, trialLimit: 1)); Reject(() => new HealthCalibration().Run(Chart(0, 1200, [])));
Pass("retry branches, nonconsecutive third low group, coefficient retention, snapshots and engineering guard");

var breaks = new[] { new HealthBreak(2000, 3000), new HealthBreak(4000, 5000) };
var modern = new HealthCalibration(); modern.Trial(Chart(10, 0, [Circle(1000), Circle(6000)], breaks));
Eq(modern.LastExcludedGap, 2000); Eq(modern.LastGapLoss, 150.0); Eq(modern.LastBreaksConsumed, 1);
var legacy = new HealthCalibration(); legacy.Trial(Chart(10, 0, [Circle(1000), Circle(6000)], breaks, 7));
Eq(legacy.LastExcludedGap, 1000); Eq(legacy.LastGapLoss, 200.0); Eq(legacy.LastBreaksConsumed, 1);
var boundary = new HealthCalibration(); boundary.Trial(Chart(0, 1200, [Circle(2000)])); Eq(boundary.LastRetry, HealthRetry.PreObject);
Pass("modern/legacy gap exclusion consumes one break, ordered safety equality branch retained");

foreach (var (t, previous, spinner, loss) in new[] { (1010, 1000, false, .5), (1010, 1000, true, .125),
    (1000, 1000, false, 0.0), (900, 1000, false, -5.0) })
{
    var h = Runtime(.05); h.State.Assign(199); h.Frame(Frame(1000, 1000)); // First pass stores eligible only.
    Eq(h.State.H, 199.0); h.Frame(Frame(t, previous, selectedSpinner: spinner));
    Eq(h.LastPassiveLoss, loss); Eq(h.State.H, Math.Max(0, 199 - loss)); Eq(h.State.A, Math.Max(0, 199 - loss));
}
var zero = Runtime(.05); zero.State.Assign(1); zero.Frame(Frame(1000, 1000)); zero.Frame(Frame(2000, 1000));
Eq(zero.State.H, 0.0); Eq(zero.Failed, false); Eq(zero.LastFailure, HealthFailure.NotChecked);
zero.ApplyResultHp(1024, true); Eq(zero.CheckFailure(2000), HealthFailure.Healthy); Eq(zero.State.H, 6.0);
Pass("signed positive/zero/negative passive dt, spinner quarter, first eligibility lag and no passive fail");
var lag = Runtime(.05); lag.State.Assign(100); lag.State.SetPresentation(50);
lag.Frame(Frame(1000, 990)); Eq(lag.State.H, 100.0);
var denied = Frame(1010, 1000) with { LateEligibility = Eligibility(1010) with { Marker = 7 } };
lag.Frame(denied); Eq(lag.State.H, 99.5); Eq(lag.Eligible, false);
lag.Frame(Frame(1020, 1010)); Eq(lag.State.H, 99.5); Eq(lag.Eligible, true);
var paused = Frame(1500, 1020) with { Paused = true, HostScale = 100, HostElapsed = 480 };
double vBefore = lag.State.V; lag.Frame(paused); Eq(lag.State.H, 99.5); Eq(lag.State.V, vBefore); Eq(lag.Eligible, true);
lag.Frame(Frame(1020, 1020)); Eq(lag.State.H, 99.5);
Pass("prior eligibility used before late recompute; pause retains state and zero-dt resume has no catch-up");

var eligibility = new HealthEligibility(800, 799, 2000, 8000, 1200, -1, 1, 14, 3000);
Eq(eligibility.Evaluate(), true); Eq((eligibility with { Time = 799 }).Evaluate(), false);
Eq((eligibility with { Time = 8000, QueueHead = null }).Evaluate(), true);
Eq((eligibility with { Time = 8001, QueueHead = null }).Evaluate(), false);
Eq((eligibility with { Time = 3000 }).Evaluate(), true); Eq((eligibility with { Time = 3001 }).Evaluate(), false);
Eq((eligibility with { Time = 3001, Format = 7 }).Evaluate(), true);
Eq((eligibility with { Marker = 0 }).Evaluate(), false); Eq((eligibility with { ClockState = 0 }).Evaluate(), false);
Eq((eligibility with { Special = true, SpecialUntil = 799 }).Evaluate(), false);
Eq((eligibility with { Recorded = true, RecordedUntil = 799 }).Evaluate(), false);
var breakChart = Chart(0, 0, [Circle(1000), Circle(3000), Circle(8000)], [new(4000, 6000)]);
var breakLive = new OrdinaryHealth(breakChart, new(.01, 1, 1, 1, [200, 200, 200], 0, 0, 0, 0));
Eq(breakLive.QueueHead, (int?)3000); breakLive.UpdateBreakState(3001, []); Eq(breakLive.QueueHead, (int?)3000);
breakLive.UpdateBreakState(4000, []); Eq(breakLive.Marker, 4000); Eq(breakLive.QueueHead, (int?)null);
breakLive.UpdateBreakState(6000, []); Eq(breakLive.Marker, 6000); breakLive.UpdateBreakState(6001, []); Eq(breakLive.Marker, 6000);
breakLive.UpdateBreakState(8000, [8000]); Eq(breakLive.Marker, -1);
Pass("inclusive bounds, explicit provider/record gates, midpoint/end queue and inclusive break marker clearing");

var smooth = new HealthState(); smooth.Assign(200); smooth.SetPresentation(100); smooth.ConfigureFill(0, .02, false);
smooth.Smooth(1, 1000, 1000, 1, 1, false); Eq(smooth.V, 125.0); Eq(smooth.H, 200.0); Eq(smooth.A, 200.0);
smooth.Assign(40); smooth.SetPresentation(100); smooth.ConfigureFill(0, .02, true);
smooth.Smooth(1, 0, 0, 1, 1, false); Eq(smooth.V, 90.0); Eq(smooth.Fill, false);
smooth.Assign(200); smooth.SetPresentation(0); smooth.ConfigureFill(100, .02, true);
smooth.Smooth(100, 50, 1000, 1, 1, false); Eq(smooth.V, 0.0);
smooth.Smooth(101, 50, 1000, 1, 1, false); Eq(smooth.V, 1.0);
smooth.Smooth(102, 0, 100, 1, 0, true); Eq(smooth.V, 3.0);
smooth.Assign(110); smooth.SetPresentation(100); smooth.ConfigureFill(0, .02, false);
smooth.Smooth(103, 0, 0, 8, 1, false); Eq(smooth.V, 120.0); // No universal target clamp.
Pass("fill/proportional rise/fall, distinct host/time sources, equality and target overshoot");
var failure = Runtime(.05); failure.State.Assign(0); failure.State.SetPresentation(100);
Eq(failure.CheckFailure(1, new(NoFail: true)), HealthFailure.Suppressed); Eq(failure.Failed, false);
Eq(failure.CheckFailure(2), HealthFailure.Latched); Eq(failure.Failed, true); Eq(failure.FailureTime, (int?)2);
Eq(failure.CheckFailure(3), HealthFailure.AlreadyLatched); Eq(failure.FailureTime, (int?)2);
failure.SupplyRecoveryCount(1); Eq(failure.CheckFailure(4), HealthFailure.Recovery); Eq(failure.State.H, 160.0); Eq(failure.RecoveryCount, 0);
failure.State.SetPresentation(155); failure.Frame(Frame(4, 4)); Eq(failure.Recovering, true);
failure.State.SetPresentation(156); failure.Frame(Frame(4, 4)); Eq(failure.Recovering, false);
var recovery = Runtime(.05); recovery.State.Assign(100); recovery.SupplyRecoveryCount(1); recovery.BeginRecovery();
Eq(recovery.State.H, 200.0); Eq(recovery.State.A, 260.0); Eq(recovery.State.FillRate, .07999999821186066);
Pass("fresh target failure versus V, suppression, one latch/timestamp, recovery-before-latch and release equality");

Beatmap map = Beatmap.Load(Path.Combine(fixtures, "health-r09.osu"));
var session = new CircleSession(map, ScoreConfiguration.FromBeatmap(map));
Check(session.Health is not null); var enabled = session.Health!;
Eq(enabled.ResultEvents, 0); Eq(session.Ledger.EventCount, 0); Eq(session.Ledger.Score, 0); Eq(enabled.State.H, 200.0); Eq(enabled.State.A, 200.0); Eq(enabled.State.V, 0.0);
Eq(enabled.Calibration.Snapshots.Count, session.ObjectCount); Check(enabled.State.Rate > 0 && enabled.State.Rate <= .05);
var reset = new CircleSession(map, session.Scoring); Check(!ReferenceEquals(enabled, reset.Health)); Eq(reset.Health!.State.A, 200.0); Eq(reset.Ledger.Score, 0);
var originalFixture = Beatmap.Load(Path.Combine(fixtures, "first-circles.osu")); var unsupported = new CircleSession(originalFixture, ScoreConfiguration.FromBeatmap(originalFixture));
Eq(unsupported.Health, (OrdinaryHealth?)null); Check(unsupported.HealthStatus.Contains("M2902"));
unsupported.Press(originalFixture.Circles[0].Position, originalFixture.Circles[0].StartTime); Eq(unsupported.Ledger.N300, (ushort)1);
Pass("fresh prepared chart/live reset and restart/replacement isolation; unsupported grouping rejects HP without breaking score");

// Independent sessions below exercise all real emissions in chronological order.
var hSession = new CircleSession(map, session.Scoring); var noHp = new CircleSession(map, session.Scoring, false);
for (int t = 0; t <= 18000; t += 10)
{
    Vector2 pointer = new(256, 192); int buttons = 0;
    var slider = map.Sliders.Single();
    if (t >= slider.Head.StartTime && t <= slider.EndTime) { pointer = slider.BallPosition(t); buttons = 1; }
    foreach (var c in map.Circles.Where(c => c.StartTime == t)) { hSession.Press(c.Position, t); noHp.Press(c.Position, t); }
    if (t == slider.Head.StartTime) { hSession.Press(pointer, t); noHp.Press(pointer, t); }
    // Explicit component pointer values, not OS events, player performance or autoplay in the app.
    hSession.Advance(t, pointer, buttons); noHp.Advance(t, pointer, buttons);
    Eq(hSession.Ledger.Score, noHp.Ledger.Score); Eq(hSession.Combo, noHp.Combo); Eq(hSession.MaxCombo, noHp.MaxCombo);
    Eq(hSession.Ledger.TotalWholeResults, noHp.Ledger.TotalWholeResults); Eq(hSession.Accuracy, noHp.Accuracy);
}
Eq(hSession.Health!.ResultEvents, hSession.Ledger.EventCount); Eq(hSession.JudgedCount, noHp.JudgedCount);
int eventCount = hSession.Health.ResultEvents; hSession.Advance(18000); Eq(hSession.Health.ResultEvents, eventCount);
Pass("actual circle/slider/spinner lifecycle once-only HP and per-step score/accuracy/combo invariance");

var hookLedger = new ScoreLedger(ScoreConfiguration.EngineeringPractice); var hookHp = new HealthState(); hookHp.Assign(100);
var ev = new ScoreEvent(new(0, ScoreObjectKind.Circle, 1000, 1000), 0, ScoreEventKind.CircleWhole, 1024, false, false, ScorePolicy.PlainLegacy);
hookLedger.Apply(ev, () => { Eq(hookLedger.CurrentCombo, 1); Eq(hookLedger.Score, 0); hookHp.ApplyPlain(1024, 5); });
Eq(hookHp.H, 106.0); Eq(hookLedger.Score, 0); Eq(hookLedger.N300, (ushort)0);
var componentLedger = new ScoreLedger(ScoreConfiguration.EngineeringPractice);
var componentHp = new HealthState(); componentHp.Assign(100);
var headEvent = new ScoreEvent(new(0, ScoreObjectKind.Slider, 1000, 3000), 0, ScoreEventKind.SliderHead, 64, true, true, ScorePolicy.PlainLegacy);
componentLedger.Apply(headEvent, () => componentHp.ApplyPlain(64, 5));
componentLedger.Apply(headEvent with { Sequence = 1, Kind = ScoreEventKind.SliderRepeatOrTerminal, ProducerRole = SliderCheckpointKind.Final, Raw = -524288 },
    () => componentHp.ApplyPlain(-524288, 5));
Eq(componentLedger.CurrentCombo, 1); Eq(componentHp.H, 89.0); Eq(componentLedger.TotalWholeResults, 0);
componentLedger.Apply(headEvent with { Sequence = 2, Kind = ScoreEventKind.SliderInterior, ProducerRole = SliderCheckpointKind.Interior, Raw = -262144 },
    () => componentHp.ApplyPlain(-262144, 5));
Eq(componentLedger.CurrentCombo, 0); Eq(componentHp.H, 74.0); Eq(componentLedger.NMiss, (ushort)0);
Pass("HP site after combo/before record, record-enable independence and terminal versus interior failure");
Console.WriteLine($"R09 HEALTH CHECKS: {groups} passed (E035 finite component controls; no original runtime or physical gameplay)");

static PreparedHealthObject Circle(int t) => new(t, t, ScoreObjectKind.Circle, true);
static PreparedHealthObject Slider(int s, int e) => new(s, e, ScoreObjectKind.Slider, true, 0, 1);
static HealthChart Chart(double hp, int approach, PreparedHealthObject[] objects, HealthBreak[]? breaks = null, int format = 14) =>
    new(hp, approach, format, objects, breaks ?? []);
static OrdinaryHealth Runtime(double rate) => new(Chart(5, 1200, [Circle(2000)]), new(rate, 1, 1, 1, [200], 0, 0, 0, 0));
static HealthEligibility Eligibility(int time) => new(time, time, 0, 100000, 0, -1, 1, 14);
static HealthFrame Frame(int time, int previous, bool selectedSpinner = false) => new(time, previous, 0, 0, 0, 1, false, selectedSpinner, false, Eligibility(time));
void Pass(string name) { groups++; Console.WriteLine("PASS R09 " + name); }
static void Eq<T>(T actual, T expected) { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"Expected {expected}; got {actual}"); }
static void Near(double actual, double expected, double tolerance = 1e-10) { if (!double.IsFinite(actual) || Math.Abs(actual - expected) > tolerance) throw new Exception($"Expected near {expected}; got {actual}"); }
static void Check(bool value) { if (!value) throw new Exception("Bounded health assertion failed."); }
static void Reject(Action action) { try { action(); } catch (System.IO.InvalidDataException) { return; } throw new Exception("Unsupported health inputs accepted."); }
