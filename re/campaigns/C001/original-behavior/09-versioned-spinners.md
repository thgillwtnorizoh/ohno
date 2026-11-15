# 1. Result

| UEP-001 revision 2.1 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work1 original research and independent fixtures / [Issue#7](https://github.com/thgillwtnorizoh/ohno/issues/7) |
| Status | VERIFIED CHECKPOINT — finite conditional original-static implementation handoff |
| Starting / Exact tested production SHA | 782c00771a436b2a2950680d4f5de379a791d9dd; latest main verified before work; actual compiled production commit/state and Beatmap byte hash checked |
| Published SHA | Introducing commit in Git history and final handoff; no self-referential hash in this file |
| Objective / Supported profile | Fresh ordinary Mode0, no modifiers/replay source compatibility; v5–v13 construction with v14 controls; finite positive integer start/end, explicit timing/sample operands, ordered separated objects, no stacking/breaks; original length-provider precondition explicit |
| Changes / Functional findings | Original parser/factory/version/constructor/NewCombo/registration/end-cap handoff,33 independently authored hashed charts, actual indexing/canonical/consumer characterization. No production changes |
| Evidence / Confidence | CONFIRMED ORIGINAL static byte identities/sites; RECONSTRUCTED conditional expectations/rational math; VERIFIED COMPONENT actual reconstruction outcomes; ENGINEERING finite admission/scope. No original-runtime equivalence |
| Tests / Environment | Python3.12/Linux,dnfile0.18.0/dncil1.0.2/pefile2024.8.26;16 selected bodies/regions,62 decisive sites,40 historical identities PASS; repeated recovery identical;33 byte fixtures/discriminators PASS. Linux .NET SDK8.0.427/runtime8.0.33:179 production assertions PASS;4 canonical preparations/29 rejections, audit twice identical; Python compilation PASS |
| Historical checks / Failures | E024/E025/E026 PASS. E036/E041 unchanged full-tree checkers FAIL AttributeError on five newer list-valued JSON summaries. Byte-identical checker/evidence schema envelope PASS349/466 method identities; exclusions listed in packet, no semantic assertion removed. New probe syntax/nullable compile failures and initial fractional-site assumption corrected and disclosed |
| CI / NOT RUN | No new CI or redundant Windows/full gameplay gate: research only, production unchanged. Original EXE/CLR/gameplay/audio/input, actual owner maps, broad v3/v128/other-mode original behavior, source-legacy replay policy/editor/degenerate runtime, late offset and R17 investigations NOT RUN |
| Coverage gained / Remaining | Precise finite spinner construction across requested versions; actual v14-only gates and misleading format-family confounder; immutable expectations and integration criteria. Work2 integration, independent new-source QA, exact Windows regression/build and owner/original qualification remain |
| Discrepancies / Limits / Blockers | Current catalog and canonical old-spinner rejection reproduced; fractional-time row failure can also be format-profile. No original-evidence blocker for declared finite subset. Host length/source compatibility and excluded legacy/editor/exceptional inputs remain conditional/unqualified |
| Updated records / Reproduction | This report; ../versioned-spinners/ scripts,fixtures,manifest,static evidence,ProductionProbe,actual results,verification. Append-only status/coverage/dependency/discrepancy/re/contexts.md |
| Next bounded task / Actor / Dependencies | Work2 finite canonical versioned spinner integration satisfying acceptance criteria below, preserving historical loader and established consumers; Work1 independently verifies actual new production; owner runtime separate Issue#4 |
| Subsystem | OPEN; C001/Issue#7 OPEN; no accepted compatibility/runtime or owner recovery-count claim |

Pinned ZIP SHA256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`;
`osu!.exe` SHA256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.
Both freshly authenticated. Self-declared Stable40/1.3.3.8 is not authenticated dated
release provenance. Read AGENTS/standing instructions, Issue#7 plus both HQ comments,
C001 authorization/current records, E024–026/E034/E036/E041 and actual canonical
consumers. E004's Taiko attribution is superseded by E024, never used here.

```sh
python re/campaigns/C001/versioned-spinners/recover.py /path/osubackup.zip /tmp/spinner.json
python re/campaigns/C001/versioned-spinners/check.py /tmp/spinner.json
python re/campaigns/C001/versioned-spinners/build_fixtures.py --check
python re/campaigns/C001/versioned-spinners/check_historical.py
dotnet run --project re/campaigns/C001/versioned-spinners/ProductionProbe/ProductionProbe.csproj -c Release -- . /tmp/spinner-production
```

# 2. What they found

## A. Which observed failure is actually reached

The owner's follow-up Issue#7 comment partitions134 reported format-profile rows:
113 standard v5–v13 spinner-bearing,4 other-mode v5–v13 spinner-bearing,14 declared
v3 and3 declared v128. These are OWNER-REPORTED aggregates, not locally replayed
charts or proof of their exception sites.113 is a candidate upper bound within
scope, not a forecast of recovered canonical or owner-playable charts. v9/v12
account for55 of those candidates. No private chart paths/content/hashes collected.

VERIFIED COMPONENT at782c007,33 synthetic inputs:

| Actual canonical outcome/site | Files | Meaning |
|---|---:|---|
| Successful v14 controls |4|Single, mixed circle/spinner/circle/L-slider, bit4 and full sample payload; actual score/session/fresh health/sample identities checked |
| Beatmap.LoadCore:894 spinner version!=14 |23|Integer-time old-spinner fixtures, including old-format malformed-end/degenerate probes and one other-mode fixture; later guards not reached |
| Beatmap.LoadCore:873 Int32 row time/type syntax |2|Fractional starts fail before spinner guard; its message contains format-v14, so audit emits unsupported/format-profile |
| Beatmap.LoadCore:841 outer format |2|Declared v3/v128 negative controls |
| Modern end-field guard |1|v14 missing end; engineering spinner-duration family |
| Final standard-mode guard |1|v14 Mode1; v9 Mode1 instead hits spinner version gate earlier |

Catalog outcomes:4 Indexed/24 Unsupported/5 Invalid. Canonical4 prepared/29 rejected;
audit family27 format-profile/1 mode-profile/1 spinner-duration. Known malformed
fractional rows can therefore appear in unsupported format-profile without being
a version-compatibility failure. Exact sites and category counts are preserved in
evidence/production-results.json and audit.json. No diagnostic fix is proposed here.

All direct format-bearing exception literals in Beatmap.cs were inventoried: missing
header lines59/69 (malformed status, not unsupported); score-reader format514
(canonical outer guard already excludes its bad versions); outer841; syntax873;
spinner894; historical Bézier914 (unreachable in extended canonical admission).
Other methods' finite count/format text and chart-driven fallback taxonomy can
confound generic substring families, so this list is not a theorem about every
possible exception/provider. Canonical C01 metadata-stage rejection already avoids
using offending numeric text for mutation or that metadata reason. Current
LocalChartCatalog.Index separately marks spinner-bearing format!=14 Unsupported;
other-mode and outer-format catalog guards have their own ordering. Both catalog
and canonical contracts must be addressed by Work2, not merely one guard removed.

## B. Original parser and factory: version is not a universal spinner-policy switch

Offsets are hexadecimal CIL-code-relative, excluding headers. Public packet records
hashes/RVAs/signatures/sites/branches/arithmetic for16 targeted bodies/regions, not
proprietary source/full disassembly. No whole executable scan was repeated.

CONFIRMED ORIGINAL: M28f7 masks raw type with-113 at1496–1499, then Normal1,
Slider2,Spinner8,Hold128 priority (E036). Spinner branch1ab8–1bc4 has no map-format
read. Position x/y are parsed by shared code but not forwarded to spinner factory.
For requested standard ordinary objects, virtual M1872 resolves via T00e8/T09c1
to M48e1→T00d8.M05ea→T067f.M3201. No v14-only original factory restriction occurs
in this reached chain. T00d8 ancestry and ordinary/Taiko separation are inherited
and body-identity matched to E024/036/041.

Source column indices are zero-based:

| Input | Original ordinary operation |
|---|---|
| col2 start | M09e4 Double.Parse with F2c2b provider; conv.i4 at1532, then add map.F100b at153e |
| col3 type | M09e5, masked flags; bit4 subsequently assigned separately |
| col4 sound | M09e5 at14a8; raw Int32 forwarded through factory, M3201:0061 direct F2526 store; no circle sound normalization |
| col5 end | M09e4 with2147483647 limit/validation flag at1b68; conv.i4 at1b6d |
| optional col6 sample | If row.Length>6, split':'; first two cells Convert.ToInt32 base/addition at1aef/1afe; index optional default0; volume optional Int32 clamped0..100; fifth cell filename or String.Empty |

M09e4 with validation=false returns parsed Double before bound/NaN checks; finite
in-range conv.i4 truncates toward zero. Do not infer pinned-CLR exceptional cast
outputs. Original ordinary errors rethrow at1d03; validation/skip caller differs.
Six-column spinner rows need no sample string. Present sample payload needs both
base/addition cells; malformed/short conversion/column access can fail before
factory. Other syntax/culture recovery is not broadened by this packet.

Nonvalidation parser takes **end=min(M3b0c()-50,truncatedEnd)+F100b** at1b70–1b99.
M3b0c returns Int32(globalF2367.F24b0), or0 if provider is null. This is an actual
host length-provider dependency, not a duration field encoded by the chart. Start
is truncatedStart+F100b. Fresh metadata M1bc1 resets F100b=0 at005b; header branch
0235–024c supplies0 for version>=5. Older/editor branch is outside supported scope.
Thus v5–v14 fixtures predict S2000/E5000 only **conditional on L>=5050**, delta0
and successful prerequisite helpers. The explicit cap fixture supplies L4000,
so E3950 and duration1950. No media/provider was executed, and the chart alone does
not establish L. A null provider would give cap-50, not invented uncapped timing.

M3201 directly stores supplied start/end F2523/F2524 at002a/0031 and fixed center
(256f,192f) at0008–0017. It does not clamp end>=start; E026's separate M0604 setter
uses Max(start,end) and reprepare, which must not be substituted for construction.
Zero/reversed duration controls characterize this distinction only; downstream
behavior is excluded. Work2 retains explicit engineering positive-duration guards.

## C. NewCombo, requirements and shared consumers

M3201 writes type8 at0038; if format<=8 it ORs4 at0059. This is a real intermediate
constructor flag. M28f7 then tests file bit4 at1bb2 and calls virtual M3e57 at1bb7:
true OR4; false AND-5. Only then M28f0 at1bbf→M28d2 registers the same reference,
inserts by existing comparator and stores F1805 at00d7. No spinner M2902 inference
occurs before construction. Therefore old-format bit4-free spinner ends false,
not true; bit4-on ends true. First-object combo numbering is not another flag write.
Following circle/slider infers true from the last registered spinner base type,
independent of its own bit4. Break forcing can later set flags (excluded fixtures).
M28db:00eb–00fc's <=8 spinner branch affects numbering state, not F2525. Preserve
that separate E036 contract; do not use constructor NewCombo as the parser result.

M05ea invokes M05f4 preparation. E026's exact expression remains:
`N=Int32(Double(Single(unchecked(end-start))/1000f)*preparedRate)`;
OD5/no-mod ordinary rate is5. For S2000/E5000, N15; acceleration coefficient
`0.00008+Max(0,((5000-duration)/1000)/2000)` gives mathematical27/25000.
Fraction-based expectations do not establish original JIT intermediate bit identity.
Existing NewComboPreparation already correctly projects constructor/parser order
for these versions; component-only tests verify it even when old canonical load
rejects. Those synthetic identities do not pretend to be a prepared old map.

M05f7 policy uses optional source date before2019-05-10 or source F1988<20190510;
null source=false. It reads no file-format field. Source-policy true changes first
angle/threshold behavior (E024–026). **Do not select legacy physics simply because
.osu format<14 or<=8.** First finite integration may retain the existing modern
component only for explicitly absent/false source compatibility, without adding
replay/recorded-source support or changing historical R07 behavior.

E041/M48e1 attaches base/addition/index/volume/extra filename after constructing the
spinner. Completion M3202 looks up timing at stored end, not base start+5; continue
channel and bonus/frequency lifecycle stay shared. E034 fresh score metadata counts
spinner rows and uses col2/col5 for summary; multiplier denominator uses registered
file start-span, not spinner duration. Pre13 difficulty Byte versus modern Single
metadata is already a shared original-derived contract. E035/E036 health source,
NewCombo grouping, registration identity, sample timing and session score sources
must retain original version and the same prepared spinner reference. Existing v14
controls exercise actual automatic score, fresh health, no-health-calibration
session and end-time sample metadata; no audio/playback judgement was executed.

## D. Finite Work2 handoff and acceptance criteria

Supported initial profile: fresh standard/no mods, positive integer start/end,
within existing duration/time/resource guards; full explicit timing fields and valid
absent/two-or-more-field samples; strictly ordered separated objects; no stack/break
ambiguity. Conditional original comparison requires source policy=false, delta0 and
L>=requestedEnd+50 so cap does not change the chart endpoint. If actual host length
is unavailable, retain and label the reconstruction's existing ENGINEERING uncapped
policy; do not claim the original end-cap was executed or silently guess L. A future
host-context input may implement the demonstrated cap separately. Full source-
record legacy policy, fractional/NaN/overflow, zero/reversed durations, reuse/editor,
<=5 mixed-stack breadth, exotic samples, original media/runtime remain OPEN.

Acceptance before publishing integration:

1. Extend **canonical** and catalog admission for supported ordinary v5–v13 spinner
   rows coherently; preserve negative v3/v128/other modes and historical API contracts.
   Raw source version/rows must survive preparation; do not alter global syntax,
   slider limits, time ordering or all-mode admission to make these fixtures pass.
2. Match immutable finite original expectations for constructor/parser flags,
   center, start/end, raw sound, sample fields and conditional N. Retain real <=8
   constructor provenance and later overwrite; explicit file bit4 and following
   circle/slider flags must match. Scope fractional and degenerate controls as
   explicit engineering exclusions rather than invent original rejections.
3. Demonstrate the source-policy/host-length decisions explicitly. Do not implement
   file-version-selected physics or constructor end clamping. Where supported host
   context exists, L4000 cap fixture predicts E3950; otherwise disclose that as
   original-static only, outside the no-cap comparison subset.
4. Feed the same prepared identities into fresh health/NewCombo, automatic score,
   end-time timing/sample selection, session input/spinner results and audio hooks.
   Exercise single and mixed fixtures on all seven requested versions; preserve
   L/B/P/C, stacking, input/audio/health/R17 historical contracts and tests.
5. Run focused development followed by the required exact-source integrated Windows
   regression/build for the admission change. Work1 then independently checks
   actual new production; owner original/ohno visual/audio/input qualification
   remains Issue#4. Do not relabel old production rejections or this static packet
   as successful new-source QA or promise113 recovered owner charts.

# 3. Pseudocode

RECONSTRUCTED from CONFIRMED ORIGINAL static evidence; conditional host state:

```text
parse common row start/type/sound; retain original source version
if reached Spinner8 branch after Normal1/Slider2 priority:
    parse optional sample base/addition/index/clampedVolume/filename
    end=truncate(DoubleParse(column5))
    if not validationCaller: end=min(globalLengthProvider()-50,end)
    start=truncatedStart+mapOffset; end=end+mapOffset
    construct ordinary spinner(center256,192,start,end,rawSound)
    constructor: set Spinner8; if format<=8 set NewCombo4
    parser: overwrite NewCombo with file bit4
    attach factory sample fields; register same reference
prepare requirement from duration and shared OD rate
apply shared registration/break/numbering and score/health/sound contracts
select legacy source policy independently of file format
```
