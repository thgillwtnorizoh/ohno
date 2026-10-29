#!/usr/bin/env python3
"""E21 read-only original CIL/metadata extraction; no slider execution/model.
Dependencies and offset convention are unchanged from E15/E16.
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_component_evidence as prior

METHODS = [0x113e,0x651,0x48de,0x48e0,0x142d,0x1433,0x144a,0x145c,
 0x146f,0x1478,0x2996,0x2be0,0x146d,0x146e,0x1449,0xac5,0x114a,0xad2,
 0x40f0,0x40f1,0x2275,0x22c5,0x226b,0x28e4,0x1465,0x28d3,0x143a,
 0x351d,0x1884,0x28be,0x28ab,0x1be3,0x1be4,0xff1,0x4ecd]
FIELDS = prior.FIELDS | {0x4000000+n for n in
 [0xbd0,0xbd5,0xbf4,0x2ce8,0x1873,0x1874,0x19d0,0x2674,0x1975,0x198a]}

def extract(path):
    prior.METHODS = METHODS
    prior.FIELDS = FIELDS
    out = prior.extract(path)
    out['expedition'] = 21
    out['limits'] = [
      'Original static metadata/instructions only; no Windows/CLR/gameplay execution',
      'One finite linear slider, two traversals and its first reversal; not all repeats',
      'Reference cases are manual conditional predictions, never synthetic runtime results',
      'Helpers, provider correction, separate reads, exceptions and original gates retained',
      'E20 remains preparation-only: NOT RUN — WINDOWS HOST REQUIRED',
      'No full scoring, HP, effects, rendering, runtime precision or performance claim']
    with zipfile.ZipFile(path) as z:
        p = dnfile.dnPE(data=z.read('osu!.exe'))
    ts = p.net.mdtables
    owners = {m.row_index:i for i,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    out['repeat_metadata'] = {}
    for rid in [0x2be0,0x2996,0x1433,0x40f1]:
        m = ts.MethodDef.rows[rid-1]
        out['repeat_metadata'][hex(0x6000000+rid)] = {
          'owner':hex(0x2000000+owners[rid]),'signature':m.Signature.value.hex()}
    out['repeat_collection_members'] = {}
    for rid in [0x2c8,0x4ba,0xad4,0xad5,0xad6]:
        m = ts.MemberRef.rows[rid-1]
        out['repeat_collection_members'][hex(0xa000000+rid)] = {
          'name':str(m.Name),'signature':m.Signature.value.hex(),
          'class_table':m.Class.table.name,'class_rid':m.Class.row_index,
          'class_signature':m.Class.row.Signature.value.hex()}
    # Sparse local/argument sites anchor stack-dataflow notes without distributing
    # the original method body or a full disassembly.
    for rid,ranges in {
      0x144a:[(0xdd6,0xe6d),(0x11c5,0x1200),(0x12be,0x12dc),(0x132f,0x1340)],
      0x2be0:[(0,0x40),(0x279,0x28f)],
    }.items():
        m = ts.MethodDef.rows[rid-1]
        b = read_method_body_from_bytes(p.get_data(m.Rva))
        out['methods'][hex(0x6000000+rid)]['local_argument_sites'] = [
          [i.offset-b.header_size,i.opcode.name,getattr(i.operand,'index',None)]
          for i in b.instructions
          if any(lo<=i.offset-b.header_size<=hi for lo,hi in ranges)
          and i.opcode.name.startswith(('ldloc','stloc','ldarg'))]
    p.close()
    return out

if __name__ == '__main__':
    print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
