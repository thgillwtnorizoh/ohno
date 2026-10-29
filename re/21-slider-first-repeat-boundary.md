# 1. Result

Expedition 21 — First Ordinary Slider Repeat Boundary & Direction Reversal. 2026-10-29 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover one ordinary osu!standard Linear slider with exactly two traversals: loading-time directional Movement records, its first reversal, the associated scheduled component and its immediate ruleset consequence. The finite control uses fresh T02af parent/T06f6 head, ordinary T0253 ruleset/T00e8 manager, base Play, non-Edit/plain mods, valid presentation and normally returning original helpers. No complete multi-repeat system, terminal judgement, score/HP/effects/rendering, optimization or replacement gameplay implementation. `reconstructed/` and Work 2 were not inspected or modified. Work 2 remains suspended. No authorised exception.

Read the complete standing contract and cumulative journal, relevant E05–E20 findings/evidence, and the original instructions at the recovered boundaries. Preserve E05's corrected ordinary chain `M113e → M0651 → M48de → M48e0 → M142d`, E12's post-refresh update correction and E14's resource-order clarification. Earlier records remain unchanged. **E20 remains preparation-only, RUNTIME VALIDATION BLOCKED; its acceptance criteria are incomplete.** No Windows executable, original CLR method, import, input, capture, timing/performance test or substitute slider engine was executed here.

Verified remote starting commit **781c248a2b28ad9d25d014722898aa48ceac9812**, tree **49e06c28d8df2f44f4f1b73f437e089c166f3d09**. Dedicated branch **work1/expedition-21-first-repeat** starts directly there. The restored local Git mirror has exactly that tree but its baseline commit has a different identity; remote publication explicitly uses the specified original parent. All99 baseline blobs/modes were verified against GitHub before editing. Preserve the other98 files and the exact original contexts prefix.

| Input | SHA-256 |
|---|---|
| Exact osubackup.zip,18 entries |7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| Original osu!.exe |6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|
| Standing contract, repository and attachment |bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9|
| New proposed two-traversal fixture |8c5573d0d4fbc84e2898170273c31747b3f8eca232290ee01af65954633e2154|

Self-declared Stable40/file-version1.3.3.8 is static metadata, not an authenticated dated official build. Provenance/modification status remain UNRESOLVED. The available environment is Linux with no original Windows execution. Pinned analysis dependencies: dnfile0.18.0, pefile2024.8.26, dncil1.0.2. The initial bare `pip` launcher failed; `python -m pip` installed those versions successfully. Full original disassembly and binary data stay private, outside the checkout.

## Deliverables and verification

Added this report, `tools/slider_repeat_evidence.py`, `tools/check_slider_repeat.py`, `evidence/21/first-repeat.json`, and `fixtures/21/two-traversal-linear.osu`; appended `contexts.md`. No old fixture or E20 packet was changed. New fixture has one intended row `128,192,2000,2,0,L|428:192,2,300`, HP0/CS5/OD5/AR5, base beat500, multiplier1/tick-rate1/version14. Its text/import-marker intent is not a successful original import. Reuse the unchanged E01 silence generator privately: expected WAV SHA256 c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977; no audio is published.

Competing explanations: separate reversed geometry versus parity-directed reads of one prepared path; exclusive versus overlapping Movement endpoints; new direction latch versus first matching interval; checkpoint carries explicit type versus timestamp-only/result selector; selected index versus due-prefix count; reversal resets tracking versus unchanged held/entry state. Discriminating procedure: inspect original normalized CIL/stack dataflow, metadata, virtual dispatch and complete selected direct field inventories; independently extract and assert tokens/branches/constants/conversions/local/argument sites. No modern osu! formula or alternative engine is used as proof.

```sh
python re/tools/slider_repeat_evidence.py /path/osubackup.zip > /tmp/repeat-a.json
python re/tools/slider_repeat_evidence.py /path/osubackup.zip > /tmp/repeat-b.json
cmp /tmp/repeat-a.json /tmp/repeat-b.json
cmp /tmp/repeat-a.json re/evidence/21/first-repeat.json
python re/tools/check_slider_repeat.py re/evidence/21/first-repeat.json
python -m compileall -q re/tools re/validation/20
python re/validation/20/check_package.py --archive /path/osubackup.zip
```

Actual results: deterministic extraction/byte comparison PASS; focused checker PASS35methods/175 historical body-hash comparisons; all18 unchanged E02–E19 checkers PASS; unchanged E20 package checker PASS34 planned methods/17 NOT RUN cases; Python compilation PASS. Full scan19659 nonzero-RVA bodies/1739zero-RVA/0decode errors; selected branch destinations validated. Evidence is474964bytes, SHA256 **7394adf53070baae45c60eecdeb8492d0ee6f5f554258ff04ba8eca59f53375b**. Historical preservation, journal-prefix, scope/diff and remote parent/tree/branch/every-blob checks are recorded in the journal and publication handoff. None of these are original gameplay tests.

Notation Mxxxx/Fxxxx/Txxxx means EXE MethodDef0600xxxx/Field0400xxxx/TypeDef0200xxxx. Offsets below are **hex, relative to CIL code start, excluding headers**; JSON offsets are decimal. CONFIRMED means original static instructions/metadata. Player terminology, finite arithmetic projections and independent pseudocode are RECONSTRUCTED. Runtime imports/settings/JIT/clock/provider/helper/interleaving/pixel behavior remain UNRESOLVED.

| Original method | RVA | CIL code bytes | Method-body SHA-256 |
|---|---|---:|---|
| M144a |7b43c|5632|d58260ebed39834edc228b1b21995da8481e87bbc342223db2b812c7c9b7ceef|
| M146f |7e70c|899|a51c8ffbe5672e31e1919d8f0467cb0ce7b53ef44146bfb1cfd6eac8df427655|
| M1478 |7efac|34|22b611d9320971830660bdb507a1715992c9feef579167955c0c7d4d940634c5|
| M2996 |1116e0|51|1157c639b698361cc9ef6f5a59e59a60488b11a0f332070262356523fc5a9647|
| M2be0 |121f78|846|bff9bf7aff21ee0005c73887fd7ad915fdda217503b5798f19f232b912b1877c|
| M0ad2 |3b384|5130|2276c97a07e987cdb9b72e2dcf256fcf105a074ebf4e03f910feb58adebcaaec|

# 2. What they found

## What this means to an osu! player

For this straight out-and-back slider, stable prepares both directions while loading. The required tracking position travels to the far endpoint and then returns over the same prepared line. The endpoint is shared by both timed Movement intervals: exactly at the turn the first, forward record wins; just afterward the reverse record is selected. The reverse-arrow-like presentation object does not drive this tracking calculation.

At the naturally produced first repeat checkpoint, success still requires the original held/cursor test **and** tracking entry at or before the checkpoint deadline. A missed head does not itself veto it. Ordinary timely success yields raw64 and a combo increment; failure yields raw−262144 and the shared combo-reset route, conditional on the established consumer gates/helpers. A late re-entry can fail that checkpoint while tracking remains active afterward.

There is a qualification: the success code uses how many leading deadlines are due now, not the identity of the single outstanding checkpoint being processed. A delayed update can return raw8 or128 for the repeat entry, or64 for an overdue interior entry. **Raw64 alone is not proof of a repeat checkpoint.** No such behavior has yet been observed on the original Windows game.

## E21-A — One path, two directed loading traversals

| Evidence | Confirmed operation |
|---|---|
| M142d:0a8→M1433; M1433:002 | Store raw supplied traversal count in F0bd0. |
| M142d:124–12c | Effective F2527=Max(1,suppliedCount), independently of raw field. For the control, both are2. |
| M144a:0ae1,0b03–0b43 | Existing E06 prepared segment list F0bdc and cumulative double lengths F0bde; no separate reversed-path assignment in the traversal loop. |
| M144a:0d44–0d74 | First-presentation Boolean=true; double global distance phase=0; absolute Movement time=r8(S); carry=0. |
| M144a:0dd6–0dff | Zero-based traversal index local32; reverse=(`index % 2 == 1`). Reverse selects segment index count−1, sentinel−1, step−1; forward selects0,count,+1. |
| M144a:0e0a–0e40 | Read segment from that same prepared list; d=conv.r4(cumulative[i]−(i==0?0:cumulative[i−1])). The distance stays positive on the finite control even for reverse indexing. |
| M144a:0e43–0e6b | Forward assigns local14=segment.F0a32/from, local12=F0a33/to. Reverse swaps those endpoints. |
| M144a:0e6d–0e7d | dt=conv.r8(1000f*d)/F0bfc; retain original float stack category and explicit conversions, not double1000*d. |
| M144a:0e87–0e95 | Construct M2996(from,to,conv.i4(absoluteTime),conv.i4(absoluteTime+dt),easing0). |
| M144a:0ea9/0ebb | Append the **same Movement reference** to follow F0bd6.F2ce8 and ball F0bd2.F2ce8 when presentation F0bd7 is nonnull. Optional sibling lists follow. |
| M2996:008/00f/016/025/02d | T0571 stores Movement kind1, vectors F1867/F186b, Int32 times F186e/F186f. Original enum name Movement was recovered in E15. |
| M144a:0ef4–0ef9,11b5–11c0,1332–1340 | Advance double absolute time after each segment; increment segment by signed step; advance traversal and repeat while index<F2527. |
| M144a:1375–1378 | Final parent.F2524=conv.i4(accumulatedAbsoluteTime), after both traversals. |

E05's start-selected rate V and E06's actual prepared lengths govern both directions. BPM/SV/rate selection is not repeated at the reversal in this loop. On the finite control V200, one segment300 and S2000, each dt1500 is exact under the stated arithmetic inputs; total end5000. This does not establish equal arithmetic for arbitrary shapes/lengths/CLR exceptional conversions. Nonempty geometry and presentation gates are material; missing Movement lists are not replaced with synthetic records.

### Checkpoint production and spacing across the turn

The schedule F0bf8 is **List<Int32>**, with no per-entry type or direction field. F0bf6 is a parallel Boolean[]; it is not the Movement collection. E16's producer still applies: Q=F0adf for version<8, otherwise F0adf/r8(M1be4(S)); ordered Q>storedLength caps Q. For our version14/no-inheritance control, Q100. Existing old-format loading order, rate lookup and original requested-length/geometry guards remain intact.

Each traversal resets local remaining distance from the original geometric sum at0db2 and suppression=false at0db5. Absolute time and distance phase are **not** reset to S/0 there. Interior loops add Q to phase, subtract Q from carry/remaining, and suppress an interior when remaining is ordered<=0.01*V (0f08–0f2a). Each accepted interior appends `M145c(conv.r4(phase))` at0f42. M145c returns `conv.i4(r8(S)+(conv.r8(singleDistance)/V)*1000.0)`, preserving division/multiplication/conversion order.

After each traversal, phase+=carry at11c5–11ca, and the reached path appends its end timestamp at11db. Then:

- If suppression=true, carry=0 at11e4–11ed.
- Otherwise, phase=phase−(Q−carry) at11f1–11f9, then carry=Q−carry at11fb–1200. Preserve this order; it reflects spacing for the reversed traversal rather than independently restarting a fresh Q clock at the turn.

For example, a separately labelled conditional prepared length260 with the same rate/Q gives forward interiors at2500/3000, boundary3300, reverse interiors3600/4100, end4600 and adjusted final4564. The reverse checkpoints revisit the forward path positions200/100 units from the head; there is no new fixture import or runtime claim. The exact code, not a generic mirrored-tick formula, is authoritative.

Only **the final stored entry** is adjusted at14af–14d7:

`times[N−1] = Max(unchecked(S + signedDiv(unchecked(E−S),2)), unchecked(oldLast−36))`.

The first repeat entry is not adjusted. No sort follows. Boolean[count] is allocated14e8–14ed. M144a:150e computes signed h=N/R;151b–1551 appends timestamps with `(i+1)%h==0` to F0bf7, only while i<N−1 and h>0. For our N6/R2, that separate list contains **[3500]**. It is not an explicit type tag added to F0bf8 and is not read by M146f's classifier.

| Role, independently established by producer | Global distance passed to M145c | F0bf8 after final adjustment |
|---|---:|---:|
| Forward interior1 |100f|2500|
| Forward interior2 |200f|3000|
| First traversal end / reversal |300f|3500|
| Reverse interior1 |400f|4000|
| Reverse interior2 |500f|4500|
| Final traversal end |600f|4964 (old5000)|

Thus conditional list **[2500,3000,3500,4000,4500,4964]**, parent end5000, Boolean[6], F0bf7[3500]. This chosen finite schedule is increasing. The general E16 nonchronology finding is preserved: final adjustment/truncation are not globally repaired. Schedule values are predictions conditional on actual constructor/prepared operands, not naturally imported observations.

### Separately constructed endpoint presentation

M144a:1256–127c computes an angle from directed from−to components via Atan2→conv.r4. At12be–12dc, when presentation exists, it constructs **T05b1/M2be0(manager,endpoint,time,presentationStart,first,nextTraversal,angle,parent)**. First traversal uses endpoint(428,192), time3500, first=true and nextTraversal=true; the presentationStart has its original first/next computation at127e–12b4, including opaque M1468, not an invented default. The second uses the head endpoint and nextTraversal=false. Each is stored in parent.F0bd5 at1302; visual collections are separately incorporated at1318/132a, with the first-only colour-list branch retained.

M2be0 invokes ordinary M45ec at009, removes approach/number references from its list, stores first flag F19d0, and conditionally creates a replacement F2bfb sprite at09a/09f only if nextTraversal is true (03c→0f1 bypass). This is RECONSTRUCTED reversal-indicator presentation; encrypted resource key2064519496 is not decoded into an assumed asset filename. Later M4b36-dependent scale/rotation work is an explicit presentation boundary, not a gameplay direction controller or pixel proof. Complete repeat-arrow rendering is outside scope.

## E21-B — Required tracking position before, at and after reversal

M146f retains parent evaluated/start guards, original held-mask/history rules, conditional provider T0170/M43a8 correction, and manager radius selection. It samples query time **q=unchecked(freshF2358−delta)** at00f0–0116, delta=M3df6(F03e7) only on the original provider branch, otherwise0. The due scan and tracking entry read F2358 separately. They do not subtract delta simply because q does.

M146f:0133 calls List.Find on ball.F2ce8 with M1478. Predicate: signed record.start<=q (bgt000c rejects greater), then record.end>=q (clt001a/ceq001d). Both endpoints are inclusive; List.Find returns the first match in list order. No kind/direction check is added. Loading appended forward then reverse records. With q3500, both [2000,3500] and [3500,5000] match, so forward wins. With q3501, only reverse matches. Both yield endpoint(428,192) at q3500 under these finite records, so first-match choice itself causes no position jump in the control.

For a nonzero-duration record, original target is:

`from + (to−from) * (1f − conv.r4(unchecked(end−q)) / conv.r4(unchecked(end−start)))`.

M146f:01ee–0233 uses original vector helpers that store Single components; no arbitrary extra rounding after every CIL opcode. Zero duration chooses `to` (01e2–01ea). Fallback may select last record for q>last.end and extrapolate, not clamp; narrow provider-before-start fallback can select first. These are retained E15 branches, not needed to force the normal in-range reversal.

For delta0 and stable reads: q3499 selects forward, X is the original Single expression near427.8; q3500 selects forward, X428; q3501 selects reverse, X near427.8. Y192. Exact off-integer binary32 values depend on original instruction/CLR evaluation; no displayed pixel coordinate is asserted.

Mapped pointer uses original M0ff1 at023b; squared distance at0242 must be **strictly less** than radius² (`clt`024c). Inactive radius=manager.F0ada; active radius=that value multiplied by2.4000000953674316f. Equality/NaN fail. A new press is not required for continued hold, but E15's complete held-history policy is retained, not replaced by an any-key Boolean.

There is **no special traversal-boundary branch or direction write** in this reached M146f target/decision path. Existing tracking F0bf3, entry F0bf2 and remembered mask F0bf0 persist unless original enter/leave/held logic changes them. Geometry orientation is encoded by the prebuilt Movement records. This is a bounded negative finding, not a claim that no presentation direction state exists.

M1449 separately uses F0bf4 and N/R parity to call ball M4515/M4533 before scanning checkpoint times, then can store F0bf4 and invoke M1469 or change selected presentation transforms. It runs later through M28e3. F0bf4 is **not** the component processed index or an operand read by M146f; full presentation/audio continuation is deferred. Do not replace the required-position search with that parity state or presume a rendered ball snapshot equals the live required target.

## E21-C — First repeat checkpoint classification

For ordinary timely progress, previous two interiors have been accounted for: j=unchecked(F0bf1+F0bf5)=2. At stable T3499, due-prefix k2, so no outstanding work. At T3500, list[2] equality becomes due and k3; N6,R2,h3. One selected entry j2 is evaluated:

1. followOK must be true; then signed entryTimeF0bf2<=times[2]=3500. Equality passes at M146f:02c6. No head quality, direction, F0bf7 or F0bf4 is read here.
2. Success increments F0bf1 at02d1 then writes outcomes[2]=true at02df. k!=N and signed `3 % (6/2)==0`, so raw64 at030c.
3. Failure increments F0bf5 at031e then writes outcomes[2]=false at032c. Updated processed count3!=6, so raw−262144 at034f. No special repeat failure code on this reached state.
4. At most one entry is processed. !followOK and old tracking=true and updated processed<N then invokes M146e at037c **before return/consumer**. Entry-time-only failure with followOK=true does not invoke this leave; tracking may stay true.

A missed/unjudged head does not veto this classifier. Entry3500 can succeed with appropriate held/cursor operands; entry3501 fails old checkpoint3500 while active tracking may remain true. Physical release has no instantaneous checkpoint callback: the next reached held/follow decision matters. Helper exceptions can leave bookkeeping changed without a normal raw handoff.

### Why delayed updates change apparent result identity

The success selector tests due-prefix **k**, not j+1; failure uses updated counters instead. At k3 with j0, an overdue first interior can return64. At k4 with j2, the real repeat-boundary item can return8. At k6 with j2, it can return128. No entry-type coercion repairs that distinction. Repeated same-time calls can advance one outstanding entry per call; the optional nullable-time gate accepts equality, but actual host callback cadence is unverified. Prefix scanning stops at the first future timestamp and samples F2358 each comparison; do not replace it with a count of every due item or a single atomic time snapshot.

Parent completion remains E17's stored-end condition end<=freshT through M28e4→M28d3(parent)→M1465, not the adjusted terminal deadline or first repeat. It does not drain a pending repeat/component. If optional tracking was skipped or backlog persists at end, aggregate consumes current successes/head/Count, then parent evaluated state prevents later M146f processing. No whole-slider scoring algorithm is added here.

## E21-D — Immediate ordinary consumer and continuation

M0ac5:004c receives M146f's enum, then separately rereads Play.F1380 for virtual M0ad2:005c. Ordinary T0253.M114a skips optional M2282 when the parent is still unevaluated, then directly calls base M0ad2:0022. Stable object/reference assumptions matter: source result and supplied reference are not proven atomic. Raw0 is forwarded but base returns immediately.

Preserve the original F05f6-dependent prefix, result masks, helper calls and record/mode guards from E16/E18. Positive masks leave64 unchanged; no manager M28d3 decoration intervenes on this component route.

| Outcome | Scoped original consumer operation |
|---|---|
| Repeat success raw64 | M0ad2:02b8→0649. Numeric local=30 (0649), auxiliary double=F0604*4.0 (064d–065c). This differs from raw8's10 and*3.0. Neither is a proof of total displayed score. |
| Success presentation boundary | Reads **Play.current**, casts T02af at0668; null or M4b36 true skips optional work. Otherwise opaque key2064574730→M38b5, new M4510 sprite at06ac using current ball drawable.F2cf6, M474a and Fade with separate fresh time+300/+360, registration M2ce6:070f. This is not the returned parent identity by unconditional proof, required tracking target, rendered pixels or decoded effect name. Raw8 uses a different key2064574750 and its own matching boundary. |
| Repeat failure raw−262144 | M0ad2:0260→0426, numeric local0 and original M125a(map.F0b54,−4,−15,−28). Same scoped failure branch as interior−262144; no explicit component index/direction argument. |
| Shared ordinary non-Mania store | obj.F153d at100c becomes30 versus0 under the stated flags/helpers. Raw8 control stores10. This is object-local, not current combo. |
| Combo success | Local combo-enable remains true for64, as for8; common positive path getter M40f0→unchecked+1→setter M40f1 at11c7. Current signed Int32 count is T0892.F2674; setter's sole store002. Maximum-like F1975 and UInt16 snapshot F198a follow. |
| Combo failure | Common negative combo-enabled path optional feedback (oldcombo>20 and other gates), then M40f1(0):1287 and snapshot0. Feedback guards do not guard reset; exceptions can interrupt before it. E19's later bit32/target/fail decision is preserved as a dependency, not re-investigated or universal game-over. |

Thus timely repeat64 and interior8 both add one combo unit on this ordinary path, despite different object-local amounts/auxiliary/resource branches. First-head success64 can enter the same base branch with the parent but has different preceding head mutations. Raw-code equivalence alone cannot reconstruct provenance. Failure at the repeat can retain tracking=true if entryTime alone failed; consumer does not itself reverse/reset the Movement schedule or classify the parent aggregate. Original result observers/effects/HP remain beyond the first consequence boundary.

## Interface, confidence and unresolved conditions

Inputs: exact parent/head/manager/handler/current identities; effective and raw traversals; prepared segment/cumulative arrays; S/V/Q/geometric sum; ball/follow Movement list order/endpoints/times; provider correction; each clock read; held latch/history/remembered mask; mapped cursor/radius; F0bf2/F0bf3; timestamp list/counters/Boolean[]; readiness/candidate/nullable gates; consumer flags/resources/records/combo state. Outputs: prepared forward/reverse records and schedule/subset; one counter/Boolean mutation and returned enum; possible enter/leave; immediate parent result store, combo and optional presentation boundary. Neither a rendered arrow nor head quality is a direction input to the reached tracking calculation.

CONFIRMED: static fields/types/branches/conversions/order, shared geometry reversal, inclusive interval query, one-entry classifier, raw64 consumer and direct combo setter. RECONSTRUCTED: gameplay names, the finite fixture schedule/targets and independent pseudocode. HYPOTHESIS: none promoted to established behavior; distorted-slider stutter remains a separate unmeasured objective. UNRESOLVED: successful original import, actual provider/mod/policy/resource/CLR/JIT state, exceptions/reentrancy/scheduling, visible reversal/captured pixels, actual runtime results and performance. No E20 case is upgraded.

## Conditional static reference predictions

Unless changed, use the finite control above, parent unevaluated, stable references, delta0, original held policy/space passing, all relevant time reads equal T, helpers returning normally. Independent cases restore state. When j2 is specified the two earlier interiors were accounted for by original calls; no list/raw field is injected. Artificial cases are labelled.

| ID | Case | Branch-derived prediction, not runtime observation |
|---|---|---|
| R01 | Fresh two-traversal loading inputs | Two directed Movement records, end5000, times[2500,3000,3500,4000,4500,4964], N6, Boolean[6], F0bf7[3500]. |
| R02 | q3499 | Forward selected, target near(427.8,192), using original Single expression. |
| R03 | q3500 | Both intervals eligible; first forward record selected; target(428,192). |
| R04 | q3501 | Reverse selected, target near(427.8,192); no explicit tracking-state reversal store. |
| R05 | Stable following from3400, j2, T3499 | k2, return0; next repeat item untouched. |
| R06 | Same state,T3500 | k3,j2; C++,Boolean[2]true,raw64, active tracking retained. |
| R07 | Entry exactly3500; all relevant reads3500 | Entry equality passes; raw64 if held/strict space passes. |
| R08 | First/re-entry3501,j2 | follow true but entry>3500: F++,Boolean[2]false,raw−262144; tracking can remain true. |
| R09 | Release published before reached T3500 check,j2 | follow false; raw−262144, then guarded leave if old tracking and pending remain; consumer later resets combo under ordinary gates. |
| R10 | Released/re-entered3499, held at3500 | Entry3499<=3500, can succeed; earlier leave alone is not a repeat miss. |
| R11 | Missed head, valid later tracking entry3400 | Repeat can succeed; no head predicate in component block. |
| R12 | Cursor exactly at follow radius² | Strict clt fails, independent of inclusive circle input disk. |
| R13 | Correct hold and target through q3499/3500/3501 | No mandatory release/new press or tracking reset at reversal; space/held checks still run. |
| R14 | T3500,j0 with earlier entry and follow true | k3 selects raw64 for outstanding interior index0. It is not the repeat entry. |
| R15 | Repeated reached T3500 calls from j0 | One index per call: j0,j1,j2 each can return64 while k3; next returns0. Not an all-due loop. |
| R16 | T4000,j2, entry<=3500 | Actual repeat entry succeeds but k4 gives raw8, not64. |
| R17 | T4964,j2, entry<=3500 | Repeat entry success can return128 since k6==N. Parent end5000 not yet reached. |
| R18 | T4964,j2,follow false | Failure advances to processed3, so−262144, not terminal−524288. |
| R19 | Timely repeat processed then sameT3500 | j3==k3, no new component; raw0 base consumer early return. |
| R20 | Head success then repeat fail | Separate head raw/result remain; component failure can reset combo without parent evaluation. |
| R21 | raw64 versus raw8, matched consumer flags | F153d30 versus10, auxiliary*4 versus*3, different optional resource key; both common combo+1. |
| R22 | Repeat failure, prior combo0 versus positive | Reached combo-enabled reset writes0 in both; >20 feedback gate is separate. |
| R23 | follow true but entry later than outstanding deadline | Failure despite active state; no guarded leave based solely on negative result. |
| R24 | Provider delta nonzero at F2358=3500 | Target selection uses3500−delta, due scan still fresh unadjusted F2358; no guaranteed shared boundary sample. |
| R25 | Clock changes between due comparisons/entry | Preserve each read; no single physical key-event or frame timestamp prediction. |
| R26 | Candidate/current absent, readiness/nullable rewind skips tracking | No M146f result; normal post-refresh path can still reach object updates/completion. |
| R27 | Parent completion at5000 with pending repeat | No drain; aggregate uses recorded C/head/N; evaluated parent blocks later tracking. |
| R28 | Conditional prepared length260, otherwise same finite inputs | First boundary3300, reverse interiors3600/4100, end4600, adjusted terminal4564; spacing carry is reflected, not reset. |
| R29 | Artificial duplicate boundary timestamps | Prefix can include both; one outstanding index/call; no deduplication/type repair; selector depends on k/N/R. |
| R30 | Artificial nonchronological list | Scan stops at first future item, hiding a later due item; no sort invented. |
| R31 | Artificial N/R==0 with successful k!=N | Original signed div/rem can throw; no safe divisor substituted. |
| R32 | Enter/leave/resource/prefix helper throws | Earlier writes can persist; no guaranteed normal result or combo mutation. |
| R33 | Missing presentation during loading | Original Movement construction gates apply; do not fabricate a target list/import pass. |
| R34 | Ordinary dirty/Edit recomputation | M1449 can call M144a(false); fresh plain control excludes this; no stale-list universal prediction. |
| R35 | Separate consumer Play.current reference changes | Optional effect can use a different current slider; record actual refs instead of equating them to result source. |
| R36 | Final adjusted4964 vs first repeat3500 vs parent end5000 | Three distinct boundaries; neither checkpoint deadline alone completes parent. |

## Remaining dependency and smallest next investigation

**Original Windows validation NOT RUN.** Reuse E20's existing packet once its host prerequisite is available; do not recreate it. Import this separately hashed fixture through the original interface, log actual R/S/E/V/Q/path, Movement object identities/endpoints/times, F0bf8/F0bf7/F0bf6, parent/head identities and active policy first. Then trace original consumed q/provider delta, selected record, mapped cursor/radius/distance, held history/entry, each due-scan read/k/j, counter/Boolean/result, M114a/base branch/F153d/combo setter before/at/after the turn. Record T05b1 construction separately from tracking and captured visible pixels.

Use uninterrupted follow, release/re-entry before/after the boundary and missed-head/later-follow controls. Exact equality/backlog/duplicate/nonchronology forced probes must be labelled artificial; natural traces require real operands. Record thread/sequence/QPC, drops and traced/untraced overhead, without invoking extra original timing/input/presentation calls merely for logging. Bind MethodDefs/CIL offsets to actual loaded module/JIT as E20 requires; CIL offsets are not native addresses. No capture/performance/physical latency inference follows from static instructions.

Smallest following static gameplay investigation: the **final traversal checkpoint's distinct success/failure branch and its ordering against parent completion** for this same two-traversal control. This would close the terminal raw128/−524288 consequence without expanding into full scoring, repeats or rendering. Actual Windows first-reversal validation remains the smallest empirical dependency. Work 2 stays suspended until explicit authorization.

# 3. Pseudocode

Independent non-executable RECONSTRUCTED description of CONFIRMED instructions. Original helpers, exceptions, distinct reads/conversion categories and gates remain; this is not a substitute slider implementation.

## Bounded loading continuation

```text
retain original E05/E06 geometry/rate/Q/length preparation and presentation clears
time = r8(S); phase = 0(double); carry = 0(double)
firstPresentation = true
for traversal = 0; traversal < reread parent.F2527; traversal++:
    remaining = original geometric sum
    suppress = false
    reverse = signedRem(traversal,2) == 1
    indices = original forward 0..count-1 or reverse count-1..0
    for i in those original indices:
        segment = same preparedSegments[i]
        d = r4(cumulative[i] - (i == 0 ? 0 : cumulative[i-1]))
        from,to = reverse ? (segment.end,segment.start) : (segment.start,segment.end)
        dt = r8(1000f*d) / V
        if original presentation exists:
            move = original M2996(from,to,i4(time),i4(time+dt),0)
            append SAME move reference to original follow/ball lists and gated siblings
        time += dt
        carry += r8(d)
        while NOT unordered_or_less(carry,Q) and NOT suppress:
            phase += Q; carry -= Q; remaining -= Q
            suppress = NOT unordered_or_greater(remaining,0.01(double)*V)
            if suppress: leave this segment's checkpoint loop
            times.Add(original M145c(r4(phase)))
            retain original optional interior presentation operations
    phase += carry
    times.Add(original M145c(r4(phase)))    // traversal-end producer identity
    if suppress:
        carry = 0
    else:
        phase = phase - (Q - carry)
        carry = Q - carry
    retain original endpoint T05b1 construction and collection registration,
        with nextTraversal=(traversal < reread R-1), firstPresentation and angle
    firstPresentation = false
parent.end = i4(time)
retain original final-entry-only Max(S + signedDiv(end-S,2), oldLast-36)
allocate Boolean[times.Count]; retain original F0bf7 subset and later loading work
```

## Tracking query and one boundary component

```text
if parent.evaluated or fresh F2358 < S: return 0
held = ORIGINAL E15 MASK/HISTORY POLICY
followOK = false
if held:
    radius = original manager radius, multiplied by 2.4000000953674316f if active
    q = unchecked(fresh F2358 - ORIGINAL CONDITIONAL PROVIDER DELTA)
    move = FIRST original ball transform with start <= q <= end
    retain original last/first fallback branches
    if move exists:
        target = move.to if move.end == move.start else
            move.from + (move.to-move.from) *
              (1f-r4(unchecked(move.end-q))/r4(unchecked(move.end-move.start)))
        followOK = original DistanceSquared(original M0ff1(pointer),target) < radius*radius
if followOK and NOT active: original M146d()     // fresh entry time; later active=true

r = 0; k = 0
while k < reread times.Count and times[k] <= fresh F2358: k++
if unchecked(successCount + failCount) < k:
    j = unchecked(reread successCount + reread failCount)
    if followOK and entryTime <= times[j]:
        successCount = unchecked(successCount+1); outcomes[j] = true
        if k == reread times.Count: r = 128
        else if signedRem(k,signedDiv(reread times.Count,reread traversals)) == 0: r = 64
        else: r = 8
    else:
        failCount = unchecked(failCount+1); outcomes[j] = false
        r = unchecked(reread successCount+reread failCount) == reread times.Count
            ? -524288 : -262144
    // No loop back to process another outstanding component.
if NOT followOK and active and unchecked(successCount+failCount) < reread times.Count:
    original M146e()                       // before raw handoff; not automatic at a reversal
return r
```

## Scoped immediate consequence

```text
M0ac5 forwards r and SEPARATELY reread current object -> ordinary M114a -> base M0ad2
retain optional evaluated hook, zero early return, original prefix/masks/helpers
if local result == 64:
    numericAmount = 30; auxiliary = F0604 * 4.0
    retain conditional current-slider resource/sprite/register boundary
else if local result == -262144:
    numericAmount = 0; auxiliary = original difficulty helper
else:
    retain original other-result branch        // delayed prefix can select8/128
under original ordinary convergence: passedObject.F153d = numericAmount
under original combo-enable gates:
    positive result -> M40f1(unchecked(currentCombo+1)), then max/snapshot operations
    negative result -> retain optional feedback, M40f1(0), snapshot and original suffix
STOP at established immediate consequence/next tracking dependency

UNRESOLVED: actual original-Windows values, natural import/results/overhead and pixels.
E20 runtime acceptance remains incomplete; no replacement gameplay is authorized.
```
