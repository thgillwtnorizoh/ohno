#!/usr/bin/env python3
"""Assert E10 original metadata/instruction facts; never simulate a gameplay clock."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==10 and len(m)==103
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def site(r,o,op,t):assert [o,op,hex(t)] in m[hex(0x6000000+r)]['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,[d]] in m[hex(0x6000000+r)]['branches'],(hex(r),hex(o))
def const(r,o,op,v):assert [o,op,v] in m[hex(0x6000000+r)]['constants'],(hex(r),hex(o))
def ar(r,o,op):assert [o,op] in m[hex(0x6000000+r)]['arithmetic_sites'],(hex(r),hex(o),op)
def accesses(t,op):return {(k,o)for k,ss in j['clock_field_sites'].items()for o,p,v in ss if v==hex(t) and p==op}
assert accesses(0x4002358,'stsfld')=={('0x6003b08',0x47),('0x6003b24',0x3bc),('0x6003b24',0x3c8),
 ('0x6003b3a',0xa8),('0x6003b4f',6),('0x6003b59',7),('0x6002269',0x1ef),('0x6002269',0x208),('0x60043a1',0x2d8)}
assert accesses(0x4002358,'ldsflda')=={('0x60038a0',0x1f9),('0x600497d',0x185)}
site(0x38a0,0x20d,'call',0xa00011d);site(0x497d,0x18a,'call',0xa00011a)
assert accesses(0x400235c,'stsfld')=={('0x6003b24',0x397)}
assert accesses(0x4002c97,'stsfld')=={('0x60046a6',0x4b),('0x60046a6',0x27f)}
assert accesses(0x4002c71,'stsfld')=={('0x60046a1',0x1af)}
for f in [0x2358,0x235b,0x235c,0x2359,0x235a,0x2353]:assert j['fields'][hex(0x4000000+f)]['signature']=='0608'
for f in [0x2352,0x235d,0x235e,0x2c71,0x2c3d,0x2c3f]:assert j['fields'][hex(0x4000000+f)]['signature']=='060d'
for f in [0x2365,0x2366]:assert j['fields'][hex(0x4000000+f)]['signature']=='060c'
# Clock update order, source sample, draw-epoch gated delta and correction branches.
br(0x3b24,5,'brtrue.s',8);site(0x3b24,0xd,'callvirt',0x6003d63)
site(0x3b24,0xca,'callvirt',0x6003d5c)
br(0x3b24,0x27,'blt.s',0x3b);br(0x3b24,0x39,'ble.s',0x5f)
site(0x3b24,0x58,'callvirt',0x6003d61);site(0x3b24,0x9d,'callvirt',0x6003d5f)
site(0x3b24,0xa8,'callvirt',0x6003d60)
ar(0x3b24,0x113,'div');ar(0x3b24,0x114,'conv.r8')
br(0x3b24,0x122,'beq.s',0x12e);site(0x3b24,0x124,'ldsfld',0x4002c71)
site(0x3b24,0x13d,'stsfld',0x400237d)
site(0x3b24,0x166,'call',0x6002d7c);br(0x3b24,0x186,'bge.un',0x2fe)
const(0x3b24,0x17d,'ldc.r8',60.0);const(0x3b24,0x21c,'ldc.r8',8.0)
const(0x3b24,0x238,'ldc.i4',1500);const(0x3b24,0x24f,'ldc.i4.s',33);const(0x3b24,0x253,'ldc.i4.s',11)
br(0x3b24,0x23d,'blt.s',0x253);br(0x3b24,0x24d,'blt.s',0x253)
br(0x3b24,0x26a,'ble.un.s',0x284);br(0x3b24,0x288,'bge.un.s',0x2ac);br(0x3b24,0x2af,'bge.un.s',0x2b9)
for o in [0x29b,0x2b2,0x2ca,0x2df]:site(0x3b24,o,'stsfld',0x4002352)
site(0x3b24,0x303,'call',0xa000398);ar(0x3b24,0x308,'conv.i4')
const(0x3b24,0x329,'ldc.i4.5',None);const(0x3b24,0x337,'ldc.i4.s',24);const(0x3b24,0x342,'ldc.r8',15.0)
site(0x3b24,0x364,'ldfld',0x4000ff9);site(0x3b24,0x36e,'ldfld',0x4000ff4)
ar(0x3b24,0x373,'add');site(0x3b24,0x37c,'callvirt',0x6003df2)
ar(0x3b24,0x394,'mul');ar(0x3b24,0x396,'conv.i4');br(0x3b24,0x3af,'brfalse.s',0x3c3)
site(0x3b24,0x3d2,'stsfld',0x4002359)
site(0x3b25,0x43,'stsfld',0x400235a);site(0x3b25,0x48,'call',0x6003b24)
site(0x3b25,0x5c,'stsfld',0x4002368);site(0x3b25,0xe4,'stsfld',0x400235d)
# Closed playback hierarchy and resolved source/rate/update slots.
d=j['dispatch']['0x2000803']
assert {r['type']for r in d['family']}=={'0x2000803','0x200086e','0x20008e4'}
for slot,a,b in [(0x3d5c,0x4054,0x4384),(0x3d63,0x4057,0x4388),(0x3d65,0x4058,0x438b)]:
 assert {r['method']for r in d['slots'][hex(0x6000000+slot)]}=={hex(0x6000000+x)for x in [slot,a,b]}
site(0x3b45,0xc8,'call',0x6003d52);site(0x3b45,0xd5,'stsfld',0x4002367)
site(0x3d52,0x45,'call',0x6003d51);site(0x3d52,0x51,'newobj',0x6004378);site(0x3d52,0x5b,'call',0x6003d51)
site(0x4384,1,'call',0x6003d5d);site(0x4384,7,'call',0x6003d5d)
site(0x4384,0xc,'call',0x600312a);site(0x4384,0x11,'call',0x6003112);const(0x4384,0x16,'ldc.r8',1000.0)
assert m['0x6004388']['code_bytes']==1
br(0x4057,0xb,'bne.un.s',0xe);site(0x4057,0x1d,'ldsfld',0x4002c71);site(0x4057,0x3f,'stfld',0x4002616)
imports={r['name']:r for r in j['native_imports']}
assert len(imports)==9 and all(r['module']=='bass.dll'for r in imports.values())
for n in ['BASS_ChannelGetPosition','BASS_ChannelBytes2Seconds','BASS_ChannelPlay','BASS_ChannelPause','BASS_ChannelSetPosition','BASS_ChannelSeconds2Bytes','BASS_ChannelSetAttribute','BASS_ChannelGetAttribute','BASS_ChannelIsActive']:assert n in imports
# Pause/seek/hold, initialization and optional recorded-input override.
site(0x3b3a,0xad,'call',0x6003b24);site(0x3b3a,0xf4,'call',0x6003b24);site(0x3b3a,0x111,'call',0x6003b24)
const(0x3b3a,0xd1,'ldc.i4',200);br(0x3b49,0x3a,'brfalse.s',0x59)
site(0x3b49,0x52,'callvirt',0x6003d61);site(0x3b49,0x6f,'callvirt',0x6003d60)
site(0x2269,0x165,'call',0x6003b4f);site(0x2269,0x160,'stsfld',0x400234d)
site(0x43a1,0x2d3,'ldfld',0x4001310);site(0x43a1,0x2d8,'stsfld',0x4002358)
site(0x2237,0x4b1,'stsfld',0x400136a)
site(0x224c,0x5d,'call',0x6003b38);site(0x224c,0x8b,'call',0x6003b38)
# Original registered host callback: Update -> Draw -> timing-tail; startup seed first.
site(0x4233,0x1a,'ldftn',0x6004241);site(0x4233,0x25,'callvirt',0x6003965)
for o,t in [(0xa,0x4249),(0x10,0x4248),(0x16,0x4246)]:site(0x4241,o,'callvirt',0x6000000+t)
site(0x4247,7,'callvirt',0x6004246);site(0x4247,0x12,'callvirt',0x600396d)
for slot,impl in [(0x4249,0x469e),(0x4248,0x46a6),(0x4246,0x46a7)]:
 rows=j['dispatch']['0x20008c1']['slots'][hex(0x6000000+slot)]
 assert any(r['method']==hex(0x6000000+impl) and not r['new_slot']for r in rows)
site(0x46a7,0x12,'call',0x60046a1);site(0x46a1,0x10,'call',0x60047cd)
site(0x46a1,0xc2,'stsfld',0x4002c3d);site(0x46a1,0xc8,'stsfld',0x4002c3f);site(0x46a1,0xd9,'stsfld',0x4002c3a)
site(0x469e,0x120,'ldftn',0x60046d7);site(0x469e,0x12b,'call',0x60043a0)
site(0x43a0,0xca,'call',0x6003b25);site(0x43a0,0xe0,'call',0x60043a1);site(0x43a0,0x790,'callvirt',0x6003350)
br(0x43a0,0x820,'blt',0xca);const(0x43a0,0x81e,'ldc.i4.s',30)
site(0x46d7,0x14,'ldftn',0x60046fb);site(0x46fb,1,'callvirt',0x6001ab2)
site(0x226b,0x959,'callvirt',0x60028e2);site(0x226b,0x9cf,'callvirt',0x60028e3)
site(0x4519,0x26,'call',0x6004736);site(0x4736,8,'ldsfld',0x4002358)
assert j['enums']['0x2000b57']['values']['DoubleTime']==64
assert j['enums']['0x2000b57']['values']['HalfTime']==256
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('0[2-9]/*.json')):
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
   shared+=1
print(f'PASS: 103 methods; seven direct writer methods, clock branches/conversions, source dispatch/native imports, seek/rate/loading, host update/draw/timing ordering; {shared} historical body-hash comparisons; no clock execution')
