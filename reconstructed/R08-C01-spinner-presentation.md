# 1. Result

R08-C01, Work2, 2026-11-03 Asia/Bangkok. Started verified main
`4c61be63265d54b19b257fb3f1c632760c9e7773` including E034. C01 was absent;
the owner explicitly authorized completing C01 first and then C02, with separate
reports/commits. Read AGENTS, full standing contract, journal/R08, E033/E034 and
existing spinner/slider input and rendering sources. No original binary/tool,
window, gameplay, OS input, recording or E20 execution.

Corrected premature spinner presentation: renderer consumes a read-only
SpinnerPresentation policy rather than reusing circle approach time. Spinner
appears at its stored start and remains visible pending its completion decision.
This is an ENGINEERING presentation correction, not a recovered native fade rule.
No arbitrary early-appearance constant, input interception or gameplay/scoring
change. Source inspection plus four bounded component groups examine visibility
and preceding-slider continuity. Windows CI status initially PENDING; actual
source/run/output will be recorded after execution. No local dotnet SDK.

Changed only reconstructed/ renderer/helper, bounded C01 checks, active README,
this report and appended journal, plus minimal existing CI step/path trigger.
Historical reports, research/QA/E20 and all fixtures preserved. Publication to main
uses an expected-head lease, no force. C02 follows only after this gate passes.

# 2. What they found

The old renderer displayed a pending spinner from `start - game.Approach`, with
an opaque filled spinner body and active instructions. Spinner approaches were
not established by E08's circle-specific timing. Removing that borrowed preview
prevents a future spinner obscuring/competing with an unfinished preceding slider.
Original spinner first-opacity/native transforms remain UNRESOLVED.

The reported preceding-slider input interruption is OWNER-REPORTED, not reproduced
by a Work-operated window. Source confirms Frame continues sampling real held Z/X
and the same pointer for sliders; presentation draws no controls or input handlers.
CircleSession spinner sampling and its head-press exclusion start at spinner start,
not its approach interval. Track continues separately. Parser disallows overlapping
spinner/object intervals. No source evidence justifies altering those mechanics.

Bounded calls verify: no spinner before start, inclusive start/end visibility,
continued visibility if post-end completion has not run, removal after evaluation;
a held slider from8000 to9600 survives old preview8800 with timely terminal,
unchanged head/aggregate300, combo2 and engineering K1 score372; a slider head8900
inside the old preview can still be admitted before spinner10000; visibility
queries do not mutate held/tracking/motion/ledger state. These are implementation
checks, not physical input or original-game observations. Native/UI focus loss,
actual owner cursor trajectory and frame scheduling remain UNVERIFIED. No claim
that the reported interruption's runtime cause has been conclusively identified.

# 3. Pseudocode

```text
ENGINEERING draw policy:
    visible = spinner result pending AND current time >= stored spinner start
    draw primitives only when visible
    do not consult circle approach or modify gameplay state

UNCHANGED original-derived gameplay projection:
    sample real input through existing frame path
    track preceding slider using existing held/history/radius/checkpoint rules
    spinner angular sampling begins at its start
    integrate only strictly inside spinner interval; complete strictly after end
    existing once-only judgement -> E33 ledger
```

No guessed native fade schedule, score adjustment or input workaround.
