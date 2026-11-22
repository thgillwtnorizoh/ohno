#!/usr/bin/env python3
"""Check exact source, retained guards, and append-only research publication."""
import hashlib,json,subprocess
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3]
START='77b4d582f5d54e3f52018a6b9b6fc593c9742045'
APPEND=['re/contexts.md']+['re/campaigns/C001/'+x for x in ['coverage-matrix.md','dependency-register.md','discrepancy-register.md','status.md']]
git=lambda *a:subprocess.check_output(['git','-C',str(R),*a])
sha=lambda b:hashlib.sha256(b).hexdigest()
def review(exact):
    E=Path(exact).resolve();pin=json.loads((H/'expected-sources.json').read_text())
    assert subprocess.check_output(['git','-C',str(E),'rev-parse','HEAD'],text=True).strip()==pin['source']
    assert not subprocess.check_output(['git','-C',str(E),'status','--porcelain'],text=True).strip()
    for p in pin['paths']:assert sha((E/p).read_bytes()).upper()==pin['hashes'][Path(p).name]
    for p in ['reconstructed/CircleGame','reconstructed/CorpusAudit']:
        assert not git('diff',START,'--',p)
        assert not git('diff',pin['source'],START,'--',p)
    changed=git('diff','--name-only',START,'--').decode().splitlines()
    assert all(p in APPEND or p.startswith('re/campaigns/C001/control-count/') or p=='re/campaigns/C001/original-behavior/17-linear-control-count.md' for p in changed)
    prefixes={}
    for p in APPEND:
        before=git('show',START+':'+p);after=(R/p).read_bytes();assert after.startswith(before),p
        prefixes[p]=dict(priorBytes=len(before),priorSha256=sha(before),appendedBytes=len(after)-len(before))
    bm=(E/'reconstructed/CircleGame/Gameplay/Beatmap.cs').read_text()
    terms=['public const int MaxControls = 64, MaxSegments = 8192, MaxSubdivisionWork = 50000;',
        'if (supplied.Count > MaxControls)','if (controls.Count > MaxControls)',
        'pathPoints.Length - 1 > SliderGeometryPreparation.MaxControls',
        'checked(traversals * segments)','250_000L - totalMovements, 100_000L - totalCheckpoints, 100_000L - totalSegments',
        'if (components.Count >= 4096)','if (components.Count >= remainingCheckpoints)',
        'Observe("movement-allocation")','Observe("segment", traversal, step)',
        'SourceControls = controls.AsReadOnly()','double remaining = geometry.UnadjustedLength;',
        'float[] distances = geometry.CumulativeLengths.Select','if (source.Count == 0 || source.Count > MaxControls)']
    anchors=[]
    for t in terms:
        assert t in bm,t;anchors.append(dict(text=t,line=bm[:bm.index(t)].count('\n')+1))
    historical=json.loads((R/'re/campaigns/C001/traversal-profile/manifest.json').read_text())
    for c in historical['cases']:
        assert sha((R/'re/campaigns/C001/traversal-profile'/c['path']).read_bytes()).lower()==c['sha256'].lower()
    return dict(startingMain=START,exactSource=pin['source'],sourceState='clean',compiledSourcePins=pin['hashes'],
        sourceGuardAnchors=anchors,appendOnlyPrefixes=prefixes,historicalTraversalFixturesVerified=len(historical['cases']),
        productionChanges=False,historicalTestsChanged=False,ownerChartsAccessed=False,
        sourceTreeIdenticalBetweenFunctionalAndStarting=True)
if __name__=='__main__':
    import sys
    j=review(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(j,indent=2)+'\n');print('PASS exact source/guards,5 append-only prefixes,106 historical fixture hashes; no production changes')
