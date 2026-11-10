# 1. Result

QA12 — independent R14 spinner audio, frequency requests and lifecycle verification.
Work1,2026-11-11 Asia/Ho_Chi_Minh. Canonical starting source
`e785774893d45fe4d2a45617212ce597ebe1bbba`; tested R14 production
`75dbf61269937fedd969dfc9e847b731c9f6b446`. Verification only. No authorised exception.

Independent .NET8 harness links actual production parser, session, spinner,
observation, channel, resource, sample selection, catalog and library sources.
Separate Windows controls link actual WindowsSliderLoops and WindowsCircleEffects;
empty resources never enter MediaPlayer construction/Open. Controlled callback
cases inject the independently authored recorder, not a replacement gameplay model.
No proprietary resources, original executable/CLR/native methods, gameplay window,
physical input, media opening, listening, capture or E020 cases are run.

Initial local compilation failed CS0169 for an unused harness field; no assertions
executed. First runtime267:260 pass/7 oracle-count failures. Later added270-case
run268 pass/2 oracle-count failures. Exact original sources/results/logs and
independent E040/E041 corrections are retained in verification/. No production
repair and no historical assertion change. R14's original compile/residual-fixture
failures and successful source chronology remain intact; r14-history.json records
all three sources/runs without turning harness failures into production defects.

Local supplemental final outcome and authoritative Windows .NET8 gate are recorded
in verification/; pending Windows verification is not a pass. The harness includes
262 finite managed checks,8 source inspections and4 asset checks (274 headless)
when the wrapper supplies its two generated packs;12 separate Windows checks.
Assets, repeated runs and66 coverage rows are excluded from case inflation.

Reproduce on the established Windows host with its installed .NET8 SDK:

```powershell
pwsh -NoProfile -File re/qa/qa12/Run-QA12.ps1 -ResultPath "$env:TEMP/qa12-results.json"
```

Local component preflight can omit pack arguments:

```text
dotnet run --project re/qa/qa12/Headless/QA12.Headless.csproj -c Release -- <repo-root> <result-path>
```

The optional preflight omits four asset controls. It is not the full QA12 gate.
All initial `.gz.b64` files decode as base64 then gzip to the exact result/log bytes.
The wrapper generates two complete independent23-file packs, captures all21 old
hashes before spinner generation, runs the headless suite and12 Windows checks,
and exits nonzero on any discrepancy. CI adds only QA12 path filters and one step,
retaining every historical QA/development/build command. No SDK installation.

# 2. What they found

Supported normal-returning finite R14 transitions satisfy the independent local
controls. No production defect is confirmed so far; final classification requires
the authoritative exact-source Windows gate. Expectations derive from published
E041/E024–E026/E037–E040 evidence, independent literal frequency bits, marker
providers and distinct metadata, rather than copied R14 development assertions.
The manifest pins the original evidence, actual source, previous reports/journals,
harnesses, generators and preserved failure provenance.

- Metadata retains raw spinner masks including high/negative bits, stored start/end,
  source identity/order, all sample overrides, filename and prepared center. Malformed
  or unsupported metadata produces explicit optional-audio limitations.
- Global refresh updates selectors before spinner work, checks nonzero active1
  Stop, resolves/acquires(false), stores the new field then writes11/2. Unchanged
  resource names do not suppress reached replacements. Partial failures survive.
- Signed Int64 length strictly above8192 gates frequency. The target is reread after
  Length; finite Single products use truncation, unchecked Int32 addition and only
  the upper100000 cap. Exceptional conversions remain explicitly unsupported.
- Observation increments once per changed converted integer, frequency follows C
  before classification, bonus precedes cache, and the first changed raw0 can
  modulate. Strict interior motion uses Double U before P accumulation, including
  both signs, zero, repeated timestamps and independently established residual U.
- Queued starts use fresh gain/target and a second handle read for Play(false).
  Pause targets the channel current at execution. No active/pending/owner token is
  inferred. Generation cancellation is an engineering policy.
- One captured settings gain feeds slider0,slider1 and spinner, with fresh target
  reads even when injected backend callbacks mutate settings/fields.
- spinnerspin(looptrue) and spinnerbonus(loopfalse) use unprefixed, unindexed stage7
  map/skin/default resolution. Marker controls distinguish cache−1/0, WAV/MP3/OGG,
  first-nonnull bytes versus load results, provider precedence and collision admission.
- Bonus threshold/parity and modifier4194304 affect ancillary sound without changing
  raw16384/accounting. Registration precedes queued playback. Global volume, no
  standard attenuation and no invented pan remain separate from continuing sound.
- Positive completion queues cleanup before evaluated/minimum/quality boundaries,
  queries stored end without+5, applies shared standard/custom selection and fresh
  center pan, and never drains pending P or changes grade for missing audio.
- Six matched finite session modes compare every intermediate P/E/C/N/U, components,
  ledger/score/multiplier, accuracy/combo, HP/recovery/NewCombo and existing circle,
  slider discrete and continuing requests. Private calibration stays silent.

The E041 coverage matrix independently reassesses all66 predictions:
55 executed finite component variants,5 partial,1 source-only,5 unsupported states.
Partial rows explicitly retain live error containment versus original interruption,
host cleanup versus native candidate ownership and artificial successive producers.
A66's original post-bonus opaque presentation failure is unsupported; nearby
registration/pre-cache failures are tested without claiming that original helper.
Every row separately retains UNVERIFIED native/original-runtime dependencies.

Windows reports audible frequency modulation unsupported. No native length is
inferred from WAV/decoder size and no MediaPlayer SpeedRatio is claimed as BASS
attribute1 equivalence. Nonempty decoder/media/device paths remain source-only.
WAV chunk integrity and deterministic generation establish neither decoder success,
seamless looping, physical scheduling, acoustic fidelity nor audible quality.
The owner retains audible qualification. Deferred scoring discrepancies, original
runtime cases, overlapping producers, R15/QA13/E042 and migration remain untouched.

# 3. Pseudocode

```text
RECONSTRUCTED expectations from CONFIRMED STATIC E041:
    changed int(P): C++ once; optional frequency(C/N); classify; bonus; cache
    interior positive interval: smooth/present U; U!=0 optional frequency +queue start
                               U==0 queue Pause; then accumulate P
    frequency: Length(current,0)>8192 -> fresh target; finite conv.i4 Single40000*r
               Attribute1 Single(Min100000,unchecked20000+k)
    refresh: selectors; old active1 Stop; resolve spin; Acquire(false); store;11 then2
    start callback: Attribute2(fresh channel,fresh gain); Play(fresh channel,false)
    Pause callback: Pause(fresh channel)
    settings: one captured gain -> fresh slider0,slider1,spinner
    completion: cleanup; evaluated; positive minimum -> stored-end sample; later grade

VERIFICATION:
    inject independent markers/native operands/errors/interleavings
    record intermediate mutations, logical requests and expected/actual values
    compare matched accounting sessions; audit media paths without opening media
    validate generated bytes/fingerprints; retain failures and justified oracle fixes
    run unchanged1344 historical checks and all development gates on Windows .NET8
    publish exact-source findings; stop after QA12
```
