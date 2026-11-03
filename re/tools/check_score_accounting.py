#!/usr/bin/env python3
"""Check E33 original sites/hashes; no numerical ledger/game implementation."""
import json,sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert len(m)==31 and j['xref_coverage']=={'decoded':19659,'zero_rva':1739,'errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o))
def br(r,o,op,d):assert [o,op,d]in row(r)['branches'],(hex(r),hex(o))
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o))
def c(r,o,op,v):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o))
site(0x114a,0x22,'call',0x6000ad2);site(0xad2,0x11,'ldfld',0x400153b)
site(0x249b,0x1f,'stfld',0x400153b)
# Basic whole-result counters and direct component amount classes.
for o,f in [(0x792,0x196b),(0x838,0x1969),(0x8aa,0x196a),(0x4a5,0x196e)]:site(0xad2,o,'stfld',0x4000000+f)
for o,v in [(0x579,10),(0x649,30),(0x719,30)]:c(0xad2,o,'ldc.i4.s',v)
site(0xad2,0x100c,'stfld',0x400153d)
# Ordinary combo bonus computed before combo increment, before ledger write.
c(0xad2,0x10fd,'ldc.i4.s',25); ar(0xad2,0x10ff,'div')
site(0xad2,0x10ef,'callvirt',0x60040f0);site(0xad2,0x1102,'ldfld',0x4000605)
ar(0xad2,0x1109,'conv.i4'); ar(0xad2,0x110a,'add')
site(0xad2,0x11c7,'callvirt',0x60040f1);site(0xad2,0x11d3,'stfld',0x4001975)
site(0xad2,0x1287,'callvirt',0x60040f1)
br(0xad2,0x1326,'brfalse.s',[0x1394]);site(0xad2,0x1357,'callvirt',0x6003c57)
site(0xad2,0x139b,'callvirt',0x6002b4b);site(0xad2,0x13a2,'callvirt',0x6002b4c);site(0xad2,0x13b6,'stfld',0x4001986)
site(0x2b4b,0xb,'ldfld',0x4001985);site(0x2b4b,0x11,'callvirt',0x6003c55);site(0x2b4c,2,'stfld',0x4001985)
# Accuracy ratio has integer arithmetic then Single conversion; zero total=1.
c(0x2b42,9,'ldc.r4',1.0)
for o,v in [(0x15,50),(0x1e,100)]:c(0x2b42,o,'ldc.i4.s',v)
c(0x2b42,0x28,'ldc.i4',300); ar(0x2b42,0x2f,'conv.r4');ar(0x2b42,0x3c,'conv.r4');ar(0x2b42,0x3d,'div')
site(0x2b44,0x15,'ldfld',0x400196e)
for f in ['0x4001969','0x400196a','0x400196b','0x400196c','0x400196d','0x400196e','0x400198a']:assert j['fields'][f]['signature']=='0607',f
assert j['fields']['0x4001985']['signature']=='0608' and j['fields']['0x4001986']['signature']=='060d'
site(0x2b62,0x18,'callvirt',0x6002b4c);site(0xace,6,'stfld',0x40005f3)
site(0xab7,0x48,'callvirt',0x6001bc4);site(0xab7,0x6e,'call',0x600156b);site(0xab7,0x74,'stfld',0x4000605)
site(0x2273,0x11,'callvirt',0x6002b4b);site(0x2273,0x2c,'callvirt',0x6002b42);site(0x2273,0x16,'callvirt',0x6001dd2);site(0x2273,0x37,'callvirt',0x6001dd3)
site(0x1dd2,2,'stfld',0x40010f3);site(0x1dd3,9,'stfld',0x40010f4)
assert {x[0]for x in j['target_xrefs']if x[3]=='0x4000605'}=={'0x6000a9c','0x6000ab7','0x6004608'}
base=Path(__file__).parents[1]/'evidence';shared=0
for n in ['12','18','19','23','25','26','27']:
 for p in (base/n).glob('*.json'):
  old=json.loads(p.read_text()).get('methods',{})
  for token,r in m.items():
   if token in old and old[token].get('body_sha256'):
    assert r['body_sha256']==old[token]['body_sha256'],(p,token)
    shared+=1
print(f'PASS E33: 31 selected methods, score/counter/accuracy/identity/reset/HUD sites; {shared} historical hash matches; targeted direct xrefs, no game execution')
