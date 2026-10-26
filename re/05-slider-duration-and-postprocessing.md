# 1. Result

Expedition 05 — Slider Duration, Timing & Postprocessing. 2026-10-27 (Asia/Bangkok). Work 1, research only.

## Objective, scope and provenance

Recover the shortest original-code chain that establishes a slider's final end time, especially M28ea → virtual M144a, its timing selection, arithmetic, conversions and compatibility branches. Geometry is followed only to establish the length consumed by timing. No replacement parser, slider, runtime, renderer or optimization was implemented. No changes under `reconstructed/`; Work 2 remains suspended.

Read complete `Instruction.md`, cumulative contexts, Expeditions 01–04 and relevant evidence. GitHub starting commit `3b5e65fb38d9e70fafe03cc1820a49ba36a69939`, tree `d1826d4802ff361b3c20d797b12a8a35fe044952`, verified and matched to the local baseline. Dedicated branch `work1/expedition-05-slider-timing`. Historical records are preserved; the Expedition 04 interpretation correction below is recorded here and appended to contexts. No authorised exception.

Reference ZIP SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; EXE SHA-256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`. Same self-declared Stable40/file-version 1.3.3.8; dated official release and modification status UNRESOLVED. Host recheck: Linux, no wine/dotnet/DISPLAY or `/dev/dri`. Original Windows execution, settings, captures and gameplay measurements remain unavailable. No original game or CLR method was executed.

## Deliverables and actual checks

New `tools/slider_timing_evidence.py`, `tools/check_slider_timing.py`, `evidence/05/slider-timing.json`, this mark, and an append to `contexts.md`. Extractor reuses the unchanged Expedition 04 collector. Evidence covers 57 MethodDefs, body hashes/RVAs/file offsets, metadata signatures/ancestry, matching virtual declarations, token/constant/branch sites and arithmetic/conversion sites. Raw proprietary disassembly and binary/assets remain private and are not published.

All offsets in this mark are **hexadecimal, relative to the first CIL opcode**, excluding headers. `Mxxxx` means MethodDef `0600xxxx`, `Fxxxx` field `0400xxxx`, `Txxxx` type `0200xxxx`. Tokens belong only to this pinned executable. Arithmetic-site lists are cross-references, not an executable surrogate.

Reproduce from repository root:

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/slider_timing_evidence.py /path/osubackup.zip > /tmp/timing-a.json
python re/tools/slider_timing_evidence.py /path/osubackup.zip > /tmp/timing-b.json
cmp /tmp/timing-a.json /tmp/timing-b.json
python re/tools/check_slider_timing.py /tmp/timing-a.json
python re/tools/check_static_control_flow.py re/evidence/02/static-control-flow.json
python re/tools/check_request_guards.py re/evidence/03/request-guards.json
python re/tools/check_beatmap_init.py re/evidence/04/initialization.json
python -m py_compile re/tools/slider_timing_evidence.py re/tools/check_slider_timing.py
```

Actual outcomes: repeated extraction byte-identical; new checker PASS (57 methods, corrected switch/dispatch, slot metadata, timing selection, length/precision/end writes, versions and separate writer); all 19 overlapping non-abstract E04 method hashes agree; old checkers and syntax compilation PASS. Every extracted branch destination was checked against instruction boundaries. Historical-file identity, append-only journal prefix, three report headings, diff whitespace and publication tree/blob checks were performed. Publication commit is supplied in the final handoff. Numerical cases below are branch-derived reference predictions, **not passed original-game tests**. No dynamic timing, frame-performance, differential, image or Windows test ran.

# 2. What they found

## Player-level mechanism and confidence

**RECONSTRUCTED interpretation of CONFIRMED static instructions:** the ordinary osu slider obtains one travel rate at its start. A slower BPM increases its duration; inherited SV changes the effective beat length and thus the travel rate; a larger effective slider multiplier increases speed. Repeated traversals use the same start-selected rate. Later timing points inside a slider are not reselected by this timing loop.

The familiar finite formula is a useful projection, not the exact numerical algorithm:

\[
D_{ideal}=\frac{L_{prepared}\,R\,B_{effective}}{100\,S},\qquad
B_{effective}=B_{base}\,d,
\]

where `R` is the effective number of traversals (the file's slide count, not additional reversals), `S` is the map multiplier field, and `d` is a clamped inverse-SV factor. For ordinary positive BPM, `B_base = 60000/BPM` is the conceptual beat-length relation; the inspected calculation consumes the beat-length value directly, not a rounded integer BPM. Tick rate cancels algebraically out of the travel-rate expression, although it participates in tick spacing, a special zero-length fallback and floating arithmetic. The actual end-time writer uses prepared segment lengths, CIL single/double conversions and an integer conversion of the **absolute accumulated end**, described below. Geometry preparation and exceptional paths can invalidate the ideal projection.

CONFIRMED throughout means static evidence. Semantic field names are RECONSTRUCTED. Original reachability, exact x86 CLR/JIT floating evaluation, captures and player-observed behavior remain UNRESOLVED.

## Correction to Expedition 04: ruleset dispatch and concrete types

Historical passage: E04 says “Case 0 constructs handler M1ecb” and calls `M1ecb → M1ee1 → M20cd → M426f → M424e` the ordinary osu chain. **That ruleset annotation is incorrect.** It confused a constructor's presence/order with the switch's actual destinations. Earlier files are unchanged; use the following corrected relationship.

| Actual selector | Grounded dispatch |
|---|---|
| Osu=0 | M223b switch at `00e` targets `033`; calls M2234 and constructs M0da8 at `03c` when true, otherwise M113e at `04a`. M0da8 calls M113e. |
| Taiko=1 | The same switch targets `025`; constructs M1ecb at `027`. |
| Catch=2 / Mania=3 | Targets `056` / `064`; not expanded here. |
| Ordinary Osu manager | Handler M1141, override of manager-factory slot M0abb: M2234 true constructs M3203; false constructs M0651(true) at `00e`. T0680/M3203 derives from ordinary manager T00e8/M0651. The special manager overrides the loading pass; it is not assumed equivalent. |
| Ordinary Osu slider | Manager M0652 overrides factory-provider M28bc, constructs factory M48de. Its M48e0 overrides slider slot M1871, directly constructs **M142d / T02af** at `017`, then assigns additional metadata. T00e8 inherits manager M28e9 and rate M28ab; T02af executes M144a itself. |
| Previously followed Taiko slider | M20cd → M20d4 → M426f → M424e creates T08c2, which overrides M144a with **M4261, a one-byte `ret` body**. Its manager overrides M28e9 with M20d7 and has separate end writers M20d6/M4263. Applying the base M144a algorithm to it is wrong. |

Evidence records every matching declaration for slots M0abb/M28bc/M28e9/M125b/M1871/M144a, with type ancestry and reused/new-slot metadata. M144a's other override M18e2 first calls base M144a and then rejects duration >60000; this is **not** a 60000 limit proven for the ordinary T02af object. M3208's special manager eventually directly calls base M28e9, after its own object work; no all-variants equivalence is claimed.

E04's selected-map capture, reader/factory/registration boundary, equal initial time writes and existence of postprocessing remain supported. Its concrete Osu labels and any implementation inference based on them must be corrected using this entry. E04's checker verified constructor presence, not switch-to-ruleset association; its PASS did not establish that annotation. The new checker explicitly checks the switch destinations.

## Timing state and inputs

| Alias | Token / assignment |
|---|---|
| Start / end | F2523 / F2524, signed int32; M142d `07c`/`083` initially write the same start. |
| Raw supplied slide count | M1433 stores constructor argument 7 into F0bd0 without clamping. |
| Effective traversal count R | M142d `123–12c`: F2527 = Math.Max(1, argument7). The raw count and effective count are different fields. |
| Requested pixel length L | Double F2528, constructor store `179`; postprocessing may replace it. |
| Multiplier S / tick rate T | Map double fields F0b56 / F0b57. M1387 defaults 1.4 / 1.0. M1bc1 `c8f–cb7` bounds the former with Max(.4,Min(3.6,parsedDouble)); `cc1–ce9` bounds the latter .5..8. Literal key text is decoded through opaque M49b0; the gameplay aliases follow defaults, use and dataflow, not an independent execution of that decoder. Later state changes must not be assumed absent. |
| Base spacing A | Manager double F0adf. M28be `009–02b`: `(100.0 * S) / T`. Loader M28f5 calls this configuration method at `07f`, before reading objects. |
| Travel rate V | Slider double F0bfc. M144a `058–06a` invokes manager virtual rate slot M125b(start), writes V, unless bool F0be3 is true. Ordinary implementation M28ab. F0be3 is normally CLR-default false on a new object; other editing/state paths can supply a rate and suppress refresh. |
| Timing list / point | Map F100f; point time double F0662, beat-length-like double F0660, timing/base bool F0666 via M0b74. |
| Geometry/time bridge | F0bdd receives unadjusted path-length sum; F0bdc is prepared segment list, F0bde cumulative double lengths. Each segment's M110d returns single-precision vector length. |

The metadata parser calls M28ff at `366`, appends non-null results at `378`, and finally stores the list into F100f at `10ce`. M28ff reads time + supplied shift and beat length as doubles. Its column6 first character `'1'` supplies the timing/base bool (default true if missing); failed parsing is caught and returns null. Constructor M0b73 stores time/beat length then calls M0b75: if bool=false and beat length is ordered >=0, replace it with -100; negative values remain. Thus an inherited point carrying a positive beat length does not simply become a new BPM. NaN is not ordered >=0 in this branch. The timing selection below operates on list order; do not silently sort it or infer a universal same-time precedence. Other callers may mutate state.

## Exact timing-point selection and inherited factor

**CONFIRMED static mechanism:** M28ab calls map.M1be3(start,true) at `009`. M1be3 returns 0 for null/empty list. Otherwise initialize baseIndex=0, inheritedIndex=0, then scan **every** index in list order. It tests `double(conv.i4(point.F0662)) <= queryTime` (unordered comparisons do not qualify), not raw double time. A qualifying timing/base point updates baseIndex; a qualifying non-base point updates inheritedIndex. It does not break on a future point. Last qualifying *list position* wins; index 0 remains the fallback even before the first point.

Start with factor d=1. Only if includeInherited=true, inheritedIndex>baseIndex, and that inherited point's beat length is ordered <0, use its factor. M1be3 `095–0ce` chooses M0b79 when map.M1bba() is 1 or 3; otherwise M0b78. Map.M1bba reads stored ruleset F0ffe, which need not equal a separately selected Play ruleset in a conversion case.

- M0b78: if value is ordered >=0 return single 1; else `ClampSingle(conv.r4(-value),10,1000)/100f`.
- M0b79: same, upper bound **10000** instead of 1000.
- M2d7d performs single-precision clamp; NaN input survives rather than becoming an endpoint. M1be3's negative-value gate normally prevents a NaN inherited value from entering the factor calculation.
- M1be3 `0cf–0e2` returns `list[baseIndex].F0660 * double(d)`.

For ordinary Osu-map mode and finite negative inherited beat length `u`, ideal `d=clamp(-u,10,1000)/100`. Thus u=-50 means approximately 2x SV (d=.5); -1 is capped at approximately 10x SV; -5000 is capped at .1x SV. The single conversion/division matters near numerical boundaries. A later base point resets inheritance by index ordering, even at an equal timestamp.

M28ab's rate calculation, preserving operation order:

```text
E = map.M1be3(start, true)
if ordered(E > 0): V = (A * T) * (1000.0 / E)
else:              V = A * T
```

The fallback includes zero, negative and NaN E. There is no ordinary rate clamp here. For no timing points, V is A*T, not division by zero or a fabricated default BPM. Positive infinity E follows the positive branch and yields zero rate for finite A/T. These are static arithmetic consequences, not proof malformed maps load successfully.

**A second timing lookup is deliberately different.** M144a `8d8–91c` sets tick-spacing variable Q=A for format<8, otherwise Q=A / map.M1be4(start). M1be4 uses M1be6: last list item whose **raw double** time <= query, or first if none, null if empty; no integer truncation. It then uses M0b78/M0b79 or returns 1 when null. This lookup has neither the integer time test nor the base/inherited-index comparison of M1be3. A point at time 1000.9 can influence travel rate at start=1000 while not influencing Q's lookup at 1000. Do not combine the two lookups. Timing changes after the selected start do not re-evaluate V inside the traversal loop.

## Construction, length preparation and final end write

| Order | Original sites and effect |
|---|---|
| Constructor | Equal start/end, raw count, effective max(1,count), L, points and metadata. No finalized duration. Optional resources are allocated, but that is not first visibility. |
| Loading pass | M28f5 `269` virtual M28e9. Ordinary M28e9 returns if current application mode=Edit(1); otherwise count=list.Count, M28ea, then M28ee(0,-1). |
| Version dispatch | M28ea: version>5 enumerates T02af sliders, virtual M144a(true) at `02e`, then M28eb(0,-1). Version<=5 calls M28ec at `056`; that method invokes virtual M144a(true) at `036` per slider while doing its older positioning/stacking pass. Old files do not skip slider timing. |
| Recompute gate | M144a `000–02b`: effective bool = supplied bool OR (L==0 OR segmentList==null ? true : F0be9). False returns. Loading supplies true. It saves prior duration for later manager bookkeeping, resets some resources via M144f and refreshes V unless F0be3. |
| Prepare geometry | M144a `0aa–89a` builds a temporary segment list from curve/points; version/curve branches matter. Detailed algorithms remain UNRESOLVED here. Sum each segment.M110d() into double G; store G in F0bdd at `8d0`. |
| Resolve stored length | If G is ordered >0 and (F0be9 OR L==0), usual branch `9a2–9ab` replaces L with G and obtains an integer auxiliary count from G/Q. Another branch, gated by non-null F290b and M4189(), computes step=Q*T/M41b4(), n=conv.i4(Max(0,(G+1)/step)), L=double(n)*step (`954–99b`). Those opaque selectors are not given an editor/product meaning. |
| Adjust segments | If Q is ordered >L, set Q=L (`9b0–9c0`). Adjust trailing segments using excess G-L; remove or trim/extend the last segment, with literal .0001 tolerance (`9d9–a88`). This is why stored file length alone is not an exact timing input. Detailed geometry/degenerate behavior is a remaining boundary. |
| Cumulative prepared length | If segments remain, store list F0bdc and build F0bde from a fresh double sum of each segment's single M110d() result (`ad2–b54`). If temporary count<1, return at `bea`, before final end write. Do not fabricate finalized zero-length success or clear stale prior state. |
| Accumulate time | Start double accumulator at double(start), `d52–d59`. For each traversal R, visit all segments forward or backward according to traversal parity. Segment length operand is `conv.r4(cumulative[i] - previousCumulative)` at `e14–e41`. Compute `(1000f * segmentLength)`, convert to double, divide by V (`e6d–e7d`), add to accumulator at `ef4–ef9`. Tick-related loops do not write that accumulator. R is tested at `1338–1340`. |
| Final state | After repeat/end-position work, M144a `1377–1378`: **F2524 = conv.i4(accumulatedAbsoluteTime)**. This is the only F2524 store in this body. M3e4d subsequently reports unchecked int32(end-start). More visual/manager bookkeeping follows; end assignment is not the method's final action. |

M144a body: RVA `0x7b43c`, code 5632 bytes, SHA-256 `d58260ebed39834edc228b1b21995da8481e87bbc342223db2b812c7c9b7ceef`. Timing helper M1be3 body SHA-256 `c73e952338744c6b6a9c9d82baef4e3614f74d72e15e619efbb80c55d77ff192`. Exact metadata and branch destinations are in the evidence JSON.

**Rounding limitation:** for finite representable ordinary values, `conv.i4` truncates toward zero. The end is not `start + round(duration)` or `start + R*truncate(oneSpan)`. Intermediate CIL uses both single and double categories; prepared segments and accumulation can differ from one double length multiplication. This report preserves conversion instructions, but has not verified x86 CLR/JIT excess precision, NaN/infinity/out-of-range integer conversion results, or geometry equivalence. Do not assume saturation, a specific sentinel or an exception for an unchecked out-of-range `conv.i4` without original-runtime evidence.

## Malformed and extreme input boundaries

M28f7 reads repeat column6 using M09e5 at `17da`. It explicitly throws ArgumentOutOfRangeException for value>9000 (`17e3–17f9`), even when numeric validation argument is false. Count=9000 passes this comparison; zero/negative values are not rejected by it. Constructor R becomes at least 1, but raw count and raw+1-dependent allocations/edge data can still fail or differ. Do not replace all uses with the clamped R.

Length column7 is read through M09e4 at `1811`, limit argument 2147483647 and validation=parser argument3. The normal M28f5(false,false) path passes false to that validation parameter. M09e4 immediately returns Double.Parse's result when validation=false; it adds no finite/positive/range check there. If validation=true, values outside ±limit throw OverflowException and NaN throws FormatException. M09e5 analogously adds range checks only when validation=true; Int32.Parse itself can still fail. Text parsing culture and .NET Parse behavior are dependencies, not replaced by a permissive custom parser. Optional missing length and degenerate geometry require the actual constructor/preparation branches.

Consequences to test, not observed game behavior:

- No explicit positive-length clamp exists at these sites. A negative L on an otherwise simple positive-length line can remove all prepared segments, then return before the end store. Other geometry can fail earlier.
- A zero L with G>0 usually adopts G; it is not necessarily a zero-duration slider. Opaque quantization branch can instead change L.
- S/T are bounded during the identified metadata-reader assignments; that does not prove all later/injected states are bounded. E<=0/NaN and E=+infinity take different rate branches.
- Extremely many traversals/segments can create large loading work. No performance cost, complexity bound over malformed curves, memory usage, first-visibility stall or cause of stutter was measured.
- The ordinary M144a does not contain the other subtype's explicit duration>60000 rejection. Transitive helpers, allocations and other loading checks can still fail. Do not infer unlimited accepted duration.

## Reference predictions and validation interface

Log original build/hash, Play ruleset and map ruleset separately, concrete manager/factory/slider type, application mode, format, start/end, raw count/R, S/T/A, timing list in actual order, E/V/Q, L before/after, F0bdd, segment count/cumulative lengths, F0be3/F0be9, completion result and managed thread. Breakpoints: M28be, M28ab, M1be3, M144a entry / `8d0` / `b55` / `1378` / return, M28f5 return, work/completion callbacks. Break at an offset before a store and single-step/log after it to avoid mistaking the old field value for the new value.

Below are **static predictions only**. Ordinary T02af, Edit gate off, supplied recompute=true, normal finite state, opaque quantization selectors off, successful geometry and normal-returning helpers are explicit preconditions. Base control: start=1000, format=14, S=1, T=1, one straight prepared segment length100, R=1, base point at0 with beat length500 (120 BPM), no inherited point. Predicted V=200, end=1500.

| Case | Single variation | Expected reference result / discrimination |
|---|---|---|
| S01 | Base control | End1500; rejects constructor start=end as finalized timing. |
| S02 | Base beat length1000 (60 BPM) | V100, end2000. |
| S03 | Inherited u=-50 after base, before start | E250, V400, end1250; SV shortens duration. |
| S04 | S=2 | V400, end1250. |
| S05 | Prepared length200 | End2000. |
| S06 | Raw/effective spans2 | End2000; two traversals, one reversal. |
| S07 | T=2 | A50, V200, end1500; tick spacing changes independently of ideal duration. |
| S08 | New base point after start but before nominal end | Duration still uses start-selected E/V; no per-segment BPM resampling here. |
| S09 | Base then inherited at same qualifying timestamp | Later inherited list index contributes factor. Reverse their order: later base index suppresses inheritance. |
| S10 | Inherited time1000.9, start1000 | Integer-time M1be3 includes it, raw-time M1be6 does not; with u=-50 end1250, Q uses earlier point. |
| S11 | Before all points | Index0 fallback, not “no timing”. Empty list instead gives E0 and V100; control length100 gives end2000. |
| S12 | Inherited u=-1 / -5000, map mode0 | Inverse factor approximately .1 / exactly10; end1050 /6000 for control. Verify single precision near the cap separately. |
| S13 | Format7 vs8, u=-50 | Same ideal V/end; Q=A vs A/d. Tick-spacing change alone must not double duration. |
| S14 | Format5 vs6, same simple line control | Both invoke M144a; end1500 if prepared segments identical. Manager positioning order differs. |
| S15 | File length0, positive line G100 | Normal fallback sets L100 and yields end1500; not automatic zero duration. |
| S16 | L=-1, otherwise positive simple line | Trailing removal can empty temporary list and return before final end store; confirm exact geometry and failure/completion status. |
| S17 | Raw count0 or -1, successful construction | R1; retain raw F0bd0 and allocation effects. Do not promise success for all negative counts. |
| S18 | Count9000 vs9001 | First passes parser cap (cost/success unmeasured); second throws at this parser site. |
| S19 | Start1000, S1, T1, beat length100, one prepared segment500.5 | V1000, accumulated end1500.5 converts to1500. Two spans give2001, not2000. |
| S20 | Same single span, start=-1000 | Accumulated -499.5 converts to -499, rather than start+truncated duration=-500. |
| S21 | Metadata S=.1 or9 | Identified reader assignments bound S to .4 or3.6; simple control predicts end2250 or1138, conditional on no later changes. |
| S22 | Timing E0 / NaN / +infinity | First two use fallback V=A*T; +infinity produces V0. Final exceptional-value conversion/result UNRESOLVED; do not invent an end. |
| S23 | M144a(false), existing nonzero L/list, F0be9=false | Early return before rate/end recomputation. Loading call true bypasses this early gate. |
| S24 | T08c2 object, virtual M144a(true) | M4261 returns; base end writer does not execute. Separate manager path required. |

These are not an executable gameplay model. Cases S19/S20 assume the stated prepared segment and CIL arithmetic, rather than asserting a full text-map geometry result. For malformed/NaN cases, test both direct instrumented state and legitimate file import, keeping those input domains separate. The archive contains no real maps or Aspire inputs; no selected community-map hash or original import success is invented.

## Loading completion and the future performance question

Ordinary path: M223b `18f` calls M28f5; the parser/registration and virtual M28e9→M28ea→M144a occur before that loader returns. M223b then scans start/end bounds and performs handler/setup work (Expedition 04). Work wrapper M0708 only queues completion through M070a after the work delegate returns; M070d invokes completion, and successful base M223c sets F13f0 at `05d`. Exact scheduler threads, queue timing and all Play variant completion/update gates remain UNRESOLVED. Timing postprocessing is therefore inside loading work on this static path, before successful completion dispatch; this is not evidence about which frame first displays the slider.

**User-reported future objective:** investigate serious stuttering associated with unusually distorted sliders, and evaluate preparing expensive slider work before gameplay. This report does **not** confirm its cause or that M144a causes first-visibility stutter. Static M144a already builds segments/cumulative data and some visual state during loading, while other work can remain later. Relevant unknown boundary: which geometry/texture/resource operations remain between prepared F0bdc/F0bde, first visibility and frame submission. Future Work 1 should instrument original loading, first consumer/visibility and frame spikes; eventual Work 2 must evaluate precomputation/caching only with measured frame-time and memory costs. No optimization was implemented or approved as established behavior.

Smallest next gameplay investigation: recover the ordinary **linear slider segment preparation and length-adjustment contract**, then identify its first consumer outside the loading pass. This resolves the principal opaque input to the exact duration procedure and establishes a concrete boundary for a later first-visibility/performance experiment. Full curve rendering, GPU caching and scoring remain separate expeditions.

# 3. Pseudocode

Documentation only. CONFIRMED static structure, RECONSTRUCTED aliases; `CIL_r4`, `CIL_r8` and `CIL_i4` preserve original conversion categories. For finite in-range inputs `CIL_i4` truncates toward zero. Exact original x86 evaluation and out-of-range conversion remain UNRESOLVED. Opaque calls can throw/mutate state; this is not a replacement engine.

```text
Select ordinary Osu path:
    M223b switch[0] -> 033
    choose M0da8 or M113e using M2234
    handler manager factory M1141:
        M2234 true -> special M3203; do not assume ordinary pass
        otherwise -> M0651(true)
    ordinary factory M48de.M48e0 -> construct M142d
    initial end = start; rawCount = suppliedCount; R = max(1, suppliedCount)

EffectiveBeatLength_M1be3(query, includeInherited):
    if timingList null or empty: return 0
    baseIndex = 0; inheritedIndex = 0
    for i in actual list order:
        if ordered(CIL_r8(CIL_i4(point[i].time)) <= query):
            if point[i].baseFlag: baseIndex = i
            else: inheritedIndex = i
    d = 1
    if includeInherited and inheritedIndex > baseIndex:
        u = point[inheritedIndex].beatLength
        if ordered(u < 0):
            upper = 10000f if mapStoredMode in {1,3} else 1000f
            d = CIL_r8(ClampSingle(CIL_r4(-u), 10f, upper) / 100f)
    return point[baseIndex].beatLength * d

OrdinaryTravelRate_M28ab(start):
    E = EffectiveBeatLength_M1be3(CIL_r8(start), true)
    if ordered(E > 0): return (A * T) * (1000.0 / E)
    return A * T

OrdinaryLoadPostpass:
    if applicationMode == Edit: return
    count = objects.Count
    if format > 5:
        for each slider: invoke virtual M144a(true)
        M28eb(0, -1)
    else:
        M28ec()  # invokes virtual M144a(true) within old positioning pass
    M28ee(0, -1)

BaseSlider_M144a(recompute):
    recompute = recompute OR (L == 0 OR preparedList == null ? true : F0be9)
    if not recompute: return
    preserve old-duration bookkeeping; M144f()
    if not F0be3: V = virtual manager.M125b(start)  # ordinary M28ab
    segments = ORIGINAL_CURVE_AND_VERSION_PREPARATION(points)
    G = sum of CIL_r8(segment.M110d()) in original order
    F0bdd = G
    Q = A if format < 8 else A / map.M1be4(start)
        # M1be4: M1be6 selects last raw-time <= start, fallback first;
        # factor helper depends on map mode; empty -> 1
    if ordered(G > 0):
        if F0be9 OR L == 0:
            if F290b non-null AND M4189():
                step = (Q * T) / CIL_r8(M41b4())
                n = CIL_i4(Math.Max(0.0, (G + 1.0) / step))
                L = CIL_r8(n) * step
            else:
                auxiliaryCount = CIL_i4(G / Q)
                L = G
        if ordered(Q > L): Q = L
        ORIGINAL_TRAILING_SEGMENT_ADJUSTMENT(segments, G - L, tolerance=.0001)
    retain original F0be9/auxiliary state updates
    if segments nonempty:
        F0bdc = segments
        F0bde = cumulative double sums of CIL_r8(segment.M110d())
    perform original conditional resource cleanup
    if temporary segment count < 1: return  # no final end write
    time = CIL_r8(start)
    for traversal in 0 .. R-1:
        visit segment indexes forward if even, backward if odd:
            length = CIL_r4(cumulative[i] - (cumulative[i-1] if i>0 else 0.0))
            dt = CIL_r8(1000f * length) / V
            perform original conditional timing/visual operations
            time = time + dt
            retain original tick/repeat operations  # no writes to time accumulator
    perform original final-position operation
    end = CIL_i4(time)                      # F2524 at M144a:1378
    perform remaining original resource/manager bookkeeping

Separate Taiko T08c2 path (not ordinary slider algorithm):
    virtual M144a -> M4261 -> return
    M20d7's loading pass invokes M4263(true, scale)
    M4263 multiplies L by R, computes E=M1be3(start,true)
    end = int32_wrap(start + CIL_i4((L / (A*T)) * E))  # store at 0dd
    subsequently multiplies L by supplied scale and prepares other state
    # M20d6 is another end writer/registration path with its own transforms;
    # full alternate-ruleset conversion/visual behavior is UNRESOLVED
```
