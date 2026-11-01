#!/usr/bin/env python3
"""E31 read-only pinned CIL/metadata evidence, never a substitute input processor."""
import hashlib
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import spinner_result_evidence as previous

METHODS=[0x4390,0x4391,0x438f,0x43a0,0x43a1,0x43a2,0x43a4,0x43a6,
 0x43a7,0x43a8,0x43b2,0x43b5,0x43b6,0x43b7,0x43b8,0x43b4,
 0x43c3,0x43cb,0x43cc,0x467d,0x2d46,0x2d47,
 *range(0x1187,0x1194),0x11a2,0x11a3,0x11a4,0x11a9,0x11ac,0x11af,
 0x11b0,0x11b2,0x11b3,0x11b4,0x11b5,0x11b6,0x1333,0x12f6,0x20c5,0x20c2,
 0x2e93,0x2e94,0x2c54,0x2c59,0x3794,0x3795,0x379a,0x379c,0x2d48,
 0x1dff,0x1dfb,0x1dfc,0x1dfd,0x21a2,0x21a8,0x21b1,0x21b5,
 0x2405,0x23f0,0x23f9,0x0718,0x071a,0x071b,0x2237,0x223b,0x224a,0x2246,0x2248,
 0x225b,0x2261,0x2266,0x226a,0x226b,0x2285,0x228f,0x22a8,0x4abc,0x4abb,
 0x4abd,0x4ac1,0x1140,0x0aaf,0x0aab,0x0aad,0x0aae,0x113e,0x651,
 0x46ad,0x46b0,0x2295,0x229b,0x28c8,0x28c9,0x24c7,0x0ad9,0x28d3,
 0x1884,0x0ff1,0x46d7,0x46fb]
FOCUS=[0x2a7c,0x2a7f,0x2a9f,0x2a7a,0x2a7e,0x2a80,0x2a6f,
 0x2a64,0x2a65,0x2a67,0x2a83,0x2a6c,0x2a88,0x2a6e,0x2a8a,
 0x2a71,0x2a66,0x2a6d,0x2a89,0x2a68,0x2a6a,0x2a84,0x2a86,
 0x2a69,0x2a6b,0x2a85,0x2a87,0x0a72,0x0a74,0x0a82,0x0a83,
 0x0a73,0x0a70,0x0a71,0x0a84,0x0a85,0x0a86,0x2a59,0x0a79,0x0a7a]
FIELDS={0x4000000+n for n in FOCUS+[
 0x2a78,0x2a79,0x2a82,0x2a90,0x2a93,0x2a5c,0x2a7b,0x2a94,0x2a95,
 0x2a72,0x2a74,0x2a73,0x2a5a,0x2c6b,0x2c6d,0x2c73,0x2c24,0x136d,
 0x1373,0x1374,0x1376,0x137b,0x137c,0x13be,0x136a,0x13bb,0x1397,
 0x1b97,0x2358,0x1118,0x1119,0x111a,0x111c,0x111e,0x111f,0x1980,
 0x131a,0x1309,0x130a,0x130b,0x130c,0x130d,0x130e,0x130f,0x1310,
 0x1317,0x1314,0x14b2,0x07d9,0x0a7c,0x0a7b,0x0a80,0x0a7d,0x0a7e,
 0x17fd,0x1380,0x1527,0x152a,0x2523,0x252c,0x2a58,0x03ad,0x153b,
 0x2aa3,0x2aa4,0x1a9f,0x2c55,0x2c6c,0x2c6e,0x03cb,0x1b50,0x1b51]}
CALLS={0x6000000+n for n in [0x4391,0x224a,0x2266,0x1dff,0x1dfb,
 0x1dfc,0x21a8,0x21b5,0x071a,0x43a4,0x43a6,0x43a7,0x11b4,
 0x11b0,0x11a9,0x43b5,0x2295,0x43b8,0x43b7,0x228f,0x11b6,0x43c3,0x229b]}
CHAINS={0x47b:[0x224a,0x225b,0x2261,0x2266,0x226a],
 0xa10:[0x224a,0x225b,0x2261,0x2266,0x226a],
 0x4af:[0x224a,0x225b,0x2261,0x2266,0x226a],
 0xff:[0x224a,0x225b,0x2261,0x2266,0x226a],
 0x466:[0x0e1f,0x0e1c],0x253:[0x0aaf,0x0aab,0x0aad,0x0ad9],
 0x94c:[0x24c7,0x24a9]}

def extract(path):
    previous.METHODS=METHODS;previous.FIELDS=FIELDS;previous.CALLS=CALLS
    previous.CHAINS=CHAINS;previous.SPIN=FOCUS
    out=previous.extract(path);out['expedition']=31
    out['limits']=[
      'Original static metadata/CIL only; no original Windows/CLR/native or surrogate gameplay/input execution',
      'Departure, live-path selection, retained history and first event7/circle boundary only',
      'Direct managed inventories exclude reflective/native/external mutation; original helpers/callbacks can interrupt',
      'Separate reads are not proof of a race; concrete synchronous controls are identified separately',
      'CIL-relative offsets are not native instruction addresses; original strings/complete disassembly remain private',
      'E20 incomplete: all17 original-runtime cases NOT RUN — WINDOWS HOST REQUIRED']
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    out['input_type_metadata']={}
    for rid in [0x455,0xa79,0xb50,0x3fc,0x466,0x8e5,0x25f]:
        t=ts.TypeDef.rows[rid-1];ids={f.row_index for f in t.FieldList}
        out['input_type_metadata'][hex(0x2000000+rid)]={
          'namespace':str(t.TypeNamespace),'name':str(t.TypeName),
          'constants':[{'field':hex(0x4000000+c.Parent.row_index),
              'name':str(c.Parent.row.Name),
              'value':int.from_bytes(c.Value.value,'little',signed=True)}
              for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids]}
    out['focus_writer_bodies']={}
    watched={hex(0x4000000+n)for n in FOCUS}
    for tok,sites in out['entry_field_sites'].items():
        if not any(op in ['stfld','stsfld']and f in watched for off,op,f in sites):continue
        rid=int(tok,16)&0xffffff;r=ts.MethodDef.rows[rid-1]
        b=read_method_body_from_bytes(p.get_data(r.Rva))
        out['focus_writer_bodies'][tok]={'owner':hex(0x2000000+owners[rid]),
          'signature':r.Signature.value.hex(),'rva':hex(r.Rva),
          'body_sha256':hashlib.sha256(p.get_data(r.Rva,b.size)).hexdigest()}
    # Complete direct setter callers with a small argument/continuation window.
    # This is not a transcription of unrelated loading/menu/network methods.
    out['selector_caller_windows']=[]
    for tok,off,op,target in out['boundary_direct_callers']:
        if target!='0x6004391':continue
        rid=int(tok,16)&0xffffff;r=ts.MethodDef.rows[rid-1]
        b=read_method_body_from_bytes(p.get_data(r.Rva));ix=next(
          n for n,i in enumerate(b.instructions)if i.offset-b.header_size==off)
        sites=[]
        for i in b.instructions[max(0,ix-6):ix+5]:
            a=i.operand;name=i.opcode.name
            v=hex(a.value)if hasattr(a,'value')else a if name.startswith('ldc.')else None
            if name.startswith(('br','beq','bne','bge','bgt','ble','blt','leave'))and isinstance(a,int):
                v=a-b.header_size
            sites.append([i.offset-b.header_size,name,v])
        out['selector_caller_windows'].append({'method':tok,'owner':hex(0x2000000+owners[rid]),
          'signature':r.Signature.value.hex(),'offset':off,'sites':sites,
          'body_sha256':hashlib.sha256(p.get_data(r.Rva,b.size)).hexdigest()})
    # Stop the record-mask consumer before record/frame construction/persistence.
    row=out['methods']['0x600229b']
    for key in ['token_sites','constants','branches','returns','arithmetic_sites',
                'local_argument_sites','array_sites','float_constant_bits']:
        if key in row:row[key]=[x for x in row[key]if (x[0]if isinstance(x,list)else x)<0x1ae]
    row['published_sites_bounded']=[[0,0x1ae]]
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
