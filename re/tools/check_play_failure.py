#!/usr/bin/env python3
"""E19 checks original instruction evidence, never runs an HP/failure model."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==19 and len(m)==52
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
slot(0x253,0xab4,[0xab4]);slot(0x253,0xacb,[0xacb]);slot(0x253,0xad0,[0xad0])
slot(0x47b,0x227d,[0x227d]);slot(0xe8,0x125a,[0x125a,0x28b3])
for root in [0x56a,0x124,0x711]:slot(root,0x2979,[0x2979])
site(0xab4,7,'newobj',0x6002963);site(0xab4,0xc,'stfld',0x40005f5)
site(0xace,1,'ldsfld',0x40013c3);site(0xace,6,'stfld',0x40005f3)
assert j['fields']['0x40005f5']['signature']=='061295a8'
assert j['mod_wrapper']['field_signature']=='06151289b40111ad5c'
assert j['mod_wrapper']['typespec_signature']=='151289b40111ad5c'
assert j['enums']['0x2000b57']['values']['SuddenDeath']==32
assert j['enums']['0x2000b57']['values']['NoFail']==1
assert j['enums']['0x2000b57']['values']['Easy']==2
# Scoped failed checkpoint: zero combo/snapshot precede record-mod bit32 test.
site(0xad2,0x1287,'callvirt',0x60040f1);site(0xad2,0x128f,'stfld',0x400198a)
site(0xad2,0x1295,'ldfld',0x40005f3);site(0xad2,0x129a,'ldfld',0x4001970)
co(0xad2,0x12a4,'ldc.i4.s',32);ar(0xad2,0x12ae,'and');ar(0xad2,0x12b0,'cgt')
br(0xad2,0x119b,'brfalse',0x12c8);br(0xad2,0x11a2,'ble.s',0x11f4)
br(0xad2,0x11ef,'br',0x12c8);br(0xad2,0x12b2,'brfalse.s',0x12c8)
site(0xad2,0x12b5,'ldfld',0x40005f5);co(0xad2,0x12ba,'ldc.r8',0.0)
site(0xad2,0x12c3,'callvirt',0x6002979)
# Immediate absolute target: clamped first double and original argument second.
co(0x2979,1,'ldc.r8',0.0);co(0x2979,0xa,'ldc.r8',200.0)
site(0x2979,0x14,'call',0xa0002d4);site(0x2979,0x19,'call',0xa0002d1)
site(0x2979,0x1e,'call',0x6002967);site(0x2979,0x25,'call',0x6002969)
assert row(0x2979)['returns']==[0x2a] and row(0x2979)['branches']==[]
for r,f in [(0x2967,0x1857),(0x2969,0x1858),(0x2965,0x1856)]:
 assert writes(f)=={(hex(0x6000000+r),2)};site(r,2,'stfld',0x4000000+f)
 assert row(r)['code_bytes']==8
for f in [0x1856,0x1857,0x1858]:assert j['fields'][hex(0x4000000+f)]['signature']=='060d'
# Important intervening delta update, observer/helper work, then direct Play decision.
site(0xad2,0x12da,'callvirt',0x6000ad0);br(0xad2,0x12e9,'ble.un.s',0x12f9)
site(0xad2,0x12f2,'callvirt',0x600297b);site(0xad2,0x1301,'callvirt',0x600297a)
site(0xad2,0x1404,'callvirt',0x600226c)
site(0x297a,0x2d,'call',0x6002969);site(0x297a,0x49,'call',0x6002967)
site(0x297b,0x1f,'call',0x6002969);site(0x297b,0x49,'call',0x6002967)
# Zero is included, unordered/positive values skip, and independent continuation gates.
site(0x226c,0x1b,'callvirt',0x6002966);br(0x226c,0x29,'bgt.un.s',0x93)
co(0x226c,0x3a,'ldc.i4.1');br(0x226c,0x43,'brtrue.s',0x93)
br(0x226c,0x4a,'brtrue.s',0x93);br(0x226c,0x51,'brtrue.s',0x93)
br(0x226c,0x58,'brfalse.s',0x61);br(0x226c,0x5f,'brfalse.s',0x93)
br(0x226c,0x66,'brtrue.s',0x93);br(0x226c,0x6e,'bne.un.s',0x8d)
br(0x226c,0x8b,'ble.s',0x93);site(0x226c,0x8e,'callvirt',0x600227d)
site(0x227d,7,'callvirt',0x6000acb)
# Failed-state latch can be diverted by positive recovery count before latch work.
br(0xacb,1,'brtrue',0xea);br(0xacb,0xd,'ble.s',0x18)
site(0xacb,0x11,'call',0x6000acc);br(0xacb,0x1d,'brtrue',0xe8)
site(0xacb,0x3c,'callvirt',0x60024c4);site(0xacb,0x5a,'call',0x6003b55)
site(0xacb,0x60,'stsfld',0x400136a);site(0xacb,0x6a,'stsfld',0x400136b)
site(0x3b55,0x13,'ldftn',0x6003baa);site(0x3b55,0x25,'callvirt',0x6002fee)
site(0xab7,0xb0,'stfld',0x40005f8);co(0xab7,0x88,'ldc.i4.s',32)
site(0xacc,0x25,'callvirt',0x600297b);co(0xacc,0x1c,'ldc.r8',160.0)
site(0xacc,0x93,'callvirt',0x600225c);site(0xacc,0xa1,'stfld',0x40005f8)
assert not any(t in ['0x40013bb','0x60046ad']for o,p,t in row(0xacb)['token_sites']if p.startswith('st')or p.startswith('call'))
# Separate frame call only at PlayMode1; E12 post-refresh rejoin preserved.
br(0x226b,0xa4c,'bne.un.s',0xa54);site(0x226b,0xa4f,'call',0x600226c)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if f.parent.name=='19':continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in m.items():
  if t in old and 'body_sha256'in old[t]and 'body_sha256'in r:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t);shared+=1
print(f'PASS: 52 methods; bit32 provenance, absolute two-field setter, intervening delta and first fail/continue/latch/recovery boundaries; {shared} historical hashes; no original or surrogate failure execution')
