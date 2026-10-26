"""R01: finite progress projection from Work 1 Expedition 02, not a host loop.

See R01-transition-kernel.md for provenance, scope and API-only policies.
"""

from collections.abc import Callable
from dataclasses import dataclass
from enum import Enum
import math


class Completion(Enum):
    OUTGOING = "outgoing"
    INCOMING = "incoming"


@dataclass(frozen=True)
class StepResult:
    phase: int
    progress: float
    completion: Completion | None


def _finite(name: str, value: float) -> float:
    # API policy: no implicit strings, booleans, Decimal or arbitrary operators.
    if type(value) not in (int, float):
        raise TypeError(f"{name} must be an int or float")
    try:
        converted = float(value)
    except OverflowError as error:
        raise ValueError(f"{name} must be representable as a finite float") from error
    if not math.isfinite(converted):
        raise ValueError(f"{name} must be finite")
    return converted


def step(
    *,
    phase: int,
    progress: float,
    outgoing_rate: float,
    incoming_rate: float,
    update_scale: float,
    on_complete: Callable[[StepResult], None] | None = None,
) -> StepResult:
    """Project one supported update; all state/rates/scale are caller supplied.

    Only input phases 0 and 1 are supported. Completion returns phase 3 or 2
    respectively, preserving progress, and invokes on_complete once if supplied.
    Arithmetic crossing a threshold emits no completion on that invocation.

    Inputs are finite binary64-convertible ints/floats. Rates and scale may be
    zero or negative: no sign restriction or extra progress clamp is imposed.
    An unbounded arithmetic overflow raises OverflowError, without a callback.
    Callback payload/exception propagation are API choices, not stable semantics.
    """
    if type(phase) is not int or phase not in (0, 1):
        raise ValueError("R01 supports only input phases 0 and 1")
    p = _finite("progress", progress)
    out_rate = _finite("outgoing_rate", outgoing_rate)
    in_rate = _finite("incoming_rate", incoming_rate)
    scale = _finite("update_scale", update_scale)
    if on_complete is not None and not callable(on_complete):
        raise TypeError("on_complete must be callable or None")

    # CONFIRMED static projection: method 0x060046a3; no other side effects.
    if phase == 0:
        if p >= 100:
            result = StepResult(3, p, Completion.OUTGOING)
        else:
            result = StepResult(0, min(100.0, p + out_rate * scale), None)
    else:
        if p <= 0:
            result = StepResult(2, p, Completion.INCOMING)
        else:
            result = StepResult(1, max(-1.0, p - in_rate * scale), None)

    if not math.isfinite(result.progress):
        raise OverflowError("R01 does not specify a nonfinite output")
    if result.completion is not None and on_complete is not None:
        on_complete(result)
    return result
