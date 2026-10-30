# 1. Result

For an osu! player, judging the last object and entering results are separate decisions. In the recovered ordinary path, Play uses time, clock/lifecycle state and a finalized-record latch. It does **not** require a scan proving every registered object's evaluated flag is true. With stable ordinary nonzero clock state, finalization can occur strictly after the maximum stored object end plus 200 ms; the automatic departure gate normally waits until maximum end plus 3000 ms, with a separate timed-event horizon when applicable. A recognized exit control can request departure earlier once finalization is latched.

These are **RECONSTRUCTED player-facing consequences of CONFIRMED original static instructions**, not measured gameplay. The ordinary destination request is original `OsuModes.Rank=7`. Issuing it does not prove acceptance, commit or a displayed results screen.

## Objective, scope and provenance

E27 connects ordinary base Play's later update to its continuation gate, finalization/record mutation, first departure selector, existing M46ad guards and conditional M46b0 commit. It investigates one circle, one one-traversal Linear slider and one ordinary spinner as separate conditional controls. It does not implement a game/exit model, full audio/lifecycle/score/accuracy/HP/replay/submission system, transition animation or results renderer. Work 2 and `reconstructed/` were neither inspected nor modified. No authorised exception.

Read the complete standing contract, cumulative history including E26, relevant E02/E03 request/commit evidence, E05 ordinary-factory correction, E07 candidate mechanism, E10 clock, E12 update correction, E14 resource-order clarification, E17/E19 and E24–E26 object completion/handoff evidence. Historical files remain unchanged. The previous suggestion to investigate “all objects evaluated” was a question, not an established global rule; this expedition finds a different cooperating set of conditions.

Dedicated branch `work1/expedition-27-gameplay-completion` starts directly from requested **`3aeb7ff581eb1181aa4bd8f8784ecc2b3e4ddbc4`**, tree `ceb351938ba3fc1f77842e134ef2bb405f789c97`. All 125 baseline blobs/modes matched remote; the local mirror has the same tree and different commit identity. Publication uses the exact specified remote parent. Preserve 124 other historical files, the exact 261782-byte journal prefix, all existing fixtures and the unchanged E20 packet.

**E20 remains preparation-only and incomplete. All 17 original-runtime cases are NOT RUN — WINDOWS HOST REQUIRED. Every E27 runtime case is also NOT RUN.** Work 2 remains suspended. No actual reference import, gameplay, clock measurement, results screen or instrumentation overhead was obtained.

## Deliverables, integrity and reproduction

Added this report, `re/tools/gameplay_completion_evidence.py`, `re/tools/check_gameplay_completion.py`, `re/evidence/27/gameplay-completion.json`, and an append-only E27 journal entry. No new fixture, binary, audio, reference asset, full disassembly or private dump is published.

| Input | SHA-256 / status |
|---|---|
| ZIP,18 entries,CRC valid | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| ZIP member osu!.exe | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Standing contract, identical supplied attachment | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |
| E20 one-circle control | `682d600f7a76436c9ec17f1b2ccf52afc968ebf5ef81c3e74b282161c5d0aa0d` |
| E16 one-traversal slider control | `f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84` |
| E24 spinner control | `85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c` |
| Existing generated E01 silence.wav | `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977`; existing E20 generator/verification, no audio added |

Archive and EXE bytes were rehashed. Self-declared Stable40/file-version1.3.3.8; exact official dated identity, authenticity and modification status remain UNRESOLVED. Linux x86_64, no wine/dotnet/pwsh/powershell/DISPLAY/WAYLAND or supplied authorized Windows endpoint. Missing dnfile was reinstalled at the start and again after session resumption: dnfile0.18.0/dncil1.0.2/pefile2024.8.26. No original EXE/CLR/native operation or surrogate completion engine ran.

Mxxxx/Fxxxx/Txxxx abbreviate original EXE MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx. Offsets are **hexadecimal, relative to the first CIL opcode**, excluding headers; JSON offsets are decimal. They are not JIT/native addresses. CONFIRMED means original instructions/metadata; aliases, player explanations and independent pseudocode are RECONSTRUCTED. Runtime settings/operands/helpers/callbacks/pixels remain UNRESOLVED.

Competing explanations: all evaluated versus timed continuation; current-candidate exhaustion versus full-list endpoints; audio EOF versus a distinct clock state; one completion/request operation versus separated latches and delays; result-count mismatch veto versus record invalidation; final-object payload versus existing record; accepted request versus delayed commit. Discriminate with private original CIL/stack inspection, closed ordinary virtual slots, full direct selected-field/caller inventories, branch destinations, metadata and body hashes. No remembered osu! rule or alternative implementation is proof.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/gameplay_completion_evidence.py /private/osubackup.zip > /private/end-a.json
python re/tools/gameplay_completion_evidence.py /private/osubackup.zip > /private/end-b.json
cmp /private/end-a.json /private/end-b.json
cmp /private/end-a.json re/evidence/27/gameplay-completion.json
python re/tools/check_gameplay_completion.py re/evidence/27/gameplay-completion.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

The read-only extractor reuses unchanged E04/E05/E15/E25 collectors: 66 selected method entries, field/signature/ancestry/slot metadata, original constants/branches/conversions/local sites, full direct watched inventories, 35 focus-writer body identities, original mode/grade/EventTypes metadata and the bounded auxiliary-end parser sites. Full EXE scan: 19659 decoded bodies/1739 zero-RVA/0 decode errors. Direct inventories exclude reflection/native/external mutation and arbitrary transitive callbacks. Original binaries/full disassembly remain private.

Actual checks recorded for publication: two fresh extractions/public-byte equality; focused checker: 66 methods/156 historical body-hash comparisons; all 24 previous research checkers; unchanged E20 package safeguards; Python compilation; fixture/history/journal-prefix/three-heading/case/diff checks and remote parent/tree/branch/all-blob verification. These are static extraction/consistency checks, not E20 acceptance. An initial draft assertion put M2285's F2c73 read at0021; original token sites show001d, corrected before focused validation. No historical report was changed.

# 2. What they found

## A. Distinct state and exact update placement

The scoped identities remain ordinary **base Play T047b**, handler **T0253**, manager **T00e8**. The corrected circle/slider/spinner paths are preserved. Special Play subtypes, recorded input and other rulesets are not silently equated with this base path.

| Symbol / original token | Type / origin | Scoped role (RECONSTRUCTED) |
|---|---|---|
| T=`F2358/04002358` | static Int32, corrected gameplay clock from E10 | Separately read at each original site |
| B=`Play.F13bf/040013bf` | Int32; ctorM222e:001f sets Int32.MinValue; loadM223b:0203 raises to each greater F2524 | Maximum stored end across the loaded full object list |
| K=`Play.F13c4/040013c4` | Boolean; sole direct writer M226b:0f0b | Recomputed continuation gate; not evaluated-object count |
| D=`Play.F1397/04001397` | Boolean; fresh defaultfalse; sole direct writer M0acd:0007=true | Finalization latch, not “all hits successful” |
| m=`Play.F13a2/040013a2` | Int32, ctor−1; M2271 updates/resets | Auxiliary boundary marker; not final evaluated-object index |
| Q=`Play.F13a7/040013a7` | Queue<Int32>, allocated M2243:0006 | Prepared boundary times, not a processed-object counter |
| q=`F2368/04002368` | original enum-like T06e4, no recovered literal names | Clock/playback state; compare numeric zero/nonzero, not assumed audio EOF |
| F17fb/F17fd/F17fc | Full list / refreshed candidates / cached count | Different collections/count operands |
| F136a/F1391/F2c6b | failed latch / startup-related gate / application transition phase | Separate gates, read at different times |
| J=`Play.F138a.F1b9f`, Z=`F1bb0` | List<T0202> and Int32 on T0628 | Separate timed-event collection and accumulated end horizon |

Loading M223b iterates **full F17fb** at01d8–0229. It compares each object's F2524 against B and stores only larger values (`01ef/01f5/01fa/0203`). B is not necessarily the end of the last object by start ordering. No per-object evaluated/result event writes B. Reusing a Play object without rebuilding its state is not assumed: B has no other direct reset writer beyond its constructor initialization.

Authoritative reached order remains clock/input/current-old-candidates→candidate refresh→optional component processing→M28e3 object updates and M28e4 evaluation→other original handler/Play work→**M2271 at M226b:0e5f**→completion cooperation below. E12 branches0972/09a6 rejoin09c9 before M28e3. There is no new completion callback invoked by the last circle/slider/spinner evaluator.

The later block requires **transition phase F2c6b==2** (`0e64–0e6a`) and **F1391=false** (`0e6f/0e74`). Earlier loading/readiness M2269 can return at000e; pause F1373 returns0853, F13be has separate branches/return08ba. **Failed latch F136a** is checked at0abd: true calls M2272/M2262 and returns0ad0 before this ordinary completion block. Exceptions and intervening original helpers can also interrupt it. These are caller conditions; M2276 itself does not repeat all of them or test current application mode Play2.

## B. The recomputed continuation gate K

M226b:0e79 leaves the Play receiver on the stack and computes the Boolean stored at0f0b. In original short-circuit order, K becomes false if any reached condition rejects:

1. F2c73 true and **F235a<=F2c74** (`0e7a–0e8b`). These are not T and B.
2. M4390() true and **F2a81>=F235a** (`0e8d–0e9e`). M4390 simply reads F2a7c; keep its original recorded-input route separate.
3. Fresh T **< unchecked(full[0].start − manager.F0adb)** (`0ea0–0ec7`). Equality passes.
4. Another fresh T **> full[F17fc−1].end** (`0ec9–0ef0`). Equality passes. This is the last indexed object's end, **not B's maximum**.
5. Play.F13a2 **>=0** (`0ef2–0ef9`).
6. Fresh q **==0** (`0efb/0f00`).
7. Otherwise use **M226f()** (`0f03`); do not replace it with true.

There is no F1527/evaluated read, result enum, pending slider schedule, spinner P/C/E or all-object predicate in these comparisons. Null/full-list/index errors are not converted to a benign false. F17fc is a separately stored count; no atomic list/count snapshot exists.

### Necessary marker/queue boundary

M2271 is called before that gate. If F1b97 is true, it writes fresh T to m at000d, retains its original optional input helper, and dequeues Q while **Peek()<fresh T** (`007c`); equality is retained. If m>=0, it searches current **candidate F17fd** using M22b1; any match resets m=−1 (`009e–00a8`). M22b1 only tests **object.start<=fresh T and object.start>=m**, with separate reads; it does not test evaluated. F1b97's directly managed writer is timed-event manager M2f05; its wider feature/UI policy is outside this expedition.

M226f returns true for empty Q. Otherwise if **T<=Q.Peek()**, returntrue. If T>Peek, return **map.F0fd1<8**, a signed format comparison (`001f–002f`). M2243 allocates Q and, for reached boundary records in F138a.F1b9e, enqueues the end of an original object selected through M28de (`0159–017f`). No claim that those entries are unique/chronological or that they count judged objects is needed. The original event enum names **Break=2**; full break/skip behavior remains bounded at these calls.

K alone is not “map finished.” It can be false before the first object or during special activity, yet M2276 has additional time/state guards. An empty candidate list alone does not set D or request Rank. Conversely full-list endpoints can stop continuation with unevaluated objects still in memory.

## C. Concrete finalization decision M2276

**M2276/06002276**, nonvirtual instance void on T047b, is called directly only by **M226b:0f11** in the EXE inventory. Its ordered decisions are:

| Offsets | Original operation |
|---|---|
| `0001–0008` | If D alreadytrue, return. |
| `000a/000f` | If K true, return0114. |
| `0014/0019` | If F1376 true, return0114. Preserve the original suspension/recovery-related flag rather than naming it a generic pause. |
| `001e–002b` | If **T>0** and a separate **q==0** read, jump0043 without the B+200 comparison. |
| `002d–003e` | Otherwise require fresh **T>unchecked(B+200)**; signed `ble→0114` includes equality in the rejection. |
| `0043–00c6` | Append handler.virtual M0a9f() to **global record F13c3.F1983**; count true/false samples using M22c7/M22c8, pass trueCount>falseCount to original M2284. |
| `00cb–00ef` | Recorded-input M4390, F2c73, or !virtual M2278 can bypass count validation; otherwise handler M0aa5 true proceeds. Invoke virtual M2277. |
| `00f5–010f` | On failed count validation, call global record **M2b5a**, original diagnostic helpers, then **still invoke M2277**. |

Thus for ordinary stable nonzero q, a valid one-object post-end state can finalize **strictly beyond B+200**, not at equality. It is not delayed until audio physically ends. The q0 shortcut is conditional on q at the actual consumed site and positive T; it is not a general audio-ended callback or an unconditional early-result rule.

M0a9f is inherited by ordinary T0253: null F05f5 returns true, otherwise its original target getter is tested against Double100 using unordered-aware instructions. F1983 is **List<Boolean>**, not a judgement-result payload. The history/majority and M2284 original helper are retained; their presentation/observer policy is not reconstructed.

### Count validation does not mean all objects evaluated

Ordinary base M2278 returns true. Ordinary T0253 inherits M0aa5: add handler-record UInt16 **F196a+F1969+F196b+F196e** using Int32 arithmetic and compare equality with manager **F17fc** (`003a ceq`). These are the previously recovered whole-object result counters; each field/reference is loaded separately. It is **not** a full F1527 scan, candidate count, combo count, group-decoration counter or proof of complete component accounting.

If equality fails, original **M2b5a:0002 sets global record F1990=false** and original diagnostic calls occur, but M2276 still calls M2277 at010f. Counter mismatch can invalidate the record without preventing finalization. Null records/helpers/exceptions can instead interrupt it. Do not silently turn this into a required `judgedCount==objectCount` exit condition.

## D. First finalization mutation and result-data provenance

Ordinary base virtual **M2277** first calls **MethodSpec2b000084→M2cee<T0970>** on Play.F13b2 with Play.F137f (`0001–000c`). M2cee removes the supplied presentation references from active F1a6c, with its original fast/fallback paths and optional M473d cleanup. This is **presentation-list work before the latch**, not disposal of full hit-object list F17fb or draining components. It can throw before finalization.

M2277 then calls handler.virtual **M0acd at0017**, resolving inherited **T0186.M0acd** on ordinary T0253. Exact immediate stores:

1. **`M0acd:0007` writes its Play receiver F1397=true** — sole direct managed writer. This is the first established session-finalization latch.
2. **`001a` writes handler record F1978 = !fresh F136a**. It records a success/failure-related condition, not whether the last object was a positive hit.
3. **`003d` writes handler record F197c** from the original signed expression `!(unchecked(F05fd−record.F1975)>0)`. F1975 is the E18 maximum-like combo operand; no complete grade/FC formula is assigned to F197c.
4. Retain conditional F05f5/M296a, two presentation-list fade-helper loops and **M296b(false):012c**, including locks/finally, virtual hooks and exceptions. No health UI or sound claim follows.

There is **no newly constructed result-summary/snapshot object required by the scoped chain**. M2276 uses **global F13c3** for Boolean history and invalidation; M0acd writes the **handler F05f3** record; M2285 later queries the **global F13c3** again. Ordinary configuration M2258→M0ace aliases global to handler at M0ace:0006, as E23 established, but assignments/reads are separate. A changed/restored/mismatched record must not be replaced by an invented immutable payload. No final-object reference is passed to M2277/M2285, and manager F17ff is not the exit payload here.

D=true guards future M2276 calls even if a later M0acd helper throws. An exception can leave D latched with partially updated records and no exit request. Conversely an exception in presentation removal/M2284 before the latch can permit a later retry with prior partial side effects. No transaction/rollback exists. D is not universally proof of completed result-data production.

## E. Automatic departure gate and first requested destination

After M2276 returns, **the same reached M226b block** performs a separate decision:

| M226b offsets | Required condition / next step |
|---|---|
| `0f17–0f27` | **unchecked(B+3000)<=fresh T**. Signed bgt skips; equality permits. |
| `0f2a–0f51` | If timed-event list **J.Count!=0**, also require **unchecked(Z−500)<=another fresh T**. Empty list bypasses this comparison. |
| `0f54–0f61` | Ordinary base requires **D=true**. Special `this is T04af` can bypass this latch check; not generalized to ordinary base. |
| `0f64` | Call **M2285**. This is an exit selector, not a rendered results screen. |

B comes from full-object loading maxima; J/Z come from a **different T0628 timed-event system**. J is List<T0202>, whose original F0877 enum metadata is `EventTypes` (Background0,Video1,Break2,Colour3,Sprite4,Sample5,Animation6). M2efd/M2efe compare a supplied event's end-like F086e against Z and store greater values at0015 before adding the event to J. M28f7 also raises Z at0b60 from the reached parsed end operand plus map offset. Full direct Z writers are those three methods, not object-evaluation routines. No exact active J contents, resource lifetimes or pixels are inferred.

The optional event horizon can hold the automatic request past B+3000. It is not a fixed “wait for all audio” rule or a second object-judgement deadline. Its default and actual contents must be logged. Arithmetic is unchecked Int32; preserve overflows and separate time/record/event reads.

### M2285 selector, exact destinations

M2285/06002285 is nonvirtual instance void, T047b:

1. Optional original prefix **F1122 && !M4390 → M1dfa(3,default nullable argument)** at0018. Its purpose is not assigned an online/submission meaning; preserve exceptions/side effects.
2. If **F2c73=true** (`001d/0022`), call original M4391(false), then **M46ad(Edit1,false):002c**, return0031. This is a separate special path, not ordinary Rank.
3. Otherwise call **global F13c3.virtual M2b47 at0037**. If its enum equals **8**, and `this` is not T04af, call global M2b5a and original diagnostics, then **M46ad(SelectPlay5,false):0058**, return005d.
4. Otherwise **virtual M2286(false):0060**, resolving ordinary base **M2286**. Its eight-byte body is **M46ad(Rank7,false):0002**, then return. The passed Boolean is not read by this base implementation; it does not forward it as force.

Original `osu.OsuModes` metadata names Edit1,SelectPlay5,Rank7. Original result-grade enum T0241 names **F=8**. M2b47 has some original arithmetic/helper work before reading F1978 at0021; **false F1978 returns8 at0028/0029**. Its other normal returns are different grade values; full grade/accuracy formulas are deliberately not reconstructed. Preserve the actual virtual call, not a replacement `if failed then F` implementation.

Under stable ordinary global/handler identity and successful M0acd with F136a=false, F1978 becomes true, supporting the non-F Rank route when that original getter returns normally. Missing the last object need not set the gameplay failed latch; it does not by itself force SelectPlay. Count invalidation F1990 and grade8 are distinct fields/operations. Failed latch, record success flag, record invalidation, returned grade and requested mode are not interchangeable.

### Earlier departure through an existing control

The automatic B+3000 gate is not a universal minimum delay for every request. On recognized controls, original M225b checks transition phase2 and its loading/input guards; **if D true, M225b:004e calls M2285 immediately**. M2262's separately guarded pending-control path likewise calls M2285 at00b9 when D true. These do not re-run the automatic time gate. Full keybinding/pause UI is out of scope. Direct callers also include special variants; the call inventory does not establish each as ordinary natural completion.

## F. Request issued, accepted and committed are different boundaries

The scoped ordinary request is exactly **M2286:0002→M46ad(7,false)**. E03's complete request contract remains authoritative; M2286 does not call the committing method directly.

For this Rank request, M46ad can:

- Defer nonzero targets when signed host F2c3a<F247e (`0030–006d`), queue **M46e6** with original unchecked delay expression, then return without a pending-mode write. Callback M46e6:000c reinvokes the entire M46ad with captured mode/Boolean and **live global guards**. No Play D/time/record eligibility recheck is added by that callback.
- Reject the original Edit-state guard if applicable. Require original opaque M477a(false)/M3b5b for non-Exit/non-Update; failure redirects to Menu0 and sets duplicate Booleantrue. Their meanings remain UNRESOLVED.
- With F2c48 true, a still-Rank target bypasses the Play-exit blockers (unlike some redirected targets). Preserve target/guard ordering from E03.
- Suppress equal-current/equal-pending requests when Booleanfalse and suppress a pending Exit3 irrespective of that Boolean. Helpers run before some suppression checks; a suppressed request is not proved side-effect-free.
- On accepted normal continuation, after original callbacks **write pending F2c6c at022a**, retain original helpers/rate, then **phase F2c6b=0 at02e8**. Current mode has not yet been changed by this method.

No extra “request already issued” flag is written by M2285/M2286. D prevents repeated **finalization**, not every request attempt. A deferred/rejected/suppressed request can be attempted again by a later eligible Play update, potentially queuing several callbacks. An accepted phase0 normally excludes the next phase2-only automatic block, subject to earlier returns and helper/state mutation.

The deferred callback carries mode/Boolean rather than the old Play instance or its result record. It can therefore execute after gameplay state changes; M46ad rechecks its own current guards, not the old completion predicate. Actual queue ordering/cancellation/thread behavior remains UNRESOLVED.

**Conditional commit boundary, not execution:** E02's outgoing transition path remains original M46a3. Phase0 tests progress>=100 before arithmetic; reached callback **F2c77 at0102** can invoke registered **M46ef→M46b0(0)**. Original M46b0:

- Returns when pending Unknown10.
- After original prefix, stores previous=current at0068 and **current=pending at0072**.
- Removes/disposes prior active mode object via M1ab0 at00ca for parameter0, then clears active reference; a positive argument uses queued cleanup instead.
- For current Rank7, original switch0255 entry7→04b5 constructs **M32d0/T06a8 at04ba**, stores active at04bf. Only constructor metadata is followed here; no results UI/body reconstruction.
- Registers active before virtual initialization at0564; later clears pending to Unknown only if it still equals current (`0569–0577`). Reentrant/superseding requests are not blindly cleared.

Thus completion permitted → selector/request → accepted pending write → commit/current change → Rank object construction/initialization are separate static boundaries. No actual transition, rendered result, GPU frame or record display is observed. Existing pending Exit or intervening requests can suppress/supersede completion's Rank request. Gameplay object disposal belongs to the later commit lifecycle, not M2276's per-object-state inference.

## G. Audio, failure, interruption and exceptional state

### Clock-state dependence is not an audio-end callback

q is **not** a direct audio position or Boolean “finished song.” E10 and this bounded continuation establish:

- M3b25 updates the corrected clock before Play work. It can set q=0 at00ce if playback F2367 is null or its M3d62 active predicate is false, unless synthetic F234c permits continuation. M3b24 has other q0 conditions. No direct Rank request occurs at this native-state boundary.
- Native receiver T08e4.M4385 invokes **bass.dll!BASS_ChannelIsActive/M313a**; original unsigned arithmetic accepts returned codes1 or2. Fallback T086e.M4053 reads F2614. No actual native return/device/end state was measured; names for raw native status codes are not imported.
- Before object updating, **M226b:091b–093d** can see q0 with !F1376,!F1391,!F1396 and call **M3b58→M3b59(F235b,false)**. That establishes synthetic state, republishes time and sets q1 before the later completion comparisons. A naturally stopped source does not guarantee q remains0 at M2276.
- M2276's shortcut only uses **q at0026** and T>0. The main automatic request gate reads T/B/event horizon, not a native audio length/position or “audio callback completed” flag. Play can reach the time-based path with nonzero q while audio remains active, if all original guards allow it.

Pause F1373's earlier return can prevent ordinary automatic completion regardless of audio state. A missing channel, held/synthetic clock, seek or stopped source must be evaluated through actual original q/T publication and caller gates. No generic audio shutdown/EOF scheduler is reconstructed.

### Failure does not mean normal completion

On the reached ordinary update, F136a=true at0abd takes M2272/M2262 and returns before M2276. M2272 retains original frequency/presentation work; when its final condition is reached it invokes virtual M227e→M2261, which records pending control F13d6 and fresh gameplay time F13d7. That is the bounded failure/control boundary here, not proof of immediate Rank or SelectPlay. Full failure menu/retry/rendering remains outside scope.

If a later original route reaches M2285 with a record getter returning F8, SelectPlay5 is the scoped destination unless the special T04af branch applies. Normal successful finalization can share M2285 as a selector but has a different route/state. Do not infer a destination merely from the failed latch or a last-object negative result.

Earlier loading/pause/F13be returns, current-object nullable gates, candidate omission, interposed helpers, original control callbacks and application transition phase can interrupt the path. A failed latch set during object result handling is checked before the ordinary late finalization block in that same normally returning Play pass. Distinct later reads can observe changes; a reentrant/forced failure after that check is not ruled out by an invented immutable state snapshot.

### Empty or partially initialized inputs

M223b:019b–01b5 detects zero manager F17fc and takes original cleanup/virtual M225a at01bc, whose base implementation requests **SelectPlay5,false**. This is a loading-failure departure, not normal completed gameplay. Keep the accompanying original timing-list checks/diagnostics and asynchronous loading boundary from E04; no import result is asserted.

If an artificial later empty full list reaches update, earlier M226d/full[0] or this gate's unchecked full[0]/full[F17fc−1] can throw. There is no safe empty-map Rank path inferred from B's Int32.MinValue initializer. Null manager/global record/history/event collection can also interrupt at original dereferences; no default payload or list is invented.

D has only the true writer M0acd; fresh Play construction supplies defaultfalse. A new ordinary session must obtain appropriate newly initialized Play/manager/record state; no direct false reset of D exists in this EXE inventory. Other startup/map/end/queue fields have independent initialization. Full replay/reset/seek/restart policies remain UNRESOLVED.

## H. Complete focused inventories and body identities

All direct EXE accesses and owning types/signatures are in `entry_field_sites`/`field_access_owners`. Relevant complete writer sets:

| Field | Complete direct managed stores | Direct readers / bounded note |
|---|---|---|
| F1397,Boolean,T047b | M0acd:0007 only | M21aa,M21b5,M2256,M225b,M2262,M226b,M2276,M2280,M2405,M2408,M43a0; all offsets in JSON |
| F13c4,Boolean,T047b | M226b:0f0b only | M0c65:0006,M1146:000e,M2276:000a |
| F13bf,Int32,T047b | M222e:001f;M223b:0203 | M223b:01f5,M226b:0f17,M226d:0042,M2276:0033 |
| F13a2,Int32,T047b | M222e:0014;M2271:000d/00a8 | M226b:0ef3,M2271:007f,M22b1:0014 |
| F13a7,Queue<Int32>,T047b | M2243:0006 | M2243:0159,M226f:0001/0015,M2271:0044/0054/0060/006d |
| F1391,staticBoolean,T047b | M2237:04cf;M226b:08ed | M226b:08bb/0929/0e6f |
| F1bb0,Int32,T0628 | M28f7:0b60;M2efd:0015;M2efe:0015 | M226b:0487/0f41;M28f7:0b51;M2efd/M2efe:0007 |
| F1b9f,List<T0202>,T0628 | M2efa:01ee/0269 | Full consumer inventory in JSON; end gate reads only count |

F1978/F197c/F1983, q, manager lists/count, failed/current/pending/phase fields are likewise inventoried; their unrelated writer variants are not assigned ordinary lifecycle semantics.35 focus-writer bodies receive independent hashes, including writers outside selected methods. Reflection/native/external mutation and arbitrary callbacks remain excluded from the direct-inventory claim.

Representative original body SHA-256, including original headers:

| Method / signature | Body SHA-256 |
|---|---|
| M226b /`200001` | `de32a6e69eedceee8be3f629c986d16f35fe7b99952e0389a3146b1b8863f52f` |
| M2271 /`200001` | `63a41eff0cc1f2cb897452c2ee8c9d26e50604a2479dc5275abdfbd210e7fece` |
| M226f /`200002` | `d74d345709a552d9aa2738f81389bf613012a1cd2dd62c71822949411e3c3aa0` |
| M2276 /`200001` | `f4fd9f4681de7532e41bf54a1bc2430883e714921e09cd4d60e2d2f10d66406a` |
| M2277 /`200001` | `326d29cb77aa84ca96275a81e74d978582fa265937f09c058f741eec16f44fd5` |
| M0acd /`200001` | `b3b2b82998a7202026f3377cdda41ab369015b411557e91d68a265299b2f3391` |
| M2285 /`200001` | `f8335ff08c9184f35c668aa36e424ee0cec243f1c5968ec93c65b0039ee4888d` |
| M2286 /`20010102` | `d3a78cce842087832666c67fc9ea5e3bd046267737198589033d5249248fc6c4` |
| M46ad /`00020111ad2c02` | `8e25430f79dd7f0cff8e522b79c12823a0e9b7371e3f1efd410017b94dce7c14` |
| M46b0 /`00010108` | `94d6c712f08f534dec50ce0d8754b7a84dd8555fec12d2cca078fcd1230b6302` |

## I. Conditional static reference cases

All rows are **NOT RUN — WINDOWS HOST REQUIRED**. Controls reuse unchanged E20 circle/E16 slider/E24 spinner, each in a separate intended ordinary run. Assume successful import/initialization, nonempty consistent full list/count, stable original helpers, F136a/F1373/F1376/F1391 false, phase2, valid aliased records and q nonzero unless varied. For timed numerical examples assume the auxiliary event-list count0; actual imported operands must be logged. No case injects artificial object state and calls it natural gameplay.

| ID | Controlled condition | Original branch-derived prediction | Runtime |
|---|---|---|---|
| G01 | One circle, positive evaluated result at S=end2000 | Per-object result alone does not finalize; K can remaintrue at end equality | NOT RUN |
| G02 | Same circle expires negatively, failed latch remainsfalse | Later time path can finalize normally; a miss is not a global veto | NOT RUN |
| G03 | Circle B2000,T2200,q nonzero,Kfalse | B+200 equality rejected by M2276 | NOT RUN |
| G04 | Circle T2201, same gates | Finalization permitted; Dtrue after original cleanup | NOT RUN |
| G05 | Circle Dtrue,T4999 | Automatic B+3000 gate not yet met | NOT RUN |
| G06 | Circle Dtrue,T5000 | Equality permits automatic M2285 selector | NOT RUN |
| G07 | One slider end/max3500, parent evaluated positively | Independent finalization waits beyond3700 in nonzero-q control | NOT RUN |
| G08 | Slider parent evaluated with pending checkpoints | Pending components are not inspected by the session gate | NOT RUN |
| G09 | Slider T3700 versus3701 | Rejection at equality versus permitted finalization | NOT RUN |
| G10 | Slider Dtrue,T6500 | Automatic departure time equality permits selector | NOT RUN |
| G11 | One spinner exact end5000 | Spinner manager completion skips equality; session K time-bound can still be true | NOT RUN |
| G12 | Spinner positively evaluated after5000 | Finalization can occur beyond5200; object result not immediate exit | NOT RUN |
| G13 | Spinner negative final result, failed latch stillfalse | Same time/finalization gates, subject to record state/normal helpers | NOT RUN |
| G14 | Spinner P/E has unreported integer change | Session gate performs no spinner notification drain | NOT RUN |
| G15 | Spinner T5200 versus5201 | Strict finalization threshold equality fails; later permits | NOT RUN |
| G16 | Spinner Dtrue,T8000 | Automatic departure equality permits selector | NOT RUN |
| G17 | Every object evaluated but T<=B+200 and q nonzero | Evaluated flags insufficient for this finalization path | NOT RUN |
| G18 | Unevaluated registered object after large jump beyond B+200 | No all-evaluated veto; count mismatch can invalidate record yet still finalize | NOT RUN |
| G19 | Empty candidates, full list still contains an unevaluated object | Empty candidates alone proves nothing; time/q/K/caller conditions decide | NOT RUN |
| G20 | Last object lost candidate attention | Its evaluation may be absent; full endpoints/max end remain independent | NOT RUN |
| G21 | Counter sum differs from F17fc | M2b5a marks global F1990false, then M2277 still called | NOT RUN |
| G22 | Counter sum equals cached count but flags differ, artificial inconsistency | Validation does not scan flags; no synthetic flag repair | NOT RUN |
| G23 | Audio source still active, q nonzero,T>B+200 | No native-audio-end requirement in reached finalization | NOT RUN |
| G24 | Source inactive but synthetic F234c supports qnonzero | Original clock/time path can continue; do not equate q with source EOF | NOT RUN |
| G25 | q0,Tpositive at consumed M2276 site,Kfalse | Can bypass B+200, if upstream conditions actually preserve q0 | NOT RUN |
| G26 | q0 upstream,!F1376,!F1391,!F1396 | M3b58 can republish synthetic time/q1 before completion check | NOT RUN |
| G27 | q0 but T<=0 | Shortcut not taken; original B+200 comparison still applies | NOT RUN |
| G28 | Pause/readiness/earlier F13be return | Automatic late block not reached; no invented completion/reset | NOT RUN |
| G29 | Failed latch true at0abd | Failure/control path and return, no ordinary M2276 call in that pass | NOT RUN |
| G30 | Phase!=2 or startup gate F1391true | Late finalization/automatic request block skipped | NOT RUN |
| G31 | D alreadytrue at M2276 entry | No repeated history/record finalization there | NOT RUN |
| G32 | Dfalse, finalization first reached atT>=B+3000 | Same normal Play pass can finalize then call selector, no extra mandatory callback | NOT RUN |
| G33 | J.Count>0,Z−500 greater than currentT | Automatic selector delayed despite B+3000 satisfied | NOT RUN |
| G34 | J.Count>0,T exactlyZ−500 and B+3000 satisfied | Auxiliary equality permits request attempt | NOT RUN |
| G35 | J.Count0 with same storedZ | Auxiliary Z comparison bypassed | NOT RUN |
| G36 | Last-by-index end differs from maximum B | K uses indexed end; M2276/timer use B; retain difference | NOT RUN |
| G37 | Recognized M225b control after Dtrue but before B+3000 | Can call selector without automatic delay gate | NOT RUN |
| G38 | M2262 pending-control prefix admits Dtrue route | Calls selector at00b9; full pause menu not inferred | NOT RUN |
| G39 | Global/handler record match, finalized with failed latchfalse | Handler success flagtrue; normal original non-F getter permits Rank route | NOT RUN |
| G40 | Global getter returns gradeF8, ordinary base | SelectPlay5,false request, not Rank | NOT RUN |
| G41 | F2c73true | Selector calls M4391(false), requests Edit1,false | NOT RUN |
| G42 | Artificial global/handler record mismatch | Handler writes do not determine later global getter result | NOT RUN |
| G43 | M2277 presentation removal throws before M0acd | No Dstore from that call; prior history/helper mutations can remain | NOT RUN |
| G44 | M0acd throws after D=true | Partial record state/latch remains; next M2276 skips, no guaranteed request/pixels | NOT RUN |
| G45 | Object evaluator marked evaluated but its event threw; later update resumes | Session gate can still finalize; record counter mismatch is not a veto | NOT RUN |
| G46 | Empty map rejected during loading | Original SelectPlay request path, not completed Rank gameplay | NOT RUN |
| G47 | Artificial empty full list after loading | Original unchecked indexing can throw; no safe empty-results conclusion | NOT RUN |
| G48 | Null manager/record/history/event collection | Original dereference/helper exception; no fallback payload | NOT RUN |
| G49 | Rank request while host deferral gate closed | Queue M46e6; no accepted pending write yet | NOT RUN |
| G50 | Deferred callback executes after Play/current mode changed | Re-runs M46ad global guards, not old completion predicate | NOT RUN |
| G51 | Pending Exit3 at original suppression check | Completion's request suppressed; flagfalse is no bypass | NOT RUN |
| G52 | Requested Rank equals pending/current, Booleanfalse | Original duplicate suppression, with preceding helper effects retained | NOT RUN |
| G53 | Required request predicates fail | May redirect Menu0/Booleantrue; no Rank acceptance proof | NOT RUN |
| G54 | Rank request accepted, outgoing threshold not committed | PendingRank/phase0 can exist while current remainsPlay | NOT RUN |
| G55 | Deferred/rejected request, later eligible phase2 update | Repeated attempt possible; D does not prevent every request | NOT RUN |
| G56 | Accepted request changes phase0 | Subsequent normal phase2-only automatic block skipped until state changes | NOT RUN |
| G57 | Conditional outgoing callback and Rank commit return normally | CurrentRank, old active disposal, M32d0 construction/initialization boundary; pixels unresolved | NOT RUN |
| G58 | Another request changes pending before commit | Commit consumes then-current pending; no retained guaranteed Rank destination | NOT RUN |
| G59 | Fresh time jumps between finalization and request reads | Different gates can change within one pass; no atomic timestamp | NOT RUN |
| G60 | Artificial integer overflow in B+200/B+3000/Z−500 | Preserve unchecked arithmetic/signed comparisons; finite ordinary delay interpretation invalid | NOT RUN |

All cases are conditional original-branch predictions, not an executable substitute classifier or actual gameplay pass. No visible result, audio event, input latency or instrumentation-overhead measurement follows.

## J. Minimal E20 addition and exact remaining boundary

Do not recreate or modify E20's packet. Add only this future observation checklist in the new report, reusing its original-process/import/privacy/provenance requirements:

- Actual EXE/map/audio/configuration/environment hashes; ordinary Play/handler/manager identities; imported full list/order/count/endpoints/max B and last relevant evaluated states.
- Actual refreshed/old candidates, last object update/result/event, record counters versus F17fc, pending slider/spinner state retained as separate evidence.
- Each consumed T/F235a/q/native-or-fallback state, synthetic fallback, marker/queue/K, failure/pause/readiness/startup/phase and event-list count/Z.
- M2276 entry/return/validation branches, history sample, M2277 cleanup, M0acd first Dstore and handler/global record identities/flags before/after, including exceptions.
- Automatic time equality/selector or recognized-control route, actual M2b47 returned enum, M2286/M46ad arguments and requested/current/pending modes; deferral/rejection/acceptance/commit distinguished.
- Original callback sequence/thread/high-resolution host timestamps, queued callback provenance, actual commit/current/active-object state and any capture. Submitted/constructed state is separate from displayed results pixels.
- Natural successful/missed circle, slider with matched component outcomes and spinner; normal audio continuation and one controlled interrupted-source case if safely reachable. Forced clocks/empty lists/record swaps/helper exceptions/pending modes are artificial probes.

Bind MethodDef/CIL offsets to actual module/JIT locations. Log original operands without calling time/audio helpers again. Measure tracing overhead against matched uninstrumented runs; stepping can alter timing, candidate attention, counters and finalization cadence. Keep proprietary/private artifacts outside Git and link permitted summaries by hashes. Every E20 case remains NOT RUN.

The smallest remaining verification dependency is an original-Windows last-object-to-request trace for the three existing controls, recording **B,K,q,D,T,event horizon,global/handler identity and M46ad acceptance**. If another static investigation is commissioned while Windows is blocked, the narrow next integration is the original pause/control gate's effect on clock advancement and object updates, stopping before menu/retry rendering. Full Rank payload consumption/rendering, score/grade formulas and audio completion timing remain unverified. Work 2 stays suspended; distorted-slider stutter remains separate and unmeasured.

# 3. Pseudocode

**RECONSTRUCTED — original dataflow, not executable gameplay or transition code.** Separate field reads, integer arithmetic, virtual helpers and exceptional behavior are retained.

```text
loading, fresh ordinary Play:
    B starts Int32.MinValue
    for original object in full registered list:
        if object.end > B: B = object.end
    retain original initialization and empty-load failure route

normally reached ordinary Play update:
    original readiness/input/current selection/candidate refresh
    optional current-object component evaluation and consumer
    original M28e3 object updates -> per-object evaluator/result events
    retain intervening original work
    if failed latch at original check:
        original failure/control path
        return
    retain original M2271 marker/queue work
    if transitionPhase != 2 or startupGate: skip completion block

    K = original short-circuit continuation conditions:
        special recorded gates permit
        and fresh T >= unchecked(full[0].start - approachOperand)
        and another T <= full[cachedCount-1].end
        and marker < 0
        and fresh q != 0
        and original M226f()
    store F13c4 = K
    original M2276()

    if unchecked(B+3000) <= fresh T:
        if eventList.Count==0 or unchecked(eventEnd-500) <= another T:
            if finalized D or original special T04af condition:
                original M2285()
```

**RECONSTRUCTED — finalization, existing records and first request:**

```text
M2276:
    if Read(D) or Read(K) or Read(F1376): return
    if not (fresh T>0 and separate q==0):
        if laterFresh T <= unchecked(Read(B)+200): return
    globalRecord.history.Add(original handler.M0a9f())
    original history-majority/M2284 helper
    if original live validation required and not handler.M0aa5():
        original globalRecord.M2b5a()            // invalidation, not veto
        retain original diagnostic helpers
    original virtual M2277()

ordinary M2277:
    original M2cee(presentationRefs)
    handler.virtual M0acd()
        Play.F1397 = true                        // first finalization latch
        handlerRecord.F1978 = not freshFailedLatch
        handlerRecord.F197c = original signed maximum-like comparison
        retain original optional component/presentation helpers

M2285:
    retain original optional prefix helper
    if Read(F2c73):
        original M4391(false)
        original M46ad(Edit1,false)
        return
    grade = original freshGlobalRecord.virtual M2b47()
    if grade==original F8 and not original special T04af:
        original record invalidation/diagnostics
        original M46ad(SelectPlay5,false)
        return
    this.virtual M2286(false)
        ordinary base: original M46ad(Rank7,false)
```

**RECONSTRUCTED — request/commit boundary only:**

```text
M46ad(target,false):
    retain original normalization, deferral, predicates, redirects,
           duplicate/pendingExit checks and callbacks
    if accepted:
        pending = actual effective target
        retain original helpers
        phase = 0
    // No immediate commit or visible result proof.

original later outgoing callback -> M46ef -> M46b0(0):
    if pending==Unknown: return
    retain original prefix
    previous = current
    current = separately read pending
    retain original active-object removal/disposal
    if reached current Rank7 case:
        active = original new M32d0(host)
    retain registration/virtual initialization
    clear pending only if it still equals current
```

**UNRESOLVED:** actual original import, clock/source/helper/native/queue/subscriber/interleaving behavior, natural record configuration and active event horizon, actual request acceptance/commit/capture/overhead. Static reachability does not complete E20 or authorize reconstruction.
