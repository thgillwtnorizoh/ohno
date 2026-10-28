# 1. Result

Expedition 15 — Ordinary Slider-Head Input & Entry into Held Tracking. 2026-10-28 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover one ordinary osu!standard parent slider T02af, its embedded head T06f6, the initial keyboard-head result route and the first live held-input/cursor evaluation. Control: one valid Linear slider, one traversal, ordinary base Play/manager, plain mods/non-Edit, no earlier overlapping object, normally returning original helpers. Stop after the first tracking predicate and its entry/guarded-leave state boundary. Complete tick/repeat/tail processing, aggregate slider judgement, score/combo/HP, effects, rendering, device configuration and optimization are not recovered here.

Work 2 and `reconstructed/` were not inspected or modified. No substitute input, tracking, geometry, renderer or gameplay implementation, even as a temporary model. No authorised exception. Independent pseudocode is non-executable research description, not a product component.

Read the complete standing contract and identical attachment, cumulative contexts and relevant E01–E14 records/evidence. Preserve E05's corrected ordinary factory path M113e→M0651→M48de→M48e0→M142d; E04's superseded Taiko interpretation is excluded. Preserve E12's correction: post-refresh M226b branches can skip intervening ruleset work, including this tracking dispatch, but rejoin before M28e3. E14's resource-adjustment ordering clarification is retained. No historical entry/report/checker changed.

Verified remote starting commit `c3cbee77afa1e14d0a067ccdcee790875696adce`, tree `f2143060707d56278617b28f158d0b8ddb652d8e`. Created dedicated remote `work1/expedition-15-slider-tracking` directly from it. Local mirror b9e98bb has the same tree and different commit identity; publication explicitly uses the requested remote parent. Fingerprinted all62 baseline files; preserve other61 and exact previous journal prefix. Final handoff supplies verified publication head/tree/branch/blob results.

| Pinned input | SHA-256 |
|---|---|
| osubackup.zip,18 entries | 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 |
| osu!.exe | 6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d |
| Repository/attached Instruction.md | bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9 |

Same self-declared Stable40/file-version1.3.3.8 reference; official dated identity/authenticity/modification status UNRESOLVED. Available Linux host lacks Wine/dotnet/DISPLAY and `/dev/dri`. Pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 reinstalled after absent-package check. **No original Windows executable, CLR method, keyboard/mouse/native API, gameplay, capture, differential or performance experiment ran.** No synthetic tracking/judgement engine was executed.

## Evidence, competing explanations and reproduction

Created this report, `tools/slider_entry_evidence.py`, `tools/check_slider_entry.py`, `evidence/15/slider-entry.json`, and append-only journal entry. Read-only collector reuses unchanged E04/E05 tools and records54 EXE methods,42 selected field inventories, signatures/body hashes, code-relative token/branch/constant/arithmetic/conversion sites, exact ordinary ancestry/slot candidates/MethodImpls, original Movement enum and held-mask TypeRef. No binaries, copyrighted assets, proprietary source or full disassembly are redistributed. Private original instruction/stack inspection supplies dataflow; the public collector/assertions pin independently reproducible sites.

Competing explanations: independently selected head versus selected parent/delegated head; circle result equals slider result versus distinct raw/return/handoff values; head success arms tracking versus a separate time/current-object/held/space path; candidate update owns all tracking versus Play's current-object reference; direct M145e position query versus prepared Movement records; inclusive distance versus strict test; fixed radius versus state-dependent radius; release callback immediately clears state versus sampled update plus guarded leave. Inspect only the original chain needed to discriminate these alternatives. No alternative osu! implementation or modern tracking formula is used as evidence.

Notation Mxxxx/Fxxxx/Txxxx = EXE MethodDef0600xxxx/Field0400xxxx/TypeDef0200xxxx. **Offsets are hexadecimal, code-relative, headers excluded**; JSON uses decimal offsets. CONFIRMED means original static instructions/metadata. Player terminology, semantic field aliases and independent pseudocode are RECONSTRUCTED. Runtime reachability, actual settings/provider/CLR behavior, interleavings and displayed pixels remain UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/slider_entry_evidence.py /path/osubackup.zip > /tmp/entry-a.json
python re/tools/slider_entry_evidence.py /path/osubackup.zip > /tmp/entry-b.json
cmp /tmp/entry-a.json /tmp/entry-b.json
cmp /tmp/entry-a.json re/evidence/15/slider-entry.json
python re/tools/check_slider_entry.py /tmp/entry-a.json
python -m compileall -q re/tools
```

Evidence JSON420576 bytes/SHA256 `87fd9994e58f350f00142796d861d4ffb5784fcdf33037ecd8ee96f1c5436b0b`. Full scan19659 decoded bodies/1739zero-RVA/0decode errors; selected branches checked against instruction boundaries. Actual deterministic extraction, focused checks, all thirteen prior E02–E14 checkers, compilation, history/diff and remote verification are recorded in the journal. Initial draft checker assumptions about M1464's new slot, a callback token, Movement enum spelling and a branch offset were corrected against original metadata/instructions before final PASS; no historical assertions were changed. Conditional cases below are manual static predictions, never original-runtime or surrogate tracking tests.

Actual checks: final extraction twice byte-identical/public match PASS; focused checker PASS54 methods/121 historical body hashes; all thirteen unchanged prior checkers PASS; compileall all research Python PASS. All62 baseline files unchanged before append; report has exactly three top-level sections and40 ordered cases. Current shared-hash counts E11/E12/E13/E14 are128/114/105/75 because the new evidence adds overlaps; their historical records are untouched. Publication and append-only preservation checks are verified separately before handoff.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M351d |152ef4|19|4342e98c65e1906924b6c71e6bb215c3e077471577ec5ba9c01c93cad25bb7ce |
| M146f |7e70c|899|a51c8ffbe5672e31e1919d8f0467cb0ce7b53ef44146bfb1cfd6eac8df427655 |
| M0ac5 |3ad60|166|1166e408b2fbaf918fb5d944be50abda75fe7efcca2e4520bf72f94b849c42b4 |
| M2275 |dc790|186|eaa8ce1b55c48608c741f9424b26c493341a62eb752bf747cb19f614845592a5 |
| M146d |7e3bc|531|78f7d1d40bed7690c0f529b1fa724849eacb260b177e5cff75ed959bc641568f |
| M146e |7e5ec|257|b0482158d157e71514d73a7b67c96cc88421c2719b76ae4518eee4d1e0e51d0e |

# 2. What they found

## What this means to an osu! player

Pressing Z or X on the slider head first selects the **parent slider**. Stable then evaluates its embedded head using the ordinary circle's three strict timing widths, with a slider-head-specific unsuccessful return code. The stored head quality and the result sent onward for the parent are different values. A positive head result does not immediately set the slider's live tracking flag.

Following begins through a separate check once the slider has started and Play gives it current-object attention. That check needs the original held-input policy **and** the cursor close enough to the required moving position. Holding a key alone does not suffice. For the plain route, a missed or untouched head is not itself a veto: a later hold on the path can enter tracking if the other conditions permit it.

The first distance test uses the prepared hit radius R. Once tracking is active, it uses **2.4000000953674316f × R**. Tracking requires computed distance² **strictly less** than radius²; equality fails, unlike the head-selection disk's inclusive comparison. Leaving and returning can therefore encounter different thresholds.

Releasing the key changes sampled input bookkeeping; it is not a direct slider-release callback. The next reached tracking evaluation can fail and invoke the leave helper, subject to the original pending-component condition. Missing updates, clock gates or interruption can delay that evaluation. This expedition does not establish complete ticks, repeats, slider breaks, tail judgement, accuracy or final slider scoring.

## E15-A — Parent versus embedded head: exact identities and dispatch

CONFIRMED corrected factory M48e0 constructs M142d/T02af at017. M28f0 passes that **parent reference** into manager M28d2 at004; F17fb/full and F17fd/candidate collections contain that registered object on this chain. M142d sets type Slider2, optionally OR NewCombo4; it does not set Normal1.

M142d calls virtual M143a at1b5 when presentation exists. M143a constructs T06f6 at054 using parent coordinates converted int32→Single per component, parent start and the original other arguments, then stores F0bdf at059. T06f6 extends T094c→T035a→T04c3→T0828. Its sole input evaluator override is M351d. **No M28d2 call registers the embedded head independently in this constructor chain.** Sharing the head's drawable references in M144a/E08 is presentation registration, not head hit-object registration.

| Operation / slot | Parent T02af | Head T06f6 |
|---|---|---|
| Spatial selection M24c7 | Inherits M24c7 | Also inherits it, but not independently enumerated on this route |
| Whole-object evaluator M24a9 | M1465, later aggregate boundary; deferred | M351d→direct M1884 circle classification |
| Object update M24b2 | M1449 | Empty inherited base |
| Live position/input evaluation M24c3 | M146f | Inherits circle M45fe; not used as parent tracking here |
| Head-evaluated query | M1463 reads head.F1527 | Separate head flag |
| Initial-head evaluation | New virtual slot M1464 | Calls head's M24a9 |

Exact ancestry, signatures, reused/new slots and MethodImpl metadata are published. M1464 is a **new virtual slot**, not a reuse of parent M24a9. Do not replace it with parent M1465, and do not apply the separate Taiko slider overrides.

### Selection and first press

Ordinary live M2295 reads stored pointer F2a5c, maps through M0ff1, calls M28c8/M28c9 with inputFlag=true and manager R=F0ada. Existing candidates are enumerated in order; the parent's inherited M24c7 checks start−A<=freshT, start+F0adc>=anotherT, parent.F1527=false, then ordered DistanceSquared(mappedPointer,parent.F252c)<=R². The input flag bypasses presentation eligibility and excludes alternate-center fallback, as E11/E13 established. There is no new spatial check inside M1884; the head's own F252c is not the center read by this selected-parent predicate.

Ruleset admission M0ad9 still follows. The **Normal1-only predecessor branch is skipped** for this Slider2 parent, while its general earlier-unevaluated-object ordering scan remains. Plain broad admission is strict AbsInt32(unchecked(parent.start−freshF2358))<400. It is separate from the head's prepared timing widths. Our single-object control avoids earlier-object arbitration; no general overlapping-slider policy is reconstructed.

At M2295:111 the Normal branch fails; Slider check at185 succeeds. Call parent.M1463 at199: if head.F1527 is already true, skip further head evaluation. Otherwise call M1464 at1a2. This means parent selection can still succeed after the head was evaluated, without granting another head judgement. Candidate membership, parent completion, head evaluation, held tracking and visibility are distinct.

## E15-B — Initial head result and result-handoff identities

M1464 first records input side in parent **F0bf0** only when left-new F2a6e or right-new F2a8a is set. Left wins when both are true:1048576 versus2097152. Its fallback current-mask load appears in the original conditional sequence; it is retained rather than substituted with a key identity. Then at03b invoke **head.virtual M24a9**, resolving M351d.

M351d:001 calls M1884 directly. E12's original arithmetic and mutations therefore apply to the embedded head:

- Set **head.F1527=true before** the fresh gameplay-time read.
- e=AbsInt32(unchecked(F2358−head.F2523)).
- Strict e<manager.F0ade → stored raw1024; else e<F0add →512; else e<F0adc →256; otherwise stored raw−131072.
- Retain sound/presentation hooks and reread/return of the stored result. These hooks are not a tracking-flag write.

The manager and start are the head's original constructor values. No new width multiplier or extra spatial test is added by M351d. E12's preparation/Easy/HardRock width evidence remains applicable to this inherited evaluator; no full mod survey is made.

M351d then returns that result if **strictly positive**, otherwise returns **−262144** (branch009→00d). It does **not** rewrite head.F152a. Thus stored head−131072 and returned−262144 can coexist. T0488 has only its int32 value__ field, not named original outcome constants. Do not call return−262144 a numeric score or silently equate it with E12's ordinary-circle raw failure code.

M2295 uses only whether M1464 returned >0:

| Reached initial-head branch | Immediate continuation |
|---|---|
| Positive | Store parent in Play.F1380 at1ac; call ruleset M0ad2(**64,parent**) at1ba; set Play.F138c=true; retain original feedback helpers |
| Zero/negative | Call ruleset M0ad2(**−262144,parent**) at210 |
| Head already evaluated | Skip M1464 and those branch-specific head-result calls |

This is **not** M28d3(head) or M28d3(parent) on the live initial-head path. A stored head1024/512/256 each leads to the same raw64 handoff here, while the head retains its quality. The ordinary circle's manager-local decoration/Perfect conversion in M28d3 must not be inserted into this route by analogy. Stop at M0ad2: downstream score/HP/effects are opaque and not assumed side-effect-free.

These reached methods do not mark parent.F1527 or assign parent.F152a, do not call M146d, and do not directly increment parent F0bf1/F0bf5. On a fresh, uninterrupted control those remain separate from head.F1527/F152a. Parent-wide M24a9 resolves M1465 at a later completion boundary; its aggregate algorithm is deliberately deferred. Raw head quality, initial raw64 handoff, tracking bool and future parent result are not interchangeable.

### Missing head and manager timeout boundary

M28e4's Slider2 branch, in application Play2, tests **unchecked(parent.start+F0adc)<freshF2358**, then !M1463() (head not evaluated). Equality skips. When reached it calls **M28d3(head)** at057, not the parent. That evaluator is again M351d→M1884 and samples time afresh; stable late time gives stored head−131072/returned−262144. Manager-local processing and the E12 common event/subscriber then apply to **the head identity**; do not substitute the initial raw64 parent route or equate raw field and manager-local value. Full decorations/score are outside this expedition.

Already evaluated heads bypass this timeout. The same manager method contains later parent completion logic, which is not recovered here. Missing-head timeout does not itself write the tracking flag. On the reached cycle below, tracking can run **before** the manager timeout check. A rewind between different original reads can change classification; no synthetic key timestamp is introduced.

## E15-C — Current-object attention and exact update ordering

First held tracking is **not owned by M1449**. Play M226b performs, after its readiness/pause/earlier gates:

1. **094e M2275** chooses a current object using the **existing F17fd** list.
2. **0959 M28e2** refreshes/replaces candidates.
3. **09be ruleset M0ac5**, only when the intervening guards permit it.
4. **09cf manager M28e3**, after those branches rejoin at09c9, whether or not step3 ran on the established normal-returning path.

This both preserves and sharpens E12's correction: the skippable ruleset work actually contains this tracking evaluation. It does not imply a skipped M28e3 call.

M2275 uses list.Find(M22c5), stores result in **Play.F1380 at03a**; F136a=true substitutes null. M22c5 requires start!=end; virtual start<=freshF2358; then virtual end>=anotherF2358 **or** object.F1527=false. For exact parent T02af, these getters inherit F2523/F2524. No head-evaluated flag, held state or spatial predicate is in this Find. It returns the first passing reference, not every active slider. After end, an unevaluated parent can still satisfy the predicate; no universal end-exclusive cutoff is inferred.

F1380's only direct managed writers are M2275:03a and positive-head M2295:1ac. Thus an earlier head press can store the parent, but M2275 can replace/clear it before tracking. If old candidates lack the parent while the subsequent refresh newly admits it, this current-object pass does not retroactively use the new list. Actual attention can be delayed until another reached pass. The live tracking routine itself reads F1380 rather than enumerating F17fd; it is a separate reference still dependent on candidate-derived selection, not a permanent independent slider registry.

Ordinary handler T0253 inherits **M0ac5**. If F1380 is nonnull, it snapshots the current object's slider tracking flag, then separately rereads current object and passes **stored raw/provider pointer F2a5c** to virtual M24c3 at04c. Exact parent resolves M146f. It directly passes returned enum and a reread current-object reference to ruleset M0ad2 at05c, even when return is0. After that consumer it reads current tracking again and ORs state-change inequality into Play.F138c. These are separate reference/flag reads, not an immutable parent snapshot.

Post-refresh gates: recorded-input/F2a90/F13f2 combinations retain their original branches. The nullable F13d9 condition allows tracking when absent or storedValue<=currentF2358; a rewind below the stored value skips it. Equality is allowed, so this is **not** a proven once-per-distinct-time scheduler. When reached, currentT is stored in F13d9 before M0ac5. Earlier loading helper M2269 or live pause can return before this sequence. F13d8's draw-preparation gate is not the tracking predicate.

Afterward ordinary M0653→base M28e3 enumerates refreshed candidates, calls parent M1449 through M24b2, then M0654→base M28e4. M1449 includes dirty/Edit preparation and presentation-related work; it is not the source of the held-distance predicate. M28e4 may then evaluate an untouched head. Queued keyboard press consumption occurred earlier in M43a0, before the component callback; same-host-callback input→tracking is possible under the gates, but never guaranteed. Separate sampled T, cursor, layout and object references must remain separate.

## E15-D — Held input is sampled bookkeeping, not another press event

CONFIRMED inputs used by M146f:

| Field | Original type / role |
|---|---|
| F0a7a | Boolean held latch set by new aggregate press, cleared after both aggregate sides become0 |
| F0a80 | Current logical-side bitmask, TypeRef0100035d System.Windows.Forms.MouseButtons |
| F0a7b /F0a7c | Previous/older mask values at an aggregate-state change, not necessarily previous two frames |
| F2a6e /F2a8a | New left/right flags used to choose a side |
| parent.F0bf0 | Remembered side/mask, same enum type |
| F137b | E11 live Relax-derived branch; outside plain control |

Keyboard and physical mouse states converge into logical F2a67/F2a83 through E11's M43a6/M43a7. Their MouseButtons enum storage does **not** imply the keyboard is ignored. In M43a0, if aggregates changed (3b1–3c7), shift F0a7b→F0a7c at3ce and F0a80→F0a7b at3d8, clear current mask at3de, OR1048576 for left-held and2097152 for right-held at3f6/418. If unchanged, this history shift is skipped. New aggregate edges set F0a7a=true at54a; both logical sides0 with old latch true clear it at6d0. These precede queued press drain773–78d and component callback790. Mouse latch reconciliation/repeat occurs later (E11); no physical-release latency guarantee is made.

For exact original boolean/bitwise ordering let A=F0a7a, C=F0a80, P=F0a7b, P2=F0a7c, B=parent.F0bf0, L=1048576,Rbutton=2097152,Both=3145728. These letters are aliases for separately read fields, **not a proved atomic snapshot**.

M146f:01c–0b8 computes:

- auxiliary `h = A && (P != Both || P2 != C)`, original short-circuit order.
- local held=false initially.
- If A:
  - If B==0 **or** (C!=Both **and** h), write B=leftNew?L:(rightNew?Rbutton:C), with left priority, and set local held=true.
  - Otherwise set local held=((C & B)>0), signed comparison.
- Else write B=0.
- Finally local held = local held OR h OR F137b, in original order.

This policy permits continuing without a new press edge; the new flags select a remembered side only on reached branches. It must not be replaced with “the same Z key is still down” or just “any button held.” The history/Both logic is real, though its design purpose remains UNRESOLVED. No anti-cheese/device interpretation is invented. Under a simple steady single-side hold, it supplies held=true; with both logical sides released, plain F137b=false yields false on the next reached check.

Release event6 is distinct from press event7. The recovered ordinary slider state changes through M0ac5/M146f/M146e, not an immediately invoked slider release handler. Sampling may be stale when earlier input gates/poll failure interrupt processing. Input-field publication is not atomic with tracking.

## E15-E — Required position, time selection and strict follow distance

M146f first returns0 if **parent.F1527=true** or if freshF2358<parent.start (000–016). Equality at start proceeds. Neither embedded-head flag nor head result is read anywhere in this method. An early return does not automatically clear F0bf0/F0bf3.

Only if the held policy succeeds does it attempt the required-position/distance work. Radius is loaded from manager.F0ada; when parent.F0bf3 is true, multiply it by **2.4000000953674316f** at0e6–0eb (LE bits9a991940). It is a Single-declared local with original CIL/JIT precision semantics. Radius preparation is the E13 mapping, not a new resource-based follow-circle radius, not the separate M1460 helper's4f branch, and not an inferred visible sprite border.

The shared preparation is M28be: stored sizeD=F17f1, divideD by2f at17e, divide by layout s at189, multiply original k=1.0004099607467651f at18f, store R=F0ada at190. D itself comes from E13's original circle-size/mod mapping; no per-tracking-update size formula or new visual-resource measurement is added. The inactive and active tracking radii therefore use the same stored playfield-coordinate R as initial selection, with the explicit active multiplier.

### Loading data actually consumed by this check

M144a produces prepared segments F0bdc and cumulative lengths F0bde as E05/E06 established. Its loading pass also clears ball-like **F0bd2.F2ce8 at0b9a** when presentation exists. For each traversal/segment it takes directed segment endpoints, derives Single distance from cumulative differences, computes double dt=conv.r8(1000f*distance)/V, and constructs **M2996** at0e95 with:

- from/to endpoints (forward for the first traversal);
- start=conv.i4(current absolute accumulator);
- end=conv.i4(accumulator+dt);
- easing0.

M2996 stores kind **Movement=1** (actual enum spelling), vectors F1867/F186b, int32 start/end F186e/F186f and easing. The **same transform reference** is appended to F0bd6.F2ce8 and F0bd2.F2ce8 (0ea9/0ebb) before the double accumulator advances at0ef9. Thus first tracking can consume geometry-derived records already built during loading; it is not evidence a sprite has been evaluated/drawn or pixels appeared.

**This reached tracker does not call M145e/M145f.** Those remain other time/distance queries from E06. Substituting a generic path-at-T query would discard the original interval selection, integer endpoint truncation and provider correction below. Later position/reset/geometry mutation helpers and other transform writers remain dependencies; the control uses unmodified loading records.

### Position selection and arithmetic

M146f stores adjusted query time q into closure.F0bfe at115–116:

`q = unchecked(freshF2358 − delta)`

Delta is the result of M3df6(F03e7) **only when selected provider F2a93 is T0170 and M43a8()==false**; otherwise0. M43a8 combines recorded-input state and F137c; exact small helper is in evidence. The setting's user-facing name/default is UNRESOLVED. This is a target-position correction, not a new global F2358 write or an offset passed into head classification. Provider class, helper and clock are read separately.

Find the **first** F0bd2.F2ce8 record satisfying M1478: `record.start <= q && record.end >= q`, both inclusive. M1478 does not test transformation kind. The fresh loading control uses its Movement records; arbitrary injected/mutated lists must not be silently filtered into a different list.

If no record and q>lastRecord.end, use the last record. For T0170 with M43a8()==false, if still none, q<parent.start and anotherF2358>=parent.start, use the first record. That is a narrow original provider fallback. No general “before start use head” fallback or lazy geometry rebuild is inserted. Empty/null ball/transform collections can fail through the original accesses; there is no explicit safe fallback in this reached method. Preserve that blocker rather than manufacturing one.

If a record was obtained:

- If end==start, required position=to vector F186b.
- Otherwise compute **from + (to−from) * (1f − conv.r4(unchecked(end−q)) / conv.r4(unchecked(end−start)))**.
- Preserve vector helper stores and division/subtraction order. This method does not call the generic easing evaluator or apply a new clamp to that factor. Last-record extrapolation can therefore extend beyond its endpoint if q is beyond its end.
- Map supplied pointer through layout **M0ff1 at23b**, then **DistanceSquared(mappedPointer,requiredPosition) at242**.
- Compute radius*radius at24b and use **ordered `clt` at24c**. Strict comparison, equality/NaN fail. No square root or visual-alpha test occurs here.

The last-record fallback is **not an endpoint clamp**: the same interpolation still uses q unless its interval has zero duration. Before/after target time and zero-duration behavior must not be imported from M145e. Exact target-JIT intermediate precision, nonfinite/out-of-range arithmetic and exceptional accesses were not executed.

## E15-F — First entry, guarded leave and deferred component boundary

Let followOK be the boolean from the first held/space check, initially false. Before processing scheduled slider components:

- If followOK and !F0bf3, M146f:25d calls **M146d**.
- M146d first stores a **fresh F2358** into int32 F0bf2 at006. It then performs original presentation/list helper work and only at20d stores **F0bf3=true**. No head-success test or successful-head counter is inserted. Helpers/exceptions/reentrancy are not assumed harmless or atomic.
- F0bf3 is the stored tracking-state gate that widens the next reached check. F0bf2 is an entry-time field, not the earlier target q or event receipt time. Direct managed writers: F0bf2 only M146d; F0bf3 only M146d:true and M146e:false.

After entry, M146f scans **F0bf8** until the first timestamp>separately readF2358. It compares due count k with unchecked(F0bf1+F0bf5). If an outstanding component is due, original processing can mutate counters/flags and returned raw result. **That block is an explicit deferred boundary**, not removed from the algorithm. Complete ticks/repeats/tail and aggregate scoring are not derived here. F0bf1/F0bf5 are observed success/failure-like component bookkeeping aliases, not a recovered score or HP formula.

One directly relevant dependency at that boundary is the comparison **entryTime F0bf2 <= selected component timestamp** (2b3–2c6; signed bgt skips that branch when entryTime is greater, while equality passes). It follows followOK and uses the separately sampled entry timestamp, not q. This site is retained as an input to the deferred component path; it does not by itself recover all component outcomes.

For the first-check control with no component due, F0bf1=F0bf5=0 and k=0, the branch at29b goes directly to355 and result remains0. Entering tracking can therefore set F0bf3=true while returning raw0 to M0ac5/M0ad2. This return is not the tracking bool.

At355–37c, **after** the component boundary, if !followOK and F0bf3 and unchecked(F0bf1+F0bf5)<F0bf8.Count, call **M146e**. This leave helper requires oldF0bf3, performs original presentation/list work using future component times and fresh clock reads, then stores F0bf3=false at0fb. It does not change the entry timestamp. If all components are already accounted for, this particular leave call is skipped; do not claim release always immediately writes false regardless of completion state.

The 60/180/100-like times inside presentation helpers are not evidence of a held-input grace interval. The established spatial hysteresis is the F0bf3-dependent radius. A failed check calls the guarded leave path in this reached update; any later persistence/slider-break policy requires component/consumer investigation. M146c's opaque clock/audio helper and M1449 visual work are not substitute tracking-state algorithms.

## Interface and conditional static reference predictions

Interface inputs: exact parent/head/manager identities; F17fd and Play.F1380; S/E and separate F2358 reads; head/parent F1527/F152a; R and layout; held latch/mask history/new flags/remembered mask; F0bf3/entry time; provider/setting; Movement records and component timestamp/count state; readiness/pause/current-object/nullable gates and normally returning original helpers. Outputs: head raw/evaluated state and returned code; initial parent raw64/−262144 handoff; F0bf0; target position/distance predicate; F0bf2/F0bf3; direct ruleset handoff and Play.F138c. Deferred component result/counters are retained as opaque original operations, not replaced by zero when due.

**Conditional stored-state fixture, not a played map or an executable model:** one fresh T02af with T06f6 head, parent/head integer center(256,192), S2000,E3000, traversal1, one prepared straight Movement from(256,192) to(356,192), interval[2000,3000], easing0, valid presentation; all component deadlines later than each first-entry probe, F0bf1/F0bf5=0, pending Count>0. Manager widths Wfirst50/Wmiddle100/Wwide150, A1200, R=E13's controlled d5 value32.013118743896484; layout s1,O=(64,56). Actual original postprocessing must be logged to establish these values from any map row. No claim that a natural installation has these settings/default resources. Restore fresh state for independent head cases.

| ID | Controlled probe | Conditional static prediction |
|---|---|---|
| T01 | Spatial center press atT2000, gates passing | Select parent; head raw1024/evaluatedtrue, return1024; raw64 sent with parent. Parent not evaluated by these input methods. Tracking flag still awaits separate check. |
| T02 | Head error49, either sign, selected/admitted | Stored/returned1024; same initial raw64 parent handoff. |
| T03 | Head error50 exactly, either sign | First strict width fails; stored/returned512; raw64 handoff. |
| T04 | Head error51 or99 | Stored/returned512, conditional on earlier gates. |
| T05 | Head error100 exactly | Middle equality advances; stored/returned256; initial raw64 handoff. |
| T06 | Head error101 or149 | Stored/returned256, still positive. Not a recovered full slider judgement. |
| T07 | Head error150 atT1850 or2150 | Selection/admission can pass; head stores−131072/evaluatedtrue, wrapper returns−262144; parent handoff−262144. |
| T08 | EarlyT1849 (error151), selected/admitted | Same unsuccessful stored/returned distinction. |
| T09 | LateT2151 | Parent selection upper bound already fails; no live head evaluation from this press. Reached later timeout can evaluate head through M28d3(head). |
| T10 | EarlyT1600, spatially inside | Broad admission error400 equality rejects before head classification; head flag stays unchanged by this route. |
| T11 | Press exactly on computed head spatial boundary | M24c7 ordered<= accepts, subject to timing/admission; later tracking uses strict<. Physical equality need not be representable. |
| T12 | Press spatially outside R but good timing | No selected parent from this candidate; no head evaluation. A new aggregate press can still establish held bookkeeping independently. |
| T13 | Second eligible press after head.F1527=true | Parent can pass its selection but M1463 skips another head evaluator call. Parent flag is independent. |
| T14 | Artificially offset head.F252c only | Parent spatial selection still uses parent.F252c; inherited head evaluator only tests time. Not independent head selection. |
| T15 | Positive head press beforeS, then first check still beforeS | Current-object selection normally requires start<=T; direct M146f also returns0 beforeS. Head success alone does not arm F0bf3. |
| T16 | Head success, hold remains, first current-object check atS with cursor atrequired position | Held/strict distance pass; enter timestamp sampled, flag becomestrue if original helpers finish; no-due result0 forwarded with parent. |
| T17 | Head success then both sides released before first tracking evaluation | Held false; no entry. Head remains evaluated/positive while parent tracking remainsfalse. |
| T18 | No press and no held state atS | Parent can be current but held false, no tracking entry. |
| T19 | No head press; later hold atT2200 with mapped cursor(276,192) | Live head selection is too late, but parent current-object path can acquire/follow. Tracking may enter before manager timeout; later timeout classifies head separately. |
| T20 | AtT2500 required Movement midpoint | Required position(306,192), provider-local cursor(370,248) maps there; simple hold passes first spatial check. No pixels asserted. |
| T21 | Same hold, cursor100 units from required position, inactive | Distance beyond R; no entry. Holding alone is insufficient. |
| T22 | Computed tracking distance² equals current radius² exactly | Ordered clt false; contrast T11. This is an operand condition, not a screen-pixel claim. |
| T23 | Cursor40 units from required position, initially inactive | R≈32; reject. |
| T24 | Same distance40, already active with pending components | Radius becomes R*2.4000000953674316f; accept and retain flag. |
| T25 | Active, depart100 units with pending components, then return40 | Departure fails and guarded leave clears flag; return40 then fails smaller entry radius. |
| T26 | After T25, return torequired position with hold | Re-entry can set a fresh F0bf2 and restore flag. No new head judgement required. |
| T27 | Active, both aggregate sides released, pending/no-due component | Next reached check clears remembered mask, fails follow, calls leave; raw method result can remain0 despite flag change. |
| T28 | Same release but tracking dispatch skipped/readiness interrupted | No flag mutation from an unreached M146f/M146e; no immediate wall-clock release guarantee. |
| T29 | Same logical held fields produced by keyboard versus mouse | Same reached tracking policy; field representation alone retains no key/button event timestamp. Provider sampling can still differ. |
| T30 | Both-mask/history combinations | Evaluate published h/B policy in exact order; do not replace with same-head-key or any-held shortcut. No design-purpose claim. |
| T31 | All components accounted for, follow fails | Final pending-count guard skips this leave call; complete finalization/cleanup remains deferred. |
| T32 | Original component deadline already due at first check | Entry predicate still occurs first; downstream component boundary must execute. Do not assert returned0 or unchanged counters. |
| T33 | Parent absent from old candidates, admitted by immediately following refresh | M2275 has already chosen null/other current object; no retroactive selection for that pass. |
| T34 | F13d9 stores2500 and currentT rewinds2499 | Optional M0ac5/first tracking skipped by its time guard; corrected M28e3 call still reached if other helpers return normally. |
| T35 | F13d9=2500 and currentT=2500 | Equality permits tracking again; no once-per-distinct-time rule proved. |
| T36 | Clock changes between head error, target q and entry timestamp | Each original read can differ; classification, required position and F0bf2 must not be assigned one synthetic event time. |
| T37 | Provider T0170, !M43a8 and nonzero setting delta | q usesT−delta for Movement selection/interpolation; start gate and marker/entry reads are separate. Conditional first-record fallback retained. |
| T38 | q exceeds last nonzero-duration Movement end | Last-record fallback still interpolates with q; can extrapolate beyond endpoint. Not M145e's out-of-range policy. |
| T39 | Selected Movement start=end | Use to vector directly, no interval division. Inclusive selection can reach equality. |
| T40 | NaN squared distance/radius, or held-path ball/transform data missing | Ordered clt does not accept NaN; missing data follows original unguarded accesses, not a fabricated lazy path fallback. |

All cases are static predictions under stated inputs and normally returning original operations. Integer arithmetic is unchecked int32 unless the original library call establishes otherwise. Original Single fields/locals, conv.r4, reciprocal, vector stores and comparison order are preserved; no forced rounding after every opcode or Python-double timing/path substitute. Actual x86 CLR/JIT, exceptional conversions/Abs overflow, target provider and original-library behavior remain unexecuted.

## Unresolved boundaries, smallest Windows experiment and next investigation

CONFIRMED static mechanisms: parent/head routing, inherited head classification with changed return, candidate-derived current-object selection, optional tracking ordering, held masks, Movement query, strict state-dependent radius, entry/guarded leave fields and timeout identity. RECONSTRUCTED: player aliases such as ball/follow/tracking, component bookkeeping roles and independent pseudocode. HYPOTHESIS about owner-reported distorted-slider stutter remains untested. UNRESOLVED: actual provider/settings/clock interleavings, Windows/native sampling, resource/helper failures, full downstream component/aggregate result semantics, slider breaks, final judgement, pixels and cost.

Minimal pinned-Windows original experiment, **NOT RUN**:

1. Pin untouched archive/EXE plus actual map/config/skin/display/DPI/backend hashes and selected ordinary types. Make a separately hashed one-slider derivative of E01's independent control: one integer straight segment, one traversal, normal head, no overlaps; preserve unchanged E01 map/WAV hashes separately. Verify actual imported geometry, widths, R, S/E, Movement intervals and future component deadlines rather than assume requested length gives exact duration.
2. Instrument token/JIT sites for key polling/aggregate publication, enqueue/dispatch, M2295 parent selection/M1464, M351d/M1884 and direct M0ad2 handoff; log parent/head identities, each F2358 read, mapped pointer, raw/evaluated fields and F0bf0. No extra poll/clock/resource calls introduced by logging.
3. At M2275/M0ac5/M146f, log candidate/current identities before and after refresh, optional time gate, held latch/current/history/new flags, provider/delta, selected Movement reference/interval/endpoints, actual target q/required position, original M0ff1 output, R or2.4R and squared operands/result. Log F0bf2/F0bf3 before/after enter/leave, due/pending count boundary, return enum and consumer identity with thread/order/QPC.
4. Fresh run for centered head press+hold, lower-quality/boundary head press, no-head late hold, release before first check, active release and outside-follow-area controls. Use a first-check interval with no component due to isolate this expedition; then separately record when one becomes due without claiming its scoring recovered. Track physical key transition and actual original polling/dispatch/check timestamps separately.
5. Repeat untraced/traced runs to measure debugger/log overhead; log masks on actual changes, not assumed frame history. Artificial forced clocks, exact-distance operands, missing lists, mismatched centers or provider states must be clearly labelled and separated from naturally reached gameplay. Captures are separate evidence; neither successful tracking nor a Movement record proves visual correctness.

**Smallest next gameplay investigation:** follow one first scheduled slider component from this now-established followOK/F0bf2/counter boundary into its raw result and immediate consumer. Keep one traversal and stop before repeat/tail/aggregate expansion; do not reconstruct the full slider scoring system. Work 2 remains suspended until explicit product authorization. No precomputation/caching/performance inference or implementation is made.

# 3. Pseudocode

Independent non-executable RECONSTRUCTED descriptions of CONFIRMED original instructions. Preserve separate state reads, original conversions and opaque helpers; no substitute input/tracking engine.

## Parent selection and initial head result

```text
RECONSTRUCTED — reached plain M2295 slider branch
parent = first existing candidate passing original M24c7(mappedPointer,true,R)
retain priority/Play guards and M0ad9 ordering/strict broad-admission decision
if admitted, parent has Slider2 and not Normal1, and not parent.head.evaluated:
    if leftNew or rightNew:
        parent.rememberedMask = leftNew ? Left : (rightNew ? Right : currentMask)
    h = parent.head.virtual M24a9()               // T06f6 -> M351d
        direct M1884():
            head.evaluated = true
            e = AbsInt32(unchecked(freshF2358 - head.start))
            head.raw = e < tight ? 1024 : e < middle ? 512 : e < wide ? 256 : -131072
            retain original hooks and result reread
        return baseResult > 0 ? baseResult : -262144
    if h > 0:
        Play.current = parent
        ruleset.M0ad2(64,parent)
        retain original activity/feedback writes
    else:
        ruleset.M0ad2(-262144,parent)
// Not manager.M28d3(head), not parent.M1465, and no live tracking flag write here.
```

## Current-object and ordering boundary

```text
RECONSTRUCTED — reached M226b ordinary update
M2275():
    Play.current = F136a ? null : existingCandidates.Find(first satisfying:
        object.start != object.end and virtualStart <= freshF2358 and
        (virtualEnd >= separatelyReadF2358 or not object.evaluated))
    retain other original derived flags
manager.M28e2()                                   // candidate refresh afterward
if original recorded/nullable-time conditions permit:
    store separatelyReadF2358 in Play.F13d9
    ruleset.M0ac5():
        if reread Play.current is not null:
            before = original current slider tracking flag
            r = reread Play.current.virtualM24c3(fresh storedPointerF2a5c)
            ruleset.M0ad2(r,reread Play.current)   // even r0
            after = original reread current slider tracking flag
            Play.activity |= (before != after)
    retain following original helper
manager.M28e3()                                   // branches rejoin before this call
    for object in refreshedCandidates:
        object.virtualM24b2()                     // parent M1449
        manager.virtualM28e4(object)              // may timeout embedded head
```

## First live held-and-space predicate

```text
RECONSTRUCTED — parent M146f, plain route plus preserved original variants
if parent.evaluated or freshF2358 < parent.start: return 0
followOK = false
h = heldLatch and (previousChangeMask != Both or olderChangeMask != currentMask)
held = false
if heldLatch:
    if rememberedMask == 0 or (currentMask != Both and h):
        rememberedMask = leftNew ? Left : (rightNew ? Right : currentMask)
        held = true
    else:
        held = (currentMask & rememberedMask) > 0
else:
    rememberedMask = 0
held = held or h or F137b
// Each original field read remains separately observable; aliases are not an atomic snapshot.
if held:
    radius = parent.tracking ? manager.R * 2.4000000953674316f : manager.R
    q = unchecked(freshF2358 - originalProviderConditionalDelta())
    record = ball.transforms.Find(first start<=q and end>=q)
    if record null and q > lastRecord.end: record = lastRecord
    retain T0170/M43a8 special first-record fallback
    if record exists:
        if record.start == record.end:
            required = record.to
        else:
            factor = 1f - r4(unchecked(record.end-q)) / r4(unchecked(record.end-record.start))
            required = record.from + (record.to-record.from)*factor
        pointer = original M0ff1(suppliedStoredPointer)
        followOK = ordered(DistanceSquared(pointer,required) < radius*radius)
// No embedded-head success read; no M145e/M145f substitution or safe missing-list fallback.
```

## Tracking mutation and intentional stopping point

```text
RECONSTRUCTED — continuation preserving the deferred boundary
if followOK and not parent.tracking:
    M146d():
        parent.entryTime = freshF2358             // first store
        retain original presentation/list work
        parent.tracking = true                   // later store, not atomic
r = 0
k = 0
while k < componentTimes.Count and componentTimes[k] <= freshF2358:
    k += 1                                      // original time read on each comparison
if unchecked(parent.F0bf1 + parent.F0bf5) < k:
    r = ORIGINAL_COMPONENT_PROCESSING_BOUNDARY(followOK,parent.entryTime,...)
    // Must execute original operations when due; full tick/repeat/tail rules UNRESOLVED.
if not followOK and parent.tracking and
   unchecked(parent.F0bf1 + parent.F0bf5) < parent.componentTimes.Count:
    M146e():
        retain original future-time/presentation/list work
        parent.tracking = false                  // entryTime not rewritten
return r

CONFIRMED no-due scoped consequence:
    fresh counters0, k0 -> r stays0 while tracking can enter or leave.
UNRESOLVED:
    Complete due-component result rules, tail/aggregate scoring, runtime execution/pixels.
```
