#!/usr/bin/env python3
"""Check original E11 instruction evidence. Never evaluates input or judgement."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==11 and len(m)==76
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def site(r,o,op,t):assert [o,op,hex(t)]in m[hex(0x6000000+r)]['token_sites'],(hex(r),hex(o),op,hex(t))
def branch(r,o,op,d):assert [o,op,[d]]in m[hex(0x6000000+r)]['branches'],(hex(r),hex(o),op,hex(d))
def const(r,o,op,v):assert [o,op,v]in m[hex(0x6000000+r)]['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in m[hex(0x6000000+r)]['arithmetic_sites'],(hex(r),hex(o),op)
def slot(root,s,expected):
 rows=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
 assert {r['method']for r in rows}=={hex(0x6000000+x)for x in expected},(root,s)
def writes(f,op):return {(k,o)for k,ss in j['input_field_sites'].items()for o,p,t in ss if p==op and t==hex(0x4000000+f)}
slot(0x94c,0x24a9,[0x24a9,0x1884]);slot(0x94c,0x24c7,[0x24c7]);slot(0x94c,0x24b6,[0x24b6,0x45fc])
slot(0x253,0xad9,[0xad9]);slot(0x253,0xaab,[0xaab]);slot(0x253,0xaaf,[0xaaf,0x1140])
slot(0xe8,0x125a,[0x125a,0x28b3]);slot(0xe8,0x28c9,[]);slot(0xe8,0x28d3,[0x28d3])
assert not m['0x60028c9']['virtual'] and not m['0x60028c8']['virtual']
assert j['native_imports']==[{'method':'0x6001332','module':'user32.dll','name':'GetKeyboardState','signature':'0001021d05'}]
site(0x1333,0x14,'call',0x6001332);const(0x1333,8,'ldc.i4',256);const(0x1333,0x1f,'ldc.i4',128)
site(0x1333,0x2a,'call',0x60020c3);site(0x20c5,2,'call',0x60020c2)
assert j['enums']['0x2000a7a']['values']=={'X':88,'Z':90}
assert j['enums']['0x2000b49']['values']['OsuLeft']==2 and j['enums']['0x2000b49']['values']['OsuRight']==3
assert j['enums']['0x2000b4a']['values']['Default']==2
const(0x2c54,0x1b,'ldc.i4.2',None);const(0x2c54,0x27,'ldc.i4.3',None)
site(0x43a7,0x6d,'call',0x6002c54);site(0x43a7,0x81,'call',0x60020c5)
site(0x43a7,0xae,'call',0x60020c5);site(0x43a7,0xc8,'stsfld',0x4002a6b)
site(0x43a7,0x1bc,'stsfld',0x4002a67);site(0x43a7,0x1e5,'stsfld',0x4002a83)
assert writes(0x2a71,'stsfld')=={('0x6000aaf',0x3d)}
const(0xaab,0,'ldc.i4.0',None)
# Shared physical-button and configured-key state converge before queued press dispatch.
site(0x1191,0x32,'ldftn',0x60011b3);site(0x1191,0x3d,'callvirt',0xa0009d0)
site(0x1191,0x48,'ldftn',0x60011b2);site(0x1191,0x53,'callvirt',0xa0008ed)
const(0x11b3,0x39,'ldc.i4.1',None);const(0x11b2,0x39,'ldc.i4.0',None)
site(0x11b3,0x3a,'call',0x6001188);site(0x1187,0,'ldsfld',0x4000a82)
site(0x1188,5,'stsfld',0x4000a82);site(0x1188,0xb,'stsfld',0x4000a72)
site(0x11b4,0x3c,'ldsfld',0x4000a72);site(0x11b4,0x41,'stsfld',0x4000a82)
site(0x43a0,0x7fc,'call',0x60011b4);branch(0x43a0,0x801,'brtrue.s',0x819)
site(0x43a6,0,'call',0x6001187);site(0x43a6,5,'stsfld',0x4002a67)
# Clock publication precedes polling/queued presses, which precede component refresh.
site(0x43a0,0xca,'call',0x6003b25);site(0x3b25,0x48,'call',0x6003b24)
site(0x43a0,0x1d0,'call',0x6001333);site(0x43a0,0x1d5,'stsfld',0x4002a64)
site(0x43a0,0x23e,'call',0x60043a7)
site(0x43a0,0x573,'ldftn',0x60011a9);site(0x43a0,0x590,'ldftn',0x60011a9)
site(0x43a0,0x77d,'callvirt',0x6003350);site(0x43a0,0x790,'callvirt',0x6003350)
site(0x43a0,0x79a,'stsfld',0x4002a6c);site(0x43a0,0x7e6,'stsfld',0x4002a65)
site(0x226b,0x959,'callvirt',0x60028e2)
# Parameterless press delegate -> numeric event7; release event6 is distinct.
const(0x11a9,0xe0,'ldc.i4.7',None);site(0x11a9,0xe1,'call',0x60043b5)
const(0x11ac,0,'ldc.i4.6',None);site(0x11ac,1,'call',0x60043b5)
site(0x2246,0x16,'ldftn',0x6002295);const(0x2246,0x14,'ldc.i4.7',None)
site(0x2246,0x23,'call',0x60043b6);site(0x223b,0x42a,'call',0x6002246)
site(0x43b6,0xb6,'callvirt',0x6002d46)
site(0x2d48,0x39,'callvirt',0xa000304);const(0x2d48,0x33,'ldc.i4.2',None)
branch(0x2d48,6,'brtrue.s',0xa);branch(0x2d48,0x43,'brfalse.s',0x47)
# Play early guards and candidate list order/time-before-space selection.
branch(0x2295,5,'brtrue.s',9);site(0x2295,0x10,'ldsfld',0x4001373)
site(0x2295,0x5a,'ldsfld',0x4001396)
const(0x2295,0xba,'ldc.i4.1',None);site(0x2295,0xbb,'callvirt',0x60028c8)
site(0x28c8,5,'ldfld',0x4000ada);site(0x28c8,0xa,'call',0x60028c9)
site(0x28c9,0x15,'ldfld',0x40017fd);site(0x28c9,0x2f,'ldfld',0x400153b)
site(0x28c9,0x3b,'callvirt',0x60024c7);branch(0x28c9,0x40,'brfalse.s',0x46)
site(0x249b,0x1f,'stfld',0x400153b)
branch(0x24c7,1,'brtrue.s',0xb);site(0x24c7,4,'callvirt',0x60024b6)
site(0x24c7,0xc,'ldfld',0x4002523);site(0x24c7,0x17,'ldfld',0x4000adb)
site(0x24c7,0x1d,'ldsfld',0x4002358);branch(0x24c7,0x22,'bgt.s',0x72)
site(0x24c7,0x30,'ldfld',0x4000adc);site(0x24c7,0x36,'ldsfld',0x4002358)
branch(0x24c7,0x3b,'blt.s',0x72);site(0x24c7,0x3e,'ldfld',0x4001527)
site(0x24c7,0x4c,'call',0x6004ecd);branch(0x24c7,0x54,'ble.s',0x70)
# Ordinary inherited admission: object ordering, then strict400/200 bound.
site(0x2295,0xe0,'callvirt',0x6000ad9);site(0xad9,0x15,'callvirt',0xa000665)
const(0xad9,0xa0,'ldc.i4.3',None);branch(0xad9,0xa8,'bge.s',0xae)
site(0xad9,0xd8,'ldfld',0x4002523);site(0xad9,0xdd,'ldsfld',0x4002358)
site(0xad9,0xe3,'call',0xa00038e);const(0xad9,0xe8,'ldc.i4',400);const(0xad9,0xd1,'ldc.i4',200)
branch(0xad9,0xef,'blt.s',0xf3)
site(0x2295,0x100,'callvirt',0x60024c8);site(0x2295,0x11a,'callvirt',0x60028d3)
site(0x28d3,1,'ldfld',0x4001527);site(0x28d3,0xc,'stfld',0x40017ff)
site(0x28d3,0x12,'callvirt',0x60024a9)
# First circle comparison: flag write, fresh T read, Abs(int), strict threshold.
site(0x1884,2,'stfld',0x4001527);site(0x1884,7,'ldsfld',0x4002358)
site(0x1884,0xd,'ldfld',0x4002523);ar(0x1884,0x12,'sub');site(0x1884,0x13,'call',0xa00038e)
site(0x1884,0x20,'ldfld',0x4000ade);branch(0x1884,0x25,'bge.s',0x34)
const(0x1884,0x28,'ldc.i4',1024);site(0x1884,0x2d,'stfld',0x400152a)
for r in [0xad9,0x24c7,0x1884]:
 refs={t for _,_,t in m[hex(0x6000000+r)]['token_sites']}
 assert not refs.intersection({'0x400235b','0x400235c','0x4002359','0x6003b24','0x4000a77','0x40013df'})
for r in [0x2295,0x28c9,0x24c7,0xad9,0x1884]:
 assert all(t!='0x40013d8'for _,_,t in m[hex(0x6000000+r)]['token_sites'])
# Prepared first-window width; no invented modern hit-window formula.
site(0x28be,0xd3,'ldfld',0x4000b55)
for o,v in [(0xd9,80.0),(0xe2,50.0),(0xeb,20.0)]:const(0x28be,o,'ldc.r8',v)
site(0x28be,0xf4,'callvirt',0x600125a);ar(0x28be,0xf9,'conv.i4');site(0x28be,0xfa,'stfld',0x4000ade)
site(0x28b3,0x15,'call',0x60028b0);const(0x28b3,1,'ldc.r8',1.4)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if f.parent.name=='11':continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
   shared+=1
print(f'PASS: 76 methods; keyboard polling/mouse latch convergence, timestamp-free dispatch, clock/input/refresh order, Play/selection/admission and first strict circle comparison; {shared} historical body-hash comparisons; no input/judge execution')
