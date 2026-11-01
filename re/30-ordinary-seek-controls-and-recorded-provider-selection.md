# 1. Result

For an osu! player, Skip is a guarded request, not a general rewind command. The key handler can queue it before the eventual callback decides whether a seek is permitted. Recorded gameplay follows a different selection path: it can supply button states and playfield cursor coordinates, and a newly selected frame can replace the published gameplay time. Live keyboard controls still have an original route through that processing. Neither operation automatically undoes earlier hits or restores the entire map.

**RECONSTRUCTED player-facing consequence of CONFIRMED original managed instructions.** No original Windows seek, replay, input, map import, audio behavior or displayed effect was observed. Work 2 remains suspended; `reconstructed/` was neither inspected nor modified. No authorised exception.

## Objective, provenance and execution boundary

E30 recovers the smallest control/provider-selection chains leading to seek, recorded-frame time/input, admitted data and the next ordinary gameplay consumers. It distinguishes application Play, concrete Play subclass, osu!standard ruleset, physical pointer provider, recorded record, selected frame, pending skip, and pending selective restoration. No replacement input/clock/replay/seek/gameplay model, full replay/file/network format, menus/retry, score/HP/rendering or lifecycle system is implemented.

Read the standing contract, cumulative journal through E29, relevant E03/E07/E10–E12/E15/E19/E20/E27–E29 and necessary E05/E13/E23/E24–E26 identities and original instructions. Preserve ordinary/Taiko dispatch, E12's update rejoin, E14 resource ordering, E24's spinner identity and E29's retained-state distinctions. Earlier records are unchanged.

Dedicated **work1/expedition-30-controls-and-recorded-provider** was created directly from remote **9add1f75ce1a8ae619a00db09a5ac8867ee740c0**, tree **3967a32ed17d90194b44f439b322721a83827c29**. The local mirror has the same tree and a different commit identity; publication explicitly uses the required remote parent. Preserve all 136 other baseline files, every prior fixture, the entire E20 packet and the exact old journal prefix. Final handoff supplies the independently verified publication commit/tree/branch/scope.

**E20 remains incomplete, preparation-only. All 17 original-runtime cases are NOT RUN — WINDOWS HOST REQUIRED. Every E30 case below is also NOT RUN.** The owner’s future Windows PC does not establish access to an authorized original process, display/input/audio, CLR debugger/profiler or instrumented callback now. Available host is Linux x86_64; no wine/dotnet/pwsh/powershell/DISPLAY/WAYLAND or supplied authorized Windows endpoint. Decoder dependencies were absent at task start and again after session resumption; pinned dnfile0.18.0/dncil1.0.2/pefile2024.8.26 were installed before successful static extraction. No original EXE, CLR or native call, input event, import, capture, overhead measurement or surrogate gameplay execution occurred.

## Reference integrity, evidence and reproducibility

| Actual input rechecked | SHA-256 |
|---|---|
| osubackup.zip; 18 entries, valid ZIP CRC | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| Archive osu!.exe | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Supplied/repository Instruction.md | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |

Self-declared Stable40/file-version1.3.3.8; exact official dated authenticity/modification status remains UNRESOLVED. These hashes identify the supplied bytes, not an officially authenticated release.

New report, `tools/control_provider_evidence.py`, `tools/check_control_provider.py`, `evidence/30/control-provider.json`, and the append to `contexts.md` are the only changed paths. The extractor reuses unchanged E04/E05/E15/E25 metadata/CIL collectors. It records 124 selected methods, 34 focused-writer body identities, full watched direct field/caller inventories, interface/ancestry/slot metadata, branch/constant/arithmetic/local sites and original enum identities. Full EXE scan: 19659 decoded bodies, 1739 zero-RVA methods, zero decode errors. Selected branches land on decoded instruction boundaries. Only bounded sites from the large M477d/M114f/M2252 methods are published; full original bodies/disassembly remain private. No proprietary executable/assets, private trace, dump or full disassembly is distributed.

Evidence identity: **807252 bytes, SHA-256 `d6d0444bd9af2ea86e68026679e8e44047fdd0a3261601931d3f35e9ea1a9e9a`**. Offsets in this report are hexadecimal, relative to the first CIL opcode, excluding headers; JSON offsets are decimal. Mxxxx/Fxxxx/Txxxx denote MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx, not invented source names or JIT addresses.

CONFIRMED denotes original static instructions/metadata. RECONSTRUCTED denotes semantic aliases, conditional predictions and independent non-executable pseudocode. HYPOTHESIS denotes unverified explanations; UNRESOLVED denotes missing original evidence. Direct-store inventories do not exclude reflection, native/external mutation, callbacks or reentrancy.

Competing explanations tested statically: queued control versus immediate seek; arbitrary clock API versus natural ordinary control; frame-list existence versus active selector; one provider object versus independent physical and recorded stages; audio seek versus T-only overwrite; all live input disabled versus retained controls; automatic index rewind versus forward selection; packet arrival versus actual restore; application Play versus ruleset identity. Original stack/branch and reference propagation discriminate these; no conventional replay implementation is proof.

```sh
python re/tools/control_provider_evidence.py /private/osubackup.zip > /private/provider-a.json
python re/tools/control_provider_evidence.py /private/osubackup.zip > /private/provider-b.json
cmp /private/provider-a.json /private/provider-b.json
cmp /private/provider-a.json re/evidence/30/control-provider.json
python re/tools/check_control_provider.py re/evidence/30/control-provider.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

Publication verification comprises deterministic repeated extraction/public equality, focused assertions, all 27 prior research checkers, unchanged E20 package checks, Python compilation, historical prefix/blob/mode/fixture preservation and remote ancestry/tree/head/scope/file verification. Focused validation passes for124 methods and333 historical body-hash comparisons; the27 prior checkers and unchanged E20 package pass. Final handoff supplies remote verification after publication; those static checks do not satisfy runtime acceptance. A draft assertion placed M1df7's M2255 call at00dc; original CIL has the call at00d7 and a branch at00dc. Corrected before PASS. A private read of an in-progress empty extraction file raised JSONDecodeError; it was not original-game execution or evidence.

| Method / token | Original body SHA-256 |
|---|---|
| M4390 / 06004390 | `dbd76ef425afff17b2f4fb1961aa077098fa3f4de40ff193137a106f0de16e69` |
| M4391 / 06004391 | `38998cd44335c49e648264282354f631fc2018399bf614671952cc0c643c6d16` |
| M2256 / 06002256 | `cfaba1153746052ac0e048f97be23edebacbfccb9be109fbc553262e5db4631c` |
| M224d / 0600224d | `1b258f08543eb9f9a372468c7debd5def1af9f9b0975684d1c469e6d7de2932d` |
| M4abc / 06004abc | `c12959bd10ade0bec707a269569c3e05a8382e63af221803c9a6ae8918f2ffef` |
| M4ac1 / 06004ac1 | `5b05231bd7005276eb4351c2a0239737d4581df5dd6e54efcabe5fc56d05dbea` |
| M43a0 / 060043a0 | `11fbe0877b6a113594d3b9cf45c19deeb2d8681e2ed8bb2f37e2e54e3ce940a4` |
| M43a1 / 060043a1 | `edfa1f3182083b8b5a88fadab23c8945846f38a381652e8ad4b6245529807008` |
| M1df7 / 06001df7 | `10a54d989a4fba2a3ba8189394cb14cccb064ace83766a64ae39378c70979bbe` |
| M1dfd / 06001dfd | `ae01e0982ee12662757f2b00c4bc3da3751cb062a1978200e1a0cee4b232a973` |

# 2. What they found

## A. Separate selectors and original identities

Ordinary osu!standard remains **T0253/M113e → virtual M1141 → T00e8/M0651(true)**. The manager/factory and object identities from E05–E29 remain authoritative. A recorded-input Play subclass can use this ordinary ruleset; it is not Taiko and need not be base Play T047b.

| State | Original type/signature | First relevant meaning, RECONSTRUCTED |
|---|---|---|
| T08e5.F2a7c | Boolean `0602` | Global recorded-input selector; M4390 reads it directly |
| F2a7f | T059e reference `06129678` | Selected source record; distinct from global Play record F13c3 and handler F05f3 |
| record.F1980 | List<T0463> `06151282c10112918c` | Mutable ordered frame references |
| F2a7a | Int32 `0608` | Current frame index, not gameplay time |
| F2a9f | T0463 reference `0612918c` | Selected frame |
| F2a7e | Boolean `0602` | This processing invocation advanced a frame, where its clearing prefix is reached |
| F2a80 | Boolean `0602` | Admitted/streaming-state branch, not synonymous with F2a7c |
| F2a82 | Boolean `0602` | Separate end/failure-associated gate, with independent writers |
| F2a6f | Boolean `0602` | Host's live-path selector, recomputed before frame processing |
| F2a78 | Boolean `0602` | Separate recorded/auxiliary policy; default true, special T0466 constructor false |
| F2a90 | Boolean `0602` | Reached input/component/manager eligibility operand, not merely “provider exists” |
| F2a93 | T03fc interface reference `06128ff0` | Physical pointer/button provider selected separately |
| F2a5c / F2a7b | Single-vector T0a82 | Stored provider/viewport pointer versus mapped playfield pointer |
| F2358 | Int32 | Corrected/published gameplay time, later overridable by selected frame |
| Play.F13f2 | Boolean | Original generated/special-input initialization branch, not interchangeable with F2a7c |

**M4390/06004390**, T08e5, static Boolean/no arguments, simply reads F2a7c at0000 and returns. **M4391/06004391**, static void(Boolean), stores the supplied selector at0001. If its subsequent selector read is false, it stores null to F2a7f:000e and F2a9f:0014. True does not create a record or frame list. Neither arm resets index, F2a7e, F2a80, F2a78, clock or chart objects. A helper exception/intervening mutation is not made transactional.

Managed defaults of otherwise uninitialized static fields are not evidence of the active configuration. M438f explicitly initializes F2a6f/F2a78/F2a79 true, but not F2a7c. Full direct writers/readers are in the JSON; the focused writer table below enumerates all stores for the key fields, not just scoped ordinary callers.

### Complete direct-store inventory for the selected provider state

| Field / original signature | All original direct stores (hex CIL offsets) |
|---|---|
| F2a7c / `0602` | M4391:0001 |
| F2a7e / `0602` | M43a1:0040/0149 |
| F2a7a / `0608` | M071a:0065/0091/009f; M21b5:0176/01d8/0224/02be; M2266:02c1; M43a1:0143/01a6; M4abc:0024 |
| F2a7f / `06129678` | M071a:0057; M1dfd:0085; M21b1:002e; M21b5:0119; M2237:03c2; M223b:03ab; M225b:028f; M25be:005d; M32e3:00aa; M32e5:006b; M4391:000e; M46ba:0379 |
| F2a9f / `0612918c` | M4391:0014; M43a1:02bf |
| F2a80 / `0602` | M0dc4:0044; M1dfb:009e; M1dfd:006a; M2571:0116; M2d96:0046; M32db:015f |
| F2a6f / `0602` | M438f:001f; M43a0:0040 |
| F2a78 / `0602` | M21a2:001b; M21a8:0010; M438f:002f |
| F2a90 / `0602` | M43a0:00d4; M43a1:0046/02de/08d0 |
| F2a93 / `06128ff0` | M43a0:0059; M43b2:0036 |
| F1310 / `0608` | M219b:0026; M219d:004d; M21af:00c0/00fb; M4abc:00e9/00ff/0179/017f |
| F1980 / `06151282c10112918c` | M0c78:000a/02af; M114f:0033/0a51; M1ef7:000a/0338; M223b:038b; M2258:00bd; M226e:0031; M2b3d:006c; M2b3e:00ab; M2b3f:009b; M2b5e:0020; M2b63:0002; M4622:000a/01a8 |
| F13f2 / `0602` | M2237:0227/0381 |
| F13ad / `0608` | M224b:00d9/0164/019e; M2252:019d |
| F13ef / `0602` | M224c:00cc |
| F13f1 / `0608` | M224d:0002/0014/0032 |
| F13f6 / `0608` | M224d:0043/0060/00c6 |
| F1120 / `0608` | M1df7:0268; M226b:10b7 |
| F1121 / `06119d00` | M1df7:0273 |

Other phase, parser, record and list mutations have separate inventories in `entry_field_sites`; a List.Add/Insert/Clear can change an object without a new stfld to its owning field. Body hashes for all focus-field writer methods are preserved even where their full bodies are outside scope. No claim that every listed UI/editor/other-ruleset writer is naturally reached in ordinary Play.

## B. Application-mode selection and initialization

**M46b0:02dd–035a**, after its existing request/commit prefix, uses this priority in the application Play2 case:

1. Previous mode F2c70==MatchSetup12 OR nonnull F14a8 selects **M23f0/T04af**.
2. Otherwise F2c73 selects **M0715/T00ff**.
3. Otherwise **M4390 true OR F2a80 true OR (F0cc6 & 2048)>0** selects **M4ab9/T0a10** at0341.
4. Otherwise select **M222e/T047b** at0350.

Original metadata identifies Mods.Autoplay=2048. This does not establish a human name for every T0a10 use. These are concrete constructor predicates at commit time, not proof that a pending M46ad request was accepted or committed. E03's deferral, veto/redirect, duplicate/pending guards and callbacks remain required. Changing a selector after construction does not retroactively change the current object's runtime type.

M4ab9 calls base M222e; M4abf calls base initialization M2237. In inherited M223b's Osu0 switch, M2234 false selects **M113e/T0253** at004a, true selects its E05 alternative M0da8. Recorded flags do not change that switch into Taiko1. Thus a statically supported configuration is T0a10 Play with ordinary T0253/T00e8 gameplay.

### Initialization is virtual and can replace the frame list

M2237 first records F13cc=!M4390. If recorded selector is true, it chooses ruleset mode from source record.F1971 (null source supplies0) through M2233. Later it initializes F13f2=false:0227. Its original local condition at0315–0349 is `!F2c73 && Autoplay && previousMode==Play2 && !F13a3`. If that local is true, OR recorded selector is false and an Editor/Autoplay/F137c branch applies, it sets F13f2=true:0381. In that branch, if !F2c73&&!F137c it enables M4391(true):0395; optional local-true stores current T to F2a81. It constructs a source record through **M2e1c(mode, original decoded name, selectedMap, false):03bd**, stores it to F2a7f:03c2 and then assigns its modifier wrapper. Existing recorded state can bypass this branch and retain its supplied source.

M223b calls virtual **M224a:025a** if recorded selector was true, initializes handler through M0aaf:0265, then calls virtual **M224a:0272** if a fresh selector read is false. These are two conditional tests, not one immutable branch. Ordinary handler M0aab=false supplies F2a71 through M0aaf:003d.

| Concrete Play | Resolved M224a and reached effect |
|---|---|
| Base T047b | M224a: unless F2c73/F137c/F137b, calls M4391(false):0016; then M1dff. M1dff retains outgoing-data/helper work and M1dfb(false); it is not a no-op. |
| Recorded T0a10 | **M4abc**, reuse-slot override: create original auxiliary control T00ed, clear F2c73, F2a7d and **index=0:0024**, clear aggregate button fields through M43a4:0029. If !F2a80 call M1dff; streaming arm skips it. No M4391(true) here: selection and source must already be valid. |
| Editor-derived T00ff | Inherits M224a; its later M071a toggle is separately restricted, below. |
| T04af | Inherits M224a on this slot; its other control/input overrides remain distinct. |

M4abc optionally aliases source to global record under F2a81>0 then reconfigures it. It can also normalize exceptional first-frame time ordering: for count>=2 and frame1.time<frame0.time, assign frame1.time=old frame0.time then frame0.time=0; for count>=3 and frame0.time>frame2.time, separately assign frame0/frame1 from frame2.time. Those are original mutable frame objects, not a sorted immutable schedule. No universal monotonicity follows.

On F13f2 true, M223b invokes **handler.virtual M0abd:036e**. Ordinary T0253 resolves **M114f**, an original generated-frame producer: replaces F2a7f.F1980 at0033, creates T0463 frames with M219b and inserts via M114d/M114c. Its first supplied times are−100000, firstObject.start−1500 and firstObject.start−1000. It can mark pre-F2a81 objects evaluated at011f. Stop at producer identity/prefix and original append/index helpers; complete automatic-input generation is outside scope. If F137c&&!M4390, a separate empty global frame list is created; otherwise global record is aliased to F2a7f at03ab. Generated input is not necessarily a parsed replay file.

M2258 separately configures the global/handler records. With selector true and nonnull source, it obtains relevant mode/modifier state from that source; **only !F2a80** copies source.F1980 into global record.F1980:00bd. Streaming can therefore keep distinct lists/record identities. E23 policy selection remains separate. An ordinary recorded source with empty frames and !F13f2&&!F2a80 reaches M223b's rejection/other-mode request and M4391(false):0344, rather than proven gameplay entry.

### A separate failed-session control can enable recorded selection
+
+Base M225b also has a concrete, narrower route at0256–02a8: after the other control branches, require paused, selector false, failed latch true and nonnull global record. Numeric key112 calls **M2267:0285**; only after that original helper returns does it alias **F2a7f=F13c3:028f**, call **M4391(true):0295**, set F2a82=true:029b and request **M46ae(Play2,true,false):02a3**. This is a request using an existing record, not a backward seek or M226e snapshot replacement. The prefix can interrupt/exit; the request retains original transition guards and does not prove a new instance was committed. Key113 takes a different record-helper path, not this selector store. No natural live replay-toggle shortcut during unpaused ordinary gameplay follows.
+
+### Active type/provider changes have bounded special routes

T0a10 M225b resolves **M4abd**: two original key70/72 branches operate on its auxiliary state, other keys call base **M225b:0043**. T0a10 M226b resolves **M4ac1**: not-ready delegates immediately to base; ready streaming state tests F2a5a against F1373 and can request virtual M2261, updates its auxiliary control, then calls base M226b:0051. This is an additional buffer/pause-request bridge, not proof immediate pause/audio stop. Requests retain E28 pending consumption/cooldown/handoff behavior.

T04af resolves Skip to **M23f3**; it can call base M2256 under F13eb or positive F1375 but otherwise has its own permission/request behavior. Its T0466 helper can change source/selector and reposition frame indices, including backwards scans and inserted transition frames; these are not the normal M43a1 algorithm. T00ff **M071a** toggles only if requested selector differs, F2c73, readiness, and T<last stored object end; enabling selects F035b, advances toward time then subtracts2/clamps0, and marks earlier full-list objects evaluated. That is an editor-derived explicit provider transition, not ordinary rewind-on-clock-change. Do not transfer it to base Play.

## C. Ordinary control-to-seek reachability

### Recognized Skip versus eventual admission

E28 registration **M2246:002a ldvirtftn M225b → M3794** remains the control boundary. New physical keyboard keys still pass M43a2→M379a, subject to focus/global enable, higher consumers and delegate order. Actual configured key for binding Skip19 is UNRESOLVED; numeric binding19 is not necessarily keycode19.

**M225b** accepts this control route only in transition phaseF2c6b==2 and after its readiness wrapper.F0344 succeeds. When not ready, only its Pause/Escape alternative is handled. Its comparison against **M2c4f(19)** at00c9–00d1 calls **M2254(null,null):00d6**. This branch precedes the later paused menu branches and has no own pause/handoff veto. It reports handled after queueing, not after a native seek.

**M2254** captures this Play receiver in M22ad and calls **F2c78.M09af(callback, M2333(0,300), false):001d**. **M22ad:0001 invokes virtual M2256**, discarding its Boolean. M09af constructs a scheduled item with original Stopwatch.ElapsedMilliseconds+converted delay and delegates insertion; it does not directly invoke the callback. M2333 range/end/random semantics are not replaced with a guessed distribution or measured delay.

**M2256**, ordinary base/inherited T0a10 target:

| Original offsets | Gate or effect |
|---|---|
| 0000–001f | Require application Play2, !F1373, !F13be, and this.F13ef. Otherwise return false. No additional direct F1376 or F136a test here; retain upstream callers. |
| 0020–0036 | Only T<F13ad and !F1397 enter the seek arm. Equality or finalized state takes the separate suffix. |
| 003b–004c | Seek arm requires !F136c and Play.F13bb==1; otherwise false. |
| 004d–0078 | Original phase/rate/resource/presentation work, then M223f. Do not declare these helpers side-effect-free. |
| 007d–00bc | Recompute M2257. True chooses target0. False chooses unchecked F13ad+M2333(−80,90). Retain M3b55 between target preparation and **M3b3a(target,false,false):00bc**. Neither bypass-bounds nor force is true. |
| 00c1–00eb | After normal seek return, set F13fb=−1:00c3; retain M1dfa(Skip2,nullable-default), possible handler M0ad3, then unchecked F1375−1. No object reset or rollback inserted. |
| 00f2–012c | No-seek arm may use F13eb&&!F138e to request M2285 and set state6; otherwise return true without seeking. Return true is not proof of clock movement/native success. |

**M2257**: let b=0 if F13f6>=10000 else unchecked−F13f6. True requires non-T04af, T<unchecked(b−(M4390?50:0)), and F13ad>6000. Equal early bound or equal6000 is false. Fresh selector/time reads are retained; this is not one universal “skip to first hit” timestamp.

F13ad has four direct stores: M224b:00d9/0164/019e and M2252:019d. M224b derives a provisional marker from current time or first start minus original timing-point spacing (4/8 multiples depending original beat-length comparison), otherwise firstStart−2000; requires a sufficient gap versus Min(F13f1,−F13f6) and 1200+approach operand, else state2/marker0. A separate intro path in M2252 can replace it with its local endpoint minus6 times a timing operand. Preserve that dependency rather than assigning a fixed constant to F13ad or fully recovering intro presentation.

Original admitted **ReplayAction.Skip=2 → M1df7:00d7→M2255** does not directly seek: if Play reference exists, set F1375=unchecked(Max(0,old)+1). Later **M226b:0398–03af**, before its main pause return, can invoke virtual M2256 when counter>0&&!paused&&!handoff. Also M224b:020b can consume an existing positive counter. A callback can queue Skip, then a later state change veto the seek. Repeated requests can accumulate callbacks/counter state; no idempotent generalized seek command is inferred.

The startup operands have a direct preparation source, **M224d**, called by loading M223b:024d. F13f1 starts0:0002; F2c73 replaces it with F2c74:0014; otherwise recorded selector plus F2a81>0 replaces it with unchecked(F2a81+5):0032. F13f6 first copies selectedMap.F0fc4:0043, then Max(−eventManager.F1bb1, old):0060, then Max(original approach-dependent minimum minus first stored start or0, old):00c6. For ordinary non-Mania that minimum is Max(1800,manager.F0adb); Mania uses1800. Keep signed unchecked arithmetic and original list/metadata/helper dependencies. These are separately prepared startup request/lead-in operands, not the current recorded-frame timestamp or an inferred audio driver's position. Timing-point/helper suffix remains original.
+
+### Startup and hold-release have different provenance

| Reached path | Original guard/time/arguments and first continuation |
|---|---|
| M2269→M22b0→M224c | Readiness work calls M3b4f:0165, optional synthetic−F13f6(true):0180, schedules M22b0:019c through M09af. Original delay0 for Edit/recorded/T04af branch, otherwise M2333(0,600); no execution-latency claim. M22b0 retains old-source cleanup and passes **this.F13f1** to M224c:0034. |
| M224c | State6/7 returns before work. Original source/rate/control helpers; if F13f6>target call M3b59(−F13f6,false), otherwise accumulator=Double(F13f6), **M3b3a(target,false,false):00c5**. Only after normal return F13ef=true:00cc. No intrinsic paused guard in this method; caller/lifecycle matter. |
| M226b:0911 | After E28 pause/handoff gates: F1391, ordered F235e>=Double(unchecked(F13f6+Min(0,F235c))), phase2 and F13ef. Clear F1391, if synthetic clear it; only **fresh T>1000** calls **M3b3a(another fresh T,false,true)**. Then M3b52:0916. Not a repeated user seek or unconditional resume operation. |
| M3b52/M3b49→M3b39 | Readiness release, E28 resumed inactive source, and recovery-related callers. If hold false M3b39 returns. Otherwise source.virtual M3d5f(Double(unchecked(H+(H==0?0:O)))):0023, clear hold/C, M3b24, fresh T→F235a. No arbitrary requested timestamp from the pause key. |
| M2240→M3b48 | Startup source preparation; nonnull source is required by M3b48. Hold release precedes source.Play(true), then q=2. Not a live seek control. |
| M3b23 | Separate range-policy setter: disabling it while synthetic can clear synthetic then request0 or duration, false,false, only outside its bounds. No ordinary recognized-key caller established here. Editor/audio/control callers must not become a live rewind claim. |

Native seek slot **T0803.M3d5f** resolves to **T08e4.M437f** or fallback **T086e.M4050**; play slot M3d60 resolves M437e/M4051. Native milliseconds/1000 and BASS boundaries remain E10/E29; fallback seek stores Double F2615. M3b3a retains unchanged-time return, original lower/upper bounds, synthetic/hold priority, >200-or-force source test, q2/publication, callbacks and exceptions. Force does not bypass bounds or q0 hold. No actual native seek accuracy/success is established.

## D. Physical provider versus recorded-frame input stage

**T0335 is the abstract provider base; T03fc is the original interface**, not a record class. M11a3 receives F2a58's List<T0335>. It selects the first enumerated provider whose virtual M17a8 returns true and which implements T03fc, calls M17a7(selectedIdentity) on each, retains its T0508/T00f5 auxiliary comparison, and finally M1192(selected):00b2. If none selected it returns null without inventing zero input. Actual registration order/provider/configuration is UNRESOLVED; other interface implementers are not exhaustively surveyed.

| Original interface/virtual operation | Relevant concrete resolution |
|---|---|
| T03fc.M1e12 position | T0170.M09f2 returns Single-converted F0597/F0598; T0508.M270d returns F16b7; T00f5.M06de returns F0334 |
| T0335.M17a8 availability | T0170.M09f9 original settings/state/focus gates; T0508.M2714 returns true; T00f5.M06e5 original setting/focus/cursor gates |
| T0335.M17a7 selected update | T0170.M09fb, T0508.M270b, T00f5.M06dc, with original side effects retained |
| T03fc.M1e13–M1e17 nullable buttons | M1192 tests HasValue and invokes M1188/M118a/M118c/M118e/M1190 only when supplied |

The concrete interface methods are new-slot implementations with matching signatures/interface metadata, not accidentally treated as base reuse-slot overrides. M1192 invokes M1e12→M1193; original pointer setter/presentation dispatch retained. This physical provider selection occurs at **M43a0:0054**, stored to F2a93:0059, before normal clock publication and optional frame processing. It can still poll/update underlying state on a recorded pass.

### Host's live-path flag is separately recomputed

M43a0:0007–0040 computes F2a6f. With M4390 true it is false. With selector false, ordinary !F137c gives true. F137c and pause/F1b97/optional Play.F1397 branches can also select false/true; do not equate !F2a6f universally with recorded playback. This computation precedes later setter/callback changes in that pass.

Then original cursor/provider work; **M3b25:00ca normal clock**, F2a90=host F2c30:00d4. If !F2a6f, call **M43a1:00e0**, regardless of a new M4390 test inside that method (there is none). The additional !M4390&&F137c branch may subsequently seed button state through M43a6. If live path is true, original focus/input work can assign F2a5c at0144 and mapped F2a7b through M0ff1 at0162. Thus a source record merely existing is neither sufficient proof of active recorded selection nor a substitute for host path guards.

Keyboard polling **M1333:01d0** and generic **M43a2:0248** still run under F2a79/focus-or-recorded gates. Configured gameplay-key combination **M43a7:023e** is skipped when recorded selector is true. M43a2 itself has no M4390 check and reaches original new-key M379a:03d5; higher handlers can still consume the event. Consequently selected live controls can coexist with recorded gameplay buttons/pointer. This is not proof every key acts identically or that physical gameplay keys override the selected frame.

## E. Frame selection, time provenance and advancement

### Frame objects and their bounded provenance

**T0463** contains Single x/y F1307/F1308, Boolean state fields F1309–F130e, original **pButtonState T0b50** mask F130f and **Int32 timestamp F1310**. Constructor **M219b** takes `(Int32 time, Single x, Single y, original mask)` and stores time at0026 after button decomposition M219e. Original metadata names Left1=1, Right1=2, Left2=4, Right2=8, Smoke=16. M219e uses masks5/1/4/10/2/8 for its aggregate/individual booleans; do not relabel them as physical key events. Constructor **M219d** obtains time from supplied BinaryReader.ReadInt32:0048 and stores004d. This identifies a source operand, not an entire replay format, timestamp-unit conversion or authenticated input file.

Other direct timestamp writers are original T0466.M21af:00c0/00fb, which can subtract difficulty-derived operands under its result/state conditions, and T0a10.M4abc:00e9/00ff/0179/017f normalization above. Frame references admitted into lists are mutable; no permanent assertion that imported F1310 always equals original file time. Source may be constructor-supplied, generated by M114f, or already supplied by the original reader boundary. M226e's unresolved natural entry is not used to explain this provenance.

### M43a1's exact first cooperating gates

**M43a1/060043a1**, static Boolean/no arguments, under Monitor(F111a):

| Offset / branch | CONFIRMED original condition |
|---|---|
| 0016–002a | Null F2a7f or null source.F1980 leaves before clearing advance/eligibility fields. No invented null-provider reset. |
| 0039–0046 | Capture Count, then F2a7e=false and F2a90=false. |
| 0052–0064 | If !F2a80 && index==Count−1 set F2a82=true, before main eligibility. |
| 0069–00aa | Require application Play2, nonnull global record, nonnull captured Play, Play.F13bb!=0, readiness F13d8, Count>0 and index<Count−1. Failure takes08b1 fallback. No lower-index bounds check is invented. |
| 00af–00c2 | If !F2a82 && F2c48 && F2c4d, fallback. |
| 00c7–00f7 | If !F2a82 && F2a80, require **unchecked(lastFrame.time−fresh T)>1000**; equality1000 falls back. Buffer horizon is not source seeking. |
| 00fc–010a | Clear F2a5a; check room for another frame. |
| 010c–0149 | If **nextFrame.time<=fresh T**, !F1373 and !M3b0d(q==0), increment index once at0143 and set F2a7e=true:0149. Equality is due. |
| 014e–027b | Compute returned lag Boolean from unchecked(T−selectedFrame.time)>16 AND advanced. When true and record.virtual M2b41 true (base T059e returns true), original neutral/unchanged-button loop may increment further. It checks future due time, current states/mask and F136f; no backward search. |
| 0280–02bf | Optional F2a71 policy clears logical aggregates; select current list[index] reference into F2a9f. |
| 02c4–02de | **Only F2a7e true** enters time/button-event arm: T=selectedFrame.F1310 at02d8; F2a90=true:02de. |

M43a1 does not invoke M3b3a, M3d5f or the main clock. The T overwrite does not update F2359/F235a/F2352/F235b. Therefore frame time, source time, previous/copy time and the clock's working state can differ within one pass. F2a7e true is an advancement flag here, not a permanent declaration of a provider; M4391(false) itself does not clear it. Null-record early returns can retain its old value, while host may already have assigned F2a90 from F2c30.

### Button and cursor consequences

Advanced ordinary frame arm sets key/mouse-like flags F2a69/F2a6b/F2a85/F2a87 from F130b–F130e, with F2a71/previous-state guards, publishes held aggregates F2a67/F2a83, uses frame Single x/y, and stores original mask to F2a66:0541. It does not feed physical keycodes through M43a7. Non-advanced ordinary frame arm retains its already prepared aggregate state and follows original pointer interpolation branches; no new key timestamp is synthesized.

Both ordinary pointer arms converge on original per-coordinate M2d7d bounds−500..1000, store playfield **F2a7b:0871**, map forward through layout **M0ff4:087c**, store provider/viewport **F2a5c:0881**, construct integer pointer fields, and clear F1374. In non-advanced arm, equal adjacent timestamps choose current frame coordinates; otherwise original previous/current or current/next selection and M4eda apply Single arithmetic. This is an identified original helper boundary, not a newly implemented interpolation policy. Mania3 has separate branches; they are not ordinary osu!standard evidence.

At fallback08b1: if neither streaming F2a80 nor F2a82, leave. With F2a82 set, F2a90=true:08d0 and conditional pause/failure helpers follow; this is distinct from selecting a new frame or overwriting time. With streaming but !F2a82, set **F2a5a=true:0917**, original buffer/presentation helpers, then return. T0a10.M4ac1 observes this value and may request pause/unpause. If the original buffer/readiness/index predicates later admit the normal arm, it clears F2a5a at00fd even while the paused/q0 advancement check retains the index; the streaming wrapper can then request the opposite pause state. This is a cooperating buffer/request handoff, not immediate resume. No generated object, immediate audio pause, fixed delay or successful continuation is inferred.

Count0/1/exhausted lists therefore do not follow the normal next-frame arm. A backward clock movement alone does not decrement the index in M43a1: it can leave a retained selected frame and use pointer interpolation without re-emitting its key events or repairing objects. Explicit T00ff/T0466 index writers remain separate counterexamples to any universal “index never moves backwards” claim. Forced negative/out-of-range indices and concurrent list changes retain original indexing/exception behavior.

## F. Recorded-data admission and later selective restore

Original metadata names **osu_common.Bancho.Objects.ReplayAction T0b4d**: Standard0, NewSong1, Skip2, Completion3, Fail4, Pause5, Unpause6, SongSelect7, WatchingOther8. This is direct naming evidence for the supplied operation, not authorization to reconstruct its network/file format or assume it is replay-file playback.

The bounded upstream producer is **T0975.M477d**, selected branch0b6a: if F1118 exists, obtain original **F2d29.M2e02()** reader, construct **T06be/M334c(reader,false)**, capture it in closure **T097a.F2d57:0b85**, dispatch **M47a7 through F2c78.M09ad:0b9c**. Stop at supplied reader/dispatch origin. No transport packet decoding, credentials, file parsing or general network subsystem recovered. M47a7 simply supplies its captured data reference to **M1df7:0006**.

M09ad is not necessarily deferred: !arg2 && receiver.M09ab true invokes callback inline; otherwise Monitor-protected enqueue. Base M09ab compares current managed thread ID with F055c. The caller supplies false. Concrete scheduler, thread identity and later timing remain runtime dependencies; this is not a guarantee every callback executes on the next host pass. M09af's scheduled-item path is distinct.

### Action admission can coexist with the ordinary ruleset

**M1df7** stores supplied action into F1124 then switches:

- **Standard0:** if !F1119 and F2a7f is null, use first supplied frame time−50 (or1 for empty) for F2a81, copy F1e0f to F137a, and call M1dfd(false,snapshot.F2045).
- **NewSong1:** clear F1119, copy F1e0f to F137a; M1dfd(true,snapshot.F2045).
- **Skip2:** M2255 increments pending F1375 if a Play reference exists; actual seek remains guarded/later.
- **Completion3 / Fail4:** set F2a82=true; do not invent an immediate whole-session result.
- **Pause5 / Unpause6:** when current pause state differs and Play reference exists, virtual M2261; request only, not immediate latch/clock operation.
- **WatchingOther8 / SongSelect7:** original reference/selection/control helpers retained as boundaries; no general UI workflow reconstructed.

**M1dfd** under F111a requires F1118. With original selected-map lookup M2c67 nonnull: select map M2c64; **M4391(true):005e**, F2a82=false:0064, **F2a80=true:006a**, construct source through **M2e1c(M2232 mode, supplied identity string, selectedMap, policyBoolean):0080**, store F2a7f:0085, invalidate through M2b5a, assign supplied modifier wrapper. Optional first argument clears F2a81. Original callback/control prefix remains, then **M46ad(Play2,true):00f0**. This can naturally request T0a10 while keeping Osu0; it does not prove request acceptance or map import. Lookup failure takes original error/request paths and can disable M4391; no artificial map or frame object inserted.

When a source already exists, the shared append block does not require a particular Play subclass, ordinary ruleset, active selector, map identity or sender identity. Under F111a, nonnull current F2a7f and its F1980 suffice for appending **supplied frame references** at01bf. The closure captures the data, not an immutable destination Play/record. If globals change before execution, it uses the then-current source; actual interleaving and trusted producer constraints remain UNRESOLVED. This is permission established by instructions, not an assertion that unrelated data is naturally delivered to ordinary live Play.

### Marker creation is not restore consumption

After append, M1df7 preserves an existing marker only when **F1120>0 && marker>=currentIndex && currentList.Count−1>=marker**. Otherwise, only supplied snapshot.F2043>0 permits **F1120=currentList.Count−1:0268** and **F1121=supplied value snapshot:0273**. Empty or exceptional states keep original behavior; no stronger validity check or atomic immutable record is invented.

Later base **M226b:109e–10b7** requires **F2a80 && F2a7a==F1120**, then **M229f:10b1**, then clear marker−1 only after normal return. E29's M229f copies selected scalar counters/combo/policy contents into existing global record; it does not seek, replace every object, copy snapshot time or clear evaluated/component history. It occurs after normal candidate/object/result/session-finalization work in that reached Play invocation. A callback arriving after its marker is passed is not repaired by an invented >= test; a helper exception can leave partial counters and marker retained.

**M226e remains UNRESOLVED natural entry.** E29's zero direct managed callers/delegate references and no matched aliases remain preserved. E30's direct boundary inventory again contains none; unchanged body hash is checked. No invocation via M1df7, M43a1 or this provider-selection chain was established. Reflection/external invocation is not disproved.

## G. First downstream gameplay consequence and retained state

| Reached path | Time/input first consumer | Eligibility/state consequence, conditional on original guards/helpers |
|---|---|---|
| Ordinary live host | Normal M3b25/M3b24; live M0ff1 pointer, configured buttons | M2295 uses stored F2a5c mapped afresh; existing candidates and fresh T. |
| Successful Skip callback/startup seek | M3b3a/hold/source helpers can publish time during or after a host pass | No immediate M28e2/M24b3/reset; next reached current/candidate work observes changed time and retained objects. |
| Advanced recorded frame | Normal source clock first, then frame T-only overwrite; selected buttons/mapped F2a7b | M2295:007b uses F2a7b directly when selector true, not physical pointer remapping. Judgement still independently reads F2358; actual input time is not an event payload. |
| Recorded pass without advanced frame | Normal T retained, pointer may interpolate; F2a90 false after its clearing prefix | Candidate refresh still reachable; optional M0ac5 and M28e4 have their own recorded/eligibility gates. No blanket freeze of all object updates. |
| Streaming fallback | F2a5a can request pause through M4ac1 | E28 pending request can be consumed later; no immediate guarantee that earlier input/clock/updates did not occur. |
| Marker match | Late M229f scalar restoration | Does not change clock/object state used earlier that invocation or retroactively rerun validation. |

E28 guards remain: M226b readiness/lifecycle, pause return0853, resume-handoff return08ba and recovery constraints can prevent later work. M2295 independently rejects failed/handoff/recovery state; paused condition rejects **only when !M4390** on its reached early branch. This is not permission to ignore M43a1's paused advancement veto or Play's update return. Pending pause alone does not retroactively cancel already dispatched input.

On admitted normal base path: **M2275:094e selects current from old candidates → M28e2:0959 refreshes → optional M0ac5:09be → M28e3:09cf**. Optional work requires !M4390 OR F2a90 OR F13f2, then nullable F13d9 absent or stored<=fresh T. Equality permits repeat. False tests rejoin09c9 before M28e3 under E12. The object-update loop invokes object M24b2 before manager M28e4; the latter's original **F2a90 OR !M4390** gate affects evaluation, not a universal skip of every earlier object method. Current object, fresh candidates, evaluated object, object raw result and manager-local result remain distinct.

M43a8 separately returns true when selector&&F2a78, otherwise F137c. E15 slider target-time correction is conditional on physical provider T0170 and !M43a8; selecting recorded/auxiliary state can therefore change that correction eligibility while leaving the physical provider reference present. Preserve its original setting/helper and fresh clock reads, not an invented uniform cursor delay.

E29 retention therefore survives this integration: rewinding below F13d9 can suppress component attention without undoing slider processed counters or spinner P/C/E. Forward jumps can expose one outstanding slider checkpoint per reached component call and then parent completion without draining backlog. An evaluated circle is not unjudged because time moves backwards. Provider selection itself does not clear finalization F1397/max-end; E27/29 finalization-before-late-restore ordering remains. No additional end-to-end lifecycle theorem or fixed physical-frame cadence follows.

## H. Conditional cases: original-runtime status NOT RUN throughout

Each row combines **CONFIRMED** original predicate/sites above with a **RECONSTRUCTED** conditional prediction. Helpers return normally, references/counts are valid, no unmentioned branch overrides/interleavings unless stated. No HYPOTHESIS is treated as a mechanism. Every row's original-runtime observation is **NOT RUN — WINDOWS HOST REQUIRED**; these are not executed input/seek/replay simulations.

| ID | Controlled difference | RECONSTRUCTED static prediction | Runtime |
|---|---|---|---|
| C01 | Base ordinary Play, selectorfalse, !F137c | Live host branch and configured gameplay keys eligible; source-list existence alone does not enable selector | NOT RUN |
| C02 | M4391(true), source null | Selector true, no record construction; later frame path returns before clearing its flags | NOT RUN |
| C03 | M4391(false), source/frame nonnull | Store selectorfalse then null source/frame; index/stream/advance flags not reset by setter | NOT RUN |
| C04 | Phase!=2 Skip key | M225b returnsfalse before queue | NOT RUN |
| C05 | Wrapper not ready, Skip key | Not the Pause/Escape alternative; no Skip callback | NOT RUN |
| C06 | Ready Skip while paused | Key route may queue; eventual M2256 paused rejects if still paused | NOT RUN |
| C07 | Ready Skip during F13be handoff | Callback rejects while handoff remains true | NOT RUN |
| C08 | Allowed seek arm, M2257false | Target wrapped marker+original M2333(−80,90), false,false | NOT RUN |
| C09 | M2257true with original early bounds | Target0, false,false; not arbitrary rewind timestamp | NOT RUN |
| C10 | T exactly marker or F1397true | No seek arm; suffix may request departure under its independent flags | NOT RUN |
| C11 | F136ctrue or state!=1 in early arm | Returnfalse before seek | NOT RUN |
| C12 | Recovery F1376true but other Skip guards satisfied | No direct recovery veto in M2256; upstream caller/helper reachability still needed | NOT RUN |
| C13 | M2333 delay callback not yet executed | No seek merely from key handled/queue constructed | NOT RUN |
| C14 | State changes between queue and callback | Original callback checks live mode/pause/handoff/readiness flags | NOT RUN |
| C15 | Startup F13f6>supplied target | Synthetic−F13f6(false), not M3b3a arm; F13ef after normal helper return | NOT RUN |
| C16 | Startup F13f6==target | M3b3a(target,false,false) arm | NOT RUN |
| C17 | Startup state6/7 | M224c returns before clock/source/helper work | NOT RUN |
| C18 | Admitted forced startup, T==1000 | Clear synthetic but no forced M3b3a; >1000 required | NOT RUN |
| C19 | Same branch, T>1000 | Fresh T passed false,true then release/start helper; no bypass-bounds | NOT RUN |
| C20 | Hold false versus true | M3b39 returns versus source seek/clear/main-clock/previous capture | NOT RUN |
| C21 | Hold targetH0 | Seek source0, special offset omission; not generic H+O | NOT RUN |
| C22 | Internal M3b3a arbitrary backward target | API ability is not an established ordinary live key control; E29 object retention remains | NOT RUN |
| C23 | Play commit, selectortrue/streamtrue, earlier special branches false | T0a10 selected; Osu0 can still construct T0253 | NOT RUN |
| C24 | Autoplay bit2048, selector initiallyfalse | T0a10 constructor selection; later original initialization can enable/generated frames | NOT RUN |
| C25 | Earlier T04af/editor predicate true | Takes priority over recorded/Autoplay constructor test | NOT RUN |
| C26 | Base versus T0a10 M224a | Base disabling/helper path differs from index/button/normalization override | NOT RUN |
| C27 | Physical list has two available interface providers | First eligible wins; actual registration/settings not inferred | NOT RUN |
| C28 | Physical provider none | Null selected provider, no invented center/zero sample | NOT RUN |
| C29 | Recorded provider and physical control key in same host pass | Physical provider sampling and generic keyboard dispatch can coexist; configured gameplay-key combine skipped | NOT RUN |
| C30 | Null source.F1980 | Return before F2a7e/F2a90 clearing prefix; host's earlier eligibility assignment retained | NOT RUN |
| C31 | Empty nonstream list | Clears advanced/eligibility, normal next-frame arm fails; fallback state matters | NOT RUN |
| C32 | One frame/index0 nonstream | Index==Count−1 sets F2a82; normal arm rejects, distinct EOF eligibility branch | NOT RUN |
| C33 | Last available frame/indexCount−1 | No next-frame selection/time overwrite; fixed/stream fallback differ | NOT RUN |
| C34 | Next frame.time==normal T | Due equality advances if pause/q and all earlier gates permit | NOT RUN |
| C35 | Next frame.time<normal T | Advances, can overwrite T backwards to that selected timestamp; no native seek | NOT RUN |
| C36 | Next frame.time>normal T | Index retained; no advanced-time overwrite; original pointer arm may interpolate | NOT RUN |
| C37 | Paused or q0 at advance predicate | No index increment/advanced store there; other prefix/fallback/pointer work may occur | NOT RUN |
| C38 | Stream lastTime−T exactly1000 | Normal arm withheld by <=1000 fallback; >1000 needed | NOT RUN |
| C39 | Advanced and lag exactly16 | Returned lag Booleanfalse; >16 needed for neutral skip loop | NOT RUN |
| C40 | Advanced lag>16, base record gettertrue and neutral guards | Additional forward increments possible, not necessarily one frame/pass | NOT RUN |
| C41 | Clock rewound behind retained frame | No M43a1 decrement/backsearch; earlier pair interpolation is not frame-event replay | NOT RUN |
| C42 | Explicit T00ff/T0466 transition | Separate index repositioning exists; not a counterexample to bounded M43a1 finding | NOT RUN |
| C43 | Admitted ReplayAction.Skip2 | Increment pending skip; later guarded virtual call, not direct seek | NOT RUN |
| C44 | Supplied Standard0, existing source/list | Append supplied object references; no inherent ordinary-mode or source-identity check in append block | NOT RUN |
| C45 | Standard0 source absent, valid lookup/F1118 | Original M1dfd can set selector/stream/source and request Play; acceptance still E03 | NOT RUN |
| C46 | Original selected-map lookup fails | Error/request branch retained; no injected state to force entry | NOT RUN |
| C47 | Existing valid future marker | Keep earlier marker/snapshot despite appended frames | NOT RUN |
| C48 | No valid marker, positive F2043 | Store last-index marker then value snapshot; not restore yet | NOT RUN |
| C49 | Streamtrue/index equals marker | Late M229f; clear marker only after normal return | NOT RUN |
| C50 | Index!=marker or streamfalse | No selective restore; no >= repair if marker passed | NOT RUN |
| C51 | Data callback queued, source changes before execution | Captured data but live destination source at append; actual occurrence UNRESOLVED | NOT RUN |
| C52 | Callback dispatch M09ad thread matches versus not | Inline versus enqueue under base scheduler predicate; no assumed fixed delay | NOT RUN |
| C53 | Nonadvanced recorded pass, nullable gate permits | Candidate refresh/object M24b2 still reachable; component/manager evaluation retain separate gates | NOT RUN |
| C54 | Backward T below F13d9 | Optional M0ac5 skipped, normal-returning E12 rejoin still reaches M28e3 | NOT RUN |
| C55 | Same time equals F13d9, new admitted pass | Optional repeated component call permitted; object histories/cache decide result | NOT RUN |
| C56 | Paused/handoff Play return | Suppresses later current/refresh/component/object/finalization chain, not earlier host clock/input | NOT RUN |
| C57 | Frame selected/time stored, later pointer/helper throws | Earlier index/T/button mutations may persist; no local rollback, Monitor finally retained | NOT RUN |
| C58 | M229f helper throws | Partial scalar restore/marker retained; no automatic object/time undo | NOT RUN |
| C59 | Supplied data has invalid list/index or concurrent replacement | Original dereference/indexing/collection exceptions; no fabricated repair | NOT RUN |
| C60 | M226e direct invocation proposed without caller | Artificial probe only; natural entry unresolved | NOT RUN |
| C61 | Paused/live-selector/failed/global-record with key112 | After normal M2267 return alias existing global source, enable selector, set end gate and request Play; no direct seek/object restore | NOT RUN |
| C62 | Streaming buffer becomes eligible while paused/q0 | Clear waiting flag before advancement veto; wrapper may request unpause, still subject to original pending-control gates | NOT RUN |

## I. Remaining boundaries and minimal E30-to-E20 observations

UNRESOLVED: actual concrete Play/provider and registration order, active flags/settings/modifiers, imported frames/times and preparation, original reader provenance/admission ordering, sender/map trust beyond the scoped block, native/fallback seek success, CLR/JIT numeric behavior, thread/GC/reentrancy, callbacks/exceptions, natural reachability of unusual state combinations, displayed/audible consequences and instrumentation overhead. HYPOTHESIS that every existing frame list is an enabled replay or that source seek guarantees matching judgement time is unsupported. E29 M226e remains without an established natural incoming chain.

Do not change or recreate E20's packet. Future minimal addition, under its existing pinned process/import/overhead requirements:

1. Log recognized control/actual binding, concrete receiver/slot, queue item and thread, state at key receipt versus M2256 admission, original target and both seek Booleans; native receiver arguments/return evidence separately from consumed T/I/X/O/H/q.
2. Log actual constructor selection, global selector/stream/live-path/advance/eligibility flags, source/global/handler identities, physical T03fc provider and list order; preserve separate reads and any actual M4391 writer.
3. Capture frame-list identity/count, previous/current index, selected object F1310/x/y/mask, original normal-clock publication, M43a1 branch and T overwrite, pointer and held aggregates actually consumed. Do not add timing/property calls merely for logging.
4. Capture queued press/control order, M2295 pointer/time operands, old-current selection, candidate refresh, nullable gate, component and object/manager update entries, retained evaluated/results/component state; separate natural Skip from forced backward-clock probes.
5. If a naturally available original data source is authorized: log supplied data identity/action, admitted destination record/list, index/marker/snapshot writes, inline/queued dispatch, and late M229f/marker clear. A supplied packet, positive marker and completed restore are three different facts.

All observations remain NOT RUN. Smallest Windows dependency: one original live Skip and one naturally configured recorded-input pass with the same pinned map, capturing requested/normal/frame/consumed time and next input/update entry. Separate forced states and measure tracer overhead. If further static work is justified, the bounded original frame-list lifecycle at provider departure/reselection is the adjacent question; do not expand into serialization or a replacement replay engine.

# 3. Pseudocode

**RECONSTRUCTED, independent and non-executable.** Token-labelled calls remain original operations with their side effects, failures and callback/receiver boundaries. Each stated read occurs at its original stage; no atomic provider/time snapshot or substitute gameplay processor is supplied.

```text
recognized Skip:
    require original control phase/readiness
    schedule this.M22ad with original M2333 delay and M09af

M22ad, when actually invoked:
    call virtual this.M2256 and discard Boolean

ordinary M2256:
    reject unless Play2, not paused, no resume handoff, startup admitted
    if fresh time < current marker and not finalized:
        reject original already-skipped/state gates
        retain original prefix and M223f
        target := 0 if fresh M2257 permits, otherwise marker + original perturbation
        retain M3b55
        original M3b3a(target, bypassBounds=false, force=false)
        F13fb := -1
        retain original notification/handler helpers
        F1375 := unchecked(F1375 - 1)
    else:
        retain separate conditional departure suffix
    return original logical Boolean, not native seek success

Play constructor selection at original accepted commit:
    retain earlier special and editor priority
    if recorded selector OR streaming flag OR original Autoplay bit:
        construct T0a10
    else:
        construct base T047b
    original initialization still separately selects Osu0 handler/manager

host input pass:
    recompute original live-path gate from selector and original special flags
    select/update physical provider through original interface
    original M3b25 normal clock publication
    F2a90 := original host gate
    if not live-path:
        call original M43a1
        retain separate non-recorded special-button branch
    else:
        original live pointer/button work
    poll physical keyboard under original enable/focus guards
    combine configured gameplay keys only when recorded selector is false
    retain generic keyboard controls, button histories and queued press dispatch
    invoke original application/component callback
    retain original post-callback histories and bounded repeat decision

M43a1, under original lock:
    if source or source.frames absent: leave before clearing per-pass flags
    count := actual frame count
    advanced := false; componentEligibility := false
    retain fixed-end, application/readiness/state/count/index and buffer guards
    if next frame is due by fresh T and not paused and q != 0:
        index := unchecked(index + 1); advanced := true
    retain original lag-dependent forward neutral-frame skipping
    selected := current source.frames[index]
    if advanced:
        F2358 := selected.originalTime
        componentEligibility := true
        original frame button-state publication and current x/y
    else:
        original ordinary pointer selection/interpolation helpers
    retain coordinate bounds
    mappedPointer := original prepared pointer
    storedPointer := original layout.M0ff4(mappedPointer)
    retain fallback/buffer/pause-request paths and original return
    // no native seek, previous/copy-clock repair or object reconstruction added

original admitted data callback:
    retain action-specific selection/skip/pause request helpers
    under original lock, if current source/list exists:
        append supplied frame references
        retain valid old marker, otherwise conditionally store new marker/snapshot

next admitted ordinary Play update:
    preserve readiness/pause/resume/recovery gates
    select current from existing candidates
    refresh candidates with current published T
    optionally process current object using recorded/eligibility/nullable-time gates
    rejoin original object/manager updates
    retain results and E27 finalization/departure ordering
    if streaming flag and current frame index equals pending marker:
        original M229f selective existing-record restoration
        clear marker only after normal return
    retain pending-control consumption
```

The established boundaries explain selection and ordering, not measured Windows playback, physical input latency, pixels, a universal rewind control or full replay correctness. Work 1 remains research-only; Work 2 remains suspended.
