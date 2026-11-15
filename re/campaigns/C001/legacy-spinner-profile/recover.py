#!/usr/bin/env python3
"""Issue9 bounded original-static identity/site recovery; no original execution."""
import hashlib,json,sys,zipfile
from pathlib import Path
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
ROOT=Path(__file__).resolve().parents[4]
ZIP='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738';EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
REGIONS={0x1461:[],0x1462:[],0x144a:[(0xd76,0xd8b)],0x3201:[(0,0x28)],0x3b3c:[],0x28ec:[],0x0603:[],0x24b7:[],0x28ea:[],0x28eb:[(0x150,0x1c4),(0x1dc,0x247),(0x292,0x2b4),(0x2f2,0x335)],0x28ff:[],0x0b73:[],0x1bc1:[(0x51,0x70),(0x350,0x37d),(0x7ab,0x7d0)]}
SITES=[(0x1461,1,'ldfld',0x4000bfd),(0x1462,2,'stfld',0x4000bfd),(0x144a,0xd81,'ldfld',0x4000a33),(0x144a,0xd86,'callvirt',0x60024b8),(0x3201,0x17,'stfld',0x400252c),(0x3201,0x23,'stfld',0x400152e),(0x3b3c,0x15,'ldfld',0x4000fd1),(0x3b3c,0x1b,'ble.s',0x24),(0x3b3c,0x1e,'ldfld',0x4000661),(0x3b3c,0x24,'ldsfld',0x400236b),(0x603,0,'ret',None),(0x24b7,1,'ldfld',0x400252c),(0x28ea,0xc,'ble.s',0x55),
(0x28ec,0x28,'isinst',0x20002af),(0x28ec,0x36,'callvirt',0x600144a),(0x28ec,0x3c,'ldfld',0x4002524),(0x28ec,0x4c,'callvirt',0x6003e4e),
(0x28ec,0x6e,'ldfld',0x4002523),(0x28ec,0x86,'mul',None),(0x28ec,0x87,'sub',None),(0x28ec,0x8a,'bgt.un',0x139),
(0x28ec,0xa6,'call',0x6004ecb),(0x28ec,0xab,'ldc.r4',3.0),(0x28ec,0xb0,'bge.un.s',0xd4),(0x28ec,0xbb,'stfld',0x400252d),
(0x28ec,0xe6,'callvirt',0x60024b7),(0x28ec,0xf5,'bge.un.s',0x129),(0x28ec,0x112,'stfld',0x400252d),(0x28ec,0x15c,'callvirt',0x60024b0),
(0x28eb,0x16e,'isinst',0x200067f),(0x28eb,0x196,'isinst',0x200067f),(0x28eb,0x29a,'isinst',0x200067f),(0x28eb,0x20f,'ldfld',0x400152e),(0x28eb,0x236,'stfld',0x400252d),(0x28eb,0x324,'callvirt',0x60024b0),
(0x28ff,0x16,'ble',0xde),(0x28ff,0x21,'ldc.i4.0',None),(0x28ff,0x87,'ldelem',0x10002a9),(0x28ff,0x97,'ldc.i4.0',None),(0x28ff,0xac,'ldarg.2',None),(0x28ff,0xc1,'ldc.i4.1',None),(0x28ff,0xd6,'newobj',0x6000b73),(0x28ff,0x115,'call',0x6003b3c),(0x28ff,0x11a,'ldc.i4.s',100),(0x28ff,0x12b,'ldnull',None),
(0xb73,0x2b,'ldc.i4.2',None),(0xb73,0x2c,'stfld',0x4000663),(0xb73,0x34,'stfld',0x4000661),(0xb73,0x3f,'call',0x6002d7c),(0xb73,0x44,'stfld',0x4000665),(0xb73,0x4c,'call',0x6000b75),
(0x1bc1,0x66,'stfld',0x4001004),(0x1bc1,0x35b,'ldfld',0x4001000),(0x1bc1,0x361,'ldfld',0x4000fe1),(0x1bc1,0x366,'call',0x60028ff)]
def recover(path):
 assert hashlib.sha256(Path(path).read_bytes()).hexdigest()==ZIP
 with zipfile.ZipFile(path) as z:raw=z.read('osu!.exe')
 assert hashlib.sha256(raw).hexdigest()==EXE
 p=dnfile.dnPE(data=raw);methods={};bodies={};checks=[]
 old=json.loads((ROOT/'re/campaigns/C001/original-behavior/static-inventory.json').read_text())['methods'];overlap=0
 for rid,regions in REGIONS.items():
  m=p.net.mdtables.MethodDef.rows[rid-1];b=read_method_body_from_bytes(p.get_data(m.Rva));bodies[rid]=b;h=b.header_size
  tokens=[];branches=[];constants=[];arithmetic=[]
  for i in b.instructions:
   off=i.offset-h;op=i.opcode.name;a=i.operand
   if regions and not any(lo<=off<hi for lo,hi in regions):continue
   if hasattr(a,'value'):tokens.append([off,op,hex(a.value)])
   if op.startswith('ldc.'):constants.append([off,op,a])
   if op.startswith(('br','bl','bg','be','bn','leave')):branches.append([off,op,a-h])
   if op in ('mul','sub','add','div') or op.startswith('conv.'):arithmetic.append([off,op])
  digest=hashlib.sha256(p.get_data(m.Rva,b.size)).hexdigest()
  if hex(0x6000000+rid) in old:assert digest==old[hex(0x6000000+rid)]['body_sha256'];overlap+=1
  methods[f'M{rid:04x}']=dict(body_sha256=digest,rva=m.Rva,code_bytes=b.code_size,signature=m.Signature.value.hex(),regions=regions,token_sites=tokens,branches=branches,constants=constants,arithmetic=arithmetic)
 for rid,off,op,want in SITES:
  b=bodies[rid];i=next(x for x in b.instructions if x.offset-b.header_size==off);a=getattr(i.operand,'value',i.operand)
  if op.startswith(('br','bl','bg','be','bn','leave')):a-=b.header_size
  assert (i.opcode.name,a)==(op,want),(hex(rid),hex(off),i.opcode.name,a,want)
  checks.append([f'M{rid:04x}',f'{off:04x}',op,want])
 assert not any(x[2]=='0x200067f' for x in methods['M28ec']['token_sites'])
 setter=p.net.mdtables.MethodDef.rows[0x24b0-1];spinner=p.net.mdtables.TypeDef.rows[0xd8-1]
 overrides=[m for m in spinner.MethodList if str(m.row.Name)==str(setter.Name)]
 assert len(overrides)==1 and overrides[0].row_index==0x603 and overrides[0].row.Flags.mdVirtual and not overrides[0].row.Flags.mdNewSlot
 assert spinner.Extends.row_index==0x67f
 parent=p.net.mdtables.TypeDef.rows[0x67f-1]
 assert parent.Extends.row_index==0x4c3 and not any(str(m.row.Name)==str(setter.Name) for m in parent.MethodList)
 assert any(m.row_index==0x24b0 for m in p.net.mdtables.TypeDef.rows[0x4c3-1].MethodList)
 assert overrides[0].row.Signature.value==setter.Signature.value
 endget=p.net.mdtables.MethodDef.rows[0x24b7-1];endset=p.net.mdtables.MethodDef.rows[0x24b8-1];slider=p.net.mdtables.TypeDef.rows[0x2af-1]
 assert any(m.row_index==0x1461 and str(m.row.Name)==str(endget.Name) and not m.row.Flags.mdNewSlot for m in slider.MethodList)
 assert any(m.row_index==0x1462 and str(m.row.Name)==str(endset.Name) and not m.row.Flags.mdNewSlot for m in slider.MethodList)
 return dict(legacySliderEnd=dict(getter='M1461',setter='M1462',field='F0bfd',producer='M144a:0d76-0d86 last path segment To; distinct from F0be0 traversal endpoint'),archive_sha256=ZIP,exe_sha256=EXE,offsetBasis='CIL code-relative excluding header',methods=methods,assertions=checks,assertionCount=len(checks),historicalIdentities=overlap,spinnerSetter=dict(type='T00d8',extends='T067f',reusesSlot='M24b0',implementation='M0603',body='ret'),earlyPassHasNoSpinnerTypeTest=True,originalExecuted=False)
if __name__=='__main__':
 out=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(out,indent=2,sort_keys=True)+'\n');print(f"PASS {out['assertionCount']} sites / {out['historicalIdentities']} inherited identities / empty spinner setter binding")
