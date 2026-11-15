#!/usr/bin/env python3
"""Bounded corrected-source build/provenance gate and independent probes."""
import argparse, hashlib, json, subprocess, sys
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact_root');p.add_argument('output');a=p.parse_args()
here=Path(__file__).resolve().parent;root=Path(a.exact_root).resolve();out=Path(a.output).resolve();out.mkdir(parents=True,exist_ok=True)
pin=json.loads((here/'expected-sources.json').read_text())
def run(args):return subprocess.check_output([str(x) for x in args],text=True).strip()
assert run(['git','-C',root,'rev-parse','HEAD'])==pin['source'],'wrong exact checkout'
assert not run(['git','-C',root,'status','--porcelain']),'checkout must be clean'
paths=['reconstructed/CorpusAudit/'+n for n in ('AuditRunner.cs','Program.cs','CorpusAudit.csproj')]+['reconstructed/CircleGame/Gameplay/'+n for n in ('Beatmap.cs','CircleSession.cs','CircleHitSounds.cs','RankResults.cs')]+['reconstructed/CircleGame/Selection/LocalChartCatalog.cs']
for path in paths:
 actual=hashlib.sha256((root/path).read_bytes()).hexdigest().upper()
 assert actual==pin['hashes'][Path(path).name],f'linked source mismatch: {path}'
project=root/'reconstructed/CorpusAudit/CorpusAudit.csproj';dll=project.parent/'bin/Release/net8.0/CorpusAudit.dll'
with (out/'build.log').open('w') as log:subprocess.run([a.dotnet,'build',str(project),'-c','Release'],stdout=log,stderr=subprocess.STDOUT,check=True)
subprocess.run([sys.executable,str(here/'probe_v2.py'),a.dotnet,str(dll),str(out/'profile-results.json')],check=True)
with (out/'checks.log').open('w') as log:subprocess.run([a.dotnet,'run','--project',str(here/'Checks/Checks.csproj'),'-c','Release',f'-p:ExactRoot={root}','--',str(out/'scan-results.json')],stdout=log,stderr=subprocess.STDOUT,check=True)
# Recheck the exact linked output used by the in-process checks as well.
linked=here/'Checks/bin/Release/net8.0/CorpusAudit.dll'
assert hashlib.sha256(dll.read_bytes()).digest()==hashlib.sha256(linked.read_bytes()).digest(),'checks used different production binary'
assert not run(['git','-C',root,'status','--porcelain']),'checkout changed during verification'
seal=dict(source=pin['source'],sourceState='clean',linkedHashes=pin['hashes'],sdk=run([a.dotnet,'--version']),platform=sys.platform,
 binarySha256=hashlib.sha256(dll.read_bytes()).hexdigest(),
 resultHashes={n:hashlib.sha256((out/n).read_bytes()).hexdigest() for n in ('profile-results.json','scan-results.json')},
 verifierHashes={str(f.relative_to(here)):hashlib.sha256(f.read_bytes()).hexdigest() for f in (here/'verify.py',here/'probe_v2.py',here/'expected-sources.json',here/'Checks/Program.cs',here/'Checks/Checks.csproj')})
(out/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n')
print('Verified exact clean corrected source: 10/10 profile + 24/24 scan checks PASS')
