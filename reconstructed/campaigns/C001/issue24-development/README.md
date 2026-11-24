# Issue24 — initialized timing development evidence

Work2 functional development checkpoint; independent Work1 QA pending.
[Report](../19-issue24-timing-initialization.md). Starting main22fedbf8,
prior functional5dda483; exact publication/testing SHA is pinned by Issue24 comments.

New550/550 development checks pass, with55 actual recording-sink sample requests.
The unchanged Issue23 oracle supplies12 exact comparator-order controls and2
ambiguous equal-key domains. No original sort-stability/runtime claim. Issue22's
47 source fixtures, manifests, original expectations and historical test code are
unchanged. All code under re/ is read-only in this implementation.

Reproduce using .NET8 SDK and PowerShell from a fresh checkout:

```sh
dotnet run --project reconstructed/C001TimingInitializationChecks -c Release -- \
  "$PWD" /tmp/issue24-focused
dotnet run --project reconstructed/C001DevelopmentChecks -c Release -- "$PWD"
pwsh -NoProfile -File re/qa/qa10/Run-QA10.ps1 -ResultPath /tmp/issue24-qa10.json
pwsh -NoProfile -File re/qa/qa11/Run-QA11.ps1 -SkipWindows -ResultPath /tmp/issue24-qa11.json
python3 reconstructed/campaigns/C001/issue24-development/verify.py
```

Source/input identities and environment are in provenance.json/environment.json;
new assertions and requests in local/results.json. Retained commands identify all
eight C001 suites and audio results. Large logs/JSON are losslessly .gz.b64-packed,
with original byte hashes and paths in packed-evidence.json. Decode base64 then gzip.
The Windows workflow runs the new suite, full retained regressions and existing
Release/self-contained win-x64 publish; its new test results join the artifact.
The functional checkpoint intentionally records Windows as pending. Final Windows
evidence will be appended after the one exact-source gate, or an exact blocker
published; no substitution with old run84.

Raw source disorder remains an admission input despite sorted preparation. Legacy
Load and standalone supplied-list selectors are unchanged; metadata/raw objects
are retained before timing-dependent prepared object/sample construction. Equal-key
cases have only conditional domain assertions. Native original execution, shown
GUI, audible speakers and physical input/owner qualification are NOT RUN.


## 2026-11-24 — Windows85 exact-byte checkout blocker and CI-only correction

Run85/45444179661,job138623260456, exact functionalf76c26a05ae88587bb44440fbe078ac75b59fb41
completed FAILURE:52 steps succeeded, new timing suite536/550PASS/14FAIL, self-contained
publish SKIPPED. All14 failures are immutable fixture SHA checks; all536 other
assertions and55 real sample requests pass. Partial evidence artifact14943674638
downloaded and SHA2566f7b86ef35a61f19f91595a77451897110c65cc0eebbc245bc8c5f0dcd890a9c
verified. It contains NO delivered application. Full failed job/log/results preserved
in issue24-development/windows85-failure/.

A separate git -c core.autocrlf=true worktree of the exact source reproduces the
identical536PASS/14FAIL IDs, with LF fixtures converted to CRLF; normalizing that
copy recovers original fixture hashes. Original primary/remote Work1 files and
expectations unchanged. CI correction sets core.autocrlf=false BEFORE checkout.
No production/harness/oracle changes. A corrected exact-source Windows gate is
necessary; failed85 is not relabeled success and old84 is not substituted.
Initial verify.py intentionally pins the first workflow; verify_delivery.py checks
the CI-only descendant plus unchanged production/Work1/initial evidence.
C001/subsystem/Issue4 OPEN, independent Work1 QA and owner acceptance pending.
