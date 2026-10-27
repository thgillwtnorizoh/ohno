#!/usr/bin/env python3
"""Assert extracted original sites; never executes candidate or eligibility logic."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text()); m=j['methods']
assert j['expedition']==7 and len(m)==49
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def site(r,o,op,t): assert [o,op,hex(t)] in m[hex(0x6000000+r)]['token_sites'],(hex(r),hex(o))
def branch(r,o,op,d): assert [o,op,d] in m[hex(0x6000000+r)]['branches'],(hex(r),hex(o))
def const(r,o,op,v): assert [o,op,v] in m[hex(0x6000000+r)]['constants'],(hex(r),hex(o))
def ar(r,o,op): assert [o,op] in m[hex(0x6000000+r)]['arithmetic_sites'],(hex(r),hex(o))
# Ordinary manager/factories, circle constructor and slider opt-in.
site(0x1141,0xe,'newobj',0x6000651)
site(0x652,1,'newobj',0x60048de)
site(0x48df,0xd,'newobj',0x60045eb)
site(0x48e0,0x17,'newobj',0x600142d)
const(0x142d,0x17f,'ldc.i4.1',None); site(0x142d,0x180,'stfld',0x400153a)
writes=[(k,o,op,t) for k,ss in j['watched_field_sites'].items() for o,op,t in ss if t=='0x400153a' and op.startswith('st')]
assert writes==[('0x600142d',0x180,'stfld','0x400153a')]
# No circle preparation override on its entire ancestry; base body returns.
slot=m['0x60024b5']
matches=[d['method'] for ds in j['circle_ancestry_declarations'].values() for d in ds if d['virtual'] and d['name']==slot['name'] and d['signature']==slot['signature']]
assert matches==['0x60024b5'] and slot['code_bytes']==1 and slot['returns']==[0]
assert m['0x60024b6']['abstract'] and m['0x60024b6']['rva']=='0x0'
assert m['0x6001448']['name']==m['0x60045fc']['name']==m['0x60024b6']['name']
# Start-order registration, end-key comparison for search.
site(0x28d2,0x4c,'callvirt',0xa0004be); site(0x28d2,0xcb,'callvirt',0xa0010b2)
branch(0x24c2,6,'brtrue.s',[0x10]); branch(0x24c2,0xe,'brfalse.s',[0x22])
site(0x24c2,0x11,'ldflda',0x4002524);site(0x24c2,0x23,'ldfld',0x4002523)
site(0x396e,9,'stfld',0x4002523);site(0x396e,0x10,'stfld',0x4002524)
assert j['search_type_specs']['0xa0004be']=='151282c10112930c'
# Candidate count, lower search, upper scan and inclusive GetRange quirks.
site(0x28e2,0x12,'stfld',0x40017fc)
branch(0x28e2,0x1d,'brfalse.s',[0x2b]);branch(0x28e2,0x29,'brfalse.s',[0x33])
site(0x28e2,0x2c,'ldfld',0x4000adb);site(0x28e2,0x34,'call',0x60028b6)
site(0x28e2,0x74,'newobj',0x600396e); site(0x28e2,0x79,'callvirt',0xa0004be)
branch(0x28e2,0x81,'bge.s',[0x86]);branch(0x28e2,0x9d,'bgt.s',[0xae])
branch(0x28e2,0xac,'blt.s',[0x8a]);branch(0x28e2,0xb5,'bne.un.s',[0xd9])
site(0x28e2,0xbd,'callvirt',0xa0004bf);site(0x28e2,0xe6,'callvirt',0xa0004c0)
site(0x28e2,0xeb,'stfld',0x40017fd)
for o,op in [(0x39,'sub'),(0x47,'add'),(0xe3,'add'),(0xe5,'sub')]: ar(0x28e2,o,op)
site(0x28e2,0x55,'call',0xa0002a6);site(0x28e2,0x64,'call',0xa0000fc)
branch(0x28b6,6,'bne.un.s',[0x14]);branch(0x28b6,0x12,'brfalse.s',[0x16])
const(0x28b6,0x16,'ldc.i4',800);const(0x28b6,0x14,'ldc.i4.0',None)
const(0x289b,0x22,'ldc.i4',240);site(0x289b,0x27,'stsfld',0x40017e7)
assert j['field_flags']['0x40017e7']['init_only']
site(0x28be,0x12e,'stfld',0x4000adb);ar(0x28be,0x12d,'conv.i4')
for o,v in [(0x10d,1800.0),(0x116,1200.0),(0x11f,450.0)]:const(0x28be,o,'ldc.r8',v)
# Eligibility uses inclusive thresholds and distinct circle late margin.
branch(0x1448,0x17,'blt.s',[0x30]);site(0x1448,0x24,'ldsfld',0x40017e7)
site(0x45fc,0xc,'callvirt',0x60024c9);site(0x45fc,0x2b,'callvirt',0x60028b6)
site(0x24c9,6,'ldfld',0x4000adb)
# Refresh/update precedes draw dispatch; readiness is a separate gate.
site(0x226b,7,'call',0x6002269);branch(0x226b,0xc,'brfalse.s',[0xf])
site(0x226b,0x959,'callvirt',0x60028e2);site(0x226b,0x9cf,'callvirt',0x60028e3)
site(0x28e1,1,'callvirt',0x60028e2);site(0x28e1,7,'callvirt',0x60028e3)
site(0x28e3,0x17,'callvirt',0x60024b2);site(0x28e3,0x1e,'callvirt',0x60028e4)
site(0x223c,0x5d,'stfld',0x40013f0);site(0x2269,0x154,'callvirt',0x6000709)
site(0x2269,0x15a,'stsfld',0x40013d8)
site(0x46a6,0x7d,'call',0x60028e5)
site(0x28e5,0x90,'ldfld',0x400153a);site(0x28e5,0x98,'callvirt',0x60024b5)
site(0x144b,0x35,'callvirt',0x60024b6);branch(0x144b,0x3a,'brfalse.s',[0x44])
site(0x144b,0x3d,'ldfld',0x4000bdc);site(0x144b,0x45,'call',0x600144f)
site(0x144f,0x33,'stfld',0x4000bd9);site(0x144f,0x3e,'stfld',0x4000be5)
# Existing evidence body identities, not numerical-model comparisons.
old=json.loads((Path(__file__).parents[1]/'evidence/06/linear-geometry.json').read_text())['methods']
shared=0
for t,r in m.items():
 if t in old and 'body_sha256' in r:
  assert r['body_sha256']==old[t]['body_sha256'],t
  shared+=1
print(f'PASS: 49 methods; candidate search/scan/storage, comparison mismatch, circle/slider gates, readiness/dispatch; {shared} E06 hashes agree; 19659 decoded bodies')
