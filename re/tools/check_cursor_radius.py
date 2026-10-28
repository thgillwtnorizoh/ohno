#!/usr/bin/env python3
"""E13 instruction/metadata checks only; no coordinate or hit-test execution."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==13 and len(m)==68
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op,hex(t))
def branch(r,o,op,d):assert [o,op,[d]]in row(r)['branches'],(hex(r),hex(o),op,hex(d))
def const(r,o,op,v=None):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def slot(root,s,expected):
 rows=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
 assert {r['method']for r in rows}=={hex(0x6000000+x)for x in expected},(root,s)
def writes(f):return {(k,o)for k,ss in j['spatial_field_sites'].items()for o,p,t in ss if p in ['stfld','stsfld']and t==hex(0x4000000+f)}
slot(0x94c,0x24c7,[0x24c7]);slot(0xe8,0x1259,[0x1259,0x28b2]);slot(0xe8,0x28be,[0x28be])
slot(0x508,0x17a7,[0x17a7,0x270b])
assert not row(0xff1)['virtual'] and not row(0xfea)['virtual']
assert j['fields']['0x4002a5c']['signature']=='0611aa08'
assert j['fields']['0x4000ada']['signature']==j['fields']['0x40009cc']['signature']==j['fields']['0x40009cd']['signature']=='060c'
# Selected provider -> stored pointer before key polling; no event position captured.
site(0x11a3,0x55,'callvirt',0x60017a7);site(0x11a3,0xb2,'call',0x6001192)
site(0x1192,1,'callvirt',0x6001e12);site(0x1192,6,'call',0x6001193)
site(0x1193,1,'call',0x600119c);site(0x119c,1,'stsfld',0x4000a89)
site(0x119b,0,'ldsfld',0x4000a89);site(0x43a0,0x13f,'call',0x600119b)
site(0x43a0,0x144,'stsfld',0x4002a5c);site(0x43a0,0x1d0,'call',0x6001333)
assert writes(0x2a5c)=={('0x60043a0',0x144),('0x60043a1',0x881),('0x60043b2',0x26)}
assert {r['method']for r in j['provider_position_declarations']}=={hex(0x6000000+i)for i in [0x1e12,0x270d,0x6de,0x9f2,0xa56,0xdb1]}
# Conditional Windows-position provider: client conversion and viewport-origin subtraction.
site(0x270b,0,'call',0x6001196);site(0x270d,1,'ldfld',0x40016b7)
site(0x1196,5,'call',0xa0009d4);site(0x1196,0xa,'callvirt',0xa00063c)
site(0x1196,0x10,'call',0x6004695);ar(0x1196,0x23,'sub');ar(0x1196,0x31,'sub')
site(0x4695,5,'call',0xa000eb2);site(0x4695,0xf,'call',0xa000397)
site(0x4695,0x19,'call',0xa000ebc);ar(0x4695,0x1e,'sub')
site(0x1e45,0x13,'stsfld',0x4001175);site(0x1e45,0x186,'stsfld',0x4001175)
site(0x1e45,0xba,'call',0xa000eb4);site(0x1e45,0x1b3,'call',0xa000eb4)
site(0x468a,0x41,'stsfld',0x4002c9d);site(0x468a,0x67,'stsfld',0x4002c9d)
# Live mapping: subtraction stored to a vector, then reciprocal multiplication.
site(0x2295,0x6f,'ldsfld',0x4002a5c);site(0x2295,0x74,'callvirt',0x6000ff1)
site(0x2295,0x7b,'ldsfld',0x4002a7b)
site(0xff1,2,'call',0x6000fe6);site(0xff1,7,'call',0x6004efe)
site(0xff1,0xd,'call',0x6000fea);site(0xff1,0x12,'call',0x6004f03)
site(0xfe6,1,'ldfld',0x40009ce);site(0xfea,1,'call',0x6000fe4)
const(0xfea,6,'ldc.r4',384.0);ar(0xfea,0xb,'div')
ar(0x4efe,0xe,'sub');site(0x4efe,0xf,'stfld',0x4003109)
ar(0x4efe,0x22,'sub');site(0x4efe,0x23,'stfld',0x400310a)
const(0x4f03,0,'ldc.r4',1.0);ar(0x4f03,6,'div');ar(0x4f03,0x11,'mul');ar(0x4f03,0x20,'mul')
# Layout recompute and ordinary vertical correction; equal-factor no-force return.
branch(0xfed,7,'bne.un.s',0xd);branch(0xfed,0xa,'brtrue.s',0xd)
site(0xfed,0x23,'stfld',0x40009d0);const(0xfed,0x30,'ldc.r4',512.0)
site(0xfed,0x3b,'callvirt',0x60018ff);const(0xfed,0x49,'ldc.r4',384.0)
site(0xfed,0x5c,'call',0x6000fe5);const(0xfed,0x6b,'ldc.r4',-16.0)
ar(0xfed,0x91,'conv.r4');ar(0xfed,0xab,'conv.r4')
site(0xfed,0xbf,'call',0x6000fe7);site(0xfed,0xf7,'call',0x60028a0)
site(0x28a0,0x27,'callvirt',0x60028be);site(0x28f5,0x7f,'callvirt',0x60028be)
site(0x18ff,1,'ldfld',0x4000e6e);const(0x18ff,7,'ldc.r4',480.0)
assert writes(0x9ce)=={('0x6000fe7',2)}
assert writes(0x9cc)=={('0x6000fe3',2)} and writes(0x9cd)=={('0x6000fe5',2)}
site(0x4668,0x23d,'newobj',0x6000fe1);site(0x4668,0x242,'stsfld',0x4002c64)
site(0x46b0,0x224,'callvirt',0x6000fed);site(0x4687,0x576,'callvirt',0x6000fed)
# CS-like source; ordinary map parsing branch and binary-cache input remain distinct.
const(0x1387,0xc,'ldc.r4',5.0);site(0x1387,0x11,'stfld',0x4000b53)
branch(0x1bc1,0xbae,'blt.s',0xc0d);site(0x1bc1,0xc03,'stfld',0x4000b53)
site(0x1bc1,0xc22,'stfld',0x4000b53);site(0x1bf2,0xce,'callvirt',0xa0000cc)
site(0x1bf2,0xd3,'stfld',0x4000b53)
# Precise size scalar and radius order, HR factor1.3 versus timing factor1.4.
site(0x28be,0x139,'callvirt',0x6000fe2);const(0x28be,0x13e,'ldc.r4',8.0)
ar(0x28be,0x143,'div');ar(0x28be,0x144,'conv.r8')
const(0x28be,0x14e,'ldc.r8',0.699999988079071)
site(0x28be,0x15e,'ldfld',0x4000b53);site(0x28be,0x164,'callvirt',0x6001259)
for o,op in [(0x169,'mul'),(0x16a,'sub'),(0x16b,'mul'),(0x16c,'conv.r4')]:ar(0x28be,o,op)
site(0x28be,0x16d,'stfld',0x40017f1);const(0x28be,0x179,'ldc.r4',2.0)
ar(0x28be,0x17e,'div');site(0x28be,0x184,'callvirt',0x6000fea);ar(0x28be,0x189,'div')
const(0x28be,0x18a,'ldc.r4',1.0004099607467651);ar(0x28be,0x18f,'mul');site(0x28be,0x190,'stfld',0x4000ada)
assert [0x14e,'ldc.r8','000000606666e63f']in row(0x28be)['float_constant_bits']
assert [0x18a,'ldc.r4','6f0d803f']in row(0x28be)['float_constant_bits']
const(0x28b2,1,'ldc.r8',1.3);site(0x28b2,0x15,'call',0x60028b0)
ar(0x28b2,0x23,'sub');ar(0x28b2,0x2d,'div')
assert writes(0xada)=={('0x60028be',0x190),('0x60020d5',0x74)}
# Existing candidates, time/evaluated checks, then ordered squared-distance equality.
site(0x2295,0xbb,'callvirt',0x60028c8);const(0x2295,0x9b,'ldc.r4',100.0)
site(0x28c8,5,'ldfld',0x4000ada);site(0x28c8,0xa,'call',0x60028c9)
site(0x28c9,0x15,'ldfld',0x40017fd);site(0x28c9,0x2f,'ldfld',0x400153b)
site(0x28c9,0x3b,'callvirt',0x60024c7);branch(0x28c9,0x44,'leave.s',0x61)
branch(0x24c7,1,'brtrue.s',0xb);branch(0x24c7,0x22,'bgt.s',0x72)
branch(0x24c7,0x3b,'blt.s',0x72);site(0x24c7,0x3e,'ldfld',0x4001527)
site(0x24c7,0x47,'ldfld',0x400252c);site(0x24c7,0x4c,'call',0x6004ecd)
ar(0x24c7,0x53,'mul');branch(0x24c7,0x54,'ble.s',0x70);branch(0x24c7,0x57,'brtrue.s',0x6e)
for o,op in [(0xc,'sub'),(0x19,'sub'),(0x1c,'mul'),(0x1f,'mul'),(0x20,'add')]:ar(0x4ecd,o,op)
assert row(0x4ecd)['signature']=='00020c11aa0811aa08'
site(0x45ec,9,'stfld',0x400252c);site(0x24b0,2,'stfld',0x400252c)
# E12 correction preserved: post-refresh branches rejoin before object update.
site(0x226b,0x959,'callvirt',0x60028e2);branch(0x226b,0x972,'brfalse.s',0x9c9)
branch(0x226b,0x9a6,'brfalse.s',0x9c9);site(0x226b,0x9cf,'callvirt',0x60028e3)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if f.parent.name=='13':continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
   shared+=1
print(f'PASS: 68 methods; provider boundary/client origin, offset/reciprocal mapping, layout/radius preparation, strict ordinary slot identity and ordered spatial comparison; {shared} historical hashes; no coordinate/judge execution')
