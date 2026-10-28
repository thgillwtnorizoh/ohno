#!/usr/bin/env python3
"""E16 evidence assertions only; never schedules/classifies a substitute slider."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==16 and len(m)==70
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,[d]]in row(r)['branches'],(hex(r),hex(o),op,hex(d))
def co(r,o,op,v=None):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def writes(f):return {(k,o)for k,ss in j['entry_field_sites'].items()for o,p,t in ss if p in ['stfld','stsfld']and t==hex(0x4000000+f)}
assert j['fields']['0x4000bf8']['signature']=='06151282c10108'
assert j['fields']['0x4000bf6']['signature']=='061d02'
assert j['list_typeref']['namespace']=='System.Collections.Generic'and j['list_typeref']['name']=='List`1'
assert j['boolean_typeref']['name']=='Boolean'
assert all(x['class_signature']=='151282c10108'for x in j['collection_metadata'].values())
assert writes(0xbf8)=={('0x600142d',0x23)}
assert writes(0xbf6)=={('0x600144a',0x14ed)}
assert writes(0xbf1)=={('0x600146f',0x2d1),('0x6001465',0x24)}
assert writes(0xbf5)=={('0x600146f',0x31e)}
# Original producer, independent spacing/rate/time arithmetic and terminal adjustment.
site(0x142d,0x1e,'newobj',0xa000147);site(0x142d,0x23,'stfld',0x4000bf8)
site(0x28be,0x2b,'stfld',0x4000adf)
br(0x144a,0x8e9,'blt.s',0x911);site(0x144a,0x908,'callvirt',0x6001be4)
br(0x144a,0x9b8,'ble.un.s',0x9c2)
br(0x144a,0xb5b,'brfalse',0xbe5);site(0x144a,0xbd5,'callvirt',0xa000ad2)
co(0x144a,0xdc4,'ldc.r8',0.01);site(0x144a,0xdce,'ldfld',0x4000bfc)
ar(0x144a,0xdd3,'mul');ar(0x144a,0xf0c,'add');ar(0x144a,0xf1a,'sub')
ar(0x144a,0xf21,'cgt.un');br(0x144a,0xf2a,'brtrue',0x11b5)
ar(0x144a,0xf32,'conv.r4');site(0x144a,0xf33,'call',0x600145c)
site(0x144a,0xf42,'callvirt',0xa00014c);br(0x144a,0xf4d,'brfalse',0x11a8)
br(0x144a,0x11ac,'blt.un.s',0x11b5);br(0x144a,0x11b0,'brfalse',0xf08)
site(0x144a,0x11d6,'call',0x600145c);site(0x144a,0x11db,'callvirt',0xa00014c)
site(0x144a,0x1378,'stfld',0x4002524)
site(0x144a,0x14af,'call',0x6003e4d);ar(0x144a,0x14b5,'div')
co(0x144a,0x14cf,'ldc.i4.s',36);site(0x144a,0x14d2,'call',0xa0000fc)
site(0x144a,0x14d7,'callvirt',0xa0008ad);site(0x144a,0x14e8,'newarr',0x100008f)
site(0x144a,0x14f8,'callvirt',0xa000ad2)
ar(0x145c,0xf,'div');co(0x145c,0x10,'ldc.r8',1000.0);ar(0x145c,0x19,'mul');ar(0x145c,0x1b,'conv.i4')
# Due prefix loop; ONE selected item, not a loop over overdue items.
site(0x146f,0x286,'ldsfld',0x4002358);br(0x146f,0x28b,'ble.s',0x268)
br(0x146f,0x278,'bge.s',0x28d);br(0x146f,0x29b,'bge',0x355)
br(0x146f,0x2b1,'brfalse.s',0x315);site(0x146f,0x2b4,'ldfld',0x4000bf2)
site(0x146f,0x2c1,'callvirt',0xa000511);br(0x146f,0x2c6,'bgt.s',0x315)
site(0x146f,0x2d1,'stfld',0x4000bf1);site(0x146f,0x31e,'stfld',0x4000bf5)
assert row(0x146f)['array_sites']==[[0x2df,'stelem.i1'],[0x32c,'stelem.i1']]
br(0x146f,0x2ec,'bne.un.s',0x2f6);co(0x146f,0x2ee,'ldc.i4',128)
ar(0x146f,0x308,'div');ar(0x146f,0x309,'rem');br(0x146f,0x30a,'brtrue.s',0x311)
co(0x146f,0x30c,'ldc.i4.s',64);co(0x146f,0x311,'ldc.i4.8')
br(0x146f,0x345,'bne.un.s',0x34f)
co(0x146f,0x347,'ldc.i4',-524288);co(0x146f,0x34f,'ldc.i4',-262144)
assert not any(o>=0x28d and any(d<o for d in ds)for o,op,ds in row(0x146f)['branches'])
assert not any(o>=0x28d and t in ['0x4002358','0x4000bdf','0x400152a']for o,op,t in row(0x146f)['token_sites'])
br(0x146f,0x379,'bge.s',0x381);site(0x146f,0x37c,'call',0x600146e)
# Reached ordinary virtual consumer and base's first result-dependent mutation.
slots=j['ordinary_dispatch']['0x2000253']['slots']
assert {x['method']for x in slots['0x6000ad2']}=={'0x6000ad2','0x600114a'}
assert [x['method']for x in slots['0x6000ad0']]==['0x6000ad0']
site(0xac5,0x4c,'callvirt',0x60024c3);site(0xac5,0x5c,'callvirt',0x6000ad2)
site(0x114a,1,'ldfld',0x400252e);site(0x114a,9,'ldfld',0x4001527)
site(0x114a,0x1a,'callvirt',0x6002282);site(0x114a,0x22,'call',0x6000ad2)
br(0xad2,1,'brtrue.s',4);site(0xad2,0xb,'stfld',0x40013ea)
co(0xad2,0x206,'ldc.i4',-12648449);co(0xad2,0x226,'ldc.i4',-14745601)
br(0xad2,0x260,'beq',0x426);br(0xad2,0x293,'beq',0x579)
co(0xad2,0x579,'ldc.i4.s',10);site(0xad2,0x57d,'ldfld',0x4000604)
site(0xad2,0x452,'callvirt',0x600125a)
co(0xad2,0x437,'ldc.r8',-4.0);co(0xad2,0x440,'ldc.r8',-15.0);co(0xad2,0x449,'ldc.r8',-28.0)
br(0xad2,0xfd5,'ble.s',0x100a);br(0xad2,0xfdf,'ble.s',0x100a)
site(0xad2,0x100c,'stfld',0x400153d)
# E12 corrected optional-ruleset/update ordering remains pinned.
site(0x226b,0x94e,'call',0x6002275);site(0x226b,0x959,'callvirt',0x60028e2)
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if f.parent.name=='16':continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
   shared+=1
print(f'PASS: 70 methods; mixed schedule producer, due-prefix/one-item decision, counters/array/raw returns, ordinary consumer and ordering; {shared} historical body hashes; no original or substitute gameplay execution')
