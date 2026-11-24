using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Selection;

class Program
{
    static readonly List<object> checks = [], recorded = [];
    static int failed;
    static string id = "";
    static readonly CircleSoundPolicy Policy = new(true, false, true, 0, 100, false, 100, 100);
    static void Check(string name, bool ok, object? actual = null)
    { checks.Add(new { id, name, passed = ok, actual }); if (!ok) { failed++; Console.WriteLine($"FAIL {id}: {name} {JsonSerializer.Serialize(actual)}"); } }
    static string Hash(byte[] bytes) => Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
    static void Throws<T>(string name, Action action) where T : Exception
    { try { action(); Check(name, false); } catch (T) { Check(name, true); } }
    static string Values(SelectedCircleSample p) => $"{p.BaseSet}/{p.Index}/{p.Volume}";
    static string Expected(JsonElement e) => $"{e.GetProperty("bank").GetInt32()}/{e.GetProperty("index").GetInt32()}/{e.GetProperty("volume").GetInt32()}";
    static SelectedCircleSample Select(SliderDefinition s, PreparedSliderSamples p, IReadOnlyList<SampleTiming> t, int k) => k switch
    { 0 => CircleSampleSelection.Select(p.Head, CircleSampleSelection.Timing(t, s.Head.StartTime)!, Policy),
      1 => SliderSampleSelection.Tick(p.Metadata, t, 0), _ => SliderSampleSelection.Edge(s, p.Metadata, t, Policy, s.Traversals) };
    static async Task<int> Main(string[] args)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        string root = Path.GetFullPath(args[0]), output = Path.GetFullPath(args[1]); Directory.CreateDirectory(output);
        string parent = Path.Combine(root, "re/campaigns/C001/timing-state"), authority = Path.Combine(root, "re/campaigns/C001/timing-ownership/conditional-oracle.json");
        string temp = Path.Combine(Path.GetTempPath(), "ohno-issue24-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(temp);
        using var oracle = JsonDocument.Parse(File.ReadAllBytes(authority));
        using var oldManifest = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(parent, "manifest.json")));
        byte[] wave = File.ReadAllBytes(Path.Combine(parent, "resources/tone.wav"));
        string pack = Path.Combine(temp, "pack"); Directory.CreateDirectory(pack);
        foreach (string bank in new[] { "normal", "soft", "drum" }) foreach (string family in new[] { "hitnormal", "hitwhistle", "hitfinish", "hitclap", "slidertick", "sliderslide", "sliderwhistle" })
            File.WriteAllBytes(Path.Combine(pack, $"{bank}-{family}.wav"), wave);
        try
        {
            foreach (var c in oracle.RootElement.GetProperty("cases").EnumerateArray())
            {
                id = c.GetProperty("id").GetString()!;
                byte[] bytes = File.ReadAllBytes(Path.Combine(parent, c.GetProperty("path").GetString()!));
                Check("immutable original fixture", Hash(bytes) == c.GetProperty("sha256").GetString());
                string dir = Path.Combine(temp, id); Directory.CreateDirectory(dir); string file = Path.Combine(dir, "control.osu"); File.WriteAllBytes(file, bytes);
                foreach (string bank in new[] { "normal", "soft", "drum" }) foreach (string family in new[] { "hitnormal", "slidertick" }) foreach (string suffix in new[] { "", "2" })
                    File.WriteAllBytes(Path.Combine(dir, $"{bank}-{family}{suffix}.wav"), wave);
                var map = Beatmap.LoadCanonical(file); var source = map.CircleSounds!; var initialized = map.TimingInitialization!;
                Check("fresh initialized owner shared with samples", initialized is not null && ReferenceEquals(initialized, source.Initialization) && ReferenceEquals(initialized.Samples, source.Timing));
                Check("lossless raw document", map.Document!.CopySourceBytes()!.SequenceEqual(bytes));
                Check("raw encounter source positions", source.RawTiming.Select(p => p.SourceOrder).SequenceEqual(c.GetProperty("source_order").EnumerateArray().Select(v => v.GetInt32())));
                Check("raw point time and flag provenance", initialized!.RawPoints.Select(p => (p.Time, p.BeatLength, p.IsBase)).SequenceEqual(c.GetProperty("points").EnumerateArray().Select(p => (p.GetProperty("time").GetDouble(), p.GetProperty("beat").GetDouble(), p.GetProperty("base").GetBoolean()))));
                Check("sample record identity follows raw SourceOrder", source.Timing.All(p => ReferenceEquals(p, source.RawTiming[p.SourceOrder])));
                Throws<NotSupportedException>("raw view read-only", () => ((IList<SampleTiming>)source.RawTiming)[0] = source.RawTiming[0]);
                Throws<NotSupportedException>("initialized view read-only", () => ((IList<SampleTiming>)source.Timing)[0] = source.Timing[0]);
                var domains = c.GetProperty("after_order_domain").EnumerateArray().Select(d => d.EnumerateArray().Select(x => x.GetInt32()).ToArray()).ToArray();
                Check("initialized comparator domain", domains.Any(d => d.SequenceEqual(initialized.SourcePositions)), initialized.SourcePositions);
                bool ambiguous = c.GetProperty("equal_key_order_unresolved").GetBoolean();
                Check("equal-key ambiguity explicitly recorded", initialized.HasAmbiguousKeys == ambiguous);
                var legacy = Beatmap.Load(file);
                Check("legacy supplied encounter profile retained", legacy.TimingInitialization is null && legacy.CircleSounds!.Timing.Select(p => p.SourceOrder).SequenceEqual(c.GetProperty("source_order").EnumerateArray().Select(x => x.GetInt32())));
                var originals = oldManifest.RootElement.GetProperty("cases").EnumerateArray().Single(x => x.GetProperty("id").GetString() == id);
                Check("legacy prior rate/end retained", legacy.Sliders.Select(s => (s.TravelRate, s.EndTime)).SequenceEqual(originals.GetProperty("expected").EnumerateArray().Select(e => (e.GetProperty("speed").GetDouble(), e.GetProperty("end").GetInt32()))));
                bool rawDisordered = source.RawTiming.Zip(source.RawTiming.Skip(1)).Any(p => p.First.Time > p.Second.Time);
                Check("raw out-of-order continuing fence", (HitSoundLibrary.ContinuousAdmission(map) is not null) == rawDisordered, HitSoundLibrary.ContinuousAdmission(map));
                var global = new GlobalSampleTiming(source.Timing, initializeAtFirstClock: true);
                var before = map.Sliders.Select(s => (s.TravelRate, s.EndTime)).ToArray();
                global.Advance(map.Sliders[0].Head.StartTime - 6); global.Advance(map.Sliders[0].Head.StartTime, rescan: true);
                Check("late rescan never recomputes geometry", before.SequenceEqual(map.Sliders.Select(s => (s.TravelRate, s.EndTime))));
                Check("late rescan leaves raw owner unchanged", source.RawTiming.Select(p => p.SourceOrder).SequenceEqual(c.GetProperty("source_order").EnumerateArray().Select(v => v.GetInt32())));
                if (ambiguous) continue; // These are conditional domains, NOT exact original sort passes.
                using var library = new HitSoundLibrary(new() { DefaultPackDirectory = pack });
                using var effects = await library.PrepareAsync(map, CancellationToken.None);
                Check("resource owner is fresh canonical map", ReferenceEquals(effects.PreparedMap, map));
                for (int n = 0; n < map.Sliders.Count; n++)
                {
                    var s = map.Sliders[n]; var e = c.GetProperty("parents")[n].GetProperty("after_sort_variants")[0];
                    Check("independent initialized rate", s.TravelRate == e.GetProperty("prepared_rate").GetDouble(), s.TravelRate);
                    Check("independent initialized end", s.EndTime == e.GetProperty("prepared_end").GetInt32(), s.EndTime);
                    Check("independent initialized checkpoint dates", s.Checkpoints.Select(p => p.Time).SequenceEqual(e.GetProperty("dates").EnumerateArray().Select(x => x.GetInt32())), s.Checkpoints.Select(p => p.Time).ToArray());
                    for (int k = 0; k < 3; k++) Check("independent head/tick/finish selection", Values(Select(s, effects.Sliders[s], source.Timing, k)) == Expected(e.GetProperty("samples")[k]));
                }
                var session = CircleSession.CreateFreshOrdinary(map, ScoreConfiguration.FromBeatmap(map), false);
                Check("session retains canonical map owner", ReferenceEquals(session.Map, map));
                var events = new List<SliderSoundEvent>();
                session.SliderSound += ev =>
                {
                    events.Add(ev); var sink = new Sink();
                    SliderSampleSelection.Request(ev, effects.Sliders[ev.Parent], effects.Source.Timing, effects.Resources, Policy, sink);
                    Check("session produces real resource request", sink.Items.Count == 1);
                    foreach (var request in sink.Items)
                    {
                        Check("tagged sample parent identity", ReferenceEquals(request.Event?.Parent, ev.Parent) && ReferenceEquals(request.Source, ev.Parent.Head));
                        Check("sample handle resolves real PCM", effects.Samples.TryGetValue(request.Handle, out var data) && Hash(data.Bytes) == Hash(wave));
                        recorded.Add(new { fixture = id, parent = ev.Parent.Head.StartTime, kind = ev.Kind.ToString(), request.Volume, name = Path.GetFileNameWithoutExtension(effects.Samples[request.Handle].Name) });
                    }
                };
                foreach (var s in map.Sliders)
                {
                    session.Advance(s.Head.StartTime - 1, s.Head.Position, 0); session.Press(s.Head.Position, s.Head.StartTime);
                    for (int t = s.Head.StartTime; t <= s.EndTime + 2; t++) session.Advance(t, s.RequiredPosition(t), 1);
                }
                Check("every parent head and aggregate finish exercised", map.Sliders.All(s => events.Any(e => ReferenceEquals(e.Parent, s) && e.Kind == SliderSoundKind.Head) && events.Any(e => ReferenceEquals(e.Parent, s) && e.Kind == SliderSoundKind.AggregateEdge)));
                if (id == "equal-inherited-base")
                {
                    string[] kinds = ["Head", "Tick", "AggregateEdge"], names = ["soft-hitnormal2", "soft-slidertick2", "soft-hitnormal2"]; int[] volumes = [56, 70, 56];
                    // Check the actual requests directly, not the presentation sequence.
                    for (int k = 0; k < 3; k++)
                    {
                        var sink = new Sink(); var ev = events.First(e => e.Kind.ToString() == kinds[k]);
                        SliderSampleSelection.Request(ev, effects.Sliders[ev.Parent], effects.Source.Timing, effects.Resources, Policy, sink);
                        Check("mandatory minimal PCM bank/index/volume", sink.Items.Count == 1 && Path.GetFileNameWithoutExtension(effects.Samples[sink.Items[0].Handle].Name) == names[k] && sink.Items[0].Volume == volumes[k]);
                    }
                    await CatalogAndCancellation(root, file, bytes, map);
                }
                effects.Dispose(); Check("disposed candidate clears map cache", effects.Resources.Caches.Map.Count == 0);
                using var replacement = await library.PrepareAsync(map, CancellationToken.None);
                Check("fresh resource generation after disposal", replacement.Samples.Count > 0 && ReferenceEquals(replacement.PreparedMap, map));
            }
            id = "boundaries";
            foreach (int version in new[] { 5, 7, 8, 13 })
            {
                var map = Beatmap.LoadCanonical(Path.Combine(parent, $"fixtures/version-{version}/control.osu"));
                Check("old-version fresh initialized state", map.TimingInitialization is not null);
                Check("old-version continuing exclusion", HitSoundLibrary.ContinuousAdmission(map)!.Contains("format14", StringComparison.Ordinal));
            }
            foreach (var (name, query, sourceOrder) in new[] { ("head-plus-five-boundary", 1005, 1), ("tick-plus-two-boundary", 1502, 1) })
            {
                var m = Beatmap.LoadCanonical(Path.Combine(parent, $"fixtures/{name}/control.osu"));
                Check("raw Double sample offset boundary", CircleSampleSelection.At(m.CircleSounds!.Timing, query)!.SourceOrder == sourceOrder);
            }
            var later = Beatmap.LoadCanonical(Path.Combine(parent, "fixtures/later-point-changes-samples-only/control.osu"));
            Check("later event changes sample not prepared speed", later.Sliders[0].TravelRate == 200 && later.Sliders[0].EndTime == 3000 && CircleSampleSelection.At(later.CircleSounds!.Timing, 1502)!.SourceOrder == 1);
            foreach (string name in new[] { "short-two-columns", "short-four-columns", "invalid-sample-bank" })
                Check("sample error remains explicit", Beatmap.LoadCanonical(Path.Combine(parent, $"fixtures/{name}/control.osu")).CircleSounds!.TimingError is not null);
            foreach (string name in new[] { "zero-base-rejected", "negative-base-rejected", "nonfinite-base-rejected", "invalid-flag-rejected", "missing-beat-rejected" })
                Throws<InvalidDataException>("historical malformed timing fence", () => Beatmap.LoadCanonical(Path.Combine(parent, $"fixtures/{name}/control.osu")));
            var strict = new GlobalSampleTiming([new(0, 1, 1, 40, true, 0), new(1000, 2, 2, 70, false, 1)], true);
            strict.Advance(995); Check("global initial rescan remains strict", strict.Current!.SourceOrder == 0);
            strict.Advance(995); Check("global forward consumes equality", strict.Current!.SourceOrder == 1);
            string adapter = File.ReadAllText(Path.Combine(root, "reconstructed/CircleGame/Platform/WindowsCircleEffects.cs"));
            Check("adapter source enables initialized clock route", adapter.Contains("initializeAtFirstClock: data.Source.Initialization is not null", StringComparison.Ordinal));
            var sourcePaths = new[] { "Gameplay/Beatmap.cs", "Gameplay/CircleHitSounds.cs", "Platform/LocalHitSoundAssets.cs", "Platform/WindowsCircleEffects.cs" };
            File.WriteAllText(Path.Combine(output, "results.json"), JsonSerializer.Serialize(new { schema = "issue24-development-v1", actor = "Work2 development; NOT independent Work1 QA", passed = checks.Count - failed, failed, executed = checks.Count,
                runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription, os = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                sourceCommit = Environment.GetEnvironmentVariable("GITHUB_SHA") ?? "local-hashed-candidate", sourceHashes = sourcePaths.ToDictionary(p => p, p => Hash(File.ReadAllBytes(Path.Combine(root, "reconstructed/CircleGame", p)))),
                authorityHash = Hash(File.ReadAllBytes(authority)), checks, recordedRequests = recorded, unambiguousControls = 12, ambiguousDomains = 2,
                limits = "Original runtime, audible/physical input and shown GUI NOT RUN; recording sinks and adapter source inspection. Equal-key domains do not establish original stable sort." }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine($"C001 TIMING INITIALIZATION: {checks.Count - failed}/{checks.Count} PASS, {failed} FAIL;12 exact controls/2 conditional domains;{recorded.Count} recorded requests");
            return failed == 0 ? 0 : 1;
        }
        finally { Directory.Delete(temp, true); }
    }
    static async Task CatalogAndCancellation(string root, string file, byte[] bytes, Beatmap initial)
    {
        using var catalog = new LocalChartCatalog([Path.GetDirectoryName(file)!]); var snap = await catalog.RefreshAsync(); var chart = snap!.Charts.Single();
        var selected = new SelectedChartState(); var model = new SelectPlayModel(selected); model.Rebuild(snap); model.Highlight(chart); Check("catalog selection commits", model.Commit());
        using var flow = new PlayEntryFlow(selected);
        Check("real selected chart captured for play", flow.Request() == StartDecision.Accepted && flow.Accept() && flow.Queue() && flow.Commit() && flow.Capture() && ReferenceEquals(flow.Captured, chart));
        long generation = flow.BeginLoading(); var loaded = flow.Captured!.LoadCanonicalGameplay(); Check("catalog captured canonical timing", loaded.Sliders[0].EndTime == 2200 && flow.Ready(generation));
        File.WriteAllText(file, System.Text.Encoding.UTF8.GetString(bytes).Replace("Title:", "Title:revision ", StringComparison.Ordinal));
        Throws<InvalidDataException>("stale catalog revision rejected", () => chart.LoadCanonicalGameplay());
        var fresh = (await catalog.RefreshAsync())!.Charts.Single(); selected.Set(fresh); Check("new revision initialized separately", fresh.LoadCanonicalGameplay().Sliders[0].EndTime == 2200 && selected.Revision >= 2);
        File.WriteAllBytes(file, bytes);
        using var cancelled = new CancellationTokenSource(); cancelled.Cancel(); Throws<OperationCanceledException>("pre-cancelled canonical load", () => Beatmap.LoadCanonical(file, cancelled.Token));
        var loader = typeof(Beatmap).GetMethod("LoadCore", BindingFlags.Static | BindingFlags.NonPublic)!;
        foreach (string stage in new[] { "timing-initialization", "timing-initialized" })
        {
            using var stop = new CancellationTokenSource(); bool reached = false, refused = false;
            Action<string, int, int> observe = (name, _, _) => { if (name == stage) { reached = true; stop.Cancel(); } };
            try { loader.Invoke(null, [file, stop.Token, true, observe]); } catch (TargetInvocationException e) when (e.InnerException is OperationCanceledException) { refused = true; }
            Check("cooperative initialization cancellation", reached && refused, stage);
        }
        Check("cancelled candidate cannot mutate previous owner", initial.Sliders[0].EndTime == 2200 && initial.HealthSource!.IsFreshFor(initial));
        string huge = Path.Combine(Path.GetDirectoryName(file)!, "huge.osu"); File.WriteAllBytes(huge, new byte[2 * 1024 * 1024 + 1]);
        Throws<InvalidDataException>("2MiB preflight retained", () => Beatmap.LoadCanonical(huge)); File.Delete(huge);
        string over = Path.Combine(Path.GetDirectoryName(file)!, "over.osu");
        File.WriteAllText(over, System.Text.Encoding.UTF8.GetString(bytes).Replace("L|456:192,1,400", "L|456:192,33,400", StringComparison.Ordinal));
        Throws<InvalidDataException>("canonical32 traversal fence retained", () => Beatmap.LoadCanonical(over));
        Throws<InvalidDataException>("legacy16 traversal fence retained", () => Beatmap.Load(over)); File.Delete(over);
    }
    sealed class Sink : ISamplePlaybackSink { public List<SamplePlaybackRequest> Items { get; } = []; public void Enqueue(SamplePlaybackRequest request) => Items.Add(request); }
}
