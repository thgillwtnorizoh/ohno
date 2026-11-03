# 1. Result

R09 — First Original-Derived Health System. Work2, 2026-11-03 Asia/Bangkok.
Started verified latest origin/main `64bec143982ef7ceadb211db9208e1324074e2fa`.
Read AGENTS/full standing contract (repository/attachment same SHA256bf9952ee…),
reconstruction journal/R07/R08/C01/C02, QA06 and E035 in full, and relevant
E019/E024–029/E033–034 boundaries. No original archive/binary inspection,
research tools, game/window, OS input, debugger, recording or E20 execution.

Added separate WPF-independent health state/calibration/eligibility/failure/runtime
classes in the historically linked Gameplay/CircleSession source. Work1 projects
continue linking actual production components unchanged. Parser preserves health
source identity/type/break rows separately from E034 metadata; no score preparation
formula changes. Supported chart calibration supplies r/B/G/snapshots, followed by
live reset200/200/0. Actual judgement HP runs after combo/before late score write;
reached failure check follows record continuation. After judgement updates, bounded
passive drain and V smoothing consume prior eligibility, then eligibility updates.

Added a small visible V bar/H marker and H/V/A/r/B/G diagnostics, Health R09 fixture
button and independent mixed practice map with explicit groups and a break. Failure
latch is visible without inventing native departure. Old controls/audio/scoring/
fixtures remain. New14 bounded development groups and one minimal existing Windows
CI step/path. All historical reports, research, independent QA01–QA06 and E20 are
preserved. Journal is append-only; no permanent branch or force update.

Actual tested source: `ebb00b6a91df976271923f6bda3c495336af5678`.
[Windows run41668909283/job126031648587](https://github.com/thgillwtnorizoh/ohno/actions/runs/41668909283/job/126031648587)
passed on2026-11-03 15:55–15:56 UTC /22:55–22:56 Asia/Bangkok,
Windows2022/preinstalled .NET8.0.427. Actual output:

```text
Build succeeded. 0 Warning(s), 0 Error(s)
HEADLESS COMPONENT TESTS: 90 passed, 0 failed, 90 executed
QA03 SLIDER TESTS: 56 passed, 0 failed, 56 executed
R06 PREPARATION TARGETS: 4 passed (static research oracle, not original runtime)
R06 FIXTURE LOADS: 4 completed (parser/preparation only)
QA04 GEOMETRY TESTS: 43 passed, 0 failed, 43 executed
R07 DEVELOPMENT CHECKS: 8 passed (finite static-oracle/component checks; no human gameplay)
QA05 SPINNER TESTS: 66 passed, 0 failed, 66 executed
R08 DEVELOPMENT CHECKS: 13 passed (E33 finite controls; no original runtime/human play)
R08-C01 DEVELOPMENT CHECKS: 4 passed (component calls only; no game window or OS input)
R08-C02 MULTIPLIER CHECKS: 14 passed (finite static predictions, no original runtime/human gameplay)
QA06 ACCOUNTING TESTS: 99 passed, 0 failed, 99 executed
R09 HEALTH CHECKS: 14 passed (E035 finite component controls; no original runtime or physical gameplay)
```

All job steps succeeded; checkout Node20 advisory is distinct from compiler
warnings. No prior assertion was changed or weakened, no failed gate hidden.
The retained workflow runs the existing build/QA/R06–C02 commands and adds:

```powershell
dotnet run --project reconstructed/R09DevelopmentChecks/R09DevelopmentChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

Final documentation-only publication updates this report and appends the journal
over the tested source; application/checks/workflow remain byte-identical. Source/
diff/whitespace and preservation checks passed. No local dotnet SDK/install/build.
These are executed component results, not human/original-runtime observations.
Current assignment authorizes implementation-only records/main integration, not an
original expedition or shared research edit. Stop after R09, no R10 or QA07.

# 2. What they found

## Source-derived components and ordering

E035's static instruction evidence is CONFIRMED STATIC; these independent finite
components are RECONSTRUCTED. Engineering substitutions and limits are distinct.

| Original-static authority | Operation sites (hex CIL-relative; E035/E019/E027) |
|---|---|
| M297d |H000a, A0019, V0028 selective reset |
| M2979 / M297a / M297b |H/A001e/0025; subtract A/H002d/0049; add A/H001f/0049 |
| M1150 |thresholds003a/0064/008e/00b8; gap01d3; safety0217; components026c–0313; snapshots03ec; end0409–044d; mean0452–0474; balance0479–04b4 |
| M1146 / M226b |r/clock/spinner inputs0030–003b; subtract0052; wrapper09da before late eligibility0f0b |
| M0ad2 |HP dispatch12df, add12f2/subtract1301, legacy write1394–13a2, fail call1404 |
| M2972 |V/H reads01ee/01f4; fill0248/024f; rise02ab–02e3; fall02fa–0337 |
| M226c / M0acb / M0acc |H check0029; recovery before latch0060; recovery addition0025/fill slope0045 |
| M2243 / M2271 / M226f |selected end queue017f; active marker000d/dequeue007c/reset00a8; inclusive queue compare001f |

Original selected-body SHA256 pins from Work1: M1150
`389f0e15e00771308e8191e65444c89c55c571137ede52c5d60bcd25a7671039`,
M1146 `4b560e5164207973fcf773a1a50326f2799895b2e0fb2f4cc3758dbe11138e9f`,
M2972 `a82b8964101ca24825347eddb2ea4dabab1d708868bcddcdb33c0b5b2defd72e`.
No new binary extraction/checker was run by Work2.


- HealthState retains separate Double H(F1857), V(F1856), A(F1858), r(F1855), B/G
  (F0603/04), fill/from/slope. Fresh component H/V/A default0, filltrue/slope.02.
  M297d resets only H200,A200,V0 in that order. M2979 assigns clamped H then raw A;
  M297a floors A−x then H−x, with no upper clamp; M297b adds uncapped A then clamps
  H+x. Both delta methods preserve the supplied F1374/fill guard, including x0.
  SetPresentation changes V only. No setter automatically tests failure.
- Plain HP table is E035-E: raw1024:+6G;512:G*Phi(17.6,2.2,2.2);256:G*Phi(3.2,.4,.4);
  whole−131072:Phi(−6,−25,−40);64/128:+4G;8:+3G;−262144/−524288:Phi(−4,−15,−28);
  4096/8192:+1.7G;16384:+2G;raw0 no mutation/check. Phi preserves the no-mod HP
  piecewise Double operations. Trial1028 adds14B+6G only through a separate sink.
  Decorated live outcomes and modifiers remain unsupported, not normalized.
- E033 accounting is retained. An optional normally-returning HP hook at the
  original-derived M0ad2 post-combo/pre-record site preserves mutation ordering;
  all old public overloads remain. Session emissions carry their actual consumed
  result time, raw and existing source/slot/producer metadata. Health is not gated
  by the record counter flag. Raw0 and unprocessed backlog do not gain HP/events.
  Spinner raw notifications occur only on actual observations. Failure follows
  record continuation; head/terminal/parent remain separate. No score/accuracy/
  combo formula is replaced or calibrated from health.

## Preparation, source admission and calibration

HealthMapSource retains file-order starts/raw types and ordered numeric-event2
break pairs from the same captured file as gameplay. HealthChart matches them to
actual prepared definitions, retaining original order and end/count identities;
nonempty count consistency and ordered/nonoverlapping endpoints are required.
Nonsupported health leaves existing gameplay/scoring usable with an explicit
HealthStatus reason. This is rejection of automatic HP, not fallback coefficients.

**Concrete Work1 dependency:** E04/contexts still say M2902 is opaque. A circle/
slider's NewCombo is M2902 OR file bit4, so false file bits cannot universally be
promoted to false prepared NewCombo. FromMap admits following circles/sliders only
when bit4 proves true; first object's flag is not used by the calibration walk.
Spinner's separate parser-set flag is retained (old<=8 constructor condition is
recorded, while existing spinner gameplay remains v14). The original first-circle
fixture therefore has unavailable automatic HP, without losing play/scoring.
Single-object controls, Mixed R07 and new explicit-group R09 are supported.
No new M2902 investigation was conducted. Work1 can later supply that operand.

E21 M144a:150e–1551 reconstructs F0bf7 subset count from h=N/R and `(i+1)%h==0`
for i<N−1,h>0, rather than counting UI Repeat labels. E035 calibration uses a=
subset+2 raw64 and b=schedule−subset−1 raw8. Spinner trials consume existing E026
prepared N raw8192 times. No simulated trial touches the live ledger or gameplay
result state. E012 establishes next M3e56 as prepared NewCombo; group trials send
raw1028 at last/next-NewCombo, otherwise1024.

M1150 projection: initial r.05, B/G1 only at fresh calibrator construction;
thresholds L/U/E/W use E035's four Phi endpoint sets. Each trial resets private
H/A/V, starts previousEnd=first.start−approach, consumes at most one fully contained
break per gap, subtracts gap then duration loss, applies prepared components,
preserves overflow safety, group/end/balance retries and exact multipliers .96,
1.07/1.03, .94/1.01, .96/1.02/1.01. B/G persist across retries. Even retry paths
compute mean(A−200)/count. Completed iterations update object-indexed HP snapshots;
untouched suffixes retain earlier values. An engineering NaN sentinel marks never-
written snapshots during diagnostic trials, never an original object assignment;
accepted results have every snapshot written. Live health resets200/200/0 after
calibration, preserving coefficients/fill. No seek restoration is implemented.

The independent HP0 circle2000/approach1200 control requires r<5/1200: .96^60 still
fails, .96^61 passes. Checks assert62 trials,61 early reductions and B/G1, then
trial A=220−1200r versus live A200. These are reconstructed finite predictions.

Original M1150 has no fixed retry cap. ENGINEERING guards10000 trials/2000000
object-component work units and chart limits10000 objects/4096 breaks reject
exhaustion explicitly, with no universal rate/gain fallback. Direct controls test
all retry classes, coefficient retention and partial snapshot behavior. Prepared
count/group/identity gaps, unsupported ordering/overlap/breaks or missing synthetic
source metadata likewise make automatic health unavailable. Failure to prepare
is not hidden by assigning r0 or B=G1 to an active health component.

## Drain, breaks, host and smoothing

E035 M1146 consumes *previous stored* eligible, computes r/1 times Double of signed
unchecked T−previousT, then selected-spinner .25f or1f and calls M297a. Zero dt
still reaches the mutation; backward operands can raise H/A above200. No clamp/
proration/cap/fail callback follows drain. E035 M0ac4 places this after result/
object updates and before V smoothing. Pause suppresses both through the wrapper;
component tests supply explicit paused frames, WPF stops invoking Advance when
paused. Resume uses frozen Stopwatch time and resets host-render interval history.

HealthEligibility preserves supplied special/recorded-time gates, inclusive lower
first−approach/upper last-indexed-end bounds, marker/clock and E027 M226f queue
comparison (T<=Peek, otherwise format<8). Queue construction uses E035 M2243/
M28dc midpoint stub/end comparison in a strictly ordered end domain. Breaks must
be positive/ordered/contained in inter-object gaps; no overlap union or clipping.
E027 clarifies M2271: only while F1b97 active does it set marker=T and dequeue
Peek<T. Equality remains. Candidate start<=T and>=marker can clear the marker,
independent of judged state. Inclusive break endpoints are retained.

**ENGINEERING host substitutions**, visible as bounded-host provenance: current
Stopwatch times substitute for separately consumed original fields; WPF rendering
delta/nominal60Hz supplies host F2c22-like scale, with separate host elapsed; normal
clock state1, no special/recorded flags, running-state true substitutes for F1374;
pre-evaluation active-spinner interval substitutes for old-candidate current
selection; full prepared starts substitute for F17fd in marker clearing. Original
candidate/provider/interruption/native BASS clocks are not reproduced. Fill setup
uses explicit map F0fc4 operand0 as engineering input (its wider setting unresolved),
retaining the recovered first-start/.02/from adjustment without inventing its name.
Zero/nonpositive fill denominator rejects supported automatic initialization.

M2972 smoothing is separate: fill V increase requires from<T and recovery or
clock1, uses corrected elapsed normally/host elapsed in recovery; ordinary rise
uses Abs(H−V)/4*hostScale; fall clears fill and uses Abs(V−H)/6*hostScale. Only
original200/0 endpoint clamps are applied; V can overshoot H. H/A are unaffected.
Native resources, width conversions, pixels, shaders and audio are not reproduced.

## Failure/recovery and remaining scope

E019/E035 M226c uses fresh H after a reached result; positive/unordered returns,
not V/A. Explicit gates can suppress continuation. Recovery count is tested before
already-failed latch; supported recovery adds160 if H<160, sets fill/.07999999821186066,
decrements count and releases strictly when V>H−5. Component API accepts recovery
counts explicitly, but ordinary no-mod UI uses0. Native playback toggling/held
clock and remaining recovery callbacks are not emulated. First failure captures a
latch/time diagnostic after the supported decision, while original candidate
cleanup/queue/departure is unresolved. Practice continues rather than inventing a
fail/retry screen or affecting scoring. State reset does not reset unrelated latch.

Live raw decorations/group bonuses remain an E033 policy boundary; calibration's
explicit1028 does not prove every live whole300 is decorated. The current engine
emits its existing plain results and HP consumes them honestly. Full HP/mod/fail/
Rank/replay/seek restoration, native scheduler/precision/callbacks, complete map
coverage, human bar/physical input and original equivalence remain UNVERIFIED or
excluded. Deferred owner score discrepancies are not investigated or patched.

## Bounded verification and use

R09DevelopmentChecks links production model/ledger and invokes direct finite
values: initialization/reset/HVA/clamps; all plain delta classes at HP0/5/10;
trial1028/raw0; complete independent calibration, early/equality/overflow/group/
end/balance retry controls, retained coefficients/snapshots/guards; modern/legacy
one-break exclusion; signed/zero/spinner drain, prior eligibility/pause; explicit
queue/marker/provider boundaries; proportional/fill/sources/overshoot; suppression/
latch/recovery/release; parsed fixture/restart/replacement/unsupported grouping;
once-only actual session emissions and per-step score/accuracy/combo invariance;
record-disabled HP hook ordering and terminal/interior failure distinction.
Fourteen bounded groups, not QA07 or an original calibration experiment. Synthetic
component inputs are not physical play or an autoplay mode in the application.

Choose Health R09 through existing UI. Aim/press/hold with mouse and Z/X; Space
pause, R/Enter restart. Header/footer explain unavailable HP for other maps. All
old fixtures and their source bytes remain unchanged; new fixture adds no audio/
original assets. Windows launcher/build instructions remain in CircleGame/README.

# 3. Pseudocode

```text
RECONSTRUCTED preparation:
    require supported prepared identities/order/count/groups/breaks
    rate=.05; B=G=1 at fresh construction
    repeat private trial:
        H=200; A=200; V=0; previousEnd=first.start-approach
        for each prepared object:
            consume at most one contained break; subtract gap/drain and duration
            apply trial slider64/8 or spinner8192 counts
            preserve pre/duration safety and retry rate*.96
            trial whole1028 at group end, else1024
            third low group -> B*=1.07,G*=1.03; retry
            write completed object's HP snapshot
        final low H -> rate*=.94,B/G*=1.01; retry
        mean=(A-200)/count even when already retrying
        low mean -> rate*=.96,B*=1.02,G*=1.01; retry
    publish rate/B/G; live reset H200/A200/V0 without coefficient/fill reset

RECONSTRUCTED runtime with explicit ENGINEERING host inputs:
    actual once-only raw result -> existing counters/base/delta/combo
    apply its HP delta -> existing admitted score -> reached H failure decision
    after object judgements:
        if old eligible: subtract rate*Double(unchecked(T-previousT))*spinnerFactor
        smooth V using distinct fill/gameplay/host operands
        recovery release only if V>H-5
        update marker/queue and store late eligibility for next invocation
    pause -> suppress wrapper, retain values; restart -> fresh calibrated state

ENGINEERING presentation/lifecycle:
    draw V bar and H marker; expose A/r/B/G and unsupported reasons
    failed latch -> diagnostic; no invented native cleanup/departure
    guard exhaustion/unknown preparation -> explicitly unavailable HP, normal score intact
```
