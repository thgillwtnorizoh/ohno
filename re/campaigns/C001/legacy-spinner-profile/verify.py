#!/usr/bin/env python3
"""Reproduce bounded static/fixture consistency and exact-source characterization."""
import argparse,hashlib,json,subprocess,sys
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact_root');p.add_argument('output');p.add_argument('--archive');a=p.parse_args()
H=Path(__file__).resolve().parent;ROOT=H.parents[3];exact=Path(a.exact_root).resolve();out=Path(a.output).resolve();out.mkdir(parents=True,exist_ok=True)
pin=json.loads((H/'expected-sources.json').read_text())
def run(*args):return subprocess.check_output([str(x) for x in args],text=True).strip()
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
assert run('git','-C',exact,'rev-parse','HEAD')==pin['source']
assert not run('git','-C',exact,'status','--porcelain')
for path in pin['paths']:assert sha(exact/path).upper()==pin['hashes'][Path(path).name],path
subprocess.run([sys.executable,str(H/'build_fixtures.py'),'--check'],check=True)
subprocess.run([sys.executable,str(H/'check.py')],check=True)
if a.archive:
 subprocess.run([sys.executable,str(H/'recover.py'),a.archive,str(out/'recovered-static.json')],check=True)
 assert (out/'recovered-static.json').read_bytes()==(H/'static-evidence.json').read_bytes(),'different original recovery'
with (out/'build.log').open('w') as log:subprocess.run([a.dotnet,'build',str(exact/'reconstructed/CorpusAudit/CorpusAudit.csproj'),'-c','Release'],stdout=log,stderr=subprocess.STDOUT,check=True)
with (out/'production.log').open('w') as log:subprocess.run([a.dotnet,'run','--project',str(H/'Characterization/Characterization.csproj'),'-c','Release',f'-p:ExactRoot={exact}','--',str(ROOT),str(out)],stdout=log,stderr=subprocess.STDOUT,check=True)
r=json.loads((out/'results.json').read_text());assert (r['passed'],r['failed'],r['admissions'],r['rejections'],r['zeroControls'])==(775,0,26,55,40)
assert r['source']==pin['source'] and r['sourceState']=='clean'
dll=exact/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll'
assert sha(dll)==sha(H/'Characterization/bin/Release/net8.0/CorpusAudit.dll')
assert not run('git','-C',exact,'status','--porcelain')
files=['recover.py','build_fixtures.py','check.py','verify.py','expected-sources.json','manifest.json','static-evidence.json','Characterization/Program.cs','Characterization/Characterization.csproj']
seal=dict(source=pin['source'],state='clean',sdk=run(a.dotnet,'--version'),runtime=r['runtime'],platform=sys.platform,linkedHashes=pin['hashes'],productionBinarySha256=sha(dll),characterizationAssemblySha256=sha(H/'Characterization/bin/Release/net8.0/Characterization.dll'),sourcePacketHashes={f:sha(H/f) for f in files},resultHashes={n:sha(out/n) for n in ['results.json','audit.json']},originalExtraction='MATCHES pinned packet' if a.archive else 'NOT RERUN; packet consistency checked',originalRuntime='NOT RUN')
(out/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n')
print('PASS775 independent characterization assertions; 81 fixtures; 26 admissions/55 rejections; 40 diagnostic scalar controls')
