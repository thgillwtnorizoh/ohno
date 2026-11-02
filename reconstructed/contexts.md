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
