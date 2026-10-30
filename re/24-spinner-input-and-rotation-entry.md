# 1. Result

Stable's ordinary spinner does not judge a head press like a circle. A current-object callback samples the cursor around the spinner component's **prepared drawing position**, obtains a signed angle change, and stores a target motion value. A separate, later candidate-object update smooths that motion and accumulates an absolute, π-normalized contribution. Holding a qualifying gameplay input gates the normal target-motion path, but existing smoothed motion is not erased immediately on release. These are **RECONSTRUCTED player-facing meanings of CONFIRMED original static instructions**, not observed gameplay.

## Objective, scope and publication base

Expedition 24 establishes one ordinary finite spinner's parser/factory/registration chain, current-object attention, sampled angle/wrap/held-input conditions, target and smoothed motion states, first accepted progress store and immediate progress readers. Required-spin calculation, complete judgement, bonus/result processing, scoring, HP, rendering, sound and general lifecycle are deferred. No substitute spinner, gameplay model or numerical rotation engine was created or run. Work 2 and `reconstructed/` were neither inspected nor modified. No authorised exception.

Dedicated branch `work1/expedition-24-spinner-entry` is based directly on requested E23 publication **`2d5f79648d24ec659db3dd72215ca55898a3ec92`**, tree `848a4cf9415e2fa4a7d679965d2bb34f55124244`. All112 baseline file blobs/modes matched the remote tree. The local mirror has a different commit identity but identical tree; the remote publication explicitly uses the requested parent. Preserve all111 other historical files and the exact old journal prefix.

Read the complete standing contract, cumulative contexts and relevant E01–E23 history/evidence. E05's corrected ordinary ruleset/factory, E12's post-refresh rejoin before M28e3 and E14's resource/position ordering remain authoritative. E20's packet remains untouched. **E20 remains preparation-only and incomplete: all17 original-runtime cases are NOT RUN — WINDOWS HOST REQUIRED. Every E24 runtime case is also NOT RUN.**

## Changes and reference integrity

Added this report, `re/tools/spinner_entry_evidence.py`, `re/tools/check_spinner_entry.py`, `re/evidence/24/spinner-entry.json`, independently authored `re/fixtures/24/single-spinner.osu`, and an append-only E24 entry in `re/contexts.md`. Evidence is structural metadata/site/hash extraction, not original code, binaries, assets or private dumps. Full private disassembly is not published.

| Input | SHA-256 / actual status |
|---|---|
| Original archive,18 entries,CRC valid | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| Original ZIP member `osu!.exe` | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |
| Standing contract | `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9` |
| New spinner fixture,605 bytes | `85fd97f6333df8ce6b7b039ff53103399e55f330b961ae0587a7a6f0fb13e36c` |
| Existing E01 generated silence.wav,529244 bytes | `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977` — re-generated privately for byte validation, not game playback |

Actual ZIP/EXE bytes were rehashed before analysis. Self-declared Stable40/file version1.3.3.8; official dated identity, authenticity and modification status remain **UNRESOLVED**. Linux static analysis; dnfile0.18.0/dncil1.0.2/pefile2024.8.26 were reinstalled after the initial missing-module check. The local host is Linux x86_64, with no wine/dotnet/PowerShell executable or DISPLAY/WAYLAND display; no authorized Windows endpoint was supplied. No original EXE/CLR/native input/math function was executed, and no map import, gameplay capture, differential test, instrumentation-overhead measurement or visible spinner result was obtained.

The proposed fixture has Mode0, HP0/CS5/OD5/AR5, a500ms timing point and one row `256,192,2000,8,0,5000,0:0:0:0:`. It references the existing independently generated six-second silence WAV. S=2000/E=5000 below are **conditional parser/constructor inputs**, not imported original values. A fixture file and valid WAV header do not establish game acceptance. Preserve this proposed fixture if later import requires a corrected derivative.

## Evidence and reproduction

Competing explanations: Taiko subclass versus ordinary spinner; key-edge judgement versus update sampling; mapped playfield center versus prepared drawable center; first-sample reference versus immediate angle contribution; signed versus absolute progress; wrap versus clamping; direct cursor accumulation versus target/smoothed/integrated stages; one update per physical frame versus the original repeated component loop. Original metadata, full direct-field inventories, virtual slots and bounded CIL stack/dataflow discriminate these alternatives. No alternative osu! implementation or remembered spinner formula is evidence.

Notation Mxxxx/Fxxxx/Txxxx = EXE MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx. All offsets below are hexadecimal from **first CIL opcode**, excluding headers. They are not native/JIT addresses. JSON offsets are decimal. CONFIRMED denotes original instructions/metadata; aliases and pseudocode are RECONSTRUCTED. Actual input/provider/layout/clock/JIT/helper/callback/pixel state is UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/spinner_entry_evidence.py /private/osubackup.zip > /private/spinner-a.json
python re/tools/spinner_entry_evidence.py /private/osubackup.zip > /private/spinner-b.json
cmp /private/spinner-a.json /private/spinner-b.json
cmp /private/spinner-a.json re/evidence/24/spinner-entry.json
python re/tools/check_spinner_entry.py re/evidence/24/spinner-entry.json
python re/validation/20/check_package.py --archive /private/osubackup.zip
python -m compileall -q re/tools re/validation/20
```

The read-only extractor reuses unchanged E04/E05/E15 collectors. It selects61 method entries, original owner/signature/body identities, constants and IEEE bit encodings, branch destinations, arithmetic/conversions, argument/local sites, exact ancestry/slot candidates/MethodImpl metadata, typed spinner fields and direct field/caller inventories. The full EXE scan decodes19659 bodies, skips1739 zero-RVA entries and has0 decode errors. M1872 is abstract; M3350 is a CLR delegate Invoke declaration with no CIL body. Neither is executed as disassembly. Direct inventories exclude reflection, external/native mutation and transitive callbacks.

Actual checks passed:

- Two fresh extractions were byte-identical and matched `re/evidence/24/spinner-entry.json`.
- Focused checker:61 original method entries, exact ordinary slots/input/wrap/motion/progress/order,211 historical body-hash comparisons and new fixture hash.
- All21 previous research checkers (E02–E19,E21–E23) passed on their historical evidence.
- Unchanged E20 package check passed:34 historical method entries,17 NOT RUN cases, pinned inputs/catalog/schema and rejection selftests. It performed no Windows/PowerShell or gameplay execution.
- Python compilation of `re/tools` and `re/validation/20` passed.
- Existing E01 silence was regenerated privately; actual SHA-256/header/length matched mono16-bit44100Hz/six seconds. The spinner fixture's text/hash was checked, but no import occurred.
- Historical preservation, report structure and diff checks are completed before publication; remote parent/tree/branch/every-file verification is supplied with the final publication commit.

These are extraction/consistency checks, not original-game runtime acceptance tests.

# 2. What they found

## A. Correct ordinary identity, construction and registration

**CONFIRMED correction in this current expedition, preserving E04 unchanged:** the E04 route `M4270→M1d6d` belongs to Taiko. Ordinary base Play selects T0253/M113e; its manager factory M1141 uses M0651(true) when original M2234 is false (`000d/000e`). Ordinary T00e8.M0652 overrides manager factory-provider M28bc and constructs **T09c1/M48de**. This factory's spinner override of M1872 is **M48e1 /060048e1**, which constructs **T00d8/M05ea at0009**, not M1d6d. The alternate T0680 manager and other Play/ruleset paths are not assumed identical.

Exact ordinary spinner ancestry: **T00d8 → T067f → T04c3 → T0828**. Taiko T03ea derives T00d8 but has additional overrides/writes; its behavior is outside the ordinary control. The public dispatch inventories explicitly distinguish them.

M28f7 retains original type-bit priority Normal1, Slider2, Spinner8, Hold128. For a Spinner8 row, at `1ba6` it invokes factory virtual M1872 with manager-owned start/end, sound and metadata inputs; after setting NewCombo via the original helper it passes the returned object to M28f0 at `1bbf`. M28f0 calls manager M28d2 at `0004`, registering that **same spinner reference** in full F17fb; common candidate selection shallow-copies references into F17fd. There is no new independent spinner-input registry on this route.

M48e1 forwards factory.F0e3c manager, supplied start/end and sound to M05ea, then writes F1537/F1538/F1539/F1536 and the original extra string through M24b9. Parsed x/y are not constructor inputs. M05ea calls M3201 at `001d`; M3201 sets common center **F252c/F152e=(256f,192f)**, start/end **F2523/F2524** (`002a/0031`), Spinner8 in F2525 (`0038`), old-format NewCombo4 when map.F0fd1<=8, and sound F2526. This center is distinct from the center later used for angular sampling.

M05ea initializes F02b2=45 and **F02b6=Double16.666666666666668** at `0012`. If manager.F1800 exists it calls M05f3 to construct presentation components; it always calls virtual M05f4, then conditionally M05f2. Fresh ordinary managed fields otherwise retain default values: nullable angle null, motion/progress/counters zero. No start-time activation call that clears all rotation fields was found.

M05f4 resets **Single F02b4=0 at01b3** and prepares **Double F029f** at0217 from the original duration-dependent expression:

`0.00008 + Max(0.0, ((5000.0 − Double(M3e4d())) /1000.0) /2000.0)`.

M3e4d is the established duration helper; preserve its original int arithmetic before Double conversion. For the proposed finite duration3000 this coefficient is conditionally0.00108, not a measured parameter. M05f4 also prepares a required-progress operand F02a0 and visual transforms; its required-spin calculation is deliberately not recovered here. It does **not** clear F029d, F02b0, F02af, F02b1, F029e or F02a2. Do not treat this reset as a full spinner state reset or infer their active values from a reused object.

## B. Candidate attention and actual live-input dispatch

The common F17fb→F17fd candidate mechanism is E07's coarse search/copy, not spinner visibility or an AR hit window. It can include the spinner before start. On the reached ordinary update:

| Stage | Exact boundary | Scoped consequence |
|---|---|---|
| Existing-candidate current selection | M226b:094e→M2275→List.Find(M22c5) | F136a failed latch substitutes null; otherwise first qualifying nonzero-duration object in the **old** candidate list |
| Current predicate | M22c5:000c/0019/0026 | Require stored start!=end; virtual start<=freshT; then end>=anotherT **or** !object.F1527. Exact ordinary getters use F2523/F2524 |
| Candidate refresh | M226b:0959→M28e2 | Replaces/clears candidates for later object updating; does not retroactively redo current-object Find |
| Optional live input | M226b:09be→ordinary inherited M0ac5 | If current object exists, pass **stored raw/provider pointer F2a5c** at0047 into object.virtual M24c3 at004c |
| Spinner target-motion method | Slot M24c3→T00d8.M05f6 | Returns original result enum; M0ac5 forwards it, including0, with a separately reread current-object reference to ruleset M0ad2 at005c |
| Object progress update | M226b:09cf→T00e8.M0653→M28e3 | Enumerate refreshed F17fd, call object.virtual M24b2 at0017→spinner M05f5; then manager M0654→M28e4 at001e |

E12's correction remains essential: post-refresh gates can skip optional M0ac5 but **rejoin09c9 before M28e3** on normal-returning paths. Optional nullable F13d9, recorded-input flags and prior readiness/pause/other returns can alter reachability. A current spinner absent from the old candidate list can miss angle sampling while the refreshed list still gives it M05f5 attention. No assumption of a single input sample and update per displayed frame is made.

M2295's key-press handler has Normal1 and Slider2 result branches; after the slider-bit test at0185, `018a brfalse→0215` skips them for the plain Spinner8 object. It is **not** the spinner's per-angle evaluator. Z/X and mouse inputs influence shared logical aggregates through polling; a new press edge is not required on every spinner update. Initial circle-style spatial selection/admission and width classification must not be substituted for M05f6.

M05f5 returns immediately if evaluated F1527 is true or a freshT is before start (`0001–0015`). A reached unevaluated object can get pre-start candidate attention but cannot integrate progress. Ordinary current-object selection normally prevents M05f6 before start; M05f6 itself has no evaluated guard and no immediate global start guard at entry. Its nonzero-angle normal input branch separately tests start/end, below. Artificially early-evaluated objects inside the start/end interval can therefore still pass M22c5; do not invent an evaluated test where none exists.

## C. Cursor operands and the real angular center

**CONFIRMED:** M05f6 does **not call M0ff1**. It takes the vector passed by M0ac5, subtracts **spinner.F02a3.F2b6e** through original vector operator M4efe (`0039–004a`), loads the resulting **Y first** (`004e`) and **X second** (`0056`), converts each Single to Double and calls **System.Math.Atan2(Double Y,Double X)**, MemberRef **0a0002d7**, at005c. M4efe subtracts X and Y separately using original Single fields F3109/F310a. No Y negation, input-distance test, radius comparison, M0ff1 playfield mapping or parent.F252c read occurs in this reached calculation. The spinner retains an angle reference, not its own previous-cursor-position vector, in this path.

This establishes radians via the primitive and π/2π constants. In a provider coordinate convention with positive Y downward, right→down has positive angle change; the opposite has negative change. Actual desktop/client/viewport orientation is the E13 provider dependency, not a newly observed physical direction.

**The center is presentation-prepared state, not the constructor's fixed beatmap center.** M05f3 constructs a T092e drawable via M4510 and stores it in F02a3 in either resource-layout branch (`01f7` or `03c0`). Its source position is F02b5 (`0092`), with original arithmetic:

- F02b2 starts45; layout.F09d1 true subtracts16 before this position is formed.
- X=`r4(M18fd()) / Max(1f,M1902()) /2f`.
- Y=`r4(unchecked(F02b2+219))`.
- M18ff returns `r4(host.F0e6e)/480f`; M18fd returns `conv.i4(Ceiling(Double(r4(host.F0e6d)/M18ff())))`; M1902 returns `r4(M18fd())/640f`.

M4510 stores that vector in F2cf6 at0034 and coordinate mode6 in F2b78 at0067. The constructor chain does not itself copy this into prepared F2b6e. On the ordinary mode6 draw-preparation branch, M4518 first copies current position to F2b6e at03cd; after earlier original transforms, `0639` switch entry5→068f multiplies it by current host M18ff scale (`069b/06a0`) and stores F2b6e at06a5. Keep earlier original translation/gates/other position writers, rounding later in M4518 and distinct draw/layout reads. This is a bounded center-source connection, not a new projection investigation.

A spinner input update reads whatever prepared F2b6e currently exists, potentially from an earlier draw preparation. No atomic draw/input/layout snapshot, first-draw completion or physical pixel alignment is established. The full direct inventory records general F2b6e writes in M4518 and an unrelated M1545 path, plus address-taking uses. That is not evidence M1545 runs on this spinner route. On the scoped fresh constructor route the field remains default zero until a later writer; do not silently replace it with either (256,192) or F02b5. Original draw gates can leave prior prepared state unchanged.

If manager.F1800 was null, M05f3 can be skipped and F02a3 can remain null. M05f6 has no fallback or null guard before dereferencing it once its entry gates permit. A resulting original null-reference exception is a conditional static prediction, not a runtime failure observed here. Resource selection, actual skin/layout, provider, DPI and exact prepared center remain Windows-validation inputs.

## D. Input gate, elapsed sample and first angular reference

M05f6 first returns raw0 unless **F2a90 is true** (`0002–000a`). It then selects local Double sample interval d:

- If signed F2a95<=0: fallback **16.666666666666668**.
- Otherwise `d=Double(unchecked(freshF2358 − F2a95))` (`001e–0029`).
- If ordered d<0, return before angle/state work (`0035 bge.un→0039`; finite int-derived d is the ordinary category).

F2a95 is **not the spinner's own previous sample timestamp**. Its sole direct managed writer is M43a0:07f7, a fresh F2358 read stored **after the component callback**, iff F2a90 is still true. It can track a prior qualifying global input/component pass, including passes before this spinner became current. Thus the fallback cannot be assumed for its first sample.

M43a0 invokes clock helper M3b25 at00ca, initially copies host timing-tail flag **F2c30→F2a90** at00d4, and later can call recorded-input helper M43a1. M43a1 has conditional writes clearing F2a90 at0046 or setting it true at02de/08d0; its consumed-record route also writes F2358. We stop at those bounded override boundaries; no full replay decoder is recovered. Base live/no recorded override retains the copied timing flag.

F2c30's direct writers are host timing-tail M46a1:0191(true) and019f(false). Relevant finite branch: host-current minus host marker is compared with Double16.666666666666668 at00e9/00f2; when below that threshold and F2c3c is false, branch00f9 skips to019e(false). Other host/current-spinner/recorded-input/focus branches alter marker handling before true is stored. This is an original nominal timing gate, **not proof of exact60Hz physical input sampling**. Host timing refresh occurs after update/draw in the established E10 callback; input uses prior published host state.

M05f6 computes θ, then checks **Nullable<Double> F029d**. With no value it calls original compatibility predicate **M05f7 at0070**:

- true: store Nullable(0.0) at0086;
- false: store Nullable(θ) at0094.

M05f7 is concrete original compatibility code: source F2a7f's optional DateTime F196f earlier than **2019-05-10**, or its F1988 signed less than **20190510**, yields true under its exact null/short-circuit branches. Null source yields false. No source DateTime or F1988 integer value is asserted for an actual session; the integer field is not assigned an original descriptive name here. For a finite nonlegacy first sample, θ−θ=0; for the compatibility branch, a first nonzero θ can yield motion relative to0. There is no general promise that the first cursor observation always contributes zero.

## E. Signed delta, wrap, hold/time gate and target motion

For a present reference p, M05f6 first forms `δ=θ−p`. Original nullable arithmetic is preserved, including separate value/HasValue reads. For stable finite state:

| Original branch | Operation |
|---|---|
| `(θ−p)<−π` at00d8–00f6 | δ=`2π+θ−p` (`00f8–010f`) |
| Else `(p−θ)<−π` at0143–0161 | δ=`−2π−p+θ` (`0163–017a`) |
| Otherwise | Keep θ−p |

Both wrap tests are **strict**. Exact antipodal ±π is not wrapped and is rejected by a later `Abs(δ)>=π` comparison. This is wrap adjustment, not a maximum accepted cursor-pixel distance. A large screen displacement can yield a small accepted angle, while a tiny near-center displacement can yield a large angle. No radial dead zone or distance-based amplification correction is present in this scoped method.

Next it updates **Double F02b6**, an interval EMA:

`w=Pow(0.999,d); F02b6 = w*oldF02b6 + (1.0−w)*d` (`017b–01a5`).

This occurs before zero-motion/held-input classification. Then:

1. **Exact δ==0 branch precedes all held/time checks.** It increments Int32 F02b1 unchecked at01c4 using saved old count. If oldCount>=1, target **F02b0=0**; otherwise F02b0=`oldF02b0/3.0` (`01e9`). Thus first stationary sample can decay target; the second consecutive one zeros it. It does not consult current held input on this branch.
2. For nonzero/unordered δ, set F02b1=0 (`01f5`). Unless original Relax-derived F137b is true, require logical aggregate **F2a67!=0 OR F2a83!=0**, freshT>=start and **another freshT<=end** (`0201–0227`). Failure sets local δ=0 but does not return; it proceeds through the target calculation. These aggregates combine configured keyboard and mouse paths (E11), not just a physical mouse button or a new press edge. Slider F0a7a/history/remembered-side logic is not read here.
3. Require **ordered Abs(δ)<Doubleπ** (`0233–0242`). Equality, larger magnitude or NaN sets target F02b0=0 at02c3.
4. For an accepted δ, compare original `M28ae(F02b6,managerMods)` with **17.333332697550457** (`0244–0268`). If greater, obtain v=`M28ae(d, fresh managerMods)` and store target `v>0 ? δ/v :0` (`029f`). Otherwise store **δ/16.666666666666668** (`02b2`). Both branches retain the sign of finite δ. No fixed per-frame angle or arbitrary rounding is substituted.
5. Store current unwrapped θ into F029d **at02cf**, even after hold/time/magnitude rejection. The EMA and previous angle are not “unchanged rejected input.”

M28ae is the original Double rate helper: bit64 divides its first argument by1.5; else bit256 divides by0.75; otherwise returns it. Original metadata names these DoubleTime and HalfTime. The bit64 branch takes priority if both are artificially present. This is a bounded timing influence, not a full mod survey. Multiple manager/mod reads are separate and can differ under mutation.

No explicit direction-change reset occurs here. Signed target can reverse; the later smoothed state need not reverse instantly. A release-only event does not call a spinner reset. Movement without qualifying input still updates angular reference/EMA, and in the nonzero branch sets the normal target to zero. With no movement, the stationary decay rule is reached first. If F2a90 is false, none of these sample-state changes occurs.

## F. First accepted progress: the separate object update

The first **target-motion** store is F02b0 in M05f6. The first **accepted smoothed motion** stores are Double F02af in M05f5. The first **accumulated angular progress** store is Single **F02b4 at M05f5:0395**. Do not merge them into one rotation count.

After its evaluated/start guards, M05f5 updates an RPM-like observer F02a1 from the **previous** F02af and current F2a94, with optional presentation/callback work. These helpers can interrupt or mutate state. For ordinary application Play2, the actual integration block requires all of:

- freshT **< end**, branch014e rejects equality or later;
- another freshT **> start**, branch015e rejects equality or earlier;
- F1376 false;
- signed **F2a94>0**, branch0173 rejects zero/negative.

Edit1 has a separate F02b4 assignment at0139, not the ordinary live integration formula. Early evaluated/start returns also bypass later presentation work. M05f6's held-inclusive start/end checks must not be substituted for these strict integration boundaries.

**F2a94 is a different interval from M05f6's d.** M43a0:0167–0182 computes it as unchecked signed Int32 freshF2358 minus global F2a96, then updates F2a96 using separately reread old state plus interval. Static initializer M438f sets F2a94/F2a96 to0. It is computed each reached input/component pass, not only when F2a90 is true. M05f5 reads it afresh at multiple sites; do not freeze it into the same sample interval used by M05f6 or assume one atomic clock snapshot.

For finite, stable state define target W=F02b0 and smoothed U=F02af. Original acceleration allowance is `a=M28ae(F029f*Double(F2a94),managerMods)` (`0178–019a`). Plain control excludes the reached automatic-motion alternatives: Play bit4096 or F137c can instead set U=0.03 at01d7. Otherwise:

- If W>U: add `Min(W−U,a)` to U; when U<0 and F137b true use a/4 instead of a (`01e1–022f`).
- Else: add `Max(W−U,−a)`; when U>0 and F137b true use −a/4 (`0236–0278`).
- Clamp finite U as **Max(−0.05,Min(U,0.05))** (`027d–02a0`).
- Compute **step=Single(U*Double(fresh F2a94))** at02b2.

There is no direct held-input read in this object-update integration block. If target was not resampled, or U has not yet reached0, progress can continue conditionally after release. It is equally possible for U to reach0 immediately when the allowance is sufficient; inertia is not a universal extra-time constant. F1376/readiness/current-object gates remain distinct.

Before progress is credited, original presentation state consumes step: F02a3/F02a7 rotation-like F2cf3 writes, an original factor of1 or0.5 from M451d, rate-helper calls, and a secondary component division by3 can occur. Then nonzero U calls **M05fb at0364**; zero U invokes virtual M24c4→M05fc at036c. M05fb invokes original M3b34 after optional ratio construction; M05fc calls M3b54. These are **opaque callback/presentation/audio-adjacent boundaries**, not assumed pure or successful. No sound identity or visible effect is asserted. An exception there can leave U changed but prevent the subsequent progress store.

If those original operations return, the exact accepted-progress expression is:

`F02b4 = Single(oldF02b4 + Min(1f, Abs(Single(Double(step)/π))))`

at0371–0395: Single1 at0378; step→Double037e; Doubleπ at037f; div0388; conv.r4 at0389; SingleAbs at038a; SingleMin at038f; add0394; stfld0395. Original CIL/JIT intermediate precision and final Single store remain significant. Finite clockwise/counterclockwise steps have equal absolute contribution, but preceding signed smoothing/history can differ. π-normalized units are **RECONSTRUCTED** from this formula; this is not a recovered count of full2π revolutions. One reached integration adds at most1 for finite state; no time-jump catch-up loop is inserted.

### Immediate readers and stopping boundary

Immediately after the progress store, M05f5 reads Abs(F02b4), divides by prepared F02a0, multiplies by100f and calls M05fd at03b4. This is a presentation-progress consumer, not proof of visible pixels or a required-spins formula. Later phase/effect branches are outside scope.

Another M05f6 input pass calls **M05eb** at02d5. M05eb reads F02b4 and `conv.i4` at0006 (truncation for finite in-range values). If that integer equals cached F029e, return0 at02e3. If different, M05f6 increments **F02a2 by1** at02ed, then enters the deferred spinner result/bonus-related continuation; at04c2 it reads M05eb again and stores F029e at04c7. This is an observed-integer-change counter, not necessarily the integer progress itself or a full-revolution counter. Separate reads/hooks can differ; no direct equality invariant across them is invented.

Thus a first small accepted fractional contribution can occur with no immediate nonzero enum: M05f6 normally returned0 **before** this callback's later M05f5 progress addition. Crossing an integer is noticed on a later reached input call. The exact later result classification/required-spin/bonus continuation is the stopping boundary for the next expedition, not reconstructed here.

## G. Field identities, initialization and complete direct inventories

All fields below belong to **T00d8/020000d8**. Original names are obfuscated; semantic aliases are RECONSTRUCTED. `0615118801010d` is Nullable<Double>, `060d` Double, `060c` Single and `0608` Int32. Complete watched-field access inventories with original owning types/signatures are in `entry_field_sites`/`field_access_owners`. No direct reference/address inventory proves absence of external or reflective writes.

| Field / token | Signature | RECONSTRUCTED alias | Complete direct stores | Complete direct reads/addresses |
|---|---|---|---|---|
| F029d / `0400029d` | `0615118801010d` | Nullable previous angle | M05f6:0086/0094/02cf | M05f6:0063/009b/00ab/0104/0113/016d |
| F02b6 / `040002b6` | `060d` | Sample-interval EMA | M05ea:0012; M05f6:01a5 | M05f6:0190/0245 |
| F02b1 / `040002b1` | `0608` | Stationary sample counter | M05f6:01c4/01f5 | M05f6:01b9 |
| F02b0 / `040002b0` | `060d` | Signed target motion | M05f6:01e9/029f/02b2/02c3; M1d78:011f/027c | M05f5:01e2/01f7/023e; M05f6:01da; M1d78:0110/0263 |
| F02af / `040002af` | `060d` | Signed smoothed motion | M05f5:01d7/022f/0278/02a0; M1d75:0011 | M05f5:004a/01e8/01f1/01fd/0204/0238/0244/024b/0288/02a6/0353 |
| F02b4 / `040002b4` | `060c` | Single accepted-step progress P | M05f4:01b3; M05f5:0139/0395 | M05eb:0001; M05f5:0373/039c |
| F029e / `0400029e` | `0608` | Cached integer progress | M05f6:04c7 | M05f6:02db |
| F02a2 / `040002a2` | `0608` | Observed integer-change count | M05f6:02ed | M05f5:03c9/03fa; M05f6:02e6/02ff/0313/0326/039c/0497/04a0/04b2; M05fa:002e/0048/005c/008c/00a6/00ba; M05fb:0018 |

The table includes nonordinary Taiko writers where present, preserving actual coverage instead of claiming spinner subclass equivalence. For F02b4 specifically, complete direct managed writers are M05f4:01b3 reset, M05f5:0139 Edit-specific assignment and M05f5:0395 ordinary accumulation. For Nullable F029d there are only the three M05f6 stores0086/0094/02cf; no direct managed null-clear store was found. Constructor defaults do not establish a later reused object's state.

## H. Managed ordering, repeated passes and completion boundary

The existing E10 host chain is retained: registered M4241→M469e→M43a0(component delegate M46d7), clock work/polling/queued callbacks, M3350 delegate Invoke→M46d7→M46fb→component virtual M1ab2→ordinary Play M226b; draw follows update, host timing-tail follows draw. M3350 is a CLR delegate method declaration, not an undiscovered managed body. Callback scheduling and actual thread cadence are unobserved.

Within each normally returning Play pass: current-object selection uses old candidates, candidate refresh happens, optional M0ac5/M05f6 samples/derives target and forwards its enum, then M28e3/M05f5 integrates on refreshed candidates before manager M28e4. This can happen in one component callback, but its two original methods use distinct clock/interval/reference reads. Optional input can be skipped while object integration still runs; queued press processing does not own that integration.

**Multiple progress updates within one host callback are statically permitted.** M43a0 loops back to00ca at0820 under its original final predicates; its old local loop count is compared with30 after increment (`0819–0820`), allowing up to31 passes including the first. Each can call the component delegate again. No once-per-physical-frame or once-per-distinct-F2358 contract exists. M05f5 has no distinct-time cache check: if a repeated invocation satisfies its guards and reads a positive interval, it can accumulate again. Conversely actual repeated clock reads can yield zero/negative F2a94, suppressing integration. Do not assume a fixed number of credited updates.

A skipped angle sample can leave previous θ and target stale; the next reached sample computes one wrap-adjusted difference, not all missed physical rotations. A skipped progress update contributes nothing at that call; a later larger positive interval can increase step but finite contribution is capped at1. Original provider/record changes or jumps can change these inputs; no guessed recovery scheduler is added.

At **exact S=2000**, current selection can admit the spinner and held angle processing permits the time equality, but ordinary progress integration rejects T<=S. At **exact E=5000**, current selection and held angle processing can again pass equality, but integration rejects T>=E. Spinner-specific manager M28e4:008f uses `bge.s→00d6` on end versus freshT, so its completion call is skipped at equality and can be reached only when freshT>end and other original gates permit. This differs from the slider's inclusive end condition. Stop at that completion-call boundary; no spinner aggregate/result algorithm is inferred.

Evaluated M05f5 returns before integration. Once ordinary manager completion has evaluated a spinner after end, subsequent old-candidate current selection normally rejects it through the late !evaluated predicate. An artificial early evaluation inside the interval is not filtered in M22c5 or M05f6, even though M05f5 will stop. F136a, pause/readiness, recorded-input, nullable-time, missing candidate, exception and callback changes remain distinct constraints. These managed orders imply no physical input latency, presentation timing, stutter cause or optimization benefit.

## Representative original body identities

All selected body hashes/RVAs/signatures are in the evidence; representative SHA-256 values include original header bytes:

| Method | Body SHA-256 |
|---|---|
| M48e1 | `d3a15dbe56988db550003b12eb04ef798ed674301bf0a8b098bc0b9580e0a046` |
| M05ea | `8ea03fc302a959658eab912c69dfc9df40803f3a68d9ac04cc56caf012dda339` |
| M05f4 | `038431582ce331e8b88c098d8d942c52df08af4d75ff2b0d0b4314e924c4debc` |
| M05f5 | `226b6593ed5ac1383d9ef22f2c416f13070173cfaf4750cac04f0f832493ba57` |
| M05f6 | `8ef889e9b9e90843e13f78e5e23353157e3d2098dce9e3ded7e56babda460efa` |
| M05f7 | `238e085c0920c825f8a2adb2dd293ff886f0576f95e16fe84d899634af78682a` |
| M05eb | `2d5d1f111e60f15222ce634620027b6daaf27996ddd6827c87211d406bfc73d1` |
| M0ac5 | `1166e408b2fbaf918fb5d944be50abda75fe7efcca2e4520bf72f94b849c42b4` |

## I. Conditional reference predictions

All cases assume normal ordinary Play2, one finite constructed spinner, unchanged pointer/layout references, finite fields, normally returning opaque helpers and plain mod flags unless explicitly varied. The fixture's imported values and actual prepared center are unverified. Angle values below denote **actual original operands if obtained**, not injected natural gameplay or an executed atan2 substitute. Cases labelled artificial require separate probes. Every runtime row is **NOT RUN**.

| ID | Controlled condition | Original branch-derived prediction | Runtime |
|---|---|---|---|
| S01 | T<S, candidate contains spinner | M05f5 returns; ordinary M22c5 rejects current spinner | NOT RUN |
| S02 | T=S=2000, old candidates include spinner | Current selection may succeed; M05f6 can initialize/sample; strict progress gate skips | NOT RUN |
| S03 | S<T<E, first finite nonlegacy angle, nullable absent, fresh W/U0 | Previous becomesθ; δ0; target0; no positive integration contribution | NOT RUN |
| S04 | Same but M05f7 compatibility=true and θ!=0 | Initial reference0; first delta may produce target if hold/time/magnitude checks pass | NOT RUN |
| S05 | Second qualifying sample, same θ, old stationary count0 and targetW!=0 | Old-count branch storesW/3; updates previous angle | NOT RUN |
| S06 | Further stationary sample, old count>=1 | Target0; smoothed U may still approach0 later | NOT RUN |
| S07 | Small positive angle change, qualifying aggregate, ordered Abs(δ)<π | Positive target through original fixed/adaptive divisor; finite positive/absolute progress if later gates/helpers permit | NOT RUN |
| S08 | Matched negative change with symmetric fresh state | Negative target/U/step, same absolute contribution; no sign-reset of P | NOT RUN |
| S09 | θ−p<−π | Add2π to theta difference before magnitude test | NOT RUN |
| S10 | p−θ<−π | Subtract2π from theta difference before magnitude test | NOT RUN |
| S11 | Abs(δ)==π at original comparison | Target0; previous and EMA still update; existing U may persist | NOT RUN |
| S12 | Large pixel movement with final normalized Abs(δ)<π | Pixel magnitude alone does not reject it; other original gates still apply | NOT RUN |
| S13 | Pointer exactly equals actual F02a3.F2b6e | Calls original Atan2 with zero components; signed-zero/CLR result not measured; no radial fallback | NOT RUN |
| S14 | Pointer near center, finite nonzero components | Uses angle with no minimum-radius gate; tiny displacement can change angle substantially | NOT RUN |
| S15 | Far pointer with same radial direction | Equal original θ operands imply zero angular change regardless of radius | NOT RUN |
| S16 | Nonzero δ, F2a67 or F2a83 nonzero, T inside | Normal target can be nonzero; no new press edge required | NOT RUN |
| S17 | Same nonzero δ, both aggregates0, F137bfalse | Localδ set0; target calculated as0; angular reference/EMA change | NOT RUN |
| S18 | Fresh U/W0, cursor movement without held aggregate | No initial accepted positive progress under plain normal-returning integration | NOT RUN |
| S19 | Release after established U!=0 with no movement | Stationary target decay occurs before hold checks; later U can continue until limited convergence reaches0 | NOT RUN |
| S20 | Press/release with unchanged cursor and target/U already0 | No new angular delta or positive accepted contribution | NOT RUN |
| S21 | Quick direction reversal, target and U have opposite signs | Target sign can change immediately; U converges under original allowance, potentially retaining old sign temporarily | NOT RUN |
| S22 | F2a90false, positive F2a94, prior target/U!=0 | M05f6 returns0 without sampling; independently reached M05f5 may still integrate stored motion | NOT RUN |
| S23 | d<0 after a clock discontinuity with prior F2a95>0 | M05f6 returns before angle/EMA/previous writes; object update has its own interval/time decisions | NOT RUN |
| S24 | d0 but F2a94<=0 | Input can still derive target using fixed divisor branch; ordinary progress integration skipped by signed interval gate | NOT RUN |
| S25 | Large positive F2a94 and allowed interior state | Smoothed U clamped±0.05; each finite integration contribution capped1; no missed-rotation reconstruction | NOT RUN |
| S26 | At T=E=5000 | Held input/sample time gate includes equality; progress and manager completion both skip at their separate equality tests | NOT RUN |
| S27 | T>E, still unevaluated/current from old candidates | Nonzero-angle plain hold branch zeroes localδ; no normal integration; later manager completion may be called under its gates | NOT RUN |
| S28 | Already evaluated ordinary spinner | M05f5 returns; late current predicate excludes it after end. Artificial early-evaluated state can still receive M05f6 | NOT RUN |
| S29 | No nullable previous reference, no source record | M05f7 false; initialize from currentθ, not a guessed zero reference | NOT RUN |
| S30 | Recorded source F196f/F1988 enters original compatibility branch | First-angle semantics can differ; recorded helper may also override gate/clock/cursor; actual source values unresolved | NOT RUN |
| S31 | Zero-duration proposed/injected spinner | Current predicate rejects equal stored start/end; strict finite progress interval empty; other reset/presentation exceptions unresolved | NOT RUN |
| S32 | Old candidate set omits spinner, refreshed set includes it | Angle callback may lack spinner while candidate update reaches M05f5; no retroactive current selection | NOT RUN |
| S33 | Readiness/pause returns before current/candidate work | Neither reached spinner path is guaranteed; no synthetic reset inferred | NOT RUN |
| S34 | Optional M0ac5 skipped but M28e3 reached | Stored target/U can be consumed independently; E12 rejoin preserved | NOT RUN |
| S35 | Exception after U store but before0395 progress write | U mutation retained, progress store not reached; no rollback or successful credit invented | NOT RUN |
| S36 | F02a3 absent and entry/elapsed gates pass | Original null dereference before angle work; no fallback to parent center | NOT RUN |
| S37 | First finite fractional P change with truncated integer still equal F029e | Later M05f6 returns0 at02e3; progress exists independently of result notification | NOT RUN |
| S38 | Truncated P differs from cached integer on a later input call | IncrementF02a2 once then defer result continuation; do not equate count with every crossed integer | NOT RUN |
| S39 | M43a0 inner-loop predicates permit repeated component passes | Multiple M05f5 opportunities in same host callback; actual T/interval/helper/gates decide each credit | NOT RUN |
| S40 | Prepared center changes between draw and next input | Delta is measured around actual then-current F2b6e; no immutable center or input-to-pixel equivalence assumed | NOT RUN |
| S41 | Artificial NaN θ/delta from invalid pointer/center | bge.un magnitude rejection sets target0; prior/EMA mutations and later smoothing remain separate, target CLR behavior unobserved | NOT RUN |
| S42 | Automatic F137c or Play bit4096 branch deliberately enabled | Original separate U=0.03 path can bypass cursor target; excluded from normal input control | NOT RUN |

## J. Minimal E20 addition and remaining boundaries

Do not recreate E20's packet. On an authorized isolated Windows host, reuse its exact-original/provenance requirements and add this independently hashed spinner map with regenerated/pinned E01 silence in a fresh private import attempt. Verify the **actual** imported spinner type/identity, S/E/type/manager and F02a3 identity; copying files is not successful import evidence. Stop on import failure and preserve a separately hashed correction if needed.

Use CLR-capable low-overhead observation at original consumption/store sites, bound tokens/CIL offsets to actual loaded-module/JIT locations. Record thread/sequence/high-resolution host timestamp and:

- Actual provider identity, F2a5c, prepared center F02a3.F2b6e, its layout/source position and last M4518 preparation, held aggregates F2a67/F2a83, F2a90 and any recorded override.
- Each actually consumed F2358/F2a95/d and F2a94/F2a96; actual source-record compatibility operands and M05f7 outcome; original θ,p,δ and wrap/magnitude/held/time branches.
- EMA F02b6 before/after, stationary counter F02b1, target F02b0, smoothed F02af, acceleration/mod operands, step and F02b4 before/after0395; original helper/exception boundaries.
- Current/candidate identities and update-order sequence, M05eb integer/cache/count on a later pass, enum0 or reached result boundary without extrapolating bonus/final judgement.

Natural controls: stationary cursor; held clockwise and counterclockwise motion; release with motion; release without motion; depart/return across wrap; center/near-center if safely measurable. Do not claim exactπ/equality naturally occurred unless original operands prove it. Keep debugger-forced center, prior angle, clock, null, NaN and equality probes separate. Preserve original EXE/map/audio/settings/environment hashes and actual import/process/module identity; keep private captures/logs private and hash-linked. Never call clock/provider/math/game methods just to manufacture log values. Measure tracer overhead with matched uninstrumented runs and repeated low-overhead runs; debugger stepping can change sample cadence and must be labelled.

**No original runtime values, screen pixels, physical latency, audio behavior or overhead measurements exist from this expedition.** E20's17 cases remain unexecuted. Completion/required spins/bonus/formal outcomes remain outside scope, and no spinner rendering equivalence or performance claim follows. Distorted-slider stutter remains a separate unmeasured objective.

Smallest next spinner investigation: follow **F02b4 integer-change detection → F02a2 bookkeeping → the first nonzero ordinary spinner component result and immediate ruleset handoff**, keeping required-spin/bonus/final completion branches bounded rather than reconstructing full scoring. Original-Windows validation remains the necessary verification dependency.

# 3. Pseudocode

**RECONSTRUCTED — independent original dataflow, not executable spinner code.** Separate field reads, numeric categories, opaque helpers, exceptions and variant gates are retained; finite stable-state aliases abbreviate them only where stated.

```text
corrected ordinary loader:
    T0253 manager factory -> M0651(true) -> M0652 -> M48de
    parser Spinner8 branch -> virtual M1872 -> M48e1
    spinner = new original T00d8/M05ea(manager,start,end,sound)
        M3201 stores center(256f,192f), start/end, Spinner8 and original flags
        default nullable angle=null; motion/progress/counters=0
        intervalEMA = Double(16.666666666666668)
        if presentation exists: original M05f3 constructs F02a3
        original M05f4 resets P=0 and prepares original coefficient/dependencies
    retain metadata/registration through M28f0/M28d2

reached ordinary Play pass:
    original current object Find from existing candidates
    original candidate refresh
    if original optional input gates permit and current object exists:
        result = current.virtual M24c3(stored provider pointer F2a5c)
                 // ordinary spinner resolves M05f6 below
        original ruleset result dispatch(result, separately reread current object)
    rejoin original M28e3 on normal-returning path
    for each refreshed candidate:
        candidate.virtual M24b2()                // spinner M05f5 below
        original manager M28e4(candidate)         // completion boundary deferred
```

**RECONSTRUCTED — M05f6 sample/target prefix, before deferred result logic:**

```text
if not Read(F2a90): return raw0
d = lastQualifyingInputTime>0
      ? Double(unchecked(freshT - Read(F2a95)))
      : 16.666666666666668
if ordered d<0: return raw0
v = original Single vector subtraction(pointer, Read(F02a3).F2b6e)
theta = original Math.Atan2(Double(v.Y),Double(v.X))  // no Y inversion
if not previousAngle.HasValue:
    previousAngle = original M05f7() ? Nullable(0.0) : Nullable(theta)
delta = theta - previousAngle.Value
if original nullable (theta-previousAngle)<-pi:
    delta = 2*pi + theta - previousAngle.Value
else if original nullable (previousAngle-theta)<-pi:
    delta = -2*pi - previousAngle.Value + theta

w = original Math.Pow(0.999,d)
intervalEMA = w*oldIntervalEMA + (1-w)*d
if delta == 0 [original ordered equality]:
    oldCount = stationaryCount
    stationaryCount = unchecked(oldCount+1)
    target = oldCount>=1 ? 0.0 : oldTarget/3.0
else:
    stationaryCount = 0
    if not Read(F137b):
        if not (Read(F2a67)!=0 or Read(F2a83)!=0)
           or freshT<start or anotherFreshT>end:
            delta = 0.0
    if not ordered Abs(delta)<pi:
        target = 0.0
    else if original M28ae(intervalEMA,managerMods)>17.333332697550457:
        divisor = original M28ae(d,freshManagerMods)
        target = divisor>0 ? delta/divisor : 0.0
    else:
        target = delta/16.666666666666668
previousAngle = Nullable(theta)

if original M05eb(P) == cachedIntegerProgress: return raw0
observedIntegerChangeCount = unchecked(observedIntegerChangeCount+1)
DEFER original component/bonus/result continuation and later cache write
```

**RECONSTRUCTED — M05f5 ordinary first-progress block, finite stable-state projection:**

```text
if evaluated or freshT<start: return
retain original observer/presentation operations using previous U
retain separate Edit branch (not ordinary Play formula)
if freshT>=end or anotherFreshT<=start or Read(F1376) or Read(F2a94)<=0:
    skip integration; retain original later presentation consumers
else:
    a = original M28ae(coefficient * Double(Read(F2a94)),managerMods)
    if original automatic-motion branch:
        U = 0.03
    else if target>U:
        limit = (U<0 and Read(F137b)) ? a/4 : a
        U += original Math.Min(target-U,limit)
    else:
        limit = (U>0 and Read(F137b)) ? -a/4 : -a
        U += original Math.Max(target-U,limit)
    U = original Math.Max(-0.05,original Math.Min(U,0.05))
    step = Single(U * Double(freshRead(F2a94)))
    retain original component rotation writes and helper calls
    if U != 0: original M05fb()
    else: original virtual M24c4() -> M05fc()
    // Exception here can prevent progress while leaving earlier mutations.
    P = Single(P + original SingleMin(1f,SingleAbs(Single(Double(step)/pi))))
    // F02b4 store at0395
retain original P/requiredOperand presentation consumer M05fd
DEFER later phase/effects/completion and full result classification
```

**UNRESOLVED:** actual original import, runtime field values, source compatibility state, provider/layout/prepared center, target-CLR primitive/precision behavior, helper side effects, callback cadence, original progress/results/captures and tracing overhead. No runtime acceptance criterion is satisfied by the static pseudocode or fixture.
