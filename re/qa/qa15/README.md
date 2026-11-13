# 1. Result

QA15 — Work1 independent R17 verification and comparison corpus, 2026-11-13
(Asia/Bangkok). Base `f1df469987ca184d28b66825718873a486a2cfc0`; tested R17
production lineage `9ae1fbde06812adeaeac7df61de23c4d4268a51c`.
No production, research evidence, historical independent QA or historical journal
prefix changes. No exception. No R18/E047 started.

| Acceptance gate | Result |
|---|---|
| 1. Comparison corpus engineering | **PASS**:27 hashed files admitted by actual R17; documented-format/package validation. Original import unobserved. |
| 2. Independent component and Windows regression | **PASS**, bounded finite profile. |
| 3. Human original/reconstruction qualification | **PENDING HUMAN TEST**. |

Exact tested QA15 source: `116b140314d6ccad73303bda5d4cb1c888f93625`.
[Windows run43381876511](https://github.com/thgillwtnorizoh/ohno/actions/runs/43381876511),
job131741281970, **success**;2026-11-13 06:53:51–06:57:23UTC /
13:53:51–13:57:23Asia/Bangkok. Windows2022 / installed SDK8.0.427 /
actual.NET8.0.33 / OS10.0.20348. Release win-x64 **0 compiler warnings /0 errors**.
The final publication adds reports/coverage/evidence, the evidence checker and
append-only research entry; exercised C# harnesses, runner, expected packet and
production remain the exact tested source. Publication SHA is in the handoff.

| Executed gate | Result |
|---|---:|
| QA15 headless production components |95/95|
| QA15 unshown Windows components |35/35|
| QA15 explicit source inspections |4/4|
| **QA15 named total** |**134/134**|
| Retained historical independent QA |**2,175/2,175**|
| Combined named checkpoint, preserving classifications |**2,309**|
| R17 retained development / Windows groups |82/82 +12/12|

All21 retained development steps passed: R06 preparation4/fixture4, R07 8,
R08 13/C01 4/C02 14, R09 15, R10 23, R11 32, R12 40, R13 54+Windows1,
R13-C01 6, R14 58+Windows1, R14-Q01 23, R15 79+Windows8, R16 52+Windows12,
R17 82+Windows12. The historical independent suites remain
90/56/43/66/99/159/115/234/229/253/286/281/264 for
QA01/03/04/05/06/07/08/09/10/11/12/13/14 respectively. Development groups,
47 numerical traces,27 chart files and279369 numeric assertions are not extra
independent cases.

## Deliverables and reproduction

- `beatmaps/Songs/`:27 actual chart folders with generated silent local WAV files.
- `beatmaps/manifest.json`: final exact .osu SHA256, input/profile, independent
  expected Double, cached Single/bits and floor display; original observation blank.
- `beatmaps/comparison-results.csv`: both observed application values blank.
- [Comparison ZIP](QA15-comparison-beatmaps.zip): Songs plus individual `.osz`
  archives and human instructions.71,707 bytes, SHA256
  `be2168196216889faba4f60fef230f28bc6cbf3a39904338a81d06ec48a12499`.
- `human-test-protocol.md`, `human-results-template.md`, `coverage.json` and
  `15-star-calculator-verification.md` provide the remaining owner procedure,
  scope and verification pseudocode.
- `verification/` contains independent expectations, actual structured results,
  separate expected/actual final values, source hashes, coverage, run/step metadata,
  full returned logs and preserved setup/report/transport failures.

Follow **beatmaps/README.md** for exact Songs installation and optional osz imports.
Copy the **27 child folders inside Songs** into the original installation's Songs
folder; do not create Songs/Songs. Configure ohno's BeatmapDirectory to the same
extracted corpus or open each identical file and return to SelectPlay. Hash installed
files again and follow the human protocol. Both application observations require
physical owner testing; predictions are not GUI observations.

On Windows with installed.NET8 and Python, from repository root:

```powershell
./re/qa/qa15/Run-QA15.ps1 -ResultPath "$env:TEMP/qa15-results.json"
```

Read-only evidence validation from a Git checkout:

```text
python re/qa/qa15/build_corpus.py --check
python re/qa/qa15/package_corpus.py --check
python re/qa/qa15/check_evidence.py --preservation
```

The optional preservation mode pins the local historical byte representation at
baseline; Windows newline conversion of old files can differ. The ordinary oracle
adapter uses explicit LF-normalized historical source-transport hashes. New corpus
.osu/WAV/ZIP files have conversion disabled, and QA15 text uses LF. Mathematical
regeneration tolerates only declared scalar differences; discrete states, Single
bits and displays remain exact. Keep human observations in separate copies before
running template checks; the generator refuses to overwrite populated results.

# 2. What they found

**VERIFIED COMPONENT**: actual R17 admits every packaged positive chart, loads
its circles through the gameplay parser and matches the independent E046 oracle
across47 complete numerical histories:27 raw packaged charts, nine retained
prepared-only references and11 exact history-boundary probes. Prepared-only
controls do not expand raw admission or belong to the positive comparison corpus.

Each comparison covers original-derived preparation, pair geometry/history,
movement/jump/velocity terms, mutable rhythm identities/content/events/branches,
dense visibility past/future/angular scans, all three recurrences, duration
aggregation, final Double, exact cached Single and displayed floor. Oracle pair0
visibility diagnostics after its early return are intentionally excluded; its raw
zero is compared. The first two movement/velocity early returns are compared;
their separate jump geometry and complete composite are still covered.

Maximum final Double absolute difference: **5.329070518200751e-15**. Declared
scalar tolerance is relative5e-12 /absolute5e-13. **All47 cached Single values,
little-endian bits and displayed floors agree exactly.**

| Packaged discriminator | Expected and actual R17 display |
|---|---:|
| QA15-01 two straight circles |0.11★|
| QA15-02 three straight circles |0.17★|
| QA15-03 four straight circles |0.22★|
| QA15-09 combined rhythm/visibility |5.11★|

QA15-09 expected and actual Double both equal5.119475874072683; cached Single
5.119475841522217, little-endian bits`bfd2a340`. These are production/oracle
comparisons, **not observations of original-game stars**.

The corpus preserves all22 E045/E046 raw numerical inputs; only metadata/audio
packaging changes. Five new practice cases are independently recomputed, including
three straight patterns with+2500ms absolute timestamps and a129-circle64-second
history chart. The original fixtures start at0ms; use practice25–27 for easier
physical qualification and23/24 for sustained play. Manifest input timestamps
remain explicit and calculator time shift stays0.

Actual cache/controller tests cover content/profile identity, reuse, duplicate
cancellation, owner cancellation/retry, same-size/same-time changed bytes,
in-flight revision changes, missing files, stale generations/results, disposal,
contained failures and64-entry eviction. Unshown WPF verifies each of27 actual
chart identities and own ratings, pending/completion, restoration, stale-file
recovery, unsupported sliders/spinners, Rank Back and fresh Retry. Four host
wiring controls are explicitly source inspection. Matched session checks retain
score/health/Rank behavior; historical audio checks pass without physical listening.
No confirmed production discrepancy was found in the tested finite cases.

## Evidence, failures and limits

The immutable Work1 oracle imports only math/struct/collections; the QA15 builder
checks this AST boundary. It does not import R17 production or Work2's development
harness. Expected traces are separate from actual C# traces. The checker verifies
all47 histories again, source identities,134 unique controls, corpus/ZIP hashes,
all968 protected historical files and the493810-byte research-journal prefix.

Full returned passing job log:2,410,192 UTF8 bytes, SHA256
`85d174a3a3326c6c1fc5ccf91187a5f652b50b9c8da32121d598181b644c83d7`.
Headless packet:3,729,518 bytes, SHA256
`6ef9cada4c5e69842f9677677ddefc5c8099ed06b606f01cedae3d2937db055e`.
Windows packet:52,904 bytes, SHA256
`db4b1852fab22dc67b5f0ffda400912a8934dcdb4e2adebb797cc89e9370857b`.
Base64-decode and gzip-decompress `.gz.b64` files to recover the exact bytes.

The first local corpus adapter failed on the format-header section; fixed before
creating fixtures. Initial Windows sourceb5aa6e6 /run43381288711 failed only the
QA15 generator's slash/backslash provenance comparison, before QA15 C# ran; all
retained historical/development steps passed. Path.as_posix fixes that engineering
issue. A local report check initially included R17's pre-existing README-only
publication difference when comparing9ae to f1; corrected code scope, with the
separate f1 byte baseline still pinning the README. An intermediate transport
truncated the first failed log's base64 text; final publication restores the
original return and verifies its hash. Sources, dispositions and original logs are
retained. No mathematical expectation was weakened and production was unchanged.

**UNRESOLVED / PENDING HUMAN TEST**: original import and star displays, original
build/version/manager/provider behavior, native/JIT/library precision, actual
rendering, physical mouse/keyboard gameplay, audible effects and owner acceptance.
The archived research build's official release identity remains unauthenticated;
record the actual original executable even when it differs. Modern build star
values may differ. A mismatch is evidence to investigate; never fit unexplained
original displays. General stacking/provider/sliders/spinners/mod difficulty is
not qualified. Queue saturation and full4096-circle performance are not newly
independently qualified. E020's17 original-runtime cases remain NOT RUN.

There was no original executable/CLR invocation, GameWindow/show, OS input,
nonempty media playback, listening or screenshot by Work1. The silent support
WAVs were parsed as bytes only. Documented import-format validation is not an
observed import. Gate3 and final R17 owner acceptance remain pending.

# 3. Pseudocode

```text
CONFIRMED / EXECUTED finite verification:
  package exact researched numerical inputs + distinct metadata + silent audio
  hash final .osu bytes and verify actual R17 reader/gameplay parser admission
  calculate independent E046 expected histories without production imports
  execute actual R17 on those exact files; retain expected and actual separately
  compare geometry, movement, rhythm identities, visibility and all three states
  compare duration/aggregates and final Double within declared scalar tolerance
  require exact cached Single/bits and promoted-Single Abs/floor2 display
  exercise cache/controller revisions, cancellation and stale publication
  inspect actual unshown WPF binding and Rank/Retry components
  pass exact-source Windows build and unchanged historical/development regressions

RECONSTRUCTED provenance:
  original-derived mathematical interpretation remains bounded to E043–E046 profile

UNRESOLVED / PENDING HUMAN TEST:
  owner imports identical hashed files and physically runs both applications
  record actual displays, executable/build/settings, screenshots and gameplay
  classify each observed comparison; preserve mismatches for bounded investigation
  only then assess the separate human acceptance gate
```
