#!/usr/bin/env python3
"""Assert original E09 metadata and instruction sites; no substitute drawing execution."""
import json
import sys
from pathlib import Path
j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==9 and len(m)==72
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def site(r,o,op,t):assert [o,op,hex(t)] in m[hex(0x6000000+r)]['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,d] in m[hex(0x6000000+r)]['branches'],(hex(r),hex(o))
def const(r,o,op,v):assert [o,op,v] in m[hex(0x6000000+r)]['constants'],(hex(r),hex(o))
def ar(r,o,op):assert [o,op] in m[hex(0x6000000+r)]['arithmetic_sites'],(hex(r),hex(o),op)
def binding(ref,decl):
    matches=j['generic_member_refs'][hex(0xa000000+ref)]['same_name_declarations']
    assert hex(decl) in {v['token'] for v in matches},(hex(ref),hex(decl))
# Closed original receiver hierarchy and sole managed construction/restore writers.
assert {v['type'] for v in j['receiver_descendants']}=={'0x200017a','0x200055e'}
assert {v['method'] for v in j['slot_candidates']['0x60031f5']}=={'0x60031f5','0x6000a3d','0x6002929'}
assert m['0x60031f5']['abstract'] and m['0x60031f5']['rva']=='0x0'
assert {k for k,ss in j['receiver_field_sites'].items() if any(op=='stfld' and t=='0x4001764' for _,op,t in ss)}=={'0x60027cd','0x60027da','0x60038be'}
site(0x27cd,0x16,'stfld',0x4001764)
site(0x27ce,3,'newobj',0x600291d);site(0x27ce,9,'call',0x60027cd)
site(0x2c2b,0x4b,'newobj',0x6000a35);site(0x2c2b,0x51,'newobj',0x60027cd)
site(0x2c29,0x1f,'newobj',0x600291d);site(0xa35,0xf,'stfld',0x40005c1)
br(0x27e3,0x28,'brfalse.s',[0x34])
site(0x27e3,0x2d,'callvirt',0x6002c2b);site(0x27e3,0x36,'newobj',0x60027ce)
assert 0x10 in m['0x60038b4']['null_sites']
site(0x38b4,0x11,'call',0x60038b5);br(0x38b5,0x5f,'brfalse.s',[0x63])
# Face resource identity, exact argument fields and guards.
site(0x45ec,0x252,'newobj',0x6004510);site(0x45ec,0x25e,'stfld',0x4002bf9)
site(0x4510,0x11,'stfld',0x4002b7e);site(0x451d,1,'ldfld',0x4002b7e)
site(0x451a,1,'call',0x6004737);site(0x451a,0x1b,'callvirt',0x600451d)
br(0x451a,0x22,'brfalse.s',[0x2c]);site(0x451a,0x25,'callvirt',0x60027d7)
site(0x27d7,0x14,'ldfld',0x4001762)
for o,t in [(0x11b,0x4001d31),(0x14c,0x4002b6d),(0x152,0x4002b8d),(0x158,0x4002cf3),(0x15e,0x4002b90)]:
    site(0x451a,o,'stfld' if o==0x11b else 'ldfld',t)
site(0x451a,0x138,'call',0x6004077);site(0x451a,0x16d,'callvirt',0x6002cfd)
site(0x451a,0x172,'callvirt',0x60031f5)
ar(0x4518,0xb6b,'conv.u1')
# Region forwarding; ready test before vertices, four appends in a fixed order.
site(0xa3d,1,'callvirt',0x60031f7);site(0xa3d,0x90,'callvirt',0x60031f5)
site(0x2929,1,'callvirt',0x60031f6);br(0x2929,6,'brtrue.s',[9])
site(0x292d,1,'callvirt',0x60031f7);br(0x292d,0xe,'bgt.s',[0x12])
site(0x292d,0x13,'ldfld',0x4001d30);site(0x292d,0x22,'call',0x6001e4a)
for o in [0x229,0x265,0x2a0,0x2db]:site(0x2929,o,'callvirt',0xa0010de)
assert sum(t=='0xa0010de' for _,_,t in m['0x6002929']['token_sites'])==4
site(0x2929,0x1ec,'ldsfld',0x4001827)
for o,t in [(0x200,0x4003634),(0x21a,0x4003636),(0x222,0x4003635)]:site(0x2929,o,'stfld',t)
# Generic metadata connects vertex append -> batch upload/draw -> concrete indexed buffer.
for ref,decl in [(0x10de,0x6000e16),(0xebe,0x6000e17),(0x807,0x6000e17),
 (0x804,0x6000e10),(0x805,0x400251d),(0x808,0x6003e35),(0x809,0x6003e33),
 (0x10dd,0x6003f9a),(0x16e3,0x60038ac),(0x16a5,0x6003e2f),(0x16a6,0x6003e30)]:binding(ref,decl)
site(0xe16,0x7e,'stelem',0x1b000079);site(0xe16,0xa2,'call',0xa000807)
br(0xe17,0xc,'bne.un.s',[0xf]);site(0xe17,0x2c,'callvirt',0xa000808)
site(0xe17,0x3e,'callvirt',0xa000809)
site(0x3f9b,0xb,'newobj',0xa0016e3)
site(0x3e35,0x34,'call',0x2b00019c)
assert j['buffer_subdata_spec']['name']=='BufferSubData'
site(0x3e33,0x26,'call',0xa0016a7);const(0x3e33,0x18,'ldc.i4',5123)
const(0x38b1,0,'ldc.i4.4',None)
for r in [0x38af,0x38b0]:ar(r,2,'mul');ar(r,4,'div')
# Flush can occur before a new texture, during capacity append or after manager iteration.
site(0x1e4a,0x19,'callvirt',0xa000ebe);site(0x1e4a,0x24,'call',0xa000ebf)
site(0x2d00,0x18,'callvirt',0xa000ebe);site(0x2cf0,0x503,'call',0x6002d00)
site(0x2cff,0x50,'newobj',0xa0010dd)
# Assembly-qualified backend: don't confuse DLL tokens with EXE tokens.
o=j['opentk'];assert o['sha256']=='b415f3e061d9758316074dcbf31d6dba48cb0b89405254db94ead0e43ed88511'
assert o['draw_int_offset_overloads']==['0x6002fd5']
assert [9,'call','0x60030b2'] in o['methods']['0x6002fd5']['token_sites']
assert [12,'calli','0x110002a6'] in o['methods']['0x60030b2']['token_sites']
assert [9,'ldc.i4.s',94] in o['methods']['0x60030b2']['constants']
assert o['native_call_signature']=='02040108080818'
assert o['entry_points_field']['name']=='EntryPoints' and o['entry_points_field']['signature']=='061d18'
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('0[4-8]/*.json')):
    old=json.loads(f.read_text()).get('methods',{})
    for t,r in m.items():
        if t in old and 'body_sha256' in r and 'body_sha256' in old[t]:
            assert r['body_sha256']==old[t]['body_sha256'],(str(f),t)
            shared+=1
print(f'PASS: 72 EXE methods + 2 OpenTK methods; receiver hierarchy/writers, argument/guard/vertex sites, generic batch/flush/native boundary; {shared} historical hashes; no runtime/pixel execution')
