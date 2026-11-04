# 1. Result

R11 — First Original-Derived Audible Circle HitSounds. Work2,2026-11-04
Asia/Bangkok. Base verified latest origin/main
`8ae7b44aa12de46a15f2043847b70add3c5454a2`. Implementation assignment only.
Consumed E037/E038 reports and their complete parsed JSON evidence (740850 /
1246588 bytes; SHA2567fc63d473be5f30bb39714b0b726f449abb9765a8fe25adbcea3282f71ce536c /
f61b5e16baa403731488953301085e1a456b3bceceb8db7d86bd9c9f3b1fd659),
canonical AGENTS/standing contract and journals, E004/E005 ordinary-factory
correction, E010/E012/E026 boundaries, R09/C01/R10 and QA08. No RTK.md exists
in the current checkout; AGENTS directs the actual re/Instruction.md. No original
archive/binary/audio-resource extraction, original-game execution, debugger,
window/input automation, listening, OBS or E020 experiment was performed.

Implemented additive circle/timing metadata; pure sample selection and resource
resolver; positive-circle pre-accounting hook; asynchronous Windows effects
preparation/queued playback with two voices per resource; enable/status/settings
controls; optional independent-tone generator; R11 component checks; minimal
existing CI integration; current README and append-only journal. Song audio and
Stopwatch remain intact. Only reconstruction files and the established CI change.
No research, independent QA01–QA08, fixture, historical report or previous check
assertion is modified. HistoricalR09/E036FreshOrdinary separation and C01 recovery
correction remain intact. E033/E034 and all HP/calibration arithmetic are preserved.

**Actual Windows verification: PASS.** Tested source
`c21aed0d3a5c1bc749d5472ccd1006f99ef43832`.
[Run41792584386/job126449168289](https://github.com/thgillwtnorizoh/ohno/actions/runs/41792584386/job/126449168289),
2026-11-04 07:29:09–07:30:13 UTC /14:29:09–14:30:13 Asia/Bangkok.
Windows2022/preinstalled .NET8.0.427. Actual output:

```text
Build succeeded. 0 Warning(s), 0 Error(s)
HEADLESS COMPONENT TESTS: 90 passed, 0 failed, 90 executed
QA03 SLIDER TESTS: 56 passed, 0 failed, 56 executed
R06 PREPARATION TARGETS: 4 passed (static research oracle, not original runtime)
R06 FIXTURE LOADS: 4 completed (parser/preparation only)
QA04 GEOMETRY TESTS: 43 passed, 0 failed, 43 executed
R07 DEVELOPMENT CHECKS: 8 passed (finite static-oracle/component checks; no human gameplay)
QA05 SPINNER TESTS: 66 passed, 0 failed, 66 executed
R08 DEVELOPMENT CHECKS: 13 passed (E33 finite controls; no original runtime/human play)
R08-C01 DEVELOPMENT CHECKS: 4 passed (component calls only; no game window or OS input)
R08-C02 MULTIPLIER CHECKS: 14 passed (finite static predictions, no original runtime/human gameplay)
QA06 ACCOUNTING TESTS: 99 passed, 0 failed, 99 executed
R09 HEALTH CHECKS: 15 passed (E035 finite component controls; no original runtime or physical gameplay)
QA07 HEALTH TESTS: 159 passed, 0 failed, 159 executed
R10 NEWCOMBO CHECKS: 23 passed (E036 finite controls, separate from independent QA; no game window or OS input)
QA08 NEWCOMBO TESTS: 115 passed, 0 failed, 115 executed
R11 HITSOUND CHECKS: 32 passed (E037/E038 finite production-component controls; no native playback, listening or game window)
R11 INDEPENDENT PACK: 12 PCM WAV files generated and headers checked; no playback
```

All628 independent cases pass; R11's32 groups and twelve generated-file controls
are separate development checks, not independent QA09. All job steps succeeded;
QA07 H160/H200 guard cases explicitly PASS. Checkout Node20 advisory is separate
from compiler warnings. No local SDK/build/install or listening/device test.
The workflow retains every previous command and adds one bounded step:

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project reconstructed/R11DevelopmentChecks/R11DevelopmentChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
./reconstructed/GenerateIndependentHitSounds.ps1 -PackDirectory <runner-temporary-directory>
# Assert12 WAV files,10628 bytes each and RIFF header; remove temporary pack.
```

The first source `5c07743ffdd37ed65120ec77f79fa39931c2d06f` also passed
[run41792253987/job126448087167](https://github.com/thgillwtnorizoh/ohno/actions/runs/41792253987/job/126448087167).
Subsequent static review corrected the named SampleSet All−1 dynamic key spelling
from numeric to `all` and added literal All/None/unknown-numeric checks within the
existing naming group. The final full run above covers that change. No failure
or historical assertion was concealed/rewritten. No new original investigation.

Final documentation-only publication changes this report and appends the journal;
application, development checks, generator and CI remain byte-identical to the
passing source. Diff/whitespace, historical report/fixture/check/research/QA and
journal-prefix preservation checks pass. Exact ledger and HP/calibration/drain /
smoothing/recovery source sections remain unchanged. Main publication uses an
expected-head lease without force or permanent branch. No new audible or visual
verification is claimed by this passing noninteractive gate.

Files: Gameplay/Beatmap.cs and CircleSession.cs; new Gameplay/CircleHitSounds.cs;
new Platform/LocalHitSoundAssets.cs and WindowsCircleEffects.cs; Program.cs /
Rendering/PlayfieldView.cs; README; GenerateIndependentHitSounds.ps1;
R11DevelopmentChecks project/program; this report and journal; one existing
workflow trigger/step. Windows use remains Start-CircleGame.cmd or
`dotnet run --project reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release`.
README documents hitsounds.json and optional map/skin/independent-pack setup.
No sample bytes are committed. The generator synthesizes independent tones,
never plays them, and has no original audio-quality claim.

# 2. What they found

**CONFIRMED STATIC authority (published Work1 evidence):** E037 M1884:007f
positive hook; inherited ordinary M24bb:0012–0014 uses stored start+5; M24ba
selects then observes then branches custom/standard; M44a2 conditionally adds
Normal; M3b32 dispatches1/4/2/8; M24bd supplies Single pan. E038 resolves keys,
provider admission and cache relationships. Representative body SHA256 pins:
M24bb562e53e79e1e025eb14ef07c0bb7065d481eae11ba91e55f2f4c85b7a50bc734;
M24ba6e8e094c3714140c7b4b90f5b5b956e2f143c17df9bf64e91dd2a91fe0aa9998;
M3b14902a5828a359274dbb122534d659842802871c1de095fdad99a0964f4c94f725;
M3b157226022009f57a885178efb81cfe930fb5cf9dcbfe1f78d458b5478d7f6b89c2.
Original instructions are authority; no BASS-library or remembered community
formula was substituted. Old E004 Taiko annotation is not used as a factory-time
ordinary timing lookup. E026 spinner end-time sound boundary is not implemented
through a circle shortcut.

**RECONSTRUCTED finite behavior:**

- Circle sidecars preserve exact CircleDefinition identity, raw sound and stored
  bits2/4/8; base/addition/index/volume/filename/raw-payload/error remain separate.
  Absent/empty and zero overrides are distinguishable; zero selector/volume
  inherits. Optional volume clamps0–100. Shared registered overrides are used
  only for resource custom/index preparation, without slider/spinner sound hooks.
- Ordered timing sample metadata is separate from existing slider TimingPoint.
  Long rows retain time/set/index/volume/uninherited/source order; set0 becomes
  Soft2. Query at unchecked stored start+5 scans all supplied records, retaining
  the last qualifying entry; none qualified→first, null/empty→no request.
- Object selectors override timing when nonzero. Nonzero addition selector is
  used for addition layers, otherwise selected base. Mask0 gains Normal;
  nonzero mask gains it only with skin present, LayeredHitSounds true, selected
  map present and mode!=3. Standard volume floors8, then Normal*.8 /Finish1 /
  Whistle*.85 /Clap*.85 with Double→Int32 truncation in1/4/2/8 order. Custom
  handle!=-1 bypasses layers/floor, including handle0. Pan is the recovered
  Single `(X/512f-.5f)*.800000011920929f`; explicit song-rate matching branch
  supplies the Single factor. Request gain preserves integer effect×global
  multiplication and subsequent Single divisions.
- Standard keys are bank-hitlayer, selector1 unsuffixed,2+ decimal suffix.
  Index0 skips map. Map probes wav→mp3→ogg; custom probes wav→ogg→mp3 only in its
  reached fallback arm. Existing exact custom file takes the exact read arm.
  First nonnull bytes end probing even if loader returns−1 or0. Custom stem
  uses original whole-string first-dot/last-dot operations and shares map cache.
- Custom setters precede unique timing then registered index>2 preloading; then
  planes0/1/2 ×sets1/2/3 ×Normal/Finish/Whistle/Clap populate36 cells. Dynamic
  judgement resolution is dictionary-only with plane0 fallback. Standard map,
  admitted non-Default skin and UI/gameplay default stages retain separate cache
  identities; only embedded loader0 becomes−1. Missing−1 and invalid0 stay distinct.
  Explicit map/skin/all cleanup and per-key eviction retain cached misses until
  invalidation. No per-hit probing or nearest-index repair occurs.
- Positive300/100/50 invokes sound after classification and before JudgedAt /
  existing result accounting; MISS and already evaluated circles do not. Private
  calibration has no sound hook. No slider, spinner or component trigger is added.

**ENGINEERING playback/admission:** explicit hitsounds.json supplies settings,
current culture, optional local skin root/name and independently supplied default
pack. The app supplies a matching fresh selected map, independent default skin
context, ordinary mode0, map suppression/process gates false, rate100/no mods.
These are documented practice inputs, not the owner's original installed state.
Original global volume live reads/repeated references/concurrent mutation are
projected as explicit stable configuration per session. Logical handles are opaque
resource IDs, never native success/error claims. Default pack keys map explicitly
to key.wav; no original embedded-file extension is inferred. No proprietary sample
is extracted, embedded or supplied. Loose map/skin/custom WAV/MP3/OGG support is
subject to Windows codecs. Missing assets give explicit status and playable silence.

File reads/preparation occur on a worker; MediaPlayer opens/MediaOpened waits
occur asynchronously on the dispatcher before session transfer. Judgement uses
only prepared metadata/handles and queues actions. Two preopened voices per
resource allow independent overlap; busy pools drop new requests with a diagnostic.
Gain/Balance/SpeedRatio map to managed properties, not original native attributes.
Pause stops voices and invalidates queued work, resume drops old requests;
restart detaches old session/resets dispatch and reuses prepared assets. Replacement
closes map voices/custom-cache IDs; skin/default caches persist in the library;
exit releases caches/players and best-effort temp files. Settings changes require
relaunch. Resource/decoder/optional-sink failures are contained and visible.
This intentionally differs from original helpers' possible exception interruption
and unresolved native queued-pause behavior, preserving practice accounting.

Safety limits: relative paths inside admitted roots, no network/absolute custom
paths/traversal/reparse points;8MiB/file,64 resources/64MiB and64 dynamic indexes.
Native short-byte Map SampleCreate is explicitly unsupported by MediaPlayer:
logical0 plus diagnostic, no manufactured audio. Short timing rows whose sample
set/default-volume depend on unreconstructed map/global operands disable effects;
well-formed long rows with explicit volume are supported. Malformed payloads
retain errors and disable affected preparation rather than being guessed zero;
keeping gameplay usable is an engineering parser-containment choice, not original
exception equivalence. Archive `.osz2`, complete skin manager/configuration,
external resource mutation, miscellaneous native sample families, native channel
stealing/thread/device/onset precision and original-audible parity remain outside
scope or UNVERIFIED. No scoring-discrepancy, clock or audio-synchronization rewrite.

**Development checks:** actual linked production parser/session/selection /
resolver/local providers with injected files/loader and recording sink. Literal
H/K finite controls cover trigger quality/order/once-only, metadata/timing /
inheritance/layering/attenuation/custom−1vs0, pan/rate/gain,36 names, extension
orders, exact/custom cache aliasing, dynamic/prepared fallback, provider gates /
priorities, invalidation, traversal, pause/restart/disposal, failure containment,
assetless replacement and score/HP invariance. They are bounded development
checks, not independent QA09 or evidence of native playback/listening. No original
static checker requiring an archive/binary is executed.

# 3. Pseudocode

```text
RECONSTRUCTED preparation:
    retain ordinary object samples and separate ordered timing sample records
    apply accepting custom filename setters into shared map/custom cache
    preload unique timing indexes>2, then registered object indexes>2
    prepare36 standard cells, preserving map→skin→default stages and sentinels

RECONSTRUCTED positive ordinary circle hook, before result handoff:
    timing = ordered last qualifying at stored start+5, else first
    no timing -> no sound request
    inherit base/index/volume; select addition; conditionally augment Normal
    observer(info)
    if customHandle!=-1:
        submit custom with unattenuated volume, pan and factor
    else:
        standardVolume=max(8,selectedVolume)
        for layer in Normal,Finish,Whistle,Clap:
            if selected bit:
                handle=prepared table OR dynamic cache/plane0 fallback
                apply original layer attenuation and integer conversion
                if handle!=-1: submit(handle,volume,pan,factor)
    continue existing judgement, score and HP accounting

ENGINEERING Windows adapter:
    read admitted files off judgement path; asynchronously prepare local voices
    enqueue request under current session generation
    on dispatch: reject paused/disposed/stale generation and invalid0/-1
    play an available independent voice with managed gain/pan/rate
    failure -> diagnostic; never mutate score or HP
    pause/restart/replacement -> invalidate queued work and stop voices
    map close -> release map cache; library close -> release skin/default caches
```

Stop after R11. No QA09/R12/E039, original experiment, framework migration or
lazer extension is begun. Human-audible and original-runtime results remain
UNVERIFIED pending the owner's independent operation.
