# Project ohno — Campaign C001
## Complete Original-Derived `.osu` File Interpretation, Beatmap Loading & Gameplay Preparation

**To:** Work 1 — Original Reverse Engineering & Independent Verification  
**To:** Work 2 — Reconstruction Engineering  
**From:** Headquarters — Mission Controller  
**Repository:** https://github.com/thgillwtnorizoh/ohno  
**Canonical branch:** `main`  
**Verified starting commit:** `0421f9249f494b4a86fdf59c7dd76cd9a664688b`  
**Reference archive:** `osubackup.zip`  
**Applicable standard:** UEP-001 — Subsystem-Complete Development  
**Campaign:** C001  
**Status:** AUTHORIZED — FULL SUBSYSTEM CAMPAIGN

---

## 1. Mission

Reconstruct the complete `.osu` file interpretation, loading and gameplay-preparation subsystem supported by the pinned original osu!stable executable.

This is our first campaign under UEP-001.

The objective is no longer to recover isolated parsing methods, individual slider branches or narrowly admitted beatmap formats.

The objective is to establish one coherent, original-derived beatmap representation and preparation system that the reconstructed client can depend on.

The campaign must cover:

- Original file-format recognition and version-dependent behavior.
- Complete supported section and field interpretation.
- Beatmap metadata and difficulty settings.
- Timing and inherited timing behavior.
- All original hit-object categories and relevant variants.
- Hit-object properties, modifiers, combinations and defaults.
- Slider path representations and preparation.
- Object ordering, timing, stacking and transformation.
- Event, break, sample and resource references.
- Mode-specific beatmap interpretation.
- Data validation, error handling and compatibility behavior.
- Integration with existing gameplay, rendering, scoring, audio, selection and difficulty-calculation consumers.

All significant functionality must be integrated into the reconstruction and independently verified.

**Minimum operational goal:** Replace the current fragmented beatmap-loading infrastructure with a unified, production-ready original-derived subsystem covering the complete declared behavior of the target executable.

A successful parser that merely accepts every file is insufficient. Prepared output must be functionally correct and usable by the relevant reconstruction systems.

## 2. Campaign authority

This authorization covers the entire C001 subsystem.

Work 1 and Work 2 may continue research, implementation, testing, corrections and integration without requesting a new Headquarters expedition for each method or newly discovered dependency.

The teams shall operate toward one shared completion specification.

Intermediate commits and checkpoints are expected.

They are not new milestones requiring separate permission.

Necessary corrections discovered during implementation remain within C001.

Do not start unrelated subsystems or expand into complete slider gameplay, new UI reconstruction or full star-difficulty mathematics merely because a dependency is discovered. Record such dependencies, implement the required integration contracts, and bring genuinely separate subsystem work to Headquarters for a scope decision.

### Research integrity

Every finding must retain its evidence classification:

- **CONFIRMED STATIC:** Established from original executable instructions, metadata or literal behavior.
- **RECONSTRUCTED:** Behavioral interpretation supported by original evidence.
- **VERIFIED COMPONENT:** Behavior exercised through an actual independently tested implementation.
- **ENGINEERING:** Reconstruction-specific policy or substitution.
- **UNVERIFIED ORIGINAL-RUNTIME:** Behavior not established through execution of the original client.

Do not equate passing reconstructed mathematical tests with original-runtime equivalence.

## 3. Initial repository assessment

Before modifying production code:

1. Read `AGENTS.md`, `re/Instruction.md`, both cumulative journals and UEP-001.
2. Review E004–E006, E010, E015–E023, E032–E036, E039–E046 and relevant dependent reports.
3. Review R04–R17 and QA01–QA15, including preserved discrepancies and owner observations.
4. Inspect the actual current beatmap, catalog, gameplay and star-admission implementations.
5. Identify every location that independently parses or prepares `.osu` information.
6. Inventory downstream consumers of parsed beatmap state.
7. Identify original executable methods, object factories, fields and runtime preparation stages that remain insufficiently investigated.

Relevant existing production areas include:

```text
reconstructed/CircleGame/Gameplay/Beatmap.cs
reconstructed/CircleGame/Selection/
reconstructed/CircleGame/Difficulty/
reconstructed/CircleGame/Rendering/
```

These paths are an initial inspection target, not an exhaustive dependency list.

The existing gameplay loader supports a restricted subset of historical format versions and hit-object features. R17 has an additional strict v14 circle-only reader.

C001 must address this fragmentation through a verified canonical representation rather than allowing independent parsers to accumulate conflicting interpretations.

## 4. Complete functional coverage inventory

Create a comprehensive coverage matrix before major implementation.

The matrix must identify the original-supported behavior for every relevant version, mode, data category and preparation stage.

At minimum, investigate the following domains.

### A. File recognition and syntax

- Version header and recognized versions.
- Text encoding and byte-order marks.
- Line endings, whitespace and comments.
- Section delimiters and unknown sections.
- Key/value syntax and duplicate properties.
- Missing properties and original defaults.
- Numerical parsing and culture behavior.
- Overflow, invalid values and malformed input.
- Version-dependent interpretation and compatibility.
- Original rejection, recovery and partial-loading behavior.

Do not assume contemporary osu! wiki behavior automatically describes the pinned historical executable.

### B. Complete `.osu` sections

Investigate the original's support for:

- `[General]`
- `[Editor]`
- `[Metadata]`
- `[Difficulty]`
- `[Events]`
- `[TimingPoints]`
- `[Colours]`
- `[HitObjects]`

Include recognized historical and mode-specific fields, defaults, absent-section behavior, unknown properties and relevant version transitions.

Distinguish properties used for gameplay from properties stored for editor, catalog, rendering or other consumers.

Do not silently discard recognized original data merely because the current prototype does not use it.

### C. Timing and inherited properties

Recover and implement:

- Base timing points.
- Inherited timing points.
- Beat lengths and slider velocity.
- Meter and timing-point flags.
- Sample sets and sample indices.
- Timing-point ordering and replacement.
- Same-timestamp timing-point behavior.
- Relevant negative or exceptional timing values.
- Effects and inherited control state.
- Slider duration, tick generation and timing conversion.
- Original integer/floating-point conversion behavior.

Preserve original preparation order and boundary conditions.

### D. Hit-object construction

Inventory and implement original-supported parsing and preparation for:

- Hit circles.
- Sliders.
- Spinners.
- Hold notes.
- Mode-dependent object interpretation.
- Combined hit-object flags.
- New-combo and combo-skip information.
- Hitsound bits and sample overrides.
- Object position and timestamp data.
- Repeat counts, lengths and endpoints.
- Object sorting and registration behavior.
- Missing, malformed and unusual object records.

The goal is complete supported data interpretation, not merely identifying an object's type.

### E. Slider data and geometry preparation

Include every original-supported slider path representation:

- Linear.
- Bézier.
- Perfect-circle.
- Catmull-Rom.
- Historical aliases and compatibility behavior.
- Duplicate control points.
- Degenerate paths.
- Invalid control points.
- Curve subdivision.
- Requested pixel length.
- Truncation and extension.
- Repeat traversal geometry.
- Tail and checkpoint positions.
- Slider timing, ticks and inherited velocity.

C001 must produce complete, correctly typed prepared slider data for the declared original format coverage.

Full slider input judgement, rendering and audio behavior belong to their respective subsystem campaigns, but C001 must provide the necessary data and integration contracts.

### F. Stacking and gameplay preparation

Investigate and implement original-derived:

- StackLeniency interpretation.
- Stack discovery and propagation.
- Stacking order.
- Negative stack behavior where applicable.
- Object-specific stacking rules.
- Version-dependent stacking.
- Coordinate displacement.
- Original position versus prepared position.
- Effective CS/AR/OD/HP preparation.
- Timing-related preprocessing.
- New-combo and break grouping.
- Object dependencies and registration.
- Preparation under supported gameplay modifiers.

Do not introduce a modern stacking algorithm as an undocumented substitute for original stable behavior.

### G. Events, breaks and resources

Cover original-supported:

- Break periods.
- Background references.
- Video references.
- Storyboard/event declarations.
- Colour declarations.
- Sample references.
- Filename and path interpretation.
- Resource resolution and missing-resource behavior.
- Event timing and ordering.

Separate complete file interpretation from playback or rendering of external resources.

C001 must preserve and expose relevant data even where the consumer remains a separate subsystem.

### H. Mode-specific interpretation

Inspect the original's beatmap loading behavior for the modes it supports, including standard, Taiko, Catch and Mania where applicable.

The file model must preserve mode-specific data and interpret the supported format accurately.

Do not misrepresent osu!standard gameplay support as complete gameplay support for all four modes.

Mode-specific gameplay engines are separate subsystem commitments, but relevant loading and preparation rules belong to C001 and must be inventoried, researched and either implemented or explicitly identified as a cross-subsystem blocker.

## 5. Architecture requirements

Implement a canonical beatmap subsystem with clearly separated stages.

Recommended conceptual structure:

```text
Original .osu bytes
        ↓
Encoding and syntax reader
        ↓
Version-aware section parser
        ↓
Complete beatmap document model
        ↓
Original-derived normalization
        ↓
Timing and object preparation
        ↓
Mode-specific preparation
        ↓
Immutable or versioned prepared beatmap
        ↓
Downstream consumers
    ├── SelectPlay / catalog
    ├── Gameplay
    ├── Rendering
    ├── Hitsounds and audio
    ├── Scoring and health
    └── Difficulty calculation
```

The final design may differ if evidence supports a better architecture.

However, every consumer must obtain its inputs through a consistent, documented contract.

### Required design properties

- Preserve distinctions between raw and prepared data.
- Preserve file version and mode identity.
- Preserve original object registration order.
- Preserve nullable, optional and version-dependent state.
- Preserve floating-point precision where supported by evidence.
- Avoid unnecessary mutable shared state.
- Avoid repeated independent file parsing by consumers.
- Make preparation deterministic for identical supported inputs.
- Support cancellation and failure isolation.
- Prevent stale cached data from crossing file revisions.
- Provide explicit provenance and compatibility status.
- Preserve original quirks rather than correcting them according to modern expectations.

One reader must not silently apply rules that contradict another reader.

## 6. Work 1 — Original behavior investigation

Work 1 shall research the full original loading and preparation subsystem.

This includes:

- Original parser and object-factory inventory.
- Complete relevant method/field dependency graph.
- Supported file-version behavior.
- Original defaults and error branches.
- Original type construction and metadata conversion.
- Timing and slider preparation.
- Stacking and coordinate transformation.
- Mode-specific loading behavior.
- Resource and event handling.
- Data dependencies supplied to gameplay and difficulty calculation.

Use static original-byte evidence and independent finite mathematical controls where appropriate.

Do not claim original runtime execution without actual original runtime evidence.

### Required research product

Maintain a continuously updated matrix:

| Feature | Original static evidence | Reconstruction status | Independent verification | Runtime status |
|---|---|---|---|---|
| Version-specific parsing | ... | ... | ... | ... |
| Timing inheritance | ... | ... | ... | ... |
| Slider geometry | ... | ... | ... | ... |
| Stacking | ... | ... | ... | ... |
| Mode-specific loading | ... | ... | ... | ... |

Every identified original branch must have a disposition: implemented, verified equivalent, externally blocked, out of declared scope or unresolved.

Do not mark a feature complete merely because its mainline case passes.

## 7. Work 2 — Reconstruction engineering

Work 2 shall implement the subsystem according to confirmed and reconstructed original behavior.

Research and engineering may proceed concurrently.

Do not wait for every original method to be recovered before implementing well-supported portions.

However, label unverified engineering choices and replace them when stronger original evidence becomes available.

### Integration obligations

The new subsystem must replace or correctly delegate existing parsing/preparation logic used by:

- Gameplay initialization.
- Local chart loading.
- Difficulty selection.
- Slider construction.
- Hitsound preparation.
- Chart-derived score metadata.
- Break and NewCombo preparation.
- Health calibration inputs.
- Star-difficulty preparation.

Preserve the accepted behavior of existing gameplay components.

Do not change R17 mathematics to force agreement with observed star displays.

If canonical preparation changes R17 inputs, document and test the resulting differences separately.

An existing supported chart must not become unplayable without a documented original-derived reason or an explicitly accepted engineering limitation.

## 8. Mandatory comparison corpus

Create a persistent, extensible `.osu` compatibility corpus.

The corpus must include original-format version variants, every supported hit-object family, curve types, inherited timing, complex metadata, resource references, stack cases, malformed-input controls, mode-specific cases and unusual boundary conditions.

Where possible, include independently generated charts with reproducible source generators.

Each case must record:

- Exact bytes and SHA256.
- Source or generator.
- Intended original file version.
- Relevant feature coverage.
- Expected parse state.
- Expected prepared state.
- Actual reconstruction result.
- Original-runtime observation, if available.
- Verification confidence.
- Known limitations.

Reuse QA15's 27 comparison beatmaps without modifying their historical evidence.

Expand with complete, independently justified cases rather than replacing previous fixtures.

Do not distribute proprietary music, original osu! assets or other unauthorized material.

## 9. Original-client comparison and R17 discrepancies

The owner has supplied direct star-display observations for all 27 QA15 comparison charts.

The reported comparison contains:

- Eight matching original-versus-ohno displayed ratings.
- Nineteen mismatches.
- No case where ohno reports fewer stars than the observed original.
- Significant differences in nearly-equal-angle and opposing-direction charts.

Treat this as owner-reported runtime evidence pending executable/version, chart-hash and cache-state corroboration.

The original executable behind E044–E046 remains pinned by SHA256, but the exact executable used for the owner's QA15 comparison has not yet been established.

C001 must investigate whether divergent file interpretation or preparation contributes to these differences.

It must not conclude that preparation is the cause without evidence.

Maintain the star discrepancies in the campaign discrepancy register.

Do not apply an unexplained global multiplier or per-map correction.

The existing E044–E046 research evidence and QA15 results remain immutable.

## 10. Independent verification and regression

Create a new independent C001 verification suite exercising actual production components.

Test at minimum:

- Every admitted file-format version.
- All supported section and field variants.
- Original defaults.
- Version-dependent differences.
- Timing-point ordering and inheritance.
- Every supported hit-object type.
- Every supported slider path type.
- Degenerate and extreme geometry.
- Break and NewCombo behavior.
- Stacking and position transformation.
- Object ordering and registration.
- Hitsound/sample metadata.
- Relevant resource paths.
- Mode-dependent data.
- Invalid and malformed files.
- Parser/preparation cancellation.
- File revision changes.
- Cache invalidation.
- Catalog and gameplay integration.
- Compatibility with existing score, health and audio behavior.

### Regression baseline

Preserve all 2,309 historical named independent QA checks, including QA15, and all retained development suites.

Rerun them against the changed production implementation.

Any failure must be explained, corrected or explicitly classified as a justified original-derived behavior correction requiring approval.

Do not silently update expectations to make tests pass.

Use exact-source Windows Release builds and CI.

Record compiler warnings, errors, SDK/runtime identity and tested commit SHA.

## 11. Human runtime qualification

Owner testing is mandatory before final acceptance.

Prepare a human-operable Windows corpus and testing procedure.

The owner must verify:

1. Installation/import into the original osu!stable where relevant.
2. Loading of the same file in Project ohno.
3. Chart identity and selection behavior.
4. Correct object count and timing.
5. Slider rendering and movement where supported.
6. Correct circle and slider positions.
7. Timing-point changes during actual gameplay.
8. Stack behavior.
9. Hitsound/sample behavior.
10. Breaks and other visible events.
11. End-to-end gameplay and Rank behavior.
12. Version-specific and unusual cases.

Comparisons must use identical hashed files and recorded build conditions.

Capture screenshots, logs or other reproducible evidence where relevant.

Agent-executed automated UI component checks are not substitutes for owner-operated Windows application testing.

Do not report human acceptance until it is performed.

## 12. Campaign coordination and publication

### Initial setup

Create a persistent campaign record:

```text
re/campaigns/C001/
    charter.md
    coverage-matrix.md
    dependency-register.md
    discrepancy-register.md
    original-behavior/
    corpus/
    verification/

reconstructed/campaigns/C001/
    implementation-status.md
    integration-matrix.md
    development-tests/
    windows-tests/
    verification/
```

Additional folders may be added as needed.

The charter must record the complete agreed functional domain and acceptance criteria.

Record UEP-001 as the adopted methodology without rewriting prior historical reports.

### Continuous work

- Work 1 and Work 2 share the campaign objective.
- Both workstreams use canonical `main`.
- Coordinate publication and verify the remote head before each push.
- Commit coherent, independently verified progress incrementally.
- Append status to the appropriate cumulative journal.
- Keep coverage and discrepancy registers current.
- Record tested source and CI evidence.
- Preserve every material failed gate and its correction.
- Never force-push or rewrite history.

Research and implementation may exchange findings continuously.

A discovered correction remains inside C001 unless it genuinely changes the campaign's scope.

### Lost-response recovery

The repository is authoritative.

Every committed checkpoint must be understandable without its original ChatGPT response.

Maintain:

- Current campaign status.
- Last verified commit.
- Completed coverage.
- Pending coverage.
- Active blockers.
- Next internal action.
- Test results.
- Owner-qualification status.

Do not depend on the chat conversation for essential campaign state.

## 13. Definition of completion

C001 can be reported complete only when:

1. The complete declared original-supported `.osu` interpretation domain is inventoried.
2. Every relevant supported version and data category has an implemented disposition.
3. Relevant supported mode-specific loading and preparation are addressed.
4. Canonical raw and prepared beatmap models exist.
5. Gameplay/catalog/difficulty consumers are integrated with those models.
6. All supported slider data representations are interpreted and prepared correctly.
7. Original-derived timing, stacking, sorting and property inheritance are implemented.
8. Version-specific and malformed-input behavior is verified.
9. Independent production regression passes.
10. Existing gameplay functionality remains operational.
11. Required Windows application behavior has been owner-qualified.
12. Significant discrepancies have been explained or explicitly accepted.
13. Coverage, evidence, limitations and reproduction procedures are published.

If a required item remains unresolved, C001 remains open.

A verified partial implementation may be published as a checkpoint, but must not be described as a complete subsystem.

An externally blocked item requires an explicit owner scope or acceptance decision. Do not close it simply because the investigation is difficult.

## 14. Prohibited shortcuts

Do not:

- Declare a subsystem complete after only one curve type, parser version or ordinary test.
- Replace original behavior with modern osu!lazer behavior without evidence.
- Copy proprietary source code or assets.
- Fabricate original-runtime tests.
- Re-label research controls as production verification.
- Change chart inputs to force an expected result.
- Fit star difficulty to human observations without explaining the mechanism.
- Bypass supported variants because their implementation is inconvenient.
- Silently break existing gameplay consumers.
- Rewrite historical journal entries or accepted evidence.
- Treat a passing build as proof of Windows playability.
- Initiate unrelated full-subsystem campaigns without authorization.

## 15. Required progress and final reports

Maintain the following report structure:

### 1. Result

- Campaign status.
- Exact repository commits.
- Completed functional coverage.
- Remaining coverage.
- Automated and independent verification.
- Windows and human qualification.
- Explicit blockers.

### 2. What they found

- Confirmed original behavior.
- Reconstructed behavioral interpretation.
- Production implementation.
- Dependency integration.
- Discrepancies.
- Limitations and uncertainties.

### 3. Pseudocode

Describe the actual data pipeline and its original-derived behavior.

Report complete subsystem status using the campaign coverage matrix, not the number of commits or research expeditions.

## 16. Execution order

Begin immediately with:

1. Verify the latest remote `main`.
2. Read applicable instructions, historical research and reconstruction evidence.
3. Establish C001's shared campaign files.
4. Build the complete functional/dependency inventory.
5. Inspect and map all existing parsers and consumers.
6. Prioritize the original loading and preparation dependencies that currently fragment the reconstruction.
7. Begin research and implementation under the same campaign.
8. Add the first complete end-to-end verified feature families.
9. Continue through the remaining coverage and integration work.

This is authorization to proceed through the whole campaign, not permission to stop after the initial audit.

Do not request a new expedition for each feature family.

### Final directive

**Complete the `.osu` file interpretation and gameplay-preparation subsystem as one coherent engineering commitment.**

Use incremental commits and continuous verification, but measure success by integrated functional coverage and original-derived correctness.

Stop only for a genuine scope decision, unavailable required resource, owner-only runtime qualification, or an explicitly documented blocker that cannot be resolved within the authorized environment.

**The campaign—not the individual experiment—is our unit of completion.**