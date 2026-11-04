#!/usr/bin/env python3
"""Independent literal-site E039 assertions; no gameplay/audio simulation.

Optional private archive argument independently checks selected original bytes,
identities and literal native imports without invoking the extraction function.
"""
import hashlib
import json
from pathlib import Path
import sys
import zipfile

ARCHIVE = '7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
EXE = '6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'

def check(d, archive=None):
    assert d['expedition'] == 39
    assert (d['archive_sha256'],d['exe_sha256']) == (ARCHIVE,EXE)
    assert d['scan'] == {'decoded':19659,'zero_rva':1739,'errors':[]}
    assert d['tools']=={'dnfile':'0.18.0','pefile':'2024.8.26','dncil':'1.0.2'}
    assert len(d['methods']) == 92
    def r(m): return d['methods'][hex(0x6000000+m)]
    def site(m,o,op,t):
        assert [o,op,hex(t)] in r(m)['token_sites'],(hex(m),hex(o),op,hex(t))
    def br(m,o,op,t):
        assert [o,op,[t]] in r(m)['branches'],(hex(m),hex(o),op,hex(t))
    def const(m,o,op,v=None):
        assert [o,op,v] in r(m)['constants'],(hex(m),hex(o),op,v)
    def op(m,o,name): assert [o,name] in r(m)['operations'],(hex(m),hex(o),name)
    def arr(m,o,name):
        assert any(x[:2] == [o,name] for x in r(m)['array_integer_sites']),(hex(m),hex(o),name)
    assert r(0x1469)['signature']=='2002010208'
    assert r(0x146a)['signature']=='200111a46008' and not r(0x146a)['virtual']
    assert r(0x3b35)['signature']=='00020111a4600c'

    # Ordinary factory, separate embedded reference and circle slot resolution.
    site(0x48e0,0x17,'newobj',0x600142d)
    for o,f in [(0x1f,0x1537),(0x27,0x1538),(0x2f,0xbfa),(0x37,0xbfb),(0x3f,0x1539),(0x47,0x1536)]:
        site(0x48e0,o,'stfld',0x4000000+f)
    site(0x48e0,0x50,'callvirt',0x60024b9)
    const(0x24b9,0x2f,'ldc.i4',2064529283);br(0x24b9,0x3f,'ble.s',0x56)
    const(0x24b9,0x42,'ldc.i4',2064459678);br(0x24b9,0x52,'bne.un.s',0x56)
    const(0x24b9,0x59,'ldc.i4.0');const(0x24b9,0x5a,'ldc.i4.0')
    site(0x48e0,0x5f,'callvirt',0x600146a)
    site(0x48e0,0x71,'stfld',0x4001537);site(0x48e0,0x82,'stfld',0x4001538)
    assert [t for o,name,t in r(0x48e0)['token_sites'] if name == 'stfld'] == [
        '0x4001537','0x4001538','0x4000bfa','0x4000bfb','0x4001539','0x4001536','0x4001537','0x4001538']
    site(0x143a,0x54,'newobj',0x600351a);site(0x143a,0x59,'stfld',0x4000bdf)
    site(0x143a,0x41,'callvirt',0xa00083e);br(0x143a,0x38,'brtrue.s',0x48)
    site(0x351a,0xa,'call',0x60045eb);site(0x351d,1,'call',0x6001884)
    for o in [9,0x11,0x19]:site(0x45eb,o,'call',0x60038e2)
    for o,opcode in [(8,'ldc.i4.2'),(0x10,'ldc.i4.4'),(0x18,'ldc.i4.8')]:const(0x45eb,o,opcode)
    const(0x45ec,0x2b,'ldc.i4.0');site(0x45ec,0x2c,'stfld',0x4002526)
    for o in [0x66,0x78,0x8a]:site(0x45ec,o,'stfld',0x4002526)
    for token in ['0x20006f6','0x200094c','0x200035a','0x2000828']:
        for slot in [0x24bb,0x24bd]:
            assert not any(x['owner']==token for x in d['slots'][hex(0x6000000+slot)])
    assert d['types']['0x20006f6']['extends']=='0x200094c'
    assert d['types']['0x20002af']['extends']=='0x2000339'
    assert any(x['method']=='0x6001466' and not x['new_slot'] for x in d['slots']['0x60024bb'])
    assert any(x['method']=='0x6001467' and not x['new_slot'] for x in d['slots']['0x60024bd'])
    site(0x1464,0x3b,'callvirt',0x60024a9)
    site(0x1884,0x7f,'callvirt',0x60024bb);site(0x1884,0x8e,'callvirt',0x60024b4)
    br(0x1884,0x7c,'ble.s',0x84)
    site(0x2295,0x1a2,'callvirt',0x6001464);const(0x2295,0x1b7,'ldc.i4.s',64)
    site(0x2295,0x1ba,'callvirt',0x6000ad2)
    site(0x24bb,0x14,'callvirt',0x6001be6);op(0x24bb,0x12,'add')
    site(0x249b,0x18,'stfld',0x4001534)

    # Audio progression is a separate latest-prefix scan, excluding final entry.
    const(0x1449,0xb8,'ldc.i4.m1');site(0x1449,0xc5,'callvirt',0xa000511)
    site(0x1449,0xca,'ldsfld',0x4002358);br(0x1449,0xcf,'bgt.s',0xe7)
    const(0x1449,0xe3,'ldc.i4.1');br(0x1449,0xe5,'blt.s',0xbe)
    br(0x1449,0xf2,'blt',0x224);br(0x1449,0x102,'bgt',0x224)
    site(0x1449,0x107,'ldsfld',0x4002368);const(0x1449,0x10c,'ldc.i4.1')
    br(0x1449,0x10d,'bne.un',0x224);site(0x1449,0x113,'ldfld',0x4000bf3)
    br(0x1449,0x118,'brfalse',0x216);site(0x1449,0x11e,'call',0x600146b)
    site(0x1449,0x125,'ldfld',0x4000bf4);br(0x1449,0x12a,'ble',0x22b)
    site(0x1449,0x131,'stfld',0x4000bf4)
    const(0x1449,0x13c,'ldc.i4.1');arr(0x1449,0x14f,'div');arr(0x1449,0x150,'rem')
    br(0x1449,0x152,'ble.s',0x163)
    const(0x1449,0x155,'ldc.i4.1');site(0x1449,0x15c,'call',0x6001469)
    const(0x1449,0x164,'ldc.i4.0');arr(0x1449,0x17f,'div');site(0x1449,0x180,'call',0x6001469)
    site(0x1449,0x217,'callvirt',0x60024c4)
    for o in [0x210,0x21e,0x226]:site(0x1449,o,'stfld',0x4000bf4)
    const(0x1449,0x225,'ldc.i4.m1')
    const(0x1431,0x18,'ldc.i4.0');site(0x1431,0x19,'stfld',0x4000bf4)
    assert {x[0] for x in d['field_sites'] if x[2]=='stfld' and x[3]=='0x4000bf4'} == {'0x6001431','0x6001449'}
    # No call from gameplay classifier to any discrete/loop dispatch helper.
    forbidden={hex(0x6000000+x) for x in [0x1469,0x146a,0x146b,0x3b31,0x3b32,0x3b33,0x3b35]}
    assert not any(t in forbidden for _,_,t in r(0x146f)['token_sites'])
    assert not any(t in {'0x4000bf1','0x4000bf5','0x4000bf6'} for _,_,t in r(0x1449)['token_sites'])

    # Tick uses timestamp+2, timing index/volume, direct ctor and mode1 (no floor).
    br(0x1469,0xb,'bgt.s',0x13);site(0x1469,0xe,'call',0x600146b)
    br(0x1469,0x14,'brfalse.s',0x82);site(0x1469,0x22,'ldfld',0x4000bf8)
    const(0x1469,0x2d,'ldc.i4.2');op(0x1469,0x2e,'add')
    site(0x1469,0x30,'callvirt',0x6001be6)
    for o,f in [(0x37,0x1537),(0x47,0x663),(0x4e,0x2526),(0x55,0x661),(0x5b,0x665),(0x61,0x1538)]:
        site(0x1469,o,'ldfld',0x4000000+f)
    site(0x1469,0x71,'newobj',0x60044a1);site(0x1469,0x7c,'call',0x6003b35)
    site(0x1469,0x84,'call',0x600146a);site(0x1469,0x8a,'call',0x60028a1)
    const(0x1469,0x95,'ldc.i4.1')
    site(0x1469,0x96,'call',0x6003b32)
    const(0x3b35,0x12,'ldc.i4.1');site(0x3b35,0x13,'call',0x6003b36)
    site(0x3b35,0x19,'ldfld',0x4002b3a);site(0x3b35,0x25,'call',0x6003b31)
    assert not any(t in ['0x60044a2','0x60028a1','0xa0000fc'] for _,_,t in r(0x3b35)['token_sites'])

    # Edge timing, short-list fallbacks and version14 head-base distinction.
    br(0x146a,3,'brtrue',0xd7);const(0x146a,0x19,'ldc.i4.5')
    site(0x146a,0x1c,'callvirt',0x6001be6)
    const(0x146a,0x84,'ldc.i4.s',14);br(0x146a,0x86,'bge.s',0x9f)
    br(0x146a,0x94,'brtrue.s',0x9f)
    br(0x146a,0xde,'bge.s',0xe4);const(0x146a,0xe1,'ldc.i4.m1')
    br(0x146a,0xe2,'bne.un.s',0x100)
    site(0x146a,0xf0,'ldfld',0x4002524);site(0x146a,0xf8,'callvirt',0x6001be6)
    site(0x146a,0x10c,'ldfld',0x4000bf7);site(0x146a,0x11c,'callvirt',0x6001be6)
    for o in [0xf5,0x119]:const(0x146a,o,'ldc.i4.5')
    for o in [0x14d,0x171,0x195]:br(0x146a,o,'blt.s',{0x14d:0x158,0x171:0x17c,0x195:0x1a0}[o])
    br(0x146a,0x1ae,'brfalse.s',0x1cc)
    for o,f in [(0x1b4,0x663),(0x1bb,0x661),(0x1c3,0x665)]:site(0x146a,o,'ldfld',0x4000000+f)
    const(0x146a,0x1cf,'ldc.i4.1');const(0x146a,0x1d1,'ldc.i4.0');const(0x146a,0x1d4,'ldc.i4.s',100)
    site(0x146a,0x1e4,'call',0x60044a2)
    for m in [0x1469,0x146a]:
        assert not any(t in ['0x4001534','0x4001539','0x4001536'] for _,_,t in r(m)['token_sites'])
    site(0x1465,8,'callvirt',0x60024c4);site(0x1465,0x4d,'callvirt',0x60024bb)
    br(0x1465,0x4a,'ble.un.s',0x52)
    site(0x1466,0xe,'call',0x6003e4d);arr(0x1466,0x14,'div')
    br(0x1466,0x16,'bgt.s',0x1b);site(0x1466,0x21,'callvirt',0xa00083d)
    site(0x1466,0x28,'call',0x6001469)

    # Parsed arrays: Int32.TryParse failure ignored; Convert.ToInt32 edge sets;
    # sound list bounded/padded; sets resized only when sound list exists.
    site(0x28f7,0x1874,'call',0xa000601)
    site(0x28f7,0x191d,'call',0xa000d73);site(0x28f7,0x1931,'call',0xa000d73)
    assert d['external_refs']['0xa000601']['name']=='System.Int32::TryParse'
    assert d['external_refs']['0xa000d73']['name']=='System.Convert::ToInt32'
    assert d['member_declaring_specs']['0xa00083e']=='151282c10111a61c'
    assert d['member_declaring_specs']['0xa00077c']=='151282c10111ac3c'
    assert d['member_declaring_specs']['0xa0002e1']=='15118801010c'
    site(0x28f7,0x185d,'call',0xa0002a6)
    for o in [0x196f,0x19b4]:site(0x28f7,o,'callvirt',0xa0010c4)
    for o in [0x1984,0x19c9]:site(0x28f7,o,'callvirt',0xa000aeb)
    site(0x28f7,0x1a4f,'call',0x6002d7c);site(0x28f7,0x1aa4,'callvirt',0x6001871)
    br(0x28f7,0x194b,'brfalse',0x19da);br(0x28f7,0x1cfc,'brfalse.s',0x1d03)

    # Parent ball pan, loop boundary, native identities (pause != stop).
    site(0x1467,1,'ldfld',0x4000bd2);site(0x1467,6,'ldflda',0x4002cf6)
    op(0x1467,0x15,'div');op(0x1467,0x1b,'sub');op(0x1467,0x21,'mul')
    const(0x146b,0x14,'ldc.i4.s',14);br(0x146b,0x16,'blt.s',0x26)
    site(0x146b,0x33,'ldfld',0x40036b0);site(0x146b,0x49,'call',0x6003b33)
    const(0x146b,0x3e,'ldc.r4',0.4000000059604645)
    site(0x3b33,0x11,'call',0x6003b20)
    for o in [0x54,0x95,0xba]:site(0x3b33,o,'callvirt',0x6002fee)
    for m in [0x3ba1,0x3ba2]:site(m,0x1d,'call',0x600313e)
    for o in [0x11,0x28]:site(0x3b9c,o,'call',0x6003146)
    site(0x146c,0,'call',0x6003b53);site(0x3b53,0x25,'callvirt',0x6002fee)
    for o in [5,0x10]:site(0x3ba8,o,'call',0x600313d)
    assert d['native_imports']['0x600313d']['entry']=='BASS_ChannelPause'
    assert d['native_imports']['0x6003151']['entry']=='BASS_ChannelStop'
    for o in [0x9e,0xe2]:site(0x3b20,o,'call',0x6003b36)
    for o in [0xa4,0xe8]:site(0x3b20,o,'call',0x6003197)
    # Prepared slider families and dynamic cache/plane0 reuse.
    for o,n in [(0xc6,2064529152),(0xe5,2064529198),(0x104,2064529212)]:const(0x3b19,o,'ldc.i4',n)
    const(0x3b19,0xd0,'ldc.i4.0');const(0x3b19,0xef,'ldc.i4.1');const(0x3b19,0x10e,'ldc.i4.1')
    site(0x3b36,0x82,'ldsfld',0x4002375);site(0x3b36,0x90,'ldsfld',0x4002376)
    br(0x3b36,6,'beq.s',0x34)
    site(0x3b36,0x1cc,'callvirt',0xa001613)
    site(0x3b36,0x201,'ldsfld',0x4002375);const(0x3b36,0x208,'ldc.i4.0')
    expected={2064529152:'slidertick',2064529198:'sliderslide',2064529212:'sliderwhistle',
        2064528831:'{0}{1}-slidertick{2}',2064528794:'slide',2064528790:'{0}{1}-slider{2}{3}',
        2064529283:'slider',2064459678:'tick',2064529308:'$slider$',2064528980:'{0}{1}-hit{2}{3}'}
    for n,value in expected.items(): assert d['strings'][str(n)]['text']==value
    resources=d['embedded_slider_resources'];assert len(resources)==2
    assert all(v is None for v in resources[0]['keys'].values())
    assert len(resources[1]['keys'])==9 and all(v['type']=='System.ByteArray' for v in resources[1]['keys'].values())
    assert [x['id'] for x in d['reference_cases']]==[f'S{i:02}' for i in range(1,49)]
    assert all(x['status']=='RECONSTRUCTED STATIC PREDICTION' for x in d['reference_cases'])
    assert all(x['evidence'] for x in d['reference_cases'])
    root=Path(__file__).resolve().parents[1]
    for source,h in d['authority_inputs'].items():
        assert hashlib.sha256((root/'evidence'/source).read_bytes()).hexdigest()==h
    for source,h in d['collector_sources'].items():
        assert hashlib.sha256((root/'tools'/source).read_bytes()).hexdigest()==h
    for m,h in {0x1449:'b65870f15ba4c334e9d9287d521ef3554224dfe2382a06f020c833c0155dc84f',
        0x1469:'8e5a7db9d5b61f87363b920b0637ffa0036829ed76d2755051bd94bbac22a3a4',
        0x146a:'262ff38645dc2570e379b3c40b42eb947a287b6ffa43d4a5adf5f9c4a6d99d66',
        0x48e0:'d428bc5c99ad965076ad2905413bbe06c5ca8c8ab99ba20cca4e93ad90d19602'}.items():
        assert r(m)['body_sha256']==h
    shared=0
    for path in sorted(Path(__file__).resolve().parents[1].glob('evidence/*/*.json')):
        if path.parent.name=='39':continue
        old=json.loads(path.read_text(encoding='utf-8'))
        for token,value in old.get('methods',{}).items():
            if token in d['methods'] and 'body_sha256' in value:
                assert value['body_sha256']==d['methods'][token]['body_sha256'],(path,token)
                shared+=1
    if archive is not None:
        assert hashlib.sha256(Path(archive).read_bytes()).hexdigest()==ARCHIVE
        with zipfile.ZipFile(archive) as z: raw=z.read('osu!.exe')
        assert hashlib.sha256(raw).hexdigest()==EXE
        import dnfile
        from dncil.cil.body.reader import read_method_body_from_bytes
        pe=dnfile.dnPE(data=raw);ts=pe.net.mdtables
        owners={m.row_index:rid for rid,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
        for token,value in d['methods'].items():
            rid=int(token,16)&0xffffff;m=ts.MethodDef.rows[rid-1]
            assert m.Signature.value.hex()==value['signature']
            assert hex(0x2000000+owners[rid])==value['owner']
            assert hex(m.Rva)==value['rva']
            assert (m.Flags.mdVirtual,m.Flags.mdNewSlot)==(value['virtual'],value['new_slot'])
            if m.Rva:
                b=read_method_body_from_bytes(pe.get_data(m.Rva))
                assert hashlib.sha256(pe.get_data(m.Rva,b.size)).hexdigest()==value['body_sha256']
        fieldowners={f.row_index:rid for rid,t in enumerate(ts.TypeDef,1) for f in t.FieldList}
        for token,value in d['fields'].items():
            rid=int(token,16)&0xffffff;field=ts.Field.rows[rid-1]
            assert field.Signature.value.hex()==value['signature']
            assert hex(0x2000000+fieldowners[rid])==value['owner']
        pe.close()
        # Independent metadata/key/byte-fingerprint check; never export bytes.
        with zipfile.ZipFile(archive) as z:
            for record in resources:
                dll=z.read(record['assembly'])
                assert hashlib.sha256(dll).hexdigest()==record['assembly_sha256']
                pe=dnfile.dnPE(data=dll)
                store=next(x for x in pe.net.resources if str(x.name)==record['resource_set'])
                entries={str(x.name):x.value for x in store.data.entries}
                for key,value in record['keys'].items():
                    if value is None:assert key not in entries
                    else:
                        assert len(entries[key])==value['bytes']
                        assert hashlib.sha256(entries[key]).hexdigest()==value['sha256']
                pe.close()
    print(f'PASS E039:92 methods;19659 bodies;{shared} historical body hashes;'
          'head/audio-index/edge/tick/loop/resource relationships;48 conditional cases;no runtime/audio execution')

if __name__=='__main__':
    check(json.loads(Path(sys.argv[1]).read_text(encoding='utf-8')),sys.argv[2] if len(sys.argv)>2 else None)
