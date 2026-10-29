# 1. Result

Expedition 19 — Failed Checkpoint, Record-Mod Bit 32 & Play Fail/Continue Decision. 2026-10-29 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover one ordinary interior slider checkpoint's raw8 versus raw−262144 consumer, the exact record-mod bit32 operation, its receiver/state effects and the first Play fail/continue boundary. Control: base Play T047b, ordinary T0253 ruleset/T00e8 manager, T02af Linear parent/T06f6 head, one traversal, non-Edit, record mode Osu0, stable references and normally returning original helpers. Full HP/score policy, failure screens/audio, replay lifecycle, all modifier behavior and replacement implementations are excluded.

Read the complete standing contract/identical supplied attachment, cumulative history and relevant E01–E18 records/evidence. Preserve E05's ordinary/Taiko correction, E12's post-refresh update rejoin and E14's resource-order clarification. Earlier files remain unchanged; only the current report/tools/evidence and append-only journal entry are added. No Work 2 or `reconstructed/` inspection/modification, substitute health/failure model or authorised exception.

Verified required remote parent **51ee90d85dce48ccd55c0dc0400bbd0478294a8b**, tree **7a9c4fe1eec67a218c043ae4ccb3d8aed2729e53**. Dedicated branch **work1/expedition-19-checkpoint-failure** created directly from it. Local mirror8fcfd2f has identical tree/different commit identity; publication uses the required remote parent. Fingerprinted79 historical files before changes; preserve other78 and exact journal prefix. Final handoff supplies verified publication commit/parent/tree/branch/all-file results.

| Pinned input | SHA-256 |
|---|---|
| osubackup.zip,18 entries |7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| osu!.exe |6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|
| Repository/attached Instruction.md |bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9|
| Unchanged E16 proposed one-slider map |f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84|
| Existing E01 independently generated silence WAV |c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977|

Same self-declared Stable40/file-version1.3.3.8; exact dated official identity/authenticity/modification status UNRESOLVED. Linux still lacks Wine/dotnet/PowerShell/DISPLAY/devdri. Pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 available. **No original Windows/CLR/native game, fixture import, input/audio/capture, instrumentation, performance, differential or synthetic HP/failure execution occurred.**

## Evidence and reproducibility

Created this report, `tools/play_failure_evidence.py`, `tools/check_play_failure.py`, `evidence/19/play-failure.json` and the journal append. Original binaries/assets/source/full disassembly remain private/unpublished. Tools only decode original metadata/CIL and assert instruction evidence; no gameplay simulator or forced state probe is implemented.

Competing explanations: bit32 directly ends gameplay versus sets a state subsequently tested; zero is a delta versus absolute target; value tested is presentation state versus separate double; every negative reaches setter versus result-local gate; setter queues a callback versus direct writes; zero always fails versus continuation/recovery branches; M226c only later per-frame versus direct result-consumer call. Discriminating procedure: original stack-flow/branch/field/caller inventories, metadata names/signatures, ordinary factory/virtual-slot resolution and independently written non-executable pseudocode. No external osu! formula or remembered mod name is used as proof.

Notation Mxxxx/Fxxxx/Txxxx = EXE0600xxxx/0400xxxx/0200xxxx. **CIL offsets hexadecimal, code-relative, headers excluded**; JSON offsets decimal. CONFIRMED means static instruction/metadata evidence; player aliases/pseudocode are RECONSTRUCTED. Runtime configuration/interleavings/reachability, target CLR exceptional behavior and visible outcomes remain UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/play_failure_evidence.py /path/osubackup.zip > /tmp/fail-a.json
python re/tools/play_failure_evidence.py /path/osubackup.zip > /tmp/fail-b.json
cmp /tmp/fail-a.json /tmp/fail-b.json
cmp /tmp/fail-a.json re/evidence/19/play-failure.json
python re/tools/check_play_failure.py /tmp/fail-a.json
python -m compileall -q re/tools
```

Evidence52methods includes the abstract M125a declaration, full selected-field/caller inventories, ordinary/alternative ancestry and virtual slots, original enum metadata, generic modifier-wrapper signature, body hashes, token/branch/constant/arithmetic/conversion/array sites. Full scan19659 decoded bodies/1739zero-RVA/0decodeerrors; selected branch destinations valid. JSON345231bytes, SHA256 **935ace51e5f5fc445f22086616cb1ee071e6773a8ecbe9bf9a024fb7bf66a1d5**. Focused checker PASS52methods/116historical body hashes. All seventeen unchanged E02–E18 checkers and Python compilation PASS. Final repeated-byte/public comparison, preservation/diff and remote publication checks are recorded in the journal. All cases below are manual static predictions, not original or substitute gameplay passes.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M2979 |111334|43|20e6dbba9ece32adb7d631e58a13317f5cefdb930397c61fb04974f51d4acf6d|
| M226c |dc1bc|148|f22e6388e72d93fdccc2b2945fd8a7a4ce7f38f3621633b952822b24a26f3af6|
| M0acb |3ae9c|284|88c7f13050dc0505bb3ddaa1f287a9d0215377e13cec92c141ba57be843c1a65|
| M0acc |3afd4|167|a4e4cbd7c8f1050a3fa5bdd984e8a781042b9d728b4058ef7f3f26c369a3e523|
| M297a |111360|79|21fd84d42ea64fc2b342307ce46089433af7a22fb7dea3ecd8f278232fbe3f43|
| M297b |1113bc|79|81b3e757e0c7f6c5be645a3ba5c6189a41074aeb5eac7a317e50dbc3a153ee4c|

# 2. What they found

## What this means to an osu! player

Missing one interior slider tick does **not necessarily end the play**. Its reached failure resets combo. Separately, it applies a negative adjustment to a health-like double. With original record flag32, named **SuddenDeath in this executable's own metadata**, it first sets that failure-tested value to zero. This is a direct state change, not itself a game-over callback.

The same result consumer then reaches Play's fail/continue test. A value of zero is included in the failing range, but original NoFail/Relax-derived/recorded-input and other gates can suppress the reached failure handler. Even when Play calls that handler, a positive recovery counter can take a continuation branch instead of latching the failed state. The normal fresh bit32 initialization does not grant the Easy-derived recovery count.

For the unchanged one-slider control, map F0b54=0 conditionally produces a−4 adjustment. Without bit32, a pre-adjustment value greater than4 remains positive; with bit32, zero is established first and remains zero after this subtraction, absent intervening state changes. Actual imported fields and live values have not been measured. No visible health-bar value, game-over screen, sound or wall-clock delay is proved.

## E19-A — Exact result branch and original modifier identity

Preserve E16/E18's naturally generated first-interior control: S2000/E3500, schedule[2500,3000,3464], j0/k1/N3/R1, unevaluated parent, positive head/main combo1, no interference. Success is original followOK plus entryTime<=2500; failure is !followOK or entryTime>2500. Counters/Boolean outcomes differ naturally, not by rewriting returned enums. Late-entry failure can still have active tracking. The handler receives the parent through M146f→M0ac5→ordinary M114a→base M0ad2.

Raw8 versus−262144 retain E18's original masks, optional evaluated/F252e hook, F05f6 prefix and success-resource work. Both must actually reach the common consumer; opaque helpers can throw or change state. LocalBoolean3 remains true for these scoped results. F153d10/0 at100c is separate from current combo. Signed raw<=0 at11a2 takes the reset arm; the result-local Boolean gate119b can skip it for other result families.

| M0ad2 offset | Confirmed operation |
|---|---|
|1287 then128f|Invoke combo M40f1(0), then store UInt16 record.F198a=0. Feedback threshold20 is already behind this point. |
|1295/129a/129f|Read handler.F05f3, its F1970 modifier wrapper, invoke original implicit conversion0a0002ca. This is not a read of the global record at this callsite. |
|12a4–12b0|Literal32; bitwise AND12ae; literal0 then signed cgt12b0, i.e. `(convertedBits & 32)>0`. |
|12b2|brfalse→12c8 skips only the M2979 call. Combo has already been reset if preceding operations returned normally. |
|12b5/12ba/12c3|Read handler.F05f5 receiver afresh; load **double0.0**; virtual M2979. No parent-slider type test, head-result check or object flag test here. |
|12c8 onward|Rejoin original consumer, including another state adjustment and later M226c. This is not an immediate return/game-over branch. |

**Original name proof:** F1970 signature `06151289b40111ad5c` is a generic T026d wrapper over value-type **T0b57**. MemberRef0a0002ca belongs to TypeSpec1b0000b1 `151289b40111ad5c`, its original name is op_Implicit, corresponding definition M1204. T0b57's original namespace/name is **osu_common.Mods**, with named constants NoFail1, Easy2, SuddenDeath32, Relax128, Relax2=8192, Perfect16384. Thus E18's deliberately unnamed bit32 can now be named from direct metadata. This is new evidence, not a historical rewrite. M1204 forwards into original generic getter M1202/its generic member calls; keep conversion and its exceptional/indirect behavior, not an invented plain field read.

Handler record and global Play.F13c3 are **separate reads**. Configuration M2258:006a establishes global record then at00e4 calls M0ace; M0ace:001/006 copies the current global reference into handler.F05f3. Restore M226e:003e/043 updates both. Under a stable ordinary configuration they refer to the same record; arbitrary concurrent replacement, reentrancy or artificial mismatch is not normalized. F137b/F137c are separately prepared latches: M0ab7 derives them from global record bits128/8192, not fresh copies made by M226c.

### Result provenance and reachability

| Original source | Can reach this bit32 setter site? |
|---|---|
| Interior success8 | No through its scoped branch: positive increment then11ef→12c8. It still reaches later adjustment/M226c on normal continuation. |
| Interior failure−262144 | Yes, if local Boolean3 permits reset and converted handler record includes32; current combo0/1/>20 does not change the bit test. |
| Initial head failure−262144 with parent | Same scoped base negative branch under matched consumer state; earlier head/input state differs. No source tag distinguishes these at12a4. |
| Ordinary circle or aggregate−131072 | Its result branch retains combo-enable true; can reach the same reset/bit32 operation after manager conversion/event/ordinary override. Different auxiliary penalty and evaluated state retained. |
| Terminal−524288 | Its branch clears localBoolean3, skipping119b–12c8, so this setter is not implied. Its later adjustment/M226c may still be reached. Full terminal policy deferred. |
| Positive ordinary Slider2 aggregate | Original type-dependent combo-enable false skips this block. It is not another interior8 result. |
| Raw0 | Base returns003, no bit32 setter or this call to M226c. A separate original caller can still test failure later. |

No claim every negative value is a defined result, reaches this path or resets combo. No claim bit32 is the only possible failure source.

## E19-B — Concrete receiver and absolute two-field mutation

Ordinary T0253 inherits base factory **M0ab4** unchanged. Initialization M0aaf calls this virtual factory at00d. M0ab4:007 constructs **T056a via M2963**, then stores handler.F05f5 at00c. F05f5 signature `061295a8` references T056a. Base Play's M227d and ordinary handler's M0acb also resolve without an ordinary override. Relevant MethodImpl inventories do not supply a different reached implementation.

T056a has two concrete derived types T0124/T0711 in metadata. Both inherit M2979 unchanged; T0124 overrides other delta slots. Their factories/semantics are alternatives, not the ordinary control: direct F05f5 assignments also occur in M1ee4/M4604. No generalization of their subsequent adjustments is made.

| Field/accessor | Confirmed type/direct writes | Scoped semantic role |
|---|---|---|
| F1857, getter M2966 | Double; sole direct managed writer M2967:002 | **Failure-tested target value**, health-like alias RECONSTRUCTED. This is what M226c reads. |
| F1858, getter M2968 | Double; sole direct writer M2969:002 | Separate adjustment accumulator; M2979 stores the original argument without its clamp. Full score/HP interpretation not recovered. |
| F1856, getter M2964 | Double; sole direct writer M2965:002 | Separate presentation/interpolated state. M2972 compares/approaches F1857 and uses F1856 in presentation work. Not the M226c operand. No actual bar pixels established. |

M2979 is virtual, signature `2001010d`: instance void(Double). Exact order:

1. M2979:001 loads0.0,00a loads200.0,013 loads argument.
2. 014 calls **System.Math.Min(Double,Double)**,019 **System.Math.Max(Double,Double)**.
3. 01e directly calls M2967 to store `Max(0.0,Min(200.0,arg))` into F1857.
4. 023–025 reload the **unclamped original argument** and call M2969 to store F1858.
5. Return02a. No branch, delegate, queued action, event, Play call or direct F1856 write.

M2967/M2969 are each8 code bytes: assignment/return. With finite argument0.0, both fields become0.0 synchronously before return. This is an **absolute target assignment**, not subtraction by zero. M2979 itself does not consult mods, parent identity, readiness, pause or clocks. NaN/exceptional Math/target-CLR behavior is not experimentally validated; retain original Math calls.

Reset M0ace conditionally calls M297d: primary F1857=200, accumulator F1858=200, then presentation F1856=0. This independently demonstrates these fields need not agree. Recorded restore M226e:05d passes record.F1989 to M2979; startup/seek M2241:094 passes stored object.F1529 on its reached branch. These are bounded distinct writers/callers, not a reconstructed reset/seek lifecycle. No extra callback is required for M226c to read F1857; updating visible presentation is separate.

## E19-C — The intervening adjustment must not be erased

M2979(0.0) does **not** call M226c. The verified connection is its enclosing consumer's remaining normal-return path:

`M0ad2:12c3 M2979 → optional M0ad0:12da → delta M297b:12f2 or M297a:1301 → original record/observer work → optional M22a8:13f9 → Play.M226c:1404`

The optional observer and earlier account/resource/presentation helpers are not side-effect-free. F05f6 controls prefix and late M22a8 bookkeeping, but it does not itself gate1404 once this scoped result reaches the end. A thrown helper can prevent the decision after combo/target state already changed. Different F05f5, Play.F13a4 or record references can be read at successive stages; stable identity is an explicit control, not an atomic snapshot.

Local double delta is prepared earlier, not returned by M2979:

- For−262144, M0ad2:0426–0457 invokes manager.virtual M125a(map.F0b54,−4.0,−15.0,−28.0).
- Ordinary T00e8→T0556→T0279 resolves abstract M125a to **M28b3**, the same piecewise difficulty mapping recovered earlier. M28b0 first applies original non-Edit Easy/HardRock transformations to the double source. M28b3 then uses ordered above/below5 comparisons and the original subtraction/multiplication/division order. No damage formula from another version is imported.
- For raw8, delta is F0604*3.0. F0604 is initialized1.0 in M0a9c but has other later writers; do not assume its live value/default. The control can conditionally set/read g=F0604=1 for a3.0 gain; this is not an observed original value.

At12e9, `ble.un` routes delta<=0 **or unordered** to M297a(−delta). Otherwise M297b(delta). Both are ordinary T056a implementations on the control:

| Helper | Original mutation order |
|---|---|
| M297a(x), subtraction | Preserve F1374/F1852 guard/flag write; F1858=Max(0,F1858−x) via002d; then F1857=Max(0,F1857−x) via0049. No upper clamp here. |
| M297b(x), addition | Same initial flag guard; F1858=F1858+x via001f, **unclamped**; then F1857=Max(0,Min(200,F1857+x)) via0049. |

For finite ordinary modified difficulty0..10, the−262144 delta lies between−4 and−28. For the E16 map's proposed HPDrainRate0, **conditional on imported map.F0b54=0 and ordinary manager state**, M28b3 returns−4. With stable refs and no intervening target mutation:

- bit32 absent: target becomes Max(0,oldTarget−4).
- bit32 present: M2979 first writes0/0; subtraction then leaves both0.
- This does not update F1856 directly. A target0 can be tested while a distinct presentation state remains positive.

Do not extrapolate to malformed difficulty/NaN: a positive delta could add after the absolute zero operation, or unordered values take the subtraction arm. The later decision consumes its actual fresh operand, not an earlier promised zero. M0ad0 can invoke ComboSoundBursts M0acf and, for positive delta, component/presentation hooks; retain these operations without expanding audio/effects/HP policy.

## E19-D — Exact first Play fail/continue decision

CONFIRMED **M226c** is connected, by direct call from M0ad2:1404. It is not merely a proposed per-frame consequence. Its body has no bit32 test and no checkpoint type/result parameter. It reads the current Play/handler state, so the same check is shared by other nonzero ordinary result sources that reach this consumer end.

| M226c offset | Gate/order and exact skip behavior |
|---|---|
|001–00b|If Play.F13a4.F05f5 is null, return093. Handler itself is assumed valid; missing handler can throw before this test. It rereads handler/component afterward. |
|01b–029|M2966→F1857 double; compare0.0. **bgt.un→093** skips for ordered value>0 **or NaN**. Finite zero and negative values continue. Negative is an artificial inconsistent state for the ordinary clamped control. |
|02b–043|Read global F13c3.F1970 through original conversion; `(bits & 1)>0` (named NoFail) returns093. This is a different record read from the setter's handler record. |
|045–051|F137b or F137c true returns093 (prepared Relax/Relax2-derived latches). No fresh32 check. |
|053–05f|M4390 reads recorded-input flag F2a7c. If false, continue061; if true, require F2a82=true or return093. Meaning/configuration of F2a82 is retained UNRESOLVED. |
|061–066|If F2c73 true, return093. Preserve this E10/E15 special playback flag; no invented pause/readiness alias. |
|068–08b|Read ruleset mode via M2232→F1371. If mode!=1, jump08d. Mode1 is original PlayModes.Taiko: require **fresh F2358 > first registered object's F2524**; equality skips. This time gate is not ordinary Osu0's gate. |
|08d–08e|Invoke Play.virtual **M227d**, which resolves base T047b for the control. It calls handler.virtual **M0acb(false)** at007 and discards its Boolean result. |

Therefore the finite target threshold is **<=0**, but reaching M227d is not yet proof that failed state was latched. No application-mode F2c6d, Play.F13bb, loading-ready or pause flag is directly tested by M226c. Those can prevent its caller from being reached; do not insert them into this method. Original wrapper/record nulls or helpers can interrupt instead of producing a normal branch result.

### Continue branch versus first failed-state latch

Ordinary T0253 inherits base **M0acb**. The false argument from M227d chooses only its false arm; its true-argument lifecycle path is not reconstructed.

1. At000–00d, if F05f8>0, call **M0acc(false):011** then returnfalse016–017. This check precedes the existing failed flag and candidate cleanup. No failed latch on this branch.
2. Otherwise if global **F136a already true**, branch01d→0e8 and returntrue. No duplicate candidate cleanup, queue request, F136b timestamp refresh or flag assignment here.
3. If not already latched, enumerate the handler manager's **candidate F17fd** and invoke each object's virtual **M24c4** at03c, with normal enumerator disposal. Slider's existing cleanup/resource slot is retained; no full-list loop or new judgement is inferred.
4. Call **M3b55:05a**. It schedules an original M3baa action through F234b.M2fee(...,2); queued request atM3b55:025, execution/order/completion unresolved. This precedes the failed-state write and can throw/interleave.
5. **First explicit Play failed-state latch: M0acb:060 stores F136a=true.** Then read fresh F2358 and store **F136b at06a**. No single atomic flag/time snapshot.
6. Later record/action work remains original. If mode!=3 it appends an original record with another fresh F2358+5, position operands and argument0; the two time reads can differ. Record Perfect16384 chooses M2263(false), otherwise it requests an opaque resource and stores F0602. These occur **after** the first latch and are stop boundaries, not reconstructed failure screens/restart/audio workflows.

F136a/F136b are static fields owned by Play T047b; their direct writers/readers are inventoried. F136a is a **RECONSTRUCTED failed-state latch** supported by this zero-gated failure path, repeated-call suppression, M2275's current-object-null branch and M226b's later latched-state branch. It is not parent.F1527, F13bb, the combo count or a mode request. On the examined path there is no direct OsuModes request or F13bb assignment; visible game-over/scene transition remains unresolved.

### What permits recovery?

F05f8 is a signedInt32 remaining-recovery counter, a RECONSTRUCTED alias from its producer/consumer:

- M0ab7:079–0b0 reads global record bits. It assigns2 only if bit32 is absent **and** Easy2 present. Fresh managed field default0 and ordinary factory/control are explicit assumptions. It does not set2 under32, even if2 is also present. This is initialization evidence, not a full invalid-mod-combination policy.
- Positive count diverts M0acb(false) into **M0acc(false)** before the latch.
- M0acc reads target; if target is ordered<160 (bge.un0014 skips equality/greater/unordered), it calls **M297b(160.0):025**. This is **addition**, not assignment to160. Starting at finite0, target becomes160; accumulator gains160; target's addition helper caps200.
- It sets F1852=true, F1854=0.07999999821186066 and invokes original presentation helpers on that branch. Preserve these side effects, no UI claim.
- With false argument it sets F1376=true, calls original machinery, then **M225c(false):093** and decrements F05f8 at0a1, unchecked. M225c has its own same-value early return and assigns F13d3 at00c before manager/presentation calls. It is not a new global fail latch, and is not equated to pause flag F1373.
- Normally returning recovery ends M0acb withfalse; M227d discards it. A counter>0/artificial already-failed combination still tests recovery first. Artificial mod changes after initialization can combine32 with an existing count; preserve reached state rather than reinitializing automatically.

Actual availability/consumption of recovery across full gameplay is beyond scope; no complete Easy lifecycle, restore policy or bar animation is inferred. T094e alternative writer M4608 is recorded, not applied to ordinary control.

## E19-E — Ordering, attention and temporary continuation

Reached ordinary order remains: clock publication/input/queued press → M2275 current selection from old candidates → M28e2 refresh → optional M0ac5/M146f → M28e3 object updates/parent completion → later drawing. E12's09c9 rejoin remains authoritative.

Inside the scoped tracking consumer: checkpoint bookkeeping → optional guarded tracking leave → return raw → M114a/base prefix → F153d → combo reset → optional absolute zero → delta adjustment → retained record/observer/M22a8 → **M226c → M227d → M0acb continuation or latch**. Separate field/time/reference reads are not flattened into a key/frame timestamp.

A combo reset can precede a skipped failure decision, recovered continuation or exception. A target change can precede Play's test and be altered before it. If a failure latches inside M0ac5, M226b's normal continuation has no new F136a test before **M28e3:09cf**: object updates/parent completion can still be reached in that same managed invocation. A later F136a test at0abd selects the existing M2272/M2262 branch and return0ad0. Next M2275 attention uses the failed flag to select null. This proves managed ordering, not physical-frame timing, frozen visuals or observed first game-over frame.

Direct M226c caller inventory has M0ad2:1404 and M226b:0a4f. The latter is gated by **PlayModes1**, not unconditional ordinary Osu0 per-frame polling. Other original methods call M227d directly and remain outside this selected source. Do not infer exhaustive failure detection from these two direct callers or promise when passive changes are noticed.

Loading/readiness M2269 and paused M226b early return can prevent this tracking dispatch altogether; if skipped, no component enum/consumer path is produced here. M226c itself has no such pause/readiness check. Restore/seek/startup calls can set target from separate stored values (above); complete interactions and reset/special Play variants remain unresolved. No displayed health/combo synchronization follows from a setter or failed latch alone.

## Interface and discriminating conditional cases

Inputs: original parent/head/handler/manager/component/record refs, actual checkpoint operands, raw result and local Boolean gate, F05f6/helpers, converted handler/global mod operands, current/max/snapshot combo, F1857/F1858/F1856, original auxiliary delta, F137b/F137c, recorded-input/F2a82/F2c73, ruleset mode, recovery count, existing F136a, candidate cleanup and scheduler. Outputs: checkpoint bookkeeping, combo mutations, target/accumulator writes, continued or recovered branch, optional failed latch/time and original action boundary. No displayed screen/audio result is an output established here.

Natural fixture import remains unverified. Restore independent fresh state for matched runs. Numerical state/flag/counter/NaN/callback probes below are **conditional/artificial when not naturally established by the fixture**, not fabricated measurements. “Failure gates pass” means M226c's original continuation suppressors all permit M227d and original helpers return normally. Parent remains unevaluated for first component; oldcombo1 main control.

| ID | Conditional case | Static prediction |
|---|---|---|
| F01 | First interior success8; same finite positive target; delta>0 | Combo increments; no bit32 absolute setter on this branch; addition then M226c sees actual fresh target. |
| F02 | First failure−262144, bit32 absent, imported F0b54=0, target>4 | Combo resets; no M2979; subtraction4 leaves target>0, M226c returns029. |
| F03 | Same first failure with handler/global32, stable target/refs | Combo resets, M2979 writes0/0, subtraction4 retains0; failure gates/count decide continuation versus latch. |
| F04 | Same failure, previous combo0 or1 | Both reach zero setter and identical bit32 test; count0 is not a veto. |
| F05 | Artificial previous combo21 instead of1 | Extra E18 feedback can precede reset; bit32/failure gates unchanged if feedback returns normally. |
| F06 | Late entry2501 for deadline2500, followOKtrue | Failure can be produced with trackingactive; bit32/latch path does not require trackingfalse. |
| F07 | Tracking exit with no newly due component/raw0 | Base returns003; neither bit32 operation nor this direct M226c call occurs. No immediate fail from a merely inferred tick miss. |
| F08 | Negative result with combo-enablefalse, e.g. terminal−524288 |119b skips reset/bit32 setter; later original delta/M226c can still inspect actual target. Not every negative means forced zero. |
| F09 | Initial head−262144 versus interior−262144 with matched consumer refs/state | Same scoped bit32/target/failure test; earlier head versus component mutations remain distinct. |
| F10 | Parent aggregate−131072 or circle miss reaches common consumer | Can reach reset/bit32/later M226c, with original different penalty and manager/evaluated state. Not a tick-specific failure method. |
| F11 | Parent positive Slider2 aggregate | Type-local gate can skip combo/bit32 block; later original adjustment/decision remains separate. |
| F12 | Artificial target h=4+epsilon, epsilon positive and representable; no32, delta−4 | Target epsilon>0; decision skips failure at029. Original Double arithmetic/precision retained. |
| F13 | Artificial target exactly4; no32, delta−4 | Target0; inclusive zero gate continues to remaining suppressors. |
| F14 | Artificial target below4 but nonnegative; no32, delta−4 | Target floored0; same zero gate. These are operand probes, not measured imported health. |
| F15 | Getter supplies smallest positive finite Double at M226c | bgt.un skips; no epsilon threshold other than0 recovered. |
| F16 | Getter supplies +0.0 or−0.0 | Both compare equal0; neither takes positive/unordered skip. |
| F17 | Artificial direct getter state<0 | Continues through remaining failure gates; ordinary helpers usually floor target, so distinguish inconsistent state probe. |
| F18 | Artificial getter NaN | bgt.un skips failure. No target-CLR NaN setter execution claimed. |
| F19 | Zero target but global NoFail1 present |043 returns before M227d; preceding combo/target writes not undone. Mixed32/1 combination is a separate validity-unverified probe. |
| F20 | Zero target, F137b orF137c true |04a/051 returns; original prepared Relax-derived state, not freshly read flag32. |
| F21 | Zero target, recorded-inputtrue/F2a82false |05f returns; same target under live flagfalse bypasses this extra guard. |
| F22 | Zero target and F2c73true |066 returns. No invented name/pause meaning assigned to this flag. |
| F23 | Control Osu0 versus alternative Taiko1 at first object's exact end | Osu0 does not read the special first-end clock gate; Taiko1 equality returns08b. Taiko mechanics not generalized. |
| F24 | Failure gates pass, recoverycount0, F136afalse | Candidate cleanup and queue request precede F136a=true:060; fresh time stored06a. First failed-state boundary. |
| F25 | Same zero target with positive recoverycount | M0acc(false), no newly failed latch; starting target0 gains160, flags/manager work and count decrement precede returnfalse. |
| F26 | Fresh Easy2 without32 at initialization | Producer assigns recoverycount2. Exact naturally consumed future count still requires runtime logging. |
| F27 | Fresh32, including artificial simultaneous Easy2 | Original producer skips assignment2; fresh default0. No automatic recovery merely from Easy bit when32present. |
| F28 | Artificial32 added after positive counter already established | Setter can zero target, then positive counter can recover; no implicit count clear at12a4/M2979. Not a natural valid-mod claim. |
| F29 | Already F136atrue, count0 | M0acb false arm returns true without repeated cleanup/latch-time store. Earlier consumer state can still change on an artificial repeated invocation. |
| F30 | Artificial alreadyfailedtrue and positive counter | Recovery test precedes failed-flag test; do not reorder/idempotently skip it. |
| F31 | Candidate cleanup or M3b55 scheduling throws | Combo/target can already differ, but F136a=true store not yet reached. No successful transition guaranteed. |
| F32 | Record/observer/M22a8 throws after target0 but before1404 | This direct Play decision not reached. A later valid call is a separate event. |
| F33 | Failure latch reached, later resource/record helper throws | F136a alreadytrue; time/event/action suffix may be partial. No rollback implied. |
| F34 | Scheduler executes queued action later or concurrently | Queue precedes latch; exact action completion/order is runtime-dependent. No deferred M2979 mutation is invented. |
| F35 | F05f5 null by consumer bit32 call | No null guard at12c3, so exception rather than a normal suppressed-call result. M226c's separate null-component guard is not retroactive. |
| F36 | Null component at direct M226c entry, valid handler |00b returns; this is a different entry state from naturally reaching M2979. |
| F37 | Handler/global record refs diverge between setter and decision | Bit32 and NoFail can be sampled from different records; report both identities. No immutable mod snapshot assumed. |
| F38 | Matched failure but original helper replaces component or changes target before decision | M226c uses its fresh handler/component/target; earlier0 does not prove consumed0. |
| F39 | Positive8 with original delta0 and preexisting target0, all failure gates pass | It can still reach failure test/latch despite success combo increment. This is an artificial preexisting-state control, not a failure caused by the positive result. |
| F40 | M2979 artificial finite argument−1 or201 | Primary clamp0 or200, secondary original−1 or201. Not a delta; no immediate presentation assignment/callback. |
| F41 | Target0 but distinct F1856 presentationstate positive | M226c reads target, not presentationstate. No visible health-bar assertion. |
| F42 | Latch set in M0ac5, outer normal callback continues | Can reach M28e3 before later0abd failed-state branch; no automatic checkpoint backlog drain. |
| F43 | Loading/notready/pause/nullable-current gate prevents M0ac5 | No scoped component consumer/bit32/Play check from this dispatch. Direct method probes lack the caller's gate. |
| F44 | Different F2358 reads during latch/record creation | F136b receives one sample; optional event timestamp uses another+5. No common synthetic event/frame timestamp. |
| F45 | Failure immediately before parent aggregate completion | First component consequences remain distinct; later M1465/manager aggregate can be positive or negative under E17, no undo of earlier latch established. |
| F46 | Record restore M226e or reset M0ace is actually reached | They explicitly set target through their original calls; not a universal auto-reset policy after every miss, pause or seek. |

All46 cases are branch-derived static predictions. Finite ranges and subtraction4 require the explicit map/helper/ref assumptions. Original unchecked count/time arithmetic, Double comparisons, Math call order, conv.u2 cache and separate field/time reads are preserved. No synthetic HP/failure engine ran.

## Confidence, original-Windows validation and remaining boundary

CONFIRMED: original bit-test/callsite, enum/typed wrapper, receiver/factory/slots, two-field immediate mutation, intervening delta operations, direct M226c call, all reached gates, M227d/M0acb false-arm dispatch, recovery diversion and first Play-owned failed latch. RECONSTRUCTED: health-like target/accumulator/presentation/recovery/failed-state aliases and player-facing explanation/independent pseudocode. HYPOTHESIS: distorted-slider stutter cause remains untested, no optimization inference. UNRESOLVED: actual fixture import/live mod configuration/provider/clock/interleavings/JIT/exception/callback behavior, full HP/score policy, deferred-action completion and visible/audible failure feedback. Semantic name SuddenDeath is metadata-confirmed, not proof of a particular installed user's setting.

Minimal pinned-Windows experiment, **NOT RUN**:

1. Verify EXE/archive hashes and actual Windows/CLR/architecture/display/audio/config/map/audio/instrumentation hashes. Import the unchanged E16 fixture, independently inspect imported map.F0b54 and naturally constructed [2500,3000,3464] schedule. Do not begin with debugger-inserted deadlines or result enums.
2. Record parent/head/current/handler/manager/F05f5 identities and concrete types; record handler and global record identities, both original wrapper conversion results, F137b/F137c, actual ruleset/application state, readiness/pause and recorded/special playback flags. Match successfulhead/previouscombo1, then create a genuine first-interior success versus held/spatial failure. Repeat late-entry failure as a distinct control.
3. Compare naturally configured no32 versus32 if the original UI permits those settings; otherwise label any forced wrapper/flag change artificial. Keep all other inputs fixed and log original counts/outcome/result/leave, combo/snapshot,12a4 branch, M2979 entry/exit target+accumulator+presentation, original delta and actual virtual receiver/operands, subsequent adjustment, M226c entry and every reached predicate operand.
4. Log M227d/M0acb(false), recovery count, candidate cleanup, M3b55 enqueue, F136a/F136b stores and the first actual Play state difference. Record optional suffix/observer actions without expanding screen/audio implementation. Distinguish queued work from completed action, stored flag from captured pixels and failure request from recovery.
5. Natural uninterrupted/release cases first. Threshold0/epsilon/negative/NaN, highcombo, inconsistentrefs, existing positive count with32, forced exceptions and direct callbacks are separate labelled debugger probes. Test recovery naturally only on a valid configured Easy control; preserve separately hashed derivative inputs if any.
6. Log thread IDs, original sequence/order and independent QPC without extra gameplay clock reads. Measure tracing versus untraced cadence, dropped logs and overhead; debugger stepping can alter due-count/update cadence. Keep actual clock samples distinct. Record parent aggregate later only as a separate result-source control. No original game-over screen, sound or displayed health claim without matched capture/audio observation.

**Exact established boundary:** `M0ad2:1404 → M226c` reads target F1857 and continuation gates; reached base `M227d → M0acb(false)` either diverts via recovery or, after original cleanup/queue, first stores Play.F136a=true at0060. No missing static connection between bit32 setter and this decision, but normal returns/stable refs are necessary conditions.

**Smallest remaining verification dependency:** one pinned-Windows natural failed-interior run with32 and matched no32 control, logging target before/after adjustment, all M226c gates, recovery count and actual0060 latch. This validates the reached path rather than a forced field-only test. A subsequent static expedition, if requested, can identify the first consumer of the latched flag that changes gameplay progression; failure-screen/audio workflows remain outside this bounded result. Work 2 remains suspended pending explicit substantial product authorization.

# 3. Pseudocode

Independent non-executable RECONSTRUCTED description of CONFIRMED static instructions, not a substitute HP/failure/state-machine implementation. Calls/refs denote original operations, with fresh reads where the original makes them; do not erase exception/reentrancy boundaries.

## Scoped checkpoint consumer continuation

```text
RECONSTRUCTED — original raw8 / -262144 ordinary branches only
retain original checkpoint bookkeeping, guarded leave, dispatch, prefix/masks/helpers
retain F153d store and original result-local combo gate
if raw == 8:
    retain original combo increment/max/snapshot
else if raw == -262144 and originalComboEnableLocal:
    retain conditional feedback and original combo/snapshot zero stores
    bits = originalConvert(handler.record.F1970)
    if (bits & 32) > 0:
        handler.component.virtual M2979(0.0)

retain optional original M0ad0(raw,delta,localGate,parent)
if ordered(delta > 0.0): originalComponent.M297b(delta)
else: originalComponent.M297a(-delta)             // includes unordered delta route
retain remaining original record/observer/optional M22a8 operations
originalPlay.M226c()                             // direct consumer call1404
```

## Absolute state operation and fail/continue test

```text
RECONSTRUCTED — ordinary T056a receiver
M2979(argumentDouble):
    M2967(Math.Max(0.0,Math.Min(200.0,argumentDouble)))  // F1857 first
    M2969(argumentDouble)                              // F1858 second, unclamped
    return                                            // no callback or Play call

M226c():
    if originalPlay.handler.component is null: return
    target = fresh originalPlay.handler.component.M2966()
    if ordered(target > 0.0) or unordered(target,0.0): return
    if (originalConvert(fresh globalRecord.F1970) & 1) > 0: return
    if F137b or F137c: return
    if originalRecordedInput() and not F2a82: return
    if F2c73: return
    if originalRulesetMode() == 1:
        if fresh F2358 <= originalManager.fullList[0].end: return
    originalPlay.virtual M227d()                  // ordinary -> M0acb(false)
```

## Continuation versus first failed-state store

```text
RECONSTRUCTED — base M0acb(false), ordinary handler
if recoveryCount > 0:
    original M0acc(false)                         // adds160 if target<160,
                                                 // flags/manager work, decrements count
    return false
if original Play.F136a: return true
for each original candidate:
    candidate.virtual M24c4()                     // original helpers can interrupt
original M3b55()                                  // queues original action before latch
Play.F136a = true                                 //0060, first explicit failed-state latch
Play.F136b = fresh F2358                          //006a, not atomic with flag
retain original post-latch record/Perfect/resource branches
return true

UNRESOLVED: natural Windows reachability/configuration/interleavings and all
screen/audio/native/deferred-action outcomes; direct state writes are not pixel proof.
```
