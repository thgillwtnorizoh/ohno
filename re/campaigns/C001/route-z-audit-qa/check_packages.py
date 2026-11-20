#!/usr/bin/env python3
"""Run actual CI portable DLLs on Linux; not a new native Windows run."""
from pathlib import Path
import hashlib,json,subprocess,sys
H=Path(__file__).resolve().parent;ROOT=H.parents[3];dotnet=sys.argv[1];manifest=json.loads((H/'manifest.json').read_text());pin=json.loads((H/'expected-sources.json').read_text());checks=[];identities=[]
for platform in ['linux','windows']:
 dll=Path('/tmp/i11-package-'+platform)/'CorpusAudit.dll'
 run=subprocess.run([dotnet,str(dll),str(H/'fixtures')],capture_output=True,text=True,check=True);r=json.loads(run.stdout)
 assert r['sourceCommit']==pin['source'] and r['sourceState']=='clean' and r['complete'] and len(r['charts'])==len(manifest['cases'])
 for path in pin['paths']:
  raw=subprocess.check_output(['git','-C',str(ROOT),'show',pin['source']+':'+path])
  if platform=='windows':raw=raw.replace(b'\r\n',b'\n').replace(b'\n',b'\r\n')
  assert r['sourceHashes'][Path(path).name].upper()==hashlib.sha256(raw).hexdigest().upper()
 for c in manifest['cases']:
  key=hashlib.sha256((c['name']+'.osu').encode()).hexdigest().upper();row=next(x for x in r['charts'] if x['entryKey']==key)
  ok=row['index']['status']=='success' and row['canonical']['status']==c['canonicalStatus'] and row['canonical']['code']==c['canonicalCode'] and row['canonical'].get('exceptionType')==c['exceptionType'];checks.append(dict(platform=platform,fixture=c['name'],pass_=ok));assert ok,c['name']
 identities.append(dict(platform=platform,executedOn=sys.platform,source=r['sourceCommit'],sourceState=r['sourceState'],toolSha256=hashlib.sha256(dll.read_bytes()).hexdigest(),sourceHashes=r['sourceHashes']))
(H/'packaged-results.json').write_text(json.dumps(dict(passed=len(checks),failed=0,checks=checks,identities=identities,qualification='Both portable DLLs executed on Linux; original native Windows/Linux295 gates corroborated, not rerun'),indent=2)+'\n');print('PASS',len(checks),'CI packaged fixture outcomes, exact compiled identities')
