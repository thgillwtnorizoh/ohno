#!/usr/bin/env python3
"""Independent synthetic byte corpus. Expectations are explicit static predictions, not game observations."""
from pathlib import Path
import hashlib,json,sys
HERE=Path(__file__).resolve().parent
BASE='''osu file format v14

[General]
Mode:0
StackLeniency:0
[Metadata]
Title:C001 control
Artist:Independent synthetic fixture
Creator:Project ohno Work 1
Version:C001
[Difficulty]
HPDrainRate:5
CircleSize:5
OverallDifficulty:5
ApproachRate:5
SliderMultiplier:1.4
SliderTickRate:1
[TimingPoints]
0,500,4,1,0,100,1,0
[HitObjects]
100,100,1000,1,0
200,100,2000,1,0
'''
CASES=[]
def add(id,text,feature,metadata,prepared=None,authority='original-behavior/01-parser-boundaries.md'):
 CASES.append({'id':id,'text':text,'feature':feature,'expected_metadata':metadata,'expected_prepared':prepared,'authority':authority})
meta={'Format':14,'Ruleset':0,'Title':'C001 control','Hp':5,'Cs':5,'Od':5,'Ar':5,'Objects':2,'Circles':2,'Sliders':0,'Spinners':0}
add('control',BASE,'ordinary distinct unstacked circles',dict(meta),{'Circles':2,'Sliders':0,'Spinners':0,'CircleSize':5,'OverallDifficulty':5,'ApproachRate':5})
for v in [3,4,5,7,8,9,10,12,13,14,15]:add(f'version-{v}',BASE.replace('format v14',f'format v{v}'),'header recognition only, not general version admission',{'Format':v})
add('title-colons',BASE.replace('Title:C001 control','Title:a:b:c'),'first-colon metadata remainder',{'Title':'a:b:c'})
add('unknown-section',BASE.replace('Title:C001 control','Title:old\n[UnrecognizedSection]\nTitle:new'),'unknown section retains Metadata',{'Title':'new'})
add('unknown-case-section',BASE.replace('Title:C001 control','Title:old\n[metadata]\nTitle:new'),'case-sensitive enum parse retains Metadata',{'Title':'new'})
add('duplicate-title',BASE.replace('Title:C001 control','Title:old\nTitle:new'),'ordinary duplicate title overwrite',{'Title':'new'})
add('ar-fallback',BASE.replace('OverallDifficulty:5\nApproachRate:5','OverallDifficulty:7'),'OD-to-AR inheritance before explicit AR',{'Od':7,'Ar':7},{'OverallDifficulty':7,'ApproachRate':7,'Circles':2})
add('ar-explicit-first',BASE.replace('OverallDifficulty:5\nApproachRate:5','ApproachRate:3\nOverallDifficulty:7'),'explicit AR protects later OD',{'Od':7,'Ar':3},{'OverallDifficulty':7,'ApproachRate':3,'Circles':2})
add('ar-explicit-last',BASE.replace('OverallDifficulty:5\nApproachRate:5','OverallDifficulty:7\nApproachRate:3'),'explicit AR after OD replaces inheritance',{'Od':7,'Ar':3})
add('defaults',BASE.replace('HPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\n',''),'fresh difficulty defaults',{'Hp':5,'Cs':5,'Od':5,'Ar':5},{'CircleSize':5,'OverallDifficulty':5,'ApproachRate':5,'Circles':2})
for field,short in [('HPDrainRate','Hp'),('CircleSize','Cs'),('OverallDifficulty','Od'),('ApproachRate','Ar')]:
 for raw,want in [('-1',0),('12',10)]:add(f'clamp-{short.lower()}-{raw.replace("-","neg")}',BASE.replace(f'{field}:5',f'{field}:{raw}'),f'v14 {field} clamp',{short:want})
add('mania-cs',BASE.replace('Mode:0','Mode:3').replace('CircleSize:5','CircleSize:17.5'),'mode3 CS upper18',{'Ruleset':3,'Cs':17.5})
add('mania-mode-after-cs',BASE.replace('Mode:0\n','').replace('CircleSize:5','CircleSize:17.5\n[General]\nMode:3\n[Difficulty]'),'CS consumes mode at field interpretation time',{'Ruleset':3,'Cs':10})
for raw in ('99','-2'):add('invalid-mode-'+raw.replace('-','neg'),BASE.replace('Mode:0',f'Mode:{raw}'),'M1bbb rejects undefined enum to0',{'Ruleset':0})
for flags in [3,9,10,11,129,130,136]:
 text=BASE.replace('100,100,1000,1,0','100,100,1000,'+str(flags)+',0,1500')
 # Header summary's exact-value switch counts no category for the combined row;
 # runtime object factory follows priority bits. These intentionally differ.
 kind='Circles' if flags&1 else 'Sliders' if flags&2 else 'Spinners' if flags&8 else 'Holds'
 prepared={'Circles':2,'Sliders':0,'Spinners':0} if kind=='Circles' else None
 add(f'combined-{flags}',text,'metadata exact masked category versus factory bit priority',{'Objects':2,'Circles':1,'Sliders':0,'Spinners':0},prepared)
# Malformed/compatibility probes deliberately have no guessed successful parse state.
for id,text,feature in [
 ('v12-fraction',BASE.replace('format v14','format v12').replace('HPDrainRate:5','HPDrainRate:4.5'),'Byte.Parse failure and partial refresh'),
 ('timing-three-columns',BASE.replace('0,500,4,1,0,100,1,0','0,500,4'),'missing required fourth column caught to null'),
 ('timing-two-columns',BASE.replace('0,500,4,1,0,100,1,0','0,500'),'historical short defaults include external sample operand'),
 ('hold-short-sample',BASE.replace('100,100,1000,1,0','100,100,1000,128,0,1500:1'),'hold length>1 then unguarded cell2'),
 ('perfect-path',BASE.replace('100,100,1000,1,0','100,100,1000,2,0,P|150:50|200:100,1,157'),'P curve data and geometry'),
 ('catmull-path',BASE.replace('100,100,1000,1,0','100,100,1000,2,0,C|150:50|200:100,1,157'),'C curve data and geometry'),
 ('slider-repeats-9001',BASE.replace('100,100,1000,1,0','100,100,1000,2,0,L|200:100,9001,100'),'original explicit >9000 rejection'),
 ('header-leading-blank','\n'+BASE,'recognition versus default14 continuation'),
 ('timing-order',BASE.replace('0,500,4,1,0,100,1,0','1000,500,4,1,0,100,1,0\n0,-50,4,2,1,60,0,1'),'unsorted timing source order'),
 ('stack-tail',BASE.replace('StackLeniency:0','StackLeniency:0.7').replace('100,100,1000,1,0','100,100,1000,2,0,L|200:100,1,100'),'negative slider-tail stack'),
]:add(id,text,feature,{})
manifest=[]
for c in CASES:
 data=c.pop('text').encode('utf-8');name=c['id']+'.osu';path=HERE/'fixtures'/name
 if '--check' in sys.argv:assert path.read_bytes()==data,name
 else:path.parent.mkdir(exist_ok=True);path.write_bytes(data)
 manifest.append(c|{'path':'fixtures/'+name,'sha256':hashlib.sha256(data).hexdigest(),'bytes':len(data),'source':'build.py','confidence':'RECONSTRUCTED finite prediction from CONFIRMED STATIC sites','original_runtime':'NOT RUN','actual_reconstruction':'verification/production-results.json (when executed)','limitation':'Only nonempty expected fields are asserted. Empty expectations are diagnostic probes, never passes proving original equivalence.'})
packet={'campaign':'C001','cases':manifest,'qa15_reuse':'re/qa/qa15/beatmaps/manifest.json (27 files unchanged)'}
encoded=(json.dumps(packet,indent=2)+'\n').encode()
if '--check' in sys.argv:assert (HERE/'manifest.json').read_bytes()==encoded
else:(HERE/'manifest.json').write_bytes(encoded)
print('PASS C001 reproducible corpus',len(manifest),'files;',sum(bool(x['expected_metadata']) for x in manifest),'static metadata controls;',sum(x['expected_prepared'] is not None for x in manifest),'prepared controls; original runtime NOT RUN')
