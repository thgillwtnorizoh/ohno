#!/usr/bin/env python3
"""Check static recovery and finite discriminators without Work2/original code."""
from pathlib import Path
from fractions import Fraction as Q
import hashlib,json,subprocess,sys
import build_fixtures as f
H=Path(__file__).resolve().parent
j=json.loads(Path(sys.argv[1]).read_bytes())
assert j['assertionCount']==61 and len(j['methods'])==8
assert j['historicalIdentityChecks']==13 and j['inheritedCLiteral']['text']=='C'
assert not j['originalExecuted'];m=j['methods']
assert [0xb4,'switch',[0xce,0x1f8,0x7a2,0x56a]] in m['M144a']['branches']
c=[t for t in m['M144a']['token_sites'] if 0xce<=t[0]<0x1f8]
assert not any(t[2] in ['0x4000fd1','0x4000a35'] for t in c)
assert len([t for t in c if t[2]=='0x6004ee0'])==2
assert len([t for t in c if t[1]=='stfld' and t[2]=='0x4000a34'])==1
assert not any(t[1] in ['call','callvirt'] for t in m['M4ee0']['token_sites'])
assert not any(t[1]=='conv.r8' for t in m['M4ee0']['arithmetic'])
assert all(t[1]!='ldc.r8' for t in m['M4ee0']['constants'])
assert [x[2] for x in m['M110c']['token_sites'] if x[1]=='stfld']==['0x4000a32','0x4000a33']
subprocess.run([sys.executable,str(H/'build_fixtures.py'),'--check'],check=True)
j=json.loads((H/'manifest.json').read_bytes());cases={c['id']:c for c in j['cases']}
assert len(cases)==19 and len(list((H/'fixtures').glob('*.osu')))==19
for c in cases.values():
 assert hashlib.sha256((H/c['path']).read_bytes()).hexdigest()==c['sha256']
 assert c['originalRuntime']=='NOT RUN'
 e=c['expected']
 if e['branch']!='Catmull':continue
 points=e['normalizedControls'];raw=e['rawSegments'];assert len(raw)==50*(len(points)-1)
 assert all(v['isSubcurveEnd']==(i%50==49) and not v['isLinear'] for i,v in enumerate(raw))
 for v in e['exactRationalSpans']:
  i=v['span'];assert v['start']==[str(Q(x)) for x in points[i]]
  assert v['end']==[str(Q(x)) for x in points[i+1]]
 if c['id']!='precision-diagnostic':assert e['maxRationalVsProjectionCoordinateDifference']<.0001
 assert len(e['cumulativeLengths'])==len(e['preparedSegments'])
 for n in range(1,len(e['cumulativeLengths'])):assert e['cumulativeLengths'][n]>=e['cumulativeLengths'][n-1]
def E(id):return cases[id]['expected']
assert E('two-controls')['exactRationalSpans'][0]['midpoint']==['575/4','100']
assert E('nonuniform-spacing')['exactRationalSpans'][1]['midpoint']==['160','845/8']
assert E('duplicate-interior')['exactRationalSpans'][1]['midpoint']==['825/4','375/4']
assert E('duplicate-interior')['rawSegmentCount']==150
assert E('collinear-backtrack')['unadjustedChordLength']>6 # overshoot, unlike linear
assert E('zero-tail-blocks-extension')['preparedEnd']==[200,100]
assert E('zero-tail-blocks-extension')['adjustedLength']<300
assert E('all-coincident')['rawSegmentCount']==100 and E('all-coincident')['adjustedLength']==0
assert E('all-coincident')['preparedCollectionWritten']
for id in ['singleton','bare-marker','vanishing-request']:assert not E(id)['preparedCollectionWritten']
for id in ['version-5','version-9','repeat-two','head-insertion']:assert E(id)==E('three-turn')
assert E('short-request')['preparedEnd'][1]<100 # initial downward lobe
assert E('long-request')['preparedEnd'][0]<200 and E('long-request')['preparedEnd'][1]>200
p=E('precision-diagnostic')['rawSegments']
assert p[49]['end']==[10000000,100] and p[50]['start']==[10000001,100]
assert p[-1]['end']==[10000004,100] # native original arithmetic still NOT VERIFIED
assert E('malformed-coordinate')['branch']=='row-error'
# Exact independent polynomial identities; these do not invoke recovered ordering.
for stencil in [[(1,3),(5,7),(2,11),(13,17)],[(100,100)]*4]:
 assert f.rational(stencil,Q(0))==stencil[1]
 assert f.rational(stencil,Q(1))==stencil[2]
 for n in range(51):assert sum([(-Q(n,50)+2*Q(n,50)**2-Q(n,50)**3)/2,(2-5*Q(n,50)**2+3*Q(n,50)**3)/2,(Q(n,50)+4*Q(n,50)**2-3*Q(n,50)**3)/2,(-Q(n,50)**2+Q(n,50)**3)/2])==1
print('PASS 61 original-site assertions, 13 inherited hashes, 19 hashed fixtures and rational/projection/degenerate discriminators; original and production runtime NOT RUN')
