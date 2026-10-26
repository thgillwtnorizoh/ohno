#!/usr/bin/env python3
"""Read-only Expedition 03 CFG/xref extraction. No game execution or simulation.
Usage: python re/tools/request_guard_evidence.py /path/osubackup.zip > evidence.json
Dependencies: dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2.
All instruction offsets exclude method headers. EH offsets from dncil already
use code-start coordinates (do NOT subtract the header from these).
"""
import hashlib
import json
import sys
import zipfile
from importlib.metadata import version
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes


def sha(data): return hashlib.sha256(data).hexdigest()

def conditional(op):
    return op.startswith(('brtrue','brfalse','beq','bne','bge','bgt','ble','blt'))

def extract(path):
    result={'archive_sha256':sha(open(path,'rb').read()),
            'tools':{n:version(n) for n in ['dnfile','pefile','dncil']},
            'offset_basis':'code start; not method header', 'methods':{}}
    with zipfile.ZipFile(path) as z:
        raw=z.read('osu!.exe');result['exe_sha256']=sha(raw);p=dnfile.dnPE(data=raw)
    fields={}; callees={}
    for token in [0x060046ad,0x060046e6]:
        m=p.net.mdtables.MethodDef.rows[(token&0xffffff)-1]
        body=read_method_body_from_bytes(p.get_data(m.Rva)); h=body.header_size
        ins=body.instructions; leaders={0}; edges={}; branch_sites=[]; refs=[]; constants=[]; rets=[]
        for n,i in enumerate(ins):
            off=i.offset-h;op=i.opcode.name;a=i.operand
            nxt=ins[n+1].offset-h if n+1<len(ins) else None
            if op=='switch':
                dest=[x-h for x in a];dest.append(nxt)
                branch_sites.append({'offset':off,'opcode':op,'taken':dest[:-1],'fallthrough':nxt})
            elif conditional(op):
                dest=[a-h,nxt];branch_sites.append({'offset':off,'opcode':op,'taken':a-h,'fallthrough':nxt})
            elif op in ['br','br.s','leave','leave.s']: dest=[a-h]
            elif op in ['ret','throw','rethrow','endfinally']: dest=[]
            else: dest=[] if nxt is None else [nxt]
            edges[off]=dest
            if op=='switch' or conditional(op) or op in ['br','br.s','leave','leave.s','ret','endfinally']:
                leaders.update(dest)
                if nxt is not None: leaders.add(nxt)
            if op=='ret': rets.append(off)
            if hasattr(a,'value'):
                v=a.value;refs.append([off,op,hex(v)])
                if v>>24==4:
                    r=p.net.mdtables.Field.rows[(v&0xffffff)-1]
                    fields[hex(v)]={'name':str(r.Name),'signature':r.Signature.value.hex()}
                elif v>>24==6:
                    r=p.net.mdtables.MethodDef.rows[(v&0xffffff)-1]
                    callees[hex(v)]={'signature':r.Signature.value.hex(),'virtual':r.Flags.mdVirtual}
            if op.startswith('ldc.'):
                constants.append([off,op,a])
        offsets={i.offset-h for i in ins}
        assert all(v in offsets for dest in edges.values() for v in dest)
        sorted_leaders=sorted(leaders);blocks=[]
        for n,start in enumerate(sorted_leaders):
            end=sorted_leaders[n+1] if n+1<len(sorted_leaders) else body.code_size
            members=[i.offset-h for i in ins if start<=i.offset-h<end]
            if members: blocks.append({'start':start,'end':end,'last':members[-1],'successors':edges[members[-1]]})
        result['methods'][hex(token)]={
            'rva':hex(m.Rva),'file_offset':p.get_offset_from_rva(m.Rva),
            'body_bytes':body.size,'code_bytes':body.code_size,'header_bytes':h,
            'body_sha256':sha(p.get_data(m.Rva,body.size)),
            'signature':m.Signature.value.hex(),'instruction_count':len(ins),
            'blocks':blocks,'branches':branch_sites,'return_offsets':rets,
            'token_sites':refs,'constants':constants,
            'exception_regions':[{'kind':e.exception_type,'try_start':e.try_start,'try_end':e.try_end,
                                  'handler_start':e.handler_start,'handler_end':e.handler_end} for e in body.exception_handlers]}
    result['fields']=fields;result['callee_signatures']=callees;p.close();return result

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),indent=2,sort_keys=True))
