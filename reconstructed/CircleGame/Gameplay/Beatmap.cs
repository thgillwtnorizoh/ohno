using System.Globalization;
using System.IO;
using System.Numerics;

namespace Ohno.CircleGame.Gameplay;

public sealed record CircleDefinition(Vector2 Position, int StartTime);

public sealed record SliderSegment(Vector2 From, Vector2 To, bool IsSubcurveEnd, bool IsLinear);

public sealed record SliderGeometry(IReadOnlyList<Vector2> Controls, IReadOnlyList<SliderSegment> RawSegments,
    IReadOnlyList<SliderSegment> Segments, IReadOnlyList<double> CumulativeLengths,
    double UnadjustedLength, double RequestedLength);

// E32 finite v14 M144a -> M2d62 -> M2d93. Kept with the shared model so the
// existing QA projects continue linking the actual implementation unchanged.
public static class SliderGeometryPreparation
{
    public const int MaxControls = 64, MaxSegments = 8192, MaxSubdivisionWork = 50000;

    public static SliderGeometry Prepare(Vector2 head, IReadOnlyList<Vector2> supplied, bool bezier, double requested)
    {
        var controls = supplied.ToList();
        if (controls.Count == 0 || controls[0] != head) controls.Insert(0, head);
        if (controls.Count > MaxControls) throw new InvalidDataException("R06 allows at most 64 normalized controls per slider.");
        var raw = new List<SliderSegment>();
        int work = 0;
        if (!bezier) Emit(controls, true);
        else
        {
            // Modern forward-adjacent equality; the final equal pair is NOT a separator.
            int start = 0;
            for (int i = 0; i < controls.Count; i++)
            {
                bool separator = i < controls.Count - 2 && controls[i] == controls[i + 1];
                if (!separator && i != controls.Count - 1) continue;
                var range = controls.GetRange(start, i - start + 1);
                Emit(range.Count == 2 ? range : Flatten(range, ref work), range.Count == 2);
                start = ++i; // Second duplicate starts the next range; no global deduplication.
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
                raw.Add(linear ? new(a + (b - a) * 0f, a + (b - a) * 1f, j == points.Count - 1, true) :
                    new(a, b, j == points.Count - 1, false));
            }
        }
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

public sealed record Beatmap(string Title, float CircleSize, float OverallDifficulty,
    float ApproachRate, IReadOnlyList<CircleDefinition> Circles)
{
    // Optional local media reference; the gameplay model remains independent of WPF.
    public string? AudioPath { get; init; }
    public IReadOnlyList<SliderDefinition> Sliders { get; init; } = Array.Empty<SliderDefinition>();

    // Deliberately narrow .osu reader, NOT stable's complete parser (E04/E05).
    public static Beatmap Load(string path)
    {
        if (new FileInfo(path).Length > 2 * 1024 * 1024)
            throw new InvalidDataException("R02 accepts beatmaps up to 2 MB.");
        string[] lines = File.ReadAllLines(path);
        string? header = lines.FirstOrDefault(l => !string.IsNullOrWhiteSpace(l))?.Trim('\uFEFF', ' ', '\t');
        if (header is null || !header.StartsWith("osu file format v", StringComparison.Ordinal) ||
            !int.TryParse(header[17..], out int version) || version < 5 || version > 14)
            throw new InvalidDataException("Use an osu file format v5–v14 supported circle/Linear-slider map.");

        var fields = new Dictionary<string, string>(StringComparer.Ordinal);
        var circles = new List<CircleDefinition>();
        var sliderRows = new List<(CircleDefinition Head, Vector2[] Controls, bool Bezier, double Length, int Traversals)>();
        var timing = new List<TimingPoint>();
        string section = "";
        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            string line = lines[lineNumber].Trim();
            if (line.Length == 0 || line.StartsWith("//")) continue;
            if (line.StartsWith('[') && line.EndsWith(']')) { section = line; continue; }
            if (section == "[HitObjects]")
            {
                string[] row = line.Split(',');
                if (row.Length < 5 || !int.TryParse(row[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int time) ||
                    !int.TryParse(row[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out int type) ||
                    (type & ~(4 | 112)) is not (1 or 2))
                    throw new InvalidDataException($"Line {lineNumber + 1}: only ordinary circles and supported Linear/v14 Bézier sliders are accepted.");
                // QA01-D02: validate syntax only; no invented sound-bit restrictions.
                if (!int.TryParse(row[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                    throw new InvalidDataException($"Line {lineNumber + 1}: hitSound (column 5) must be an integer, not empty or malformed.");
                bool isBezier = (type & 2) != 0 && row.Length >= 6 && row[5].StartsWith("B|", StringComparison.Ordinal);
                float x = isBezier ? (float)(int)Math.Clamp(DoubleNumber(row[0], "head X"), 0, 512) : Number(row[0], "circle X");
                float y = isBezier ? (float)(int)Math.Clamp(DoubleNumber(row[1], "head Y"), 0, 512) : Number(row[1], "circle Y");
                if (time < 0 || time > 3_600_000)
                    throw new InvalidDataException("R02 requires object times between 0 and 3600000 ms.");
                // E04 parser bounds both axes to 512, then truncates. No mods/offsets.
                var head = new CircleDefinition(new((int)Math.Clamp(x, 0, 512), (int)Math.Clamp(y, 0, 512)), time);
                if ((type & 2) == 0) circles.Add(head);
                else
                {
                    if (row.Length < 8) throw new InvalidDataException($"Line {lineNumber + 1}: slider needs path, span count and pixel length.");
                    string[] pathPoints = row[5].Split('|');
                    if (isBezier && version != 14) throw new InvalidDataException("R06 Bézier preparation supports only format v14.");
                    if ((!isBezier && (pathPoints.Length != 2 || pathPoints[0] != "L")) || pathPoints.Length < 2)
                        throw new InvalidDataException("Use L|x:y (one straight segment) or format-v14 B| controls. Other curve types are not implemented.");
                    if (pathPoints.Length - 1 > SliderGeometryPreparation.MaxControls)
                        throw new InvalidDataException("R06 allows at most 64 normalized controls per slider.");
                    var controls = new List<Vector2>();
                    foreach (string point in pathPoints.Skip(1))
                    {
                        string[] xy = point.Split(':');
                        if (xy.Length != 2) throw new InvalidDataException("Invalid slider control; expected x:y.");
                        controls.Add(isBezier ? new(ControlCoordinate(xy[0]), ControlCoordinate(xy[1])) :
                            new(Number(xy[0], "slider endpoint X"), Number(xy[1], "slider endpoint Y")));
                    }
                    // Constructor normalization is exact and preserves duplicate controls.
                    if (controls.Count == 0 || controls[0] != head.Position) controls.Insert(0, head.Position);
                    if (!int.TryParse(row[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out int spans) || spans < 1 || spans > 16)
                        throw new InvalidDataException("R05 supports 1–16 slider traversals (span count includes the first traversal).");
                    double length = DoubleNumber(row[7], "slider pixel length");
                    if (length <= 0.0001 || length > 4096)
                        throw new InvalidDataException("R05 slider length must be greater than 0.0001 and at most 4096 pixels.");
                    sliderRows.Add((head, controls.ToArray(), isBezier, length, spans));
                }
                if (circles.Count + sliderRows.Count > 10000) throw new InvalidDataException("R04 accepts at most 10000 objects.");
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
            }
            else if (section is "[General]" or "[Metadata]" or "[Difficulty]")
            {
                int colon = line.IndexOf(':');
                if (colon >= 0) fields[section + line[..colon].Trim()] = line[(colon + 1)..].Trim();
            }
        }
        if (!fields.TryGetValue("[General]Mode", out string? mode) || mode != "0")
            throw new InvalidDataException("R02 requires explicit Mode: 0 (osu!standard).");
        if (circles.Count + sliderRows.Count == 0) throw new InvalidDataException("No playable objects found.");
        circles.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
        var starts = circles.Select(c => c.StartTime).Concat(sliderRows.Select(s => s.Head.StartTime)).Order().ToArray();
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
                var prepared = PrepareSlider(slider.Head, slider.Controls, slider.Bezier, slider.Length, slider.Traversals, version, multiplier, tickRate, timing);
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
        return new(title, Difficulty("CircleSize"), Difficulty("OverallDifficulty"),
            Difficulty("ApproachRate"), circles.AsReadOnly()) { AudioPath = audioPath, Sliders = sliders.AsReadOnly() };

        double SliderSetting(string name, double lower, double upper)
        {
            if (!fields.TryGetValue("[Difficulty]" + name, out string? raw))
                throw new InvalidDataException($"Slider maps require explicit {name}.");
            return Math.Clamp(DoubleNumber(raw, name), lower, upper); // E05 parser bounds.
        }

        float Difficulty(string name)
        {
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
        SliderGeometry geometry = SliderGeometryPreparation.Prepare(head.Position, controls, bezier, length);
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
