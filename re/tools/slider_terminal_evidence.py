#!/usr/bin/env python3
"""E22 original metadata/CIL only; no terminal-checkpoint or completion model."""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_completion_evidence as prior

METHODS=[0x144a,0x145c,0x146f,0x1478,0x146d,0x146e,0x2996,0xac5,0x114a,
 0xad2,0xad0,0x40f0,0x40f1,0x226b,0x2275,0x22c5,0x2273,0x28e2,0x28e3,
 0x653,0x654,0x28e4,0x24be,0x28d3,0x1465,0x1466,0x1469,0x146c,0x3b53,
 0x229c,0x291a,0x226c,0x227d,0x297a,0x297b,0x2966,0x28b3,0x28b0,
 0x1141,0x651,0x48e0,0x142d,0x143a,0x1884,0x351d,0x1449,0x24a9,0x2282]
FIELDS=prior.FIELDS|{0x4000000+n for n in [0x153b,0x153d,0x604,0x5f2,0x5f3,
 0x5f5,0x5f6,0x5f7,0x5fa,0x13bb,0x138c,0x1970,0x1971,0x1975,0x198a,
 0x2674,0x1857,0x1858,0x136a,0x136b,0x137b,0x137c,0xada,0xbd2,0xbfc,
 0xbfe,0x2ce8,0x1867,0x186b,0x186e,0x186f]}

def extract(path):
    prior.METHODS=METHODS;prior.FIELDS=FIELDS
    out=prior.extract(path);out['expedition']=22
    out['limits']=[
      'Static original instruction/metadata evidence only; no Windows or surrogate gameplay execution',
      'One E21 two-traversal linear slider, final checkpoint and first parent-completion handoff',
      'Opaque prefix, policy, observer, resource, adjustment and lifecycle helpers retain side effects',
      'Natural fixture import/provider/policy/clock/JIT interleavings and captured pixels unresolved',
      'E20 remains preparation-only; runtime acceptance NOT RUN — WINDOWS HOST REQUIRED']
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    for rid,ranges in {
      0xad2:[(0x1ed,0x22d),(0x3ed,0x458),(0x719,0x732),
             (0x119a,0x11ef),(0x1277,0x1306)],
      0xad0:[(0,0x16)],
    }.items():
        m=p.net.mdtables.MethodDef.rows[rid-1]
        b=read_method_body_from_bytes(p.get_data(m.Rva))
        out['methods'][hex(0x6000000+rid)]['local_argument_sites']=[
          [i.offset-b.header_size,i.opcode.name,getattr(i.operand,'index',None)]
          for i in b.instructions
          if any(lo<=i.offset-b.header_size<=hi for lo,hi in ranges)
          and i.opcode.name.startswith(('ldloc','stloc','ldarg'))]
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
