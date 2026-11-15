#!/usr/bin/env python3
"""Bounded evidence/fixture checks; no original or replacement game execution."""
from pathlib import Path
import hashlib,json,subprocess,sys
H=Path(__file__).resolve().parent
j=json.loads(Path(sys.argv[1]).read_bytes());assert j['assertionCount']==62 and j['historicalIdentityChecks']==40 and len(j['methods'])==16 and not j['originalExecuted']
m=j['methods'];assert [0x4e,'bgt.s',[0x5e]] in m['M3201']['branches']
assert not any(s[2]=='0x4000fd1' for s in m['M28f7']['token_sites'])
assert not any(s[2]=='0x4000fd1' for s in m['M05f7']['token_sites'])
assert j['externalMembers']['0xa0002a6']=='Min'
assert j['externalMembers']['0xa000615']=='Parse'
assert [0x1bb7,'callvirt','0x6003e57'] in m['M28f7']['token_sites']
subprocess.run([sys.executable,str(H/'build_fixtures.py'),'--check'],check=True)
cases=json.loads((H/'manifest.json').read_bytes())['cases'];assert len(cases)==33
for c in cases:
 b=(H/c['path']).read_bytes();assert hashlib.sha256(b).hexdigest()==c['sha256'];e=c['expected']
 if 'constructorNewCombo' in e:
  assert e['constructorNewCombo']==(c['version']<=8) and e['parserNewCombo']==c['fileBit4']
  if e['duration']==3000:assert e['requiredAtOD5']==15 and e['accelerationExact']=='27/25000'
  if c['mixed']:assert e['finalNoBreakFlags']==[True,False,True,True]
  if 'sample-full' in c['id']:assert e['storedSample']['volume']==100 and e['storedSample']['filename']=='synthetic.wav'
  if 'fractional' in c['id']:assert e['start']==2000 and e['end']==5000
assert next(c for c in cases if c['id']=='v13-conditional-end-cap')['expected']['end']==3950
assert {c['version'] for c in cases if c['mixed']}=={5,6,9,10,12,13,14}
print('PASS62 decisive sites/40 inherited hashes;33 exact fixtures; independent flags, duration, sample and end-cap discriminators; runtime NOT RUN')
