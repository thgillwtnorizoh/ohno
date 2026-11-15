#!/usr/bin/env python3
"""Bounded ordinary versioned spinner CIL-site evidence. No original execution or full-EXE scan."""
from pathlib import Path
import hashlib,json,sys,zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
ROOT=Path(__file__).resolve().parents[4]
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
REGIONS={0x28f7:[(0x1484,0x157e),(0x1ab8,0x1bc9),(0x1cfa,0x1d05)],0x3201:[],0x48e1:[],0x5ea:[],0x3e57:[],0x28f0:[],0x28d2:[(0xbe,0xdc)],0x9e4:[],0x1bc1:[(0x51,0x60),(0x201,0x251)],0x3b0c:[],0x28f5:[(0xcb,0xd9)],0x5f4:[(0x1ae,0x21d)],0x5f7:[],0x28db:[(0x86,0x13d)],0x3202:[],0x3e4d:[]}
SITES=[(0x28f7,0x1496,'ldc.i4.s',-113),(0x28f7,0x152d,'call',0x60009e4),(0x28f7,0x1532,'conv.i4',None),(0x28f7,0x1539,'ldfld',0x400100b),(0x28f7,0x153e,'add',None),
(0x28f7,0x1aba,'ldc.i4.8',None),(0x28f7,0x1abb,'call',0x60038e1),(0x28f7,0x1ac0,'brfalse',0x1bc9),
(0x28f7,0x1acf,'ldc.i4.6',None),(0x28f7,0x1ae0,'callvirt',0xa000320),(0x28f7,0x1aef,'call',0xa000d73),(0x28f7,0x1afe,'call',0xa000d73),
(0x28f7,0x1b36,'ldc.i4.s',100),(0x28f7,0x1b38,'call',0x6002d7c),(0x28f7,0x1b58,'ldc.i4.5',None),(0x28f7,0x1b68,'call',0x60009e4),(0x28f7,0x1b6d,'conv.i4',None),
(0x28f7,0x1b71,'brtrue.s',0x1b84),(0x28f7,0x1b73,'call',0x6003b0c),(0x28f7,0x1b78,'ldc.i4.s',50),(0x28f7,0x1b7d,'call',0xa0002a6),
(0x28f7,0x1b94,'ldfld',0x400100b),(0x28f7,0x1b99,'add',None),(0x28f7,0x1ba6,'callvirt',0x6001872),(0x28f7,0x1bb2,'call',0x60038e1),(0x28f7,0x1bb7,'callvirt',0x6003e57),(0x28f7,0x1bbf,'callvirt',0x60028f0),(0x28f7,0x1d03,'rethrow',None),
(0x3201,0x8,'ldc.r4',256.0),(0x3201,0xd,'ldc.r4',192.0),(0x3201,0x2a,'stfld',0x4002523),(0x3201,0x31,'stfld',0x4002524),(0x3201,0x38,'stfld',0x4002525),
(0x3201,0x48,'ldfld',0x4000fd1),(0x3201,0x4e,'bgt.s',0x5e),(0x3201,0x59,'stfld',0x4002525),(0x3201,0x61,'stfld',0x4002526),
(0x48e1,9,'newobj',0x60005ea),(0x48e1,0x11,'stfld',0x4001537),(0x48e1,0x19,'stfld',0x4001538),(0x48e1,0x21,'stfld',0x4001539),(0x48e1,0x29,'stfld',0x4001536),(0x48e1,0x31,'callvirt',0x60024b9),
(0x5ea,0x1d,'call',0x6003201),(0x5ea,0x31,'callvirt',0x60005f4),(0x3e57,0x19,'ldc.i4.s',-5),(0x3e57,0x1c,'stfld',0x4002525),
(0x28f0,4,'callvirt',0x60028d2),(0x28d2,0xd7,'stfld',0x4001805),
(0x1bc1,0x5b,'stfld',0x400100b),(0x1bc1,0x23d,'bge.s',0x247),(0x1bc1,0x24a,'ldc.i4.s',24),
(0x28f5,0xd2,'ldarg.2',None),(0x28f5,0xd4,'call',0x60028f7),
(0x5f4,0x1bf,'conv.r4',None),(0x5f4,0x1c0,'ldc.r4',1000.0),(0x5f4,0x1c6,'conv.r8',None),(0x5f4,0x1d4,'stfld',0x40002a0),
(0x5f7,0x21,'ldc.i4',2019),(0x5f7,0x27,'ldc.i4.s',10),(0x5f7,0x59,'ldc.i4',20190510),(0x28db,0xf7,'bgt.s',0xfe)]
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
 overlap=0
 for k,v in methods.items():
  key=hex(0x6000000+int(k[1:],16))
  for oldpath in ['24/spinner-entry.json','25/spinner-result.json','26/spinner-completion.json','34/score-multiplier.json','36/newcombo-preparation.json','41/spinner-audio.json']:
   path=ROOT/'re/evidence'/oldpath
   if not path.exists():continue
   inherited=json.loads(path.read_bytes())['methods']
   if key in inherited:assert v['body_sha256']==inherited[key]['body_sha256'];overlap+=1
 assert not any(t[2]=='0x4000fd1' for t in methods['M28f7']['token_sites'])
 assert not any(t[2]=='0x4000fd1' for t in methods['M05f7']['token_sites'])
 return {'archive_sha256':ARCHIVE,'exe_sha256':EXE,'offsetBasis':'CIL code-relative, no header','methods':methods,'externalMembers':external,'assertions':checks,'assertionCount':len(checks),'historicalIdentityChecks':overlap,'originalExecuted':False,'limits':['Only 16 selected bodies/regions decoded; no whole-EXE scan','No original CLR, UI or runtime precision claim; media length and source compatibility are conditional inputs','Fresh ordinary Play; full legacy/editor/error scheduling excluded']}
if __name__=='__main__':
 out=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(out,indent=2,sort_keys=True)+'\n');print('PASS',out['assertionCount'],'static assertions;',out['historicalIdentityChecks'],'historical hash comparisons;',len(out['methods']),'selected methods')
