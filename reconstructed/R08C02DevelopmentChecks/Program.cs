// Bounded E034 preparation targets and explicit conditional event sequences.
// No gameplay automation, window, original binary, QA06 framework or fitted scores.
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using Ohno.CircleGame.Gameplay;

if (args.Length != 1) throw new ArgumentException("Pass repository root.");
string fixtures = Path.Combine(args[0], "reconstructed", "CircleGame", "Fixtures");
int groups = 0;
var mixed = Beatmap.Load(Path.Combine(fixtures, "mixed-r07.osu"));
var joined = Beatmap.Load(Path.Combine(fixtures, "bezier-joined.osu"));
Eq(Hash("mixed-r07.osu"), "2d76f1c3e7f7fd76e65987f01dc4dc6651610214d543d0b6ac814fecbcbae4cf");
Eq(Hash("bezier-joined.osu"), "2d844b472e2fd138e638dedac6a1fe2ce9e94164131ad258be0b97f162bde85b");
var m = mixed.ScoreMetadata!; var j = joined.ScoreMetadata!;
Eq(m.RowCount, 4); Eq(m.FirstStart, 3000); Eq(m.LastStart, 15000); Eq(m.WholeSeconds, 12);
Eq(ScoreConfiguration.FromBeatmap(mixed).K, 2.0);
Eq(j.RowCount, 1); Eq(j.WholeSeconds, 0); Eq(j.Hp, 0f); Eq(j.Cs, 4f); Eq(j.Od, 4f);
Eq(j.Prepare().DensityTerm, 16f); Eq(ScoreConfiguration.FromBeatmap(joined).K, 3.0);
Pass("pinned unchanged fixture metadata: Mixed K2, Joined K3 including positive-count zero seconds");

var mixedAuto = MixedScore(ScoreConfiguration.FromBeatmap(mixed));
var mixedOne = MixedScore(ScoreConfiguration.EngineeringPractice);
Eq(mixedAuto.Score, 2392); Eq(mixedOne.Score, 2236);
Eq(mixedAuto.N300, (ushort)4); Eq(mixedAuto.AccuracyFraction, 1f); Eq(mixedAuto.MaximumCombo, 7);
Eq(mixedAuto.EventCount, mixedOne.EventCount);
Pass("Mixed conditional matched event sequence:2392 automatic vs2236 engineering");
var joinedAuto = JoinedScore(ScoreConfiguration.FromBeatmap(joined));
var joinedOne = JoinedScore(ScoreConfiguration.EngineeringPractice);
Eq(joinedAuto.Score, 534); Eq(joinedOne.Score, 438); Eq(joinedAuto.MaximumCombo, 5); Eq(joinedAuto.N300, (ushort)1);
Eq(joinedAuto.AccuracyFraction, 1f);
Pass("Joined conditional events:534 automatic vs438 engineering; reported582 remains unresolved");

Eq(Read([1000, 101000], "HPDrainRate:0\nCircleSize:5\nOverallDifficulty:5").Prepare().K, 1);
Eq(Read(Enumerable.Range(0, 20).Select(i => 1000 + i * 5000 / 19), "HPDrainRate:0\nCircleSize:5\nOverallDifficulty:5").Prepare().K, 3);
Pass("ordinary multi-object sparse and density-saturated spans");
var midpoint = Read([1000, 5000]); Eq(midpoint.Prepare().BeforeRounding, 2.5f); Eq(midpoint.Prepare().K, 2);
var lower = midpoint with { Hp = 4.99999f }; var upper = midpoint with { Hp = 5.00001f };
Check(lower.Prepare().BeforeRounding < 2.5f && upper.Prepare().BeforeRounding > 2.5f);
Eq(lower.Prepare().K, 2); Eq(upper.Prepare().K, 3); Eq(Read([1000, 4000]).Prepare().K, 3);
Eq((midpoint with { Hp = 0 }).Prepare().K, 2); Eq((midpoint with { Hp = 10 }).Prepare().K, 3);
Pass("Single arithmetic, exact midpoint-to-even, neighboring boundary and HP variation");
foreach (var (end, seconds, k) in new[] { (1999, 0, 3), (2000, 1, 3), (3000, 2, 2) })
{
    var v = Read([1000, end], "HPDrainRate:0\nCircleSize:5\nOverallDifficulty:5");
    Eq(v.WholeSeconds, seconds); Eq(v.Prepare().K, k);
}
var spinnerA = Raw("256,192,2000,8,0,5000"); var spinnerB = Raw("256,192,2000,8,0,10000");
Eq(spinnerA.WholeSeconds, 0); Eq(spinnerB.WholeSeconds, 0); Eq(spinnerA.LastEnd, 5000); Eq(spinnerB.LastEnd, 10000);
Eq(spinnerA.Prepare().K, spinnerB.Prepare().K);
Pass("zero-second transitions and spinner end excluded from denominator");
var noBreak = Read([1000, 2000, 3000, 5000]); Eq(noBreak.Prepare().K, 3);
var withBreak = Read([1000, 2000, 3000, 5000], events: "2,2000,4000");
Eq(withBreak.BreakMilliseconds, 2000); Eq(withBreak.WholeSeconds, 2); Eq(withBreak.Prepare().K, 4);
var overlap = Read([1000, 2000, 3000, 5000], events: "2,1500,3000\n2,2000,3500");
Eq(overlap.BreakMilliseconds, 3000); Eq(overlap.WholeSeconds, 1);
var negative = Read([1000, 2000, 3000, 5000], events: "2,0,5000");
Eq(negative.WholeSeconds, -1); Eq(negative.Prepare().DensityTerm, 0f); Eq(negative.Prepare().K, 2);
var backward = Read([1000, 5000], events: "2,3000,2000"); Eq(backward.BreakMilliseconds, -1000); Eq(backward.WholeSeconds, 5);
Pass("break sum without clipping/union/positivity repair; negative denominator clamps density");
var unordered = Raw("0,0,3000,1,0\n0,0,1000,1,0");
Eq(unordered.FirstStart, 3000); Eq(unordered.LastStart, 1000); Eq(unordered.WholeSeconds, -2);
var unknown = Raw("0,0,8000,0,0\n0,0,3000,5,0\n0,0,1000,1,0\n0,0,9000,0,0");
Eq(unknown.RowCount, 4); Eq(unknown.Circles, (ushort)2); Eq(unknown.FirstStart, 8000); Eq(unknown.LastStart, 1000);
var hold = Raw("0,0,1000,128,0,4000:0:0:0:");
Eq(hold.LastStart, 4000); Eq(hold.LastEnd, 4000); Eq(hold.Sliders, (ushort)1);
Pass("raw row ordering, type mask, unknown row count and separate hold metadata branch (not gameplay admission)");
var fractional = Raw("0,0,1000.9,1,0\n0,0,3000.1,1,0");
Eq(fractional.FirstStart, 1000); Eq(fractional.LastStart, 3000); Eq(fractional.WholeSeconds, 2);
var defaults = Read([1000, 5000]); Eq(defaults.Hp, 5f); Eq(defaults.Od, 5f); Eq(defaults.Cs, 5f);
var difficulty = Read([1000, 5000], "HPDrainRate:11\nCircleSize:-1\nOverallDifficulty:7.5");
Eq(difficulty.Hp, 10f); Eq(difficulty.Cs, 0f); Eq(difficulty.Od, 7.5f);
Eq(Raw("0,0,1000,1,0", "HPDrainRate:7", version: 12).Hp, 7f);
Reject(() => Raw("0,0,1000,1,0", "HPDrainRate:7.5", version: 12));
Pass("Double time truncation, fresh5 defaults, modern Single clamping and older Byte boundary");
var wrapped = Read([1000, 5000], events: "2,-2147483648,2147483647");
Eq(wrapped.BreakMilliseconds, -1); Eq(wrapped.WholeSeconds, 4);
var subtracted = Raw("0,0,-2147483648,1,0\n0,0,2147483647,1,0"); Eq(subtracted.WholeSeconds, 0);
var counts = Raw(string.Join('\n', Enumerable.Repeat("0,0,1000,1,0", 65536)));
Eq(counts.RowCount, 65536); Eq(counts.Circles, (ushort)0);
Pass("unchecked Int32 break/span arithmetic and independent UInt16 recognized-count wrap");
Reject(() => Read([]).Prepare()); // 0/0 is unsupported, never a fallback K.
Reject(() => Read([1000], "HPDrainRate:NaN"));
Reject(() => Raw("0,0,1e100,1,0")); Reject(() => Read([1000], events: "2,1000,bad"));
Reject(() => ScoreConfiguration.FromBeatmap(new("Synthetic", 5, 5, 5, Array.Empty<CircleDefinition>())));
Pass("explicit exceptional/nonfinite/missing-metadata status without engineering fallback");
var timingA = Raw("0,0,1000,2,0,L|100:0,1,100\n0,0,5000,1,0", extra: "[TimingPoints]\n0,1000,4,1,0,100,1,0");
var timingB = Raw("0,0,1000,2,0,B|900:800,16,4096\n0,0,5000,1,0", extra: "[TimingPoints]\n0,50,4,1,0,100,1,0");
Eq(timingA.WholeSeconds, timingB.WholeSeconds); Eq(timingA.Prepare().K, timingB.Prepare().K);
Pass("timing/SV/path/repeats do not replace metadata row-start denominator");
foreach (string path in Directory.GetFiles(fixtures, "*.osu"))
{
    var map = Beatmap.Load(path); var config = ScoreConfiguration.FromBeatmap(map);
    Check(config.MultiplierProvenance.StartsWith("AUTO E034 fresh .osu"));
    Check(config.K >= 0 && config.K <= 10);
    var session = new CircleSession(map, config); var restarted = new CircleSession(map, session.Scoring);
    Eq(restarted.Ledger.Score, 0); Eq(restarted.Scoring, config);
}
Eq(ScoreConfiguration.FromBeatmap(mixed).K, 2.0); Eq(ScoreConfiguration.FromBeatmap(joined).K, 3.0);
var changedDifficulty = mixed with { ScoreMetadata = m with { Hp = 10 } };
Check(ScoreConfiguration.FromBeatmap(changedDifficulty).K != ScoreConfiguration.FromBeatmap(mixed).K);
Pass("all13 old fixtures prepare; fresh sessions retain config and map replacement recomputes K");
var oneSlider = Beatmap.Load(Path.Combine(fixtures, "first-slider.osu")) with { Circles = Array.Empty<CircleDefinition>() };
var auto = new CircleSession(oneSlider, ScoreConfiguration.FromBeatmap(oneSlider)); var d = auto.Sliders[0].Definition;
auto.Press(d.Head.Position, d.Head.StartTime); auto.Advance(d.TerminalTime, d.RequiredPosition(d.TerminalTime), 1);
auto.Advance(d.EndTime, d.RequiredPosition(d.EndTime), 1); auto.Advance(d.EndTime, d.RequiredPosition(d.EndTime), 1);
Eq(auto.Ledger.Score, 360 + (int)(12 * auto.Scoring.K)); Eq(auto.Ledger.EventCount, 3); Eq(auto.Ledger.N300, (ushort)1); Eq(auto.Combo, 2);
Pass("automatic config consumed by unchanged actual slider lifecycle without duplicate events");
Console.WriteLine($"R08-C02 MULTIPLIER CHECKS: {groups} passed (finite static predictions, no original runtime/human gameplay)");

LegacyScoreMetadata Read(IEnumerable<int> starts, string difficulty = "", string events = "") =>
    Raw(string.Join('\n', starts.Select(t => $"0,0,{t},1,0")), difficulty, events);
LegacyScoreMetadata Raw(string rows, string difficulty = "", string events = "", int version = 14, string extra = "") =>
    LegacyScoreMetadata.ReadFresh(($"[Difficulty]\n{difficulty}\n[Events]\n{events}\n{extra}\n[HitObjects]\n{rows}").Split('\n'), version);
string Hash(string file) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(fixtures, file)))).ToLowerInvariant();
ScoreLedger MixedScore(ScoreConfiguration config)
{
    var l = new ScoreLedger(config); Emit(l, 0, ScoreEventKind.CircleWhole, 1024);
    Emit(l, 1, ScoreEventKind.SliderHead, 64);
    Emit(l, 1, ScoreEventKind.SliderInterior, 8, SliderCheckpointKind.Interior);
    Emit(l, 1, ScoreEventKind.SliderInterior, 8, SliderCheckpointKind.Interior);
    Emit(l, 1, ScoreEventKind.SliderRepeatOrTerminal, 128, SliderCheckpointKind.Final);
    Emit(l, 1, ScoreEventKind.SliderAggregate, 1024);
    var observation = new SpinnerProgressObservation();
    for (int i = 1; i <= 16; i++) Emit(l, 2, ScoreEventKind.SpinnerProgress, observation.Observe(i, 15));
    Emit(l, 2, ScoreEventKind.SpinnerFinal, 1024); Emit(l, 3, ScoreEventKind.CircleWhole, 1024);
    return l;
}
ScoreLedger JoinedScore(ScoreConfiguration config)
{
    var l = new ScoreLedger(config); Emit(l, 0, ScoreEventKind.SliderHead, 64);
    for (int i = 0; i < 3; i++) Emit(l, 0, ScoreEventKind.SliderInterior, 8, SliderCheckpointKind.Interior);
    Emit(l, 0, ScoreEventKind.SliderRepeatOrTerminal, 128, SliderCheckpointKind.Final);
    Emit(l, 0, ScoreEventKind.SliderAggregate, 1024); return l;
}
void Emit(ScoreLedger l, int id, ScoreEventKind kind, int raw, SliderCheckpointKind? role = null)
{
    var family = kind == ScoreEventKind.CircleWhole ? ScoreObjectKind.Circle : kind is ScoreEventKind.SpinnerFinal or ScoreEventKind.SpinnerProgress ? ScoreObjectKind.Spinner : ScoreObjectKind.Slider;
    var result = l.Apply(new(new(id, family, 0, 3000), l.EventCount, kind, raw, true, family == ScoreObjectKind.Slider, ScorePolicy.PlainLegacy, role));
    Check(result.Status is ScoreAccountingStatus.Applied or ScoreAccountingStatus.NoEvent);
}
void Pass(string s) { groups++; Console.WriteLine("PASS C02 " + s); }
static void Check(bool condition) { if (!condition) throw new Exception("Control assertion failed."); }
static void Reject(Action action) { try { action(); } catch (InvalidDataException) { return; } throw new Exception("Unsupported metadata accepted."); }
static void Eq<T>(T actual, T expected) { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"Expected {expected}; got {actual}"); }
