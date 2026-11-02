# R04 — First Slider Practice for Windows

A minimal desktop osu!standard reconstruction using **C# / WPF / .NET 8**.
Move a real mouse and physically press **Z or X** to hit circles. No autoplay,
simulated input, network service, original assets or original-game execution.

**R04 status:** see `../R04-first-slider.md` for Windows CI build and existing
90-case regression outcomes. Human gameplay, visuals, input and playback remain
unverified by Work. No game window is operated by an agent.

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
   For the mixed circle/slider control, choose **Slider fixture** instead.
2. Click **Start** or press **Space**. The first circle is due at 3 seconds.
3. Aim at the circle with the mouse. When its shrinking ring reaches the circle,
   press **Z** or **X** once. Holding a key does not repeatedly hit objects.
   For sliders, press at the head, keep Z or X held, and move with the ball.
4. Read the 300 / 100 / 50 / MISS feedback and the combo/accuracy HUD.
5. After the last circle, the result overlay appears at 16.5 seconds.
6. Press **R** or **Enter**, or click **Restart**, to play again immediately.

**Space** pauses/resumes. Losing window focus pauses without automatically
resuming; press Space after returning. Mouse clicks focus the playfield but do
not score. **Open .osu** pauses and opens a local file picker; cancellation leaves
the old session paused. A rejected file leaves the old map, audio and progress
intact. A newly loaded map waits for Start. **Silent fixture** restores the
original silent control. All interactive operation remains human-controlled.

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

R04 explicitly uses the recovered **legacy, no-ScoreV2** aggregate: positive head
and successful terminal →300; exactly one successful component →100; neither
→MISS. A 50-quality positive head can therefore still yield300 with a successful
terminal. Head and terminal feedback remain separate. Head success increments
practice combo and head failure resets it; terminal success increments combo,
while terminal failure follows E22's no-reset branch. Aggregate display does not
award a duplicate combo increment. Accuracy counts one completed parent object,
not head+tail as separate circles. This is not stable's complete scoring pipeline.

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

Run/publish commands are human Windows instructions. R04 uses noninteractive
Windows CI for compilation and the unchanged component suite; see the report.
No original osu! installation is required. If a launch fails, run the command in
PowerShell to retain the error output. Source, fixture and Windows instructions
are committed; build outputs are intentionally ignored.

## Accepted maps and limits

- `.osu` format v5–v14; explicit `Mode: 0`, CircleSize, OverallDifficulty and
  ApproachRate, with finite difficulty values in [0,10].
- Ordinary circles and `L|x:y,1,length` straight sliders, at distinct integer start
  times in [0,3600000] ms. Normal/NewCombo/colour-skip bits are accepted. Repeats,
  multi-point paths, other curves, interior ticks, overlapping slider intervals,
  spinners, holds, equal times and malformed/empty maps are explicitly rejected.
- Slider maps require explicit SliderMultiplier and SliderTickRate, clamped to
  E05's [.4,3.6] / [.5,8] ranges. Timing points are retained in file order; base
  and inherited selection at slider start drive travel rate and spacing. Missing
  timing uses E05's rate fallback. Changes during a slider do not change its rate.
- Nondegenerate slider length must be >.0001 and <=4096 pixels; duration >=1 ms,
  end <=3600000 ms. Maximum 2 MB / 10000 objects. These are slice input limits.
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
| `Gameplay/Beatmap.cs` | Narrow circle/slider reader, timing and prepared straight geometry |
| `Gameplay/CircleSession.cs` | Separate circle/head/slider state, held-side history, tracking, terminal and aggregate feedback |
| `Rendering/PlayfieldLayout.cs` | Forward/inverse layout relation inside a 4:3 stage |
| `Rendering/PlayfieldView.cs` | Primitive drawing, frame callback, actual mouse sampling |
| `Fixtures/first-circles.osu` | Independently authored eight-circle practice input |
| `Fixtures/audible-circles.osu` | Separate template paired with generated tones |
| `Fixtures/first-slider.osu` | Independent mixed circle/slider control, no interior ticks |

The gameplay layer has no WPF dependency. Rendering consumes session state;
platform input supplies a mapped pointer and stored gameplay time. R01's Python
kernel and tests remain intact and are not artificially wired into this slice.

The implementation report separates research-derived rules from deliberate
slice policies and records unresolved original-equivalence dependencies.

API reference: [Microsoft WPF MediaPlayer](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.mediaplayer?view=windowsdesktop-8.0).
