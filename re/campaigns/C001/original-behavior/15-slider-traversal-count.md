# Issue17 — original-derived traversal count discrimination

Authority: original archive SHA256
`7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`;
osu!.exe `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`.
Fresh instruction recovery and all precise method body hashes,RVA,signatures,
selected offsets/operands and prior evidence packet hashes:
[static-evidence.json](../traversal-profile/static-evidence.json).
Reuse E005,E016,E017,E021,E022,E039,E040; no original execution.

CONFIRMED ORIGINAL: M28f7(0x060028f7) reads count at17da through M09e5,
17e3 loads9000,17e8 branches ble.s17fa,17f4 constructs
ArgumentOutOfRangeException and17f9 throws.9000 passes this comparison;9001
fails. This is not an admission theorem or a safe simulation bound. M1433 stores
raw F0bd0; M142d0124–012c applies Math.Max(1,raw) to effective F2527.
Raw+1 still controls edge data. Zero/negative cannot be globally normalized.

M144a's repeated segment traversal and parity use effective count. Retain the
instruction sites in JSON: Single segment difference, Single1000 multiplication,
Double division, absolute-time accumulation; F2524 stores conv.i4 at1377–1378.
Do not replace this with count times truncated span duration. Checkpoint phase
conversion remains separate. Final checkpoint moves by Max(midpoint,oldLast-36)
without sorting. M145c/M146f/M146a/M1465/M1449/M146b/M3b33 connect lookup,
completion,hit selection and continuous audio. Existing E17 completion can leave
pending checkpoints and derives grade from successes+head over N+1. This is not
license to repair schedules during traversal extension.

RECONSTRUCTED finite rationale: no recovered special branch at16 or32; small
positive17–32 uses same loops and connected formulas.32 is an engineering ceiling
for one auditable extension, not an original maximum. Counts33 remain negative
controls. Geometry is already bounded independently. Every higher-count experiment
is explicitly an isolated unchanged-production component, not admitted canonical
or original gameplay. Binary/runtime numerical equivalence is NOT VERIFIED.

Observed current source: Beatmap.LoadCore count guard and metadata Parse guard
both refuse17; index admits outer metadata. `slider-repeat-profile` is broader:
R16 duration-over is also labelled that way. Therefore64 owner labels cannot be
identified as64 high-count recoveries. L300 independent schedule has3R entries,
500ms spacing with moved final end-36. Fraction100.25 accumulates501.25 per span
before final truncation. Curves exercise the actual prepared geometry without
inventing an independent closed-form curve oracle. Sound subset floor N/R may
produce more than R-1 entries: P32 has159 checkpoints,39 subset entries,one pending
and aggregate100 under follow input; L30032 has96,31,none pending,300. This
preserves an observed distinction, not a newly repaired behavior.

See [checkpoint](../traversal-profile/README.md), immutable106-fixture manifest,
actual per-case results and [handoff](../traversal-profile/handoff.md).
