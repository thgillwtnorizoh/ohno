# 1. Result

Issue19 / C001 / Work1, 2026-11-22. Original static research and independent
characterization only. Starting main77b4d582f5d54e3f52018a6b9b6fc593c9742045;
exact functional production6a152648c72773eb33d37f8ac01f18ca35ab1f41. No production
change. [Full UEP checkpoint](../control-count/README.md),
[finite handoff](../control-count/handoff.md),
[authenticated static packet](../control-count/static-evidence.json).

Fresh archive/executable identity checks use SHA256
7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 /
6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d.
These authenticate supplied bytes against historical evidence, not an official
dated build or an unmodified installation. Original self-description remains
Stable40/file-version1.3.3.8; official provenance remains unresolved.

Recovered18 original method identities,39 historical body matches,748 selected
historical instruction sites,15 additional anchors and3 independently checked
branches. `control-count/recover.py` repeats extraction from the supplied archive;
output must equal the committed static packet. Original bytes, disassembly and
assets are not published. Python3.12/dnfile0.18.0/dncil1.0.2/pefile2024.8.26 on
Linux; no original CLR/game execution, GUI, device audio or owner corpus access.
Existing E005/E006/E032 checkers remain unchanged and pass.

# 2. What they found

CONFIRMED ORIGINAL means original static instructions below, not observed game
execution. RECONSTRUCTED describes semantic aliases and finite predictions.
ENGINEERING describes reconstruction admission and resource limits.

| Original site (MethodDef0600xxxx; code-relative hex) | Direct evidence and interpretation |
|---|---|
| M28f7:1484–1558 | Head coordinates use M09e3 Single parsing, bounding helper calls with0 and512, conv.i4, then conv.r4/Vector2 construction. Head Y flip subtracts the integer Y from384. Head parsing precedes slider-path parsing. Do not apply head bounds to path points. |
| M28f7:1691–17c8 | Split path by124 (`\|`), iterate array length. One-character cells take marker dispatch; enum2 store1728 is Linear. Other cells split by58 (`:`). Control X uses M09e4 then conv.i4/conv.r4 at176c/176d. Y optionally subtracts parsed Double from384 BEFORE conv.i4/conv.r4 at17ac/17ad. Append at17b7 in input order. |
| M09e4:0000–0053 | Double.Parse with stored provider, then immediate return when validation=false.131072 is a conditional validation operand, not an unconditional clamp. With validation enabled, bounds and NaN can throw. Unchecked out-of-range conv.i4 and original CLR precision remain UNVERIFIED. |
| M142d:00ad–0123 | Null/empty lists receive head. Non-null supplied list is retained by reference. Exact Vector2 comparison determines insertion at index0. No general duplicate removal or collinearity collapse occurs. |
| M144a:00b4 | Switch selects Linear2→07a2, distinct from B/P/C. P fallback reaching Linear does not make a source P path eligible for an L-only extension. |
| M144a:07a2–0895 | Loop starts i=1, compares i with actual point Count at0877. Temporary pair allocation07c6; inner count1 at07cd; interpolated segment allocation082d, append083f, flag writes0837/085f. N normalized controls emit N−1 raw pieces, including coincident endpoints. Endpoints are `A+(B−A)*0f` and `A+(B−A)*1f`, preserving Single operation order. |
| M110d/M4ec9/M4ecb/M4ed2 and M4efd/e/f00 | Segment subtraction, squared length/sqrt and Single conversion; distinct distance/normalization calls remain significant. Current scalar oracle deliberately uses horizontal integer pieces with exact +/-1 direction. It does not establish arbitrary x86 normalization bit identity. |
| M144a:08b1–08d0,09d9–0b54 | G sums unadjusted Single piece lengths as Double and is stored separately. Tail remove threshold is `distance<=excess+.0001`; otherwise unequal endpoints are shortened/extended using normalization and a Single excess. Equal final endpoints block extension. Cumulative prepared lengths are recomputed; G is not replaced by that sum. |
| M144a:0bea,0d47–0f45,1332–1378 | Empty temporary geometry returns before final end assignment. Nonempty prepared cumulative differences convert to Single; Single1000 multiplication precedes Double division/absolute accumulation, repeated forward/backward. Final end uses conv.i4. G also controls checkpoint suppression; requested length is not a substitute for either G or prepared length. |
| M28f7:1cfa–1d03 / M28f5:00cb–0158 | Row catch is metadata catch type System.Object. Validation=false takes rethrow at1d03; true takes the continue route. Normal M28f5 call passes false to validation (00d3/00d4); its Exception handler contains a false result at0156/0157 and further cleanup/error handling. No dedicated original64/96-control refusal is established. Transitive exceptions, allocation and postprocessing can still fail. |
| M28ea/M28ec/M1457/M145f/M144b | Version5/6 changes loading/stacking order; M1457 translates source/prepared coordinates without recomputing cumulative lengths. Position queries can request preparation. Visual preparation reads prepared endpoints and cumulative lengths; it has resource dependencies beyond pair construction. This is not a measured first-visible-frame or performance guarantee. |

The aliases C/B/L/P reuse accepted C001 parser research; the new extraction checks
the enum2 branch and opaque literal token identity without executing the original
string decoder. Marker cells are not control points. The new fixtures explicitly
reject extra marker cells under the current canonical single-kind engineering
profile; they do not assert that the original parser rejected all such strings.

For ordinary finite converted coordinates, normalized count is
`raw coordinate count + (first converted point != prepared head ? 1 : 0)`.
The path marker contributes zero. Equality is tested AFTER conversion. Fractional
128.9:192.9 therefore equals head128:192 in these no-flip fixtures. Duplicate
generation can also put the first point back on the head; manifest counts derive
from bytes rather than the requested case label. Count65 is not an original
discontinuity, and96 is not an established original maximum.

INDEPENDENTLY VERIFIED current reconstruction:168 fixtures give38 canonical
preparations and130 refusals.104 stop at the raw-cell gate;15 pass that gate and
fail the geometry helper's supplied-count check after loader head insertion.
The latter message differs from the helper's own normalized guard when that
public helper is called with a raw list. Both direct and catalog/audit routes
are recorded; unsupported catalog version/mode can preempt direct-loader errors.
All admitted cases exercise genuine canonical session/score/health/sample owners.
110 isolated predicted-geometry scheduling probes are ENGINEERING component tests,
never successful >64 canonical loads. Full scalar geometry/checkpoint predictions
are checked against actual admitted production and actual scheduling separately.

Resource prediction:96 controls imply at most95 raw/prepared pieces; tail
adjustment can remove pieces or extend one, not increase their count.32 traversals
therefore imply at most3040 movements. Existing4096-parent/100000-map checkpoint,
100000 prepared-map-segment/250000 map-movement limits remain independent.
Exact4096 and one-over/remaining-budget/cancellation tests execute the unchanged
scheduler on predicted96-control geometry. No original safe allocation maximum,
benchmark, GPU bound or frame-time guarantee follows from finite record counts.

No missing original-static dependency blocks the proposed finite canonical-only
L profile. New-source integrated gameplay admission and Windows/independent QA
remain future gates. C001, Issue19, subsystem and original-runtime Issue4 OPEN.

# 3. Pseudocode

RECONSTRUCTED original ordinary finite no-editor/no-flip path, backed by the
CONFIRMED ORIGINAL sites above; engineering refusals are separate:

```text
parse head through original bounded Single -> Int32 -> Single path
iterate every path cell; marker selects kind, coordinate cell parses Double
convert each coordinate Double -> Int32 -> Single, retaining input order
retain supplied point list; insert head iff empty or first Vector2 differs
if source kind is Linear:
    for each consecutive pair A,B:
        append segment(A+(B-A)*0f, A+(B-A)*1f), retaining zero pieces/flags
sum Single lengths into Double G
perform original requested-length tail removal/adjustment with .0001 tolerance
if final endpoints coincide, preserve extension-blocking behavior
if no pieces remain, retain original early-return boundary
build prepared cumulative lengths independently from G
repeat existing Single-distance/timing/checkpoint operations forward/backward
truncate accumulated absolute time only at original stores
let existing loading, translation, position and visual consumers read their owners
```

ENGINEERING proposal only: canonical ordinary Mode0 source L/v5–v14 may opt into
at most96 normalized controls. Keep all historical public helpers/string APIs
and source B/P/C at64. Check actual normalized count and existing resource plans;
never deduplicate, replace a chain with its chord, fit coordinates, sort schedules
or treat a private scheduling probe as canonical admission. Original execution
and official-build authentication remain UNRESOLVED.
