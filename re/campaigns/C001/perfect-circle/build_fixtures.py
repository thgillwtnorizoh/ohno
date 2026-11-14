#!/usr/bin/env python3
"""Finite mathematical controls, not original execution or a production parser.
Exact rational circumcenters independently check the declared simple examples.
Projected vertices use explicit binary32 stores + Python binary64 math; original
x86 evaluation/trig precision is NOT established by these calculations.
"""
from pathlib import Path
from fractions import Fraction as Q
import hashlib,json,math,struct,sys
HERE=Path(__file__).resolve().parent
TAU=6.2831854820251465  # Actual Double literal; not Python 2*pi.
def s(v):return struct.unpack('<f',struct.pack('<f',v))[0]
def add(a,b):return tuple(s(x+y) for x,y in zip(a,b))
def sub(a,b):return tuple(s(x-y) for x,y in zip(a,b))
def mul(a,v):return tuple(s(x*v) for x in a)
def length(a,b):
 x,y=sub(a,b);return s(math.sqrt(s(s(x*x)+s(y*y))))
def center_exact(points):
 (ax,ay),(bx,by),(cx,cy)=[tuple(Q(x) for x in p) for p in points]
 d=2*(ax*(by-cy)+bx*(cy-ay)+cx*(ay-by))
 if d==0:return None
 aa=ax*ax+ay*ay;bb=bx*bx+by*by;cc=cx*cx+cy*cy
 return ((aa*(by-cy)+bb*(cy-ay)+cc*(ay-by))/d,(aa*(cx-bx)+bb*(ax-cx)+cc*(bx-ax))/d)
def tail_adjust(vertices,requested):
 segs=[[tuple(a),tuple(b)] for a,b in zip(vertices,vertices[1:])];g=sum(length(a,b) for a,b in segs)
 excess=g-requested
 if g>0:
  while segs:
   a,b=segs[-1];d=length(a,b)
   if d<=excess+.0001:segs.pop();excess-=d;continue
   if a!=b:
    v=sub(b,a);norm=mul(v,s(1/s(math.sqrt(s(s(v[0]*v[0])+s(v[1]*v[1]))))))
    segs[-1][1]=add(a,mul(norm,s(d-s(excess))))
   break
 cumulative=[];total=0.
 for a,b in segs:total+=length(a,b);cumulative.append(total)
 return dict(unadjustedChordLength=g,preparedSegments=segs,cumulativeLengths=cumulative,adjustedLength=total,preparedEnd=segs[-1][1] if segs else None,preparedCollectionWritten=bool(segs),finalSchedule='Shared E005/E016/E021, not separately emulated here')
BASE='''osu file format v{version}

[General]
Mode:0
StackLeniency:0
[Metadata]
Title:C001 P {id}
Artist:Independent mathematical control
Creator:Project ohno Work 1
Version:Perfect circle research
[Difficulty]
HPDrainRate:5
CircleSize:5
OverallDifficulty:5
ApproachRate:5
SliderMultiplier:1
SliderTickRate:1
[TimingPoints]
0,500,4,1,0,100,1,0
[HitObjects]
{headx},{heady},1000,2,0,P|{path},{repeats},{length}
'''
CASES=[]
def arc(id,points,requested,center,n,version=14,repeats=1):
 assert tuple(center_exact(points))==tuple(Q(x) for x in center)
 r=length(center,points[0]);a,b,c=[math.atan2(s(p[1]-center[1]),s(p[0]-center[0])) for p in points]
 while b<a:b+=TAU
 while c<a:c+=TAU
 if b>c:c-=TAU
 analytic=abs((c-a)*r);assert int(analytic*.125)==n,(id,analytic,n)
 vertices=[tuple(points[0])]
 for i in range(1,n):
  t=i/n;theta=c*t+a*(1-t)
  vertices.append(add(center,(s(math.cos(theta)*r),s(math.sin(theta)*r))))
 vertices.append(tuple(points[2]))  # Original exact final control, not reevaluation.
 expect=dict(branch='Perfect-three',storedPreparedKind=3,exactCenter=[str(x) for x in center_exact(points)],center=center,radius=r,startAngle=a,endAngle=c,sweep=c-a,angleWrapLiteral=TAU,analyticLengthUsedOnlyForSampling=analytic,samplingInteger=n,rawSegmentCount=max(1,n),rawVertices=vertices,segmentFlags={'IsLinear':False,'IsSubcurveEnd':False},**tail_adjust(vertices,requested))
 CASES.append(dict(id=id,points=points,requested=requested,version=version,repeats=repeats,expected=expect))
def linear(id,points,requested,source=None):
 expect=dict(branch='Linear-fallback',storedPreparedKind=2 if len(points)<3 else 3,normalizedControls=points,rawVertices=points,rawSegmentCount=max(0,len(points)-1),segmentFlags={'IsLinear':True,'IsSubcurveEnd':True},**tail_adjust(points,requested))
 CASES.append(dict(id=id,points=points,source=source,requested=requested,version=14,repeats=1,expected=expect))
def cases():
 arc('semicircle-positive',[(132,100),(100,132),(68,100)],100,(100,100),12)
 arc('semicircle-negative',[(132,100),(100,68),(68,100)],100,(100,100),12)
 arc('major-negative',[(106,100),(100,94),(100,106)],26,(100,100),3)
 arc('sampling-zero',[(102,100),(100,102),(98,100)],4,(100,100),0)
 arc('sampling-one',[(103,100),(100,103),(97,100)],6,(100,100),1)
 arc('sampling-two-trim',[(106,100),(100,106),(94,100)],12,(100,100),2)
 arc('chord-extension',[(106,100),(100,106),(94,100)],24,(100,100),2)
 arc('vanishing-length',[(106,100),(100,106),(94,100)],.00005,(100,100),2)
 arc('near-collinear',[(100,100),(200,100),(300,101)],200,(150,10100.5),25)
 for v in [5,9]:arc(f'version-{v}-arc',[(106,100),(100,106),(94,100)],12,(100,100),2,version=v)
 arc('repeat-two',[(106,100),(100,106),(94,100)],12,(100,100),2,repeats=2)
 linear('collinear-backtrack',[(100,100),(104,100),(102,100)],8)
 linear('closed-degenerate',[(106,100),(100,106),(106,100)],12)
 linear('all-coincident',[(100,100)]*3,1)
 linear('two-controls',[(100,100),(104,100)],12)
 linear('singleton',[(100,100)],1)
 linear('fractional-collinear',[(100,100),(101,100),(102,100)],2,source=['100:100','101.9:100.9','102.9:100.1'])
 # E032 published cubic leaf controls, translated by exactly(100,100).
 vertices=[(100,100),(100.5,100.421875),(101,100.671875),(101.5,100.75),(102,100.671875),(102.5,100.421875),(103,100)]
 pts=[(100,100),(101,101),(102,101),(103,100)]
 CASES.append(dict(id='four-controls-modern',points=pts,requested=4,version=14,repeats=1,expected=dict(branch='Bezier-fallback',storedPreparedKind=1,helper='M2d62, E032 cubic',rawVertices=vertices,rawSegmentCount=6,**tail_adjust(vertices,4))))
 CASES.append(dict(id='four-controls-v9-dispatch',points=pts,requested=4,version=9,repeats=1,expected=dict(branch='Bezier-fallback',storedPreparedKind=1,helper='M2d64, inherited E032 only',uniformSampleCount=200,lastParameter='Single(199)/Single(200)',preparedGeometry='NOT VERIFIED by this finite fixture; compatibility dispatch only')))
 CASES.append(dict(id='malformed-coordinate',points=[(100,100)],source=['100:100','101'],requested=4,version=14,repeats=1,expected=dict(branch='runtime-row-error',site='M28f7:1775 reads missing Y cell after successful X parse; token lacks colon',normalCaller='row error rethrown; validation/skip caller can skip row',preparedGeometry='NOT PRODUCED; CLR exception identity NOT VERIFIED RUNTIME')))
def build(check=False):
 cases();rows=[]
 for case in CASES:
  points=case['points'];tokens=case.get('source') or [f'{x}:{y}' for x,y in points]
  data=BASE.format(id=case['id'],headx=points[0][0],heady=points[0][1],version=case['version'],path='|'.join(tokens),repeats=case['repeats'],length=case['requested']).encode()
  path=HERE/'fixtures'/(case['id']+'.osu')
  if check:assert path.read_bytes()==data,path
  else:path.parent.mkdir(exist_ok=True);path.write_bytes(data)
  rows.append(dict(id=case['id'],path='fixtures/'+path.name,sha256=hashlib.sha256(data).hexdigest(),version=case['version'],repeats=case['repeats'],requested=case['requested'],expected=case['expected'],classification='RECONSTRUCTED finite mathematical prediction from CONFIRMED ORIGINAL static sites',originalRuntime='NOT RUN',production='P path currently rejected; Work2 implementation pending'))
 out=dict(schema=1,source='build_fixtures.py; exact rational centers and independent finite math',arithmetic='Projected binary32 stores, Python binary64 atan2/sin/cos/sqrt; original x86 intermediate/JIT/transcendental precision NOT VERIFIED',coordinateComparisonTolerance=.0001,lengthComparisonTolerance=.0001,cases=rows)
 data=(json.dumps(out,indent=2,sort_keys=True)+'\n').encode();p=HERE/'manifest.json'
 if check:assert p.read_bytes()==data,p
 else:p.write_bytes(data)
 print('PASS',len(rows),'exact hashed P fixtures; rational centers and declared sampling integers checked')
if __name__=='__main__':build('--check' in sys.argv)
