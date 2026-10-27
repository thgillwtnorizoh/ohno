#!/usr/bin/env python3
"""E11 read-only original metadata/CIL evidence. No input, judge or clock execution.
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2 and unchanged E04/E05 tools.
Usage: python re/tools/circle_input_evidence.py osubackup.zip > circle-input.json
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS = [0x1332,0x1333,0x20c2,0x20c3,0x20c5,0x2c54,0x2e93,0x2e94,
 0x1191,0x1192,0x1187,0x1188,0x1189,0x118a,0x11b2,0x11b3,0x11b4,
 0x43a0,0x43a6,0x43a7,0x4390,0x438f,0xaab,0xaaf,0xad9,
 0x113e,0x1140,0x1141,0x651,0x652,0x48de,0x48df,0x45eb,0x45ec,0x1883,0x249b,
 0x11a9,0x11ac,0x43b4,0x43b5,0x43b6,0x43b8,0x43ca,0x2d45,0x2d46,0x2d48,0x2d49,0x2d4c,
 0x2246,0x223b,0x223c,0x2237,0x2262,0x2269,0x226b,0x2295,
 0x289a,0x28c8,0x28c9,0x28d3,0x28e2,0x24c7,0x3e4e,0x38e1,0x1884,0x24a9,
 0x28be,0x125a,0x28b3,0x28b0,0x45fc,0x469e,0x46d7,0x3b25,0x2c3c,0x2c3d]
FIELDS = {0x4000000+n for n in [0x2358,0x2c3a,0xa77,0x13df,0x2a64,0x2a65,
 0x2a59,0x2a6e,0x2a8a,0x2a71,0x2a79,0x1527,0x152a,0x153b,0x0ade,0x0adc,
 0x0adb,0x17fd,0x13d8,0x1373,0x1396,0x1a9f,0x0a82,0x0a72,0x0a83,0x0a74]}
CALLS = {0x6000000+n for n in [0x2246,0x2295,0x43b5,0x1333,0x28c9,0x28d3,0x24a9,0x24c7,0xad9]}
CHAINS = {0x94c:[0x24a9,0x24c7,0x24b6],0x253:[0xad9,0xaab,0xaaf],
          0xe8:[0x125a,0x28c9,0x28d3]}

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=11
    with zipfile.ZipFile(path)as z:p=dnfile.dnPE(data=z.read('osu!.exe'))
    ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
    fowners={f.row_index:i for i,t in enumerate(ts.TypeDef,1)for f in t.FieldList}
    for v in FIELDS:
        f=ts.Field.rows[(v&0xffffff)-1]
        out['fields'][hex(v)]={'owner':hex(0x2000000+fowners[v&0xffffff]),
          'signature':f.Signature.value.hex(),'static':f.Flags.fdStatic}
    out['ordinary_dispatch']={}
    for root,slots in CHAINS.items():
        chain=[];tid=root
        while tid:
            chain.append(tid);t=ts.TypeDef.rows[tid-1]
            tid=t.Extends.row_index if t.Extends.table.name=='TypeDef' else 0
        declarations=[]
        for tid in chain:
            t=ts.TypeDef.rows[tid-1]
            declarations.append({'type':hex(0x2000000+tid),'extends_table':t.Extends.table.name,
                'extends_rid':t.Extends.row_index,'methods':[{'method':hex(0x6000000+x.row_index),
                  'name':str(x.row.Name),'signature':x.row.Signature.value.hex(),
                  'virtual':x.row.Flags.mdVirtual,'new_slot':x.row.Flags.mdNewSlot,
                  'abstract':x.row.Flags.mdAbstract}for x in t.MethodList]})
        matches={}
        for rid in slots:
            s=ts.MethodDef.rows[rid-1]
            matches[hex(0x6000000+rid)]=[{'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
                'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
                for i,r in enumerate(ts.MethodDef,1)if owners[i]in chain and r.Flags.mdVirtual
                and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value]
        out['ordinary_dispatch'][hex(0x2000000+root)]={'declarations':declarations,'slots':matches,
          'method_impls':[{'class':hex(0x2000000+r.Class.row_index),'body_table':r.MethodBody.table.name,
             'body_rid':r.MethodBody.row_index,'declaration_table':r.MethodDeclaration.table.name,
             'declaration_rid':r.MethodDeclaration.row_index}for r in ts.MethodImpl if r.Class.row_index in chain]}
    sites={};calls=[];decoded=zero=0
    for rid,r in enumerate(ts.MethodDef,1):
        if not r.Rva:zero+=1;continue
        b=read_method_body_from_bytes(p.get_data(r.Rva));decoded+=1
        for i in b.instructions:
            v=getattr(i.operand,'value',None);off=i.offset-b.header_size
            if v in FIELDS:sites.setdefault(hex(0x6000000+rid),[]).append([off,i.opcode.name,hex(v)])
            if v in CALLS and i.opcode.name in {'call','callvirt','newobj','ldftn','ldvirtftn'}:
                calls.append([hex(0x6000000+rid),off,i.opcode.name,hex(v)])
    out['input_field_sites']=sites;out['boundary_direct_callers']=calls
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    out['native_imports']=[{'method':hex(0x6000000+r.MemberForwarded.row_index),
        'name':str(r.ImportName),'module':str(r.ImportScope.row.Name),
        'signature':r.MemberForwarded.row.Signature.value.hex()}
        for r in ts.ImplMap if r.MemberForwarded.table.name=='MethodDef'and r.MemberForwarded.row_index in METHODS]
    for tid in [0xb49,0xb4a,0xb57,0xa7a,0x69d,0x569]:
        t=ts.TypeDef.rows[tid-1];ids={f.row_index for f in t.FieldList}
        vals={str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
            for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}
        if tid==0xa7a:vals={k:v for k,v in vals.items()if k in ['Z','X']}
        out['enums'][hex(0x2000000+tid)]={'name':str(t.TypeName),'values':vals}
    out['limits'].extend(['No original Windows, CLR/native API, input injection, judge or substitute execution',
       'Static input route is conditional on polling/priority/Play gates and normally returning opaque helpers',
       'No event timestamp is passed by the traced parameterless queued delegate/null-argument dispatcher',
       'Direct managed field inventory excludes reflection and external/native mutation',
       'Full result classification, scoring/effects and alternative input/replay routes are outside scope'])
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
