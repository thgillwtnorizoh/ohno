# 1. Result

QA06 — independent R08/R08-C01/R08-C02 verification, Work1, 2026-11-03
(Asia/Bangkok). Production target `ae0b35c6aa0f3dbda815559f844c9678cde8f7d6`.
Initial Windows gate ran on1efda537090421a25499ef4aac80ddb25376b88c:
WPF compiled0 warnings/errors; all255 previous QA cases and every development
suite passed. QA06 executed98 cases:97 passed/1 failed. The failure was an authored
oracle arithmetic error, not evidence of a production defect. The corrected
binary32 assertion and an additional discriminator require a new gate; no final
passing outcome is claimed yet.
No local .NET SDK is available; no SDK installation or desktop provisioning.

The new net8.0 headless project links actual production Beatmap.cs,
CircleSession.cs (including ScoreLedger) and the pure SpinnerPresentation helper.
It does not reference WPF, audio, app entry points or development-check methods.
The existing Windows2022/preinstalled-SDK8 workflow retains every regression and
adds one QA06 step/path. No production fix, game launch, OS input, OBS, debugger,
original executable inspection/execution or E20 experiment. All17 E20 cases remain
NOT RUN — WINDOWS HOST REQUIRED.

```powershell
dotnet run --project re/qa/qa06/Headless/QA06.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

# 2. What they found

## Independent expectations and scope

E33 M0ad2 supplies the literal base/whole/combo table, old-current-combo bonus,
UInt16 counts, unchecked score write and Single accuracy. E34 M1bc1/M1bf2/M1bc4/
M2d7d/M0ab7 supplies raw file-order metadata, start-span-minus-breaks integer
seconds, fresh difficulty defaults, Single-order computation and Double Round.
Original research is unchanged and no new original-software claim is made.

OracleVectors.cs contains manually derived finite N/first/last/B/T/K vectors,
not a second parser. AccountingTests.cs uses literal E33 amounts, named finite
sequences and a closed-form same-300 sum for the supported Int32 wrap probe.
No conventional scoring engine, replay player or copied production algorithm.
The valid positive base amounts are all divisible by25, so numeric quotient tests
alone cannot distinguish Int32 division from equivalent Double division for those
values; source inspection independently checks that b/25 precedes conversion.

The supplied fixtures predict Mixed K2/2392 and Joined K3/534 under E34's stated
component stream; engineering K1 controls give2236/438. The owner-reported582 is
not an expectation for the supplied Joined fixture. Original test3 bytes/settings
and complete event identity remain unknown. Explicit ledger sequences and seeded
spinner observations are artificial component controls, never natural gameplay.
Session controls separately exercise actual head/checkpoint/parent emissions.

C01 visibility is ENGINEERING: pending && time>=start, including exact end and a
post-end time before completion. No native fade timing is inferred. Pure queries
are checked for state invariance, with actual held slider processing across the
old pre-start preview interval. Physical input continuity is still UNVERIFIED.

Coverage excludes cached/reused partial original parsing, native precision and
callbacks, modifier/ScoreV2/high-decoration rules, arbitrary chart equivalence,
full bonus payouts, pixels/audio and human physical input. Engineering rejection
policies are separately identified. Metadata-only unknown/equal/unsorted/overflow
controls do not imply those maps are accepted for playable sessions.

Read AGENTS/standing contract, research/reconstruction journals, E33/E34, R08/C01/
C02 and QA01–QA05 contracts/interfaces. No prior assertion or fixture changes.
Publication uses canonical main and expected-head leases, no permanent branch or
force update. Actual run/diagnostics/results will be recorded after execution.

## Preserved initial oracle error (not a production correction)

[Initial run41640348883/job125932009558](https://github.com/thgillwtnorizoh/ohno/actions/runs/41640348883/job/125932009558)
failed A28's originally authored expected1.5; actual1.4999999. Exact input is in
initial-gate-output.txt. It used HP5/OD6.4/CS0 and two decreasing row starts, so
negative density clamps0. Independently applying IEEE binary32 rounding at the
E034-prescribed boundaries gives:

| Operation | Exact resulting Single / hex bits |
|---|---|
| Single(5f+6.4f) |11.399999618530273 /41366666 |
| preceding Single /38f |0.29999998211860657 /3e999999 |
| preceding Single *5f |1.4999998807907104 /3fbfffff |
| Double Math.Round |1 |

This was checked separately using Python struct binary32 pack/unpack, without
calling production code or another gameplay implementation. A28 retains the same
input and checks exact bits/K1; promoting the sum to Double for the remaining
operations then casting yields1.5/3fc00000 instead. The old failure and original
run remain published, not hidden or counted as a production pass.

Added A35 independently: N21/T40,HP=OD=CS10 gives density40866666, Single sum
4208cccd, /38 ->3f666667, *5 ->40900000 (4.5), midpoint-even K4. Continuing from
that same Single sum entirely in Double instead gives K5. This adds a stronger
final-integer discriminator rather than removing the arithmetic boundary check.
The correction does not alter production, E034 or any historical assertion.

# 3. Pseudocode

E33/E34 and E24–E26 original behavioral models remain unchanged.

```text
QA observation procedure:
    read independent finite input/expected vector
    invoke actual production metadata/preparation or ledger/session method
    compare original-derived operands/results and separately labeled engineering policy
    retain each failure, expected/actual values and deterministic input
    run unchanged historical component suites separately
    classify only executed finite cases as VERIFIED COMPONENT
```

No new reconstruction feature, E035, QA07 or R09. Stop after QA06.
