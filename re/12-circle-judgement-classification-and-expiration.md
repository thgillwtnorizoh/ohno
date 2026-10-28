# 1. Result

Expedition 12 — Ordinary Circle Judgement Classification & Expiration. 2026-10-28 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover one ordinary osu!standard circle's remaining timing classification, evaluated state, update-driven late check and common manager-result handoff. Start at E11's M1884:034; retain the earlier selection/admission gates without treating them as judgement windows. No reconstructed gameplay/input/timing/scoring component, numerical surrogate, renderer or optimization was built. Full score/accuracy/HP/combo calculation, sounds/effects, sliders/spinners, replay serialization and general mod/input configuration are outside scope. Work 2 remains suspended; its code and `reconstructed/` were not inspected or changed. No authorised exception.

Read the complete repository contract and identical supplied attachment, cumulative contexts and relevant E01–E11 records/evidence, especially E05's ruleset correction, E07 update/candidate path, E08 object lifecycle hooks, E10 clock and E11 input route. Historical files remain unchanged; this expedition appends one journal entry. E09 projection remains deferred. Correct ordinary chain: M113e→M0651→M48de→M48df→M45eb/M45ec, exact circle T094c. Do not substitute E04's superseded Taiko mapping or the slider-head override.

Verified requested remote parent `9c38655aa75473b7de037432eb66f382fc43ec0f`, tree `fdce4fa0afdfdc5feebb32e0ffb3528eebed5630`; dedicated remote branch `work1/expedition-12-circle-judgement` created directly from it. Local mirror `eff87e9317dcdd879cb9e9b024770a5bceb7d2e9` has that identical tree but a different commit identity; remote publication explicitly uses the required parent. All 50 tracked baseline files were checked before append; preserve 49 other files byte-for-byte and the exact old journal prefix. Verified publication revision is supplied in the final handoff, avoiding a self-referential hash in this tree.

| Input | SHA-256 |
|---|---|
| osubackup.zip, 18 entries | 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 |
| osu!.exe | 6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d |
| Repository/supplied Instruction.md | bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9 |

Same self-declared Stable40/file-version1.3.3.8 reference. Official dated build/authenticity/modification status UNRESOLVED. Current host: Linux x86_64, no Wine/dotnet/DISPLAY or `/dev/dri`. Pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 reinstalled for static analysis. No original executable, CLR method, native input/audio call, Windows instrumentation, actual gameplay, capture or performance test was executed. All reference cases are conditional static predictions.

## Evidence, method and actual checks

Created this report, `tools/circle_judgement_evidence.py`, `tools/check_circle_judgement.py`, `evidence/12/circle-judgement.json`, and the E12 journal append. Read-only tooling reuses unchanged E04/E05 collectors. Evidence records 48 methods, RVAs/body hashes/signatures, original token/constant/arithmetic/conversion/branch sites, full ordinary ancestry/slot declarations and MethodImpl records, direct managed accesses to 19 selected fields, boundary callers, original enum definitions and the result-delegate signature. Original binaries/assets/source/full disassembly are not distributed. Private normalized instruction inspection provides stack/local-variable interpretation; extraction/assertions verify sites without running a replacement classifier.

Competing explanations: the first window is the complete accepted range versus successive strict windows; equality remains in the better window versus demotion; timeout writes a separate miss versus reusing the same evaluator; evaluated means successful versus merely attempted/classified; object raw result equals manager output versus conditional conversion/flagging; gates after refresh skip object updates versus rejoin before them. Follow actual original instructions, resolve exact ordinary virtual receivers and compare field writers/readers. Do not use modern osu! documentation, another engine or an executable surrogate as proof.

Mxxxx/Fxxxx/Txxxx denote EXE MethodDef `0x0600xxxx`, Field `0x0400xxxx`, TypeDef `0x0200xxxx`. Report offsets are **hexadecimal, code-relative, header-excluded**; JSON offsets are decimal. CONFIRMED means original static instructions/metadata. Player aliases and independent pseudocode are RECONSTRUCTED. Actual Windows timing, physical input latency, displayed judgements and exceptional/runtime interleavings are UNRESOLVED. Predictions require stated state and normally returning opaque helpers.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/circle_judgement_evidence.py /path/osubackup.zip > /tmp/judgement-a.json
python re/tools/circle_judgement_evidence.py /path/osubackup.zip > /tmp/judgement-b.json
cmp /tmp/judgement-a.json /tmp/judgement-b.json
cmp /tmp/judgement-a.json re/evidence/12/circle-judgement.json
python re/tools/check_circle_judgement.py /tmp/judgement-a.json
python -m compileall -q re/tools
```

Actual local checks: repeated extraction byte-identical/public JSON match;245270 bytes, SHA-256 `6923b66613b24b0a16e955203345055efeca0b05f28814a56dea11579e6f61da`. New checker PASS for 48 methods and 74 overlapping historical body-hash comparisons. Full EXE scan19659 decoded bodies/1739zero-RVA/0decode errors; selected branch targets land on decoded instructions. All ten unchanged E02–E11 checkers PASS; E11 now reports 84 shared historical comparisons because E12 adds evidence, rather than its original 61. Python compilation of all research tools PASS. Historical bytes, exact journal prefix, three top-level headings, scope/diff and remote parent/tree/branch/every-blob/mode checks are verified for publication. These validate evidence extraction and preservation, not gameplay equivalence.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M1884 | 9a828 | 154 | e1ec885ada0f20d49a2ecebf8cf979b8effee6d17d4f0053d4bc80ac22aa9338 |
| M28d3 | 108454 | 2651 | 931f0b853149359d24e26906f45fc824183285d9d1a6c1044cc6cebe41d3e8bd |
| M28e4 | 1097f8 | 215 | bb57be9a4658f4470bb0cf1e14ae44cf9572dff521e04da01a9fade2a23c34ac |
| M0653 | 1ffa0 | 26 | e25f425a37639827a990c07dc827e3f92dd3a723886d2acc4ba42d2dbd052cc5 |
| M0654 | 1ffbc | 8 | f02ca24d3318e33269378cc91007f373edbc47b67a2f2479c58c0951d06768c3 |
| M229c | de654 | 132 | 3cbb1dd730f45963dc2252105b6fede193eaf0585e37e055d79d90797af2aa13 |

# 2. What they found

## What this means to an osu! player

A circle has three success windows. A press inside the tightest window produces the raw result associated by the manager with300; outside that window, stable tries100, then50. **Each comparison is strict:** exactly on a threshold moves to the next result, and exactly on the widest threshold produces the negative, miss-like result. This classification uses absolute integer timing error, so early and late errors of the same magnitude classify alike once the evaluator is reached. Earlier object selection/admission can still prevent that call.

If you never press, the manager waits until gameplay time is **strictly later than the circle's end plus the widest width**, then calls the same circle evaluator used by input. An untouched ordinary circle initially has end=start. With unchanged time between the late check and evaluation, this necessarily produces the negative result. At exact equality, timeout does nothing, although a permitted press at that same time can evaluate the circle negatively.

Once evaluation begins, the circle is marked evaluated before the clock read and result calculation. This includes low-quality hits and negative results. Ordinary selection, manager entry and timeout check all reject an already evaluated circle, so it is unavailable for a second ordinary hit. Evaluated is not synonymous with successful, invisible, removed from memory or already scored. Reset/initialization can change that state separately.

The manager does more than forward the object's raw result: it can convert lower positive results under Perfect, and can add low-order flags at an object-group boundary. Its output and the circle's stored F152a must remain distinct. Both key-driven and late-update results enter M28d3 and can reach the same result delegate, registered to Play.M229c. No displayed pixels, sound, complete score, accuracy, HP or frame cost was established here.

## E12-A — Complete ordinary circle classification

CONFIRMED dispatch: exact T094c→T035a→T04c3→T0828 inherits T035a.M1884 for abstract slot M24a9; there is no ordinary-circle override replacing it. E11's input route M2295:11a→M28d3:012 reaches this implementation. M1884 returns the enum-like F152a, not a numeric300/100/50 score.

Let `S=F2523`, `T=F2358` and `e=AbsInt32(unchecked(T−S))`. M1884 writes F1527=true at002, samples T at007, loads S at00d, subtracts012 and calls Math.Abs(int32)013. E12 continues the established first comparison rather than reinterpreting its operands:

| Priority / offset | Signed branch | Result if branch not taken | Equality |
|---|---|---|---|
| F0ade at020; `bge.s`025→034 | e>=W300 | raw1024 stored02d | Try next window |
| F0add at03b; `bge.s`040→04f | e>=W100 | raw512 stored048 | Try next window |
| F0adc at056; `bge.s`05b→06a | e>=W50 | raw256 stored063 | Negative result |
| 06a–074 | All three failed | raw−131072 stored070 | Same negative path |

The comparisons use signed int32; no per-key rounding, `<=`, extra millisecond, local offset subtraction or playback-rate scaling occurs. Fields are loaded only when that comparison is reached; modified/stale/nonstandard widths must not be silently sorted. For finite ordinary ordered widths, the projection is e<W300→1024, W300<=e<W100→512, W100<=e<W50→256, otherwise−131072.

At075–082 the method reads F152a and, only if it is signed positive, invokes virtual M24bb. Exact ordinary circle inherits the base M24bb; its downstream sound-related helper is not reconstructed. At084–092 it rereads F152a, forms signed `(result>0)` and passes it to virtual M24b4. Exact T094c resolves to M45f7, which first calls base M24b4 then performs presentation-state work. E08 identifies this visual lifecycle boundary, but E12 does not reconstruct the animations. Neither reached body directly writes F1527 or F152a. At094–099 M1884 rereads and returns F152a. Keep these rereads and normally-returning helper requirements; no result snapshot through arbitrary callbacks is promised.

Consequences: the negative branch still leaves F1527=true, calls M24b4(false) and returns a nonzero negative result. A positive branch calls M24bb before M24b4(true). A helper exception can interrupt the handoff after flag/result writes; there is no local rollback/catch in M1884. Math.Abs(int.MinValue) and target-CLR exceptional/conversion behavior are not experimentally resolved; the pre-time-read evaluated write must not be moved later to create a convenient recovery policy.

## E12-B — Prepared widths and justified result names

CONFIRMED configuration: M28be reads map single F0b55→conv.r8 and invokes manager mapping slot M125a separately for each width. Ordinary T00e8→T0556→T0279 resolves that slot to M28b3, as E11 established. Original order is **widest, middle, tightest**, not the evaluation order:

| Field / alias | Original anchors a,b,c | Mapping call / conversion / store |
|---|---|---|
| F0adc / W50 (also selection's late width) | 200,150,100 | M28be:08c /091 `conv.i4` /092 |
| F0add / W100 | 140,100,60 | M28be:0c0 /0c5 `conv.i4` /0c6 |
| F0ade / W300 | 80,50,20 | M28be:0f4 /0f9 `conv.i4` /0fa |

The shared modified difficulty d is M28b0(raw,1.4,manager.F17ef): outside Edit, Easy bit2 applies Max(0,raw/2) first; HardRock bit16 applies Min(10,value*1.4) next. M28b3 maps d>5 to `b+(c−b)*(d−5)/5`, d<5 to `b−(b−a)*(5−d)/5`, otherwise b (including unordered d). Original operation order and float-to-double source conversion matter. Final conversion is truncation toward zero for finite in-range results, with no Math.Round. The ideal finite d0..10 algebra is W300=80−6d, W100=140−8d, W50=200−10d; this algebra is not a substitute for original conversions/JIT precision at numerical edges.

Conditional values: plain d0 gives80/140/200, d5 gives50/100/150, d10 gives20/60/100, ordered as tight/middle/wide. Base operand5 with Easy gives d2.5→65/120/175; with HardRock gives d7→38/84/130. These are static arithmetic predictions, not measured settings. F0b55's OD-like alias follows its use; its parser/UI literal was not decoded/executed. Input reads stored widths, not a newly calculated formula for each press. DT/HT do not multiply them locally; E10's clock/rate production remains separate. No wall-clock latency claim follows.

CONFIRMED result identity: F152a signature `06119220` refers to value type T0488. T0488 extends System.Enum, contains only int32 `value__` F1451, and has **no named literal constants**. The metadata therefore does not independently name1024 “300” or−131072 “Miss.” Do not invent enum member spellings.

Direct original consumer evidence in M28d3 gives the positive numeric association. After optional manager-local conversion, it masks a local result with **520095494 (0x1f000706)** and selects a numeric local:

| Masked manager result | Branch / numeric assignment | Interpretation |
|---|---|---|
| 256 | compare0c8→107; numeric50 at113 | 50-associated result |
| 512 | compare0d1→12e; numeric100 at13a | 100-associated result |
| 514 | compare0df→155; numeric100 at161 | Flagged100-associated result |
| 1024 | switch0e9 index0→179; numeric300 at185 | 300-associated result |
| 1028 /1026 | same switch index4→1a0 /index2→1c7; numeric300 at1ac /1d3 | Flagged300-associated results |

At358–35f that numeric local is converted using Int32.ToString and concatenated into a conditional resource lookup; decoded key strings and actual assets/pixels remain UNRESOLVED. Low bit1 is excluded by the mask, so257 also reduces to256. This original numeric conversion justifies RECONSTRUCTED player-facing300/100/50 aliases; **raw1024 is not a literal score value300**.

Any signed negative result takes a separate branch at09c to09e, using an unresolved decoded resource key; numeric local remains its initialized0. The evaluator's exhausted windows and stable-time timeout both produce exact−131072; the manager also explicitly counts that exact code with256 at213–244. We use **miss / negative failure** as a RECONSTRUCTED behavioral alias for this branch, not an observed screen label, recovered named enum member, generic rule that all negative enum values are equivalent, or a recovered full score/HP policy.

## E12-C — Manager entry, conversion and common handoff

CONFIRMED ordinary manager T00e8 inherits M28d3. Both input and timeout pass only the object reference. Exact prefix:

1. At001–009, if object.F1527 is already true, return0 immediately. No new F17ff write, classification or result event follows that branch.
2. Store F17ff=object at00c; virtual M24a9 at012 returns raw enum to local0 at017.
3. BinarySearch the full F17fb at01f. Store raw search result in local4. Set F17f8 to result if nonnegative, otherwise its bitwise complement, at033. Preserve the **original signed search result** separately; later boundary logic does not use the normalized index instead.
4. If local result is0, return it at03c after the index bookkeeping. Fresh ordinary M1884 always assigns one of four nonzero values on normal completion, so this zero-result continuation is not an ordinary classifier outcome.

At047–090, when F13c3 is nonnull and converted manager mods include **Perfect16384**, mask local result with0x1f000706. If unsigned `(masked−256)<=1` OR unsigned `(masked−512)<=2`, replace **manager local0** with−131072. Exact comparisons are `ble.un.s`07e→08b and `bgt.un.s`089→091. For fresh ordinary raw256/512 this converts the manager result; raw1024 does not convert. F13c3's full lifetime/policy is not reconstructed, so the extra nonnull gate is retained. This occurs after circle evaluation and its positive hooks, before manager outcome bookkeeping. It does **not** rewrite object.F152a or undo F1527. Thus stored512 with handed-off−131072 is possible on this reached Perfect path.

At213–249 manager bookkeeping increments:

- Exact local−131072 or256: F17f6++ at244.
- Exact local512: F17f7++ at234.
- Other values here do not take those increments.

These are manager group counters, not a recovered total score/combo/accuracy formula. M2258 initializes both to0 at0f8/104. Under ordinary M28bb (returns true), result-group decoration runs only when the **original** search result>=0 and it is the final registered index or the following object's M3e56 reports NewCombo bit4. It scans backward from selected index: for this circle, stop at an unevaluated object (pending=true) or an evaluated NewCombo object; otherwise continue down to0. The special T00d8 skip is irrelevant to exact T094c and is preserved as a variant boundary, not investigated.

At2dc–327, if local result>0, flag priority is:

- F17f7==0 AND F17f6==0 AND !pending → `result |=4` at302.
- Else F17f6==0 AND !pending → `result |=2` at320.
- Else → `result |=1` at326.

At328–335, reset F17f7 then F17f6 to0 even when the local result is negative, if this group-boundary block was reached. At a non-boundary, there is no flag decoration or counter reset here. The flags' complete displayed/score semantics are outside scope; do not name them from another osu! version. A first/only registered circle with initial counters0 gives manager outputs1028,514,257 or−131072 for raw1024,512,256 or−131072 respectively, assuming ordinary gates, no Perfect conversion and normal helper returns. Object.F152a remains the raw value.

The result is kept in manager local0 through an interposed resource/presentation section336–a2d. It includes conditional resource resolution, helper/visual calls and later M28d4 (ordinary inherited empty body); those implementations are outside this expedition. They are **not** replaced with no-ops or claimed cost-free. Exceptions/callbacks can interrupt progress. No further direct local0 assignment occurs in this section; no direct object F1527/F152a store or full/candidate-list removal occurs in M28d3. This does not prove arbitrary subscribers/helpers cannot mutate shared state.

The first common manager entry is M28d3 itself. Its explicit **classified-result event boundary** is:

- At a2f/a37, separately read F1804 for null check and invocation receiver; no atomic subscriber snapshot is proved by these two reads.
- At a40, invoke delegate T055d.M291a with `(managerLocalResult, auxiliaryString, originalObject)` if nonnull. String construction/meaning stays unresolved; do not substitute the numeric local for that argument.
- M2246:063–06e registered Play.M229c through M28c2. M28c2/M28c3 use Delegate.Combine/Remove plus an Interlocked generic compare-exchange loop; subscription and later reads are separate operations. Other dynamically attached subscribers are not ruled out.
- M229c stores the object to Play.F13ea at002 and at00f passes `(result,object)` to ruleset virtual M0ad2. **Stop here.** Complete score/accuracy/HP/count/sound/effect work inside that consumer is deliberately not recovered.
- After the event, M28d3 conditionally calls optional F17f9.M2f17(object) at a54, then returns the manager-local result at a5a. Its optional helper's semantics remain opaque.

Negative results therefore take the same event handoff when the intervening original helpers return normally; they are not discarded by a `result>0` guard around this event. A handled input bool, object raw enum, flagged manager enum and eventual score are four separate values.

## E12-D — Untouched-circle expiration and exact update ordering

CONFIRMED exact ordinary update chain:

`M226b:9cf → virtual M28e3 → T00e8.M0653 → base M28e3 → obj.M24b2 → virtual M28e4 → T00e8.M0654 → base M28e4`

M0653 calls base M28e3 first at001, then performs conditional M3b09/M3b53 work only after that call returns. M0654 simply forwards to base M28e4 at002. Ordinary circle's M24b2 resolves to the empty base body. The manager, not a per-circle update override or visual fade, owns this reached late check. This refines the earlier generic base-update description; special manager overrides must not be assumed identical.

M28e3 enumerates **candidate F17fd**, invoking object update at017 then manager M28e4 at01e for each. M28e4 first checks F2a90; if false, M4390()==true returns at0d6. Thus ordinary live M4390()==false can proceed even with F2a90=false; recorded-input mode retains the F2a90 dependency. Do not assign F2a90 an invented readiness/focus meaning.

M28e4 tests Slider bit2 first and Spinner bit8 next. Exact ordinary circle Normal bit1 has neither and reaches0aa:

| Offset | Ordinary-circle operation |
|---|---|
| 0ab /0b1 /0b6 | Load end F2524, load manager.F0adc (W50), unchecked int32 add |
| 0b7 /0bc | Read F2358; signed `bge.s`→0d6 skips when end+W50>=T |
| 0bf /0c4 | If object.F1527 true, skip |
| 0c6–0cc | Require application mode F2c6d==Play2 |
| 0ce–0d5 | virtual manager M28d3(object), discard its return |

For a fresh ordinary circle whose end equals start, stable-time, nonoverflow projection: **T>S+W50** and not evaluated. Equality does not expire. The check does not directly store a miss, set an extra expired bool, remove the circle or inspect visual opacity/lifetime. It invokes M28d3, which rechecks evaluated state and calls the same M1884. The evaluator samples F2358 again; it does not consume the late-check's T as an argument. If T remains later, e>W50 and raw−131072 follows. A rewind between the check and evaluation can instead produce a positive result; a concurrent reset/mutation/exception must not be ignored. Natural interleaving of those artificial cases is unverified.

Both M24c7 selection and M28d3 reject already evaluated circles; M28e4 also checks F1527. Thus an input-evaluated circle, including a negative one, is not normally expired again. M1884 itself has **no initial evaluated guard**; that guarantee belongs to these callers. Direct artificial invocation is a different experiment. Reset slot M24b3→M45f6 first calls base M24b3, whose02c writes F1527=false. Reset/seek/loading/editor policy is not reconstructed; do not claim the flag can never be cleared. Conversely loading helper M2241 can mark past objects evaluated at06e when end<=T without calling M1884 or writing F152a. A true flag is therefore not universal proof of this judgement path or a completed scoring event.

Expiration still requires candidate/update attention. An object remaining in F17fb but absent from F17fd is not tested by this loop. E07's search/library/order dependencies and possible time jumps remain relevant; this is not a recovered global catch-up scan over every missed object. Readiness helper M2269 can return before candidate refresh/update; live paused branch M226b:838–853 can also return before this reached chain. F13d8's drawing gate is not itself the timeout predicate. No guarantee of immediate expiration on every wall-clock millisecond, pause, seek or arbitrary jump is made.

### Correction to earlier update-gate interpretation (history preserved)

E07's integration text says, “Selection can refresh even when that later update dispatch is skipped.” Its W26 says, “Candidate refresh reached, later M226b update gates block M28e3”. E10 describes M28e3 as “separately gated”, and E11 repeats that interpretation. The original **branch destinations do not establish that skip** for the gates95e–9c3:

- 0963→0974,096a→0974;0972→09c9.
- 097f→09a8 or the nullable/time comparisons;09a6→09c9.
- The optional work09a8–09c4 calls ruleset M0ac5 and Play.M2273.
- **09c9 loads Play.F138f;09cf calls M28e3.** Both gate outcomes rejoin before this call.

Correction: those conditions decide whether the intervening ruleset work runs; on normal-returning paths already through refresh, they do **not** suppress the subsequent M28e3 call. Earlier returns, exceptions and called-helper effects still matter. E12 adds exact branch assertions; all old files/checkers remain unchanged. Their previous passing assertions did not validate that prose interpretation. No historical rewrite or exception procedure was needed because the correction is recorded here and in the new journal entry.

Relevant reached ordering, preserving distinct clock reads:

1. E10/E11 M43a0 publishes the clock, polls keys and queues shared parameterless presses.
2. Queue dispatch runs before the component callback. Selection searches the pre-existing candidate list; selection bounds, admission and M1884 each read F2358 separately.
3. M226b readiness/earlier state paths may return. On the reached update path, refresh M28e2 at959 precedes optional ruleset work and M28e3 at9cf, per correction above.
4. Ordinary M0653→M28e3 visits refreshed candidates, calls empty circle object update, then M0654→M28e4's late check. A circle evaluated by the earlier input normally fails its evaluated-state check here.
5. Drawing later evaluates visual state using separate clock reads. None of those scheduling/visibility predicates defines the three classification widths.

## E12-E — Conditional reference predictions and interface

RECONSTRUCTED static cases, **not** original-game or substitute-classifier tests. Control: exact T094c, first/only registered and candidate circle, start=end S2000, plain mods/difficulty operand5, stored widths W30050/W100100/W50150, A1200, valid cursor/radius and all E11 input/priority/Play gates passing, F1527=false, manager counters0, stable F2358 through each call, ordinary live Play2 and normally returning helpers. Fresh state is restored between cases. “Raw” means object.F152a; “handoff” means manager's event argument after decoration. Additional setup for a non-final circle is explicitly distinct.

| ID | Controlled condition | Branch-derived expectation |
|---|---|---|
| J01 | Perfect timing T2000 | raw1024, evaluated=true; manager numeric association300; first/only handoff1028 |
| J02 | e49, early1951 or late2049 | One unit inside first threshold: raw1024 |
| J03 | e50,1950/2050 | First equality fails, middle succeeds: raw512 |
| J04 | e51,1949/2051 | One outside first threshold: raw512 |
| J05 | e99,1901/2099 | One inside middle threshold: raw512 |
| J06 | e100,1900/2100 | Middle equality fails, wide succeeds: raw256 |
| J07 | e101,1899/2101 | One outside middle threshold: raw256 |
| J08 | e149,1851/2149 | One inside widest threshold: raw256; still positive, evaluated=true |
| J09 | e150,1850/2150 | Widest equality fails: raw−131072; selection and strict400 admission can pass |
| J10 | e151 early1849 | One outside wide threshold still passes ordinary selection/admission; raw−131072 |
| J11 | e151 late2151 | Input selection fails S+W50<T before classifier; a reached subsequent update can expire it |
| J12 | Early/late matched e49/50/99/100/149/150 | Same raw classification when each reaches M1884 with equal e; earlier route constraints remain asymmetric |
| J13 | Early T1600, e400 | Broad admission equality returns1, no M1884; evaluated remains false through this classified-input path |
| J14 | Early T1601, e399 | Admission can pass; all prepared windows fail, raw−131072 and evaluated=true |
| J15 | T799 versus800 | Below S−A selection rejects; at800 lower equality passes but admission rejects; neither reaches M1884 |
| J16 | Circle absent from existing F17fd | No input selection, even atT=S; registration/visibility alone insufficient |
| J17 | Already evaluated second press | Selection rejects; direct M28d3 call also returns0 before evaluator/event; timeout skips |
| J18 | Untouched update T2149 | Below late threshold: no timeout; evaluated remains false |
| J19 | Untouched update T2150 exactly end+W50 | Timeout equality skips; a permitted press at the same T instead yields negative evaluated result |
| J20 | Untouched update T2151 | Strictly beyond: M28d3→M1884, raw−131072 and common negative handoff |
| J21 | Raw256 from earlier press, later T2151 | Already evaluated; no second timeout classification/event through this update route |
| J22 | No press, object not in refreshed candidates | M28e3 never visits it; no timeout from this loop, regardless of stored end |
| J23 | Readiness helper returns true or reached paused early-return branch | No later candidate/update chain from that M226b invocation; do not infer score/expiration from visual fade |
| J24 | Either outcome of post-refresh gates95e–9c3, helpers return normally | Both rejoin at9c9 and call M28e3; only optional ruleset work differs (corrects E07 W26 interpretation) |
| J25 | M4390=true and F2a90=false | M28e4 returns before type/time late check; recorded-input policy not generalized to live |
| J26 | F2c6d!=Play2 at ordinary late branch | No M28d3 timeout call from0d0, even if late and unevaluated |
| J27 | Direct M1884 reached with fresh result512/256 | Positive hooks then M24b4(true); raw returned; low-quality still evaluated and unavailable |
| J28 | Direct M1884 exhausted windows | No M24bb positive hook; M24b4(false), evaluated=true, returns−131072; not zero/ignored |
| J29 | First/only circle raw512 or256, counters initially0 | Numeric100/50 association; manager handoff514/257, object raw remains512/256; boundary counters reset |
| J30 | Selected circle not at group boundary, otherwise same | No low-bit decoration/reset from that block; initial raw512 increments F17f7, raw256 increments F17f6 |
| J31 | Perfect16384 plus F13c3nonnull, raw512/256 | Manager local converts−131072; circle retains positive raw/evaluated state and already ran positive hooks |
| J32 | Perfect gate false, or raw1024 | No such conversion; raw1024 still can receive low flags at boundary |
| J33 | Admission T1951, circle evaluation rereadsT1950 | Admission can pass but later e50 classifies512; no event-time1951 snapshot substituted |
| J34 | Late check T2151, artificial rewind to2000 before M1884 read | Can produce raw1024 rather than timeout-negative, if all other fields/gates remain; not claimed naturally observed |
| J35 | Direct M28d3 given evaluated=true | Return0 before F17ff/index/evaluator/handoff, unlike negative nonzero result |
| J36 | Plain d0 or10 at configuration | Widths80/140/200 or20/60/100; equality at tight/middle/wide demotes in the same order |
| J37 | Base5 with Easy orHardRock | Widths65/120/175 or38/84/130; e65/Easy→512, e120→256, e175→negative; HR e38/84/130 analogous |
| J38 | Exactly representable d5.25 finite configuration | Width mapping yields48.5/98/147.5 then conv.i4→48/98/147; first/wide fractional part discarded |
| J39 | Explicit reset M45f6→baseM24b3 | Evaluated flag cleared before later reset work; re-hit depends on whole reset/candidate/input policy, not recovered here |
| J40 | Loading M2241 with object.end<=itsT | Can set evaluated without M1884/result write; flag alone does not prove a timed hit/miss event |
| J41 | Source/copy/host clock differs from F2358 | Classifier/late check use F2358, not F2359/F235b/native position; their separate reads can still differ |
| J42 | Interposed helper throws after flag/result write | No guaranteed common event/return; no rollback specified; actual exceptional behavior requires original CLR trace |

The accepted positive raw range for ordinary ordered width W50 and stable input time is `Abs(T−S)<W50`, **conditional on all earlier gates**. For the control, integer positive range1851..2149, with300-associated1951..2049,100-associated1901..1950 and2050..2099,50-associated1851..1900 and2100..2149. This does not assert physical key receipt times, displayed labels, score changes or unconditional reachability. Boundary T2150 is selected but negatively classified; T2151 cannot be selected by this input predicate and may instead be processed by timeout.

Interface for independent future implementation/research: inputs are actual candidate/full list identities/order/count/search result, exact circle/ruleset/manager types, S/end, current prepared widths, per-site F2358 reads, evaluated flag, ordinary input/readiness/application/recorded-input gates, Perfect/mod/context flags, group counters, NewCombo references and delegate/resource/helper state. Outputs are raw F152a, evaluated F1527, manager F17ff/F17f8/counters/local flags, positive/negative hook entry, optional common event `(result,string,object)` and returned enum. Timing/state/score/visual outputs are separate; unsupported helpers/variants must remain unresolved rather than fabricated no-ops.

## E12-F — Remaining dependencies and smallest Windows experiment

UNRESOLVED: actual event/poll/publication/classification timing and threading, natural jumps/seek/reset reachability, actual runtime difficulty/configuration/assets, target CLR conversion/exception behavior, opaque helper effects, enum spellings/negative screen label, complete result consumers and first pixels. E09 projection remains deferred. Owner-reported distorted-slider stutter cause, caching/precomputation benefits and measured cost remain unverified and outside this circle expedition. No optimization is implemented or inferred.

Minimal pinned-original Windows experiment (not run):

1. Reproduce E01's immutable ZIP/EXE/environment/settings baseline on a disposable offline Windows copy; record actual bindings, plain ordinary Osu, mods, offsets/rates, process architecture/CLR, input/display/limiter and skin/config hashes. Use unchanged E01 fixture first circle S2000, map SHA256 `dc4112acfcb0e7f345c6350a0cf1640d74f7acdc6eb2ae77047023b09e330541`, WAV `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977`. It remains unvalidated for import. Record actual runtime circle type, start/end/position, widths, candidates/full count and group flags; this three-circle fixture is not the first/only-circle decoration control. For that control, create a separate hashed one-circle map by removing the other two rows from a copy, documenting the exact change; no parser/classifier implementation is needed.
2. Resolve original CIL tokens/offsets to actual JIT locations. Instrument original key receipt/poll/enqueue/dispatch (E11), M1884 before flag/time/each branch/return, M28d3 raw return/conversion/decoration/event, M28e4 late check and M229c entry. CIL offsets are not native addresses. Log monotonic sequence/QPC/thread/object identity, all original time reads and widths, F1527/F152a, manager local result/counters/F17ff/F17f8 and event arguments. Do not add extra original clock calls, read audio positions on behalf of the game or replace its scheduler.
3. With fresh object state per run, press at perfect time and collect reached errors one inside/equal/one outside each of the three **actual stored** widths. Classify by original consumed T, not intended wall-clock injection delay. Exact-equality debugger probes are labelled artificial and separate from natural playback. Record whether selection/admission prevented classification. Press again after an evaluated low result to verify no second ordinary evaluator call.
4. Run without a press; collect candidate presence, late operand end+W50, equality/beyond boundary, evaluator result and event. Compare input-negative at the exact widest equality with untouched timeout just beyond it. A forced rewind between late check and evaluation is an optional separate artificial-state test; log every write instead of reporting it as naturally occurring. Optional Perfect control logs differing raw versus manager-local result without investigating the entire mod.
5. Preserve pre/post input/build hashes and trace overhead controls; retain raw logs privately and publish reviewed measurements. Screenshots can corroborate displayed labels only if actually captured; numeric resource choices alone are not pixels or full score validation. Silence cannot establish heard-audio synchronization. No reference outcome is claimed until this original experiment runs.

Smallest following gameplay investigation: resolve **ordinary cursor-to-playfield mapping and circle hit radius at M0ff1/F0ada→M24c7**. Timing/classification now connects to a real input route, while the spatial operand remains opaque; recovering that bounded path would make the future circle interaction contract more complete without expanding into full scoring or a renderer. Mission Control chooses the next assignment; Work 2 remains suspended until explicit product authorization.

# 3. Pseudocode

RECONSTRUCTED independent documentation grounded in CONFIRMED original instructions. Non-executable; no replacement classifier/timeout engine supplied. `i32` preserves unchecked arithmetic, `uint32` preserves unsigned branch interpretation. Keep per-site field reads and actual original helpers. Positive player aliases are justified by numeric conversion; “miss” remains a reconstructed negative-failure alias. Opaque presentation/consumer calls are not no-ops.

## Circle and prepared widths

```text
M1884, exact ordinary-circle evaluator:
    F1527 = true
    e = AbsInt32(i32(F2358 - F2523))
    if e < manager.F0ade: F152a = 1024
    else if e < manager.F0add: F152a = 512
    else if e < manager.F0adc: F152a = 256
    else: F152a = -131072
    if F152a > 0: virtual M24bb()              // exact circle -> base; opaque continuation
    virtual M24b4(F152a > 0)                   // exact circle -> M45f7; retain effects
    return F152a                              // reread, not a hoisted immutable result

M28be width configuration, after map-null guard:
    F0adc = conv_i4(virtual M125a(r8(map.F0b55),200,150,100))
    F0add = conv_i4(virtual M125a(r8(map.F0b55),140,100,60))
    F0ade = conv_i4(virtual M125a(r8(map.F0b55),80,50,20))
    ... other original configuration ...
ordinary M125a -> M28b3(D,a,b,c):
    d = M28b0(D,1.4,converted manager mods)
    if ordered(d>5): return b+(c-b)*(d-5)/5
    if ordered(d<5): return b-(b-a)*(5-d)/5
    return b
M28b0(D,factor,mods):
    if mode!=Edit and Easy2: D=Max(0,D/2)
    if mode!=Edit and HardRock16: D=Min(10,D*factor)
    return D
```

## Common manager entry and handoff projection

```text
M28d3(obj), ordinary inherited manager:
    if obj.F1527: return 0
    F17ff = obj
    r = obj.virtual M24a9()                    // exact circle -> M1884
    k = original F17fb.BinarySearch(obj)
    F17f8 = k if k>=0 else ~k
    if r==0: return r

    if F13c3!=null and Perfect16384 in converted F17ef:
        c = r & 0x1f000706
        if uint32(c-256)<=1 or uint32(c-512)<=2:
            r = -131072                       // manager local only

    numericLocal = original numeric conversion(r)
        // negative separate unresolved-key route; 256->50,512/514->100,
        // 1024/1026/1028->300 after original mask/branch sequence.
    preserve original auxiliary-string selection
    if r==-131072 or r==256: F17f6 = i32(F17f6+1)
    else if r==512: F17f7 = i32(F17f7+1)

    boundary = false
    if virtual M28bb() and k>=0:
        if k==i32(F17fc-1): boundary=true
        else if k<i32(F17fc-1) and F17fb[k+1].M3e56(): boundary=true
    if boundary:
        pending=false
        if obj is not special T00d8:
            i=k
            while i>-1:
                if !F17fb[i].F1527: pending=true; break
                if F17fb[i].M3e56(): break
                i=i32(i-1)
        if r>0:
            if F17f7==0 and F17f6==0 and !pending:
                preserve original auxiliary-string update
                r = r |4
            else if F17f6==0 and !pending:
                preserve original auxiliary-string update
                r = r |2
            else: r = r |1
        F17f7=0
        F17f6=0

    execute_original_resource_and_presentation_section_336_to_a2d()
        // Not reconstructed; normal return required. Retain its original
        // auxiliary-string handling. Do not assume no side effects/cost.
    if F1804!=null:
        reread F1804.Invoke(r,originalAuxiliaryString,obj)
        // registered Play.M229c: F13ea=obj; ruleset.M0ad2(r,obj); STOP investigation.
    if F17f9!=null: F17f9.M2f17(obj)             // opaque optional post-event helper
    return r
```

`numericLocal` is original numeric resource-selection state, not the event's auxiliary string, total score or object raw result. String/presentation implementation is intentionally unresolved; the projection specifies all direct local-result modifications through the common handoff without inventing replacements for that section.

## Reached ordinary update/timeout projection

```text
E11 press callbacks drain before component update:
    ... original polling/priority/selection/admission ...
    if admitted ordinary circle: manager.M28d3(circle)

reached ordinary M226b path, after earlier readiness/pause/other returns:
    manager.M28e2()                           // candidate refresh at959
    ... optional ruleset work; both guard paths rejoin before9cf ...
    manager.virtual M28e3()                   // T00e8.M0653 -> base M28e3
        for obj in F17fd:
            obj.virtual M24b2()               // exact circle: empty base
            manager.virtual M28e4(obj)        // M0654 forwards to base
        ... M0653's original trailing conditional work ...

base M28e4(obj), exact ordinary-circle branch only:
    if !F2a90 and M4390(): return
    ... original Slider/Spinner type branches; neither applies to T094c ...
    if i32(obj.F2524 + manager.F0adc) >= F2358: return
    if obj.F1527: return
    if F2c6d != Play2: return
    manager.virtual M28d3(obj)                // reads time again in evaluator
    discard returned enum

UNRESOLVED:
    natural original key/clock/timeout interleavings and reset/seek policies;
    opaque helper exceptions, subscribers, decoded resource labels and pixels;
    full downstream score/accuracy/HP/effects and nonordinary variants.
```
