using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Selection;
using Ohno.CorpusAudit;

string root=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);string folder=Path.Combine(root,"re/campaigns/C001/versioned-spinners");
var cases=JsonDocument.Parse(File.ReadAllText(Path.Combine(folder,"manifest.json"))).RootElement.GetProperty("cases");
var checks=new List<string>();var results=new List<object>();
void Check(bool value,string why){if(!value)throw new InvalidOperationException(why);checks.Add(why);}
string FailureSite(Exception e)
{
 string m=e.Message;
 if(m=="R07 supports only modern-policy, format-v14 spinners.")return "Beatmap.LoadCore: spinner version !=14 guard";
 if(m=="Use an osu file format v5–v14 supported circle/Linear-slider map.")return "Beatmap.LoadCore: outer format <5 or >14 guard";
 if(m.Contains("use supported circles, sliders or format-v14 spinners.",StringComparison.Ordinal))return "Beatmap.LoadCore: row Int32 time/type syntax gate";
 if(m=="R02 requires explicit Mode: 0 (osu!standard).")return "Beatmap.LoadCore: standard-mode guard";
 if(m.Contains("spinner end must follow start",StringComparison.Ordinal))return "Beatmap.LoadCore: spinner end syntax/duration engineering guard";
 return "Other production rejection; type="+e.GetType().Name;
}
foreach(var c in cases.EnumerateArray())
{
 string id=c.GetProperty("id").GetString()!,path=Path.Combine(folder,c.GetProperty("path").GetString()!);int version=c.GetProperty("version").GetInt32(),mode=c.GetProperty("mode").GetInt32();
 byte[] before=File.ReadAllBytes(path);string hash=Convert.ToHexString(SHA256.HashData(before));Check(hash.Equals(c.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),id+" immutable source hash");
 var chart=LocalChartCatalog.Index(path);string? site=null,type=null;Beatmap? map=null;
 try{map=chart.LoadCanonicalGameplay();}catch(Exception e)when(e is InvalidDataException or FormatException or OverflowException){site=FailureSite(e);type=e.GetType().Name;}
 bool expected=c.GetProperty("expected").GetProperty("currentCanonicalSuccess").GetBoolean();Check((map is not null)==expected,id+" current canonical disposition");
 if(version is <5 or >14)Check(site=="Beatmap.LoadCore: outer format <5 or >14 guard",id+" exact outer format site");
 else if(id.Contains("fractional",StringComparison.Ordinal))Check(site=="Beatmap.LoadCore: row Int32 time/type syntax gate",id+" fractional start stops before spinner guard");
 else if(version<14)Check(site=="Beatmap.LoadCore: spinner version !=14 guard",id+" exact spinner-only format site (including mode-before-final-check)");
 else if(mode!=0)Check(site=="Beatmap.LoadCore: standard-mode guard",id+" actual standard mode site");
 else if(!expected)Check(site=="Beatmap.LoadCore: spinner end syntax/duration engineering guard",id+" actual malformed end site");
 object? prepared=null;
 if(map is not null)
 {
  var spinner=map.Spinners.Single();var state=new SpinnerState(spinner,map.OverallDifficulty);var score=ScoreConfiguration.FromBeatmap(map);var session=CircleSession.CreateFreshOrdinary(map,score,false);var health=HealthChart.FromMap(map,session.Approach,HealthPreparationPolicy.E036FreshOrdinary);
  var e=c.GetProperty("expected");Check(spinner.StartTime==e.GetProperty("start").GetInt32()&&spinner.EndTime==e.GetProperty("end").GetInt32(),id+" prepared start/end");
  Check(state.Required==e.GetProperty("requiredAtOD5").GetInt32(),id+" E026 requirement");
  Check(map.HealthSource!.IsFreshFor(map)&&map.CircleSounds!.Spinners.Any(s=>ReferenceEquals(s.Object,spinner)),id+" actual sample/health reference ownership");
  Check(health.NewComboProvenance.Select(p=>p.FinalFlag).SequenceEqual(e.GetProperty("finalNoBreakFlags").EnumerateArray().Select(x=>x.GetBoolean())),id+" real prepared no-break NewCombo");
  var payload=(map.CircleSounds ?? throw new InvalidOperationException("missing prepared sound sidecar")).Spinners.Single().Payload;var expectedSample=e.GetProperty("storedSample");
  Check(payload.BaseSet==expectedSample.GetProperty("base").GetInt32()&&payload.AdditionSet==expectedSample.GetProperty("addition").GetInt32()&&payload.Index==expectedSample.GetProperty("index").GetInt32()&&payload.Volume==expectedSample.GetProperty("volume").GetInt32()&&payload.Filename==expectedSample.GetProperty("filename").GetString()&&payload.StoredMask==2&&payload.Object.StartTime==spinner.EndTime,id+" actual end-time sample/raw sound contract");
  prepared=new{spinner.StartTime,spinner.EndTime,state.Required,state.Acceleration,healthFlags=health.NewComboProvenance.Select(p=>p.FinalFlag),scoreK=score.K,sessionObjects=session.Heads.Count+session.Spinners.Count};
 }
 // Independently exercise the existing version-aware NewCombo component even
 // when canonical load stops earlier. This is NOT successful old-map preparation.
 object? component=null;
 if(mode==0&&version is >=5 and <=14&&c.GetProperty("expected").TryGetProperty("parserNewCombo",out var fileBit))
 {
  bool mixed=c.GetProperty("mixed").GetBoolean();int raw=fileBit.GetBoolean()?12:8;
  var entries=mixed?new[]{new NewComboInput(new object(),1000,1,OriginalRegisteredType.Circle),new NewComboInput(new object(),2000,raw,OriginalRegisteredType.SpinnerBase),new NewComboInput(new object(),6000,1,OriginalRegisteredType.Circle),new NewComboInput(new object(),7000,6,OriginalRegisteredType.Slider)}:new[]{new NewComboInput(new object(),2000,raw,OriginalRegisteredType.SpinnerBase)};
  var flags=NewComboPreparation.Prepare(entries,Array.Empty<HealthBreak>(),version,NewComboContext.FreshZeroShift with { OriginalFactoryPriority=true });var s=flags.Single(p=>p.RawType==raw);
  Check(s.ConstructorFlag==(version<=8)&&s.ParserFlag==fileBit.GetBoolean(),id+" existing component preserves constructor/parser order");
  component=new{confidence="VERIFIED COMPONENT only; synthetic registered identities, not canonical old-map success",constructor=s.ConstructorFlag,parser=s.ParserFlag,flags=flags.Select(x=>x.FinalFlag)};
 }
 Check(File.ReadAllBytes(path).SequenceEqual(before),id+" source not written");
 results.Add(new{id,hash,version,mode,index=chart.Availability.ToString(),indexRule=chart.Error,indexedSpinnerCount=chart.Metadata?.Spinners,canonical=map is null?"rejected":"prepared",site,exceptionType=type,prepared,component});
}
var audit=AuditRunner.Run(Path.Combine(folder,"fixtures"));string auditJson=AuditRunner.Json(audit);Check(auditJson==AuditRunner.Json(AuditRunner.Run(Path.Combine(folder,"fixtures"))),"actual audit twice deterministic");
Check(audit.Complete&&audit.Charts.Count==cases.GetArrayLength(),"actual audit complete denominator");
var baseline=JsonDocument.Parse(File.ReadAllText(Path.Combine(folder,"production-baseline.json"))).RootElement.GetProperty("compiledSourceHashes");
Check(audit.SourceState=="clean"&&baseline.EnumerateObject().All(p=>audit.SourceHashes[p.Name]==p.Value.GetString()),"actual referenced canonical production clean and all baseline source bytes pinned");
Check(audit.SourceHashes["Beatmap.cs"]==Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,"reconstructed/CircleGame/Gameplay/Beatmap.cs")))),"compiled actual preparation source hash");
// Preserve actual error-site discriminator for declared unsupported formats,
// plus locate every literal production InvalidDataException involving 'format'.
string source=Path.Combine(root,"reconstructed/CircleGame/Gameplay/Beatmap.cs");var sites=File.ReadAllLines(source).Select((line,i)=>new{line=i+1,text=line.Trim()}).Where(x=>x.text.Contains("throw new InvalidDataException",StringComparison.Ordinal)&&x.text.Contains("format",StringComparison.OrdinalIgnoreCase)).ToArray();
Directory.CreateDirectory(output);File.WriteAllText(Path.Combine(output,"audit.json"),auditJson);File.WriteAllText(Path.Combine(output,"results.json"),JsonSerializer.Serialize(new{classification="Work1 actual production characterization, not original-runtime observations",exactTestedMain=audit.SourceCommit,productionBaselineSHA="782c00771a436b2a2950680d4f5de379a791d9dd",runtime=Environment.Version.ToString(),checks=checks.Count,pass=checks.Count,fail=0,cases=results,formatLiteralSites=sites,sourceHashes=new Dictionary<string,string>{{"Beatmap.cs",Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(source)))},{"LocalChartCatalog.cs",Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root,"reconstructed/CircleGame/Selection/LocalChartCatalog.cs"))))}},results=checks,originalRuntime="NOT RUN"},AuditRunner.JsonOptions)+"\n");
Console.WriteLine($"PASS {checks.Count} production assertions; {cases.GetArrayLength()} fixtures, {results.Count} characterized; original runtime NOT RUN");
