#!/usr/bin/env python3
"""Independent Issue17 finite literal charts and original-derived predictions."""
from pathlib import Path
import hashlib,json,sys
H=Path(__file__).resolve().parent
PATHS={'L':'L|428:192','B':'B|278:192|428:192','P':'P|278:342|428:192','C':'C|228:242|428:192'}
def cases():
 out=[]
 def add(id,n,kind='L',v=14,length='300',timing='0,500,4,1,0,100,1,0',edge='',sets='',payload=None,tick='1',extra=''):
  curve='L|4224:192' if id=='checkpoints-over' else PATHS[kind]
  row=f'128,192,2000,2,2,{curve},{n},{length}'
  if edge or sets or payload is not None:row+=','+edge+','+sets+('' if payload is None else ','+payload)
  raw=f'osu file format v{v}\n[General]\nMode:0\nStackLeniency:0\n[Metadata]\nTitle:Work1 Issue17 {id}\nCreator:Project ohno\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:{tick}\n[TimingPoints]\n{timing}\n[HitObjects]\n{row}\n{extra}'.encode()
  integer=isinstance(n,int);base=integer and 1<=n<=16
  reason='none' if base else 'repeat-guard'
  if base and id=='duration-over':base=False;reason='duration'
  if base and id=='checkpoints-over':base=False;reason='checkpoints'
  heads=None;times=None;end=None
  if integer and n>0 and kind=='L' and length=='300' and timing=='0,500,4,1,0,100,1,0' and tick=='1':
   end=2000+1500*n;times=[2000+500*j for j in range(1,3*n+1)];times[-1]=max(2000+(end-2000)//2,times[-1]-36);heads=[428,192] if n%2 else [128,192]
  out.append(dict(id=id,path='fixtures/'+id+'.osu',sha256=hashlib.sha256(raw).hexdigest(),version=v,kind=kind,rawTraversals=n,length=float(length),tickRate=float(tick),timing=timing,edge=edge,sets=sets,payload=payload,current=dict(prepared=base,guard=reason,index='Indexed'),original=dict(confidence='RECONSTRUCTED finite prediction from authenticated static sites; original not executed',effective=max(1,n) if integer else None,end=end,schedule=times,finalTraversalEndpoint=heads,largeCounts='9000/9001 excluded from executed fixtures'),probe=integer and 1<=n<=33 and id not in ['duration-over','checkpoints-over'],futureCandidate=integer and 1<=n<=32 and id not in ['duration-over','checkpoints-over'],raw=raw))
 for v in [5,6,14]:
  for kind in PATHS:
   for n in [15,16,17,24,32,33]:add(f'v{v:02}-{kind}-r{n}',n,kind,v)
 for kind in PATHS:
  for n in [1,2]:add(f'v14-{kind}-r{n}',n,kind)
 for n in [-1,0]:add('raw'+str(n),n)
 for label,n in [('fraction','17.5'),('text','oops'),('overflow','2147483648'),('empty','')]:add('syntax-'+label,n)
 for n in [16,17]:
  for label,edge,sets,payload in [('short','2|bad','1:2','0:0:0:100:'),('full','|'.join(str((i%4)*2) for i in range(n+1)),'|'.join('1:2' for i in range(n+1)),'0:0:0:100:'),('long','|'.join('2' for _ in range(n+3)),'|'.join('1:2' for _ in range(n+3)),None),('bad-set','2|4','1',None),('bad-payload','','','x')]:
   add(f'samples-{label}-r{n}',n,edge=edge,sets=sets,payload=payload)
 for n in [16,32]:
  add(f'inherited-r{n}',n,timing='0,500,4,1,0,100,1,0\n1000,-50,4,1,0,100,0,0')
  add(f'late-timing-r{n}',n,timing='0,500,4,1,0,100,1,0\n2100,250,4,2,1,37,1,0')
  add(f'fraction-duration-r{n}',n,length='100.25')
  add(f'nonmonotone-final-r{n}',n,length='206')
 add('duration-over',16,timing='0,500000,4,1,0,100,1,0')
 add('checkpoints-over',16,length='4096',tick='8')
 return out

def build(check=False):
 (H/'fixtures').mkdir(exist_ok=True);packet=[]
 for c in cases():
  raw=c.pop('raw');p=H/c['path']
  if check:assert p.read_bytes()==raw,p
  else:p.write_bytes(raw)
  packet.append(c)
 raw=(json.dumps(dict(authority='Work1 independent small traversal controls; no original runtime',cases=packet),indent=2)+'\n').encode()
 if check:assert (H/'manifest.json').read_bytes()==raw
 else:(H/'manifest.json').write_bytes(raw)
 print('PASS',len(packet),'immutable fixture hashes; current',sum(c['current']['prepared'] for c in packet),'preparations; no9000/9001 executions')
if __name__=='__main__':build('--check' in sys.argv)
