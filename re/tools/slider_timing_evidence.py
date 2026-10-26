#!/usr/bin/env python3
"""Expedition 05 read-only CIL evidence collector. Never executes osu! or a surrogate.
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2; reuses E04 collector.
Usage: python re/tools/slider_timing_evidence.py /path/osubackup.zip > timing.json
"""
import json
import math
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import beatmap_init_evidence as collector

METHODS = [0x223b,0x2234,0x113e,0xda8,0x1141,0x651,0x652,0x3203,0x3208,
 0x48de,0x48e0,0x1871,0x142d,0x1433,0x144a,0x144f,0x145b,0x110d,
 0x125b,0x28ab,0x28be,0x28f5,0x28f7,0x28ff,0x28e9,0x28ea,0x28eb,0x28ec,
 0x1bc1,0x1be3,0x1be4,0x1be6,0xb73,0xb74,0xb75,0xb78,0xb79,0x2d7d,
 0x1387,0x9e4,0x9e5,0x3e4d,0x1ecb,0x1ee1,0x20cd,0x20d4,0x20d6,0x20d7,
 0x426f,0x424e,0x4261,0x4263,0x18e2,0x708,0x70a,0x70d,0x223c]
ARITH = {'add','sub','mul','div','rem','neg','and','or','xor','shr','shl',
         'ceq','clt','clt.un','cgt','cgt.un'}

def clean(x):
    if isinstance(x,float) and not math.isfinite(x):return str(x)
    if isinstance(x,list):return [clean(v) for v in x]
    if isinstance(x,dict):return {k:clean(v) for k,v in x.items()}
    return x

def extract(path):
    collector.METHODS=METHODS
    out=collector.extract(path)
    out['expedition']=5
    out['limits'].extend(['Arithmetic sites are xrefs, not an executable formula',
      'Predictions require resolved virtual targets and normally returning opaque helpers',
      'No timing measurements, original CLR conversion probes or geometry equivalence'])
    with zipfile.ZipFile(path) as z: p=dnfile.dnPE(data=z.read('osu!.exe'))
    for rid in METHODS:
        m=p.net.mdtables.MethodDef.rows[rid-1]
        if not m.Rva:continue
        b=read_method_body_from_bytes(p.get_data(m.Rva))
        row=out['methods'][hex(0x6000000+rid)]
        row['arithmetic_sites']=[[i.offset-b.header_size,i.opcode.name]
          for i in b.instructions if i.opcode.name in ARITH or i.opcode.name.startswith('conv.')]
        sig=b.local_var_sig_tok
        if sig and sig.value:
            row['locals_signature']=p.net.mdtables.StandAloneSig.rows[(sig.value&0xffffff)-1].Signature.value.hex()
    # Every matching virtual declaration, including unselected types: exposes slot
    # alternatives without guessing which one executes. Ancestry remains explicit.
    owners={mm.row_index:n for n,t in enumerate(p.net.mdtables.TypeDef,1) for mm in t.MethodList}
    out['slot_candidates']={}
    for slot in [0xabb,0x28bc,0x28e9,0x125b,0x1871,0x144a]:
        base=p.net.mdtables.MethodDef.rows[slot-1]
        out['slot_candidates'][hex(0x6000000+slot)]=[
          {'method':hex(0x6000000+rid),'owner':hex(0x2000000+owners[rid]),
           'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
          for rid,r in enumerate(p.net.mdtables.MethodDef,1)
          if r.Flags.mdVirtual and r.Name.value==base.Name.value and r.Signature.value==base.Signature.value]
    p.close()
    return clean(out)

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
