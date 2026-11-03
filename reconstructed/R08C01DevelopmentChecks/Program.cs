using System.Numerics;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Rendering;

int groups = 0;
var spinner = new SpinnerState(new(10000, 13000, 0, null), 5);
foreach (int t in new[] { 0, 8799, 8800, 9999 }) Eq(SpinnerPresentation.IsVisible(spinner, t), false);
Eq(SpinnerPresentation.IsVisible(spinner, 10000), true);
Eq(SpinnerPresentation.IsVisible(spinner, 13000), true);
Eq(SpinnerPresentation.IsVisible(spinner, 13001), true); // Decision has not run yet.
Eq(spinner.Complete(13001), true);
Eq(SpinnerPresentation.IsVisible(spinner, 13001), false);
Pass("visibility at start, exact end, pending decision and completed removal");

string path = Path.Combine(Path.GetTempPath(), "ohno-c01-" + Guid.NewGuid() + ".osu");
try
{
    File.WriteAllText(path, Map(8000, 160)); // End9600; old approach preview began8800.
    var game = new CircleSession(Beatmap.Load(path), ScoreConfiguration.EngineeringPractice);
    var s = game.Sliders.Single();
    Eq(s.Definition.EndTime, 9600);
    game.Press(s.Definition.Head.Position, 8000);
    foreach (int t in new[] { 8000, 8700, 8800, 9000, s.Definition.TerminalTime, 9600 })
    {
        game.Advance(t, s.Definition.BallPosition(t), 1);
        Eq(SpinnerPresentation.IsVisible(game.Spinners[0], t), false);
        Eq(s.Tracking, true);
        Eq(s.FollowEligible, true);
        Eq(s.EntryTime, 8000);
    }
    Eq(s.Head.Result, Judgement.Hit300); Eq(s.Result, Judgement.Hit300);
    Eq(s.TerminalSucceeded, true); Eq(game.Combo, 2); Eq(game.Ledger.Score, 372);
    Eq(game.Ledger.EventCount, 3); Eq(game.Spinners[0].Progress, 0f);
    Pass("held preceding slider survives old spinner preview boundary with unchanged ledger");

    File.WriteAllText(path, Map(8900, 100)); // Head itself inside old preview; end9900.
    var entry = new CircleSession(Beatmap.Load(path), ScoreConfiguration.EngineeringPractice);
    entry.SetHeldButtons(1);
    entry.Press(entry.Sliders[0].Definition.Head.Position, 8900);
    entry.Advance(8900, entry.Sliders[0].Definition.Head.Position, 1);
    Eq(entry.Sliders[0].Head.Result, Judgement.Hit300); Eq(entry.Sliders[0].Tracking, true);
    Eq(entry.Ledger.EventCount, 1); Eq(entry.Spinners[0].Observation.Count, 0);
    Pass("future spinner does not block physical-input component contract at preceding head");

    int events = entry.Ledger.EventCount;
    for (int i = 0; i < 50; i++) SpinnerPresentation.IsVisible(entry.Spinners[0], 10000);
    Eq(entry.Ledger.EventCount, events); Eq(entry.Sliders[0].Tracking, true);
    Eq(entry.Spinners[0].PreviousAngle, (double?)null);
    Pass("presentation queries have no input, motion, combo or score side effects");
}
finally { File.Delete(path); }
Console.WriteLine($"R08-C01 DEVELOPMENT CHECKS: {groups} passed (component calls only; no game window or OS input)");

static string Map(int start, int length) => $"""
osu file format v14
[General]
Mode:0
[Difficulty]
HPDrainRate:0
CircleSize:5
OverallDifficulty:5
ApproachRate:5
SliderMultiplier:1
SliderTickRate:0.5
[TimingPoints]
0,1000,4,1,0,100,1,0
[HitObjects]
160,192,{start},2,0,L|{160 + length}:192,1,{length}
256,192,10000,8,0,13000
""";
void Pass(string s) { groups++; Console.WriteLine("PASS C01 " + s); }
static void Eq<T>(T actual, T expected) { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"Expected {expected}; got {actual}"); }
