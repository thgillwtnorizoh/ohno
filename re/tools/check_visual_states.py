#!/usr/bin/env python3
"""Check original evidence sites, slot identities and historical hashes; no animation execution."""
import json
import sys
from pathlib import Path

j = json.loads(Path(sys.argv[1]).read_text()); m = j['methods']
assert j['expedition'] == 8 and len(m) == 59
assert j['archive_sha256'] == '7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256'] == '6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan'] == {'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def site(r,o,op,t):
    assert [o,op,hex(t)] in m[hex(0x6000000+r)]['token_sites'], (hex(r),hex(o),op,hex(t))
def branch(r,o,op,d):
    assert [o,op,d] in m[hex(0x6000000+r)]['branches'], (hex(r),hex(o),op,d)
def const(r,o,op,v):
    assert [o,op,v] in m[hex(0x6000000+r)]['constants'], (hex(r),hex(o),op,v)
def ar(r,o,op):
    assert [o,op] in m[hex(0x6000000+r)]['arithmetic_sites'], (hex(r),hex(o),op)

# Shared constructor and distinct component identities/collections.
site(0x48df,0xd,'newobj',0x60045eb)
site(0x48e0,0x17,'newobj',0x600142d)
for o,t in [(0x1c3,0x4002bf8),(0x25e,0x4002bf9),(0x2ba,0x4002bfa),(0x340,0x4002bfb)]:
    site(0x45ec,o,'stfld',t)
for o in [0x208,0x22d,0x38f,0x3af]: site(0x45ec,o,'newobj',0x6002995)
const(0x45ec,0x1ee,'ldc.r4',0.8999999761581421)
const(0x45ec,0x21e,'ldc.r4',4.0); const(0x45ec,0x223,'ldc.r4',1.0)
const(0x289b,0x18,'ldc.i4',400); site(0x289b,0x1d,'stsfld',0x40017e6)
site(0x45ec,0x202,'call',0xa0002a6)
for o in [0x3c1,0x3d7,0x3ed]: site(0x45ec,o,'callvirt',0x600299c)
site(0x45ec,0x502,'callvirt',0x60024b3)
site(0x45f6,1,'call',0x60024b3)
for o in [0x5e,0x70]: site(0x45f6,o,'ldfld',0x4000add if o==0x5e else 0x4000adc)
const(0x45f6,0xa1,'ldc.r8',0.6); const(0x45f6,0xba,'ldc.r8',0.3)
const(0x24cf,6,'ldc.i4',148)
assert j['enums']['0x200021a']['values']['Fade']==2
assert j['enums']['0x200021a']['values']['Scale']==4
assert j['enums']['0x2000b57']['values']['Hidden']==8
for o,t in [(0xf,0x4001866),(0x16,0x400186a),(0x1e,0x400186e),(0x26,0x400186f)]:
    site(0x2995,o,'stfld',t)

# Slider head is a circle subclass, shares instances during postprocessing.
assert j['ancestry_declarations']['0x20006f6']['extends_rid']==0x94c
site(0x351a,0xa,'call',0x60045eb)
site(0x143a,0x54,'newobj',0x600351a); site(0x143a,0x59,'stfld',0x4000bdf)
for o in [0x12,0x1f]: ar(0x143a,o,'conv.i4')
const(0x45ed,0,'ldc.i4',2064519436); const(0x351c,0,'ldc.i4',2064459632)
for o,t in [(0xbf7,0x4001531),(0xc02,0x4001530),(0xc17,0x4001531)]:
    site(0x144a,o,'ldfld',t)
site(0x144a,0xc28,'stfld',0x4002ced)
site(0x144a,0x1575,'callvirt',0x2b000006)

# Registration, loading queue and time-equality admission.
assert j['registration_method_spec']['method']=='0x6002ce9'
site(0x28d2,0xf8,'callvirt',0x2b000006); site(0x2ce9,0x1f,'call',0x6002ce6)
site(0x28f5,0x43,'callvirt',0x6002cdb); site(0x28f5,0x269,'callvirt',0x60028e9)
site(0x28f5,0x288,'callvirt',0x6002cdb)
site(0x2cdb,0x50,'stfld',0x4001a63)
site(0x473b,0xc,'ldflda',0x400186e)
branch(0x2cf0,0x31e,'bgt.s',[0x366])
site(0x2cf0,0x34c,'call',0x6002ce6)
site(0x2cf0,0x400,'callvirt',0x6004738); site(0x2cf0,0x4e1,'callvirt',0x6004737)

# Clock, inclusive active endpoints and first-per-property writes.
site(0x4736,8,'ldsfld',0x4002358)
site(0x4518,0x11,'call',0x6004519)
branch(0x4519,0xc4,'bgt',[0x57e]); branch(0x4519,0xff,'blt',[0x57e])
site(0x4519,0x1fa,'call',0x6002d71); site(0x4519,0x1ff,'stfld',0x4002cf5)
site(0x4519,0x33c,'call',0x6002d71); site(0x4519,0x341,'stfld',0x4002cf4)
site(0x4519,0x893,'stfld',0x4002cf5); site(0x4519,0x913,'stfld',0x4002cf4)
site(0x4519,0xb93,'stfld',0x4002cf5); site(0x4519,0xc13,'stfld',0x4002cf4)
site(0x2d71,0x25,'call',0x6002d74)
for o,op in [(0xc4,'div'),(0xc5,'mul'),(0xc7,'add')]: ar(0x2d74,o,op)

# Draw-stage linkage and exact abstract-operation boundary.
site(0x46a6,0xe4,'callvirt',0xa001863)
site(0x46fe,0xb,'callvirt',0x6004416)
site(0x2287,0x1b7,'callvirt',0x6002288)
site(0x2288,0x11,'callvirt',0x60028e7); site(0x28e7,6,'callvirt',0x6002cf0)
site(0x4518,0x2e,'ldfld',0x4002cf5); site(0x4518,0x3e0,'ldfld',0x4002cf4)
ar(0x4518,0xb6b,'conv.u1')
site(0x451a,0x14c,'ldfld',0x4002b6d); site(0x451a,0x152,'ldfld',0x4002b8d)
site(0x451a,0x172,'callvirt',0x60031f5)
assert m['0x60031f5']['abstract'] and m['0x60031f5']['rva']=='0x0'
site(0x44c8,1,'call',0x6004518); site(0x1541,1,'call',0x6000949)
site(0x949,1,'call',0x6004518)

shared=0
for oldpath in sorted((Path(__file__).parents[1]/'evidence').glob('0[4-7]/*.json')):
    old=json.loads(oldpath.read_text()).get('methods',{})
    for t,r in m.items():
        if t in old and 'body_sha256' in r and 'body_sha256' in old[t]:
            assert r['body_sha256']==old[t]['body_sha256'],(str(oldpath),t)
            shared+=1
print(f'PASS: 59 original methods; component/queue/transform/draw sites; {shared} historical body hashes; 19659 decoded bodies, 0 errors; no runtime/pixel tests')
