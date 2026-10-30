#!/usr/bin/env python3
"""E29 verifies original CIL sites/metadata, never simulates seeking or results."""
import hashlib
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==29 and len(m)==98
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,p,t):assert [o,p,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),p,hex(t))
def br(r,o,p,d):assert [o,p,[d]]in row(r)['branches'],(hex(r),hex(o),p,hex(d))
def co(r,o,p,v=None):assert [o,p,v]in row(r)['constants'],(hex(r),hex(o),p,v)
def ar(r,o,p):assert [o,p]in row(r)['arithmetic_sites'],(hex(r),hex(o),p)
def stores(f):return {(t,o)for t,ss in j['entry_field_sites'].items()for o,p,v in ss if p in ['stfld','stsfld']and v==hex(0x4000000+f)}
def calls(r):return [x for x in j['boundary_direct_callers']if x[-1]==hex(0x6000000+r)]
def slot(root,s,wanted):
 assert {x['method']for x in j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]}=={hex(0x6000000+x)for x in wanted}
assert row(0x3b3a)['body_sha256']=='010e12ccb0251199e280dada9bff835c7306626a8fce3ca0d473fc722d627606'
assert row(0x226e)['body_sha256']=='cd4785e46d96c12cdeda72431a48c6f584a53922bb12013ad7f7532fb50c48a3'
assert row(0x229f)['body_sha256']=='1b97c014c1f63608f54cd5efa5969e5a291b36edb932632120bff1cc3732dec2'
# Exact clock seek priority; unchanged early return is not a successful native seek.
site(0x3b3a,0,'ldsfld',0x400235b);br(0x3b3a,6,'bne.un.s',0x14)
site(0x3b3a,8,'ldsfld',0x4002358);br(0x3b3a,0xe,'bne.un.s',0x14)
br(0x3b3a,0x11,'brtrue.s',0x14);assert 0x13 in row(0x3b3a)['returns']
site(0x3b3a,0x21,'call',0x6003b65);site(0x3b3a,0x27,'stsfld',0x4001bb8)
br(0x3b3a,0x33,'bge.s',0x49);br(0x3b3a,0x47,'br.s',0x67)
br(0x3b3a,0x55,'ble.s',0x67);site(0x3b3a,0xa8,'stsfld',0x4002358)
site(0x3b3a,0xad,'call',0x6003b24);br(0x3b3a,0xbc,'brfalse.s',0x116)
site(0x3b3a,0xcc,'call',0xa00038e);co(0x3b3a,0xd1,'ldc.i4',200)
ar(0x3b3a,0xd6,'cgt');br(0x3b3a,0xda,'brfalse.s',0xee)
site(0x3b3a,0xe9,'callvirt',0x6003d5f);site(0x3b3a,0xef,'stsfld',0x4002368)
site(0x3b3a,0xf4,'call',0x6003b24);site(0x3b3a,0xfe,'stsfld',0x400235a)
site(0x3b3a,0x106,'stsfld',0x4002353);site(0x3b3a,0x10c,'stsfld',0x4002382)
site(0x3b3a,0x111,'call',0x6003b24);site(0x3b3a,0x117,'call',0x6003b66)
# Hold release/reset are different ordered operations and do not directly touch objects.
site(0x3b39,0x23,'callvirt',0x6003d5f);site(0x3b39,0x29,'stsfld',0x4002382)
site(0x3b39,0x2f,'stsfld',0x400234c);site(0x3b39,0x34,'call',0x6003b24)
site(0x3b39,0x3e,'stsfld',0x400235a)
site(0x3b4f,6,'stsfld',0x4002358);site(0x3b4f,0xc,'stsfld',0x4002359)
site(0x3b4f,0x28,'call',0x6003d64);site(0x3b4f,0x2e,'stsfld',0x4002368)
slot(0x8e4,0x3d5f,[0x3d5f,0x437f]);slot(0x86e,0x3d5f,[0x3d5f,0x4050])
assert any(x['method']=='0x6003143'and x['import_name']=='BASS_ChannelSeconds2Bytes'for x in j['native_imports'])
assert any(x['method']=='0x600314e'and x['import_name']=='BASS_ChannelSetPosition'for x in j['native_imports'])
# Startup time work, separate loading mark-past pass and recognized skip.
site(0x2269,0x165,'call',0x6003b4f);site(0x2269,0x180,'call',0x6003b59)
site(0x2269,0x19c,'ldftn',0x60022b0);site(0x22b0,0x34,'call',0x600224c)
site(0x224c,0xaf,'call',0x6003b59);site(0x224c,0xc5,'call',0x6003b3a)
assert calls(0x2241)==[['0x600223b',0x424,'call','0x6002241']]
br(0x2241,0x22,'bgt.s',0x2f);br(0x2241,0x2a,'ble',0xba)
site(0x2241,0x55,'ldsfld',0x4002358);br(0x2241,0x5a,'bgt.s',0x99)
site(0x2241,0x6e,'stfld',0x4001527);site(0x2241,0x8f,'ldfld',0x4001529)
site(0x2241,0x94,'callvirt',0x6002979)
assert not any(t=='0x60028d3'for o,p,t in row(0x2241)['token_sites'])
site(0x2256,0xbc,'call',0x6003b3a);co(0x2256,0xa8,'ldc.i4.s',-80)
co(0x2256,0xaa,'ldc.i4.s',90);site(0x2256,0xac,'call',0x6002333)
# The reset slot clears evaluated only after its transform callback loop.
slot(0x94c,0x24b3,[0x24b3,0x45f6]);slot(0x6f6,0x24b3,[0x24b3,0x45f6])
slot(0x2af,0x24b3,[0x24b3,0x1431]);slot(0xd8,0x24b3,[0x24b3,0x606])
site(0x24b3,0x14,'ldftn',0x60024ce);site(0x24b3,0x2c,'stfld',0x4001527)
co(0x24cf,6,'ldc.i4',148);site(0x45f6,1,'call',0x60024b3)
site(0x1431,1,'call',0x60024b3);site(0x1431,7,'callvirt',0x60024c4)
site(0x1431,0xd,'ldfld',0x4000bdf);site(0x1431,0x12,'callvirt',0x60024b3)
site(0x1431,0x19,'stfld',0x4000bf4);site(0x606,1,'call',0x60024b3)
for r in [0x24b3,0x45f6,0x1431,0x606]:
 assert not any(t=='0x400152a'and p in ['stfld','stsfld']for o,p,t in row(r)['token_sites'])
for f in [0xbf1,0xbf5,0xbf6,0xbf8,0xbf2,0xbf3,0x29e,0x2a2,0x2b4]:
 assert not any(t==hex(0x4000000+f)for o,p,t in row(0x1431)['token_sites'])
assert stores(0xbf1)=={('0x6001465',0x24),('0x600146f',0x2d1)}
assert stores(0xbf5)=={('0x600146f',0x31e)}
assert stores(0x29e)=={('0x60005f6',0x4c7)}
assert stores(0x2a2)=={('0x60005f6',0x2ed)}
# A later tracking exit resets separate typed children, not the embedded head.
site(0x146e,0xc8,'ldfld',0x4001527);site(0x146e,0xd0,'ldfld',0x4000bd5)
site(0x146e,0xe3,'ldftn',0x6001487);site(0x1487,1,'callvirt',0x60024b3)
site(0x2be4,0x1d,'call',0x60045f6)
assert j['fields']['0x4000bd5']['signature']=='06151282c1011296c4'
# Record replacement exists, but no natural managed inbound connection is invented.
assert calls(0x226e)==[]
ident=j['restore_method_identity'];assert ident['self_memberref_aliases']==[]and ident['methodspec_aliases']==[]
assert 'mdPrivate'in ident['flags']and row(0x226e)['virtual']is False
assert j['snapshot_metadata']['extends_name']=='System.ValueType'
assert len(j['snapshot_metadata']['fields'])==18
site(0x226e,0x10,'call',0x6002e1d);site(0x226e,0x21,'stfld',0x4001973)
site(0x226e,0x31,'stfld',0x4001980);site(0x226e,0x3e,'stsfld',0x40013c3)
site(0x226e,0x43,'stfld',0x40005f3);site(0x226e,0x5d,'callvirt',0x6002979)
site(0x226e,0x77,'callvirt',0x60040f1);site(0x226e,0x91,'stfld',0x40010f3)
site(0x2e1d,0x36,'newobj',0x6002b40);site(0x2e1d,0x3d,'ldfld',0x4002045)
site(0x2e1d,0x45,'call',0x6002e1f)
# Connected late selective restore and snapshot marker provenance.
site(0x47a7,6,'call',0x6001df7);site(0x1df7,0x268,'stsfld',0x4001120)
site(0x1df7,0x273,'stsfld',0x4001121);br(0x1df7,0x255,'ble.s',0x278)
site(0x226b,0x109e,'ldsfld',0x4002a80);br(0x226b,0x10af,'bne.un.s',0x10bc)
site(0x226b,0x10b1,'call',0x600229f);site(0x226b,0x10b7,'stsfld',0x4001120)
assert calls(0x229f)==[['0x600226b',0x10b1,'call','0x600229f']]
site(0x229f,0,'ldsfld',0x4001121);site(0x229f,0x8b,'callvirt',0x60040fc)
site(0x40fc,0x1e,'callvirt',0x60040f1);site(0x40f1,2,'stfld',0x4002674)
site(0x229f,0xde,'stfld',0x40023f9);site(0x229f,0xf3,'stfld',0x40023fa)
for r in [0x226e,0x229f]:
 assert not any(t in ['0x4002358','0x400235a','0x4002042','0x4001527','0x400152a','0x40013d9']for o,p,t in row(r)['token_sites'])
# Recorded frame overwrite follows clock; it does not repair previous/copy/working time.
site(0x43a0,0xca,'call',0x6003b25);site(0x43a0,0xe0,'call',0x60043a1)
site(0x43a1,0x12e,'ldsfld',0x4001373);site(0x43a1,0x143,'stsfld',0x4002a7a)
site(0x43a1,0x2bf,'stsfld',0x4002a9f);br(0x43a1,0x2c9,'brfalse',0x54b)
site(0x43a1,0x2d8,'stsfld',0x4002358)
assert not any(t in ['0x400235a','0x4002359','0x4002352','0x400235b']and p=='stsfld'for o,p,t in row(0x43a1)['token_sites'])
# Retained time gate; E12 update rejoin and spinner component/final distinctions.
assert stores(0x13d9)=={('0x600226b',0x9b3)}
br(0x226b,0x9a6,'brfalse.s',0x9c9);site(0x226b,0x959,'callvirt',0x60028e2)
site(0x226b,0x9be,'callvirt',0x6000ac5);site(0x226b,0x9cf,'callvirt',0x60028e3)
br(0x22c5,0x26,'bge.s',0x32)
assert not any(t=='0x4001527'for o,p,t in row(0xac5)['token_sites'])
assert not any(t=='0x4001527'for o,p,t in row(0x5f6)['token_sites'])
site(0x5f5,1,'ldfld',0x4001527);br(0x5f6,0x35,'bge.un.s',0x39)
site(0x28d3,1,'ldfld',0x4001527)
assert stores(0x1397)=={('0x6000acd',7)}
assert stores(0x13bf)=={('0x600222e',0x1f),('0x600223b',0x203)}
site(0x226b,0xf11,'call',0x6002276)
# Scoped timing callback, not a pure/no-callback seek assumption.
site(0x3b65,1,'stsfld',0x400238e);site(0x3b65,7,'stsfld',0x400238f)
site(0x3b66,0xd,'ldsfld',0x4002358);co(0x3b66,0x12,'ldc.i4.5')
site(0x3b84,0xb,'callvirt',0x6003b8c);site(0x2246,0x46,'call',0x6003b82)
site(0x2249,0x23,'callvirt',0x6000ad8);slot(0x253,0xad8,[0xad8])
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
 if int(f.parent.name)>=29:continue
 old=json.loads(f.read_text()).get('methods',{})
 for t,r in {**j['focus_writer_bodies'],**m}.items():
  if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
   assert r['body_sha256']==old[t]['body_sha256'],(str(f),t);shared+=1
for p,pin in [('fixtures/16/single-linear-component.osu','f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84'),('fixtures/24/single-spinner.osu','85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c'),('validation/20/fixtures/circle-control.osu','682d600f7a76436c9ec17f1b2ccf52afc968ebf5ef81c3e74b282161c5d0aa0d')]:
 assert hashlib.sha256((Path(__file__).parents[1]/p).read_bytes()).hexdigest()==pin,p
print(f'PASS:98 original methods; seek/hold/initialization/partial reset/record/frame retention and update order; {shared} historical body hashes; no original or surrogate gameplay execution')
