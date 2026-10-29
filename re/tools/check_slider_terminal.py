#!/usr/bin/env python3
"""E22 verifies original evidence sites, never evaluates a slider or score model."""
import hashlib
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==22 and len(m)==48
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,[d]]in row(r)['branches'],(hex(r),hex(o),op,hex(d))
def co(r,o,op,v=None):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def la(r,o,op,n=None):assert [o,op,n]in row(r)['local_argument_sites']
def slot(root,s,expected):
    a=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
    assert {x['method']for x in a}=={hex(0x6000000+x)for x in expected}
def writes(f):
    return {(k,o)for k,ss in j['entry_field_sites'].items()for o,op,t in ss
            if op in ['stfld','stsfld']and t==hex(0x4000000+f)}
for f in [0xbf1,0xbf2,0xbf5]:assert j['fields'][hex(0x4000000+f)]['signature']=='0608'
assert j['fields']['0x4000bf3']['signature']=='0602'
assert j['fields']['0x4000bf6']['signature']=='061d02'
assert writes(0xbf1)=={('0x600146f',0x2d1),('0x6001465',0x24)}
assert writes(0xbf5)=={('0x600146f',0x31e)}
assert writes(0xbf3)=={('0x600146d',0x20d),('0x600146e',0xfb)}
# Final-only timestamp adjustment and the original inclusive due scan.
site(0x144a,0x1378,'stfld',0x4002524);site(0x144a,0x14af,'call',0x6003e4d)
ar(0x144a,0x14b5,'div');co(0x144a,0x14cf,'ldc.i4.s',36)
site(0x144a,0x14d2,'call',0xa0000fc);site(0x144a,0x14d7,'callvirt',0xa0008ad)
site(0x144a,0x14ed,'stfld',0x4000bf6)
site(0x146f,1,'ldfld',0x4001527);br(0x146f,6,'brtrue.s',0x15)
site(0x146f,0x286,'ldsfld',0x4002358);br(0x146f,0x28b,'ble.s',0x268)
br(0x146f,0x29b,'bge',0x355);br(0x146f,0x2b1,'brfalse.s',0x315)
br(0x146f,0x2c6,'bgt.s',0x315)
site(0x146f,0x2d1,'stfld',0x4000bf1);site(0x146f,0x31e,'stfld',0x4000bf5)
assert row(0x146f)['array_sites']==[[0x2df,'stelem.i1'],[0x32c,'stelem.i1']]
br(0x146f,0x2ec,'bne.un.s',0x2f6);co(0x146f,0x2ee,'ldc.i4',128)
br(0x146f,0x345,'bne.un.s',0x34f);co(0x146f,0x347,'ldc.i4',-524288)
br(0x146f,0x379,'bge.s',0x381);site(0x146f,0x37c,'call',0x600146e)
assert not any(o>=0x28d and any(d<o for d in ds)for o,op,ds in row(0x146f)['branches'])
# Ordinary terminal dispatch: positive amount30 versus zero amount/false gate.
slot(0x2af,0x24c3,[0x146f,0x24c3]);slot(0x253,0xad2,[0x114a,0xad2])
slot(0x253,0xac5,[0xac5])
site(0xac5,0x4c,'callvirt',0x60024c3);site(0xac5,0x5c,'callvirt',0x6000ad2)
site(0xac5,0xa0,'stfld',0x400138c);site(0x114a,0x22,'call',0x6000ad2)
co(0xad2,0x1ed,'ldc.i4.1');la(0xad2,0x1ee,'stloc.3')
co(0xad2,0x1ef,'ldc.i4.0');la(0xad2,0x1f0,'stloc.0')
co(0xad2,0x206,'ldc.i4',-12648449);co(0xad2,0x226,'ldc.i4',-14745601)
br(0xad2,0x255,'beq',0x3ed);br(0xad2,0x2c3,'beq',0x719)
site(0xad2,0x419,'callvirt',0x600125a);co(0xad2,0x41f,'ldc.i4.0')
la(0xad2,0x420,'stloc.3');br(0xad2,0x421,'br',0xe2d)
co(0xad2,0x719,'ldc.i4.s',30);co(0xad2,0x722,'ldc.r8',4.0)
br(0xad2,0x72d,'br',0xe2d)
assert [(o,op,t)for o,op,t in row(0xad2)['token_sites']if 0x719<=o<0x732]==[(0x71d,'ldfld','0x4000604')]
br(0xad2,0xe39,'bne.un',0xfd0);br(0xad2,0xfd5,'ble.s',0x100a)
br(0xad2,0xfdf,'ble.s',0x100a);site(0xad2,0x100c,'stfld',0x400153d)
br(0xad2,0x119b,'brfalse',0x12c8);br(0xad2,0x11a2,'ble.s',0x11f4)
site(0xad2,0x11c7,'callvirt',0x60040f1);site(0xad2,0x1287,'callvirt',0x60040f1)
site(0xad2,0x12c3,'callvirt',0x6002979);site(0xad2,0x12da,'callvirt',0x6000ad0)
site(0xad2,0x12f2,'callvirt',0x600297b);site(0xad2,0x1301,'callvirt',0x600297a)
site(0xad2,0x13d8,'callvirt',0x60019f5);site(0xad2,0x1404,'callvirt',0x600226c)
br(0xad0,1,'brfalse.s',9);site(0xad0,4,'call',0x6000acf)
br(0xad0,0x13,'bgt.un.s',0x16)
site(0x40f1,2,'stfld',0x4002674)
# Rejoin/order, separate parent end read, exact slot and no completion drain.
site(0x226b,0x94e,'call',0x6002275);site(0x226b,0x959,'callvirt',0x60028e2)
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9c4,'call',0x6002273)
site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
site(0x28e3,0x17,'callvirt',0x60024b2);site(0x28e3,0x1e,'callvirt',0x60028e4)
site(0x28e4,0x57,'callvirt',0x60028d3);site(0x28e4,0x63,'ldsfld',0x4002358)
br(0x28e4,0x68,'bgt.s',0xd6);site(0x28e4,0x74,'callvirt',0x60028d3)
slot(0x2af,0x24a9,[0x24a9,0x1465]);slot(0xe8,0x28e4,[0x28e4,0x654])
site(0x1465,2,'stfld',0x4001527);site(0x1465,0x24,'stfld',0x4000bf1)
ar(0x1465,0x3e,'div');site(0x1465,0x5e,'ldfld',0x400198e)
assert not any(t in [hex(0x4000000+x)for x in [0xbf5,0xbf6,0xbf7]]for o,op,t in row(0x1465)['token_sites'])
for r in [0x146f,0x1465,0x28d3,0xad2]:
    assert not any(op.startswith('st')and t=='0x400152a'for o,op,t in row(r)['token_sites'])
assert not any(t in ['0x4000bdf','0x400152a']for o,op,t in row(0x146f)['token_sites'])
assert not any(op.startswith('st')and t=='0x4001527'for o,op,t in row(0x146f)['token_sites'])
for r in [0x1449,0x28e4,0x1465,0x28d3]:
    assert not any(op.startswith('stelem')for o,op in row(r)['array_sites'])
site(0x28d3,0x12,'callvirt',0x60024a9);site(0x28d3,0xa40,'callvirt',0x600291a)
site(0x229c,0xf,'callvirt',0x6000ad2)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
    if f.parent.name=='22':continue
    old=json.loads(f.read_text()).get('methods',{})
    for t,r in m.items():
        if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
            assert r['body_sha256']==old[t]['body_sha256'],(str(f),t);shared+=1
fixture=Path(__file__).parents[1]/'fixtures/21/two-traversal-linear.osu'
assert hashlib.sha256(fixture.read_bytes()).hexdigest()=='8c5573d0d4fbc84e2898170273c31747b3f8eca232290ee01af65954633e2154'
print(f'PASS: 48 original methods; final due/result/Boolean/combo/consumer and parent ordering; {shared} historical hashes; E21 fixture unchanged; no original or surrogate gameplay execution')
