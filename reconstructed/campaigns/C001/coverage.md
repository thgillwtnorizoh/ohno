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
| E Geometry: Perfect-circle/Catmull/aliases | E006 branch inventory only | raw retained; gameplay rejects | Work 1 helper research required |
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
