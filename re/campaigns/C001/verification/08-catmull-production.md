# 1. Result

| UEP-001 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | ohno / C001 / Work1 / Issue#3 C production reference verification |
| Status | VERIFIED CHECKPOINT — reference-based component reproduction; separate-reviewer authorship NOT VERIFIED |
| Starting SHA | d7572749011bf10bf82c3fe42377ee793f035e58; latest main verified |
| Exact tested SHA | 2d776dac175c608e213dbd523c9d237c9411ec8c, detached exact-source worktree |
| Published SHA | Introducing commit in Git history and issue handoff |
| Objective / Profile / Changes | Verify finite ordinary C with unchanged c353b83 research/19fixture manifest; new QA helper/results only, no production edits |
| Evidence / Confidence | Original-static authority reused, no extraction. Independent pre-production expectations exercised against production; VERIFIED COMPONENT. Reviewer continuity disclosed below |
| Tests / Environment | Linux SDK8.0.427/runtime8.0.33;5782/5782 assertions PASS,13 admitted/6 engineering rejections;retained independent P1093/1093PASS;19fixture regeneration PASS |
| CI / Artifact | Existing exact-source43595153711/job132447966714 corroborated SUCCESS:2309named historical QA/0fail,0compiler warnings/errors;C4375,P299,C001264,stack43+394,finite106+4PASS. Existing artifact14113958938, not re-downloaded |
| NOT RUN | New QA helper on Windows; new full Windows regression; original CLR/client, GUI/audio/physical input, owner qualification; independent reviewer authorship |
| Coverage / Remaining | Finite fixture geometry and consumer reference checks pass; reviewer-independent acceptance, original runtime and excluded compatibility/exceptional/zero scheduling remain open |
| Discrepancies / Limits / Blockers | No finite implementation discrepancy found. Six engineering exclusions are not original equivalence. Session previously implemented C, so no separate-author review claim |
| Updated records / Reproduction | This report, catmull/Independent and qa-2d776dac; append-only journal/status/coverage/dependency/discrepancy records; commands below |
| Next task / Actor / Dependencies | Headquarters arranges a fresh independent reviewer for authorship separation and owner runtime qualification as required |
| Subsystem | OPEN |

# 2. What they found

Source review matches recovered asymmetric p0/p3 stencils, Single polynomial order,
t2/t3, separate j/50 and(j+1)/50 evaluations,50 pairs per retained adjacent control
span, duplicates and last-piece flags. No C fallback/B separator, endpoint forcing
or new modern interpolation library. Shared length adjustment/cumulative output
and per-segment clocks consume the resulting pieces unchanged.

New verifier never implements Catmull interpolation. It reads unchanged pre-existing
Work1 raw/prepared pairs, flags, controls and cumulative predictions. Every pair is
compared at the research1e-4 tolerance; stencils/operation order are additionally
source-reviewed. Precision diagnostic is excluded rather than treated as an
original numerical oracle. Every movement is checked for forward/reverse endpoint
selection and the established E005 integer time accumulation contract. Checkpoint
positions and sound schedule preserve stored producer order, including inherited
nonmonotone final schedules; no all-checkpoints-success assumption is introduced.

Actual sessions judge C heads and emit sound events with the same parent identity.
Fresh health/NewCombo/resource and session references are checked. Reloaded movements
remain stable. Additional positive/negative C stack probes compare fixed original-
derived displacement, unchanged raw controls/segments, translated render/input
segments and checkpoint time/position/resource coherence. Render source still uses
Geometry.Segments and BallPosition; this is data/source verification, not pixels.

Six observed exclusions: all-coincident,singleton,bare-marker,vanishing-request,
precision-diagnostic,malformed-coordinate. Actual rejection messages are recorded.
Their engineering admission is not evidence of original exceptions, empty schedules
or x86 behavior. Full parameter/legacy/editor/modifier/mode compatibility remains
NOT VERIFIED. Existing P1093 checks pass locally; complete historical L/B/P/stacking
Windows evidence corroborated without another full gate. No R17/JoinedBezier change.

Independence limitation: this same session previously authored Work2 C production.
The expected corpus predates that implementation, and the new verifier does not copy
its polynomial or use Work2 result packets as an oracle. This establishes independent
expectation-based reproducibility, but not independent reviewer authorship. A fresh
reviewer is still required where that separation is an acceptance gate. No owner or
complete C001 acceptance is claimed.

The SDK was absent after environment refresh; restored8.0.427 before executing.
No original extraction or historical expectation adjustment was necessary.

# 3. Pseudocode and reproduction

```sh
# In exact2d776dac checkout, copy this new Independent helper to the same path:
dotnet run --project re/campaigns/C001/catmull/Independent/C001.CatmullIndependent.csproj -c Release -- "$PWD" /tmp/c-qa.json
dotnet run --project re/campaigns/C001/perfect-circle/Independent/C001.PerfectIndependent.csproj -c Release -- "$PWD" /tmp/p-qa.json
python re/campaigns/C001/catmull/build_fixtures.py --check
```

Results: catmull/qa-2d776dac/results.json.gz.b64 (base64+gzip JSON), summary with
SHA256, stdout and selected primary Windows log lines/counts. Full Windows raw log
remains on GitHub. Helper links actual exact-checkout production source.

```text
read immutable fixture bytes/expected pairs → load actual canonical production
compare kinds/normalized controls/pairs/flags/lengths/prepared endpoint
check existing traversal clocks/repeats/checkpoint order and actual identities
exercise sessions and source-preserving stack translations
record admissions separately from engineering exclusions
```
