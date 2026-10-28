# 1. Result

Expedition 13 — Ordinary Cursor Mapping & Circle Hit Radius. 2026-10-28 (Asia/Bangkok). Work 1, original-software research only.

## Objective, scope and provenance

Recover the ordinary live pointer's spatial bridge into one hitcircle's selection: source representation, M0ff1 conversion, layout dependencies, manager F0ada preparation and ordered squared-distance test. No replacement converter, input system, renderer, gameplay prototype or numerical surrogate was built. Full devices/configuration, overlapping-object arbitration, visual projection, skins, slider/spinner tracking, scoring/HP and optimization are outside scope. Work 2 and `reconstructed/` were not inspected or modified; Work 2 remains suspended. No authorised exception.

Read the complete standing contract and identical attachment, cumulative journal and relevant E01–E12 evidence/records. Retain E05's corrected ordinary M113e→M0651→M48de→M48df→M45eb/M45ec, exact circle T094c and manager T00e8. Preserve E12's correction: M226b's post-refresh conditions rejoin before M28e3; they control intervening ruleset work rather than skipping that call on normal-returning reached paths. Earlier records remain unchanged.

Verified remote starting commit `22d9b68973adc615e86e3463d4c09c98bfc14c79`, tree `65738fa61b1187d2a32f7a8ae1a7e58d088def90`; created dedicated remote `work1/expedition-13-cursor-radius` directly from it. Local mirror `d0eb57a` has the same tree and a different commit identity; publication uses the required remote parent explicitly. All 54 tracked baseline files were checked unchanged before the journal append; preserve 53 other files and the exact old journal prefix. Final handoff supplies verified publication commit/branch/tree/blob identities.

| Input | SHA-256 |
|---|---|
| osubackup.zip,18 entries | 7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738 |
| osu!.exe | 6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d |
| Repository/supplied Instruction.md | bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9 |

Self-declared Stable40/file-version1.3.3.8 remains the reference; dated official identity/authenticity/modification status UNRESOLVED. Host recheck: Linux x86_64, no Wine/dotnet/DISPLAY or `/dev/dri`. Pinned dnfile0.18.0/pefile2024.8.26/dncil1.0.2 installed after the initial missing-module failure. No original executable, CLR/native input/API, Windows instrumentation, physical cursor placement, actual gameplay, capture or performance test executed.

## Evidence, method and actual checks

Created this report, `tools/cursor_radius_evidence.py`, `tools/check_cursor_radius.py`, `evidence/13/cursor-radius.json`, and the E13 journal append. Read-only extractor reuses unchanged E04/E05 collectors. It records 68 selected methods, body identities/RVAs/signatures, token/branch/constant/arithmetic/conversion sites, original floating constant bit patterns,25 selected field inventories, exact ordinary ancestry/virtual matching/MethodImpl metadata, provider position declarations/interfaces and direct boundary callers. Original binaries/assets/source/full disassembly remain private and are not redistributed.

Competing explanations: desktop coordinates compared directly; client coordinates compared directly; provider state transformed by a conventional hardcoded playfield; original layout-dependent conversion; radius equals visual border; fixed familiar CS formula versus stored size normalization and an extra multiplier; nearest-object selection versus first passing candidate. Discriminate by tracing original field writers/readers, precise arithmetic/casts and argument order, resolving ordinary virtual slots and preserving unknown provider/layout states. No alternative engine, modern formula or executed reference-code decoder was used as proof.

Mxxxx/Fxxxx/Txxxx denote EXE MethodDef `0x0600xxxx`, Field `0x0400xxxx`, TypeDef `0x0200xxxx`. Offsets below are **hexadecimal, code-relative and header-excluded**; JSON offsets are decimal. CONFIRMED means original static instructions/metadata. Player terminology, CS-like alias and independent pseudocode are RECONSTRUCTED. Runtime provider choice, OS/DPI units, actual layout/viewport, intermediate JIT precision and pixels are UNRESOLVED. Conditional cases require stated state and normally returning original helpers.

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/cursor_radius_evidence.py /path/osubackup.zip > /tmp/spatial-a.json
python re/tools/cursor_radius_evidence.py /path/osubackup.zip > /tmp/spatial-b.json
cmp /tmp/spatial-a.json /tmp/spatial-b.json
cmp /tmp/spatial-a.json re/evidence/13/cursor-radius.json
python re/tools/check_cursor_radius.py /tmp/spatial-a.json
python -m compileall -q re/tools
```

Actual outcomes: repeated extraction byte-identical/public JSON match;339262 bytes, SHA256 `d4e11514dc226f68792d9a0862d341e3551410cc6537a10638739dec53e0c6ef`. New checker PASS for 68 methods and 74 historical body-hash comparisons. Full EXE scan19659 bodies/1739zero-RVA/0decode errors; selected branch destinations land on instructions. All eleven unchanged E02–E12 checkers PASS; Python compilation of all research tools PASS. E11/E12 report 102/88 shared historical body comparisons after new E13 evidence is present; their original recorded counts remain untouched. Historical bytes, append-only prefix, report headings/cases, diff/scope and remote publication identity are checked for final handoff. These are evidence-validation results, not coordinate/gameplay-equivalence tests.

| Method | RVA | Code bytes | Body SHA-256 |
|---|---|---:|---|
| M0ff1 | 63fe0 |24|42b40b9120db5d1ba484cbb7fe9b5ab99a4b0b2fdfe27421363de4bf5d1c7115 |
| M0fed | 63e8c |253|7da516612d44c74771dad2619436898fc8a5e959a2d0059cbea79a08ebb685fe |
| M28be | 1079e4 |513|f9e1de22a60ae85deecb2bd1647972fc2fa5219e48e45cc50ac7479ccf326357 |
| M28b2 | 1077d4 |47|e57918c95cdfad4ac5b5b390e354ab4341fb009314b52879c01edf68f108fc3a |
| M1196 | 6d43c |56|efa743ab4247b8eb4b05fac164e49400c96ee5ea246a416bfadbf4fb5aaa797b |
| M4ecd | 1e88fc |34|1d686703ec6f2684d6284be57dc008db9d51e1d63d7644cdc8baa154d32c0002 |

# 2. What they found

## What this means to an osu! player

When Z or X is consumed, the traced ordinary live route uses a **stored cursor position transformed into the circle's playfield coordinates**. It does not compare raw desktop coordinates directly to the map circle. A Windows-position provider can first translate desktop position to the window client and subtract the viewport origin. Stable then subtracts its playfield-layout offset and scales into the nominal 512×384 coordinate space used by these objects.

The hittable area is a circular distance test around the circle's **current stored center**, after time/evaluated-state gates. Its radius comes from a map size operand and difficulty/mod processing. HardRock uses 1.3 for this size input, versus1.4 in the earlier timing-width mapping; Easy halves the size input. Larger ordinary size input reduces the resulting radius. An additional `1.0004099607467651f` multiplier is part of the original radius calculation.

Exactly equal **computed squared distance and computed squared radius** pass the ordered comparison. A geometrically intended boundary point can be rounded before it reaches that comparison, so that statement is not a promise about a particular physical pixel. The visible texture edge, draw projection and actual Windows DPI/layout/provider state remain separate, unverified dependencies. We do not equate hit radius with the visible circle border.

## E13-A — Pointer source, storage and sampling boundary

CONFIRMED M2295:063 calls M4390. On the ordinary live false branch it loads global layout F2c64 at06a, stored pointer **F2a5c** at06f and calls nonvirtual M0ff1 at074. On the true branch it uses **F2a7b** directly at07b, bypassing this mapping. E10/E11 identify M4390 as the recorded-input selector; full replay behavior is outside scope.

F2a5c is value type T0a82, whose X/Y fields F3109/F310a are System.Single. It is not a System.Drawing.Point or an event-specific position payload. Complete direct managed stores are M43a0:144, recorded-input M43a1:881 and explicit cursor-position helper M43b2:026. No event position is captured by E11's parameterless key-press delegate.

On the reached ordinary update route, M43a0 first calls provider processing M11a3 at054 and stores its returned provider reference at059. M11a3 selects the first active provider satisfying T03fc from the runtime list, calls virtual M17a7 with whether each provider is selected, then calls M1192 on the selected provider at0b2. M1192 obtains virtual M1e12 position at001, forwards it through M1193 at006. M1193 first calls M119c, then broadcasts the supplied position through M43b3 to other provider objects under its original selected-reference tests. This broadcast is not assumed side-effect-free or atomic with selection.

M119c stores the supplied vector to **F0a89 at001 before** preparing other coordinate representations. It separately adds an auxiliary vertical value, normalizes for F0a88 through M119a, and rounds the *original supplied* X/Y with Math.Round→conv.i4 for F0a87 through M1198. Those auxiliary rounded/normalized values do not overwrite F0a89. M119b simply returns F0a89. M43a0 compares the stored pointer with M119b, rereads M119b at13f and stores F2a5c at144 when the pointer-change branch is reached. At153–162 it also maps F2a5c to F2a7b. Key polling at1d0 and queued callback drain at773–78d happen later (E11); M2295 rereads F2a5c during consumption, not a position frozen at physical key receipt.

Runtime gates F2a6f/F2c24 and special input paths can bypass that live storage block. With unchanged pointer, the existing stored value is reused. The active provider cannot be uniquely inferred from ordinary ruleset identity: metadata finds abstract M1e12 plus five concrete matching getters M06de/T00f5, M09f2/T0170, M0a56/T017f, M0db1/T01e7 and M270d/T0508. Their signatures/declarations/interfaces are in evidence. Original provider list/activation, relative input and sensitivity settings must be observed; no default provider is fabricated.

### Smallest verified desktop-to-provider bridge

One concrete Windows-position route is T0508's update **M270b**, a reuse-slot implementation of M17a7, beginning with M1196 at000. Its getter M270d returns stored Single vector F16b7. M270b has relative/absolute and configuration branches that may modify that vector; this expedition does not reconstruct full sensitivity, clipping or device policy. In its reached absolute branch with the original setting comparison selecting271, X/Y are taken from the sampled integer point and converted to Single at27d/28f. Other branches retain state/adjustments. This is a conditional route, not proof the actual installation selected this receiver.

M1196 gives a precise original source boundary:

| Sites | Operation |
|---|---|
| 000–00f | Obtain control through M466a; call WinForms Cursor.Position at005 and Control.PointToClient at00a |
| 010–015 | Read adjustment point J from M4695 |
| 016–031 | Integer client.X−J.X first, then client.Y−J.Y; unchecked arithmetic |
| 032–037 | Construct integer T0a80 point and return |

M4695 constructs **J=(F1175.Left, F2c9d.Height−F1175.Bottom)** at000–024. Thus viewport-origin subtraction is already present before M0ff1 on this route. F1175 is a stored System.Drawing.Rectangle; M1e45 writes it at013/186 and passes its components to original OpenTK GL.Viewport at0ba/1b3. Its optional supplied rectangle path normalizes negative extents first; its generated rectangle path depends on dimensions/settings. M468a/M4699 write host client-size state F2c9d; M468a also passes that size to Form.ClientSize. We stop at these input-relevant stored origins/dimensions, without reconstructing full viewport/projection/backend policy.

`PointToClient` and the actual control/backend/DPI behavior have not run. Client/viewport units must not be silently called physical pixels, and current rectangle/client values are not supplied by the archive. Null/unavailable control or provider failures have no invented fallback here.

## E13-B — Exact playfield-coordinate conversion

CONFIRMED global layout identity: host M4668 constructs T0364 dimension state at1db, stores F2c65 at1e0, passes that reference into T0228.M0fe1 at23d and stores the layout object as F2c64 at242. Layout F09d2 retains that dimension-state reference.

| Layout value | Getter / stored field | Type |
|---|---|---|
| W, prepared playfield width | M0fe2→F09cc | Single |
| H, prepared playfield height | M0fe4→F09cd | Single |
| O, playfield offset | M0fe6→F09ce | T0a82, Single X/Y |
| q, requested layout factor | F09d0 | Single |
| b, dimension base scale | M18ff: `r4(F0e6e)/480f` | Single-declared |
| s, input playfield scale | M0fea: `H/384f` | Single-declared |

M0ff1 is 24 bytes, with no mode, rotation, matrix, clamp or skin lookup:

1. Load supplied vector Q, obtain O by M0fe6 at002.
2. Call vector subtraction M4efe at007. It subtracts X then stores X at00f; subtracts Y then stores Y at023 into a new Single-field vector.
3. Obtain s through M0fea at00d: getter H, literal384f, divide at00b.
4. Call **vector/scalar division M4f03** at012. This is implemented as `a=1f/s` at006 into a Single-declared local, then X*a and Y*a with Single-field stores012/021. It is **not** two independent component divisions.
5. Return the vector at017.

RECONSTRUCTED finite algebra: `P=(Q−O)*(1/s)`. Preserve the original vector store and reciprocal ordering rather than replacing it with a conventional hardcoded offset, a matrix inverse or direct divisions. No int conversion, integer rounding or per-coordinate clipping occurs inside M0ff1. Offset and height are read at separate calls; a consistent immutable layout snapshot is not proved.

### How layout dimensions and offsets are prepared

Constructor M0fe1 stores q=1 at013 and initializes W/H to `r4(dimension.width/height)*0.8f` at026/039. Initial offset retains the CLR default until a setter path runs. That constructor-only state is not universally the same aspect ratio/offset as recomputed gameplay layout.

M0fed(factor,force) is the main recompute boundary:

- At001–00c, if stored q equals supplied factor and force=false, return. The `bne.un.s` branch enters recomputation for unequal **or unordered** values. A same-factor call without force does not refresh dimensions or the mode-dependent offset.
- At00d–020 compute `ordinaryOffset = (applicationMode!=Edit1 && M2232()==Osu0)`. Store q at023 and this flag at02a.
- Set **W=(512f*b)*factor** at02f–043; set **H=(384f*b)*factor** at048–05c. M18ff is called separately for these two calculations.
- Vertical correction v is `−16f*b` when ordinaryOffset, otherwise0; b is sampled again at076.
- O.X=`(r4(dimension.width)−W)`→explicit conv.r4 at091, then `/2f`.
- O.Y=`(r4(dimension.height)−H)`→explicit conv.r4 at0ab, then `/4f`, then `*3f`, then `+v`.
- Construct/store O through M0fe7 at0bf. It also stores a separate secondary offset through M0fe9 at0f1; M0ff1 reads F09ce, not that secondary field.
- At0f7 call manager registry M28a0(true). Under its registry lock, each registered manager gets virtual M28be(true,true) at027. Thus a reached layout recompute can refresh radius and other manager configuration; it is not an atomic layout+all-manager publication.

For stable dimensions and consistent recomputed fields, ideal s=b*q. This is conditional algebra, not a guarantee of bit-identical cancellation, nor proof of a particular current factor. M46b0:224 forces recompute using stored q on its reached mode-commit path; M4687:576 does likewise after dimension work. M2237:0d6 can force factor1 on its specific preceding map-mode branch; do not describe that conditional call as an unconditional ordinary Play reset. Special callers can use other factors. M0ffd adds supplied dx/dy to current O through M0fe7; it has a special nonordinary caller, so no unobserved ordinary translation is assumed.

M0fe7 is the sole direct writer of F09ce. Width/height setters M0fe3/M0fe5 are sole direct writers of their fields, called by constructor/recompute. F0e6d/F0e6e dimensions have host/configuration writers; M4687 includes window/fullscreen/dimension branches. Current state, settings and sequencing remain runtime inputs rather than invented screen resolution. Editing and other rulesets differ in vertical correction; no complete special-Play equivalence is inferred.

## E13-C — Original size-to-radius preparation

CONFIRMED ordinary manager T00e8 inherits M28be. Radius F0ada and size scalar F17f1 are Single fields. Direct stores to F0ada are M28be:190 and M20d5:074; M20d5 is the separate Taiko manager override established by E05 and must not be substituted for ordinary osu!standard. Base M28be's map-null guard at001–008 returns without writing a radius. A fresh default or stale field is not repaired by a fabricated fallback.

Loading M28f5 calls virtual M28be at07f before later map reading/postprocessing, while M0fed→M28a0 can reconfigure registered managers later. No per-key radius recomputation is performed by M28c8/M28c9/M24c7. Changing fields/configuration and comparing a stored radius are distinct operations.

The size operand is map single **F0b53**, initialized5 by M1387:00c–011. Metadata text parsing M1bc1 has its difficulty-field case at0ba6: formats<13 branch to Byte.Parse/clamp0..10→conv.r4→store0c22; formats>=13 ordinary non-Mania branch uses Single.Parse then Max(0,value), Min(10,value), store0c03. The mode3 path has1..18 limits and is outside ordinary scope. Binary-cache reader M1bf2 independently reads Single at0ce→F0b53 at0d3. Other editor/variant writers are in the full inventory. Literal encrypted parser-key text remains undecoded; **CS-like/circle-size operand** is a RECONSTRUCTED alias grounded in this original size dataflow, not an asserted decoded UI spelling or proof of actual imported settings.

Abstract slot M1259 resolves to **M28b2** on the ordinary manager. At001 it supplies double **1.3** to shared M28b0; at015 that method returns the modified input d, followed by double `(d−5)/5` at01a–02e. Outside Edit, M28b0 first applies Easy2 `Max(0,D/2)` and then HardRock16 `Min(10,D*factor)`. Here factor=1.3; E11/E12's timing mapping uses1.4. These are different original call arguments. No mod directly expands the geometric radius by multiplying it1.3.

M28be numerical order (CIL mixed precision retained):

| Offset | Operation |
|---|---|
| 139–144 | Get W through M0fe2; divide by literal8f; conv.r8 |
| 145 /14e | Push double1 and double **c=0.699999988079071** (LE bits `000000606666e63f`) |
| 15e–164 | Read map F0b53, conv.r8, call virtual M1259→M28b2 for n=(d−5)/5 |
| 169–16d | Multiply c*n; compute1−that; multiply by prior r8(W/8f); explicit conv.r4 then store D=F17f1 |
| 174–17e | Reread stored D; divide by2f |
| 17f–189 | Get layout s through M0fea; divide prior value by s |
| 18a–190 | Multiply by **k=1.0004099607467651f**, LE bits `6f0d803f`; store R=F0ada |

Thus independent operation-level description is:

`D = conv.r4( r8(W/8f) * (1.0 − c * ((d−5.0)/5.0)) )`

`R = stored_single( ((D/2f)/s) * k )`

The scalar radius division in M28be is CIL `div`; M0ff1's vector division helper instead forms a reciprocal then multiplies. Do not merge their rounding schedules. F17f1 is a size scalar/diameter-like input to other presentation code, not an independently measured visible texture diameter.

With consistent recomputed layout and finite nonzero scale, ideal cancellation gives `R≈32*(1−c*(d−5)/5)*k`, conventionally approximated by `(54.4−4.48*d)*k`. **This is derived algebra, not an imported modern formula or bit-exact replacement.** Stored W/H/D rounding and separate reads remain part of the original. Under controlled b=q=1, W512/H384, d5 gives D64 and R=32*k, exactly the representable Single value32.013118743896484 after the field store. Other useful approximate magnitudes: d4→36.5; d5→32.0; Easy base5 (d2.5)→43.2; HR base5 (d6.5)→25.3. Use the original equations for thresholds; rounded prose values are not accepted-radius constants.

No explicit radius finite/positive guard appears in the reached comparison. With malformed/artificial R, a negative finite value is squared just like its positive magnitude; NaN makes the primary ordered success fail; zero/infinite/underflow behavior follows actual operations, not an invented clamp. Ordinary parser bounds and actual mods may restrict natural reachability; exceptional parse/CLR behavior is not executed here.

## E13-D — Exact spatial test, center and precedence

CONFIRMED normal F137c=false M2295 calls M28c8 at0bb with mapped P.X/P.Y and inputFlag=true. M28c8 loads **current stored R=F0ada at005** and forwards to nonvirtual M28c9 at00a. F137c=true instead directly calls M28c9 at0a0 with radius100f; E11 identifies the live derivation from Relax2/Autopilot8192. Relax128's callback gate is separate. Recorded-input branch obtains F2a7b without invoking M0ff1 here; neither route is generalized to the ordinary live control.

M28c9 constructs a T0a82 from the supplied x/y (Single fields), enumerates existing F17fd in order, applies live/input F153b gate, calls object virtual M24c7 at03b and returns the first success via leave044→061. Exact ordinary T094c inherits M24c7 without override. A preceding passing candidate wins even if another later object would be closer; no nearest-distance ranking is performed by this method. We stop at this first-selection property rather than reconstructing all overlapping-object arbitration or later admission retries.

For inputFlag=true, M24c7 skips M24b6, then checks in order:

1. `i32(start−A)>F2358` rejects at022. Lower equality passes.
2. `i32(start+F0adc)<F2358` rejects at03b. Upper equality passes. This is a separate time read.
3. F1527=true rejects at043.
4. Load supplied P and **C=object.F252c** at047, call **M4ecd DistanceSquared at04c**, then push R twice and multiply at053. Ordered **`ble.s`054→070** accepts when computed distance²<=computed R².
5. If the primary test fails, inputFlag=true branches057→06e and returns false. It does **not** use the alternate center M24b7 fallback. That false-argument variant is outside ordinary hit input.

M4ecd reads P.X−C.X at00c, then P.Y−C.Y at019 into a Single-declared local. Dx remains on the evaluation stack; square dx at01c, square dy at01f, add at020, return at021. Signature returns System.Single. There is no square root, absolute-axis box, integer cursor rounding, texture-alpha mask or skin-radius query in this test. T094c construction M45ec initially stores supplied position in F252c at009 and F152e separately; setter M24b0 can later change F252c and conditionally F152e. Other position writers/postprocessing exist. Compare the **current** center, not an assumed immutable original map row or visual draw position.

For ordinary finite values far from rounding boundaries, geometric interpretation is the Euclidean disk centered C with radius |R|. All signs of dx/dy are symmetric in this final squared arithmetic. A NaN distance or squared radius does not take the primary ordered `ble` success. If both compared values are positive infinity, ordered equality can succeed; if a subtraction produced NaN it fails instead. These are conditional static floating-comparison consequences, not claims that malformed maps can naturally reach every such state.

The archive declares a 32-bit CLR build. Single fields, explicit conv.r4 sites, Single signatures, double constants and instruction ordering are confirmed; x86 CLR/JIT internal evaluation precision, inlining and spills have not been measured. Do not force binary32 rounding after every arithmetic opcode or silently use Python double arithmetic. Extremely close floating-point cases below identify that remaining dependency instead of claiming observed pixel boundaries.

Passing space is only selection. E11 action admission can reject the selected object, and E12 classification can produce a negative result even when selection passed. E12's corrected input→candidate refresh→object update ordering remains authoritative; drawing/approach visibility are not spatial-hit eligibility.

## E13-E — Conditional static reference cases and interface

Not original-game or executable surrogate tests. Main control: exact ordinary T094c, current stored center C=(256,192), stored CS-like input5, stable recomputed dimensions w640/h480, q1, non-Edit Osu0, W512/H384, O=(64,56), s1, R=32.013118743896484, candidate present, unjudged/selectable, plain mods, all E11 gates and timing tests passing. Q means already provider-produced viewport-local F2a5c, **not raw desktop pixels**. Forced-field arithmetic probes are explicitly separate from normal configuration.

| ID | Controlled condition | Static prediction |
|---|---|---|
| S01 | Q=(320,248), main layout | P=(256,192); distance²0, spatial pass |
| S02 | Q=(336,248) or(304,248) | P offset±16 horizontally; inside, pass |
| S03 | Q=(320,264) or(320,232) | P offset±16 vertically; same squared result |
| S04 | P offset±64 horizontally or vertically | Outside main radius; spatial false |
| S05 | P offset(22,22), signs independently varied | Distance²968<R²; pass |
| S06 | P offset(24,24), signs independently varied | Distance²1152>R²; reject |
| S07 | Conditional computed distance²==computed R² | Ordered equality passes; does not assert a particular physical pixel realizes it |
| S08 | Isolated artificial probe C=(0,0), R32, P=(32,0),(-32,0),(0,±32) | Exact squares1024; all pass boundary equality |
| S09 | Same probe P.X=32+2^-18,Y0 | Next Single above32; square exceeds1024; reject |
| S10 | Same probe P.X=32−2^-19,Y0 | Previous Single below32; inside, pass |
| S11 | Artificial C0,R40,P=(±24,±32) | Exact diagonal40 boundary, squares1600; pass |
| S12 | Artificial C0,R32,P=(32,2^-8) | Mathematical square1024+2^-16. If comparison receives rounded Single1024, accepts; if internal precision retains the difference, rejects. Pin original JIT/spill behavior; no unconditional prediction |
| S13 | Artificial C0, finite radius, P has NaN component | Primary distance comparison unordered, false; inputFlag=true gives no alternate-center fallback |
| S14 | Artificial R NaN, finite P/C | R² NaN; primary success false |
| S15 | Artificial R−32 versus+32, C0,P=(32,0) | Same squared radius, equality pass; no positive-radius sanitization here |
| S16 | Artificial R0, P=C versus nonzero exact representable offset1 | Center passes, offset fails; tiny-underflow probes require actual comparison operands |
| S17 | Artificial P.X infinity,C.X finite,R infinity, other components finite | D² and R² can both be+infinity; ordered equality passes. Infinity−infinity giving NaN instead fails |
| S18 | M0ff1 scale0 orNaN | No explicit guard/clamp; reciprocal and multiplication may yield infinities/NaNs; record actual operands rather than inventing origin fallback |
| S19 | Matched viewport-local P offset34, CS-like operand4 versus5 | Radius approximately36.5 versus32.0; inside for4, outside for5 under stated configuration |
| S20 | Base5 with Easy2 versus plain | d2.5 versus5; radius about43.2 versus32.0; offset34 passes only Easy in this pair |
| S21 | Base5 with HR16 | d6.5 (factor1.3), radius about25.3; offset30 fails, whereas plain passes |
| S22 | Both modifier bits present in an artificial configuration | Easy first:5→2.5; HR then2.5*1.3; preserve order, not HR-first or direct radius multiplication |
| S23 | Edit1 with those bits | M28b0 skips Easy/HR; M0fed ordinary vertical correction is0 rather than−16*b. Not an ordinary Play equivalence claim |
| S24 | w1280/h960, q1, consistent recompute | W1024/H768,O(128,112),s2. Q(640,496) maps to same C; d5 gives same stored R in this exact power-of-two control |
| S25 | Fixed Q, O.X increased8 via offset setter, s1 | P.X decreases8; R unchanged by that offset-only write |
| S26 | O.Y decreased4, fixed Q, s1 | P.Y increases4; no rotation or global matrix inversion inside M0ff1 |
| S27 | Same factor as stored q, force=false, dimensions altered separately | M0fed returns before refresh; no automatic recompute from that call |
| S28 | Same factor, force=true | Recomputes W/H/O and invokes registered manager configuration; preserve mutation order and possible exceptions |
| S29 | Consistent positive q or dimension change in general | Ideal normalized radius cancellation applies, but not guaranteed bit-identical across rounded W/H/D or separate reads; use original operands |
| S30 | Conditional Windows source: viewport J increases(10,20), same client point | Sampled integer Q decreases(10,20); with s1/O unchanged P does too |
| S31 | Same viewport shift and client point increases by same(10,20) | Q restored; later mapping/spatial result unchanged under stable other state |
| S32 | M4390=true | M2295 uses F2a7b directly; do not remap that field with M0ff1 again |
| S33 | F137c=true and all its other guards permit | Radius100f supplied instead ofF0ada; timing/admission effects remain separate |
| S34 | Cursor at center but T<start−A orT>start+W50 | Reject before distance call; being spatially centered does not confer selection |
| S35 | Cursor outside radius, T=start | Spatial rejection prevents later action classification for this selected-circle route |
| S36 | Centered, selection time passes, T=start−400 | E11 broad admission equality can reject before M1884 despite spatial success |
| S37 | Centered, T=start+W50 exactly | Selection can pass, E12 classifier can produce negative result; no spatial-success implies positive-hit claim |
| S38 | Evaluated flagtrue or live selectable flagfalse | Rejected before distance (selectable gate in M28c9); no second ordinary hit |
| S39 | Earlier candidate passes its predicate; a later one is closer | M28c9 returns earlier reference; no nearest-distance ranking. Later admission/arbitration not reconstructed |
| S40 | Stored center changed by M24b0, supplied P unchanged | Compare against newF252c, not old row coordinate/F152e/visible face |
| S41 | Provider supplies fractional Q | M119c preserves vector inF0a89 before auxiliary rounding; M0ff1 does not round it to integer |
| S42 | Layout/provider fields change between original reads | No atomic position+offset+scale+radius snapshot established; symbolic equal-layout predictions no longer apply |

Interface: input includes actual provider identity/state and its supplied Single vector, F2a5c/F2a7b selector, client conversion and J only on reached provider paths, layout dimension reference W/H/O/q/scale/mode flags, map F0b53 and manager mods, prepared F17f1/F0ada, current object F252c, candidate identities/order, selectability/evaluated/timing gates. Output is mapped P, selected reference/null and spatial predicate result, followed separately by admission/classification. Original input/layout/field mutations and time reads remain ordered, not one synthetic event snapshot.

## E13-F — Remaining limits and smallest original Windows experiment

UNRESOLVED: actual active provider, sensitivity/relative-device behavior outside the bounded source route, OS/DPI/control/client units, current viewport/layout dimensions and settings, callback/resize interleavings, target JIT intermediate precision, literal parser-key spelling, actual visual center/border/projection and any measured latency/pixels. HYPOTHESIS that visible texture border matches the prepared spatial radius is unsupported and must not guide reconstruction. E09 projection remains deferred beyond the narrow stored viewport-origin bridge. Owner-reported distorted-slider stutter and caching benefits remain unmeasured; no optimization is inferred or implemented.

Minimal pinned-Windows experiment (not run):

1. Reproduce E01 build/hash/environment baseline on a disposable offline installation. Record actual client/control dimensions, DPI awareness/scaling, viewport rectangle F1175 and host client size F2c9d, layout object identity/dimensions/O/q/scale, ordinary handler/manager/circle types, active provider and relevant settings/mods. Use unchanged E01 fixture first circle; confirm actual imported map hash `dc4112acfcb0e7f345c6350a0cf1640d74f7acdc6eb2ae77047023b09e330541` and WAV `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977`, or preserve original/imported hashes if import changes them. Fixture remains unvalidated for original import.
2. Instrument original M1196/provider position getter, M119c, F2a5c store/read, M0ff1, M28be, M28c9 and M24c7. Resolve tokens/CIL offsets to actual JIT locations, not native-address arithmetic from CIL offsets. Log ordered thread/QPC samples of OS cursor placement/PointToClient, provider output, J, Q, O, H/s, mappedP, current C, raw/modded size operands, size scalarD, storedR, actual squared operands and branch. Read existing original values; do not add extra original polling/clock/layout calls that perturb order.
3. Under fresh unjudged state and a consumed time near circle start, test center and four cardinal directions around measured R, then diagonals. Classify by logged operands, not intended desktop pixel offsets. Equality may not be attainable with one-pixel physical steps at a particular scale; debugger-forced P/R probes are separately labelled artificial. Repeat with two actual circle-size settings and separately Easy/HR, recording the reached F0b53 and factor rather than assuming labels.
4. Change one window/layout condition and repeat, logging every provider/client/viewport/layout/radius refresh. Compare actual mapped center and radius, not guessed screenshot dimensions. A captured visible border is a separate measurement and does not define the hit predicate. Optional artificial S12 probes compare uninstrumented/instrumented JIT paths to detect spill/precision effects; never call them normal gameplay observations.
5. Preserve input/build/settings/log hashes and tracing-overhead controls. No score, GPU completion, heard-audio timing, frame cost or pixel-equivalence claim follows from a true spatial predicate. Original Windows execution remains the required validation dependency.

Smallest subsequent investigation: resume the **deferred one-circle draw-center projection boundary** and compare its original position/layout transformation with this recovered input mapping. That would connect hittable playfield coordinates to a verifiable displayed location, without authorizing general graphics reconstruction, skin work or product implementation. Mission Control chooses the assignment; Work 2 remains suspended until explicit authorization.

# 3. Pseudocode

RECONSTRUCTED independent documentation from CONFIRMED original instructions. Non-executable; no coordinate or gameplay implementation supplied. Preserve Single-field stores, explicit conv.r4/conv.r8, scalar div versus reciprocal-vector helper and original fresh field reads. `i32` denotes unchecked int32 operations. Runtime providers and opaque side effects are not fabricated no-ops.

```text
conditional original Windows-position source M1196:
    client = control.PointToClient(Cursor.Position)
    J = integer_point(F1175.Left, i32(F2c9d.Height - F1175.Bottom))
    return integer_point(i32(client.X-J.X), i32(client.Y-J.Y))
    // Selected provider can process this point before M1e12 returns its vector.

M11a3 / M1192 / M1193 boundary:
    ... original runtime provider selection and updates ...
    supplied = selected.virtual M1e12()
    M119c(supplied):
        F0a89 = supplied                       // first store, preserves fractional input
        ... original auxiliary normalized/rounded representations ...
    M43b3(supplied)                            // original provider broadcast

reached M43a0 ordinary live block:
    ... original pointer-change/activity conditions ...
    F2a5c = M119b()                            // reads F0a89
    F2a7b = layout.M0ff1(F2a5c)
    ... later keyboard polling and queued input consumption ...

M2295 pointer/radius projection, after original guards:
    P = F2a7b if M4390() else layout.M0ff1(F2a5c)
    if F137c: selected=manager.M28c9(P.X,P.Y,true,100f)
    else: selected=manager.M28c8(P.X,P.Y,true)   // forwards stored F0ada
    ... E11 admission and E12 classification are separate ...

M0ff1(Q):
    delta = M4efe(Q,M0fe6())                   // X subtraction/store, then Y/store
    s = M0fea()                               // fresh H read /384f
    return M4f03(delta,s)
M4f03(delta,s):
    reciprocal = 1f/s                         // Single-declared local; original CIL F precision
    out.X = delta.X*reciprocal                 // Single field store
    out.Y = delta.Y*reciprocal
    return out
```

```text
M0fed(factor,force):
    if ordered_equal(F09d0,factor) and !force: return
    ordinaryOffset = mode!=Edit1 and M2232()==Osu0
    F09d0=factor; F09d1=ordinaryOffset
    W=(512f*dimension.M18ff())*factor
    H=(384f*dimension.M18ff())*factor           // separately sampled base scale
    v=(-16f*dimension.M18ff()) if ordinaryOffset else0f
    O.X = conv_r4(r4(dimension.width)-W)/2f
    O.Y = (conv_r4(r4(dimension.height)-H)/4f)*3f+v
    store O through M0fe7
    ... original secondary offset store ...
    M28a0(true)                                // original registry lock/configuration calls
M18ff(): return r4(F0e6e)/480f
M0fea(): return H/384f

ordinary M1259 -> M28b2(CS):
    d=M28b0(r8(CS),1.3,converted manager mods)
    return (d-5.0)/5.0
M28b0(D,factor,mods):
    if mode!=Edit and Easy2: D=Max(0.0,D/2.0)
    if mode!=Edit and HardRock16: D=Min(10.0,D*factor)
    return D
M28be radius projection, after original map-null guard:
    D = conv_r4(r8(layout.W/8f) *
        (1.0 - 0.699999988079071 * virtual M1259(r8(map.F0b53))))
    F17f1 = D
    F0ada = ((F17f1/2f)/layout.M0fea()) * 1.0004099607467651f
    ... other original configuration/conditional callbacks ...
```

```text
M28c9(x,y,inputFlag,R):
    P=Single_field_vector(x,y)
    requireFlag = !M4390() and inputFlag
    for obj in existing F17fd order:
        if requireFlag and !obj.F153b: continue
        if obj.virtual M24c7(P,inputFlag,R): return obj
    return null

M24c7 reached ordinary-circle inputFlag=true:
    // Bypasses M24b6 and excludes the alternate-center fallback.
    if i32(start-manager.A) > F2358: return false
    if i32(start+manager.W50) < F2358: return false
    if F1527: return false
    distance2 = M4ecd(P,F252c)
    if ordered(distance2 <= R*R): return true
    return false
M4ecd(P,C):
    dx=P.X-C.X                                // retained on CIL evaluation stack
    dy=P.Y-C.Y                                // Single-declared local
    return dx*dx + dy*dy                       // original order; Single signature

UNRESOLVED:
    actual provider/OS/DPI/viewport/layout and mutation interleavings;
    target JIT intermediate precision at extremely close float boundaries;
    visible center/border and full draw projection, not defined by this predicate.
```
