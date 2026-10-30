#!/usr/bin/env python3
"""E25 original CIL/metadata extraction; no spinner/result/score implementation."""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_entry_evidence as collector

METHODS=[0x5eb,0x5f6,0x5f4,0x5f5,0x5ea,0x48e1,0x3201,0x3b28,0x604,
 0xac5,0x114a,0xad2,0xad0,0x297a,0x297b,0x2966,0x2967,0x2968,0x2969,
 0x40f0,0x40f1,0x226b,0x2275,0x22c5,0x28e3,0x653,0x654,0x28e4,
 0x43a0,0x3350,0x46d7,0x46fb,0x1141,0x651,0x652,0x113e,0x28f0,0x249b,
 0x28be,0x28b3,0x28b0,0x3e4d,0x125a,0x1ad1,0x3c57,0x3121,0x3122,0x3146,0x5fa,0x28d3,0x5fc]
SPIN=[0x2b4,0x29e,0x2a2,0x2a0]
FIELDS={0x4000000+n for n in SPIN+[0xae0,0x36af,0x2f16,0x2356,0x2358,
 0x2523,0x2524,0x1527,0x152a,0x252e,0x153b,0x153d,0x1380,0x13ea,
 0x2674,0x1975,0x198a,0x5f2,0x5f3,0x5f5,0x5f6,0x604,0x1857,0x1858,
 0x2a90,0x2a94,0x2a95,0x17fd,0xcc6]}
CALLS={0x6000000+n for n in [0x5eb,0x24c3,0xac5,0xad2,0x3b28,0x5f4,
 0x40f1,0x297a,0x297b,0x2967,0x2969]}
CHAINS={0xd8:[0x24c3,0x24b2,0x24a9,0x24be,0x24c0],
 0x253:[0xac5,0xad2,0xad0],0xe8:[0x28e3,0x28e4,0x125a],
 0x56a:[0x297a,0x297b]}

def extract(path):
    collector.METHODS=METHODS;collector.FIELDS=FIELDS;collector.CALLS=CALLS
    collector.CHAINS=CHAINS
    out=collector.extract(path);out['expedition']=25
    out['limits']=[
      'Original static instructions/metadata only; no Windows, CLR, native or surrogate gameplay execution',
      'One ordinary T00d8 integer-change/result path with normally returning original helpers',
      'Prepared F02a0 sign/range and N+3 predicate only; full requirement/final judgement/bonus payout deferred',
      'Direct EXE field/caller inventories exclude reflection/native/external mutation',
      'First and later progress reads, counter reads and current-object references are separate',
      'E20 preparation-only; all17 runtime cases NOT RUN — WINDOWS HOST REQUIRED']
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    fowners={f.row_index:i for i,t in enumerate(ts.TypeDef,1)for f in t.FieldList}
    out['focus_field_metadata']={}
    for rid in SPIN+[0x36af,0x604,0x153d,0x1857,0x1858]:
        f=ts.Field.rows[rid-1]
        out['focus_field_metadata'][hex(0x4000000+rid)]={
          'owner':hex(0x2000000+fowners[rid]),'name':str(f.Name),
          'signature':f.Signature.value.hex(),
          'flags':sorted(k for k,v in vars(f.Flags).items()if isinstance(v,bool)and v)}
    out['field_access_owners']={t:{'owner':hex(0x2000000+owners[int(t,16)&0xffffff]),
      'signature':ts.MethodDef.rows[(int(t,16)&0xffffff)-1].Signature.value.hex()}
      for t in out['entry_field_sites']}
    for rid in METHODS:
        m=ts.MethodDef.rows[rid-1]
        if not m.Rva:continue
        b=read_method_body_from_bytes(p.get_data(m.Rva));row=out['methods'][hex(0x6000000+rid)]
        row['local_argument_sites']=[
          [i.offset-b.header_size,i.opcode.name,getattr(i.operand,'index',None)]
          for i in b.instructions if i.opcode.name.startswith(('ldarg','starg','ldloc','stloc'))]
        row['array_sites']=[[i.offset-b.header_size,i.opcode.name]
          for i in b.instructions if i.opcode.name.startswith(('ldelem','stelem'))]
    out['result_enum']={'token':'0x2000488','fields':[{
      'token':hex(0x4000000+f.row_index),'name':str(f.row.Name),
      'signature':f.row.Signature.value.hex(),'literal':f.row.Flags.fdLiteral}
      for f in ts.TypeDef.rows[0x488-1].FieldList]}
    out['native_imports']=[{'method':hex(0x6000000+r.MemberForwarded.row_index),
      'import_name':str(r.ImportName),'module':str(r.ImportScope.row.Name)}
      for r in ts.ImplMap if r.MemberForwarded.table.name=='MethodDef'
      and r.MemberForwarded.row_index in METHODS]
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
