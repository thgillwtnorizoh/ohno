# 1. Result

R06 — Human-Playable Bézier Sliders. Work 2, 2026-11-03 (Asia/Bangkok).
Started clean latest origin/main at `8a92b88c32e11a5d6d513f732a47f540e5fd772a`.
Read AGENTS, complete standing contract (repository/attachment SHA256 identical),
implementation journal/R05, E32, existing QA01/QA03 contracts and relevant shared
slider findings. No original-binary investigation or runtime expedition.

Implemented ordinary format-v14 B| paths: quadratic/cubic/multiple subcurves,
E32 control normalization/separators/adaptive preparation, shared E06 length
adjustment and ordered per-segment Movement/checkpoint integration. Existing
Linear inputs use the same geometry interface. CircleSession judgement/held
input is unchanged. WPF caches a frozen connected path at Load, draws its body,
moving ball/ticks/follow feedback and traversal-end repeat cues until parent end.

Added independent quadratic, cubic, joined and mixed fixtures with selectable UI
buttons; previous fixture buttons and audio/clock remain. Updated Windows user
instructions in `CircleGame/README.md`; standard run/build commands are unchanged.
Implementation/fixtures/report/journal and a small development preparation-check
project live under reconstructed/. The only shared CI infrastructure change is
one preparation-check step and its path trigger in the established workflow;
QA01/QA03 test files and research are untouched. This enables the requested
bounded noninteractive checks without starting QA04 or another test framework.

CI at initial source publication: **PENDING**, not a pass. The final record will
identify actual source SHA/run/job, WPF win-x64 compilation,90 QA01 cases,
56 QA03 cases and four bounded preparation targets/four fixture loads. No local
dotnet executable is available; no SDK installation or local compilation.

No gameplay window, OS input operation, recording/OBS, debugger, original-game
execution or routine supervised gameplay. Human R06 operation is UNVERIFIED.
Compilation/component checks do not establish visible or original equivalence.
Historical R04 owner feedback and R05/QA03 records remain scope-limited records,
not R06 validation. No new RE mark or research journal edit. Current user scope
continues implementation on main instead of historical suspension/branch rules.
Stop after R06; Work 1 owns subsequent dedicated QA04.

# 2. What they found

## Research-derived implementation

**RECONSTRUCTED** from E32's **CONFIRMED static** instructions, preserving its
original-runtime and precision uncertainty:

- `Gameplay/Beatmap.cs`, `SliderGeometryPreparation`: head insertion only when
  empty/unequal, control ordering and exact duplicates retained. Fresh owned
  lists are copied into read-only geometry; original list aliasing/editor reuse
  is not reconstructed. Bézier controls parse finite Double→Int32→Single, with
  no new control clamp. Existing head clamp/no-mod policy remains; B heads use
  Double before integer conversion. Existing Linear numeric grammar is retained.
- Modern range scan: separator only i<count-2 and P[i]==P[i+1]; emit through i,
  advance once to the second duplicate as the next start, then normal loop step.
  Final equal pair is not split; no global deduplication. Two-control ranges emit
  the recovered line special case; singleton adaptive input emits two equals.
- M2d90 flatness: `(P[i-1]-2f*P[i])+P[i+1]`, Single squared length<=.25f,
  equality accepted. M2d91 halves control polygons with Single midpoint operations.
  Stack pushes right then left. M2d92 emits leaf first and weighted interior
  `.25f*((buffer[2i-1]+2f*buffer[2i])+buffer[2i+1])`; only M2d93 appends the final
  source endpoint. No analytic uniform-t sampler or library approximation.
- `SliderGeometry` retains normalized controls, raw segment pairs/boundary flags,
  unadjusted G, requested L, adjusted segments and Double cumulative lengths.
  Segment length uses Single component squared sum→Double Sqrt→Single return.
  E06 tail adjustment removes pieces while d<=excess+.0001; otherwise preserves
  the zero-direction stop or shortens/extends the remaining tail. Adjusted totals
  are recomputed, never assigned L. Entirely erased/zero paths explicitly reject.
- E05/E16/E21 use each cumulative difference converted to Single; per-segment
  `dt=Double(1000f*d)/V` accumulates absolute time before integer Movement endpoints.
  Reverse traversals visit the same pieces backwards with swapped endpoints.
  Existing first inclusive Movement selection, zero-duration To and late fallback
  extrapolation remain. Requested length is not used as analytic path distance.
- Checkpoint phase/carry persists across every prepared segment and traversal;
  only remaining original G/suppression reset per traversal. Separate M145c
  stamps and final-only adjustment remain. Due-prefix/one-item processing, timely
  entry, tracking/history, combo and explicitly selected legacy aggregate consume
  the same interfaces, with no separate Bézier judgement engine.

`Rendering/PlayfieldView.cs` caches map-space StreamGeometry before replacing the
session and reuses it on restart/resize. Rendering transforms it through the same
layout mapping used by the cursor. Tick marker positions sample the prepared
Movement at their produced integer deadline, retaining R05's independent marker
policy; roles never drive judgement. Repeat chevrons use the ending piece of the
current traversal, rather than treating every flattened piece as a reversal.
Zero final direction retains a TURN label without inventing a geometric tangent.

## Supported inputs and explicit engineering limits

Supported: existing circles/v5–v14 one-segment Linear inputs, plus v14 Bézier
subcurves (including quadratic/cubic, joined ranges and two-control ranges),
1–16 traversals, mixed maps, generated ticks and repeats. Other curve types,
older Bézier branches, stacking/mods/storyboards/full score/HP/ScoreV2 stay outside
scope. All unsupported types reject explicitly, preserving the previous session.

Safety limits are **engineering choices, not original rules**:64 normalized
controls,8192 raw segments and50000 visited subdivision nodes per slider;
100000 prepared segments and250000 Movement records per map. No fixed original
subdivision depth is asserted. Previous requested-length/time/checkpoint/map-size
bounds remain. Nonfinite operands/output and zero/erased geometry reject instead
of inventing original exceptional behavior. Each whole traversal must last>=1 ms;
individual segment records may have equal integer times and retain the researched
zero-duration rule. No segment is silently removed to hide those boundaries.

The four new fixtures are silent, independently authored v14 inputs: Quadratic
and Cubic begin at3000; Joined Bézier intentionally repeats its shared endpoint;
Mixed R06 contains circles, Linear, quadratic, joined and a two-span cubic with
ticks. All use beat1000/multiplier1.4/tickRate1. Mixed's final circle is27500 and
results30500; other ends derive from actual adjusted segment arithmetic.

## Validation boundary

`R06PreparationChecks` links the real Beatmap/geometry implementation. Its bounded
development checks compare exact raw output to E32's quadratic/cubic vertices,
joined raw vertices/boundary counts and final-pair guard; they separately load the
four fixtures. These are conditional static-oracle checks, not original execution
or dedicated QA04. No gameplay input/state machine is simulated by that project.
Original research/tool extraction is not repeated. Actual hosted outcomes are
recorded separately from predictions in Result and the appended journal outcome.

Single storage/operation ordering is preserved where practical; modern .NET SIMD,
Normalize and x86 evaluation/JIT differences are not bitwise equivalence claims.
WPF strokes are independent presentation, with no original mesh/skin/shader/pixel
claim. Provider/scheduler/candidate gates and helper exceptions remain excluded.
Stopwatch clock authority and audio lifecycle are unchanged; no offsets, BASS
clock or drift/calibration redesign. Actual R06 human input/render/media behavior
and original-runtime fidelity remain unverified. No concrete blocking algorithm
gap remains for the supplied finite v14 scope.

# 3. Pseudocode

**RECONSTRUCTED — E32/E06 preparation and existing E05/E16/E21 integration:**

```text
parse B controls (v14 only), Double -> Int32 -> Single
normalize head; preserve exact point order and duplicates
split modern ranges at guarded forward-adjacent equals
for each range:
    if two controls: emit one line
    otherwise pending <- controls
    while pending:
        pop polygon
        if second-difference squared <= .25f everywhere:
            half-split; emit first + weighted leaf interiors
        else: half-split; push right then left
    append original range endpoint once
    connect consecutive emitted vertices; mark subcurve end
retain raw segments and unadjusted G
apply shared requested-length suffix trim/extension and zero-direction guard
recompute Double cumulative adjusted lengths
for each traversal:
    visit prepared pieces forward or reverse with swapped endpoints
    append per-piece timed Movement using cumulative Single distance differences
    carry existing checkpoint spacing across pieces and turns
append traversal boundaries; adjust only final deadline
existing physical head/held/mouse tracking -> one due component -> legacy parent
```

**ENGINEERING:** bounded allocations/work, immutable owned snapshots, frozen WPF
path/primitive cues and finite development checks. **UNVERIFIED:** human R06 play,
native/pixel/audio equivalence and pathological/reused/editor behavior.
