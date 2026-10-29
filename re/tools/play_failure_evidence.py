#!/usr/bin/env python3
"""E19 read-only original metadata/CIL extraction; never simulates HP or failure.
Pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2, unchanged E04/E05/E15 collectors.
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_entry_evidence as collector

METHODS=[0x113e,0x1141,0x651,0x48de,0x48e0,0x142d,0x146f,0x146d,0x146e,
 0xac5,0x114a,0xad2,0xad0,0xacf,0xa9c,0xaaf,0xab4,0xab7,0xace,0xacb,0xacc,
 0x2963,0x2964,0x2965,0x2966,0x2967,0x2968,0x2969,0x2979,0x297a,0x297b,0x297d,
 0x2972,0x28b3,0x28b0,0x125a,0x226b,0x226c,0x227d,0x2232,0x4390,0x225c,
 0x2258,0x226e,0x2241,0x2275,0x3b55,0x1204,0x1202,0x22a8,0x40f0,0x40f1]
FIELDS={0x4000000+n for n in [0x5f5,0x5f3,0x5f4,0x5f7,0x5f8,0x5f6,0x5f2,
 0x1856,0x1857,0x1858,0x1852,0x136a,0x136b,0x1376,0x1373,0x1374,0x137b,
 0x137c,0x13c3,0x1970,0x1971,0x13bb,0x13a4,0x138f,0x2358,0x2a7c,0x2a82,
 0x2c73,0x2674,0x198a,0x153d,0xbf1,0xbf5,0xbf3,0x604,0x197d]}

def extract(path):
    collector.METHODS=METHODS;collector.FIELDS=FIELDS
    collector.CALLS={0x6000000+n for n in [0x2979,0x297a,0x297b,0x226c,0x227d,
      0xacb,0xacc,0x2967,0x2969,0xab4]}
    collector.CHAINS={0x253:[0xab4,0xacb,0xad0,0xad2],0xe8:[0x125a],
      0x47b:[0x227d],0x56a:[0x2979,0x297a,0x297b],0x124:[0x2979,0x297a],
      0x711:[0x2979],0x2af:[0x24c3]}
    out=collector.extract(path);out['expedition']=19
    out['limits']=[x for x in out['limits']if not x.startswith('First tracking predicate')]
    out['limits'].extend(['One checkpoint result to first fail/continue boundary only; no complete HP/score/lifecycle policy',
      'No original Windows, synthetic HP/failure engine, callback/field probe, pixels or sound execution'])
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    for tid in [0xb57,0xb58,0xb4b]:
        t=ts.TypeDef.rows[tid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+tid)]={'namespace':str(t.TypeNamespace),'name':str(t.TypeName),
          'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
          for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
    out['mod_wrapper']={'field':'0x4001970','field_signature':ts.Field.rows[0x1970-1].Signature.value.hex(),
      'member':'0xa0002ca','name':str(ts.MemberRef.rows[0x2ca-1].Name),
      'signature':ts.MemberRef.rows[0x2ca-1].Signature.value.hex(),
      'parent_typespec':'0x1b0000b1','typespec_signature':ts.TypeSpec.rows[0xb1-1].Signature.value.hex(),
      'definition':'0x6001204','getter':'0x6001202'}
    for rid in METHODS:
        r=ts.MethodDef.rows[rid-1]
        if not r.Rva:continue
        b=read_method_body_from_bytes(p.get_data(r.Rva))
        out['methods'][hex(0x6000000+rid)]['array_sites']=[
          [i.offset-b.header_size,i.opcode.name]for i in b.instructions
          if i.opcode.name.startswith(('ldelem','stelem'))]
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
