using Ohno.CorpusAudit;
using Ohno.CircleGame.Selection;

internal static class Program
{
    private static int Main(string[] args)
    {
        if(args.Length==0||args.Contains("--help"))
        {
            Console.Error.WriteLine("Usage: CorpusAudit <osu-directory> [--output report.json] [--include-relative-paths] [--max-charts N] [--max-directories N] [--max-entries N] [--max-depth N]");
            Console.Error.WriteLine("Read-only source scan. JSON goes to stdout unless --output is supplied outside the source tree. Summary goes to stderr. Ctrl+C produces a partial report. Paths/content are omitted by default.");
            return args.Length==0?64:0;
        }
        try
        {
            string root=CatalogPaths.Canonical(args[0]);string? output=null;var options=new AuditOptions();
            for(int i=1;i<args.Length;i++)
            {
                string Value()=>i+1<args.Length?args[++i]:throw new ArgumentException("Missing option value.");
                switch(args[i])
                {
                    case "--output":output=Path.GetFullPath(Value());break;
                    case "--include-relative-paths":options=options with { IncludeRelativePaths=true };break;
                    case "--max-charts":options=options with { MaxCharts=int.Parse(Value(),System.Globalization.CultureInfo.InvariantCulture) };break;
                    case "--max-directories":options=options with { MaxDirectories=int.Parse(Value(),System.Globalization.CultureInfo.InvariantCulture) };break;
                    case "--max-entries":options=options with { MaxEntries=int.Parse(Value(),System.Globalization.CultureInfo.InvariantCulture) };break;
                    case "--max-depth":options=options with { MaxDepth=int.Parse(Value(),System.Globalization.CultureInfo.InvariantCulture) };break;
                    default:throw new ArgumentException("Unknown option.");
                }
            }
            options.Validate();
            if(output is not null)
            {
                CatalogPaths.Canonical(output);
                if(!Path.GetExtension(output).Equals(".json",StringComparison.OrdinalIgnoreCase)||Within(root,output))throw new ArgumentException("Output must be .json outside the scanned source tree.");
            }
            using var cts=new CancellationTokenSource();
            ConsoleCancelEventHandler cancel=(_,e)=>{e.Cancel=true;cts.Cancel();};Console.CancelKeyPress+=cancel;
            AuditReport report;
            try {report=AuditRunner.Run(root,options,cts.Token);}finally{Console.CancelKeyPress-=cancel;}
            string json=AuditRunner.Json(report);
            if(output is null)Console.Write(json);
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                string temporary=output+"."+Guid.NewGuid().ToString("N")+".tmp";
                try {File.WriteAllText(temporary,json,new System.Text.UTF8Encoding(false));File.Move(temporary,output,true);}
                finally {if(File.Exists(temporary))File.Delete(temporary);}
            }
            Console.Error.WriteLine($"ohno audit: {report.Charts.Count} chart files; complete={report.Complete}; {report.Notices.Count} notices (+{report.NoticesOmitted} omitted). Source {report.SourceCommit} ({report.SourceState}).");
            foreach(var pair in report.Cohorts)Console.Error.WriteLine($"{pair.Key}: {pair.Value.Files} files / {pair.Value.UniqueHashes} unique hashes; indexed {pair.Value.Indexed}; canonical {pair.Value.CanonicalSuccess}/{pair.Value.CanonicalAttempts} attempts; consumer checks ready {pair.Value.ConsumerReady}; missing audio {pair.Value.MissingAudio}.");
            foreach(var family in report.CanonicalFailureFamilies.OrderByDescending(p=>p.Value).ThenBy(p=>p.Key,StringComparer.Ordinal).Take(8))Console.Error.WriteLine($"rejection {family.Key}: {family.Value}");
            Console.Error.WriteLine("Headless results do not certify original equivalence or owner playability. Audio presence is not decoding; samples/GUI/health calibration NOT VERIFIED.");
            return !report.Complete?2:report.StageOutcomes.Values.Any(s=>s.ContainsKey("unexpected-failure"))?3:0;
        }
        catch(Exception e) when(e is ArgumentException or IOException or UnauthorizedAccessException or FormatException or OverflowException)
        {Console.Error.WriteLine("Audit invocation/output failed: "+e.GetType().Name+". Check arguments, local root and outside-tree JSON output permissions; no input files were written.");return 64;}
    }
    private static bool Within(string root,string file)=>CatalogPaths.Comparer.Equals(root,file)||file.StartsWith(Path.EndsInDirectorySeparator(root)?root:root+Path.DirectorySeparatorChar,OperatingSystem.IsWindows()?StringComparison.OrdinalIgnoreCase:StringComparison.Ordinal);
}
