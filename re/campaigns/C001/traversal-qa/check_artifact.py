#!/usr/bin/env python3
"""Independent existing Windows gate corroboration; never executes package."""
import hashlib,json,re,struct,sys,zipfile
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3];sha=lambda b:hashlib.sha256(b).hexdigest()
a=Path(sys.argv[1]);log_path=Path(sys.argv[2]);out=Path(sys.argv[3]);out.mkdir(parents=True,exist_ok=True)
meta=json.loads((H/'ci-live.json').read_text());logs=log_path.read_text(encoding='utf-8-sig');art=meta['artifacts'][0];source='6a152648c72773eb33d37f8ac01f18ca35ab1f41';raw=a.read_bytes();assert len(raw)==68152301 and sha(raw)==art['digest'][7:]=='d8d6c02ffc8c6ccea92e736cedbaa87df02fb7a3ac018e7e7513806f406b699a'
assert art['workflow_run']['head_sha']==source and not art['expired'];job=meta['jobs'][0];assert job['id']==136810681097 and job['run_id']==44892345157 and job['conclusion']=='success' and len(job['steps'])==53 and all(x['conclusion']=='success' for x in job['steps'])
history=[]
for line in logs.splitlines():
 m=re.search(r'(QA\d+[^:]*|HEADLESS COMPONENT TESTS):\s*(\d+) passed,\s*(\d+) failed',line)
 if m:history.append(dict(suite=m[1],passed=int(m[2]),failed=int(m[3]),line=line))
assert len(history)==19 and sum(x['passed'] for x in history)==2312 and all(x['failed']==0 for x in history)
assert 'Issue18 development 3974/3974 PASS' in logs and 'S25' in logs and 'M16' in logs
manifest=json.loads((R/'re/campaigns/C001/traversal-profile/manifest.json').read_text());pins=json.loads((H/'expected-sources.json').read_text());compiled={}
with zipfile.ZipFile(a) as z:
 pref='_temp/ohno-c001-windows/';assert z.read(pref+'SOURCE-COMMIT.txt').decode().strip()==source
 assert z.read(pref+'TRAVERSAL-MANIFEST.json').replace(b'\r\n',b'\n')==(R/'re/campaigns/C001/traversal-profile/manifest.json').read_bytes()
 for c in manifest['cases']:assert sha(z.read(pref+'TraversalCorpus/'+c['id']+'.osu'))==c['sha256']
 packet=z.read('_temp/c001-traversal/results.json');j=json.loads(packet);assert (j['source'],j['state'],j['passed'],j['failed'],j['admitted'],j['rejected'],j['added'])==(source,'clean',3974,0,86,20,45)
 assert len(j['outcomes'])==106 and len({x['id'] for x in j['outcomes']})==106
 for c in manifest['cases']:
  x=next(x for x in j['outcomes'] if x['id']==c['id']);assert x['fixtureSha256'].lower()==c['sha256'] and x['prepared']==c['futureCandidate'] and x['previouslyPrepared']==c['current']['prepared']
 assert sum(bool(x['sampleError']) for x in j['outcomes'])==4
 for f in pins['paths']:
  if Path(f).name=='LocalHitSoundAssets.cs':continue
  b=(R/f).read_bytes().replace(b'\r\n',b'\n').replace(b'\n',b'\r\n');assert sha(b).upper()==j['sourceHashes'][Path(f).name];compiled[f]=sha(b)
 exe=z.read(pref+'Ohno.CircleGame.exe');assert len(exe)==154045712 and sha(exe)=='cb85f2950f309154c285bd16a849196496e148c61ea9291efe6917560b1e99d0' and source.encode() in exe
 pe=struct.unpack_from('<I',exe,0x3c)[0];assert exe[pe:pe+4]==b'PE\0\0' and struct.unpack_from('<H',exe,pe+4)[0]==0x8664
 sig=bytes.fromhex('8b1202b96a612038727b930214d7a03213f5b9e6efae3318ee3b2dce24b36aae');pos=struct.unpack_from('<q',exe,exe.index(sig)-8)[0]
 def value(fmt):
  global pos
  v=struct.unpack_from(fmt,exe,pos);pos+=struct.calcsize(fmt);return v[0] if len(v)==1 else v
 def string():
  global pos
  n=0;shift=0
  while True:
   b=value('<B');n|=(b&127)<<shift
   if not b&128:break
   shift+=7
  s=exe[pos:pos+n].decode();pos+=n;return s
 major=value('<I');minor=value('<I');count=value('<i');bid=string();assert major==6 and count==443;header=value('<qqqqQ');entries=[]
 for _ in range(count):
  off=value('<q');size=value('<q');compressed=value('<q');kind=value('<B');name=string();assert off+size<=len(exe);entries.append(dict(name=name,offset=off,size=size,compressed=compressed,kind=kind))
 assert any(x['name']=='System.Private.CoreLib.dll' for x in entries) and any(x['name']=='PresentationFramework.dll' for x in entries)
 def entry(name):
  e=next(x for x in entries if x['name']==name);assert e['compressed']==0;return exe[e['offset']:e['offset']+e['size']]
 runtime=json.loads(entry('Ohno.CircleGame.runtimeconfig.json'));assert {x['name'] for x in runtime['runtimeOptions']['includedFrameworks']}=={'Microsoft.NETCore.App','Microsoft.WindowsDesktop.App'}
 dll=entry('Ohno.CircleGame.dll');assert sha(dll)=='f3219f60216072af7b281412a7ccb96483b0434677db8adeb21caf7be0ca2c52';(out/'wpf-managed.sha256').write_text(sha(dll)+'\n')
 # Optional exact bundled method IL identification, without loading managed code.
 import dnfile
 from dncil.cil.body.reader import read_method_body_from_bytes
 p=dnfile.dnPE(data=dll);methods=[]
 for t in p.net.mdtables.TypeDef.rows:
  if str(t.TypeName)!='Beatmap':continue
  for m in t.MethodList:
   name=str(m.row.Name)
   if name not in ['LoadCore','PreflightSliderMovements','PrepareSliderBounded','PrepareSliderGeometry']:continue
   body=read_method_body_from_bytes(p.get_data(m.row.Rva));ops=[i.opcode.name for i in body.instructions];methods.append(dict(name=name,rva=m.row.Rva,codeSize=body.code_size,bodySha256=sha(p.get_data(m.row.Rva,body.size)),checkedMultiply='mul.ovf' in ops))
 assert len(methods)==4 and next(x['checkedMultiply'] for x in methods if x['name']=='PreflightSliderMovements')
seal=dict(source=source,run=44892345157,job=job['id'],successfulSteps=53,historical=history,historicalPassed=2312,developmentCorroborated=3974,artifactSha256=sha(raw),artifactBytes=len(raw),exeSha256=sha(exe),exeBytes=len(exe),fixtureHashes=106,individualDispositions=106,storedSampleErrors=4,windowsCRLFSourceHashes=compiled,packetSha256=sha(packet),normalizedDecodedLogSha256=sha(logs.encode()),bundleEntries=count,runtime=runtime,wpfMethods=methods,wpfManagedSha256=sha(dll),exeExecuted=False,independentNewChecks=False)
(out/'artifact-seal.json').write_text(json.dumps(seal,indent=2)+'\n')
(out/'ci-summary.log').write_text('\n'.join(x for x in logs.splitlines() if len(x)<450 and re.search(r'passed,|/\d+ PASS|0 Warning|0 Error|S25|M16|SHA256 digest|--self-contained|8.0.428',x))+'\n')
print('PASS downloaded exact Windows ZIP/exe/bundle/managed methods/106 dispositions and hashes;53steps;3974development and2312historical corroborated only')
