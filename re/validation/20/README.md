# E20 Windows operator packet

**RUNTIME VALIDATION BLOCKED. All 17 cases: NOT RUN — WINDOWS HOST REQUIRED.**

This packet was prepared and checked on Linux. No original executable was launched. Preparation success, valid JSON, copied reference files, schema selftests and static regressions are not gameplay-validation passes. Work 2 remains suspended. Keep the reference, logs, debugger files and captures private; this repository contains no proprietary binaries/assets or runtime dumps.

## Required capability and stop rules

Provide an owner-authorized, disposable **actual Windows** environment with an interactive display/input/audio device, snapshot/rollback, offline operation, the pinned reference and a matching-architecture original-CLR debugger/profiler. No Windows endpoint, VM, desktop or bound instrumentation adapter is exposed to the current session. A generic browser, Linux process, CI shell or unverified emulator does not supply this missing evidence. Do not probe other machines or credentials.

An operator must supply the debugger/profiler and implement its recording/binding configuration using `probes.json` and `INSTRUMENTATION.md`. The packet does **not** contain an automatic CLR operand tracer. If a consumed operand cannot be captured without invoking another game method, mark it unavailable and the comparison inconclusive. Do not substitute a re-read field or a convenient formula. PowerShell scripts have not been parsed or executed on Windows; first run the parser check below. Stop on parse/import/launch/dependency/resource errors and preserve the actual error before changing anything.

Requirements: Python3.10+ standard library for `pilot.py` (tested here with host Python3.12), Windows PowerShell5.1 for the two helpers, an existing authorized recording tool, and an existing managed debugger/profiler capable of binding original tokens to the current JIT. Record actual versions, binary hashes, process architecture and CLR. Installed .NET metadata alone does not prove the original process's CLR. No downloads, online login or unrelated networking are needed by these scripts. If a dependency is missing, record that blocker and provision it outside the experiment with its hash, then use a new run.

## 1. Preserve inputs and prepare a fresh private run

Read `../../Instruction.md` and E01/E10–E19 first. The required research parent is `49726ef54767804f45784fae092c8ad70b23f7b5`; use the E20 publication for the packet. Work on copies, keep the archive read-only and retain a VM snapshot before launch. Software ownership/authorization and offline isolation are operator prerequisites, not facts established by a checkbox or script.

In PowerShell, adjust only the three local paths/run ID:

```powershell
$repo = 'C:\Research\ohno'
$archive = 'D:\ReadOnlyInputs\osubackup.zip'
$run = 'C:\PrivateEvidence\ohno-e20-run01'
$packet = Join-Path $repo 're\validation\20'
py -3 (Join-Path $packet 'pilot.py') prepare --archive $archive --output $run --run-id e20-run01
if ($LASTEXITCODE -ne 0) { throw 'Preparation failed; do not launch.' }
```

`prepare` refuses a wrong archive, changed fixture/generator, existing output and a destination inside the checkout. It checks CRCs and executable SHA256; extracts the exact archive to `$run\game`; copies the unchanged E16 slider and the separately hashed E20 circle into distinct Songs folders; regenerates E01 silence with the unchanged generator and verifies WAV bytes/header/hash. It never launches the game, imports objects, sets raw results or calls any gameplay code. Directory placement is an import **attempt**, not proof the game accepted the map.

The bundle includes:

- `baseline-files.json`: every extracted archive-member hash, before first launch.
- `manifest.json`: populated input hashes but all runtime cases still NOT RUN, no actual observations.
- `environment.manual.json`: unresolved display/CLR/skin/input/isolation/settings fields to fill from real measurements.
- `bindings.template.json`: original method identities/body hashes; every runtime binding initially unverified/null.
- `private_artifacts/`: your private traces, captures, settings snapshots and diagnostics.

The E16 fixture is preserved byte-for-byte. Its predicted schedule[2500,3000,3464]/end3500 is **not** an imported observation. New E20 circle: one ordinary stored(256,192), S2000, HP0/CS5/OD5/AR5, base500ms, multiplier1/tickrate1; its original import is also untested. Both reference `silence.wav`, six-second mono16-bit44100Hz silence. Silence cannot establish acoustic synchronization. If either fixture needs correction, preserve it, write a new separately hashed derivative and record why; never inject runtime objects to bypass import.

## 2. Parse the Windows scripts and collect the environment

Run this actual Windows parser check before either helper; keep errors/output as a private text artifact. A Linux text/guard check is not this parser test.

```powershell
foreach ($name in @('Collect-Environment.ps1','Start-Reference.ps1')) {
    $tokens = $null
    $errors = $null
    [System.Management.Automation.Language.Parser]::ParseFile((Join-Path $packet $name), [ref]$tokens, [ref]$errors) | Out-Null
    if ($errors.Count -gt 0) { $errors | Format-List; throw ('Parse failed: ' + $name) }
}
powershell -NoProfile -File (Join-Path $packet 'Collect-Environment.ps1') -RunDirectory $run -Label pre
```

The collector reads Windows/CIM adapter/sound/installed-runtime/config metadata and host QPC frequency/ticks. It does not read original gameplay state. Complete manual fields for actual active monitor/resolution/DPI/scaling/refresh, window/client/viewport, active sound endpoint/driver, keyboard/mouse/bindings, skin/assets, frame limiter/compatibility/offsets, dim/video/storyboard, capture settings and instrument versions/hashes. Adapter-reported refresh and listed sound devices are not proof of the active display/endpoint. Unknowns stay explicit. Record relevant settings/config/skin hashes before and after each run; keep local paths/user data private.

## 3. Launch the exact disposable copy; establish import first

Start an untraced recording before launch if available. The following switches assert prerequisites already established by the operator; they do not configure a firewall or VM snapshot.

```powershell
powershell -NoProfile -File (Join-Path $packet 'Start-Reference.ps1') -RunDirectory $run -AuthorizedCopy -DisposableOffline
$launch = Get-Content -LiteralPath (Join-Path $run 'private_artifacts\launch-attempt.json') -Raw | ConvertFrom-Json
powershell -NoProfile -File (Join-Path $packet 'Collect-Environment.ps1') -RunDirectory $run -Label process -GameProcessId $launch.process_id
```

The launch helper checks the pinned EXE hash and all original EXE/DLL hashes before calling `Start-Process`. It records only launch attempt/process ID/error; it does not certify import, entry or gameplay. Stop if updater/network/login/dependency dialogs block the offline copy; do not log in or replace the binary. Confirm the running module's disk hash and loaded module identity again in the debugger. Self-declared Stable40/version1.3.3.8 is not official authentication.

Use the original Play/map-selection controls actually present. Record exact menu labels, clicks, selected map identity and errors instead of assuming unobserved UI. Select the unchanged slider from its copied folder; preserve an actual gameplay-entry capture. Verify P1-IMPORT/P1-ENTRY before other pilots: selected/imported map hash/path, parser values, T02af parent and T06f6 head identities/types, registered parent, actual segments/Movement interval/endpoints, duration/end, component list/order and readiness. A real mismatch is a finding; stop, retain it, then diagnose before proceeding. Do not use `[2500,3000,3464]` as measured input to an explanation.

## 4. Bind and exercise the minimum cases

Follow `INSTRUMENTATION.md`. Resolve method tokens/body identities against the loaded module, then current JIT addresses/IL mappings. Do not set breakpoints at a CIL offset treated as a native address. Record null/missing/optimized operands explicitly. Work in progressively enabled method groups, not all methods continuously. A stopped debugger pass and a low-overhead natural trace are different observation modes.

Use fresh runs/restored matched baseline for independent controls, with original UI configuration and hashes. Record configured keyboard hit keys rather than assuming Z/X defaults. Use real keyboard/mouse actions and original visible objects; no injected result/state/timestamp in natural runs. The full procedure/alternatives/observations per case are in `test-matrix.json`.

| Order | Required natural action and evidence |
|---|---|
| P2-HIT/WIDER/SPATIAL/EXPIRE | Select new circle control. Fresh run: center and press near start; fresh earlier/later press; spatially outside with comparable timing; no press through timeout. Read actual clock/error/width/radius/mapped cursor/evaluated/raw/handoff operands. Sort a press into a timing category only from consumed values. Exact integer equality and distance equality are not assumed. |
| P3-A | Fresh no-SD slider: successful head, hold configured input and follow the original ball through its one traversal. Observe genuine first-interior success. |
| P3-B | Same head/config/prior combo where possible: genuinely release or leave the follow area across an imported interior deadline. Observe failed component from original conditions, not a rewritten enum. |
| P3-C | Fresh run: miss/omit head then genuinely hold/follow before a later component if naturally reachable. Log a blocker if selection/timing prevents the intended case. |
| P3-D | Release/re-enter around a deadline in separate fresh runs. Log actual F0bf2 versus selected timestamp and current follow predicate; intended input time is not the consumed clock. |
| P4-COMBO | Follow matched P3-A/B through F153d and M40f1. Record liveF2674, max-likeF1975/snapshotF198a separately, tracking state and reached feedback/observer callbacks. |
| P4-COMPLETE/POLICY | Continue no-SD runs to actual end. Record pending count, head raw, F13c3 and **actual F198e null/presence/type/identity**, q operands, returned parent aggregate, manager conversions/decorations and stored flags separately. The actual optional-policy configuration is a priority, not inferred from a displayed score. |
| P5-NO-SD/SD/PAIR | Fresh matched failed-interior runs with SD absent/present using original UI if permitted. Log both record refs/converted bitmasks, target/accumulator/presentation, M2979 argument, subsequent delta, M226c guards, recovery count, actual latch/time and later observed continuation/failure. If UI cannot configure SD offline, record that exact blocker; forced bits are separate probes. |
| P0-OVERHEAD | Where practical, three untraced and three traced repeats for relevant controls, with identical display/settings/input intent. Record actual callback durations, QPC hook entry/exit, dropped events and cadence distributions. A stepped breakpoint run must not stand in for natural timing. |

Do not engineer a backlog, force exact equality or alter counters in a natural run. A debugger-forced probe can be useful, but must get a separate run ID/mode/configuration manifest. Distinguish naturally reached original state under debugger pauses from low-overhead gameplay; both have confounds. Do not claim physical input latency, audio synchronization or first visible frame from managed trace ordering.

## 5. Record and validate evidence without inventing a pass

Copy actual artifacts into `$run\private_artifacts`. Trace records follow `trace.schema.json`, with original numeric types, exact float bits, process-local ref IDs, real thread/collector/per-thread sequence/QPC and actual consumed operands. Do not publish placeholder JSONL as runtime evidence. No actual trace example is included in this repository.

After collecting files:

```powershell
py -3 (Join-Path $packet 'pilot.py') seal --run $run
# Edit only this PRIVATE manifest: classify artifacts; fill actual environment,
# instrumentation, case status/observations/comparison/evidence IDs and confounds.
py -3 (Join-Path $packet 'pilot.py') validate --run $run
```

`seal` hashes private artifacts and updates their index in the private manifest. It never uploads anything. Retain originals if redacting; link derivative and source hashes and record the transformation. `validate` checks format/hash/typed records/sequence/declared provenance and rejects empty unrun cases claiming observations, missing evidence or completion overclaims. It is not a full JSON-Schema engine, does not authenticate debugger origin and **never certifies gameplay acceptance**. Hash integrity alone cannot prove a trace came from the game. All positive observations require human review against captures, verified bindings, original operand sites, real settings and perturbation measurements.

For a genuine run, use run status `OBSERVED_PENDING_REVIEW` or `BLOCKED_DURING_WINDOWS_RUN`; corresponding cases use those statuses, comparisons MATCH/MISMATCH/INCONCLUSIVE, actual_observations as an object with a real summary and event_seqs list, and nonempty evidence IDs. Referenced event sequences must belong to that case and match its observation_mode. The instrumentation.binding_artifact_id must link the filled verified binding file; observed trace events require matching process/module/JIT bindings. Cases not run retain the exact NOT RUN status, null observations, NOT_RUN comparison and empty evidence IDs. `gameplay_validation_completed` staysfalse in this structural manifest. A later reviewed report decides E20's acceptance criteria from actual evidence. Keep clock/field/reference snapshots distinct from consumed operands and do not claim missing callbacks as suppressed without branch/exception evidence.

Before any public publication, review all private artifacts for usernames, local paths, account/config data, memory dumps and copyrighted content. Publish only authorized minimal redacted summaries plus hashes; no automatic artifact publication is included. Preserve disagreements in a new journal/report entry; do not edit completed expeditions.

## What is still missing

No original import/readiness, circle key or expiry, live slider checkpoint success/failure, current-combo consequence, optional-policy selection or matched SD/no-SD fail/continue observation exists for E20. No capture, trace, Windows environment or overhead measurements were obtained. Windows script parsing/launch/collection and debugger/profiler binding are unexecuted. The acceptance objective is **incomplete**, even though the preparation package is reproducible.
