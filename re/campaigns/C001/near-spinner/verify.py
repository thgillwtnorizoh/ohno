#!/usr/bin/env python3
"""Source-pinned Issue13 characterization; never alters or executes original software."""
import argparse,hashlib,json,os,subprocess,sys
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact_root');p.add_argument('output');p.add_argument('--archive');a=p.parse_args()
H=Path(__file__).resolve().parent;R=H.parents[3];E=Path(a.exact_root).resolve();O=Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True)
def cmd(*xs):return subprocess.check_output([str(x) for x in xs],text=True).strip()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
def run(args,log):
 with (O/log).open('w') as f:subprocess.run([str(x) for x in args],stdout=f,stderr=subprocess.STDOUT,check=True,env={**os.environ,'DOTNET_CLI_TELEMETRY_OPTOUT':'1'})
pins=json.loads((H/'expected-sources.json').read_text());assert cmd('git','-C',E,'rev-parse','HEAD')==pins['source'];assert not cmd('git','-C',E,'status','--porcelain')
for path in pins['paths']:assert sha(E/path).upper()==pins['hashes'][Path(path).name],path
assert cmd('git','-C',E,'rev-parse','HEAD:reconstructed/CircleGame')==cmd('git','-C',E,'rev-parse','9ed9eb5c58f631f89fb66fc97802cfbf9ca86e44:reconstructed/CircleGame')=='2075c2a7dd9816e6e330db44da2682bf8b4fe9a8'
run([sys.executable,H/'build_fixtures.py','--check'],'fixtures.log')
run([sys.executable,H.parent/'legacy-spinner-profile/check.py'],'historical-preservation.log')
static=json.loads((H/'static-evidence.json').read_text());assert static['siteCount']==41 and len(static['directHeightUseCensus']['uses'])==6
if a.archive:
 run([sys.executable,H/'recover.py',a.archive,O/'recovered-static.json'],'recovery.log');assert (O/'recovered-static.json').read_bytes()==(H/'static-evidence.json').read_bytes()
run([a.dotnet,'build',E/'reconstructed/CorpusAudit/CorpusAudit.csproj','-c','Release'],'build.log')
run([a.dotnet,'run','--project',H/'Checks/Checks.csproj','-c','Release',f'-p:ExactRoot={E}','--',R,O],'production.log')
r=json.loads((O/'results.json').read_text());assert r['source']==pins['source'] and r['state']=='clean' and r['failed']==0 and r['passed']==len(r['checks'])
assert (r['fixtures'],r['prepared'],r['rejected'],r['zeroSettingConsumerControls'])==(100,47,53,12)
assert sha(E/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll')==sha(H/'Checks/bin/Release/net8.0/CorpusAudit.dll')
assert not cmd('git','-C',E,'status','--porcelain')
manifest=json.loads((H/'manifest.json').read_text())
# Literal matrix preserves expectations separately from actual outcomes.
import csv
with (O/'decision-matrix.csv').open('w',newline='') as f:
 w=csv.writer(f);w.writerow(['id','version','cs','ar','route','originalHeights','originalOrdinaryDisplacement','currentPrepared','handoffPrepared','reason'])
 for c in manifest['cases']:w.writerow([c['id'],c['version'],c['cs'],c['ar'],c['route'],json.dumps(c['original']['heights']),c['original']['displacement'],c['current']['prepared'],c['handoff']['prepared'],c['original']['note']])
packet=['build_fixtures.py','manifest.json','expected-sources.json','recover.py','static-evidence.json','Checks/Checks.csproj','Checks/Program.cs','verify.py']
seal=dict(source=pins['source'],sourceState='clean',gameplayAuthority='9ed9eb5c58f631f89fb66fc97802cfbf9ca86e44',gameplayTree='2075c2a7dd9816e6e330db44da2682bf8b4fe9a8',sdk=cmd(a.dotnet,'--version'),runtime=r['runtime'],platform=sys.platform,linkedSourceHashes=pins['hashes'],productionBinarySha256=sha(E/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll'),checksBinarySha256=sha(H/'Checks/bin/Release/net8.0/Checks.dll'),packetHashes={f:sha(H/f) for f in packet},resultsSha256=sha(O/'results.json'),matrixSha256=sha(O/'decision-matrix.csv'),fixtureHashes={c['id']:c['sha256'] for c in manifest['cases']},passed=r['passed'],failed=r['failed'],originalRuntime='NOT RUN',newWindows='NOT RUN',ownerCharts='NOT ACCESSED',handoffImplementation='NOT IMPLEMENTED')
(O/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n');print(f"PASS {r['passed']}/{len(r['checks'])};100 cases47/53;12 positive consumer controls; clean exact source/linkage; preserved81+33")
