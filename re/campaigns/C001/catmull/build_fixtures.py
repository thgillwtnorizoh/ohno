#!/usr/bin/env python3
"""Independent research mathematics; neither original execution nor Work2 code.
Fraction basis weights and scalar binary32 operation projection are deliberately
separate calculations. Native x86 evaluation-stack precision remains unknown.
"""
from fractions import Fraction as Q
from pathlib import Path
import hashlib,json,math,struct,sys
H=Path(__file__).resolve().parent
def s(x):return struct.unpack('<f',struct.pack('<f',x))[0]
def add(a,b):return tuple(s(x+y) for x,y in zip(a,b))
def sub(a,b):return tuple(s(x-y) for x,y in zip(a,b))
def mul(a,v):return tuple(s(x*v) for x in a)
def length(a,b):
 x,y=sub(b,a);return s(math.sqrt(s(s(x*x)+s(y*y))))
def rational(points,t):
 # Basis-weight derivation, independent of the recovered evaluation expression.
 weights=[(-t+2*t*t-t**3)/2,(2-5*t*t+3*t**3)/2,
          (t+4*t*t-3*t**3)/2,(-t*t+t**3)/2]
 return tuple(sum(w*Q(p[k]) for w,p in zip(weights,points)) for k in range(2))
def projected(points,t):
 t2=s(t*t);t3=s(t*t2)
 def coordinate(p0,p1,p2,p3):
  a=s(s(2*p1)+s(s(-p0+p2)*t))
  b=s(s(s(s(2*p0)-s(5*p1))+s(4*p2))-p3)
  c=s(s(s(-p0+s(3*p1))-s(3*p2))+p3)
  return s(.5*s(s(a+s(b*t2))+s(c*t3)))
 return tuple(coordinate(*(p[k] for p in points)) for k in range(2))
def raw_path(points):
 segments=[];exact=[];projection_error=0.
 for i in range(len(points)-1):
  p0=points[max(i-1,0)];p1=points[i];p2=points[i+1]
  p3=points[i+2] if i+2<len(points) else add(p2,sub(p2,p1))
  stencil=[p0,p1,p2,p3]
  for j in range(50):
   a=projected(stencil,s(j/50));b=projected(stencil,s((j+1)/50))
   segments.append(dict(start=a,end=b,isLinear=False,isSubcurveEnd=j==49))
   for n,p in [(j,a),(j+1,b)]:
    q=rational(stencil,Q(n,50));projection_error=max(projection_error,*(abs(float(x)-y) for x,y in zip(q,p)))
  exact.append(dict(span=i,stencil=stencil,midpoint=[str(x) for x in rational(stencil,Q(1,2))],start=[str(x) for x in rational(stencil,Q(0))],end=[str(x) for x in rational(stencil,Q(1))]))
 return segments,exact,projection_error
def prepare(raw,requested):
 segs=[dict(v) for v in raw];g=sum(length(v['start'],v['end']) for v in segs);excess=g-requested
 if g>0:
  while segs:
   v=segs[-1];a,b=v['start'],v['end'];d=length(a,b)
   if d<=excess+.0001:segs.pop();excess-=d;continue
   if a!=b:
    direction=sub(b,a);norm=mul(direction,s(1/length(a,b)))
    v['end']=add(a,mul(norm,s(d-s(excess))))
   break
 cumulative=[];total=0.
 for v in segs:total+=length(v['start'],v['end']);cumulative.append(total)
 return dict(unadjustedChordLength=g,preparedSegments=segs,cumulativeLengths=cumulative,adjustedLength=total,preparedEnd=segs[-1]['end'] if segs else None,preparedCollectionWritten=bool(segs))
CASES=[
 ('two-controls',[(100,100),(200,100)],100,{}),
 ('three-turn',[(100,100),(200,100),(200,200)],200,{}),
 ('nonuniform-spacing',[(100,100),(110,100),(210,110),(220,100)],240,{}),
 ('duplicate-interior',[(100,100),(200,100),(200,100),(200,200)],200,{}),
 ('collinear-backtrack',[(100,100),(104,100),(102,100)],8,{}),
 ('zero-tail-blocks-extension',[(100,100),(200,100),(200,100),(200,100)],300,{}),
 ('all-coincident',[(100,100)]*3,1,{}),
 ('singleton',[(100,100)],1,{}),
 ('bare-marker',[(100,100)],1,{'bare':True}),
 ('head-insertion',[(100,100),(200,100),(200,200)],200,{'omitHead':True}),
 ('fractional-controls',[(100,100),(101,100),(102,100)],2,{'tokens':['100:100','101.9:100.9','102.9:100.1']}),
 ('short-request',[(100,100),(200,100),(200,200)],10,{}),
 ('long-request',[(100,100),(200,100),(200,200)],300,{}),
 ('vanishing-request',[(100,100),(200,100),(200,200)],.00005,{}),
 ('version-5',[(100,100),(200,100),(200,200)],200,{'version':5}),
 ('version-9',[(100,100),(200,100),(200,200)],200,{'version':9}),
 ('repeat-two',[(100,100),(200,100),(200,200)],200,{'repeat':2}),
 ('precision-diagnostic',[(0,100),(10000001,100),(10000003,100)],64,{'diagnostic':True}),
 ('malformed-coordinate',[(100,100)],4,{'tokens':['100:100','101'],'malformed':True}),
]
BASE='''osu file format v{version}

[General]
Mode:0
StackLeniency:0
[Metadata]
Title:C001 C {id}
Artist:Independent mathematical control
Creator:Project ohno Work 1
Version:Catmull research
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
{x},{y},1000,2,0,{curve},{repeat},{requested}
'''
def build(check=False):
 rows=[]
 for ident,points,requested,opts in CASES:
  tokens=opts.get('tokens') or [f'{x}:{y}' for x,y in points[(1 if opts.get('omitHead') else 0):]]
  curve='C' if opts.get('bare') else 'C|'+'|'.join(tokens)
  data=BASE.format(id=ident,version=opts.get('version',14),x=points[0][0],y=points[0][1],curve=curve,repeat=opts.get('repeat',1),requested=requested).encode()
  p=H/'fixtures'/(ident+'.osu')
  if check:assert p.read_bytes()==data,p
  else:p.parent.mkdir(exist_ok=True);p.write_bytes(data)
  if opts.get('malformed'):
   expected=dict(branch='row-error',site='M28f7:1775 missing Y cell; ordinary caller rethrows at1d03',geometry='NOT PRODUCED; original CLR exception identity NOT VERIFIED')
  else:
   raw,exact,error=raw_path(points)
   expected=dict(branch='Catmull',storedPreparedKind=0,normalizedControls=points,rawSegments=raw,rawSegmentCount=len(raw),exactRationalSpans=exact,maxRationalVsProjectionCoordinateDifference=error,**prepare(raw,requested))
  rows.append(dict(id=ident,path='fixtures/'+p.name,sha256=hashlib.sha256(data).hexdigest(),version=opts.get('version',14),repeats=opts.get('repeat',1),requested=requested,expected=expected,profile='diagnostic precision only' if opts.get('diagnostic') else 'finite geometry prediction; downstream zero/empty scheduling excluded',confidence='RECONSTRUCTED from CONFIRMED ORIGINAL static evidence; projected arithmetic is ENGINEERING',originalRuntime='NOT RUN',production='NOT EXECUTED; Work2 C implementation pending'))
 out=dict(schema=1,source='Independent Fraction basis weights and separately evaluated recovered polynomial',arithmetic='Every scalar operation projected to binary32; Python binary64 sqrt rounded to Single; original x86 CLR stack precision NOT VERIFIED',coordinateComparisonTolerance=.0001,lengthComparisonTolerance=.0001,toleranceScope='ENGINEERING for ordinary small finite coordinates only; precision-diagnostic has no original-runtime numerical oracle',cases=rows)
 data=(json.dumps(out,indent=2,sort_keys=True)+'\n').encode();p=H/'manifest.json'
 if check:assert p.read_bytes()==data,p
 else:p.write_bytes(data)
 print('PASS',len(rows),'hashed C fixtures; exact rational and explicit binary32 predictions')
if __name__=='__main__':build('--check' in sys.argv)
