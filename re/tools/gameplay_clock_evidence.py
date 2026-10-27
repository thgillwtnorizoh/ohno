#!/usr/bin/env python3
"""E10 read-only managed metadata/CIL inventory. Never executes a game or clock model.
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2 and unchanged E04/E05 tools.
Usage: python re/tools/gameplay_clock_evidence.py osubackup.zip > clock.json
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS = [0x3b08,0x3b0c,0x3b0d,0x3b22,0x3b24,0x3b25,0x3b37,0x3b38,
 0x3b39,0x3b3a,0x3b44,0x3b45,0x3b48,0x3b49,0x3b4f,0x3b50,0x3b51,
 0x3b52,0x3b59,0x3b5a,0x3b5e,0x3d51,0x3d52,0x3d5c,0x3d5d,
 0x3d5f,0x3d60,0x3d61,0x3d62,0x3d63,0x3d64,0x3d65,0x3d66,0x3d67,0x3d68,
 0x404f,0x4050,0x4051,0x4052,0x4053,0x4054,0x4057,0x4058,0x4059,
 0x4378,0x437c,0x437d,0x437e,0x437f,0x4384,0x4385,0x4388,0x4389,0x438a,0x438b,0x438c,0x438d,
 0x3112,0x3117,0x312a,0x312b,0x313a,0x313d,0x313e,0x3143,0x3146,0x314b,0x314e,
 0x2237,0x223e,0x2240,0x224c,0x2262,0x2269,0x226b,0x2287,0x2288,0x28e2,0x28e3,
 0x38a0,0x497d,0x43a0,0x43a1,0x469e,0x46d7,0x46fb,0x46fe,
 0x4233,0x4241,0x4246,0x4247,0x4248,0x4249,0x3965,0x396d,
 0x46a1,0x46a6,0x46a7,0x47cd,0x4736,0x4519,0x2d7c,0x3df2]
FIELDS = {0x4000000+n for n in [0x2358,0x2352,0x235b,0x235c,0x2359,0x235a,
 0x234c,0x234d,0x2353,0x235d,0x235e,0x2367,0x2368,0x2365,0x2366,
 0x237d,0x237c,0x2382,0x2389,0x2371,0x136a,0x2c97,0x2c71,0x2c3a,0x2c3d,0x2c3f]}
CALLS = {0x6000000+n for n in [0x3b24,0x3b25,0x3b3a,0x3b38,0x4241]}

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=10
    with zipfile.ZipFile(path) as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    fieldowners={f.row_index:i for i,t in enumerate(ts.TypeDef,1) for f in t.FieldList}
    for v in FIELDS:
        f=ts.Field.rows[(v&0xffffff)-1]
        out['fields'][hex(v)]={'owner':hex(0x2000000+fieldowners[v&0xffffff]),'signature':f.Signature.value.hex()}
    sites={};calls=[];decoded=zero=0
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:zero+=1;continue
        b=read_method_body_from_bytes(p.get_data(m.Rva));decoded+=1
        for i in b.instructions:
            v=getattr(i.operand,'value',None);off=i.offset-b.header_size
            if v in FIELDS:sites.setdefault(hex(0x6000000+rid),[]).append([off,i.opcode.name,hex(v)])
            if v in CALLS and i.opcode.name in {'call','callvirt','newobj','ldftn','ldvirtftn'}:
                calls.append([hex(0x6000000+rid),off,i.opcode.name,hex(v)])
    out['clock_field_sites']=sites;out['clock_direct_callers']=calls
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    # Resolve matching slots only within an explicit receiver ancestry, not by name alone.
    out['dispatch']={}
    for root,slots in [(0x803,[0x3d5c,0x3d5f,0x3d60,0x3d61,0x3d62,0x3d63,0x3d65,0x3d66,0x3d67,0x3d68]),
                       (0x8c1,[0x4246,0x4248,0x4249])]:
        family={root};pending=[root]
        while pending:
            parent=pending.pop()
            for i,t in enumerate(ts.TypeDef,1):
                if t.Extends.table and t.Extends.table.name=='TypeDef' and t.Extends.row_index==parent and i not in family:
                    family.add(i);pending.append(i)
        rows=[]
        for i in sorted(family):
            t=ts.TypeDef.rows[i-1]
            rows.append({'type':hex(0x2000000+i),'abstract':t.Flags.tdAbstract,
                'extends_table':t.Extends.table.name,'extends_rid':t.Extends.row_index})
        matches={}
        for rid in slots:
            s=ts.MethodDef.rows[rid-1]
            matches[hex(0x6000000+rid)]=[{'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
                'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
                for i,r in enumerate(ts.MethodDef,1) if owners[i] in family and r.Flags.mdVirtual
                and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value]
        out['dispatch'][hex(0x2000000+root)]={'family':rows,'slots':matches,
          'method_impls':[{'class':hex(0x2000000+r.Class.row_index),'body_table':r.MethodBody.table.name,
             'body_rid':r.MethodBody.row_index,'declaration_table':r.MethodDeclaration.table.name,
             'declaration_rid':r.MethodDeclaration.row_index} for r in ts.MethodImpl if r.Class.row_index in family]}
    out['native_imports']=[{'method':hex(0x6000000+r.MemberForwarded.row_index),
        'name':str(r.ImportName),'module':str(r.ImportScope.row.Name),
        'signature':r.MemberForwarded.row.Signature.value.hex()}
        for r in ts.ImplMap if r.MemberForwarded.table.name=='MethodDef' and r.MemberForwarded.row_index in METHODS]
    for rid in [0x6e4,0x71c,0xb57]:
        t=ts.TypeDef.rows[rid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+rid)]={'name':str(t.TypeName),'values':{
            str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
            for c in ts.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in ids}}
    out['limits'].extend(['No original CLR, audio driver, executable or clock surrogate execution',
        'Direct managed writer inventory excludes reflection, native and external mutation',
        'Host callback instruction order is not a measurement of scheduler cadence or audio latency',
        'Offset-setting user-facing names and some side-effect helpers remain unresolved',
        'No heard audio position, displayed pixels, frame cost or judgement-clock equivalence established'])
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
