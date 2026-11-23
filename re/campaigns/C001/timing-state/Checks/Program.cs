using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Selection;

// Independent expectations are sealed before execution by build_fixtures.py.
// Every compiled gameplay, catalog and asset class is the unchanged production file.
class Program
{
    static readonly List<object> assertions = [];
    static readonly List<object> outcomes = [];
    static int failed;
    static string current = "";
    static void Check(string name, bool ok, object? observed = null)
    {
        assertions.Add(new { fixture = current, name, passed = ok, observed });
        if (!ok) { failed++; Console.WriteLine($"FAIL {current}: {name} {JsonSerializer.Serialize(observed)}"); }
    }
    static string Hash(byte[] b) => Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
    static readonly CircleSoundPolicy Policy = new(true, false, true, 0, 100, false, 100, 100);
    static int At(JsonElement c, int time)
    {
        int selected = -1, i = 0;
        foreach (var p in c.GetProperty("points").EnumerateArray())
        { if (p.GetProperty("time").GetDouble() <= time) selected = i; i++; }
        return selected >= 0 ? selected : i > 0 ? 0 : -1;
    }
    static int[] Sample(JsonElement c, int query)
    {
        int i = At(c, query);
        return i < 0 ? [] : c.GetProperty("points")[i].GetProperty("sample").EnumerateArray().Select(v => v.GetInt32()).ToArray();
    }
    static void Near(string name, double actual, double expected) => Check(name,
        Math.Abs(actual - expected) <= 1e-9 * Math.Max(1, Math.Abs(expected)), new { actual, expected });
    static async Task<int> Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        string packet = Path.GetFullPath(args[0]);
        string temp = Path.Combine(Path.GetTempPath(), "ohno-issue22-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temp);
        using var manifest = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(packet, "manifest.json")));
        byte[] wav = File.ReadAllBytes(Path.Combine(packet, "resources/tone.wav"));
        string pack = Path.Combine(temp, "pack"); Directory.CreateDirectory(pack);
        string[] banks = ["normal", "soft", "drum"];
        string[] families = ["hitnormal", "hitwhistle", "hitfinish", "hitclap", "slidertick", "sliderslide", "sliderwhistle"];
        foreach (var bank in banks) foreach (var family in families)
            File.WriteAllBytes(Path.Combine(pack, $"{bank}-{family}.wav"), wav);
        current = "packet"; Check("resource hash", Hash(wav) == manifest.RootElement.GetProperty("resource_sha256").GetString());
        try
        {
            foreach (var c in manifest.RootElement.GetProperty("cases").EnumerateArray())
            {
                current = c.GetProperty("id").GetString()!;
                int before = failed;
                byte[] input = File.ReadAllBytes(Path.Combine(packet, c.GetProperty("path").GetString()!));
                Check("sealed fixture hash", Hash(input) == c.GetProperty("sha256").GetString());
                string dir = Path.Combine(temp, current); Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, "control.osu"); File.WriteAllBytes(path, input);
                // Real author-owned wave resources, loaded into the production sample catalog.
                foreach (var bank in banks) foreach (var family in new[] { "hitnormal", "slidertick" })
                    foreach (int index in new[] { 1, 2 })
                        File.WriteAllBytes(Path.Combine(dir, $"{bank}-{family}{(index == 1 ? "" : "2")}.wav"), wav);
                var doc = OsuDocument.Read(input);
                Check("canonical lossless bytes", doc.CopySourceBytes()!.SequenceEqual(input));
                Check("canonical timing encounter order", doc.Entries.Where(e => e.Section == "[TimingPoints]").Select(e => e.Text)
                    .SequenceEqual(c.GetProperty("rows").EnumerateArray().Select(r => r.GetString()!)));
                bool reject = c.GetProperty("reject").ValueKind != JsonValueKind.Null;
                Beatmap? map = null;
                try { map = Beatmap.LoadCanonical(path); Check("canonical admission", !reject); }
                catch (InvalidDataException e)
                { Check("explicit engineering rejection", reject && e.Message.Contains(c.GetProperty("reject").GetString()!, StringComparison.OrdinalIgnoreCase), e.Message); }
                using var catalog = new LocalChartCatalog([dir]);
                var snapshot = await catalog.RefreshAsync();
                Check("catalog indexes one chart", snapshot?.Charts.Count == 1);
                var chart = snapshot!.Charts.Single();
                if (map is null)
                {
                    bool refused = !chart.Available;
                    if (chart.Available) try { chart.LoadCanonicalGameplay(); } catch (InvalidDataException) { refused = true; }
                    Check("catalog canonical refusal", reject && refused);
                    outcomes.Add(new { id = current, disposition = "ENGINEERING REJECT", failures = failed - before });
                    continue;
                }
                try
                {
                    var catalogMap = chart.LoadCanonicalGameplay();
                    Check("catalog canonical connected timing", catalogMap.Sliders.Select(s => (s.TravelRate, s.EndTime))
                        .SequenceEqual(map.Sliders.Select(s => (s.TravelRate, s.EndTime))));
                    var legacy = Beatmap.Load(path);
                    Check("legacy unchanged timing control", legacy.Sliders.Select(s => (s.TravelRate, s.EndTime))
                        .SequenceEqual(map.Sliders.Select(s => (s.TravelRate, s.EndTime))));
                    var expected = c.GetProperty("expected").EnumerateArray().ToArray();
                    Check("parent registration count", map.Sliders.Count == expected.Length);
                    Check("fresh health/source owner", map.HealthSource is not null && map.HealthSource.IsFreshFor(map));
                    bool soundError = c.GetProperty("sample_error").GetBoolean();
                    Check("sample error classification", (map.CircleSounds!.TimingError is not null) == soundError, map.CircleSounds.TimingError);
                    if (!soundError)
                    {
                        var points = c.GetProperty("points").EnumerateArray().ToArray();
                        Check("sample timing encounter order and independent fields", points.Length == map.CircleSounds.Timing.Count &&
                            points.Select((p, i) => (p.GetProperty("time").GetDouble(), p.GetProperty("sample")[0].GetInt32(), p.GetProperty("sample")[1].GetInt32(), p.GetProperty("sample")[2].GetInt32(), i))
                            .SequenceEqual(map.CircleSounds.Timing.Select(p => (p.Time, p.Set, p.Index, p.Volume, p.SourceOrder))));
                    }
                    for (int n = 0; n < expected.Length; n++)
                    {
                        var s = map.Sliders[n]; var e = expected[n];
                        Near("original-derived travel rate", s.TravelRate, e.GetProperty("speed").GetDouble());
                        Check("absolute end truncation", s.EndTime == e.GetProperty("end").GetInt32(), new { actual = s.EndTime, expected = e.GetProperty("end").GetInt32() });
                        Check("independent checkpoint dates", s.Checkpoints.Select(p => p.Time).SequenceEqual(e.GetProperty("dates").EnumerateArray().Select(p => p.GetInt32())), s.Checkpoints.Select(p => p.Time).ToArray());
                        Check("checkpoint kinds", s.Checkpoints.Select(p => p.Kind.ToString()).SequenceEqual(e.GetProperty("kinds").EnumerateArray().Select(p => p.GetString())));
                        Check("slider sample metadata parent binding", s.Samples is not null && s.Samples.Schedule.SequenceEqual(s.Checkpoints.Select(p => p.Time)));
                        if (!soundError && map.CircleSounds.Timing.Count > 0)
                        {
                            foreach (int query in e.GetProperty("sample_queries").EnumerateArray().Select(p => p.GetInt32()))
                            {
                                var selected = CircleSampleSelection.At(map.CircleSounds.Timing, query)!;
                                int[] want = Sample(c, query);
                                Check("raw-time query identity", selected.SourceOrder == At(c, query) && new[] { selected.Set, selected.Index, selected.Volume }.SequenceEqual(want), new { query, selected });
                            }
                        }
                    }
                    using var library = new HitSoundLibrary(new() { DefaultPackDirectory = pack });
                    using var effects = await library.PrepareAsync(map, CancellationToken.None);
                    Check("resource owner is canonical map", ReferenceEquals(effects.PreparedMap, map));
                    string? continuous = HitSoundLibrary.ContinuousAdmission(map);
                    bool disordered = map.CircleSounds.Timing.Zip(map.CircleSounds.Timing.Skip(1)).Any(p => p.First.Time > p.Second.Time);
                    bool unusual = map.CircleSounds.Timing.Any(p => p.Set is < 0 or > 3 || p.Index < 0);
                    bool excluded = map.CircleSounds.Format < 14 || soundError || map.CircleSounds.Timing.Count == 0 || disordered || unusual;
                    Check("continuing profile limitations retained", (continuous is not null) == excluded, continuous);
                    if (!soundError && map.CircleSounds.Timing.Count > 0)
                    {
                        var session = CircleSession.CreateFreshOrdinary(map, ScoreConfiguration.FromBeatmap(map), false);
                        var sink = new Sink(); var events = new List<SliderSoundEvent>();
                        session.SliderSound += ev =>
                        {
                            events.Add(ev);
                            int at = sink.Requests.Count;
                            SliderSampleSelection.Request(ev, effects.Sliders[ev.Parent], effects.Source.Timing, effects.Resources, Policy, sink);
                            int parent = Array.FindIndex(map.Sliders.ToArray(), s => ReferenceEquals(s, ev.Parent));
                            var dates = expected[parent].GetProperty("dates").EnumerateArray().Select(x => x.GetInt32()).ToArray();
                            var kinds = expected[parent].GetProperty("kinds").EnumerateArray().Select(x => x.GetString()).ToArray();
                            int query = ev.Kind == SliderSoundKind.Head ? ev.Parent.Head.StartTime + 5 :
                                ev.Kind == SliderSoundKind.Tick ? dates[ev.Index] + 2 :
                                ev.Index >= ev.Parent.Traversals || ev.Index == -1 ? expected[parent].GetProperty("end").GetInt32() + 5 :
                                dates.Where((_, i) => kinds[i] != "Interior").ElementAt(ev.Index - 1) + 5;
                            var want = Sample(c, query);
                            int volume = ev.Kind == SliderSoundKind.Tick ? want[2] : (int)(Math.Max(8, want[2]) * .8);
                            Check("session event emits actual sample request", sink.Requests.Count == at + 1, new { kind = ev.Kind.ToString(), query });
                            foreach (var request in sink.Requests.Skip(at))
                            {
                                Check("multi-parent event ownership", ReferenceEquals(request.Event?.Parent, ev.Parent) && ReferenceEquals(request.Source, ev.Parent.Head));
                                Check("request volume from independent sample query", request.Volume == volume, new { actual = request.Volume, expected = volume, query });
                                Check("sample catalog handle and real bytes", effects.Samples.ContainsKey(request.Handle) && Hash(effects.Samples[request.Handle].Bytes) == Hash(wav));
                                if (effects.Samples.TryGetValue(request.Handle, out var resource))
                                {
                                    string bank = want[0] == 2 || want[0] == 0 ? "soft" : want[0] == 3 ? "drum" : "normal";
                                    string family = ev.Kind == SliderSoundKind.Tick ? "slidertick" : "hitnormal";
                                    string suffix = want[1] == 2 ? "2" : "";
                                    Check("sample resource bank/index identity", Path.GetFileNameWithoutExtension(resource.Name) == $"{bank}-{family}{suffix}", resource.Name);
                                }
                            }
                        };
                        foreach (var s in map.Sliders)
                        {
                            session.Advance(s.Head.StartTime - 1, s.Head.Position, 0);
                            session.Press(s.Head.Position, s.Head.StartTime);
                            for (int t = s.Head.StartTime; t <= s.EndTime + 2; t++) session.Advance(t, s.RequiredPosition(t), 1);
                        }
                        Check("session head/tick/finish exercised per parent", map.Sliders.All(s =>
                            events.Any(ev => ReferenceEquals(ev.Parent, s) && ev.Kind == SliderSoundKind.Head) &&
                            events.Any(ev => ReferenceEquals(ev.Parent, s) && ev.Kind == SliderSoundKind.AggregateEdge)));
                        outcomes.Add(new { id = current, disposition = "ADMITTED", failures = failed - before,
                            continuous, events = events.Select(ev => new { kind = ev.Kind.ToString(), start = ev.Parent.Head.StartTime, ev.Index, ev.ScheduleIndex, ev.Time }),
                            requests = sink.Requests.Select(r => new { kind = r.Event?.Kind.ToString(), start = r.Event?.Parent.Head.StartTime, r.Volume, name = effects.Samples.GetValueOrDefault(r.Handle)?.Name }) });
                    }
                    else outcomes.Add(new { id = current, disposition = "ADMITTED/SOUND_UNSUPPORTED", failures = failed - before, continuous });
                }
                catch (Exception ex) { Check("connected execution completed", false, ex.ToString()); }
            }
            current = "global-cursor";
            var unsorted = new GlobalSampleTiming([new(0, 1, 1, 40, true, 0), new(900, 2, 2, 70, false, 1), new(200, 3, 1, 60, true, 2)]);
            unsorted.Advance(1000);
            Check("unsorted full scan last-list-position", unsorted.Current?.SourceOrder == 2);
            var equal = new GlobalSampleTiming([new(1000, 2, 2, 70, false, 0), new(1000, 1, 1, 40, true, 1)]);
            equal.Advance(995, true);
            Check("global explicit rescan base before inherited", equal.Points.Select(p => p.SourceOrder).SequenceEqual(new[] { 1, 0 }));
            Check("global strict equality rescan first fallback", equal.Current?.SourceOrder == 1);
            equal.Advance(995);
            Check("global forward consumes equality", equal.Current?.SourceOrder == 0);
            var blocked = new GlobalSampleTiming([new(0, 1, 1, 40, true, 0), new(2000, 2, 2, 70, false, 1), new(500, 3, 1, 60, false, 2)]);
            blocked.Advance(0); blocked.Advance(1000);
            Check("global future entry blocks forward scan", blocked.Current?.SourceOrder == 0);
            current = "conversion";
            Check("finite CLR truncation controls", (int)1000.9 == 1000 && (int)(-.9) == 0 && (int)(-1000.9) == -1000);
            var result = new { schema = "issue22-results-v1", runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                os = System.Runtime.InteropServices.RuntimeInformation.OSDescription, assertions = assertions.Count, passed = assertions.Count - failed, failed,
                cases = outcomes, checks = assertions, limitations = "Original CLR/x86/native runtime, Windows GUI, audible playback and physical input NOT RUN. Real synthetic PCM bytes and real production handles; recording sink only." };
            File.WriteAllText(args.Length > 1 ? args[1] : Path.Combine(packet, "results.json"), JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine($"Issue22: {assertions.Count - failed}/{assertions.Count} PASS, {failed} FAIL; {outcomes.Count} fixture dispositions recorded");
            return failed == 0 ? 0 : 1;
        }
        finally { Directory.Delete(temp, true); }
    }
    sealed class Sink : ISamplePlaybackSink
    {
        public List<SamplePlaybackRequest> Requests { get; } = [];
        public void Enqueue(SamplePlaybackRequest request) => Requests.Add(request);
    }
}
