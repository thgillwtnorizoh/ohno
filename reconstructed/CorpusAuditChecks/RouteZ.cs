using System.Text.Json;
using Ohno.CorpusAudit;
using Ohno.CircleGame.Gameplay;

internal static partial class Program
{
    private const string Unproven="Legacy spinner stack profile lacks a zero-displacement certificate.";
    private const string WorkLimit="Legacy spinner zero-displacement certificate exceeds pair work limit.";
    private static void RouteZChecks(string repo,string output,string temp,bool baseline)
    {
        string root=Path.Combine(temp,"route-z");Directory.CreateDirectory(root);
        string authority=Path.Combine(repo,"re/campaigns/C001/legacy-spinner-profile/fixtures");
        var files=new Dictionary<string,string>{
            ["unproven.osu"]=File.ReadAllText(Path.Combine(authority,"v05-spinner-after.osu")),
            ["accepted.osu"]=File.ReadAllText(Path.Combine(authority,"v05-default-single.osu")),
            ["modern.osu"]=File.ReadAllText(Path.Combine(authority,"v06-circle-pair.osu")),
            ["work.osu"]=Map("256,192,2000,8,0,5000\n"+string.Join('\n',Enumerable.Range(0,1414).Select(i=>$"64,64,{6000+2*i},1,0"))).Replace("format v14","format v5").Replace("StackLeniency:0","StackLeniency:0.7")};
        foreach(var p in files)File.WriteAllText(Path.Combine(root,p.Key),p.Value);
        foreach(var (file,message) in new[]{("unproven.osu",Unproven),("work.osu",WorkLimit)})
        {
            try{Beatmap.LoadCanonical(Path.Combine(root,file));Check(false,file+" real canonical rejection");}
            catch(InvalidDataException e){Check(e.Message==message,file+" exact production exception");}
        }
        var before=Snapshot(root);var report=AuditRunner.Run(root);string json=AuditRunner.Json(report);
        Check(report.Complete&&report.Charts.Count==4,"RouteZ independent stage denominator");
        Check(Row(report,"unproven.osu").Canonical.Status==(baseline?"unclassified-rejection":"unsupported-admission")&&Row(report,"unproven.osu").Canonical.Code==(baseline?"production-rejection-unclassified":"stack-certificate-unproven"),"RouteZ unproven precise code");
        Check(Row(report,"work.osu").Canonical.Status=="unsupported-admission"&&Row(report,"work.osu").Canonical.Code==(baseline?"production-profile":"stack-certificate-work-limit"),"RouteZ work limit precise code");
        foreach(string f in files.Keys){Check(Row(report,f).Index.Status=="success",f+" actual indexing stays independent");Check(Row(report,f).Sha256==Hash(File.ReadAllBytes(Path.Combine(root,f))),f+" exact bytes captured");}
        Check(Row(report,"accepted.osu").Canonical.Status=="success"&&Row(report,"modern.osu").Canonical.Status=="success","zero-proof and modern accepted controls");
        Check(json==AuditRunner.Json(AuditRunner.Run(root)),"RouteZ deterministic report");
        Check(before.SequenceEqual(Snapshot(root)),"RouteZ read-only file bytes and stamps");
        Check(!json.Contains(root)&&!json.Contains(Unproven)&&!json.Contains(WorkLimit)&&!json.Contains("Private chart title must not appear"),"RouteZ privacy excludes paths/messages/chart text");
        foreach(var (message,code) in new[]{(Unproven,"stack-certificate-unproven"),(WorkLimit,"stack-certificate-work-limit")})
        {
            if(!baseline)Check(AuditRunner.Failure(new InvalidDataException(message)) is { Status:"unsupported-admission",ExceptionType:"InvalidDataException" }&&AuditRunner.Failure(new InvalidDataException(message)).Code==code,"typed exact fixed classifier "+code);
            foreach(string variant in new[]{"prefix "+message,message+" suffix",message.ToLowerInvariant(),message+"\n",message.Replace("certificate","certificateX")})
                Check(AuditRunner.Failure(new InvalidDataException(variant)) == new Outcome(message==Unproven?"unclassified-rejection":"unsupported-admission",message==Unproven?"production-rejection-unclassified":"production-profile","InvalidDataException"),"adversarial nonexact retains exact historical taxonomy");
            Check(AuditRunner.Failure(new FormatException(message)).Code=="numeric-or-format","wrong exception type cannot select fixed code");
            string path=Path.Combine(root,"spoof.osu");string text=Map("64,64,1000,1,0").Replace("CircleSize:5","CircleSize:"+message);File.WriteAllText(path,text);
            var spoof=AuditRunner.Run(root);Check(Row(spoof,"spoof.osu").Canonical is {Status:"malformed-input",Code:"metadata-syntax"},"chart controlled exact message remains metadata syntax");
            Check(!AuditRunner.Json(spoof).Contains(message),"spoof diagnostic payload private");File.Delete(path);
        }
        var changed=AuditRunner.RunCore(root,new(),default,new((path,_)=>{if(Path.GetFileName(path)=="unproven.osu")File.AppendAllText(path,"\n//actual mutation");}));
        Check(Row(changed,"unproven.osu").Canonical is {Status:"changed-input",Code:"fingerprint-changed"},"actual revision change outranks stack rejection");
        File.WriteAllText(Path.Combine(root,"unproven.osu"),files["unproven.osu"]);
        File.WriteAllText(Path.Combine(output,baseline?"route-z-baseline.json":"route-z.json"),json);
        File.WriteAllText(Path.Combine(output,"route-z-fixtures.json"),JsonSerializer.Serialize(files.ToDictionary(p=>p.Key,p=>Hash(System.Text.Encoding.UTF8.GetBytes(p.Value))),AuditRunner.JsonOptions));
        Directory.Delete(root,true);
    }
}
