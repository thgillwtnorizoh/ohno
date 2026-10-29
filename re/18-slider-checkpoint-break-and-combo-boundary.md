# 1. Result

Expedition 18 — Ordinary Slider Checkpoint Failure, Break Notification & Combo Boundary. 2026-10-29 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Compare one naturally generated interior checkpoint's raw8 success with raw−262144 failure through ordinary ruleset dispatch, the first current-combo mutation and any directly reached break-associated notification. Control: one Linear T02af parent/T06f6 head, one traversal, ordinary T0253 handler, base Play/non-Edit, score-record mode Osu0, stable object references and normally returning original operations. Full score/accuracy/HP, combo UI, sounds/effects/rendering, multi-repeat/terminal outcomes, replay/input configuration and optimization are excluded. Adjacent result families are retained only where needed to disprove universal equivalence.

Read the complete standing contract/identical supplied attachment, cumulative contexts and relevant E01–E17 evidence/reports. Preserve E05's ordinary/Taiko correction, E12's post-refresh update rejoin and E14's resource-order clarification. No historical record/tool/evidence changed. No Work 2 or `reconstructed/` inspection/modification, replacement combo/break engine, synthetic gameplay simulation or authorised exception.

Verified remote parent **77ce3b4cece9aa16a915860ea021318c433cfe4d**, tree **4119398cb13acf50280ae8102c0b0605f0fadef4**. Dedicated remote branch **work1/expedition-18-slider-break** created directly from it. Local mirror eb29ae3 has identical tree/different commit identity; publication explicitly uses the required remote parent. Fingerprinted75 baseline files; preserve other74 and exact journal prefix. Final handoff identifies verified publication parent/tree/branch/all-file results.

| Pinned input | SHA-256 |
|---|---|
| osubackup.zip,18 entries |7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| osu!.exe |6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|
| Repository/attached Instruction.md |bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9|
| Unchanged E16 proposed one-slider map |f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84|
| Existing independently generated E01 silence WAV |c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977|

Same self-declared Stable40/file-version1.3.3.8; dated official identity/authenticity/modification status UNRESOLVED. Linux host lacks Wine/dotnet/PowerShell/DISPLAY and `/dev/dri`; pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 reinstalled after absent-package check. **No original Windows/CLR/native input/audio, fixture import, gameplay, capture, instrumentation, differential, pixel, performance or surrogate combo execution occurred.**

## Evidence, procedure and actual checks

Created this report, `tools/slider_break_evidence.py`, `tools/check_slider_break.py`, `evidence/18/slider-break.json`, and the journal append. The independently authored E16 fixture and E01 audio remain unchanged; they are proposed runtime inputs, not passed original-game tests. No binaries, reference assets, proprietary source or full disassembly are published.

Competing explanations: every negative result resets combo versus a separate local gate; failure always exits tracking versus independent entry-time failure; direct break event versus gated feedback request/current-combo store/generic observer; F153d or F0bf5 as combo versus separate state; setter callback versus primitive field assignment; parent aggregate duplicates component combo versus its own type-dependent gate. Privately inspect original normalized CIL/stack flow, method signatures/ancestry/slots, field writers/readers and branches. Keep opaque prefix/helpers and exact original dispatch, without substituting no-ops or a modern slider policy.

Notation Mxxxx/Fxxxx/Txxxx = EXE0600xxxx/0400xxxx/0200xxxx. **Offsets hexadecimal, code-relative, headers excluded**; JSON uses decimal offsets. CONFIRMED means static instructions/metadata. Player concepts/aliases and independent non-executable pseudocode are RECONSTRUCTED. Runtime execution, reachability/interleavings and displayed/audible feedback remain UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/slider_break_evidence.py /path/osubackup.zip > /tmp/break-a.json
python re/tools/slider_break_evidence.py /path/osubackup.zip > /tmp/break-b.json
cmp /tmp/break-a.json /tmp/break-b.json
cmp /tmp/break-a.json re/evidence/18/slider-break.json
python re/tools/check_slider_break.py /tmp/break-a.json
python -m compileall -q re/tools
```

Evidence:48 methods, selected field/caller inventories, exact ordinary factory/virtual dispatch, body hashes/signatures/token/constant/branch/arithmetic/conversion sites, original FontCombo/FontComboOverlap/ComboSoundBursts metadata names and optional-observer ancestry. JSON330460bytes, SHA256 **fe7f4952cd27454c880811a2e9c985486a45b6157a4a4a14ef71b6c348db3d92**. Full scan19659 decoded bodies/1739zero-RVA/0decode errors; selected branch destinations valid. Focused checker validates150 historical body hashes. Final repeated extraction/public-byte match, focused checks, all sixteen prior E02–E17 checkers, Python compilation, historical preservation/diff and remote publication checks are reported in the journal. Conditional cases below are manual predictions, not original or substitute gameplay passes.

Actual validation: repeated extraction and public-byte comparison PASS; focused checker PASS48methods/150historical body hashes; all sixteen unchanged E02–E17 checkers PASS; Python compilation PASS. Historical preservation, scope/diff and remote publication verification are recorded in the journal. No original-game test ran.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M0ad2 |3b384|5130|2276c97a07e987cdb9b72e2dcf256fcf105a074ebf4e03f910feb58adebcaaec|
| M0abc |3aa1c|18|74bbd59a7d1936b25aae2768f3035ae322c465b02b360ce3e400431a7b45c5c6|
| M40f0 |19b6cc|7|5370aaaee9dbcd2a83efe9e3f6058118d955bd4ada5c88952629b1253b40b2cf|
| M40f1 |19b6d4|8|016b1a278515e6f28dab3bf1442bd0ca21e8afb8232d1fa8520f3143293768fa|
| M3b31 |1819e0|202|7374324b185ad961ab23c04355d44c498b4350fd16a9b4627e36685cab885193|
| M4924 |1cea5c|443|52263f10b8b199e12cc1067b5b1a94b6480977db8fcbe9f0cc6d1f1ddd8fb257|

# 2. What they found

## What this means to an osu! player

For this ordinary interior checkpoint, a reached successful result **adds one to current combo**. A reached failed result **sets current combo to zero**, including when combo was already zero or only one. The separate “greater than20” condition controls additional feedback, not whether combo resets.

Missing the checkpoint is not the same as losing tracking. A late-entry failure can reset combo while the slider remains actively followed. Conversely, leaving the follow area when no component is due can return raw0 and leave combo unchanged through this consumer. The next due checkpoint can then fail independently.

The inspected path does not emit a dedicated parent-slider “broken” event or set one global sliderBroken flag. It directly changes the ruleset's combo component, conditionally requests feedback through the original resource/playback machinery, and can later forward the raw enum to an optional generic result observer. These are different boundaries. No break sound, displayed animation or physical-frame latency was observed.

## E18-A — Matched, originally produced checkpoint paths

Use unchanged E16's conditional loading control: S2000, E3500, one traversal, schedule[2500,3000,3464], fresh pre-component counters C0/F0 and Boolean[3]. Actual fixture import remains unverified. Positive head input raw64 can previously increment combo0→1; take current combo1 and head raw1024/evaluatedtrue as the main matched pre-state, parent unevaluated, no interfering object, ordinary score-record mode Osu0 and identical consumer configuration.

At the first due deadline, M146f scans k1; j=C+F=0. Original success requires followOK plus entryTime F0bf2<=2500. Success increments F0bf1 and sets outcome[0]=true, then returns8 because k1 is neither N3 nor a multiple of N/R3. Failure increments F0bf5, sets outcome[0]=false and returns−262144 because new processedCount1!=N3. Neither path writes a raw result field or marks the parent evaluated.

Two discriminating pairs, **not rewritten return values**:

| Pair | Success history | Failure history | Inherent post-state differences |
|---|---|---|---|
| Held/spatial pair at stableT2500 | Following since2400, held policy and strict distance pass | Same prior history but original held or spatial predicate fails | C1/F0/true slot versus C0/F1/false slot; failure can leave tracking before returning. |
| Entry-time pair at stableT2501 | Previously entered before deadline; still held/on required path | First reached valid entry occurs2501; original M146d records2501, then2501>2500 fails | Both can have active tracking at the consumer; entry timestamp and outcome counters differ. A new entry transition occurred on the failure call. |

Keep parent/head/current identities, schedule, preceding outcome counts, combo1, record/handler flags and resources matched wherever these outcomes permit. Do not claim every field is identical: the original decision must produce different counters/Boolean outcomes and possibly tracking state. No extra press timestamp, inserted schedule entry or artificial result overwrite is used to derive these paths.

Original return chain is **M146f→M0ac5:04c/05c→ordinary T0253.M114a→direct base M0ad2:022**. M0ac5 rereads Play.current for the forwarded object; a stable current reference is an explicit control condition. M114a's F252e/evaluated/nonzero hook is skipped because this parent is unevaluated. Raw0 is forwarded too; with a valid stable parent, base M0ad2 returns at003 without result bookkeeping.

## E18-B — Consumer divergence and the shared continuation

M0ad2 first stores Play.F13ea=parent at00b and reads parent.F153b. Preserve its F05f6-dependent prefix018–01ed, including account/state/resource/scheduled-callback work; its helpers can throw or mutate state. It is not a result-specific break notification and is not assumed harmless. Initialization M0aaf sets F05f6=true only after its original component setup; a false flag is not automatically a fully initialized ordinary gameplay state.

| Offset / operation | Raw8 versus raw−262144 |
|---|---|
|000–003|Both nonzero; neither takes the base zero-result return. |
|01ff–020c|Copy raw to local. **First differing branch is signed ble0203→020d**: negative skips the positive AND−12648449 at0206; raw8 takes it unchanged. Optional F05f3.F198e positive AND−14745601 at0226 also leaves8 unchanged. |
|0260 /0293|Negative equality selects failure0426; positive equality selects success0579. |
|0579–058c|Success sets local numeric amount10 and auxiliaryDouble=F0604*3.0. |
|0426–0458|Failure leaves numeric amount0, computes auxiliaryDouble via original virtual M125a(map.F0b54,−4,−15,−28). No F0bf5 read occurs here. |
|0593–0644|Success-only optional presentation preparation reads Play.current, requires T02af and !M4b36, and can call M2ce6. Retain these original resource/helper boundaries; no pixels inferred. |
|0e2d–0fd0|Preserve common state/mode-dependent work. Control's F05f3.F1971 is Osu0; Mania3 processing is not generalized. |
|100c|Store parent.F153d=10 for8,0 for−262144 under the recovered control. **This is not the combo mutation.** |
|119a–11a2|Local Boolean3 gates the combo block; it was initializedtrue at01ed and remains true for these two branches. Signed raw<=0 at11a2 selects the reset route; raw8 selects increment. |

Parent.F153b gates other bookkeeping but is not the local Boolean3 combo gate on these scoped branches. Parent.F1527 false suppresses M114a's earlier optional hook; it is not a combo-reset condition in base M0ad2. Parent type is not tested to suppress the raw−262144 or8 combo path. Current Play object is used by optional success presentation, whereas the combo object comes from the handler. Do not turn these facts into a guarantee that all opaque prefix/helpers complete.

## E18-C — Current combo object and exact first writes

CONFIRMED ordinary initialization: T0253 inherits M0abc unchanged. M0aaf:013 invokes that virtual slot; M0abc:007 creates **T0892 via M40ef**, then stores handler **F05f2 at00c**. Constructor M40ef stores its presentation ref and calls virtual M40f3. M40f3 reads original metadata-named **FontCombo** F36ab and **FontComboOverlap** F36ac at01f/025 to build its components. Together with the increment/reset/max/milestone reader dataflow, this grounds **current combo** as a RECONSTRUCTED semantic alias, not a guess based on F153d.

| State | Type / original access |
|---|---|
| handler.F05f2 | Reference to the ordinary T0892 combo component; signature0612a248. Other direct assignments M0c75/M1ee3/M461e belong to alternative paths, not assumed equivalent. |
| T0892.F2674 | Signed Int32 current count. **M40f0:001 getter; M40f1:002 setter**. These are its only direct managed field accessors. |
| T0892.F2673 | Separate presentation/display count/state. It is not the value reset by M0ad2. M40f8 begins by comparing fresh current count with F2673; full display timing/effects deferred. |
| record.F1975 | Signed Int32 maximum-like count, updated with Math.Max on success. RECONSTRUCTED maximum-combo alias. |
| record.F198a | UInt16 snapshot of current count, updated by conv.u2. Separate from the live Int32. |

**M40f1 is an eight-byte direct setter**, just arg0/arg1/store/return. No subscriber, event, virtual callback, drawing call or equality test inside it. The only direct F2674 writer is M40f1:002; full setter callers are inventoried. Unresolved reflective/native/external mutation is not included in that direct inventory.

### Successful interior raw8

At M0ad2:11bc read count via M40f0, unchecked add1 at11c2, preserve new local value, call **M40f1 at11c7** on the same stacked combo receiver. This is the first scoped current-combo mutation, after F153d100c. Then:

- Math.Max(previous record.F1975,new local count) at11ce, store F1975 at11d3.
- Reread current combo through handler.F05f2/M40f0 at11e4; conv.u2 at11e9; store F198a at11ea.
- Branch11ef→12c8 bypasses the failure feedback/reset work.

Under the stable-reference main control, oldcombo1 becomes2, max becomes Max(oldMax,2), snapshot2. These are original integer effects, not a complete score formula or an observed screen number.

### Failed interior raw−262144

At11f4–120f, read current count and decide optional feedback (next section). **Every normally returning branch rejoins at1277**, including count<=20 and F137b/F137c suppression. At1283 load0, local copy0, call **M40f1 at1287**; store conv.u2(0) to F198a at128f. No decrement and no preservation for small positive/zero combo: it is a zero assignment. F1975 is not updated on this failure route.

The prior failure-feedback call and optional presentation work can interrupt before the zero store if an original helper throws. Thus a recorded F0bf5 increment is not proof that the combo setter was reached. Once reached, the setter's first effect is the direct Int32 store; it does not require any result-event subscriber.

The handler/record refs and later getters are separate reads, not an atomic snapshot. The positive setter uses the stacked combo receiver; record maxima/snapshots use other field reads. Preserve those original reference identities and mutation order under instrumentation. Integer add is unchecked; cache conversion is UInt16 truncation. No overflow saturation is recovered.

## E18-D — Break-associated feedback and generic observation are separate

### Failure-specific feedback request, before combo zero

The failure route requests additional feedback only when **signed currentCombo>20**, F137b=false and F137c=false. Exact gates: ble1201→1277 includes equality20; brtrue1208/120f→1277 skip on the E11 Relax/Autopilot-derived flags. These gates do **not** skip the later combo zero assignment.

On the reached branch:

1. Decode original identifier **2064529101** through M49b0 at1216; literal key/asset identity remains UNRESOLVED. Call **M3b14(decodedString,false,true,7)** at121e to obtain an integer handle. This method is a resource boundary here, not a reimplemented loader.
2. Call **M3b31(handle,100,0f,false,1f)** at1230. If handle==−1, M3b31 returnsnull at00a/00b. Otherwise it constructs/configures a T02a9 wrapper, appends it to global F2351 under Monitor at09a, and queues **M3bb6** with the wrapper through global F234b.M2fee(...,2) at0bd. Stop at this original playback/action dispatch; no native playback or audibility assertion.
3. If global F2c48 is true at123b, original work on F2c4c follows (colour/scale/animation helper calls). Record it as an optional presentation boundary; do not reconstruct its effects or pixels. If false, proceed directly to1277. Both normally returning paths reset combo afterward.

Neither request carries the parent slider reference or a checkpoint index. The resource/playback wrapper is the queued receiver, not a parent-slider “broken” callback. This directly reached failure-specific feedback is distinct from the current-combo state write. Calling it a **break-associated feedback request** is RECONSTRUCTED from its producer/gates; its encrypted name, sound and displayed meaning are not confirmed.

### No dedicated break delegate established on the examined direct path

No dedicated break-event invocation appears in the inspected M0ad2 failure branch or M40f1 setter. There is a later **generic raw-result observer**: when Play.F13bb==2 and handler.F05fa is nonnull, M0ad2:13d8 invokes virtual **M19f5(rawEnum)**. It receives only the raw enum explicitly, not the parent/index/provenance tag. This happens after the combo mutation and intervening original consumers.

Original M0ab1 factories are conditional: T02e9 and special-derived T02ac inherit the empty base M19f5; alternative T09d0 overrides it with M4924. Metadata/ancestry closes these reached alternatives without guessing actual setting F03ea. M4924 first writes F0ed5=unchecked(freshF2358+4000) at00c, calls M19f8, then tests signed result>=0 at019. This is a generic observer-state boundary, not evidence of a dedicated slider-break notification. Further observer UI/colour work is deliberately out of scope. A null F05fa or nonmatching Play state suppresses this observer, **not the earlier combo setter**.

Common virtual M0ad0 at12da is also retained: the ordinary handler inherits it; it receives raw, auxiliaryDouble, local Boolean3 and parent. It can reach the original ComboSoundBursts milestone helper M0acf, which reads current combo, plus other original component helpers. It is not a newly recovered dedicated checkpoint-break event and is not replaced with a no-op. Subsequent full health/score/effects remain unresolved. A record-mod bit32 branch can call original M2979(0.0) after the failure reset; its further mechanism is an explicit out-of-scope boundary, not assigned a guessed policy.

**Answer to notification identity:** handler's T0892 combo component receives the zero setter. Optional feedback dispatch queues the T02a9 wrapper. Optional generic observer object F05fa can receive the enum. No dedicated parent-slider break recipient/event was established. This is a bounded negative finding, not proof that no later observer/setting/native path can produce feedback.

## E18-E — Tracking exit, provenance and later aggregate are independent

Exact reached order when leaving is required:

`M146f outcome counter/Boolean write → guarded M146e → return enum → M0ac5 → M114a/base M0ad2 → F153d → combo branch`

M146e is inside M146f **before** it returns; the combo consumer is afterward. Failure with followOK=true but entryTime>deadline skips M146e and can retain F0bf3=true while combo resets. Failure with followOK=false calls leave only when old tracking true and new processedCount<total. A no-due update can invoke leave and return0; base M0ad2's zero return skips combo entirely. The leave method itself contains no M0ad2 invocation or direct current-combo write in this inspected path. Its presentation helpers remain original dependencies.

| Result source | Scoped combo consequence / distinction |
|---|---|
| Initial head success64 with unevaluated parent | Its consumer Boolean3 stays true; positive path can increment current combo independently of head stored quality. |
| Initial head failure−262144 with parent | Same raw/parent combo reset route as checkpoint failure if consumer state matches. Earlier M2295 input effects and head mutations remain different. |
| Interior success8 / failure−262144 | Increment versus zero setter established here; no extra head/checkpoint provenance parameter in this combo branch. It does not read F0bf1/F0bf5/F0bf2/head raw to distinguish those two negative sources. |
| Terminal failure−524288 | Counterexample to “every negative resets”: its branch sets local Boolean3=false at0420, which can skip the combo block at119b. Full terminal handling deferred. |
| Positive parent aggregate256/512/1024 and ordinary decorations | These result branches derive local Boolean3 from the object's Slider2 type test; for the ordinary Slider2 parent/Osu0 control it is false, so they do not add another combo unit. Broader record-mode/optional branches are preserved, not generalized. |
| Parent aggregate−131072, including an applicable manager conversion | Its branch retains Boolean3=true and can independently take the reset route. E17 manager conversion/provenance and already-evaluated parent are distinct from an interior failure. |

Same negative numbers do not prove whole histories equivalent. Initial-head and interior failure with the parent share this scoped base branch under matched state, while timeout can pass the **embedded head** through the manager event; an evaluated object's F252e hook may differ. Parent aggregate arrives via M28d3 event/M229c, with conversion/decoration and completed state, not M0ac5. No global sliderBroken Boolean is introduced.

Managed ordering remains E12/E15–E17: clock/input/queued press; M2275 current from old candidates; refresh M28e2; optional ruleset M0ac5; later M28e3/parent completion; drawing afterward. Post-refresh gates rejoin before M28e3. A skipped current/readiness/nullable-time path emits no component return from an unreached M146f. A clock jump can process one entry before completion, not a synthetic all-due catch-up. Physical scheduling and displayed/audible latency remain unverified.

## Interface and conditional static reference cases

Inputs: original parent/head/current/handler/record/combo refs, schedule and original component index, held/strict-space/entry-time predicate, counters/outcomes and tracking flag, raw enum, parent flags, F05f6 prefix state, score-record mode/optional object, current/max/snapshot combo, F137b/F137c, resource handle, F2c48/F2c4c, Play state and optional generic observer. Outputs: component bookkeeping, optional leave, F13ea/F153d, current/max/UInt16 snapshot count, conditional feedback dispatch and generic observer. Output is not an observed sound, rendered combo number, total score or aggregate grade.

Primary fixture is still unvalidated E16 S2000/E3500/times[2500,3000,3464], first item j0/k1, head-positive main control has oldcombo1. Predicates/fields below are original stored operands with normal-return conditions, not an executable model. Restore fresh state for independent cases; high-count/inconsistent/post-return state probes are explicitly artificial relative to this one-slider fixture.

| ID | Conditional probe | Static prediction |
|---|---|---|
| B01 | First component success, entry<=2500, follow maintained | C1/F0/slottrue, raw8, combo1→2 after F153d10; max/snapshot updated. |
| B02 | First failure because held/spatial predicate fails | C0/F1/slotfalse, raw−262144; pending-count leave may run before raw return; combo1→0 after F153d0. |
| B03 | First valid tracking entry at2501, deadline2500 already due | follow true but entry too late; failure/raw−262144, tracking can staytrue while combo becomes0. |
| B04 | Entry exactly2500 and all original reads permit due check | Entry equality passes; success/raw8 and increment. Original host landing at that equality not observed. |
| B05 | Successful first checkpoint, then release with no new component due | First increments; subsequent leave can return0 and cause no additional combo reset through base M0ad2. |
| B06 | First failure, re-enter before3000 and remain following | Earlier failure not undone; later successful interior can increment combo0→1. No second head judgement required. |
| B07 | Failed head, then first interior succeeds | Head−262144 parent handoff can leave combo0; raw8 later increments it1. Separate head/component state. |
| B08 | Successful head then first interior fails | Head64 can establishcombo1; checkpoint−262144 resets0 despite positive stored head. |
| B09 | First and second interior failures | Each increments failure count/writes its slot and reaches zero setter; second can store0 over0, with no setter equality suppression. |
| B10 | Matched raw8 versus−262144 with same consumer refs/configuration | Same zero-prefix admission; first differing mask branch0203, selected0579/0426, later increment11c7 versus reset1287. |
| B11 | No due component, raw0 with valid stable parent | Base returns003 before F13ea/F153d/combo; M0ac5 can still observe tracking-state change afterward. |
| B12 | Previous combo0, failure | Setter stores0; feedback count gate skips; no positive-combo prerequisite for state write. |
| B13 | Artificial previous combo20 versus21, plain flagsfalse |20 skips extra feedback;21 reaches request path; both zero combo on normal continuation. Not naturally reached by this fresh one-slider fixture alone. |
| B14 | Artificial previous combo21 with F137b or F137c true at consumer | Feedback skipped; reset still reached. Not a full modifier-behavior claim. |
| B15 | Qualified feedback, resource lookup returns−1 normally | M3b31 returnsnull, but caller continues to optional presentation/rejoin and combo reset. |
| B16 | Qualified feedback, valid handle, dispatcher returns normally | Wrapper added to locked global list and action queued before zero store; no sound completion/audibility claim. |
| B17 | F2c48 false versustrue after feedback request | False jumps to reset; true adds original F2c4c presentation work before reset. Both conditional on helper return. |
| B18 | Null F05fa or Play.F13bb!=2 | Later generic observer skipped; earlier increment/reset unaffected by this gate. No invented break-event subscribers. |
| B19 | Optional observer T02e9/T02ac versusT09d0 | First two resolve empty M19f5; third M4924 receives raw and updates generic deadline state. Continuation not classified as a slider-break event. |
| B20 | Artificial parent evaluated=true only after M146f return | May enable M114a's F252e-dependent hook; base scoped combo branch still selected by raw/local Boolean. Such component production cannot naturally begin with evaluatedtrue because M146f returns0. |
| B21 | Artificial parent.F153b=false at the scoped consumer | It affects other bookkeeping but does not itself clear local Boolean3 on8/−262144 branches. Do not confuse it with the combo gate. |
| B22 | Valid initialized objects with F05f6 false, artificial consumer probe | Prefix skipped, subsequent combo branch still present. Uninitialized natural state is not assumed safe. |
| B23 | Original prefix or success-resource helper throws before combo block | No guaranteed current-combo mutation/handoff completion. Both raw outcomes require normal continuation. |
| B24 | Failure recorded, then M146e helper throws before returning | Counter/slot may already record failure; raw not delivered to M0ad2, so no guaranteed combo reset from this call. |
| B25 | Failure feedback helper throws before1277 | Component/F153d can already have changed; combo zero setter not yet reached. Distinct from successful queued playback. |
| B26 | Consumer interrupted after M40f1 zero store but before observer | Live count already0; no guarantee later snapshot/observer/score work completed. Mutation is not atomic. |
| B27 | First success then failure at3000 | First increments1→2; next failure resets2→0, preserving maximum-like count. |
| B28 | Time jump makes multiple entries due but j0 processed only | Exactly one returned component enum consumed in that call. If k==N a success can be128, so do not force the raw8 comparison onto every jump. |
| B29 | First interior failure before eventual parent completion | Raw−262144 reset is independent of later M1465 aggregate; E17 can still yield a positive whole-slider result. |
| B30 | Positive Slider2 aggregate under ordinary Osu0 branches | Type gate clears combo-enable local; no additional positive combo increment at119b. Not equivalent to positive8/64. |
| B31 | Aggregate−131072 or matching manager-converted failure reaches consumer | Can reset independently through its own result branch. No assumption every aggregate follows the checkpoint path. |
| B32 | Terminal−524288 consumer control | Original Boolean3=false skips this combo block; disproves universal negative-reset shorthand. Full terminal behavior deferred. |
| B33 | Readiness/current-object/nullable-time gate skips M0ac5 | No component/checkpoint-consumer call from that dispatch; later parent updates remain separate under E12 ordering. |
| B34 | Artificial current combo65535 then raw8 | Int32 becomes65536; UInt16 snapshot becomes0. Do not substitute the snapshot for live count. |
| B35 | Artificial Int32.MaxValue current then raw8 | Unchecked add wraps before setter; no saturation. Maximum/snapshot operations retain original ordering; target-runtime exceptional probes unexecuted. |
| B36 | Repeated current/record refs change between reads | Stacked increment receiver, later cache getter/record and feedback/reset refs need not form an atomic snapshot. Log actual identities. |
| B37 | Current combo reset but F2673 display state still positive | Setter alone does not draw or synchronize display state. M40f8's later comparison is a separate observer boundary. |
| B38 | Initial head failure and checkpoint failure both raw−262144,parent,matched consumer state | Same scoped combo reset/feedback gates; earlier head versus checkpoint mutations differ and no universal gameplay equivalence is claimed. |

All38 cases are branch-derived static predictions. None were executed on original Windows or a replacement combo/scoring model. Integer comparisons are signed where shown, increments unchecked, UInt16 conversion explicit; original floating/resource/helper operations are not numerically reimplemented.

## Confidence, minimum Windows validation and next investigation

CONFIRMED: original branches, direct combo accessor/field/constructor, success increment/reset/max/cache sites, independent feedback gate, optional observer dispatch/ancestry, leave-before-consumer ordering and relevant counterexamples. RECONSTRUCTED: current/max-combo and break-associated feedback aliases, player explanation and non-executable pseudocode. HYPOTHESIS: reported distorted-slider stutter cause remains untested. UNRESOLVED: original runtime/import/provider/configuration/interleavings, encrypted feedback asset identity, native playback/action success, full observer/visual/HP/score/reset behavior, displayed combo/audio and physical-frame latency. No optimization or pre-rendering work.

Minimal pinned-Windows experiment, **NOT RUN**:

1. Verify reference/map/audio/environment/instrumentation hashes above and actual ordinary parent/head/handler/combo types and identities. Confirm imported deadlines, current/max/snapshot/display states and ordinary record mode. Log actual settings/F05f6/observer/resource flags instead of assuming defaults.
2. Fresh successful versus failed first-interior runs: keep head result and previous combo1 matched, then change only required held/spatial condition for failure. Separately compare continued tracking versus first late entry at2501. Log each real key/cursor/time/Movement/radius/follow operand, entry/active state, index/counters/array write and returned enum. Do not force a raw result to masquerade as a natural checkpoint.
3. Trace M146f outcome/leave/return, M0ac5 and M114a/base arguments,0203/0260/0293, F153d100c, Boolean gate119b/sign11a2, getter/setter11bc/11c7 or1287, F1975/F198a, feedback1201/1208/120f/request1230 and optional observer13d8. Record real refs and QPC/thread/order; first setter store is distinct from any displayed count.
4. First-slider natural combo is small. Test count20/21, UInt16/Int32 boundaries, forced helper failures, evaluated-parent flip, missing observer and exact-equality clocks as **separate artificial probes**, or create separately hashed extended preparation maps for natural high combo. Keep those apart from the unchanged one-slider control.
5. Log feedback handle/scheduler wrapper/action and observer receiver, but stop before full audio/effects. A separate capture/audio measurement is needed to assert audible or displayed feedback. Log eventual parent aggregate as a different source, not another tick result.
6. Compare traced/untraced cadence and repeated runs; measure logging overhead, dropped samples and debugger-induced timing changes. Add no extra key polls, clock updates or gameplay/resource calls. Distinguish a failed checkpoint with no consumer return from a completed reset when helpers interrupt.

**Smallest subsequent gameplay investigation:** follow one ordinary failure with record-mod bit32 through the already identified M2979(0.0) boundary and Play.M226c's first fail/continue decision, contrasted with the same checkpoint without that bit. Establish one actual failure-state transition without a full HP/score/effects implementation. Windows validation of the current combo/feedback path remains outstanding. Work 2 remains suspended until explicit substantial product authorization.

# 3. Pseudocode

Independent non-executable RECONSTRUCTED description of CONFIRMED static instructions. Retain E15–E17 original gates, helpers, repeated refs and conversions. No substitute input, combo, break or scoring engine.

## Checkpoint result versus tracking exit

```text
RECONSTRUCTED — scoped first interior decision in original M146f
retain original held/space/entry/due-prefix checks
if one outstanding component is due:
    if followOK and entryTime <= timestamps[j]:
        successes++; outcomes[j] = true; raw = 8       // scoped k1/N3 control
    else:
        failures++; outcomes[j] = false; raw = -262144
else:
    raw = 0
if not followOK and tracking and processedCount < timestamps.Count:
    original M146e()                                  // before result return
return raw
// Other k/terminal result arms remain the original E16 operations.
```

## Ordinary consumer and first combo mutation

```text
RECONSTRUCTED — M0ac5 -> M114a -> base M0ad2, original parent ref rereads retained
retain M114a optional evaluated/F252e/nonzero hook
if raw == 0: return
Play.F13ea = parent
retain F153b read and original F05f6-dependent prefix
retain positive masks and exact raw8 / -262144 branch selection
retain original auxiliary-value, resource/presentation and mode operations
parent.F153d = raw == 8 ? 10 : 0                       // scoped ordinary control only

// Local Boolean3 remains true for these two branches.
if original comboEnableLocal:
    if raw > 0:
        newCount = unchecked(originalComboGetter() + 1)
        originalComboSetter(newCount)                // M40f1 -> F2674
        record.maxLike = Max(previousMaxLike,newCount)
        rereadRecord.snapshot = conv.u2(rereadComboGetter())
    else:
        if signed originalComboGetter() > 20 and not F137b and not F137c:
            handle = original M3b14(M49b0(2064529101),false,true,7)
            original M3b31(handle,100,0f,false,1f)     // may returnnull for -1
            if F2c48: retain original F2c4c presentation work
        originalComboSetter(0)                       // even previous combo0 or <=20
        record.snapshot = conv.u2(0)
        retain original record-mod-bit32 helper boundary

retain M0ad0 and remaining original consumers; no complete score/HP recovery
if Play.state == 2 and original F05fa observer is not null:
    observer.virtual M19f5(raw)                       // generic enum-only observer
    // original alternatives include empty base or T09d0.M4924
```

## Receiver and bounded negative finding

```text
CONFIRMED static direct setter:
    T0892.M40f1(value): F2674 = value; return
    // No delegate/event/draw/equality guard here.

RECONSTRUCTED feedback boundary:
    M3b31(handle,...):
        if handle == -1: return null
        retain original T02a9 construction/configuration
        append wrapper to locked original list
        queue original M3bb6 action through M2fee(...,2)
        return wrapper

UNRESOLVED:
    no dedicated parent-slider break event established on the examined direct path;
    exact encrypted feedback asset, native/audio completion, later observer/display
    consequences and actual original Windows behavior require matched instrumentation.
```
