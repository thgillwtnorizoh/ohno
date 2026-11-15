#!/usr/bin/env python3
"""Independent original-static finite expectations, not an original import trace."""
from pathlib import Path
from fractions import Fraction as Q
import hashlib,json,sys
H=Path(__file__).resolve().parent
VERSIONS=[5,6,9,10,12,13,14]
BASE='''osu file format v{version}
[General]
Mode:{mode}
StackLeniency:0
[Metadata]
Title:C001 independent versioned spinner {id}
Creator:Project ohno Work 1
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
def fixtures():
 cases=[]
 def add(id,v,mode=0,mixed=False,bit=False,sample=None,start='2000',end='5000',length=10000,omit_end=False):
  raw=12 if bit else 8;row=f'11,23,{start},{raw},2'+('' if omit_end else ','+end)+(','+sample if sample is not None else '')
  rows=['64,64,1000,1,0',row,'128,128,6000,1,0','160,160,7000,6,0,L|260:160,1,100'] if mixed else [row]
  if mode!=0 or v not in VERSIONS:
   expected={'profile':'NEGATIVE CONTROL: outside finite standard v5-v14 handoff','originalConstruction':'NOT ASSERTED for this profile','currentCanonicalSuccess':False}
  elif omit_end or end=='not-a-number':
   expected={'profile':'ORIGINAL STATIC row-error boundary','originalConstruction':'M28f7 column5 access or M09e4 Double.Parse fails before factory; ordinary nonvalidation caller rethrows','currentCanonicalSuccess':False}
  else:
   s=int(Q(start));e=min(length-50,int(Q(end)));d=e-s
   numeric=sample.split(':') if sample else []
   stored=dict(base=int(numeric[0]) if numeric else 0,addition=int(numeric[1]) if numeric else 0,index=int(numeric[2]) if len(numeric)>2 else 0,volume=max(0,min(100,int(numeric[3]))) if len(numeric)>3 else 0,filename=numeric[4] if len(numeric)>4 else '')
   flags=[True,bit,True,True] if mixed else [bit]
   expected={'profile':'finite supported construction' if d>0 else 'ORIGINAL STATIC degenerate duration; downstream NOT VERIFIED',
    'conditionalInputs':{'F100b':0,'globalLengthProviderMilliseconds':length,'M28f7ValidationArgument':False,'modifiers':'none','ordinaryOD':5,'sourceCompatibility':'null source, or independently supplied modern predicate=false'},
    'originalFactory':'M1872 -> M48e1 -> M05ea -> M3201','storedCenter':[256,192],'rawXYNotForwarded':[11,23],
    'start':s,'end':e,'duration':d,'constructorNewCombo':v<=8,'parserNewCombo':bit,'finalNoBreakFlags':flags,'rawSound':2,'storedSample':stored,
    'currentCanonicalSuccess':v==14 and d>0 and start.isdigit() and end.isdigit()}
   if d>0:
    expected['requiredAtOD5']=int(Q(d,1000)*5)
    expected['accelerationExact']=str(Q(1,12500)+max(Q(0),Q(5000-d,2000000)))
    expected['newComboAfterSpinner']=True if mixed else 'No following object in fixture'
  cases.append(dict(id=id,version=v,mode=mode,mixed=mixed,fileBit4=bit,rows=rows,expected=expected))
 for v in VERSIONS:
  add(f'v{v:02}-single',v);add(f'v{v:02}-mixed',v,mixed=True)
 for v in [5,6,9,14]:add(f'v{v:02}-bit4',v,bit=True)
 for v in [5,13]:
  add(f'v{v:02}-sample-full',v,sample='2:3:2:150:synthetic.wav')
  add(f'v{v:02}-fractional',v,start='2000.9',end='5000.9')
 add('v14-sample-full',14,sample='2:3:2:150:synthetic.wav')
 add('v06-sample-two',6,sample='2:3')
 add('v13-conditional-end-cap',13,length=4000)
 add('v13-end-before-start',13,end='1000');add('v13-zero-duration',13,end='2000')
 add('v09-invalid-end',9,end='not-a-number');add('v14-missing-end',14,omit_end=True)
 for v in [3,128]:add(f'negative-format-{v}',v)
 for v in [9,14]:add(f'negative-mode1-v{v}',v,mode=1)
 return cases
def build(check=False):
 out=[]
 for c in fixtures():
  raw=BASE.format(version=c['version'],mode=c['mode'],id=c['id'],rows='\n'.join(c['rows']))
  b=raw.encode();path=H/'fixtures'/(c['id']+'.osu')
  if check:assert path.read_bytes()==b,path
  else:path.parent.mkdir(exist_ok=True);path.write_bytes(b)
  out.append({k:v for k,v in c.items() if k!='rows'}|{'path':'fixtures/'+path.name,'sha256':hashlib.sha256(b).hexdigest(),'confidence':'RECONSTRUCTED from CONFIRMED ORIGINAL static; conditional host operands, not observed original runtime'})
 j={'schema':1,'source':'Work1 independent generator; Fraction mathematics on deliberately exact representable 3-second durations','versions':VERSIONS,'limits':['No original execution','Source date compatibility is not format version','End=min(globalLength-50,truncated end)+F100b for nonvalidation caller','M3201 directly stores supplied end; end setters are different paths','v3/v128 and other modes are negative engineering controls, not original rejection claims','Audio/host length/current global settings not exercised; supplying a .osu file does not prove original import','Integer/floating overflow/NaN and degenerate downstream behavior NOT VERIFIED'],'cases':out}
 b=(json.dumps(j,indent=2)+'\n').encode();p=H/'manifest.json'
 if check:assert p.read_bytes()==b,p
 else:p.write_bytes(b)
 print('PASS',len(out),'exact hashed independent spinner fixtures and conditional finite expectations')
if __name__=='__main__':build('--check' in sys.argv)
