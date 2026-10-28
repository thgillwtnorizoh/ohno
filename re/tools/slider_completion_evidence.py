#!/usr/bin/env python3
"""E17 read-only metadata/CIL extraction; never evaluates a substitute slider.
Requires dnfile0.18.0/pefile2024.8.26/dncil1.0.2; unchanged E04/E05/E15 tools.
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_entry_evidence as collector

METHODS=[0x113e,0x1141,0x651,0x652,0x48de,0x48e0,0x142d,0x143a,0x351a,
 0x351d,0x1884,0x1463,0x1465,0x1466,0x1469,0x146a,0x146b,0x146c,0x3b53,
 0x3ba8,0x1449,0x146f,0x146d,0x146e,0x144a,0x145c,0x24a9,0x24bb,0x24be,
 0x24c7,0x24b3,0x24c4,0x1431,0x28e4,0x28e3,0x653,0x654,0x28d3,0x28bb,
 0x28c2,0x2246,0x229c,0x291a,0x28d4,0x28d5,0x3e56,0x226b,0x2269,
 0x2275,0x22c5,0xac5,0x114a,0xad2,0x2295,0x2241,0x2258,0x28be,0x28e2,
 0x28d2,0x28db]
FIELDS={0x4000000+n for n in [0x2358,0x2523,0x2524,0x252e,0x1527,0x152a,
 0xbdf,0xbf1,0xbf2,0xbf3,0xbf4,0xbf5,0xbf6,0xbf7,0xbf8,0x13c3,0x198e,
 0x17fd,0x17fb,0x17fc,0x17f8,0x17ff,0x17f6,0x17f7,0x17ef,0x1804,0x13ea,
 0x2a90,0x2c6d,0xadc,0x1380,0x13d9]}

def extract(path):
    collector.METHODS=METHODS;collector.FIELDS=FIELDS
    collector.CALLS={0x6000000+n for n in [0x1465,0x24a9,0x146f,0x24c3,0x28d3,
      0x28e4,0x291a,0x229c,0xad2,0x1466,0x146c]}
    collector.CHAINS={0x2af:[0x24a9,0x24b2,0x24be,0x24bb,0x24c4,0x24c3],
      0x6f6:[0x24a9],0xe8:[0x28d3,0x28e3,0x28e4,0x28bb,0x28d4,0x28d5],
      0x253:[0xac5,0xad2]}
    out=collector.extract(path);out['expedition']=17
    out['limits']=[s for s in out['limits']if not s.startswith('First tracking predicate')]
    out['limits'].extend(['Parent completion and aggregate-return boundary only; full scoring/effects/seek-reset policy unresolved',
      'No original execution, numerical completion engine, or pending-component catch-up implementation'])
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    for rid in METHODS:
        r=ts.MethodDef.rows[rid-1]
        if not r.Rva:continue
        b=read_method_body_from_bytes(p.get_data(r.Rva))
        out['methods'][hex(0x6000000+rid)]['array_sites']=[
          [i.offset-b.header_size,i.opcode.name]for i in b.instructions
          if i.opcode.name.startswith(('ldelem','stelem'))]
    t=ts.TypeDef.rows[0x488-1]
    out['result_enum_fields']=[{'token':hex(0x4000000+f.row_index),'name':str(f.row.Name),
      'signature':f.row.Signature.value.hex(),'literal':f.row.Flags.fdLiteral}for f in t.FieldList]
    out['result_interface']={'invoke':'0x600291a',
      'invoke_signature':ts.MethodDef.rows[0x291a-1].Signature.value.hex(),
      'result_storage_field':'0x400152a','aggregate_method':'0x6001465'}
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
