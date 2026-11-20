#!/usr/bin/env python3
from pathlib import Path
import hashlib,json,sys
H=Path(__file__).resolve().parent
BASE='''osu file format v{version}
[General]
Mode:0
{stack}
[Metadata]
Title:PRIVATE_ROUTE_Z_TITLE
Artist:PRIVATE_ARTIST
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
{rows}
'''
U='Legacy spinner stack profile lacks a zero-displacement certificate.'
W='Legacy spinner zero-displacement certificate exceeds pair work limit.'
spin='11,23,20000,8,2,23000'
cases={
 'isolated':(5,'',spin,'success','canonical-prepared'),
 'unproven':(5,'','256,192,1000,1,0\n'+spin,'unsupported-admission','stack-certificate-unproven'),
 'modern':(6,'','64,64,1000,1,0\n64,64,1100,1,0\n'+spin,'success','canonical-prepared')}
for n in [1414,1415]:cases['pairs'+str(n)]=(5,'','\n'.join(f'{8*(i%64)},{8*(i//64)},{100+2*i},1,0' for i in range(n-1))+'\n'+spin,'success' if n==1414 else 'unsupported-admission','canonical-prepared' if n==1414 else 'stack-certificate-work-limit')
for name,msg in [('spoof-unproven',U),('spoof-work',W),('spoof-mutation','changed since indexing')]:cases[name]=(5,'',spin,'malformed-input','metadata-syntax')
for name in ['title-unproven','title-work']:cases[name]=(5,'',spin,'success','canonical-prepared')
items=[]
for name,(v,stack,rows,status,code) in cases.items():
 b=BASE.format(version=v,stack=stack,rows=rows).encode()
 if name.startswith('spoof-'):b=b.replace(b'CircleSize:5',('CircleSize:'+dict([('spoof-unproven',U),('spoof-work',W),('spoof-mutation','changed since indexing')])[name]).encode())
 if name.startswith('title-'):b=b.replace(b'PRIVATE_ROUTE_Z_TITLE',(U if name=='title-unproven' else W).encode())
 p=H/'fixtures'/(name+'.osu')
 if '--check' in sys.argv:assert p.read_bytes()==b,p
 else:p.write_bytes(b)
 items.append(dict(name=name,path='fixtures/'+p.name,sha256=hashlib.sha256(b).hexdigest(),canonicalStatus=status,canonicalCode=code,index='success',exceptionType='InvalidDataException' if status!='success' else None))
data=(json.dumps(dict(author='Work1 independent synthetic RouteZ diagnostic fixtures; engineering outcomes, no owner data',source='f6d0411abc865134a1dafb7fb7ef1473b621a84f',cases=items),indent=2)+'\n').encode()
if '--check' in sys.argv:assert (H/'manifest.json').read_bytes()==data
else:(H/'manifest.json').write_bytes(data)
print('PASS',len(items),'exact fixtures and hashes')
