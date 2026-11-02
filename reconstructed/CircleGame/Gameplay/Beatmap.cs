using System.Globalization;
using System.IO;
using System.Numerics;

namespace Ohno.CircleGame.Gameplay;

public sealed record CircleDefinition(Vector2 Position, int StartTime);

public sealed record Beatmap(string Title, float CircleSize, float OverallDifficulty,
    float ApproachRate, IReadOnlyList<CircleDefinition> Circles)
{
    // Optional local media reference; the gameplay model remains independent of WPF.
    public string? AudioPath { get; init; }

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
                    (type & 1) == 0 || (type & ~(1 | 4 | 112)) != 0)
                    throw new InvalidDataException($"Line {lineNumber + 1}: only ordinary circles are supported; no sliders, spinners or holds.");
                // QA01-D02: validate syntax only; no invented sound-bit restrictions.
                if (!int.TryParse(row[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                    throw new InvalidDataException($"Line {lineNumber + 1}: hitSound (column 5) must be an integer, not empty or malformed.");
                float x = Number(row[0], "circle X"), y = Number(row[1], "circle Y");
                if (time < 0 || time > 3_600_000)
                    throw new InvalidDataException("R02 requires object times between 0 and 3600000 ms.");
                // E04 parser bounds both axes to 512, then truncates. No mods/offsets.
                circles.Add(new(new((int)Math.Clamp(x, 0, 512), (int)Math.Clamp(y, 0, 512)), time));
                if (circles.Count > 10000) throw new InvalidDataException("R02 accepts at most 10000 circles.");
            }
            else if (section is "[General]" or "[Metadata]" or "[Difficulty]")
            {
                int colon = line.IndexOf(':');
                if (colon >= 0) fields[section + line[..colon].Trim()] = line[(colon + 1)..].Trim();
            }
        }
        if (!fields.TryGetValue("[General]Mode", out string? mode) || mode != "0")
            throw new InvalidDataException("R02 requires explicit Mode: 0 (osu!standard).");
        if (circles.Count == 0) throw new InvalidDataException("No circles found.");
        circles.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
        for (int i = 1; i < circles.Count; i++)
            if (circles[i].StartTime == circles[i - 1].StartTime)
                throw new InvalidDataException("R02 does not yet support equal-time objects/stacking.");
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
            Difficulty("ApproachRate"), circles.AsReadOnly()) { AudioPath = audioPath };

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
}
