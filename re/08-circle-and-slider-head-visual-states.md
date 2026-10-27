# 1. Result

Expedition 08 — Circle & Slider-Head Visual State Timing. 2026-10-27 (Asia/Bangkok). Work 1, original-software investigation only.

## Objective and scope

Recover one coherent ordinary osu!standard circle/linear-slider-head path from construction, through time-dependent visual properties, to an original draw-stage consumer. This is research for a future visible, playable product, not a replacement visual system. Work 2 remains suspended. No implementation, optimization, replacement animation model or changes under `reconstructed/`; Work 2 was not inspected.

Read the complete contract, cumulative contexts, Expeditions 01–07 and relevant evidence. Preserve Expedition 05's correction: ordinary M113e → M0651 → M48de; circle M48df → M45eb/M45ec; slider M48e0 → M142d. The superseded Taiko interpretation in E04 is not reused. This expedition examines the ordinary base Play/manager branch, non-Edit, initially untouched objects; alternate Play types, hit-driven effects, skin resolution, complete clipping/coordinate logic and backend rendering are outside its recovered projection.

Requested remote parent `10b99e0c8de166d6aae1c70a3ac2214b9b5798b0` has tree `2246ab346cd8db51a6c74fc9e68c056eb1279fe4`, identical to the local E07 mirror baseline. Dedicated remote branch `work1/expedition-08-circle-visuals` starts directly from that parent. Historical records remain intact, and `contexts.md` receives only an append. No authorised exception.

## Reference and evidence

Reference archive SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; `osu!.exe` SHA-256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`; contract SHA-256 `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9`. Same self-declared Stable40/file-version 1.3.3.8 baseline. Exact official release date/authenticity/modification status remain UNRESOLVED. Linux/Python static inspection; **no original executable, CLR method, Windows gameplay, capture or frame-time measurement was run**.

Created this report, `tools/visual_state_evidence.py`, `tools/check_visual_states.py`, `evidence/08/visual-states.json`, and the journal append. The extractor reuses unchanged E04/E05 tools. It publishes metadata, signatures, original method-body hashes, token sites, constants, normalized branches, arithmetic/conversions, ancestry declarations, virtual-slot candidates, registration MethodSpec resolution and syntactic direct callers. It does not publish original binaries, source, assets or full disassembly. Private original instruction inspection supplies operand/stack/branch interpretation; the independent collector pins reproducible sites without implementing the behavior.

Notation: Mxxxx = MethodDef `0x0600xxxx`; Fxxxx = Field `0x0400xxxx`; Txxxx = TypeDef `0x0200xxxx`. All offsets below are hexadecimal relative to the **first CIL opcode**, headers excluded. JSON retains exact RVAs, file offsets, signatures and hashes. `CONFIRMED` means confirmed original static instructions/metadata, not observed gameplay. Conceptual component names and pseudocode are `RECONSTRUCTED` unless explicitly described as metadata names.

| Method | RVA | Code bytes | Method-body SHA-256 |
|---|---|---:|---|
| M45ec | 1bc984 | 1288 | f6f216fd91045b41db5c0946bf74056978b050983c88d12d9c36200827ff8541 |
| M143a | 7abdc | 95 | efb86763cd228dd2e3b988a249d7e362d17a2b2a8ff7bde18f4e09bcda512da3 |
| M144a | 7b43c | 5632 | d58260ebed39834edc228b1b21995da8481e87bbc342223db2b812c7c9b7ceef |
| M2cf0 | 128948 | 1379 | f7397d0d0b28222c118d160c9f0f8dd20313626eb81b5e5ecd05791fb1ed9447 |
| M4519 | 1b7e54 | 3292 | af8b04deaab9b07d21ea4050c713ba3989988e4684323fb56f4681d1fcac5892 |
| M4518 | 1b6ef8 | 3920 | 993a728c3a8989fb22fdeaf14ce0428543df2d1580bfb77f975857fcd6a35679 |
| M451a | 1b8b58 | 412 | de7f1bda1890017eb2936bcf330a7c2d1f0ff5722c3cc9b16be3c03bac9b5416 |

Competing explanations were: candidate eligibility directly turns a circle visible; all visual preparation waits for M24b5; circle/head have independent timing engines; alpha is a direct function of current candidate membership; transforms alone guarantee pixels. Discrimination: follow exact constructor identities and list references, resolve the circle/head inheritance chain, trace registration/staging/queue admission and draw-stage virtual dispatch, then inspect Fade/Scale writes and their consumers. The instructions reject each simple conflation. No modern formula, alternative engine or public documentation is used as proof.

## Reproduction and actual validation

From repository root, with a private copy of the hashed archive:

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/visual_state_evidence.py /path/osubackup.zip > /tmp/visual-a.json
python re/tools/visual_state_evidence.py /path/osubackup.zip > /tmp/visual-b.json
cmp /tmp/visual-a.json /tmp/visual-b.json
cmp /tmp/visual-a.json re/evidence/08/visual-states.json
python re/tools/check_visual_states.py /tmp/visual-a.json
python -m py_compile re/tools/visual_state_evidence.py re/tools/check_visual_states.py
```

Actual validation: two independent extractions byte-identical and identical to the committed evidence file; new checker PASS, E02–E07 historical evidence checkers PASS, and Python compilation PASS. Historical preservation and remote publication checks are recorded in the appended E08 journal and final handoff. The 59-method evidence checker asserts component/transform/queue/dispatch/property-consumer sites and 28 overlapping E04–E07 body identities; complete managed scan decodes 19,659 bodies with 1,739 zero-RVA declarations and zero decode errors. Zero-RVA M31f5 is deliberately retained as an abstract boundary, not mistaken for an empty implementation. Static cases below are hand-derived predictions, not tests run against stable or a surrogate renderer.

# 2. What they found

## What this means to an osu! player

A normal circle's appearance is set up while the map loads. Stable creates separate components for the circle face, an overlay, the number and the shrinking approach ring, with timed transformations already attached. A later **presentation queue**, separate from the hit-object candidate list, admits those components when their first transformation is due. The draw stage evaluates their fade/scale state, checks other gates, and may pass prepared colour and scale to an original drawing operation.

The approach ring shrinks from relative scale 4 to 1 over the recovered approach interval. The main face has a separate fade-in: normally 400 time units, beginning at `start − A`. Those are different animations. At that exact beginning their evaluated alpha is zero; being admitted does not itself produce visible pixels. As the start approaches, the ring reaches scale 1 and the main face has usually finished fading in. After start, untouched components have separate expiry/fade deadlines; hitting an object can mutate this state through other methods not recovered here.

A slider head **is a circle subclass**, constructed with the same timing engine and a different resource identifier. Loading-time slider postprocessing gathers the head's existing drawable references into the parent slider's presentation list. The slider's finalized end time does not become the head's fade or approach deadline in this path. Slider-body preparation in E07 is an additional route, not the circle/head animation engine.

**CONFIRMED static mechanism; RECONSTRUCTED player interpretation.** Actual first visible frame, exact pixels, skin results, scheduler timing and concrete drawing backend remain UNRESOLVED. This expedition reaches a virtual draw/batch operation with prepared values; it does not establish that the operation immediately submits GPU work or displays a pixel.

## Component identities and construction order

M45eb delegates to M45ec. Base construction via M1883/M249b supplies manager F152f and collections F1530/F1531. M45ec assigns position F252c (`009`), start F2523 (`017`), end F2524 (`01e`, initially start) and circle type. Its null-manager/null-manager-presentation guard returns at `09d` before visual allocation. Ordinary non-null path obtains `A` via virtual M24c9 at `09f`; ordinary inherited implementation reads manager F0adb, as established in E07.

| Component alias | Original identity / construction | Collection membership | Recovered state |
|---|---|---|---|
| Approach ring | F2bf8; M4510/T092e at M45ec:`1be`, stored `1c3` | F1531 only when approach gate passes (`1c8–1d8`) | Independent Fade and Scale; not in F1530 |
| Main circle face | F2bf9; M4510/T092e at `252`, stored `25e` | F1531 `26f`, F1530 `280` | Fade; tag-like F2cee=1; scalar scale reset to1 |
| Animated overlay | F2bfa; M44b7/T0920 at `2b5`, stored `2ba` | F1531 `2df`, F1530 `2f0` | Same cloned Fade; scalar scale1; animation helper M44c0 receives500.0 at `2ce` |
| Number/font | F2bfb; M153d/T02df at `33b`, stored `340` | F1531 at `369` if virtual M45ee true; absent from F1530 | Same cloned Fade; scalar scale `0.8f` |

The number constructor uses metadata-named `SkinOsu.FontHitCircle` / `FontHitCircleOverlap` fields (`2fc`, `306`). The other resource strings remain behind M49b0; literal texture names are **not recovered**. Component aliases above follow transformation behavior and use, not guessed decoded strings. Normal M45ed returns identifier2064519436; approach resource lookup uses2064519527. Resource fallback, dimensions and pixel identity remain outside scope. M44c0's500.0 call is recorded without asserting a displayed frame rate.

M4510 sets clock selector F2ce6 (`02c`), position F2cf6 (`034`), scalar scale F2cf4=1 (`04b`), ordering value F2ce7 (`053`), opacity F2cf5 from supplied Color.A/255f (`07b`) and base colour F2ceb (`083`). Constructors here pass clock selector1, so M4736 reads F2358 (`008`), the gameplay-time operand used in E07. Their lifetime flag F2cea is false. A freshly allocated white component can thus hold opacity1 **before any transform evaluation**; withholding it in a queue is different from assigning alpha0.

Approach registration uses virtual M45ef (base true), Hidden bit8 from manager F17ef, full registered count F17fb.Count and, for the first-object exception, opaque boolean conversion M177c(F0410):

- M45ef false: do not append the approach component.
- No Hidden: append it.
- Hidden with nonempty full object list: do not append it.
- Hidden with empty full list: append only if M177c(F0410) true.

Ordering is significant: construction precedes this object's own manager registration. No claim about the user-facing name/default of F0410 is justified here. M45ee and M45ef both return true for the ordinary circle and are inherited by the head. The component can be allocated and timed even when omitted from F1531.

## Exact time transformations

Inputs: signed int32 `S=F2523`, `A=virtual M24c9()`/managerF0adb, static signed int32 `C=F17e6=400` (M289b:`018–01d`), `W1=managerF0add`, `W2=managerF0adc`. E07 recovered A's modified approach-related mapping; A is not the E05 spacing value F0adf. Here A is captured during construction, not recomputed on every frame. Later reset/rebuild paths can change state; changing a setting in isolation is not proof that already-created deadlines move.

Metadata enum T021a explicitly names Fade=2, Scale=4 and Colour=16. M2995 stores type F1873, scalar endpoints F1866/F186a, signed start/end F186e/F186f, easing F1874. M299c uses MemberwiseClone, so face/overlay/number receive separate transform instances with equal initial values.

| Target | Kind / endpoints | Start | End | Original sites |
|---|---|---|---|---|
| Approach | Fade 0f→0.9f | S−A | Min(S, S−A+2*C) | M45ec:`1e8–20d`, Min at`202` |
| Approach | Scale 4f→1f | S−A | S | M45ec:`21d–232` |
| Face, overlay, number; normal | Fade 0f→1f | S−A | S−A+C | M45ec:`397–3af`, clone/add `3c1–3f2` |
| Same; Hidden | Fade 0f→1f | S−A | S−i4(r8(A)*0.6) | M45ec:`371–38f` |
| Same; normal untouched fade-out | Fade 1f→0f | S+W1 | S+W2 | M45f6:`047–087` |
| Same; Hidden fade-out | Fade 1f→0f | S−i4(r8(A)*0.6) | S−i4(r8(A)*0.3) | M45f6:`089–0d2` |

All these transforms use easing0. The stored 0.9f is exactly the decoded float32 literal `0.8999999761581421`; 0.8f is `0.800000011920929`. `i4` denotes original `conv.i4`, not a rounding-to-nearest prescription. Integer additions/subtractions/multiply are unchecked signed-int32 operations; preserve original nesting and conversions. Formulas here do not define safe behavior for overflow, NaN or out-of-range CLR conversions. Normal finite interval cases are the validation scope.

Non-Edit M45ec calls virtual M24b3 at `502`; for ordinary circle/head this resolves M45f6. M45f6 first calls base M24b3 (`001`), whose collection callback M24ce removes transforms satisfying M24cf: tag F1875 equals148 (`001–00b`); it also resets F1527. M45f6 then creates a fresh fade-out tagged148 and clones it onto the face/overlay/number (`0e9`, `0ff`, `115`). Normal approach gets no fade-out in this method. Hidden may append a cloned fade-out to the approach list when M45ef and first-object conditions pass (`11a–12f`), but constructor registration still controls whether that component participates. Overlapping transforms are resolved by list order below; do not assume a later appended Fade overrides an earlier active Fade.

M45f6 resets face and overlay scalar scale to1 (`13f`, `14f`) and number to0.8f (`15f`). Later colour setup M24a8 adds a Colour transform to F1530 with deadline `S−400 → S−300`, tag147. It does not replace these Fade/Scale values, but can influence drawable lifetime. Edit-specific M45ec:`3f7–500` is a separate branch; normal-play predictions do not apply to it. Hit/reset/selection methods such as M45f7 can mutate transforms and are outside the untouched-object cases.

## Slider head identity, position and registration

**CONFIRMED:** M142d calls virtual M143a at `1b5` on its presentation path. M143a constructs head position by converting each parent coordinate through **conv.i4 then conv.r4** (`012–013`, `01f–020`), supplies parent S (`027`), NewCombo-related value, selected edge/parent sound value and F152c, then constructs M351a (`054`) and stores slider F0bdf (`059`). M351a belongs to **T06f6, extending circle T094c**; it calls M45eb at`00a`. Alternate M351b calls M45ec. Head override M351c supplies resource identifier2064459632 in the M45ed virtual slot; M45ee/M45ef/M45f6 and timing getter are inherited. This is shared code and state construction, not merely equal-looking formulas.

After nonempty geometry, M144a's loading postpass:

1. With F0bd7 present, clears parent F1531/F1530 (`bfc`, `c07`).
2. Reads head F0bdf.F1531 (`c12–c17`), takes each **same reference**, writes drawable.F2ced=parent slider (`c28`), and appends to parent F1531 (`c35`).
3. Appends head F1530 references to parent F1530 (`c65`); other slider components are then included outside this head projection.
4. Near postpass completion calls F0bd7.M2ce9(parent.F1531) through MethodSpec2b000006 (`1575`).

The path does not clone head drawables or substitute parent E for their stored S-based transformation deadlines. Geometry-empty early return at`bea` precedes this merge/registration. A head allocation alone therefore does not guarantee registration. Repeated postprocessing/reset is outside the initial-load prediction; list clearing is not proof of de-registering previously submitted references.

This makes the E07 difference understandable: a normal circle has its timed presentation components registered without opting into M24b5; the slider head takes the shared circle path, while the parent slider additionally participates in M144b body presentation preparation. Candidate eligibility M45fc/M1448 does not directly evaluate these components' Fade/Scale.

## Presentation staging and admission: another window

**CONFIRMED:** M28d2 registers circle F1531 with manager F1800 when its registration argument and presentation manager allow it (`0dd–0f8`). MethodSpec2b000006 resolves to generic M2ce9, which enumerates non-null references and calls M2ce6 (`01f`). Loading M28f5 enables staging with M2cdb(true) (`043`) before parsing, calls virtual postpass M28e9 (`269`) and then disables staging with M2cdb(false) (`288`) on the reached non-Edit path.

M2ce6 checks M2cda/F1a63. Staging plus a nonempty transform list inserts the drawable into F1a65 by BinarySearch/Insert (`01b–042`). Its default comparer M473b compares **first transformation F186e**, not all transform starts or object end. Otherwise M2ce6 locks F1a54 and inserts into active presentation F1a6c using comparer F1a56; M2f4a compares float ordering F2ce7. No clone is made. Disabling staging transfers entries in their staging order to queue F1a67 (M2cdb:`020–049`) and then sets F1a63 (`050`). Equal-key insertion order is not specified by this investigation.

Ordinary manager construction M289a enables F1a62 for non-Edit (`092–0a1`). In presentation draw M2cf0:

- Empty active collection is not an early return when this queue flag is true (`1f5–203`).
- Manager opacity F1a57 `< 0.01f` or F1a58 true marks active items F2cf7=false and returns **before queue draining** (`208–243`). Exact stored threshold is `0.009999999776482582`; finite equality passes.
- Queue processing peeks F1a67 (`2e7`). With transforms, it compares the first transform start (`30a`) to selected time: clock selector F2ce6 zero uses sampled F2c3a, nonzero uses sampled F2358 (`30f–31c`). **Start > time breaks** at`31e`; equality admits.
- Empty-transform entries are dequeued/disposed (`334–339`); others dequeue and re-enter M2ce6 (`347–34c`), now entering active F1a6c on the normal non-staging path.

The queue is ordered by a drawable's first transform, not a general minimum over its whole transform list. It is independent of F17fd and does not consult F153a, M24b5, M45fc or M1448 in this admission step. Global time-field sampling in the queue and later per-drawable clock reads are distinct; a real frame need not observe one immutable t throughout.

## Draw-stage evaluation and virtual dispatch

Smallest verified ordinary draw route:

- E02 active Play registration in host F2c9f → M46a6 ForEach at`0e4`, delegate M46fe.
- M46fe casts T08fb and calls virtual M4416 (`00b`). Play T047b extends T05b4→T08fb; M2287 is a reuse-slot override, with M2bf4 intermediate override metadata. Matching name/signature alone is not assumed to imply shared runtime type.
- M2287's main path requires nonzero F13bb (`006`, purpose remains opaque) before virtual M2288 (`1b7`).
- M2288 → manager M28e7 (`011`) → F1800.M2cf0 (`006`).
- M2cf0 active-item loop skips F2cfe via M4734, calls virtual M4738 (`400`), and handles status0=draw continuation,1=skip,2=remove (conditional disposal). Then virtual M4737 at`4e1` supplies the draw call.

Ordinary sprites resolve update slot M4738 to **M4518**. Animated overlay resolves to **M44c8**, which first calls M4518 (`001`) and, on status0, calls M44c9 (`00d`) for later animation/frame work. Number resolves through **M1541 → M0949 → M4518**; glyph-specific continuation is not recovered. Ordinary approach/face and overlay use draw override **M451a**. Number has M1545, whose full glyph submission is outside scope; equal alpha state does not prove identical drawing code.

M4518 first calls base M4738 (`001`), retaining a global F0420/M177c/optional membership gate and disabled F2cfe gate. It then calls M4519(true) (`011`). Nonzero status returns immediately. Thus neither a prepared object nor a candidate can force transform evaluation through a disabled drawable or an unreached Play draw path. E07's F13d8 preparation-readiness gate and this F13bb/main-presentation path must remain distinct.

## Transformation evaluation and state priority

**CONFIRMED instructions; RECONSTRUCTED finite/non-repeating projection.** M4519 is a general evaluator with other transform types and loop/repetition branches. Only the ordinary non-repeating Fade/Scale projection is recovered here.

- Empty transform list: return2 if lifetime flag F2cea false, otherwise0 (`00f–01c`).
- Read clock through M4736 (`026`). For constructor clock1, this is F2358.
- Forward enumeration tracks `futureOrActive` when start>=t **or** end>t, and `pastOrEnding` when end<=t. Skip start>t (`0c4`) and end<t (`0ff`). Active endpoints are **both inclusive**.
- First active transformation for each property wins in original list order. Fade calls M2d71 and writes F2cf5 (`1fa–1ff`); Scale writes F2cf4 (`33c–341`). A second active Fade/Scale for that property is skipped. This matters at the exact junction of fade-in and fade-out, including Hidden.
- Before endpoint fallback, a fully expired nonpersistent drawable can return2 and set F2cf7=false (`745–76d`), when clock0 or presentation F1a62 is enabled. If no active/keep state and not both future and past, another gate returns1 for clock1 (or2 for clock2), also clearing F2cf7 (`76e–78c`). In particular, future-only state can be skipped **before assigning from-values**.
- Remaining reverse pass considers end<t (`7af`), first unassigned property wins and receives its endpoint (Fade F2cf5 at`893`, Scale F2cf4 at`913`). Reverse list order, not sorting by latest timestamp, determines this fallback.
- If future work exists and clock is nonzero, a forward pass over transforms with start>=t (`b26` skips start<t) supplies still-unassigned starting values (Fade `b93`, Scale `c13`). Final status depends on retained/active/past state. Repetition path `59a–745` is outside the projection; do not silently emulate it as simple clamping.

M2d71 preserves these numerical steps: equal scalar endpoints return that value; signed unchecked elapsed=t−start and duration=end−start; **elapsed==0 returns from before checking duration==0**; zero duration otherwise returns to. The difference of the r4 endpoint parameters is followed by conv.r8 (M2d71:`021–022`), with no intervening explicit conv.r4; elapsed/duration and from are also converted to r8 before M2d74, and the returned result is converted to r4. Preserve original CLR evaluation-stack precision rather than inserting an extra forced rounding step. For easing0, M2d74 uses the default arithmetic at`0c0–0c8`: `delta * (elapsed/duration) + from`, with its exact-zero and exact-end early guards (`000–02f`). It is not a nearest-integer or frame-index animation. This does not establish behavior for malformed intervals/overflow/nonfinite values beyond the original conversions and branches.

## Property consumer and the exact unresolved pixel boundary

M4518 builds a drawable candidate only when **r8(F2cf5)>0.001** and manager F1a57>0 (`02e–04f`, unordered alpha fails). Clipping/coordinate branches can suppress it. When candidate=false and manager F1a75=false, it sets F2cf7=false and returns1 before most composition (`3b2–3c5`). The optional F1a75 branch permits further processing, but does not prove successful drawing.

Scale F2cf4 is read at`3e0`, clamped through Max(0,scale), combined with vector scale F2cf8 and other factors into F2b8d (`cac–cbe`, with later conditional manager scaling). Therefore the recovered 4→1 is a **relative scalar**, not a screen pixel radius. Full coordinate/skin scaling is not recovered.

Alpha composition at`b58–b6c` is:

`alphaByte = conv.u1((F2cf5 * manager.F1a57) * 255f)`

It then conditionally scales that byte by base colour alpha/255f only when base alpha is strictly between0 and255 (`b7e–b96`). In the controlled finite in-range cases, conv.u1 truncates rather than rounds. Colour/manager mixing builds prepared F2b6d (`bcf` or`ca7`); complete RGB mixing is outside scope. Later bounds checks can still return1. At the normal end M4518 stores drawable candidate into F2cf7 (`f3f`) and returns0 if true,1 otherwise.

M451a calls base M4737 (`001`, global/disabled gate), obtains resource via virtual M451d (`01b`), and can return without drawing if null or M27d7 says unavailable (`022–02d`). Its boolean return alone is not proof of submission. Resource-dependent scaling/source adjustments also occur before the final call. With resource F1764 non-null, M451a passes prepared colour **F2b6d (`14c`)** and scale **F2b8d (`152`)**, position/origin/rotation/source and other arguments to virtual **M31f5 (`172`)**.

**UNRESOLVED exact continuation:** M31f5 is abstract, RVA0, declared by T067e. Receiver comes from resource.F1764 (field signature `061299f8`). Concrete receiver selection, concrete operation, batching/flush/backend and the first captured non-background pixel are not resolved. This is a verified draw-stage **operation boundary**, not verified GPU submission. No pixel-identical or visually close claim follows. Resource failures, byte-alpha quantization, clipping and unsampled draw times can all separate theoretical animation start from first visible frame.

## Interface and discriminating static predictions

The recovered interface consumes S/A/C/W1/W2, manager mods/count/presentation flags, component clock/lifetime/disabled flags, original transform list order, sampled clocks and resources. Outputs include component registration references, queue→active transfer, float Fade/Scale writes, evaluator status, F2cf7, composed byte alpha/scale and possible M31f5 call. It does not output judgement, hittability, texture pixels or measured cost.

For V01–V13: non-Edit ordinary base Play, valid manager/presentation, Hidden off, no input/hit/reset mutations, non-repeating transforms, onscreen resources ready, all global gates permitting, F1a75=false, manager opacity1, queue flagtrue, clock constant throughout each probe. Set S=5000,A=1200,C=400,W1=100,W2=150 **as controlled field inputs**, not invented defaults. Body means face/overlay/number Fade state; their scalar scales are1/1/0.8, not the approach scalar. Readouts below are approximate decimals of the recovered float calculations. Each time probe assumes normal admission/retention history; removed components are not magically re-added by seeking.

| Case | Probe | Static expected result, conditional on reached path |
|---|---|---|
| V01 | t=3799, before S−A | First transform3800>t; queued component withheld. Do not assert stored opacity0 or pixels. |
| V02 | t=3800 exactly | Queue admits at equality. Body Fade0; approach Fade0, Scale4. Float alpha gate fails. |
| V03 | t=3801 | Body Fade≈0.0025 and approach Fade≈0.001125 pass >0.001; both byte alpha0 after multiplying255/truncating. A draw call may still carry zero alpha. |
| V04 | t=4000 | Body Fade0.5; approach Fade≈0.225, Scale3.5. |
| V05 | t=4400, halfway A | Body Fade1; approach Fade≈0.675, Scale2.5. |
| V06 | t=4600 | Approach Fade reaches≈0.9 at its inclusive end; Scale2. Main face remains1. |
| V07 | t=5000, start | Body Fade1; approach Fade≈0.9 through endpoint fallback, Scale1 at inclusive active end. Possible draw operation, not proven pixels/hit eligibility. |
| V08 | t=5001 | Nonpersistent approach has no future/active transform: status2/removal under queue-enabled manager. Main body remains1 before its later fade-out. No requirement to first write approach opacity0. |
| V09 | t=5100 | Body fade-out starts at equality, Fade1. |
| V10 | t=5125 | Body Fade0.5. |
| V11 | t=5150 | Body fade-out inclusive end, Fade0 and float gate fails. |
| V12 | t=5151 | These untouched finite visual lists fully expired: status2/removal. This is not a recovered hit-object disposal or miss judgement. |
| V13 | Matched circle/head S,A; slider E=9000 | Same head/body/approach transformation schedule; parent duration does not extend head deadlines in the recovered initialization path. Resource identifier and coordinate conversion can differ. |

Additional branch cases:

| Case | Change | Static expected result / limit |
|---|---|---|
| V14 | A=450 set before construction; t=S−225 | Body Fade225/400=0.5625; approach Fade≈0.45, Scale2.5. |
| V15 | A=1800 before construction; t=S−900 | Body Fade1; approach Fade≈0.9 (800 fade interval already ended), Scale2.5. |
| V16 | A=1200 Hidden, full count nonzero | Approach allocated but omitted from F1531. Body fade-in3800→4280, fade-out4280→4640. At4400 Fade≈2/3. |
| V17 | Same Hidden at4280 | Both Fades active at shared endpoint; earlier fade-in wins, gives1. At4640 fade-out gives0. Colour transform ending4700 may retain the component beyond fade completion; alpha0 is not identical to immediate removal. |
| V18 | Hidden and full count0 | Constructor approach registration depends on M177c(F0410). Preserve setting as opaque; inherited reset has separate first-count condition and overlapping-Fade ordering. |
| V19 | manager/presentation null during circle construction | Return before visual component allocation. Object data can exist without these visuals. |
| V20 | Linear slider prepared geometry empty | M144a returns before head-reference merge/registration. Allocation is not proof of registered presentation. |
| V21 | F2cfe true | Active-loop disabled check skips update/draw; do not invent freshly evaluated alpha/scale. |
| V22 | manager opacity below0.01f or F1a58 true | Queue draining and visual update blocked by early return; active F2cf7 cleared. Exact0.01f passes this gate, subject to later ones. |
| V23 | F13bb zero on main Play draw path | M2288 route unreached. Candidate membership, F13d8 and loading geometry do not prove visual evaluation. |
| V24 | Draw resource null/unavailable or receiver F1764 null | No M31f5 call on that route, even where M451a returns true. |
| V25 | Direct future-only M4519 evaluation before3800 | Status1 for clock1 before endpoint fallback; constructor opacity can remain1. Contrasts with a simplistic always-clamped fade function. |
| V26 | Zero-duration Fade, t=start=end, differing endpoints | Active endpoint gate includes equality; M2d71 elapsed0 returns **from**, not to. Later behavior still depends on lifetime/other transforms. |
| V27 | Two overlapping active Fade transforms | First in list wins; appending an active Fade does not necessarily replace alpha. No general latest-start rule. |
| V28 | Mutate A after construction without resetting/rebuilding | Existing transform start/end integers do not automatically track that field change; construction snapshots and later writers must be distinguished. |

These cases are reference predictions for future instrumentation; no original CLR numerical probes or artificial-state fixtures were executed. No claimed visibility time can be derived from them without the missing downstream conditions.

## Unresolved mechanisms, performance and smallest next experiment

**UNRESOLVED:** concrete M31f5 receiver/dispatch and how one face component reaches a presented frame; exact global gate settings and F13bb purpose; texture/resource identity and full glyph/animated-frame output; complete coordinate/clipping/RGB logic; time field writing/audio offset/scheduler behavior; hit-driven state mutations, Edit/special Play variants, repetition/easing families and malformed numeric conversion results. These are explicit dependencies, not implementation defaults.

The smallest useful next static investigation is **one ordinary circle face's concrete M31f5 receiver selection and its next draw/batch boundary**. Keep one known loaded resource and stop before general skin/GPU reconstruction. This advances the verified state path toward actual output. In parallel, the minimum original-Windows experiment is one pinned-build control map with one centered circle and a second map replacing it with a straight slider at the same S/A (fixed hashes/settings/resources), normal playback/no hits:

1. Record Windows/display/capture details and archive/exe/map/config/skin hashes per E01; log actual selected ordinary types and component identities.
2. Instrument one object only: load completion, candidate admission (M28e2), slider preparation entry M144b, queue dequeue M2cf0:`347`, component M4519 entry/return, F2cf5/F2cf4/F2cf7, composed alpha, and M451a:`172` receiver type/time. Include clock reads and high-resolution host timestamps. No whole-engine replacement.
3. Capture frames with corresponding presentation timestamps. Identify first non-background pixel in fixed component regions against a no-object baseline; distinguish face and approach. Check t=T−1/T/T+1, halfway A, S/S+1 and fade-out endpoints with natural playback first. Artificial time/flag probes must be separately labelled, not described as ordinary playback.
4. Compare no-debugger, tracing and cold/warm runs to quantify instrumentation cost. Only then correlate admission/preparation/draw/first pixels with measured frame intervals. Distorted sliders are a later contrasted probe, not the sole control.

Owner-reported distorted-slider stutter remains an unmeasured observation, with cause UNRESOLVED. This expedition adds a boundary: component/transform construction and staging during loading versus queue transfer, transform evaluation, property composition and animated-overlay continuation during draw. E06 loading geometry and E07 conditional body preparation remain distinct. No CPU/GPU cost, caching benefit or frame-spike attribution is inferred. Precomputation/caching remains a future measured product-design question; Work 1 implements none, and Work 2 still requires explicit authorisation.

# 3. Pseudocode

All blocks are independent research descriptions of original mechanisms, **not executable replacement components**. `RECONSTRUCTED` means the scoped projection matches the cited static instructions; runtime order/timing/pixels remain unverified. Opaque dependencies stay explicit. Arithmetic uses original unchecked int32 and r4/r8 conversions; no defensive clamps are added except the original composition Max.

## Initial visual state and shared slider head

```text
RECONSTRUCTED — initial ordinary non-Edit circle path (M45ec/M45f6)
construct base object, F1530 and F1531; assign position, S, end=S, type
if manager is null or manager.F1800 is null: return
A = virtual M24c9()                    // ordinary: manager.F0adb
hidden = (converted manager.F17ef & 8) > 0
showApproach = virtual M45ef() and
               (not hidden or (manager.F17fb.Count == 0 and M177c(F0410)))
create distinct approach, face, overlay, number objects; preserve resource helpers
all use clock1; nonpersistent lifetime; number uses scalar0.8f
if showApproach: F1531.add(approach)
F1531.add(face); F1530.add(face)
F1531.add(overlay); F1530.add(overlay)
if virtual M45ee(): F1531.add(number)
append approach Fade(0f,0.9f, S-A, Min(S,(S-A)+2*C), easing0)
append approach Scale(4f,1f, S-A,S,easing0)
fadeInEnd = hidden ? S-i4(r8(A)*0.6) : (S-A)+C
for component in [face,overlay,number]:
    append separate clone of Fade(0f,1f,S-A,fadeInEnd,0)
virtual M24b3():                       // ordinary resolves M45f6
    base removes tag148 transforms from F1531, resets F1527
    outStart = hidden ? S-i4(r8(A)*0.6) : S+manager.F0add
    outEnd   = hidden ? S-i4(r8(A)*0.3) : S+manager.F0adc
    append separate tagged148 Fade(1f,0f,outStart,outEnd,0) to each main component
    if hidden and virtual M45ef() and manager.F17fb.Count == 0:
        append same-kind separate clone to approach
    reset scalar scales to [face1f,overlay1f,number0.8f]
UNRESOLVED: decoded resources, other settings, later hit-driven state mutations

RECONSTRUCTED — slider head connection, not slider body construction
headPosition = Vector2(r4(i4(parent.x)), r4(i4(parent.y)))
head = new T06f6(manager,headPosition,parent.S,other recovered inputs)
    // inherits circle construction above; resource virtual slot resolves M351c
parent.F0bdf = head
in M144a loading postpass after nonempty geometry:
    if parent.F0bd7 exists:
        clear parent.F1531 and F1530
        for sameDrawableReference in head.F1531:
            sameDrawableReference.F2ced = parent
            parent.F1531.add(sameDrawableReference)
        parent.F1530.addRange(head.F1530)
        preserve other slider-component work outside head projection
    when reached, parent.F0bd7.M2ce9(parent.F1531)
// No substitution of parent.end for head's stored transformation deadlines.
```

## Registration and draw-stage opportunity

```text
RECONSTRUCTED — presentation registration/admission projection
M2ce9(references): for each non-null reference: M2ce6(reference)
M2ce6(d):
    if d is null: return
    if stagingFlag and d.transforms.Count > 0:
        binaryInsert staging by first-transform.start (M473b)
    else:
        under original lock, binaryInsert active by F2ce7 (M2f4a)
M2cdb(false), if staging was true:
    enqueue staging entries in their current order; clear staging
    stagingFlag = false

host draw delegate -> Play.M2287 -> M2288 -> M28e7 -> presentation.M2cf0
retain F13bb and all earlier original gates
if manager opacity < 0.01f or F1a58:
    clear active components' F2cf7; return before draining
if queueFlag:
    while queue not empty:
        d = queue.peek()
        if d.transforms nonempty and first.start > sampledSelectedClock(d): break
        dequeue d
        if transforms empty: original Dispose(d)
        else: M2ce6(d)                 // same reference into active list
for each active d in original order:
    if d.F2cfe: continue
    status = virtual M4738(d)          // ordinary sprite M4518
    if status == 2: remove with original conditional disposal
    else if status == 0: virtual M4737(d) // ordinary sprite M451a
UNRESOLVED: scheduler timing, earlier/global gates, concrete batch output
```

## Finite non-repeating Fade/Scale projection

```text
RECONSTRUCTED — M4519 projection; not the full transform/repetition engine
if transform list empty: return lifetimeFlag ? 0 : 2
clock = M4736()                         // clock1 reads F2358
keep = lifetimeFlag; future = false; past = false
assigned = empty per-property flags
for tr in original list order:
    if tr.start >= clock or tr.end > clock: future = true
    if tr.start > clock: continue
    if tr.end <= clock: past = true
    if tr.end < clock: continue
    keep = true
    if tr.kind is Fade or Scale and that property not assigned:
        write corresponding F2cf5 or F2cf4 = interpolate(tr,clock)
        mark that property assigned
    preserve other transform-type branches outside this projection
// Ordinary non-repeating inputs bypass repetition section.
if not future and not keep and (selector == 0 or currentPresentation.queueFlag):
    F2cf7 = false; return 2
if not ((future and past) or keep):
    F2cf7 = false; return selector == 2 ? 2 : 1
for tr in reverse original list order:
    if tr.end < clock:
        keep = true
        if relevant property unassigned: write tr.to; mark assigned
if future and selector != 0:
    for tr in forward original list order with tr.start >= clock:
        if relevant property unassigned: write tr.from; mark assigned
return keep ? 0 : 1

CONFIRMED arithmetic projection — M2d71 -> easing0 M2d74
interpolate(tr,t):
    if tr.from == tr.to: return tr.from
    elapsed = i32(t - tr.start); duration = i32(tr.end - tr.start)
    if elapsed == 0: return tr.from
    if duration == 0: return tr.to
    delta = conv.r8(original_F_sub(tr.to, tr.from)) // no explicit conv.r4 here
    if delta == 0 or r8(elapsed) == 0 or r8(duration) == 0:
        return r4(r8(tr.from))          // M2d74 early return
    if r8(elapsed) == r8(duration): return r4(r8(tr.from) + delta)
    return r4(delta * (r8(elapsed)/r8(duration)) + r8(tr.from))
UNRESOLVED: repeat mutations, other easing/types, malformed CLR numeric results
```

## State consumer and stopping point

```text
RECONSTRUCTED — M4518/M451a boundary, partial coordinate/resource projection
M4518(d):
    status = direct base M4738(d); if status != 0: return status
    status = M4519(d,true); if status != 0: return status
    candidate = (r8(d.F2cf5) > 0.001) and manager.opacity > 0
    apply original clipping/coordinate conditions to candidate
    if not candidate and not manager.F1a75: d.F2cf7=false; return 1
    preparedScale = original composition using Max(0,d.F2cf4), vectorScale, manager factors
    a = conv.u1((d.F2cf5 * manager.opacity) * 255f)
    if 0 < baseColour.A < 255: a = conv.u1(r4(a)*r4(baseColour.A)/255f)
    preparedColour = original colour branches with byte a
    retain later bounds/resource-related side effects and skip conditions
    d.F2cf7 = candidate
    return candidate ? 0 : 1
M451a(d):
    retain base global/disabled gate
    resource = virtual M451d(d)
    if resource is null or M27d7(resource): return without M31f5
    retain resource-dependent source/scale/origin adjustments
    if resource.F1764 exists:
        virtual M31f5(resource.F1764, prepared position/origin,
                      F2b6d colour,F2b8d scale,rotation,source,other original inputs)
UNRESOLVED — concrete M31f5 receiver/backend, flush and actual presented pixels
```
