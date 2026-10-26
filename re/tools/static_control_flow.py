#!/usr/bin/env python3
"""Expedition 02: read-only metadata/CIL cross-reference evidence, not a runtime.
Usage: python re/tools/static_control_flow.py /path/osubackup.zip > evidence.json
Dependencies: dnfile==0.18.0, pefile==2024.8.26, dncil==1.0.2.
Offsets are relative to first CIL opcode (method headers excluded).
Calls are syntactic sites, not proof of reachability or virtual dispatch targets.
"""
import hashlib
import json
import sys
import zipfile
from importlib.metadata import version
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes

SELECTED = [0x2c49, 0x23b9, 0x23bc, 0x23be, 0x4668, 0x468e,
            0x469e, 0x46a1, 0x46a3, 0x46ad, 0x46ae, 0x46af,
            0x46b0, 0x46b5, 0x46ef, 0x46e6, 0x4710, 0x4711,
            0x222e, 0x2237, 0x226b, 0x0715, 0x4ab9, 0x4abf]
CALLS = {'call', 'callvirt', 'newobj', 'ldftn', 'ldvirtftn', 'jmp', 'calli'}

def sha(b):
    return hashlib.sha256(b).hexdigest()

def run(path):
    result = {'archive_sha256': sha(open(path, 'rb').read()),
              'tools': {n: version(n) for n in ['dnfile','pefile','dncil']},
              'offset_basis': 'CIL code start; excludes method header',
              'inventory': [], 'selected_methods': {}, 'limitations':
              ['No execution', 'No virtual-target or delegate reachability proof',
               'No reflection/native/generated-code completeness guarantee']}
    with zipfile.ZipFile(path) as z:
        for name in ['osu!.exe','osu!gameplay.dll','osu!ui.dll','osu!auth.dll','osu!seasonal.dll']:
            data = z.read(name)
            pe = dnfile.dnPE(data=data)
            inv = {'file': name, 'sha256': sha(data), 'managed': bool(pe.net)}
            result['inventory'].append(inv)
            if not pe.net:
                pe.close()
                continue
            tables = pe.net.mdtables
            owners = {m.row_index: (i,t) for i,t in enumerate(tables.TypeDef,1) for m in t.MethodList}
            def label(token):
                rid = token & 0xffffff
                kind = token >> 24
                if kind == 6:
                    t = owners[rid][1]
                    return str(t.TypeNamespace)+'.'+str(t.TypeName)+'::'+str(tables.MethodDef.rows[rid-1].Name)
                if kind == 10:
                    r=tables.MemberRef.rows[rid-1]; t=r.Class.row
                    return str(getattr(t,'TypeNamespace',''))+'.'+str(getattr(t,'TypeName','<TypeSpec>'))+'::'+str(r.Name)
                if kind == 43:
                    return 'MethodSpec('+label((0x06000000 if tables.MethodSpec.rows[rid-1].Method.table.name=='MethodDef' else 0x0a000000)+tables.MethodSpec.rows[rid-1].Method.row_index)+')'
                return hex(token)
            inv.update(types=len(tables.TypeDef.rows), methods=len(tables.MethodDef.rows),
                       entry_token=hex(pe.net.struct.EntryPointTokenOrRva),
                       resources=[{'name':str(r.name),'bytes':r.size} for r in pe.net.resources])
            if name != 'osu!.exe':
                inv['method_names']=[str(m.Name) for m in tables.MethodDef]
                pe.close()
                continue
            parsed=0; errors=[]; invalid=[]; call_count=0; constructors={}; callsites=[]
            for rid,m in enumerate(tables.MethodDef,1):
                if not m.Rva: continue
                try: body=read_method_body_from_bytes(pe.get_data(m.Rva))
                except Exception as e:
                    errors.append({'token':hex(0x6000000+rid),'error':type(e).__name__});continue
                parsed+=1
                boundaries={i.offset for i in body.instructions}
                calls=[]; fields=[]; switches=[]
                for i in body.instructions:
                    op=i.opcode.name; arg=i.operand; off=i.offset-body.header_size
                    if op in CALLS:
                        call_count+=1
                        if hasattr(arg,'value'):
                            row={'offset':off,'opcode':op,'target':hex(arg.value),'label':label(arg.value)}
                            calls.append(row);callsites.append((rid,arg.value,op))
                    if op.endswith('fld') or op.endswith('flda'):
                        fields.append({'offset':off,'opcode':op,'field':hex(arg.value)})
                    if op=='switch':
                        switches.append({'offset':off,'targets':[v-body.header_size for v in arg]})
                    if op=='switch' or op.startswith(('br','beq','bne','bge','bgt','ble','blt','leave')):
                        targets=arg if op=='switch' else [arg]
                        for v in targets:
                            if v not in boundaries:invalid.append([hex(0x6000000+rid),off,v-body.header_size])
                if rid in SELECTED:
                    result['selected_methods'][hex(0x6000000+rid)]={
                        'label':label(0x6000000+rid),'owner':hex(0x2000000+owners[rid][0]),
                        'base_type':hex({'TypeDef':0x02000000,'TypeRef':0x01000000,'TypeSpec':0x1b000000}[owners[rid][1].Extends.table.name]+owners[rid][1].Extends.row_index),
                        'rva':hex(m.Rva),'file_offset':pe.get_offset_from_rva(m.Rva),
                        'body_sha256':sha(pe.get_data(m.Rva,body.size)),
                        'signature_blob':m.Signature.value.hex(),'header_bytes':body.header_size,
                        'body_bytes':body.size,'virtual':m.Flags.mdVirtual,'new_slot':m.Flags.mdNewSlot,
                        'exception_regions':len(body.exception_handlers),
                        'calls':[[c['offset'],c['opcode'],c['target']] for c in calls],
                        'external_labels':{c['target']:c['label'] for c in calls if c['target'].startswith('0xa')},
                        'field_accesses':[[f['offset'],f['opcode'],f['field']] for f in fields],
                        'switches':switches}
            result['scan']={'decoded_bodies':parsed,'zero_rva_methods':sum(not m.Rva for m in tables.MethodDef),
                            'decode_errors':errors,'invalid_branch_targets':invalid,'call_like_sites':call_count}
            result['commit_callers']=[{'caller':hex(0x6000000+r),'opcode':op} for r,t,op in callsites if t==0x60046b0]
            mode=next((i,t) for i,t in enumerate(tables.TypeDef,1) if str(t.TypeNamespace)=='osu' and str(t.TypeName)=='OsuModes')
            result['modes']={'type_token':hex(0x2000000+mode[0]),'values':{
                str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
                for c in tables.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in {f.row_index for f in mode[1].FieldList}}}
            pe.close()
    return result

if __name__ == '__main__':
    if len(sys.argv) == 4 and sys.argv[2] == '--inspect-token':
        # Local inspection only; never commit the resulting original disassembly.
        with zipfile.ZipFile(sys.argv[1]) as archive:
            pe = dnfile.dnPE(data=archive.read('osu!.exe'))
            token = int(sys.argv[3], 16)
            if token >> 24 != 6 or not 1 <= (token & 0xffffff) <= len(pe.net.mdtables.MethodDef.rows):
                raise ValueError('Expected a valid MethodDef token')
            method = pe.net.mdtables.MethodDef.rows[(token & 0xffffff)-1]
            body = read_method_body_from_bytes(pe.get_data(method.Rva))
            for instruction in body.instructions:
                operand = instruction.operand
                if isinstance(operand, list): operand = [v-body.header_size for v in operand]
                elif isinstance(operand, int) and instruction.opcode.name.startswith(('br','beq','bne','bge','bgt','ble','blt','leave')):
                    operand -= body.header_size
                print(hex(instruction.offset-body.header_size), instruction.opcode.name, operand)
            pe.close()
    else:
        print(json.dumps(run(sys.argv[1]),sort_keys=True))
