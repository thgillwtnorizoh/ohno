# 1. Result

QA05 — independent R07 spinner mechanics/integration regression, Work1,
2026-11-03 (Asia/Bangkok). Target/current origin/main:
`c63d1d36641abea7ac096e63b0468e9552e1f4f8`.
Initial status: **HOSTED EXECUTION PENDING**. Prepared cases are not passes.

66 focused cases link production Beatmap.cs/CircleSession.cs, including actual
SpinnerState/SpinnerProgressObservation. They exclude WPF, entry points, audio,
physical input and clocks. Expected values derive from E24–E26, not R07 development
output. No replacement spinner or schedule simulator is implemented.

The existing Windows workflow is minimally extended with QA05, retaining pinned
checkout and preinstalled .NET8 SDK selection. It compiles WPF win-x64, then runs
unchanged QA01(90), QA03(56), QA04(43), R06 development(4 targets/4 loads), R07
 development(8 groups) and QA05. No SDK installation; no local dotnet available.
Results-only publication does not trigger another gate. Failure diagnostics will
be preserved; Work1 does not change production code.

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa03/Headless/QA03.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R06PreparationChecks/R06PreparationChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa04/Headless/QA04.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R07DevelopmentChecks/R07DevelopmentChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa05/Headless/QA05.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

No game, OS input, OBS, debugger, GUI, virtualization, supervised play or original
binary execution/inspection. E20 remains incomplete, all17 original-runtime
cases NOT RUN. Research, implementation, journals, fixtures, previous QA and
E20 are unchanged. QA05 stops after verification and publication.

# 2. What they found

## Independent contract and coverage

**CONFIRMED STATIC:** E24 M05f6:00d8–017a strict signed wrap,017b–01a5 EMA,
01c4–01e9 stationary handling,0201–02c3 held/time/target gates,02cf previous angle;
M05f5:014e–0173 strict integration eligibility,0178–02b2 smoothing/clamp/Single
step and0371–0395 Single progress store. E25 M05eb conv.i4; M05f6:02db inequality,
02ed C increment, component selectors and04c7 cache. E26 M05f4:01ba–01d4 prepares
N; M05fa modern thresholds, existing strict manager end test and ordinary final
combo consumer. Existing evidence resolves the finite contract; no new binary
excavation or revised original claim is needed.

| Cases | Independent discrimination |
|---|---|
| A01–A16 | atan2(Y,X), center-relative projection, modern reference, both signs/wraps, ±pi rejection and previous update, near-antipode acceptance, no radial threshold, held/time eligibility, stationary decay and negative interval |
| B01–B10 | Numeric EMA updates, fixed/adaptive divisor and zero divisor, acceleration, Single step/progress, clamps, direction reversal, strict integration gates, conditional residual vs immediate stop |
| C01–C07 | Fractional/cache equality, first raw0, skipped integer accounting, 8192/4096 alternation, bonus16384 at qualifying even offsets, no duplicate observation, artificial downward inequality |
| D01–D09 (15 cases) | N/acceleration preparation, OD endpoints, seven individually named N15 grade boundaries, zero/small N, strict end/once, pending observation, same high P/low C, evaluated integration guard |
| E01–E08 | Separate collections/head admission, session component/combo isolation, once-only final accuracy, final failure reset, fresh state, nominal cadence, separate mapped pointer and mixed circle/slider/spinner lifecycle |
| F01–F10 | Three R07 fixture loads, optional metadata, malformed fields, unsupported versions, duration/end equalities, overlap rejection, retained old maps |

N15 is independently derived from E26: Single(3000)/1000f=3f; Double×OD5 anchor5,
then Int32 conversion gives15. The seven cases C=0,2,3,13,14,15,16 expect raw
-131072,-131072,256,256,512,512,1024 respectively. Integer division N/4 gives3,
with strict > for best/middle. These are prepared-count thresholds, not full
revolutions, displayed scores or bonus payouts.

The bounded Spin16 control calls actual angular input and integration methods
with separate supplied intervals (16ms sample and100ms integration), wrapping
finite theta through atan2(sin(i),cos(i)). Smoothing reaches the .05 clamp and
each100ms integration contributes the one-unit cap. After16 integrations:
P16, cached integer15, C15. Complete without another sample gives raw512; an
explicit final observation at exact end increments C16 and yields raw1024.
This is deterministic component sequencing, **not** natural gameplay or a claim
about native callback/render cadence. No whole-spinner algorithm is simulated.

SeedCount deliberately calls the real public Observation.Observe with integers
for threshold/session-isolation tests E03/E08; it does not set Progress or private
fields. These are **artificial component state setups**, not naturally generated
spins. C07's decreasing integer is likewise artificial/reset-like. E02 instead
uses actual session cursor arguments to reach nonzero component observations and
checks that combo/whole-object count remain0. The mixed control plays no windows:
it calls components for circle1000, Linear slider2000–3000, spinner4000–7000,
circle8000 and slider9000–10000. Five whole-object results and combo7 distinguish
component credit from duplicate final accuracy/combo.

## Classification, engineering and limitations

**VERIFIED COMPONENT:** assigned from actual CI output only. Until then assertions
are proposed expectations. Any confirmed discrepancy remains a failing case with
expected/actual output and a Work2 correction recommendation.

**OWNER-REPORTED:** the owner confirms R07 launches and spinner behavior is close
to original in tested scenarios. No identified build/map/settings/trace capture
was supplied here; preserve this positive scoped feedback without upgrading it
to universal equivalence or asking the owner to repeat it.

**ENGINEERING:** R07's mapped displayed center(256,192), explicit nominal sample
qualification, serial callbacks, practical duration1–60000/end<=3600000/v14-only
admission, disjoint intervals including endpoints, immutable fresh-session state,
finite-method defenses and practice score/accuracy. E06's older static reports
or E24 compatibility predicate do not establish that v14 itself selects modern
original policy. Source compatibility remains explicitly unsupported.

Parser sound/end numeric syntax is independently exercised; v14/duration bounds
and interval exclusions are R07 restrictions, not claimed original parser rules.
Optional hit-sample text is retained as data; no playback is invoked. Existing
circle/slider behavior is protected by the unchanged189 QA cases, not counted
as new spinner coverage.

**UNVERIFIED:** original callback timing/multiple passes/provider gates, actual
native operands, x86 arithmetic/exception interleavings, complete score/HP/bonus
payouts, original pixels/audio synchronization and general cross-map equivalence.
Successful headless execution cannot supply these observations. No E20 changes.

# 3. Pseudocode

E24–E26 behavioral model unchanged. No new original-binary evidence is obtained.
QA05 checks the existing modern finite projection with explicit component inputs;
implementation observations are not newly recovered stable behavior.
Stop after QA05; no R08 or further expedition is started.
