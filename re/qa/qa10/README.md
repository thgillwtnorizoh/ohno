# 1. Result

QA10 — Independent Discrete Slider HitSound Progression & Resource Verification.
Work1, 2026-11-05 Asia/Bangkok. Verification only; production, development checks,
historical assertions, original evidence and fixtures are unchanged.

**PASS — VERIFIED COMPONENT for the covered finite discrete behavior.** Windows
tested source: `b469a6549e2fb5d96e55984fb70a6583b1c9c8e1`.
[Run41942929741/job126967963661](https://github.com/thgillwtnorizoh/ohno/actions/runs/41942929741/job/126967963661)
completed successfully on Windows2022, installed SDK8.0.427/runtime8.0.33,
2026-11-05 00:39–00:40 UTC /07:39–07:40 Asia/Bangkok.
QA10: **229/229 named checks**, comprising **225 executed component cases and four
source-only audits**. Historical independent QA: **862/862**. No confirmed
production discrepancy. Three new-oracle/audit failures were preserved and proved
incorrect separately; no production repair occurred. All development checks pass,
including R11 **32/32** and R12 **40/40**. WPF Release win-x64: **zero compiler
warnings/errors**. The checkout action's Node20 advisory is separate from build
warnings. No game, WPF window, original executable,
native decoder, audio output, physical input or listening experiment is used.

Published suite and final evidence are on canonical `main`; the final publication
SHA is supplied in the handoff. This final evidence commit changes no tested
production/harness/generator/workflow/fixture bytes. No authorised exception.
Local .NET10 major-roll-forward preflights are supplemental, not the .NET8 gate.

| Executed Windows suite | Passed / required |
|---|---:|
| QA01 |90/90|
| QA03 |56/56|
| QA04 |43/43|
| QA05 |66/66|
| QA06 |99/99|
| QA07 |159/159|
| QA08 |115/115|
| QA09 |234/234|
| Historical independent total |862/862|
| QA10 component cases |225/225|
| QA10 source-only audits |4/4|

Development gate: R06 targets4/fixture loads4, R07 8, R08 13, C01 4, C02 14,
R09 15, R10 23, R11 32 and R12 40; all passed. Generated files and these
development groups are excluded from the independent QA10 case count.

`verification/windows-results.json` preserves exact expected/actual values and
all case inputs; SHA256
`f10d0d02d9dd8b80eee6dda6dbdd2c846d259977211995930dc84286a52db50e`.
Decoded complete job log SHA256
`004d87737c56c6142cdfcf8344ea662f7de46b7ae3d925966ca1436336226ca1`.
The JSON was recovered from all ordered gzip/base64 log chunks and matched its
runner-emitted hash. `windows-provenance.json` records job steps and gate counts.
Runner-read raw source hashes remain intact; the packet checker permits only exact
bytes or LF/CRLF checkout conversion when matching local sources. Initial failed
harness snapshots and evidence files are byte-preserved by local attributes.

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
and execution classification. There are40 metadata,33 progression,22 tick-selection,
38 edge-selection,3 head-selection,34 resource,28 session,22 lifecycle and5 generated-
asset component cases, plus4 source audits. Source-only Windows/renderer bindings are explicitly
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
**RECONSTRUCTED**. Covered finite execution is **VERIFIED COMPONENT**.
Continuing loops S44–S47, native BASS scheduling, exact clock interleaving,
arbitrary original skin/.osz2 behavior, spinner audio, unsupported preparation and
universal gameplay equivalence remain **UNVERIFIED/unsupported**. No all48 dynamic
reference-case claim is made; the coverage map records partial and omitted cases.

Failure preservation: local attempts1/2 stopped before assertions (offline NuGet
audit and new harness CS8600). Attempt3 executed227 cases,224 passed and3 failed.
Its exact JSON/log and byte-identical five harness source snapshots are retained.
The corrected expectation proof is in `verification/oracle-corrections.json`.
No production repair or historical assertion change was made.

Test visibility limits: the ledger exposes last event/outcome and cumulative
counters, not a subscription for every raw event inside one Advance call.
Matched tests compare those snapshots after every supplied action; head/tick/
aggregate callbacks additionally inspect intermediate ordering. No inaccessible
full internal event stream is claimed. S18's actual session analogue consumes one
failed pending item on the reached end update before aggregate; two remain pending.
The direct original all-pending entry and all native clock rereads are not reproduced.
S34 covers the positive custom-loop loader-flag example, not loop playback or all
filename predicate boundaries. S09/S35/S39 are partial as recorded in the coverage
map. Existing safety bounds, short timing/native paths and the continuous subsystem
are retained; playable gameplay can continue with unavailable effects.

`Inputs/` contains67 independently authored finite byte fixtures with a hash
manifest; additional generated inputs are preserved inline in execution JSON.
`verification/assets.json` separates independent expected PCM metadata from actual
tick fingerprints and historical circle hashes. No WAV contents are published.
Run `rtk proxy python re/qa/qa10/check_evidence.py` to verify packet integrity,
source correspondence, preserved failures and the bounded E039 coverage map.

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
