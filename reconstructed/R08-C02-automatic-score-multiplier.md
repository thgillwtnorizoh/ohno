# 1. Result

R08-C02 — Automatic Legacy Score Multiplier Integration. Work2,
2026-11-03 Asia/Bangkok. Base: published/verified C01
`761431c31acbf11b26ea324c011fa65a177c7136`, descending from E034
`4c61be63265d54b19b257fb3f1c632760c9e7773`. Owner explicitly resolved missing C01
by authorizing C01 then C02 sequentially. C01 sourceb9f89745f4310a115dfeab70f3b093b8eecad99f
passed Windows run41635206573/job125914546857 before C02 edits began.
Read AGENTS/full standing contract (attachment/repository same bytes), reconstruction
journal, R08/C01 reports, E033 and E034 in full. No original binary inspection,
new expedition, E20, game/window, debugger, OS input or recording.

Implemented fresh metadata and Single/Double multiplier preparation in the shared
Beatmap source, exposed ScoreConfiguration.FromBeatmap, and changed normal map
loading to supply that automatic config before replacing active state. E33 ledger,
judgement/event ordering, combo/counts, slider/spinner mechanisms and C01 renderer
are unchanged. Source and bounded checks remain under reconstructed/. Only shared
CI change adds one step/path to the established Windows gate. Historical reports,
research, QA01–QA05, E20, existing checks and all fixture bytes preserved; journal
append-only. No permanent branch or force update.

Conditional controls: Mixed K2 /2392 versus engineering K1 /2236; Joined K3 /534
versus engineering K1 /438. Original owner-reported582 remains unresolved, never
fitted by selecting K4 or changing HP/CS/OD. New14 bounded development groups;
Windows build/255 QA/R06/R07/R08/C01 and multiplier gate initially PENDING. No local
SDK install or build; final actual source/run/log evidence follows after CI.
Windows commands and old fixture selection remain in CircleGame/README.md.

# 2. What they found

## Recovered metadata inputs

RECONSTRUCTED from E034 sections A–D, original static evidence (M1bc1/M1387/M1bc4/
M2d7d/M0ab7), not runtime observations. Beatmap.Load supplies the same captured
file lines to gameplay and LegacyScoreMetadata.ReadFresh, preserving source
identity. Metadata does not consume sorted circles, prepared maximum ends, slider
movement duration, audio duration or ledger counts.

- First start initializes from the first reached row before type dispatch, with
  the original -1 sentinel. Type dispatch uses Int32(type)&139. Last recognized
  row overwrites last-start text even if times decrease. Circle/slider stores its
  start; spinner stores start and separate end; Hold128 stores pre-colon end for
  both texts. Hold/unknown metadata controls do not enable unsupported gameplay.
- RowCount increments for each reached row, including unknown masked types.
  Recognized UInt16 counts wrap independently. Original fresh row count is not
  reconstructed by summing those counts. Gameplay parser still rejects unsupported
  types, overlaps/equal starts etc.; metadata-only probes expose this distinction.
- Numeric-event2 duration is unchecked(end-start), then unchecked sum. No union,
  clipping, positive-duration requirement or object-range intersection. Two signed
  unchecked elapsed subtractions then integer /1000 produce WholeSeconds. No
  Max(1,T), interval sorting or floating duration replaces them.
- Finite first/last Double values convert to Int32 by truncation. Fresh HP/OD/CS
  default to Single5. Modern>=13 values parse Single/clamp0..10; older supported
  formats use Byte syntax before clamping/conversion. Existing gameplay CS/OD/AR
  validation remains stricter where already established. No gameplay difficulty
  substitution/clamping redesign. E034's standard key/field aliases retain their
  reconstructed classification.

Metadata records first/last/last-end, N/B/T, version, recognized counts and HP/OD/CS.
Prepared multiplier separately exposes clamped Single density, Single pre-round
value and Int32 K. The helper is kept in linked Beatmap.cs so historical QA projects
compile the actual dependency without changes to Work1 files/project links.

## Arithmetic and integration

M1bc4 ordering: Single(HP+OD), +CS; Single(N)/Single(T), *8f; clamp0..16 preserving
NaN; add density; /38f; *5f; convert Single to Double; Math.Round(Double), midpoint-
to-even; finite Int32 conversion. Ordinary no-mod factor1 gives Double K. Positive
N/T0 yields infinity capped16; negative density clamps0; 0/0 stays NaN and rejects
explicitly at unsupported conversion. No guessed fallback, integer density divide,
Double reassociation or count of physical revolutions/components.

ScoreConfiguration.FromBeatmap feeds this prepared value directly to unchanged
E33 ScoreLedger. Live HUD/results already consume config.PolicyLabel and now show
AUTO E034 fresh .osu with N/T/B provenance. Restart retains config but creates a
fresh ledger; replacing map computes its own K. UI no longer uses engineering K1.
Explicit engineering overrides remain via the existing config constructor/preset,
including fractional K controls. The historical one-argument CircleSession overload
retains its documented K1 compatibility path for unchanged QA/synthetic callers;
the app always uses the explicit automatic factory, with no fallback on failure.

Unsupported/malformed/nonfinite metadata raises an explicit InvalidDataException
before session/audio replacement, leaving the old valid session paused. This is
ENGINEERING load error handling, not a claim that original M1bc1 rolls back partial
mutations. Native stream failure, cached reader format/reused difficulty, global/
selected/manager identity changes, automatic ScoreV2 policy selection, modifiers/
decorations and original x86 extended precision are not recreated. Original start/
end parsing outside the finite Int32 range is rejected, not assigned CLR-dependent
output. No original numeric equivalence beyond E034's bounded fresh/no-mod policy.

## Bounded checks and unresolved comparison

R08C02DevelopmentChecks links production model/ledger. Fourteen groups cover pinned
fixture hashes/K and both event-sequence score predictions; sparse/dense ordinary
spans; exact2.5 midpoint and neighboring Single values; HP variation; positive-N/T0
and T1/T2; spinner-end independence; break/overlap/negative duration; raw unordered/
unknown/hold rows; fractional time truncation; fresh defaults/modern clamp/older
Byte; unchecked break/span and UInt16 wrap; empty0/0/bad metadata rejection;
timing/path independence; all13 fixture preparations, fresh session config and
replacement selection; actual slider lifecycle accounting using automatic config.
These are bounded implementation controls, not QA06 or original game experiments.

Mixed conditional ledger inputs are E034's first300, slider head64/two8/terminal128/
parent300, spinner observed C1–C16 (800 component contribution)/final300, last300.
Whole300 count4, maxcombo7 and100% accuracy alone do not establish those exact raw
components. Joined conditional inputs head64/three8/terminal128/parent300 give one
300/maxcombo5. Tests execute explicitly supplied component values; they do not
simulate mouse/keyboard or pretend that a player achieved them.

Joined's reported582 cannot be inferred from its one300/maxcombo5 alone. The exact
original .osu bytes, metadata/cache/settings/policy and raw event provenance remain
needed by Work1 for that comparison. This milestone requests no replay or original
investigation, does not change the supplied fixture, and does not silently promote
E034's HP5 example to the original chart. Actual visual/human score verification
remains UNVERIFIED. Stop after C02: no R09, QA06, lazer/framework migration.

# 3. Pseudocode

```text
RECONSTRUCTED E034 fresh-file preparation:
    defaults HP=OD=CS=Single5; first/last=-1; N=B=0
    reached HitObjects row in raw order:
        initialize first before masked type dispatch
        update recognized UInt16 counter and last-start/end text where applicable
        N=unchecked(N+1), including unknown metadata type
    reached numeric event2: B=unchecked(B+unchecked(end-start))
    T=unchecked(unchecked(last-first)-B) / 1000  // signed Int32
    density=Single(N)/Single(T); density*=8f; clamp0..16 (NaN stays NaN)
    u=Single(HP+OD); u+=CS; u+=density; u/=38f; u*=5f
    D=Int32(Math.Round(Double(u)))  // original midpoint-to-even, finite domain
    K=Double(D)*1.0                // ordinary no-mod only

ENGINEERING app load:
    parse supported gameplay + fresh metadata from same file lines
    prepare automatic config with source N/T/B provenance; error -> preserve old session
    fully prepare audio -> replace with fresh configured session
    restart -> fresh ledger, same config; next map -> its own prepared config

UNCHANGED E33:
    actual once-only raw judgement with provenance -> conditional whole counters
    base amount -> old-combo bonus using K -> combo/max/snapshot mutation
    admitted unchecked score write -> Single accuracy -> HUD/results
```
