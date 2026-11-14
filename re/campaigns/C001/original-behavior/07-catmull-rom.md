# 1. Result

| UEP-001 revision 2.1 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work 1 / Issue #3 finite Catmull-Rom original investigation |
| Status | VERIFIED CHECKPOINT — original-static research and reproducible mathematical handoff |
| Starting SHA / Exact inspected SHA | bbd59f0a4f709688fcb791abb0e5783649d952fe; latest remote main verified at start |
| Published SHA / Exact tested research | Introducing commit in Git history and final handoff; packet source hashes and results in ../catmull/verification.json |
| Objective / Supported profile | Explicit C marker, fresh ordinary standard Play; finite normalized coordinates, positive requested length, fixed sampling and inherited prepared geometry. v5/v9/v14 geometry controls only |
| Functional changes / Integrations | Research packet, 19 synthetic hashed fixtures, independent rational and projected expectations, Work 2 integration contract; no production changes |
| Evidence / Confidence | CONFIRMED ORIGINAL CIL sites and hashes; RECONSTRUCTED mechanism/rational mathematics; ENGINEERING binary32 projection/tolerance. No original runtime or production equivalence claim |
| Tests / Environment | Linux Python 3.12; dnfile 0.18.0, dncil 1.0.2, pefile 2024.8.26. 61 decisive static assertions, 8 selected bodies/regions, 13 inherited method-hash comparisons PASS. Twice-extracted packet identical; 19 exact fixtures and rational/projection/degenerate discriminators PASS; Python compilation PASS |
| CI / Artifact / NOT RUN | No new CI/build artifact. Original CLR/client, Work 2 C execution, Windows/full regression, GUI/audio/physical input and owner qualification NOT RUN; production unchanged and research-only gate authorized by Issue #3 |
| Coverage gained / Remaining | C recognition/control selection/polynomial/sample/flags/degenerate/shared output handoff established. Work 2 implementation and consumer integration, independent actual-source QA and runtime qualification remain |
| Discrepancies / Limitations / Blockers | C absent from current production enum/admission; finite implementation gap. Native x86 intermediate precision unknown; diagnostic is not an original numeric oracle. Broader limits below; no research blocker |
| Updated records / Reproduction | This report and ../catmull/; append-only status, coverage, dependency, discrepancy and re/contexts.md. Commands below |
| Next bounded task / Actor / Dependencies | Work 2 finite C integration using this packet plus E006/E032/E005/E016/E021 and accepted P/stacking contracts; then Work 1 independent actual-source QA |
| Subsystem | OPEN; C001 remains OPEN; no owner acceptance or comprehensive slider equivalence |

Pinned ZIP SHA256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`;
`osu!.exe` SHA256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.
Build identity remains pinned bytes, not authenticated release provenance. Read AGENTS,
standing instructions, Issue #3, C001 authorization/registers, historical E006/E032
and completed P/stacking research and QA. Current canonical L/B/P source inspected
at the starting SHA only. No production execution, full-EXE inventory, P/stacking
re-investigation or R17 mathematical change. Historical provenance correction in
bbd59f0 remains intact; no new Windows result is inferred from it.

```sh
python -m pip install dnfile==0.18.0 dncil==1.0.2 pefile==2024.8.26
python re/campaigns/C001/catmull/recover.py /path/osubackup.zip /tmp/c.json
python re/campaigns/C001/catmull/check.py /tmp/c.json
python re/campaigns/C001/catmull/build_fixtures.py --check
python -m py_compile re/campaigns/C001/catmull/*.py
```

# 2. What they found

## A. Recognition, normalization and dispatch

Offsets are hexadecimal, code-relative, excluding CIL header. Method body hashes,
signatures, RVAs, decisive sites, branches and arithmetic order are in
../catmull/static-evidence.json. Only eight selected bodies/regions are decoded;
no original binary, full disassembly, assets or proprietary source is redistributed.

CONFIRMED ORIGINAL: M28f7:16c9 loads decoder identifier2064560670, whose existing
C001 decoded string is `C`; 16d8 branches to1718, storing enum0 at1719. The curve
kind also starts at0 (166f–1670). Unknown one-character markers leave the current
kind, so case-sensitive recognition does NOT prove that lowercase/unknown markers
are rejected. Finite handoff admits explicit C only. Longer tokens enter existing
coordinate parsing; missing Y can reach row-error/rethrow, not invented recovery.

Reuse E006 normalization: controls parse Double → Int32 truncation → Single
(M28f7:176c/d,17ac/d); optional Y inversion precedes conversion. Source controls
are not universally clamped or deduplicated. Head coordinates have their separate
inherited clamp/truncation contract. M142d:00ad–011e creates a list containing head
when null, appends head when supplied list is empty, or inserts head at index0
when first point differs exactly (M4efc inequality). Equal first head is retained.
Repeated interior/trailing controls remain. No Bézier duplicate separator rule.

M144a:00b4 switch enum0 enters00ce. Unlike P, there is no count-based L/B fallback;
C with two controls is still C. Prepared kind F0be4 becomes0 at01ec–01ee, then
branches to shared chord-sum at089a. No format-version/config/editor field read
occurs in this C branch, and helper M4ee0 is pure coordinate arithmetic. v5/v9/v14
therefore have the same recovered C geometry mechanism for identical normalized
controls. This is not proof of full old-version loading, ordering or gameplay.

## B. Stencils, polynomial, operation order and sampling

Let P be normalized controls, N their count. For i=0..<N-1:

| Role | Original selection |
|---|---|
| p0 preceding | P[i-1] if i>0; otherwise P[i] (duplicate first control), 00d6–00fb |
| p1 start | P[i], 00fd–010a |
| p2 end | P[i+1], 010c–013e |
| p3 following | P[i+2] if present; otherwise p2+(p2-p1), 0140–0172 |

There is also a syntactic p2 fallback `p1+(p1-p0)` at011d–0128, unreachable under
the ordinary immutable-list loop i<N-1. Preserve the distinction between code
present and behavior reached; do not introduce a fallback classification from it.
Vector addition/subtraction M4efd/M4efe store each component as Single.

M4ee0 metadata name `CatmullRom`, signature
`000511aa0811aa0811aa0811aa0811aa080c`, computes t2=t*t then t3=t*t2.
For each scalar coordinate the recovered expression order is:

```text
0.5f * (((2f*p1 + (-p0+p2)*t)
         + (((2f*p0 - 5f*p1) + 4f*p2) - p3)*t2)
         + (((-p0 + 3f*p1) - 3f*p2) + p3)*t3)
```

X starts000d, stores Single F3109 at008c; Y starts0093, stores F310a at0112.
All constants/arguments and t2/t3 locals are Single-category. No Double conversion,
call, distance-based knot parameter, tension setting, epsilon or adaptive recursion
exists in this helper. The evidence arithmetic sites preserve each add/sub/mul/neg
position. Reassociation, fused expressions or a modern library spline can change
results. Static CIL does not determine the pinned x86 JIT's extended intermediate
precision. Native bit identity is NOT VERIFIED.

For j=0..<50 (0174–01bc), compute t=Single(j)/50f and
u=Single(j+1)/50f; call helper independently for both endpoints, create M110c
segment. Total raw piece count =50*(N-1) for N>=2. The final piece of EACH control
span receives F0a34=true (01be–01cd). M110c sets only endpoints; F0a35/IsLinear
remains false. There is no original-endpoint overwrite, no duplicate elimination,
no chord-distance resampling and no omission of t=1. Do not replace pairs by a
single vertex chain that silently forces continuity at separately evaluated knots.

In exact rational mathematics t0/t1 equal p1/p2. Under the declared per-operation
binary32 projection, diagnostic controls X=[0,10000001,10000003] give first span's
end10000000, next start10000001 and final10000004. This demonstrates why stores and
operation order matter; it is NOT a measured original-runtime mismatch or a safe
numeric acceptance oracle for huge coordinates.

## C. Shared length/output and degenerate boundaries

Reuse E006 and P's already-established common pass; no new C-specific length rule.
At089a–08d0 sum Single segment lengths into ordered Double G; segment distance is
M110d/end-start followed by Single vector length. Sum only each segment's distance,
not any gap between separately computed adjacent pieces. Requested L is distinct.
For ordinary G>0,L>0, excess=G-L; remove final piece of length d when
Double(d)<=excess+0.0001; subtract d and repeat. Otherwise, if end!=start,
replace final end by start+Normalize(end-start)*(d-Single(excess)), then stop.
Zero final direction blocks extension. Retained flags are not reclassified when
trimming removes a span's marked last piece. Recompute cumulative lengths and
actual adjusted length, not a forced L. Empty output bypasses prepared collection/
final endpoint-time writes; G0 skips positive-length adjustment, retaining zeros.
Existing configuration/zero-request/editor mutation branches are not newly admitted.

| Discriminator | Independent finite mathematical prediction |
|---|---|
| Two controls100→200, Y100 | 50 pieces; midpoint X143.75, not150; endpoint stencil is asymmetric |
| Nonuniform four controls | 150 pieces; middle midpoint(160,105.625), uniform parameterization |
| Interior duplicate B,B | Still150 pieces; middle midpoint(206.25,93.75), not discarded |
| Collinear backtrack100→104→102 | 100 pieces; projected G≈6.086273, not linear6; cubic overshoot retained |
| Three-turn | 100 pieces, projected G≈204.190878; L200 prepared end≈(200.081787,195.810120) |
| Long request300 | Final-chord extension to≈(199.070587,295.804596), not analytic final tangent |
| Final three identical controls | Last50 pieces zero; projected G≈114.810394; L300 extension blocked at(200,100) |
| All identical, singleton, bare C | 100 zero pieces vs zero pieces vs zero pieces; scheduling NOT VERIFIED |
| Tiny request0.00005 | All pieces removed by common tolerance; no invented scheduled object |
| Fractional controls | Truncation yields normalized(100,100),(101,100),(102,100);100 pieces |
| Head omitted / v5 / v9 / repeat2 | Same three-turn geometry; head insertion and separate shared traversal contracts |
| Missing Y | Row error; actual CLR exception identity/recovery execution NOT VERIFIED |

19 exact synthetic .osu files contain no media; hashes in ../catmull/manifest.json.
Independent Fraction basis weights check exact span endpoints/midpoints and sum1;
separate scalar projection generates all raw pairs/flags, adjusted pairs/cumulative
lengths. Ordinary small finite examples differ from rational samples by<1e-4;
that tolerance is ENGINEERING, not original equivalence. The large-coordinate
precision diagnostic is explicitly outside that tolerance and numerical oracle.
No expected original runtime values are invented for malformed/zero/empty scheduling.

## D. Work 2 dependencies and exclusions

Current Beatmap.cs SliderPathKind has Bezier1/Linear2/Perfect3, no Catmull0;
canonical marker admission lacks C. Extend enum/dispatch and preserve raw marker,
normalized controls, SourceKind and PreparedKind=Catmull. Emit segment pairs with
IsLinear=false and every50th original piece IsSubcurveEnd=true; route them into
existing shared length adjustment/cumulative representation. Avoid applying modern
B duplicate splitting, P fallbacks, extra endpoint forcing or arbitrary fitting.
Use explicit finite/cancellation/allocation guards as ENGINEERING: source loop has
50*(N-1) allocation with no recovered safe cap. Precision-safe admission and
zero/empty rejection may be narrower than original and must be disclosed.

Feed the same prepared object through E005/E016/E021 timing, tick carry, repeats,
reverse movement, rendering/follow/input, sound/health/NewCombo and stacking.
Timing is not per new C piece and tick phase must not restart at stencil boundaries.
Retain accepted stack translation order/source-prepared separation; final prepared
endpoint used by stacking comes from actual common adjusted output, not last raw
control. Repeat2 traverses back to its prepared start; do not rebuild curve per span.
No new consumer mathematics is inferred here. Work 2 should test actual consumer
identity and integration before Windows regression/build; Work 1 independently
verifies the resulting exact production afterwards.

Explicitly OPEN: original x86 JIT precision; NaN/Infinity/Int32 overflow, extreme
extrapolation/cancellation, allocation exhaustion and concurrent mutation; malformed
exception identity and skip/recovery caller behavior; zero/negative length and
configuration quantization; zero/empty downstream scheduling; editor reprepare/
control mutation; full <=5 version pipeline/equal-time ordering/modifiers and
nonstandard modes. No R17/Joined Bézier/owner star discrepancy changed. Full slider
compatibility, original-runtime equivalence and owner qualification remain unverified.

# 3. Pseudocode

RECONSTRUCTED from CONFIRMED ORIGINAL static evidence:

```text
recognize explicit C as kind0; apply inherited parser and constructor normalization
raw=[]
for i=0 .. normalized.count-2:
    p0=normalized[max(i-1,0)]; p1=normalized[i]; p2=normalized[i+1]
    p3=normalized[i+2] if available else p2+(p2-p1)
    for j=0 ..49:
        a=Catmull(p0,p1,p2,p3,Single(j)/50f)
        b=Catmull(p0,p1,p2,p3,Single(j+1)/50f)
        raw.add(segment(a,b,IsLinear=false,IsSubcurveEnd=(j==49)))
preparedKind=0
G=Double sum of Single lengths of individual raw pieces
join inherited requested-length trim/extension and cumulative output
existing shared traversal/tick/time/identity/stacking consumers use prepared pieces
```
