#!/usr/bin/env python3
"""E23 read-only original field provenance/metadata/CIL; no scoring model."""
import hashlib
import json
import logging
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_entry_evidence as collector

METHODS=[0x1465,0x28d3,0x2258,0x223b,0x229d,0xace,0x226e,0x2266,0x229e,
 0x229f,0x2e1c,0x2e1d,0x2e1e,0x2e1f,0x2b3d,0x2b3e,0x2b3f,0x2b40,
 0x2b4b,0x2b4c,0x2b5a,0x2b5c,0x2b62,0x3c54,0x3c55,0x3c56,0x3c57,
 0x3c58,0x1d5f,0x1d60,0x88d,0x3c4f,0x35c7,0x4550,0x2234,0x23f1,
 0x4aba,0x3756,0x3758,0xad2,0x1ad1,0x4390,0x2274,0x2232,0x1bc1,0x1bf2,0x2237]
FIELDS={0x4000000+n for n in [0x198e,0x13c3,0x5f3,0x2045,0x2046,0x2047,
 0x1971,0x1970,0x23f5,0x23fb,0x23f9,0x23fa,0xf57,0xf58,0xf59,0xf5a,
 0xcc6,0x2a7f,0x2a7c,0x1371,0xff2,0x1527,0x152a,0xbf1,0xbf8]}
CALLS={0x6000000+n for n in [0x2e1f,0x2e1c,0x2e1d,0x2258,0x226e,0x229d,
 0x3c57,0x3c58,0x1d5f,0x3c54,0x2b3e,0x2b62]}

def extract(path):
    collector.METHODS=METHODS;collector.FIELDS=FIELDS;collector.CALLS=CALLS
    collector.CHAINS={0x47b:[0x229d],0x4af:[0x229d],0xa10:[0x229d],
      0x3e5:[0x3c55,0x3c56,0x3c58],0x253:[0xad2],0x2af:[0x24a9],
      0x59e:[0x2b4b,0x2b62],0x934:[0x2b4b,0x2b62]}
    out=collector.extract(path);out['expedition']=23
    out['limits']=[
      'Original static evidence only; actual Windows policy selection unobserved',
      'Direct EXE field inventory excludes reflection/native/external runtime mutation',
      'One accumulator presence/creation/restore path, not complete scoring or serialization',
      'Global/handler/entry/policy/head reads are not one immutable snapshot',
      'E20 remains preparation-only, 17 runtime cases NOT RUN — WINDOWS HOST REQUIRED']
    with zipfile.ZipFile(path)as z:
        p=dnfile.dnPE(data=z.read('osu!.exe'));ts=p.net.mdtables
        field=ts.Field.rows[0x198e-1];decl=ts.TypeDef.rows[0x59e-1]
        owners={m.row_index:i for i,t in enumerate(ts.TypeDef,1)for m in t.MethodList}
        def flags(x):return sorted(k for k,v in vars(x).items()if isinstance(v,bool)and v)
        def attrs(table,rid):return [
          {'constructor_table':x.Type.table.name,'constructor_rid':x.Type.row_index,
           'value':x.Value.value.hex()}
          for x in ts.CustomAttribute if x.Parent.table.name==table and x.Parent.row_index==rid]
        out['policy_identity']={
          'field_token':'0x400198e','name':str(field.Name),'owner':'0x200059e',
          'signature':field.Signature.value.hex(),'flags':flags(field.Flags),
          'custom_attributes':attrs('Field',0x198e),
          'record_name':str(decl.TypeName),'record_namespace':str(decl.TypeNamespace),
          'record_flags':flags(decl.Flags),
          'reference_type':'0x20007e4','reference_name':str(ts.TypeDef.rows[0x7e4-1].TypeName),
          'reference_namespace':str(ts.TypeDef.rows[0x7e4-1].TypeNamespace),
          'reference_flags':flags(ts.TypeDef.rows[0x7e4-1].Flags),
          'self_memberrefs':[hex(0xa000000+i)for i,r in enumerate(ts.MemberRef,1)
            if r.Name.value==field.Name.value
            and str(getattr(r.Class.row,'TypeName',''))==str(decl.TypeName)
            and str(getattr(r.Class.row,'TypeNamespace',''))==str(decl.TypeNamespace)],
          'properties_returning_reference':[{'token':hex(0x17000000+i),'name':str(q.Name)}
            for i,q in enumerate(ts.Property,1)if '129f90'in q.Type.value.hex()]}
        descendants=[]
        for i,t in enumerate(ts.TypeDef,1):
            cur=i;chain=[]
            while cur:
                chain.append(cur);e=ts.TypeDef.rows[cur-1].Extends
                cur=e.row_index if e.table.name=='TypeDef'else 0
            if 0x7e4 in chain:
                descendants.append({'token':hex(0x2000000+i),'name':str(t.TypeName),
                  'chain':[hex(0x2000000+x)for x in chain],
                  'abstract':t.Flags.tdAbstract,'sealed':t.Flags.tdSealed})
        out['policy_family']=descendants
        out['policy_access_inventory']=[]
        out['global_record_writers']=[]
        for token,sites in out['entry_field_sites'].items():
            rid=int(token,16)&0xffffff;m=ts.MethodDef.rows[rid-1]
            selected=[s for s in sites if s[2]in ['0x400198e','0x4002045']]
            ident={'method':token,'owner':hex(0x2000000+owners[rid]),
              'signature':m.Signature.value.hex()}
            if selected:out['policy_access_inventory'].append({**ident,'sites':selected})
            stores=[s for s in sites if s[2]=='0x40013c3'and s[1]in ['stfld','stsfld']]
            if stores:out['global_record_writers'].append({**ident,'sites':stores})
        tid=0xb57;t=ts.TypeDef.rows[tid-1];ids={f.row_index for f in t.FieldList}
        out['original_mod_enum']={'token':hex(0x2000000+tid),'name':str(t.TypeName),
          'namespace':str(t.TypeNamespace),'values':{
            str(c.Parent.row.Name):{'field':hex(0x4000000+c.Parent.row_index),
              'value':int.from_bytes(c.Value.value,'little',signed=True)}
            for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
        out['policy_metadata_members']={}
        for rid in [0x4d5,0x163c,0x163d,0x1498,0x695,0x697]:
            r=ts.MemberRef.rows[rid-1]
            out['policy_metadata_members'][hex(0xa000000+rid)]={
              'name':str(r.Name),'signature':r.Signature.value.hex(),
              'class_table':r.Class.table.name,'class_rid':r.Class.row_index}
        for rid in METHODS:
            m=ts.MethodDef.rows[rid-1]
            if not m.Rva:continue
            b=read_method_body_from_bytes(p.get_data(m.Rva))
            out['methods'][hex(0x6000000+rid)]['local_argument_sites']=[
              [i.offset-b.header_size,i.opcode.name,getattr(i.operand,'index',None)]
              for i in b.instructions if i.opcode.name.startswith(('ldloc','stloc','ldarg','starg'))]
            out['methods'][hex(0x6000000+rid)]['array_sites']=[
              [i.offset-b.header_size,i.opcode.name]for i in b.instructions
              if i.opcode.name.startswith(('ldelem','stelem'))]
        out['writer_method_flags']=flags(ts.MethodDef.rows[0x2e1f-1].Flags)
        t=ts.TypeDef.rows[0xb24-1];ids={f.row_index for f in t.FieldList}
        out['original_file_sections']={'token':'0x2000b24','name':str(t.TypeName),
          'values':{str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
            for c in ts.Constant if c.Parent.table.name=='Field'and c.Parent.row_index in ids}}
        # A field in another assembly would use that module's MemberRef, not this
        # EXE's FieldDef token. Check matching owner/name metadata before claiming
        # the EXE direct scan is sufficient for companion managed inputs.
        out['companion_field_reference_audit']=[]
        class Capture(logging.Handler):
            def emit(self,record):warnings.append(record.getMessage())
        logger=logging.getLogger('dnfile');capture=Capture();old_propagate=logger.propagate
        logger.addHandler(capture);logger.propagate=False
        for name in sorted(z.namelist()):
            if not name.lower().endswith(('.exe','.dll'))or name=='osu!.exe':continue
            warnings=[]
            data=z.read(name);q=dnfile.dnPE(data=data)
            item={'file':name,'sha256':hashlib.sha256(data).hexdigest(),'managed':bool(q.net),
              'metadata_warnings':warnings}
            if q.net:
                item['matching_memberrefs']=[hex(0xa000000+i)
                  for i,r in enumerate(q.net.mdtables.MemberRef,1)
                  if r.Name.value==field.Name.value
                  and str(getattr(r.Class.row,'TypeName',''))==str(decl.TypeName)
                  and str(getattr(r.Class.row,'TypeNamespace',''))==str(decl.TypeNamespace)]
            out['companion_field_reference_audit'].append(item);q.close()
        logger.removeHandler(capture);logger.propagate=old_propagate;p.close()
    return out

if __name__=='__main__':print(json.dumps(extract(sys.argv[1]),sort_keys=True,allow_nan=False))
