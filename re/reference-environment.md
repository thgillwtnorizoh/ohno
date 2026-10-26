# Reference environment — Expedition 01

Date: 2026-10-26 (Asia/Bangkok). Owner: Work 1. Layer A.
This is a static baseline and a pending execution protocol, not a gameplay trace.
Contract: [Instruction.md](Instruction.md), copied byte-for-byte from the supplied attachment.

## Evidence and reproducibility

| Evidence | Direct result | Status |
|---|---|---|
| `evidence/01/archive-audit.json` | 18 entries; all CRC checks pass; 99,395,816 uncompressed bytes | CONFIRMED |
| Updater config | `_ReleaseStream = Stable40`; 16/16 declared MD5 values match bytes | CONFIRMED as config/file agreement only |
| Executable PE/CLR metadata | I386 `0x14c`, PE32 `0x10b`, CLR flags `3` (ILONLY and 32BITREQUIRED), metadata `v4.0.30319` | CONFIRMED as static declarations |
| Executable version resource | FileVersion and ProductVersion `1.3.3.8`; assembly metadata `0.0.0.0` | CONFIRMED as embedded metadata, not dated release identity |
| Certificate directory | Offset 4,526,080, size 15,648 bytes | CONFIRMED presence; signature trust NOT tested |
| Exact dated stable release | No observed in-game version; metadata alone insufficient | UNRESOLVED |
| Clean/modified installation | Internal hashes agree, but config and payload have no independent trusted provenance | UNRESOLVED; do not call clean or modified |
| Corpus | No `.osu`, `.osz`, `.osr`, Songs directory, user options config, logs or captured frames in ZIP | CONFIRMED absence in supplied archive only |

Archive SHA-256: `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`.
Archive size: 78,546,961 bytes.
Executable SHA-256: `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.
Contract SHA-256: `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9`.
Every member's SHA-256/MD5 and all config values are in the audit JSON. MD5 is used only to compare the supplied updater entries; SHA-256 identifies research inputs.
Do not derive a release date from PE timestamps, signing timestamps, or the copyright string.
The names `osu!gameplay.dll` and `osu!ui.dll` do not establish runtime responsibility; no such mapping was tested.

Repeat the read-only audit with Python 3:

```sh
python -m pip install pefile==2024.8.26 dnfile==0.18.0
python re/tools/audit_reference.py /path/to/osubackup.zip > /outside/repo/audit.json
```

Compare the JSON with committed evidence. The audit executes no reference code and extracts no assets.

## Available execution environment

`evidence/01/host-capabilities.json` records Linux x86_64, kernel 6.18.55, no DISPLAY or WAYLAND_DISPLAY, no `/dev/dri` or X11 socket directory. No wine, wine64, Xvfb, dotnet, PowerShell or Windows session was available through this workspace. ffmpeg is installed; its presence does not establish a game capture capability. No reference launch or gameplay capture was attempted. This Linux host is not the reference Windows environment. Installing Wine would not establish native Windows equivalence.

| Required reference condition | Current status | How to resolve |
|---|---|---|
| Windows edition/build | UNRESOLVED | Windows CIM collector and `winver` screenshot |
| GPU/driver and active adapter | UNRESOLVED | Collector plus display/driver evidence |
| Active display resolution, scaling/DPI, refresh | UNRESOLVED | Windows Display and Advanced display screenshots; CIM fields alone insufficient |
| Window/fullscreen mode, compatibility mode, limiter | UNRESOLVED | In-game options screenshot plus generated user config |
| Dim, storyboard/video enablement, skin and asset state | UNRESOLVED | In-game options, selected skin inventory and dependency hashes |
| Audio device, sample configuration, universal/local offsets | UNRESOLVED | Windows audio/game settings; use same device across repetitions |
| Input device, key bindings, raw input, sensitivity, mouse buttons | UNRESOLVED | Record actual settings/device; no defaults assumed |
| Mods, ruleset, fail behaviour, autoplay state | UNRESOLVED | Record actual selection; planned control uses standard, Auto only |
| Capture source, resolution, fps, audio and timestamps | UNRESOLVED | Configure/test recorder, preserve recording metadata and dropped-frame log |

`osu!.cfg` contains updater fields, not these user gameplay settings. Do not substitute assumed defaults.

## Control and stress inputs

No supplied map can be selected. A project-original candidate control is reproducible with:

```sh
python re/tools/make_entry_control.py /fresh/output/entry-control
```

It generates standard-mode v14 text with circles at 2,000/3,000/4,000 ms, a 500 ms timing point and six seconds of mono 44,100 Hz, 16-bit PCM silence. No custom skin, background, video or storyboard is specified. These are fixture definitions, not measured stable behaviour. Import, WAV support, object scheduling, mod handling and exit behaviour are UNRESOLVED until tested. Audio silence cannot reveal audible clock synchronization; this fixture is for entry/exit only.

| Generated input | Bytes | SHA-256 |
|---|---:|---|
| `entry-control.osu` | 707 | `dc4112acfcb0e7f345c6350a0cf1640d74f7acdc6eb2ae77047023b09e330541` |
| `silence.wav` | 529244 | `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977` |

Generation was repeated in two fresh directories; bytes match. Existing outputs are refused. Generated binaries are not committed.

Aspire selection: UNRESOLVED, no stress map is in the input. Before the stress run, select one legitimately available Aspire map with unusual geometry/timing and provide exact creator/title/difficulty, ID if present, `.osu` SHA-256 and all enabled dependency hashes. Record why it contrasts with the simple control. Do not fetch or commit copyrighted map/audio assets to fill this gap. Do not treat the original fixture as an Aspire case.

## Smallest discriminating execution experiment (NOT RUN)

Question: With byte-identical reference files and fixed settings, what externally visible checkpoints occur between choosing Play, initiating the control map, and leaving gameplay, and are their order and capture-frame durations repeatable?

Competing hypotheses, all HYPOTHESIS:

- H1: repeated runs have the same visible checkpoint sequence and duration within capture resolution.
- H2: sequence agrees but first-load work changes entry duration.
- H3: first-launch/setup or missing dependencies block entry entirely.

The smallest useful start is one offline Windows launch and one control-map run. If it succeeds, record three launches from fresh process copies to test repeatability. A warm repeat is a separate labelled condition; never mix it with fresh-process timing. First-launch setup and map import must finish before defining the ready baseline. Stop on a blocker and preserve its text/log rather than declaring gameplay entered.

1. Keep the ZIP immutable; verify its SHA-256. Extract a disposable offline Windows working copy. Hash root EXE/DLL/config before launching. Disable network connectivity for this run so auto-update cannot silently substitute a build. Do not use `repair osu!.lnk`. Preserve pre/post hashes. If the build changes, reject comparability and record it.
2. On Windows, inspect signature trust and capture the actual in-game version string. Run the supplied collector (PowerShell 5.1, output outside the reference directory):

   ```powershell
   .\re\tools\collect_windows_baseline.ps1 -GameRoot C:\ReferenceCopy -ControlMap C:\EntryControl\entry-control.osu -OutputDirectory C:\Evidence\run01-pre
   ```

   Collector is provided but NOT Windows-tested. It gathers OS/GPU/version/signature/hashes, and does not supply active DPI or launch the game. Repeat with a different output path after the run. Keep raw local configs private; publish reviewed relevant settings and hashes, not credentials or usernames.
3. Generate/import the fixture in the disposable copy. Re-hash the actual imported `.osu` and audio; import may alter bytes. If imported bytes differ, preserve both hashes and use actual played bytes in the run manifest. Record exact selected map text/hash, standard ruleset, Auto mod only (proposed means of reaching completion without input-skill variance), and every effective setting above. Do not infer gameplay defaults from missing config.
4. Choose and record fixed display and capture conditions. Suggested initial condition: 1280x720 windowed, 100% display scaling, 60 Hz, 60 fps capture with monotonic frame timestamps; these are experimental choices, NOT supplied-installation facts. Record actual supported values if these cannot be set. Capture full screen through menu interaction, include cursor and audio track, and record input-event timestamps with clock/source and resolution. Validate dropped/duplicated frames; video cannot locate sub-frame internal transitions.
5. Start capture before choosing Play. From a ready main screen, click Play, record the actual route/options presented, select standard gameplay if needed, select the exact control difficulty, and initiate once using a recorded input. The route is a proposed operator procedure, NOT a recovered internal state machine. Do not skip intro, pause, or send gameplay inputs during the Auto baseline.
6. Continue until the visible destination after play remains stable for two seconds. Record any failure/blocker, end/result/return screen actually observed. Then use one recorded Back/Escape action and record the destination; exit the application using the visible exit control and record process termination. Results, song selection and end delay are not assumed.
7. Repeat the fixed ready baseline in three fresh game processes, retaining per-run hashes, configs, version display, capture and logs. Run a warm repeat separately only if needed to distinguish H1/H2. After a successful control baseline, repeat with the selected Aspire case under the same conditions; stress run is pending map availability.

Use this observation sheet; every row is currently UNRESOLVED:

| Checkpoint | Required observation |
|---|---|
| C0 | Ready main screen plus version/settings evidence |
| C1 | Play action timestamp and first visible response |
| C2 | Selected standard-mode control difficulty and import/hash evidence |
| C3 | Initiation action timestamp, last selection frame |
| C4 | Any intervening loading/fade/blank screen, or explicitly none observed |
| C5 | First frame with identifiable gameplay field/HUD; distinguish from first object |
| C6 | First frame displaying an object; audio/map-time relation stays UNRESOLVED |
| C7 | Final object visible, last gameplay frame, first stable destination after play |
| C8 | Back action/destination and eventual process exit |

Store frame index, capture PTS, input time, screenshot/log paths, visible evidence, interpretation and uncertainty separately. Do not fill a missing checkpoint from expectations. Measure action-to-first-gameplay and last-gameplay-to-destination frame intervals only after capture validation. Report capture-limited uncertainty and all runs individually; no invented millisecond precision.

## Gameplay-entry state information actually established

Observed runtime transitions: **none**. There are no gameplay frames, runtime logs or matched input/output traces. C0–C8 are observation labels, not confirmed game states. File presence, CLR references and DLL names establish no transition ordering. Pixel identity, visual similarity and behavioural equivalence are all UNTESTED. Work 2 rendering components were not touched.

Next useful result: a version/settings capture and one successful control entry-to-exit trace on Windows; repeat before inferring timing or implementing a runtime state machine. No authorised exception was used.
