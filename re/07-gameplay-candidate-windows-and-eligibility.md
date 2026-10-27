# 1. Result

Expedition 07 — Gameplay Object Candidate Windows & Visual Preparation Eligibility. 2026-10-27 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover ordinary osu!standard's connection from loaded objects to candidate refresh, per-object eligibility and visual-preparation dispatch. One ordinary circle and one Linear slider are the controlled conceptual targets. No reconstruction component, numerical surrogate, rendering optimization or changes under `reconstructed/`; Work 2 was not inspected and remains suspended.

Read the complete standing contract, cumulative contexts, Expeditions 01–06 and relevant evidence. Preserve E05's correction: ordinary M113e → M0651 → M48de, circle factory M48df → M45eb, slider factory M48e0 → M142d. E04's Taiko annotation is superseded, not reused. GitHub starting commit `d88468fb96ee97953a83db5ccd1b308510e854da` has tree `832068fddf73f1fc6057fb244a8f4c222d450948`, matching the local baseline. Dedicated branch `work1/expedition-07-candidates` was created from that exact remote parent. Historical records remain unchanged; contexts receives only an append. No authorised exception.

Reference ZIP SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; EXE `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`; contract `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9`. Same self-declared Stable40/file-version 1.3.3.8 baseline. Dated official build, provenance and modification status remain UNRESOLVED. Static Python analysis on Linux; no original executable or CLR method was executed. Windows settings, captures, actual admission/visibility/frame-time observations remain unavailable.

## Evidence and reproduction

Created this report, `tools/candidate_window_evidence.py`, `tools/check_candidate_windows.py`, `evidence/07/candidate-windows.json`, and appended `contexts.md`. The read-only extractor reuses unchanged E04/E05 collectors. It records 49 methods, metadata/ancestry/virtual declarations, body identities, constants, branches, arithmetic/conversions and token sites; full managed-body inventories of direct accesses to four relevant fields and direct callers of three boundary methods. The complete circle ancestry declarations make the absence of a preparation override checkable. No proprietary bytes, disassembly, source or assets are published.

Mxxxx = MethodDef `0600xxxx`; Fxxxx = field `0400xxxx`; Txxxx = TypeDef `0200xxxx`. Every offset below is **hexadecimal, relative to the first CIL opcode, excluding headers**. JSON contains exact RVAs/file offsets/signatures; EH offsets already use code-relative coordinates. CONFIRMED means direct static evidence, not observed gameplay. Semantic aliases are RECONSTRUCTED.

| Method | RVA | Code bytes | Method-body SHA-256 |
|---|---|---:|---|
| M28e2 | 109568 | 534 | 8fae788b929f022b86564ecadc3e8cf3db92b26989209348f6ccc0b2a16d3db3 |
| M24c2 | e93a4 | 87 | 4b371cef2165363301a4f591cd07bee1b888d9fc633eed4ba96550953b8a9f09 |
| M1448 | 7b1d0 | 50 | 52260b58f73664e968d21532688a0cd2f847c86faf7288e4768b60bdc20478d9 |
| M45fc | 1bd52c | 57 | ed832ef1f967d4a84632ed954dfb804556b05042742271bc35c9fb4be73a49a9 |
| M144b | 7ca64 | 2173 | f2a7a94e4fe319670174d77a6cba8364c553f956068618af029fa0ad12ff3164 |

Competing explanations: an AR-only interval filter versus coarse search/copy plus eligibility; lower search on start versus end; exclusion versus inclusion of the first future object; per-object dirty flag versus constructor opt-in; shared circle/slider preparation versus distinct slots. Discriminating investigation: privately inspect original operands/branch ordering, resolve virtual slots and comparer dataflow, inventory direct flag writers, then check independently extracted sites. No alternative engine or modern documentation was used as proof.

Reproduce from repository root:

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/candidate_window_evidence.py /path/osubackup.zip > /tmp/candidates-a.json
python re/tools/candidate_window_evidence.py /path/osubackup.zip > /tmp/candidates-b.json
cmp /tmp/candidates-a.json /tmp/candidates-b.json
python re/tools/check_candidate_windows.py /tmp/candidates-a.json
python -m py_compile re/tools/candidate_window_evidence.py re/tools/check_candidate_windows.py
```

Actual outcomes: extraction repeated byte-identically; checker PASS for 49 methods and 17 shared E06 body hashes; 19,659 bodies decoded, 1,739 zero-RVA methods skipped, zero decoding errors. Selected branch targets validate against instruction boundaries. E02–E06 historical evidence checkers and Python compilation PASS. Historical-file identity, journal-prefix preservation, exactly three top-level headings and diff checks PASS. Publication commit/tree/parent/branch/blob verification is supplied in the final handoff. Static cases below were **not** run on original osu!. No import/play, differential, image, Windows instrumentation or performance test ran.

# 2. What they found

## What this means to an osu! player

**RECONSTRUCTED interpretation of CONFIRMED instructions:** loaded hit objects can exist in the map's full object list while remaining outside a smaller candidate list. Stable refreshes that list during a gameplay-update path. This list gives objects update attention and supplies a later draw-stage preparation pass, but it is **not an exact visibility window**: its search has a special end-time comparison, and its forward copy includes an object beyond the nominal future bound.

A candidate slider must then pass a separate inclusive time test before its prepared geometry can be used by this presentation-preparation path. Circles share candidate selection, but ordinary circles do not opt into this particular preparation dispatch. Their eligibility method has an additional conditional margin dependency, which is zero in ordinary Play. None of those facts alone establishes when pixels appear or when input can hit an object.

For sliders, loading-time segments and cumulative lengths from E06 are therefore only part of the preparation story. Later M144b may check existing resources, evaluate progress, discard/reset presentation state, and consume prepared geometry. Actual first visibility, chronological first consumer and cause of owner-reported stutter remain UNRESOLVED.

## Objects, time operands and scope of dispatch

| State | Token / evidence |
|---|---|
| Full registered object list | Manager F17fb; M28d2 BinarySearch at `04c`, Insert at `0cb`. Normal parser registration passes false/true as established in E04/E05. |
| Candidate list | Manager F17fd, same list element type as F17fb. M289a allocates separate lists (`011` / `01c`). Copies retain object references, not clones. |
| Full count | F17fc, refreshed from F17fb.Count at M28e2:`012`, not candidate count. |
| Time operand t | Static signed int32 F2358. RECONSTRUCTED map/gameplay-time alias from use with parsed start/end fields. This expedition does not recover its audio/offset/clock-update algorithm. Reads occur separately, not as one atomic snapshot. |
| Start / finalized end | Signed int32 F2523 / F2524. Circle initially equal; ordinary slider final end is written by E05's loading postprocessing. |
| Approach-like interval A | Manager signed int32 F0adb. Assigned by M28be:`12e` after `conv.i4` at `12d`, using map F0b52 and virtual M125a with values 1800/1200/450. Not a complete recovered AR-rendering contract. |
| Lower-search margin B | M28b6: returns **0** when application mode is not Edit=1 (`006 → 014`), or when Edit and M177c(F0398) true. Returns **800** (`016`) only for Edit with that predicate false. |
| Post-end constant C | Readonly static F17e7, assigned **240** by M289b:`022–027`; full direct writer inventory has this one store. |
| Preparation opt-in | Object bool F153a, assigned true by slider M142d:`17f–180`; complete direct access inventory finds this sole managed writer and M28e5 reads. Fresh ordinary circle retains CLR-default false. This is not a recovered dirty/visible/hittable flag. |

For ordinary manager T00e8 (M0651), candidate slot M28e2 and mapping slot M125a resolve to inherited M28e2/M28b3. M28b3 first calls M28b0(raw,1.4,converted manager flags): outside Edit, bit2 halves raw with Max(0,raw/2), then bit16 applies Min(10,raw*1.4). It then linearly maps x below5 from 1800 to1200, above5 from1200 to450, with x=5 returning1200; result is converted to int32 toward zero for finite in-range values. Constants are original, not selected numerical defaults. Literal metadata key decoding, clock-rate scaling elsewhere and non-finite/out-of-range conversion behavior are not assumed. No hit-window F0adc/F0add/F0ade is read by this candidate selector or either eligibility body.

Special manager T0680 overrides candidate refresh with M320a (extra work before direct base call at `050`); T011f overrides it with M0847 and different bounds. Other Play variants and rulesets cannot inherit the ordinary policy by assumption. Optional F290b is retained token-labelled below; it must not silently be considered null in every application mode.

## Exact candidate selection: bounds, comparison and inclusive copy

**CONFIRMED static mechanism, M28e2:**

| Offsets | Operation, in evaluation order |
|---|---|
| `000–012` | Capture optional F290b into local; set full count from F17fb.Count. |
| `017–03a` | Read t. If optional reference is null OR M177c(F0398) true, subtract A; otherwise subtract M28b6(). Null short-circuits the predicate (`01d → 02b`). Save lower bound lo. |
| `03c–048` | Read t again, add A, save upper bound hi. All arithmetic here is unchecked signed int32. |
| `04a–069` | If optional reference exists, lo=Min(lo,reference.F2946), then hi=Max(hi,reference.F2947). These widen the two bounds; no invented selection/editor semantics. |
| `06b–085` | Construct special key M396e(manager,lo), BinarySearch F17fb with MemberRef0a0004be (one item argument, no explicit comparer). If result<0, complement it. Save index k. |
| `086–0ac` | Set j=k. While j<fullCount-1, read objects[j].start. If start>hi, break; otherwise j++. Equality advances. The final object index is not tested by this loop. |
| `0ae–0d8` | If k==fullCount, Clear the existing candidate list (`0bd`). Optional context also clears F1800.F1a6c, then return. Empty full list follows this path. |
| `0d9–0eb` | Otherwise replace F17fd with F17fb.GetRange(k, **1+j-k**). Includes j, even when its start exceeded hi and caused the break. |
| `0f0–215` | Without optional context, return. With it, clear/repopulate a presentation-related collection from candidate object.F1531 through M2903, run conditional M28eb/M28ee, and remove selected noncandidate presentation references. These extra calls are preserved, not reconstructed as a renderer. |

With the ordinary null optional context, bounds are **lo=t-A and hi=t+A**, using two time reads. The lower bound still enters the end-key BinarySearch; it is not a per-object predicate. The upper scan is not `start<=hi` filtering. For a nonempty eligible search tail it includes **the first object with start>hi**, or includes the final object without testing it. A one-object list can thus supply a candidate far before the object's eligibility threshold. No per-object type, judged flag, prepared-segment check or F153a test occurs in the normal selection/copy block.

### The lower search uses end time, unlike ordinary registration ordering

M396e is a special T0792 object derived from the common object base; it stores lo into both start (`009`) and end (`010`). Comparator M24c2:

- If either operand is T0792 (`001–00e`), compare **end fields** F2524 through Int32.CompareTo (`010–021`).
- Otherwise compare starts F2523; equal starts compare the *other* object's NewCombo predicate against this object's predicate (`030–044`). M3e56 tests type flag4. Thus NewCombo=true sorts before false at equal start, while remaining equal entries are not made unique.
- M24c6(int) also compares end, but the inspected BinarySearch uses a hit-object key, not that integer overload. Metadata lists the generic comparison interfaces and the List type specification in evidence.

This establishes an important limit: F17fb is normally maintained by **start ordering**, whereas the special lower search compares **ends**. Overlapping long sliders can make those end values non-monotone. This expedition does not substitute a linear interval search, sort by end or claim BinarySearch finds the globally earliest unexpired object in that state. Exact runtime List.BinarySearch behavior for non-monotone ends and which equal match is returned remain library/runtime dependencies. Even with monotone ends, exact matches are not rewound to the first equal end. The code, not a corrected ideal algorithm, is the recovered contract.

A finalized slider end beyond hi does not by itself exclude it: hi's forward scan reads **start**, not end. Conversely an earlier-start long slider is not guaranteed admission for every overlapping layout because of the ordering mismatch. Use monotone-end controls to isolate the ordinary branch predictions, then add one overlap probe to measure this dependency.

## Refresh, lifetime and gameplay readiness

M28e2 has no cached previous-time comparison, refresh timer or incremental admission queue. Every invocation reaching its body recomputes count/bounds/search and either clears the current candidate list or replaces it with a new range list. Replacement is shallow; object state/geometry is retained. No promise of one invocation per rendered frame follows from this static fact.

Smallest ordinary integration chain:

1. Loading constructs circle T094c via M48df→M45eb/M45ec or slider T02af via M48e0→M142d. Registration inserts their references into F17fb. Loading slider postprocessing finalizes geometry/end before successful base completion (E04–E06).
2. Play update M226b first calls M226a, then loading/readiness helper M2269 at `007`. If helper returns true, update returns at `00e`, before candidate refresh. Additional earlier-return sites `24f`, `853`, `8ba` are controlled by other original state; no unconditional-every-update claim is made. For example F1373 true at `838–83d` selects `83f–853`, ending with M2262 and return. These predicates are preserved without inventing pause/failure semantics.
3. On the path reaching `953–959`, Play.F138f receives virtual M28e2. Later `9cf` invokes virtual M28e3; that later call has additional intervening M4390/F2a90/F13f2/nullable F13d9 gates (`95e–9c3`). Selection can refresh even when that later update dispatch is skipped. Manager M28e1 is another wrapper that calls M28e2 then M28e3 in order (`001`, `007`). No caller is converted into a timing guarantee.
4. M28e3 enumerates F17fd and calls each object's virtual M24b2 (`017`) then manager virtual M28e4 (`01e`). Ordinary circle inherits the one-byte return M24b2; ordinary slider overrides with M1449. M28e4 contains separate type/state-dependent gameplay operations; full judgement is outside scope. Candidate membership is therefore update attention, not an assertion that the object can be hit.
5. A later host draw-stage M46a6 calls M28e5 at `07d`. Manager constructor(true) registers into static F17e3 under a lock (E06). M28e5 locks/enumerates this registry. Edit=1 uses full F17fb up to F17fc and its F153a gate (`042`, dispatch `055`); otherwise Play=2 with F13d8=false skips that manager's candidate loop (`069–076`). Other non-Edit application modes do not get that specific Play readiness suppression. Candidates whose F153a is true dispatch virtual M24b5 at `098`.

Base completion M223c sets instance F13f0=true at `05d`; that is not itself F13d8. A prior M2269 branch at `092–0b4`, when F13f0 and F13f5 are true, requests the current mode through M46ae, resets F13f0=false (`0ae`) and returns true before the readiness transition. M2269 requires F13f0 (`0b6`), virtual M2268 (`0c1`, base returns true), optional F13cf/F13d0 progress (ordered/non-NaN <=0 required if reference exists), optional F13b1 collection TrueForAll(M22c2), and optional F1385.F2d9d/type condition before M0709 at `154`, then **static F13d8=true at `15a`**. Predicate M22c2 and special type semantics remain opaque. M2269 returns true during this incomplete-wrapper processing (`249`); it returns false immediately for null wrapper (`009`) or wrapper.F0344=true (`018`). Do not reduce this helper to `return !F13f0`, or infer successful readiness from geometry alone. Scheduler/inter-thread order, flag resets and all variant completions are not a recovered global state machine.

Relevant collection mutation boundaries: M289a allocates candidate list; normal M28e2 clears/replaces it as above; M28da removes a successfully removed object from F17fd at `065` after full-list removal/count bookkeeping. M28c5 clears/disposes full-list objects but has **no direct candidate-field access**; do not invent an immediate candidate Clear in that method. Other consumers/editing/ruleset paths appear in the full direct field-site inventory; their transitive mutations are not an exhaustive global collection-lifetime proof. Direct inventories also exclude reflection/native writes.

## Eligibility and the circle/slider difference

M24b6 is **abstract**, not executable base code. Ordinary slider T02af overrides it with **M1448**; ordinary circle T094c overrides with **M45fc**. Neither body mutates fields or invokes a rendering operation.

For stable fields, ordinary finite in-range times (preserve separate live reads and unchecked int32 arithmetic):

| Object | Eligibility projection | Exact sites |
|---|---|---|
| Slider | `t >= start-A` AND `t <= end+240` | M1448 first signed blt at `017` rejects early; end+C add at `029`, signed cgt/ceq at `02a–02d` rejects strictly late. |
| Circle | `t >= start-M24c9()` AND `t <= end+240+M28b6()` | M45fc first signed blt `012`; second adds C then B before cgt/ceq `024–034`. Ordinary circle inherits M24c9, which returns manager.A (`006`). Non-Edit B=0. |

Both boundaries include equality. Circle's conditional extra margin is zero in ordinary Play and is not a hit-window proof; candidate membership can continue after this eligibility becomes false. No curve type, repeat count, segment list, hit-window field or object judgement flag is tested in these two bodies. Slider duration affects them through the finalized end field only. These selector/eligibility bodies have no beatmap-version or Linear-curve branch; earlier version-sensitive postprocessing still determines their input end. Malformed int overflow wraps at these arithmetic sites; do not silently saturate or promote the whole expression to mathematical integers. CLR/JIT behavior and natural reachability of injected extremes are unmeasured.

F153a sits **before virtual preparation dispatch**, not inside these eligibility methods. Complete managed direct access scan finds only M142d:`180` storing true and M28e5:`042`/`090` reading it. Circle constructor chain never sets it, so a fresh ordinary circle is skipped by M28e5 regardless of its M45fc result. Circle has no M24b5 override anywhere in T094c→T035a→T04c3 ancestry; even an externally forced true flag resolves to base one-byte return. This does not mean circles are invisible: they construct other visual state during loading. That distinct presentation route is not recovered here.

### Ordinary slider M144b: ordered preparation gates and mutations

After M28e5's application/manager/candidate/F153a gates, virtual M24b5 resolves to M144b. Dispatch is an opportunity, not guaranteed rebuilding:

1. Allocate its closure and store this, then if global F2c24 true return (`00f–016`).
2. If F0bda non-null, its M451d() is truthy, and F2c30=false, return (`017–033`). Existing resource predicates remain opaque; do not assume a particular texture/GPU state.
3. Invoke virtual M24b6 at `035` → M1448. If false, **short-circuit before reading prepared F0bdc**; call M144f (`045`) then return (`04a`). If true but F0bdc=null, take the same cleanup path. Prepared geometry can therefore exist while eligibility fails, and that failure does not clear the geometry fields.
4. If M1468() false OR F0be8 true OR M24a5() true (reads F1528), use progress p=1. Otherwise `p=Min(1f, r4(t-(start-A))/(r4(A)/3f))` with the original int arithmetic before conversion (`069–09f`). M1468 preserves F03c0, application Edit and F0397 predicate ordering; setting purposes remain unresolved. This progress is not an opacity/visibility equation proved here.
5. If p equals cached F0be5 and `(position equals F0be1 OR !F0be2)`, and F0bda/M451d/M27d7 conditions all permit, return at `0ed`. Otherwise store position into F0be1 (`0f5`), call M144f (`0fb`), then store p into F0be5 (`103`), before the later geometry-consumption branches.
6. M144f, when its optional resource conditions pass, invokes M144e then M451e(null). It always clears F0bd9 at `033` and resets F0be5 to0 at `03e`. It does not write F0bdc/F0bde. Downstream resource meanings remain unresolved.
7. Later partial-progress branch reads cumulative F0bde (`12f`); full-progress branch reads F0bdc.Count (`14b–150`). Further endpoint reads/building are in E06. This expedition ends at that established consumer boundary; it does not recover complete rendering, GPU uploads or a first visible frame.

Thus preparation can remain after loading, fail eligibility, return for existing resources, or proceed into progress-dependent consumption. Static call order cannot tell which event is first in an actual run or attribute a frame spike.

## Static reference predictions and interface

**Not original-game test passes.** Unless changed: ordinary base Play/manager, no optional F290b, non-Edit, A=1200 (controlled manager field / x=5 mapping), B=0, C=240, fresh objects, normal-returning helpers, no integer overflow, time stable within each invocation. Candidate examples use distinct monotonically increasing ends to avoid the comparison-order mismatch. Numerical values below are controlled cases, not measured defaults. Geometry cases assume successfully finalized Linear data from E06.

| ID | Input / variation | Branch-derived prediction |
|---|---|---|
| W01 | Circles at1000,5000,10000; t2500 | lo1300,hi3700; k1,j1; candidates[5000]. Expired1000 excluded, later10000 excluded, first beyond hi retained. 5000 exists but M45fc false (t<3800). |
| W02 | Same; t3799 then3800 | At3799 hi4999, list[5000]; at3800 hi5000, scan advances and includes final10000 too. Equality changes candidate range; 5000 eligibility false→true. |
| W03 | Same; t4000 | lo2800,hi5200; candidates[5000,10000]. First inside and following beyond-bound object both copied. |
| W04 | Same; t6200 then6201 | lo5000 exact match keeps5000; lo5001 insertion moves to10000. M45fc for5000 is already false at both times, demonstrating membership and eligibility differ. |
| W05 | Same; t11201 | lo10001 after all ends; Clear existing F17fd, not replace by GetRange. |
| W06 | Empty full list | Count0,k0==Count; candidate Clear, no indexed read. |
| W07 | Only object start5000, t0 | k0,j0; Count-1 loop not entered; object is a candidate although early eligibility false. No admission==visibility inference. |
| W08 | Separately test circle and Linear slider, both start5000; slider end9000; t3799/3800 | Both early eligibility reject at3799, accept at3800; circle F153a=false, slider true. Only slider can dispatch M144b through this manager path. |
| W09 | Circle start=end5000, t5240/5241 | M45fc inclusive late threshold5240; false5241. With the stated monotone control list it remains a candidate at both times; eligibility changes independently. |
| W10 | Slider5000→9000; t9240/9241 | M1448 true then false. With monotone-end list[1000circle,slider,10000circle], slider is still a candidate at9241 (lo8041); failed eligibility takes M144f, no prepared-field read at03d, if earlier resource gates permit. |
| W11 | Same long slider; t4000, hi5200 | Its end9000 exceeds hi but does not reject it. Forward scan reads start5000; geometry/end came from loading. |
| W12 | Same monotone-end list; t10200/10201 | lo9000 exact match retains slider; lo9001 moves past it. M1448 is already false; no claim that candidate expiry is visual lifetime. |
| W13 | F17fb starts[1000,2000,3000], ends[1000,10000,3000] | Lower key compares non-monotone ends. Preserve List.BinarySearch result; globally earliest unexpired candidate is UNRESOLVED. Do not substitute end sorting/linear search. |
| W14 | Several ends exactly lo | Search may return any equal match allowed by target library; no rewind loop. Record returned k and actual copied identities. |
| W15 | Optional F290b non-null, M177c(F0398)=false | Initial lo=t-B (non-Edit B0), instead of the null-context t-A; then Min with F2946 and Max hi with F2947. Additional presentation collection work follows range copy. |
| W16 | Edit, M177c(F0398)=true/false | M28b6 returns0/800 respectively. Edit preparation loops full list, not candidate list; do not generalize ordinary Play dispatch. |
| W17 | Same t and full list across two M28e2 calls, k<Count | Fresh range list replaces F17fd again; no time-change guard. Object identities retained. |
| W18 | Successful full-list removal via M28da | Removes object from candidate list too, after count bookkeeping. M28c5 full reset alone has no direct candidate Clear. |
| W19 | M2269 returns true | M226b returns00e, no refresh at959. Geometry/completion flag alone is insufficient to assume update reached selection. |
| W20 | F13f0=false, incomplete non-null wrapper | Readiness transition at15a unavailable; M2269 returns true through its processing path. Draw Play/F13d8=false also skips candidates. |
| W21 | F13f0=true but required M2268/progress/optional predicate fails | No F13d8 store from this path; do not treat completion as draw readiness. |
| W22 | Candidate slider, F153a=true, Play F13d8=false | M28e5 skips candidate dispatch. With true, dispatch possible; other M144b gates remain. |
| W23 | Same circle/slider candidates, eligible, readiness true | Circle skipped by F153a; slider dispatch resolves M144b. Forcing circle flag externally still resolves empty base M24b5. |
| W24 | Prepared slider data non-null, t<start-A or t>end+240, early resource gates off | M1448 false; M144f then return, geometry collections unchanged by cleanup. Not a geometry-construction failure. |
| W25 | Eligible slider with F2c24=true, or resource/F2c30 early-return combination | Return before M24b6; no eligibility evaluation or corresponding cleanup on that invocation. |
| W26 | Candidate refresh reached, later M226b update gates block M28e3 | New list can exist without per-object M24b2 dispatch in that invocation. Draw timing remains unobserved. |
| W27 | A from x=0/5/10 with no modifier flags | M28be stores1800/1200/450 respectively; admission/eligibility thresholds shift accordingly. This is not a full AR rendering test. |
| W28 | start-A or end+240 overflows signed int32 | Original unchecked wrap and signed comparisons remain; widened/saturating replacement is unsupported. File import/reachability and observed outcome unresolved. |

Research interface: pinned build, concrete Play/manager/object types, application mode, F2358 reads, map F0b52/manager flags/A, optional context/predicate/range fields, full list identities/order/start/end/typeflags, BinarySearch result, candidate list identity/content, F13f0/F13d8/wrapper state, F153a, F2c24/F2c30, resource predicates, prepared collections and progress/cache state. Outputs: refresh/clear/replacement, per-object update opportunity, dispatch/early return/cleanup, geometry-read entry. Do not replace unknown calls, library tie behavior, clocks, visibility or hit arbitration with assumptions.

## Smallest Windows experiment, limitations and next investigation

Original execution remains UNRESOLVED. On a disposable pinned Windows installation, first reproduce E01's environment/hash/settings baseline. Use project-original control-map inputs: one circle at5000 and, in a separate matched variant, one finite horizontal Linear slider at5000 with finalized end9000; add far-separated padding controls only to probe scan boundaries. Pin imported file/audio hashes, format, ruleset/mods, actual A and finalized end. No community assets or distorted-slider cause assumption are needed.

Instrument original M28e2 entry/after79/aftereb (or after Clear), M1448/M45fc, M28e5 before098, M144b entry/early returns, loading completion and F13d8 store. Log managed thread, high-resolution timestamp, F2358, bounds/k/j, full and candidate identities, gates, geometry/resource identities and method duration. Pair with a timestamped frame capture and frame-time trace; define first visibility by a predeclared pixel/region test on the capture, independently of preparation entry. Probe just below/equal/above early and late bounds via normal playback first; debugger-controlled time/flags are separate artificial-state tests, not ordinary observed gameplay. Compare fresh-process and warm repeats, record debugger/capture overhead and do not infer GPU events from managed entry alone.

This distinguishes geometry prepared at load, candidate admission, successful/failed preparation dispatch, first visible frame and measured frame time. It does not establish a cause of stutter until traces correlate costly operations under matched conditions and a discriminating control rules out alternatives. Work 1 implements no optimization; Work 2 remains suspended.

UNRESOLVED: exact clock/audio synchronization, runtime BinarySearch version/ties/non-monotone behavior, all Play/manager variants, natural extreme-state reachability, scheduler/frame interleaving, actual first visibility/hittability, complete circle presentation route and slider downstream resource pipeline. Static branch predictions are conditional; original exceptions and reentrancy can interrupt any chain.

**Smallest following gameplay investigation:** recover the ordinary circle's already-created visual state's time/enable/opacity transition at the first consumer, with the slider head as a comparison. This addresses the gap exposed here: circle candidates never enter M24b5, so the next visible scene depends on a separate presentation route. Bound the work to one circle/head and their time-driven state; avoid full skins, judgement or renderer reconstruction. A Windows trace of W01/W08/W24 is the smallest dynamic validation in parallel with that research.

# 3. Pseudocode

CONFIRMED static operations and order; RECONSTRUCTED names. Independent documentation only, not executable replacement components. Read live fields at original sites. Arithmetic marked wrap32 is unchecked int32; BinarySearch remains the original library operation, including unresolved non-monotone/tie behavior.

```text
ApproachIntervalInput [finite ordinary projection]:
    x = M28b0(r8(map.F0b52),1.4,converted manager flags)
        # outside Edit: bit2 => Max(0,x/2), then bit16 => Min(10,x*1.4)
    if x > 5: value = 1200 + ((450-1200)*(x-5))/5
    else if x < 5: value = 1200 - ((1200-1800)*(5-x))/5
    else: value = 1200
    manager.A = CIL_i4(value)
    # retain original double arithmetic and conversion, not a full AR contract

OrdinaryCompare_M24c2(a,b):
    if a is T0792 or b is T0792: return Int32.Compare(a.end,b.end)
    if a.start != b.start: return Int32.Compare(a.start,b.start)
    return Boolean.Compare(b.hasTypeFlag4,a.hasTypeFlag4)

Refresh_M28e2(manager):
    optional = F290b
    manager.fullCount = manager.fullList.Count
    t1 = Read(F2358)
    if optional == null or M177c(F0398): margin = manager.A
    else: margin = manager.M28b6()       # non-Edit =>0; Edit/predicate-false =>800
    lo = wrap32(t1-margin)
    hi = wrap32(Read(F2358)+manager.A)
    if optional != null:
        lo = Min(lo,optional.F2946)
        hi = Max(hi,optional.F2947)
    key = special T0792(manager,lo)       # start=end=lo
    k = originalList.BinarySearch(key)   # compare end when key participates
    if k < 0: k = bitwiseComplement(k)
    j = k
    while j < manager.fullCount-1:
        if manager.fullList[j].start > hi: break
        j = wrap32(j+1)
    if k == manager.fullCount:
        manager.candidates.Clear()
        if optional != null: manager.F1800.F1a6c.Clear()
        return
    manager.candidates = manager.fullList.GetRange(k,wrap32(1+j-k))
    if optional != null:
        ORIGINAL_OPTIONAL_PRESENTATION_COLLECTION_AND_POSITIONING_WORK

SliderEligibility_M1448(obj):
    if Read(F2358) < wrap32(obj.start-obj.manager.A): return false
    return not (Read(F2358) > wrap32(obj.end+Read(F17e7)))

CircleEligibility_M45fc(obj):
    if Read(F2358) < wrap32(obj.start-obj.M24c9()): return false
    return not (Read(F2358) > wrap32(wrap32(obj.end+Read(F17e7))
                                    +obj.manager.M28b6()))
    # ordinary circle M24c9 returns A; C=240; non-Edit B=0

OrdinaryIntegrationBoundary:
    loading -> construct/register objects -> slider geometry/end postprocessing
    successful base completion sets F13f0
    M226b calls M226a then M2269
    if M2269 returns true: return before candidate refresh
    preserve other original earlier-return paths
    when site959 reached: virtual manager.M28e2()
    preserve intervening update gates
    when site9cf reached: virtual manager.M28e3()
        for each candidate:
            virtual candidate.M24b2()
            virtual manager.M28e4(candidate)  # gameplay side effects unresolved
    independently scheduled draw-stage M46a6 -> M28e5:
        lock registry
        for manager:
            if application Edit: source = full list up to fullCount
            else:
                if not F13d8 and application Play: continue
                source = candidates
            for obj in source:
                if obj.F153a: virtual obj.M24b5()
    # fresh circle: F153a=false, inherited M24b5 is empty
    # ordinary slider: F153a=true, M24b5 resolves M144b

SliderPreparationEntry_M144b(obj):
    allocate original closure/store this
    if F2c24: return
    if F0bda != null and F0bda.M451d() and not F2c30: return
    if not virtual obj.M24b6() or obj.F0bdc == null:
        M144f(); return
    if not M1468() or F0be8 or M24a5(): p = 1f
    else:
        p = Min(1f, r4(wrap32(Read(F2358)-wrap32(start-manager.A)))
                     /(r4(manager.A)/3f))
    if p == F0be5 and (position == F0be1 or not F0be2)
       and F0bda != null and F0bda.M451d() and not F0bda.M451d().M27d7():
        return
    F0be1 = position
    M144f()                     # conditional resource calls; resets cache to0
    F0be5 = p
    continue original conditional cumulative/segment consumption (E06)
    DOWNSTREAM_PRESENTATION_AND_FIRST_VISIBILITY_UNRESOLVED
```
