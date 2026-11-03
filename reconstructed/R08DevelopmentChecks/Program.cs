// Bounded development controls from E33; linked production components only.
// No windows, OS input, autoplay, original runtime or independent QA06 campaign.
using System.Numerics;
using Ohno.CircleGame.Gameplay;

if (args.Length != 1) throw new ArgumentException("Pass repository root.");
string fixtures = Path.Combine(args[0], "reconstructed", "CircleGame", "Fixtures");
int groups = 0;

var a = Fresh(); Whole(a, 1024); Whole(a, 1024);
Eq(a.Score, 600); Eq(a.CurrentCombo, 2); Eq(a.N300, (ushort)2); Eq(a.AccuracyFraction, 1f);
Pass("two circle300s ->600");
var b = Fresh(); Whole(b, 1024); Whole(b, 512);
Eq(b.Score, 400); Eq(b.AccuracyFraction, 400f / 600f); Eq(b.N100, (ushort)1);
Near(b.AccuracyPercent, 66.66667, .0001); Pass("circle300/100 ->400 and Single accuracy");
foreach (double k in new[] { 0.0, .375, 1.0, 1.25, 2.75 })
{
    var l = Fresh(k); Whole(l, 1024); Whole(l, 1024); Whole(l, 512);
    Eq(l.Score, 700 + (int)(4 * k)); Eq(l.LastOutcome.ComboBefore, 2); Eq(l.CurrentCombo, 3);
    var fifty = Fresh(k); Whole(fifty, 1024); Whole(fifty, 1024); Whole(fifty, 256);
    Eq(fifty.Score, 650 + (int)(2 * k));
}
Pass("old-combo bonus, explicit K, truncation and 50/100 integer factors");
var missed = Fresh(); Whole(missed, 1024); Whole(missed, -131072);
Eq(missed.Score, 300); Eq(missed.CurrentCombo, 0); Eq(missed.MaximumCombo, 1);
Eq(missed.ComboSnapshot, (ushort)0); Eq(missed.NMiss, (ushort)1); Eq(missed.AccuracyFraction, .5f);
Pass("circle300/miss ->300 and combo reset");
foreach (double k in new[] { .375, 1.0, 1.25 })
{
    var l = Fresh(k); Head(l); Checkpoint(l, 128, SliderCheckpointKind.Final); Whole(l, 1024, ScoreObjectKind.Slider);
    Eq(l.Score, 360 + (int)(12 * k)); Eq(l.CurrentCombo, 2); Eq(l.MaximumCombo, 2);
    Eq(l.ComboSnapshot, (ushort)2); Eq(l.N300, (ushort)1); Eq(l.WeightedHitTotal, 300);
}
Pass("slider64/128/parent300 with bonus and no duplicate combo/accuracy");
var broken = Fresh(); Head(broken); Checkpoint(broken, -262144, SliderCheckpointKind.Interior);
Checkpoint(broken, 128, SliderCheckpointKind.Final); Whole(broken, 512, ScoreObjectKind.Slider);
Eq(broken.Score, 160); Eq(broken.CurrentCombo, 1); Eq(broken.N100, (ushort)1);
Eq(broken.NMiss, (ushort)0); Eq(broken.AccuracyFraction, 100f / 300f);
Pass("slider head/failure/terminal/parent100 ->160");
var spin = Fresh(); var observer = new SpinnerProgressObservation();
for (int i = 1; i <= 16; i++)
{
    int raw = observer.Observe(i, 15);
    if (raw != 0) spin.Apply(Event(ScoreEventKind.SpinnerProgress, raw, ScoreObjectKind.Spinner, i));
}
Eq(spin.Score, 800); Eq(spin.CurrentCombo, 0); Eq(spin.TotalWholeResults, 0);
Whole(spin, 1024, ScoreObjectKind.Spinner); Eq(spin.Score, 1100); Eq(spin.CurrentCombo, 1); Eq(spin.N300, (ushort)1);
var bonus = Fresh(); bonus.Apply(Event(ScoreEventKind.SpinnerProgress, 16384, ScoreObjectKind.Spinner));
Eq(bonus.Score, 1100); Eq(bonus.CurrentCombo, 0); Eq(bonus.TotalWholeResults, 0);
Pass("actual C1..16 selector + spinner final ->1100; bonus-associated amount isolated");

var terminal = Fresh(); Head(terminal); Checkpoint(terminal, -524288, SliderCheckpointKind.Final);
Eq(terminal.Score, 30); Eq(terminal.CurrentCombo, 1); Eq(terminal.ComboSnapshot, (ushort)1);
Eq(terminal.NMiss, (ushort)0); Eq(terminal.MaximumCombo, 1);
var ticks = Fresh(); Checkpoint(ticks, 8, SliderCheckpointKind.Interior); Eq(ticks.Score, 10);
Checkpoint(ticks, 64, SliderCheckpointKind.Repeat); Checkpoint(ticks, 128, SliderCheckpointKind.Final);
Eq(ticks.Score, 70); Eq(ticks.CurrentCombo, 3); Eq(ticks.TotalWholeResults, 0);
var suppression = Fresh(); suppression.Apply(Event(ScoreEventKind.CircleWhole, 1024, ScoreObjectKind.Circle) with { AccountingEnabled = false });
Eq(suppression.Score, 0); Eq(suppression.TotalWholeResults, 0); Eq(suppression.CurrentCombo, 1);
Eq(suppression.LastOutcome.Status, ScoreAccountingStatus.RecordSuppressed);
Pass("component base table, terminal bypass and conditional record admission vs combo");

Eq(Fresh().AccuracyPercent, 100f);
var max = Fresh(); Whole(max, 1024); Whole(max, 1024); Whole(max, 1024); Whole(max, -131072); Whole(max, 512);
Eq(max.Score, 1012); Eq(max.MaximumCombo, 3); Eq(max.CurrentCombo, 1); Eq(max.ComboSnapshot, (ushort)1);
var empty = Fresh(); Eq(empty.Apply(Event(ScoreEventKind.CircleWhole, 0, ScoreObjectKind.Circle)).Status, ScoreAccountingStatus.NoEvent);
Eq(empty.EventCount, 0);
foreach (double invalid in new[] { double.NaN, double.PositiveInfinity, -1, 11 })
{
    bool rejected = false;
    try { Fresh(invalid); } catch (ArgumentOutOfRangeException) { rejected = true; }
    if (!rejected) throw new Exception("Invalid K accepted.");
}
var unsupported = Fresh(); Whole(unsupported, 1024);
Eq(unsupported.Apply(Event(ScoreEventKind.CircleWhole, 1028, ScoreObjectKind.Circle)).Status, ScoreAccountingStatus.Unsupported);
Whole(unsupported, 1024); Eq(unsupported.Score, 300); Eq(unsupported.TotalWholeResults, 1); Eq(unsupported.IsSupported, false);
var v2 = new ScoreLedger(new(1, ScorePolicy.ScoreV2, "unsupported test")); Whole(v2, 1024);
Eq(v2.IsSupported, false); Eq(v2.Score, 0);
Pass("empty accuracy, old vs maximum combo, raw0, bounded K and unsupported flags/policy");

Beatmap map = Beatmap.Load(Path.Combine(fixtures, "first-slider.osu"));
var isolated = map with { Circles = Array.Empty<CircleDefinition>() };
var s = new CircleSession(isolated, ScoreConfiguration.EngineeringPractice); var d = s.Sliders[0].Definition;
s.Press(d.Head.Position, d.Head.StartTime); s.Advance(d.TerminalTime, d.RequiredPosition(d.TerminalTime), 1);
Eq(s.Ledger.Score, 60); Eq(s.Ledger.EventCount, 2);
s.Advance(d.TerminalTime, d.RequiredPosition(d.TerminalTime), 1); Eq(s.Ledger.EventCount, 2);
s.Advance(d.EndTime, d.RequiredPosition(d.EndTime), 1); s.Advance(d.EndTime, d.RequiredPosition(d.EndTime), 1);
Eq(s.Ledger.Score, 372); Eq(s.Ledger.EventCount, 3); Eq(s.Combo, 2); Eq(s.JudgedCount, 1); Eq(s.HitValueTotal, 300);
var renewed = new CircleSession(isolated, s.Scoring); var replaced = new CircleSession(map, s.Scoring);
Eq(renewed.Ledger.Score, 0); Eq(renewed.Combo, 0); Eq(renewed.JudgedCount, 0); Eq(replaced.Ledger.EventCount, 0);
if (ReferenceEquals(s.Ledger, renewed.Ledger) || ReferenceEquals(s.Ledger, replaced.Ledger)) throw new Exception("Ledger state leaked across replacement.");
Pass("actual slider lifecycle once-only accounting and fresh restart/replacement state");

Beatmap repeats = Beatmap.Load(Path.Combine(fixtures, "slider-repeat.osu"));
var delayed = new CircleSession(repeats, ScoreConfiguration.EngineeringPractice); var rd = delayed.Sliders[0].Definition;
delayed.Press(rd.Head.Position, rd.Head.StartTime); delayed.Advance(rd.Head.StartTime, rd.Head.Position, 1);
delayed.Advance(6000, rd.RequiredPosition(6000), 1);
Eq(delayed.Ledger.Score, 60); Eq(delayed.Ledger.LastEvent!.Value.Kind, ScoreEventKind.SliderInterior);
Eq(delayed.Ledger.LastEvent.Value.Raw, 64); Eq(delayed.Ledger.LastEvent.Value.ProducerRole, (SliderCheckpointKind?)SliderCheckpointKind.Interior);
delayed.Advance(6000, rd.RequiredPosition(6000), 1); delayed.Advance(6000, rd.RequiredPosition(6000), 1);
Eq(delayed.Ledger.Score, 120); Eq(delayed.Ledger.EventCount, 4);
delayed.Advance(6000, rd.RequiredPosition(6000), 1); Eq(delayed.Ledger.EventCount, 4);
Pass("due-prefix raw64 on interior provenance; repeated-time distinct slots, no manufactured backlog score");

Beatmap spinnerMap = Beatmap.Load(Path.Combine(fixtures, "spinner-practice.osu"));
var live = new CircleSession(spinnerMap, ScoreConfiguration.EngineeringPractice);
for (int i = 0; i <= 8; i++) live.Advance(3000 + 100 * i,
    SpinnerState.DisplayCenter + new Vector2((float)(100 * Math.Cos(i)), (float)(100 * Math.Sin(i))), 1);
Eq(live.Ledger.Score, 100); Eq(live.Ledger.EventCount, 1); Eq(live.Combo, 0); Eq(live.Ledger.TotalWholeResults, 0);
live.Advance(3800, Vector2.Zero, 1); live.Advance(3801, Vector2.Zero, 1);
Eq(live.Ledger.Score, 100); Eq(live.Ledger.EventCount, 1);
Pass("spinner emits only actual nonzero observation, never replays stored raw on other frames");

string path = Path.Combine(Path.GetTempPath(), "ohno-r08-" + Guid.NewGuid().ToString("N") + ".osu");
try
{
    File.WriteAllText(path, "osu file format v14\n[General]\nMode:0\n[Difficulty]\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,1000,4,1,0,100,1,0\n[HitObjects]\n0,0,1000,1,0\n0,0,3000,2,0,L|1:0,1,1\n");
    var shortSlider = new CircleSession(Beatmap.Load(path), ScoreConfiguration.EngineeringPractice);
    shortSlider.Press(Vector2.Zero, 1000); shortSlider.Press(Vector2.Zero, 1000); Eq(shortSlider.Ledger.EventCount, 1);
    shortSlider.Advance(3010, new(float.NaN,0), 0);
    Eq(shortSlider.Sliders[0].Head.Result, Judgement.Pending); Eq(shortSlider.Sliders[0].Result, Judgement.Miss);
    Eq(shortSlider.Combo, 0); Eq(shortSlider.Ledger.NMiss, (ushort)1); Eq(shortSlider.Ledger.Score, 300);
    shortSlider.Advance(3161); Eq(shortSlider.Ledger.NMiss, (ushort)1); Eq(shortSlider.JudgedCount, 2);
}
finally { File.Delete(path); }
Pass("circle deduplication and E33 parent-miss reset before late head timeout");
Console.WriteLine($"R08 DEVELOPMENT CHECKS: {groups} passed (E33 finite controls; no original runtime/human play)");

ScoreLedger Fresh(double k = 1) => new(new(k, ScorePolicy.PlainLegacy, "ENGINEERING test"));
ScoreEvent Event(ScoreEventKind kind, int raw, ScoreObjectKind source, int sequence = 0, SliderCheckpointKind? role = null) =>
    new(new(0, source, 0, source == ScoreObjectKind.Circle ? 0 : 3000), sequence, kind, raw, true, source == ScoreObjectKind.Slider, ScorePolicy.PlainLegacy, role);
void Whole(ScoreLedger l, int raw, ScoreObjectKind kind = ScoreObjectKind.Circle)
{
    ScoreEvent e = Event(kind switch { ScoreObjectKind.Slider => ScoreEventKind.SliderAggregate,
        ScoreObjectKind.Spinner => ScoreEventKind.SpinnerFinal, _ => ScoreEventKind.CircleWhole }, raw, kind);
    if (kind == ScoreObjectKind.Circle) e = e with { Source = new(l.EventCount, kind, l.EventCount * 1000, l.EventCount * 1000) };
    l.Apply(e);
}
void Head(ScoreLedger l) => l.Apply(Event(ScoreEventKind.SliderHead, 64, ScoreObjectKind.Slider));
void Checkpoint(ScoreLedger l, int raw, SliderCheckpointKind role) => l.Apply(Event(role == SliderCheckpointKind.Interior ?
    ScoreEventKind.SliderInterior : ScoreEventKind.SliderRepeatOrTerminal, raw, ScoreObjectKind.Slider, role: role));
void Pass(string name) { groups++; Console.WriteLine("PASS R08 " + name); }
static void Eq<T>(T actual, T expected) { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"Expected {expected}; got {actual}"); }
static void Near(double actual, double expected, double tolerance) { if (!double.IsFinite(actual) || Math.Abs(actual - expected) > tolerance) throw new Exception($"Expected near {expected}; got {actual}"); }
