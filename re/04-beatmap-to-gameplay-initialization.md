# 1. Result

Expedition 04 — Beatmap-to-Gameplay Object Initialization. 2026-10-27 (Asia/Bangkok). Work 1, reverse engineering only.

## Objective and preparation

Question: how does a selected Play component connect a selected beatmap to runtime circles, sliders and spinners? Scope is the shortest verified ordinary `PlayModes.Osu` path, plus relevant virtual-dispatch and loading boundaries. No replacement parser, objects, renderer, gameplay simulation or changes under `reconstructed/`.

Read complete Instruction.md, contexts and Expeditions 01–03, relevant baseline and selected-method evidence. Starting GitHub commit `633cdd7605a1fd8c2d089e3ccdb14fafd6b29871`, tree `fa3e70e4afc8eb87ec9e1d8a2abec3466da96ea9`, verified remotely and matched to the local baseline tree. Dedicated branch `work1/expedition-04-beatmap`. Prior records preserved; contexts only appended. No authorised exception.

## Evidence and actual tests

Reference archive SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; executable SHA-256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`. Same baseline file-version metadata 1.3.3.8/Stable40; official dated build and modification status remain UNRESOLVED. Analysis on Linux, no original Windows execution/capture or measured gameplay. No supplied beatmap was present; no map import or loading test was performed.

New `tools/beatmap_init_evidence.py` reads ZIP bytes and emits `evidence/04/initialization.json`: 66 selected MethodDefs (including abstract slots), 32 type/ancestor records, field signatures, enum constants, method body hashes, call/field sites, numeric constants, branches and exception regions. Site arrays are `[code_offset, opcode, metadata_token]`. Offsets here are hexadecimal relative to first CIL opcode, excluding headers. EH offsets already use code-start coordinates. Evidence is metadata/cross-references, not proprietary decompiled source or asset copies.

Reproduction from repository root:

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/beatmap_init_evidence.py /path/osubackup.zip > /tmp/init.json
python re/tools/check_beatmap_init.py /tmp/init.json
python re/tools/static_control_flow.py /path/osubackup.zip --inspect-token 060028f7
python -m py_compile re/tools/beatmap_init_evidence.py re/tools/check_beatmap_init.py
```

Actual checks: evidence extraction/repetition, branch-target boundary assertions, selected-map/callback/factory/registration/field consistency checks and Python compilation passed. Historical file identity, contexts-prefix and diff checks passed before publication. Original gameplay tests, factory execution, pixel comparison, first-hit timing and Windows instrumentation were NOT run. Parser branch predictions are not runtime observations.

Changed files: this mark, two read-only evidence tools, one evidence JSON, and contexts append. Publication revision is supplied in final handoff. No earlier tool or evidence was modified.

# 2. What they found

## What this means for gameplay

**RECONSTRUCTED interpretation of CONFIRMED instructions:** the ordinary Play component does not receive a filename through its constructor. Its initializer captures the currently selected map object from a global selector. Later loading work selects an osu-specific handler, which owns an object manager and factory. A text reader consumes map data and calls that factory directly while reading object rows. On this path, there is no demonstrated intermediate list of generic parsed-hit-object definitions that must later be converted wholesale into gameplay objects: parsed scalar/list locals go directly into runtime factory calls, followed by registration.

Circle/slider objects receive positions and start times; spinners receive start/end times and use a fixed center in their base constructor. Slider duration is not established by its initial start=end assignment: loading subsequently performs a slider-specific postprocessing pass. Objects also carry a manager relationship, type flags, sound flags and visual-state collections. Their construction is not proof that they are already visible or hittable. Completion callbacks, handler setup, object postprocessing and frame/input/timing gates remain.

All **CONFIRMED** statements below are static, build-specific findings. Gameplay labels and aliases are **RECONSTRUCTED** from enum names, parser indexes, factory ancestry and field dataflow. Unknown helpers remain token-labelled. No visibility/judgment equivalence is claimed.

## Selected-map and loading chain

Abbreviations: `Mxxxx` = MethodDef `0x0600xxxx`; `Fxxxx` = field `0x0400xxxx`; `Txxxx` = TypeDef `0x0200xxxx`. Exact RVAs, file offsets, signatures, hashes and inheritance are in JSON.

| Boundary | Static sites and data relationship |
|---|---|
| Selected map capture | M2c63 reads static F1a0f. M2237 calls it at `001` and writes returned map reference to Play.F13f8 at `006`. Base constructor M222e has no map parameter. M2c64 is a setter that ultimately writes F1a0f, with old-selection cleanup calls. UI origin of a particular selection is not traced. |
| Work/completion callbacks | At `656`/`663`, M2237 takes virtual addresses of M223b (void work) and M223c (bool-to-bool completion), constructs delegates, and passes them into wrapper constructor M0707 at `66e`; wrapper stored in F13c9. |
| Work dispatch | M0707 stores delegates F0345/F0346 and queues M0708 through M46c4. M0708 invokes work at `011`, catches exceptions into F0348, and calls M070a with a success value. M070a queues M070d through scheduler M09ad; M070d contains the completion delegate Invoke. Actual threading, queue timing and first-update ordering are UNRESOLVED. |
| Ruleset handler | M223b switches on M2232() (reads static F1371). Metadata enum `osu_common.PlayModes`: Osu=0, Taiko=1, CatchTheBeat=2, OsuMania=3. Case 0 constructs M1ecb at `027` and writes Play.F13a4 at `02c`. Other modes select different handlers; they are not interchangeable with this osu path. |
| Handler-owned manager | M1ecb calls handler-base constructor M0a9c. That constructor invokes virtual manager factory M0abb at `072`, storing its result in handler.F05f4 at `077`. For handler T0421, reused-slot override M1ee1 constructs manager M20cd(true). Manager T0456 derives from T0556. |
| Selected map refresh and handler setup | M223b calls map method M1bc1 at `08e`; this has its own StreamReader/metadata parsing path. It calls M2258(map,true) at `0ff` before taking handler.F05f4 into Play.F138f at `110`. M2258 performs locked shared state/mod/handler configuration, not the hit-object factory chain. |
| Bind map to object manager | M223b at `182` invokes manager.M28a2(Play.F13f8, converted F13c3.F1970). M28a2 stores map into manager.F17ee and converted flags into F17ef, calls abstract factory-provider slot M28bc and stores result in F17fa, then calls M28a3. |
| Concrete osu factory | Manager T0456 overrides M28bc with M20d4, which constructs M426d(this). Factory T08c5 derives from factory base T0357; constructor chain stores manager in factory.F0e3c. M1870/M1871/M1872 are abstract circle/slider/spinner slots; M426e/M426f/M4270 match name+signature, virtual/reused-slot metadata and inheritance. Do not execute abstract RVAs as CIL. |
| Load objects | M223b calls manager.M28f5(false,false) at `18f`. M28f5 resets previous state, constructs auxiliary state, calls M28c5 to clear/dispose old objects, then reads map.F0fcd. Only if that bool is true does it call M28f7(511,true,arg2,false) at `0d4`. Here arg2 is false on the observed caller path. False can yield no parsed objects; do not assume every initialized map is loadable. |
| Stream boundary | M28f7 forms an input-name list beginning with map.F0feb, may add another source, and calls map.M1c05(name) at `161`, then StreamReader constructor at `166`. M1c05 contains ordinary path-combine/File.Exists/File.OpenRead and alternate provider branches, with exception-to-null handling. Alternate-provider semantics are not fully recovered. |

The parser mask 511 is confirmed `FileSection.All`; metadata `osu.GameplayElements.FileSection.HitObjects`=64. M28f7 uses Enum.Parse for section dispatch and ReadLine/Split/numeric conversion helpers. The exact extra-source, version compatibility, forgiving-error, culture and all section semantics are outside this expedition. In particular, scalar helper M09e3/M09e4/M09e5 behavior is not replaced by a guessed parser.

## Where rows become objects

The HitObjects path in M28f7 begins around `146d`. It clears/prepares through virtual M289f, then:

- Reads row column 3 with M09e5 and masks with **-113** (`~112`) at `1496–1498`; original colour-skip bits are separately extracted as `(Convert.ToInt32(column3, F2c2b) >> 4) & 7` at `1541–1555`.
- Reads sound/type auxiliary column 4 into a local at `149b–14ad`.
- Reads x/y (columns 0/1) through M09e3; both are bounded with explicit max(0,min(512,value)), then `conv.i4`. Both upper bounds are 512, including y. It builds Vector2(x,y), or Vector2(x,384-y) on a condition derived from current Play/argument4 and flag bit 16. This is not evidence that all original coordinate ranges are accepted unchanged.
- Reads column 2 through M09e4, `conv.i4`, then adds map.F100b at `1533–153f`. F100b is a map-owned integer time shift; its source/units are not established here.
- Tests NewCombo bit 4 and then type bits in priority order **Normal 1 → Slider 2 → Spinner 8 → Hold 128** via helper M38e1. M38e1 is `(a & mask) > 0`. These are bit tests, not equality comparisons. Circle/slider factory new-combo arguments are `M2902() OR bit4(typeBits)` (calls at `1631`/`1a79`), not the file bit alone; M2902 semantics remain opaque. A malformed multi-type row follows the first matching branch. Hold is a separate path and is not reconstructed here.

| Concept | Abstract call → concrete osu override → constructor | Initial arguments / registration |
|---|---|---|
| Circle | parser `164e`: M1870 → M426e → M20dc (`02e`) | Parsed position, shifted start, new-combo bool and sound flags are forwarded. Factory queries map.M1be6(start+5) and uses returned point.M0b76() or false as an extra bool. Factory interface also receives sample/combo metadata that this override does not forward. Parser calls M28ef at `1658`. |
| Slider | parser `1aa4`: M1871 → M426f → M424e (`038`) | Position/start/new-combo/sound plus curve-kind, repeat count, pixel length, control-point list and edge-sound list forwarded; same start+5 timing lookup. More sample lists are parsed/passed to the interface than forwarded by this override. Parser calls M28f1 at `1aae`. Detailed curve construction and tick rules remain UNRESOLVED. |
| Spinner | parser `1ba6`: M1872 → M4270 → M1d6d (`009`) | Manager, shifted start, parsed/adjusted end and sound flags forwarded; parsed x/y are not forwarded. End comes from column 5 via M09e4, may be limited to `M3b0c()-50` when parser argument3=false, then adds map.F100b. Parser sets NewCombo on returned object via M3e57, then calls M28f0 at `1bbf`. |

These concrete runtime constructors invoke substantial inherited code, including visual state creation conditioned on a non-null manager.F1800. This is not merely allocation of small data records. Asset resolution and drawing behavior were intentionally not investigated.

## Initial object properties and storage

| Property | Grounded assignment / limitation |
|---|---|
| Manager relationship | Common constructor M249b stores constructor manager in object.F152f (`02c`) and allocates collections F1530/F1531. Factory holds the same manager in F0e3c. |
| Circle position | M20dc → M45eb → M45ec: position assigned to F252c (`009`) and F152e (`010`). |
| Circle times and type | M45ec writes provided start into F2523 (`017`) and F2524 (`01e`), type=1 into F2525 (`025`), and ORs bit4 when new combo. Sound flags derive from bool tests of bits2/4/8 through M45eb/M45ec. These are initial assignments, not a judgment result. |
| Slider position and initial times | M424e → M142d stores start in F2523/F2524 (`07c`/`083`), position in F252c/F152e (`08a`/`091`), sound flags in F2526 (`0a0`). Curve kind stored in F0bd8/F0be4, repeat count via M1433 into F0bd0, length into F2528 (`179` on its applicable path), control data into inherited lists. F2524 initially equals start; this is NOT final slider duration. |
| Spinner center/times/type | M1d6d → M05ea → M3201 writes fixed center (256,192) into F252c/F152e, supplied start/end into F2523/F2524 (`02a`/`031`), type=8 into F2525; format-version field F0fd1<=8 additionally ORs NewCombo bit4. Sound flags stored in F2526. |
| Registration | M28ef/M28f0/M28f1 each call virtual M28d2(object,false,true). That method BinarySearches manager.F17fb, complements a negative insertion index, then Inserts at `0cb`, records last object F1805, conditionally adds visual collection to manager.F1800, and increments F17fc. It also adjusts neighboring combo state. Equal-time ordering/comparer semantics are not proven here; do not assume stable ordering. |

There are compatibility/mode-dependent transforms and virtual methods on these paths. The table is not a complete constructor contract: defaults in ancestors, postprocessing, exceptions and further methods can change state. In particular, slider final end time, head/repeat/tail geometry and full property semantics require further research.

## Construction is not readiness

After parsing, M28f5 calls virtual M28e9 at `269`. Its base body writes F17fc from list.Count, calls M28ea and then M28ee(0,-1). For map format-version field F0fd1>5, M28ea enumerates registered sliders and invokes M144a(true) at `02e`, then M28eb; older formats take M28ec. M144a contains end-time writes. Exact duration equations and compatibility effects are not recovered here. M28ee also performs object/visual follow-up. Count and objects can therefore be subject to postprocessing, not just parser allocation.

M223b then scans registered start/end fields to derive map bounds (Play.F13c0/F13bf), sets other component references, calls handler initialization M0aaf, performs further setup, calls M2258(map,false), and invokes M28e8(true). Several conditions can return early or fail. M0708 handles work exceptions and queues completion through M070a/M070d. M223c has distinct success/failure paths; success sets Play.F13f0=true at `05d` and performs further handler/UI preparation, while failure requests a return mode and reports the stored error. M226b is not proven ready merely because object constructors ran; wrapper completion and update gates still matter. Audio clock, first visibility, input binding and judgment activation are **UNRESOLVED**.

## Four Play variants: shared start does not imply identical behavior

| Play constructor | Initialization/work/completion dependency |
|---|---|
| M222e | Base M2237 creates virtual work M223b and completion M223c delegates. |
| M0715 | M071c directly calls M2237; inherits work slot, overrides completion with M071d, which eventually calls base M223c. Its update also differs. |
| M4ab9 | M4abf directly calls M2237; inherits work slot, overrides completion M4abe, which calls base M223c after additional state changes. |
| M23f0 | M23f7 conditionally returns before base M2237; M23f8 overrides work and calls base M223b before extra operations; M23fb overrides completion and calls base M223c. Constructor reads original `bMatch.playMode` into the ruleset setter. This path is not identical to base Play. |

Only the base/ordinary Osu ruleset-to-manager-to-factory chain is resolved here. Alternate ruleset factories, all variant conditions, map selection changes while loading, collection comparers and transitive native/reflection paths are not generalized from it.

## Validation interface and next experiment

Inputs for a future original-reference test: pinned executable, selected map identity/F1a0f, Play.F13f8, ruleset F1371, manager map/flags/factory F17ee/F17ef/F17fa, loadability F0fcd, map.F100b, format version F0fd1, raw object row and parser mode flags. Outputs to log: factory target, returned type, position, start/end/type fields, manager relationship, insertion order/count, postprocessing values and completion result. All natural reachability and dynamic values remain unobserved.

Branch-derived expectations, NOT executed original tests:

1. Changing global selection after initializer capture must not be assumed to replace Play.F13f8; work also re-reads selector for one conditional check.
2. Osu mode chooses handler M1ecb and its concrete manager/factory; Taiko/Catch/Mania must not use that factory by assumption.
3. F0fcd=false bypasses this loader's parser call.
4. A Normal row reaches M1870; a Slider row M1871; Spinner row M1872. Combined Normal|Slider follows Normal first.
5. Both x and y exhibit explicit bound 512 before optional 384-y transformation on this path.
6. Normal object's two time fields are initially written from the same start argument; spinner uses a separate end and fixed center.
7. Slider construction's initial start=end is not the expected final value after postprocessing.
8. Factory return precedes registration; registration uses BinarySearch/Insert rather than unconditional append.
9. Missing stream/invalid scalar/constructor failures may prevent completion; do not fabricate empty-success semantics.
10. Original fixture should log a circle, slider and spinner before expecting any visible or hittable scene.

Smallest next gameplay investigation: recover the **slider initialization/postprocessing boundary M28ea → M144a**, specifically how length, repeats, timing points and velocity set end time before the first active update. Use one circle as a control and one slider as the discriminating case on the pinned Windows original when available. First visibility/judgment remains a separate future boundary, not inferred from this report.

# 3. Pseudocode

CONFIRMED static callsites; RECONSTRUCTED conceptual names. Opaque operations preserve unknown behavior. This is documentation only; no replacement parser or object implementation exists.

```text
Play.Initialize [base path]:
    this.map = SelectedMapGetter_M2c63()   # reads F1a0f
    perform original mode/flags/visual preparation
    create work wrapper with virtual Work_M223b and Complete_M223c
    # wrapper queues work; exact scheduler/thread policy UNRESOLVED

Play.Work [ordinary Osu ruleset projection]:
    choose ruleset handler; Osu => M1ecb(this)
    handler base ctor invokes virtual manager factory
        Osu override M1ee1 => M20cd(true)
    refresh selected-map metadata through M1bc1
    configure handler/map state through M2258(map, true)
    manager = handler.F05f4
    manager.M28a2(map, original converted flags)
        store map and flags
        virtual M28bc => Osu M20d4 => factory M426d(manager)
    manager.M28f5(false, false)
        reset/dispose old state
        if map.F0fcd:
            M28f7(FileSection.All, true, false, false)
        perform original postprocessing and return status
    perform original map-bound scan and handler setup
    # wrapper later dispatches virtual completion; success is not guaranteed

M28f7 [HitObjects branch projection only]:
    obtain map source stream through map.M1c05(name)
    read lines and parse section/row values with original helpers
    derive bounded integer coordinates and conditional vertical transform
    start = original_numeric_conversion(column2) + map.F100b
    typeBits = original_conversion(column3) & ~112
    newCombo = bit4(typeBits)
    if bit1(typeBits):
        factoryNewCombo = M2902() OR newCombo
        obj = virtual factory.M1870(position, start, factoryNewCombo, other parsed arguments)
        # ordinary Osu override => M20dc(runtime circle ctor chain)
        manager.M28ef(obj)
    else if bit2(typeBits):
        factoryNewCombo = M2902() OR newCombo
        obj = virtual factory.M1871(position, start, factoryNewCombo, other parsed slider arguments)
        # ordinary Osu override => M424e(runtime slider ctor chain)
        manager.M28f1(obj)
    else if bit8(typeBits):
        end = original parsed/adjusted spinner end + map.F100b
        obj = virtual factory.M1872(start, end, parsed arguments)
        # ordinary Osu override => M1d6d(runtime spinner ctor chain)
        set original NewCombo property
        manager.M28f0(obj)
    else:
        retain separate Hold/unsupported branches; not reconstructed here

Registration [shared normal call projection]:
    M28d2(obj, false, true)
    locate insertion index by original BinarySearch
    Insert into manager.F17fb; update neighbor/visual/count state

Postprocess:
    invoke virtual M28e9
    base implementation calls version-dependent slider pass and M28ee
    final slider duration and first gameplay-ready update remain UNRESOLVED
```
