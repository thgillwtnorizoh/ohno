#!/usr/bin/env python3
"""E34 original metadata/CIL checks; no chart loader or score simulator."""
import json,sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert len(m)==21 and j['scan']=={'decoded':19659,'zero_rva':1739,'errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op)
def const(r,o,op,v):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def br(r,o,op,v):assert [o,op,v]in row(r)['branches'],(hex(r),hex(o),op)
assert j['enums']['0x2000b24']['values']['HitObjects']==64
assert j['enums']['0x2000b24']['values']['Events']==32
for o,f in [(0x30,0xff2),(0x37,0xfe5),(0xf81,0xff2),(0x1098,0xfe5)]:site(0x1bc1,o,'stfld',0x4000000+f)
site(0x1bc1,0x2e8,'call',0xa000135);br(0x1bc1,0x33c,'beq',[0xe83])
site(0x1bc1,0xea4,'call',0x60009e4);ar(0x1bc1,0xea9,'conv.i4')
const(0x1bc1,0xeb8,'ldc.i4',139);br(0x1bc1,0xe91,'bne.un.s',[0xeac])
for o in [0x1063,0x1081]:site(0x1bc1,o,'call',0x60009e4)
for o in [0x108e,0x1091]:ar(0x1bc1,o,'sub')
const(0x1bc1,0x1092,'ldc.i4',1000);ar(0x1bc1,0x1097,'div')
site(0x1bc1,0xdce,'call',0xa000321);site(0x1bc1,0xdda,'call',0xa000321)
ar(0x1bc1,0xddf,'sub');ar(0x1bc1,0xde0,'add')
# Cache replaces stored seconds, reconstructs count from three UInt16 values.
site(0x1bf1,0x21,'call',0x6001bf2)
site(0x1bf2,0x1e8,'callvirt',0xa00050e);site(0x1bf2,0x1ed,'stfld',0x4000fe5)
for o in [0x92,0x9e,0xaa]:site(0x1bf2,o,'callvirt',0xa000d8b)
site(0x1bf2,0x369,'stfld',0x4000ff2)
for f in ['0x4000fe5','0x4000ff2']:assert j['fields'][f]['signature']=='0608'
# Exact original K getter operation order, including Single division before r8.
for o,f in [(1,0xb54),(7,0xb55),(0xe,0xb53),(0x15,0xff2),(0x1c,0xfe5)]:site(0x1bc4,o,'ldfld',0x4000000+f)
for o,op in [(0x1a,'conv.r4'),(0x21,'conv.r4'),(0x22,'div'),(0x28,'mul'),(0x38,'add'),(0x3e,'div'),(0x44,'mul'),(0x45,'conv.r8'),(0x4b,'conv.i4')]:ar(0x1bc4,o,op)
site(0x1bc4,0x33,'call',0x6002d7d);site(0x1bc4,0x46,'call',0xa000398)
for o,v in [(0x23,8.0),(0x29,0.0),(0x2e,16.0),(0x39,38.0),(0x3f,5.0)]:const(0x1bc4,o,'ldc.r4',v)
br(0x2d7d,0x18,'ble.un.s',[0x1c]);br(0x2d7d,0x1e,'bge.un.s',[0x22])
# Read metadata before setup; bind same selected map; optional reload=false in Play.
site(0x2237,1,'call',0x6002c63);site(0x2237,6,'stfld',0x40013f8)
site(0x2c63,0,'ldsfld',0x4001a0f);site(0x2c64,0x21,'stsfld',0x4001a0f)
site(0x223b,0x8e,'callvirt',0x6001bc1);site(0x223b,0xff,'call',0x6002258)
site(0x28a2,2,'stfld',0x40017ee);site(0x223b,0x182,'callvirt',0x60028a2)
site(0x223b,0x18f,'callvirt',0x60028f5);site(0x223b,0x265,'callvirt',0x6000aaf)
site(0x1140,0x18,'call',0x6000aaf);site(0xaaf,1,'callvirt',0x6000ab7)
site(0xab7,0x43,'call',0x6002c63);site(0xab7,0x48,'callvirt',0x6001bc4);site(0xab7,0x6e,'call',0x600156b);site(0xab7,0x74,'stfld',0x4000605)
writers={(x[0],x[1],x[3])for x in j['operand_sites']if x[2]=='stfld'}
assert writers=={('0x6001bc1',0x30,'0x4000ff2'),('0x6001bc1',0x37,'0x4000fe5'),('0x6001bc1',0xf81,'0x4000ff2'),('0x6001bc1',0x1098,'0x4000fe5'),('0x6001bf2',0x1ed,'0x4000fe5'),('0x6001bf2',0x369,'0x4000ff2')}
shared=0
for n in ['04','05','33']:
 for p in (Path(__file__).parents[1]/'evidence'/n).glob('*.json'):
  old=json.loads(p.read_text()).get('methods',{})
  for token,r in m.items():
   if token in old and old[token].get('body_sha256'):
    assert r['body_sha256']==old[token]['body_sha256'],(p,token)
    shared+=1
print(f'PASS E34:21 methods, six exact operand stores, parser/cache/setup/float/rounding sites; {shared} historical hashes; no original runtime')
