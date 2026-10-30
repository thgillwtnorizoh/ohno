# 1. Result

For an osu! player, pressing pause does not freeze one universal game state. The recovered ordinary path first records a request, later toggles the Play pause flag, and separately asks the original clock/playback machinery to stop. Resume clears that flag before a further handoff gate permits hit input and object updates again. The host can continue publishing time, polling input, updating presentation state and drawing while ordinary object simulation is suppressed.

This is **RECONSTRUCTED player-facing meaning from CONFIRMED original static instructions**, not a Windows observation. No pause duration, audible state, visible menu, latency or frame cadence was measured. Work 2 remains suspended. `reconstructed/` was neither inspected nor modified. No authorised exception.

## Objective, preparation and provenance

E28 follows ordinary base Play T047b, ordinary handler T0253 and manager T00e8 from control registration through pending requests, pause/resume/recovery gates, clocks, object attention and E27 finalization. It stops at existing presentation/action/request boundaries. It does not build a pause system, clock, scheduler, results transition or gameplay implementation; complete menus, retry loading, score/HP/audio/rendering systems and lifecycle recovery remain outside scope.

Read the complete standing contract and cumulative record through E27, relevant E02/E03, E10–E12, E17/E19/E20/E27 reports/evidence, and necessary E07/E15/E24–E26 dispatch/state continuations. Preserve E05's ordinary/Taiko correction, E12's post-refresh rejoin, E14's resource ordering and E24's ordinary-spinner correction. Old reports and journal entries are historical, not edited to incorporate this investigation.

Exact requested parent **c06689fa168b25e07daa7f0a4a1be1062d719e85**, tree **c4c0b09f2a7fa5a8c420ae00626f996708c5ec51**. Dedicated branch **work1/expedition-28-gameplay-pause-resume** was created directly from that remote commit. The local mirror has an identical tree and different commit identity. All 129 baseline blobs/modes matched remote. Preserve the other 128 files and exact **273879-byte** old journal prefix. Publication uses the exact requested remote parent; final handoff supplies verified publication identity.

**E20 remains preparation-only and incomplete. All 17 original-runtime validation cases are NOT RUN — WINDOWS HOST REQUIRED. All 48 E28 runtime cases are also NOT RUN.** A future owner-authorized Windows PC is not an already verified host or an automatically available CLR operand tracer. The E20 packet is unchanged.

## Inputs, evidence and reproduction

| Input | Actual SHA-256 / status |
|---|---|
| osubackup.zip | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; 18 entries, CRC valid |
| Archive osu!.exe | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Repository / supplied standing contract | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |
| Existing E20 circle | `682d600f7a76436c9ec17f1b2ccf52afc968ebf5ef81c3e74b282161c5d0aa0d` |
| Existing E16 slider | `f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84` |
| Existing E24 spinner | `85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c` |

Reference bytes were rehashed before analysis. Self-declared Stable40/file-version1.3.3.8; exact official dated identity/authenticity/modification remains UNRESOLVED. Actual host: Linux x86_64, no wine/dotnet/pwsh/powershell/DISPLAY/WAYLAND or supplied authorised Windows endpoint. Existing dnfile0.18.0/dncil1.0.2/pefile2024.8.26 decoded bytes only. No original EXE, CLR, native import, game input, fixture import or surrogate clock/gameplay engine executed.

New evidence `re/evidence/28/pause-resume.json` is a deterministic sparse original-instruction inventory, **722586 bytes**, SHA-256 **`2bedeb9e65c2b4573a5aaace12ba75c40fdea86f5e5ff485c01ccb0020574af7`**. New extractor/checker: `re/tools/pause_resume_evidence.py` and `re/tools/check_pause_resume.py`. Reuse unchanged E04/E05/E15/E25 collectors. There are 119 selected method entries, 14 focused-writer body identities, full selected-field/caller inventories, ancestry/slots, field signatures/flags, native imports and original binding/key metadata. Full scan: 19659 decoded bodies, 1739 zero-RVA methods, zero decode errors. No proprietary executable/assets, full source/disassembly, memory dump or runtime trace is published.

Mxxxx/Fxxxx/Txxxx abbreviate original MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx. All report offsets are hexadecimal, relative to the first CIL opcode, excluding method headers. JSON offsets are decimal. They are not native/JIT addresses. Direct inventories exclude reflection, external/native writes and arbitrary transitive callback effects.

Competing explanations: immediate pause versus deferred consumption; one paused flag versus separate resume/recovery gates; stopped Play update versus stopped host time; source pause versus constant corrected clock; clock state0 versus Play pause; resume rebuild versus retained objects; focus loss versus a guarded callback; finalized record versus a new pause. Privately inspect original instructions, stack flow, signatures, callers and virtual slots. Preserve side effects and exceptions rather than treating unknown helpers as pure.

```sh
python re/tools/pause_resume_evidence.py /private/osubackup.zip > /private/pause-a.json
python re/tools/pause_resume_evidence.py /private/osubackup.zip > /private/pause-b.json
cmp /private/pause-a.json /private/pause-b.json
cmp /private/pause-a.json re/evidence/28/pause-resume.json
python re/tools/check_pause_resume.py re/evidence/28/pause-resume.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

Static verification for publication comprises fresh extraction equality, focused checks/210 historical body-hash comparisons, all 25 prior research checkers, unchanged E20 safeguards, Python compilation, historical/blob/mode/fixture/journal-prefix checks, scoped diff and remote parent/tree/branch/all-file verification. These checks do not satisfy E20's runtime acceptance. A draft checker confused the F05f5 load at M226b:0ade with the M2964 call at0ae3; corrected to the original callsite before PASS. Private inspection-file lookups that were missing were regenerated/read from original evidence, not labelled game failures or runtime observations.

# 2. What they found

## A. Three distinct interruption states and one pending request

**CONFIRMED metadata/stores; descriptive aliases RECONSTRUCTED:**

| Field / token | Original type / owner | Relevant role |
|---|---|---|
| F1373 /04001373 | static Boolean, T047b | Main Play pause toggle |
| F13be /040013be | static Boolean, T047b | Separate resume-handoff gate |
| F1376 /04001376 | static Boolean, T047b | Recovery-related suspension gate; not a universal object-update freeze |
| F13d6 /040013d6 | Boolean, T047b instance | Pending control request |
| F13d7 /040013d7 | Int64, T047b instance | Request-time value converted from gameplay Int32 |
| F1390 /04001390 | Int32, T047b instance | Gameplay-time value stored on entering pause |
| F13bd /040013bd | Boolean, T047b instance | Resume continuation permission |
| F13ed /040013ed | Int32, T047b instance | Original control-selection state; reset to−1 during toggle |
| F13ee /040013ee | List<T092e>, T047b | Resume/control presentation references; not hit objects |
| F2985 /04002985 | Boolean, host T08c1 | Stored host-focus predicate, separate from Play pause |
| F1397 /04001397 | Boolean, T047b instance | E27 finalization latch |

F1373, F13be and F1376 have different writers and gates. They are neither one Boolean nor a transaction with the clock. F2358, previous F235a, raw-rounded F235b, offset F235c, working Double F2352, synthetic F234c, hold F2382/F2353 and clock state F2368 also retain their separate E10 identities.

## B. Recognized keyboard/control entry and actual receiver

Loading registration **M2246:002a–0035** obtains `this.virtual M225b` through ldvirtftn, constructs T074e and registers it using M3794. Ordinary base T047b resolves that slot to **M225b/0600225b**, signature `2002021c11a9e8` (instance Boolean(Object,Keys)). M2248 unregisters it through M3795. Special T00ff overrides M071b and calls base; do not use its direct F1373 toggle as the ordinary base path.

For the reached keyboard route:

1. **M43a0** processes clock M3b25 at00ca, polls keyboard through M1333 at01d0, and calls **M43a2 at0248**. Polling/host guards and native errors remain E11 dependencies.
2. M43a2 compares current key collection against previous F2a75. A new key can reach **M379a at03d5** after original UI/text/control handlers and M1ffb-related gates. Releases call **M379b at00f0**, which uses separate F2077, not Play's F2076 registration. Held-repeat work invokes M379c with false at057a; it is not an unconditional repeated call to M225b.
3. M379a first gives M379c(key,true) a chance to consume it; then walks the F2076 delegate snapshot last-to-first, subject to its original modifier-order reversal. **DynamicInvoke at006b** supplies null sender and the actual boxed key. A true result consumes dispatch. Earlier handlers can suppress Play.
4. M225b calls **M2c57→M2c50(key,0)**. Original `osu.Input.Bindings` metadata names **Pause18**, **Skip19**, **QuickRetry23**. `Keys.Escape=27` is original metadata. Actual configured Pause key is runtime state; Escape27 is also directly recognized. Do not claim a configured default.

This keyboard control dispatch is synchronous within M43a2. It precedes M43a0's queued shared gameplay-press dispatch at0773–078d and component delegate at0790. It does **not** carry a physical key timestamp. An additional original pointer-button-provider route **M11a4:004a** dispatches key27 through M379a before M3b25 on M43a0's prefix; retain that separate ordering without expanding device configuration.

### Pause/Escape branch priority in M225b

Require transition phase **F2c6b==2** (`0000–0009`). If F13c9.F0344 is false, Pause18/Escape27 takes virtual M225a and returns its Boolean; this is a loading/not-ready departure route, not established live pause.

With that flag true, store F13c6=true first. Pause18 or Escape27 reaches:

- **F1397 true → M2285:004e**, then returntrue: finalized-session departure before any pause toggle.
- Otherwise ordinary base, no special T04af, !M4390 and !F136a → **virtual M2261:00c2**.
- Recorded-input/failed/special branches differ. Reached F2c73 can request Edit; recorded-input work and F13bb=7 precede further handling; an already paused reached alternative can request Edit1/SelectPlay5; another branch writes F2366=300. These are not ordinary universal pause behavior.

Original helpers may interrupt before return. The Boolean means control handled, not that audio or objects have already stopped.

## C. Pending request, consumption and exact first pause write

**M2261/06002261** does not toggle pause or call playback:

- If F13d6 already true, return001b without refreshing request time.
- Otherwise **000a stores F13d6=true**; fresh F2358 at0010, **conv.i8 at0015**, **0016 stores F13d7**. The two writes are separate.

**M2262/06002262** consumes it only when reached by Play or another original caller:

| Offsets | Original ordered operation |
|---|---|
| `0000–0024` | If !F1373 and instance F1370 true, return while **Int64(fresh T)−F13d7 <1000**. Equality passes. This is gameplay-time arithmetic, not a guaranteed host delay. |
| `0025–0030` | Return if no pending request; otherwise **clear F13d6=false** before the following vetoes. |
| `0035–0053` | Require F13b2, and not(failed && already paused), and F13ef. Otherwise return with request already cleared. |
| `0054–00af` | On !synthetic,!F2c73,!failed,!recorded,!F13be,!paused,!F1b97 and F1390!=0, **unchecked(Int32(T−F1390))<1000** takes original diagnostic/helper and returns. Equality proceeds; other flags bypass this cooldown. |
| `00b0–00be` | If finalized F1397, call M2285 and return, **without toggling F1373**. |
| `00bf–00c7` | Read F1373, compare equal to0, **stsfld F1373**. This is the ordinary first pause/resume state write. |
| `00cc–00e7` | Reset F13ed=−1; retain original M4790 and M2291(!F1373) helper calls, then reread F1373 to select pause versus resume arm. |

The first guard uses Int64 subtraction after converting T; the later cooldown subtracts Int32 without overflow checking. Do not merge them. Unknown F1370/F13ef/settings and helper effects remain explicit dependencies.

A pending request can wait, be dropped after clearing, or select departure rather than pause. Repeating M2261 while pending is idempotent for those two stores. A new accepted request after consumption can toggle the state again; there is no unconditional duplicate-pause suppression across all calls.

### Entering pause: mutation precedes playback request

On the true F1373 arm, M2262 retains original resource/presentation/helper setup; conditionally remembers normalized cursor-derived F139a, calls M227b, then **F1390=fresh T at01ba**. It enumerates the **existing candidates F17fd** and invokes virtual **M24c4 at01da**, with enumerator finally cleanup. Circle inherits empty M24c4; slider resolves **M146c→M3b53**, spinner **M05fc→M3b54**. These are original resource/action boundaries, not judgement or a full-list component drain.

At0204 call **M3b0d**, which is exactly `q==0`. Only if false does **020b call M3b49**. Presentation construction/registration continues afterward; its later menu body is not reconstructed. There is no direct pause-arm write to object F1527, slider outcome counters/tracking flag, spinner P/C/E or manager candidate/full-list references.

An exception after F1373's store but before020b can leave Play paused while playback was never asked to pause. A dispatch/native helper can fail after q changes. No rollback or assertion that menu/clock state changes atomically exists.

## D. Resume handoff before object processing is released

The false F1373 arm jumps to0912:

1. **M2262:0913 sets F13be=true**. Retain original F13d5 stop and F13ee presentation callbacks/clear.
2. The branch may grant **F13bd=true at09a1** for synthetic time, recorded input, T00ff, handler M0aa4 or F137c/F137b; preserve those checks and their original order.
3. Otherwise existing F1398 or original newly constructed resume-control presentation can defer permission. F1b97 takes **F13bd=true at0bbd**. The constructed control registers **M2265 via ldftn at0a54**. Its graphical appearance is not established here.
4. **M2265** checks original F139b[0].F2cf7; if true it returns0013. Otherwise it changes the supplied presentation component and **sets F13bd=true at0022**. This is a callback/control permission, not a new object judgement.

M2262's resume arm does **not** directly call M3b49, M3b52, M3b4f, M28e2 or rebuild gameplay objects. Clearing F1373 therefore does not itself release simulation or restart audio.

The next reached ordinary **M226b:0854–08ba** examines F13be:

- If F13bd false, or a nonempty F139b exists and its first component F2cf7 is true, call M2262 at08b5 and **return08ba**.
- If permission and component conditions permit, call **M227b at088d**. It retains original presentation removal/fade helpers, then clears **F13be at0069** and **F13bd at0070**.
- After another presentation flag write, if **M3b0d()==true AND F1376==false**, call **M3b49 at08ac**. Otherwise leave the clock branch alone.
- Rejoin08bb and continue toward original startup/fallback/current-object/candidate work **in that same normally returning invocation**.

If M227b or intervening helpers throw, gate and presentation mutations can be partial. A resumed native receiver already active can select a different M3b49 branch from an inactive one; its internal live reads remain authoritative.

The paused callsite itself (**M226b:084e**) always returns at0853 even if its M2262 call just cleared F1373. It does not jump immediately into object updates. A later component pass may occur within the host's bounded inner loop, so “later pass” is not a promised next physical frame.

## E. Clock behavior during paused and resumed passes

### Host time and gameplay publication are separate

E10's reached callback remains **M4241 update→draw→timing tail**. M469e→M43a0 calls **M3b25 before Play M226b**. Neither M3b25 nor M3b24 directly tests F1373 or F13be. They can run while Play is paused, provided the enclosing host path returns normally. Independent stopwatch-derived F2c3a/F2c71 can advance in later M46a7→M46a1 timing-tail work. A Play early return does not return from the whole host callback.

M3b25 first **copies old F2358→F235a at0043**, calls **M3b24:0048**, then processes state/delta/helpers. q2 can become1 at005c. An absent/inactive receiver with !synthetic can later set q0 at00ce. Keep these post-clock writes and optional recorded-input writer M43a1 separate.

M3b24 returns immediately if F2367 null. Otherwise it invokes receiver update, retains E10 range/seek logic, samples original position once at00ca, computes correction and **recomputes offset**. It writes raw rounded F235b at0309, offset F235c at0397, corrected **F2358 at03bc or03c8**, then F2359 at03d2. Play's loading/readiness M2269 can subsequently overwrite T before its pause guard. No general `pausedTime` variable replaces these ordered writes.

### M3b49 is an original toggle, not an unconditional Pause or Resume

| Reached condition | Original sequence |
|---|---|
| Synthetic F234c, q==1 | q=0 at001d; M3b56; returntrue |
| Synthetic, any other q | q=1 at0010; M3b57; returnfalse |
| !synthetic, receiver null OR active predicate true | q=0 at003d; M3b56; optional receiver.virtual M3d61 pause; returntrue |
| !synthetic, nonnull receiver active predicate false | M3b39 hold release; M3b57; q=2 at0064; receiver.virtual M3d60(false); returnfalse |

M2262's pause arm tests q!=0 before toggling; resume-release tests q==0 before toggling. Neither substitutes a snapshot of receiver activity for M3b49's own reads. C can also change through original helpers/range logic. An inactive source with nonzero q can therefore choose M3b49's resume arm even though Play has just set F1373=true. This is a conditional instruction result, not an observed normal pause.

**M3b56/M3b57** dispatch cached actions through **F234b.virtual M2fee(...,2)** before subsequent main-source calls. M2fee constructs a request using virtual receiver methods, M279e/M27a1 and returns original M36b6; M2ffd invokes the captured Action. Reached action M3bac iterates F2357 and M3bad pauses qualifying auxiliary resources, marking F0655; M3bae/M3baf resumes marked resources and clears that flag. This is a separate action/receiver boundary. Actual concrete dispatch scheduling, inline versus later execution, threads and ordering are UNRESOLVED; it must not be labelled a measured asynchronous delay or completed audio operation.

Main audio receiver slots are closed by original ancestry:

| Slot | Audio-backed T08e4 | Fallback T086e |
|---|---|---|
| M3d61 pause | M437c→M437d | M4052 sets F2614=false |
| M3d60 play(bool) | M437e | M4051 optionally zeroes position when true, then sets F2614=true; false retains it |
| M3d62 active | M4385 | M4053 returns F2614 |
| M3d63 update | M4388, E10 empty body | M4057 |

M437c checks active; M437d has F24ae veto, rereads activity, and on its active arm calls **bass.dll!BASS_ChannelPause/M313d at0018**. M437e calls **BASS_ChannelPlay/M313e at000e**. Return values are discarded. Activity accepts native codes1/2 through original unsigned arithmetic; neither that test nor issuing Pause proves audible silence or stable source position. Native failure, a veto, receiver changes and exceptions can separate q from real playback.

Fallback M4057 only adds `hostDelta*F2617/100.0` to F2615 for a new F2c97 epoch **and F2614 true**. A normally paused fallback therefore retains its position; its inactive branch does not update the cached epoch. Host time can continue independently.

### What can stay constant, change, or be overwritten?

**RECONSTRUCTED conditional predictions from the original arithmetic:**

- !synthetic, q0, no hold: main-clock normal branch snaps X to the sampled P because q!=1. With stable finite P and unchanged offset O, I/T can repeat; if P or O changes they need not. q0 alone does not guarantee stopped source position.
- Synthetic q0: no synthetic `X+=d` advance. If F234d false, the existing-T range reclamp can still replace X before publication; offset recomputation or other writers can change T. With stable X/O and original gates, publication can repeat. At q1, synthetic addition requires ordered d<60, with once-per-epoch extrapolation; no fixed frame duration inferred.
- Hold F2382 takes precedence over normal source smoothing, after synthetic branch priority. It assigns X=Double(H); final publication can bypass offset subtraction. It is not F1373.
- **M3b39** releases a reached hold by original seek of unchecked `H+(H==0?0:O)`, clears hold and synthetic, calls M3b24 and copies a fresh T→F235a. Thus a resume toggle with hold can republish time within Play, after the earlier host publication. Without that path M3b49 does not directly assign T.
- Ordinary resume does not call reset M3b4f. That separate loading/reset path sets T=−O, copy0, holdH0/true, optional pause/seek0 and q0. M3b3a's separate seek/clamp/force semantics remain E10; for q0 it can store H/request, set hold and run the clock. A seek performed while interrupted is not automatically the resume algorithm.
- Reached startup **M226b:08bb–0916** has its own F1391/accumulator/phase/F13ef conditions, can clear synthetic state, force seek when T>1000, then call M3b52. It is not an unconditional action on every resume. q0 fallback at091b–093d is suppressed by F1376/startup/already-attempted gates and can instead establish synthetic time via M3b58.

F235a can be captured again by seek/hold release after its host-prefix capture; it is not a physical input-event timestamp. Position/offset/q/previous/copy fields are not atomically published. No paused host pass is promised constant T without its actual source/hold/range/offset/override operands.

## F. Exact suppression and first resumed object attention

Within normal M226b, original M226a and readiness M2269 run first. Their return at000e can prevent everything later; M226a itself can invoke modifier-dependent ruleset work. Presentation/components and other work follow before the main pause guard. A separate early branch **023a–024f** calls M2262 and returns if F13bb==0 or transition phase0; it is not the ordinary stable phase2/lifecycle continuation.

Before **0838** the method can already update presentation values, run timed-event manager **M2f05 at06fb**, check host focus/request through M2239 at071d and run QuickRetry recognition. Therefore even this one Play method is not entirely inert while paused.

| Boundary | Reached effect |
|---|---|
| `M226b:0838–0853` | If F1373 true, reset F13fb/F13fc to−1, call M2262, return. |
| `0854–08ba` | F13be handoff can return as above, even though F1373 is false. |
| `0942–0959` | Original handler M0aca; **M2275 selects current object from old candidates**, then M28e2 refreshes. |
| `095e–09c9` | Optional current-object M0ac5 subject to recorded/nullable-time conditions. E12 branches0972/09a6 rejoin09c9; they do not skip manager updates. |
| `09cf` | M28e3 object updates, ordinary M0653/M0654→M28e4 evaluation paths. |
| `0abd–0ad0` | Reached failed latch takes its separate work/request-control path and returns. |
| `0ad1–0b34` | Recovery F1376 handling, after object updates. |
| `0e5f–0f64` | E27 continuation/finalization/automatic departure checks. |
| `10c4` | Later normal pending-control consumption M2262, after the above reached work. |

M28e4 is invoked through manager object updating on its reached candidate paths, not independently by the pause mechanism. The main pause/handoff returns suppress current selection, candidate refresh, component processing, ordinary object updates/evaluation **and E27's late block** from that invocation. They do not undo clock/input/presentation operations already performed.

**Judgement has its own guard:** live **M2295:0009–002d** rejects failed latch, F1373&&!M4390, F13be, or F1376 before spatial/timing selection. The shared press queue can still drain while paused; rejecting its callback is not a replayable stored hit. Host held/previous-state bookkeeping continues after the component delegate on normally returning paths. Holding a key through resume does not itself prove a fresh press edge; E11 aggregate-edge policy still applies.

A keyboard Pause request recorded before the queued gameplay press need not suppress that press immediately: F13d6 is not read by M2295. In a normal unpaused phase2 pass, M2262 may consume it only at10c4 **after** object attention/evaluation/finalization. Focus callback requests can similarly wait until another pass. Preserve actual branches rather than inventing “pause first, then no object work” globally.

On release of the resume handoff, the same existing manager/object state continues through original M2275/M28e2. The reached control methods do not reconstruct objects, clear all candidates, drain checkpoints, repair spinner observations, clear evaluated flags, or reset F13d9. Original candidate refresh then recomputes its window from fresh clock reads. A clock rewind can leave the nullable-time component gate holding back M0ac5 while M28e3 still runs. Current selection uses old candidates before this refresh; no immediate-attention guarantee for a newly admitted object is implied.

Paused slider/spinner state does not become an artificial completed result. Their next reached tracking/rotation/update retains original E15–E26 rules and helper side effects, including candidate/resource hooks on pause. Exact eventual backlog, held history, angular delta and result timing require runtime operands.

## G. Recovery F1376 is distinct from manual pause

E19's reached **M0acc(false)** sets **F1376=true at006e**, calls M3b49 directly at0073, later M225c(false), then decrements recovery counter. It does not toggle F1373 or set F13be. Original target/addition/presentation helpers and their exceptions remain E19, not a new pause implementation.

M2295 rejects F1376. Spinner M05f5 integration also tests it at0163/0168. But M226b does **not** universally return before M28e2/M28e3 for F1376. At0ad1, after object updates and failed-latch check, it reads original **M2964→F1856** and **M2966→F1857** separately. Ordered **actual > target−5.0** permits clearing F1376 at0b05 and calling **M3b52 at0b2f** after original helper work. `ble.un` at0b02 skips equality, less and NaN. It is not a fixed pause countdown. M3b52's synthetic/native branches are original and distinct from M3b49.

Thus F1376 can suppress input and selected integration while manager updating continues with whatever T was actually published. E27 M2276 has its own F1376 return guard, even if reached after such updating. This does not establish full HP behavior or an observed bar animation.

## H. Focus/control provenance with exact host ordering

**CONFIRMED:** M2237 registers M2239 on both host events via M4238 at0028 and M423a at003e; M2266 unregisters both. M4236 is the sole direct F2985 writer. If its supplied Boolean differs, store at000c, then invoke F2986 when true or F2987 when false. Play's callback is therefore not a uniquely labelled “lost focus” callback.

The concrete host override M469e does not itself directly call base M4249. The necessary connection is **M43a0's component delegate→M46d7**, whose **0025 component iteration invokes M46fb→Play**, then **002b calls base M4249**. Base M4249 converts backend F21c1 and calls M4236. This puts that refresh **after** reached Play component work in the delegate, while Play's earlier M4235 reads can still see the previously stored value.

The original Windows backend constructs F21c1 with a supplier M2429 (M2420:005e–0073, original arguments50,true). M2429 compares **user32.dll!GetForegroundWindow/M2426** with stored IntPtr F14ce via IntPtr.Equals. Actual supplier cache cadence/native handle values are UNRESOLVED; constructor argument50 is not a measured focus delay.

M2239: if !F1373, F1374 true and F1aad false, invoke **virtual M2261 at0016**. It does not set F1373 there, and does not test event-specific direction. If already paused, it performs original presentation/input-availability work instead; focus regain is not an unconditional automatic resume. Its queued presentation re-enable callback is separate from pending Play consumption.

M226b also polls stored **M4235** at0705: if false, F1374 true and !F1373, call M2239 at071d. **M2290** prepares F1374 under its exact current Play/manager/readiness/time-range/failed/pause/F1b97/record/recorded-input predicates, catching errors tofalse. It is not automatically true in every map or while loading. F1aad can suppress the request. With both host event and polling routes, M2261's pending guard prevents repeated timestamp replacement until consumption.

M46d1 has another original conditional M2261 call associated with its own host/control work. Its broader window/control feature is not generalized to every OS interruption. External OS scheduling, actual focus events and input provider behavior remain runtime dependencies. No menu visibility or OS event is asserted from these static paths.

## I. Session finalization, departure, and control variants

E27 remains authoritative. Main pause/handoff returns occur before M2276. A clock q0 does not override those returns. If M2276 is reached, its own **F1376 at0014** returns0114 before the q0 shortcut/time threshold. With other guards allowed, q0 and positive T can take its separate shortcut; **q0 is not F1373**. Upstream synthetic recovery can change q before the consumed read.

If a normal request is consumed before finalization, it can toggle pause and prevent later passes from reaching E27. If finalization becomes latched **earlier in the same normally returning Play pass**, the late M2262 reads F1397=true and calls M2285 instead. A control request does not capture the earlier value of that latch.

Already-finalized Pause/Escape takes M225b→M2285 directly; pending M2262 also selects it before toggle. These routes can precede automatic maxEnd+3000, retaining original selectors and M46ad guards. M2285 can choose Edit1, SelectPlay5 or ordinary Rank7; issuing, accepting, committing and displaying remain separate.

Other controls are not pause aliases:

The pause-arm callback registrations supply concrete provenance without reconstructing the menu: M2262 obtains M22af for F139c at0413/041e, M22bd for F139d at0515/0526, and M22bf for F139e at061d/062e. M22af invokes M2261; M22bd invokes M2263(false); M22bf makes the mode requests below. Those registrations do not establish visible button labels or successful mouse interaction.

- **QuickRetry23** or the reached original modifier/key82 combination is checked before the main pause return. Original normalized host progress can reach M2263(true), which invokes original M2255 and requests **M46ad(Play2,true)**. M22bd calls M2263(false), also requesting Play2,true without that prefix. Stop before retry loading. The Boolean only affects the duplicate checks recovered in E03, not every request guard.
- **M22bf** requests Edit1,false for F2c73 or otherwise SelectPlay5,false, after original helper work. It does not require this investigation's finalization predicate or implement a pause toggle.
- **Skip19** in M225b precedes the later paused-selection branch and dispatches M2254→queued M22ad→M2256. The latter rejects paused/F13be states; its allowed skip branch has original seek operations. Its separate F13eb/F138e branch can select departure. Full skip/retry/UI behavior is outside scope; retain those original gates.
- M225f with selection sentinel can call M2261; selected presentation actions can instead invoke their registered retry/departure callbacks. No full pause-menu reconstruction follows.

M46ad can defer, redirect, suppress or accept these requests using live global state. Deferred M46e6 reruns it with captured target/Boolean, not the old Play eligibility. An accepted pending-mode store and phase change are later than request issuance; E02/E03/E27 commit callbacks remain independent. No results screen or completed retry is claimed.

## J. Complete focused writer inventory and original body hashes

Complete direct managed stores (other field accesses, caller identities, signatures/flags and exact offsets are in JSON):

| Field | Writers / stores |
|---|---|
| F1373 | M2237:04ab=false; M2262:00c7=toggle; M2266:00c6=false; special T00ff.M071b:0180=toggle |
| F1376 | M2237:04b7=false; M0acc:006e=true; M226b:0b05=false |
| F13be | M2237:04d5=false; M2262:0913=true; M227b:0069=false; M2266:00c0=false |
| F13d6 | M2261:000a=true; M2262:0030=false |
| F13d7 | M2261:0016 only |
| F1390 | M2262:01ba only |
| F13bd | M2262:09a1/0bbd=true; M2265:0022=true; M227b:0070=false |
| F13ed | M2262:00ce; M225f:0072/03fd |
| F13ee | Fresh collection reference M222e:0047; original later Add/Clear mutations are separate |
| F1374 | M2290:0010/00ef/00f8; optional recorded branch M43a1:08a7/08c3 |
| F2985 | M4236:000c only |

Host F2986/F2987 delegate updates use address-taking and original compare-exchange MethodSpec, through M4238/M4239/M423a/M423b. They are not falsely reported immutable merely because no stfld exists. Reflection, external/native mutation and arbitrary callbacks remain outside the direct-store coverage.

| Method / exact original signature | Original body SHA-256 (including header) |
|---|---|
| M225b /`2002021c11a9e8` | `3660d747d1b78c41b4be58cc609155701b40e0c816a2d08bbff0c5fc088465c0` |
| M2261 /`200001` | `876f5822d1226de7ad82589970e041e0604f46a846af7b29663560a8417cb215` |
| M2262 /`200001` | `9ce41412f438af334aa5cec529527f3229feb9b89c0ab41c8790754efc96c8c2` |
| M227b /`200001` | `6331c217e2ca822c382d5a591468fd7d22d65be0308e96bf5a2ed9a5043f754b` |
| M2265 /`2002011c1285a9` | `9c4933d15b4eb7e639e36ad3f904e87e190bc0098db00c9aa0165220e6e61df1` |
| M3b49 /`000002` | `3c36e7ff3ecb11031dc6de0df89e31e1b51fffa1bf30b9d917ef054597404c57` |
| M3b24 /`000001` | `29c9774eafa8c06cfaac6732806015eb94a5d48f0c8f6d4654da6cd050bc9aee` |
| M3b25 /`000001` | `0a6661c4fc0e824b7b4f80a63efe1fa31708b9247b3fae95fcf6c530f37e95d8` |
| M226b /`200001` | `de32a6e69eedceee8be3f629c986d16f35fe7b99952e0389a3146b1b8863f52f` |

## K. Conditional cases: instruction prediction, never runtime observation

Every row is a **RECONSTRUCTED conditional prediction** citing the **CONFIRMED instruction boundaries above**. Every original-runtime observation is **NOT RUN — WINDOWS HOST REQUIRED**. Reuse unchanged circle/slider/spinner fixtures in separate intended ordinary runs; successful original import, actual bindings/configuration and helper returns remain assumptions, not observations. Unless varied: ordinary base, phase2, valid references, live !M4390, no failed/recovery latch, caller/readiness gates permit and original helpers return normally.

| ID | Discriminator | Predicted original boundary | Runtime |
|---|---|---|---|
| P01 | No interruption | Clock/input then ordinary attention/update and later E27 guards | NOT RUN |
| P02 | Pause18/Escape27 recognized before first object | Pending request first; actual toggle still requires consumption/throttle/readiness gates | NOT RUN |
| P03 | Pause key and circle hit edge in same poll, flag initiallyfalse | Pending alone does not reject M2295; press can run before late toggle | NOT RUN |
| P04 | F1373 true while circle would otherwise pass timing/space | Live M2295 rejects; manager update blocked at0853 | NOT RUN |
| P05 | Paused slider with pending component | No M0ac5/M28e3 from paused invocation; no automatic checkpoint drain | NOT RUN |
| P06 | Paused active spinner | Outer Play return suppresses progress update there; host clock/input can still run | NOT RUN |
| P07 | Pause request shortly before final end | Late consumption can follow that pass's object updates; next passes suppressed | NOT RUN |
| P08 | After final end but before B+200, q nonzero | E27 can still reject finalization; reached pending control may then toggle pause | NOT RUN |
| P09 | Request pending, same pass first finalizes beyond threshold | Late M2262 sees new Dtrue and selects departure, not toggle | NOT RUN |
| P10 | F1397 alreadytrue before recognized Pause/Escape | M225b004e calls selector before automatic B+3000 requirement | NOT RUN |
| P11 | F1370true, unpaused, Int64 elapsed999 | M2262 returns retaining pending | NOT RUN |
| P12 | Same elapsed1000 | First guard permits consumption, later gates remain | NOT RUN |
| P13 | Repeated M2261 while pending | No request-time refresh or second toggle from the helper itself | NOT RUN |
| P14 | Request consumed but F13effalse/null presentation | Pending cleared before return; no guaranteed deferred retry | NOT RUN |
| P15 | Ordinary cooldown delta999 versus1000 | First drops consumed request via helper; equality permits later branch | NOT RUN |
| P16 | New request after earlier consumption | Can toggle opposite state if reached guards permit | NOT RUN |
| P17 | Resume toggle sets F1373false, permission not yet granted | F13be blocks hit input and Play object work | NOT RUN |
| P18 | M2265 invoked while first presentation F2cf7true | Callback returns before F13bd store | NOT RUN |
| P19 | M2265 permissiontrue but outer component still gated | M226b handoff can still return; no immediate source restart proof | NOT RUN |
| P20 | Resume permission/component conditions pass, q0,!F1376 | M227b clears gates, M3b49 invoked, same normal pass can refresh candidates | NOT RUN |
| P21 | Same handoff but q nonzero | No resume-toggle call at08ac; clock state retained | NOT RUN |
| P22 | Same handoff but F1376true | Clock toggle suppressed; manager work may still follow | NOT RUN |
| P23 | Stable paused fallback position and unchanged offset | Repeated T publication can equal prior T while host advances | NOT RUN |
| P24 | Paused native route, actual source P constant, offset changes | Corrected T can change despite q0 | NOT RUN |
| P25 | F24ae veto/native Pause unsuccessful/source still moves | F1373/q0 do not prove constant original source/T | NOT RUN |
| P26 | Synthetic q1 toggled to0 | No later synthetic X+=d at q0; range/offset publication still matters | NOT RUN |
| P27 | Synthetic q0 toggled to1 | Synthetic continuation selected; no main-source Play call in that arm | NOT RUN |
| P28 | Resume with hold set | Original seek/republication/F235a recapture may occur within M3b39 | NOT RUN |
| P29 | Seek/reinit changes clock while interrupted | Preserve original H/C/q/offset writes, not a saved paused-time restoration | NOT RUN |
| P30 | Null receiver in main clock | M3b24 returns; M3b25 already captured previous T; other writers remain | NOT RUN |
| P31 | Loading M2269 reached while interrupted | Can overwrite host-published T and return before pause/object work | NOT RUN |
| P32 | Host callback normally returns after paused Play | Draw/timing-tail still invoked through original outer path | NOT RUN |
| P33 | Recovery sets F1376 without manual pause | Input/spinner integration blocked, not universal M28e3 suppression | NOT RUN |
| P34 | Actual target delta exactly−5 versus ordered greater | Equality retains F1376; greater permits clear/restart | NOT RUN |
| P35 | Recovery comparison NaN, artificial | ble.un skips clear/restart | NOT RUN |
| P36 | Stored focusfalse, F1374true,!paused,!F1aad | Polling route records pending through M2239, not direct pause | NOT RUN |
| P37 | Host focus refresh changes after Play component work | Event can request another pass; already executed object work not undone | NOT RUN |
| P38 | Focus regained while paused | Shared callback does not automatically toggle resume | NOT RUN |
| P39 | Either focus event while unpaused and original eligibility true | Same callback can request; no event-direction test before M2261 | NOT RUN |
| P40 | F1374false or F1aadtrue | Shared automatic request predicate can be suppressed | NOT RUN |
| P41 | QuickRetry path reaches its threshold while paused | It precedes main pause return; original Play2,true request possible | NOT RUN |
| P42 | Skip callback executed while paused/F13betrue | M2256 rejects; not a resume operation | NOT RUN |
| P43 | Held gameplay key through resume, no new aggregate edge | No guaranteed fresh circle press; retained held history is distinct | NOT RUN |
| P44 | Rewound T below retained nullable component time | M0ac5 can be skipped while M28e3 still reached after refresh | NOT RUN |
| P45 | Exception after F1373store before audio-toggle call | Paused Play can coexist with unchanged playback branch | NOT RUN |
| P46 | Exception after resume flagclear before handoff setup/cleanup | Partial flags/resources remain; no atomic recovery promised | NOT RUN |
| P47 | q0, positive T, all Play/handoff/recovery guards allow E27 | Only then can q0 shortcut matter; ordinary pause does not bypass early returns | NOT RUN |
| P48 | Departure request deferred/suppressed/accepted | E03/E27 pending/commit boundaries remain separate; no visible result proof | NOT RUN |

No fixture was imported, no original key/focus/audio operation occurred and no case has a measured host cadence. Integer overflow, invalid/null references, helper exceptions, alternative receivers and artificial states retain original behavior rather than default repairs.

## L. Minimal E20 follow-up and unresolved boundary

Do not modify or recreate E20's packet. When the owner supplies a Windows PC, first verify authorized execution, pinned-process/import identity, display/input/audio and an actually capable managed/native debugger or profiler. Use the existing README, INSTRUMENTATION, matrix and probes. This report adds only these observation requirements:

- EXE/fixture/audio/settings/environment hashes, actual Play/handler/manager/receiver identities, and imported object types/start/end. Natural controls versus debugger-forced state must be separate.
- Actual key recognition and consumer, F13d6/F13d7, every reached M2262 guard, F1373/F13be/F13bd/F1390/F1376 and original helper exceptions before/after each store.
- Host sample/epoch and each source position/q/C/hold/H/X/I/O/T/copy/previous read/write; original source Pause/Play/seek arguments, return values and dispatch callbacks. Do not call timing methods again just to log them.
- Current selection/candidate refresh/component/manager evaluation entries or suppressed branches, retained slider/spinner progress and nullable time gate, then K/D/session selector/request state.
- Host stored focus/native supplier operands and callback order, thread IDs/QPC sequence, source action execution versus request, actual M46ad acceptance/commit and any capture provenance.
- Matched untraced/traced normal, pause and resume runs; quantify instrumentation overhead. Stepping/forced clocks, handler changes and exceptional states are separate probes. A visible menu is not proof of stopped clock/object updates.

**UNRESOLVED:** actual configured key, input/focus/native/cache/provider/scheduler behavior, runtime F1370/F13ef and presentation gates, source pause success, real T stability, source offsets, event/exception/reentrant order, resumed candidate and component cadence, request acceptance, pixels/audio and overhead. No static case establishes these observations.

The smallest remaining verification dependency is one original Windows pause→handoff→resume trace with F1373/F13be/q/T and actual object-update entries. The next bounded static investigation, if Windows remains unavailable, is the original seek/record-restore path's effect on retained object evaluation and component-time state; stop before replay serialization or full retry loading.

# 3. Pseudocode

**RECONSTRUCTED — independent non-executable dataflow. Preserve separate reads, helper boundaries, branch priority and exceptions.**

```text
recognized ordinary Pause/Escape:
    require original dispatch, phase and readiness guards
    if current finalized latch:
        original M2285 departure selector
    else if original ordinary live branch:
        M2261:
            if pending: return
            pending = true
            requestTime = Int64(fresh gameplay time)
    retain other original control branches

M2262 when reached:
    if original unpaused/F1370/time-delay predicate: return, retain pending
    if not pending: return
    pending = false
    if original readiness/failure/cooldown veto: return
    if fresh finalized latch:
        original departure selector; return
    F1373 = not fresh F1373
    reset original selection; retain original helpers
    if later F1373 is true:
        retain original preparation
        record fresh gameplay time
        retain original candidate M24c4 hooks
        if original M3b0d() is false: original M3b49()
        retain original presentation construction
    else:
        F13be = true
        retain original presentation/control work
        grant F13bd only on original branch or later original callback
```

The record-time store precedes candidate hooks (M2262:01ba before01da). The following projection keeps clock publication separate from Play suppression.

```text
normal host pass:
    original source/host/input prefix, including optional earlier control entry
    M3b25:
        previous = fresh T
        original M3b24()                 // still called during Play pause
        retain original state/delta/helpers
    keyboard controls and queued gameplay presses
    component delegate -> ordinary Play:
        original early preparation/readiness/presentation/timed-event work
        if F1373:
            original pending-control consumer
            return from Play only
        if F13be:
            if original resume permission/component gates fail:
                original pending-control consumer
                return from Play only
            original M227b clears F13be/F13bd after its helpers
            if original q0 predicate and not F1376:
                original M3b49()
        retain original startup/source fallback
        select current from old candidates
        refresh candidates
        optional original component evaluation
        original manager object updates/evaluation
        retain failed/recovery checks and E27 session finalization
        original later pending-control consumer
    base host focus refresh after component iteration
    retain original later host work, draw and timing tail
```

**RECONSTRUCTED — original clock-toggle boundary, not a replacement clock:**

```text
M3b49:
    if original synthetic flag:
        if separately read q == 1:
            q = 0; original M3b56 action dispatch; return true
        q = 1; original M3b57 action dispatch; return false
    if receiver null or original active predicate:
        q = 0
        original M3b56 action dispatch
        optional original receiver pause
        return true
    original M3b39 hold-release/seek/publication
    original M3b57 action dispatch
    q = 2
    separately read receiver.play(false)
    return false

M3b24 during interruption:
    retain original receiver/range/synthetic/hold/correction arithmetic
    Round working Double -> conv.i4 raw integer
    recompute original offset
    publish corrected T or original bypass T
    publish separate copy
    // Constant T requires original operands to justify it, not only F1373/q0.
```

**UNRESOLVED runtime selection and effects remain unresolved. E20 is incomplete; Work 2 remains suspended.**
