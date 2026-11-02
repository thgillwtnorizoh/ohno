# 1. Result

QA02 — R03 build and regression gate, Work 1, 2026-11-02
(Asia/Ho_Chi_Minh). Application target:
`a026a2e6b980e9181c70fb76268423bcdaa0664d`, verified as current `origin/main`.

The local Work host has no `dotnet` command or R02-documented
`/tmp/ohno-dotnet/dotnet`. No SDK installation or environment repair is performed.
The authorized fallback is one minimal GitHub Actions gate on `windows-2022`,
using an already installed .NET8 SDK. Initial gate status: **PENDING PUBLICATION
AND EXECUTION**. This procedure is not a passing build/test result. A later
QA02 result record will identify the actual run or precise permission/SDK blocker.

The gate selects an installed stable8.0 SDK using an untracked runner-local
`global.json`, compiles the actual WPF application for Windows x64, and runs only
the existing QA01 headless C# suite. It never runs the application, starts media
playback, creates a game window, generates OS input, captures gameplay, starts
OBS or invokes a debugger. No original executable is read or executed for QA02.

Exact commands:

```powershell
dotnet --list-sdks
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

`.github/workflows/qa02-r03.yml` triggers on a push to `main` that changes that
workflow file. Later QA02 results-only commits do not repeat the gate. Its only
permission is contents:read; checkout is pinned and does not retain credentials.
If compilation fails, the headless step still runs when SDK selection succeeded.
No additional framework, SDK setup action or installation is included.

All Work 2 files, historical research and QA01 source/provenance/results remain
unchanged. The old QA01 Python fingerprint checks target R02 and are not run or
relabeled as R03 tests. No all-history research campaign is repeated. Publication
uses `main`, an expected-head check and no force-push or permanent milestone branch.

# 2. What they found

The bounded source diff shows both requested corrections in R03:

- `CircleSession.Press` uses rejection of `!(distanceSquared <= radiusSquared)`.
  This expresses ordered acceptance and preserves finite equality (QA01-D01).
- `Beatmap.Load` validates `row[4]` with invariant `Int32.TryParse` and rejects
  malformed/empty values (QA01-D02).

These are source observations, not executed test passes. The existing headless
suite links current gameplay/layout sources and includes NaN X/Y, finite radius
equality, malformed hitSound and empty hitSound assertions, alongside the existing
finite timing, expiration, selection, parser and session-isolation cases. Its
source and expectations are not modified for this gate.

CI logs will establish compiler diagnostics and component-test outcomes only.
Windows launch, visual presentation, audio output, physical input and original
runtime equivalence remain **UNVERIFIED**, exclusively human-operated. No audio
latency, device or BASS investigation is performed.

# 3. Pseudocode

Original behavioral model unchanged. No new original-code evidence is obtained.
QA02 performs only compilation and the existing noninteractive component suite;
it does not revise gameplay pseudocode or Work 2 implementation.
