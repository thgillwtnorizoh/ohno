# 1. Result

R13-C01 — Shared Slider Channel Gain-Capture Correction. Work2, 2026-11-05.
Baseline canonical main `13d2bb58000f65c5526ce7cd81800c8fdc9b47bd`;
R13 production baseline `221c6e1d55a488489155acdadd6e8ed32243a014`.
One authorized production defect only: QA11-D01 in SettingsChanged.

Changed only that production method: preserve settings assignment order; evaluate
ReplacementGain once into a Single local; use that local for both attribute writes;
retain separate current Channel0/Channel1 reads. Added six R13-C01 development
controls and a minimal step/path in the existing Windows gate. The reusable
`ci/verification` ref runs the exact corrected commit before canonical main is
advanced, satisfying the mission's verification-before-publication requirement.
No permanent expedition/milestone development branch is introduced.

Required reading: AGENTS, standing contract, both journals, E040 report/full parsed
JSON evidence, QA11 README/discrepancy/TimingChannels, historical R13 report,
current production, R13 harness and workflow. No RTK.md is present. No original
binary re-extraction/research, original or reconstructed game/window operation,
physical input, media opening, audio playback/listening or E020 experiment.

Windows verification pending. The unchanged QA11 gate must pass253 named checks,
including the exact retained G08, together with1091 historical checks and all
prior development groups. R13-C01 controls are development, not independent QA.
Failed QA11 run42018275697 and its published artifacts remain unchanged.

# 2. What they found

**CONFIRMED STATIC:** published E040 MethodDef06003b11/T07b9, body SHA256
`81b56cede5a835a944737fcdcd34c15ff744d821dffd406bbfdaff8f1d5f4dc3`:
0018 stloc.0 captures gain;0019 reads F2354;001f ldloc.0 feeds its write;
0026 reads F2355;002c ldloc.0 feeds the second. No new original decoding occurred.
QA11's original CIL evidence, seven independently corrected expectations,
first-failure packet and G08 assertion are preserved byte-for-byte.

**RECONSTRUCTED correction:** only gain is captured. Channel targets are not a
transactional snapshot. A callback changing UniversalVolume between writes cannot
change the second gain; a callback changing Channel1 still changes the second
target. ReplacementGain retains unchecked Int32 multiplication followed by the
existing Single conversion/division/multiplication order. No Double, reassociation,
lock, deferred callback, settings cache or per-slider owner was introduced.

**Development controls (actual production component):**

| Control | Discrimination |
|---|---|
|C01|Ordinary E80/U50/V25 refresh writes0.1 to101 and202 immediately|
|C02|First write changes U50→100; both writes still use bits0x3dcccccd|
|C03|First write changes Channel1→402 and U→100; second target402, original gain0.1|
|C04|E1/U1/V17 replacement bits0x378e9b38; unchecked46341² product with V100 bits0xc851b6fa|
|C05|First/second write throws after controlled setting/target mutation; partial mutations remain, no extra writes or recomputed second gain|
|C06|Replacement order/gain, deferred fresh gain (distinct start bits0x378e9b39), captured pan/current targets, Stop versus queued Pause, activity/shared selectors preserved|

Literal IEEE32 controls use independently published QA11 G02/G05 expectations;
the harness does not reproduce an alternative gain algorithm. Exact independent
G08 remains in the unchanged QA11 suite. Baseline observed0.1/0.2 versus expected
0.1/0.1 is a conditional model discrepancy, not a measured audible imbalance.

**VERIFIED COMPONENT:** pending exact-source Windows gate. No local SDK is
available. CI will execute unchanged historical1091, full QA11 (234 components,
9 source audits,4 asset controls,6 empty-Windows controls =253), all prior
R06–R13 development groups, separate R13 Windows empty-resource group, and the
six new development controls separately. Named independent total1344 includes
source audits; it is not1344 original-runtime experiments.

**ENGINEERING:** original model exception propagation/partial mutation and the
existing Windows containment, queue/generation policy, logical activity mapping,
resource preparation, presentation bridge and backend remain unchanged. The
recording callback creates a controlled synchronous interleaving; ordinary WPF
reachability of that interleaving is not established. The six controls do not
open a media player or rely on physical audio devices.

**UNVERIFIED ORIGINAL-RUNTIME:** native BASS, actual callback scheduling, device
output, audible equivalence and owner qualification remain unchanged/unverified.
Score/HP, discrete sound progression, global timing, resource tables, replacement
conditions, deferred starts/pan/pause and Windows backends are outside the fix.
No R14/E041/QA12, spinner audio or deferred score-rating investigation is begun.

# 3. Pseudocode

```text
RECONSTRUCTED from CONFIRMED STATIC M3b11:
    store effect argument
    store universal argument
    gain = existing ReplacementGain()  # exactly one Single evaluation
    Attribute(current Channel0, 2, gain)
    Attribute(current Channel1, 2, gain)

VERIFICATION:
    mutate settings and/or Channel1 during first backend write
    assert same captured gain, fresh later target
    assert exact arithmetic bits and partial-mutation exception behavior
    execute unchanged QA11 G08 plus complete historical/QA11/development gate
    after successful exact-source Windows gate, advance canonical main safely
```
