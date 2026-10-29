#!/usr/bin/env python3
"""Consolidate existing E10-E19 evidence only; no executable decoding/execution."""
import hashlib
import json
from pathlib import Path
import sys
PACKAGE = Path(__file__).resolve().parent
RE = PACKAGE.parents[1]
PLAN = {
 'clock': [0x3b24,0x2269,0x43a0,0x43a7],
 'circle': [0x2295,0xff1,0x28c9,0x24c7,0xad9,0x1884,0x28d3,0x28e4],
 'import': [0x144a,0x145c],
 'tracking': [0x2275,0x28e2,0xac5,0x1464,0x351d,0x146f,0x146d,0x146e],
 'combo_completion': [0x114a,0xad2,0x40f1,0x1465],
 'failure': [0x2979,0x297a,0x297b,0x226c,0x227d,0xacb,0xacc],
 'order': [0x226b],
}
# Narrow large bodies to the existing report's pilot boundaries, not new excavation.
RANGES = {0x226b:[(0x920,0x9e0),(0xa46,0xad1)],
 0xad2:[(0x1ed,0x2a0),(0x426,0x458),(0x100a,0x1011),(0x119a,0x1306),(0x13bb,0x140a)]}
def encoded(x): return (json.dumps(x,sort_keys=True,indent=2,allow_nan=False)+'\n').encode()
def build():
    history={};sources={}
    for n in range(10,20):
        f=next((RE/'evidence'/str(n)).glob('*.json'))
        j=json.loads(f.read_text())
        for t,r in j['methods'].items():
            if 'body_sha256' not in r:continue
            if t in history:assert history[t]['body_sha256']==r['body_sha256'],t
            history[t]=r;sources[t]=f.relative_to(RE).as_posix()
    rows=[]
    for group,ids in PLAN.items():
        for rid in ids:
            t=hex(0x6000000+rid);r=history[t]
            keep=lambda off:rid not in RANGES or any(a<=off<b for a,b in RANGES[rid])
            rows.append({'probe_id':f'{group}-M{rid:04x}','group':group,'method_token':f'0x{0x6000000+rid:08x}',
              'owner_token':r['owner'],'signature':r['signature'],'body_sha256':r['body_sha256'],
              'code_bytes':r['code_bytes'],'source_evidence':sources[t],
              'token_sites':[x for x in r['token_sites']if keep(x[0])],
              'branches':[x for x in r['branches']if keep(x[0])],
              'returns':r['returns']})
    return {'purpose':'DEBUGGER BINDING PLAN ONLY; no original runtime observations',
      'offset_basis':'CIL code-relative, hexadecimal in operator notes; JSON offsets decimal; never native addresses',
      'reference_exe_sha256':'6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d',
      'probes':rows}
if __name__=='__main__':sys.stdout.buffer.write(encoded(build()))
