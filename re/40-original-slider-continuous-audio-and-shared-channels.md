# 1. Result

Expedition 040 — Original Slider Continuous-Audio Timing, Shared Channels & Lifecycle.
Work1, 2026-11-05 Asia/Bangkok. Original-static research only. Verified canonical
`main` starts at `040ffbf85b5f1f758af0e7bf5117eb446d24bb0c`. No authorised exception.

**Recovered:** the two timing-list cursors and separate global sample index; timing
advance/rescan predicates; sample-bank refresh; continuing resource preparation;
shared channel replacement; row eligibility; deferred start/pan/pause callbacks;
gain operands; ordinary manager guard; reached pause/reset/cache teardown boundaries;
and their ordering around E039 discrete audio. There is **no per-slider channel
owner** in this direct managed chain. Both producers address the same global pair.

The managed transition model supports a bounded prospective R13 subset with explicit
global state and an injected persistent-channel backend. Native scheduling, actual
overlap, device changes and audible equivalence remain **UNVERIFIED**. R13 is not
started. R12 production, development checks, QA01–QA10, earlier research/fixtures,
scoring/HP and E020's17 NOT RUN cases are unchanged. No game, original CLR/native
method, debugger, physical input, OBS, audio capture, playback or listening ran.

Created this report, `evidence/40/slider-continuous-audio.json`,
`tools/slider_continuous_audio_evidence.py`, `tools/check_slider_continuous_audio.py`,
and an append-only E040 journal entry. Exact verified publication SHA is supplied
in the handoff. Original source bytes remain private; no audio contents or complete
disassembly are distributed.

| Pinned input verified before instruction decoding | Bytes | SHA256 |
|---|---:|---|
| `osubackup.zip` |78546961|7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
| ZIP member `osu!.exe` |4541728|6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|

Self-declared Stable40/file version1.3.3.8 remains unauthenticated as a dated,
unmodified official release. Host: Windows, Python3.12; existing read-only
dnfile0.18.0, dncil1.0.2, pefile2024.8.26. No CLR is loaded. Preparation uses the
standing contract/AGENTS/directed RTK, both journals, E005/E010/E015–E018/E021–E023,
E037–E039 reports/evidence, R11/R12, QA09/QA10 and unchanged current parser,
session/sample/resource/Windows adapter interfaces. E005's ordinary factory
correction remains authoritative; the Taiko factory is not substituted.

Competing explanations checked in original CIL: per-tick timing lookup versus a
global cursor; sample index versus timing-list index; per-slider ownership versus
shared channel fields; every-call playback versus native-state suppression;
immutable queued handles versus later rereads; tracking-false Stop versus queued
Pause; automatic gain refresh versus refresh only on reached write paths; complete
reset versus partial selector/cursor reset; and terminal audio versus aggregate
cleanup followed by another continuing call.

## Deterministic verification

The evidence contains119 selected MethodDefs with signatures, types, RVAs, body
hashes, token/constant/branch/comparison sites, virtual declarations, direct watched
field/caller inventories and MethodSpec resolution. Full scan: **19659 decoded
bodies,1739 zero-RVA entries,0 decode errors**. Selected branch targets are checked
against instruction boundaries. The independent checker pins literal relationships,
the complete **26 direct channel-field accesses**, all direct assignments, native
imports and historical body hashes. Archive mode independently decodes original
instructions and inventories critical globals, without calling the collector.

Two independent fresh final extractions are byte-identical and match the published
JSON: **PASS**,1151511 bytes/SHA256
`c4f5219a5f3ab36ec184335135fa9f233f7bc3b340905aeef2f4e555344b387b`.
Literal relationships and independent archive-backed instruction/global-inventory/
native-import checks: **PASS**,119 methods/26 channel accesses/**428 historical
body-hash comparisons**. E038 archive-backed checker: **PASS**,56 independently
decoded strings and12 historical embedded keys. E039 archive-backed checker:
**PASS**,92 methods/463 historical body comparisons after the new evidence was
present. Unchanged E005/E010/E015–E018/E021–E023/E037/E038/E039 JSON relationship
checkers: **all12 PASS**. E023's actual tool is `check_aggregate_policy.py`; an
initial filename probe for `check_slider_aggregate_policy.py` found no file and
executed no checker. The correct unchanged pairing passed. No historical assertion
was modified. New tools compile in memory; report sections, journal-prefix and
changed-path preservation checks are performed before publication.

The56 controls in JSON are hand-derived **RECONSTRUCTED STATIC
PREDICTIONS**, not original-runtime experiments or additional passing QA cases.
The historical1091-check checkpoint includes QA10's four source-only audits and is
not rerun by this expedition. No WPF build or new Windows gameplay gate is claimed.

Reproduce from repository root, with the pinned packages already available:

```text
rtk proxy python re/tools/slider_continuous_audio_evidence.py /private/osubackup.zip /private/e040-a.json
rtk proxy python re/tools/slider_continuous_audio_evidence.py /private/osubackup.zip /private/e040-b.json
rtk proxy python re/tools/check_slider_continuous_audio.py /private/e040-a.json /private/osubackup.zip
```

Actual host commands prepend `../.research-deps` and `re/tools` to Python's module
search path; no installation, executable launch or native loading is involved.

# 2. What they found

**CONFIRMED STATIC** means pinned original instructions/metadata. Semantic aliases,
equations and conditional controls are **RECONSTRUCTED**. Future backend policies
are **ENGINEERING**; original runtime/native success/output remain **UNVERIFIED**.
M/F/T abbreviate EXE MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx.
All report offsets are hexadecimal relative to the first CIL opcode, excluding
headers; JSON stores decimal offsets. Fresh reads below must not be collapsed into
one immutable frame snapshot. Arithmetic sites are unchecked unless stated.

## A. Exact ordinary boundary and state identities

Corrected ordinary chain: M113e → T00e8/M0651 → M48de → M48e0 → T02af/M142d.
The parent's update slot M24b2 resolves M1449, sound M24bb resolves M1466, pan
M24bd resolves M1467, and cleanup M24c4 resolves M146c. JSON retains ancestry,
matching signatures/reused slots and MethodImpl metadata. M146b/M3b33/M3b20 are
concrete calls; M3b33 and M3b20 are static on T07b9.

| Original field | Declaring type / signature | Recovered role |
|---|---|---|
|F2354/F2355|T07b9 /0608|Static Int32 continuing channels0/1|
|F238e/F238f|T07b9 /0608|Static timing-list cursor for base records / all records|
|F2360|T07b9 /06118d7c|Current **sample index** (T035f enum); not a timing-list position|
|F2362|T07b9 /06118d7c|Last sample index compared by continuing replacement|
|F2361|T07b9 /0611ac3c|Current global SampleSet bank|
|F2363/F2364|T07b9 /0611ac3c|Last selected base/addition continuing banks|
|F236b|T07b9 /06118d7c|Fallback sample index when current point/version is unavailable|
|F2367|T07b9 /0612a00c|Song receiver, base T0803; its F24ad refers to current song map|
|F100f|T03ad /06151282c101128668|Song-map List<T019a> timing records|
|F0662/F0661/F0663/F0665/F0666|T019a|Double time / sample index / SampleSet / Int32 volume / base-record Boolean|
|F2376|T07b9 /061408030003000000|Int32 continuing sample table[2,3,3]|
|F234e/F2350/F234f|T07b9|Map/custom, skin, embedded sample caches respectively|
|F234b|T07b9 /061299c0|Shared action scheduler, concrete T0670 constructed by M3b21|
|F239b|T07c3 /0615118801010c|Per-call captured Nullable<Single> pan operand|
|F136e|T047b /0602|Ordinary selected-current-object slider predicate used by manager cleanup|

M3b08 is T07b9's `.cctor`: allocates caches and F2376[2,3,3], initializes sample
banks F2361/F2363/F2364 to0, timing cursors to−1 (00d6–00dd). Channels and sample
index fields have CLR zero defaults, not a discovered eager channel allocation.
The full direct inventory finds **only M3b20:00a9 and00ed** assigning F2354/F2355.
No managed zeroing, owner token, per-slider channel dictionary or owner equality
test appears in their26 direct accesses. This excludes reflection/native/external
mutation and aliases; it is not a claim that native handles remain valid forever.
Neither channel field has a custom attribute, default constant or FieldRVA data;
in particular there is no ThreadStatic attribute on this pair.

Complete direct channel access inventory (all on T07b9 static fields):

| Reader/writer method | F2354 sites | F2355 sites |
|---|---|---|
|M3b09|000e read|0001 read|
|M3b11|0019 read|0026 read|
|M3b20|006d/0075/0081/010b/011d reads;00a9 write|00b1/00b9/00c5/012a/0137 reads;00ed write|
|M3b33|0023 read|0064 read|
|M3b9c|0000 read|0017 read|
|M3ba1|0000/0017 reads|—|
|M3ba2|—|0000/0017 reads|
|M3ba8|0000 read|000b read|

Other complete direct writer families: F238e/F238f are written by M3b08/M3b65/
M3b66/M3b73; F2360 by M3b18/M3b19/M3b1f; F2361 by M1bc1/M28f5/M3b08/M3b18/
M3b19/M3b1f; F2362 by M3b19/M3b20; F2363/F2364 by M3b08/M3b19/M3b20;
F236b only by M3b3d. JSON preserves each offset, read/address use and writer-body
identity. No store through an untraced alias is claimed impossible.

Representative method identities (body hashes include original headers):

| Method / owner | Raw signature | RVA / code bytes | Body SHA256 |
|---|---|---|---|
|M3b66/T07b9|00010102|183130 /309|0e50d3e151f5d094c10b7b66b938ebef770df7865781a40134faa24438c9527f|
|M3b61/T07b9|000001|183014 /48|841cb988ec36a3736fe8fe0f13e72cc94cfbf4e2e626e9918a0d7aa3dd72f00f|
|M3b1f/T07b9|00020111ac3c02|180c6c /292|0f737cebec60ae7706e3d55e790f3e61a59457c44f2b09a24890dcf38d29832c|
|M3b20/T07b9|00030111ac3c11ac3c02|180d9c /330|c50a3770060a6bda1356e4f0286bd17b16a362cc61c479a0ff8e72642ca7811a|
|M3b33/T07b9|00050111a61c11ac3c11ac3c0215118801010c|181bd8 /193|6e5458f35a8148c42375de1087f163292be2bf609d785801cf78cb28e9d1d4a0|
|M3b9c/T07c3|200001|183800 /47|bc1880ced3e1b6324e634526f340733fc2f14074d4efa27e79e4da55e7023375|
|M3ba1/T07c4|200001|183854 /36|57d241977ad89c3395b0abd9b14bf67717569c244cbcbb1afc2d5002673db5e4|
|M3ba8/T07c4|200001|183944 /23|9d2dad6be1fd31f156a1cfb091d79e95b8b43d8ddd3f6940a780e02dd5ec52d1|

## B. Global timing is a separate state machine

M3b72 returns `M3b0e()?.F100f`; M3b0e returns `F2367?.F24ad`, both with original
null branches. M3b68 reads F238f then calls M3b6a; M3b67 instead uses F238e.
M3b6a returns null for a null list, otherwise calls MethodSpec2b00018d.
That MemberRef resolves inside this module to **T098a/M47cb**, not an opaque
external LINQ policy. M47cb→MethodSpec2b0001b0→M47ca checks signed
`0<=index && index<list.Count`, returns default(T) when invalid, otherwise get_Item.
For T019a default is null. There is **no first/last fallback in M3b68**.
M3b69 is a separate helper that explicitly falls back to M3b6a(0).

### M3b66(Boolean rescan) instruction-derived order

1. Obtain song timing list at0006. Capture `unchecked(F2358+5)` in closure
   F2394 at000d–0014. This is Int32 gameplay-clock publication plus5, converted
   to Double only at comparisons. It is not song-byte position or a tick deadline.
2. Null/empty list: M3b65 sets F238e/F238f=−1, then F238b=−999 and return.
3. If rescan=true, call original `List.Sort()` at002f. M0b7e compares Double
   time; at equal time it returns **other.baseFlag.CompareTo(this.baseFlag)**,
   placing true before false. No stable sort for equal time/equal flag is proved.
4. Full rescan if rescan=true, F238e<0, or F2368!=numeric1. FindLastIndex with
   M3b89 chooses base records whose **time < Double(capturedQuery)**; M3b8a
   chooses any record under the same **strict** test. Each result is Max(0,result),
   including no qualifying record. Store F238e:006b then F238f:0088. Changed base
   index invokes M3b81; changed all-record index invokes M3b84, then F238b=−999.
5. Otherwise forward scan from **F238f+1**, using the captured query and supplied
   list order. If record.time **> Double(query)** at00e4, return immediately.
   Equality is consumed: write F238f:00e7, invoke M3b84:00ed. If record.M0b74()
   is true, then write F238e:00fb and invoke M3b81:0101. Continue all reached
   leading entries; a future head blocks later entries even if unsorted.
6. Catch System.Object over this method's try: reset both cursors and F238b. It
   does not roll back a sort, earlier subscriber effects, cache/channel changes
   or actions submitted before a callback throws. Null elements can reach this
   catch. NaN/concurrent-list cases retain the literal clt/bgt behavior; no broad
   finite-validation policy is inserted into original code.

The strict rescan and inclusive forward predicates can produce different states
at the same nominal query. This is an entry-state/control-path distinction, not
a claimed audible delay or fixed one-frame lag. M3b25's state2 arm can rescan,
set state1, then its later M3b61 can forward-scan again during the same call.
Do not combine them into one lookup. F238e/F238f also reset in M3b65; M3b73's
timing-list setter instead sets both0 after assigning song-map.F100f.

M3b81/M3b84 null-check their F2391/F2392 delegates and invoke synchronously.
Ordinary M2246 registers **M2249** to F2392, M2248 unregisters it. This subscriber
updates its original timing flag/effect state; it does **not** itself call M3b1f.
Subscribers can throw or mutate wider state. They are not a deferred audio action.

### From current timing point to sample selectors

M3b61 calls M3b66(false), then M3b68. If point exists and the **selected global
map** M2c63 exists with format>3, it passes point.F0663 to M3b1f(base,false).
The selected map and song-map are separate original references. Fresh ordinary
same-map identity is a handoff prerequisite, not an implicit single cached read.
If the point/map/version gate fails, M3b61 leaves F2360/F2361 unchanged; the timing
cursor reset does not itself clear those selectors or the channel pair.

M3b3c returns current point.F0661 only when that point and the song-map exist
and the song-map version>3; otherwise returns F236b. The parser resets fallback
to0 for the current song-map, sets format<4 default1 versus later0, and has a
separate file-flag assignment at M1bc1:0702–0718. Do not replace these with an
object's sample-index override. Modern finite handoff can exclude those old paths.

M3b1f compares requested base against F2361 and a fresh M3b3c() against F2360.
Unless forced, unchanged values return; changed values also return outside
application numeric1/2. On a reached refresh, base0 becomes Normal1, write F2361
at002f, reread M3b3c and write F2360:0039. It performs shared resource work
(including separate non-slider state, not expanded here). It calls M3b20(base,
base,force) at011e **only if forced or F2363==0**. Otherwise active-loop bank
replacement is deferred until a continuing helper reaches M3b20.

Inherited timing points can update F238f, the selected sample bank/index and gain
source while F238e remains on the prior base point. The audio bank/index selection
does not filter them out by beat-length sign. M0b73 stores the independent metadata;
supplied SampleSet0 becomes Soft2 and point volume is clamped0..100. Those constructor
facts do not authorize guessing a missing global current record.

### Ordering with gameplay

In reached M3b25: clock M3b24:0048 precedes state2 timing refresh:0056 and later
M3b61:00ae. E010's host chain then reaches Play/current-object/candidate work.
M3b61 has readiness/application gates inherited from M3b25; it is not unconditional
on every OS frame. M224d also performs a conditional selector refresh then forced
M3b66(true):0133 during initialization. Seek/reset call sites remain inventoried.
Per-request M1be6(timestamp), which scans all supplied records with `<=`, is a
different consumer. **Never derive continuing state from the tick+2/edge+5 point.**

## C. Continuing resources and preparation

M146b initializes local base/addition0. Format>=14 reads parent F1537/F1538;
older formats keep0. It passes parent raw F2526, those selectors, unguarded skin
F2f16.F36b0 LayeredHitSounds, and parent virtual pan multiplied by.4000000059604645f
as Nullable<Single> to M3b33. Parent index, volume, custom handle and edge lists
are **not its continuing sample selectors**.

M3b20 selects only Normal1 and Whistle2 with **mode2**. M3b36 maps Normal1→row0
and Whistle2→row1. SampleSet Normal1→bank0, None0/Soft2→bank1, Drum3→bank2,
other values→bank0 for table access. Index Max(input,0)<=2 reads F2376[row,bank,index].
Metadata labels planes Default0/Custom1/Custom2. Dynamic index>2 builds:

```text
Normal1:  prefix + lowercase(SampleSet.ToString()) + "-sliderslide" + index
Whistle2: prefix + lowercase(SampleSet.ToString()) + "-sliderwhistle" + index
```

Confirmed decoded string provenance: identifiers2064529198=`sliderslide`,
2064529212=`sliderwhistle`,2064528794=`slide`,2064528790=`{0}{1}-slider{2}{3}`.
They are independently decoded by unchanged E038 StaticStrings from pinned bytes;
source resource/key/cipher fingerprints and original call sites are carried in JSON.
Ordinary Osu0 prefix is empty; the separate Play/Taiko prefix is not applied here.
Parameterless ToLower/Format retain current-culture behavior and enum naming.

M3b36 checks F234e for the dynamic key at01c4–01da. Found non−1 returns, **including0**.
Missing/−1 returns corresponding F2376[row,bank,0]:020f–021c. It does not probe a
filename or load on this request path. No nearest custom-index fallback is present.
Mode0 uses F2374 circle/edge rows; mode1 uses F2375 tick row. They are distinct
tables/keys. The reached mode2 caller does not manufacture Finish/Clap loop rows.

M3b19 prepares all **18** continuing cells while cycling planes0/1/2 and banks1/2/3:
row0 load at00e5–00f7 then row1 at0104–0116, both M3b14(loop=true,false,7).
M3b18 similarly preloads both continuing names for each dynamic index and bank.
M3b14 begins native flags131072, ORs4 for loop=true:0054–005a. Existing E038
cache/provider rules apply only because these original calls reach that helper:

- Index0 suppresses map lookup;1 uses unsuffixed map key;2 uses suffix2; larger
  indexes use original decimal suffix. Standard candidate order is WAV→MP3→OGG.
- Map stage then admitted skin then embedded/default, with original flags and
  process/map/skin gates. First nonnull bytes and backend load success are distinct.
- Map/skin0 remains0; embedded load0 becomes−1. Cached−1 can advance fallback.
- Loop flag is not an additional cache-key component. An existing shared cache
  entry can terminate preparation without a new loop-enabled native load. Do not
  assume colliding custom/standard names have independently prepared handles.
- Short map bytes reach native SampleCreate, other bytes/file loaders retain
  E038 arguments. No such native operation is executed or reproduced here.

M3b19 tail resets F2360=F2362=0 and F2363=F2364=0, then restores saved F2361
after miscellaneous preparation. M28f5 separately sets F2361=0 and conditionally
refreshes from map metadata. Those are shared mutable preparation operands.
Neither operation clears F2354/F2355. M3b08's initial zero table cells differ from
completed missing-resource−1. Original six continuing ByteArrays are present in
`osu!gameplay.dll` and absent under those keys in `osu!ui.dll`. Only metadata,
sizes/hashes are recorded; no audio assets are supplied to Work2.

## D. Shared channel replacement: M3b20

Signature `00030111ac3c11ac3c02`: static Void(SampleSet base,SampleSet addition,
Boolean force), on T07b9. All state below is read/written globally:

1. Base0 inherits current F2361:0000–0008. Addition0 inherits resolved base:000a–000e.
2. Compute two independent Booleans: `change0=(F2360!=F2362 || base!=F2363)`;
   `change1=(fresh F2360!=fresh F2362 || addition!=F2364)`. Keep fresh reads.
3. Proceed if force, or if either change and application F2c6d is numeric2 or1.
   Otherwise return at005f. This return is from M3b20 only; caller M3b33 continues.
4. Write F2362=**fresh F2360** at0065 before replacing either channel.
5. If change0: if F2354!=0 and **fresh ChannelIsActive(F2354)==1**, invoke
   **M3151/BASS_ChannelStop** on another F2354 read at0086. Discard Boolean result.
   Store F2363=base:008d; resolve M3b36(Normal1,F2363,F2360,2):009e; invoke
   **M3197/BASS_SampleGetChannel(sample,false)**:00a4; store returned Int32
   in F2354:00a9. No sample−1/0 validity branch or per-slider owner test exists.
6. Independently if change1, do the analogous old F2355 check/Stop:00ca, store
   addition bank:00d1, resolve Whistle2/mode2:00e2, GetChannel(false):00e8,
   store F2355:00ed. Unchanged rows preserve their channel field.
7. Compute current gain, then SetAttribute in order **channel0 attr11=1**,
   **channel0 attr2=g**, **channel1 attr2=g**, **channel1 attr11=1**.
   These writes happen after the proceed gate even when force alone admitted it
   and neither row was replaced. Return values are discarded.

Native active values other than1 skip Stop but do not prevent replacement. A
nonzero old paused/stopped/invalid handle is not assigned a repaired native state.
GetChannel return0/−1/high-bit values remain opaque and are stored. Unlike the
discrete M3b31 player path, this helper does not convert missing samples into a
managed no-player result. Invalid native calls can be reached conditionally.

There is no local try/catch or rollback. F2362 and a bank may already be updated,
and an old channel may already have been stopped when a later helper throws.
Subsequent comparisons use the surviving fields; do not assume transactional
retry or restore the old channel. Native false return is distinct from a thrown
managed exception. Creation/replacement is independent of the later mask-based
decision whether to start either row.

## E. Start and pan submission: M3b33 and callbacks

M3b33 signature `00050111a61c11ac3c11ac3c0215118801010c` is static
Void(soundMask,base,addition,Boolean layered,Nullable<Single> pan). It receives
**no slider identity**. It allocates T07c3, stores the nullable pan at0009, then
calls M3b20(base,addition,false):0011 before testing rows.

| Submission | Exact original eligibility and capture |
|---|---|
|Q0 → M3ba1|`layered || !M38e2(mask,Whistle2)` and native ChannelIsActive(F2354)!=1;0016–0059|
|Q1 → M3ba2|`M38e2(mask,Whistle2)` and native ChannelIsActive(F2355)!=1;005a–009a|
|QPan → M3b9c|Captured Nullable has value;009b–00bf, regardless of which row is selected or active|

M38e2 is `(mask&bit)>0` signed; the bit here is2. Row0 does not require explicit
Normal1. Mask Finish4/Clap8 without Whistle still admits row0. If a preceding row0
is active and a new mask becomes Whistle-only, this method neither pauses nor
stops that old row0 just because it is no longer selected. Channel replacement,
native state and later cleanup are separate influences.

All three submissions use **F234b.M2fee(Action,2)**. Row starts use cached delegates
F239f/F23a0 on singleton T07c4. Pan uses the new closure's F239b. No pending-start
Boolean prevents duplicate requests before the worker changes native state.
Repeated native state!=1 calls can enqueue repeated starts; state1 calls suppress
those starts but can enqueue pan every time, even with no discrete deadline.

M3b21's sole direct F234b assignment constructs T0670/M30b1(60000,1,1):00bd/00c2.
E037's dispatch proof remains: inherited M2fee creates a work item; resolved
submission M30bf→M33f2 queues rather than invoking Action inline; worker M30c6/
M36b3→M2ffd invokes the Action. Work-item exception handling is separate from
caller errors and contains no state rollback. Numeric policy2 is not milliseconds.
Submission order Q0→Q1→QPan is confirmed; actual callback/native interleaving with
the caller and other submissions is not measured or promised FIFO.

### Execution-time operands

**M3ba1** reads F2354 as the attribute target **before** calling M3b41 for gain,
then SetAttribute(handle,2,M3b41()/100f):0011. It separately rereads F2354:0017
and calls **BASS_ChannelPlay(handle,true)**:001d. M3ba2 does the same for F2355.
No handle, parent, volume, clock, mode or pause-generation value was captured in
these cached delegates. If replacement interleaves, attribute and Play can even
address different field reads. There is no fresh native-state test inside the
start callbacks; Boolean native results are discarded.

**M3b9c** reads current F2354 then captured nullable.Value and sets attr3:0011;
reads current F2355 then nullable.Value again and sets attr3:0028. Pan's source
value is captured at submission, target handles are not. Direct writer inventory
finds F239b assigned only by M3b33:0009; external mutation is outside the proof.

M1467 reads actual stored parent F0bd2.F2cf6.X, computes
`(X/512f-.5f)*.800000011920929f`. M146b multiplies it again by
`.4000000059604645f`. Thus finite X0/256/512 gives approximately−.16/0/.16 for
continuing pan, versus−.4/0/.4 discrete parent pan. No clamp is present in this
continuing chain. Null presentation can throw. R12's last-rendered-X bridge is an
engineering substitute, not proven equal to original presentation state.

No frequency/rate-matching argument or attr1 write occurs in these reached loop
start/replacement/pan methods. Attr11=1 and numeric attrs2/3 are literal operands;
gain/pan aliases follow managed dataflow, not a complete middleware specification.
Do not reuse circle factor matching or infer native pitch/channel semantics.

## F. Gain and refresh boundaries

Let E=F236c.M3df2(), U=F236e.M3df2(). E038 establishes the effect/universal setting
keys, not their current values. Original **M3b40** multiplies the two Int32 results
unchecked before Single conversion and division100f. Null settings can throw.

**M3b42** returns100 if current song-map is null. Otherwise obtain M3b68's current
point; if present return **Max(8,point.F0665)**; if absent return **map.F1000**
directly, **without a floor on that fallback**. These are fresh song-map reads.
No parent sample volume or tick/edge timing query participates.

Preserve these different arithmetic orders (`f` marks original Single operands):

```text
G = r4(unchecked(E*U)) /100f                 # M3b40
V = M3b42()                                # current global point/fallback
M3b41 = (r4(V) * fresh G) /100f
start_callback_attribute2 = fresh M3b41 /100f
replacement_or_M3b11_attribute2 = ((fresh G /100f) * r4(fresh V)) /100f
```

Ideal real arithmetic is V*E*U/1000000, but these expressions must not be collapsed
when reproducing original rounding/overflow/read order. V0/7/8 current point
selects8; V9 selects9. With E=U=100, start gain is approximately.08 or.09. There
is **no Normal .8 or Whistle .85 attenuation**. Null-current-point fallback0 can
give zero gain. E80/U50/V25 gives.1 under finite stated operands.

M3b20 writes gain only if its proceed gate passes. M3b33 on unchanged selectors
with both native channels state1 does not queue starts, and only pan is written.
Consequently a timing-volume-only change need not update an already active pair
through those calls. This is not a universal frozen-volume claim: starts, selector
replacement and **M3b11** can write gain. M3b21 subscribes M3b11 to relevant volume
setting events; M3b11 synchronously recomputes and writes attr2 on F2354 then F2355
(plus separate audio state). It is not itself an M2fee queue submission.

## G. Pause, cleanup, reset and resource lifetime

M146c calls M3b53. M3b53 caches Action(M3ba8) and submits it through F234b with
policy2. It performs **no synchronous ChannelPause**. At execution M3ba8 calls
**M313d/BASS_ChannelPause(F2354)** then independently
**BASS_ChannelPause(F2355)**, discarding results. No activity, validity, ownership
or generation check; no handle-field clear; no SampleFree. A pending cleanup may
therefore target channels assigned after it was submitted.

| Producer | Confirmed order / boundary |
|---|---|
|M1449 tracking=false in eligible range/state1|virtual cleanup:0217 before audio latest-index store:021e|
|M1449 outside range/state!=1|writes audio index−1:0226; this branch does not itself call cleanup|
|M1431 reset|base reset, virtual cleanup:0007, head reset, audio index0:0019; no global channel/index reset|
|M1465 parent aggregate|evaluated flag, virtual cleanup:0008, ratio/head contribution, q>0 sound hook:004d, then grade/handoff|
|M0653 manager tail|base M28e3 candidate updates first; if !F136e and M3b09 true, queue M3b53|
|M2262 reached pause-on branch|after its gates, toggles F1373 true; enumerates current candidates and calls virtual cleanup:01da; later optional song toggle:020b|

**M3b09** queries channel2355 first, returns true immediately if native state1;
otherwise queries2354 and returns its equality to1. It does not inspect a parent.
**M2275** sets F136e after Find(M22c5) in the prior candidate list: selected ref
nonnull and type includes Slider2 via M3e4e at004e. F136a forces null. M22c5 uses
start!=end, start<=fresh time and end>=another time OR not evaluated. This current-
object predicate is external arbitration, **not channel ownership**. Other direct
F136e writers (initialization and a different manager) remain inventoried.

E039 S47's phrase “queues stop” refers to that cleanup producer. The precise
native boundary is **Pause**, as E039 section G already established. Only the
replacement branch above explicitly calls ChannelStop. Earlier records are not
edited. Pausing, stopping, freeing a sample and clearing a field are distinct.

M2262's resume arm and E010's song toggle/play paths do not directly Play this
pair. M3b56/M3b57 queue actions over the separate scheduled-player collection,
not F2354/F2355. Loop reentry is determined by a later reached M146b and then-
current native activity. Upstream pause/readiness can prevent the whole slider
update. A queued loop action has no local F1373 veto; no cancel-on-pause promise
follows. Full focus/countdown/alternate resume routing remains outside this scope.

M3b65 resets timing cursors only; M3b44's rate/reset route invokes it; M3b47's
song-loading route does too. M3b45 may reuse a receiver while replacing its F24ad
map reference, or construct a new receiver and reset timing through M3b44.
M3b50 nulls/disposes the song receiver without assigning loop channel fields.
These do not collectively prove a full native loop restart contract.

Resource cleanup **M3b1c** enumerates dictionary values, calls
**M3196/BASS_SampleFree**, then clears that dictionary. M28f6→M3b1d frees the
map/custom cache; M3b1b frees skin, map/custom and embedded caches. M3b5e's reached
device-change arm can dispose the song receiver, free caches, then call
**BASS_Free** before resource preparation/receiver recreation. None writes zero
to F2354/F2355. Native effects on outstanding handles/queued callbacks are opaque.
The full device configuration/shutdown graph is deliberately not reconstructed.

## H. Shared arbitration and interaction with discrete audio

The direct chain retains global channel IDs and selected bank/index comparisons,
not a producer owner. Two sliders can submit different pan closures against the
same pair; a second bank request can Stop/replace a pair used by an earlier
producer. A cleanup submitted by A can Pause channels later acquired by B.
No managed owner check prevents it. Submission order and field mutations can be
specified; actual native overlap/cancellation depends on scheduler and native
states, not an invented per-slider lease or last-writer audible guarantee.

Normal-returning reached ordering retains E015–E023/E039:

1. Positive embedded-head M1884→own inherited circle hook, before parent raw64.
2. Separate tracking evaluation M146f can enter/leave before candidate audio.
3. M1449 tracking=true calls M146b **before** latest>old audio-index test; an
   exception here can prevent the store and the following discrete request.
4. New latest: store F0bf4 first, then M1469; its separate fresh T<=end test calls
   M146b **again** before tick/edge selection. Exception here can leave the audio
   index advanced with no discrete submission. No rollback is inserted.
5. Tracking=false cleanup occurs before latest-index consumption. Continuing
   reentry can start with no new tick/edge deadline.
6. Parent aggregation queues cleanup first. For q>0, M1466→M1469(false,edge)
   can call M146b again at a fresh exactly-end time. That may submit starts/pan
   after cleanup, then discrete edge layers. After-end skips this additional
   continuing call, while the edge path remains reachable.
7. Later ordinary manager tail can queue another cleanup through F136e/M3b09.

QPause→QStart submission does not by itself prove Pause→Play execution. If native
state is still1 at the later start eligibility check, the new QStart is suppressed
even though a pause is pending. If native!=1 it can enqueue a fresh start. Callback
replacement between gain/pan/pause field reads creates additional conditional
outcomes. F0bf4 and gameplay success/failure counters are not ownership tokens.

## I. Finite conditional controls

JSON `reference_case_common` states exact shared initial fields, clocks, selected
maps/timing, masks/settings/resources, channel IDs and helper/native assumptions.
Its symbolic table samples11/22 denote row0/row1 handles respectively at reached
cells; other cells are irrelevant unless a control overrides them. Resource/native
alternatives explicitly remain conditional rather than fabricated loader results.
In the common slider setup, absent file EdgeSounds/simple mode denotes the fresh
constructor's empty F0bf9 list, not a null Count receiver; the aggregate controls
use head raw1024, zero component counters and three pending entries (q=1/4).
The X operand in continuing-pan controls denotes stored F0bd2.F2cf6.X, with a
nonnull presentation object, rather than a cursor or newly evaluated target.
Each of **L01–L56** records overridden fields, conditional native results, original
path, ordered managed mutations, actions, implied native calls, offsets and limits.
They are all **RECONSTRUCTED STATIC PREDICTIONS**, not a surrogate audio execution.

| Controls | Principal discrimination |
|---|---|
|L01–L06|Before-due continuation, due double helper, repeat submission, tracking-false cleanup, invalid0 and reentry|
|L07–L13|Whistle/layering/Finish/Clap masks, no implicit row-off, format13/14 and base/addition inheritance|
|L14–L19|Global index/bank changes, native state1 versus other values, old0, unchanged/forced/application gates|
|L20–L25|Mode2 dynamic key, miss/−1/0, table planes/banks, loop-enabled preparation and cache flag collision|
|L26–L32|Gain floor and null fallbacks, effect/global arithmetic, volume-only change, captured pan and absent nullable|
|L33–L40|Callback field replacement, pause/start order, exact-end/after-end aggregate, two shared producers|
|L41–L44|Manager current-object predicate, native-active short circuit, fresh type defaults versus slider reset|
|L45–L51|Strict rescan/inclusive forward equality, unsorted prefix, sort/ties, null/catch and fallback index|
|L52–L56|Helper exceptions, reached pause, map/device/cache teardown, settings gain refresh and partial mutations|

Particularly discriminating finite states: list[0 U,2505 inherited] at T2500
selects point0 on a full strict rescan but point1 on initialized forward advance;
mask2/layeredfalse starts only row1 while pan targets both; a cache0 continues to
GetChannel(0,false), unlike a discrete missing-player shortcut; an active pair with
only timing volume changed can receive pan without fresh gain; QPause captured
under101/202 can execute on301/402. All native operands in those examples are
conditional inputs, not measurements. Constant fractional gains are approximate
real descriptions where original Single rounding remains material.

## J. Bounded Work2 handoff and implementation readiness

**RECONSTRUCTED decision:** a narrowly bounded modern ordinary continuous subsystem
is technically defensible. The model is more than knowing two filenames: global
timing cursors/sample state, replacement comparisons, shared targets, event order,
native-state gates and callback captures are now specified. A complete native
lifecycle/equivalence claim is not justified.

Minimum contract:

| Input/state | Required operation/output |
|---|---|
|Song-map timing records, Double time/base flag/set/index/volume, format, map fallback volume/index|Separate global timing cursor advance/rescan and global bank/index refresh; never substitute M1be6|
|Gameplay F2358-equivalent and numeric audio/application states; explicit reset/rescan history|Original unchecked+5 and strict/full versus inclusive/forward predicates, supplied order and sort boundary|
|Parent format/raw mask/base/addition and stored presentation X; layering setting|M146b operands, pan*.4, independently selected continuing rows|
|Prepared two-row3-bank3-plane loop table and dynamic map cache; provider/settings/independent assets|E038 loop-enabled preparation and mode2 lookup; explicit missing/0/native results|
|One shared channel pair, last index/base/addition and backend active results|Replace with conditional Stop/GetChannel(false); no per-slider ownership invented|
|Persistent backend + action queue|Record SetAttribute numeric operands, native-state checks, Play(true), Pause and Stop separately; callback handle/gain rereads and captured pan|
|R12 event order/progression|Keep head/tick/edge one-shot requests and private calibration separate; continuing calls do not require a new discrete event|

A prospective engineering backend needs persistent resource/channel operations,
not just R11/R12 one-shot `SamplePlaybackRequest`. Recording implementations can
inject active states, sample/channel handles, exceptions and callback execution
order without decoding or playing audio. Tests can assert exact submissions,
mutations, pair replacement, gain/pan operands, callback rereads and discrete
ordering. The existing WPF song MediaPlayer/Stopwatch remains an engineering
timeline; this research proposes no replacement and implements no backend.

Smallest initial R13 scope: fresh same-map ordinary format>=14, finite supported
timing/preparation, one eligible continuous producer at a time, explicit global
settings/state, independently supplied loop assets, injected/persistent pair,
tracking loss/reentry and documented engineering pause/restart/disposal policy.
Keep overlaps, arbitrary seeking/editor/retained-manager states, device replacement,
native channel stealing and ambiguous cache collisions explicitly unsupported
until their external operands are supplied or measured. Even that single-producer
subset must model the pair globally, not claim original per-slider ownership.

Required independent resources are six bank×family names
`normal|soft|drum-sliderslide` and `normal|soft|drum-sliderwhistle`, plus supported
indexed variants; original cache/table rules still apply. Existing twelve circle
and three tick files do not supply them. Original assets are present in the pinned
archive but are not exported, licensed or manufactured by this expedition.

Engineering generation cancellation, handle validation, error containment and
safe disposal can be appropriate for a future backend, but differ from the original
callbacks' unguarded global rereads and discarded native results. Label those
decisions explicitly rather than silently describing them as original lifecycle.
No assertion of original audio continuity, pitch, driver latency, voice overlap,
presentation-clock equality or audibility follows from static metadata or mocks.

# 3. Pseudocode

**RECONSTRUCTED from CONFIRMED STATIC** instructions. Documentation only; original
helpers, exceptions, separate reads and integer/Single operations are retained.
`fresh` denotes a new original field/helper read; Q names are method aliases.

```text
global_current_point():                          # M3b68/M3b6a/M47cb
    index = fresh F238f                          # M3b68 reads before helper
    list = song_receiver?.map?.timing_list
    if list == null: return null
    return list[index] if 0<=index<list.Count else null

advance_global_timing(rescan):                   # M3b66
    try:
        list = current_song_timing_list()
        query = unchecked(fresh gameplayTime + 5)
        if list == null or list.Count<=0: resetCursorsAndSentinel(); return
        if rescan: list.Sort(original CompareTo)
        if rescan or F238e<0 or fresh audioState!=1:
            oldBase,oldAll = F238e,F238f
            F238e = Max(0,FindLastIndex(baseFlag && time<Double(query)))
            F238f = Max(0,FindLastIndex(time<Double(query)))
            if oldBase!=fresh F238e: invoke_base_delegate(current_base_point())
            if oldAll!=fresh F238f: invoke_all_delegate(current_point()); sentinel=-999
        else:
            for i from unchecked(F238f+1) to captured list.Count-1:
                p = list[i]
                if p.time>Double(query): return
                F238f=i; invoke_all_delegate(p)
                if p.baseFlag: F238e=i; invoke_base_delegate(p)
    catch original Object:
        reset F238e/F238f=-1; sentinel=-999       # no other rollback

clock_followup():                               # reached M3b61
    advance_global_timing(false)
    p = current_point()
    if p!=null && selected_map()!=null && fresh selected_map().format>3:
        refresh_global_samples(p.baseSet,false)  # M3b1f, not per-object lookup

refresh_global_samples(base,force):
    if !force:
        if base==F2361 && current_sample_index()==F2360: return
        if application!=2 && fresh application!=1: return
    if base==0: base=1
    F2361=base; F2360=current_sample_index()       # point.index or F236b
    retain original shared cache/other-channel refresh
    if force || F2363==0: prepare_pair(base,base,force)

slider_continue():                              # M146b
    base,addition=0,0
    if parent.map.format>=14: base,addition=parent.base,parent.addition
    submit_loops(parent.rawMask,base,addition,skin.LayeredHitSounds,
                 Nullable(parent.virtualPan()*.4000000059604645f))

prepare_pair(base,addition,force):               # M3b20
    if base==0: base=fresh globalBase
    if addition==0: addition=base
    change0 = fresh currentIndex!=fresh lastIndex || base!=fresh lastBase
    change1 = fresh currentIndex!=fresh lastIndex || addition!=fresh lastAddition
    if !force && (!(change0||change1) || (application!=2 && fresh application!=1)):
        return
    lastIndex=fresh currentIndex
    if change0:
        if channel0!=0 && IsActive(fresh channel0)==1: Stop(fresh channel0)
        lastBase=base
        channel0=GetChannel(resolveMode2(Normal1,fresh lastBase,fresh currentIndex),false)
    if change1:
        if channel1!=0 && IsActive(fresh channel1)==1: Stop(fresh channel1)
        lastAddition=addition
        channel1=GetChannel(resolveMode2(Whistle2,fresh lastAddition,fresh currentIndex),false)
    g = ((M3b40()/100f)*r4(M3b42()))/100f
    SetAttribute(fresh channel0,11,1f)
    SetAttribute(fresh channel0,2,g)
    SetAttribute(fresh channel1,2,g)
    SetAttribute(fresh channel1,11,1f)

submit_loops(mask,base,addition,layered,nullablePan): # M3b33
    closure.pan=nullablePan
    prepare_pair(base,addition,false)
    if (layered || not((mask&Whistle2)>0)) && IsActive(fresh channel0)!=1:
        scheduler.M2fee(cached Q0,2)
    if (mask&Whistle2)>0 && IsActive(fresh channel1)!=1:
        scheduler.M2fee(cached Q1,2)
    if closure.pan.HasValue: scheduler.M2fee(closure.QPan,2)

Q0/Q1 execution:
    target = fresh respective global channel     # before gain helper
    value = M3b41()/100f                         # fresh global timing/settings
    SetAttribute(target,2,value)
    Play(fresh respective global channel,true)   # separate reread
QPan execution:
    SetAttribute(fresh channel0,3,capturedNullable.Value)
    SetAttribute(fresh channel1,3,capturedNullable.Value)

slider_cleanup(): scheduler.M2fee(cached QPause,2) # M146c/M3b53
QPause execution: Pause(fresh channel0); Pause(fresh channel1)

original discrete integration:
    M1449 eligible/tracking: slider_continue() before audio-index comparison
    if new latest: store audioIndex; M1469 -> if fresh time<=end: slider_continue()
    then unchanged dedicated tick or standard edge request
    trackingfalse: queue cleanup, then consume latest index
    aggregate: evaluated/cleanup; existing ratio without drain
    if ratio>0: M1466 -> M1469(false,edge) -> possible continuation then discrete edge
    later manager !F136e && any native state1: queue cleanup

UNVERIFIED:
    actual callback execution/native state changes, handle validity and overlap;
    full external device/focus/seek/shutdown graph, pitch, physical output and audibility.
```

Stop after E040. No R13, E041, spinner sound implementation, framework migration
or deferred scoring-discrepancy investigation is begun.
