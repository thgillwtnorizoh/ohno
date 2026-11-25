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


## 2026-11-25 — Issue24 Windows86 evidence recovery; artifact inspection BLOCKED

Work2 / C001 / Issue24 evidence-only recovery under HQ7158535949 and UEP-001
Rev2.1/COMMS-01. Starting main/exact CI-tested source
`346638cec4548023dbb480710327cd8efa31ce69`; functional production
`f76c26a05ae88587bb44440fbe078ac75b59fb41`. Remote comparison confirms the intervening
commit changes workflow/evidence only; no production or focused-harness edit.
Published documentation SHA is pinned by the final Issue24 recovery comment.

Existing Windows run#86/45446210102/job138630568056 SUCCESS,56/56 steps;
Windows Server2022 10.0.20348, image20261116.370.1, SDK8.0.428/runtime8.0.34.
Release build0 compiler warnings/0 errors; self-contained win-x64 publish succeeds
in the log. Timing550/550,12 exact controls/2 conditional domains/55 recorded
requests; historical QA2312/2312; retained C001264/P299/C4375/stack43+394/
Linear5430/WPF564/traversal3974/near-spinner1503/legacy801/versioned656/Audit295
and finite106/106+4/4 PASS. Full distinct counts, commands, metadata, decoded job
log and confidence boundaries are retained in issue24-development/windows86-recovery/.
Two upload-tool Node deprecation notices and one checkout/upload Node20 notice
are disclosed separately from compiler results.

Artifact14945414784: metadata reports68704166bytes, unexpired, digest
`3d75daac31252feaaba931eae37fea185e7b1d25da57b9ff2c2b1589824ecbdb`;
the upload log independently records the same digest. The connected download
returns a ZIP file reference, but local byte retrieval fails with
`Invoke-WebRequest: No such host is known.` The alternative managed JS runtime
fails before execution: `Managed execution requires a usable CODEX_CLI_PATH`.
Local Git access also fails `getaddrinfo() thread failed to start`; the stale
local checkout is not the tested source and was not used as substitute evidence.
**BLOCKED: artifact bytes/digest calculation, ZIP contents, bundled source seals,
executable/PDB/runtime bundle and application delivery are NOT VERIFIED.**
Existing verify_delivery.py was read but NOT RUN; its full-source/input/evidence
seal audit remains required on an exact byte-preserving source checkout.
Reported upload/publish success is not claimed as inspected application delivery.

No implementation, workflow, original research, fixture, oracle or historical
test change; no regression/build/CI rerun/corpus scan/application launch.
Earlier pending statements, failed85(536/550,14 fixture-hash failures) and all
historical evidence remain intact. All edits to existing records in this recovery
are append-only. This checkpoint preserves verified CI findings without declaring
final development delivery. No authorized exception was needed.

Next bounded actor: Work2, using a host able to retrieve the existing ZIP, finishes
artifact digest/content/source/application inspection and verify_delivery.py;
then HQ reviews final development delivery and authorizes independent Work1 QA.
Original17 E20/runtime/x86/GUI/audible/physical/private-owner qualification NOT RUN.
C001-I23-ORDER-01 remains development-corrected with independent QA pending;
equal-key sort stability and excluded profiles remain NOT VERIFIED.
**Issue24/C001/subsystem/original-runtime Issue4 OPEN; no owner acceptance.**
