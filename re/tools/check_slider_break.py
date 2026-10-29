#!/usr/bin/env python3
"""E18 original-instruction assertions only; never computes a substitute combo."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==18 and len(m)==48
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
slot(0x253,0xabc,[0xabc]);slot(0x253,0xad2,[0xad2,0x114a]);slot(0x253,0xad0,[0xad0])
slot(0x2af,0x24c3,[0x24c3,0x146f]);slot(0x2ac,0x19f5,[0x19f5]);slot(0x2e9,0x19f5,[0x19f5]);slot(0x9d0,0x19f5,[0x19f5,0x4924])
# Natural component outcome and leave-before-consumer relationship.
br(0x146f,0x2b1,'brfalse.s',0x315);br(0x146f,0x2c6,'bgt.s',0x315)
site(0x146f,0x2d1,'stfld',0x4000bf1);site(0x146f,0x31e,'stfld',0x4000bf5)
co(0x146f,0x311,'ldc.i4.8');co(0x146f,0x34f,'ldc.i4',-262144)
br(0x146f,0x357,'brtrue.s',0x381);br(0x146f,0x379,'bge.s',0x381)
site(0x146f,0x37c,'call',0x600146e);site(0x146e,0xfb,'stfld',0x4000bf3)
site(0xac5,0x4c,'callvirt',0x60024c3);site(0xac5,0x5c,'callvirt',0x6000ad2)
site(0x114a,9,'ldfld',0x4001527);site(0x114a,0x22,'call',0x6000ad2)
assert not any(t=='0x6000ad2'for o,p,t in row(0x146e)['token_sites'])
# Shared prefix, masks and scoped checkpoint branches.
br(0xad2,1,'brtrue.s',4);site(0xad2,0xb,'stfld',0x40013ea)
br(0xad2,0x1e,'brfalse',0x1ed);br(0xad2,0x203,'ble.s',0x20d)
co(0xad2,0x206,'ldc.i4',-12648449);co(0xad2,0x226,'ldc.i4',-14745601)
br(0xad2,0x260,'beq',0x426);br(0xad2,0x293,'beq',0x579)
co(0xad2,0x1ed,'ldc.i4.1');co(0xad2,0x579,'ldc.i4.s',10)
site(0xad2,0x452,'callvirt',0x600125a);site(0xad2,0x100c,'stfld',0x400153d)
# Current combo identity is a distinct direct Int32 getter/setter, not F153d.
site(0xaaf,0x13,'callvirt',0x6000abc);site(0xabc,7,'newobj',0x60040ef)
site(0xabc,0xc,'stfld',0x40005f2);site(0x40f0,1,'ldfld',0x4002674)
site(0x40f1,2,'stfld',0x4002674)
assert row(0x40f1)['code_bytes']==8 and row(0x40f1)['token_sites']==[[2,'stfld','0x4002674']]
assert writes(0x2674)=={('0x60040f1',2)} and j['fields']['0x4002674']['signature']=='0608'
assert j['fields']['0x400198a']['signature']=='0607'and j['fields']['0x4001975']['signature']=='0608'
assert j['semantic_field_names']=={'0x40036ab':'FontCombo','0x40036ac':'FontComboOverlap','0x40036b5':'ComboSoundBursts'}
site(0x40f3,0x1f,'ldfld',0x40036ab);site(0x40f3,0x25,'ldfld',0x40036ac)
br(0xad2,0x119b,'brfalse',0x12c8);br(0xad2,0x11a2,'ble.s',0x11f4)
site(0xad2,0x11bc,'callvirt',0x60040f0);ar(0xad2,0x11c2,'add')
site(0xad2,0x11c7,'callvirt',0x60040f1);site(0xad2,0x11ce,'call',0xa0000fc)
site(0xad2,0x11d3,'stfld',0x4001975);ar(0xad2,0x11e9,'conv.u2')
site(0xad2,0x11ea,'stfld',0x400198a)
# Failure feedback gate does not gate the later zero store.
site(0xad2,0x11fa,'callvirt',0x60040f0);co(0xad2,0x11ff,'ldc.i4.s',20)
br(0xad2,0x1201,'ble.s',0x1277);br(0xad2,0x1208,'brtrue.s',0x1277);br(0xad2,0x120f,'brtrue.s',0x1277)
co(0xad2,0x1211,'ldc.i4',2064529101);site(0xad2,0x121e,'call',0x6003b14)
co(0xad2,0x1223,'ldc.i4.s',100);co(0xad2,0x1225,'ldc.r4',0.0);co(0xad2,0x122a,'ldc.i4.0');co(0xad2,0x122b,'ldc.r4',1.0)
site(0xad2,0x1230,'call',0x6003b31);br(0xad2,0x123b,'brfalse.s',0x1277)
co(0xad2,0x1283,'ldc.i4.0');site(0xad2,0x1287,'callvirt',0x60040f1)
ar(0xad2,0x128e,'conv.u2');site(0xad2,0x128f,'stfld',0x400198a)
co(0xad2,0x12a4,'ldc.i4.s',32);site(0xad2,0x12c3,'callvirt',0x6002979)
br(0x3b31,8,'bne.un.s',0xc);site(0x3b31,0x9a,'callvirt',0xa001622)
site(0x3b31,0xb1,'ldftn',0x6003bb6);site(0x3b31,0xbd,'callvirt',0x6002fee)
# Later generic observer is optional and receives only the raw enum.
br(0xad2,0x13c7,'bne.un.s',0x13dd);br(0xad2,0x13cf,'brfalse.s',0x13dd)
site(0xad2,0x13d8,'callvirt',0x60019f5);assert row(0x19f5)['code_bytes']==1
site(0x4924,0xc,'stfld',0x4000ed5);br(0x4924,0x19,'bge.s',0x30)
# Counterexample to universal negative reset; aggregate positives use the slider type gate.
co(0xad2,0x41f,'ldc.i4.0')
site(0xad2,0x737,'callvirt',0x6003e4e);ar(0xad2,0x73d,'ceq')
site(0xad2,0xb01,'callvirt',0x6003e4e);ar(0xad2,0xb07,'ceq')
site(0x226b,0x959,'callvirt',0x60028e2);site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if f.parent.name=='18':continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
   shared+=1
print(f'PASS: 48 methods; checkpoint provenance/leave ordering, direct combo increment/reset, independent feedback gate and optional observer; {shared} historical body hashes; no original or surrogate combo execution')
