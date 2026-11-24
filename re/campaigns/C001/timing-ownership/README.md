# Issue23 — original initialization and shared timing-list ownership

**VERIFIED CHECKPOINT**, C001 OPEN, production unchanged. Starting main
`eddb26540d2889bb5294b2630f279e136bed7230`; exact functional source
`5dda483348e21e79c8354bf6ea322d50b7fb3e72`.

[Numbered report](../original-behavior/21-timing-ownership.md) ·
[call/field graph and stage table](graph.md) · [Work2 handoff](handoff.md).

**Key edge:** M28f5 calls M28be(false,true,true), which sorts the shared timing
list **before** object parsing and slider/head preparation. M224d sorts again
later. Audio setup binds the same selected map by identity on reuse/new success.
The original getters and Sort operate on the same container; actual production
prepares from encounter order and copies global sample timing.

Actual results:40 selected original methods,17 call/field anchors,12 identity/
operand relations and64 reused body-hash comparisons PASS.233/233 component
characterization checks PASS;52 original-derived compatibility comparisons yield
**35 matches/17 mismatches**. Three real PCM resource requests corroborate the
minimal already-admitted v14 discrepancy. These are not passed original-equivalence
tests. Original/native/Windows/GUI/audible/device execution NOT RUN.

14 Issue22 charts are reused unchanged:12 exact comparator order controls and2
explicitly ambiguous equal-key domains. `conditional-oracle.json` records raw,
early-initialized and later visibility predictions. For ambiguous keys the early
and late sort may choose different permutations; listed same-order variants are
conditional, never a stable-sort claim. Original47 fixtures and2180 results remain
byte-identical. Only their integrity was rechecked;2180 were not rerun.

Reproduce from a fresh checkout/worktree, using the required Python dependencies
dnfile0.18.0, dncil1.0.2 and pefile2024.8.26, and .NET8 SDK:

```sh
python3 re/campaigns/C001/timing-ownership/extract.py /path/osubackup.zip \
  > /tmp/issue23-static.json
python3 re/campaigns/C001/timing-ownership/check.py /tmp/issue23-static.json
python3 re/campaigns/C001/timing-ownership/oracle.py /tmp/issue23-oracle.json
dotnet run --project re/campaigns/C001/timing-ownership/Checks -c Release -- \
  re/campaigns/C001/timing-ownership
python3 re/campaigns/C001/timing-ownership/verify.py
```

The runner writes production-results.json; preserve published evidence by running
in a fresh checkout and compare the recorded assertions separately. A successful
runner exit means the characterization and known discrepancy were reproduced,
not that all52 compatibility comparisons pass. Windows adapter checks are labeled
source inspection. Temporary sorted chart copies are explicit transformed operand
controls, not replacement archived fixtures or an implementation patch.

`provenance.json`, `environment.json`, `commands.json` and `seal.json` record exact
source, inputs, compiled DLL/PDB and checksums. Extractor decodes only the selected
40 methods and targeted slot metadata; no whole-original rescan. Authority and
parent file hashes are preserved.

`attempts/` retains the superseded late-only-sort oracle and195/39 results,
initial extractor syntax error, call/callvirt checker typo, and revised-runner
compile failure. The early-sort discovery changes rate and head-bank expectations,
and final233/52 results supersede those provisional conclusions. Large/CRLF traces
are losslessly packed as `.gz.b64`; decode base64 then gzip. Original byte hashes
and paths are in packed-evidence.json. No prior published record was edited.

Next bounded task: Work2 implementation under Headquarters assignment, using the
handoff's raw/prepared ordering and admission protections; then Work1 independent
new-source QA and appropriate Windows/owner gates. No profile expansion or short
timing defaults work is authorized here.
