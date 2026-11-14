#!/usr/bin/env python3
"""Independent CLI diagnostic probe. Returns1 for the known taxonomy mismatch."""
import argparse,hashlib,json,subprocess,tempfile
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('assembly');p.add_argument('output');a=p.parse_args()
H=Path(__file__).resolve().parent;checks=[]
def sha(b):return hashlib.sha256(b).hexdigest()
def check(name,ok):checks.append(dict(name=name,pass_=bool(ok)))
with tempfile.TemporaryDirectory(prefix='audit-independent-') as tmp:
 root=Path(tmp)/'input';root.mkdir()
 for path in (H/'fixtures').glob('*.osu'):(root/path.name).write_bytes(path.read_bytes())
 before={x.name:sha(x.read_bytes()) for x in root.iterdir()}
 def run(private=False):
  result=subprocess.run([a.dotnet,a.assembly,str(root)]+(['--include-relative-paths'] if private else []),capture_output=True,text=True,check=True)
  return result.stdout,json.loads(result.stdout)
 raw,report=run();again,second=run();_,private=run(True)
 check('exact clean source identity',report['sourceCommit']=='8c5ba835f260401a56dea27367c6a6e9c55e9d84' and report['sourceState']=='clean')
 # Match rows via published relative-path hash contract, not traversal assumptions.
 def row(name):return next(x for x in report['charts'] if x['entryKey']==sha(name.encode()).upper())
 check('valid control canonical success',row('control.osu')['canonical']['status']=='success')
 check('ordinary numeric error is malformed',row('malformed.osu')['canonical']['status']=='malformed-input')
 check('chart-supplied phrase cannot impersonate mutation',row('spoof.osu')['canonical']['status']=='malformed-input')
 check('exact bytes never changed',before=={x.name:sha(x.read_bytes()) for x in root.iterdir()})
 check('read hashes match unchanged bytes',all(row(n)['sha256'].lower()==h for n,h in before.items()))
 check('default privacy canaries absent',all(s not in raw for s in ['control.osu','spoof.osu','malformed.osu','synthetic diagnostic probe','changed since indexing','not-a-number',str(root)]))
 check('repeat default JSON byte-identical',raw==again)
 check('private paths opt-in only',all('relativePath' not in x for x in report['charts']) and {x['relativePath'] for x in private['charts']}==set(before))
 check('complete finite scan and stage denominators',report['complete'] and len(report['charts'])==3 and all(sum(counts.values())==3 for counts in report['stageOutcomes'].values()))
 packet=dict(source=report['sourceCommit'],sourceState=report['sourceState'],sourceHashes=report['sourceHashes'],checks=checks,passed=sum(x['pass_'] for x in checks),failed=sum(not x['pass_'] for x in checks),fixtureHashes=before,complete=report['complete'],observations={n:dict(read=row(n)['read'],index=row(n)['index'],canonical=row(n)['canonical']) for n in before},defaultReport=report,classification='Work1 independent tool verification; reproducible diagnostic mismatch; not compatibility statistics',originalRuntime='NOT RUN')
 Path(a.output).write_text(json.dumps(packet,indent=2)+'\n')
 print(f"Independent probes: {packet['passed']}/{len(checks)} PASS; {packet['failed']} discrepancy")
raise SystemExit(1 if packet['failed'] else 0)
