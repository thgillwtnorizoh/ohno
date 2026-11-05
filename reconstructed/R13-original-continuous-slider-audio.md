# 1. Result

R13, Work2, 2026-11-05. Starting verified canonical main
`37ceaeaf96a6ee6c13fd3807ec95057cdba677aa`. Implementation only; consumes E040
and E037–E039 published original-static evidence, E010/E015–E018/E021–E023,
AGENTS/standing contract, both journals, R11/R12 and QA09/QA10. No RTK.md is
present or directed by current AGENTS. No original binary execution, extraction,
proprietary sample copying, game window, audio playback or listening was performed.

Implemented global timing/sample model, separate mode2 resource table, shared
persistent pair and recording backend, queued start/pan/pause and synchronous
selector-change Stop, bounded Windows persistent players, R12 lifecycle hooks,
independent six-loop generator, R13 development harness and existing CI extension.
Historical research/QA/fixtures/reports and journal prefixes remain unchanged.
Windows verification is pending; no local .NET SDK is available in this host.
This report must not be read as completion before successful exact-source CI.

# 2. What they found

**CONFIRMED STATIC:** E040 identifies F238e/F238f separately from F2360/F2361
and last continuing selectors F2362–F2364. F2354/F2355 are one global pair.
M3b66 full rescan uses strict <, forward advancement uses <= in supplied order.
M3b20 replacement and M3b33 eligibility, M3ba1/2 start rereads, M3b9c captured pan,
M3ba8 current-target Pause, gain expressions and R12 ordering follow the report.
No new original-binary conclusions are claimed.

**RECONSTRUCTED:** GlobalSampleTiming and SharedSliderChannels implement these
finite transitions independently of per-request M1be6/tick+2/edge+5 selection.
Resource modes remain separate: four circle rows, one tick row, two continuing
rows, each with three banks and planes. Continuing preparation sets Loop=true;
standard WAV/MP3/OGG priority, index0 map exclusion, dynamic cache-only lookup,
plane0 fallback and logical -1 versus0 remain distinct. Shared custom cache
collisions are not given a fabricated loop-aware cache key.

Original model mutations precede possible injected exceptions without rollback.
Original callbacks reread channel targets at execution, including a second start
read after gain assignment. The recording backend exposes activity and acquisition
operands explicitly; its integers are not measured native results. Numeric attr11
is retained in the model; Windows does not reinterpret it as an unproven pitch rule.
Gain uses unchecked Int32 product then Single arithmetic with distinct start and
replacement operation order. Timing-point volume floor8 differs from map fallback
(no floor), no song100, tick volume and standard layer attenuation.

**ENGINEERING:** WindowsCircleEffects owns one shared subsystem per active prepared
map/session; Attach verifies the actual prepared-map reference. Fresh format14+,
finite supported timing and bank/index operands, nonoverlapping ordinary sliders
are admitted. Unsorted timing, overlap (including touching endpoints), malformed
sample metadata and loop/custom collisions disable continuous support without
changing slider admission, discrete selection or accounting. The original model
still permits controlled out-of-order/shared-producer cases for deterministic tests.
The original non-slider cache refresh and timing delegate side effects are outside
this bounded model. Global readiness maps to the running WPF update state.

Session hooks preserve head-before64, tracking before audio, continuing before
latest-index comparison, second helper on a new discrete producer at time<=end,
tracking-loss cleanup before consumption, aggregate cleanup before positive hook,
and exact-end additional continuation. Exceptions are contained at optional effect
boundaries so gameplay accounting proceeds; this intentionally differs from native
helper exception propagation. Manager cleanup uses an explicit current-object
interval/pending proxy, not a claim to reconstruct native candidate arbitration.
Private health calibration never calls the live event stream.

WindowsSliderLoops preopens loop MediaPlayers asynchronously while loading; no
sample file loading or decoder Open occurs in judgement callbacks. They are
persistent prepared resources, separate from one-shot voice pools. The model owns
only two current logical channel IDs. Prepared players are retained for resource
reuse and closed on map disposal. Only selected channels Play. MediaEnded restarts
at zero while logical activity is1; this is not gapless playback. Gain and balance
are clamped to platform ranges. WAV/MP3/OGG admission uses installed Windows codecs;
OGG in particular may fail. Decoder/operation errors are status, not native codes.
Loop disabled/missing status appears alongside discrete status.

FIFO action draining on the WPF dispatcher is ENGINEERING. Session pause, disable,
restart, replacement, exit and errors cancel queued work; pause uses PauseAll,
not selector-change Stop. Restart resets timing cursors while retaining selector
state; resume requires a future eligible helper and never blindly resumes loops.
Generation cancellation can suppress callbacks that original code would execute.
The presentation-X bridge is the same last published R12 ball X, possibly a frame
behind native state. Pan is parentPan*.4; no pointer substitution or rate change.
Arbitrary seek, editor/reused-manager, device replacement, overlap arbitration,
full native cache lifecycle/scheduling/channel stealing remain unsupported.

Independent optional setup: run GenerateIndependentHitSounds.ps1, then
GenerateIndependentSliderTicks.ps1, then GenerateIndependentSliderLoops.ps1 with
the same -PackDirectory. The six added files are normal/soft/drum-sliderslide.wav
and normal/soft/drum-sliderwhistle.wav, mono PCM16 44100Hz, 0.5 seconds each,
mathematical periodic tones with no proprietary input. Configure that absolute
local directory as DefaultPackDirectory in hitsounds.json beside the app.
Existing Enabled/LayeredHitSounds/VolumeEffect/VolumeUniversal settings apply.
Existing twelve circle and three tick files remain unchanged. Map/skin resources
and supported indexed variants use the established local-root policy. WAV header
checks establish structure only, not decoding, seamless looping or acoustic output.

**VERIFIED COMPONENT:** pending Windows gate. R13 checks link actual production
sources with injected recording channels/resources/session events. Two labelled
source inspections cover Windows lifecycle wiring; they are not executed Windows
media operation tests. Existing independent gate remains1091 named checks,
including QA10's225 executed components and4 source audits, not1091 runtime cases.
Coverage mapping for E040 L01–L56 is tracked separately; no full56 execution claim.

**UNVERIFIED ORIGINAL-RUNTIME:** native BASS success, worker interleavings,
audible fidelity, actual Windows playback/device/onset, seamless loops and owner
listening remain unverified. Owner qualification remains necessary. No R14,
E041, QA11, spinner sounds, framework or score-discrepancy work is started.

# 3. Pseudocode

```text
RECONSTRUCTED clock:
    advance separate base/all cursors at unchecked(time+5)
    full scan strict <; initialized forward supplied-order <=
    selected/song identity and version gates -> current global sample selectors

RECONSTRUCTED continuing:
    resolve parent base/addition against global base
    independently compare index/base and index/addition
    changed row: Stop old only if active1; retain selector; resolve mode2; Acquire(false)
    set11/2/2/11 attributes using global timing/settings gain
    eligible inactive rows -> queue starts; optional captured pan -> queue pan
    execution: fresh shared target for gain, reread for Play(true)
    cleanup -> queue Pause on execution-time current pair

R12 integration:
    tracking -> continuing helper -> latest-index comparison
    new deadline: store index -> fresh time<=end helper -> unchanged discrete sound
    tracking false: cleanup then consume due index
    aggregate: cleanup -> existing positive ratio -> possible exact-end helper -> edge
    later manager proxy cleanup gate

ENGINEERING Windows:
    prepare local resources and persistent decoders during loading
    FIFO generation-protected callback dispatch
    MediaEnded loops prepared stream; Pause differs from Stop
    unavailable/error -> explicit status, no score/HP mutation
```
