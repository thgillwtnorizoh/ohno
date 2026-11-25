# C001 functional/dependency inventory — checkpoint 01

Original-supported domain inventory is OPEN; this is the complete identified
production entry-point inventory at baseline, not an assertion that all original
branches have already been discovered.

| Entry/consumer | Original authority | Current contract / remaining dependency |
|---|---|---|
| Beatmap.Load | E004–006, E032 | OsuDocument → bounded standard circles/L/B/spinner projection; attached raw document |
| LegacyScoreMetadata.ReadFresh | E034 | document entries in original source order; public headerless compatibility adapter retained |
| LocalChartCatalog.Index | E043, engineering index | byte fingerprint → document → metadata projection; index admits more than gameplay |
| StarAdmission.Read/Parse | E044–046 | same syntax; strict v14 circle-only no-stack profile retained; mathematics unchanged |
| CalculatedStarCache | E044–046 + engineering | fingerprint/profile key; stale publication guards retained |
| NewComboPreparation | E036 | raw registration order/types + breaks; ordinary fresh ownership provenance |
| SliderGeometryPreparation/PrepareSlider | E005/006/016/021/022/032 | typed segments, lengths, movements/checkpoints; L and version-aware B; P/C pending |
| CircleSession / ledger | E011–019/033–036 | consumes prepared definitions, K, health source; regression required |
| CircleHitSounds | E037–041 | circle/slider/spinner sidecars + source timing order; short-row defaults unresolved |
| Rendering/PlayfieldLayout | E008/009/014 + engineering | prepared object positions/movements; full translated original stacks pending |
| Program/GameWindow | R02–17 + E043 | selected fingerprint, async loading, session/audio/rank entry; owner physical qualification pending |
| BeatmapAudio / resource library | E010/037–041 + engineering | local contained references, optional independently supplied media; original resource semantics incomplete |
| SelectPlay/Rank flow | E027/042/043 + engineering | identity, captured chart, Back/Retry; no extra independent parser |

Additional non-production parsers are evidence tools, fixture generators and QA
oracles; keep them independent. Do not rewrite historical original fixtures to
match production. All independent and development csproj files directly linking
Beatmap.cs remain supported; shared syntax is deliberately kept in that file.

Original method dependencies: M1bc1 metadata sections, M28f5 loading, M28f7 object
parse, M28ff timing, M28ea postprocess, M28eb/M28ec stacks, M144a geometry/timing,
M2d62/M2d64 samplers, M2902 NewCombo, M48de/M48e0 ordinary factory. Their general
mode/default/malformed branches still require Work 1 inventory. E005 corrects
E004's historical Taiko/standard dispatch attribution; do not use E004 alone.

## Issue #5 diagnostic consumer — 2026-11-14

CorpusAudit links unchanged production ChartFingerprint/LocalChartCatalog.Index,
OsuDocument/ReadStage/type priority and CatalogChart.LoadCanonicalGameplay; legacy
Load(string) is not audit admission. Actual ScoreConfiguration, fresh session with
health disabled and HealthChart.FromMap/Validate are independent readiness stages.
No UI/audio/clock/offset or R17 math change. Exact8c5ba835 focused Windows/Linux
239PASS each; owner/independent Work1 tool QA pending. See09-issue5-corpus-audit.md.


## Issue #7 finite v5–v13 spinner verified integration — 2026-11-15

Exact production2a2e29177d5f1173c711cf3d12d70be2f1759e09, starting7fc7636.
Windows43711842075/job132804890163 SUCCESS; SDK8.0.428/runtime8.0.34,Release0/0,
self-contained win-x64 package14160976919 verified by download/source/hash inspection.
New653/653 Work2 checks,33 unchanged fixtures=23admissions/10exclusions; retained
2309historical QA,C001264/P299/C4375,stack43+394,finite106+4,Audit259PASS.
Initial8a7e7b9 gate failed one unchanged QA14 old-spinner catalog control; corrected
nonzero/default stackleniency admission and added three controls; initial failure
retained. Full mandatory checkpoint12-issue7-verified-integration.md, evidence under
versioned-spinner-development. Constructor<=8NewCombo then parser overwrite, raw
times/samples and matching real score/health/session/results/audio owners integrated.
Historical API,v14,source-compatibility physics,R17,sliders/timing/offset unchanged.
Host cap remains ENGINEERING uncapped without provider; v3/v128/other-mode/fractional/
degenerate and broader stack/break/parser profiles explicitly excluded. Original/GUI/
audibleaudio/physicalinput/privatecorpus NOT RUN. Work1 independent new-source QA
next; owner CorpusAudit only afterward. C001/Issue#7 OPEN; no owner acceptance.


## Issue #9 Work2 — reproducible gate contradiction — 2026-11-16

Starting f9cce9e; exact detached candidate2c212abd963cdc5473d72d06bfc4207f74b56ca7
retained as patch/thin Git bundle, NOT integrated into main. Candidate Route M reuses
modern pass unchanged; Route Z checks actual heads/both endpoints with original strict
Single distance primitive, cancellation and1million pair bound; six/seven timing floor.
Debian13 SDK8.0.427/runtime8.0.33:801/801 new development PASS,81immutablefixtures=
66prepared/15excluded individually; actual score/calibrated health/NewCombo/session/
sample identities/audio admission/coordinates checked. No claimed owner-chart gain.
BLOCKED: unchanged QA14 C/spinner-old requires Unsupported, while HQ Issue9 explicitly
requires Indexed metadata candidacy; exact filtered check0PASS/1FAIL(exit1). Preserved
Issue7 suite also aborts at obsolete blanket nonzero-stack exclusion. No checker
changed, no special-case production rejection. Main production remains2a2e291.
Full Windows2309 regression/Release application package/additional integrated QA
NOT RUN after focused blocker; original/GUI/audible/physical/privatecorpus NOT RUN.
Fixture81+33 regeneration/static55/prior548seal checksPASS. Initial new harness
CS0103 and QA14 incomplete scratch-root packet invocation failures retained/corrected.
Full UEP13-issue9-integration-blocker.md, source/results/hashes/reproduction under
legacy-spinner-development. Headquarters next resolves versioned acceptance gates
without rewriting historical evidence; Work2 then completes full integration gates,
Work1 independent new-source QA afterward. C001/Issue9/subsystemOPEN; no owner action.


## Issue9-C01 verified integrated checkpoint — 2026-11-16
Startingd66cc28, recovered2c212abd; actual tested/publishedsource9ed9eb5c58f631f89fb66fc97802cfbf9ca86e44.
HQ6908226762 supersession snapshots/ledger preserve old QA14/Issue7 assertions.
Active QA14 gains3controls; Windows2312/2312active historical PASS,QA14headless237/
Windows30. Issue7 656PASS; Issue9 801PASS/81immutablefixtures=66admitted15excluded.
Full Windows43813166021/job133119149186 SUCCESS,SDK8.0.428/runtime8.0.34,Release0/0
and portable app publish; downloaded artifact14201431678 source/hash/packets verified.
Retained C001264/P299/C4375,stack43+394,finite106+4,CorpusAudit259PASS.
Initial Windows6f831c2 failed162byte assertions due checkoutCRLF; all66/15 semantics
matched. Added -text fixture policy (bytes unchanged) and workflow trigger, rerunPASS;
failed log sealed, no hidden failure/expectation edit. Linux QA14 threeWindows-only
failures preserved, native rerunPASS. Full UEP14-issue9-c01-integration.md and
legacy-spinner-c01/windows.json. Coverage: finite nonzero/default legacy stacking
RouteM/RouteZ and six/seven explicit-sample timing now integrated; no owner gain count.
No early actual v5/general stacks/short2–5/global defaults/host cap/replay expansion.
Original/GUI/audible/physical/privatecorpus NOT RUN. Work1 independent exactnewsource
QA next. C001/Issue9/subsystemOPEN; no additional owner manual action requested.


## Issue11 Work2 — RouteZ diagnostic classification — 2026-11-20
Startingc6447af, exact corrected sourcef6d0411abc865134a1dafb7fb7ef1473b621a84f.
Actual baseline34checks reproduce unproven=unclassified/generic worklimit; two exact
ordinal InvalidDataException mappings now yield unsupported-admission/stack-certificate-
unproven and stack-certificate-work-limit. Gameplay tree identical to independently
verified9ed9eb5; no admission/geometry/timing/score/health/R17 change. New36+retained259
=295PASS localSDK8.0.427/runtime8.0.33. Exact-source Windows/Linux focused44658294123
295PASS EACH,0NOT RUN/0warnings/errors,SDK8.0.428/runtime8.0.34; both downloaded tool
ZIP/source markers/DLL/report hashes verified. Unchanged historical probe9/10 locally
and actual Linux package:only obsolete8c5ba835 source pin fails,not new-source QA.
Campaign-record publication also triggered full Windows44658294130/job136029877254:
2312historical PASS,retained C001264/P299/C4375/stack43+394/Issue9 801/Issue7 656/
finite106+4/Audit295PASS;Release0/0,self-contained application publishPASS. App ZIP
metadata retained,not downloaded/launched; focused tool ZIPs verified.
Full UEP15-issue11-route-z-diagnostics.md and issue11-diagnostics seals/baseline/
fixture hashes/adversarial/real-fingerprint-precedence evidence persisted. No owner
chart access/scan request; which owner33 cases reach these throws NOT VERIFIED.
No original/runtime/GUI/audible/physical/private corpus execution or compatibility
gain. Next Headquarters assigns independent Work1 new-source diagnostic QA;
Issue11/C001/subsystemOPEN. Prior fixtures/tests/failure results preserved.


## Issue15 verified finite circle-free v5 integration — 2026-11-20
Starting61e8981, accepted researchd013dc18; exact tested/published functional
source22a403505f13461477343b9c6a251980716dd90f. Only circle-free Mode0v5 spinner+slider
positive/default genuine leniency exempts earlier-spinner proximity; earlier-slider
head/geometric/final endpoints retained. All-object1m pair cap before exemptions,
entry/outer/inner cancellation retained; no internal spinner height modeled/zeroed.
New Work2 suite all100immutable fixtures59/41,exactly12new positives individually;
local1502PASS,Windows1503PASS including clean compiled source stamp. All59 actual
score/calibrated health/NewCombo/session/samples/WAV resources checked;12exact slider
geometry and real input/result/recording-hook histories match comparison controls.
Fresh Windows44774297221/job13641985280052stepsSUCCESS,SDK8.0.428/runtime8.0.34,
Release0/0,self-contained win-x64 publishPASS. Historical2312PASS; retained C001264/
P299/C4375/stack43+394/Issue9 801/Issue7 656/Issue11 295/finite106+4PASS.
Downloaded artifact14648998351 size/digest/source/executable and100fixture hashes
verified; app not launched. No assertion failures; initial1429run retained before
strengthening completion/delta/cancellation controls. Full UEP16-issue15-circle-free-v5
report and issue15-development source/results/dispositions/CI/artifact seals.
Circle-bearing,other versions/profiles,early actual stacks and general CS behavior
remain excluded. Modern stack pass,distance primitive,session/audio/classifier and
R17 unchanged. Prior research1261/independent QA remain historical,not new passes.
Original runtime/GUI/audible/physical/private owner corpus NOT RUN. No owner31
recovery forecast or manual action. Next Headquarters review then Work1 independent
actual-new-source QA; Issue15/C001/subsystemOPEN,Issue4OPEN.


## Issue18 verified canonical1–32 integration — 2026-11-21
Starting127e80a, accepted22a4035; final tested/published source6a152648c72773eb33d37f8ac01f18ca35ab1f41
(gameplay identical to introducing2d6768c). Explicit canonical32/legacy16 profile
shared by row and sample admission. Historical string/public metadata1–16 and named
R12S25/QA10M16 refusals preserved. CheckedInt64 remaining movement/segment preflight,
EXACT bounded checkpoint plan before repeated movement arrays, responsive stage/
loop cancellation and no partial parent/effects publication. Numeric clocks/parity/
N/R/pending E17/nonmonotone deadlines remain.106unchanged fixtures86prepared/20refused,
exactly45new/0losses individually.82real canonical audio-ready consumers,4stored
sample-error preparations. Local3973PASS,Windows3974PASS(extra clean source stamp).
Stress248376movements across3parents,98304checkpoints,97464segments andoverbudget/
Int64/8192geometry/deterministic cancellation checked. Fresh final Windows44892345157/
job13681068109753stepsSUCCESS,SDK8.0.428/runtime8.0.34,Release0/0,self-contained win-x64
publishPASS. Historical2312 and retained C001264/P299/C4375/stack43+394/Issue15 1503/
Issue9 801/Issue7 656/Issue11 295/finite106+4PASS. Downloaded artifact14702004442
source/ZIP/exe/actualpacket/106fixture hashes verified; app not launched. Initial
compile/stress failures retained(test-only mistaken member and Catmull-count workload);
initial full2d6768c gate succeeded, final gate repeats with stronger actual-resource
resolver assertions. Full UEP17-issue18-canonical-traversals.md andissue18-development
seals/source hashes/dispositions. No research/old fixture/test rewrite, no classifier/
R17/near-spinner/score/health/audio algorithm change.32isENGINEERING,not originalmax;
no9000/9001 execution. Original/GUI/audible/physical/privatecorpus NOT RUN. No owner
rescan or manual action. Next HQreview then Work1 independently verifies exactnew
production beforeIssue18closure. C001/Issue18/subsystemOPEN;Issue4OPEN.


## 2026-11-22 — Issue20 canonical source-L96 development publication
Starting131c788; accepted6a152648. Explicit internal canonical Linear96 capability;
public MaxControls/APIs/legacy and B/P/C64 preserved; canonical32/legacy16 traversals
unchanged.168immutable cases112prepared/56excluded,+74/0losses; genuine canonical
consumers and exact250000movement/100000prepared-segment/100000checkpoint maps
plus one-over, full-loader cooperative cancellation and custom map-byte cleanup.
Linux development5429PASS, retained264/299/4375/3973/1502/801/656/295PASS.
Only Beatmap functional source changed; re/ and old expectations read-only.
Full records18-issue20-linear-controls.md/issue20-development including initial
failed test assumptions and environment failure. Fresh Windows gates PENDING, not
substituted by Issue18 CI. No independent new-source QA or original equivalence.
Issue20/C001/subsystem/Issue4OPEN; next Work2 Windows gate then HQ and Work1.


## 2026-11-25 — Issue24 delivery dependency reconciliation

Canonical timing initialization has existing Work2 Windows86 timing550/550 and
connected consumer regression evidence at346638cec4548023dbb480710327cd8efa31ce69
(functional f76c26a). Source comparison shows no production/harness difference
between those commits. Artifact retrieval/inspection is BLOCKED on the current
host; ZIP bytes, bundled source/application seals and delivery remain NOT VERIFIED.
See issue24-development/windows86-recovery/checkpoint.md for exact CI counts,
commands, reproduction and dependencies. Work2 must inspect the existing artifact
and run unchanged verify_delivery.py on a complete exact source checkout; no new
build/regression/corpus scan. HQ then assigns independent Work1 QA. No Work1
dependency-register or original evidence change. C001/Issue24/Issue4 OPEN.
