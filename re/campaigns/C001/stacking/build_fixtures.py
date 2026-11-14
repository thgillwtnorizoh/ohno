#!/usr/bin/env python3
"""Explicit finite expectations from 03-standard-stacking.md; no stacking oracle."""
from pathlib import Path
import hashlib,json,sys
HERE=Path(__file__).resolve().parent
BASE='''osu file format v{version}

[General]
Mode:0
StackLeniency:{leniency}
[Metadata]
Title:C001 independent stacking {id}
Artist:Project ohno
Creator:Work 1
Version:Stacking
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
'''
def c(x,t,y=100):return f'{x},{y},{t},1,0'
def s(x,t,endx,length=100,repeats=1,kind='L'):return f'{x},100,{t},2,0,{kind}|{endx}:100,{repeats},{length}'
CASES=[
('circle-chain',[c(100,1000),c(100,1100),c(100,1200)],[2,1,0],'reverse anchor propagation'),
('distance-equal-three',[c(100,1000),c(103,1100)],[0,0],'distance ==3 excluded'),
('distance-inside',[c(100,1000),c(102,1100)],[1,0],'distance 2 included'),
('distance-diagonal-outside',[c(100,1000),c(102,1100,103)],[0,0],'Euclidean distance not per-axis'),
('time-equal',[c(100,1000),c(100,1600)],[1,0],'600 == Single(1200)*.5 included'),
('time-outside',[c(100,1000),c(100,1601)],[0,0],'601 >600 exits'),
('moving-anchor',[c(100,1000),c(100,1500),c(100,2000)],[2,1,0],'compare to updated anchor, not outer object'),
('zero-leniency',[c(100,1000),c(100,1100)],[0,0],'distinct times, threshold0'),
('tail-negative',[s(100,1000,200),c(200,1600),c(200,1700)],[0,-1,-2],'tail offset height(anchor)-height(slider)+1'),
('tail-selective',[s(100,1000,200),c(200,1600),c(300,1650),c(200,1700)],[0,-1,0,-2],'negative range only points within3 of tail'),
('circle-before-slider',[c(100,1000),s(100,1100,200)],[1,0],'slider backward endpoint-to-head match'),
('slider-chain',[s(100,1000,200),s(200,1600,300)],[1,0],'earlier slider end matches later slider head'),
('slider-start-boundary',[s(100,1000,300,length=200),s(300,2100,400)],[0,0],'slider branch uses start-start, not earlier end'),
('tail-even-repeat',[s(100,1000,200,repeats=2),c(100,2100)],[0,-1],'final traversal endpoint returns to head'),
('early-time-break',[s(0,1000,200,length=200),c(300,1300),c(200,2000)],[0,0,0],'far intervening circle causes time break before long slider'),
('bezier-tail',[s(100,1000,200,kind='B'),c(200,1600)],[0,-1],'straight Bezier uses prepared tail too'),
('v6-circle-chain',[c(100,1000),c(100,1100),c(100,1200)],[2,1,0],'>5 version boundary'),
('spinner-skip',[c(100,1000),'256,192,1100,8,0,1150',c(100,1200)],[1,0,0],'spinner skipped before time test'),
]
def build(check=False):
 cases=[]
 for ident,rows,heights,feature in CASES:
  data=(BASE.format(version=6 if ident.startswith('v6') else 14,leniency=0 if ident=='zero-leniency' else .5,id=ident)+'\n'.join(rows)+'\n').encode()
  path=HERE/'fixtures'/f'{ident}.osu'
  if check:assert path.read_bytes()==data,path
  else:path.parent.mkdir(exist_ok=True);path.write_bytes(data)
  heads=[[int(v) for v in row.split(',')[:2]] for row in rows]
  cases.append(dict(id=ident,path=f'fixtures/{ident}.osu',sha256=hashlib.sha256(data).hexdigest(),rows=rows,expected_heights=heights,original_heads=heads,feature=feature,confidence='RECONSTRUCTED from CONFIRMED STATIC',originalRuntime='NOT RUN'))
 out={'schema':1,'authority':'../original-behavior/03-standard-stacking.md','generator':'build_fixtures.py','coordinate_contract':'prepared = raw - height * Single(radius/10) per component; viewport512x384, CS5, no mods radius=32.013118743896484; tolerance .00003','cases':cases}
 data=(json.dumps(out,indent=2,sort_keys=True)+'\n').encode();path=HERE/'manifest.json'
 if check:assert path.read_bytes()==data
 else:path.write_bytes(data)
 print('PASS',len(cases),'exact independent stacking fixtures', 'verified' if check else 'generated')
if __name__=='__main__':build('--check' in sys.argv)
