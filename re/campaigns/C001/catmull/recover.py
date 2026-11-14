#!/usr/bin/env python3
"""Bounded C-path CIL-site evidence. No original execution or full-EXE scan."""
from pathlib import Path
import hashlib,json,sys,zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
ROOT=Path(__file__).resolve().parents[4]
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
REGIONS={0x144a:[(0,0x1f8),(0x89a,0xb55)],0x28f7:[(0x166f,0x17cd),(0x1cfa,0x1d05)],
0x142d:[(0xad,0x123)],0x4ee0:[],0x110c:[],0x110d:[],0x4efd:[],0x4efe:[]}
SITES=[(0x144a,0xb4,'switch',[0xce,0x1f8,0x7a2,0x56a]),
(0x144a,0xce,'ldc.i4.0',None),(0x144a,0xd1,'br',0x1d8),
(0x144a,0xdb,'bge.s',0xec),(0x144a,0x123,'call',0x6004efe),(0x144a,0x128,'call',0x6004efd),
(0x144a,0x14f,'blt.s',0x163),(0x144a,0x157,'call',0x6004efe),(0x144a,0x15c,'call',0x6004efd),
(0x144a,0x177,'br.s',0x1b8),(0x144a,0x184,'conv.r4',None),(0x144a,0x185,'ldc.r4',50.0),
(0x144a,0x18a,'div',None),(0x144a,0x18b,'call',0x6004ee0),
(0x144a,0x19b,'add',None),(0x144a,0x19c,'conv.r4',None),(0x144a,0x19d,'ldc.r4',50.0),
(0x144a,0x1a2,'div',None),(0x144a,0x1a3,'call',0x6004ee0),(0x144a,0x1a8,'newobj',0x600110c),
(0x144a,0x1ba,'ldc.i4.s',50),(0x144a,0x1bc,'blt.s',0x179),
(0x144a,0x1cc,'ldc.i4.1',None),(0x144a,0x1cd,'stfld',0x4000a34),
(0x144a,0x1e6,'sub',None),(0x144a,0x1e7,'blt',0xd6),
(0x144a,0x1ed,'ldc.i4.0',None),(0x144a,0x1ee,'stfld',0x4000be4),(0x144a,0x1f3,'br',0x89a),
(0x144a,0x8d0,'stfld',0x4000bdd),(0x144a,0x944,'bne.un.s',0x9b0),
(0x144a,0xa12,'ldc.r8',0.0001),(0x144a,0xa1c,'ble.un.s',0xa70),
(0x144a,0xa31,'brfalse.s',0xa8d),(0x144a,0xa4f,'call',0x6004ed2),(0x144a,0xa5d,'conv.r4',None),
(0x144a,0xae1,'stfld',0x4000bdc),(0x144a,0xaf6,'stfld',0x4000bde),
(0x28f7,0x166f,'ldc.i4.0',None),(0x28f7,0x16c9,'ldc.i4',2064560670),
(0x28f7,0x16d8,'brtrue.s',0x1718),(0x28f7,0x1718,'ldc.i4.0',None),
(0x28f7,0x176c,'conv.i4',None),(0x28f7,0x176d,'conv.r4',None),
(0x28f7,0x17ac,'conv.i4',None),(0x28f7,0x17ad,'conv.r4',None),
(0x28f7,0x1d03,'rethrow',None),
(0x4ee0,0x4,'mul',None),(0x4ee0,0x9,'mul',None),
(0x4ee0,0xd,'ldc.r4',0.5),(0x4ee0,0x12,'ldc.r4',2.0),
(0x4ee0,0x3c,'ldc.r4',5.0),(0x4ee0,0x49,'ldc.r4',4.0),
(0x4ee0,0x67,'ldc.r4',3.0),(0x4ee0,0x74,'ldc.r4',3.0),
(0x4ee0,0x8c,'stfld',0x4003109),(0x4ee0,0x93,'ldc.r4',0.5),
(0x4ee0,0x112,'stfld',0x400310a),(0x4ee0,0x118,'ret',None),
(0x110c,0x8,'stfld',0x4000a32),(0x110c,0xf,'stfld',0x4000a33)]
def recover(path):
 data=Path(path).read_bytes();assert hashlib.sha256(data).hexdigest()==ARCHIVE
 with zipfile.ZipFile(path) as z:raw=z.read('osu!.exe')
 assert hashlib.sha256(raw).hexdigest()==EXE
 p=dnfile.dnPE(data=raw);methods={};bodies={};external={}
 for rid,regions in REGIONS.items():
  m=p.net.mdtables.MethodDef.rows[rid-1];b=read_method_body_from_bytes(p.get_data(m.Rva));bodies[rid]=b;h=b.header_size
  row={'body_sha256':hashlib.sha256(p.get_data(m.Rva,b.size)).hexdigest(),'rva':m.Rva,'code_bytes':b.code_size,'signature':m.Signature.value.hex(),'regions':regions,'token_sites':[],'constants':[],'branches':[],'arithmetic':[]}
  bounds={i.offset-h for i in b.instructions}
  for i in b.instructions:
   off=i.offset-h;op=i.opcode.name;a=i.operand
   if regions and not any(lo<=off<hi for lo,hi in regions):continue
   if hasattr(a,'value'):
    row['token_sites'].append([off,op,hex(a.value)])
    if a.value>>24==10:
     r=p.net.mdtables.MemberRef.rows[(a.value&0xffffff)-1];external[hex(a.value)]=str(r.Name)
   if op.startswith('ldc.'):row['constants'].append([off,op,a])
   if op=='switch' or op.startswith(('br','beq','bne','bge','bgt','ble','blt','leave')):
    dest=[v-h for v in a] if op=='switch' else [a-h];assert all(v in bounds for v in dest)
    row['branches'].append([off,op,dest])
   if op in ['add','sub','mul','div','neg','ceq','stind.r4','stind.r8'] or op.startswith('conv.'):row['arithmetic'].append([off,op])
  methods[f'M{rid:04x}']=row
 checks=[]
 for rid,off,op,want in SITES:
  b=bodies[rid];i=next(x for x in b.instructions if x.offset-b.header_size==off)
  got=getattr(i.operand,'value',i.operand)
  if op=='switch':got=[v-b.header_size for v in got]
  elif op.startswith(('br','bl','bg','be','bn')):got-=b.header_size
  elif hasattr(got,'index') and not isinstance(got,list):got=got.index
  assert (i.opcode.name,got)==(op,want),(hex(rid),hex(off),i.opcode.name,got,want)
  checks.append([f'M{rid:04x}',f'{off:04x}',op,want])
 # Same pinned method bytes establish that inherited E006/E032/decoder evidence applies.
 old=json.loads((ROOT/'re/campaigns/C001/original-behavior/static-inventory.json').read_bytes())
 overlap=0
 for k,v in methods.items():
  key=hex(0x6000000+int(k[1:],16))
  for oldpath in ['re/evidence/06/linear-geometry.json','re/evidence/32/bezier-geometry.json']:
   inherited=json.loads((ROOT/oldpath).read_bytes())['methods']
   if key in inherited:assert v['body_sha256']==inherited[key]['body_sha256'];overlap+=1
 literal=old['strings']['2064560670'];assert literal['text']=='C'
 assert methods['M28f7']['body_sha256']==old['methods']['0x60028f7']['body_sha256']
 assert not any(t[2]=='0x4000fd1' for t in methods['M144a']['token_sites'] if 0xce<=t[0]<0x1f8)
 assert not any(i.opcode.name in ['conv.r8','call','callvirt'] for i in bodies[0x4ee0].instructions)
 assert not any(t[2]=='0x4000a35' for t in methods['M144a']['token_sites'] if 0xce<=t[0]<0x1f8)
 return {'archive_sha256':ARCHIVE,'exe_sha256':EXE,'offsetBasis':'CIL code-relative, no header','methods':methods,'externalMembers':external,'assertions':checks,'assertionCount':len(checks),'historicalIdentityChecks':overlap,'inheritedCLiteral':literal,'originalExecuted':False,'limits':['Only 8 selected bodies/regions decoded; no whole-EXE scan','No original CLR, UI or runtime precision claim','Fresh ordinary Play; full legacy/editor/error scheduling excluded']}
if __name__=='__main__':
 out=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(out,indent=2,sort_keys=True)+'\n');print('PASS',out['assertionCount'],'static assertions;',out['historicalIdentityChecks'],'historical hash comparisons;',len(out['methods']),'selected methods')
