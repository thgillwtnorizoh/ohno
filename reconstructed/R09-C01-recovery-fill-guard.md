# 1. Result

R09-C01 — Recovery Fill Threshold Correction, Work2, 2026-11-03 Asia/Bangkok.
Started verified latest origin/main `34249b9895f0d0ebabbeb6436d3ab37960c88857`.
Read canonical rules/standing contract (repository/attachment SHA256 agree),
reconstruction journal, R09/E035 and QA07 report/results/preserved failure evidence.
No original binary/tool/game/window/OS input/debugger/OBS or E20 execution.

Corrected only QA07-D01's production guard in OrdinaryHealth.BeginRecovery:
State.Add and State.ConfigureFill now both occur only when H<160. Recovery
activation/count decrement remain outside. Existing missing-count rejection,
fill-from operand and unresolved native playback boundary are unchanged.

Changes limited to Gameplay/CircleSession.cs, one added focused group in existing
R09DevelopmentChecks/Program.cs, this report and append-only reconstruction journal.
No workflow change is needed: the current Windows gate already includes QA07 and
all earlier suites/checks. All original R09 assertions are retained; no Work1
research/QA assertion/results/log, fixture, original R09 report, score/calibration/
drain/smoothing/routing/UI code or E034 multiplier is changed.

Initial verification: inspected diff/whitespace and hash-verified preserved QA07
failure logs. No local dotnet SDK/build. Full Windows gate PENDING until actual
execution; tested source SHA/run/output will be recorded here. QA07-D01 is
source-corrected, with component-verification disposition pending the159-case gate.
Publication uses main/expected-head checks, no permanent branch/force/history
rewrite. Scope is targeted R09 correction only; no R10 or original expedition.

# 2. What they found

**CONFIRMED STATIC authority:** E035 MethodDef06000acc, owner02000186,
signature20010102, body SHA256
`a4e4cbd7c8f1050a3fa5bdd984e8a781042b9d728b4058ef7f3f26c369a3e523`.
QA07's retained `original-recovery-guard.json` cites E035 evidence SHA256
`ce876493b80f6a396d3d8bac051a3484caf93cd25ccbdd0d00042bc2a14f9687`.
CIL0014 bge.un.s→006a skips Add0025, fill0031 and fillRate0045 together;
recovery006e and count00a1 follow the guarded block. Work2 consumed this published
evidence and conducted no new original-binary investigation.

Before: H>=160 preserved H/A but unconditionally enabled fill and replaced its
slope. After: H>=160 retains prior fill flag, slope, from, H/A; recovery still
activates and consumes one count. H<160 behavior remains Add160 (A uncapped/H capped)
and original recovery fill. All five finite controls are explicit direct calls,
including positive H; they are not weakened by CheckFailure's earlier positive-H
return or the ordinary no-mod UI's zero recovery count.

| Initial H/A | Final H | Final A | Fill / slope / from |
|---|---:|---:|---|
|0/0|160|160|enabled / .07999999821186066 / prior from |
|100/100|200|260|enabled / original recovery slope / prior from |
|159.999/159.999|200|319.999 approximately|enabled / original recovery slope / prior from |
|160/160|160|160|all prior fill settings retained |
|200/200|200|200|all prior fill settings retained |

Focused Work2 group checks those five H values with both initial fill flags and
both existing failed-latch states (20 combinations). It retains V and prepared
r/B/G, activates recovery, consumes count1→0 exactly once and verifies a subsequent
missing-count call still rejects. A seeded actual session ledger remains unchanged:
score/current/max/snapshot combo, four hit counts, accuracy, event count and last
source event; no HP result notifications or completed objects are manufactured.
These are finite component controls, not human gameplay or a new QA campaign.

**Historical evidence preserved:** QA07 expanded source
`8185478d4a1e064a3e74dbee82c8dbc8907b9187`,
[run41671715769/job126041531509](https://github.com/thgillwtnorizoh/ohno/actions/runs/41671715769/job/126041531509),
157 PASS/2 FAIL/159 executed. The two failures are F13 H160/H200. Original
initial-gate-output.txt SHA256ad7ecfdd2eb04dedbe2d579ed6d86e391249fd6c4ca59b9cec15efacf6db52ae
and gate-output.txt SHA2563e0c0f6057e3235dbcd1be8ec5b68b694a0a46e2273841221a1f5a09681a2585
were checked unchanged. Existing results.json remains a historical red-run record;
the correction's green gate will be recorded separately, not written over it.

**RECONSTRUCTED:** the corrected bounded branch. **VERIFIED COMPONENT:** only the
actual Windows executions recorded in Result. **ENGINEERING/UNVERIFIED:** explicit
recovery-count API, private calibration and host projections, diagnostic failed
latch/practice continuation, native playback/cleanup/recovery scheduling and all
original runtime/pixel/physical-input equivalence remain R09's stated boundaries.
No conventional full recovery/failure UI or deferred score discrepancy fix follows.

# 3. Pseudocode

```text
RECONSTRUCTED E035 bounded recovery entry:
    require existing recoveryCount > 0
    if H < 160:
        Add160 using original uncapped-A/capped-H ordering
        enable fill; preserve fillFrom; set original recovery fillRate
    recovering = true
    recoveryCount -= 1
    retain unresolved native playback/callback boundary

H >= 160:
    retain H/A/V, fill flag/slope/from and coefficients
    still activate recovery and consume one count
    emit no score/accuracy/combo or judgement notification
```
