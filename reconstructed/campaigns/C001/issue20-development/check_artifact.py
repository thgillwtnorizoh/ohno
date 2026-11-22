#!/usr/bin/env python3
"""Work2 exact Windows delivery verification. Never executes the package."""
import hashlib,json,re,struct,sys,zipfile
from pathlib import Path
R=Path(__file__).resolve().parents[4];a=Path(sys.argv[1]);logs=Path(sys.argv[2]).read_text(encoding='utf-8-sig');meta=json.loads(Path(sys.argv[3]).read_text());out=Path(sys.argv[4]);out.mkdir(parents=True,exist_ok=True)
sha=lambda b:hashlib.sha256(b).hexdigest();source=meta['source'];art=meta['artifacts'][0];job=meta['jobs'][0];raw=a.read_bytes()
assert sha(raw)==art['digest'][7:] and len(raw)==art['size_in_bytes'] and art['workflow_run']['head_sha']==source
assert job['conclusion']=='success' and all(s['conclusion']=='success' for s in job['steps'])
history=[]
for line in logs.splitlines():
 m=re.search(r'(QA\d+[^:]*|HEADLESS COMPONENT TESTS):\s*(\d+) passed,\s*(\d+) failed',line)
 if m:history.append(dict(suite=m[1],passed=int(m[2]),failed=int(m[3])))
assert len(history)==19 and sum(x['passed'] for x in history)==2312 and not any(x['failed'] for x in history)
assert 'Issue20 development 5430/5430 PASS' in logs and 'Issue18 development 3974/3974 PASS' in logs and 'S25' in logs and 'M16' in logs
assert all(int(x)==0 for x in re.findall(r'(\d+) Warning\(s\)',logs)) and all(int(x)==0 for x in re.findall(r'(\d+) Error\(s\)',logs))
with zipfile.ZipFile(a) as z:
 pref=next(n[:-len('SOURCE-COMMIT.txt')]for n in z.namelist() if n.endswith('/SOURCE-COMMIT.txt'));assert z.read(pref+'SOURCE-COMMIT.txt').decode().strip()==source
 fixtures=[]
 for folder,corpus in [('control-count','LinearControlCorpus'),('traversal-profile','TraversalCorpus')]:
  h=R/'re/campaigns/C001'/folder;m=json.loads((h/'manifest.json').read_text())
  for c in m['cases']:
   b=z.read(pref+corpus+'/'+c['id']+'.osu');assert sha(b).lower()==c['sha256'].lower();fixtures.append(dict(id=c['id'],sha256=sha(b),profile=folder))
 packet=z.read(next(n for n in z.namelist() if n.endswith('/c001-linear-controls/results.json')));j=json.loads(packet)
 assert (j['source'],j['state'],j['passed'],j['failed'],j['prepared'],j['refused'],j['added'],j['lost'],j['audio'])==(source,'clean',5430,0,112,56,74,0,112)
 assert len(j['cases'])==168 and len({x['id']for x in j['cases']})==168
 m=json.loads((R/'re/campaigns/C001/control-count/manifest.json').read_text())
 for c in m['cases']:
  x=next(x for x in j['cases']if x['id']==c['id']);assert x['sha256'].lower()==c['sha256'].lower() and x['prepared']==(c['futureCandidate']or c['current']['prepared']) and x['previouslyPrepared']==c['current']['prepared']
 linked=json.loads(z.read(next(n for n in z.namelist() if n.endswith('/c001-linear-controls/windows-linked.json'))));assert (linked['source'],linked['prepared'],linked['added'],linked['failed'])==(source,112,74,0) and all(x['pass']for x in linked['checks'])
 # The actual WPF DLL portable PDB was read by the Windows suite, not a source-only assertion.
 assert len(linked['documents'])==1
 compiled={}
 pins=json.loads((R/'re/campaigns/C001/control-count/expected-sources.json').read_text())
 for f in pins['paths']:
  if Path(f).name=='LocalHitSoundAssets.cs':continue
  b=(R/f).read_bytes();win=b.replace(b'\r\n',b'\n').replace(b'\n',b'\r\n');h=j['sourceHashes'][Path(f).name];assert h.lower() in [sha(b),sha(win)];compiled[f]=h
 assert linked['documents'][0]['sourceHash'].lower()==j['sourceHashes']['Beatmap.cs'].lower()
 exe=z.read(pref+'Ohno.CircleGame.exe');assert source.encode()in exe
 pe=struct.unpack_from('<I',exe,0x3c)[0];assert exe[pe:pe+4]==b'PE\0\0'and struct.unpack_from('<H',exe,pe+4)[0]==0x8664
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
 major=value('<I');minor=value('<I');count=value('<i');bid=string();assert major==6;header=value('<qqqqQ');entries=[]
 for _ in range(count):
  off=value('<q');size=value('<q');compressed=value('<q');kind=value('<B');name=string();assert off+size<=len(exe);entries.append(dict(name=name,offset=off,size=size,compressed=compressed,kind=kind))
 def entry(name):
  e=next(x for x in entries if x['name']==name);assert e['compressed']==0;return exe[e['offset']:e['offset']+e['size']]
 runtime=json.loads(entry('Ohno.CircleGame.runtimeconfig.json'));assert {x['name']for x in runtime['runtimeOptions']['includedFrameworks']}=={'Microsoft.NETCore.App','Microsoft.WindowsDesktop.App'}
 assert any(x['name']=='PresentationFramework.dll' for x in entries)and any(x['name']=='System.Private.CoreLib.dll' for x in entries)
 dll=entry('Ohno.CircleGame.dll');linkedDll=z.read(next(n for n in z.namelist()if n.endswith('/c001-linear-controls/windows-linked.dll')));assert sha(linkedDll)==linked['assemblySha256'].lower()
 import dnfile
 from dncil.cil.body.reader import read_method_body_from_bytes
 p=dnfile.dnPE(data=dll);methods=[]
 for t in p.net.mdtables.TypeDef.rows:
  if str(t.TypeName)not in ['Beatmap','SliderGeometryPreparation','SliderControlProfile']:continue
  for m in t.MethodList:
   name=str(m.row.Name)
   if name not in ['LoadCore','PreflightSliderMovements','PrepareSliderBounded','PrepareSliderGeometry','PrepareProfile','ProfileForSource']:continue
   body=read_method_body_from_bytes(p.get_data(m.row.Rva));ops=[i.opcode.name for i in body.instructions];methods.append(dict(type=str(t.TypeName),name=name,codeSize=body.code_size,bodySha256=sha(p.get_data(m.row.Rva,body.size)),checkedMultiply='mul.ovf' in ops))
 lp=dnfile.dnPE(data=linkedDll);linkedMethods={}
 for t in lp.net.mdtables.TypeDef.rows:
  for m in t.MethodList:
   key=(str(t.TypeName),str(m.row.Name))
   if key not in [(x['type'],x['name'])for x in methods]:continue
   body=read_method_body_from_bytes(lp.get_data(m.row.Rva));linkedMethods[key]=sha(lp.get_data(m.row.Rva,body.size))
 assert all(linkedMethods[(x['type'],x['name'])]==x['bodySha256']for x in methods)
 assert len(methods)==6 and next(x['checkedMultiply']for x in methods if x['name']=='PreflightSliderMovements')
seal=dict(source=source,run=art['workflow_run']['id'],job=job['id'],successfulSteps=len(job['steps']),historical=history,historicalPassed=2312,development=j['passed'],wpfAssertions=linked['passed'],artifactSha256=sha(raw),artifactBytes=len(raw),exeSha256=sha(exe),exeBytes=len(exe),fixtureHashes=fixtures,individualDispositions=168,compiledSourceHashes=compiled,packetSha256=sha(packet),decodedLogSha256=sha(logs.encode()),bundleEntries=count,runtime=runtime,wpfMethods=methods,wpfManagedSha256=sha(dll),linkedWpfManagedSha256=sha(linkedDll),linkedAndBundledMethodsIdentical=True,wpfPortablePdb=linked['documents'],exeExecuted=False,independentQA=False)
(out/'artifact-seal.json').write_text(json.dumps(seal,indent=2)+'\n');(out/'windows-results.json').write_bytes(packet);(out/'windows-linked.json').write_text(json.dumps(linked,indent=2)+'\n')
(out/'ci-summary.log').write_text('\n'.join(x for x in logs.splitlines()if len(x)<450 and re.search(r'passed,|/\d+ PASS|0 Warning|0 Error|S25|M16|SHA256 digest|--self-contained|8.0.428',x))+'\n')
print(f'PASS exact Windows ZIP/exe/bundle/PDB/6 methods/274 fixture hashes/168 dispositions;{len(job["steps"])}steps;5430development;{linked["passed"]}WPF;2312historical')
