using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;

namespace Ohno.CorpusAudit;

public sealed record AuditOptions(int MaxCharts = LocalChartCatalog.MaximumCharts,
    int MaxDirectories = LocalChartCatalog.MaximumDirectories, int MaxEntries = LocalChartCatalog.MaximumFileCandidates,
    int MaxDepth = LocalChartCatalog.MaximumDepth, bool IncludeRelativePaths = false)
{
    public void Validate()
    {
        if (MaxCharts is < 1 or > LocalChartCatalog.MaximumCharts || MaxDirectories is < 1 or > LocalChartCatalog.MaximumDirectories ||
            MaxEntries is < 1 or > LocalChartCatalog.MaximumFileCandidates || MaxDepth is < 0 or > LocalChartCatalog.MaximumDepth)
            throw new ArgumentOutOfRangeException(nameof(AuditOptions), "Audit limits must be within the production catalog limits.");
    }
}
public sealed record Outcome(string Status, string Code, string? ExceptionType = null);
public sealed record Features(int Format, int? Mode, int RuntimeRows, int Circles, int Sliders, int Spinners,
    int OtherObjects, int UnclassifiedRows, SortedDictionary<string,int> SliderPaths, int? MinimumRepeats, int? MaximumRepeats,
    int UnknownRepeats, int TimingRows, int DistinctBeatLengths, int ExplicitBaseTimingRows, int ExplicitInheritedTimingRows,
    int UnparsedBeatLengthRows, string StackingProfile);
public sealed record IndexFeatures(int Format, int Mode, int Objects, int Circles, int Sliders, int Spinners);
public sealed class AuditRow
{
    public required string EntryKey { get; init; }
    public string? RelativePath { get; init; }
    public string? Sha256 { get; set; }
    public long? Bytes { get; set; }
    public string? DuplicateOfEntryKey { get; set; }
    public string Cohort { get; set; } = "unknown";
    public Features? Features { get; set; }
    public IndexFeatures? IndexedFeatures { get; set; }
    public Outcome Read { get; set; } = new("not-run", "not-attempted");
    public Outcome FeatureProjection { get; set; } = new("not-run", "not-attempted");
    public Outcome Index { get; set; } = new("not-run", "not-attempted");
    public Outcome Canonical { get; set; } = new("not-run", "not-attempted");
    public SortedDictionary<string,Outcome> Consumers { get; } = new(StringComparer.Ordinal);
    public Outcome Audio { get; set; } = new("unknown", "canonical-not-prepared");
    public int? PreparedObjects { get; set; }
    public int? PreparedSegments { get; set; }
}
public sealed record AuditNotice(string EntryKey, string Code);
public sealed record CohortCounts(int Files, int CanonicalAttempts, int CanonicalSuccess, int Indexed,
    int ConsumerReady, int MissingAudio, int UniqueHashes);
public sealed record AuditReport(int SchemaVersion, string Tool, string SourceCommit, string SourceState,
    SortedDictionary<string,string> SourceHashes, AuditOptions Limits, bool Complete, int Directories, int Entries,
    int NonChartFiles, int NoticesOmitted, IReadOnlyList<AuditNotice> Notices, IReadOnlyList<AuditRow> Charts,
    SortedDictionary<string,CohortCounts> Cohorts, SortedDictionary<string,int> CanonicalOutcomes, SortedDictionary<string,int> CanonicalFailureFamilies, SortedDictionary<string,SortedDictionary<string,int>> StageOutcomes, string Boundary);
internal sealed record AuditHooks(Action<string,CatalogChart>? AfterIndex = null);

public static class AuditRunner
{
    public static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
    public static string Json(AuditReport report) => JsonSerializer.Serialize(report, JsonOptions) + "\n";
    public static AuditReport Run(string root, AuditOptions? options = null, CancellationToken token = default)
        => RunCore(root, options ?? new(), token, null);
    internal static AuditReport RunCore(string root, AuditOptions options, CancellationToken token, AuditHooks? hooks)
    {
        options.Validate();
        var rows = new List<AuditRow>(); var notices = new List<AuditNotice>(); int omitted = 0, dirs = 0, entries = 0, nonCharts = 0;
        bool complete = true;
        string Key(string relative) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(relative.Replace('\\','/'))));
        void Notice(string relative, string code) { if (notices.Count < 1024) notices.Add(new(Key(relative), code)); else omitted++; }
        try
        {
            root = CatalogPaths.Canonical(root);
            if (!Directory.Exists(root)) { complete = false; Notice(".", "root-missing"); }
            else Visit(root, ".", 0);
        }
        catch (OperationCanceledException) { complete = false; Notice(".", "cancelled-partial-report"); }
        catch (Exception e) when (Recoverable(e)) { complete = false; Notice(".", "root-" + Failure(e).Code); }
        var hashes = new Dictionary<string,string>(StringComparer.Ordinal);
        foreach (var row in rows)
            if (row.Sha256 is { } hash) { if (hashes.TryGetValue(hash, out string? first)) row.DuplicateOfEntryKey = first; else hashes.Add(hash, row.EntryKey); }
        var cohorts = new SortedDictionary<string,CohortCounts>(StringComparer.Ordinal);
        foreach (var group in rows.GroupBy(r => r.Cohort).OrderBy(g => g.Key, StringComparer.Ordinal))
            cohorts.Add(group.Key, new(group.Count(), group.Count(r => r.Canonical.Status != "not-run"),
                group.Count(r => r.Canonical.Status == "success"), group.Count(r => r.Index.Status == "success"),
                group.Count(r => r.Canonical.Status == "success" && r.Consumers.Count == 4 && r.Consumers.Values.All(x => x.Status == "success")),
                group.Count(r => r.Audio.Status == "missing-resource"), group.Where(r => r.Sha256 is not null).Select(r => r.Sha256).Distinct().Count()));
        var outcomes = new SortedDictionary<string,int>(StringComparer.Ordinal);
        foreach (var g in rows.GroupBy(r => r.Canonical.Status)) outcomes.Add(g.Key,g.Count());
        var families = new SortedDictionary<string,int>(StringComparer.Ordinal);
        foreach (var g in rows.Where(r=>r.Canonical.Status is not ("success" or "not-run")).GroupBy(r=>r.Canonical.Status+"/"+r.Canonical.Code)) families.Add(g.Key,g.Count());
        var stages=new SortedDictionary<string,SortedDictionary<string,int>>(StringComparer.Ordinal);
        foreach(string stage in new[]{"read","feature-projection","index","canonical","audio","structure","automatic-score","session-without-health-calibration","fresh-health-source"})
        {
            var counts=new SortedDictionary<string,int>(StringComparer.Ordinal);
            foreach(var row in rows)
            {
                Outcome result=stage switch { "read"=>row.Read,"feature-projection"=>row.FeatureProjection,"index"=>row.Index,"canonical"=>row.Canonical,"audio"=>row.Audio,_=>row.Consumers.GetValueOrDefault(stage,new("not-run","canonical-dependency")) };
                counts[result.Status]=counts.GetValueOrDefault(result.Status)+1;
            }
            stages.Add(stage,counts);
        }
        var stamps = typeof(AuditRunner).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>().ToDictionary(a => a.Key, a => a.Value ?? "", StringComparer.Ordinal);
        var sources = new SortedDictionary<string,string>(StringComparer.Ordinal);
        foreach (var p in stamps.Where(p => p.Key.StartsWith("SourceHash.",StringComparison.Ordinal))) sources.Add(p.Key[11..],p.Value);
        return new(1, "ohno-corpus-audit/1", stamps.GetValueOrDefault("SourceCommit","unknown"), stamps.GetValueOrDefault("SourceState","unknown"),
            sources, options, complete, dirs, entries, nonCharts, omitted, notices, rows, cohorts, outcomes, families, stages,
            "Headless finite indexing/preparation/structural checks only; health calibration, media decoding, original runtime and owner playability NOT VERIFIED.");

        void Visit(string directory, string relative, int depth)
        {
            token.ThrowIfCancellationRequested();
            if (dirs >= options.MaxDirectories) { complete = false; Notice(relative,"directory-limit"); return; }
            dirs++;
            string[] children;
            try
            {
                CatalogPaths.Canonical(directory);
                // A whole directory is excluded if oversized, instead of selecting an
                // enumeration-order-dependent prefix. Enumeration itself is bounded.
                children = Directory.EnumerateFileSystemEntries(directory).Take(Math.Min(10000,options.MaxEntries)+1).ToArray();
                if (children.Length > Math.Min(10000,options.MaxEntries) || children.Length > options.MaxEntries-entries)
                { complete = false; Notice(relative,"entry-limit-directory-excluded"); return; }
                Array.Sort(children,StringComparer.Ordinal);
            }
            catch (Exception e) when (Recoverable(e)) { complete = false; Notice(relative,"directory-"+Failure(e).Code); return; }
            foreach (string path in children)
            {
                token.ThrowIfCancellationRequested();
                string rel = relative == "." ? Path.GetFileName(path) : relative + "/" + Path.GetFileName(path);
                if (entries >= options.MaxEntries) { complete = false; Notice(rel,"entry-limit"); return; }
                entries++;
                try
                {
                    FileAttributes attributes = File.GetAttributes(path);
                    if ((attributes & FileAttributes.ReparsePoint)!=0) { complete = false; Notice(rel,"reparse-entry-excluded"); continue; }
                    if ((attributes & FileAttributes.Directory)!=0)
                    {
                        if (depth >= options.MaxDepth) { complete = false; Notice(rel,"depth-limit"); }
                        else Visit(path,rel,depth+1);
                    }
                    else if (Path.GetExtension(path).Equals(".osu",StringComparison.OrdinalIgnoreCase))
                    {
                        if (rows.Count >= options.MaxCharts) { complete = false; Notice(rel,"chart-limit"); return; }
                        rows.Add(AuditFile(path,rel,Key(rel),options,token,hooks));
                    }
                    else nonCharts++;
                }
                catch (OperationCanceledException) { throw; }
                catch (Exception e) when (Recoverable(e))
                { complete = false; Notice(rel,"entry-"+Failure(e).Code); }
            }
        }
    }
    private static AuditRow AuditFile(string path,string relative,string key,AuditOptions options,CancellationToken token,AuditHooks? hooks)
    {
        var row = new AuditRow { EntryKey=key,RelativePath=options.IncludeRelativePaths?relative:null };
        CatalogChart chart; bool metadataRejected = false;
        try
        {
            var read = ChartFingerprint.Read(path,token); row.Sha256=read.Fingerprint.Sha256;row.Bytes=read.Bytes.Length;row.Read=new("success","exact-bytes-sha256");
            // Source-stage feature projection is diagnostic only: production document,
            // section readers, metadata normalization and runtime type priority are used.
            try { row.Features=Classify(OsuDocument.Read(read.Bytes,token),token,out metadataRejected); row.FeatureProjection=new("success","production-document-runtime-stage"); }
            catch (Exception e) when (Recoverable(e)) { row.FeatureProjection=Failure(e); }
            chart=LocalChartCatalog.Index(path,token:token);
            row.Index=chart.Availability switch { ChartAvailability.Indexed=>new("success","catalog-indexed"),
                ChartAvailability.Unsupported=>Failure(new InvalidDataException(chart.Error??"Unsupported catalog profile")),
                ChartAvailability.Missing=>new("missing-resource","chart-missing"),
                _=>Failure(new InvalidDataException(chart.Error??"Unclassified index rejection")) };
            if (chart.Metadata is { } m) row.IndexedFeatures=new(m.Format,m.Ruleset,m.Objects,m.Circles,m.Sliders,m.Spinners);
            if (chart.Fingerprint is null) { row.Canonical=new("not-run","no-index-fingerprint");return row; }
            if (read.Fingerprint != chart.Fingerprint) { row.Canonical=new("changed-input","changed-during-index");return row; }
            if (row.Features is { } f) row.Cohort=f.Mode switch { 0 when f.UnclassifiedRows==0 && f.OtherObjects==0 && f.Spinners==0=>"standard-no-spinner",
                0 when f.Spinners>0=>"standard-with-spinner",0=>"standard-unclassified-objects",null=>"unknown",_=>"other-mode" };
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception e) when (Recoverable(e)) { if(row.Read.Status=="not-run")row.Read=Failure(e);else row.Index=Failure(e);return row; }
        Beatmap map;
        try
        {
            // Deliberately attempt canonical preparation even when index status is
            // Unsupported/Invalid but has a fingerprint. Index != gameplay admission.
            hooks?.AfterIndex?.Invoke(path,chart);
            map=chart.LoadCanonicalGameplay(token);
            if (map.Document?.SourceSha256 is not { } preparedHash) { row.Canonical=new("unexpected-failure","prepared-source-hash-missing");return row; }
            if (preparedHash != row.Sha256) { row.Canonical=new("changed-input","prepared-bytes-mismatch");return row; }
            row.Canonical=new("success","canonical-prepared");
            row.PreparedObjects=map.Circles.Count+map.Sliders.Count+map.Spinners.Count;
            row.PreparedSegments=map.Sliders.Sum(s=>s.Geometry.Segments.Count);
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception e) when (Recoverable(e)) { row.Canonical=CanonicalFailure(chart,e,metadataRejected,token);return row; }
        row.Consumers["structure"]=Try(()=>CheckStructure(map,token));
        ScoreConfiguration? score=null; CircleSession? session=null;
        row.Consumers["automatic-score"]=Try(()=>score=ScoreConfiguration.FromBeatmap(map));
        row.Consumers["session-without-health-calibration"]=score is null?new("not-run","score-dependency"):Try(()=>session=CircleSession.CreateFreshOrdinary(map,score,false));
        row.Consumers["fresh-health-source"]=session is null?new("not-run","session-dependency"):Try(()=>HealthChart.FromMap(map,session.Approach,HealthPreparationPolicy.E036FreshOrdinary));
        if (map.AudioPath is null) row.Audio=new("not-declared","audio-not-declared");
        else row.Audio=Try(()=> { CatalogPaths.Canonical(map.AudioPath);if(!File.Exists(map.AudioPath))throw new FileNotFoundException(); },"audio-present-not-decoded");
        return row;
    }
    private static Outcome CanonicalFailure(CatalogChart chart,Exception failure,bool metadataRejected,CancellationToken token)
    {
        // Catalog verification is retained, but its exception text is not trusted
        // provenance. Observe the revision independently even when loading failed.
        try
        {
            if (ChartFingerprint.Read(chart.Path,token).Fingerprint != chart.Fingerprint)
                return new("changed-input","fingerprint-changed");
        }
        catch(OperationCanceledException){throw;}
        catch(Exception observation)when(Recoverable(observation)){return Failure(observation);}
        // The same captured document failed production metadata preparation.
        // Never select a reason from the offending value embedded in its error.
        return metadataRejected && failure is InvalidDataException
            ? new("malformed-input","metadata-syntax",nameof(InvalidDataException)) : Failure(failure);
    }
    private static Outcome Try(Action action,string success="checked")
    { try { action();return new("success",success); }catch(OperationCanceledException){throw;}catch(Exception e)when(Recoverable(e)){return Failure(e);} }
    private static bool Recoverable(Exception e)=>e is not OutOfMemoryException and not StackOverflowException and not AccessViolationException;
    internal static Outcome Failure(Exception e)
    {
        string name=e.GetType().Name;string m=e.Message.ToLowerInvariant();
        if(e is FileNotFoundException or DirectoryNotFoundException)return new("missing-resource","file-missing",name);
        if(e is UnauthorizedAccessException)return new("read-error","access-denied",name);
        if(e is FormatException or OverflowException)return new("malformed-input","numeric-or-format",name);
        if(e is InvalidDataException)
        {
            if(new[]{"malformed","missing osu format header","must be an integer","needs path","invalid slider control","invalid int32","invalid finite number","invalid automatic score","needs start/end","needs int32","needs time and beat","timing uninherited","short timing","must be 0 or 1","metadata line"}.Any(m.Contains))
                return new("malformed-input",RejectionCode(m,"production-syntax"),name);
            if(new[]{"limit","exceed","at most","up to","requires","supports","supported","unsupported","outside","finite","must be greater","must follow","must be separate","must stay","must be a local","only ordinary","only standard","use supported","use an osu","no playable","no indexed","needs explicit controls","needs ordered","needs nonempty","newcombo input is unresolved","unreconstructed"}.Any(m.Contains))
                return new("unsupported-admission",RejectionCode(m,"production-profile"),name);
            return new("unclassified-rejection","production-rejection-unclassified",name);
        }
        if(e is IOException)return new("read-error","io-error",name);
        return new("unexpected-failure","unexpected-exception",name);
    }
    private static string RejectionCode(string m,string fallback)
    {
        // Conservative allowlist for this exact production revision. Do not emit
        // raw exception messages: they can contain paths or chart-supplied text.
        (string Pattern,string Code)[] rules=[
            ("format", "format-profile"),("mode:", "mode-profile"),("ruleset", "mode-profile"),
            ("factory", "object-factory"),("equal-time", "equal-time-objects"),("overlapping", "overlap-profile"),
            ("spinner intervals", "spinner-overlap"),("spinner end", "spinner-duration"),
            ("2 mb", "gameplay-file-size"),("4 mib", "catalog-file-size"),("line limit", "catalog-line-limit"),
            ("hitSound".ToLowerInvariant(), "hitsound-syntax"),("invalid finite number", "numeric-syntax"),
            ("slider control", "slider-control-syntax"),("normalized controls", "slider-control-limit"),
            ("path needs explicit", "slider-minimal-controls"),("l/b paths", "slider-kind-profile"),
            ("traversal", "slider-repeat-profile"),("slider length", "slider-length-profile"),
            ("131072", "catmull-coordinate-limit"),("catmull", "catmull-finite-profile"),
            ("perfect circle", "perfect-finite-profile"),("nonzero raw", "zero-path"),("vanished", "empty-adjusted-path"),
            ("segment", "geometry-resource-limit"),("checkpoint", "checkpoint-profile"),
            ("timing", "timing-profile"),("audiofilename", "audio-path-safety"),
            ("score", "score-profile"),("health", "health-profile"),("metadata line", "metadata-syntax"),
            ("no playable", "no-playable-objects"),("no indexed", "no-indexed-objects")];
        return rules.FirstOrDefault(r=>m.Contains(r.Pattern,StringComparison.Ordinal)).Code??fallback;
    }
    private static Features Classify(OsuDocument doc,CancellationToken token,out bool metadataRejected)
    {
        var settings=OsuMetadataSettings.Prepare(doc,token);metadataRejected=settings.Error is not null;int rows=0,circles=0,sliders=0,spinners=0,other=0,unknown=0,unknownRepeats=0,timing=0,bases=0,inherited=0,unknownTiming=0;
        int? min=null,max=null;var beats=new HashSet<double>();var paths=new SortedDictionary<string,int>(StringComparer.Ordinal);
        foreach(var entry in doc.ReadStage(OsuReadStage.RuntimeObjects,token))
        {
            if(entry.Section=="[TimingPoints]")
            {
                timing++;var cells=entry.Text.Split(',');
                if(cells.Length>=2&&double.TryParse(cells[1],NumberStyles.Float,CultureInfo.InvariantCulture,out double b)&&double.IsFinite(b))beats.Add(b);else unknownTiming++;
                if(cells.Length>=7) {if(cells[6].Trim()=="1")bases++;else if(cells[6].Trim()=="0")inherited++;}
            }
            if(entry.Section!="[HitObjects]")continue;
            rows++;var p=entry.Text.Split(',');
            if(p.Length<4||!int.TryParse(p[3],NumberStyles.Integer,CultureInfo.InvariantCulture,out int raw)){unknown++;continue;}
            switch(OriginalObjectClassification.Runtime(raw))
            {
                case OriginalRegisteredType.Circle:circles++;break;
                case OriginalRegisteredType.SpinnerBase:spinners++;break;
                case OriginalRegisteredType.Slider:
                    sliders++;string k=p.Length>5?p[5].Split('|')[0]:"";if(k is not ("L" or "B" or "P" or "C"))k="unknown-or-other";
                    paths[k]=paths.GetValueOrDefault(k)+1;
                    if(p.Length>6&&int.TryParse(p[6],NumberStyles.Integer,CultureInfo.InvariantCulture,out int repeats)){min=min is null?repeats:Math.Min(min.Value,repeats);max=max is null?repeats:Math.Max(max.Value,repeats);}else unknownRepeats++;
                    break;
                default:other++;break;
            }
        }
        string stack=settings.Error is not null?"unknown":settings.StackLeniency<=0?"disabled":doc.FormatVersion>5&&settings.Cs==5&&settings.Ar==5?"finite-cs5-ar5-no-mod":"outside-verified-stacking-profile";
        return new(doc.FormatVersion,settings.Error is null?settings.Mode:null,rows,circles,sliders,spinners,other,unknown,paths,min,max,unknownRepeats,timing,beats.Count,bases,inherited,unknownTiming,stack);
    }
    private static void CheckStructure(Beatmap map,CancellationToken token)
    {
        if(map.Document?.SourceSha256 is null||map.HealthSource?.IsFreshFor(map)!=true||map.CircleSounds is null)throw new InvalidOperationException("Prepared sidecar identity missing.");
        static bool Finite(Vector2 p)=>float.IsFinite(p.X)&&float.IsFinite(p.Y);
        foreach(var c in map.Circles)if(!Finite(c.Position))throw new InvalidOperationException("Nonfinite circle.");
        var soundParents=map.CircleSounds.RegisteredOverrides.Where(o=>o.Slider is not null).Select(o=>o.Slider!).ToHashSet(ReferenceEqualityComparer.Instance);
        foreach(var s in map.Sliders)
        {
            token.ThrowIfCancellationRequested();var g=s.Geometry;
            if(g.Segments.Count==0||g.CumulativeLengths.Count!=g.Segments.Count||s.Movements.Count!=g.Segments.Count*s.Traversals||!double.IsFinite(g.CumulativeLengths[^1]))throw new InvalidOperationException("Invalid prepared geometry contract.");
            if(g.Segments.Any(p=>!Finite(p.From)||!Finite(p.To))||s.Checkpoints.Any(p=>!Finite(p.Position))||s.Movements.Any(p=>!Finite(p.From)||!Finite(p.To)))throw new InvalidOperationException("Nonfinite prepared path.");
            for(int i=0;i<g.CumulativeLengths.Count;i++)if(!double.IsFinite(g.CumulativeLengths[i])||g.CumulativeLengths[i]<0||(i>0&&g.CumulativeLengths[i]<g.CumulativeLengths[i-1]))throw new InvalidOperationException("Invalid cumulative path distance.");
            if(s.Samples is null||!s.Samples.Schedule.SequenceEqual(s.Checkpoints.Select(c=>c.Time)))throw new InvalidOperationException("Checkpoint/sample schedule identity missing.");
            if(!soundParents.Contains(s))throw new InvalidOperationException("Slider sound identity missing.");
        }
    }
}
