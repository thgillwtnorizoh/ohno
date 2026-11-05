#!/usr/bin/env python3
"""Independent E041 literal relationships and optional pinned-byte audit.

Does not import the E041 collector or invoke original CLR/native/audio methods.
"""
import hashlib,json,sys,zipfile,struct
from pathlib import Path
ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
def sha(b):return hashlib.sha256(b).hexdigest()
def check(d,archive=None):
 assert d['expedition']==41 and (d['archive_sha256'],d['exe_sha256'])==(ARCHIVE,EXE)
 assert d['scan']=={'decoded':19659,'zero_rva':1739,'errors':[]}
 def m(n):return d['methods'][hex(0x6000000+n)]
 def t(n,o,op,token):assert [o,op,hex(token)] in m(n)['token_sites'],(hex(n),hex(o),op,hex(token))
 def f(n,o,op,r):t(n,o,op,0x4000000+r)
 def call(n,o,r):t(n,o,'call',0x6000000+r)
 def c(n,o,op,v=None):assert [o,op,v] in m(n)['constants'],(hex(n),hex(o),op,v)
 def br(n,o,op,dest):assert [o,op,[dest]] in m(n)['branches'],(hex(n),hex(o),op,hex(dest))
 def op(n,o,operation):assert [o,operation] in m(n)['integer_sites'],(hex(n),hex(o),operation)
 # Complete direct channel assignments/accesses, not a guessed per-object player.
 inv=d['channel_access_inventory'];assert len(inv)==12
 assert [x for x in inv if x[2].startswith('st')]==[['0x6003b1f',0xd4,'stsfld','0x4002356']]
 assert d['fields']['0x4002356']['owner']=='0x20007b9' and d['fields']['0x4002356']['signature']=='0608' and d['fields']['0x4002356']['static']
 assert not d['fields']['0x4002356']['has_default'] and not d['fields']['0x4002356']['has_rva']
 assert d['fields']['0x40036af']['name']=='SpinnerFrequencyModulate'
 assert any(x['method']=='0x6003202' and not x['new_slot'] for x in d['slots']['0x60024bb'])
 assert any(x['method']=='0x60005fc' and not x['new_slot'] for x in d['slots']['0x60024c4'])
 assert d['types']['0x20000d8']['extends']=='0x200067f'
 # One increment, frequency gate/fresh operands, bonus selection then late cache.
 call(0x5f6,0x2d5,0x5eb);br(0x5f6,0x2e0,'bne.un.s',0x2e4)
 f(0x5f6,0x2ed,'stfld',0x2a2);f(0x5f6,0x2f7,'ldfld',0x36af);br(0x5f6,0x2fc,'brfalse.s',0x312)
 f(0x5f6,0x2ff,'ldfld',0x2a2);op(0x5f6,0x304,'conv.r4');f(0x5f6,0x306,'ldfld',0x2a0);op(0x5f6,0x30b,'conv.r4');op(0x5f6,0x30c,'div');call(0x5f6,0x30d,0x3b28)
 br(0x5f6,0x320,'ble',0x496);op(0x5f6,0x335,'rem');br(0x5f6,0x336,'brtrue',0x496)
 c(0x5f6,0x358,'ldc.i4',16384);c(0x5f6,0x35e,'ldc.i4',4194304);br(0x5f6,0x378,'brtrue.s',0x390)
 c(0x5f6,0x37a,'ldc.i4',2064529271);call(0x5f6,0x384,0x3b42);c(0x5f6,0x389,'ldc.i4.7');call(0x5f6,0x38a,0x3b2b)
 call(0x5f6,0x4c2,0x5eb);f(0x5f6,0x4c7,'stfld',0x29e)
 # Signed native length threshold; exact conversion/add/Min order, no finite repair.
 f(0x3b28,0,'ldsfld',0x2356);call(0x3b28,5,0x3121);c(0x3b28,0xa,'ldc.i4',8192);op(0x3b28,0xf,'conv.i8');br(0x3b28,0x10,'ble.s',0x37)
 f(0x3b28,0x12,'ldsfld',0x2356);c(0x3b28,0x17,'ldc.i4.1');c(0x3b28,0x18,'ldc.i4',100000);c(0x3b28,0x1d,'ldc.i4',20000);c(0x3b28,0x22,'ldc.r4',40000.0)
 for offset,operation in [(0x28,'mul'),(0x29,'conv.i4'),(0x2a,'add'),(0x30,'conv.r4')]:op(0x3b28,offset,operation)
 call(0x3b28,0x31,0x3146);c(0x3121,1,'ldc.i4.0');call(0x3121,2,0x3122)
 assert not any(tok in ['0x600313a','0x6003b31'] for _,_,tok in m(0x3b28)['token_sites'])
 # Progress update is a separate loop producer before accepted P store.
 br(0x5f5,0x14e,'bge',0x39a);br(0x5f5,0x15e,'ble',0x39a);br(0x5f5,0x173,'ble',0x39a)
 f(0x5f5,0x353,'ldfld',0x2af);br(0x5f5,0x361,'beq.s',0x36b);call(0x5f5,0x364,0x5fb);t(0x5f5,0x36c,'callvirt',0x60024c4);f(0x5f5,0x395,'stfld',0x2b4)
 f(0x5fb,5,'ldfld',0x36af);br(0x5fb,0xa,'brtrue.s',0x17);f(0x5fb,0x18,'ldfld',0x2a2);f(0x5fb,0x1f,'ldfld',0x2a0);op(0x5fb,0x25,'div');call(0x5fb,0x2b,0x3b34)
 br(0x3b34,7,'brfalse.s',0x15);call(0x3b34,0x10,0x3b28);t(0x3b34,0x28,'ldftn',0x6003ba3);t(0x3b34,0x3a,'callvirt',0x6002fee);c(0x3b34,0x39,'ldc.i4.2')
 f(0x3ba3,0,'ldsfld',0x2356);call(0x3ba3,6,0x3b41);call(0x3ba3,0x11,0x3146);f(0x3ba3,0x17,'ldsfld',0x2356);c(0x3ba3,0x1c,'ldc.i4.0');call(0x3ba3,0x1d,0x313e)
 assert not any(tok=='0x600313a' for _,_,tok in m(0x3b34)['token_sites'])
 # Exact replacement, loop/unprefixed/provider operands. Zero/-1 not normalized here.
 br(0x3b1f,0x15,'beq.s',0x27);f(0x3b1f,0x2f,'stsfld',0x2361);f(0x3b1f,0x39,'stsfld',0x2360)
 f(0x3b1f,0x9d,'ldsfld',0x2356);call(0x3b1f,0xaa,0x313a);call(0x3b1f,0xb6,0x3151)
 c(0x3b1f,0xbc,'ldc.i4',2064529249);c(0x3b1f,0xc6,'ldc.i4.1');c(0x3b1f,0xc7,'ldc.i4.1');c(0x3b1f,0xc8,'ldc.i4.7');call(0x3b1f,0xc9,0x3b14);c(0x3b1f,0xce,'ldc.i4.0');call(0x3b1f,0xcf,0x3197);f(0x3b1f,0xd4,'stsfld',0x2356)
 call(0x3b1f,0xfe,0x3146);call(0x3b1f,0x10b,0x3146)
 c(0x3b19,0x15a,'ldc.i4',2064529249);c(0x3b19,0x164,'ldc.i4.1');c(0x3b19,0x165,'ldc.i4.1');c(0x3b19,0x16d,'ldc.i4',2064529271);c(0x3b19,0x177,'ldc.i4.0')
 # Bonus has its own player creator, not M3b31 or global channel manipulation.
 c(0x3b2b,8,'ldc.i4.0');c(0x3b2b,9,'ldc.i4.1');call(0x3b2b,0xb,0x3b14);t(0x3b2b,0x10,'newobj',0x60013fb);t(0x3b2b,0x1e,'callvirt',0x60013fd);t(0x3b2b,0x5a,'ldftn',0x6003b87);t(0x3b2b,0x66,'callvirt',0x6002fee)
 c(0x3b87,6,'ldc.i4.1');c(0x3b87,7,'ldc.i4.0');t(0x3b87,8,'callvirt',0x600140b)
 c(0x1407,0xe,'ldc.i4.m1');br(0x140b,0xf,'brtrue.s',0x19);call(0x140b,0x2f,0x313e)
 # Cleanup before evaluated/grade and exact spinner end-time sound lookup.
 t(0x5fa,1,'callvirt',0x60024c4);f(0x5fa,0x20,'stfld',0x1527)
 for offset in [0x42,0x80,0xa0]:t(0x5fa,offset,'callvirt',0x60024bb)
 call(0x5fc,0,0x3b54);t(0x3b54,0x13,'ldftn',0x6003ba9);t(0x3b54,0x25,'callvirt',0x6002fee);f(0x3ba9,0,'ldsfld',0x2356);call(0x3ba9,5,0x313d)
 f(0x3202,0xc,'ldfld',0x2524);t(0x3202,0x12,'callvirt',0x6001be6);br(0x3202,0x19,'brfalse.s',0x34);call(0x3202,0x2f,0x24ba)
 assert not any(x[0]<0x17 for x in m(0x3202)['constants'])
 for meth,offset in [(0x28c5,0x50),(0x2262,0x1da),(0xacb,0x3c)]:t(meth,offset,'callvirt',0x60024c4)
 f(0x3b11,0x33,'ldsfld',0x2356);call(0x3b11,0x3a,0x3146);assert [0x39,'ldloc.0',None] in m(0x3b11)['argument_local_sites']
 call(0x3b1c,0x15,0x3196)
 # Native import names, not inferred audible meanings of numeric attributes.
 for rid,name in {0x3122:'BASS_ChannelGetLength',0x313a:'BASS_ChannelIsActive',0x313d:'BASS_ChannelPause',0x313e:'BASS_ChannelPlay',0x3146:'BASS_ChannelSetAttribute',0x3151:'BASS_ChannelStop',0x3197:'BASS_SampleGetChannel',0x3196:'BASS_SampleFree',0x3154:'BASS_Free'}.items():assert d['native_imports'][hex(0x6000000+rid)]=={'module':'bass.dll','entry':name}
 assert d['strings']['2064529249']['text']=='spinnerspin' and d['strings']['2064529271']['text']=='spinnerbonus'
 assert d['strings']['2064456445']['text']=='SpinnerFrequencyModulate'
 root=Path(__file__).resolve().parents[1]
 for name,h in d['authority_inputs'].items():assert sha((root/'evidence'/name).read_bytes())==h
 for name,h in d['collector_sources'].items():assert sha((root/'tools'/name).read_bytes())==h
 cases=d['reference_cases'];assert [x['id']for x in cases]==[f'A{i:02}'for i in range(1,len(cases)+1)]
 assert len(cases)>=50 and all(x['status']=='RECONSTRUCTED STATIC PREDICTION' and x['evidence'] for x in cases)
 shared=0
 for path in sorted((root/'evidence').glob('*/*.json')):
  if path.parent.name=='41':continue
  old=json.loads(path.read_text(encoding='utf-8'))
  for token,v in old.get('methods',{}).items():
   if token in d['methods'] and 'body_sha256' in v:assert v['body_sha256']==d['methods'][token]['body_sha256'];shared+=1
 audited=0
 if archive:
  assert sha(Path(archive).read_bytes())==ARCHIVE
  with zipfile.ZipFile(archive)as z:raw=z.read('osu!.exe')
  assert sha(raw)==EXE
  import dnfile
  from dncil.cil.body.reader import read_method_body_from_bytes
  p=dnfile.dnPE(data=raw);ts=p.net.mdtables;owners={x.row_index:n for n,t in enumerate(ts.TypeDef,1)for x in t.MethodList}
  fowners={x.row_index:n for n,t in enumerate(ts.TypeDef,1)for x in t.FieldList}
  # Include zero-RVA P/Invoke/delegate signatures and field declarations in the
  # independent audit; those have no managed body to hash or execute.
  for token,value in d['methods'].items():
   rid=int(token,16)&0xffffff;row=ts.MethodDef.rows[rid-1]
   assert value['signature']==row.Signature.value.hex() and value['owner']==hex(0x2000000+owners[rid])
   assert value['rva']==hex(row.Rva) and value['virtual']==row.Flags.mdVirtual and value['new_slot']==row.Flags.mdNewSlot
  for token,value in d['fields'].items():
   rid=int(token,16)&0xffffff;row=ts.Field.rows[rid-1]
   assert value['signature']==row.Signature.value.hex() and value['owner']==hex(0x2000000+fowners[rid])
   assert value['name']==str(row.Name) and value['static']==row.Flags.fdStatic
  for slot,recorded in d['slots'].items():
   original=ts.MethodDef.rows[(int(slot,16)&0xffffff)-1]
   actual_slots=[{'method':hex(0x6000000+n),'owner':hex(0x2000000+owners[n]),'virtual':row.Flags.mdVirtual,'new_slot':row.Flags.mdNewSlot} for n,row in enumerate(ts.MethodDef,1) if str(row.Name)==str(original.Name) and row.Signature.value==original.Signature.value]
   assert recorded==actual_slots
  watched={0x4002356,0x40036af,0x40002a2,0x400029e};actual=[]
  for rid,row in enumerate(ts.MethodDef,1):
   if not row.Rva:continue
   b=read_method_body_from_bytes(p.get_data(row.Rva));ins=list(b.instructions);token=hex(0x6000000+rid)
   actual.extend([token,i.offset-b.header_size,i.opcode.name,hex(i.operand.value)]for i in ins if getattr(i.operand,'value',None)in watched)
   if token not in d['methods']:continue
   value=d['methods'][token];assert sha(p.get_data(row.Rva,b.size))==value['body_sha256'];assert value['signature']==row.Signature.value.hex() and value['owner']==hex(0x2000000+owners[rid])
   assert value['token_sites']==[[i.offset-b.header_size,i.opcode.name,hex(i.operand.value)]for i in ins if hasattr(i.operand,'value')]
   assert value['constants']==[[i.offset-b.header_size,i.opcode.name,i.operand]for i in ins if i.opcode.name.startswith('ldc.')]
   branches=[]
   for i in ins:
    if i.opcode.name=='switch' or i.opcode.name.startswith(('br','beq','bne','ble','blt','bgt','bge','leave')):branches.append([i.offset-b.header_size,i.opcode.name,[x-b.header_size for x in(i.operand if i.opcode.name=='switch' else[i.operand])]])
   assert branches==value['branches'];audited+=1
  assert actual==[x for x in d['field_sites'] if int(x[3],16)in watched]
  assert not any(x.Parent.table.name=='Field' and x.Parent.row_index==0x2356 for x in ts.CustomAttribute)
  imports={hex(0x6000000+x.MemberForwarded.row_index):{'module':str(x.ImportScope.row.Name),'entry':str(x.ImportName)}for x in ts.ImplMap}
  assert all(imports[k]==v for k,v in d['native_imports'].items())
  # Independent reduced E038 key + byte decoder, not collector class/methods.
  from check_hitsound_resources import independent_key,i32
  key=independent_key();assert key==d['string_decoder']['final_key']
  table=next(r.data for r in p.net.resources if str(r.name)==d['string_decoder']['resource']['name'])
  for identifier,record in d['strings'].items():
   at=i32(int(identifier)^-351960377^key^1516081623);assert at==record['offset'];keys=bytes(table[at+i]^((key>>(8*i))&255)for i in range(3));n=i32(struct.unpack_from('<I',table,at+3)[0]^at^-973073496^key);assert n&0xf0000000==0x40000000
   cipher=table[at+7:at+7+(n&0xfffffff)];assert sha(cipher)==record['cipher_sha256'];last=((len(cipher)+11)^(keys[1]+7))&255;state=keys[0]+(keys[2]<<8)+(last<<3);out=[];word=0
   for index,byte in enumerate(cipher):
    if index%2==0:state=(state*214013+2531011)&0xffffffff;word=(state>>16)&65535
    out.append(chr((keys[1]^byte^(last+3)^(word&255))&255));word>>=8;last=byte
   assert ''.join(out)==record['text']
  p.close()
  with zipfile.ZipFile(archive)as z:
   for store in d['embedded_spinner_resources']:
    raw=z.read(store['assembly']);assert sha(raw)==store['assembly_sha256'];p=dnfile.dnPE(data=raw);res=next(r for r in p.net.resources if str(r.name)==store['resource_set']);entries={str(e.name):e for e in res.data.entries};assert len(entries)==store['entry_count']
    for name,v in store['keys'].items():
     if v is None:assert name not in entries
     else:assert entries[name].type_name==v['type'] and len(entries[name].value)==v['bytes'] and sha(entries[name].value)==v['sha256']
    p.close()
 print(f'PASS E041: {len(d["methods"])} methods;12 direct channel sites;{len(cases)} conditional predictions;{shared} historical body hashes;{audited} independently byte-audited bodies;no original/audio execution')
if __name__=='__main__':check(json.loads(Path(sys.argv[1]).read_text(encoding='utf-8')),sys.argv[2]if len(sys.argv)>2 else None)
