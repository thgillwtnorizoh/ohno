#!/usr/bin/env python3
"""Issue17 bounded fresh original-byte recovery; no original execution or large-count simulation."""
import json,hashlib,zipfile,sys
from pathlib import Path
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
H=Path(__file__).resolve().parent;R=H.parents[3]
sha=lambda b:hashlib.sha256(b).hexdigest()
PACKETS=['05/slider-timing.json','16/slider-components.json','17/slider-completion.json','21/first-repeat.json','22/terminal-checkpoint.json','39/slider-hitsounds.json','40/slider-continuous-audio.json']
REGIONS={0x28f7:[(0x17cd,0x17fa),(0x1840,0x19dc)],0x142d:[(0xa5,0xad),(0x123,0x16e)],0x1433:[(0,8)],0x144a:[(0xd47,0xd75),(0xdd5,0xe80),(0xef0,0xf45),(0x11c5,0x1202),(0x1332,0x1345),(0x1377,0x137d),(0x14aa,0x1553)],0x145c:[(0,30)],0x146f:[(0x2e0,0x313)],0x146a:[(0xc6,0x125)],0x1465:[(0xd,0x40)],0x1449:[(0x131,0x181)],0x146b:[],0x3b33:[]}
def recover(path):
 data=Path(path).read_bytes();assert sha(data)=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
 with zipfile.ZipFile(path) as z:raw=z.read('osu!.exe')
 assert sha(raw)=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
 packets={p:json.loads((R/'re/evidence'/p).read_text()) for p in PACKETS};pe=dnfile.dnPE(data=raw);methods={};checks=[];overlaps=[]
 for rid,regions in REGIONS.items():
  md=pe.net.mdtables.MethodDef.rows[rid-1];body=read_method_body_from_bytes(pe.get_data(md.Rva));digest=sha(pe.get_data(md.Rva,body.size));key=hex(0x6000000+rid)
  for p,j in packets.items():
   if key in j['methods']:assert digest==j['methods'][key]['body_sha256'];overlaps.append([p,key])
  methods[key]=dict(rva=md.Rva,codeBytes=body.code_size,signature=md.Signature.value.hex(),bodySha256=digest)
  old=next((j['methods'][key] for j in packets.values() if key in j['methods']),None)
  if old is None:
   assert rid==0x1433 and [(i.opcode.name,getattr(i.operand,'value',i.operand)) for i in body.instructions]==[('ldarg.0',None),('ldarg.1',None),('stfld',0x4000bd0),('ret',None)];checks.append([key,2,'stfld',0x4000bd0]);continue
  sites={}
  for field in ['token_sites','constants','arithmetic_sites','operations']:
   for item in old.get(field,[]):
    off,op=item[:2]
    if not any(lo<=off<hi for lo,hi in regions):continue
    if field in ['arithmetic_sites','operations'] and len(item)<3:want=None
    else:want=item[2] if len(item)>2 else None
    if isinstance(want,str) and want.startswith('0x'):want=int(want,16)
    sites[(off,op)]=want
  for (off,op),want in sorted(sites.items()):
   i=next(x for x in body.instructions if x.offset-body.header_size==off);got=getattr(i.operand,'value',i.operand)
   assert (i.opcode.name,got)==(op,want),(key,hex(off),op,got,want)
   checks.append([key,off,op,want])
 # Independently asserted parser boundary and constructor normalization anchors.
 b=read_method_body_from_bytes(pe.get_data(pe.net.mdtables.MethodDef.rows[0x28f7-1].Rva));by={i.offset-b.header_size:i for i in b.instructions}
 assert by[0x17e3].operand==9000 and by[0x17e8].opcode.name=='ble.s' and by[0x17e8].operand-b.header_size==0x17fa
 assert getattr(by[0x17f4].operand,'value',0)==0xa00011f and by[0x17f9].opcode.name=='throw'
 member=pe.net.mdtables.MemberRef.rows[0x11f-1];assert str(member.Class.row.TypeName)=='ArgumentOutOfRangeException'
 bounds=[dict(raw=n,parserCapPass=n<=9000,effective=max(1,n),edgeRawPlusOne=n+1,execution='STATIC ONLY') for n in [-1,0,1,2,15,16,17,24,32,33,9000,9001]]
 return dict(archiveSha256=sha(data),exeSha256=sha(raw),methodCount=len(methods),methods=methods,siteCount=len(checks),sites=checks,historicalBodyMatches=overlaps,authorityPacketHashes={p:sha((R/'re/evidence'/p).read_bytes()) for p in PACKETS},rawCountDecisions=bounds,exception='ArgumentOutOfRangeException at >9000; not a successful load theorem',originalExecuted=False,largeCountSimulation=False)
if __name__=='__main__':
 p=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(p,indent=2,sort_keys=True)+'\n');print('PASS',p['siteCount'],'original sites;',p['methodCount'],'bodies;',len(p['historicalBodyMatches']),'historical matches;9000/9001 static-only')
