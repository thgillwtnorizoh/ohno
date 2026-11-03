# 1. Result

Expedition 032 — Ordinary Bézier Slider Geometry Preparation. Work 1,
2026-11-03 (Asia/Bangkok). Research only, based directly on canonical main
`2f4b439fe965f76dd3568c572b4bf90c364c3e54` (QA03). No R06 implementation,
application change, new CI gate or original-game execution.

Recovered the finite Bézier preparation chain, including its **format-version
branches**, repeated-point boundaries, adaptive flattening, older uniform
sampling, shared requested-length adjustment and prepared-geometry interface.
This is sufficient for a bounded format-v14 R06 implementation without choosing
an invented sampler. Versions outside that initial engineering scope should be
explicitly rejected or implemented using their separate original branches.

Read AGENTS.md, standing Instruction.md (identical to attached contract),
cumulative journal, E05/E06 and bounded E15/E16/E21 downstream evidence; inspected
R05 Beatmap/SliderDefinition/Movement preparation read-only. Historical ordinary
factory, update-order and timing corrections are preserved. Main is canonical;
old expedition branch notices do not govern this publication.

Reference SHA-256, checked against actual bytes by extraction:

- ZIP: `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`.
- osu!.exe: `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.

Stable40/file-version1.3.3.8 remains a self-description, not official dated-build
or modification-status authentication. Python/Linux static inspection only.
Pinned Python metadata-reader dependencies were installed in `/tmp/ohno-static32`;
no original CLR, debugger or Windows application was run. E20 remains incomplete;
all17 original-runtime cases **NOT RUN — WINDOWS HOST REQUIRED**.

Added this report, `tools/bezier_geometry_evidence.py`,
`tools/check_bezier_geometry.py`, `evidence/32/bezier-geometry.json`, and one
append-only journal entry. Evidence contains22 selected method identities,
signatures, owners, field signatures/access sites, body hashes, branch targets,
constants and arithmetic/conversion sites. It does not publish a full disassembly,
original executable, assets or private dump. Field inventories are expressly
**scoped to those direct bodies**, not a new whole-assembly scan.

Actual checks: two byte-identical extractions from the archive; focused checker
PASS; all11 selected bodies shared with E06 match historical hashes; existing
E05 timing and E06 geometry evidence checkers PASS; new-tool Python compilation,
whitespace, exact historical blob/mode and journal-prefix preservation checked.
No E01–E31 campaign or original-runtime test was repeated. Publication SHA and
verified remote parent/tree are supplied in the final handoff.

Reproduce from repository root (same dependency versions as E06: dnfile0.18.0,
pefile2024.8.26, dncil1.0.2):

```sh
PYTHONPATH=/path/deps python re/tools/bezier_geometry_evidence.py /private/osubackup.zip > /tmp/e32-a.json
PYTHONPATH=/path/deps python re/tools/bezier_geometry_evidence.py /private/osubackup.zip > /tmp/e32-b.json
cmp /tmp/e32-a.json /tmp/e32-b.json
python re/tools/check_bezier_geometry.py /tmp/e32-a.json
python re/tools/check_slider_timing.py re/evidence/05/slider-timing.json
python re/tools/check_linear_geometry.py re/evidence/06/linear-geometry.json
```

# 2. What they found

## Player-level consequence and confidence

**RECONSTRUCTED from CONFIRMED static instructions:** the curved path followed by
an ordinary slider is an ordered chain of finite straight pieces. For modern
format controls, stable repeatedly halves a Bézier until a second-difference
criterion passes, then emits a particular weighted approximation. It does not
simply evaluate the mathematical curve at equally spaced times. Repeated controls
can end one subcurve and begin another. The declared pixel length then trims or
extends the last pieces; duration and tracking consume that prepared path.

All instruction claims below are **CONFIRMED static**. Algorithm names,
player-facing meaning and finite predictions are **RECONSTRUCTED**. No original
runtime, visible curve, native precision or human-play equivalence is established.
There is no need for a community/lazer sampler or an unverified hypothesis.

Abbreviations Mxxxx=MethodDef0600xxxx, Fxxxx=FieldDef0400xxxx,
Txxxx=TypeDef0200xxxx. Offsets are hexadecimal **CIL-relative**, header excluded.
Exact signatures, RVAs, complete body hashes and referenced member identities are
in the evidence JSON. Newly resolved helper identities:

| Method | Owner / role | Signature hex |
|---|---|---|
| M2d62 | T05e6; construct flattener then obtain list |0001151282c10111aa08151282c10111aa08|
| M2d64 | T05e6; uniform old sampler |0001151282c10111aa08151282c10111aa08|
| M2d8f | T05e7 constructor |200101151282c10111aa08|
| M2d90 | T05e7 static flatness test |0001021d11aa08|
| M2d91 | T05e7 half-subdivision |2003011d11aa081d11aa081d11aa08|
| M2d92 | T05e7 flat-leaf approximation |2002011d11aa08151282c10111aa08|
| M2d93 | T05e7 iterative traversal / output |2000151282c10111aa08|

The vector value type is T0a82; X/Y Single fields F3109/F310a. Segment constructor
M110c/T024c stores Vector2 endpoints F0a32/F0a33 (declaring type is independently
recorded in JSON). T05e7 stores source list F1aa7, Int32 count F1aa6,
Vector2[count] scratch F1aa8 and Vector2[2*count−1] buffer F1aa9 at M2d8f:0008,
0014,0025,003a respectively. It retains the list reference. M2d90 has no state
writes; M2d91/92 write array elements, M2d93 mutates local stacks/output list.
No gameplay judgement or result field is read/written in those helpers.

## A. Inputs and exact dispatch / separator rules

Correct ordinary construction remains E05 `M113e→M0651→M48de→M48e0→M142d`,
T02af. E06 constructor normalization retains a supplied list reference, appends
head if empty, or inserts head at0 if unequal. Parser controls undergo Double
parse→Int32→Single, with optional Y flip before conversion; no new general
control clamp, deduplication or collinearity filter is added. Fresh cases here
have F0be9=false; M144a:0075–00a5's separate editor/reprepare point mutation
remains outside this fresh-loading control.

M144a:00b4 switch targets [00ce,01f8,07a2,056a]; original enum T03bb Bezier=1
therefore selects01f8. `F152f.F17ee.F0fd1` supplies format version, not curve degree.
M144a keeps a subcurve start index and scans i from0 through count−1.

| Version | Boundary and helper selection |
|---|---|
| >8 | At0218–024a, separator is `i<count−2 && P[i]==P[i+1]` (exact vector equality M4efb); emit when separator or i==count−1. GetRange(start,i−start+1) at0271. Two controls: one linear piece at0285–031d, F0a35=true. Otherwise version9 calls M2d64 at033a; >=10 calls M2d62 at037a. |
| 7–8 | Same forward-adjacent separator predicate at03f0–0436; no two-control special branch; GetRange then M2d62 at044e. |
| <=6 | At04ae–04e1 emit when `(i>0 && P[i]==P[i−1]) || i==last`; GetRange(start,i−start+1) then M2d62 at04f6. No additional skip. |

For >6, after an emitted separator range, increment i once before assigning the
new start (03ca–03d3 /049e–04a7), then the ordinary loop increments it again.
Thus `[A,B,C,C,D,E]` emits `[A,B,C]` and `[C,D,E]`, beginning the second range at
the second C. This is not global duplicate removal. The `<count−2` guard means
an equal **last pair** is not a forward separator. For <=6 the boundary includes
the second duplicate in the preceding range, then uses that same index as the
next start. Do not apply the modern rule retroactively.

After each range, the last produced segment gets F0a34=true at03c5/0499/0541.
Bezier branch writes F0be4=1 at0560 and joins the common length pass at089a.
Flags are preserved as metadata; this report does not invent rendering meanings.

## B. Adaptive flattening (modern v14 control)

M2d62:0001 constructs T05e7 through M2d8f;0006 calls M2d93. No fixed sampling
count, maximum depth or requested-length-dependent flatness is read here.

**Flatness M2d90:** for i=1..n−2, form
`d = (P[i−1] − 2f*P[i]) + P[i+1]` in that order (0004–0031).
M4eca:0000–001b returns Single X*X+Y*Y. At0039–003e compare it against
**0.25f**, `ble.un.s→0042`; anything strictly greater rejects the leaf, equality
passes. Unordered also passes that branch; finite cases alone are guaranteed
here. This is neither point-to-chord distance nor a maximum edge-length limit.

**Half split M2d91:** copies input to F1aa8 scratch (000b–0024). For each level,
store current first in left[level], current last in right[n−level−1]
(002a–0053), then replace scratch[j] by `(scratch[j]+scratch[j+1])/2f`
for j<n−level−1 (005c–0091). M4f03 implements division by first computing
Single reciprocal1f/2f, then multiplying components. Both children share the
midpoint, with original orientation retained. This is de Casteljau at one half,
not a parameter step chosen from requested length.

**Accepted leaf M2d92:** call M2d91 again into left buffer F1aa9 and right
scratch F1aa8 at0014. Copy right[1..n−1] into buffer[n..2n−2] at001d–0042.
The resulting buffer is left followed by right excluding its first midpoint.
Append input[0] at004c, then for i=1..n−2 append
`0.25f * ((buffer[2i−1] + 2f*buffer[2i]) + buffer[2i+1])`
(0055–0095). The final input endpoint is **not** appended by this leaf method.
These emitted points need not be exact mathematical Bézier values at uniformly
spaced t, even when their X coordinates look uniformly spaced.

**Traversal M2d93:** copy source to array, push to pending stack; a second stack
recycles n-element arrays. Pop, test flatness, approximate accepted leaf and pool
its array (0038–0057). Otherwise split into shared left scratch buffer and a
right array (0079–007f), copy left into the popped array (0088–00a2), push right
at00a7 then left at00ae. LIFO therefore processes left before right, preserving
path order. After pending stack empties, append source[n−1] once at00bf–00d3.
Empty source returns empty at000c–000f; singleton input yields two equal output
points (leaf first plus final endpoint), not an invented minimum-degree error.
The n=0 constructor's buffer allocation behavior is an earlier exceptional
boundary; the empty-method guard alone does not prove public construction safe.
No smoothing, resampling by arc length or output deduplication follows here.

## C. Uniform sampler and precision boundary

For the version9 non-two-control branch only, M2d64 sets steps=unchecked(50*n)
(0015–0019). For integer sample s starting0 while s<steps (0092–0096), it copies
controls, computes Single t=Single(s)/Single(steps), and performs in-place
successive interpolation using M4edb at0061. M4edb writes each component as
`a + (b−a)*t`, X then Y; output scratch[0] is appended. Exactly steps points are
returned, **excluding t==1**. M144a connects adjacent returned points without
adding the original endpoint. Shared requested-length extension may later change
the tail; do not silently repair this sampler by appending P[last].

All vectors are Single storage. Arithmetic helpers M4efd/M4efe/M4f01/M4f03 return
Single-component vector values; flatness squared length is Single. CIL floating
stack precision and original x86 JIT intermediate behavior remain runtime
uncertainties: this does not justify replacing the path with Double analytic
Bézier, or asserting portable bitwise identity from a Python reimplementation.
Integer array/index arithmetic is unchecked where no overflow opcode appears;
malformed/nonfinite or enormous controls can throw or fail to converge. No
hypothetical depth cap is attributed to the original.

## D. Prepared geometry and Work 2 interface

M144a pairs flattened vertices in order with `new M110c(prev,next)` at039b
(modern),035b (version9),046f/0517 (older branches). Then all curves rejoin the
**same** E06 tail pass; there is no Bézier-specific duration engine.

1. Sum each segment's Single M110d length converted to Double at08b1–08c1;
   store unadjusted total **F0bdd** at08d0. M110d→M4ec9 performs component squared
   sum, conversion to Double for Sqrt, then Single return. Keep original order.
2. For fresh positive requested length L and G>0, excess=G−L. Last segment d is
   removed if Double(d)<=excess+0.0001 (unordered also removes); subtract d from
   excess and repeat. Otherwise equal endpoints stop adjustment; nonzero direction
   sets end=start+Normalize(end−start)*(M110d()−Single(excess)). This can extend
   the last piece when excess<0. The shared L=0/reprepare quantization alternatives
   remain E05/E06 boundaries, not new Bézier rules.
3. If nonempty, store ordered segments to **F0bdc at0ae1**; create/clear
   **F0bde at0af6/0b03** and append Double cumulative sums of the **adjusted**
   Single segment lengths at0b28–0b37. Empty temporary output does not perform
   these assignments and returns before final end write; do not manufacture reset.
4. E05/E21 consume each cumulative difference as Single distance d; each directed
   segment becomes Movement with `dt=Double(1000f*d)/V`, advancing a Double
   absolute time and converting integer endpoints. Reverse traversal visits these
   same prepared segments backwards with endpoints swapped. E16/E21's checkpoint
   carry runs through segment distances; it is not a new equal-parameter curve
   sampling operation. The unadjusted G, requested L and adjusted cumulative total
   remain different values.

**Engineering recommendation, not original metadata:** R06 needs an ordered
prepared-segment collection (Single endpoints plus boundary flags), Double
cumulative lengths, unadjusted G, requested L and the source controls/version.
Current R05's single Tail and one Movement per traversal cannot represent a curve
by themselves. Its existing list of Movement records can consume multiple
prepared pieces per traversal; keep segment-level time conversion, carried
checkpoint phase and the same inclusive first-match selection. Do not replace a
curve with a straight head-to-tail distance or restart ticks per flattened edge.
No implementation change is made here.

## E. Small discriminating static controls

All cases: ordinary no-mod fresh object, normalized points as listed, format14,
one traversal, finite positive requested **L=2** (quadratic), **L=3** (cubic),
**L=4** (joined case). Coordinates are intentionally small integers to make
midpoint arithmetic exactly representable. They are supplied mathematical
controls, **not imported maps or executed original traces**. Vertices below are
**before the shared requested-length tail adjustment**, which changes only the
suffix. Pair each adjacent vertex; do not treat the table as final post-trim output.

| Control | Conditional raw flattening prediction |
|---|---|
| Quadratic A(0,0),B(1,1),C(2,0) | Root second-difference²4 rejects; half children have²0.25, equality accepts. Vertices [(0,0),(0.5,0.375),(1,0.5),(1.5,0.375),(2,0)], four segments. |
| Cubic (0,0),(1,1),(2,1),(3,0) | Root interior tests²1 reject; half children tests²0.0625 accept. Vertices [(0,0),(0.5,0.421875),(1,0.671875),(1.5,0.75),(2,0.671875),(2.5,0.421875),(3,0)], six segments. |
| Joined (0,0),(1,1),(2,0),(2,0),(3,-1),(4,0) | Separator at i2, ranges[0..2]/[3..5]. Two quadratic outputs joined: the five quadratic vertices above, then (2.5,-0.375),(3,-0.5),(3.5,-0.375),(4,0). Eight pieces, no extra bridge piece between the two equal C controls. |

Quadratic raw segment lengths: 0.625f, Single(Sqrt(0.265625)),
Single(Sqrt(0.265625)),0.625f. Raw G is their ordered Double sum (about2.2807764,
not an exact asserted original-x86 decimal). With L2 only the final piece is
shortened: let e=G−2, its new end is
`(1.5,.375)+Normalize((.5,-.375))*(.625f−Single(e))` with original vector helpers.
Recompute adjusted cumulative lengths from the resulting endpoints. Do not force
the final sum to exactly2 by assignment. Cubic and joined controls apply the same
suffix loop with their stated L; the first emitted vertex remains unchanged.

The cubic point (.5,.421875) differs from the analytic cubic at t=1/6 whose
Y=5/12 (~.4166667). This directly discriminates the weighted flat-leaf algorithm
from uniform mathematical evaluation. A version9 quadratic instead yields150
samples, ending at t=149f/150f before common adjustment; it must not reuse the
five-vertex adaptive expectation. A modern two-control range bypasses flattening;
a repeated final pair does not trigger the same separator as an interior pair.
These predictions are derived by the documented finite midpoint operations, not
by running a substitute curve engine. All original-runtime statuses are NOT RUN.

## Remaining uncertainty / smallest verification

The finite v14 path is recovered without an unresolved mathematical helper.
Runtime precision, actual import/control normalization, exceptional allocations,
nonfinite convergence, reused/editor mutation and later translated presentation
remain outside the bounded claim. No visible smoothness, pixel placement or
performance result is established. E20 packet and QA03 are unchanged.

For later owner-authorized validation, add only actual imported control/version
and type identity, each subrange/helper, accepted flatness operands, emitted
vertices, pre/post-tail segments, G/L/cumulative values and resulting Movement
records to the existing E20 trace checklist; capture original operands without
extra stateful calls and measure tracing overhead. Do not launch a game for E32.

The next bounded step is Work 2's R06 format-v14 Bézier preparation followed by
noninteractive geometry regression against these finite controls and existing
QA03 schedules. Original runtime validation remains separate and incomplete.

# 3. Pseudocode

**RECONSTRUCTED, non-executable description of CONFIRMED finite instructions:**

```text
preserve constructor normalization, source-list alias and format version
scan ranges using the version-specific duplicate rules above
for each emitted range:
    modern (>8) and exactly2 controls: emit one interpolated straight piece
    else version9: generate 50*n samples at Single(s)/Single(50*n), s<50*n
    else: adaptive helper
    connect adjacent emitted points; mark last piece as subcurve boundary
join common E06 length adjustment and prepared-collection storage

adaptive helper:
    pending stack <- copy of controls; pool empty
    while pending nonempty:
        p <- pop
        if all interior squared((p[i-1]-2f*p[i])+p[i+1]) <= .25f
           [original unordered branch also accepts]:
            split p at one half, preserving original Single-vector operations
            buffer <- left followed by right excluding first midpoint
            append p[0]
            for each interior i append .25f*((buffer[2i-1]+2f*buffer[2i])+buffer[2i+1])
            recycle p
        else:
            split p at one half into left and right, using original scratch arrays
            push right then left
    append original last control

shared output:
    retain unadjusted sum G separately
    shorten/remove/extend trailing pieces with original tolerance and zero guard
    store ordered segments and recomputed Double cumulative adjusted lengths
    existing per-segment movement/time/checkpoint loops consume that interface
```

**UNRESOLVED original-runtime precision/behavior is not replaced by an executable
reference engine.** No gameplay algorithm, QA03 or Work 2 code is modified.
