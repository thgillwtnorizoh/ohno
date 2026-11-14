# 1. Result

| UEP-001 rev2.1 field | Checkpoint |
|---|---|
| Project / Campaign / Actor / Assignment | Project ohno / C001 / Work2 / Issue#3 finite Catmull integration (user's implementation assignment supersedes historical issue research actor) |
| Status | VERIFIED CHECKPOINT — development/Windows delivery; independent C QA pending |
| Starting SHA | c353b83e2dfb5e6da7cfe20053f15a6afdc3626f |
| Exact integrated source | 2d776dac175c608e213dbd523c9d237c9411ec8c |
| Published SHA | Final evidence-only introducing commit in Git history |
| Objective / Supported profile | Explicit canonical C, finite normalized controls, positive nonempty prepared output, existing version5–14 and gameplay limits; v5/v9/v14 reference fixtures, not comprehensive legacy behavior |
| Functional changes | Kind0, original Single polynomial/stencils,50 independently evaluated pieces per pair, span flags, existing segment/cumulative/traversal/tick/identity/stack/render/input/sound/health consumers |
| Evidence / Confidence | Original-static authority c353b83/07-catmull-rom and immutable19fixture expectations; VERIFIED COMPONENT development only, new independent Work1 verdict PENDING |
| Local tests | Linux SDK8.0.427/runtime8.0.33:4375/4375 C development;19dispositions=13admissions+6explicit engineering rejections; retained P299/299;fixture regeneration PASS |
| CI / Artifact | Run43595153711/job132447966714 SUCCESS at exact2d776dac; SDK8.0.428/runtime8.0.34,0warnings/errors;2309historical QA/0fail, C4375/4375, P299/299, C001264/264, stack43+394 and finite106+4PASS; artifact14113958938,67870674bytes,sha256:6c5d308fcaf78856c4840957e4d0fccc8597ff56483af610d0c76afef78bd390 |
| NOT RUN | Original CLR/client/x86 precision, visible GUI/physical input/listening and owner qualification |
| Coverage gained / Remaining | Finite C integrated; independent new-source QA/runtime and full C001 coverage remain open |
| Discrepancies / Limits / Blockers | No development mismatch after correcting one new test assumption; six exclusions and engineering guards below; no original-runtime equivalence |
| Records / Reproduction | C001CatmullChecks, catmull-development evidence, workflow, implementation journal and live matrices; commands below |
| Next task / Actor / Dependencies | Work1 independent actual-source C QA using unchanged19fixture packet; owner qualification remains separate |
| Subsystem | OPEN |

# 2. What they found

Work2 extends SliderPathKind with Catmull0 and canonical C marker admission only.
Historical string loader still excludes C; L/B/P behavior and R17 admission/math
are retained. Source document, RawPath, normalized SourceControls and both kinds
retain original provenance. No Catmull count fallback or Bézier duplicate split.

The emitter uses first p0=p1 and final p3=p2+(p2-p1), the recovered ordered Single
polynomial/t2/t3, and exactly50 separately evaluated endpoint pairs per adjacent
control pair. It preserves duplicate controls, evaluates t1 rather than forcing
knots, and sets only every50th piece's span-end flag. Shared tail adjustment
retains those flags even if trimming removes a marked final piece. Requested L,
unadjusted chord G and adjusted cumulative lengths remain distinct.

Traversal/repeats/tick phase/checkpoints run through the same prepared-segment
pipeline. Tests compare every raw pair/flag and prepared endpoints/lengths to
unchanged Work1 predictions, exercise reverse traversal, actual session head/follow
input and sound event parent, health/NewCombo/resource identities and reloads.
Stack tests use the adjusted C endpoint for negative stacks and translate moved C
controls/movements/checkpoints without rewriting raw segments or times. Rendering
source consumes those same segments; no GUI test substitutes for owner qualification.

ENGINEERING guards: existing64controls/8192segments and cancellation; C coordinate
magnitude<=131072 per axis excludes the unqualified1e7 precision diagnostic and
bounds extreme extrapolation. The value is an engineering admission choice (also
matching the historical numeric helper's optional limit), not proof of an ordinary
original Play clamp or original-x86 accuracy throughout that range. Finite positive
nonzero output and inherited gameplay limits remain. Rejected fixtures:
all-coincident,singleton,bare-marker,vanishing-request,malformed-coordinate,
precision-diagnostic. Their rejection does not establish original exception or
zero/empty scheduling equivalence.13 admitted fixtures are development controls,
not complete curve-domain or original-runtime coverage.

Initial new positive-stack test incorrectly expected a preceding circle to move
a following slider. Existing original-derived stacking instead moves the earlier
object. Changed only the test's object order to put a circle at the C head later;
production stacking is untouched. Initial finding and correction retained in
local-summary.json; no historical expectation or research fixture changed.

Reproduce from exact integrated source:

```sh
dotnet run --project reconstructed/C001CatmullChecks/C001CatmullChecks.csproj -c Release -- "$PWD" /tmp/c-results.json
dotnet run --project reconstructed/C001PerfectChecks/C001PerfectChecks.csproj -c Release -- "$PWD" /tmp/p-results.json
python re/campaigns/C001/catmull/build_fixtures.py --check
```

Windows portable package includes CatmullCorpus/fixtures and its immutable manifest.
For owner testing, choose admitted synthetic fixtures in the chart browser; compare
identical hashed inputs in original stable and ohno with recorded build/settings.
Start with two-controls/three-turn/duplicate-interior, then short/long/repeat-two.
Six engineering-excluded fixtures should reject clearly. Fixtures have no music;
no listening equivalence is implied. Record path shape, movement, checkpoints,
stacking, selection/Rank and input observations. Never label headless runs as owner
acceptance. General editor/legacy/mod/mode, exceptional/zero-output behavior,
19star discrepancies and JoinedBezier stay OPEN.

# 3. Pseudocode

RECONSTRUCTED original mechanism implemented and development-tested:

```text
canonical C bytes → original-normalized controls, kind0
for each adjacent pair:
    select asymmetric four-control stencil
    for j=0..49 independently evaluate Catmull(j/50), Catmull((j+1)/50)
    retain raw pair and original span-end flag
shared chord sum → requested tail adjustment → adjusted cumulative path
existing movement/repeats/tick/checkpoint → stacking/identity binding → consumers
```

Windows evidence: catmull-development/windows.json retains retrieved step summaries, selected actual log lines, parsed historical counts/runtime/source and artifact metadata. Full raw job log remains at GitHub; no claim of local full-log archive. Portable download: https://github.com/thgillwtnorizoh/ohno/actions/runs/43595153711/artifacts/14113958938. Artifact not launched; packaging success is not owner qualification. Retained independent P regression1093/1093 also passes locally.
