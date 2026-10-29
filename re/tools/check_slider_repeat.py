#!/usr/bin/env python3
"""E21 original evidence assertions, not an executable reversal/checkpoint model."""
import hashlib
import json
import sys
from pathlib import Path

j=json.loads(Path(sys.argv[1]).read_text()); m=j['methods']
assert j['expedition']==21 and len(m)==35
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,[d]]in row(r)['branches'],(hex(r),hex(o),op,hex(d))
def co(r,o,op,v=None):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def loc(r,o,op,n=None):assert [o,op,n]in row(r)['local_argument_sites']
# Raw and effective traversal count are separate; same geometry, parity-driven loop.
site(0x1433,2,'stfld',0x4000bd0)
co(0x142d,0x124,'ldc.i4.1');site(0x142d,0x127,'call',0xa0000fc)
site(0x142d,0x12c,'stfld',0x4002527)
co(0x144a,0xdd8,'ldc.i4.2');ar(0x144a,0xdd9,'rem');ar(0x144a,0xddb,'ceq')
loc(0x144a,0xddd,'stloc.s',46)
br(0x144a,0xde1,'brtrue.s',0xde6);br(0x144a,0xdee,'brtrue.s',0xdf4)
br(0x144a,0xdf9,'brtrue.s',0xdfe);co(0x144a,0xdf4,'ldc.i4.m1')
co(0x144a,0xdfe,'ldc.i4.m1')
site(0x144a,0xe0d,'callvirt',0xa0009cd)
site(0x144a,0xe1c,'callvirt',0xa000a44);ar(0x144a,0xe3f,'sub');ar(0x144a,0xe40,'conv.r4')
br(0x144a,0xe45,'brfalse.s',0xe5b)
site(0x144a,0xe49,'ldfld',0x4000a33);loc(0x144a,0xe4e,'stloc.s',14)
site(0x144a,0xe52,'ldfld',0x4000a32);loc(0x144a,0xe57,'stloc.s',12)
site(0x144a,0xe5d,'ldfld',0x4000a32);site(0x144a,0xe66,'ldfld',0x4000a33)
co(0x144a,0xe6d,'ldc.r4',1000.0);ar(0x144a,0xe74,'mul');ar(0x144a,0xe75,'conv.r8');ar(0x144a,0xe7c,'div')
site(0x144a,0xe95,'newobj',0x6002996)
site(0x144a,0xea9,'callvirt',0xa0002c8);site(0x144a,0xebb,'callvirt',0xa0002c8)
site(0x2996,8,'stfld',0x4001873);site(0x2996,0xf,'stfld',0x4001867)
site(0x2996,0x16,'stfld',0x400186b);site(0x2996,0x25,'stfld',0x400186e)
site(0x2996,0x2d,'stfld',0x400186f)
ar(0x144a,0xef8,'add');site(0x144a,0x1378,'stfld',0x4002524)
# Per-traversal end append, spacing carry, final-only adjustment and subset list.
site(0x144a,0xf42,'callvirt',0xa00014c);site(0x144a,0x11db,'callvirt',0xa00014c)
br(0x144a,0x11e2,'brfalse.s',0x11f1);co(0x144a,0x11e4,'ldc.r8',0.0)
ar(0x144a,0x11f7,'sub');ar(0x144a,0x11f8,'sub');ar(0x144a,0x11ff,'sub')
br(0x144a,0x1340,'blt',0xdb2)
site(0x144a,0x14af,'call',0x6003e4d);ar(0x144a,0x14b5,'div')
co(0x144a,0x14cf,'ldc.i4.s',36);site(0x144a,0x14d7,'callvirt',0xa0008ad)
site(0x144a,0x14ed,'stfld',0x4000bf6);ar(0x144a,0x150e,'div')
ar(0x144a,0x1521,'rem');br(0x144a,0x1522,'brtrue.s',0x153c)
site(0x144a,0x1537,'callvirt',0xa00014c)
# Separately constructed endpoint presentation object; no role in target query.
site(0x144a,0x12dc,'newobj',0x6002be0);site(0x144a,0x1302,'callvirt',0xa000ad4)
site(0x2be0,9,'call',0x60045ec);br(0x2be0,0x3c,'brfalse',0xf1)
site(0x2be0,0x9a,'newobj',0x6004510);site(0x2be0,0x9f,'stfld',0x4002bfb)
assert j['repeat_metadata']['0x6002be0']['owner']=='0x20005b1'
assert j['repeat_collection_members']['0xa0004ba']['name']=='Find'
# Inclusive first-match Movement query, original provider adjustment and strict disk.
site(0x146f,0xf0,'ldsfld',0x4002358);site(0x146f,0x110,'call',0x6003df6)
ar(0x146f,0x115,'sub');site(0x146f,0x133,'callvirt',0xa0004ba)
br(0x1478,0xc,'bgt.s',0x20);ar(0x1478,0x1a,'clt');ar(0x1478,0x1d,'ceq')
br(0x146f,0x1e2,'bne.un.s',0x1ee);ar(0x146f,0x217,'sub')
ar(0x146f,0x218,'conv.r4');ar(0x146f,0x226,'conv.r4')
ar(0x146f,0x227,'div');ar(0x146f,0x228,'sub')
site(0x146f,0x23b,'callvirt',0x6000ff1);site(0x146f,0x242,'call',0x6004ecd)
ar(0x146f,0x24c,'clt');site(0x146f,0x25d,'call',0x600146d)
# Due prefix k differs from selected index; no repeat-direction operand in classifier.
br(0x146f,0x28b,'ble.s',0x268);br(0x146f,0x29b,'bge',0x355)
br(0x146f,0x2c6,'bgt.s',0x315);site(0x146f,0x2d1,'stfld',0x4000bf1)
site(0x146f,0x31e,'stfld',0x4000bf5)
assert row(0x146f)['array_sites']==[[0x2df,'stelem.i1'],[0x32c,'stelem.i1']]
br(0x146f,0x2ec,'bne.un.s',0x2f6);co(0x146f,0x2ee,'ldc.i4',128)
ar(0x146f,0x308,'div');ar(0x146f,0x309,'rem');co(0x146f,0x30c,'ldc.i4.s',64)
co(0x146f,0x311,'ldc.i4.8');co(0x146f,0x347,'ldc.i4',-524288)
co(0x146f,0x34f,'ldc.i4',-262144);site(0x146f,0x37c,'call',0x600146e)
assert not any(o>=0x28d and any(d<o for d in ds)for o,op,ds in row(0x146f)['branches'])
assert not any(t in ['0x4000bf4','0x4000bd5','0x4000bf7']for o,op,t in row(0x146f)['token_sites'])
# Direct component consumer, raw64 branch, common object store and combo.
site(0xac5,0x4c,'callvirt',0x60024c3);site(0xac5,0x5c,'callvirt',0x6000ad2)
site(0x114a,0x22,'call',0x6000ad2);br(0xad2,0x2b8,'beq',0x649)
co(0xad2,0x649,'ldc.i4.s',30);co(0xad2,0x652,'ldc.r8',4.0)
site(0xad2,0x663,'ldfld',0x4001380);site(0xad2,0x6ac,'newobj',0x6004510)
site(0xad2,0x70f,'callvirt',0x6002ce6);site(0xad2,0x100c,'stfld',0x400153d)
site(0xad2,0x11c7,'callvirt',0x60040f1);site(0xad2,0x1287,'callvirt',0x60040f1)
site(0x40f1,2,'stfld',0x4002674)
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
    if f.parent.name=='21':continue
    old=json.loads(f.read_text()).get('methods',{})
    for t,r in m.items():
        if t in old and 'body_sha256'in old[t]:
            assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
            shared+=1
fixture=Path(__file__).parents[1]/'fixtures/21/two-traversal-linear.osu'
assert fixture.read_text().split('[HitObjects]\n')[1]=='128,192,2000,2,0,L|428:192,2,300\n'
assert 'AudioFilename: silence.wav\n'in fixture.read_text()
assert hashlib.sha256(fixture.read_bytes()).hexdigest()=='8c5573d0d4fbc84e2898170273c31747b3f8eca232290ee01af65954633e2154'
print(f'PASS: 35 original methods, reversal/carry/Movement/query/checkpoint/consumer sites; {shared} historical body hashes; fixture {hashlib.sha256(fixture.read_bytes()).hexdigest()}; no gameplay execution')
