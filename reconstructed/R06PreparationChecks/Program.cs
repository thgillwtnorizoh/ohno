// Bounded development checks against E32's finite preparation targets.
// No window, input, clock, judgement engine or original executable.
using System.Numerics;
using Ohno.CircleGame.Gameplay;

if (args.Length != 1) throw new ArgumentException("Pass the repository root.");
Vector2[] quadratic = [new(0, 0), new(1, 1), new(2, 0)];
Vector2[] quadraticExpected = [new(0, 0), new(.5f, .375f), new(1, .5f), new(1.5f, .375f), new(2, 0)];
Vector2[] cubic = [new(0, 0), new(1, 1), new(2, 1), new(3, 0)];
Vector2[] cubicExpected = [new(0, 0), new(.5f, .421875f), new(1, .671875f), new(1.5f, .75f),
    new(2, .671875f), new(2.5f, .421875f), new(3, 0)];
Same(SliderGeometryPreparation.FlattenBezier(quadratic), quadraticExpected, "quadratic raw E32 vertices");
Same(SliderGeometryPreparation.FlattenBezier(cubic), cubicExpected, "cubic raw E32 vertices");
var joined = SliderGeometryPreparation.Prepare(Vector2.Zero,
    [new(0, 0), new(1, 1), new(2, 0), new(2, 0), new(3, -1), new(4, 0)], true, 4);
Vector2[] joinedExpected = [..quadraticExpected, new(2.5f, -.375f), new(3, -.5f), new(3.5f, -.375f), new(4, 0)];
Same(new[] { joined.RawSegments[0].From }.Concat(joined.RawSegments.Select(s => s.To)).ToArray(), joinedExpected, "joined raw E32 vertices");
if (joined.RawSegments.Count(s => s.IsSubcurveEnd) != 2 || joined.Controls.Count != 6)
    throw new InvalidOperationException("Joined boundary/control preservation failed.");
var finalPair = SliderGeometryPreparation.Prepare(Vector2.Zero,
    [new(0, 0), new(1, 1), new(2, 0), new(2, 0)], true, 2);
if (finalPair.RawSegments.Count(s => s.IsSubcurveEnd) != 1 || finalPair.Controls.Count != 4)
    throw new InvalidOperationException("Final pair incorrectly separated/deduplicated.");
Console.WriteLine("PASS final-pair guard and preserved controls");
Console.WriteLine("R06 PREPARATION TARGETS: 4 passed (static research oracle, not original runtime)");

foreach (string fixture in new[] { "bezier-quadratic", "bezier-cubic", "bezier-joined", "bezier-mixed" })
{
    Beatmap map = Beatmap.Load(Path.Combine(args[0], "reconstructed", "CircleGame", "Fixtures", fixture + ".osu"));
    Console.WriteLine($"LOAD {fixture}: circles={map.Circles.Count}, sliders={map.Sliders.Count}, " +
        $"segments={map.Sliders.Sum(s => s.Geometry.Segments.Count)}, movements={map.Sliders.Sum(s => s.Movements.Count)}");
}
Console.WriteLine("R06 FIXTURE LOADS: 4 completed (parser/preparation only)");

static void Same(IReadOnlyList<Vector2> actual, IReadOnlyList<Vector2> expected, string name)
{
    if (!actual.SequenceEqual(expected)) throw new InvalidOperationException(name + ": unexpected raw vertices");
    Console.WriteLine("PASS " + name);
}
