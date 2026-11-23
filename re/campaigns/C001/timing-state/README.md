# Issue22 — bounded timing and sample-state packet

**VERIFIED CHECKPOINT**, Work1, C001 OPEN. Exact functional source
`5dda483348e21e79c8354bf6ea322d50b7fb3e72`, identical production at starting main
`e4fbf88d7512420add0afb1ba02e4dd7a9f993b8`. Full report and staged pseudocode:
[20-timing-point-state.md](../original-behavior/20-timing-point-state.md).

47 immutable synthetic charts:42 canonical admissions (38 connected real-resource
sound cases,4 explicit sound limitations),5 expected engineering rejections.
New independent2180/2180; retained development264/264, QA10 229/229,
QA11 247/247 PASS. No production fix established; original lifecycle/list mutation
remains an explicit next research dependency. Original/native/Windows/GUI/audible/
physical input/owner-private corpus NOT RUN. No full Windows/package/historical run.

`manifest.json` holds raw timing rows, input hashes, independent scalar/date/query
oracles and confidence. `matrix.md` is the per-case disposition table. `results.json`
records each assertion and actual tagged resource requests. `provenance.json` pins
source/archive/exe identities; `evidence-map.json` pins reused authority and methods.
`resources/tone.wav` is an author-created0.2-second PCM16 sine wave, not game audio.
Checks/ compiles unchanged actual production gameplay/catalog/resource sources;
no fabricated sound handles or substitute parser is used.

From repository root, use the installed SDK8 and PowerShell paths:

```sh
python3 re/campaigns/C001/timing-state/build_fixtures.py
dotnet run --project re/campaigns/C001/timing-state/Checks -c Release -- \
  re/campaigns/C001/timing-state /tmp/issue22-new-results.json
dotnet run --project reconstructed/C001DevelopmentChecks -c Release -- "$PWD"
pwsh -NoProfile -File re/qa/qa10/Run-QA10.ps1 -ResultPath /tmp/issue22-qa10.json
pwsh -NoProfile -File re/qa/qa11/Run-QA11.ps1 -SkipWindows -ResultPath /tmp/issue22-qa11.json
python3 re/campaigns/C001/timing-state/verify.py --output /tmp/issue22-audit --audit-only
```

For a complete bounded reproduction into a fresh output directory:

```sh
python3 re/campaigns/C001/timing-state/verify.py --output /tmp/issue22-fresh \
  --dotnet /absolute/path/to/dotnet --pwsh /absolute/path/to/pwsh
```

The actual execution paths/environment are recorded in environment.json and
commands.json. Existing full-tree E037–040 checkers fail on later list-shaped
evidence envelopes; unchanged checker semantics pass on a compatible copied
envelope. `static-checks.json` names all excluded files and separate failure logs.
This is not a repaired historical checker or a fresh original extraction.

Attempts preserve first2040/2042, corrected44-case2042/2042, initial expected-value
files and the missing-argument retained invocation. E600 end3399 is explained by
E005 divide-then-multiply chronology, not an empirical oracle change. The subsequent
three controls address absent-base/before-first/finish-boundary questions; input
bytes of the original44 charts remain identical. Final47 chart manifest is sealed.

Large retained traces are losslessly packed as `.gz.b64` with original hashes in
packed-evidence.json. Decode base64 then gzip. `seal.json` covers research inputs,
runner, final output and attempt records, excluding compiler products and itself.
Reproduction may produce different platform/source-path trace details; preserve
the published results and compare substantive assertions in the new output folder.
