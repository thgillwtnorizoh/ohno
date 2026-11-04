#!/usr/bin/env python3
"""E038 independent assertions and optional independent original-byte decoder.
No import of the extractor, no original CLR/native execution, no audio export.
Usage: python check_hitsound_resources.py evidence.json [osubackup.zip]
"""
import hashlib
import json
import pathlib
import struct
import sys
import zipfile

def i32(x):return (x+2**31)%2**32-2**31
def sha(x):return hashlib.sha256(x).hexdigest()

def independent_key():
    # Reduced equations from M0bc4/5/6 and acyclic token dependencies. These
    # deliberately do not use the extractor's CIL folder or intermediate values.
    def f(a,b):return i32(a^i32(b+1312710413))
    def g(a,b):return i32(i32(a-929752135)^i32(b-20085209))
    def h(a,b):return i32(a^i32(b-1717224570)^i32(a-b))
    a,b,c,d,e,f_token=[0x2000000+x for x in [0x1a9,0x1ab,0x1ac,0x1ad,0x1ae,0x1af]]
    bd1=h(d,f(f_token,g(e,h(b,f(c,a)))))
    bd3=f(b,d^g(e,h(a,bd1)))
    bcd=h(f(bd3^527758446,d),g(f_token^a,-1654432510))
    bcf=g(g(bcd,f(c,bd3)),a)
    bc8=f(a,h(g(c,f_token),h(b^1411708072,bcf)))
    token_seed=h(g(e,h(f_token,c)),bc8)
    assert token_seed==i32(0xc06b3b32)
    t=0x20001a7
    identity='osu!'.encode('utf-16le')+bytes([(t>>16)&255,(token_seed>>24)&255,
      (t>>24)&255,(token_seed>>8)&255,t&255,(token_seed>>16)&255,(t>>8)&255,token_seed&255])
    mask=(1<<64)-1;v=0
    for x in identity:
        v=(v+x)&mask;v=(v+(v<<20))&mask;v^=v>>12
    v=(v+(v<<6))&mask;v^=v>>22;v=(v+(v<<30))&mask;v^=(-5874924180012351533)&mask
    seed=i32(-1876543868^v^1095194678^412844976)
    assert seed==135435612 and v==0x385151ffc1e8785e
    prior=0;current=1;remaining=seed;values=[]
    for power in [1,2,4,8,16,32,5]:
        value=i32(current^seed)
        if value%4==0:value=i32(value^-87050054)
        if value%16==0:value=i32(value^572754781)
        values.append(value);remaining-=1
        prior,current=current,i32(i32(current+prior+remaining)^i32(1400855004+power))
    v=seed;adjust=117
    for value in values:v=i32(v^i32(value-adjust));adjust=i32(adjust-((v+3)>>8))
    product=i32(v*5293);rem=(abs(product)%27741677)*(-1 if product<0 else 1)
    result=i32((seed^(715058+306)^(-2013062011))+i32(i32(rem+753968978)^1730989952))
    assert result==-900258630
    return result

def check(d,archive=None):
    assert d['expedition']==38 and len(d['methods'])==112
    assert d['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
    assert d['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
    assert d['scan']=={'decoded':19659,'zero_rva':1739,'errors':[]}
    def r(m):return d['methods'][hex(0x6000000+m)]
    def site(m,o,op,t):assert [o,op,hex(t)] in r(m)['token_sites'],(hex(m),hex(o))
    def br(m,o,op,t):assert [o,op,[t]] in r(m)['branches'],(hex(m),hex(o))
    def const(m,o,op,v=None):assert [o,op,v] in r(m)['constants'],(hex(m),hex(o),v)
    # Preserve E037 boundary and exact independent selector/resource edges.
    site(0x1884,0x7f,'callvirt',0x60024bb);site(0x24bb,0x14,'callvirt',0x6001be6)
    site(0x24ba,0x6f,'call',0x6003b31);site(0x24ba,0x7e,'call',0x6003b32)
    site(0x24b9,0x5b,'call',0x6003b15);site(0x24b9,0x60,'stfld',0x4001534)
    # Standard preparation order differs from eventual playback order.
    for off,row in [(0x47,0),(0x66,2),(0x85,1),(0xa4,3)]:const(0x3b19,off,'ldc.i4.'+str(row))
    for off in [0x57,0x76,0x95,0xb4]:site(0x3b19,off,'call',0x6003b14)
    site(0x28f5,0x1,'callvirt',0x60028f6);site(0x28f6,5,'call',0x6003b1d)
    site(0x28f5,0x18e,'ldfld',0x4000661);site(0x28f5,0x1e4,'ldfld',0x4001539)
    site(0x28f5,0x225,'ldftn',0x6002914);site(0x2914,1,'call',0x6003b18)
    site(0x28f5,0x23b,'call',0x6003b19)
    for off,f in [(0xb,0x234e),(0x15,0x234f),(0x1f,0x2350)]:site(0x3b08,off,'stsfld',0x4000000+f)
    site(0x3b1c,0x15,'call',0x6003196);site(0x3b1c,0x35,'callvirt',0xa00161a)
    site(0x4b2b,0x11f,'call',0x6003b1e);site(0x4b2b,0x12c,'call',0x6003b19)
    # M3b14 map cache, extension loop, historical fallback, skin, embedded.
    const(0x3b14,0xcf,'ldc.i4.4');site(0x3b14,0xea,'ldsfld',0x4002360)
    const(0x3b14,0x200,'ldc.i4.2');const(0x3b14,0x2c4,'ldc.i4.1')
    const(0x3b14,0x138,'ldc.i4.0');br(0x3b14,0x1e7,'blt',0x140)
    site(0x3b14,0x17f,'callvirt',0x6001c06);site(0x3b14,0x1a5,'callvirt',0x6001c06)
    const(0x3b14,0x192,'ldc.i4.5');br(0x3b14,0x193,'blt.s',0x198)
    site(0x3b14,0x1f4,'callvirt',0xa0011c8);br(0x3b14,0x1fb,'beq.s',0x1ff)
    site(0x3b14,0x208,'call',0x6004b35);site(0x3b14,0x217,'call',0x600177c)
    site(0x3b14,0x270,'callvirt',0x6005351);site(0x3b14,0x287,'call',0xa0003ec)
    site(0x3b14,0x29e,'call',0x60031a4);site(0x3b14,0x2b8,'callvirt',0xa0011c8)
    const(0x31a4,2,'ldc.i4',-2147483648);site(0x31a4,0x11,'call',0x60031a7)
    assert d['native_imports']['0x60031a7']=={'module':'bass.dll','entry':'BASS_SampleLoad'}
    site(0x3b14,0x36e,'call',0xa00051c);site(0x3b14,0x382,'call',0xa00065b)
    site(0x3b14,0x374,'callvirt',0xa00051d);site(0x3b14,0x388,'callvirt',0xa00051d)
    br(0x3b14,0x3bf,'brtrue.s',0x3c4);const(0x3b14,0x3c1,'ldc.i4.m1')
    # Custom-name exact-exists versus extension probing, replacement, no zero normalization.
    site(0x3b15,0x1e,'callvirt',0xa00023d);br(0x3b15,0x24,'ble.s',0x38)
    site(0x3b15,0x2b,'callvirt',0xa0003b9);site(0x3b15,0x30,'callvirt',0xa00023e)
    site(0x3b15,0x88,'callvirt',0x6001c0d);br(0x3b15,0x8d,'brtrue.s',0x10d)
    site(0x3b15,0xd1,'callvirt',0x6001c06);site(0x3b15,0x113,'callvirt',0x6001c06)
    site(0x3b15,0x188,'call',0x6003196);site(0x3b15,0x196,'callvirt',0xa0011c8)
    site(0x1c05,0xf,'call',0xa0003ec);site(0x1c05,0x23,'call',0xa000ab1)
    site(0x1c05,0x4f,'callvirt',0x6001dac);site(0x1c06,0x3a,'callvirt',0xa0000a6)
    site(0x1bb8,0,'ldsfld',0x40019ff);site(0x1bb8,6,'ldfld',0x4000fd6)
    site(0x5351,0,'call',0x6004b21);site(0x5351,6,'ldfld',0x40036a7)
    assert d['fields']['0x40036b0']['name']=='LayeredHitSounds'
    const(0x534e,0x9a,'ldc.i4.1');site(0x534e,0x9b,'stfld',0x40036b0)
    site(0x5353,0x119,'ldflda',0x40036b0);site(0x5353,0x11e,'call',0x2b00020d)
    # Literal source identities for decoder, cache, encoding and LCG.
    site(0x49b0,8,'callvirt',0xa001913);site(0x49b0,0x13,'call',0x60049b1)
    const(0x49b1,0,'ldc.i4',-1709971236);const(0x49b1,7,'ldc.i4',-218750486)
    site(0x49b1,0x11e,'callvirt',0xa00107a);site(0x49b1,0x2e3,'call',0x6000bbf)
    const(0x49b1,0x66c,'ldc.i4',387884575);const(0x49b1,0x676,'ldc.i4',-216487127)
    site(0x49b1,0x740,'call',0x60049b5);site(0x49b1,0x781,'call',0xa0006ad)
    site(0x49b1,0x819,'call',0xa001915);site(0x49b1,0x828,'callvirt',0xa001916)
    p=d['string_decoder'];assert p['final_key']==independent_key()
    assert p['F2e70']==1607814 and p['global_key_length']==3 and p['initial_F2e72']==-1876543868
    expected={2064529391:'hitnormal',2064529407:'hitfinish',2064529359:'hitwhistle',
      2064529372:'hitclap',2064528980:'{0}{1}-hit{2}{3}',2064631985:'.wav',2064631948:'.mp3',
      2064631963:'.ogg',2064631958:'wav',2064632044:'mp3',2064632058:'ogg',2064615002:'-',
      2064593714:'.',2064456412:'LayeredHitSounds',2064529910:'SkinSamples',2064529892:'Default',
      2064528048:'VolumeUniversal',2064528026:'VolumeEffect',2064453750:'Skins',2064536391:'General'}
    for n,text in expected.items():assert d['strings'][str(n)]['text']==text
    # Literal extension-site lists independently assert the two distinct orders.
    for m,sites,ids in [(0x3b14,[0x149,0x15c,0x16a],[2064631985,2064631948,2064631963]),
      (0x3b15,[0x94,0xaa,0xbd],[2064631985,2064631963,2064631948])]:
        for off,n in zip(sites,ids):assert [hex(0x6000000+m),off,n] in d['string_sites']
    assert len(d['prepared_key_controls'])==36
    ui,game=d['embedded_standard_resources']
    assert ui['assembly']=='osu!ui.dll' and not any(ui['keys'].values())
    assert game['assembly']=='osu!gameplay.dll' and len(game['keys'])==12
    assert all(x and x['type']=='System.ByteArray' and x['bytes']>1024 for x in game['keys'].values())
    shared=0
    for path in sorted(pathlib.Path(__file__).resolve().parents[1].glob('evidence/*/*.json')):
        if path.parent.name=='38':continue
        try:old=json.loads(path.read_text(encoding='utf-8'))
        except (ValueError,UnicodeError):continue
        for token,method in old.get('methods',{}).items():
            if token in d['methods'] and 'body_sha256' in method:
                assert method['body_sha256']==d['methods'][token]['body_sha256'],(path,token)
                shared+=1
    decoded=0
    if archive:
        assert sha(pathlib.Path(archive).read_bytes())==d['archive_sha256']
        import dnfile
        with zipfile.ZipFile(archive) as z:raw=z.read('osu!.exe')
        assert sha(raw)==d['exe_sha256']
        pe=dnfile.dnPE(data=raw)
        table=next(r.data for r in pe.net.resources if str(r.name)==p['resource']['name'])
        assert sha(table)==p['resource']['sha256']
        key=independent_key()
        for n,record in d['strings'].items():
            position=i32(int(n)^(-351960377)^key^1516081623)
            assert position==record['offset']
            keys=bytes(table[position+j]^((key>>(8*j))&255) for j in range(3))
            encoded=i32(struct.unpack_from('<I',table,position+3)[0]^position^(-973073496)^key)
            assert encoded&0xf0000000==0x40000000 # all selected records are uncompressed byte characters
            length=encoded&0xfffffff;cipher=table[position+7:position+7+length]
            assert sha(cipher)==record['cipher_sha256']
            prev=((length+11)^(keys[1]+7))&255;state=(keys[0]+(keys[2]<<8))+(prev<<3)
            output=[];random=0
            for j,c in enumerate(cipher):
                if j%2==0:state=(214013*state+2531011)&0xffffffff;random=(state>>16)&65535
                output.append(chr((c^keys[1]^(prev+3)^(random&255))&255));random>>=8;prev=c
            assert ''.join(output)==record['text'];decoded+=1
        pe.close()
        # Independently re-read only the twelve named byte-array entries. Never
        # write their bytes or invoke a decoder/player for their audio contents.
        with zipfile.ZipFile(archive) as z:
            for store in d['embedded_standard_resources']:
                raw=z.read(store['assembly']);assert sha(raw)==store['assembly_sha256']
                pe=dnfile.dnPE(data=raw)
                resource=next(r for r in pe.net.resources if str(r.name)==store['resource_set'])
                entries={str(x.name):x for x in resource.data.entries}
                assert len(entries)==store['entry_count'] and resource.size==store['resource_set_bytes']
                for name,record in store['keys'].items():
                    if record is None:assert name not in entries
                    else:
                        entry=entries[name]
                        assert entry.type_name==record['type'] and len(entry.value)==record['bytes']
                        assert sha(entry.value)==record['sha256']
                pe.close()
    print(f'PASS E038: {len(d["methods"])} methods; {len(d["strings"])} strings; {decoded} independently decoded from archive; 12 embedded keys; {shared} historical body hashes; no audio execution')

if __name__=='__main__':check(json.loads(pathlib.Path(sys.argv[1]).read_text(encoding='utf-8')),sys.argv[2] if len(sys.argv)>2 else None)
