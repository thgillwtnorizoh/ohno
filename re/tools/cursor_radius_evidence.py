#!/usr/bin/env python3
"""E13 read-only original metadata/CIL extraction; never maps a cursor or judges.
Requires dnfile0.18.0/pefile2024.8.26/dncil1.0.2 and unchanged E04/E05 tools.
Usage: python re/tools/cursor_radius_evidence.py osubackup.zip > spatial.json
"""
import json
import struct
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS=[0xff1,0xfe1,0xfe2,0xfe3,0xfe4,0xfe5,0xfe6,0xfe7,0xfea,0xfed,0xffd,
 0x18ff,0x18fc,0x1907,0x119b,0x119c,0x1193,0x1192,0x11a3,0x1196,0x4695,
 0x466a,0x466b,0x2708,0x270b,0x270d,0x1e12,0x6de,0x9f2,0xa56,0xdb1,
 0x43a0,0x43b2,0x43b3,0x2295,0x28c8,0x28c9,0x24c7,0x4ecd,0x4efe,0x4f03,
 0x4ebe,0x28be,0x1259,0x28b2,0x28b0,0x28a0,0x28f5,0x1387,0x1bc1,0x1bf2,
 0x24b0,0x45eb,0x45ec,0x48df,0x113e,0x1141,0x651,0x652,0x48de,0x226b,
 0x28e2,0x1e45,0x4687,0x468a,0x4668,0x46b0,0x20d5]
FIELDS={0x4000000+n for n in [0x2a5c,0xa89,0x9cc,0x9cd,0x9ce,0x9d0,0x9d1,
 0x9d2,0xe6d,0xe6e,0xada,0x17f1,0xb53,0x17ef,0x252c,0x152e,0x2a7b,0x137c,
 0x17fd,0x153b,0x1175,0x2c9d,0x2c64,0x2c65,0x16b7]}
CALLS={0x6000000+n for n in [0xff1,0xfed,0xffd,0x28be,0x119c,0x1193,0x1196,0x28c9,0x24c7]}
CHAINS={0x94c:[0x24c7],0xe8:[0x1259,0x28be],0x508:[0x17a7]}

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=13
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    fowners={f.row_index:i for i,t in enumerate(ts.TypeDef,1)for f in t.FieldList}
    for v in FIELDS:
        f=ts.Field.rows[(v&0xffffff)-1]
        out['fields'][hex(v)]={'owner':hex(0x2000000+fowners[v&0xffffff]),
          'signature':f.Signature.value.hex(),'static':f.Flags.fdStatic}
    out['ordinary_dispatch']={}
    for root,slots in CHAINS.items():
        chain=[];tid=root
        while tid:
            chain.append(tid);t=ts.TypeDef.rows[tid-1]
            tid=t.Extends.row_index if t.Extends.table.name=='TypeDef'else 0
        matches={}
        for rid in slots:
            s=ts.MethodDef.rows[rid-1]
            matches[hex(0x6000000+rid)]=[{'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
              'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
              for i,r in enumerate(ts.MethodDef,1)if owners[i]in chain and r.Flags.mdVirtual
              and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value]
        out['ordinary_dispatch'][hex(0x2000000+root)]={
          'chain':[hex(0x2000000+i)for i in chain],'slots':matches,
          'declarations':[{'type':hex(0x2000000+i),'methods':[{
            'method':hex(0x6000000+x.row_index),'name':str(x.row.Name),
            'signature':x.row.Signature.value.hex(),'virtual':x.row.Flags.mdVirtual,
            'new_slot':x.row.Flags.mdNewSlot,'abstract':x.row.Flags.mdAbstract}
            for x in ts.TypeDef.rows[i-1].MethodList]}for i in chain],
          'method_impls':[{'class':hex(0x2000000+r.Class.row_index),
            'body_table':r.MethodBody.table.name,'body_rid':r.MethodBody.row_index,
            'declaration_table':r.MethodDeclaration.table.name,
            'declaration_rid':r.MethodDeclaration.row_index}for r in ts.MethodImpl if r.Class.row_index in chain]}
    s=ts.MethodDef.rows[0x1e12-1]
    pos=[(i,r)for i,r in enumerate(ts.MethodDef,1)if r.Flags.mdVirtual
        and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value]
    roots={owners[i]for i,_ in pos}
    out['provider_position_declarations']=[{'method':hex(0x6000000+i),
       'owner':hex(0x2000000+owners[i]),'signature':r.Signature.value.hex(),
       'abstract':r.Flags.mdAbstract,'new_slot':r.Flags.mdNewSlot}for i,r in pos]
    out['provider_interfaces']=[{'class':hex(0x2000000+r.Class.row_index),
       'interface_table':r.Interface.table.name,'interface_rid':r.Interface.row_index}
       for r in ts.InterfaceImpl if r.Class.row_index in roots]
    sites={};calls=[];decoded=zero=0
    for rid,r in enumerate(ts.MethodDef,1):
        if not r.Rva:zero+=1;continue
        b=read_method_body_from_bytes(p.get_data(r.Rva));decoded+=1
        for i in b.instructions:
            v=getattr(i.operand,'value',None);off=i.offset-b.header_size
            if v in FIELDS:sites.setdefault(hex(0x6000000+rid),[]).append([off,i.opcode.name,hex(v)])
            if v in CALLS and i.opcode.name in {'call','callvirt','newobj','ldftn','ldvirtftn'}:
                calls.append([hex(0x6000000+rid),off,i.opcode.name,hex(v)])
        if rid in METHODS:
            out['methods'][hex(0x6000000+rid)]['float_constant_bits']=[
              [i.offset-b.header_size,i.opcode.name,struct.pack('<f'if i.opcode.name=='ldc.r4'else'<d',i.operand).hex()]
              for i in b.instructions if i.opcode.name in {'ldc.r4','ldc.r8'}]
    out['spatial_field_sites']=sites;out['boundary_direct_callers']=calls
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    t=ts.TypeDef.rows[0xb57-1];ids={f.row_index for f in t.FieldList}
    out['enums']['0x2000b57']={'name':str(t.TypeName),'values':{
      str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
      for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
    out['limits'].extend(['No original Windows/CLR/provider/coordinate conversion or surrogate execution',
      'Active provider and host/DPI/viewport/layout values require original runtime observation',
      'Float-declared CIL order is preserved; target JIT intermediate precision not measured',
      'CS-like field alias is from original size dataflow; literal parser key remains undecoded',
      'No visual-border equality, full graphics projection or overlapping-object arbitration recovered',
      'Direct field inventory excludes reflective/external/native mutation'])
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
