# 1. Result

| Checkpoint field | Record |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work1 / bounded Perfect-circle slider investigation, UEP-001 rev2.1 |
| Status | VERIFIED CHECKPOINT — research and independent mathematical fixtures; implementation pending |
| Starting SHA / Exact inspected production | b413f6fdd0fa363be4e4c69572e3a7375c8bf4e5; remote main verified, no newer checkpoint at start |
| Published SHA | This file's introducing commit, recorded by Git history and final handoff |
| Objective / Supported profile | Fresh ordinary Play P-path dispatch; finite normalized controls, finite positive requested length; exact-three circle construction and shared geometry output. v5/v9/v14 controls, not full legacy compatibility |
| Functional changes | Research tools, 21 byte-exact fixtures, mathematical expectations and handoff only; no Work2 code changed |
| Evidence / Confidence | CONFIRMED ORIGINAL static methods/sites; RECONSTRUCTED finite mathematics. No production/runtime equivalence claim |
| Tests / Environment | Python3.12 Linux; dnfile0.18.0/dncil1.0.2/pefile2024.8.26;73 decisive original-site assertions,15 selected bodies,20 inherited hash comparisons PASS;21 fixture bytes and finite discriminators PASS; repeated extraction identical; inherited E006/E032 stored-evidence checkers PASS |
| NOT RUN / CI | Original CLR/client, production P path, Windows/gameplay/render/audio/input, owner qualification NOT RUN. No new CI/artifact/full regression; production unchanged |
| Coverage gained / Remaining | P dispatch, circumcircle/angles/sampling/fallback/length rules and finite controls recovered; Work2 implementation/independent production QA/runtime and listed exceptional/editor boundaries remain |
| Discrepancies / Limitations / Blockers | P currently rejected by canonical gameplay; original x86 intermediate/trig precision not measured; zero-request/configuration/editor/error boundaries explicitly unresolved below |
| Updated records | This report; ../perfect-circle/ tools/evidence/fixtures/manifest; append-only campaign status/coverage/dependency/discrepancy and re/contexts.md |
| Reproduction | Commands below; reference archive supplied separately |
| Next bounded task / Actor / Dependencies | Work2 finite P integration using this handoff and shared E006/E032/E005/E016/E021 contracts; then Work1 independent actual-source QA |
| Subsystem | OPEN; no owner acceptance or comprehensive slider compatibility |

Pinned ZIP SHA256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`;
EXE SHA256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.
Stable40/file-version1.3.3.8 remains self-declared metadata, not release authentication.
Read standing rules/C001 authorization, current campaign state and E006/E032 reports,
evidence and actual geometry interfaces. Issue#1 stacking work is closed/reused;
no stacking research or full-EXE scan performed. The supplemental Headquarters file
is historical coordination material; this assignment's actor is Work1.

```sh
python -m pip install dnfile==0.18.0 dncil==1.0.2 pefile==2024.8.26
python re/campaigns/C001/perfect-circle/recover.py /path/osubackup.zip /tmp/p.json
python re/campaigns/C001/perfect-circle/check.py /tmp/p.json
python re/campaigns/C001/perfect-circle/build_fixtures.py --check
```

# 2. What they found

## A. Recognition, normalization and fallback

All method offsets below are hexadecimal, CIL-code-relative (headers excluded).
Full method SHA256/signature/RVA and selected sites are in ../perfect-circle/
static-evidence.json. No original instructions/listing/assets are redistributed.

CONFIRMED ORIGINAL: M28f7:1702–1731 recognizes decoded literal **P** as enum3
(PerfectCurve). The pinned C001 literal decoder record is reused, with matching
M28f7 body identity. Marker processing is case-sensitive; unknown single-character
path tokens leave the current kind; longer tokens enter coordinate parsing.
Controls follow inherited E006 Double parse → Int32 truncation → Single, with
optional Y flip BEFORE conversion. No general duplicate removal or epsilon filter.
M142d normalizes the supplied list by inserting the head only if empty/unequal at
index0; fixtures supply explicit head controls to make this boundary unambiguous.

M144a:00b4 dispatches kind3 to056a. On normalized count<3 (0576), jump to Linear
07a2; on count>3 (0587), jump to existing Bézier01f8. Exactly3 calls M2d65 at05bc.
Its exact Single determinant equality test is
`(Bx-Ax)*(Cy-Ay) - (Cx-Ax)*(By-Ay) == 0f` (M2d65:0000–003e).
Zero—including coincident controls—selects the consecutive Linear-pair branch,
NOT a single first-to-last line or a full circle when A==C. It writes prepared kind
F0be4=3 at05c5. Linear output writes kind2 only for original Linear kind or count<3
(087c–0895); thus a three-point collinear P retains kind3 despite linear geometry.
P with >3 controls inherits E032 version/duplicate/helper behavior and kind1.

No format field/version conditional exists in exact-three P region056a–079d or
its pure helpers. This is algorithm-level version independence, not proof of all
legacy loading/preparation. More-than-three fallback DOES inherit E032 differences:
v9 non-two-control ranges use uniform M2d64, v14 adaptive M2d62. Fixtures explicitly
separate these. <=5 stack/loading-order compatibility remains outside this packet.
Malformed `P|100:100|101` has a longer coordinate token with no Y cell: M28f7:1775
accesses missing index1 after X parse; row catch1cfa–1d03 rethrows for ordinary
validate/skip=false, can skip for true. Exact original CLR exception/runtime remains
UNVERIFIED RUNTIME. Do not treat every one-character malformed-looking token as an
error: single-character tokens have their own marker path.

## B. Center, radius and angles

CONFIRMED ORIGINAL M2d6a:0000–00c8 constructs a Single vector circumcenter:

```text
D  = 2f * (Ax*(By-Cy) + Bx*(Cy-Ay) + Cx*(Ay-By))
AA = Single(Ax*Ax + Ay*Ay); BB, CC likewise (M4eca)
Ux = (AA*(By-Cy) + BB*(Cy-Ay) + CC*(Ay-By)) / D
Uy = (AA*(Cx-Bx) + BB*(Ax-Cx) + CC*(Bx-Ax)) / D
```

Retain left-to-right expression order and original Single operands/stores. Center
components are stored Single; radius is Single distance(center,A) via M4ecb
(00cd–00db). There is no helper epsilon/determinant/radius guard after the separate
M2d65 test. Mathematically equivalent translated-center formulas may differ after
rounding. Exceptional cancellation/overflow must not silently become a modern
near-collinearity fallback attributed to stable.

M2d6b takes Single `(point-center)` components, promotes to Double, calls Atan2.
Angles start=a(A),middle=b(B),end=c(C), all Double. M2d6a:0107–014a:

```text
T = 6.2831854820251465  # actual Double literal, NOT exact 2*pi
while b < a: b += T
while c < a: c += T
if b > c: c -= T
```

The middle control chooses the major/minor and signed arc; it need not be emitted
as a vertex. Coordinates are screen-space, so increasing Y affects visual direction.
Do not replace this mechanism by a cross-product sign shortcut with exact2pi.
For start0/endpi, a negative semicircle becomes end=pi-T, whose magnitude exceeds
pi by about1.74846e-7. This literal difference is retained in reference math.

## C. Sampling and shared length adjustment

CONFIRMED ORIGINAL M144a:070b–079d:

```text
analytic = Abs((end-start) * Double(radius))
n = Int32(analytic * 0.125)       # positive finite conversion truncates
previous = original A
for i=1 while i<n:
    t = Double(i) / Double(n)
    theta = end*t + start*(1-t)  # preserve order
    q = center + (Single(Cos(theta)*Double(radius)),
                  Single(Sin(theta)*Double(radius)))  # M2d6c
    append Segment(previous,q); previous=q
append Segment(previous,original C)
preparedKind = 3
```

No minimum clamp to2/3, no modern sagitta-tolerance sampler, no requested length in
this sampling count. n=0 or1 emits ONE chord A→C without entering the division loop.
n>1 emits n pieces. Initial A and final C are supplied controls, not re-evaluated
trigonometric positions. M110c only writes endpoints; P branch does not set either
F0a34/F0a35 flag, hence both are fresh CLRfalse. Linear fallback sets its own flags
per E006. Preserve flags in the canonical segment contract.

The temporary analytic F0bdd value is **overwritten** by ordered Double sum of the
Single segment lengths at089a–08d0. Reuse E006's exact tail pass, NOT circular-angle
length adjustment. For G>0 and positive requested L, excess=G-L; remove trailing
piece d when `Double(d)<=excess+0.0001`, subtract d and continue. Otherwise a nonzero
last direction uses original Normalize and `d-Single(excess)`; a zero final piece
blocks extension. Thus long requests extend the FINAL CHORD, not the circumference
or exact analytic tangent. Recompute adjusted cumulative lengths; do not force them
to equal requested L. Empty output skips prepared-collection assignment and returns
before final endpoint/time write (0ad2–0bea), rather than inventing a zero-time object.
All-coincident three-control fallback instead retains two zero segments (G0 skips
adjustment); downstream zero-distance scheduling is NOT established by this packet.

There is a separate editor-mode gate at05e2–0706: mode1 builds bounds from endpoints
and intervening quadrant extrema; F0be9 can trigger512/384 checks, and width/height
strictly>640 falls back to Bézier. M1cde/M1ce6 accumulate extrema; M1ce4/5 return
max-min. Ordinary Play bypasses this block. Do NOT apply a universal640 circle cap.
Editor point mutation/reprepare and exact configuration state are excluded from
finite fixture predictions. Editor bounds mechanisms are recorded, not qualified.

## D. Independent finite fixtures

21 hashed .osu files and structured expectations are in ../perfect-circle/manifest.json.
They have synthetic metadata and no third-party media. Rational circumcenters are
computed independently, then separately checked with scalar binary32 arithmetic.
Projected sample vertices/tail outputs use binary32 stores and Python binary64
math, NOT original CLR execution. Suggested1e-4 comparison tolerances are ENGINEERING
numerical controls, not evidence of original-x86 bit identity.

| Fixture family | Discriminator / independently calculated expectation |
|---|---|
| Positive/negative semicircle, R32 | n12; retain signed wrap constant and exact final raw C; shared shortening L100 gives adjusted length near100 |
| Negative major arc, R6 | sweep about-3pi/2 with literal-wrap correction; n3, not a minor arc |
| Sampling R2/R3/R6 | n0/n1/n2 →1/1/2 raw pieces; R2 chord length4, not analytic2pi |
| R6 trim / extension | G=16.9705619812; L12 endpoint≈(97.5147171,103.5147171), L24 endpoint≈(89.0294342,95.0294342) |
| Tiny requested .00005 | Entire two-piece path removed by shared tolerance; no prepared collection/final-time write predicted |
| Collinear backtrack | (100,100)→(104,100)→(102,100), G6; L8 ends(100,100), preserving final negative direction |
| Coincident / closed / singleton | A==C falls back to out-and-back lines; all-equal retains two zero segments; singleton emits none |
| Fractional coordinates | Truncation makes three controls collinear; no floating-input circumcircle |
| P4 v14 / v9 | Modern inherited cubic6 pieces vs v9 helper200 samples excluding t1; v9 fixture asserts dispatch only, not full compatibility output |
| Near-collinear | Exact nonzero determinant; center(150,10100.5), n25; do not substitute epsilon fallback |
| v5/v9 exact-three / repeat2 | Same P geometry under M144a; version/order and traversal remain separate inherited stages |
| Missing Y coordinate | Original row-error branch; no invented recovery or prepared path |

Expectations distinguish raw vertices, chord G, requested L, adjusted segments/lengths
and final endpoint. Only finite cases with explicit expectations are admitted by this
research packet. It does not establish universal parsing, production or runtime.

## E. Handoff, dependencies and unresolved branches

Work2: current canonical SliderGeometryPreparation.Prepare(...,bool bezier,...)
and Beatmap.PrepareSlider boolean cannot represent P. Extend the path-kind contract
without discarding raw P identity or its distinct fallback/prepared classification.
Keep source controls/document separate from sampled/prepared segments. Dispatch
P using normalized control count, exact determinant, original center/angle/sample
order. Reuse existing L/B fallback contracts and common E006 tail adjustment.
Apply explicit cancellation/resource guards as ENGINEERING; original has no
recovered safe finite sample-count limit. Do not copy a modern arc approximation.

Feed resulting segments/cumulative lengths into existing E005/E016/E021 traversal,
reverse movement, tick/checkpoint carry and integer-time conversion. Requested
length and sampled/adjusted length differ; repeat2 reverses the prepared path, with
final traversal endpoint back at head. Do not re-sample the circle per traversal or
restart tick phase at each generated piece. Sample overrides and stacking follow
existing canonical identity/translation stages. New P geometry must reach rendering,
input, hitsounds and health through that same model. Existing Bool adapters may be
retained for historical API compatibility. Add finite P admission only after actual
production regression; present source still rejects P. R17 remains restricted.

OPEN, not silently substituted: zero/negative requested length enters existing
configuration/quantization branches (0935–09ab); editor F0be9/state and full legacy
pipeline; denominator cancellation despite nonzero relative determinant; NaN/Inf,
Int32 overflow/out-of-range conversion, huge n/allocation and original x86 math;
zero/empty path downstream scheduling; malformed original-runtime exception and
recovery; nonstandard factories; P4 comprehensive old compatibility. No investigation
of Catmull or star discrepancies. These are NOT VERIFIED; finite normal implementation
can proceed without claiming them. If Work2 needs a broader variant, identify its
specific operand/case rather than extending admission by guesswork.

# 3. Pseudocode

RECONSTRUCTED from CONFIRMED ORIGINAL static evidence:

```text
normalize controls using original parser/constructor order
if P and count<3: use Linear pairs; preparedKind=Linear
else if P and count>3: use existing version-aware Bezier; preparedKind=Bezier
else if P and exact Single cross==0:
    use Linear pairs; retain preparedKind=P for this three-control branch
else:
    compute Single circumcenter/radius and Double angles
    unwrap with literal T, choose signed arc through middle control
    handle editor bounds only in its explicit mode/state
    n=Int32(Abs((end-start)*radius)*.125)
    emit original A, intermediate samples i=1..<n, original C
    preparedKind=P; keep default segment flags
sum Single chord lengths into Double G, overwriting analytic temporary
join inherited requested-length adjustment and prepared cumulative output
existing shared traversal/time/tick/checkpoint stages consume those pieces
```
