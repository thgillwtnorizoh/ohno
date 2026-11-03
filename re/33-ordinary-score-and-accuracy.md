# 1. Result

Expedition033 — Ordinary Score & Accuracy Record Accounting. Work1,
2026-11-03 (Asia/Bangkok). Base/current canonical main:
`0806ac69ea63b3f2ecda9ca2b65a718bc8daefee` (QA05). Research only; no R08 or QA06.

Recovered a **bounded legacy ordinary result ledger**, its hit-count accuracy
getter, current/max/snapshot combo relationship, record initialization/reset,
score-factor arithmetic, HUD consumers and finalization validation. The numeric
update formula is established for the plain result identities below. Automatic
reproduction of every map's multiplier/density metadata, high-bit decorations,
ScoreV2 and external callback effects is **not** claimed. R08 can implement the
ledger and exact finite reference cases while making those remaining inputs
explicit. Existing practice `HitValueTotal` is not original numeric score.

Read AGENTS.md, standing contract (repository/attachment identical), cumulative
journal, E11–12/E17–19/E23/E25–27 and QA05. Inspected only the requested session
accounting interface under reconstructed/, without modifications. Original EXE
read-only on Linux; no original CLR/game/debugger/GUI/input/OBS/recording.
E20 remains incomplete; all17 runtime cases NOT RUN — WINDOWS HOST REQUIRED.

Pinned actual-byte hashes, independently enforced by extraction/checker:

- Archive: `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`.
- osu!.exe: `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.

Stable40/file-version1.3.3.8 self-description remains unauthenticated as an official
dated/unmodified release. Same E04/E05 Python readers (dnfile0.18.0,
pefile2024.8.26,dncil1.0.2), no new decompiler or accounting simulator.

Added this report, `tools/score_accounting_evidence.py`,
`tools/check_score_accounting.py`, `evidence/33/score-accounting.json` and append-only
contexts.md entry. Evidence supplies31 selected methods/signatures/owners/body
hashes, field signatures, code-relative branches/operands, scoped field inventories
and virtual candidates. One narrow direct-xref scan is justified specifically to
close mutable F0605 multiplier writers, F153b accounting-enable writers and
score/accuracy getter consumers:19659 decoded bodies,1739 zero-RVA,0 errors.
It is not a whole-project field survey; all other inventories are selected-body
scope. Full disassemblies, original bytes/assets/private dumps are not published.

Actual validation: repeated extraction byte-identical; focused checker PASS,
41 selected historical method-hash occurrences agree across retained E12/E18/
E19/E23/E25/E26/E27 evidence. New Python tools compiled in memory; diff whitespace,
exact prior journal prefix and historical blobs/modes preserved. No old expedition
campaign, replacement ledger execution or original-runtime test. Remote commit,
exact parent and complete blob/mode verification supplied in final handoff.

Reproduction (same reader dependencies as E32):

```sh
PYTHONPATH=/path/deps python re/tools/score_accounting_evidence.py /private/osubackup.zip > /tmp/e33-a.json
PYTHONPATH=/path/deps python re/tools/score_accounting_evidence.py /private/osubackup.zip > /tmp/e33-b.json
cmp /tmp/e33-a.json /tmp/e33-b.json
python re/tools/check_score_accounting.py /tmp/e33-a.json
```

# 2. What they found

## Player-level interpretation and confidence

A hit's displayed quality, component feedback, combo and accumulated numeric score
are separate. In the plain legacy path, whole-object positive results can receive
a bonus based on combo **before that result increases combo**. Slider head/ticks/
terminal and spinner progress events have their own amounts and combo policies.
Only the whole-object 50/100/300/miss counters enter ordinary accuracy; component
amounts do not become extra accuracy hits.

**CONFIRMED STATIC** means original instructions/metadata below. Formula aliases,
ledger semantics and conditional examples are **RECONSTRUCTED**. No original
runtime numbers, HUD pixels, score screen, sound or universal policy is observed.
**UNRESOLVED** and **ENGINEERING** boundaries appear beside the handoff.

All offsets hexadecimal, first CIL opcode-relative, headers excluded.
Mxxxx=MethodDef0600xxxx; Fxxxx=FieldDef0400xxxx; Txxxx=TypeDef0200xxxx.
Exact signatures/RVAs/body hashes are in JSON, not abbreviated invented names.

## A. Receiver, record identity and legacy admission

Ordinary T0253.M114a (0600114a, signature20020111922012930c) calls base
T0186.M0ad2 (06000ad2, same signature) at0022. Its optional M2282 prefix remains
conditional on object.F252e/evaluated/nonzero raw, not discarded as pure. Base
raw0 returns at0003. Nonzero captures object.F153b into local4 at0011–0016;
F05f6-dependent helpers precede local dispatch. Exceptions can interrupt this path.

F153b is Boolean on common T04c3; M249b:001d–001f initializes true. Narrow writer
inventory also finds special M20eb:013a/0269 and M21b2:01d1; those paths are not
ordinary defaults. This flag is not the evaluated flag or combo-enable Boolean.

Global Play.F13c3 and handler.F05f3 both reference T059e records. M0ace:0001–0006
copies the global reference into the handler, then invokes its virtual reset
M2b62 at0012. They can subsequently diverge (E23/E29). The ledger uses the
**handler** record; HUD M2273 uses `this.virtual M2274()`, base getter returning
global F13c3. Do not silently equate these separately read identities.

For base ordinary record, M2e1c:0034–0047 selects T059e/M2b3e when M2234 is false;
otherwise T0934 has different overridden record getters. M2234 checks bit8388608
and source/recorded state; fresh live no-mod/no-source selects false. E23's
M229d/factory enable separately controls F198e (ScoreV2 accumulator). Fresh ordinary
no-mod/live, valid small map metadata below the automatic7000-object gate, no
special Play override/reuse/restore can leave F198e null. This is a supported
**conditional static path**, not proof of every installation's runtime selection.

At M0ad2:1306–1312 the late record block is reached when captured F153b is true
or the raw argument's signed-positive `(raw &8388608)` permits it. Local3 (combo)
is not this admission gate. Handler F198e is read at131d; null branches at1326
into legacy1394. Non-null follows M3c57/ScoreV2 and is excluded from the formula
here. Current mod bits alone do not identify presence after reuse/restore (E23).

## B. Ledger fields, reset and immediate readers

| Field / owner | Signature / storage | Established role and original sites |
|---|---|---|
| T059e.F1985 |0608 / Int32|Legacy numeric score; M2b4c:0002 setter; M2b4b:000b getter when F198e null |
| T059e.F196a |0607 / UInt16|300-family whole-result count; M0ad2:08a2–08aa for raw1024 |
| T059e.F1969 |0607 / UInt16|100-family count;0830–0838 for raw512 |
| T059e.F196b |0607 / UInt16|50-family count;078a–0792 for raw256 |
| T059e.F196e |0607 / UInt16|Whole-result miss count;049d–04a5 for raw−131072 |
| T059e.F196c/F196d |0607 / UInt16|Additional decorated-result counts; not included by base M2b44/M2b42 accuracy |
| T0892.F2674 |0608 / Int32|Current combo, separate object handler.F05f2; M40f0 getter/M40f1 setter (E18) |
| T059e.F1975 |0608 / Int32|Maximum-like combo, max(old,new current) at M0ad2:11d3 |
| T059e.F198a |0607 / UInt16|Current-combo snapshot at11e9–11ea; reset to0 at128f |
| T0186.F0605 |060d / Double|Mutable numeric-score multiplier K, prepared at M0ab7:0074 |
| object.F153d |0608 / Int32|Branch-local base amount stored100c **before** ordinary combo scaling |
| T059e.F1986 |060d / Double|Separate accumulator reset M2b62:0027, +=local7 at13b6; not ordinary accuracy or F1985 |

Fresh managed numeric fields default0. Constructor M2b3e does not assign F1985 or
hit counters; its other fields/resources remain original. M2b62's legacy arm
sets score0 via virtual M2b4c at0018, F1986=0 at0027, snapshot0 at002e, max0 at0035,
then all selected hit counters0 at003c–0066 (F196e appears twice). Non-null policy
resets its accumulator instead. This is not an atomic rollback or a global
current-combo reset. M0ace's later conditional M40f7/health reset hooks are separate;
R08 must distinguish a fresh ledger/session from arbitrary record restoration.

Direct relevant result writers are M0ad2 branches; reset M2b62 and fresh defaults
are established. Scoped constructor/field sites and getters are in JSON; parsed/
restored record constructors and unrelated persistence writers are not asserted
absent. Legacy score setter M2b4c is a plain Int32 store, no cap or score animation.
M2b4b instead calls virtual M3c55 if F198e is present; writing F1985 does not force
that alternate getter to expose it.

## C. Plain result table and combo-scaled score

The following table requires ordinary mode0, base ordinary record, null F198e,
plain low raw values, stable record/object references, accounting enabled and
normally returning helpers. Result quality aliases are the previously recovered
ones, not new literal enum metadata names. No high decoration/mod policy is
silently folded into these rows.

| Provenance / raw | Base amount b / local5 scaling | Hit-count write | Combo consequence |
|---|---|---|---|
| Circle / spinner final / slider aggregate1024 |300 / true|n300++ if captured flag|Positive: +1 unless object has Slider2 |
| Same whole-result families512 |100 / true (ordinary T0253)|n100++|Same positive rule |
| Same families256 |50 / true|n50++|Same positive rule |
| Whole-object miss−131072 |0 / false|nMiss++|Reset to0, including a slider parent miss |
| Positive slider head64 or repeat-like component64 |30 / false|None|+1 |
| Interior component8 |10 / false|None|+1 |
| Due-prefix terminal-coded success128 |30 / false|None|+1 |
| Head/interior failure−262144 |0 / false|None|Reset0 |
| Terminal processed-count failure−524288 |0 / false|None|Common combo block bypassed |
| Spinner component4096 |0 / false|None|Bypassed |
| Spinner component8192 |100 / false|None|Bypassed |
| Spinner component16384 |1100 / false|None|Bypassed |
| raw0 |No consumer work|None|No consumer work |

Same64/−262144 does not erase head/component provenance. Result code128 can
represent an earlier overdue item (E22), not a new component identity. Spinner
raw16384 is bonus-associated, but the ledger addition1100 is not a claim of a
rendered “1100 bonus” label or complete bonus payout machinery.

M0ad2 initializes local5=false at01fc–01fd. Whole positive branch sets it true
(e.g.0740–0741); its local3 is `NOT object.HasType(Slider2)` at0735–073f and
corresponding branches. The miss branch does not set local5. Raw4096/8192/16384
explicitly clear combo-enabled local3, while terminal failure does so at0420.
Branch b values were already E16–E26 object-local evidence; E33 connects them to
an actual legacy ledger write, rather than calling F153d itself total score.

When local5 true and global mode M2232 is ordinary0, default arm10e7–110b computes:

`delta = unchecked(b + conv.i4(Double(Max(0, unchecked(comboBefore−1))) * Double(signedIntDiv(b,25)) * K))`.

The b/25 division is **integer before Double conversion**; 50→2,100→4,300→12.
There is no integer division by25 after the entire product. `conv.i4` truncates
finite in-range products; invalid/out-of-range floating conversions are a CLR
runtime boundary, not invented saturation. Int32 subtraction/additions are
unchecked. No local5 scaling means delta=b. This is not the Taiko-specific
integer arithmetic at102e or Mania branch10a8.

**Mutation order:** result counters first (UInt16 via add/conv.u2), object.F153d=b
at100c, calculate delta using old combo at10ef, then mutate current/max/snapshot
combo at11c7/11d3/11ea (or reset1287/128f), retain original feedback/health helpers,
then admitted legacy score update1394–13a2:

`record.M2b4c(unchecked(record.M2b4b() + delta))`.

The getter/setter stack holds the same record for that add; the subsequent
F1986 write rereads handler.F05f3 and adds local7 (initialized0, ordinary mode
control leaves it0). Helpers can throw before/after counters/combo but before
score write; there is no local transaction or rollback. Maximum and snapshot
tracking do not supply the old combo operand to the bonus formula.

**Flagged-result boundary:** M0ad2's positive masks−12648449/−14745601 affect
local dispatch, while later logic also reads the original raw argument. At0fd7
mask12648448 can adjust b or choose an extra−1 combo operand; at1113 mask12582912
can adjust delta/reuse prior object amount, and mask8388608 can disable combo.
Mask2162688 can update extra counters/double the local amount at1145–1199.
These branches are captured in original evidence, but their upstream provenance
and complete ordinary manager-decoration reachability are **UNRESOLVED for R08**.
Do not discard those flags and claim exact scores for every map. R08's honest
initial scope may accept the plain table only and expose unsupported flag cases.

## D. Multiplier provenance and explicit remaining operand

T0186 constructor M0a9c:0028 sets F0605=1.0, but this is **not** the normal final
map multiplier. Ordinary T0253 inherits setup M0ab7 (slot metadata plus ancestry).
It calls map.M1bc4 at0048, converts Int32 result to Double, multiplies by
`M156b(handlerRecord.mods, globalMode, manager.map)` at006e and stores K at0074.
For ordinary mode0/mod bits0, M156b starts1.0, bypasses its bit tests, returns
Max(0,1.0)=1.0 at0354–0363. Nonzero modifier matrix is out of scope.

Map T03ad.M1bc4 (06001bc4, signature200008) is recovered exactly:

`D = conv.i4(Math.Round(Double(((((F0b54+F0b55)+F0b53) + Clamp((Single(F0ff2)/Single(F0fe5))*8f,0f,16f))/38f)*5f)))`.

Preserve Single-category arithmetic and the Double Math.Round overload at0046
before integer conversion004b; do not substitute integer density division or a
round-away-from-zero policy. F0b54/55/53 are existing map difficulty inputs.
F0ff2 and F0fe5 are Int32 metadata operands; their **active values and full
loading/cache provenance are the remaining chart-initialization dependency**.
Private bounded inspection found M1bc1 initializes them at0030/0037, increments
F0ff2 at0f81 while parsing rows, and derives F0fe5 using original integer locals
and division1000 at1098; metadata-loading M1bf2 also writes them. This expedition
does not assign a universal object-count/drain-duration interpretation or rebuild
those parsing/cache paths. Supplying D=1 or D=3 merely because the chart is simple
would be an **engineering preset**, not recovered original configuration.

The narrow EXE inventory finds only three direct F0605 writers: constructor
M0a9c:0028, base M0ab7:0074 and T094e.M4608:0043. The last is a distinct ruleset
setup using T011f/F0499 and F297e; do not import it into ordinary T0253. Reuse,
restoration or callbacks can still alter surrounding state. Under the finite
ordinary no-mod setup K=Double(D), with D the actually supplied map getter result.

## E. Accuracy, HUD and finalization data

Base T059e.M2b44 (06002b44,200008) adds n50+n100+n300+nMiss as signed Int32;
M2b45 omits misses. M2b42 (06002b42,20000c) returns Single accuracy fraction:

- Call M2b44; if total<=0 return1f at0009–000e.
- Numerator uses Int32 `50*n50 +100*n100 +300*n300`, then conv.r4 at002f.
- Call M2b44 again; multiply Int32 total*300 then conv.r4 at003c; Single division003d.

Thus four UInt16 hit counters are the base accuracy state. No stored weighted
score accumulator is consulted. Combo, b, numeric score F1985, slider components,
spinner P/C and extra F196c/d do not enter this getter. Separate total reads are
not an atomic snapshot. UInt16 stores wrap modulo65536 before future arithmetic;
Int32 operations are unchecked. Fresh zero totals produce100% as a getter policy,
not a pre-existing successful judgement. Alternative record overrides are listed
in metadata and excluded from this base-record claim.

Play M2273:000c calls virtual M2274 (base returns fresh global F13c3), then score
getter M2b4b at0011, passes its Int32 to HUD receiver T03f1.M1dd2 at0016, storing
F10f3 at0002. It separately obtains the record again, calls M2b42 at002c,
multiplies by100f at0036, passes Single to M1dd3:0037. That receiver converts to
Double and `Math.Round(value,2)` at0004, storing Double F10f4 at0009. This is a
verified HUD input/state boundary, not evidence of actual digits rendered.

E27's M2276 invokes handler validation M0aa5 on its reached branch at00e7.
M0aa5:0001–002e sums handler n300/n100/n50/nMiss and compares with manager.F17fc
at003a. It does not count successful components as extra objects. Completion,
record validity and departure remain separate; no results screen is reconstructed.
The global/handler record retains numeric score, hit counts, max/snapshot combo,
mods and map references. E27's lifecycle/record reset branches still apply;
this does not promise that arbitrary stale records are final snapshots.

## F. Conditional static reference scenarios / R08 handoff

All examples below assume fresh zero record/combo, ordinary base record, enabled
accounting, null policy, no modifiers/high flags, finite in-range arithmetic,
stable inputs and normally returning helpers. Let K be the prepared multiplier.
These are **RECONSTRUCTED static predictions, NOT RUN original observations**.

| Event sequence | Verified bounded prediction |
|---|---|
| Circle1024 then circle1024 | Score300 then600; combo1 then2; n300=2; accuracy1f. Second hit uses comboBefore1, so no bonus. |
| Circle1024 then circle512 | Score400, combo2; n300=1/n100=1; Single accuracy400/600 (HUD approximately66.67). |
| Two300s then100 | Score700+conv.i4(4*K), combo3; numerator700/denominator900 for accuracy, independent of K. |
| Circle300 then whole miss | Score remains300; combo0/max1; n300=1/nMiss=1; accuracy.5f. |
| Slider head64, final component128, best parent1024 | Score30→60→360+conv.i4(12*K); combo1→2→2. Only parent increments n300; accuracy1f. Requires a valid no-interior schedule and successful head/component. |
| Slider head64, interior failure−262144, terminal128, positive parent512 | Score30→30→60→160; combo1→0→1→1; n100=1, no nMiss from checkpoint failure; accuracySingle100/300. A natural two-component schedule with head+terminal successful gives middle aggregate. |
| Spinner C1..C16, N15, then best final1024 | C1 raw0; eight even observations8192 add800 total, seven odd4096 add0. Final adds300; score1100, combo1, n300=1. No bonus-associated predicate reached before C19. |
| Spinner raw16384 admitted | Legacy score+1100, no combo or whole accuracy count; full visual bonus/payout meaning remains excluded. |
| Terminal failure−524288 | Delta0; combo preserved through this component block; later parent result is independent. |
| Plain F153b=false artificial/special object | Counter/legacy-write gate skipped, but result-local/combo work may already occur; no generic “ignored event” promise. |
| ScoreV2 receiver becomes non-null | Getter/consumer leave this ledger; do not apply plain F1985 formula as the selected policy. |
| Exception after count/combo before1394 | Partial earlier mutations may remain; no score commit guaranteed. |

**ENGINEERING R08 minimum interface:**

- Maintain separate Int32 legacy score and current/max combo; UInt16 n300/n100/
  n50/nMiss, optional extra counters and combo snapshot. Keep practice HitValueTotal
  separate or replace its uses deliberately; it is not total score.
- Input an admitted raw result with explicit provenance (whole circle, slider
  head/component/parent, spinner component/final), object Slider2/accounting flags,
  policy presence and prepared K. Preserve event order and combo-before snapshot.
- Output numeric delta/score, hit counts, accuracy fraction, current/max/snapshot
  combo and unsupported-policy/flag status. Do not silently drop a source event or
  attribute aggregate accuracy to a head/tick.
- Implement the plain legacy table, bonus formula, hit-count getter and the finite
  cases above as a useful foundation. Until map metadata provenance is resolved,
  receive K explicitly and label any reconstructed-chart preset as ENGINEERING.
  Reject/report unsupported policy/high-flag states instead of claiming original
  cross-map scoring. Implementing ScoreV2 is not authorized by this handoff.

**Smallest next bounded dependency:** recover the producer/cache meaning of
M1bc4's F0ff2/F0fe5 and ordinary manager high-decoration provenance if R08 requires
automatic original-style chart scores. Actual original configuration/record/HUD
reads still require E20's owner-authorized Windows setup; add only score/K/count/
combo-before/policy/flag/getter operands to its future checklist. No E20 packet
change or runtime campaign was initiated. Stop E33, no R08 or QA06 implemented.

# 3. Pseudocode

**RECONSTRUCTED finite plain legacy accounting; not executable reference code:**

```text
retain original M114a/F05f6 prefix and exceptional boundaries
if raw==0: base consumer returns
capture object accounting flag; dispatch plain raw identity
conditionally increment corresponding UInt16 whole-result counter
choose b, comboEnabled and wholePositiveScaling from original branches
store object-local amount b
if wholePositiveScaling and ordinary mode:
    delta=b + conv.i4(Double(Max(0,comboBefore-1))*Double(Int32Div(b,25))*K)
else delta=b
perform original enabled combo increment/max/snapshot or reset
retain original feedback/health/helper continuation
if record-admission condition:
    policy = fresh handlerRecord.F198e
    if policy non-null: original policy dispatch (outside this ledger)
    else:
        record.score = unchecked(record.scoreGetter() + delta)
        separately reread handler record; F1986 += original local7

accuracy getter on base ordinary record:
    if first fresh total<=0: return Single(1)
    numerator=Int32(50*n50+100*n100+300*n300)
    denominator=Int32(laterFreshTotal*300)
    return Single(numerator)/Single(denominator)

HUD:
    send fresh record score to original score receiver
    send separately read Single(accuracy*100f) to original two-decimal receiver
```

**UNRESOLVED** automatic chart-density initialization, upstream high-result flags,
actual runtime record selection, opaque helpers and native precision are explicit
inputs/boundaries, not replaced by invented score rules.
