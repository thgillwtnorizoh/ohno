# 1. Result

Expedition 17 — Ordinary Slider Completion, Pending Components & Aggregate Judgement. 2026-10-28 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover the ordinary parent completion trigger, treatment of pending scheduled components, exact M1465 aggregation and first parent-result handoff. Control: one ordinary Linear T02af with embedded T06f6 head, one traversal, base Play, T00e8 manager/T0253 handler, non-Edit, valid presentation, ordinary live gates and normally returning original operations. Shared manager conversion/group decoration is followed only to identify the aggregate handoff. Full repeats, scoring/accuracy/combo/HP, effects/sounds/rendering, reset/seek policy, performance and reconstruction are excluded.

Read the complete repository contract and identical supplied attachment, cumulative contexts and relevant E01–E16 records/evidence. Preserve E05's ordinary/Taiko correction, E12's post-refresh rejoin correction and E14's resource-order clarification. No earlier research changed. No Work 2 or `reconstructed/` inspection/modification, substitute completion engine or gameplay implementation. No authorised exception.

Verified requested remote parent **4c0e8a441614d20c4cc1c5245d4ebde878385beb**, tree **0671df718835f902a2618d43b2be208a6d85673a**. Dedicated remote branch **work1/expedition-17-slider-completion** created directly from that parent. Local mirror f0ca906 has the same tree and a different commit identity; publication explicitly uses the required remote parent. Fingerprinted71 baseline files; preserve other70 plus exact old journal prefix. Final handoff supplies verified publication commit/parent/tree/branch/blob results.

| Reference input | SHA-256 |
|---|---|
| osubackup.zip,18 entries |7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| osu!.exe |6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|
| Repository/attached contract |bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9|
| Unchanged E16 proposed map |f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84|

Same self-declared Stable40/file-version1.3.3.8; exact official dated release, authenticity and modification status UNRESOLVED. Linux host still lacks Wine/dotnet/PowerShell/DISPLAY and `/dev/dri`. Reinstalled pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 after missing-module check. **No original Windows/CLR/native execution, map import/playback, instrumentation, capture, differential, pixel, performance or synthetic completion-engine execution occurred.**

## Evidence, discriminating procedure and reproduction

Created this report, `tools/slider_completion_evidence.py`, `tools/check_slider_completion.py`, `evidence/17/slider-completion.json`, and append-only journal entry. Reuse unchanged E16 map and E01 independent audio generator; no extra asset or map modification is required to establish the static control. Raw proprietary disassembly, binaries and assets remain private/unpublished.

Competing explanations: completion at adjusted checkpoint versus parent end; processed-count prerequisite versus immediate aggregation; implicit failure/drain versus incomplete numerator; head timing directly determines parent grade versus binary contribution plus an optional quality policy; returned raw result stored in F152a versus return-only value; direct tracking handoff versus common manager event. Inspect original CIL/stack flow, metadata/virtual slots, field writers/readers, array accesses and branch destinations. Keep helpers opaque at their actual boundaries rather than substituting no-ops.

Mxxxx/Fxxxx/Txxxx denote EXE0600xxxx/0400xxxx/0200xxxx. **Offsets hexadecimal, code-relative, headers excluded**; JSON stores decimal offsets. CONFIRMED refers to original static instructions/metadata. Player aliases, finite interpretations and independent non-executable pseudocode are RECONSTRUCTED. Actual Windows state, natural interleavings/JIT behavior and displayed judgements remain UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/slider_completion_evidence.py /path/osubackup.zip > /tmp/completion-a.json
python re/tools/slider_completion_evidence.py /path/osubackup.zip > /tmp/completion-b.json
cmp /tmp/completion-a.json /tmp/completion-b.json
cmp /tmp/completion-a.json re/evidence/17/slider-completion.json
python re/tools/check_slider_completion.py /tmp/completion-a.json
python -m compileall -q re/tools
```

Evidence:60 methods,32 selected direct field inventories, ordinary ancestry/reused slots/MethodImpls, exact signatures/body hashes/token sites/branches/constants/conversions/array sites. JSON436358bytes, SHA256 **4694f24bbee2430a68ef312b089c0fbde1c4c489d728f2eadaf51530da276c89**. Full scan19659 decoded bodies/1739zero-RVA/0decode errors; selected branch targets validated against instruction boundaries. Focused checker validates180 historical body hashes. A draft assertion excluding every array read in M28d3 was narrowed after original367 proved to be a resource-array read, not component bookkeeping; the final assertion excludes array writes. No historical assertion was edited. All actual extraction/regression/compilation/history/publication results are recorded in the journal.

Actual checks: final extraction twice byte-identical and matching public JSON PASS; focused60-method/180-shared-hash checker PASS; all fifteen unchanged E02–E16 checkers PASS; compilation of all research Python PASS. All71 baseline files unchanged before the journal append, other70 and exact journal prefix preserved, exactly three report sections/42 ordered cases and diff whitespace/scope checks PASS. These are evidence/fixture-document checks, not original gameplay validation. Remote publication is independently verified before the final handoff.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M28e4 |1097f8|215|bb57be9a4658f4470bb0cf1e14ae44cf9572dff521e04da01a9fade2a23c34ac|
| M1465 |7df28|254|8a76dd6cd93705ce792566422c7ab2b9cec04a6eab5719518e0f226701e547ab|
| M1466 |7e034|46|265bc5420df0a36f6b9915a735e47e2ad16715368b9534157c3af8d209a3318d|
| M146c |7e3b4|6|cad9dfb117aab3e0888800a80e42c517d42976611767b21b9eb67a6661dfd5e1|
| M28d3 |108454|2651|931f0b853149359d24e26906f45fc824183285d9d1a6c1044cc6cebe41d3e8bd|

# 2. What they found

## What this means to an osu! player

Stable can finish this ordinary slider when gameplay time reaches its **stored end time**, including exact equality. The separately adjusted final checkpoint deadline is earlier in the control map and does not itself finish the parent slider.

Completion does **not wait for every checkpoint to be processed**. The parent uses the successes recorded so far, adds one for any positive head result, and divides by the number of scheduled entries plus one. Unprocessed entries contribute no success, just like explicitly failed entries for this calculation, but they are not marked failed or drained by the completion path. A delayed update can therefore finish the slider with a backlog still present.

A missed head can still leave a positive final result if later components succeeded. A perfect head alone does not guarantee the best whole-slider result. One original policy treats all positive head qualities equally; another, selected by an optional object described below, additionally constrains the final grade by the head's raw quality. The code establishes both branches, not which policy an unobserved installation selects.

The parent is marked evaluated before its aggregate is returned. That prevents ordinary tracking and manager judgement from running on it again. The returned whole-slider raw result is **not written into parent.F152a by this evaluator**. Stored head raw result, returned parent raw result and decorated manager result must remain separate.

## E17-A — Exact parent trigger and separate missing-head timeout

The verified ordinary update chain is:

`M226b:09cf → virtual M28e3 → T00e8.M0653 → base M28e3 → parent.M24b2/M1449 → T00e8.M0654 → base M28e4`

M28e3 loops the refreshed candidate collection F17fd, not the entire registered list. M0654 directly calls base M28e4 at002. Its prefix permits F2a90=true **or** M4390()==false; F2a90=false with M4390 true returns. The Slider2 type test at013 must pass and cast020 resolves T02af; application mode F2c6d must equal Play2 at027. This excludes Edit and preserves alternative recorded-input gates rather than assuming all variants identical.

| M28e4 offsets | Original operation |
|---|---|
|02c–03e|Compute unchecked(parent.start F2523 + manager.F0adc), read fresh F2358; signed bge03e→05d skips timeout for equality or earlier. |
|040–057|If !parent.M1463() (head not evaluated), recheck Play2, call **M28d3(head F0bdf)** at057. Its evaluator M351d→M1884 samples time again. |
|05d–068|Call parent.virtual M24be at05e, read **another F2358 at063**; signed bgt068→0d6 skips only when end>T. |
|06a–079|If !parent.F1527, call **M28d3(parent)** at074; discard return. |

Parent end slot M24be is inherited unchanged: M24be:001 reads **F2524**, return006. This is the E05 finalized end, not F0bf8[last], start+hit width or a fresh duration calculation. Therefore the finite time predicate is **end<=currentT**, inclusive. There is no component counter, array, schedule-count or head-evaluated predicate in this parent branch. Manager M28d3 also rechecks parent.F1527 before dispatching the evaluator.

The head timeout precedes parent completion but is not a universal prerequisite. For E16 S2000/E3500/Wwide150 with stable late time, an untouched head is evaluated by that first branch before parent completion. For a sufficiently short slider ending before start+Wwide, the parent can complete while head.F1527 is still false. Even an already evaluated parent does not bypass the earlier head-timeout branch: later candidate updates can timeout an untouched short-slider head before the parent-evaluated check skips another aggregate. This is not a second parent judgement.

Readiness/loading or live-pause returns in M2269/M226b, missing refreshed candidate, outer M28e4 gates and normally returning helper dependencies remain material. A large clock jump can remove the parent from candidates; no full-list catch-up is proved. A clock change between the head timeout's test/evaluator and the end comparison can change which branches run. Do not replace those samples with one key-event or frame timestamp.

## E17-B — Pending components are not drained or marked failed

CONFIRMED scoped path: neither M1449's ordinary unchanged-geometry work, M28e4's completion branch, M1465, nor M28d3 contains a loop calling the component evaluator to consume remaining entries. The full managed direct field inventory still gives:

- F0bf1 writes only M146f:2d1 (successful scheduled component) and M1465:024 (positive-head contribution).
- F0bf5 writes only M146f:31e (failed scheduled component).
- F0bf6 reference assigned by loading M144a:14ed; M146f's original Boolean stores remain2df/32c. None in the completion/aggregate/manager methods.
- F0bf3 tracking flag writes only M146d:20d and M146e:0fb. Completion does not directly clear it.

M1449 does scan deadlines for its own F0bf4 presentation/audio index, excluding the final schedule item in its prefix loop. That index is **not** the component processed count F0bf1+F0bf5. It performs original presentation/sound work and can conditionally rebuild geometry in Edit/dirty state; these are retained dependencies, not a recovered backlog drain. The control uses ordinary non-Edit/F0be9=false, unchanged loading data. Direct accesses and array-write checks distinguish this work from E16's outcome stores.

M1465 reads F0bf8 **only for Count**, not individual deadlines. It never reads F0bf5, F0bf6, F0bf7 or traversal count F2527. Thus, conditional on otherwise equal successful count, head raw reads and optional-policy state, these situations give the same ratio:

- Three components accounted for with two successes/one failure.
- Two successes and one still unprocessed.
- Equal successful count with differently arranged Boolean outcomes or deadlines.

This does not make those histories equivalent for previously emitted component results, future reset behavior or downstream scoring. It means this specific aggregate does not distinguish them. There is no instruction assigning an implicit failed outcome to unset slots, sorting deadlines, forcing counters to total or waiting for processedCount==N.

Once M1465 writes parent.F1527=true, M146f's entry guard returns0 on later reached calls before touching pending entries. M24c7 selection and M28d3 also reject an evaluated parent. A tracking flag that remains true is not permission to keep processing components. Counters and unprocessed Boolean slots can remain as they were; the positive-head increment changes F0bf1's role, so **after aggregation its sum with failures is no longer solely the scheduled processed index**.

These are direct original-code conclusions conditional on the reached ordinary path. Opaque scheduler/audio/resource helpers are not claimed side-effect-free, and reflective/external/native mutation or unusual reentrancy is not ruled out. No transitive no-side-effects theorem or full reset/seek policy is manufactured.

## E17-C — M1465 aggregation, ordering and two policies

Virtual slot proof: T02af→T0339→T04c3→T0828; abstract M24a9 is reused by concrete **M1465**. The manager's call at M28d3:012 therefore invokes this parent evaluator. It does not use the embedded head's M351d override or the ordinary circle's M1884 for the parent.

| Order / offsets | Exact operation |
|---|---|
|000–002|Set **parent.F1527=true first**. No initial already-evaluated guard inside M1465 itself. |
|007–008|Call virtual M24c4, resolving parent **M146c→M3b53**. M3b53 forwards a cached action M3ba8 to M2fee with argument2; M3ba8 calls M313d on global F2354/F2355. Actual scheduling/native effects unresolved. No pending-list/counter operation is substituted. |
|00d–024|Read **head.F152a at013**. If signed raw>0, increment parent **F0bf1 by unchecked Int32 add**, store024. Zero/negative does not contribute. **head.F1527 is not read.** |
|029–03f|Numerator=`conv.r8(F0bf1)` after that mutation. Denominator=`conv.r8(unchecked(F0bf8.Count+1))`, preserving integer addition before conversion. Double division03e, store double local q03f. **Not integer division.** |
|040–04d|If q is ordered>0, call virtual M24bb→**M1466**. This occurs before selecting the optional policy. |
|052–066|Read F13c3; null gives null, otherwise read reference F198e; compare nonnull at064. Nonnull→head-sensitive branch068; null→branch0c2. |

M1466 reads a fresh F2358 and original start/half-duration, selects a resource-list index, calls **M1469(false,index)**. Its false branch at014→082 bypasses the schedule lookup in its other branch and calls M146a, then original audio helpers. Depending on another time read, M1469 may also call M146b. These are retained original hook boundaries, not a component result dispatch or outcome-array write. No full sound implementation is recovered. If one throws, parent.evaluated may already be true and the manager handoff may not occur; no local catch/rollback appears in M1465.

Let H be each original head raw read, N=times.Count, C=successful-component count on entry. The normal finite projection is q=(C+(H_at013>0?1:0))/(N+1). Actual numerator is read after the first helper and written in place; the optional branch rereads head raw **after** q and its positive hook. These reads must not be made atomic or replaced with one immutable H if callbacks/state change.

F198e is a reference field (signature06129f90), not a Boolean mod flag. This expedition does not assign a settings name/default or claim its presence in every ordinary installation. Both policies are recovered exactly:

| Policy | Priority-ordered return rules |
|---|---|
| F13c3 null **or** its F198e null; branch0c2 | q==1.0 →**1024** (bne.un0cc falls through); else ordered q>=0.5 →**512** (blt.un0de rejects lower/unordered); else ordered q>0 →**256** (ble.un0f0 rejects <=/unordered); else **−131072**. |
| F13c3.F198e nonnull; branch068 | q==1.0 **and a fresh head raw exactly1024** →**1024** (072/084). Else ordered q>=0.5 **and a later head raw signed>=512** →**512** (096/0a8). Else ordered q>0 →**256**; else **−131072** (0ba→0f8). |

There is no head-quality mask in M1465. Any positive head raw supplies one numerator unit, but the optional branch checks exact1024 for the best result and signed>=512 for the middle result. With natural head1024/512/256, all-positive qualities count equally in the null-policy branch; the nonnull policy limits which upper result they permit. Failed or zero head raw can still produce positive results from C>0. Missing **head reference**, however, is an unguarded dereference, not the same as a present unjudged head with raw0.

Meaningful equalities: q==1 is a special exact test, not q>=1; q==0.5 passes the middle threshold; q==0 fails the positive threshold. Artificial q>1 takes the middle route where its head constraint passes, rather than best. Comparisons retain unordered behavior: NaN does not qualify for best/middle/positive. Natural List.Count is nonnegative and valid counters yield finite q; malformed Count/counter overflow is not silently clamped. Int32 increments/count+1 can wrap before double conversion. Original x86 CLR precision/exceptional probes remain unexecuted.

### Returned raw result is not parent.F152a

M1465's only direct field stores are **F1527 at002** and conditional **F0bf1 at024**. It returns one of1024/512/256/−131072 directly. It has **no F152a store**, no outcome array store and no F0bf3 clear. M28d3 likewise keeps the evaluator return in a local and has no F152a store. Relevant reached hooks/M229c/M114a also have no such store; their other effects remain original boundaries.

Under fresh ordinary state without an independent writer, parent.F152a can therefore remain its initial0 while parent.evaluated=true and the manager carries a positive aggregate raw value. This differs from head M1884, which explicitly stores its raw result into head.F152a. Do not patch the original behavior in pseudocode by adding `parent.raw = aggregate`.

Caller guards provide ordinary single processing. Directly invoking M1465 again is an **artificial** probe: it sets evaluated again and can add the positive head unit again. The evaluator itself is not idempotent. Reset M1431→base M24b3 can clear evaluated; its complete counter/reset policy is not recovered here. Loading M2241 can instead set evaluated for past objects without M1465. A true flag alone is not universal proof that an aggregate event occurred.

## E17-D — Whole-slider manager result and first common handoff

CONFIRMED M28e4:074 passes the **parent** to ordinary inherited M28d3:

1. At001/006 return0 if obj.F1527 was already true. Otherwise F17ff=obj at00c, virtual M24a9 at012→M1465, store returned raw in manager local0 at017.
2. BinarySearch full F17fb at01f, store normalized index F17f8 at033 while retaining signed search result. Nonzero aggregate proceeds; M1465's normal returns are nonzero. This search does not enumerate pending scheduled components.
3. Preserve E12 conversion: F13c3 nonnull and manager Perfect16384 gate; masked raw (`520095494`,0x1f000706), unsigned(mask−256)<=1 or unsigned(mask−512)<=2 can replace **manager local** with−131072. Fresh256/512 can convert,1024 does not. No parent/head F152a rewrite or undo of evaluated/head contribution.
4. Exact local−131072/256 increments manager F17f6;512 increments F17f7. At an ordinary group boundary, positive local is OR4 if both group counters0 and no pending registered object, else OR2 if F17f6==0 and no pending, else OR1. Counters reset at boundary. This “pending” scan is about **registered objects' evaluated flags/NewCombo**, not pending slider components. Parent T02af does not take the special T00d8 bypass.
5. Retain original resource/presentation helpers without assuming no-ops. At0a40 invoke **F1804 / T055d.M291a(managerLocal,auxiliaryString,parent)**, using separate null-check/receiver reads. Subscriber M2246:063–06e→M28c2 registers Play.M229c.
6. M229c:002 stores Play.F13ea=parent;00f invokes ruleset virtual M0ad2(managerLocal,parent), resolving **ordinary M114a**. M114a's F252e/evaluated/nonzero condition can now call Play.M2282 at01a because the parent has become evaluated; F252e remains an explicit additional gate. It then directly calls base M0ad2 at022. **Stop at this verified aggregate consumer.**

F252e direct writers are M28d2:079 and M28db:1a3. The former marks a predecessor when registering a following NewCombo object; the latter can mark the final registered object. It is not a new aggregate score field, and no independent full group/UI mechanism is reconstructed. Prior E16's pre-completion component route skipped M114a's hook because parent.F1527 was false; final judgement has a different state at that same dispatch.

For the isolated one-parent control with registered index0/count1, no earlier manager group state, ordinary gates and no Perfect conversion, returned1024/512/256/−131072 becomes manager1028/514/257/−131072. These aliases are original raw codes plus group flags, not literal scores. Original M28d3 numeric mapping supports **300-associated** for1024/1026/1028, **100-associated** for512/514, and **50-associated** for masked256 (including257): numeric assignments at185/1ac/1d3,13a/161,113 feed a later resource lookup. Negative−131072 is the recovered aggregate failure; “miss” is a RECONSTRUCTED alias. No actual displayed label, total score/accuracy/HP or pixels were observed.

| Source | Original identity/result handoff |
|---|---|
| Initial live head input, E15 | Head stores quality; M2295 sends64 or−262144 with **parent** directly through M114a/base, without M28d3 group decoration. |
| Scheduled tracking component, E16 | M146f returns8/64/128/negative variants; M0ac5 forwards result with reread **current parent** directly to M114a/base, including0. |
| Missing-head manager timeout | M28d3(**head**) evaluates M351d/M1884 and uses the manager event with the head reference. |
| Parent completion, this expedition | M28d3(**parent**) evaluates M1465, can convert/decorate the manager local, emits common event→M229c→M114a/base with the parent. |

After the common event M28d3 can call optional F17f9.M2f17(parent), then returns its local result. Callback exceptions/reentrancy and external subscribers remain dependencies. Do not infer that the final handoff automatically drains or records every missing component, or that head/component/parent raw codes are interchangeable.

## Integration, interface and conditional reference predictions

Authoritative managed order remains: gameplay clock/input/queued presses; M2275:094e chooses current from old candidates; M28e2:0959 refreshes; optional M0ac5:09be→M146f can process **one** outstanding entry; then M28e3:09cf updates candidates and reaches M28e4 completion. M226b guards0972/09a6 rejoin09c9 before M28e3, per E12. Thus the same normal-returning reached callback can process one due component **before** aggregating. It is not guaranteed to process all pending entries before completion. Skipped tracking work does not by itself skip the subsequent manager completion check. Earlier returns, helpers, candidate identity, nullable-time rewind gates and separate F2358 reads remain explicit. No physical-frame cadence is asserted.

Interface: parent/head/manager/ruleset refs, S/E, every relevant clock read, refreshed candidate membership, Play/recorded/readiness gates, head raw/evaluated, successful/failure counters, times.Count/order/outcome array, tracking-entry/flag, F13c3 and optional F198e reference, group/mod/subscriber state. Outputs: evaluated parent; conditional in-place head success contribution; returned aggregate; manager-local conversion/index/decoration/event; consumer object identity. Nonoutputs in this scope: no pending-array repair, no failure-counter increment, no parent F152a assignment or forced tracking-flag clear.

**Static control, not original gameplay:** unchanged E16 proposed map, intended S2000/E3500, one traversal, deadlines[2500,3000,3464], N3, Wwide150 from E12's controlled difficulty5. Imported values remain unvalidated. Set policy P0=F13c3 null or F198e null, P1=F13c3.F198e nonnull. Unless stated, parent remains in refreshed candidates, unevaluated on entry, head already classified, original helpers return, no Perfect conversion and stable separate time reads. C/F below are pre-aggregate component success/failure counts; restore fresh state for independent rows.

| ID | Conditional case | Static prediction |
|---|---|---|
| A01 | C3/F0, head1024, P0 or P1 | Numerator4/4, raw1024; F0bf1 becomes4; parent evaluated. |
| A02 | C3/F0, head512 | P0 raw1024; P1 raw512. Both add one head unit. |
| A03 | C3/F0, head256 | P0 raw1024; P1 raw256. Low positive head is not automatically the whole grade under P0. |
| A04 | C3/F0, failed head−131072 | q3/4; P0 raw512, P1 raw256. Missing-head wrapper return−262144 is not substituted for stored head raw. |
| A05 | C0/F3, head1024 | q1/4; raw256 under either policy. Perfect head does not guarantee best aggregate. |
| A06 | C0/F3, failed head | q0; raw−131072 under both policies. |
| A07 | C1/F2, head1024 | q2/4==0.5; raw512 for P0/P1, equality accepted. |
| A08 | C2/F1, head1024 | q3/4; raw512. |
| A09 | C1/F2, failed head | q1/4; raw256 in both policies. Positive final result without head success. |
| A10 | C2/F0, one pending, head1024 | q3/4 raw512; pending slot/counter not filled. Same aggregate as A08, different component history. |
| A11 | C1/F0, two pending, head1024 | q2/4 raw512; no forced failures or backlog drain. |
| A12 | C0/F0, all pending, head1024 | q1/4 raw256; counters end1/0, Boolean slots remain unset. |
| A13 | C0/F0, all pending, failed head | raw−131072; no counter correction to total. |
| A14 | Time3464, adjusted last deadline due | Parent end3500>3464; no parent completion yet. Tracking can separately process one item. |
| A15 | Time3499 | Parent end comparison still skips; no invented extra hit-width margin. |
| A16 | Time3500 exactly, otherwise eligible | Parent completion allowed by inclusive end comparison. |
| A17 | Time3501 | Also allowed, conditional on candidate/gates; not a strict-after-end rule. |
| A18 | Atend, C2/F0/head1024; tracking skipped | Aggregate consumes C2→3/4 raw512, leaving one pending slot. |
| A19 | Same state, optional tracking reaches remaining component and succeeds before completion | C2→3 by M146f, then3→4 head contribution; aggregate raw1024. Separate component result128 was emitted first. |
| A20 | Same reached tracking but due component fails | F becomes1, C stays2; aggregate q3/4 raw512. |
| A21 | Jump2400→3500, C0/F0, head1024, inactive entry starts3500 | Entry after first deadline2500; one failed component, C0/F1. Parent then adds head, q1/4 raw256; other two pending. |
| A22 | Same jump with prior tracking entry<=2500, current follow true | One success only, C1/F0; then aggregate q2/4 raw512. M146f can return128 because all deadlines due, but that does not imply all succeeded. |
| A23 | All components processed before end, raw head asA01 | Aggregate same asA01. ProcessedCount==N is not a trigger prerequisite. |
| A24 | Unset false array slot versus explicitly failed false slot, same C/head/N | Same aggregate; F0bf5 and array contents are not read. Prior component handoffs differ. |
| A25 | Conditional E16 length206 variant[2500,3000,2994], end3030 | Parent uses3030, not final-list2994 or maximum-list3000. Existing C only; no sorting/drain on completion. |
| A26 | Repeat manager update after parent evaluated | M28e4 parent guard skips; direct M28d3(parent) also returns0; later M146f returns0 before pending work. |
| A27 | Artificial direct second M1465 call afterA01 | Positive head can increment again to5; q5/4, notexact1, returns512. Caller guards, not evaluator idempotence, prevent this normally. |
| A28 | Present head with evaluated=false/raw0, C2, completion reached without timeout | q2/4 raw512 P0 or256 P1; head evaluated flag not consulted. Timeout/order preconditions must actually permit this. |
| A29 | Missing/null head reference, artificial partial state | At primary lateT3500, M1463's timeout query dereferences the head before parent aggregation, so it can fail while parent remains unevaluated. If that timeout is skipped (short path) or M1465 is invoked directly, the raw-head dereference follows the parent-evaluated store and first helper. No successful handoff promised. |
| A30 | Proposed short variant length20, V200,Qcapped20,S2000 | No interior checkpoint; one adjusted terminal2064, E2100. At2100, head-timeout bound2150 not exceeded; parent can aggregate before head evaluation. Not an imported/played variant. |
| A31 | A30 with its sole component success/head raw0 | q1/2; P0 raw512, P1 raw256. Shows a positive aggregate with unjudged head under the reached short-path conditions. |
| A32 | A30 laterT2151, parent already evaluated, head untouched and candidate retained | Earlier head-timeout branch can judge head; parent aggregate remains skipped. Separate identities/order. |
| A33 | Artificial q0,1/4,1/2,3/4,1 threshold operands with P0 | Respectively−131072,256,512,512,1024. N3/C+head units produce these finite ratios naturally where stated. |
| A34 | P1 q1/2, head256 versus512 | First raw256, second512; head signed threshold equality512 passes. |
| A35 | P1 q1, artificial head1028 | Best exact1024 head check fails; signed>=512 allows512. No head mask inserted. |
| A36 | Isolated boundary aggregate raw1024/512/256/−131072, group counters0 | Manager results1028/514/257/−131072 under stated ordinary boundary conditions. Parent.F152a still not assigned by this path. |
| A37 | Perfect16384 gate active, aggregate256 or512 | Manager local can become−131072; evaluator-return value and earlier head contribution/evaluated write remain distinct. |
| A38 | Tracking query/counter decision and completion use different F2358 samples | Preserve actual sampled order; end test can finish after a prior skipped/failed tracking decision. No retrospective component replay. |
| A39 | Readiness/pause return or parent absent from refreshed candidates | Completion path unreached; end passing alone does not emit an event. A large jump may affect candidate admission. |
| A40 | Counter overflow or inconsistent N/counters, artificial probe | Unchecked Int32 mutation/add before double division; no clamp or manufactured success range. Original exceptional/target-JIT details unexecuted. |
| A41 | M146c or positive-result hook throws | Parent was marked evaluated first; no local rollback and no guaranteed M28d3 event. Natural exception recovery unresolved. |
| A42 | Head raw changes after its sign read but before P1's later reads | Numerator can reflect earlier positivity while grade uses later exact/threshold read. Do not snapshot head raw across hooks. |

All42 rows are manually branch-derived predictions, not original-game or executable surrogate tests. The primary fixture remains proposed/unvalidated; short/nonchronological derivatives and artificial states are labelled separately. No displayed score, sound, pixel or timing equivalence is claimed.

## Confidence, Windows experiment and next investigation

CONFIRMED: original end/evaluated gates, actual slot resolution, no explicit pending drain in reached methods, aggregation arithmetic/two policies, direct stores/returns and manager event/ordinary consumer. RECONSTRUCTED: player-grade aliases, finite equations and independent pseudocode. HYPOTHESIS: owner-reported distorted-slider stutter cause remains untested. UNRESOLVED: original Windows environment, fixture import, optional-policy selection/default, helper exceptions/reentrancy, reset/seek/candidate behavior under arbitrary jumps, full scoring/effects/sounds, displayed pixels and cost. No optimization/pre-rendering inference or implementation.

Minimal pinned-Windows original experiment, **NOT RUN**:

1. Pin untouched archive/EXE and actual imported E16 fixture, audio/config/skin/environment/instrumentation hashes. E16 proposed map hash above; E01 generated silence WAV SHA256 c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977. Verify actual parent/head types/identities, S/E, schedule order and Boolean length, plus F13c3/F198e and manager mods. Silence does not validate audible synchronization.
2. Log original clock operands and key/held/cursor/tracking state at the last M146f calls; record C/F and Boolean slots before/after each actual component, entryTime/flag, due-prefix/index/result. Then log current/candidate state, optional tracking gate and M28e3/M1449/M28e4 entry. Do not infer absent outcomes from false slots alone.
3. Probe M28e4:039/03e/head057,063/068/070/parent074; distinguish head and parent M28d3 calls. Log each consumed time, not extra clock-function samples. At M1465:002/013/024/03e/04d/05e/072/084/096/0a8 and returns, record evaluated/head raw/numerator/denominator/q/policy and all mutations.
4. At M28d3:017, conversion/decoration sites and event0a40, M229c:00f, M114a:01a/022, log raw versus manager enum and original object refs. Also record parent/head F152a and flags independently after handoff. Do not fill parent.F152a in a debugger to make it agree with the returned value.
5. Fresh natural runs: uninterrupted following, low positive head, missed head with later hold, deliberately failed component, release/re-entry near the last checkpoint. For delayed updates, measure actual scheduler delay and whether candidates/current remain available; log remaining entries rather than assume catch-up. Exact clock boundaries, forced skipped tracking, corrupted counters or null head are separate debugger probes. A short-slider derivative must be separately hashed/import-validated.
6. Record thread/order/QPC and traced/untraced update cadence. Stepping changes the backlog and clock samples, so separate breakpoint/forced-state traces from low-overhead natural logging. Measure instrumentation time/drop count and compare repeats. Original state/events are distinct from any optional capture or displayed judgement.

**Smallest next gameplay investigation:** follow one interior failure−262144 and matched success8 through ordinary M114a/base M0ad2 to the first verified slider-break notification or combo state mutation. Determine what difference is actually emitted, without assuming the aggregate grade or expanding into full score/HP/effects. This closes the player feedback dependency left by E16/E17. Work2 remains suspended until explicit substantial product authorization.

# 3. Pseudocode

Independent non-executable RECONSTRUCTED descriptions of CONFIRMED static instructions. Preserve original helper calls, separate field reads and checked library/unchecked integer behavior; no synthetic completion/scoring engine.

## Manager update continuation

```text
RECONSTRUCTED — exact ordinary slider branch of M28e4
if not (F2a90 or not original M4390()): return
if original object type test has Slider2:
    parent = original cast T02af
    if applicationMode != Play2: return
    if unchecked(parent.start + manager.Wwide) < freshF2358:
        if not parent.M1463() and applicationMode == Play2:
            manager.M28d3(parent.head)       // separate head timeout, fresh evaluator clock
    end = parent.virtualM24be()             // inherited F2524
    if end <= separatelyReadF2358 and not parent.evaluated:
        manager.M28d3(parent)               // no processed-count prerequisite/drain
```

## Parent evaluator M1465

```text
RECONSTRUCTED — called through parent M24a9
parent.evaluated = true                     // first mutation, before any helper
parent.virtual M24c4() -> M146c -> original M3b53(...)
if parent.head.raw > 0:                     // first head raw read, no head evaluated read
    parent.F0bf1 = unchecked(parent.F0bf1 + 1)
q = r8(parent.F0bf1) / r8(unchecked(parent.times.Count + 1))
if ordered q > 0:
    parent.virtual M24bb() -> M1466 -> original M1469(false,originalIndex)

optional = F13c3 == null ? null : F13c3.F198e
if optional != null:
    if q == 1.0 and freshlyRereadHeadRaw == 1024: return 1024
    if ordered q >= 0.5 and laterRereadHeadRaw >= 512: return 512
    if ordered q > 0: return 256
    return -131072
else:
    if q == 1.0: return 1024
    if ordered q >= 0.5: return 512
    if ordered q > 0: return 256
    return -131072

// No times traversal, outcome repair, F0bf5 increment, F0bf3 clear or F152a assignment.
```

## Aggregate handoff and pending-state boundary

```text
RECONSTRUCTED — inherited ordinary manager M28d3(parent)
if parent.evaluated: return 0               // checked BEFORE invoking the evaluator
manager.F17ff = parent
raw = parent.virtual M24a9()                // M1465 now marks evaluated
retain original full-list BinarySearch/index work
localResult = raw                          // not parent.F152a
retain original Perfect-gated conversion and group flags/counter reset
retain original resource/presentation helper work
if original result delegate exists:
    delegate.Invoke(localResult,auxiliaryString,parent)
        -> Play.M229c:
            Play.F13ea = parent
            ruleset.virtual M0ad2(localResult,parent) -> ordinary M114a:
                if parent.F252e and parent.evaluated and localResult != 0:
                    original Play.M2282(localResult)
                direct base M0ad2(localResult,parent)
                STOP_AGGREGATE_CONSUMER_BOUNDARY
retain optional manager post-event helper; return localResult

CONFIRMED scoped backlog consequence:
    completion can consume current C even when C+F < N;
    pending outcomes remain untouched by this aggregate path;
    later ordinary M146f returns0 because parent.evaluated is true.
UNRESOLVED:
    actual Windows/interleavings/optional-policy selection, full reset/seek,
    score/HP/combo/effects/pixels and instrumentation-adjusted real cadence.
```
