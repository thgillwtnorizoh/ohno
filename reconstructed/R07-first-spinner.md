# 1. Result

R07 — First Human-Playable Spinner. Work 2, 2026-11-03 (Asia/Bangkok).
Started clean latest origin/main at `a3f414e1ab220a5d7c62d1bacf4aa7b4850e5985`.
Read AGENTS/full standing contract, reconstruction journal/R05–R06, E24–E26,
relevant E10–E13/E27 boundaries and QA01/03/04 contracts. No original-binary
inspection, debugger, runtime experiment or new expedition.

Implemented v14 Spinner8 parsing, separate angular/motion/progress/observation/
final state, real held Z/X and mapped cursor sampling, modern finite preparation
and strict completion, practice combo/accuracy/results and independent WPF rotor/
progress/held feedback. Spinners never become circle/slider heads. Added selectable
Spinner Practice, Circle + Spinner and Mixed R07 inputs. Existing fixtures/buttons,
slider mechanisms, audio and Stopwatch clock are retained.

Implementation changes are under reconstructed/: Beatmap/CircleSession, renderer,
UI/project/README, three independent silent fixtures, bounded R07DevelopmentChecks,
this report and appended journal. Minimal authorized CI change: one step and its
path trigger in the established Windows gate; all Work 1 research/QA unchanged.
No permanent branch, force push, QA05 or new automated gameplay framework.

**Windows CI PASS** on R07 source commit
`a0a058c12a365f7f6d8b922fa3240dd0bfded006`:
[run41571969274 / job125706717861](https://github.com/thgillwtnorizoh/ohno/actions/runs/41571969274/job/125706717861).
Actual execution2026-11-03 10:24 Asia/Bangkok (03:24 UTC), Windows2022,
preinstalled SDK8.0.427. Release WPF win-x64 compilation succeeded with **0 warnings
/0 errors**; QA01 **90 passed/0 failed**, QA03 **56/0**, QA04 **43/0** (189 existing
QA cases). R06 **4 preparation targets passed/4 fixture loads completed**.
R07 **8 bounded development groups passed**, including three spinner fixture
imports, finite arithmetic/observation, strict completion and parser rejection.
Every step and the job succeeded. Checkout's Node20 advisory is separate from
compiler diagnostics. No local dotnet executable, installation or compilation.

Commands actually run at repository root by the established gate:

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa03/Headless/QA03.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R06PreparationChecks/R06PreparationChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa04/Headless/QA04.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R07DevelopmentChecks/R07DevelopmentChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

The workflow/build-step label retains historical R06 wording; its checkout and
logs identify the initial R07 SHA above. A subsequent source-review refinement
adds nominal angular-sample qualification, with an explicit component check;
that changed source is **PENDING reverification** at its publication. The final
baseline will identify its actual run/outcome separately. Windows instructions
remain in CircleGame/README.md. Source/whitespace/preservation checks passed.

No game window, OS input, playback/recording, OBS, original debugger, E20 run,
virtualization or supervised gameplay. Human R07 play remains **UNVERIFIED**.
QA04's positive scoped owner report about R06 is preserved, not promoted to R07
or universal equivalence. Current R07 authorization continues main implementation
over historical Work 2 suspension/branch rules; no research journal/RE mark edit.
Stop after R07; subsequent independent spinner QA belongs to Work 1.

# 2. What they found

## Research-derived finite model

All mechanisms below are **RECONSTRUCTED** from **CONFIRMED STATIC** E24–E26
instructions. Modern .NET execution is not original-runtime verification.

| Stage / source | Implemented behavior |
|---|---|
| E24 M3201/M05ea | Spinner definition separate from heads; fresh nullable angle absent, target/U/P/C/cache0, interval EMA16.666666666666668 |
| E24 M05f4 | Acceleration `.00008+Max(0,((5000-duration)/1000)/2000)`; duration3000 gives.00108 |
| E26 M05f4/M28b3 | No-mod OD-derived Double anchors3/5/7.5; N=int(Double(Single(duration)/1000f)*rate). OD5/duration3000 gives N15 |
| E24 M05f6 | Single relative vector→Double atan2(Y,X); modern first angle=current theta; strict ±pi wrap tests, Abs(delta)>=pi rejection, previous theta updated after relevant rejection, no radial threshold |
| E24 sample stage | EMA uses Pow(.999,d); exact zero delta first decays old target/3 then zeros on second stationary sample, before held/time checks; nonzero normal delta requires held and inclusive S<=T<=E |
| E24 target | Reject magnitude; EMA>17.333332697550457 uses positive sample interval divisor, otherwise16.666666666666668; retain signed target |
| E24 M05f5 | Only S<T<E with positive update interval; U approaches target by coefficient*interval, clamp[-.05,.05], Single step=U*interval; P=Single(P+Min(1f,Abs(Single(Double(step)/pi)))) |
| E25 M05eb/M05f6 | Observe old stored Single P before integration; truncate to Int32, compare cache by inequality, increment C once on change regardless of skipped integers; select raw then update cache |
| E25 selector | First ordinary C1→raw0; otherwise C>1 even8192/odd4096; C>N+3 with even offset16384. Raw identities are not literal points; no bonus payout/native frequency/effect path |
| E25 consumer | No circle-like combo/whole-object judgement for rotation components |
| E26 M28e4/M05fa | Completion requires T>E, once; no P/cache drain. Modern N0→1024; else C<Max(0,N/4)→-131072; C>N→1024; C>N-2→512; else256 |
| E26 ordinary consequence | Final raw1024/512/256/-131072 maps to practice300/100/50/MISS; positive increments combo, failure resets; one whole-object accuracy contribution |

`SpinnerProgressObservation`, `SpinnerState` and session state keep theta,
interval EMA, target motion, U, Single P, cached integer, observed C, component raw
and final raw distinct. In this serial no-hook model, the second progress read
for cache has the same value as the first; original callback/reentrant/native
effects are not fabricated. Completion itself never invokes Observe. A normal
late input stage can still observe old P once before the later completion stage,
as E26 specifies; there is no special completion flush.

Modern policy is selected because the reconstruction has no historical source
record/Replay. E24's pre-2019-05-10 source compatibility uses first reference0 and
E26's different minimum/best thresholds; that policy is explicitly unsupported.
Format-v14 admission is a slice restriction, not proof that map version selects
the original source-record predicate. No Relax/Auto/mod behavior is inserted.

## Session and presentation engineering boundaries

- Three separate model collections coexist. Spinner stored intervals must be
  disjoint from all other stored object intervals (circles are points), including
  shared endpoints. This is **ENGINEERING admission**, not recovered full candidate
  arbitration. Normal key-down head admission is suppressed during a pending
  started spinner; spinner angular input reads held state every running frame.
  Existing slider head/tracking/checkpoint/aggregate code is not repurposed.
- `Advance` maintains separate previous-update and previous-qualifying-sample
  histories globally, including callbacks before spinner activation. Sample d
  uses prior qualifying time when positive, otherwise the recovered fallback;
  integration gets the separate signed update interval. Angular input qualifies
  only after the recovered nominal16.666666666666668 ms period. This is an explicit
  ENGINEERING approximation of E24's timing-tail gate, avoiding use of the nominal
  divisor on every high-Hz render sample. Integration still runs per frame. Original
  markers/current-candidate/recorded-input gates, repeated clock reads and up to31
  host passes are not reproduced; refresh-rate equivalence remains unverified.
- The actual mouse maps through the same layout as the displayed center(256,192),
  including coordinates outside the stage; the slider pointer retains its prior
  stage policy. E24 reads original F02a3.F2b6e presentation-prepared state, with
  native layout/draw history not reconstructed here. Our center is an explicit
  UI projection, not an invented recovered native coordinate or fallback.
- Renderer uses frozen pens, simple concentric rings/rotor and a best-target bar
  based on observed C vs N+1 (N0 special). Rotor angle consumes signed step as
  independent presentation; it is not a recovered native transform. The body
  persists through exact end equality until completion, then existing feedback
  displays its final judgement. No original skin/shader/effects/pixel claim.
- Pause preserves session state and elapsed clock; resume does not reset motion.
  Restart and map replacement construct fresh spinner state/interval histories.
  Existing staged media replacement, missing-audio handling and close/dispose
  behavior remain. No audio calibration, BASS clock, native spinner sound or
  speculative offsets. Results use last object end+3000 engineering horizon.
- Finite safety policy: v14, duration1–60000 ms, end<=3600000 ms, existing2MB/
  10000-object bounds; malformed numeric sound/end rejects. Optional hit-sample
  text is retained without invented playback/mask restrictions. Coordinates parse
  as before but do not place the spinner. Nonfinite UI pointer skips that sample;
  direct mathematical methods require finite in-range operands. No claims about
  corrupt/nonfinite original conversion, reset/reuse or exceptional native paths.

## Fixtures and bounded development checks

Independently authored silent v14 inputs, OD5: Spinner Practice3000–6000 with N15,
results9000; Circle + Spinner has circle3000, spinner5000–8000, circle10000,
results13000; Mixed R07 has circle3000, quadratic slider5000, spinner10000–13000,
circle15000, results18000. Timing/requirements are source-derived predictions,
not imported original values or successful human plays. No autoplay assistance.

R07DevelopmentChecks links actual components and supplies explicit finite inputs:
three fixture imports/preparation; atan2/first-angle/negative d; signed wraps and
antipodal equality; stationary/held rules; acceleration/clamp/contribution cap/
strict integration/residual U; one-change observation/cache/raw values; final
thresholds/strict end/no drain/fresh session/no head press; malformed sound/end,
old format and overlap rejection. Eight bounded groups, not independent QA05.
The existing189 QA cases and R06 checks are unchanged, not relabeled as spinner
coverage. Actual hosted results are recorded in Result and the journal append.

**UNVERIFIED:** human R07 usability, original layout/center/provider scheduling,
native precision and primitives, callbacks/exceptions, sound/alignment, cross-map
fidelity, full score/HP/bonus/result-record effects. No concrete missing formula
blocks this ordinary finite scope; excluded native helpers are explicit limits,
not guessed constants. Work 1 owns subsequent independent QA.

# 3. Pseudocode

**RECONSTRUCTED — finite modern/no-mod normal-returning projection:**

```text
prepare N from Single duration/1000 then Double OD-derived rate; truncate
prepare coefficient; initialize theta=null, EMA=16.666..., target/U/P/C/cache=0

human-input sample:
    theta=atan2(Single(pointer-center).Y, .X)
    first modern reference=theta
    delta=theta-previous; apply strict signed wrap tests
    update sample-interval EMA
    if delta==0: first stationary target/=3, subsequent target=0
    else: require held and inclusive start/end; reject Abs(delta)>=pi
          target=delta/(adaptive positive interval or recovered fixed divisor)
    previous=theta, including relevant rejection
    if int(P)!=cache: C++ once; select component raw; cache=int(P)
    // Component does not change combo/whole-object result.

later progress stage:
    if start<T<end and updateInterval>0:
        approach signed target by coefficient*interval
        clamp U to +/-0.05; step=Single(U*interval)
        P=Single(P+Min(1f,Abs(Single(Double(step)/pi))))

later completion:
    if pending and T>end:
        grade from C and prepared N using modern strict thresholds
        no P observation/drain here
        record one final practice result; positive combo++, miss resets
```

**ENGINEERING:** WPF mapped displayed center, nominal sample qualification and
one integration opportunity per callback, rotor/bar,
finite validation, session pause/restart/replacement and practice counters.
**UNVERIFIED:** physical gameplay and original-runtime/pixel/audio equivalence.
