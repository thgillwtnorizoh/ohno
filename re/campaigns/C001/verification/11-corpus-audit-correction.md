# 1. Result

| UEP-001 revision 2.1 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work 1 independent verification / Issue #5 corrected CorpusAudit |
| Status | **VERIFIED CHECKPOINT** for bounded finite audit scope; Headquarters acceptance pending |
| Starting SHA | 41743d4a5ee3de11f96baa52730761a9d7411068; GitHub main checked before work and publication |
| Exact tested SHA | 2369b406b09ec34cf07f33f8278a155afc6a4ebf; detached clean checkout, Release actual linked production |
| Published SHA | This report's introducing commit, recorded in the Issue #5 publication comment; evidence-only descendant of starting main |
| Objective / Supported profile / Changes | Independently verify diagnostic correction and remaining bounded read-only audit scope. Versioned independent harness only; production and historical evidence unchanged |
| Evidence / Confidence | INDEPENDENTLY VERIFIED finite actual-production results; ENGINEERING diagnostic/guard contracts. No new original-executable claim |
| Tests / Environment | Linux Debian 13, .NET SDK 8.0.427/runtime 8.0.33; clean Release build 0 warnings/errors; corrected profile **10/10 PASS** and independent scan checks **24/24 PASS**. Initial new scan harness 22/23 retained and explained below |
| CI / Artifacts | Reused exact-source run 43618348530, Linux job 132518255285 and Windows job 132518255539 SUCCESS independently corroborated through GitHub metadata. Existing Work 2 259/259 per platform and synthetic 158-chart results remain development evidence, not newly executed independent counts. Artifacts/seals in reconstructed/CorpusAudit/correction-01/ |
| Coverage gained / Remaining | Corrected-source identity gate resolved; immutable malformed phrase, real byte/stamp transitions, stages, cohorts, duplicates, counts, privacy, determinism, limits, cancellation, blocked reads and isolation independently checked. Owner corpus statistics and runtime qualification remain absent |
| Discrepancies / Limitations / Blockers | No new actionable production discrepancy reproduced. Unknown rejection taxonomy remains heuristic/conservative; finite filesystem observations are not atomic hostile-path guarantees. Sharing-blocked reads tested, OS-ACL denial not independently executed. Linux symlink evidence does not prove Windows reparse equivalence |
| Updated records / Reproduction | corpus-audit-qa-v2/ including versioned verifier, fixed hashes, results and logs; this report; append-only campaign status/coverage/dependency/discrepancy registers and re/contexts.md. Commands below |
| NOT RUN | Full development suite or historical gameplay/Windows regression rerun (diagnostic-only scope; reused CI). Owner collection NOT PROVIDED/NOT RUN. Original client, GUI, audio decoding/playback, physical input, complete health validation, owner qualification, deferred timing investigation NOT RUN |
| Next bounded task / Actor / Dependencies | Headquarters reviews independent packet for Issue #5 closure/deployment; owner subsequently scans authorized local collection. Select compatibility work only from actual results; Issue #4 remains separate |
| Subsystem | **OPEN**. Issue #5 remains OPEN until Headquarters acceptance; C001 OPEN |

# 2. What they found

## Independent basis and provenance

Work 1 authored the new challenge corpus and test assertions and reviewed actual
AuditRunner, catalog and canonical preparation source. No Work 2 implementation
was copied as a reference oracle. The versioned profile reuses the original three
immutable fixtures and ten behavioral questions. The original probe, results,
reports and fixture bytes remain unchanged, including the original defect and the
historical corrected-binary 9/10 obsolete-pin result.

Version 2 requires exactly corrected SHA 2369b406, clean build metadata, and all
eight pinned source-file hashes (tool/project plus five actual gameplay/catalog
files). Hashes were independently calculated from that commit's Git blobs.
verify.py also checks a completely clean exact checkout before/after the build,
checks local source bytes, builds Release, runs both probes, and verifies the
in-process checks linked the identical CorpusAudit binary. provenance.json seals
binary, verifier and result hashes. A different truthful commit fails this profile;
there is no arbitrary-SHA option or waived provenance assertion.

| Source/profile | Result retained |
|---|---|
| Defective 8c5ba835 with historical probe | 9/10; immutable phrase misclassified as mutation |
| Corrected 2369b406 with historical hardcoded old-source probe | Historical 9/10; source pin alone fails; diagnostic passes |
| Corrected 2369b406 with separately versioned profile | Independently executed 10/10; exact clean identity plus eight linked hashes enforced |

Actual production still uses LocalChartCatalog.Index and
CatalogChart.LoadCanonicalGameplay. The tool project explicitly links Beatmap,
CircleSession, CircleHitSounds, RankResults and LocalChartCatalog; no parallel
canonical parser or legacy string-loader admission substitutes for these calls.
Source identity checks include these actual files, not just a version label.

## Diagnostics and stage boundaries

The unchanged spoof fixture now produces malformed-input/metadata-syntax.
CanonicalFailure observes the production ChartFingerprint on failure and emits
changed-input only when the observed revision differs. Metadata rejection with
an unchanged revision is classified from the recorded metadata stage, without
interpreting the offending value. Both an actual byte append and a timestamp-only
change with identical SHA256 bytes produced changed-input in independent checks.
A forged catalog-shaped InvalidDataException on unchanged input stayed
unclassified-rejection. No raw exception text appeared in the checked report.

Remaining Failure/RejectionCode matching still uses allowlisted message fragments
for diagnostic families/codes. This is ENGINEERING taxonomy, not an authenticated
origin for every possible message. Crucially, that fallback cannot emit mutation;
known failed metadata bypasses payload matching. Unknown messages are retained as
unclassified rather than invented precise diagnoses. The tests do not certify
all future exception messages or hostile injected exceptions.

The independently authored mixed scan contains eight chart occurrences. It checks
standard/no-spinner, standard/spinner, alternate-mode and unknown-object cohorts;
spinner admission; ordinary canonical and four-consumer readiness; missing audio
independent of preparation; duplicate occurrence retention; conserved stage/cohort
denominators; and catalog success distinct from canonical rejection.

The first new scan run was 22/23: its equal-time fixture expected
unsupported-admission but actually returned unclassified-rejection /
production-rejection-unclassified. Source says "does not yet support" whereas the
allowlist recognizes "supports/supported/unsupported"; README explicitly documents
unclassified fallback. This is a too-specific **new harness expectation**, not a
hidden production fix or admission failure. scan-initial-results.json preserves
that result. The final assertion requires the exact documented conservative
outcome with successful indexing; it does not accept arbitrary failures. A further
mid-scan cancellation assertion yields final 24/24. No historical assertion changed.

## Bounded safety coverage and limits

Independent execution verifies repeated byte-identical JSON; unchanged normal
source bytes; path/title/artist/media/exception canary privacy; explicit relative
private paths; duplicates scanned rather than skipped; per-file failure isolation;
actual sharing-blocked read; cancellation before scanning and after indexing;
chart/entry/depth caps explicitly incomplete; and an actual Linux symlink excluded
with a reparse notice. The synthetic mutation/hook tests deliberately modify only
temporary test files, not owner input or historical fixtures.

Existing Windows/Linux 259-check evidence covers broader historical L/B/P/C,
stacking fixture roots, file-size/directory limits, private paths and reparse
variants. It is reused as development corroboration, not relabeled independent
execution. Neither the 158-chart synthetic denominator nor this eight-occurrence
challenge is a real-world success statistic. Resource admission, audio-file
presence and structural consumer readiness do not prove playback, physical
playability, full health behavior or original-client equivalence.

No production edits, assets, real beatmaps, extraction, offset work or unrelated
C001 changes. Owner collection and runtime qualification remain separate.

# 3. Reproduction

```sh
# From a publication checkout, create a separate exact clean production worktree:
git worktree add --detach /tmp/ohno-audit-exact 2369b406b09ec34cf07f33f8278a155afc6a4ebf
python re/campaigns/C001/corpus-audit-qa-v2/verify.py /path/to/dotnet /tmp/ohno-audit-exact /tmp/ohno-audit-v2-results
```

Expected exit 0: profile 10/10 and scan checks 24/24. The wrapper invokes Release
build, probe_v2.py and the independent Checks project (ExactRoot points only at
corrected production). It captures build/check logs and writes a provenance seal.
Committed final logs/results record the actual Linux execution. The initial
22/23 packet is an intentionally preserved earlier test-design result; final
source regenerates the final packet. No runtime pseudocode is reconstructed here;
this is an ENGINEERING/INDEPENDENTLY VERIFIED tool checkpoint.
