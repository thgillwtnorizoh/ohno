#!/usr/bin/env python3
"""Independent E037 literal-site assertions. Does not simulate sound or play audio."""
import json
import pathlib
import sys

def check(d):
    assert d['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
    assert d['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
    assert d['scan']=={'decoded':19659,'zero_rva':1739,'errors':[]}
    def r(m):return d['methods'][hex(0x6000000+m)]
    def site(m,o,op,t):assert [o,op,hex(t)] in r(m)['token_sites'],(hex(m),hex(o),op,hex(t))
    def br(m,o,op,t):assert [o,op,[t]] in r(m)['branches'],(hex(m),hex(o))
    def const(m,o,op,v=None):assert [o,op,v] in r(m)['constants'],(hex(m),hex(o))
    def operation(m,o,op):assert [o,op] in r(m)['operations'],(hex(m),hex(o))
    site(0x1884,0x76,'ldfld',0x400152a);br(0x1884,0x7c,'ble.s',0x84)
    site(0x1884,0x7f,'callvirt',0x60024bb);site(0x1884,0x86,'ldfld',0x400152a)
    site(0x1884,0x8e,'callvirt',0x60024b4);site(0x1884,0x94,'ldfld',0x400152a)
    assert (r(0x24bb)['owner'],r(0x24bb)['signature'])==('0x20004c3','200001')
    for m,o in [(0x24bb,0x14),(0x3202,0x12)]:site(m,o,'callvirt',0x6001be6)
    site(0x24bb,0xc,'ldfld',0x4002523);const(0x24bb,0x11,'ldc.i4.5');operation(0x24bb,0x12,'add')
    br(0x24bb,0x1b,'brtrue.s',0x1e);site(0x24bb,0x32,'call',0x60024ba)
    site(0x3202,0xc,'ldfld',0x4002524);site(0x3202,0x2f,'call',0x60024ba)
    assert not any(t=='0x6001be6' for o,op,t in r(0x48df)['token_sites'])
    site(0x48df,0xd,'newobj',0x60045eb);site(0x426e,0xf,'callvirt',0x6001be6)
    for off,f in [(0x15,0x1537),(0x1d,0x1538),(0x25,0x1539),(0x2d,0x1536)]:site(0x48df,off,'stfld',0x4000000+f)
    site(0x48df,0x35,'callvirt',0x60024b9)
    site(0x249b,0x18,'stfld',0x4001534);const(0x249b,0x17,'ldc.i4.m1')
    site(0x1be6,0x2b,'ldfld',0x4000662);br(0x1be6,0x31,'bgt.un.s',0x35)
    br(0x1be6,0x48,'brtrue.s',0x57);site(0x1be6,0x51,'callvirt',0xa0004b4)
    for o,f in [(7,0x663),(0x1e,0x661),(0x2d,0x665)]:site(0x24ba,o,'ldfld',0x4000000+f)
    site(0x24ba,0x47,'call',0x60044a2);site(0x24ba,0x4e,'call',0x60028a1)
    br(0x24ba,0x5a,'beq.s',0x76);site(0x24ba,0x6f,'call',0x6003b31);site(0x24ba,0x7e,'call',0x6003b32)
    site(0x24b9,0x1e,'stfld',0x4001535);site(0x24b9,0x5b,'call',0x6003b15)
    site(0x24b9,0x60,'stfld',0x4001534);site(0x24b9,0x68,'stfld',0x4001534)
    site(0x0b73,0x2c,'stfld',0x4000663);site(0x0b73,0x34,'stfld',0x4000661)
    site(0x0b73,0x44,'stfld',0x4000665);site(0x28ff,0xd6,'newobj',0x6000b73)
    # Layer policy and ordered Normal/Finish/Whistle/Clap requests.
    br(0x44a2,1,'brfalse.s',0x2a);site(0x44a2,0xf,'ldfld',0x40036b0)
    const(0x44a2,0x27,'ldc.i4.3');br(0x44a2,0x28,'beq.s',0x2f);operation(0x44a2,0x2c,'or')
    for o,v in [(0x35,'ldc.i4.1'),(0x70,'ldc.i4.4'),(0x9a,'ldc.i4.2'),(0xd0,'ldc.i4.8')]:const(0x3b32,o,v)
    for o,t in [(0x4b,0x3b36),(0x64,0x3b31),(0x81,0x3b36),(0x8e,0x3b31),
                (0xab,0x3b36),(0xc4,0x3b31),(0xe1,0x3b36),(0xfa,0x3b31)]:site(0x3b32,o,'call',0x6000000+t)
    const(0x3b32,0x52,'ldc.r8',.8);const(0x3b32,0xb2,'ldc.r8',.85);const(0x3b32,0xe8,'ldc.r8',.85)
    site(0x3b36,0x56,'call',0xa0000fc);br(0x3b36,0x5e,'bgt.s',0xa0)
    assert [0xb,'switch',[0x26,0x2a,0x34,0x2e]] in r(0x3b36)['branches']
    assert [0x35,'switch',[0x4e,0x4a,0x4e,0x52]] in r(0x3b36)['branches']
    site(0x3b36,0x1cc,'callvirt',0xa001613);const(0x3b36,0x1fa,'ldc.i4.0')
    site(0x3b31,0x51,'newobj',0x60013fb);site(0x3b31,0x66,'callvirt',0x60013fd)
    site(0x3b31,0x6d,'callvirt',0x60013ff);site(0x3b31,0x75,'callvirt',0x6001403)
    site(0x3b31,0x9a,'callvirt',0xa001622);site(0x3b31,0xb1,'ldftn',0x6003bb6)
    const(0x3b31,0xbc,'ldc.i4.2');site(0x3b31,0xbd,'callvirt',0x6002fee)
    site(0x3b21,0xbd,'newobj',0x60030b1);site(0x3b21,0xc2,'stsfld',0x400234b)
    site(0x2fee,0x2f,'callvirt',0x6002fdd);site(0x30bf,0x19,'callvirt',0x60033f2)
    site(0x30c6,0x165,'call',0x60030c7);site(0x30c7,0x23,'callvirt',0x60036b0)
    site(0x36b0,0x18,'call',0x60036b3);site(0x36b3,0x41,'callvirt',0x6000e88)
    site(0x2ffd,6,'callvirt',0xa000d4a)
    site(0x3bb6,8,'callvirt',0x600140b);site(0x140b,0x2f,'call',0x600313e)
    br(0x140b,0xf,'brtrue.s',0x19);site(0x1406,0x10,'call',0x6003197)
    for o in [0x99,0xac,0xc6]:site(0x1404,o,'call',0x6003146)
    operation(0x24bd,0x10,'div');operation(0x24bd,0x16,'sub');operation(0x24bd,0x1c,'mul')
    expected={0x313e:'BASS_ChannelPlay',0x3146:'BASS_ChannelSetAttribute',0x3197:'BASS_SampleGetChannel',
              0x3117:'BASS_ChannelGetAttribute',0x3195:'BASS_SampleCreate',0x31a5:'BASS_SampleLoad'}
    for m,name in expected.items():assert d['native_imports'][hex(0x6000000+m)]=={'module':'bass.dll','entry':name}
    assert d['sample_enums']['0x2000987']['values']=={'None':0,'Normal':1,'Whistle':2,'Finish':4,'Clap':8}
    assert d['sample_enums']['0x2000b0f']['values']=={'All':-1,'None':0,'Normal':1,'Soft':2,'Drum':3}
    assert d['sample_enums']['0x200035f']['values']=={'Default':0,'Custom1':1,'Custom2':2}
    # Ordinary circle ancestry contains no replacement sound/pan slot.
    ordinary={'0x200094c','0x200035a','0x2000828'}
    for slot in ['0x60024bb','0x60024bd']:
        assert not any(x['owner'] in ordinary for x in d['slots'][slot])
    shared=0
    for path in sorted(pathlib.Path(__file__).resolve().parents[1].glob('evidence/*/*.json')):
        if path.parent.name=='37':continue
        try:old=json.loads(path.read_text(encoding='utf-8'))
        except (ValueError,UnicodeError):continue
        for token,v in old.get('methods',{}).items():
            if token in d['methods'] and 'body_sha256' in v:
                assert v['body_sha256']==d['methods'][token]['body_sha256'],(path,token)
                shared+=1
    print(f"PASS E037: {len(d['methods'])} methods;19659 bodies;{shared} historical body hashes; trigger/selection/queue/native relationships; no audio execution")

if __name__=='__main__':check(json.loads(pathlib.Path(sys.argv[1]).read_text(encoding='utf-8')))
