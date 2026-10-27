#!/usr/bin/env python3
"""Expedition 08: read-only original metadata/CIL facts, never a visual model.
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2 and unchanged E04/E05 tools.
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS = [0x48df,0x48e0,0x45eb,0x45ec,0x45ed,0x45ee,0x45ef,0x45f6,
 0x249b,0x24b3,0x24ce,0x24cf,0x24c9,0x24a8,
 0x142d,0x143a,0x351a,0x351b,0x351c,0x144a,
 0x2995,0x299c,0x2d71,0x2d74,0x4510,0x4728,0x4736,
 0x4518,0x4519,0x451a,0x31f5,0x4738,0x4737,0x4734,
 0x28d2,0x2ce9,0x2ce6,0x2cda,0x2cdb,0x473b,0x2f4a,
 0x28f5,0x289a,0x289b,0x2cf0,0x2287,0x2288,0x28e7,
 0x46a6,0x46fe,0x4416,0x2bf4,0x44b7,0x44c8,0x44c9,
 0x153d,0x1541,0x949,0x1545]
SLOTS = [0x24b3,0x24c9,0x45ed,0x45ee,0x45ef,0x4738,0x4737,0x4416,0x31f5]
WATCH_CALLS = {0x6004519,0x600451a,0x60031f5,0x60028e7,0x6002ce6}

def extract(path):
    previous.METHODS = METHODS
    out = previous.extract(path)
    out['expedition'] = 8
    with zipfile.ZipFile(path) as z:
        p = dnfile.dnPE(data=z.read('osu!.exe'))
    ts = p.net.mdtables
    owners = {m.row_index:n for n,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    for rid in SLOTS:
        s = ts.MethodDef.rows[rid-1]
        out['slot_candidates'][hex(0x6000000+rid)] = [
          {'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
           'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
          for i,r in enumerate(ts.MethodDef,1) if r.Flags.mdVirtual
          and r.Name.value == s.Name.value and r.Signature.value == s.Signature.value]
    out['ancestry_declarations'] = {}
    for root in [0x6f6,0x94c,0x47b,0x920,0x2df]:
        tid = root
        while tid:
            t = ts.TypeDef.rows[tid-1]
            out['ancestry_declarations'][hex(0x2000000+tid)] = {
              'extends_table':t.Extends.table.name,'extends_rid':t.Extends.row_index,
              'methods':[{'method':hex(0x6000000+m.row_index),'name':str(m.row.Name),
                'signature':m.row.Signature.value.hex(),'virtual':m.row.Flags.mdVirtual,
                'new_slot':m.row.Flags.mdNewSlot} for m in t.MethodList]}
            tid = t.Extends.row_index if t.Extends.table.name == 'TypeDef' else 0
    for rid in [0x21a,0xb57]:
        t = ts.TypeDef.rows[rid-1]; ids = {f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+rid)] = {'name':str(t.TypeNamespace)+'.'+str(t.TypeName),
          'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
            for c in ts.Constant if c.Parent.table.name == 'Field' and c.Parent.row_index in ids}}
    s = ts.MethodSpec.rows[5]
    out['registration_method_spec'] = {'token':'0x2b000006',
      'method':hex(0x6000000+s.Method.row_index),'table':s.Method.table.name,
      'instantiation':s.Instantiation.value.hex()}
    calls = []; decoded = zero = 0
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:
            zero += 1
            continue
        b = read_method_body_from_bytes(p.get_data(m.Rva)); decoded += 1
        for i in b.instructions:
            v = getattr(i.operand,'value',None)
            if v in WATCH_CALLS and i.opcode.name in {'call','callvirt','ldftn','ldvirtftn'}:
                calls.append([hex(0x6000000+rid),i.offset-b.header_size,i.opcode.name,hex(v)])
    out['boundary_direct_callers'] = calls
    out['scan'] = {'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    out['limits'].extend(['No game, CLR, surrogate animation, pixel or frame-time execution',
      'Selected property sites are not a complete inventory of hit-driven mutations',
      'M31f5 is abstract: concrete receiver and first visible frame remain unresolved',
      'Non-repeating finite Fade/Scale projection only; other transform branches retained as dependencies'])
    p.close()
    return out

if __name__ == '__main__':
    print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
