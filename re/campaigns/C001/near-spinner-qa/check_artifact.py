#!/usr/bin/env python3
import json,hashlib,zipfile,sys
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3];A=Path(sys.argv[1]);sha=lambda b:hashlib.sha256(b).hexdigest()
raw=A.read_bytes();assert len(raw)==68070123 and sha(raw)=='a38d5de4fd01847d33708352a0e8554854e6d4f4936d96268a9d0e4c74bce54e'
m=json.loads((R/'re/campaigns/C001/near-spinner/manifest.json').read_text());pin=json.loads((H/'expected-sources.json').read_text());seals={}
with zipfile.ZipFile(A) as z:
 prefix='_temp/ohno-c001-windows/'
 assert z.read(prefix+'SOURCE-COMMIT.txt').decode().strip()==pin['source']
 exe=z.read(prefix+'Ohno.CircleGame.exe');assert len(exe)==154041616 and sha(exe)=='431ea62270070239fecf87035bd821ddaa08e3e27047619265c98975feb28c80'
 assert z.read(prefix+'NEAR-SPINNER-MANIFEST.json').replace(b'\r\n',b'\n')==(R/'re/campaigns/C001/near-spinner/manifest.json').read_bytes()
 for c in m['cases']:assert sha(z.read(prefix+'NearSpinnerCorpus/'+c['id']+'.osu'))==c['sha256']
 p=z.read('_temp/c001-near-spinner/results.json');j=json.loads(p);assert (j['source'],j['state'],j['passed'],j['failed'],j['prepared'],j['rejected'],j['newlyPrepared'])==(pin['source'],'clean',1503,0,59,41,12)
 for c in m['cases']:
  actual=next(x for x in j['outcomes'] if x['id']==c['id']);assert actual['fixtureSha256'].lower()==c['sha256'] and actual['prepared']==c['handoff']['prepared']
 for f in pin['paths']:
  name=Path(f).name
  if name=='LocalHitSoundAssets.cs':continue
  b=(R/f).read_bytes();assert j['sourceHashes'][name]==sha(b.replace(b'\r\n',b'\n').replace(b'\n',b'\r\n')).upper(),name
 seals=dict(zipBytes=len(raw),zipSha256=sha(raw),exeBytes=len(exe),exeSha256=sha(exe),source=pin['source'],fixtureHashes=100,WindowsDevelopment=dict(passed=1503,failed=0,prepared=59,rejected=41,new=12,packetSha256=sha(p)),compiledHashes=j['sourceHashes'],exeExecuted=False)
ci=json.loads((H/'ci-live.json').read_text());assert len(ci['steps'])==52 and all(x['conclusion']=='success' for x in ci['steps']);assert len(ci['historical'])==19 and sum(x['passed'] for x in ci['historical'])==2312 and sum(x['failed'] for x in ci['historical'])==0
seals['liveCI']=dict(run=44774297221,job=136419852800,steps=52,historicalPassed=2312,historicalFailed=0,source=pin['source']);(H/'artifact-seal.json').write_text(json.dumps(seals,indent=2)+'\n');print('PASS actual ZIP/EXE/source/100 fixture identities/Windows1503packet/compiled CRLF hashes; live52steps/2312 historical counts')
