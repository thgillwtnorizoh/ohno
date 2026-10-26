# Reconstruction journal

Work 2 implementation records. Research remains owned by Work 1 under `re/`.

## R01 — Finite Transition-Progress Kernel

Date: 2026-10-26 (Asia/Bangkok). Dedicated branch:
`work2/r01-transition-kernel`, based on research commit
`586ed1d668d535533cd5822cf0fdd6790b61d5d0`.

Objective: implement only Expedition 02's finite progress projection, with no
original-software investigation. Read the full pinned contract, contexts, report,
and published progress-method evidence record. No binaries were read, no original
experiments or research tools were run, and no files under `re/` were changed.

Implementation: explicit finite phase/progress/rates/scale inputs, immutable
result, optional completion hook; phase 0 tests >=100 before min-clamped addition
and phase 1 tests <=0 before max(-1)-clamped subtraction. Completion returns phase
3/2 while preserving progress. Arithmetic crossing does not complete immediately.
Unspecified phases are rejected rather than given guessed behavior.

Evidence: `re/02-static-gameplay-initialization-and-control-flow.md`, finite
arithmetic finding/pseudocode and two validation sequences; corroborating metadata
in `re/evidence/02/static-control-flow.json` for method `0x060046a3`. Detailed
field/offset traceability, API policies and Work 1 requests are in
`R01-transition-kernel.md`. Confidence is inherited CONFIRMED static projection;
original dynamic equivalence is UNRESOLVED. No hypothesis was promoted.

Files added: `transition_progress.py`, `tests/test_transition_progress.py`,
`fixtures/r01-finite-cases.json`, `R01-transition-kernel.md`, this journal, all
under `reconstructed/`. No original assets or code included.

Actual validation on Python 3.12.14/Linux:
`PYTHONDONTWRITEBYTECODE=1 python -m unittest discover -s reconstructed/tests -v`
passed 12 test methods including 22 explicit static-contract fixture cases.
The tests are implementation validation against Work 1's branch-derived oracle
and explicit API policies, not original-game observations. No dynamic gameplay or
pixel comparison ran. Diff/ownership and remote publication checks are reported
in the final handoff; their success is not assumed here.

Limitations: no default state, full phase lifecycle, scheduler, clock units,
rendering, mode/commit effects or original callback semantics. Nonfinite inputs
and unbounded overflow are outside the API; bit-level CLR equivalence is untested.
Next dependency: Work 1 must specify lifecycle, timing/rate origins, delegate and
side-effect ordering, plus original-run reference traces before host integration.

User scope explicitly places R01 documentation in `reconstructed/` and prohibits
editing `re/`; therefore no shared research journal update or new numbered
reverse-engineering mark is made. No other contract exception is used.
