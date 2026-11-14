# 1. Result

| UEP-001 revision 2.1 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work 2 / Issue #5 read-only real-beatmap corpus audit |
| Status | Functional local focused checkpoint; exact-source focused Release CI pending at initial publication; final seal appended below |
| Starting main SHA | 3a26667bb80e085b17e0bfb6754176bbadd4d180, verified against GitHub main |
| Published / Exact tested source | Introducing Git commit; initial local source is modified working tree on starting SHA, compiled source hashes retained; final exact CI revision/result sealed below |
| Objective / Supported profile | Bounded deterministic local .osu tree; actual catalog indexing and canonical preparation, primary standard/no-spinner source cohort and separate other profiles |
| Changes / Integrations | New CorpusAudit linked to unchanged actual Beatmap/catalog/session/score/health sources, CorpusAuditChecks, focused Linux/Windows workflow and owner instructions; no production gameplay changes |
| Evidence / Confidence | VERIFIED COMPONENT development evidence; ENGINEERING scanner/taxonomy/safety bounds. Headless results are not original-runtime or owner-playability evidence |
| Local tests / Environment | Linux SDK8.0.427/runtime8.0.33; focused239/239 PASS, six historical roots158 charts plus mixed16; zero platform controls skipped. Release0warnings/0errors; deterministic JSON and source-preservation controls PASS |
| CI / Artifacts / NOT RUN | Exact-source focused Release CI pending initial publication; no historical full Windows regression because production unchanged. Owner collection, original client, GUI/input/audio decoding, health calibration and timing-offset investigation NOT RUN |
| Coverage gained | Reproducible hashes/features and independently visible read/index/canonical/consumer/audio outcomes; duplicate/resource/change/cancel/bound/error controls; runnable owner-local tool |
| Remaining requirements | Work1 independent tool QA; owner runs actual collection; Headquarters selects one supported correction. Issue#4 owner-only runtime qualification remains separate; broader C001/19 R17 differences/Joined Bézier remain OPEN |
| Discrepancies / Limitations | Conservative message-code allowlist, known finite admissions and full limits documented in README; actual canonical preparation != full consumer or runtime readiness; health overlap can reject after canonical success |
| Updated records / Reproduction | CorpusAudit/, CorpusAuditChecks/, .github/workflows/corpus-audit.yml, append-only reconstructed/contexts.md; campaign status/coverage/dependency/discrepancy final delta appended after CI |
| Next actor / Dependencies | Work1 independently verifies runner; owner runs locally and shares authorized summary; Headquarters prioritizes one supported gap from actual results |
| Subsystem | OPEN; checkpoint is not campaign completion or owner acceptance |

Build/run and privacy-safe owner procedure: [README.md](README.md).
Focused reproduction:

```sh
dotnet build reconstructed/CorpusAuditChecks/CorpusAuditChecks.csproj -c Release
dotnet run --project reconstructed/CorpusAuditChecks/CorpusAuditChecks.csproj -c Release --no-build -- . /tmp/ohno-audit-results
dotnet run --project reconstructed/CorpusAudit/CorpusAudit.csproj -c Release -- /local/Songs --output /outside-tree/audit.json
```

No exception to historical preservation or production regression requirements.
Issue #5 explicitly requests a focused diagnostic-only gate when production behavior
is unchanged. The old application/full-history gate is preserved byte-for-byte.

# 2. What they found

VERIFIED COMPONENT: six unchanged fixture roots158 files give135 canonical successes:

| Root | Files | Canonical success | Malformed | Unsupported finite admission |
|---|---:|---:|---:|---:|
| Work2 C001 |24|21|0|3|
| Work1 finite corpus |49|39|3|7|
| P research |21|17|1|3|
| C research |19|13|1|5|
| Stacking |18|18|0|0|
| QA15 |27|27|0|0|

These are preparation outcomes on synthetic fixtures, not a real-world compatibility
rate. C minimal-control classification is an ENGINEERING admission boundary;
unknown original-runtime errors are not inferred from this taxonomy. All existing
fixtures and expectations remain unchanged. The stacking corpus additionally
shows one canonical success whose existing health overlap admission rejects later;
this is reported separately, with no speculative correction.

The mixed16 includes duplicate content in different locations, L/B/P/C, other mode,
spinner, malformed header/hitSound/number, unsupported marker/hold, ignored files,
nesting and present/missing audio. Separate controls cover changed/missing files,
actual sharing-blocked reads, unexpected exception isolation, opt-in paths,
unknown rejection taxonomy, preexisting finite size guards, directory/chart/depth/
entry limits, symlink file/directory exclusion, cancellation and real CLI outputs/
exit codes. Input bytes and modification stamps are unchanged by the ordinary scan;
mutation hooks deliberately alter only scratch synthetic controls and restore bytes.

Production entry points are compiled by source linkage, matching established QA
practice; assembly provenance includes exact compiled source hashes and build Git
commit/state. The deterministic wrapper uses LocalChartCatalog.Index per file,
not the UI refresh's enumeration/truncation path. Canonical preparation is always
CatalogChart.LoadCanonicalGameplay, never legacy Beatmap.Load(string). Runtime
source features use production OsuDocument, ReadStage, metadata normalization and
OriginalObjectClassification; their descriptive projection does not admit charts.

ENGINEERING limitations: same build/platform/layout/resources are required for
byte-identical reports. Dynamic cancellation, denied permissions and changing files
can change results. Scans are bounded and best-effort snapshots; no hostile atomic
filesystem guarantee. Audio presence is not decoding; custom sample resolution,
health calibration, rendering, input and original behavior remain NOT VERIFIED.
Fatal runtime resource failures are not disguised as completed scans. No user paths,
raw messages or metadata/content are published by default; optional relative paths
are owner-local only. Actual owner collection statistics remain NOT RUN.

# 3. Pseudocode

ENGINEERING, verified through actual production components:

```text
validate local root and finite catalog bounds
traverse sorted bounded directory entries; exclude reparse paths; disclose truncation
for each .osu occurrence:
    capture exact bytes/hash through production ChartFingerprint
    project reliable features using production document/stage/type contracts
    index with LocalChartCatalog.Index
    compare fingerprints; call CatalogChart.LoadCanonicalGameplay
    independently record structure, automatic score, fresh session/health-source checks
    record audio presence without decoding; isolate normalized failure outcomes
group source cohorts; count duplicate captured hashes without skipping occurrences
emit deterministic path-private JSON and summary; disclose partial scans
```
