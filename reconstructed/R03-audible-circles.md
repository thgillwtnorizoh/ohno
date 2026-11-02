# 1. Result

**R03 — Audible Circle Practice**, Work 2, 2026-11-02 (Asia/Ho_Chi_Minh).

Started from latest `origin/main` at
`2c569937fde3e258dd703ead043daa05e78e4b52`. Read AGENTS.md, the standing contract,
implementation journal, QA01 README/headless source and corresponding R02
application source. No original-software expedition, binary inspection or
original-game operation. All application changes stay under `reconstructed/`.

Implemented in source:

- QA01-D01: ordered squared-distance acceptance; NaN cannot select a circle,
  while finite exact radius equality still passes.
- QA01-D02: column 5 hitSound must parse as an invariant Int32; empty/malformed
  input produces a line-specific InvalidDataException. No sound-mask restriction.
- Optional AudioFilename resolved relative to the selected map's directory.
- Native WPF MediaPlayer preparation/play/pause/resume/restart/close lifecycle.
- Candidate media loads asynchronously before replacing the paused old session;
  errors/cancellation/timeout preserve that previous map and its progress.
- Audible fixture button and independent PCM tone generator, alongside the
  unchanged silent fixture. No third-party audio assets or extra audio packages.
- Active Windows README updated; this report and R03 journal entry added. R01,
  historical R02 report, Work 1 research and QA assets are preserved.

## Actual execution versus source inspection

| Activity | Actual outcome |
|---|---|
| Source inspection | D01/D02 corrections and audio lifecycle reviewed; not executed behavior |
| Windows x64 compilation | **NOT RUN — .NET SDK unavailable** |
| QA01 headless C# suite | **NOT RUN — no pre-existing SDK**; tests unmodified |
| Windows GUI or audio playback | **NOT RUN / UNVERIFIED**, exclusively human-controlled |
| Audible WAV generation | Generator supplied in C#; **NOT RUN** on this host |
| Dedicated QA/test development | None; no new automated test campaign |
| Git whitespace/scope/preservation | Reviewed before publication; final handoff records verified remote status |

`dotnet` was absent from PATH, `/tmp/ohno-dotnet/dotnet` was absent, and bounded
local SDK-path checks found no compiler. An SDK download attempt was rejected by
automatic approval review because it conflicted with the existing-SDK and
environment-repair limit. No workaround, indirect SDK execution, OS installation,
VM, desktop setup, GUI launch, simulated input, OBS or CDB followed. This milestone
does **not** reuse R02's historical successful build as a result for changed code.

This is an **uncompiled, playback-unverified source delivery**, not a proven
audible/Windows-playable build. No compilation or runtime acceptance is claimed.

Windows instructions, not commands executed here:

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false
dotnet run --project reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release
```

Requires the .NET 8 SDK on Windows (or .NET 8 Windows Desktop Runtime for an
already compiled framework-dependent executable). See `CircleGame/README.md` for
controls, file loading and the independent audible fixture.

The user authorizes implementation and source-only status reporting while
reserving all interactive gameplay for the human owner and dedicated QA for
Work 1. R03 creates no RE number and does not edit the shared research journal.
No additional contract exception is used.

# 2. What they found

## Defect traceability

**CONFIRMED SOURCE corrections, not executed QA passes:**

| Finding | Source change | Preserved behavior |
|---|---|---|
| QA01-D01 / E13 ordered comparison | `CircleSession.Press`: reject when `!(distanceSquared <= HitRadius * HitRadius)` | Finite inclusive boundary; existing timing/admission/classification/expiration untouched |
| QA01-D02 / mandatory field syntax | `Beatmap.Load`: `int.TryParse(row[4], NumberStyles.Integer, InvariantCulture, out _)` | Ordinary valid rows; any representable signed Int32 accepted without invented bit mask |

NaN's unordered comparison is rejected by the explicit positive condition;
infinity from a pointer with finite prepared radius is also rejected. This is a
correction against QA01's established finding, not new original-binary research.
No claim that the original parser throws for malformed hitSound is added.
QA01 tests/provenance/results remain historical and unchanged; their old results
must not be relabelled as current R03 outcomes.

## Local audio lifetime

`Platform/BeatmapAudio.cs` owns one MediaPlayer on the WPF dispatcher thread.
Its implementation uses the documented WPF MediaOpened/MediaFailed/MediaEnded,
HasAudio, Position, Play/Pause, Stop and Close API; API reference:
[Microsoft MediaPlayer](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.mediaplayer?view=windowsdesktop-8.0).
This is an engineering backend choice, not a reconstructed BASS implementation.

- Missing/blank AudioFilename produces a silent prepared audio owner with no
  native player. The existing silent fixture is unchanged and still the default.
- Declared audio must be relative/local, lexically remain inside the map folder,
  and name an accepted audio extension. Absolute/network/URI/escaping paths and
  playlists are rejected. These are slice input policies, not original syntax.
  Decoder support is determined by Windows, not by an extension alone.
- GameWindow pauses the old session, disables gameplay controls and prepares a
  separate candidate. Awaiting MediaOpened never commands Play. A 15-second
  timeout, missing file, unsupported decoder, absent audio stream or load error
  disposes the candidate. The existing session/audio/progress remain paused;
  explicit Space can resume them. Successful load waits for human Start.
- Only a ready, current, noncancelled candidate can transfer ownership to the
  playfield. A superseded operation cannot commit stale state. Replacement closes
  the former player after the new model/media have passed preparation.
- Closing cancels pending loading and synchronously disposes both pending and
  active audio, without relying on an await resuming after dispatcher shutdown.
  Event handlers are detached; Stop/Close are attempted even after decoder errors.
- MediaFailed during play stops/releases that media, pauses the practice clock
  and reports the problem. The human can reload or choose Silent fixture. If a
  decoder fails after a successfully committed replacement, the former disposed
  map is not resurrected. No automatic silent fallback hides an audio error.

All of these lifecycle statements describe source design; Windows decoder
behavior, actual file-handle release, async event ordering and audible output
remain unobserved here. Shutdown errors are contained and written to Debug output.

## Clock authority and player controls

**ENGINEERING POLICY:** Stopwatch remains the practice-time authority. Its rounded
integer time is published on WPF rendering frames, just as in R02. The geometry
fix is the only CircleSession change; judgement windows and calculation ordering
remain deterministic against the supplied integer time. Media position does not
modify the clock or add an offset.

Start/restart prepares a fresh session, seeks audio to zero, commands Play, then
restarts Stopwatch. Pause/focus loss stops Stopwatch and commands media Pause;
resume commands Play and resumes Stopwatch. Physical Z/X input, real pointer
sampling, restart keys, local picker, visual cues and result feedback stay in
their existing UI paths. No OS input is generated by the implementation.

The control commands share a UI action boundary; **audible sample output is not
guaranteed simultaneous with clock zero**. Opening, seeking, device buffering,
resume and decode latency can differ. Drift/buffering stalls can accumulate
because there is no audio-position correction, device calibration or BASS-clock
reconstruction. No precise synchronization claim is made.

A short track's MediaEnded event marks audio ended but leaves the timeline
running silently to the existing last-circle+3000 result horizon. Pause/resume
does not restart an ended track. Explicit restart rewinds it. A longer track
pauses when that result overlay is reached. Replacing/closing releases resources.

## Independent audible fixture

`Fixtures/audible-circles.osu` uses the same separated circle deadlines as the
silent practice map, with AudioFilename `practice-beat.wav`. The **Audible fixture**
button invokes `Platform/PracticeFixture.cs`: create a unique temp directory,
copy the independent template, and write a complete 18-second mono 22050 Hz,
16-bit PCM RIFF/WAV. Enveloped 440 Hz guide ticks occur every half second;
880 Hz ticks coincide with the eight circle deadlines from3.0 through13.5 seconds.

The generator is the supplied reproducible artifact; no actual WAV is committed
or claimed generated on this host. There is no imported commercial music or
original osu! asset. Its synthetic tone output is dedicated under **CC0-1.0**.
Cross-runtime byte-for-byte transcendental-math output is not certified.
Temporary fixture files are deleted on exit on a best-effort basis, after media
closure; delayed decoder file handles can leave an `ohno-r03-*` temp folder.

Remaining limits include R02's simplified candidates/admission, primitive
rendering, no sliders/spinners/mods/HP/stable scoring, Windows codec variability,
uncompiled R03 source and all real playback/input/latency behavior. Stop after
R03; Work 1 owns the next dedicated QA and the human owns all interaction.

# 3. Pseudocode

CONFIRMED SOURCE correction; ENGINEERING audio/control design:

```text
circle admission:
    require ordered(distanceSquared <= radiusSquared)
object row:
    require mandatory hitSound column parses as signed Int32; no bit-mask limit

load selected map:
    pause current clock/audio; keep session
    parse map and local AudioFilename
    create separate candidate media; open asynchronously without playing
    await ready audio stream, or fail/cancel/timeout
    if failure: close candidate; retain old session paused
    if still current and ready: transfer candidate, then close old media

human start/restart: seek audio zero + Play; restart practice clock/session
human pause or focus loss: pause clock and audio
human resume: Play unless audio already ended; resume practice clock
media ended: keep timeline running silently
playback failure: close failed media; pause clock and report
results: pause both
map replacement/exit: cancel pending work, detach events, Stop/Close owned media
```

UNVERIFIED: compilation, Windows playback/event ordering, perceptual alignment,
physical input and original-game equivalence. No automated or original-runtime
results are manufactured from the source design.
