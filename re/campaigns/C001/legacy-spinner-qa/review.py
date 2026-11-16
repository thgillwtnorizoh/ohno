#!/usr/bin/env python3
"""Independent source, history/supersession and immutable evidence review."""
import hashlib,json,subprocess,gzip,base64
from pathlib import Path
H=Path(__file__).resolve().parent;ROOT=H.parents[3];R=ROOT/'reconstructed/campaigns/C001/legacy-spinner-c01';SOURCE='9ed9eb5c58f631f89fb66fc97802cfbf9ca86e44'
def git(*args):return subprocess.check_output(['git','-C',str(ROOT),*args])
def sha(b):return hashlib.sha256(b).hexdigest()
def method(text,name):
 start=text.index('    private static void '+name+'(');pos=text.index('{',start);depth=1;i=pos+1
 while depth:
  depth+=(text[i]=='{')-(text[i]=='}');i+=1
 return text[start:i]
old=git('show','2a2e291:reconstructed/CircleGame/Gameplay/Beatmap.cs').decode();new=git('show',SOURCE+':reconstructed/CircleGame/Gameplay/Beatmap.cs').decode()
assert method(old,'PrepareFiniteStandardStacking')==method(new,'PrepareFiniteStandardStacking')
unchanged=['Gameplay/CircleSession.cs','Gameplay/CircleHitSounds.cs','Platform/WindowsCircleEffects.cs','Platform/LocalHitSoundAssets.cs']
for f in unchanged:assert git('show','2a2e291:reconstructed/CircleGame/'+f)==git('show',SOURCE+':reconstructed/CircleGame/'+f)
assert not git('diff','2a2e291',SOURCE,'--','reconstructed/CircleGame/Difficulty').strip()
ledger=json.loads((R/'supersession.json').read_text());history=[]
for entry in ledger['historical']:
 raw=git('show',entry['source']+':'+entry['path']);assert sha(raw)==entry['sha256']
 assert git('rev-parse',entry['source']+':'+entry['path']).decode().strip()==entry['blob']
 snapshot=R/('historical-CatalogIdentity.cs' if entry['path'].endswith('CatalogIdentity.cs') else 'historical-Program.cs');assert snapshot.read_bytes()==raw
 history.append(dict(path=entry['path'],source=entry['source'],blob=entry['blob'],snapshotSha256=sha(raw)))
prior=ledger['historical'][0];a=git('show',prior['source']+':'+prior['path']).decode();b=git('show',SOURCE+':'+prior['path']).decode()
# Invert just the authorized admission substitution and three new named controls.
b=b.replace('("spinner-old",Text(rows:"1,2,1000,8,0,1500",format:13),ChartAvailability.Indexed)','("spinner-old",Text(rows:"1,2,1000,8,0,1500",format:13),ChartAvailability.Unsupported)')
lines=b.splitlines(keepends=True);added=[l for l in lines if any('Add("C/'+n+'"' in l for n in ('spinner-old-canonical','spinner-old-candidate-rejected','spinner-old-geometric-qualification'))];assert len(added)==3
assert ''.join(l for l in lines if l not in added)==a,'unrelated QA14 source changed'
prior=ledger['historical'][1];a=git('show',prior['source']+':'+prior['path']).decode();b=git('show',SOURCE+':'+prior['path']).decode()
lo='            foreach(var pair in new[]{("stack",';end='            File.WriteAllText(path,text.Replace("8,2","136,2"));';alo=a.index(lo);ahi=a.index(end,alo)
blo=b.index('            foreach(var pair in new[]{("short timing",');bhi=b.index(end,blo)
assert a[:alo]==b[:blo] and a[ahi:]==b[bhi:],'unrelated Issue7 assertion changed'
for retained in ['("short timing",text.Replace("0,500,4,1,0,100,1,0","0,500"))','("start zero",text.Replace("2000,8","0,8"))']:
 assert retained in a[alo:ahi] and retained in b[blo:bhi]
assert 'Reject(()=>Beatmap.LoadCanonical(path),"RouteZ constructor-head proximity excluded")' in b[blo:bhi]
# Work1 fixtures, manifests, old verifiers/results unchanged relative to research publication.
for folder in ['re/campaigns/C001/legacy-spinner-profile','re/campaigns/C001/versioned-spinners','re/campaigns/C001/versioned-spinner-qa']:
 changes=git('diff','f9cce9e',SOURCE,'--name-only','--',folder).decode().splitlines()
 assert changes==(['re/campaigns/C001/legacy-spinner-profile/.gitattributes'] if folder.endswith('legacy-spinner-profile') else []),changes
 for rel in git('ls-tree','-r','--name-only','f9cce9e','--',folder).decode().splitlines():assert (ROOT/rel).read_bytes()==git('show','f9cce9e:'+rel),rel
assert (ROOT/'re/campaigns/C001/legacy-spinner-profile/.gitattributes').read_text()=='fixtures/*.osu -text\n'
# Original-static and historical independent result/fixture seals; no original extraction.
subprocess.run(['python',str(ROOT/'re/campaigns/C001/legacy-spinner-profile/check.py')],check=True)
w=json.loads((R/'windows.json').read_text());assert w['source']==SOURCE
assert sum(x['passed'] for x in w['historical'])==2312 and sum(x['failed'] for x in w['historical'])==0
for name in ['initial-gate','final-gate']:
 info=json.loads((R/(name+'.json')).read_text());raw=gzip.decompress(base64.b64decode((R/(name+'.log.gz.b64')).read_bytes()));assert sha(raw)==info['summary_sha256']
initial=json.loads((R/'initial-gate.json').read_text())
(H/'source-review.json').write_text(json.dumps(dict(source=SOURCE,modernMethodSha256=sha(method(new,'PrepareFiniteStandardStacking').encode()),unchangedProduction=unchanged,unchangedR17=True,supersession=history,qa14OnlyAuthorizedChanges=True,issue7OnlyAuthorizedBlockChanged=True,allResearchFixturesAndOldEvidenceUnchanged=True,attributes='-text only; fixture blobs unchanged',initialCRLFFailurePreserved=True,windowsHistorical=2312,fullRegression='REUSED, NOT RERUN'),indent=2)+'\n')
print('PASS independent source/supersession/fixture preservation and initial/final gate seals')
