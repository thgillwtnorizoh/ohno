using System.Globalization;
using System.IO;
using System.Numerics;

namespace Ohno.CircleGame.Gameplay;

public sealed record CircleDefinition(Vector2 Position, int StartTime);

public sealed record SliderDefinition(CircleDefinition Head, Vector2 Tail,
    int EndTime, int TerminalTime, double TravelRate, double PixelLength,
    IReadOnlyList<Vector2> ControlPoints)
{
    // E15's one Movement record; its late fallback extrapolates, not clamps.
    public Vector2 RequiredPosition(int time) => Head.Position + (Tail - Head.Position) *
        (1f - (float)(EndTime - time) / (EndTime - Head.StartTime));
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
            throw new InvalidDataException("Use an osu file format v5–v14 ordinary-circle map.");

        var fields = new Dictionary<string, string>(StringComparer.Ordinal);
        var circles = new List<CircleDefinition>();
        var sliderRows = new List<(CircleDefinition Head, Vector2 Tail, double Length)>();
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
                        throw new InvalidDataException("R04 supports only L|x:y (one straight segment); curves and multi-point paths are not implemented.");
                    string[] endpoint = pathPoints[1].Split(':');
                    if (endpoint.Length != 2) throw new InvalidDataException("Invalid slider endpoint; expected x:y.");
                    if (!int.TryParse(row[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out int spans) || spans != 1)
                        throw new InvalidDataException("R04 supports exactly one slider span; repeats are not implemented.");
                    double length = DoubleNumber(row[7], "slider pixel length");
                    if (length <= 0.0001 || length > 4096)
                        throw new InvalidDataException("R04 slider length must be greater than 0.0001 and at most 4096 pixels.");
                    sliderRows.Add((head, new(Number(endpoint[0], "slider endpoint X"), Number(endpoint[1], "slider endpoint Y")), length));
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
            foreach (var slider in sliderRows.OrderBy(s => s.Head.StartTime))
                sliders.Add(PrepareSlider(slider.Head, slider.Tail, slider.Length, version, multiplier, tickRate, timing));
            for (int i = 1; i < sliders.Count; i++)
                if (sliders[i].Head.StartTime <= sliders[i - 1].EndTime)
                    throw new InvalidDataException("R04 does not support overlapping slider intervals.");
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
        int version, double multiplier, double tickRate, List<TimingPoint> timing)
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
            throw new InvalidDataException("R04 requires a nondegenerate finite slider segment.");
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
        // E16: detect an interior Add, rather than silently dropping ticks. For one
        // segment, if the first spacing candidate survives suppression, reject it.
        if (preparedLength >= q && geometricLength - q > 0.01 * velocity)
            throw new InvalidDataException("This slider generates interior tick checkpoints; R04 supports head + terminal only.");
        double absoluteEnd = head.StartTime + (double)(1000f * preparedLength) / velocity;
        if (!double.IsFinite(absoluteEnd) || absoluteEnd > 3_600_000 || absoluteEnd < head.StartTime + 1)
            throw new InvalidDataException("R04 requires slider duration >= 1 ms and end <= 3600000 ms.");
        int end = (int)absoluteEnd;
        double phase = 0, carry = preparedLength;
        if (carry >= q) { phase += q; carry -= q; } // Surviving interior Add was rejected above.
        phase += carry;
        int originalTerminal = (int)(head.StartTime + ((double)(float)phase / velocity) * 1000.0);
        int terminal = Math.Max(head.StartTime + (end - head.StartTime) / 2, originalTerminal - 36);
        return new(head, tail, end, terminal, velocity, length, Array.AsReadOnly(new[] { head.Position, suppliedTail }));
    }

    private static double InverseSv(double beatLength) => beatLength < 0 ? Math.Clamp((float)-beatLength, 10f, 1000f) / 100f : 1;
}
