#!/usr/bin/env python3
"""E33 focused read-only original metadata/CIL sites, no accounting/game execution.
Uses E04/E05 collector and their pinned dnfile/pefile/dncil dependencies.
"""
import json,sys,zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as prior
METHODS=[0x114a,0xad2,0xa9c,0xab7,0xace,0xaa5,0x40f0,0x40f1,0x249b,
 0x2b3e,0x2b42,0x2b44,0x2b45,0x2b4b,0x2b4c,0x2b62,
 0x1bc4,0x156b,0x2232,0x2234,0x2d7d,0x2273,0x2274,0x1dd2,0x1dd3,
 0x2276,0x2277,0x4608,0x2e1c,0x2e1f,0x2258]
def extract(path):
 prior.METHODS=METHODS;j=prior.extract(path);j['expedition']=33
 with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
 ts=p.net.mdtables;owners={mr.row_index:i for i,t in enumerate(ts.TypeDef,1)for mr in t.MethodList}
 j['scoped_field_sites']={}
 for token,row in j['methods'].items():
  for off,op,v in row.get('token_sites',[]):
   if v.startswith('0x4'):j['scoped_field_sites'].setdefault(v,[]).append([token,off,op])
 j['slots']={}
 for rid in [0xab7,0xad2,0x2b42,0x2b44,0x2b4b,0x2b4c,0x2b62]:
  r=ts.MethodDef.rows[rid-1]
  j['slots'][hex(0x6000000+rid)]=[{'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),'new_slot':s.Flags.mdNewSlot}for i,s in enumerate(ts.MethodDef,1)if s.Flags.mdVirtual and s.Name.value==r.Name.value and s.Signature.value==r.Signature.value]
 # One narrowly justified whole-EXE syntactic xref pass closes mutable multiplier
 # writers, accounting-enable writers, and getter consumers; no general survey.
 j['target_xrefs']=[];decoded=zero=0
 for rid,r in enumerate(ts.MethodDef,1):
  if not r.Rva:zero+=1;continue
  b=read_method_body_from_bytes(p.get_data(r.Rva));decoded+=1
  for i in b.instructions:
   v=getattr(i.operand,'value',None);op=i.opcode.name
   if (v in [0x4000605,0x400153b]and op in ['stfld','stsfld'])or(v in [0x6002b42,0x6002b44,0x6002b4b]and op in ['call','callvirt']):
    j['target_xrefs'].append([hex(0x6000000+rid),i.offset-b.header_size,op,hex(v)])
 j['xref_coverage']={'decoded':decoded,'zero_rva':zero,'errors':0}
 j['limits']+=['No original runtime or surrogate ledger execution','Field sites limited to selected methods; only stated narrow target xrefs exhaustive for direct managed instructions','Map-density inputs and flagged result transformations require their original operands; no invented defaults','No ScoreV2/HP/online/persistence implementation']
 p.close();return j
if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
