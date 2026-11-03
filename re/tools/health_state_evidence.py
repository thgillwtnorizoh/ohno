#!/usr/bin/env python3
"""E035 read-only original CIL/metadata extraction, not an HP simulator.
Uses unchanged E04/E05 readers: dnfile0.18.0/pefile2024.8.26/dncil1.0.2.
"""
import hashlib
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as prior

METHODS = [0x113e,0x1141,0x651,0xa9c,0xaaf,0xab4,0xab6,0x1142,0x1150,
 0xace,0x2258,0x223b,0x2963,0x2964,0x2965,0x2966,0x2967,0x2968,0x2969,
 0x2972,0x2975,0x2979,0x297a,0x297b,0x297c,0x297d,0xac4,0xac6,0xac7,0xac8,
 0x1146,0x114a,0xad2,0xad0,0x226b,0x2275,0x22c5,0x226f,0x2271,0x22b1,
 0x2243,0x28de,0x28dc,0x396e,0x24c2,0xe7d,0x2f05,0x3b25,0x2290,
 0x226c,0x227d,0xacb,0xacc,0xab7,0x28b3,0x28b0,0x125a,0x28be,
 0x1ef8,0x2241,0x226e]
WATCH = {0x4000000+n for n in [0x1852,0x1853,0x1854,0x1855,0x1856,0x1857,
 0x1858,0x603,0x604,0x13c4,0x136f,0x1374,0x13a2,0x13a7,0x1b97]}
HELPERS = {0x6000000+n for n in [0x2964,0x2965,0x2966,0x2967,0x2968,0x2969,
 0x2972,0x2979,0x297a,0x297b,0x297c,0x297d,0x1150,0x1146,0xac7,0x226c,0x227d]}
SLOTS = [0xab4,0xab6,0xac6,0xac7,0xac8,0x125a,0x1150,0x2972,0x2975,
 0x2979,0x297a,0x297b,0x297d,0x227d,0xacb,0xad0]

def extract(path):
    assert len(METHODS) == len(set(METHODS))
    prior.METHODS = METHODS
    out = prior.extract(path)
    out['expedition'] = 35
    with zipfile.ZipFile(path) as z:
        p = dnfile.dnPE(data=z.read('osu!.exe'))
    ts = p.net.mdtables
    owners = {m.row_index:n for n,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    fieldowners = {f.row_index:n for n,t in enumerate(ts.TypeDef,1) for f in t.FieldList}
    for token in sorted(WATCH):
        rid = token & 0xffffff
        f = ts.Field.rows[rid-1]
        out['fields'][hex(token)] = {'owner':hex(0x2000000+fieldowners[rid]),
            'signature':f.Signature.value.hex(),'name':str(f.Name),'static':f.Flags.fdStatic}
    out['comparison_interfaces'] = []
    for i in ts.InterfaceImpl:
        if i.Class.row_index == 0x4c3:
            r = i.Interface.row
            out['comparison_interfaces'].append({'table':i.Interface.table.name,
                'rid':i.Interface.row_index,'signature':r.Signature.value.hex() if hasattr(r,'Signature') else None,
                'name':str(getattr(r,'TypeNamespace',''))+'.'+str(getattr(r,'TypeName',''))})
    out['unselected_writer_methods'] = {}
    out['direct_field_accesses'] = []
    out['direct_helper_xrefs'] = []
    decoded = zero = 0
    for rid, m in enumerate(ts.MethodDef,1):
        if not m.Rva:
            zero += 1
            continue
        b = read_method_body_from_bytes(p.get_data(m.Rva))
        decoded += 1
        for i in b.instructions:
            value = getattr(i.operand,'value',None)
            off, op = i.offset-b.header_size, i.opcode.name
            if value in WATCH:
                out['direct_field_accesses'].append([hex(0x6000000+rid),off,op,hex(value)])
                if op in ['stfld','stsfld'] and rid not in METHODS:
                    out['unselected_writer_methods'][hex(0x6000000+rid)] = {
                        'owner':hex(0x2000000+owners[rid]),'signature':m.Signature.value.hex(),
                        'name':str(m.Name),'rva':hex(m.Rva),
                        'body_sha256':hashlib.sha256(p.get_data(m.Rva,b.size)).hexdigest()}

            if value in HELPERS and op in ['call','callvirt','ldftn','ldvirtftn']:
                out['direct_helper_xrefs'].append([hex(0x6000000+rid),off,op,hex(value)])
    out['scan'] = {'decoded':decoded,'zero_rva':zero,'errors':0,
                   'watched_fields':len(WATCH),'helper_tokens':len(HELPERS)}
    out['slot_candidates'] = {}
    for rid in SLOTS:
        base = ts.MethodDef.rows[rid-1]
        out['slot_candidates'][hex(0x6000000+rid)] = [
            {'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
             'new_slot':m.Flags.mdNewSlot,'abstract':m.Flags.mdAbstract}
            for i,m in enumerate(ts.MethodDef,1)
            if m.Flags.mdVirtual and m.Name.value==base.Name.value
            and m.Signature.value==base.Signature.value]
    out['ordinary_resolution'] = {}
    for tid, slots in [(0x253,[0xab4,0xab6,0xac6,0xac7,0xac8,0x1150,0xacb,0xad0]),
                       (0xe8,[0x125a]),(0x56a,[0x2972,0x2975,0x2979,0x297a,0x297b,0x297d]),
                       (0x47b,[0x227d])]:
        for slot in slots:
            base = ts.MethodDef.rows[slot-1]
            chain=[]; resolved=None; cursor=tid
            while cursor:
                chain.append(hex(0x2000000+cursor))
                t = ts.TypeDef.rows[cursor-1]
                found=[m.row_index for m in t.MethodList
                    if m.row.Flags.mdVirtual and m.row.Name.value==base.Name.value
                    and m.row.Signature.value==base.Signature.value]
                if found:
                    assert len(found)==1
                    resolved=hex(0x6000000+found[0]);break
                cursor=t.Extends.row_index if t.Extends.table and t.Extends.table.name=='TypeDef' else None
            out['ordinary_resolution'][f'{hex(0x2000000+tid)}:{hex(0x6000000+slot)}']={
                'chain':chain,'method':resolved}
    for rid in [0xb57]:
        t=ts.TypeDef.rows[rid-1];fields={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+rid)]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),
            'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
            for c in ts.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in fields}}
    out['limits'] += [
        'Only15 health/coefficient/eligibility fields and17 helper tokens scanned across EXE; direct syntactic inventory only',
        'No health/gameplay engine, calibration-loop simulation, original execution, runtime observation or reconstruction change',
        'Global/handler/manager references and original opaque helpers remain separate; no atomic snapshot or callback purity assertion',
        'Scoring discrepancy explicitly deferred; no E33/E34 conclusions revised',
        'Earlier full E035 specification not retrievable; current owner five priorities and standing contract bound this report']
    p.close()
    return out

if __name__ == '__main__':
    print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
