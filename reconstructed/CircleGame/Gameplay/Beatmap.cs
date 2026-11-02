using System.Globalization;
using System.IO;
using System.Numerics;

namespace Ohno.CircleGame.Gameplay;

public sealed record CircleDefinition(Vector2 Position, int StartTime);

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
    public int TerminalTime => Checkpoints[^1].Time;
    public Vector2 EndPosition => Movements[^1].To;

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
        var sliderRows = new List<(CircleDefinition Head, Vector2 Tail, double Length, int Traversals)>();
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
                    throw new InvalidDataException($"Line {lineNumber + 1}: only ordinary circles and supported Linear sliders are accepted.");
                // QA01-D02: validate syntax only; no invented sound-bit restrictions.
                if (!int.TryParse(row[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                    throw new InvalidDataException($"Line {lineNumber + 1}: hitSound (column 5) must be an integer, not empty or malformed.");
                float x = Number(row[0], "circle X"), y = Number(row[1], "circle Y");
                if (time < 0 || time > 3_600_000)
                    throw new InvalidDataException("R02 requires object times between 0 and 3600000 ms.");
                // E04 parser bounds both axes to 512, then truncates. No mods/offsets.
                var head = new CircleDefinition(new((int)Math.Clamp(x, 0, 512), (int)Math.Clamp(y, 0, 512)), time);
                if ((type & 2) == 0) circles.Add(head);
                else
                {
                    if (row.Length < 8) throw new InvalidDataException($"Line {lineNumber + 1}: slider needs path, span count and pixel length.");
                    string[] pathPoints = row[5].Split('|');
                    if (pathPoints.Length != 2 || pathPoints[0] != "L")
                        throw new InvalidDataException("R05 supports only L|x:y (one straight segment); curves and multi-point paths are not implemented.");
                    string[] endpoint = pathPoints[1].Split(':');
                    if (endpoint.Length != 2) throw new InvalidDataException("Invalid slider endpoint; expected x:y.");
                    if (!int.TryParse(row[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out int spans) || spans < 1 || spans > 16)
                        throw new InvalidDataException("R05 supports 1–16 slider traversals (span count includes the first traversal).");
                    double length = DoubleNumber(row[7], "slider pixel length");
                    if (length <= 0.0001 || length > 4096)
                        throw new InvalidDataException("R05 slider length must be greater than 0.0001 and at most 4096 pixels.");
                    sliderRows.Add((head, new(Number(endpoint[0], "slider endpoint X"), Number(endpoint[1], "slider endpoint Y")), length, spans));
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
            int totalCheckpoints = 0;
            foreach (var slider in sliderRows.OrderBy(s => s.Head.StartTime))
            {
                var prepared = PrepareSlider(slider.Head, slider.Tail, slider.Length, slider.Traversals, version, multiplier, tickRate, timing);
                totalCheckpoints += prepared.Checkpoints.Count;
                if (totalCheckpoints > 100_000) throw new InvalidDataException("R05 accepts at most 100000 scheduled checkpoints per map.");
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

    private static SliderDefinition PrepareSlider(CircleDefinition head, Vector2 suppliedTail, double length,
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
        float geometricLength = Vector2.Distance(head.Position, suppliedTail);
        if (!float.IsFinite(geometricLength) || geometricLength <= 0)
            throw new InvalidDataException("R05 requires a nondegenerate finite slider segment.");
        // E06's one-segment trailing trim/extension. Unsupported degeneracy is explicit.
        float excess = (float)(geometricLength - length);
        if (geometricLength <= geometricLength - length + 0.0001)
            throw new InvalidDataException("Slider geometry vanished during length preparation.");
        Vector2 tail = head.Position + Vector2.Normalize(suppliedTail - head.Position) * (geometricLength - excess);
        float preparedLength = Vector2.Distance(head.Position, tail);
        if (!float.IsFinite(preparedLength) || preparedLength <= 0 || !double.IsFinite(velocity) || velocity <= 0)
            throw new InvalidDataException("Unsupported slider geometry or travel rate.");

        double q = Math.Min(version < 8 ? spacing : spacing / spacingFactor, length);
        if (!double.IsFinite(q) || q <= 0) throw new InvalidDataException("Unsupported slider checkpoint spacing.");
        // E16/E21: movement time and checkpoint distance phase are separate clocks.
        var moves = new List<SliderMovement>();
        var components = new List<SliderCheckpoint>();
        double absoluteTime = head.StartTime, phase = 0, carry = 0;
        double dt = (double)(1000f * preparedLength) / velocity;
        if (!double.IsFinite(dt) || dt < 1 || absoluteTime + dt * traversals > 3_600_000)
            throw new InvalidDataException("R05 requires each traversal >= 1 ms and parent end <= 3600000 ms.");
        for (int traversal = 0; traversal < traversals; traversal++)
        {
            bool reverse = traversal % 2 == 1;
            var move = new SliderMovement(reverse ? tail : head.Position, reverse ? head.Position : tail,
                (int)absoluteTime, (int)(absoluteTime + dt));
            moves.Add(move);
            absoluteTime += dt;
            double remaining = geometricLength; // Unadjusted sum, NOT requested/prepared L.
            bool suppressed = false;
            carry += preparedLength;
            while (carry >= q && !suppressed)
            {
                phase += q; carry -= q; remaining -= q;
                suppressed = remaining <= 0.01 * velocity;
                if (suppressed) break;
                AddCheckpoint(Stamp(phase), SliderCheckpointKind.Interior, traversal);
            }
            phase += carry;
            AddCheckpoint(Stamp(phase), traversal == traversals - 1 ? SliderCheckpointKind.Final : SliderCheckpointKind.Repeat, traversal);
            if (suppressed) carry = 0;
            else { phase -= q - carry; carry = q - carry; }
        }
        int end = (int)absoluteTime;
        // E22: adjust ONLY the final stored item. Do not sort or repair the list.
        components[^1] = components[^1] with { Time = Math.Max(head.StartTime + (end - head.StartTime) / 2, components[^1].Time - 36) };
        var definition = new SliderDefinition(head, tail, end, velocity, length,
            Array.AsReadOnly(new[] { head.Position, suppliedTail }), traversals, moves.AsReadOnly(), components.AsReadOnly());
        for (int i = 0; i < components.Count; i++)
        {
            var c = components[i];
            // Independent display markers; these positions do not select results.
            components[i] = c with { Position = c.Kind == SliderCheckpointKind.Interior ?
                definition.RequiredPosition(c.Time) : moves[c.Traversal].To };
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
