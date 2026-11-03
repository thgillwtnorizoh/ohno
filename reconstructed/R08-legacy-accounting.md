# 1. Result

R08 — Legacy Score, Accuracy & Results Accounting. Work2,
2026-11-03 (Asia/Ho_Chi_Minh). Started latest verified origin/main at
`37e35c0ca214aa0fc7f5de1da80f50ec26b62b23`. Read AGENTS/standing contract,
implementation journal/R07, E33 in full, relevant component/completion boundaries
and QA01–QA05 contracts. Repository/attached contract hashes agree. No original
binary inspection, expedition, runtime game or debugger.

Implemented a dedicated public ScoreLedger component, explicit ScoreConfiguration,
and provenance-bearing ScoreSource/ScoreEvent interface under Gameplay. They are
separate classes in CircleSession.cs so historical headless projects can continue
linking the actual model without modifying their source/project files. No UI
dependency or copied accounting implementation. Session current/max combo and
accuracy now delegate to this single ledger. HitValueTotal is a compatibility
weighted-hit getter, not numeric score; JudgedCount remains completed-object state.

Connected circles, slider heads/checkpoints/aggregate and actual spinner progress/
final events. Live HUD and results show legacy score, Single-derived percentage,
whole-result counts, combo/max, completion count and policy/K provenance. Default
UI construction explicitly supplies **K1.0 ENGINEERING preset**, not original map
preparation. Restart/replacement construct fresh records while retaining config.
No hit-object, geometry, input, clock or audio overhaul. Old fixtures/buttons remain.

Changes: reconstructed/CircleGame/Gameplay/CircleSession.cs, Program.cs,
Rendering/PlayfieldView.cs, active README, R08DevelopmentChecks, this report and
appended reconstruction journal. Only shared CI change is one step/path trigger
in the established Windows gate. Work1 research/QA01–05/E20 and historical reports/
journal content are preserved. No new fixture is necessary; all existing no-mod
circle/slider/spinner controls exercise the record. Windows run commands unchanged.

Initial gate [run41592125988/job125771010001](https://github.com/thgillwtnorizoh/ohno/actions/runs/41592125988/job/125771010001)
on source `b17c32cdc0eb1700c4686023e29ccbee3908f533` passed: WPF0 warnings/errors,
all255 existing QA cases, retained R06/R07 checks and13 R08 groups. Source review
then separated E33's captured hit-counter flag from its exact late admission OR
predicate, adding the supported negative/disabled-flag control.

Final tested source: `d09906f5689b78cfd2ebc5ab9aea8ef9dc371fd3`.
[Run41592942636/job125773612554](https://github.com/thgillwtnorizoh/ohno/actions/runs/41592942636/job/125773612554)
completed successfully on Windows2022 using preinstalled SDK8.0.427, on
2026-11-03 at06:55–06:56 UTC (13:55–13:56 Asia/Ho_Chi_Minh). Actual output:

```text
Build succeeded. 0 Warning(s), 0 Error(s)
HEADLESS COMPONENT TESTS: 90 passed, 0 failed, 90 executed
QA03 SLIDER TESTS: 56 passed, 0 failed, 56 executed
R06 PREPARATION TARGETS: 4 passed (static research oracle, not original runtime)
R06 FIXTURE LOADS: 4 completed (parser/preparation only)
QA04 GEOMETRY TESTS: 43 passed, 0 failed, 43 executed
R07 DEVELOPMENT CHECKS: 8 passed (finite static-oracle/component checks; no human gameplay)
QA05 SPINNER TESTS: 66 passed, 0 failed, 66 executed
R08 DEVELOPMENT CHECKS: 13 passed (E33 finite controls; no original runtime/human play)
```

The established workflow executed these commands in order (PowerShell repo root;
`$env:GITHUB_WORKSPACE` is the checkout directory):

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa03/Headless/QA03.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R06PreparationChecks/R06PreparationChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa04/Headless/QA04.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R07DevelopmentChecks/R07DevelopmentChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa05/Headless/QA05.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R08DevelopmentChecks/R08DevelopmentChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

All steps succeeded. Checkout emitted a Node20 deprecation advisory, separate from
compiler diagnostics. No historical assertion conflicted with the E33 parent-miss
reset. Final publication changes only this report and the appended journal after
the tested source; application, checks and CI remain byte-identical to that source.
No local dotnet executable, SDK install or compilation.
No game launch, OS input, gameplay, OBS/recording, original debugger, E20 experiment
or routine supervised testing. Earlier owner feedback is positive scoped evidence
for R04/R06/R07; R08 score numbers and visuals remain **UNVERIFIED human/runtime**.
No permanent branch or force push. Stop after R08; no QA06 or new expedition.

# 2. What they found

## E33 translation and mutation order

**CONFIRMED STATIC** is Work1's original instruction evidence. The implementation
is the **RECONSTRUCTED** finite plain no-mod/base-record/null-ScoreV2 projection.
It does not create new original-software claims.

ScoreLedger maintains Int32 numeric score/current/max combo; UInt16 combo snapshot,
n300/n100/n50/nMiss; Single accuracy fraction; explicit Double K. LastEvent,
EventCount and LastOutcome are small ENGINEERING audit views, not original record
fields. ScoreSource identifies the session-local object ID/family/start/end;
ScoreEvent adds ordinal, one of seven provenance categories, original-derived raw,
captured accounting flag, Slider2 flag, policy and optional checkpoint producer role.

Supported amounts follow E33 M0ad2 exactly: whole1024/512/256 give base300/100/50,
whole-131072 gives0;64/128 give30;8 gives10;-262144/-524288 give0;
spinner4096/8192/16384 give0/100/1100. Raw0 returns before any accounting event.
Whole-positive scaling alone computes:

```text
b + trunc(Double(Max(0, unchecked(oldCombo-1))) * Double(Int32Div(b,25)) * K)
```

On admitted finite inputs the mutation order is conditional UInt16 whole counter,
base amount, delta from old current combo, combo/max/snapshot, then admitted
unchecked Int32 score addition. Counts and snapshot cast explicitly to UInt16;
score/combo operations retain unchecked Int32 behavior. Maximum combo never supplies
the bonus operand. Integer b/25 precedes Double multiplication. No bonus applies
to components, even spinner8192's base100.

AccountingEnabled=false suppresses whole counters but does not silently suppress
known combo work. The late write separately admits `flag || (raw &8388608)>0` per
E33-A. Supported negative raw identities carry that bit and admit a zero-delta
legacy write even with flagfalse; they do not gain hit counts. Positive plain
flagfalse results skip that write. Gameplay emits true for its fresh ordinary
objects (E33 M249b default); the public event contract carries the flag explicitly
for component controls/future recovered sources. Additional decorated identities
are unsupported, not guessed. No HP/auxiliary accumulator/native observer effects.

Positive circle/final spinner/head/component increments current/max/snapshot;
ordinary negatives reset current/snapshot, retaining maximum. Terminal-524288 and
spinner progress bypass combo mutation. Positive slider parent bypasses duplicate
combo credit, but a **parent whole MISS resets combo** per E33. This corrects R07's
blanket aggregate bypass. A bounded naturally parsed short-slider control checks
the meaningful case: earlier circle combo1, pending head, terminal failure then
parent MISS at3010, before head timeout; combo resets immediately. Historical QA
assertions are unchanged. Their actual result is recorded by the hosted gate;
any conflict must be disclosed rather than rewriting Work1 tests.

Accuracy uses only four whole counters: total=n50+n100+n300+nMiss; empty<=0→1f;
otherwise Single(50*n50+100*n100+300*n300)/Single(300*total). The second total read
remains separate in code. Percent is fraction*100f, formatted to two decimals in
the UI. Components do not become accuracy hits. JudgedCount is a separate runtime
completed-object count; ordinary enabled records should have equal admitted totals
under the supported map bounds. UInt16 wrapping is not replaced with saturation.

## Actual lifecycle integration

- Circle result guards emit exactly one plain whole raw and completed-object event.
- Slider head emits64 or-262144, independently of stored head quality. Checkpoint
  accounting follows the existing outcome update and pending-count leave. Producer
  role is retained separately; actual raw comes from the unchanged due-prefix
  selector. An overdue interior can therefore add30 via64/128. Repeated-time calls
  can process distinct outstanding slots. Unprocessed backlog creates no score.
- Parent aggregate emits one whole event after existing completion/ratio logic.
  It can score a combo-scaled300 while leaving combo unchanged; parent failure is
  the E33 reset exception above. A later missing-head timeout remains a distinct
  component failure, not another whole accuracy miss.
- Spinner emits only the raw returned by an actual reached SampleCursor call.
  Qualifier-skipped frames do not replay LastComponentRaw. C1/raw0 can advance
  observation state without accounting; actual4096/8192/16384 events have their
  own amounts and no combo/count inflation. Completion emits final raw once and
  does not observe/drain progress. Numeric1100 for16384 is the E33 ledger addition,
  not a reconstructed complete bonus-payout/effects implementation.
- Ledger Apply expects ordered, once-only lifecycle emissions. It does not use a
  timestamp deduplication set that could discard valid same-time checkpoint slots.
  Existing result/processed/evaluated guards enforce ownership of those emissions;
  bounded checks inspect actual EventCount/Source/ordinal/role/raw at the boundary.

## Explicit engineering policy and unresolved inputs

K must be finite0–10 with a nonempty provenance label (safety bounds, not original
rules). UI passes named K1 ENGINEERING preset explicitly. The historical
one-argument CircleSession constructor delegates to that same named preset only
to preserve existing callers/tests. New scoring callers can supply any supported
configuration. Pause retains the ledger; fresh construction resets all fields.

E33 M1bc4's active F0ff2/F0fe5 loading/cache operands remain unresolved. No object-
density formula or automatic original K is invented. This is the exact remaining
Work1 dependency for automatic chart preparation. E33 also distinguishes handler
and global HUD record references; this reconstruction intentionally uses one
session-owned ledger with no external record swaps/opaque callbacks.

Only plain raw identities are accepted: no masking/normalization of decorated
results. Unsupported raw/provenance/type flags or ScoreV2 mark accounting
unavailable, retaining the prior supported prefix for inspection. Further numeric
updates stop until a fresh ledger; UI hides prefix metrics as final values. This
is ENGINEERING unsupported-state handling, not an emulation of those policies.
Bonus conversion outside finite Int32 range is similarly unsupported after earlier
counter/base writes; no target-CLR exceptional conversion or rollback is invented.
Original helper interruption/reentrancy is not modeled as an atomic transaction.

Existing engine judgements emit the explicitly selected plain event family; original
manager/group/high-flag decoration is not recreated. Even with supplied K, general
original cross-map score equivalence, actual record selection/native precision,
HUD pixels and human score comparison remain unverified. ScoreV2/modifier scoring,
HP/fail, rank letters, online records/PP, replay/persistence and audio/graphics
overhauls remain outside R08.

## Bounded controls and Windows use

R08DevelopmentChecks links the real ledger/session. Seven E33 reference sequences
cover600,400,700+trunc(4K),300,360+trunc(12K),160,1100, with several explicit K
values. Additional bounded groups cover Single accuracy/empty state; base8/64/128
amounts; terminal bypass and disabled admission; old vs maximum combo; raw0 and
invalid K/policy/flags; actual slider/spinner event boundaries; fresh state and the
short-slider parent-miss correction. Thirteen groups, not a QA06 campaign or a
replacement scoring model. Restart/replacement UI wiring is source-inspected;
fresh-session reset is executed at component level. No WPF window is operated.

Use existing Windows commands in CircleGame/README.md. All old fixture buttons
remain, with visible score/policy/K. Eight all300s on the old circle fixture predict
2652 at K1; this is a conditional ledger prediction, not an observed stable-map
score. Actual hosted outcomes are recorded separately in Result/journal.

# 3. Pseudocode

**RECONSTRUCTED E33 finite accounting; ENGINEERING source/configuration boundaries:**

```text
existing judgement/component lifecycle reaches one result:
    retain source object/slot/kind/producer role, actual raw and captured flags
    if raw0: emit no accounting event
    unsupported policy/raw/provenance -> explicit unavailable status
    conditionally increment whole UInt16 hit counter
    retain base amount b separately from object judgement
    calculate delta using old CURRENT combo, integer b/25, explicit K, truncation
    perform branch-specific combo increment/max/snapshot, reset or bypass
    if record admitted: score=unchecked(score+delta)
    derive Single accuracy from whole counters only

HUD/results:
    read ledger score/accuracy/counts/combo/max and runtime completed-object count
    show explicit policy and engineering K provenance; hide unsupported metrics
    restart/replacement -> fresh ledger with supplied configuration
```

**UNVERIFIED:** automatic original K/density preparation, decorations/ScoreV2,
native/helper/record interleavings and human/visual/cross-map equivalence.
