#!/usr/bin/env python3
import argparse,hashlib,json,subprocess,os
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact');p.add_argument('output');a=p.parse_args();H=Path(__file__).resolve().parent;R=H.parents[3];E=Path(a.exact).resolve();O=Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True)
def run(*xs):return subprocess.check_output([str(x) for x in xs],text=True).strip()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
pin=json.loads((H/'expected-sources.json').read_text());assert run('git','-C',E,'rev-parse','HEAD')==pin['source'] and not run('git','-C',E,'status','--porcelain')
for f in pin['paths']:assert sha(E/f).upper()==pin['hashes'][Path(f).name]
subprocess.run(['python',H/'review.py'],check=True)
with (O/'build.log').open('w') as f:subprocess.run([a.dotnet,'build',E/'reconstructed/CorpusAudit/CorpusAudit.csproj','-c','Release'],stdout=f,stderr=subprocess.STDOUT,check=True)
with (O/'production.log').open('w') as f:subprocess.run([a.dotnet,'run','--project',H/'Checks','-c','Release',f'-p:ExactRoot={E}','--',R,O],stdout=f,stderr=subprocess.STDOUT,check=True)
r=json.loads((O/'results.json').read_text());assert (r['passed'],r['failed'],r['prepared'],r['rejected'],r['zeroSettingConsumerControls'])==(1833,0,59,41,12);assert r['source']==pin['source'] and r['state']=='clean'
dll=E/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll';assert sha(dll)==sha(H/'Checks/bin/Release/net8.0/CorpusAudit.dll')
assert not run('git','-C',E,'status','--porcelain')
seal=dict(source=pin['source'],startingMain='1638dfd9f890e8286b578c74bbda32cb732eef4f',sourceState='clean',sdk=run(a.dotnet,'--version'),runtime=r['runtime'],sourceHashes=pin['hashes'],binarySha256=sha(dll),checksBinarySha256=sha(H/'Checks/bin/Release/net8.0/Checks.dll'),packetHashes={str(p.relative_to(H)):sha(p) for p in [H/'verify.py',H/'review.py',H/'Checks/Program.cs',H/'Checks/Extra.cs',H/'Checks/Checks.csproj',H/'expected-sources.json']},resultsSha256=sha(O/'results.json'),passed=1833,failed=0,originalRuntime='NOT RUN',ownerCharts='NOT ACCESSED',Windows='existing exact CI independently corroborated, not newly run')
(O/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n');print('PASS1833 independent checks, exact clean source and linked binaries;59/41,12 new,0 losses')
