# R12 — Discrete HitSound Practice for Windows

A minimal desktop osu!standard reconstruction using **C# / WPF / .NET 8**.
Move a real mouse and physically press **Z or X** to hit circles. No autoplay,
simulated input, network service, original assets or original-game execution.

**R08 baseline:** see `../R08-legacy-accounting.md` for its Windows build,255 unchanged
QA cases and bounded ledger checks. R08 human score/visual equivalence remains
unverified. Historical reports and scoped R04/R06/R07 owner feedback are preserved;
they do not validate R08 scores. The R05 report records its Windows build and
existing 90-case regression outcomes. The owner reported that the tested R04
slider interaction/scoring matched the original at the user-observable level;
this is scope-limited human feedback, not universal equivalence or R05 validation.
No game window is operated by an agent. Historical R04 conclusions are preserved.

R02's implementation/build status is recorded in `../R02-first-circle.md`.
A successful build does not mean a Windows window or physical input has been
verified on the development host. Work 1 owns the subsequent independent QA.

## Build and play on Windows

Prerequisite: Windows 10/11 and the **.NET 8 SDK for Windows** (not only a .NET
runtime). WPF uses the Windows Desktop framework. The first build restores its
Microsoft targeting/runtime packages; the game itself has no network features.

From a checkout of the latest `main`, either double-click
`reconstructed\CircleGame\Start-CircleGame.cmd`, or use PowerShell at repo root:

```powershell
dotnet run --project reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release
```

1. The original silent eight-circle fixture loads into a ready screen.
   For audio, click **Audible fixture** and wait for its local media to load.
   **Slider fixture** retains the R04 control. For R05, choose **Tick fixture**,
   **Repeat fixture**, or **Mixed R05**.
2. Click **Start** or press **Space**. The first circle is due at 3 seconds.
3. Aim at the circle with the mouse. When its shrinking ring reaches the circle,
   press **Z** or **X** once. Holding a key does not repeatedly hit objects.
   For sliders, press at the head, keep Z or X held, and move with the ball.
4. Read the 300 / 100 / 50 / MISS feedback and the combo/accuracy HUD.
5. Results appear 3 seconds after the final object ends (16.5 seconds for the
   original circle fixture; see below for the new maps).
6. Press **R** or **Enter**, or click **Restart**, to play again immediately.

**Space** pauses/resumes. Losing window focus pauses without automatically
resuming; press Space after returning. Mouse clicks focus the playfield but do
not score. **Open .osu** pauses and opens a local file picker; cancellation leaves
the old session paused. A rejected file leaves the old map, audio and progress
intact. A newly loaded map waits for Start. **Silent fixture** restores the
original silent control. All interactive operation remains human-controlled.

## Legacy score and results (R08 / R08-C02)

Start any existing fixture or load a supported local map as before. The HUD shows
legacy score, accuracy, current combo and maximum combo from the E33 ledger.
Completion shows final score, four whole-result counts, accuracy, maximum combo,
completed objects and policy/K provenance. No official ranks or online records.

The human application's default is now **automatic E034 fresh-file K**. The policy
line identifies e.g. `Plain legacy · K=2 (AUTO E034 fresh .osu N4/T12s/B0ms)` for
Mixed R07. N counts raw hit-object rows, T is signed/truncated whole seconds between
first/last recognized row starts minus summed numeric-event2 break durations; B is
the raw break sum. Slider/spinner ends, audio length, tick count and judgement
counts do not supply this denominator. HP/OD/CS retain their fresh-file metadata
values/defaults. Difficulty, density and rounding preserve the recovered Single /
Double operation order and midpoint-to-even behavior. Positive count with T=0 is
a supported infinity-to16 clamp, including the sole Joined Bézier slider.

**Mixed R07** automatically prepares K2. With E034's matched conditional events it
predicts2392, versus2236 with an explicit engineering K1. **Joined Bézier** prepares
K3 and conditionally predicts534, versus438 at K1. The owner's reported original582
remains unresolved. Fixture difficulty and E33 ledger arithmetic are unchanged.
These are finite predictions, not newly observed human gameplay or proof that the
owner's original chart/settings/events match these fixture bytes.

For controlled component comparisons, explicitly construct
`new CircleSession(map, ScoreConfiguration.EngineeringPractice)` or supply
`new ScoreConfiguration(k, ScorePolicy.PlainLegacy, "ENGINEERING test override")`.
Finite K0–10 remains an engineering safety bound. Normal app loading instead calls
`ScoreConfiguration.FromBeatmap(map)` before replacing the session/audio. The
historical one-argument CircleSession API retains its named engineering K1 preset
for compatibility with existing QA/synthetic callers; it is not used by the UI.
There is no automatic-to-engineering fallback. Malformed/unsupported automatic
metadata is rejected explicitly, preserving the prior paused valid session.
Synthetic maps without fresh metadata must use explicit configuration.

Fresh file preparation uses the same in-memory bytes as gameplay loading. No
cached/reused original record or database format is emulated. Metadata parsing is
separate from sorted gameplay objects; existing gameplay admission restrictions
remain. Direct metadata controls can describe unordered/overlapping/unknown/hold
rows without admitting those as playable objects. NaN/nonfinite conversion, bad
break fields and unsupported metadata formats fail explicitly rather than receiving
a guessed multiplier. Fresh missing HP defaults5; reused original difficulty/cache
state and original x86 marginal precision remain unverified.

E33's score formula, whole-result-only Single accuracy, component amounts,
pre-result combo bonus and once-only lifecycle emission are unchanged. Positive
slider aggregates add no duplicate combo; parent MISS resets combo, terminal
component failure preserves combo and spinner components bypass combo. Pause
retains the ledger. Restart creates fresh score state with the same prepared K;
map replacement prepares that map's K. Only the plain ordinary null-ScoreV2 table
is supported; modifiers/decorations/ScoreV2 and complete native record selection
remain unsupported. No fitted scores or metadata changes are used.

See `../R08-C02-automatic-score-multiplier.md` for exact CI source/outcomes and
bounded metadata controls. Windows launch command and all13 fixture buttons remain
unchanged. All interactive operation belongs to the human owner.

## First slider

**Slider fixture** loads `Fixtures/first-slider.osu`: circle at3000, straight
slider at5000, then circles at8000 and10000 ms. The slider moves from(160,192)
to(320,192) over1600 ms, calculated from beat length2000, multiplier2, tick rate1
and prepared length160. Its only scheduled component is the terminal at6564;
parent completion is at6600. These are derived fixture numbers, not observed
original-game playback. Results appear at13000 ms.

Aim and press Z/X at the head, then follow the moving ball while holding a key.
The head result does **not** remove the body. The follow ring widens while tracking
and changes colour when eligibility is lost. Releasing both keys or leaving the
follow disk loses tracking before the terminal; re-entry uses the smaller radius
and records a new entry time. Re-entry before/on the terminal deadline can succeed.
Acquiring only after that deadline cannot rescue it. There are no interior ticks
in R04, so a gap in the middle is not automatically a failed whole slider.

The original R04 fixture retains the recovered **legacy, no-ScoreV2** aggregate: positive head
and successful terminal →300; exactly one successful component →100; neither
→MISS. A 50-quality positive head can therefore still yield300 with a successful
terminal. Head and terminal feedback remain separate. Head success increments
practice combo and head failure resets it; terminal success increments combo,
while terminal failure follows E22's no-reset branch. Aggregate display does not
award a duplicate combo increment. Accuracy counts one completed parent object,
not head+tail as separate circles. This is not stable's complete scoring pipeline.

## Ticks and repeating sliders

Keep Z or X held and move with the ball through each **TURN** chevron; do not
release merely because the ball reaches an endpoint. The complete path remains
visible until the final traversal ends. Span progress and checkpoint/hit counts
appear during tracking. Yellow dots identify interior checkpoints for the current
span; processed dots become teal for success or pink for failure. Turn chevrons,
markers and labels are independent visual aids, not original skin reconstruction.

| Button / fixture | Contents | Derived schedule / results time |
|---|---|---|
| Tick fixture / `slider-ticks.osu` | One 300-pixel span, head at3000 | Ticks4000/5000, terminal5964, end6000; results9000 ms |
| Repeat fixture / `slider-repeat.osu` | Same line out and back, head at3000 | Ticks4000/5000, repeat6000, ticks7000/8000, terminal8964, end9000; results12000 ms |
| Mixed R05 / `mixed-sliders.osu` | Circles plus one-, two- and three-span sliders | Final circle30500; results33500 ms |

These independently authored silent maps use multiplier1, tick rate1 and base
beat1000 ms (travel rate100 pixels/second). The schedules are source-derived
predictions, not original-runtime or human-play measurements. New maps contain
no original assets; R03 audio loading and its generated audible fixture remain.

At a shared endpoint the first inclusive Movement record wins. The next record
then moves in the opposite direction; a turn does not reset held/tracking state.
Each reached frame processes **at most one** outstanding checkpoint from the
leading due prefix, in producer order. Deadlines are not sorted after the final
adjustment. Late/stalled frames can therefore leave a backlog at parent end;
completion uses current successes without draining or repairing pending entries.

A checkpoint succeeds only while follow-eligible and with entry time at/before
its own deadline. Head success is not required. A failed interior or repeat
resets practice combo; failure of the final scheduled entry does not. Successful
components increment combo. The recovered raw success selector uses the due
prefix count, not producer identity, so a delayed tick can carry a repeat/terminal
raw code. Display labels retain producer roles separately.

Legacy parent grading uses `(successful checkpoints + positive head)/(N+1)`:
exactly1 gives300; at least.5 gives100; any smaller positive ratio gives50; zero
givesMISS. Whole-object accuracy and feedback remain practice counters, with no
extra aggregate combo point. Full stable score/health/ScoreV2 are not implemented.

## Bézier practice (R06)

Choose **Quadratic**, **Cubic**, **Joined Bézier**, or **Mixed R06** in the toolbar,
then Start/Space. Aim and press Z/X at the head, hold and follow the ball through
the curved body and TURN marker. R/Enter restarts; Space pauses/resumes. All older
fixture buttons remain. These instructions are for human operation; no agent has
operated gameplay or observed successful R06 play.

| Fixture | Contents |
|---|---|
| `bezier-quadratic.osu` | One quadratic arc starting at3000 ms |
| `bezier-cubic.osu` | One cubic arch starting at3000 ms |
| `bezier-joined.osu` | Two joined quadratic subcurves with an intentional repeated control |
| `bezier-mixed.osu` | Circles, Linear, quadratic, joined curve and a two-traversal cubic with ticks |

All are independently authored, silent format-v14 maps. They use base beat1000 ms,
multiplier1.4/tickRate1. Duration/checkpoints use the **prepared segments** after
requested-length adjustment, rather than analytic Bézier parameter steps. Results
appear3 seconds after the final object ends (30500 ms on Mixed R06).

`B|` is supported only in v14. Exact adjacent duplicate controls split subcurves
with E32's final-pair guard; controls are not globally deduplicated. Bézier control
coordinates parse Double→Int32→Single, without a new control clamp. Raw flattening
and length-adjusted geometry are distinct. The engine uses E32's .25f squared
second-difference test, half subdivision and weighted leaf points. The cached WPF
polyline is primitive presentation, not original meshes/skins/pixels.

Engineering limits:64 normalized controls,8192 raw segments and50000 subdivision
nodes per slider;100000 prepared segments /250000 Movement records per map.
There is no claimed original fixed subdivision-depth cap. Work-limit violations,
nonfinite results or paths erased by trimming reject the candidate map, retaining
the previous session. Existing 1–16 traversals and checkpoint/parser bounds remain.
Only single-segment `L|x:y` and v14 `B|` are supported; Catmull-Rom, PerfectCurve,
older Bézier formats, stacking/mods/storyboards/full score remain outside R06.

No adaptive subdivision or requested-length adjustment runs in the drawing loop.
Forward/reverse Movement consumes every prepared piece in order, preserving first
inclusive matches at shared boundaries. Repeat cues use the **traversal** end,
not each tiny segment end. Held input, entry deadlines, one-component due handling
and legacy grading are unchanged; audio remains Stopwatch-controlled as below.

## Spinner practice (R07)

Choose **Spinner Practice**, **Circle + Spinner**, or **Mixed R07**, then
Start/Space. Hold **Z or X** and move the real mouse in circles around the marked
center. Holding without moving does not generate new angular motion. The rotor
and progress bar show motion and progress; filling the best-target bar earns300
under the supported finite policy. Keep moving through the playable interval.
Space pauses/resumes; R/Enter starts a fresh session. All old fixture buttons,
audio loading, pause/restart and results remain available.

| Fixture | Contents / timing |
|---|---|
| `spinner-practice.osu` | One spinner3000–6000 ms, OD5; prepared N15, best needs C>15; results9000 ms |
| `circle-spinner.osu` | Circle3000, spinner5000–8000, circle10000; results13000 ms |
| `mixed-r07.osu` | Circle3000, quadratic slider5000, spinner10000–13000, circle15000; results18000 ms |

These are independent silent v14 practice inputs, with no original assets. Their
requirements/timing are source-derived; they have not been played by Work. The
displayed progress is an observed-integer-change counter, **not a literal number
of full physical revolutions**. Under modern N15 grading, C0–2 misses,3–13 gives50,
14–15 gives100,16+ gives300. Other durations/OD derive N from the recovered formula;
N0 has its separate best-grade special case.

Spinner components are distinct from the final grade and award no practice combo
points. Final positive judgement adds one combo; final failure resets it. Progress
integrates strictly inside start/end; completion is strictly after end, with no
unread-progress drain. Releasing a key changes normal target generation but does
not directly zero smoothed motion. No radial-distance threshold is imposed.

R07 accepts v14 positive-duration spinners up to60000 ms, end<=3600000 ms, with
stored intervals disjoint from every other object (circles are point intervals;
shared endpoints reject). This is a bounded parser policy, not stable's complete
overlap/candidate rules. Sound column5 must be an Int32; optional hit-sample text
is retained without sound playback. Spinner coordinates are parsed but their
display center is the independent UI center(256,192).

The mouse and displayed center use the same map/display projection, including
positions outside the stage. Original E24 instead reads a presentation-prepared
native center with different layout/update history. WPF offers one integration
per running rendering callback and qualifies angular input only after the recovered
nominal16.666… ms period. This explicit engineering gate avoids feeding every
high-Hz render sample into the nominal divisor; it is not native scheduling
equivalence. It does not reproduce all timing-tail markers, provider cadence,
repeated clock reads or up to31 original host passes. Separate histories persist through pause
and reset with a new session. No Replay/legacy source policy, Relax/Auto, bonus
payout, native spinner effects, full score/HP or audio redesign is included.

## Local beatmap audio

`[General] AudioFilename` is resolved relative to the selected `.osu` file's
directory. Omitted/blank AudioFilename means silent practice. Use a local relative
file inside that directory; subfolders are allowed. URLs, absolute/UNC references
and paths escaping the map directory are rejected as an R03 input policy.
Surrounding double quotes are allowed. No audio is copied from an osu! install.

The Windows-native WPF `MediaPlayer` opens audio asynchronously, with a 15-second
timeout and a visible loading status. Gameplay controls are disabled during
loading. The previous session is paused and retained until the candidate reports
MediaOpened with an audio stream. Missing files, decoder failures, unsupported
formats, cancellation or timeout discard only the candidate; Space can resume the
previous session. Closing cancels and closes both pending and active media.

Accepted filename extensions are WAV, MP3, WMA, M4A, AAC, OGG, FLAC and OPUS.
This is **not a codec guarantee**: decoding depends on the user's Windows media
components. Playlists are excluded. Try the generated PCM WAV fixture first.
A declared but broken audio file is an error, not an automatic silent fallback.
There are no new NuGet audio packages and no streaming/network feature.

- Start/restart seeks audio to zero and commands Play, then restarts the practice
  clock. Space/focus loss pauses both; explicit resume commands Play and resumes
  that clock. Seeking/starting are asynchronous media operations, not a measured
  guarantee of simultaneous audible output.
- **Clock authority stays Stopwatch**, rounded to integer milliseconds and
  published on WPF frames. Circle judgement math is unchanged except QA01-D01.
  Audio position never rewrites judgement time. Startup/seek/device buffering
  latency and drift can occur; stalls are not corrected. No BASS-clock
  equivalence, speculative offsets, device calibration or drift controller.
- If a short track ends early, the timeline keeps running in silence until the
  map's last-object-end+3000 result horizon. Resume does not replay an ended
  track; restart does. A longer track pauses at the result overlay.
- A late playback error pauses the current session and shows an audio error.
  Reload the map or choose Silent fixture. A late decoder error after successful
  replacement does not resurrect the previously disposed map.
- Replacement closes/releases the former player only after the candidate is
  ready. Exit closes all players. Driver/decoder shutdown is best-effort on error;
  exceptions are contained rather than terminating the UI.

### Independent audible fixture

The **Audible fixture** button runs `Platform/PracticeFixture.cs`, which generates
an 18-second, mono, 22050 Hz, 16-bit PCM WAV in a uniquely created temporary
directory, alongside a copy of `Fixtures/audible-circles.osu`. It needs no Python,
external recording, generator executable or original-game asset. The waveform
algorithm is fully supplied: enveloped 440 Hz guide ticks every half second and
880 Hz ticks at the eight circle deadlines (3.0 through 13.5 seconds). Volume is
0.7 at the media player; waveform amplitude is also limited in the generator.

The synthetic tone output is independently authored for this project and
dedicated to the public domain under **CC0-1.0**. No commercial music is included.
The algorithm is reproducible; cross-runtime byte identity is not certified.
The temporary directory is deleted on exit where Windows has released its file
handles; otherwise it can remain under `%TEMP%\ohno-r03-*`. No user map is edited.
The `.osu` file in the checkout references the generated WAV; use the button to
create the complete audible pair rather than opening that source template alone.

Supply a map at startup, optionally:

```powershell
dotnet run --project reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -- "C:\Maps\circle-practice.osu"
```

To build an executable rather than run through the SDK:

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false
# Requires the matching .NET 8 Windows Desktop Runtime on the player's machine:
.\reconstructed\CircleGame\bin\Release\net8.0-windows\win-x64\Ohno.CircleGame.exe
```

For an optional standalone distribution (copy the **entire output folder**):

```powershell
dotnet publish reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained true -o reconstructed/CircleGame/bin/standalone
```

Run/publish commands are human Windows instructions. R05 uses noninteractive
Windows CI for compilation and the unchanged component suite; see the report.
No original osu! installation is required. If a launch fails, run the command in
PowerShell to retain the error output. Source, fixture and Windows instructions
are committed; build outputs are intentionally ignored.

## Accepted maps and limits

- `.osu` format v5–v14; explicit `Mode: 0`, CircleSize, OverallDifficulty and
  ApproachRate, with finite difficulty values in [0,10].
- Ordinary circles, `L|x:y,spans,length` straight sliders and format-v14
  `B|x:y|...,spans,length` Bézier sliders with 1–16 traversals,
  at distinct integer start times in [0,3600000] ms. The count includes the first
  traversal (2 means out-and-back). Normal/NewCombo/colour-skip bits are accepted.
  Multi-point Linear paths, other curve types, older Bézier formats, overlapping slider intervals,
  holds, equal times and malformed/empty maps are explicitly rejected. Ordinary
  v14 Spinner8 is supported under the separate bounds above.
- Slider maps require explicit SliderMultiplier and SliderTickRate, clamped to
  E05's [.4,3.6] / [.5,8] ranges. Timing points are retained in file order; base
  and inherited selection at slider start drive travel rate and spacing. Missing
  timing uses E05's rate fallback. Changes during a slider do not change its rate.
- Nondegenerate slider length must be >.0001 and <=4096 pixels; each traversal
  >=1 ms, parent end <=3600000 ms. Maximum 2 MB / 10000 objects, 4096 scheduled
  checkpoints per slider and100000 per map. These are explicit engineering limits,
  not original format restrictions. Exceeding them rejects the candidate map.
- The included map is the intended control: separated, unstacked, no mods or
  timing/coordinate tricks. General dense/overlapping maps are not certified.
- Mandatory hitSound column 5 must parse as an Int32 (negative/unusual bit values
  are not mask-restricted); hit-sample playback is still unimplemented.
- Difficulty keys, AudioFilename and slider timing points are read; combo colours,
  hit samples, other timing-point effects and events are not consumed. There is no full stable
  parser, old-version offset handling, stacking, HP/fail policy or mod support.

## Source layout

| File | Responsibility |
|---|---|
| `Program.cs` | Window, file picker, real keyboard edge handling and focus policy |
| `Platform/GameplayClock.cs` | Practice clock authority for both silent and audible maps |
| `Platform/BeatmapAudio.cs` | Asynchronous local-media loading and native playback lifetime |
| `Platform/PracticeFixture.cs` | Independent reproducible PCM tone generator |
| `Gameplay/Beatmap.cs` | Reader, E32 flattening/shared prepared segments, timing, per-segment Movement and checkpoint producer |
| `Gameplay/CircleSession.cs` | Circle/slider state plus separate spinner angular, motion, observation and final-grade components |
| `Rendering/PlayfieldLayout.cs` | Forward/inverse layout relation inside a 4:3 stage |
| `Rendering/PlayfieldView.cs` | Primitive drawing, frame callback, actual mouse sampling |
| `Fixtures/first-circles.osu` | Independently authored eight-circle practice input |
| `Fixtures/audible-circles.osu` | Separate template paired with generated tones |
| `Fixtures/first-slider.osu` | Independent mixed circle/slider control, no interior ticks |
| `Fixtures/slider-ticks.osu` | Independent interior-tick slider |
| `Fixtures/slider-repeat.osu` | Independent two-traversal out-and-back slider |
| `Fixtures/mixed-sliders.osu` | Independent circles and one-/two-/three-traversal sliders |
| `Fixtures/bezier-*.osu` | Independent quadratic/cubic/joined/mixed R06 controls |
| `Fixtures/spinner-practice.osu`, `circle-spinner.osu`, `mixed-r07.osu` | Independent R07 spinner controls |

`ScoreLedger`, `ScoreConfiguration` and provenance-bearing `ScoreEvent` are separate
gameplay components in `Gameplay/CircleSession.cs`, preserving existing QA source
links unchanged. The UI consumes the ledger; it does not recalculate score/counts.

The gameplay layer has no WPF dependency. Rendering consumes session state;
platform input supplies a mapped pointer and stored gameplay time. R01's Python
kernel and tests remain intact and are not artificially wired into this slice.

The implementation report separates research-derived rules from deliberate
slice policies and records unresolved original-equivalence dependencies.

API reference: [Microsoft WPF MediaPlayer](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.mediaplayer?view=windowsdesktop-8.0).

## Spinner presentation correction (R08-C01)

Spinners appear at their stored start, rather than borrowing a circle's approach
preview. They remain visible until completion is evaluated. This is an engineering
presentation policy; native spinner fade timing is unrecovered. Earlier sliders
retain their physical held-input/tracking path. No scoring/input formula changed.
See `../R08-C01-spinner-presentation.md` for checks and remaining runtime limits.

## Original-derived health (R09)

Choose **Health R09** for the new independent silent practice map: circles at3000 /
4500, a straight slider6500–8100, break8500–9500, spinner10000–13000 and circle15000.
Use the same mouse, Z/X, Space and R/Enter controls. All older fixtures stay intact.
The bar fills using **V**; the white target marker and diagnostics identify **H**.
The footer also shows uncapped calibration balance **A**, prepared drain **r**,
gains **B/G** and calibration trial count. These are independent WPF primitives,
not original skin/fade/pixel fidelity. E33/E034 scores and accuracy are unchanged.

Health uses E035 chart calibration rather than a universal rate. Private trials
use prepared object order, original slider subset/schedule counts, spinner N,
combo-group boundaries and ordered breaks. Trial notifications never enter your
live score. Live reset is H200/A200/V0 while retaining calibrated coefficients.
Supported plain judgement HP applies once at the original-derived event boundary;
passive drain consumes the previous eligibility flag after judgement processing.
Zero/negative consumed deltas retain the recovered arithmetic. Pause does not run
health updates; restart/map replacement create fresh independent health state.

**R10 automatic HP uses E036 fresh ordinary NewCombo preparation.** The application
loads and restarts through `CircleSession.CreateFreshOrdinary(map, scoring)`.
The first circle/slider infers NewCombo from the fresh null last-registration
reference; subsequent ordinary circles/sliders infer false, unless the previous
registered object is a spinner. Explicit file bit4 is ORed after helper evaluation.
Spinners themselves use only their file bit4. A separate postparse pass forces
NewCombo when the next pending break ends strictly before the object's start;
equality remains pending. Final flags feed the existing E035 private calibration.

The unchanged silent eight-circle fixture now prepares health when the remaining
bounds pass. No fixture data or HP arithmetic was changed to enable it. Prepared
objects must still have strictly increasing source starts and nonoverlapping
intervals; breaks must be ordered, positive and contained in inter-object gaps.
No sorting repairs ambiguous registration. Unsupported identity/count, time-domain,
manager reuse/clone/editor/alternate factory, break structure or calibration safety
exhaustion disables HP with a precise status, preserving normal gameplay/scoring.
There is no guessed r/B/G fallback. The fresh parser's exact map ownership is an
engineering safeguard, not an emulation of every original manager lifecycle.

**Versioned API compatibility (owner approved):** historical `new CircleSession(...)`
constructors and `HealthChart.FromMap(map, approach)` retain R09's former unknown-
M2902 contract for unchanged QA07 I04/I08 and historical development assertions.
They may report HP unavailable for unmarked sequences. For the R10 application
contract use `CircleSession.CreateFreshOrdinary(map, scoring)` or explicitly select
`HealthPreparationPolicy.E036FreshOrdinary` in the extended APIs. Existing513 QA
cases therefore verify historical/component regressions; the separate R10 checks
exercise the new fresh path. No test-suite detection or hidden switch is used.
Missing parser metadata on synthetic maps also disables automatic HP.

Diagnostics are available on `Health.Chart.NewComboProvenance`: original raw type,
file bit4/colour skip, exact registered definition identity, prior runtime category,
helper result, factory argument, constructor/parser flags, expired-break count and
final flag. Retained T081f/alternate objects are not admitted as ordinary rows.
The health status distinguishes E036 fresh grouping from historical R09 grouping.
See `../R10-original-newcombo-health-preparation.md` for static provenance, bounded
controls, compatibility scope and actual Windows verification.

Calibration engineering guards:10000 trials,2000000 object/component operations,
10000 objects and4096 breaks. Original M1150 has no fixed retry limit. The live
host supplies fill offset0 explicitly because original F0fc4's wider setting is
unrecovered. WPF uses rendering elapsed /nominal60Hz for host scale and Stopwatch
for gameplay milliseconds, a running-state proxy for F1374, full prepared starts
for break candidate clearing, and a pre-evaluation active-spinner interval proxy
for the original old-candidate selected-spinner flag. These substitutions are
visible in the health status; native scheduler/BASS/provider identity is unverified.
Break queue midpoint/end selection and inclusive markers are recovered operations
inside this bounded ordered domain, not general overlapping-map support.

Failure is a **state boundary**, not a new fail screen. A reached nonzero result
checks H after its mutation/record continuation. Passive drain alone does not call
failure; a positive result can revive a zero target before that check. No-mod
practice has zero recoveries. A first failed latch is shown while practice continues
because native cleanup/departure is unimplemented. Explicit component recovery
inputs support the recovered +160/fill/release boundary, without native playback
or mod behavior. Full fail/retry UI, HP restoration/seek, high-result decorations
and native effects remain excluded. See `../R09-original-health-system.md` for
source provenance, unsupported cases and actual Windows gate results.

## Circle HitSounds (R11)

Positive ordinary circles now select E037/E038 samples before score accounting.
Slider/spinner/head/tick audio is still outside this feature. Song MediaPlayer and
Stopwatch controls remain as before. **No original hitsound samples are bundled.**
Without optional assets, gameplay works and the footer reports missing effects.
The **Discrete HitSounds** checkbox enables/disables effects. **Effects setup** shows
the settings-file location. All playing/listening is exclusively human-controlled.

Create `hitsounds.json` beside `Ohno.CircleGame.exe` (for `dotnet run`, in the
project's `bin/Release/net8.0-windows/` output directory), then relaunch:

```json
{
  "Enabled": true,
  "LayeredHitSounds": true,
  "SkinSamples": true,
  "SkinName": "Default",
  "SkinDirectory": null,
  "DefaultPackDirectory": "C:\\MyIndependentSamples",
  "VolumeEffect": 80,
  "VolumeUniversal": 100
}
```

These are explicit **ENGINEERING practice settings**, not the owner's installed
osu! configuration. Volumes are integer0–100; global gain uses the recovered
integer product followed by Single division. Layered Normal needs the applicable
skin/map/mode predicate; mask0 always gains Normal. The supplied ordinary context
has a matching selected map, an independent default skin descriptor, mode0,
map suppression/process gates false and song rate100/no rate-match adjustment.
`SkinName: "Default"` skips loose-skin lookup. To admit your own skin directory,
set its explicit local path and a non-Default SkinName; SkinSamples must be true.
Settings/asset-root changes require relaunch; restart only resets playback/session.
Invalid configuration is reported and effects remain unavailable until corrected.

There are three supported sources:

- **Map directory:** standard bank-layer names with optional indexes, e.g.
  `normal-hitnormal.wav`, `soft-hitwhistle2.mp3`, `drum-hitclap7.ogg`.
  Standard search is WAV→MP3→OGG. Index0 skips the map provider; index1 is
  unsuffixed; index2 and greater append their decimal number.
- **Explicit skin directory:** unsuffixed standard names, WAV→MP3→OGG, admitted
  only under the settings above. Map resources take precedence.
- **Independent default pack:** twelve PCM WAV files, exactly
  `{normal,soft,drum}-{hitnormal,hitfinish,hitwhistle,hitclap}.wav`.
  This is an engineering file adapter for extensionless default resource keys,
  not a claim that original embedded resources were loose WAVs. No original DLL
  is loaded or extracted. The pack follows admitted map and skin resources.

For an optional original, copyright-free practice pack, run at repository root:

```powershell
./reconstructed/GenerateIndependentHitSounds.ps1 -PackDirectory C:\MyIndependentSamples
```

The script writes twelve independently synthesized120ms tones and prints suitable
JSON; it does not play audio. Copy that JSON to the output settings file above.
These tones demonstrate the interface and make no original sound-quality claim.
Use **Silent fixture**, Start/Space, real mouse and Z/X to hear circle effects;
its omitted/zero selectors inherit its timing sample record. All old fixtures and
controls remain. WAV is the most portable first choice; MP3/OGG map/skin/custom
files depend on Windows media codecs and may fail explicitly. Exact custom
filenames are supported; if absent, fallback is WAV→OGG→MP3, distinct from the
standard order. A declared existing custom file that cannot be read does not
trigger the extension loop. Non-audio formats/playlists are not supported.

Circle metadata keeps raw sounds and constructor bits2/4/8, sample-set/addition /
index/volume/filename overrides. Zero overrides inherit; volume0 is not explicit
mute. Timing lookup scans the supplied prepared order at **stored start+5**,
independent of hit time and inherited flag. The supported long timing rows include
an explicit volume (six fields minimum), set0→Soft2; shorter timing defaults need
unreconstructed original map/global operands and disable effects explicitly.
Malformed sample payloads remain labeled unavailable instead of being silently
coerced to zero. Gameplay remains usable; this is an engineering error-containment
policy rather than original parser exception equivalence. Existing slider timing
lookup and gameplay definitions are untouched. Shared custom/index resource
preparation includes registered object metadata; only circles emit playback.

Assets are prepared off the judgement path, then WPF asynchronously opens two
independent voices per resource before session replacement. Each positive circle
queues prepared requests in Normal→Finish→Whistle→Clap order. No judgement-time
file read/decoder open or song-clock seek occurs. Effects use gain/Balance /
SpeedRatio; current no-mod factor is1. A busy two-voice pool drops the extra voice
with a diagnostic. MediaPlayer/dispatcher latency, codec/device behavior and
native BASS equivalence are unverified. Opening/compilation does not prove sound.

Pause stops active effects and discards queued work; resume does not replay it.
Restart stops voices, detaches the old session and starts fresh judgement state
while reusing prepared assets. Replacement/exit close voices and map samples;
skin/default caches persist within the configured library until exit. Model APIs
expose separate map/skin/all invalidation and per-key eviction; cached misses are
not automatically rediscovered. Relaunch refreshes assets/configuration. Private
health calibration never emits sounds. Optional sink errors are contained so
score, combo, accuracy, judgement and HP proceed.

Local safeguards: relative candidate names within admitted roots, no network /
absolute custom paths, traversal or symlink/reparse-point paths;8MiB/file,
64 loaded resources/64MiB and64 dynamic indexes. Limits are engineering choices.
Native map-byte SampleCreate for inputs<1024 bytes is unsupported and represented
as logical invalid handle0 with a diagnostic, without invented sample bytes.
No `.osz2`, full skin manager, complete native cache/concurrency/scheduler/device
semantics or acoustic fidelity is claimed. Footer shows enablement, decoded and
unresolved selections, default-pack configuration, volume policy and errors.
See `../R11-original-circle-hitsounds.md` for actual Windows verification.

## Discrete slider HitSounds (R12)

Choose **Discrete R12** for the new mixed circle/out-and-back control, or use the
existing Tick/Repeat/Bézier fixtures. Start/Space, aim and press Z/X at the head,
then hold and follow the ball as before. The **Discrete HitSounds** checkbox now
controls ordinary circles plus embedded slider heads, interior ticks, repeat
edges and positive parent-aggregate edges. Continuous slider slide/whistle loops
and spinner sounds remain unavailable. All playing/listening is human-controlled.

For a fresh independent default pack, run both file-only generators:

```powershell
./reconstructed/GenerateIndependentHitSounds.ps1 -PackDirectory C:\MyIndependentSamples
./reconstructed/GenerateIndependentSliderTicks.ps1 -PackDirectory C:\MyIndependentSamples
```

If you already generated the twelve R11 circle WAVs, run only the second command.
It adds `normal-slidertick.wav`, `soft-slidertick.wav` and `drum-slidertick.wav`
without changing existing circle files. Both scripts refuse overwrites unless
explicitly forced. Point the same output `hitsounds.json` DefaultPackDirectory to
that directory and relaunch. The existing twelve-file pack remains valid for
circle/head/edge layers; missing ticks are reported rather than synthesized at
runtime. These original practice tones are independent of osu!stable assets.

Tick map/skin lookup uses the dedicated slidertick key, WAV→MP3→OGG and the same
provider hierarchy. Index0 skips map;1 is unsuffixed,2+ use decimal suffixes.
A dynamic miss/−1 falls back to the tick plane0; invalid0 remains distinct.
No circle Whistle/Finish/Clap expansion or minimum-volume floor applies to ticks.
They query timing at their selected mixed-schedule timestamp+2 and use its index /
volume. Edges query their recovered head/subset/end timestamp+5, use timing index /
volume and the existing standard layer order/floor. Parent index/volume/custom
fields are preserved but do not directly select tick/edge playback. Head receives
only selected edge0 base/addition, keeping its own zero index/volume and absent
custom handle. Explicit head base0 uses the format13/14 compatibility distinction.

The Discrete R12 fixture contains circle3000, two-traversal linear slider5000–8000
and circle10000. Its explicit edges select Whistle at head, Finish at reversal and
Clap at positive aggregate completion, with separate normal/soft/drum base/addition
banks. Interior deadlines5500/6000/7000/7500 select ticks; reversal6500 selects
edge1; final checkpoint7964 itself does not request an edge. These are finite
source-derived expectations, not observed audio. Results remain13000ms.

Audio progression is independent of checkpoint success/failure counts: one reached
update consumes only the latest due prefix entry before the final schedule item.
Tracking loss consumes without replay on reentry. Final edge comes from positive
aggregate, including failed terminal or pending-component cases. A fresh/reset
audio index is0; a reached pre-start update sets−1. Normal UI playback supplies
pre-start frames; direct component calls without that history can skip index0.
No scoring or judgement is rescheduled to align sounds.

Parent pan uses the **last ball X published by WPF drawing**, initialized at head X.
This engineering bridge deliberately does not substitute the pointer or a fresh
required-position query. It can lag the native same-update presentation operand
and is not original panning/update-order equivalence. The running host supplies
numeric audio state1 explicitly. R11 pause/disable/generation cancellation and
optional-failure containment remain; slider events carry separate identities.

Malformed EdgeSets/payload retain explicit unsupported-audio metadata while geometry
and gameplay stay usable (an engineering error-containment boundary). Invalid
EdgeSounds elements become0 through ignored TryParse, as recovered. Existing1–16
traversals, geometry, ordering, path/asset and clock safeguards remain. Slider sound
preparation requires fresh parser identity and strictly increasing registered
starts; no sorting fixes ambiguous registration. Parent custom stems colliding with
standard discrete keys or other custom stems explicitly disable preparation.
The parent loader loop flag is retained as metadata; no continuous voice is started.
Native continuing helpers, global shared channels, archive providers, BASS/codec /
device/onset precision and human-audible fidelity remain unverified or excluded.
See `../R12-original-discrete-slider-hitsounds.md` for actual CI evidence and scope.

### Continuous slider audio (R13)

Optional loops use the same hitsounds.json enabled/layering/effect/global settings.
After the existing circle and tick generators, add six independent loop tones:

```powershell
./reconstructed/GenerateIndependentSliderLoops.ps1 -PackDirectory C:\MyIndependentPack
```

Set DefaultPackDirectory to that local directory in hitsounds.json beside the app.
The generator preserves the other15 files and writes normal/soft/drum-sliderslide.wav
and normal/soft/drum-sliderwhistle.wav. User map/skin WAV→MP3→OGG resources and
indexed variants follow the existing hierarchy; codec availability is platform
specific. No original sample pack is supplied or required. Missing or rejected
loops leave gameplay and discrete effects available with continuous status.

R13 admits fresh format14+ finite supported timing and one continuous producer at
a time. Overlaps, ambiguous loop/custom cache stems, out-of-order timing and
unsupported selectors disable continuing effects. One shared logical channel pair
uses persistent preopened MediaPlayers, separate from one-shot voices. Gain/pan
are supported; no pitch or native scheduling claim. MediaEnded restarts the stream,
so gapless looping is not guaranteed. Pause, disable, restart and map replacement
cancel queued generations; resume needs a later eligible slider update. Parent pan
uses R12's last published ball X and may lag the native presentation state.

Compilation, headless recording checks and PCM headers do not prove playback,
loop quality or native BASS fidelity. The owner alone opens/plays/listens to the app.
See ../R13-original-continuous-slider-audio.md for exact-source CI and coverage.

Settings are read at application startup. Restart after changing hitsounds.json or
adding previously missing skin/default-pack resources: those shared resource-cache
identities persist across map replacement. Map-specific resources are released
with their prepared map; no file watcher or native device reinitialization is claimed.

### Spinner audio (R14)

Add two independently authored samples to the same optional default pack:

```powershell
./reconstructed/GenerateIndependentSpinnerSounds.ps1 -PackDirectory C:\MyIndependentPack
```

This preserves the earlier21 files and adds spinnerspin.wav (continuing) and
spinnerbonus.wav (one-shot). Local map/skin resources use those exact unsuffixed,
unprefixed names and WAV→MP3→OGG lookup. Positive completion uses the spinner's
standard/custom samples selected at its stored end time. No proprietary samples
are distributed. Missing resources or unsupported spinner metadata leave gameplay
usable and appear in effects status.

hitsounds.json also accepts SpinnerFrequencyModulate (default true) and
SpinnerModifierBits (default0, original numeric suppression operand without an
assumed descriptive name). The existing enabled/layering/effect/global volumes
and local directories apply. Settings are loaded at startup; restart after changes.

The Windows MediaPlayer backend supports nonmodulated looping spin, bonus and
completion effects when the resources decode. **Audible frequency modulation is
unsupported:** there is no verified native length/attribute1 mapping, and the app
does not invent pitch or speed changes. The managed frequency requests are tested
with injected lengths and recording channels only. MediaEnded looping may have
gaps. Frequency output, decoding, sound quality and synchronisation require future
owner-only listening qualification; automated checks play no audio.

R14 remains bounded to fresh format14 supported single-producer state. The spinner
channel is separate from the slider pair. Overlaps, seek/editor/reused-manager,
ambiguous cache collisions, device replacement and native BASS equivalence remain
unsupported or unverified. See ../R14-original-spinner-audio.md for evidence.

## R14-Q01 owner audio diagnosis

`hitsounds.json` is read from `AppContext.BaseDirectory`, beside the executable
actually running. It is optional: an absent file leaves the default pack
**unconfigured**. Files merely existing at `C:\MyIndependentPack` do not configure
it. Use `Effects setup` to see the precise settings path. The full pack has23 WAVs;
spinner audio specifically needs unsuffixed `spinnerspin.wav` and `spinnerbonus.wav`.
Map samples take precedence, then an admitted skin, then the independent default
pack. Previously missing default/skin resources stay cached across map reloads;
close and reopen the app after changing pack files or configuration.

`Save audio diagnostics` pauses gameplay and writes an owner-chosen JSON snapshot.
It includes effective settings/paths, optional-audio admission reason, per-resource
provider/key/logical handle and decoder readiness, spinner C/N/U at each last event,
separate bonus/completion requests, shared-channel/start/Pause/Stop counters,
dispatch/generation counts, last32 changed channel-operation transitions and media
errors. Counts are cumulative per prepared map and bounded in memory; no per-frame
file logging or sample buffers. The save action itself causes expected Pause and
cancellation entries. A logical Play or decoder-ready flag does not prove device
output. Paths in the report identify the local installation; review before sharing.

Start with `Spinner Practice` (format14, N15). Hold Z/X and rotate manually during
3–6 seconds: ongoing motion should request the spin loop. Progress16 can earn a300
and its ordinary completion tap; first normal bonus is **C20**, then22,24… . An end
tap alone does not prove a bonus was attempted. The footer separately shows spin
and bonus logical handles and decoder readiness. The current parser already rejects non14 spinners (and format15+ maps);
the separate audio admission is also exact14. Other optional-audio limitations
can leave a valid14 chart playable. Do not relabel a file as14 to bypass admission. Windows frequency modulation remains unsupported even when loops play.
See `../R14-Q01-spinner-audio-playback-diagnosis.md` for the reproducible procedure,
JSON example, diagnostic interpretation and exact verification evidence.

## R15 Rank / results

A supported fresh no-mod base-legacy session now enters a separate results screen.
It displays original enum grade X/S/A/B/C/D (silver XH/SH exist in the independently
callable getter), minimum-eight-digit score, Single-derived two-decimal accuracy,
four whole-result counts and maximum combo. The grade uses unrounded binary32
ratios compared to binary64 thresholds, not the formatted accuracy percentage.
Perfect text is shown only when the completed E035/R10 preparation supplies the
required combo scalar. Validity, success and perfect eligibility remain separate.

The host retains the existing gameplay kernel and B+3000 session end. A separate
finalization latch normally requires time strictly beyond maximum stored end+200;
Rank request, admission, commitment, audio/input cleanup, current-record binding,
widget initialization and presentation are distinct operations. The bounded host
uses stable nonzero clock state and an **empty unsupported storyboard-event
horizon**, with subsequent render callbacks committing/initializing/presenting;
this is an engineering coordinator, not the original transition/fade framework.
It defers finalization while kernel judgements are pending. No events are invented.

On results, **Space / Enter** or Reveal score skips the deterministic score reveal;
it does not retry. **R / Retry** creates a fresh gameplay/score/health/results
session on the selected chart. **Escape / Back** requests original SelectPlay5 and
lands on a clearly labelled engineering chart/fixture-selection state. Use the
existing Open/fixture controls there, or Space to retry the selected chart.
Gameplay's prior R/Enter/Space controls retain their meanings during gameplay.

The screen uses independent WPF typography/shapes. The mutable selected record is
retained, but visible text/grade are captured once at results initialization.
Reveal uses its own Stopwatch host clock, start+300, elapsed/500 prefix and dot
placeholders; numeric score never changes. Native random digits, textures/fonts,
grade pulses, graph, player/date, decorated300g/100k, replay, online/submission and
Rank/interface audio are unsupported. No applause is inferred from a good grade.

Admission requires exact fresh parser identity, ordinary/no-mod/no-source/base
record policy and source-row count **below7000**. E023 automatically enables
ScoreV2 at7000; R15 marks this unsupported without changing existing gameplay or
legacy score arithmetic. Failure-latched or unavailable HP/success provenance
likewise does not claim normal Rank entry. A bounded unavailable panel offers
Back/Retry. Perfect eligibility can be unavailable independently of grade.
This work does not correct the deferred Joined Bézier score discrepancy.
See `../R15-original-derived-rank-results.md` for provenance, verified counts,
engineering substitutions and the78-row E042 coverage matrix. Windows source is
built/tested without launching the gameplay window, listening or screenshot claims.
