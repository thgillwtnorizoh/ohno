#!/usr/bin/env python3
"""E09 read-only metadata/CIL evidence: no renderer, game or surrogate execution.
Pinned dependencies and unchanged E04/E05 collectors are required.
"""
import hashlib
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS = [0x48df,0x45eb,0x45ec,0x45ed,0x4510,0x4518,0x451a,0x451d,
 0x4524,0x4525,0x4737,0x38b4,0x38b5,0x38be,
 0x27cd,0x27ce,0x27d0,0x27d2,0x27d4,0x27d5,0x27d7,0x27da,
 0x27de,0x27e0,0x27e1,0x27e2,0x27e3,0x2c29,0x2c2b,
 0xa35,0xa36,0xa38,0xa3d,0xa3e,0xa3f,0x291d,
 0x31f5,0x31f6,0x31f7,0x2925,0x2928,0x2929,0x292a,0x292d,0x292e,
 0xe0e,0xe0f,0xe10,0xe14,0xe15,0xe16,0xe17,
 0x3f9a,0x3f9b,0x38ac,0x38ad,0x38ae,0x38af,0x38b0,0x38b1,
 0x3e2d,0x3e2e,0x3e33,0x3e35,
 0x2cf0,0x2cfb,0x2cfc,0x2cfd,0x2cfe,0x2cff,0x2d00,0x1e4a]
SLOTS = [0x31f5,0x31f6,0x31f7,0xe15,0x3e2d,0x3e2e,0x3e2f,0x3e30,0x3e31]
FIELDS = {0x4001764,0x40005c1,0x40019e9}
CALLS = {0x60027cd,0x600291d,0x6000a35,0x60031f5}
MEMBERS = [0x10de,0x10dd,0xebe,0x7f3,0x7f4,0x7f5,0x7f6,0x7f7,0x7f8,
 0x7ff,0x800,0x801,0x804,0x805,0x807,0x808,0x809,0x16e3,
 0x1565,0x1567,0x1568,0x16a4,0x16a5,0x16a6,0x16a7,0x16a8]

def compressed(b,off):
    a=b[off]
    if a < 128:return a,off+1
    if a < 192:return ((a&63)<<8)|b[off+1],off+2
    return ((a&31)<<24)|(b[off+1]<<16)|(b[off+2]<<8)|b[off+3],off+4

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=9
    with zipfile.ZipFile(path) as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    for rid in METHODS:
        r=ts.MethodDef.rows[rid-1]
        if r.Rva:
            b=read_method_body_from_bytes(p.get_data(r.Rva))
            out['methods'][hex(0x6000000+rid)]['null_sites']=[i.offset-b.header_size for i in b.instructions if i.opcode.name=='ldnull']
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    for rid in SLOTS:
        s=ts.MethodDef.rows[rid-1]
        out['slot_candidates'][hex(0x6000000+rid)]=[
          {'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
           'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
          for i,r in enumerate(ts.MethodDef,1) if r.Flags.mdVirtual
          and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value]
    out['receiver_descendants']=[]
    pending={0x67e}
    while pending:
        parent=pending.pop()
        for i,t in enumerate(ts.TypeDef,1):
            if t.Extends.table and t.Extends.table.name=='TypeDef' and t.Extends.row_index==parent:
                out['receiver_descendants'].append({'type':hex(0x2000000+i),'parent':hex(0x2000000+parent),'abstract':t.Flags.tdAbstract})
                pending.add(i)
    out['receiver_descendants'].sort(key=lambda x:x['type'])
    out['declarations']={}
    for tid in [0x67e,0x17a,0x55e,0x533,0x5bf,0x1f3,0x854,0x826,0x77d,0xb26]:
        t=ts.TypeDef.rows[tid-1]
        out['declarations'][hex(0x2000000+tid)]={
          'extends_table':t.Extends.table.name,'extends_rid':t.Extends.row_index,
          'extends_signature':getattr(getattr(t.Extends.row,'Signature',None),'value',b'').hex(),
          'methods':[{'token':hex(0x6000000+x.row_index),'name':str(x.row.Name),
             'signature':x.row.Signature.value.hex(),'virtual':x.row.Flags.mdVirtual,
             'new_slot':x.row.Flags.mdNewSlot,'abstract':x.row.Flags.mdAbstract} for x in t.MethodList],
          'fields':[{'token':hex(0x4000000+x.row_index),'name':str(x.row.Name),
             'signature':x.row.Signature.value.hex()} for x in t.FieldList]}
    out['receiver_method_impls']=[{'class':hex(0x2000000+r.Class.row_index),
      'body_table':r.MethodBody.table.name,'body_rid':r.MethodBody.row_index,
      'declaration_table':r.MethodDeclaration.table.name,'declaration_rid':r.MethodDeclaration.row_index}
      for r in ts.MethodImpl if r.Class.row_index in [0x67e,0x17a,0x55e]]
    out['generic_member_refs']={}
    for rid in MEMBERS:
        r=ts.MemberRef.rows[rid-1]; owner=r.Class
        raw=getattr(getattr(owner.row,'Signature',None),'value',b'')
        fact={'name':str(r.Name),'signature':r.Signature.value.hex(),'class_table':owner.table.name,
              'class_rid':owner.row_index,'class_signature':raw.hex()}
        # Metadata resolution only: GENERICINST CLASS TypeDef + declared members.
        if raw[:2]==bytes([0x15,0x12]):
            enc,_=compressed(raw,2)
            if enc&3==0:
                tid=enc>>2; t=ts.TypeDef.rows[tid-1]
                fact['generic_type_definition']=hex(0x2000000+tid)
                pool=t.FieldList if r.Signature.value[0]==6 else t.MethodList
                base=0x4000000 if r.Signature.value[0]==6 else 0x6000000
                fact['same_name_declarations']=[{'token':hex(base+x.row_index),
                  'signature':x.row.Signature.value.hex()} for x in pool if x.row.Name.value==r.Name.value]
        out['generic_member_refs'][hex(0xa000000+rid)]=fact
    s=ts.MethodSpec.rows[0x19c-1];r=s.Method.row
    out['buffer_subdata_spec']={'token':'0x2b00019c','method_table':s.Method.table.name,
      'method_rid':s.Method.row_index,'instantiation':s.Instantiation.value.hex(),
      'name':str(r.Name),'signature':r.Signature.value.hex()}
    fields={};calls=[];decoded=zero=0
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:zero+=1;continue
        b=read_method_body_from_bytes(p.get_data(m.Rva));decoded+=1
        for i in b.instructions:
            v=getattr(i.operand,'value',None);off=i.offset-b.header_size
            if v in FIELDS:fields.setdefault(hex(0x6000000+rid),[]).append([off,i.opcode.name,hex(v)])
            if v in CALLS and i.opcode.name in {'call','callvirt','newobj','ldftn','ldvirtftn'}:
                calls.append([hex(0x6000000+rid),off,i.opcode.name,hex(v)])
    out['receiver_field_sites']=fields;out['boundary_direct_callers']=calls
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    out['limits'].extend(['No original execution, renderer, surrogate model, pixels or frame-time tests',
      'Managed metadata inventory does not rule out reflection or external/native mutation',
      'Cache hits preserve receiver history; null-atlas cache-miss proof is conditional',
      'OpenTK GL calls are backend invocation sites, not GPU completion or visible-frame evidence'])
    p.close()
    # The immediate OpenTK continuation is two small managed wrappers only.
    with zipfile.ZipFile(path) as z:raw=z.read('OpenTK.dll')
    q=dnfile.dnPE(data=raw); qt=q.net.mdtables
    out['opentk']={'archive_member':'OpenTK.dll','sha256':hashlib.sha256(raw).hexdigest(),'methods':{}}
    for rid in [0x2fd5,0x30b2]:
        m=qt.MethodDef.rows[rid-1];b=read_method_body_from_bytes(q.get_data(m.Rva))
        out['opentk']['methods'][hex(0x6000000+rid)]={
          'name':str(m.Name),'signature':m.Signature.value.hex(),'rva':hex(m.Rva),
          'header_bytes':b.header_size,'code_bytes':b.code_size,
          'body_sha256':hashlib.sha256(q.get_data(m.Rva,b.size)).hexdigest(),
          'token_sites':[[i.offset-b.header_size,i.opcode.name,hex(i.operand.value)]
            for i in b.instructions if hasattr(i.operand,'value')],
          'constants':[[i.offset-b.header_size,i.opcode.name,i.operand]
            for i in b.instructions if i.opcode.name.startswith('ldc.')],
          'arithmetic_sites':[[i.offset-b.header_size,i.opcode.name]
            for i in b.instructions if i.opcode.name in {'ldelem.i','ret'}]}
    out['opentk']['draw_int_offset_overloads']=[hex(0x6000000+x.row_index) for t in qt.TypeDef if str(t.TypeNamespace)=='OpenTK.Graphics.ES20' and str(t.TypeName)=='GL' for x in t.MethodList if str(x.row.Name)=='DrawElements' and x.row.Signature.value.hex().endswith('08')]
    out['opentk']['native_call_signature']=qt.StandAloneSig.rows[0x2a6-1].Signature.value.hex()
    f=qt.Field.rows[0x5be9-1]
    out['opentk']['entry_points_field']={'token':'0x4005be9','name':str(f.Name),'signature':f.Signature.value.hex()}
    r=qt.MemberRef.rows[0x198-1]
    out['opentk']['offset_conversion']={'member_ref':'0xa000198','name':str(r.Name),
      'class':str(r.Class.row.TypeNamespace)+'.'+str(r.Class.row.TypeName),'signature':r.Signature.value.hex()}
    q.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
