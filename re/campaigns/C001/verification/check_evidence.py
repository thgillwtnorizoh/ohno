#!/usr/bin/env python3
"""Verify C001 audit provenance without upgrading failures to passes."""
import base64,gzip,hashlib,json,subprocess,sys
from pathlib import Path
HERE=Path(__file__).resolve().parent;ROOT=HERE.parents[3];C=HERE.parent
sha=lambda b:hashlib.sha256(b).hexdigest()
base=json.loads((HERE/'baseline.json').read_bytes())
j=(ROOT/'re/contexts.md').read_bytes();assert sha(j[:base['journal_bytes']])==base['journal_sha256']
# Work2's published changes are an explicit separate ownership set, never silently
# exempt all production paths or weaken historical evidence checks.
w2='80f7ac1a2ead36f88887c33b7e60ae7feccdfbe3'
changed=set(subprocess.check_output(['git','diff','--name-only',base['base'],w2],cwd=ROOT,text=True).splitlines())
for p,h in base['protected'].items():
 if p in changed:
  expected=subprocess.check_output(['git','show',w2+':'+p],cwd=ROOT)
  assert (ROOT/p).read_bytes()==expected,p
 else:assert sha((ROOT/p).read_bytes())==h,p
print('PASS preserved historical files; exact concurrent Work2 source pinned',w2)
subprocess.run([sys.executable,str(C/'corpus/build.py'),'--check'],cwd=ROOT,check=True)
for name in ('baseline-production-results.json','production-results.json'):
 p=json.loads((HERE/name).read_bytes());assert (p['fixtures'],p['checks'],p['matches'],p['gaps'],p['engineeringControls'])==(49,106,62,44,4)
 assert not p['originalExecuted'] and p['humanQualification']=='PENDING'
 assert sum(len(x['mismatches']) for x in p['results'])==44
 for r in p['results']:
  expected=next(x for x in json.loads((C/'corpus/manifest.json').read_bytes())['cases'] if x['id']==r['id'])
  assert r['sha256']==expected['sha256']
 # Current run must bind the actual published C001 production files.
 if name=='production-results.json':
  for path,h in p['production'].items():assert sha(subprocess.check_output(['git','show',p['source']+':'+path],cwd=ROOT))==h,path
print('PASS exact fixture and source provenance; conformance remains FAILED44, engineering PASS4')
p=json.loads((HERE/'windows-initial.provenance.json').read_bytes())
b=gzip.decompress(base64.b64decode((HERE/'windows-initial.log.gz.b64').read_bytes()))
assert len(b)==p['returned_log_bytes'] and sha(b)==p['returned_log_sha256'];text=b.decode()
assert 'FAIL F01 v13 Bezier rejected' in text and 'C001: corpus hash v05-bezier.osu' in text
job=json.loads((HERE/'windows-initial.jobs.json').read_bytes())['jobs'][0]
assert job['id']==p['job'] and job['conclusion']=='failure'
assert [s['name'] for s in job['steps'] if s['conclusion']=='failure']==p['failed_steps']
print('PASS preserved failed Windows gate, source',p['source'],'run',p['run'],'job',p['job'])
print('C001 OPEN; original runtime NOT RUN; no historical failure waived')
