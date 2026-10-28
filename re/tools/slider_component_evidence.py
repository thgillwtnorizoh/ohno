#!/usr/bin/env python3
"""E16 read-only original metadata/CIL evidence; no slider or score simulation.
Reuses unchanged E04/E05/E15 collectors. Requires pinned dnfile/pefile/dncil.
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_entry_evidence as prior

METHODS=sorted(set(prior.METHODS+[0x145c,0x114a,0xad2,0xad0,0x28ab,0x1be3,
 0x1be4,0x1be6,0xb78,0xb79,0x1387,0x1bc1,0x143e,0x1465,0x3e4d,0x2282]))
FIELDS=prior.FIELDS|{0x4000000+n for n in [0xbf7,0xbfc,0xadf,0xb56,0xb57,
 0xfd1,0x17ee,0x2527,0x2528,0x5f7,0x5f3,0x5f6,0x604,0x13ea,0x153d,0x252e]}

def extract(path):
    prior.METHODS=METHODS
    prior.FIELDS=FIELDS
    prior.CALLS=prior.CALLS|{0x6000000+n for n in [0x145c,0xad2,0x114a,0xad0]}
    prior.CHAINS={**prior.CHAINS,0x253:[0xac5,0xad9,0xad2,0xad0]}
    out=prior.extract(path);out['expedition']=16
    out['limits']=[x for x in out['limits']if not x.startswith('First tracking predicate')]
    out['limits'].extend(['Scheduled-item producer and one reached item result only; full repeats/tail/aggregate/score unresolved',
      'No executable component scheduler, classification model or numerical fixture evaluator'])
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    out['collection_metadata']={}
    for rid in [0x147,0x14c,0x50f,0x511,0x8ad,0xad2]:
        r=ts.MemberRef.rows[rid-1];c=r.Class
        out['collection_metadata'][hex(0xa000000+rid)]={
          'name':str(r.Name),'signature':r.Signature.value.hex(),
          'class_table':c.table.name,'class_rid':c.row_index,
          'class_signature':c.row.Signature.value.hex()if c.table.name=='TypeSpec'else None}
    r=ts.TypeRef.rows[0xb0-1]
    out['list_typeref']={'token':'0x10000b0','namespace':str(r.TypeNamespace),'name':str(r.TypeName)}
    out['boolean_typeref']={'token':'0x100008f','name':str(ts.TypeRef.rows[0x8f-1].TypeName)}
    for rid in METHODS:
        r=ts.MethodDef.rows[rid-1]
        if not r.Rva:continue
        b=read_method_body_from_bytes(p.get_data(r.Rva))
        out['methods'][hex(0x6000000+rid)]['array_sites']=[
          [i.offset-b.header_size,i.opcode.name]for i in b.instructions
          if i.opcode.name.startswith(('ldelem','stelem'))]
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
