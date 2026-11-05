# 1. Result

QA10 — Independent Discrete Slider HitSound Progression & Resource Verification.
Work1, 2026-11-05 Asia/Bangkok. Verification only; production, development checks,
historical assertions, original evidence and fixtures are unchanged.

Initial suite publication: Windows .NET8 gate **PENDING**. Local preflight uses
the already installed .NET10 runtime with major roll-forward and is not the final
.NET8 gate. Final execution provenance and classification will be appended after
the existing Windows runner completes. No game, WPF window, original executable,
native decoder, audio output, physical input or listening experiment is used.

Production baseline is `932085ba96cf8c79bf2aec86b4ac316b9b247e64`, including R12's
correction `ad8cf337ee332259955cf99c820cf4f51b0553d8`. The historical R12 runs remain:

| Source | Windows run | Meaning |
|---|---|---|
| `27c2fde21676869b851ac4755a2d6392251dc9fc` | [41877100114](https://github.com/thgillwtnorizoh/ohno/actions/runs/41877100114) | WPF and862 historical checks passed; development harness CS8602, zero R12 assertions executed |
| `ad8cf337ee332259955cf99c820cf4f51b0553d8` | [41879777131](https://github.com/thgillwtnorizoh/ohno/actions/runs/41879777131) | 862 historical checks and40 R12 development groups passed; three independent tick files checked |

The first run is a harness compilation failure, not a demonstrated production
behavior defect. Its original record is retained and fingerprinted by QA10.

# 2. What they found

The new net8.0 headless harness links the actual production Beatmap, CircleSession,
CircleHitSounds and LocalHitSoundAssets sources. It does not copy a replacement
slider implementation. Expected operands come from E039 and supporting published
research; R12 development checks are inspected for interfaces, not used as oracle.
Every named case records its authority, concrete input, expected/actual operands
and execution classification. Source-only Windows/renderer bindings are explicitly
separate from executed component cases. Artifacts, repeated runs and development
groups do not add independent cases.

The main finite control shifts E016/E039's slider to start4000 and X64: beat500,
multiplier1, length300, velocity200, tick spacing100. Independently expected mixed
schedules are `[4500,5000,5464]`, `[4500,5000,5500,6000,6500,6964]`, and
`[4500,5000,5500,6000,6500,7000,7500,8000,8464]` for one/two/three traversals.
The edge subsets are empty, `[5500]`, and `[5500,7000]`. Literal deadlines and
layer-volume vectors are asserted independently of production preparation.

Coverage distinguishes parent raw mask from normalized head bits; missing/empty
and malformed edge arrays; independent list fallbacks; format13/14 edge0 base
selection; parent payload from head defaults; and fresh source ownership. Head
300/100/50 hooks execute before raw64 accounting, while MISS does not reach the
positive hook. Stored head start+5 and head X are used even for late evaluations.

Audio index starts/resets0, becomes−1 on a reached pre-start/outside update, scans
the leading schedule prefix excluding its final entry, consumes tracking-false
deadlines, and selects only the latest due item. Direct audio controls preserve
separate gameplay counters. Actual sessions demonstrate audio after failed
checkpoint classification, skipped catch-up sounds, reversal quotient/remainder,
no replay on reentry, and no direct final-checkpoint sound. Positive aggregate
requests precede parent grading even with a failed final checkpoint or pending
components; no pending drain is introduced. Zero aggregate skips that hook.

Tick selection uses deadline+2, parent base/addition, timing index/volume and a
dedicated resource. It has no edge observer, layer expansion, floor8 or attenuation.
Edges use start/subset/end+5, independent arrays and timing index/volume; observer
precedes Normal→Finish→Whistle→Clap. Literal standard-volume controls include
V0→6/8/6/6, V9→7/9/7/7 and V23→18/23/19/19. Parent custom preparation never
creates a direct discrete custom arm.

Marker-distinct providers verify all three tick banks and planes0/1/2, decimal
dynamic keys, `.wav→.mp3→.ogg`, index0 map suppression, cache-only resolution,
missing/−1 fallback versus handle0 preservation, skin/default gates, first bytes
versus subsequent load failure, and cache cleanup/replacement. Local library
controls cover actual generated assets, parser ownership, collision rejection,
cancellation and map identity. Recording queues cover order, pause/resume,
generation cancellation, disposal and controlled dispatch/backend errors.

Matched sessions compare intermediate raw results, component counts, ledger
events/outcomes, score/K/accuracy/combo, circle hooks, spinner observations, health
calibration/mutation and failure/recovery state with prepared, absent or throwing
effects. Private calibration creates no live slider sound. HistoricalR09 versus
E036FreshOrdinary and the H160 recovery guard remain distinct.

The renderer's last published map-space ball X is an **ENGINEERING** bridge. Its
finite stored operands and pan requests are executed; renderer publication,
Windows stream attachment and absence of synchronous decoding in OnSlider are
**SOURCE INSPECTION**. This does not measure frame publication lag, WPF concurrency,
decoding, voice overlap, physical playback or original F0bd2.F2cf6 equivalence.

Independent WAV validation walks RIFF chunks, checks total10628/data10584 bytes,
PCM1/mono/44100Hz/16bit/block2/byteRate88200, validates bounds, compares two
controlled generator invocations and captures fingerprints. Adding the three tick
files preserves separately captured hashes for all twelve circle assets. The
generator is mathematical source, has no proprietary sample input and performs no
playback. Header agreement does not prove decoder success or audibility.

E039 remains **CONFIRMED STATIC** original authority; finite expectations are
**RECONSTRUCTED**. Covered execution can become **VERIFIED COMPONENT** only after
the gate. Continuing loops S44–S47, native BASS scheduling, exact clock interleaving,
arbitrary original skin/.osz2 behavior, spinner audio, unsupported preparation and
universal gameplay equivalence remain **UNVERIFIED/unsupported**. No all48 dynamic
reference-case claim is made; the coverage map records partial and omitted cases.

Failure preservation: local attempts1/2 stopped before assertions (offline NuGet
audit and new harness CS8600). Attempt3 executed227 cases,224 passed and3 failed.
Its exact JSON/log and byte-identical five harness source snapshots are retained.
The corrected expectation proof is in `verification/oracle-corrections.json`.
No production repair or historical assertion change was made.

# 3. Pseudocode

```text
generate two independent packs: twelve circle files, then three tick files
capture old circle hashes before adding ticks
link exact production sources into net8 headless executable
for each independently specified named case:
    record deterministic .osu text / artificial operands / original authority
    invoke production parser, preparation, session, selector or recording queue
    assert identities, ordered events and intermediate state, not just counts
    preserve actual values and exceptions even when an assertion fails
walk generated RIFF chunks independently; compare controlled fingerprints
audit Windows/renderer bindings as source-only, never instantiate gameplay WPF
write structured JSON + SHA256 + compressed CI-log chunks
rerun unchanged historical862, all development groups and Release win-x64 build
publish actual Windows provenance and bounded conclusions; stop after QA10
```

Reproduce on the existing Windows runner with its selected installed .NET8 SDK:
`pwsh -NoProfile -File re/qa/qa10/Run-QA10.ps1 -ResultPath <outside-source-results.json>`.
Repository shell use retains the directed RTK prefix. No SDK installation is needed.
