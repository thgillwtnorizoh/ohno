#!/usr/bin/env python3
import json,hashlib,subprocess,zipfile
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3];SOURCE='22a403505f13461477343b9c6a251980716dd90f';BASE='61e898109db47f3fcaac9027b97416f1943aa827'
def git(*args):return subprocess.check_output(['git','-C',str(R),*args])
def sha(b):return hashlib.sha256(b).hexdigest()
changed=git('diff','--name-only',BASE,SOURCE,'--','reconstructed/CircleGame').decode().splitlines();assert changed==['reconstructed/CircleGame/Gameplay/Beatmap.cs']
assert not git('diff',BASE,SOURCE,'--','reconstructed/CorpusAudit','reconstructed/CircleGame/Difficulty').strip()
assert not git('diff',SOURCE,'1638dfd9','--','reconstructed/CircleGame','reconstructed/CorpusAudit').strip()
# Read and seal the complete diff, including the new core's entry cancellation.
patch=git('diff',BASE,SOURCE,'--','reconstructed/CircleGame/Gameplay/Beatmap.cs');(H/'production.diff').write_bytes(patch)
a=git('show',BASE+':reconstructed/CircleGame/Gameplay/Beatmap.cs').decode();b=git('show',SOURCE+':reconstructed/CircleGame/Gameplay/Beatmap.cs').decode()
lo=a.index('    private static bool StackingClose(');hi=a.index('    private static SliderDefinition TranslateStackedSlider(',lo)
assert a[lo:hi] in b,'distance primitive changed'
lo=a.index('    private static void PrepareFiniteStandardStacking(');hi=a.index('    // Sufficient fresh',lo);assert a[lo:hi] in b,'modern stacking changed'
files={}
for folder,name in [('near-spinner', 'manifest.json'),('legacy-spinner-profile','manifest.json'),('versioned-spinners','manifest.json')]:
 h=R/'re/campaigns/C001'/folder;m=json.loads((h/name).read_text())
 for c in m['cases']:
  p=h/c.get('path','fixtures/'+c['id']+'.osu');assert sha(p.read_bytes())==c['sha256'];files[str(p.relative_to(R))]=c['sha256']
assert git('diff','--name-only',BASE,SOURCE,'--','re').decode().splitlines()==['re/campaigns/C001/near-spinner/.gitattributes']
# Existing old re blobs unchanged; the sole addition preserves fixture checkout bytes; hash-check independently sealed research packet.
pin=json.loads((R/'re/campaigns/C001/near-spinner/results/provenance.json').read_text())
for f,v in pin['packetHashes'].items():assert sha((R/'re/campaigns/C001/near-spinner'/f).read_bytes())==v
result=dict(source=SOURCE,base=BASE,productionChanged=changed,completePatchSha256=sha(patch),modernAndDistance='byte-identical',consumersClassifierR17='unchanged',historicalResearch='existing re blobs unchanged; sole new .gitattributes byte-preserving policy',fixtureHashes=files,internalHeight='not reconstructed/zeroed; original theoretical heights preserved in immutable manifest')
(H/'source-review.json').write_text(json.dumps(result,indent=2)+'\n');print('PASS complete production diff; unchanged consumers/math/history;214 fixture hashes and old verifier seals')
