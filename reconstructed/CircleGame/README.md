# R03 — Audible Circle Practice for Windows

A minimal desktop osu!standard reconstruction using **C# / WPF / .NET 8**.
Move a real mouse and physically press **Z or X** to hit circles. No autoplay,
simulated input, network service, original assets or original-game execution.

**R03 status: source implemented; compilation and playback UNVERIFIED on this
host.** No SDK is available for this milestone's build/headless checks; no GUI or
audio was operated. See `../R03-audible-circles.md`. The R02 build result remains
historical and does not certify the modified R03 source.

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
2. Click **Start** or press **Space**. The first circle is due at 3 seconds.
3. Aim at the circle with the mouse. When its shrinking ring reaches the circle,
   press **Z** or **X** once. Holding a key does not repeatedly hit objects.
4. Read the 300 / 100 / 50 / MISS feedback and the combo/accuracy HUD.
5. After the last circle, the result overlay appears at 16.5 seconds.
6. Press **R** or **Enter**, or click **Restart**, to play again immediately.

**Space** pauses/resumes. Losing window focus pauses without automatically
resuming; press Space after returning. Mouse clicks focus the playfield but do
not score. **Open .osu** pauses and opens a local file picker; cancellation leaves
the old session paused. A rejected file leaves the old map, audio and progress
intact. A newly loaded map waits for Start. **Silent fixture** restores the
original silent control. All interactive operation remains human-controlled.

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
  map's existing last-circle+3000 result horizon. Resume does not replay an ended
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

Build/run/publish commands are Windows instructions, not commands executed for R03.
No original osu! installation is required. If a launch fails, run the command in
PowerShell to retain the error output. Source, fixture and Windows instructions
are committed; build outputs are intentionally ignored.

## Accepted maps and limits

- `.osu` format v5–v14; explicit `Mode: 0`, CircleSize, OverallDifficulty and
  ApproachRate, with finite difficulty values in [0,10].
- Ordinary circles only, at distinct integer start times in [0,3600000] ms.
  Normal/NewCombo/colour-skip type bits are accepted. Sliders, spinners, holds,
  equal-time objects, malformed rows and empty maps are rejected with a message.
- Maximum 2 MB / 10000 circles is an implementation input limit, not a stable rule.
- The included map is the intended control: separated, unstacked, no mods or
  timing/coordinate tricks. General dense/overlapping maps are not certified.
- Mandatory hitSound column 5 must parse as an Int32 (negative/unusual bit values
  are not mask-restricted); hit-sample playback is still unimplemented.
- Difficulty keys and AudioFilename are read; combo colours, hit samples,
  timing-point effects, events and other sections are not consumed. There is no full stable
  parser, old-version offset handling, stacking, HP/fail policy or mod support.

## Source layout

| File | Responsibility |
|---|---|
| `Program.cs` | Window, file picker, real keyboard edge handling and focus policy |
| `Platform/GameplayClock.cs` | Practice clock authority for both silent and audible maps |
| `Platform/BeatmapAudio.cs` | Asynchronous local-media loading and native playback lifetime |
| `Platform/PracticeFixture.cs` | Independent reproducible PCM tone generator |
| `Gameplay/Beatmap.cs` | Narrow local circle-only reader |
| `Gameplay/CircleSession.cs` | Timing rules, ordered spatial selection, judgement, expiration, feedback counters |
| `Rendering/PlayfieldLayout.cs` | Forward/inverse layout relation inside a 4:3 stage |
| `Rendering/PlayfieldView.cs` | Primitive drawing, frame callback, actual mouse sampling |
| `Fixtures/first-circles.osu` | Independently authored eight-circle practice input |
| `Fixtures/audible-circles.osu` | Separate template paired with generated tones |

The gameplay layer has no WPF dependency. Rendering consumes session state;
platform input supplies a mapped pointer and stored gameplay time. R01's Python
kernel and tests remain intact and are not artificially wired into this slice.

The implementation report separates research-derived rules from deliberate
slice policies and records unresolved original-equivalence dependencies.

API reference: [Microsoft WPF MediaPlayer](https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.mediaplayer?view=windowsdesktop-8.0).
