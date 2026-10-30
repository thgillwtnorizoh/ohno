#!/usr/bin/env python3
"""E24 checks original instruction sites and metadata, never simulates spinning."""
import hashlib
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==24 and len(m)==61
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,[d]]in row(r)['branches'],(hex(r),hex(o),op,hex(d))
def co(r,o,op,v=None):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def slot(root,s,expected):
 a=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
 assert {x['method']for x in a}=={hex(0x6000000+x)for x in expected}
def writes(f):return {(t,o)for t,ss in j['entry_field_sites'].items()for o,p,v in ss if p in ['stfld','stsfld']and v==hex(0x4000000+f)}
# Corrected ordinary factory and exact concrete virtual slots.
site(0x1141,0xe,'newobj',0x6000651);site(0x652,1,'newobj',0x60048de)
site(0x48e1,9,'newobj',0x60005ea);assert row(0x5ea)['owner']=='0x20000d8'
slot(0x9c1,0x1872,[0x1872,0x48e1]);slot(0xd8,0x24c3,[0x24c3,0x5f6])
slot(0xd8,0x24b2,[0x24b2,0x5f5]);slot(0x253,0xac5,[0xac5])
slot(0xe8,0x28e3,[0x28e3,0x653]);slot(0xe8,0x28e4,[0x28e4,0x654])
assert j['ordinary_dispatch']['0x20000d8']['chain']==['0x20000d8','0x200067f','0x20004c3','0x2000828']
co(0x3201,8,'ldc.r4',256.0);co(0x3201,0xd,'ldc.r4',192.0)
site(0x3201,0x2a,'stfld',0x4002523);site(0x3201,0x31,'stfld',0x4002524)
site(0x28f7,0x1ba6,'callvirt',0x6001872);site(0x28f7,0x1bbf,'callvirt',0x60028f0)
site(0x28f0,4,'callvirt',0x60028d2)
# Typed state and exhaustive direct writes, including separate Taiko writers.
for f in [0x29f,0x2af,0x2b0,0x2b6]:assert j['fields'][hex(0x4000000+f)]['signature']=='060d'
assert j['fields']['0x400029d']['signature']=='0615118801010d'
assert j['fields']['0x40002b4']['signature']=='060c'
assert writes(0x29d)=={('0x60005f6',x)for x in [0x86,0x94,0x2cf]}
assert writes(0x2b4)=={('0x60005f4',0x1b3),('0x60005f5',0x139),('0x60005f5',0x395)}
assert writes(0x2b6)=={('0x60005ea',0x12),('0x60005f6',0x1a5)}
assert writes(0x2a2)=={('0x60005f6',0x2ed)}
assert writes(0x29e)=={('0x60005f6',0x4c7)}
assert writes(0x2af)=={('0x60005f5',x)for x in [0x1d7,0x22f,0x278,0x2a0]}|{('0x6001d75',0x11)}
assert writes(0x2b0)=={('0x60005f6',x)for x in [0x1e9,0x29f,0x2b2,0x2c3]}|{('0x6001d78',0x11f),('0x6001d78',0x27c)}
# Candidate current-object input uses raw passed pointer and prepared center.
site(0xac5,0x47,'ldsfld',0x4002a5c);site(0xac5,0x4c,'callvirt',0x60024c3)
site(0x5f6,2,'ldsfld',0x4002a90);br(0x5f6,7,'brtrue.s',0xb)
site(0x5f6,0x23,'ldsfld',0x4002a95);br(0x5f6,0x35,'bge.un.s',0x39)
site(0x5f6,0x3b,'ldfld',0x40002a3);site(0x5f6,0x40,'ldfld',0x4002b6e)
site(0x5f6,0x45,'call',0x6004efe);site(0x5f6,0x5c,'call',0xa0002d7)
assert j['external_refs']['0xa0002d7']['name']=='System.Math::Atan2'
assert not any(t in ['0x6000ff1','0x400252c','0x4001527']for o,p,t in row(0x5f6)['token_sites'])
site(0x5f6,0x70,'call',0x60005f7);co(0x5f6,0x78,'ldc.r8',0.0)
co(0x5f7,0x21,'ldc.i4',2019);co(0x5f7,0x59,'ldc.i4',20190510)
# Exact strict wrap and pi-rejection; zero-motion path precedes hold/time gating.
co(0x5f6,0xd8,'ldc.r8',-3.141592653589793);ar(0x5f6,0xec,'clt')
co(0x5f6,0xf8,'ldc.r8',6.283185307179586)
co(0x5f6,0x143,'ldc.r8',-3.141592653589793);ar(0x5f6,0x157,'clt')
co(0x5f6,0x163,'ldc.r8',-6.283185307179586)
co(0x5f6,0x17b,'ldc.r8',0.999);site(0x5f6,0x185,'call',0xa0002d2)
br(0x5f6,0x1b4,'bne.un.s',0x1f3);co(0x5f6,0x1df,'ldc.r8',3.0)
br(0x5f6,0x1ff,'brtrue.s',0x233);site(0x5f6,0x201,'ldsfld',0x4002a67)
site(0x5f6,0x208,'ldsfld',0x4002a83);br(0x5f6,0x21a,'blt.s',0x229)
br(0x5f6,0x227,'ble.s',0x233);br(0x5f6,0x242,'bge.un.s',0x2b9)
co(0x5f6,0x25f,'ldc.r8',17.333332697550457)
site(0x5f6,0x25a,'call',0x60028ae);site(0x5f6,0x27b,'call',0x60028ae)
co(0x5f6,0x2a8,'ldc.r8',16.666666666666668)
site(0x5f6,0x2cf,'stfld',0x400029d);site(0x5f6,0x2d5,'call',0x60005eb)
br(0x5f6,0x2e0,'bne.un.s',0x2e4)
# Later object-update smoothing, strict time interior, finite clamp and pi-normalized progress.
br(0x5f5,6,'brtrue.s',0x15);br(0x5f5,0x13,'bge.s',0x16)
br(0x5f5,0x14e,'bge',0x39a);br(0x5f5,0x15e,'ble',0x39a)
br(0x5f5,0x168,'brtrue',0x39a);br(0x5f5,0x173,'ble',0x39a)
site(0x5f5,0x195,'call',0x60028ae);co(0x5f5,0x1ce,'ldc.r8',0.03)
co(0x5f5,0x27e,'ldc.r8',-0.05);co(0x5f5,0x28d,'ldc.r8',0.05)
ar(0x5f5,0x2b2,'conv.r4');site(0x5f5,0x364,'call',0x60005fb)
site(0x5f5,0x36c,'callvirt',0x60024c4)
co(0x5f5,0x378,'ldc.r4',1.0);co(0x5f5,0x37f,'ldc.r8',3.141592653589793)
site(0x5f5,0x38a,'call',0xa0002d5);site(0x5f5,0x38f,'call',0xa0002d6)
site(0x5f5,0x395,'stfld',0x40002b4);site(0x5f5,0x3b4,'call',0x60005fd)
site(0x5eb,1,'ldfld',0x40002b4);ar(0x5eb,6,'conv.i4')
# Input/current/candidate/object update order; host loop permits repeated passes.
site(0x43a0,0x172,'stsfld',0x4002a94);site(0x43a0,0x182,'stsfld',0x4002a96)
site(0x43a0,0x790,'callvirt',0x6003350);site(0x43a0,0x7f7,'stsfld',0x4002a95)
co(0x43a0,0x81e,'ldc.i4.s',30);br(0x43a0,0x820,'blt',0xca)
site(0x226b,0x94e,'call',0x6002275);site(0x226b,0x959,'callvirt',0x60028e2)
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
site(0x28e3,0x17,'callvirt',0x60024b2);site(0x28e3,0x1e,'callvirt',0x60028e4)
br(0x28e4,0x8f,'bge.s',0xd6) # Spinner end equality does not complete here.
# Original prepared coordinate field, no assumed playfield-center fallback.
site(0x5f3,0x1f7,'stfld',0x40002a3);site(0x5f3,0x3c0,'stfld',0x40002a3)
site(0x4518,0x3cd,'stfld',0x4002b6e);site(0x4518,0x69b,'callvirt',0x60018ff)
site(0x4518,0x6a0,'call',0x6004f00);site(0x4518,0x6a5,'stfld',0x4002b6e)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if int(f.parent.name)>=24:continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t);shared+=1
fixture=Path(__file__).parents[1]/'fixtures/24/single-spinner.osu'
assert hashlib.sha256(fixture.read_bytes()).hexdigest()=='85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c'
print(f'PASS:61 original methods; ordinary spinner slots/input/wrap/motion/progress/order; {shared} historical body hashes; fixture hashed, not imported; all runtime cases NOT RUN')
