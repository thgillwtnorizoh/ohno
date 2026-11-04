#!/usr/bin/env python3
"""E037 deterministic read-only pinned metadata/CIL extraction. No original execution."""
import hashlib
import json
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import beatmap_init_evidence as base

ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
METHODS=[0x1884,0x24bb,0x24ba,0x24b9,0x24bd,0x249b,0x3202,0x1be6,
 0x48df,0x45eb,0x45ec,0x28f7,0x44a2,0x44a1,0x28a1,0x2f1a,
 0x3b31,0x3b32,0x3b36,0x3b15,0x3b13,0x3b14,0x3b40,0x3bb6,
 0x13fb,0x13fd,0x13ff,0x1403,0x1404,0x1405,0x1406,0x1407,0x1409,0x140a,0x140b,
 0x313e,0x3146,0x3197,0x3117,0x3195,0x31a2,0x3196,
 0x2fee,0x2fd7,0x2fdd,0x2fde,0x279e,0x2ffd,0x36b6,0x38e2,
 0x28ff,0x1bc1,0x0b73,0x0b72,0x3b3c,0x3b08,0x3b19,0x3b21,0x31a5,
 0x30b1,0x30bf,0x30fc,0x30f6,0x30c0,0x30c5,0x30c6,0x30e1,0x30f0,
 0x27a1,0x36a0,0x36b0,0x36b2,0x36b3,0x3c05,0x24b4,0x05fa,
 0x3d65,0x3d69,0x3df2,0x3b68,0x426e,0x30c7]
FIELDS={0x4000000+x for x in [0x1534,0x1535,0x1536,0x1537,0x1538,0x1539,
 0x2526,0x0661,0x0662,0x0663,0x0665,0x234b,0x234e,0x2351,0x2374,0x2375,0x2376,
 0x0bc0,0x0bc1,0x0bc4,0x0bc5,0x2b36,0x2b37,0x2b38,0x2b39,0x2b3a]}
CALLS={0x6000000+x for x in [0x24bb,0x24ba,0x24b9,0x3b31,0x3b32,0x3b36,0x2fee,0x140b]}
SLOTS=[0x24bb,0x24bd,0x1be6,0x2fd7,0x2fdd,0x2fde,0x2fee]

def extract(path):
    assert hashlib.sha256(open(path,'rb').read()).hexdigest()==ARCHIVE
    with zipfile.ZipFile(path) as z:raw=z.read('osu!.exe')
    assert hashlib.sha256(raw).hexdigest()==EXE
    base.METHODS=METHODS
    out=base.extract(path);out['expedition']=37
    p=dnfile.dnPE(data=raw);ts=p.net.mdtables
    owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    out['field_sites']=[];out['direct_callers']=[];out['writer_bodies']={}
    decoded=0;errors=[]
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:continue
        try:b=read_method_body_from_bytes(p.get_data(m.Rva))
        except Exception as e:errors.append([rid,str(e)]);continue
        decoded+=1;writes=False
        for i in b.instructions:
            op=i.opcode.name;a=i.operand;off=i.offset-b.header_size
            if hasattr(a,'value'):
                if a.value in FIELDS:
                    out['field_sites'].append([hex(0x6000000+rid),off,op,hex(a.value)])
                    if op.startswith('st'):writes=True
                if a.value in CALLS:out['direct_callers'].append([hex(0x6000000+rid),off,op,hex(a.value)])
        if writes:out['writer_bodies'][hex(0x6000000+rid)]={
          'owner':hex(0x2000000+owners[rid]),'signature':m.Signature.value.hex(),
          'body_sha256':hashlib.sha256(p.get_data(m.Rva,b.size)).hexdigest()}
        if rid in METHODS:
            r=out['methods'][hex(0x6000000+rid)];r['operations']=[];r['argument_local_sites']=[]
            for i in b.instructions:
                op=i.opcode.name;a=i.operand;off=i.offset-b.header_size
                if op.startswith(('ldarg','starg','ldloc','stloc')):
                    r['argument_local_sites'].append([off,op,getattr(a,'index',None)])
                if op in ('add','sub','mul','div','and','or','ceq','cgt','cgt.un','conv.r4','conv.r8','conv.i4','ldnull'):
                    r['operations'].append([off,op])
    out['scan']={'decoded':decoded,'zero_rva':sum(not x.Rva for x in ts.MethodDef),'errors':errors}
    out['slots']={}
    for slot in SLOTS:
        m=ts.MethodDef.rows[slot-1]
        out['slots'][hex(0x6000000+slot)]=[
          {'method':hex(0x6000000+i),'owner':hex(0x2000000+owners[i]),'virtual':r.Flags.mdVirtual,'new_slot':r.Flags.mdNewSlot}
          for i,r in enumerate(ts.MethodDef,1) if str(r.Name)==str(m.Name) and r.Signature.value==m.Signature.value]
    out['native_imports']={hex(0x6000000+x.MemberForwarded.row_index):
       {'module':str(x.ImportScope.row.Name),'entry':str(x.ImportName)}
       for x in ts.ImplMap if x.MemberForwarded.row_index in METHODS}
    # Original enum literals and field declarations, not inferred sound names.
    out['sample_enums']={}
    for rid in [0x987,0xb0f,0x137,0x35f,0x910]:
        t=ts.TypeDef.rows[rid-1];ids={x.row_index for x in t.FieldList}
        out['sample_enums'][hex(0x2000000+rid)]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),
          'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
           for c in ts.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in ids}}
    out['limits']=['Original static only; no game/CLR/native/audio execution',
      'Direct inventories exclude reflection/native/external mutation',
      'Encrypted string-key contents unresolved unless independently established',
      'Resource handles and scheduler timing are runtime operands; no audible result claimed',
      'E20 all17 NOT RUN; historical628 QA checkpoint not rerun by this extractor']
    p.close();return out

if __name__=='__main__':
    result=json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2)+'\n'
    if len(sys.argv)>2:
        with open(sys.argv[2],'w',encoding='utf-8',newline='\n') as f:f.write(result)
    else:print(result,end='')
