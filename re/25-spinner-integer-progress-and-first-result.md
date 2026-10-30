# 1. Result

For an osu! player, gaining spinner progress and receiving a spinner component result are separate events. Stable first observes whether the accumulated progress converts to a different integer. On a fresh ordinary spinner, the **first observed change normally returns0**, and the **second observed change returns raw8192** under the bounded non-bonus conditions recovered here. It does not award one result for every integer skipped between observations. That raw8192 consumer bypasses the common combo-increment/reset block.

These are **RECONSTRUCTED player-facing consequences of CONFIRMED original static instructions**, not gameplay observations. A nonzero enum, a local numeric amount, a target adjustment, a final judgement and a displayed score remain different mechanisms.

## Objective, scope and provenance

Expedition25 continues E24's boundary `M05f6:02d5–04c7`: original conversion/comparison, changed-integer counter, first nonzero component, cache timing and ordinary `M0ac5→M114a→M0ad2` handoff. The only requirement/bonus logic recovered is the prepared operand and comparisons needed to distinguish this component path. Full spinner required-spin formula, bonus payouts, final judgement, total score/accuracy/HP, sounds, rendering and lifecycle remain outside scope. No surrogate spinner/classifier/scoring program was built or run. Work2 and `reconstructed/` were neither inspected nor modified. No authorised exception.

Read the complete standing contract, cumulative journal and relevant E01–E24 evidence. Preserve E05 ordinary/Taiko dispatch, E12's post-refresh rejoin before M28e3, E14 resource ordering and E24's ordinary spinner correction. Dedicated branch `work1/expedition-25-spinner-first-result` starts directly from **`21bdf03d2a7ed32b3c539920ed08903bda1e4fcc`**, tree `1186a309e089a04401adab648fa541780cbbca42`. All117 baseline blobs/modes were verified against remote. Local mirror commit identity differs, tree is identical; publication explicitly uses the requested remote parent. Preserve116 other files and the exact old journal prefix.

**E20 remains preparation-only and incomplete. All17 original-runtime validation cases are NOT RUN — WINDOWS HOST REQUIRED. Every E25 runtime case below is also NOT RUN.** No new E20 packet or runtime data is created.

## Changes, inputs and reproducibility

Added this report, `re/tools/spinner_result_evidence.py`, `re/tools/check_spinner_result.py`, `re/evidence/25/spinner-result.json`, and an append-only journal entry. The E24 spinner fixture is reused unchanged; no new fixture or proprietary asset is needed. Structural evidence contains tokens/sites/metadata/branches/signatures/hashes, not original source, binaries, full disassembly or private captures.

| Pinned input | SHA-256 / status |
|---|---|
| osubackup.zip,18 entries,CRC valid | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| ZIP member osu!.exe | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Standing contract | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |
| Unchanged `re/fixtures/24/single-spinner.osu`,605 bytes | `85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c` |
| Existing E01 silence.wav | `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977`; reuse original generator/verification in E20, no audio asset published |

ZIP and EXE bytes were rehashed. Self-declared Stable40/file-version1.3.3.8; exact official dated identity, authenticity and modification status remain UNRESOLVED. Linux static analysis, pinned dnfile0.18.0/dncil1.0.2/pefile2024.8.26 reinstalled after an initial missing-module error. The local host is Linux x86_64, without wine/dotnet/PowerShell or DISPLAY/WAYLAND; no authorized Windows endpoint is supplied. No original EXE/CLR/conv.i4/BASS/gameplay/import/capture or substitute gameplay execution occurred. Fixture S2000/E5000, ordinary Mode0 and OD5 are intended inputs, not observed imported values.

Competing explanations: floor/Abs conversion versus raw conv.i4; monotonic increase versus inequality; crossed-integer drain versus one change event; first-change award versus counter/parity gate; atomic progress/cache versus fresh later read; result implies combo gain versus explicit Boolean suppression; notification guaranteed at completion versus missed current-object dispatch. Discriminate using original CIL/metadata, whole-EXE direct access inventories, exact ordinary virtual slots, helper boundaries and branch-derived cases. No remembered osu! formula or alternative engine is used as evidence.

Mxxxx/Fxxxx/Txxxx abbreviate MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx. All CIL offsets are **hexadecimal relative to the first opcode**, excluding headers, not native/JIT addresses. Evidence JSON offsets are decimal. CONFIRMED means original static instructions/metadata; semantic aliases and independent pseudocode are RECONSTRUCTED; runtime/JIT/helpers/interleavings/UI remain UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/spinner_result_evidence.py /private/osubackup.zip > /private/result-a.json
python re/tools/spinner_result_evidence.py /private/osubackup.zip > /private/result-b.json
cmp /private/result-a.json /private/result-b.json
cmp /private/result-a.json re/evidence/25/spinner-result.json
python re/tools/check_spinner_result.py re/evidence/25/spinner-result.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

The extractor reuses unchanged E04/E05/E15 collectors:51 selected method entries, full direct watched-field/caller scans, owner/signature/type/virtual metadata, original constants/IEEE bit encodings, branches, arithmetic/conversions and local/argument sites. Full EXE coverage:19659 decoded bodies,1739 zero-RVA entries,0 decode errors. Abstract/native/delegate declarations have metadata but no CIL body to execute. Direct inventories exclude native/external/reflection writes and transitive callback effects.

Actual checks passed:

- Two fresh extractions are byte-identical and equal the evidence JSON.
- Focused checker:51 original method entries, integer/counter/cache/result branches, exact ordinary handoff/combo bypass/order,216 historical body-hash comparisons and unchanged E24 fixture.
- All22 previous research checkers (E02–E19,E21–E24) passed on their historical evidence.
- Unchanged E20 package check passed:34 historical method entries,17 NOT RUN cases, input/catalog/schema/hash/path/overwrite/overclaim safeguards. No PowerShell/Windows/gameplay execution was performed by that check.
- Python compilation of `re/tools` and `re/validation/20` passed.
- Historical-file/journal-prefix, exactly three report headings,44 conditional NOT RUN cases, scope/whitespace/diff checks and remote parent/tree/branch/all-blob verification are completed before final handoff.

These are static evidence/consistency checks, not original-Windows acceptance tests.

# 2. What they found

## A. Exact conversion, comparison and counter mutation

The corrected ordinary object remains **T00d8→T067f→T04c3→T0828**, constructed through `M28f7→virtual M1872→M48e1→M05ea→M3201`. Taiko M4270/M1d6d is not used. Relevant field identities are:

| Token | Owner / signature | Original field name | RECONSTRUCTED role |
|---|---|---|---|
| `040002b4` | T00d8,`060c` Single,private | `#=zQik3JrVEJfHB` | accumulated accepted progress P |
| `0400029e` | T00d8,`0608` Int32,private | `#=zfEmNFNPOaYN8` | cached converted integer E |
| `040002a2` | T00d8,`0608` Int32,private | `#=zbDqYEmEAUPOEQEWqfw==` | observed-change counter C |
| `040002a0` | T00d8,`0608` Int32,Assembly | `#=zU2RnfG37jR31` | prepared requirement-like operand N, bounded here |

**M05eb /060005eb**, owner T00d8, signature **`200008`**, is a nonvirtual instance method with no explicit arguments and signed Int32 return. Its entire operation is `ldarg.0; ldfld F02b4:0001; conv.i4:0006; ret:0007`. No Abs, Math.Round, floor, ceiling, loop or mutation occurs in it. The source is the stored Single field itself, not raw cursor angle, velocity, elapsed time or counter C.

For finite in-range operands the CIL conversion truncates toward zero. This is not a floor rule for negative artificial P. Primary opcode documentation: [Microsoft OpCodes.Conv_I4, Remarks](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.conv_i4?view=netcore-3.1) (retrieved2026-10-30) specifies truncation, unchecked conversion and unspecified floating-to-integer overflow result. It describes the instruction contract, **not the target build's measured CLR/JIT output**. NaN, infinity/out-of-range return values are deliberately not assigned a concrete Int32 here. There is no conv.ovf.i4 in M05eb; do not invent a checked-conversion exception or substitute a current-runtime cast as a pinned-Windows result.

After all E24 sampling/angle/target work, M05f6 performs:

| Offset | Original operation |
|---|---|
| `02d5` | Invoke M05eb, reading P now |
| `02db` | Read cached E=F029e |
| `02e0 bne.un.s→02e4` | Branch on integer **inequality**, not greater-than or positive direction |
| `02e2/02e3` | Equal: return raw0 immediately; no counter/cache store |
| `02e6–02ed` | Different: read C, unchecked add1, store C at02ed |

The `.un` suffix does not turn this equality test into an increasing-progress predicate. A converted value less than the old cache also takes the change path. One invocation increments C by **one**, not by `newInteger−oldInteger`, and has no loop draining crossed integers. Missing several observations can therefore discard any per-integer interpretation. Neither C nor E is a proven full-revolution, bonus or score counter.

Equality's raw0 return does not undo the preceding angle/reference/target/EMA side effects from E24. Likewise, detecting an integer change does not require the current sampled angle to be nonzero or the key still held: after the original prefix returns normally, even rejected target motion reaches this integer comparison. F2a90=false and a negative sample interval remain earlier returns before this boundary.

## B. First change and first nonzero classification

Result local2 is initialized raw0 at method entry0000/0001. Its only later assignments are **16384 at0358/035d**, **8192 at04a9/04ae**, and **4096 at04ba/04bf**. All normally returning M05f6 values are therefore drawn from `{0,4096,8192,16384}`; no negative failure enum is returned by this method's scoped component path. The original T0488 result enum metadata has only `value__` Int32 and no named literal members. These raw values are not modifier bits despite numerical collisions elsewhere.

### Optional helper occurs before classification

Immediately after C is incremented, M05f6 reads global F2f16 and its original metadata-named Boolean **`SpinnerFrequencyModulate` /040036af**, owner T0b3a (`02f2/02f7`). If true, it calls **M3b28 /06003b28** at030d with original **Single(C)/Single(N)**. This reads C and N again; neither is an immutable saved local from the increment.

M3b28 first obtains M3121(F2356), compares its signed Int64 result with8192, and conditionally invokes M3146 with original channel, attribute1 and an internally calculated Single parameter. Exact native imports establish M3122=`BASS_ChannelGetLength`, M3146=`BASS_ChannelSetAttribute`, module `bass.dll`; M3121 is a wrapper around M3122. This is a native request boundary before cache update, not proof of sound, success, timing correction or a side-effect-free helper. Stop here; no audio synthesis/formula or all bonus effects are recovered. With N0 the original Single ratio can be nonfinite; do not invent an integer divide-by-zero exception or a pinned native result.

### Bounded requirement/bonus predicate

After that helper, the result selector reads C and N afresh:

| Sites | Original branch |
|---|---|
| `0313/0319/031e/031f/0320` | If signed C<=unchecked(N+3), jump0496 |
| `0326–0336` | Otherwise compute unchecked(C−unchecked(N+3)) **signed rem2**; nonzero jumps0496 |
| `0358/035d` | Strictly beyond N+3 with remainder0 assigns raw16384 before later bonus-associated helpers |
| `0497/049d` | Non-bonus branch: if fresh C<=1, go04b1 |
| `04a0–04a7` | Otherwise if another fresh C rem2==0, assign raw8192 and go04c0 |
| `04b2/04b8` | Otherwise if later fresh C>1, assign raw4096; else retain0 |

For stable counter/operand values this projects to:

- Beyond N+3 and even offset: raw16384, the bounded bonus-associated path.
- Otherwise C>1 even: raw8192.
- Otherwise C>1 odd: raw4096.
- Otherwise: raw0.

Signed comparisons, unchecked N+3 and subtraction, and original rem are retained. Arbitrary overflow/corrupt counters need not follow the positive finite projection. Raw assignment and the object identity being evaluated are separate; there is no direct spinner.F152a write here.

There is no separate F02b3 phase-state test in this result continuation. Cursor direction and accepted-motion sign do not select these raw values after P has been accumulated. Earlier input/provider/recorded-state gates and prepared N can still affect reachability.

### Why the first ordinary change returns0

On a **fresh** T00d8, C and E begin managed-default0. For the unchanged proposed finite fixture, assume ordinary imported OD5, plain mods, positive intended duration and successful normal initialization. We need only establish **N>=0 and no N+3 overflow**. M05f4's prepared store is01d4; the sole manager-scalar F0ae0 writer is M28be:005e, through ordinary virtual M125a→M28b3 with positive anchors3/5/7.5. Under raw5/plain flags it selects the positive middle anchor. The finite short fixture duration keeps the prepared value in range. This supplies the necessary sign/range bound; **no exact required-spin value or general required-spin formula is recovered or claimed as runtime data**.

Under that bound, the first changed integer stores C1, cannot reach the bonus predicate, and retains result0; it still updates the integer cache on normal return. To obtain the first nonzero ordinary component, extend the same control only to **a second distinct converted-integer observation**: C becomes2 and selects raw8192. No new map, longer spinner, forced result field or artificially inserted component is necessary. E24's held-motion/positive-integration path can supply progress across multiple qualified updates inside the intended interval, but actual import, callback cadence and crossing operands remain unobserved.

If the first input observation sees P already past several integers, it still stores only C1 and returns0 under these bounds. A repeated unchanged observation returns0 again. Another **different** converted integer is required for C2. C is thus a count of observations, not the mathematical integer part of P.

N is a prepared gameplay operand, not established as a Boolean setting or a live result accumulator. Other original rulesets/subclasses write it differently; they are inventoried but not generalized. Prior count, malformed N or arithmetic overflow can select a different branch even on the first newly observed change. Such states must be labelled artificial/reset-derived unless actually reached naturally.

## C. Counter, result, helpers and cache: exact ordering

On the bonus-associated branch, the optional component/M452b call at0353 occurs **before** assigning raw16384 at0358/035d. After assignment come the global F0cc6 bit4194304 test, conditional resource/effect requests including M3b42/M3b2b, label request M093a at03b6, then original transform-list clear/add/item/flag work through0494. The bit test after035d can suppress an ancillary request without changing the already assigned raw under normal return. No visible feedback identity, sound or complete bonus-payout arithmetic is inferred. The raw4096/8192 branches take a shorter route to the same cache label.

Every **normally returning inequality path**, including C1/raw0, reaches:

`04c0 ldarg.0;04c1 ldarg.0;04c2 call M05eb;04c7 stfld F029e;04cc load local2;04cd ret`.

The cache is written **after** result selection and intervening original helpers, from a **new P read**. It is not assigned the first M05eb result. All paths after the counter store are forward branches; there is no pending-integer drain loop or local exception handler in M05f6.

Consequences, conditional on original helper behavior:

- An exception after02ed and before04c7 can leave C incremented and E stale. A later call observing the same integer can increment C again. No rollback is implemented inside this method.
- If an original callback/reentrant update or another writer changes P between the two reads, E records the later converted P. Static inventories do not establish actual interleaving, and none was observed.
- In serial, stable state, a normally returning changed-integer call updates E and prevents a duplicate for unchanged P. The gameplay timestamp need not change: **the cache, not a once-per-time guard, suppresses that duplicate**.
- C1 advances C and E with raw0. A later C value can advance without a positive result in artificial overflow/nonpositive states too.
- A cache store in this scoped method is always preceded by that invocation's C+1 store, though unchecked wrap may decrease the numeric count. E can jump farther than C advances; it cannot be treated as one-to-one accounting.
- If the subsequent ruleset consumer throws **after M05f6 returned**, E has normally already been updated. An unchanged-P call does not automatically retry the lost handoff.

Direct P writers are M05f4 reset01b3, M05f5 Edit assignment0139 and ordinary accumulation0395. C has **one** direct managed writer M05f6:02ed; E has **one** M05f6:04c7. M05f4 does not reset C/E. Original end-time setter M0604 calls M05f4 at002f, establishing a bounded reset-derived path where P can fall below an old cache without resetting the other fields. Its use during ordinary active Play is not established; treat forced use there as artificial. No general pause/seek/restart model follows.

## D. Exact ordinary handoff and first consumer mutations

Virtual slot M24c3 resolves to T00d8.M05f6, ordinary handler T0253 inherits M0ac5, and handler result slot M0ad2 resolves **M114a→direct base M0ad2**. No alternate spinner-specific ruleset bypass was found on this exact path.

M0ac5 calls object.M24c3 at004c, then separately loads Play.F1380 at0057 and calls its own virtual M0ad2 at005c with **the enum and that later object reference**. There is no zero-value filter here. Stable control forwards the same spinner, but source object and dispatched object are not guaranteed one atomic identity. A changed current reference can receive the earlier spinner's result; a null can throw in M114a's first object dereference. Zero does not make that wrapper null-safe.

M114a first reads supplied object.F252e; if it and object.F1527 and raw!=0 are true, calls Play.M2282 at001a. For a fresh unevaluated spinner this hook is skipped. It then forwards unchanged raw/object to base M0ad2 at0022. The hook remains an opaque original boundary for artificial early-evaluated controls, not a no-op.

Base M0ad2 returns immediately at0003 when raw0, before Play.F13ea and F153d/common combo work. A first changed integer can therefore mutate spinner C/E and invoke the frequency helper while still producing no base result-consumer mutation.

For nonzero input, base **M0ad2:000b stores the supplied reference into Play.F13ea**. It then captures object.F153b and runs the original F05f6-dependent prefix, preserving its helper/queue/record/mod checks and exceptions. Do not assume the prefix is pure or always completes. Local3 begins true at01ed/01ee; local0 numeric amount begins0. Positive masks **−12648449 at0206**, then optional **−14745601 at0226**, preserve the plain raw4096/8192/16384 values. There is no circle/parent M28d3 Perfect conversion inserted into this component route.

| Raw result | Dispatch / branch | Local amount / auxiliary / combo-enable |
|---|---|---|
| 4096 | `0351→0b63` | amount0; Double auxiliary=`F0604*1.7`; local3=false at0b77 |
| **8192** | **`035c→0b7d`** | **amount100; auxiliary=`F0604*1.7`; local3=false at0b92** |
| 16384 | `036c→0b98` | amount1100; auxiliary=`F0604*2.0`; local3=false at0bb0 |

These are Int32 amount and Double auxiliary locals, not player-visible points or a decoded bonus payout. The receiver's **F0604 is mutable**; its full writer inventory includes construction and later setup/variant methods, so an ordinary constructor default does not establish the active gain. Do not substitute1 without an original runtime read.

All three branches rejoin0e2d. With handler record modeOsu0, `0e39 bne.un→0fd0` skips Mania-specific code. Plain raw values have none of the later high decoration masks set. Then **`100c stfld obj.F153d`** records amount0/100/1100 under this scoped control. F153d (`0400153d`, ownerT04c3, Int32) is the first branch-dependent object-local amount store; **it is not spinner.F152a, current combo or a displayed score**.

### Common combo block is bypassed

At119a/119b, local3=false branches directly to12c8, bypassing both M40f1(current+1):11c7 and M40f1(0):1287, along with their common max/snapshot/feedback/SuddenDeath block. The result's positivity does **not** re-enable combo here. T0892.F2674/M40f0/M40f1 retains the E18-verified current-combo identity. Under normally returning original prefix/suffix without independent callback mutations, raw8192 does not increment or reset it through this block. No universal claim that opaque observers cannot change combo elsewhere is made.

### Immediate target and observer boundaries

The selected consequence is already established by F13ea/F153d and combo bypass. The suffix is followed only far enough to identify required immediate boundaries, not complete scoring/HP:

- If Play.F13bb is nonzero, call virtual **M0ad0(raw,auxiliary,false,obj)** at12da. Its false Boolean skips M0acf; positive auxiliary can still reach original M2977/M2978 hooks. No sound/effect identity is asserted.
- `12e9 ble.un→12f9`: finite positive auxiliary calls **M297b(auxiliary) at12f2**; nonpositive or NaN takes **M297a(−auxiliary) at1301**. Preserve separate receiver/reference reads and unknown target state.
- For the ordinary T056a receiver, after its original conditional F1374/F1852 prefix, M297b uses M2968/2969 to add the operand to **F1858**, then M2966/2967 to set **F1857** through the original0..200 clamp. These two stores are separate; neither is a whole-game failure decision. M297a is the distinct subtraction route. E19's later fail/continue mechanism remains a dependency, not an automatic consequence of every component.
- Under original object-flag/record gates, optional F198e policy receives a result-data object `(original raw, false, original extra Boolean)` through M3c57 at1357; otherwise legacy record work is used. E23 provenance/default-selection uncertainty is preserved, not reopened.
- Optional observer **M19f5(raw) at13d8**, F05f6 work and the established Play.M226c boundary can follow. No total-score, HP UI, final-spinner result or visible feedback is inferred.

The first nonzero result's source spinner is not marked evaluated by M05f6, M0ac5 or this direct raw classification. There is no direct F152a store in those reached methods. Object-local amount, raw result local, C/E/P, combo state and later whole-object evaluation remain distinct.

## E. Ordering, missed observations and completion interruption

E24's authoritative sequence remains:

1. Clock/input publication and queued press work.
2. M2275 selects current object using existing candidates.
3. M28e2 refreshes candidates.
4. Optional M0ac5→M05f6 observes existing P and dispatches its result.
5. M28e3→M05f5 can produce **new** P later in that pass.
6. M0654→M28e4 performs the manager's object-state checks.
7. Later draw/timing-tail, subject to host looping/gates.

M05f6 cannot observe a progress store that has not yet occurred. It can observe one from an earlier component pass, including another pass within the same host callback. M43a0's original bounded loop permits up to31 passes; no fixed physical-frame frequency, timestamp atomicity or input-latency claim follows. Each newly observed integer difference can produce one C increment; it does not recover all differences hidden by skipped sampling.

M05f5 can integrate with positive F2a94 while M05f6 is skipped by F2a90 or current-object/optional dispatch gates. E12's rejoin means skipping that input work does not itself skip object updating. Residual smoothed motion can make P cross an integer after release, and the next permitted M05f6 can notice it even if no currently accepted target movement exists.

**Detection has no new start/end/evaluated gate after02d4.** The earlier angle prefix and current-object selection still constrain natural reachability. At exact S/E, integration is excluded but pre-existing P can be observed by M05f6 when its original entry/pointer/interval gates pass. A fresh normal spinner at S has P0, so this is not evidence of earned start-boundary points. After E, a still-unevaluated spinner can remain current through M22c5's late !evaluated branch and report pending progress before the same pass's manager completion.

If optional input is skipped and end<T, the manager spinner branch can instead invoke **M28d3 at M28e4:00a3 → virtual M24a9→M05fa**. Only the minimum completion dependency is inspected: after its original prefix helpers, **M05fa:0020 stores evaluated F1527=true**. It has no direct P/F029e read or M05eb/M05f6 call, and no C writer beyond the inventoried M05f6 exists. No component catch-up is established there. Stop before its final classification formula.

After that ordinary evaluation, a later current Find at time>end rejects the evaluated spinner. A pending changed integer may thus never produce this component notification. Conversely input permitted immediately before completion can report it first. This is a conditional managed-order consequence, not a measured loss of displayed score. Full final judgement is deliberately unrecovered.

M05f6 itself has no F1527 test. Artificial early evaluation inside the interval can still pass M22c5's end>=T branch and run detection, while M05f5 refuses further integration. M114a's optional evaluated hook may then intervene. Normal late evaluated objects are excluded by current selection; do not replace this layered behavior with a new blanket guard.

## F. Complete direct inventories and body identities

The complete direct EXE access inventory for the three core fields is below. Reads include address-taking where applicable; owner/signature metadata for all inventoried methods is supplied in JSON. Static managed coverage excludes reflective/native/external writes and unproven reentrancy.

| Field | Complete direct stores | Complete direct reads/addresses |
|---|---|---|
| F02b4 | M05f4:01b3; M05f5:0139/0395 | M05eb:0001; M05f5:0373/039c |
| F029e | M05f6:04c7 | M05f6:02db |
| F02a2 | M05f6:02ed | M05f5:03c9/03fa; M05f6:02e6/02ff/0313/0326/039c/0497/04a0/04b2; M05fa:002e/0048/005c/008c/00a6/00ba; M05fb:0018 |

N=F02a0's direct writers are M05f4:01d4 plus Taiko M1d6d:0024/005a/0093 and M1d78:012d. Those alternatives are inventoried, not applied to T00d8. F0ae0 has one direct writer M28be:005e. The supplied original-method signatures, constants, primitive instructions and hashes make these claims reproducible without publishing full disassembly.

All selected original body hashes/RVAs/signatures are in the evidence. Representative SHA-256 values include the original header:

| Method | Body SHA-256 |
|---|---|
| M05eb | `2d5d1f111e60f15222ce634620027b6daaf27996ddd6827c87211d406bfc73d1` |
| M05f6 | `8ef889e9b9e90843e13f78e5e23353157e3d2098dce9e3ded7e56babda460efa` |
| M0ac5 | `1166e408b2fbaf918fb5d944be50abda75fe7efcca2e4520bf72f94b849c42b4` |
| M114a | `0807575024205f26b05e0ba7cb21fd11a4541b05d15245bc7fb6167b55b57d71` |
| M0ad2 | `2276c97a07e987cdb9b72e2dcf256fcf105a074ebf4e03f910feb58adebcaaec` |
| M3b28 | `fe634ce4a424ecbe6945bc8132dc1d437bfffbddc23ff5c2c34fe0830c7d25fc` |
| M297b | `81b3e757e0c7f6c5be645a3ba5c6189a41074aeb5eac7a317e50dbc3a153ee4c` |

## G. Conditional static reference cases

All rows are **NOT RUN** against original stable. Preconditions: intended E24 finite ordinary fixture, successful import/initialization, stable object/provider references and normally returning original helpers unless varied. N bounds are stated rather than fabricated runtime counts. P values denote **stored Single operands**; naturally reaching them must be verified, not achieved by forced fields and called natural spinning.

| ID | State / differing condition | Predicted original branch | Runtime |
|---|---|---|---|
| I01 | Fresh P0,E0,C0 | conv0 equalsE; raw0; no C/cache write | NOT RUN |
| I02 | Finite0<P<1,E0 | Converted0; raw0 even though fractional progress exists | NOT RUN |
| I03 | P changes fractionally within same integer,E matching | No changed-integer event; C unchanged | NOT RUN |
| I04 | P just below1 then exactly1,E0,C0,N nonnegative/no overflow | C1,raw0,E1 on normal return | NOT RUN |
| I05 | P just above1,E0,C0 | Same integer1 and C1/raw0, not a rounding-to2 result | NOT RUN |
| I06 | P1→1.75,E1 | conv1 equalsE; raw0,C unchanged | NOT RUN |
| I07 | Next observed P2,E1,C1,ordinary N bound | C2; first raw8192; E updated before dispatch | NOT RUN |
| I08 | M05f6 skipped across several P stores then P2.5,E0,C0 | One change only: C1/raw0/E2; no two-entry drain | NOT RUN |
| I09 | Several missed integer crossings after clock/update discontinuity | Same inequality algorithm; one C+1 if conversion differs, not count difference | NOT RUN |
| I10 | First changed integer, C initially0, threshold bound | Raw0 regardless of how large the finite converted increase was | NOT RUN |
| I11 | Changed integer with prior C1 | C2/raw8192 under non-bonus predicate | NOT RUN |
| I12 | Changed integer with prior C2, non-bonus predicate | C3/raw4096; amount0 but positive raw/auxiliary route | NOT RUN |
| I13 | C+1 equals N+3 | Strict bonus test not passed; ordinary parity selector applies | NOT RUN |
| I14 | C+1>N+3, offset rem2==0 | Raw16384 assigned; original bonus-associated helpers precede cache | NOT RUN |
| I15 | C+1>N+3, offset rem2!=0 | Ordinary parity selector; no inferred bonus payout | NOT RUN |
| I16 | Repeat permitted M05f6 at identical T and unchanged P/E | Equality raw0; timestamp equality is not the suppressor | NOT RUN |
| I17 | Counter increment then helper throws before cache | C changed,E stale; no normal result handoff or rollback established | NOT RUN |
| I18 | Retry same converted P afterI17, original gates/helper now return | Stale E can cause another C+1 and different raw result | NOT RUN |
| I19 | Normally returned changed-integer call | Cache always written from later M05eb before caller dispatch | NOT RUN |
| I20 | Artificial/reinitialized P below old E | Inequality still increments C; no positive-direction check | NOT RUN |
| I21 | Clockwise versus matched counterclockwise yielding same stored P | Same integer/result selector; angular sign not read in this continuation | NOT RUN |
| I22 | Direction reversal or release with residual integration | P can continue changing under E24 conditions; next eligible observation can notify | NOT RUN |
| I23 | Cursor stopped,U0,P stable,E matching | No new component notification | NOT RUN |
| I24 | Optional input skipped but M05f5 reached | Pending integer may build without immediate C/cache/result work | NOT RUN |
| I25 | Earlier component pass crossed integer; next pass in same host callback | Can be noticed in later pass if original gates permit; not necessarily another physical frame | NOT RUN |
| I26 | Exact start with fresh ordinary state | No prior earned P; no new integration; normal detection equality returns0 | NOT RUN |
| I27 | Exact end with pending changed integer | Input detection may report it; strict integration and manager end-equality checks do not complete it here | NOT RUN |
| I28 | Just after end, input dispatch allowed before manager update | Pending notification can precede evaluation in same pass | NOT RUN |
| I29 | Just after end, input skipped and manager evaluation returns | F1527 set; subsequent late current selection can suppress outstanding notification | NOT RUN |
| I30 | Artificial early-evaluated spinner inside interval | M05f6 has no evaluated check; M114a optional hook depends on other flag; M05f5 integration stops | NOT RUN |
| I31 | Raw0 with stable valid object at ruleset boundary | M0ac5 forwards0; M114a reaches base early return; no base F13ea/F153d/common combo work | NOT RUN |
| I32 | Raw8192 ordinary consumer, normal return | F13ea identity store; F153d100; common combo increment/reset bypassed; auxiliaryF0604*1.7 | NOT RUN |
| I33 | Raw4096 with matched ordinary consumer state | F153d0; same combo bypass; auxiliaryF0604*1.7 | NOT RUN |
| I34 | Raw16384 bounded alternate consumer | F153d1100; combo bypass; auxiliaryF0604*2; no displayed payout claim | NOT RUN |
| I35 | F0604 finite positive versus0, respective normal suffix | Positive auxiliary→M297b; zero→M297a(−0); actual state values/observers still needed | NOT RUN |
| I36 | Current object changed between evaluation and0057 reread | Raw source and dispatched object may differ; cache already belongs to evaluated spinner | NOT RUN |
| I37 | Current object becomes null at dispatch | M114a can throw before base, including for raw0; no automatic retry for already cached change | NOT RUN |
| I38 | Consumer/helper throws after M05f6 normal return | Cache updated already; unchanged progress normally produces0 next time | NOT RUN |
| I39 | Artificial concurrent/reentrant P change between M05eb reads | Cache captures second converted value, not first; actual natural interleaving unobserved | NOT RUN |
| I40 | NaN/infinity/out-of-range P, artificial | Preserve original conv.i4; concrete target-CLR integer and resulting equality branch unresolved | NOT RUN |
| I41 | Artificial negative P−0.75 versus−1.25,E0 | Finite truncation toward0 distinguishes0 from−1; no Abs/floor replacement | NOT RUN |
| I42 | SpinnerFrequencyModulate false versustrue | Helper skipped versus original ratio/native boundary before classification/cache; output predicates otherwise unchanged if state stable/normal return | NOT RUN |
| I43 | Artificial counter overflow at C+1 or N+3 | Preserve unchecked Int32 add/sub and signed rem/comparisons; positive-count projection need not hold | NOT RUN |
| I44 | Forced inconsistent N−4,C0 then changed integer | New C1 is beyond N+3 with even offset, permitting16384; not ordinary fixture behavior | NOT RUN |

For concrete near1 Single controls, the preceding representable value has bits`3f7fffff`, exactly1 is`3f800000`, and the next is`3f800001`; these are static operand descriptions, not original math execution. A Float-to-Int cast in a current Python/.NET surrogate is not an accepted substitute for a pinned CLR measurement.

## H. Minimal E20 observation addition and next question

Do not edit/rebuild E20's packet. Reuse its isolated Windows reference/provenance/overhead requirements and unchanged E24 fixture when a host is authorized. Verify actual import/type/manager/identity/S/E, actual N sign/range and original settings/input/provider conditions first. Do not inject a spinner or force P/result fields to bypass import.

Add only logs at original consumption/store sites: M05f5:0395 P before/after; both M05eb inputs/returns at M05f6:02d5/04c2; cachedE, C before/after02ed; exact N+3/rem/parity operands and selected local result; SpinnerFrequencyModulate/helper entry/return/exception; cache04c7 and return04cd; M0ac5's separately read object/raw at0057/005c; M114a hook/gate; M0ad2 masks/branch/F13ea/F153d/local3/common-combo bypass and first target/observer calls. Include consumed clocks, F2a90/F2a94/F2a95, callback order/thread/high-resolution host timestamps and module identity. Do not call timing methods again to log values.

Naturally exercise two distinct converted-integer observations, matched directions, stationary hold, release/residual movement and skipped attention where feasible. Forced exact Single values, negative/NaN/overflow progress, stale-cache/counter edits, helper exceptions and object-reference swaps are separate artificial probes. Compare traced and uninstrumented controls and measure overhead. Record actual MATCH/MISMATCH/INCONCLUSIVE/NOT RUN statuses only from real evidence. **No runtime observation or overhead value exists here.**

Smallest subsequent spinner question: recover the bounded whole-spinner completion evaluator from its already identified M05fa entry, using the observed-change counter and prepared operand, up to the first manager result handoff. Keep required-spin preparation and bonus payouts separate. Original-Windows E20 validation remains the priority verification dependency. Work2 remains suspended; distorted-slider stutter remains unrelated and unmeasured.

# 3. Pseudocode

**RECONSTRUCTED — independent dataflow description, not an executable spinner/result/scoring implementation.** Original entry gates and helpers from E24, separate reads, exception behavior and ordinary-versus-variant dispatch are retained.

```text
M05eb(this):
    return original conv.i4(Read(this.F02b4))      // no Abs/floor substitute

M05f6, after original angle/target prefix:
    // result local was initialized0 at method entry
    observed = original M05eb()                  // first P read
    if observed == Read(F029e): return raw0
    F02a2 = unchecked(Read(F02a2) + 1)

    if Read(global F2f16).SpinnerFrequencyModulate:
        original M3b28(Single(freshRead(F02a2)) / Single(Read(F02a0)))

    if freshRead(C) > unchecked(Read(N)+3)
       and unchecked(freshRead(C)-unchecked(freshRead(N)+3)) rem 2 == 0:
        retain original optional F02a8/M452b work BEFORE raw assignment
        result = 16384
        retain original later conditional presentation/native/helper work
        goto cache

    if freshRead(C)>1:
        if freshRead(C) rem 2 == 0:
            result = 8192
            goto cache
    if laterFreshRead(C)>1:
        result = 4096
    // else keep result0
cache:
    F029e = original M05eb()                     // SECOND P read
    return result                               // cache before handoff
```

**RECONSTRUCTED — ordinary consumer boundary:**

```text
M0ac5:
    if original current-object reference exists:
        result = currentObject.virtual M24c3(storedPointer)  // spinner M05f6
        objectForDispatch = separately read Play.F1380
        this.virtual M0ad2(result,objectForDispatch)
        retain original suffix

ordinary M114a(raw,obj):
    if obj.F252e and obj.F1527 and raw!=0: original Play.M2282(raw)
    base M0ad2(raw,obj)

base M0ad2, scoped stable ordinary path:
    if raw==0: return
    Play.F13ea = obj
    retain original F05f6-dependent prefix and masks
    if raw==4096:  amount=0;    aux=F0604*1.7; comboEnabled=false
    if raw==8192:  amount=100;  aux=F0604*1.7; comboEnabled=false
    if raw==16384: amount=1100; aux=F0604*2.0; comboEnabled=false
    retain original ordinary-mode/mask work
    obj.F153d = amount
    skip common combo increment/reset block because comboEnabled is false
    retain original optional callback
    if ordered aux>0: original target.M297b(aux)
    else: original target.M297a(-aux)
    retain bounded policy/observer/failure-decision boundaries
```

**RECONSTRUCTED — ordering only:**

```text
component pass:
    current selection from old candidates
    candidate refresh
    optional input -> M05f6 -> result consumer
    candidate update -> M05f5 -> new P
    manager object update
        if reached spinner end<T and unevaluated:
            original M28d3 -> M05fa
            after original prefix: F1527=true
            STOP before final judgement formula
    // later component pass can observe new P, unless attention/evaluation prevents it
```

**UNRESOLVED:** actual fixture import, source/CLR conversion edge outputs, original C/E/P/N/clock/reference values, helper/interleaving/callback behavior, runtime result delivery/target/observed combo, displayed score and tracing overhead. E20's17 cases remain NOT RUN; none of this static pseudocode completes runtime validation.
