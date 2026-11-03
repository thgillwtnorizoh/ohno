# 1. Result

QA04 — R06 Bézier geometry and integration, Work1, 2026-11-03 (Asia/Bangkok).
Target/current origin/main `c4957dfdafc1ba8cda1e88c8a90192fe95177ef1`.
Initial status: **HOSTED EXECUTION PENDING**; tests below are not yet passes.

43 focused cases link actual production Beatmap/geometry and CircleSession source.
No copied flattener, scheduler or gameplay model is executed. Expected raw vertices
come from E32, with small dyadic and integer-length controls derived from the same
instructions. Existing QA01 90, QA03 56 and R06 development preparation checks
are unchanged and are run separately. They are not claimed as new QA04 cases.

The existing `.github/workflows/r04-build.yml` is minimally extended, retaining
its filename, Windows2022 runner, preinstalled .NET8 selection and pinned checkout.
It runs WPF win-x64 compilation, QA01, QA03, R06 preparation checks and QA04.
No SDK install, GUI project launch, OS input, original executable, debugger,
recording or OBS. Component steps still run after earlier failures if SDK selection
succeeded. A single gate is sufficient; results-only publication does not rerun it.

```powershell
dotnet build reconstructed/CircleGame/Ohno.CircleGame.csproj -c Release -r win-x64 --self-contained false --nologo
dotnet run --project re/qa/qa01/Headless/QA01.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa03/Headless/QA03.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project reconstructed/R06PreparationChecks/R06PreparationChecks.csproj -c Release -- "$env:GITHUB_WORKSPACE"
dotnet run --project re/qa/qa04/Headless/QA04.Headless.csproj -c Release -- "$env:GITHUB_WORKSPACE"
```

No local dotnet SDK is available. Original research and all Work2 source, fixtures,
journals and earlier QA records remain unchanged. No new expedition or original
binary inspection is performed. E20 remains incomplete, all17 NOT RUN.

# 2. What they found

## Evidence and independent oracle

**CONFIRMED STATIC:** E32 `M144a:01f8`, format>=10 `M2d62→M2d93`;
`M2d90:0034–003e` squared second-difference <=.25f; M2d91 half split;
M2d92 weighted leaf output; M2d93 pushes right then left and appends final endpoint.
Modern duplicate guards/subranges at M144a:0218–0271. E06 shared suffix pass
09f8–0a8d and F0bdc/F0bde storage0ae1–0b37. E05/E21 directed per-piece Movement
and E16 inclusive due-prefix/one-entry judgement remain the downstream contract.
Existing E32/E06 evidence is sufficient; no original method is re-excavated.

| Cases | Independently discriminated behavior |
|---|---|
| A01–A08 | Published quadratic/cubic vertices, exact .25f flatness equality, next representable value above boundary, uneven collinear second difference, asymmetric half ordering, unchanged source and singleton endpoint duplication |
| B01–B05 | Joined repeated controls, second-range start, final-pair guard, two-control line flags, preserved leading duplicate/zero raw piece, head insertion |
| C01–C08 | Single raw lengths summed in Double, raw snapshot vs adjusted suffix, trim/removal/tolerance, extension, zero-direction guard, erased-path rejection, cumulative recomputation |
| D01–D04 | Integer-length forward/reverse pieces and inclusive first match; literal target positions; actual quadratic first three per-piece times |
| E01–E08 | Independent carried tick times/positions, one outstanding item/call, real curved segment continuity, held reversal/re-entry, head independence and final-parent/combo ordering |
| F01–F10 | v14 restriction, unsupported curves, malformed/nonfinite controls, zero geometry, 64/65 control boundary, defensive arithmetic checks, constants/work limits and Double→Int32→Single parsing |

A03 is deliberately a direct component control with fractional y=.25, not a claim
that the integer-truncating map parser produces that coordinate. A04 supplies its
next larger Single. The separate parser cases verify truncation. A06's asymmetric
control [(0,0),(1,1),(2,1)] predicts [(0,0),(.5,.4375),(1,.75),
(1.5,.9375),(2,1)] through E32 half-subdivision. No production private state is
written; calls only supply explicit geometry or gameplay arguments.

The integration control is a parsed v14 joined B path:
`0,0,3000,2,0,B|60:0|60:0|60:80,2,140`, beat1000/multiplier1/tickRate1.
Two two-control ranges yield pieces60 and80, G=L140, V100 and Q100.
This tests the Bézier range/interface through to gameplay with exact integer
lengths, separately from adaptive curved cases A/D04/E03.

| Traversal / piece | Time interval | Endpoints |
|---|---|---|
| Forward0 |3000–3600|(0,0)→(60,0)|
| Forward1 |3600–4400|(60,0)→(60,80)|
| Reverse1 |4400–5200|(60,80)→(60,0)|
| Reverse0 |5200–5800|(60,0)→(0,0)|

Recovered spacing arithmetic predicts deadlines **4000,4400,4800,5764**.
The interior at4000 is at(60,40), after traveling60+40 rather than resetting the
spacing at the bend. The reverse interior at4800 returns to(60,40). The final
entry is adjusted from5800 to5764; its display producer marker is the ending
point, whereas live required position at5764 is(3.6,0). Tests assert these literal
values; they do not ask R06 for a target and then use that answer as the oracle.
E07 earns head plus four component successes (combo5), then one whole-object
result at5800 without a sixth combo credit. D04 checks true quadratic piece
intervals3000–3006,3006–3011,3011–3016 independently from E32 lengths.

C04's L3.00005 on pieces3+4 removes the trailing4 within the recovered .0001
tolerance and adjusts the retained first direction; it is not a geometric
"clamp to the corner" oracle. C06 directly supplies a Linear path to the shared preparation interface, ending in a zero piece,
which blocks extension. C07/F05 reject erased/all-zero paths as **documented R06
engineering policy**, not original exceptional-behavior equivalence. C08 includes
an independent first cumulative value and then verifies endpoint/sum consistency;
that consistency check alone is not a separate flattening oracle.

F06/F07 execute the finite control limit. F09 checks published MaxSegments8192
and MaxSubdivisionWork50000 constants only; source inspection locates enforcement
in Emit/Append/Flatten. The 8192-output and50000-node exhaustion branches are
**not dynamically stress-tested**. That limitation avoids the prohibited
pathological subdivision campaign. No full-map maximum-allocation test is claimed.

## Classification and disposition

**VERIFIED COMPONENT** status will be assigned only from actual CI output.
Failures will be retained with expected/actual diagnostics, source location and
correction ownership; production code will not be modified or expectations weakened.

**OWNER-REPORTED:** the current assignment says R06 launches and gameplay is close
to original stable in the owner's tested scenarios. No map/build/settings/capture
provenance accompanies that feedback here; preserve it as positive scoped evidence,
not universal equivalence. Earlier R04 feedback remains historical and unchanged.

**UNVERIFIED:** original x86/CLR/SIMD precision, actual original import, complete
cross-map behavior, pixel identity, audio synchronization, physical input and
untested interactions. Modern .NET component results cannot certify these.
Older Bézier branches, full modifiers/ScoreV2/scoring/HP, original scheduler and
opaque callbacks remain excluded. Owned immutable snapshots and defensive limits
are documented engineering choices, not newly discovered original behavior.

# 3. Pseudocode

E32's recovered model is unchanged. This assignment exercises actual R06 methods
against existing finite original-CIL expectations. It adds no original-behavior
claims, replacement algorithm, application fix or permission to operate a game.
Stop after QA04; corrections and further implementation belong to Work2.
