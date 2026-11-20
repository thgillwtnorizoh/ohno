#!/usr/bin/env python3
"""Independent Issue11 exact-source verification; historical oracle stays immutable."""
from pathlib import Path
import argparse,subprocess,json,hashlib,sys
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact_root');p.add_argument('output');a=p.parse_args()
H=Path(__file__).resolve().parent;ROOT=H.parents[3];exact=Path(a.exact_root).resolve();out=Path(a.output).resolve();out.mkdir(parents=True,exist_ok=True);pin=json.loads((H/'expected-sources.json').read_text())
def run(*args):return subprocess.check_output([str(x) for x in args],text=True).strip()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
assert run('git','-C',exact,'rev-parse','HEAD')==pin['source'] and not run('git','-C',exact,'status','--porcelain')
for path in pin['paths']:assert sha(exact/path).upper()==pin['hashes'][Path(path).name],path
subprocess.run([sys.executable,str(H/'review.py')],check=True);subprocess.run([sys.executable,str(H/'build_fixtures.py'),'--check'],check=True)
with (out/'build.log').open('w') as log:subprocess.run([a.dotnet,'build',str(exact/'reconstructed/CorpusAudit/CorpusAudit.csproj'),'-c','Release'],stdout=log,stderr=subprocess.STDOUT,check=True)
dll=exact/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll'
# Expected historical failure is inspected explicitly; do not waive or edit its assertion.
old=subprocess.run([sys.executable,str(ROOT/'re/campaigns/C001/corpus-audit-qa/probe.py'),a.dotnet,str(dll),str(out/'historical-probe-replay.json')],capture_output=True,text=True)
r=json.loads((out/'historical-probe-replay.json').read_text());assert old.returncode==1 and (r['passed'],r['failed'])==(9,1) and [x['name'] for x in r['checks'] if not x['pass_']]==['exact clean source identity']
subprocess.run([sys.executable,str(H/'probe_current.py'),a.dotnet,str(dll),str(out/'profile-results.json')],check=True)
profile=json.loads((out/'profile-results.json').read_text());assert (profile['passed'],profile['failed'])==(10,0)
with (out/'production.log').open('w') as log:subprocess.run([a.dotnet,'run','--project',str(H/'Checks/Checks.csproj'),'-c','Release',f'-p:ExactRoot={exact}','--',str(H),str(out)],stdout=log,stderr=subprocess.STDOUT,check=True)
r=json.loads((out/'results.json').read_text());assert r['failed']==0 and r['passed']==len(r['checks']) and all(c['pass'] for c in r['checks'])
assert r['source']==pin['source'] and r['state']=='clean' and sha(dll)==sha(H/'Checks/bin/Release/net8.0/CorpusAudit.dll')
assert not run('git','-C',exact,'status','--porcelain')
files=['build_fixtures.py','manifest.json','expected-sources.json','review.py','verify.py','probe_current.py','Checks/Checks.csproj','Checks/Program.cs']
seal=dict(source=pin['source'],state='clean',sdk=run(a.dotnet,'--version'),runtime=r['runtime'],platform=sys.platform,sourceHashes=pin['hashes'],toolDllSha256=sha(dll),checksDllSha256=sha(H/'Checks/bin/Release/net8.0/CorpusAuditChecks.dll'),sourcePacketHashes={f:sha(H/f) for f in files},resultHashes={f:sha(out/f) for f in ['results.json','audit.json','profile-results.json','historical-probe-replay.json']},focusedPassed=r['passed'],focusedFailed=r['failed'],newProfilePassed=10,historicalProfile='9/10, exit1, exact obsolete source pin only; preserved',originalRuntime='NOT RUN',ownerCorpus='NOT RUN')
(out/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n');print(f"PASS {r['passed']}/{len(r['checks'])} independent focused assertions + new profile10/10; historical9/10 source-pin failure preserved")
