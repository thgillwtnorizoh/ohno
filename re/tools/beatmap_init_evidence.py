#!/usr/bin/env python3
"""Expedition 04 read-only metadata and CIL xref collector; no game/parser execution.
Usage: python re/tools/beatmap_init_evidence.py /path/osubackup.zip > evidence.json
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2. All offsets code-relative.
"""
import hashlib
import json
import sys
import zipfile
from importlib.metadata import version
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes

METHODS = [0x222e,0x2232,0x2237,0x223b,0x223c,0x2258,0x226b,0x2c63,0x2c64,
 0x707,0x708,0x70a,0x70d,0x1ecb,0xa9c,0xabb,0x1ee1,0x20cd,0x28a2,0x28bc,
 0x20d4,0x426d,0x186f,0x1870,0x1871,0x1872,0x426e,0x426f,0x4270,
 0x1bc1,0x1c05,0x28f5,0x28f7,0x28ef,0x28f0,0x28f1,0x28d2,0x28e9,0x28ea,
 0x20dc,0x424e,0x1d6d,0x45eb,0x45ec,0x142d,0x1433,0x3201,0x5ea,
 0x249b,0x1883,0x17c2,0x38e1,0x715,0x71c,0x71d,0x23f0,0x23f7,0x23f8,
 0x23fb,0x4ab9,0x4abf,0x4abe,0x144a,0x28f6,0x1c0b,0x1bed]

def sha(b): return hashlib.sha256(b).hexdigest()

def extract(path):
    with zipfile.ZipFile(path) as z: raw=z.read('osu!.exe')
    p=dnfile.dnPE(data=raw);tables=p.net.mdtables
    owners={m.row_index:(n,t) for n,t in enumerate(tables.TypeDef,1) for m in t.MethodList}
    fieldowners={f.row_index:n for n,t in enumerate(tables.TypeDef,1) for f in t.FieldList}
    out={'archive_sha256':sha(open(path,'rb').read()),'exe_sha256':sha(raw),
         'tools':{n:version(n) for n in ['dnfile','pefile','dncil']},
         'offset_basis':'first CIL opcode; headers excluded; EH already code-relative',
         'methods':{},'types':{},'fields':{},'external_refs':{},'enums':{},
         'limits':['Static sites only, no original execution','No transitive/virtual reachability proof',
                   'Type hierarchy/slot candidates require reading branches and arguments']}
    def typeref(index):
        return None if index.row is None else hex({'TypeDef':0x2000000,'TypeRef':0x1000000,'TypeSpec':0x1b000000}[index.table.name]+index.row_index)
    def save_type(rid):
        token=hex(0x2000000+rid)
        if token in out['types']:return
        t=tables.TypeDef.rows[rid-1]
        out['types'][token]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),'extends':typeref(t.Extends)}
        if t.Extends.table and t.Extends.table.name=='TypeDef':save_type(t.Extends.row_index)
    for rid in METHODS:
        m=tables.MethodDef.rows[rid-1];owner,t=owners[rid];save_type(owner)
        row={'name':str(m.Name),'owner':hex(0x2000000+owner),'signature':m.Signature.value.hex(),
             'rva':hex(m.Rva),'virtual':m.Flags.mdVirtual,'new_slot':m.Flags.mdNewSlot,'abstract':m.Flags.mdAbstract}
        out['methods'][hex(0x6000000+rid)]=row
        if not m.Rva:continue
        b=read_method_body_from_bytes(p.get_data(m.Rva));h=b.header_size
        row.update(file_offset=p.get_offset_from_rva(m.Rva),header_bytes=h,body_bytes=b.size,
                   code_bytes=b.code_size,body_sha256=sha(p.get_data(m.Rva,b.size)),
                   token_sites=[],constants=[],branches=[],returns=[],exception_regions=[])
        boundaries={i.offset-h for i in b.instructions}
        for i in b.instructions:
            off=i.offset-h;op=i.opcode.name;a=i.operand
            if hasattr(a,'value'):
                v=a.value;row['token_sites'].append([off,op,hex(v)])
                if v>>24==4:
                    f=tables.Field.rows[(v&0xffffff)-1];out['fields'][hex(v)]={
                      'owner':hex(0x2000000+fieldowners[v&0xffffff]),'signature':f.Signature.value.hex()}
                if v>>24==10:
                    r=tables.MemberRef.rows[(v&0xffffff)-1];c=r.Class.row
                    out['external_refs'][hex(v)]={'name':str(getattr(c,'TypeNamespace',''))+'.'+str(getattr(c,'TypeName','<TypeSpec>'))+'::'+str(r.Name),'signature':r.Signature.value.hex()}
            if op.startswith('ldc.'):row['constants'].append([off,op,a])
            if op=='switch' or op.startswith(('brtrue','brfalse','beq','bne','bge','bgt','ble','blt','leave')) or op in ['br','br.s']:
                dest=[v-h for v in a] if op=='switch' else [a-h]
                assert all(v in boundaries for v in dest),(rid,off,dest)
                row['branches'].append([off,op,dest])
            if op=='ret':row['returns'].append(off)
        row['exception_regions']=[{'kind':e.exception_type,'try_start':e.try_start,'try_end':e.try_end,
         'handler_start':e.handler_start,'handler_end':e.handler_end} for e in b.exception_handlers]
    for rid in [0xb24,0x746,0xb58]:
        t=tables.TypeDef.rows[rid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+rid)]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),'values':{
          str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
          for c in tables.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in ids}}
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True))
