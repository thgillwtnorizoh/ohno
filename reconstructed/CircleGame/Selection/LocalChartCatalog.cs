using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Selection;

// ENGINEERING local index, not osu!.db or the original filename-only comparator.
public sealed record SelectionSettings
{
    public string BeatmapDirectory { get; init; } = "Songs";
    public bool PreferUnicode { get; init; }
    public static string ConfigurationPath(string? applicationDirectory = null) => Path.Combine(applicationDirectory ?? AppContext.BaseDirectory, "selection.json");
    public string SongsRoot(string applicationDirectory) => CatalogPaths.Canonical(Path.IsPathRooted(BeatmapDirectory) ? BeatmapDirectory : Path.Combine(applicationDirectory, BeatmapDirectory));
    public static SelectionSettings Read(string path)
    {
        if (!File.Exists(path)) return new();
        if (new FileInfo(path).Length > 16384) throw new InvalidDataException("Selection settings exceed16 KiB.");
        var settings = JsonSerializer.Deserialize<SelectionSettings>(File.ReadAllText(path)) ?? throw new InvalidDataException("Empty selection settings.");
        if (string.IsNullOrWhiteSpace(settings.BeatmapDirectory)) throw new InvalidDataException("BeatmapDirectory must be nonempty.");
        return settings;
    }
}
public static class CatalogPaths
{
    public static StringComparer Comparer { get; } = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    public static string Canonical(string path)
    {
        string full = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
        if (new Uri(full).IsUnc) throw new InvalidDataException("Catalog roots/charts must be local.");
        for (string? p = full; !string.IsNullOrEmpty(p); p = Path.GetDirectoryName(p))
            if ((File.Exists(p) || Directory.Exists(p)) && (File.GetAttributes(p) & FileAttributes.ReparsePoint) != 0)
                throw new InvalidDataException("Catalog symlink/reparse paths unsupported: " + p);
        return full;
    }
}
public sealed record ChartFingerprint(long Length, long ModifiedUtcTicks, string Sha256)
{
    public const int MaximumBytes = 4 * 1024 * 1024;
    public static (ChartFingerprint Fingerprint, byte[] Bytes) Read(string path, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested(); CatalogPaths.Canonical(path);
        using var stream = File.OpenRead(path);
        if (stream.Length > MaximumBytes) throw new InvalidDataException("Chart exceeds ENGINEERING4 MiB index limit.");
        var bytes = new byte[checked((int)stream.Length)]; stream.ReadExactly(bytes); token.ThrowIfCancellationRequested();
        return (new(bytes.Length, File.GetLastWriteTimeUtc(path).Ticks, Convert.ToHexString(SHA256.HashData(bytes))), bytes);
    }
}
public enum ChartAvailability { Indexed, Missing, Invalid, Unsupported }
public sealed record ChartMetadata(string Title, string? Artist, string? Creator, string? Version, int Format, int Ruleset,
    float? Hp, float? Cs, float? Od, float? Ar, int Objects, int Circles, int Sliders, int Spinners,
    double? MinimumBpm, double? MaximumBpm, int? DurationMilliseconds, int? ChartId, int? SetId, string? AudioFilename)
{
    public string Details => string.Join("\n", new[] { Title, Artist ?? "Artist unavailable", "Creator: " + (Creator ?? "unavailable"),
        "Difficulty: " + (Version ?? "unavailable"), $"Ruleset {Ruleset} · format{Format}",
        $"HP:{Scalar(Hp)} CS:{Scalar(Cs)} OD:{Scalar(Od)} AR:{Scalar(Ar)}",
        $"{Objects} objects · {Circles} circles / {Sliders} sliders / {Spinners} spinners",
        MinimumBpm is { } min && MaximumBpm is { } max ? "BPM (base timing): " + (min == max ? min.ToString("0", CultureInfo.InvariantCulture) : min.ToString("0", CultureInfo.InvariantCulture) + "–" + max.ToString("0", CultureInfo.InvariantCulture)) : "BPM unavailable",
        DurationMilliseconds is { } ms ? $"End time: {ms / 60000}:{ms / 1000 % 60:00}" : "Duration unavailable until slider preparation" });
    private static string Scalar(float? value) => value?.ToString("0.##", CultureInfo.InvariantCulture) ?? "?";
}
public sealed class CatalogChart(string path, ChartFingerprint? fingerprint, ChartMetadata? metadata, ChartAvailability availability, string? error)
{
    public string Path { get; } = path;
    public string Folder => System.IO.Path.GetDirectoryName(Path)!;
    public string Filename => System.IO.Path.GetFileName(Path);
    public ChartFingerprint? Fingerprint { get; } = fingerprint;
    public ChartMetadata? Metadata { get; } = metadata;
    public ChartAvailability Availability { get; } = availability;
    public string? Error { get; } = error;
    public string? GameplayError { get; private set; }
    public bool GameplayValidated { get; private set; }
    public bool Available => Availability == ChartAvailability.Indexed && GameplayError is null;
    public string Label => (Metadata?.Version ?? Filename) + (Available ? GameplayValidated ? " · validated" : " · indexed" : " · unavailable");
    public void ReportGameplay(string? error) { GameplayError = error; GameplayValidated = error is null; }
    public void Verify(CancellationToken token = default)
    {
        if (Fingerprint is null || ChartFingerprint.Read(Path, token).Fingerprint != Fingerprint)
            throw new InvalidDataException("Chart changed since indexing; refresh and commit its new row: " + Filename);
    }
    public Beatmap LoadGameplay(CancellationToken token = default)
    {
        Verify(token); var map = Beatmap.Load(Path); Verify(token); return map; // Fresh authoritative parser, never cached Beatmap substitution.
    }
}
public sealed record CatalogSnapshot(long Generation, IReadOnlyList<CatalogChart> Charts, IReadOnlyList<string> Notices);
public sealed class LocalChartCatalog : IDisposable
{
    public const int MaximumCharts = 10000, MaximumDirectories = 4096, MaximumDepth = 32, MaximumFileCandidates = 50000;
    private readonly string[] roots;
    private readonly HashSet<string> external = new(CatalogPaths.Comparer);
    private readonly object sync = new();
    private CancellationTokenSource? refresh;
    private long generation;
    private bool disposed;
    private readonly bool unicode;
    private readonly Func<long, Task>? beforePublish;
    public CatalogSnapshot Snapshot { get; private set; } = new(0, [], []);
    public LocalChartCatalog(IEnumerable<string> roots, bool preferUnicode = false, Func<long, Task>? beforePublish = null)
    { this.beforePublish = beforePublish; this.roots = roots.Select(CatalogPaths.Canonical).Distinct(CatalogPaths.Comparer).ToArray(); unicode = preferUnicode; }
    public async Task<CatalogSnapshot?> RefreshAsync(CancellationToken token = default, string? addExternal = null)
    {
        long epoch; CancellationTokenSource cts; string[] extras; CatalogSnapshot previous;
        lock (sync)
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            if (addExternal is not null) { if (external.Count >= MaximumCharts) throw new InvalidDataException("External catalog limit reached."); external.Add(CatalogPaths.Canonical(addExternal)); }
            refresh?.Cancel(); cts = CancellationTokenSource.CreateLinkedTokenSource(token); refresh = cts;
            epoch = ++generation; extras = external.ToArray(); previous = Snapshot;
        }
        try
        {
            var candidate = await Task.Run(() => Scan(epoch, extras, previous, cts.Token), cts.Token);
            if (beforePublish is not null) await beforePublish(epoch); // Injected engineering verification barrier; host supplies none.
            lock (sync) { if (disposed || epoch != generation || cts.IsCancellationRequested) return null; Snapshot = candidate; return candidate; }
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested) { return null; }
        finally { lock (sync) { if (ReferenceEquals(refresh, cts)) refresh = null; } cts.Dispose(); }
    }
    private CatalogSnapshot Scan(long epoch, string[] extras, CatalogSnapshot previous, CancellationToken token)
    {
        var paths = new HashSet<string>(extras, CatalogPaths.Comparer); var notices = new List<string>();
        var todo = new Stack<(string Path, int Depth)>(); foreach (string root in roots) todo.Push((root, 0));
        int directories = 0, examined = 0;
        while (todo.TryPop(out var item))
        {
            token.ThrowIfCancellationRequested();
            if (++directories > MaximumDirectories) { notices.Add("Directory limit reached; index is partial."); break; }
            try
            {
                CatalogPaths.Canonical(item.Path);
                if (!Directory.Exists(item.Path)) { if (notices.Count < 32) notices.Add("Root/directory missing: " + item.Path); continue; }
                var files = Directory.EnumerateFiles(item.Path).Take(Math.Min(MaximumCharts, MaximumFileCandidates - examined) + 1).ToArray();
                int admitted = Math.Min(files.Length, Math.Min(MaximumCharts, MaximumFileCandidates - examined));
                if (files.Length > admitted && notices.Count < 32) notices.Add("File-candidate limit reached; index is partial.");
                foreach (string p in files.Take(admitted).Order(CatalogPaths.Comparer))
                {
                    token.ThrowIfCancellationRequested(); examined++;
                    if (System.IO.Path.GetExtension(p).Equals(".osu", StringComparison.OrdinalIgnoreCase)) paths.Add(CatalogPaths.Canonical(p));
                    if (paths.Count >= MaximumCharts) break;
                }
                if (examined >= MaximumFileCandidates) { notices.Add("Scan candidate limit reached; index is partial."); break; }
                if (paths.Count >= MaximumCharts) { notices.Add("Chart limit reached; index is partial."); break; }
                if (item.Depth < MaximumDepth)
                    foreach (string p in Directory.EnumerateDirectories(item.Path).OrderDescending(CatalogPaths.Comparer).Take(MaximumDirectories))
                        if (todo.Count < MaximumDirectories && (File.GetAttributes(p) & FileAttributes.ReparsePoint) == 0) todo.Push((p, item.Depth + 1));
            }
            catch (Exception e) when (e is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException)
            { if (notices.Count < 32) notices.Add("Index directory unavailable: " + e.Message); }
        }
        var old = previous.Charts.ToDictionary(c => c.Path, CatalogPaths.Comparer); var rows = new List<CatalogChart>();
        foreach (string path in paths.Order(CatalogPaths.Comparer).Take(MaximumCharts))
        {
            token.ThrowIfCancellationRequested(); var chart = Index(path, unicode, token);
            if (old.TryGetValue(path, out var prior) && prior.Fingerprint is not null && prior.Fingerprint == chart.Fingerprint && prior.Availability == chart.Availability && prior.GameplayError is null)
                chart = prior; // Stable reference only for byte/stamp-identical entries, never a changed chart.
            rows.Add(chart);
        }
        return new(epoch, rows, notices.Take(32).ToArray());
    }
    public static CatalogChart Index(string path, bool preferUnicode = false, CancellationToken token = default)
    {
        path = Path.GetFullPath(path); ChartFingerprint? fingerprint = null;
        try
        {
            CatalogPaths.Canonical(path);
            if (!System.IO.Path.GetExtension(path).Equals(".osu", StringComparison.OrdinalIgnoreCase)) throw new InvalidDataException("Choose an .osu chart.");
            var read = ChartFingerprint.Read(path, token); fingerprint = read.Fingerprint;
            using var reader = new StreamReader(new MemoryStream(read.Bytes), Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            string? first = reader.ReadLine();
            if (first is null || !first.StartsWith("osu file format v", StringComparison.Ordinal) || !int.TryParse(first[17..], out int format)) throw new InvalidDataException("Missing osu format header.");
            var fields = new Dictionary<string, string>(StringComparer.Ordinal); string section = ""; var bpm = new List<double>();
            int objects = 0, circles = 0, sliders = 0, spinners = 0, end = 0, lines = 0; bool unsupportedObject = false;
            while (reader.ReadLine() is { } line)
            {
                token.ThrowIfCancellationRequested(); if (++lines > 100000) throw new InvalidDataException("Index line limit exceeded.");
                line = line.Trim(); if (line.Length == 0 || line.StartsWith("//", StringComparison.Ordinal)) continue;
                if (line.StartsWith('[')) { section = line; continue; }
                int colon = line.IndexOf(':');
                if (section is "[Metadata]" or "[General]" or "[Difficulty]")
                { if (colon > 0) fields[section + line[..colon].Trim()] = line[(colon + 1)..].Trim(); continue; }
                var cells = line.Split(',');
                if (section == "[TimingPoints]" && cells.Length >= 2 && double.TryParse(cells[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double beat) && double.IsFinite(beat) && beat > 0 && (cells.Length < 7 || cells[6].Trim() == "1")) { double value = 60000 / beat; if (double.IsFinite(value)) bpm.Add(value); }
                if (section != "[HitObjects]") continue;
                if (cells.Length < 5 || !int.TryParse(cells[2], out int time) || !int.TryParse(cells[3], out int raw) || time < 0 || time > 3600000) throw new InvalidDataException("Malformed object metadata.");
                objects++; end = Math.Max(end, time); int type = raw & 139;
                if ((type & 8) != 0) { spinners++; if (cells.Length < 6 || !int.TryParse(cells[5], out int stop) || stop < time || stop > 3600000) throw new InvalidDataException("Malformed spinner end metadata."); end = Math.Max(end, stop); }
                else if ((type & 2) != 0) sliders++;
                else if ((type & 1) != 0) circles++;
                else unsupportedObject = true;
            }
            string? Get(string key) => fields.GetValueOrDefault(key) is { Length: > 0 } value ? value : null;
            int Int(string key, int fallback) => int.TryParse(Get(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out int n) ? n : fallback;
            float? Scalar(string key) => float.TryParse(Get("[Difficulty]" + key), NumberStyles.Float, CultureInfo.InvariantCulture, out float f) && float.IsFinite(f) ? f : null;
            string title = (preferUnicode ? Get("[Metadata]TitleUnicode") : null) ?? Get("[Metadata]Title") ?? System.IO.Path.GetFileNameWithoutExtension(path);
            var metadata = new ChartMetadata(title, (preferUnicode ? Get("[Metadata]ArtistUnicode") : null) ?? Get("[Metadata]Artist"), Get("[Metadata]Creator"), Get("[Metadata]Version"), format, Int("[General]Mode", 0), Scalar("HPDrainRate"), Scalar("CircleSize"), Scalar("OverallDifficulty"), Scalar("ApproachRate"), objects, circles, sliders, spinners, bpm.Count == 0 ? null : bpm.Min(), bpm.Count == 0 ? null : bpm.Max(), sliders == 0 ? end : null, Int("[Metadata]BeatmapID", 0), Int("[Metadata]BeatmapSetID", 0), Get("[General]AudioFilename"));
            string? limitation = format is < 5 or > 14 ? "Gameplay format unsupported." : metadata.Ruleset != 0 ? "Only ordinary ruleset0 supported." : unsupportedObject || spinners > 0 && format != 14 ? "Unsupported gameplay object/format." : objects == 0 ? "No indexed objects." : null;
            return new(path, fingerprint, metadata, limitation is null ? ChartAvailability.Indexed : ChartAvailability.Unsupported, limitation);
        }
        catch (Exception e) when (e is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException)
        { return new(path, fingerprint, null, File.Exists(path) ? ChartAvailability.Invalid : ChartAvailability.Missing, e.Message); }
    }
    public void Dispose() { lock (sync) { if (disposed) return; disposed = true; generation++; refresh?.Cancel(); } }
}
public sealed record SuppliedStar(string Compatibility, string Source, string ChartSha256, int Ruleset, int NormalizedMods, float Value, bool Stale = false);
public sealed record StarDisplay(string State, string Text)
{
    public static StarDisplay Prepare(CatalogChart chart, int ruleset, int mods, SuppliedStar? supplied = null)
    {
        if (supplied is null) return new("missing", "Stars not calculated · no compatible cache");
        if (supplied.Compatibility != "osu-stable:E043:cache-v1" || string.IsNullOrWhiteSpace(supplied.Source)) return new("unsupported", "Star source unsupported");
        if (supplied.ChartSha256 != chart.Fingerprint?.Sha256) return new("stale", "Star cache belongs to another chart revision");
        if (ruleset != 0 || supplied.Ruleset != ruleset || supplied.NormalizedMods != (mods & 1370)) return new("mismatched", "Star mode/mod key mismatch");
        if (!float.IsFinite(supplied.Value)) return new("unsupported", "Star value unavailable");
        bool stale = supplied.Stale || supplied.Value < 0;
        double value = Math.Floor(Math.Abs((double)supplied.Value) * 100) / 100;
        return new(stale ? "stale" : "valid", value.ToString("0.##", CultureInfo.InvariantCulture) + "★ · " + (stale ? "stale supplied cache" : "supplied cache") + " · " + supplied.Source);
    }
}
