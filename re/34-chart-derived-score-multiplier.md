# 1. Result

Expedition034 — Original Chart-Derived Legacy Score Multiplier. Work1,
2026-11-03 (Asia/Bangkok). Base/current main:
`cb73b445aa20ab39d0ed886b46210b37715a0a0a` (R08).
Original-binary static research only; no reconstruction change, QA06 or E20 run.

Recovered the fresh metadata reader's two operands, cache replacement writes,
ordinary setup ordering and preserved Single/Double multiplier expression.
**Conditional fixture predictions: Mixed R07 K2; Joined Bézier K3.** Mixed can
reconcile with the reported2392 under explicit event assumptions; the supplied
Joined fixture predicts534 rather than reported582. This is retained as an
unresolved chart/configuration/event comparison, not repaired by choosing K4.

Read AGENTS, full standing contract (same repository/attached bytes), cumulative
journal, E33 in full/tool/evidence, E04/E05 and relevant E17–19/E23/E25–27 boundaries;
R08 report read only for its explicit K1 interface. Prior conclusions are not
edited. Current R08 corrective implementation was already on main before this
expedition; publication leases against that exact head. Work1 makes no code fixes.

Actual archive/EXE SHA256 rechecked through extraction and checker:

- ZIP `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`.
- EXE `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.

Stable40/file-version1.3.3.8 self-description remains unauthenticated as an official
dated/unmodified release. Metadata/CIL inspected read-only on Linux with the same
pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 readers, installed privately under
/tmp/ohno-static34. No game/CLR execution, debugger, window/input/OBS/capture.
All17 E20 original-runtime cases remain NOT RUN — WINDOWS HOST REQUIRED.

Added this report, `evidence/34/chart-score-multiplier.json`,
`tools/chart_score_multiplier_evidence.py`, `tools/check_chart_score_multiplier.py`
and one append-only contexts.md entry. Public evidence contains21 method metadata/
body hashes and selected token/constant/arithmetic/branch sites, not original
bytes, full disassembly, proprietary assets or private dumps. A narrowly targeted
EXE pass inventories just F0ff2/F0fe5 accesses and five helper-call tokens;19659
bodies decoded,1739 zero-RVA,0 errors. No general field/decompiler campaign.

Actual verification: two byte-identical extractions; focused checker PASS for21
methods/six exact operand stores/parser/cache/setup/float/rounding sites;22 shared
historical E04/E05/E33 body hashes agree. New Python tools compile in memory;
whitespace, prior journal prefix, historical tracked blobs/modes, QA/E20 and all
implementation files preserved. No historical test campaign or gameplay test.
Remote publication SHA/parent/full-tree verification supplied in final handoff.

```sh
PYTHONPATH=/path/deps python re/tools/chart_score_multiplier_evidence.py /private/osubackup.zip > /tmp/e34-a.json
PYTHONPATH=/path/deps python re/tools/chart_score_multiplier_evidence.py /private/osubackup.zip > /tmp/e34-b.json
cmp /tmp/e34-a.json /tmp/e34-b.json
python re/tools/check_chart_score_multiplier.py /tmp/e34-a.json
```

# 2. What they found

## Player-level meaning and evidence classes

For this original metadata reader, density depends on **object rows per truncated
second between first and last row starts, minus all parsed break durations**.
It does not use the last slider's prepared end, the last spinner's end, audio end,
number of ticks or actual time spent holding a slider. A one-object map therefore
reaches the zero-denominator case even when that object lasts several seconds.
Its positive count produces infinity, which the original clamp caps at16.

**CONFIRMED STATIC** identifies original instructions/metadata below.
**RECONSTRUCTED** labels semantic aliases, arithmetic projections and fixture
predictions. **OWNER-REPORTED** totals are supplied evidence, not our observations.
**UNRESOLVED** includes actual original chart bytes/settings/event streams/cache
selection/precision. No instruction extraction is called a runtime match.

Mxxxx=MethodDef0600xxxx, Fxxxx=FieldDef0400xxxx, Txxxx=TypeDef0200xxxx.
All offsets hexadecimal CIL-relative, headers excluded. JSON records exact
signatures, owners, RVAs, body hashes, branches and external member signatures.
Key identities:

| Method | Owner / signature | Scoped role |
|---|---|---|
| M1bc1 |T03ad /200002|Fresh metadata StreamReader, returns Boolean |
| M1bf1 |T03ad /20010112991c, virtual|Binary metadata read wrapper |
| M1bf2 |T03ad /20010112991c|Binary metadata field population |
| M1bc4 |T03ad /200008|Int32 map score factor D |
| M2d7d |T05e6 /00030c0c0c0c|Single clamp(value,min,max) |
| M0ab7 |T0186 /200001, virtual|Prepare Double F0605 multiplier |
| M156b |T02e5 /00030d11ad5c11ad60128eb4|Modifier/mode factor, no-mod ordinary1 |
| M223b |T047b /200001|Ordinary Play loading/setup ordering |

Representative complete-body SHA256:
M1bc1 `680dab0bc73f7b4d9cfb5922c58194b840a49d2f15c5cfc6511bd921f0ec3f7f`;
M1bf2 `24b9b1cd0c9ba1bf5a5ec7cb7d023f9d30e409ef3b137f84b794397c542e88fe`;
M1bc4 `1c404480174a195bb6a37524d78bcafd375ed21789b347ebbdb94fae45279a43`.

## A. Fresh metadata operands: row provenance and units

F0ff2 and F0fe5 are **instance Int32 fields**, signature0608, on map T03ad.
The exact direct managed writer inventory contains only six stores:

| Method:offset | Value source |
|---|---|
| M1bc1:0030 |F0ff2=0 before stream opening |
| M1bc1:0037 |F0fe5=0 before stream opening |
| M1bc1:0f81 |unchecked F0ff2+1 for each reached HitObjects row |
| M1bc1:1098 |signed Int32 `(lastStart−firstStart−breakSum)/1000` |
| M1bf2:01ed |BinaryReader.ReadInt32 into F0fe5 |
| M1bf2:0369 |F0ff2 = Int32 sum of restored UInt16 circle/slider/spinner counts |

No direct field-address/token mutation is present in the targeted access inventory.
This does not prove absence of external/reflection/concurrent mutation.

### Header/section and HitObjects path

M1bc1 gets a stream through M1c04→M1c05 at012c, constructs StreamReader at016c.
A null stream returns false after earlier resets (0133–015e); successful opening
sets F0fcd=true. The original format header is read separately, with version
stored F0fd1 at021a. Section names are parsed via Enum.Parse at02e8 using original
metadata T0b24 `osu.GameplayElements.FileSection`: HitObjects=64, Events=32,
Difficulty=128, TimingPoints=16. These names/values are independently readable
metadata, not guessed from switch ordering. Nonempty lines are trimmed at025d;
section64 bypasses the generic colon key/value split at0282–0286.

Local aliases in this report: firstStart=Int32 local0c, lastStartText=String local6,
lastEndText=String local7, breakSum=Int32 local0d. At01ac–01c4 initialize firstStart,
lastStart/end parsed defaults to−1, text strings empty, breakSum0.

For each HitObjects row (0e83 onward):

1. Split on comma with maximum7 fields at0e88. If firstStart still−1, read column2
   through M09e4(Double parser with limit2147483647, validation=false), conv.i4,
   store firstStart at0eaa. This happens before the type switch, not after sorting.
2. Convert column3 to Int32 at0eb3, AND **139/0x8b** at0ebd. New-combo bits are
   discarded; it retains base1/2/8/128 bits. Branches recognize equality1,2,8,128.
3. Circle1: UInt16 F0fd9++, lastStartText=column2 and lastEndText=same (0ee7–0f03).
   Slider2: UInt16 F0fda++, same start/start texts (0f05–0f21). No slider duration,
   control point, repeat count or tick calculation is performed here.
4. Spinner8: UInt16 F0fdb++, startText=column2, endText=column5 (0f23–0f44).
   Hold128: UInt16 F0fda++, take the part before ':' in column5 for **both** texts
   (0f46–0f76). That shared metadata branch is not ordinary spinner gameplay.
5. Rejoin0f78 and increment F0ff2 once, even if the masked type matched none of
   those branches. Thus fresh row count is not universally identical to the sum
   of recognized UInt16 counts. Valid ordinary fixtures use recognized1/2/8.

There is no max-start search, ordering sort or interval-overlap union in this
path. The last recognized row overwrites lastStartText, regardless of chronology.
Overlapping/simultaneous objects still contribute rows if the metadata parser
reaches them. Head/tick/repeat/spinner-progress events are not counted.
Malformed conversions/indexing can throw into the existing loop catch at0f9d;
it reports then rejoins later metadata work, preserving partial mutations.
Do not interpret a row parse failure as transactional rejection or a safe result.

### Events and the denominator

Events section dispatch reaches0d46. First character uppercased, minus '0',
switch at0d5a selects **numeric event2**→0db2. It splits by comma and performs:

`breakSum = unchecked(breakSum + Int32.Parse(column2) − Int32.Parse(column1))`

at0dc5–0de1. The exact stack order is `oldBreakSum + (end−start)`; subtraction
happens before addition. No intersection with object range, positivity check,
overlap merge, clipping or ignored-gap threshold appears. This is RECONSTRUCTED
as accumulated break duration from the actual event2 operands, in milliseconds.

After the reader loop, parse nonempty lastStartText via M09e4/conv.i4 at1063/1068.
The ordinary first/last time parser conversions truncate finite in-range fractional
values toward zero. At1089–1098, perform **two unchecked Int32 subtractions**,
then signed integer division by1000 and store F0fe5. Thus F0fe5 is whole seconds,
truncated toward zero, and can be0 or negative. It is not a floating duration.
The separate following109d–10ac path uses lastEndText to store **F0fe6**, while
F1008 later receives parsed lastEnd at10c6. Neither replaces F0fe5 in M1bc4.

Only the first-start initializer, HitObjects branch and numeric-event2 branch
write these local aliases. TimingPoints processing does not update them. BPM,
inherited SV, slider length/repeats, spinner end, and audio length therefore do
not directly enter these two operands, holding row starts and difficulty fixed.
Those inputs can still change component events and total score independently.
Changing their values is not proof of unchanged gameplay score.

## B. Difficulty and cached metadata

M1387 initializes base difficulty fields F0b53/F0b54/F0b55 to Single5 at0011/001c/
0027. Existing E05/gameplay aliases are CS/HP/OD respectively. For format>=13,
M1bc1 recognized Difficulty branches parse Single using F2c2b, clamp0..10 and store:
HP F0b54 at0b7d, ordinary-mode CS F0b53 at0c03, OD F0b55 at0c56. Older formats
use Byte parsing before conversion (not assigned modern fractional behavior).
Mania's CS range branch is separate and excluded.

Original string selectors remain M49b0 keys2064567252/2064567098/2064567051 for
these branches; their plaintext was not independently decoded or executed here.
Field-role aliases are RECONSTRUCTED from established consumers. Fixture
predictions assume the standard supplied HPDrainRate/CircleSize/OverallDifficulty
keys take those established branches. No novel settings identity is invented.
M1bc1 does not reset these difficulty fields to5 before every read; a fresh base
object defaults5, while omitted keys on reused/cached objects can retain old values.
All supplied fixture difficulty operands are explicit.

M1bf1 is a virtual binary-read wrapper, with an extra discarded ReadInt32 for
cache-version range20160408<=F1a06<20191106, then M1bf2:0021. Exceptions are caught
at0028 without a rollback. Generic cache invocation above this virtual boundary
is not reconstructed as a full database/serialization system.

M1bf2 restores recognized counts via ReadUInt16 at0092/009e/00aa into F0fd9/F0fda/
F0fdb; difficulty via ReadSingle into F0b53/54/55 at00d3/00df/00eb. F0fe5 is supplied
by **ReadInt32** at01e8–01ed, not recomputed from objects or timing points. Near
return, sum those three loaded UInt16 counts at0355–0369 into F0ff2. A complete
cache read can therefore replace parsed seconds and counts; it can disagree with
fresh row counts for unsupported rows or wrapped UInt16 counts. No cache version
or binary format is fabricated. Data supplied to this reader is a runtime input.

## C. Ordinary setup selects the consumed values

The concrete ordinary path retains E05's corrected T0253 handler/T00e8 manager,
not Taiko. In M223b:

1. Read selected map F13f8; call its M1bc1 at008e. The Boolean return is **popped**
   at0093, not used to prove success. Previously loaded F0fec is used for a separate
   mode-change check, not as a condition bypassing the metadata read.
2. Configure new record at00ff→M2258; bind selected map to manager through
   M28a2 at0182 (`F17ee=argument` at M28a2:0002).
3. Call M28f5(false,false) at018f. Its optional M1bc1 re-read at000f is guarded by
   first Boolean argument, so this ordinary call does not request that extra read.
   Normal object loading/gates may still abort before later setup.
4. At0265 invoke handler.virtual M0aaf, ordinary override M1140. Its prefix resource
   helper precedes base call0018→M0aaf; base0001→virtual M0ab7. Ordinary T0253
   inherits base M0ab7 (E33 slot evidence); alternate T094e setup is not substituted.
5. M0ab7:0042–004d calls **M2c63**, which freshly returns static **F1a0f**
   (T05c7.M2c63:0000), then calls M1bc4 and conv.r8. M2237:0001–0006 originally
   captured that getter into Play.F13f8; manager.F17ee was bound later. These are
   three reference reads, not one immutable snapshot. M2c64:0021 can replace
   F1a0f after its old-reference cleanup. The fixture chain requires identity
   continuity; a changed current global map could supply a different D than the
   earlier refreshed Play/manager map. No natural concurrent replacement is
   claimed. The rest of M0ab7 reads
   handler record modifier state and global mode separately, calls M156b at006e,
   multiplies and stores **Double F0605=K at0074**. E33 establishes no-mod ordinary
   factor1.0, so stable same-reference setup consumes K=Double(D).
6. The later E33 M0ad2 whole-positive score arm reads F0605 at1102. Constructor's
   earlier1.0 default is not the prepared chart result.

Normally reached successful fresh reading thus overwrites these two cached operands
before K preparation, **when the selected/current/manager map identity remains the same**. A failed/null/partially read stream does not establish valid
fresh metadata; no automatic retry/fallback to cached values is manufactured.
M1bf2 can replace fields on another reached cache read, but that alone does not
recompute already stored K; another setup must consume them. No ordinary callback
ordering or active mid-play cache replacement is asserted without evidence.

## D. Exact numeric computation and exceptional boundaries

M1bc4's existing E33 formula is preserved in **instruction order**:

```text
u = (F0b54 + F0b55) + F0b53                // Single fields, add/add
r = (Single(F0ff2) / Single(F0fe5)) * 8f   // conv.r4, div, mul
u = u + M2d7d(r, 0f, 16f)
u = (u / 38f) * 5f
D = conv.i4(Math.Round(Double(u)))         // conv.r8 only at0045
```

Exact sites: F0ff2/F0fe5 loads0015/001c; conv.r4 at001a/0021; div0022; ×8 at0028;
clamp call0033; final add0038; /38 at003e; ×5 at0044; Double conversion0045;
System.Math.Round(Double) MemberRef0a000398 at0046; conv.i4 at004b. Do not reassociate
as Double density or use integer count/seconds division. Standard CLR semantics
of this Round overload are nearest with midpoint-to-even; no round-away-from-zero
rule or original numeric runtime probe is substituted. Original x86 extended
intermediate precision remains an E20 dependency at marginal floating boundaries.

M2d7d checks NaN **bounds**, then ordered/unordered branch instructions:
0018 `ble.un.s→001c`, otherwise returns max;001e `bge.un.s→0022`, otherwise min;
0022 returns original value. With fixed finite bounds0/16:

- Positive infinity→16; negative infinity→0; finite values clamp normally.
- NaN input propagates through both unordered branches and returns NaN.
- Positive N/zero seconds produces floating +infinity, not integer DivideByZero.
- 0/0 produces NaN, reaching Round and an invalid/nonfinite conv.i4 boundary.
  No portable numeric D or universal gameplay outcome is assigned to that probe.
- Negative seconds can yield a negative term, clamped0. No denominator Max(1,...).

Count additions, break arithmetic and elapsed subtractions are unchecked Int32;
signed /1000 truncates toward zero, including negative spans. UInt16 recognized
count increments wrap before a future cache reconstruction; fresh F0ff2 counts
rows independently. Malformed/out-of-range M09e4 conversions, I/O errors and later
helpers preserve original exceptional boundaries, not synthetic corrected metadata.

## E. Conditional finite cases and supplied fixtures

All table cases are **RECONSTRUCTED static predictions**, assuming successful
fresh metadata reading, stated recognized rows/difficulty, ordinary no-mod/null
legacy policy and normally returning setup. None was imported or played here.
N=F0ff2; B=breakSum milliseconds; T=F0fe5 whole seconds.

| Finite control | Operands / multiplier prediction |
|---|---|
| Two circles1000/101000; HP0,CS5,OD5; no break |N2,T100,density.16; D=Round(10.16/38*5)=1. |
| Twenty ordinary rows spanning1000..6000; same difficulty |N20,T5,density32→16; D=Round(26/38*5)=3. |
| Two circles1000/5000; HP5,CS5,OD5 |N2,T4,density4; u19/38*5=2.5 exactly; Round→2 (even). |
| Same two circles but last4000 |T3,density about5.3333; D3. |
| Two rows1000/1999; HP0,CS5,OD5 |T0, positive-infinity density→16; D3. At last2000 T1 also density16/D3; last3000 T2 density8/D2. |
| Four rows spanning1000..5000; HP/CS/OD5 |N4,T4,density8,D3. Add break event2,2000,4000: B2000,T2,density16,D4. |
| Same starts with two overlapping breaks each1500ms |B3000 without union, T1. No overlap correction is implied. |
| Same starts with break-sum5000ms (artificial/out-of-range breaks) |T−1,density negative→0; HP sum15 gives D2. Not proof that gameplay accepts the malformed chart. |
| Sole spinner start2000,end5000 vs end10000 |N1,T0 in both; end changes F0fe6/F1008, not M1bc4 denominator. |
| Two rows in order start3000 then1000 (artificial unordered chart) |first3000,last1000,T−2; no max-time repair. |
| Timing/SV/slider length changes with same recognized row starts/count/difficulty |Same N/T/D; movement/ticks and total score can still differ. |
| Binary metadata counts1/2/1 and storedT25; HP0,CS5,OD5 |Restored N4,density1.28, D1; fresh parse can replace this before ordinary setup. |
| Empty N0,T0 getter probe |NaN path; D unresolved, not an observed playable empty-map result. |

Pinned repository fixture bytes at the E34 base:

| Fixture | SHA256 |
|---|---|
| mixed-r07.osu |`2d76f1c3e7f7fd76e65987f01dc4dc6651610214d543d0b6ac814fecbcbae4cf`|
| bezier-joined.osu |`2d844b472e2fd138e638dedac6a1fe2ce9e94164131ad258be0b97f162bde85b`|

Mixed R07: four rows at3000,5000,10000,15000; first3000,last15000,B0 gives
T12,N4. HP0,CS5,OD5; density=Single(4)/Single(12)*8f (about2.6666667),
u about1.6666667 before Round→**D2,K2**. Spinner end13000 and slider prepared end
are irrelevant to this metadata denominator. Timing beat1000 does not set T.

Joined Bézier: one slider row at3000; first=last=3000,B0,T0,N1. HP0,CS4,OD4;
density+infinity clamps16; u24/38*5 about3.1578947→**D3,K3**. Requested length500,
curve shape, traversal count and its end do not replace T0.

## F. Owner comparison reconciliation: one agreement, one unresolved difference

The November3 observations are **OWNER-REPORTED**, not new E20 instrumentation:

| Comparison | Reconstruction | Original stable | Common reported results |
|---|---:|---:|---|
| Mixed R07 |2236|2392|Four300s,100%,max combo7 |
| Joined Bézier |438|582|One300,100%,max combo5 |

Original screenshots identify `test2 - test2 [normal]` and `test3 - test3 [normal]`.
Exact original `.osu` bytes/settings/modifiers/cache/component events are absent.
Their equivalence to the pinned fixtures is **not established**. We did not open
or operate either game or independently inspect those screenshots in this task.

**Mixed conditional event calculation:** assume the supplied fixture, plain legacy
all300 outcomes, successful slider head, two interior successes and terminal,
matched spinner component contribution800, no high-decoration changes. Whole base
amounts total1200; slider head/components30+10+10+30=80. The whole bonus factors
are0 (first circle),48K (slider parent at comboBefore5),48K (spinner final at
comboBefore5),60K (last circle at comboBefore6). Thus:

`total = 1280 + spinnerComponentContribution + 156*K`.

With matched contribution800: K1→2236 and independently metadata-derived K2→2392.
That contribution is consistent with N15 spinner observations through C16 or C17
(E25 even8192 events8×100, odd4096 adds0) and a best final result. It is **not**
an observed event count inferred uniquely from accuracy/max combo. This is a
conditional reconciliation, not proof that K was the sole runtime difference.

**Joined conditional event calculation:** assume successful head30, three interior
successes3×10, terminal30, parent300 at comboBefore5, plain legacy flags. Then
`total=90+300+48*K=390+48*K`: K1→438, metadata-derived **K3→534**. Reported582 would
require K4 **under these same event assumptions**, or other accounting inputs.
It is not the prediction for the supplied HP0/CS4/OD4 fixture.

For example, a *different* original chart with HP5,CS4,OD4 and the same sole-row
structure has Round(29/38*5)=4, conditionally producing582. This is a **HYPOTHESIS
example**, not evidence that `test3` used HP5. Other difficulty/configuration,
policy, decorations, cached metadata or component streams remain possible. Matching
one300/accuracy/max combo does not prove all those inputs identical. No universal
K is fitted from the two score differences, and the original mismatch is retained.

## R08 handoff and remaining boundary

**ENGINEERING recommendation grounded in the recovered finite path:** replace a
blanket K1 preset only where the reconstruction has supported original-derived
metadata inputs. Maintain raw textual object row count and first/last **recognized
row-start** semantics, accumulated event2 break sum, explicit HP/OD/CS and exact
signed/Single/Double operation order. Do not substitute prepared max end, audio
length, elapsed gameplay clock, JudgedCount, tick count or interval union.
A zero denominator with positive count is a supported clamp case, not something
to normalize to one. Preserve an unsupported status for nonfinite conversions or
inputs whose original path is unresolved. Keep source/cache identity/provenance
visible; selecting difficulty fields or policy from a different record changes
what is being compared. This report changes no application code or ledger formula.

The bounded fresh-parser provenance is sufficient for ordinary finite fixtures;
the full cache file format/generic reader dispatcher, malformed/reused object
behavior, native floating precision, actual original metadata/policy and high
result decorations are not certified. E33 remains unchanged. Its high-flag
boundary is independent of the newly resolved multiplier operands.

The smallest comparison dependency is the **actual original test2/test3 `.osu`
bytes/hashes and settings/event provenance**, especially test3 Difficulty fields.
No owner replay is requested. Later authorized E20 tracing can add original N/T/
first/last/B, global F1a0f/Play F13f8/manager F17ee identities, cache source, M1bc4 operands/return, M0ab7 mods/K and
E33 score events. Existing E20 packet remains unchanged;17 cases still NOT RUN.
Stop after E034. No QA06, reconstruction fix or next expedition is started.

# 3. Pseudocode

**RECONSTRUCTED finite original metadata-to-K chain; non-executable:**

```text
M1bc1 entry:
    N=0; T=0
    obtain original stream; retain null/error exits and partial-state behavior
    first=-1; lastStartText=""; lastEndText=""; breakSum=0
    for each reached trimmed row under original section parser:
        numeric event2: breakSum += Int32(end)-Int32(start)
        HitObjects:
            if first==-1: first=conv.i4(original parse(column2))
            classify Int32(type)&139
            recognized1/2: lastStartText=column2; lastEndText=column2
            recognized8: lastStartText=column2; lastEndText=column5
            recognized128: both texts=column5 before colon
            update original UInt16 recognized counter when applicable
            N=unchecked(N+1)
    lastStart=conv.i4(original parse(nonempty lastStartText)) or original default-1
    T=signedIntDiv(unchecked(lastStart-first-breakSum),1000)
    separately retain end-based F0fe6/F1008; do not use them as T

cache read:
    T=original ReadInt32 operand
    N=loaded UInt16 circle+slider+spinner counts
    retain original versioned read order/exception boundary

normally reached ordinary Play:
    refresh metadata; configure/bind selected map; load objects
    virtual input/setup -> M1140 -> M0aaf -> M0ab7
    freshly read current global map via M2c63; retain separate manager/record reads
    use that current map's N/T/difficulty, conditional on reference continuity
    density=clamp((Single(N)/Single(T))*8f,0f,16f)
    u=(((HP+OD)+CS)+density)/38f*5f, preserving Single-category order
    D=conv.i4(Math.Round(Double(u)))
    K=Double(D)*original modifier/mode factor
    later E33 whole-positive result consumes stored K
```

**UNRESOLVED runtime provenance is not replaced by fitted score constants.**
