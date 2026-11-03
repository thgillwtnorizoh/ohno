// Bounded R07 development checks on linked components. No windows, OS events,
// autoplay, original execution, native media or independent QA05 framework.
using System.Numerics;
using Ohno.CircleGame.Gameplay;

if (args.Length != 1) throw new ArgumentException("Pass repository root.");
string directory = Path.Combine(args[0], "reconstructed", "CircleGame", "Fixtures");
Beatmap practice = Beatmap.Load(Path.Combine(directory, "spinner-practice.osu"));
Eq(practice.Spinners.Count, 1); Eq(practice.Circles.Count, 0); Eq(practice.Sliders.Count, 0);
var prepared = Fresh(); Eq(prepared.Required, 15); Near(prepared.Acceleration, .00108);
Eq(prepared.Definition.HitSample, "0:0:0:0:");
foreach (string name in new[] { "circle-spinner", "mixed-r07" })
    Eq(Beatmap.Load(Path.Combine(directory, name + ".osu")).Spinners.Count, 1);
Pass("parsing, metadata, three fixtures and OD5 preparation");

var angle = Fresh(); angle.SampleCursor(new(256, 193), SpinnerState.DisplayCenter, 3000, 16, true);
Near(angle.PreviousAngle!.Value, Math.PI / 2); Near(angle.TargetMotion, 0);
angle.SampleAngle(0, 3010, -1, true); Near(angle.PreviousAngle.Value, Math.PI / 2);
var adaptive = Fresh(); adaptive.SampleAngle(0, 3000, 100, true); adaptive.SampleAngle(.5, 3100, 100, true);
Near(adaptive.TargetMotion, .005);
var cadence = new CircleSession(practice);
cadence.Advance(3000, new(300,192), 1);
cadence.Advance(3008, new(256,236), 1); Near(cadence.Spinners[0].PreviousAngle!.Value, 0);
cadence.Advance(3017, new(256,236), 1); Near(cadence.Spinners[0].PreviousAngle!.Value, Math.PI / 2);
Pass("atan2 projection, nonlegacy first angle, negative interval and adaptive divisor");

var wrap = Fresh(); wrap.SampleAngle(3, 3000, 16, true); wrap.SampleAngle(-3, 3016, 16, true);
Near(wrap.TargetMotion, (2 * Math.PI - 6) / SpinnerState.InitialInterval);
var reverse = Fresh(); reverse.SampleAngle(-3, 3000, 16, true); reverse.SampleAngle(3, 3016, 16, true);
Near(reverse.TargetMotion, -(2 * Math.PI - 6) / SpinnerState.InitialInterval);
var antipode = Fresh(); antipode.SampleAngle(0, 3000, 16, true); antipode.SampleAngle(Math.PI, 3016, 16, true);
Near(antipode.TargetMotion, 0); Near(antipode.PreviousAngle!.Value, Math.PI);
Pass("signed wrapping and exact antipodal rejection with previous-angle update");

var stationary = Fresh(); stationary.SampleAngle(0, 3000, 16, true); stationary.SampleAngle(.5, 3016, 16, true);
double target = stationary.TargetMotion;
stationary.SampleAngle(.5, 3032, 16, false); Near(stationary.TargetMotion, target / 3);
stationary.SampleAngle(.5, 3048, 16, false); Near(stationary.TargetMotion, 0);
stationary.SampleAngle(.6, 3064, 16, false); Near(stationary.TargetMotion, 0); Near(stationary.PreviousAngle!.Value, .6);
Pass("stationary decay before hold gate and released angular reference");

var motion = Fresh(); motion.SampleAngle(0, 3000, 1, true); motion.SampleAngle(1, 3001, 1, true);
motion.Integrate(3000, 100); Eq(motion.Progress, 0f);
motion.Integrate(3100, 100); Near(motion.SmoothedMotion, .05); Eq(motion.Progress, 1f);
motion.Integrate(6000, 100); Eq(motion.Progress, 1f);
motion.SampleAngle(1.1, 3101, 1, false); motion.Integrate(3101, 1);
Near(motion.SmoothedMotion, .05 - .00108);
if (motion.Progress <= 1) throw new Exception("Release incorrectly erased residual motion.");
Pass("strict integration interval, acceleration, clamp, contribution cap and residual motion");

var observed = new SpinnerProgressObservation();
Eq(observed.Observe(.9f, 15), 0); Eq(observed.Count, 0);
Eq(observed.Observe(1.1f, 15), 0); Eq(observed.Count, 1);
Eq(observed.Observe(8.9f, 15), 8192); Eq(observed.Count, 2); Eq(observed.CachedInteger, 8);
Eq(observed.Observe(8.95f, 15), 0); Eq(observed.Count, 2);
Eq(observed.Observe(9f, 15), 4096); Eq(observed.Count, 3);
Pass("Single integer truncation, one-change accounting, first zero/second8192 and cache");

foreach (var c in new[] { (0,-131072), (2,-131072), (3,256), (13,256), (14,512), (15,512), (16,1024) })
    Eq(SpinnerState.ClassifyModern(c.Item1, 15), c.Item2);
Eq(SpinnerState.ClassifyModern(0, 0), 1024);
var completion = Fresh(); completion.SampleAngle(0, 3000, 1, true); completion.SampleAngle(1, 3001, 1, true);
completion.Integrate(3100, 100); Eq(completion.Progress, 1f); Eq(completion.Observation.Count, 0);
Eq(completion.Complete(6000), false); Eq(completion.Complete(6001), true);
Eq(completion.FinalRaw, -131072); Eq(completion.Observation.CachedInteger, 0); Eq(completion.Observation.Count, 0);
Eq(completion.Complete(6002), false);
var session = new CircleSession(practice); session.Press(SpinnerState.DisplayCenter, 3000); Eq(session.JudgedCount, 0);
session.Advance(6000); Eq(session.JudgedCount, 0); session.Advance(6001); Eq(session.JudgedCount, 1);
session.Advance(6002); Eq(session.JudgedCount, 1); Eq(new CircleSession(practice).Spinners[0].Progress, 0f);
Pass("modern final thresholds, strict completion, no drain, no head press and fresh session");

string original = File.ReadAllText(Path.Combine(directory, "spinner-practice.osu"));
foreach (string invalid in new[] {
    original.Replace("8,0,6000", "8,,6000"), original.Replace("8,0,6000", "8,0,3000"),
    original.Replace("8,0,6000", "8,0,63001"), original.Replace("v14", "v13"),
    original + "\n128,192,4000,1,0\n" })
{
    string path = Path.Combine(Path.GetTempPath(), "ohno-r07-" + Guid.NewGuid().ToString("N") + ".osu");
    try
    {
        File.WriteAllText(path, invalid); bool rejected = false;
        try { Beatmap.Load(path); } catch (InvalidDataException) { rejected = true; }
        if (!rejected) throw new Exception("Unsupported spinner input accepted.");
    }
    finally { File.Delete(path); }
}
Pass("parser rejection of malformed sound, invalid duration, old format and overlap");
Console.WriteLine("R07 DEVELOPMENT CHECKS: 8 passed (finite static-oracle/component checks; no human gameplay)");

SpinnerState Fresh() => new(practice.Spinners[0], 5);
static void Eq<T>(T actual, T expected) { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"Expected {expected}, got {actual}."); }
static void Near(double actual, double expected) { if (Math.Abs(actual - expected) > 1e-12) throw new Exception($"Expected near {expected}, got {actual}."); }
static void Pass(string name) => Console.WriteLine("PASS R07 " + name);
