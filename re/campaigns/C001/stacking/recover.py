#!/usr/bin/env python3
"""Targeted original-byte operands, never executes original code or emits listings."""
import hashlib,json,sys,zipfile
from pathlib import Path
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
# Independent assertions at decisive sites: method, code-relative offset, opcode, operand.
SITES=[(0x28ea,0xc,'ble.s',0x55),(0x28ea,0x2e,'callvirt',0x600144a),(0x28ea,0x4f,'call',0x60028eb),
(0x28eb,0x6,'callvirt',0x6001bba),(0x28eb,0xb,'brfalse.s',0xe),
(0x28eb,0x1f,'ldfld',0x4000ae1),(0x28eb,0x25,'ldfld',0x4000ae1),
(0x28eb,0x30,'ldfld',0x4000adb),(0x28eb,0x3c,'ldfld',0x4001004),(0x28eb,0x41,'mul',None),
(0x28eb,0x57,'stfld',0x400252d),(0x28eb,0xc6,'bgt.s',0x124),
(0x28eb,0xe0,'blt.s',0x105),(0x28eb,0x103,'bge.un.s',0x111),
(0x28eb,0x168,'brtrue',0x2e7),(0x28eb,0x17e,'brfalse',0x280),
(0x28eb,0x1a7,'ldfld',0x4002524),(0x28eb,0x1b0,'bgt',0x2e7),
(0x28eb,0x1cd,'callvirt',0x600249c),(0x28eb,0x1e2,'bge.un.s',0x24b),
(0x28eb,0x1f0,'sub',None),(0x28eb,0x1f2,'add',None),
(0x28eb,0x235,'sub',None),(0x28eb,0x236,'stfld',0x400252d),
(0x28eb,0x244,'ble.s',0x1fc),(0x28eb,0x26b,'add',None),
(0x28eb,0x272,'stloc.0',None),(0x28eb,0x2a9,'ldfld',0x4002523),
(0x28eb,0x2b2,'bgt.s',0x2e7),(0x28eb,0x2b6,'callvirt',0x600249c),
(0x28eb,0x2d6,'add',None),(0x28eb,0x2de,'stloc.0',None),
(0x28eb,0x2ed,'bgt',0x153),(0x28eb,0x316,'conv.r4',None),
(0x28eb,0x319,'call',0x6004f01),(0x28eb,0x31e,'call',0x6004efe),
(0x28eb,0x324,'callvirt',0x60024b0),(0x24b0,2,'stfld',0x400252c),
(0x24b0,8,'brtrue.s',0x18),(0x24b0,0xc,'stfld',0x400152e),
(0x28be,0x12d,'conv.i4',None),(0x28be,0x12e,'stfld',0x4000adb),
(0x28be,0x18a,'ldc.r4',1.0004099607467651),(0x28be,0x190,'stfld',0x4000ada),
(0x28be,0x1e6,'ldfld',0x4000ada),(0x28be,0x1eb,'ldc.r4',10.0),
(0x28be,0x1f0,'div',None),(0x28be,0x1f1,'stfld',0x4000ae1),
(0x1444,1,'ldfld',0x4000be0),(0x249c,1,'callvirt',0x6003e58),
(0x249c,0x12,'call',0x6004efe),(0x249c,0x17,'call',0x6004efe),
(0x1457,0xe,'call',0x6004efe),(0x1457,0x1b,'call',0x60024b0),
(0x1457,0x87,'stfld',0x4000a32),(0x1457,0xa9,'stfld',0x4000a33),
(0x1457,0x264,'callvirt',0x6003e59),(0x144a,0x136f,'callvirt',0x6003e59),
(0x144a,0x1378,'stfld',0x4002524),(0x28e9,0x1b,'call',0x60028ea),
(0x28d2,0xcb,'callvirt',0xa0010b2)]
def recover(path):
 data=Path(path).read_bytes();assert hashlib.sha256(data).hexdigest()==ARCHIVE
 with zipfile.ZipFile(path) as z:raw=z.read('osu!.exe')
 assert hashlib.sha256(raw).hexdigest()==EXE
 p=dnfile.dnPE(data=raw);methods={};bodies={}
 for rid in sorted({x[0] for x in SITES}|{0x4ecb,0x4f01,0x4efe,0x28b2}):
  m=p.net.mdtables.MethodDef.rows[rid-1];b=read_method_body_from_bytes(p.get_data(m.Rva));bodies[rid]=b
  methods[f'M{rid:04x}']={'body_sha256':hashlib.sha256(p.get_data(m.Rva,b.size)).hexdigest(),'code_bytes':b.code_size}
 results=[]
 for rid,off,op,want in SITES:
  b=bodies[rid];i=next(x for x in b.instructions if x.offset-b.header_size==off)
  got=getattr(i.operand,'value',i.operand)
  if op.startswith(('br','bl','bg','be','bn')):got-=b.header_size
  assert (i.opcode.name,got)==(op,want),(hex(rid),hex(off),i.opcode.name,got,want)
  results.append({'method':f'M{rid:04x}','offset':f'{off:04x}','opcode':op,'operand':want})
 # Cross-check old identities, not just new claims against newly generated data.
 root=Path(__file__).resolve().parents[4]
 old=json.loads((root/'re/campaigns/C001/original-behavior/static-inventory.json').read_bytes())['methods'];overlap=0
 for k,v in methods.items():
  if hex(0x6000000+int(k[1:],16)) in old:
   assert v['body_sha256']==old[hex(0x6000000+int(k[1:],16))]['body_sha256'];overlap+=1
 return {'archive_sha256':ARCHIVE,'exe_sha256':EXE,'methods':methods,'assertions':results,'assertion_count':len(results),'historical_hash_matches':overlap,'originalExecuted':False,'scope':'targeted >5 standard full-range no-mod stacking; not an exhaustive callgraph'}
if __name__=='__main__':
 result=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(result,indent=2,sort_keys=True)+'\n');print('PASS',result['assertion_count'],'decisive sites;',result['historical_hash_matches'],'historical method hashes')
