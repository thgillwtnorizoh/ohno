using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CorpusAudit;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;

internal static class Program
{
    private static readonly List<string> checks=[];
    private static void Check(bool value,string label){if(!value)throw new InvalidOperationException("CHECK FAILED: "+label);checks.Add(label);}
    private static string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes));
    private static string Map(string rows,string extra="",int mode=0)=>$"""
osu file format v14
[General]
Mode:{mode}
StackLeniency:0
{extra}
[Metadata]
Title:Private chart title must not appear
[Difficulty]
HPDrainRate:5
CircleSize:5
OverallDifficulty:5
ApproachRate:5
SliderMultiplier:1
SliderTickRate:1
[TimingPoints]
0,500,4,1,0,100,1,0
[HitObjects]
{rows}
""";
    private static AuditRow Row(AuditReport r,string relative)=>r.Charts.Single(c=>c.EntryKey==Hash(System.Text.Encoding.UTF8.GetBytes(relative)));
    private static string[] Snapshot(string root)=>Directory.GetFiles(root,"*",SearchOption.AllDirectories).Order(StringComparer.Ordinal).Select(p=>p+" "+Hash(File.ReadAllBytes(p))+" "+File.GetLastWriteTimeUtc(p).Ticks).ToArray();
    private static int Main(string[] args)
    {
        string repo=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);Directory.CreateDirectory(output);
        string temp=Path.Combine(Path.GetTempPath(),"ohno-audit-checks-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);
        var corpusResults=new List<object>();var notRun=new List<string>();
        try
        {
            string circles=Map("100,100,1000,1,0\n200,200,3000,1,0\n300,200,5000,1,0");
            var files=new Dictionary<string,string>{{"secret-name.osu",circles},{"duplicate.OSU",circles},
                {"mode1.osu",Map("100,100,1000,1,0",mode:1)},
                {"spinner.osu",Map("256,192,1000,8,0,2000")},
                {"bad-header.osu","private junk"}, {"bad-hitSound.osu",Map("100,100,1000,1,garbage")},
                {"unknown-path.osu",Map("100,100,1000,2,0,Q|100:100|200:100,1,100")},
                {"missing-audio.osu",Map("100,100,1000,1,0\n200,200,3000,1,0","AudioFilename:private-missing.wav")},
                {"present-audio.osu",Map("100,100,1000,1,0\n200,200,3000,1,0","AudioFilename:present.wav")},
                {"L.osu",Map("100,100,1000,2,0,L|100:100|200:100,2,100")},
                {"B.osu",Map("100,100,1000,2,0,B|100:100|150:150|200:100,1,100")},
                {"P.osu",Map("106,100,1000,2,0,P|106:100|100:106|94:100,1,12")},
                {"C.osu",Map("100,100,1000,2,0,C|100:100|200:100|200:200,1,200")},
                {"invalid-number.osu",Map("junk,100,1000,1,0")},
                {"hold.osu",Map("100,100,1000,128,0,2000:0:0:0:0:")}};
            foreach(var p in files.Reverse())File.WriteAllText(Path.Combine(temp,p.Key),p.Value);
            File.WriteAllBytes(Path.Combine(temp,"present.wav"),[1,2,3]); // Presence only; never claim decode.
            File.WriteAllText(Path.Combine(temp,"ignored.txt"),"not a chart");
            Directory.CreateDirectory(Path.Combine(temp,"nested"));File.WriteAllText(Path.Combine(temp,"nested","z.osu"),circles);
            string[] before=Snapshot(temp);var report=AuditRunner.Run(temp);string json=AuditRunner.Json(report);
            Check(report.Complete&&report.Charts.Count==16,"mixed deterministic recursive chart count; case insensitive suffix");
            Check(Snapshot(temp).SequenceEqual(before),"source exact bytes and modification stamps unchanged");
            Check(json==AuditRunner.Json(AuditRunner.Run(temp)),"same directory twice byte-identical JSON");
            CultureInfo.CurrentCulture=CultureInfo.GetCultureInfo("vi-VN");Check(json==AuditRunner.Json(AuditRunner.Run(temp)),"invariant JSON and feature numbers across vi-VN culture");CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
            Check(!json.Contains(temp)&&!json.Contains("secret-name")&&!json.Contains("Private chart title")&&!json.Contains("private-missing"),"default report omits absolute/relative paths, metadata and media names");
            Check(report.Charts.All(r=>r.RelativePath is null)&&AuditRunner.Run(temp,new(IncludeRelativePaths:true)).Charts.Any(r=>r.RelativePath=="nested/z.osu"),"relative paths opt-in only; normalized descendant path");
            foreach(var p in files)
            {
                var r=Row(report,p.Key);Check(r.Sha256==Hash(File.ReadAllBytes(Path.Combine(temp,p.Key))),"exact input hash "+p.Key);
            }
            Check(report.Charts.Count(r=>r.DuplicateOfEntryKey is not null)==2,"duplicate bytes counted per occurrence with one stable representative");
            Check(Row(report,"secret-name.osu").Canonical.Status=="success","ordinary primary chart canonical success");
            Check(Row(report,"mode1.osu").Cohort=="other-mode"&&Row(report,"mode1.osu").Canonical.Status=="unsupported-admission","other mode counted outside primary and canonical rejection");
            Check(Row(report,"spinner.osu").Cohort=="standard-with-spinner"&&Row(report,"spinner.osu").Canonical.Status=="success","spinner remains supported, separate cohort");
            Check(Row(report,"hold.osu").Cohort!="standard-no-spinner"&&Row(report,"hold.osu").Canonical.Status=="unsupported-admission","hold is not ordinary primary");
            Check(Row(report,"bad-header.osu").Read.Status=="success"&&Row(report,"bad-header.osu").Index.Status=="malformed-input","read success independent of malformed header/index");
            Check(Row(report,"bad-hitSound.osu").Index.Status=="success"&&Row(report,"bad-hitSound.osu").Canonical.Status=="malformed-input","catalog index != canonical malformed admission");
            Check(Row(report,"invalid-number.osu").Canonical.Status=="malformed-input","production finite-number syntax classification");
            Check(Row(report,"unknown-path.osu").Index.Status=="success"&&Row(report,"unknown-path.osu").Canonical.Status=="unsupported-admission","catalog index != unsupported canonical path");
            foreach(string p in new[]{"L","B","P","C"})Check(Row(report,p+".osu").Features!.SliderPaths[p]==1&&Row(report,p+".osu").Canonical.Status=="success","canonical explicit "+p+" path");
            Check(Row(report,"L.osu").Features!.MaximumRepeats==2,"repeat-count feature");
            Check(Row(report,"missing-audio.osu").Canonical.Status=="success"&&Row(report,"missing-audio.osu").Audio.Status=="missing-resource","missing audio separated from prepared success");
            Check(Row(report,"present-audio.osu").Audio.Code=="audio-present-not-decoded","audio presence does not pretend decoding");
            Check(report.Cohorts["standard-no-spinner"].CanonicalSuccess<report.Cohorts["standard-no-spinner"].Files,"primary denominator includes canonical rejections");
            Check(Row(report,"secret-name.osu").Consumers.Values.All(x=>x.Status=="success"),"actual score, fresh-health-source and session construction pass");
            File.WriteAllText(Path.Combine(output,"mixed.json"),json);

            // Immutable diagnostic strings are data, never revision evidence.
            string spoof=Path.Combine(temp,"spoof.osu");
            foreach(string phrase in new[]{"changed since indexing","CHANGED SINCE INDEXING","Chart changed since indexing; refresh and commit its new row: private.osu","unsupported mode: fingerprint-changed"})
            {
                File.WriteAllText(spoof,circles.Replace("CircleSize:5","CircleSize:"+phrase));
                string hash=Hash(File.ReadAllBytes(spoof));long stamp=File.GetLastWriteTimeUtc(spoof).Ticks;
                var immutable=AuditRunner.Run(temp);var rejected=Row(immutable,"spoof.osu");string privateSafe=AuditRunner.Json(immutable);
                Check(rejected.Canonical.Status=="malformed-input"&&rejected.Canonical.Code=="metadata-syntax","immutable metadata phrase rejected by trusted stage: "+phrase);
                Check(rejected.Sha256==hash&&Hash(File.ReadAllBytes(spoof))==hash&&File.GetLastWriteTimeUtc(spoof).Ticks==stamp,"immutable spoof bytes/hash/stamp preserved: "+phrase);
                Check(!privateSafe.Contains(phrase,StringComparison.Ordinal)&&!privateSafe.Contains("spoof.osu")&&privateSafe==AuditRunner.Json(AuditRunner.Run(temp)),"spoof privacy and deterministic report: "+phrase);
                Check(immutable.StageOutcomes.Values.All(c=>c.Values.Sum()==17),"spoof retains every stage denominator: "+phrase);
            }
            File.Delete(spoof);
            Check(AuditRunner.Failure(new InvalidDataException("changed since indexing")).Status=="unclassified-rejection","arbitrary phrase cannot assert revision evidence");
            var forged=AuditRunner.RunCore(temp,new(),default,new((path,_)=>{if(Path.GetFileName(path)=="C.osu")throw new InvalidDataException("Chart changed since indexing; refresh and commit its new row: private.osu");}));
            Check(Row(forged,"C.osu").Canonical.Status=="unclassified-rejection","even forged exact catalog message is not mutation proof");
            // Tests deliberately mutate their synthetic source through internal hooks;
            // production runner exposes no CLI mutation operation.
            var changed=AuditRunner.RunCore(temp,new(),default,new((path,_)=>{if(Path.GetFileName(path)=="C.osu")File.AppendAllText(path,"\n// deliberate change");}));
            Check(Row(changed,"C.osu").Canonical.Status=="changed-input"&&Row(changed,"L.osu").Canonical.Status=="success","changed file detected; subsequent files isolated");
            File.WriteAllText(Path.Combine(temp,"C.osu"),files["C.osu"]);
            var stampOnly=AuditRunner.RunCore(temp,new(),default,new((path,_)=>{if(Path.GetFileName(path)=="C.osu")File.SetLastWriteTimeUtc(path,File.GetLastWriteTimeUtc(path).AddSeconds(2));}));
            Check(Row(stampOnly,"C.osu").Canonical.Status=="changed-input"&&Row(stampOnly,"C.osu").Sha256==Hash(File.ReadAllBytes(Path.Combine(temp,"C.osu"))),"real timestamp-only catalog revision preserved");
            var mutatedMetadata=AuditRunner.RunCore(temp,new(),default,new((path,_)=>{if(Path.GetFileName(path)=="C.osu")File.WriteAllText(path,files["C.osu"].Replace("CircleSize:5","CircleSize:changed since indexing"));}));
            Check(Row(mutatedMetadata,"C.osu").Canonical.Status=="changed-input","actual mutation to malformed metadata uses observed fingerprint transition");
            File.WriteAllText(Path.Combine(temp,"C.osu"),files["C.osu"]);
            var missing=AuditRunner.RunCore(temp,new(),default,new((path,_)=>{if(Path.GetFileName(path)=="C.osu")File.Delete(path);}));
            Check(Row(missing,"C.osu").Canonical.Status=="missing-resource","file disappears after indexing");File.WriteAllText(Path.Combine(temp,"C.osu"),files["C.osu"]);
            var unexpected=AuditRunner.RunCore(temp,new(),default,new((path,_)=>{if(Path.GetFileName(path)=="C.osu")throw new InvalidOperationException("private exception path "+temp);}));
            Check(Row(unexpected,"C.osu").Canonical.Status=="unexpected-failure"&&!AuditRunner.Json(unexpected).Contains("private exception path"),"unexpected error isolated without leaking exception text");
            using(var cts=new CancellationTokenSource())
            {
                var cancelled=AuditRunner.RunCore(temp,new(),cts.Token,new((_,_)=>cts.Cancel()));
                Check(!cancelled.Complete&&cancelled.Notices.Any(n=>n.Code=="cancelled-partial-report"),"mid-scan cooperative cancellation yields disclosed partial report");
            }
            Check(!AuditRunner.Run(temp,new(MaxCharts:2)).Complete,"chart limit disclosed");
            Check(!AuditRunner.Run(temp,new(MaxDirectories:1)).Complete,"directory limit disclosed");
            var entryLimited=AuditRunner.Run(temp,new(MaxEntries:2));Check(!entryLimited.Complete&&entryLimited.Charts.Count==0,"oversized directory excluded deterministically, not arbitrary prefix");
            Check(!AuditRunner.Run(temp,new(MaxDepth:0)).Complete,"depth boundary disclosed");
            Check(!AuditRunner.Run(Path.Combine(temp,"missing-root")).Complete,"missing root disclosed");
            try{AuditRunner.Run(temp,new(MaxCharts:10001));Check(false,"limit upper bound");}catch(ArgumentOutOfRangeException){Check(true,"limits cannot loosen catalog guards");}
            string big=Path.Combine(temp,"large.osu");using(var s=File.Create(big))s.SetLength(ChartFingerprint.MaximumBytes+1);
            Check(Row(AuditRunner.Run(temp),"large.osu").Read.Status=="unsupported-admission","over-four-MiB safely excluded with unknown hash");File.Delete(big);
            // The 2 MiB gameplay guard differs from catalog's 4 MiB guard.
            File.WriteAllText(big,circles+"\n//"+new string('x',2*1024*1024));var medium=Row(AuditRunner.Run(temp),"large.osu");
            Check(medium.Sha256 is not null&&medium.Index.Status=="success"&&medium.Canonical.Status=="unsupported-admission","catalog and canonical independent file-size limits");File.Delete(big);
            try
            {
                Directory.CreateSymbolicLink(Path.Combine(temp,"linked-folder"),Path.Combine(temp,"nested"));
                File.CreateSymbolicLink(Path.Combine(temp,"linked.osu"),Path.Combine(temp,"C.osu"));
                var linked=AuditRunner.Run(temp);Check(!linked.Complete&&linked.Charts.Count==16&&linked.Notices.Count(n=>n.Code=="reparse-entry-excluded")==2,"symlink file/directory excluded without traversal");
                Directory.Delete(Path.Combine(temp,"linked-folder"));File.Delete(Path.Combine(temp,"linked.osu"));
            }
            catch(Exception e)when(e is UnauthorizedAccessException or IOException){notRun.Add("symlink creation unavailable: "+e.GetType().Name);}
            Check(AuditRunner.Failure(new UnauthorizedAccessException("private")).Code=="access-denied","unreadable file taxonomy without message disclosure");
            Check(AuditRunner.Failure(new InvalidDataException("opaque rejection")).Status=="unclassified-rejection","unknown production rejection stays unclassified");
            Check(AuditRunner.Failure(new OverflowException()).Status=="malformed-input","numeric overflow taxonomy");
            string blocked=Path.Combine(temp,"C.osu");
            using(var hold=File.Open(blocked,FileMode.Open,FileAccess.ReadWrite,FileShare.None))
                Check(Row(AuditRunner.Run(temp),"C.osu").Read.Status=="read-error","actual file-sharing read failure isolated");
            Check(report.CanonicalFailureFamilies.Keys.Any(k=>k.EndsWith("hitsound-syntax",StringComparison.Ordinal)),"normalized failure-family summary is actionable and content-free");
            (int Code,string Stdout,string Stderr) Cli(params string[] arguments)
            {
                var start=new System.Diagnostics.ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")??"dotnet"){RedirectStandardOutput=true,RedirectStandardError=true};
                start.ArgumentList.Add(Path.Combine(repo,"reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll"));
                foreach(string arg in arguments)start.ArgumentList.Add(arg);
                using var process=System.Diagnostics.Process.Start(start)!;
                var stdout=process.StandardOutput.ReadToEndAsync();var stderr=process.StandardError.ReadToEndAsync();
                if(!process.WaitForExit(30000)){process.Kill(true);throw new InvalidOperationException("CLI timeout");}
                return (process.ExitCode,stdout.GetAwaiter().GetResult(),stderr.GetAwaiter().GetResult());
            }
            var cli=Cli(temp);Check(cli.Code==0&&cli.Stdout==json&&cli.Stderr.Contains("standard-no-spinner:"),"real CLI stdout JSON and stderr summary");
            string cliOut=Path.Combine(output,"cli-report.json");var cliFile=Cli(temp,"--output",cliOut);
            Check(cliFile.Code==0&&cliFile.Stdout==""&&File.ReadAllText(cliOut)==json,"real CLI JSON file outside source tree");
            var forbidden=Cli(temp,"--output",Path.Combine(temp,"report.json"));
            Check(forbidden.Code==64&&!File.Exists(Path.Combine(temp,"report.json")),"CLI rejects output under source root");
            Check(Cli(Path.GetPathRoot(temp)!,"--output",Path.Combine(temp,"report.json")).Code==64&&!File.Exists(Path.Combine(temp,"report.json")),"filesystem-root output containment guard");
            Check(Cli(temp,"--max-charts","1").Code==2,"CLI partial scan exit2");
            Check(Cli(temp,"--unknown").Code==64,"CLI usage exit64");
            Check(files.All(p=>File.ReadAllText(Path.Combine(temp,p.Key))==p.Value),"all deliberate synthetic mutation controls restored exact contents");
            var source=report.SourceHashes;Check(source["Beatmap.cs"]==Hash(File.ReadAllBytes(Path.Combine(repo,"reconstructed/CircleGame/Gameplay/Beatmap.cs")))&&source["LocalChartCatalog.cs"]==Hash(File.ReadAllBytes(Path.Combine(repo,"reconstructed/CircleGame/Selection/LocalChartCatalog.cs"))),"assembly stamps exact actual production linkage");
            foreach(var definition in new[]{("C001-24","reconstructed/campaigns/C001/corpus",24),("C001-49","re/campaigns/C001/corpus/fixtures",49),("P21","re/campaigns/C001/perfect-circle/fixtures",21),("C19","re/campaigns/C001/catmull/fixtures",19),("stacking18","re/campaigns/C001/stacking/fixtures",18),("QA15","re/qa/qa15/beatmaps/Songs",27)})
            {
                var r=AuditRunner.Run(Path.Combine(repo,definition.Item2));string data=AuditRunner.Json(r);
                Check(r.Complete&&r.Charts.Count==definition.Item3,definition.Item1+" expected fixture count");
                Check(data==AuditRunner.Json(AuditRunner.Run(Path.Combine(repo,definition.Item2))),definition.Item1+" twice byte-identical");
                foreach(var c in r.Charts)Check(c.Sha256 is not null,definition.Item1+" exact hash available "+c.EntryKey[..8]);
                if(definition.Item1=="P21")Check(r.Charts.Count(c=>c.Canonical.Status=="success")==17,"retained P 17 admissions/4 exclusions");
                if(definition.Item1=="C19")Check(r.Charts.Count(c=>c.Canonical.Status=="success")==13,"retained C 13 admissions/6 exclusions");
                if(definition.Item1=="stacking18"){Check(r.Charts.All(c=>c.Canonical.Status=="success"),"retained stacking18 canonical admissions");Check(r.Charts.Any(c=>c.Canonical.Status=="success"&&c.Consumers["fresh-health-source"].Status=="unsupported-admission"),"prepared stacking success distinct from existing health overlap admission");}
                File.WriteAllText(Path.Combine(output,definition.Item1+".json"),data);
                corpusResults.Add(new{name=definition.Item1,files=r.Charts.Count,reportSha256=Hash(System.Text.Encoding.UTF8.GetBytes(data)),r.Cohorts,r.CanonicalOutcomes,r.CanonicalFailureFamilies});
            }
            var result=new{status="PASS",actor="Work2 development; independent Work1 verification pending",sourceCommit=report.SourceCommit,sourceState=report.SourceState,runtime=Environment.Version.ToString(),checks=checks.Count,notRun,corpora=corpusResults,results=checks,ownerCollection="NOT PROVIDED / NOT RUN",originalRuntime="NOT RUN",healthCalibrationAndGui="NOT RUN"};
            File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(result,AuditRunner.JsonOptions)+"\n");
            Console.WriteLine($"CORPUS AUDIT DEVELOPMENT: {checks.Count}/{checks.Count} PASS; 6 historical fixture roots + mixed synthetic; {notRun.Count} platform controls NOT RUN");return 0;
        }
        finally{Directory.Delete(temp,true);}
    }
}
