#!/usr/bin/env python3
"""E25 checks original instructions, not a spinner/result/score simulation."""
import hashlib
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==25 and len(m)==51
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,[d]]in row(r)['branches'],(hex(r),hex(o),op,hex(d))
def co(r,o,op,v=None):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def la(r,o,op,n=None):assert [o,op,n]in row(r)['local_argument_sites'],(hex(r),hex(o),op,n)
def writes(f):return {(t,o)for t,ss in j['entry_field_sites'].items()for o,p,v in ss if p in ['stfld','stsfld']and v==hex(0x4000000+f)}
def slot(root,s,expected):
 a=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
 assert {x['method']for x in a}=={hex(0x6000000+x)for x in expected}
# No substitute rounding, absolute value, crossed-integer loop or counter delta.
assert row(0x5eb)['signature']=='200008'and not row(0x5eb)['virtual']
assert row(0x5eb)['token_sites']==[[1,'ldfld','0x40002b4']]
assert row(0x5eb)['arithmetic_sites']==[[6,'conv.i4']]
assert j['fields']['0x40002b4']['signature']=='060c'
for f in [0x29e,0x2a2,0x2a0]:assert j['fields'][hex(0x4000000+f)]['signature']=='0608'
assert writes(0x2b4)=={('0x60005f4',0x1b3),('0x60005f5',0x139),('0x60005f5',0x395)}
assert writes(0x29e)=={('0x60005f6',0x4c7)}
assert writes(0x2a2)=={('0x60005f6',0x2ed)}
site(0x5f6,0x2d5,'call',0x60005eb);site(0x5f6,0x2db,'ldfld',0x400029e)
br(0x5f6,0x2e0,'bne.un.s',0x2e4);co(0x5f6,0x2e2,'ldc.i4.0')
co(0x5f6,0x2eb,'ldc.i4.1');ar(0x5f6,0x2ec,'add');site(0x5f6,0x2ed,'stfld',0x40002a2)
site(0x5f6,0x2f7,'ldfld',0x40036af);site(0x5f6,0x30d,'call',0x6003b28)
assert j['focus_field_metadata']['0x40036af']['name']=='SpinnerFrequencyModulate'
br(0x5f6,0x320,'ble',0x496);co(0x5f6,0x31e,'ldc.i4.3');ar(0x5f6,0x31f,'add')
ar(0x5f6,0x333,'sub');ar(0x5f6,0x335,'rem');br(0x5f6,0x336,'brtrue',0x496)
br(0x5f6,0x49d,'ble.s',0x4b1);co(0x5f6,0x4a5,'ldc.i4.2');ar(0x5f6,0x4a6,'rem')
br(0x5f6,0x4a7,'brtrue.s',0x4b1);br(0x5f6,0x4b8,'ble.s',0x4c0)
for o,v in [(0x358,16384),(0x4a9,8192),(0x4ba,4096)]:co(0x5f6,o,'ldc.i4',v)
assert [(o,p,n)for o,p,n in row(0x5f6)['local_argument_sites']if p=='stloc.2']==[(1,'stloc.2',None),(0x35d,'stloc.2',None),(0x4ae,'stloc.2',None),(0x4bf,'stloc.2',None)]
site(0x5f6,0x353,'callvirt',0x600452b);site(0x5f6,0x38a,'call',0x6003b2b)
site(0x5f6,0x3b6,'callvirt',0x600093a);site(0x5f6,0x3c6,'callvirt',0xa0002cf)
site(0x5f6,0x4c2,'call',0x60005eb);site(0x5f6,0x4c7,'stfld',0x400029e)
assert not any(o>=0x2d4 and any(d<o for d in ds)for o,op,ds in row(0x5f6)['branches'])
assert row(0x5f6)['exception_regions']==[]
assert not any(t in ['0x4001527','0x400152a']for o,p,t in row(0x5f6)['token_sites'])
# Bounded preparation/range input, without implementing a required-spin formula.
site(0x5f4,0x1d4,'stfld',0x40002a0);site(0x28be,0x59,'callvirt',0x600125a)
site(0x28be,0x5e,'stfld',0x4000ae0);slot(0xe8,0x125a,[0x125a,0x28b3])
slot(0xd8,0x24c3,[0x24c3,0x5f6]);slot(0xd8,0x24b2,[0x24b2,0x5f5])
slot(0x253,0xac5,[0xac5]);slot(0x253,0xad2,[0xad2,0x114a]);slot(0x253,0xad0,[0xad0])
site(0xac5,0x4c,'callvirt',0x60024c3);site(0xac5,0x57,'ldfld',0x4001380)
site(0xac5,0x5c,'callvirt',0x6000ad2);site(0x114a,0x1a,'callvirt',0x6002282)
site(0x114a,0x22,'call',0x6000ad2)
# Raw0 early return; raw4096/8192/16384 take distinct amount/aux branches, all combo-disabled.
br(0xad2,1,'brtrue.s',4);site(0xad2,0xb,'stfld',0x40013ea)
co(0xad2,0x206,'ldc.i4',-12648449);co(0xad2,0x226,'ldc.i4',-14745601)
br(0xad2,0x351,'beq',0xb63);br(0xad2,0x35c,'beq',0xb7d);br(0xad2,0x36c,'beq',0xb98)
co(0xad2,0xb63,'ldc.i4.0');co(0xad2,0xb7d,'ldc.i4.s',100);co(0xad2,0xb98,'ldc.i4',1100)
co(0xad2,0xb6b,'ldc.r8',1.7);co(0xad2,0xb86,'ldc.r8',1.7);co(0xad2,0xba4,'ldc.r8',2.0)
for o in [0xb77,0xb92,0xbb0]:la(0xad2,o,'stloc.3')
br(0xad2,0xe39,'bne.un',0xfd0);site(0xad2,0x100c,'stfld',0x400153d)
br(0xad2,0x119b,'brfalse',0x12c8);site(0xad2,0x11c7,'callvirt',0x60040f1)
site(0xad2,0x1287,'callvirt',0x60040f1);site(0x40f1,2,'stfld',0x4002674)
site(0xad2,0x12da,'callvirt',0x6000ad0);br(0xad2,0x12e9,'ble.un.s',0x12f9)
site(0xad2,0x12f2,'callvirt',0x600297b);site(0xad2,0x1301,'callvirt',0x600297a)
site(0x2969,2,'stfld',0x4001858);site(0x2967,2,'stfld',0x4001857)
assert writes(0x2674)=={('0x60040f1',2)}
site(0xad2,0x1357,'callvirt',0x6003c57);site(0xad2,0x13d8,'callvirt',0x60019f5)
for r in [0x5f6,0xac5,0x114a,0xad2]:
 assert not any(op in ['stfld','stsfld']and t=='0x400152a'for o,op,t in row(r)['token_sites'])
# Input before integration; bounded manager evaluated latch may supersede pending notification.
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
site(0x5f5,0x395,'stfld',0x40002b4);br(0x43a0,0x820,'blt',0xca)
br(0x28e4,0x8f,'bge.s',0xd6);site(0x28e4,0xa3,'callvirt',0x60028d3)
site(0x28d3,0x12,'callvirt',0x60024a9);slot(0xd8,0x24a9,[0x24a9,0x5fa])
site(0x5fa,0x20,'stfld',0x4001527)
assert not any(t in ['0x40002b4','0x400029e','0x60005eb','0x60005f6']for o,p,t in row(0x5fa)['token_sites'])
assert j['result_enum']['fields']==[{'token':'0x4001451','name':'value__','signature':'0608','literal':False}]
assert {a['import_name']for a in j['native_imports']}=={'BASS_ChannelGetLength','BASS_ChannelSetAttribute'}
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if int(f.parent.name)>=25:continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t);shared+=1
assert hashlib.sha256((Path(__file__).parents[1]/'fixtures/24/single-spinner.osu').read_bytes()).hexdigest()=='85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c'
print(f'PASS:51 original methods; integer/counter/cache/result/ordinary dispatch/combo bypass/order; {shared} historical body hashes; E24 fixture unchanged; no original or surrogate execution')
