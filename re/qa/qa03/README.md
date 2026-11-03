# 1. Result

QA03 — independent R05 slider checkpoint/repeat regression, Work 1,
2026-11-03. Target: `06cfaee3e9ade203852d494a26e003ca2da61174`, verified
as `origin/main` before editing. This is bounded QA, not an expedition.

Execution status: **PENDING HOSTED GATE**. No local .NET SDK is available;
no SDK is installed. Final executed results will be recorded in `results.json`
and `gate-output.txt`, with exact run/job/source identities. A prepared test is
not an executed pass. All implementation and previous QA files are unchanged.

`Headless/QA03.Headless.csproj` links the actual R05 `Gameplay/Beatmap.cs` and
`Gameplay/CircleSession.cs`. It excludes the application entry point, WPF,
rendering, media, physical input providers and clocks. The 56 named tests call
component methods with explicit arguments; assertions and finite expected data
do not reproduce the slider algorithms. No substitute scheduler or game is run.

The existing `.github/workflows/r04-build.yml` is minimally extended and renamed
in its display label to run the new suite after the WPF win-x64 build and
unchanged 90-case QA01 suite. Its historical filename is retained. It selects a
preinstalled stable .NET8 SDK; no setup/install action. Each component step can
still run following an earlier gate failure, once SDK selection succeeds.
One real noninteractive gate is sufficient; results-only changes do not trigger
it. Historical QA02 and R02 source-fingerprint results are untouched.

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa03/Headless/QA03.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

Use the absolute repository-root argument on another noninteractive SDK host.
No game, GUI, OS input, recording, OBS or debugger is launched. Human operation
remains exclusively the owner's. Owner-reported positive R04 feedback remains
valid within its stated scope; it does not validate R05. E20 remains incomplete:
all 17 original-runtime cases **NOT RUN — WINDOWS HOST REQUIRED**.

# 2. What they found

## Independent expectations and source inspection

The assertions below derive from the existing original-CIL research and the
published fixture inputs, rather than accepting R05 output as an oracle.
No new original-binary inspection or original-behavior claim is needed.

| Cases | Contract / evidence |
|---|---|
| A01–A06 | E16/E21 `M144a` producer, `M145c`; alternating directed Movement, carried distance phase, final-only adjustment and producer order |
| B01–B06 | E21 `M1478` inclusive first-match intervals; `M146f:0133,01ee–024c` Single interpolation and strict disk; no turn-triggered tracking reset |
| C01–C08 | E16 `M146f:026c–034f`: inclusive due prefix, one outstanding index, due-prefix positive selector, failure bookkeeping; independent head state |
| D01–D14 | E15 held-history/remembered side and entry/leave; strict radius R or R×2.4000000953674316f; E16 signed entryTime<=deadline |
| E01–E09 | E18/E21/E22 immediate consumer: raw8/64/128 increment combo; -262144 resets; -524288 disables common combo reset. E22 `M0ad2:119a–12c8`, `M40f1:11c7/1287` |
| F01–F09 | E17/E22 inclusive parent end, no pending drain; E23 explicitly selected legacy null-policy ratio; single parent judgement without duplicate component combo |
| R04-01–04 | Unchanged first-slider fixture, sole deadline6564/end6600; positive head/terminal independence, exact-half legacy threshold and timely re-entry |

These original instructions are **CONFIRMED static research**. Their independent
finite projection into this test contract is **RECONSTRUCTED**. CI results
establish only linked C# component behavior. Native precision, actual original
operands and scheduling remain **UNRESOLVED original-runtime fidelity**.

| Fixture | Asserted deadlines | Parent end |
|---|---|---:|
| slider-ticks | 4000,5000,5964 |6000|
| slider-repeat |4000,5000,6000,7000,8000,8964|9000|
| mixed-sliders, third slider |22000,23000,23600,24200,25200,26200,27200,28200,28764|28800|
| first-slider (R04) |6564|6600|

Movement assertions use literal endpoints and intermediate coordinates. Tracking
controls use the implementation's prepared position to place the supplied cursor
at its target; those calls are not independent geometry checks. `One` isolates
one *actual parsed definition* from unrelated earlier-circle admission so that
the slider contract is tested directly. It does not fabricate checkpoint lists.
The translated origin control is parsed from a private temporary file and tests
exact binary32 squared-radius equality without center-addition rounding. D13's
explicit logical-history/head-side combination is a component-level probe, not
a claim that the WPF/physical input wrapper naturally produces that combination.
No assertions write private/internal gameplay state or force raw results.

The suite discriminates particularly easy mistakes: restarting spacing at a
turn, selecting the second inclusive record at equality, draining overdue items
in one update, choosing a result from producer metadata, equating pending with
failed, requiring head success for later tracking, accepting a late entry,
resetting combo for terminal failure, grading by head quality under legacy
policy, and awarding duplicate parent combo credit.

R05 explicitly chooses the legacy policy. This is **not** proof of which
`F13c3.F198e` policy a real original installation selects. Its full-list
attention, single frame snapshot, WPF polling, logical mask projection, practice
accuracy/result counters, finite input bounds and primitive visuals are
documented engineering policies, not new original-game defects. R05 separately
stores the parent result and keeps successful-checkpoint counts distinct from
the head contribution; the original M1465 mutates F0bf1 for that contribution
and returns a manager-local aggregate without directly storing parent.F152a.
The selected output contract does not demand identical internal representation.

No human-play, visual, audible, latency or original-runtime equivalence claim
follows from this gate. Actual candidate/provider gates, native clock sampling,
opaque callback failures, ScoreV2, complete score/HP, curved geometry and arbitrary
maps are outside coverage. No broad random corpus or historical-checker campaign
is added. R01, QA01 and all historical evidence remain preserved.

## Defect disposition and follow-up

Final passing/failing behavior and actionable defects, if any, will be filled
from the actual gate, without changing Work 2 code or weakening expectations.
The smallest runtime dependency remains an owner-operated, identified R05
reversal/checkpoint control; original comparisons require the unchanged E20
packet and authorized instrumentation. No such evidence has been supplied here.

# 3. Pseudocode

Original behavioral model unchanged. No new original-code evidence is obtained;
E15–E18/E21–E23 pseudocode is preserved. This suite independently checks the
existing finite component contract and does not propose a replacement model.
Stop after QA03; implementation corrections, if required, belong to Work 2.
