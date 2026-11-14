#!/usr/bin/env python3
"""Bounded P-path CIL-site evidence. No original execution or full-EXE scan."""
from pathlib import Path
import hashlib,json,sys,zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
ROOT=Path(__file__).resolve().parents[4]
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
REGIONS={0x144a:[(0,0xc9),(0x56a,0x7a2),(0x87c,0xb55)],0x28f7:[(0x16a1,0x17cd),(0x1cfa,0x1d05)],
0x142d:[(0x50,0x5d),(0xad,0x123)],0x2d65:[],0x2d6a:[],0x2d6b:[],0x2d6c:[],0x110c:[],0x110d:[],0x4eca:[],0x4ecb:[],0x4ed2:[],0x4efd:[],0x4efe:[],0x4f00:[]}
SITES=[(0x144a,0xb4,'switch',[0xce,0x1f8,0x7a2,0x56a]),
(0x144a,0x576,'blt',0x7a2),(0x144a,0x587,'bgt',0x1f8),
(0x144a,0x5bc,'call',0x6002d65),(0x144a,0x5c1,'brfalse.s',0x5cf),(0x144a,0x5c5,'stfld',0x4000be4),
(0x144a,0x5ca,'br',0x7a2),(0x144a,0x5dd,'call',0x6002d6a),
(0x144a,0x5e2,'ldsfld',0x4002c6d),(0x144a,0x5e8,'bne.un',0x70b),
(0x144a,0x622,'call',0xa0002d4),(0x144a,0x62b,'call',0xa0002d1),
(0x144a,0x6b0,'ldc.r4',512.0),(0x144a,0x6cb,'ldc.r4',384.0),
(0x144a,0x6e6,'ldc.r4',640.0),(0x144a,0x701,'ldc.r4',640.0),
(0x144a,0x706,'bgt',0x1f8),(0x144a,0x715,'call',0xa0002d3),
(0x144a,0x71a,'stfld',0x4000bdd),(0x144a,0x725,'ldc.r8',0.125),
(0x144a,0x72f,'conv.i4',None),(0x144a,0x736,'ldc.i4.1',None),
(0x144a,0x741,'div',None),(0x144a,0x758,'add',None),
(0x144a,0x761,'call',0x6002d6c),(0x144a,0x76d,'newobj',0x600110c),
(0x144a,0x785,'blt.s',0x73b),(0x144a,0x78a,'ldloc.s',58),
(0x144a,0x78c,'newobj',0x600110c),(0x144a,0x798,'stfld',0x4000be4),
(0x144a,0x79d,'br',0x89a),
(0x144a,0x895,'stfld',0x4000be4),(0x144a,0x8d0,'stfld',0x4000bdd),
(0x144a,0x944,'bne.un.s',0x9b0),(0x144a,0xa12,'ldc.r8',0.0001),
(0x144a,0xa1c,'ble.un.s',0xa70),(0x144a,0xa31,'brfalse.s',0xa8d),
(0x144a,0xa4f,'call',0x6004ed2),(0x144a,0xa5d,'conv.r4',None),
(0x144a,0xae1,'stfld',0x4000bdc),(0x144a,0xaf6,'stfld',0x4000bde),
(0x28f7,0x1702,'ldc.i4',2064465807),(0x28f7,0x1711,'brtrue.s',0x1730),
(0x28f7,0x1730,'ldc.i4.3',None),(0x28f7,0x176c,'conv.i4',None),
(0x28f7,0x176d,'conv.r4',None),(0x28f7,0x17ac,'conv.i4',None),
(0x28f7,0x17ad,'conv.r4',None),(0x28f7,0x1cfc,'brfalse.s',0x1d03),
(0x28f7,0x1d03,'rethrow',None),(0x2d65,0x37,'ldc.r4',0.0),
(0x2d65,0x3c,'ceq',None),(0x2d6a,0x0,'ldc.r4',2.0),
(0x2d6a,0x47,'call',0x6004eca),(0x2d6a,0x90,'div',None),
(0x2d6a,0xc2,'div',None),(0x2d6a,0xd6,'call',0x6004ecb),
(0x2d6a,0xdb,'stind.r4',None),(0x2d6a,0xe5,'call',0x6002d6b),
(0x2d6a,0x10a,'ldc.r8',6.2831854820251465),(0x2d6a,0x119,'blt.s',0x109),
(0x2d6a,0x122,'ldc.r8',6.2831854820251465),(0x2d6a,0x133,'blt.s',0x11d),
(0x2d6a,0x139,'ble.un.s',0x14b),(0x2d6a,0x140,'ldc.r8',6.2831854820251465),
(0x2d6b,0xc,'sub',None),(0x2d6b,0xd,'conv.r8',None),
(0x2d6b,0x1c,'call',0xa0002d7),(0x2d6c,1,'call',0xa0004ca),
(0x2d6c,9,'conv.r4',None),(0x2d6c,0xb,'call',0xa0004cb),
(0x2d6c,0x13,'conv.r4',None),(0x2d6c,0x1a,'call',0x6004efd)]
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
   if op in ['add','sub','mul','div','ceq','stind.r4','stind.r8'] or op.startswith('conv.'):row['arithmetic'].append([off,op])
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
 literal=old['strings']['2064465807'];assert literal['text']=='P'
 assert methods['M28f7']['body_sha256']==old['methods']['0x60028f7']['body_sha256']
 assert not any(t[2]=='0x4000fd1' for t in methods['M144a']['token_sites'] if 0x56a<=t[0]<0x7a2)
 return {'archive_sha256':ARCHIVE,'exe_sha256':EXE,'offsetBasis':'CIL code-relative, no header','methods':methods,'externalMembers':external,'assertions':checks,'assertionCount':len(checks),'historicalIdentityChecks':overlap,'inheritedPLiteral':literal,'originalExecuted':False,'limits':['Only 15 selected bodies/regions decoded; no whole-EXE scan','No original CLR, UI or runtime precision claim','Fresh ordinary Play; editor size fallback is separately recorded']}
if __name__=='__main__':
 out=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(out,indent=2,sort_keys=True)+'\n');print('PASS',out['assertionCount'],'static assertions;',out['historicalIdentityChecks'],'historical hash comparisons;',len(out['methods']),'selected methods')
