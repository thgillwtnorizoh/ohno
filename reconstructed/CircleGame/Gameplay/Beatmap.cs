using System.Globalization;
using System.IO;
using System.Numerics;

namespace Ohno.CircleGame.Gameplay;

// C001 shared syntax contract. ENGINEERING lossless retention, not a claim that
// every retained section/field is interpreted by the pinned original executable.
// Kept in this shared file because historical QA links Beatmap.cs directly.
public sealed record OsuEntry(int LineNumber, string Section, string Text, string? Key, string? Value);
public enum OsuReadStage { MetadataRefresh, RuntimeObjects }
[Flags]
public enum OsuFileSection { Unknown = 0, General = 1, Colours = 2, Editor = 4, Metadata = 8,
    TimingPoints = 16, Events = 32, HitObjects = 64, Difficulty = 128, Variables = 256, All = 511 }
public sealed class OsuDocument
{
    private readonly byte[]? sourceBytes;
    public int FormatVersion { get; }
    public string Header { get; }
    public string FirstLine { get; }
    public IReadOnlyList<string> SourceLines { get; }
    public IReadOnlyList<OsuEntry> Entries { get; }
    public string? SourceSha256 { get; }
    public byte[]? CopySourceBytes() => sourceBytes?.ToArray();
    public string Compatibility => "C001.syntax-v1; semantic admission belongs to each prepared projection";
    private OsuDocument(int version, string header, string first, List<string> lines,
        List<OsuEntry> entries, byte[]? bytes)
    {
        FormatVersion = version; Header = header; FirstLine = first;
        SourceLines = lines.AsReadOnly(); Entries = entries.AsReadOnly(); sourceBytes = bytes;
        SourceSha256 = bytes is null ? null : Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(bytes));
    }
    public static OsuDocument Read(byte[] bytes, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        byte[] snapshot = bytes.ToArray();
        using var stream = new MemoryStream(snapshot, writable: false);
        using var reader = new StreamReader(stream, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        return ReadCore(reader, snapshot, token);
    }
    public static OsuDocument Parse(string text, CancellationToken token = default)
    {
        using var reader = new StringReader(text);
        return ReadCore(reader, null, token);
    }
    private static OsuDocument ReadCore(TextReader reader, byte[]? bytes, CancellationToken token)
    {
        var lines = new List<string>(); var entries = new List<OsuEntry>();
        string section = "", header = ""; int version = 0;
        while (reader.ReadLine() is { } source)
        {
            token.ThrowIfCancellationRequested(); lines.Add(source);
            string line = source.Trim();
            if (header.Length == 0 && line.Length > 0)
            {
                header = source.Trim('\uFEFF', ' ', '\t');
                if (!header.StartsWith("osu file format v", StringComparison.Ordinal) ||
                    !int.TryParse(header[17..], NumberStyles.Integer, CultureInfo.InvariantCulture, out version))
                    throw new InvalidDataException("Missing osu format header.");
                continue;
            }
            if (line.Length == 0 || line.StartsWith("//", StringComparison.Ordinal)) continue;
            if (line.StartsWith('[') && line.EndsWith(']')) { section = line; continue; }
            int colon = line.IndexOf(':');
            entries.Add(new(lines.Count, section, line,
                colon < 0 ? null : line[..colon].Trim(), colon < 0 ? null : line[(colon + 1)..].Trim()));
        }
        token.ThrowIfCancellationRequested();
        if (header.Length == 0) throw new InvalidDataException("Missing osu format header.");
        return new(version, header, lines[0], lines, entries, bytes);
    }
    // Duplicate keys and registration order remain visible through Entries.
    public IEnumerable<OsuEntry> Section(string name) => Entries.Where(e => e.Section == name);

    // C001 Work 1 checkpoint01: M1bc1 and M28f7 are distinct readers. Raw
    // SourceLines remain authoritative; normalized Entries are an engineering view.
    public IEnumerable<OsuEntry> ReadStage(OsuReadStage stage, CancellationToken token = default)
    {
        OsuFileSection section = OsuFileSection.Unknown;
        for (int i = 0; i < SourceLines.Count; i++)
        {
            token.ThrowIfCancellationRequested(); string raw = SourceLines[i];
            if (stage == OsuReadStage.RuntimeObjects && (raw.Length == 0 || raw[0] is ' ' or '_' || raw.StartsWith("//", StringComparison.Ordinal))) continue;
            string line = raw.Trim();
            if (line.Length == 0) continue;
            int colon = line.IndexOf(':');
            string sectionOperand = stage == OsuReadStage.MetadataRefresh && section != OsuFileSection.HitObjects && colon >= 0 ? line[..colon] :
                stage == OsuReadStage.RuntimeObjects ? raw : line;
            if (sectionOperand.StartsWith('['))
            {
                try { section = Enum.Parse<OsuFileSection>(sectionOperand.Trim('[', ']'), ignoreCase: false); }
                catch (Exception e) when (e is ArgumentException or OverflowException) { /* Original retains previous section. */ }
                continue;
            }
            string? value = colon < 0 ? null : line[(colon + 1)..];
            if (stage == OsuReadStage.RuntimeObjects && value is not null)
            {
                int next = value.IndexOf(':'); if (next >= 0) value = value[..next];
            }
            yield return new(i + 1, "[" + section + "]", line,
                colon < 0 ? null : line[..colon].Trim(), value?.Trim());
        }
    }
}

// Fresh M1387/M1bc1 state only. Invariant finite parsing is an explicit engineering
// profile until Work 1 closes F2c2b/native CLR behavior; errors preserve prior writes.
public sealed record OsuMetadataSettings(int Mode, float Hp, float Cs, float Od, float Ar,
    bool ExplicitAr, float StackLeniency, double SliderMultiplier, double SliderTickRate,
    int? ErrorLine, string? Error)
{
    public static OsuMetadataSettings Prepare(OsuDocument document, CancellationToken token = default)
    {
        int mode = 0; float hp = 5, cs = 5, od = 5, ar = 5, stack = .7f;
        bool explicitAr = false; double multiplier = 1.4, tickRate = 1;
        foreach (var entry in document.ReadStage(OsuReadStage.MetadataRefresh, token))
        {
            if (entry.Key is null || entry.Value is null) continue;
            try
            {
                if (entry.Section == "[General]")
                {
                    if (entry.Key == "Mode") { int value = int.Parse(entry.Value, NumberStyles.Integer, CultureInfo.InvariantCulture); mode = value is >= 0 and <= 3 ? value : 0; }
                    else if (entry.Key == "StackLeniency") stack = Math.Clamp(Single(entry.Value), 0, 1);
                }
                else if (entry.Section == "[Difficulty]")
                {
                    switch (entry.Key)
                    {
                        case "HPDrainRate": hp = Difficulty(entry.Value, 0, 10); break;
                        case "CircleSize": cs = Difficulty(entry.Value, document.FormatVersion >= 13 && mode == 3 ? 1 : 0, document.FormatVersion >= 13 && mode == 3 ? 18 : 10); break;
                        case "OverallDifficulty": od = Difficulty(entry.Value, 0, 10); if (!explicitAr) ar = od; break;
                        case "ApproachRate": ar = Difficulty(entry.Value, 0, 10); explicitAr = true; break;
                        case "SliderMultiplier": multiplier = Math.Clamp(Double(entry.Value), .4, 3.6); break;
                        case "SliderTickRate": tickRate = Math.Clamp(Double(entry.Value), .5, 8); break;
                    }
                }
            }
            catch (Exception e) when (e is FormatException or OverflowException or InvalidDataException)
            { return new(mode, hp, cs, od, ar, explicitAr, stack, multiplier, tickRate, entry.LineNumber, e.Message); }
        }
        return new(mode, hp, cs, od, ar, explicitAr, stack, multiplier, tickRate, null, null);
        float Difficulty(string value, float min, float max) => Math.Clamp(document.FormatVersion < 13 ?
            byte.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture) : Single(value), min, max);
        static float Single(string value)
        {
            float v = float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
            return float.IsFinite(v) ? v : throw new InvalidDataException("Nonfinite metadata is outside the finite C001 profile.");
        }
        static double Double(string value)
        {
            double v = double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
            return double.IsFinite(v) ? v : throw new InvalidDataException("Nonfinite metadata is outside the finite C001 profile.");
        }
    }
}

public sealed record CircleDefinition(Vector2 Position, int StartTime);
public sealed record SpinnerDefinition(int StartTime, int EndTime, int HitSound, string? HitSample);

public sealed record SliderSegment(Vector2 From, Vector2 To, bool IsSubcurveEnd, bool IsLinear);

public sealed record SliderGeometry(IReadOnlyList<Vector2> Controls, IReadOnlyList<SliderSegment> RawSegments,
    IReadOnlyList<SliderSegment> Segments, IReadOnlyList<double> CumulativeLengths,
    double UnadjustedLength, double RequestedLength);

// E32 finite v14 M144a -> M2d62 -> M2d93. Kept with the shared model so the
// existing QA projects continue linking the actual implementation unchanged.
public static class SliderGeometryPreparation
{
    public const int MaxControls = 64, MaxSegments = 8192, MaxSubdivisionWork = 50000;

    public static SliderGeometry Prepare(Vector2 head, IReadOnlyList<Vector2> supplied, bool bezier, double requested, int format = 14)
    {
        var controls = supplied.ToList();
        if (controls.Count == 0 || controls[0] != head) controls.Insert(0, head);
        if (controls.Count > MaxControls) throw new InvalidDataException("R06 allows at most 64 normalized controls per slider.");
        var raw = new List<SliderSegment>();
        int work = 0;
        if (!bezier) Emit(controls, true);
        else
        {
            // E032: <=6 includes the second duplicate in the preceding subcurve;
            // >6 uses forward equality and skips to the second duplicate.
            int start = 0;
            for (int i = 0; i < controls.Count; i++)
            {
                bool separator = format <= 6 ? i > 0 && controls[i] == controls[i - 1] :
                    i < controls.Count - 2 && controls[i] == controls[i + 1];
                if (!separator && i != controls.Count - 1) continue;
                var range = controls.GetRange(start, i - start + 1);
                bool linear = format > 8 && range.Count == 2;
                Emit(linear ? range : format == 9 ? Uniform(range) : Flatten(range, ref work), linear);
                start = format <= 6 ? i : ++i;
            }
        }
        double originalLength = 0;
        foreach (var s in raw) originalLength += Length(s);
        if (!double.IsFinite(originalLength) || originalLength <= 0)
            throw new InvalidDataException("R06 requires a finite nonzero raw slider path.");
        var prepared = raw.ToList();
        double excess = originalLength - requested;
        while (prepared.Count > 0)
        {
            var s = prepared[^1];
            float d = Length(s);
            if (d <= excess + 0.0001)
            {
                prepared.RemoveAt(prepared.Count - 1); excess -= d; continue;
            }
            // E06: a zero-direction final piece blocks extension; do not skip it.
            if (s.To != s.From)
                prepared[^1] = s with { To = s.From + Vector2.Normalize(s.To - s.From) * (d - (float)excess) };
            break;
        }
        if (prepared.Count == 0) throw new InvalidDataException("Slider geometry vanished during requested-length adjustment.");
        var cumulative = new List<double>();
        double total = 0;
        foreach (var s in prepared) { total += Length(s); cumulative.Add(total); }
        if (!double.IsFinite(total) || total <= 0) throw new InvalidDataException("Unsupported adjusted slider geometry.");
        return new(controls.AsReadOnly(), raw.AsReadOnly(), prepared.AsReadOnly(), cumulative.AsReadOnly(), originalLength, requested);

        void Emit(IReadOnlyList<Vector2> points, bool linear)
        {
            for (int j = 1; j < points.Count; j++)
            {
                if (raw.Count >= MaxSegments) throw new InvalidDataException("R06 allows at most 8192 raw segments per slider.");
                Vector2 a = points[j - 1], b = points[j];
                raw.Add(linear ? new(a + (b - a) * 0f, a + (b - a) * 1f, true, true) :
                    new(a, b, j == points.Count - 1, false));
            }
        }
    }

    // E032 M2d64: v9 only, 50*n samples, s<steps; deliberately no t=1.
    private static IReadOnlyList<Vector2> Uniform(IReadOnlyList<Vector2> controls)
    {
        int steps = checked(50 * controls.Count);
        var output = new List<Vector2>(steps);
        for (int s = 0; s < steps; s++)
        {
            float t = (float)s / steps;
            Vector2[] scratch = controls.ToArray();
            for (int level = controls.Count - 1; level > 0; level--)
                for (int j = 0; j < level; j++) scratch[j] = scratch[j] + (scratch[j + 1] - scratch[j]) * t;
            output.Add(scratch[0]);
        }
        return output;
    }

    // Raw flattener exposed for the two E32 finite preparation targets, not gameplay.
    public static IReadOnlyList<Vector2> FlattenBezier(IReadOnlyList<Vector2> source)
    {
        int work = 0;
        return Flatten(source, ref work).AsReadOnly();
    }

    private static List<Vector2> Flatten(IReadOnlyList<Vector2> source, ref int work)
    {
        if (source.Count == 0 || source.Count > MaxControls)
            throw new InvalidDataException("R06 flattener requires 1–64 finite controls.");
        if (source.Any(p => !float.IsFinite(p.X) || !float.IsFinite(p.Y)))
            throw new InvalidDataException("Bézier controls must be finite.");
        var output = new List<Vector2>();
        var pending = new Stack<Vector2[]>();
        pending.Push(source.ToArray());
        while (pending.Count > 0)
        {
            if (++work > MaxSubdivisionWork) throw new InvalidDataException("R06 Bézier subdivision work limit (50000) exceeded.");
            Vector2[] p = pending.Pop();
            bool flat = true;
            for (int i = 1; i < p.Length - 1; i++)
            {
                Vector2 d = (p[i - 1] - p[i] * 2f) + p[i + 1];
                float squared = d.X * d.X + d.Y * d.Y;
                if (!float.IsFinite(squared)) throw new InvalidDataException("Nonfinite Bézier flatness operand.");
                if (squared > 0.25f) { flat = false; break; }
            }
            var (left, right) = Split(p);
            if (!flat) { pending.Push(right); pending.Push(left); continue; }
            // M2d92: left + right[1..], then the weighted approximation, NOT uniform t.
            var buffer = new Vector2[2 * p.Length - 1];
            Array.Copy(left, buffer, p.Length);
            Array.Copy(right, 1, buffer, p.Length, p.Length - 1);
            Append(p[0]);
            for (int i = 1; i < p.Length - 1; i++)
                Append(((buffer[2 * i - 1] + buffer[2 * i] * 2f) + buffer[2 * i + 1]) * 0.25f);
        }
        Append(source[^1]); // Only the traversal appends the source endpoint.
        return output;

        void Append(Vector2 p)
        {
            if (!float.IsFinite(p.X) || !float.IsFinite(p.Y) || output.Count >= MaxSegments + 1)
                throw new InvalidDataException("R06 Bézier output exceeds finite geometry/8192-segment bounds.");
            output.Add(p);
        }
    }

    private static (Vector2[] Left, Vector2[] Right) Split(Vector2[] source)
    {
        var scratch = (Vector2[])source.Clone();
        var left = new Vector2[source.Length];
        var right = new Vector2[source.Length];
        for (int level = 0; level < source.Length; level++)
        {
            left[level] = scratch[0]; right[source.Length - level - 1] = scratch[source.Length - level - 1];
            for (int j = 0; j < source.Length - level - 1; j++) scratch[j] = (scratch[j] + scratch[j + 1]) * (1f / 2f);
        }
        return (left, right);
    }

    public static float Length(SliderSegment s)
    {
        Vector2 d = s.To - s.From;
        return (float)Math.Sqrt((double)(d.X * d.X + d.Y * d.Y));
    }
}

public sealed record SliderMovement(Vector2 From, Vector2 To, int StartTime, int EndTime)
{
    // E15/E21: inclusive interval, Single interpolation, zero-duration endpoint.
    public Vector2 Position(int time) => EndTime == StartTime ? To : From + (To - From) *
        (1f - (float)(EndTime - time) / (EndTime - StartTime));
}

// Producer identity is UI metadata; original judgement selects raw codes from k/N/R.
public enum SliderCheckpointKind { Interior, Repeat, Final }
public sealed record SliderCheckpoint(int Time, SliderCheckpointKind Kind, int Traversal, Vector2 Position);

public sealed record SliderDefinition(CircleDefinition Head, Vector2 Tail,
    int EndTime, double TravelRate, double PixelLength, IReadOnlyList<Vector2> ControlPoints,
    int Traversals, IReadOnlyList<SliderMovement> Movements, IReadOnlyList<SliderCheckpoint> Checkpoints)
{
    public SliderSoundMetadata? Samples { get; init; }
    public SliderGeometry Geometry { get; init; } = null!; // Always supplied by the parser.
    public int TerminalTime => Checkpoints[^1].Time;
    public Vector2 EndPosition => Movements[^1].To;
    public int TraversalIndex(int time) => MovementIndex(time) / Geometry.Segments.Count;
    public SliderMovement TraversalEndMovement(int traversal) => Movements[(traversal + 1) * Geometry.Segments.Count - 1];

    // First match wins at the shared endpoint. Late queries retain extrapolation.
    // Before-start fallback is for this client's ready-screen rendering only.
    public int MovementIndex(int time)
    {
        for (int i = 0; i < Movements.Count; i++)
            if (Movements[i].StartTime <= time && time <= Movements[i].EndTime) return i;
        return time < Movements[0].StartTime ? 0 : Movements.Count - 1;
    }
    public Vector2 RequiredPosition(int time) => Movements[MovementIndex(time)].Position(time);
    public Vector2 BallPosition(int time) => RequiredPosition(Math.Clamp(time, Head.StartTime, EndTime));
}

public sealed record TimingPoint(double Time, double BeatLength, bool IsBase);

// E034 fresh-file metadata, deliberately independent of sorted gameplay objects,
// prepared slider ends and the ledger's component/whole-result counters.
public sealed record LegacyScoreMetadata(int FormatVersion, int RowCount, int FirstStart, int LastStart,
    int LastEnd, int BreakMilliseconds, int WholeSeconds, ushort Circles, ushort Sliders, ushort Spinners,
    float Hp, float Od, float Cs)
{
    public int? PartialErrorLine { get; init; }
    public string? PartialError { get; init; }
    public LegacyScoreMultiplier Prepare()
    {
        if (!float.IsFinite(Hp) || !float.IsFinite(Od) || !float.IsFinite(Cs) ||
            Hp < 0 || Hp > 10 || Od < 0 || Od > 10 || Cs < 0 || Cs > 10)
            throw new InvalidDataException("Automatic legacy multiplier requires finite prepared HP/OD/CS within 0–10.");
        // M1bc4: preserve Single operations; positive N/0 is supported infinity.
        float density = (float)RowCount / (float)WholeSeconds;
        density *= 8f;
        // M2d7d finite bounds: infinities clamp, NaN propagates.
        if (density > 16f) density = 16f;
        else if (density < 0f) density = 0f;
        float u = Hp + Od;
        u += Cs;
        u += density;
        u /= 38f;
        u *= 5f;
        double rounded = Math.Round((double)u); // Original Double overload, midpoint-to-even.
        if (!double.IsFinite(rounded) || rounded < int.MinValue || rounded >= 2147483648.0)
            throw new InvalidDataException("Automatic legacy multiplier is unsupported: nonfinite/out-of-range E034 conversion (including 0/0).");
        return new(this, density, u, (int)rounded);
    }

    // Only a fresh file; no cache restore, reused difficulty or partial-error recovery.
    // Malformed metadata rejects explicitly instead of fabricating an original result.
    public static LegacyScoreMetadata ReadFresh(IEnumerable<string> lines, int version)
        => ReadFreshEntries(LegacyEntries(lines), version);
    public static LegacyScoreMetadata ReadFresh(OsuDocument document, bool originalStages = false)
        => ReadFreshEntries(originalStages ? document.ReadStage(OsuReadStage.MetadataRefresh) : document.Entries, document.FormatVersion, originalStages);
    // Compatibility adapter for independent callers supplying headerless lines.
    // Production loading uses the already-tokenized canonical document instead.
    private static IEnumerable<OsuEntry> LegacyEntries(IEnumerable<string> lines)
    {
        string section = ""; int number = 0;
        foreach (string source in lines)
        {
            number++; string line = source.Trim();
            if (line.Length == 0 || line.StartsWith("//")) continue;
            if (line.StartsWith('[') && line.EndsWith(']')) { section = line; continue; }
            yield return new(number, section, line, null, null);
        }
    }
    private static LegacyScoreMetadata ReadFreshEntries(IEnumerable<OsuEntry> entries, int version, bool allowPartial = false)
    {
        if (version < 5 || version > 14) throw new InvalidDataException("Unsupported metadata format version.");
        int count = 0, first = -1, breaks = 0;
        ushort circles = 0, sliders = 0, spinners = 0;
        float hp = 5, od = 5, cs = 5; // M1387 fresh object defaults, not cached reuse.
        string lastStart = "", lastEnd = ""; int? partialLine = null; string? partialError = null;
        foreach (var entry in entries)
        {
            try
            {
            string line = entry.Text, section = entry.Section;
            if (section == "[HitObjects]")
            {
                string[] row = line.Split(',', 7);
                if (row.Length < 4) throw new InvalidDataException("Invalid automatic score metadata hit-object row.");
                if (first == -1) first = Stamp(row[2]); // Before type dispatch, raw order.
                int type = Integer(row[3]) & 139;
                switch (type)
                {
                    case 1: circles = unchecked((ushort)(circles + 1)); lastStart = lastEnd = row[2]; break;
                    case 2: sliders = unchecked((ushort)(sliders + 1)); lastStart = lastEnd = row[2]; break;
                    case 8:
                        if (row.Length < 6) throw new InvalidDataException("Spinner metadata needs an end field.");
                        spinners = unchecked((ushort)(spinners + 1)); lastStart = row[2]; lastEnd = row[5]; break;
                    case 128:
                        if (row.Length < 6) throw new InvalidDataException("Hold metadata needs an end field.");
                        sliders = unchecked((ushort)(sliders + 1)); lastStart = lastEnd = row[5].Split(':')[0]; break;
                }
                count = unchecked(count + 1); // Includes unrecognized masked types, NOT count sum.
            }
            else if (section == "[Events]" && line[0] == '2')
            {
                string[] row = line.Split(',');
                if (row.Length < 3) throw new InvalidDataException("Numeric event2 needs start/end break fields.");
                int duration = unchecked(Integer(row[2]) - Integer(row[1]));
                breaks = unchecked(breaks + duration); // No clipping, positivity or overlap union.
            }
            else if (section == "[Difficulty]")
            {
                int colon = line.IndexOf(':');
                if (colon < 0) continue;
                string key = line[..colon].Trim(), value = line[(colon + 1)..].Trim();
                if (key is not ("HPDrainRate" or "OverallDifficulty" or "CircleSize")) continue;
                float difficulty;
                if (version < 13)
                {
                    if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out byte b))
                        throw new InvalidDataException("Pre-v13 score difficulty requires Byte syntax.");
                    difficulty = b;
                }
                else if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out difficulty) || !float.IsFinite(difficulty))
                    throw new InvalidDataException("Automatic score difficulty must be finite Single syntax.");
                difficulty = Math.Clamp(difficulty, 0f, 10f);
                switch (key) { case "HPDrainRate": hp = difficulty; break; case "OverallDifficulty": od = difficulty; break; case "CircleSize": cs = difficulty; break; }
            }
            }
            catch (Exception e) when (allowPartial && e is InvalidDataException or FormatException or OverflowException)
            { partialLine = entry.LineNumber; partialError = e.Message; break; }
        }
        int last = lastStart.Length == 0 ? -1 : Stamp(lastStart);
        int end = lastEnd.Length == 0 ? -1 : Stamp(lastEnd);
        int seconds = unchecked(unchecked(last - first) - breaks) / 1000;
        return new(version, count, first, last, end, breaks, seconds, circles, sliders, spinners, hp, od, cs) { PartialErrorLine = partialLine, PartialError = partialError };

        static int Integer(string text) => int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int n) ? n :
            throw new InvalidDataException("Invalid Int32 score metadata operand.");
        static int Stamp(string text)
        {
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double n) ||
                !double.IsFinite(n) || n < int.MinValue || n > int.MaxValue)
                throw new InvalidDataException("Unsupported finite Double-to-Int32 score metadata time.");
            return (int)n;
        }
    }
}

public sealed record LegacyScoreMultiplier(LegacyScoreMetadata Metadata, float DensityTerm, float BeforeRounding, int K);

// R09 preserves parser provenance for E035; this is not inferred from judgements.
public sealed record HealthObjectSource(int Start, int RawType)
{
    public OriginalRegisteredType? RegisteredType { get; init; }
}
public sealed record HealthBreak(int Start, int End);
public sealed record HealthMapSource(int Version, IReadOnlyList<HealthObjectSource> Rows, IReadOnlyList<HealthBreak> Breaks)
{
    public bool OriginalFactoryPriority { get; init; }
    // ENGINEERING ownership proof for the exact fresh parsed map. A record clone
    // must not silently inherit a claim of fresh original registration identity.
    private WeakReference<Beatmap>? freshOwner;
    internal void BindFreshOwner(Beatmap map) => freshOwner = new(map);
    public bool IsFreshFor(Beatmap map) => freshOwner is not null && freshOwner.TryGetTarget(out Beatmap? owner) &&
        ReferenceEquals(owner, map) && ReferenceEquals(map.HealthSource, this);
}

public enum HealthPreparationPolicy { HistoricalR09, E036FreshOrdinary }
public enum OriginalRegisteredType { Null, Circle, Slider, SpinnerBase, AssignableT081f, Unknown }
public enum NewComboOrigin { FreshOrdinaryText, RetainedManager, CloneOrEditor, AlternateFactory, Unknown }
public readonly record struct NewComboContext(NewComboOrigin Origin, OriginalRegisteredType InitialLast, bool SharedTimeDomain)
{
    public bool OriginalFactoryPriority { get; init; }
    // Current parser's explicit zero-shift domain for both objects and breaks.
    public static NewComboContext FreshZeroShift => new(NewComboOrigin.FreshOrdinaryText, OriginalRegisteredType.Null, true);
}
public static class OriginalObjectClassification
{
    // M28f7 priority differs from M1bc1's exact masked-category summary switch.
    public static OriginalRegisteredType Runtime(int raw) => (raw & 1) != 0 ? OriginalRegisteredType.Circle :
        (raw & 2) != 0 ? OriginalRegisteredType.Slider : (raw & 8) != 0 ? OriginalRegisteredType.SpinnerBase : OriginalRegisteredType.Unknown;
}
public sealed record NewComboInput(object Identity, int Start, int RawType, OriginalRegisteredType RuntimeType);
public sealed record PreparedNewCombo(int SourceIndex, object Identity, int Start, int RawType,
    OriginalRegisteredType RuntimeType, OriginalRegisteredType LastRegisteredBefore, bool FileBit4,
    bool? Inferred, bool? FactoryArgument, bool ConstructorFlag, bool ParserFlag,
    int ExpiredBreaksConsumed, bool FinalFlag)
{
    public int FileColourSkip => (RawType >> 4) & 7;
}

// E036 M2902 / M28f7 / M28d2 / M28db. This pure projection has no game or score state.
public static class NewComboPreparation
{
    public static bool Infer(OriginalRegisteredType last) => last switch
    {
        OriginalRegisteredType.Null or OriginalRegisteredType.SpinnerBase or OriginalRegisteredType.AssignableT081f => true,
        OriginalRegisteredType.Circle or OriginalRegisteredType.Slider => false,
        _ => throw Unsupported("Unknown last registered runtime identity.")
    };
    public static IReadOnlyList<PreparedNewCombo> Prepare(IReadOnlyList<NewComboInput> registeredSource,
        IReadOnlyList<HealthBreak> preparedBreaks, int format, NewComboContext context)
    {
        if (context.Origin != NewComboOrigin.FreshOrdinaryText || context.InitialLast != OriginalRegisteredType.Null || !context.SharedTimeDomain)
            throw Unsupported("R10 requires proven fresh ordinary text registration and shared adjusted time domain; retained/clone/editor/alternate state is unavailable.");
        if (registeredSource.Count == 0 || registeredSource.Count > 10000 || format < 5 || format > 14)
            throw Unsupported("Unsupported finite NewCombo source count/format.");
        var seen = new HashSet<object>(ReferenceEqualityComparer.Instance);
        var prepared = new List<PreparedNewCombo>();
        OriginalRegisteredType last = context.InitialLast;
        for (int i = 0; i < registeredSource.Count; i++)
        {
            NewComboInput row = registeredSource[i];
            if (row.Identity is null || !seen.Add(row.Identity)) throw Unsupported("Missing/duplicate registered object identity.");
            if (i > 0 && row.Start <= registeredSource[i - 1].Start)
                throw Unsupported("R10 requires strictly increasing source/registered starts; no sorting or equal-start inference.");
            int masked = row.RawType & -113;
            OriginalRegisteredType expected = context.OriginalFactoryPriority ? OriginalObjectClassification.Runtime(row.RawType) : (masked & ~4) switch
            {
                1 => OriginalRegisteredType.Circle, 2 => OriginalRegisteredType.Slider, 8 => OriginalRegisteredType.SpinnerBase,
                _ => throw Unsupported("Unsupported ordinary registered type/factory.")
            };
            if (expected == OriginalRegisteredType.Unknown || row.RuntimeType != expected) throw Unsupported("Original runtime type is inconsistent/alternate, not an ordinary row category.");
            bool fileBit = (masked & 4) > 0;
            bool? inferred = null, argument = null;
            bool constructor, parser;
            if (row.RuntimeType == OriginalRegisteredType.SpinnerBase)
            {
                constructor = format <= 8;
                parser = fileBit; // M3e57 overwrites constructor flag; no M2902 call.
            }
            else
            {
                inferred = Infer(last); // Always reached BEFORE OR, even when fileBit is true.
                argument = inferred.Value | fileBit;
                constructor = parser = argument.Value;
            }
            prepared.Add(new(i, row.Identity, row.Start, row.RawType, row.RuntimeType, last, fileBit,
                inferred, argument, constructor, parser, 0, parser));
            last = row.RuntimeType; // Only after this object's successful bounded registration.
        }
        int consumed = -1;
        for (int i = 0; i < prepared.Count; i++)
        {
            int count = 0;
            while (consumed + 1 < preparedBreaks.Count && preparedBreaks[consumed + 1].End < prepared[i].Start)
            {
                consumed++; count++;
            }
            prepared[i] = prepared[i] with { ExpiredBreaksConsumed = count, FinalFlag = prepared[i].ParserFlag || count > 0 };
        }
        return prepared.AsReadOnly();
    }
    private static InvalidDataException Unsupported(string reason) => new("Health unavailable: " + reason);
}


// R11 additive metadata. Definitions/positional constructors remain unchanged.
public sealed record CircleSampleMetadata(CircleDefinition Object, int RawSound, int StoredMask,
    int BaseSet, int AdditionSet, int Index, int Volume, string Filename, string? RawPayload, string? Error)
{
    public static CircleSampleMetadata Parse(CircleDefinition circle, int raw, string? payload)
    {
        int baseSet = 0, addition = 0, index = 0, volume = 0; string filename = "";
        string? error = null;
        if (!string.IsNullOrEmpty(payload))
        {
            try
            {
                string[] parts = payload.Split(':');
                if (parts.Length < 2) throw new FormatException("Sample payload needs base and addition selectors.");
                baseSet = Convert.ToInt32(parts[0], CultureInfo.InvariantCulture);
                addition = Convert.ToInt32(parts[1], CultureInfo.InvariantCulture);
                if (parts.Length > 2) index = Convert.ToInt32(parts[2], CultureInfo.InvariantCulture);
                if (parts.Length > 3) volume = Math.Clamp(Convert.ToInt32(parts[3], CultureInfo.InvariantCulture), 0, 100);
                if (parts.Length > 4) filename = parts[4];
            }
            catch (Exception e) when (e is FormatException or OverflowException)
            { error = "Malformed circle sample metadata: " + e.Message; }
        }
        return new(circle, raw, raw & 14, baseSet, addition, index, volume, filename, payload, error);
    }
}
// E041 spinner factory sidecar; raw mask is never circle-constructor normalized.
public sealed record SpinnerSampleMetadata(SpinnerDefinition Object, int SourceOrder, CircleSampleMetadata Payload);
// E039 parent fields remain distinct from circle-normalized embedded-head fields.
public sealed record SliderSoundMetadata(int RawSound, int RawTraversals, int EffectiveTraversals,
    int Format, int SourceOrder, string? RawEdgeSounds, string? RawEdgeSets, string? RawPayload,
    bool Simple, IReadOnlyList<int> EdgeSounds, IReadOnlyList<int> Bases, IReadOnlyList<int> Additions,
    int BaseSet, int AdditionSet, int Index, int Volume, string Filename, string? Error)
{
    public IReadOnlyList<int> Schedule { get; init; } = Array.Empty<int>();
    public IReadOnlyList<int> EdgeSubset { get; init; } = Array.Empty<int>();
    public bool CustomLoopFlag => Filename.IndexOf("slider") > 0 && Filename.IndexOf("tick") == -1;
    public static SliderSoundMetadata Parse(int sound, int rawTraversals, int format, int sourceOrder,
        string? edgeSounds, string? edgeSets, string? payload)
    {
        if (rawTraversals is < 1 or > 16) throw new InvalidDataException("R12 retains the ENGINEERING 1–16 traversal admission bound.");
        var sounds = new List<int>(); var bases = new List<int>(); var additions = new List<int>();
        int baseSet = 0, addition = 0, index = 0, volume = 0; string filename = ""; string? error = null;
        bool simple = string.IsNullOrEmpty(edgeSounds);
        try
        {
            if (!simple)
            {
                foreach (string field in edgeSounds!.Split('|').Take(rawTraversals + 1))
                { int.TryParse(field, out int parsed); sounds.Add(parsed); } // Ignored TryParse return is original.
                while (sounds.Count < rawTraversals + 1) sounds.Add(sound);
            }
            if (!string.IsNullOrEmpty(edgeSets))
                foreach (string field in edgeSets.Split('|'))
                {
                    string[] parts = field.Split(':');
                    bases.Add(Convert.ToInt32(parts[0])); additions.Add(Convert.ToInt32(parts[1]));
                }
            if (!simple)
            {
                if (bases.Count > rawTraversals + 1) bases.RemoveRange(rawTraversals + 1, bases.Count - rawTraversals - 1);
                if (additions.Count > rawTraversals + 1) additions.RemoveRange(rawTraversals + 1, additions.Count - rawTraversals - 1);
                while (bases.Count < rawTraversals + 1) bases.Add(0);
                while (additions.Count < rawTraversals + 1) additions.Add(0);
            }
            if (payload is not null)
            {
                string[] parts = payload.Split(':');
                baseSet = Convert.ToInt32(parts[0]); addition = Convert.ToInt32(parts[1]);
                if (parts.Length > 2) index = Convert.ToInt32(parts[2]);
                if (parts.Length > 3) volume = Math.Clamp(Convert.ToInt32(parts[3]), 0, 100);
                if (parts.Length > 4) filename = parts[4];
            }
        }
        catch (Exception e) when (e is FormatException or OverflowException or IndexOutOfRangeException)
        { error = "Slider samples unavailable: malformed edge/payload metadata: " + e.Message; }
        return new(sound, rawTraversals, Math.Max(1, rawTraversals), format, sourceOrder, edgeSounds, edgeSets, payload,
            simple, sounds.AsReadOnly(), bases.AsReadOnly(), additions.AsReadOnly(), baseSet, addition, index, volume, filename, error);
    }
    public SliderSoundMetadata Prepare(SliderDefinition slider)
    {
        int count = slider.Checkpoints.Count, h = count / slider.Traversals;
        if (h <= 0) throw new InvalidDataException("Unsupported slider sound schedule divisor.");
        return this with { Schedule = slider.Checkpoints.Select(c => c.Time).ToArray(),
            EdgeSubset = slider.Checkpoints.Take(count - 1).Where((_, i) => (i + 1) % h == 0).Select(c => c.Time).ToArray() };
    }
}
public sealed record SampleTiming(double Time, int Set, int Index, int Volume, bool Uninherited, int SourceOrder);
public sealed record SamplePreparationOperand(CircleDefinition? Circle, int Index, string Filename, string? Error)
{
    public SpinnerDefinition? Spinner { get; init; }
    public SliderDefinition? Slider { get; init; }
}
public sealed record CircleSoundMap(string Directory, int Format, IReadOnlyList<CircleSampleMetadata> Circles,
    IReadOnlyList<SampleTiming> Timing, string? TimingError)
{
    public IReadOnlyList<SpinnerSampleMetadata> Spinners { get; init; } = Array.Empty<SpinnerSampleMetadata>();
    // Shared resource preloading only. No slider/spinner playback hook is added.
    public IReadOnlyList<SamplePreparationOperand> RegisteredOverrides { get; init; } = Array.Empty<SamplePreparationOperand>();
}

public sealed record MapDisplayMetadata(string? Difficulty, string? Artist);
// Explicit application request selects C001 interpretation and carries cancellation.
// String callers retain the historical compatibility admission profile.
public readonly record struct CanonicalBeatmapPath(string Value, CancellationToken Token)
{
    public static implicit operator string(CanonicalBeatmapPath path) => path.Value;
    public override string ToString() => Value;
}

public sealed record Beatmap(string Title, float CircleSize, float OverallDifficulty,
    float ApproachRate, IReadOnlyList<CircleDefinition> Circles)
{
    public OsuDocument? Document { get; init; }
    public OsuMetadataSettings? MetadataSettings { get; init; }
    public LegacyScoreMetadata? ScoreMetadata { get; init; }
    public MapDisplayMetadata? DisplayMetadata { get; init; }
    public HealthMapSource? HealthSource { get; init; }
    public CircleSoundMap? CircleSounds { get; init; }

    // Optional local media reference; the gameplay model remains independent of WPF.
    public string? AudioPath { get; init; }
    public IReadOnlyList<SliderDefinition> Sliders { get; init; } = Array.Empty<SliderDefinition>();
    public IReadOnlyList<SpinnerDefinition> Spinners { get; init; } = Array.Empty<SpinnerDefinition>();

    // Historical R02-R17 API keeps its admission boundary for existing callers.
    public static Beatmap Load(string path, CancellationToken token = default)
        => LoadCore(path, token, extendedPaths: false);

    // C001 application entry point. Both APIs share syntax and preparation;
    // only the historical Bezier admission guard differs.
    public static Beatmap LoadCanonical(string path, CancellationToken token = default)
        => LoadCore(path, token, extendedPaths: true);
    public static Beatmap Load(CanonicalBeatmapPath path) => LoadCanonical(path.Value, path.Token);

    private static Beatmap LoadCore(string path, CancellationToken token, bool extendedPaths)
    {
        if (new FileInfo(path).Length > 2 * 1024 * 1024)
            throw new InvalidDataException("R02 accepts beatmaps up to 2 MB.");
        token.ThrowIfCancellationRequested();
        var document = OsuDocument.Read(File.ReadAllBytes(path), token);
        int version = document.FormatVersion;
        if (version < 5 || version > 14)
            throw new InvalidDataException("Use an osu file format v5–v14 supported circle/Linear-slider map.");

        var fields = new Dictionary<string, string>(StringComparer.Ordinal);
        OsuMetadataSettings? settings = extendedPaths ? OsuMetadataSettings.Prepare(document, token) : null;
        if (settings?.Error is { } metadataError)
            throw new InvalidDataException($"Metadata line {settings.ErrorLine}: {metadataError}");
        if (extendedPaths)
            foreach (var entry in document.ReadStage(OsuReadStage.MetadataRefresh, token))
                if (entry.Section is "[General]" or "[Metadata]" or "[Difficulty]" && entry.Key is not null && entry.Value is not null)
                    fields[entry.Section + entry.Key] = entry.Value;
        var circles = new List<CircleDefinition>();
        var spinners = new List<SpinnerDefinition>();
        var sliderRows = new List<(CircleDefinition Head, Vector2[] Controls, bool Bezier, double Length, int Traversals, SliderSoundMetadata Samples)>();
        var timing = new List<TimingPoint>();
        var healthRows = new List<HealthObjectSource>();
        var healthBreaks = new List<HealthBreak>();
        var circleSamples = new List<CircleSampleMetadata>();
        var sampleOperands = new List<SamplePreparationOperand>();
        var sampleTiming = new List<SampleTiming>();
        var spinnerSamples = new List<SpinnerSampleMetadata>();
        string? sampleTimingError = null;
        int timingSourceOrder = 0;
        foreach (var entry in extendedPaths ? document.ReadStage(OsuReadStage.RuntimeObjects, token) : document.Entries)
        {
            token.ThrowIfCancellationRequested();
            int lineNumber = entry.LineNumber - 1;
            string line = entry.Text, section = entry.Section;
            if (section == "[HitObjects]")
            {
                string[] row = line.Split(',');
                if (row.Length < 5 || !int.TryParse(row[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int time) ||
                    !int.TryParse(row[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out int type))
                    throw new InvalidDataException($"Line {lineNumber + 1}: use supported circles, sliders or format-v14 spinners.");
                OriginalRegisteredType registered = extendedPaths ? OriginalObjectClassification.Runtime(type) : (type & ~(4 | 112)) switch
                { 1 => OriginalRegisteredType.Circle, 2 => OriginalRegisteredType.Slider, 8 => OriginalRegisteredType.SpinnerBase, _ => OriginalRegisteredType.Unknown };
                if (registered == OriginalRegisteredType.Unknown) throw new InvalidDataException("Unsupported object factory; hold/mode preparation remains unavailable.");
                bool isSlider = registered == OriginalRegisteredType.Slider, isSpinner = registered == OriginalRegisteredType.SpinnerBase;
                healthRows.Add(new(time, type) { RegisteredType = extendedPaths ? registered : null });
                // QA01-D02: validate syntax only; no invented sound-bit restrictions.
                if (!int.TryParse(row[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out int hitSound))
                    throw new InvalidDataException($"Line {lineNumber + 1}: hitSound (column 5) must be an integer, not empty or malformed.");
                bool isBezier = isSlider && row.Length >= 6 && row[5].StartsWith("B|", StringComparison.Ordinal);
                float x = isBezier || extendedPaths ? (float)(int)Math.Clamp(DoubleNumber(row[0], "head X"), 0, 512) : Number(row[0], "circle X");
                float y = isBezier || extendedPaths ? (float)(int)Math.Clamp(DoubleNumber(row[1], "head Y"), 0, 512) : Number(row[1], "circle Y");
                if (time < 0 || time > 3_600_000)
                    throw new InvalidDataException("R02 requires object times between 0 and 3600000 ms.");
                // E04 parser bounds both axes to 512, then truncates. No mods/offsets.
                var head = new CircleDefinition(new((int)Math.Clamp(x, 0, 512), (int)Math.Clamp(y, 0, 512)), time);
                int sampleColumn = isSpinner ? 6 : isSlider ? 10 : 5;
                var sample = CircleSampleMetadata.Parse(head, hitSound, row.Length > sampleColumn ? row[sampleColumn] : null);
                sampleOperands.Add(new(!isSlider && !isSpinner ? head : null, sample.Index, sample.Filename, sample.Error));
                if (isSpinner)
                {
                    if (version != 14) throw new InvalidDataException("R07 supports only modern-policy, format-v14 spinners.");
                    if (row.Length < 6 || !int.TryParse(row[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out int end) ||
                        end <= time || end > 3_600_000 || end - time > 60_000)
                        throw new InvalidDataException($"Line {lineNumber + 1}: spinner end must follow start, duration <=60000 ms, end <=3600000 ms.");
                    var spinner = new SpinnerDefinition(time, end, hitSound, row.Length > 6 ? row[6] : null);
                    spinners.Add(spinner);
                    var payload = sample with { Object = new CircleDefinition(new(256, 192), end), StoredMask = hitSound };
                    spinnerSamples.Add(new(spinner, healthRows.Count - 1, payload));
                    sampleOperands[^1] = sampleOperands[^1] with { Spinner = spinner };
                }
                else if (!isSlider)
                {
                    circles.Add(head);
                    circleSamples.Add(sample);
                }
                else
                {
                    if (row.Length < 8) throw new InvalidDataException($"Line {lineNumber + 1}: slider needs path, span count and pixel length.");
                    string[] pathPoints = row[5].Split('|');
                    if (isBezier && version != 14 && !extendedPaths)
                        throw new InvalidDataException("Historical R06 API supports only format-v14 Bezier; use LoadCanonical for C001 versioned paths.");
                    if ((!isBezier && pathPoints[0] != "L") || pathPoints.Length < 2)
                        throw new InvalidDataException("Use L| or B| control points. Other curve types are not implemented.");
                    if (pathPoints.Length - 1 > SliderGeometryPreparation.MaxControls)
                        throw new InvalidDataException("R06 allows at most 64 normalized controls per slider.");
                    var controls = new List<Vector2>();
                    foreach (string point in pathPoints.Skip(1))
                    {
                        string[] xy = point.Split(':');
                        if (xy.Length != 2) throw new InvalidDataException("Invalid slider control; expected x:y.");
                        // E006/E032: both path families parse Double -> Int32 -> Single.
                        controls.Add(new(ControlCoordinate(xy[0]), ControlCoordinate(xy[1])));
                    }
                    // Constructor normalization is exact and preserves duplicate controls.
                    if (controls.Count == 0 || controls[0] != head.Position) controls.Insert(0, head.Position);
                    if (!int.TryParse(row[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out int spans) || spans < 1 || spans > 16)
                        throw new InvalidDataException("R05 supports 1–16 slider traversals (span count includes the first traversal).");
                    double length = DoubleNumber(row[7], "slider pixel length");
                    if (length <= 0.0001 || length > 4096)
                        throw new InvalidDataException("R05 slider length must be greater than 0.0001 and at most 4096 pixels.");
                    var sliderSamples = SliderSoundMetadata.Parse(hitSound, spans, version, healthRows.Count - 1,
                        row.Length > 8 ? row[8] : null, row.Length > 9 ? row[9] : null, row.Length > 10 ? row[10] : null);
                    sliderRows.Add((head, controls.ToArray(), isBezier, length, spans, sliderSamples));
                }
                if (circles.Count + sliderRows.Count + spinners.Count > 10000) throw new InvalidDataException("R07 accepts at most 10000 objects.");
            }
            else if (section == "[Events]" && line[0] == '2')
            {
                string[] row = line.Split(',');
                if (row.Length < 3 || !int.TryParse(row[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int a) ||
                    !int.TryParse(row[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int b))
                    throw new InvalidDataException("Numeric event2 needs Int32 start/end fields.");
                healthBreaks.Add(new(a, b));
            }
            else if (section == "[TimingPoints]")
            {
                string[] row = line.Split(',');
                if (row.Length < 2) throw new InvalidDataException("Timing point needs time and beat length.");
                double t = DoubleNumber(row[0], "timing time"), beat = DoubleNumber(row[1], "beat length");
                if (t < -3_600_000 || t > 3_600_000) throw new InvalidDataException("Timing time is outside R04's supported range.");
                bool isBase = row.Length < 7 || row[6].Trim() == "1";
                if (row.Length >= 7 && row[6].Trim() is not ("0" or "1")) throw new InvalidDataException("Timing uninherited field must be 0 or 1.");
                if (isBase && beat <= 0) throw new InvalidDataException("R04 requires positive base beat lengths.");
                // E05 M0b75 normalization for an inherited nonnegative value.
                timing.Add(new(t, !isBase && beat >= 0 ? -100 : beat, isBase));
                // E037 long timing rows: explicit volume avoids guessing the caller's
                // default or M3b3c's old/global sample-set dependency. Slider selection is untouched.
                try
                {
                    if (row.Length < 6) throw new FormatException("Short timing sample defaults need unreconstructed global operands; supply six or more fields.");
                    int set = Convert.ToInt32(row[3], CultureInfo.InvariantCulture);
                    int index = Convert.ToInt32(row[4], CultureInfo.InvariantCulture);
                    int volume = Math.Clamp(Convert.ToInt32(row[5], CultureInfo.InvariantCulture), 0, 100);
                    sampleTiming.Add(new(t, set == 0 ? 2 : set, index, volume, isBase, timingSourceOrder));
                }
                catch (Exception e) when (e is FormatException or OverflowException)
                { sampleTimingError ??= "Hitsounds unavailable: " + e.Message; }
                timingSourceOrder++;
            }
            else if (!extendedPaths && section is "[General]" or "[Metadata]" or "[Difficulty]")
            {
                int colon = line.IndexOf(':');
                if (colon >= 0) fields[section + line[..colon].Trim()] = line[(colon + 1)..].Trim();
            }
        }
        if (extendedPaths ? settings!.Mode != 0 : !fields.TryGetValue("[General]Mode", out string? mode) || mode != "0")
            throw new InvalidDataException("R02 requires explicit Mode: 0 (osu!standard).");
        if (circles.Count + sliderRows.Count + spinners.Count == 0) throw new InvalidDataException("No playable objects found.");
        circles.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
        var starts = circles.Select(c => c.StartTime).Concat(sliderRows.Select(s => s.Head.StartTime))
            .Concat(spinners.Select(s => s.StartTime)).Order().ToArray();
        for (int i = 1; i < starts.Length; i++)
            if (starts[i] == starts[i - 1])
                throw new InvalidDataException("R02 does not yet support equal-time objects/stacking.");
        var sliders = new List<SliderDefinition>();
        if (sliderRows.Count > 0)
        {
            double multiplier = SliderSetting("SliderMultiplier", 0.4, 3.6);
            double tickRate = SliderSetting("SliderTickRate", 0.5, 8);
            int totalCheckpoints = 0, totalSegments = 0, totalMovements = 0;
            foreach (var slider in sliderRows.OrderBy(s => s.Head.StartTime))
            {
                token.ThrowIfCancellationRequested();
                var prepared = PrepareSlider(slider.Head, slider.Controls, slider.Bezier, slider.Length, slider.Traversals, version, multiplier, tickRate, timing);
                prepared = prepared with { Samples = slider.Samples.Prepare(prepared) };
                int sourceIndex = slider.Samples.SourceOrder;
                sampleOperands[sourceIndex] = new(null, slider.Samples.Index, slider.Samples.Filename, slider.Samples.Error) { Slider = prepared };
                totalCheckpoints += prepared.Checkpoints.Count;
                if (totalCheckpoints > 100_000) throw new InvalidDataException("R05 accepts at most 100000 scheduled checkpoints per map.");
                totalSegments += prepared.Geometry.Segments.Count; totalMovements += prepared.Movements.Count;
                if (totalSegments > 100_000 || totalMovements > 250_000)
                    throw new InvalidDataException("R06 map geometry exceeds 100000 prepared segments / 250000 Movement records.");
                sliders.Add(prepared);
            }
            for (int i = 1; i < sliders.Count; i++)
                if (sliders[i].Head.StartTime <= sliders[i - 1].EndTime)
                    throw new InvalidDataException("R05 does not support overlapping slider intervals.");
        }
        // Bounded R07 admission: spinner stored intervals cannot intersect any
        // other object's stored interval (circles are point intervals).
        int maximumEnd = int.MinValue, spinnerEnd = int.MinValue;
        var intervals = circles.Select(c => (Start: c.StartTime, End: c.StartTime, Spinner: false))
            .Concat(sliders.Select(s => (Start: s.Head.StartTime, End: s.EndTime, Spinner: false)))
            .Concat(spinners.Select(s => (Start: s.StartTime, End: s.EndTime, Spinner: true))).OrderBy(v => v.Start);
        foreach (var interval in intervals)
        {
            if ((interval.Spinner && interval.Start <= maximumEnd) || interval.Start <= spinnerEnd)
                throw new InvalidDataException("R07 spinner intervals must be separate from all other objects, including shared endpoints.");
            maximumEnd = Math.Max(maximumEnd, interval.End);
            if (interval.Spinner) spinnerEnd = interval.End;
        }
        string title = fields.GetValueOrDefault("[Metadata]Title", Path.GetFileNameWithoutExtension(path));
        string? audioPath = null;
        if (fields.TryGetValue("[General]AudioFilename", out string? audioName) && !string.IsNullOrWhiteSpace(audioName))
        {
            // Local-only slice policy: no URI/absolute/network references or escape
            // from this map's directory. This is not a stable parser claim.
            audioName = audioName.Trim().Trim('"');
            if (audioName.Length == 0 || Path.IsPathRooted(audioName) || audioName.StartsWith('\\') ||
                audioName.StartsWith('/') || audioName.Contains(':'))
                throw new InvalidDataException("AudioFilename must be a local relative file within the beatmap folder.");
            string directory = Path.GetDirectoryName(Path.GetFullPath(path))!;
            audioPath = Path.GetFullPath(Path.Combine(directory, audioName.Replace('\\', Path.DirectorySeparatorChar)));
            string prefix = Path.TrimEndingDirectorySeparator(directory) + Path.DirectorySeparatorChar;
            if (!audioPath.StartsWith(prefix, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                throw new InvalidDataException("AudioFilename must stay within the beatmap folder.");
        }
        var map = new Beatmap(title, Difficulty("CircleSize"), Difficulty("OverallDifficulty"),
            Difficulty("ApproachRate"), circles.AsReadOnly()) { Document = document, MetadataSettings = settings, AudioPath = audioPath, Sliders = sliders.AsReadOnly(),
                Spinners = spinners.OrderBy(s => s.StartTime).ToArray(), DisplayMetadata = new(fields.GetValueOrDefault("[Metadata]Version"), fields.GetValueOrDefault("[Metadata]Artist")), ScoreMetadata = LegacyScoreMetadata.ReadFresh(document, extendedPaths),
                HealthSource = new(version, healthRows.AsReadOnly(), healthBreaks.AsReadOnly()) { OriginalFactoryPriority = extendedPaths },
                CircleSounds = new(Path.GetDirectoryName(Path.GetFullPath(path))!, version,
                    circleSamples.AsReadOnly(), sampleTiming.AsReadOnly(), sampleTimingError)
                    { RegisteredOverrides = sampleOperands.AsReadOnly(), Spinners = spinnerSamples.AsReadOnly() } };
        map.HealthSource!.BindFreshOwner(map);
        return map;

        double SliderSetting(string name, double lower, double upper)
        {
            if (settings is not null) return name == "SliderMultiplier" ? settings.SliderMultiplier : settings.SliderTickRate;
            if (!fields.TryGetValue("[Difficulty]" + name, out string? raw))
                throw new InvalidDataException($"Slider maps require explicit {name}.");
            return Math.Clamp(DoubleNumber(raw, name), lower, upper); // E05 parser bounds.
        }

        float Difficulty(string name)
        {
            if (settings is not null) return name switch { "CircleSize" => settings.Cs, "OverallDifficulty" => settings.Od, "ApproachRate" => settings.Ar, _ => throw new InvalidDataException("Unknown prepared difficulty setting.") };
            if (!fields.TryGetValue("[Difficulty]" + name, out string? raw))
                throw new InvalidDataException($"R02 requires explicit {name}.");
            float value = Number(raw, name);
            if (value < 0 || value > 10) throw new InvalidDataException($"{name} must be within 0–10.");
            return value;
        }
    }

    private static float Number(string text, string name)
    {
        if (!float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float value) || !float.IsFinite(value))
            throw new InvalidDataException($"Invalid finite number for {name}.");
        return value;
    }

    private static double DoubleNumber(string text, string name)
    {
        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value) || !double.IsFinite(value))
            throw new InvalidDataException($"Invalid finite number for {name}.");
        return value;
    }

    private static float ControlCoordinate(string text)
    {
        double value = DoubleNumber(text, "Bézier control coordinate");
        if (value < int.MinValue || value > int.MaxValue)
            throw new InvalidDataException("R06 Bézier controls must fit finite Int32 coordinates.");
        return (float)(int)value; // E32: Double -> Int32 (truncate) -> Single, no control clamp.
    }

    private static SliderDefinition PrepareSlider(CircleDefinition head, Vector2[] controls, bool bezier, double length,
        int traversals, int version, double multiplier, double tickRate, List<TimingPoint> timing)
    {
        // E05: list order is significant; rate uses truncated time, spacing raw time.
        double effectiveBeat = 0, spacingFactor = 1;
        if (timing.Count > 0)
        {
            int baseIndex = 0, inheritedIndex = 0, rawIndex = 0;
            for (int i = 0; i < timing.Count; i++)
            {
                if ((int)timing[i].Time <= head.StartTime)
                {
                    if (timing[i].IsBase) baseIndex = i; else inheritedIndex = i;
                }
                if (timing[i].Time <= head.StartTime) rawIndex = i;
            }
            effectiveBeat = timing[baseIndex].BeatLength *
                (inheritedIndex > baseIndex ? InverseSv(timing[inheritedIndex].BeatLength) : 1);
            spacingFactor = InverseSv(timing[rawIndex].BeatLength);
        }
        double spacing = (100.0 * multiplier) / tickRate;
        double velocity = effectiveBeat > 0 ? (spacing * tickRate) * (1000.0 / effectiveBeat) : spacing * tickRate;
        SliderGeometry geometry = SliderGeometryPreparation.Prepare(head.Position, controls, bezier, length, version);
        if (!double.IsFinite(velocity) || velocity <= 0)
            throw new InvalidDataException("Unsupported slider geometry or travel rate.");

        double q = Math.Min(version < 8 ? spacing : spacing / spacingFactor, length);
        if (!double.IsFinite(q) || q <= 0) throw new InvalidDataException("Unsupported slider checkpoint spacing.");
        // E16/E21: movement time and checkpoint distance phase are separate clocks.
        var moves = new List<SliderMovement>();
        var components = new List<SliderCheckpoint>();
        double absoluteTime = head.StartTime, phase = 0, carry = 0;
        float[] distances = geometry.CumulativeLengths.Select((v, i) => (float)(v - (i == 0 ? 0 : geometry.CumulativeLengths[i - 1]))).ToArray();
        double duration = distances.Sum(d => (double)(1000f * d) / velocity);
        if (!double.IsFinite(duration) || duration < 1 || absoluteTime + duration * traversals > 3_600_000)
            throw new InvalidDataException("R05 requires each traversal >= 1 ms and parent end <= 3600000 ms.");
        for (int traversal = 0; traversal < traversals; traversal++)
        {
            bool reverse = traversal % 2 == 1;
            double remaining = geometry.UnadjustedLength; // NOT requested/prepared L.
            bool suppressed = false;
            for (int step = 0; step < geometry.Segments.Count; step++)
            {
                int index = reverse ? geometry.Segments.Count - 1 - step : step;
                SliderSegment segment = geometry.Segments[index];
                double dt = (double)(1000f * distances[index]) / velocity;
                moves.Add(new(reverse ? segment.To : segment.From, reverse ? segment.From : segment.To,
                    (int)absoluteTime, (int)(absoluteTime + dt)));
                absoluteTime += dt;
                carry += distances[index];
                while (carry >= q && !suppressed)
                {
                    phase += q; carry -= q; remaining -= q;
                    suppressed = remaining <= 0.01 * velocity;
                    if (suppressed) break;
                    AddCheckpoint(Stamp(phase), SliderCheckpointKind.Interior, traversal);
                }
            }
            phase += carry;
            AddCheckpoint(Stamp(phase), traversal == traversals - 1 ? SliderCheckpointKind.Final : SliderCheckpointKind.Repeat, traversal);
            if (suppressed) carry = 0;
            else { phase -= q - carry; carry = q - carry; }
        }
        int end = (int)absoluteTime;
        // E22: adjust ONLY the final stored item. Do not sort or repair the list.
        components[^1] = components[^1] with { Time = Math.Max(head.StartTime + (end - head.StartTime) / 2, components[^1].Time - 36) };
        var definition = new SliderDefinition(head, geometry.Segments[^1].To, end, velocity, length,
            geometry.Controls, traversals, moves.AsReadOnly(), components.AsReadOnly()) { Geometry = geometry };
        for (int i = 0; i < components.Count; i++)
        {
            var c = components[i];
            // Independent display markers; these positions do not select results.
            components[i] = c with { Position = c.Kind == SliderCheckpointKind.Interior ?
                definition.RequiredPosition(c.Time) : definition.TraversalEndMovement(c.Traversal).To };
        }
        return definition;

        int Stamp(double distance)
        {
            double t = head.StartTime + ((double)(float)distance / velocity) * 1000.0;
            if (!double.IsFinite(t) || t < 0 || t > int.MaxValue)
                throw new InvalidDataException("Checkpoint time is outside R05's finite supported range.");
            return (int)t;
        }
        void AddCheckpoint(int time, SliderCheckpointKind kind, int traversal)
        {
            if (components.Count >= 4096) throw new InvalidDataException("R05 accepts at most 4096 checkpoints per slider.");
            components.Add(new(time, kind, traversal, default));
        }
    }

    private static double InverseSv(double beatLength) => beatLength < 0 ? Math.Clamp((float)-beatLength, 10f, 1000f) / 100f : 1;
}
