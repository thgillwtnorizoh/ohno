# 1. Result

QA06 — independent R08/R08-C01/R08-C02 verification, Work1, 2026-11-03
(Asia/Bangkok). Production target `ae0b35c6aa0f3dbda815559f844c9678cde8f7d6`.
**PASS — genuine Windows headless execution.** WPF win-x64 Release compiled with
0 warnings/0 errors. Independent QA06 **99/99**; unchanged QA01 **90/90**, QA03
**56/56**, QA04 **43/43**, QA05 **66/66**. Total **354 independent QA cases**, no
final failures. R06 four preparation targets/four fixture loads; unchanged R07
8, R08 13, C01 4 and C02 14 development groups passed separately.

[Passing run41640870103/job125933753872](https://github.com/thgillwtnorizoh/ohno/actions/runs/41640870103/job/125933753872),
attempt1, checkout **767d36a5b868d241d2fefc4020d8bf05be613fe8**, 2026-11-03
13:02:59–13:03:49 UTC /20:02:59–20:03:49 Asia/Bangkok. Windows Server2022
Datacenter10.0.20348/image20261025.346.1, preinstalled SDK8.0.427. results.json
records exact named QA06 outcomes, prior suite counts, all step conclusions,
commands, source hashes and both retrieved-log hashes. gate-output.txt contains
selected actual output; no private/authentication details or full runner logs.
Only checkout's Node20 advisory, distinct from compiler diagnostics.

Two executions: the initial gate's97/98 result exposed one authored oracle
arithmetic error. Its input/output and failed run are preserved in
initial-gate-output.txt and results.json. Independent binary32 verification
corrected that assertion and added A35; production was unchanged. This is not a
hidden implementation failure, weakened assertion or original-research correction.
The passing gate reran the entire required matrix. No further successful gate
was redundantly rerun. Final publication changes only results/report/journal over
the tested source; harness, workflow and all production code remain identical.

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
force update. Actual runs, diagnostics and results are recorded here and in results.json.



## Disposition and preservation

**VERIFIED COMPONENT:** the99 independent cases satisfy their bounded assertions.
**No confirmed implementation defect and no Work2 correction required for the
tested finite cases.** Both supplied fixture totals match the independently
specified E34 controls; no test uses582 as the Joined fixture expectation.
C01's pure visibility predicate preserves component state and preceding-slider
head/held/checkpoint/parent accounting in the selected supported scenarios.
It does not prove that a Windows physical-input interruption was resolved.

Source inspection confirms b/25 before Double multiplication and the explicit
accounting/provenance boundaries; it does not establish untested callback effects.
E33 M0ad2 F153d store100c, old-combo score arithmetic10e7–110b, combo writes11c7/
11d3/11ea and admitted score write1394–13a2 remain the original authority.
E34 M1bc1 F0ff2 store0f81 and F0fe5 store1098, M1bc4 Single→Double0045/Round0046,
and M0ab7 F0605 store0074 supply the finite preparation contract.
No original executable inspection or new E035 finding was needed.

All242 baseline paths remain.240 blobs/modes outside the authorized workflow and
append-only journal are unchanged. The journal preserves its entire345905-byte
prefix. Work2 implementation/reports/journal, QA01–QA05 assertions/results, original
research, E20 and all historical fixtures are retained. Remote expected-head,
ancestry and complete tree/blob/mode verification are performed for final handoff.
Published commits preserve both QA runs. No force push or permanent QA branch.

## Bounded coverage map

| Cases | Independent finite discriminator |
|---|---|
| A01–A35 (37 cases including midpoint variants) | Literal N/first/last/break/T/K vectors; zero/subsecond/equal/unsorted/unknown rows; numeric breaks/overlap/signed and unchecked duration; spinner end independence; counts/type mask; modern fractional/default/clamped difficulty versus older Byte syntax; exact/near ties and Single-to-Double boundaries; malformed/nonfinite/empty rejection; strict gameplay admission versus metadata-only controls; missing source |
| B01–B22 (39 cases including raw/quality/policy variants) | E33 literal base table; whole counters/Single accuracy; old rather than incremented/maximum combo; b/25 quotients and fractional bonus truncation; zero K; parent scaling/bypass/MISS; terminal/interior distinction; spinner amounts/final; disabled admission versus counters; raw0; UInt16 count/snapshot wrap; finite unchecked Int32 score wrap; explicit unsupported policies/provenance/decorations/K; due-prefix raw128 on interior producer |
| C01–C08 (11 cases including fixtures/K controls) | Automatic production config; exact E034 Mixed2392/2236 and Joined534/438 streams; real Joined session emissions; Mixed lifecycle with explicitly seeded spinner observations; fresh restart/replacement; failed new metadata leaves independent prior session intact; delayed parent with pending backlog |
| D01–D07 (12 cases including six time boundaries) | Pre-start/start/interior/end/post-end-pending/evaluated presentation; state snapshot invariance; head admission inside old preview; held slider7000–9900 through8000/9000/9864 checkpoints; unchanged held-button behavior; strict spinner progress/end completion retained |

A28/A35 arithmetic proof is below. A18/A19 use metadata-only Int32 boundary
operands; they do not claim a playable negative-time map. B17 uses65536 supported
zero-K events, the smallest count wrap; B18 uses8000 same300 events with K10 and
the independent closed form300*n+60*(n-2)*(n-1) before Int32 wrap. This is a bounded
arithmetic boundary test, not random generation, a performance test or complete
score simulation. Each individual bonus stays within the admitted conversion
range. Current-combo Int32 overflow and invalid floating-to-int results are not
forced or assigned original behavior.

C02/C03 intentionally send explicit events to a normally configured session's
ledger while JudgedCount remains0; they are not natural object evaluations. C04
instead drives actual production slider head/checkpoint/parent methods. C05 drives
real circle/slider/final dispatch but explicitly calls the public spinner observer
and supplies its component results; it does not fake a physical spin. No private
field rewrite, OS input or wall-clock scheduling is used.

D05 derives linear slider7000–9900 independently: V100/Q100, deadlines8000,9000,
9864 and endpoint390. Metadata row starts7000/10000 give T3/N2/K2. Head30+two10+
terminal30 plus parent300+72 gives452, combo4, one whole result. D04 independently
uses slider9000–9800 with K3: head30+terminal30+parent336=396. Both are supported
nonoverlapping intervals and remain before spinner10000. Repeated presentation
queries are compared against state and a query-free held-button control.

Source inspection confirms Program map loading prepares FromBeatmap before
replacing the active session; restart constructs a new ledger with retained
configuration. PlayfieldView calls SpinnerPresentation only from drawing and
separately samples buttons/pointer. CircleSession.Press excludes pending spinners
from their start, not start-minus-approach; Track remains a separate path.
These are source/component findings, not evidence about native focus, WPF
presentation, asynchronous audio replacement or physical input interruption.

**ENGINEERING boundaries:** full Beatmap.Load rejects unsupported types/equal
starts/overlapping spinner intervals/malformed inputs; ReadFresh exposes narrower
metadata-only behavior. Missing metadata, nonfinite conversion, K0–10, unsupported
policies and event provenance fail explicitly. Older Byte syntax is tested only
within supported0–10 difficulty, not as certification of every historical parser
branch or cached difficulty reuse. Original partial-error recovery is not modeled.
The one-argument historical session constructor remains an explicit engineering
K1 compatibility path; the normal app uses the tested automatic factory.

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
