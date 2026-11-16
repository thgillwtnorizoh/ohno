#!/usr/bin/env python3
"""Exact-source independent new-profile verifier; preserves historical harnesses."""
import argparse,hashlib,json,subprocess,sys
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact_root');p.add_argument('output');a=p.parse_args()
H=Path(__file__).resolve().parent;ROOT=H.parents[3];exact=Path(a.exact_root).resolve();out=Path(a.output).resolve();out.mkdir(parents=True,exist_ok=True)
pin=json.loads((H/'expected-sources.json').read_text())
def cmd(*args):return subprocess.check_output([str(x) for x in args],text=True).strip()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
assert cmd('git','-C',exact,'rev-parse','HEAD')==pin['source']
assert not cmd('git','-C',exact,'status','--porcelain'),'exact source checkout must be clean'
for path in pin['paths']:assert sha(exact/path).upper()==pin['hashes'][Path(path).name],path
subprocess.run([sys.executable,str(H/'review.py')],check=True)
with (out/'build.log').open('w') as log:subprocess.run([a.dotnet,'build',str(exact/'reconstructed/CorpusAudit/CorpusAudit.csproj'),'-c','Release'],stdout=log,stderr=subprocess.STDOUT,check=True)
with (out/'production.log').open('w') as log:subprocess.run([a.dotnet,'run','--project',str(H/'Checks/Checks.csproj'),'-c','Release',f'-p:ExactRoot={exact}','--',str(ROOT),str(out)],stdout=log,stderr=subprocess.STDOUT,check=True)
r=json.loads((out/'results.json').read_text());assert r['failed']==0 and r['passed']==len(r['checks']) and all(c['pass'] for c in r['checks'])
assert r['admitted']==66 and r['excluded']==15 and r['prior33Admitted']==23
assert r['source']==pin['source'] and r['state']=='clean'
dll=exact/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll'
assert sha(dll)==sha(H/'Checks/bin/Release/net8.0/CorpusAudit.dll')
assert not cmd('git','-C',exact,'status','--porcelain')
files=['verify.py','review.py','Checks/Program.cs','Checks/Checks.csproj','expected-sources.json','numeric-expectations.json']
seal=dict(source=pin['source'],state='clean',sdk=cmd(a.dotnet,'--version'),runtime=r['runtime'],platform=sys.platform,linkedHashes=pin['hashes'],productionBinarySha256=sha(dll),checksAssemblySha256=sha(H/'Checks/bin/Release/net8.0/Checks.dll'),sourcePacketHashes={f:sha(H/f) for f in files},resultsSha256=sha(out/'results.json'),passed=r['passed'],failed=r['failed'],originalRuntime='NOT RUN',ownerCollection='NOT RUN')
(out/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n');print(f"PASS {r['passed']}/{len(r['checks'])} independent assertions; exact source and binaries;66/15 +23/10")
