# R16 — Original-derived local SelectPlay

## 1. Result

Work2 implements the authorized bounded R16 browser from canonical
`352ea0b161153565e1ec2748d51dfc8cb3813b81`. E043 is primary original-static
authority, with E002/E003 transitions, E004/E005 ordinary loading, E023 record
policy, E027 completion, E033/E034 accounting and E042 Rank preserved.

**PASS — exact-source Windows verification completed before canonical publication.**
Tested source: `19bb644e01c646ca8850f0751ec03f7ef881d766`.
Run: <https://github.com/thgillwtnorizoh/ohno/actions/runs/43153555528>,
job130968150828, Windows2022, installed SDK8.0.427, 2026-11-12 04:15–04:17UTC (11:15–11:17Asia/Bangkok).
WPF Release win-x64: **0 warnings, 0 errors**. All **1,911/1,911 independent checks**
and every retained development suite pass. New R16 **52/52** headless groups
(50 component,2 source) and separate **12/12** Windows groups (11 component,1 source)
pass on the first Windows run. No initial failing gate or weakened oracle exists.

Publication is a report/evidence/README/coverage-only child of this tested source,
using an expected-head, non-force update to canonical `main`. The final handoff gives
the publication SHA; `verification/r16/gate.json` pins the tested code/harness/workflow
hashes independently of self-referential publication metadata.
No local .NET SDK, application Window, OS input, nonempty media, preview or listening
was used. Owner navigation remains unverified for R16.

Added production files `CircleGame/Selection/{LocalChartCatalog,SelectPlayModel}.cs`
and `CircleGame/Rendering/SelectPlayView.cs`; modified only `CircleGame/Program.cs`
for host integration. Existing parser, session, ledger, health, Rank and audio
implementations remain byte-identical. New development harnesses are
`R16DevelopmentChecks` and `R16WindowsChecks`; existing Windows workflow adds only
these paths and two steps. README usage changes and this report accompany an
append-only journal entry. No original research, QA01–QA13 oracle/source/evidence,
fixture, historical report or journal prefix is changed. No exception is requested.

The executed retained independent gate is **1,911** named checks: QA01 90, QA03 56,
QA04 43, QA05 66, QA06 99, QA07 159, QA08 115, QA09 234, QA10 229, QA11 253,
QA12 286 and QA13 281. Generator outputs and development groups are not counted in
that total. QA11 G08 remains unchanged. Executed retained development groups include
R06 preparation4 targets/4fixtures, R07 8, R08 13, C01 4, C02 14, R09 15, R10 23,
R11 32, R12 40, R13 54 plus Windows1, R13-C01 6, R14 58 plus Windows1,
R14-Q01 23 and R15 79 plus Windows8.

New executed verification is **52 headless development groups** (50 actual production
component groups, two explicitly source-inspected groups), plus **12 separate
Windows groups** (11 unshown component controls, one source audit). These are
Work2 development controls, not QA14. One headless group compares four matched
finite old-loader/SelectPlay-loader sessions. E043's 66 predictions are a separate
coverage inventory, never 66 additional executed tests.

Verification evidence is in `verification/r16/`: full GitHub decoded job log compressed
as gzip/base64, exact structured R16 development output, readable step/group summary
and gate/source/hash manifest. Decoded log SHA256:
`32ee4048455b6fa196ff19b373519b8129489e65643febd64efd5f47c4a81700`
(869,069 bytes, original UTF8/BOM/CRLF preserved). QA11 reports247 component/source/
asset +6Windows=253; QA12 reports274+12=286; QA13 reports257+24=281. The unchanged
G08 settings interleaving records expected0.1/0.1 and actual0.1/0.1.

The finalized E043 matrix records **21 finite /24 partial /21 unsupported** predictions,
with evidence sites, published expected behavior, bounded scope, actual passing group
references and engineering exclusions. All66 retain original-runtime uncertainty.
The finalized matrix changes documentation only; production, harness executable
sources and workflow remain exactly those tested. No static row is an additional QA
case. Gate preparation and final outcomes are append-only in the project journal.

## 2. What they found

### Authority and boundaries

**CONFIRMED STATIC:** E043 report and complete parsed JSON were read, including all
66 predictions. `re/evidence/43/selectplay.json` is 2,610,266 bytes, SHA256
`9ac182e9b6af5faaca3e517c4500bbeef574b715af01350f5d223b8fc88324a4`.
M3ebc/F2557/M2581/M2c64 distinguish browser commit and global-map update;
M2578/M2577/M46ae/M4711/M46b0/M2237 distinguish Start and later live-map capture.
E005's ordinary/forced-Taiko correction is retained. E043 supplies original resource
and state evidence, not measured browser layout or an implemented local calculator.

**RECONSTRUCTED:** the finite model separates highlighted `CatalogChart`, committed
row, `SelectedChartState.Current`, `PlayEntryFlow.Captured` and loaded `Beatmap`.
Same-reference selection performs no cleanup/store; null callback leaves the valid
selector intact. Commit writes the row before invoking its callback. Injected old
cleanup failure can prevent store; a failure after store does not fabricate rollback.
Callback containment and explicit status messages are engineering policies. Same-run
highlight can commit immediately; a different run stays pending until confirmation.

### Catalog identity and preparation

**ENGINEERING:** `selection.json` is read from `AppContext.BaseDirectory`, exact-case
JSON keys `BeatmapDirectory` (default `Songs`) and `PreferUnicode` (default false).
Relative roots resolve against that executable directory. The configured root,
bundled Fixtures and process-local external picker paths form a bounded metadata
index. There is no `osu!.db`, filesystem watcher, binary cache import or original
profile-based preferred-difficulty chooser. Configuration errors leave fixtures and
visible recovery available. The picker/command line and historical fixture toolbar
continue to load paused and register external selection afterward.

Canonical full paths use Windows ordinal-ignore-case (ordinal elsewhere); UNC and
symlink/reparse ancestry are excluded. This is deliberately stronger than original
filename-only M1bf4. Identical filenames in unrelated folders and duplicate online
chart/set IDs are retained, not silently merged/discarded. Prepared paths sort by
that comparator. Adjacent available rows of an identical containing folder form
runs; supplied s/t/s forms three runs. An unavailable row terminates a run. UI exposes
unavailable rows separately and never groups unrelated directories by online ID.

A revision is length + write-time ticks + SHA256. Refresh preserves the exact old
reference only when that identity and admission are unchanged and there is no prior
load failure; changed revisions create new rows. A retained selected reference is
never silently substituted by same-path changed bytes. Newer scan generations alone
install; cancelled scans leave the prior snapshot. Limits are 4MiB/chart, 100,000
lines/chart, 10,000 charts/external entries, 4,096 directories, depth32, 50,000 examined
file candidates and at most10,000 candidates/directory; limit/IO/root notices are
bounded to32. Truncation is explicitly partial, not a complete inventory guarantee.

Metadata is prepared once per scan, never full gameplay parsing on highlight. BOM
UTF8/UTF16 input is detected. Title/artist/creator/version, format/ruleset, finite
HP/CS/OD/AR, object types/count, positive-base-timing BPM range and circle/spinner
stored end are display operands. Missing title/version falls back to filename;
optional artist/creator and scalars can remain unavailable. Unicode preference falls
back to ordinary text. Invariant culture is engineering. Slider duration is marked
unavailable until preparation. These display fields never feed E034's K, health,
slider geometry or gameplay timing. Indexed means metadata admitted; full existing
parser may still reject the chart. Successful host load marks a revision validated;
refresh permits retries of previously failed revisions.

### Play request and error handling

**CONFIRMED STATIC:** ordinary Start requires selected map, !special-temporary flag
and phase2. Original request arguments include Play2/false/1000 but no immutable map
payload. M2237 later reads the then-current selector. Direct file existence is not
an M2578 request guard. Rank Back requests SelectPlay5 without a chart payload and
initialization/relookup reads the live selected reference.

**RECONSTRUCTED:** `PlayEntryFlow` exposes Selected → StartRequested → Accepted →
PendingPlay → PlayCommitted → Captured → Loading → Ready, with Failed/Cancelled/
Disposed states. Duplicate in-flight Start is rejected without a second load. Guards
are independently exercisable. Selection changed before capture affects Play;
changed after capture leaves that Play reference intact. A request may succeed yet
file/parsing/audio loading fail.

**ENGINEERING:** one dispatcher turn commits the queued request. Scalar1000 is
retained as evidence, not `Task.Delay` or native duration. Request generation is
checked before commit and load generation/reference/token before installation.
The existing parser, scoring preparation, song decoder and optional effects loader
run in the established order. Fingerprints are checked before parsing and again
after all awaited preparation, immediately before replacing the old valid host.
No synchronous decoding is added to gameplay. Missing/changed/invalid chart,
unsupported existing parser object/format or missing declared song audio fails
visibly, disposes candidates, and leaves a previous valid session paused. Missing
optional effect resources remain nonfatal under the existing policy. A change after
the final validation is an unavoidable filesystem race; the installed session keeps
its parsed in-memory map rather than rereading changed bytes during gameplay.
Refresh/Back/close cancels pending work; stale completion cannot install or replace
newer status. No locks/native queue equivalence or full mode-manager reconstruction
is claimed.

### Windows navigation, Rank and lifecycle

**ENGINEERING:** independent WPF song and difficulty lists, detail pane, explicit
highlight/committed/selected labels, status, Commit/Play, Refresh, Open and Back.
Default entry is SelectPlay. A first row click commits; repeated selected-row click
can Start. Up/Down move available rows; Page Up/Down clamp ±10; Left/Right commit a
pending highlight or navigate runs. Native list scrolling handles the wheel. Escape
returns to named Local Practice Home, not an original Menu0 clone. Exact wrap,
mouse geometry/release timing, native layout and scroll restoration are excluded.
Controls route by visible browser/home/Rank/gameplay state. Rank Space/Enter still
skip reveal; Retry remains fresh and independent of later browser selection.

Rank Back initializes the browser against the live authoritative selector and starts
a refresh. Available identical references restore difficulty and accessible group;
changed or missing selection is an explicit recovery state with no first-row
replacement. Entering browser pauses existing gameplay, clears logical held buttons
and leaves existing audio lifecycle safety intact. Installation uses the existing
fresh surface.Load path. Existing Rank grade/stats/reveal and audio diagnostics are
untouched. Old compatibility loader and new catalog LoadGameplay invoke the same
parser and accounting preparation. The deferred Joined Bézier score discrepancy is
unchanged, neither fitted nor investigated.

**VERIFIED COMPONENT** from the successful exact-source gate: the new finite controls
exercise production catalog/selection/flow, controlled publication interleaving,
revision changes, failures, actual parser, shared gameplay/session/Rank history and
unshown WPF widgets. GameWindow keyboard/dispatcher/installation wiring is explicitly
source-inspected rather than an automated UI interaction. Unshown tests do not
establish human navigation, window pixels, media audibility or OS input fidelity.

### Star and preview policy

No star value is fabricated. Normal UI says **Stars not calculated · no compatible
cache**. The optional component-only `SuppliedStar` contract requires explicit
`osu-stable:E043:cache-v1` compatibility, source, chart SHA256, ordinary ruleset and
normalized modifiers (`mods & 1370`). This tag is an engineering admission contract,
not independent certification of a producer. Hash mismatch is stale, key mismatch
is mismatched; negative cached magnitude is displayed as stale. Finite Single is
promoted to Double, Abs applied, floor(value*100)/100 then invariant `0.##★`.
Nonfinite values are unavailable. There is no file importer or automatically supplied
cache and no original M08d9/alternate/lazer calculator. E034 K is never a star formula.

Selection is silent; no preview player, original receiver/fade/thread scheduling,
interface sounds or proprietary assets. The 66-row matrix retains original expected
text, evidence sites, bounded scope and supported/partial/unsupported classification.
All original-runtime rows remain **UNVERIFIED ORIGINAL-RUNTIME**. Full binary database,
original loading19/profile/frontend, native preview, calculator/cache workers,
proprietary skins/graphics and original scheduler are excluded.

### Owner-only qualification

Build the documented Release app on Windows; `selection.json` goes beside its actual
executable (for `dotnet run`, use `bin/Release/net8.0-windows` output, or the selected
runtime output directory). Point BeatmapDirectory at a local Songs root, restart,
then Refresh. Verify song-folder grouping with two `.osu` difficulties, highlight a
different song without committing, confirm difficulty, then Play. Physically play
to accepted R15 Rank, Back, verify the selected difficulty/group, and Start again.
Verify Retry starts a fresh same Play map; Rank Enter/Space only skips reveal.
Delete/change one indexed file, Refresh and verify explicit recovery, then choose
another chart. Check unsupported/invalid rows and missing declared song recovery.
Existing fixture/picker and Resume loaded chart routes remain available. No preview
or guessed stars should appear. These steps are prepared, **not performed by agent**.

## 3. Pseudocode

```text
CONFIRMED STATIC / RECONSTRUCTED finite model:
  index local chart metadata separately from full Play parse
  for each supplied available adjacent folder run: retain individual row references
  highlight(row):
    retain highlight candidate
    if supported current committed run: commit(row)
  commit(row):
    set committed row
    invoke nonnull callback -> authoritative selection
    same-reference/no callback does not clear valid selected map
  Start:
    guard selected != null, ordinary selection, phase2, no in-flight duplicate
    request -> accept -> pending Play (no chart payload)
  later commit:
    commit Play -> capture fresh authoritative selected reference
    load captured chart -> existing gameplay parser/preparation
  Rank Back:
    request SelectPlay -> initialize -> fresh selector reference lookup
  Retry:
    existing R15 fresh session from captured Play map

ENGINEERING catalog / host safety and presentation:
  read app-relative selection.json; default Songs + bundled Fixtures + external files
  scan bounded local paths; preserve unchanged revision references
  publish only latest noncancelled scan; never substitute changed selected revision
  queue next dispatcher turn; reject stale request generation before commitment
  verify captured file; existing parser/song/effects preparation; verify file again
  install only if current load ticket/token/reference and host not closing
  otherwise dispose candidates, retain prior paused valid session, expose failure
  browser is silent independent WPF; invariant metadata; stars unavailable by default
  supplied compatible star only -> mode/mod/hash check -> abs Single->Double floor2

UNVERIFIED ORIGINAL-RUNTIME:
  native queue interleaving, preview/device audibility, original database/cache worker,
  native layout/input geometry/resource fallback and full mode-manager guards
```
