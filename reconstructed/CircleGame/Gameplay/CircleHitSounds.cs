using System.IO;
using System.Globalization;

namespace Ohno.CircleGame.Gameplay;

// E037/E038 finite ordinary circle projection. No Windows, clock, scoring or HP dependency.
public enum SampleOrigin { Map, Skin, Default }
public enum SampleLoadMode { MapShortCreate, MapBytes, SkinFile, EmbeddedBytes }
public sealed record SampleBytes(string Name, byte[] Bytes, SampleOrigin Origin, SampleLoadMode Mode);
public interface ISampleFiles
{
    bool Exists(string name);
    byte[]? Read(string name);
}
public interface IDefaultSamples
{
    byte[]? Ui(string key);
    byte[]? Gameplay(string key);
}
public interface ISampleLoader
{
    int Load(SampleBytes sample); // Opaque engineering IDs; -1 missing and 0 invalid stay distinct.
    void Free(int handle);
}
public sealed record SampleResourcePolicy(int Format, bool MapSuppressed, bool EarlyProcessGate,
    bool HasSelectedMap, bool HasSkin, string SkinName, bool SkinSamples, CultureInfo Culture);
public sealed class SampleCaches
{
    public Dictionary<string, int> Map { get; } = new(StringComparer.Ordinal);
    public Dictionary<string, int> Skin { get; }
    public Dictionary<string, int> Embedded { get; }
    public SampleCaches(Dictionary<string, int>? skin = null, Dictionary<string, int>? embedded = null)
    { Skin = skin ?? new(StringComparer.Ordinal); Embedded = embedded ?? new(StringComparer.Ordinal); }
}
public sealed class CircleSampleResources
{
    public static readonly int[] LayerOrder = [1, 4, 2, 8];
    private static readonly string[] StandardExtensions = [".wav", ".mp3", ".ogg"];
    private static readonly string[] CustomExtensions = [".wav", ".ogg", ".mp3"];
    private readonly ISampleFiles map;
    private readonly ISampleFiles? skin;
    private readonly IDefaultSamples defaults;
    private readonly ISampleLoader loader;
    private readonly SampleResourcePolicy policy;
    public SampleCaches Caches { get; }
    private readonly int[,,] table = new int[4, 3, 3]; // Original fresh cells0, not prepared missing -1.
    public CircleSampleResources(ISampleFiles map, ISampleFiles? skin, IDefaultSamples defaults,
        ISampleLoader loader, SampleResourcePolicy policy, SampleCaches? caches = null)
    { this.map = map; this.skin = skin; this.defaults = defaults; this.loader = loader; this.policy = policy; Caches = caches ?? new(); }
    public static string LayerName(int bit) => bit switch
    { 1 => "hitnormal", 4 => "hitfinish", 2 => "hitwhistle", 8 => "hitclap", _ => throw new ArgumentOutOfRangeException(nameof(bit)) };
    private string SetName(int set) => (set switch { -1 => "All", 0 => "None", 1 => "Normal", 2 => "Soft", 3 => "Drum", _ => set.ToString(policy.Culture) }).ToLower(policy.Culture);
    public string Key(int bit, int set, int index = 1) => SetName(set) + "-" + LayerName(bit) + (index > 1 ? index.ToString(policy.Culture) : "");
    private static int Row(int bit) => bit switch { 1 => 0, 2 => 1, 4 => 2, 8 => 3, _ => throw new ArgumentOutOfRangeException(nameof(bit)) };
    private static int Bank(int set) => set is 0 or 2 ? 1 : set == 3 ? 2 : 0;

    public int Standard(int bit, int set, int index, int stages = 7)
    {
        if (policy.EarlyProcessGate) return -1;
        string baseKey = Key(bit, set), mapKey = Key(bit, set, index);
        int handle;
        if ((stages & 4) != 0 && !policy.MapSuppressed && policy.HasSelectedMap && index != 0)
        {
            if (!Caches.Map.TryGetValue(mapKey, out handle))
            {
                handle = -1;
                foreach (string ext in StandardExtensions)
                {
                    byte[]? bytes = map.Read(mapKey + ext);
                    if (bytes is null && policy.Format < 5) bytes = map.Read(LayerName(bit) + ext);
                    if (bytes is null) continue;
                    handle = MapLoad(mapKey + ext, bytes); break; // Stop on bytes, even load -1/0.
                }
                Caches.Map[mapKey] = handle;
            }
            if (handle != -1) return handle;
        }
        if ((stages & 2) != 0 && policy.SkinSamples && !(policy.HasSkin && policy.SkinName == "Default"))
        {
            if (!policy.HasSkin || skin is null) throw new InvalidDataException("Unsupported null skin receiver with SkinSamples enabled.");
            if (!Caches.Skin.TryGetValue(baseKey, out handle))
            {
                handle = -1;
                foreach (string ext in StandardExtensions)
                {
                    string name = baseKey + ext;
                    if (!skin.Exists(name)) continue;
                    byte[]? bytes = skin.Read(name);
                    handle = bytes is null ? -1 : loader.Load(new(name, bytes, SampleOrigin.Skin, SampleLoadMode.SkinFile));
                    break; // First existing path ends this provider, even decoder/load failure.
                }
                Caches.Skin[baseKey] = handle;
            }
            if (handle != -1) return handle;
        }
        if ((stages & 1) == 0) return -1;
        if (Caches.Embedded.TryGetValue(baseKey, out handle)) return handle;
        byte[]? embedded = defaults.Ui(baseKey) ?? defaults.Gameplay(baseKey);
        handle = embedded is null ? -1 : loader.Load(new(baseKey, embedded, SampleOrigin.Default, SampleLoadMode.EmbeddedBytes));
        if (handle == 0) handle = -1; // Embedded-only normalization.
        Caches.Embedded[baseKey] = handle;
        return handle;
    }
    private int MapLoad(string name, byte[] bytes) => loader.Load(new(name, bytes, SampleOrigin.Map,
        bytes.Length < 1024 ? SampleLoadMode.MapShortCreate : SampleLoadMode.MapBytes));
    public int Custom(string name, bool acceptingSetter = true, bool force = false)
    {
        if (!acceptingSetter || string.IsNullOrEmpty(name) || policy.MapSuppressed) return -1;
        bool hasExtension = name.IndexOf('.') > 0;
        string stem = hasExtension ? name[..name.LastIndexOf('.')] : name;
        if (!force && Caches.Map.TryGetValue(stem, out int cached)) return cached;
        byte[]? bytes = null; string selected = name;
        if (!hasExtension || !map.Exists(name))
        {
            foreach (string ext in CustomExtensions)
            {
                selected = stem + ext; bytes = map.Read(selected);
                if (bytes is null && policy.Format < 5) { selected = name + ext; bytes = map.Read(selected); }
                if (bytes is not null) break;
            }
        }
        else
        {
            bytes = map.Read(name);
            if (bytes is null && policy.Format < 5) bytes = map.Read(name);
        }
        int handle = bytes is null ? -1 : MapLoad(selected, bytes);
        if (Caches.Map.TryGetValue(stem, out int previous) && previous != -1) loader.Free(previous);
        Caches.Map[stem] = handle; return handle;
    }
    public void Prepare(IEnumerable<int> timingIndexes, IEnumerable<int> registeredIndexes)
    {
        int[] indexes = timingIndexes.Concat(registeredIndexes).Where(i => i > 2).Distinct().ToArray();
        if (indexes.Length > 64) throw new InvalidDataException("Effects exceed ENGINEERING 64 dynamic-index preparation limit.");
        foreach (int index in indexes)
            foreach (int set in new[] { 1, 2, 3 })
                foreach (int bit in LayerOrder) Standard(bit, set, index);
        for (int plane = 0; plane < 3; plane++)
            foreach (int set in new[] { 1, 2, 3 })
                foreach (int bit in LayerOrder) table[Row(bit), Bank(set), plane] = Standard(bit, set, plane);
    }
    public int Resolve(int bit, int set, int suppliedIndex)
    {
        int index = Math.Max(suppliedIndex, 0);
        if (index <= 2) return table[Row(bit), Bank(set), index];
        return Caches.Map.TryGetValue(Key(bit, set, index), out int handle) && handle != -1 ? handle : table[Row(bit), Bank(set), 0];
    }
    public void EvictMap(string key)
    {
        if (!Caches.Map.Remove(key, out int handle)) return;
        if (handle != -1) loader.Free(handle);
    }
    private void Clear(Dictionary<string, int> cache)
    { foreach (int handle in cache.Values) loader.Free(handle); cache.Clear(); }
    public void ClearMap() => Clear(Caches.Map);
    public void ClearSkin() => Clear(Caches.Skin);
    public void ClearAll() { ClearMap(); ClearSkin(); Clear(Caches.Embedded); Array.Clear(table); }
}

public sealed record CircleSoundPolicy(bool HasSkin, bool LayeredHitSounds, bool HasSelectedMap,
    int SelectedMode, int SongRate, bool RateMatching, int VolumeEffect, int VolumeUniversal);
public sealed record SelectedCircleSample(int Mask, int BaseSet, int AdditionSet, int Index, int Volume);
public sealed record SamplePlaybackRequest(CircleDefinition Source, int Handle, int Layer, int Volume,
    float Pan, float Factor, float Gain);
public interface ISamplePlaybackSink { void Enqueue(SamplePlaybackRequest request); }
public static class CircleSampleSelection
{
    public static SampleTiming? Timing(IReadOnlyList<SampleTiming>? list, int storedStart)
    {
        if (list is null || list.Count == 0) return null;
        double query = unchecked(storedStart + 5);
        SampleTiming? selected = null;
        foreach (var point in list) if (point.Time <= query) selected = point;
        return selected ?? list[0];
    }
    public static SelectedCircleSample Select(CircleSampleMetadata circle, SampleTiming timing, CircleSoundPolicy policy)
    {
        int set = circle.BaseSet != 0 ? circle.BaseSet : timing.Set;
        int index = circle.Index != 0 ? circle.Index : timing.Index;
        int volume = circle.Volume != 0 ? circle.Volume : timing.Volume;
        int addition = set != 0 ? circle.AdditionSet : set;
        int mask = circle.StoredMask;
        if (mask == 0 || (policy.HasSkin && policy.LayeredHitSounds && policy.HasSelectedMap && policy.SelectedMode != 3)) mask |= 1;
        return new(mask, set, addition, index, volume);
    }
    public static void Request(CircleSampleMetadata circle, IReadOnlyList<SampleTiming>? timing,
        int customHandle, CircleSampleResources resources, CircleSoundPolicy policy, ISamplePlaybackSink sink,
        Action<SelectedCircleSample>? observer = null)
    {
        if (circle.Error is not null) throw new InvalidDataException(circle.Error);
        SampleTiming? point = Timing(timing, circle.Object.StartTime);
        if (point is null) return;
        SelectedCircleSample info = Select(circle, point, policy); observer?.Invoke(info);
        float pan = Math.Clamp((circle.Object.Position.X / 512f - .5f) * .800000011920929f, -1f, 1f);
        float factor = policy.SongRate != 100 && policy.RateMatching ? 1f + ((float)policy.SongRate - 100f) / 100f : 1f;
        void Submit(int handle, int layer, int volume)
        {
            if (handle == -1) return; // M3b31; zero still queues, worker validity is separate.
            float global = (float)unchecked(policy.VolumeEffect * policy.VolumeUniversal) / 100f;
            float gain = (volume / 100f) * global / 100f;
            sink.Enqueue(new(circle.Object, handle, layer, volume, pan, factor, gain));
        }
        if (customHandle != -1) { Submit(customHandle, 0, info.Volume); return; }
        int selectedVolume = Math.Max(8, info.Volume);
        foreach (int bit in CircleSampleResources.LayerOrder)
        {
            if ((info.Mask & bit) == 0) continue;
            int set = bit == 1 ? info.BaseSet : info.AdditionSet != 0 ? info.AdditionSet : info.BaseSet;
            int volume = bit == 1 ? (int)(selectedVolume * .8) : bit == 4 ? selectedVolume : (int)(selectedVolume * .85);
            Submit(resources.Resolve(bit, set, info.Index), bit, volume);
        }
    }
}

// ENGINEERING bounded dispatch lifetime, independently usable with recording sinks.
// Pause drops queued and stops active effects; resume never replays dropped requests.
public sealed class CircleSoundPlayback : ISamplePlaybackSink
{
    private readonly Action<SamplePlaybackRequest> play;
    private readonly Action<Action> dispatch;
    private readonly Action stop;
    private int generation;
    public bool Enabled { get; set; } = true;
    public bool Paused { get; private set; } = true;
    public bool Disposed { get; private set; }
    public string? Error { get; private set; }
    public CircleSoundPlayback(Action<SamplePlaybackRequest> play, Action<Action> dispatch, Action stop)
    { this.play = play; this.dispatch = dispatch; this.stop = stop; }
    public void Enqueue(SamplePlaybackRequest request)
    {
        if (Disposed || Paused || !Enabled) return;
        int epoch = generation;
        try { dispatch(() => { if (Disposed || Paused || !Enabled || epoch != generation || request.Handle is 0 or -1) return;
            try { play(request); } catch (Exception e) { Error = e.Message; } }); }
        catch (Exception e) { Error = e.Message; }
    }
    public void Pause() { if (Disposed) return; Paused = true; generation++; Stop(); }
    public void Resume() { if (!Disposed) Paused = false; }
    public void Restart() { Pause(); Resume(); }
    private void Stop() { try { stop(); } catch (Exception e) { Error = e.Message; } }
    public void Dispose() { if (Disposed) return; Pause(); Disposed = true; }
}
