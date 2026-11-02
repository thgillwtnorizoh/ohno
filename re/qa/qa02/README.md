# 1. Result

QA02 — R03 build and regression gate, Work 1, 2026-11-02
(Asia/Ho_Chi_Minh). Application target:
`a026a2e6b980e9181c70fb76268423bcdaa0664d`, verified as current `origin/main`.

**PASS — genuine hosted compilation and component execution obtained.**

| Gate | Actual outcome |
|---|---|
| Windows x64 WPF build | **SUCCEEDED: 0 compiler warnings, 0 errors** |
| Existing QA01 headless C# suite on R03 sources | **90 executed, 90 passed, 0 failed** |
| QA01-D01 | NaN X/Y rejected; finite computed radius equality still accepted |
| QA01-D02 | Malformed and empty mandatory hitSound values rejected |
| Application launch, GUI, physical input, audible playback | **UNVERIFIED / not performed** |

The local Work host has no `dotnet` command or R02-documented
`/tmp/ohno-dotnet/dotnet`. No SDK installation or environment repair was performed.
The authorized fallback ran once on GitHub-hosted **Windows Server 2022
Datacenter 10.0.20348**, image `windows-2022` version `20261025.346.1`, using
preinstalled **.NET SDK8.0.427**.

Execution evidence:

- Gate commit: `6dd36f703504b07a3a97a136b09bba0dc4b1b2b3`, direct child of R03;
  only the workflow and initial QA02 procedure were added. Application and
  existing test blobs were unchanged.
- [Actions run41442622060, attempt1](https://github.com/thgillwtnorizoh/ohno/actions/runs/41442622060),
  job `125267948839`, completed **success**.
- `results.json` records exact commands, all90 case names/outcomes, SDK/runner,
  source blob/SHA-256 identities, run/job/step status and evidence hashes.
- `gate-output.txt` preserves the relevant SDK/build/component console output.
  Timestamps/ANSI, checkout/auth details and runner cleanup are omitted; the hash
  of the complete retrieved UTF-8 job-log text is separately recorded.
- The runner emitted a checkout Node20 deprecation/Node24 compatibility advisory.
  This was not an application compiler warning or test failure; no extra rerun or
  unrelated action-upgrade work was started.

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

The bounded source diff shows both requested corrections in R03, and the actual
headless test output now verifies their scoped component behavior:

- `CircleSession.Press` uses rejection of `!(distanceSquared <= radiusSquared)`.
  This expresses ordered acceptance and preserves finite equality (QA01-D01).
- `Beatmap.Load` validates `row[4]` with invariant `Int32.TryParse` and rejects
  malformed/empty values (QA01-D02).

The existing headless suite linked current gameplay/layout sources and passed
NaN X/Y, finite radius equality, malformed hitSound and empty hitSound assertions,
alongside its finite timing, expiration, selection, parser and session-isolation
cases. Its source and expectations were not modified for this gate. No compiler
error or regression failure was observed in the requested build and suite.

These CI logs establish compiler diagnostics and component-test outcomes only.
Windows launch, visual presentation, audio output, physical input and original
runtime equivalence remain **UNVERIFIED**, exclusively human-operated. No audio
latency, device or BASS investigation is performed.

R03's audio implementation **compiles**, but MediaPlayer playback, asynchronous
decoder behavior and synchronization were not exercised. Passing90 component
cases is not certification of the complete application or original-game fidelity.
The hosted Windows capability is restricted to this noninteractive gate; it does
not provide or authorize an original-game/E20 execution environment. E20 remains
incomplete with all17 original-runtime cases NOT RUN. QA01's old R02 source
fingerprints, failing static results and originally unexecuted C# status remain
unchanged historical records; QA02 provides this later execution evidence.

The results-only publication does not change the workflow path, so it does not
trigger a second gate. All178 R03 baseline blobs/modes, including Work2 code and
all QA01/history assets, are retained. Final publication ancestry/scope and the
complete remote tree are verified in the handoff. QA02 stops after this gate.

# 3. Pseudocode

Original behavioral model unchanged. No new original-code evidence is obtained.
QA02 performs only compilation and the existing noninteractive component suite;
it does not revise gameplay pseudocode or Work 2 implementation.
