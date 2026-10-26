#!/usr/bin/env python3
"""Validate static Expedition 05 evidence, not runtime slider behavior."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text()); m=j['methods']
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert len(m)==57

def site(rid,off,op,target):
    assert [off,op,hex(target)] in m[hex(0x6000000+rid)]['token_sites'],(hex(rid),hex(off))
def ar(rid,off,op):
    assert [off,op] in m[hex(0x6000000+rid)]['arithmetic_sites'],(hex(rid),hex(off),op)
def branch(rid,off,op,targets):
    assert [off,op,targets] in m[hex(0x6000000+rid)]['branches'],(hex(rid),hex(off))
def constant(rid,off,op,value):
    assert [off,op,value] in m[hex(0x6000000+rid)]['constants'],(hex(rid),hex(off))
# Regression check for E04's incorrect ruleset annotation: offsets exclude headers.
branch(0x223b,0xe,'switch',[0x33,0x25,0x56,0x64])
site(0x223b,0x27,'newobj',0x6001ecb)
site(0x223b,0x4a,'newobj',0x600113e)
site(0x1141,0xe,'newobj',0x6000651)
site(0x652,1,'newobj',0x60048de)
site(0x48e0,0x17,'newobj',0x600142d)
# Abstract rate slot -> inherited manager implementation; base slider has no override.
for a,b in [(0x125b,0x28ab),(0x144a,0x4261),(0x28e9,0x20d7)]:
    x,y=m[hex(0x6000000+a)],m[hex(0x6000000+b)]
    assert x['name']==y['name'] and x['signature']==y['signature']
    assert y['virtual'] and not y['new_slot']
assert m['0x600125b']['abstract'] and m['0x600125b']['rva']=='0x0'
assert m['0x6004261']['code_bytes']==1 and m['0x6004261']['returns']==[0]
assert m['0x600144a']['body_sha256']=='d58260ebed39834edc228b1b21995da8481e87bbc342223db2b812c7c9b7ceef'
# Initial fields, raw repeat setter and separately clamped effective span count.
site(0x142d,0x7c,'stfld',0x4002523);site(0x142d,0x83,'stfld',0x4002524)
site(0x1433,2,'stfld',0x4000bd0);site(0x142d,0x12c,'stfld',0x4002527)
site(0x142d,0x179,'stfld',0x4002528)
# Rate, inherited factor and two deliberately distinct timing lookups.
site(0x28be,0x2b,'stfld',0x4000adf)
site(0x28f5,0x7f,'callvirt',0x60028be)
assert [x['method'] for x in j['slot_candidates']['0x600125b']]==['0x600125b','0x60028ab']
assert not any(x['owner']=='0x20000e8' for x in j['slot_candidates']['0x60028e9'])
site(0x28ab,9,'callvirt',0x6001be3)
for off,op in [(0x2c,'mul'),(0x37,'div'),(0x38,'mul')]:ar(0x28ab,off,op)
constant(0x28ab,0x2d,'ldc.r8',1000.0)
site(0x144a,0x65,'callvirt',0x600125b);site(0x144a,0x6a,'stfld',0x4000bfc)
site(0x1be3,0x33,'ldfld',0x4000662);ar(0x1be3,0x38,'conv.i4')
branch(0x1be3,0x77,'ble.s',[0xcf]);site(0x1be3,0xc8,'callvirt',0x6000b78)
site(0x1be6,0x2b,'ldfld',0x4000662);branch(0x1be6,0x31,'bgt.un.s',[0x35])
for rid,upper in [(0xb78,1000.0),(0xb79,10000.0)]:
    constant(rid,0x1f,'ldc.r4',10.0);constant(rid,0x24,'ldc.r4',upper)
    ar(rid,0x1e,'conv.r4');ar(rid,0x33,'div')
# Old-version path still postprocesses sliders.
branch(0x28ea,0xc,'ble.s',[0x55]);site(0x28ea,0x2e,'callvirt',0x600144a)
site(0x28ec,0x36,'callvirt',0x600144a)
branch(0x144a,0x8e9,'blt.s',[0x911]);site(0x144a,0x908,'callvirt',0x6001be4)
# Stored length fallback, float segment-time arithmetic, sole base end-time write.
for off in [0x99b,0x9ab]:site(0x144a,off,'stfld',0x4002528)
for off,op in [(0xe3f,'sub'),(0xe40,'conv.r4'),(0xe74,'mul'),(0xe75,'conv.r8'),
               (0xe7c,'div'),(0xef8,'add'),(0x1377,'conv.i4')]:ar(0x144a,off,op)
site(0x144a,0x1378,'stfld',0x4002524)
assert [o for o,op,t in m['0x600144a']['token_sites'] if op=='stfld' and t=='0x4002524']==[0x1378]
constant(0x28f7,0x17e3,'ldc.i4',9000);branch(0x28f7,0x17e8,'ble.s',[0x17fa])
# Separate Taiko writer: do not apply ordinary slider loop to it.
site(0x20d7,0x1d6,'callvirt',0x6004263)
site(0x4263,0xdd,'stfld',0x4002524)
# Loading/completion chain remains the original one.
site(0x28f5,0x269,'callvirt',0x60028e9)
site(0x28e9,0x1b,'call',0x60028ea)
# Check every overlapping E04 body identity without editing that historical evidence.
old=json.loads((Path(__file__).parents[1]/'evidence/04/initialization.json').read_text())['methods']
shared=0
for t,r in m.items():
    if t in old and 'body_sha256' in r:
        assert r['body_sha256']==old[t]['body_sha256'],t
        shared+=1
print(f'PASS: 57 methods, ruleset correction, virtual slots, timing/length/arithmetic sites, versions, separate writer; {shared} E04 body hashes agree')
