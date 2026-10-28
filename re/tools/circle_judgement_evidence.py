#!/usr/bin/env python3
"""E12 read-only original CIL/metadata evidence. No judgement or timeout execution.
Requires dnfile0.18.0, pefile2024.8.26, dncil1.0.2; unchanged E04/E05 collector.
Usage: python re/tools/circle_judgement_evidence.py osubackup.zip > judgement.json
"""
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_timing_evidence as previous

METHODS = [0x1884,0x28d3,0x28e4,0x28e3,0x653,0x654,0x24b2,0x24a9,
 0x24bb,0x24b4,0x45f7,0x24b3,0x45f6,0x24d0,0x24d1,0x28be,0x125a,
 0x28b3,0x28b0,0x28bb,0x3e56,0x3e4e,0x28c2,0x28c3,0x28c4,0x2246,
 0x229c,0x291a,0x48df,0x45eb,0x45ec,0x1883,0x249b,0x651,0x652,
 0x48de,0x113e,0x1141,0x226b,0x2269,0x28e2,0x24c7,0xad9,0x2295,
 0x2258,0x2241,0x28d4,0x28d5]
FIELDS = {0x4000000+n for n in [0x1527,0x152a,0x2523,0x2524,0x152f,
 0x0adc,0x0add,0x0ade,0x17fd,0x17ff,0x17f8,0x17f6,0x17f7,0x1804,
 0x2a90,0x13d8,0x13f0,0x2358,0x17ef]}
CALLS = {0x6000000+n for n in [0x1884,0x28d3,0x28e3,0x28e4,0x24a9,
 0x24b2,0x28c2,0x28c4,0x291a,0x229c]}
CHAINS = {0x94c:[0x24a9,0x24b2,0x24bb,0x24b4,0x24b3],
 0xe8:[0x28d3,0x28e3,0x28e4,0x28bb,0x125a,0x28d4,0x28d5]}

def extract(path):
    previous.METHODS=METHODS
    out=previous.extract(path);out['expedition']=12
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
            tid=t.Extends.row_index if t.Extends.table.name=='TypeDef'else 0
        matches={}
        for rid in slots:
            s=ts.MethodDef.rows[rid-1]
            matches[hex(0x6000000+rid)]=[{'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
              'new_slot':r.Flags.mdNewSlot,'abstract':r.Flags.mdAbstract}
              for i,r in enumerate(ts.MethodDef,1)if owners[i]in chain and r.Flags.mdVirtual
              and r.Name.value==s.Name.value and r.Signature.value==s.Signature.value]
        out['ordinary_dispatch'][hex(0x2000000+root)]={
          'chain':[hex(0x2000000+i)for i in chain], 'slots':matches,
          'declarations':[{'type':hex(0x2000000+i),'methods':[{
            'method':hex(0x6000000+x.row_index),'signature':x.row.Signature.value.hex(),
            'virtual':x.row.Flags.mdVirtual,'new_slot':x.row.Flags.mdNewSlot,
            'abstract':x.row.Flags.mdAbstract}for x in ts.TypeDef.rows[i-1].MethodList]}for i in chain],
          'method_impls':[{'class':hex(0x2000000+r.Class.row_index),
             'body_table':r.MethodBody.table.name,'body_rid':r.MethodBody.row_index,
             'declaration_table':r.MethodDeclaration.table.name,
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
    out['judgement_field_sites']=sites;out['boundary_direct_callers']=calls
    out['scan']={'decoded_bodies':decoded,'zero_rva':zero,'decode_errors':0}
    for tid in [0x488,0xb57]:
        t=ts.TypeDef.rows[tid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][hex(0x2000000+tid)]={'name':str(t.TypeName),
          'extends_name':str(t.Extends.row.TypeNamespace)+'.'+str(t.Extends.row.TypeName),
          'fields':[{'token':hex(0x4000000+f.row_index),'name':str(f.row.Name),
             'signature':f.row.Signature.value.hex(),'literal':f.row.Flags.fdLiteral}for f in t.FieldList],
          'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
            for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
    out['result_interface']={'field':'0x400152a','enum':'0x2000488',
        'delegate':'0x200055d','invoke':'0x600291a',
        'invoke_signature':ts.MethodDef.rows[0x291a-1].Signature.value.hex()}
    spec=ts.MethodSpec.rows[0x10d-1];target=spec.Method.row;owner=target.Class.row
    out['subscription_methodspec']={'token':'0x2b00010d','method_table':spec.Method.table.name,
      'method_rid':spec.Method.row_index,'owner':str(owner.TypeNamespace)+'.'+str(owner.TypeName),
      'name':str(target.Name),'signature':target.Signature.value.hex(),
      'instantiation':spec.Instantiation.value.hex()}
    out['limits'].extend(['No original Windows/CLR, input, timeout or surrogate execution',
      'Circle raw result and manager-local flagged result are distinct',
      'Positive numeric labels grounded in manager conversion; negative miss alias is reconstructed',
      'Presentation/resource helpers not reconstructed; normal return needed for result handoff',
      'Full scoring/accuracy/health/combo/effects and nonordinary objects outside scope',
      'Managed access inventory excludes reflection/external/native mutation'])
    p.close();return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
