# 1. Result

**R02 — First Human-Playable Circle**, Work 2, 2026-11-02 (Asia/Bangkok).

Delivered a C# WPF Windows desktop application under `CircleGame/`, containing
an independently authored eight-circle `.osu` fixture, actual mouse sampling and
physical Z/X key-down handling, a pausable practice clock, scheduled circle and
approach-ring drawing, hit/miss judgement, expiration, combo/accuracy feedback,
result overlay, restart and a local file picker. No autoplay or synthetic input.

**Built for Windows x64; Windows launch / human play remains UNVERIFIED.**
This is meaningful application implementation progress, not a proven playable
Windows release. Work 1 owns subsequent independent QA; stop after R02.

## Preparation, ownership and actual verification

Started from current `origin/main` at
`38dee7561569210f25c49f9eb6810bc0a06f634b`, matching the assignment. Read AGENTS.md,
the full re/Instruction.md, the existing reconstruction journal and R01 source,
and relevant E04, E07–E14, E20 and E27 findings/pseudocode. The E05 correction of
E04's Taiko mapping is retained via the later ordinary-circle reports. No original
archive/binary was inspected, no research tools or original experiments were run.

Actual development-host build commands (Linux, .NET SDK 8.0.427):

```sh
/tmp/ohno-dotnet/dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release --nologo
/tmp/ohno-dotnet/dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
```

Both builds **SUCCEEDED, 0 warnings / 0 errors**. The Windows-targeted output
contains `Ohno.CircleGame.exe`, managed assembly/dependency/runtime files and
`Fixtures/first-circles.osu`. `file` identifies the generated apphost as a
PE32+ Windows GUI executable for x86-64. This examines our own generated output,
not original software. The first build used the host apphost; the second
explicitly produces the Windows apphost.

Minimal launch attempt:

```sh
/tmp/ohno-dotnet/dotnet reconstructed/CircleGame/bin/Release/net8.0-windows/win-x64/Ohno.CircleGame.dll
```

**BLOCKED before window creation, exit 150:** missing
`Microsoft.WindowsDesktop.App` on Linux. No DISPLAY/WAYLAND session or Wine was
available. No window, real mouse/key interaction, DPI behavior, human timing or
result overlay was observed on this host. No self-contained publish was run.
The Windows launcher `.cmd` is provided but was not run here.

No automated tests, dedicated QA harness or corpus were developed or run. R01
source and its existing tests are preserved. Scope/diff and publication-integrity
checks are repository hygiene, not gameplay QA. Windows build/run instructions
and controls are in `CircleGame/README.md`; compiled outputs are ignored.

The current assignment explicitly authorizes actual playable-software work,
removes original-runtime experiments as a prerequisite and assigns dedicated
QA/testing to Work 1. It supersedes historical research-only suspension and
milestone-branch arrangements. Documentation/journal changes remain under
`reconstructed/`; no `re/` or root policy files are modified. This does not waive
evidence honesty or authorize claims that E20 passed. No other exception is used.

# 2. What they found

## Implementation and evidence traceability

Research confidence is inherited: CONFIRMED static instruction findings with
RECONSTRUCTED semantic projections. R02 does not independently verify the
original or promote E20's NOT RUN cases to observed behavior.

| Application component | Research contract used | R02 boundary |
|---|---|---|
| `Gameplay/Beatmap.cs` | E04 object columns, clamp both coordinates to [0,512], truncate, start=end for circles; later reports correct ordinary identity | Narrow standard-format reader; explicit Mode/CS/OD/AR; no full stable parser or map offset |
| `CircleSession` approach | E07 M28be/M28b3 finite no-mod 1800/1200/450 mapping, int truncation | Does not reproduce E07 candidate BinarySearch/range storage |
| Face/approach cues | E08 M45ec: 400-unit face fade, approach 4→1; approach alpha 0→0.9 over min(A,800); untouched fade between S+W100 and S+W50 | WPF circles/text, independent palette, no skin/transform engine; judged objects disappear immediately |
| Drawing backend | E09 distinguishes prepared geometry from displayed pixels | Independently chosen WPF DrawingContext; no GL/batch/shader equivalence claim |
| Stored clock/input | E10 clock/update boundary; E11 consumes stored T, not event timestamp | Stopwatch milliseconds rounded to int; latest frame time passed to key handler; no BASS/audio drift/offsets or stable scheduler |
| Live input | E11 edge/action boundary and no judgement from dispatch alone | WPF PreviewKeyDown/KeyUp for physical Z/X, repeat suppression; actual Mouse.GetPosition; no mouse-click scoring |
| Ordered selection/admission | E11 M28c9/M24c7: inclusive [S-A,S+W50], first disk hit, evaluated exclusion; earlier unjudged end+3 gate and strict abs error<400 | Full-list ordered scan, simple earlier-object block; opaque predecessor F252d and special/provider/mod gates omitted |
| Circle outcomes | E12 M1884 strict W300/W100/W50; M28e4 timeout strictly T>S+W50 | Named UI outcomes rather than original raw/flagged enum; timeout scans all pending objects to catch up after host stalls |
| Input radius | E13 M28be scale-one formula and factor 1.0004099607467651f; ordered disk test | Radius prepared at logical W512/H384/s1 and scaled primitives; no x86 CLR bit-identity claim |
| Position alignment | E13 inverse and E14 forward map, normal q=1 relation | Letterboxed 4:3 WPF DIP stage; no original viewport/provider/scissor/skin/DPI reconstruction |
| Result time | E27 B+3000 departure horizon is distinct from finalization | Local result overlay at last circle+3000, not original Rank request/commit/record/grade pipeline |
| Evidence limits | E20 preparation-only, all 17 runtime cases NOT RUN | Windows launch/human play delegated to Work 1; no new original-equivalence claim |

The supplied fixture uses CS4/OD4/AR4 and separated circle times 3000–13500 ms.
It is independent practice data with no copyrighted asset/audio dependencies,
not an imported or original-validated stable beatmap. The app reads it using
the same local reader as a user-selected map. No hardcoded fixture outcomes.

## Explicit slice policies and known deviations

- WPF CompositionTarget drives repaint/update; Stopwatch is independent of audio.
  No audio/video/storyboard/network services, sliders, spinners, mods, skin assets,
  HP/fail behavior, replay input, stacking or main-menu reconstruction.
- The UI waits for Start, freezes on focus loss/pause and resumes explicitly.
  R/Enter makes a fresh session at time zero. These are engineering policies,
  not recovered stable lifecycle/seek behavior. No automatic judgement on restart.
- A fixed-aspect stage is fitted inside the resizable window in WPF device-
  independent units. Within it, s=height/480; O.X=(width-512*s)/2 and
  O.Y=3*(height-384*s)/4-16*s. Draw=C*s+O and input=(Q-O)/s. Physical DPI
  transforms are left to WPF; original native mapping equivalence is unverified.
- Primitive body radius uses the scale-one diameter/2; accepted disk uses the
  separately researched radius multiplier. Circles may be hit independently of
  opacity. Object numbers are ordinal labels, not stable combo-number recovery.
- Full-list scanning and stored UI-frame timestamps are deliberate small-slice
  choices. Event ordering, stalls, candidate attention, close overlaps and early
  admission can differ from stable. Distinct equal-time rows are rejected rather
  than inventing tie/stack ordering. The supplied separated fixture avoids these
  dependencies. Parser limits and required explicit keys are documented.
- Visible combo/max-combo and weighted accuracy are simple practice counters.
  No stable score, manager group flags, grade, health or complete score-record
  pipeline is claimed. Latest judgement is shown briefly; hit animation is not
  recovered here. End overlay keeps the window open for replay.

## Handoff and remaining dependency

Work 1 should build/launch on Windows and physically play the fixture, checking
ready→start→circle→Z/X→feedback→result→restart, focus/pause, file-picker rejection,
and high-DPI/resizing/input alignment. Those checks are **NOT RUN** by Work 2.
No dedicated test system is supplied as a substitute. Future fidelity questions
are original candidate/admission gates, audio-backed clock, stacked/overlapping
maps, hit animations, exact score/record/finalization and native precision.
Do not block this source milestone on reproducing original runtime experiments,
and do not call its currently unobserved Windows UX verified playable.

# 3. Pseudocode

RECONSTRUCTED finite circle rules; ENGINEERING host/presentation choices:

```text
load bundled or locally chosen circle-only .osu
prepare approach interval, strict judgement widths and logical radius
wait for human Start
on each WPF frame while running:
    publish rounded Stopwatch practice time
    expire each pending circle only when T > start + W50
    render primitive face, number and shrinking ring at forward-mapped position
    show latest feedback and practice counters
    at last start + 3000: pause clock; show local results
on physical Z/X key-down edge, while running:
    sample actual mouse in playfield surface
    inverse-map to logical coordinates
    scan pending circles in start order, inclusive time bounds then disk test
    apply supported earlier-object/admission conditions
    classify abs(T-start) using strict 300,100,50 thresholds; otherwise miss
    mark evaluated and update practice feedback/counters once
on focus loss or Space: freeze clock; require explicit resume
on R/Enter/restart button: new session and restarted clock
```

UNRESOLVED original dependencies stay outside the implementation scope; the
host, primitives, pause policy and results overlay are not claimed as stable's
complete mechanisms.
