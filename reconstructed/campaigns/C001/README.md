# C001 — Complete .osu interpretation and preparation

Status: OPEN — Work 2 checkpoint 01, verification pending. Authority: authorization.md
and UEP-001 (subsystem coverage, evidence integrity, continuous preservation and
owner acceptance). Canonical main baseline: 0421f9249f494b4a86fdf59c7dd76cd9a664688b.
No exceptions. Work 1 owns independent C001 QA and original investigation.

## Implemented checkpoint

OsuDocument provides ordered immutable source lines/section entries, defensively owned original byte snapshot and exact
byte SHA256, format identity and encoding detection. Unknown sections/fields and
all raw object/event/timing payloads survive. This is an ENGINEERING retention
contract, not a claim that every retained property has original semantics. Raw mode
identity remains in General entries; a fully typed raw model is still pending.

Gameplay, catalog and strict R17 admission use the shared syntax. The application
uses Beatmap.Load(CanonicalBeatmapPath), carrying the selected canonical profile and
cancellation; catalog calls LoadCanonical. String Beatmap.Load preserves historical
v14-only Bezier admission boundary as a compatibility wrapper over the same core. Gameplay attaches
the source document to its prepared Beatmap and passes it directly to fresh score
metadata preparation. The headerless ReadFresh(lines,version) compatibility API
remains for existing independent callers. No mutable cached document is shared
between file revisions; catalog fingerprints verify captured loads before/after.
No claim of a single cached parse across catalog/calculation/gameplay requests.

E032-derived Bezier sampler selection now covers admitted versions5–14: <=6 uses
backward duplicate equality;7–8 adaptive without two-control bypass;9 uniform50*n
samples without t=1 except two-control bypass;>=10 adaptive with bypass. Existing
length adjustment/movement/checkpoint consumers retain their contracts. E006-derived
Linear paths now accept multiple controls; every linear piece carries both original
flags. Linear controls now use Double→Int32→Single, correcting the old Single-only
endpoint path. Head Linear parsing still has the historical Single conversion;
its extreme-rounding discrepancy is tracked, not hidden.

## Reproduction and evidence

- `python reconstructed/campaigns/C001/build_corpus.py --check`
- `python re/tools/check_bezier_geometry.py re/evidence/32/bezier-geometry.json`
- `dotnet run --project reconstructed/C001DevelopmentChecks/C001DevelopmentChecks.csproj -c Release -- <repository>`
- Existing Windows workflow `.github/workflows/r04-build.yml` runs every retained
  historical suite, C001 development checks, Release build and portable publish.
- `human-qualification.md` describes owner-only operation; all original runtime,
  physical input/rendering/listening and final human acceptance remain pending.

Development checks are Work 2 component evidence, not new independent QA. Preserve
all2309 historical named checks and all retained development suites. Passing them
cannot prove the still-unimplemented coverage in coverage.md.

## Data pipeline / reconstructed pseudocode

ENGINEERING: bytes → BOM-aware reader → version header + ordered OsuEntries + hash
→ consumer-specific admitted projection; preserve all raw entries, including unused.
RECONSTRUCTED E006/E032: admitted L/B object → version-specific ranges/sampler →
raw Single segments → original-derived shared length adjustment → movements/ticks.
RECONSTRUCTED E034–E041: original registration order → score/NewCombo/sound sidecars.
ENGINEERING: captured file fingerprint verified before and after gameplay/calculation.
UNRESOLVED: complete typed fields/defaults/errors, P/C paths, arbitrary stacks,
equal-time registration and nonstandard preparation; see coverage and discrepancies.

## Coordination and recovery

Read status.md, coverage.md, discrepancies.md, consumer-inventory.md and both
journals before continuing. Fetch remote main before publication, integrate any
new Work 1 findings, use expected-head non-force publication. Work 2 does not edit
historical research/QA. Work 1 can add research and independent verification under
re/ and reference these contracts continuously. Checkpoint completion does not close
C001; original-supported inventory itself still needs Work 1 closure.

## Work 1 checkpoint01 integration

Authority: re/campaigns/C001/original-behavior/01-parser-boundaries.md at
 ee83d5cb3960f327aa30be056b8e5970eee736eb;277 static assertions checked locally.
OsuDocument.ReadStage separates metadata-refresh and runtime-object projections.
Both use original case-sensitive CLR FileSection parsing, including Variables and
numeric enum values, retaining prior section on failed conversion. Runtime filters
raw SPACE/underscore/comment prefixes before trimming; metadata preserves the full
first-colon remainder while runtime key values use only the second colon cell.
Normalized Entries remain an explicitly separate engineering/raw inspection view.

OsuMetadataSettings models fresh defaults, finite version13 Byte/Single parsing,
source-order OD→AR inheritance until explicit AR, mode-at-CS parse-time bounds,
StackLeniency and slider clamps. A bad supported scalar records partial prior writes
and stops this projection. Canonical gameplay rejects incomplete metadata rather
than claiming complete original partial-recovery behavior. It consumes prepared
CS/OD/AR and slider settings; catalog exposes PreparedSettings alongside display fields, using finite prepared
defaults/clamps and stage-aware metadata. Invalid-scalar nullable display behavior
remains an explicit engineering compatibility policy. Original metadata category
counts use exact masked values; runtime priority combinations remain open. Score metadata uses the metadata stage;
object registration uses the runtime stage, so counts can legitimately differ.

Still unresolved: original header recognition/default-version recovery, complete
field dispatch/error side effects, F2c2b culture identity/native CLR exceptional
parsing, resource/event semantics, general stacks, P/C and alternate factories.
This finite implementation must not be described as full parser equivalence.

## Compatibility and registered identity

Historical string Beatmap.Load and CatalogChart.LoadGameplay retain explicit-field
and v14-Bezier admission. Their canonical counterparts and actual typed GameWindow
request share the same document/preparation implementation with extended C001
semantics. Canonical runtime construction follows original bit priority, while
metadata summary counts exact masked values. Raw flags and actual registered type
remain separate through NewCombo/health. A hold category preserved by the catalog
still cannot become playable without its missing standard factory. Catalog ignores
comment lines for its engineering index contract; complete original refresh error
recovery is pending and is not certified by that policy.
