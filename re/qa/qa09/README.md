# 1. Result

QA09 — Independent Ordinary Circle HitSound Selection, Resource Resolution &
Playback Lifecycle Verification. Work1,2026-11-04 Asia/Bangkok.
Intake/latest verified R11 source `e1b207e7067d8424f82e9cc4e4f7230c7b797c45`.
Verification only; no production correction or new original-binary expedition.
No authorised exception.

**PASS — hosted Windows2022, preinstalled SDK8.0.427/runtime8.0.33.** Tested source
`bd082124e7b69daa2f242ca38559d5b8aaa5098d`.
[Run41823138601/job126551035601](https://github.com/thgillwtnorizoh/ohno/actions/runs/41823138601/job/126551035601),
2026-11-04 11:42:46–11:43:57 UTC /18:42:46–18:43:57 Asia/Bangkok.
QA09 **234 PASS/0 FAIL/234 executed**. Historical baseline **628/628**.
WPF Release win-x64 build **0 warnings/0 errors**. Every job step succeeded;
all retained development checks and R11's32 groups remain green. No confirmed
production discrepancy in the covered finite scope. No repair was made.

| Hosted suite | Actual outcome |
|---|---|
|QA01/03/04/05/06/07/08|90/56/43/66/99/159/115 PASS =628 historical cases|
|QA09 independent named checks|234 PASS,0 FAIL; separate from historical/development counts|
|R06 targets /fixture loads|4 PASS /4 completed|
|R07/R08/C01/C02/R09/R10/R11 development groups|8/13/4/14/15/23/32 PASS|
|R11 generator development control|12 files checked; separate from QA09 cases|

Local preliminary execution with already installed SDK10.0.402/runtime10.0.13
also passed234/234 after an earlier226/226 run. Repetitions are not added to the
independent count. The committed project targets net8.0; no SDK/OS was installed.

Created `Headless/QA09.Headless.csproj`, five independently authored test files,
`Inputs/ordinary-samples.osu`, `Inputs/generated-pack-expectations.json`,
`Run-QA09.ps1`, this report and execution/provenance records. The existing Windows
workflow gains three path filters and one independent suite step; every old
command/assertion remains unchanged. Research journal receives append-only QA09
entries. No Work2 production/development source, old independent QA, research,
fixture, score/HP arithmetic or E020 packet is modified.

Reproduction on the existing Windows .NET8 runner, from repository root:

```powershell
./re/qa/qa09/Run-QA09.ps1 -ResultPath <independent-output-path.json>
```

The wrapper generates two packs in its own checked temporary directory, calls
the net8 headless harness, then removes only that directory. It never opens or
plays media. Structured JSON contains exact generated `.osu` inputs, provider
traces, opaque load results, expected/actual comparisons, request fields, session
snapshots, all case outcomes, source hashes and generated-file fingerprints.
Compressed/chunked console evidence preserves it through CI without audio bytes.
`results.json` is the exact285916-byte JSON recovered from the six logged gzip /
base64 chunks, verified against the harness's printed SHA256
`529a36aaf960fc7cef7e52708182e32ff35466f82338b1f4060438a38d9b01c8`.
`ci-job.json` retains API source/time/step provenance; `ci-job.log` preserves the
connector-decoded job output with CRLF normalized to LF (SHA256
`f5d326480fa188c6953a900ac8139cbdac4497acd527ed4ee09e190a5211b7dd`).
`gate-output.txt` is a labelled selection of actual suite/build output;
`generated-assets.json` separates actual PCM fingerprints from the independent
expected metadata. No audio data is included.

**Byte provenance:** the hosted Windows checkout converted text from canonical
LF to CRLF. The actual runner file hashes are preserved unchanged in results.
`checkout-byte-provenance.json` independently reproduces **all15** of them by
exact LF→CRLF conversion of the corresponding canonical bytes, including the
research JSON. This explains differing raw hashes without changing source or
weakening an assertion. Canonical Git SHA and canonical research fingerprints
below remain distinct from runner checkout-byte fingerprints.

234 is the independent **named-check** count:226 executed component/asset/input
controls plus8 explicitly labelled source-binding checks A01–A08. Twelve WAV
artifacts, their repeated generation,628 historical cases and R11's32 development
groups are separate quantities. Windows media decoding, voices, physical audio
output, WPF window actions and native BASS are not executed by QA09.

# 2. What they found

## Authority and independent oracle

**CONFIRMED STATIC authority:** unchanged E037/E038 reports and complete parsed
evidence; E004/E005 ordinary-versus-Taiko correction, E010 clock boundary and
E012 positive-hook order. E037 JSON740850 bytes/SHA256
`7fc63d473be5f30bb39714b0b726f449abb9765a8fe25adbcea3282f71ce536c`;
E038 JSON1246588 bytes/SHA256
`f61b5e16baa403731488953301085e1a456b3bceceb8db7d86bd9c9f3b1fd659`.
No original archive/binary/resource/audio method was inspected or executed anew.
No original sample contents were exported. E020's17 cases remain NOT RUN.

Preparation read the actual AGENTS/canonical standing contract and directed RTK,
research/reconstruction journals, E037/E038 and their evidence, relevant earlier
research, R11 report, QA06/07/08 reports/interfaces, actual parser/session/selection/
resource/library/Windows adapter/host code, Windows README and existing CI.
The harness links the actual four production files, not copies/replacements.
R11 development expected values are not imported or called as the oracle.

**RECONSTRUCTED independent expectations:**

* M1884:007f invokes the positive-circle hook before008e completion and subsequent
  accounting. 300/100/50 reach it; MISS and evaluated circles do not. The actual
  exposed callback can inspect positive Result while JudgedAt/counts/HP events
  remain unchanged. This is directly discriminated by I01.
* M24bb:0012–0014 queries `Double(unchecked(storedStart+5))`, not input-hit time.
  M1be6 keeps the last qualifying supplied record or first when none qualify;
  null/empty returnsnull. Equality, out-of-order records, inherited flags,
  start+5 overflow as an artificial operand and a late50 hit discriminate it.
* M24ba separately inherits zero object base/index/volume; zero is not an explicit
  mute. M44a2 always adds Normal to zero mask and conditionally to nonzero mask.
  M3b32 order is1/4/2/8 (Normal/Finish/Whistle/Clap), with separate addition-bank
  choice and custom!=−1 bypass. Custom0 remains a queued selection operand.
* For volumes0/1/7/8/9/10/19/99/100, independently specified
  `(Normal,Finish,Whistle/Clap)` tuples are
  `(6,8,6)/(6,8,6)/(6,8,6)/(6,8,6)/(7,9,7)/(8,10,8)/(15,19,16)/
  (79,99,84)/(80,100,85)`. They follow Max(8,V), Double*.8/*.85, truncation.
  The expected tuples do not call production attenuation. Custom volume0 has no
  floor. X0/64/256/448/512 supplies pans−.4/−.3/0/.3/.4; rate75/100/150 and the
  rate flag yield bounded factors. Native pan/frequency output is unverified.
* E038 M3b14:0140–01e7 standard map probes wav→mp3→ogg. M3b15:008f–0109 custom
  fallback probes wav→ogg→mp3. Existing exact filenames, leading-dot operands,
  cached stems, provider exceptions/gates and load results remain separate.
  Nonnull bytes end probing even if a later load returns−1 or0.
* Index0 skips map;1 is unsuffixed;2 appends2; larger indexes append decimal
  values during preparation, then judgement uses cache/plane0 fallback only.
  R36 injects36 distinct marker-derived handles to discriminate every row/bank/
  plane, rather than accepting36 aliases to the same default resource.
* Map/custom, skin and default caches differ. Map/skin0 returns0; embedded0
  becomes−1. Cached−1 is retained until explicit invalidation. Custom and standard
  map keys can collide. Skin/default handles can be shared across table planes.

No finite generated case is described as original-runtime observation.
**VERIFIED COMPONENT** now applies to the226 executed finite component/asset/input
controls in the passing Windows gate. The8 source-binding assertions remain
**SOURCE INSPECTION**, even though their file assertions executed successfully.
Neither category establishes universal original behavior or WPF/native audio.

## Coverage and observation boundaries

| Family | Named checks | Discriminators |
|---|---:|---|
|M|31|Raw/normalized sounds; payload defaults/errors/clamps; identity/position/start; ordered timing; slider-duration and R10 separation |
|S|44|Inheritance, all augmentation operands, layer order, literal volume boundaries, custom−1/0, pan/rate/gain, observer/no-I/O and distinct addition banks |
|R|85|36 names;36-cell matrix; standard/custom precedence; exact filename; sentinels; hierarchy/gates; cache misses/aliases/eviction; dynamic preload/fallback |
|L|12|Fresh paused/active, queued/consecutive/logical layers, pause/resume/restart/dispose, stale generation, invalid ID, disabled and injected errors |
|P|30|Actual local paths/files/settings/catalog/library, native-short unsupported state, bounds/cancellation, absent/default/dynamic packs and replacement |
|I|17|Positive quality/order/once-only, own metadata and late-time timing, missing/failing effects, private calibration, matched streams, mixed objects, recovery/API split and fixture |
|W|6|Twelve names/RIFF/PCM, sample bounds, two-run hashes, provider integration, original-fingerprint exclusion and overwrite refusal |
|A|9|Eight source-binding checks plus executed unsupported-archive/retained-session control |

The pure APIs admit explicitly artificial supplied enum/overflow/native-handle
controls; they do not establish support for arbitrary original managers/mods.
Parser errors/unavailable short sample timing are tested against R11's documented
engineering containment, not recast as original parser restrictions.

**Generated assets:** the independent chunk reader checks RIFF length, WAVE ID,
chunk boundaries/padding/count/order, PCM1/mono/44100Hz/16-bit,88200 bytes/sec,
block alignment2,5292 frames/10584 data bytes/10628 file bytes. Sample endpoints,
finite amplitude and nonzero contents are inspected without playing or decoding
audio. A/B fingerprints establish same-host/runtime reproducibility only.
All12 differ from E038's original twelve fingerprints. The generator source uses
mathematical synthesis and no original-asset read; no proprietary samples enter
the test. WPF decoding/audibility does not follow from these checks.
Two named tones (drum-hitwhistle and soft-hitfinish) share the generator's760Hz
formula and therefore the same bytes. Twelve required names do not imply twelve
distinct waveforms; no incorrect uniqueness oracle was introduced.

**Actual production headless lifecycle:** CircleSoundPlayback accepts an injected
recording play action/dispatcher/stop action. L checks execute its real generation,
pause/disable/dispose/exception guards. Library cases execute actual local file
admission and bounded preparation with generated or synthetic bytes; optional
load failures use controlled opaque loader values. They do not call a native
decoder or translate managed errors into original BASS codes.

**Source-inspected Windows adapter and host:** A checks pin the actual callback
and lifecycle wiring. WindowsCircleEffects.OnCircle consumes prepared identities;
its preparation writes/opens media asynchronously before use. Attach detaches
the old session; Restart invalidates playback then attaches; Dispose detaches,
disposes playback, closes voices and releases map resources. The host Load/Restart
uses CreateFreshOrdinary; malformed settings disable optional effects and missing
settings supply explicit practice defaults. Pause/disable intentionally invalidate
queued work. These engineering policies are not E037 native queued-pause rules.

No WPF class, MediaPlayer opening, voice pool, MediaOpened/MediaFailed event,
dispatcher thread or device is dynamically invoked. Their exact scheduling,
decoder failure, two-voice busy/overlap behavior, in-flight close/cancel races and
temporary-file behavior remain **UNVERIFIED** beyond source inspection/build.
The supported injected component boundary must not be described as WPF/BASS
concurrency equivalence.

**Accounting isolation:** matched enabled/disabled sessions compare every exposed
post-step snapshot: raw results/last events, counters, K/score/accuracy/combo,
evaluated times, slider checkpoints, spinner final/observation states, calibrated
HP coefficients/state, passive loss, failure/recovery and fill flags. I01 directly
observes the pre-accounting hook. Private calibration leaves sound/ledger/live HP
event counts0. Errors and missing resources do not turn positives into MISS.
C01 H159/160/200 and HistoricalR09/E036FreshOrdinary remain separate controls.
Unexposed internal record/HP callback interleavings rely on retained source and
historical QA authority, not a new test-only gameplay hook.

**Remaining unsupported/engineering boundaries:** no native short-sample creation,
short timing global defaults, `.osz2` provider, arbitrary original skin config,
slider/spinner sound hooks, original embedded-audio assets, native channel stealing
or output-time fidelity. Independent default-pack key→key.wav mapping is an
engineering adapter. Local-only path, reparse, byte/resource/index limits remain
intact. Settings belong beside the executable per actual AppContext source;
missing, malformed and explicitly disabled states are distinguished.

## Setup and preservation

Local setup initially corrected two new-harness compilation errors (a combined
property declaration and an incorrect assumption that the production queue's
Dispose method implied IDisposable). No cases executed in those build attempts;
no production or historical assertion changed. Windows PowerShell5's script
authorization/module environment refused generator invocation. The already
bundled PowerShell7 executed it normally; no execution policy was changed.
The two subsequent local component runs pass226 and234 respectively. The final
Windows run passes234. There is no failed finite assertion or oracle correction
to preserve, and no production-defect packet is claimed. Initial setup/compiler
errors above are not disguised as original behavior or production defects.
`setup-attempts.json` preserves selected diagnostics, exact commands/zero-case
status and initial harness fingerprints. It is not represented as a raw log.
The retained CI log has three literal trailing spaces (two timestamp-only lines
and the empty-payload case label); source/docs whitespace checks exclude this
immutable execution-text artifact rather than altering its recorded output.

Historical journal prefix, prior research/QA/fixtures/production/development
bytes and all existing workflow commands were preserved. Final publication adds
results/provenance and updates this report/manifest/journal only over the tested
source; harness, inputs, wrapper, workflow and production remain byte-identical
to that commit. No redundant successful gate rerun is claimed. Exact publication
SHA and verified remote head are supplied in the handoff. Checkout's existing
Node20 advisory is separate from compiler warnings. This campaign
does not revisit deferred score discrepancies or start E039/R12. No game,
physical input, OBS/microphone/loopback, debugger, original executable or human
listening test is performed.

# 3. Pseudocode

```text
independent QA09 procedure:
    take E037/E038 literal operations as expected authority
    parse independently constructed source rows through actual production
    check timing/inheritance/layers/volumes against separate literal controls
    inject recording file/default/loader operands and inspect ordered traces
    distinguish first nonnull resource, opaque loader return and prepared handle
    validate all36 prepared cells and dynamic dictionary/plane0 decisions
    inspect independently generated PCM bytes and A/B fingerprints; never play
    invoke production positive-circle callback and real queued-playback component
    observe before/after exposed accounting and matched enabled/disabled streams
    execute supported lifecycle/error/cancellation boundaries without WPF media
    source-check WPF/host wiring, preserving unexecuted runtime limitations
    run unchanged628 historical cases and every development check on Windows .NET8
    preserve input/expected/actual/source provenance for any failure; never repair production
```

Stop after QA09. Native BASS/device/driver/onset/audible and universal gameplay
equivalence remain UNVERIFIED regardless of component/build results.
