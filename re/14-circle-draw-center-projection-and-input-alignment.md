# 1. Result

Expedition 14 — Ordinary Circle Draw-Center Projection & Input Alignment. 2026-10-28 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover the smallest original path from one ordinary hitcircle's current stored center to its face position, prepared quad, projection/attribute/viewport handoff, then compare it with E13's input mapping. **The CPU position mapping is established; actual shader use and displayed-pixel alignment remain conditional.** This bounded integration stops at the precise shader/reflection/native-state dependency below. No renderer, projection simulator, coordinate converter, gameplay component, optimization or replacement numerical model was built. Work 2 and `reconstructed/` were not inspected or modified; Work 2 remains suspended. No authorised exception.

Read the complete standing contract and identical attachment, cumulative contexts, relevant E01–E13 records and evidence, especially E08/E09/E13. Preserve E05's corrected ordinary M113e→M0651→M48de→M48df→M45eb/M45ec, exact circle T094c. Preserve E12's correction: M226b's post-refresh conditions rejoin before M28e3, controlling intervening ruleset work rather than skipping that call on reached normal-returning paths. Historical wording and checkers remain unchanged.

Verified requested remote parent `db5aa384c248e232681428332672f9e0368af50d`, tree `d0c7cc5b01664114de5201346bef5e622cab331f`. Dedicated remote branch `work1/expedition-14-circle-projection` was created directly from that parent. Local mirror `b4957c5` has the same tree and a different commit identity; publication explicitly uses the requested remote parent. All 58 baseline files were fingerprinted before research edits; preserve the other 57 files and exact journal prefix. Final handoff supplies the verified publication commit.

The initial E14 publication `9fe7e49b8e36ce21535e199c59f13812b52ef438` is directly parented by E13. A subsequent additive commit clarifies M451a ordering and adds checker assertions; it preserves that publication and all prior history. The journal clarification is appended, not substituted for its earlier wording.

| Pinned input | SHA-256 |
|---|---|
| osubackup.zip, 18 entries | 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 |
| osu!.exe | 6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d |
| OpenTK.dll | b415f3e061d9758316074dcbf31d6dba48cb0b89405254db94ead0e43ed88511 |
| Repository/attached Instruction.md | bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9 |

Same self-declared Stable40/file-version1.3.3.8 reference. Official dated release, authenticity and modification status remain UNRESOLVED. Linux static analysis; no Wine/dotnet/DISPLAY or `/dev/dri`. Pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 installed after the initial missing-package check. **No original Windows executable, CLR method, native API, shader, gameplay, capture, differential, pixel or performance experiment was executed.**

## Evidence, competing explanations and reproduction

Created this report, `tools/circle_projection_evidence.py`, `tools/check_circle_projection.py`, `evidence/14/circle-projection.json`, and an append to `contexts.md`. Evidence contains 57 EXE methods and two OpenTK matrix methods, signatures/RVAs/body hashes, original token/constant/branch/arithmetic/conversion sites, selected field/address inventories, ordinary position-setter slot alternatives, type declarations and shader enum identities. It does not redistribute binaries, assets, proprietary source, shader text or full disassembly. Private instruction inspection supplies stack/dataflow interpretation; public tools extract/check original evidence, never execute a substitute projection.

Competing explanations: raw map coordinates submitted unchanged versus forward layout mapping; sprite corner equals circle center versus origin compensation; full inverse identity versus conditional alignment with rounding/manager factors; viewport rectangle equals projection/scissor state versus distinct fields; API calls imply pixels versus unresolved shader/native execution. Trace the original face only, resolve its constructor/virtual setter, inspect the reached coordinate-mode branch, origin and bounds order, then the existing E09 buffer path and directly participating graphics-state methods. No familiar graphics transform, alternate engine or modern osu! formula is used as proof.

Mxxxx/Fxxxx/Txxxx denote EXE MethodDef0600xxxx/Field0400xxxx/TypeDef0200xxxx. **OpenTK tokens are assembly-qualified.** All offsets below are hexadecimal, relative to first CIL opcode, headers excluded; JSON stores decimal offsets. CONFIRMED refers to original static instructions/metadata. Semantic aliases, finite algebra and independent pseudocode are RECONSTRUCTED. Runtime state, reachability, target-JIT intermediates and pixels are UNRESOLVED.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/circle_projection_evidence.py /path/osubackup.zip > /tmp/projection-a.json
python re/tools/circle_projection_evidence.py /path/osubackup.zip > /tmp/projection-b.json
cmp /tmp/projection-a.json /tmp/projection-b.json
cmp /tmp/projection-a.json re/evidence/14/circle-projection.json
python re/tools/check_circle_projection.py /tmp/projection-a.json
python -m compileall -q re/tools
```

Evidence JSON: 281485 bytes, SHA256 `a9f355c55da681cc32816b21b3e7dfb6d9c5677c8c6ea1859071a20c7c1f3e83`. Actual final validation is recorded in the journal: deterministic extraction/public match, focused checks, all twelve prior E02–E13 checkers, Python compilation, historical-file/prefix/diff checks and remote parent/tree/branch/blob/mode verification. Cases below are manual conditional static predictions, not original-game observations or projection-model test passes.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M4518 | 1b6ef8 |3920|993a728c3a8989fb22fdeaf14ce0428543df2d1580bfb77f975857fcd6a35679 |
| M0ff5 | 64038 |37|28ba1a0dd79079e76703e6c9a86969bf06d50ca43f5a6082b225aae7307fbd46 |
| M1e46 | bb9b4 |142|f4c7b7ce61b58508648bb6c0abc77e2dd1a55f02c78f4996b71205de5a8dc204 |
| M52ca | 1f465c |76|9c8fd2f5bad606181100524afd1a4f3f887a86e4c2f6dc2544cbbf7537ee2ce4 |
| OpenTK M092c | 22fac |158|855c41c8dd913fa495729dce34ac50d048e2d67eaf4cb6cc07b8bf37d80d7d3a |

# 2. What they found

## What this means to an osu! player

Stable positions the circle face by taking its stored drawable position, scaling it by the playfield height divided by384, then adding the playfield offset. **Those are the same scale and offset used in reverse by the ordinary cursor mapping.** For a steady layout and an unchanged circle/face position, the two CPU coordinate paths are compatible. The circle's face is centered using its sprite origin; its top-left corner is not the circle center.

That compatibility has conditions. Object and drawable positions are separate fields, presentation managers can apply additional scale, and an original rounding branch can move the submitted anchor slightly. Input and drawing also sample state at different moments. A successful hit selection therefore does not prove that a displayed circle was exactly under a displayed cursor.

The recovered drawing state includes a matrix with an inverted Y basis, a viewport rectangle and separate clipping state. The matrix can reach an original uniform upload, and textured vertices reach indexed drawing. However, the selected shader source, reflection-resolved offsets, linked uniform/attribute state and actual native display conditions have not been verified. We establish submitted geometry and its intended matrix handoff, **not actual screen pixels or equality between the visible circle edge and hit radius**.

## E14-A — Object center, face identity and later movement

CONFIRMED M45ec stores supplied T0a82 position in object.F252c at009 and separately F152e at010. Its manager/presentation null gate returns before visual allocation. The ordinary face constructor call at252 supplies resource, coordinate mode1 at239, origin selector1 at23a, clock1 at23b, **current F252c at23d**, ordering helper result, lifetime=false and colour. Face reference is stored in F2bf9 at25e and registered through the E08 collections/presentation path.

M4510 stores coordinate mode in F2b78 at067, supplied position in **F2cf6 at034**, copies it to F2cec at040, and initializes scalar scale1. Base M4728 initializes vector scale F2cf8=(1,1) at011. Thus initial object center and face position have equal values, but are **not one shared live position field**. M4518 does not read the parent object's F252c afresh on each draw.

The ordinary virtual position setter is **M45f2**, reusing M24b0's exact slot on T094c. It first calls base M24b0 at003, which writes F252c at002 and conditionally F152e; it then enumerates F1531 and stores the supplied position into each registered drawable.F2cf6 at01e. No position-transform recomputation or atomic transaction wraps these writes. Other concrete object setter overrides are preserved in the evidence, not generalized to circles.

F2cf6 also has generic presentation writers, including Move-related branches of M4519 (023b/08b5/0bb5 whole-vector stores and component address writes), resource/sprite helper methods and other callers in the full inventory. The untouched E08 normal face carries Fade/Colour, not a Move transform. A direct object-field mutation without the ordinary setter, a later drawable transform, a collection omission or an interleaving can separate the two centers. These are conditional dependencies, not claims of naturally observed misalignment. No broad movement/skin system is reconstructed.

## E14-B — Exact forward map, size factor and bounds preparation

At M4518:3c8–3cd copy the face's current F2cf6 into temporary field **F2b6e**. Origin F2b7f is copied to prepared F2b8c at3d9; scalar F2cf4 is clamped by original Math.Max with0 into F2b8e. Coordinate mode1 follows:

| Original boundary | Reached operation |
|---|---|
| M4518:585 switch entry0→0601 | Mode1 scalar factor multiplies F2b8e by presentation.F1a60; store060f |
| M4518:639 switch entry0→09d9 | Load global layout F2c64 and address of F2b6e; call M0ff5 at09e4 |
| M0ff5:002/008 | Get s via M0fea=H/384f; call M4ef5 to multiply X/Y into the addressed Single-field vector |
| M0ff5:015/01a/01f | Get O via M0fe6/F09ce; M4eee adds X/Y; store the vector through its address |
| M4518:09e9–09ec | Clear local flag controlling the later presentation-rectangle translation |
| M4518:cac–cbe | Compose prepared K=F2cf8*F2b8e into F2b8d |

Finite algebra: **B₀ = (drawablePosition*s)+O**, with original component stores after multiplication and addition. M4ef5 stores X at009,Y at017; M4eee stores added X at00f,Y at023. Drawing reads s first and O later; E13 input reads O first and s later. Neither path freezes layout state. `callvirt` at09e4 is not proof M0ff5 is virtual: metadata marks it nonvirtual, accepting a by-reference vector argument.

Mode2 instead reaches M0ff6 using the secondary offset; it is not the ordinary face's initial mode. Other mode switch branches and Edit/special components are not substituted. The manager's F1a6a-dependent pre-translation at3f4–51c can modify F2b6e before the mode map. Fresh M2cd8 leaves that flag false; ordinary constructor M289a→M2cd8 has no reached assignment making it true. Keep later clipping/transform changes conditional rather than assume every manager has immutable constructor defaults.

**Size is distinct from center:** M28be takes the E13 stored size D=F17f1, divides by static F17e9=128 (M289b:033–038), stores F17f0 at1c6 and presentation.F1a60 at1df. Thus ordinary initial face scalar1/vector(1,1) produces K=(D/128,D/128), before other manager adjustments. Resource dimensions still determine the quad extent. F17f1 alone is not a measured texture diameter, and hit radius uses the additional E13 factor k independently.

When presentation.F1a5f !=1 and drawable.F2b85 is false, M4518:cc3–d09 additionally scales position, and (except T0147) prepared K, by that manager factor. Fresh presentation constructor sets F1a5f=1 at02e; another writer exists. Preserve this factor g rather than silently equate every presentation with the input layout. The mode1 local flag skips the later F1a73.X/Y addition atd10, but does not cancel this separate scale.

### Origin, optional rounding and the true submitted anchor

Face origin selector1 is centered: M4524 obtains logical resource dimensions (receiver dimensions integer-divided by resource multiplier Dtex); M4525:042–062 sets origin=(r4(logicalWidth)/2f,r4(logicalHeight)/2f). Logical source extents and crop fields remain separate. No assumption about actual skin dimensions is made.

After the above position/scale composition, call the resulting position **B**, prepared scale K and prepared origin o. M4518 preserves this order:

1. Set F2b6f=false atd48.
2. If nullable F2b77.HasValue, use its contained bool atd66. Otherwise auto-enable only when K.X==1, K.Y==1 and DistanceSquared(previous F2b6e snapshot, current B) is ordered `< float.Epsilon` (literal1.401298464324817e−45; bge.un atda3 skips). Previous value was saved at01f–025 **before** the fresh position copy. This is a history-dependent predicate, not simply “all circles snap.”
3. Compute **a₀=(o.X*Abs(K.X),o.Y*Abs(K.Y))**, store F2b8f atdea.
4. Compute top-left-like **L=B−a₀** through vector subtraction atdfb.
5. If F2b6f, apply Math.Round(r8(L.X/Y))→conv.r4 individually (e14/e19 and e29/e2e). It rounds L, not object.F252c. Native CLR midpoint/precision behavior was not executed.
6. Construct float bounds **F2b81=(L.X,L.Y,r4(logicalWidth)*Abs(K.X),r4(logicalHeight)*Abs(K.Y))**, storee78.
7. Retain clipping/source/callback/status checks through method exit.

M451a then adds **F2b81.X/Y + F2b8f** at0c4–0f4 to form receiver argument **P**. Without rounding and for normal finite values, ideal P=B. With rounding, P=Round(B−a₀)+a₀: the original origin compensation does not remove the rounding displacement. Floating field stores can also prevent bit-exact algebraic cancellation. F2b81's only direct whole-field writer is M4518:e78; F2b8f's is M4518:dea, with address readers recorded separately. Indirect/reflection/native mutation remains outside this inventory guarantee.

M451a preserves E09's resource multiplier order: divide K by r4(Dtex), multiply source Width/Height/X/Y by Dtex, multiply o by Dtex. This normally preserves finite extent/anchor algebra, but integer dimension truncation and original zero/nonfinite operations cannot be cancelled away. The original submitted P is computed from bounds and a₀ prepared before that resource adjustment.

## E14-C — Quad center and the existing batch boundary

CONFIRMED E09 continuation retained: M451a:172→virtualM31f5; fresh successful null-atlas cache miss selects T055e/M2929. A cached region T017a/M0a3d delegates to its backing texture with altered source region and unchanged P/o/K/rotation. Actual receiver/cache/resource identity remains runtime-dependent.

M2929 first requires readiness M292d. It computes h=r4(source.Height)*Abs(K.Y), then w=r4(source.Width)*Abs(K.X); adjusted origin is **a=(o.X*w/r4(source.Width),o.Y*h/r4(source.Height))**. Four local corners subtract a, optionally rotate with the original Sin/Cos helper, then add P (first addition12f). Position is passed into TexturedVertex2d.Position/F3634, not into a top-left-only API.

For finite nonzero full-source extents, zero rotation and centered o, the geometric quad midpoint is **P+(w/2,h/2)−a**, ideally P. With noncenter origin or source cropping/region dimensions differing from those used for o, it need not be P. This midpoint is a RECONSTRUCTED geometric interpretation, not a measurement of the texture's opaque center. Transparent padding, asymmetric image content and sampling can move the apparent pixel center without changing these vertices. Negative scale flips UV orientation in M2929; it uses absolute geometry sizes, not a geometric center reflection.

Four vertices append BL/BR/TR/TL to E09's batch. Capacity/state changes can flush inside or around a face call; otherwise later M2d00 flushes. T077d/T0826.M3e33 binds then calls original OpenTK DrawElements at026, mode4/index type5123 and original count/offset arithmetic. No new per-circle drawing engine or shader is inferred. The recovered vertex/matrix/viewport paths below are state dependencies of that operation, not proof of GPU completion or pixels.

## E14-D — Attributes, original projection matrix and uniform boundary

**CONFIRMED vertex metadata:** T0b26 is named `TexturedVertex2d`, with Position T0a82, Colour T0a78 and TexturePosition T0a82. Its M52ca `Bind` asks M17aa to enable3 arrays, then calls VertexAttribPointer:

| Index | Components / original type constant | Normalized | Stride / offset | Callsite |
|---|---|---|---|---|
| 0 |2 /5126|false|F363a /F3637|M52ca:018|
| 1 |4 /5121|true|F363a /F3638|M52ca:02f|
| 2 |2 /5126|false|F363a /F3639|M52ca:046|

M52c8 initializes offsets via Marshal.OffsetOf(T0b26, M49b0(identifier)) for identifiers2064460041,2064460036,2064460051; stride via generic Marshal.SizeOf. T0826.M3e28 obtains its generic vertex type's static field by decoded identifier2064460611, then creates an Action delegate from a method selected by identifier2064460636, binding flags24. M3e2d invokes that cached delegate at017 only when M1e49 reports a changed vertex-buffer bind. E09 resolves the ordinary buffer's generic vertex type to T0b26. **Successful reflected binding is conditional:** decoded field/method names, actual offset values and cached GL attribute state were not executed or independently resolved here. The public evidence records original field types and offset-producing calls; it does not manufacture numeric stride/offset values or silently label each array by an assumed shader variable.

M4b17 binds program attribute indices0..4 to decoded names at09a/0b0/0c6/0dc/0f2 before LinkProgram at0fd. These name identifiers remain opaque; a known attribute index and an uploaded position do not by themselves prove shader position semantics.

### Matrix preparation is directly recovered

M1e46 stores projection rectangle **F1176**, distinct from viewport F1175 and scissor F1174. With no supplied rectangle it constructs(0,0,dimension.F0e6d,dimension.F0e6e); with a supplied one it uses that rectangle without the viewport helper's extent-normalization pass. It calls **OpenTK.Matrix4.CreateOrthographicOffCenter** with:

`r4(Left), r4(Right), r4(Bottom), r4(Top), −1f, +1f`

Sites039/044/04f/05a convert the rectangle edges,060/065 supply depths,06a calls the library. It stores returned Matrix4 in **F116e at06f**, boxes it and calls M4b1a with decoded key identifier2064460275 at088. M1e46 is the only direct F116e writer. Callers also exist in offscreen preparation helpers; do not assume a past default call fixes the matrix forever.

The pinned library return overload **OpenTK M092d** delegates at00a to by-ref **OpenTK M092c**, whose own instructions establish:

- Initialize result from Matrix4.Identity.
- rx=1f/(right−left), ry=1f/(top−bottom), rz=1f/(far−near), in that order.
- Row0.X=2f*rx; Row1.Y=2f*ry; Row2.Z=−2f*rz.
- Row3.X=−(right+left)*rx; Row3.Y=−(top+bottom)*ry; Row3.Z=−(far+near)*rz.

Here “bottom” and “top” are library parameter names, while the original caller deliberately supplies rectangle.Bottom then rectangle.Top. For a finite default rectangle(0,0,w,h), this produces **Row0.X≈2/w, Row1.Y≈−2/h, Row3.X=−1, Row3.Y=+1** under the original arithmetic. That is the exact identified **inverted Y basis**. It is not a prior CPU-coordinate `384−y` mutation of the ordinary face or mapped cursor. Matrix row layout/upload/shader multiplication must not be conflated.

### The guarded uniform connection and precise stopping point

M4b1a writes the key/value into global F2f0f at007, enumerates existing programs, and calls their uniform collection M3235 only when the key exists. M3235 requires program-ready F2f08, key presence and compatible value type (original Convert.ChangeType/catch otherwise); unchanged Equals can return. It stores F12fa at066, invokes M2161 immediately only if program-active F2f09, otherwise marks collection dirty. M4b18 requires ready/not-active, calls UseProgram at018, updates tracked program state, invokes dirty collection M3236 at039, then marks active. M3236 visits values and invokes M2161.

M2161 returns for null value, dispatches by original uniform type, and for type35676 reaches the Matrix4 branch: unbox Matrix4 at135; load uniform location, **false transpose at141**, by-ref matrix; call **UniformMatrix4 at144**. This is a verified conditional upload path for the prepared matrix, not proof that the selected program exposes the decoded key with type35676 or uses it to compute gl_Position.

Host M46a6:073–078 selects global program **F116f** before E08's Play draw. Its sole direct initialization M1e41:118→11d calls M4442(0,0,false). Metadata identifies vertex enum T071b value0=`Texture2D`, fragment enum T0502 value0=`Texture`. M4442 calls M4441 with those enum names and shader types35633/35632, then constructs/links a T0a1d program. M4441 normalizes the resource key via M4440, may return cached shader F2af1, otherwise obtains bytes through **osu_ui.ResourcesStore.ResourceManager.GetObject** at031 after replacing '.' with '_', constructs M091b and invokes compile M091d.

**UNRESOLVED blocker:** the exact selected/cache shader bytes, decoded resource/uniform/attribute/reflection names, compiled/linked state, active uniform locations/types, offset values and native GL context/backend are not established. Other original rendering work can temporarily bind programs; an early host program selection is not an immutable per-face guarantee. Recovering all shader/resource/native state would exceed this bounded integration. Stop here. No assumed vertex shader or conventional projection is substituted.

## E14-E — Viewport, scissor and comparison with input

CONFIRMED M1e45 with supplied rectangle first stores F1175 at013, normalizes negative width/height by moving X/Y then negating extent, and calls original **GL.Viewport(Left,Top,Width,Height)** at0ba. This uses Rectangle.Top as the API Y argument; no client-height subtraction occurs at this callsite. Generated-rectangle branch derives its origin from host dimensions/settings and F2c9e, stores F1175 at186 and calls Viewport at1b3. It then calls **M1e46(null) at1c1 on that generated branch only**, followed by M1e44(currentViewport) at1d0 on both branches. A supplied viewport change therefore **does not automatically rebuild projection in this method**. F1175's only direct whole-field writer is M1e45, with in-method address mutations preserved.

M1e44 manages **F1174/scissor**, normalizing a local rectangle and comparing it with viewport F1175; it conditionally enables/disables capability3089 and invokes Scissor at0ee. M2cf0 prepares a presentation clip rectangle before each active drawable, can flush on a clip change, and derives native clip coordinates using viewport.Left/Top plus dimension height and clip bottom (383–3c8,496–4db). Clipping rejects or limits drawing; it is not an extra position translation. The face also has M4518's clipping/status checks. A valid computed center can still be clipped away.

E13 source route uses **J=(viewport.Left,hostClientHeight−viewport.Bottom)**, with integer arithmetic in M4695 and subtraction from PointToClient result in M1196. That is the viewport's client-top-origin adjustment on the traced provider route. Actual provider selection/DPI/sensitivity remains unknown; recorded input bypasses M0ff1 at M2295, and special input/ruleset paths remain distinct.

The established CPU relation for ordinary mode1, g=1, no pre-translation/rounding, drawablePosition=current object center C and stable finite nonzero s is:

| Path | Original operation-level projection |
|---|---|
| Drawing | B=storeVector(storeVector(C*s)+O) |
| Input | P=storeVector(storeVector(Q−O)*(1f/s)) |
| Hit test | ordered DistanceSquared(P,C)<=R*R after independent time/state gates |

These are inverse **finite algebra**, not guaranteed bit-exact inverses after reciprocal arithmetic and field rounding. Rounding, resource/source origin effects, manager g, changed drawable position and state interleavings add independent factors.

**Conditional projection interpretation, not a recovered shader:** if the selected original shader consumes the prepared matrix and position in the corresponding orthographic way, and native viewport maps those normalized coordinates to its supplied rectangle, then default projection w/h and viewport w/h yield viewport-local top-origin geometry B and client geometry J+B. Under those explicitly missing conditions, the E13 client→J subtraction and layout inverse align with the face's quad center. For mismatched projection/viewport extents, the conditional client geometry scales B.X by viewport.Width/w and B.Y by viewport.Height/h before adding J. This equation is a future discriminating prediction, not proof of shader behavior or a projection simulator.

**Successfully selecting a circle does not guarantee pixel alignment:** timing/selection can succeed with invisible alpha, absent resource, stale layout, changed face position, downstream clipping or a different shader/context. Conversely visible texture pixels do not establish the selectable radius. E13's R includes k=1.0004099607467651f, while the face's extent depends on resource dimensions and the size factor D/128. Texture padding/border, origin and sampling remain separate.

## Interface and controlled static reference predictions

Inputs/state: object C=F252c; face identity/F2cf6/F2b78/F2cf8/F2cf4; resource logical dimensions/source/origin/Dtex; presentation g/F1a60/F1a6a/clip/opacity; previous prepared position; nullable rounding override; separately sampled layout O/H; viewport F1175/projection rectangle F1176/matrix F116e/scissor F1174; program/cache/uniform/attribute state and E08/E09 readiness gates. Outputs: F2b6e, prepared K/o/a₀/bounds/P; CPU quad vertices; original attribute/uniform/viewport/scissor/draw invocation opportunities. No pixel oracle or production interface is supplied.

**Conditional field fixture, not natural gameplay/captured pixels:** exact T094c at C=(256,192), untouched non-Edit Osu face coordinate mode1, no Move/filter, g1/F1a6a=false, permitted gates, size input5 and E13 consistent dimensions640×480/q1 → s1,O=(64,56), stored size D64, F1a60=0.5f. Supply artificial logical resource128×128, centered origin(64,64), full source, Dtex1, zero rotation, vector/scalar1, rounding false. All expected numbers below refer to CPU geometry unless specifically conditional on shader/native state.

| ID | Probe | Conditional static prediction |
|---|---|---|
| P01 | Centered fixture | B/P=(320,248); quad BL(288,280),BR(352,280),TR(352,216),TL(288,216). Center=(320,248), not top-left. |
| P02 | Provider-local Q=(320,248), same stable layout | E13 maps to(256,192), spatial center pass if other gates permit. No displayed cursor/circle claim. |
| P03 | Consistently double dimensions1280×960/q1 | s2,O=(128,112),D128,K1; B/P=(640,496), quad half extent64; Q=(640,496) maps to C. Auto-round may become eligible after matching previous position, but this integral fixture is unchanged. |
| P04 | Add8 to layout O.X only | Submitted anchor shifts+8X; same Q now maps8 left of C at s1. Shifting Q+8 restores the CPU center relation. Artificial offset mutation; no ordinary UI setting assumed. |
| P05 | Subtract4 from O.Y only | Anchor shifts−4Y; same Q maps4 below C at s1; Q−4Y restores relation. |
| P06 | Keep viewport extent but shift stored/API origin by(10,20) | CPU B/P unchanged; provider J changes(+10,−20) with fixed client height. Conditional client geometry follows J+B only if shader/native mapping conditions hold. |
| P07 | Change viewport width only through supplied M1e45 rectangle | This call does not rebuild F116e; input layout need not change. Conditional viewport/projection mismatch introduces a scale factor; actual pixels unresolved. |
| P08 | Circle's virtual M45f2 setter moves C to(264,192), face in F1531 | Base center store precedes per-drawable stores; after complete normal return face position follows, anchor(328,248). Interleaving can observe different phases. |
| P09 | Directly change face.F2cf6 to(264,192), leave C unchanged | Draw anchor(328,248); input hit center remains C. This is an artificial state probe, not observed gameplay. |
| P10 | Change C directly but omit face update | Selection moves with current F252c; unchanged face draws at old mapped location. Ordinary setter is required to infer synchronized movement. |
| P11 | Logical resource64×64 instead of128×128, centered origin | K remains0.5; quad half extent16 rather than32, midpoint still P in finite ideal/full-source arithmetic. Hit radius unchanged. |
| P12 | Logical128×128, force origin(0,0) | Quad midpoint P+(32,32); P becomes top-left anchor. Not the ordinary face constructor origin. |
| P13 | Resource multiplier2 with compatible logical/full-source extents | Receiver scale halves, source dimensions/origin double; finite ideal center/extent retained. Original int division/stores still apply. |
| P14 | Logical odd dimensions127×125, centered origin | Origin(63.5,62.5), half extent(31.75,31.25) at K0.5; no unconditional integer-center cast. Texture opaque center remains unknown. |
| P15 | Force rounding true, B=(320.25,248.25), a₀=(32,32) | Bounds round(288.25,216.25) to(288,216); submitted P=(320,248), displacement−0.25 each. Forced field fixture. |
| P16 | Same fractional B, force rounding false | Bounds(288.25,216.25), P=(320.25,248.25) under exact finite fixture arithmetic. |
| P17 | Nullable rounding unset, K=(0.5,0.5) | Automatic branch fails scale equality; no auto-round regardless of previous-position test. |
| P18 | Nullable unset, K=(1,1), previous prepared position matches B, finite exact distance0 | Automatic round enabled because0<float.Epsilon. If position differs or comparison unordered, automatic branch skipped. |
| P19 | Manager g2 with original F2b85=false and ordinary face type | Additional position and K scale; no matching g factor in M0ff1. Controlled field mutation does not establish normal settings. |
| P20 | F2cf5=0 | E08 float gate fails on ordinary path; no vertices merely because center is valid. Retain manager forced-processing flag as separate branch. |
| P21 | Positive partly faded alpha, permitted gates | Position map unchanged; colour/quantized alpha changes. Receiver has no added center correction from alpha. |
| P22 | E08 fade≈0.0025, manager alpha1 | Float gate can pass, byte alpha0; permitted downstream route can append geometry carrying zero alpha. No pixel visibility inferred. |
| P23 | Disabled drawable, missing/disposed resource, null receiver or texture readiness false | Stop at the corresponding original gate; prepared/selected circle does not force a draw. M451a true can still mean no receiver call. |
| P24 | Valid geometry outside current clip | M4518/status or native scissor can suppress/limit output; no change to input radius follows. |
| P25 | Draw uses O=(64,56), input later reads O=(72,56), s1 | Draw anchor320,248; Q at that anchor maps(248,192), not C. Different snapshots are sufficient for CPU mismatch. |
| P26 | M0ff5 samples old s, then new O; input samples reverse combination | Neither result need equal a transform from one complete layout epoch. No atomic snapshot/lock was proved. |
| P27 | Default projection rectangle0,0,640,480 | Original matrix Row1.Y is negative, Row3.Y=+1; no CPU 384−y occurs in ordinary face mapping. Actual shader multiplication unresolved. |
| P28 | Projection rectangle has nonzero Left/Top or differs from viewport | M1e46 uses its exact edges; do not reuse default normalized-coordinate equation. No actual screen prediction without shader/native state. |
| P29 | Vertex receiver reached, reflected bind or expected uniform absent | Matrix/geometry existence does not establish a working attribute or projection connection; stop at precise dependency. |
| P30 | Actual shader/cache/context unknown but DrawElements instruction reached | Report CPU vertices/API invocation only. GPU completion, capture center, displayed border and pixel equivalence remain UNRESOLVED. |

Single fields, explicit conversions, original Math.Round and stack order are confirmed. Target32-bit CLR/JIT internal precision, spills/inlining, NaN/overflow/out-of-range/native behavior were not executed. Do not implement these cases as a forced-binary32-every-op simulator or use Python double arithmetic as reference. Degenerate zero source dimensions retain M2929 division; no invented sanitization or geometry/pixel prediction is made.

## Ordering, limitations, next dependency and Windows experiment

Input's stored provider sample can precede key polling/queued action; M2295 maps when consumed. Candidate selection, action admission and classification use separate state/time reads (E11/E12). Draw later follows E08 queue/evaluator→M4518→M451a→M2929→batch; layout and resources can change between those points. E12's corrected post-refresh update ordering remains authoritative. Presentation lock in M2cf0 is not proof of an immutable shared layout/provider/object/resource snapshot. Field-address operations and native calls are not assumed atomic or side-effect-free.

The smallest remaining **pixel-proof** dependency is observing the exact one-face program/reflection state and matched submitted geometry/capture on the pinned original. Static continuation, if separately commissioned, would resolve only Texture2D/Texture shader bytes and their position/matrix/attribute relationship. This expedition stops without that larger resource/shader investigation. The user's intended next gameplay priority can now proceed to **ordinary slider-head input and held-slider tracking**, carrying inherited circle spatial/timing rules as evidence, not assuming identical parent tracking behavior.

Minimal original-Windows experiment, **NOT RUN**:

1. Pin untouched EXE/archive hashes, actual Windows/GPU/backend/DPI/window/client dimensions, viewport, layout, mods/config/skin/resource hashes and selected ordinary types. Use unchanged E01 fixture's first circle: mapSHA256 dc4112acfcb0e7f345c6350a0cf1640d74f7acdc6eb2ae77047023b09e330541, WAV c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977; import remains unverified. A one-circle derivative must be separately hashed, not called the unchanged fixture.
2. Resolve tokens/offsets to actual JIT locations. Log one face: C, F2cf6, mode, O/H/s and read order, manager factors, logical/source/origin/Dtex, rounding decision, F2b6e/F2b81/F2b8f, M451a P, four appended vertices. Log projection/viewport/scissor/program identity, actual shader hashes, uniform key/type/location/value and attribute offset/stride/buffer state at the scoped draw. Record thread/order/QPC without extra clock/provider/layout calls.
3. Log OS/client/provider/stored pointer and original M0ff1 result plus actual M24c7 operands/result. Fresh circle per key probe near start; center/cardinal points first. Compare scale1 and one consistent doubled layout/window change; then viewport origin. Forced mismatched snapshots/origin/rounding probes are explicitly artificial and separate.
4. Capture timestamped original frames plus matched no-object background. Measure circle-face opaque-content centroid/bounds separately from submitted quad midpoint and separately from cursor pixels; hide/isolate overlays only under separately recorded conditions. Native API trace and submitted vertices are not capture evidence. Preserve capture scaling/DPI and instrumentation overhead controls; debugger logging can perturb state and floating spills.

Owner-reported distorted-slider stutter remains an unmeasured future objective. Loading geometry/visual construction, draw-time state evaluation/coordinate composition, command preparation, flush/backend invocation and measured cost stay distinct. No frame-spike cause, caching benefit or pre-rendering optimization is inferred or implemented.

# 3. Pseudocode

Independent non-executable research descriptions. RECONSTRUCTED projections of CONFIRMED static instructions, preserving original conversions/order and opaque guards. They are not replacement graphics components.

## Object-to-face state and ordinary position map

```text
RECONSTRUCTED — M45ec / virtual M45f2
object.currentCenter = suppliedPosition             // F252c
if original manager/presentation guard permits:
    face = original sprite(resource, coordinateMode1, origin1, clock1,
                           object.currentCenter, ...)
    face.position = suppliedPosition               // F2cf6, independent value
    register original face reference via F1531/presentation
on ordinary virtual setPosition(p,flag):
    direct base M24b0(p,flag)                       // center first
    for drawable in F1531: drawable.position = p    // then separate stores

RECONSTRUCTED — M0ff5, ordinary face forward map
multiply input vector X,Y by sampled M0fea() = H/384f,
    storing component Single fields through original by-ref helper M4ef5
obtain O through M0fe6 afterward
add O through M4eee; store returned Single-field vector
// E13 M0ff1 instead subtracts sampled O, then multiplies by 1f/sampledScale.
// Finite algebra inverses only; separate reads, stores and reciprocal matter.
```

## Prepared anchor and quad midpoint

```text
RECONSTRUCTED — scoped M4518 ordinary mode1, after original gates/evaluator
oldPreparedPosition = F2b6e
B = face.F2cf6; o = original prepared origin
retain manager pre-translation when original flags require it
scalar = Max(face.scalarScale,0f) * presentation.F1a60
    // F1a60 prepared from stored size F17f1 / 128
M0ff5(ref B)
skipLaterRectangleTranslation = true
K = face.vectorScale * scalar
if presentation.g != 1 and not original drawable flag:
    if not T0147: K *= presentation.g
    B *= presentation.g
roundFlag = false
if nullable roundOverride.HasValue:
    roundFlag = roundOverride.Value
else if K.X == 1f and K.Y == 1f and
        DistanceSquared(oldPreparedPosition,B) ordered< float.Epsilon:
    roundFlag = true
anchorOffset = (o.X*Abs(K.X),o.Y*Abs(K.Y))           // F2b8f
L = B - anchorOffset
if roundFlag:
    L.X = conv.r4(Math.Round(conv.r8(L.X)))
    L.Y = conv.r4(Math.Round(conv.r8(L.Y)))
bounds = (L.X,L.Y, r4(logicalWidth)*Abs(K.X),
                   r4(logicalHeight)*Abs(K.Y))      // F2b81
retain original clipping/source/callback/status work

RECONSTRUCTED — M451a / M2929, reached only through original resource guards
retain original resource multiplier adjustment of K/source/o first
P = (bounds.X+anchorOffset.X, bounds.Y+anchorOffset.Y)
    // F2b8f anchorOffset remains as composed by M4518, not Dtex-adjusted here.
retain receiver/cache/filter selection
receiver.M31f5(P,o,colour,K,rotation,source,batch)
texture receiver, if ready:
    h = r4(source.Height)*Abs(K.Y)
    w = r4(source.Width)*Abs(K.X)
    a = (o.X*w/r4(source.Width),o.Y*h/r4(source.Height))
    subtract a from four corners; optionally original rotation; add P
    append original textured coloured vertices BL/BR/TR/TL
// Finite zero-rotation geometric midpoint is P+(w/2,h/2)-a.
// Centered full-source origin gives ideal midpoint P, not necessarily opaque center.
UNRESOLVED: actual resource content, JIT edges, shader/native/displayed pixels
```

## Projection and viewport state handoff

```text
CONFIRMED instruction-level projection — M1e46 / pinned OpenTK M092d→M092c
rect = supplied rectangle or (0,0,dimensionWidth,dimensionHeight)
F1176 = rect
matrix = original CreateOrthographicOffCenter(
    r4(rect.Left),r4(rect.Right),r4(rect.Bottom),r4(rect.Top),-1f,+1f)
    // Original library reciprocal/subtract order and row stores recorded above.
F116e = matrix
M4b1a(OPAQUE_DECODED_KEY_2064460275, boxed matrix)
    store global key/value
    for registered program exposing key: original M3235(value)
        ready/type/equality gates; store value; upload if active, else dirty
on program bind: UseProgram; dirty collection M3236 invokes M2161
if reached uniform is nonnull Matrix4 type35676:
    UniformMatrix4(location,false,ref originalMatrix)

RECONSTRUCTED — M1e45 viewport boundary
if supplied rectangle:
    store F1175; normalize negative extents; GL.Viewport(Left,Top,Width,Height)
else:
    retain original host/settings-derived rectangle calculation
    store F1175; GL.Viewport(Left,Top,Width,Height)
    M1e46(null)                         // generated branch only
M1e44(currentViewport)                  // separate scissor state

UNRESOLVED — final integration boundary
resolve original reflected vertex offsets/bind delegate and actual GL array state
resolve exact original Texture2D/Texture cached shader and linked program
verify it consumes this matrix/position, then actual native viewport/presentation
compare submitted geometry with original capture; do not manufacture pixel proof
```
