# 1. Result

Expedition 09 — Circle-Face Drawing Receiver & Batch Boundary. 2026-10-27 (Asia/Bangkok). Work 1, original-software investigation only.

## Objective, scope and provenance

Recover one ordinary osu!standard circle face's resource receiver, original M31f5 dispatch, immediate vertex/batch continuation and earliest precisely established backend invocation. The fixed conceptual control is a single untouched circle at map position(256,192), start5000, ordinary base Play, non-Edit, Hidden off, no Blur transformation and valid on-screen presentation. This is a static path investigation; this map was not played or captured. Concrete resource bytes, settings and actual screen-space mapping are not invented.

Read the complete standing contract, cumulative contexts and relevant Expeditions 01–08/evidence. Preserve E05's correction: ordinary M113e→M0651→M48de, circle M48df→M45eb/M45ec; E04's superseded Taiko attribution is not reused. Work 2 remains suspended. No reconstructed components, graphics prototype, optimization or changes under `reconstructed/`; Work 2 was not inspected. No authorised exception.

Starting remote commit `5defae75064b469fb4e876d674fe69d171b7c864`, tree `0340cd2915cc2926a4b12dce8008d0cb42bb5b29`, verified through GitHub. Dedicated branch `work1/expedition-09-circle-drawing` was created directly from that commit. Workspace maintenance removed the first local checkout and private exploratory files during the investigation. Re-cloning recovered the exact E08 commit/tree; supplied inputs remained available. The final evidence was freshly extracted from those inputs. Published history is preserved, and contexts receives only an append.

## Inputs and evidence standard

| Input | SHA-256 |
|---|---|
| osubackup.zip | 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 |
| osu!.exe | 6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d |
| OpenTK.dll | b415f3e061d9758316074dcbf31d6dba48cb0b89405254db94ead0e43ed88511 |
| Instruction.md | bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9 |

Same self-declared Stable40/file-version1.3.3.8 baseline. Exact official dated release, authenticity and modification status remain UNRESOLVED. Linux static Python analysis; current host probe found no Wine/dotnet/DISPLAY or `/dev/dri`. No original executable/CLR method, Windows instrumentation, native function, screenshot, GPU completion or performance measurement was executed.

Created this report, `tools/circle_draw_evidence.py`, `tools/check_circle_drawing.py`, `evidence/09/circle-drawing.json`, and an append to `contexts.md`. The read-only extractor reuses unchanged E04/E05 collectors, records72 EXE methods and2 small OpenTK wrappers, exact method identities/hashes, token/constant/branch/arithmetic/null sites, receiver descendants/virtual metadata, generic member declarations, receiver-field accesses, construction callers and native-call signature. No binaries, copyrighted assets, original source or full disassembly are published. Private instruction inspection supplies stack/dataflow interpretation; the public collector checks reproducible facts without executing a replacement renderer.

Unless explicitly prefixed **OpenTK**, Mxxxx/Fxxxx/Txxxx are `osu!.exe` MethodDef0600xxxx/Field0400xxxx/TypeDef0200xxxx. All offsets are hexadecimal and exclude CIL headers. Identical numeric tokens in OpenTK are a different namespace. CONFIRMED refers to original static instructions or metadata; semantic aliases and independent pseudocode are RECONSTRUCTED. Runtime reachability, natural values, costs and pixels remain unobserved.

| EXE method | RVA | Code bytes | Method-body SHA-256 |
|---|---|---:|---|
| M38b5 | 163868 | 883 | d94677e4feaa3b188e5b4b1a1407a5a95f8d63c03e46af77232f08c39a14fbe4 |
| M0a3d | 388f0 | 150 | 600aae3a4aa3be6cfb866fc182f3f8a86e0713bc632e1a9749a4be4b1a9753e9 |
| M2929 | 10eee0 | 737 | 6d28e6523e24e2efa901353fc8d4589494d92042ac10c9f0ac400fa2e42ee8aa |
| M0e16 | 52514 | 184 | 1042171ef323e670f7dfaad3300e186fafc9721fbd1fb1038d4a372c9a54fd30 |
| M0e17 | 525d8 | 115 | c71cc0f8d073ffb338321f1e94dcbf6042bd7757f820efbbad17fb3d832e4e43 |
| M3e33 | 191574 | 50 | debfd8092a1115dddad0898ac942829264923b39733e5149a1cf1afc4594ba70 |

## Discriminating procedure and reproduction

Competing explanations: a unique ordinary receiver versus resource/cache-dependent receivers; a draw command object versus CPU vertex accumulation; each face immediately dispatches versus deferred/capacity-triggered flush; float alpha eligibility implies nonzero submitted alpha versus byte quantization; a managed DrawElements site proves visible pixels versus unresolved native/context/presentation state.

Investigate only the shortest chain: original face resource construction/getter, all direct managed F1764 writers, closed receiver ancestry and virtual slots, M451a's argument stack, two concrete implementations, batch append/flush and two OpenTK wrappers. Preserve opaque resource/filter/helper continuations. Full skin search, shader setup, complete graphics state, slider rendering and native implementation are outside scope.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/circle_draw_evidence.py /path/osubackup.zip > /tmp/draw-a.json
python re/tools/circle_draw_evidence.py /path/osubackup.zip > /tmp/draw-b.json
cmp /tmp/draw-a.json /tmp/draw-b.json
cmp /tmp/draw-a.json re/evidence/09/circle-drawing.json
python re/tools/check_circle_drawing.py /tmp/draw-a.json
python -m py_compile re/tools/*.py
```

Actual validation: two independently extracted outputs are byte-identical and match the public evidence; the new checker, unchanged E02–E08 checkers and Python compilation all PASS. Historical preservation and verified publication are recorded in the journal/final handoff. Checker assertions cover72 EXE methods plus2 OpenTK wrappers, hierarchy/writers, arguments/guards, four vertex appends, generic batch resolution and native boundary;14 overlapping E04–E08 body-hash comparisons. Full managed EXE scan:19,659 decoded bodies,1,739 zero-RVA declarations, zero errors. Selected branch targets are instruction-boundary checked. Reference cases below are manual static predictions, never original-game or surrogate-renderer passes.

# 2. What they found

## What this means to an osu! player

When the circle face is ready to draw, stable passes its prepared position, origin, size, colour/opacity, rotation and source rectangle to a texture receiver. The receiver builds **four textured, coloured vertices** and adds them to a batch. It does not create a separate circle-draw command object in this path. Those vertices can wait for a later flush, but filling the buffer can trigger a flush during the same call.

The next verified drawing operation is an indexed triangle draw through OpenTK. Its small wrappers reach an indirect native function pointer. We are therefore closer to explaining the circle's route toward pixels: the formerly abstract call now connects to concrete quad data and a backend invocation. We still have no observed screen pixels, and have not recovered the complete texture/shader/projection/context/presentation conditions that determine them.

A circle that passes the float opacity gate can submit vertices carrying alpha byte0. Conversely, even a valid resource can fail a later texture-readiness guard before any vertex is added. Timing eligibility, property evaluation, vertex accumulation, a flush, native invocation and visible output remain distinct facts.

**CONFIRMED static mechanism; RECONSTRUCTED player interpretation.** Ordinary osu!standard alone does not select one unique resource receiver for all cache histories. A **fresh cache-miss resource load with the face's null atlas argument** selects the direct texture receiver. A cache hit can return an existing resource, including a region receiver created through the alternate atlas path. The precise uncertainty is resource/cache history, not an excuse to substitute a guessed renderer.

## Face resource identity and concrete receiver selection

The corrected ordinary circle factory M48df constructs M45eb (`00d`), which reaches M45ec. Its face is F2bf9, allocated through M4510 (`252`) and stored (`25e`). The main resource comes from M38b4 with the virtual M45ed/fallback identifier path; ordinary M45ed returns opaque decoded-string identifier2064519436. Literal asset name/resource pixels remain unresolved. M4510 stores the returned resource in F2b7e (`011`); ordinary M451d returns that same field (`001`). This is the circle face, not the approach component or animated overlay.

M38b4 calls M38b5 with third argument **null** (`010–011`). M38b5's combined cache lookup can return an existing value immediately (`055–062`), before loading or using that argument. On its reached cache-miss loading paths, it forwards the atlas argument to stream/file/embedded loaders: M27e2, M27e1, M27e0/M27de, eventually M27e3. This expedition follows forwarding and receiver allocation, not the full skin/fallback policy. Null results, exceptions caught by those helpers and differing cache histories are retained dependencies.

| Original allocation | Receiver stored in resource.F1764 | Evidence |
|---|---|---|
| M27e3 with atlas=null | Direct T055e | Branch`028→034`; new resource M27ce at`036`; M27ce constructs M291d/T055e at`003` then calls M27cd at`009` |
| M27e3 with atlas nonnull | Region T017a over a T055e backing receiver | Atlas.M2c2b at`02d`; M2c2b allocates region M0a35 at`04b`, then resource M27cd at`051` |
| M27cd(receiver,atlas) | Supplied receiver, not a factory guess | Sole constructor store to F1764 at`016`; also records atlas reference F175f |
| Atlas backing creation/reset | T055e in atlas.F19e9 | M2c29 constructs M291d at`01f`, stores F19e9 at`024` |
| Region constructor M0a35 | Stores supplied region F05c2 and backing F05c1 | `008` / `00f`; no cloning of backing receiver |
| Resource disposal | Clears F1764 after original receiver.Dispose | M27da:`011–025`, preceded by F1762 guard/set |
| Cache-resource reload M38be | Replaces existing F1764 with newly loaded receiver or null | Transfer`08f–094`; null store`0c2`; old receiver may be disposed first |

Complete direct managed field inventory finds F1764 stores only in **M27cd, M27da and M38be**. F05c1's direct writer is M0a35, F19e9's is M2c29. This constrains original managed construction; it does not rule out reflection/native/external mutation. A resource cache hit is a reference-history dependency, not a fresh null-atlas allocation. M38be can rebind an existing resource object, so reference equality alone does not freeze receiver identity.

Metadata descendant scan finds exactly two concrete EXE descendants of abstract T067e, with no further descendants:

| Type | M31f5 override | Readiness slot M31f6 | Preparation slot M31f7 |
|---|---|---|---|
| T017a region | M0a3d | M0a3f | M0a3e |
| T055e texture | M2929 | M292d | M292e |

Both overrides reuse the abstract slot's exact name/signature; neither introduces a new slot. Relevant MethodImpl metadata is also included, so a same-name search alone is not the dispatch proof. The complete descendant list establishes the EXE candidates, not which one a particular unobserved Windows resource cache contains.

**CONFIRMED conditional selection:** fresh ordinary face loading, all relevant caches missing, M38b4 null-atlas argument forwarded normally and resource load successful → T055e/M2929. **UNRESOLVED unconditional selection:** real cache contents, reload history and decoded resource identity have not been observed. The smallest missing evidence is the runtime type of the exact face.F2b7e.F1764, plus whether its load returned from M38b5:`062`. The two statically justified paths are preserved below.

## Exact M451a call arguments and preceding guards

E08 already recovered M4518's property evaluation and composition. M451a retains these additional ordered operations:

1. Direct base M4737 returns false → M451a returns false (`001–009`). It retains the global membership/disabled gate; no receiver call is implied by an earlier update.
2. Ask current presentation manager M2cfe(F2b6b) to establish state (`015`); this can itself cross a batch-state/flush boundary.
3. Obtain resource via virtual M451d (`01b`). Null, or M27d7(resource) true, returns **true without drawing** (`022–02d`). M27d7 returns F1762 (`014`) and can update F1761 timestamp if F1765 (`001–00e`). Do not treat a true M451a result as a draw count.
4. Let D=resource.F1767. If D!=1 (`035`), mutate prepared scale `F2b8d *= 1f/r4(D)`; multiply prepared source Width,Height,X,Y by D using unchecked int arithmetic; multiply prepared origin F2b8c by r4(D) (`03c–0bf`). Order is scale, rectangle W/H/X/Y, origin. No zero/negative-D sanitization here.
5. Form position **P=(F2b81.X+F2b8f.X, F2b81.Y+F2b8f.Y)** (`0c4–0f4`). It is not simply the parsed map position.
6. If original resource.F1764 null, skip receiver call (`0fa–0ff`). Otherwise set that receiver's F1d31 to33071 when F2b6c false,10497 when true (`102–11b`). These are original numeric state values; region forwarding does not contain an explicit copy of this field to its backing receiver.
7. Select resource Q=original resource; if F2cfb>0, call M4077(resource,F2cfb,F2cfc) (`123–13d`). This filter/Blur route is outside the untouched normal face control; preserve it as opaque. The subsequent receiver comes from **Q**, not necessarily the resource whose F1d31 was just assigned. No extra null fallback is inserted after this helper.
8. Load Q.F1764 and the seven arguments, invoke virtual M31f5 at`172`. Optional accounting follows (`177–195`).

| Parameter | Exact source at M451a | Notes |
|---|---|---|
| Receiver | Selected resource Q.F1764 (`13f`) | Base resource can be replaced by the positive-filter helper |
| 1: position P | local Vector2 at`144` | Bounds position plus prepared offset, as above |
| 2: origin O | F2b8c (`146`) | Already D-adjusted if needed |
| 3: colour C | F2b6d (`14c`) | Contains E08's quantized alpha byte; passed by value |
| 4: scale K | F2b8d (`152`) | Already composed and D-adjusted |
| 5: rotation theta | F2cf3 (`158`) | r4 value; no conversion at this callsite |
| 6: nullable source rectangle R | F2b90 wrapped in Nullable (`15e–163`) | Always has a value on this caller's path |
| 7: batch B | Current presentation.M2cfd() (`168–16d`) | Existing selected batch reference, not a newly allocated command |

M4510 receives origin selector1 for the face. M4524 obtains logical dimensions from M27d4/M27d5: receiver width/height divided by D with **integer division**, sets full source extents and zero crop offsets. M4525 selector1 (`008→042`) sets F2b7f=(r4(logicalWidth)/2f,r4(logicalHeight)/2f), stored`062`. M4518 copies that into prepared origin F2b8c (`3d4–3d9`), subject to its remaining original branches. Its source rectangle at`ee5–f25` uses current crop offsets and Min(logicalDimension,Max(0,requestedExtent)); M451a then applies D. Odd dimensions/D therefore must not be simplified into an assumed exact texture-half operation.

For the untouched face, scalar1 and vector1 feed E08's size composition; alpha comes from its Fade and manager opacity. Normal default rotation/filter fields remain zero unless later writers intervene. These facts do not establish actual screen-space P, texture dimensions or shader-projected radius. Full coordinate transformations remain a separate dependency. Re-entering M451a without a fresh M4518 composition can repeatedly mutate prepared D-scaled fields; the recorded normal caller order matters.

## Region receiver: delegation, not a second vertex engine

**CONFIRMED M0a3d:** first invoke virtual M31f7 (`001`) and discard its bool. Copy own region F05c2. If source supplied, add source.X/Y to region.X/Y, set Width=Min(source.Width,region.Width), Height=Min(source.Height,region.Height) (`00e–076`). This is not a complete clipping intersection: the width minimum does not subtract source.X. It then invokes backing F05c1.M31f5 with unchanged P/O/C/K/theta/batch and this adjusted nullable rectangle (`07c–090`).

M0a3e can transfer pending region data and ask the backing receiver to prepare; its returned bool is not a universal draw gate. M0a3f similarly asks preparation then returns backing.M31f6. Do not turn either ignored preparation result into a new rejection rule. The backing object created by the atlas path is T055e, so drawing converges on M2929 with an adjusted source rectangle. No extra alpha calculation or fresh circle timing appears in the region draw wrapper.

## Texture readiness before quad construction

M2929 first invokes virtual M31f6 (`001`), returning immediately if false (`006–008`). T055e resolves that slot to M292d, which:

- Invokes virtual M31f7/M292e and **discards its bool** (`001–006`).
- Rejects when texture handle F1822 is not positive (`008–011`).
- Rejects when F1d30 is true (`013–01b`); keep this field's precise wider meaning unresolved.
- Calls M1e4a(handle) (`022`), then conditionally M292a when stored F1821 differs from requested F1d31; returns true.

M292e has its own F0423/M177c, F1d32, pending-data and level guards. It can perform texture-related GL work before vertex creation. Its bool must not be substituted for M292d's final answer: an existing positive handle with F1d30=false can still pass when preparation returned false. Conversely, handle0 remaining after preparation suppresses the quad even if M4518 returned draw status0. This expedition records the preparation boundary and guard operands, not a complete upload algorithm.

M1e4a compares the handle against static F1178. If different, it first flushes the current presentation batch when F1a78 exists (`008–019`), then calls OpenTK BindTexture (`024`) and stores the handle (`02a`). If equal, it returns without that flush. Thus a face's readiness call can flush **earlier accumulated work** before adding this face. This is distinct from submitting the face itself.

## Concrete quad data and numerical order

For successful readiness, M2929 uses the supplied rectangle (always supplied by M451a); its no-source fallback constructs(0,0,virtualWidth,virtualHeight). Let the resulting R=(x,y,w,h), texture storage dimensions TW=F181c,TH=F181b, and call arguments P,O,C,K,theta,B.

**CONFIRMED instructions, RECONSTRUCTED equations for finite ordinary values:**

- Compute H=r4(h)*Abs(K.y) **first**, then W=r4(w)*Abs(K.x) (`02f–059`).
- Compute adjusted origin `(O.x*W/r4(w), O.y*H/r4(h))` (`05b–07f`). Preserve multiply/divide rather than algebraically cancelling w/h: zero dimensions can change results.
- Initialize local corners TL=(0,0),TR=(W,0),BL=(0,H),BR=(W,H); subtract adjusted origin from each (`084–0e0`).
- When theta!=0 (`0e9`), calculate r4(Sin(r8(theta))) then r4(Cos(r8(theta))), and rotate each corner through M2928: x'=oldx*cos−oldy*sin; y'=oldx*sin+oldy*cos (`0eb–128`). Then add P to each (`12d–14d`). No angle conversion occurs here.
- Construct floating source UV rectangle `(r4(x)/r4(TW), r4(y)/r4(TH), r4(w)/r4(TW), r4(h)/r4(TH))` (`14f–18d`). Do not divide by logical resource dimensions instead.
- If K.y is ordered negative, set UV.y=UV.bottom and negate UV.height (`192–1b8`); if K.x ordered negative, set UV.x=UV.right and negate UV.width (`1bd–1e3`). `bge.un` skips these flips for unordered comparisons. Geometry uses Abs scale; signs alter UV orientation.
- If B null, substitute static F1827 (`1e8–1f1`). M2925 can initialize that fallback as T0854 with constructor arguments1,100. This does not prove it was initialized in an unobserved run.
- Append four **TexturedVertex2d/T0b26 values** in order **BL,BR,TR,TL**, each with Position, TexturePosition and the **same input C**, through MemberRef0a0010de at`229`,`265`,`2a0`,`2db`. UVs are(left,bottom),(right,bottom),(right,top),(left,top). No alpha>0 rejection occurs in this method after readiness.

Original floating-point operations/conversions and evaluation-stack behavior are retained; do not impose extra rounding at every multiply. Sin/Cos results explicitly convert to r4. Requested malformed numeric/resource states, overflow, exceptions and target-CLR excess precision are not verified by an executable numerical model. There is no invented screen-pixel clamp or visibility guarantee.

## Generic batch identity, accumulation and flush

The MemberRef class signature `151287cc0111ac98` is T01f3<TexturedVertex2d>. MemberRef0a0010de resolves to generic **M0e16**, not a native draw. Current presentation M2cfd chooses F1a81[F1a80]. M2cff's reached setup allocates concrete **T0854<TexturedVertex2d>** through MemberRef0a0010dd (`050`), passing twice `M2d7c(activeCount,1,100)` and500. The allocation arguments are retained without needing to reconstruct that helper here. M2cfb selects one or three batch slots based on F0429; M2cfc allocates that slot array. T0854 derives T01f3 and overrides buffer creation with M3f9b.

| Boundary | Confirmed action |
|---|---|
| M0e16 append | Ensure current ring-buffer object exists via virtual factory (`009`); compare old vertex using Equals (`047`), mark dirty start/end if different, then always store vertex into CPU array (`07e`) and increment write count F07d5 |
| Capacity boundary | If write count>=current array length, call M0e17 via MemberRef0a000807 (`0a2`), then reset counters; this can happen inside M2929's fourth append |
| M0e17 empty boundary | If write count F07d5 equals submitted count F07d6, return (`00c–00e`) without upload/draw |
| Dirty upload | When dirty start F07d2>=0, buffer.M3e35(dirtyStart,dirtyEnd) through MemberRef0a000808 (`02c`) |
| Indexed draw request | buffer.M3e33(submittedCount,writeCount) through MemberRef0a000809 (`03e`), even if existing identical vertices required no dirty upload |
| After normal flush | Advance buffer-ring index F07d4 modulo configured F07d8, reset write/submitted counts to0, dirty start to−1 (`043–071`) |
| Concrete buffer creation | T0854.M3f9b constructs **T077d<T>** (`00b`), passing batch capacity and35048; T077d extends generic T0826 |

Generic type signatures, matching declared members, base types and relevant overrides are in JSON. In particular the abstract generic factory M0e15 resolves to M3f9b on the presentation-created batch; buffer virtual primitive/count/offset methods resolve to T077d's M38b1/M38af/M38b0. No arbitrary runtime subclass is assumed interchangeable with that verified allocation.

The ring retains buffer objects for reuse; absence of a dirty upload is **not** absence of a draw request. Conversely, CPU append is not automatically a draw request. This implementation stores vertex values in arrays, not independently timestamped object commands; the face's resource binding must remain consistent with flush order.

Verified flush opportunities on this path:

1. Presentation state change M2cfe→M2cff→M2d00 can flush older work before the face.
2. Texture handle change M1e4a flushes current batch before binding another texture.
3. M0e16 reaching array capacity flushes during vertex append.
4. At normal end of M2cf0's drawable loop, M2d00(true) (`503`) flushes pending work. M2d00 clears global current presentation when requested **before** its F1a7e check; if that flag false, returns. If true, calls selected batch.M0e17 (`018`), clears F1a7e and resets F29a3.

These are syntactic reached-path opportunities, not a measured chronological first flush. Other original helper calls can alter graphics state or fail; exceptions and actual scheduling remain dependencies.

## Earliest backend/native boundary

M3e35 binds the array buffer through the original virtual hook, computes byte offset/size from `(start,end)` times generic vertex stride, takes the original array reference at start and calls **OpenTK BufferSubData** through MethodSpec2b00019c (`034`). It then invokes its original ending hook. Buffer content/GL-context correctness and driver acceptance are not established by this callsite.

M3e33 binds for indexed drawing and requests:

```text
mode        = virtual M3e31()      -> T077d.M38b1 returns4
indexCount  = virtual M3e2f(end-start) -> M38af: (3*(end-start))/2
indexType   = 5123
byteOffset  = virtual M3e30(start)*2 -> M38b0: ((3*start)/2)*2
OpenTK.GL.DrawElements(mode,indexCount,indexType,byteOffset) at026
```

Multiplication precedes signed integer division. Preserve integer overflow/truncation behavior rather than rewrite for arbitrary malformed ranges. For complete ordinary quads,4 vertices yield6 indices. T077d constructor M38ac provides a shared ushort index pattern per quad **[0,1,3,2,3,1]**, advances vertex base4/index base6 and allocates storage for capacity*4 vertices. This explains the fixed BL/BR/TR/TL vertex order without needing a replacement renderer. The complete buffer/attribute/projection system is not recovered.

EXE MemberRef0a0016a7 names `OpenTK.Graphics.ES20.GL.DrawElements` with final int offset. In the hashed OpenTK DLL the corresponding int-offset overload is **OpenTK M2fd5**, RVA8de20, code15. It constructs `IntPtr(int)` at`004` then calls **OpenTK M30b2** at`009`. M30b2, RVA8fd64, code18, reads **OpenTK F5be9 named EntryPoints**, loads slot94 (`009`), loads the pointer (`00b`) and executes **calli at00c** using standalone signature `02040108080818` (unmanaged stdcall, void, three int32 arguments plus native pointer).

This is the earliest precisely established **native invocation instruction boundary** reached through the scoped indexed-draw chain. It does not identify the actual EntryPoints[94] address, graphics provider, current context, framebuffer, shader/texture sample, successful API execution, GPU completion, swap/presentation or displayed pixel. Those are UNRESOLVED. No game or native call was run. Tokens and hashes for both wrappers are assembly-qualified in evidence to prevent accidental EXE-token substitution.

## Controlled static reference cases and validation interface

Shared control preconditions: ordinary base Play/manager, map circle(256,192), S5000/A1200 set before construction, no hit or filter mutations, permitted E08 gates. On fresh resource cache miss, null atlas and successful resource loading select T055e. Skin bytes and actual P after the coordinate pipeline remain unknown. For numeric **receiver-level** cases D04/D12, additionally supply artificial call arguments P=(256f,192f), O=(32f,32f), K=(1f,1f), theta0, R=(0,0,64,64), storage64x64. Those are discriminating scalar fixtures, **not measured screen coordinates or an executed API call**.

| Case | Condition | Static expected result |
|---|---|---|
| D01 | Fresh ordinary face, caches miss, null atlas, load succeeds | F2bf9.F2b7e.F1764 is T055e, dispatch M2929. Runtime readiness still required. |
| D02 | Successful load with nonnull atlas; region resource cached and later returned to face | F1764=T017a; M0a3d adjusts source and delegates to backing T055e/M2929. Ordinary ruleset alone cannot exclude this history. |
| D03 | M38b5 combined cache hit | Immediate cached reference return; current null atlas argument does not rebuild or normalize its receiver. |
| D04 | Numeric receiver probe above; readiness passes | Appended positions BL(224,224),BR(288,224),TR(288,160),TL(224,160), UV(0,1),(1,1),(1,0),(0,0), unchanged C. No pixels asserted. |
| D05 | Base M4737 false | M451a false; no resource/receiver draw. |
| D06 | Resource null or F1762 true | M451a true without M31f5; F1765 can still cause timestamp side effect in M27d7. |
| D07 | Resource exists but initial F1764 null | Skip receiver path; later accounting/true return can still occur. |
| D08 | E08 t=3801, normal body Fade≈0.0025, manager opacity1 | Float gate can pass but alpha byte0. If downstream readiness passes, four vertices still carry C.A=0; no proof of visible output. |
| D09 | After M292e, handle F1822 remains0 | M292d false; M2929 returns before source/vertices. An accepted presentation draw is suppressed downstream. |
| D10 | Positive handle but F1d30 true | Same suppression; no vertex append. |
| D11 | Preparation M292e returns false, existing handle positive, F1d30 false | Do not reject solely from that bool: M292d discards it and can proceed through bind/state helpers. |
| D12 | Numeric probe, K.x=−1 | Same absolute-size positions; U orientation reversed. No winding/position mirror is introduced by this branch. |
| D13 | Region(128,64,64,64), supplied source(5,7,100,20) | Delegated source(133,71,64,20); preserve Min dimensions without subtracting X/Y offsets. Not a complete clipping intersection. |
| D14 | D=2, finite valid resource | Scale divided by2; rectangle W/H/X/Y each multiplied2; origin multiplied2 before M31f5. Logical dimensions used earlier underwent integer division. |
| D15 | Supplied source width0 with otherwise valid readiness | No explicit width-zero return in M2929; origin arithmetic contains division by r4(width). Do not cancel it or promise a visible quad; malformed floating results remain unvalidated. |
| D16 | Batch capacity array8, empty write/submitted0; four face vertices | Write count4, no capacity flush from these appends; normal reached end-of-presentation flush requests6 indices. |
| D17 | Same array8 but write4 from earlier complete quad | Four new vertices fill array; fourth append triggers flush. Complete8-vertex range requests12 indices, then counters reset. |
| D18 | Null batch argument, initialized fallback from M2925 | Fallback capacity1 creates4 vertex slots; ordinary four appends can flush before M2929 returns. This is not the normal nonnull presentation argument. |
| D19 | Flush with write==submitted | Return without BufferSubData or DrawElements. |
| D20 | Flush with pending range but all compared vertices unchanged | Dirty upload can be skipped while indexed draw is still requested. Buffer reuse is not proof that no drawing work occurs. |
| D21 | Texture handle differs; current presentation batch has older vertices | M1e4a flushes old batch before BindTexture; this does not prove the new face has already been appended. Equal handle bypasses this flush. |
| D22 | M2d00(true), F1a7e false | Clears global current presentation first, then returns without batch flush. Preserve mutation order. |
| D23 | Positive F2cfb filtering branch | Selected Q/receiver can differ from original; preserve opaque M4077 and lack of an inserted extra fallback. Outside normal untouched face case. |
| D24 | DrawElements wrapper reached | Original native call instruction uses EntryPoints[94]. Pointer binding/context/output remain unresolved; reaching a backend instruction is not a captured visible frame. |

Interface for future independent implementation/validation: resource and receiver identities/history, D, P/O/C/K/theta/R/batch references; texture readiness fields and preparation result ordering; normalized storage UVs, four vertex values/order, CPU write/submitted/dirty counters, selected generic buffer and flush triggers; BufferSubData range and indexed draw arguments. Outputs here are state changes and invocation opportunities, not a production graphics API or a pixel oracle. Missing helper outcomes, invalid resources and malformed numerical cases must remain dependencies rather than convenient defaults.

## Remaining uncertainty, next investigation and performance

Smallest next static gameplay investigation: **one circle face's prepared screen-space quad through the existing projection/vertex-attribute/viewport boundary**, using the same resource and batch. This targets how the now-recovered vertices map toward a framebuffer; stop before general skin/shader/backend reconstruction. Alternatively, a pinned original-Windows trace can immediately discriminate resource cache and flush histories.

Minimum dynamic experiment (NOT RUN): fixed one-circle map at(256,192), pinned hashes/settings/skin/display, no input; log resource F1764 type and cache return, M451a arguments including byte alpha, M292d result, M0e16 write counts, M0e17 flush range, M3e33 arguments and OpenTK EntryPoints[94] address/context. Correlate timestamped captures against a no-object frame. Compare cold/warm loads and separately label any artificial clock/field probes. Recording a native call must not be reported as first visibility; identify first non-background circle pixels independently and measure tracing overhead.

Owner-reported distorted-slider stutter remains cause-UNRESOLVED. Preserve four boundaries: loading geometry/visual state and resource construction; runtime visual-state evaluation; runtime quad/vertex preparation; actual flush/backend invocation and measured frame cost. Original readiness can reach texture preparation, and resource wrappers/buffer reuse are useful future leads. None establishes first-visibility stalls, cost, caching benefit or optimization correctness. No pre-rendering or caching implementation was made; Work 2 still requires explicit authorisation.

# 3. Pseudocode

Independent research descriptions only. RECONSTRUCTED means a scoped projection of confirmed instructions, not an executed replacement renderer. Use original r4/r8 and unchecked int32 conversions/order; opaque helpers and caller ordering remain explicit.

## Resource and receiver

```text
RECONSTRUCTED — ordinary face resource route
face = M45ec-created F2bf9; face.resource = M38b4(original identifier sequence,...)
M38b4 calls M38b5(name,flags,nullAtlas)
if matching cache contains an entry: return that existing resource reference
otherwise preserve ordered resource/provider/exception logic
on successful M27e3(bitmap,atlas):
    dimensions = (Min(F1169,bitmapWidth), Min(F1169,bitmapHeight))
    if atlas is null:
        receiver = new T055e(dimensions)                 // M27ce -> M291d
    else:
        region,backing = original atlas allocation       // M2c2b
        receiver = new T017a(region,backing)             // M0a35
    resource.F1764 = receiver                           // M27cd
UNRESOLVED: decoded resource, actual cache/reload history and runtime bytes
```

## Face arguments and concrete dispatch

```text
RECONSTRUCTED — M451a, reached after normal M4518 composition
if direct base M4737 is false: return false
currentPresentation.M2cfe(F2b6b)
resource = virtual M451d()                              // ordinary F2b7e
if resource null or M27d7(resource): return true          // no draw
D = resource.F1767
if D != 1:
    preparedScale *= 1f/r4(D)
    source.Width *= D; source.Height *= D; source.X *= D; source.Y *= D
    preparedOrigin *= r4(D)
P = (bounds.X+offset.X, bounds.Y+offset.Y)
if resource.F1764 exists:
    resource.F1764.F1d31 = F2b6c ? 10497 : 33071
    Q = F2cfb > 0 ? OPAQUE_M4077(resource,F2cfb,F2cfc) : resource
    Q.F1764.virtualM31f5(P,preparedOrigin,preparedColour,preparedScale,
                         F2cf3,Nullable(source),currentPresentation.M2cfd())
preserve optional original accounting; return true

RECONSTRUCTED — T017a/M0a3d
invoke virtual preparation M31f7; discard bool
R = own region
if source.HasValue:
    R.X += source.X; R.Y += source.Y
    R.Width = Min(source.Width,R.Width)
    R.Height = Min(source.Height,R.Height)
backing.virtualM31f5(P,O,C,K,theta,Nullable(R),B)

RECONSTRUCTED — T055e readiness M292d
invoke virtual preparation M292e; discard bool
if handle <= 0 or F1d30: return false
M1e4a(handle)       // flush current batch before BindTexture if handle changes
if cached state != F1d31: M292a()
return true
```

## Four vertices, then a distinct flush

```text
RECONSTRUCTED — T055e/M2929; finite ordinary quad projection
if virtual M31f6() is false: return
R = supplied source or (0,0,virtualWidth,virtualHeight)
H = r4(R.Height)*Abs(K.y); W = r4(R.Width)*Abs(K.x)
a = (O.x*W/r4(R.Width), O.y*H/r4(R.Height))
TL=(0,0)-a; TR=(W,0)-a; BL=(0,H)-a; BR=(W,H)-a
if theta != 0:
    sin = r4(Sin(r8(theta))); cos = r4(Cos(r8(theta)))
    rotate each using oldX*cos-oldY*sin, oldX*sin+oldY*cos
add P to each corner
UV = (r4(R.X)/r4(storageWidth), r4(R.Y)/r4(storageHeight),
      r4(R.Width)/r4(storageWidth), r4(R.Height)/r4(storageHeight))
if K.y ordered<0: UV.Y = UV.bottom; UV.Height = -UV.Height
if K.x ordered<0: UV.X = UV.right; UV.Width = -UV.Width
if B null: B = F1827
append vertices [BL,BR,TR,TL] with corresponding UV corners and unchanged C
// Each append is M0e16; capacity may flush during this operation.

RECONSTRUCTED — M0e16 generic append
ensure current buffer object exists via original virtual factory
if not current.vertices[write].Equals(value):
    if dirtyStart == -1: dirtyStart = write
    dirtyEnd = write+1
current.vertices[write] = value
write += 1
if write >= current.vertices.Length:
    M0e17(); reset original write/submitted counters

RECONSTRUCTED — M0e17 flush
if write == submitted: return
buffer = current ring buffer
if dirtyStart >= 0: buffer.M3e35(dirtyStart,dirtyEnd)   // BufferSubData boundary
buffer.M3e33(submitted,write)                        // indexed draw boundary
ringIndex = (ringIndex+1) % configuredRingCount
write = submitted = 0; dirtyStart = -1
```

## Backend stopping point

```text
CONFIRMED static arithmetic/calls — presentation-created T077d buffer
M3e33(start,end):
    invoke original indexed-bind hook
    mode = 4
    count = i32(3*i32(end-start)) / 2
    byteOffset = i32((i32(3*start)/2)*2)
    EXE MemberRef0a0016a7 -> OpenTK GL.DrawElements(mode,count,5123,byteOffset)
    invoke original ending hook
OpenTK M2fd5:
    construct IntPtr(int offset); call OpenTK M30b2
OpenTK M30b2:
    calli stdcall void(int32,int32,int32,nativePointer)
        through OpenTK EntryPoints[94]
UNRESOLVED: actual native address/provider/context, framebuffer output,
            GPU completion, presentation and first visible circle pixel
```
