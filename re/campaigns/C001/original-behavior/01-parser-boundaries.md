# 1. Result

C001 Work 1 checkpoint 01, 2026-11-13 (client date; UTC extraction).
Campaign remains OPEN. Original-byte investigation and inherited-production audit;
no production code changes. Full authorization is retained in ../authorization.md.

Pinned source: main 0421f9249f494b4a86fdf59c7dd76cd9a664688b. Pinned EXE SHA256
6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d;
archive SHA256 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738.
Self-declared Stable40/file-version1.3.3.8; dated official release/authenticity
unresolved. Static Linux Python3.12 analysis; no original CLR/native/UI execution.

New extract.py produces deterministic metadata/relationship evidence for353 methods,
2214 branch sites and243 independently reduced literal strings. Whole-EXE scan:
19659 decoded bodies,1739 zero-RVA methods,zero decode errors. Branch destinations
for selected methods are validated against instruction boundaries. Method body
hashes, signatures, owner ancestry, token/constant/operation sites, exception
regions, direct dependencies and literal equality dispatch are retained in
static-inventory.json. It is a bounded root-plus-one-hop inventory, not proof of
complete dynamic reachability. Every inventoried branch initially has an unresolved
semantic/production disposition; this report identifies only recovered portions.
Original disassembly and resource contents are not redistributed.

Reproduce from repository root:

```sh
python re/campaigns/C001/original-behavior/extract.py /path/osubackup.zip /tmp/c001.json
python re/campaigns/C001/original-behavior/check.py /tmp/c001.json
```

The same report also records the source-level independent-parser inventory.
No new canonical C001 implementation exists at the tested starting head. Existing
QA15 evidence checker and actual Linux headless QA15 run pass95/95 cases. This is
neither a new Windows regression run nor qualification of the original client.

# 2. What they found

## Syntax is stage dependent

All statements marked CONFIRMED STATIC below refer to the pinned executable,
with code-relative hexadecimal offsets. Mxxxx means MethodDef0x0600xxxx.

| Stage | CONFIRMED STATIC mechanism | Consequence / limitation |
|---|---|---|
| Header refresh M1bc1 | Stores format14 at0054, shift0 at005b, StackLeniency Single .7 at0066, Countdown1 at006d, SampleSet1 at0090. Initial reader loop01d6–0251 accepts a line only when IndexOf("osu file format")==0, then parses the suffix after the LAST lowercase v as Int32. | No min/max version rejection in that recognition block; this does not prove complete preparation of arbitrary versions. Wrong prefix can end recognition and still continue later parsing with default14; exact first-line strictness is not a shared original rule. |
| Compatibility shift | M1bc1:0235–024c writes24 iff version<5 AND current application mode==Edit1, otherwise0. | Never apply a universal old-map+24 shift during ordinary Play. |
| Header body lines | M1bc1:0256–0262 trims the whole line. Outside HitObjects it finds the FIRST colon and splits by Remove operations at0298–02b8. | Title:a:b retains a:b. A blanket Split(':')[1] is wrong for this reader. |
| Header section recognition | M1bc1:02c7–02f2 uses case-sensitive Enum.Parse(Type,string), assignment follows successful conversion. Catch02f9 resumes the loop without clearing section local. | Unknown/case-mismatched section retains previous section. Sections containing a colon encounter key splitting first. Numeric enum names follow CLR behavior, not a hand-selected wiki section list. |
| Runtime-loader filtering | M28f7:0192–01ce skips empty raw lines and raw prefixes SPACE, underscore, // BEFORE trimming. | Leading-space content is ignored on this loader; leading tab is not the same tested prefix. Do not merge the readers' whitespace rules. |
| Runtime sections | M28f7:01ec–0229 identifies '[' before trim, trims only bracket chars for Enum.Parse, catches errors and retains previous section. Mask intersection0231–0233 controls admitted sections. | Unknown section behavior is stateful; trimming section whitespace changes results. |
| Runtime key splitting | M28f7:0238–0265 trims then splits separately on comma and ALL colons;0275–0298 selects only cells0/1 for key/value. | Runtime General/Editor properties have a different colon treatment from metadata refresh. Preserve original bytes and stage identity. |
| Additional section | FileSection enum T0b24 includes Variables256, alongside the eight requested sections; All511. M28f7:0603 consumes Variables. | Variables/storyboard substitution is in C001 interpretation inventory. Playback remains another subsystem. |

Unknown sections retaining prior state follows the catch and assignment ordering;
it is not a guess based on contemporary file documentation. Exceptions/partial
mutations matter: M1bc1 catches processing errors at0f9d, logs/notifies then continues
to bookkeeping and a true return at10f6. It does not mean the whole file parsed
correctly. Stream absence has a different false return. M28f7 catches a row error
at1cfa: argument3=true skips the row; false rethrows. Normal M28f5 call supplies its
argument2 (false on ordinary M223b path) to argument3. A validating helper and a
forgiving row catch therefore cannot be generalized into one universal policy.

## Numeric conversion and defaults

CONFIRMED STATIC: M09e3 Single.Parse, M09e4 Double.Parse and M09e5 Int32.Parse all
load provider field F2c2b. They return immediately when their validation Boolean
argument2 is false. With true they check value against negative/positive limit;
Single/Double additionally reject NaN via IsNaN. There is no invented finite-only
guard on the false path. Native CLR parse syntax, overflow behavior and conversion
of nonfinite/out-of-range values remain UNVERIFIED ORIGINAL-RUNTIME. F2c2b's sole
managed direct writer is M4669:001f; preceding construction/read-only culture setup
is inventoried, but its literal identity still needs explicit recovery.

M1387 constructor stores AR/CS/HP/OD=5 (F0b52/53/54/55), multiplier Double1.4
(F0b56), tick rate Double1 (F0b57). These are fresh-object defaults, not proof that
refreshing a reused map resets every difficulty field.

M1bc1 difficulty dispatch has these original rules:

| Input | CONFIRMED STATIC branch |
|---|---|
| HP/OD before v13 | Byte.Parse followed by clamp0..10, then Single store (HP0b87,OD0c5d) |
| HP/OD v13+ | Provider Single.Parse then clamp0..10 (HP0b5d,OD0c36) |
| CS before v13 | Byte.Parse/clamp0..10 (0c0d), even for Mania |
| CS v13+, current stored mode3 | Provider Single.Parse/clamp1..18 (0bb9) |
| CS v13+, other stored modes | Provider Single.Parse/clamp0..10 (0be3) |
| AR | Same version13 split/clamp0..10; records explicit-AR flag at0d3e–0d3f |
| OD before any explicit AR | Copies newly stored OD to AR at0c77–0c85 |
| OD after explicit AR | Does not overwrite AR |
| Multiplier / tick rate | Provider Double.Parse, clamp.4..3.6 / .5..8 (0c8f /0cc1) |
| StackLeniency | Provider Single.Parse, Math.Max(0,Math.Min(1,value)) at07ab–07cb |

Field ordering is observable: Mode parsed after CS cannot retroactively select the
mode3 CS branch within this pass. Duplicate metadata strings ordinarily overwrite;
BeatmapID/SetID have specific guarded writes (0a9a and0ab6), so a universal
last-key-wins dictionary loses original behavior. Every recognized literal dispatch
is machine-indexed with its handler offset in static-inventory.json; this is not
yet complete semantic coverage of every property.

## Objects and preparation boundaries

CONFIRMED STATIC, reinforced E004/E005/E036: runtime object classifier masks
colour bits~112, then tests Normal1,Slider2,Spinner8,Hold128 in that order.
Catalog's current source instead tests8,2,1; gameplay rejects combined types;
metadata summary M1bc1 uses exact masked values, with default counting behavior
already recovered in E034. A single canonical category is insufficient unless
stage-specific counting/construction semantics are preserved explicitly.

M28f7 slider path defaults to enum0; single-character C/B/L/P select0/1/2/3.
Unknown single-character tokens leave the current kind unchanged; longer tokens
are parsed as coordinates. M28f7:17da parses raw repeats,17e8 explicitly rejects
values>9000; missing pixel-length column leaves the initial Double0. M142d's
effective traversal Max(1,raw) remains distinct from raw count (E005). These do not
prove safe full geometry for every negative/zero/exceptional case.

Hold path M28f7:1bc9–1cf0 uses start as default end; nonempty column5 is colon-split,
parsed end is not given the spinner's extra F100b addition here. Sample columns
have different optional-length guards, including direct access to cell2 after
length>1. Factory slot M1873 receives position,start,end,inferred-or-file NewCombo,
sound,conditional combo skip and sample metadata; null return is not registered.
Exact mode-specific hold construction still requires resolution. Do not silently
convert all holds into circles in the canonical data model.

Stacking: M28ea selects >5 M144a+M28eb versus<=5 M28ec. M28eb returns immediately
for stored map mode!=0. Its stack proximity is STRICT distance<3, and temporal
break tests are strict greater-than, so equality stays in search. Modern negative
slider-tail stack propagation exists at01c4–0246. Final displacement uses F0ae1
in both vector components and M24b0; old pass mutates positions during its forward
walk. Detailed independent stack oracle and finite production verification are
still OPEN; recording these branches does not close stacking.

## Existing parser/consumer inventory

SOURCE INSPECTION at0421f924:

| Parser / preparation owner | Consumers / divergence |
|---|---|
| Gameplay/Beatmap.cs Beatmap.Load | Program/GameWindow, sessions, rendering, audio, hitsounds, health and Rank; narrow v5–14, explicit Mode/AR, L or v14 B, equal-time rejection, unstacked preparation |
| Same file LegacyScoreMetadata.ReadFresh | Raw row counts/first/last/break seconds and HP/OD/CS feed legacy score multiplier independently of prepared objects |
| Selection/LocalChartCatalog.cs Index | Catalog/details/selection identity; independent metadata/type counting; revision fingerprint controls subsequent LoadGameplay |
| Difficulty/CircleDifficultyPreparation.cs StarAdmission.Parse | Separate exact-v14 five-column strictly increasing simple circles, CS5/OD5/AR5-or10/StackLeniency0 profile; explicit engineering admission |
| Same file CircleDifficultyPreparation.Prepare | Independent radius/preempt/window and pair-history geometry for R17; must receive canonical prepared operands without changing R17 math |
| Beatmap sample parsers / PrepareSlider | Sample override/time lookup and geometry conversion feed CircleHitSounds, Windows audio adapters, PlayfieldView and CircleSession |

Original timing list order/inheritance (E005), NewCombo/break mutation(E036), sample
selection(E037–E041), scoring/HP(E033–E035), catalog provenance(E043) and restricted
star math(E044–E046) remain inherited authority. E005's correction of E004 ordinary
versus Taiko factories must be retained. No star mismatch is explained by this
checkpoint alone. Owner-reported19 mismatches remain open with uncorroborated build,
chart-hash and cache provenance.

Next: independent finite corpus against actual existing production, culture identity,
version/mode branch recovery, complete geometry/stack/event semantics and Work 2
canonical integration. Final Windows/original qualification remains owner-only.

# 3. Pseudocode

RECONSTRUCTED from CONFIRMED STATIC branches, not an original-runtime observation:

```text
refresh_metadata(map):
    apply the explicit refresh defaults; keep other reused fields as original does
    inspect original header prefix and parse suffix after last 'v'
    shift = 24 only when version < 5 and application_mode == Edit
    for trimmed line:
        split first colon into key and complete remainder (outside HitObjects)
        if section declaration and enum conversion succeeds: replace section
        if enum conversion fails: keep previous section
        dispatch original fields in source order
        parse difficulty according to version and mode NOW
        OD copies to AR only until explicit AR has appeared
        on processing error: preserve prior writes; log; continue final bookkeeping

load_runtime_objects(manager, section_mask, validate_and_skip_bad_rows):
    for raw line:
        skip empty / SPACE-prefixed / underscore-prefixed / //-prefixed line
        update recognized section; unknown section retains previous state
        apply section mask
        split runtime key/value cells separately from metadata refresh
        parse rows with helper(value, bound, validation_flag)
        choose Normal before Slider before Spinner before Hold
        construct through the selected ruleset factory, then register
        catch row error: skip iff supplied Boolean is true, otherwise rethrow
    apply break-driven combo preparation
    invoke mode/version-specific geometry, timing and stacking preparation
    # Complete mode/geometry/event semantic coverage remains UNRESOLVED.
```
