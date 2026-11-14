# 1. Result

C001 Work 1 bounded independent verification, 2026-11-14. Issue #1.
**ACCEPTED AS A FINITE VERIFIED COMPONENT CHECKPOINT**, not complete C001 or
owner/original-runtime acceptance. No new stacking discrepancy found in this cycle.
Exact tested production: **52c300a9b508567987331c54f51a0944f9c5b8fd**.
Publication parent:83dc9fe0be212ddcc127e329b0e2bdc326e8d031 (documentation only).
Production/old verifier/fixtures at that parent are byte-identical to tested52c300a9.

- Unchanged independent Work1 stacking runner, STRICT:18/18 fixtures load,
  **43/43 exposed prepared heads PASS,0 gaps**, exit0. Old17-gap baseline preserved.
- Additional independent integration checks: **394/394 PASS**, including raw bytes,
  translation invariants across every existing slider control/segment/movement/
  checkpoint and endpoints, retained timing/length/sample schedules, fresh owner,
  exact circle/resource/NewCombo/session identities, stable reloads and8 actual
  session input edge probes. Spinner has no exposed position; no fabricated check.
- Linux .NET SDK8.0.427/runtime8.0.33; Release build with TreatWarningsAsErrors.
  Exact-source detached worktree; no production changes. New integration helper
  supplied separately; tracked production remained clean throughout execution.
- Independently retrieved GitHub Windows run43540928098/job132281072732: exact
  head52c300a9, conclusion success; historical regression stages success; log
  confirms0 warnings/0 errors,264/264 Work2 development and106/106 finite fields
  plus4/4 engineering controls. This is corroboration of the existing Windows gate,
  not a newly executed Windows run of this added integration helper.
- No full historical rerun or new original research: neither is warranted for this
  verification-only publication. No original code execution, GUI, listening or owner
  qualification. No authorization exceptions or changed historical expectations.

Evidence:../stacking/verification-52c300a9/{heads.json,heads.log,integration.json,
integration.log,windows-corroboration.json}; reusable supplemental source:
../stacking/Integration/. Exact fixture bytes/generator and baseline results remain
unchanged. Full old research/journal prefixes are preserved by a tracked-file check.

Reproduce from an exact52c300a9 checkout, with the new Integration helper copied in:

```sh
dotnet run --project re/campaigns/C001/stacking/Headless/C001.Stacking.csproj -c Release -- "$PWD" /tmp/heads.json
dotnet run --project re/campaigns/C001/stacking/Integration/C001.StackingIntegration.csproj -c Release -- "$PWD" /tmp/integration.json
```

# 2. What they found

VERIFIED COMPONENT / SOURCE REVIEW: PrepareFiniteStandardStacking matches the
already-published fixed outer-kind/reverse moving-anchor rules, circle-end versus
slider-start time comparison, strict spatial/time boundaries, negative tail range
and Single displacement for its declared finite profile. No original re-extraction
or alternative copied stacking oracle was used. Expected heights are unchanged
Work1 fixture predictions.

The pass runs after slider preparation and admission, before fresh sound/health map
binding. Reference-equality replacement dictionaries remap changed circles/sliders
in definitions and sound/resource operands. Health/NewCombo uses the final fresh
map's definitions; session state retains those same objects. Source rows/raw flags
and original document bytes remain unchanged. Distinct-time ordering and existing
admission remain intact. Raw and prepared positions/heights are not newly exposed
as separate public fields; this checkpoint verifies retained raw document/segments
and observable prepared coordinates, not an unimplemented public height contract.

Slider translation changes public/prepared controls, adjusted segments, far tail,
final traversal endpoint, movements and checkpoints. RawSegments and all retained
length/time/traversal/sample-schedule operands remain invariant. Tests compare the
canonical prepared geometry with the same preserved historical unstacked geometry
plus a measured translation; that is a consumer/translation invariant, not proof of
independent original geometry correctness. Head predictions remain the independent
original-static contract. Reloading does not compound shifts.

Source consumption inspected in Rendering/PlayfieldView.cs: BuildPath consumes
prepared Geometry.Segments; ticks use checkpoint positions; ball uses BallPosition;
circles use session Definition.Position. CircleSession.Press and slider tracking
consume those prepared definitions/movements. Eight head-edge probes execute
CircleSession.Press at a point inside the prepared head radius and outside the raw
head radius, yielding Hit300. These are headless component tests, not automated GUI
playtests or physical input qualification.

Existing limitation retained: early-time-break fixture has an overlapping circle
inside a long slider. HealthChart.FromMap throws the same ordered-nonoverlapping
admission error for both historical and canonical loads. Initial supplemental
execution stopped on this error; verifier now explicitly compares both errors and
records the retained limitation. It does NOT count that chart as accepted health
integration. Health/NewCombo identity assertions execute for the other17 fixtures.
This is a pre-existing boundary, not a new stacking defect or an accepted waiver.

R17 admission/math and all files outside Beatmap.cs plus Work2's additive development
checks are unchanged by52c300a9. Canonical stacking is restricted to version>5,
standard, full-range, CS5/AR5, no-mod512x384 operands; legacy Load remains unstacked.
No equivalence claim for other CS/AR, viewport/mods, <=5, ties, partial-editor calls,
alternate modes or exceptional numerics.19 owner star differences, Joined Bezier
and full C001 acceptance remain OPEN. Final owner comparison remains mandatory.

Supplemental verifier construction: initial compile failed due to an extra closing
parenthesis in result serialization; corrected before execution. This test-tool
failure and the subsequent retained health admission classification are disclosed;
no production patch or changed fixture expectation was needed.

# 3. Pseudocode

RECONSTRUCTED original algorithm already published; VERIFIED COMPONENT integration:

```text
load original bytes and prepare slider geometry/timing
if supported finite canonical profile:
    discover heights using original-derived reverse traversal
    preparedHead = rawHead - Single(height) * (radius/10, radius/10)
    translate each changed slider's prepared geometry by head delta
    replace changed definitions and remap identity-bound sound operands
bind fresh health/map owner
session/render/input consumers use the final prepared definitions
```

The supplemental verifier does not implement stack discovery: it consumes existing
independent expected heads, checks translation/identity invariants, and exercises
actual session input. Original runtime remains UNVERIFIED ORIGINAL-RUNTIME.
