# 1. Result

Expedition 10 — Gameplay Clock Source & Update Ordering. 2026-10-27 (Asia/Bangkok). Work 1; original-software research only.

## Objective, scope and provenance

Recover the original source and writers of ordinary osu!standard gameplay time F2358, its arithmetic and directly relevant interruption/rate paths, and its managed ordering relative to candidate refresh, object update and visual evaluation. No replacement clock, numerical simulator, gameplay component, renderer or optimization was built. Work 2 remains suspended; its code and `reconstructed/` were not inspected or changed. No authorised exception.

Read the complete standing contract, cumulative contexts, Expeditions 01–09 and relevant evidence. Preserve E05's corrected ordinary route M113e→M0651→M48de, circle M48df→M45eb/M45ec and slider M48e0→M142d. E04's superseded Taiko annotation is not reused. E05's beatmap/slider-duration arithmetic does not establish live audio synchronization. E09's screen projection question is deliberately deferred by this assignment; rendering is not complete.

Requested remote parent: `b27573c6b513d0e24b2b3cd1f1b5dfc9b5e48a77`; tree `9fcb5b4dbc2c6501528e3e7b47f88f4b8bd547f2`. Verified that remote commit/tree and created `work1/expedition-10-gameplay-clock` directly from it. Local baseline commit `2d81a23fb657c2b0619b5b8f6a783272ce9b25d3` is a mirror with the identical tree, not the requested remote commit identity. Publication uses the requested remote parent explicitly. Earlier records remain unchanged; contexts receives only an append. The verified publication hash is supplied in the final handoff, avoiding a self-referential commit hash inside its own tree.

| Input | SHA-256 |
|---|---|
| osubackup.zip | 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 |
| osu!.exe | 6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d |
| Instruction.md | bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9 |

Same self-declared Stable40/file-version1.3.3.8 reference; official dated release, authenticity and modification status remain UNRESOLVED. Host recheck: Linux x86_64, no Wine, dotnet, DISPLAY or `/dev/dri`. No original executable, CLR method, BASS native operation, Windows instrumentation, audio playback, capture or frame-time experiment was executed.

Created this report, `tools/gameplay_clock_evidence.py`, `tools/check_gameplay_clock.py`, `evidence/10/gameplay-clock.json`, and this expedition's contexts append. Extractor reuses unchanged E04/E05 collectors; it records 103 selected EXE methods, normalized token/constant/branch/arithmetic sites, method identities/hashes, complete direct managed accesses to 26 clock-related fields, direct callers of selected clock methods, playback/host receiver ancestry and slots, and nine bass.dll import identities. It does not execute an alternative timing model. Original binaries, assets, source and full disassembly are not distributed.

## Procedure, confidence and reproduction

Competing explanations: F2358 is raw driver position; it is an independent stopwatch; it is a corrected combination; every update advances it; pause freezes all clocks; candidate and visual stages share one immutable snapshot; host time is sampled before update; all similarly named time fields agree. Discriminate by scanning all original managed bodies for field accesses, privately reading normalized instructions/stack dataflow, resolving concrete playback slots, then extracting and asserting exact sites and branches. Follow only the involved playback/factory/rate/seek and host callback boundaries. Opaque operations are retained, not replaced with guessed synchronization policy.

Mxxxx/Fxxxx/Txxxx mean EXE MethodDef `0x0600xxxx` / Field `0x0400xxxx` / TypeDef `0x0200xxxx`. Offsets below are hexadecimal, relative to first CIL opcode, excluding headers. Public JSON offsets are decimal. CONFIRMED means original static instructions/metadata, not observed runtime behavior. Names such as working time, hold and draw epoch are RECONSTRUCTED aliases. Conditional predictions require stated state, resolved receiver, normally returning helpers and no intervening mutation. Original CLR conversion behavior for non-finite/out-of-range values remains UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/gameplay_clock_evidence.py /path/osubackup.zip > /tmp/clock-a.json
python re/tools/gameplay_clock_evidence.py /path/osubackup.zip > /tmp/clock-b.json
cmp /tmp/clock-a.json /tmp/clock-b.json
cmp /tmp/clock-a.json re/evidence/10/gameplay-clock.json
python re/tools/check_gameplay_clock.py /tmp/clock-a.json
python -m compileall -q re/tools
```

Checks performed for publication: deterministic extraction/byte comparison, E10 checker, unchanged E02–E09 checkers, Python compilation, baseline-file/journal-prefix preservation, report-heading and scope/diff checks, and remote parent/tree/branch/every-file blob/mode verification. Exact outcomes are recorded in contexts and the final handoff. Cases C01–C32 below are manually branch-derived static predictions, not original-game passes or surrogate-clock tests.

Actual local outcomes: repeated extraction byte-identical to public JSON, SHA-256 `a3695f9372636a1788de9fe8738812ed27e46420d6fa957a60705bcf3b283f46`; E10 checker PASS for103 methods and17 overlapping historical body-hash comparisons; all eight unchanged E02–E09 checkers PASS; compilation of all research Python tools PASS. All42 baseline tracked files were byte-identical before the journal append. Selected branch destinations validate as original instruction boundaries. No runtime comparison was possible. During checker development, a drafted M28e1 target at M226b:959 failed; the original token is M28e2 and the assertion was corrected before publication, without editing an earlier record.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M3b24 | 181024 | 984 | 29c9774eafa8c06cfaac6732806015eb94a5d48f0c8f6d4654da6cd050bc9aee |
| M3b25 | 181408 | 267 | 0a6661c4fc0e824b7b4f80a63efe1fa31708b9247b3fae95fcf6c530f37e95d8 |
| M4384 | 1ae22c | 33 | 21ef5dcd730badd50db71838d96ea537f0b537717e9cbda5fc3f3a88a5976926 |
| M4057 | 198964 | 69 | e8441596bed9aae562b8db54321d0c44cd5d5ca9d83891dd97b4f3e747745bdd |
| M4241 | 1a493c | 28 | 5e7870c1ea282e05f71adfab0efa4b99b8f75424f19199ebf80079b0ac6cf723 |
| M46a1 | 1c2428 | 484 | 4dcff9e0a3ecbffdbac5b76a4dc85dbe1fbec490370b31a0f5c0f04656420ea5 |

# 2. What they found

## Player-level explanation

Stable's current song time is a corrected gameplay clock. On the audio-backed route it asks BASS for a playback position, combines that with elapsed host time, and either smooths, catches up, slows down or snaps to that position. It rounds a working value, calculates an offset, then publishes the integer used by the candidate-window and circle-animation paths recovered earlier. It is not simply a copy of the audio driver's reported position.

Loaded circles and sliders approach their scheduled hit times because those systems read the published clock. On the verified managed callback path, clock processing occurs before the ordinary Play candidate refresh and object update; visual animation reads it later during drawing. Loading, seeking and an optional recorded-input path can overwrite it in between. This is a connection to scheduling and appearance, not proof that every judgement routine uses this field or that a particular pixel appears at a measured audio instant.

Pause does not freeze the application's stopwatch. Native playback can pause while the host clock advances; the song-clock update still executes and can republish the paused position. A separate synthetic branch supports advancing time without relying on native audio position, including negative/lead-in cases. There is also a genuine no-audio fallback playback object. Actual playback latency, pause-menu behavior in Windows and heard-audio alignment remain unmeasured.

## E10-A — Fields and complete direct gameplay-time writer inventory

CONFIRMED metadata: F2358 belongs to static timing/audio module T07b9. Its signature `0608` is int32, not double. The following interface aliases are descriptive, not original symbol names.

| Field / alias | Stored type | Direct relevance |
|---|---|---|
| F2358 / T | int32 | Adjusted gameplay time, read by E07/E08 paths |
| F2352 / X | float64 | Working position before round/publication |
| F235b / I | int32 | `conv.i4(Math.Round(X))` on main update |
| F235c / O | int32 | Recomputed offset; sole direct writer M3b24:397 |
| F2359 | int32 | Copy after main publication, but not a guaranteed alias |
| F235a | int32 | Previous T captured by M3b25; also reset by seek paths |
| F2353 / H; F2382 | int32; bool | Forced position and hold flag |
| F234c / C; F234d | bool; bool | Synthetic/range branch; suppress its X reclamp |
| F2368 / q | enum-like value type | Numeric states 0,1,2; no recovered named constants in T06e4 |
| F2367 | reference T0803 | Current playback receiver |
| F2365 / f0; F2366 / f | float32; float32 | Base/current frequency-like operands |
| F237c | int32 | Host time of last snap/large correction |
| F237d | int64 | Last consumed F2c97 epoch |
| F136a | bool | Forced snap selector; ordinary M2237:4b1 writes false |
| F2389 | bool | Selects −15 term; native-info flag source, purpose unresolved |
| F235e | float64 | Additional accumulator modified by synthetic/seek paths |
| F235d; F2371 | float64; float32 | Published T delta; smoothed delta, separate from host dt |
| F2c3f; F2c3d | float64; float64 | Current/previous host stopwatch samples |
| F2c3a | int32 | Rounded host time, not song time |
| F2c71 | float64 | Host sample difference |
| F2c97 | int64 | Counter incremented by draw stage, including an early-skip path |

Full scan: 19,659 nonzero-RVA managed bodies decoded; 1,739 zero-RVA methods; zero decode errors. Exactly seven methods contain direct `stsfld F2358`, at nine sites:

| Writer | Offsets | Value / condition |
|---|---|---|
| M3b08 static initializer | 047 | 0 |
| M3b24 main clock | 3bc,3c8 | I−O, or I under bypass condition |
| M3b3a seek/discontinuity | 0a8 | Rounded X−O in synthetic branch, then calls main clock |
| M3b4f reset | 006 | −O, followed by F2359=0 |
| M3b59 synthetic initialization | 007 | Supplied int target; also I/F2359/X |
| M2269 loading/readiness | 1ef,208 | F13f1 or −F13f6; also writes F2359 |
| M43a1 optional frame path | 2d8 | Selected frame.F1310 when F2a7e true |

Two address-taking sites are not additional clock algorithms: M38a0:1f9 passes F2358 to Int32.ToString at20d; M497d:185 does likewise at18a. Direct inventories exclude reflective, external/native writes and mutations outside this assembly. They do not prove those never occur.

## E10-B — Where the playback position originates

CONFIRMED: M3b45 can reuse a current track under its map/resource checks. Otherwise M3b45:0c8 calls M3d52 and stores its returned playback receiver in F2367 at0d5, optionally disposing the old receiver first. Factory M3d52 attempts a map stream; with a stream it constructs T08e4 through M4378 at051. A null stream or caught constructor failure routes through M3d51 to T086e/M404f. M3d51 assigns duration from nonnegative map.F1008+10000, otherwise600000. These are factory alternatives, not proof which receiver a particular Windows run selected.

Metadata closes T0803's in-assembly descendants to T086e and T08e4. Relevant overrides reuse the declared slots; unrelated methods with equal obfuscated names are excluded by ancestry.

| T0803 virtual slot | T08e4, audio-backed | T086e, fallback |
|---|---|---|
| M3d5c position | M4384 | M4054, returns F2615 |
| M3d5f seek(double) | M437f | M4050, stores F2615 |
| M3d60 play(bool) | M437e | M4051, optionally zero then enable |
| M3d61 pause | M437c | M4052, clears F2614 |
| M3d62 active predicate | M4385 | M4053, returns F2614 |
| M3d63 update | M4388, empty return | M4057, host-delta accumulation |
| M3d65 / M3d66 rate get/set | M438b / M438c | M4058 / M4059 |
| M3d67 / M3d68 policy flag | M4389 / M438a, delegate to base then setter reapplies | Base F24b3 get/set |

Audio-backed M4384:001 and007 separately call handle getter M3d5d, which reads F24b1. The second handle goes to M312a→M312b `BASS_ChannelGetPosition(handle,0)`; its returned byte count and the first handle go to M3112 `BASS_ChannelBytes2Seconds`. The resulting double is multiplied by1000 at01f. There are two handle reads, not one captured handle. Native DLL identity/import names and signatures are confirmed in ImplMap; actual native return values, driver latency/error conventions and heard position are UNRESOLVED. M3b24 does not test native error returns before using this value.

Fallback M4057 first compares F2c97 against its F2616. Equal means return. Otherwise, if F2614 is true, it adds `F2c71 * F2617 / 100.0` to F2615, then stores F2616=F2c97. If false, it does not store the epoch. Default rate is100. This is original fallback code, not an analysis surrogate. M3b24 calls the update slot before reading the position slot, so fallback position can advance first.

## E10-C — Main calculation, correction and publication

CONFIRMED instruction projection of M3b24; full independent pseudocode is in section3.

1. Null F2367 returns at007 with no main-clock writes. Otherwise invoke playback update at00d.
2. When M3b22() / F237b is true, apply range handling using the **existing** T and O. Bounds are −O and `duration−O`, signed inclusive. Outside bounds, if !C and q!=1, set C then pause the receiver. Inside bounds, if C, clear C, hold and H (H=−1); q==1 seeks/restarts only if active predicate is false, otherwise other q values seek. The flag's full policy is not assigned a convenient user-facing name.
3. Sample playback position P once through M3d5c at0ca. With !C and q!=0, equality of P and playback.F24b0 can set q=0 at0f5. Equality is exact; the code also reloads F2367 for its null check.
4. Compute effective multiplier and extrapolation delta, then store consumed epoch at13d:

   `r = (M3b37()/100.0) * conv.r8(F2366 / F2365)`

   `d = F2c97 != F237d ? F2c71*r : 0.0`

   The ratio is a CIL division on float32 field loads followed by conv.r8, not a forced conv.r4 of the ratio. Original CLR intermediate precision is not independently measured. Rate helper M3b37 returns virtual M3d65 or100 if null; later calls read it again.
5. Branch priority is C, then hold, then normal playback. C can first set X using upper-first M2d7c(T,−O,duration−O) unless F234d. It advances X and F235e by d only for q==1 and ordered d<60; exactly60 and NaN do not advance. Hold sets X=double(H), updates last-correction host time and bypasses smoothing. A later null reload has a separate X=0 path; an initial nonnull check is not an immutable receiver snapshot.
6. Normal branch snaps if q!=1, X==0, P==0, or F136a. Otherwise:

   `v = X + d; v = v - (v - P)/8.0; e = v - P`

   Threshold h is11 if signed wrap32(hostNow−lastCorrection)<1500 **or** the later rate read is ordered<100; otherwise33. The original two `blt` tests route NaN rate to33 if the first test did not route to11.

| Priority after v/e | Action before integer publication |
|---|---|
| ordered Abs(e)>2h | Snap X=P; if f equals f0, call opaque M469d (return does not change snap choice) |
| ordered e<−h | X=X+2d; record host correction time |
| ordered e<h | X=v; no correction-time write |
| Otherwise | X=X+d/2; record host correction time |

At −h the v branch wins; at +h the half-delta branch wins; at ±2h there is no forced snap. Unordered error skips the first two ordered tests and the e<h test, reaching half-delta; subsequent conversions of non-finite X remain unresolved. Extrapolation is gated once per draw epoch, but **smoothing and position sampling are not**. A second call with d=0 can still change X toward P.

7. At303→309 call `System.Math.Round(double)` then `conv.i4` to store I. This differs from direct truncation. No alternate rounding overload or explicit MidpointRounding mode is supplied. Preserve the original call; target-runtime half-tie/non-finite/overflow probes were not run.
8. Recompute O at397. Define B=24 when selected map exists, map versionF0fd1<5, and application mode is not Edit1; otherwise0. Define M=0 for Edit or null map, otherwise wrap32(map.F0ff9+map.F0ff4). Define U=M3df2(F03de), an int-converted setting value whose user-facing name remains unresolved; D=15 if F2389 else0. Then the arithmetic is:

   `O = conv.i4( double(B) + double(M) + (-double(D)-double(U)) * (M3b37()/100.0) )`

   B and M are not multiplied by rate; the −15/setting term is. Map additions happen in int32 before conversion. Setting/rate/selected-map calls are separate reads. O uses direct conv.i4, without Math.Round; finite in-range fractions truncate toward zero. The −15 flag's writer M3b5e derives it from native info `(F0cdf &262144)==0` at21b, catches failure to false at25c. Its audible rationale is UNRESOLVED. No claim is made that F0ff9/F0ff4 are specific named local/online offsets.
9. If hold OR (q==0 AND existing T==0), set T=I at3c8. Otherwise set T=wrap32(I−O) at3bc. Finally load T again and set F2359 at3d2. No principal publication uses a lock/Volatile/Interlocked transaction. I, O, T, F2359 are separate ordered writes. Range/seek handling used the old O before this new O was computed.

RECONSTRUCTED: these operations form a playback-corrected gameplay clock in millisecond-like beatmap units, strongly grounded by both native seconds×1000 and stopwatch ticks/frequency×1000. They do not establish exact synchronization to sound at the speakers or monotonicity. A source jump, changed offset, reset or optional writer can move T discontinuously.

## E10-D — Host source and exact managed update/draw order

CONFIRMED: host T095e derives T08c1 and overrides M4249→M469e (update), M4248→M46a6 (draw), M4246→M46a7 (timing tail). T08c1 constructor M4233 wires its M4241 EventHandler into backend receiver via M3965 at025; M3965 combines the event delegate into F21c5. Backend Run M396d remains abstract in this boundary investigation; OS scheduling and actual callback cadence were not measured.

M4247 startup calls initialization at001, invokes timing virtual M4246 at007, then backend Run at012. Thus timing has a startup seed. On a reached, normally returning M4241 callback with F2983 false:

| Order | Original sites | Recovered operation |
|---:|---|---|
| 1 | M4241:00a→M469e | Update, using most recently published host samples |
| 2 | M469e:120/12b→M43a0 | Pass M46d7 as component-update delegate |
| 3 | M43a0:0ca→M3b25:048→M3b24 | Capture previous T, process playback clock and publish |
| 4 | M43a0:0e0;773–78d | Optional M43a1 override; other processing and queued callbacks |
| 5 | M43a0:790→M46d7:025→M46fb:001 | F2c9f components receive virtual M1ab2; base ordinary Play resolves M226b |
| 6 | M226b:959 then9cf, when gates permit | Candidate refresh M28e2 before separately gated object updates M28e3 |
| 7 | M4241:010→M46a6 | Draw-stage preparation and component draw; visual state reads T separately |
| 8 | M46a6:04b or27f | Increment F2c97 on throttled early-skip or normal-completion path |
| 9 | M4241:016→M46a7:012→M46a1 | Refresh host samples/delta for subsequent use |

The order is **Update → Draw → Host timing refresh**, not a guessed Time → Update → Draw scheduler. Startup seed aside, the reached callback's update uses the last completed timing-tail samples. No fixed frame duration is implied. Early returns, exceptions, backend callbacks and other helpers can interrupt the normal path.

M46a1 samples M47cd at010, may apply its limiter and Sleep/resample at090, then writes previous F2c3d at0c2, current F2c3f at0c8, rounded F2c3a at0d9, and F2c71=current−previous at1af. M47cd computes `double(Stopwatch.ElapsedTicks)/double(Stopwatch.Frequency)*1000`. Thus host dt is not audio position, not T−previousT, and not the normalized F2c22=dt/16.666666666666668 stored later. Detailed limiter policy is outside scope.

M43a0 can loop back to0ca at820 under its remaining predicates. Its old local counter is compared with30 after increment, permitting up to31 reached passes including the first. Thus a component update and clock processing need not occur only once per host callback. F2c97 is unchanged by that inner loop; later passes have d=0 but still sample/correct P. Draw-stage counter increments also happen on an early rendering skip, so it is not proof of a presented frame.

E07's M28e2 uses separate T reads at017 and03c for its bounds. M28e3 enumerates candidates and calls virtual object updates, circle empty base versus slider M1449. E08/E09's draw route is M46a6→M46fe→PlayM2287→M2288→M28e7→M2cf0→M4738/M4518→M4519. M2cf0 separately samples host time at2c6 and T at2cd for its presentation-queue admission (E08); this is not the candidate list. Later M4519:026 calls clock selector M4736: F2ce6!=0 reads T at008, otherwise reads host F2c3a at00e. Circle components use clock1. Candidate admission, eligibility, animation evaluation, submission and actual pixels remain distinct; none is promoted to observed first visibility. There is no shared immutable time snapshot proven across these stages.

M3b25 before its main call stores T→F235a. Afterward q==2 becomes1 at05c following opaque M3b66(false). It calls other helpers, and (outside excluded application mode19 and Play-not-ready branch) stores smoothed delta `r4(0.9*oldF2371 + 0.1*double(wrap32(T-F235a)))`, may set q=0 if receiver absent/inactive and !C, then stores F235d=double(wrap32(T−F235a)). These post-clock operations matter: q seen by Play need not equal q that selected smoothing earlier in the same pass. Related helpers are not assumed side-effect-free.

## E10-E — Loading, holds, seeking, pause and rate changes

CONFIRMED directly involved branches; complete UI and replay state machines are outside scope.

- Loading/readiness M2269 can overwrite T after the host clock. On its not-ready path, F2c73 true and F13f1>=F13f6 selects F13f1 at1ef; otherwise −F13f6 at208. It copies F2359 and returns true, causing M226b to return before candidate refresh. On its completion path, M0709 at154 precedes F13d8=true at15a, F234d=true at160, reset M3b4f at165; when F13f6>F13f1, M3b59(−F13f6,true) follows. Other readiness predicates stay as E07, not an invented single boolean.
- Reset M3b4f sets T=−O, F2359=0, H=0, hold=true, optionally calls M3d64 (pause then seek0), then q=0. F2359 and T therefore can immediately differ. It does not update I/X/O as a single reset transaction.
- Synthetic initialization M3b59 sets C=true; T=I=F2359=target; X=double(target); hold=false; F235e=double(O); if its boolean is false, q=1, otherwise q is unchanged. M3b5a only clears C. C is not synonymous with paused.
- Seek M3b3a(request,bypassBounds,force) returns early if I==request AND T==request AND !force. It calls opaque helpers and sets F1bb8. Bounds: unless bypass, lower clamp to−O applies only when !C; if that clamp is taken, the upper test is skipped. Otherwise upper clamp is duration−O. In C branch, if request>X, increase F235e by request−X; set X=double(wrap32(request+O)); set T=wrap32(i4(Round(X))−O); call M3b24. If C is now false OR force, then for q!=0 issue native seek only when Abs(wrap32(request−T))>200 OR force, set q=2, call M3b24 and copy T→F235a. For q==0 store H=request, hold=true and call M3b24 instead. Finally opaque M3b66(false). C can change inside the first main-clock call; do not reuse its entry value.
- Hold release M3b39, when hold true, seeks `H+(H==0?0:O)` in int32, clears hold and C, calls M3b24 and captures previous T. M3b48 releases hold, plays(true), then q=2. M3b52 on C sets q=1 if needed and returns; on !C releases hold, plays(false), then q=2. A reached M226b block calls M3b5a, optionally forced seek if T>1000, then M3b52 at916 before candidate refresh. Its preceding gates must pass; it is not an unconditional resume every update.
- Toggle M3b49 in C changes q1→0 with M3b56, or any other q→1 with M3b57. In !C, null receiver or active==true selects q=0, M3b56 then optional pause. Active==false selects hold release, M3b57, q=2, Play(false). Native M437c first tests active; M437d has an F24ae guard and uses BASS_ChannelPause on its active branch. Play M437e invokes BASS_ChannelPlay. Native activity M4385 accepts states1 or2 by unsigned `(state−1)<=1`; its user-facing meaning beyond the import is not invented. M2262's pause-related branch calls M3b49 at20b only when M3b0d(q==0) is false. Full focus/pause-menu/countdown wiring remains UNRESOLVED.
- Native seek M437f divides double milliseconds by1000 then calls M314b; M314b converts seconds to bytes through M3143 then invokes M314e BASS_ChannelSetPosition(handle,bytes,0). No driver seek accuracy is established.
- M43a1 is an optional recorded-input/frame branch, not the ordinary no-recording control. It locks F111a; requires frame/list, Play/recording/readiness and index/count predicates. When F2a7e is true at2c9, selected frame.F1310 is copied into T at2d8 without copying F2359. Preserve it as a possible later writer, not a promise every replay overwrites the clock. The focused ordinary-live prediction explicitly assumes F2a7f null.

Rate: M3b37 returns a double percentage. M3b38 clears lastCorrectionHost to0, calls receiver.M3d68(!F236f), then receiver.M3d66(rate). On T08e4 the policy setter calls base and reapplies M438d; the rate setter stores F2a56 then reapplies. If policy is false, M438d sets native attribute1 to `r4(double(F2a51)*rate/100)` and attribute65536 to0; if true, attribute1 gets F2a51 and65536 gets `r4(rate−100)`. These literal branches are confirmed, but naming the flag as a complete pitch/sync policy would exceed recovered evidence. Native success is not tested here.

M3b45 reads attribute1 into F2366, defaults to44100 only if `bgt.un` does not take the >0/unordered path, copies to F2365 and resets rate. NaN is not rejected by that unordered-positive test. Other F2366 writers (including M225b/M2272 and M3b96 callback) remain inventoried; this expedition does not generalize an unchanged frequency ratio to every special/failure path.

M224c returns for Play variants6/7 before rate reset. On reached ordinary reset it calls M3b44 (rate100, f=f0), sets F236f from Nightcore bit512, then DT64 selects150 before else-HT256 selects75. M2240 has the same DT/HT priority in its setup path. M223e additionally has relative ×1.5/÷1.5 or ×0.75/÷0.75 branches, so a call is not necessarily an absolute assignment. Rate affects source/native setup, extrapolation and the offset subterm separately. No undocumented formula is imported from a modern implementation.

## E10-F — Discriminating conditional reference predictions

RECONSTRUCTED static expectations. Unless overridden: base ordinary Osu Play, selected modern map, non-Edit, C=false, hold=false, q=1, F136a=false, nonnull valid receiver, finite in-range numbers, no optional frame override or helper/external mutation. Arithmetic wrap and source errors outside those assumptions are not silently repaired. No case was run against original Windows or a replacement model.

| Case | Controlled input / branch | Expected original instruction consequence |
|---|---|---|
| C01 | F2367 null at M3b24 entry | Immediate return; T/I/O/copy not refreshed by this method |
| C02 | X=1000,P=1008,d=8,h=11 | v=1008,e=0; X/I=1008, then offset publication |
| C03 | Same epoch, X=1000,P=1008 | d=0; v=1001,e=−7; I=1001. Zero extrapolation does not freeze clock correction |
| C04 | q!=1 or X==0 or P==0 | Snap X=P before round, irrespective of ordinary small-error policy |
| C05 | F136a true, remaining normal route | Snap; ordinary initialization writes false but not immutable globally |
| C06 | Computed e exactly−h | No forced snap; v branch (strict e<−h fails) |
| C07 | Computed e exactly+h | Half-delta branch; updates correction host time |
| C08 | Computed e=−2h / +2h | No forced snap; twice-delta / half-delta respectively |
| C09 | Abs(e)>2h | Snap P; f==f0 also calls M469d, without changing this snap choice |
| C10 | host−last=1499 vs1500, rate100 | h11 vs33; at rate75 it remains11 even at1500 |
| C11 | C=true,F234d=true,q1,d=59 vs60 | X advances59 versus does not advance; this is not a frame-cost measurement |
| C12 | C=true,F234d=true,q0 | No synthetic X advance; independent host stopwatch can continue |
| C13 | C=true,F234d=false | Reassign X from upper-first clamp before optional synthetic increment |
| C14 | hold=true,C=false,H=1234,O=20 | X/I/T=1234; offset still recomputed but subtraction bypassed |
| C15 | q0, oldT0, P=1000,newO20 | Snap I1000; T1000 rather than980 on this publication |
| C16 | q0, oldT nonzero, P=1000,newO20 | Snap I1000; T980. With constant source/offset it stays980 on subsequent main calls |
| C17 | I1000, modern map offsets sum30,U10,F2389=false,rate100 | O20,T980 on normal subtraction route |
| C18 | Same as C17,rate150, f=f0 | O15,T985 if I is independently fixed1000; source/extrapolation are not asserted unchanged naturally |
| C19 | Version4 vs5, non-Edit; all other O terms zero | B24 vs0; same I gives T=I−24 vsI |
| C20 | Edit1, map offsets30,version4,U10,flagfalse,rate100 | B/M0 but setting term remains; O−10 on reached branch |
| C21 | O20 then M3b4f | T−20,copy0,H0,holdtrue,q0; similar fields disagree |
| C22 | Seek q1,!C,difference200 vs201,!force, bounds valid | No native seek vs native seek; both still set q2 and call main clock, unless early-return condition |
| C23 | Seek I=T=request,!force vsforce | First returns immediately; forced call continues into discontinuity logic |
| C24 | Hold release H0 vsH500,O20 | Source seek0 vs520; exact zero special case retained |
| C25 | C toggle q1 vsq0; or native active true vsfalse | C stops/starts q; native pauses/plays(false), with intermediate q2 on resumed branch |
| C26 | M224c ordinary DT64 and HT256 both present | DT branch150 wins; HT alone75; variants6/7 return before this reset |
| C27 | T08e4 vsT086e with matching nominal times | Native sample conversion versus F2615 accumulation; no guarantee equal, no-audio fallback is legal factory output |
| C28 | Not-ready M2269, F2c73true,F13f1>=F13f6 | T/copy becomeF13f1 after host clock; method returns and candidates are not refreshed |
| C29 | Other reached not-ready branch | T/copy become−F13f6; ready transition later resets/possibly initializes synthetic branch |
| C30 | One normally returning registered host callback | Clock before reached candidate/object work; draw before timing-tail refresh; startup has prior timing seed |
| C31 | M43a0 loops without draw-epoch change | Each reached pass samples/corrects; after first, d0; separate candidates/visual time reads not one immutable snapshot |
| C32 | Reached M43a1 F2a7e frame write; or clock selector0/1 | T can change while F2359 does not; selector0 uses host time, selector1 uses T. Neither pair is guaranteed equal |

Additional numerical boundaries: negative d satisfies d<60 in synthetic branch; no lower clamp to0 is present there. M2d7c tests upper before lower, so inverted bounds are not repaired. All int add/sub/neg sites here are unchecked CIL, not saturating time arithmetic. Abs(int.MinValue) and conv.i4 non-finite/out-of-range behavior depend on original runtime and are not assigned fabricated results. Offset and source rate reads can differ between calls; a 150% extrapolation prediction also requires the stated frequency ratio.

## E10-G — Remaining boundary and smallest Windows experiment

UNRESOLVED: actual source receiver per run, actual native/stopwatch values and callback cadence, output-device latency, target-CLR precision/conversion boundaries, setting U/map-offset user-facing identities, full F237b/F2389 policies, complete pause/focus/resume/recorded-input state machines, error/native success handling, and all judgement time-source choices. We have a static clock-to-candidate/visual chain, not a measured audio-to-pixels chain. E09 projection/vertex-attribute/viewport work stays deferred. No stutter cause or optimization benefit follows from timing helpers or later preparation sites.

Minimal original-Windows validation, not executed:

1. Copy the hashed installation; record Windows/CLR/process architecture, audio device/driver, selected map/audio hashes, mode, mods, offset values, frame limiter and display/capture conditions. Use one ordinary audio-backed map with a circle and straight slider, no replay or special Play. Log concrete F2367 type/handle. A silent control alone cannot verify an audio-backed source.
2. Attach managed/native instrumentation to the original process. Resolve method tokens to current JIT addresses; never use CIL offsets as native addresses. Log entry/exit sequence, thread ID and independent monotonic timestamp at M4241, M3b25/M3b24, M4384 or M4057, M2269, M28e2, M28e3, M4519 and M46a1. At M3b24 log P plus X/I/O/T/F2359/F235a/q/C/hold, rate, frequency operands, F2c97/F237d, hostNow/dt and lastCorrection. Instrument the original sample rather than making extra BASS/clock calls that alter observations. Preserve separate read/write order and exceptions.
3. Record a short uninterrupted pass, then one pause/resume with no other setting change. Verify source pauses versus host advancement, q transitions, hold/seek calls, ordering and repeated inner passes. Keep naturally reached behavior separate from debugger-forced state tests. Correlate existing BASS byte/seconds sample with original T; that is reported playback position, not heard sound. Add loopback audio capture only if testing acoustic/output alignment.
4. Repeat with one controlled nonzero offset setting, logging U/map terms to establish which setting changed; separately test DT or HT while holding the map/settings fixed. A forced seek or debugger source discontinuity is a separately labelled experiment, never disguised as ordinary live play. Check error threshold equality and same-epoch repeated calls using original-method breakpoints in an isolated test copy; no replacement scheduler.
5. Compare uninstrumented and instrumented cadence, cold/warm runs and log loss. If extending to owner-reported stutter, additionally trace candidate admission, M144b preparation entry, native batch boundary, capture first visible pixels and frame duration with overhead controls. This future measurement is not performed here and does not authorize pre-rendering/caching implementation.

Smallest subsequent gameplay-focused static investigation: trace **one ordinary hitcircle's live input timestamp into its first hit-window comparison**, resolving whether it uses F2358, a different sampled time or an offset-adjusted event timestamp. Keep judgement output narrow; this connects the recovered clock to an actual player action without assuming every judgement shares it or broadening into scoring. The original-Windows clock trace remains the necessary dynamic validation dependency.

# 3. Pseudocode

RECONSTRUCTED independent descriptions, grounded in CONFIRMED static sites above. These are documentation only, not executable code or an implementation specification for unresolved helpers. Field aliases mean fresh reads where used, not a function-entry snapshot. `i32` means original unchecked int32 arithmetic; `i4` means original conv.i4; `Round` is the original System.Math.Round call; ordered comparisons are false for NaN. Native/helper results are inputs, not substitutes. Opaque calls retain side effects; projections omit unrelated audio/input/UI work explicitly.

## Main clock projection

```text
M3b24():
    if F2367 == null: return
    F2367.virtual_update()                         // M3d63
    if M3b22():                                   // F237b, policy UNRESOLVED
        if T < i32(-O) or T > i32(duration_i4() - O):
            if !C and q != 1:
                C = true
                F2367.pause()
        else if C:
            C = false; hold = false; H = -1
            if q == 1:
                if !F2367.active():
                    F2367.seek(double(i32(T + O)))
                    F2367.play(true)
            else:
                F2367.seek(double(i32(T + O)))

    P = F2367.position_double()                    // one position sample
    if !C and q != 0:
        if F2367 == null or P == F2367.duration_double: q = 0
    r = (M3b37()/100.0) * conv_r8(CIL_div(f, f0))
    d = F2c71*r if F2c97 != F237d else 0.0
    F237d = F2c97                                 // second epoch read

    if C:
        if !F234d: X = double(upper_first_clamp(T, i32(-O), i32(duration_i4()-O)))
        if q == 1 and ordered(d < 60.0):
            F235e = F235e + d
            X = X + d
    else if hold:
        X = double(H)
        F237c = F2c3a
    else if F2367 != null:
        snap = (q != 1 or X == 0.0 or P == 0.0) ? true : F136a
        if !snap:
            v = X + d
            v = v - (v - P)/8.0
            e = v - P
            if i32(F2c3a-F237c) < 1500: h = 11.0
            else if ordered(M3b37() < 100.0): h = 11.0
            else: h = 33.0
            if ordered(Abs(e) > 2.0*h):
                snap = true
                if f == f0: M469d()                // side effect retained
            else if ordered(e < -h):
                X = X + d*2.0
                F237c = F2c3a
            else if ordered(e < h): X = v
            else:
                X = X + d/2.0
                F237c = F2c3a
        if snap:
            X = P
            F237c = F2c3a
    else: X = 0.0

    I = i4(Round(X))
    // Selected-map calls below are separate original reads, not a cached map.
    B = 24 if selected_map()!=null and selected_map().version<5 and mode!=Edit1 else 0
    z = 0.0
    if F2389: z = z - 15.0
    M = 0 if mode==Edit1 or selected_map()==null else i32(selected_map().F0ff9 + selected_map().F0ff4)
    sum = double(B) + double(M)
    z = z - double(M3df2(F03de))
    O = i4(sum + z*(M3b37()/100.0))
    if hold or (q==0 and T==0): T = I
    else: T = i32(I-O)
    F2359 = T                                     // separate publication

upper_first_clamp(value, lower, upper):            // M2d7c exact priority
    if value > upper: return upper
    if value < lower: return lower
    return value
```

## Playback and interruption boundaries

```text
CONFIRMED static / RECONSTRUCTED aliases:
T08e4.position():
    handle_for_conversion = M3d5d()
    bytes = BASS_ChannelGetPosition(M3d5d(), 0)
    return BASS_ChannelBytes2Seconds(handle_for_conversion, bytes) * 1000.0
T086e.update():
    if F2c97 == this.F2616: return
    if this.F2614:
        this.F2615 += F2c71 * this.F2617 / 100.0
        this.F2616 = F2c97

M3b4f_reset():
    T=i32(-O); F2359=0; H=0; hold=true
    if F2367!=null: F2367.pause_then_seek_zero()    // M3d64
    q=0
M3b59(target, preserve_q):
    C=true; T=target; I=target; F2359=target; X=double(target)
    hold=false; F235e=double(O)
    if !preserve_q: q=1

M3b3a(request, bypassBounds, force):
    if I==request and T==request and !force: return
    if mode==Edit1: M3b55()                       // opaque
    M3b65(); F1bb8=true
    if request<i32(-O) and !bypassBounds and !C: request=i32(-O)
    else if request>i32(duration_i4()-O) and !bypassBounds:
        request=i32(duration_i4()-O)
    if C:
        if ordered(double(request)>X): F235e += double(request)-X
        X=double(i32(request+O))
        T=i32(i4(Round(X))-O)
        M3b24()
    if !C or force:                              // re-read C after call
        if q!=0:
            if Abs(i32(request-T))>200 or force:
                F2367.seek(double(i32(request+O)))
            q=2; M3b24(); F235a=T
        else:
            H=request; hold=true; M3b24()
    M3b66(false)                                 // opaque, retained

M3b49_toggle():
    if C:
        if q==1: q=0; M3b56(); return true
        q=1; M3b57(); return false
    if F2367==null or F2367.active():
        q=0; M3b56()
        if F2367!=null: F2367.pause()
        return true
    M3b39_release_hold(); M3b57(); q=2
    F2367.play(false)
    return false
```

## Managed ordering and consumer interface

```text
RECONSTRUCTED reached-path ordering; scheduler cadence UNRESOLVED:
startup M4247:
    virtual_initialize()
    virtual_timing_tail()                        // seed before backend Run
    backend.virtual_Run()
registered M4241 callback:
    if this.F2983: return
    virtual_update()                             // T095e.M469e
        ... earlier helpers/queues ...
        M43a0(delegate M46d7):
            reached pass:
                M3b25():
                    ... pre-helper ...
                    F235a=T
                    M3b24()
                    if q==2: M3b66(false); q=1
                    ... post-helpers/readiness-gated delta publications ...
                optional M43a1()                 // can overwrite T
                ... process/drain queued callbacks ...
                M46d7 -> component ForEach -> virtual M1ab2:
                    ordinary Play M226b:
                        if M2269_loading_or_readiness(): return
                        ... other gates/possible seek or start operations ...
                        manager.virtual M28e2()   // candidate bounds read T
                        ... separate gates ...
                        manager.virtual M28e3()   // object updates
                ... may loop to reached pass, bounded by original counter ...
    virtual_draw()                               // T095e.M46a6
        ... candidate presentation preparation, component draw ...
        M4519 -> M4736 -> read T or hostNow       // later separate time read
        F2c97++                                  // also on early-skip path
    virtual_timing_tail()                         // M46a7 -> M46a1
        sample = Stopwatch_elapsed_ms_with_original_limiter()
        F2c3d = F2c3f
        F2c3f = sample
        F2c3a = i4(Round(F2c3f,0))
        ... cadence flag updates ...
        F2c71 = F2c3f-F2c3d
        F2c22 = F2c71/16.666666666666668

UNRESOLVED:
    actual callback cadence, native timing/error results and heard audio alignment;
    complete interruption/UI/recording policies and all input/judgement clocks;
    first visible pixels and E09 projection dependency;
    actual frame costs, distorted-slider stutter cause and caching benefit.
```
