#!/usr/bin/env python3
"""Targeted stack/consumer recovery and exact F252d direct-use census. No execution."""
import hashlib,importlib.util,json,sys,zipfile
from pathlib import Path
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
H=Path(__file__).resolve().parent;R=H.parents[3]
def sha(b):return hashlib.sha256(b).hexdigest()
def previous(folder):
 p=H.parent/folder/'recover.py';s=importlib.util.spec_from_file_location('reuse_'+folder,p);m=importlib.util.module_from_spec(s);s.loader.exec_module(m);return m
SITES=[(0x142d,0x191,'ldc.i4.2',None),(0x142d,0x192,'stfld',0x4002525),(0x142d,0x1a2,'ldc.i4.4',None),(0x142d,0x1a3,'or',None),(0x142d,0x1a4,'stfld',0x4002525),(0x28f0,4,'callvirt',0x60028d2),(0x28c9,0x3b,'callvirt',0x60024c7),(0x28c9,0x42,'ldloc.0',None),(0x28c9,0x43,'stloc.2',None),(0x28ec,0x43,'ldfld',0x400252d),(0x28ec,0x48,'brfalse.s',0x56),(0x28ec,0x51,'brfalse',0x139),(0x28ec,0x73,'conv.r4',None),(0x28ec,0x7a,'conv.r4',None),(0x28ec,0x89,'conv.r4',None),(0x28ec,0x9b,'ldfld',0x400252c),(0x28ec,0xa1,'ldfld',0x400252c),(0x28ec,0xcc,'ldfld',0x4002524),(0x28ec,0xd1,'stloc.3',None),(0x28ec,0xe6,'callvirt',0x60024b7),(0x28ec,0x10f,'ldloc.s',4),(0x28ec,0x111,'sub',None),(0x28ec,0x128,'stloc.3',None),(0x28ec,0x151,'call',0x6004f01),(0x28ec,0x156,'call',0x6004efe),
(0xad9,1,'ldc.i4.1',None),(0xad9,2,'callvirt',0x6003e4e),(0xad9,7,'brfalse.s',0x70),(0xad9,0x32,'ldfld',0x400252d),(0xad9,0x38,'ble.s',0x70),(0xad9,0x4d,'callvirt',0x60024b6),(0xad9,0x52,'brfalse.s',0x70),(0xad9,0x67,'ldfld',0x4001527),(0xad9,0x6e,'ldc.i4.0',None),
(0x5f0,0x1e,'cgt',None),(0x5f0,0x21,'ceq',None),(0x603,0,'ret',None),(0x24b7,1,'ldfld',0x400252c),(0x3e4e,1,'ldfld',0x4002525),(0x3e5c,0x60,'ldflda',0x400252d),(0x2295,0xe0,'callvirt',0x6000ad9)]
def recover(archive):
 inherited={}
 for f in ['legacy-spinner-profile','stacking']:
  packet=previous(f).recover(archive);assert json.loads(json.dumps(packet))==json.loads((H.parent/f/'static-evidence.json').read_text()),f
  inherited[f]=dict(packetSha256=sha((H.parent/f/'static-evidence.json').read_bytes()),methods=len(packet['methods']),sites=packet.get('assertionCount',packet.get('assertion_count')))
 data=Path(archive).read_bytes();assert sha(data)=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
 with zipfile.ZipFile(archive) as z:raw=z.read('osu!.exe')
 assert sha(raw)=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
 p=dnfile.dnPE(data=raw);bodies={};methods={};uses=[];decoded=0;zero=0
 # A single exact-field reference census closes consumer uncertainty. No whole
 # methods/listings are emitted, and only relevant bodies are retained by hash.
 for rid,m in enumerate(p.net.mdtables.MethodDef.rows,1):
  if not m.Rva:zero+=1;continue
  b=read_method_body_from_bytes(p.get_data(m.Rva));decoded+=1
  hits=[[i.offset-b.header_size,i.opcode.name] for i in b.instructions if getattr(i.operand,'value',None)==0x400252d]
  if hits:uses.append(dict(method=f'M{rid:04x}',sites=hits))
  if hits or rid in {x[0] for x in SITES}|{0x48e0}:
   bodies[rid]=b;methods[f'M{rid:04x}']=dict(rva=m.Rva,codeBytes=b.code_size,bodySha256=sha(p.get_data(m.Rva,b.size)),signature=m.Signature.value.hex())
 assert [x['method'] for x in uses]==['M0ad9','M24b0','M24b1','M28eb','M28ec','M3e5c']
 checks=[]
 for rid,off,op,want in SITES:
  b=bodies[rid];i=next(x for x in b.instructions if x.offset-b.header_size==off);a=getattr(i.operand,'value',i.operand)
  if hasattr(a,'index'):a=a.index
  if op.startswith(('br','bl','bg','be','bn')):a-=b.header_size
  assert (i.opcode.name,a)==(op,want),(hex(rid),hex(off),i.opcode.name,a,want)
  checks.append([f'M{rid:04x}',f'{off:04x}',op,want])
 e15=json.loads((R/'re/evidence/15/slider-entry.json').read_text())
 for rid in [0x142d,0x28f0,0x28c9,0x48e0]:assert methods[f'M{rid:04x}']['bodySha256']==e15['methods'][hex(0x6000000+rid)]['body_sha256']
 inherited['E015']=dict(packetSha256=sha((R/'re/evidence/15/slider-entry.json').read_bytes()),matchedMethods=4)
 base=p.net.mdtables.MethodDef.rows[0x24b6-1];setter=p.net.mdtables.MethodDef.rows[0x24b0-1];end=p.net.mdtables.MethodDef.rows[0x24b7-1]
 t=p.net.mdtables.TypeDef.rows[0xd8-1]
 for slot,rid in [(base,0x5f0),(setter,0x603)]:
  a=[m for m in t.MethodList if str(m.row.Name)==str(slot.Name)];assert len(a)==1 and a[0].row_index==rid and not a[0].row.Flags.mdNewSlot and a[0].row.Signature.value==slot.Signature.value
 for tr in [0xd8,0x67f]:assert not any(str(m.row.Name)==str(end.Name) for m in p.net.mdtables.TypeDef.rows[tr-1].MethodList)
 return dict(archiveSha256=sha(data),exeSha256=sha(raw),offsetBasis='CIL code-relative without header',inherited=inherited,methods=methods,sites=checks,siteCount=len(checks),directHeightUseCensus=dict(decoded=decoded,zeroRva=zero,errors=0,uses=uses),spinnerSlots=dict(position='M0603 ret',eligibility='M05f0 time-dependent; not constant false',end='inherited M24b7 F252c'),originalExecuted=False)
if __name__=='__main__':
 p=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(p,indent=2,sort_keys=True)+'\n');print('PASS',p['siteCount'],'new sites; inherited55+61; F252d',p['directHeightUseCensus']['decoded'],'bodies / 6 users / 0 decode errors')
