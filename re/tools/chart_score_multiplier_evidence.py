#!/usr/bin/env python3
"""E34 read-only original-CIL extraction. No game, chart or score simulator.
Uses unchanged E04/E05 collector; dnfile0.18.0/pefile2024.8.26/dncil1.0.2.
"""
import json,sys,zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as prior
METHODS=[0x1bc1,0x1bc4,0x1bf1,0x1bf2,0x1c04,0x1387,0x9e4,0x2d7d,
 0x28f5,0x28a2,0x2237,0x2c63,0x2c64,0x223b,0x2258,0xaaf,0x1140,0xab7,0x156b,0x2232,0xad2]
WATCH={0x4000ff2,0x4000fe5}
CALLS={0x6001bc1,0x6001bf1,0x6001bf2,0x6000ab7,0x6001bc4}
def extract(path):
 prior.METHODS=METHODS;j=prior.extract(path);j['expedition']=34
 with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
 ts=p.net.mdtables;j['operand_sites']=[];j['direct_callers']=[];decoded=zero=0
 for rid,r in enumerate(ts.MethodDef,1):
  if not r.Rva:zero+=1;continue
  b=read_method_body_from_bytes(p.get_data(r.Rva));decoded+=1
  for i in b.instructions:
   v=getattr(i.operand,'value',None);off=i.offset-b.header_size;op=i.opcode.name
   if v in WATCH:j['operand_sites'].append([hex(0x6000000+rid),off,op,hex(v)])
   if v in CALLS and op in ['call','callvirt','ldftn','ldvirtftn']:j['direct_callers'].append([hex(0x6000000+rid),off,op,hex(v)])
 j['scan']={'decoded':decoded,'zero_rva':zero,'errors':0}
 j['limits']+=['Only two operand fields and five helper-call tokens scanned across EXE; no whole-project semantic survey',
 'Static conditional predictions, no original execution or import',
 'Difficulty key plaintext not independently decoded; original key-selection constants retained',
 'Owner-reported totals do not establish original chart bytes or component event identity']
 p.close();return j
if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
