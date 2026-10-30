#!/usr/bin/env python3
"""E27 checks original instruction evidence, never executes a completion predicate."""
import hashlib
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==27 and len(m)==66
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,p,t):assert [o,p,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),p,hex(t))
def br(r,o,p,d):assert [o,p,[d]]in row(r)['branches'],(hex(r),hex(o),p,hex(d))
def co(r,o,p,v=None):assert [o,p,v]in row(r)['constants'],(hex(r),hex(o),p,v)
def ar(r,o,p):assert [o,p]in row(r)['arithmetic_sites'],(hex(r),hex(o),p)
def writes(f):return {(t,o)for t,ss in j['entry_field_sites'].items()for o,p,v in ss if p in ['stfld','stsfld']and v==hex(0x4000000+f)}
def slot(root,s,expected):
 a=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
 assert {x['method']for x in a}=={hex(0x6000000+x)for x in expected}
# Pinned whole-session methods, not guessed names or all-evaluated simulation.
assert row(0x226b)['body_sha256']=='de32a6e69eedceee8be3f629c986d16f35fe7b99952e0389a3146b1b8863f52f'
assert row(0x2276)['body_sha256']=='f4fd9f4681de7532e41bf54a1bc2430883e714921e09cd4d60e2d2f10d66406a'
assert row(0xacd)['body_sha256']=='b3b2b82998a7202026f3377cdda41ab369015b411557e91d68a265299b2f3391'
assert writes(0x1397)=={('0x6000acd',7)}
assert writes(0x13c4)=={('0x600226b',0xf0b)}
assert writes(0x13bf)=={('0x600222e',0x1f),('0x600223b',0x203)}
assert writes(0x13a2)=={('0x600222e',0x14),('0x6002271',0xd),('0x6002271',0xa8)}
assert writes(0x13a7)=={('0x6002243',6)}
co(0x222e,0x1a,'ldc.i4',-2147483648);site(0x223b,0x1ef,'ldfld',0x4002524)
br(0x223b,0x1fa,'ble.s',0x208);site(0x223b,0x203,'stfld',0x40013bf)
# Empty load validation uses a different request, not a successful end predicate.
br(0x223b,0x1a0,'brfalse.s',0x1b5);site(0x223b,0x1bc,'callvirt',0x600225a)
co(0x225a,0,'ldc.i4.5');site(0x225a,2,'call',0x60046ad)
# Authoritative ordinary object work precedes this later Play condition.
site(0x226b,0x94e,'call',0x6002275);site(0x226b,0x959,'callvirt',0x60028e2)
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x226b,0x972,'brfalse.s',0x9c9);br(0x226b,0x9a6,'brfalse.s',0x9c9)
site(0x226b,0xe5f,'call',0x6002271);br(0x226b,0xe6a,'bne.un',0x106d)
br(0x226b,0xe74,'brtrue',0x106d)
for o,p,d in [(0xe8b,'ble.s',0xf0a),(0xe9e,'bge.s',0xf0a),(0xec7,'blt.s',0xf0a),
               (0xef0,'bgt.s',0xf0a),(0xef9,'bge.s',0xf0a),(0xf00,'brfalse.s',0xf0a)]:br(0x226b,o,p,d)
site(0x226b,0xeab,'ldfld',0x40017fb);site(0x226b,0xeb6,'ldfld',0x4002523)
site(0x226b,0xedf,'ldfld',0x40017fc);site(0x226b,0xeeb,'ldfld',0x4002524)
site(0x226b,0xef3,'ldfld',0x40013a2);site(0x226b,0xefb,'ldsfld',0x4002368)
site(0x226b,0xf03,'call',0x600226f);site(0x226b,0xf0b,'stfld',0x40013c4)
site(0x226b,0xf11,'call',0x6002276)
# Queue/marker predicates do not read evaluated flags.
site(0x2271,0xd,'stfld',0x40013a2);site(0x2271,0x8d,'ldfld',0x40017fd)
site(0x2271,0x93,'ldftn',0x60022b1);site(0x2271,0xa8,'stfld',0x40013a2)
site(0x22b1,1,'ldfld',0x4002523);site(0x22b1,0x14,'ldfld',0x40013a2)
br(0x226f,0x1f,'ble.s',0x30);co(0x226f,0x2c,'ldc.i4.8');ar(0x226f,0x2d,'clt')
# Actual finalization: latch/keep-going/suspension gates, q0 shortcut, strict max+200.
br(0x2276,6,'brfalse.s',9);br(0x2276,0xf,'brtrue',0x114)
br(0x2276,0x19,'brtrue',0x114);br(0x2276,0x24,'ble.s',0x2d)
br(0x2276,0x2b,'brfalse.s',0x43);co(0x2276,0x38,'ldc.i4',200)
ar(0x2276,0x3d,'add');br(0x2276,0x3e,'ble',0x114)
site(0x2276,0x48,'ldfld',0x4001983);site(0x2276,0x53,'callvirt',0x6000a9f)
site(0x2276,0xc6,'call',0x6002284);site(0x2276,0xda,'callvirt',0x6002278)
site(0x2276,0xe7,'callvirt',0x6000aa5);br(0x2276,0xec,'brfalse.s',0xf5)
site(0x2276,0xef,'callvirt',0x6002277);site(0x2276,0xfa,'callvirt',0x6002b5a)
site(0x2276,0x10f,'callvirt',0x6002277);site(0x2b5a,2,'stfld',0x4001990)
slot(0x253,0xaa5,[0xaa5]);slot(0x253,0xacd,[0xacd])
for o,f in [(6,0x196a),(0x11,0x1969),(0x1d,0x196b),(0x29,0x196e),(0x35,0x17fc)]:site(0xaa5,o,'ldfld',0x4000000+f)
ar(0xaa5,0x3a,'ceq');site(0x2277,0xc,'callvirt',0x2b000084)
assert j['method_specs']['0x2b000084']['rid']==0x2cee
site(0x2277,0x17,'callvirt',0x6000acd);site(0xacd,7,'stfld',0x4001397)
site(0xacd,0x12,'ldsfld',0x400136a);site(0xacd,0x1a,'stfld',0x4001978)
site(0xacd,0x31,'ldfld',0x4001975);site(0xacd,0x3d,'stfld',0x400197c)
for r in [0x2276,0x2277,0x226f,0x2271,0x22b1,0xacd,0x2285,0x2286]:
 assert not any(t=='0x4001527'for o,p,t in row(r)['token_sites'])
assert not any(o>=0xe5e and t=='0x4001527'for o,p,t in row(0x226b)['token_sites'])
# Conditional first automatic departure, with exact equality and separate event horizon.
co(0x226b,0xf1c,'ldc.i4',3000);ar(0x226b,0xf21,'add');br(0x226b,0xf27,'bgt.s',0xf69)
site(0x226b,0xf2f,'ldfld',0x4001b9f);br(0x226b,0xf39,'brfalse.s',0xf53)
co(0x226b,0xf46,'ldc.i4',500);ar(0x226b,0xf4b,'sub');br(0x226b,0xf51,'bgt.s',0xf69)
site(0x226b,0xf54,'ldfld',0x4001397);br(0x226b,0xf59,'brtrue.s',0xf63)
site(0x226b,0xf5c,'isinst',0x20004af);site(0x226b,0xf64,'call',0x6002285)
assert writes(0x1bb0)=={('0x60028f7',0xb60),('0x6002efd',0x15),('0x6002efe',0x15)}
for r in [0x2efd,0x2efe]:
 site(r,1,'ldfld',0x400086e);br(r,0xc,'ble.s',0x1a);site(r,0x15,'stfld',0x4001bb0)
assert [0xb60,'stfld','0x4001bb0']in j['aux_end_parser_sites']['token_sites']
# Failure and pause exclude the ordinary automatic tail on the reached paths.
br(0x226b,0xac2,'brfalse.s',0xad1);site(0x226b,0xac5,'call',0x6002272)
site(0x226b,0xacb,'call',0x6002262);assert 0xad0 in row(0x226b)['returns']
br(0x226b,0x83d,'brfalse.s',0x854);assert 0x853 in row(0x226b)['returns']
# Existing finalized-state controls can request earlier departure; no universal 3000-delay claim.
site(0x225b,0x46,'ldfld',0x4001397);site(0x225b,0x4e,'call',0x6002285)
site(0x2262,0xb1,'ldfld',0x4001397);site(0x2262,0xb9,'call',0x6002285)
# Original record-grade F is metadata, not a reconstructed complete grade formula.
assert j['enums']['0x2000241']['values']['F']==8
assert j['enums']['0x2000b4b']['values']['Rank']==7
assert j['enums']['0x2000b22']['values']['Break']==2
site(0x2b47,0x21,'ldfld',0x4001978);co(0x2b47,0x28,'ldc.i4.8')
site(0x2285,0x1d,'ldsfld',0x4002c73);site(0x2285,0x2c,'call',0x60046ad)
co(0x2285,0x2a,'ldc.i4.1');site(0x2285,0x37,'callvirt',0x6002b47)
co(0x2285,0x3c,'ldc.i4.8');br(0x2285,0x3d,'bne.un.s',0x5e)
co(0x2285,0x56,'ldc.i4.5');site(0x2285,0x58,'call',0x60046ad)
site(0x2285,0x60,'callvirt',0x6002286);co(0x2286,0,'ldc.i4.7')
site(0x2286,2,'call',0x60046ad);slot(0x47b,0x2286,[0x2286])
# Accepted request != committed transition; original request guards remain pinned.
site(0x46ad,0x22a,'stsfld',0x4002c6c);site(0x46ad,0x2e8,'stsfld',0x4002c6b)
site(0x46e6,0xc,'call',0x60046ad);br(0x46ad,0x1ad,'bne.un.s',0x1b0)
site(0x46a3,0x102,'callvirt',0xa000300);site(0x46ef,1,'call',0x60046b0)
site(0x46b0,0x72,'stsfld',0x4002c6d);site(0x46b0,0xca,'callvirt',0x6001ab0)
assert next(ds for o,p,ds in row(0x46b0)['branches']if o==0x255)[7]==0x4b5
site(0x46b0,0x4ba,'newobj',0x60032d0);site(0x46b0,0x564,'callvirt',0x6001ab1)
# Audio source state is read by timing; no direct audio-to-Rank callback is inferred.
site(0x3b25,0xbf,'callvirt',0x6003d62);site(0x3b25,0xce,'stsfld',0x4002368)
slot(0x8e4,0x3d62,[0x3d62,0x4385]);slot(0x86e,0x3d62,[0x3d62,0x4053])
site(0x4385,6,'call',0x600313a);assert j['native_imports']==[{'method':'0x600313a','import_name':'BASS_ChannelIsActive','module':'bass.dll'}]
site(0x226b,0x937,'call',0x6003b58);site(0x3b58,6,'call',0x6003b59)
site(0x3b59,0x34,'stsfld',0x4002368)
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if int(f.parent.name)>=27:continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in {**j['focus_writer_bodies'],**m}.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t);shared+=1
for name,pin in [('24/single-spinner.osu','85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c'),('16/single-linear-component.osu','f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84')]:
 assert hashlib.sha256((Path(__file__).parents[1]/'fixtures'/name).read_bytes()).hexdigest()==pin
print(f'PASS:66 original methods; completion/finalization/end thresholds/record/Rank request/guards/conditional commit/order; {shared} historical body hashes; no original or surrogate gameplay execution')
