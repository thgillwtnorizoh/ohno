# 1. Result

For an osu! player, the recovered difference is this: with the optional reference absent, the whole slider's grade depends on the successful-component fraction; with it present, the higher grades also require corresponding head quality. The original code ties creation of this reference to metadata-named **ScoreV2**, including a count-based automatic enable during loading. This is not proof of which policy the supplied executable actually uses in a particular Windows play session.

Expedition 23 recovers the reference's identity, all direct EXE managed assignments, ordinary new-record configuration, reuse, snapshot restoration, the presence-only aggregate branch and one bounded result-accumulator consumer. **CONFIRMED** here means original static instructions or metadata. **RECONSTRUCTED** identifies their interpretation. No original Windows execution, displayed judgement, active setting or runtime object identity was observed. **E20 remains preparation-only; all 17 runtime cases are NOT RUN — WINDOWS HOST REQUIRED.** Work 2 remains suspended.

## Objective, scope and changes

Question: where does `F13c3.F198e` come from, and under which original conditions does its presence select head-sensitive slider aggregation? Competing explanations were a Boolean setting, a default-constructed field, an optional result-policy object, indirect population, and restore-specific state. Metadata, exhaustive direct access scanning, factory callsites and bounded consumers distinguish these possibilities without reconstructing scoring or replay systems.

Based directly on remote E22 commit `88956dc32788bec316dfce06840e0640d1339655`, on branch `work1/expedition-23-aggregate-policy`. Changes are this report, `re/tools/aggregate_policy_evidence.py`, `re/tools/check_aggregate_policy.py`, `re/evidence/23/aggregate-policy.json`, and an append-only E23 journal entry. No historical report/tool/evidence/fixture or E20 packet is edited. `reconstructed/` is neither inspected nor modified. No exception to the contract is requested or used.

The evidence is a structural extraction of original metadata, selected instruction sites, branches, signatures, hashes and access inventories. It contains no executable, reference assets, private dump, copied original source or replacement scoring implementation.

## Inputs, coverage and reproduction

| Input | SHA-256 / status |
|---|---|
| Original `osubackup.zip` | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| ZIP member `osu!.exe` | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Standing `re/Instruction.md` | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |
| Unchanged E21 two-traversal fixture | `8c5573d0d4fbc84e2898170273c31747b3f8eca232290ee01af65954633e2154` |
| Original execution | NOT RUN — WINDOWS HOST REQUIRED |

Actual bytes were rehashed, ZIP's 18 entries passed CRC checking. The executable self-declares Stable40/file version 1.3.3.8. Its precise official dated release identity and modification status remain **UNRESOLVED**. Analysis used Linux, Python, dnfile 0.18.0, dncil 1.0.2 and pefile 2024.8.26. These libraries decode data; the original managed game was not launched.

Read the complete standing contract, cumulative contexts and earlier reports/evidence, particularly E17 aggregation, E19 record/configuration distinctions, E20 blocked validation, and E22's optional consumer. E05's ordinary osu!standard/Taiko correction, E12's update ordering and E14's resource ordering remain authoritative.

Reproduce using a private local archive; from repository root:

```sh
python re/tools/aggregate_policy_evidence.py /private/osubackup.zip > /private/policy-a.json
python re/tools/aggregate_policy_evidence.py /private/osubackup.zip > /private/policy-b.json
cmp /private/policy-a.json /private/policy-b.json
cmp /private/policy-a.json re/evidence/23/aggregate-policy.json
python re/tools/check_aggregate_policy.py re/evidence/23/aggregate-policy.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

The full EXE scan decodes **19,659 nonzero-RVA method bodies**, identifies **1,739 zero-RVA entries**, and reports **zero decode errors**. Zero-RVA/abstract declarations are metadata, not missing executable instructions. The focused extraction selects 47 methods, including the abstract score-getter declaration. Tokens are original module tokens; `Mxxxx`/`Fxxxx`/`Txxxx` abbreviate MethodDef `0x0600xxxx`, FieldDef `0x0400xxxx`, TypeDef `0x0200xxxx`. CIL offsets below are hexadecimal **relative to code start**, not file, method-header or native/JIT addresses.

The supplementary audit examines the other 15 EXE/DLL members for MemberRefs matching this exact declaring-type name, namespace and field name. None is found in parsed metadata. dnfile emits `invalid compressed int: leading byte: 0xec` for **Microsoft.Ink.dll**, captured explicitly in evidence; the EXE scan does not have that warning. This supplementary result is not a guarantee about malformed metadata, reflection, generated/native code, an unprovided assembly or externally forced memory mutation. The exhaustive writer claim is bounded to direct managed EXE accesses decoded by this pinned extraction. No indirect population mechanism is needed to explain either factory-created or restored non-null states.

## Actual validation

- Two fresh extractions were byte-identical and matched the published evidence JSON (`cmp` PASS).
- Focused checker PASS:47 selected original methods, exact four stores, factory/source/restore/consumer gates,44 prior body-hash comparisons.
- All20 previous research checkers passed on their historical evidence (E02–E19,E21,E22).
- Unchanged E20 `check_package.py --archive` passed:34 historical method entries,17 NOT RUN cases, input/catalog/schema/hash/overclaim/path/overwrite safeguards.
- `python -m compileall -q re/tools re/validation/20` passed.
- All108 baseline blobs were identical before this expedition; after the append, the other107 files and exact old journal prefix are preserved. Whitespace/scope/report-heading checks passed. Remote publication verification is reported with its commit in the final handoff.

The static checks validate extraction/provenance consistency, not original gameplay behavior. No captures, original-runtime logs, imported map values, configured ScoreV2 state or selected aggregate branch exist from this expedition.

# 2. What they found

## Player consequence and exact original identity

**CONFIRMED:** `F198e` is a mutable instance reference field, not a Boolean modifier. `M1465` reads its presence only. **RECONSTRUCTED:** it is a per-record, ScoreV2-associated result accumulator whose presence also selects the head-sensitive slider-grading branch. This alias is supported independently by the named modifier's factory gate, result-record collection, reset behavior and score getter. It is not an original readable field or type name.

| Metadata | Original value |
|---|---|
| Field | `0x0400198e`, name `#=z05t2c3Y=` |
| Declaring record | `0x0200059e`, name `#=zWQjPCUezkZ$WXRmRpbq0cfjnab5SCorhHC9a1HY=`; empty namespace |
| Signature | `06129f90`: FIELD, CLASS reference to `0x020007e4` |
| Access/attributes | Assembly access, instance, mutable; not static, literal, init-only, NotSerialized, default-valued or RVA-backed; no field custom attributes |
| Reference base | `0x020007e4`, name `#=zlCqy0EzB5yPtlqgs3kyVlHMxDqzJ3T4uouoJxl23ePz2`; empty namespace; internal abstract class |
| Declaring record flags | Internal, nonabstract, nonsealed, BeforeFieldInit; no Serializable flag |
| Matching property | No Property metadata return signature containing this exact reference type was found; access is direct at the scoped consumers |
| Global record | Play `F13c3`, static reference, signature `06129678` → T059e |
| Handler record | T0186 `F05f3`, instance reference, same signature → T059e |

Original metadata supplies no descriptive name for F198e or its types. It **does** supply enum `0x02000b57`, namespace/name **`osu_common.Mods`**, and literal **`ScoreV2`**, field **`0x04003ced`**, value **536870912 (`0x20000000`)**. Naming that bit is therefore direct metadata evidence; assuming the UI's active setting would not be.

The reference family's complete EXE ancestry inventory is abstract T07e4 plus four sealed concrete types. The direct managed factory selects:

| Record.F1971 | Constructed accumulator | Constructor token | `newobj` / `stfld F198e` in M2e1f |
|---|---|---|---|
| Osu 0 | T03e5 | M1d5f / `06001d5f` | `002d` / `0032` |
| Taiko 1 | T0718 | M35c7 / `060035c7` | `0047` / `004c` |
| Catch 2 | T012f | M088d / `0600088d` | `0020` / `0025` |
| Mania 3 | T07e3 | M3c4f / `06003c4f` | `003a` / `003f` |

Only ordinary Osu behavior is interpreted here. Other constructors are identified solely to close the factory/virtual-type boundary. No assumption that their gameplay or grading is the same follows.

## Complete direct writer inventory and ownership

**CONFIRMED:** all **four** direct managed F198e stores occur in **one** method, private static **T05f8.M2e1f / `0x06002e1f`**, signature **`000101129678`** (Void, one T059e argument). Each store above follows a fresh `newobj`, passing the same record as constructor argument. Its switch at `0008` has targets `[002b,0045,001e,0038]` for record modes 0–3. The unsupported/default route returns at `001d` without assigning the reference.

There is no prior null, identity, modifier or existing-reference check inside M2e1f. No store assigns null or copies an existing accumulator. Calling it again with a supported mode replaces the old reference; calling it with an unsupported mode preserves it. Its **complete direct EXE caller inventory** is M2e1c:`0051` and M2e1d:`0045`, both conditional, described below. No direct `ldflda`, `ldsflda` or `ldtoken` access to F198e was found. The EXE also has no same-owner/name MemberRef alias for this field in the parsed metadata. There is no direct field-clearing writer hidden in the record constructors or reset routines.

Fresh managed records initially have null F198e because no constructor writes it; that is only the pre-initialization state, not proof of normal play's policy. Base constructor M3c54 creates the result list F23fb at `0006`, calls Object's constructor, then stores the owning record into F23f5 at `0013`. Ordinary constructor M1d5f calls it at `0002`. Thus the factory establishes **record → accumulator → same record**, plus the accumulator's own list. It is not a shared static singleton. Global and handler can share the same record/accumulator by aliasing that record; snapshot restoration instead constructs a fresh pair.

## Ordinary new-record configuration: ScoreV2 predicate before mod assignment

**CONFIRMED static call relationship:** ordinary loading M223b calls `M2258(map,true)` at `00ff`, then later `M2258(map,false)` at `02b2`. The earlier argument true is at `00fe`; the later false at `02b1`. Preserve intervening loading helpers, gates and exceptions; this is not proof both calls complete in a real session.

M2258 (`06002258`, signature `200201128eb402`) at `0056 brfalse.s →006f` skips creation when its second argument is false. With true it passes current ruleset F1371, the original chosen name, map and `this.virtual M229d()` (`0060`) into M2e1c (`0065`), then assigns global F13c3 at `006a`.

M2e1c (`06002e1c`, signature `000412967811ad600e128eb402`) constructs the mode-specific record first. For ordinary Osu its default branch uses predicate M2234 to choose base T059e/M2b3e or T0934/M4550, which forwards to M2b3e. Neither writes F198e. The fourth Boolean argument is tested at `004d–004e`; false jumps to `0056`, true invokes M2e1f at `0051`. This is the conditional constructor/initializer boundary, not a field initializer.

After factory creation, M2258 performs other original record setup and assigns record.F1970 (modifier representation via original conversion) at `009f`. **The optional accumulator has already been selected before this assignment.** The M2b5a calls on this route reset F1990, not F198e. M2258 then calls handler M0ace at `00e4` with `!createNew`. M0ace assigns `handler.F05f3=global.F13c3` at `0006`, then invokes record's virtual M2b62 at `0012`.

Base ordinary M2b62 checks F198e at `0001/0006`, reads it again at `0009` and invokes virtual M3c56 at `000f` if non-null; the other branch resets legacy record state. M3c56 clears/resets accumulator contents under the original Boolean, optionally retaining its own prior values. **It does not clear the record's reference.** A zero/empty accumulator still selects the non-null policy.

With `createNew=false`, M2258 does **not** resample M229d or invoke the factory; it can overwrite record modifier state and reset contents while retaining F198e's existing null/non-null state. Static evidence therefore does not support the invariant “current record mod bits and current F198e presence always agree.” It also does not establish whether a particular UI action changes bits during active Play.

### Which bits does ordinary M229d consult?

Base Play M229d (`0600229d`, signature `200002`) is a virtual Boolean getter. It calls M4390 at `0000` (reads original recorded-input state F2a7c). If that predicate is true **and** source record F2a7f is non-null, it reads that record's F1970 at `0013`, applies original conversion `0a0002ca`, masks ScoreV2 at `001d–0026`, and tests signed `>0` at `0028`. Otherwise it masks global modifier bits F0cc6 (`0031`) and tests signed `>0` at `003d`. A current global bit does not override the reached recorded-source branch.

An initialization callsite in M2237 creates source record F2a7f through M2e1c at `03bd` with enable=false at `03bc`, assigns F2a7f at `03c2`, and subsequently writes its modifier state. That is a **different record**, not global F13c3. Its null accumulator does not force the later ordinary M229d result false, because M229d consults its **bits**, not that reference.

Virtual resolution preserves variants: ordinary/base T047b uses M229d; T04af overrides with M23f1 (optional F14a8, then F378c==3); T0a10 overrides with M4aba (source-record ScoreV2 bit **or** source F198e presence). These are different original predicates. Do not transfer them to ordinary Play. Existing parsed-string factory M2e1e does not call M2e1f; not every record construction route installs the reference.

### Loading can enable ScoreV2 automatically

M223b:`00ad` obtains the current ruleset via M2232. It enters the count branch for mode0 or2. At `00ba` M2c63 obtains the selected map; `00bf` reads F0ff2, `00c4` loads **7000**, and `00c9 blt.s →00f7` skips automatic enabling for counts below7000. If the bit is not already present, `00ec` loads536870912, `00f1 or` combines it with F0cc6 and **`00f2 stsfld F0cc6`** writes it before M2258(true).

F0ff2 is **RECONSTRUCTED as a map metadata object-row count**, not a format-version field or the live manager object count. Its complete direct writers are:

| Writer | Original value origin / limit |
|---|---|
| M1bc1:`0030` | Reset to0 in the reached map metadata parser |
| M1bc1:`0f81` | Unchecked +1 after a reached nonempty trimmed HitObjects row; section enum T0b24 has original `HitObjects=64` (`0284`, branch `033c→0e83`); row-type counters distinguish masked type1/2/8/128 before common increment |
| M1bf2:`0369` | Cached metadata route: unchecked sum of F0fd9 (`0356`), F0fdb (`035c`) and F0fda (`0363`), with adds at `0361/0368` |

No claim that every metadata row produces a valid live object follows; cached and parsed routes need not be one immutable snapshot. For the conditional one-slider E21 fixture, a valid one-object metadata count would not trigger7000. **Conditional ordinary reachable states are now established:** fresh live ordinary record with bit absent and count below7000 can retain null F198e; bit present, or the reached automatic enable followed by base live M229d, requests non-null ordinary T03e5. These are static reachability statements. Actual import, selected map metadata, modifiers, recorded-input source, virtual Play type and final runtime reference are still **UNRESOLVED**.

## Snapshot provenance and restoration can change the policy

The smallest restore connection is explicit and does not require surveying all persistence:

| Method / token | Relevant original sites | Meaning established |
|---|---|---|
| M229e / `0600229e` | F13c3 `0117`, F198e `011c`, `ldnull; cgt.un` at `0121/0122`, F2045 store `0124` | Snapshot T0740.F2045 Boolean records **presence**, not record mod bits or an inner score value |
| M229e | Separate F198e reads `0130/0155`, payload stores F2046/F2047 `0149/016e` | Copies accumulator F23f9/F23fa, null yielding Double0; no atomic snapshot guarantee |
| M3756 / `06003756` | MemberRef `0a000695 ReadBoolean` `00aa`, F2045 store `00af` | Explicit read of saved presence flag; conditional Double payloads follow |
| M3758 / `06003758` | F2045 read `00bb`, MemberRef `0a000697 Write` `00c0` | Explicit write of that presence flag; conditional Double payloads follow |
| M2e1d / `06002e1d` | New ordinary record M2b40 at `0036`, F2045 read `003d`, `0042 brfalse.s→004a`, M2e1f call `0045` | Restore installs a **fresh** accumulator iff saved flag is true, not from a fresh modifier-bit test |
| M226e / `0600226e` | M2e1d `0010`; global new record store `003e`; handler store `0043` | Replaces global then handler record in distinct writes; preserves some other old fields, not old F198e identity |
| M229f / `0600229f` | Current F198e `00a5`, then F23f9/F23fa stores `00de/00f3` | Separate content-restore route changes existing accumulator values, not its presence |

A true flag can produce a new non-null accumulator even if current live modifier bits differ; a false flag leaves the new record's reference null. The producer naturally obtains the flag from prior presence. Arbitrarily changing it is an artificial probe, not natural evidence. The read/write methods establish serialized flag handling, not the full source, version compatibility or reachability of every external snapshot. Stop at that boundary.

Global and handler assignments are separate: M0ace aliases them on configuration; M226e writes global first and handler second. This permits a transient logical mismatch between writes, but no scheduler/interleaving/runtime observation is asserted. Handler result processing may also be interrupted before resynchronization. A permanent mismatch is an artificial test unless an actual original route proves it.

### Global record replacement audit

The complete direct EXE F13c3 writer inventory is:

| Method | Owner | Store offset | Scoped origin / limitation |
|---|---|---|---|
| M2258 | T047b | `006a` | Factory-created record, described above |
| M2266 | T047b | `00d1` | `this.virtual M2274()` at `00cc`; base M2274 simply reads current global F13c3, so base path reassigns it to itself; variants not generalized |
| M226e | T047b | `003e` | New restore record |
| M2571 | T04d6 | `00f7` | Outside scoped ordinary configuration; no lifecycle interpretation assigned |
| M32d3 | T06a8 | `00d9` | Outside scoped ordinary configuration |
| M3cf4 | T07f8 | `000e` | Outside scoped ordinary configuration |
| M418a | T08b6 | `0109` | Outside scoped ordinary configuration |
| M4abc | T0a10 | `0047` | Special Play source-record assignment; not ordinary base selection |

This inventory closes direct store coverage, not all lifecycle semantics. No broad claim that the global record cannot be cleared/replaced during Play is made. For F198e itself, ordinary content reset/restore does not replace it, but M2e1f can replace it and global-record replacement can change the presence observed by M1465.

## Bounded aggregate selection and manager conversion

**CONFIRMED:** M1465 marks parent evaluated at `0002`, calls original virtual M24c4 (`0008`), reads head raw at `0013`, increments F0bf1 at `0024` iff that raw is signed positive, then computes `Double(F0bf1) / Double(unchecked(F0bf8.Count+1))` (`002f/003d conv.r8`, `003e div`). Original integer counter increment is unchecked before conversion. It may call original M24bb on the positive-ratio branch at `004d` **before reading the policy**. Do not assume these original helpers are pure, always return or preserve global state.

At `0052`, it reads global F13c3 afresh. `0058 brtrue.s→005e` handles non-null record; null record is popped and replaced by null. At `005e` it directly loads F198e from the record already on the stack. `0063 ldnull;0064 cgt.un;0066 brfalse.s→00c2` selects the null branch. Non-null starts `0068`.

It reads **no inner accumulator field, type discriminator or score**, invokes no method on that object, and writes neither F198e nor its contents. It does not reread global between the null-record test and that one dereference; changing global then would not replace the stack-held record. Across methods and head reads there is no immutable snapshot.

| Policy branch | Ordered tests and raw return |
|---|---|
| Null global **or** null F198e (`00c2`) | q==Double1 →1024; else q>=Double0.5 →512; else q>Double0 →256; else−131072 |
| Non-null F198e (`0068`) | q==Double1 **and fresh head raw==1024** →1024; else q>=Double0.5 **and later head raw>=512 signed** →512; else q>Double0 →256; else−131072 |

Head reads for the two quality tests are `007a` and `009e`; they are separate from the numerator read `0013`. The best-test branch is `0084 bne.un.s→008c`; the second head test is `00a8 blt.s→00b0`. The Double comparisons retain original unordered handling: best uses `bne.un`, middle uses `blt.un`, final positive uses `ble.un`. Artificial NaN ratios do not pass these grade thresholds. Normally constructed finite controls have a positive denominator; no convenient clamping for inconsistent counters/overflow is invented.

The same successful checkpoints and **different positive head qualities** can give q1 in both runs yet produce1024 versus512/256 only in the non-null branch. A failed head contributes no numerator increment; it is not equivalent to a low but positive head. A missed head can still yield a positive whole result through component successes.

M1465 does **not** store the returned aggregate into parent.F152a. Through `M28e4 → M28d3(parent) → virtual M24a9 → M1465`, M28d3 receives a local return. Its **Perfect16384** conversion and later group/decorating/result-event stages are independent of the F198e presence test: M28d3 has no direct F198e access. At M28d3:`0047/004c`, a non-null global record permits the manager.F17ef modifier read (`004f`), original conversion and Perfect16384 test (`0059–006a`). The manager masks its local result with520095494 (`006d–0072`); a512/256 can become manager-local−131072 (`008b/0090`); parent/head raw state is not thereby rewritten. The manager ultimately forwards its then-current local through the established M291a event boundary (`M28d3:0a40`) and ordinary Play M229c→ruleset dispatch. Preserve conversions, conditional decorations, delegates and exceptions; “returned aggregate” and “forwarded result” are not synonyms.

A visible 300/100/50 label alone would not identify the selected policy: the branches overlap, component outcomes and head quality vary, and manager conversion/decorations intervene. No original visible label was observed here.

## Related optional consumer uses handler record and result data

This is a bounded cross-check of base M0ad2 (`06000ad2`), reached for ordinary T0253 through M114a. It is not a reconstruction of complete scoring. Its prefix, F05f6-related work, other F198e reads and result-specific helpers are preserved as original operations, **not** declared side-effect-free. Raw0 returns early. Earlier calls can mutate state or throw before this late block.

At `1306–1312`, original local4 (captured object.F153b at `0016`) or the signed-positive masked raw bit **8388608** permits entry. If local4 is false and `(raw &8388608)<=0`, branch `1312 ble→13bb` skips the block, including the legacy alternative. This gate is **not** local3, the combo-enable Boolean.

At `1318`, it reads **handler.F05f3**, then that record's F198e at `131d` into local8. `1326 brfalse.s→1394` selects the legacy accumulation path if null. This is **not a global F13c3 read**. With a non-null receiver it reads handler.F05f3 **again**, then mode F1971 (`1329/132e`), switching at `1337`. Osu/default constructs T0399 through M1ad1 (`1352`) and calls M3c57 (`1357`) with the captured accumulator receiver and the new result-data object. Other modes use different data constructors; do not apply them to ordinary gameplay.

The result-data constructor (`06001ad1`, owning T0399) stores:

| Constructor operand | Store |
|---|---|
| Original raw argument | F0f5a at `0008` |
| Original local3 combo-enable Boolean | F0f58 at `000f` |
| Original local5 extra Boolean | F0f57 at `0016` |

No parent-slider reference is passed to M3c57; the parent had earlier been supplied to M0ad2, where flags/helpers still matter. A false combo-enable Boolean is data and does not itself suppress optional dispatch. Raw terminal−524288 can reach a data record with that Boolean false under the original other gates, distinct from negative raw−262144.

M3c57 (`06003c57`, signature `200101128e64`) is **nonvirtual** on abstract base T07e4 despite the caller's `callvirt`. First meaningful operations:

1. If F23fb.Count>0 (`0006/000c`), copy last result-data.F0f59 into incoming result-data.F0f59 (`001a/001f`).
2. If incoming F0f58 is true (`0025/002a`), signed-positive raw F0f5a increments incoming F0f59 unchecked (`003e`); otherwise set it0 (`0047`). If the Boolean is false, preserve the copied/default value.
3. Append the same data reference to List<T0399> F23fb (`0053`, MemberRef `0a00163d Add`).
4. Invoke virtual M3c58 at `005a`. Ordinary T03e5 inherits base M3c58 and reset M3c56 unchanged; its score getter overrides M3c55 with M1d60.

Base M3c58 writes Double accumulator fields F23f9/F23fa using the incoming result and its M1ad2 value helper. This is the stopping boundary: no complete score formula, presentation or online persistence is recovered here. The append and contents mutation are direct evidence of accumulator behavior, not a global combo mutation claim. F0f59 is per-result-data state; do not equate it with gameplay current combo T0892.F2674 merely because both react to results.

Record getter M2b4b uses a non-null reference to call virtual M3c55, otherwise returns the legacy field; M2b62 resets its contents; under its original guard, M2b5c ORs the named ScoreV2 bit into the record's mods when the reference exists. These corroborate the interpretation without determining a runtime configuration.

Neither M0ad2 nor M3c57 directly stores F198e. Updating list/score contents alone cannot toggle M1465's presence branch. Mode and receiver reads can refer to different record snapshots if state changes between them; no synthetic single “policy snapshot” is substituted. Exceptional helpers and broader indirect mutation remain **UNRESOLVED**.

## Complete direct reader inventory

There are **30 direct EXE reader methods** and one writer method for F198e. The complete reader table below records all `ldfld` offsets; other readers are inventoried, not expanded into unrelated modes, UI, persistence or scoring. The JSON `entry_field_sites` includes the full watched-field inventory; `policy_access_inventory` adds owning type and original method signature, including F2045 producer/consumer accesses. No claim that every reader is ordinary gameplay follows.

| Method / token | Owner | Original method signature | F198e read offsets |
|---|---|---|---|
| M0837 / `06000837` | T011f | `200301020202` | `001e` |
| M0848 / `06000848` | T011f | `200001` | `0040` |
| M0849 / `06000849` | T011f | `20010212930c` | `0022` |
| M0ad2 / `06000ad2` | T0186 | `20020111922012930c` | `021a`, `04c4`, `0568`, `07dd`, `0877`, `0cbc`, `10ae`, `131d` |
| M1465 / `06001465` | T02af | `2000119220` | `005e` |
| M1d70 / `06001d70` | T03ea | `200011aa08` | `000c` |
| M1d71 / `06001d71` | T03ea | `20010111aa08` | `000c` |
| M1d75 / `06001d75` | T03ea | `2000119220` | `0031`, `0130`, `015e` |
| M1d78 / `06001d78` | T03ea | `200111922011aa08` | `0081`, `00b2`, `00e9` |
| M1ed5 / `06001ed5` | T0421 | `200002` | `000d` |
| M1ee0 / `06001ee0` | T0421 | `20010112930c` | `0026` |
| M1ef7 / `06001ef7` | T0421 | `200001` | `02b8` |
| M1ef8 / `06001ef8` | T0421 | `20000d` | `00d2` |
| M20d7 / `060020d7` | T0456 | `200001` | `005c`, `0124` |
| M20eb / `060020eb` | T0457 | `2000119220` | `006f` |
| M20ed / `060020ed` | T0457 | `200008` | `0012` |
| M229e / `0600229e` | T047b | `0000119d00` | `011c`, `0130`, `0155` |
| M229f / `0600229f` | T047b | `000001` | `00a5`, `00d3`, `00e8` |
| M2810 / `06002810` | T053f | `20000c` | `000f` |
| M2b4b / `06002b4b` | T059e | `200008` | `0001` |
| M2b5c / `06002b5c` | T059e | `200001` | `000b` |
| M2b62 / `06002b62` | T059e | `20010102` | `0001`, `0009` |
| M32d7 / `060032d7` | T06a8 | `200201129678151188010108` | `0083` |
| M4175 / `06004175` | T08ab | `2000119220` | `004a` |
| M4178 / `06004178` | T08ab | `2000119220` | `00d9` |
| M4257 / `06004257` | T08c2 | `20000d` | `0020` |
| M425b / `0600425b` | T08c2 | `2000119220` | `0013` |
| M4264 / `06004264` | T08c2 | `200111922011aa08` | `0021` |
| M4625 / `06004625` | T094e | `20000d` | `0268` |
| M4aba / `06004aba` | T0a10 | `200002` | `0023` |

## Representative body integrity

All selected nonzero-RVA method body hashes are in the evidence; abstract M3c55 has no body. Shared selected methods are checked against all historical evidence JSONs containing a body hash. Representative SHA-256 values (including original header bytes) are:

| Method | SHA-256 |
|---|---|
| M2e1f | `684bbfba8a176d8ae27e21d44ca5ab68eeabea8b015124204473a61953d3b927` |
| M2258 | `a0836e2be42d1ac8b2846124448d91afad2b6938b71a7dd0808badf172c0474f` |
| M229d | `cd1445fcf5b5224b21757606e1d40b1b7836a795487915416a2698812c2138da` |
| M2e1d | `08901ec37d9f75c297f3c9c3f2443f83e97ff4392faac2ff6a157d6e5cf1ba11` |
| M1465 | `8a76dd6cd93705ce792566422c7ab2b9cec04a6eab5719518e0f226701e547ab` |
| M0ad2 | `2276c97a07e987cdb9b72e2dcf256fcf105a074ebf4e03f910feb58adebcaaec` |
| M3c57 | `c956de63df4aa4a177f003f0ffbe03ca489287b9d9f184ecf03e7b39cbf48ab7` |

## Conditional static reference cases

Every case below is a branch-derived **static prediction**, not original gameplay or a synthetic scoring run. Preconditions include normally returning intervening helpers and reached initialization/evaluator/consumer gates. Values use original raw enums; no displayed score or pixels are asserted. “A/B” refer to the null/non-null branches above. For E21 N6, denominator7 means3/7 is below0.5 and4/7 above; exact0.5 is conditional on an appropriate even denominator such as E16 N3, not fabricated for N6.

| ID | Controlled state / differing input | Predicted branch or observation to test |
|---|---|---|
| P01 | Global F13c3 null at M1465:0052 | BranchA after null stack handling; other earlier helpers must already return |
| P02 | Non-null global record, F198e null | BranchA |
| P03 | Non-null global record, F198e valid non-null | BranchB; object contents are not inspected |
| P04 | Fresh base record before conditional M2e1f | F198e managed-default null; not an assertion about final Play setup |
| P05 | Fresh ordinary live configuration, ScoreV2 absent, metadata count1, no special getter/restore | M229d false; M2e1c does not install accumulator; BranchA if record remains current |
| P06 | Same ordinary config, ScoreV2 present | M229d true; M2e1f constructs T03e5; BranchB if still current |
| P07 | Reached ordinary M223b count6999 versus7000, live bit initially absent |6999 skips automatic enable;7000 enables bit before new-record configuration; recorded-source/special getter can still change factory outcome |
| P08 | Recorded-input predicate true, F2a7f non-null with ScoreV2 absent, global bit present | Base M229d uses source record bits, returns false |
| P09 | Recorded-source ScoreV2 present, global absent | Base M229d returns true |
| P10 | M2258(createNew=false), existing accumulator non-null, current bits now absent | No new M229d/factory call; contents reset does not clear reference |
| P11 | Same reuse with null reference, bits now present | No automatic installation in this call; reference remains null absent another writer |
| P12 | Snapshot produced from null/non-null reference | F2045 false/true; separate payload reads can differ under artificial concurrent mutation |
| P13 | Restore M2e1d with saved false | New record default-null, regardless of prior record accumulator |
| P14 | Restore with saved true and modeOsu | Fresh T03e5/back-reference; replaces identity, BranchB if global remains current |
| P15 | M226e after003e but before0043 | Global is new record; handler assignment not yet executed. An interleaved consumer is not proven naturally reachable |
| P16 | Handler/global identity match | M1465 and late M0ad2 can observe same record, but at separate times |
| P17 | Artificial persistent handler/global mismatch | Aggregate uses global; optional result consumer uses handler; do not merge null states |
| P18 | All6 checkpoints successful; head1024 | q7/7; both policies return1024 before manager conversion |
| P19 | All6 successful; head512 | q7/7; A1024, B512 |
| P20 | All6 successful; head256 | q7/7; A1024, B256 |
| P21 | All6 successful; head negative | q6/7; A512, B256; head does not increment numerator |
| P22 | N6 numerator3 versus4, positive head high enough when used | q3/7→256; q4/7→512 in A, and in B only with head>=512; no exact half for N6 |
| P23 | Appropriate even denominator, q exactly0.5 | A512; B512 iff fresh head>=512, otherwise256 |
| P24 | q0 with no positive head contribution | Both return−131072; non-null reference alone grants no credit |
| P25 | 0<q<0.5 | Both return256; no head-quality minimum on that branch |
| P26 | q1 but changing head between numerator and later quality reads | Preserve separate reads; output depends on actual later operands; artificial unless trace shows mutation |
| P27 | BranchB returns512/256; independently reached Perfect16384 manager gate | Manager-local conversion can yield−131072; no change to parent.F152a or reference |
| P28 | BranchA with identical manager state | Same independent conversion logic; policy and conversion are separate |
| P29 | Late M0ad2 gate admitted, handler accumulator non-null, modeOsu | New T0399(raw,local3,local5) appended through M3c57 then virtual hook |
| P30 | Same gate, handler reference null | Skip M3c57; take1394 legacy route |
| P31 | Object flag false and masked raw bit8388608 not positive | Skip entire late optional/legacy block to13bb, even if accumulator exists |
| P32 | Optional dispatch admitted but local3=false | Data append still occurs; incoming inherited F0f59 does not increment/reset in M3c57 |
| P33 | Optional dispatch admitted, local3=true, positive raw versus nonpositive raw | Incoming per-result F0f59 increments unchecked versus resets0; not asserted as gameplay combo |
| P34 | M3c56 or M229f resets/restores inner values to0 | Presence unchanged; next M1465 stillB for same non-null reference |
| P35 | M2e1f called twice with supported ordinary mode | New reference replaces old, both non-null; no singleton/cache identity |
| P36 | Unsupported mode passed artificially to M2e1f | Switch default returns without assignment; existing null/non-null state retained |
| P37 | Valid different concrete family type placed into ordinary record artificially | M1465 presence-only check stillB; ordinary natural factory selects T03e5, not this mismatch |
| P38 | Helper throws before aggregate return or consumer append/hook | Later handoff not established; retain already performed mutations; no synthetic rollback |
| P39 | F198e or global record replaced between separate method calls | Later reads observe actual then-current identity; direct code permits specified factory/restore replacements, concurrent scheduling unverified |
| P40 | Configuration lacks recorded actual bits/map count/Play runtime type/snapshot flag | Active ordinary policy UNRESOLVED; run minimal E20 addition, do not infer from default ctor or visible label |

## Minimal addition to existing E20 Windows checklist

No E20 file or runtime status is changed. Reuse its authorized isolated Windows host, exact EXE/archive hashes, private artifact directory, fixtures and low-overhead CLR token-to-JIT binding. Add only these observations to an existing ordinary slider run:

- At reached M223b automatic-bit branch: actual ruleset, selected map identity, F0ff2 and global F0cc6 before/after; identify parsed versus cached metadata without forcing a count.
- At M2258(createNew=true) and M229d: actual concrete Play type, M4390 outcome, global bits and source F2a7f identity/mod operand, Boolean returned, new global record identity/type, F198e identity/null state before/after the actual M2e1f store, constructor and record back-reference. Do not log by calling stateful timing/configuration helpers again.
- At M0ace completion and before M1465: actual global and handler record identities/types, accumulator identity/type/null state, any observed writer/replacement, configuration/mode. Record initial versus reused setup.
- At M1465: original head reads, counters/N/q, policy null-check result and returned raw. At M28d3: separate Perfect operand, local conversion/decorations and forwarded result. Keep consumer-site values with their thread, sequence and host timestamp, not guessed configuration.
- At late M0ad2: actual gate, handler receiver, original mode read, result-data constructor arguments, whether M3c57 append/hook occurs. If an actual restore happens, record F2045 source and the separate global/handler writes; do not force a restore merely to label it natural.

A minimal useful matched comparison is the existing slider with all checkpoints succeeded but a naturally lower positive head, with ScoreV2 absent versus present through original controls if available. No need to hit an exact boundary. Verify actual bits and reference; do not infer them from the displayed result. Keep optional forced-bit/reference/snapshot probes separately labelled artificial. Record exceptions, thread/order, tracing overhead and untraced controls as in E20. **No such observations were obtained.**

## Remaining dependency and stopping boundary

The original direct factories explain non-null state; snapshot producers explain its saved presence. No unresolved reflection mechanism is needed to complete that bounded provenance. What remains is the **actual ordinary Windows configuration and reference identity at the consumption sites**, including selected Play variant, metadata count, recorded/live bit source, restore path and interruptions. Static callsites cannot establish that runtime snapshot.

The smallest next useful gameplay investigation is the existing E20 ordinary-slider trace with the reference/bit/branch additions above, especially a lower positive head with successful components. Do not begin another scoring/persistence survey to substitute for this missing observation. Full score formula, submission, UI setting reachability, visible judgement feedback and slider performance remain outside this expedition. The distorted-slider stutter report remains a future measurement objective; nothing here establishes its cause or a caching benefit.

# 3. Pseudocode

**RECONSTRUCTED — independent description of original reference flow, not executable gameplay/scoring code.** Original helpers, virtual dispatch, exceptions, separate reference reads and evaluation order remain significant.

```text
ordinary base Play policy predicate M229d:
    if original recordedInputPredicate M4390() and sourceRecord F2a7f != null:
        return (originalConvert(sourceRecord.F1970) & 536870912) > 0  [signed]
    return (globalModBits F0cc6 & 536870912) > 0                      [signed]

reached loading prefix M223b:
    retain prior original operations
    mode = original M2232()
    if mode is Osu0 or Catch2:
        if originalSelectedMap().F0ff2 >= 7000 [signed]:
            if (globalModBits & 536870912) is not positive:
                globalModBits = globalModBits OR 536870912
    original M2258(map, true)
    retain original loading operations
    later original M2258(map, false)

configuration M2258(map, createNew):
    retain original name/source selection and guards
    if createNew:
        enabled = this.virtual M229d()          // special Play may override
        r = original mode-specific record factory M2e1c(mode,name,map,enabled)
        globalRecord F13c3 = r
    retain original reset helpers
    assign current globalRecord.F1970 using original source/conversions
    handler.M0ace(!createNew)
    retain suffix

factory M2e1c(mode,name,map,enabled):
    r = original record constructor selected by mode and M2234
    if enabled: original installAccumulator M2e1f(r)
    return r

installAccumulator M2e1f(r):
    switch r.F1971:
        Osu0:   r.F198e = new original T03e5(r)
        Taiko1: r.F198e = new original T0718(r)
        Catch2: r.F198e = new original T012f(r)
        Mania3: r.F198e = new original T07e3(r)
        default: return                         // no assignment
    // base constructor creates a result list and records owning r

handler M0ace(resetArgument):
    this.F05f3 = current globalRecord F13c3
    this.F05f3.virtual M2b62(resetArgument)
    // original reset may change contents; it does not store F198e

snapshot M229e:
    snapshot.F2045 = (current globalRecord.F198e != null)
    copy original nullable payloads through separate reads
restore M226e(snapshot):
    r = original mode-specific M2e1d(...)
        // constructor first; iff snapshot.F2045, call M2e1f(r)
    retain original selected old-field transfers (not old F198e)
    globalRecord = r
    handler.F05f3 = r                           // distinct later store
    retain original restore suffix
```

**RECONSTRUCTED — bounded M1465 evaluator and independent manager continuation:**

```text
parent.F1527 = true
original virtual M24c4()
if signed fresh head.F152a > 0:
    parent.F0bf1 = unchecked(parent.F0bf1 + 1)
q = Double(parent.F0bf1) / Double(unchecked(parent.F0bf8.Count + 1))
if original ordered q > 0: original virtual M24bb()

r = fresh global F13c3
presence = (r == null ? null : r.F198e) != null
if presence:
    if ordered q == 1.0 and fresh head.F152a == 1024: return 1024
    if ordered q >= 0.5 and later head.F152a >= 512 [signed]: return 512
    if ordered q > 0.0: return 256
    return -131072
else:
    if ordered q == 1.0: return 1024
    if ordered q >= 0.5: return 512
    if ordered q > 0.0: return 256
    return -131072

// Caller M28d3 receives a local; no parent.F152a store here.
// Preserve separate Perfect16384 conversion, decorations and event dispatch.
```

**RECONSTRUCTED — bounded related handler consumer, not a score implementation:**

```text
M0ad2 late block, after all original prefix/result-specific helpers:
    if originalObjectFlag local4 or signed(raw & 8388608) > 0:
        accumulator = this.F05f3.F198e           // handler, not global
        if accumulator != null:
            mode = fresh this.F05f3.F1971
            data = original mode-specific result-data constructor
                   (original raw, local3 comboEnabled, local5 extraBoolean)
            accumulator.M3c57(data)
        else:
            retain original legacy record accumulation branch
    retain suffix

M3c57(data), ordinary inherited implementation:
    if this.F23fb.Count > 0:
        data.F0f59 = original last-record.F0f59
    if data.F0f58:
        if signed data.F0f5a > 0: data.F0f59 = unchecked(data.F0f59 + 1)
        else: data.F0f59 = 0
    this.F23fb.Add(data)
    original virtual M3c58(data)                // bounded content/value hook
    // no direct F198e store; content changes do not change presence
```

**UNRESOLVED:** which reference/configuration is actually active in ordinary Windows gameplay; naturally reached policy/callback/restore sequencing; actual displayed result and instrumentation overhead. E20 has not been executed. These pseudocode blocks do not establish any original-runtime observation.
