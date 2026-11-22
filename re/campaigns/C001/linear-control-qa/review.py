#!/usr/bin/env python3
"""Independent provenance/preservation and artifact-vs-new-QA assessment."""
import argparse,hashlib,json,subprocess,zipfile,re,struct
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3]
START='184ba46fc02a1339b37a0b842c1e903f98430f00';TARGET='5dda483348e21e79c8354bf6ea322d50b7fb3e72';BASE='6a152648c72773eb33d37f8ac01f18ca35ab1f41'
p=argparse.ArgumentParser();p.add_argument('exact');p.add_argument('zip');p.add_argument('log');p.add_argument('output');a=p.parse_args();E=Path(a.exact).resolve();O=Path(a.output).resolve();O.mkdir(exist_ok=True,parents=True)
git=lambda *a:subprocess.check_output(['git','-C',str(R),*a]);sha=lambda b:hashlib.sha256(b).hexdigest()
assert subprocess.check_output(['git','-C',str(E),'rev-parse','HEAD'],text=True).strip()==TARGET
assert not subprocess.check_output(['git','-C',str(E),'status','--porcelain'],text=True).strip()
assert git('diff','--name-only',BASE,TARGET,'--','reconstructed/CircleGame').decode().splitlines()==['reconstructed/CircleGame/Gameplay/Beatmap.cs']
assert not git('diff',BASE,TARGET,'--','reconstructed/CorpusAudit','re/qa','re/evidence','re/tools')
assert not git('diff',TARGET,START,'--','reconstructed/CircleGame','reconstructed/CorpusAudit','re/qa')
historical={}
for path in git('ls-tree','-r','--name-only',START,'re').decode().splitlines():
 before=git('show',START+':'+path);after=(R/path).read_bytes()
 if path in ['re/contexts.md']+['re/campaigns/C001/'+f for f in ['coverage-matrix.md','dependency-register.md','discrepancy-register.md','status.md']]:assert after.startswith(before),path
 else:assert after==before,path
 historical[path]=sha(before)
j=json.loads((O/'results.json').read_text());m=json.loads((R/'re/campaigns/C001/control-count/manifest.json').read_text());assert len(m['cases'])==len(j['cases'])==168
static=R/'re/campaigns/C001/control-count/static-evidence.json';assert static.read_bytes()==git('show','131c78835594b5814cd3d6ee3d9435fb9ae26ee1:'+str(static.relative_to(R)))
raw=Path(a.zip).read_bytes();assert sha(raw)=='91c41682aaeab1ba18e311bbebbeeb4307752098dd33bf1059c71a11205348cb' and len(raw)==68714855
meta=json.loads((H/'ci-live.json').read_text());job=meta['jobs'][0];assert job['id']==137146777470 and len(job['steps'])==54 and all(s['conclusion']=='success' for s in job['steps']);assert [s['number'] for s in job['steps']]==list(range(1,53))+[104,105]
log=Path(a.log).read_text(encoding='utf-8-sig');assert sha(log.encode())=='92dd1d1b849b5af3674db3c1534bafb6ea3fa9fa4260954032873561fa5c12e0'
history=re.findall(r'(?:QA\d+[^:]*|HEADLESS COMPONENT TESTS):\s*(\d+) passed,\s*(\d+) failed',log);assert len(history)==19 and sum(int(x[0]) for x in history)==2312 and all(x[1]=='0' for x in history)
with zipfile.ZipFile(a.zip) as z:
 names=z.namelist();pref=next(n[:-len('SOURCE-COMMIT.txt')] for n in names if n.endswith('/SOURCE-COMMIT.txt'));assert z.read(pref+'SOURCE-COMMIT.txt').decode().strip()==TARGET
 w=json.loads(z.read(next(n for n in names if n.endswith('/c001-linear-controls/results.json'))))
 assert (w['source'],w['passed'],w['failed'],w['prepared'],w['refused'],w['added'],w['lost'])==(TARGET,5430,0,112,56,74,0)
 # Compare every development outcome with independently byte-derived expectations,
 # rather than trusting futureCandidate or the development success flag.
 for x in j['cases']:
  y=next(c for c in w['cases'] if c['id']==x['id'])
  assert (x['hash'].lower(),x['error'],x['prepared'],x['audit']['Code'])==(y['sha256'].lower(),y['error'],y['prepared'],y['audit']['Code']),x['id']
 linked=json.loads(z.read(next(n for n in names if n.endswith('/c001-linear-controls/windows-linked.json'))));assert linked['passed']==564 and linked['failed']==0 and linked['prepared']==112 and linked['pathSegments']==13350
 for folder,corpus in [('control-count','LinearControlCorpus'),('traversal-profile','TraversalCorpus')]:
  manifest=json.loads((R/f're/campaigns/C001/{folder}/manifest.json').read_text())
  for c in manifest['cases']:assert sha(z.read(pref+corpus+'/'+c['id']+'.osu')).lower()==c['sha256'].lower()
 exe=z.read(pref+'Ohno.CircleGame.exe');pe=struct.unpack_from('<I',exe,0x3c)[0];assert exe[pe:pe+4]==b'PE\0\0' and struct.unpack_from('<H',exe,pe+4)[0]==0x8664 and TARGET.encode() in exe
 assert sha(exe)=='409e7fc13a92047e1eff0a56bf681c351f5616eca9e4b8d3be029f5e866238d7'
# Preserve and assess the verifier-only correction without calling it production.
patch=git('diff','585c624b2675e534dfaacf9c497120099f61dc6d',TARGET,'--').decode();assert 'GetFlattenedPathGeometry' in patch and 'PathGeometry.CreateFromGeometry' in patch
assert not git('diff','585c624b2675e534dfaacf9c497120099f61dc6d',TARGET,'--','reconstructed/CircleGame')
first=(R/'reconstructed/campaigns/C001/issue20-development/first-windows-summary.log').read_text();assert '5430' in first
seal=dict(startingMain=START,exactSource=TARGET,priorProduction=BASE,onlyGameplayFileChanged='Beatmap.cs',
 priorResearchFilesVerified=len(historical),historicalFileHashes=historical,staticPacketSha256=sha(static.read_bytes()),
 artifactSha256=sha(raw),artifactBytes=len(raw),exeSha256=sha(exe),freshLogSha256=sha(log.encode()),liveSteps=54,
 novelPerFixtureDevelopmentComparisons=168,packagedFixtureHashes=274,headlessDevelopment=5430,wpfDevelopment=564,historicalWindows=2312,
 allNewIndependentExpectationsAgree=True,firstFailurePreserved=True,rendererUnchangedAcrossVerifierCorrection=True,
 originalExecuted=False,WindowsExecuted=False,GUIExecuted=False,ownerCorpusAccessed=False)
(O/'source-and-artifact-review.json').write_text(json.dumps(seal,indent=2)+'\n')
print(f'PASS {len(historical)} historical research files/append-only prefixes; isolated exact production; fresh ZIP/log/PE/stamp/274fixture identities;168 independently derived outcomes agree;54steps; preserved verifier failure')
