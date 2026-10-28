#!/usr/bin/env python3
"""E15 read-only original metadata/CIL extraction, never input/tracking simulation.
Requires dnfile0.18.0/pefile2024.8.26/dncil1.0.2 and unchanged E04/E05 collectors.
"""
import json
import struct
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS=[0x113e,0x1141,0x651,0x652,0x48de,0x48e0,0x142d,0x143a,0x351a,
 0x351d,0x45eb,0x45ec,0x1884,0x2295,0xad9,0x28c8,0x28c9,0x24c7,0x28f0,
 0x28d2,0x28d3,0x229c,0x2246,0x1463,0x1464,0x146f,0x146d,0x146e,0x1478,
 0x144a,0x2996,0x2275,0x22c5,0xac5,0x226b,0x28e2,0x28e3,0x28e4,
 0x653,0x654,0x1449,0x43a0,0x43a6,0x43a7,0x43a8,0xff1,0x4f03,0x4ecd,
 0x28be,0x24be,0x24c0,0x1431,0x146c,0x1457]
FIELDS={0x4000000+n for n in [0x1380,0x13d9,0x17fd,0x17fb,0xbdf,0xbf0,0xbf1,
 0xbf2,0xbf3,0xbf5,0xbf6,0xbf8,0xbd2,0xbd6,0xbdc,0xbde,0x2358,0x2523,
 0x2524,0x252c,0x1527,0x152a,0xada,0x2a5c,0xa7a,0xa7b,0xa7c,0xa80,
 0x2a6e,0x2a8a,0x2a67,0x2a83,0x2a93,0x137b,0x137c,0x3e7,0x153b,0x1867,
 0x186b,0x186e,0x186f,0xbfe]}
CALLS={0x6000000+n for n in [0x143a,0x351a,0x1463,0x1464,0x146d,0x146e,
 0x1478,0x2275,0xac5,0x24c3,0x28d3]}
CHAINS={0x2af:[0x24c7,0x24a9,0x24b2,0x24c3,0x24be,0x24c0,0x24b3,0x24c4],
 0x6f6:[0x24c7,0x24a9,0x24b2,0x24b4],0x253:[0xac5,0xad9],0xe8:[0x28e3,0x28e4]}

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=15
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    fowners={f.row_index:i for i,t in enumerate(ts.TypeDef,1)for f in t.FieldList}
    for v in FIELDS:
        f=ts.Field.rows[(v&0xffffff)-1]
        out['fields'][hex(v)]={'owner':hex(0x2000000+fowners[v&0xffffff]),
          'signature':f.Signature.value.hex(),'static':f.Flags.fdStatic}
    held_type=ts.TypeRef.rows[0x35d-1]
    out['held_mask_type']={'token':'0x100035d','namespace':str(held_type.TypeNamespace),
      'name':str(held_type.TypeName),'scope_table':held_type.ResolutionScope.table.name,
      'scope_rid':held_type.ResolutionScope.row_index}
    out['ordinary_dispatch']={}
    for root,slots in CHAINS.items():
        chain=[];tid=root
        while tid:
            chain.append(tid);t=ts.TypeDef.rows[tid-1]
            tid=t.Extends.row_index if t.Extends.table.name=='TypeDef'else 0
        matches={}
        for rid in slots:
            s=ts.MethodDef.rows[rid-1]
            matches[hex(0x6000000+rid)]=[{'method':hex(0x6000000+i),
              'owner':hex(0x2000000+owners[i]),'new_slot':r.Flags.mdNewSlot,
              'abstract':r.Flags.mdAbstract}for i,r in enumerate(ts.MethodDef,1)
              if owners[i]in chain and r.Flags.mdVirtual
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
    sites={};calls=[];decoded=zero=0
    for rid,r in enumerate(ts.MethodDef,1):
        if not r.Rva:zero+=1;continue
        b=read_method_body_from_bytes(p.get_data(r.Rva));decoded+=1
        for i in b.instructions:
            v=getattr(i.operand,'value',None);off=i.offset-b.header_size
            if v in FIELDS:sites.setdefault(hex(0x6000000+rid),[]).append([off,i.opcode.name,hex(v)])
            if v in CALLS and i.opcode.name in ['call','callvirt','newobj','ldftn','ldvirtftn']:
                calls.append([hex(0x6000000+rid),off,i.opcode.name,hex(v)])
        if rid in METHODS:
            out['methods'][hex(0x6000000+rid)]['float_constant_bits']=[
              [i.offset-b.header_size,i.opcode.name,struct.pack('<f'if i.opcode.name=='ldc.r4'else'<d',i.operand).hex()]
              for i in b.instructions if i.opcode.name in ['ldc.r4','ldc.r8']]
    out['entry_field_sites']=sites;out['boundary_direct_callers']=calls
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    for tid in [0x21a,0x488]:
        t=ts.TypeDef.rows[tid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+tid)]={'name':str(t.TypeName),'values':{
          str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
          for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
    out['limits'].extend(['No original Windows/CLR/input/held-tracking or surrogate execution',
      'First tracking predicate and no-due-component continuation only; complete ticks/repeats/tail/scoring deferred',
      'Presentation Move data is used by original tracking; no assertion that rendering/pixels have occurred',
      'Runtime provider/offset/source state, target JIT precision and interleavings unresolved',
      'Direct field/address inventories exclude reflective/external/native mutation'])
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
