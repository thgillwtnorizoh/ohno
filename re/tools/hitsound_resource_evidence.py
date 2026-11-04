#!/usr/bin/env python3
"""E038 pinned static resource/string evidence. Never invokes original managed/native code.

The numeric folder is limited to straight-line arithmetic and the acyclic pure
metadata-token seed helpers. It is not a CLR, game loader or audio implementation.
Requires the unchanged E004/E037 collectors and their pinned Python libraries.
"""
import hashlib
import json
import pathlib
import struct
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import circle_hitsound_evidence as previous

METHODS = [0x1884,0x24bb,0x24ba,0x24b9,0x24bd,0x48df,0x45eb,0x45ec,
 0x44a2,0x3b08,0x3b13,0x3b14,0x3b15,0x3b16,0x3b17,0x3b18,0x3b19,
 0x3b1a,0x3b1b,0x3b1c,0x3b1d,0x3b1e,0x3b1f,0x3b31,0x3b32,0x3b36,
 0x3b40,0x3195,0x3196,0x31a2,0x31a4,0x31a5,0x31a7,0x1bc1,0x28f5,0x28f7,0x2914,
 0x1bb7,0x1bb8,0x1bbd,0x1bee,0x1c01,0x1c05,0x1c06,0x1c0d,0x1da7,0x1dac,
 0x1dad,0x1dbf,0x1dc0,0x0efd,0x4b21,0x4b35,0x23b8,0x0759,0x0764,0x0767,
 0x0768,0x177c,0x1779,0x3df2,0x534e,0x5351,0x5353,0x531d,0x531e,0x5302,
 0x5304,0x5316,0x49af,0x49b0,0x49b1,0x49b2,0x49b3,0x49b4,0x49b5,0x49b6,
 0x49b7,0x49b8,0x49b9,0x49bb,0x49bc,0x49bd,0x0bbe,0x0bbf,0x0bc0,0x0bc1,
 0x0bc2,0x0bc3,0x0bc4,0x0bc5,0x0bc6,0x0bc8,0x0bc9,0x0bca,0x0bcb,0x0bcd,
 0x0bcf,0x0bd1,0x0bd3,0x0bd5,0x3414,0x341a,0x341c,0x341e,0x3421,0x3425,
 0x342a,0x2237,0x28f6,0x3b5e,0x4b2b]
FIELDS = [0x234a,0x234e,0x234f,0x2350,0x2360,0x2361,0x2374,0x2379,
 0x36b0,0x36a7,0x03be,0x236c,0x236e,0x19ff,0x0fd6,0x0fee,0x0fc0,
 0x2e6c,0x2e6d,0x2e6e,0x2e6f,0x2e70,0x2e71,0x2e72,0x2e73,0x2e74]
CALLS = [0x3b14,0x3b15,0x3b16,0x3b18,0x3b19,0x3b1b,0x3b1d,0x3b1e,
 0x5353,0x531e,0x1c06,0x1c0d]
STRING_METHODS = {0x3b14,0x3b15,0x3b19,0x3b36,0x24b9,0x4b21,0x4b35,0x1bee}
EXTRA_STRINGS = {2064528048,2064528026,2064529910,2064456412,2064536391}

def sha(b): return hashlib.sha256(b).hexdigest()
def i32(x): return (x + 2**31) % 2**32 - 2**31

class StaticStrings:
    """Own mathematical translation for this pinned same-assembly caller profile."""
    def __init__(self, pe):
        self.pe=pe;self.ts=pe.net.mdtables;self.bodies={}
        self.locals={0:-1709971236,1:i32(-1709971236-218750486)}
        words=[self.fold(0x49b1,lo,hi,self.locals)[1][3] for lo,hi in
          [(0x6d,0x76),(0x8b,0x94),(0xa9,0xb2),(0xc7,0xd0),(0xe5,0xee),(0x103,0x10c)]]
        self.resource_name=''.join(chr((words[j]>>shift)&65535) for j,shifts in
          enumerate([(0,16),(16,0),(0,16),(0,16),(16,0),(0,)]) for shift in shifts)
        self.data=next(r.data for r in pe.net.resources if str(r.name)==self.resource_name)
        self.provenance=self.derive()
        self.key=self.provenance['final_key']
        self.constants={}
        ranges=[('position1',0x480,0x488),('position2',0x490,0x498),
          ('length',0x53e,0x552),('size_mask',0x5c6,0x5ce),('byte_flag',0x5f7,0x5ff),
          ('compression_flag',0x608,0x610),('no_cache_flag',0x619,0x621),('redirect',0x559,0x561)]
        for name,lo,hi in ranges:
            self.constants[name]=self.fold(0x49b1,lo,hi,self.locals,stack=[0,0])[0][-1]
        self.provenance['constants']=self.constants
        self.provenance['resource']={'name':self.resource_name,'bytes':len(self.data),'sha256':sha(self.data)}
        assert (struct.unpack_from('<h',self.data)[0] ^ self.fold(0x49b1,0x268,0x27c,self.locals)[0][-1])==0
        self.key_length=struct.unpack_from('<h',self.data,2)[0]^self.fold(0x49b1,0x28f,0x2a3,self.locals)[0][-1]
        assert self.key_length==3
        self.provenance['global_key_length']=self.key_length

    def instructions(self,rid):
        if rid not in self.bodies:
            m=self.ts.MethodDef.rows[rid-1]
            b=read_method_body_from_bytes(self.pe.get_data(m.Rva))
            self.bodies[rid]=[(i.offset-b.header_size,i.opcode.name,i.operand) for i in b.instructions]
        return self.bodies[rid]

    def fold(self,rid,lo=0,hi=999999,loc=None,args=None,stack=None):
        """Constant arithmetic only; reject branches, fields, I/O and unknown calls."""
        loc={} if loc is None else dict(loc);args=[] if args is None else args
        s=[] if stack is None else list(stack)
        for off,op,a in self.instructions(rid):
            if not lo<=off<=hi:continue
            if op.startswith('ldc.i4'):
                s.append(a if op in ('ldc.i4','ldc.i4.s') else (-1 if op.endswith('m1') else int(op[-1])))
            elif op.startswith(('ldarg','ldloc','stloc')):
                n=a.index if a is not None else int(op[-1]);v=args if op.startswith('ldarg') else loc
                if op.startswith('stloc'):v[n]=s.pop()
                else:s.append(v[n])
            elif op in ('add','sub','mul','xor','or','and','shl','shr','shr.un','rem'):
                y=s.pop();x=s.pop()
                v={'add':lambda:x+y,'sub':lambda:x-y,'mul':lambda:x*y,'xor':lambda:x^y,
                  'or':lambda:x|y,'and':lambda:x&y,'shl':lambda:x<<(y&31),
                  'shr':lambda:x>>(y&31),'shr.un':lambda:(x&0xffffffff)>>(y&31),
                  'rem':lambda:x-int(x/y)*y}[op]()
                s.append(i32(v))
            elif op in ('not','neg','conv.u1','conv.u2','conv.i2'):
                x=s.pop();s.append(i32({'not':lambda:~x,'neg':lambda:-x,
                  'conv.u1':lambda:x&255,'conv.u2':lambda:x&65535,
                  'conv.i2':lambda:(x+32768)%65536-32768}[op]()))
            elif op=='dup':s.append(s[-1])
            elif op=='pop':s.pop()
            elif op=='ldtoken':s.append(a.value)
            elif op=='call' and a.value==0xa0000ff:pass # Type.GetTypeFromHandle, metadata identity only
            elif op in ('call','callvirt') and a.value==0x6000bc3:pass # MetadataToken
            elif op=='call' and 0x6000bc4<=a.value<=0x6000bd5:
                r=a.value&0xffffff;n=2 if r in (0xbc4,0xbc5,0xbc6) else 0
                aa=s[-n:] if n else [];s=s[:-n] if n else s
                s.append(self.fold(r,args=aa)[0][-1])
            elif op=='ret':break
            else:raise ValueError(('not a pure numeric region',hex(rid),hex(off),op))
        return s,loc

    def derive(self):
        l=self.locals
        ll={2:-1206307296,3:i32(-1206307296-1939401338),1:0}
        for lo,hi in [(0x4d,0x74),(0x89,0x9f),(0x142,0x158)]:
            _,ll=self.fold(0x49af,lo,hi,ll)
        initial=ll[0];token_seed=self.fold(0xbd5)[0][-1];t=0x20001a7
        assembly=self.ts.Assembly.rows[0];assert not assembly.PublicKey.value
        identity=bytearray(str(assembly.Name).encode('utf-16le'))+bytes([
          (t>>16)&255,(token_seed>>24)&255,(t>>24)&255,(token_seed>>8)&255,
          t&255,(token_seed>>16)&255,(t>>8)&255,token_seed&255])
        h=0;mask=(1<<64)-1
        for byte in identity:
            h=(h+byte)&mask;h=(h+(h<<20))&mask;h^=h>>12
        h=(h+(h<<6))&mask;h^=h>>22;h=(h+(h<<30))&mask
        h^=(-5874924180012351533)&mask
        key=i32(initial^h)
        key=self.fold(0x49b1,0x2f1,0x2fb,l,stack=[key])[1][2]
        key=self.fold(0x49b1,0x2fc,0x307,l|{2:key})[1][2]
        seed=key;seq=[];prev=0;b=1;remaining=key
        # M3414 powers; M3425 recurrence; M341c yields only its first seven.
        for power in [1,2,4,8,16,32,5]:
            x=i32(b^key)
            if x&3==0:x=i32(x^-87050054)
            if x&15==0:x=i32(x^572754781)
            seq.append(x);remaining-=1
            prev,b=b,i32(i32(b+prev+remaining)^i32(1400855004+power))
        e=key;k=117
        for x in seq:e=i32(e^i32(x-k));k=i32(k-i32((3+e)>>8))
        rem=self.fold(0x49b1,0x376,0x38c,l|{0x3e:e})[1][0x3a]
        l2=l|{2:key,0x3a:rem}
        for lo,hi in [(0x38e,0x3c3),(0x3c5,0x3de)]:_,l2=self.fold(0x49b1,lo,hi,l2)
        f70=0
        for lo,hi in [(0x55,0x5e),(0x137,0x143),(0x18c,0x198),(0x1de,0x1e7),(0x23e,0x24c),(0x3ea,0x3fd)]:
            f70=self.fold(0x49b1,lo,hi,l|{7:0},stack=[f70])[0][-1]
        assert self.fold(0x49b1,0x408,0x41c,l,stack=[16])[0][-1]
        return {'profile':'same-assembly ordinary managed caller; nonnull declaring type other than RuntimeMethodHandle; F2e74=16',
          'assembly_name':str(assembly.Name),'public_key_present':False,'initial_F2e72':initial,
          'metadata_token_seed':token_seed,'identity_bytes_sha256':sha(identity),
          'identity_hash_u64':hex(h),'pre_generator_key':seed,'generator_values':seq,
          'generator_fold':e,'signed_remainder':rem,'final_key':l2[2],'F2e70':f70}

    def decode(self,identifier,depth=0):
        assert depth<8
        c=self.constants;k=self.key
        off=i32(identifier^c['position1']^k^c['position2']);pos=off
        assert 0<=pos<len(self.data)
        key=bytearray(self.data[pos:pos+self.key_length]);pos+=self.key_length
        for j in range(len(key)):key[j]^=(k>>(((j&3)<<3)&31))&255
        size=i32(struct.unpack_from('<I',self.data,pos)[0]^off^c['length']^k);pos+=4
        if size==c['redirect']:
            v=self.data[pos:pos+4];other=i32(v[2]|v[3]<<16|v[0]<<8|v[1]<<24)
            # M49b1:0571-059e derives identifier base from the same key.
            base=self.fold(0x49b1,0x571,0x57b,self.locals|{2:k})[0][-1]
            return {'identifier':identifier,'offset':off,'redirect':self.decode(i32(other^-base),depth+1)}
        n=size&c['size_mask'];cipher=self.data[pos:pos+n];assert len(cipher)==n
        data=bytearray(cipher);prev=((11+n)^(key[1]+7))&255
        state=(key[0]|key[2]<<8)+(prev<<3);word=0
        for j,byte in enumerate(data):
            if j%2==0:state=(state*214013+2531011)&0xffffffff;word=(state>>16)&65535
            random=word&255;word>>=8
            data[j]=(key[1]^byte^(3+prev)^random)&255;prev=byte
        if size&c['compression_flag']:
            count=data[2]|data[0]<<16|data[3]<<8|data[1]<<24
            result=bytearray(count);src=4;dest=0;bit=128;flags=0
            while dest<count:
                bit<<=1
                if bit==256:bit=1;flags=data[src];src+=1
                if flags&bit:
                    length=(data[src]>>2)+3;back=((data[src]<<8)|data[src+1])&1023;src+=2
                    start=dest-back
                    if start<0:break # original M49b5 returns, leaving remaining zeros
                    for _ in range(length):
                        if dest>=count:break
                        result[dest]=result[start];dest+=1;start+=1
                else:result[dest]=data[src];dest+=1;src+=1
            data=result
        text=''.join(map(chr,data)) if size&c['byte_flag'] else data.decode('utf-16le',errors='replace')
        return {'identifier':identifier,'offset':off,'encoded_size':size,'cipher_bytes':n,
          'cipher_sha256':sha(cipher),'decoded_key_sha256':sha(key),'text':text,
          'byte_characters':bool(size&c['byte_flag']),'compressed':bool(size&c['compression_flag']),
          'no_cache':bool(size&c['no_cache_flag'])}

def extract(path):
    assert sha(pathlib.Path(path).read_bytes())==previous.ARCHIVE
    with zipfile.ZipFile(path) as z:raw=z.read('osu!.exe');names=z.namelist()
    assert sha(raw)==previous.EXE # before any original instructions
    previous.METHODS=METHODS
    previous.FIELDS|={0x4000000+x for x in FIELDS}
    previous.CALLS|={0x6000000+x for x in CALLS}
    out=previous.extract(path);out['expedition']=38
    pe=dnfile.dnPE(data=raw);decoder=StaticStrings(pe)
    out['string_decoder']=decoder.provenance;out['strings']={};out['string_sites']=[]
    for m in METHODS:
        if not pe.net.mdtables.MethodDef.rows[m-1].Rva:continue
        ins=decoder.instructions(m)
        for j,(off,op,arg) in enumerate(ins):
            if op=='call' and getattr(arg,'value',0)==0x60049b0:
                _,prevop,n=ins[j-1]
                if prevop!='ldc.i4':continue
                if m not in STRING_METHODS and n not in EXTRA_STRINGS:continue
                out['strings'][str(n)]=decoder.decode(n)
                out['string_sites'].append([hex(0x6000000+m),off,n])
    for token,f in out['fields'].items():f['name']=str(pe.net.mdtables.Field.rows[int(token,16)-0x4000001].Name)
    out['archive_entries']=names
    out['embedded_standard_resources']=[]
    wanted={f'{bank}-hit{sound}' for bank in ['normal','soft','drum'] for sound in ['normal','whistle','finish','clap']}
    with zipfile.ZipFile(path) as z:
        for name in ['osu!ui.dll','osu!gameplay.dll']:
            dll=z.read(name);p=dnfile.dnPE(data=dll)
            for resource in p.net.resources:
                entries={str(x.name):x for x in resource.data.entries}
                out['embedded_standard_resources'].append({'assembly':name,'assembly_sha256':sha(dll),
                  'resource_set':str(resource.name),'resource_set_bytes':resource.size,
                  'entry_count':len(entries),'keys':{key:(
                    {'type':entries[key].type_name,'bytes':len(entries[key].value),'sha256':sha(entries[key].value)}
                    if key in entries and isinstance(entries[key].value,bytes) else None) for key in sorted(wanted)}})
            p.close()
    out['prepared_key_controls']=[{'plane':plane,'bank':bank,'row':row,
      'map_stage_admitted_by_plane':plane!=0,'map_cache_key':f'{bank}-hit{sound}'+('2' if plane==2 else ''),
      'skin_embedded_key':f'{bank}-hit{sound}'}
      for plane in [0,1,2] for bank in ['normal','soft','drum']
      for row,sound in enumerate(['normal','whistle','finish','clap'])]
    out['limits']=['CONFIRMED STATIC metadata/instructions and independent mathematical string decoding only',
      'Caller profile explicit; no original executable/CLR/native/audio method invoked',
      'No audio asset contents are exported; embedded entries have names/sizes/hashes only',
      'Provider archive internals, current skin/config/resources, native handles and audible output remain runtime inputs',
      'E037 remains authoritative for selection/layer order/volume/queue/playback; E020 all17 NOT RUN; 628 QA not rerun']
    pe.close();return out

if __name__=='__main__':
    result=json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2)+'\n'
    if len(sys.argv)>2:pathlib.Path(sys.argv[2]).write_text(result,encoding='utf-8',newline='\n')
    else:print(result,end='')
