#!/usr/bin/env python3
"""Read-only Expedition 06 metadata/CIL evidence. No geometry or game execution.
Usage: python re/tools/linear_geometry_evidence.py /path/osubackup.zip > geometry.json
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2; unchanged E04/E05 collectors.
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS=[0x113e,0x651,0x652,0x48de,0x48e0,0x142d,0x28f7,0x9e4,
 0x144a,0x110c,0x110d,0x4ec9,0x4ecb,0x4ed2,0x4efd,0x4efe,0x4f00,0x4efb,0x4efc,
 0x28f5,0x28e9,0x28ea,0x28ec,0x28eb,0x1457,0x144b,0x24b5,0x28e5,0x289a,
 0x2269,0x223c,0x223b,0x145f,0x145e,0x3e5d,0x1449,0x24b2,0x28e3,0x28e2,
 0x144f,0x709,0x70d,0x46a6]
WATCH_FIELDS={0x4000bdc,0x4000bde}

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=6
    with zipfile.ZipFile(path) as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owner={m.row_index:n for n,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    t=ts.TypeDef.rows[0x3bb-1];ids={f.row_index for f in t.FieldList}
    out['enums']['0x20003bb']={'name':str(t.TypeName),'values':{
      str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
      for c in ts.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in ids}}
    for rid in [0x24b5,0x24b2,0x3e5d]:
        s=ts.MethodDef.rows[rid-1]
        out['slot_candidates'][hex(0x6000000+rid)]=[
          {'method':hex(0x6000000+i),'owner':hex(0x2000000+owner[i]),
           'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
          for i,r in enumerate(ts.MethodDef,1) if r.Flags.mdVirtual
          and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value]
    hits={};callers=[];decoded=0;zero=0
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:zero+=1;continue
        b=read_method_body_from_bytes(p.get_data(m.Rva));decoded+=1
        for ins in b.instructions:
            v=getattr(ins.operand,'value',None);off=ins.offset-b.header_size
            if v in WATCH_FIELDS:
                hits.setdefault(hex(0x6000000+rid),[]).append([off,ins.opcode.name,hex(v)])
            if v==0x60024b5 and ins.opcode.name in {'call','callvirt','ldftn','ldvirtftn'}:
                callers.append([hex(0x6000000+rid),off,ins.opcode.name])
    out['prepared_field_sites']=hits
    out['visual_slot_direct_callers']=callers
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    out['limits'].extend(['All prepared-field sites are syntactic, not a temporal execution order',
      'First identified post-load consumer is not proof of globally earliest read',
      'No geometry implementation, frame measurements, visibility or GPU claims'])
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
