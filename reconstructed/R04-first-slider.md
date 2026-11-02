# 1. Result

R04 — First Human-Playable Slider. Work 2, 2026-11-02 (Asia/Bangkok).
Starting verified `origin/main`: `9ab59fbe4c7b6f68875cb755f50959f00f308187`.
Read canonical rules, full standing contract, journal/R02/R03 reports and relevant
E05–E06/E11–E18/E21–E23 research. No original binary investigation or execution.

Source implements mixed circles and one-segment Linear, one-span, zero-interior-
tick sliders: timing/SV-derived duration and geometry, head press, live held Z/X
and mouse sampling, entry/loss/re-entry, terminal checkpoint and parent aggregate.
Primitive body/ball/follow-ring rendering remains after head judgement. A new
Slider fixture button loads independently authored `Fixtures/first-slider.osu`.
The earlier silent/audible controls, audio backend and practice clock remain.

CI state at initial source publication: **PENDING**, not a pass. The final R04
entry will record the actual run/job/commit, compiler output and unchanged 90-case
suite result after hosted execution. No local SDK installation or game launch.

The existing QA02 workflow triggers only on changes to its own file. It and all
Work 1 QA assets are preserved. The small companion `.github/workflows/r04-build.yml`
uses the same Windows2022/preinstalled .NET8 selection, WPF win-x64 build and
unchanged QA01 headless suite. It adds no tests, OS input, media playback or game
launch. This CI-enabling infrastructure is the only added path outside
`reconstructed/`, necessary for the explicitly requested noninteractive gate.
Application source/docs/fixture and implementation journal stay in reconstructed/.
No permanent branch, force push, original-runtime experiment or separate QA
campaign. R01 and historical reports/evidence remain untouched.

Human operation is **UNVERIFIED**: no game window, mouse/key actions, desktop
recording, audio listening, OBS, CDB or virtualization. A compiler/regression pass
does not establish successful human slider play. Stop after R04.

# 2. What they found

CONFIRMED STATIC research is translated as scoped RECONSTRUCTED behavior; this
milestone supplies no new original-code or original-runtime evidence.

| Implementation | Research basis / explicit boundary |
|---|---|
| Type2 fields, `L|x:y`, count1, pixel length | E05/E06; all other path/count constructions explicitly rejected |
| Timing selection | E05 M1be3 truncated-time rate lookup and M1be4 raw-time spacing lookup, each preserving list order and fallback index0 |
| Rate | A=(100*multiplier)/tickRate; effective beat includes inherited negative-length clamp; V=(A*tickRate)*(1000/E) for E>0, else A*tickRate |
| Straight preparation | E06 trim/extension of one finite segment to requested length; cumulative length is its prepared Single distance |
| No interior ticks | E16 first spacing candidate and .01*V near-end suppression; reject any surviving interior Add instead of dropping it |
| Movement/end | E05/E15 Single1000*distance then double division, int conversion of absolute end; one integer Movement record |
| Terminal | E16/E22 final-only Max(S+(E-S)/2, originalEndComponent-36), separate from full parent end |
| Head | E11–E15 inclusive admission disk/time range, strict circle judgement widths; own head state, not parent completion |
| Held input | E15 logical left/right/Both and two change-history masks, remembered side and new-edge choice; app publishes real WPF input, no Relax branch |
| Follow | E15 strict distance<R², inactive R and active R*2.4000000953674316; one Movement query, late fallback extrapolates; no head-success prerequisite |
| Entry/component/exit | E15/E16 entry records T; due terminal succeeds only with followOK and entry<=deadline; leave on loss while terminal pending |
| Parent | E17 tracking then separate head timeout, inclusive T>=end; no forced backlog drain or terminal repair |
| Aggregate | E17/E23 legacy null-accumulator policy, no mods/ScoreV2: (positiveHead+terminalSuccess)/2; head-sensitive alternate not selected |
| Combo/feedback | E18/E21 positive head/component increments; E22 terminal negative suppresses reset; full score/HP/record pipeline excluded |

## Fixture and player-visible behavior

The fixture contains circle3000, slider5000, circle8000, circle10000. Slider
head(160,192), control endpoint(320,192), requested/prepared160, one span;
base beat length2000, multiplier2, tick rate1 give A200/V100, end6600,
only terminal6564. These are source-derived inputs/predictions, not an executed
original import or a human-play result. It is silent; R03 audio fixtures and local
audio loading remain independently usable.

Head press does not set tracking or complete the parent. Holding before/after
the head can establish follow independently. Loss before the terminal shrinks
the next entry radius; reacquisition updates entry time. Since no interior ticks
exist, a middle release is not automatically a failed slider: a timely re-entry
can still earn the terminal. Acquisition after the deadline cannot rescue it.
After a terminal decision, E22 does not require clearing the active flag; live
eligibility is displayed separately. Completion at end grades one whole slider.

Legacy grading gives300 for both successes,100 for one, MISS for none. Positive
head50 plus terminal success can yield aggregate300. This is the deliberately
selected no-ScoreV2 policy, not a claim about every original installation. Whole
object accuracy is separate from component combo feedback; aggregate display
adds no duplicate combo point. Stable scoring/HP/effects are not claimed.

## Engineering choices and limitations

- R02's simple full-list attention, earlier-object admission approximation and
  frame-clock snapshots remain. Input polling/events occur on the WPF UI thread,
  not the original host scheduler. The app maps physical Z/X to logical1/2,
  preserving the researched history predicate while not reproducing OS/provider
  polling cadence, original mask numeric values or mouse-button bindings.
- Every running frame samples actual mouse coordinates and held Z/X; key-down
  still requires a real edge for a head. Event changes also publish held history.
  Focus loss clears published held state and pauses; explicit resume/restart use
  the existing policies. No input is generated by Work or by an autoplay engine.
- R04 supports exactly one endpoint and one span, with no scheduled interior Add.
  It rejects curves, multiple controls, repeats, overlapping slider intervals,
  degenerate/nonfinite geometry, unsupported lengths/durations and ticked paths.
  Timing points require finite values and positive base beat lengths; inherited
  nonnegative values normalize to-100 per E05. This is a bounded parser, not
  stable's complete forgiving/old-map/stacking/offset behavior.
- CircleState, SliderState.Head, tracking and terminal state, and parent Result
  are separate. Required tracking position is distinct from the visible clamped
  ball after its Movement interval. Independent WPF capsules and circles replace
  skin meshes/textures/snaking/hit animations. No pixel or x86 floating identity.
- R03 BeatmapAudio, Stopwatch authority and local media behavior are retained.
  No audio-position correction, calibration or original BASS equivalence.
- No new slider automated tests or QA corpus were created. The required existing
  90 cases cover circle regressions, not slider gameplay. New slider behavior and
  actual Windows input/render/audio/latency remain for Work 1 QA and human use.

# 3. Pseudocode

RECONSTRUCTED finite research rules, with ENGINEERING host/UI boundaries:

```text
load supported slider:
    parse L endpoint, one span, pixel length and timing points
    select inherited/base rate and spacing separately
    trim/extend line; reject interior checkpoint schedule
    compute Movement end and separate adjusted terminal deadline

physical Z/X edge at head:
    shared admission and strict timing classification -> head result/feedback
    remember logical side; leave parent pending

live frame at/after start, parent pending:
    evaluate held-side/history predicate and strict ball follow disk
    if newly eligible: entryTime=T; tracking=true
    if terminal due and undecided:
        success = followOK and entryTime<=terminalTime
        record terminal feedback; success increments component combo
    if follow lost and terminal still pending: tracking=false
    separately timeout missing head when T>S+W50
    if T>=parent.end:
        grade legacy ratio (terminalSuccess + positiveHead)/2 exactly once
        display whole-object result; no duplicate aggregate combo increment

draw:
    path/body persists while parent pending, even after head result
    draw independently scheduled head/approach, moving ball and follow feedback
```

ENGINEERING: WPF rendering, real-input sampling cadence, practice counters,
results overlay and supported-input validation. UNVERIFIED: real human play,
pixel/audio fidelity, original scheduler/helper interleavings and native precision.
