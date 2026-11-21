# Finite Work2 handoff — canonical positive 1–32 traversals

Proposal for Headquarters review; implementation separately authorized.
Actual accepted source22a403505f13461477343b9c6a251980716dd90f has two1–16 guards:
Beatmap.LoadCore and SliderSoundMetadata.Parse. LocalChartCatalog.Index does not
validate traversal count; AuditRunner labels traversal-related failures broadly.
Do not remove the classifier or treat its64 owner labels as >16-only failures.

| Input / path | Current | Proposed finite contract |
|---|---|---|
| Canonical positive1–16, existing supported L/B/P/C/version/timing profiles | Existing | Preserve geometry, schedules, samples, consumers and errors |
| Canonical positive17–32 within existing work/time/profile bounds | Count refusal | Admit through explicit canonical metadata opt-in |
| Historical string Beatmap.Load and existing public metadata Parse |1–16 | Preserve unchanged unless separately scoped |
|33 and larger | Refuse | Retain ENGINEERING exclusion, not original cliff |
| Zero,negative,fraction,overflow,empty,nonnumeric | Refuse | Retain precise typed refusal; do not globally clamp |
| Duration,geometry,checkpoint,map work,malformed samples | Existing separate limits/errors | Preserve; earlier bounded allocation allowed with stable diagnostics |
|9000/9001 | Not executed | Static-only parser observations; no admission requirement |

Implement one canonical profile parameter/path shared by canonical count and
metadata admission, leaving historical callers at1–16. Keep raw integer count
separate from original effective Max(1,raw); positive finite profile makes them
equal. Edge arrays remain raw+1, empty-list/simple semantics unchanged. Do not
interpret slider heads as extra circles or alter Route M/Z,stacking,R17 or timing.

Before repeated movement expansion, compute checked Int64 `R * segmentCount`
and reject against remaining map250000 movement budget. At32*8192=262144,
existing per-geometry8192 segments alone is insufficient; existing map checking
happens after expansion. Preserve segment100000,map-checkpoint100000,
per-slider4096,geometry/subdivision and health budgets. A conservative checkpoint
preflight bound `R*(ceil(D/q)+2)` may be used only after proving q's effective
scheduling meaning, positivity and suppression cases from actual source; do not
introduce new exclusions within the supported1–16 profile merely because an
upper bound is loose. Prefer bounded incremental checkpoint construction with
remaining map/per-slider counters and deterministic diagnostics. Check token at
traversal,segment and tick-loop boundaries; validate cancellation before returning
any prepared object or resource owner. Dispose cancelled resource preparations.
No algorithmic reordering, per-span rounding, pending-checkpoint draining or
sorting of moved final deadlines is justified.

Original-backed numerical contract: use existing prepared Single segment distance,
Single1000*distance then Double division by start-selected velocity, accumulated
absolute Double time and final conv.i4 truncation. Retain separate checkpoint
phase conversion; late timing does not silently resample travel velocity. End
parity uses traversal count, sound/result membership uses integer floor N/R and
existing modulo/ordering. Subset need not have R-1 entries. Preserve terminal
Max(midpoint,last-36) behavior and existing parent completion timing.

Future development acceptance: replay all106 hashes individually. Exact candidate
set is `futureCandidate` in manifest:86 canonical preparations/20 retained
exclusions, zero losses of41 previous admissions;45 newly prepared cases. This
includes malformed sample metadata that remains audio-unavailable and must not be
counted ready. Compare all original expectations and baseline per-fixture schedules,
Single distances,endpoints,sample queries,N/R subsets,result ratios and pending
counts. Test complete fresh canonical score/health/NewCombo/session/audio owner
connections for newly admitted maps, not synthetic component ownership.

Add independently reviewable stress fixtures near actual movement/checkpoint/map
budgets (including repeated max-segment geometry), overflow arithmetic, monotone
and nonmonotone deadlines, pre-cancel and deterministic in-loop cancellation,
resource disposal and multi-slider aggregate limits. No9000-count execution needed.
Retain R33,syntax,zero/negative,duration,work,sample and unsupported-path controls.
Preserve historical R12DevelopmentChecks S25 and QA10 M16 old string-loading17
refusals. Run focused development tests and full historical regression, clean exact
Windows build/package, source marker and hashes. Subsequent Work1 must independently
test the final exact commit and actual linked binaries; do not use this component
probe as a substitute. Original runtime equivalence remains unverified.
