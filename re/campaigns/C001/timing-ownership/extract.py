#!/usr/bin/env python3
"""Issue23 selected-method metadata/token evidence; no whole-method-body scan.

Pinned E004 collector, dnfile0.18.0/dncil1.0.2/pefile2024.8.26. Original binaries,
raw CIL and audio remain private. Output is original-static evidence only.
"""
import hashlib,json,pathlib,sys,zipfile
H=pathlib.Path(__file__).resolve().parent;R=H.parents[3]
sys.path.insert(0,str(R/'re/tools'))
import beatmap_init_evidence as base
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
METHODS=[0x2237,0x223b,0x2240,0x224d,0x3b0e,0x3b0f,0x3b45,0x3d52,
 0x3b72,0x3b66,0x1bc1,0x28a2,0x28f5,0x28f7,0x28e9,0x28ea,0x28ab,
 0x1be3,0x1be4,0x1be6,0x24bb,0x1469,0x146a,0x48e0,0x3b25,0x3b61,
 0x0b7e,0x223c,0x0708,0x070d,0x28ff,0x28be,0x28f6,0x28a3,0x3b48,
 0x3b39,0x3b4f,0x3b44,0x3b3a,0x0651]
base.METHODS=METHODS
e=base.extract(sys.argv[1]);assert e['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert e['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
raw=zipfile.ZipFile(sys.argv[1]).read('osu!.exe');pe=dnfile.dnPE(data=raw)
def instructions(rid):
 m=pe.net.mdtables.MethodDef.rows[rid-1];b=read_method_body_from_bytes(pe.get_data(m.Rva));out={}
 for i in b.instructions:
  v=i.operand
  if hasattr(v,'value'):v=hex(v.value)
  out[i.offset-b.header_size]=(i.opcode.name,v)
 return out
relations=[]
def relation(name,rid,sites,claim):
 ins=instructions(rid)
 for off,op,value in sites:
  assert ins[off]==(op,value),(name,hex(off),ins[off],(op,value))
 relations.append(dict(id=name,method=hex(0x6000000+rid),sites=[hex(s[0]) for s in sites],claim=claim,passed=True))
relation('play-captures-selected-map',0x2237,[(1,'call','0x6002c63'),(6,'stfld','0x40013f8')],'Play stores selected map reference, not a map clone.')
relation('manager-binds-same-map',0x28a2,[(0,'ldarg.0',None),(1,'ldarg.1',None),(2,'stfld','0x40017ee')],'Manager map is the passed object.')
relation('audio-reuse-binds-map',0x3b45,[(0xb1,'ldsfld','0x4002367'),(0xb6,'ldarg.0',None),(0xb7,'stfld','0x40024ad')],'Audio reuse branch replaces song-map reference with passed map.')
relation('new-audio-binds-map',0x3d52,[(0x66,'ldloc.0',None),(0x67,'ldarg.0',None),(0x68,'stfld','0x40024ad')],'Every non-null factory result receives passed map after construction.')
relation('global-song-getter',0x3b0e,[(0,'ldsfld','0x4002367'),(0xb,'ldfld','0x40024ad')],"Getter returns current audio object's map, null when audio object is null.")
relation('global-list-getter',0x3b72,[(0,'call','0x6003b0e'),(0xb,'ldfld','0x400100f')],'Getter returns song-map F100f directly, without constructing a list.')
relation('in-place-sort-receiver',0x3b66,[(6,'call','0x6003b72'),(0xb,'stloc.0',None),(0x2e,'ldloc.0',None),(0x2f,'callvirt','0xa000877')],'List.Sort receiver is the exact list returned by global-list getter.')
relation('forced-init-rescan',0x224d,[(0x132,'ldc.i4.1',None),(0x133,'call','0x6003b66')],'M224d calls rescan with true.')
relation('early-loader-config-arguments',0x28f5,[(0x7c,'ldc.i4.0',None),(0x7d,'ldc.i4.1',None),(0x7e,'ldc.i4.1',None),(0x7f,'callvirt','0x60028be')],'Loader calls config(false,true,true) BEFORE object parser and postpass.')
relation('early-config-forced-rescan',0x28be,[(0x1af,'ldarg.2',None),(0x1b2,'ldc.i4.1',None),(0x1b3,'call','0x6003b66')],'True second config argument reaches forced in-place sort before object construction/preparation.')
relation('late-circle-owner-read',0x24bb,[(1,'ldfld','0x400152f'),(6,'ldfld','0x40017ee'),(0x14,'callvirt','0x6001be6')],'Discrete head/circle selector reaches current manager map at request time.')
relation('raw-selector-current-field',0x1be6,[(1,'ldfld','0x400100f'),(9,'ldfld','0x400100f'),(0x1e,'ldfld','0x400100f')],'M1be6 re-reads map F100f, not an object snapshot.')
e['ownership_relations']=relations;e['selected_method_count']=len(METHODS)
# Targeted metadata only: ordinary manager inherits M28be. No global body scan.
tables=pe.net.mdtables;target=tables.MethodDef.rows[0x28be-1];decl=[]
for rid in [0xe8,0x556,0x680]:
 typ=tables.TypeDef.rows[rid-1]
 for item in typ.MethodList:
  method=item.row
  if str(method.Name)==str(target.Name) and method.Signature.value==target.Signature.value:
   decl.append(dict(owner=hex(0x2000000+rid),method=hex(0x6000000+item.row_index),new_slot=method.Flags.mdNewSlot))
assert not any(d['owner']=='0x20000e8' for d in decl)
assert any(d['method']=='0x60028be' for d in decl)
impl=[]
if tables.MethodImpl:
 for row in tables.MethodImpl.rows:
  if row.Class.row_index==0xe8:
   impl.append(dict(declaration=row.MethodDeclaration.row_index,body=row.MethodBody.row_index))
assert not any(d['declaration']==0x28be for d in impl)
e['configuration_slot']=dict(declarations=decl,ordinary_method_impls=impl,ordinary_type='0x20000e8',ordinary_extends=hex(0x2000000+tables.TypeDef.rows[0xe8-1].Extends.row_index),special_type_excluded='0x2000680')
e['collector_sha256']=hashlib.sha256(pathlib.Path(__file__).read_bytes()).hexdigest()
e['reused_collector_sha256']=hashlib.sha256((R/'re/tools/beatmap_init_evidence.py').read_bytes()).hexdigest()
e['limits']+=['Only40 selected methods decoded; no global caller/writer rescan.','Successful non-null audio/map setup and reached ordinary M223b path; special manager T0680 excluded.','Original runtime, GUI/audio and x86 CLR NOT RUN.']
print(json.dumps(e,sort_keys=True,indent=2))
