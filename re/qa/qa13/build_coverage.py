"""Reassess E042 rows from QA13 observations, never from the R15 coverage assessment."""
import base64,gzip,hashlib,json
from pathlib import Path
QA=Path(__file__).resolve().parent;ROOT=QA.parents[2]
def read(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def packet(name,local):
    p=QA/'verification'/name
    return json.loads(gzip.decompress(base64.b64decode(p.read_bytes()))) if p.exists() else read(ROOT.parent/local)
def build():
    original=read(ROOT/'re/evidence/42/rank-results.json')
    head=packet('windows-results.json.gz.b64','qa13-local-final2.json')
    win=packet('windows-widget-results.json.gz.b64','qa13-windows-final.json')
    cases={c['id']:c for c in head['cases']+win['cases']}
    vectors={v['id']:v for v in read(QA/'Inputs/grade-vectors.json')['cases']}
    # Own conservative assessment. Finite means declared model variant, never native equivalence.
    mapping={
    27:('FINITE',['G/independent-valid'],'Grade unchanged when validity alone changes; persistence omitted.'),
    28:('PARTIAL',['G/independent-perfect','W11 replacement presentation frozen fields'],'Grade independence and engineering perfect text executed; native ranking-perfect resource not loaded.'),
    29:('FINITE',['G/uncached-success'],'Fresh second getter returns F after success mutation.'),
    30:('DESCRIPTION CONFLICT',['G/R30-independent-conflict'],'Original descriptive A is preserved; independently derived and executed binary32 result B5.'),
    31:('FINITE',['I/global-replacement-before-selector','I/replacement-during-cleanup'],'Distinct handler/global mutation and delayed selected-record binding with same map identity.'),
    32:('PARTIAL',['I/fresh-alias'],'Fresh alias executed; original virtual M2b62 reset of an existing global record not implemented.'),
    33:('PARTIAL',['T/deferred-resubmit'],'Bounded deferred outcome and explicit resubmission executed; native M46e6 queue/clock guards unsupported.'),
    34:('PARTIAL',['T/deferred-resubmit'],'Fresh guard admission sets pending7/phase0; original queue callback itself unavailable.'),
    35:('FINITE',['W02 admitted not presented','T/duplicate'],'Accepted pending Rank retains current Play and no presentation.'),
    36:('SOURCE INSPECTION',[],'RankTransitions.Commit explicitly returns false for null Pending; original Unknown10 mapping is engineering and not a dedicated QA13 executed assertion.'),
    37:('PARTIAL',['T/commit-before-cleanup','W03 commit cleanup first frame','W04 initialize second frame'],'Actual supported synchronous cleanup before binding; original registration/container/fade helpers not reproduced.'),
    38:('PARTIAL',['I/deferred-cleanup-live-vs-captured'],'Explicit deferred artificial action changes live record after capture; no original worker/fade scheduler execution.'),
    39:('FINITE',['T/new-pending-survives'],'Different reentrant pending request retained after initialization.'),
    40:('FINITE',['I/replacement-during-cleanup'],'Current global replacement selected after cleanup, not request-time snapshot.'),
    41:('FINITE',['W04 initialize second frame','I/live-session-ledger'],'Actual SessionRankRecord retains session/ledger identity; no new authoritative ledger copy.'),
    42:('PARTIAL',['I/complete-flags-not-score','W11 replacement presentation frozen fields'],'Invalidity does not change grade; engineering WPF invalid text verified. Original persistence predicates unsupported.'),
    43:('PARTIAL',['I/null-at-selector','I/null-at-initialization'],'R15 rejects missing record as unavailable; original queued Menu0 path intentionally not reproduced.'),
    44:('UNSUPPORTED',[],'Source-only historical record branch and its null dereference are excluded from fresh policy.'),
    45:('PARTIAL',['I/map-mismatch','V/missing-map'],'Bounded missing/mismatched map rejection; original hash resolver/Menu scheduling unsupported.'),
    46:('UNSUPPORTED',[],'Original selected-map hash resolution/restore not reproduced.'),
    47:('UNSUPPORTED',[],'No public M32d7 null-record builder analogue; unshown unavailable panel is an engineering substitute, not this early-return path.'),
    48:('PARTIAL',['V/display-override-only','I/live-session-ledger'],'Actual record getter supplies text; original once-before-widget-cleanup call order source-inspected, not instrumented for getter count.'),
    49:('FINITE',['V/score-format-0'],'Minimum eight-digit score0.'),
    50:('FINITE',['V/score-format-123456789'],'Large score retained, no truncation.'),
    51:('FINITE',['V/display-override-only'],'Explicit displayed123 leaves authoritative123456789 unchanged (finite variant of original999 input).'),
    52:('UNSUPPORTED',[],'Alternate record/mode3 seven-digit policy excluded; ScoreV2 admission is separately tested.'),
    53:('FINITE',['V/accuracy-1-0-0-0'],'Single accuracy100f rendered invariant100.00%.'),
    54:('FINITE',['V/accuracy-1-1-0-0'],'Single400/600 percent rendered66.67% with explicit engineering culture.'),
    55:('FINITE',['G/E042-R11','V/accuracy-0-0-0-0'],'Empty artificial successful record gradeD and accuracy100%; no playable empty-map claim.'),
    56:('PARTIAL',['V/counters-maxcombo'],'Four whole counters and maximum executed; original extra decorated counters4/5 have no supported source and are omitted.'),
    57:('PARTIAL',['V/resource-X','V/third-grade-read'],'Resource-key operands/repeated reads executed; native stage7 texture/sprite construction unsupported.'),
    58:('PARTIAL',['V/resource-D','V/resource-F'],'First key and no secondary request executed; F texture availability not inferred.'),
    59:('FINITE',['V/second-grade-read','V/third-grade-read'],'Independent fresh grade/eligibility/key reads with intervening mutation executed.'),
    60:('UNSUPPORTED',[],'Native M451a texture availability/draw return boundary not executed or emulated.'),
    61:('PARTIAL',['W04 initialize second frame','W05 third frame permits view','W07 unshown view initially collapsed'],'Preparation/presentation stages executed without drawing; original GPU/container draw path unsupported.'),
    62:('PARTIAL',['R/prefix-1300','R/prefix-1800','R/prefix-5300','R/prefix-6300'],'Captured prefix arithmetic executed; deterministic dots replace original M2333 random digits.'),
    63:('PARTIAL',['R/skip-1300','R/skip-6299'],'Skip flag/host-7000/false return executed; native transform and overlay retiming unsupported.'),
    64:('PARTIAL',['R/skip-6300','R/skip-6301'],'False-argument later skip returns true; true-argument automatic original Back arm absent from public reveal API.'),
    65:('PARTIAL',['T/back-fresh-destination','W16 actual Back commits selection','A/keyboard-routing'],'Back5 request/commit and wiring executed/audited; original menuback audio/frontend unavailable.'),
    66:('PARTIAL',['T/retry-independent-skip-False','T/retry-independent-skip-True','W18 actual fresh Retry'],'Retry2 independent of reveal return and actual fresh session; original menuhit/busy/fade helper subset unsupported.'),
    67:('PARTIAL',['T/disposal-guards','W20 repeat host disposal'],'Engineering disposal clears global/reveal and retains aliases; graph/replay serialization/cleanup not reproduced.'),
    68:('UNSUPPORTED',[],'Pending7/14/17/18 original disposal alternatives outside deterministic R15 lifetime policy.'),
    69:('PARTIAL',['I/synchronous-success-mutation'],'Controlled cleanup changes selected grade X to F; original F1402/F136b eligibility helper chain not executed.'),
    70:('UNSUPPORTED',[],'Original Environment.Exit(-1) cleanup prefix intentionally not invoked/reproduced.'),
    71:('UNSUPPORTED',[],'Playback-source/player-name fill and its null exception excluded from fresh ordinary policy.'),
    72:('UNSUPPORTED',[],'Historical saved record restore/list replacement unsupported.'),
    73:('PARTIAL',['A/no-invented-statistics','V/map-fields'],'Basic result fields without graph/history supplied; original optional history statistics/graphs not reproduced.'),
    74:('PARTIAL',['T/initialized-idempotent-rejection','W10 hide and rebind same score widget'],'R15 rejects repeated session initialization and permits engineering view rebind; no original reentrant equivalence.'),
    75:('UNSUPPORTED',[],'History-majority applause and original player removal deliberately unimplemented; silent Rank audited.'),
    76:('FINITE',['C/count-mismatch-still-finalized'],'Actual count mismatch invalidates yet completes with success and live X grade.'),
    77:('FINITE',['C/gate-failure','I/failure-not-accuracy'],'Failed outer route blocks automatic Rank; artificial direct completion separately grades F.'),
    78:('UNSUPPORTED',[],'Original partial viewport/player/child graph has no production analogue; unshown WPF view construction does not test it.')}
    rows=[]
    for r in original['reference_cases']:
        i=int(r['id'][1:])
        if i<=26:
            classification='FINITE';refs=['G/E042-'+r['id']];note='Exact original finite operands, expected grade independently regenerated by rational binary32 oracle.'
            independent=vectors['E042-'+r['id']]['expected']
        else:
            classification,refs,note=mapping[i];independent=note
        actual=[]
        for ref in refs:
            assert ref in cases,ref
            c=cases[ref];actual.append(dict(case=ref,passed=c['passed'],classification=c.get('classification'),fixture=c.get('fixture'),observations=c['observations']))
        rows.append(dict(id=r['id'],originalEvidence=r['evidence'],originalInitial=r['initial_state'],historicalPrediction=r['expected'],
            independentExpected=independent,productionComponents='RankResults.cs; referenced QA13 case source and Windows PlayfieldView/RankResultsView where named',
            classification=classification,inputAndOutcome=actual,assessment=note,engineering='Stored binary32 .NET8 projection; scoped transition/record lifetime; invariant capture; WPF typography; silent results. See assessment for row-specific boundaries.',
            originalRuntime='UNVERIFIED ORIGINAL-RUNTIME',discrepancy='Historical R30 description A contradicted by its own retained instructions; production B agrees with independent binary32 oracle.' if i==30 else None))
    assert len(rows)==78
    counts={c:sum(r['classification']==c for r in rows) for c in sorted({r['classification'] for r in rows})}
    output=dict(schema='QA13.E042.coverage.v1',source=head['source'],matrixNotCaseCount=True,counts=counts,rows=rows)
    (QA/'e042-coverage.json').write_text(json.dumps(output,indent=2,ensure_ascii=False)+'\n',encoding='utf-8')
    print('QA13 independent E042 coverage',counts)
if __name__=='__main__':build()
