# Reconstruction journal

Work 2 implementation records. Research remains owned by Work 1 under `re/`.

## R01 — Finite Transition-Progress Kernel

Date: 2026-10-26 (Asia/Bangkok). Dedicated branch:
`work2/r01-transition-kernel`, based on research commit
`586ed1d668d535533cd5822cf0fdd6790b61d5d0`.

Objective: implement only Expedition 02's finite progress projection, with no
original-software investigation. Read the full pinned contract, contexts, report,
and published progress-method evidence record. No binaries were read, no original
experiments or research tools were run, and no files under `re/` were changed.

Implementation: explicit finite phase/progress/rates/scale inputs, immutable
result, optional completion hook; phase 0 tests >=100 before min-clamped addition
and phase 1 tests <=0 before max(-1)-clamped subtraction. Completion returns phase
3/2 while preserving progress. Arithmetic crossing does not complete immediately.
Unspecified phases are rejected rather than given guessed behavior.

Evidence: `re/02-static-gameplay-initialization-and-control-flow.md`, finite
arithmetic finding/pseudocode and two validation sequences; corroborating metadata
in `re/evidence/02/static-control-flow.json` for method `0x060046a3`. Detailed
field/offset traceability, API policies and Work 1 requests are in
`R01-transition-kernel.md`. Confidence is inherited CONFIRMED static projection;
original dynamic equivalence is UNRESOLVED. No hypothesis was promoted.

Files added: `transition_progress.py`, `tests/test_transition_progress.py`,
`fixtures/r01-finite-cases.json`, `R01-transition-kernel.md`, this journal, all
under `reconstructed/`. No original assets or code included.

Actual validation on Python 3.12.14/Linux:
`PYTHONDONTWRITEBYTECODE=1 python -m unittest discover -s reconstructed/tests -v`
passed 12 test methods including 22 explicit static-contract fixture cases.
The tests are implementation validation against Work 1's branch-derived oracle
and explicit API policies, not original-game observations. No dynamic gameplay or
pixel comparison ran. Diff/ownership and remote publication checks are reported
in the final handoff; their success is not assumed here.

Limitations: no default state, full phase lifecycle, scheduler, clock units,
rendering, mode/commit effects or original callback semantics. Nonfinite inputs
and unbounded overflow are outside the API; bit-level CLR equivalence is untested.
Next dependency: Work 1 must specify lifecycle, timing/rate origins, delegate and
side-effect ordering, plus original-run reference traces before host integration.

User scope explicitly places R01 documentation in `reconstructed/` and prohibits
editing `re/`; therefore no shared research journal update or new numbered
reverse-engineering mark is made. No other contract exception is used.

## R02 — First Human-Playable Circle

Date: 2026-11-02 (Asia/Bangkok). Work 2. Started current `origin/main` at
`38dee7561569210f25c49f9eb6810bc0a06f634b`; canonical publication to `main`, no
milestone branch. Read AGENTS.md/full contract/journal/R01 and relevant E04,
E07–E14, E20, E27 material, preserving the ordinary/Taiko correction and E20's
preparation-only status. No original archive/binary inspection, research tooling,
original-game experiment, OBS or CDB.

Implemented `CircleGame/`: .NET 8 WPF Windows application, independent circle
fixture, local narrow `.osu` reader, separate gameplay/layout/clock/input/render
responsibilities, real mouse and Z/X key-down edges, approach timing/size/radius,
strict judgement windows, expiration, visible feedback/combo/accuracy, results and
restart. Added Windows launcher/run instructions and `R02-first-circle.md` with
source-to-research mapping, exact limits and handoff. R01 files/tests retained.

Actual checks: Linux .NET SDK8.0.427 Release build and explicit framework-dependent
`win-x64` Release build both SUCCEEDED with 0 warnings/0 errors. Windows GUI PE32+
apphost generated and fixture copied. Launch attempt via dotnet returned150 before
window creation: Microsoft.WindowsDesktop.App unavailable on Linux; no graphical
Windows session or Wine. **Windows launch, physical input, visible judgement and
restart are UNVERIFIED**, not a proven playable build. No automated tests/QA
harness/corpus were created or run; independent QA belongs to Work 1.

Research-derived finite circle rules are separated from slice policies:
Stopwatch silent clock, WPF primitives/DIP letterbox, full-list catch-up scan,
simplified admission, immediate judged-object removal, practice counters,
focus/pause/restart and local last-circle+3000 result overlay. No stable score,
complete candidate/record/transition pipeline, audio sync, mods or pixel identity
claim. Source remains complete and Windows-buildable; original runtime fidelity
and actual Windows usability need separate evidence. See R02 report for details.

The user's R02 assignment explicitly authorizes actual application development,
removes routine original-runtime experiments as a prerequisite, assigns QA/tests
to Work 1, and requires main-only publication and implementation-only records.
It supersedes historical suspension/branch instructions. All changes stay under
`reconstructed/`; no shared research journal edit or additional RE number, and no
other contract exception. Final handoff records actual remote publication and
preservation checks. Stop after R02; Work 1 performs subsequent independent QA.

## R03 — Audible Circle Practice

Date: 2026-11-02 (Asia/Ho_Chi_Minh). Work 2. Started latest `origin/main` at
`2c569937fde3e258dd703ead043daa05e78e4b52`. Read canonical rules/contract/journal,
QA01 README and headless source, and corresponding application code. No RE
expedition, original binary/tool execution, interactive window operation,
simulated OS input, OBS, CDB, VM or desktop provisioning.

Source-confirmed fixes: QA01-D01 now requires ordered distanceSquared<=radius²
and retains finite inclusive equality; QA01-D02 parses mandatory hitSound column5
as invariant Int32, rejects malformed/empty input and adds no sound-bit mask.
CircleSession timing/admission/classification/expiration are otherwise unchanged.
Work 1 QA tests/results/provenance remain untouched; no passing QA run is claimed.

Implemented optional local AudioFilename resolution; UI-thread-owned WPF
MediaPlayer async preparation with15-second timeout; staged candidate replacement
retaining the old paused session on load error; start/pause/resume/zero-seek
restart; short-track/end/results policy; close/dispose on replacement/exit and
playback-error pause. Silent fixture remains default. Added independent audible
map template and C# PCM tone generator, invoked by human through an Audible
fixture button; no commercial/original assets. Active README updated and
`R03-audible-circles.md` added; journal append only. All changes are reconstructed/.

Clock remains Stopwatch/latest WPF frame time, not audio/BASS position. Media
startup/seek/device latency, stalls and drift are uncorrected; no offsets,
calibration or original equivalence claimed. Short media ends into silence while
the map timeline continues; restart rewinds, results pause longer media. Failed
candidate loads preserve the prior session; a later post-commit decoder failure
pauses the current session rather than resurrecting the previous disposed map.

Actual validation: source review and repository hygiene only. SDK absent from
PATH and historical temp path; automatic approval review rejected an SDK download
as conflicting with the existing-SDK/environment-repair limit. No bypass or
further environment repair. **R03 build NOT RUN; QA01 headless C# suite NOT RUN;
Windows GUI/audio/physical input and generator execution NOT RUN/UNVERIFIED.**
R02's old build result does not certify R03. No dedicated QA campaign was created.
Final handoff records diff/preservation and actual main publication verification.

R03's current authorization supersedes historical Work 2 suspension and permanent
milestone branches; all interaction remains exclusively human-controlled. No
research-journal edit or additional numbered expedition; no other contract
exception. Stop after R03. Work 1 owns subsequent independent QA.

## R04 — First Human-Playable Slider

Date: 2026-11-02 (Asia/Bangkok). Work 2. Started latest origin/main at
`9ab59fbe4c7b6f68875cb755f50959f00f308187`. Read canonical contract/journal/R02/R03
and relevant E05–E06/E11–E18/E21–E23. No original-code investigation or runtime.

Implemented the bounded mixed circle/straight-slider client: parse Linear L|x:y,
one span, requested length and timing/SV; prepare geometry and rate-derived end;
reject generated interior ticks, unsupported curves/counts and overlapping slider
intervals. Separate head/tracking/terminal/parent state, real held Z/X and mouse
sampling, strict follow radius with history/remembered-side policy, entry/loss/
re-entry, adjusted terminal checkpoint, legacy aggregate grade and practice combo.
Slider path/ball persist after head judgement. Added independent mixed fixture and
Slider fixture button. Audio backend/clock and earlier controls remain unchanged.

Application changes/docs/fixture remain reconstructed/. The only new CI path is
.github/workflows/r04-build.yml, a companion using the established Windows2022
preinstalled .NET8 gate and unchanged QA01 90-case suite. Existing QA02 workflow,
all research/QA assets, R01 and historical reports are preserved. No new QA tests
or independent campaign. R04 report records mechanisms, known deviations and
source-derived fixture arithmetic. Initial CI state PENDING, not a claimed pass;
actual hosted result will be appended after execution. No game/GUI operation,
simulated OS input, recording, OBS, CDB, virtualization or SDK installation.

Scope remains head + terminal, no interior ticks/repeats/curves/full score/HP/mods.
Full-list attention, WPF cadence/primitives, practice counters and legacy policy
are explicit; precise original equivalence and successful human play are not
claimed. Current user authorization supersedes historical Work 2 suspension and
milestone branches. CI infrastructure enables the specifically requested gate;
no research journal edit or additional RE number. Stop after R04.

R04 hosted verification outcome (append after execution): source commit
`922d64b80e4c3c4f7ac886c791f7c49379071df1`, run41483942160,
job125409196900, https://github.com/thgillwtnorizoh/ohno/actions/runs/41483942160 .
2026-11-02 16:41–16:42 UTC; Windows2022 / preinstalled SDK8.0.427.
Release WPF win-x64 build PASS, 0 compiler warnings / 0 errors. Unchanged QA01
suite: 90 passed, 0 failed, 90 executed; job SUCCESS. Runner checkout emitted a
Node20 deprecation notice, separate from compiler warnings. No new slider tests,
game launch, input operation or audio playback; human slider play UNVERIFIED.
Research tree unchanged, historical journal preserved as a byte prefix, original
fixtures/audio/clock/R01 and Work 1 QA assets preserved. Final documentation-only
publication records this outcome with application content identical to the tested
source commit. R04 stops here; Work 1 owns the subsequent dedicated slider QA.

## R05 — Repeating Sliders & Scheduled Checkpoints

2026-11-03 (Asia/Bangkok), Work 2. Started current origin/main at
`b18867ea23607ead66d5daba9599d7255eb1d6a1`, clean local main. Read AGENTS, full
standing contract, implementation journal/sources and E05–E06/E15–E18/E21–E23.
Scope: extend playable straight-line sliders to repeats and scheduled checkpoints;
no new original-software expedition, curved paths, full score/HP/ScoreV2 or QA
campaign. Existing static findings retain their confidence/runtime limitations.

Owner-reported R04 observation from this assignment: the human owner reports
that the tested slider interaction and scoring match the original at the
user-observable level. This is limited to the owner's tested R04 interaction;
map/build/settings/trace details were not supplied here. It is not universal
equivalence, independent Work observation or R05 validation. Historical R04
report/journal conclusions are preserved.

RECONSTRUCTED implementation: 1–16 directed one-line traversals with inclusive
first-Movement selection at shared endpoints; persistent tracking across turns;
distance-phase/carry checkpoint production with original near-end suppression,
separate Single-distance timestamps and final-only adjustment; unsorted leading
due-prefix processing of at most one outstanding item per reached update; timely
entry equality, independent head, raw k/N/R success selector and processed-count
negative selector. Separate null/pending outcomes, success/failure counters and
producer UI roles. Ordinary failure resets combo; final negative does not; parent
legacy grade uses (successes+positiveHead)/(N+1), no aggregate combo or drain.

Changed implementation under reconstructed/ only: Beatmap/CircleSession,
PlayfieldView, Program, project/README, three independent playable fixtures,
R05 report and this append. Tick dots, TURN chevrons, span/checkpoint counts and
feedback are independent WPF aids. Existing R02–R04 fixtures, input/audio/clock,
R01, historical records and Work 1 research/QA preserved. Engineering limits:
16 traversals, 4096 checkpoints/slider,100000/map and existing bounded parser;
unsupported constructions explicitly rejected. Host snapshot cadence and legacy
practice counters remain limitations, not new stable claims.

Initial verification: source/diff review and whitespace checks only; no local
dotnet executable. Hosted Windows compilation and unchanged90 QA01 cases PENDING
at initial source publication; actual run/commit/outcome will be appended after
execution using existing R04-named workflow, with no workflow edits/new tests.
No game launch, physical/synthetic input, recording, OBS, debugger, original-game
execution, binary inspection, virtualization or SDK installation. All interactive
operation stays exclusively with the owner. Current R05 authorization supersedes
historical Work 2 suspension/branch rules; no research journal or RE number edit.
Stop after R05; Work 1 owns dedicated subsequent repeat/tick QA.

R05 hosted outcome (append after execution): source commit
`2583c550d63142b2c07134126b35c3593d4772ac`, run41489134446,
job125427203516, https://github.com/thgillwtnorizoh/ohno/actions/runs/41489134446 .
2026-11-03 00:13–00:14 Asia/Bangkok (2026-11-02 17:13–17:14 UTC),
Windows2022 / preinstalled SDK8.0.427. Release WPF win-x64 compile PASS with
0 compiler warnings / 0 errors; unchanged QA01 suite90 passed,0 failed,90 executed;
job SUCCESS. Runner checkout Node20 deprecation notice is separate from compiler
output. Existing R04-named workflow checked out this R05 source SHA. No new
gameplay tests or game/input/media execution. R05 owner play and repeat/tick
runtime equivalence remain UNVERIFIED. Final publication records only report/
journal outcomes; application/fixtures/workflow are identical to tested source.
Verified only reconstructed/ changed, research/QA/workflows and previous fixtures/
audio/clock/historical reports preserved, old journal kept as an exact byte prefix.
No new expedition or further implementation/QA. Stop after R05.

## R06 — Human-Playable Bézier Sliders

2026-11-03 (Asia/Bangkok), Work 2. Clean latest origin/main:
`8a92b88c32e11a5d6d513f732a47f540e5fd772a`. Read AGENTS/full contract (attachment
and repo identical), journal/R05, E32 and relevant shared slider/QA01/QA03 contracts.
No original-binary investigation, original-runtime work or new expedition.

RECONSTRUCTED E32 v14 preparation: exact forward-adjacent separator/final-pair
guard; Double→Int32→Single B controls; normalized head/order/duplicates; Single
second-difference².25 flatness, half subdivision left-first and weighted leaf
output, endpoint only at traversal end. Shared geometry records preserve raw
segments/boundary flags/G, requested L, post-adjustment segments and Double
cumulative lengths. E06 suffix removal tolerance/zero-direction guard and actual
recomputed totals feed per-piece forward/reverse Movement/checkpoint loops.
CircleSession judgement/held input unchanged. No generic curve library/sampler.

Cached frozen WPF prepared path at load, same layout mapping, current Movement
ball/ticks/follow feedback and traversal-end repeat indicators. Added four silent
independent v14 fixtures with Quadratic/Cubic/Joined Bézier/Mixed R06 buttons;
all old buttons/fixtures/audio/Stopwatch/R01/historical records preserved.
Engineering bounds:64 normalized controls,8192 raw pieces/50000 subdivision nodes
per slider,100000 prepared pieces/250000 Movements per map, existing R05 bounds.
No fixed original depth or universal fidelity is claimed. Unsupported types and
old Bézier formats explicitly reject; no Catmull/PerfectCurve/stacking/mods/full
scoring or audio redesign. Historical owner R04 feedback is not R06 validation.

Changes under reconstructed/: model/parser/preparation, cached rendering, UI/
project, four fixtures, README/report, bounded R06PreparationChecks and this append.
Only shared infrastructure edit: one preparation-check step/path trigger in the
existing Windows gate, enabling the requested finite checks; Work 1 research/QA
files and old reports/journals remain untouched. Four preparation targets plus
four fixture loads are bounded development checks against static E32 predictions,
not a QA04 framework or original behavior evidence. Hosted build/90 QA01/56 QA03/
preparation execution PENDING initially; actual outcome will be appended. No local
dotnet executable; no SDK installation. Source/diff and whitespace review only.

No game launch, OS input, gameplay operation, recording/OBS/debugger/original game,
supervised play or virtualization. All interactive play stays with owner. User
R06 continues implementation/main publication over historical suspension/branches;
no research journal/RE number edit. Stop after R06; QA04 belongs to Work 1.

R06 hosted verification outcome (append after execution): source commit
`0cf4030f3a0dbb7dddf6e8d5b98bfa5c2683d36d`, run41567855352,
job125693599596, https://github.com/thgillwtnorizoh/ohno/actions/runs/41567855352 .
2026-11-03 09:43–09:44 Asia/Bangkok (02:43–02:44 UTC), Windows2022,
preinstalled SDK8.0.427. WPF Release win-x64 build PASS:0 warnings/0 errors;
unchanged QA01:90 passed/0 failed; unchanged QA03:56 passed/0 failed. Four finite
preparation targets PASS (exact E32 raw quadratic/cubic/joined vertices and final-
pair guard), four fixture loads completed. Actual adjusted segments/Movements:
quadratic64/64,cubic96/96,joined128/128,mixed284/375. All steps/job succeeded.
Checkout Node20 advisory is separate from compiler warnings. These are actual
noninteractive implementation results, not original observations or human play.
Final documentation-only publication preserves the exact tested application,
fixtures/checks/workflow content; research/QA/journal prefix and prior assets/
reports/audio/clock/held-judgement code remain preserved. No QA04, additional
expedition, gameplay operation or claim of original runtime/pixel equivalence.
R06 stops here.

## R07 — First Human-Playable Spinner

2026-11-03 (Asia/Bangkok), Work 2. Started clean latest origin/main
`a3f414e1ab220a5d7c62d1bacf4aa7b4850e5985`. Read AGENTS/full standing contract,
journal/R05–R06, E24–E26, relevant E10–E13/E27 and QA01/03/04 contracts.
No original-binary inspection, debugger or new expedition. QA04/189 baseline
passes and its scoped owner-reported R06 feedback are preserved, not R07 evidence.

RECONSTRUCTED finite modern spinner: separate definition/state, atan2 relative
to an ENGINEERING mapped displayed center; modern first reference, strict wraps/
antipodal rejection, previous-angle update, interval EMA, stationary decay before
held/time checks; signed target then coefficient-limited U/clamp±.05, strict
S<T<E positive-interval Single progress capped1/update. Separate old-P integer
observation increments C once per inequality, selects raw0/4096/8192/16384 then
updates cache; no bonus payout or component combo. Required N preserves Single/
Double/int OD3/5/7.5 preparation (3000ms/OD5→15). Completion strictly T>E, modern
thresholds from C/N, no unread-progress drain; final positive combo increment or
failure reset and one practice accuracy object. Slider mechanisms preserved.

ENGINEERING host: one qualifying WPF frame sample, two global interval histories,
same projection for actual mouse and displayed center, no radial gate. Original
native prepared center/timing-tail/candidate/helper/repeated-clock behavior is
not reproduced. Pause retains state; restart/replacement create fresh state.
Finite v14 spinner duration1–60000/end<=3600000; disjoint stored object intervals,
including endpoints; sound Int32 and optional raw sample metadata, no sound-mask
or playback invention. Separate spinner lifecycle/body/rotor/progress/held/final
feedback, no circle-head admission while active. Old fixtures/buttons/audio/clock
remain. No mods/Replay/Relax/Auto/full score/HP/native effects or BASS redesign.

Changed under reconstructed/: parser/model/session, renderer/UI/project/README,
three independently authored silent fixtures, bounded R07DevelopmentChecks,
R07 report and this append. Only authorized CI infrastructure edit is a step/path
trigger in existing Windows gate; research/QA01–04/E20/historical records intact.
Initial source/diff/whitespace review; no local dotnet executable/SDK install.
Windows build,189 QA cases, R06 checks and8 spinner development groups PENDING;
actual source/run/job/outcomes will be appended after execution. These finite
component checks are not original runtime or independent QA05.

No reconstruction/original launch, OS mouse/keyboard input, playback/recording,
OBS/debugger/E20 experiments, VM or supervised routine testing. All interaction
stays human-controlled; R07 usability/fidelity remains UNVERIFIED. Current R07
scope authorizes main implementation over historical suspension/branch rules;
no research journal/numbered RE mark change. Stop after R07; QA05 belongs to Work 1.

R07 hosted verification outcome (append after execution): source commit
`a0a058c12a365f7f6d8b922fa3240dd0bfded006`, run41571969274,
job125706717861, https://github.com/thgillwtnorizoh/ohno/actions/runs/41571969274 .
2026-11-03 10:24 Asia/Bangkok (03:24 UTC), Windows2022/preinstalled SDK8.0.427.
WPF Release win-x64 PASS:0 warnings/0 errors. Unchanged QA01:90/0, QA03:56/0,
QA04:43/0 (189 existing cases). Existing R06:4 preparation targets PASS/4 loads
completed. R07:8 bounded development groups PASS, including three fixture imports,
atan2/first-angle/wrap/EMA/held rules, acceleration/clamp/cap/residual U, integer
observation/cache and first raw0/second8192, modern thresholds/strict completion/
no drain/session isolation and parser rejection. All steps/job succeeded; runner
checkout Node20 advisory is separate from compiler diagnostics. Historical R06
gate labels executed this R07 SHA. No new human/original runtime observations.
Final documentation-only publication preserves exact tested app/fixtures/checks/
workflow; research/QA01–04/E20, old fixtures/reports, audio/clock, slider mechanisms
and historical journal prefix remain unchanged. No QA05 or further implementation.
R07 stops here.

R07 source-review follow-up before final handoff: the first gate passed, then a
host-sampling usability concern was identified. Applying E24's nominal divisor
to every high-Hz rendering sample could weaken progress. Added an explicit
ENGINEERING qualification gate using E24's recovered16.666666666666668 ms period;
integration remains per running frame, with separate global histories. This does
not reproduce full original timing-tail/marker/provider scheduling. The prior
"final documentation-only" intention above is superseded by this small R07 host
refinement; published history is retained. Added explicit3000/3008/3017 component
samples to the existing bounded development group and updated current instructions/
report. Reverification of the changed source will be recorded below. No OS input,
gameplay, original investigation, additional milestone or QA05 is initiated.

R07 final refined-source verification: `52cf7f0a928228e2877b08839b7b15f866d9cc04`,
run41572775844/job125709298355,
https://github.com/thgillwtnorizoh/ohno/actions/runs/41572775844 .
2026-11-03 10:32 Asia/Bangkok (03:32 UTC), Windows2022/preinstalled SDK8.0.427.
Actual WPF win-x64 build0 warnings/0 errors; QA01 90/0,QA03 56/0,QA04 43/0;
R06 four preparation targets/four fixture loads; R07 eight development groups,
including nominal sample qualification and all prior finite controls, PASS.
All steps/job succeeded; only checkout Node20 advisory, separate from compiler
diagnostics. No human gameplay or frame-rate equivalence was observed. Final
documentation-only commit now preserves this exact tested app/fixtures/checks/
workflow, with protected historical files and journal prefix preserved. Source
review refinement/reverification remains R07 only; stop, no QA05 or expedition.

## R08 — Legacy Score, Accuracy & Results Accounting

2026-11-03 (Asia/Ho_Chi_Minh), Work2. Clean latest origin/main at
`37e35c0ca214aa0fc7f5de1da80f50ec26b62b23`. Read AGENTS/standing contract,
journal/R07, E33 in full, shared component/completion policies and QA01–QA05.
No original binary inspection, new expedition, game/window/input, OBS/debugger,
E20, supervised play or original runtime work. Prior owner feedback remains
positive scoped evidence, not advance validation of R08 numeric scores.

RECONSTRUCTED E33 ledger: separate public ScoreLedger/ScoreConfiguration and
provenance-bearing ScoreSource/ScoreEvent classes in linked Gameplay/CircleSession
source (unchanged QA project links). Int32 score/current/max, UInt16 snapshot and
whole300/100/50/miss counts, Single derived accuracy and explicit K. Record flag
gates counters/score independently of combo. Old-combo bonus retains Int32 b/25
before Double multiplications/truncation. Mutation order counters/base/delta/combo/
score; unchecked integer fields. Components use actual raw amounts, producer roles
retained separately; raw0 is no event. Terminal failure/spinner components bypass
combo; positive slider aggregate bypasses duplicate credit, parent MISS resets.
The latter corrects R07's broad aggregate bypass per authoritative E33; a parsed
short-slider control checks reset before separate late head timeout. Historical
QA tests/results are not edited. Any actual test conflict must be disclosed.

Session ledger is authoritative for combo/max/accuracy; old HitValueTotal becomes
a weighted-hit getter, not numeric score. JudgedCount remains lifecycle completion.
Every actual circle/head/checkpoint/parent/spinner observation/final reaches its
typed accounting sink once; no backlog fabrication or stale spinner raw replay.
UI HUD/results show ledger values, whole counts and completion/policy/K metadata.
Restart/replacement use fresh ledgers with supplied config; audio/clock/geometry/
input/movement/judgement unchanged apart from required accounting integration.

ENGINEERING explicit UI K1 preset, finite K0–10/provenance label; automatic map
density operands F0ff2/F0fe5 unresolved. No guessed map preparation or ScoreV2/high
flags. Unsupported event/policy/provenance marks unavailable and freezes supported
prefix, hidden as a final metric; outside-domain bonus conversion is not assigned
target-CLR behavior. One session-owned record, no native callbacks/global-handler
swaps, full score/HP/mod/Rank/PP/network/replay persistence or graphics/audio overhaul.

Changes under reconstructed/: ledger/session, renderer/Program/README, bounded
R08DevelopmentChecks, report and this append. All prior fixtures/buttons retained;
no new fixture needed. Minimal authorized CI change: one step/path trigger in the
existing Windows gate. Work1 research/QA01–QA05/E20 and historical reports/journal
preserved. Source/diff/whitespace checks only initially; no local dotnet executable.
Windows build/255 existing QA cases, R06/R07 checks and13 bounded R08 groups PENDING
until actual hosted execution; source/run/job/outcomes will be appended. Component
controls are E33 static predictions, not original game or human verification.
Current R08 scope continues main implementation over historical suspension/branch
rules. Stop after R08, no QA06 or original-software expedition.

R08 initial hosted gate: source `b17c32cdc0eb1700c4686023e29ccbee3908f533`,
run41592125988/job125771010001. Actual Windows2022/SDK8.0.427 WPF build0 warnings/
0 errors, QA01 90/0,QA03 56/0,QA04 43/0,QA05 66/0, R06 four targets/four loads,
R07 eight groups, R08 thirteen groups PASS. Source review then corrected a narrow
admission detail before final handoff: E33-A's late record gate is captured flag
OR signed-positive(raw &8388608), separate from hit counts. Supported negative
enums contain the bit and admit zero writes even with captured flagfalse; positive
flagfalse cases stay suppressed. Added that finite assertion to an existing group.
Main fixture numbers are unchanged; tests/research remain untouched. This current
R08 refinement will be reverified; no new expedition/QA06 or original observation.

R08 final hosted verification: source `d09906f5689b78cfd2ebc5ab9aea8ef9dc371fd3`,
run41592942636/job125773612554, 2026-11-03 06:55–06:56 UTC /13:55–13:56 Vietnam.
Windows2022, preinstalled .NET8.0.427: WPF win-x64 build0 warnings/0 errors;
QA01 90/90,QA03 56/56,QA04 43/43,QA05 66/66 (255 total,0 failed);
R06 four finite preparation targets/four fixture loads, R07 eight development
groups, R08 thirteen groups including corrected late-admission assertion PASS.
All job steps succeeded. Checkout Node20 advisory is separate from compilation.
No historical QA assertion conflicts with E33 parent-MISS combo reset. Actual
command/output and tested SHA are retained in R08-legacy-accounting.md. Final
handoff updates only report/journal over this tested source; app/checks/workflow
remain identical. Journal historical byte prefix and research/QA/E20/old fixtures/
reports were checked preserved. No game/window/input or human score comparison.
Original automatic K, record/decorated policy fidelity and cross-map numeric
identity remain unresolved. R08 ends here; no QA06 or original expedition.

## R08-C01 — Spinner presentation and preceding-slider continuity

2026-11-03 Asia/Bangkok, Work2. Verified main4c61be63265d54b19b257fb3f1c632760c9e7773
included E034 but no C01. Owner explicitly authorized C01 before C02, independently
published/gated. Read contract/AGENTS/journal/R08/E033/E034 and relevant source.
Corrected renderer's borrowed circle-approach spinner preview with read-only stored-
start/pending-result visibility. ENGINEERING, no native opacity schedule claimed.
Source inspection found no pre-start spinner input gate in slider Track/frame path;
no speculative input/scoring patch. Owner interruption remains scope-limited report,
actual cause/runtime focus/input unverified. Four bounded checks: visibility
boundaries, held preceding-slider through old preview/tail (300/combo2/K1score372),
head inside old preview, read-only presentation. No game/window/OS events or RE.
Changes under reconstructed/ helper/renderer/checks/README/report/journal plus one
minimal existing CI step/path. Historical research/QA/fixtures preserved. Gate
PENDING until actual Windows build/255 QA/R06/R07/R08/C01 outcomes append. Current
user authorization main implementation/implementation-only records supersedes
historical branch/investigation defaults. C02 only after C01 publication and gate.

R08-C01 actual hosted outcome: sourceb9f89745f4310a115dfeab70f3b093b8eecad99f,
run41635206573/job125914546857, Windows2022/preinstalled SDK8.0.427. WPF win-x64
0 warnings/0 errors; QA01 90/90,QA03 56/56,QA04 43/43,QA05 66/66 (255/255);
R06 4 targets/4 loads,R07 8 groups,R08 13 groups,C01 4 groups PASS. All job steps
succeeded; checkout Node20 advisory only. Report links actual logs. No human game
or original-runtime observation. C01 app/checks/workflow unchanged in final docs
publication. C02 now authorized to proceed from this published and verified base.

## R08-C02 — Automatic legacy score multiplier integration

2026-11-03 Asia/Bangkok, Work2. Started published/verified C01 main
761431c31acbf11b26ea324c011fa65a177c7136 after its passing Windows gate, with E034
4c61be63265d54b19b257fb3f1c632760c9e7773. Owner dependency resolution authorizes
sequential C01 then C02 with independent reports/commits. Read AGENTS/full contract,
journal/R08/C01 and E033/E034 in full. No original binary/new expedition/E20,
game/window/input/debugger/recording or QA06.

RECONSTRUCTED E034 fresh metadata N per raw row (independent UInt16 type counts),
first-before-type and last-recognized row starts, separate last end, unchecked raw
numeric-event2 break sum, two signed subtractions /1000; fresh Single5 HP/OD/CS,
modern Single clamp or older Byte parse. Single N/T*8 clamp, ordered HP+OD+CS+density,
/38*5 then Double Math.Round/to-even and finite Int32 D; no-modDouble K=D*1.
Positive N/T0 infinity clamps16; 0/0 and exceptional conversion reject explicitly.
UI FromBeatmap prepares config from same captured file lines before replacement;
no fallback K. Historical singlearg session compatibility stays ENGINEERING K1;
explicit config overrides retained. HUD/results show AUTO E034 N/T/B provenance.
E33 ledger/event/judgement/combo/accuracy and C01 drawing unchanged. All fixture
bytes preserved, particularly Joined HP0/CS4/OD4. Conditional MixedK2/2392 vsK1/2236;
JoinedK3/534 vsK1/438. Owner582 remains unresolved, not fitted withK4 or edited HP.

Changed reconstructed/ Beatmap metadata/helper, ScoreConfiguration factory, app
load/README, bounded C02 checks/report and this append; minimal CI step/path only.
Fourteen dev groups use real components, including hashes, spans/breaks/rounding/
zero denominators/overflow/difficulty and conditional event streams. Initial
Windows gate PENDING; exact tested SHA/run/output will append. No local SDK/build.
Fresh-parser/error boundaries are engineering rejection, not emulation of original
partial mutations/cache/reused identity/runtime precision. Historical research/QA/
reports/fixtures/C01 source and journal prefix preserved. Stop after C02; no R09,
lazer or framework migration. Current main implementation scope supersedes old
branch/RE defaults; no research journal or original test required/attempted.

C02 initial gate sourceb615a41717903ec6d77921a42cbc46ef3c687b9c,
run41636269428/job125918152607: WPF0 warnings/errors,255 QA,R06/R07/R08/C01 passed.
C02 step FAILED before first group at fixture hash: Windows CRLF checkout differs
from E034 LF repository blob. Changed only new check's hash canonicalization
CRLF->LF; all fixture bytes/app arithmetic preserved. Full source re-run required;
no passed new multiplier checks claimed yet. This is verification portability,
not an original-game discrepancy or evidence for fitted K.

C02 final actual verification: sourceab1a6a6f7ad34a8f0d45a66f75be42b1083de461,
run41636528239/job125919009472, 2026-11-03 12:32–12:33 UTC /19:32–19:33 Bangkok.
Windows2022/preinstalled .NET8.0.427: WPF win-x64 0 warnings/0 errors; QA01 90/90,
QA03 56/56,QA04 43/43,QA05 66/66 (255 total,0 failed); R06 4 targets/4 loads,
R07 8 groups,R08 13 groups,C01 4 groups,C02 14 groups PASS. Actual executed checks
confirm canonical fixture hashes, automatic MixedK2/2392 and JoinedK3/534 versus
explicit K1/2236 and438; original582 unresolved. All steps/job passed; checkout
Node20 advisory separate from compilation. Exact command/output/source/job in
R08-C02 report. Final docs-only publication leaves tested app/checks/workflow
unchanged. E33 ledger/all session mechanics, C01 renderer/report/checks, prior
fixtures/research/QA/E20/historical records preserved and old journal byte prefix
verified. No human/game/window/input/original observations claimed. Stop after
C02; no QA06/R09/lazer/framework migration or new original expedition.

## R09 — First Original-Derived Health System

2026-11-03 Asia/Bangkok, Work2, verified latest main64bec143982ef7ceadb211db9208e1324074e2fa.
Read AGENTS/full contract (attachment/repo identical), journal/R07/R08/C01/C02,
QA06, E035 full and relevant E019/E024–029/E033–034 boundaries. No original binary/
research tool/game/window/OS input/debugger/recording/E20 or deferred score work.

Separate WPF-free HealthState/HealthChart/HealthCalibration/HealthEligibility/
OrdinaryHealth components in existing linked source keep independent QA unchanged.
E035 H/V/A, selective200/200/0 reset, exact helper clamp/accumulator differences,
plain HP table, private trial1028 and chart-calibrated r/B/G with snapshot retention.
Prepared order/count/type/break provenance captured independently of score metadata.
E21 subsetCount uses N/R/index rule, not UI repeat labels. Original M2902 NewCombo
is unresolved: following circle/slider without explicitbit4 rejects automatic HP;
first-circle fixture remains playable/scored but visibly HP-unavailable. No guessed
rate/B/G. New independent Health R09 map has sufficient explicit groups and break.

Original-derived post-combo/pre-record HP hook and post-record failure check preserve
E33 arithmetic/order and once-only raw event boundaries. Passive drain after object
judgements consumes prior eligibility/signed dt/selected-spinner quarter, then V
smoothing and late eligibility. E027 supplies active-break guard for queue dequeue;
E035 midpoint/end queue and inclusive marker/time endpoints preserved in ordered
nonoverlap domain. Original no fixed calibration cap; engineering guards10000
trials/2M work,10k objects/4096 breaks reject, nofallback. First independent HP0
circle2000/approach1200 control predicts61 reductions/62 trials,B/G1.

ENGINEERING WPF host: Stopwatch gameplay, rendering-delta/nominal60Hz hostscale,
normalclock1/running F1374 proxy, interval-selected spinner instead of oldcandidate
selection, full prepared starts for marker clearing, explicit F0fc4 filloperand0.
Failure suppression/recovery inputs isolated; no-mod UI count0, first latch/time
visible with practice continuation, no native departure or fail/retry UI invented.
Recovery+160/fill/release boundary supported by direct input; native playback/clock
holds not implemented. Vbar/Hmarker +H/V/A/r/B/G/trials/status inHUD. Old audio/input/
geometry/scoring and fixtures remain. New Health R09 fixture/button/project copy.

Changes reconstructed/ only plus minimal existing CI step/path: parser/session/
health classes, primitive HUD/host timing, README/fixture/report,14 bounded devgroups
and this append. Work1 research/QA01–QA06/E20, historical reports/journal preserved.
No local dotnet/installation/build; source/diff checks only initially. Windows
build/354 QA/R06/R07/R08/C01/C02/R09 gate PENDING; actual source/run/output appends.
Current user scope authorizes main implementation/implementation-only records over
historical suspension/branch/RE defaults; no research journal or expedition edit.
Original runtime/native precision/partial helpers/record swaps/full group decorations/
physical play andgeneral map equivalence unverified. Stop after R09, no R10/QA07.

R09 actual hosted outcome: sourceebb00b6a91df976271923f6bda3c495336af5678,
run41668909283/job126031648587, 2026-11-03 15:55–15:56 UTC /22:55–22:56 Bangkok.
Windows2022/preinstalled .NET8.0.427: WPF Release win-x64 0 warnings/0 errors;
QA01 90/90,QA03 56/56,QA04 43/43,QA05 66/66,QA06 99/99 (354/354);
R06 4 preparation targets/4 fixture loads; R07 8 groups,R08 13,C01 4,C02 14,
R09 14 groups PASS. Exact output/command/source/job and static-site provenance
recorded in R09 report. All steps/job succeeded; checkout Node20 advisory separate
from compilation. New controls passed independent62-trial/61-reduction HP0 case,
all retry branches/retained coefficients, signed/quarter/previous-eligible drain,
break/queue/pause/smoothing, result HP/failure/recovery and ledger invariance.
No failed gate or modified independent QA. Final documentation-only commit leaves
tested application/fixture/checks/workflow identical. Historical research/QA/E20,
old fixtures/reports/development checks and prior journal byte prefix preserved.
Unsupported automatic grouping (opaque M2902), overlapping/order/identity/break
metadata and engineering calibration guard exhaustion remain explicitly unavailable,
not guessed rates. Full original host/candidate/F1374/F0fc4/native lifecycle/physical
play remain unverified; no game/window/OS input occurred. R09 ends; no R10/QA07.

## R09-C01 — Recovery Fill Threshold Correction (QA07-D01)

2026-11-03 Asia/Bangkok, Work2. Latest verified main34249b9895f0d0ebabbeb6436d3ab37960c88857.
Read AGENTS/standing contract (attachment/repo hash agree), journal/R09/E035 and
QA07 README/results/guard/failure logs. QA07 red source8185478d4a1e064a3e74dbee82c8dbc8907b9187,
run41671715769/job126041531509 records157/159 with F13 H160/H200 failing. Failure
log hashes checked and preserved; no Work1 files/assertions/results overwritten.

Minimal confirmed correction: BeginRecovery's Add160 AND ConfigureFill are guarded
by H<160, following original M0acc0014→006a; guarded0025/0031/0045, later recovery006e
and count00a1. Recovery activation/count consumption remain outside; original missing-
count precondition/native playback boundary unchanged. No scoring, multiplier,
calibration, drain, smoothing/routing or UI changes. Original R09 report preserved.
One added Work2 R09 check group covers0/100/159.999/160/200, both fill flags/latch
states (20 combinations), retained HVA/fillFrom/coefficient/failure state, count,
no seeded-ledger mutation and repeated missing-count rejection. Old14 groups intact;
existing CI already runs new15-group program plus QA01–QA07, so no workflow change.
Only production guard, Work2 checks/new report and this append changed. Source/diff/
whitespace checks only initially, no local SDK/build; Windows full gate PENDING,
actual source/run/outcomes append after execution. No original game/binary/tool,
window/OS input/debugger/recording/E20 or new expedition. Native playback/lifecycle
and general original equivalence unverified. Targeted R09 authorization/main records
supersede historical branch/RE defaults. Stop after R09-C01; no R10 or scoring work.

R09-C01 actual hosted verification: sourceb14d98ef7a6787621ab65369d3c0daa0e3130504,
run41673876722/job126049132491, 2026-11-03 16:24–16:25 UTC /23:24–23:25 Bangkok.
Windows2022/preinstalled .NET8.0.427: WPF Release win-x64 0 warnings/0 errors;
QA01 90/90,QA03 56/56,QA04 43/43,QA05 66/66,QA06 99/99,QA07 159/159
(513 independent cases/0 failed). Both F13 H160/H200 now PASS. R06 4 preparation
checks/4 fixture loads; R07 8,R08 13,C01 4,C02 14 development groups; R09 15
(previous14 plus focused C01 group/20 combinations) PASS. All steps/job succeeded;
checkout Node20 advisory separate from compiler diagnostics. QA07-D01 disposition:
corrected and VERIFIED COMPONENT for scoped finite recovery entries. Exact output,
commands and tested SHA in new R09-C01 report. Final docs-only publication retains
tested production/checks/workflow; original R09 report, all Work1 research/QA/logs,
fixtures/other code, prior assertions and journal byte prefix preserved. Historical
red gate/results are not rewritten. No original game/window/input/native playback
or human equivalence observed. Stop after R09-C01, no R10 or further feature work.

## R10 — Original NewCombo Preparation & Expanded Health Calibration

Work2, 2026-11-03 UTC /2026-11-04 Asia/Bangkok. Base verified origin/main
2e060a69f3aca94afdd6747629bd72ede84a7297 (E036). Read full standing contract,
canonical AGENTS, journal, E035/E036, relevant E004/E012/E021, R09/C01 and QA07;
inspected parser/source/chart/calibration/session/renderer/CI. Implementation-only
assignment; no original archive/binary/tools/game/window/OS input/debugger/OBS/E20.
E036 CONFIRMED STATIC instructions support reconstructed fresh NewCombo: M2902
null/spinner-base/T081f predicate before circle/slider file-bit OR; spinner parser
file-bit overwrite without helper; last-registration update; strict pending-break
postpass in actual prepared order; final next-property into private1024/1028 trials.
Pure preparation retains raw/file/helper/factory/constructor/parser/final/type /
reference identity provenance. Exact fresh parsed-map owner is an ENGINEERING
safeguard; rejects cloned/retained/editor/alternate identity, ambiguous order or
unknown time-domain state rather than assuming null after clear. Existing HP
validation/calibration/rates/coefficient arithmetic, live H/A/V, drain/smoothing,
C01 H<160 recovery guard, judgement/input/audio, E033/E034 formulas unchanged.

Owner-authorized decision after concrete contract conflict: QA07 I04/I08 and a
historical R09 development assertion expect unmarked maps' HP to remain unavailable
because M2902 was unresolved. User selected Versioned compatibility API. Old public
constructors/2arg FromMap retain HistoricalR09. App Load/Restart and new checks use
explicit CreateFreshOrdinary/E036FreshOrdinary; no test/assembly detection or QA
modification. Existing513 cases cover historical/component regressions, not an
independent validation of the new path. New R10 checks separately cover E036 finite
N01–N22 derivations, flags/identity/order/type/reuse/break boundaries, private group
rewards221/235, changed r/B/G balance retries, original eight-circle eligibility
and an independent three-circle control, restart/replacement and per-update score /
accuracy/combo/judgement invariance. No fixture edits; supplied original silent file
has eight circles, no distinct tracked three-circle file found. All historical
research/QA/logs/reports/checks and journal byte prefix preserved.

Changes only reconstructed parser/session/title/Load+Restart/current README,
new R10DevelopmentChecks/report, append-only journal and minimal CI path/step.
New report R10-original-newcombo-health-preparation.md contains exact provenance,
API choice, finite controls, engineering bounds and remaining native limitations.
Local static diff/whitespace/preservation checks; no local SDK. Windows full gate
PENDING at source publication; actual tested SHA/run/outputs append after execution.
No original runtime or physical gameplay claim. Main expected-head publication,
no permanent branch/force/history rewrite. Stop after R10; no R11/QA08/E037,
score-discrepancy investigation, framework migration or lazer extensions.

R10 actual hosted verification: tested source33ae9c18ed1c5837d77321f022dcdf13144eb883,
run41681640363/job126076528314,2026-11-03 17:10:36–17:11:42 UTC /2026-11-04
00:10:36–00:11:42 Bangkok. Windows2022/preinstalled .NET8.0.427: WPF Release
win-x64 0 warnings/0 errors; QA01 90,QA03 56,QA04 43,QA05 66,QA06 99,QA07 159
passed (513/513 independent cases,0 failed), all job steps succeeded. QA07 F13
H160/H200 remain PASS. R06 targets4/fixture loads4; R07 8,R08 13,C01 4,C02 14,
R09 15 development groups passed. New R10 23 groups passed separately. The older
QA entry points remain historical R09 by the explicit owner-approved API decision;
those513 do not independently establish new fresh E036 integration. R10 checks
execute that path directly, including private group balance221/235, break while
versus one-exclusion separation, source order/identity guards and fixture health.
Executed five-circle finite control: unmarked r0.0035204659802861625,
B1.08243216,G1.04060401,66 trials; all separate groups r0.004144911095285019,
B1,G1,62 trials. Matched E034 K/zero trial score and per-update actual-component
score/accuracy/combo/judgement invariance passed. Three-unmarked-circle temporary
control and unchanged bundled eight-circle fixture prepare health; no fixture was
modified/force-enabled. Restart/replacement/clone/retained-domain checks passed.
Exact output/commands/link in R10 report. Checkout Node20 advisory separate from
compiler diagnostics. No local SDK/GUI/original runtime/physical gameplay, no E20
execution or QA08. Final docs-only record update preserves tested production,
checks/workflow, all Work1 research/QA/logs, prior reports/checks/fixture bytes and
journal prefix. Health/score/calibration/drain/smoothing/recovery arithmetic exact
source-preservation checks passed. Native timing/manager ties/reuse/alternate
factories and general equivalence remain unresolved; unsupported HP stays explicit.
Stop after R10, no R11/E037/framework migration or deferred score investigation.

## R11 — First Original-Derived Audible Circle HitSounds

Work2,2026-11-04 Asia/Bangkok. Started verified latest origin/main
8ae7b44aa12de46a15f2043847b70add3c5454a2. Read canonical contract/AGENTS/journal,
full E037/E038 reports and parsed JSON evidence, E004/E005 correction, E010/E012 /
E026 boundaries, R10/QA08 and current parser/session/audio/UI/workflow. No RTK.md
is present/current AGENTS directs re/Instruction. No original binary/archive /
asset extraction or RE tools, original or reconstructed window/input, audio
listening, debugger/OBS/E020. This is authorized R11 implementation only; no
shared research or historical report/QA assertion/fixture changed.

CONFIRMED STATIC E037/E038 authority supplies reconstructed metadata, ordered
stored-start+5 timing selection, override inheritance, conditional Normal layering,
custom handle versus1/4/2/8 layer requests and exact volume/pan/rate operands;
decoded resource keys, distinct standard/custom extension order, provider gates,
map/skin/default cache/sentinel identities, dynamic preloading and36 prepared cells.
Added pure Gameplay/CircleHitSounds, additive Beatmap sidecars, ordinary-positive
pre-accounting hook, local-root/resource library and WindowsCircleEffects adapter.
Shared object custom/index preloads do not introduce slider/spinner sound hooks.
Optional playback failures are ENGINEERING-contained; judgement/scoring/HP continue.
Exact E033/E034, HP/calibration/drain/smoothing/recovery/C01 arithmetic and explicit
HistoricalR09/E036FreshOrdinary API choice retained. No trial calibration sounds.

ENGINEERING settings in output hitsounds.json: optional local skin/default pack,
layering, effect/global volume, enable/status controls; no guessed original assets.
Independent PowerShell tone generator supplies optional original practice PCM WAVs
without playback. No sample bytes committed/extracted. Windows adapter preopens
2 voices/resource asynchronously, queues no-I/O judgement requests, discards paused/
stale queued work and stops/releases on restart/replacement/exit. Song MediaPlayer
and Stopwatch unchanged. Managed IDs/codec failures/threads are not BASS semantics.
Original pause/worker timing/native device precision and acoustic equivalence remain
UNVERIFIED. Local path/size/index/resource guards are engineering limits; short native
SampleCreate and short timing global defaults remain explicit unsupported boundaries.

New R11DevelopmentChecks use actual linked production with injected providers and
recording sinks: H/K quality/order/once-only/metadata/timing/layers/volume/custom /
resource-cache/fallback/invalid-sentinel/local path/lifecycle/failure/HP-score controls.
Current README/new R11 report and minimal established Windows workflow path/step
updated; retain628 independent QA cases and all prior development groups. Source /
diff/preservation review only initially; no local SDK. Windows full gate PENDING
at source publication, actual source/run/results append after execution. No human
playback claim or new independent QA campaign. Expected-head non-force main
publication only, no permanent branch/rewrite. Stop after R11; no QA09/R12/E039,
framework/lazer work or deferred scoring-discrepancy investigation.

R11 first source5c07743ffdd37ed65120ec77f79fa39931c2d06f passed Windows
run41792253987/job126448087167: WPF zero warnings/errors; all628 independent QA,
all historical groups, new R11 32 groups and twelve independent generated WAV
headers passed. Subsequent bounded static review noticed E037's named SampleSet
All−1 needed `all` rather than the generic numeric enum string in dynamic keys.
Corrected that pure naming arm and added literal All/None/unknown-numeric checks
inside the existing naming group; no broader admission/scoring behavior changed.
A second full gate is required for this final source. First passing run is retained
as evidence, not replaced. No listening/native/gameplay-window execution.

R11 final Windows source c21aed0d3a5c1bc749d5472ccd1006f99ef43832 passed
run41792584386/job126449168289,2026-11-04 07:29:09–07:30:13 UTC /14:29:09–
14:30:13 Asia/Bangkok,Windows2022/preinstalled .NET8.0.427. WPF Release win-x64
0 warnings/0 errors; QA01 90,QA03 56,QA04 43,QA05 66,QA06 99,QA07 159,
QA08 115 PASS (628/628 independent cases). R06 targets4/fixture loads4;
R07 8,R08 13,C01 4,C02 14,R09 15,R10 23 groups PASS. Separate R11 32 development
groups PASS, including named All/None/numeric resource strings; independent tone
generator produced12 PCM WAV files with expected10628-byte/RIFF headers, never
played. All steps/job succeeded; recovery F13 H160/H200 PASS. Checkout Node20
advisory separate from compiler diagnostics. First successful source/run retained
in the report; final run covers the small subsequent enum-naming correction.
Actual output, commands, tested SHA and CI links recorded in R11 report.

Final docs-only publication updates new R11 report and appends this journal,
retaining tested application/checks/generator/workflow bytes. Historical journals,
reports, fixtures, original research/QA01–QA08/logs and all prior assertions
preserved. Exact ledger, health-state/calibration/drain/smoothing/recovery blocks
verified unchanged. Fresh/Historical health APIs unchanged. No original archive/
binary/audio extraction, game/window/OS input/OBS/debugger/E020, listening/native
playback, QA09 or R12/E039. R11 delivers source-confirmed/compiled Windows local
effects and executed finite selection/resource/lifecycle controls; actual media
onset/overlap/codec/device behavior and human-audible/native BASS equivalence remain
UNVERIFIED. Optional independent pack/loose-map/skin setup and engineering inputs/
limits/unsupported short timing/native short-create states documented in README.
Expected-head non-force main publication, no permanent branch or history rewrite.
Stop after R11; no new milestone/research/QA or deferred scoring investigation.

## R12 — Original-Derived Discrete Slider HitSounds

Work2,2026-11-04 Asia/Bangkok. Base verified latest origin/main
3c263ec1258e15b78ed993a47508cbfebaf4b300. Read canonical AGENTS/standing contract,
both journals, E039 report/full parsed JSON (1258274 bytes/SHA2562a45c9a2d6565246cfe9a627f6ef96929f7c36cd81fd4c9780e8d5375ad58a6e),
E015–E018/E021–E023 and E037/E038 published research/evidence, R11/QA09, actual
parser/session/resource/Windows/renderer/generator/CI. No physical RTK.md present.
No original archive/binary/sample extraction, original checker/RE tool, original
or reconstructed game/window/OS input, debugger/OBS/listening/E020 execution.

CONFIRMED STATIC E039 consumes reconstructed raw parent/edge metadata; independent
head base/addition copy (own index/volume/custom defaults); positive head sound
before64; separate latest-prefix audio index; dedicated tick mode1/deadline+2;
edge subset/end+5/version/null/list selection; positive aggregate final edge before
grading/accounting. Fresh/reset audio index0 and reached pre-start−1 retained.
No checkpoint result/label is used as audio progress. E033/E034/E035/E036 arithmetic,
C01 recovery and HistoricalR09/E036FreshOrdinary split unchanged. Calibration silent.
New event provenance separates circle/head/tick/repeat/aggregate requests. R11
standard provider/cache/volume/rate/generation components reused; tick table opt-in
keeps legacy circle-only Prepare behavior. Parent custom loader flag retained as
metadata, no direct parent custom request or continuous voice. Ambiguous custom
cache collisions and unsupported fresh/registered identity explicitly excluded.

ENGINEERING parent pan uses renderer's last-published actual drawn ball X (initial
head X), not new target/pointer/endpoint; native same-update identity/order unverified.
Running host numeric state1 explicit. Omitted continuing M146b/M146c helpers and
native shared-channel effects/errors are a documented boundary, not fake loops.
ExistingWPF async voices/pause/generation/error containment/song clock preserved.
Malformed sound metadata unavailable while geometry/gameplay remain playable;
1–16 traversal/local-root/resource/count safety limits retained. Short original
sample-create/timing globals, archive/editor/reuse/native/audio precision unsupported.

New Discrete R12 independently authored mixed fixture/button and companion tick-only
generator (3 resources) preserve old fixtures and unchanged12-circle generator.
New separate R12DevelopmentChecks exercise actual production with injected providers/
recording sinks, conditional S controls and score/HP/lifecycle preservation. Current
README/new R12 report and minimal existing Windows CI path/step updated; preserve
all862 independent QA and all R06–R11 development groups. Source/diff/preservation
checks only initially, no local SDK/build claim. Windows gate PENDING at source
publication; actual SHA/run/counts append after execution. No human-audible result,
full48 original cases or continuous S44–S47 execution claimed. Main expected-head
non-force publication, no permanent branch or history rewrite. Stop after R12;
no QA10/R13/E040/spinner audio/framework/lazer or deferred scoring investigation.

R12 first hosted run41877100114/job126739350436, source27c2fde21676869b851ac4755a2d6392251dc9fc:
actual retrieved logs show WPF zero warnings/errors, all862 independent QA and all
historical development groups passed;3 tick files generated/header-checked. R12
harness compilation failed with CS8602 at Program.cs207 (nullable CircleSounds
access in test setup); no R12 assertions executed. Corrected only that test's
known parser metadata access with null-forgiving annotation. Production unchanged.
Preserved compiler/suite output in R12DevelopmentChecks/verification/initial-build-failure.txt
and linked original CI. Status endpoints had reported queued while retrieved logs
show execution17:23–17:24 UTC; no actual runner-outage cause is claimed. A new full
gate is required; no test assertion was weakened or finite failure hidden.

R12 final hosted verification: sourcead8cf337ee332259955cf99c820cf4f51b0553d8,
run41879777131/job126748948361. Actual runner logs2026-11-04 17:43–17:44 UTC /
2026-11-05 00:43–00:44 Asia/Bangkok; completed/success metadata confirmed17:48:05 UTC.
Windows2022/preinstalled .NET8.0.427: WPF Release win-x64 zero warnings/errors;
QA01 90,QA03 56,QA04 43,QA05 66,QA06 99,QA07 159,QA08 115,QA09 234 PASS
(862/862 independent cases). R06 targets4/fixture loads4; R07 8,R08 13,C01 4,
C02 14,R09 15,R10 23,R11 32 development groups PASS. Separate R12 40 groups PASS.
New3 independent tick PCM files generated/header-checked alongside unchanged12
circle names, with no playback; artifact counts not added to independent QA.
C01 F13 H160/H200 remains PASS. Checkout Node20 advisory separate from compiler
warnings. No local SDK/build/install, original/runtime/native/listening/window test.

Final pass follows one test-harness-only CS8602 correction. The first failed
compiler output/source/run is preserved; no R12 assertion had executed then, and
no production formula or expected assertion was weakened. APIs initially showed
queued while logs recorded execution; later completed/success confirmed. No actual
runner-outage cause is claimed. Report contains full summary/commands/CI links.
Final docs-only update preserves tested application/checks/generator/new fixture /
workflow and all research/QA/historical report/fixture/checks/journal prefixes.
Parent pan remains last-WPF-published ball X, host state1/attention a bounded
engineering projection; native same-update identity not established. No mode2
continuous table/shared-channel playback is implemented; parent custom loop flags
are load metadata, not voice looping. Source-confirmed/compiled discrete circle /
head/tick/repeat/positive-aggregate delivery remains human-audible/native-unverified.
Expected-head non-force canonical main publication, no permanent branch/history
rewrite. Stop after R12; no QA10/R13/E040/spinner hitsounds/framework/lazer work or
deferred cross-beatmap scoring investigation.

## R13 — Continuous slider shared channel foundation — 2026-11-05

Work2 authorized R13 only, from main37ceaeaf96a6ee6c13fd3807ec95057cdba677aa.
Read standing contract/AGENTS, both journals, E040 report/evidence and supporting
E010/E015–E018/E021–E023/E037–E039, R11/R12 and QA09/QA10, current parser,
session, selection, local library, Windows media/presentation, generators and CI.
No original binary/sample extraction or execution, game windows, listening or
physical/graphical automation. E020 remains NOT RUN. No authorized exception.

Added original-derived separate global timing cursors and sample selectors,
mode2 continuing preparation/resolution with loop flags, shared channel pair,
injected recording backend and controlled queue preserving deferred current-field
reads, gain/pan, selector Stop and cleanup Pause. R12 live hooks preserve the
discrete progression/accounting boundaries. Added persistent Windows loop players,
optional independent six-file generator, R13 checks, minimal README/CI updates
and R13-original-continuous-slider-audio.md. Report documents engineering
FIFO/cancellation/current-object proxy, last-rendered ball X, format14/single
producer admission and unsupported state, codec and native-runtime boundaries.
No prior reports/research/independent QA/source fixtures or journal prefix changed;
scoring/HP/NewCombo arithmetic preserved. Windows exact-source verification pending;
no local SDK available. Stop after R13; no QA11/R14/E041. Verification result will
be appended, not substituted for historical failure evidence if any.

R13 initial Windows run42009465622, source c4d7e81dbde807dcb60d18b05f861b6f8849b87c:
WPF clean; historical1091 and all prior development checks passed. R13 passed38
groups then its new fixture lacked mandatory ApproachRate and parser admission
failed. Preserve verification/initial-r13-failure.log; add fixture field, do not
weaken parser or historical tests. Add Windows-only empty-resource lifecycle
harness (no decoder/player/window opened). Final exact-source gate pending.

R13 final verification — source221c6e1d55a488489155acdadd6e8ed32243a014,
Windows run42010372847/job127187559676: PASS. WPF Release win-x64 zero warnings/
errors; historical independent1091/1091 named checks (QA10 includes4 source-only);
all retained R06–R12 groups pass; R13 54/54 groups (51 managed component,2 source
inspection,1 six-file structural group), separate Windows empty-resource lifecycle
1/1. No audio resource was opened/played by that lifecycle test. Six generated
loop structures checked, previous15 generated asset hashes retained. Intermediate
runs42009817763/42010084278 passed48/53 groups. Final review corrected no-song
cursor clearing and its new control passed. Initial38-group fixture failure is
preserved, not reclassified as a production defect. Final report/README/evidence
publication changes no tested production/harness/workflow/generator bytes. Owner
audible qualification and original-runtime/native scheduling remain unverified.
No R14/E041/QA11 begun. All historical records/journal prefixes preserved.

## R13-C01 — Shared channel settings gain capture — 2026-11-05

Work2 correction authorized for QA11-D01 only. Baseline main
13d2bb58000f65c5526ce7cd81800c8fdc9b47bd; R13 production221c6e1d55a488489155acdadd6e8ed32243a014.
Read AGENTS/standing contract, both journals, E040 report and full parsed evidence,
QA11 README/discrepancy/TimingChannels, original R13 report and production/harness/
workflow. Original M3b11 body81b56cede5a835a944737fcdcd34c15ff744d821dffd406bbfdaff8f1d5f4dc3
captures gain at0018 and reuses local at001f/002c while rereading channel targets.

Minimal SettingsChanged correction captures ReplacementGain once, retains settings
assignment order, Single arithmetic and fresh current target reads. Added six
separate development controls (ordinary refresh, controlled setting mutation,
fresh second target, literal Single/overflow bits, partial exceptions, other gain/
pan/pause/stop/activity paths). Existing Windows workflow retains every historical
command and adds the bounded controls; reusable ci/verification ref permits exact
Windows verification before advancing canonical main, as this mission requires.
No permanent expedition/milestone branch. Report R13-C01-shared-channel-gain-capture.md.
QA11 sources/oracles/evidence/G08 and failed run42018275697 remain immutable.
No original research/binary/game/media/window/input/listening/E020 execution;
no score/HP/backend/resource/timing/discrete/ownership redesign. Windows verification
pending: expected1344 named independent checks and all development checks. No
local SDK. Stop after R13-C01; do not begin R14/E041/QA12. Final verification will
be appended after the gate; historical R13 report/journal prefixes stay intact.

R13-C01 final exact-source Windows verification: source8336074d08e3a4eadf0af0b9585a7eb5740194b3,
run42020483267/job127221700444 on ci/verification passed before canonical main
advanced to that exact commit. Windows2022, installed SDK8.0.427/runtime8.0.33.
WPF Release win-x64 zero warnings/errors. Historical1091/1091 plus QA11 253/253
(234 components,9 source audits,4 asset controls,6 empty-Windows) =1344/1344
named independent checks. Unchanged G08 passes: both expected0.1; both actual
Single0x3dcccccd (0.10000000149011612, round-trip0.1). R13-C01 development6/6,
all retained R06–R13 groups including R11 32/R12 40/R13 54 plus separate Windows1
pass. Full headless/Windows JSON recovered from ordered compressed CI log payloads
and verified against runner hashes; compressed originals, G08 observations and
summary/provenance saved under R13C01DevelopmentChecks/verification. Original
QA11 failing run42018275697, seven corrected-oracle records, all research/QA/prior
reports/journal prefixes unchanged. Final report/evidence-only publication changes
no tested production/harness/workflow. No media/window/listening/original runtime.
Stopping after R13-C01; R14/E041/QA12 and deferred score work remain unstarted.

## R14 — Spinner audio and separate shared channel — 2026-11-10

Work2 authorized R14 only; canonical base b20c00fe3279032357af682a4946ca57936440d4.
Read AGENTS/standing contract, both journals, E024–E026/E037–E041 and full parsed
E041 JSON, R07/R11–R13/C01, QA05/QA09–QA11 and current parser/session/resource/
Windows/clock/presentation/CI/generators. No RTK.md, original archive inspection,
binary execution/re-extraction, media/game/window/input/OBS/listening or E020 case.

Added spinner sidecar identity/raw payload; pre-classification/cache observation
modulation/bonus hooks, pre-P motion/zero-U cleanup and pre-quality positive final
hook. Shared spinner channel separate from slider rows; admitted global refresh
acquires it independently; settings reuse one R13-C01 gain across three live targets.
Added native-length/frequency recording contract, finite original operand math,
explicit unsupported nonfinite/length cases, Play(false) start and queued live-target
Pause. Named spinnerspin/spinnerbonus stage7 map-index0 resources retain cache/flags;
bonus distinct player registration; final uses stored End without+5 and standard/
custom rules. Windows preopened-player reuse, explicit unsupported audible modulation
(no fake WAV length/pitch), generation/error safety and full-list cleanup bridge.
Added independent two-file generator, bounded R14 and Windows-empty checks, E041
coverage map, README, R14 report and minimal existing CI steps. Source-only Windows
controls and synthetic conditional states are labelled; no all66 original claim.
Score/HP/R07 formulas, NewCombo, slider/circle behavior and historical evidence are
preserved; only optional hooks observe gameplay. Existing1344 QA and every retained
development group remain required. Windows gate pending; no local SDK. Reusable
ci/verification will gate exact source before canonical main publication. Stop after
R14; do not begin QA12/R15/E042, framework migration or deferred score inquiry.

R14 initial Windows run42858280712, source0000b976ca56f02d7926624861e3d838f63a5923:
WPF zero warnings/errors; historical1344 and all prior development suites passed.
New R14 harness failed CS0136 (sample local shadowing); zero R14 assertions and
zero R14 Windows groups executed. Preserve initial Program.cs.txt and log under
R14DevelopmentChecks/verification; rename that local, no production or oracle
change for this failure. Final exact-source gate remains pending.

R14 second run42858711501/source6e08c1df018d87cb25edccb1405aaa1c9b257c10:
WPF/historical1344/prior development green, R14 groups1–24 passed, Windows-empty1
passed. Group25 residual-motion fixture failed: two-second acceleration0.00158
gives20ms allowance0.0316, fully damping priorU~0.018 to0. Fixture expectation
precondition was wrong, not a production discrepancy. Preserve second Program/log;
use six-second acceleration0.00008, two held steps U0.0016→0.0032 then released
U0.0016. Keep expected continuation and add explicit U>0 assertion. No gameplay,
score/HP or original/independent QA change. Final gate pending.

R14 final Windows gate42859198871, tested source75dbf61269937fedd969dfc9e847b731c9f6b446:
PASS before main advanced to that exact commit. WPF Release win-x64 zero warnings/
errors; all1344 historical independent named checks pass, including unchangedQA11
G08 and all retained development checks (R11 32,R12 40,R13 54,Windows1,C01 6).
R14 58/58 groups =55 executed managed-component,2 source-inspected,1 two-file asset
structure group; separate Windows empty-resource capability/lifecycle1/1. Existing
21 generated WAV hashes preserved after adding2 independent spinner WAVs. No audio
decoder/player/window/device was opened by the empty test, no playback/listening.
Failed initial compilation and residual-fixture precondition are preserved with
source snapshots/logs; no production fix was needed for those new harness failures.
Final report/evidence-only publication preserves exact tested code/CI/generator.
Managed frequency requests verified with injected lengths; Windows native length/
audible attribute1 modulation explicitly unsupported, never inferred from WAV bytes.
Owner-only audible qualification remains pending; original BASS scheduling/device/
frequency/overlap equivalence unverified. Stop after R14; no QA12/R15/E042 or
framework/deferred score work. Historical journals/research/QA/reports preserved.

## R14-Q01 — Owner spinner-audio failure/resource routing diagnosis — 2026-11-11

Work2 authorized bounded investigation; verified latest canonical main
 d991dc7c411c28c3bbe88a22a35773c43b054c47. Read standing contract/AGENTS/journals,
R14/R13/C01, E041 report/full parsed evidence and66 predictions, QA12 report/
evidence/current production/Windows/renderer/Program/settings/generator/CI.
OWNER-OBSERVED no continuing spin and ordinary tap where bonus expected, pack files
at C:\MyIndependentPack. Session configuration/map/C/U/provider/decoder/output not
available; no specific audible-path source defect established. Confirmed settings
read beside running executable; absent default pack is null. Cached default−1
survives map reload, full relaunch needed. Exact14 spinner admission independent
of playable maps. N15 first bonusC20; C16 positive300 can play normal completion.

Added bounded in-memory diagnostic snapshot, per-resource logical vs decoder status,
provider/admission/config fields, separate event/request/channel/dispatch/generation
counts/errors and last32 changed loop operations. Owner-initiated JSON save pauses
first; no per-frame disk logs/audio buffers. Updated misleading12-file setup and
logical-only spinner status. No scheduling/cache/resource/backend/frequency/gameplay
rule correction; exact shared identity/Play(false)/fresh callbacks/Stop-Pause
preserved. New23 development groups planned (22 component,1 source inspection),
existing Windows gate minimally extended. R14-Q01 report contains hypotheses,
full audit and executable-adjacent JSON/owner-only listening procedure.
No local SDK/no local .NET run; exact Windows gate pending. Preserve all1630
independent QA and every prior dev suite/G08/research/fixtures/failure evidence.
No original binary/gameplay window/input/media opening/listening/capture/E020,
R15/QA13/E042 or score discrepancy work. Verification details append after execution.
