#!/usr/bin/env python3
"""E23 original metadata/field provenance checker; no gameplay or score model."""
import json
import sys
from pathlib import Path

j=json.loads(Path(sys.argv[1]).read_text());m=j['methods']
assert j['expedition']==23 and len(m)==47
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert j['scan']=={'decoded_bodies':19659,'zero_rva':1739,'decode_errors':0}
def row(r):return m[hex(0x6000000+r)]
def site(r,o,op,t):assert [o,op,hex(t)]in row(r)['token_sites'],(hex(r),hex(o),op,hex(t))
def br(r,o,op,d):assert [o,op,[d]]in row(r)['branches'],(hex(r),hex(o),op,hex(d))
def co(r,o,op,v=None):assert [o,op,v]in row(r)['constants'],(hex(r),hex(o),op,v)
def ar(r,o,op):assert [o,op]in row(r)['arithmetic_sites'],(hex(r),hex(o),op)
def la(r,o,op,n=None):assert [o,op,n]in row(r)['local_argument_sites']
def writes(f):
    return {(k,o)for k,ss in j['entry_field_sites'].items()for o,op,t in ss
            if op in ['stfld','stsfld']and t==hex(0x4000000+f)}
def slot(root,s,expected):
    a=j['ordinary_dispatch'][hex(0x2000000+root)]['slots'][hex(0x6000000+s)]
    assert {x['method']for x in a}=={hex(0x6000000+x)for x in expected}
p=j['policy_identity']
assert (p['owner'],p['signature'],p['reference_type'])==('0x200059e','06129f90','0x20007e4')
assert p['name']=='#=z05t2c3Y=' and p['flags']==['fdAssembly']
assert not p['custom_attributes'] and not p['properties_returning_reference'] and not p['self_memberrefs']
assert 'tdSerializable'not in p['record_flags'] and 'tdAbstract'in p['reference_flags']
assert {x['token']for x in j['policy_family']}=={'0x200012f','0x20003e5','0x2000718','0x20007e3','0x20007e4'}
assert writes(0x198e)=={('0x6002e1f',o)for o in [0x25,0x32,0x3f,0x4c]}
assert not any(op in ['ldflda','ldsflda','ldtoken']and t=='0x400198e'
               for ss in j['entry_field_sites'].values()for o,op,t in ss)
assert row(0x2e1f)['branches']==[[8,'switch',[0x2b,0x45,0x1e,0x38]]]
for o,t in [(0x20,0x88d),(0x2d,0x1d5f),(0x3a,0x3c4f),(0x47,0x35c7)]:site(0x2e1f,o,'newobj',0x6000000+t)
assert [(r,o)for r,o,op,t in j['boundary_direct_callers']if t=='0x6002e1f']==[('0x6002e1c',0x51),('0x6002e1d',0x45)]
assert row(0x2e1f)['signature']=='000101129678'
assert {'mdPrivate','mdStatic'}<=set(j['writer_method_flags'])
assert all(not x.get('matching_memberrefs')for x in j['companion_field_reference_audit'])
assert len(j['companion_field_reference_audit'])==15
assert j['original_mod_enum']['namespace']=='osu_common'
assert j['original_mod_enum']['values']['ScoreV2']=={'field':'0x4003ced','value':536870912}
assert j['original_file_sections']['values']['HitObjects']==64
# Ordinary live versus recorded source predicate, plus nonordinary overrides.
slot(0x47b,0x229d,[0x229d]);slot(0x4af,0x229d,[0x229d,0x23f1])
slot(0xa10,0x229d,[0x229d,0x4aba])
site(0x229d,0,'call',0x6004390);br(0x229d,5,'brfalse.s',0x2b)
site(0x229d,0x13,'ldfld',0x4001970);co(0x229d,0x1d,'ldc.i4',536870912)
site(0x229d,0x31,'ldsfld',0x4000cc6);ar(0x229d,0x28,'cgt');ar(0x229d,0x3d,'cgt')
co(0x2237,0x3bc,'ldc.i4.0');site(0x2237,0x3bd,'call',0x6002e1c)
site(0x2237,0x3c2,'stsfld',0x4002a7f)
# Loader's count-based automatic bit and exact counter writers.
co(0x223b,0xc4,'ldc.i4',7000);br(0x223b,0xc9,'blt.s',0xf7)
site(0x223b,0xbf,'ldfld',0x4000ff2);site(0x223b,0xf2,'stsfld',0x4000cc6)
co(0x223b,0xfe,'ldc.i4.1');site(0x223b,0xff,'call',0x6002258)
co(0x223b,0x2b1,'ldc.i4.0');site(0x223b,0x2b2,'call',0x6002258)
assert writes(0xff2)=={('0x6001bc1',0x30),('0x6001bc1',0xf81),('0x6001bf2',0x369)}
co(0x1bc1,0x284,'ldc.i4.s',64);br(0x1bc1,0x33c,'beq',0xe83)
for o,f in [(0x356,0xfd9),(0x35c,0xfdb),(0x363,0xfda)]:site(0x1bf2,o,'ldfld',0x4000000+f)
ar(0x1bf2,0x361,'add');ar(0x1bf2,0x368,'add')
# Create, alias, reset and restore. Reset may mutate contents, never presence.
br(0x2258,0x56,'brfalse.s',0x6f);site(0x2258,0x60,'callvirt',0x600229d)
site(0x2258,0x65,'call',0x6002e1c);site(0x2258,0x6a,'stsfld',0x40013c3)
site(0x2258,0x9f,'stfld',0x4001970);site(0x2258,0xe4,'callvirt',0x6000ace)
la(0x2e1c,0x4d,'ldarg.3');br(0x2e1c,0x4e,'brfalse.s',0x56)
site(0xace,6,'stfld',0x40005f3);site(0xace,0x12,'callvirt',0x6002b62)
site(0x2b62,0xf,'callvirt',0x6003c56)
site(0x3c54,6,'stfld',0x40023fb);site(0x3c54,0x13,'stfld',0x40023f5)
site(0x1d5f,2,'call',0x6003c54)
site(0x229e,0x124,'stfld',0x4002045);ar(0x229e,0x122,'cgt.un')
site(0x3756,0xaa,'callvirt',0xa000695);site(0x3756,0xaf,'stfld',0x4002045)
site(0x3758,0xbb,'ldfld',0x4002045);site(0x3758,0xc0,'callvirt',0xa000697)
site(0x2e1d,0x3d,'ldfld',0x4002045);br(0x2e1d,0x42,'brfalse.s',0x4a)
site(0x226e,0x10,'call',0x6002e1d);site(0x226e,0x3e,'stsfld',0x40013c3)
site(0x226e,0x43,'stfld',0x40005f3)
site(0x229f,0xde,'stfld',0x40023f9);site(0x229f,0xf3,'stfld',0x40023fa)
# Presence-only branch; separate fresh head reads and manager-local conversion.
slot(0x2af,0x24a9,[0x24a9,0x1465])
site(0x1465,0x52,'ldsfld',0x40013c3);site(0x1465,0x5e,'ldfld',0x400198e)
br(0x1465,0x58,'brtrue.s',0x5e);br(0x1465,0x66,'brfalse.s',0xc2)
ar(0x1465,0x2f,'conv.r8');ar(0x1465,0x3d,'conv.r8');ar(0x1465,0x3e,'div')
for o in [0x13,0x7a,0x9e]:site(0x1465,o,'ldfld',0x400152a)
br(0x1465,0x84,'bne.un.s',0x8c);br(0x1465,0xa8,'blt.s',0xb0)
assert not any(op.startswith('st')and t in ['0x400198e','0x400152a']for r in [0x1465,0x28d3]for o,op,t in row(r)['token_sites'])
assert not any(t=='0x400198e'for o,op,t in row(0x28d3)['token_sites'])
site(0x28d3,0x47,'ldsfld',0x40013c3);site(0x28d3,0x4f,'ldfld',0x40017ef)
co(0x28d3,0x59,'ldc.i4',16384);co(0x28d3,0x6d,'ldc.i4',520095494)
co(0x28d3,0x8b,'ldc.i4',-131072)
# Related optional consumer uses handler, not global record; data/list mutate.
br(0xad2,0x1308,'brtrue.s',0x1317);br(0xad2,0x1312,'ble',0x13bb)
site(0xad2,0x1318,'ldfld',0x40005f3);site(0xad2,0x131d,'ldfld',0x400198e)
br(0xad2,0x1326,'brfalse.s',0x1394)
site(0xad2,0x1352,'newobj',0x6001ad1);site(0xad2,0x1357,'callvirt',0x6003c57)
for o,f in [(8,0xf5a),(0xf,0xf58),(0x16,0xf57)]:site(0x1ad1,o,'stfld',0x4000000+f)
assert not row(0x3c57)['virtual']
site(0x3c57,0x53,'callvirt',0xa00163d);site(0x3c57,0x5a,'callvirt',0x6003c58)
for o in [0x1f,0x3e,0x47]:site(0x3c57,o,'stfld',0x4000f59)
slot(0x3e5,0x3c58,[0x3c58]);slot(0x3e5,0x3c56,[0x3c56])
shared=0
for f in sorted((Path(__file__).parents[1]/'evidence').glob('*/*.json')):
    if int(f.parent.name)>=23:continue
    old=json.loads(f.read_text()).get('methods',{})
    for t,r in m.items():
        if t in old and 'body_sha256'in r and 'body_sha256'in old[t]:
            assert r['body_sha256']==old[t]['body_sha256'],(str(f),t);shared+=1
print(f'PASS: 47 original methods; exact four F198e stores, metadata, factory/source/restore/consumer gates; {shared} historical body hashes; static only, E20 NOT RUN')
