# 1. Result

R05 — Repeating Sliders & Scheduled Checkpoints. Work 2, 2026-11-03
(Asia/Bangkok). Started latest verified origin/main at
`b18867ea23607ead66d5daba9599d7255eb1d6a1`. This is implementation from existing
research, not a new original-software expedition.

Implemented one-segment Linear sliders with 1–16 traversals, alternating directed
Movement records, carried checkpoint spacing, interior/repeat/final producer
roles, one-entry due processing, separate outcomes and legacy aggregate judgement.
The path survives head judgement and intermediate repeats. The player sees a
moving ball, current-span ticks, TURN chevron, span/count display and tracking/
component feedback. Physical mouse and Z/X remain the only gameplay input.

Added independently authored `CircleGame/Fixtures/slider-ticks.osu`,
`slider-repeat.osu` and `mixed-sliders.osu`, with selectable buttons and output
copy entries. Updated `Gameplay/Beatmap.cs`, `Gameplay/CircleSession.cs`,
`Rendering/PlayfieldView.cs`, `Program.cs`, project/README and appended
`reconstructed/contexts.md`. Build/run instructions remain in
`CircleGame/README.md`. Silent/audible/R04 fixtures, audio backend/clock, R01,
historical reports, research and Work 1 QA are preserved.

**Windows CI PASS** on R05 source commit
`2583c550d63142b2c07134126b35c3593d4772ac`:
[run41489134446 / job125427203516](https://github.com/thgillwtnorizoh/ohno/actions/runs/41489134446/job/125427203516).
Executed 2026-11-03 00:13–00:14 Asia/Bangkok (2026-11-02 17:13–17:14 UTC),
Windows2022 / preinstalled SDK8.0.427. Release WPF win-x64 build succeeded with
**0 compiler warnings / 0 errors**; unchanged QA01 suite reported **90 passed,
0 failed, 90 executed**. Both steps and the job succeeded. The runner's checkout
Node20 deprecation notice is separate from compiler output.

Commands actually executed at repository root:

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

The unchanged `.github/workflows/r04-build.yml` triggered on CircleGame changes;
its historical R04 name does not identify the source under test. The checkout and
logs explicitly identify the R05 source SHA above. No workflow edits, SDK
installation or new gameplay tests. The development host has no dotnet executable;
no local compilation was run. The final documentation-only publication records
this result with application/fixture/workflow content identical to that tested
source commit. Source/preservation/whitespace review passed; no game was launched.

**Owner-reported R04 validation:** the R05 assignment states that the human
owner's tested slider interaction and scoring matched original gameplay at the
user-observable level. This is scope-limited human feedback for that tested R04
interaction, with no recorded map/build/settings/trace comparison supplied here.
It is not universal equivalence or validation of R05 repeats/ticks. Historical
R04 conclusions are unchanged.

No original assets, binary inspection/execution, reverse-engineering tools,
debugger, game window, keyboard/mouse operation, recording, OBS, virtualization
or autoplay. User R05 authorization continues implementation on main and
supersedes historical Work 2 suspension/old milestone branches; no research
journal or numbered RE mark is edited. Stop after R05; new dedicated QA belongs
to Work 1.

# 2. What they found

## Research translation and confidence

The following are **RECONSTRUCTED implementations of Work 1's CONFIRMED static
instructions**, retaining the evidence's original-runtime uncertainty. There is
no newly discovered claim about the original game.

| Source / mechanism | Implemented rule |
|---|---|
| E05 M1be3/M1be4/M28ab; E06 M144a | Keep R04's separate timing/SV rate/spacing selection, finite prepared straight geometry and Single distance ×1000 then double division |
| E21-A M144a:0dd6–0ef9/1332–1378 | Alternating directed endpoints over the same path, accumulated double absolute time, integer Movement endpoints and final parent end after all traversals |
| E15/E21-B M1478/M146f | First inclusive interval wins at a shared endpoint; Single interpolation; zero-duration To; late last-record extrapolation; no reversal tracking reset |
| E16 producer; E21-A | Global distance phase/carry; each traversal resets unadjusted geometric remaining/suppression; advance spacing Q, suppress ordered remaining<=.01*V; append boundary after each traversal |
| E21 M144a:11e0–1200 | After boundary, suppressed→carry0; otherwise phase-=Q-carry, then carry=Q-carry, in that order |
| E16 M145c; E22 | Separate timestamp: int(S+((double)(float)phase/V)*1000); adjust only last stored entry with Max(S+(end-S)/2, oldLast-36); no sort |
| E15 held/entry/leave | Preserve R04 logical-mask history, remembered side, strict follow disk with active radius multiplier2.4000000953674316f; no head-success prerequisite |
| E16/E21-C/E22 M146f | Scan leading inclusive due prefix k; process only j=C+F if j<k; success requires follow and entry<=times[j], including equality; no per-call drain |
| E21-C raw selector | Success raw128 if k==N; else64 if k%(N/R)==0 using integer division; else8. Producer role is NOT the selector |
| E18/E21/E22 consumer | Failure raw-524288 only when updated processed==N, otherwise-262144; pending-count leave precedes combo consumer; positives increment, ordinary negative resets, final negative skips reset |
| E17/E22/E23 legacy branch | Separate head timeout; inclusive parent-end completion without backlog drain; q=(C+positiveHead)/(N+1); q==1→300, q>=.5→100, q>0→50, elseMISS |

Checkpoint definitions retain producer identity and traversal only as independently
designed UI metadata. The recovered schedule itself is a timestamp list. Raw
codes remain separate from producer roles: a delayed interior can emit64/128, or
a repeat can emit8. The original presentation subset, skin objects, score/HP and
optional policy/observer helpers are not reconstructed by these metadata fields.
State keeps null pending distinct from false failed, with raw result and decision
time available for later Work 1 inspection.

As in R04, head, component and parent results are distinct. Successful head50
still contributes one numerator point in the explicitly selected legacy policy.
No ScoreV2/mods/head-sensitive alternate or complete stable scoring is claimed.
Parent accuracy is the existing practice counter, one object per completed parent;
component successes change combo without extra aggregate combo credit.

Completion uses successes so far. If a frame arrives at end with several overdue
entries, only one is processed before completion; other outcomes can stay null.
An out-of-order adjusted final timestamp remains behind a future predecessor
until the leading-prefix scan reaches it. No repair, sort or catch-up loop hides
these recovered distinctions. Once parent Result is set, later tracking returns.

## Playable inputs and source-derived predictions

All three new maps are independently authored, silent, v14, CS/OD/AR4,
multiplier1/tickRate1/base beat1000. Thus V100 and Q100. These numbers are static
fixture predictions, not execution of either game or a new automated QA case.

| Fixture | Movement / checkpoint schedule |
|---|---|
| slider-ticks | Head(106,192) at3000 →(406,192), length300. Movement[3000,6000]; interior4000/5000, final5964; results9000 |
| slider-repeat | Same head/time/path, count2. Forward[3000,6000], reverse[6000,9000]; interior4000/5000, repeat6000, interior7000/8000, final8964; ends back at head, results12000 |
| mixed-sliders | Circle3000, tick slider5000→8000, circle9500, out-and-back11500→17500, circle19000, three spans21000→28800, circle30500; results33500 |

The mixed map's length260 three-span slider exercises nonzero spacing carry:
producer schedule[22000,23000,23600,24200,25200,26200,27200,28200,28764], with
traversal ends23600/26200/28800. Reversal geometry is still one line; it does not
restart a generic equally spaced tick clock at every turn.

## Engineering choices, limits and unresolved fidelity

- Input policy: one straight segment (`L|x:y`), 1–16 traversals, finite positive
  requested length>.0001 and<=4096, each traversal>=1 ms, end<=3600000 ms,
  <=4096 checkpoints per slider /100000 per map, previous2MB/10000-object limits.
  Reject violations before replacing the old session. These are engineering
  bounds, not stable format restrictions. Curves, multi-control paths, stacking,
  equal start times and overlapping slider intervals remain explicitly rejected.
- R04's gameplay clock, physical input mapping and full-list attention remain.
  WPF supplies one integer time/cursor/held snapshot per running frame; it does
  not reproduce repeated native clock reads, provider delta, helper exceptions,
  scheduler/candidate gates or original polling cadence. Before-start Movement
  fallback is a display policy; tracking is guarded until start. Rendered ball
  is clamped while late required-position queries retain extrapolation.
- Primitive current-span tick dots are sampled from the prepared Movement at the
  produced deadline. Boundary metadata uses the directed endpoint. They and the
  TURN chevron/span/count labels are display aids, not recovered skin placement,
  snaking, hit effects, animation or pixel identity. They never determine success.
- More than two traversals use the same recovered loading loop, within the
  explicit16 bound; no claim that every count/path/timing edge has been validated.
- R03 Stopwatch authority and media lifecycle remain unchanged. No BASS-clock
  equivalence, speculative offset, drift correction or audio-device calibration.
- Existing90 cases cover circle/parser regressions, not new repeat/tick gameplay.
  No new automated gameplay tests or QA campaign were authored. Actual R05 human
  play, Windows input/render/media behavior, native precision and original-runtime
  equivalence remain unverified and are left for owner/Work 1 validation.

No concrete implementation-blocking gap remains for this bounded research-derived
slice. Original scheduling/policy/media/visual fidelity remain documented limits;
no unrelated investigation or invented workaround was used to fill them.

# 3. Pseudocode

**RECONSTRUCTED** from E05/E06/E15–E18/E21–E23, finite supported inputs:

```text
prepare line and start-selected V/Q using existing R04 rules
absolute=S; phase=0; carry=0
for span in 0..R-1:
    append forward/reverse Movement(int(absolute), int(absolute+dt))
    absolute+=dt; carry+=preparedDistance
    remaining=unadjustedGeometry; suppressed=false
    while carry>=Q and not suppressed:
        phase+=Q; carry-=Q; remaining-=Q
        suppressed=(remaining<=.01*V)
        if not suppressed: append interior stamp(float(phase))
    phase+=carry; append traversal-end stamp(float(phase))
    if suppressed: carry=0
    else: phase-=Q-carry; carry=Q-carry
end=int(absolute)
adjust only final deadline; retain producer order

physical head edge: classify head; parent remains pending
each running frame at/after start, parent pending:
    select FIRST inclusive Movement; compute required target
    evaluate held history + strict follow disk
    enter/re-enter records entryTime; reversal alone changes no tracking state
    k=length of leading due prefix
    j=successful+failed
    if j<k:
        success=follow and entryTime<=deadline[j]
        update one outcome/counter only
        raw success: k==N ? 128 : k%(N/R)==0 ? 64 : 8
        raw failure: processed==N ? -524288 : -262144
    if not follow and tracking and processed<N: leave
    consume raw for scoped combo feedback
    separately timeout head
    if T>=end: legacy aggregate once from current successes; no backlog drain
```

**ENGINEERING:** producer metadata, finite limits, WPF sampling and primitive UI,
practice accuracy/results and frame time snapshot. **UNVERIFIED:** R05 human play
and original native/runtime/pixel/audio equivalence. No hypothesis is promoted to
an established original rule.
