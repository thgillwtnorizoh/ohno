#!/usr/bin/env python3
"""E28 original-site consistency checks; never evaluates a pause/gameplay model."""
import hashlib
import json
import sys
from pathlib import Path

j=json.loads(Path(sys.argv[1]).read_text()); methods=j['methods']
assert j['expedition']==28 and len(methods)==119
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r): return methods[hex(0x6000000+r)]
def site(r,o,p,t): assert [o,p,hex(t)] in row(r)['token_sites'],(hex(r),hex(o),p,hex(t))
def br(r,o,p,d): assert [o,p,[d]] in row(r)['branches'],(hex(r),hex(o),p,hex(d))
def co(r,o,p,v=None): assert [o,p,v] in row(r)['constants'],(hex(r),hex(o),p,v)
def ar(r,o,p): assert [o,p] in row(r)['arithmetic_sites'],(hex(r),hex(o),p)
def stores(f):
 return {(t,o) for t,ss in j['entry_field_sites'].items() for o,p,v in ss
         if p in ['stfld','stsfld'] and v==hex(0x4000000+f)}
def slot(root,s,expected):
 a=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
 assert {x['method'] for x in a}=={hex(0x6000000+x) for x in expected}
assert row(0x2262)['body_sha256']=='9ce41412f438af334aa5cec529527f3229feb9b89c0ab41c8790754efc96c8c2'
assert row(0x3b49)['body_sha256']=='3c36e7ff3ecb11031dc6de0df89e31e1b51fffa1bf30b9d917ef054597404c57'
assert stores(0x1373)=={('0x600071b',0x180),('0x6002237',0x4ab),('0x6002262',0xc7),('0x6002266',0xc6)}
assert stores(0x1376)=={('0x6000acc',0x6e),('0x6002237',0x4b7),('0x600226b',0xb05)}
assert stores(0x13be)=={('0x6002237',0x4d5),('0x6002262',0x913),('0x6002266',0xc0),('0x600227b',0x69)}
assert stores(0x13d6)=={('0x6002261',0xa),('0x6002262',0x30)}
assert stores(0x13d7)=={('0x6002261',0x16)}
assert stores(0x1390)=={('0x6002262',0x1ba)}
assert stores(0x13bd)=={('0x6002262',0x9a1),('0x6002262',0xbbd),('0x6002265',0x22),('0x600227b',0x70)}
# Original configured control name, registered receiver, synchronous key dispatch.
assert j['enums']['0x2000b49']['values']['Pause']==18
assert j['enums']['0x2000b49']['values']['Skip']==19
assert j['enums']['0x2000b49']['values']['QuickRetry']==23
assert j['enums']['0x2000a7a']['values']['Escape']==27
site(0x2246,0x2a,'ldvirtftn',0x600225b); site(0x2246,0x35,'call',0x6003794)
slot(0x47b,0x225b,[0x225b]); slot(0x47b,0x2261,[0x2261])
site(0x43a0,0x248,'call',0x60043a2); site(0x43a2,0x3d5,'call',0x600379a)
site(0x379a,0x6b,'callvirt',0xa000304); br(0x379a,0x75,'brfalse.s',0x79)
site(0x43a2,0xf0,'call',0x600379b); site(0x379b,0,'ldsfld',0x4002077)
site(0x11a4,0x4a,'call',0x600379a); co(0x11a4,0x48,'ldc.i4.s',27)
site(0x43a0,0x5e,'call',0x60011a4); site(0x43a0,0xca,'call',0x6003b25)
# Pending request is not the pause mutation, and duplicates do not refresh timestamp.
br(0x225b,6,'beq.s',0xa); co(0x225b,0x1f,'ldc.i4.s',18)
co(0x225b,0x24,'ldc.i4.s',27); site(0x225b,0xc2,'callvirt',0x6002261)
br(0x2261,6,'brtrue.s',0x1b); site(0x2261,0xa,'stfld',0x40013d6)
site(0x2261,0x10,'ldsfld',0x4002358); ar(0x2261,0x15,'conv.i8')
site(0x2261,0x16,'stfld',0x40013d7)
# Consumption: delay, clear-before-veto, cooldown, finalized departure, toggle.
co(0x2262,0x1c,'ldc.i4',1000); ar(0x2262,0x21,'conv.i8')
br(0x2262,0x22,'bge.s',0x25); site(0x2262,0x30,'stfld',0x40013d6)
co(0x2262,0x99,'ldc.i4',1000); br(0x2262,0x9e,'bge.s',0xb0)
site(0x2262,0xb9,'call',0x6002285); ar(0x2262,0xc5,'ceq')
site(0x2262,0xc7,'stsfld',0x4001373); br(0x2262,0xec,'brfalse',0x912)
site(0x2262,0x1ba,'stfld',0x4001390); site(0x2262,0x1c5,'ldfld',0x40017fd)
site(0x2262,0x1da,'callvirt',0x60024c4)
site(0x2262,0x204,'call',0x6003b0d); site(0x2262,0x20b,'call',0x6003b49)
slot(0x94c,0x24c4,[0x24c4]); slot(0x2af,0x24c4,[0x24c4,0x146c])
slot(0xd8,0x24c4,[0x24c4,0x5fc])
# Resume handoff, callback permission, late normal-loop release.
site(0x2262,0x913,'stsfld',0x40013be); site(0x2262,0x95e,'ldsfld',0x400234c)
site(0x2262,0x9a1,'stfld',0x40013bd); site(0x2262,0xa54,'ldftn',0x6002265)
site(0x2262,0xbbd,'stfld',0x40013bd)
site(0x2265,0xc,'ldfld',0x4002cf7); br(0x2265,0x11,'brfalse.s',0x14)
site(0x2265,0x22,'stfld',0x40013bd)
site(0x226b,0x85c,'ldfld',0x40013bd); site(0x226b,0x885,'ldfld',0x4002cf7)
site(0x226b,0x88d,'call',0x600227b); site(0x226b,0x8ac,'call',0x6003b49)
site(0x227b,0x69,'stsfld',0x40013be); site(0x227b,0x70,'stfld',0x40013bd)
site(0x226b,0x8a5,'ldsfld',0x4001376)
# Pause suppresses this later work, not prior clock/input/presentation work.
site(0x226b,0x24a,'call',0x6002262); assert 0x24f in row(0x226b)['returns']
site(0x226b,0x6fb,'callvirt',0x6002f05); site(0x226b,0x71d,'call',0x6002239)
site(0x226b,0x838,'ldsfld',0x4001373); br(0x226b,0x83d,'brfalse.s',0x854)
site(0x226b,0x84e,'call',0x6002262); assert 0x853 in row(0x226b)['returns']
site(0x226b,0x8b5,'call',0x6002262); assert 0x8ba in row(0x226b)['returns']
site(0x226b,0x94e,'call',0x6002275); site(0x226b,0x959,'callvirt',0x60028e2)
site(0x226b,0x9be,'callvirt',0x6000ac5); site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9); br(0x226b,0x9a6,'brfalse.s',0x9c9)
site(0x226b,0x10c4,'call',0x6002262)
for o in [0x10,0x1e,0x25]: site(0x2295,o,'ldsfld',0x4000000+{0x10:0x1373,0x1e:0x13be,0x25:0x1376}[o])
# Clock publication precedes normal pause consumption; no pause check in main clock.
site(0x3b25,0x43,'stsfld',0x400235a); site(0x3b25,0x48,'call',0x6003b24)
for r in [0x3b24,0x3b25]:
 assert not any(t in ['0x4001373','0x40013be'] for o,p,t in row(r)['token_sites'])
site(0x3b24,0xd,'callvirt',0x6003d63); site(0x3b24,0xca,'callvirt',0x6003d5c)
site(0x3b24,0x309,'stsfld',0x400235b); site(0x3b24,0x397,'stsfld',0x400235c)
site(0x3b24,0x3bc,'stsfld',0x4002358); site(0x3b24,0x3c8,'stsfld',0x4002358)
site(0x3b24,0x3d2,'stsfld',0x4002359)
br(0x3b24,0x177,'bne.un',0x2fe); br(0x3b24,0x186,'bge.un',0x2fe)
# Numeric q states, action boundary, native versus fallback receiver resolution.
site(0x3b49,0x10,'stsfld',0x4002368); site(0x3b49,0x1d,'stsfld',0x4002368)
site(0x3b49,0x3d,'stsfld',0x4002368); site(0x3b49,0x64,'stsfld',0x4002368)
site(0x3b49,0x52,'callvirt',0x6003d61); site(0x3b49,0x6f,'callvirt',0x6003d60)
site(0x3b56,0x25,'callvirt',0x6002fee); site(0x3b57,0x25,'callvirt',0x6002fee)
site(0x2ffd,6,'callvirt',0xa000d4a)
site(0x3b39,0x23,'callvirt',0x6003d5f); site(0x3b39,0x34,'call',0x6003b24)
site(0x3b39,0x3e,'stsfld',0x400235a)
slot(0x8e4,0x3d61,[0x3d61,0x437c]); slot(0x86e,0x3d61,[0x3d61,0x4052])
site(0x437d,0x18,'call',0x600313d); site(0x437e,0xe,'call',0x600313e)
site(0x4052,2,'stfld',0x4002614); br(0x4057,0x14,'brfalse.s',0x44)
# Separate recovery flag: manager update precedes its clear/restart branch.
site(0xacc,0x6e,'stsfld',0x4001376); site(0xacc,0x73,'call',0x6003b49)
site(0x226b,0xae3,'callvirt',0x6002964); site(0x226b,0xaf3,'callvirt',0x6002966)
co(0x226b,0xaf8,'ldc.r8',5.0); br(0x226b,0xb02,'ble.un.s',0xb34)
site(0x226b,0xb05,'stsfld',0x4001376); site(0x226b,0xb2f,'call',0x6003b52)
site(0x5f5,0x163,'ldsfld',0x4001376); br(0x5f5,0x168,'brtrue',0x39a)
# Finalization and controls remain separate from acceptance/commit.
site(0x2276,0x14,'ldsfld',0x4001376); br(0x2276,0x19,'brtrue',0x114)
site(0x225b,0x4e,'call',0x6002285); site(0x2262,0xb9,'call',0x6002285)
site(0xacd,7,'stfld',0x4001397); site(0x46ad,0x22a,'stsfld',0x4002c6c)
# Bounded focus connection: base refresh follows component iteration in its delegate.
site(0x2237,0x1d,'ldftn',0x6002239); site(0x2237,0x28,'callvirt',0x6004238)
site(0x2237,0x33,'ldftn',0x6002239); site(0x2237,0x3e,'callvirt',0x600423a)
assert stores(0x2985)=={('0x6004236',0xc)}
site(0x4236,0x2b,'callvirt',0xa000300); site(0x4236,0x44,'callvirt',0xa000300)
site(0x4249,0x11,'call',0x6004236); slot(0x95e,0x4249,[0x4249,0x469e])
assert not any(t=='0x6004249' for o,p,t in row(0x469e)['token_sites'])
site(0x46d7,0x14,'ldftn',0x60046fb)
site(0x46d7,0x25,'callvirt',0xa001863); site(0x46d7,0x2b,'call',0x6004249)
site(0x2429,0,'call',0x6002426); site(0x2239,0x16,'callvirt',0x6002261)
assert any(x['method']=='0x6002426' and x['import_name']=='GetForegroundWindow' for x in j['native_imports'])
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if int(f.parent.name)>=28: continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in {**j['focus_writer_bodies'],**methods}.items():
  if t in old and 'body_sha256' in r and 'body_sha256' in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t); shared+=1
for path,pin in [
 ('fixtures/16/single-linear-component.osu','f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84'),
 ('fixtures/24/single-spinner.osu','85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c'),
 ('validation/20/fixtures/circle-control.osu','682d600f7a76436c9ec17f1b2ccf52afc968ebf5ef81c3e74b282161c5d0aa0d')]:
 assert hashlib.sha256((Path(__file__).parents[1]/path).read_bytes()).hexdigest()==pin,path
print(f'PASS:119 original methods; pause/pending/resume/clock/update/recovery/finalization/dispatch and bounded focus sites; {shared} historical body hashes; no original or surrogate gameplay execution')
