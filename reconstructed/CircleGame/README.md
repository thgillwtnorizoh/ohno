# R02 — Circle Practice for Windows

A minimal desktop osu!standard reconstruction using **C# / WPF / .NET 8**.
Move a real mouse and physically press **Z or X** to hit circles. No autoplay,
simulated input, network service, original assets or original-game execution.

**Status:** implementation/build status is recorded in `../R02-first-circle.md`.
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

1. The independent eight-circle fixture loads into a ready screen.
2. Click **Start** or press **Space**. The first circle is due at 3 seconds.
3. Aim at the circle with the mouse. When its shrinking ring reaches the circle,
   press **Z** or **X** once. Holding a key does not repeatedly hit objects.
4. Read the 300 / 100 / 50 / MISS feedback and the combo/accuracy HUD.
5. After the last circle, the result overlay appears at 16.5 seconds.
6. Press **R** or **Enter**, or click **Restart**, to play again immediately.

**Space** pauses/resumes. Losing window focus pauses without automatically
resuming; press Space after returning. Mouse clicks focus the playfield but do
not score. **Open .osu** pauses and opens a local file picker; cancellation leaves
the old session paused. A rejected file leaves the old map intact. A newly loaded
map waits for Start. The app runs silent even when a map names audio.

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

The self-contained publish command is guidance, not a claim it was run for R02.
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
- Difficulty keys are read; combo colours, hit samples, audio, timing-point
  effects, events and other sections are not consumed. There is no full stable
  parser, old-version offset handling, stacking, HP/fail policy or mod support.

## Source layout

| File | Responsibility |
|---|---|
| `Program.cs` | Window, file picker, real keyboard edge handling and focus policy |
| `Platform/GameplayClock.cs` | Pausable Stopwatch clock for silent practice |
| `Gameplay/Beatmap.cs` | Narrow local circle-only reader |
| `Gameplay/CircleSession.cs` | Timing rules, ordered spatial selection, judgement, expiration, feedback counters |
| `Rendering/PlayfieldLayout.cs` | Forward/inverse layout relation inside a 4:3 stage |
| `Rendering/PlayfieldView.cs` | Primitive drawing, frame callback, actual mouse sampling |
| `Fixtures/first-circles.osu` | Independently authored eight-circle practice input |

The gameplay layer has no WPF dependency. Rendering consumes session state;
platform input supplies a mapped pointer and stored gameplay time. R01's Python
kernel and tests remain intact and are not artificially wired into this slice.

The implementation report separates research-derived rules from deliberate
slice policies and records unresolved original-equivalence dependencies.
