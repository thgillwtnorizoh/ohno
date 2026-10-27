#!/usr/bin/env python3
"""Read-only Expedition 07 original metadata/CIL evidence; no runtime model.
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2 and unchanged E04/E05 tools.
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS = [0x223b,0x2234,0x1141,0x651,0x652,0x48de,0x48df,0x48e0,
 0x45eb,0x45ec,0x142d,0x249b,0x24b2,0x24b5,0x24b6,0x24c2,0x24c6,
 0x24c9,0x3e56,0x289a,0x289b,0x28be,0x28b0,0x28b3,0x28b6,
 0x28d2,0x28da,0x28c5,0x28e1,0x28e2,0x28e3,0x28e4,0x28e5,0x2903,
 0x396e,0x1448,0x45fc,0x144b,0x144f,0x1468,0x24a5,
 0x223c,0x2268,0x2269,0x226a,0x226b,0x46a6,0x847,0x320a]
WATCH_FIELDS = {0x400153a,0x40017fd,0x4000adb,0x40017e7}
WATCH_CALLS = {0x60028e2,0x60028e1,0x60024b5}
SLOTS = [0x28e2,0x24b2,0x24b5,0x24b6,0x24c9,0x125a]

def extract(path):
    previous.METHODS = METHODS
    out = previous.extract(path)
    out['expedition'] = 7
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
    # Include declarations on the circle's complete ancestry: absence of a
    # matching preparation override must be checkable, not inferred from a name.
    out['circle_ancestry_declarations'] = {}
    tid = 0x94c
    while tid:
        t = ts.TypeDef.rows[tid-1]
        out['circle_ancestry_declarations'][hex(0x2000000+tid)] = [
          {'method':hex(0x6000000+m.row_index),'name':str(m.row.Name),
           'signature':m.row.Signature.value.hex(),'virtual':m.row.Flags.mdVirtual,
           'new_slot':m.row.Flags.mdNewSlot} for m in t.MethodList]
        tid = t.Extends.row_index if t.Extends.table.name == 'TypeDef' else 0
    out['comparison_interfaces'] = [
      {'table':r.Interface.table.name,'rid':r.Interface.row_index,
       'signature':getattr(getattr(r.Interface.row,'Signature',None),'value',b'').hex(),
       'name':str(getattr(r.Interface.row,'TypeName',''))}
      for r in ts.InterfaceImpl if r.Class.row_index == 0x4c3]
    out['search_type_specs'] = {hex(0xa000000+rid):
      ts.MemberRef.rows[rid-1].Class.row.Signature.value.hex() for rid in [0x4be,0x4c0]}
    out['field_flags'] = {hex(v):{'static':ts.Field.rows[(v&0xffffff)-1].Flags.fdStatic,
      'init_only':ts.Field.rows[(v&0xffffff)-1].Flags.fdInitOnly} for v in WATCH_FIELDS}
    hits = {}; calls = []; decoded = 0; zero = 0
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:
            zero += 1
            continue
        b = read_method_body_from_bytes(p.get_data(m.Rva))
        decoded += 1
        for ins in b.instructions:
            v = getattr(ins.operand,'value',None)
            off = ins.offset-b.header_size
            if v in WATCH_FIELDS:
                hits.setdefault(hex(0x6000000+rid),[]).append([off,ins.opcode.name,hex(v)])
            if v in WATCH_CALLS and ins.opcode.name in {'call','callvirt','ldftn','ldvirtftn'}:
                calls.append([hex(0x6000000+rid),off,ins.opcode.name,hex(v)])
    out['watched_field_sites'] = hits
    out['boundary_direct_callers'] = calls
    out['scan'] = {'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    out['limits'].extend(['Syntactic field/call inventories are not runtime traces',
      'BinarySearch library implementation and non-monotone end ordering are not simulated',
      'Preparation eligibility is not visibility, hit eligibility or measured cost'])
    p.close()
    return out

if __name__ == '__main__':
    print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
