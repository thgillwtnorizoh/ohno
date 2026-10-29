# 1. Result

Expedition 22 — Final Slider Checkpoint, Terminal Results & Parent Completion Ordering. 2026-10-29 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope, reference and provenance

Recover one ordinary T02af Linear slider's final scheduled checkpoint, raw128/−524288 consumer branches, and ordering against M1465 parent aggregation. Reuse E21's two-traversal control, ordinary T0253 ruleset/T00e8 manager/base Play, non-Edit/plain mods, stable references, valid presentation and normally returning original operations. No replacement slider/scheduler/score/HP component, complete scoring/effects/rendering, arbitrary repeats, UI or performance work. Work 2 and `reconstructed/` were not inspected or modified. Work 2 remains suspended. No authorised exception.

Read the complete standing contract, cumulative research and relevant E05–E21 findings/evidence. Preserve E05's ordinary/Taiko correction, E12's update-order correction, E14's resource-order clarification and all later evidence. **E20 remains RUNTIME VALIDATION BLOCKED — PREPARATION ONLY; its original-runtime acceptance criteria are incomplete.** Nothing here is an original Windows observation, original numerical probe or synthetic gameplay execution.

Verified remote parent **2e543db50dd068ad573304ff32e7471fa9ea8eea**, tree **88a3b54f602d9918ec414a09bb20d287a3203094**, and all104 baseline blobs/modes. Dedicated branch **work1/expedition-22-terminal-checkpoint** is directly based on that parent. Local mirror46d8925 has the same tree but a different commit identity; remote publication uses the exact requested parent. All historical files, fixtures and E20 materials are preserved; contexts receives only a new entry.

| Rechecked input | SHA-256 |
|---|---|
| osubackup.zip,18 entries, ZIP CRC valid |7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| Original osu!.exe |6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|
| Standing contract |bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9|
| Unchanged fixtures/21/two-traversal-linear.osu |8c5573d0d4fbc84e2898170273c31747b3f8eca232290ee01af65954633e2154|

Self-declared Stable40/file-version1.3.3.8 remains unauthenticated as a dated official build; modification/provenance status UNRESOLVED. Linux static analysis only, no authorised original-Windows execution surface. A fresh original decode first failed for absent dnfile; installed the pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 with `python -m pip`, then re-extracted successfully. Proprietary executable/assets/full disassembly/private data remain outside the repository. The original was not launched.

## Deliverables, method and actual validation

Added this report, `tools/slider_terminal_evidence.py`, `tools/check_slider_terminal.py`, `evidence/22/terminal-checkpoint.json`; appended `contexts.md`. No fixture or prior tool/evidence was changed. Competing explanations: final deadline equals end versus earlier checkpoint; final code identifies final entry versus due-prefix/count selectors; all negatives reset combo versus local Boolean gate; terminal action completes parent versus separate manager update; implicit pending drain versus aggregation of recorded successes. Inspect the pinned original CIL/stack dataflow, signatures, inheritance/slots, selected direct field inventories and branch destinations. No modern osu! formula or alternate implementation is proof.

```sh
python re/tools/slider_terminal_evidence.py /path/osubackup.zip > /tmp/terminal-a.json
python re/tools/slider_terminal_evidence.py /path/osubackup.zip > /tmp/terminal-b.json
cmp /tmp/terminal-a.json /tmp/terminal-b.json
cmp /tmp/terminal-a.json re/evidence/22/terminal-checkpoint.json
python re/tools/check_slider_terminal.py re/evidence/22/terminal-checkpoint.json
python -m compileall -q re/tools re/validation/20
python re/validation/20/check_package.py --archive /path/osubackup.zip
```

The focused evidence covers48 original MethodDefs, direct field/caller inventories, ordinary virtual dispatch, original constants/branches/conversions/array stores and sparse argument/local sites. Full scan19659 decoded bodies/1739zero-RVA/0decode errors. Selected branch destinations land on instructions. Focused checker validates225 historical body hashes. Repeated extraction/public-byte identity, all19 previous research checkers (E02–E19 and E21), E20 package checks, Python compilation, historical preservation and remote publication verification are recorded in the journal/final handoff. These checks establish evidence consistency, not runtime gameplay acceptance.

Actual extraction repeated byte-identically and matched the public JSON: **PASS**. Focused checker, all19 prior research checkers, unchanged E20 package checker and Python compilation: **PASS**. Evidence479478bytes, SHA256 **e6c1484d2adf93a8c8134f7eced418cc2f630664fd5bcc6abe6551745f4abf61**. E20 retains17 NOT RUN cases; no original-game acceptance test ran.

Mxxxx/Fxxxx/Txxxx denotes original EXE MethodDef0600xxxx/Field0400xxxx/TypeDef0200xxxx. Offsets are **hex, CIL code-relative, excluding method headers**; JSON offsets are decimal. CONFIRMED below means static original instructions/metadata. Gameplay terminology, conditional arithmetic and independent pseudocode are RECONSTRUCTED. Windows behavior, actual provider/policy/JIT/interleaving/helper results and pixels remain UNRESOLVED.

| Method | Original body SHA-256 |
|---|---|
| M144a |d58260ebed39834edc228b1b21995da8481e87bbc342223db2b812c7c9b7ceef|
| M146f |a51c8ffbe5672e31e1919d8f0467cb0ce7b53ef44146bfb1cfd6eac8df427655|
| M0ad2 |2276c97a07e987cdb9b72e2dcf256fcf105a074ebf4e03f910feb58adebcaaec|
| M0ad0 |4e276e02dd630837d14b16e469e9d2c393727eb086ae7d25783b6d3787d099cd|
| M28e4 |bb57be9a4658f4470bb0cf1e14ae44cf9572dff521e04da01a9fade2a23c34ac|
| M1465 |8a76dd6cd93705ce792566422c7ab2b9cec04a6eab5719518e0f226701e547ab|

# 2. What they found

## What this means to an osu! player

The game checks a scheduled final component before this slider's stored end. In the E21 control it becomes due at4964ms, while the path and parent object end at5000ms. The reached check uses the player's held/cursor state **then**, together with when the current tracking run began. It is not a retrospective input sample taken automatically at4964, nor a test of the graphical tail's pixels.

A sequential final success adds a combo unit through the ordinary consumer. A sequential final failure **skips the common combo-reset and SuddenDeath-zero-setter block**. That does not guarantee continued play: its separate negative adjustment and shared Play fail/continue decision still follow. It also does not erase earlier head/checkpoint failures or replace the later whole-slider judgement.

The final component can fail while tracking remains marked active. After its processed count reaches N, the existing leave guard skips the leave call. Later parent completion is a separate update operation, and can return a positive whole-slider result despite the failed final component.

Parent completion does not wait for all checkpoints. With delayed/skipped tracking it can finish while the final entry is pending. Raw128 is especially misleading under backlog: any successful outstanding entry can return it once every leading deadline is due. No original-runtime behavior or visible feedback has been measured here.

## E22-A — Actual final entry and the current tracking test

E21's unchanged fixture intends S2000, length300, V200, Q100, two traversals. Conditional original loading gives forward Movement(128,192)→(428,192)[2000,3500], reverse(428,192)→(128,192)[3500,5000], times **[2500,3000,3500,4000,4500,4964]**, N6/R2 and end5000. Successful parser/import/state construction is still unverified.

M144a appends the final traversal boundary through M145c at11d6/11db, originally5000. After the independent parent end store1378, it replaces only the last list entry at14d7:

`Max(unchecked(S + signedDiv(unchecked(E−S),2)), unchecked(oldLast−36))`.

For these finite inputs that is Max(3500,4964)=4964. The Movement end remains5000. The Boolean[6] allocation follows at14e8/14ed; no final-entry type tag is introduced. This is the producer/index proof of final-component identity, not an interpretation of raw128.

| M146f site | Confirmed operation |
|---|---|
|001–016|Parent.F1527 or freshF2358<start returns0 before tracking/component work. |
|01c–00b8|Original E15 held-mask/history/remembered-mask policy. Failed held eligibility yields followOK=false; it does not itself return before a due component can fail. |
|00c6–0116|R=manager.F0ada, or R*2.4000000953674316f when F0bf3 active. Query q=unchecked(freshF2358−original conditional provider delta), distinct from the due-scan time. |
|0133; M1478:00c/01a–01d|First Movement satisfying inclusive start<=q<=end; preserve original last/first fallback and zero-duration behavior. |
|01ee–024e|Original interpolated target, M0ff1 mapped pointer, ordered strict DistanceSquared<radius²; equality/NaN fail. |
|025d→M146d|If followOK&&!active, record a fresh F2358 into F0bf2 before original helpers and later active=true. |
|026c–028b|Count leading entries with times[k]<=a **fresh F2358 each comparison**; equality is due. First future timestamp stops the scan. |
|028d–029b|If signed unchecked(C+F)>=k, skip component. Otherwise reread that sum into j at02ad. No explicit j==N−1 predicate. |
|02af–02c6|Success requires followOK and signed entryTime<=times[j]. Equality passes; no head result or separate terminal width is read. |

In the ordinary sequential control, five prior checkpoints have been accounted for, so stable C+F=5 gives **j5**. At stableT4963 k5; at4964 k6. The counter relationship identifies the final entry; distinct counter/list reads must not be made atomic in the general description.

With delta0 and query4964, the reverse Movement target is the original Single/vector expression

`(428,192) + ((128,192)−(428,192)) * (1f−conv.r4(5000−4964)/conv.r4(5000−3500))`.

Its ideal finite projection is near **(135.2,192)**, not the end(128,192). Actual explicit Single stores/conversions and target-CLR precision remain relevant. This is neither a screen coordinate nor an observed slider-ball center. If the reached check is later, it uses that later query, not the old deadline's position. At q5000 it reaches(128,192); the final checkpoint may still be outstanding if parent evaluation has not yet occurred. Provider correction can separate the target query from unadjusted due/entry times.

The head need not have succeeded. The component block does not read head.F1527/F152a, and no condition requires the parent's eventual aggregate to equal the head's judgement. Missing head **reference**, malformed arrays, missing Movement/resource state and helper exceptions are different from an ordinary missed head and are not silently repaired.

## E22-B — Actual terminal bookkeeping and return

Let C=F0bf1, F=F0bf5, active=F0bf3, entryTime=F0bf2 and outcomes=F0bf6. C/F/entry are signed Int32; active and outcomes entries are Boolean. At stable j5/k6/N6:

| Outcome | Exact original sequence |
|---|---|
| Success | Signed entryTime<=times[5] and followOK. Unchecked C+1→F0bf1 at02d1; outcomes[5]=true at02df; compare **k** with reread Count at02ec; equality sets local result128 at02ee. |
| Failure | F+1→F0bf5 at031e; outcomes[5]=false at032c; compare **new C+F** with reread Count at0345; equality sets−524288 at0347. Otherwise−262144 at034f. |
| Leave guard | !followOK and active and **new C+F<N** are all needed for M146e:037c. At consistent processed6/N6, bge0379→0381 skips it even if followOK=false/active=true. |
| Return | Local enum returned0382 after that guard. No special terminal callback or parent-evaluated store intervenes in this terminal block. |

Thus success increments C but not F; failure increments F but not C. Both record their Boolean before result dispatch. Counter increment precedes array store: an artificial bad array/index can throw after the increment. No local rollback is invented.

The final block directly changes neither parent.F1527 nor parent.F152a, head result/evaluated fields, start/end, prepared path, Movement records or timestamp list. Existing held/entry operations earlier in M146f can separately change remembered mask/entry/tracking. The component completion itself does not clear tracking. Complete callbacks/helpers are not claimed pure.

Repeated reached calls after processed==N return0 from the outstanding gate even before the parent end. A release after a recorded final success does not undo it through this code. A release before the **actual reached** final check can cause failure; physical release time alone does not identify what the consumed held state was. A late re-entry can produce followOK=true with entryTime>4964, failing the old final entry while retaining active=true.

### Due count is not selected-item identity

Raw128 tests k==N, not j==N−1. Raw−524288 tests updated processed count==N, not k==N alone. At stableT5000 with j0 and a sufficiently early entry, success of the outstanding **first interior** can return128. Failure of that same first item instead returns−262144 because processed becomes1, not6. There is no all-outstanding loop or code-based component identity repair.

An actual terminal failure does not reach the post-component leave helper under stable normal counters. A leave-helper exception control is meaningful for an earlier outstanding failure with processed<N; it is not a natural final-j5 leave path. Entry/provider/resource/consumer helpers can still interrupt terminal processing at their original stages.

## E22-C — Ordinary terminal consumer, combo and first failure-state boundary

Dispatch is `M146f → M0ac5:004c/005c → T0253.M114a → base M0ad2:0022`. M0ac5 forwards the enum with a **separately reread Play.F1380 reference**. Fresh parent.F1527=false skips M114a's optional F252e/evaluated/nonzero M2282 hook; raw0 is forwarded but base returns003. Nonzero stores Play.F13ea=obj00b, reads obj.F153b into local4, and retains the F05f6-dependent prefix. Its resource/callback/exception effects are not erased.

Local3 is the common combo-enable Boolean; local0 numeric amount; local2 auxiliary double. Initialization after the prefix: local3=true at01ed/01ee, amount0 at01ef/01f0. Positive AND masks−12648449 at0206 and optional−14745601 at0226 preserve raw128/64/8. Negative−524288 is not positively masked. Result enum metadata remains an Int32 enum without named literal members; no tail/score name is fabricated.

| Raw result | Original branch and immediate values |
|---|---|
|128|Comparison02c3→0719; amount30; auxiliary=F0604*4.0 at071d–072c; direct branch072d→0e2d. **Local3 remains true.** This short branch allocates no sprite and reads no resource. |
|−524288|Comparison0255→03ed; auxiliary=manager.virtualM125a(selectedMap.F0b54,−4.0,−15.0,−28.0):0419; amount remains0; **local3=false at041f/0420**, then0421→0e2d. |
|64|Distinct0649 branch: amount30 and auxiliary*4 too, but optional Play.current slider/resource/sprite/register work from E21 follows. It is not that same short terminal128 branch. |
|8|Distinct0579 branch: amount10, auxiliary*3, separate optional presentation resource. |
|−262144|Distinct0426 branch: same auxiliary anchors as terminal failure, amount0, but local3 stays true. |

All reached routes rejoin0e2d. For ordinary record mode Osu0, bne.un0e39→0fd0 skips Mania-specific work. At0fd5/0fdf the scoped raw values avoid positive decoration handling. **obj.F153d=30 or0 at100c** is the first established result-dependent object-local store on these terminal branches. It is not the current-combo field or proof of total displayed points. Other shared prefix mutations precede it but do not distinguish these two results under matched state.

### Exact combo and SuddenDeath distinction

At119a/119b, local3=false jumps directly to **12c8**, skipping the entire common combo block. Therefore:

- Raw128 keeps true, follows positive arm, reads current combo through M40f0, performs unchecked+1, calls **M40f1:11c7**; maximum-like F1975 and UInt16 snapshot F198a follow.
- Raw−524288 skips positive increment, negative feedback, **M40f1(0):1287**, snapshot0:128f, record bit32 test and **M2979(0):12c3**. It does **not** reset combo through this block, even with SuddenDeath present or oldcombo>20.
- Raw8/64 also increment through that positive arm. Raw−262144 keeps true, takes the reset arm and its original conditional feedback/bit32 route.

T0892.F2674 is the signed Int32 current-combo field. M40f1 is its direct assignment/return setter, sole direct field writer. For a fresh ordinary control with combo0, a positive head and five successful scheduled components give combo6 before the final check; terminal128 predicts7, terminal−524288 predicts preservation of6 through this common block. These are conditional original-setter arithmetic, not observed gameplay values. External/opaque policy/observer behavior is not proven globally side-effect-free.

### Other reached consumers still matter

Skipping combo is not skipping the result consumer suffix:

1. If Play.F13bb!=0, M0ad2:12da invokes virtual **M0ad0(raw,auxiliary,local3,obj)**, ordinary inherited base. M0ad0 calls M0acf at004 only when that Boolean is true. For finite negative terminal auxiliary it then returns at015, without its positive-only M2977/M2978 hooks. Raw128's positive auxiliary can reach those original hooks; no full sound/HP implementation is recovered.
2. At12e9, original `ble.un` chooses subtraction for auxiliary<=0 or unordered, otherwise addition. **M297a(−aux):1301** or **M297b(aux):12f2** is not gated by comboEnabled. Finite ordinary difficulty0..10 gives a negative failure auxiliary; raw128 with F0604>0 takes addition. No live/default F0604 is assumed. E19's target/accumulator updates, flags and independent presentation field remain unchanged.
3. Original record bookkeeping/policy boundary follows. With obj.F153b and nonnull F198e on the reached ordinary branch, result/local3/extra Boolean are wrapped and sent to **M3c57**; otherwise original record setters are used. Treat this as an opaque policy dispatch, not a no-op or decoded setting. Conditional Play-state/non-null observer calls **M19f5(raw):13d8**; optional F05f6 work calls M22a8:13f9.
4. The normal suffix directly calls **Play.M226c:1404** for both nonzero terminal results. E19's target F1857>0 orNaN returns; finite<=0 proceeds through NoFail/Relax-derived/recorded-input/special gates to recovery or the first failed latch. There is no terminal raw argument or new SuddenDeath requirement in M226c.

For example, conditional selected map.F0b54=0 and ordinary difficulty processing give terminal penalty−4. With stable references/no intervening target mutation, target>4 stays positive; target4 becomes0; M226c can then request recovery/failure if its gates permit, despite no terminal combo reset or SuddenDeath setter. Actual target, callback behavior and failed-state display remain unobserved. This bounded consequence uses the already recovered E19 helper, not a new complete HP model.

A nonzero component consumer can thus invoke policy/observer/failure work **before** parent aggregation. If Play.F136a is latched there and helpers return, the established M226b continuation has no additional latch guard before M28e3; parent work can still be reached in that managed invocation. It is not evidence of one physical rendered frame or a visible failure screen.

## E22-D — Exact ordering with parent completion and backlog

| Original order | Boundary |
|---|---|
|Clock/input|E10/E11 publication, polling and queued input precede this reached Play work. No synthetic event-time snapshot is introduced. |
|Current selection|M226b:094e→M2275 picks current using existing candidate list; parent/current can be absent. |
|Candidate refresh|0959→M28e2 refreshes the list used by ordinary object updates. |
|Optional tracking|Original recorded-input/nullable-time gates at0972/09a6 can skip to09c9; otherwise09be→M0ac5 runs M146f and its **complete consumer** before returning. |
|Intervening work|M0ac5 records changed tracking in Play.F138c after the consumer, then returns. M226b:09c4 calls **M2273**, original record-to-F05f9 update boundary; do not declare its helpers pure. |
|Ordinary object updates|09cf→virtualM28e3 resolves ordinary M0653→base. Each refreshed candidate gets M24b2 (sliderM1449), then virtualM28e4 resolves M0654→base. |
|Parent trigger|M28e4 first handles the separate missing-head timeout. Then inherited M24be reads parent.F2524; **freshF2358 at0063**, signed bgt0068 skips only when end>T; if !parent.F1527 call **M28d3(parent):0074**. |
|Aggregation/handoff|M28d3 rechecks evaluated state, sets F17ff, invokes virtual M24a9→M1465:0012, then local conversion/decorations/event→M229c→ordinary consumer. |

E12 remains authoritative: skipping optional tracking does not skip the reached normal-returning M28e3 call. Earlier loading/pause returns, candidate omission, exceptions or callbacks can prevent/delay the path. A large clock jump does not guarantee that the parent survives candidate refresh; cases below explicitly require the relevant current/candidate refs.

For stable time in **[4964,5000)**, the final component can be processed but the parent time predicate fails. At time5000, both can occur in the same reached callback: component first, then object update/aggregate. There is no fixed number of tracking calls per rendered frame.

If optional tracking is skipped or only one earlier outstanding item is processed at5000, the later parent trigger does **not** wait for processed==N, call M146f again, sort/drain the schedule, fill Boolean slots or increment failed count for pending entries. M1449's separate F0bf4 presentation index is not a drain. M1465 sets parent.F1527=true first, so later M146f returns0 before any pending work. Repeated parent manager judgement is also guarded; M1465 itself is not idempotent when artificially invoked directly.

The parent end comparison, tracking query, entry timestamp, due comparisons and helper clocks are separate reads. A component check at4964 followed by an end read5000 can complete both in one managed continuation. A component check5000 followed by a corrected end read4999 can leave the parent incomplete. No guaranteed host scheduling or physical input latency follows.

## E22-E — Minimal aggregate interaction

M1465:002 sets evaluated=true, calls original cleanup hook, then reads **head.F152a**, not head.F1527. Positive raw adds one to C at0024. Ratio at003e is Double:

`q = conv.r8(C after head contribution) / conv.r8(unchecked(N+1))`.

It does not read F0bf5, outcomes F0bf6, subset F0bf7 or individual timestamps; only F0bf8.Count. No parent.F152a store occurs in M146f, M1465 or M28d3. The component enum and aggregate enum are returned/forwarded separately; parent.F152a can remain0 while parent.evaluated=true and an aggregate event is delivered.

| Recorded components / head | Aggregate consequence for N6 |
|---|---|
|Six successes, positive head|C6→7, q1. Null policy returns1024; nonnull policy requires exact head1024 for1024, otherwise head512 permits512, lower positive head gives256. |
|First five succeed, final fails; head1024|C5/F1→C6 after head, q6/7: raw512 under either policy. Positive aggregate after terminal failure. |
|First five fail, final succeeds; positive head|C1/F5→C2, q2/7: raw256. |
|Six successes, missed/zero head raw|C6, q6/7: null policy512; nonnull head-sensitive policy256. |
|Final pending versus explicitly failed, same C5/head1024|Both q6/7/raw512. F differs0 versus1 and consumer history differs; pending Booleanfalse is not rewritten as failure. |
|Only one success processed after all-due jump, head1024|C1→2, q2/7/raw256, even though that one component may have returned128. |

Policy selection is original global F13c3 null or F198e null versus nonnull reference. Actual runtime configuration remains UNRESOLVED. Null policy: exactq1→1024, elseq>=0.5→512, elseq>0→256, else−131072. Nonnull policy adds exact head1024 at the best branch and signed head>=512 at the middle branch. q/cleanup/positive hooks and later head reads are not an atomic snapshot.

M28d3 can subsequently convert/decorate this local aggregate (including existing Perfect handling) before the common event **M291a at0a40→Play.M229c→M114a/baseM0ad2**. This is separate from the direct component route. A later whole-object failure can reset combo through its own branch; preserved terminal combo alone does not establish final combo/UI. Stop at these known consumer boundaries, without total score/accuracy/HP/effects reconstruction.

## Conditional static reference predictions

Use unchanged E21 inputs, chronological times, valid refs/gates/helpers, parent unevaluated, delta0 and stable indicated reads unless explicitly varied. C/F denote component counts **before aggregation**. Unless changed, head1024 has already been evaluated and prior outcomes are genuinely accounted for by original calls. No `.osu` import or numerical case has run in the original game.

| ID | Discriminator | Conditional prediction |
|---|---|---|
| T01 | E21 producer inputs | Last deadline4964, parent/Movementend5000; original fixture unchanged. |
| T02 | Five processed,T4963 | k5,processed5; no new component, return0. |
| T03 | Five processed,T4964,follow true,entry<=4964 | k6,j5,C++,outcomes[5]true,128; parent still unevaluated. |
| T04 | Same but follow false,old active true | F++,false,−524288; leave skipped because processed6; active can remaintrue. |
| T05 | First tracking entry exactly4964, five earlier entries accounted for (possibly failures) | Entry equality passes if original held/strict space passes; final128 possible. No claim previous entries succeeded. |
| T06 | Entry4965, final still outstanding | follow true can retain active but entry>deadline fails;−524288. |
| T07 | Prior five successes, final failure | C5/F1; whole q6/7 with head1024→512 under either policy. |
| T08 | Prior five failures, final success | C1/F5; positive head givesq2/7→256. |
| T09 | Missed head, all six later components succeed | q6/7;512 null policy,256 head-sensitive policy. |
| T10 | All six succeed, head512/256 | Null policy1024; nonnull policy512/256 respectively. |
| T11 | Sequential raw128 ordinary consumer | F153d30; local3true; combo+1, max/snapshot follow; no branch-local resource allocation. |
| T12 | Sequential raw−524288 ordinary consumer | F153d0; local3false; common combo/feedback/SuddenDeath-setter block skipped. |
| T13 | Matched128/64/8 | All keep combo enabled; amounts30/30/10;128 skips the distinct64/8 branch-local presentation work. |
| T14 | Matched−524288/−262144 | Same finite auxiliary mapping, different local3; only latter reaches common reset/bit32 branch. |
| T15 | SuddenDeath32 present, terminal failure, imported difficulty0 | No explicit M2979(0) on this route; later delta−4 still applies. target>4 can continue, target4 reaches0/test, subject to helpers/gates. |
| T16 | Terminal failure consumes zero target after adjustment | M226c can recover/latch failure despite skipped combo reset; not proof of visible game over. |
| T17 | NoFail gate active at M226c | Suppresses that failure continuation, not prior component accounting/target adjustment. |
| T18 | Component already processed,parent time4999 | Parent end5000 still greater; no aggregate yet. |
| T19 | Final outstanding, tracking reached at5000, early entry | Final128 first, then normal parent aggregate can use its added success. |
| T20 | Final succeeded4964; release sampled4965 | Already processed: no undo/second component, return0; pending-count leave gate still skips atN. |
| T21 | Optional tracking skipped, candidate parent reaches end5000 | Aggregate consumes current C/head/Count; final entry can remain pending. |
| T22 | Jump to5000,j0,already following since<=2500 | k6; first interior success can return128; only one entry added, then q2/7 aggregate256 with head1024. Five entries can remain pending. |
| T23 | Same jump but follow false | First outstanding failure−262144, not terminal; leave can run before consumer; parent aggregate uses C0 plus head if reached. |
| T24 | At5000,j4,C4/F0,follow true and early entry | Index4 success can return128; actual final remains pending; subsequent q6/7 aggregate512. |
| T25 | Repeat sameT4964 after final processed | processed6==k6,return0; no duplicate slot/result before parent end. |
| T26 | Parent evaluated with pending entries | M146f returns0 at entry; completion does not imply those entries were processed. |
| T27 | Component consumedT4964, parent end readT5000 | Both decisions can occur on one normal-returning continuation; no shared time snapshot. |
| T28 | Component consumedT5000, parent end readT4999 | Component can finish while parent time condition now skips. |
| T29 | Pending final Booleanfalse versus explicit failurefalse, equal C/head/policy | Same aggregate; different failed counter, processed index and prior result/adjustment history. |
| T30 | Counter/Boolean written, consumer helper interrupted | Earlier component bookkeeping may persist; normal consumer/aggregate continuation is not guaranteed. |
| T31 | Artificial bad array store, or entry helper interrupted | Counter may increment before array exception; entry helper can change entry time before any component result. No rollback/normal result invented. |
| T32 | Leave-helper exception control | Sequential finalj5 skips leave; an overdue earlier failure with processed<N can reach it and interrupt raw handoff. |
| T33 | Artificial nonchronological final entry behind a future timestamp | Prefix stops before it; no global due-count/sort assumption. |
| T34 | Current ref changes between evaluator and consumer reread | Log actual identities; result and supplied object need not be an immutable pair by static proof. |
| T35 | Nonnull policy/observer versus absent | Original suffix may dispatch M3c57/M19f5 before parent work; actual side effects/settings unresolved. |
| T36 | Parent omitted by refresh or earlier readiness/pause return | Neither same-callback completion nor catch-up is guaranteed; no full-list drain substituted. |

All36 cases are manual conditional predictions, not runtime observations or an executable reference model. Signed unchecked counters/subtraction, comparisons, integer Count+1 before double conversion, separate reads and helper/exception order remain explicit.

## Remaining dependency and smallest next investigation

**NOT RUN — WINDOWS HOST REQUIRED.** Reuse E20's existing operator packet and unchanged E21 fixture. First verify actual import/types/parent/head/schedule/Movement/end. Then record final-entry j/k/C/F/Boolean, original held/q/target/mapped pointer/distance/radius/entry operands, each consumed F2358, raw return, M114a/base branch/local3/F153d, combo setter or skipped block, target adjustment and M226c, post-consumer tracking flag, M2273 boundary, refreshed candidate/update/head-timeout/end test, M1465 operands/policy/return and aggregate event. Link original EXE/map/audio/environment/settings hashes. Separate natural uninterrupted/released/re-entered controls from debugger-forced equality/backlog/counter probes.

Measure low-overhead tracing against untraced repeats, record threads/QPC/order/drops and pauses, and add no extra original clock/input/helper calls just for logging. Distinguish submitted geometry, captured pixels and managed results. No trace/capture/overhead result was obtained here. Distorted-slider stutter remains a separate unmeasured objective, with no optimization inference.

Smallest remaining static gameplay question: **how the ordinary Play path constructs/selects F13c3.F198e**, the reference that changes aggregate head-quality policy. Its actual runtime configuration remains an E20 validation priority. Do not infer it from a displayed grade. Work 2 stays suspended.

# 3. Pseudocode

Independent non-executable RECONSTRUCTED procedure grounded in CONFIRMED original instructions. Preserve original helpers, exceptions, separate field/time/reference reads and numeric categories. No replacement terminal engine is provided.

```text
loading continuation:
    retain original parent.end and appended traversal-end timestamps
    times[last] = original Max(unchecked(S + signedDiv(unchecked(end-S),2)),
                               unchecked(oldLast-36))
    // E21 finite control: final4964, parent end5000; not one shared deadline.

reached M146f:
    if parent.evaluated or freshF2358 < start: return 0
    followOK = ORIGINAL held/provider/Movement/mapped-pointer/strict-radius operation
    if followOK and not tracking: original M146d()       // fresh entry read
    k = original due-prefix scan, each times[k] <= freshF2358
    r = 0
    if unchecked(C+F) < k:
        j = unchecked(reread C+reread F)
        if followOK and entryTime <= times[j]:
            C = unchecked(C+1)
            outcomes[j] = true
            r = 128 if k == reread Count else ORIGINAL 64/8 selector
        else:
            F = unchecked(F+1)
            outcomes[j] = false
            r = -524288 if unchecked(reread C+reread F) == reread Count
                else -262144
    if not followOK and tracking and unchecked(C+F) < reread Count:
        original M146e()                  // skipped at consistent processed==N
    return r

ordinary M0ad2(r,separately reread current):
    retain zero return, optional hook, F05f6 prefix and original masks
    amount = 0; comboEnabled = true
    if localResult == 128:
        amount = 30; auxiliary = F0604*4.0
    else if localResult == -524288:
        auxiliary = original M125a(selectedDifficulty,-4,-15,-28)
        comboEnabled = false
    else:
        retain original other-result branches
    retain original ordinary-mode convergence: obj.F153d = amount
    if comboEnabled:
        retain original positive increment / negative feedback-reset-bit32 branches
    retain optional M0ad0(raw,auxiliary,comboEnabled,obj)
    retain original M297b(auxiliary) or M297a(-auxiliary)
    retain record/policy/observer/helpers and Play.M226c()
    // No terminal parent-evaluated or parent.F152a assignment is inserted.

later normal-returning M226b continuation:
    retain post-consumer F138c and original M2273 work
    M28e3 candidate loop -> slider update -> M28e4
    retain separate missing-head timeout
    if parent.end <= freshF2358 and not parent.evaluated:
        original M28d3(parent) -> virtual M1465:
            parent.evaluated = true
            retain original cleanup hook
            if head.raw > 0: C = unchecked(C+1)
            q = r8(C) / r8(unchecked(times.Count+1))
            retain positive hook and original nullable/head-sensitive policy
            return original aggregate enum
        retain manager conversion/decorations/common result event
    // No loop drains pending entries; no completion write to their Boolean slots.

UNRESOLVED: original Windows values, policy configuration, interleavings and pixels.
E20 acceptance remains incomplete. Work 2 remains suspended.
```
