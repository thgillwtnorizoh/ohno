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
