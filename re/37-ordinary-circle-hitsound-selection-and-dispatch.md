# 1. Result

Expedition037 — Ordinary Circle HitSound Selection & Playback Dispatch.
Work1, 2026-11-04 Asia/Bangkok. Starting canonical main:
`42a3c92fe19d305ca2071973d1d1f559cfa7cccc` (QA08). Original-binary research only.

**CONFIRMED STATIC:** a positive ordinary circle result reaches inherited
M24bb, obtains a timing/sample record at **stored start+5**, selects object versus
timing sample operands, then either requests the object's custom handle or
expands a sound mask into ordered sample requests. M3b31 creates an effects-player
object and queues an Action. On reaching that Action, the effects path obtains
a BASS sample channel, sets its numeric attributes and calls
**bass.dll!BASS_ChannelPlay**. This uses a sample/channel pair separate from E10's
song playback handle. A positive judgement is not a guarantee of audible output.

Recovered operations are implementation-ready at the **prepared resource-handle
interface**. Complete asset-name/skin resolution remains bounded by encrypted
string keys and unresolved configuration/resource inputs. No asset filenames,
native error conventions, audible latency or physical output are invented.

Created this report, `evidence/37/circle-hitsound.json`,
`tools/circle_hitsound_evidence.py`, `tools/check_circle_hitsound.py`, and appended
the E037 journal entry. No reconstruction/QA/fixture/historical report changes.
No authorised exception. Final publication commit and verified remote status
are supplied in the handoff.

## Preparation, identity and procedure

Read the canonical standing contract/RTK instruction and project records;
consulted E04/E08–12/E26/E33, research/reconstruction journals, actual current
Beatmap parser, BeatmapAudio and CircleGame README. The user AGENTS reference
points to RTK.md; no physical repository AGENTS.md is present. Preserve E05's
ordinary/Taiko correction and E12's result/callback ordering. Current MediaPlayer /
Stopwatch remains an engineering system; E037 neither replaces it nor adds sounds.

Verified actual bytes **before** E037 instruction inspection:

| Input | SHA256 |
|---|---|
|osubackup.zip|7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
|ZIP member osu!.exe|6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|

Self-described Stable40/file-version1.3.3.8; exact official dated identity,
authenticity/modification status remain UNRESOLVED. Windows host/Python3.12,
dnfile0.18.0/pefile2024.8.26/dncil1.0.2. Only PE/metadata/CIL bytes were read.
No original EXE/CLR/native/audio execution, listening, device test, game/window,
OS input, OBS, debugger or E20 experiment. All17 E20 cases remain NOT RUN.
The628-case independent QA checkpoint is preserved, not newly executed here.

Competing explanations discriminated by instruction/stack/field/slot evidence:
positive result versus every judgement trigger; construction-time versus
callback-time timing lookup; object override versus timing metadata; one sample
versus layered requests; direct native playback versus queued callback;
song handle versus separate sample handle; missing override versus standard
fallback. Private normalized inspection supplies interpretation; deterministic
extraction and independent literal-site checks support it without a sound simulator.

Report offsets are **hexadecimal, CIL-relative, headers excluded**; JSON offsets
are decimal. M/F/T abbreviate EXE MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx.
CONFIRMED means original-static evidence; semantic aliases and finite controls
are RECONSTRUCTED. Original bytes/full disassembly/assets remain private.

Reproduction (pinned libraries may be installed privately via PYTHONPATH):

```text
python re/tools/circle_hitsound_evidence.py /path/osubackup.zip /tmp/hitsound-a.json
python re/tools/circle_hitsound_evidence.py /path/osubackup.zip /tmp/hitsound-b.json
python re/tools/check_circle_hitsound.py /tmp/hitsound-a.json
```

Extractor pins both hashes before decoding and inventories selected field/call
sites over19659 bodies/1739 zero-RVA methods. Metadata records identities,
signatures, body hashes, inheritance, slot candidates, native imports and original
enum literals. Numeric constants/branches/local arguments and arithmetic preserve
operation order. Native/abstract zero-RVA declarations are not executed as CIL.
Actual final checker/repetition/preservation outcomes are appended in the journal.

Actual local outcomes: two final extractions byte-identical; E037 checker PASS
**82 methods /19659 decoded bodies /83 historical body hashes**, including literal
trigger/metadata/inheritance/queue/native relationships. The eight retained
E04/E08/E10/E11/E12/E26/E33/E36 checkers PASS with their correct evidence files;
new tools compile in memory. Scope, original journal prefix and diff whitespace
checks pass before publication. These are original-static consistency checks,
not listening, gameplay or a rerun of the628 independent QA cases.

Representative original identities (complete signatures/hashes are in JSON):

| Method | Owner / signature | RVA | Body SHA256 |
|---|---|---|---|
|M24bb|T04c3 /200001|e9268|562e53e79e1e025eb14ef07c0bb7065d481eae11ba91e55f2f4c85b7a50bc734|
|M24ba|T04c3 /20040112866811a61c11ac3c11ac3c|e91d8|6e8e094c3714140c7b4b90f5b5b956e2f143c17df9bf64e91dd2a91fe0aa9998|
|M3b31|T07b9 /0005128aa408080c020c|1819e0|7374324b185ad961ab23c04355d44c498b4350fd16a9b4627e36685cab885193|
|M3b32|T07b9 /00030111a4600c02|181ac8|eea9733686f91b80e2ad9bacc553668cdd635bf669f02e0312497b32694abd22|
|M140b|T02a9 /2002010202|799b0|01c5493d223322fd2e1864a3e0911c476481b6c7ef3b63ac1a2dd53b02d6c9e2|
|M3202|T067f /200001|142660|94ff1ab56c824f3e7aa93e4f134209a41f60bcee033c476be3c907cd2178247b|

# 2. What they found

## A. Trigger and exact ordinary receiver

Exact ordinary circle ancestry is T094c→T035a→T04c3→T0828.
Virtual M24bb resolves to **T04c3.M24bb**, instance void(), signature200001;
none of T094c/T035a/T0828 replaces that slot. Likewise panning slot M24bd
resolves to T04c3. Do not substitute the slider parent's M1466 or spinner M3202.

E12 M1884 preserves this order:

| CIL | Operation |
|---|---|
|0076/007c|Read stored F152a; signed <=0 skips sound hook |
|007f|Call virtual M24bb only for positive stored result |
|0086/008c/008e|Reread F152a, form >0, call virtual M24b4 |
|0094/0099|Reread F152a and return |

Raw1024/512/256 all take the same sound hook; their quality is not an argument
to it. Raw−131072 does not take it. E037 proves no audible-miss request from this
hook and makes no global claim about every other sound producer. E12's later
Perfect conversion of a manager-local result can happen after the object's
positive hook; it cannot retroactively suppress an already submitted request.

The callback occurs inside evaluation, before manager group decoration/common
result delegate and E33 score accounting. Queue submission returns before the
completion callback on normal serial caller execution, but a worker can run
concurrently; no heard-sound-before/after-visual assertion follows.

M24bb loads object.manager F152f → manager.map F17ee, and calls map **M1be6**
at0014 with `Double(unchecked(F2523+5))` (add0012 precedes conversion0013).
Null record returns001d. Otherwise it calls M24ba at0032 with that record,
object.F2526, F1537 and F1538. There is no local pause/result-quality/volume gate.
Null manager/map can throw at their original dereferences, and reached helpers
can interrupt before M24b4 or result return; there is no rollback in this chain.

### E04 clarification, preserved historical text

E04's table says “Factory queries map.M1be6(start+5)” for M426e→M20dc and calls
that the ordinary route. E05 already corrected that chain to **Taiko**. E037
independently confirms M426e's lookup at000f. The actual ordinary factory
**M48df→M45eb/M45ec** contains no timing lookup: it forwards metadata, then the
successful circle's M24bb performs the runtime start+5 lookup. The historical
passage remains unchanged; this current clarification prevents importing that
factory-time extra Boolean into ordinary sample selection.

## B. Parsed and constructed metadata

### Ordinary circle row

M28f7 reads sound column5 (zero-based row[4]) at149b–14ad. Circle construction
M45eb checks its supplied mask against2/4/8; M45ec starts F2526=0 at002c and ORs
2/4/8 at0066/0078/008a. Thus this constructor retains those three bits; it does
not copy all raw sound bits. A raw0 or raw1 can both become stored0.

Original enum T0987 has names **None0, Normal1, Whistle2, Finish4, Clap8**.
Those names come from metadata, not remembered file conventions.

Optional circle row[5] is colon-split at15af. The first two components use
Convert.ToInt32 (15be/15cd); missing trailing third/fourth/fifth components have
their own defaults. Preserve the parser's length/index branches and exception
behavior: a malformed short payload is not a guessed all-zero override.

| Parsed payload / parser sites | Ordinary factory/field | Later role |
|---|---|---|
|component0→local15 at15c3|M48df:0015→F1537|Base sample-set override;0 inherits timing |
|component1→local16 at15d2|M48df:001d→F1538|Addition sample-set override;0 ultimately uses selected base |
|optional component2→local17 at15eb, otherwise0|M48df:0025→F1539|Sample-index selector;0 inherits timing |
|optional component3 Int32, clamp0..100 via M2d7c:1607→local19|M48df:002d→F1536|Volume operand;0 inherits timing, not an explicit mute |
|optional component4→local18 at1624, otherwise empty string|M48df:0035→M24b9|Custom sample name/handle preparation |

M48df signature in JSON retains all ten arguments. It constructs the ordinary
circle first (000d), then assigns these fields and invokes the filename setter.
Shared constructor M249b initializes **F1534=−1** at0018. Circle constructor
initializes F1537/F1538=0 before the factory overwrites them. Raw type/NewCombo /
colour skip are independent arguments; E036 grouping does not select sounds.

### Timing/sample records

Map metadata refresh M1bc1 handles TimingPoints16 through **M28ff at0366**.
M28ff splits comma fields and passes shifted time, beat length, meter, sample
set, index, volume and further flags into **T019a.M0b73**. For the normal long-row
path, row[3] supplies sample set (008c), optional row[4] index (00a1, default0),
optional row[5] volume (00b6, otherwise method arg2); row[0] time receives the
map shift before construction. This does not replace text parsing/culture with
a permissive parser; M28ff catches errors and returns null on its own path.

M0b73 writes time F0662:000f, sample set F0663:002c, index F0661:0034, and
volume F0665:0044. A supplied sample set0 becomes literal2 at002b; volume is
clamped0..100 through M2d7c:003f. Metadata T0b0f identifies SampleSet
None0/Normal1/Soft2/Drum3/All−1. Therefore this constructor's zero-set default
is specifically **Soft2**; do not generalize it to every old/cache/editor path.
The separate two-column timing path M28ff:00e4–011e uses M3b3c and volume100;
M3b3c can use another selected record's F0661 when its guarded map/version state
permits, otherwise F236b. These are retained older/global operands, not a guessed
universal timing default.

**M1be6 is nonvirtual**, despite callvirt syntax. Null/empty F100f returns null.
Otherwise it scans the whole supplied list in order (001d–0045), retaining the
last encountered record whose F0662 is <=query; `bgt.un`0031 skips greater or
unordered records but does not break the scan. If no record qualified, it returns
list[0] (004a–0056). It does not filter inherited/uninherited timing flags here,
nor choose greatest time independently of list order. Runtime callback reads the
then-current list; a construction-time snapshot is not established.

## C. Selected sample information and resource expansion

M24ba is T04c3 instance void(record,soundMask,baseSet,additionSet), signature
`20040112866811a61c11ac3c11ac3c`. Define record fields from above and object fields
at consumption time. In the original order:

```text
base = argument3 !=0 ? argument3 : timing.F0663       //0000–000c
index = object.F1539 !=0 ? object.F1539 : timing.F0661 //000d–0023
volume = object.F1536 !=0 ? object.F1536 : timing.F0665 //0024–003a
addition = base !=0 ? argument4 : base                //003f–0047
info = M44a2(mask,base,index,volume,addition)
M28a1(info)                                          //004e, before either dispatch arm
```

Info is T0918, constructed by M44a1 with fields F2b36=mask,F2b37=base,
F2b38=addition,F2b39=index,F2b3a=volume. M44a2 adjusts the mask first:

`mask |=1` if mask==0, **or** F2f16 exists, F2f16.F36b0 is true, selected map
exists and its M1bba result is not3. Otherwise retain the mask. This is the exact
layering predicate (0001–002f); the setting field's UI/configuration meaning is
not decoded here. Do not make “Normal always layers” an unconditional rule for
nonzero sound masks. A zero mask always receives Normal1.

M28a1 locks registry F17e3 and, if its first manager has F17f9, calls
M2f1a(info):0040. M2f1a invokes optional static delegate F1bbd/M18f9. This is an
observer boundary before playback selection, not the native dispatch. Subscriber
behavior/exceptions can affect progress; no side-effect-free observer is assumed.

### Custom handle versus standard selection

If object.F1534 !=−1 (M24ba:005a), call **M3b31(handle,volume,pan,true,1f)** at006f
and return. Standard sound expansion is bypassed even if the selected handle is0.
If F1534==−1, call **M3b32(info,pan,true)** at007e.

The custom-handle condition loads F1534 at0054 and the argument reloads it at005d;
do not hoist those into an immutable handle snapshot across arbitrary mutation.

M24b9 prepares a filename override only when manager.F1800 is nonnull, the supplied
string is nonempty, and F234a is false (0001–001a). It stores original string in
F1535:001e, then calls M3b15(name,specialSliderFlag,false,false):005b and stores
its returned handle F1534:0060. For ordinary circle the slider-specific substring
test does not set that extra flag. Otherwise it stores−1:0068. A failed lookup
returning−1 permits standard fallback; a native-loaded zero handle is a distinct
case, not normalized to−1 by this setter. Reassignments/editor/clone writers are
inventoried, not adopted as fresh ordinary semantics.

M3b15 performs dictionary cache lookup, selected-map byte-resource calls and
three candidate-suffix attempts where its name/extension tests require them.
Its suffix/key literals are encrypted M49b0 identifiers; E037 does not assign
them `.wav`, `.ogg` or other guessed filenames. Bytes shorter than1024 take
M3195 **BASS_SampleCreate** with literals0,44100,2,8 and prepared flags; other
bytes take M31a2→M31a5 **BASS_SampleLoad** with mem=true,offset0,length,max8 and
prepared flags. It can replace/free a previously cached non−1 handle through
M3196. These are exact requested arguments, not proof of an audible silent
fallback or a guarantee of eight simultaneous voices. File/provider failure,
native errors, resource meaning and cache lifetime remain boundary operands.

### Ordered standard requests

M3b32 uses volume `arg2 ? Max(8,info.volume) : info.volume`; this circle path
passes true, so an inherited volume0 becomes request-volume8 on the standard
arm. **The custom-handle arm does not apply this floor.** Addition bank is
info.addition when nonzero, else info.base. It tests bits and dispatches in this
order, each through M3b36(bit,set,index,0) then M3b31:

| Order | Bit/name | Set operand | Int32 request volume |
|---:|---|---|---|
|1|1 Normal|base|conv.i4(Double(volume)*.8),0035–0064 |
|2|4 Finish|addition-or-base|volume,0070–008e |
|3|2 Whistle|addition-or-base|conv.i4(Double(volume)*.85),009a–00c4 |
|4|8 Clap|addition-or-base|conv.i4(Double(volume)*.85),00d0–00fa |

This is Normal→Finish→Whistle→Clap, not numeric bit order. Each request has
rate-match=true and initial factor1f. Helpers/observers may throw or worker
execution may interleave; no atomic multi-sample audible onset is guaranteed.

### M3b36's numeric resource resolver

For fourth argument0 (this path), map sound1/2/4/8 to row0/1/2/3. Map SampleSet
Normal1→bank0, None0 or Soft2→bank1, Drum3→bank2; other set values take default
bank0. Set index=`Max(suppliedIndex,0)`. Index<=2 reads **F2374[row,bank,index]**
at0074–0081. Metadata T035f labels those planes Default0/Custom1/Custom2.
F2375/F2376 are different resource families selected by fourth argument1/2, not
circle layers inferred from similarity.

For index>2, construct a key through original encrypted format identifier
2064528980 with mode prefix, lowercase SampleSet.ToString, lowercase sound enum
ToString and index (00e0–012d). In ordinary ruleset0 the prefix is empty; the
special Taiko selector is separate. Look up F234e; a found non−1 handle returns.
Otherwise return the same row/bank's **index0** table handle (01db–0200), not a
guessed nearest custom index. M3b36 does not itself load that indexed filename.

M3b08 allocates F2374 as4×3×3. **M3b19** populates it by iterating selector planes
0/1/2 and metadata sample sets1/2/3, loading four keys through M3b14 and storing
rows0/2/1/3 (005c/007b/009a/00b9). M3b14's original cache/map/skin/embedded-resource
fallbacks and configuration gates remain in evidence; complete string-key names,
skin archives and resource provenance are **UNRESOLVED**. The verified handoff can
consume these prepared handle tables/dictionary directly, without inventing names.

## D. Playback request, queue and native boundary

**M3b31**, static T07b9, signature `0005128aa408080c020c`, receives
`(Int32 sampleHandle, Int32 volume, Single pan, Boolean matchRate, Single factor)`.
Handle−1 returns null at000b, with no player/list/queue mutation after the initial
closure allocation. Other handles proceed, including0.

If matchRate=true, it reads song receiver F2367.M3d65. If rate!=100 and receiver's
M3d69 is true, it changes factor to the original Single expression
`1f + (conv.r4(freshRate)-100f)/100f` (000c–004d). Keep separate receiver/rate
reads and the opaque flag's actual outcome. There is no song-receiver null guard
on this true arm; it can throw. No sample-playback seek timestamp is passed.

At0051 M3b31 constructs exact **T02a9/M13fb(sampleHandle)**, then redundantly
calls M1409(handle):0058. It sets volume through M13fd(volume/100f):0066,
pan through M13ff:006d (clamp−1..1), and factor through M1403:0075. These are
nonvirtual concrete methods; exact sample player is not an unresolved virtual
audio receiver. Before it has a channel, M1404's validity gate suppresses native
attribute writes. The object is added under lock to F2351 at009a.

Then **F234b.M2fee(Action(M3bb6),2)** at00bd schedules playback. Sole direct
F234b writer is M3b21:00c2, constructing **T0670/M30b1(60000,1,1)** at00bd.
T0670 inherits M2fee from T0649; its dependency slots resolve to
M30fc (disposed/timeout checks), M30f6 (options) and M30bf (queue submission).
M30bf:0019 calls the queue's M33f2, and signals/workers can follow; it does not
synchronously invoke the sound Action at that site. Worker M30c6 and work item
M36b3 lead through delegate M0e88→M2ffd→Action.Invoke→**M3bb6**. The work item
catches delegate exceptions at M36b3:0049. Scheduler cancellation/timeouts,
thread interleaving and actual worker start/ordering are not experimentally
resolved. Argument2 is retained as numeric task policy, not milliseconds.

M3bb6 calls exact player's **M140b(true,false)** at0008. Relevant behavior:

1. F0bc5 true returns0008. M1407 rejects sampleHandle0 or−1; that arm sets
   F0bc4=true:0013 and returns without a native play call.
2. Otherwise false second argument skips forced channel reset. M1406 obtains
   channel if F0bc0 is zero: **M3197(sampleHandle,false):0010**, stores F0bc0,
   sets native attribute11=1 at0027, reads native attribute1 into F0bc3:003a,
   applies M1404 and returns channel. No assertion about the middleware meaning
   of attribute11 is needed.
3. M1404 applies the channel's gain/pan/factor attributes; M140b calls it again
   at0029, then **M313e(channel,true):002f** = bass.dll!BASS_ChannelPlay.
   Return Boolean is popped at0034; F0bc4=true at0037 is not native success proof.

The channel-validity helper M1405 uses `cgt.un(channel,0)` at0007: with Int32
storage this is a nonzero test, including high-bit handles, not a signed-positive
constraint. Sample validity M1407 separately excludes exactly0 and−1.

M1404's numeric attribute writes (native M3146/BASS_ChannelSetAttribute):

| Attribute / site | Value |
|---|---|
|2 /0099|`F0bbc * M3b40() /100f`; F0bbc is requestVolume/100f |
|3 /00ac|F0bbd; ordinary caller's initial pan=`(object.X/512f-.5f)*.800000011920929f` from M24bd |
|1 /00c6|stored native base attribute F0bc3 × F0bbf factor |

M3b40 reads integer settings via F236c.M3df2 and F236e.M3df2, multiplies them
before conv.r4, then divides by100f. Preserve this order and live setting reads;
their exact installed UI values are not assumed. M1404 has an additional
F0bbe>0 branch that recalculates pan from handler/pointer operands; ordinary
M3b31 never sets F0bbe, so its fresh default0 bypasses that branch.

For circle X0/256/512 the initial pan is−.4/0/.4 (Single operations); this is
an operand, not a measured stereo image. Spinner's fixed center yields0 through
the same inherited pan method. Attribute numbers above are exact original
arguments; volume/pan/frequency aliases follow dataflow, not an imported middleware
specification. If SampleGetChannel returns0 despite a valid sample handle,
M1404's channel-validity gate returns, but M140b can still reach ChannelPlay(0,true).
No native error is converted into a successful audible observation.

Each request constructs a distinct player object, so consecutive circles have
separate queued player references even if sample handles are shared. Channel
allocation uses native GetChannel(false); actual stealing/overlap capacity, device
selection and acoustic timing remain middleware/runtime dependencies. Max8 in
sample load/create is recorded as an argument, not a guaranteed voice count.

### Pause and song-audio availability

E11's ordinary live input rejects paused F1373 before evaluation; E12/E28's
paused update return prevents the reached timeout/update route. These are
upstream gameplay guards. M24bb/M24ba/M3b32/M140b do not themselves test that
pause flag or E10's q/active-song predicate. A request already queued before a
pause is not proven cancelled by these bodies. Wider pool cleanup/pause behavior
is UNRESOLVED; do not promise it plays through pause either.

E10's song receiver may be the native-backed T08e4 or fallback T086e. Sample
requests still use their separate sample handle/F0bc0 channel; no equality with
song.F24b1 is asserted. F2367 can influence rate matching, and a null receiver can
interrupt M3b31 on this true arm. A stopped/paused but nonnull song does not by
itself fail any explicit active-state test in this bounded sample path. Missing
sample, invalid native channel, zero gain, disabled player, scheduler/resource
failure or exceptions can prevent audible output independently of judgement.

## E. Related objects and operation boundaries

E26's spinner hook resolves to **T067f.M3202**. It queries M1be6 at **stored end
F2524**, with no+5, returns when no record, then calls the same M24ba with
F2526/F1537/F1538. It shares selection/layering/custom/queue/native machinery,
but its timing lookup and object type differ. E037 does not reinterpret E26's
positive-grade or completion conditions.

Slider parent's M24bb override M1466 and slider pan override M1467 differ;
slider head/callback/component audio and loop families have additional producers.
They are dependency boundaries, not all reduced to circle sound. No complete
slider tick/repeat/terminal policy is inferred. The first meaningful sample-play
boundary recovered here is M140b:002f, after the separate queued callback.

## F. Finite reference controls

Every row is **RECONSTRUCTED STATIC PREDICTION**, original runtime **NOT RUN**.
Unless varied: exact fresh ordinary T094c, E11 admission gates pass, unevaluated
circle, stable references, normally returning helpers/observer, timing list and
native resources as explicitly supplied, matchRate=true with nonnull song receiver
rate100, scheduler operational, F0bc5=false. Native-play predictions mean only
that the managed call is reached, not that sound is heard. Fresh state per row.

| ID | Additional prerequisites | Conditional original-static result |
|---|---|---|
|H01|E12 e0, widths50/100/150|raw1024 invokes hook before completion/handoff |
|H02|e50 under same widths|raw512 invokes identical hook/operands |
|H03|e100 under same widths|raw256 invokes identical hook/operands |
|H04|e150, reached classifier|raw−131072 skips M24bb; no miss sample from it |
|H05|Positive lower raw later converted by manager Perfect gate|Sound hook precedes that later conversion |
|H06|M1be6 null (null/empty timing list)|No M24ba request; positive result can still finish |
|H07|Timing records at S+4 and S+5, supplied order increasing|Record at S+5 qualifies inclusively |
|H08|Another record at S+6|It is skipped; no late-hit-time reselection |
|H09|No time qualifies but list nonempty|list[0] returned, not null |
|H10|Artificial unsorted timing records [time100,time90], query105|Later supplied qualifying record90 retained; no greatest-time repair |
|H11|Raw sound0 or1 via ordinary constructor; no custom handle|Stored0 becomes mask1; Normal request |
|H12|Raw2, layering predicate false, prepared volume100|Whistle only, request-volume85 |
|H13|Raw4, layering false, volume100|Finish only, request-volume100 |
|H14|Raw8, layering false, volume100|Clap only, request-volume85 |
|H15|Raw14, layering false, volume100|Finish100→Whistle85→Clap85 |
|H16|Raw14, F36b0/map predicate forces Normal|Normal80 precedes the H15 sequence |
|H17|Object base0/addition0/index0/volume0; timing set2/index1/volume40|Select set2/index1/volume40; additions use base; Normal volume32 if selected |
|H18|Object base1/addition3/index2/volume25; conflicting timing data|Object operands win; Normal20 and selected additions21 or25 according to bit |
|H19|Inherited timing volume0; standard arm|Volume floor8; Normal conv.i4(6.4)=6, Whistle/Clap conv.i4(6.8)=6 |
|H20|Custom handle valid and effective volume0|Custom request0 without standard floor/bit attenuation; no standard layers |
|H21|Index3, exact dynamic key found with non−1 handle|Use that handle |
|H22|Same dynamic key missing or value−1|Fallback same row/bank index0 table handle |
|H23|Artificial negative nonzero object index|It wins inheritance, then M3b36 Max(index,0) selects plane0 |
|H24|Override filename setter is gated off|F1534=−1; standard expansion remains available |
|H25|Custom loader returns−1|Standard expansion used on judgement |
|H26|Custom loader returns0|Custom arm queued; worker rejects invalid sample0; no standard fallback |
|H27|Any standard resolver returns−1|M3b31 returns null for that layer; subsequent layers may still be requested |
|H28|Valid handle, native channel0|Managed ChannelPlay(0,true) can be reached; success unknown |
|H29|Two successful circles with same selected sample handle|Distinct player objects/list entries/actions; no guaranteed channel/audible overlap |
|H30|Fresh circle X64 versus448, rate100|Initial pan about−.3 versus+.3; request time still callback-dependent |
|H31|Song rate150 and M3d69 true|Factor becomes original Single1+(150−100)/100=1.5; false flag retains1 |
|H32|F2367 null at rate-match read|Can throw before player enqueue; no audible request guaranteed |
|H33|Paused live input entry under E11 guards|No evaluator/hook from that blocked input |
|H34|Artificial direct positive evaluator during pause|Hook has no local pause veto; wider queued audio state unresolved |
|H35|Observer throws at M28a1|Playback arm and later completion callback need not be reached |
|H36|Worker action throws|Task catches delegate exception; scheduling call is not native-success proof |
|H37|Spinner endE versus circle startS, timing change betweenS+5 andE|Spinner uses E record; circle uses S+5 record; shared dispatch otherwise |

Payload/numeric/native-invalid controls above are artificial operand assignments
unless naturally produced by their stated parser path. No fixture was imported,
no listening/device test occurred, and none of these rows is counted as a QA pass.

## G. Work 2 bounded handoff and remaining dependency

Inputs needed beyond current reconstruction: raw circle sound flags; optional
five-component sample payload; stored shifted start/position; ordered prepared
timing records/time/set/index/volume; per-object custom resource handle or−1;
layering predicate inputs; prepared4×3×3 handle table and dynamic key→handle
dictionary; global gain operands; song rate/match flag; sample-player disabled /
channel/native results; queue/observer state. Keep file data, stored fields,
resolved info and final per-layer playback arguments separate.

Trigger: reached positive M1884 stored-result check, before completion/manager
result handoff. Operation order: query record→inherit object/timing selectors→
layer mask→observer→custom-or-ordered-standard resolution→create player→set
request properties→register→queue→validate sample/channel→attributes→native play.
No judgement-time timestamp/seek position is passed to the sample Action.

Headless tests could independently check payload storage, timing-list selection,
inheritance, layer order, integer attenuation, custom bypass/fallback, numeric
handle lookup and immutable request fields using explicit injected resource tables
and a recording sink. Such a queue/sink/asset loader would be an **ENGINEERING**
replacement for the original middleware, not proof of BASS timing/channel/output
behavior. E037 implements none of it and proposes no replacement of current
MediaPlayer/Stopwatch. Existing HP/group/scoring models remain untouched.

Smallest unresolved static dependency for complete filename-based selection:
recover the exact relevant M49b0 format/suffix/resource keys and the involved
M3b14/provider/skin fallback inputs, including F36b0 configuration and table/cache
lifetime. Without that, use explicit prepared handle operands for the recovered
bounded model; do not guess standard filenames or silently assign fallback assets.
Runtime dependencies remain worker scheduling/cancellation, callbacks, current
resource/setting identities, native load/channel/play/error returns, driver/device
latency and actual audibility. No new runtime experiment is authorised or run.

# 3. Pseudocode

**RECONSTRUCTED from CONFIRMED STATIC instructions**, independent non-executable
description. Original helpers, separate reads, numeric types and exception
boundaries are retained. Encrypted key format and runtime operands stay explicit.

```text
ordinary circle evaluation:
    classify and store F152a using E12 windows
    if Read(F152a)>0: virtual M24bb -> inherited base
        point = map.M1be6(Double(unchecked(Read(start)+5)))
        if point is null: return from hook
        info = select original object/timing operands
        adjust Normal bit using original M44a2 predicate
        original observer M28a1(info)
        if Read(customHandle)!=-1:
            reload customHandle, load effectiveVolume, then evaluate virtual pan
            M3b31(customHandle,effectiveVolume,pan,true,1f)
        else:
            load info, then evaluate virtual pan
            v = Max(8,info.volume)
            in order Normal1,Finish4,Whistle2,Clap8:
                if info.mask contains that bit:
                    set = Normal ? base : (addition!=0 ? addition : base)
                    handle = M3b36(bit,set,index,0)
                    requestVolume = original [.8,1,.85,.85] scaling/conversion
                    M3b31(handle,requestVolume,pan,true,1f)
    virtual completion M24b4(Read(F152a)>0)
    return Read(F152a)

ordinary inherited virtual pan:
    return (Read(position.X)/512f-.5f)*.800000011920929f

M3b31(handle,volume,pan,matchRate,factor):
    if handle==-1: return null
    preserve original separately read song-rate/match-flag factor adjustment
    player = new exact T02a9(handle)
    player.volume = volume/100f; player.pan = clamp(pan,-1f,1f)
    player.factor = factor
    register player under original F2351 lock
    F234b.M2fee(Action(M3bb6(player)),2)
    return player

on reached original queued M3bb6:
    player.M140b(true,false)
        if disabled: return
        if sample handle is0 or-1: set original flag; return
        channel = existing valid channel OR BASS_SampleGetChannel(sample,false)
        preserve original attribute11 setup and base-attribute1 read
        set original numeric attributes2/3/1 using volume/global/pan/factor
        BASS_ChannelPlay(channel,true)       // return discarded
        set original flag                   // not proof that sound was heard

spinner positive completion:
    M3202 queries timing/sample at stored end, without+5
    share M24ba and remaining resource/queue/native path
```

UNRESOLVED: exact encrypted asset keys/default resource contents, complete skin /
provider/cache policy, queue timing/cancellation/native errors and audible output.
Stop after E037; no E038/R11/audio implementation or deferred-score investigation.
