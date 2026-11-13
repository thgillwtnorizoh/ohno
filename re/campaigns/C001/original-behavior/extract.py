#!/usr/bin/env python3
"""Pinned C001 metadata/relationship recovery. Never invokes original CLR code.

Published output deliberately excludes instruction listings and binary contents.
Branch inventories establish audit obligations, not behavioral equivalence.
"""
import hashlib
import json
from pathlib import Path
import sys
import zipfile
from importlib.metadata import version

ROOT = Path(__file__).resolve().parents[4]
sys.path.insert(0, str(ROOT / 're/tools'))
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
from hitsound_resource_evidence import StaticStrings

EXE = '6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
ARCHIVE = '7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
GROUPS = {
    'entry': [0x223b,0x2258,0x1bc1,0x28a2,0x28f5,0x28f7,0x28f8,0x28f9],
    'syntax_defaults': [0x1387,0x1bb2,0x1bb3,0x1bb4,0x1bbb,0x09e3,0x09e4,0x09e5],
    'timing': [0x28ff,0x0b73,0x0b75,0x0b78,0x0b79,0x1be3,0x1be4,0x1be6,0x28ab,0x28be],
    'objects_registration': [0x1870,0x1871,0x1872,0x1873,0x28d2,0x28ef,0x28f0,0x28f1,0x2902,0x24c2,0x38e1],
    'preparation': [0x28db,0x28e9,0x28ea,0x28eb,0x28ec,0x28ee,0x28b0,0x28b2,0x28b3],
    'ordinary_factory': [0x651,0x652,0x48de,0x48df,0x48e0,0x48e1,0x45eb,0x45ec,0x142d,0x1433,0x144a,0x1441,0x3201],
    'alternate_modes': [0x2232,0x0abb,0x113e,0x1141,0x1ecb,0x1ee1,0x20cd,0x20d4,0x20d6,0x20d7,0x426e,0x426f,0x4270,0x4261,0x4263,0x18e2,0x17d8,0x1c91],
    'resources': [0x1c04,0x1c05,0x1c06,0x1c0b,0x1c0d,0x1bee,0x1bed,0x3b3c],
}

def sha(data): return hashlib.sha256(data).hexdigest()
def token(kind, rid): return hex((kind << 24) | rid)

def extract(archive):
    data = Path(archive).read_bytes()
    assert sha(data) == ARCHIVE, 'reference archive mismatch'
    with zipfile.ZipFile(archive) as z: raw = z.read('osu!.exe')
    assert sha(raw) == EXE, 'reference executable mismatch'
    pe = dnfile.dnPE(data=raw); ts = pe.net.mdtables
    dec = StaticStrings(pe)
    owners = {x.row_index:n for n,t in enumerate(ts.TypeDef,1) for x in t.MethodList}
    fowners = {x.row_index:n for n,t in enumerate(ts.TypeDef,1) for x in t.FieldList}
    roots = set(x for g in GROUPS.values() for x in g)
    bodies = {}
    def body(rid):
        if rid not in bodies:
            m = ts.MethodDef.rows[rid-1]
            bodies[rid] = read_method_body_from_bytes(pe.get_data(m.Rva)) if m.Rva else None
        return bodies[rid]
    # Explicitly bounded dependency inventory, not a transitive reachability claim.
    selected = set(roots)
    for rid in sorted(roots):
        b = body(rid)
        if b:
            selected.update(i.operand.value & 0xffffff for i in b.instructions
                if i.opcode.name in ('call','callvirt','newobj','ldftn','ldvirtftn')
                and hasattr(i.operand,'value') and i.operand.value >> 24 == 6
                and i.operand.value != 0x60049b0)
    out = {'campaign':'C001','archive_sha256':ARCHIVE,'exe_sha256':EXE,
        'offset_basis':'first CIL opcode; headers excluded; exception offsets already code-relative',
        'tools':{x:version(x) for x in ('dnfile','dncil','pefile')},
        'groups':{k:[token(6,x) for x in v] for k,v in GROUPS.items()},
        'methods':{},'fields':{},'external_refs':{},'types':{},'strings':{},
        'string_sites':[],'property_dispatch':[],'enums':{},'culture_field_writers':[],
        'string_decoder':dec.provenance,
        'limits':['CONFIRMED STATIC metadata and independently reduced literal strings only',
            'One-hop direct dependencies; unresolved virtual targets and external calls remain',
            'No original method, CLR, native module, audio or UI executed',
            'All original-runtime cases UNVERIFIED ORIGINAL-RUNTIME']}
    used_fields = {0x2c2b}
    def save_type(rid):
        key=token(2,rid)
        if key in out['types']:return
        t=ts.TypeDef.rows[rid-1];ex=t.Extends
        out['types'][key]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),
            'extends':None if ex.row is None else {'table':ex.table.name,'row':ex.row_index}}
        if ex.row is not None and ex.table.name=='TypeDef':save_type(ex.row_index)
    for rid in sorted(selected):
        m=ts.MethodDef.rows[rid-1];save_type(owners[rid]);b=body(rid)
        row={'name':str(m.Name),'owner':token(2,owners[rid]),'signature':m.Signature.value.hex(),
            'rva':m.Rva,'abstract':m.Flags.mdAbstract,'virtual':m.Flags.mdVirtual,
            'new_slot':m.Flags.mdNewSlot,'token_sites':[],'constants':[],'branches':[],
            'operations':[],'argument_local_sites':[],'exception_regions':[]}
        out['methods'][token(6,rid)]=row
        if not b:continue
        row.update(body_sha256=sha(pe.get_data(m.Rva,b.size)),body_bytes=b.size,
            code_bytes=b.code_size,file_offset=pe.get_offset_from_rva(m.Rva),header_bytes=b.header_size)
        boundaries={i.offset-b.header_size for i in b.instructions}
        for j,i in enumerate(b.instructions):
            op=i.opcode.name;a=i.operand;off=i.offset-b.header_size
            if hasattr(a,'value'):
                v=a.value;row['token_sites'].append([off,op,hex(v)])
                if v>>24==4:used_fields.add(v&0xffffff)
                elif v>>24==10:
                    r=ts.MemberRef.rows[(v&0xffffff)-1];c=r.Class.row
                    out['external_refs'][hex(v)]={'name':str(getattr(c,'TypeNamespace',''))+'.'+str(getattr(c,'TypeName','<TypeSpec>'))+'::'+str(r.Name),'signature':r.Signature.value.hex()}
                if v==0x60049b0 and j and b.instructions[j-1].opcode.name=='ldc.i4':
                    ident=b.instructions[j-1].operand;decoded=dec.decode(ident)
                    out['strings'][str(ident)]=decoded
                    out['string_sites'].append([token(6,rid),off,ident])
                    # Only the directly adjacent equality-and-branch pattern is admitted.
                    if rid in (0x1bc1,0x28f7) and j+2<len(b.instructions):
                        eq,nxt=b.instructions[j+1:j+3]
                        if eq.opcode.name=='call' and getattr(eq.operand,'value',0)==0xa00012a and nxt.opcode.name.startswith(('brtrue','beq')):
                            out['property_dispatch'].append({'method':token(6,rid),'literal':decoded.get('text'),
                                'literal_site':off,'branch_site':nxt.offset-b.header_size,'handler':nxt.operand-b.header_size})
            if op.startswith('ldc.'):row['constants'].append([off,op,a])
            if op.startswith(('ldarg','starg','ldloc','stloc')):row['argument_local_sites'].append([off,op,getattr(a,'index',None)])
            if op=='switch' or op.startswith(('br','beq','bne','bge','bgt','ble','blt','leave')):
                dest=[v-b.header_size for v in a] if op=='switch' else [a-b.header_size]
                assert all(v in boundaries for v in dest),(rid,off,dest)
                row['branches'].append({'offset':off,'opcode':op,'targets':dest,'disposition':'UNRESOLVED; static site inventoried, semantic/production coverage pending'})
            if op in ('add','sub','mul','div','and','or','ceq','cgt','cgt.un','conv.r4','conv.r8','conv.i4','conv.u2','ldnull','throw','ret'):
                row['operations'].append([off,op])
        row['exception_regions']=[{'kind':e.exception_type,'try_start':e.try_start,'try_end':e.try_end,
            'handler_start':e.handler_start,'handler_end':e.handler_end,'catch_type':getattr(e.catch_type,'value',None)} for e in b.exception_handlers]
    for rid in sorted(used_fields):
        f=ts.Field.rows[rid-1];save_type(fowners[rid])
        out['fields'][token(4,rid)]={'owner':token(2,fowners[rid]),'name':str(f.Name),'signature':f.Signature.value.hex()}
    for rid in (0xb24,0x746,0xb58,0xb0f,0xb14):
        t=ts.TypeDef.rows[rid-1];ids={f.row_index for f in t.FieldList}
        out['enums'][token(2,rid)]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),
            'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
                for c in ts.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in ids}}
    errors=[];decoded=0
    for rid,m in enumerate(ts.MethodDef,1):
        if not m.Rva:continue
        try:b=body(rid)
        except Exception as e:errors.append([token(6,rid),type(e).__name__]);continue
        decoded+=1
        for j,i in enumerate(b.instructions):
            if i.opcode.name=='stsfld' and getattr(i.operand,'value',0)==0x4002c2b:
                calls=[(x.offset-b.header_size,x.opcode.name,hex(x.operand.value)) for x in b.instructions[max(0,j-5):j] if hasattr(x.operand,'value')]
                out['culture_field_writers'].append({'method':token(6,rid),'offset':i.offset-b.header_size,
                    'body_sha256':sha(pe.get_data(m.Rva,b.size)),'preceding_token_sites':calls})
    out['scan']={'decoded':decoded,'zero_rva':sum(not m.Rva for m in ts.MethodDef),'errors':errors}
    pe.close();return out

if __name__=='__main__':
    result=extract(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(result,sort_keys=True,indent=2)+'\n',encoding='utf-8')
    print('PASS C001 extraction:',len(result['methods']),'methods;',sum(len(m['branches']) for m in result['methods'].values()),'branch sites;',len(result['strings']),'literals;',result['scan'])
