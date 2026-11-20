# C001 discrepancy register

Status: OPEN. None of these records authorizes star fitting or historical expectation changes.

| ID | Finding | Classification / evidence | Disposition |
|---|---|---|---|
| C001-D01 | QA15 owner reports 8 matching / 19 mismatching displays, no ohno lower; nearly-equal-angle/opposing-direction especially divergent | OWNER-REPORTED in campaign authorization; exact chart hashes, original executable and cache state uncorroborated | OPEN. Preserve QA15 CSV/manifest as historical. Obtain matched provenance; investigate preparation without assuming causality |
| C001-D02 | Catalog chooses spinner then slider then circle for combined bits; original M28f7 chooses circle then slider then spinner then hold | CONFIRMED STATIC source/original relationship; production execution pending | OPEN. Canonical classification must serve catalog/gameplay; historical gameplay currently rejects combinations |
| C001-D03 | Catalog requires exact first-line header; gameplay accepts leading blanks/header whitespace; star admission requires exact v14 | SOURCE INSPECTION, ENGINEERING profiles | OPEN. Distinguish numeric admission from shared document parsing |
| C001-D04 | Metadata parser M1bc1 preserves colons after first key separator; object loader M28f7 uses only first two colon-split cells | CONFIRMED STATIC; original-behavior/01-parser-boundaries.md | OPEN. Canonical representation must preserve stage differences where applicable |
| C001-D05 | Unknown sections retain the previous recognized section on inspected original readers | CONFIRMED STATIC Enum.Parse catch-before-assignment | OPEN. Current independent readers replace section with unknown text |
| C001-D06 | HP/CS/OD/AR defaults/version13 parsing/mode3 CS and order-sensitive AR inheritance exceed prototype coverage | CONFIRMED STATIC M1387/M1bc1 | OPEN. Work 2 must preserve field history and mode at parse time |
| C001-D07 | Original slider repeats reject >9000; prototype limits 16; original missing length starts0 and effective traversal max1 | CONFIRMED STATIC M28f7/M142d; current implementation limits are ENGINEERING | OPEN; do not merely raise limits without resource controls and preparation fidelity |
| C001-D08 | Retained original-runtime E020 cases never run | UNVERIFIED ORIGINAL-RUNTIME | OPEN owner gate; component checks do not close it |

## Independent Work2 checkpoint audit (333f6d1)

- C001-D09 OPEN: QA04 F01 historical v13-Bezier rejection fails after C001 admission expansion. Preserve original expectation and failed evidence; requires Headquarters disposition per authorization §10.
- C001-D10 OPEN: Windows C001 fixture hash fails, local115/115 passes. Suspected checkout line endings, not yet measured Windows raw bytes. Preserve exact fixtures; Work2 add explicit byte policy and rerun.
- C001-D11 VERIFIED COMPONENT differences:49-file independent corpus exposes44 field gaps across defaults, AR inheritance, clamping, unknown sections, invalid/mode-order and combined-type classification. See verification/02-production-audit.md and production-results.json. No original-runtime equivalence or mathematical fitting.

Work2 correction80f7ac1: D09 compatibility boundary restored via legacy Load versus
application/catalog LoadCanonical, without historical test edits. Local QA04 43/43
PASS; corrected Windows confirmation pending. D10 path-specific fixture attributes
added; exact Windows byte confirmation pending. Original44 semantic gaps unchanged
under actual canonical API; do not close them because legacy regression passes.

## Issue #1 original-derived stacking audit — 2026-11-14

C001-D12 OPEN / VERIFIED COMPONENT: production85ec7bf accepts all18 new finite
stacking charts but has17 mismatching prepared heads out of43. No stacking pass
exists at shared e6881e1; source review agrees with runtime audit. New independent
strict runner retains failure and returns1. B1–B3 research dependency is now answered
for normal >5 standard full-range/no-mod loading; production integration remains
pending. No star mismatch is explained by this result and no old failure is waived.

## C001-D12 follow-up — 2026-11-14 independent verification

Finite17/43 missing-displacement gaps at85ec7bf no longer reproduce at52c300a9:
unchanged strict43/43 PASS. Supplemental394/394 integration checks PASS. D12 is
RESOLVED for the tested finite profile, with historical failure files unchanged.
No new stacking discrepancy found. early-time-break still fails the same historical
and canonical HealthChart nonoverlap guard; no health acceptance for that fixture.
General C001/owner/runtime and all unrelated discrepancies remain OPEN.

## C001-D13 — Perfect-circle admission/preparation gap, 2026-11-14

OPEN: pinned original recognizes P and has exact-three circular sampling plus
count/collinear L/B fallbacks. Current canonical geometry uses Boolean L/B and
rejects P; source-inspected at b413f6f (no new production execution). Finite original
handoff/21 mathematical fixtures now published. No runtime equivalence or zero/
editor/exceptional closure. Sampled chord G differs from analytic arc length;
Work2 must not substitute requested length/circular extension or fitted math.

## C001-D13 finite resolution / C001-D14 provenance correction — 2026-11-14

D13 finite P preparation gap RESOLVED at a6c7dabc:independent1093/1093PASS across
17 finite admissions+4 engineering rejections. Full compatibility/runtime remains
OPEN; four exclusions/v9P4 dispatch-only oracle and nonmonotone final schedule
explicitly retained. No new actionable finite production mismatch found.

D14 documentation discrepancy CORROBORATED/CORRECTED BY APPENDED EVIDENCE:
Work2 Windows prose saysSDK8.0.427/runtime8.0.33 and windows.json sdk says8.0.427;
retrieved job43575301213/132388122215 selected8.0.428, actual result JSON8.0.34.
See perfect-circle/verification-a6c7dabc/windows-corroboration.json. No historical
rewrite, altered test result or production change. Local Linux QA is8.0.427/8.0.33.

## C001-D15 — finite C implementation gap — 2026-11-14

OPEN, SOURCE-INSPECTED at bbd59f0 (not production execution): canonical path-kind
and marker admission support L/B/P but not C. Pinned original C emits50 uniform
Catmull pieces per adjacent normalized pair, preserving duplicate spans and flags.
Finite handoff/19 independent mathematical fixtures now available in catmull/ and
original-behavior/07-catmull-rom.md. Work2 integration and independent QA required.
Native x86 arithmetic is NOT VERIFIED; projected large-coordinate knot gaps are a
precision diagnostic, not an observed original-runtime discrepancy. Prior D14,
Joined Bézier and19 owner star discrepancies preserved without revision.

Issue#3 finite C gap follow-up: production2d776dac admits13finite corpus controls
and rejects6explicit engineering cases;4375development and exact Windows gate pass.
No new development discrepancy retained after correcting test-only positive-stack
object order. Original research/test expectations unchanged. Independent C verdict
and original runtime remain pending; no broad C001 discrepancy closure claimed.

C001 checkpoint08 C reference QA: exact2d776dac,5782/5782PASS (13admissions,
6engineering exclusions),retained P1093PASS. No new finite discrepancy; original
19fixtures unchanged. Same session previously implemented C; separate-reviewer
authorship NOT VERIFIED and remains an acceptance dependency. See verification/
08-catmull-production.md. Original/owner runtime and broader C001 remain OPEN.

## Checkpoint09 — separate-reviewer finite C assessment — 2026-11-14

Fresh reviewer authored neither production nor prior verifier. Exact2d776dac source
review +22/22 focused actual-production checks PASS, including independent Hermite
endpoints, signed/duplicate/trim/engineering boundaries and fixed three-traversal
checkpoint schedule. No finite defect reproduced; prior5782 result seal and original
19fixture packet/source identities verified;13admissions/6engineering exclusions
unchanged. See verification/09-catmull-fresh-review.md and catmull/FreshReview/.
Separate-reviewer gate now has independent evidence; Headquarters/owner acceptance
is not implied. Earlier continuity limitation remains historical. Original native
runtime/GUI/audio/physical input/owner and broader compatibility remain NOT VERIFIED.
No production changes or Windows/full rerun; C001 and subsystem OPEN.

Issue#5 independent QA checkpoint10 (2026-11-14): exact8c5ba835,9/10 focused
assertions PASS/1FAIL. Stable malformed CircleSize containing "changed since
indexing" is falsely reported changed-input/fingerprint-changed. Actual hashes and
bytes unchanged; two default JSON scans deterministic. See verification/10-corpus-
audit-diagnostic.md and corpus-audit-qa/. Work2 taxonomy correction required before
Issue#5 closure; remaining independent scope pending. No production edits; C001 OPEN.


## Issue #5 correction independent checkpoint 11 — 2026-11-15

Exact corrected2369b406 independently verified: versioned profile10/10 and focused
scan24/24 PASS; clean source and eight linked file hashes enforced. Historical
probe/fixtures/results, including obsolete-pin9/10, preserved. Real byte and
timestamp-only mutation, diagnostic stages/cohorts/counts/duplicates/privacy,
determinism, blocked reads/isolation, cancellation and finite limits checked.
Initial new harness22/23 preserved with documented conservative-classification
expectation correction; no production edit or new actionable defect. See
verification/11-corpus-audit-correction.md and corpus-audit-qa-v2/. Windows/Linux
CI43618348530 SUCCESS corroborated;259/platform reused, not rerun. Independent
tool gate ready for Headquarters review; Issue#5 remains OPEN pending acceptance,
owner corpus NOT PROVIDED/NOT RUN, Issue#4 separate, C001/subsystem OPEN.

## Issue #7 — old-spinner gates and format-family ambiguity — 2026-11-15

OPEN implementation gap, exact782c007: integer Mode0 v5–v13 spinner rows rejected
by canonical version!=14 and catalog object/format admission, while pinned original
ordinary factory/parser has no such guard. Finite static handoff now available;
source-policy and host-length prerequisites explicitly conditional. Not fixed here.

Diagnostic limitation CONFIRMED: fractional starts stop at earlier Int32 time/type
syntax message containing format-v14 and also map to unsupported/format-profile.
Thus owner aggregate134/standard candidates113 does not prove one exception source
or a guaranteed recovery count. Raw source v3/v128 and other modes remain separate
engineering exclusions. No new owner-chart execution or original runtime claim.

Historical E036/E041 checker compatibility issue: full-tree runs crash on five
later list-valued summary JSONs; preserved unchanged. Byte-identical checker/tool/
dictionary-evidence envelope passes original semantic assertions and349/466hashes;
new packet independently authenticates selected16bodies. No historical file edited.


## Issue #7 independent finite spinner checkpoint 12 — 2026-11-15

Work1 starting55fdfb0, exact tested2a2e291: independently authored548/548 actual
production assertions PASS,23 immutable fixture admissions/10 finite exclusions.
33 hashes preserved; exact clean source/nine compiled identities and binary linkage
verified. Static packet consistency62sites/40inherited identities PASS, no extraction.
Constructor/parser flags, samples, source version, fresh score/health/NewCombo/session,
modern first angle, results, end-time sound and recording hooks verified. Additional
v7/v8/v11 and catalog/default0.7/break/order/overlap/sample/timing guards challenged.
Host cap remains explicit ENGINEERING uncapped, original L4000/E3950 NOT EXECUTED.
Initial compile/resource harness failures preserved; no production discrepancy found.
Windows run43711842075 exact source SUCCESS; fetched job's19 historical summaries
2309PASS/0FAIL and source/packet/artifact identities corroborated, not rerun.
See verification/12-versioned-spinner-production.md and versioned-spinner-qa/.
Independent gate returned to Headquarters for acceptance; Issue#7 remains OPEN,
owner rescan only after acceptance. Issue#4/original/runtime separate; C001 OPEN.


## Issue #9 — legacy spinner stacking/timing research checkpoint — 2026-11-15

Work1 starting ee4a335, exact production 2a2e291 clean. Targeted original-static
recovery55sites/13bodies/10inherited identities PASS; repeated final packet identical.
Ordinary spinner M0603 position override is ret: internal height need not move center.
<=5 forward pass includes spinner comparisons; >5 skips anchors/candidates. Early
slider getter F0bfd geometric end differs from modern final traversal endpoint.
81 independent hashed fixtures;775/775 actual-production characterization assertions
PASS:26 preparations/55 rejections,40 temporary zero-setting diagnostic controls.
Defaults/nonzero settings are not computed stacks; audit labels are not throw sites.
Finite Work2 handoff: reuse modern CS5/AR5 stacking, conservative zero-displacement
certificate for other CS/AR/v5, retain unsupported early actual stacks/breaks/order/
overlap; prove six/seven-column explicit-sample timing defaults only. Two-to-five
fields retain caller/global/error dependencies. No production edits or compatibility
gain claim; owner113 matched candidates had zero preparations per HQ update.
See original-behavior/11-legacy-spinner-stacking-timing.md and legacy-spinner-profile/.
Prior33 fixtures/548 seals checked unchanged;2309 Windows baseline preserved, no
full rerun. Original/runtime/GUI/audio/physical input/owner maps/host cap NOT RUN.
Next Headquarters authorizes Work2 bounded integration and Windows gate, then
independent new-source QA. Issue#9/C001/subsystem OPEN; Issue#4 separate.
Headquarters Issue9 refinement records Issue7/8 CLOSED; earlier checkpoint
entries remain historical. No additional owner actions are part of this research.


## Issue #9 independent integration verification — 2026-11-16

Work1 starting7564b0e, exact tested9ed9eb5 clean:1344/1344 independent assertions
PASS;81 immutable Issue9 cases66/15; prior33 cases23/10. Nine compiled identities
and linked binary verified. RouteM displacement/geometry/consumer ownership, fixed
spinner center with source-derived internal correction, RouteZ strict numeric
boundaries/both endpoints/all pairs/1m work/cancellation, six/seven timing, stored-end
samples/source order, score/calibrated health/NewCombo/session/input/resources and
recording hooks checked. No actionable finite discrepancy found. Supersession review
restores archived QA14 exactly except authorized change+3new controls; Issue7 only
authorized block changed. Historical sources/775/548/fixtures/failures unchanged.
Actual Windows ZIP/EXE/source marker/801+656 packet seals checked; fetched exact-source
run43813166021/job13311914918651steps SUCCESS;19 historical summaries2312PASS/0FAIL.
Initial CRLF162 failures and -text correction preserved; no repeated full regression.
See verification/13-legacy-spinner-integration.md and legacy-spinner-qa/. No production
edits; no private owner collection, original runtime, GUI/audible/physical input or
host cap qualification. Next Headquarters acceptance review/potential Issue9 closure;
no owner scan requested now. C001/Issue9/subsystem OPEN; Issue4 separate.


## Issue #11 independent Route Z diagnostics — 2026-11-20

Work1 starting300d76e, exact testedf6d0411 clean. Entire CircleGame tree41files
identical to9ed9eb5 (2075c2a7dd9816e6e330db44da2682bf8b4fe9a8); only two exact
InvalidDataException mappings change tool behavior. Independent89/89 focused
assertions,10/10 new source/hash profile,20/20 actual CI portable-DLL fixture
comparisons PASS. Original Issue5 probe unchanged9/10 exit1 solely obsolete8c5 pin;
not waived or relabeled. Exact real unproven/pair-limit codes/types,1414/1415
boundaries, accepted M/Z controls, metadata/type/message adversaries, byte/stamp/
missing-file precedence, privacy/determinism/read-only/stages verified. Three actual
artifact ZIP/source markers/full EXE/result packets checked; focused native295
per platform and full Windows2312/2312 from fetched CI corroborated, not rerun.
No production edit, gameplay gain or owner33 attribution. See verification/14-route-z-
audit-diagnostics.md and route-z-audit-qa/. Historical sources/fixtures/results
preserved. Next Headquarters final acceptance review/potential Issue11 closure;
no owner action requested. Issue11/C001/subsystem OPEN; Issue4 runtime separate.


## Issue #13 — 2026-11-20

Current RouteZ conservatively refuses12 newly specified circle-free v5 spinner-first
no-ordinary-displacement synthetic positives. This is an ENGINEERING admission gap,
not a supported-production regression; no code correction made. Geometry-only
exemption for all earlier spinners is rejected as insufficient: CONFIRMED ORIGINAL
M0ad9 can consume height for Normal1 selected objects. Whether any private chart
exhibits a resulting runtime difference is NOT VERIFIED. No original-runtime defect
or owner31 recoveries asserted. Report13/handoff retains circle-bearing exclusions.
