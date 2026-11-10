using System.IO;
using System.Globalization;

namespace Ohno.CircleGame.Gameplay;

// E037/E038 finite ordinary circle projection. No Windows, clock, scoring or HP dependency.
public enum SampleOrigin { Map, Skin, Default }
public enum SampleLoadMode { MapShortCreate, MapBytes, SkinFile, EmbeddedBytes }
public sealed record SampleBytes(string Name, byte[] Bytes, SampleOrigin Origin, SampleLoadMode Mode)
{
    public bool Loop { get; init; } // Original load flag only; never a discrete voice loop command.
}
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
    private readonly int[,,] loopTable = new int[2, 3, 3];
    private readonly int[,] tickTable = new int[3, 3];
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

    public int Standard(int bit, int set, int index, int stages = 7) => LoadStandard(LayerName(bit), set, index, stages);
    public string TickKey(int set, int index = 1) => SetName(set) + "-slidertick" + (index > 1 ? index.ToString(policy.Culture) : "");
    public int StandardTick(int set, int index, int stages = 7) => LoadStandard("slidertick", set, index, stages);
    private int LoadStandard(string family, int set, int index, int stages, bool loop = false, bool unprefixed = false)
    {
        if (policy.EarlyProcessGate) return -1;
        string baseKey = unprefixed ? family : SetName(set) + "-" + family, mapKey = baseKey + (!unprefixed && index > 1 ? index.ToString(policy.Culture) : "");
        int handle;
        if ((stages & 4) != 0 && !policy.MapSuppressed && policy.HasSelectedMap && (unprefixed || index != 0))
        {
            if (!Caches.Map.TryGetValue(mapKey, out handle))
            {
                handle = -1;
                foreach (string ext in StandardExtensions)
                {
                    byte[]? bytes = map.Read(mapKey + ext);
                    if (bytes is null && policy.Format < 5) bytes = map.Read(family + ext);
                    if (bytes is null) continue;
                    handle = MapLoad(mapKey + ext, bytes, loop); break; // Stop on bytes, even load -1/0.
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
                    handle = bytes is null ? -1 : loader.Load(new(name, bytes, SampleOrigin.Skin, SampleLoadMode.SkinFile) { Loop = loop });
                    break; // First existing path ends this provider, even decoder/load failure.
                }
                Caches.Skin[baseKey] = handle;
            }
            if (handle != -1) return handle;
        }
        if ((stages & 1) == 0) return -1;
        if (Caches.Embedded.TryGetValue(baseKey, out handle)) return handle;
        byte[]? embedded = defaults.Ui(baseKey) ?? defaults.Gameplay(baseKey);
        handle = embedded is null ? -1 : loader.Load(new(baseKey, embedded, SampleOrigin.Default, SampleLoadMode.EmbeddedBytes) { Loop = loop });
        if (handle == 0) handle = -1; // Embedded-only normalization.
        Caches.Embedded[baseKey] = handle;
        return handle;
    }
    private int MapLoad(string name, byte[] bytes, bool loop = false) => loader.Load(new(name, bytes, SampleOrigin.Map,
        bytes.Length < 1024 ? SampleLoadMode.MapShortCreate : SampleLoadMode.MapBytes) { Loop = loop });
    public int Custom(string name, bool acceptingSetter = true, bool force = false, bool loop = false)
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
        int handle = bytes is null ? -1 : MapLoad(selected, bytes, loop);
        if (Caches.Map.TryGetValue(stem, out int previous) && previous != -1) loader.Free(previous);
        Caches.Map[stem] = handle; return handle;
    }
    public void Prepare(IEnumerable<int> timingIndexes, IEnumerable<int> registeredIndexes, bool includeTicks = false)
    {
        int[] indexes = timingIndexes.Concat(registeredIndexes).Where(i => i > 2).Distinct().ToArray();
        if (indexes.Length > 64) throw new InvalidDataException("Effects exceed ENGINEERING 64 dynamic-index preparation limit.");
        foreach (int index in indexes)
            foreach (int set in new[] { 1, 2, 3 })
            {
                foreach (int bit in LayerOrder) Standard(bit, set, index);
                if (includeTicks) StandardTick(set, index);
            }
        for (int plane = 0; plane < 3; plane++)
            foreach (int set in new[] { 1, 2, 3 })
            {
                foreach (int bit in LayerOrder) table[Row(bit), Bank(set), plane] = Standard(bit, set, plane);
                if (includeTicks) tickTable[Bank(set), plane] = StandardTick(set, plane);
            }
    }
    public int Resolve(int bit, int set, int suppliedIndex)
    {
        int index = Math.Max(suppliedIndex, 0);
        if (index <= 2) return table[Row(bit), Bank(set), index];
        return Caches.Map.TryGetValue(Key(bit, set, index), out int handle) && handle != -1 ? handle : table[Row(bit), Bank(set), 0];
    }
    public int ResolveTick(int set, int suppliedIndex)
    {
        int index = Math.Max(suppliedIndex, 0);
        if (index <= 2) return tickTable[Bank(set), index];
        return Caches.Map.TryGetValue(TickKey(set, index), out int handle) && handle != -1 ? handle : tickTable[Bank(set), 0];
    }
    public int SpinnerResource(bool spin) => LoadStandard(spin ? "spinnerspin" : "spinnerbonus", 0, 0, 7, spin, true);
    public string ContinuingKey(int row, int set, int index = 1) => SetName(set) + "-" + LoopFamily(row) + (index > 1 ? index.ToString(policy.Culture) : "");
    private static string LoopFamily(int row) => row switch { 0 => "sliderslide", 1 => "sliderwhistle", _ => throw new ArgumentOutOfRangeException(nameof(row)) };
    public void PrepareContinuing(IEnumerable<int> indexes)
    {
        int[] dynamicIndexes = indexes.Where(i => i > 2).Distinct().ToArray();
        if (dynamicIndexes.Length > 64) throw new InvalidDataException("Too many continuing sample indexes.");
        foreach (int index in dynamicIndexes)
            for (int set = 1; set <= 3; set++) for (int row = 0; row < 2; row++)
                LoadStandard(LoopFamily(row), set, index, 7, true);
        for (int plane = 0; plane < 3; plane++) for (int set = 1; set <= 3; set++) for (int row = 0; row < 2; row++)
            loopTable[row, Bank(set), plane] = LoadStandard(LoopFamily(row), set, plane, 7, true);
    }
    public int ResolveContinuing(int row, int set, int suppliedIndex)
    {
        int index = Math.Max(0, suppliedIndex);
        if (index <= 2) return loopTable[row, Bank(set), index];
        return Caches.Map.TryGetValue(ContinuingKey(row, set, index), out int h) && h != -1 ? h : loopTable[row, Bank(set), 0];
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
    public void ClearAll() { ClearMap(); ClearSkin(); Clear(Caches.Embedded); Array.Clear(table); Array.Clear(tickTable); Array.Clear(loopTable); }
}

public sealed record CircleSoundPolicy(bool HasSkin, bool LayeredHitSounds, bool HasSelectedMap,
    int SelectedMode, int SongRate, bool RateMatching, int VolumeEffect, int VolumeUniversal);
public sealed record SelectedCircleSample(int Mask, int BaseSet, int AdditionSet, int Index, int Volume);
public enum SoundEventKind { Circle, SliderHead, SliderTick, SliderRepeatEdge, SliderAggregateEdge, SpinnerBonus, SpinnerCompletion }
public sealed record SoundEventIdentity(SoundEventKind Kind, SliderDefinition Parent, int Index, int ScheduleIndex, int UpdateTime);
public sealed record SamplePlaybackRequest(CircleDefinition Source, int Handle, int Layer, int Volume,
    float Pan, float Factor, float Gain)
{
    public SoundEventIdentity? Event { get; init; }
    public SpinnerBonusPlayer? BonusPlayer { get; init; }
    public SpinnerDefinition? Spinner { get; init; }
    public SoundEventKind? SpinnerKind { get; init; }
    public SoundEventKind Kind => SpinnerKind ?? Event?.Kind ?? SoundEventKind.Circle;
}
public interface ISamplePlaybackSink { void Enqueue(SamplePlaybackRequest request); }
public static class CircleSampleSelection
{
    public static SampleTiming? Timing(IReadOnlyList<SampleTiming>? list, int storedStart) => At(list, unchecked(storedStart + 5));
    public static SampleTiming? At(IReadOnlyList<SampleTiming>? list, int query)
    {
        if (list is null || list.Count == 0) return null;
        SampleTiming? selected = null;
        foreach (var point in list) if (point.Time <= (double)query) selected = point;
        return selected ?? list[0];
    }
    public static int Augment(int mask, CircleSoundPolicy policy) =>
        mask == 0 || (policy.HasSkin && policy.LayeredHitSounds && policy.HasSelectedMap && policy.SelectedMode != 3) ? mask | 1 : mask;
    public static SelectedCircleSample Select(CircleSampleMetadata circle, SampleTiming timing, CircleSoundPolicy policy)
    {
        int set = circle.BaseSet != 0 ? circle.BaseSet : timing.Set;
        int index = circle.Index != 0 ? circle.Index : timing.Index;
        int volume = circle.Volume != 0 ? circle.Volume : timing.Volume;
        int addition = set != 0 ? circle.AdditionSet : set;
        int mask = Augment(circle.StoredMask, policy);
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
        if (customHandle != -1) { Submit(circle.Object, customHandle, 0, info.Volume, pan, policy, sink); return; }
        Standard(circle.Object, info, pan, resources, policy, sink);
    }
    public static void Submit(CircleDefinition source, int handle, int layer, int volume, float pan,
        CircleSoundPolicy policy, ISamplePlaybackSink sink)
    {
        if (handle == -1) return; // Zero is still queued, worker validity is separate.
        float factor = policy.SongRate != 100 && policy.RateMatching ? 1f + ((float)policy.SongRate - 100f) / 100f : 1f;
        float global = (float)unchecked(policy.VolumeEffect * policy.VolumeUniversal) / 100f;
        float gain = (volume / 100f) * global / 100f;
        sink.Enqueue(new(source, handle, layer, volume, Math.Clamp(pan, -1f, 1f), factor, gain));
    }
    public static void Standard(CircleDefinition source, SelectedCircleSample info, float pan,
        CircleSampleResources resources, CircleSoundPolicy policy, ISamplePlaybackSink sink)
    {
        int selectedVolume = Math.Max(8, info.Volume);
        foreach (int bit in CircleSampleResources.LayerOrder)
        {
            if ((info.Mask & bit) == 0) continue;
            int set = bit == 1 ? info.BaseSet : info.AdditionSet != 0 ? info.AdditionSet : info.BaseSet;
            int volume = bit == 1 ? (int)(selectedVolume * .8) : bit == 4 ? selectedVolume : (int)(selectedVolume * .85);
            Submit(source, resources.Resolve(bit, set, info.Index), bit, volume, pan, policy, sink);
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

// E039 finite discrete slider selection. Continuing M146b/channel helpers are
// intentionally omitted; their errors/interleavings are not emulated.
public sealed record PreparedSliderSamples(SliderDefinition Parent, SliderSoundMetadata Metadata,
    CircleSampleMetadata Head, int ParentCustomHandle)
{
    public static PreparedSliderSamples Create(SliderDefinition parent, IReadOnlyList<SampleTiming>? timing,
        CircleSoundPolicy policy, int customHandle)
    {
        var metadata = parent.Samples ?? throw new InvalidDataException("Slider sample provenance unavailable.");
        int initial = metadata.Simple ? metadata.RawSound : metadata.EdgeSounds[0];
        var head = new CircleSampleMetadata(parent.Head, initial, initial & 14, 0, 0, 0, 0, "", null, metadata.Error);
        if (metadata.Error is null)
        {
            var edge = SliderSampleSelection.Edge(parent, metadata, timing, policy, 0);
            head = head with { BaseSet = edge.BaseSet, AdditionSet = edge.AdditionSet };
        }
        // Parent index/volume/custom handle/filename are deliberately NOT copied.
        return new(parent, metadata, head, customHandle);
    }
}
public static class SliderSampleSelection
{
    public static SelectedCircleSample Edge(SliderDefinition parent, SliderSoundMetadata data,
        IReadOnlyList<SampleTiming>? timing, CircleSoundPolicy policy, int edge)
    {
        if (data.Error is not null) throw new InvalidDataException(data.Error);
        int query = edge == 0 ? unchecked(parent.Head.StartTime + 5) : edge >= parent.Traversals || edge == -1 ?
            unchecked(parent.EndTime + 5) : unchecked(data.EdgeSubset[edge - 1] + 5);
        SampleTiming? point = CircleSampleSelection.At(timing, query);
        int mask = data.RawSound, set = data.BaseSet, addition = data.AdditionSet;
        if (!data.Simple)
        {
            if (edge == 0)
            {
                mask = data.EdgeSounds.Count == 0 ? mask : data.EdgeSounds[0];
                set = data.Bases.Count == 0 || (data.Format < 14 && data.Bases[0] == 0) ? set : data.Bases[0];
                addition = data.Additions.Count == 0 ? addition : data.Additions[0];
            }
            else
            {
                mask = edge >= data.EdgeSounds.Count ? mask : data.EdgeSounds[edge];
                set = edge >= data.Bases.Count ? set : data.Bases[edge];
                addition = edge >= data.Additions.Count ? addition : data.Additions[edge];
            }
        }
        if (set == 0) set = point?.Set ?? 1;
        if (addition == 0) addition = set;
        return new(CircleSampleSelection.Augment(mask, policy), set, addition, point?.Index ?? 0, point?.Volume ?? 100);
    }
    public static SelectedCircleSample Tick(SliderSoundMetadata data, IReadOnlyList<SampleTiming>? timing, int index)
    {
        if (data.Error is not null) throw new InvalidDataException(data.Error);
        SampleTiming point = CircleSampleSelection.At(timing, unchecked(data.Schedule[index] + 2)) ??
            throw new InvalidDataException("Slider tick requires a selected timing sample record; no edge defaults apply.");
        int set = data.BaseSet != 0 ? data.BaseSet : point.Set;
        return new(data.RawSound, set, data.AdditionSet != 0 ? data.AdditionSet : set, point.Index, point.Volume);
    }
    public static void Request(SliderSoundEvent e, PreparedSliderSamples prepared, IReadOnlyList<SampleTiming>? timing,
        CircleSampleResources resources, CircleSoundPolicy policy, ISamplePlaybackSink sink,
        Action<SelectedCircleSample>? observer = null)
    {
        if (!ReferenceEquals(e.Parent, prepared.Parent)) throw new InvalidDataException("Slider sound source identity mismatch.");
        SoundEventKind kind = e.Kind switch { SliderSoundKind.Head => SoundEventKind.SliderHead, SliderSoundKind.Tick => SoundEventKind.SliderTick,
            SliderSoundKind.RepeatEdge => SoundEventKind.SliderRepeatEdge, _ => SoundEventKind.SliderAggregateEdge };
        var tagged = new Tagged(sink, new(kind, e.Parent, e.Index, e.ScheduleIndex, e.Time));
        if (e.Kind == SliderSoundKind.Head)
        {
            CircleSampleSelection.Request(prepared.Head, timing, -1, resources, policy, tagged, observer);
            return;
        }
        float pan = (e.PresentationX / 512f - .5f) * .800000011920929f;
        if (e.Kind == SliderSoundKind.Tick)
        {
            var info = Tick(prepared.Metadata, timing, e.Index);
            CircleSampleSelection.Submit(e.Parent.Head, resources.ResolveTick(info.BaseSet, info.Index), 0,
                info.Volume, pan, policy, tagged); // Dedicated mode1, no observer/floor/layers.
        }
        else
        {
            var info = Edge(e.Parent, prepared.Metadata, timing, policy, e.Index);
            observer?.Invoke(info);
            CircleSampleSelection.Standard(e.Parent.Head, info, pan, resources, policy, tagged);
        }
    }
    private sealed class Tagged(ISamplePlaybackSink sink, SoundEventIdentity identity) : ISamplePlaybackSink
    { public void Enqueue(SamplePlaybackRequest request) => sink.Enqueue(request with { Event = identity }); }
}

// R13/E040: explicit global state, separate from M1be6 and discrete progression.
public sealed class GlobalSampleTiming
{
    private readonly List<SampleTiming> points;
    public IReadOnlyList<SampleTiming> Points => points;
    public int BaseCursor { get; private set; } = -1;
    public int AllCursor { get; private set; } = -1;
    public int Sentinel { get; private set; } = -999;
    public SampleTiming? Current => AllCursor >= 0 && AllCursor < points.Count ? points[AllCursor] : null;
    public GlobalSampleTiming(IEnumerable<SampleTiming> timing)
    {
        points = timing.ToList();
        if (points.Any(p => !double.IsFinite(p.Time) || p.Set is < 0 or > 3 || p.Index < 0 || p.Volume is < 0 or > 100))
            throw new InvalidDataException("Unsupported continuing timing operands.");
    }
    public void Reset() { BaseCursor = AllCursor = -1; Sentinel = -999; }
    public void Advance(int time, bool rescan = false, int audioState = 1)
    {
        int query = unchecked(time + 5);
        if (points.Count == 0) { Reset(); return; }
        if (rescan) points.Sort((a, b) => a.Time != b.Time ? a.Time.CompareTo(b.Time) : b.Uninherited.CompareTo(a.Uninherited));
        if (rescan || BaseCursor < 0 || audioState != 1)
        {
            BaseCursor = Math.Max(0, points.FindLastIndex(p => p.Uninherited && p.Time < (double)query));
            int old = AllCursor;
            AllCursor = Math.Max(0, points.FindLastIndex(p => p.Time < (double)query));
            if (old != AllCursor) Sentinel = -999;
            return;
        }
        for (int i = unchecked(AllCursor + 1); i < points.Count; i++)
        {
            if (points[i].Time > (double)query) return;
            AllCursor = i;
            if (points[i].Uninherited) BaseCursor = i;
        }
    }
}
public interface IPersistentSampleChannels : IDisposable
{
    int Acquire(int sample, bool onlyNew);
    int Activity(int channel);
    void Attribute(int channel, int attribute, float value);
    void Play(int channel, bool restart);
    void Pause(int channel);
    void Stop(int channel);
}
public sealed record ChannelOperation(string Name, int Channel, int Attribute = 0, float Value = 0);
// Recording implementation: activity is an injected native operand, never an inferred pending flag.
public sealed class RecordingSampleChannels : ISpinnerSampleChannels
{
    public List<ChannelOperation> Operations { get; } = [];
    public Dictionary<int, int> States { get; } = [];
    public Func<int, bool, int> GetChannel { get; set; } = (sample, _) => sample;
    public FrequencyCapability FrequencyCapability { get; set; } = FrequencyCapability.Supported;
    public Func<int, int, long?> QueryLength { get; set; } = (_, _) => null;
    public long? Length(int channel, int mode) { Record(new("Length", channel, mode)); return QueryLength(channel, mode); }
    public Action<ChannelOperation>? OnOperation { get; set; }
    private void Record(ChannelOperation op) { Operations.Add(op); OnOperation?.Invoke(op); }
    public int Acquire(int sample, bool onlyNew) { Record(new("Acquire", sample, 0, onlyNew ? 1 : 0)); return GetChannel(sample, onlyNew); }
    public int Activity(int channel) { Record(new("Activity", channel)); return States.GetValueOrDefault(channel); }
    public void Attribute(int channel, int attribute, float value) => Record(new("Attribute", channel, attribute, value));
    public void Play(int channel, bool restart) => Record(new("Play", channel, 0, restart ? 1 : 0));
    public void Pause(int channel) => Record(new("Pause", channel));
    public void Stop(int channel) => Record(new("Stop", channel));
    public void Dispose() => Record(new("Dispose", 0));
}
public sealed class SharedSliderChannels
{
    private readonly IPersistentSampleChannels backend;
    private readonly Func<int, int, int, int> resolve;
    private readonly Queue<Action> actions = new();
    public SharedSpinnerChannel? Spinner { get; private set; }
    public SharedSpinnerChannel AttachSpinner(ISpinnerSampleChannels channels, Func<int> resolve) =>
        Spinner = new(channels, resolve, () => ReplacementGain, () => StartGain, action => actions.Enqueue(action));
    public int CurrentVolume => Volume;
    public GlobalSampleTiming Timing { get; }
    public int Channel0 { get; set; }
    public int Channel1 { get; set; }
    public int CurrentIndex { get; private set; }
    public int CurrentBase { get; private set; }
    public int LastIndex { get; private set; }
    public int LastBase { get; private set; }
    public int LastAddition { get; private set; }
    public int ApplicationState { get; set; } = 1;
    public int SelectedFormat { get; set; } = 14;
    public int SongFormat { get; set; } = 14;
    public bool HasSelectedMap { get; set; } = true;
    public bool HasSongMap { get; set; } = true;
    public bool SameMapIdentity { get; set; } = true;
    public int FallbackIndex { get; set; }
    public int FallbackVolume { get; set; } = 100;
    public int EffectVolume { get; set; } = 80;
    public int UniversalVolume { get; set; } = 100;
    public int Pending => actions.Count;
    public SharedSliderChannels(GlobalSampleTiming timing, IPersistentSampleChannels backend, Func<int, int, int, int> resolve)
    { Timing = timing; this.backend = backend; this.resolve = resolve; }
    private int SampleIndex => HasSongMap && SongFormat > 3 && Timing.Current is { } p ? p.Index : FallbackIndex;
    private int Volume => !HasSongMap ? 100 : Timing.Current is { } p ? Math.Max(8, p.Volume) : FallbackVolume;
    private float GlobalGain => (float)unchecked(EffectVolume * UniversalVolume) / 100f;
    private float ReplacementGain => ((GlobalGain / 100f) * (float)Volume) / 100f;
    private float StartGain => ((float)Volume * GlobalGain) / 100f / 100f;
    public void Clock(int time, bool rescan = false, int audioState = 1)
    {
        if (!HasSongMap) { Timing.Reset(); return; }
        Timing.Advance(time, rescan, audioState);
        if (Timing.Current is { } p && HasSelectedMap && SelectedFormat > 3) Refresh(p.Set, false);
    }
    public void Refresh(int selectedBase, bool force)
    {
        if (!force && ((selectedBase == CurrentBase && SampleIndex == CurrentIndex) || ApplicationState is not (1 or 2))) return;
        CurrentBase = selectedBase == 0 ? 1 : selectedBase;
        CurrentIndex = SampleIndex;
        Spinner?.Refresh(); // E041 unconditional on each admitted global sample refresh.
        if (force || LastBase == 0) PreparePair(CurrentBase, CurrentBase, force);
    }
    public void PreparePair(int selectedBase, int addition, bool force = false)
    {
        if (selectedBase == 0) selectedBase = CurrentBase;
        if (addition == 0) addition = selectedBase;
        bool change0 = CurrentIndex != LastIndex || selectedBase != LastBase;
        bool change1 = CurrentIndex != LastIndex || addition != LastAddition;
        if (!force && (!(change0 || change1) || ApplicationState is not (1 or 2))) return;
        LastIndex = CurrentIndex; // Original partial mutation survives subsequent exceptions.
        if (change0)
        {
            if (Channel0 != 0 && backend.Activity(Channel0) == 1) backend.Stop(Channel0);
            LastBase = selectedBase;
            Channel0 = backend.Acquire(resolve(0, LastBase, CurrentIndex), false);
        }
        if (change1)
        {
            if (Channel1 != 0 && backend.Activity(Channel1) == 1) backend.Stop(Channel1);
            LastAddition = addition;
            Channel1 = backend.Acquire(resolve(1, LastAddition, CurrentIndex), false);
        }
        float gain = ReplacementGain;
        backend.Attribute(Channel0, 11, 1f); backend.Attribute(Channel0, 2, gain);
        backend.Attribute(Channel1, 2, gain); backend.Attribute(Channel1, 11, 1f);
    }
    public void Submit(int mask, int selectedBase, int addition, bool layered, float? presentationX)
    {
        PreparePair(selectedBase, addition);
        if ((layered || (mask & 2) == 0) && backend.Activity(Channel0) != 1) actions.Enqueue(() =>
        { int target = Channel0; float gain = StartGain; backend.Attribute(target, 2, gain); backend.Play(Channel0, true); });
        if ((mask & 2) != 0 && backend.Activity(Channel1) != 1) actions.Enqueue(() =>
        { int target = Channel1; float gain = StartGain; backend.Attribute(target, 2, gain); backend.Play(Channel1, true); });
        if (presentationX is { } x)
        {
            float pan = (x / 512f - .5f) * .800000011920929f * .4000000059604645f;
            actions.Enqueue(() => { backend.Attribute(Channel0, 3, pan); backend.Attribute(Channel1, 3, pan); });
        }
    }
    public void QueuePause() => actions.Enqueue(() => { backend.Pause(Channel0); backend.Pause(Channel1); });
    public void ManagerTail(bool currentSlider)
    { if (!currentSlider && (backend.Activity(Channel1) == 1 || backend.Activity(Channel0) == 1)) QueuePause(); }
    public void SettingsChanged(int effect, int universal)
    {
        EffectVolume = effect; UniversalVolume = universal;
        float gain = ReplacementGain;
        backend.Attribute(Channel0, 2, gain);
        backend.Attribute(Channel1, 2, gain);
        Spinner?.SettingsGain(gain); // Same captured local, fresh spinner target.
    }
    public void ExecuteNext() { if (actions.TryDequeue(out var action)) action(); }
    public void Drain() { while (actions.Count > 0) ExecuteNext(); }
    // ENGINEERING cancellation discards stale work; original callbacks have no generation veto.
    public void Cancel() => actions.Clear();
}

// E041 frequency support is explicit, separate from recording logical requests.
public enum FrequencyCapability { Supported, Unsupported, Failed }
public interface ISpinnerSampleChannels : IPersistentSampleChannels
{
    long? Length(int channel, int mode); // null = unavailable engineering capability, never encoded-byte size.
    FrequencyCapability FrequencyCapability { get; }
}
public sealed class SharedSpinnerChannel(ISpinnerSampleChannels backend, Func<int> resolve,
    Func<float> replacementGain, Func<float> startGain, Action<Action> enqueue)
{
    public int Channel { get; set; } // F2356 CLR0, not slider row0/row1.
    public FrequencyCapability FrequencyStatus { get; private set; } = backend.FrequencyCapability;
    public string? FrequencyError { get; private set; }
    public float? LastRatio { get; private set; }
    public float? RequestedFrequency { get; private set; }
    public void Refresh()
    {
        if (Channel != 0 && backend.Activity(Channel) == 1) backend.Stop(Channel);
        Channel = backend.Acquire(resolve(), false);
        float gain = replacementGain();
        backend.Attribute(Channel, 11, 1f); backend.Attribute(Channel, 2, gain);
    }
    public void SettingsGain(float captured) => backend.Attribute(Channel, 2, captured);
    public void Modulate(float ratio)
    {
        try
        {
            LastRatio = ratio; RequestedFrequency = null;
            long? length = backend.Length(Channel, 0);
            if (length is null) { Unsupported("Native channel length/frequency unavailable; encoded bytes are not a substitute."); return; }
            if (length.Value <= 8192) return;
            int target = Channel; // Fresh read after Length; invalid handles are not special-cased here.
            float product = 40000f * ratio;
            if (!float.IsFinite(product) || product < int.MinValue || (double)product >= 2147483648.0)
            { Unsupported("Unpinned nonfinite/out-of-range original conv.i4; no frequency fallback."); return; }
            int k = (int)product;
            float frequency = (float)Math.Min(100000, unchecked(20000 + k));
            RequestedFrequency = frequency;
            if (backend.FrequencyCapability != FrequencyCapability.Supported)
            { Unsupported("Audible numeric attribute1 frequency changes unsupported by this backend."); return; }
            backend.Attribute(target, 1, frequency);
            FrequencyStatus = FrequencyCapability.Supported; FrequencyError = null;
        }
        catch (Exception e) { FrequencyStatus = FrequencyCapability.Failed; FrequencyError = e.Message; throw; }
    }
    private void Unsupported(string reason) { FrequencyStatus = FrequencyCapability.Unsupported; FrequencyError = reason; }
    public void Observe(int count, int required, bool enabled)
    { if (enabled) Modulate((float)count / (float)required); }
    public void Continue(float? ratio)
    {
        if (ratio.HasValue) Modulate(ratio.Value);
        enqueue(() =>
        {
            int target = Channel; float gain = startGain();
            backend.Attribute(target, 2, gain); backend.Play(Channel, false);
        }); // No active1 suppression or pending-start flag.
    }
    public void QueuePause() => enqueue(() => backend.Pause(Channel));
}
public sealed record PreparedSpinnerSamples(SpinnerSampleMetadata Metadata, int Custom);
public sealed record SpinnerBonusPlayer(int Sample, float InitialVolume, bool Restart = true, bool Secondary = false);
public static class SpinnerSampleSelection
{
    public static bool BonusAllowed(int count, int required, int modifierBits) =>
        count > unchecked(required + 3) && unchecked(count - unchecked(required + 3)) % 2 == 0 && (modifierBits & 4194304) == 0;
    public static SpinnerBonusPlayer Bonus(SpinnerDefinition spinner, int handle, int globalVolume,
        CircleSoundPolicy policy, ISamplePlaybackSink sink, Action<SpinnerBonusPlayer>? register = null)
    {
        // Named player registration occurs even for -1/0; worker validity is separate.
        var player = new SpinnerBonusPlayer(handle, globalVolume / 100f);
        register?.Invoke(player); // Before deferred player start; missing/0 still registers.
        float global = (float)unchecked(policy.VolumeEffect * policy.VolumeUniversal) / 100f;
        sink.Enqueue(new(new CircleDefinition(SpinnerState.DisplayCenter, spinner.EndTime), handle, 0, globalVolume,
            0, 1, (globalVolume / 100f) * global / 100f)
            { Spinner = spinner, SpinnerKind = SoundEventKind.SpinnerBonus, BonusPlayer = player });
        return player;
    }
    public static void Completion(PreparedSpinnerSamples prepared, IReadOnlyList<SampleTiming>? timing,
        System.Numerics.Vector2 freshCenter, CircleSampleResources resources, CircleSoundPolicy policy, ISamplePlaybackSink sink,
        Action<SelectedCircleSample>? observer = null)
    {
        var data = prepared.Metadata;
        if (data.Payload.Error is not null) throw new InvalidDataException(data.Payload.Error);
        var point = CircleSampleSelection.At(timing, data.Object.EndTime); // Stored end, NO +5.
        if (point is null) return;
        var payload = data.Payload with { Object = new CircleDefinition(freshCenter, data.Object.EndTime) };
        var info = CircleSampleSelection.Select(payload, point, policy); observer?.Invoke(info);
        float pan = (freshCenter.X / 512f - .5f) * .800000011920929f;
        var tagged = new SpinnerSink(sink, data.Object);
        if (prepared.Custom != -1) CircleSampleSelection.Submit(payload.Object, prepared.Custom, 0, info.Volume, pan, policy, tagged);
        else CircleSampleSelection.Standard(payload.Object, info, pan, resources, policy, tagged);
    }
    private sealed class SpinnerSink(ISamplePlaybackSink sink, SpinnerDefinition spinner) : ISamplePlaybackSink
    { public void Enqueue(SamplePlaybackRequest r) => sink.Enqueue(r with { Spinner = spinner, SpinnerKind = SoundEventKind.SpinnerCompletion }); }
}
