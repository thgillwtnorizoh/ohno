# 1. Result

Expedition 11 — Ordinary Circle Input Timestamp & First Hit-Window Comparison. 2026-10-28 (Asia/Ho_Chi_Minh). Work 1, original-software investigation only.

## Objective, scope and provenance

Connect one configured ordinary osu!standard keyboard hit action to its first circle timing comparison, preserving polling, queued dispatch, object selection, admission, state and separate clock reads. This is not a reconstruction of input handlers or of the full 300/100/50/miss system. Score/combo/accuracy, effects, slider/spinner input, replay serialization, general UI and full binding configuration are outside scope. No replacement numerical input/judge/clock component or gameplay prototype was built. Work 2 remains suspended; its code and `reconstructed/` were not inspected or changed. No authorised exception.

Read the complete standing contract and supplied attachment, cumulative contexts, relevant Expeditions01–10 and evidence. Prior records are preserved. E05's correction remains authoritative: M113e→M0651→M48de→M48df→M45eb/M45ec constructs the ordinary circle T094c. The E04 Taiko interpretation is not reused. The circle's inherited hit slot differs from the slider-head subclass slot; this report is about the exact ordinary factory product.

Verified requested remote parent `887e58edbfb90a6ec44494bf81c9efc11ee80713`, tree `19cc14e7255e59d8ac22212980934b56807a5e11`. Dedicated remote branch `work1/expedition-11-circle-input` was created directly from it. The local E10 mirror has the identical tree but a different commit identity; publication explicitly uses the requested remote parent. All46 baseline tracked files were checked byte-identical before this expedition's journal append. Final publication hash and remote parent/tree/branch/blob verification are supplied in the final handoff.

| Input | SHA-256 |
|---|---|
| osubackup.zip | 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 |
| osu!.exe | 6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d |
| Instruction.md, repository and supplied attachment | bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9 |

Same self-declared Stable40/file-version1.3.3.8 baseline. Official dated identity/authenticity/modification status remain UNRESOLVED. Rechecked host: Linux x86_64, no Wine/dotnet/DISPLAY or `/dev/dri`. No original executable, original CLR method, native keyboard/audio API, injected original input, Windows instrumentation, screenshot or performance experiment ran. All reference cases below are static predictions, not observed gameplay.

## Evidence, method and actual local checks

Created this report, `tools/circle_input_evidence.py`, `tools/check_circle_input.py`, `evidence/11/circle-input.json`, and an append to `contexts.md`. Read-only extraction reuses unchanged E04/E05 tooling and records76 selected methods, method-body hashes/RVAs/signatures, normalized token/constant/branch/arithmetic sites, ordinary ancestry/declarations/virtual-slot candidates, full direct managed accesses to26 selected fields, direct boundary callers, enum identities and the user32.dll import. No original binaries, assets, source or full disassembly are distributed. Private instruction inspection establishes stack/dataflow interpretation; the public extractor checks original sites without running a substitute input or timing model.

Competing explanations: a keyboard KeyDown callback carrying an event timestamp; polled state plus an event timestamp captured later; polled state plus fresh gameplay-time reads; raw playback position or host time used for judging; candidate/preparation eligibility directly defines a hit; one direct circle lookup versus ordered selection and multiple admission gates. The discriminating investigation follows original producers, delegate payloads, consumers and virtual targets in both directions, retaining gates rather than inventing a direct key→circle shortcut.

Mxxxx/Fxxxx/Txxxx denote EXE MethodDef `0x0600xxxx`, Field `0x0400xxxx`, TypeDef `0x0200xxxx`. All report offsets are **hexadecimal, code-relative, header-excluded**; JSON offsets are decimal. CONFIRMED means static instructions or metadata. Gameplay aliases and pseudocode are RECONSTRUCTED. Original runtime values, scheduling, device latency and physical key-to-audio behavior are UNRESOLVED. Conditional cases require stated state, normally returning helpers and no unmentioned intervening mutation.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/circle_input_evidence.py /path/osubackup.zip > /tmp/input-a.json
python re/tools/circle_input_evidence.py /path/osubackup.zip > /tmp/input-b.json
cmp /tmp/input-a.json /tmp/input-b.json
cmp /tmp/input-a.json re/evidence/11/circle-input.json
python re/tools/check_circle_input.py /tmp/input-a.json
python -m compileall -q re/tools
```

Actual local outcomes: extraction repeated byte-identically and matches public JSON,362667 bytes, SHA-256 `0140c45ff96477a6519401c6b587f219d517e1188a57e745ca107e90572cadc9`. E11 checker PASS for76 methods;61 overlapping historical body-hash comparisons agree. Full EXE scan19659 nonzero-RVA bodies/1739zero-RVA methods/0decode errors; selected branch destinations validate on instruction boundaries. All nine unchanged E02–E10 checkers PASS; Python compilation of all research tools PASS. Baseline preservation, journal prefix, three report headings, scope/diff checks and remote verification are checked for publication. During checker development, an assumed virtual-slot candidate for M28c9 failed: metadata proves it nonvirtual despite a `callvirt` callsite. The assertion was corrected before publication; no historical record was edited.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M1333 | 73f54 | 61 | 88f94b732417c165db08b9efbfa3e78bbb5a07fe598303bd0ea4394b2d5df292 |
| M43a7 | 1b02f0 | 491 | 79daf889ccf8b407f65bb497c41d81aa1e6c19633cf453ac83752df5f8ed926e |
| M2295 | dde78 | 555 | 60d78c3ddacdcb3a428ffebb41762b3eac463a05a9bc2f916c55332154e8b681 |
| M24c7 | e941c | 116 | c3182e7e44e9561db762772ee8e7988118695cbeb4b1819004af14db7f1a2104 |
| M0ad9 | 3d1d4 | 247 | 26cebc0ec147e65ffb53f57e58322149b59d11f1230ce21cfc1707269aa86752 |
| M1884 | 9a828 | 154 | e1ec885ada0f20d49a2ecebf8cf979b8effee6d17d4f0053d4bc80ac22aa9338 |

# 2. What they found

## What this means to an osu! player

For the traced ordinary Z/X route, stable polls the configured key state and queues a shared press action. That action does not carry the moment the physical key went down. When selection and circle evaluation run, they read the corrected gameplay clock F2358 separately. Host-time bookkeeping also exists, but those values are not the operands of the recovered circle timing comparison.

A press still needs a circle to consider: the circle must already be in the manager's candidate list, pass selection flags and time bounds, and be under the mapped cursor. Selection takes the first passing object in list order. Then the ruleset checks ordering and a separate broad admission limit before calling the circle evaluator. A key alone does not directly identify the circle, and being visible does not make a circle hittable.

The circle evaluator marks the object evaluated, reads current gameplay time, subtracts its scheduled start, takes the absolute integer error, and first compares that error **strictly below** a prepared width. At equality it takes the next classification branch. That is the first circle-owned judgement window; the selection interval and the earlier400-unit admission limit are different mechanisms. The rest of the result classification is deliberately deferred.

The fixed conceptual control below is one exact T094c circle with runtime S=2000, pointer equal to its stored F252c, ordinary base Play/non-Edit, plain mods, map difficulty operand5, and a verified configured `OsuLeft=Z` or `OsuRight=X`. The archive has no actual gameplay bindings/configuration. These assignments are controlled assumptions, not recovered installation defaults or a claim that the map was played. E01's existing unvalidated fixture has its first circle at(256,192),S2000 and difficulty5; no new map/parser/gameplay component is required for the proposed Windows test.

## E11-A — Keyboard recognition and the shared press boundary

CONFIRMED native/managed polling chain:

- M43a0 calls M1333 at1d0, stores its returned T0455 keyboard snapshot in F2a64 at1d5, within its keyboard-collection try/catch. A failure can leave the old field; do not assume every attempted pass yields fresh state.
- M1333 allocates256 bytes, calls M1332 at014, ignores its bool return, then tests each byte's bit128 and sets the corresponding T0455 bit through M20c3. ImplMap names M1332 `user32.dll!GetKeyboardState`. No timestamp is returned or stored by this wrapper. Native success, message-pump state age and physical-event latency are unmeasured.
- M43a7 on application Play2 reads binding M2c54 and tests current F2a64 with M20c5→M20c2. The latter selects a32-bit group by key>>5 and tests `1 << (key &31)`. It also ORs a supplementary held-key list through M2e93; previous-state decisions can consult F2a65 and M2e94's previous supplementary list. These supplementary sources remain separate input dependencies, not invented devices.
- For ordinary Osu0, M2c54 maps channel arguments0/1 to dictionary F19f8 key2, and2/3 to key3. Metadata T0b49 names them `Bindings.OsuLeft=2` and `OsuRight=3`; Keys T0a7a names Z90 and X88. A zero binding skips that channel. Actual binding contents are runtime inputs. In ordinary mode, each configured binding participates in two channel calculations; this is not proof of two press callbacks for one aggregate edge.
- M43a6 seeds aggregate button states F2a67/F2a83 from M1187/M1189. M43a7 ORs configured keyboard state into the same logical states, setting left/right to1 on its reached live/unpaused path. Physical mouse wiring M1191 registers M11b3 for MouseDown at03d and M11b2 for MouseUp at053; they pass1/0 to M1188/M118a. The left setter writes latched F0a82 only for1, but always writes raw F0a72; the right pair is F0a83/F0a74. Getters read the latched values. Thus keyboard and mouse converge before press dispatch; a WinForms keyboard KeyDown handler is not required by this particular route.

F2a71 chooses edge policy. On the exact ordinary handler T0253, virtual M0aab inherits the base false return; M0aaf stores it to F2a71 at03d. This is its sole direct managed writer. With false, M43a0's left edge is aggregate state1 **and previous aggregate state differs**; the right edge is analogous. With true, the code instead uses channel-specific new-press predicates from M43a6/M43a7. Preserve the alternative, but do not apply it to the exact ordinary handler without a different writer/receiver. A held key does not create a new ordinary aggregate edge on every update. A keyboard key and mouse button occupying an already-held same logical side need not produce another edge under the false policy.

At M43a0:4b5–55a, left-new F2a6e or right-new F2a8a leads to common new-press bookkeeping, including F0a79=true. If F2c6b!=0, it enqueues a parameterless M11a9 delegate at573 for left-new, and separately at590 for right-new. Two simultaneous sides can enqueue two invocations. No key identity, cursor position or timestamp is captured in either delegate. M11a9 does optional pointer-effect work first, then dispatches numeric event7 at0e1. Its unrelated resource/effect continuation is opaque and must return normally to reach dispatch.

Release is distinct: M43a0 can enqueue M11ac at63e when its aggregate-held/release conditions pass; M11ac emits numeric event6 at001. The ordinary Play registration below attaches M2295 to7, not6. No circle hit from a release-only action is established by this route. Other input/UI consumers are not generalized from this one registration.

The mouse latch is reconciled later, not simply cleared by MouseUp: M43a0 calls M11b4 at7fc, after dispatch/component processing and previous-state writes. If any of its five latched/raw button pairs differ, M11b4 copies raw values back into the latches (left store041, right04b) and returns true. That true result takes M43a0:801→819 toward its bounded repeat test. A press/release arriving between passes can therefore have a latched press considered before a subsequent release pass. This is a conditional static ordering consequence, not a measured guarantee for every short click. The keyboard poll wrapper does not pass an analogous per-key event timestamp.

## E11-B — Registration, priority and Play guards

CONFIRMED: loading work M223b calls M2246 at42a, before work returns and wrapper completion. M2246 registers M2295 for event7 with priority Default2 through M43b6 at023; it separately registers M2296 for event8. Numeric event names remain unresolved because T069d has no recovered named constants. Here “shared press” describes producers and consumers, not a recovered original enum spelling.

M43b6 finds the priority target using M43ca, then calls M2d46 at0b6. M2d46 finds/creates an event list, removes the same delegate, then appends it. M43b4 creates priority targets in reversed enum order; T0b4a identifies Highest8 through Lowest0. M43b5 walks those targets and stops when M2d48 returns true. M2d48 first rejects a disabled target F1a9f, then invokes event-list delegates last-to-first. A true delegate return consumes this dispatch. Higher-priority handlers, disabled targets and more recently registered handlers can therefore prevent Play from receiving event7. Their general UI meanings are outside scope.

M2d48:033–039 allocates a fresh two-element Object array and passes it to Delegate.DynamicInvoke; both entries are null. The actual Play callback receives null sender/null EventArgs. Those arguments are not an event-specific time. `callvirt` is not itself proof of virtual dispatch: M28c8/M28c9 are nonvirtual metadata methods, while their object predicate and ruleset/manager/evaluation continuations below use resolved virtual slots.

M2295's exact early guard projection, in original order:

| Offsets | Rejection condition / required state |
|---|---|
| 000–008 | F2c6b==0 returns false; E02 identifies this as transition-phase state, not an OS focus timestamp |
| 009–02d | Reject F136a; then reject F1373 && !M4390(); then F13be; then F1376 |
| 02e–045 | Require F0a79 OR F137b OR ruleset M2232()==Taiko1 |
| 046–059 | If F137b and sender==null and ruleset!=Taiko1, reject |
| 05a–062 | Reject F1396 |

For the ordinary live control, M4390()==false, F136a/F1373/F13be/F1376/F137b/F1396 are false and F0a79 is the new-press flag. M2262 toggles F1373 at0c7 on its reached pause-related branch and later interacts with the E10 clock controls. Thus the live paused flag directly rejects this callback; no assumption that a frozen clock alone prevents a hit is needed. F13be/F1376/F1396 and other flags retain token names rather than guessed complete state-machine meanings. M2237 initializes these gates; other writer sites remain in the field inventory where watched. No complete pause/focus/resume UI model is claimed.

M2237 derives F137b from Relax128, and F137c from Relax2/Autopilot8192, when not M4390. A Relax-enabled ordinary keyboard callback with null sender can be rejected at058; that does not recover Relax's automatic input route. F137c changes the direct selection radius to100 and the later admission limit to200. Special Play/recorded-input state can differ; these flags are not assumed immutable aliases in every path.

M2295 returns true at229 even if selection found no object or a later step did not judge it. That bool means input dispatch was handled; it is not proof of a successful hit or of any particular result.

## E11-C — Real selection: candidate list, time bounds, then space

CONFIRMED exact ordinary selection route:

`M2295 → M28c8 → M28c9 → virtual M24c7`

M2295 obtains a mapped pointer from F2a5c through M0ff1 when !M4390, otherwise F2a7b (063–080). Screen-to-playfield conversion is retained as an opaque original helper; no invented screen coordinate is substituted. Ordinary F137c=false calls M28c8 at0bb with argument `true`. M28c8 forwards that boolean and prepared manager radius F0ada to M28c9. F137c=true instead calls M28c9 directly with radius100. Exact radius construction is outside the timing question; reference cases use a valid radius and the actual object position.

M28c9 enumerates the **existing** F17fd list in list order. In live !M4390 with the supplied true flag, F153b=false skips the object at034 before its predicate. Fresh ordinary constructor M249b writes F153b=true at01f. First predicate success returns immediately; no closest-object sort or direct lookup is introduced.

T094c inherits M24c7 unchanged. Because the input argument is true, branch001 goes directly to00b, **bypassing** virtual M24b6. The E07 circle eligibility method M45fc is therefore not called by this reached selection. At00b–043 the input predicate performs:

1. Read S=F2523 and manager A=F0adb; compute wrap32(S−A); read F2358 at01d. If lower>T, return false (`bgt.s` at022).
2. Read S again, read manager Wwide=F0adc; compute wrap32(S+Wwide); separately read F2358 at036. If upper<T, return false (`blt.s` at03b).
3. Read F1527. If true, return false.
4. Compute DistanceSquared(pointer,F252c) and compare to radius*radius. Ordered `<=` succeeds, including equality (`ble.s` at054). With input flag true, a failed primary position test returns false; it does not take the alternate position fallback.

Finite, no-overflow, unchanged-T projection: **S−A <= T <= S+Wwide**, plus unjudged and spatial checks. The early side is approach-related, not a symmetric timing window. The upper side uses a prepared width, not E07's end+240 presentation eligibility. Candidate membership can outlive either predicate, and visibility is not tested here. Float NaN distance/radius does not take the ordered spatial success branch on this true-argument path.

Ordinary loaded circle S is runtime F2523, initially also F2524 (E04, preserved after E05's ruleset correction). Do not reapply a parser/map shift to this start inside the recovered input comparison. This expedition does not establish equivalence of all parsed offsets and live clock offsets.

## E11-D — The first action-admission timing test

CONFIRMED virtual resolution: M2295:0e0 invokes ruleset slot M0ad9. Ordinary handler T0253 extends T0186 and does not override it, so the base M0ad9 executes. The Taiko override is not used. It first handles object-order dependencies:

- For a Normal-bit object, find its index in F17fd. If index>0 and predecessor.F252d>0, predecessor.virtual M24b6() true and predecessor.F1527 false, return0. One first circle at index0 skips this predecessor gate. The meaning of F252d is not invented.
- Enumerate candidates, skipping F1527=true. Reaching the selected reference stops the earlier-object scan. An earlier unevaluated object with wrap32(end+3)<selected.start returns1 (`bge.s` at0a8 skips this rejection at equality). This is an object-order comparison, not an input timestamp comparison. Do not remove it when generalizing beyond the single-circle control.

Only then set marginAdjust=200 when F137c else0 and compute:

`Abs(wrap32(selected.F2523 − F2358)) < (400 − marginAdjust)`

The gameplay-time read is at0dd; subtraction0e2; Abs(int)0e3; `blt.s`0ef. Strict success returns2 at0f3; equality or larger returns1 at0f1. This is a separate broad action-admission threshold. It is not the prepared first circle width and does not assign a300/100/50 outcome.

M2295 stores the returned int, switches at0e9:0 skips,1 invokes M24c8 feedback at100,2 takes the evaluation branch at10a. The feedback implementation and result effects are outside scope. On2, Normal-bit check M3e4e(1) passes for the ordinary circle and M28d3 is called at11a. No event timestamp or signed timing error is passed into this method; only the selected object reference is supplied.

## E11-E — First circle-owned timing comparison and width preparation

CONFIRMED virtual resolution and mutation order:

`M2295:11a → manager M28d3:012 → virtual M24a9 → T035a.M1884`

The exact factory-created T094c extends T035a→T04c3→T0828; it inherits T035a's reuse-slot M1884. This is not the slider-head T06f6 override. M28d3 rechecks selected.F1527 and returns0 if already true. Otherwise it stores manager F17ff=selected at00c, then calls M24a9 at012. These operations precede evaluation.

M1884 first writes **F1527=true at002**, before reading any time. It then reads **F2358 at007**, selected start **F2523 at00d**, subtracts at012 and calls `System.Math.Abs(int32)` at013. The first window test loads manager **F0ade at020** and branches `bge.s 034` at025. Hence:

`Abs(wrap32(T − S)) < Wfirst`

Strict success stores raw result token value1024 to object F152a at02d; the common continuation ultimately returns the object's result to M28d3. Equality goes to034, where further original classification continues. The complete result mapping and post-result sound/animation/score mutations are deliberately not reconstructed here. In particular, failure of this **first** window is not automatically “miss” or “ignored input.” The already-evaluated flag has been written even when this first comparison fails.

Timing-source conclusion, **CONFIRMED static dataflow**: the selection, broad admission and circle test each directly load F2358. These methods do not read F235b (raw rounded time), F2359 (copy), F235c (offset), native playback position or an event-time argument for those comparisons. They do not call M3b24 to refresh the clock at input receipt. E10's clock processing supplies T earlier; no second local offset or rate multiplication occurs in the recovered comparison. This is a bounded proof for this route, not proof every input/judgement routine uses the same field.

Two time-like bookkeeping writes must not be confused with this operand: M43a0 writes host F2c3a→F0a77 at554 during common press bookkeeping; selected M2295 writes F2c3a→Play.F13df at0d4. Neither is forwarded into M0ad9 or M1884. Separately, E08's draw-time M4736 selects F2358 for circle visuals later. Same field identity does not establish equal samples or identical physical-event timing.

Wfirst is **prepared manager state**, not a formula evaluated per key press. M28be during loading configuration (M28f5:07f, E05/E07) loads map float F0b55, conv.r8, supplies anchors80/50/20 to virtual M125a at0f4, conv.i4 at0f9, then stores F0ade at0fa. Other configuration paths/writers exist; the input test reads the current stored width. It does not snapshot it at circle construction.

Ordinary manager T00e8→T0556→T0279 resolves M125a to M28b3. The latter first calls M28b0(D,1.4,manager.F17ef). Outside Edit, Easy2 applies `Max(0,D/2)` first; HardRock16 then applies `Min(10,D*1.4)`. In Edit neither applies. The original piecewise mapping is:

| Modified difficulty d | Mapping of anchors a,b,c |
|---|---|
| ordered d>5 | b+(c−b)*(d−5)/5 |
| ordered d<5 | b−(b−a)*(5−d)/5 |
| otherwise, including unordered d | b |

For Wfirst, a80,b50,c20; then direct original conv.i4, no Math.Round. For ordinary finite d0..10 the ideal algebra is80−6d; use the original operations and conversion at boundaries. F0b55's “OD-like” name follows its use; this expedition did not execute the string decoder to prove its parser/UI name independently. The supplied E01 fixture declares OverallDifficulty5, but matching its runtime field remains a Windows validation step.

For the selection's Wwide, M28be independently uses anchors200/150/100 and stores F0adc at092, through the same virtual mapping. A is separately prepared from F0b52 using1800/1200/450 anchors, as E07 established. Thus, conditional on plain modified operands5, Wfirst50, Wwide150 and A1200. Other widths are written by M28be but their outcome mapping is outside this first-comparison expedition. DT/HT percentage is not read or multiplied locally by these comparisons; E10 already establishes that playback rate can affect T's production. Do not translate this directly into a measured wall-clock window.

All time subtraction/addition here is original unchecked int32 arithmetic. Abs(int.MinValue), target CLR exceptions and non-finite/out-of-range conversion behavior were not probed in the original runtime; no safe fallback/saturation is invented.

## E11-F — Ordering, readiness and exact interface

CONFIRMED reached managed chain, refining E10:

| Order | Sites | State effect / timing use |
|---:|---|---|
| 1 | M469e→M43a0:0ca→M3b25:048 | Original clock processing occurs first |
| 2 | M43a0:1d0/1d5, then23e and248 on reached branch | Capture keyboard snapshot, recognize configured channels, then process other keyboard consumers |
| 3 | M43a0:573/590 | Queue shared press delegates |
| 4 | M43a0:773–78d | Drain queued delegates; M11a9→priority dispatch→Play→selection/admission/evaluation |
| 5 | M43a0:790→M46d7→M46fb | Invoke component-update delegate after draining queue |
| 6 | M226b:959, when gates permit | Refresh candidate F17fd, then separately gated object updates at9cf |
| 7 | M43a0:79a/7a4/7e6, then7fc | Previous aggregate/key writes, then mouse-latch reconciliation and possible bounded repeat |
| 8 | E10 host draw path after update returns | Presentation admission and visual clock evaluation later |

Exact keyboard sub-order from original M43a0: M43a7 at23e precedes M43a2 at248. M43a2 processes other keyboard consumers after configured-channel recognition but before press synthesis/queue drain. Those helpers can have side effects; no immutable snapshot of application state is promised. The snapshot is captured at1d5; F2a65 is updated from current state only later at7e6, after component callback. F2a6c/F2a88 previous aggregate states are likewise written after callback at79a/7a4.

Consequently input selection uses the candidate list left by an **earlier refresh**, while its time comparisons read the newly published T. On E10's bounded inner loop, a preceding pass in the same host callback can have supplied that list. It is not necessarily a whole video frame old, and there is no measured fixed latency. First-circle tests cannot assume current T automatically refreshes candidate membership before input.

Readiness is layered. M223b can register the input callback before wrapper completion; M2295/M28c9/M24c7/M0ad9/M1884 do not directly test F13d8. E07's M2269 returns before candidate refresh while loading gates fail. With a fresh ordinary manager's initially empty F17fd and no alternative refresh, an input before its first refresh finds no object, even if callbacks are already registered. F13d8=false alone is not a universal rejection predicate for artificially populated/stale lists. Loading-time input retention, actual registration-thread scheduling and all transition interleavings remain UNRESOLVED.

Global input processing in M43a0 also requires F2a79 and, on its ordinary live path, host activity predicate M4235. When those gates fail, new polling/press synthesis can be skipped, but previously queued callbacks still drain later; no blanket queue cancellation is inferred. M2295's transition/pause/state guards and priority-target enable guard are additional barriers. Full host-focus policy is not recovered beyond these original predicates.

Interface for another investigator:

- Inputs: configured Keys enum, current/previous keyboard and supplementary held states, stored physical buttons, prior logical aggregates, F2a71 edge policy, input queue/priority target state, Play guards, mapped pointer, actual candidate list/order, selected runtime circle/manager, F2358 and current prepared A/Wfirst/Wwide/radius.
- State: key snapshots F2a64/F2a65; edge flags F2a6e/F2a8a; new-press F0a79; candidate F17fd; selection F153b; evaluated F1527; selected manager ref F17ff; raw result F152a. The first successful strict-window branch writes1024; downstream handling remains deferred.
- Time dependencies: clock publication before polling and queue consumption; distinct selection lower/upper T reads; separate admission T read; later fresh circle T read; host bookkeeping values are separate. No action timestamp passed by the traced delegate.
- Outputs: zero or more queued press calls, handler-consumed bool, selected object or null, admission int0/1/2, evaluated flag and first-window raw-result branch. These are different outputs; none is silently substituted for a hit result.

## E11-G — Focused static reference predictions

RECONSTRUCTED conditional cases, not original-game or surrogate test results. Default control: exact T094c, first/only candidate, S2000, F153b=true, F1527=false, pointer equal F252c, valid radius, ordinary live/no mods, permitted input gates/priority, fixed T through one callback, d5→A1200/Wwide150/Wfirst50. Values are gameplay-time operands, not measured physical key receipt times. Fresh circle state is reset between cases; later classification is explicitly outside the projection.

| Case | Controlled input/state | Expected reached branch |
|---|---|---|
| I01 | OsuLeft binding Z90, previous aggregate0, current down | New left aggregate edge can enqueue M11a9; on permitted dispatch proceeds through selection |
| I02 | OsuRight binding X88, previous right0, current down | Right path enqueues the same M11a9; no timestamp/key identity is carried |
| I03 | Same key remains held, ordinary F2a71=false | No new aggregate edge on later unchanged polls; no automatic repeat press from that state alone |
| I04 | Release-only action | Distinct release path/event6; not M2295's event7 registration |
| I05 | Physical left mouse down through M11b3, same logical control | Stored button state joins M43a6 and the same M11a9/event7 boundary |
| I06 | Left side already held by mouse; add left bound key, edge policyfalse | No aggregate transition; another same-side press is not guaranteed |
| I07 | Both sides newly down | Two delegates may be queued in left-then-right order; first evaluation can make the second find no unjudged circle |
| I08 | Higher-priority consumer returns true, or Default target disabled | Play callback not reached by this dispatch |
| I09 | F2c6b0, or live paused F1373true, or F1396true | Corresponding M2295 early false return; circle comparison not reached |
| I10 | Fresh loading manager before first candidate refresh | No object selected from empty F17fd, even if callback already registered; true callback return is not a hit |
| I11 | Artificial F13d8false with nonempty candidate and all input gates passing | This flag alone does not block the recovered input methods; not asserted naturally reachable |
| I12 | Circle registered in F17fb but absent from F17fd | Not considered by M28c9, regardless of pointer/time |
| I13 | F153bfalse in live true-argument selection | Skip before predicate/time comparison |
| I14 | F1527true | Selection rejects after time bounds; M28d3 also has a recheck before evaluation |
| I15 | T799, below S−A800 | Reject lower selection bound, no spatial/admission/evaluation continuation |
| I16 | T800 exactly S−A | Lower equality passes; broad admission Abs1200>=400 returns1; circle evaluator not reached |
| I17 | T1600, Abs(S−T)==400 | Selection can pass, strict admission equality returns1, not2 |
| I18 | T1601, Abs399 | Admission2 can reach M1884; first-window comparison fails; F1527 was already set true. Do not infer ignored input |
| I19 | T1950, error50 | First strict-window equality branches034; later classification deferred |
| I20 | T1951, error49 | Earliest integer T passing this first window under the fixed control; raw first-branch result1024 |
| I21 | T2000 exactly S | Error0<50 passes this first window |
| I22 | T2049, error49 | Last integer T passing this first window under the fixed control |
| I23 | T2050, error50 | First-window equality falls through; not proof of a miss or rejected press |
| I24 | T2150 exactly S+Wwide | Late selection equality passes; admission can pass, first window fails; wider outcome deferred |
| I25 | T2151, beyond S+Wwide | Late selection rejects before distance/admission/evaluation |
| I26 | DistanceSquared==radius² versus greater | Ordered equality selects; greater fails this true-argument circle selection even at T=S |
| I27 | Ordinary width operand d0/d5/d10, otherwise matched state | Prepared Wfirst80/50/20, strict integer ranges S±79/±49/±19 respectively |
| I28 | Base operand5 with Easy2 or HR16 at configuration | Modified d2.5→Wfirst65 or d7→38; no per-key recomputation |
| I29 | Relax128 live/null sender ordinary route | F137b gate can reject queued keyboard callback; automatic Relax path not reconstructed |
| I30 | Autopilot-derived F137ctrue | Selection radius100 and admission strict200 replace ordinary radius/400; threshold equality still fails admission |
| I31 | Clock correction/seek before delegate execution changes T from1950 to1951 | With candidate/gates retained, comparison uses1951, not any captured1950 physical-event time |
| I32 | Artificial T differs between admission and M1884, or stale host bookkeeping | Circle reads later T at007; F0a77/F13df/F2359 are not substituted. Actual interleaving must be measured |
| I33 | Earlier unjudged candidate end+3 < selectedS versus equality | Admission scan returns1 for strict gap; equality alone does not take that branch |
| I34 | Same selected circle, clock/render samples differ | Input and visuals may read the same field at different moments; no equality/first-pixel claim follows |

The “earliest/last accepted” values in I20/I22 refer **only to the first recovered circle window**, not the full positive-judgement range. Overall 300/100/50/miss acceptance and late-update expiration are not recovered as a complete mechanism here. Their precise continuation is M1884:034 onward and manager M28d3's subsequent handling. The inclusive selection boundary atS+Wwide must not be promoted to an accepted hit boundary. This distinction is part of the handoff, not an implementation assumption.

## E11-H — Remaining questions and minimal original-Windows experiment

UNRESOLVED: physical-device/key-message time versus snapshot availability; native polling success and thread/message-pump cadence; actual bindings and input provider per run; supplementary lists; original scheduling/concurrent mutation; complete focus, loading and pause/resume policies; all hotkey/priority side effects; target-CLR edge behavior; full result classification and downstream score/effects. E09's projection dependency remains deferred. Owner-reported distorted-slider stutter remains an unmeasured cause hypothesis; no performance or optimization conclusion follows from this input investigation.

Smallest practical pinned-Windows experiment, not executed:

1. Use a copied hashed installation. Generate the unchanged E01 fixture in a fresh directory and verify its map hash `dc4112acfcb0e7f345c6350a0cf1640d74f7acdc6eb2ae77047023b09e330541` and WAV hash `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977`. Target its first centered circle only; log actual runtime S/F252c, manager widths/radius, candidate index and resolved exact T094c/T0253 types. Record actual bindings Z/X, plain mods, Windows/CLR/process architecture, focus, input mode, offsets, audio/display/limiter settings and changed installation hashes. The fixture is still unvalidated for stable import.
2. Instrument the original process at M43a0 clock-return/poll/queue boundaries, M1333/M1332, M11a9, M43b5/M2d48, M2295, M24c7, M0ad9 and M1884. Resolve CIL tokens to JIT addresses; CIL offsets are not native addresses. Record ordered sequence, thread ID, independent monotonic timestamp, key bits and prior aggregate, binding/rate/offset state, T/F2359/host times, selected identity, gates and comparison operands/results. Hook/log original reads rather than making extra clock refresh or keyboard reads.
3. Record an independently timestamped physical or injected key-down and key-up receipt, first original poll that recognizes it, enqueue, callback and each consumed T. An instrumentation/OS timestamp is a measurement reference, not a newly discovered original judgement timestamp. First run one ordinary press near S, then a release-only and a held-key repeat control. Compare mouse entry under the same cursor/state only after verifying the shared route actually executes.
4. Test inputs around the measured first width boundary, classifying by original T−S, not intended wall-clock injection delay. Collect exact equality and one-integer-inside/outside cases when possible. Any debugger-forced T/state probe is labelled artificial and separated from natural playback. Repeat one pause/resume or controlled timing discontinuity, retaining candidate membership logs; determine whether publication, dispatch or selection changed, rather than attributing all differences to the key timestamp.
5. Preserve logs and uninstrumented/instrumented cadence controls. No audible synchronization, pixel timing or frame-cost claim follows from those comparison traces alone. Silent fixture audio cannot validate heard-audio alignment; E10's separate audio-backed trace remains needed for that question.

Smallest next static gameplay investigation: continue **one ordinary circle from the first-window fall-through through the remaining result classification and its evaluated/expired state changes**, stopping at the first manager result handoff. That would resolve the currently deferred positive/miss boundaries and test the relationship between input evaluation and later update expiration, without broadening into score/accuracy/combo or effects. Work 2 remains suspended until explicit product authorization.

# 3. Pseudocode

RECONSTRUCTED independent documentation, grounded in CONFIRMED original instructions. No executable input or judge model is supplied. Opaque helpers retain their original identity/side effects. Aliases denote fresh field reads where used; do not hoist time reads into a shared snapshot. `i32` means original unchecked int32 arithmetic. Native input/clock values are external observations to collect, not guessed replacements. Wider classification and effects are deliberately unresolved continuations.

## Polling, shared dispatch and consumer order

```text
reached ordinary live pass of M43a0:
    M3b25()                                  // includes E10 clock publication
    ... original gates/provider/keyboard collection ...
    F2a64 = M1333()                           // GetKeyboardState -> bit snapshot
    M43a7()                                  // configured bindings/channels
    M43a2(...)                               // other keyboard processing
    ... original aggregate/edge computations ...
    if left_new or right_new:
        record original host-time bookkeeping
        F0a79 = true
        if F2c6b != 0:
            if left_new: queue(parameterless M11a9)
            if right_new: queue(parameterless M11a9)
    ... distinct release callbacks may also be queued ...
    while queue not empty: dequeue().Invoke()
        M11a9:
            ... optional original pointer-effect work ...
            M43b5(event7):
                for target in original priority order:
                    if target.M2d48(event7): break
                        if !target.F1a9f: return false
                        for delegate in matching_list from last to first:
                            if delegate.DynamicInvoke([null,null]): return true
                        return false
    component_update_delegate()               // later reached Play candidate refresh
    ... write previous button/key snapshots ...
    M11b4() reconciles raw/latched mouse states
    ... original bounded repeat test may restart the clock/input pass ...

M1333_snapshot():
    state = default T0455
    bytes = new byte[256]
    M1332_GetKeyboardState(bytes)              // bool return ignored
    for k=0..255:
        if (bytes[k] &128)!=0: state.M20c3(k)
    return state                              // no timestamp field added

ordinary configured-channel observation in M43a7:
    key = M2c54(channel)                       // OsuLeft/Right binding values
    if key != 0:
        held = F2a64.contains_bit(key) or M2e93(key)
        // Channel edge logic depends on F2a71 and original previous-state tests.
        // Ordinary handler sets F2a71=false via M0aab/M0aaf.
        ... update original channel fields ...
        if held and live permitted branch:
            corresponding aggregate button = 1
```

## Play selection and broad admission

```text
M2295(sender,args):
    if F2c6b==0: return false
    if F136a: return false
    if F1373 and !M4390(): return false
    if F13be: return false
    if F1376: return false
    if !(F0a79 or F137b or M2232()==1): return false
    if F137b and sender==null and M2232()!=1: return false
    if F1396: return false
    p = M0ff1(F2a5c) if !M4390() else F2a7b
    selected = M28c9(p.x,p.y,true,100f) if F137c else M28c8(p.x,p.y,true)
    if selected != null:
        ... original activity bookkeeping ...
        decision = ruleset.virtual M0ad9(selected)
        if decision==1: selected.virtual M24c8()   // feedback continuation outside scope
        else if decision==2 and selected.has_type_bit(1):
            manager.virtual M28d3(selected)
        ... other object/result continuations outside circle projection ...
    ... optional callback ...
    return true                                  // handled, not necessarily hit

M28c9(x,y,inputFlag,radius):
    pointer = original_vector(x,y)
    require_selection_flag = !M4390() and inputFlag
    for obj in existing F17fd list order:
        if require_selection_flag and !obj.F153b: continue
        if obj.virtual M24c7(pointer,inputFlag,radius): return obj
    return null

M24c7 ordinary circle, reached with inputFlag=true:
    // M24b6 is bypassed for this argument; do not substitute E07 visibility eligibility.
    if i32(S-manager.A) > F2358: return false
    if i32(S+manager.Wwide) < F2358: return false   // another time read
    if F1527: return false
    return ordered(DistanceSquared(pointer,F252c) <= radius*radius)

M0ad9 ordinary inherited ruleset admission(selected):
    if selected.has_type_bit(1):
        k = F17fd.IndexOf(selected)
        if k>0 and predecessor.F252d>0 and predecessor.M24b6() and !predecessor.F1527:
            return 0
    for obj in F17fd list order:
        if obj.F1527: continue
        if obj is selected: break
        if i32(obj.end+3) < selected.start: return 1
    adjust = 200 if F137c else 0
    error = Abs(i32(selected.start - F2358))       // fresh read; no event timestamp
    if error < 400-adjust: return 2
    return 1
```

## First circle window and prepared width

```text
M28d3 prefix(selected):
    if selected.F1527: return 0
    F17ff = selected
    result = selected.virtual M24a9()             // exact T094c -> M1884
    ... original manager continuation, not reconstructed here ...

M1884 first-window projection:
    F1527 = true                                 // before time read and comparison
    error = Abs(i32(F2358 - F2523))
    if error < manager.F0ade:
        F152a = 1024                              // original raw first-branch value
        continue_original_common_path()
    else:
        continue_original_classification_at_034() // equality also goes here
    // Full outcomes, effects and manager accounting are outside this projection.

M28be first-width preparation projection:
    D = double(map.F0b55)
    Wfirst = conv_i4(manager.virtual M125a(D,80.0,50.0,20.0))
    manager.F0ade = Wfirst

ordinary M125a -> M28b3(D,a,b,c):
    d = M28b0(D,1.4,manager.mods)
    if ordered(d>5): return b+(c-b)*(d-5)/5
    if ordered(d<5): return b-(b-a)*(5-d)/5
    return b
M28b0(D,hardRockFactor,mods):
    if mode!=Edit1 and Easy2: D=Max(0.0,D/2.0)
    if mode!=Edit1 and HardRock16: D=Min(10.0,D*hardRockFactor)
    return D

UNRESOLVED / deliberately deferred:
    actual original event/poll/dispatch latency and runtime interleavings;
    complete pause/focus/loading/binding-provider policies;
    wider circle result classification and update-driven expiration;
    target CLR exceptional arithmetic and actual visible/audio output.
```
