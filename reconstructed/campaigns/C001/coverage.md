# C001 coverage matrix — OPEN

Inventory disposition: preliminary complete domain categories, original branch-by-branch closure pending Work 1. No row is marked subsystem-complete. Raw retention does not prove semantic preparation. Finite independent verification at85ec7bf:106/106 declared fields plus4 engineering controls PASS; complete branch/variant verification remains pending. Original runtime remains unverified for every row.

| Domain / feature | Evidence | Production checkpoint | Remaining coverage |
|---|---|---|---|
| A Recognition/syntax: Header/version recognition | E004/E034 | shared version stored; gameplay5–14/star14 admission | full original version domain open |
| A Recognition/syntax: Encoding/BOM/line endings | R16 engineering | shared BOM-aware reader; source lines retained | original encoding/error fallback open |
| A Recognition/syntax: Whitespace/comments/sections | historical loaders | one lexer for production consumers | stage-specific filtering/Enum section state implemented; complete error closure open |
| A Recognition/syntax: Key syntax/duplicates/unknown sections | E004/E043 partial | all ordered entries retained; projection duplicate policies explicit | complete original key inventory open |
| A Recognition/syntax: Numeric/culture/overflow/recovery | E005/006/034 partial | existing finite invariant projections retained | full malformed/partial loading open |
| B Sections/fields: General | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| B Sections/fields: Editor | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| B Sections/fields: Metadata | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| B Sections/fields: Difficulty | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| B Sections/fields: Events | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| B Sections/fields: TimingPoints | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| B Sections/fields: Colours | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| B Sections/fields: HitObjects | E004/E043 partial | all raw entries retained | fresh difficulty settings typed and integrated; remaining recognized fields/defaults open |
| C Timing/inheritance: Base/inherited normalization | E005/E016/E037–041 | existing bounded timing/sample preparation retained | general branch/default/error closure open |
| C Timing/inheritance: List order and same timestamp | E005/E016/E037–041 | existing bounded timing/sample preparation retained | general branch/default/error closure open |
| C Timing/inheritance: Truncated query versus raw timestamp | E005/E016/E037–041 | existing bounded timing/sample preparation retained | general branch/default/error closure open |
| C Timing/inheritance: SV/meter/effect/sample inheritance | E005/E016/E037–041 | existing bounded timing/sample preparation retained | general branch/default/error closure open |
| C Timing/inheritance: Slider duration/tick rounding | E005/E016/E037–041 | existing bounded timing/sample preparation retained | general branch/default/error closure open |
| D Object construction: Circles/source flags/positions | E004/011–014/034/036 | bounded circle preparation; raw order retained | full flags/modifiers/defaults open |
| D Object construction: Sliders/repeats/samples | E005/006/016/021/022/032/039 | bounded L/B prepared data + sidecars | P/C/degenerate domain open |
| D Object construction: Spinners | E024–026/041 | v14 standard bounded prep | older versions/alternate modes open |
| D Object construction: Holds | E034 metadata only | raw payload retained; standard gameplay rejects | mode-specific construction/prep open |
| D Object construction: Combined flags/combo skip/sample overrides | E036–041 partial | raw flags and actual registered type retained through runtime/health/NewCombo | all factory combinations open |
| D Object construction: Sorting/registration/equal times | E004/036 partial | raw order preserved; prepared current sort; equal time rejected | original tie rules and registration closure open |
| E Geometry: Linear multi-control | E006 | implemented pair flags, Int32 controls, common adjustment | extreme/degenerate and head precision open |
| E Geometry: Bezier5–6 | E032 | backward duplicate boundary + adaptive | runtime/edge closure open |
| E Geometry: Bezier7–8 | E032 | forward duplicates, adaptive no pair bypass | runtime/edge closure open |
| E Geometry: Bezier9 | E032 | 50*n sampler excludes endpoint, pair bypass | runtime/edge closure open |
| E Geometry: Bezier10–14 | E032 | adaptive weighted leaf + pair bypass | runtime/edge closure open |
| E Geometry: Perfect-circle finite P | Work1 05-perfect-circle /21 exact fixtures | a6c7dab canonical source/prepared kinds, L/B fallbacks, arc sampling/flags and shared tail/consumers;299 development checks PASS | subsequent Work1 independent P QA; editor/exceptional/zero-output/general legacy remain open |
| E Geometry: Catmull/remaining aliases | separate original research required | unsupported; unchanged explicit rejection | outside Issue #2; OPEN |
| E Geometry: Length/truncation/extension/cumulative | E006 | shared Single-segment/Double cumulative pass | all exceptional states open |
| E Geometry: Traversal/movements/checkpoints | E005/016/021/022 | shared existing typed definitions | full malformed/extreme branch closure open |
| F Preparation: StackLeniency/threshold | E004/006/036/045 partial | raw/prepared distinction preserved; existing restrictions explicit | original general algorithm/relevant variants open |
| F Preparation: Positive/negative stacks | E004/006/036/045 partial | raw/prepared distinction preserved; existing restrictions explicit | original general algorithm/relevant variants open |
| F Preparation: Old versus new stacking | E004/006/036/045 partial | raw/prepared distinction preserved; existing restrictions explicit | original general algorithm/relevant variants open |
| F Preparation: Coordinate displacement and raw positions | E004/006/036/045 partial | raw/prepared distinction preserved; existing restrictions explicit | original general algorithm/relevant variants open |
| F Preparation: Effective CS/AR/OD/HP/mods | E004/006/036/045 partial | raw/prepared distinction preserved; existing restrictions explicit | original general algorithm/relevant variants open |
| F Preparation: NewCombo/break grouping | E004/006/036/045 partial | raw/prepared distinction preserved; existing restrictions explicit | original general algorithm/relevant variants open |
| F Preparation: Mode conversion/registration | E004/006/036/045 partial | raw/prepared distinction preserved; existing restrictions explicit | original general algorithm/relevant variants open |
| G Events/resources: Breaks/event ordering | E034/E037–043 partial | raw declarations retained; existing break/local audio projections | complete original interpretation/resource resolution open |
| G Events/resources: Background/video | E034/E037–043 partial | raw declarations retained; existing break/local audio projections | complete original interpretation/resource resolution open |
| G Events/resources: Storyboard declarations/loops/triggers | E034/E037–043 partial | raw declarations retained; existing break/local audio projections | complete original interpretation/resource resolution open |
| G Events/resources: Samples/filenames/paths | E034/E037–043 partial | raw declarations retained; existing break/local audio projections | complete original interpretation/resource resolution open |
| G Events/resources: Colours/resource fallback | E034/E037–043 partial | raw declarations retained; existing break/local audio projections | complete original interpretation/resource resolution open |
| H Modes: Standard | E005 corrected dispatch; remaining partial | raw mode-specific payload retained; standard engine only | full supported loading/preparation domain open |
| H Modes: Taiko | E005 corrected dispatch; remaining partial | raw mode-specific payload retained; standard engine only | full supported loading/preparation domain open |
| H Modes: Catch | E005 corrected dispatch; remaining partial | raw mode-specific payload retained; standard engine only | full supported loading/preparation domain open |
| H Modes: Mania | E005 corrected dispatch; remaining partial | raw mode-specific payload retained; standard engine only | full supported loading/preparation domain open |
| I Integration/acceptance: Canonical complete typed raw/prepared contracts | C001 authorization | see consumer-inventory and status | pending complete campaign gate |
| I Integration/acceptance: Cancellation/failure isolation | C001 authorization | see consumer-inventory and status | pending complete campaign gate |
| I Integration/acceptance: Revision/cache invalidation | C001 authorization | see consumer-inventory and status | pending complete campaign gate |
| I Integration/acceptance: Catalog/gameplay/difficulty integration | C001 authorization | see consumer-inventory and status | pending complete campaign gate |
| I Integration/acceptance: Historical2309 independent QA + retained dev | C001 authorization | exact Windows2309/2309 and all retained development steps PASS at85ec7bf | continue regression for remaining original domain |
| I Integration/acceptance: Independent C001 production QA | Work1 unchanged independent suite | strict106/106 declared fields and4/4 engineering controls PASS at85ec7bf | complete original branch/variant coverage open;10 probes not passes |
| I Integration/acceptance: Windows Release build/portable artifact | exact-source Windows run43455800196 | Release0warnings/0errors and self-contained artifact14053703311 PASS | owner actual application qualification pending |
| I Integration/acceptance: Human application qualification | C001 authorization | see consumer-inventory and status | pending complete campaign gate |
| I Integration/acceptance: Reproducible corpus/provenance | C001 authorization | see consumer-inventory and status | pending complete campaign gate |
| I Integration/acceptance: Original R17 display discrepancy analysis | C001 authorization | see consumer-inventory and status | pending complete campaign gate |

## Issue #1 bounded-cycle delta — 2026-11-14

>5 standard stacking remains **BLOCKED / NOT IMPLEMENTED**, pending targeted Work1
B1–B3 height propagation, displacement operands and expected final-coordinate
fixtures. Existing zero-stack proof remains bounded to its prior profile. No new
coverage row is marked complete. Focused existing-evidence audit passed10 consistency
checks; it is not new functional/independent production coverage. See
04-issue1-stacking-evidence-blocker.md. <=5/modes/modifiers/ties remain OPEN.

## Issue #1 verified implementation delta — 2026-11-14 (supersedes earlier blocker for finite scope)

Work 1's published `03-standard-stacking.md` and 18-fixture corpus resolved the
B1–B3 research blocker for finite >5/standard/CS5/AR5/full-range/no-mod/512x384
preparation. Work 2 implementation `52c300a9b508567987331c54f51a0944f9c5b8fd` integrates height computation and
coordinate translation across circle heads, prepared slider controls/segments,
movements, endpoints, checkpoints and registered sample operands. Exact-source
Windows run43540928098: 43/43 independent-fixture-derived **development**
head assertions PASS, 20 slider consumer controls PASS, all 264 C001 development
checks PASS; unchanged finite Work 1 independent fields106/106 and controls4/4
PASS, historical workflow stages PASS, Release0 warnings/errors. The historical
'BLOCKED' row above is a preserved chronology, not the current status.

**OPEN:** Independent Work 1 production stacking verification (original runner),
<=5/partial editor, variable CS/AR/mod/viewport, ties, other modes, original
runtime/human qualification, all other C001 rows. No subsystem completion claim.

## Issue #2 verified finite P delta — 2026-11-14

Actual sourcea6c7dabc65372f83641926b89384cd14b3813a71; Windows43575301213 PASS.
P299/299 development,21 fixture dispositions (17finite/4explicit rejections),
retained C001264/264, historical2309 and retained suites, stacking43+394 and finite
106+4 regression PASS. Work1's original fixture values remain unchanged; v9 P4
oracle scope is dispatch only. Independent new-source P QA and owner/runtime remain
PENDING. No Catmull, full legacy/editor/exceptional/zero-path or star expansion.

Issue#3 C delta: explicit Catmull0, original Single polynomial/asymmetric stencils,
50 endpoint pairs per control span with preserved duplicates/flags, shared tail/
cumulative/traversal/checkpoint/stack/input/sound/health/NewCombo integrated at
2d776dac.4375development+exactWindows full gate PASS.13admitted/6engineering rejects;
independent C and owner/runtime qualification PENDING; comprehensive C001 OPEN.
