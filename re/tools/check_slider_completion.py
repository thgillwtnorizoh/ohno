#!/usr/bin/env python3
"""E17 original-instruction checks, never a slider completion/scoring engine."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==17 and len(m)==60
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
 assert {x['method']for x in a}=={hex(0x6000000+x)for x in expected},(root,s)
def writes(f):return {(k,o)for k,ss in j['entry_field_sites'].items()for o,p,t in ss if p in ['stfld','stsfld']and t==hex(0x4000000+f)}
slot(0x2af,0x24a9,[0x24a9,0x1465]);slot(0x2af,0x24be,[0x24be])
slot(0x2af,0x24bb,[0x24bb,0x1466]);slot(0x2af,0x24c4,[0x24c4,0x146c])
slot(0xe8,0x28d3,[0x28d3]);slot(0xe8,0x28e4,[0x28e4,0x654])
slot(0x253,0xad2,[0xad2,0x114a]);slot(0x6f6,0x24a9,[0x24a9,0x1884,0x351d])
# Exact slider branch: head timeout then separate inclusive parent end condition.
br(0x28e4,5,'brtrue.s',0x11);br(0x28e4,0xc,'brtrue',0xd6)
br(0x28e4,0x27,'bne.un',0xd6);br(0x28e4,0x3e,'bge.s',0x5d)
site(0x28e4,0x52,'ldfld',0x4000bdf);site(0x28e4,0x57,'callvirt',0x60028d3)
site(0x28e4,0x5e,'callvirt',0x60024be);site(0x24be,1,'ldfld',0x4002524)
site(0x28e4,0x63,'ldsfld',0x4002358);br(0x28e4,0x68,'bgt.s',0xd6)
site(0x28e4,0x6b,'ldfld',0x4001527);br(0x28e4,0x70,'brtrue.s',0xd6)
site(0x28e4,0x74,'callvirt',0x60028d3)
assert not any(t in [hex(0x4000000+x)for x in [0xbf1,0xbf5,0xbf6,0xbf7,0xbf8]]for o,p,t in row(0x28e4)['token_sites'])
# Evaluated first; no head-evaluated read, no local parent-result store or backlog loop.
assert row(0x1465)['body_sha256']=='8a76dd6cd93705ce792566422c7ab2b9cec04a6eab5719518e0f226701e547ab'
site(0x1465,2,'stfld',0x4001527);site(0x1465,8,'callvirt',0x60024c4)
site(0x1465,0x13,'ldfld',0x400152a);br(0x1465,0x19,'ble.s',0x29)
site(0x1465,0x24,'stfld',0x4000bf1);ar(0x1465,0x23,'add')
ar(0x1465,0x2f,'conv.r8');site(0x1465,0x31,'ldfld',0x4000bf8)
co(0x1465,0x3b,'ldc.i4.1');ar(0x1465,0x3c,'add');ar(0x1465,0x3d,'conv.r8');ar(0x1465,0x3e,'div')
co(0x1465,0x41,'ldc.r8',0.0);br(0x1465,0x4a,'ble.un.s',0x52)
site(0x1465,0x4d,'callvirt',0x60024bb);site(0x1465,0x52,'ldsfld',0x40013c3)
site(0x1465,0x5e,'ldfld',0x400198e);ar(0x1465,0x64,'cgt.un');br(0x1465,0x66,'brfalse.s',0xc2)
for o,op,d in [(0x72,'bne.un.s',0x8c),(0x84,'bne.un.s',0x8c),
 (0x96,'blt.un.s',0xb0),(0xa8,'blt.s',0xb0),(0xba,'ble.un.s',0xf8),
 (0xcc,'bne.un.s',0xd4),(0xde,'blt.un.s',0xe6),(0xf0,'ble.un.s',0xf8)]:br(0x1465,o,op,d)
for o,v in [(0x7f,1024),(0x86,1024),(0xa3,512),(0xaa,512),(0xbc,256),(0xce,1024),(0xe0,512),(0xf2,256),(0xf8,-131072)]:co(0x1465,o,'ldc.i4',v)
co(0x1465,0x69,'ldc.r8',1.0);co(0x1465,0x8d,'ldc.r8',0.5)
assert row(0x1465)['array_sites']==[] and row(0x1465)['exception_regions']==[]
assert not any(p=='ldfld'and t==hex(0x4001527)for o,p,t in row(0x1465)['token_sites'])
assert not any(t in [hex(0x4000000+x)for x in [0xbf5,0xbf6,0xbf7,0x2527,0x2358]]for o,p,t in row(0x1465)['token_sites'])
assert not any(o>d for o,p,ds in row(0x1465)['branches']for d in ds)
assert writes(0xbf1)=={('0x6001465',0x24),('0x600146f',0x2d1)}
assert writes(0xbf5)=={('0x600146f',0x31e)}
assert writes(0xbf3)=={('0x600146d',0x20d),('0x600146e',0xfb)}
for r in [0x1465,0x28d3,0x1466,0x1469,0x146a,0x146b,0x146c,0x3b53,0x3ba8,0x229c,0x114a,0xad2]:
 assert not any(p in ['stfld','stsfld']and t==hex(0x400152a)for o,p,t in row(r)['token_sites']),hex(r)
site(0x146c,0,'call',0x6003b53);co(0x1466,1,'ldc.i4.0');site(0x1466,0x28,'call',0x6001469)
br(0x1469,0x14,'brfalse.s',0x82);site(0x1469,0x84,'call',0x600146a)
# Manager result remains a local, with existing conversion/group decoration/event.
br(0x28d3,6,'brfalse.s',0xa);site(0x28d3,0xc,'stfld',0x40017ff)
site(0x28d3,0x12,'callvirt',0x60024a9);site(0x28d3,0x33,'stfld',0x40017f8)
co(0x28d3,0x59,'ldc.i4',16384);br(0x28d3,0x7e,'ble.un.s',0x8b)
br(0x28d3,0x89,'bgt.un.s',0x91);co(0x28d3,0x8b,'ldc.i4',-131072)
co(0x28d3,0x113,'ldc.i4.s',50);co(0x28d3,0x13a,'ldc.i4.s',100);co(0x28d3,0x185,'ldc.i4',300)
for o,v in [(0x301,4),(0x31f,2),(0x325,1)]:co(0x28d3,o,'ldc.i4.'+str(v))
site(0x28d3,0xa40,'callvirt',0x600291a);site(0x229c,0xf,'callvirt',0x6000ad2)
site(0x114a,9,'ldfld',0x4001527);site(0x114a,0x22,'call',0x6000ad2)
assert j['result_enum_fields']==[{'token':'0x4001451','name':'value__','signature':'0608','literal':False}]
# No catch-up was inserted in object update or manager result handling.
for r in [0x1449,0x28e4,0x28d3]:
 assert not any(p in ['stfld','stsfld']and t in ['0x4000bf1','0x4000bf5']for o,p,t in row(r)['token_sites'])
 assert not any(op.startswith('stelem')for off,op in row(r)['array_sites'])
site(0x28e3,0x17,'callvirt',0x60024b2);site(0x28e3,0x1e,'callvirt',0x60028e4)
site(0x226b,0x94e,'call',0x6002275);site(0x226b,0x959,'callvirt',0x60028e2)
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if f.parent.name=='17':continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
   shared+=1
print(f'PASS: 60 methods; inclusive parent completion, no pending drain, aggregate ratio/head branch, return-versus-field distinction, manager handoff/order; {shared} historical body hashes; no original or surrogate gameplay execution')
