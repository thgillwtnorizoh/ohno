# Attempt disclosure

The initial compile attempt failed with six CS0103 errors because the reused
resource-cancellation partial file lacked `using System.Reflection`.
`first-compile.log` retains the exact output. No tests ran in that attempt.

The first completed characterization was2695/2702 PASS, seven failed assertions.
`first-characterization.log`, `first-results.json`, `first-Program.cs.txt` and
`first-build_fixtures.py.txt` retain that runnable source/expectation epoch.
Four failures assumed the requested "insert" generation flag meant the first
point differed; duplicates can put it back on the head. Actual converted equality
is now stored separately from the requested flag, without changing fixture bytes.
Two failures expected requested4096/32 traversals/tick8 to exceed checkpoints,
but short raw G suppresses ticks. That original case remains admitted and named
as before; a new genuine long-raw-path checkpoint negative was added. The seventh
failure was the dependent expected preparation total. No production bug was
established by these oracle mistakes.

The corrected intermediate154-case run passed2709/2709,38/116,101 probes. No raw
record of that intermediate run is claimed as final evidence. The final suite
adds v7–v13 bounds, full independent schedule comparisons, preserved legacy
profile assertions and public metadata checks:168 cases,3324/3324 PASS,38/130 and
110 isolated probes. These counts are separate, not cumulative.

All153 initial .osu fixture byte hashes are identical in the final manifest;
`initial-fixture-hashes.json` pins them. Only metadata/expectations were corrected
and15 new .osu files added. Historical repository fixtures and tests never changed.
This work used scratch-only setup for SDK/private inspection; no private original
disassembly, executable or assets are included in this packet.
