# 1. Result

| UEP-001 rev2.1 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | ohno / C001 / Work1 independent tool verification / Issue#5 |
| Status | FAILED — reproducible diagnostic classification discrepancy; bounded QA stops for Work2 correction |
| Starting main | 55cb969e3e300b5905867235ed34af9d8197e824, latest remote verified |
| Exact tested source | 8c5ba835f260401a56dea27367c6a6e9c55e9d84, detached clean checkout; tool/gameplay code identical to starting main |
| Published SHA | Introducing commit in Git history and issue handoff |
| Objective / Profile / Changes | Independently test read-only canonical audit diagnostic correctness; three self-authored synthetic charts, no production edits |
| Evidence / Confidence | VERIFIED COMPONENT / independently reproduced actual CLI misclassification; ENGINEERING taxonomy contract. No original-client equivalence assertion |
| Tests / Environment | Debian13/Linux SDK8.0.427/runtime8.0.33;10 targeted assertions,9PASS/1FAIL. Two default runs byte-identical; one private-path run; source byte/hash preservation and clean exact provenance PASS |
| CI / NOT RUN | Existing focused run43611606831 exact8c5ba835 SUCCESS corroborated via GitHub metadata; prior239Windows/239Linux are reused development evidence, not new independent passes. No new Windows gate or full historical rerun. Owner collection NOT PROVIDED/NOT RUN; original client/GUI/audio/physical input/health calibration NOT RUN |
| Coverage gained / Remaining | Direct production linkage inspected, finite stage/hash/privacy/private-path/determinism/read-only checks executed. Broader modes/resources/duplicates/cancellation/mutation/symlink limits are NOT independently re-executed in this bounded cycle; acceptance suspended at discrepancy |
| Discrepancy / Limitations | Stable malformed numeric value containing a known error phrase is incorrectly labeled changed-input. Synthetic finite checks do not establish hostile filesystem safety or real-world compatibility |
| Updated records / Reproduction | corpus-audit-qa/{probe.py,fixtures,results.json}; this report and append-only status/coverage/dependency/discrepancy/journal deltas. Commands below |
| Next bounded task / Actor / Dependencies | Work2 correct mutation classification provenance and add this discriminating case; Work1 rechecks correction and remaining scope. Headquarters must not close Issue#5 yet |
| Subsystem | OPEN; Issue#4 owner runtime remains separate |

# 2. What they found

## Actual production linkage

CorpusAudit.csproj explicitly compiles the current Gameplay/Beatmap, CircleSession,
CircleHitSounds, RankResults and Selection/LocalChartCatalog source. AuditFile calls
LocalChartCatalog.Index, then CatalogChart.LoadCanonicalGameplay even when catalog
admission differs. Feature projection uses production OsuDocument stage readers and
classification helpers; it is diagnostic projection, not a replacement gameplay parser.
No legacy Beatmap.Load(string) call was found in this tool's preparation path.
The tested binary reports the exact clean8c5ba835 commit and linked file hashes.
Starting main changes only documentation/evidence relative to that tool source.

## Minimal mismatch

Use the valid synthetic control chart and change just its difficulty value:

```text
[Difficulty]
CircleSize:changed since indexing
```

The input has an invalid numeric property. It is never edited while scanning.
Exact-byte read and indexing succeed. Actual canonical preparation rejects its
metadata with InvalidDataException. Correct diagnostic family is malformed input;
actual report is:

```json
{"status":"changed-input","code":"fingerprint-changed","exceptionType":"InvalidDataException"}
```

The sibling `CircleSize:not-a-number` produces malformed-input as expected. Both
are stable, hashed files with the same shape. Default scans repeat byte-for-byte;
all three file hashes before/after match their reported captured SHA256 values.
This is not an intermittent mutation or a catalog fingerprint race.

Root cause: AuditRunner.Failure lowercases arbitrary e.Message and tests
`m.Contains("changed since indexing")` before its exception-type/category branches.
The production metadata error retains the offending numeric string, so chart-
supplied text can impersonate the diagnostic's mutation signature. Actual
ChartFingerprint/read/index/prepared hash checks did not identify a changed file.
This can mislead failure-family prioritization and mutation statistics; it does
not turn rejection into canonical success and does not demonstrate an input write.

Work2 should classify observed fingerprint transitions through explicit stage/
typed provenance, or narrowly recognize only the exact trusted mutation exception
shape. Do not let arbitrary nested metadata or other input-derived exception text
select a changed-input outcome. Preserve genuine mutation detection and test it
separately. Do not weaken the malformed-input expectation to match current output.
No production fix was made by Work1.

## Focused results and limits

The independent probe makes10 assertions: clean exact-source identity, valid control,
ordinary numeric rejection, phrase-bearing numeric rejection, unchanged input bytes,
reported SHA equality, default canary privacy, deterministic default JSON, opt-in
relative paths, and finite completion/stage denominators. Only phrase-bearing numeric
classification fails. Default reports do not contain the synthetic chart filenames,
absolute temporary root, title, bad values or raw error text; opt-in private reports
include only the expected relative chart paths. These are sampled privacy checks,
not a certificate for all possible exception/resource paths.

The prior158chart synthetic result and239-per-platform development suites remain
historical evidence, not new independent executions or owner collection statistics.
This checkpoint stops at a minimal actionable discrepancy, as assigned. Further
independent challenge of unknown objects/cohorts/duplicates/resource boundaries,
concurrent mutation/cancellation and symlink behavior remains pending after correction.
No late offset, gameplay rule, R17 mathematics or original executable investigation.

# 3. Reproduction

```sh
# Build exact8c5ba835 detached checkout:
dotnet build reconstructed/CorpusAudit/CorpusAudit.csproj -c Release
# Run the new independent probe from the publication checkout:
python re/campaigns/C001/corpus-audit-qa/probe.py /path/dotnet /exact-checkout/reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll /tmp/audit-qa.json
```

Expected current probe exit1:9/10PASS,1taxonomy mismatch. The helper copies the three
synthetic fixtures into an isolated temporary directory, checks all source hashes,
runs the real CLI twice by default and once with private relative paths, and writes
a safe structured evidence packet. No external assets, real beatmaps or music.

```text
invalid chart number contains trusted-looking phrase
→ actual canonical metadata exception retains offending value
→ message substring wins classification before type/stage provenance
→ false changed-input diagnostic on unchanged bytes
```
