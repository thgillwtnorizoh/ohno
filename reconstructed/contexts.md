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
