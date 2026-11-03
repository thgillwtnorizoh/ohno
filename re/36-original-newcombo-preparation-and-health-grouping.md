# 1. Result

Expedition 036 — Original NewCombo Preparation & Health Grouping. Work 1,
2026-11-03 Asia/Bangkok. Base main `8dc3a879bdc394a1f6b372fe172ab325fd9ffa6a`,
verified by fetching origin. Original-binary research only; no reconstruction fix.

**CONFIRMED STATIC:** M2902 is a side-effect-free test of the manager's last
registered object's runtime type. Fresh ordinary loading makes the first circle
or slider NewCombo; a circle or slider immediately following a spinner also
receives NewCombo independently of file bit4. A separate postparse pass can set
NewCombo after a break. Therefore the factory argument alone is not always the
final prepared property used by health calibration.

Deliverables: this report, `evidence/36/newcombo-preparation.json`,
`tools/newcombo_preparation_evidence.py`, `tools/check_newcombo_preparation.py`,
and an append-only E036 journal entry. Publication commit and verified remote
status are supplied in the final handoff, avoiding a self-referential commit hash.
No authorised exception. Earlier research, reconstruction, QA/fixtures/workflow,
QA07 failure logs, R09-C01 and the historical green 513-case checkpoint are retained.

## Inputs, procedure and verification

Actual archive/executable SHA256, verified **before** E036 static inspection:

| Input | SHA256 |
|---|---|
| `osubackup.zip` |7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| ZIP member `osu!.exe` |6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|

Stable40/file version1.3.3.8 are the retained self-description; official dated
build/authenticity/unmodified status remains UNRESOLVED. Analysis host is Windows,
Python3.12; dnfile0.18.0/pefile2024.8.26/dncil1.0.2. No game launch, CLR-method
execution, gameplay input, OBS, game window or debugger instrumentation occurred.
E20 remains incomplete, all17 original-runtime cases NOT RUN. No original-runtime
measurement or new execution of the 513-case component gate is claimed.

The user-provided AGENTS instruction points to RTK.md, read in full; no repository
AGENTS.md exists. Preparation consulted the standing contract, cumulative record,
E04/E12/E19/E27/E035, R09/R09-C01, QA07 and relevant E05–08/E21 boundaries. E05's
correction of E04's ordinary/Taiko factory mapping is preserved here.

Competing explanations tested against original instructions: timing-dependent
versus type-dependent inference; short-circuit versus always-evaluated helper;
file-only versus constructed flag; registration-only versus break postprocessing;
list clear versus actual last-reference reset. The extractor verifies pins before
decoding, checks branch targets and inventories direct managed field accesses and
target calls across19659 bodies/1739 zero-RVA methods, with zero decode errors.
The independent checker pins literal method/branch/operand relationships and
compares overlapping historical body hashes. No replacement parser or HP simulator
is executed. Original bytes and complete disassembly remain private.

Reproduce (dependencies installed privately; PYTHONPATH may point to them):

```text
python re/tools/newcombo_preparation_evidence.py /path/osubackup.zip /tmp/newcombo-a.json
python re/tools/newcombo_preparation_evidence.py /path/osubackup.zip /tmp/newcombo-b.json
python re/tools/check_newcombo_preparation.py /tmp/newcombo-a.json
```

Actual extraction/checker/repetition and preservation outcomes are recorded in the
journal after execution. The JSON contains method identities/signatures, names,
RVA/body hashes, operands, field owners/signatures, branches, argument/local sites,
slot candidates and direct callers. Report offsets are **hexadecimal CIL-relative,
headers excluded**; JSON offsets are decimal. M/F/T abbreviate EXE MethodDef
0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx.

# 2. What they found

## A. Exact helper and state

M2902 is declared on **T0556**, original name
`#=zU_2mZRjtyEtLPd4Gazd9qlLkItf$`; its obfuscated method name is escaped in JSON.
Signature `200002` = instance Boolean(), nonvirtual, RVA10ebe4,38 code bytes,
one-byte header, body SHA256
`3d34c695021f9f3e85f3614df524072bde01394675edb95e030d44de9b0df46b`.
Its complete behavioral instruction chain is:

| Offset | Original operation / consequence |
|---|---|
|0001;0006|Read F1805; null branches to0024 |
|0009;000e;0013|Read F1805 again; `isinst T067f`; nonnull branches to0024 |
|0016;001b;0020–0023|Read F1805 again; `isinst T081f`; compare reference to null with `cgt.un`, return Boolean |
|0024–0025|Load1, return |

There are **no calls, field writes, locals, timing/version/mode/mod reads**. The
first two tests short-circuit the third, but repeated F1805 reads must be retained
when describing arbitrary concurrent mutation. Under stable references its result
is `last == null || last is T067f || last is T081f`.

T067f (original `#=zcGutR9dXPdF3KJ1GJU$oqCTl3m5lK1miBg==`) is the spinner
base reached by ordinary T00d8: M48e1→M05ea→M3201. T081f's exact original name and
ancestry are retained in JSON; it derives from T052c, whose NewCombo is a separate
F174a property (M27b7/M27b8), not necessarily F2525. The helper tests **assignable
runtime type**, not Spinner8 or another bit in F2525. Do not translate T081f to an
ordinary row category without resolving its different factory; its gameplay
meaning is outside the ordinary control. An artificially supplied T081f receiver
nevertheless unambiguously satisfies the helper.

F1805 is declared by T0556, signature `0612930c` (T04c3 reference). The complete
direct managed inventory finds one store: **M28d2:00d7**, assigning the object
just inserted at00cb. Thus “last” means last registered, **not** the final indexed
object after sorting, not a slider head and not the last judged object. M2902's
three reads are the only direct reads found. No direct address-taking/reset writer
is present. Ordinary circles/sliders are neither T067f nor T081f.

### Initialization and fresh/reused load boundaries

Fresh managed manager allocation supplies null F1805. M289a initializes lists and
base state but does not assign that field; ordinary M0651 reaches it. M28a2 binds
the map/factory, also without resetting F1805. M223b:018f calls M28f5(false,false)
on the newly selected ordinary manager. M28f5 calls M28f6:0001, configures state,
clears objects through M28c5:00b4, then reaches M28f7 at00d4 if loadable. **Neither
M28c5 nor M28f6 resets F1805.** The latter is clock/host helper work, not a
last-object reset. Ordinary T00e8 inherits that slot; its only other matching
override is T084e.M3f7e.

M28f7 itself does not reset F1805 at entry, first HitObjects row, section boundary
or source boundary. Its HitObjects rows are processed only for source index0
(146d–1470); colour preparation M289f at147b does not clear the reference.
Whole-EXE callers of M28f5 are M223b:018f, wrapper M28f4:0003, M41d6:006d and
M48d4:0014. They share that no-reset loader; their broader lifecycle/configuration
is not asserted identical to ordinary fresh Play. Direct M28f7 callers and
M28f8 copy-loading callers are enumerated in JSON.

**Implementation constraint:** distinguish a genuinely fresh manager from reuse.
A list.Count of zero does not establish null F1805. On reuse after a prior circle,
the first new bit4-free circle can get false at construction; after a prior
spinner it gets true. Fresh load/restart via a newly constructed manager starts
null. Exceptions before registration do not advance F1805; an exception after
00d7 can leave it advanced. Reflection/native mutation and arbitrary concurrent
callbacks are outside the direct managed inventory proof.

## B. File bit, factory argument and stored property

M28f7 reads row type, masks with signed **−113 (~112)** at1496–1498, and stores
local0x21. M38e1 at155b computes `(type & 4)>0` into local0x22 at1560. Colour skip
is separately `(originalType >>4)&7`; inference does not make an unmarked row
behave as explicitly marked for that separate argument. Type priority remains
Normal1, Slider2, Spinner8, Hold128; malformed combinations are not equality tests.

| Path | Exact evaluation order |
|---|---|
| Circle |Load factory/position/start; M2902 at1631; load file Boolean1636; bitwise `or`1638; sound and other arguments; M1870:164e; register M28ef:1658 |
| Slider |Load factory/position/start; M2902 at1a79; load file Boolean1a7e; bitwise `or`1a80; remaining parsed arguments; M1871:1aa4; register M28f1:1aae |
| Spinner |M1872:1ba6 constructs without M2902; M38e1(type,4):1bb2; M3e57:1bb7 explicitly assigns that Boolean; M28f0:1bbf registers |

The circle/slider helper is **always invoked on reaching that argument expression**,
even when explicit bit4 is true. It can be skipped by earlier parsing/type/error
branches, not by a short-circuit file-bit test. A third direct M2902 call at1cc5
belongs to the separate Hold branch; no universal all-object OR rule follows.

Correct **ordinary** resolution (E05, not E04's Taiko path): T00e8 factory M0652
constructs T09c1 via M48de. Slots M1870/71/72 resolve to M48df/e0/e1. Circle:
M48df:000d constructs M45eb, which forwards Boolean arg4 unchanged to M45ec:0020.
M45ec writes F2525=1 at0025; if arg4 true (003f–0041), OR4/store004c. Slider:
M48e0:0017 constructs M142d with the same Boolean as arg4; M142d writes type2
at0192, conditionally OR4/store01a4 after0197–0199. Neither path imports the raw
row type wholesale. Factory sample/sound/colour metadata remain distinct.

Shared field **T0828.F2525**, signature `06119d18`, holds type flags. M3e56 is
virtual instance Boolean(), signature200002; at0002 it calls M3e4e(4), whose0001
reads F2525 and0007 calls M38e1. M3e57 is virtual instance void(Boolean),
signature20010102: true OR4/store000c; false AND−5/store001c. Ordinary T094c,
T02af and spinner T00d8 inherit these property implementations through their
recorded ancestry. The only same-signature slot replacements are on T052c,
T0553 and T0747, outside these ordinary receiver chains. This is virtual
resolution by ancestry plus slot metadata, not a shared-name assumption.

Spinner M3201 writes8 at0038 and, for map.F0fd1<=8, OR4/store0059. **The parser's
later M3e57(fileBit) overwrites that property**, including clearing the constructor's
old-version NewCombo when the file bit is absent. The <=8 constructor branch is
real but is not the final parser rule. Following circle/slider inference is true
because F1805 is a spinner base instance regardless of that spinner's own bit4.

Registration M28ef/f0/f1 invokes M28d2(object,false,true). M28d2 BinarySearches at
004c, complements negative result, marks a predecessor's **F252e** at0079 when
the new object is NewCombo, and advances insertion past equal starts before
Insert00cb/F1805=object00d7. F252e is a separate terminal/group field: these
operations do **not** set NewCombo. Equal-time search uses the original E07
comparer (NewCombo true before false at equal start); do not replace actual
registered order with unconditional file append or a guessed stable sort.

## C. Later preparation can change NewCombo

M28f5 unconditionally reaches **M28db(false) at015c** on its normal continuation,
before virtual M28e9:0269. M28db's false argument skips its optional sort/rebuild
arm (0021–0049). It walks the existing registered list and the prepared break list
manager.F17f9.F1b9e. This is the same break family used by E035; each record has
start F0875/end F086e, and M0e7d computes their difference. M2efd adds original
EventTypes.Break2 records to F1b9e and sorts through its original helper. E036
consumes the actual prepared order, rather than assuming arbitrary raw event order.

Break index starts **−1** at0050, not0. For each current object:

1. Jump0086→0095 checks whether index+1 is in range (009b bge→00be).
2. Load next break's end F086e at00b1 and current start F2523 at00b7.
3. **Signed strict `blt.s`00bc→0088**: only if break.end < object.start,
   increment the break index and call current.M3e57(true) at0090; repeat check.
4. Equality leaves the break pending for a later object. Multiple expired breaks
   can be consumed by the same object and repeatedly set its flag true. A pending
   break whose end is not less blocks subsequent list entries from inspection.

This pass never clears ordinary NewCombo. It also assigns combo numbers/colours
and final F252e; those writes must not be confused with the property. Its first
ordinary object's combo-number initialization is **not** another M3e57(true).
Its version<=8 spinner logic at00eb–00fc affects its numbering state, not F2525.
NewCombo break forcing applies before the spinner/nonspinner numbering branch.

E05's M28e9→M28ea→M144a/M28eb or old M28ec, and M28ee, prepare slider geometry,
timing/stack/visual relationships; they have no direct F2525 store or M3e57 call.
Original direct writer inventory includes alternative factory M12f9, alternative
slider M18e2, hold-related M17d8/M1c91 and T081f M3dfa, plus ordinary constructors
and M3e57. These bodies/owners/hashes and **all** direct setter callers are retained
in JSON; they are not silently declared unreachable in every mode.

Other setter callers include editor/group edits M28df, clone/copy M05ff/M28f8,
and editor operations such as M3518. M28e0 contains a setter but its local0 starts
false and is never set true in that body, so its conditional setter is not a
fresh-load forcing rule. M28f8 copies source M3e56 into new circle/slider arguments
and explicitly copies spinner bit4; it does not invoke M2902. It must not be
represented as a fresh text parse. Ordinary new Play uses the investigated text
path. General editor/alternate ruleset/mutating subscribers remain boundaries.

## D. Health consumes the prepared property

**CONFIRMED:** E035 M1150 uses handler.F05f4.F17fb, the registered list populated
above. At0355, last-index equality jumps to0371 without reading a next object.
Otherwise0365 fetches list[i+1]; **036a callvirt M3e56** reads the final property,
and036f brfalse→03c6 selects ordinary reward. True/last loads raw1028 at0372
and calls M0ad2:0378; false loads raw1024 at03c7 and calls it at03cd.

M223b loads through M28f5 before handler M0aaf initialization/calibration (E035).
Thus it consumes the post-M28db state, not a second file-bit parse or the earlier
factory Boolean. The same ordinary slot is used by E12 group judgement. E036
changes no E035 HP arithmetic, low-group policy, trial order, coefficients,
snapshot updates, retry conditions or fresh live reset. In particular, next flag
ends the **current** object's calibration group; the first object's flag is never
consulted by this forward boundary test.

## E. Finite independent reference cases

All rows are **RECONSTRUCTED conditional predictions from CONFIRMED STATIC sites**,
not original-runtime measurements. Fresh ordinary manager, strictly increasing
start times, normal returns and no breaks unless stated. C=circle, L=slider,
P=ordinary spinner, suffix0/1=file bit4 absent/present. Values are final prepared
NewCombo in registered order. “Rewards” means per-object group-choice raw only,
not full HP/trial arithmetic. No artificial original state was actually installed.

| ID / input | Constructed → final flags | Calibration reward choices / discriminating evidence |
|---|---|---|
|N01 C0|true→true|[1028], last; null F1805 atM2902:0006 |
|N02 C0,C1|[true,true] unchanged|[1028,1028]; explicit following bit4; helper still called |
|N03 C0,C0|[true,false] unchanged|[1024,1028]; previous circle does not satisfy either isinst |
|N04 C0,L0|[true,false] unchanged|[1024,1028]; circle-to-slider, no inferred type-change combo |
|N05 C0,L1|[true,true] unchanged|[1028,1028] |
|N06 L0,C0|[true,false] unchanged|[1024,1028]; slider-to-circle |
|N07 L0,L0|[true,false] unchanged|[1024,1028]; consecutive sliders |
|N08 C0,L0,C0,L0|[true,false,false,false] unchanged|[1024,1024,1024,1028]; last-registration state advances each row |
|N09 C0,P0,C0|[true,false,true] unchanged|[1024,1028,1028]; spinner not automatically NewCombo, following circle is |
|N10 L0,P1,L0|[true,true,true] unchanged|[1028,1028,1028]; spinner file bit and following inference independent |
|N11 P0,P0,L0|[false,false,true] unchanged|[1024,1028,1028]; consecutive spinners bypass M2902 |
|N12 format8 P0,C0|spinner temporary12→8, then circle5|[1028,1028]; parser clears old constructor bit, next inference still true |
|N13 format9 P0,C0|spinner8, circle5|Same choices; no <=8 constructor OR |
|N14 C0@1000,C0@3000; prepared break end2500|[true,false]→[true,true]|[1028,1028]; M28db strict break-end test |
|N15 C0@1000,C0@3000,C0@3001; break end3000|[true,false,false]→[true,false,true]|[1024,1028,1028]; equality pending, then consumed |
|N16 C0@1000,P0@3000,C0@5000; break end2500|[true,false,true]→[true,true,true]|[1028,1028,1028]; break forcing applies to spinner too |
|N17 C0@1000,C0@5000; two ordered break ends2000/4000|[true,false]→[true,true]|Two setter calls on same current object; not two synthetic objects |
|N18 reused manager whose last reference is circle; new C0|false→false absent break|[1028]; **artificial retained-state setup**, no null reset from list clear |
|N19 same but last spinner|true→true|[1028]; artificial retained-state setup |
|N20 last assignable T081f; new L0|true→true|[1028]; **artificial type operand**, alternate factory/natural ordinary reachability unresolved |
|N21 out-of-order file rows / equal starts|Compute constructor flags in registration sequence, then break forcing in actual list order|Do not use these increasing-time tables to invent insertion tie behavior; R09 ordering guard retained |
|N22 unknown manager reuse/source clone/alternate factory|Unavailable without actual retained property/type/order inputs|No speculative default, no original runtime measurement |

Version-dependent slider preparation/count/duration and R09's spinner version
support remain independent admission conditions. N12 is a static parser case,
not an assertion that R09 already supports v8 spinner gameplay. Artificial N18–20
establish the direct method result only; wider natural lifecycle is UNRESOLVED.

## F. Work 2 implementation interface and eligibility

Minimum **additional** grouping inputs for the bounded ordinary text-loader
projection: original file-order accepted object identities/raw type flags; resolved
ordinary kind/type of each actually registered object; initial last-reference
category (fresh null versus retained spinner/T081f/other); actual registered order;
prepared ordered break-end values in the **same shifted time domain** as starts.
Retain existing chart format metadata for spinner constructor/path admission,
and existing E035 prepared starts/ends/component counts/approach/break inputs.
Timing/BPM/SV, HP/mods, colour skip and previous object's NewCombo are **not inputs
to M2902 itself**. Previous runtime type is required. Postparse break forcing uses
registered starts and pending break ends, independently of calibration's one-break
gap-subtraction policy; do not merge those two loops.

For fresh ordinary supported increasing-time circle/slider charts, helper true
only at the first accepted object or after a registered spinner (T081f unavailable
in this bounded input domain). Other ordinary C/L factory values equal the file
bit; then apply M28db break forcing to final flags before calibration.

**Newly supportable after Work 2 implements this evidence:** R09-compatible ordinary
charts with following C/L rows lacking bit4, including consecutive C/L and mixed
transitions, supported spinner adjacency, and supported ordered breaks that force
additional prepared combos. This removes R09's M2902-unknown restriction, not its
independent ordering/overlap/finite/count/geometry/version/break/work-budget limits.
The original three-circle fixture's grouping becomes determinable; whether any
chart initializes health still requires **all** existing preparation guards.

Do not modify R09 as part of E036. Retain unavailable health for unknown retained
manager state, alternate ruleset/factory/clone/edit mutations, unsupported registered
order and unresolved preparation operands. No guessed grouping fields or fallback
HP coefficients follow. Original static completeness does not establish actual
imported runtime objects, callbacks, x86 marginal numerical behavior or visible HP.
No new runtime experiment is proposed for execution under this assignment.

# 3. Pseudocode

Independent **RECONSTRUCTED** descriptions of the cited original instructions;
not executable reconstruction, not a universal all-mode parser.

```text
M2902(manager):
    if Read(manager.F1805) == null: return true
    if Read(manager.F1805) is T067f: return true
    return (Read(manager.F1805) is T081f)

ordinary text circle/slider row after prior parse gates:
    maskedType = originalType & -113
    fileNewCombo = M38e1(maskedType,4)
    helperValue = M2902(manager)          // always evaluated when reached
    factoryNewCombo = helperValue OR fileNewCombo
    obj = original ordinary factory(...,factoryNewCombo,...)
        circle constructor: type=1; if factoryNewCombo: type |=4
        slider constructor: type=2; if factoryNewCombo: type |=4
    original register(obj,false,true)
        BinarySearch and original equal-time insertion handling
        preserve F252e and unrelated registration work
        Insert(obj)
        manager.F1805 = obj

ordinary spinner row:
    obj = original spinner factory(start,end,sound,...)
        type=8; if format<=8: type |=4
    obj.M3e57(M38e1(maskedType,4))        // sets OR4 or clears AND -5
    original register(obj,false,true)    // advances last reference

normal loader continuation M28db(false):
    nextConsumedBreakIndex = -1
    for obj in actual registered order:
        while nextConsumedBreakIndex+1 < actual break count
          and breaks[nextConsumedBreakIndex+1].end < obj.start:
            nextConsumedBreakIndex++
            obj.M3e57(true)
        preserve original numbering/colour/F252e preparation
    retain original slider/timing/visual postprocessing

fresh ordinary M1150, after loading/postprocessing:
    preserve E035 trial walk, components, arithmetic and retries
    for indexed object i:
        if i == storedCount-1: groupEnd=true
        else: groupEnd = registered[i+1].virtual M3e56()
        original M0ad2(groupEnd ? raw1028 : raw1024, original operands)
        preserve E035 thresholds/coefficient/snapshot/retry order
```

**UNRESOLVED:** natural runtime/imported operands and interleavings; alternate
factory/T081f product semantics; general manager reuse/editor/clone lifecycles;
native/reflection mutation and exceptional continuation. E035 HP arithmetic and
deferred scoring mismatches remain unchanged. Stop at E036; no E037/R10/correction.
