# 1. Result

For an osu!standard player, returning from recorded input is not itself a new Z/X press. The original game separately chooses its input path, builds logical held buttons, compares them with previous logical buttons, queues a shared press callback, and decides whether Play may accept it. A key held across a change can yield no new edge when the previous recorded logical button was already held, or an edge when it was released. Both predictions require the original live seeding, keyboard, policy and gameplay gates; neither predicts a hit merely from disabling recorded selection.

**Work 1, research-only. Work 2 remains suspended. E20 remains preparation-only and incomplete: all 17 original-runtime cases are NOT RUN — WINDOWS HOST REQUIRED.** No original EXE, CLR, native input/audio method or replacement input model was executed. No Windows observation, latency, visible effect or naturally successful return-to-live hit is claimed.

Dedicated branch `work1/expedition-31-provider-departure`, based directly on E30 publication `a6350f567820f4f1420970a5dbbcf2e63197b9d0`, base tree `25d1c22cb0a675a940e10ca8f7171935a4271751`. The local checkout was recovered after workspace maintenance using the actual Git commit; no substitute parent or reconstructed history was used. Publication identity and remote verification are supplied in the publication response, avoiding a self-referential commit hash here.

New files: this report, `evidence/31/provider-departure.json`, `tools/provider_departure_evidence.py`, and `tools/check_provider_departure.py`. The only historical-file change is an E31 append to `contexts.md`. The complete prior journal prefix, reports, evidence, tools, fixtures and E20 packet are preserved. `reconstructed/` was neither inspected nor modified.

Reference integrity, rechecked from actual bytes:

| Input | SHA-256 |
|---|---|
| Attached `01-osubackup.zip`, original archive content | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| Archive member `osu!.exe` | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Owner's complete attached standing contract | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |

The archive has 18 entries and passes CRC checking. Stable40/file-version1.3.3.8 is the executable's self-description, not an authenticated official date or proof of unmodified release identity. The actual host is Linux x86_64, with no available authorized Windows process, interactive Windows display/input, CLR debugger or profiler. A future Windows PC is not assumed accessible. Proprietary binaries, assets, full disassemblies and private dumps remain outside this repository.

Reproduction, from the repository root with Python and pinned `dnfile0.18.0`, `dncil1.0.2`, `pefile2024.8.26`:

```text
python re/tools/provider_departure_evidence.py /private/osubackup.zip > /private/e31-a.json
python re/tools/provider_departure_evidence.py /private/osubackup.zip > /private/e31-b.json
cmp /private/e31-a.json /private/e31-b.json
cmp /private/e31-a.json re/evidence/31/provider-departure.json
python re/tools/check_provider_departure.py re/evidence/31/provider-departure.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

The extraction scans all 19,659 decoded managed bodies, 1,739 zero-RVA methods, zero decode errors; it records selected instructions for 111 methods, 25 setter-call windows, direct field/access inventories and hashes for 37 focused writer bodies. It reuses unchanged earlier metadata/CIL collectors; it is not an input processor. Offsets below are hexadecimal relative to the first CIL opcode, not a header or JIT/native address. `Mxxxx`, `Fxxxx`, `Txxxx` expand to MethodDef `0600xxxx`, FieldDef `0400xxxx`, TypeDef `0200xxxx`. Original body hashes include the original method header/body bytes as in earlier expeditions.

Focused instruction checks, byte-identical repeated extraction, all 28 prior research checkers (E02–19 and E21–30), unchanged E20 package checks, Python compilation and historical preservation are required and recorded with publication. These are static/material checks, not E20 gameplay acceptance tests. The final focused check compares 276 historical method-body hash occurrences. E05's ordinary/Taiko correction, E12's update-order correction, E14's resource ordering, E24's ordinary spinner correction and E28–30's gates/retained-state/provider distinctions remain authoritative.

# 2. What they found

**CONFIRMED** means original metadata/instructions. **RECONSTRUCTED** means interpretation or a conditional prediction from those instructions. **HYPOTHESIS** is an unverified explanation. **UNRESOLVED** identifies missing original runtime/helper evidence. All conditional cases below have **NOT RUN** original-runtime status.

### Reference identity and direct evidence

The global input owner is **T08e5**; the physical raw/latch owner is **T025f**. They are not fields allocated anew by each Play object. The ordinary ruleset remains **T0253**, with ordinary manager **T00e8**; recorded selection does not retype that ruleset. Base Play **T047b**, recorded/generated Play **T0a10**, and special **T00ff/T04af** remain distinct concrete receivers.

| Boundary | Declaring type; original signature | Body SHA-256 |
|---|---|---|
| M4391(Boolean) | T08e5; `00010102` | `38998cd44335c49e648264282354f631fc2018399bf614671952cc0c643c6d16` |
| M43a0(component callback) | T08e5; `000101129afc` | `11fbe0877b6a113594d3b9cf45c19deeb2d8681e2ed8bb2f37e2e54e3ce940a4` |
| M43a1() Boolean | T08e5; `000002` | `edfa1f3182083b8b5a88fadab23c8945846f38a381652e8ad4b6245529807008` |
| M43a7() | T08e5; `000001` | `79daf889ccf8b407f65bb497c41d81aa1e6c19633cf453ac83752df5f8ed926e` |
| M11b4() Boolean | T025f; `000002` | `e959af7e3f518de30d68b87ff24bed7f23ff67c5a63d25266f9248040a482918` |
| M21b5() | T0466; `200001` | `5c8bb756c211a5da7c994efe0a7b39293b63372d5683c4ac0c43e58309d857ec` |
| M1dfb(Boolean) | T03f6; `00010102` | `26ee38384b9794c5d5ab62f609fa151314eadcbb76762904f27636bf9233e15a` |
| M2266(Boolean) | T047b; `20010102` | `2a122562d93e25cdf5e9f4f574bb71ee00a6581844f147063a6a2010ca217342` |

All other selected body hashes, owning types, signatures, branch destinations, exception regions, virtual declarations and field reader/writer sites are in the evidence JSON. Original names for the input classes/fields are obfuscated. Two value-type identities are meaningful in metadata:

* **T0a79**: `Microsoft.Xna.Framework.Input.ButtonState`, literals **F3047 Pressed=1**, **F3048 Released=0**. Logical/current/previous aggregate and physical raw/latch fields use signature `0611a9e4`.
* **T0b50**: `osu_common.Bancho.Objects.pButtonState`, F3793 None0, F3794 Left1=1, F3795 Right1=2, F3796 Left2=4, F3797 Right2=8, F3798 Smoke16. **F2a66** has signature `0611ad40`. It is not the same representation as a keyboard snapshot or a new-edge flag.
* **F2a64/F2a65** have signature `06119154`: value-type **T0455**, with original bit-test M20c5→M20c2, not an array of captured key events. **F2a7f** is a T059e reference (`06129678`), **F2a9f** a T0463 frame reference (`0612918c`), **F2a7a** Int32 (`0608`). Boolean fields use `0602`.

### A. Departure changes selection and references, not every history

**M4390** simply reads F2a7c. **M4391:0001** stores its Boolean argument to F2a7c, then reads that field again at0006. If the later read is true it returns at0019. If false, **000e clears F2a7f** and **0014 clears F2a9f**. Those are its only direct field stores. The setter has no lock, queued action, Play construction, clock reset, frame-index reset, keyboard-history normalization or gameplay-press-queue clear. This describes direct instructions, not a promise that an unrelated caller/helper cannot mutate other state.

The exhaustive direct managed caller inventory has **25 sites in 22 methods**. The evidence supplies each owner/signature/body hash and a bounded original argument/continuation window:

| Caller sites | Supplied selector value; bounded provenance |
|---|---|
| M0718:002b | false; T00ff disposal before base M2266 |
| M071a:0042 | supplied Boolean; guarded T00ff reselection |
| M0dc4:003e | false; original locked mode-specific setup boundary, followed by streaming false |
| M1dfb:0030 | false; conditional association departure described below |
| M1dfc:0089 | false; association replacement/admission under F111a |
| M1dfd:005e / 01df | true on source setup / false on original rejection continuation |
| M21a8:0016 | false; T0466 disposal |
| M21b5:00f3 | inverse original local Boolean; T0466 reselection |
| M2237:0395 | true; E30 generated/selected-source initialization |
| M223b:0344 | false; rejected empty nonstreaming recorded input |
| M224a:0016 | false; guarded base initialization |
| M225b:0073 / 0295 | false on special departure / true on paused failed-session recorded-source request |
| M2285:0025 | false; special F2c73 departure, then request Edit1 |
| M22b3:0016 | false; captured loading continuation; retain selected-map/setup helpers |
| M2571:0110, M2d96:0040, M32db:0159 | false; original mode-specific locked boundaries, followed by streaming false |
| M25be:0063 | true after source assignment; request Rank7 |
| M32e3:009f / M32e5:0060 | true before source assignment; distinct original receiver boundaries |
| M3cf4:0014 | false; original setup, then control subscription |
| M46ba:037f / 04a6 | true after source assignment / false after original callback setup |

The inventory is exhaustive for direct `call/callvirt/newobj/ldftn/ldvirtftn` token sites in the pinned EXE, not for reflective/native/external calls. Unrelated original mode/loading callers are inventoried without reconstructing their menus, networking or full lifecycles. Their presence does not establish a live ordinary replay-toggle shortcut.

**Base initialization:** M223b tests recorded selection separately before virtual **M224a:025a**, handler initialization0265, and another possible M224a0272. Base **M224a** calls M4391(false):0016 only if **!F2c73 && !F137c && !F137b**, then always reaches **M1dff:001b** if its prefix returns. M1dff constructs a fresh empty F111f, invokes original M1dfa(NewSong1, default nullable), then **M1dfb(false):001a**. Its outgoing/helper work is not declared side-effect-free.

**M1dfb** has separate operations. Only a true argument plus `(currentMode==Play2 || (currentMode!=Menu0 && F2c48))` reaches selectorfalse0030 and requests SelectPlay5,true when F1118 is null, otherwise Menu0,true. A false argument skips this selector/request block. Independently, if F1118 exists, it locks F111a, retains original outgoing/observer helpers, clears **F1118:0098**, sets **F2a80=false:009e**, then later **F2a81=0:010b**. It clears F111e/F111c and calls M46b7 afterward. Exceptions or callbacks can interrupt; no transaction/rollback is added. Thus calling M1dff does not universally clear selection/source, and clearing an association is not equivalent to normalizing input history.

**Ordinary recorded departure:** phase-ready M225b recognizes the original Pause binding18 or numeric key27. After original loading/readiness and finalized-state alternatives, a non-T04af receiver with recorded selection or failed state takes its narrower branch. F2c73 uses selectorfalse0073 and M46ad(Edit1,false):007a. Otherwise a fresh true selector calls **M1dfb(true):0089**, then **this.F13bb=7:0090**. The paused suffix can issue another departure request; the unpaused suffix writes F2366=300. T0a10 **M4abd:0043** delegates unhandled controls to base M225b. None of these calls commits a new Play in the setter.

**Loading rejection:** M223b's `!F13f2 && selector && !F2a80 && source.frames.Count==0` branch requests Rank7 or SelectPlay5 according to F1118, then disables selection at0344 and returns after original error helpers. It is not evidence of gameplay entry with artificial frames. A null source at a required dereference can throw rather than being normalized to an empty list.

**Source exhaustion:** M43a1 with a valid source/list can set **F2a82=true:0064** when the nonstreaming index equals count−1. Its fallback/paused/failure/buffer paths are retained. It contains **no M4391 call**. End-of-source marking is not automatic return to live input, clearing logical buttons, or committing base Play. With a null source/list it leaves before advancefalse0040 and eligibilityfalse0046; an old F2a7e value can remain.

### B. Same-instance changes and actual Play commitment are different

**T00ff:** M071b first checks its M0719/M4390 state and original action-key predicate M2c59, then calls **M071a(false):0017**, before base M225b001f. For Osu0, M2c59 tests configured action bindings2/3; this is a specific special receiver, not a base Play shortcut. M071a returns unless the selector would change, F2c73 is true, T is before the last full-list object's end, and F13d8 is ready. It then calls M4391(argument):0042. Its false arm skips source/index preparation and ends through the original feedback helper; it does not construct another Play or call M43a4. The true arm assigns F035b to source, advances/resynchronizes index with original −2/clamp0 logic and can mark past full-list objects evaluated. E29's distinction from ordinary seek is unchanged. M0718 disposal separately disables selection before base disposal.

**T04af/T0466:** T04af constructor M23f0 uses an original configuration switch; cases1/3 construct **T0466 via M21a2:0098**, stored as base T01f4 reference F14b2:009d. Other cases construct different helpers; no universal T0466 assertion. The update wrapper **M2405:00cb** calls helper virtual **M0e1f**, which resolves to **M21b5** on T0466. Earlier in that wrapper, M226b:0077 has already run. T0466 therefore offers a concrete callback-time reselection path within the same T04af object, after the host press-edge/queue stage.

M21b5 retains its timing, selected-identity and original M21a7/M21a5 predicate helpers. If its later selector equals the original local Boolean `b`, it calls **M4391(!b):00f3**, calls owner **M22a8:00fe**, then reads selection again. False reaches **M43a6:0326**. True assigns existing **this.F131a to F2a7f:0119**, retains index searches/mutations and original inserted-frame preparation. The meaning/natural selection of `b` remains an original helper/configuration dependency; it is not renamed a user setting or ownership guarantee. M22a8 itself retains M2259 and parsing/record-related stores, not an invented input reset. Disposal **M23f9:0041→virtual M0e1c→M21a8** disables selection at0016, restores F2a78=true, retains M21b4/observer/base disposal. These methods do not retype their owner.

**New Play:** E30's M46b0 constructor priority remains: earlier match/editor alternatives, then recorded selector/streaming/Autoplay2048 can select T0a10; otherwise base T047b. M4391 does not run that switch. T0a10 initialization **M4abc** resets index0:0024 and calls **M43a4:0029**. M43a4 clears exactly ten fields: F2a67/69/68/6b/6a and F2a83/85/84/87/86. It does not clear F2a6c/88, F2a64/65, F2a6e/8a, F2a66 or mouse history. An old previous logical1 plus reset current0 is a permitted intermediate state; subsequent original polling decides what happens.

**Base disposal:** M2266 stores Play state6, clears F137b/F137c/F13be/F1373, publishes global record from virtual M2274 and clears F136d/F137d. It retains original record/audio helpers, **M2248:00f3**, manager cleanup M28c6 and later handler virtual M0aad; **index0:02c1** is separate from selector reset. The ordinary handler inherits M0aad, which removes a Play delegate through M228f and retains observers/resources. M2248 removes its control/clock/manager subscriptions. These are not proof of clearing F2a59 or every logical history field.

**Input subscription lifetime has another boundary.** M2246 registers M2295 for event7 via **M43b6:0023** at priority2. M43b6 registers through M2d46 and queues its removal action in **F2aa4**, a different queue from gameplay-press **F2a59**. Original **M43b4:001f→M467d** registers M43c3 on departure delegate F2c55. On a reached mode commitment **M46b0** stores current mode at0072, handles old-object disposal or deferred disposal, and invokes F2c55 at00f0; M43c3 drains F2aa4, whose M43cb→M2d47 removes subscriptions. Preserve conditional original registration, callback order and exceptions. Do not call that a selector-triggered press-queue clear.

E03 remains authoritative: M46ad can defer, redirect, suppress or accept a request; acceptance writes pending F2c6c:022a then phase F2c6b=0:02e8, with intervening helpers. It does not invoke the later constructor/commit switch. An accepted departure's phase0 can prevent new shared-press enqueue and causes M2295 to reject. A deferred/vetoed request is not proof that those later guards pass either: Play state7, readiness, higher handlers and original callbacks still matter.

### C. Host selection, publication, polling and late history writes

**CONFIRMED ordering in M43a0:**

| Stage | Original sites and distinct reads |
|---|---|
| Prefix path selection | Save old F2a6f; M4390:0007. True selector→false path flag. False selector normally !F137c→true; F137c additionally uses pause/F1b97/optional Play.F1397 branches. Store F2a6f:0040. |
| Physical provider | M11a2; M11a3:0054→F2a93:0059, then synchronous M11a4:005e. Physical provider selection continues to exist on recorded passes. |
| Normal clock | **M3b25:00ca**, then F2a90=host F2c30:00d4. Preserve E10/E28 clock behavior and separate frame overwrite. |
| Selected path | Read F2a6f:00d9. False→M43a1:00e0. Afterward fresh !selector plus F137c may seed via M43a6:00f9. True→unless F2c24, M43a6:010c and live pointer work; mapped F2a7b:0162. |
| Polling guard | F2a79 required; lack of focus M4235 is tolerated only by fresh recorded selector. Rejection goes0738, clearing logical aggregates/modifier keys, then still reaches queue drain. |
| Physical snapshot | M1333:01d0→F2a64:01d5 inside a narrow try/catch. Poll failure can retain old snapshot. |
| Extra/key combination | Optional F03cb→M12f6:0223; supplementary list F1b50 added. Fresh selectortrue skips M43a7, otherwise call023e. Generic controls M43a2:0248 run afterward. |
| Logical new edges | F2a6e:02df and F2a8a:0316 from current logical aggregates and policy/previous state, after generic controls. |
| Shared press | Reset common F0a79:04b0 on the reached processing arm; new-side path sets F0a7a=true054a/F0a79=true055a. With nonzero phase, enqueue parameterless M11a9 at0573 and/or0590. |
| Dispatch | Drain F2a59 through Dequeue0778/Invoke077d until count≤0; then invoke component callback0790. No selector cancellation is inserted here. |
| History after callback | Current logical left/right→previous F2a6c:079a/F2a88:07a4; Boolean forms F2a6d:07b1/F2a89:07be; mouse snapshots F2a72/74/73:07c8/07d2/07dc; current keyboard→previous F2a65:07e6; conditional time history, then M11b4:07fc. |
| Possible repeat | Mouse reconciliation true, or original selector/path-and-lag predicate, leads to bounded counter test0820 and backedge **00ca**, not0007. Up to31 component passes on this bounded path; no claim about passes per physical frame. |

F2a6f is a global computed on the prefix, not an immutable local snapshot of all later state. The repeat does not recalculate it from a new selector. A normal new outer invocation does. Other explicit writers/reentrant calls would need their own evidence. E12 selection-old-candidates→refresh→optional component processing→ordinary object updates remains unchanged inside reached Play callbacks. E28 pause/resume/recovery can reject a press or return from M226b after host input work already occurred; none implies the host has frozen all input history.

**Concrete synchronous interleaving evidence, not a race assumption:** M11a4:004a dispatches numeric key27 through M379a after path selection but before clock/frame work, under original focus/mode/extra-mouse-edge gates. M43a2:03d5 similarly dispatches newly recognized controls after configured-key processing but before logical edge calculation. M379a first offers M379c, then invokes registered handlers in original order using DynamicInvoke. M2246 registers virtual M225b at0035; T0a10 delegates the relevant unhandled controls to base. Therefore a reached original control can disable selection within that same call stack. Higher handlers may consume it; eligibility is conditional.

If selection becomes false after F2a6f=false was chosen, a later M43a1 can see a now-null source and return without clearing its old advance flag. For ordinary !F137c the post-frame fallback does not seed M43a6. A fresh selectorfalse can nevertheless allow M43a7. Retained logical state can then be ORed with physical state rather than rebuilt from released mouse state. A switch through M43a2 occurs later still: frame/button/time work and the decision to skip M43a7 may already have occurred. The stored cursor may remain frame-derived; M2295's fresh selectorfalse merely chooses live mapping of stored F2a5c, not a promise of a freshly sampled physical cursor.

A helper reselection during component callback0790 is later than press-edge dispatch but earlier than previous-history stores. Those stores consume post-callback values. For example, a callback's M43a6 reseed can change the current mouse aggregate that is copied into previous history, even though this pass's new-edge flags were calculated earlier. No atomic current/previous/button/provider snapshot is manufactured. Actual OS callback threads, native input timing, concurrent selector mutation and observed cadence remain **UNRESOLVED**; separate reads alone do not prove a data race.

### D. Held buttons, keyboard history and physical latches

**Direct writer inventory.** The complete direct reader/address/store sites for every watched field are in `entry_field_sites`; method owner/signature tables and writer body hashes accompany them. The following lists every direct managed store to the central history fields, including nonordinary or auxiliary boundaries instead of silently excluding them:

| Field(s) | Direct writer sites |
|---|---|
| F2a64 / F2a65 | M43a0:01d5 / 07e6 only |
| F2a6c / F2a88 | M43a0:079a / 07a4 only |
| F2a6d / F2a89 | M43a0:07b1 / 07be only |
| F2a6e / F2a8a | M43a0:02df / 0316 only |
| F2a71 | M0aaf:003d only |
| F2a67 | M0c6a:005f; M43a0:0739; M43a1:02a0/03b1; M43a4:0001; M43a6:0005; M43a7:01bc |
| F2a83 | M43a0:073f; M43a1:02a6/03c8; M43a4:001f; M43a6:004a; M43a7:01e5 |
| F2a68 | M0c6a:002b/0042; M12f6:01a2; M43a1:030d; M43a4:000d; M43a6:0034; M43a7:0033 |
| F2a6a | M12f6:01c6; M43a1:033c; M43a4:0019; M43a6:0040; M43a7:0094 |
| F2a84 | M12f6:01ea; M43a1:036b; M43a4:002b; M43a6:0079; M43a7:00f5 |
| F2a86 | M12f6:020e; M43a1:039a; M43a4:0037; M43a6:0085; M43a7:015e |
| F2a69 | M0c6a:0052; M12f6:0159; M1ee0:026e/05dc; M43a1:0288/02fe; M43a4:0007; M43a6:0027; M43a7:0067 |
| F2a6b | M12f6:0165; M1ee0:0283/05e3; M43a1:028e/032d; M43a4:0013; M43a6:003a; M43a7:00c8 |
| F2a85 | M12f6:0171; M1ee0:0297/05ea; M43a1:0294/035c; M43a4:0025; M43a6:006c; M43a7:0129 |
| F2a87 | M12f6:017e; M1ee0:02ac/05f1; M43a1:029a/038b; M43a4:0031; M43a6:007f; M43a7:0193 |
| F2a66 | M229b:01a9; M43a1:0541 |
| F2a72 / F2a74 / F2a73 | M43a0:07c8 / 07d2 / 07dc only |
| Raw F0a72 / F0a74 | M1188:000b, M11b6:0007 / M118a:000b, M11b6:0013 |
| Latched F0a82 / F0a83 | M1188:0005, M11b4:0041, M11b6:0001 / M118a:0005, M11b4:004b, M11b6:000d |
| F0a79 | M43a0:04b0/055a only |
| F0a7a | M43a0:054a/06d0 only |
| F2a59 queue reference | M438f:0019 only; its contents can still be enqueued/dequeued |

Selector/source/index/advance inventories are re-extracted completely and agree with E30. In particular F2a7e stores remain M43a1:0040/0149; selected frame stores remain M4391:0014/M43a1:02bf. No assumption of indirect purity follows from these direct inventories. M0c6a belongs to T01ba, M1ee0 to T0421, outside exact ordinary T0253's ancestry; their global writes are not transferred to ordinary gameplay. Optional **M12f6**, reached from M43a0 only with F03cb, has its own readiness/state sampling and fresh selector gate0148; when selectorfalse it ORs auxiliary button predicates/held states into the four channel fields. Its device/provider configuration is outside scope and remains a live operand, not an ignored no-op. The conditional physical-only table below assumes it and supplementary sources contribute no additional held input.

**Ordinary edge policy:** virtual T0253.M0aaf resolves to **M1140**, which calls base **M0aaf:0018**; base initialization eventually invokes virtual M0aab at0038 and stores F2a71:003d. Exact T0253 inherits **M0aab returning false**. This is the field's sole direct writer. The true policy remains a supported alternative receiver/state, not an ordinary default inferred from memory.

**M43a6** reads latched mouse via M1187/M1189 and overwrites logical aggregates at0005/004a, derives current channel-held and per-channel new flags, and clears the secondary channel flags. It does not modify previous aggregates, previous physical keyboard, selected frame/index or raw/latch state. **M43a7** on application Play2 gets original bindings M2c54, ORs current snapshot key tests M20c5 and supplementary held list M2e93 into channel-held fields, and ORs new predicates into channel-new fields. Under policytrue those predicates consult previous snapshot F2a65 and previous supplementary list M2e94; policyfalse permits held state. At01a6/01cf it reads selection afresh; only !selector and the original pause/M1ffb condition allow stores of aggregate Pressed1. It contains no matching unconditional aggregate Released0 store.

With policyfalse, **M43a0:02ad–02df** says left-new is `(current left==Pressed1 && previous left!=current left)`. Right mirrors this at02e4–0316. This is equality/inequality on the ButtonState enum, not a counter or physical key-down event. With policytrue, current Pressed1 instead combines per-channel new flags F2a69/6b or F2a85/87. Physical keyboard snapshots still update on admitted recorded passes, even when M43a7 is skipped.

**Conditional physical-only ordinary table** — stable selectorfalse before the prefix, live seeding reached, Play2, policyfalse, original polling/auxiliary/pause guards permit combination, and no intervening callback changes these operands. `L` is the post-seed/post-combination logical side, `H` the previous logical side retained from the last tail. This is an interpretation table, not executed tests or a substitute input processor.

| Previous H | Latched mouse | Bound physical key | Resulting L; new edge |
|---|---|---|---|
| Released0 | Released0 | up | Released0; false |
| Released0 | Released0 | down | Pressed1; true, including a key already held physically during recorded input |
| Pressed1 | Released0 | down | Pressed1; false |
| Pressed1 | Released0 | up | Released0; false; release handling is separate |
| Released0 | Pressed1 | up/down | Pressed1; true |
| Pressed1 | Pressed1 | up/down | Pressed1; false |

Thus releasing a physical key during recorded playback does not necessarily release the logical recorded button at that time. The next properly seeded live pass can replace it with Released0. Conversely, physical key continuity alone does not decide the next ordinary edge; previous recorded logical state matters. Neither conclusion says that a natural base-Play continuation is available after every departure control.

**Physical mouse:** M1191 wires MouseDown→M11b3 and MouseUp→M11b2; left/right dispatch setters M1188/M118a. A Pressed1 setter writes both latch and raw; a Released0 setter writes raw only. M1187/M1189 read the latch. Focus Deactivate→**M11b0** calls all five original setters with0, so their raw states change but latches are not synchronously released by those setters. At **M11b4**, any unequal latch/raw pair causes **all five** raw states to be copied into latches; it returns true and can cause the bounded host repeat. This happens after callbacks and previous-state writes. A held mouse latch can consequently outlive raw release until reconciliation; no automatic selector-based mouse-up is established.

There is a separate clearing helper: **M11a4:0013→M11b6**, gated by F1374 and setting F03ad. It explicitly clears raw and latch for the first three buttons, not all five logical/input-history fields. Provider polling **M1192** can also feed nullable button samples into the same setters. The chosen physical T03fc implementation, focus/device events and their timing remain runtime inputs. Do not infer a new device configuration or a fixed click lifetime.

**Recorded button publication:** an advanced ordinary frame assigns per-channel held/new fields from F130b–130e, stores aggregate F2a67/F2a83 at03b1/03c8 and mask F2a66 at0541. Nonadvanced ordinary processing can retain prior logical state; source absence precedes flag clearing. Independently, later Play **M229b**, under its original selector/streaming/clock-state/failure/pause/T00ff/handoff gates, constructs a current mask through M11af and stores F2a66:01a9. It is not a selector reset. Do not equate that mask with all physical key histories or silently assume it remains unchanged across every Play callback.

### E. From retained history to the first accepted circle press

The first shared boundary at which retained previous logical state can affect a new ordinary hit opportunity is **new-edge calculation M43a0:02df/0316**. Reached common press bookkeeping then sets **F0a79=true** and, only if F2c6b!=0, queues M11a9 separately for each new side. No key identity, cursor, original event timestamp, Play instance or source reference is captured by those parameterless delegates. The queue exists globally; M4391 does not cancel it.

**M11a9** retains its original resource/pointer-effect prefix; normal paths reach numeric **event7 at00e0→M43b5:00e1**. A prefix exception can prevent dispatch. M43b5 follows its priority collection; M2d48 checks target.F1a9f, looks up event7, invokes registered delegates last-to-first via DynamicInvoke and stops on true. A disabled/missing target, higher consumer, unregistration or exception can prevent M2295. A queued callback from before departure is therefore neither erased by the setter nor guaranteed to hit the old or new Play.

At **M2295**, phase0 rejects immediately. Failed F136a, resume-handoff F13be and recovery F1376 reject; pause F1373 rejects when a fresh selector read is false. The ordinary path requires shared F0a79 unless the original Relax/Taiko exceptions apply, retains the Relax/null-sender and F1396 gates, then reads selection again at0063. False obtains **M0ff1(F2a5c):0074**; true uses **F2a7b:007b**. This is selection of stored input coordinates, not proof a physical sample was refreshed that pass.

The existing ordinary circle path is preserved: **M28c8→M28c9→virtual M24c7** uses existing candidate ordering, separate time/evaluated checks and ordered squared-distance≤radius²; then ordinary **M0ad9** uses its fresh strict admission-time and predecessor/state checks. A selected admitted ordinary T094c reaches **M28d3→virtual M24a9→M1884**. M28d3 rechecks evaluated state; M1884 stores F1527=true before its fresh timing-error comparisons. The manager/ruleset handoff remains separate from the queued event. No time or key captured by M11a9 substitutes for those later F2358 reads.

If both logical sides gain new edges, two delegates may be queued. The first can change object or Play state before the second. The same evaluated ordinary circle is guarded against another ordinary evaluation; another candidate or a rejected event remains possible. No duplicate-hit, dropped-hit or one-result-per-frame assertion follows. A callback exception after dequeue removes that delegate before the later history tail; no rollback is added. A component callback exception can similarly prevent previous-key/aggregate updates and mouse reconciliation. The narrow keyboard catch does not surround those later callbacks.

### F. Bounded conditional cases

Every prediction assumes the specifically reached original helpers return normally unless stated otherwise. **C** anchors CONFIRMED instructions described above; predictions are **RECONSTRUCTED**, not Windows observations. An artificial selector/state probe is not a natural departure. **HYPOTHESIS** that any case occurs with a particular real configuration remains unverified until import, receiver, operands and gates are traced.

| Case | C: original boundary | Conditional prediction | Runtime |
|---|---|---|---|
| C01 selectorfalse, null source | M4391/M43a0 prefix | Ordinary !F137c selects live flag; source presence is not required for that choice | NOT RUN |
| C02 false, old index/advance/mask | M4391 three stores | Those fields are not reset by setter; subsequent consumers may overwrite them | NOT RUN |
| C03 selectortrue, null source | M43a1:001b | Return before advance clear; no frame construction or automatic selectorfalse | NOT RUN |
| C04 source exhausted | M43a1:0064/fallback | End marker can be set; no M4391 call from this method | NOT RUN |
| C05 source reference replaced | F2a7f direct stores; M4391(true) | Replacement alone need not reset selected frame/index/keyboard histories; actual caller matters | NOT RUN |
| C06 false before path calculation | M43a0:0007–0040 | Under ordinary !F137c, next path is live and can seed mouse/keys | NOT RUN |
| C07 false after path calculation | M11a4→M379a→M225b/M1dfb | A reached synchronous control can clear source while the retained path flag remains false | NOT RUN |
| C08 null source in C07 | M43a1 exit; M43a0:00f2 | For !F137c no fallback seed; physical key combination may OR onto retained state | NOT RUN |
| C09 change through generic keyboard control | M43a2:03d5 | Earlier frame/time/button publication and M43a7 skip can already have occurred | NOT RUN |
| C10 accepted departure | M46ad:022a/02e8 | Pending mode and phase0 differ from commit; phase0 blocks new enqueue and M2295 | NOT RUN |
| C11 deferred departure | M46ad:0067 | Setter may already have run; eligibility still depends on all later Play/host state | NOT RUN |
| C12 physical key held, old logical1 | M43a6/7→M43a0 edge | Properly seeded L1/H1 gives no new ordinary edge | NOT RUN |
| C13 physical key held, old logical0 | Same | Properly seeded L1/H0 gives an edge despite no new physical key-down | NOT RUN |
| C14 physical key released during recorded input | M43a7 skipped; tail updates keyboard | Physical previous snapshot can change while recorded logical held remains1 | NOT RUN |
| C15 recordedheld1, physicalup, latch0 | Live seed/combination | L0/H1, no new press; release path is distinct | NOT RUN |
| C16 new key press after live reselection | Live seed/combination | If H0 and L1 and remaining guards permit, queue shared press | NOT RUN |
| C17 same-side mouse already held | Aggregate false policy | New bound key can coexist without a second aggregate edge | NOT RUN |
| C18 both sides H0→L1 | Two enqueue sites | Two shared delegates, no captured side/time; each dispatch rechecks consumers | NOT RUN |
| C19 both sides already held | Two comparisons | Neither becomes a new aggregate edge merely by selectorfalse | NOT RUN |
| C20 true policy alternative | F2a71 branch | Per-channel new flags and previous physical/supplementary state matter; not exact T0253 policy | NOT RUN |
| C21 auxiliary contribution | F03cb→M12f6 | Original extra channel input can invalidate physical-only table assumptions | NOT RUN |
| C22 physical release before reconciliation | M1188/118a | Raw0 can coexist with latched1 until original M11b4 or clearing helper | NOT RUN |
| C23 reconciliation detects inequality | M11b4; M43a0:0820 | All five latch copies occur, and bounded repeat may reobserve input using retained path flag | NOT RUN |
| C24 focus deactivate | M11b0 | Five raw releases; no direct previous aggregate/keyboard reset by this handler | NOT RUN |
| C25 F1374/F03ad clearing path | M11a4→M11b6 | First three raw/latch pairs reset, other logical histories remain separate | NOT RUN |
| C26 queue already contains M11a9 | Setter / queue drain | Setter does not clear it; fresh flags, target registration and Play gates decide its consequence | NOT RUN |
| C27 polling disabled/unfocused live | M43a0:0738→0773 | Current aggregates cleared but old queued callbacks still drain; skipped new-edge/bookkeeping writes are not synthesized | NOT RUN |
| C28 prefix resource helper throws | M11a9 before00e1 | This invocation does not reach event7; no manufactured hit | NOT RUN |
| C29 higher priority handles event7 | M43b5/M2d48 | Ordinary M2295 may never be called | NOT RUN |
| C30 paused return-to-live | M2295:0010–002c | Fresh selectorfalse makes pause rejection applicable | NOT RUN |
| C31 resume-handoff or recovery | M2295 guards; E28 M226b | Press/object-update attention can be suppressed even though host history processing ran | NOT RUN |
| C32 T00ff suppliedfalse | M071b→M071a | Only its original special/readiness/time/change guards permit same-instance disable; no universal base toggle | NOT RUN |
| C33 T00ff suppliedtrue | M071a | Existing source assignment/index resynchronization and past-object writes are separate from false arm | NOT RUN |
| C34 T0466 changes to false | M21b5:00f3/0326 | Same owner remains; reseeds mouse during component callback before history tail | NOT RUN |
| C35 late mouse reseed to1 | M21b5 then M43a0:079a | Post-callback previous becomes1 without retroactively recalculating this pass's earlier edge | NOT RUN |
| C36 late mouse reseed to0, physical key held | Same; next live M43a7 | Tail can store H0; a subsequent seeded admitted pass can detect L1/H0 | NOT RUN |
| C37 T0466 changes to true | M21b5:0119 onward | Existing record/index preparation applies; no fresh Play constructor from setter | NOT RUN |
| C38 T0466 disposal | M23f9→M21a8 | Selectorfalse and auxiliary flag true precede base cleanup; not a press-history reset | NOT RUN |
| C39 base initialization guard skips setter | M224a→M1dff | Original helper/association cleanup still runs; selection need not be cleared by M1dfb(false) | NOT RUN |
| C40 T0a10 initialization | M4abc→M43a4 | Ten current logical fields/index reset; previous keyboard/logical fields not directly reset there | NOT RUN |
| C41 mode commit with registered removal callback | M46b0→F2c55→M43c3 | Input subscriptions can be removed via F2aa4; gameplay queue F2a59 is different | NOT RUN |
| C42 new Play committed | M46b0 constructor/initialization | Global histories are not automatically instance-local; reached init/poll writers determine values | NOT RUN |
| C43 keyboard poll throws | M43a0 narrow catch | Retained current snapshot may later be combined/copied; freshness is not guaranteed | NOT RUN |
| C44 queued delegate throws | Dequeue→Invoke077d | Dequeued callback is not rolled back; component/history/reconciliation suffix may not run | NOT RUN |
| C45 component callback throws | Invoke0790 | Earlier dispatch can have occurred, while subsequent history stores/reconciliation are skipped | NOT RUN |
| C46 repeated inner pass | Backedge00ca | Normal clock/frame work repeats without prefix path recomputation; no physical-frame frequency inferred | NOT RUN |
| C47 old queue, F0a79 nowfalse | M2295 shared new-press guard | Ordinary path can reject although delegate existed | NOT RUN |
| C48 admitted press outside circle area/time | M24c7/M0ad9 | Event7 is not a successful judgement; selection/admission independently reject | NOT RUN |
| C49 admitted press on eligible circle | M28d3→M1884 | Evaluated store precedes fresh error/classification; later manager handoff remains distinct | NOT RUN |
| C50 changed selector on an unproven external thread | No natural thread binding established | UNRESOLVED; separate loads alone cannot prove a race or its outcome | NOT RUN |

### G. Confidence boundary and smallest E20 observation addition

CONFIRMED instructions distinguish global selector/source clearing, source exhaustion, same-instance special reselection, new-instance initialization, physical seeding, recorded publication, edge detection, subscription cleanup and judgement admission. RECONSTRUCTED consequences depend on all stated preconditions. There is no original instruction evidence for “selectorfalse normalizes everything,” “every held key becomes a hit,” “source exhaustion automatically returns to base Play,” or “a queued callback necessarily hits.” These are rejected generalizations, not experimental findings.

UNRESOLVED: naturally selected provider/Play/helper configurations; actual binding values; original auxiliary-input state; Windows focus/event thread and capture cadence; native keyboard/provider success; callback interruption frequency; real requested/accepted/committed departure ordering; first naturally admitted post-departure judgement; visible/audio effects and instrumentation overhead. Exact T0466 helper predicate provenance beyond its reached Boolean remains a bounded configuration boundary. No full input-device, replay, transport or lifecycle survey is justified to turn that into an assumed ordinary value.

Do not modify or recreate the E20 packet. Add only this future observation checklist to its existing README/INSTRUMENTATION/test-matrix/probes workflow:

1. Verify original module, fixture/audio/settings/environment hashes and genuine import; record current/pending mode, phase, Play runtime type/identity, ordinary handler/manager identity and the actual natural departure caller. Forced M4391 writes are separate probes.
2. At original selector writes and prefix consumption, record supplied argument and fresh F2a7c/F2a6f reads, source/frame identities, list/index/advance/streaming state, physical provider identity, F03cb/auxiliary state and exact callback sequence. Do not invoke getters/input/clock methods merely to obtain additional log samples.
3. Capture consumed physical key bits/supplementary state, current/previous snapshots, logical/current/previous/new-side fields, policy, raw/latch/previous mouse pairs, common F0a79, mask F2a66 and the normally reached history stores. Include a held-key control and a genuinely released/new-key control across a naturally permitted provider change.
4. Log event7 enqueue/dequeue/priority target/invocation and original M2295 phase/pause/handoff/recovery gates. If admitted, capture the actual stored/mapped cursor, fresh clock reads, candidate/admission operands and first ordinary-circle evaluated/result mutation. A queue trace alone is insufficient.
5. Capture accepted mode-request writes separately from later commitment/disposal/subscription removal/new constructor. Use thread IDs, monotonic host timestamps, sequence numbers and original before/after operands; preserve exceptions and tracer overhead. Compare repeated low-overhead and uninstrumented natural controls. A debugger-forced history or selector probe cannot be relabeled natural gameplay.

The smallest missing observation is one naturally reached selector departure/reselection with its last recorded history, first live path/physical sample, new-edge decision and first accepted-or-rejected event7 chain. It would discriminate the retained-state predictions directly. Further broad static excavation is not justified for runtime selection or timing; if a concrete Windows trace identifies a specific opaque auxiliary predicate as the blocker, investigate only that caller.

# 3. Pseudocode

RECONSTRUCTED, independent **non-executable** observation of cooperating boundaries. It is not a replacement input manager. Names in prose are explanatory; separate original reads, callbacks, exceptions and other helpers remain explicit.

```text
original selector setter(argument):
    write F2a7c from argument
    if a fresh F2a7c read is false:
        write source reference null
        write selected-frame reference null
    return without an added input-history or event-queue reset

original caller decides what else happens:
    base initialization may disable selection, then retains M1dff/M1dfb(false)
    recorded departure may disable selection and request another mode
    special T00ff or T0466 may reselect within its existing owner
    actual mode commitment separately disposes/constructs and invokes callbacks
    retain every original caller guard and helper; no caller is replaced by setter

original outer host input invocation(component callback):
    read old live-path flag
    calculate and store F2a6f using selector and original special/pause gates
    select/update physical provider
    invoke synchronous original pointer controls
    retain original prefix state work

    at each admitted bounded inner pass:
        original normal clock publication
        assign original component-eligibility seed
        read retained F2a6f
        if false:
            invoke original recorded-frame processor under its lock
            // null source can return before clearing advancement
            // an advanced frame can overwrite time, buttons, mask and pointer
            retain separately guarded special live-button seeding
        otherwise:
            retain guarded mouse seeding and live pointer/mapping work

        if original enable/focus-or-recorded gates permit:
            attempt physical keyboard snapshot under original narrow catch
            retain auxiliary and supplementary input processing
            if a fresh recorded-selector read is false:
                combine original configured gameplay keys with current channels
            dispatch original generic controls synchronously
            // a control can change selection here; do not recompute prefix by fiat

            calculate each new-side flag from current logical button
                and previous aggregate, or original alternative channel policy
            retain common new-press bookkeeping and original presentation helpers
            when phase permits, enqueue parameterless shared callback per new side
            retain distinct release and other event paths
        otherwise:
            retain original aggregate/modifier clearing arm
            // do not synthesize missing edge or common-latch writes

        drain actual gameplay delegate queue in original order
        invoke actual component callback
        // callbacks can change fields; exceptions can prevent the remaining tail
        copy then-current aggregates and keyboard into previous histories
        record original mouse snapshots and conditional time history
        reconcile raw mouse states into latches when any pair differs
        retain bounded repeat test; its backedge begins at clock work

shared press callback:
    retain original resource/pointer prefix
    dispatch numeric event7 through enabled priority targets
    original handlers may consume, reject, unregister or throw

ordinary Play handler, only if actually reached:
    retain phase, failure, fresh selector-dependent pause, handoff and recovery gates
    retain shared-new-press and ordinary mode/input guards
    choose mapped stored live cursor or recorded playfield cursor by a fresh selector
    search existing candidates with original time/state/spatial checks
    retain action admission and fresh gameplay-time reads
    if an ordinary circle is selected and admitted:
        original manager rechecks evaluated state
        original circle evaluator marks evaluated, then computes/classifies error
        retain original manager/result-event handoff
```

Work 1 remains research-only. Work 2 remains suspended. E20's missing Windows execution is still the decisive verification dependency; this static expedition does not complete it.
