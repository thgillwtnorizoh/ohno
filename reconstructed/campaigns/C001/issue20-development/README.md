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
