# 1. Result

Expedition 06 — Linear Slider Geometry Preparation & First Consumer. 2026-10-27 (Asia/Ho_Chi_Minh). Work 1, original-software research only.

## Objective, preparation and scope

Recover how the ordinary osu!standard slider turns control points and requested pixel length into segments/cumulative lengths, then identify a consumer outside loading without reconstructing its subsystem. Follow corrected `M113e → M0651 → M48de → M48e0 → M142d` and base M144a. Non-goals: other curve algorithms, full tracking/judgment/rendering, GPU operations, caching, optimization or replacement gameplay components. Work 2 remains suspended; nothing under `reconstructed/` changed.

Read full `Instruction.md`, cumulative contexts, Expeditions 01–05 and relevant evidence. GitHub parent `641ea3a9a64339dd530be84c0e465fefdf158800` verified, tree `4dcf2f81a8d4ac19fd86325813324e2ae6f912e1` matches local baseline. Dedicated branch `work1/expedition-06-linear-geometry`. All historical records, tools and evidence remain unchanged; journal append only. No authorised exception.

Pinned archive SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; executable SHA-256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`. Same Stable40/file-version 1.3.3.8 metadata; dated official build, authenticity/modification status remain UNRESOLVED. Analysis is Python on Linux. No original executable/CLR execution, Windows display/settings, gameplay capture, frame-time or memory measurement occurred. No real maps/Aspire inputs are supplied by this archive.

## Evidence and validation

New files: this report, `tools/linear_geometry_evidence.py`, `tools/check_linear_geometry.py`, `evidence/06/linear-geometry.json`; appended `contexts.md`. Collectors from E04/E05 are reused unchanged. Public evidence contains metadata, body hashes, token/constant/branch/arithmetic sites and cross-references, not original bytes, disassembly, decompiled source or assets. Private disassembly was inspected directly from pinned ZIP.

Abbreviations: Mxxxx = MethodDef 0600xxxx, Fxxxx = Field 0400xxxx, Txxxx = TypeDef 0200xxxx. All offsets below are **hexadecimal, code-relative, headers excluded**. Exact RVAs/file offsets/signatures/ancestry are in JSON. Important identities:

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---|---|
| M144a | 7b43c | 5632 | d58260ebed39834edc228b1b21995da8481e87bbc342223db2b812c7c9b7ceef |
| M144b | 7ca64 | 2173 | f2a7a94e4fe319670174d77a6cba8364c553f956068618af029fa0ad12ff3164 |
| M28e5 | 1098dc | 222 | c115defbea9357b3fa3672aae1c61ef8dffe4fd63948cfd8a382a1db61a127f5 |
| M2269 | dae1c | recorded in JSON | 32451cc91cfdf7d9d5d06fb33bfb73f589bcd4241713e3cc38d3fbcf0ce8af6d |

Competing explanations: one first-to-last line versus consecutive pairs; clamped requested length versus extending/trimming geometry; zero length discarded versus retained zero segments; loader prepares all presentation data versus a later consumer. Smallest discriminating static experiment: resolve Linear enum/switch, inspect pair-loop bounds, tail guards/stores, cumulative writes and matching virtual consumer slots. Full EXE scan records every direct field access to F0bdc/F0bde and direct call/address site for slot M24b5; it is not a temporal trace or reflection/native reachability proof.

Reproduction from repository root:

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/linear_geometry_evidence.py /path/osubackup.zip > /tmp/linear-a.json
python re/tools/linear_geometry_evidence.py /path/osubackup.zip > /tmp/linear-b.json
cmp /tmp/linear-a.json /tmp/linear-b.json
python re/tools/check_linear_geometry.py /tmp/linear-a.json
python re/tools/check_slider_timing.py re/evidence/05/slider-timing.json
python -m py_compile re/tools/linear_geometry_evidence.py re/tools/check_linear_geometry.py
```

Actual tests: repeated extraction byte-identical; new checker PASS for 43 methods, Linear dispatch/pair construction, coordinate conversions, tail guards, collection/time stores, virtual consumer and loading gate. All 19 shared E05 body hashes agree. Scan decoded 19,659 bodies, skipped 1,739 zero-RVA methods, zero decode errors; ten methods reference prepared fields. Selected branches validate against instruction boundaries through the reused collector. E02–E05 checkers and compilation PASS. Historical file bytes/journal prefix, report headings, diff checks and publication tree/blob consistency verified. Remote commit is supplied in the final handoff. Reference cases below are static predictions, **not executed gameplay tests**.

# 2. What they found

## What this means to an osu! player

**RECONSTRUCTED interpretation of CONFIRMED static instructions:** a linear slider can be a chain of straight pieces through all its control points. Stable prepares those pieces during map loading. If the declared length is shorter, it removes pieces from the tail and shortens the last remaining piece; if longer, it can extend that final piece in its existing direction. The exact code has a small removal tolerance and a zero-direction guard, so the resulting path is not always exactly the declared length.

That prepared path supplies the duration calculation from Expedition 05. In particular, a duplicate final point can prevent extension, and a tiny positive requested remainder can be removed completely. Geometry and timing must therefore agree on the actual prepared lengths, rather than substituting the requested pixel length into a formula.

Prepared geometry is later read through an identifiable **visual-preparation dispatch in the host's draw routine**, with additional position-query consumers also present. This establishes a boundary outside loading; it does not establish the globally first runtime read, first visible frame, GPU behavior or stutter cause. All CONFIRMED labels here refer to static instructions. Gameplay aliases are RECONSTRUCTED; unmeasured runtime behavior remains UNRESOLVED.

## Correct ordinary path and control-point inputs

E05's corrected factory is retained: M0652 constructs M48de; M48e0 at `017` constructs M142d/T02af directly. This is not Taiko T08c2/M424e; its empty M4261 override must not replace ordinary M144a. Original metadata enum T03bb gives Catmull=0, Bezier=1, **Linear=2**, PerfectCurve=3.

Parser M28f7 splits row column5 by `'|'` at `1691–16a1`. One-character elements go through string comparisons; equality against opaque `M49b0(2064461905)` branches to storing curve value2 at `1728–1729`. The literal decoded marker was not recovered here; do not claim its text was executed or decoded. Non-one-character elements split by `':'` at `1738–1751` and are parsed as coordinates:

- X: M09e4(value,131072,validation) at `1767`, then **conv.i4, conv.r4** at `176c–176d`.
- Y: parse normally or compute `384.0 - parsedY` depending on original flip flag, then **conv.i4, conv.r4** at `17ac–17ad`.
- Construct Vector2 at `17ae`, append at `17b7` in input order. This is not floating-coordinate preservation: for finite in-range values, conversion truncates toward zero before storage as single.
- The 131072 argument is an optional numeric-validation limit, not an unconditional clamp. Normal loading passes validation=false (E05); Double.Parse can still fail and out-of-range integer conversions remain runtime-dependent.

The slider head position is supplied separately, with its earlier parser bounding/flip path (E04). Do not impose the head's 0..512 bounds on all control points. For Y flipping, subtraction occurs **before** truncation; `384 - trunc(y)` is not equivalent for fractional values.

M142d constructor:

| Site | Effect |
|---|---|
| `050–05c` | Store supplied curve kind into F0bd8 and F0be4. |
| `0ad–0cd` | Null supplied point list: allocate F0bdb, append head position. |
| `0cf–0d2` | Non-null supplied list: store the **same list reference** into F0bdb; it is not cloned here. |
| `0d7–11e` | If list empty, append head. If first point differs by exact Vector2 inequality, insert head at index0; otherwise keep it. No general duplicate elimination or collinearity collapse here. |
| `179` | Store requested double length in F2528. |

Thus a list of one endpoint different from the head becomes two points; a list containing only the head remains one point and creates no pairs. Adjacent duplicates elsewhere remain. Original points are F0bdb; prepared segment objects have separate endpoint fields. Normal tail adjustment changes those objects, not the supplied list. A pre-existing F0be9 path in M144a `075–0a5` can modify the point list before curve dispatch; this report's fresh-loading cases set that flag false. Do not generalize the fresh-object input normalization into a complete editor contract.

## Linear segment construction and length calculation

M144a switch at `0b4`, indexed by F0bd8, targets `[0ce,1f8,7a2,56a]`. Therefore Linear=2 goes to `7a2` regardless of map version. No non-linear algorithm is needed for this branch.

At `7a2–877`, loop i=1 while i<pointCount. It constructs a temporary segment M110c(P[i-1],P[i]) at `7c6`. An inner interpolation loop has count **1** (`7cd`): j=0 only. It creates a new segment from:

\[
A' = A + (B-A)\,(float(j)/float(1)),\quad
B' = A + (B-A)\,(float(j+1)/float(1)).
\]

Vector operations are single-valued XNA methods in this same EXE. For ordinary finite points this represents consecutive A→B pieces, but retain the arithmetic order rather than replacing it with endpoint copies for extreme inputs. The actual appended segment is the second allocation at `82d`, appended at `83f`. F0a35=true at `837`; after each pair F0a34=true on the last appended segment at `85f`. With one inner iteration, each linear piece receives both flags. Their downstream presentation meanings are not assigned here. F0be4 is set to Linear2 at `893–895`.

M110c stores segment start/end Vector2 into F0a32/F0a33 at `008`/`00f`. M110d obtains `(end-start).Length()` using M4efe and M4ec9. M4ec9 computes `X*X + Y*Y`, converts to double for Math.Sqrt, then conv.r4 at `021`. Vector arithmetic methods M4efe/M4efd/M4f00 write single X/Y fields. M4ecb separately computes a single Distance used during tail adjustment. Preserve both callsites rather than assuming every intermediate has double precision.

M144a sums each single M110d result converted to double (`8b1–8c1`), stores the **unadjusted** sum G in F0bdd at `8d0`. It does not overwrite F0bdd with the final adjusted sum.

## Requested length replacement, shortening and extension

This shared tail pass runs only when **G is ordered >0** (`928`). For fresh loading F0be9=false and no opaque quantization selector, L=0 is replaced by G at `9a9–9ab`; positive/negative nonzero L remains supplied. F0be9=true also selects replacement. The alternative branch gated by non-null F290b and M4189 uses the quantized-length procedure documented in E05 (`954–99b`); it is preserved as a dependency, not assigned a product setting name.

Set excess=`G-L` at `9d9–9e1`. The loop examines the last temporary segment while Count>0:

| Guard / offsets | Exact consequence |
|---|---|
| `9f8–a0b` | Compute single distance d between current segment endpoints using M4ecb. |
| `a0d–a1c` | Compare double(d) with double(excess)+**.0001**. `ble.un` takes the remove branch when d<=threshold or comparison unordered. This is a removal tolerance, not a test that silently accepts a matching length. |
| Remove branch `a70–a7f` | List.Remove(that segment reference), discard bool, excess -= double(d); continue if any pieces remain. |
| Keep/adjust branch `a1e–a31` | If exact endpoint inequality is false (coincident endpoints), exit the adjustment loop immediately without normalization or extension. |
| Endpoint write `a33–a69` | Set end = start + Normalize(end-start) * (M110d() - **conv.r4(excess)**), then exit. Normalize is M4ed2; resulting endpoint is F0a33. |

For nonzero direction, positive excess shortens that tail; negative excess extends it. Removing a complete tail can expose an earlier piece, which is adjusted by the remaining excess. Even excess=0 normally reaches endpoint arithmetic; do not assume bitwise no-op. Normalize uses reciprocal square root in single-category arithmetic and has no internal zero-vector guard; the surrounding inequality check is therefore significant. Non-finite vector states remain outside an ordinary finite guarantee.

Consequences established by the branches:

- A requested length exactly at a pair boundary removes the following piece(s), then adjusts the earlier tail with remaining excess.
- When the requested remainder is <=.0001 in the comparison's actual arithmetic, the pass can remove the last piece entirely. A small positive L does not guarantee a nonempty prepared list.
- If G>0 but the final piece has identical endpoints and the requested extension exceeds the tolerance, it enters the keep branch and exits on equality: **extension is blocked**. No earlier nonzero direction is substituted. Requested L can remain larger than actual prepared length.
- If all pieces have zero length, G=0 skips this tail pass entirely. Those zero segments can remain; requested nonzero L does not make a direction appear.
- A negative L on an ordinary finite positive-length path removes all pieces. The empty-result behavior below is not successful finalized zero-duration geometry.

## Stored output, ordering and duration relationship

At `ad2–b54`, if the temporary list is nonempty:

1. Store it into **F0bdc** at `ae1`.
2. Allocate cumulative list **F0bde** at `af6`, or clear its existing list at `b03`.
3. Starting from double0, add each *adjusted* segment's single M110d result converted to double at `b28–b2f`, then append each cumulative sum at `b37`.

Cumulative lengths are not forced to requested L. If the temporary list is empty, neither output collection is freshly assigned by this block. Conditional resource cleanup follows, and temporary Count<1 returns at `bea` before the final end-time store. A reused object can retain older fields; do not invent a clear/reset that this path does not perform.

For a nonempty result, E05's duration pass reads cumulative differences at `e14–e41`, converts to single, multiplies by 1000f, converts to double and divides by V. It repeats forward/backward R times, accumulating from absolute start; F2524 gets conv.i4(accumulator) at `1378`. Hence **actual prepared length**, including zero pieces and failed extension, is the input to this base timing loop. F2528 and final cumulative length are different state values and are not universally interchangeable.

Additional loading-time consumption exists. M28ea invokes M144a before M28eb for versions>5; M28eb eventually invokes position slot M24b0 (`324`), resolving to slider M1457. M1457 moves source points and both prepared endpoints by the same vector delta (`087`/`0a9`) without writing F0bde or end time. Versions<=5 call M144a inside M28ec before related positioning work. Treat stored coordinates at the end of loading as potentially translated. Real-number translation preserves distance; floating arithmetic and this method's lack of cumulative recomputation must not be silently replaced with a fresh sum.

Version boundary summary: Linear pair construction itself has no version check. Version5/6 changes manager postprocessing order; version7/8 changes Q used by the existing shared zero-length/quantization/tick path. Other version checks in M144a belong to non-linear branches and were not generalized to Linear.

## First identified consumer outside loading

**CONFIRMED static boundary, not a measured global-first event:** host method M46a6 calls M28e5 at `07d`, immediately before its later GL.Clear call. M46a6's surrounding draw calls make this a draw-stage preparation boundary; no GL implementation, upload, render-target algorithm or GPU behavior was investigated.

Manager constructor M289a(true) adds the manager to static registry F17e3 at `079`. M28e5 locks/enumerates that registry. In application Play=2, it skips the per-manager body while F13d8=false (`069–076`); otherwise it enumerates that manager's **F17fd** candidate subset, tests each object's F153a and invokes virtual **M24b5 at `098`**. Edit uses a separate full-list branch at `055`. Slot M24b5 resolves to ordinary T02af's **M144b**, matched by ancestry/name/signature/reused-slot metadata.

The candidate subset is populated by manager M28e2's time-window search/GetRange at `0e6–0eb`, and differs from the entire object list F17fb. M28e2 is also called by Play update; exact candidate policy is not reconstructed here. Base completion M223c writes F13f0=true at `05d`; loading/update gate M2269 reads it at `0b6` and, after further opaque conditions and a wrapper call, sets F13d8=true at `15a`. This ties the preparation opportunity to loading-related state, but does not establish all scheduler timing, variant behavior or successful entry in an actual run.

M144b's earliest prepared-field read and continuation:

| Site | Boundary behavior |
|---|---|
| `00f–033` | Early returns based on F2c24 and existing optional F0bda/resource/F2c30 state. Purposes remain token-labelled. |
| `035–04a` | If M24b6() is false or F0bdc is null, call M144f then return. First prepared-list read is **`03d`**. This gate is not proof of visibility. |
| `04b–0ed` | Further state/progress/existing-resource gates can return. Do not imply the whole list is consumed on every dispatch. |
| `12f` | Reads F0bde on the partial-progress branch, followed by FindIndex; full-progress branch instead takes prepared-list Count at `14b–150`. |
| `1f2–214` | Reads prepared segment endpoints F0a32/F0a33 and computes Distance. This is actual geometry consumption beyond a null check. |
| Continuation | Builds another temporary segment list, with conditional truncation/grouping and coordinate helper calls. Full downstream presentation/resource pipeline is **UNRESOLVED and outside this expedition**. Prepared loading segments alone are not the complete presentation preparation. |

A second identifiable consumer is distance query **M145f**: reads F0bdc/F0bde at `001`/`009`, requests virtual M144a(true) at `012` when either is missing, checks again, and falls back to head position when lists are absent/empty. With data present it returns first start for distance0, final end for distance>=last cumulative, or BinarySearches cumulative data and interpolates a selected segment (`0a8`, `0f2–13d`). M145e calls it for a time-based position query through virtual M3e5d; out-of-time-range behavior has its separate base path. This establishes a possible lazy preparation route, not proof it runs first in gameplay. Duplicate cumulative values, reflection/native callers and actual interleaving remain unresolved.

The ordinary object update slot M24b2 resolves to M1449; its visible early dependency is dirty/Edit-triggered M144a(false), not a direct F0bdc/F0bde field read. Other animation/position-query paths could consume geometry before M144b. The complete static field-site inventory is supplied, but **chronologically earliest post-loading read remains UNRESOLVED without an execution trace**. The earliest verified external preparation chain above is sufficient to name the next boundary; it must not be reported as an observed first-visibility event.

## Reference cases and interface

All cases are **branch-derived static predictions**, not original-game passes. Unless stated otherwise: corrected ordinary T02af, curve Linear2, normalized constructor point list P, fresh F0be9=false/F0be3=false, recompute=true, opaque quantization selectors off, finite in-range arithmetic, and all invoked auxiliary operations return normally. Capture geometry immediately after cumulative construction and again after loading translations. Duration examples use start1000, V200, R1; they are conditional on reaching the final time write.

| ID | Variation | Prepared-state prediction / discriminating purpose |
|---|---|---|
| L01 | Head(0,0), supplied list[(100,0)], L100 | Constructor prepends head to same list; one pair/segment, cumulative100, end1500. |
| L02 | Supplied list begins with head | No extra head inserted; no global deduplication. |
| L03 | P[(0,0),(100,0),(100,100)], L200 | Two segments through the corner, cumulative[100,200]; not direct diagonal. End2000. |
| L04 | Same, L150 | Final endpoint(100,50), cumulative[100,150], end1750. |
| L05 | Same, L250 | Tail extends to(100,150), cumulative[100,250], end2250. Original control list still ends(100,100). |
| L06 | Same, L100 | Last full segment removed; one segment to(100,0), cumulative100, end1500. |
| L07 | Same, L50 | Remove last piece then shorten first to(50,0), end1250. |
| L08 | Positive path G200, L0 | Normal replacement L=G; geometry prepared, not necessarily zero duration. |
| L09 | Positive path, L=-1 | Remove all temporary segments; fresh outputs remain null, end retains initial start if no earlier side effect changes it. No final end store. |
| L10 | One positive line G100, L=.00005 | Tail distance<=excess+.0001 removes it; demonstrates tiny positive requested length can yield empty result. Numerical boundary probes must log actual floats. |
| L11 | P[(0,0),(100,0),(100,0)], L150 | Last zero piece stops extension; prepared cumulative[100,100], L remains150. Duration uses100 → end1500 rather than1750. |
| L12 | Same duplicate tail, L100 | Zero tail removed by tolerance; prior piece retained/adjusted. |
| L13 | P[(0,0),(0,0),(100,0)], L100 | Leading zero piece retained; cumulative[0,100]. No general zero-piece filter. |
| L14 | P[(0,0),(0,0)], L100 | G0 bypasses adjustment; nonempty zero segment/cumulative[0]. If later auxiliary code returns normally, timing increment0. It does not extend to100. |
| L15 | Null/empty list or only-head list | Constructor leaves one head point, no pairs; temporary list empty, no final time write. |
| L16 | File coordinate x100.9/y10.9, no flip | Stored control point(100,10). Flip Y instead gives trunc(384-10.9)=373; not384-trunc(10.9)=374. |
| L17 | Same ordinary finite Linear input, versions5/6/7/8 | Pair loop unchanged. Manager positioning order/Q dependencies differ as documented; compare pre-translation and final-loading snapshots separately. |
| L18 | Tail length exactly equals excess+.0001 in actual comparison | Remove branch includes equality; perturb both sides to discriminate tolerance/rounding. |
| L19 | Existing nonempty outputs then recompute produces empty temporary list | This block does not clear old F0bdc/F0bde; verify stale-state consequences separately, not as fresh-load success. |
| L20 | Ready/candidate/F153a gates allow M28e5, M144b early gates allow | Virtual M24b5 resolves M144b; F0bdc first read03d, later conditional cumulative/endpoint reads. No inferred pixels. |
| L21 | Application Play, F13d8=false | M28e5 skips its candidate-loop path; does not establish that every other consumer is suppressed. |
| L22 | M145f called with missing prepared fields | Calls M144a(true), rechecks lists, may fall back to head. Does not prove loading always supplied geometry. |

Research interface: input head, point-list identity/order/coordinates, curve enum, requested L, format, recompute/dirty flags, Q-dependent selectors, rate/R/start, manager candidates, F13f0/F13d8/F153a and visual-preparation gates. Outputs: normalized point list, segment start/end/flags, G, stored L, F0bdc/F0bde identity/content, end, early-return/exception and consumer-entry/read sites. Keep CIL single/double/int conversions, list mutation, static-vs-runtime labels and reference hashes with each measurement. NaN/overflow, text import success and downstream resource failures are not fabricated.

## Remaining work and performance objective

The owner reports substantial stutter around distorted sliders and wants future evaluation of work that could safely move before gameplay. This expedition confirms only the static split: Linear segments/cumulative distances/timing are prepared in loading; M144b can perform further presentation preparation later, and M145f can request missing geometry lazily. It does not measure cost or attribute stutter to any of these sites. Full non-linear/pathological curves, first-visibility ordering, scheduler ownership, downstream resource preparation and actual frame spikes are UNRESOLVED.

Future Work 1 should log loading completion, M144b/M145f entries, first submitted/visible frame and frame times on the pinned Windows reference. Eventual Work 2 must evaluate any precomputation/caching with measured frame-time and memory costs and explicit authorization. No optimization or replacement component was built.

**Smallest next gameplay investigation:** recover M28e2's candidate time window and M24b6's object eligibility gate for one circle and one linear slider, through the first M24b5 dispatch. This connects prepared data to activation/visual preparation and moves toward a visible playable scene without expanding into the full renderer or scoring system. The frame-time/stutter experiment remains a separately measured dependency.

# 3. Pseudocode

CONFIRMED static branches and stores; RECONSTRUCTED names/equations. This is independent documentation, not executable replacement geometry. `r4`, `r8` and `i4` denote original CIL conversion categories; finite in-range i4 truncates toward zero. Original x86 evaluation/exception details remain UNRESOLVED.

```text
NormalizeConstructorPoints(head, suppliedList):
    if suppliedList is null:
        points = new list; points.Add(head)
    else:
        points = suppliedList             # same reference
        if points empty: points.Add(head)
        else if exactVectorInequality(points[0], head): points.Insert(0, head)
    store points in F0bdb

LinearPartOf_M144a(points):
    retain original recompute/rate/pre-existing-point-edit handling
    segments = new list
    for i from 1 while i < points.Count:
        A = points[i-1]; B = points[i]
        # Original inner subdivision count is 1; retain interpolation arithmetic.
        start = A + (B-A) * (r4(0) / r4(1))
        end   = A + (B-A) * (r4(1) / r4(1))
        s = new segment(start,end)
        s.F0a35 = true
        segments.Add(s)
        segments.Last.F0a34 = true
    F0be4 = Linear2
    G = sum_in_order(r8(segment.M110d()))
    F0bdd = G
    Q = original format/timing-dependent spacing       # E05
    if ordered(G > 0):
        if F0be9 or L == 0:
            L = original gated quantized length OR G # E05; preserve opaque calls
        if ordered(Q > L): Q = L
        retain original auxiliary count/flag writes
        excess = G - L
        while segments.Count > 0:
            s = segments.Last
            d = singleDistance(s.start,s.end)
            if unordered_or_le(r8(d), excess + .0001):
                segments.Remove(s)
                excess = excess - r8(d)
                continue
            if exactVectorInequality(s.end,s.start):
                s.end = s.start + Normalize(s.end-s.start) *
                                  (s.M110d() - r4(excess))
            break
    retain original post-adjustment point/flag handling
    if segments.Count > 0:
        F0bdc = segments
        allocate or Clear F0bde
        total = double0
        for s in segments:
            total = total + r8(s.M110d())
            F0bde.Add(total)
    perform original conditional cleanup
    if temporary segments.Count < 1: return before final time write
    perform E05 traversal-time accumulation from cumulative differences
    F2524 = i4(accumulatedAbsoluteTime)
    retain remaining loading/positioning/bookkeeping

IdentifiedExternalConsumerBoundary:
    manager ctor(true) -> registry F17e3
    M46a6 draw-stage path -> M28e5
    M28e5:
        lock registry
        for each registered manager:
            if application Play and not F13d8: skip this manager's candidate path
            retain separate Edit routing
            for each candidate in manager.F17fd:
                if candidate.F153a: invoke virtual M24b5
    ordinary slider override M144b:
        retain original early global/resource gates
        if not M24b6() or F0bdc is null:
            M144f(); return
        retain further progress/resource gates
        conditionally read F0bde and prepared segment endpoints
        ORIGINAL_DOWNSTREAM_PRESENTATION_PREPARATION_UNRESOLVED

UNRESOLVED:
    globally first post-loading read, thread/frame ordering and actual first visibility
    full candidate/eligibility policy, downstream consumer implementation and stutter cause
```
