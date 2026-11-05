#!/usr/bin/env python3
"""E040 independently authored literal relationships and optional archive audit.

Does not import the E040 collector, run original CLR code, simulate playback or
call a native library. Full archive audit independently inventories watched fields.
"""
import hashlib
import json
from pathlib import Path
import sys
import zipfile

ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
def sha(b): return hashlib.sha256(b).hexdigest()

def check(d,archive=None):
    assert d['expedition']==40 and (d['archive_sha256'],d['exe_sha256'])==(ARCHIVE,EXE)
    assert d['scan']=={'decoded':19659,'zero_rva':1739,'errors':[]}
    assert d['tools']=={'dnfile':'0.18.0','pefile':'2024.8.26','dncil':'1.0.2'}
    def r(m):return d['methods'][hex(0x6000000+m)]
    def site(m,o,op,t):assert [o,op,hex(t)] in r(m)['token_sites'],(hex(m),hex(o),op,hex(t))
    def field(m,o,op,f):site(m,o,op,0x4000000+f)
    def call(m,o,n):site(m,o,'call',0x6000000+n)
    def br(m,o,op,t):assert [o,op,[t]] in r(m)['branches'],(hex(m),hex(o),op,hex(t))
    def const(m,o,op,v=None):assert [o,op,v] in r(m)['constants'],(hex(m),hex(o),op,v)
    def op(m,o,n):assert [o,n] in r(m)['operations'],(hex(m),hex(o),n)
    def stores(f):return [x for x in d['field_sites'] if x[3]==hex(0x4000000+f) and x[2].startswith('st')]

    # The only direct channel assignments are the two replacement stores. No owner.
    assert stores(0x2354)==[['0x6003b20',0xa9,'stsfld','0x4002354']]
    assert stores(0x2355)==[['0x6003b20',0xed,'stsfld','0x4002355']]
    assert len(d['channel_access_inventory'])==26
    assert stores(0x234b)==[['0x6003b21',0xc2,'stsfld','0x400234b']]
    assert any(x['method']=='0x600146c' and not x['new_slot'] for x in d['slots']['0x60024c4'])
    assert any(x['method']=='0x6001467' and not x['new_slot'] for x in d['slots']['0x60024bd'])
    assert all(d['fields'][hex(0x4000000+f)]['static'] for f in [0x2354,0x2355,0x2360,0x2361,0x238e,0x238f])
    assert d['fields']['0x4002354']['owner']=='0x20007b9'
    # Exact native imports distinguish stop/pause/free; no middleware assumptions.
    for m,name in {0x313a:'BASS_ChannelIsActive',0x313d:'BASS_ChannelPause',
      0x313e:'BASS_ChannelPlay',0x3146:'BASS_ChannelSetAttribute',0x3151:'BASS_ChannelStop',
      0x3196:'BASS_SampleFree',0x3197:'BASS_SampleGetChannel',0x3154:'BASS_Free'}.items():
        assert d['native_imports'][hex(0x6000000+m)]=={'module':'bass.dll','entry':name}

    # Global timing pointer initialization, full rescan vs streaming equality.
    const(0x3b08,0xd6,'ldc.i4.m1');field(0x3b08,0xd7,'stsfld',0x238e)
    const(0x3b08,0xdc,'ldc.i4.m1');field(0x3b08,0xdd,'stsfld',0x238f)
    field(0x3b65,1,'stsfld',0x238e);field(0x3b65,7,'stsfld',0x238f)
    call(0x3b68,5,0x3b6a);field(0x3b68,0,'ldsfld',0x238f)
    call(0x3b6a,0,0x3b72);br(0x3b6a,6,'brtrue.s',0xb);site(0x3b6a,0xc,'call',0x2b00018d)
    call(0x3b72,0,0x3b0e);field(0x3b72,0xb,'ldfld',0x100f)
    field(0x3b0e,0,'ldsfld',0x2367);field(0x3b0e,0xb,'ldfld',0x24ad)
    assert d['method_spec_resolutions']['0x2b00018d']['local_definition']=='0x60047cb'
    assert d['method_spec_resolutions']['0x2b0001b0']['local_definition']=='0x60047ca'
    site(0x47cb,2,'call',0x2b0001b0);br(0x47cb,7,'brtrue.s',0x13)
    br(0x47ca,2,'blt.s',0xe)
    field(0x3b66,0xd,'ldsfld',0x2358);const(0x3b66,0x12,'ldc.i4.5');op(0x3b66,0x13,'add')
    br(0x3b66,0x2c,'brfalse.s',0x34);br(0x3b66,0x35,'brtrue.s',0x47)
    br(0x3b66,0x3d,'blt.s',0x47);br(0x3b66,0x45,'beq.s',0xbd)
    field(0x3b66,0x6b,'stsfld',0x238e);field(0x3b66,0x88,'stsfld',0x238f)
    call(0x3b66,0xac,0x3b84);br(0x3b66,0xe4,'bgt.s',0x120)
    field(0x3b66,0xe7,'stsfld',0x238f);call(0x3b66,0xed,0x3b84)
    site(0x3b66,0xf3,'callvirt',0x6000b74);field(0x3b66,0xfb,'stsfld',0x238e)
    call(0x3b66,0x101,0x3b81);call(0x3b66,0x111,0x3b65);call(0x3b66,0x123,0x3b65)
    field(0x3b89,9,'ldfld',0x0662);op(0x3b89,0x14,'conv.r8')
    # These clt sites are independently tied to actual bytes in archive mode.
    for m,off in [(0x3b89,0x15),(0x3b8a,0xd),(0x47ca,0xb)]:
        assert [off,'clt'] in r(m)['comparison_sites']
    call(0x3b61,1,0x3b66);call(0x3b61,6,0x3b68)
    const(0x3b61,0x20,'ldc.i4.3');br(0x3b61,0x21,'ble.s',0x2f)
    field(0x3b61,0x24,'ldfld',0x0663);call(0x3b61,0x2a,0x3b1f)
    call(0x3b25,0x48,0x3b24);call(0x3b25,0x56,0x3b66);call(0x3b25,0xae,0x3b61)
    field(0x3b3c,0x1e,'ldfld',0x0661);field(0x3b3c,0x24,'ldsfld',0x236b)
    field(0x3b1f,0x2f,'stsfld',0x2361);call(0x3b1f,0x34,0x3b3c);field(0x3b1f,0x39,'stsfld',0x2360)
    br(0x3b1f,0x112,'brtrue.s',0x11b);field(0x3b1f,0x114,'ldsfld',0x2363);call(0x3b1f,0x11e,0x3b20)
    field(0x3b84,0,'ldsfld',0x2392);site(0x3b84,0xb,'callvirt',0x6003b8c)
    site(0x2246,0x3b,'ldftn',0x6002249);call(0x2246,0x46,0x3b82)
    call(0x2248,0x1e,0x3b83)
    assert not any(t=='0x6003b1f' for _,_,t in r(0x2249)['token_sites'])

    # Replacement compares global index and independent banks, then guarded Stop.
    for o,f in [(0x10,0x2360),(0x15,0x2362),(0x1d,0x2363),(0x2b,0x2360),(0x30,0x2362),(0x38,0x2364)]:field(0x3b20,o,'ldsfld',f)
    br(0x3b20,1,'brtrue.s',0xa);field(0x3b20,3,'ldsfld',0x2361)
    br(0x3b20,0x47,'brtrue.s',0x60);br(0x3b20,0x4d,'brfalse.s',0x5f)
    br(0x3b20,0x55,'beq.s',0x60);br(0x3b20,0x5d,'beq.s',0x60)
    field(0x3b20,0x65,'stsfld',0x2362)
    for active,guard,stop,bank,mode,res,get,assign in [
      (0x7a,0x7f,0x86,0x8d,0x9d,0x9e,0xa4,0xa9),
      (0xbe,0xc3,0xca,0xd1,0xe1,0xe2,0xe8,0xed)]:
        call(0x3b20,active,0x313a);br(0x3b20,guard,'bne.un.s',0x8c if bank==0x8d else 0xd0)
        call(0x3b20,stop,0x3151);field(0x3b20,bank,'stsfld',0x2363 if bank==0x8d else 0x2364)
        const(0x3b20,mode,'ldc.i4.2');call(0x3b20,res,0x3b36)
        const(0x3b20,get-1,'ldc.i4.0');call(0x3b20,get,0x3197)
        field(0x3b20,assign,'stsfld',0x2354 if bank==0x8d else 0x2355)
    for o in [0x117,0x124,0x131,0x143]:call(0x3b20,o,0x3146)
    const(0x3b20,0x110,'ldc.i4.s',11);const(0x3b20,0x13c,'ldc.i4.s',11)
    assert not r(0x3b20)['exception_regions']
    # No sample-player validation/owner/rate-matching branch in continuing helper.
    assert not any(t in ['0x6003b31','0x6001407','0x6003b37'] for _,_,t in r(0x3b20)['token_sites'])

    # Row eligibility, cached actions, captured pan, no duplicate-pending flag.
    call(0x146b,0x49,0x3b33);const(0x146b,0x14,'ldc.i4.s',14)
    br(0x146b,0x16,'blt.s',0x26);const(0x146b,0x3e,'ldc.r4',0.4000000059604645)
    site(0x146b,0x39,'callvirt',0x60024bd)
    field(0x3b33,9,'stfld',0x239b);call(0x3b33,0x11,0x3b20)
    br(0x3b33,0x17,'brtrue.s',0x22);br(0x3b33,0x20,'brtrue.s',0x5a)
    br(0x3b33,0x2d,'beq.s',0x5a);br(0x3b33,0x61,'brfalse.s',0x9b);br(0x3b33,0x6e,'beq.s',0x9b)
    for off,target in [(0x42,0x3ba1),(0x83,0x3ba2),(0xae,0x3b9c)]:site(0x3b33,off,'ldftn',0x6000000+target)
    for off in [0x54,0x95,0xba]:site(0x3b33,off,'callvirt',0x6002fee);const(0x3b33,off-1,'ldc.i4.2')
    br(0x3b33,0xa6,'brfalse.s',0xc0)
    for m,f in [(0x3ba1,0x2354),(0x3ba2,0x2355)]:
        field(m,0,'ldsfld',f);call(m,6,0x3b41);const(m,0xb,'ldc.r4',100.0)
        call(m,0x11,0x3146);field(m,0x17,'ldsfld',f);const(m,0x1c,'ldc.i4.1');call(m,0x1d,0x313e)
    for off,f in [(0,0x2354),(0x17,0x2355)]:field(0x3b9c,off,'ldsfld',f)
    const(0x3b9c,5,'ldc.i4.3');const(0x3b9c,0x1c,'ldc.i4.3')
    call(0x3b9c,0x11,0x3146);call(0x3b9c,0x28,0x3146)

    # Gain: integer multiplication precedes r4; null point fallback has no floor.
    field(0x3b40,0,'ldsfld',0x236c);field(0x3b40,0xa,'ldsfld',0x236e)
    op(0x3b40,0x14,'mul');op(0x3b40,0x15,'conv.r4');const(0x3b40,0x16,'ldc.r4',100.0)
    call(0x3b41,0,0x3b42);call(0x3b41,6,0x3b40);op(0x3b41,0xb,'mul');op(0x3b41,0x11,'div')
    const(0x3b42,7,'ldc.i4.s',100);call(0x3b42,0xa,0x3b68)
    const(0x3b42,0x13,'ldc.i4.8');field(0x3b42,0x15,'ldfld',0x0665)
    field(0x3b42,0x25,'ldfld',0x1000)
    call(0x3b11,0x20,0x3146);call(0x3b11,0x2d,0x3146)

    # Dedicated mode2 table, dynamic key, dictionary-only fallback, loop flag.
    field(0x3b36,0x90,'ldsfld',0x2376);field(0x3b36,0x20f,'ldsfld',0x2376)
    const(0x3b36,0x177,'ldc.i4',2064528794);const(0x3b36,0x183,'ldc.i4',2064528790)
    br(0x3b36,0x1d1,'brfalse.s',0x1db);const(0x3b36,0x1d5,'ldc.i4.m1');br(0x3b36,0x1d6,'beq.s',0x1db)
    for off in [0xef,0x10e]:const(0x3b19,off,'ldc.i4.1')
    for off in [0xf2,0x111]:call(0x3b19,off,0x3b14)
    const(0x3b14,0x4e,'ldc.i4',131072);const(0x3b14,0x58,'ldc.i4.4');op(0x3b14,0x59,'or')
    for off in [0x71,0x84]:const(0x3b18,off,'ldc.i4.1')
    for off,f in [(0x282,0x2360),(0x28c,0x2362),(0x292,0x2363),(0x298,0x2364)]:field(0x3b19,off,'stsfld',f)

    # Pause is queued, global rereads are separate, manager guard is not ownership.
    call(0x146c,0,0x3b53);site(0x3b53,0x13,'ldftn',0x6003ba8);site(0x3b53,0x25,'callvirt',0x6002fee)
    field(0x3ba8,0,'ldsfld',0x2354);call(0x3ba8,5,0x313d)
    field(0x3ba8,0xb,'ldsfld',0x2355);call(0x3ba8,0x10,0x313d)
    call(0x0653,1,0x28e3);field(0x0653,6,'ldsfld',0x136e);call(0x0653,0xd,0x3b09);call(0x0653,0x14,0x3b53)
    field(0x2275,0x56,'stsfld',0x136e);site(0x2275,0x4e,'callvirt',0x6003e4e)
    field(0x3b09,1,'ldsfld',0x2355);br(0x3b09,0xb,'beq.s',0x1b);field(0x3b09,0xe,'ldsfld',0x2354)
    site(0x1431,7,'callvirt',0x60024c4);field(0x1431,0x19,'stfld',0xbf4)
    site(0x1465,8,'callvirt',0x60024c4);site(0x1465,0x4d,'callvirt',0x60024bb)
    call(0x1449,0x11e,0x146b);field(0x1449,0x131,'stfld',0xbf4)
    br(0x1469,0xb,'bgt.s',0x13);call(0x1469,0xe,0x146b)
    site(0x2262,0x1da,'callvirt',0x60024c4);call(0x2262,0x20b,0x3b49)
    call(0x3b1c,0x15,0x3196);call(0x28f6,5,0x3b1d);call(0x3b5e,0xd1,0x3b1b);call(0x3b5e,0xd6,0x3154)
    # Existing queue boundary is asynchronous submission, not inline Action.Invoke.
    site(0x2fee,0x2f,'callvirt',0x6002fdd);site(0x30bf,0x19,'callvirt',0x60033f2)

    expected={2064529198:'sliderslide',2064529212:'sliderwhistle',2064528794:'slide',2064528790:'{0}{1}-slider{2}{3}'}
    for n,s in expected.items():assert d['strings'][str(n)]['text']==s
    assert [c['id'] for c in d['reference_cases']]==[f'L{i:02}' for i in range(1,57)]
    assert all(c['status']=='RECONSTRUCTED STATIC PREDICTION' and c['evidence'] for c in d['reference_cases'])
    root=Path(__file__).resolve().parents[1]
    for name,h in d['authority_inputs'].items():assert sha((root/'evidence'/name).read_bytes())==h
    for name,h in d['collector_sources'].items():assert sha((root/'tools'/name).read_bytes())==h
    shared=0
    for path in sorted((root/'evidence').glob('*/*.json')):
        if path.parent.name=='40':continue
        old=json.loads(path.read_text(encoding='utf-8'))
        for token,value in old.get('methods',{}).items():
            if token in d['methods'] and 'body_sha256' in value:
                assert value['body_sha256']==d['methods'][token]['body_sha256'],(path,token)
                shared+=1
    if archive:
        assert sha(Path(archive).read_bytes())==ARCHIVE
        with zipfile.ZipFile(archive) as z:raw=z.read('osu!.exe')
        assert sha(raw)==EXE
        import dnfile
        from dncil.cil.body.reader import read_method_body_from_bytes
        pe=dnfile.dnPE(data=raw);ts=pe.net.mdtables
        assert not any(x.Parent.table.name=='Field' and x.Parent.row_index in [0x2354,0x2355] for x in ts.CustomAttribute)
        for fid in [0x2354,0x2355,0x2360,0x2362]:
            assert not ts.Field.rows[fid-1].Flags.fdHasDefault and not ts.Field.rows[fid-1].Flags.fdHasFieldRVA
        owners={m.row_index:n for n,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
        watched={hex(0x4000000+x) for x in [0x2354,0x2355,0x2360,0x2361,0x2362,0x2363,0x2364,0x238e,0x238f,0x239b]}
        inventory=[];decoded=0
        for rid,m in enumerate(ts.MethodDef,1):
            if not m.Rva:continue
            b=read_method_body_from_bytes(pe.get_data(m.Rva));decoded+=1
            token=hex(0x6000000+rid)
            for i in b.instructions:
                v=getattr(i.operand,'value',None)
                if v is not None and hex(v) in watched:inventory.append([token,i.offset-b.header_size,i.opcode.name,hex(v)])
            if token not in d['methods']:continue
            value=d['methods'][token]
            assert sha(pe.get_data(m.Rva,b.size))==value['body_sha256']
            assert value['owner']==hex(0x2000000+owners[rid]) and value['signature']==m.Signature.value.hex()
            tokens=[[i.offset-b.header_size,i.opcode.name,hex(i.operand.value)] for i in b.instructions if hasattr(i.operand,'value')]
            assert tokens==value['token_sites']
            constants=[[i.offset-b.header_size,i.opcode.name,i.operand] for i in b.instructions if i.opcode.name.startswith('ldc.')]
            assert constants==value['constants']
            branches=[]
            for ins in b.instructions:
                name=ins.opcode.name
                if name=='switch' or name.startswith(('br','beq','bne','bge','bgt','ble','blt','leave')):
                    targets=ins.operand if name=='switch' else [ins.operand]
                    branches.append([ins.offset-b.header_size,name,[n-b.header_size for n in targets]])
            assert branches==value['branches']
            assert [[i.offset-b.header_size,i.opcode.name] for i in b.instructions if i.opcode.name in ['clt','clt.un','cgt','cgt.un','ceq']]==value['comparison_sites']
        assert decoded==19659 and inventory==[x for x in d['field_sites'] if x[3] in watched]
        imports={hex(0x6000000+x.MemberForwarded.row_index):{'module':str(x.ImportScope.row.Name),'entry':str(x.ImportName)} for x in ts.ImplMap}
        assert all(imports[t]==v for t,v in d['native_imports'].items())
        fowners={f.row_index:n for n,t in enumerate(ts.TypeDef,1) for f in t.FieldList}
        for token,f in d['fields'].items():
            rid=int(token,16)&0xffffff;actual=ts.Field.rows[rid-1]
            assert f['owner']==hex(0x2000000+fowners[rid])
            assert f['signature']==actual.Signature.value.hex() and f['static']==actual.Flags.fdStatic
        for token,resolution in d['method_spec_resolutions'].items():
            spec=ts.MethodSpec.rows[(int(token,16)&0xffffff)-1]
            local_rid=int(resolution['local_definition'],16)&0xffffff
            local=ts.MethodDef.rows[local_rid-1]
            assert spec.Instantiation.value.hex()==resolution['instantiation']
            assert spec.Method.row.Signature.value==local.Signature.value and str(spec.Method.row.Name)==str(local.Name)
            assert owners[local_rid]==0x98a
        pe.close()
    print(f'PASS E040: {len(d["methods"])} methods;26 direct channel accesses;{shared} historical body hashes;56 conditional controls;archive audit={bool(archive)};no runtime/native/audio execution')

if __name__=='__main__':
    check(json.loads(Path(sys.argv[1]).read_text(encoding='utf-8')),sys.argv[2] if len(sys.argv)>2 else None)
