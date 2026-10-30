#!/usr/bin/env python3
"""E24 read-only original spinner input/progress evidence; never a spinner model."""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_entry_evidence as collector

METHODS=[0x113e,0x1141,0x651,0x652,0x48de,0x48e1,0x1872,0x28a2,0x28f7,
 0x28f0,0x28d2,0x3201,0x5ea,0x1d6d,0x5f3,0x5f4,0x5f5,0x5f6,0x5f7,
 0x5eb,0x5fc,0x28ae,0x4efe,0x4f00,0x18ff,0x18fd,0x1902,0x4510,0x4518,
 0x2275,0x22c5,0xac5,0x114a,0x226b,0x28e2,0x28e3,0x653,0x654,0x28e4,
 0x24c0,0x24be,0x249b,0x2295,0x43a0,0x43a6,0x43a7,0x43a8,0x4390,
 0xff1,0x3b25,0x5fb,0x5fd,0x4728,0x438f,0x43a1,0x46a1,
 0x4241,0x469e,0x46d7,0x46fb,0x3350]
SPIN_FIELDS=[0x29d,0x29e,0x29f,0x2a0,0x2a1,0x2a2,0x2a3,0x2af,0x2b0,
 0x2b1,0x2b3,0x2b4,0x2b5,0x2b6]
FIELDS={0x4000000+n for n in SPIN_FIELDS+[0x2a7,0x1527,0x2523,0x2524,0x2525,
 0x252c,0x152f,0x1380,0x17fb,0x17fd,0x2358,0x2a5c,0x2a90,0x2a94,0x2a95,
 0x2a96,0x2a67,0x2a83,0x137b,0x137c,0x1376,0x2c30,0x2b6e]}
CALLS={0x6000000+n for n in [0x24c3,0x24b2,0x5f5,0x5f6,0x5f4,0x1872,
 0x48e1,0x5ea,0x28e3,0x28ae,0x5eb]}
CHAINS={0xd8:[0x24c3,0x24b2,0x24b6,0x24a9,0x24c0,0x24be,0x24c4],
 0x3ea:[0x24c3,0x24b2],0x253:[0xac5,0xabb],0xe8:[0x28e3,0x28e4,0x28bc],
 0x9c1:[0x1872]}

def extract(path):
    collector.METHODS=METHODS;collector.FIELDS=FIELDS;collector.CALLS=CALLS
    collector.CHAINS=CHAINS
    out=collector.extract(path);out['expedition']=24
    out['limits']=[
      'Original metadata and static instructions only; no original Windows or surrogate gameplay execution',
      'Ordinary T00d8, normal-returning helpers, one finite spinner; special Play/managers/mods not generalized',
      'First input, velocity and progress boundary only; required spins, bonus and final judgement deferred',
      'Direct EXE inventories exclude reflection/native/external mutation',
      'Prepared drawable center/layout and input/clock fields are separate mutable reads',
      'E20 remains preparation-only; all17 runtime cases NOT RUN — WINDOWS HOST REQUIRED']
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    fowners={f.row_index:i for i,t in enumerate(ts.TypeDef,1)for f in t.FieldList}
    out['spinner_field_metadata']={}
    for rid in SPIN_FIELDS:
        f=ts.Field.rows[rid-1]
        out['spinner_field_metadata'][hex(0x4000000+rid)]={
          'owner':hex(0x2000000+fowners[rid]),'name':str(f.Name),
          'signature':f.Signature.value.hex(),
          'flags':sorted(k for k,v in vars(f.Flags).items()if isinstance(v,bool)and v)}
    out['field_access_owners']={t:{'owner':hex(0x2000000+owners[int(t,16)&0xffffff]),
      'signature':ts.MethodDef.rows[(int(t,16)&0xffffff)-1].Signature.value.hex()}
      for t in out['entry_field_sites']}
    for rid in METHODS:
        m=ts.MethodDef.rows[rid-1]
        if not m.Rva:continue
        b=read_method_body_from_bytes(p.get_data(m.Rva))
        row=out['methods'][hex(0x6000000+rid)]
        row['local_argument_sites']=[
          [i.offset-b.header_size,i.opcode.name,getattr(i.operand,'index',None)]
          for i in b.instructions if i.opcode.name.startswith(('ldarg','starg','ldloc','stloc'))]
        row['initialization_sites']=[
          [i.offset-b.header_size,i.opcode.name,hex(i.operand.value)]
          for i in b.instructions if i.opcode.name=='initobj']
    # Original named enum constants, not inferred modifier names.
    out['original_mods']={}
    t=ts.TypeDef.rows[0xb57-1];ids={f.row_index for f in t.FieldList}
    for c in ts.Constant:
        if c.Parent.table.name=='Field'and c.Parent.row_index in ids:
            out['original_mods'][str(c.Parent.row.Name)]={
              'token':hex(0x4000000+c.Parent.row_index),
              'value':int.from_bytes(c.Value.value,'little',signed=True)}
    out['nullable_double_spec']={'token':'0x1b0000b4',
      'signature':ts.TypeSpec.rows[0xb4-1].Signature.value.hex()}
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
