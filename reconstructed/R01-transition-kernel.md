# 1. Result

R01 — Finite Transition-Progress Kernel. Work 2, 2026-10-26 (Asia/Bangkok).

Objective: independently implement only the finite phase-0/phase-1 projection
supplied by Work 1 Expedition 02. This is a reconstruction milestone, not another
numbered reverse-engineering expedition or a complete transition controller.

## Source and execution

- `transition_progress.py`: dependency-free Python 3.10+ component.
- `tests/test_transition_progress.py`: contract and API-policy tests.
- `fixtures/r01-finite-cases.json`: 22 explicitly expected cases derived from the
  documented branches, including the two Work 1 crossing/completion sequences.
  These are Work 2 test vectors, not measured original-game fixtures.
- `contexts.md`: separate implementation journal. No files under `re/` changed.

Run from repository root:

```sh
PYTHONDONTWRITEBYTECODE=1 python -m unittest discover -s reconstructed/tests -v
```

Actual result on Python 3.12.14/Linux: **12 test methods passed**, including all
22 fixture cases. Coverage includes threshold equality and beyond-threshold
inputs, adjacent floats, fractional steps, exact arrival and overshoot, both
one-sided clamps, zero and negative rates/scales, inactive-rate independence,
two-invocation sequences, completion payload/count, absent callback, unsupported
phases, nonfinite/type rejection and documented overflow/exception policies.
No reference software or reverse-engineering tools were run. No differential
gameplay, scheduling, frame, or rendering comparison was performed.

User-directed scope takes precedence over general expedition journal placement:
all R01 records stay in `reconstructed/`; `re/contexts.md` and all historical
research remain untouched. No other contract exception is used.

# 2. What they found

## Evidence and confidence

Pinned research commit: `586ed1d668d535533cd5822cf0fdd6790b61d5d0`.
Read the complete `re/Instruction.md`, `re/contexts.md`, and
`re/02-static-gameplay-initialization-and-control-flow.md` at that revision.
The report's “CONFIRMED (finite-value progress arithmetic)” passage, finite
pseudocode, and validation table are the behavior oracle. CONFIRMED here means
**Work 1 static evidence**, not observed original execution. Semantic aliases
remain RECONSTRUCTED. Presentation as a fade is not assumed.

Work 1's published `re/evidence/02/static-control-flow.json`,
`selected_methods["0x60046a3"]`, supplies cross-reference provenance:

| Component rule | Work 1 evidence |
|---|---|
| Phase/progress inputs | fields `0x04002c6b` / `0x04002c6a` |
| Incoming completion | phase write at code offset 136; callback field `0x04002c76`, Invoke at 158 |
| Incoming arithmetic | rate `0x04002c7f`, scale `0x04002c22`; Math.Max at 194; progress write at 199 |
| Outgoing completion | phase write at 236; callback field `0x04002c77`, Invoke at 258 |
| Outgoing arithmetic | rate `0x04002c7e`, scale `0x04002c22`; Math.Min at 294; progress write at 299 |
| Threshold-before-arithmetic order and constants | Expedition 02 prose and pseudocode; not independently re-derived by Work 2 |

Offsets are Work 1's decimal code offsets excluding method headers, scoped to its
pinned build. Work 2 read the published JSON, not original method bodies.
No new claim about original internals is made.

## Supported contract

All phase, progress, both rates and update scale inputs are required. There are
no initial-state, rate, timing-unit or scheduling defaults. Input phase 0 may
produce phase 0 or 3; input phase 1 may produce phase 1 or 2. On a threshold
branch, progress is preserved even if already beyond the threshold. Arithmetic
arrival/crossing retains its input phase and emits no completion that invocation.
Only the documented bound is applied: no outgoing lower bound or incoming upper
bound. Rates/scale are not restricted to nonnegative values.

The immutable `StepResult` contains phase, progress and optional `Completion`.
An optional single `on_complete(result)` hook runs once per completing invocation,
after result construction. Its payload is an engineering interface, not a clone
of the original EventHandler sender/arguments. The kernel has no mutable global
state. Callers must retain returned state; re-submitting an old completing input
can emit completion again. No cross-invocation exactly-once guarantee is claimed.
Returned phases 2/3 are outputs only; their update behavior is outside R01.

Example (explicit caller-supplied rates and scale):

```python
from reconstructed.transition_progress import step

events = []
first = step(phase=0, progress=99, outgoing_rate=10, incoming_rate=10,
             update_scale=1, on_complete=events.append)
# first: phase=0, progress=100, no completion; events remains empty
second = step(phase=first.phase, progress=first.progress, outgoing_rate=10,
              incoming_rate=10, update_scale=1, on_complete=events.append)
# second: phase=3, progress=100, OUTGOING; events contains second once
```

## Engineering policies, limitations and evidence requests

Engineering policies are tested separately from behavioral predictions:

- Accept Python `int`/`float`, converting to finite Python float; reject booleans,
  other types, NaN/infinity and integers too large for finite conversion.
  Validate all numeric inputs, including the inactive rate, before dispatch.
- Reject unsupported input phases with ValueError; do not assume they are no-ops.
- Use separate Python float multiplication then addition/subtraction; no fused
  arithmetic or bit-identical CLR/JIT claim. Selected fixture arithmetic is
  exactly representable, so comparisons use exact equality without tolerances.
- Overflow that the documented clamp bounds produces that bound. An unbounded
  nonfinite result raises OverflowError before any completion hook. This is an
  API safety policy, not recovered original behavior.
- Invalid hook type raises TypeError. A hook's exception propagates without
  retries; the function owns no caller state to roll back. This is not a claim
  about original delegate exceptions, reentrancy or threading.

UNRESOLVED requests for Work 1 before integrating a host controller:

1. Supply initial/reset values and phase lifecycle, including phases 2/3 and
   transitions between outgoing/incoming updates.
2. Specify scheduling, clock/update-scale source and units, and rate origins.
3. Supply completion delegate arguments, ordering relative to omitted side
   effects, and exception/reentrancy/thread requirements.
4. Provide original-run input/output traces for both crossing sequences and
   boundary cases; characterize overflow/nonfinite and numeric precision only
   if that domain is required.

Mode-request guards, constructor selection, commit dispatch, rendering, clocks,
other original progress side effects and all gameplay rules are excluded. No
HYPOTHESIS is used as an implementation rule. A failing implementation test
would indicate a contract/API discrepancy, not establish that Work 1 is wrong.

# 3. Pseudocode

CONFIRMED [Work 1 static finite projection; original execution unvalidated]:

```text
require input phase 0 or 1 and caller-supplied finite numbers [API policy]

if phase == 0:
    if progress >= 100:
        result = (phase 3, unchanged progress, outgoing completion)
    else:
        result = (phase 0, min(100, progress + outgoing_rate * update_scale), none)
else:  # input phase 1
    if progress <= 0:
        result = (phase 2, unchanged progress, incoming completion)
    else:
        result = (phase 1, max(-1, progress - incoming_rate * update_scale), none)

reject nonfinite output [API policy; unresolved original overflow behavior]
if completion exists and caller supplied a hook:
    invoke hook(result) once [engineering callback interface]
return result
```

UNRESOLVED operations are not filled with a scheduler, automatic next phase,
original commit handler, UI effect, or guessed clock.
