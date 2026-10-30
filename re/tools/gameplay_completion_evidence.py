#!/usr/bin/env python3
"""E27 read-only original CIL/metadata extraction, never a gameplay/exit model."""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import hashlib
import spinner_result_evidence as previous

METHODS=[0x222e,0x2237,0x223b,0x223c,0x2241,0x2243,0x2269,0x226b,0x226f,
 0x2271,0x22b1,0x2276,0x2277,0x2278,0x2285,0x2286,0xacd,0xaa5,0xa9f,
 0x2284,0x22c7,0x22c8,0x2b47,0x2b5a,0x2cee,0x3b25,0x3b24,0x3b58,
 0x3b59,0x3b0d,0x3d62,0x4385,0x4053,0x313a,0x43a0,0x46ad,0x46e6,
 0x46b0,0x46a3,0x46ef,0x46b5,0x2261,0x2262,0x2272,0x227e,0xacb,
 0x226c,0x227d,0x225b,0x225a,0x4390,0x46af,0xace,0x2258,0x28d3,
 0x28e3,0x28e4,0x653,0x654,0x229c,0x28c2,0x2f05,0x28de,0x3b52,0x2efd,0x2efe]
FOCUS=[0x1397,0x13c4,0x13bf,0x13a2,0x13a7,0x1391,0x1978,0x197c,0x1983,
 0x1b9f,0x1bb0,0x2368]
FIELDS={0x4000000+n for n in FOCUS+[0x138f,0x17fb,0x17fd,0x17fc,0x1527,
 0x13c3,0x5f3,0x136a,0x1373,0x1376,0x13d8,0x13f0,0x13f6,0x13ad,
 0x138a,0x13a4,0x13b2,0x137f,0x2358,0x235a,0x234c,0x1396,0x2c6c,
 0x2c6d,0x2c6b,0x2c6e,0x2c77,0x2c70,0x13bb,0x2c73,0x2a81,0x2c74,
 0x13f8,0x1b97,0x2523,0x2524,0x13d6,0x13d7,0x1b9e,0x247e,0x2c3a]}
CALLS={0x6000000+n for n in [0x2276,0x2277,0x2278,0x2285,0x2286,0xacd,
 0xaa5,0xa9f,0x2271,0x226f,0x22b1,0x46ad,0x46b0,0x32d0,0x2243]}
CHAINS={0x47b:[0x1ab2,0x223b,0x223c,0x2277,0x2278,0x2286,0x227e,0x2261],
 0x253:[0xacd,0xaa5,0xa9f,0xaca,0xabb],0xe8:[0x28e3,0x28e4,0x28d3],
 0x8e4:[0x3d62],0x86e:[0x3d62]}

def extract(path):
    previous.METHODS=METHODS;previous.FIELDS=FIELDS;previous.CALLS=CALLS
    previous.CHAINS=CHAINS;previous.SPIN=FOCUS
    out=previous.extract(path);out['expedition']=27
    out['limits']=[
      'Original static EXE CIL/metadata only; no Windows/CLR/gameplay or substitute exit engine execution',
      'Ordinary base Play/T0253/T00e8 completion through first mode request and conditional commit boundary',
      'Direct inventories exclude reflection/native/external mutation; callbacks and exceptional helpers are not pure',
      'Full-list endpoints, maximum end, candidate activity, record counts, q, finalized latch and pending mode are distinct',
      'Result-grade internals, complete audio/lifecycle/submission/results rendering remain outside scope',
      'E20 incomplete: all17 original-runtime cases NOT RUN — WINDOWS HOST REQUIRED']
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    for tid in [0x241,0xb4b,0x6e4,0xb22,0xb23]:
        t=ts.TypeDef.rows[tid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+tid)]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),
          'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
          for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
    out['method_specs']={}
    for rid in [0x84]:
        r=ts.MethodSpec.rows[rid-1]
        out['method_specs'][hex(0x2b000000+rid)]={'table':r.Method.table.name,
          'rid':r.Method.row_index,'instantiation':r.Instantiation.value.hex(),
          'signature':r.Method.row.Signature.value.hex()}
    out['boundary_metadata']={}
    for rid in [0x32d0,0x0dc3,0x2571]:
        r=ts.MethodDef.rows[rid-1]
        out['boundary_metadata'][hex(0x6000000+rid)]={'owner':hex(0x2000000+owners[rid]),
          'name':str(r.Name),'signature':r.Signature.value.hex()}
    out['focus_writer_bodies']={}
    focus_tokens={hex(0x4000000+n)for n in FOCUS}
    for token,sites in out['entry_field_sites'].items():
        if not any(op in ['stfld','stsfld']and field in focus_tokens for off,op,field in sites):continue
        rid=int(token,16)&0xffffff;r=ts.MethodDef.rows[rid-1]
        b=read_method_body_from_bytes(p.get_data(r.Rva))
        out['focus_writer_bodies'][token]={'owner':hex(0x2000000+owners[rid]),
          'signature':r.Signature.value.hex(),'rva':hex(r.Rva),
          'body_sha256':hashlib.sha256(p.get_data(r.Rva,b.size)).hexdigest()}
    body=read_method_body_from_bytes(p.get_data(ts.MethodDef.rows[0x28f7-1].Rva))
    selected=[i for i in body.instructions if 0xaf0<=i.offset-body.header_size<=0xb80]
    out['aux_end_parser_sites']={
      'method':'0x60028f7',
      'token_sites':[[i.offset-body.header_size,i.opcode.name,hex(i.operand.value)]for i in selected if hasattr(i.operand,'value')],
      'branches':[[i.offset-body.header_size,i.opcode.name,i.operand-body.header_size]for i in selected if i.opcode.name in ['ble.s','bge.s','br.s']],
      'arithmetic':[[i.offset-body.header_size,i.opcode.name]for i in selected if i.opcode.name in ['add','sub']]}
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
