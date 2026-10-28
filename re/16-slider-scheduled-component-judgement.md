# 1. Result

Expedition 16 — Scheduled Slider Component Judgement & First Result Handoff. 2026-10-28 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover the original producer of F0bf8, one naturally generated interior checkpoint, its due-time and success/failure branches in M146f, component bookkeeping, and the immediate ordinary ruleset consumer. Control: ordinary T02af Linear slider, T06f6 embedded head, one traversal, ordinary base Play/T0253/T00e8, non-Edit/plain mods, valid presentation and normally returning original helpers. Complete repeats/tail/aggregate judgement, combo/score/accuracy/HP, effects, rendering, devices, caching and performance are outside scope. Necessary shared endpoint branches are recorded without reconstructing those systems.

Read the complete standing contract/identical supplied attachment, cumulative journal and relevant E01–E15 reports/evidence. Preserve E05's ordinary/Taiko correction, E12's post-refresh update-order correction, and E14's resource-order clarification. Earlier files are unchanged. No Work 2 or `reconstructed/` inspection/modification, replacement component/scheduler/scoring model, or authorised exception.

Verified requested remote parent **672e0bd53de831ea788df7922524f84521348c38**, tree **098dd4f8a84d27e228cfdfac0d0aaf393625cad9**. Dedicated remote branch **work1/expedition-16-components** created directly from that commit. Local mirror1715795 has the same tree but a different commit identity; publication explicitly uses the requested remote parent. All66 baseline files fingerprinted before edits; preserve other65 and the exact old contexts prefix. Publication commit and remote parent/tree/branch/every-blob verification are supplied in final handoff.

| Pinned input | SHA-256 |
|---|---|
| osubackup.zip,18 entries |7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| osu!.exe |6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|
| Repository/attached contract |bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9|

Same self-declared Stable40/file-version1.3.3.8; official dated build, authenticity and modification status UNRESOLVED. Available host is Linux, no Wine/dotnet/PowerShell/DISPLAY or `/dev/dri`. Reinstalled dnfile0.18.0/pefile2024.8.26/dncil1.0.2 after a missing-package check. **No original Windows executable, CLR/native method, import, gameplay, instrumentation, capture, differential, pixel, performance or surrogate-slider execution occurred.**

## Deliverables, procedure and actual validation

New report; `tools/slider_component_evidence.py`; `tools/check_slider_components.py`; `evidence/16/slider-components.json`; independently authored proposed `fixtures/16/single-linear-component.osu`; append-only `contexts.md` entry. The map is an unvalidated Windows input proposal, not a passed import or observed tick trace. No copyrighted reference assets, binaries, proprietary source or full disassembly are redistributed.

Competing explanations: list of ticks only versus mixed timestamps; stored object records versus integers; all-overdue loop versus one entry per reached call; instantaneous follow alone versus entry-time constraint; component identity versus due count selecting the success code; head success prerequisite versus independent tracking; generic base consumer versus ordinary override. Privately inspect original normalized CIL/stack flow, complete selected field inventories and virtual slots; extract/check metadata and original token/branch/constant/arithmetic/conversion/array sites. No modern implementation/formula is used as proof.

Notation Mxxxx/Fxxxx/Txxxx denotes EXE0600xxxx/0400xxxx/0200xxxx. Offsets below are **hexadecimal, code-relative, headers excluded**; JSON offsets are decimal. CONFIRMED means static instructions/metadata. Semantic aliases, player terminology and independent pseudocode are RECONSTRUCTED. Runtime settings, reachability/interleavings, CLR exceptional behavior and displayed results remain UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/slider_component_evidence.py /path/osubackup.zip > /tmp/components-a.json
python re/tools/slider_component_evidence.py /path/osubackup.zip > /tmp/components-b.json
cmp /tmp/components-a.json /tmp/components-b.json
cmp /tmp/components-a.json re/evidence/16/slider-components.json
python re/tools/check_slider_components.py /tmp/components-a.json
python -m compileall -q re/tools
```

Evidence covers70 methods, full managed selected-field/caller inventories, exact ordinary dispatch/declarations/MethodImpls, collection signatures, floating literal bits and array stores. Full scan19659 nonzero-RVA bodies/1739zero-RVA/0decode errors; selected branch destinations verified at instruction boundaries. JSON SHA256 **02846eab9fac8fd48c9e775141239f64f78e4d2def757b9dda5745c4845915b3**. Final deterministic extraction/byte comparisons, focused checker, all fourteen unchanged E02–E15 checkers, Python compilation, historical-prefix/diff checks and remote publication verification are recorded in the journal. Focused checker validates187 historical body hashes. Its draft exclusion range was corrected from262 to28d: the due scan legitimately reads F2358 at286; the classification block afterward does not. No historical assertion was changed.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M144a |7b43c|5632|d58260ebed39834edc228b1b21995da8481e87bbc342223db2b812c7c9b7ceef|
| M145c |7dba0|29|c84899a67e247b43727414f8bddbbc300114396c58a5f7068702e830a1d22106|
| M146f |7e70c|899|a51c8ffbe5672e31e1919d8f0467cb0ce7b53ef44146bfb1cfd6eac8df427655|
| M114a |6aa84|40|0807575024205f26b05e0ba7cb21fd11a4541b05d15245bc7fb6167b55b57d71|
| M0ad2 |3b384|5130|2276c97a07e987cdb9b72e2dcf256fcf105a074ebf4e03f910feb58adebcaaec|

# 2. What they found

## What this means to an osu! player

A slider's interior checkpoints are scheduled during loading. On a reached tracking update, stable counts the leading timestamps that have become due, then evaluates **one outstanding checkpoint**. It does not necessarily process every overdue checkpoint in that one call.

For that checkpoint, being inside the follow area now is necessary but insufficient: the current tracking entry must also have begun **at or before its deadline**. Re-entering exactly at the deadline can pass; returning afterward can fail that old checkpoint even while tracking is active again. Releasing/outside-follow state at the check fails it. A missed head is not itself a veto on earning this later component.

The slider remembers outcomes using two counters and a Boolean array. For the first interior checkpoint of our one-traversal control, success returns raw **8**, failure raw **−262144**. Those are original result codes, not literal points or a complete slider judgement. The ruleset receives the parent slider, through an ordinary override before its shared consumer. Full slider breaks, repeats, tail completion and final judgement remain unresolved.

## E16-A — The original schedule is a mixed List<Int32>

CONFIRMED F0bf8 signature06151282c10108 resolves to **System.Collections.Generic.List<Int32>**: TypeRef010000b0 names List`1; TypeSpec1b00007d signature151282c10108 supplies Int32. MemberRefs0a000147/14c/50f/511/8ad/ad2 are that list's constructor/Add/Count/get_Item/set_Item/Clear. F0bf6 is **Boolean[]**, signature061d02; TypeRef0100008f names Boolean.

M142d:01e allocates F0bf8, stores023. This is its only direct managed field assignment. M144a reads/mutates that same list: conditional Clear0bd5, interior Add0f42, traversal-end Add11db, final set_Item14d7. It stores freshly allocated Boolean[count] to F0bf6 at14ed. Other direct reads/copy/subtype paths are inventoried; no claim that every variant uses this producer identically. Full managed field inventory excludes reflection/native/external mutation and aliases beyond directly inspected calls.

Ordinary loading remains M113e→M0651→M48de→M48e0→M142d, registration of the parent, then M28ea/M28ec→M144a(true), before work completion. E05's corrected ordinary path applies; the Taiko empty override does not. M144a's existing recompute, segment-nonempty and helper gates remain. Its Clear0bd5 is inside the nonnull presentation F0bd7 block; do not invent an unconditional clear on every reused/headless call. Fresh construction provides an empty list.

### Spacing and timestamps, preserving numerical order

Use E05 aliases S=parent.F2523, R=F2527 traversal count, V=F0bfc double travel rate, Aspace=manager.F0adf, Q local6. **Aspace is not E07's approach interval F0adb.**

| Original operation | Token/offset evidence |
|---|---|
| Aspace=(100.0*map.F0b56)/map.F0b57 | M28be:009–02b. Multiplier/tick-rate aliases RECONSTRUCTED from original dataflow; no literal-key decoder execution. |
| V start-selected using timing data | M144a:065/06a invokes ordinary M28ab→M1be3(S,true), stores V; F0be3 can suppress refresh. E05's base/inherited selection, single inverse-SV factor and rate fallback remain authoritative. |
| Q=Aspace for map version<8; otherwise Aspace/conv.r8(M1be4(S)) | M144a:08d8–091c. M1be4/M1be6 use raw-double timing-point time, distinct from rate lookup's integer-converted time. |
| Cap Q to stored requested length when ordered Q>L | 09b0–09c0, `ble.un` skips cap for <= or unordered. Geometry adjustment/cumulative lengths remain E06's separate inputs. |
| Tick-distance phase and absolute movement clock | Running double distance local11 starts0 at0d47; carried segment distance local5 starts0 at0d6b; absolute time local8 starts r8(S) at0d52–59. |
| Segment contribution | Prepared cumulative difference→conv.r4 at0e40; add that Single distance as double to local5 at0efb–0f01. Movement duration uses conv.r8(1000f*distance)/V, appends directed Movement records, then advances absolute accumulator at0ef9. |
| Interior spacing loop | At11a8, enter0f08 if carriedDistance is not unordered/<Q and suppression=false. Advance runningDistance+=Q, carry-=Q and residual local40-=Q at0f08–0f1b. Local40 was initialized from the unadjusted geometric sum at0db2–0db3; do not silently replace it with adjusted requested length. |
| Near-end suppression | threshold=0.01(double)*V at0dc4–0dd4. At0f1d–0f2a set suppression=`!(remaining cgt.un threshold)`, i.e. **ordered remaining<=threshold**. Suppressed candidate skips the rest of this segment's checkpoint loop without interior Add;11b5 advances to the next prepared segment, whose Movement work still occurs. Not a universal malformed-value guard. |
| Interior timestamp | runningDistance→conv.r4 at0f32; call M145c at0f33; Add at0f42, **before** later optional checkpoint-sprite construction/registration gates. |

M145c does exactly the conversion sequence **conv.i4(r8(S) + (conv.r8(singleDistance)/V)*1000.0)** (00–1c). It accepts a Single distance; do not retain the producer's earlier double distance instead. This division-then-double-multiply differs from Movement's Single1000*segmentDistance then double division and absolute accumulation. Equal conceptual travel time is not a bit-exact identity of these arithmetic paths.

For each traversal, after all segments, local11 += remaining carried segment distance at11c5–11ca, then M145c(conv.r4(local11)) is **unconditionally appended at11db** on that reached valid traversal path. It is a traversal-end boundary, not an interior tick. Related end/reversal visuals are constructed separately; no full repeat/tail classification is inferred from those sprites. Distance phase carry is adjusted at11e0–1200 for further traversals; one traversal needs no later-repeat reconstruction.

After final end F2524 is set at1378, **only the last schedule entry** is replaced:

`times[N−1] = Max(unchecked(S + CIL_signed_div(unchecked(E−S),2)), unchecked(oldLast−36))`

M3e4d returns unchecked(E−S); M144a:14af call,14b5 signed integer div,14cf literal36,14d2 Math.Max(int,int),14d7 set_Item. This adjusted last deadline differs from the full slider end and the final Movement end. It is not evidence that final displayed judgement happens exactly there. Array allocation follows; F0bf7 is cleared14f8 and receives an index-derived subset at1511–1551, not a duplicate of every interior item. Full later uses of that subset are deferred.

### Natural interior control and chronology limitation

**Conditional static construction, not manual list insertion or a played map:** fresh ordinary Linear parent, points(128,192)→(428,192), requested length300, S2000, one traversal, ordinary map multiplier1/tick-rate1, base timing point(time0,beatLength500), no inheritance, version14, normal geometry/presentation. Original E05/E06 arithmetic gives prepared one segment300, Aspace=Q100, V200, Movement[2000,3500]. The loading producer generates:

| Generated role | Distance / original timestamp | Final F0bf8 entry |
|---|---|---:|
| First interior checkpoint |100; M145c(100f)=2500|2500|
| Second interior checkpoint |200; M145c(200f)=3000|3000|
| Candidate at300 |Remaining0<=0.01*200=2: suppressed as interior|No interior Add|
| Traversal end |300; M145c(300f)=3500|Max(2750,3500−36)=3464|

Thus **[2500,3000,3464]** and fresh Boolean[3], counters0. The first entry is justified as an interior/tick-like checkpoint by the distance producer, separate traversal-end append and original consumer's raw8 branch; no named enum member “Tick” was recovered. Raw result enum T0488 still has only int32 value__, no named literal values.

Proposed independent map `fixtures/16/single-linear-component.osu`, SHA256 **f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84**, supplies that intended simple row/settings. Original parser key/curve marker decoding and successful import are not dynamically verified. The static chain above is conditional on the stated actual constructor/prepared inputs; a Windows run must log these before calling the text fixture validated. Obtain silence.wav using unchanged E01 make_entry_control.py in a fresh private directory; WAV SHA256 c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977. Do not replace E01's three-circle map or publish audio/reference assets.

**Chronological order is not guaranteed generally.** No sort follows final-entry adjustment. With the same rate/Q/S but a conditional prepared one-span length206, interiors2500/3000 survive (remaining6>2), end3030 and adjusted lastMax(2515,2994)=2994: list[2500,3000,2994]. This is a producer-derived finite-state prediction; it is not an observed map import. Positive spacings provide increasing interior distances, but final adjustment can invert the last pair. Integer conversion can also collapse distinct distances into equal times. Nonpositive/nonfinite spacing/rate, exceptional conversion and malformed-loop termination are unresolved outside the finite control; no global chronology/sanitization is manufactured.

## E16-B — Due prefix, one selected entry and exact success condition

The E15 held/space mechanism runs first, with fresh target-time reads, strict distance and entry transition. M146d records a **fresh F2358 into F0bf2** before its original presentation work and later sets F0bf3=true. Neither head quality nor evaluated flag is required by the live M146f path. Existing readiness/current-object/nullable-time gates remain.

CONFIRMED continuation:

| Stage | Original instructions and consequence |
|---|---|
| Initialize | At262 result=0; k=0. |
| Scan prefix | At26c–28b, while k<times.Count and times[k]<=**fresh F2358 per comparison**, increment k. `ble.s`28b→268 includes equality. Stop at the first future entry; no search of later entries. |
| Outstanding gate | Signed unchecked(F0bf1+F0bf5)>=k at29b→355 skips classification. Otherwise recompute that sum into index j at2a0–2ad. Do not replace both sums with one atomic snapshot. |
| Success predicate | followOK must be true at2af/2b1, then **F0bf2<=times[j]** at2b4–2c6; signed bgt fails greater, equality passes. There is no head-field/result read or extra current-time read in this block. |
| Success mutation | Increment F0bf1 at2d1, then Boolean[j]=true at2df. |
| Failure mutation | Increment F0bf5 at31e, then Boolean[j]=false at32c. |
| Finish | Each result arm goes forward to355, optional guarded leave, return at382. **No loop over additional due entries.** |

F0bf1 is the successful-component count **during this pre-completion phase**; F0bf5 counts failures. Their sum supplies the next index even after failure. Boolean false alone cannot distinguish unprocessed from failed because a newly allocated Boolean[] is false too. Later aggregate M1465 also increments F0bf1 at024 when the head's raw result is positive, before its deferred completion arithmetic. Thus it is incorrect to define F0bf1 globally as “ticks only” or include the head in the live index prematurely. Direct F0bf1 stores only M146f:2d1/M1465:024; F0bf5 only M146f:31e. No direct zero reset of those fields was found; fresh CLR defaults are0. Full reset/seek/reuse behavior is not recovered.

F0bf1/F0bf5 and entryTime F0bf2 are signed Int32; F0bf3 is Boolean. Counter increment precedes the Boolean array store. If that original store throws because of inconsistent state, the preceding increment has already occurred and no local rollback is present; a normal return/handoff must not be asserted for that exceptional probe.

This classification tests **followOK and entryTime**, not stored trackingFlag alone. When followOK is true and old tracking=false, entry runs first; its new entryTime may be later than an overdue checkpoint. That component then fails while F0bf3 stays true for following updates. If followOK=false but old F0bf3=true, it fails even though the flag has not yet been cleared. E15's leave helper is invoked **after** component bookkeeping only when !followOK, flag true and new processedCount<total. Accounting for the last component can therefore skip that leave call. Presentation helper durations are not a recovered input grace interval.

No current-time read occurs after the due scan in the classification block. The entryTime read can be from a different sample than the target query and each due comparison. A discontinuity can produce a nonuniform prefix scan; no event-time snapshot or retrospective cursor history is supplied. Exactly-at-deadline success requires the separately recorded entryTime<=deadline too; an actual host callback landing there is not guaranteed.

## E16-C — Raw returns, backlog and repeated calls

Let N=times.Count, R=parent.F2527, k=the scanned due-prefix count, j=old successful+failed count. These aliases denote separately read original fields where present.

| Decision | Raw local result and exact selector |
|---|---|
| No outstanding prefix entry |0, including a due timestamp already processed or a later due timestamp hidden behind the first future entry. |
| Successful entry; **k==N** |128 at2ee. This tests k, **not j+1**. |
| Successful entry; k!=N and signed `k % (N / R)==0` |64 at30c; integer div308/rem309, no invented safe divide. |
| Other successful entry |8 at311. |
| Failure; **new(F0bf1+F0bf5)==N** |−524288 at347. This selector uses newly updated counters, not k. |
| Other failure |−262144 at34f. |

The primary N3/R1 first interior has k1, so successful result8; unsuccessful−262144. No raw code is assigned into parent.F152a or head.F152a, and neither evaluated flag is set by these component branches. Return enum, Boolean outcome, stored tracking state, entry timestamp and future aggregate result remain distinct.

A jump across several deadlines increases k but still processes just index j. Repeated reached calls at the same F2358 can drain that backlog, because E15's nullable gate permits equality. Every call returns at most one raw enum; several original tracking calls per host callback are possible, so this is **not** a proven one-component-per-frame policy. If time has reached all deadlines, an early backlog success can return128 due to k==N even when j is the first interior index. Preserve the actual selector instead of inventing an item-kind lookup. Failure uses a different terminal selector.

Equal timestamps are counted together by the inclusive prefix loop; only one index advances each call. A repeated call with processedCount>=k returns0. A rewind normally lowers k without decrementing counters or undoing Boolean slots, so processed entries are not naturally reprocessed by this method alone. Counter wrap, external writes, reset/recreation, exceptions and later completion remain separate dependencies. Signed unchecked sums/increments can wrap; negative/out-of-range j or N/R=0 can fail via original array/list/division operations. No saturation, clamp or recovery is added.

For the nonmonotone producer prediction[2500,3000,2994], a stableT2995 gives k1: entry2994 is not even examined because entry3000 stops the prefix. AtT3000 k3; repeated calls process remaining indices. This is why a sorted-deadline abstraction would change original behavior.

## E16-D — Immediate ruleset consumer and bounded stopping point

CONFIRMED inherited M0ac5 passes M146f's returned enum at04c and a **separately reread Play.F1380** at057 to virtual M0ad2 at05c, even0. Under the unchanged single-object control that reference is the parent. There is no manager M28d3 decoration/event between these operations. Arbitrary callbacks/reference changes are not ruled out.

Exact ordinary handler T0253 **overrides M0ad2 with M114a**, reuse slot. Its ancestry/MethodImpl evidence resolves it; T0253 inherits M0ad0 unchanged. M114a:

- If obj.F252e, obj.F1527 and raw!=0, call Play.M2282(raw) at01a. Keep this original extra condition; its product meaning is not guessed.
- Always directly call base M0ad2(raw,obj) at022.
- Fresh pre-completion parent has F1527=false and component logic leaves it false, so the extra call is skipped for our control, regardless of F252e.

Base M0ad2 returns immediately for raw0 (001–003), before its object bookkeeping. Nonzero stores **Play.F13ea=obj at00b**, then reads obj.F153b. A large F05f6-dependent prefix remains original opaque work; normal completion reaches local-result dispatch. Do not remove it or claim it side-effect-free.

Positive local result is ANDed with−12648449 at206; optional F05f3.F198e branch additionally ANDs with−14745601 at226. Raw8/64/128 retain their values under both masks; negative values are not masked here. No new flag OR or quality conversion occurs on our recovered path. Shared enum metadata supplies no named literals.

| Returned raw | First relevant shared consumer branch |
|---|---|
|8|293→579: local numeric amount=10, local double parameter=F0604*3.0, common Boolean remains true. |
|−262144|260→426: local numeric amount remains initialized0; local double parameter is virtual M125a(map.F0b54,−4.0,−15.0,−28.0), Boolean remains true. Do not equate F0b54 or this value to a recovered full HP formula. |
|64|2b8→649, distinct from8 despite also used by initial head success. Full branch deferred. |
|128|2c3→719, distinct endpoint-coded route; full tail behavior deferred. |
|−524288|255→3ed, related failure route with Boolean=false at420; full terminal consumer deferred. |

For raw8, the branch may prepare a presentation object from **Play.current** rather than the passed parent, conditional on current being T02af and M4b36()==false; M2ce6 at63f is the first explicit presentation-registration call on that branch. Treat that as a boundary, not a rendered pixel. Resource failures can interrupt it. The raw−262144 branch bypasses that success-specific preparation. No full effects/sound reconstruction is attempted.

For ordinary non-Mania state (F05f3.F1971!=3), these branches converge at0fd0. Raw8 has no positive decoration bits selected by mask12648448, so0fdf→100a; negative−262144 takes0fd5→100a. Thus the first reliably identified common **result-dependent parent store** is **obj.F153d at100c**:10 for raw8,0 for−262144, under the stated prefix/helper/ordinary-state conditions. This field is an object-local numeric amount, **not** proof of displayed points, total score or combo. An earlier shared F13ea store has the same identity for both and does not discriminate outcomes; the optional success presentation boundary can occur before this numeric store.

Stop here. The rest of M0ad2 includes other result consumers, optional M0ad0 and further systems; their full accumulation/HP/combo/effects are not reconstructed. The report does not assert that reaching the numeric store completes gameplay scoring. Initial head success64 and later aggregate M1465 are distinct sources; a head failure can share−262144 numerically without making its object identity/state transition the same.

## Integration, interface and limits

Preserve E10–E15 ordering: corrected gameplay-clock publication; key polling/held publication and queued presses; M2275 selects current from the **old** candidate list; M28e2 refreshes; optional/gated M0ac5→M146f→ordinary M114a/base M0ad2; then M28e3 ordinary object updates; later drawing. M226b:094e/0959/09be/09cf and branches0972/09a6→09c9 preserve E12's correction: skipping intervening ruleset work does not skip reached normal-returning M28e3. Earlier loading/pause returns, missing current, candidate lag, F13d9 rewind gate and original helpers can prevent this component check. No guarantee of same-host-callback head input and tracking is added.

Inputs: parent/head/manager/ruleset identities; candidate/current refs; each clock read; original held masks/mapped pointer/Movement target/strict radius; followOK and F0bf2/F0bf3; List<Int32> times/order; F0bf1/F0bf5/Boolean[]; traversal count; readiness/nullable gate; consumer flags/prefix/resource state. Outputs: one updated outcome counter and Boolean slot if due; raw return; possible guarded tracking leave; parent reference handed onward; nonzero Play.F13ea and scoped F153d store. No global clock, head result, parent evaluated/raw field or schedule timestamp is changed by the recovered component block itself. Helpers/consumer callbacks are not assumed pure.

CONFIRMED: original producer, conversions, branch priority/equality, one-entry control flow, direct writes, dispatch/consumer sites. RECONSTRUCTED: tick/checkpoint/tracking/outcome aliases and independent pseudocode. HYPOTHESIS: none promoted to an algorithm; owner-reported distorted-slider stutter remains unmeasured. UNRESOLVED: runtime parser literals/import, providers/settings/JIT precision/exceptional arithmetic, scheduler interleavings/helper failures, full later components/aggregate/scoring, displayed pixels, performance and provenance. No caching/pre-rendering inference or implementation.

## Conditional static reference predictions

Use the natural-producer control above: times[2500,3000,3464], fresh counters0, array[false,false,false], one traversal, parent unevaluated, original gates/helpers normal. Cursor lies at E15's required Movement position, original held policy passes where stated. “T” below denotes stable values at all relevant reads **unless a case explicitly separates them**; it is not an invented event timestamp. Restore fresh state for independent cases. AtT2500 the first Movement's ideal finite target is(228,192); retain E15's original conversion/evaluation order for actual operands, and assert no screen pixels.

| ID | Discriminating case | Conditional original-code prediction |
|---|---|---|
| C01 | Natural finite producer control | Interior2500/3000 plus adjusted terminal3464; N3, Boolean[3]. No manual entries. |
| C02 | T2499, already following since2400 | k0; no component mutation, return0. Tracking can remain active. |
| C03 | T2500, following since2400 | k1,j0; follow true and entry2400<=2500; successCount1,array[0]true,raw8. |
| C04 | T2501, same uninterrupted tracking | Still k1; unprocessed first checkpoint can succeed because entry predates its deadline. |
| C05 | Enter exactlyT2500, all entry/due samples2500 | Entry written2500, equality passes; raw8. |
| C06 | First/re-entry atT2501 with no previous processed item | Entry2501>deadline2500; failCount1,array[0]false,raw−262144, tracking can remaintrue. |
| C07 | Successful head but no held input atT2500 | follow false; component fails; head success does not substitute. |
| C08 | Unsuccessful head; tracking entered2400 and held/spatial check passes2500 | First component succeeds; no head-result predicate in this block. |
| C09 | No head press, later valid hold begins2400 | Tracking and first component can succeed despite separate missing-head timeout, subject to actual current/update gates. |
| C10 | Held key, outside follow area at due check | follow false, failure even if old trackingFlagtrue. Leave happens afterward if pending components remain. |
| C11 | Release sampled before2500 and remain released | Next reached check fails component. A skipped check is not an instantaneous result at physical release. |
| C12 | Release, then re-enter at2499, remain following2500 | New entry2499<=2500; success. Earlier uninterrupted history not required. |
| C13 | Release and re-enter only2501 | Old deadline fails despite active tracking afterward. |
| C14 | Current spatial equality to tracking radius² | E15 strict clt fails; component fails when due. Head's inclusive selection disk is irrelevant here. |
| C15 | First success raw8 reaches unchanged ordinary consumer | M114a skips completion hook; base stores F13ea parent, selects579 and reaches parent.F153d=10 if interposed helpers return. Not a displayed-score assertion. |
| C16 | First failure raw−262144 | M114a→base426, later parent.F153d=0 under ordinary shared-state conditions. No parent raw/evaluated assignment in component block. |
| C17 | RepeatT2500 after first success/failure, next deadline3000 | processed1==k1; return0, no second processing of index0. Base raw0 returns immediately. |
| C18 | JumpT3100, processed0, entry2400, follow true | k2,j0; process only index0, raw8. A second reached same-time call processes index1/raw8. Third returns0. |
| C19 | Jump beyond all deadlines with follow true/entry early, processed0 | k3; index0 success returns128 because k==N. Not a proof this was a tail item or of final aggregate judgement. |
| C20 | Same all-due jump, follow false, processed0 | First failure returns−262144; only when new processedCount==N does failure return−524288. |
| C21 | Artificial times[2500,2500,3464], fresh counters | T2500 scans k2; one index/call, successive calls advance both; no deduplication. Natural integer-time collapse is possible conditionally, not established for the base fixture. |
| C22 | processed0 versus processed==k versus processed>k | First has outstanding work when k>0; equality/greater skip via signed bge, even if an individual timestamp is due. |
| C23 | Clock rewinds to2499 after index0 already processed | k0,processed1; return0, counters/slot not undone by this method. Other seek/reset paths unresolved. |
| C24 | Entry recorded2501, due scan later seesT2500 | Deadline2500 due, entry2501 still greater: failure. Do not merge separate reads. |
| C25 | Due-scan T changes between iterations | Each comparison uses its own sample; k can differ from a fixed-time snapshot. No extra time reread in following classification. |
| C26 | Nonmonotone naturally derived conditional[2500,3000,2994] atT2995 | k1; later2994 hidden behind3000. AtT3000 k3; one outstanding entry/call. |
| C27 | Requested length equals Q, positive V | Near-end test suppresses the endpoint as interior; separate end append remains. First entry is not automatically a tick. |
| C28 | Remaining distance exactly0.01*V at spacing boundary | Ordered<= suppression is true; no interior Add for that candidate. Just greater can append. |
| C29 | Candidate absent/old currentnull or readiness return | M146f unreached, no component decision; later refresh alone is not immediate catch-up. |
| C30 | Nullable gate stores2500, actualT2499 versus2500 | Rewind can skip optional tracking; equality allows another call. M28e3 still follows normal post-refresh rejoin. |
| C31 | Due failure with old trackingtrue and processedAfter<N | Counter/slot first, then M146e guarded leave. If processedAfter==N, this leave call is skipped. |
| C32 | F0bf2<=deadline but current followOK=false | Failure; entry timestamp alone is insufficient. |
| C33 | followOK=true but entry helpers fail/throw before completion | No guaranteed component return/handoff. Normal-return assumptions are material. |
| C34 | Artificial inconsistent counters/array/divisor/overflow | Original signed arithmetic, index and div/rem apply; exception/overflow outcomes not sanitized or asserted as natural gameplay. |
| C35 | raw8, parent has completed/evaluated flag artificially set only before consumer | M114a's F252e/evaluated/nonzero hook can run. This is a separate state probe, not the fresh component path. |
| C36 | M0ac5's current ref changes before its reread | Returned result and forwarded object need not share identity by static proof alone; log actual references/thread/callbacks. |

These36 cases are hand-derived conditional predictions, not executed original or synthetic scheduling tests. Original Single fields/conversions and CLR evaluation categories are preserved; no forced rounding after every opcode or Python-double replacement. Numerical results outside finite in-range conversion and confirmed operands remain unresolved.

## Minimal pinned-Windows validation and smallest next investigation

**NOT RUN.** Use a disposable offline copy of the pinned reference and proposed one-slider fixture plus generated E01 silence. Preserve archive/EXE/map/audio/config/skin/Windows/GPU/DPI/backend/settings hashes and instrumentation binary/version; verify actual ordinary parent/head identities and actual imported S/E/Q/V/segments/Movement/times rather than trusting fixture intent. Silence tests timing fields, not audible synchronization.

1. Log M144a's Clear/interior Add/end Add/final set_Item/array allocation, actual inputs and final list order. Confirm naturally produced first interior2500 or document actual differing values; do not insert an artificial entry and call it a natural tick.
2. Trace original key polling/held publication/press dispatch; M2275 current before refresh; M226b gate; M0ac5 and M146f entry. Record parent/head/current identities, both evaluated/raw fields, actual held masks, mapped pointer, target/provider delta, Movement record, radius²/distance²/followOK, F0bf2/F0bf3.
3. Probe M146f:286/28b/29b/2ad/2c6/2d1/2df/31e/32c/result arms/return. Log **each actual F2358 operand**, k/j/N, counters before/after and array index/value, not an extra clock sample masquerading as the consumed operand.
4. At M0ac5:05c, M114a and base M0ad2 branch293/260, log raw enum and actual object ref, masks/extra-hook gates, reached branch and scoped F153d store. Separate presentation registration from scoring/output/capture. Do not trace full HP/combo merely to label this result.
5. Fresh runs: uninterrupted hold entering before the checkpoint; release and re-entry before it; re-entry after it; outside-follow-area control; missed/no-head later hold. Natural exact equality may not land; debugger-forced clocks/counters/equality/duplicate lists are separately labelled artificial probes. Keep unmodified natural traces separate.
6. Record thread/order/QPC with original operand samples and tracer overhead. Compare untraced/traced elapsed and update cadence, repeats and dropped events; logging must add no extra key polls, clock updates or presentation calls. Capture is optional separate evidence, not proof supplied by a raw result.

**Smallest next gameplay investigation:** trace the one-traversal parent's completion boundary from the final scheduled deadline and processed state through the exact call to M1465 and its first aggregate-result handoff. Establish whether pending components are drained or bypassed before completion, without expanding into complete repeats/HP/score/effects. This follows the newly established backlog/counter dependency and keeps head, components and whole-slider judgement distinct. Work 2 remains suspended; owner-reported stutter/precomputation/caching remains a separate unmeasured objective.

# 3. Pseudocode

Independent, non-executable RECONSTRUCTED description of CONFIRMED static instructions. Keep original helpers, exceptions, separate reads and conversion categories. This is not a replacement slider scheduler, tick engine or scoring implementation.

## Loading producer for the bounded single traversal

```text
RECONSTRUCTED — reached ordinary M144a with fresh state/nonempty prepared line
retain E05/E06 rate refresh and initial temporary geometry/length sum
Q = version < 8 ? Aspace : Aspace / r8(original M1be4(S))
retain original stored-length replacement before the following cap
if ordered Q > storedLength: Q = storedLength
retain original segment adjustment, cumulative storage and nonempty-result gate
if presentation exists:
    retain original presentation clears; times.Clear()
distancePhase = 0(double); carry = 0(double); absoluteMovementTime = r8(S)
remaining = unadjusted geometric length       // original local2, not newly invented requested L
suppressed = false; threshold = 0.01(double) * V
for each original prepared segment in forward traversal order:
    d = r4(cumulative[i] - (i==0 ? 0 : cumulative[i-1]))
    dt = r8(1000f * d) / V
    retain original Movement construction/append at i4(absoluteTime), i4(absoluteTime+dt)
    absoluteMovementTime += dt
    carry += r8(d)
    while not unordered_or_less(carry,Q) and not suppressed:
        distancePhase += Q
        carry -= Q
        remaining -= Q
        suppressed = NOT unordered_or_greater(remaining,threshold)
        if suppressed: leave this segment's checkpoint loop; continue next segment
        t = i4(r8(S) + (r8(r4(distancePhase))/V)*1000.0)   // M145c
        times.Add(t)                           // before optional sprite preparation
        retain original optional presentation work
distancePhase += carry
times.Add(original M145c(r4(distancePhase)))     // traversal-end item, not interior Add
retain original single-traversal end/presentation operations
E = i4(absoluteMovementTime)
if times.Count > 0:
    times[last] = Max(unchecked(S + signedDiv(unchecked(E-S),2)),
                      unchecked(times[last]-36))
outcomes = new Boolean[times.Count]
retain original subset/registration/manager operations; no new sort or counter reset
```

## One scheduled decision inside M146f

```text
RECONSTRUCTED — preserve E15 front end, do not replace it with any-held shorthand
if parent.evaluated or freshF2358 < parent.start: return 0
followOK = ORIGINAL_E15_HELD_AND_STRICT_SPACE_PREDICATE()
if followOK and not tracking:
    original M146d()                 // writes entryTime=freshF2358 before later tracking=true

r = 0; k = 0
while k < reread times.Count and times[k] <= freshF2358:
    k += 1
if unchecked(successCount + failCount) < k:
    j = unchecked(reread successCount + reread failCount)
    if followOK and entryTime <= times[j]:
        successCount = unchecked(successCount + 1)
        outcomes[j] = true
        if reread times.Count == k:
            r = 128
        else if signedRem(k, signedDiv(reread times.Count, traversalCount)) == 0:
            r = 64
        else:
            r = 8
    else:
        failCount = unchecked(failCount + 1)
        outcomes[j] = false
        r = unchecked(reread successCount + reread failCount) == reread times.Count
            ? -524288 : -262144
    // No loop back for remaining overdue entries.
if not followOK and tracking and unchecked(successCount+failCount) < reread times.Count:
    original M146e()                 // leave AFTER the component decision
return r
```

## Immediate handoff and stopping point

```text
RECONSTRUCTED — reached ordinary M0ac5 continuation
r = reread Play.current.virtualM24c3(storedPointer)      // parent M146f
obj = separately reread Play.current
virtual ruleset.M0ad2(r,obj) -> ordinary M114a:
    if obj.F252e and obj.evaluated and r != 0:
        original Play.M2282(r)
    direct base M0ad2(r,obj):
        if r == 0: return
        Play.F13ea = obj
        retain original F153b read and F05f6-dependent prefix
        localResult = r
        retain positive-mask operations; raw8 and -262144 remain unchanged
        if localResult == 8:
            numericAmount = 10
            auxiliaryDouble = F0604 * 3.0
            retain conditional original success presentation work
        else if localResult == -262144:
            numericAmount = 0
            auxiliaryDouble = original virtual M125a(map.F0b54,-4,-15,-28)
        else:
            ORIGINAL_OTHER_RESULT_BRANCH_BOUNDARY
        // Under ordinary non-Mania/raw8 or -262144 conditions:
        obj.F153d = numericAmount
        STOP_RESEARCH_BOUNDARY       // subsequent full score/HP/combo/effects unresolved

UNRESOLVED: actual Windows/import/provider/JIT/helper/clock interleavings;
complete endpoint, repeats, final aggregate judgement, displayed points/pixels and cost.
```
