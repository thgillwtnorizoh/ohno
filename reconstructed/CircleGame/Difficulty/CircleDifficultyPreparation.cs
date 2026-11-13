using System.Globalization;
using System.IO;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Difficulty;

public sealed class UnsupportedStarProfileException(string message) : Exception(message);
public readonly record struct CircleDefinition(int Start, float X, float Y);
public readonly record struct CirclePoint(float X, float Y);
public sealed record CirclePreparation(float Radius, double Preempt, double FullWindow);
public sealed record StarProfile(int Mods = 0, float Width = 512, float Height = 384, int TimeShift = 0, double Rate = 1);
public sealed record AdmittedCircles(IReadOnlyList<CircleDefinition> Circles, CirclePreparation Preparation);

// ENGINEERING strict projection of C001 syntax; independent gameplay admission.
public static class StarAdmission
{
    public const string CalculatorProfile = "E044-E046.circle-v1:CS5:OD5:AR5-or10:512x384:shift0:rate1:mods0";
    public const int MaximumCircles = 4096;
    public static AdmittedCircles Read(CatalogChart chart, CancellationToken token = default, StarProfile? profile = null)
    {
        chart.Verify(token);
        var read = ChartFingerprint.Read(chart.Path, token);
        using var reader = new StreamReader(new MemoryStream(read.Bytes), System.Text.Encoding.UTF8, true);
        var admitted = Parse(reader.ReadToEnd(), profile, token);
        chart.Verify(token);
        return admitted;
    }
    public static AdmittedCircles Parse(string text, StarProfile? profile = null, CancellationToken token = default)
    {
        profile ??= new();
        if (profile.Mods != 0 || profile.Width != 512 || profile.Height != 384 || profile.TimeShift != 0 || profile.Rate != 1)
            throw new UnsupportedStarProfileException("Only mods0,512×384,shift0,rate1 independently verified profile is admitted.");
        OsuDocument document;
        try { document = OsuDocument.Parse(text, token); }
        catch (InvalidDataException e) { throw new UnsupportedStarProfileException(e.Message); }
        if (document.FirstLine != "osu file format v14") throw new UnsupportedStarProfileException("Format14 required.");
        if (document.SourceLines.Count - 1 > 100000) throw new UnsupportedStarProfileException("Restricted reader line limit exceeded.");
        var fields = new Dictionary<string, string>(StringComparer.Ordinal);
        var circles = new List<CircleDefinition>();
        foreach (var entry in document.Entries)
        {
            token.ThrowIfCancellationRequested();
            string line = entry.Text, section = entry.Section;
            int colon = line.IndexOf(':');
            if (section is "[General]" or "[Difficulty]" && colon >= 0)
            {
                string key = section + line[..colon].Trim();
                if (!fields.TryAdd(key, line[(colon + 1)..].Trim())) throw new UnsupportedStarProfileException("Duplicate required/preparation metadata is unsupported.");
                continue;
            }
            if (section != "[HitObjects]") continue;
            string[] cells = line.Split(',');
            if (cells.Length != 5) throw new UnsupportedStarProfileException("Restricted five-column circle rows required; sample overrides/other objects unsupported.");
            int[] values = new int[5];
            for (int i = 0; i < values.Length; i++)
                if (!int.TryParse(cells[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out values[i])) throw new UnsupportedStarProfileException("Integer finite circle row required.");
            int x = values[0], y = values[1], time = values[2], kind = values[3], sound = values[4];
            if (kind is not (1 or 5) || sound != 0) throw new UnsupportedStarProfileException("Only simple ordinary circles with sound0 are in the research reader profile.");
            if (x is < 0 or > 512 || y is < 0 or > 512 || time < 0) throw new UnsupportedStarProfileException("Circle coordinate/time outside E045/E046 admission.");
            if (circles.Count > 0 && time <= circles[^1].Start) throw new UnsupportedStarProfileException("Strictly increasing timestamps required; equal-time registration unsupported.");
            circles.Add(new(time, x, y));
            if (circles.Count > MaximumCircles) throw new UnsupportedStarProfileException("Restricted4096-circle engineering limit exceeded.");
        }
        if (fields.GetValueOrDefault("[General]Mode") != "0") throw new UnsupportedStarProfileException("Explicit ordinary Mode0 required.");
        float Scalar(string key)
        {
            if (!float.TryParse(fields.GetValueOrDefault(key), NumberStyles.Float, CultureInfo.InvariantCulture, out float value) || !float.IsFinite(value))
                throw new UnsupportedStarProfileException("Explicit finite scalar required: " + key);
            return value;
        }
        if (Scalar("[General]StackLeniency") != 0) throw new UnsupportedStarProfileException("Explicit StackLeniency0 required; arbitrary stacking unsupported.");
        float cs = Scalar("[Difficulty]CircleSize"), ar = Scalar("[Difficulty]ApproachRate"), od = Scalar("[Difficulty]OverallDifficulty");
        if (cs != 5 || od != 5 || ar is not (5 or 10)) throw new UnsupportedStarProfileException("Only CS5/OD5 with AR5 or10 independently executed raw profiles admitted.");
        if (circles.Count < 2) throw new UnsupportedStarProfileException("At least two complete circles required by the restricted calculation profile.");
        // With threshold preempt*0 and all gaps>0, M28eb scans exit before stack increments.
        return new(circles.AsReadOnly(), CircleDifficultyPreparation.Operands(cs, ar, od));
    }
}
public sealed class DifficultyPair
{
    public required int Index { get; init; }
    public required double Time { get; init; }
    public required double Delta { get; init; }
    public required double Strain { get; init; }
    public required CirclePoint Position { get; init; }
    public required CirclePoint PreviousPosition { get; init; }
    public required CirclePreparation Preparation { get; init; }
    public double Gap => Strain;
    public required double Jump { get; init; }
    public double? Angle { get; set; }
    public double? Axis { get; set; }
    public required IReadOnlyList<DifficultyPair> History { get; init; }
    public DifficultyPair? Previous(int k = 0) => At(Index - k - 1);
    public DifficultyPair? Following(int k = 0) => At(Index + k + 1);
    private DifficultyPair? At(int i) => i >= 0 && i < History.Count ? History[i] : null;
}
public static class CircleDifficultyPreparation
{
    // Explicit Single stores; component products/sums cannot become a vector SIMD/FMA oracle.
    internal static float S(double x) => (float)x;
    internal static CirclePoint Sub(CirclePoint a, CirclePoint b) => new(S((double)a.X - b.X), S((double)a.Y - b.Y));
    public static float Distance(CirclePoint a, CirclePoint b)
    {
        var d = Sub(a,b); float xx = S((double)d.X*d.X), yy = S((double)d.Y*d.Y);
        return S(Math.Sqrt(S((double)xx+yy)));
    }
    public static CirclePreparation Operands(float cs, float ar, float od)
    {
        if (!float.IsFinite(cs) || !float.IsFinite(ar) || !float.IsFinite(od) || cs != 5 || od != 5 || ar is not (5 or 10))
            throw new UnsupportedStarProfileException("Unverified manager preparation profile.");
        float diameter = S(S(512f / 8f) * (1 - .699999988079071 * (((double)cs-5)/5)));
        float radius = S((double)S((double)S(diameter/2f)/S(384f/384f))*S(1.0004099607467651));
        double Interpolate(double d, double low, double middle, double high) => d > 5 ? middle+((high-middle)*(d-5))/5 : d < 5 ? middle-((middle-low)*(5-d))/5 : middle;
        return new(radius, (int)Interpolate(ar,1800,1200,450), 2*(Math.Floor(Interpolate(od,80,50,20))-.5));
    }
    public static IReadOnlyList<DifficultyPair> Prepare(IReadOnlyList<CircleDefinition> circles, CirclePreparation preparation)
    {
        if (!float.IsFinite(preparation.Radius) || preparation.Radius <= 0 || !double.IsFinite(preparation.Preempt) || preparation.Preempt <= 0 || !double.IsFinite(preparation.FullWindow) || preparation.FullWindow <= 0)
            throw new UnsupportedStarProfileException("Finite positive explicit prepared operands required.");
        if (circles.Count > StarAdmission.MaximumCircles) throw new UnsupportedStarProfileException("Circle limit exceeded.");
        var pairs = new List<DifficultyPair>();
        for (int j=1;j<circles.Count;j++)
        {
            var current=circles[j]; var old=circles[j-1];
            if (!float.IsFinite(current.X) || !float.IsFinite(current.Y) || !float.IsFinite(old.X) || !float.IsFinite(old.Y)) throw new UnsupportedStarProfileException("Nonfinite prepared position.");
            double delta=unchecked(current.Start-old.Start);
            var pos=new CirclePoint(current.X,current.Y);var previous=new CirclePoint(old.X,old.Y);
            var pair=new DifficultyPair {Index=j-1,Time=current.Start,Delta=delta,Strain=Math.Max(delta,25),Position=pos,PreviousPosition=previous,Preparation=preparation,History=pairs,Jump=S((double)Distance(pos,previous)*S(50f/preparation.Radius))};
            if(!double.IsFinite(pair.Jump))throw new UnsupportedStarProfileException("Nonfinite Single spacing.");
            var earlier=pair.Previous(1);
            if (earlier is not null)
            {
                var a=Sub(earlier.Position,previous);var b=Sub(pos,previous);
                float cross=S((double)S((double)a.X*b.Y)-S((double)a.Y*b.X));
                float dot=S((double)S((double)a.X*b.X)+S((double)a.Y*b.Y));
                pair.Angle=Math.Abs(Math.Atan2(cross,dot));pair.Axis=Math.Atan2(Math.Abs(b.Y),Math.Abs(b.X));
            }
            pairs.Add(pair); // Preserve original history references before completed-pair sorting.
        }
        return pairs.AsReadOnly();
    }
}
