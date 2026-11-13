using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Difficulty;

internal static class Program
{
    static readonly List<object> Results = [];
    static void Check(bool condition, string label)
    {
        if (!condition) throw new Exception("C001: " + label);
        Results.Add(new { label, passed = true }); Console.WriteLine("PASS C001 " + label);
    }
    static void Reject<T>(Action action, string label) where T : Exception
    { try { action(); } catch (T) { Check(true, label); return; } throw new Exception("Expected " + typeof(T).Name + ": " + label); }
    static string Map(int version, string objects) => $"osu file format v{version}\n[General]\nMode:0\nStackLeniency:0\n[Metadata]\nTitle:C001 Independent Control\nVersion:Control\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n{objects}\n";
    static int Main(string[] args)
    {
        string repo = args[0], tmp = Path.Combine(Path.GetTempPath(), "ohno-c001-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tmp);
        try
        {
            string text = Map(14, "64,64,1000,1,0\n128,128,2000,1,0");
            var doc = OsuDocument.Parse(text + "[Editor]\nBookmarks:100,200\n[Colours]\nCombo1:1,2,3\n[Unknown]\nFuture:data\n[Metadata]\nTitle:Second\n");
            Check(doc.FormatVersion == 14 && doc.Section("[HitObjects]").Count() == 2, "version and raw object registration count");
            Check(doc.Section("[Metadata]").Where(e => e.Key == "Title").Select(e => e.Value).SequenceEqual(new[] { "C001 Independent Control", "Second" }), "duplicates remain ordered and observable");
            Check(doc.Section("[Editor]").Single().Value == "100,200" && doc.Section("[Colours]").Single().Value == "1,2,3" && doc.Section("[Unknown]").Single().Text == "Future:data", "unused and unknown sections retained");
            Check(doc.Entries.All(e => doc.SourceLines[e.LineNumber - 1].Trim() == e.Text), "entry provenance points to exact source line");
            foreach (var encoding in new Encoding[] { new UTF8Encoding(true), Encoding.Unicode, Encoding.BigEndianUnicode })
            {
                byte[] bytes = encoding.GetPreamble().Concat(encoding.GetBytes(text)).ToArray();
                var decoded = OsuDocument.Read(bytes);
                Check(decoded.FormatVersion == 14 && decoded.SourceSha256 == Convert.ToHexString(SHA256.HashData(bytes)) && decoded.Section("[HitObjects]").Count() == 2, "encoding and exact byte hash " + encoding.WebName);
            }
            foreach (string newline in new[] { "\n", "\r\n", "\r" })
                Check(OsuDocument.Parse(text.Replace("\n", newline)).Entries.Count == OsuDocument.Parse(text).Entries.Count, "line ending " + JsonSerializer.Serialize(newline));
            Reject<InvalidDataException>(() => OsuDocument.Parse("[General]\nMode:0"), "missing header rejects");
            using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
            Reject<OperationCanceledException>(() => OsuDocument.Parse(text, cancelled.Token), "syntax cancellation");
            string path = Path.Combine(tmp, "map.osu"); File.WriteAllText(path, text);
            Reject<OperationCanceledException>(() => Beatmap.Load(path, cancelled.Token), "gameplay cancellation propagates");
            var chart = LocalChartCatalog.Index(path); var map = chart.LoadGameplay();
            Check(map.Document?.SourceSha256 == chart.Fingerprint?.Sha256 && map.Circles.Count == 2, "catalog and prepared gameplay byte provenance");
            var admitted = StarAdmission.Parse(text);
            Check(admitted.Circles.Count == map.Circles.Count && admitted.Circles[0].X == map.Circles[0].Position.X && admitted.Circles[1].Start == map.Circles[1].StartTime, "star and gameplay consume consistent circle syntax");
            Reject<UnsupportedStarProfileException>(() => StarAdmission.Parse(text.Replace("Mode:0", "Mode:0\nMode:0")), "star duplicate admission retained");
            Reject<UnsupportedStarProfileException>(() => StarAdmission.Parse("\n" + text), "star strict first-line profile retained");
            File.WriteAllText(path, text.Replace("1000,1,0", "1100,1,0"));
            Reject<InvalidDataException>(() => chart.LoadGameplay(), "stale revision cannot load");
            var updated = LocalChartCatalog.Index(path).LoadGameplay();
            Check(updated.Document?.SourceSha256 != map.Document?.SourceSha256 && updated.Circles[0].StartTime == 1100, "new revision receives fresh document");
            Vector2[] controls = [new(0,0), new(1,1), new(2,0)];
            for (int version = 5; version <= 14; version++)
            {
                var geometry = SliderGeometryPreparation.Prepare(Vector2.Zero, controls, true, 2, version);
                Check(version == 9 ? geometry.RawSegments.Count == 149 && geometry.RawSegments[^1].To.X < 2 : geometry.RawSegments.Count == 4 && geometry.RawSegments[^1].To == new Vector2(2,0), $"E032 v{version} raw sampler branch");
                File.WriteAllText(path, Map(version, "0,0,1000,2,0,B|1:1|2:0,1,2"));
                var parsed = Beatmap.Load(path);
                Check(parsed.Sliders.Single().Geometry.RawSegments.Count == geometry.RawSegments.Count && parsed.ScoreMetadata?.FormatVersion == version, $"v{version} Bezier reaches gameplay and score preparation");
            }
            Vector2[] joined = [new(0,0),new(1,1),new(2,0),new(2,0),new(3,-1),new(4,0)];
            var old = SliderGeometryPreparation.Prepare(Vector2.Zero, joined, true, 4, 6);
            var modern = SliderGeometryPreparation.Prepare(Vector2.Zero, joined, true, 4, 14);
            Check(old.RawSegments.Count(s => s.IsSubcurveEnd) == 2 && modern.RawSegments.Count(s => s.IsSubcurveEnd) == 2 && !old.RawSegments.SequenceEqual(modern.RawSegments), "v6 versus modern duplicate-boundary geometry differs");
            var pair7 = SliderGeometryPreparation.Prepare(Vector2.Zero, [Vector2.Zero,new(2,0)], true, 2, 7);
            var pair9 = SliderGeometryPreparation.Prepare(Vector2.Zero, [Vector2.Zero,new(2,0)], true, 2, 9);
            Check(!pair7.RawSegments.Single().IsLinear && pair9.RawSegments.Single().IsLinear, "two-control linear bypass only after v8");
            foreach (int version in Enumerable.Range(5,10))
            {
                File.WriteAllText(path, Map(version, "0,0,1000,2,0,L|100:0|100:100,1,250"));
                var linear = Beatmap.Load(path).Sliders.Single();
                Check(linear.Geometry.RawSegments.Count == 2 && linear.Geometry.RawSegments.All(s => s.IsLinear && s.IsSubcurveEnd) && linear.Tail == new Vector2(100,150) && linear.EndTime == 2250, $"E006 v{version} polyline extension and end time");
            }
            File.WriteAllText(path, Map(14, "0,0,1000,2,0,L|100.9:10.9,1,100"));
            Check(Beatmap.Load(path).Sliders.Single().ControlPoints[^1] == new Vector2(100,10), "E006 L16 control truncation before Single storage");
            foreach (string fixture in Directory.EnumerateFiles(Path.Combine(repo, "reconstructed/CircleGame/Fixtures"), "*.osu"))
            {
                var loaded = Beatmap.Load(fixture);
                Check(loaded.Document is not null && loaded.Document.SourceSha256 is not null, "bundled fixture document " + Path.GetFileName(fixture));
            }
            using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(repo,"reconstructed/campaigns/C001/corpus-manifest.json")));
            foreach (var fixture in manifest.RootElement.GetProperty("cases").EnumerateArray())
            {
                string fixturePath = Path.Combine(repo,"reconstructed/campaigns/C001",fixture.GetProperty("file").GetString()!);
                byte[] bytes = File.ReadAllBytes(fixturePath);
                var raw = OsuDocument.Read(bytes);
                Check(raw.SourceSha256!.Equals(fixture.GetProperty("sha256").GetString(), StringComparison.OrdinalIgnoreCase), "corpus hash " + Path.GetFileName(fixturePath));
                if (fixture.GetProperty("expected_gameplay").GetBoolean())
                {
                    var prepared = Beatmap.Load(fixturePath);
                    if (fixture.GetProperty("expected_prepared").TryGetProperty("end",out var expectedEnd))
                        Check(prepared.Sliders.Single().EndTime == expectedEnd.GetInt32() && prepared.Sliders.Single().Tail == new Vector2(228,342), "corpus polyline prepared " + Path.GetFileName(fixturePath));
                    else Check(prepared.Circles.Count + prepared.Sliders.Count == 1, "corpus gameplay " + Path.GetFileName(fixturePath));
                }
                else Reject<InvalidDataException>(() => Beatmap.Load(fixturePath), "raw retained but unsupported gameplay " + Path.GetFileName(fixturePath));
            }
            string resultPath = Path.Combine(repo, "reconstructed/campaigns/C001/development-results.json");
            File.WriteAllText(resultPath, JsonSerializer.Serialize(new { classification = "Work 2 production development checks; not independent QA or original runtime", groups = Results.Count, results = Results }, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine($"C001 DEVELOPMENT: {Results.Count}/{Results.Count} PASS"); return 0;
        }
        finally { Directory.Delete(tmp, true); }
    }
}
