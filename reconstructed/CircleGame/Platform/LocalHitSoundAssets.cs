using System.IO;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Platform;

// ENGINEERING local-only admission. Original helpers did not impose this path policy.
public sealed class LocalSampleFiles : ISampleFiles
{
    public const int MaxFileBytes = 8 * 1024 * 1024;
    private readonly string root;
    private readonly Action<string> report;
    public LocalSampleFiles(string root, Action<string>? report = null)
    {
        this.root = Path.GetFullPath(root); this.report = report ?? (_ => { });
        if (new Uri(this.root).IsUnc) throw new InvalidDataException("Network sample roots are unsupported.");
        RejectLinks(this.root);
    }
    public string Resolve(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 240 || Path.IsPathRooted(name) ||
            name.Contains(':') || name.StartsWith('/') || name.StartsWith('\\'))
            throw new InvalidDataException("Samples require a relative local path inside the admitted root.");
        string normalized = name.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        if (normalized.Split(Path.DirectorySeparatorChar).Any(p => p is ".." or "."))
            throw new InvalidDataException("Sample path traversal is unsupported.");
        string full = Path.GetFullPath(Path.Combine(root, normalized));
        if (!full.StartsWith(Path.TrimEndingDirectorySeparator(root) + Path.DirectorySeparatorChar,
                OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            throw new InvalidDataException("Sample path escapes its admitted root.");
        RejectLinks(full);
        return full;
    }
    private static void RejectLinks(string full)
    {
        string? path = full;
        while (!string.IsNullOrEmpty(path))
        {
            if ((File.Exists(path) || Directory.Exists(path)) && (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
                throw new InvalidDataException("Symlink/reparse-point sample paths are unsupported.");
            path = Path.GetDirectoryName(path);
        }
    }
    public bool Exists(string name) => File.Exists(Resolve(name));
    public byte[]? Read(string name)
    {
        string path = Resolve(name);
        try
        {
            if (!File.Exists(path)) return null;
            using var stream = File.OpenRead(path);
            if (stream.Length > MaxFileBytes) throw new InvalidDataException("Sample exceeds ENGINEERING 8 MiB limit.");
            byte[] bytes = new byte[checked((int)stream.Length)]; stream.ReadExactly(bytes); return bytes;
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        { report("Sample read failed: " + e.Message); return null; }
    }
}
public sealed record HitSoundSettings
{
    public bool Enabled { get; init; } = true;
    public bool LayeredHitSounds { get; init; } = true;
    public bool SkinSamples { get; init; } = true;
    public string SkinName { get; init; } = "Default";
    public string? SkinDirectory { get; init; }
    public string? DefaultPackDirectory { get; init; }
    public int VolumeEffect { get; init; } = 80;
    public int VolumeUniversal { get; init; } = 100;
    public static HitSoundSettings Read(string path)
    {
        if (!File.Exists(path)) return new();
        if (new FileInfo(path).Length > 16384) throw new InvalidDataException("Hitsound settings exceed 16 KiB.");
        var settings = JsonSerializer.Deserialize<HitSoundSettings>(File.ReadAllText(path)) ?? throw new InvalidDataException("Empty hitsound settings.");
        if (settings.VolumeEffect is < 0 or > 100 || settings.VolumeUniversal is < 0 or > 100 || string.IsNullOrWhiteSpace(settings.SkinName))
            throw new InvalidDataException("Hitsound volumes must be 0–100 and SkinName nonempty.");
        return settings;
    }
}
// Independent pack interface: exact resource key maps to key.wav, no native
// extension probing claim. UI store absent; pack supplies the gameplay-store slot.
public sealed class IndependentSamplePack(ISampleFiles? files) : IDefaultSamples
{
    public byte[]? Ui(string key) => null;
    public byte[]? Gameplay(string key) => files?.Read(key + ".wav");
}
public sealed class EffectSampleCatalog : ISampleLoader
{
    private int next;
    private Dictionary<int, SampleBytes> Samples { get; } = new();
    private readonly object sync = new();
    public List<string> Errors { get; } = [];
    private long bytes;
    public int Load(SampleBytes sample)
    {
        lock (sync)
        {
        // Native short-byte SampleCreate is unsupported by MediaPlayer. Keep
        // logical0 (invalid playback) rather than inventing audible sample bytes.
        if (sample.Mode == SampleLoadMode.MapShortCreate)
        { Errors.Add("Native short-sample creation unsupported: " + sample.Name); return 0; }
        if (Samples.Count >= 64 || bytes + sample.Bytes.Length > 64 * 1024 * 1024)
            throw new InvalidDataException("Effects exceed ENGINEERING 64 samples /64 MiB preparation limit.");
        if (sample.Origin != SampleOrigin.Default && Path.GetExtension(sample.Name).ToLowerInvariant() is not (".wav" or ".mp3" or ".ogg"))
        { Errors.Add("Unsupported effects format: " + sample.Name); return 0; }
        int id = ++next; Samples.Add(id, sample); bytes += sample.Bytes.Length; return id;
        }
    }
    public void Free(int handle)
    { lock (sync) { if (Samples.Remove(handle, out var sample)) bytes -= sample.Bytes.Length; } }
    public IReadOnlyDictionary<int, SampleBytes> Snapshot(IEnumerable<int> ids)
    { lock (sync) return ids.Where(Samples.ContainsKey).ToDictionary(h => h, h => Samples[h]); }
}
public sealed class PreparedCircleEffects : IDisposable
{
    public required CircleSoundMap Source { get; init; }
    public required CircleSampleResources Resources { get; init; }
    public required IReadOnlyDictionary<CircleDefinition, (CircleSampleMetadata Metadata, int Custom)> Circles { get; init; }
    public required IReadOnlyDictionary<int, SampleBytes> Samples { get; init; }
    public Beatmap? PreparedMap { get; init; }
    public string? ContinuousLimitation { get; init; }
    public string? Error { get; init; }
    public int MissingLayers { get; init; }
    public IReadOnlyDictionary<SliderDefinition, PreparedSliderSamples> Sliders { get; init; } =
        new Dictionary<SliderDefinition, PreparedSliderSamples>(ReferenceEqualityComparer.Instance);
    public void Dispose() => Resources.ClearMap();
}
// One library per settings configuration. Shared skin/default cache persists
// across map replacement; each candidate owns a separate map/custom cache.
public sealed class HitSoundLibrary : IDisposable
{
    public HitSoundSettings Settings { get; }
    private readonly SampleCaches shared = new();
    private readonly EffectSampleCatalog catalog = new();
    private readonly SemaphoreSlim preparation = new(1);
    private bool disposed;
    private readonly ISampleFiles? skin;
    private readonly IDefaultSamples defaults;
    public HitSoundLibrary(HitSoundSettings settings)
    {
        Settings = settings;
        skin = settings.SkinDirectory is null ? null : new LocalSampleFiles(settings.SkinDirectory, catalog.Errors.Add);
        defaults = new IndependentSamplePack(settings.DefaultPackDirectory is null ? null : new LocalSampleFiles(settings.DefaultPackDirectory, catalog.Errors.Add));
    }
    public async Task<PreparedCircleEffects> PrepareAsync(Beatmap map, CancellationToken token)
    {
        await preparation.WaitAsync(token);
        try { if (disposed) throw new ObjectDisposedException(nameof(HitSoundLibrary)); return await Task.Run(() => Prepare(map, token), token); }
        finally { preparation.Release(); }
    }
    private PreparedCircleEffects Prepare(Beatmap map, CancellationToken token)
    {
        var source = map.CircleSounds ?? throw new InvalidDataException("Hitsounds need fresh parser sample metadata.");
        // Bounded fresh registered-index order. R10 health already guards this;
        // discrete sound preparation must not silently repair an ambiguous source.
        if (map.Sliders.Count > 0 && (map.HealthSource is null || !map.HealthSource.IsFreshFor(map) ||
            map.HealthSource.Rows.Zip(map.HealthSource.Rows.Skip(1)).Any(p => p.First.Start >= p.Second.Start)))
            throw new InvalidDataException("Slider sounds require proven fresh, strictly increasing registered source order.");
        foreach (var operand in source.RegisteredOverrides.Where(o => o.Slider is not null && !string.IsNullOrEmpty(o.Filename)))
        {
            string stem = CustomStem(operand.Filename);
            if (IsStandardStem(stem) || source.RegisteredOverrides.Count(o => !string.IsNullOrEmpty(o.Filename) && CustomStem(o.Filename) == stem) > 1)
                throw new InvalidDataException("Unsupported parent custom-cache collision: " + stem);
        }
        var resources = new CircleSampleResources(new LocalSampleFiles(source.Directory, catalog.Errors.Add), skin, defaults, catalog,
            new(source.Format, false, false, true, true, Settings.SkinName, Settings.SkinSamples, System.Globalization.CultureInfo.CurrentCulture),
            new SampleCaches(shared.Skin, shared.Embedded));
        try
        {
            int errorStart = catalog.Errors.Count;
            var circles = new Dictionary<CircleDefinition, (CircleSampleMetadata, int)>(ReferenceEqualityComparer.Instance);
            var custom = new Dictionary<CircleDefinition, int>(ReferenceEqualityComparer.Instance);
            string? operandError = null;
            foreach (var operand in source.RegisteredOverrides)
            {
                token.ThrowIfCancellationRequested();
                if (operand.Error is not null) { operandError ??= operand.Error; continue; }
                int handle;
                try { handle = resources.Custom(operand.Filename, loop: operand.Slider?.Samples?.CustomLoopFlag == true); }
                catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
                { operandError ??= "Hitsound custom resource rejected: " + e.Message; handle = -1; }
                if (operand.Circle is not null) custom.Add(operand.Circle, handle);
            }
            var parentHandles = new Dictionary<SliderDefinition, int>(ReferenceEqualityComparer.Instance);
            // Parent handles were resolved in original registration order above.
            foreach (var operand in source.RegisteredOverrides.Where(o => o.Slider is not null))
            {
                string key = CustomStem(operand.Filename);
                parentHandles[operand.Slider!] = string.IsNullOrEmpty(operand.Filename) ? -1 : resources.Caches.Map.GetValueOrDefault(key, -1);
            }
            foreach (var sample in source.Circles)
                circles.Add(sample.Object, (sample, custom.GetValueOrDefault(sample.Object, -1)));
            var selectionPolicy = new CircleSoundPolicy(true, Settings.LayeredHitSounds, true, 0, 100, false, Settings.VolumeEffect, Settings.VolumeUniversal);
            var sliders = new Dictionary<SliderDefinition, PreparedSliderSamples>(ReferenceEqualityComparer.Instance);
            foreach (var slider in map.Sliders)
                sliders.Add(slider, PreparedSliderSamples.Create(slider, source.Timing, selectionPolicy, parentHandles.GetValueOrDefault(slider, -1)));
            resources.Prepare(source.Timing.Select(t => t.Index), source.RegisteredOverrides.Select(c => c.Index), includeTicks: map.Sliders.Count > 0);
            string? continuousLimitation = ContinuousAdmission(map);
            if (continuousLimitation is null && map.Sliders.Count > 0)
            {
                try { resources.PrepareContinuing(source.Timing.Select(t => t.Index)); }
                catch (Exception e) { continuousLimitation = "Loop preparation failed: " + e.Message; }
            }
            token.ThrowIfCancellationRequested();
            var handles = resources.Caches.Map.Values.Concat(resources.Caches.Skin.Values).Concat(resources.Caches.Embedded.Values).Where(h => h > 0).Distinct();
            var samples = catalog.Snapshot(handles);
            int missing = 0;
            foreach (var sample in circles.Values)
            {
                var point = CircleSampleSelection.Timing(source.Timing, sample.Item1.Object.StartTime);
                if (point is null || sample.Item1.Error is not null) continue;
                if (sample.Item2 != -1) { if (sample.Item2 == 0) missing++; continue; }
                var info = CircleSampleSelection.Select(sample.Item1, point, selectionPolicy);
                foreach (int bit in CircleSampleResources.LayerOrder)
                    if ((info.Mask & bit) != 0 && resources.Resolve(bit, bit == 1 ? info.BaseSet : info.AdditionSet != 0 ? info.AdditionSet : info.BaseSet, info.Index) is 0 or -1)
                        missing++;
            }
            string[] errors = circles.Values.Select(c => c.Item1.Error).Where(e => e is not null).Cast<string>().Concat(catalog.Errors.Skip(errorStart)).Distinct().ToArray();
            return new() { Source = operandError is null ? source : source with { TimingError = operandError }, Resources = resources, Circles = circles, Samples = samples, MissingLayers = missing, Sliders = sliders, PreparedMap = map, ContinuousLimitation = continuousLimitation,
                Error = source.TimingError ?? operandError ?? (errors.Length > 0 ? string.Join("; ", errors.Take(3)) : null) };
        }
        catch { resources.ClearMap(); throw; }
    }
    public static string? ContinuousAdmission(Beatmap map)
    {
        var source = map.CircleSounds;
        if (source is null || source.Format < 14) return "continuing audio requires fresh format14+ metadata";
        if (source.TimingError is not null) return source.TimingError;
        if (source.Timing.Count == 0) return "continuing audio requires timing records";
        if (source.Timing.Any(t => !double.IsFinite(t.Time) || t.Set is < 0 or > 3 || t.Index < 0 || t.Volume is < 0 or > 100)) return "unsupported continuing timing operands";
        if (source.Timing.Zip(source.Timing.Skip(1)).Any(p => p.First.Time > p.Second.Time)) return "out-of-order continuing timing (model supports conditional controls only)";
        if (source.RegisteredOverrides.Any(o => System.Text.RegularExpressions.Regex.IsMatch(CustomStem(o.Filename),
            @"^(normal|soft|drum)-(sliderslide|sliderwhistle)\d*$"))) return "ambiguous continuing/custom cache collision";
        var sliders = map.Sliders.OrderBy(s => s.Head.StartTime).ToArray();
        if (sliders.Zip(sliders.Skip(1)).Any(p => p.First.EndTime >= p.Second.Head.StartTime)) return "overlapping continuous producers unsupported";
        if (sliders.Any(s => s.Samples is null || s.Samples.Error is not null || s.Samples.BaseSet is < 0 or > 3 || s.Samples.AdditionSet is < 0 or > 3)) return "unsupported parent continuing selectors";
        return null;
    }
    private static string CustomStem(string name) => name.IndexOf('.') > 0 ? name[..name.LastIndexOf('.')] : name;
    private static bool IsStandardStem(string stem) => System.Text.RegularExpressions.Regex.IsMatch(stem,
        @"^(normal|soft|drum|none|all|[+-]?\d+)-(hitnormal|hitfinish|hitwhistle|hitclap|slidertick)\d*$",
        System.Text.RegularExpressions.RegexOptions.CultureInvariant);
    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        // Cancellation is requested by the owner first. Let an in-flight bounded
        // read finish without racing its cache mutation or blocking the dispatcher.
        _ = ClearAfterPreparationAsync();
    }
    private async Task ClearAfterPreparationAsync()
    {
        await preparation.WaitAsync().ConfigureAwait(false);
        try
        {
            foreach (int id in shared.Skin.Values.Concat(shared.Embedded.Values).ToArray()) catalog.Free(id);
            shared.Skin.Clear(); shared.Embedded.Clear();
        }
        finally { preparation.Release(); }
    }
}
