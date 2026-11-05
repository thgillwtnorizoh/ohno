# 1. Result

QA11 — Work1 independent continuous slider audio verification, 2026-11-05.
Authorized baseline `8bd0433528cbceb0bffcaa4bf29119776deaf0e9`.
R13 production source `221c6e1d55a488489155acdadd6e8ed32243a014`; its production
files are unchanged at the baseline. Publication and Windows execution provenance
are finalized in `verification/windows-provenance.json` after the gate completes.

**One conditional production-model discrepancy: QA11-D01.**
`SharedSliderChannels.SettingsChanged` recomputes the gain for the second channel,
where E040 M3b11 retains one local value for both writes. With an artificial
synchronous backend interleaving, actual values are0.1/0.2; expected0.1/0.1.
The failing assertion is retained. No production repair is part of QA11.
Normal noninterleaved settings refresh passes. Windows reachability and audible
impact of this conditional difference are **UNVERIFIED**.

Local supplemental run: **247 named headless checks,246 pass/1 fail**:
234 executed component controls (233 pass), four asset controls (four pass),
nine source audits (nine pass). Separate actual Windows empty-resource adapter:
**6/6**. Total new named checks **253**,252 pass/1 discrepancy. Files, repeated
runs, E040 matrix rows and R13 development groups are not additional QA cases.
Local execution uses the installed .NET10 runtime with the net8 target and explicit
roll-forward; it is supplemental to the established Windows .NET8 gate.

Windows .NET8 historical/build/development outcomes and exact source SHA are
recorded below after execution. A failing QA11 gate is intentional while D01
remains unfixed; this report does not declare complete R13 verification.

Preserved first run:236 checks,228 pass/eight fail. Seven were corrected QA11
expectation/setup errors, with exact first artifacts and independent reasoning in
`oracle-corrections.md`. G08 was not weakened. R13's historical fixture-admission
failure and subsequent corrections remain unchanged in their original locations.

No original executable/archive/CLR/native method was inspected or executed for
QA11. No gameplay window, physical input, media opening, audio playback, listening,
capture, debugger or E020 experiment ran. All production/development code,
historical research/QA fixtures/assertions and journal prefixes are preserved.

# 2. What they found

## Authority and provenance

Expected operations derive from E040's existing report/evidence, with E037–E039
and E015–E018/E021–E023 for shared resources and discrete ordering. The harness
links actual production Beatmap, CircleSession, CircleHitSounds and
LocalHitSoundAssets files; its own backend dictates numeric activity results and
records acquisition, attributes, Stop, Pause and Play separately. No proprietary
resources are needed. R13 development tests were inspected for interfaces, not
copied as expected-value oracles.

| Preserved R13 source | Windows run | Historical result |
|---|---|---|
|c4d7e81dbde807dcb60d18b05f861b6f8849b87c|42009465622|Historical QA passed; new fixture lacked ApproachRate and failed admission. Not a production defect.|
|ab2fa0ab62fcbd132c119a72583e82f7e2108a68|42009817763|48 development groups passed.|
|f8d7370a7110b063fd8e70b5b0af14efa560cb30|42010084278|53 development groups passed.|
|221c6e1d55a488489155acdadd6e8ed32243a014|42010372847|54 groups plus separate Windows empty-resource group passed.|

The QA11 result manifest hashes actual linked sources, E037–E040 evidence,
generators, R13 historical failure/provenance files and QA11 harness sources.
Sixteen byte-preserved `.osu` files in `Inputs/` capture fifty case-input references;
the same text is generated independently by the harness and retained per outcome.
`verification/initial/manifest.json` pins the first harness. Results retain expected
and actual values, event snapshots and backend operation attempts/failures.

## Global timing and sample state

T01–T26 independently distinguish post-song-load cursor−1 initialization, empty
lists, strict rescan `< unchecked(time+5)` from inclusive forward `<=`, base-only
versus all-record cursors, inherited records, sample index versus cursor index,
first-future termination, ordering, explicit sort/ties, signed wrapping, format
gates and retained selectors after reset. The equality control selects record0 on
the first full scan and record1 on initialized forward progression at the same
time. Null enumerable and malformed operands are engineering rejections, not a
claim to implement the original nullable-list/delegate/catch graph.

I11 makes head/tick timing, parent index19/volume91 and global point/index2
marker-distinct: continuing state uses the global time+5 cursor, while the tick
uses deadline+2 and timing volume23. Same-map identity is an adapter guard,
not a hidden veto in the pure model. Raw CLR-zero/list-setter/editor/seek states
remain outside the supported post-load entry contract.

## Resources and shared channels

R01 executes all18 prepared row/bank/plane combinations. Dynamic controls cover
all six families, distinct mode0/mode1/mode2 tables, index0 map exclusion,
unsuffixed1/suffix2/decimal17, WAV→MP3→OGG probing, provider gates, skin/default
fallback, cached−1 versus0, first nonnull bytes versus loader return values,
loop flags, collisions, map invalidation and complete cleanup. P08 proves that
prepared mode2 lookup and callback submission do not perform more provider I/O.
Original native acquisition results remain opaque; managed decoder results are
not called BASS return values.

C01–C16 verify one shared pair, separate current/last selectors, independent row
replacement, nonzero-active1 Stop only, Acquire(false), assignment/attribute order,
force/application gates and partial mutations at controlled failures. Masks select
row0 through `layered || !(mask&2)`, row1 through `mask&2`. No Finish/Clap loop is
invented; later exclusion does not stop an already active row. A new discrete
deadline is not required to submit continuing work.

D01–D16/P04–P06 exercise callbacks deterministically: execution-time handle/gain
reads, a second handle read between Attribute and Play(true), current-pair Pause,
captured pan/current targets, repeated pending starts, active1 suppression,
manager short circuit, cancellation and artificial shared-producer replacement.
The101/202→301/402 queued-Pause control targets301/402. Queue cancellation is a
separate engineering operation. Recording Play is a logical call, not audibility.

Gain controls independently distinguish unchecked Int32 multiplication, Single
conversion/order, current-point Max8, un-floored map fallback and no-song100.
E1/U1/V17 yields replacement bits0x378e9b38 versus start0x378e9b39. Stored pan
positions use the two E040 Single multipliers; presentation publication is an
engineering bridge, not equality to original F0bd2 state.

## QA11-D01: one settings gain must remain captured

Authority: original MethodDef06003b11/T07b9, body SHA256
`81b56cede5a835a944737fcdcd34c15ff744d821dffd406bbfdaff8f1d5f4dc3`.
E040's published CIL-relative sites:0018 `stloc.0`,0019 read channel0,
001f `ldloc.0`,0020 call M3146;0026 read channel1,002c `ldloc.0`,002d call M3146.
The gain is retained while target channels are reread.

Finite input: current point volume25, E80/U50, shared channels101/202. First
backend Attribute changes U to100 before returning. Expected both attribute2
values0.1 (Single0x3dcccccd). Production gives second value0.2
(Single0x3e4ccccd). G07 proves ordinary same-state refresh; P01 independently
checks the published local-use sites; G08 preserves the discrepancy. Exact input,
observations and affected component are in `discrepancy-QA11-D01.json`.

Smallest Work2 correction scope: capture replacement gain once inside
`SharedSliderChannels.SettingsChanged`, retain fresh target-field reads and
existing exception ordering. QA11 makes no correction and starts no new milestone.
This artificial interleaving establishes a model difference, not ordinary WPF
reentrancy, native behavior, volume imbalance heard by a human or a scoring defect.

## Session, gameplay and engineering lifecycle

I01–I17/P02/P03/P10 exercise actual parsing and sessions: head hook before raw64,
continuing before audio-index comparison, second call after store/before discrete
dispatch, same-time no duplicate, latest-only jumps, tracking-loss consumption,
reentry, repeat independent of checkpoint success, terminal/aggregate separation,
exact-end continuing after cleanup and after-end omission. Pending components are
not drained. Current FollowEligible may be false while stored Tracking stays true
after all checkpoints; the corrected oracle preserves that E018 distinction.

Five matched-session modes (enabled, disabled, missing, backend failure and callback
failure) compare intermediate ledger events/counters, scores/K, accuracy/combos,
head/checkpoint/aggregate states, circles/spinner states, health/calibration,
failure/recovery, presentation and R12 discrete/circle event sequences. Optional
effects failures remain contained by production session policy. Private calibration
emits no sound; recovery H160 and HistoricalR09/E036FreshOrdinary split remain.
This does not reopen deferred cross-beatmap scoring discrepancies.

Fresh overlapping/touching maps are rejected by the preexisting R05 parser before
continuous admission. P07's artificial definition directly tests continuous
overlap admission. Thus no claim is made that a freshly loaded overlapping map
keeps gameplay/discrete support through that earlier parser restriction. Other
continuous-only limitations retain discrete preparation/gameplay where admitted.

Nine audits read real Windows wiring, selected-map identity, event detach/attach,
generation guards, pause/resume/restart/disposal, source-only decoder failures,
render publication, independent generator and the manager interval/pending proxy.
The proxy is an engineering substitute for original M2275/F136e, not per-slider
channel ownership. Nonempty adapter/media paths were **SOURCE INSPECTION** only.
The separate six empty-resource controls execute actual WindowsSliderLoops but
never reach its MediaPlayer construction/open branch. Empty cancelled preparation
has no decoder work and returns normally; it is not a cancellation guarantee for
nonempty resources. Logical activity3 Pause versus0 Stop remains source-audited
for nonempty players. Physical pause, decoder success, stream repetition and
device behavior remain unverified.

Four asset groups independently walk RIFF chunks and check exact six names,
PCM16 mono44100Hz/88200 byte rate/blockAlign2,22050 frames/0.5s,
44100 data bytes/44144 file bytes, chunk bounds and nonempty waveform. Two
controlled generator invocations match; pre-loop manifests preserve all fifteen
historical files. W05 proves actual provider/catalog byte correspondence. Generator
provenance uses independent oscillators with no proprietary sample input.
Header integrity is not Windows decoding, seamless looping or audible quality.

## Coverage, reproduction and limits

`e040-coverage.json` retains every original L01–L56 prerequisite/prediction and
maps it to named actual outcomes. Reassessment:44 finite managed variants,
seven partial controls, one source-only lifecycle control, three unsupported
original paths, one conditional discrepancy. This is independent of R13's
18/35/3 self-assessment. Multiple controls may share a case; no matrix row adds
to the QA count. Shifted times/marker identities and artificial native operands
are explicit. None is an original-runtime observation.

Reproduce on Windows with the already installed .NET8 SDK:

```text
pwsh -NoProfile -File re/qa/qa11/Run-QA11.ps1 -ResultPath /absolute/qa11-results.json
```

The wrapper generates independent packs in its own checked temporary directory,
runs headless checks, then empty Windows checks even when G08 fails. It exits1
while any discrepancy remains. The minimal CI addition retains every earlier
command and adds QA11 filters/one final step. Compressed exact results and SHA256
are emitted into the existing job log for deterministic evidence recovery.

Owner-only qualification still requires separately authorized media/driver/device
and human listening checks. BASS scheduling/equivalence, original native activity,
seamless looping, physical timing/pitch, overlaps, arbitrary skins/providers,
seek/editor/reused manager/device transitions and universal osu!stable fidelity
remain **UNVERIFIED** or unsupported. No E041/R14/QA12 is begun.

# 3. Pseudocode

```text
read published original-static evidence; inspect actual production interfaces
link production into independent headless harness
derive finite timing/resource/channel/session expectations
inject activity results, markers, backend exceptions and controlled interleavings
record expected/actual, mutations, queue submission and execution separately
audit Windows nonempty media paths without opening media
validate independent loop bytes and preservation of fifteen old fingerprints
preserve first failures; prove seven QA oracle/setup corrections independently
retain settings-gain discrepancy G08 unchanged
run existing Windows build +1091 historical QA +all development groups
run QA11 headless and empty Windows controls; preserve the red discrepancy gate
publish findings for targeted owner-coordinated Work2 correction; stop after QA11
```
