# 1. Result

Expedition035 — Ordinary Health State, Passive Drain & Judgement-Driven HP.
Work1, 2026-11-03 Asia/Bangkok. Canonical main base
`98f1c9c1ca05359d93c75a7278c9c69294bb30c0` (QA06).

For a player: the original has a failure-tested HP target, a separate smoothed
presentation value and a separate calibration balance. A judgement changes the
target immediately; the presentation can catch up later. Passive drain uses
corrected gameplay milliseconds and a chart-calibrated rate. Break eligibility,
pause, current-spinner selection and the order of updates all matter; one
universal “HP -= drain * frameTime” rule would omit original behavior.

Recovered the bounded fresh ordinary T0253/T00e8/T056a chain: initialization,
chart calibration, three-state mutations, passive drain, break/time eligibility,
plain judgement deltas and E19 fail/recovery boundary. This is original-static
research, **not an implementation or an original-runtime validation**.

## Scope, records and provenance

Read AGENTS/complete standing contract (repository/attachment byte-identical),
cumulative research through QA06 and relevant E05/E07/E10/E15/E17–19/E24–29
instruction boundaries. QA06's354 passing independent component cases are a
preserved historical checkpoint, not newly executed E035 tests. No reconstruction,
QA assertion, fixture, E20 packet or earlier report is edited. The new owner
feedback that scores still differ on some maps is **OWNER-REPORTED and deferred**;
E33/E34 conclusions are unchanged. Inspection of shared M0ad2 here concerns HP
operands only, not a new numeric-score theory.

The earlier complete E035 assignment referenced by the owner was not in the
available record and could not be retrieved. This report follows the five explicit
priorities in the current instruction and the standing contract; it does not
pretend that additional unseen requirements were checked.

Actual-byte verification:

- Archive SHA256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`.
- EXE SHA256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.

The Stable40/file-version1.3.3.8 self-description remains unauthenticated as an
official dated/unmodified release. Read-only metadata/CIL analysis on Linux using
pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2, privately installed under
/tmp/ohno-static35. No game/GUI/input/OBS/debugger/original CLR execution, calibration
simulator, Windows experiment or E20 run. All17 E20 cases remain **NOT RUN —
WINDOWS HOST REQUIRED**.

Created this report, `evidence/35/health-state.json`,
`tools/health_state_evidence.py`, `tools/check_health_state.py` and an append-only
journal entry. Evidence records61 selected methods/signatures/owners/body hashes,
original operands/branches/constants, virtual resolution,9 additional store-holder
body hashes and a narrow direct
inventory of15 health/coefficient/gate fields and17 helper tokens across19659
managed bodies (1739 zero-RVA,0 errors). No proprietary binaries/assets/private
captures or full disassemblies are published.

Verification commands and actual outcomes are recorded below; original-runtime
cases are never counted as passing static checks. Remote publication identity,
exact ancestry and complete tree/blob/mode verification are supplied at handoff.

```sh
PYTHONPATH=/private/readers python re/tools/health_state_evidence.py /private/osubackup.zip > /tmp/health-a.json
PYTHONPATH=/private/readers python re/tools/health_state_evidence.py /private/osubackup.zip > /tmp/health-b.json
cmp /tmp/health-a.json /tmp/health-b.json
python re/tools/check_health_state.py /tmp/health-a.json
```

# 2. What they found

## A. Identities, fields and fresh initialization

**CONFIRMED STATIC** = original metadata/instructions. Field aliases, formulas and
conditional predictions are **RECONSTRUCTED**. Actual runtime configuration,
helper interleavings and presentation remain **UNRESOLVED**. No external osu!
version, remembered HP rule or community implementation is used as evidence.

Mxxxx/Fxxxx/Txxxx denote MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx.
Offsets below are hexadecimal CIL-relative, excluding method headers; JSON offsets
are decimal. Selected method signatures and exact hashes are in the evidence.

| Method | Owner / signature | Bounded role |
|---|---|---|
| M1142 |T0253 /200001|Ordinary override of M0ab6: prepare rate and fill setup |
| M1150 |T0253 /20000d|Chart calibration, returns Double rate |
| M1146 |T0253 /200001|Ordinary override of M0ac7: passive subtraction |
| M0ac4 |T0186 /200001, nonvirtual|Reached ruleset-update wrapper |
| M2979/M297a/M297b |T056a /2001010d|Absolute assignment/subtraction/addition |
| M297d |T056a /200001|Three-value reset |
| M2972 |T056a /200001|Smoothed state and presentation update |
| M226c |T047b /200001|Existing target-based fail/continue decision |

Ordinary M0aaf calls virtual M0ab4 at000d, which T0253 inherits. M0ab4:0007 constructs
**T056a via M2963**, stores handler.F05f5 at000c. T0124/T0711 alternatives are not
substituted. T0253's M0ab6 slot resolves to M1142; M0ac7 resolves to M1146; M0ac6
is inherited empty, M0ac8 inherited combo/presentation work. Ordinary manager
T00e8 inherits M28b3 for abstract M125a. Concrete ancestry and matching virtual
signatures are recorded, rather than inferring a receiver from a shared method
name. E05's ordinary/Taiko correction remains intact.

Use these aliases only within this report:

| Alias | Original field/accessors | Type / relevant meaning |
|---|---|---|
| V |F1856, M2964 getter/M2965 setter|Double060d, smoothed/presentation state |
| H |F1857, M2966/M2967|Double060d, target read by failure decision |
| A |F1858, M2968/M2969|Double060d, separate adjustment balance used by calibration |
| r |F1855|Double060d, prepared passive rate, HP units per gameplay ms |
| B |handler.F0603|Double060d, group-associated gain coefficient |
| G |handler.F0604|Double060d, ordinary gain coefficient |
| fill |F1852|Boolean0602, fill-in mode, **not** passive-drain enable |
| fillFrom |F1853|Int32 timestamp gate for fill-in |
| fillRate |F1854|Double, fill-in slope |
| eligible |Play.F13c4|Boolean, late-computed continuation/drain eligibility |

Sole direct managed stores to V/H/A are M2965/M2967/M2969 respectively, each at0002.
Their getters simply load the corresponding field at0001. The setters are plain
stores, not callbacks. The exhaustive targeted accessor-call inventory is in JSON;
absence of other direct stores does not exclude reflection/external mutation.

Fresh managed numeric fields initially zero. M2963 sets fill=true at000d,
fillRate=.02 at001c, then retains resource construction/registration and an
initial M2972 call. It does not set H/A to200 by itself. M0a9c initializes B/G to1
at000a/0019. **M297d** subsequently assigns, in order:

`H=200` via000a; `A=200` via0019; `V=0` via0028.

It does **not** reset r/B/G/fill/fillFrom/fillRate. Reusing it is not equivalent
to constructing an entirely fresh health component.

### Ordinary preparation order and relevant resets

M223b loads prepared objects before handler setup. At0265, virtual M0aaf performs
factory creation and virtual M0ab6 (0025), resolving M1142. M1142 requires a
non-null component (0001–0008), calls virtual M1150 at0010 and stores its return
into F1855 at0015. Global record bit256 conditionally multiplies r by.75 at003f–
004e; original Mods metadata names it HalfTime. It then calls base M0ab6 at0054.
The bounded no-mod control skips that multiplier.

Later M223b:02b2 calls M2258(map,false), whose00e4 calls M0ace with true. With
F05f6 now true, M0ace:0041 calls M297d after record/combo reset work. Thus the
calibration's temporary state is followed by the reached live reset200/200/0;
r and the calibrated gain coefficients survive. Do not start live play from the
last calibration target or mistake its trial mutations for actual gameplay.

Base M0ab6 prepares **fill**, separately from r. If map.F0fc4>0, fillFrom becomes
−F0fc4 at001e. If firstStart*fillRate<200, fillRate becomes
`200 / Double(unchecked(firstStart + map.F0fc4))` at0083. Otherwise firstStart>10000
sets fillFrom=firstStart−10000 at00c9. Bit64 additionally multiplies fillRate by1.5
at00ff. The map operand's wider setting policy is retained as F0fc4; no new
user-setting name or observed startup animation is asserted.

Additional absolute operations are distinct: M297c writes V only; M2979 writes H
and A only; E29's M2241:0094 can set H/A from object.F1529, and M226e:005d from
record.F1989. Their original caller/restoration restrictions remain E29; these
are not universal pause/seek resets.

### Coefficient writer inventory boundaries

| Field | Complete direct managed store sites (hex offsets) |
|---|---|
| r/F1855 |M1142:0015/004e ordinary; M0c62:0071/00aa and M4607:0016/0059 alternative handlers |
| B/F0603 |M0a9c:000a; M1150:03a7/0437/049e; M1ef8:0091; M0c79:02d3/0373/03da; M4625:0344/03ab |
| G/F0604 |M0a9c:0019; M1150:03bd/044d/04b4; M1ef8:0082; M0c79:02e9/0389/03f0; M4625:035a/03c1 |
| fillFrom/F1853 |M0ab6:001e/00c9 |
| fillRate/F1854 |M2963:001c; M0ab6:0083/00ff; M0acc:0045 |
| fill/F1852 |M2963:000d; M297a/M297b:0011; M2972:02fa; M0acc:0031; alternative M085c:0014 and M359e:0136 |

M1ef8 explicitly writes B/G1, but its wider alternative source/selection is not
reconstructed as a normal fresh Play reset. Alternative calibrations/resource
reset routes above are enumerated, not assigned ordinary semantics. All other
selected readers and direct target accesses are retained in the evidence. These
facts do not justify assuming live B/G are always1 or fill alwaystrue.

## B. Chart calibration: difficulty plus prepared objects, not an invented constant

Define Phi(h;a,b,c) as the original ordinary M125a→M28b3 mapping. After M28b0's
mode/modifier-dependent HP transformation, for h>5 it returns
`b + (c-b)*(h-5)/5`; for h<5, `b - (b-a)*(5-h)/5`; otherwise b. Comparisons are
unordered-aware (`ble.un`, `bge.un`), Double arithmetic; no integer rounding.
The no-mod ordinary finite control uses the supplied HP directly. Easy/HardRock
transformations are E19's separate manager state, not a newly assumed global
modifier snapshot.

M1150 loads global map.F0b54 once at000a–0015 for four threshold calculations:

| Prepared threshold | Original call / endpoints at HP0,5,10 |
|---|---|
| L: pre-object safety threshold |003a;195,160,60 |
| U: group-end threshold |0064;198,170,80 |
| E: final target threshold |008e;198,180,80 |
| W: balance gain per object threshold |00b8;8,4,0 |

Start r=.05 (0000). Each retry at00bf resets F05fd, health via M297d, current combo
and global record; initializes previousEnd to **first registered start minus
manager approach F0adb** (00f5–0117), break index0 and low-group count0. B/G are
**not** reinitialized between retries. The stored manager count and list references
are reread, not an atomic immutable object snapshot.

For each prepared object in full-list order:

1. Consider **at most one** next prepared break. Accept it only when its start is
   >=previousEnd and its end<=current object.start (017c–0194). For format>=8,
   excluded gap is break.end−previousEnd (01a3–01ac); for older format it is
   original M0e7d=end−start (01af–01b6). Advance the break index once. No union,
   clipping or loop consuming every break in the gap.
2. Subtract `r*Double(unchecked(object.start-previousEnd-excludedGap))` through
   M297a at01d3; set previousEnd=object.end. If H<=L in the ordered finite control,
   reduce r by.96 at0217 and retry. The actual `bgt.un` at0209 allows continuation
   for H>L or unordered; do not replace it with a universal NaN rejection.
3. Compute duration loss d=`r*Double(unchecked(end-start))`; remember
   overflowLoss=Max(0,d−fresh H), then subtract d at0256. This is a preparation
   walk, not real-time per-checkpoint drain.
4. For Slider2, let a=`F0bf7.Count+2`, b=`F0bf8.Count−F0bf7.Count−1` (026c–0297).
   Invoke the original result consumer M0ad2 with raw64 a times and raw8 b times
   (02b6/02cf). For Spinner8 invoke raw8192 F02a0 times (02ff–0313). Other objects
   have no such component loop. These are calibration notifications, not observed
   spins or an instruction to emit extra live results.
5. If overflowLoss>0 and H−overflowLoss<=L, reduce r by.96 and retry (0315–0342).
6. If last indexed object or the next object's original M3e56 is true, invoke
   raw1028 at0378. If resulting H<U, increment the low-group counter; on its
   **third qualifying occurrence in that trial**, multiply B by1.07, G by1.03,
   then retry without changing r at that branch (0398–03c4). The counter need not
   represent consecutive bad groups. Otherwise invoke raw1024 at03cd.
7. Normally completed iterations increment F05fd and store fresh H into that
   object's **F1529 at03ec**. A retry/exception can leave a partial snapshot prefix.

After the walk, if no retry yet and H<E, reduce r by.94 and multiply B/G each by1.01
(0409–044d). Compute `(A−200)/Double(fresh manager.F17fc)` at0452–0474, even if an
already-set retry flag later skips its comparison. If still not retrying and that
value<W, reduce r by.96, multiply B by1.02 and G by1.01 (0479–04b4). Retry at04ba
or return r at04fb, retaining the optional special-Play bookkeeping suffix.

**No explicit iteration cap, synthetic catch-up guarantee or defensive empty-list
fallback is present in this method.** Empty/inconsistent direct invocations,
nonfinite values and helper failures are not assigned invented outcomes. The
ordinary loader's nonempty object gates matter. Calibration calls original
M0ad2/helpers and changes original temporary state; it is not side-effect-free.
The report does not execute this algorithm or reopen its score side effects.

### A bounded complete calibration prediction

**RECONSTRUCTED, not runtime:** fresh no-mod/null-policy handler, B=G=1, one circle
S=E=2000, manager approach1200, HP0, no breaks, stable references and all helpers
normal. Every failed early trial leaves B/G1. Trial pre-object H is200−1200r;
L=195 requires r<5/1200. The relevant closed expressions are:

- `.05*.96^60 ≈ .0043176157243`: H≈194.818861, fails the strict safety continuation.
- `.05*.96^61 ≈ .0041449110953`: H≈195.026107, passes.

The final-circle calibration raw1028 adds **14B+6G=20**. H caps200, A≈215.026107;
U/E198 and W8 are satisfied. Thus the bounded recurrence accepts after61 rate
reductions, B/G remain1. Printed decimals are approximate mathematical projections
of the repeated-Double operations, not measured original operands. The later live
reset restores H/A200,V0 while retaining the prepared rate. Other maps require the
actual prepared list, break structure, thresholds and gain iteration; no universal
HP0 or HP5 rate is inferred from this control.

## C. Passive drain, clock, current spinner and pause ordering

Reached Play order preserves E12: old-candidate current selection M2275, refresh
M28e2, optional component result processing, M28e3 object updates/evaluation,
**M0ac4 at M226b:09da**, and later eligibility/finalization logic. M0ac4 returns
when !F05f6 (0001–0008); otherwise it calls virtual M0ac6, virtual M0ac7 at0010
(ordinary M1146), then health.virtual M2972 at0023 and remaining original helpers.
Do not reorder an object's final HP event behind this drain.

M1146 requires a component and the **existing Play.F13c4 true** (0001–0013), then
calls M297a at0052 with:

`(F1855 / 1.0) * Double(unchecked(F2358 - F235a)) * (F136f ? Double(.25f) : Double(1f))`.

- F2358 and F235a are separately read Int32 values at0030/0035; subtraction occurs
  before conv.r8 at003b. No Max(0,dt), wall-clock frame interval or dt cap occurs. This is per reached
  invocation, not one guaranteed update per rendered frame.
- M3b25 captures old F2358→F235a at0043 before normal publication M3b24. But E10/
  E29 recorded-frame, seek, hold-release or readiness writes can intervene. The
  drain reads **current fields at its consumption site**, not a guaranteed earlier
  F235d snapshot or physical callback duration.
- F136f is set by M2275:0072 from selected current object's Spinner8 bit. That
  selection uses existing candidates before refresh; it is not a universal
  `spinner.start<=time<=end` predicate. True quarters this reached drain.
- Base M0ac7 runs after the optional subtraction (M1146:0058). Its SuddenDeath32
  and positive whole-miss-count gate can force H/A0 via M2979. It does not skip
  the earlier drain or directly invoke the failure decision.
- Zero dt still reaches M297a(0) if eligible, including its fill-mode flag work.
  Negative dt passes a negative subtraction argument: H and A can **increase**,
  and this subtraction helper has no upper clamp. This is a conditional retained-
  state/clock-discontinuity result, not proof that a natural seek is available.

**Pause:** the normal F1373 return in M226b:0838–0853 and E28 resume-handoff/
readiness guards occur before09da. They suppress this reached drain and M2972
through this Play path, while earlier host clock/input/presentation/timed-event
work may already run. Pause itself is not a store normalizing V/H/A or coefficients.
The normal clock can repeat or be overwritten according to E10/E28; q0 is not the
pause flag. Do not replace distinct clock fields with pausedTime.

**Recovery:** F1376 need not stop M0ac4. M2972 can fill V using host elapsed F2c71
while recovery holds gameplay time. Only later M226b:0b02 checks ordered
`V > H−5`; equality/less/unordered skip. A reached true branch clears F1376 at0b05
and calls original M3b52 after helpers. No fixed wall-clock recovery duration is
established.

## D. Breaks and the late eligibility flag

The sole direct Play.F13c4 writer is **M226b:0f0b**, **after** live drain09da.
It defaults false in a fresh object. Live drain therefore consumes an earlier
stored eligibility result; it does not retroactively prorate the current delta
against the late test or recompute the flag inline.

In original short-circuit order it becomes false if:

1. F2c73 and F235a<=F2c74 (0e7a–0e8b).
2. Recorded selector M4390 true and F2a81>=F235a (0e8d–0e9e).
3. Fresh T<unchecked(full[0].start−approach) (0ec7); equality is allowed.
4. Another fresh T>last indexed object's stored end (0ef0); equality is allowed.
5. Marker F13a2>=0 (0ef9), or clock state F2368==0 (0f00).
6. Otherwise M226f's queue check decides (0f03).

No evaluated-flag/count-of-judged-objects test is added. This is the same F13c4
continuation gate used by E27, not the session-finalization latch F1397.

### Actual break and queue producers

Prepared break records are Play.F138a.F1b9e, with start F0875/end F086e;
M0e7d returns end−start. Timed-event manager M2f05 checks each in list order:
`start<=fresh T<=end` (01e3–01fd), inclusive on both ends. A matching break sets
its local active Boolean and F1b96; it eventually stores F1b97 at03e6. In Play
mode2, pre-first/post-last manager conditions can also set that Boolean; do not
rename F1b97 exclusively “inside a user-authored break”. M226b reaches M2f05 at06fb,
before its main pause guard.

Later M2271, before late eligibility, writes fresh T to F13a2 while F1b97 is true
(000d). It dequeues F13a7 while Peek()<fresh T at007c, preserving equality.
If marker>=0 and a current candidate matches M22b1's separately sampled
`object.start<=T && object.start>=marker`, it sets marker−1 at00a8. Thus break
exit, next candidate attention and late eligibility remain cooperating boundaries.

M2243 initializes Queue<Int32> F13a7 and, for reached break.end>T and break.start>
first registered start, enqueues a selected object's **end** at017f. Selection is
M28de→M28dc: break.start + signed `(end−start)/2`, a T0792 comparison stub with
both stored times set to that midpoint, List.BinarySearch, complement if negative,
then subtract1. Original M24c2:0001–0021 compares **end times** when either operand
is that stub; normal object/object comparisons use their other original rules.
Do not turn this into a generic max-end search or certify overlapping/unsorted
end-time lists. The original failed index/equality behavior is not repaired.

M226f returns true for empty Q or **T<=Peek** at001f. Otherwise it returns
`format<8` at002c–002f. For a modern sorted finite control this can suppress drain
in the gap **after the selected preceding object's end and before break start**,
subject to the prior-value eligibility lag. At break end equality, F1b97 still
includes the interval. It is not simply an audio timer that resumes HP loss at
break.end+1 regardless of candidate state.

## E. Immediate mutations and judgement-driven HP

Ordinary T056a setters retain separate state and original order:

| Helper | Confirmed normally returning effect |
|---|---|
| M2979(x) |H=Max(0,Min(200,x)) at001e, then A=x at0025; V untouched |
| M297a(x) |If F1374&&fill, clear fill at0011; A=Max(0,A−x) at002d, then H=Max(0,H−x) at0049; no upper clamp |
| M297b(x) |Same fill guard; A=A+x at001f, then H=Max(0,Min(200,H+x)) at0049; A uncapped |
| M297d() |H200,A200,V0; leaves other state unchanged |
| M297c(x) |V=x only |

Setters do not themselves call Play.M226c. Math/field operations and separate reads
are preserved; NaN/invalid/artificial calls are not normalized by a new policy.
The accumulator can exceed200 after gains while H remains200. M2979 can make
A negative or>200 if called with such an artificial argument while H clamps.

The original ordinary result route remains M114a→base M0ad2. Preserve its F05f6
prefix, F252e/evaluated optional M2282 hook, masks, helpers and exceptional exits.
For the **plain scoped raw operands**, local Double delta is:

| Source/raw | HP delta | Relevant original sites |
|---|---|---|
| Whole circle/slider parent/spinner final1024 |6G|0884–0893 |
| Same whole family512 |G*Phi(h;17.6,2.2,2.2)|07ea–0821 |
| Same whole family256 |G*Phi(h;3.2,.4,.4)|0744–077b |
| Whole miss−131072 |Phi(h;−6,−25,−40)|045d–048e |
| Slider head or repeat64 |4G|064d–065c |
| Interior8 |3G|057d–058c |
| Terminal-coded128 |4G|071d–072c |
| Head/interior fail−262144 |Phi(h;−4,−15,−28)|0426–0457 |
| Terminal processed-count fail−524288 |Same Phi(h;−4,−15,−28)|03ed–0420; combo-enable local cleared |
| Spinner4096 and8192 |1.7G each|0b66–0b90; combo local cleared |
| Spinner16384 |2G|0b9f–0bae; combo local cleared |
| Calibration1028 |14B+6G|0b0e–0b2e |
| Raw0 |Immediate return, no delta application|0000–0003 |

Positive variants257/513/1025 add6B to their corresponding plain whole gain;
514/1026 add10B;1028 adds14B. Their body sites are retained, but full manager-
decoration provenance and scoring consequences are **not reopened here**. The
calibration's explicit1028 use is established regardless of which decorated
results an actual ordinary run later emits. General live fidelity needs the actual
raw notification, not only a visible300/100/50 label.

h is fetched through the original global map getter for the reached delta branch;
Phi uses the manager's separately stored modifier state. B/G are live calibrated
fields, not score multiplier K. Stable map/record/manager identity is a conditional
control, not a single immutable snapshot. Accounting-enable F153b gates record
work, **not** the unconditional later delta application on a reached raw branch.

### Ordering into failure and recovery

M0ad2 performs branch selection and its earlier state/combo work. A reached
combo-enabled negative can set H/A0 under handler-record SuddenDeath32 at12c3.
Terminal−524288 and spinner progress bypass that combo block; their HP delta still
reaches12df. Optional M0ad0 at12da retains its presentation/observer helpers.
Then ordered delta>0 calls M297b at12f2; nonpositive or unordered goes through
M297a(−delta) at1301. Original record/observer work follows, then **Play.M226c at1404**.
A helper exception can prevent later phases after earlier mutations have occurred.

M226c consumes fresh H, not V or A. Its `bgt.un→0093` at0029 returns for H>0 or
NaN; finite zero continues. NoFail1, prepared Relax/Relax2 flags, recorded/special
state and the original other-mode condition can suppress the failure continuation.
Otherwise M227d→M0acb(false) first tests remaining recovery count, then already-
failed latch. A normal no-recovery route performs cleanup/queue work before
**F136a=true at M0acb:0060**, then a separately read failure timestamp. These are
E19's retained boundaries, not a new game-over screen claim.

M0acc(false), when H<160, **adds160** through M297b at0025, sets fill=true and
fillRate=.07999999821186066. It sets recovery F1376, toggles playback through
M3b49 and decrements the recovery count. From H0 this yields H160; from an
artificial H100 it caps H200 while A still gains160. The recovery release comparison
uses V>H−5 later, independently of the target setter.

**Important bounded negative finding:** M1146/passive subtraction has no direct
M226c/M227d call. Its wrapper continues into M2972/helpers. M226b's direct M226c
call at0a4f is conditional on ruleset mode1, not ordinary Osu0. The first verified
ordinary HP fail test here is the result consumer at1404, as E19 established.
Do not invent an unconditional per-frame fail call after passive drain. A later
positive judgement can raise a zero target before its reached test. This is a
conditional instruction prediction, **not** an exhaustive claim that opaque or
external lifecycle callbacks can never fail a session between judgements.

## F. Smoothed state and update sources

M2972 has original resource/presentation work before its numeric continuation.
Preserve it and its exceptions. At01ee/01f4 it reads V and H separately:

- If V<H and fill true: require fillFrom<T (0214; equality skips) and either
  recovery F1376 or clock state1 (0226). Set V=Min(200,V+fillRate*d), where d is
  F235d normally and **host elapsed F2c71** during recovery (0248/024f).
- If V<H and fill false: V=Min(200,V+Abs(H−V)/4*F2c22) at02ab–02e3.
- If V>H: clear fill at02fa and V=Max(0,V−Abs(V−H)/6*F2c22) at02ff–0337.
- Ordered equality or the original unordered branches perform no numeric
  convergence write on that path.

F2c22 is the existing host update scale, not corrected gameplay milliseconds.
The linear fill and exponential branches do not universally clamp V to H; large
scales can overshoot and select a later opposite branch. The prefix, subsequent
bar-width conversion at037e/037f and M2975 call are original presentation boundaries,
not proof of an actual pixel, HUD percentage or audible feedback. A failed latch
can set a presentation width earlier in M2972 without replacing H. V/H/A remain
separate throughout pause, recovery and finalization.

## G. Conditional finite reference cases

All cases below are **RECONSTRUCTED static predictions from CONFIRMED instructions**,
with stable stated references, no additional intervening health mutations, and
normally returning original helpers. Numeric setter predictions identify the
immediate store boundary; later opaque calls are not declared side-effect-free. Every row's
original-runtime status is **NOT RUN**. Artificial field/clock/exception probes are
explicitly identified; no numerical health simulator or forced debugger run exists.
Use V/H/A units as stored (normal target cap200), G/B/r as specified, ordinary
no-mod/null-policy unless stated.

| ID | Conditions | Conditional prediction / evidence |
|---|---|---|
| H01 | Fresh health object before reset |V/H/A numeric defaults0; fill true/fillRate.02; constructor helpers retained |
| H02 | M297d on existing V50,H90,A210,fillfalse |V0,H200,A200; fill staysfalse, coefficients unchanged |
| H03 | Absolute setter x250, artificial |H200,A250; V unchanged; M2979 not delta |
| H04 | Absolute setter x−1, artificial |H0,A−1; no fail callback from setter |
| H05 | H199,A210, G1, plain300 delta6 |H200,A216; V unchanged until later update |
| H06 | H2,A2, raw interior fail, HP0 |Subtract4→H0,A0; reached M226c gates decide failure |
| H07 | H100,A100, G1, HP5 |300→106;100→102.2;50→100.4, as separate fresh controls |
| H08 | G1, HP0 |100 gain17.6;50 gain3.2; not the HP5 constants |
| H09 | G1, HP10 |100 gain2.2;50 gain.4; high anchors equal middle anchors |
| H10 | G2, raw8 versus64 versus128 |Gains6/8/8, separate checkpoint provenance |
| H11 | H100,A100, HP5, head failure vs terminal failure |Both target85; distinct combo/SuddenDeath gates remain |
| H12 | Same failures, SuddenDeath32 |Head/interior common reset arm can set0 first; terminal branch skips that forced-zero block |
| H13 | G1, raw4096 vs8192 |Each gains1.7, no common combo mutation |
| H14 | G1, raw16384 |Gain2, independently of spinner final judgement |
| H15 | B1,G1, calibration raw1028 |Gain20, compared with plain1024 gain6 |
| H16 | F153b false with otherwise reached raw8 |Record suppression does not erase HP gain3G |
| H17 | Raw0/no newly due component |No M0ad2 HP mutation or its failure test |
| H18 | r.05, eligible, T1010/previous1000, no spinner flag |Subtract.5, using signed consumed Int32 difference |
| H19 | Same, F136f true |Subtract.125, quarter factor from selected spinner |
| H20 | Same clocks, eligiblefalse |No passive subtraction; base SuddenDeath check still reached |
| H21 | eligibletrue, equal T/previous |M297a(0) still can clear fill if F1374 true |
| H22 | r.05, H199,A199,T900/previous1000, artificial backward operand |Negative subtraction raises H/A204, no upper clamp on M297a |
| H23 | Large forward time jump while previous eligibilitytrue |No dt cap/proration; target floors0 if loss exceeds it |
| H24 | Fresh eligible defaultfalse, first reached update |No passive loss from that flag yet; late eligibility can enable next pass |
| H25 | Main manual pause guard reached |09da wrapper skipped; no automatic normalization/reset of health |
| H26 | Paused host clock still published or overwritten |Does not itself run passive health update; E28 ordering retained |
| H27 | Resume permitted, same consumed T as previous |Zero difference if eligible; no invented catch-up interval |
| H28 | Recovery true, gameplay time constant, permitted wrapper |V fill can advance via host elapsed; H changes only via reached setters |
| H29 | Recovery V155,H160 |V==H−5:ble.un skips release; at V156 ordered release may proceed |
| H30 | Pre-object lower eligibility threshold equality |Late comparison permits equality; prior stored flag controls current drain |
| H31 | Last indexed end equality |Late upper comparison permits equality; immediately greater rejects |
| H32 | Break start/end equality in M2f05 |Both belong to inclusive interval; F1b97 can update marker |
| H33 | Queue head3000, T3000, modern |M226f true; Q equality retained by M2271 |
| H34 | Queue head3000, T3001, markernegative/other gates pass |Modern M226f false; late flagfalse, no retroactive undo of current drain |
| H35 | Same queue/time but format7 |M226f true; other break/marker gates still independent |
| H36 | Objects end1000/3000/8000, break4000–6000, sorted finite control |Midpoint5000 stub insertion selects preceding end3000 for Q; gap-before-break suppression is possible |
| H37 | F1b97 active then next candidate start<=T and>=marker |M2271 can reset marker−1; it does not inspect evaluated state |
| H38 | Calibration break contained in priorEnd1000/currentStart6000; break2000–3000 |Modern excluded2000 gives drainable gap3000; legacy excluded1000 gives gap4000 |
| H39 | Same gap contains another later break4000–5000 |Only one break consumed for current object; no break-union/catch-up loop |
| H40 | Calibration H exactly L after gap |Ordered equality fails continuation and reduces r*.96 |
| H41 | HP0 single circle/approach1200 control above |61 rate reductions conditionally accept; no native execution claimed |
| H42 | HP5 single circle first trial, G/B1, approach1200 |H140<=L160 forces r.05→.048 before group gain; B/G unchanged |
| H43 | HP10 same first trial |H140>L60; group gain20 leaves160; mean−40<W0 triggers r.048,B1.02,G1.01 |
| H44 | Third qualifying calibration group end below U |B*=1.07,G*=1.03; retry branch does not itself reduce r |
| H45 | End criterion fails without earlier retry |r*=.94;B/G*=1.01; mean getter still reached before retry |
| H46 | Artificial direct calibration with empty full list and valid other receivers |List[0] can throw after earlier resets; no safe fallback rate assigned |
| H47 | Helper throws after a snapshot prefix in calibration |Earlier fields/F1529 writes can remain; no F1855 return store guaranteed |
| H48 | H0,V100 at M226c; all gates pass, count0 |Target0 leads through existing handler to F136a latch despite positive V |
| H49 | Same H0 with NoFail1 or original continuation veto |No new failed latch through M226c; preceding HP writes remain |
| H50 | Same H0 with positive recovery counter |M0acc adds160, sets recovery/fill machinery and decrements count instead of new latch |
| H51 | Passive subtraction reaches H0; no later raw result yet |No direct M226c in M1146; no invented ordinary per-frame test |
| H52 | H0 followed by G1 raw300 before its fail test |Gain makes H6; actual fresh test returns for positive target |
| H53 | V100,H200,fillfalse, host scale1 |Rise to125; H/A unchanged, exact exponential branch |
| H54 | V100,H40, host scale1 |Fall to90 and clear fill; target remains40 |
| H55 | V0,H200,filltrue, fillFrom==T |Linear fill equality skips; greater T with clock1 can permit advance |
| H56 | Setters/clock refs change between reads or original helper interrupts |Preserve separate consumed references/partial mutations; no atomic frame snapshot |
| H57 | Artificial two direct M1146 calls with same T1010/previous1000,r.05,eligibletrue |Each subtracts.5; method has no consumed-delta marker. Normal host clock publication between calls is a separate condition |

## H. Implementation boundary, validation and remaining uncertainty

**Implementation-ready bounded inputs:** fresh ordinary HP0..10 and manager
modification mapping; full prepared object order/start/end/count, approach,
new-combo tests, slider F0bf7/F0bf8 counts, spinner F02a0, ordered prepared breaks
and format; calibrated r/B/G; independent V/H/A/fill state; consumed corrected
T/previousT, host dt/scale, current-spinner and late eligibility state; actual raw
result/provenance, record/global identities and failure/recovery gates.

**Outputs:** exact three-state updates, calibration return and snapshots, passive
subtraction, result delta, target consumed by E19 decision, and first latch or
recovery boundary. Do not normalize to one health field or fit constants from
visible bars. Record health observations separately from displayed smoothing.
Unsupported lifecycle/record swaps and exceptional nonfinite values must stay
explicit; this is not a complete all-mode/native HP implementation or a Work2 fix.

**ENGINEERING handoff:** a future health implementation can evaluate the bounded
calibration on private health state with the recovered trial HP dispatch, then
publish its resulting rate/gains to a fresh live health state. Do not inject the
trial64/8/8192/1028 notifications into the live score ledger as player actions.
The original instead calls its full consumer and later resets selected state;
a private calibration projection is an explicit engineering separation, not proof
that every original record/object/Play side effect is reproduced. F1529 snapshots
and any supported restoration use must be preserved deliberately. The original
live reset clears only its stated fields, not every field touched by calibration.

Focused extraction/checking status: deterministic repeat and61-method checker
PASS, including140 shared historical body hashes; pins/branches/slot resolution,
three sole state writers, calibration/drain/break/delta/recovery sites verified.
New tools compile in memory. Relevant retained E10/E17/E19/E24–29 evidence checks
are run separately as original-static regressions, not gameplay tests. Historical
blobs/modes/complete journal prefix, reconstruction/QA06/E20 and fixtures are
checked unchanged for publication. Exact regression outcomes are appended to the
journal after execution. No broad historical/GUI test campaign is represented.

Representative body SHA256:

- M1150 `389f0e15e00771308e8191e65444c89c55c571137ede52c5d60bcd25a7671039`.
- M1146 `4b560e5164207973fcf773a1a50326f2799895b2e0fb2f4cc3758dbe11138e9f`.
- M2972 `a82b8964101ca24825347eddb2ea4dabab1d708868bcddcdb33c0b5b2defd72e`.

**UNRESOLVED:** actual original calibration iteration counts/fields for imported
maps, native x86 Double behavior at marginal thresholds, active mod/policy/record
identity, break/object lists, clock/provider/interruption values, opaque resource
callbacks, alternative field writers, visual/audio health presentation and timing.
The full earlier E035 assignment was unavailable. The owner-reported score gap is
preserved as a deferred separate investigation. No E33/E34 correction follows.

Minimal future E20 addition **in this report only**, NOT RUN: capture original
process/map/audio/settings hashes; health receiver/manager/global/handler identity;
setup r/B/G, preparation threshold inputs/iterations/snapshots and final reset;
V/H/A/fill values before/after; exact consumed T/previousT/F235d/F2c71/F2c22;
F13c4 before drain and its late store, current spinner, break records/queue/marker;
actual judgement raw/delta/setter call; M226c operands, recovery counter and first
failed latch/recovery release. Preserve thread/callback order and measure tracer
overhead without extra clock calls. Natural events and forced probes stay separate.
No new E20 packet, game session or request for owner attendance.

The smallest remaining verification dependency is one authorized original trace
of ordinary calibration followed by passive drain and one judgement, with a break
and pause boundary as separate controls. No E036, R09 or other milestone is started.

# 3. Pseudocode

Independent **RECONSTRUCTED, non-executable** descriptions of original cooperation.
Keep original helper calls/exception boundaries and separate reads. These are not
an executable HP replacement, score simulator or claim of measured gameplay.

```text
fresh ordinary setup:
    B=1; G=1                              // handler constructor
    create original T056a component       // fill=true, fillRate=.02, resources
    r = original calibration()            // M1150
    component.r = r
    if original global bit256: component.r *= .75
    run original base fill setup          // does not replace passive r
    retain remaining setup
    later reached record setup/reset -> M297d: H=200; A=200; V=0

ordinary calibration:
    rate=.05
    capture original HP for thresholds L,U,E,W through four Phi calls
    repeat:
        retry=false
        reset original health, combo, record and bookkeeping
        prevEnd=first.start-approach; breakIndex=0; lowGroups=0
        for each indexed full-list object while no retry:
            excluded=0
            if next break fully lies in [prevEnd,object.start]:
                excluded = modern ? break.end-prevEnd : break.end-break.start
                breakIndex++                 // once, not while
            M297a(rate * Double(object.start-prevEnd-excluded))
            prevEnd=object.end
            if original H>L/unordered continuation fails:
                rate*=.96; retry=true; leave object loop
            durationLoss=rate*Double(object.end-object.start)
            lostBelowFloor=Max(0,durationLoss-fresh H)
            M297a(durationLoss)
            slider: invoke original raw64 (repeatList.Count+2) times,
                    raw8 (schedule.Count-repeatList.Count-1) times
            spinner: invoke original raw8192 preparedRequirement times
            if lostBelowFloor>0 and H-lostBelowFloor<=L (original ordered tests):
                rate*=.96; retry=true; leave object loop
            if last indexed object or next original new-combo test:
                invoke original raw1028
                if H<U:
                    lowGroups++
                    if lowGroups>2:
                        B*=1.07; G*=1.03; retry=true; leave object loop
            else: invoke original raw1024
            retain bookkeeping; object.F1529=fresh H
        if not retry and H<E:
            rate*=.94; B*=1.01; G*=1.01; retry=true
        perObject=(fresh A-200)/Double(fresh stored object count)
        if not retry and perObject<W:
            rate*=.96; B*=1.02; G*=1.01; retry=true
    until not retry
    retain optional original special-receiver suffix
    return rate
```

```text
normal reached Play pass:
    retain clock/input/readiness/pause/handoff gates
    select current from old candidates; refresh candidates
    process optional component/result; update objects and their whole results
    if ruleset initialized:
        original prefix hook
        if health component exists and prior stored F13c4:
            loss=(rate/1.0)*Double(unchecked(fresh T-fresh previousT))
            loss *= original current-spinner flag ? .25 : 1
            M297a(loss)
        retain base SuddenDeath/miss-count check
        original M2972 smooths V using its own flags/time sources
        retain original suffix helpers
    retain subsequent Play work
    update break marker/queue
    F13c4 = original late time/provider/marker/q/queue predicate
    retain E27 finalization and E28 recovery/departure boundaries
```

```text
on reached scoped judgement (nonzero raw):
    retain original prefix, dispatch/masks and state/combo work
    delta = original HP arm(raw, fresh HP, calibrated B/G, manager state)
    optional combo-enabled negative/SuddenDeath -> M2979(0)
    retain original observer/presentation hook
    if ordered delta>0: M297b(delta)
    else: M297a(-delta)
    retain record/observer helpers
    original M226c consumes fresh H:
        positive/unordered target -> return
        original continuation veto -> return
        original recovery available -> M0acc(false)
        otherwise original cleanup/queue -> first F136a=true store

M297a(x): retain fill guard; A=Max(0,A-x); H=Max(0,H-x)
M297b(x): retain fill guard; A=A+x; H=Max(0,Min(200,H+x))
M2979(x): H=Max(0,Min(200,x)); A=x
M297d(): H=200; A=200; V=0

UNRESOLVED: original runtime operands/interleavings, alternative policies,
rendered HP/audio and general-map equivalence. Scoring discrepancy stays deferred.
```
