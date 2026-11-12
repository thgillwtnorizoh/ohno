# 1. Result

E044 — Work1 original-static local difficulty research, 2026-11-12,
Asia/Bangkok. Research source and latest verified canonical `main`:
`00ad4e790e5a0c23bc6bb4476a2313951dcb996f`. R16/QA14 and the owner's eight
manual checks are retained context. No reconstruction, historical QA,
development test, fixture or earlier report was changed. No exception.

**PARTIAL: the calculator identity, preparation/processing topology, component
state transitions, two distinct aggregation families, final numerical formula
and cache conversion are recovered. A general fresh-chart no-mod calculator is
NOT yet independently reconstructed or ready for faithful R17 implementation.**
The limiting dependencies are named below; none is filled with another version's
algorithm. This is a substantive extension of E043's producer-entry boundary,
not a claim of complete difficulty or original-runtime equivalence.

| Verified original input | Bytes | SHA256 |
|---|---:|---|
| `osubackup.zip` |78,546,961 /18 members|`7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`|
| ZIP member `osu!.exe` |4,541,728|`6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`|

These hashes were verified before decoding instructions. Self-reported
Stable40/file version1.3.3.8 is still historically unauthenticated as an official
dated release. Cache-policy numbers found here do not authenticate a build date.
All findings apply only to these bytes.

## Deliverables and reproduction

New report, `re/evidence/44/star-difficulty.json`, independent evidence checker,
research-only mathematical projection, finite expected/actual results,
historical-check records, preservation manifest and retained tool-development
failure. `re/contexts.md` receives only an append. Exact publication commit is
provided by the verified Git handoff, avoiding a self-referential file hash.

With Python3.12, dnfile0.18.0, pefile2024.8.26 and dncil1.0.2:

```text
python re/tools/star_difficulty_evidence.py /path/osubackup.zip /tmp/stars-a.json
python re/tools/star_difficulty_evidence.py /path/osubackup.zip /tmp/stars-b.json
python re/tools/check_star_difficulty.py /tmp/stars-a.json /path/osubackup.zip /tmp/projections.json
```

The local host prefixes shell commands with `rtk proxy` and uses the already
available private Python dependency directory. No new .NET SDK or original CLR
is needed. The collector reuses the unchanged E004/E037 metadata extraction and
E038 proven string decoder. The independent checker does not import the E044
extractor. It reparses the original, compares every selected body hash,
owner/signature, token/constant/branch/numeric site, generic call edge and complete
bounded direct field/caller inventory. An independent E038 key reducer checks
the four selected decoded strings. The only RVA data exported are four integer
cache cutoffs and their hash, not resource contents.

Final exact coverage, repeat hashes and executed outcomes are recorded in
`verification.json` and `projection-checks.json`. Finite mathematical controls
are **VERIFIED COMPONENT only for the research projection**, not calls to original
methods. No end-to-end `.osu` star oracle is supplied. The 2,175 historical
reconstruction QA checks are retained history and were not rerun by E044.
E020's17 original-runtime cases remain NOT RUN. No game, original CLR/native call,
gameplay window, asset export, input, listening or capture occurred.

Actual final extraction: **352 MethodDefs /335 nonzero-RVA bodies,394 field
declarations,65 type records,23 declared virtual resolutions,2,514 focused call
edges,3,021 watched-field sites and1,503 direct watched-call sites**. The whole
EXE scan decodes19,659 bodies with1,739 zero-RVA methods and no errors. Both
final extractions are byte-identical:2,587,199 bytes, SHA256
`7d7d5ab57ac533507b3b8c066d2da4599106872c7978cf26ce4513331175c917`.
Independent verification passes40 literal controls, all335 body audits,
294 overlapping historical body comparisons, four string reductions,
**82 finite mathematical controls** and848 preserved prior files. D01–D58
are conditional static predictions, not additional executed original tests.

# 2. What they found

**CONFIRMED STATIC** means original instructions/metadata. Aliases and their
behavioral interpretation are **RECONSTRUCTED**. Numeric controls use an explicit
binary32-storage/binary64-operation research profile, not a measured original
x86 JIT. `Mxxxx`, `Fxxxx`, `Txxxx` mean MethodDef0600xxxx, FieldDef0400xxxx and
TypeDef0200xxxx. All offsets are hexadecimal relative to the first CIL opcode;
headers are excluded. Exact obfuscated names, signature blobs, RVAs/file offsets,
type ancestry, reused/new virtual slots and body hashes are in the packet.

**ENGINEERING** is the research-only numeric profile and explicit prepared-input
projection. **UNVERIFIED ORIGINAL-RUNTIME** covers all original gameplay,
provider, scheduler, native/JIT and human-observed behavior in this report.

Preparation reviewed the standing contract, both cumulative journals, E043 report
and parsed full JSON, E004/E005 corrected ordinary loading, E006/E032 geometry,
E023 identity, E033/E034 scoring, R16 and QA14, plus reached timing/parser methods.
These systems remain separate. In particular, no star result is derived from
E034's integer score multiplier K or from gameplay results/accuracy.

## Calculator identity and actual dispatch

`M1bc6(mode)` switches alternate rulesets1/2/3 to other factories; ordinary0
reaches `new T011b.M0801` at002c. Its constructor calls `T072f.M3696:0002`,
then `T0138.M08c4:0002`. The base stores rate Double1 at000a and calls the
supplied map's `M1bc0:0017`, storing its result in **F04d6:001c**.

This is a separate map construction, not the selected catalog object retained
as the calculator's authoritative input. `M1bc0` requires filename and folder,
combines them, then constructs `T03ad.M1bb3(path):002e`. That constructor derives
filename/folder and calls `M1bc1:00f2` to read metadata. Construction failures
can produce null. Parsed metadata, provider/filesystem state and catalog cached
metadata therefore must not be silently equated.

**Important refinement to E043's shorthand:** `M08d9` and `M08c8` are
**nonvirtual** methods on T0138. Their callers use `callvirt`, but that does not
make them virtual slots. The actual overridden calculation inside the wrapper
is **M08d3 → T011b.M0809**. E043 is preserved unchanged.

| Base slot | Reached ordinary implementation |
|---|---|
|M08c5 / mode|T011b.M0802 returns0|
|M08c6 / modifier choices|T011b.M0804|
|M08d0 / manager factory|T011b.M0806 → new T00e8.M0651(false)|
|M08d1 / regeneration predicate|T011b.M0807, XOR(old,new)&18>0|
|M08d2 / object follow-up|T011b.M0808|
|M08d3 / result calculation|T011b.M0809 → T072f.M3699|
|M369a / result assembly|T011b.M080a|
|M369b / difficulty-object creation|T011b.M0810|
|M369c / component construction|T011b.M0811|

Ancestry plus matching original name/signature and reused-slot flags establish
these targets; compatible signatures alone do not. Abstract declarations and
alternate implementations remain separately inventoried. `M3698` throws
NotImplementedException, but it is not called by this supported path.

Main T0138 state: F04d6 cloned/reparsed map, F04d7 manager, F04d8 prepared object
list, F04d9 Double clock-rate divisor, F04da modifier-choice state. Construction
does not itself complete hit-object loading. `M08d9` returns−1 if F04d6 is null
before its try region; inside the region it can return0 for absent preparation
or a caught Exception. A returned0 is not a universal valid-empty-chart result.

## Chart preparation and original object inputs

On first calculation, `M08d7` clears F04d8, disposes any prior manager, creates
the ordinary manager, conditionally refreshes cloned metadata (preserving its
chart ID), and calls manager.M28a2(map,mods):0062. Ordinary manager factory is
M0652→M48de/T09c1; **E005's correction is retained**. This is not E004's earlier
Taiko annotation.

It computes F04d9, calls M28be(false,false,true):0076, and, only if cloned
F0fec is true, calls **M28f7(64,false,true,true):0093**. Here64 is the HitObjects
section; these parser flags differ from ordinary Play's all-section loading.
The registered list is copied into F04d8 at00a9. `M0808` enumerates sliders,
calls M144a(false):0022 and M1441:002d; then the ordinary manager's M28ea is
reached at M08d7:00c7. E005/E006/E032 geometry and version-specific passes are
dependencies, not replaced by R06 geometry or endpoint interpolation.

`M28f7` uses the original scalar converters and registration M28d2; its row
exception handler catches at1cfa and can continue when argument3 is true. Thus
malformed rows are not universally fatal here. BinarySearch/Insert registration
precedes calculator pair creation; equal-time comparator ordering is not
invented. X/Y are parsed/clamped in the original loader, then stored as Single
Vector2 components; signed start/end fields include map shift F100b. HardRock
and parser flags can change the coordinate path. This is prepared geometry,
not direct unbounded raw `.osu` coordinates.

Original circle/slider/spinner factories create the existing runtime object
families. T0339 is a shared ancestor in circle/slider paths, **not a sufficient
test for a slider**; explicit slider tests use T02af. Spinner-base tests use
T067f. Preserve these actual runtime-type relationships in any future model.

### Difficulty-object construction and order

`M0810:0053-00b0` starts at registered index1. Each new **T072e.M366d** receives
current and previous prepared objects, rate, a hit-window operand, Single radius,
approach/preempt operand, zero-based difficulty index and the growing difficulty
list. The first registered object is context only: N prepared objects normally
produce N−1 pair objects.

`M2cb4` stores references F1a32/F1a33, history-list F1a31 and index F1a38:

```text
F1a34 = Double(unchecked(current.Start - previous.Start)) / rate
F1a35 = Double(current.Start) / rate
F1a36 = Double(current.End) / rate
F1a37 = rate
```

Subtracting Int32 before conversion is significant. `M366d` stores
**F1fcb=Max(F1a34,25.0)**. It also derives a gap from previous end with another
Max25, stores approach and radius operands, zeros the hit-window parameter for
a current spinner, and calls slider/spacing preparation M3692 then M3691.
The underlying delta remains0 or negative when only F1fcb was clamped.

The hit-window input in M0810 is `Floor(manager.M125a(OD,80,50,20))−0.5`;
M366d divides it by rate and doubles it. Radius is manager.Single F0ada;
approach is manager.Int32 F0adb. M28be prepares them through original manager
interpolation and viewport-related fields. No universal literal radius is
substituted for these dependencies. OD and CS participate; AR participates in
the history/visibility component. HP does not appear as a direct input to the
recovered mathematical core, which is narrower than proving no indirect effect
through every preparation helper.

Only **after pair construction**, M3699 uses OrderBy on current object's raw
Int32 Start (M369f:0001-0006), then ToList. Do not sort raw definitions first
and rebuild pairs: that changes preceding-object/history operands. The computed
Ceiling(firstStart/(400*rate)) at006a-0082 is popped; it does not initialize a
shared chart-section cursor. Each component owns its own processing state.

## Mathematical component topology and proven state changes

M0811 constructs, in order, T0379(Boolean true), T06fe, T0698, and optionally
T084a when mods&1024>0. Every M3699 call constructs fresh components, even when
the object manager is reused. Each reached difficulty object is sent to every
component through nonvirtual M43e1, which invokes virtual M43e2 and appends its
returned Double into F2ac0. Components' own state can mutate before that append.

The original obfuscated types are retained as primary identities. Descriptions
below are dataflow aliases, not imported osu!lazer skill names.

| Component | Per-object target / state | Important reached dependencies |
|---|---|---|
|T0379, movement/angle composite|M3f33→M19e7; Double F0ecd|M11c1×70.9, M29f7×2.35, M408c×242; M19ea blend; M19ec duration aggregation|
|T06fe, interval/history composite|M1d06→M3539; Double F1f13|M0d6c interval operand, M1744 backward rhythm/history multiplier|
|T0698, visibility/history composite|M1d06→M325d; Double F1d71, list F1d6f|M1e57 and forward/backward/angle visibility helpers; M325f first-minute preprocessing|
|T084a, optional1024|M407f→M3f4b; Double F25b4|M3f4c with object/visibility history; optional only, not a fourth no-mod component|

The three base recurrences preserve store order. With `d=Pow(base,dt/1000)`:

```text
component0: F0ecd = F0ecd*d; F0ecd += raw0*(1-d)       base .2
component1: F1f13 = F1f13*d; F1f13 += raw1*(1-d)*1.16  base .3
            returned value = F1f13 * M1744(object)
component2: add object to F1d6f
            F1d71 = F1d71*d; F1d71 += raw2*(1-d)*2.5  base .8
```

Component0/1 consume the Max25 strain-time field; component2's decay uses the
unclamped pair delta F1a34. A negative or zero delta therefore does not have one
universal treatment. Relax128/Relax2 8192 and TouchDevice4 have direct evaluator
branches, but ordinary background/cache key enumeration does not include them.
Do not extend normalized-cache equivalence to arbitrary direct calculator calls.

M3691 normalizes spacing with Single vector length and Single `50f/radius`,
then converts the product to Double. M3694 forms cross and dot products from
Single components before Double Atan2/Abs. Spinner adjacency has early returns.
Slider lazy-travel M3692 walks actual nested entries, uses an adjusted terminal
observation time, integer duration/traversal division, thresholds90/50, Single
vector moves, and a nullable final-position cache. It is not one distance from
head to geometric endpoint. M3693 can use a penultimate nested endpoint for an
angle; repeats, durations and geometry matter independently.

### Precisely bounded unresolved mathematics

All named bodies are statically decoded and hashed; no encrypted or unavailable
native calculator is being alleged. **Their complete semantic projection and
independent finite validation are not finished**, which blocks a general-chart
implementation claim:

1. **M11c1/M11c3**: angle-history movement terms, previous interval/position
   states, smooth thresholds, slider-related travel terms and repeated angle
   penalties. M11c1:00cd-03fc consumes nullable angle/history state;03fc-051c adds
   interval contrasts/travel and scaling. M11c3 can inspect up to six predecessors.
2. **M408c/M408d**: distinct angular/velocity contrast channel, history of three
   pair objects, slider travel and exponent1.45. It must not be merged with M11c1.
3. **M1744/M1746-M174f**: bounded backward history (up to32,5000ms), mutable
   integer pattern groups, timing-comparison predicates and result accumulation.
   A conventional rhythm bonus would erase this actual state machine.
4. **M1e57-M1e65**: visibility/history contribution with forward and backward
   walks, 3000/2000ms bounds, nullable angles and original object preempt times.
   Extracted entry arithmetic is not a complete independent algorithm.
5. **M1441/M432d plus M3691/M3692**: nested slider event identity/order and
   lazy-travel geometry must be integrated with E005/E032 precision. Extracting
   M432d's iterator factory does not prove all generated event semantics.

These are explicit research stopping boundaries, not permission to use defaults.
The package provides exact reached call edges and the immediate methods outside
the extracted mathematical clusters. Cluster membership alone is not proof that
every helper is reached for every chart. UI/logging/provider/native helper edges
are inventoried without an unrestricted transitive frontend reconstruction.

## Two different aggregation families

### Component0: duration-valued envelope

T0845.M3f31 receives decay .9 and window400. Its duration retention threshold
F25a6 is `11/(1−.9)`, used against total retained duration times400. M3f33
initializes the current interval at the first **pair's** start, with next=start+400.
M3f34 advances only while object time **strictly exceeds** next boundary.
Equality stays in the existing interval. It retains/removes candidate maxima,
stores value/duration segments and can trim entire trailing retained segments.
This is not the ordinary fixed-section maximum list used by the optional T0879
family, and neither is a manager-global timing cursor.

M19ec first obtains finalized segments via M3f38 (once guard F25aa), filters
value>0, transforms the initial duration prefix in20-unit slices while accumulated
duration<4000 using logarithmic interpolation between.727 and1, then sorts the
remaining processed segments descending by value. M19e9 consumes that order:

```text
t = 0; total = 0
for (value,duration) in processed_order:
    next = t + duration/400
    w = Pow(.9,t) - Pow(.9,next)
    total += value*w
    t = next
return total/(1-.9)
```

The executed duration controls take **already processed ordered segments** as
explicit inputs. They do not claim to test every M3f33/M3f34/M19ec state branch.

### Components1/2: per-object sorted values

M1d08 returns0 for an empty value list. It calls virtual M1d07 first (identity
for T06fe, M325f for T0698), then OrderByDescending, filters ordered value>0 and
accumulates. For zero-based positive-entry index i:

```text
top = 1 + s/(1+i)
bottom = Pow(Double(i), exponent) + 1 + s/(1+i)
weight = top/bottom
total += value*weight
```

T06fe overrides s=20 (M3536); T0698 inherits s=1. Exponent is.9. **The power is
`i^0.9`, not `0.9^i`.** With values10 and5, the totals are approximately
14.5833333333333 (s20) and13 (s1), rather than a conventional .9-weighted sum.
Equal values retain successive weights; there is no deduplication.

M325f filters positive inputs, obtains a prefix count through M3260 (first
object time+60000, equality included, stop at first later), then scales the
earliest positive values using log10 interpolation. This occurs before M1d08's
descending sort; sorting first would change which entries are transformed.

The optional T084a uses T0879's400-unit section mechanism and .9 weighting,
which is separately inventoried. Its `bgt` equality and section initialization
must not be projected back onto all three no-mod components.

## Final output formula — recovered from component accumulators

Let q0/q1/q2 be the Double results returned by the three base components, and
q3 the optional1024 result. For finite nonnegative operands:

```text
a0 = Pow(q0,.63)*.02275                       M080d
a1 = Sqrt(q1)*.0675                          M080e
a2 = Sqrt(q2)*.0675
a3 = optional ? Sqrt(q3)*.0675 : 0
e0 = 4*((a0*a0)*a0)                          M080b/M19e0(exp3)
e1 = 4*((a1*a1)*a1)                          M1d09
e2 = 4*((a2*a2)*a2)
e3 = 25*(a3*a3)                             M3f4e — square, NOT cube
e23 = Combine_M080c(e2,e3)
joined = Pow(Pow(e0,1.1)+Pow(e1,1.1)+Pow(e23,1.1),1/1.1)
starsDouble = Pow(joined*1.12,.3333333333333333)
```

M19d8 adds each powered value in array order, then applies reciprocal exponent.
M080c returns b if finite a<=0, returns a if finite b<=0; otherwise it takes the
1.1-norm of `[a, b*Clamp(b/a,.25,1)]`. Original `bgt.un` makes NaN follow the
combine arm, not those finite shortcuts. No final rounding/clamping is present.
The result is stored in F1094 on a new T08f1 result, then M0809 returns that field.
Optional diagnostic dictionary strings independently decode to `Strain`,
`Hit window 300` and `0.00`; these strings do not name the individual components.

**Three quantities must remain separate:** M08d9 returns Double; M1bd0:002b
converts to Single before dictionary insertion; M1bc7 promotes that cached Single,
M1bd5 takes Double Abs, and M2d7f floors display magnitude to two decimals.
For example, supplied1.15 becomes cached binary32 bits0x3f933333, which displays
1.14 under the declared projection profile. This is not an exact calculator
output of1.14. Nonfinite values are tracked as categories rather than assigned
fabricated usable star values.

### Independently derived narrow numerical control

With **exactly two prepared circles**, no mods, finite positive radius, full
hit-window99, normal helpers and interval dt>=100:

- Difficulty index0 makes M11c1 and M408c return0; M1e57 also returns0.
- M19ea's third-channel0 blend yields movement contribution0, even when the
  separate M29f7 operand is positive. This does not generalize to longer charts.
- M1744 has no qualifying backward history and returns1. M0d6c's decay
  normalization cancels the recurrence factor in an independently simplified
  oracle; the executed projection keeps the original separate operations.
- The interval component is approximately `1.16*1000/dt`; final star output is
  `Pow(4*1.12,1/3)*.0675*Sqrt(1.16*1000/dt)` in that independent oracle.

Five dt controls compare this closed form with the instruction-ordered projection.
Same-position and separated circles have equal output in this deliberately small
control. This is neither a loaded-original measurement nor a general circle-map
calculator. Empty/one-object/all-spinner conditional predictions likewise require
successful stated preparation and ordinary helper returns.

| Prepared two-circle dt | Projection Double star result |
|---|---:|
|100|0.3789879823511248|
|200|0.26798497230868795|
|500|0.16948857823852112|
|1000|0.119846523006125|
|2000|0.08474428911926056|

These are conditional research-projection numbers, not measured original stars.
The independently simplified Decimal oracle is checked with a stated tight
tolerance; no original x86 bit-for-bit equivalence is asserted.

## Modifiers, cache policy and worker freshness

M0804 supplies choices256,64,2,16,1024,8. M08df builds combinations; M08e8
excludes DT+HT (320), Easy+HardRock (18), Relax+Relax2 (8320), and more than one
eligible key mod. The six choices admit36 compatible masks, including0, **under
the explicit no-hash-collision assumption**: M08f2 uses GetHashCode equality in
the overlap filter, so an unrestricted runtime enumeration claim is stronger
than these finite bit predicates.

M08d6 chooses rate1.5 for64, else.75 for256, else1. M0807 regenerates objects
when `(old XOR new)&18>0`. Other changes can reuse the prepared manager/list,
write new modifiers, recompute rate and call M28be(false,false,false). New
components still reset their own state each calculation. M28b0 applies Easy/2
and HardRock multiplication/min10 only outside application Edit1. M28b2 uses1.3
for the CS scalar; M28b3 uses1.4 for its difficulty interpolation. Global mode,
viewport and preparation helpers remain inputs; no universal modifier snapshot
is invented. Flashlight adds a fourth component. Hidden changes selected history
processing. Complete adjusted-map results are secondary and remain partial.

Ordinary cache normalization is `mods & 1370` (M0803/M0805/M08cf). NoFail1,
TouchDevice4, Nightcore512, Relax128, Relax2 8192 and ScoreV2 are not independent
keys here. Raw direct-call evaluator branches outside that mask do not establish
that these cache collisions calculate identically.

F0fea contains four mode dictionaries. Missing M1bc7 key returns Double0.
M1bd7 schedules key0 missing or negative only when F1014 is false; its `bge.un`
causes NaN to take the nonnegative/no-schedule arm. M1bf2's **20250107 boundary
changes cache representation**: older dictionaries use Double→Single conversion
plus key-normalization filtering; newer dictionaries hold Single. No mathematical
calculator switch is present at that branch.

Separately M1bb4 initializes F0fbd from16 bytes at F0c9a RVA0x1fe0f8, SHA256
`f4051c25e14d3ec85587a1a21bd8c5fa53fc69ac685b1b3df110c98e34528245`:
**[20260630,20260630,20260630,20231214]** in mode order0–3. Older entries become
`−Abs(Single)` stale markers. These constants are not proof of an official
dated build or a complete history of algorithm revisions.

M1c1e waits while current mode2, sets F1014 under a lock, calls M1bd0, clears
the flag under a lock, cancels its timeout callback then queues M1c23. Timeout
M1c1f also clears the flag and invokes completion before further task-related
branches. The whole worker computation is not enclosed in a flag-clearing
finally. M08d9 catches ordinary calculation exceptions, but construction,
enumeration, logging and cleanup can still interrupt outer work.

M1c23 merges results into its **retained map reference** using M1bc9, then invokes
completion. It has no content-hash/revision/generation check at that write.
M25d1 refreshes selected metadata only on reference equality. Cache merge,
catalog replacement and UI refresh are distinct; a stale worker is not proven
cancelled merely because selection changed. R16's strong file hashes and
generation policy are ENGINEERING and are not original cache-freshness evidence.

## Verification, preservation and remaining precision boundaries

`star-difficulty.json.reference_cases` records D01 onward, each with initial state,
exact sites, prediction and unresolved conditions. `projection-checks.json` records
82 separately named finite expected/actual controls: decays, Single distance,
unchecked time subtraction, sorted and duration weights, the five narrow
two-circle controls, final transforms, nonfinite branches, cache bits/floor,
modifier-key collisions and stale/versioned cache behavior. Static prediction
rows are not additional executed tests.

The initial new-checker failure was an offset transcription: M08d7:00ae is
`ldarg.0`, while the intended virtual M08d2 call is00af. Its reproduced initial
checker source/failure are preserved in `tool-development/initial-checker`.
Correction changes only that literal expected site, with the complete body hash
and surrounding instructions unchanged. A private capture command also had a
quoting SyntaxError before execution; no original or production failure follows.

Unchanged historical E004/E005/E006/E023/E032/E033/E034 relationship checkers and
E043's unchanged relationships/projections/archive-audit functions are rerun.
E043's pre-R16 reconstruction-source manifest is deliberately not asserted
against newer R16 production; this historical tooling boundary is explicit,
not a modified checker. Exact outcomes are retained in historical-checks files.

The preservation manifest pins848 prior tracked files and the research journal's
469,272-byte prefix SHA256
`7fdaeaea67f07ad6c6c941167890191a18a62bba197f8bc9f24cee10e16b89f3`.
No existing byte may change; only the journal suffix is new. Method extraction
repeatability, new-tool syntax and Git diff checks accompany publication.

NaN/unordered comparisons, integer division/overflow, Single vector operations,
stored float conversion and Double Math calls remain in their original order.
The model is not an original-method interpreter. Exception-catching0, missing
map−1, legitimate0 and NaN results remain distinct paths. Original x86 extended
intermediates, Math library last-bit behavior, sort/iterator exceptions,
provider/viewport/configuration state and scheduler effects are unmeasured.

## R17 readiness decision

| Component | Classification / bounded handoff |
|---|---|
|Calculator dispatch|Recovered, concrete ordinary hierarchy and true virtual targets|
|Object inputs|Recovered field identities; provider/global preparation remains conditional|
|Object preprocessing|Partial: pair ordering/normalization recovered; nested slider integration not complete|
|Per-object calculation|Partial: recurrences and reached arithmetic identified; four critical history families not independently reconstructed|
|Skill/strain system|Blocked for complete independently checked semantics; topology/state recovered|
|Aggregation|Recovered final formulas and key branches; Partial complete duration-envelope/history preprocessing projection|
|Final formula|Recovered for explicitly supplied component accumulators|
|No-mod calculation|**PARTIAL — not ready for general fresh-chart R17**|
|Modifier variations|Partial; clock/scalar/cache gates recovered, full adjusted results deferred|
|Cache integration|Partial; conversion/key/cutoffs recovered, stale-worker identity/lifetime not a cancellation model|
|R16 display compatibility|Ready for explicitly provenance-bearing cached/supplied values; not automatic calculated stars|

Recommendation, not authorization: retain R16's **Stars not calculated** default.
Use the recovered final formula/precision and finite controls as research fixtures,
not a chart-wide substitute. The smallest mathematical follow-up is a bounded
ordinary-circle-only translation/independent validation of M11c1/M408c/M1744 and
M1e57 with their history-state helpers. A separate nested-slider preparation
closure is then needed before slider support. Neither a familiar strain model
nor this report authorizes inaccurate R17. No R17, E045 or QA15 is started.

# 3. Pseudocode

Documentation of original-static relationships with RECONSTRUCTED aliases;
opaque mathematical dependencies remain explicit. Helpers may fail after earlier
mutations. No original execution or rollback is implied.

```text
ConstructOrdinary(map):
    calc = T011b -> T072f -> T0138
    calc.rate = 1
    calc.map = map.M1bc0()                    # new path-backed metadata record

Calculate_M08d9(mods, diagnostics=null, suppliedManager=null):
    if calc.map == null: return -1            # outside try
    try:
        if suppliedManager != null:
            M08d8(suppliedManager)            # separate supported-state boundary
        elif objects == null or ((oldMods XOR mods)&18)>0:
            dispose old manager; create ordinary T00e8(false)
            refresh/bind map and mods
            rate = 1.5 if mods&64 else .75 if mods&256 else 1
            manager.M28be(false,false,true)
            if map.F0fec:
                manager.M28f7(64,false,true,true)
                objects = copy(manager.F17fb)
                for slider: slider.M144a(false); slider.M1441()
                manager.M28ea()
        else:
            manager.mods = mods
            recompute rate; manager.M28be(false,false,false)
        if objects == null: return 0
        return virtual M08d3(diagnostics)      # T011b.M0809
    catch original Exception:
        return 0

M0809:
    components = [new T0379(mods,true), new T06fe(mods), new T0698(mods)]
    if mods&1024: append new T084a(mods,objectCount)
    if objects nonempty:
        pairs = []
        for i=1 .. objects.Count-1:
            pair = new T072e(objects[i],objects[i-1],rate,
                            hitWindowOperand,radius,preempt,pairs.Count,pairs)
            pairs.Add(pair)
        # Pair history already established; sort only now.
        for pair in OrderBy(pairs, currentRawStart):
            for component in components:
                value = component.virtual M43e2(pair)
                component.F2ac0.Add(value)
    result = M080a(components)
    optionally prepare diagnostic strings
    return result.F1094

Component0Raw(pair):
    # NOT yet a complete independently reconstructed implementation:
    x = M11c1(pair,true)*70.9
    y = M29f7(pair)*2.35
    z = M408c(pair,true)*242
    preserve M19ea blending and OD-derived multiplier

Component1Raw(pair):
    preserve M0d6c and state decay/store order
    return updated state * M1744(pair)        # unresolved full rhythm history

Component2Raw(pair):
    preserve M1e57 and state decay/store order # unresolved full visibility history

Assemble(components):
    read q0/q1/q2 and optional q3 through their actual aggregate getters
    apply Pow/Sqrt/scales, ordered cubes and optional squared energy
    combine component2/optional energy through M080c
    return Pow(Norm1.1(e0,e1,e23)*1.12,.3333333333333333)

BackgroundWriteback:
    for enumerated compatible modifier entry:
        dictionary[mod] = Single(calc.M08d9(mod,null,null))
    calc.M08cd()                              # not an encompassing finally
    queued M1c23 merges into retained map
    display = Floor(Abs(Double(cachedSingle))*100)/100
```
