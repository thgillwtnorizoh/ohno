# 1. Result

**QA01 — independent static and automated verification**, Work 1,
2026-11-02 (Asia/Bangkok). Target: Work 2 R02 commit
`140f2db00a513221dde4d00cd9897663635375c1`, verified as current `origin/main`
before work. Publication also uses `main` with an expected-head check. No
permanent QA/expedition branch is created.

For the player-facing circle rules, R02's finite timing comparisons and timeout
polarity match the recovered specification by source inspection. This does **not**
establish that its Windows application launches, draws correctly or accepts real
input. One non-finite geometry discrepancy and one malformed-input validation gap
were identified. Neither is an observed normal-play failure.

## Actual execution and limits

| Check | Actual result |
|---|---|
| QA01 automated static source/original-byte checks | **37 run: 35 PASS, 2 FAIL, 0 errors, 0 skipped**; exit1 intentionally exposes D01/D02 |
| Pinned original reference | ZIP/EXE SHA-256 and seven selected method-body hashes match; selected CIL branch bytes/targets checked directly, read-only |
| Work 2 source identity | All 13 R02 changed paths pinned in `provenance.json` and matched |
| R01 legacy regression | 12 test methods PASS, including its 22 static-contract fixture cases; distinct from R02 |
| Historical repository checks | All29 E02–19/E21–31 checkers PASS; in-memory compilation of67 Python files PASS; Git fsck exits0 with existing dangling local preparation objects, no corruption |
| R02 headless C# component suite | **NOT RUN — .NET SDK unavailable**; source-linked suite supplied, zero C# behavioral cases executed here |
| R02 Windows launch/build/visual/physical-input verification | **UNVERIFIED**; no new build attempted, application never launched, no owner captures supplied |
| Original osu!stable runtime / E20 | **NOT RUN**, E20 still incomplete, all17 original-runtime cases unexecuted |

`dotnet`, `csc` and `mcs` were absent from PATH; R02's documented
`/tmp/ohno-dotnet/dotnet` was also unavailable. These were bounded capability
checks, not a launch attempt. No runtime installation, environment repair,
virtualization, desktop automation, simulated OS input, OBS, debugger or original
software execution was performed. No owner attendance or routine supervision was
requested. QA continued with the available static/automated checks.

Work 2's earlier build/blocked-launch statements remain historical claims in its
R02 report; QA01 neither repeats that launch nor promotes them to independent
Windows verification. Missing C# execution is an automated-test coverage gap,
separate from the prohibition on interactive operation.

## Assets and reproduction

All QA01 assets are under `re/qa/qa01/`:

- `provenance.json`: pinned target/source/report hashes and original method byte
  ranges/body hashes. Original archive/binary bytes are not published.
- `test_static.py`: bounded assertions about the reviewed source and original
  CIL bytes. It is not a general C# parser/compiler or a gameplay simulator.
- `run_static.py`, `results.json`: repeatable machine-readable actual static
  results, including the two failing checks and explicit unexecuted categories.
- `regressions.json`: actual historical-checker/R01/compilation/Git integrity
  results, kept separate from R02's unexecuted C# component cases.
- `Headless/QA01.Headless.csproj`, `Headless/CoreTests.cs`: deterministic tests
  linking the **actual unchanged** R02 Beatmap, CircleSession and PlayfieldLayout
  sources. No copied replacement classifier, Windows GUI project, GameWindow,
  physical input provider or application entry point is linked.
- `fixtures/bad-hitsound.osu`: independent malformed-input reproducer, not an
  original-import/gameplay fixture.

Run the performed checks from repository root:

```sh
PYTHONDONTWRITEBYTECODE=1 python re/qa/qa01/run_static.py \
  --archive /private/osubackup.zip --output /private/qa01-results.json
# Expected at the reviewed R02 revision: exit1, 35 PASS and D01/D02 FAIL.
PYTHONDONTWRITEBYTECODE=1 python -m unittest discover -s reconstructed/tests -v
```

The static runner never runs C# even on a host that has an SDK. On a suitable
headless .NET8 SDK host, Work 1/CI can separately run the following **unit suite**:

```sh
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- /absolute/repository/root
```

This command is **NOT RUN here**. It exercises pure components using method
arguments and temporary parser files, not OS keyboard/mouse events or a running
game. It has no WPF dependency and must not be replaced with `dotnet run` on the
application project. Known D01/D02 expectations intentionally remain failures
until Work 2 decides and implements corrections; no failures are hidden as
expected successes. The C# suite itself is not compilation-certified on this
host. Source checks are explicitly not substitutes for executing that suite.

Later Work 2 changes require a fresh source review and updated provenance; passing
substring checks against alternative code forms is not an equivalence proof.

Historical reports, research pseudocode, evidence, fixtures, E20 materials and
every Work 2 application/implementation file remain unchanged. No new gameplay
model, application fix or expedition is introduced. Publication checks cover
the exact QA-only file scope, ancestry, all retained blobs/modes and fresh remote
head. The final response supplies the verified publication SHA.

# 2. What they found

## Confidence labels and original evidence

**CONFIRMED STATIC** identifies inspected source, original metadata/instructions
or byte/hash checks. **RECONSTRUCTED** identifies the consequence of those
instructions. **UNRESOLVED/UNVERIFIED** identifies missing runtime/configuration
evidence. No original gameplay observations are available. Source-derived
consequences below are not described as executed R02 C# results.

Original reference hashes:

| Input | SHA-256 |
|---|---|
| `osubackup.zip` | `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738` |
| `osu!.exe` | `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d` |

Self-declared Stable40/file-version1.3.3.8 remains unauthenticated as to official
dated identity/modification status. QA uses the pinned reference, not another
osu! version. Existing E05 ordinary/Taiko and E12 update-order corrections are
preserved. `Mxxxx/Fxxxx/Txxxx` expand to MethodDef0600xxxx, FieldDef0400xxxx and
TypeDef0200xxxx; offsets here are hexadecimal relative to the first CIL opcode.

| Existing research reviewed | Original relationship relevant to R02 | QA disposition |
|---|---|---|
| E07 candidates | M28be/M28b3 approach anchors1800/1200/450; F17fb full list versus F17fd candidates | Approach preparation matches no-mod finite source form; R02 full-list scan is documented simplification |
| E08 visual states | M45ec face fade400 and approach4→1; separate F2bf8/F2bf9 and time-dependent opacity | WPF primitives and immediate judged disappearance are explicit scope choices; no pixel equivalence claimed |
| E09 drawing | M451a→receiver vertices→batch/backend, with resource gates | R02's WPF backend is independent; no original resource/quad/GPU evidence is transferred to WPF pixels |
| E10 clock | Stored corrected F2358 and distinct normal publication/input/update work | Stopwatch/latest WPF-frame time is documented engineering policy, not the recovered audio clock |
| E11 input | M2295→M28c8/M28c9→M24c7; M0ad9 strict abs error<400 and earlier end+3 comparison | Finite order/admission comparisons present; per-key WPF edges, missing candidate machinery/predecessor gates are disclosed |
| E12 judgement | M1884 strict comparisons and raw results; M28e4 strict timeout; evaluated exclusion | R02 source matches finite classification/timeout polarity; no full manager raw/decorated result implementation claimed |
| E13 mapping/radius | M0ff1 subtraction/reciprocal; M28be separate radius; M24c7 ordered distance²≤R² | Finite mapping/radius form present; unordered comparison differs (D01) |
| E14 draw/input alignment | Conditional q1 CPU mapping, resource origins/rounding and unresolved native/shader/pixel boundary | R02's fixed-aspect DIP stage is a disclosed independent layout; physical DPI/display alignment remains unverified |

Focused binary confirmation reuses published metadata rather than repeating
excavation. The runner hashes the original method ranges at the recorded file
offsets. Seven bodies checked: M28b3, M24c7, M0ad9, M1884, M28e4, M28be and M0ff1.
It also checks actual original opcode bytes and relative destinations:

- **M1884:0025/0040/005b**: `bge.s` to0034/004f/006a. Equality leaves the better
  quality branch; raw1024/512/256/−131072 correspond to the E12 scoped outcomes.
- **M24c7:0054**: ordered `ble.s` to0070, not `ble.un.s`. Its preceding operands
  are DistanceSquared(P,F252c) and R*R in the existing E11/E13 evidence.
- **M28e4:00bc**: `bge.s` to00d6 skips when end+F0adc≥fresh F2358, so circle
  expiration requires strict T>end+W50 and the other original gates.

No original helper was invoked. The byte checks corroborate these concrete
comparisons and bind the existing instructions to the pinned bytes; they do not
independently re-prove every historical semantic interpretation.

## QA01-D01 — Unordered pointer distance is accepted

**Classification:** CONFIRMED STATIC original-fidelity defect; **low priority**,
artificial/non-finite component-input boundary. Natural Windows reachability is
unverified; no claim this affects the supplied finite fixture in normal play.

**Location:** `reconstructed/CircleGame/Gameplay/CircleSession.cs:74`, within
`Press(Vector2,int)`. R02 rejects only when `distanceSquared > HitRadius²`.
It has no finite-pointer guard. With a NaN component, that comparison is false,
so an otherwise eligible first circle falls through to admission and judgement.
Original M24c7:0054 instead requires ordered `distanceSquared <= radius²`, which
does not accept NaN. Finite equality is accepted in both forms; those facts do
not make the two comparisons equivalent for unordered operands.

**Minimal headless component repro** (provided, NOT EXECUTED here): one circle at
(0,0), S2000, CS/OD/AR5, fresh session; call `Press((NaN,0),2000)` or
`Press((0,NaN),2000)`. All other fields are finite and no earlier circle blocks.

| Expected from E13 | Source-derived R02 consequence |
|---|---|
| Reject selection, remain Pending, judged count0 | Distance rejection not taken; `Judge` selects Hit300 and increments count |

The static test fails against this source form and the original ordered opcode
is checked directly. **Actual C# execution remains NOT RUN.** Work 2 should retain
an explicit ordered acceptance condition (for example reject when `!(d²<=R²)`),
or establish/document a finite-input contract with guards, preserving equality.
Work 1 did not edit the implementation.

## QA01-D02 — Required hitSound field is never validated

**Classification:** CONFIRMED STATIC R02 validation/documentation gap; **low
priority**. This is not asserted as a newly proven original-parser mismatch.

**Location:** `reconstructed/CircleGame/Gameplay/Beatmap.cs:33–42`. A hit-object row
must have at least five fields, but Load reads only row[0] through row[3]. The
required fifth field row[4] is never read, numerically parsed or validated.
R02's README says malformed rows are rejected; ignoring hit sounds during silent
play does not establish that arbitrary text is valid syntax for this field.

Reproducer: `fixtures/bad-hitsound.osu` contains ordinary explicit Mode/CS/OD/AR5
and `256,192,2000,1,not-a-number`. A row ending in an empty fifth field has the
same issue. The provided C# tests expect InvalidDataException. The inspected path
instead reaches a Beatmap return because all *read* fields satisfy its checks.
This is a source-derived outcome, **not an observed parser execution** here.

Work 2 can validate the mandatory numeric field while continuing to ignore its
playback effect, or explicitly document the intended permissive grammar and
revise its malformed-row promise. Do not invent extra sound-mask constraints.
E04 M28f7:149b–14ad reads this column through original M09e5, but that helper's
forgiving/error semantics were left unresolved. QA01 does **not** infer that
the original necessarily throws for this malformed row, and does not expand into
a new parser reverse-engineering campaign.

## Supported source choices and test coverage boundaries

The passing checks support these **static** conclusions on the pinned R02 source:

| Area | Verified source choices / deterministic component cases supplied | Execution level |
|---|---|---|
| Timing | Tight/middle/wide anchors80/50/20,140/100/60,200/150/100; successive strict comparisons; absolute integer error | Source and original bytes checked; C# cases NOT RUN |
| Boundaries | OD5 e49/50/51,99/100/101,149/150/151 on both sides; OD4 fixture thresholds56/108/160; fractional OD5.25 truncation | Explicit headless test expectations supplied; NOT RUN |
| Expiration | Pending at equality, Miss after+1, no repeated count after evaluation | Source strict operator checked; C# cases NOT RUN |
| Selection | Inclusive time bounds, Pending exclusion, first passing object, earlier+3 and strict400 admission | Source checked; C# cases NOT RUN; complete stable candidate/arbitration not claimed |
| Geometry | Center, cardinal signs, exact computed boundary, adjacent Singles, diagonal, NaN and infinity; CS4/5; separated visible/hit radius | Source/original comparison checked; C# cases NOT RUN |
| Mapping | Controlled640×480 scale1 and1280×960 scale2, independent letterbox translation | Source finite formula checked; C# cases NOT RUN; no displayed pixels |
| Input state | IsRepeat/HashSet guard, KeyUp removal, focus-clear/pause, stopped guard before Mouse.GetPosition, mouse-click focus only | **STATIC ONLY**; no WPF events or input-state surrogate executed |
| Reset/isolation | Fresh CircleState objects per session; view Load/Restart creates new session and resets displayed time; no direct shared mutable judgement state | Source checked; pure session C# cases NOT RUN; window/OS state unverified |
| Parsing | Explicit Mode0, version5–14, finite difficulty/range, coordinate clamp/truncation, supported type mask, sorting/equal-time/empty/size/count/time rejection | Source checked; parser C# corpus NOT RUN; D02 remains failing static check |

The headless suite links actual R02 components; it does not copy them or run a
surrogate state machine. Input *wrapper* behavior is not tested by calling pure
CircleSession methods. The physical key-to-WPF-event-to-pointer chain remains
unverified, including focus, pause/restart held-key continuity and timing cadence.

The following are disclosed simplifications, not newly filed bugs: full-list
catch-up expiration instead of original candidate attention; missing F252d-related
predecessor gate; a stored WPF-frame timestamp rather than the complete clock and
input scheduler; fixed Z/X with mouse clicks excluded from judgement; WPF DIP
letterboxing/primitives and immediate judged disappearance; simplified practice
counters and local end overlay. Maps with unusual density/overlap, original
callback exceptions, raw/manager-result flags, nonfinite original conversion
behavior, HP/mods and complete lifecycle remain outside this slice.

There is no supported conclusion about pixel identity, real hit latency, Windows
usability, audio alignment or end-to-end playable correctness. Owner-supplied
captures/logs can later be analyzed, but QA01 requests no owner-operated routine
test and does not start a capture, GUI or debugger. The immediate engineering
handoff is the two scoped findings and noninteractive suite; the missing SDK is
recorded rather than repaired under this task's restrictions.

# 3. Pseudocode

**No behavioral-model revision.** E07–E14 historical pseudocode remains unchanged.
QA01-D01 is a discrepancy with the *already recovered* ordered comparison, not
new original behavior. D02 does not resolve the original malformed-input helper.

The relevant existing E12/E13 projection remains:

```text
RECONSTRUCTED from CONFIRMED original instructions; unchanged model
selection requires original time/state gates and ordered(distance² <= radius²)
classification uses abs(int32 gameplayTime - start), then strict W300/W100/W50
timeout requires fresh gameplayTime > storedEnd + widestWindow, then evaluator
retain distinct original time reads, evaluated state and manager-result handoff
```

That projection is not implemented again as a reference game here. QA01 ends with
the source/static results and prepared headless tests; further execution remains
noninteractive component testing on a host with an already available SDK. All
interactive operation of either application remains exclusively the owner's.
