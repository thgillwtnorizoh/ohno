#!/usr/bin/env python3
"""Assert E12 original instruction/metadata evidence; no judgement simulation."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==12 and len(m)==48
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
slot(0x94c,0x24a9,[0x24a9,0x1884]);slot(0x94c,0x24b2,[0x24b2])
slot(0x94c,0x24bb,[0x24bb]);slot(0x94c,0x24b4,[0x24b4,0x45f7])
slot(0x94c,0x24b3,[0x24b3,0x45f6])
slot(0xe8,0x28d3,[0x28d3]);slot(0xe8,0x28e3,[0x28e3,0x653])
slot(0xe8,0x28e4,[0x28e4,0x654]);slot(0xe8,0x28bb,[0x28bb])
slot(0xe8,0x125a,[0x125a,0x28b3]);slot(0xe8,0x28d4,[0x28d4])
assert row(0x24b2)['code_bytes']==row(0x28d4)['code_bytes']==1
# Full circle classification: signed strict widths in branch priority order.
site(0x1884,2,'stfld',0x4001527);site(0x1884,7,'ldsfld',0x4002358)
site(0x1884,0xd,'ldfld',0x4002523);ar(0x1884,0x12,'sub');site(0x1884,0x13,'call',0xa00038e)
for o,f,bo,d in [(0x20,0xade,0x25,0x34),(0x3b,0xadd,0x40,0x4f),(0x56,0xadc,0x5b,0x6a)]:
 site(0x1884,o,'ldfld',0x4000000+f);branch(0x1884,bo,'bge.s',d)
for o,v,write in [(0x28,1024,0x2d),(0x43,512,0x48),(0x5e,256,0x63),(0x6b,-131072,0x70)]:
 const(0x1884,o,'ldc.i4',v);site(0x1884,write,'stfld',0x400152a)
branch(0x1884,0x7c,'ble.s',0x84);site(0x1884,0x7f,'callvirt',0x60024bb)
ar(0x1884,0x8c,'cgt');site(0x1884,0x8e,'callvirt',0x60024b4)
site(0x1884,0x94,'ldfld',0x400152a);assert row(0x1884)['returns']==[0x99]
site(0x45f7,2,'call',0x60024b4);site(0x24b3,0x2c,'stfld',0x4001527)
# Widths are separately mapped and integer-converted at configuration.
for offsets,values,call,convert,write,field in [
 ([0x71,0x7a,0x83],[200.0,150.0,100.0],0x8c,0x91,0x92,0xadc),
 ([0xa5,0xae,0xb7],[140.0,100.0,60.0],0xc0,0xc5,0xc6,0xadd),
 ([0xd9,0xe2,0xeb],[80.0,50.0,20.0],0xf4,0xf9,0xfa,0xade)]:
 for o,v in zip(offsets,values):const(0x28be,o,'ldc.r8',v)
 site(0x28be,call,'callvirt',0x600125a);ar(0x28be,convert,'conv.i4')
 site(0x28be,write,'stfld',0x4000000+field)
site(0x28b3,0x15,'call',0x60028b0);const(0x28b3,1,'ldc.r8',1.4)
# Manager entry, index bookkeeping, Perfect conversion and numeric identities.
site(0x28d3,1,'ldfld',0x4001527);branch(0x28d3,6,'brfalse.s',0xa)
site(0x28d3,0xc,'stfld',0x40017ff);site(0x28d3,0x12,'callvirt',0x60024a9)
site(0x28d3,0x1f,'callvirt',0xa0004be);site(0x28d3,0x33,'stfld',0x40017f8)
branch(0x28d3,0x39,'brtrue.s',0x3d)
site(0x28d3,0x47,'ldsfld',0x40013c3);const(0x28d3,0x59,'ldc.i4',16384)
const(0x28d3,0x6d,'ldc.i4',520095494);branch(0x28d3,0x7e,'ble.un.s',0x8b)
branch(0x28d3,0x89,'bgt.un.s',0x91);const(0x28d3,0x8b,'ldc.i4',-131072)
const(0x28d3,0xc3,'ldc.i4',256);branch(0x28d3,0xc8,'beq.s',0x107)
const(0x28d3,0x113,'ldc.i4.s',50)
const(0x28d3,0xcc,'ldc.i4',512);branch(0x28d3,0xd1,'beq.s',0x12e)
const(0x28d3,0x13a,'ldc.i4.s',100)
assert [0xe9,'switch',[0x179,0x1ee,0x1c7,0x1ee,0x1a0]]in row(0x28d3)['branches']
const(0x28d3,0x185,'ldc.i4',300)
site(0x28d3,0x234,'stfld',0x40017f7);site(0x28d3,0x244,'stfld',0x40017f6)
site(0x28d3,0x24a,'callvirt',0x60028bb);const(0x28bb,0,'ldc.i4.1')
site(0x28d3,0x286,'callvirt',0x6003e56);site(0x28d3,0x2b1,'ldfld',0x4001527)
branch(0x28d3,0x2dc+2,'ble.s',0x328)
for o,v in [(0x301,4),(0x31f,2),(0x325,1)]:const(0x28d3,o,'ldc.i4.'+str(v));ar(0x28d3,o+1,'or')
site(0x28d3,0x32a,'stfld',0x40017f7);site(0x28d3,0x331,'stfld',0x40017f6)
# Common event handoff, already registered by Play. Helpers in between stay opaque.
site(0x2246,0x63,'ldftn',0x600229c);site(0x2246,0x6e,'callvirt',0x60028c2)
site(0x28d3,0xa2f,'ldfld',0x4001804);site(0x28d3,0xa37,'ldfld',0x4001804)
site(0x28d3,0xa40,'callvirt',0x600291a);site(0x28d3,0xa54,'callvirt',0x6002f17)
site(0x229c,2,'stfld',0x40013ea);site(0x229c,0xf,'callvirt',0x6000ad2)
assert j['fields']['0x400152a']['signature']=='06119220'
e=j['enums']['0x2000488'];assert e['extends_name']=='System.Enum' and e['values']=={}
assert [(f['name'],f['signature'],f['literal'])for f in e['fields']]==[('value__','0608',False)]
assert j['enums']['0x2000b57']['values']['Perfect']==16384
assert j['result_interface']['invoke_signature']=='2003011192200e12930c'
assert j['subscription_methodspec']=={'token':'0x2b00010d','method_table':'MemberRef',
 'method_rid':0x2fe,'owner':'System.Threading.Interlocked','name':'CompareExchange',
 'signature':'1001031e00101e001e001e00','instantiation':'0a01129574'}
# Update dispatch resolves through ordinary forwarding overrides. Timeout not opacity.
site(0x226b,0x959,'callvirt',0x60028e2);site(0x226b,0x9cf,'callvirt',0x60028e3)
# Correct earlier interpretation: these gates rejoin before M28e3, not after it.
for o,op,d in [(0x963,'brfalse.s',0x974),(0x96a,'brtrue.s',0x974),
 (0x972,'brfalse.s',0x9c9),(0x97f,'brfalse.s',0x9a8),(0x9a6,'brfalse.s',0x9c9)]:
 branch(0x226b,o,op,d)
site(0x653,1,'call',0x60028e3);site(0x654,2,'call',0x60028e4)
site(0x28e3,1,'ldfld',0x40017fd);site(0x28e3,0x17,'callvirt',0x60024b2)
site(0x28e3,0x1e,'callvirt',0x60028e4)
site(0x28e4,0,'ldsfld',0x4002a90);site(0x28e4,7,'call',0x6004390)
branch(0x28e4,0xc,'brtrue',0xd6)
site(0x28e4,0xab,'ldfld',0x4002524);site(0x28e4,0xb1,'ldfld',0x4000adc)
ar(0x28e4,0xb6,'add');site(0x28e4,0xb7,'ldsfld',0x4002358)
branch(0x28e4,0xbc,'bge.s',0xd6);site(0x28e4,0xbf,'ldfld',0x4001527)
branch(0x28e4,0xc4,'brtrue.s',0xd6);const(0x28e4,0xcb,'ldc.i4.2')
branch(0x28e4,0xcc,'bne.un.s',0xd6);site(0x28e4,0xd0,'callvirt',0x60028d3)
for r in [0x1884,0x28d3,0x28e4]:
 refs={t for _,_,t in row(r)['token_sites']}
 assert not refs.intersection({'0x400235b','0x400235c','0x4002359','0x6003b24'})
 if r!=0x1884:
  assert all(not(op=='stfld'and t in ['0x4001527','0x400152a'])for _,op,t in row(r)['token_sites'])
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if f.parent.name=='12':continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
   shared+=1
print(f'PASS: 48 methods; strict classification/widths, ordinary update slots/timeout, manager conversion/bookkeeping/result event; {shared} historical body-hash comparisons; no judgement execution')
