# 1. Result

| UEP-001 Revision 2.1 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work2 Reconstruction Engineering / Issue #9 |
| Status | **BLOCKED** — reproducible admission-gate contradiction; candidate retained, verified main production unchanged |
| Starting SHA | f9cce9e085735668f86509b89845e0001929186d, live main verified before work/publication |
| Existing main production | 2a2e29177d5f1173c711cf3d12d70be2f1759e09; no production change in this checkpoint |
| Exact tested candidate SHA / Tree | 2c212abd963cdc5473d72d06bfc4207f74b56ca7 / b33555ac6aa661cd526f7a658fc61084472190b9, clean detached candidate worktree; recoverable exact Git commit in candidate.bundle.b64 |
| Published SHA | Evidence-only introducing commit of this report, not a production integration commit; candidate patch/bundle, tests, result hashes, failure records and journal persisted |
| Objective / Profile | Route M existing v6–v13 CS5/AR5 modern pass; Route Z original-derived no-match proof for v5/other finite CS/AR; explicit-sample six/seven-column timing; preserve breaks/order/overlap/sample/resource restrictions |
| Functional candidate changes | Catalog scalar-only exclusion removed; canonical break guard retained separately; six-column timing floor with existing base/effects defaults; geometry-qualified Route Z with strict original distance primitive, actual spinner centers, geometric AND traversal ends, cancellation and engineering1,000,000 pair limit; existing modern stack pass untouched |
| Evidence / Confidence | CONFIRMED ORIGINAL static authority11/handoff, preserved55 sites/13 bodies/10 identities; RECONSTRUCTED hand-derived manifest positions/flags; VERIFIED COMPONENT candidate development; ENGINEERING conservative certificate/work bound/uncapped endpoint. No independent new-source QA or runtime equivalence |
| Tests / Environment | Debian13 Linux x64, SDK8.0.427/runtime8.0.33. New separately versioned development801/801 PASS,81 fixtures:66 prepared/15 excluded individually against immutable targets. Actual catalog/direct canonical, settings, prepared heads, score, calibrated health, NewCombo/session/sample identity and audio admission checked. Repeated on exact candidate; fixture81+prior33 regeneration and55-site/prior548 seal checks PASS |
| Failure / Blocker | Unchanged QA14 C/spinner-old:0PASS/1FAIL,exit1, expected Unsupported, actual Indexed. Preserved Issue7 development also terminates at Unexpected admission: finite legacy exclusion stack. These expectations require the admission boundaries this assignment explicitly expands |
| CI / Artifacts / NOT RUN | Full exact-source Windows2309 regression, application Release/package and additional integrated C001 QA NOT RUN after focused blocker; no CI/artifact/app pass claimed. Exact source bundle, patch, compiled development DLL hash and logs sealed locally in repository evidence. Original/GUI/audible playback/physical input/private corpus/host cap NOT RUN |
| Coverage gained / Remaining | Candidate demonstrates synthetic66/81 and connected preparation; **zero newly published production coverage**. Detailed input/result/audio backend execution, boundary/work-limit tests, Windows build/full regression and independent Work1 QA remain before integration acceptance |
| Limits / Discrepancies | Current required Indexed candidacy and historical Unsupported expectation cannot both hold for same metadata-valid v13 row. No chart-name/test/environment-specific workaround, expectation edit or test filtering in production workflow.15 canonical exclusions retained; no owner recovery forecast |
| Updated records | This report; legacy-spinner-development/{candidate.patch,candidate.bundle.b64,candidate-commit.txt,seal.json,exact-results.json,exact-qa14.json,logs,README.md}; appended reconstructed status/coverage/discrepancy/consumer/journal entries; no re/ edits |
| Reproduction | Recover bundle from verified base, checkout exact candidate, run new development and unchanged QA14 filter per README; full commands below |
| Next bounded task / Actor / Dependencies | Headquarters resolves versioned acceptance disposition for QA14 old catalog exclusion and Issue7 blanket stack exclusions while preserving historical files/results. Work2 then resumes complete consumer/boundary tests and exact Windows gate; Work1 independently verifies NEW production afterward. No owner manual action requested |
| Subsystem | OPEN; Issue #9/C001 OPEN; Issue #4 runtime remains separate |

## Reproduction

From repository root, with dotnet8 on PATH:

```sh
base64 --decode reconstructed/campaigns/C001/legacy-spinner-development/candidate.bundle.b64 > /tmp/ohno-i9.bundle
git bundle verify /tmp/ohno-i9.bundle
git fetch /tmp/ohno-i9.bundle HEAD
git worktree add --detach /tmp/ohno-i9-candidate 2c212abd963cdc5473d72d06bfc4207f74b56ca7
dotnet run --project /tmp/ohno-i9-candidate/reconstructed/C001LegacySpinnerChecks -c Release -- /tmp/ohno-i9-candidate /tmp/i9-development
dotnet run --project /tmp/ohno-i9-candidate/re/qa/qa14/Headless/QA14.Headless.csproj -c Release -- /tmp/ohno-i9-candidate /tmp/i9-qa14.json C/spinner-old
```

Expected new development801/801 PASS,66admitted/15excluded; unchanged QA14 control
0pass/1fail,exit1. QA14's root argument must contain source directories; the first
isolated-root attempt reproduced the assertion then failed collecting source hashes.
Corrected exact-source execution completed the failure packet normally. QA14 writes
only its generated fixture inputs in that disposable worktree and temporary scratch.
The filter isolates the contradiction; it is NOT a substituted full historical pass.
Actual invocation used `/workspace/scratch/d01df0260ba5/dotnet/dotnet` (8.0.427).

```sh
python re/campaigns/C001/legacy-spinner-profile/build_fixtures.py --check
python re/campaigns/C001/versioned-spinners/build_fixtures.py --check
python re/campaigns/C001/legacy-spinner-profile/check.py
# Additional old-profile assertion conflicts with new certificate admission:
dotnet run --project /tmp/ohno-i9-candidate/reconstructed/C001VersionedSpinnerChecks -c Release -- /tmp/ohno-i9-candidate /tmp/i9-old-development
```

Initial new harness build failed CS0103 due three stale audit-report references when
adapting the separate development harness from Work1 characterization; corrected
without altering original tests/oracles. All attempts retained. Work1 old775 tests
remain old-source characterization and were not relabeled as new verification.
New801 assertions consume independently authored manifest coordinates/flags and
per-case handoff targets, not generated production expectations. Prepared-head
comparisons use the original harness tolerance0.00004; full bitwise geometry and
input/results/backend verification remain pending, not claimed complete.

# 2. What they found

HQ authorization comment6905874085 explicitly removes the old scalar-only index
block and requires metadata-valid old spinner candidates to reach canonical
qualification. Preserved QA14 Headless/CatalogIdentity.cs:18–19 instead requires
Unsupported for Text(rows:"1,2,1000,8,0,1500",format:13). Its helper provides valid
Mode0,CS5,AR5,full explicit timing and omitted StackLeniency=.7. The actual candidate
now indexes it as required. Retaining a different special rejection for this chart
would contradict metadata candidacy; changing the fixture or hiding the failure
would contradict historical preservation. This is an acceptance-contract dependency,
not missing original spinner evidence or a gameplay failure observed at runtime.

Issue7's preserved C001VersionedSpinnerChecks/Program.cs:98–99 similarly requires
isolated v5 spinners with explicit/implicit.7 to reject. Route Z must admit exactly
that no-displacement configuration. Its unchanged runner aborts at the first such
assertion, before writing results; do not call that653passes or assign a final count.
This checkpoint does not change either checker or the production workflow.

Candidate geometry is qualified only after actual slider preparation. The certificate
compares distinct chronological pairs, raw/constructor heads including spinner256/192,
and both adjusted geometric last-segment To and final-traversal EndPosition. It calls
the existing Single difference/product/sum→Double sqrt→Single strict<3 primitive.
It rejects inconclusive proximity and more than1,000,000pairs; it never invokes
modern stacking for v5 or changes leniency to zero. Route M calls the existing pass
unchanged, including its position-remapping rules that never translate spinners.
Six/seven timing uses the existing parser's base=true/effects0 omissions, retains
sample/source order and existing sample validity checks. Two-to-five remains excluded.
Source-record spinner policy and uncapped host-length classification are unchanged.

Candidate tests exercise all81 original bytes and verify their66/15 outcomes
individually. Source/DLL/result/patch hashes are retained in seal.json. The thin Git
bundle includes exact source and new test code, requires only startingf9cce9e and
was checked with git bundle verify. Main's accepted gameplay/QA/research/workflow
bytes remain unchanged. No original owner chart, deferred offset, audio compensation,
slider algorithm or R17 mathematics is changed or tested by inference.

# 3. Pseudocode

```text
candidate index: admit safe old spinner metadata; preserve raw/default settings
canonical: preserve timing/sample/break/order/interval bounds; prepare real paths
if positive leniency and modern CS5/AR5: call unchanged modern stack pass
else if positive leniency:
    bound pair work; compare distinct heads and BOTH path endpoints
    reject if original strict distance <3 leaves no-displacement proof inconclusive
reuse shared ownership/score/health/NewCombo/session/audio admission
stop publication of production when preserved gate contradicts required admission
retain exact candidate and failure; return acceptance dependency to Headquarters
```
