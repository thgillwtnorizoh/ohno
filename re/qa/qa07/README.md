# 1. Result

QA07 — independent R09 health verification, Work 1, 2026-11-03 (Asia/Bangkok).
Production target/current main at intake: `03aceee58438d4a5d394314fc2cd53cc37c85f8c`.
Only QA07 files, minimal existing CI configuration and an append-only research
journal entry are changed. Work 2 application code, all earlier assertions,
fixtures, research and the complete E20 packet are preserved.

**Executed outcome:157 PASS,2 FAIL,159 QA07 cases. One confirmed defect,
QA07-D01, with two recovery-threshold reproductions.** All354 earlier independent
cases and all requested development checks pass. WPF Release win-x64 compilation
succeeds with0 warnings/0 errors. Production is unchanged; this is a retained red
regression gate awaiting an owner-coordinated Work2 correction.

[Expanded Windows run41671715769/job126041531509](https://github.com/thgillwtnorizoh/ohno/actions/runs/41671715769/job/126041531509)
executed source `8185478d4a1e064a3e74dbee82c8dbc8907b9187`, 2026-11-03
16:12–16:13 UTC, Windows2022/preinstalled .NET8.0.427. Its only failed step is QA07.
The first gate at `ed2516b2ce7f1282d8dc9fabb8b68f3409680872`,
[run41671435814/job126040543938](https://github.com/thgillwtnorizoh/ohno/actions/runs/41671435814/job/126040543938),
passed157 cases. The concrete guard contradiction discovered while reviewing
retained E035 evidence justified expanding that gate by two cases. Both outputs
are retained. There was no corrected oracle or weakened expectation.

| Executed gate | Result |
|---|---|
| WPF win-x64 compilation |PASS,0 warnings/0 errors |
| QA01 /QA03 /QA04 /QA05 /QA06 |90/56/43/66/99 PASS =354 historical independent cases |
| QA07 |157 PASS,2 FAIL,159 executed |
| R06 preparation /fixture loads |4/4 PASS |
| R07 /R08 /C01 /C02 /R09 development groups |8/13/4/14/14 PASS, separate from independent QA |

Final report publication changes documentation/results/journal only relative to
the expanded tested source. Harness, production and workflow remain identical to
that CI commit; no redundant rerun is claimed for documentation. `results.json`
records tested commit identities. The exact final publication SHA and verified
remote tree are supplied in the handoff, avoiding self-referential commit content.

Reproduction on the existing Windows runner, with its preinstalled .NET8 SDK:

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa07/Headless/QA07.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

The retained workflow also executes unchanged QA01/03/04/05/06 and the R06/R07/
R08/C01/C02/R09 development programs. No SDK installation, game launch, application
window, OS input, OBS, debugger, owner attendance or E20 execution is involved.
The QA01 historical R02 source fingerprints are not represented as R09 tests.
E20 remains incomplete: all17 original-runtime cases **NOT RUN — WINDOWS HOST
REQUIRED**. No new human gameplay evidence was obtained.

# 2. What they found

## Authority, production binding and finite coverage

**CONFIRMED STATIC:** E035's retained instruction evidence, supplemented by E019
failure and E028 pause ordering. No new original-binary excavation or runtime
execution. **RECONSTRUCTED:** the E035 interpretation and independently calculated
finite expected values. **VERIFIED COMPONENT:** only actual production methods
executed in this gate. **ENGINEERING:** private calibration, finite safety guards,
WPF/Stopwatch scheduling, prepared-candidate/active-spinner projections, explicit
component recovery inputs and practice continuation after a latch. **UNVERIFIED:**
original callback/clock operands, native audio, physical input, visible health,
complete policy/mod coverage and general chart equivalence.

The headless project links production `Gameplay/Beatmap.cs` and
`Gameplay/CircleSession.cs`. It contains no replacement health engine or gameplay
scheduler. Tests use explicit method arguments, finite prepared controls and small
private `.osu` files, never generated OS events. The field mutation/delta oracle
is literal E035 evidence plus independently shown arithmetic; the calibration
oracle is selected hand-derived intermediate states, not the production Phi or
calibrator used to predict itself. Source hashes, all case names, commands,
counts and exact CI identities are recorded in `results.json`.

| Cases | Independent discrimination / authority |
|---|---|
| H01–H10 (14) | H/A/V defaults and selective reset, H01–H05 E035 controls, uncapped A, subtraction without upper cap, signed additions, zero-loss fill guard, finite extrema; M2979/M297a/M297b/M297d |
| C01–C21 (31) |62-trial anchor plus rate/gains/snapshots/balance, five HP safety anchors, component counts, next-NewCombo group selection, all retry families, nonconsecutive low groups, E/U/W equalities, old/modern single-break subtraction and explicit rejection; M1150 |
| J01–J10 (45) |12 plain raw classes at HP0/5/10 and G2.5, intermediate HP, separate B/G calibration1028, live-decoration rejection, record-disabled HP hook, terminal/interior combo distinction, component/final spinner rewards and callback interruption; M0ad2 |
| D01–D10 (27) |signed Int32 subtraction before Double, overflow probe, no dt cap, quarter drain, prior eligibility, pause/resume, distinct host/gameplay sources,14 eligibility discriminators and inclusive break queue/marker boundaries; M1146/M226b/M226f/M2271 |
| V01–V08/F01–F13 (26) |fill/time gates, exponential rise/fall and overshoot, H versus V failure, suppression, heal/loss before failure, no invented passive callback, recovery ordering/release and the reported H>=160 discrepancy; M2972/M226c/M0acb/M0acc |
| I01–I16 (16) |real parser→calibration→session state, preparation identities, no trial score events, M2902-unavailable isolation, fresh restart/map replacement, once-only emissions and per-step enabled/disabled scoring/judgement comparison |

New case count159 is independent QA07, not the354 historical cases or development
groups. Repeating a gate does not increase unique coverage.

## Independently derived calibration operands

- HP0 circle S=E2000, approach1200: safety is `200-1200r>195`, hence
  `r<5/1200`. Repeated `.96` reductions from `.05` leave trial61 below safety;
  rate after61 reductions is approximately `.004144911095285020`. Trial62 accepts:
  preH195.026106685658, final calibration1028 adds20, H/snapshot200,
  A215.026106685658, mean15.026106685658, B/G1. Live reset independently yields
  H200/A200/V0 and no result/score events.
- C03's approach100/450/800/1800/2800 at HP0/2.5/5/7.5/10 leaves H exactly
  195/177.5/160/110/60, respectively. Equality takes the early `.96` retry.
- C06 slider duration100, subset1 and schedule3: loss5, three raw64 gains12,
  one raw8 gain3 and group gain20 give uncapped A230, H200. C07 spinner duration100
  with prepared N4 gives A `200-5+4*1.7+20=221.8`; no live observation counter is used.
- C08 next-group false gives first whole6, gap5 and last group20: A221; true gives
  first20 instead: A235. Those histories have equal final capped H200.
- C11 HP10: duration3000 slider, then circle, duration1000 slider, circle and
  duration900 slider, each separated by1ms. Post-object H values are
  78,97.95,75.9,95.85,78.8. The third **nonconsecutive** H<80 group retries with
  B1.07/G1.03 and unchanged r.05; the final snapshot remains uncommitted.
- C19 HP5 slider duration960 gives H180: final-target equality passes, then balance
  retries. Duration961 gives H179.95 and the earlier final-target retry. C20
  distinguishes group H80 from H<80. C21 HP10 circle approach400 gives mean0 and
  accepts; approach401 gives mean−.05 and balance retry.
- Modern break exclusion includes its leading gap; format7 excludes only duration.
  Two breaks in one gap consume only one. No union or synthetic catch-up is used.

All are finite component or static predictions, not imported original-Windows data.

## QA07-D01 — recovery fill writes escape the original H<160 guard

**Classification:** confirmed production component discrepancy against retained
original instructions. Limited to direct finite recovery entry at H>=160; normal
`CheckFailure` returns for positive H, and the no-mod UI supplies zero recoveries.
This is **not** evidence of a normal-play health failure, native failure-screen
problem or need to reopen scoring.

Affected component: `OrdinaryHealth.BeginRecovery`, production
`reconstructed/CircleGame/Gameplay/CircleSession.cs:952–954`.
Minimal deterministic reproduction (actual production calls):

```csharp
h.State.Assign(160); // also tested separately at200
h.State.ConfigureFill(123, .03, false);
h.SupplyRecoveryCount(1);
h.BeginRecovery();
```

| State | Expected | Actual (captured gate) |
|---|---|---|
| H/A |160/160 (or200/200) |unchanged |
| Fill |false retained |true |
| FillRate |.03 retained |.07999999821186066 |
| FillFrom |123 |123 |
| Recovering / remaining count |true /0 |true /0 |

**CONFIRMED STATIC:** E035 `methods[0x6000acc]`, owner T0186/02000186,
MethodDef06000acc signature20010102, body SHA256
`a4e4cbd7c8f1050a3fa5bdd984e8a781042b9d728b4058ef7f3f26c369a3e523`.
CIL0014 `bge.un.s ->006a` skips the entire addition/fill/resource arm.
M297b call0025, F1852 store0031 and F1854 store0045 all precede destination006a.
F1376 store006e and remaining recovery-count decrement00a1 follow that guard.
Original fill flag/slope changes are conditional together with the addition.
`original-recovery-guard.json` retains only these relevant existing evidence sites
and its source hash; no new full disassembly or proprietary bytes are published.

R09 line952 conditions only `State.Add`; line953 unconditionally configures fill.
Smallest Work2 correction: include the fill configuration in the same H<160 arm,
leaving recovery activation/count consumption afterward. Retain F13 equality and
above-threshold tests. Work1 has not implemented this correction. Changed fill
mode/rate can alter subsequent V smoothing under this explicit direct API probe.
Natural reachability beyond normal H0 failure entry remains unestablished.

## Unsupported cases and limits

M2902 remains unresolved. Following circle/slider rows without proved bit4 cannot
silently be treated as non-NewCombo: health is explicitly unavailable, while score,
accuracy, combo and object judgement remain functional. First-object unknown flags
are unused by the calibration walk and single-object controls can initialize HP.
Session restart/replacement is fresh; unsupported health cannot leak coefficients
or failure state into later supported maps. No arbitrary universal rate is tested
or endorsed. Engineering ordering/break/finite-work guards reject unsupported
calibration, not the already supported gameplay/scoring path.

Live decorated notifications are deliberately unsupported by R09. QA07 verifies
explicit rejection without mutation; calibration1028 uses a separate private sink.
The bounded result hook still applies HP when score accounting is disabled.
Terminal slider failure reduces HP but preserves combo through its distinct branch;
SuddenDeath configuration is outside R09's no-mod support and is not invented by
these tests. Passive drain reaching H0 is not a claim that an original per-frame
ordinary failure callback exists. The wrapper uses old eligibility before its late
store; break/pause tests are explicit component operands, not WPF scheduling proof.

The prior owner-reported score discrepancies remain deferred. E033/E034 formulas
and historical QA expectations are unchanged. No universal score, visual, original
HP calibration, audio synchronization or physical-input equivalence follows.

# 3. Pseudocode

E035/E019/E028 original behavioral models are unchanged. QA07 adds executable
component assertions, not new original-software discoveries. The required recovery
correction follows their already retained guard:

```text
RECONSTRUCTED bounded recovery(false):
    if original H is ordered less than160:
        add160 using original H/A rules
        fill=true; fillRate=0.07999999821186066
        retain original presentation helper boundaries
    recovering=true
    retain original playback and callback boundaries
    consume one recovery count
```

```text
independent QA observation procedure:
    bind actual production model/session sources
    compare finite state writes to literal E035 expectations
    compare selected calibration checkpoints to independent arithmetic
    verify calibration never emits live score/accuracy/combo notifications
    exercise raw event -> existing combo -> HP -> score -> failure ordering
    exercise prior eligibility -> signed drain -> V smoothing -> late eligibility
    compare matched enabled/disabled HP sessions after every explicit update
    retain each failure and stop for Work2 correction; do not weaken its oracle
```

Smallest missing original-Windows dependency: an authorized trace of actual
calibration and a reached judgement/drain/failure operand sequence. E20's existing
packet remains untouched; QA07 does not initiate that campaign. Next action is the
owner-coordinated Work2 D01 correction and targeted regression, not E036 or R10.
