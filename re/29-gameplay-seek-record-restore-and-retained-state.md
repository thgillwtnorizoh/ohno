# 1. Result

For an osu! player, changing song time is not an undo operation for hits. The recovered clock-seek path does not itself rebuild the map or reverse earlier judgements. Candidate refresh can bring existing object references back into attention with their evaluated flags and component history still present. The engine also has separate, partial object resets and selective record-restoration operations; those must not be substituted for a universal rewind system.

**RECONSTRUCTED player-facing consequence of CONFIRMED original static instructions.** No original seek, import, replay, hit, displayed state or audio behavior was observed on Windows. Work 2 remains suspended; `reconstructed/` was neither inspected nor modified. No authorised exception.

## Objective, provenance and verification boundary

E29 connects original time-discontinuity entry points, startup marking, object reset slots, record/snapshot operations, recorded-frame overrides and subsequent ordinary Play gates. Scope is the existing circle, Linear slider and ordinary spinner state, not complete replay parsing/serialization, retry loading, score/HP, a generic save-state system, time-travel-safe gameplay implementation or performance work.

Read the complete standing contract and cumulative research through E28, relevant E05/E07/E10–E12/E15–E19/E20/E24–E28 and necessary E21–E23 component/policy evidence. Preserve E05's ordinary/Taiko correction, E12's post-refresh update rejoin, E14's resource ordering and E24's ordinary-spinner correction. Historical wording remains intact; this report bounds previously named “restore” methods by their actual incoming call evidence.

Dedicated **work1/expedition-29-seek-and-retained-state** starts directly from requested remote **c632ea05e3060b03b8026b096da780083bf9e0bc**, tree **4b44b268919c62e19b9cca28a454600521087d6a**. All 133 baseline blobs/modes matched the local mirror, whose commit identity differs but tree is identical. Preserve the other 132 files, exact **287948-byte** prior journal prefix, all fixtures and unchanged E20 packet. Publication uses the exact remote parent; final handoff supplies verified publication commit/tree/branch/scope.

**E20 remains preparation-only and incomplete. All 17 original-runtime cases are NOT RUN — WINDOWS HOST REQUIRED. All 48 E29 runtime cases are likewise NOT RUN.** A future owner-authorized Windows PC is not present access or proof of debugger/profiler capability. There are no original-runtime observations in this expedition.

## Inputs, evidence and reproducibility

| Actual bytes rechecked | SHA-256 / status |
|---|---|
| ZIP | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; 18 entries, CRC valid |
| Archive osu!.exe | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Supplied/repository contract | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |
| Existing E20 circle fixture | `682d600f7a76436c9ec17f1b2ccf52afc968ebf5ef81c3e74b282161c5d0aa0d` |
| Existing E16 slider fixture | `f57e2231d7bd3f376632992eed4fced5e6bc57a885c9375a7c1fbf0e71483a84` |
| Existing E24 spinner fixture | `85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c` |

Self-declared Stable40/file-version1.3.3.8; precise official dated identity, authentication and modification status remain UNRESOLVED. Actual Linux x86_64 host has no wine/dotnet/pwsh/powershell/DISPLAY/WAYLAND or supplied authorized Windows endpoint. dnfile was missing on this turn; pinned dnfile0.18.0/dncil1.0.2/pefile2024.8.26 were reinstalled before extraction. No original EXE, CLR, native import or substitute clock/seek/gameplay model executed.

New `re/evidence/29/seek-restore.json`: **801465 bytes**, SHA-256 **`413a4722f40755d049d521c680861ebc12c0b42ccd751bd8f2653fdd5f2c0fcf`**. New tools: `re/tools/seek_restore_evidence.py`, `re/tools/check_seek_restore.py`. Reuses unchanged E04/E05/E15/E25 collectors; 98 selected method entries, 45 focused-writer body identities, full selected direct field/caller inventories, signatures, ancestry/slots, native imports, snapshot metadata and restore-reference alias checks. Full EXE scan: 19659 decoded bodies/1739 zero-RVA/0 decode errors. No proprietary executable/assets/full source/full disassembly/private dump or trace is published.

Mxxxx/Fxxxx/Txxxx mean original MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx. Report offsets are hexadecimal, relative to the first CIL opcode, excluding headers; JSON offsets are decimal. They are not native/JIT addresses. CONFIRMED denotes instructions/metadata, RECONSTRUCTED denotes semantics and conditional pseudocode, HYPOTHESIS denotes an untested explanation, UNRESOLVED denotes missing original evidence. Direct-store absence is not a theorem excluding reflective, native, external or arbitrary callback mutation.

Competing explanations: clock movement versus object reset; candidate refresh versus reconstruction; partial reset versus history repair; fresh-record replacement versus in-place counter restoration; recorded frame time versus audio time; pending progress versus final judgement; finalization versus departure. Inspect the original CIL/stack/branches, exact ancestry and full managed direct inventories, then discriminate using conditional cases. No remembered version's replay behavior or alternative engine is proof.

```sh
python re/tools/seek_restore_evidence.py /private/osubackup.zip > /private/seek-a.json
python re/tools/seek_restore_evidence.py /private/osubackup.zip > /private/seek-b.json
cmp /private/seek-a.json /private/seek-b.json
cmp /private/seek-a.json re/evidence/29/seek-restore.json
python re/tools/check_seek_restore.py re/evidence/29/seek-restore.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

Verification recorded for publication: repeated extraction/public-byte equality, focused checks/376 historical body-hash comparisons, all 26 previous checkers, unchanged E20 safeguards, Python compilation, historical/fixture/journal-prefix/diff checks and remote parent/tree/branch/all-file verification. These are static evidence checks, not Windows acceptance. A draft assertion used M3b66:0011 instead of the actual F2358 opcode at000d; corrected before PASS. An initial extraction attempt failed before decoding because dnfile was absent; reinstall/retry was host preparation, not an original-game failure.

# 2. What they found

## A. Entry paths are distinct

Ordinary relationships remain base **Play T047b**, handler **T0253**, manager **T00e8**, clock owner **T07b9**, record **T059e**, circle **T094c**, slider **T02af/head T06f6**, spinner **T00d8**. No Taiko route is substituted.

| Original path | Established condition / immediate effect | Boundary |
|---|---|---|
| M3b3a /06003b3a | Static `(Int32 request, Boolean bypassBounds, Boolean force)`; clock/source seek semantics below | No intrinsic Play-pause or object-reset guard |
| M3b49/M3b48/M3b52→M3b39 | Hold-release branch | Native/fallback source seek, then main-clock publication and previous-time capture |
| M2240→M3b4f:00c7 | Loading/setup clock reset after original source/rate work | Separate from object construction/reset |
| M2269:0165→M3b4f; optional0180→M3b59 | Original loading/readiness predicates | Can overwrite clock state before normal object attention |
| M2269 queues M22b0; M22b0:0034→M224c | Startup continuation | M224c chooses synthetic initialization or M3b3a; original queue/helper timing unresolved |
| M225b Skip19→M2254→M22ad→M2256:00bc→M3b3a | Existing E28 control registration and original skip guards | No inferred general ordinary backward-seek key |
| M226b:0911→M3b3a(T,false,true) | E28 startup/synthetic-clear conditions, T>1000 | Not unconditional every update or resume |
| M43a0:00e0→M43a1 | Optional recorded-input branch after normal clock | Selected frame can overwrite only T at02d8 |
| M226b:10b1→M229f | F2a80 and current recorded index matches F1120 | Connected late selective in-place record/counter restoration |
| M226e /0600226e | Private instance method taking value-type T0740 | Body recovered; **no direct managed incoming caller/delegate site established** |

The complete direct M3b3a caller inventory includes editor, UI and other variants; only the reached ordinary loading/startup/skip paths above are assigned those meanings. The API's ability to accept a negative/backward request is not proof of an ordinary live rewind control. Forced direct calls, arbitrary clock edits and resets of used objects must be labelled artificial unless a concrete original caller is established.

## B. Exact clock-seek priority and numeric operations

Define separate original reads: T=corrected F2358, I=rounded F235b, X=Double F2352, O=offset F235c, C=synthetic F234c, H=F2353, hold=F2382, q=F2368, L=original M3b0c() duration accessor. These are not one snapshot.

**M3b3a/06003b3a**, signature `000301080202`, code285 bytes:

| Offsets | Original operation / branch |
|---|---|
| `0000–0013` | Return only if **I==request AND separately read T==request AND !force**. This is before bounds/source/helper work. |
| `0014–0027` | Edit1 may call M3b55; call M3b65; set F1bb8=true. M3b65 resets timing-point indices F238e/F238f to−1, not object flags. |
| `002c–0047` | If request<−O, !bypassBounds and !C, assign request=−O and **jump0067, skipping upper test**. Unchecked Int32 negation. |
| `0049–0065` | Otherwise, if request>L−O and !bypassBounds, assign that freshly read upper bound. Preserve separate L/O calls and signed comparison. |
| `0067–00ad` | If C: if ordered Double(request)>X, add request−X to F235e. Set X=Double(unchecked(request+O)); Math.Round(X)→conv.i4→subtract a separately read O→T. Call M3b24. |
| `00b2–00bc` | Reread C **after** the clock helper. Continue source/hold path only if !C or force. Do not reuse entry C. |
| `00be–00ed` | If q!=0, issue receiver.M3d5f(Double(unchecked(request+O))) only if **AbsInt32(unchecked(request−T))>200 OR force**. Exactly200 skips the source seek without force. |
| `00ee–0103` | In that nonzero-q arm, set q=2 even when native seek skipped; call M3b24; copy fresh T→F235a. |
| `0105–0111` | q0 arm instead stores **H=request, hold=true**, calls M3b24. Even force does not replace this arm with a direct source seek. |
| `0116–011c` | Original M3b66(false), then return. |

Force bypasses the initial unchanged-time return and the later distance test; it does **not** inherently bypass bounds or q0's hold branch. The two Boolean arguments have different effects. AbsInt32 of wrapped Int32.MinValue can throw at00cc; no replacement saturating arithmetic is supplied. A synthetic assignment can have occurred before a later failure. Target-CLR non-finite/overflow conversion behavior remains unmeasured.

M3b24 is E10's correction/publication machinery, not an identity assignment of its request. It can update/range-adjust the receiver, snap/smooth X, round, recompute O, publish T and then F2359; a null receiver returns before its writes. Source seek, corrected time, copy and previous time can disagree. Native calls, source errors, helper callbacks, changing offsets or receiver identity can prevent the requested Int32 from being the final published T.

### Hold release and reset

**M3b39:** if !hold, return. Otherwise request the original receiver seek with **Double(unchecked(H+(H==0?0:O))) at0023**; clear hold at0029 and C at002f; M3b24 at0034; copy fresh T→F235a at003e. O is sampled before that helper recomputes it. No common object reset or record operation is inserted.

**M3b4f:** T=unchecked(−O):0006, F2359=0:000c, H=0:0012, hold=true:0018; optional receiver.M3d64 pause/seek0; then q=0:002e. It does not atomically set X/I/previous/O. An exception in the receiver can leave earlier stores but old q. This clock reset is not M24b3.

**M3b59:** explicit synthetic initialization C=true, T/I/copy supplied target, X=Double(target), hold=false and original accumulator; Booleanfalse sets q1, true preserves q. M3b5a clears C. E28 resume retains its own handoff/q/hold conditions; ordinary resume does not become a full clock reset.

Native T08e4 resolves seek slot **M3d5f→M437f**: Double milliseconds/1000→M314b→BASS_ChannelSeconds2Bytes/M3143→**BASS_ChannelSetPosition/M314e**, flags0. Fallback T086e resolves **M4050**, a direct F2615 Double store. M3d64 is the inherited original pause-then-seek0 helper. No return value, driver seek accuracy, heard position or audible discontinuity was observed.

## C. Necessary timing-point callbacks remain side-effect boundaries

Seek is not declared callback-free. M3b65 invalidates F238e/F238f. M3b66(false) uses **unchecked(fresh T+5)**, original timing-point list/find predicates, and can reselect current timing points and invoke **M3b81/F2391** and **M3b84/F2392** delegates. Its original catch resets timing indices and F238b sentinel; it does not roll back already published time or undo object history.

Ordinary Play registers **M2249 via M2246:0046→M3b82** on F2392; M2248 unregisters it. M2249 compares the point's original M0b76 Boolean with F13d2, updates F13d2 and invokes handler.virtual **M0ad8** only on change. Ordinary T0253 inherits base M0ad8: true calls **M0ad6**, an original Play/timed-event/presentation-transform path. Inspected calls do not insert an object-list reconstruction, judgement undo or component drain. Retain its allocations, current-time reads, resources, callbacks and possible exceptions. Other subscriber contents, reentrancy and native/external mutation remain UNRESOLVED.

M3b3a itself has no catch. M3b66 can swallow exceptions within its own region, leaving earlier clock changes and reset timing indices; earlier source/correction/helper failures can instead interrupt M3b3a. A “normally returned seek” is not proof every notification succeeded.

## D. Startup time initialization and past-object marking

M2269's admitted readiness arm calls original wrapper M0709, stores F13d8=true/F234d=true, then **M3b4f:0165**. If F13f6>F13f1, **M3b59(−F13f6,true):0180** follows. Its not-ready branches instead publish F13f1 or −F13f6 directly into T/copy and return before normal attention. Those are original loading operations, not a guarantee that reset means fresh object results.

M2269 registers **M22b0 through F2c78.M09af** with original delay argument; no callback execution/cadence measured. M22b0 retains old-source cleanup and calls **M224c(F13f1)**. M224c rejects Play states6/7, performs original rate/prefix work, then:

- If F13f6>supplied target, initialize synthetic **−F13f6,false**.
- Otherwise write F235e=Double(F13f6), call **M3b3a(target,false,false):00c5**.
- Store F13ef=true afterward. Exceptions can prevent that final store.

### M2241 is a separate loading pass, not every seek's reset

The sole direct caller is **M223b:0424**, after loading/postprocessing and before input-registration M2246 at042a. **M2241/06002241** enters when `(F2c73 && F2c74>full[0].start) OR F2a81>0`; preserve short-circuit/indexing behavior. The guarded reference values are not substituted for its actual time reads.

It sets F235e=Double(F13f6), then iterates **full F17fb**, using cached count F17fc and fresh reads. For each object with **end F2524<=fresh T** (`005a bgt` skips only end>T):

1. **006e sets that existing object's F1527=true.**
2. **0094 calls handler target.M2979(object.F1529)** with the stored Double operand; E19's ordinary receiver assigns clamped target and separate accumulator.
3. Continue across the list; no M28d3/result event, object evaluator or raw-result assignment is inserted. End equality marks evaluated.

It does not mark embedded head T06f6 merely because the parent is in the full registered list. It does not count pending slider checkpoints or spinner observations. F1529 has its own preparation writers; it is not F152a, a freshly inferred hit grade or a value manufactured from current time. Later calls can observe a marked object without a corresponding judgement event from this pass. Its timing-point Boolean suffix is retained.

No direct M2241 caller occurs inside M3b3a/M3b39/M226e/M229f/M43a1. Do not rerun this loading pass on an arbitrary live seek. Conversely do not equate its evaluated marking with successful object classification. Partial helper failure or time changes between loop iterations retain original behavior.

## E. Reset slot and retained-state matrix

Original reset slot **M24b3/060024b3**, T04c3, first iterates object.F1531 presentation components with M24ce. The callback removes transforms whose **F1875==148** via M24cf. Only **after that loop returns**, **002c writes F1527=false**. There is no F152a store. A presentation/helper exception can occur before the flag clears.

Exact ordinary resolutions:

| Object | M24b3 resolution / ordered extra work |
|---|---|
| Circle T094c and head T06f6 | **M45f6→base M24b3**, then original transform construction/tag148 and presentation numeric fields1,1,0.8. Stored raw F152a is retained. |
| Slider parent T02af | **M1431→base M24b3**, virtual M24c4 resource/action hook, head F0bdf.virtual M24b3, then **F0bf4=0 at0019**. |
| Spinner T00d8 | **M0606→base M24b3**, return. No progress, requirement, cached integer or observed-count reset there. |

F0bf4 is the E17 presentation/audio checkpoint index, **not successful+failed processed count**. M1431 does not clear F0bf1/F0bf5, F0bf6 outcomes, F0bf8 deadlines, F0bf2 entry time or F0bf3 tracking flag. It clears parent evaluation before its resource/head operations; an exception can leave parent reset but head/index unchanged. The head reset clears evaluated but retains its old raw result. No reset transaction repairs counters or records.

### A genuinely reached later reset has different object identity

Original guarded tracking leave **M146e** can invoke **M1487** via the F0bd5 list when parent.F1527 is false (`00c7–00f4`); M1487 calls virtual M24b3 on each member. **F0bd5 is List<T05b1>**, whose members resolve reset to **M2be4**, then ordinary-circle reset M45f6 unless its original bit8 gate returns. T05b1 and embedded head T06f6 are different sibling subclasses of T094c. This path resets those component/presentation objects, not the parent or F0bdf head by identity. Its final F0bf3=false store and helper ordering remain E15.

Therefore a later follow failure after a seek may reach selective child reset through normal tracking. That is not evidence that the clock primitive undoes parent/head judgement or clears scheduled outcome history. Complete repeat rendering is outside scope.

### State effects, assuming the reached helpers return normally

“Retained” here means no explicit overwrite/reconstruction in the named inspected path, **not proof against arbitrary callbacks/native/reflection**. Other dirty-geometry, editing or setup paths are independent.

| State | Clock seek/hold/reset primitives | Explicit object M24b3 | M2241 loading mark | Record restores |
|---|---|---|---|---|
| Full F17fb / object identities | No reconstruction | Existing receiver | Existing full-list objects | No chart-object construction |
| Candidates F17fd | No direct refresh | No replacement | No replacement | No replacement; later M28e2 separate |
| Play.current F1380 / manager.F17ff | Retained until original later selection/result call | Not a manager reset | No evaluator-selected F17ff assignment | No selected-object reset |
| Circle/head evaluated / F152a | Retained | Evaluationfalse, raw retained | Full-list object's evaluationtrue, raw retained | No object flag/raw restoration |
| Slider parent evaluation / raw | Retained | Parentfalse; head reset; parent raw not assigned | Parenttrue if it qualifies | No component/parent repair |
| Slider success/failure/outcomes/deadlines | Retained | Retained; only F0bf4 reset | Not processed/drained | Not restored from snapshot |
| Tracking active/entry | Retained by primitive | No direct clear | No direct clear | No snapshot copy |
| Spinner P/N/C/E/angle/motion | Retained by primitive | Retained; evaluationfalse only | No progress drain | Not in snapshot object data |
| Nullable Play.F13d9 | Retained | Retained | Retained | Retained |
| Record/counters/combo | No general restore | Not a record rollback | Target gets each qualifying stored F1529 | Explicit selective writes below |
| Maximum end B / finalized D | Not reset | Not reset | No B/D write | Not reset by either restore body |

Fresh construction and reusing an object are distinct. M45ec's constructor invokes the reset slot on fresh circle state; that does not establish reset-on-seek. Spinner **M05f4** setup resets P and prepares N but not C/E/angle/motion; its end-setter/preparation callers remain E26, not the common seek route. Resetting a used spinner/slider directly is an artificial partial-reset probe unless its original caller is actually reached.

## F. Value snapshot, fresh-record replacement and its missing caller

**T0740/02000740** is an original obfuscated **System.ValueType** with 18 scalar fields, no hit-object array/reference fields. Its useful payload is independently captured by **M229e**:

| Snapshot fields | Origin in M229e |
|---|---|
| F2036–F203b, UInt16 | Global record result counters F1969/F196a/F196b/F196c/F196d/F196e |
| F203f, UInt16 | Record F1975 through conv.u2 at00a3 |
| F203c, UInt16 | Current combo M40f0 through conv.u2 at00bb |
| F203d, Int32 | Original target getter M2966 through conv.i4; null target gives0 |
| F2041/F2040, Boolean | Record F197c / !fresh Play.F136a |
| F2042, Int32 | Fresh gameplay F2358 at00fc, store0101 |
| F2043, Int32 | Original record virtual M2b4b result |
| F2045/F2046/F2047 | Policy-reference presence and separate Double accumulator reads, null giving0 |

F203e Byte/F2044 Int32 and other consumers remain metadata boundaries, not an expanded serialization format. M229e catches errors and returns a default value; its field reads are not one atomic snapshot. It is directly called by M1dfa, M227f and M2406; only the scoped capture/restore dataflow is followed. Current and maximum-like combo can be truncated to UInt16, and target conversion is not a measured CLR boundary probe.

### M226e body: known operations, unproven ordinary entry

**M226e/0600226e**, signature `200101119d00`, is a private nonvirtual T047b instance method. Full 19659-body direct scan finds **no call/callvirt/newobj/ldftn/ldvirtftn site referencing it**. Metadata has no matched self MemberRef or MethodSpec alias. This establishes the static incoming-reference limitation, **not an assertion that reflection/external code can never invoke it**. Do not describe a naturally reached ordinary M226e restoration as observed or proven.

If it is invoked with its value argument, exact instructions are:

1. **M2232() mode + snapshot + current global.F197d→M2e1d at0010**. Ordinary mode0 defaults to fresh record **T059e/M2b40**; other rulesets have other constructors. Snapshot F2045 can attach a fresh policy through M2e1f, preserving E23.
2. Copy **old global F1973** history reference into new record at0021, and separately read/copy **old global F1980** frame-list reference at0031. These are shared existing references, not deep copies.
3. Load handler; **003e store new global F13c3**, then **0043 store handler.F05f3** from the same new reference. The two writes are distinct.
4. Original target.M2979(newGlobal.F1989):005d; combo.M40f1(newGlobal.F198a):0077; handler result/display object.F10f3=global.virtual M2b4b():0091.

M2b40 initializes ordinary record counters/max/current/target/success-like fields from snapshot scalar operands, assigns original global modifier wrapper and metadata, and creates its normal collections. M226e then replaces only the two references above with old ones. Factory-selected policy presence is not restored policy identity or a copy of all its accumulators; this method does not read snapshot F2046/F2047. Its creation policy and the in-place policy write below are different.

M226e does **not** read snapshot F2042, assign T/previous/copy/X/I, invoke M24b3, reset objects/components/nullable time, rebuild candidates or clear D. It can replace record success-like F1978 without clearing the separate Play failed latch F136a. No automatic Play failure follows merely from restored target0.

A constructor/helper failure can prevent installation; a null/interrupted handler store can leave global replaced without completing handler synchronization or target/combo work. No rollback occurs. Natural reachability of this private method is the precise missing connection, so further speculation about its use in replay/retry is stopped here.

## G. The actually connected selective in-place restore

The original managed path **M47a7→M1df7** supplies a recorded-data object T06be. No network/file parser is reconstructed. Within M1df7's F111a lock, require existing **F2a7f and its F1980 list**; append the supplied frame references. Then:

- If old **F1120>0**, **F1120>=current F2a7a**, and **frames.Count−1>=F1120**, preserve the pending old marker/snapshot.
- Otherwise, if supplied snapshot **F2043>0** (`0255 ble` skips zero/negative), set **F1120=frames.Count−1 at0268**, then **F1121=supplied T0740 at0273**.

Other control variants in M1df7 can start/change a Play path; they are not all generalized to already active ordinary gameplay. Actual source configuration, packet arrival and thread order are UNRESOLVED. F2a80 has multiple original writer variants and is not assumed true in a default ordinary live run.

Late original **M226b:109e–10b7** calls **M229f** only if **F2a80=true AND current F2a7a==F1120**. There is no additional positivity test on the marker there. M229f returns normally before F1120 is cleared to−1 at10b7. This is its sole direct managed caller.

**M229f/0600229f** copies static value snapshot F1121 to local0, then selectively writes the **existing** global record:

1. Result counters **F196a/F1969/F196b/F196c/F196d/F196e**, then maximum-like F1975.
2. Current combo through **M40fc:008b**, which conditionally stores F2673 for positive value/Abs difference>10, then invokes **M40f1 at001e→F2674**. This reaches the original current-combo setter, not merely a displayed number.
3. F197c from snapshot F2041.
4. If current F198e null: original **M2b4c(snapshot.F2043)** then F1986=Double(F2043). Otherwise separately reload F198e and copy **F23f9/F23fa from F2046/F2047 at00de/00f3**.

It does **not** replace F13c3/F05f3, restore target F1989/F203d, assign Play failure/snapshot F2040, assign time/F2042, reset any object evaluation/raw/component state, or clear finalization. Thus even the connected route is not a complete gameplay snapshot restore. Fresh global/policy reads and handler dereferences remain separate; source locking does not make this whole late operation atomic.

An exception can leave some counters changed and prevent the marker clear in the caller, permitting a later attempt if its gates remain. Existing object flags, manager group counters and record counters can then describe different histories. Do not invent automatic reconciliation.

## H. Recorded-frame time overwrite is another operation

E10/E28 host order remains **M43a0:00ca→M3b25/M3b24**, then the optional branch **00e0→M43a1**. M43a1 locks F111a; it requires F2a7f and F1980, preserves original application/record/Play/readiness/count/index/recorded-state guards, and works with selected frame references. It is not M226e or M229f.

Within admitted Play2 processing, original next-frame time **<=fresh T**, !F1373 and !M3b0d permit advancing F2a7a and setting F2a7e. Original identical-input/skip helpers can advance more; they are not replaced with a generic timeline search. The examined path advances its index; it does not rewind the index when T is moved backward.

At02ab–02bf select **F2a9f=frames[F2a7a]**. If the separately read **F2a7e is true**, **02d8 stores selectedFrame.F1310 directly to F2358**, then writes input-related state. It does **not** copy this value to F2359/F235a/F2352/F235b or reset objects/records there. Without the selected override, preserve original cursor interpolation and input processing, not an assumed time write. Later draw and judgement can therefore read a T different from the normal clock's last copy/raw/previous fields.

Pause and original state/count/index predicates can prevent index advancement or the override. A backward source seek alone is not proof the recorded frame index moved back or a snapshot was restored. Exact frame-list provenance and active runtime branch require E20 observation. Full recorded input interpolation, playback serialization and control variants remain outside scope.

## I. Next reached update: retained objects, backlog and session state

Account for E28 first: readiness/loading, F1373 pause, F13be resume handoff and other early returns can suppress subsequent ordinary attention. F1376 separately blocks input/spinner integration and E27 finalization, not universally M28e3. A clock API directly called while paused has no own F1373 check, but that does not establish a UI control bypasses its caller's guard.

When admitted, the original order is:

1. Corrected time publication or later recorded-frame override; controls/queued press dispatch.
2. **M2275 current selection from old F17fd**. M22c5 requires nonzero duration/start<=fresh T and `(end>=another T OR !evaluated)`.
3. **M28e2 refresh** using fresh bounds and original full-list BinarySearch; clear or shallow GetRange replacement. No reset/evaluation call. Start-order/end-search mismatch and equal-key runtime behavior remain E07 dependencies, not a rewritten interval filter.
4. Optional current-object **M0ac5** gated by recorded/nullable conditions. **F13d9's sole direct writer is M226b:09b3**; seek/reset/restore does not reset it. Stored value>current T skips the component call; equality permits another call.
5. **M28e3/M28e4** still runs on E12's normal09c9 rejoin, even when that nullable component gate skipped M0ac5.
6. Original result consumers, failed/recovery checks, then **E27 M2276:0f11/automatic request**.
7. Later **M229f:10b1**, if admitted, followed by original pending-control handling. The connected restore is **after**, not before, that pass's E27 decision.

### Circle

Retained F1527=true prevents ordinary spatial/timing selection and M28d3 evaluation, regardless of moving T before the circle's start. F152a is not automatically cleared or replayed. A fresh unevaluated candidate can expire only when the reached original **end+F0adc<T** strict comparison passes, with remaining Play/type/evaluated guards. Equality skips timeout. A large forward jump can instead remove it from candidates; there is no global full-list timeout catch-up guaranteed by moving time.

### Linear slider

F0bf1 successes, F0bf5 failures, F0bf6 Boolean outcomes and F0bf8 deadlines are not reset by the clock primitive or selective record restores. The original next processed index is **unchecked(successes+failures)** before aggregation. Moving T back can reduce the due-prefix count without reducing this index, so previously processed entries are not simply reprocessed when their time returns.

A forward jump can make several deadlines due, but **M146f handles at most one outstanding entry per reached call**. It uses current followOK and original entryTime<=selectedDeadline. Repeated calls at equal T can process further outstanding entries because nullable equality permits; no one-result-per-frame rule follows. Due-prefix-based raw codes can describe an overdue earlier entry, as E21/E22 established.

Parent end<=fresh T can then complete the slider in M28e4/M1465 without draining the remaining schedule. Parent.F1527=true suppresses later M146f even after a backward seek. F0bf1 may already include the positive-head contribution after aggregation, so its sum with failures is not thereafter a clean scheduled index. An explicit partial M1431 reset does not repair it; reused-state effects are artificial/conditional, not established ordinary seek behavior.

### Ordinary spinner: integration and notification are different

A direct seek does not reset previous angle F029d, signed target F02b0, smoothed F02af, accepted P=F02b4, requirement N=F02a0, cached E=F029e or observed count C=F02a2. M05f5 integration rejects evaluated spinners; strict interval/input/update/recovery gates remain E24/E26. Final M05fa grades stored C/N and does not drain P/E differences.

**Do not generalize that guard to M05f6.** The component method has no direct F1527 check, and M0ac5 likewise has none. After a backward time change into a spinner's interval, M22c5's end>=T branch can select it even if already evaluated. If retained nullable-time gate, F2a90 and M05f6's own elapsed-time guard permit, the component method can still examine retained P/E. Its elapsed operand is original F2358−F2a95 (or original default), and a negative finite interval returns before processing; host bookkeeping can later update F2a95. Thus this is a conditional possibility, not a guarantee that a completed spinner emits another component after a natural seek. M28d3's evaluated guard still prevents a duplicate ordinary final evaluator.

Skipped observations can leave C smaller than mathematical integer crossings; a later inequality increments once, not by the difference. A seek does not turn that counter into a reconstructed revolution total. Spinner setup/reset/notification/final result remain separate.

### Session finalization

B=maximum stored object end is written only by fresh Play constructor and loading M223b; D=F1397 only has the true writer M0acd. The scoped clock/reset/restore methods do not change either. Later K is recomputed using original endpoints/clock/marker/queue state. Unevaluated objects or pending components are not repaired by that recomputation.

A forward jump with missing object attention can reach E27 count mismatch: original record invalidation need not veto finalization. An in-place M229f restore can subsequently alter counters **after** that decision; if D already latched, next M2276 skips finalization/validation rather than revalidating the restored history. A backwards seek after D does not clear it, though the separate automatic B+3000/event time gate can become ineligible.

An already issued/deferred M46ad request is independent: its callback rechecks its own live mode guards, not the old gameplay-time predicate. Pending acceptance/commit and later pixels remain E02/E03/E27 boundaries. No time-travel-safe session machine or automatic request cancellation is inferred.

## J. Focused inventories and original body identities

Complete direct selected field/caller sites and owners/signatures appear in the JSON. Key sets:

| State | Complete focused direct managed writer facts |
|---|---|
| F13d9 | M226b:09b3 only; no explicit pause/seek/restore reset |
| F0bf1 | M146f:02d1; M1465:0024 positive-head addition |
| F0bf5 | M146f:031e only |
| F0bf2/F0bf3 | M146d:0006 / M146d:020d and M146e:00fb |
| F0bf4 | M1431:0019 and M1449 presentation-index sites; not success/failure index |
| F0bf6 | Loading M144a:14ed reference assignment; M146f's element writes02df/032c separate |
| F029e/F02a2 | M05f6:04c7 / M05f6:02ed only |
| F02b4 | M05f4:01b3; M05f5:0139/0395 |
| F1397/B | M0acd:0007 / M222e:001f,M223b:0203 |
| F1120 | M1df7:0268; M226b:10b7 |
| F1121 | M1df7:0273 only |

F1527/F152a/F2523/F2524, lists/current refs, manager F17ff/group fields, global/handler records, clocks and source/snapshot state have full watched direct inventories. Other editor/ruleset/lifecycle writers are not assigned ordinary seek behavior. Constructor list assignments do not exhaust later Add/Clear/array mutations. No direct-store inventory excludes arbitrary indirect mutation.

| Method / original signature | Original method-body SHA-256 |
|---|---|
| M3b3a /`000301080202` | `010e12ccb0251199e280dada9bff835c7306626a8fce3ca0d473fc722d627606` |
| M2241 /`200001` | `1bdc4924250b318a1ad8a25603187518aee9a33b0bffed54ff1b7a719535e3e5` |
| M24b3 /`200001` | `0d901eafda7a12688f81d50deaae6fe446e496b0fa14ba61d5db80387b9b7f57` |
| M1431 /`200001` | `2636e127e33bd10059923d73b504a53eafb544b3daed892f1cc5d112efeca605` |
| M226e /`200101119d00` | `cd4785e46d96c12cdeda72431a48c6f584a53922bb12013ad7f7532fb50c48a3` |
| M229f /`000001` | `1b97c014c1f63608f54cd5efa5969e5a291b36edb932632120bff1cc3732dec2` |
| M43a1 /`000002` | `edfa1f3182083b8b5a88fadab23c8945846f38a381652e8ad4b6245529807008` |

## K. Conditional static cases

All rows use **CONFIRMED instruction branches** to derive **RECONSTRUCTED conditional predictions**. All runtime observations are **NOT RUN — WINDOWS HOST REQUIRED**. Reuse unchanged proposed circle/E16 slider/E24 spinner controls, separately. Original import, actual source/clock/bindings and helper returns are unverified. Arbitrary backwards/forced/reset/value probes are artificial unless the named original caller is reached; no result history is fabricated.

| ID | Condition | Original branch-derived prediction | Runtime |
|---|---|---|---|
| S01 | Ordinary startup/skip forward request before first circle | Only reached original caller gates establish the seek; no universal rewind control inferred | NOT RUN |
| S02 | I==request,T==request,!force | M3b3a returns before bounds/helpers/source work | NOT RUN |
| S03 | Same operands,force | Early return bypassed; bounds and q arm still apply | NOT RUN |
| S04 | Nonzero q,!C,!force, absolute request−T exactly200 versus201 | Skip versus issue source seek; both still setq2/run clock | NOT RUN |
| S05 | Lower bound violated,!C,!bypass | Clamp to−O and skip upper test | NOT RUN |
| S06 | BypassBounds true | Original lower/upper assignments skipped, other branches retained | NOT RUN |
| S07 | q0,force true | Hold/request path, not unconditional direct source seek | NOT RUN |
| S08 | Hold release H0 versus nonzero | Original source request0 versus unchecked(H+O) | NOT RUN |
| S09 | Ctrue seek, C changes inside M3b24 | Later branch uses fresh C, not entry snapshot | NOT RUN |
| S10 | Direct seek while F1373true, artificial if no reached control caller | Clock helper can run; paused Play still suppresses later object work | NOT RUN |
| S11 | Recognized Skip while paused/F13be | Original caller M2256 rejects, no claimed direct-call bypass | NOT RUN |
| S12 | M2241 admitted, object.end==fresh T | Mark existing object evaluated without evaluator/result event there | NOT RUN |
| S13 | M2241 marks a past slider parent | No automatic embedded-head classification or component drain | NOT RUN |
| S14 | Backward seek after circle evaluated | Retained flag blocks ordinary input and second manager evaluation | NOT RUN |
| S15 | Same circle, new T before its start | Clock movement does not undo flag/raw result | NOT RUN |
| S16 | Forward T>end+widest width, untouched circle still candidate | Strict timeout can call original evaluator under remaining gates | NOT RUN |
| S17 | Same jump removes circle from candidates | No guaranteed full-list timeout catch-up | NOT RUN |
| S18 | Backward T after one processed slider checkpoint | Counts/outcome remain; lower due-prefix does not decrease processed index | NOT RUN |
| S19 | T below retained nullable component-time value | M0ac5 skipped, but corrected manager-update rejoin still eligible | NOT RUN |
| S20 | T equals nullable value; more due entries outstanding | Repeated reached calls can process more entries, not one per distinct T | NOT RUN |
| S21 | Forward jump across several checkpoint deadlines | At most one outstanding entry per M146f call; current follow/entry predicates retained | NOT RUN |
| S22 | Same pass tracking then parent completion | One result may precede parent aggregate with backlog still unprocessed | NOT RUN |
| S23 | Backward seek after parent evaluated | M146f returns0; no aggregate/history undo from clock primitive | NOT RUN |
| S24 | Artificial M1431 on used parent | Clear parent/head evaluated and F0bf4; retain head raw/counts/outcomes/entry/active | NOT RUN |
| S25 | Later normal tracking leave, parent unevaluated | Reset typed F0bd5 children, not F0bdf head/parent by identity | NOT RUN |
| S26 | Seek while spinner active | Prior angle/motion/P/C/E/N retained until original later methods change them | NOT RUN |
| S27 | Seek after spinner final evaluation | M05f5 rejects evaluated; no second manager final judgement without separate reset | NOT RUN |
| S28 | Rewind into evaluated spinner interval; all component/elapsed gates permit | Current predicate can select it; M05f6 lacks evaluated guard and can inspect retained P/E | NOT RUN |
| S29 | Several crossed integers with one later observation | Existing inequality increments observed count once; no mathematical catch-up | NOT RUN |
| S30 | Artificial M0606 reset on used spinner | Evaluatedfalse only through base; P/C/E/N/motion remain | NOT RUN |
| S31 | Separate spinner setup/end reprepare actually reached | P/N setup distinct from retained C/E; not automatic seek behavior | NOT RUN |
| S32 | M226e requested as presumed ordinary restore | No static incoming site established; natural entry remains UNRESOLVED | NOT RUN |
| S33 | Artificial M226e with failure after new global store | Partial record replacement possible before handler/target/combo completion | NOT RUN |
| S34 | M229e capture | Scalar snapshot includes time/target/counters, not chart-object history | NOT RUN |
| S35 | Existing pending marker remains within appended frame range | M1df7 can preserve old marker/snapshot rather than replace it | NOT RUN |
| S36 | F2a80true,index==F1120,late Play path reached | M229f copies selected counters/combo/policy; clears marker only after normal return | NOT RUN |
| S37 | F2a80false or index differs | No M229f from that late callsite | NOT RUN |
| S38 | Existing policy null versus nonnull | Original M2b4c/F1986 versus F23f9/F23fa updates, no policy-presence replacement | NOT RUN |
| S39 | Snapshot time/target differs from current live values | M229f does not restore those operands or object evaluation | NOT RUN |
| S40 | Recorded next frame time equality, pausedfalse,qnonzero and other guards permit | Original index advancement can set override flag | NOT RUN |
| S41 | Selected recorded override reached after normal clock | T overwritten alone; copy/previous/X/I need not agree | NOT RUN |
| S42 | Backwards clock with retained recorded index | No inverse frame-index search in examined M43a1 path | NOT RUN |
| S43 | Forward jump reaches E27 with result-count mismatch | Invalidation can coexist with finalization; no component repair | NOT RUN |
| S44 | Seek after Dtrue to earlier T | B/D retained; automatic time gate can fail, recognized departure still independent | NOT RUN |
| S45 | M229f occurs after same-pass E27 finalization | Counters can change after validation; D prevents ordinary repeated finalization | NOT RUN |
| S46 | Departure already deferred before clock rewind | Request callback applies own live mode guards, not old map-time test | NOT RUN |
| S47 | Wrapped AbsInt32.MinValue/native/helper exception | Retain partial preceding clock/index writes; no rollback/reset invented | NOT RUN |
| S48 | M3b66 callback failure caught inside its region | Timing sentinels reset; earlier clock mutation retained, notification success unproven | NOT RUN |

Every case remains conditional. No fixed host/frame rate, successful native seek, actual input timing, displayed grade, observed combo or audio result follows from this matrix.

## L. Minimal E29 addition to future E20 observation

Preserve E20's existing packet and use it only after authorized Windows process/import/display/input/audio/debugger capabilities are verified. Add this checklist in the new report only:

- Exact EXE/fixture/audio/settings/environment hashes and actual Play/manager/handler/source/global-record identities; original current/candidate/full object references.
- Actual seek caller/arguments/guards, each C/q/hold/H/X/I/O/T/copy/previous value, source seek request/return and timing-point callbacks, before/after and exceptions. Never call a clock helper merely to log it again.
- Object S/E/evaluated/raw, slider head/parent/tracking/count/outcome/deadline identities, spinner P/N/C/E/angle/motion and nullable F13d9 before/after. Separate any real M24b3 call from a candidate refresh or clock store.
- Actual snapshot capture/source/marker/index, M229f gate/field writes and record identities; do not claim M226e naturally reached without its runtime caller. Forced direct invocation is a different probe.
- Consumed recorded frame and override versus normal clock/copy; subsequent current selection, candidate refresh, optional component evaluation, manager update/result and E27 K/D/count validation/request ordering.
- Thread IDs/sequence/QPC, traced versus matched untraced controls and measured overhead. Natural forward/skip/hold controls versus forced backward/time/reset probes must remain distinct. Keep proprietary/private traces out of Git; publish permitted summaries linked by hashes.

**UNRESOLVED:** original import/settings/receiver selection, actual source seek outcome and corrected time, natural ordinary backward-seek control, M226e entry, callback/record source and concurrency, partial-restore configuration, exact resumed attention/interleaving, pixels/audio/latency/overhead. Static checks do not satisfy E20 acceptance.

Smallest original-Windows dependency: one ordinary seek/hold or skip trace recording both requested and consumed time, object identities/evaluated flags, F13d9 and next update entries, plus a separately identified selective-restore trace if its original source is available. If further static work is justified while Windows remains blocked, investigate the original seek-control/provider selection and its permitted ordinary versus recorded/special Play entry; stop before full replay/file/network lifecycle.

# 3. Pseudocode

**RECONSTRUCTED — independent non-executable projection. Keep original arithmetic, fresh reads, callbacks and exceptions; this is not a seek simulator.**

```text
M3b3a(request, bypassBounds, force):
    if rawRoundedTime==request and separatelyReadT==request and not force:
        return
    retain original Edit/helper work and timing-index invalidation
    apply original lower-bound branch, including its skip of upper test
    otherwise apply original upper-bound branch
    if synthetic:
        retain original accumulator/working-time/rounded T writes
        original main clock()
    if freshly read not synthetic or force:
        if freshly read q != 0:
            if original Abs(wrapped request-T)>200 or force:
                original receiver.seek(Double(wrapped request+offset))
            q = 2
            original main clock()
            previous = fresh T
        else:
            holdPosition = request
            hold = true
            original main clock()
    original timing-point reselection/callback helper
    // No inserted object reset, record restore or component catch-up.
```

**RECONSTRUCTED — distinct partial operations:**

```text
base object reset M24b3:
    original presentation callback loop removes tagged transforms
    evaluated = false
ordinary slider reset:
    original base reset
    original resource hook
    original embedded-head reset
    presentationCheckpointIndex = 0
    // Counts, outcome array, schedule and stored head raw are not cleared here.
ordinary spinner reset:
    original base reset

M2241 loading mark, only on its reached original guard:
    for existing full-list object using original live reads:
        if object.end <= fresh T:
            object.evaluated = true
            original target setter(object.F1529)
    retain timing-point suffix
```

**RECONSTRUCTED — record and frame paths are separate:**

```text
M226e(valueSnapshot), incoming ordinary caller UNRESOLVED:
    construct original mode-specific record from scalar snapshot
    preserve separately read old history/frame-list references
    globalRecord = newRecord
    handler.record = same newRecord
    original target/combo/result-field writes
    // No object/time restoration inserted.

connected late Play restore:
    if original F2a80 and recordedIndex == original pending marker:
        M229f copies scalar snapshot to local
        restore selected existing-record counters/max/current combo
        restore original flag and aggregate-or-policy content branch
        clear pending marker only after normal return

recorded frame path after normal clock:
    retain original lock, eligibility and index/input processing
    if original override flag:
        T = selected frame time
    // Copy, previous and working/raw time remain separate.
```

**RECONSTRUCTED — later ordinary update boundaries:**

```text
if original readiness/pause/handoff guards allow:
    select current from old candidates
    refresh shallow candidate references using original search
    if original recorded/nullable time gates permit:
        invoke original current-object component method
    original manager object update/evaluation on normal E12 rejoin
    retain result/failure/recovery/session-finalization logic
    retain later selective record restore and pending control work
```

**UNRESOLVED original-runtime selection remains unresolved. E20 is incomplete. Work 2 remains suspended.**
