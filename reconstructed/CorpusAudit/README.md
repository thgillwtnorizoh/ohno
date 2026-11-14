# Read-only local beatmap corpus audit

C001 / Issue #5 / Work 2. Requires .NET 8 SDK and Git when building; the packaged
framework-dependent tool needs only the .NET 8 runtime. No original client/media
is required. This is a headless diagnostic, not certification of playability or
original osu!stable equivalence. C001 remains OPEN; Issue #4 owner qualification
remains separate. The owner's larger collection has NOT been provided/tested.

From the repository root (Windows PowerShell or Linux/macOS shell):

```sh
dotnet run --project reconstructed/CorpusAudit/CorpusAudit.csproj -c Release -- "C:/osu/Songs" --output "C:/ohno-audits/audit.json"
```

For the downloaded tool package:

```sh
dotnet CorpusAudit.dll "C:/osu/Songs" --output "C:/ohno-audits/audit.json"
```

Use your own local directory/output paths. Output must be a `.json` file outside
the scanned tree. Without `--output`, JSON goes to stdout; the compact summary goes
to stderr. Do not redirect stdout into the scanned tree. The runner never writes
charts or imports/extracts `.osz` files. Output publication is your choice; no
network service, upload, media decoding or original executable is used.

Paths, chart text, titles, artist/creator metadata, audio names, timestamps and raw
exception messages are omitted. Each entry has SHA256 of its normalized relative
path as `entryKey`, plus exact-byte chart `sha256`. The latter is null for unreadable
or resource-excluded charts. Hashes describe the captured read snapshot; changed-input
rows do not certify the final on-disk revision. **`--include-relative-paths` is owner-local only**:
use a second private report with that flag to match failed keys to files, and do
not commit/share it. Hashes still identify charts and may warrant private handling.
Only synthetic repository fixtures are used in published/CI evidence.

## Reading the report

Schema1 records build commit/state and exact linked source hashes. A locally
modified checkout is marked `modified`; use a clean verified revision for shared
results. The packaged binary retains its build provenance even when moved elsewhere.

Stages are independent:

- `read`: exact bytes/hash read under the 4 MiB catalog limit.
- `featureProjection`: production OsuDocument/ReadStage and metadata/type-priority
  projection. Counts describe runtime source rows, not objects already admitted.
  `indexedFeatures` separately retains the catalog's category counts, which can
  differ from runtime priority (for example combined type bits).
- `index`: actual `LocalChartCatalog.Index`; this diagnostic wrapper intentionally
  supplies deterministic traversal instead of the UI catalog refresh traversal.
- `canonical`: actual `CatalogChart.LoadCanonicalGameplay`, including fingerprint
  verification before/after production `Beatmap.LoadCanonical`. It is attempted
  even when catalog metadata admission rejects a fingerprinted chart. The legacy
  `Beatmap.Load(string)` entry point is never used.
- `consumers`: finite geometry/sidecar identity checks, actual automatic score
  preparation, actual session construction with health disabled, and actual fresh
  HealthChart construction/validation. Health calibration, decoded audio, custom
  samples, rendering/input/gameplay execution and owner experience are NOT checked.
- `audio`: declared audio existence under local/reparse safety checks only. Missing
  audio does not invalidate an otherwise prepared chart; no filename is emitted.

`standard-no-spinner` is the primary source-feature cohort: normalized Mode0,
zero spinners, and no unclassified/other runtime object categories. Spinner-bearing
standard charts and other modes have separate cohorts. Unknown metadata/rows stay
unknown. A source-feature cohort does not imply admission or compatibility.

`cohorts` reports per-file counts, canonical attempts/success, indexed counts,
consumer-ready counts, missing audio and distinct chart hashes. All duplicate
occurrences are audited because resources can differ between folders; duplicate
hashes point to a deterministic first `entryKey`. Unique hashes are reported per
cohort. Read/excluded/unknown charts are never silently included as successes.
`stageOutcomes` supplies every stage's denominator; `canonicalFailureFamilies`
provides conservative normalized reasons for Headquarters prioritization.
Production exceptions are currently text-based, so a documented allowlist maps
known messages to safe codes. Unknown InvalidDataException rejections remain
`unclassified-rejection`, not invented malformed/unsupported diagnoses. Unexpected
exceptions are isolated and retain only their type. This taxonomy is engineering,
not an original-game error contract.

Format, normalized Mode, circle/slider/spinner/other counts, explicit L/B/P/C
markers, repeat min/max/unknown counts, timing row and distinct beat-length counts
are derived through production document readers. Explicit base/inherited timing
counts report only literal flags0/1; missing/default flags are not guessed.
`unparsedBeatLengthRows` is distinct from the explicit flag counts. Stacking profile
labels describe existing finite CS5/AR5/no-mod preparation boundaries, not a new
compatibility implementation. Versions and stage-specific exclusions remain visible.

## Bounds and repeatability

Defaults:10,000 charts;4,096 directories;50,000 filesystem entries (including
non-charts);depth32;maximum10,000 children per directory. Options
`--max-charts`, `--max-directories`, `--max-entries`, `--max-depth` may only tighten
catalog guards. Per-file catalog4 MiB and gameplay2 MiB limits are preserved, as
are canonical object/geometry/checkpoint/health guards. One chart is prepared at a
time and its production model released before the next; report memory is bounded
by the chart limit. No unbounded whole-tree enumeration or duplicate shortcut.

Entries use ordinal sorting. If a directory exceeds its per-directory or remaining
entry budget, it is excluded as a whole; an arbitrary filesystem-order prefix is
never treated as deterministic coverage. Any truncation/depth/reparse/access issue
makes `complete=false` with hashed notice keys. At most1,024 notices are retained;
additional notices are counted. Root/ancestor and descendant symlink/reparse paths
are excluded, including audio references before presence checks. Standard filesystem
APIs cannot promise an atomic snapshot under hostile path replacement; avoid
editing/moving the collection during a scan. Production fingerprint checks detect
observed changes; this is not an adversarial filesystem sandbox.

JSON is byte-reproducible for identical bytes/resources/relative layout, limits,
platform/build and unchanged files. It includes no wall clock, absolute root or
modification times. Renaming a folder changes its entry keys. Cancellation and
concurrent mutation necessarily produce partial/event-dependent evidence. Ctrl+C
cancels cooperatively; finite production operations without cancellation points
finish before control returns. Fatal runtime failures such as out-of-memory are
not converted into false completed reports.

Exit codes:0 completed scan (may contain admission/resource rejections),2 partial
scan/cancelled,3 completed scan with unexpected failures,64 invocation/output error.
Review stage outcomes rather than interpreting exit0 as compatibility success.

## Verification and owner handoff

```sh
dotnet run --project reconstructed/CorpusAuditChecks/CorpusAuditChecks.csproj -c Release -- . /tmp/ohno-audit-checks
```

Use an outside-tree temporary evidence directory. Focused checks exercise the
mixed synthetic tree, deterministic output, CLI, file mutations, duplicates,
missing/blocked files, bounds and six preserved fixture roots: C00124, Work149,
P21, C19, stacking18 and QA1527. Synthetic mutation hooks exist only for the friend
test assembly; the CLI exposes none. Tests compare hashes and modification stamps
around the ordinary read-only scan. CI runs Release on Windows and Linux, retaining
only synthetic reports and a runnable tool package. No production behavior change
means the full historical Windows application gate is not repeated for this tool.

Owner: run against the local collection, retain the path-bearing report privately,
and share only an authorized summary/report. Headquarters then selects one supported
failure family; Work1 independently verifies this runner and supplies original
research where needed. Late timing-offset investigation is explicitly deferred;
clock/offset/audio compensation and R17 mathematics are unchanged.
