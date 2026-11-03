#!/usr/bin/env python3
"""E036 pinned original-static extraction. Never executes original methods.

Usage: python re/tools/newcombo_preparation_evidence.py osubackup.zip [output.json]
Requires dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2.
"""
import hashlib
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import beatmap_init_evidence as base

ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
METHODS=[0x2902,0x28f7,0x28f5,0x28f6,0x28c5,0x289a,0x28a2,
 0x28ef,0x28f0,0x28f1,0x28d2,0x28aa,0x38e1,0x3e4e,0x3e56,0x3e57,
 0x113e,0x1141,0x651,0x652,0x48de,0x48df,0x48e0,0x48e1,
 0x1870,0x1871,0x1872,0x45eb,0x45ec,0x142d,0x5ea,0x3201,
 0x249b,0x1883,0x17c2,0x28e9,0x28ea,0x28eb,0x28ec,0x28ee,0x144a,0x1150,
 0x28db,0x28df,0x28e0,0x28f8,0x5ff,0x3518,0x27b7,0x27b8,0x186f,
 0x249f,0x24a0,0xe7d,0x2efd,0x2f23,0x28f4,0x41d6,0x48d4,0x223b,0x8d8]
FIELDS={0x4001805,0x4002525}
CALLS={0x6002902,0x6003e56,0x6003e57,0x60028f7,0x60028f5,0x60028f6,
 0x60028db,0x60028df,0x60028e0,0x60028f8}

def extract(path):
    rawzip=open(path,'rb').read()
    assert hashlib.sha256(rawzip).hexdigest()==ARCHIVE, 'archive pin'
    with zipfile.ZipFile(path) as z: raw=z.read('osu!.exe')
    assert hashlib.sha256(raw).hexdigest()==EXE, 'executable pin'
    p=dnfile.dnPE(data=raw); ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    field_sites=[]; callers=[]; bodies={}; errors=[]
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:continue
        try:b=read_method_body_from_bytes(p.get_data(m.Rva))
        except Exception as e:errors.append([rid,str(e)]);continue
        bodies[rid]=b
        for i in b.instructions:
            a=i.operand; op=i.opcode.name; off=i.offset-b.header_size
            if not hasattr(a,'value'):continue
            if a.value in FIELDS:field_sites.append([hex(0x6000000+rid),off,op,hex(a.value)])
            if a.value in CALLS:callers.append([hex(0x6000000+rid),off,op,hex(a.value)])
    writers={int(m,16)&0xffffff for m,o,op,f in field_sites if op in ('stfld','stsfld')}
    base.METHODS=sorted(set(METHODS)|writers)
    out=base.extract(path)
    out.update(expedition=36,field_sites=field_sites,direct_callers=callers,
       scan={'decoded':len(bodies),'zero_rva':sum(not m.Rva for m in ts.MethodDef),'errors':errors})
    for rid in base.METHODS:
        if rid not in bodies:continue
        b=bodies[rid];r=out['methods'][hex(0x6000000+rid)]
        r['argument_local_sites']=[];r['operations']=[]
        for i in b.instructions:
            off=i.offset-b.header_size;op=i.opcode.name;a=i.operand
            if op.startswith(('ldarg','starg','ldloc','stloc')):
                r['argument_local_sites'].append([off,op,getattr(a,'index',None)])
            if op in ('or','and','not','cgt.un','ceq','add','sub','shr','isinst'):
                r['operations'].append([off,op,hex(a.value) if hasattr(a,'value') else None])
    # Enumerate matching slot declarations; ordinary inheritance is checked separately.
    out['slots']={}
    for slot in [0x3e56,0x3e57,0x1870,0x1871,0x1872,0x28f6,0x28d2]:
        target=ts.MethodDef.rows[slot-1]
        out['slots'][hex(0x6000000+slot)]=[
          {'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),
           'signature':m.Signature.value.hex(),'virtual':m.Flags.mdVirtual,
           'new_slot':m.Flags.mdNewSlot}
          for i,m in enumerate(ts.MethodDef,1)
          if str(m.Name)==str(target.Name) and m.Signature.value==target.Signature.value]
    out['focus_types']={}
    for rid in [0x556,0xe8,0x94c,0x2af,0xd8,0x67f,0x81f,0x828,0x339,0x4c3,0x35a,0x9c1]:
        t=ts.TypeDef.rows[rid-1]
        out['focus_types'][hex(0x2000000+rid)]={'name':str(t.TypeName),
          'extends':hex((0x2000000 if t.Extends.table.name=='TypeDef' else 0x1000000)+t.Extends.row_index)}
    out['limits']=['Original static only; no game/CLR/runtime experiment',
      'Direct managed inventories do not exclude reflection/native/external mutation',
      'Original complete disassembly and bytes not published',
      'E035 arithmetic/retry policy unchanged; E20 all17 NOT RUN']
    p.close();return out

if __name__=='__main__':
    result=json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2)+'\n'
    if len(sys.argv)>2:
        with open(sys.argv[2],'w',encoding='utf-8',newline='\n') as f:f.write(result)
    else:print(result,end='')
