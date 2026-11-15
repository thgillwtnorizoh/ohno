#!/usr/bin/env python3
"""Exact-source, clean-build independent finite versioned spinner verification."""
import argparse, hashlib, json, subprocess, sys
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact_root');p.add_argument('output');a=p.parse_args()
here=Path(__file__).resolve().parent;repo=here.parents[3];exact=Path(a.exact_root).resolve();out=Path(a.output).resolve();out.mkdir(parents=True,exist_ok=True)
pin=json.loads((here/'expected-sources.json').read_text())
def run(*args):return subprocess.check_output([str(x) for x in args],text=True).strip()
def sha(path):return hashlib.sha256(path.read_bytes()).hexdigest()
assert run('git','-C',exact,'rev-parse','HEAD')==pin['source'],'wrong tested commit'
assert not run('git','-C',exact,'status','--porcelain'),'exact worktree must be clean'
for path in pin['paths']:
 assert sha(exact/path).upper()==pin['hashes'][Path(path).name],path
 assert run('git','-C',exact,'diff','HEAD','--',path)=='',path
fixture=repo/'re/campaigns/C001/versioned-spinners'
subprocess.run([sys.executable,str(fixture/'build_fixtures.py'),'--check'],check=True)
subprocess.run([sys.executable,str(fixture/'check.py'),str(fixture/'static-evidence.json')],check=True)
with (out/'build.log').open('w') as log:
 subprocess.run([a.dotnet,'build',str(exact/'reconstructed/CorpusAudit/CorpusAudit.csproj'),'-c','Release'],stdout=log,stderr=subprocess.STDOUT,check=True)
with (out/'final-run.log').open('w') as log:
 subprocess.run([a.dotnet,'run','--project',str(here/'Checks/Checks.csproj'),'-c','Release',f'-p:ExactRoot={exact}','--',str(repo),str(out)],stdout=log,stderr=subprocess.STDOUT,check=True)
result=json.loads((out/'results.json').read_text())
assert result['failed']==0 and result['passed']==548 and result['source']==pin['source'] and result['sourceState']=='clean'
dll=exact/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll'
assert sha(dll)==sha(here/'Checks/bin/Release/net8.0/CorpusAudit.dll'),'different compiled production linked'
assert not run('git','-C',exact,'status','--porcelain'),'source changed during verification'
seal=dict(source=pin['source'],state='clean',sdk=run(a.dotnet,'--version'),runtime=result['runtime'],platform=sys.platform,linkedHashes=pin['hashes'],binarySha256=sha(dll),checksAssemblySha256=sha(here/'Checks/bin/Release/net8.0/Checks.dll'),resultsSha256=sha(out/'results.json'),verifierHashes={str(f.relative_to(here)):sha(f) for f in (here/'verify.py',here/'Checks/Program.cs',here/'Checks/Checks.csproj',here/'expected-sources.json')},fixtureHashes={c['path']:sha(fixture/c['path']) for c in json.loads((fixture/'manifest.json').read_text())['cases']},notRun=['original runtime','audible output','physical input','owner corpus','host-length cap','full Windows regression rerun'])
(out/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n')
print('PASS exact clean source + nine linked hashes + 548 independent production assertions')
