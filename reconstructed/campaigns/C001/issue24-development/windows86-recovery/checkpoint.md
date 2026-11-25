# Issue24 Windows86 recovery checkpoint — BLOCKED


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


## Evidence and reproduction

- CI metadata: [ci.json](ci.json); original [run86](https://github.com/thgillwtnorizoh/ohno/actions/runs/45446210102).
- Exact suite counts: [counts.md](counts.md), [counts.json](counts.json); [summaries.log](summaries.log).
- Complete connector-decoded job text (including existing embedded evidence): [job.log](job.log). It is decoded Unicode log evidence, not original HTTP response bytes or artifact bytes.
- [commands.yml](commands.yml) is an unchanged snapshot of the tested workflow, including every shell command/argument and publish/copy paths. It is evidence, not an active workflow.
- [source-delta.json](source-delta.json) records the one-commit production/harness-preserving recovery difference.
- Current byte/ZIP/source-seal/application checks: **NOT RUN / NOT VERIFIED**. Do not reuse run84 or failed85's partial application evidence.

On a host with download access, request artifact14945414784 from the linked run,
save the exact ZIP, verify68704166bytes and SHA2563d75daac31252feaaba931eae37fea185e7b1d25da57b9ff2c2b1589824ecbdb,
inspect contained source marker and evidence packets, verify bundled application/
PDB/runtime/fixtures and run unchanged verify_delivery.py from a complete
core.autocrlf=false checkout at346638cec4548023dbb480710327cd8efa31ce69.
The audit uses git show f76c26a for the original README prefix; retain that commit.
Do not rerun gameplay tests/build/CI/corpus scans to resolve this delivery-only gate.

Current-host reproduction: Git ls-remote fails DNS resolver initialization; the
returned artifact file-reference download URL fails Invoke-WebRequest DNS resolution;
the alternative managed JS tool cannot initialize without usable CODEX_CLI_PATH.
Obtain a fresh file reference on retry; temporary signed URLs are deliberately
excluded from the persistent evidence. Current host Windows10.0.26200,PowerShell7.6.9.
No network-permission override or credential change was attempted.

## Confidence and limits

CONFIRMED ORIGINAL: earlier Work1 static authority reused; no fresh original analysis.
RECONSTRUCTED: earlier finite initialized outcomes preserved.
VERIFIED COMPONENT: existing CI tests confirmed by job log and success metadata.
ENGINEERING: owned initialization, resource profiles and checkout byte policy unchanged.
UNVERIFIED RUNTIME: original executable,shown GUI,speakers,physical input,owner corpus.
NOT VERIFIED DELIVERY: exact downloaded ZIP,source/fixture seals,bundled application.
No independent Work1 Issue24 QA, owner acceptance or subsystem completion asserted.
