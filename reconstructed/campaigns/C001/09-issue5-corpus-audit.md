# 1. Result

| Mandatory UEP-001 revision 2.1 field | Verified checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work 2 Reconstruction Engineering / [Issue #5](https://github.com/thgillwtnorizoh/ohno/issues/5) |
| Status | **VERIFIED CHECKPOINT — functional read-only corpus audit**; stop bounded execution |
| Starting SHA | 3a26667bb80e085b17e0bfb6754176bbadd4d180; latest main verified before edits |
| Published / Exact tested SHA | Tool first source75bac84968f81be4405d5c1e80f809be61797e0a; exact successful CI8c5ba835f260401a56dea27367c6a6e9c55e9d84. Final evidence-only publication is this file's introducing Git commit; tool/tests/production identical to successful CI |
| Objective / Supported profile | Reproducible local .osu tree audit through actual catalog indexing and canonical preparation; primary normalized standard/no-spinner cohort plus separately counted other/unknown profiles |
| Functional changes / Integration | Standalone CorpusAudit executable, owner guide, focused tests/workflow; exact production source linkage for catalog/document/canonical preparation/score/session/health. Deterministic private-by-default JSON, hashes/features/stages/failure families/duplicates, bounded traversal, read-only source scan, cancellation and error isolation |
| Evidence / Confidence | VERIFIED COMPONENT: Work2 development on unchanged synthetic fixtures and new mixed controls; ENGINEERING: scanner bounds/privacy/taxonomy/provenance. Independent Work1 tool QA pending; no original-runtime equivalence |
| Tests / Environment | Local Linux .NET SDK8.0.427/runtime8.0.33,239/239PASS; clean exact75bac849 reproduction239PASS. Windows2022 and Ubuntu focused Release at8c5ba835:239/239PASS EACH, zero skipped platform controls,0compiler warnings/0errors; SDK8.0.428/runtime8.0.34 verified in downloaded results. Both artifact digests/sizes/source markers and12 report seals PASS; actual downloaded Linux package executes QA1527/27 canonical and consumer-ready |
| CI / Artifacts | [Successful focused run43611606831](https://github.com/thgillwtnorizoh/ohno/actions/runs/43611606831); Windows job132497651512, Linux132497651761. Download links/digests below and ../../CorpusAudit/evidence/ci-seal.json |
| NOT RUN | Owner's actual collection NOT PROVIDED/NOT RUN; original client/runtime, GUI/physical input, audio decoding/sample resolution, health calibration and owner qualification NOT VERIFIED. Late timing-offset investigation NOT RUN. Historical full Windows application gate not rerun: no production behavior change; focused diagnostic gate requested by Issue#5 |
| Coverage gained / Remaining | Executable audit and classification/coverage resource across158 historical synthetic fixtures+mixed16; no new beatmap compatibility rule/admission. Work1 independently verifies tool, owner supplies local results, Headquarters selects one supported correction. Broad C001 and Issue#4 runtime qualification remain OPEN |
| Discrepancies / Limits / Blockers | No functional blocker. Production exception taxonomy is conservative message allowlist with unknown rejections explicit. Existing health-overlap admission can reject after canonical success. Finite limits/partial scans/precision/runtime limits are disclosed, no fitting or compatibility fix |
| Updated records / Reproduction | CorpusAudit/ source,README,initial09 mark,evidence and coverage/dependencies; CorpusAuditChecks/; new workflow; append-only reconstructed/contexts.md and campaign status/coverage/discrepancy/consumer inventory. Commands below |
| Next bounded task / Actor / Dependencies | Work1 independently audits source linkage/taxonomy/privacy/bounds and actual fixture outcomes. Owner runs locally and shares authorized summary; Headquarters selects highest-impact supported gap from actual results. No automatic timing or broad compatibility work |
| Subsystem | **OPEN**; checkpoint is not complete slider compatibility, original-runtime equivalence or owner acceptance |

Owner procedure: [CorpusAudit README](../../CorpusAudit/README.md).

```sh
dotnet run --project reconstructed/CorpusAudit/CorpusAudit.csproj -c Release -- "C:/osu/Songs" --output "C:/ohno-audits/audit.json"
# Or use the downloaded package with .NET8 runtime:
dotnet CorpusAudit.dll "C:/osu/Songs" --output "C:/ohno-audits/audit.json"
# Reproduce focused tests from the repository root:
dotnet build reconstructed/CorpusAuditChecks/CorpusAuditChecks.csproj -c Release
dotnet run --project reconstructed/CorpusAuditChecks/CorpusAuditChecks.csproj -c Release --no-build -- . /tmp/ohno-audit-results
```

Use your own source/output locations. Output must be outside the scanned tree.
Default JSON contains no filenames, titles, chart contents, absolute paths or raw
exception messages. Optional relative paths are for private owner-local use only.
No owner's chart/media was copied, uploaded or included in repository/CI evidence.

| Package / Actual downloaded ZIP | Artifact | SHA256 |
|---|---|---|
| Windows .NET8 tool + synthetic reports;309007bytes | [14120870195](https://github.com/thgillwtnorizoh/ohno/actions/runs/43611606831/artifacts/14120870195) | ceacd685f3bc3696922c1ed40dd67ff7130d556d2ee6fb71f3ac7ac5e9e14189 |
| Linux .NET8 tool + synthetic reports;271913bytes | [14120093328](https://github.com/thgillwtnorizoh/ohno/actions/runs/43611606831/artifacts/14120093328) | f88b031058024ccd9fd69e8f68076a846a4ae88ef76e0f78455db3dd9f859b94 |

The first focused run43611466103 at75bac849 failed both SDK-selection steps before
compilation because a regex lost its backslashes during command construction.
Corrected only the new workflow to explicit character classes; no test/source
expectations were weakened. That failed run, its source, job identifiers and log
hashes remain in ci-seal.json and append-only journal. Successful CI is8c5ba835.
No exception or waiver to preservation/production regression rules was requested.

# 2. What they found

VERIFIED COMPONENT, synthetic fixture results (not real-world compatibility rates):

| Unchanged corpus | Files | Canonical success | Malformed | Unsupported engineering admission |
|---|---:|---:|---:|---:|
| Work2 C001 |24|21|0|3|
| Work1 finite corpus |49|39|3|7|
| P |21|17|1|3|
| C |19|13|1|5|
| Stacking |18|18|0|0|
| QA15 |27|27|0|0|
| Total |158|135|5|18|

These corpus runs preserve all historical source bytes and expected files. New
mixed16 gives10 canonical successes/3 malformed/3 unsupported, with independent
index/read/consumer/audio counts. Additional controls prove changed/disappearing/
sharing-blocked files and unexpected exceptions are isolated; traversal/bounds/
reparse/cancellation/duplicates/CLI/output containment and privacy are exercised.
All six roots are scanned twice with identical JSON within each build/platform.
One stacking canonical success separately hits an existing health-overlap guard;
this execution reports that consumer boundary without a gameplay change.

The runner invokes LocalChartCatalog.Index per enumerated file and fingerprint-
checked CatalogChart.LoadCanonicalGameplay, including when index admission differs.
Source features come from production document/stage/type-priority projection;
catalog category counts remain separately labeled. Production sources are compiled
through exact source linkage, consistent with retained QA contracts. Assembly
provenance includes build commit/state and actual compiled file hashes. Duplicate
occurrences are all audited because folder resources can differ. Primary cohort
success denominators exclude other modes/spinner-bearing/unknown profiles rather
than discarding them. Spinner support remains accurately represented.

Limits are ENGINEERING, inherited or tighter:10000charts/4096directories/50000entries/
depth32, bounded per-directory enumeration;4MiB index and2MiB gameplay guards plus
existing geometry/object/checkpoint/health limits. Oversized directories are excluded
whole, avoiding nondeterministic truncated prefixes. Missing resources are distinct
from preparation; media is never decoded. Reports identify any partial traversal;
no hostile atomic-filesystem snapshot or fatal-runtime recovery is claimed.

The actual owner collection, runtime experience and offset observation remain
unmeasured. R17 mathematics/19 owner differences and Joined Bézier are unchanged.
This tool creates evidence for the next correction; it does not choose a gap from
unseen owner data or close C001 acceptance gates.

# 3. Pseudocode

ENGINEERING, VERIFIED COMPONENT through production entry points:

```text
validate local root, outside-tree output and finite limits
traverse bounded ordinal-sorted entries; disclose reparse/access/limit exclusions
for each .osu occurrence:
    capture production fingerprint and exact-byte hash
    derive diagnostic features using production document/stage/type contracts
    index using actual LocalChartCatalog.Index
    verify captured/indexed revision; call LoadCanonicalGameplay
    record independent canonical, structural, score/session/health-source and audio outcomes
    isolate safe normalized failure categories; retain unknowns
count cohorts, stage denominators and duplicate captured hashes
emit private-by-default deterministic JSON and compact summary; disclose partial scans
```
