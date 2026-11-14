# C001 — Original-derived .osu interpretation and preparation

**Campaign OPEN.** See status.md for the latest exact tested source, Windows gate
and artifact. authorization.md and UEP-001 govern subsystem completion. A passing
finite checkpoint does not close the remaining domains in coverage.md or Work 1's
re/campaigns/C001/coverage-matrix.md.

## Production contracts

| Contract | Disposition |
|---|---|
| OsuDocument.Read / Parse | Immutable ordered source lines/entries and version; byte input owns a defensive original snapshot with exact SHA256. Normalized Entries are an ENGINEERING inspection view, not universal original semantics. |
| OsuDocument.ReadStage | Distinct original-derived metadata-refresh and runtime-object projections: case-sensitive CLR section parsing, prior-section retention on failed conversion, Variables/numeric enum support, stage-specific whitespace and colon handling. |
| OsuMetadataSettings.Prepare | Fresh defaults, version13 Byte/Single split, source-order OD-to-AR inheritance until explicit AR, mode-at-CS bounds, undefined-mode fallback, slider and StackLeniency clamps. Finite invariant parsing is an explicit engineering profile. |
| Beatmap.LoadCanonical / Load(CanonicalBeatmapPath) | Canonical application preparation. Typed requests carry cancellation. The actual GameWindow uses this profile. Prepared Beatmap retains raw document and settings provenance. |
| Beatmap.Load(string) | Historical admission compatibility over the shared core: explicit-field requirements and v14-only Bezier guard remain for existing callers. |
| CatalogChart.LoadCanonicalGameplay / LoadGameplay | Explicit canonical versus historical admission, with revision verification before/after loading. No cached Beatmap substitution. |
| LocalChartCatalog.Index | Metadata-stage interpretation with finite prepared defaults/clamps, exact masked category counts, separate runtime availability and PreparedSettings. Invalid-scalar nullable display and comment-tolerant indexing remain explicit ENGINEERING compatibility policies. |
| StarAdmission | Shared document syntax, retained strict research profile and unchanged R17 mathematics. It does not claim full original parser/preparation support. |

Each request has its own deterministic document; this is not a claim of one cached
parse across indexing, calculation and gameplay. Fingerprints prevent stale
revision installation/cache publication under the retained tests.

## Implemented path and object families

E006-derived Linear paths accept multiple controls, retain both flags on every
piece, and parse controls through Double→Int32→Single. E032-derived Bezier branches
cover admitted versions5–14: <=6 backward duplicate boundary;7–8 adaptive without
two-control bypass;9 uniform50*n samples excluding t=1 except two-control bypass;
>=10 adaptive weighted leaves with bypass. Common requested-length adjustment,
Single segments, Double cumulative lengths, movements and checkpoints remain shared.

Canonical heads use Double→clamp→Int32→Single before Single rounding. Runtime type
construction follows Circle→Slider→Spinner priority; metadata summary retains its
separate exact-category switch. Raw type and actual registered type survive through
health and NewCombo preparation. A retained hold category does not authorize its
missing standard gameplay factory.

Score metadata consumes the metadata stage; registration consumes the runtime
stage, so counts can differ. Finite score-summary errors retain prior writes and
expose PartialErrorLine/PartialError. This supports trailing comments without
claiming complete original key/error/notification/bookkeeping recovery. Other
incomplete metadata can still reject canonical gameplay under a documented finite
engineering policy.

## Verification and reproduction

Work 1 authority: original-behavior/01-parser-boundaries.md, E004–006/E032–046 and
verification/02-production-audit.md under re/campaigns/C001. Original evidence is
static; actual production controls are component evidence; original Windows runtime
and human input/rendering/listening remain unverified.

Run from repository root:

```sh
python re/campaigns/C001/original-behavior/check.py re/campaigns/C001/original-behavior/static-inventory.json
python re/campaigns/C001/corpus/build.py --check
python reconstructed/campaigns/C001/build_corpus.py --check
dotnet run --project reconstructed/C001DevelopmentChecks/C001DevelopmentChecks.csproj -c Release -- <repo>
dotnet run --project re/campaigns/C001/verification/Headless/C001.Headless.csproj -c Release -- <repo> <results.json>
```

The unchanged independent suite has106 explicitly declared original-static fields,
4 engineering controls and10 diagnostic probes. Probes are not semantic passes.
Strict execution exits nonzero on any declared field gap. This finite corpus is
not complete original behavior coverage. Preserve all2309 historical named QA and
retained development suites; the Windows workflow runs them unchanged and publishes
a self-contained x64 application plus evidence. Work 2 controls are not independent
QA. Earlier failed gates remain under verification/ with their corrections recorded
in the append-only implementation journal.

Persistent corpora:24 Work 2 fixtures here,49 Work 1 fixtures under re/campaigns/C001,
and27 unchanged QA15 comparison charts. Every new fixture has reproducible source
and exact bytes/hash; no original assets/music are distributed. See
human-qualification.md for owner operation and matched original comparisons.

## Remaining acceptance gates

Complete original version/field/default/error inventory; full P/C/alias/degenerate
geometry; stacking/ties/transforms/modifiers; mode-specific factories/preparation;
complete events/Variables/resource interpretation; native culture/exceptional
conversion and general partial recovery; full independent branch/variant coverage;
owner qualification. The19 reported R17 display mismatches and historical Joined
Bezier discrepancy remain open without fitted multipliers or map patches.

Read status.md, both coverage matrices, consumer-inventory.md, discrepancies.md,
Work 1's dependency/discrepancy registers and both journals before continuing.
Fetch/reconcile canonical main and use an expected-head non-force update. Research
and implementation exchange findings continuously; historical evidence stays intact.

## Issue #2 finite P checkpoint (2026-11-14)

Canonical finite P now implemented/tested at a6c7dabc65372f83641926b89384cd14b3813a71.
See06-issue2-perfect-integration.md and current status. SliderPathKind overload
retains bool L/B adapter; SourceKind/PreparedKind/SourceControls/source-circle and
RawPath distinguish original identity from prepared fallback/translated geometry.
Existing shared tail/traversal/consumer and finite stacking contracts are reused.
Windows P299 checks and all retained gates pass; new independent P QA remains
Work1's next task. Prior statements about unimplemented P describe earlier/full
coverage; Catmull/general legacy/editor/exceptional/zero-output behavior remain open.
