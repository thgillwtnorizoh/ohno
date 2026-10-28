#!/usr/bin/env python3
"""E14 read-only original CIL/metadata collection; no projection or renderer execution.
Requires pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 and unchanged E04/E05 tools.
"""
import hashlib
import json
import struct
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS=[0x45eb,0x45ec,0x45f2,0x24b0,0x45f6,0x4510,0x4518,0x4519,0x451a,
 0x4524,0x4525,0x4728,0x4737,0x4738,0xff1,0xff5,0xfe6,0xfea,0xfed,
 0x4ef5,0x4eee,0x4f03,0x28be,0x289b,0x289a,0x28b7,0x2cd8,0x2cf0,
 0x2cfe,0x2cff,0x2d00,0x2929,0x292d,0x3e28,0x3e2d,0x3e33,0x52c8,0x52ca,
 0x17aa,0x1e41,0x1e44,0x1e45,0x1e46,0x1e47,0x1196,0x4695,0x46a6,
 0x4b17,0x4b18,0x4b1a,0x3235,0x3236,0x2161,0x4442,0x4441,0x4440,0x226b]
FIELDS={0x4000000+n for n in [0x252c,0x2bf9,0x2cf6,0x2cec,0x2b6e,0x2b78,
 0x2b7f,0x2b81,0x2b8c,0x2b8d,0x2b8f,0x2b77,0x2b6f,0x2b85,0x1a5f,
 0x1a60,0x1a6a,0x1a73,0x116e,0x116f,0x1174,0x1175,0x1176,0x3637,0x3638,
 0x3639,0x363a,0x2f0f,0x2f09,0x12fa]}
CALLS={0x6000000+n for n in [0xff5,0x24b0,0x1e45,0x1e46,0x4b1a,0x52ca,0x4442]}

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=14
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    fowners={f.row_index:i for i,t in enumerate(ts.TypeDef,1)for f in t.FieldList}
    for v in FIELDS:
        f=ts.Field.rows[(v&0xffffff)-1]
        out['fields'][hex(v)]={'owner':hex(0x2000000+fowners[v&0xffffff]),
          'signature':f.Signature.value.hex(),'static':f.Flags.fdStatic}
    out['circle_position_slot']=[]
    s=ts.MethodDef.rows[0x24b0-1]
    for i,r in enumerate(ts.MethodDef,1):
        if r.Flags.mdVirtual and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value:
            out['circle_position_slot'].append({'method':hex(0x6000000+i),
              'owner':hex(0x2000000+owners[i]),'new_slot':r.Flags.mdNewSlot})
    out['declarations']={}
    for tid in [0x94c,0x4c3,0x35a,0x828,0x92e,0x970,0xb26,0x826]:
        t=ts.TypeDef.rows[tid-1]
        out['declarations'][hex(0x2000000+tid)]={
          'parent_table':t.Extends.table.name,'parent_rid':t.Extends.row_index,
          'parent_signature':getattr(getattr(t.Extends.row,'Signature',None),'value',b'').hex(),
          'sequential':t.Flags.tdSequentialLayout,'explicit':t.Flags.tdExplicitLayout,
          'fields':[{'token':hex(0x4000000+x.row_index),'name':str(x.row.Name),
            'signature':x.row.Signature.value.hex()}for x in t.FieldList]}
    out['relevant_method_impls']=[{'class':hex(0x2000000+r.Class.row_index),
      'body_table':r.MethodBody.table.name,'body_rid':r.MethodBody.row_index,
      'declaration_table':r.MethodDeclaration.table.name,'declaration_rid':r.MethodDeclaration.row_index}
      for r in ts.MethodImpl if r.Class.row_index in [0x94c,0x4c3,0x35a,0x828]]
    out['projection_field_sites']={};out['boundary_direct_callers']=[];decoded=zero=0
    for rid,r in enumerate(ts.MethodDef,1):
        if not r.Rva:zero+=1;continue
        b=read_method_body_from_bytes(p.get_data(r.Rva));decoded+=1
        for i in b.instructions:
            v=getattr(i.operand,'value',None);off=i.offset-b.header_size
            if v in FIELDS:out['projection_field_sites'].setdefault(hex(0x6000000+rid),[]).append([off,i.opcode.name,hex(v)])
            if v in CALLS and i.opcode.name in ['call','callvirt','newobj','ldftn','ldvirtftn']:
                out['boundary_direct_callers'].append([hex(0x6000000+rid),off,i.opcode.name,hex(v)])
        if rid in METHODS:
            out['methods'][hex(0x6000000+rid)]['float_constant_bits']=[
              [i.offset-b.header_size,i.opcode.name,struct.pack('<f'if i.opcode.name=='ldc.r4'else'<d',i.operand).hex()]
              for i in b.instructions if i.opcode.name in ['ldc.r4','ldc.r8']]
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    for tid in [0x71b,0x502]:
        t=ts.TypeDef.rows[tid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+tid)]={'name':str(t.TypeName),'values':{
          str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
          for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
    p.close()
    with zipfile.ZipFile(path)as z:raw=z.read('OpenTK.dll')
    q=dnfile.dnPE(data=raw);qt=q.net.mdtables
    out['opentk']={'sha256':hashlib.sha256(raw).hexdigest(),'methods':{}}
    for rid in [0x92c,0x92d]:
        m=qt.MethodDef.rows[rid-1];b=read_method_body_from_bytes(q.get_data(m.Rva));h=b.header_size
        out['opentk']['methods'][hex(0x6000000+rid)]={
          'name':str(m.Name),'signature':m.Signature.value.hex(),'rva':hex(m.Rva),
          'header_bytes':h,'code_bytes':b.code_size,
          'body_sha256':hashlib.sha256(q.get_data(m.Rva,b.size)).hexdigest(),
          'token_sites':[[i.offset-h,i.opcode.name,hex(i.operand.value)]for i in b.instructions if hasattr(i.operand,'value')],
          'arithmetic_sites':[[i.offset-h,i.opcode.name]for i in b.instructions if i.opcode.name in previous.ARITH],
          'constants':[[i.offset-h,i.opcode.name,i.operand]for i in b.instructions if i.opcode.name.startswith('ldc.')]}
    out['opentk']['matrix_fields']={hex(0x4000000+rid):str(qt.Field.rows[rid-1].Name)for rid in [0x123,0x124,0x125,0x126,0xea,0xeb,0xec]}
    q.close()
    out['limits'].extend(['No original execution, coordinate converter, projection simulator or replacement renderer',
      'CPU geometry and matrix/attribute invocation facts do not establish shader use, GL success or pixels',
      'Decoded shader/reflection strings, runtime resource contents, native/DPI/context/JIT state unresolved',
      'Direct field/address inventory does not exclude indirect, reflective or native mutation'])
    return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
