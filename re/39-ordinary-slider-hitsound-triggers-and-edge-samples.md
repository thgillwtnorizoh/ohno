# 1. Result

Expedition 039 — Ordinary Slider HitSound Triggers, Edge Samples & Audio Dispatch.
Work1, 2026-11-04 Asia/Bangkok. Original-static research only, on canonical
`main` starting at `df99f40d2cede2fc6944116f6a3f7cf9a2e47fb9`.
No authorised exception. No reconstruction, historical research, independent QA,
fixture, scoring or health implementation is changed. Stop after E039.

**Recovered:** the embedded head's inherited circle hook; the independent latest
due-prefix audio progression; dedicated tick selection; indexed edge selection;
aggregate-positive edge requests; parser-to-head/parent sample propagation; shared
resource/queue boundaries; and the entry, channel and pause boundaries of the
separate continuous subsystem. A slider sound is **not** universally a reaction to
a successful checkpoint judgement. The audio index and component counters have
different consumers and can produce different histories.

Added this report, `evidence/39/slider-hitsounds.json`,
`tools/slider_hitsound_evidence.py`, `tools/check_slider_hitsounds.py`, and an
append-only E039 entry in `contexts.md`. Publication SHA is supplied in the final
handoff after canonical remote verification.

| Pinned private input, verified before original instruction decoding | SHA-256 |
|---|---|
| `osubackup.zip` | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| ZIP member `osu!.exe` | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |

Self-declared Stable40/file version1.3.3.8 remains unauthenticated as a dated,
unmodified official build. Analysis uses Windows, Python3.12 and read-only
dnfile0.18.0/pefile2024.8.26/dncil1.0.2. Neither executable, original CLR method,
native audio method nor gameplay client is executed. No input, debugger, audio
device/listening, capture or OBS experiment occurs. Original sample bytes are
never exported as deliverables. E020 retains all17 NOT RUN cases.

Preparation consulted the actual AGENTS/standing contract and directed RTK,
both journals, E015–E018/E021–E023, E037/E038 and their evidence, the relevant
E004–E006/E010–E012 boundaries, R11/QA09 and the actual current parser, slider
session, sample selector, resource and effects interfaces and Windows README.
E005's corrected ordinary factory remains authoritative; E004's historical Taiko
annotation is preserved, not reused as an ordinary factory.

Competing explanations checked directly: head calls parent sound override versus
its own inherited hook; checkpoint results versus a separate update producer;
drain-all versus latest-prefix audio catch-up; final checkpoint versus aggregate
sound; common object sample overrides versus edge/tick-specific timing inputs;
custom field existence versus a reached custom selector; per-slider player versus
shared continuing channels; native pause versus native stop.

## Deterministic verification

The collector asserts both pinned hashes before decoding. It records92 selected
MethodDefs, signatures/RVAs/body hashes, code-relative selected sites, branches,
locals/arguments, array/integer operations, ancestry/slot declarations,
MethodImpls, exhaustive direct watched-field/caller inventories, ten independently
decoded E038 string keys, and nine slider-resource metadata fingerprints.
Full scan: **19,659 decoded bodies /1,739 zero-RVA entries /0 decode errors**.
Every selected branch destination must be an instruction boundary. No full
disassembly, executable surrogate slider or audio data is distributed.

Two fresh final extractions are byte-identical and match the published JSON:
**PASS**,1258274 bytes/SHA256
2a45c9a2d6565246cfe9a627f6ef96929f7c36cd81fd4c9780e8d5375ad58a6e.
Independent archive-backed identity/body/resource checks and the literal
relationship checker: **PASS**. E038 archive-backed checker: **PASS**,56/56
strings independently decoded and12 historical embedded resource keys checked.
New Python tools compile in memory; three report sections/48 unique conditional
cases and source/evidence fingerprints: **PASS**. Historical checker results
executed: **PASS E004/E005/E006/E010/E011/E012/E015/E016/E017/E018/E021/E022/E023/
E037/E038** (15 unchanged checkers). The new literal relationship checker passes
92 methods and416 historical body-hash comparisons. These counts concern static
evidence, not gameplay cases. QA01–QA09's **862** passing independent checks remain
a preserved historical checkpoint; no QA or WPF build is claimed by this expedition.

Reproduction with the pinned Python packages available, repository root, private
archive/output paths (all shell commands retain the directed RTK prefix):

```text
rtk proxy python re/tools/slider_hitsound_evidence.py /private/osubackup.zip /private/slider-a.json
rtk proxy python re/tools/slider_hitsound_evidence.py /private/osubackup.zip /private/slider-b.json
rtk proxy python -c "from pathlib import Path; assert Path('/private/slider-a.json').read_bytes()==Path('/private/slider-b.json').read_bytes()==Path('re/evidence/39/slider-hitsounds.json').read_bytes()"
rtk proxy python re/tools/check_slider_hitsounds.py re/evidence/39/slider-hitsounds.json /private/osubackup.zip
rtk proxy python re/tools/check_hitsound_resources.py re/evidence/38/hitsound-resources.json /private/osubackup.zip
```

The actual local commands prepend `../.research-deps` and `re/tools` to Python's
module search path; they do not install or load a CLR. Optional archive checking
independently verifies selected method identity/body bytes and resource metadata.
Literal relationships in the checker were authored from original instruction
inspection, not generated from the collector's predictions. One draft literal
offset typo for M2295's raw64 constant was corrected from01b6 to the inspected
**01b7**; no historical assertion or original conclusion was changed.

# 2. What they found

All **CONFIRMED STATIC** claims below refer to pinned original instructions or
metadata. **RECONSTRUCTED** equations, semantic aliases and finite controls explain
those instructions conditionally. **ENGINEERING** identifies a future bounded
adapter decision. Original Windows gameplay, native output, acoustic timing and
human audibility remain **UNVERIFIED**. Mxxxx/Fxxxx/Txxxx abbreviate original
MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx. Offsets are hexadecimal relative
to the first CIL opcode, excluding headers; JSON site offsets are decimal.

## Representative original method-body integrity

Full original signatures, FieldDefs, ancestry and all92 body identities are in JSON.
Hashes below include original method headers; code lengths exclude headers.

| Method / owner | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M143a / T02af | 0x7abdc | 95 | efb86763cd228dd2e3b988a249d7e362d17a2b2a8ff7bde18f4e09bcda512da3 |
| M1449 / T02af | 0x7b204 | 556 | b65870f15ba4c334e9d9287d521ef3554224dfe2382a06f020c833c0155dc84f |
| M1465 / T02af | 0x7df28 | 254 | 8a76dd6cd93705ce792566422c7ab2b9cec04a6eab5719518e0f226701e547ab |
| M1466 / T02af | 0x7e034 | 46 | 265bc5420df0a36f6b9915a735e47e2ad16715368b9534157c3af8d209a3318d |
| M1467 / T02af | 0x7e070 | 35 | fce1f38936aa4330e122e0d4ff82dc5c22b0286bbe531fecb3f21936f74797f9 |
| M1469 / T02af | 0x7e0b8 | 156 | 8e5a7db9d5b61f87363b920b0637ffa0036829ed76d2755051bd94bbac22a3a4 |
| M146a / T02af | 0x7e160 | 490 | 262ff38645dc2570e379b3c40b42eb947a287b6ffa43d4a5adf5f9c4a6d99d66 |
| M146b / T02af | 0x7e358 | 79 | a9557bebce52cbed3524cff832728d45b596acbfa32b7839469b6728ce0698db |
| M351d / T06f6 | 0x152ef4 | 19 | 4342e98c65e1906924b6c71e6bb215c3e077471577ec5ba9c01c93cad25bb7ce |
| M48e0 / T09c1 | 0x1cda58 | 137 | d428bc5c99ad965076ad2905413bbe06c5ca8c8ab99ba20cca4e93ad90d19602 |
| M3b20 / T07b9 | 0x180d9c | 330 | c50a3770060a6bda1356e4f0286bd17b16a362cc61c479a0ff8e72642ca7811a |
| M3b33 / T07b9 | 0x181bd8 | 193 | 6e5458f35a8148c42375de1087f163292be2bf609d785801cf78cb28e9d1d4a0 |
| M3b35 / T07b9 | 0x181cf8 | 44 | cb601d6e9375d9cf9498518b313413025c19301bfbc1305df66050f5093b13e1 |
| M3b53 / T07b9 | 0x1829b8 | 44 | 52585d9d4a9db6ec4e12842a8f7cdef18789be54d02e2b08e3a583bcae1f10aa |
| M3ba8 / T07c4 | 0x183944 | 23 | 9d2dad6be1fd31f156a1cfb091d79e95b8b43d8ddd3f6940a780e02dd5ec52d1 |

## A. Ordinary identity, head construction and its sound hook

The corrected path is ordinary handler M113e → manager T00e8/M0651 → factory
T09c1/M48de → slider factory **M48e0 → T02af/M142d**. Parent ancestry is
T02af→T0339→T04c3→T0828. The registered object is this parent: parser
M28f7:1aa4 factory return, then1aae M28f1→M28d2. The embedded head is
not separately registered in that chain.

M142d:01b5 invokes virtual M143a when presentation F0bd7 exists. M143a:0054
constructs **T06f6/M351a**, storing it at0059 in F0bdf. F0bdf is declared as
circle base T094c, signature `0612a530`; its concrete constructed reference is
T06f6. The head receives manager, parent X/Y through Int32→Single conversions,
parent stored start, NewCombo, selected initial sound and colour argument.
Initial sound is parent.F2526 in simple mode F0bee=true, otherwise
F0bf9[0] (0033–004e). This access has no short-list catch. M351a:000a calls
M45eb; M45eb/M45ec retain only sound bits2/4/8 as circle Boolean operands.
Parent M142d:00a0 instead stores the supplied sound mask directly in F2526.

**Slot resolution matters:** T06f6→T094c→T035a→T04c3→T0828 has no M24bb or
M24bd replacement. Its inherited sound is **T04c3.M24bb**, and its pan is
T04c3.M24bd. It does not invoke parent override M1466 or parent pan M1467.
T06f6 overrides evaluation with M351d→direct M1884. The ancestry declarations,
matching signatures/reused slots and MethodImpl inventory are included in JSON.

Parent M1464:003b calls head.virtual M24a9→M351d:0001→M1884. In M1884 the
head stores its raw quality/evaluated state, reads stored result at0076, and
only when signed-positive calls virtual M24bb:007f. It then rereads raw0086,
calls M24b4(result>0):008e, rereads0094 and returns. M351d returns a positive
head result unchanged, otherwise **−262144**, without replacing the stored
head raw−131072. Live M2295 gets that return at01a2, then sends **64 with the
parent** at01b7/01ba if positive. Thus the head hook precedes that raw64 handoff,
with intervening original callbacks and result rereads preserved.

The head's M24bb queries M1be6 at **Double(unchecked(head.F2523+5))**,
then uses the E037 shared M24ba observer/custom/standard selection. It does not
select at the late hit time. Null timing makes this inherited head hook return.
Its own X supplies `(X/512f-.5f)*.800000011920929f`. Positive300/100/50 share
the hook; head MISS has no sound producer in this positive hook. Upstream
evaluated guards, rather than evaluator idempotence, normally prevent repeats.

### What the ordinary factory actually copies

M48e0, after construction, writes **parent** base/addition F1537/F1538 at001f/
0027, edge base/addition lists F0bfa/F0bfb at002f/0037, index F1539 at003f,
volume F1536 at0047, then calls parent M24b9(filename):0050. Only afterward,
if the head exists, it calls **M146a(0):005f** and copies the resulting selected
base/addition fields F2b37/F2b38 into the **head** at0071/0082.

It copies **neither index, volume, filename nor custom handle** to the head.
Shared constructor M249b:0018 initializes each object's custom F1534 to−1;
the fresh head's index/volume remain CLR zero. Accordingly its inherited circle
selector uses its own start-selected timing index/volume. The head's base/addition
have already been prepared by M146a(0), including its version rule below.
No sample copy was found in the ordinary M144a presentation-registration work;
that work shares drawable collections, not parent sample state. This is bounded
to the fresh ordinary path, not editor/cloning/subtype/external mutation.

An unsuccessful head can coexist with later parent tick/edge/loop requests.
E018 additionally established the separate negative-result **combobreak** feedback
when combo>20 and its suppression gates permit; E038 statically decoded that key.
It is a shared result-consumer request, not a dedicated missed-head sample or
proof that every head miss is audible. This expedition does not reopen that
consumer or classify every unrelated combo/feedback sound as a slider hitsound.

## B. Gameplay checkpoints and audio progression are different state machines

| State | Original fields and writers/readers |
|---|---|
| Gameplay next component | F0bf1(successes)+F0bf5(failures); M146f:02d1/031e writes, Boolean[] F0bf6 stores02df/032c; M1465 later adds the positive-head unit |
| Presentation/audio progress | Int32 **F0bf4**; direct writers only M1431:0019 and M1449:0131/0210/021e/0226; M146f does not read it |
| Current tracking state | Boolean F0bf3; direct stores only M146d:020d=true and M146e:00fb=false; M1449 reads it at0113 |
| Mixed schedule | List<Int32> F0bf8, allocated M142d:0023; M144a interior Add0f42, traversal-end Add11db and final-entry adjustment14d7 |
| Edge timing subset | List<Int32> F0bf7, prepared by M144a:14f8–1551 using N/R divisibility, excluding final entry |

E016's M146f processes **one** outstanding due gameplay entry, and can succeed
only when followOK and tracking entry<=that entry's deadline. Its raw result
selector uses due-prefix count k; raw64/128 are not dependable entry identities.
**There is no direct tick/edge/loop playback call in M146f.** Enter/leave helpers
change tracking/presentation state; negative classification does not itself call
M1469. An entry-time failure can retain F0bf3=true, while a held/space failure
with pending components can call leave before the later object update.

### Exact M1449 audio-update order

M1449 first retains base/dirty/Edit preparation and ball direction/animation work
(0000–00b3). Its initial N/R parity work can divide by zero on inconsistent
prepared inputs; no safe divisor is inserted. The following ordinary unchanged
geometry path is **CONFIRMED STATIC**:

1. Set local latest=−1 at00b8 and i=0. Scan in supplied order while
   **i < F0bf8.Count−1** (00d7–00e5). Read times[i], compare with a separately
   read F2358:00ca; if time>that sample, stop. Otherwise latest=i and continue.
   Equality is due. The final list entry is deliberately excluded, not delayed
   by a different comparison. This is a **prefix scan**, not a maximum over all
   due timestamps or a sorted reconstruction.
2. Read gameplay time again for start comparison00e7 and end comparison00f7.
   Eligible range is **start<=T<=end**, and separately read F2368 must equal
   numeric1 (0107–010d). E010 did not recover readable names for this enum;
   retain the numeric test, rather than substituting a generic running flag.
3. In range/state1, read **stored tracking F0bf3**. If true, call **M146b:011e**
   first, even when no new discrete event exists. If latest<=oldF0bf4, return.
4. When latest>oldF0bf4, **store latest to F0bf4 at0131 before selection**.
   Compute h=signedDiv(N,R). If signedRem(latest+1,h)>0, call
   **M1469(true,latest):015c**. Otherwise call
   **M1469(false,signedDiv(latest+1,h)):0180**. Continue the original
   presentation-transform work, then store latest again0210.
5. If in range/state1 but tracking=false, call virtual M24c4→M146c at0217,
   then **store latest at021e** and return. Missed audio events are consumed
   by this index progression; reentry without a new due entry does not replay them.
6. Outside range or state!=1, store **−1 at0226** and return. That branch does
   not itself call the tracking-false cleanup hook. Wider manager/pause behavior
   remains separate.

`true` is the tick arm argument, `false` the edge arm argument; these semantic
names are RECONSTRUCTED from the consumers, not named original parameters.
Arithmetic is signed unchecked Int32. Here h is an equation alias: the false
arm rereads Count/traversals and recomputes N/R for its quotient, rather than
reusing a cached original local. The edge index is the quotient above,
not a raw64 result and not the gameplay processed index. For the finite E016/
E021 schedules it coincides with the producer's relevant traversal boundary.
Do not generalize that coincidence to malformed, unequal or externally changed
schedule counts; preserve the actual quotient/remainder selector.

A jump over multiple deadlines produces at most **one discrete family invocation**
from a reached M1449, for the latest leading due entry before the last. Earlier
requests are skipped. The standard family can expand to several layer requests.
The next same-time update can continue loops but emits no new discrete family if
the index is unchanged. Gameplay can still consume a backlog one entry per call.
There is no universal one-sample-per-frame rule.

**Initialization is not a guessed −1:** M142d contains no F0bf4 assignment;
fresh CLR value is0. Reset M1431 explicitly stores0 at0019 after base reset,
cleanup and head reset. A reached pre-start M1449 normally writes−1. The controls
below require that pre-start history unless varied. Direct factory/reset-state
calls without it can skip audio index0. Outside-range rewinds can reset−1 and
permit a later repeated request; this does not establish full native restart or
seek equivalence. M1431 does not reset all component counters in this scoped path.

## C. Dedicated tick requests

M1469 has signature `2002010208`, instance Void(Boolean,Int32), on T02af.
It first reads F2358 and parent end; if **T<=end**, invokes M146b:000e.
This is a second loop-control call on an ordinary new-event M1449 continuation,
in addition to011e. Preserve both and the separate time reads; native callbacks
can run between them. M1469 itself has no tracking or evaluated guard.

All sound queries here use E037's M1be6: null/empty list returnsnull; otherwise scan
all supplied timing records and retain the last encountered time<=query, with
first supplied record as fallback when none qualify. Equality qualifies, no base/
inherited filter or sort is inserted, and timing time remains Double.

For true at0013–0081:

| Operand | Literal instruction-derived source |
|---|---|
| Timing query | **Double(unchecked(F0bf8[suppliedIndex]+2))**, 0022–0030; no lookup at hit/current time |
| Base set | parent.F1537 if nonzero, otherwise selected point.F0663, 0036–004c |
| Mask | parent.F2526 at004e, not EdgeSounds at this index |
| Index / volume | point.F0661/F0665 at0055/005b; parent F1539/F1536 are not read |
| Addition set | parent.F1538 if nonzero, otherwise selected base, 0060–0071 |
| Sample record | direct **new M44a1** at0071, not M44a2; no Normal augmentation or M28a1 observer in this tick arm |
| Pan | parent.virtual M24bd→M1467 at0077 |
| Dispatch | **M3b35(info,pan):007c** |

This tick branch does **not** null-check the timing lookup. Missing timing can
throw before a discrete request. It does not borrow M146a's no-timing defaults.

M3b35 (T07b9, signature `00020111a4600c`, static Void(T0918,Single)) calls
**M3b36(mask,base,index,lookupMode1):0013** then
**M3b31(handle,volume,pan,true,1f):0025**. In mode1, M3b36:0004–0006 skips
mask row decoding; row stays0 and the table is **F2375[1,3,3]**. Thus this is
the dedicated **slidertick** family, not Whistle/Finish/Clap layers inferred from
the parent mask. Addition set does not participate in M3b35's lookup. Tick volume
is forwarded directly: **no Max(8,V), .8 or .85 attenuation**. The later global
gain and rate-matching behavior belongs to shared M3b31/E037.

## D. Edge requests, sample association and aggregate completion

The false arm M1469:0082–009b calls **M146a(index):0084**, passes the resulting
record through **M28a1:008a**, reads virtual parent pan0090 and invokes
**M3b32(info,pan,true):0096**. It never branches on parent F1534. Shared E037
standard dispatch remains Normal→Finish→Whistle→Clap, with volume floor8 and
Double→Int32 multipliers .8/1/.85/.85. Normal uses base bank; the other layers
use nonzero addition or base. M44a2's conditional Normal augmentation is retained.
Observer exceptions/mutation can interrupt or affect dispatch.

### Exact M146a selector

M146a is nonvirtual despite M48e0's `callvirt`; signature `200111a46008`,
instance T0918(Int32). Let e be its supplied edge index, R effective traversals,
simple=F0bee, sounds=F0bf9, bases=F0bfa, additions=F0bfb. Lists are independently
read with original Count/get_Item calls, not treated as one atomic array snapshot.

| Decision | Selected timing and sample operands |
|---|---|
| e==0 | Query parent start+5 (0014–001c) |
| e!=0 and (e>=R **or e==−1**) | Query parent end+5 (00d7–00fd) |
| Other e!=0 | Query **F0bf7[e−1]+5** (0100–0121), before list-length fallbacks; an invalid subset access can throw |
| simple true, either timing arm | Parent mask/base/addition F2526/F1537/F1538 |
| e==0, explicit edge mode | sounds.Count==0 ? parent mask : sounds[0]; addition.Count==0 ? parent addition : addition[0] |
| e==0 base compatibility | Use parent base if bases.Count==0 **or** (format<14 and bases[0]==0); otherwise bases[0] (0067–00ab) |
| Other e, explicit edge mode | For each list independently, e>=its.Count ? parent corresponding value : list[e] (0141–01ac); no negative-index repair |

Then, if timing exists, base0 inherits its F0663, and index/volume come from
its **F0661/F0665** (01ad–01c8). If timing is null, base0 becomes **Normal1**,
index=**0**, volume=**100** (01cc–01d6). Addition0 becomes the selected base
(01d8–01dc). Finally **M44a2(mask,base,index,volume,addition):01e4** constructs
the conditionally augmented standard sample record. Parent F1539/F1536/F1534
are not read. This differs materially from the circle M24ba override policy.

For valid fresh parser lists, association is head entry0; first reversal entry1;
later reversals2…R−1; final edge entryR. Timing at intermediate edges comes from
the prepared subset, not a new generic duration multiplication. The fallback
e==−1 supports the simple empty-sounds aggregate path after the midpoint.
Manually shortened/negative-index states retain original unchecked/list behavior;
they are not silently assigned these valid associations.

### Final checkpoint is not the final sound producer

M1449 excludes the last schedule entry. M146f's final success128/failure−524288
has no dedicated terminal sound call. Its adjusted deadline is earlier than the
parent end in the finite controls. Instead **M1465 aggregation**:

1. Sets parent evaluated=true at0002.
2. Calls virtual cleanup M24c4→M146c→M3b53 at0008.
3. Adds one successful-head unit when its stored raw is positive; calculates the
   E017 ratio from current successes and schedule.Count+1. No pending drain.
4. **If ordered ratio>0**, calls virtual M24bb→**M1466 at004d** before selecting
   the optional head-quality grading policy and before manager decoration/handoff.

M1466:0002–0016 compares a fresh F2358 against
`unchecked(start + signedDiv(unchecked(end-start),2))`. At or before that midpoint
it passes edge0. After it passes **sounds.Count−1**. M1469(false,index):0028
performs the edge selector above. With a valid nonempty R+1 list and ordinary
completion at end, this is edgeR; with no list, it is−1. Zero/negative/overflow
durations or artificial early invocation must use the actual midpoint branch.

A positive aggregate can request its edge even after a failed final checkpoint,
with a missed head but earlier component success, or with pending components.
Ratio0 skips this hook. It is **not conditioned on successful tracking at the
final checkpoint or current F0bf3**. At an exactly-end clock M1469's first read
can call M146b again after the cleanup was queued; after-end it skips that call.
Do not describe the first cleanup as a permanent stop that no later helper can
counteract. Later Perfect conversion can alter the manager result after the
sound hook, as E012/E017 already established.

## E. Parsed metadata, defaults and bounded malformed handling

CONFIRMED M28f7 (ordinary call reaches M1871→M48e0) distinguishes:

| File operand | Original processing and storage |
|---|---|
| Initial hitSound, human column5 / index4 | M09e5 at14a8→local36, forwarded1a81→M142d.F2526:00a0; no parent circle-bit normalization |
| Traversals, index6 | M09e5:17da; >9000 throws; constructor separately Max(1,raw); edge allocation/padding still uses **raw+1** |
| EdgeSounds, index8 | Missing/empty leaves local39=null (1687). Nonempty split on `|`:1842; take Min(split.Length,raw+1):185d; **Int32.TryParse(String,out Int32):1874, return discarded**; append result187e; pad remaining to raw+1 with parent mask1895 |
| EdgeSets, index9 | Allocate separate base/addition lists18a8/18af; nonempty split `|`:18e3 then `:`:190c; Convert.ToInt32(String) at191d/1931 reads positions0 and1 without a short-entry guard; extra colon components are not used |
| List alignment | Only if EdgeSounds local39 nonnull (1949/194b): excess EdgeSets removed with RemoveRange196f/19b4, missing positions padded with **zero**1984/19c9 until raw+1 entries |
| Slider sample payload, index10 | If present, split `:`:19f7; first two Convert.ToInt32 selectors required; optional index defaults0 at1a23, optional volume defaults0 at1a3c else parses/clamps0..100:1a47–1a54; optional filename defaults String.Empty:1a5d |
| Factory handoff | Base/addition/edge lists/index/volume/filename passed1a96–1aa4; final parent stores/copies described in A |

Counts, local numbers and offsets above are original instruction operands; semantic
list labels are reconstructed. Conversion MemberRef0a000d73 is
**System.Convert.ToInt32(String)**, not a guessed permissive parser. The edge-set
and payload overloads here supply no explicit culture. EdgeSounds TryParse likewise
uses its String/out overload. Current culture/CLR conversion behavior and native
exception text are not replaced with invariant-culture assumptions. Empty split
items are not removed by the used String.Split(Char[]) overload.

M142d:0131–0143 sets **simple=true if supplied EdgeSounds null or Count==0**.
Otherwise it installs that sounds list at0153 and allocates edge-set capacities;
M48e0 later overwrites the set-list references with the parsed lists. Therefore
EdgeSets supplied **without nonempty EdgeSounds** do not enable explicit-edge
selection. Invalid EdgeSounds text can become zero through the ignored TryParse
failure; a malformed/short EdgeSets item can throw. The parser's row catch
1cfa–1d03 continues only when parser **argument3=true**, otherwise rethrows.
Ordinary M28f5's established false validation/error-mode operand must not be
recast as universal graceful rejection. No prior row/partial-loading rollback is
proved by this catch.

Format14's head-base rule is easy to lose: an explicit edge list padded with
base0 inherits timing even when parent base is nonzero; format<14 instead falls
back to parent base for head edge0. The corresponding addition list0 becomes
the selected base rather than automatically restoring parent addition. Nonhead
edge base0 inherits timing regardless of that head-only version rule.
Other <=5 geometry/positioning and <8 spacing compatibility remain E005/E016;
this expedition adds no universal old-map admission policy. Modern finite
prepared schedules are the implementation-ready starting subset.

## F. Resources, custom filename boundary and discrete playback

The decoded strings are reproduced mathematically by unchanged E038 StaticStrings,
not by executing M49b0. Identifier, source resource/hash, key derivation,
cipher offset/length/hash and decoded text are in the evidence:

| Encrypted identifier | Confirmed decoded string / reached role |
|---|---|
|2064529152|`slidertick`, M3b19:00c6/00cb discrete tick table |
|2064529198|`sliderslide`, M3b19:00e5/00ea continuing row0 |
|2064529212|`sliderwhistle`, M3b19:0104/0109 continuing row1 |
|2064528831|`{0}{1}-slidertick{2}`, M3b36:0133/0138 dynamic tick key |
|2064528794 /2064528790|`slide` /`{0}{1}-slider{2}{3}`, M3b36:0177/017c and0183/0188 continuing key |

M3b19 prepares **F2375[1,3,3]** and **F2376[2,3,3]** alongside E037's
F2374[4,3,3]. For sets Normal1/Soft2/Drum3 and planes0/1/2, tick loading passes
M3b14(loop=false):00d0–00d3; both continuing families pass loop=true:00ef–00f2/
010e–0111. M3b18 preloads all three families for dynamic indexes>2 as well.

Discrete edges use F2374 standard resource names already proved in E038. Ticks
use `{normal,soft,drum}-slidertick` with plane/index behavior unchanged:
index0 suppresses map lookup;1 map unsuffixed;2 map suffix2; greater indexes
decimal suffix during preparation. Later mode1 resolution is dictionary-only
for indexes>2: a miss or−1 falls back to row0/bank/plane0; **0 is retained**.
Mode1 ignores the mask row. Normal1 bank0, None0/Soft2 bank1, Drum3 bank2,
other values bank0 are the original table mapping; named dynamic Enum.ToString
and culture-dependent lowercase remain E038's separate key construction.

Where these calls reach M3b14, its E038 hierarchy applies: admitted map → admitted
skin → embedded/default, with standard `.wav → .mp3 → .ogg` order, provider gates,
distinct caches and first-nonnull-resource versus loader-result distinction.
Original embedded handle0 normalization differs from map/custom0. No judgment-time
file-probing loop is introduced by M3b36. M28f5 unique timing/registered index
preloading remains distinct from per-edge timing selection; preparing a parent
sample index does not prove that a parent edge reads that override.

**Custom payload is a separate preparation effect:** parent M24b9 is nonvirtual.
It can prepare F1534 through M3b15 before the head set copy, but M1469/M146a/
M3b35 do not read F1534. Nor does M48e0 set the head's custom handle/filename.
Consequently the recovered ordinary parent discrete branches have **no custom
versus standard arm equivalent to circle M24ba**. A parent custom handle0 is
still a real stored operand; it is not converted to absent, and it does not create
a nonexistent parent custom dispatch branch.

M24b9 also has a slider-specific loader flag: if Slider2 and
**filename.IndexOf("slider")>0** and **IndexOf("tick")==−1**, it passes true
as the **loop** argument of M3b15 at0058/005b. A name starting with `slider`
at index0 does not pass the strict greater-than test. Its following two Boolean
arguments remain false. E038 custom exact/stem/cache and `.wav→.ogg→.mp3`
fallback rules apply to this reached preparation call. Do not confuse this loop
flag with M3b15's separate special-name argument.

M3b15 mutates the **shared map/custom cache**; custom and standard stems can
collide, and replacement can free an old handle. Therefore “not directly selected
by parent playback” is not a proof of no indirect resource effect. Controls with
a noncolliding filename establish that direct absence; arbitrary custom/cache
collisions/native lifetimes need their actual initial cache/provider state.
A bounded first reconstruction may explicitly exclude such collisions rather
than invent precedence or copy the parent's override into the head.

### The meaningful playback boundary

Head/edge standard layers and dedicated tick each ultimately call the same
**M3b31** recovered in E037. Handle−1 returnsnull. Other handles, including0,
construct/configure exact T02a9, set request volume/100f, clamped pan and rate
factor, add to locked F2351 and queue Action(M3bb6) through F234b.M2fee(...,2).
The exact scheduler resolves to T0670 and worker submission, not a synchronous
native play at the judgment callsite. Worker M3bb6→M140b(true,false) checks the
sample then obtains its separate effects channel with BASS_SampleGetChannel;
attribute writes and BASS_ChannelPlay follow. Native return booleans are discarded.

The discrete M3b31 request contains no hit/deadline timestamp or seek offset.
Tick/edge timestamps choose the timing sample metadata; they do not schedule
playback retroactively at a missed deadline. Native onset follows the queued
worker and its own runtime conditions.

For the common true rate-match argument, the original current song receiver and
its rate/policy can affect the factor; null receiver can interrupt. Fixed argument
1f is not a guarantee that the final factor is always1. Gain uses live E037
effect/universal integer-product settings. Parent **M1467** instead of circle
M24bd supplies pan from **ball F0bd2.F2cf6.X**, then `/512f−.5f`, times
`.800000011920929f`. This is a stored presentation position, not automatically
the newly evaluated required-position query or the scheduled edge endpoint.
Separate helper pan reads must remain distinct.

Positive judgement/request, resource availability, queued work, native call and
audible output are different boundaries. Original observer/native/scheduler
exceptions can interrupt after state stores; there is no local rollback in
M1449/M1469/M1465. A missing−1 request does not roll the audio index back.
R11's contained failures, generation cancellation and WPF voice pool remain
ENGINEERING, not newly established original policies.

## G. Continuing slider audio: verified boundaries and stopping point

This is a separate subsystem, **not another M3b31 discrete player**:

| Boundary | CONFIRMED STATIC operations |
|---|---|
| Entry/continuation | M1449 eligible range/state1 plus stored tracking true calls M146b every reached update; M1469 additionally calls it whenever its own fresh time<=parent end |
| Parent inputs | M146b starts base/addition locals0. For format>=14 reads parent F1537/F1538; older formats retain zeros. Reads parent mask, **unguarded F2f16.F36b0 LayeredHitSounds**, and parent pan*.4000000059604645f; wraps pan as Nullable<Single>, calls M3b33:0049 |
| Shared channels | M3b33 calls M3b20(base,addition,false):0011. That helper uses global current index F2360 and selected banks F2361/F2363/F2364; global channel fields **F2354/F2355**, not parent-owned T02a9 players |
| Resource choice | M3b20 resolves mode2 M3b36 for Normal1→`sliderslide` and Whistle2→`sliderwhistle`; bank0 selector inherits global bank; addition0 inherits base. It checks stored bank/index changes, can native-Stop an active old channel, then calls SampleGetChannel(sample,false) |
| Loop start policy | M3b33 schedules row0 when LayeredHitSounds OR parent mask lacks Whistle2, unless native ChannelIsActive==1. Schedules row1 when mask has Whistle2 and that channel is not state1. Finish/Clap are not extra continuing rows here |
| Native start actions | M3ba1/M3ba2 set attribute2 from M3b41()/100f and call **BASS_ChannelPlay(globalChannel,true)**; actual meaning/success remains native. M3b9c separately sets attribute3 on both current global channels using captured nullable pan |
| Gain | M3b41 combines current volume M3b42 and effect/global M3b40. M3b42 uses100 if no current song map, else Max(8,current globally selected timing volume), or map.F1000 fallback if no current point. This is not the tick/edge prepared-timestamp volume query |
| Tracking-false/cleanup | M1449:0217, M1431 and M1465 cleanup resolve M146c→M3b53, queuing M3ba8. **M3ba8 calls BASS_ChannelPause (M313d) on F2354 then F2355**, not ChannelStop or SampleFree |
| Manager guard | Ordinary M0653 calls base candidate updates first; if !F136e and M3b09 reports either native channel state1, it queues M3b53:0014. F136e is retained as an external original predicate here |

M3b20's replacement call **M3151 is BASS_ChannelStop**; cleanup M313d is
**BASS_ChannelPause**. The metadata/native import distinction is independently
checked. Queued callbacks reread global channel fields on execution, not immutable
per-slider handles. No assertion of per-slider overlap/channel ownership,
cancellation ordering, restart rewind, native frequency, perfect loop continuity
or physical pause latency follows. Missing native resources are not processed
through M3b31's managed sample-validity guard on this continuing path.

**Stopping boundary:** the current global timing/index/bank updates, worker/native
interleavings, shared-channel arbitration under overlaps, complete pause/seek/
replacement/disposal and native looping semantics are not fully reconstructed.
M3b68→M3b6a uses the current global timing index into the song map's list; it is
not the discrete M1be6(edge/tick timestamp) lookup. Completing that global timing
and continuing-channel lifecycle would be a separate bounded investigation.
The verified entry/resource/start/pan/pause boundaries above are preserved without
claiming a complete continuing audio engine.

## H. Ordering and finite reference controls

Original normal-return ordering remains E010/E011/E012/E015–E023:

`clock publication / input / queued press → optional M0ac5/M146f and its result
consumer → M28e3 candidate loop → M1449 audio/presentation update → M28e4 missing
head timeout and inclusive parent-end test → M28d3(parent)/M1465 aggregate hook →
later result event/accounting`.

Post-refresh gates can skip tracking yet rejoin before M28e3. Earlier readiness/
pause returns can prevent both. Input head sound can precede this optional
tracking pass; no physical frame count is inferred. Fresh F2358 reads occur in
head classification, each due comparison, range guards, M1469's loop guard,
M1466 midpoint and completion. Prepared timestamps supply sound sample queries;
pan and global loop volume are separate runtime reads. Do not combine them into
one immutable frame timestamp.

The following **48 RECONSTRUCTED STATIC PREDICTIONS** are not original-runtime
measurements, a synthetic gameplay run or additional independent QA checks.
Exact rows, conditions, symbolic request sequences and original sites are also
in JSON `reference_cases`. Common conditions unless varied: exact ordinary types,
valid original loading/presentation, positive N/R with valid divisors/subsets,
normal-returning helpers/observer, nonnull skin/song/resources, rate100, explicit
LayeredHitSounds=false, stable references and the stated original time operands.
Unless varied, parent raw sound/selectors are0, edge arrays are absent (supplied
entries are0 when only their length is specified), timing set/index/volume are
1/1/100, and custom cache stems do not collide. Resources and their handles/gain/
native output remain conditional; stored pan is symbolic except S35. A pre-start
M1449 has set audio index−1, except S42. Finite timing records can use set1/index1/
volume100 unless varied. No actual installed defaults are claimed.

`tick(j)` means the exact C selector at times[j]+2, mode1; `edge(e)` means D's
selector, observer and standard layers. Loop helper invocations described in G
precede discrete selection where reached even when a row abbreviates them.
E016's proposed straight-slider producer gives S2000/E3500 and
[2500,3000,3464]. E021 gives two spans/end5000 and
[2500,3000,3500,4000,4500,4964]. These prepared operands are conditional on actual
import, not measured fixture values. Artificial direct-state probes are labelled.

| ID | Required initial state / setup | Conditional request sequence |
|---|---|---|
| S01 | E016 one-span S2000 E3500 times2500/3000/3464, tracking true, audio state1; reached updates2500,3000 | tick(0,query2502), tick(1,query3002); final schedule item2 is not scanned |
| S02 | Positive embedded head raw1024/512/256 in separate fresh controls | one inherited circle hook each, before raw64 parent handoff; own start+5 timing |
| S03 | Embedded head stored raw-131072, wrapper returns-262144 | no M1884 positive sound hook; later tracking audio is independent |
| S04 | E016 gameplay success j0 at2500, tracking true, eligible M1449 | component raw8 first; later tick0; neither call is the other producer |
| S05 | E016 late entry2501 fails old deadline2500 but leaves tracking true | raw-262144 can precede tick0 at query2502 |
| S06 | Due checkpoint, follow false and pending count<N causes successful leave | later M1449 tracking false suppresses discrete request and records due index |
| S07 | Tracking dispatch skipped; retained tracking true; M1449 reached at2500 | tick0 can be requested with no component consumption on that continuation |
| S08 | E016 jump to3100 with audio index-1, tracking true | only tick1/query3002; tick0 skipped; audio index1 |
| S09 | Repeat reached3100 without another due entry | loop continuation remains reached; no new discrete request |
| S10 | E021 two spans times2500/3000/3500/4000/4500/4964; timely tracking at3500 | edge1/query3505; independent of whether component raw64 describes current item |
| S11 | E021 late entry3501; repeat component fails but tracking remains true | edge1 can still be requested by M1449 |
| S12 | E021 released at3500, tracking false by M1449 | no edge request; audio index2 consumed; reentry3501 does not replay edge1 |
| S13 | E021 jump3500 from audio-1 while tracking true | only edge1, not earlier ticks0/1 |
| S14 | E021 jump4000 from audio-1 while tracking true | only tick3/query4002, not missed repeat edge1 |
| S15 | E021 stable4964 before parent end5000, audio index already4; final component succeeds or fails | no new discrete M1449 sound; scan latest index4; gameplay128/-524288 is not an audio identity |
| S16 | Parent aggregation reached5000, q>0, edgeSounds length3 | cleanup queue then M1466(false edge2)/query5005, observer then standard layers |
| S17 | Parent aggregation q0 with head miss and no successes | cleanup reached, positive edge hook skipped; no aggregate edge request |
| S18 | Parent q>0 from successful head alone, all components pending at end | aggregate edge request still reached; pending components are not drained |
| S19 | Final component fails but aggregate q>0 | aggregate edge request can still occur despite final failure |
| S20 | Three spans, E016 finite length300/rate200/spacing100; timely tracked updates at each nonfinal deadline and completion6500 | times2500/3000/3500/4000/4500/5000/5500/6000/6464; tick0,tick1,edge1,tick3,tick4,edge2,tick6,tick7; aggregate edge3 at6500 |
| S21 | R2 edgeSounds supplied[2,4,8], parent sound0, edgeSets1:2 / 2:3 / 3:1, version14 | head stored2/base1/add2; edge1 mask4/base2/add3; aggregate edge2 mask8/base3/add1 |
| S22 | Missing or empty EdgeSounds; supplied EdgeSets otherwise valid | simple mode true; parent sound/base/add are used; edge-set lists not selected |
| S23 | R2 nonempty short EdgeSounds[4] with parent sound2 | parser pads to[4,2,2]; short EdgeSets padded with0; excess EdgeSets trimmed |
| S24 | Malformed EdgeSounds element bad, otherwise nonempty R2 list | TryParse result discarded; element0; constructor head normalizes bits2/4/8 |
| S25 | EdgeSets entry lacks colon second item, or non-Int32 selector | original index/conversion exception; row catch continues only when parser arg3 true; otherwise rethrows |
| S26 | Explicit edge mode, head edge base0, parent base3, timing base2 | version13 head chooses parent3; version14 chooses timing2; nonhead base0 inherits timing in both |
| S27 | Edge1 with timing index7 volume25, parent index9 volume90 | edge info index7 volume25; parent override fields are not read by M146a |
| S28 | Tick j0/querytimes0+2, base override0/addition3, timing base2 index7 volume0 | slidertick Soft index7 volume0; no layer expansion, observer or standard floor |
| S29 | No timing record; reached edge selector | base defaults Normal1 if zero, index0 volume100; then M44a2/observer/standard |
| S30 | No timing record; reached tick selector | unguarded timing dereference can throw; no guaranteed discrete submission |
| S31 | Timing change at tick deadline+2 versus edge boundary+5 | each query uses own prepared timestamp; full supplied-order timing rule applies; not hit timestamp |
| S32 | Parent custom filename accepted, resource returns valid handle or0 | parent custom field stored; head no filename/index/volume copy; parent discrete paths do not branch on custom handle |
| S33 | Same preparation with custom lookup-1/missing | same direct standard/tick selectors; cache collision/provider/native effects remain conditional |
| S34 | Parent custom name with IndexOf(slider)>0 and IndexOf(tick)==-1 | M24b9 passes loop=true to M3b15; starts-at0 or tick substring fails this special loop predicate |
| S35 | Artificial parent ball X0/256/512; head own X128 | parent discrete pan-.4/0/.4; loop pan those values*.4; head pan-.2; native output unobserved |
| S36 | Dynamic tick key soft-slidertick7 cached-1 versus0 versus valid | -1/miss falls back F2375[row0,bank1,plane0];0/valid retained |
| S37 | Eligible tracking update, standard/tick selected handle-1 | M3b31 returnsnull; no queued player; audio index was already advanced |
| S38 | Same with selected handle0 | player request can queue; later sample validity fails; do not equate0 with absent selection |
| S39 | Native/observer/resource/scheduler helper throws after audio index store0131 | index can remain advanced without a completed request; no local rollback |
| S40 | M1449 outside [start,end] or numeric audio state!=1 | audio index set-1; discrete and loop-start branch skipped; upstream pause may prevent whole update |
| S41 | Tracking false update consumes latest due index then reentry without new deadline | no discrete replay; loop continuation can resume independently |
| S42 | Fresh constructor or M1431 reset, no pre-start update | audio index0, not-1; direct first-index0 update does not pass latest>old gate |
| S43 | Rewind outside range then return into due range; update reached | outside writes-1 and may allow repeated due sound; not proof of full native restart behavior |
| S44 | Tracking true in eligible range before next deadline | M146b->M3b33 reached even with no discrete event; uses global loop channels |
| S45 | Version>=14 loop base/add overrides versus version<14 | newer forwards parent sets; older forwards zeros; current global bank/index and LayeredHitSounds still apply |
| S46 | Completion cleanup or inactive M1449 branch invokes M146c | M3b53 queues M3ba8 native ChannelPause on global2354/2355; later eligible call can schedule starts again |
| S47 | Ordinary manager update, F136e false and native loop state1 | M0653 queues stop after candidate updates; callback/native timing unobserved |
| S48 | Standard edge mask14 at volume100, layering augmentation disabled/enabled with other gates satisfied | disabled Finish100,Whistle85,Clap85; enabled Normal80 then same three layers; shared E037 request order |

## I. Bounded Work2 handoff and remaining limits

**Implementation-ready subset, RECONSTRUCTED:** positive embedded-head sound with
its **own** prepared circle fields; latest-prefix discrete tick/edge progression
in M1449 order; aggregate-positive edge hook separate from final component result;
dedicated tick table and exact sample queries; standard edge layering/pan/queue.
Head, tick, reversal and aggregate edge all have recovered triggers **and** sample
operands in the modern finite ordinary domain. Neither arbitrary raw score labels
nor SliderCheckpointKind alone are the original audio selector.

Minimum explicit inputs/outputs for a future bounded interface:

| Stage | Required input → output |
|---|---|
| Parse | Parent identity/raw sound, raw/effective traversal count, EdgeSounds and base/addition lists, sample payload, format/culture, shifted timestamps and error mode → original separate parent/head preparation operands |
| Prepare | Exact head construction, M146a(0) base/addition copy; final parent start/end, actual mixed schedule/order and subset F0bf7; timing samples in supplied order → independent head sample metadata, edge lists and dedicated tables/cache handles |
| Event | Positive head result **before parent raw64**; stored tracking/time/audio-state and separate audio index; aggregate ratio-positive hook before final result handoff → explicit event provenance, independent of raw component enum |
| Selection | Event-specific prepared query, parent/head sample fields, timing record, layering/global operands, actual pan source → sample info/mode/opaque handle and ordered requests |
| Discrete playback | Handle−1 versus0 versus valid, integer volume, pan, true rate-match and1f input, external rate/volume state → E037 effects-player submission; engineering sink may report unsupported/dropped/error |

Current R11 source inspection: SliderDefinition/SliderState have geometry,
movements, component states and gameplay counters, **no F0bf4-equivalent audio
progression**. Parser reads shared sample payloads for preload but retains no
EdgeSounds/EdgeSets on SliderDefinition. JudgeHead does not call the ordinary
PositiveCircleSound event. R11 resources prepare only the36 circle cells, and
its generated default pack contains twelve circle resources. These are intended
R11 scope boundaries, **not QA09 defects**. No production or development source
is changed by E039.

Reusable components: supplied-order timing selection, E038 provider/cache and
standard layer tables, mask augmentation, layer volume math, pan/rate/gain request
operands and asynchronous engineering effects sink. Required additions for a
discrete-only milestone would include **three unsuffixed tick resource keys** plus
indexed variants, tick preparation/lookup mode1, edge metadata and separate event
progression. Tick forwarding must bypass circle standard attenuation/floor and
circle custom precedence. Parent edge selection must bypass parent index/volume
overrides. A generic sample event/source identity is an engineering adapter
choice; existing CircleDefinition-only request provenance is not original slider
event identity.

**Original assets are present, but are not supplied to Work2:** metadata-only
inventory finds all nine Normal/Soft/Drum ×slidertick/sliderslide/sliderwhistle
ByteArray entries in `osu!gameplay.dll`; none of those keys in `osu!ui.dll`.
Exact sizes/hashes are in evidence; no format/audio bytes or listening are exposed.
Work2 must receive independently provided replacement assets/default-pack inputs.
No proprietary pack is extracted, manufactured or redistributed.

An initial R12 **could** support the bounded discrete subset while explicitly
omitting continuous playback. That omission is ENGINEERING: original M146b calls
still occur in the recovered order, and their errors/interleavings are not
reproduced by omitting them. Full loop resources, global timing/channel ownership,
overlap/pause/restart arbitration and native BASS behavior remain unsupported.
Restrict malformed/nonfinite geometry/counts, alternate factories/edit/reused
states, unsafe paths, unsupported short timing/providers and ambiguous custom-cache
collisions explicitly; do not invent defaults or promote these safeguards into
original parser restrictions.

Headless future checks can record event provenance/order, own-head versus parent
fields, tick+2/edge+5 queries, version13/14 difference, latest-prefix skips and
tracking-loss consumption, aggregate versus terminal outcomes, exact provider/
cache traces and pause-generation engineering policies without audio output.
Private health calibration must not become an audio event source; ledger result
codes are insufficient triggers. Existing E033–E036 arithmetic, R11 circle
behavior and862 historical checks remain independent preservation obligations.

**UNVERIFIED:** imported original map state, x86 CLR numeric edge behavior,
native handles/looping/channel stealing, actual global settings/providers,
subscriber mutations, worker ordering/races, device latency and audibility,
WPF-versus-original playback and universal gameplay fidelity. The smallest missing
empirical dependency is an owner-authorised matched original trace of these
already identified operands/callbacks; none is run or requested here. The smallest
remaining static loop dependency is G's global timing/shared-channel lifecycle.
No E040, R12 or unrelated scoring investigation begins.

# 3. Pseudocode

**RECONSTRUCTED**, independent non-executable descriptions of the CONFIRMED STATIC
sites above. Each `freshT` denotes the separate original read at that site, not a
single event/frame timestamp. Original exceptions, side effects, callback rereads,
signed unchecked integer arithmetic and Single/Double conversions remain material.

```text
ordinary factory after parent construction:
    write parent base/addition/edge-set lists/index/volume
    parent.M24b9(filename)                         // independent cache preparation
    if embedded head exists:
        info = parent.M146a(0)
        head.base = info.base; head.addition = info.addition
        // No parent custom/index/volume copy to head.

reached positive head evaluation:
    head.M351d -> M1884 classification/stored raw
    if reread stored raw > 0:
        inherited M24bb -> circle timing at unchecked(head.start+5)
        preserve shared observer/custom-or-standard request and own head pan
    preserve completion hook and result rereads
    wrapper returns positive raw or -262144
    live parent input consumer sends 64 or -262144 with parent afterward

reached ordinary M1449, after original dirty/direction/presentation prefix:
    latest = -1; i = 0
    while i < reread times.Count - 1:
        if times[i] > freshT: break
        latest = i; i++
    if freshT < parent.start OR separatelyReadT > parent.end OR F2368 != 1:
        audioIndex = -1
        return
    if NOT storedTracking:
        parent.virtual cleanup -> M146c -> queue native loop-pause action
        audioIndex = latest
        return
    M146b()                                       // even if no new discrete event
    if latest <= audioIndex: return
    audioIndex = latest                           // BEFORE helpers/dispatch
    if signedRem(unchecked(reread audioIndex+1),
                 signedDiv(reread times.Count,reread traversals)) > 0:
        M1469(true,reread audioIndex)
    else:
        M1469(false,signedDiv(unchecked(reread audioIndex+1),
            signedDiv(separatelyReread times.Count,separatelyReread traversals)))
    retain original transform work
    audioIndex = latest

M1469(tick,index):
    if freshT <= parent.end: M146b()               // separate loop read/control
    if tick:
        point = M1be6(Double(unchecked(times[index]+2)))
        base = parent.base != 0 ? parent.base : point.base
        info = new M44a1(parent.sound,base,point.index,point.volume,
                        parent.addition != 0 ? parent.addition : base)
        pan = parent.virtual M24bd -> M1467        // stored ball X
        handle = M3b36(info.mask,info.base,info.index,mode1)
        M3b31(handle,info.volume,pan,true,1f)       // NO floor/layer expansion
    else:
        info = M146a(index)
        original M28a1(info)                       // observer before fresh pan
        M3b32(info,parent.virtual pan,true)        // standard layers/floor

M146a(edge):
    if edge == 0: point = M1be6(Double(unchecked(start+5)))
    else if edge >= traversals OR edge == -1:
        point = M1be6(Double(unchecked(end+5)))
    else: point = M1be6(Double(unchecked(edgeSubset[edge-1]+5)))
    if simpleMode: mask,base,addition = parent sound/base/addition
    else if edge == 0:
        mask = sounds.Count == 0 ? parent.sound : sounds[0]
        base = bases.Count == 0 OR (format < 14 AND bases[0] == 0)
            ? parent.base : bases[0]
        addition = additions.Count == 0 ? parent.addition : additions[0]
    else:
        mask = edge >= sounds.Count ? parent.sound : sounds[edge]
        base = edge >= bases.Count ? parent.base : bases[edge]
        addition = edge >= additions.Count ? parent.addition : additions[edge]
    if point != null:
        if base == 0: base = point.base
        index,volume = point.index,point.volume
    else:
        if base == 0: base = Normal1
        index,volume = 0,100
    if addition == 0: addition = base
    return M44a2(mask,base,index,volume,addition)    // conditional Normal policy

M1465 parent aggregation:
    evaluated = true
    virtual cleanup -> queue loop-pause action
    retain E017 positive-head contribution and ratio calculation without drain
    if ordered ratio > 0:
        hookTime = freshT                         // read before start/duration
        midpoint = unchecked(reread start + signedDiv(original M3e4d(),2))
        edge = hookTime > midpoint ? reread sounds.Count-1 : 0
        M1469(false,edge)
    retain E023 grading, manager conversion/decorations and result handoff
    // Final checkpoint raw128/-524288 is NOT this sound hook.

verified continuing boundary only:
    M146b -> M3b33(parent mask,version-conditioned sets,LayeredHitSounds,pan*.4f)
    retain global-bank/index mode2 resource/channel selection
    queue original loop start/pan actions as gated by native channel state
    M146c -> M3b53 -> queued M3ba8 -> ChannelPause(global2354),ChannelPause(global2355)
    // Full global timing/channel/native pause/restart lifecycle remains UNRESOLVED.

shared discrete M3b31 boundary:
    -1 -> null; otherwise retain E037 player configuration/list/queued action
    worker validates sample, obtains separate effects channel, sets attributes,
        calls original BASS_ChannelPlay; no audible result is asserted
```
