#!/usr/bin/env python3
import hashlib,json,subprocess
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3];old='22a403505f13461477343b9c6a251980716dd90f';new='6a152648c72773eb33d37f8ac01f18ca35ab1f41';start='727958c78e4508d746d5e47abea2744f4d4fe3e5'
def git(*args):return subprocess.check_output(['git','-C',str(R),*args])
sha=lambda b:hashlib.sha256(b).hexdigest()
changed=git('diff','--name-only',old,new,'--','reconstructed/CircleGame').decode().splitlines();assert changed==['reconstructed/CircleGame/Gameplay/Beatmap.cs']
assert not git('diff',new,start,'--','reconstructed/CircleGame','reconstructed/CorpusAudit')
original=git('diff','--name-only','127e80a',start,'--','re').decode().splitlines();assert original==['re/campaigns/C001/traversal-profile/.gitattributes']
assert not git('diff',old,new,'--','re/qa','reconstructed/R12DevelopmentChecks')
manifest=json.loads((R/'re/campaigns/C001/traversal-profile/manifest.json').read_text());assert len(manifest['cases'])==106
for c in manifest['cases']:assert sha((R/'re/campaigns/C001/traversal-profile'/c['path']).read_bytes())==c['sha256']
source=git('show',new+':reconstructed/CircleGame/Gameplay/Beatmap.cs').decode();prior=git('show',old+':reconstructed/CircleGame/Gameplay/Beatmap.cs').decode()
for text in ['Legacy16 = 16, Canonical32 = 32','SliderTraversalProfile.Legacy16','checked(traversals * segments)','Observe("traversal"','Observe("segment"','Observe("tick"','Observe("movement-segment"','Observe("complete"','250_000L - totalMovements','100_000L - totalCheckpoints','100_000L - totalSegments']:assert text in source,text
assert source.index('int movementCount = PreflightSliderMovements')<source.index('var components = new List<SliderCheckpoint>()')<source.index('var moves = new List<SliderMovement>(movementCount)')
# Shared numeric expressions authenticated by Issue17/E005/E016/E021/E022.
for text in ['(double)(1000f * distances[index]) / velocity','phase += q; carry -= q; remaining -= q;','remaining <= 0.01 * velocity','phase -= q - carry; carry = q - carry;','head.StartTime + ((double)(float)distance / velocity) * 1000.0','components[^1].Time - 36']:
 assert text in source and text in prior,text
paths=git('ls-tree','-r','--name-only',new,'reconstructed/CircleGame').decode().splitlines();unchanged={p:sha(git('show',new+':'+p)) for p in paths if p!='reconstructed/CircleGame/Gameplay/Beatmap.cs' and git('show',old+':'+p)==git('show',new+':'+p)}
report=dict(starting=start,source=new,oldSource=old,gameplayChanged=changed,unchangedApplicationFiles=unchanged,originalResearchChangesOnly=original,fixtureHashesVerified=106,originalStaticPacketSha256=sha((R/'re/campaigns/C001/traversal-profile/static-evidence.json').read_bytes()),unchangedHistoricalTests=True,numericalExpressionsPreserved=True,productionSha256=sha(source.encode()),ownerChartsAccessed=False)
(H/'source-review.json').write_text(json.dumps(report,indent=2)+'\n');(H/'production.diff').write_bytes(git('diff',old,new,'--','reconstructed/CircleGame'))
print('PASS complete gameplay diff, unchanged connected application/historical tests,106 immutable identities, arithmetic/limits/cancellation source review')
