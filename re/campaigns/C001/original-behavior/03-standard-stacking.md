# 1. Result

C001 Work 1 targeted issue #1 handoff, 2026-11-14. Campaign OPEN.
Base e6881e1a65601703dce5fb82d029aa6bb251ea09; production remains
85ec7bffc108e966f373da40a2f5cc495f11f77b. Resolves the finite no-mod/full-range
B1–B3 research handoff requested by Work 2's 04-issue1 report. No production edit.
Exact executable/archive identity and 61 independently checked decisive sites,
14 method body hashes (12 overlap historical C001 evidence) are recorded in
../stacking/static-evidence.json. Collector reads original bytes; never runs them.
No disassembly or original assets redistributed. 18 independently specified exact
byte fixtures have hand-derived heights, discriminating features and SHA256s.
Finite fixture predictions are RECONSTRUCTED, not observed original output.

Reproduce from repository root:

```sh
python re/campaigns/C001/stacking/recover.py /path/osubackup.zip /tmp/stack-evidence.json
python re/campaigns/C001/stacking/build_fixtures.py --check
# Compare regenerated evidence to stacking/static-evidence.json.
```

# 2. What they found

## B1: Full-range >5 standard pass

CONFIRMED STATIC, offsets exclude CIL header. M28ea:0000–0054 chooses >5,
prepares every slider through M144a(true), then calls M28eb(0,-1). <=5 calls
M28ec instead. M28eb returns before resets for stored mode !=0 (0000–000d).
-1 means count-1. Empty range is naturally skipped. Heights F252d are reset in
[start,end] (0044–0065). Threshold is **Single(preempt Int32) * leniency Single**,
not a rounded integer (002f–0042). Positive finite ordinary inputs only here.

M28eb has a forward extension stage (0067–0148) for partial-range calls: from end
back toward start it follows head/head proximity, or earlier-slider end/later-head
proximity, stopping on a spinner anchor or a strict time excess; matching later
objects become anchors, are reset, and extend scan end. For the ordinary call
(0,count-1), no later objects exist and this cannot extend the range. Do not omit
it from a future editor/partial-update implementation. Such consumers are OPEN.

Main walk runs **from extended end down to start+1** (014b–02ed). Skip nonzero-height
outer objects and spinner instances before classification. A circle's backward
walk can reach index0, even below a partial requested start; a slider's backward
walk stops at requested start. This asymmetry must survive future partial support.

| Branch | Precise CONFIRMED STATIC rule |
|---|---|
| Circle early exit | anchor.start - earlier.end, Int32 subtraction then Single conversion, >threshold exits (01a0–01b0); equality remains |
| Circle spinner | earlier spinner skipped BEFORE time comparison (0195–019b) |
| Circle ordinary match | distance(earlier.rawHead,anchor.rawHead)<3 => earlier.height=anchor.height+1; anchor=earlier (024b–0272) |
| Circle slider-tail match | Takes priority over head/head: earlier is slider and distance(earlier.endPosition,anchor.rawHead)<3 (01c4–01e2) |
| Tail correction | delta=anchor.height-earlier.height+1; for j=earlierIndex+1..outerIndex inclusive, if distance(earlier.endPosition,objects[j].rawHead)<3, objects[j].height-=delta; then stop this backward walk (01e4–0246) |
| Slider early exit | anchor.start - earlier.start, Int32 then Single, >threshold exits (02a1–02b2) |
| Slider match | distance(earlier.endPosition,anchor.rawHead)<3 => earlier.height=anchor.height+1; anchor=earlier (02b4–02de) |
| Other classes | Spinner outer skipped; circle/slider classes are explicit isinst tests, not arbitrary raw flag equivalence |

The circle walk resets a newly encountered index below its tracked minimum to
height0 (01b5–01c2); initial minimum=start. It has no effect for full-range start0.
Distance M4ecb uses Single differences/products/sum, Double sqrt, Single result.
All <3 tests are strict; equality3 and unordered results do not match. Do not use
per-axis tolerances or <=3. Fixtures use exactly representable operands and avoid
exceptional numeric behavior. No integer overflow policy is inferred for extreme
external operands; canonical admitted inputs still need their engineering guards.

## B2: Scale, sign and original versus prepared position

CONFIRMED STATIC: M28be:01e4–01f1 stores F0ae1=Single(F0ada / 10f).
F0ada is manager radius, not diameter or a constant3.2. It is calculated at
0172–0190 from diameter/2, viewport M0fea scaling, and Single1.0004099607467651.
CS affects diameter via M28b2; original E045's explicit ordinary viewport512x384,
CS5, no-mod radius is **32.013118743896484**, so step Single(radius/10f) is
**3.2013118267059326**. These viewport operands are declared fixture conditions,
not a proof that all game configurations use them. General modifiers and viewport
configuration normalization remain OPEN.

M28eb:02f2–0334 applies **rawHead - Single(height)*(step,step)** via M4f01 and
M4efe, then M24b0(position,true). Positive height shifts both axes negatively;
negative height shifts them positively. M24b0 stores F252c (prepared) and with true
preserves F152e (raw) and height. The false caller instead replaces raw and clears
height. Do not mutate the raw document or derive the next pass from displaced heads.
Original intermediate Single arithmetic is significant; fixtures permit .00003
coordinate tolerance to test the contract without claiming bit-identical CLR/JIT.

## B3: Geometry, tails and registration order

CONFIRMED STATIC: M28d2 binary-searches registration and inserts AFTER existing
same-time objects (0080–00cb). Distinct-time corpus is in chronological order.
General comparator/ties remain outside this packet; do not expand equal-time
admission merely because stack heights can be calculated for an ordered list.
M28e9 stores count, calls M28ea then M28ee. Geometry precedes modern stack discovery.

Endpoint helper M249c is `virtualEnd - (preparedHead - rawHead)` (0000–001c),
thus gives the unshifted endpoint during discovery even after previous translations.
Ordinary slider M1444 returns F0be0. M144a:136f sets that virtual endpoint from the
last traversal endpoint, THEN writes end time at1378. An even repeat ends at its
head, not its geometric far tail. A circle's end position is its head (inherited
ordinary type semantics); spinners are skipped as anchors and search candidates.

Slider position override M1457 computes delta=newPrepared-oldPrepared, calls base
M24b0, then translates control points, prepared segment endpoints, checkpoint/visual
state and the virtual endpoint (final virtual setter0264). Cumulative lengths and
times are not recomputed for translation. Work 2 must translate ALL prepared
representations and remap identity-based sound/health/NewCombo sidecars; this is an
integration obligation, not proof of present source correctness. Raw controls must
remain separately available. Repeated preparation must not compound displacement.

## Confidence, gaps and next gate

CONFIRMED STATIC means byte-level mechanisms at the pinned supplied EXE, not an
official release claim or original execution. Fixture heights and coordinates are
RECONSTRUCTED finite predictions. Actual production audit is separate under
../stacking/production-results.json when available; a loaded chart with an unmoved
head is a gap, not a passed stacking test. No substitute copy of this algorithm is
used as a production oracle. Work 2 had no stack production at e6881e1, so independent
review of its future implementation and exact Windows regression remain PENDING.

<=5, partial editor updates, arbitrary CS/AR/mod/config state, equal-time registration
admission, non-standard modes, exceptional numbers, P/C curves, 19 owner star gaps
and Joined Bezier remain OPEN. No fitted star correction, historical evidence edit,
original execution, owner acceptance or complete C001 claim. Next bounded action is
Work 2 implementation of this handed-off family, then independent actual-source
fixture execution, consumer review and one integrated Windows gate.

# 3. Pseudocode

RECONSTRUCTED from CONFIRMED STATIC for normal full-range >5 standard loading:

```text
prepare all slider geometry and traversal endpoints
if map.mode != standard: return
heights[:] = 0
threshold = Single(preempt) * Single(leniency)
for i = count-1 down to 1:
    anchor = objects[i]
    outerKind = runtime type of anchor
    if anchor.height != 0 or anchor is Spinner: continue
    for n = i-1 down to 0:
        earlier = objects[n]
        if earlier is Spinner: continue
        if outerKind is Circle:
            if Single(anchor.start-earlier.end) > threshold: break
            if earlier is Slider and distance(earlier.unshiftedEnd,anchor.rawHead)<3:
                delta = anchor.height-earlier.height+1
                for j = n+1 through i:
                    if distance(earlier.unshiftedEnd,objects[j].rawHead)<3:
                        objects[j].height -= delta
                break
            if distance(earlier.rawHead,anchor.rawHead)<3:
                earlier.height = anchor.height+1
                anchor = earlier
        else if outerKind is Slider:
            if Single(anchor.start-earlier.start) > threshold: break
            if distance(earlier.unshiftedEnd,anchor.rawHead)<3:
                earlier.height = anchor.height+1
                anchor = earlier
        else: break
for each object:
    newPrepared = object.rawHead - Single(object.height)*(step,step)
    setPreparedPosition(newPrepared, preserveRaw=true)
    # slider virtual override translates dependent geometry by position delta
```

The branch identity stays fixed while the anchor changes. A circle walk that adopts
an earlier slider as anchor still executes circle-branch rules on its next iteration.
