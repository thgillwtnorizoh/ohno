Work2 development packet for Issue20. Original authority and 168 fixtures remain
read-only under re/campaigns/C001/control-count. This suite reuses published Work1
scalar geometry/schedule and connected-consumer assertions as DEVELOPMENT evidence;
it does not replace separately authorized new-source independent QA.

Reproduce from repo root with .NET8:
python reconstructed/campaigns/C001/issue20-development/verify.py /path/dotnet /path/output
Windows additionally executes reconstructed/C001LinearControlWindowsChecks against
the actual WPF application assembly, its portable-PDB compiled source checksum and
actual frozen BuildPath results. No Window, media device or original game is launched.
The complete r04-build workflow retains historical QA and packages both 168 control
and 106 traversal fixtures plus exact SOURCE-COMMIT marker.

Decode generated-fixtures.zip.b64 with Python base64.b64decode to recover the
additional development input files; generated-fixtures.json seals every byte.
Names of malformed sample probes include a process-local hash; content is sealed
and the test generator reproduces the same behavior without relying on names.
Large exact/one-over map tests are compressed; fixture generation code is in Stress.cs.

local-* evidence is prepublication modified-source development evidence. Windows
and later exact-source logs are recorded separately; do not substitute old CI.

Final reconciliation, 2026-11-22: exact Windows source
`5dda483348e21e79c8354bf6ea322d50b7fb3e72`; run#84/45002466140,
job137146777470, artifact14746253714. All54steps succeed;5430headless/564WPF/
2312historical and retained gates PASS. Downloaded ZIP/exe/source/PDB/bundle/
6method/274fixture/168disposition verification PASS. See
[complete UEP checkpoint and counts](windows84/checkpoint.md),
[artifact seal](windows84/artifact-seal.json),
[live CI metadata](windows84-ci.json) and
[verifier output](windows84/verification.log).
windows84.log.gz.b64 preserves the decoded job log as normalized UTF-8/LF without
BOM; decode base64 then gzip before rerunning unchanged check_artifact.py. The log
and result packet hashes are sealed. Windows-linked.json describes managed WPF
constructor data, not a shown window/rasterization test. Whole publish/build DLLs
have different hashes; six selected method bodies match exactly.
Initial pending statements and first-Windows failures are retained as history.
Evidence-only publication requires no new build/corpus scan. Next HQ review then
separately authorized Work1 QA. Issue20/C001/subsystem and Issue4 remain OPEN.
