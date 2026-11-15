#!/usr/bin/env python3
"""Independent discriminators; no production code imported as an oracle."""
from pathlib import Path
import json,hashlib,sys
H=Path(__file__).resolve().parent
VERSIONS=[5,6,9,12,13,14]
SPIN='11,23,2000,8,2,5000'
def cases():
 out=[]
 def add(id,v,stack=None,shape='single',cs=5,ar=5,columns=8,breaks=False,order=False):
  rows=[SPIN];heights=[0];positions=[[256,192]];relation='no relationships';profile='geometric-no-op';actual=False
  if shape=='mixed':rows=['64,64,1000,1,0',SPIN,'400,300,6000,2,0,L|450:300,1,50'];heights=[0,0,0];positions=[[64,64],[256,192],[400,300]]
  if shape=='circle-pair':rows=[SPIN,'128,128,6000,1,0','128,128,6100,1,0'];heights=[0,1,0];positions=[[256,192],[124.79868817329407,124.79868817329407],[128,128]];relation='ordinary head/head';profile='reuse-modern-cs5-ar5' if v>5 else 'excluded-early-actual-stack';actual=True
  if shape=='spinner-after':rows=['256,192,1900,1,0',SPIN];heights=[1,0] if v<=5 else [0,0];positions=[[252.79868817329407,188.79868817329407],[256,192]] if v<=5 else [[256,192],[256,192]];relation='circle before spinner center';profile='excluded-early-actual-stack' if v<=5 else 'reuse-modern-cs5-ar5';actual=v<=5
  if shape=='spinner-before':rows=[SPIN,'256,192,5100,1,0'];heights=[1,0] if v<=5 else [0,0];positions=[[256,192],[256,192]];relation='spinner internal height may change, M0603 ignores position';profile='excluded-early-certificate' if v<=5 else 'reuse-modern-cs5-ar5'
  if shape in ('tail-center','tail-away'):
   y=192 if shape=='tail-center' else 128;x=256 if shape=='tail-center' else 228;length=128 if shape=='tail-center' else 100
   rows=[f'128,{y},1000,2,0,L|{x}:{y},1,{length}','11,23,1700,8,2,1800',f'{x},{y},1900,1,0'];heights=[0,-1 if shape=='tail-center' else 0,-1];positions=[[128,y],[256,192],[x+3.2013118267059326,y+3.2013118267059326]];relation='negative slider-tail correction including spinner height; fixed spinner center';profile='reuse-modern-cs5-ar5';actual=True
  if shape=='even-tail':
   rows=['128,128,1000,2,0,L|228:128,2,100','228,128,2100,1,0','11,23,4000,8,2,5000'];heights=[0,-1,0] if v<=5 else [0,0,0];positions=[[128,128],[231.20131182670593,131.20131182670593],[256,192]] if v<=5 else [[128,128],[228,128],[256,192]];relation='legacy geometric path end versus modern final traversal endpoint';profile='excluded-early-actual-stack' if v<=5 else 'reuse-modern-cs5-ar5';actual=v<=5
  if shape=='overlap':rows=[SPIN,'64,64,3000,1,0'];heights=None;positions=None;relation='overlapping spinner interval';profile='excluded-overlap'
  if order:rows=list(reversed(rows));heights=None;positions=None;relation='out of chronological source order';profile='excluded-source-order'
  if stack=='0':
   # All geometric matches need a positive temporal threshold in these fixtures.
   if shape=='circle-pair':heights=[0,0,0];positions=[[256,192],[128,128],[128,128]];actual=False
   profile='retained-zero-setting' if not(shape=='overlap' or order or breaks or columns<8) else profile
  if breaks:profile='excluded-breaks'
  if columns<6:profile='deferred-timing-defaults'
  elif columns<8:profile='explicit-sample-short-timing'
  flags={'single':[False],'mixed':[True,False,True],'circle-pair':[False,True,False],'spinner-before':[False,True],'spinner-after':[True,False],'tail-center':[True,False,True],'tail-away':[True,False,True],'even-tail':[True,False,False]}.get(shape)
  if order or breaks:flags=None
  setting=.7 if stack is None else float(stack)
  unsupported=v<14 and setting!=0
  guard='none'
  if v<14:
   if columns<8:guard='explicit-timing'
   elif setting!=0 or breaks:guard='stack-setting-or-break'
   elif order:guard='registered-order'
   elif shape=='overlap':guard='separated-intervals'
  elif shape=='overlap':guard='spinner-intervals'
  canonical=guard=='none'
  # Original three-field row reaches row[3] and is caught/null; defaults differ.
  originalTiming='full explicit' if columns==8 else 'base=true effects=0; explicit samples' if columns==6 else 'effects=0; explicit base/samples' if columns==7 else 'caller F1000 volume' if columns in (4,5) else 'M28ff catches missing row[3], returns null' if columns==3 else 'map sample set + M3b3c index; volume100'
  if columns<6:heights=None;positions=None
  raw=f'''osu file format v{v}
[General]
Mode:0
{'' if stack is None else 'StackLeniency:'+stack}
[Metadata]
Title:C001 independent Issue9 {id}
Creator:Project ohno Work1
[Difficulty]
HPDrainRate:5
CircleSize:{cs}
OverallDifficulty:5
ApproachRate:{ar}
SliderMultiplier:1
SliderTickRate:1
[Events]
{ '2,1200,1600' if breaks else ''}
[TimingPoints]
{','.join(['0','500','4','1','0','100','1','0'][:columns])}
[HitObjects]
{chr(10).join(rows)}
'''.encode()
  # Static coordinates assume finite preparation, no cap-active host and modern source policy.
  out.append(dict(id=id,version=v,stack=stack,expectedSetting=setting,cs=cs,ar=ar,timingColumns=columns,shape=shape,breaks=breaks,sourceOrder=not order,path='fixtures/'+id+'.osu',sha256=hashlib.sha256(raw).hexdigest(),current=dict(index='Unsupported' if unsupported else 'Indexed',canonical=canonical,guard=guard),original=dict(classification='RECONSTRUCTED from pinned original static, conditional host length/source policy',heights=heights,preparedHeads=positions,finalNoBreakFlags=flags,relation=relation,timing=originalTiming,spinnerCenterSetter='M0603 ret'),handoff=dict(profile=profile,actualOrdinaryStack=actual),raw=raw))
 for v in VERSIONS:
  for label,stack in [('default',None),('zero','0'),('point7','0.7')]:
   for shape in ['single','mixed']:add(f'v{v:02}-{label}-{shape}',v,stack,shape)
  add(f'v{v:02}-cs4-ar8-isolated',v,cs=4,ar=8)
  add(f'v{v:02}-cs7-ar3-mixed',v,cs=7,ar=3,shape='mixed')
 for v in [5,6,14]:
  for shape in ['circle-pair','spinner-before','spinner-after']:add(f'v{v:02}-{shape}',v,stack='0.7',shape=shape)
 for v in [5,6,14]:add(f'v{v:02}-even-tail',v,'0.7','even-tail')
 for v in [6,14]:
  for shape in ['tail-center','tail-away']:add(f'v{v:02}-{shape}',v,stack='0.7',shape=shape)
 add('v09-break-zero',9,'0','mixed',breaks=True)
 add('v09-overlap-zero',9,'0','overlap')
 add('v09-order-zero',9,'0','mixed',order=True)
 for n in range(2,8):
  for label,stack in [('default',None),('zero','0')]:add(f'v09-timing{n}-{label}',9,stack,columns=n)
 for n in [6,7]:add(f'v14-timing{n}',14,'0.7',columns=n)
 return out
def build(check=False):
 all=cases();result=[]
 for c in all:
  raw=c.pop('raw');p=H/c['path']
  if check:assert p.read_bytes()==raw,p
  else:p.write_bytes(raw)
  result.append(c)
 packet=dict(schema=1,source='Work1 independent finite Issue9 discriminators; no imported production/reference algorithm',conditions=['ordinary no mods, fresh map, no recorded source legacy','host length high enough not to activate end cap; original not executed','CS5 AR5 step=3.2013118267059326 for nonzero coordinate predictions','Other CS/AR predictions assert zero displacement only, no radius extrapolation'],cases=result)
 data=(json.dumps(packet,indent=2)+'\n').encode();p=H/'manifest.json'
 if check:assert p.read_bytes()==data,p
 else:p.write_bytes(data)
 print('PASS',len(all),'deterministic fixtures, hashes and independent conditional expectations')
if __name__=='__main__':build('--check' in sys.argv)
