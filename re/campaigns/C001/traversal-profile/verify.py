#!/usr/bin/env python3
import argparse,hashlib,json,subprocess,os
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact');p.add_argument('output');a=p.parse_args();H=Path(__file__).resolve().parent;R=H.parents[3];E=Path(a.exact).resolve();O=Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True)
def run(*xs):return subprocess.check_output([str(x) for x in xs],text=True).strip()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
pin=json.loads((H/'expected-sources.json').read_text());assert run('git','-C',E,'rev-parse','HEAD')==pin['source'] and not run('git','-C',E,'status','--porcelain')
for f in pin['paths']:assert sha(E/f).upper()==pin['hashes'][Path(f).name]
subprocess.run(['python',H/'build_fixtures.py','--check'],check=True)
with (O/'build.log').open('w') as f:subprocess.run([a.dotnet,'build',E/'reconstructed/CorpusAudit/CorpusAudit.csproj','-c','Release'],stdout=f,stderr=subprocess.STDOUT,check=True)
with (O/'production.log').open('w') as f:subprocess.run([a.dotnet,'run','--project',H/'Checks','-c','Release',f'-p:ExactRoot={E}','--',R,O],stdout=f,stderr=subprocess.STDOUT,check=True)
r=json.loads((O/'results.json').read_text());assert (r['passed'],r['failed'],r['admitted'],r['rejected'],r['componentProbes'])==(6786,0,41,65,94);assert r['source']==pin['source'] and r['state']=='clean'
dll=E/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll';assert sha(dll)==sha(H/'Checks/bin/Release/net8.0/CorpusAudit.dll')
assert not run('git','-C',E,'status','--porcelain')
seal=dict(source=pin['source'],startingMain='6e31cb63a8901ac7522e91dae386afcc2825f571',sourceState='clean',sdk=run(a.dotnet,'--version'),runtime=r['runtime'],sourceHashes=pin['hashes'],binarySha256=sha(dll),checksBinarySha256=sha(H/'Checks/bin/Release/net8.0/Checks.dll'),packetHashes={str(p.relative_to(H)):sha(p) for p in [H/'verify.py',H/'build_fixtures.py',H/'Checks/Program.cs',H/'Checks/Checks.csproj',H/'expected-sources.json']},resultsSha256=sha(O/'results.json'),passed=6786,failed=0,originalRuntime='NOT RUN',ownerCharts='NOT ACCESSED',Windows='NOT RUN; no CI run attributed to this research')
(O/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n');print('PASS6786 independent checks;106 fixtures41/65;94 isolated components;exact clean source and linked DLL')
