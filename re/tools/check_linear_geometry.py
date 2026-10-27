#!/usr/bin/env python3
"""Check original CIL evidence; implements no slider geometry or consumer."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert len(m)==43 and j['enums']['0x20003bb']['values']['Linear']==2
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def site(r,o,op,t):assert [o,op,hex(t)] in m[hex(0x6000000+r)]['token_sites'],(hex(r),hex(o))
def branch(r,o,op,ds):assert [o,op,ds] in m[hex(0x6000000+r)]['branches'],(hex(r),hex(o))
def ar(r,o,op):assert [o,op] in m[hex(0x6000000+r)]['arithmetic_sites'],(hex(r),hex(o),op)
def const(r,o,op,v):assert [o,op,v] in m[hex(0x6000000+r)]['constants'],(hex(r),hex(o))
# Correct ordinary construction, aliased point list, conditional insertion of head.
site(0x652,1,'newobj',0x60048de);site(0x48e0,0x17,'newobj',0x600142d)
site(0x142d,0xd2,'stfld',0x4000bdb);site(0x142d,0x10b,'callvirt',0xa00042f)
site(0x142d,0xc8,'callvirt',0xa000392);site(0x142d,0x11e,'callvirt',0xa000392)
# Parser retains integer conversion before float coordinate storage.
for o,op in [(0x176c,'conv.i4'),(0x176d,'conv.r4'),(0x17ac,'conv.i4'),(0x17ad,'conv.r4')]:ar(0x28f7,o,op)
site(0x28f7,0x17b7,'callvirt',0xa000392)
# The third entry is Linear=2. Consecutive pair loop, one iteration of lerp inner loop.
branch(0x144a,0xb4,'switch',[0xce,0x1f8,0x7a2,0x56a])
const(0x144a,0x7a2,'ldc.i4.1',None);const(0x144a,0x7cd,'ldc.i4.1',None)
site(0x144a,0x7c6,'newobj',0x600110c);site(0x144a,0x82d,'newobj',0x600110c)
site(0x144a,0x837,'stfld',0x4000a35);site(0x144a,0x85f,'stfld',0x4000a34)
branch(0x144a,0x877,'blt',[0x7aa]);site(0x144a,0x8d0,'stfld',0x4000bdd)
# Segment endpoint storage and length sqrt/single conversions.
site(0x110c,8,'stfld',0x4000a32);site(0x110c,0xf,'stfld',0x4000a33)
site(0x110d,0x14,'call',0x6004ec9)
site(0x4ec9,0x1c,'call',0xa00049e);ar(0x4ec9,0x21,'conv.r4')
# Tail processing: unordered-or-less -> remove; equal endpoints bypass normalization.
const(0x144a,0xa12,'ldc.r8',0.0001)
branch(0x144a,0xa1c,'ble.un.s',[0xa70]);branch(0x144a,0xa31,'brfalse.s',[0xa8d])
site(0x144a,0xa4f,'call',0x6004ed2);ar(0x144a,0xa5d,'conv.r4')
site(0x144a,0xa69,'stfld',0x4000a33);site(0x144a,0xa73,'callvirt',0xa000acb)
branch(0x144a,0xa88,'bgt',[0x9e8])
# Prepared collections and duration writer are the E05 mechanism.
site(0x144a,0xae1,'stfld',0x4000bdc);site(0x144a,0xaf6,'stfld',0x4000bde)
site(0x144a,0xb28,'callvirt',0x600110d);site(0x144a,0xb37,'callvirt',0xa000ad0)
assert 0xbea in m['0x600144a']['returns']
site(0x144a,0x1378,'stfld',0x4002524)
# Translation occurs in old/new loading follow-up and keeps cumulative field untouched.
site(0x1457,0x87,'stfld',0x4000a32);site(0x1457,0xa9,'stfld',0x4000a33)
assert not any(t=='0x4000bde' for _,_,t in m['0x6001457']['token_sites'])
# Known external preparation dispatch and earliest gated prepared-field read.
site(0x289a,0x79,'callvirt',0xa0010a5)
site(0x46a6,0x7d,'call',0x60028e5)
site(0x28e5,0x98,'callvirt',0x60024b5)
site(0x144b,0x3d,'ldfld',0x4000bdc);site(0x144b,0x12f,'ldfld',0x4000bde)
site(0x144b,0x1fe,'ldfld',0x4000a32);site(0x144b,0x20f,'ldfld',0x4000a33)
a,b=m['0x60024b5'],m['0x600144b']
assert a['name']==b['name'] and a['signature']==b['signature'] and b['virtual'] and not b['new_slot']
assert j['types']['0x20002af']['extends']=='0x2000339'
assert j['types']['0x2000339']['extends']=='0x20004c3'
site(0x223c,0x5d,'stfld',0x40013f0);site(0x2269,0x15a,'stsfld',0x40013d8)
# Position query can lazily request preparation; it is not proof of a global first read.
site(0x145f,0x12,'callvirt',0x600144a);site(0x145f,0xa8,'callvirt',0xa000ae7)
site(0x28e3,0x17,'callvirt',0x60024b2);site(0x1449,0x18,'callvirt',0x600144a)
assert len(j['prepared_field_sites'])==10
assert ['0x60028e5',0x98,'callvirt'] in j['visual_slot_direct_callers']
old=json.loads((Path(__file__).parents[1]/'evidence/05/slider-timing.json').read_text())['methods']
shared=0
for t,r in m.items():
 if t in old and 'body_sha256' in r:
  assert r['body_sha256']==old[t]['body_sha256'],t
  shared+=1
print(f'PASS: 43 methods, Linear switch/pairs, coordinate conversions, tail guards, storage, duration and external consumer; {shared} E05 hashes agree; full field scan 19659 bodies')
