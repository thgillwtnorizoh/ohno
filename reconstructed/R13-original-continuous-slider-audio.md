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
Initial Windows run42009465622 on source c4d7e81dbde807dcb60d18b05f861b6f8849b87c
built cleanly and passed all1091 historical QA checks and prior development groups.
R13 passed38 groups then its new fixture failed parser admission: missing mandatory
ApproachRate. The failure excerpt is preserved under R13DevelopmentChecks/verification.
The fixture was corrected without parser or historical assertion changes.

**Final Windows verification: PASS**, tested source
`221c6e1d55a488489155acdadd6e8ed32243a014`.
[Run42010372847/job127187559676](https://github.com/thgillwtnorizoh/ohno/actions/runs/42010372847/job/127187559676),
Windows2022/preinstalled .NET8, 2026-11-05, completed successfully.
WPF Release win-x64: **0 warnings, 0 errors**. Independent historical gate:
QA01 90, QA03 56, QA04 43, QA05 66, QA06 99, QA07 159, QA08 115,
QA09 234, QA10 229 = **1091/1091 named checks**. QA10 includes225 executed
component cases and4 source-only audits. All prior development checks pass:
R06 targets4/fixtures4, R07 8, R08 13, C01 4, C02 14, R09 15, R10 23,
R11 32, R12 40. New R13: **54/54 groups**, including51 executed managed
component groups,2 explicit source inspections and1 six-file structural asset group.
Separate Windows backend: **1/1 empty-resource lifecycle group**, no media opened.
Six independent loop WAVs validated; all15 previous generated WAV hashes preserved.
Generator counts are not independent QA counts. No local .NET SDK was available;
the exact-source Windows gate is the executed build/test authority.

Intermediate run42009817763 on ab2fa0ab62fcbd132c119a72583e82f7e2108a68
passed48 R13 groups and the Windows empty-resource group; expanded run42010084278
on f8d7370a7110b063fd8e70b5b0af14efa560cb30 passed53 groups. Final review then
corrected absent-song cursor clearing and added its control before the final gate.
Initial failure and subsequent source history remain published. Final publication
adds only report/journal/README and verification evidence to the tested source.

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
this bounded model. Global readiness maps to the running WPF update state. The timing model constructor
chooses the post-song-load M3b65 reset state (-1 cursors), not raw CLR type-zero
initialization or the timing-list setter's zero cursors. Those alternate lifecycle
entries remain partial/unsupported. No-song Clock resets cursors without inventing
a current point or sample refresh; selector fields survive that reset.

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

**VERIFIED COMPONENT:** final54 groups and separate Windows empty-resource lifecycle passed as reported above. R13 checks link actual production
sources with injected recording channels/resources/session events. Two labelled
source inspections cover Windows lifecycle wiring; a separate Windows-only harness
executes empty-resource backend lifecycle without opening a decoder, player or window; they are not executed Windows
media operation tests. Existing independent gate remains1091 named checks,
including QA10's225 executed components and4 source audits, not1091 runtime cases.
Coverage mapping for E040 L01–L56 is tracked separately:18 have executed finite
controls,35 are partial and3 unsupported. These are conditional-component coverage
labels, not56 original-runtime tests. Full native lifecycle/null/device paths are
not asserted. The log excerpt and provenance are under R13DevelopmentChecks/verification.

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
