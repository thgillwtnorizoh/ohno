# 1. Result

R14-Q01, Work2,2026-11-11 Asia/Bangkok. Started from verified latest canonical
main `d991dc7c411c28c3bbe88a22a35773c43b054c47`. R14 production baseline
`75dbf61269937fedd969dfc9e847b731c9f6b446`; independent QA12 tested source
`51c5d7262c0643213eae756165d9824eda35c831`. No authorised exception.

**Investigation outcome:** owner's audible failure is accepted as OWNER-OBSERVED;
its session-specific root cause is **NOT YET ESTABLISHED**. No playback algorithm
defect has been demonstrated. Published an additive bounded diagnostic facility,
clearer setup/status text and owner-only reproduction procedure. The new status
separates logical spin/bonus handles from decoder readiness. No frequency/backend,
resource precedence/cache, shared-channel/scheduling, scoring or health rewrite.

Preparation: AGENTS/standing contract and journals; complete R14/R13-C01 reports,
R13 channel implementation; QA12 README, evidence/provenance and linked actual
controls; E041 report/parsed66 reference cases, evidence SHA256
`4fae962a1d69000f522be14ba5c91b3587d001e245795287a457ee058a010a5e`;
current settings/library/resource/channel/Windows/session/presentation/Program,
generator, README and established Windows workflow. No RTK.md found.
No original binary extraction/research/execution, game window, physical input,
media opening, decoder playback/listening or capture is performed by automation.

Changes: `CircleGame/Platform/LocalHitSoundAssets.cs` (configuration path helper,
bounded snapshot and preparation metadata), Windows adapters (observation only),
Program/PlayfieldView (owner export/setup/status), README, focused
R14Q01DevelopmentChecks, minimal existing CI addition, this report and append-only
implementation journal. Historical research, QA01–QA12, previous reports,
fixtures/generators and gameplay arithmetic are preserved.

Initial Windows gate42941701913/job130264802295 at
`ab62bea33a08e0496d1c53c546698062d80971ef` is preserved. WPF built with0 warnings/
errors;1629/1630 independent checks passed; QA09 A05 source audit failed because
Program delegated the path expression to a helper rather than retaining its explicit
`Path.Combine(AppContext.BaseDirectory,"hitsounds.json")`. Restored that existing
expression without changing the QA09 audit. All other independent checks and prior
development suites passed. New harness failed compilation CS0136 (nested `before`
local shadowed a later local); zero new assertions executed. Renamed only that
harness local. Exact returned log/gzip, summaries, provenance and both initial
Program snapshots remain under R14Q01DevelopmentChecks/verification.

**Final exact-source Windows gate: PASS.** Tested source
`2630e3ebe97d89eb712810258651fc6bca99007d`.
[Windows run42942529851/job130267497951](https://github.com/thgillwtnorizoh/ohno/actions/runs/42942529851),
Windows2022/installed .NET8,2026-11-11 01:56–01:58 UTC /08:56–08:58 Asia/Bangkok.
Verified on reusable ci/verification before canonical main publication; final
report/evidence-only child preserves the tested production/harness/workflow bytes.
Publication commit is supplied in the final handoff, with no force update.

| Gate | Actual result |
|---|---:|
|WPF Release win-x64|0 warnings /0 errors|
|QA01/03/04/05/06|90/56/43/66/99 passed|
|QA07/08/09/10|159/115/234/229 passed|
|QA11 (including unchanged G08)|253/253|
|QA12 (274 headless +12 Windows)|286/286|
|Historical independent total|1630/1630|
|New R14-Q01 development controls|23/23 (22 component,1 source inspection)|

Every retained development suite passed: R06 targets4/fixtures4; R07 8; R08 13;
C01 4; C02 14; R09 15; R10 23; R11 32; R12 40; R13 54 plus Windows1;
R13-C01 6; R14 58 plus Windows1. Counts retain historical source/asset/empty-backend
classifications and are not claimed as1630 runtime-only cases. Generated artifacts
and new development groups are not additional independent QA. The two earlier
failed gates and exact logs/source snapshots remain immutable. No original research
or historical oracle was changed to achieve this result.

# 2. What they found

## Evidence classification and owner symptoms

- **OWNER-OBSERVED:** no continuing spinner sound, ordinary tap where a bonus was
  expected, generated samples present at `C:\MyIndependentPack`.
- **VERIFIED COMPONENT:** historical QA12 passed286 named checks for supported
  managed selection/state transitions, including injected Windows callbacks.
  Its nonempty Windows decoding/output path was not executed. This investigation's
  newly executed results are recorded separately below when available.
- **CONFIRMED STATIC:** E041 establishes separate shared F2356, named spin/bonus,
  queued live-target Play(false)/Pause, motion before progress accumulation,
  bonus after count mutation and positive stored-end completion selection.
- **ENGINEERING:** local default pack, MediaPlayer, dispatcher, generation policy
  and this diagnostic facility. It observes existing decisions without replacing
  their original-derived managed boundaries.
- **NOT YET ESTABLISHED:** owner's executable identity, actual settings contents,
  chart identity/admission, C/U history, selected providers, decoded readiness,
  surviving playback interval and audible device output.

## Confirmed routing, and ranked remaining hypotheses

1. **Configuration/provider/admission:** strongest first discriminant, not a proved
   owner root cause. `Program.EffectsConfigurationPath` is exactly
   `Path.Combine(AppContext.BaseDirectory,"hitsounds.json")`; an absent file uses
   Enabled=true/E80/U100/**DefaultPackDirectory=null**. No pack discovery scans
   `C:\MyIndependentPack`. The owner's actual JSON has not been supplied/read.
   Configured roots resolve to full local paths; invalid JSON/volumes/root admission
   leaves optional effects unavailable. A circle-only pack can play a completion
   tap while both spinner keys are missing. Inspection confirmed the current parser already
   rejects non14 spinners (and all format15+ maps); the separate exact14 audio
   predicate is defensive, not an explanation for silence after a normally parsed
   spinner has loaded. The original UI's setup text still described12 circle files
   and its spinner status used positive handles alone; those diagnostic gaps are
   now corrected without asserting an audible-path bug.
2. **Event mistaken for bonus:** independently plausible. The R07 fixture is
   start3000/end6000/OD5, N15. C16 earns300 at positive completion with rawmask0,
   selecting normal-hitnormal. First normal bonus is C20 (`C>18`, even offset),
   then22/24…; bit4194304 suppresses its sound. A completion tap is expected and
   is not a fallback from a missing bonus. Owner's reached C is unknown.
3. **Catalogued but decoder unavailable:** both resources require positive logical
   handles **and** successfully opened MediaPlayer resources. File existence and
   PCM structure establish neither. The new snapshot reports individual readiness,
   provider/key and failure messages rather than aggregate counts alone.
4. **Motion/lifecycle/dispatcher:** zero U queues Pause; nonzero U queues start
   independently of new components. Repeated starts use false. A later zero-motion,
   completion, owner pause/focus deactivation, restart/map replacement or effects
   disable can pause/cancel work. Injected controls distinguish start→Pause and
   stale-generation cancellation. No owner trace yet establishes premature cleanup.
5. **Device/output/codec or Windows runtime:** remains possible even if all logical
   operations succeed. MediaOpened/HasAudio and MediaPlayer.Play are intermediate
   observations only. No device, latency, gapless or audible equivalence claim.

## Configuration/resource/channel audit

| Boundary | Confirmed source behavior | New owner evidence |
|---|---|---|
|Executable → settings|AppContext.BaseDirectory/hitsounds.json; read once at app startup|executable/baseDirectory/configurationFile/configurationExists|
|Settings → pack|DefaultPackDirectory, optional; no implicit discovery|defaultPackDirectory/defaultPackDirectoryExists, settings volumes/enabled|
|Spinner admission|Exact14, fresh HealthSource bound identity, matching sidecars; supported overrides/timing, no collisions/overlap|sourceFormat/spinnerAdmission/spinnerCount|
|SpinnerResource|spin(looptrue), bonus(loopfalse), unprefixed/unsuffixed stage7; map index0 admitted|spin/bonus.handle/provider/key/loop/catalogued|
|Providers|Map WAV→MP3→OGG, admitted skin, default key.wav; first found bytes do not prove load success|provider identifies winning source; original cache identity retained|
|Shared caches|Map per preparation; skin/default cache incl−1 persists per library|close/reopen after settings/files change; map reload alone insufficient|
|Catalog → Windows|Prepared sample dictionary; pre-game async decoder Open/HasAudio; no judgement-path decoding|spin/bonus.decoder, decoder/media failed counters/messages|
|Motion → queue|Session Motion before P accumulation; same-map/enabled/unpaused/metadata gates|spinner.event.Motion detail C/N/U; rejection/start.submitted|
|Queue → shared channel|Global timing refresh acquires; fresh target/gain, Play(false)|sharedSpinnerChannel, loop.acquire, shared.flush counters, loop.Play.ID|
|Cleanup/replacement|Queued fresh Pause; selector replacement Stop only prior active1; lifecycle cancellation distinct|loop.Pause.ID/Stop.ID, lifecycle.cancel, generation and transitions|
|Bonus/completion|Distinct event provenance and prepared resource dispatch|SpinnerBonus.* and SpinnerCompletion.* counts/gain/handle|

Every SpinnerAdmission condition was inspected: no spinner returns null; otherwise
fresh HealthSource, exact format14 with supported timing metadata, exact spinner
sidecar count/reference identity, no payload errors/base/addition outside0–3 or
negative indexes, nonempty finite supported timing/set/index, supplied-order
nondecreasing times, no touching/overlapping spinner producers, no registered custom
stem spinnerspin/spinnerbonus and no standard/duplicate completion custom stem.
The parser already rejects non14 spinners and can reject other malformed/touching
object states even earlier. Older-format circle/linear-slider parsing is broader,
not spinner parsing. D06 isolates the defensive audio gate with an explicitly
artificial metadata variant and rebound internal freshness proof; it is not a
claim that the parser accepts a format15 spinner.
Map/session identity is additionally checked by reference at Windows Attach.
No supported domain was broadened. Rejection preserves otherwise playable gameplay.

New diagnostics retain128 fields/256 counters/1600 characters per value, last
sequence/time per counter and last32 changed loop-operation transitions across at
most64 channel IDs. Repeated Play calls aggregate rather than emit frame log lines.
No audio buffers or per-frame disk writes. Snapshot export is an explicit owner
action and includes local paths. Review it before sharing. Counts are cumulative
for the prepared effects instance and can include multiple restarts. Saving pauses
the session first, so its final cleanup/cancellation/Pause entries are expected.
Bonus registration/invalid−1/0 are not converted into normal-sample fallback.

MediaPlayer readiness observations do not change acquisition/playback decisions.
Late MediaFailed clears only diagnostic readiness and keeps existing error/state
policy. Windows native length/attribute1 frequency remains explicitly Unsupported;
setting SpinnerFrequencyModulate=false removes requests without disabling loops.
The model and backend are not transactionally equivalent to native BASS. Generation
vetoes, clamping, voice pools and dispatcher ordering remain ENGINEERING.

## Development controls and regression evidence

D01–D23 exercise actual production path/config reading, incorrect/circle-only/full
packs, exact14 rejection, map/skin/default precedence, persistent cached misses,
logical mapping versus unopened decoder, motion/queued dispatcher execution,
start→Pause, restart cancellation, missing channel, C20 bonus/C16 completion,
distinct Windows event request identities, missing voice and injected worker
failure, enabled/zero gain, accounting/slider preservation, bounded diagnostics.
D23 alone is source-only nonempty MediaFailed/decoder/export wiring inspection.
D10/D15 use an **empty** real Windows backend, never a decoder/player. D11–D14 and
D18/D20–D21 use actual Windows callbacks with an injected recording backend and a
real WPF dispatcher pump without a Window. D16/17 use controlled artificial count
progression, not synthetic physical gameplay or a claimed owner performance.
Missing-voice/injected-worker failure controls are not actual codec failures.

Second gate42942081383/job130266038300 at
`29dcdeedb2118facf482a4646e5730baf6563599` passed WPF clean, all1630 independent
checks and all prior development suites. New D01–D05 passed; D06 incorrectly tried
to parse a v15 spinner before checking optional-audio admission. Parser correctly
rejected it. Preserved exact log/source/provenance. Corrected the fixture to assert
real v13/v15 parser rejection, then an explicitly artificial rebound-freshness
metadata variant to isolate the unchanged exact14 audio guard. No parser or
production admission change and no weakened exact14 assertion.

Final authoritative run passed23/23 as tabulated in Result. Complete exact
returned Windows job log is retained as `verification/windows-job.log.gz.b64`;
base64 decode then gzip decompress reproduces the UTF-8/CRLF bytes. Summary,
SHA256/byte provenance and the **artificial development-session** diagnostic JSON
are alongside it. This JSON is not evidence from the owner's failing session.
Initial logs use the same reversible encoding. Final publication-byte review
found that text transport had normalized the synthetic diagnostic JSON CRLF to LF
in the first docs publication `2f8291d36c52babd7c685b3ec40d1310fe654cf2`.
Restored that JSON through a raw-byte Git blob to match the runner artifact and
its retained SHA256. The compressed exact job log, tested code and assertions were
unaffected; first publication remains in ancestry. All exact sources/runs are distinct;
second-run production bytes are identical to final tested production. Final source
changes after the second gate were the D06 fixture and documentation/evidence only.
No test oracle is weakened and unchanged QA11 G08 remains mandatory.

## Owner-only Windows qualification procedure

1. Build or use the published exact-source executable. For the repository's existing
   Release command:
   `dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false`.
   Run `reconstructed\CircleGame\bin\Release\net8.0-windows\win-x64\Ohno.CircleGame.exe`.
   If using another copied/published executable, its adjacent settings file is the
   relevant one; record its exact full path. `Effects setup` displays it.
2. Close all app instances. Generate independently authored samples into the same
   pack with the existing PowerShell generators (circle12, ticks3, loops6, spinner2).
   Spinner generator alone:
   `pwsh -NoProfile -File reconstructed/GenerateIndependentSpinnerSounds.ps1 -PackDirectory C:\MyIndependentPack`.
   Confirm exact filenames `spinnerspin.wav` and `spinnerbonus.wav`, not doubled
   extensions. Keep the twelve circle files for the separate completion control.
3. Save this JSON as **hitsounds.json beside that executable** (not the source
   directory and not the pack). Exact keys/case; JSON uses doubled backslashes:

   ```json
   {
     "Enabled": true,
     "DefaultPackDirectory": "C:\\MyIndependentPack",
     "SkinName": "Default",
     "SkinSamples": true,
     "LayeredHitSounds": true,
     "VolumeEffect": 80,
     "VolumeUniversal": 100,
     "SpinnerFrequencyModulate": false,
     "SpinnerModifierBits": 0
   }
   ```

   Disabling the unsupported frequency request is only for a simple diagnostic
   run; it does not change continuing eligibility. Confirm Windows app/device
   volume is audible manually. No software capture/loopback qualification required.
4. Owner opens app and selects **Spinner Practice**. This bundled fixture has no
   AudioFilename: song status “Silent practice” is expected and does not mute
   optional effects. Keep HitSounds checked. Before play, footer should show no
   spinner admission limitation, positive spin/bonus handles, both decoder=True.
   On mismatch save diagnostics and stop diagnosing through listening guesses.
5. Owner starts and holds Z/X, rotating around center during3–6s. Continuing tone
   should accompany nonzero U (nonmodulated160Hz generated tone). A natural brief
   stop can reach U0/Pause; no guaranteed seamless onset/looping is claimed.
6. Distinguish outcomes: UI Progress16 is enough for300 but no bonus. If progress
   reaches20 while sampled before completion, a separate short decaying880Hz bonus
   is expected, then at22 etc. A normal tap after end is the rawmask0 **completion**
   control. Do not require bonus when C never reaches20. This short fixture may be
   challenging; report the actual count instead of inferring from the final grade.
7. Immediately after the attempt choose **Save audio diagnostics**; this pauses
   and adds expected cleanup. Save owner-selected JSON, plus exact executable/map
   path, observed sound identities/times, reached Progress and a manual note of
   whether continuing/bonus/completion were heard. No recording is requested.
   Inspect configuration/admission/provider/decoder first; then event C/N/U,
   submitted/executed/cancelled counts, shared channel, last operations/errors.
   `SpinnerBonus.request=0` with C16 does not implicate its sample. Requests with
   `voiceMissing` or decoder unavailable do not prove playback. Play counts with
   no sound require further owner/device evidence rather than fabricated fidelity.
8. After changing JSON or pack, fully exit/relaunch, reload fixture, repeat.
   Restart R and map reload retain the library cache; they do not reread JSON.
   If this fixture succeeds, open the original chart and export a separate report;
   exact format/admission or map/skin precedence may differ. Do not change a chart's
   format header merely to bypass rejection.

**UNVERIFIED ORIGINAL-RUNTIME:** actual owner resource decoding, media/device output,
continuing interval, native frequency/pitch, scheduling, gapless looping and audible
fidelity remain unqualified here. The next discriminating evidence is the owner's
export from the failing session and its separately identified sounds, not another
headless pass. No R15/QA13/E042 or deferred scoring investigation begun.

# 3. Pseudocode

```text
CONFIRMED STATIC E041 / retained RECONSTRUCTED model:
    changed integer -> C++; optional modulation; conditional bonus; cache
    reached motion U!=0 -> optional modulation; queue live-channel start
    reached zero U / cleanup -> queue live-channel Pause
    callback -> current gain/target; Play(fresh shared spinner channel,false)
    positive completion -> stored-end standard/custom selection (no +5)

ENGINEERING diagnosis (observation, no new playback arbitration):
    read executable-adjacent settings; record effective local roots/settings
    prepare existing map -> admitted skin -> default selection
    record admission, provider/key, logical handles and catalog membership
    await existing decoder Open; record readiness or failure separately
    count distinct stage/request/dispatcher/generation/channel operations
    aggregate repeated calls; keep bounded changed-operation transitions
    owner Save -> existing Pause; snapshot JSON to owner-selected file

UNRESOLVED owner root cause:
    compare failing-session configuration/admission/resource/decoder first
    then C20 bonus identity vs C16 positive completion and motion U
    then surviving deferred Play/Pause/Stop/error/device evidence
    no proved playback discrepancy -> preserve playback semantics
```
