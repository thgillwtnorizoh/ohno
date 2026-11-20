#!/usr/bin/env python3
"""New Issue13 independent literal cases. Never imports a production stack algorithm."""
import hashlib,json,sys
from pathlib import Path
H=Path(__file__).resolve().parent
STEP=3.2013118267059326

def cases():
 out=[]
 def add(name,v,rows,heights,shift=None,cs=5,ar=5,stack=None,current=None,future=None,guard=None,note='',columns=8,events='',general=''):
  # Shift indices/heights are explicit hand-derived decisions for these literal cases.
  shift=shift or {}; positions=[]; kinds=[]
  for i,row in enumerate(rows):
   a=row.split(',');kind='spinner' if int(a[3])&8 else 'slider' if int(a[3])&2 else 'circle';kinds.append(kind)
   x,y=(256.,192.) if kind=='spinner' else (float(a[0]),float(a[1]))
   if i in shift:x-=shift[i]*STEP;y-=shift[i]*STEP
   positions.append([x,y])
  route='M' if v>5 and cs==5 and ar==5 else 'Z'
  if current is None:current=route=='M' or stack=='0'
  if future is None:future=current
  if guard is None:guard='none' if current else 'certificate'
  raw=(f'osu file format v{v}\n[General]\nMode:0\n'+('' if stack is None else f'StackLeniency:{stack}\n')+general+f'[Metadata]\nTitle:Work1 Issue13 {name}\nCreator:Project ohno\n[Difficulty]\nHPDrainRate:5\nCircleSize:{cs}\nOverallDifficulty:5\nApproachRate:{ar}\nSliderMultiplier:1\nSliderTickRate:1\n[Events]\n'+events+'[TimingPoints]\n'+','.join(['0','500','4','1','0','100','1','0'][:columns])+'\n[HitObjects]\n'+'\n'.join(rows)+'\n').encode()
  out.append(dict(id=name,version=v,cs=cs,ar=ar,setting=.7 if stack is None else float(stack),route=route,rows=rows,kinds=kinds,original=dict(confidence='RECONSTRUCTED conditional original-static; not original runtime',heights=heights,preparedHeads=positions,displacement=bool(shift),note=note),current=dict(prepared=current,guard=guard),handoff=dict(prepared=future,extension=future and not current),sha256=hashlib.sha256(raw).hexdigest(),raw=raw))
 S='7,11,1000,8,2,1100';C='256,192,1200,1,0';far='64,64,800,1,0'
 for v in [5,6,9,12,13,14]:
  early=v==5
  add(f'v{v:02}-spinner-circle',v,[S,C],[1,0] if early else [0,0],future=True,note='v5 only spinner height changes; M0603 ret')
  add(f'v{v:02}-circle-spinner',v,['256,192,900,1,0',S],[1,0] if early else [0,0],shift={0:1} if early else {},note='v5 ordinary displacement; modern spinner skipped')
  add(f'v{v:02}-sandwich',v,['256,192,900,1,0',S,C],[2,1,0] if early else [1,0,0],shift={0:2 if early else 1},note='v5 mutates first circle before spinner outer visit; modern ordinary chain crosses spinner')
  add(f'v{v:02}-pair-after',v,[S,'128,128,1200,1,0','128,128,1300,1,0'],[0,1,0],shift={1:1},note='ordinary pair remains actual stack')
  add(f'v{v:02}-even-geometric',v,['128,128,1000,2,0,L|228:128,2,100','228,128,2100,1,0','7,11,4000,8,2,4100'],[0,-1,0] if early else [0,0,0],shift={1:-1} if early else {},note='early geometric end228 vs modern even traversal end128')
  add(f'v{v:02}-tail-through-spinner',v,['128,192,1000,2,0,L|256:192,1,128','7,11,1700,8,2,1800','256,192,1900,1,0'],[0,-1,-2] if early else [0,-1,-1],shift={2:-2 if early else -1},note='v5 forward tail counter reaches2; modern correction subtracts1 from spinner and circle')
  # v14 positive and negative controls included above; zero profile stays historical.
  add(f'v{v:02}-zero-neighbor',v,[S,C],[0,0],stack='0',current=True,future=True,note='100ms gap exceeds zero temporal threshold')
 for cs,ar in [(5,5),(4,8),(7,3)]:
  for v in [5,6,9,12,13]:
   add(f'v{v:02}-cs{cs}-ar{ar}-forward',v,[far,S,C],[0,1,0] if v==5 else [0,0,0],cs=cs,ar=ar,future=(v==5 or (cs==5 and ar==5)),note='all ordinary origins distinct; v5 earlier spinner exemption only')
 for label,x,y in [('inside','258','194'),('equal','259','192'),('outside','259','193')]:
  near=label=='inside'
  add('v05-distance-'+label,5,[S,f'{x},{y},1200,1,0'],[1 if near else 0,0],current=not near,future=True,note='strict Single distance<3')
 for delta in [839,840,841]:
  add(f'v05-forward-gap{delta}',5,[S,f'256,192,{1100+delta},1,0'],[1 if delta<=840 else 0,0],future=True,note='later Single start - (1200f*.7f) > tracked end; equality eligible')
  add(f'v05-reverse-gap{delta}',5,['256,192,1000,1,0',f'7,11,{1000+delta},8,2,2000'],[1 if delta<=840 else 0,0],shift={0:1} if delta<=840 else {},note='conservative refusal retained even when temporal threshold rules out displacement')
 add('v05-spinner-slider',5,[S,'256,192,1200,2,0,L|356:192,1,100'],[1,0],future=True,note='spinner head/end coincide; head branch increments own height only')
 add('v05-two-spinners',5,[S,'99,88,1200,8,2,1300','256,192,1400,1,0'],[2,1,0],future=True,note='each spinner can scan later heads but M0603 retains centers')
 add('v05-nonadjacent',5,[S,'64,64,1200,1,0','400,300,1300,1,0','64,64,1400,1,0'],[0,1,0,0],shift={1:1},note='ordinary all-pairs guard must survive exemption')
 add('v05-forward-explicit',5,[S,C],[1,0],stack='0.7',future=True)
 add('v05-no-neighbor',5,[S,'64,64,1200,1,0'],[0,0],current=True)
 add('v05-file-xy',5,[S,'7,11,1200,1,0'],[0,0],current=True,note='spinner constructor center ignores row xy')
 for n in [6,7]:add(f'v05-forward-timing{n}',5,[S,C],[1,0],columns=n,future=True)
 # Narrow positive handoff: no circles anywhere. Input M0ad9 reads positive
 # predecessor height only when selected object's Circle bit1 is set.
 for kind,curve in [('B','B|306:242|356:192'),('P','P|306:242|356:192'),('C','C|306:242|356:192')]:
  add(f'v05-slider-only-{kind}',5,[S,f'256,192,1200,2,0,{curve},1,100'],[1,0],future=True,note='existing prepared path kind; no new curve algorithm')
 for cs,ar in [(5,5),(4,8),(7,3)]:
  for label,stack in [('default',None),('explicit','0.7')]:
   add(f'v05-slider-only-cs{cs}-ar{ar}-{label}',5,[S,'256,192,1200,2,0,L|356:192,1,100'],[1,0],cs=cs,ar=ar,stack=stack,future=True,note='circle-free slider candidate bypasses M0ad9 circle-bit height gate')
 add('v05-slider-only-two-spinners',5,[S,'7,11,1200,8,2,1300','256,192,1400,2,0,L|356:192,1,100'],[2,1,0],future=True)
 add('v05-slider-only-far-before',5,['64,64,100,2,0,L|114:64,1,50',S,'256,192,1200,2,0,L|356:192,1,100'],[0,1,0],future=True)
 add('v05-slider-only-relation-negative',5,[S,'256,192,1200,2,0,L|356:192,1,100','356,192,1800,2,0,L|456:192,1,100'],[1,0,-1],shift={2:-1},note='ordinary endpoint relation remains unsupported')
 for v in [6,9,12,13]:
  add(f'v{v:02}-other-cs-reverse',v,['256,192,900,1,0',S],[0,0],cs=4,ar=8,note='modern skip means no geometry change; outside bounded v5 extension')
 for delta in [839,840,841]:
  h=1 if delta<=840 else 0
  add(f'v06-backward-gap{delta}',6,[S,'128,128,4000,1,0',f'128,128,{4000+delta},1,0'],[0,h,0],shift={1:h} if h else {},note='modern Int32 gap then Single vs 1200f*.7f; equality eligible')
 add('v05-tail-spinner-only',5,['128,192,1000,2,0,L|256:192,1,128','7,11,1700,8,2,1800'],[0,-1],note='only negative spinner height; earlier slider pair retained conservatively')
 # Guard predictions are independent engineering/source boundary expectations, not original behavior.
 add('negative-break',5,[S,'64,64,2000,1,0'],None,current=False,future=False,guard='break',events='2,1200,1400\n')
 add('negative-order',5,[C,S],None,current=False,future=False,guard='order')
 add('negative-shared-end',5,[S,'64,64,1100,1,0'],None,current=False,future=False,guard='interval')
 add('negative-intersection',5,[S,'64,64,1050,1,0'],None,current=False,future=False,guard='interval')
 add('negative-timing',5,[S,C],None,current=False,future=False,guard='timing',columns=2)
 add('negative-resource-path',5,[S,'64,64,1200,1,0'],None,current=False,future=False,guard='resource',general='AudioFilename:../escape.wav\n')
 for c in out:
  if c['handoff']['extension'] and (c['version']!=5 or 'circle' in c['kinds']):
   c['handoff']=dict(prepared=False,extension=False,reason='circle-bearing no-displacement alone does not prove input-consumer safety')
 return out

def build(check=False):
 (H/'fixtures').mkdir(exist_ok=True);packet=[]
 for c in cases():
  raw=c.pop('raw');p=H/'fixtures'/(c['id']+'.osu')
  if check:assert p.read_bytes()==raw,p
  else:p.write_bytes(raw)
  packet.append(c)
 raw=(json.dumps(dict(schema=1,authority='Issue13 independent literal geometry expectations; no executable oracle',conditions=['ordinary fresh no mods; uncapped host length/source-record compatibility excluded','nonzero numeric displacement only CS5 AR5; other settings assert zero ordinary displacement','v5 forward mutations occur before next outer visit; spinner height never means translated center'],cases=packet),indent=2)+'\n').encode()
 if check:assert (H/'manifest.json').read_bytes()==raw
 else:(H/'manifest.json').write_bytes(raw)
 print(f'PASS {len(packet)} immutable independent fixtures; current {sum(c["current"]["prepared"] for c in packet)}; handoff {sum(c["handoff"]["prepared"] for c in packet)}')
if __name__=='__main__':build('--check' in sys.argv)
