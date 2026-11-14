#!/usr/bin/env python3
"""Focused packet assertions, no game or production implementation executed."""
import hashlib,json,math,subprocess,sys
from pathlib import Path
H=Path(__file__).resolve().parent
j=json.loads(Path(sys.argv[1]).read_bytes());assert j['assertionCount']==73 and len(j['methods'])==15
assert j['inheritedPLiteral']['text']=='P' and j['historicalIdentityChecks']==20
assert not j['originalExecuted'];m=j['methods']
assert [0xb4,'switch',[0xce,0x1f8,0x7a2,0x56a]] in m['M144a']['branches']
assert not any(t[2] in ['0x4000fd1','0x4000a34','0x4000a35'] for t in m['M144a']['token_sites'] if 0x56a<=t[0]<0x7a2)
assert [0x10a,'ldc.r8',6.2831854820251465] in m['M2d6a']['constants']
assert j['externalMembers']['0xa0002d7']=='Atan2'
assert j['externalMembers']['0xa0004ca']=='Cos' and j['externalMembers']['0xa0004cb']=='Sin'
subprocess.run([sys.executable,str(H/'build_fixtures.py'),'--check'],check=True)
corpus=json.loads((H/'manifest.json').read_bytes());cases={c['id']:c for c in corpus['cases']}
assert len(cases)==21 and len(list((H/'fixtures').glob('*.osu')))==21
for c in cases.values():
 assert hashlib.sha256((H/c['path']).read_bytes()).hexdigest()==c['sha256']
 assert c['originalRuntime']=='NOT RUN'
 e=c['expected']
 if e['branch']=='Perfect-three':
  assert e['rawSegmentCount']==max(1,e['samplingInteger'])
  assert e['rawVertices'][-1] in [[68,100],[100,106],[98,100],[97,100],[94,100],[300,101]]
  assert len(e['rawVertices'])-1==e['rawSegmentCount']
  assert e['segmentFlags']=={'IsLinear':False,'IsSubcurveEnd':False}
for ident in ['sampling-zero','sampling-one']:
 assert cases[ident]['expected']['rawSegmentCount']==1
assert cases['sampling-two-trim']['expected']['rawSegmentCount']==2
pos=cases['semicircle-positive']['expected'];neg=cases['semicircle-negative']['expected']
assert pos['sweep']>0 and neg['sweep']<0
assert 1e-7<abs(neg['sweep'])-math.pi<3e-7 # actual wrap literal, not exact tau
assert cases['major-negative']['expected']['sweep']<-math.pi
assert cases['chord-extension']['expected']['preparedEnd'][1]<100
assert not cases['vanishing-length']['expected']['preparedCollectionWritten']
assert cases['collinear-backtrack']['expected']['preparedEnd']==[100,100]
assert cases['closed-degenerate']['expected']['branch']=='Linear-fallback'
assert cases['all-coincident']['expected']['unadjustedChordLength']==0
assert cases['all-coincident']['expected']['preparedCollectionWritten']
assert not cases['singleton']['expected']['preparedCollectionWritten']
assert cases['four-controls-v9-dispatch']['expected']['uniformSampleCount']==200
assert cases['four-controls-modern']['expected']['rawSegmentCount']==6
assert cases['fractional-collinear']['expected']['branch']=='Linear-fallback'
assert cases['malformed-coordinate']['expected']['branch']=='runtime-row-error'
for v in [5,9]:assert cases[f'version-{v}-arc']['expected']==cases['sampling-two-trim']['expected']
assert cases['repeat-two']['expected']==cases['sampling-two-trim']['expected']
# A separate binary32 scalar calculation checks the analytic rational centers
# chosen for fixtures. It does not call the original or Work2 geometry code.
import build_fixtures as f
f.cases()
for c in f.CASES:
 if c['expected']['branch']!='Perfect-three':continue
 a,b,z=c['points'];S=f.s
 norm=lambda p:S(S(p[0]*p[0])+S(p[1]*p[1]))
 D=S(2*S(S(S(a[0]*S(b[1]-z[1]))+S(b[0]*S(z[1]-a[1])))+S(z[0]*S(a[1]-b[1]))))
 X=S(S(S(S(norm(a)*S(b[1]-z[1]))+S(norm(b)*S(z[1]-a[1])))+S(norm(z)*S(a[1]-b[1])))/D)
 Y=S(S(S(S(norm(a)*S(z[0]-b[0]))+S(norm(b)*S(a[0]-z[0])))+S(norm(z)*S(b[0]-a[0])))/D)
 assert [X,Y]==list(c['expected']['center']),c['id']
print('PASS 73 original-static site assertions, 20 inherited identities, 21 byte fixtures and finite discriminators; original/production runtime NOT RUN')
