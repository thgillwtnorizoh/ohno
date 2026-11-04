# 1. Result

QA08 — Independent NewCombo Preparation & Health Calibration Integration.
Work 1, 2026-11-04 Asia/Bangkok. Intake canonical main:
`7ce5b86282f601f405af74883d9b34a8a965276f`. Verification only; no production edits,
original-binary excavation or original-runtime experiment.

The .NET8 Windows gate is **PENDING at harness publication**. The final report and
results will record its exact tested commit, run/job URL, case outcomes and source
hashes after execution. A preliminary local net10 run using the already installed
SDK10.0.402 passes115/115 after the independently justified oracle correction
below. That preliminary run does not satisfy the required net8 gate.

New `Headless/QA08.Headless.csproj` links actual production `Beatmap.cs` and
`CircleSession.cs`, with no replacement parser/preparer/calibrator/session engine.
It calls no R10 development-test code and changes no historical assertion.
`Inputs/three-unmarked.osu` is a separate independent input; the bundled
eight-circle `first-circles.osu` stays byte-identical. Each generated parser input
and selected calibration intermediate is recorded with its named case in results.
Only QA08 files, two workflow path filters/one suite step and the research-journal
append are changed. No authorised exception.

Reproduction on the existing Windows2022 runner/preinstalled .NET8 SDK:

```powershell
dotnet run --project re/qa/qa08/Headless/QA08.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

The unchanged gate also compiles WPF win-x64 Release and executes QA01(90),
QA03(56), QA04(43), QA05(66), QA06(99), QA07(159):513 historical independent
cases. R06 targets/fixture loads and R07/R08/C01/C02/R09/R10 development groups
remain separate. QA08 adds115 independent named cases, rather than relabeling
R10's23 development groups or counting the repeated local run again.

No SDK or OS installation, game launch/window, OS-input generation, OBS,
debugger, original executable inspection or E20 experiment. E20's17 cases remain
NOT RUN. The WPF code compiles in the hosted gate; no WPF window is launched.

# 2. What they found

## Independent authority and coverage

**CONFIRMED STATIC authority:** retained E036/E035 and relevant E004/E012/E021;
E026 is consulted solely to prove the short-spinner oracle correction. No new
original-static extraction was performed. E05's corrected ordinary factory chain
is preserved. **RECONSTRUCTED** describes independently derived expected values;
**VERIFIED COMPONENT** applies only to actually executed finite component cases.
General original runtime, pixels, input/audio scheduling and chart equivalence
remain UNVERIFIED.

Preparation reads the standing contract/AGENTS reference to RTK, the research and
reconstruction records, E036/E035/E004/E012/E021, R09/C01/R10 and QA06/07 reports
and interfaces. No physical repository AGENTS.md is present. Production parser,
NewCombo preparation, chart admission/calibration/session and application
Load/Restart binding were inspected. All original evidence and historical
journal prefixes are preserved.

| Case family | Independently checked distinction |
|---|---|
| P01–P06 |Null/C/L/spinner/T081f helper operands; unknown rejection; literal E036 N01–N13 flags; explicit bit still retains helper result; separate legacy-spinner constructor and parser overwrite |
| B01–B08 |Strict less/equal/greater break-end boundaries; multiple consumed breaks; true flag retention; supplied break order and blocking pending head; spinner forcing; E036 while versus E035 single-gap exclusion |
| G01–G18 |Retained/clone/editor/alternate/domain/order/type/identity guards; complete provenance operands; colour skip; reference rather than value identity; actual parser-definition/health correspondence; unsupported charts stay playable |
| A01–A07 |Historical constructors/two-argument FromMap; fresh E036 factory; unavailable fresh clone cannot fall back; application Load/Restart source binding |
| C01–C07 |Next final property versus first flag; last-object reward; explicit/break-forced preceding reward; equality discriminator; independently derived five-circle trial states and coefficients |
| I01–I07 |Fresh production parsing/session eligibility for three circles, mixed sliders/spinners/breaks and bundled eight circles; restart/replacement isolation; unavailable-health score continuation |
| R00–R05 |Short-spinner threshold correction discriminators; matched health-enabled/disabled score/judgement/checkpoint/spinner streams; group changes versus E034 K; C01 recovery threshold; E035 delta/drain/smoothing |

Expanded loops produce115 named cases. Results retain each name and actual
outcome. Direct type/control inputs (especially AssignableT081f and old-format
spinner construction), artificial unsorted prepared breaks and explicit host
timings are labeled component operands. They do not establish alternate-factory,
legacy-spinner gameplay or original manager reuse support.

The reconstruction categories simplify E036's assignable runtime-type checks.
The ordinary source adapter rejects T081f/unknown identities and mixed raw types;
the separate pure predicate accepts the explicit artificial T081f operand. No
stable sort replaces original BinarySearch registration: the bounded API rejects
equal/decreasing starts. Parser source order is retained even when its circle
list is sorted, and fresh HP rejects the inconsistent order.

## Final state, provenance and API binding

Expected circle/slider flags come from E036 M2902:0006/0013/0023 followed by
M28f7:1638/1a80 bitwise OR. Spinner M28f7:1bb7 separately assigns file bit4,
overwriting temporary M3201<=8 constructor state. Following C/L sees the spinner
runtime category regardless of its own bit. Provenance checks separately retain
raw type/file bit/colour skip, source index, exact object identity, previous
registered category, inferred value, factory argument, constructor/parser flag,
expired-break count and final flag. E036 M28db:0090/00bc supplies strict break
forcing; existing true flags are never cleared.

Fresh HealthChart ownership is an **ENGINEERING** safeguard. A record clone or
manually assembled map with copied source metadata is rejected, even when all
data references match. Domain, retained-manager, alternate-factory and registration
ambiguity are separately rejected. Pure supplied-order break controls do not
weaken HealthChart's existing ordered/gap-contained break guards.

`HistoricalR09` is the owner-approved historical API contract. The default
constructors and two-argument `HealthChart.FromMap` retain unknown following
unmarked NewCombo rejection; their old QA07 assertions remain untouched.
`CreateFreshOrdinary` constructs `E036FreshOrdinary`; production Load/Restart
call this factory and assign its result. Fresh unsupported preparation leaves
health unavailable with the E036 reason, rather than trying the historical path.
The two policies are an engineering compatibility split, not two original rules.

**Application evidence limit:** A06/A07 inspect the actual Load/Restart method
bodies and factory-result assignment; A04/I05 execute their production session
factory headlessly. These are separately reported source binding and component
execution. The WPF Load/Restart methods themselves are not executed, because doing
so would enter WPF/audio/window lifecycle outside this assignment. Source
inspection confirms `Infer(last)` precedes bitwise OR even for explicit bit4;
the pure API exposes no injected call counter, so provenance values plus source
binding establish this bounded behavior, not an original call-count measurement.

## Independent calibration derivations

E035 M1150:0355/036a/0378/03cd selects1028 for last/next.NewCombo, otherwise1024.
The oracle never invokes production calibration to compute its expected value.

- Two circles2000/2100, HP10, approach0, r.05/B=G1: following false gives
  `A=200+6-5+20=221`; following true gives `200+20-5+20=235`.
  Both cap H/snapshots200. Either first flag produces the same forward decision.
- Break2010–2020 between those circles forces the second flag and modern exclusion20:
  `A=200+20-.05*(100-20)+20=236`.
- Break2010–2100 has end equal to the second start. It does not force that flag,
  while E035 permits inclusive gap containment and excludes100. Thus
  `A=200+6+20=226`. This discriminates the two algorithms' boundaries.
- Objects2000/3000, HP10/approach0, breaks2100–2200 and2300–2400: E036 consumes
  both and forces the second flag; E035 consumes only the first, excludes200,
  loses40, ends H180/A200. A conflated break loop would change those operands.

Five-circle control: HP0, AR5/approach1200, OD5, starts2000,2100,2200,2300,2400.
First pre-object safety requires `200-1200r>195`, so61 repeated `.96` reductions
from .05 precede trial62. Each completed object's H caps200 under these controls;
no low-group or final-target retry occurs. Total drainable time is1600ms.

All five explicit groups add `70B+30G`, hence `A=200-1600r+70B+30G`.
The unmarked chart has four ordinary rewards and the final group reward, hence
`A=200-1600r+14B+30G`. Mean `(A-200)/5` must reach W8. Starting at B=G1,
the unmarked mean is below8 for four completed trials; each balance retry applies
`r*=.96,B*=1.02,G*=1.01`. The next trial accepts. The harness checks each of
those intermediate A/mean/snapshot states, rather than only comparing final
outputs copied from the development report. Expected results:

| Groups | Trials | Rate | B | G |
|---|---:|---:|---:|---:|
|All marked|62|.004144911095285019|1|1|
|Unmarked|66|.0035204659802861625|1.08243216|1.04060401|

Private trials leave live ledger score/events/whole results/combo and health
ResultEvents at zero; live H/A/V reset200/200/0. Matched health-enabled/disabled
sessions compare E034 K, score, accuracy, all hit counts, current/max/snapshot
combo, every last event/outcome, object judgements, slider raw checkpoints and
spinner progress/observation count at every explicitly supplied component step.
The historical513-case gate remains the independent broad arithmetic/regression
authority, including QA07 passive/smoothing and C01 H160/H200 checks.

## Preserved preliminary QA08 oracle error

The first executed local preliminary net10 harness returned112 PASS/1 FAIL/113.
I07 expected three misses; actual miss count was2. This was an authored oracle
error, not an R10 production defect. `initial-local-output.txt`,
`local-preflight-results.json` (including exact input and stack/expected/actual),
and `initial-local-harness.sha256` preserve the failed run. No historical QA
assertion or production source was changed.

Exact input: slider2000–2500, spinner4000–4500, circle6000, OD5, no input.
The spinner's500ms preparation at OD5 yields requirementN2. E026's modern grade
tests C against `Max(0,N/4)` with integer division: N2/4=0. C0 is not less
than0; it is not greater thanN or N−2 either, so raw256/50 follows. Therefore
three whole judgements comprise two misses and one50. I07 now asserts all these
specific intermediate/result values. Added R00 controls distinguish C0/N2→256
from C0/N4→−131072. `oracle-correction.md` documents the exact changed assertion.
The subsequent preliminary local run passes115/115; only the hosted net8 run
will be used for the final required Windows gate outcome.

Local build setup initially encountered NU1900 vulnerability-feed access and a
net10 override asset-target mismatch before any cases executed. Explicit restore
and build targeting the already installed net10 SDK, with NuGetAudit=false for
that local preflight only, resolved them. No project/workflow target or SDK policy
was changed: committed project remains net8.0 and hosted gate uses installed8.

## Limitations and disposition

Covered behaviors will be classified VERIFIED COMPONENT after the hosted gate;
no universal osu!stable equivalence follows. Physical input, WPF/audio lifecycle,
native clocks/JIT interleavings, old spinner gameplay, original manager reuse/ties,
editor/alternate factories, live decorations and complete mod policy remain
outside scope. Deferred cross-beatmap score discrepancies remain deferred.
No E037/R11/framework migration or lazer work. Stop after QA08.

# 3. Pseudocode

```text
QA08 independent observation procedure:
    select literal E036 flags / separately derived E035 scalar expectations
    call linked production predicate, preparation, parser, chart or session factory
    inspect factory/parser/final flags and every provenance operand separately
    check strict break postpass against supplied prepared order
    check calibration gap exclusion separately from break flag forcing
    check trial intermediate values against closed equations
    verify private trials emit no live scoring or judgement events
    compare matched health-enabled/disabled actual component event streams
    retain historical APIs and execute the fresh factory as separate contracts
    preserve failed input/output and prove any oracle correction independently
    run unchanged513 historical cases and retained development checks on Windows8
    classify executed finite cases only; retain unsupported and runtime boundaries
```
