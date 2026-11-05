# 1. Result

Expedition041 — Original Spinner Audio, Frequency Modulation & Channel Lifecycle.
Work1 original-static research,2026-11-05 Asia/Bangkok. Starting canonical main
`b95ed38b5103c44235964e44d637b04603409502`. No authorised exception.

**Recovered:** ordinary spinner motion-driven continuing requests, integer-change
frequency requests, the bonus-specific one-shot path, positive completion sound,
shared channel replacement and deferred cleanup. Spinner audio uses global
**F2356**, separate from the slider pair2354/2355 and from one-shot player channels.
The continuing callback requests **Play(false)**; slider starts use Play(true).
The loop request is not selected by a spinner score/result code.

**RECONSTRUCTED readiness:** a bounded modern, fresh same-map, single-producer
managed R14 model is justified. It requires its own persistent channel and explicit
frequency/length backend operands, plus independent `spinnerspin`/`spinnerbonus`
assets. Audible frequency modulation, native scheduling, arbitrary reuse/device
states and overlapping producers remain **UNVERIFIED ORIGINAL-RUNTIME**. Existing
WindowsSliderLoops does not implement numeric attribute1, so current R13 playback
alone cannot establish an audible spinner-frequency implementation.

Created only this report, `evidence/41/spinner-audio.json`,
`tools/spinner_audio_evidence.py`, `tools/check_spinner_audio.py`, and an
append-only E041 journal entry. Reconstruction, R13-C01/QA11 evidence, earlier
research/assertions/fixtures and E020 remain unchanged. Publication SHA is supplied
in the verified handoff. No R14,QA12,E042 or deferred scoring work is started.

## Input identity and procedure

| Private input, verified before instructions | Bytes | SHA256 |
|---|---:|---|
|osubackup.zip|78546961|7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
|ZIP member osu!.exe|4541728|6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|

Self-described Stable40/file version1.3.3.8 remains unauthenticated as a dated,
unmodified official build. Host Windows/Python3.12; existing pinned dnfile0.18.0,
dncil1.0.2,pefile2024.8.26. Static PE/metadata/CIL/resource hashing and mathematical
string decoding only. No original CLR, native BASS call, executable, game/window,
physical input, debugger, OBS, listening, media opening or capture ran. No original
sample contents are exported. E020's17 cases remain NOT RUN. The1344-check QA
checkpoint is historical, **not rerun** by extraction or checking. No new
VERIFIED COMPONENT gameplay/audio claim is made.

Preparation follows AGENTS/directed RTK/full standing contract and both journals;
E024–E026/E037–E040 reports/evidence and relevant E010 ordering; R07/R11–R13/
R13-C01 and QA05/QA09/QA10/QA11; current parser, spinner, effects/resource/channel
interfaces. E005's correction is preserved: ordinary M48e1→T00d8, not the earlier
Taiko M4270/M1d6d mapping. Prior reports are not revised.

Competing explanations discriminated directly: component-code-triggered loop
versus motion update; changed integer versus every crossed integer; immutable
queued channel versus live global reads; universal rate matching versus distinct
spinner frequency; standard table versus named bonus resource; end+5 versus stored
end lookup; Stop versus queued Pause; full reset versus surviving fields/actions.
No remembered spinner convention or community filename was used as evidence.

## Deterministic verification

The collector selects152 MethodDefs with owner/signature/RVA/body hashes,
code-relative branches, token/constant/conversion/local sites, field declarations,
slot candidates/ancestry/MethodImpls, native imports, complete direct watched-field
and caller inventories, nine decoded strings and embedded metadata fingerprints.
Full scan:19659 decoded bodies/1739 zero-RVA entries/0 decode errors. Native,
abstract and CLR delegate declarations are not executed as managed bodies.

Two fresh final extraction processes are byte-identical and equal the published
JSON: **PASS**,1542138 bytes/SHA256
`4fae962a1d69000f522be14ba5c91b3587d001e245795287a457ee058a010a5e`.
New literal/independent archive checker: **PASS**,152 selected methods,
134 original managed bodies byte-audited,12 complete direct F2356 accesses,
443 historical body-hash comparisons,nine independent string decodes and both
embedded resource fingerprints. Watched field sites3096/direct caller sites184;
66 hand-derived predictions remain unexecuted original-static controls.

All seven unchanged E024/E025/E026/E037/E038/E039/E040 relationship checkers
**PASS**. Additional original-archive audits: E038 **PASS**,56 independent string
decodes/12 embedded standard keys/173 historical hashes; E039 **PASS**,92 methods/
499 historical hashes; E040 **PASS**,119 methods/26 direct channel accesses/
477 historical hashes. Those dynamic counts include the new matching E041 record;
historical assertions were not edited. Twelve independently supplied evidence
mutations (restart Boolean, length equality, ratio conversion, cache ordering,
end lookup, Pause import, resource key, sole writer, loop flag, bonus flag,
bonus suppression, cleanup target) were all rejected. New tools compile in memory.

Collector source SHA256
`e52690d23fdfc5953a742cb63a4177ac9dcdaff518ac400a40e50313b58a35f1`;
checker `c14cc02f6b53199ab47b4c4d8076eb96a0fd3a7d071c03ad4eb00d2aa7e44f8e`.
Initial148-method extraction/archive audit passed, then four helper bodies were
included. A private shell edit helper failed quoting before writing either file;
direct patches corrected the helper task. No original experiment or historical
assertion failed or was changed. These are static-tool consistency/rejection
checks, not original playback or a rerun of1344 independent QA checks.

Representative body hashes include original method headers:

| Method / owner | Raw signature | RVA | SHA256 |
|---|---|---|---|
|M05fb/T00d8|200001|1ec94|c7f02996266b22b1556548e1296cb34b52c5ec9fa4c16a3069791d3294af9101|
|M3b28/T07b9|0001010c|18160c|fe634ce4a424ecbe6945bc8132dc1d437bfffbddc23ff5c2c34fe0830c7d25fc|
|M3b2b/T07b9|0003128aa40e08119be4|1816b0|a8480b538148db62dc07fcab4758c08a73830d55acbd2c1b61590bd07a08bdd1|
|M3b34/T07b9|00010115118801010c|181ca8|ed0bae1315cce02d8ca98b83fb111ae78fde5875b587d53726f7c4eaeca62d23|
|M3ba3/T07c4|200001|1838a4|efd7fe92fb159baf1fb2482953cd147b3ca2d6ee97e021aa26bb211e58ac9a3b|
|M3ba9/T07c4|200001|18395c|86e39778b7cd0d062da8c7210be5bd2c88c021802bf016eb9da573831c39b4e7|

Reproduce from repository root with the pinned packages already available:

```text
rtk proxy python re/tools/spinner_audio_evidence.py /private/osubackup.zip /private/spinner-a.json
rtk proxy python re/tools/spinner_audio_evidence.py /private/osubackup.zip /private/spinner-b.json
rtk proxy python re/tools/check_spinner_audio.py /private/spinner-a.json /private/osubackup.zip
```

Actual host commands prepend `../.research-deps` and `re/tools` to Python's module
search path; no installation or CLR loading. The separately authored checker does
not import the E041 collector. Archive mode independently checks original bodies,
literal sites, the watched global inventory, native import mappings, string bytes
using E038's independent reduced key, and resource fingerprints. Conditional
cases are hand-derived descriptions, not an executable spinner/audio simulator.

# 2. What they found

**CONFIRMED STATIC** below means pinned original instructions/metadata.
Semantic aliases, equations and predictions are **RECONSTRUCTED**. Future backend
policies are **ENGINEERING**. All original runtime/audible outcomes remain
**UNVERIFIED ORIGINAL-RUNTIME**. M/F/T abbreviate MethodDef0600xxxx,
FieldDef0400xxxx,TypeDef0200xxxx. Offsets are hexadecimal from the first CIL opcode,
headers excluded; JSON offsets are decimal. Preserve each separate field/clock
read, callback, native result and exception boundary.

## A. Exact channel identity, writers and preparation

Ordinary ancestry: **T00d8→T067f→T04c3→T0828**.
Factory M48e1/M05ea/M3201 stores raw sound F2526, start/end F2523/F2524 and
centerF252c=(256f,192f). Unlike circle construction, M3201 copies the supplied
sound mask directly. M48e1 then writes base/addition F1537/F1538:0011/0019,
indexF1539:0021, volumeF1536:0029 and invokes M24b9(filename):0031. These fields
participate in final hitsound selection, not in selecting `spinnerspin` by bank.

| Field | Declaring type/signature | Role |
|---|---|---|
|F2356|T07b9/static0608 Int32|One shared continuing spinner channel|
|F2354/F2355|T07b9/static0608|Separate continuing slider pair|
|F0bc0/F0bc1|T02a9/0608|A particular effects-player channel/sample|
|F2360/F2361|T07b9|Current global sample index/base bank, updated by E040 timing path|
|F02af|T00d8/060d Double|Smoothed signed motion U|
|F02b4/F029e/F02a2/F02a0|T00d8|Single progress P/cached integer/observed-change count C/prepared N|
|F36af|T0b3a/0602 Boolean|Metadata name SpinnerFrequencyModulate|
|F234b|T07b9|Shared original action scheduler|

F2356 has CLR default0: no constant/FieldRVA/ThreadStatic attribute or explicit
initializer store. Complete direct managed inventory has **12 accesses**:

| Method | F2356 CIL-relative sites |
|---|---|
|M3b11|0033 read|
|M3b1f|009d/00a5/00b1 reads; **00d4 sole direct store**;00f2/0104 reads|
|M3b28|0000/0012 reads|
|M3ba3|0000/0017 reads|
|M3ba9|0000 read|

No per-spinner owner token or channel dictionary exists in this direct chain.
Reflection/native/external mutation and indirect aliases are outside the inventory.
No direct zeroing/freeing of F2356 is established merely from sample cleanup.

### Global refresh M3b1f and replacement

M3b1f on T07b9, signature`00020111ac3c02`, static Void(baseSet,force), retains
E040 gates: unless forced, unchanged requested base/current sample index returns;
changed selectors also return outside application numeric1/2. A reached refresh
normalizes base0→Normal1, stores F2361:002f, rereads M3b3c and stores F2360:0039.
After shared standard-resource refresh it **always** reaches the spinner block;
this differs from the later conditional slider-pair refresh.

1. If F2356!=0 and fresh ChannelIsActive(F2356)==1, call
   M3151/**BASS_ChannelStop** on another field read:00b6. Other native activity
   values skip Stop; they do not prevent replacement.
2. Decode `spinnerspin` at00bc/00c1, call
   **M3b14(name,true,true,7)**:00c9. First true requests loop loading; second true
   selects the unprefixed/nonindexed name path. It is not a bank×plane table lookup.
3. **BASS_SampleGetChannel(sample,false)**:00cf; store returned Int32 in
   **F2356:00d4**. No sample−1/0 early return like M3b31 is inserted here.
4. Compute gain into a Single local. Set numeric attr11=1:00fe, then attr2=g:010b
   on separately read current F2356. Native Boolean results are popped.

A global timing sample-index/bank change can consequently replace the spinner
channel even though its resource name itself has no index or bank suffix. Cached
sample reuse does not prove that GetChannel returns the same native channel.
Force bypasses the early admission gates and repeats this spinner replacement.
There is no spinner-object argument or owner check. Exceptions can leave selectors
updated or an old channel stopped before a later acquisition/store succeeds.

Loading M28f5:023b calls M3b19;0241 sets global base0, then version/status branches
can call M3b1f:0263. E040 M3b61's later current-global-point refresh is another
reached source. Do not promise channel creation at a particular spinner start or
substitute the spinner's own timing query for those separate global paths.

## B. Frequency modulation: two original producers

### Observed integer change, before component selection

E025's M05f6 compares M05eb(P) with cache:02d5–02e0. Equal returns0. Different
increments C **once** at02ed, including a jump across several integers. It then
dereferences F2f16.F36af:02f7. If true, reads **C once at02ff and N once at0306**,
converts each separately to Single, divides at030c and calls M3b28:030d.
This precedes fresh C/N bonus/parity comparisons and late cache reread/store04c7.
It is not one request per mathematical integer crossed, score unit or frame.

The setting is initialized true by SkinOsu M534e:0093/0094. M5353:00f7–0107 reads
the independently decoded key`SpinnerFrequencyModulate` into that Boolean.
This default is not proof of a particular loaded skin's setting. Null skin can
throw after C has already advanced, before cache or result handoff.

### Motion update, before progress accumulation

On the ordinary M05f5 integration path, separate fresh comparisons require
**start<T<end**, !F1376 and positive F2a94 (0143–0173), after the method's early
evaluated/pre-start guards. Existing smoothing/presentation work produces U and
step. At0353 the method rereads **Double U**, not the Single step:

- If U equals0, call virtual cleanup M24c4→M05fc at036c.
- Otherwise call **nonvirtual M05fb at0364**.
- Only afterward accumulate/store Single P at0395.

M05fb dereferences the same setting. True reads C:0018 and N:001f once each,
converts each to Single, divides0025 and wraps Nullable<Single>. False constructs
an absent Nullable. Either arm calls **M3b34(nullableRatio):002b**.
M3b34 calls M3b28 synchronously only if HasValue, then queues the continuing start.
Thus disabling frequency modulation does **not** disable continuing sound starts.
Residual U after release can still request a loop; direction does not choose a
different resource. Before the first integer change, C0/N15 can request frequency
operand20000 and a start while the component evaluator has returned0.

Outside the strict integration region the branch is skipped, not converted into
an automatic pause. Exact start/end, nonpositive update intervals, paused/readiness
or missing-candidate paths must retain their own original gates/cleanup routes.
Edit's separate preparation/progress path is not silently treated as ordinary Play.

### M3b28: exact requested numeric attribute

Static T07b9 signature`0001010c`, Void(Single ratio):

```text
length = M3121(fresh F2356)              #0000/0005
         -> BASS_ChannelGetLength(handle,0)  #M3121:0001/0002
if signed Int64 length <= 8192: return   #000a-0010
target = fresh F2356                    #0012, after native query
k = original conv.i4(Single(40000f * ratio))
frequencyOperand = Single(Min(100000,unchecked(20000+k)))
BASS_ChannelSetAttribute(target,1,frequencyOperand)  #0031; result discarded
```

There is no Activity/nonzero/channel-validity/ratio-positive/isfinite/lower-clamp
test. Returned native length is compared as signed Int64; **8192 equality skips**.
Native length is not the encoded resource ByteArray length. The length unit and
error result are not inferred from asset size or remembered BASS conventions.
For finite safe ratios0/.5/1/2/3/−1, requested operands are respectively
20000/40000/60000/100000/100000/−20000. This is a managed request equation, not a
measured pitch curve. Positive C/N generally changes it only on reached producers.

N0 uses floating division, not integer divide-by-zero: zero/zero may yield NaN,
positive/zero positive infinity. Subsequent unchecked floating conv.i4 for
nonfinite/out-of-range values has no pinned observed result here. Keep its output
symbolic; do not manufacture a frequency, throw a checked-conversion exception or
substitute a current CLR cast. The Int32 add is unchecked before integer Min.

The channel can change during the native length query; the later attribute write
uses the new field read. This method itself queues nothing. A thrown helper after
C increment can leave C advanced/cache stale; a thrown update helper can leave
U/presentation changed but P not yet accumulated. Native false return is different
from a managed exception and is discarded, without rollback.

## C. Continuing start, gain and channel lifecycle

M3b34 signature`00010115118801010c`, static Void(Nullable<Single>), caches
Action(**M3ba3**) and submits **F234b.M2fee(action,2):003a** after optional frequency
work. There is no active1 test, pending-start flag, spinner identity or rate-match
argument. Every normally returning reached call can enqueue another action.
Queue argument2 is an original policy operand, not a claimed two-millisecond delay.

M3ba3(T07c4) on execution:

1. Read F2356:0000 as attribute target **before** calling M3b41 for fresh gain.
2. Set attr2 to M3b41()/100f:0011.
3. **Reread F2356:0017**, call **BASS_ChannelPlay(channel,false):001d**.

No channel, spinner, volume, timestamp or generation is captured. Target changes
between writes are possible; false is an exact restart operand, not proof of
resume/seamless behavior. Unlike slider starts, it is **not true**. There is no
spinner-loop pan write or circle-style song-rate factor in these methods.
Neither old count/new spinner boundary nor disabling modulation explicitly
restores attribute1; resource acquisition/native state remains an external input.

Preserve E040's arithmetic and fresh reads:

```text
G = Single(unchecked(effectInt*universalInt)) /100f       #M3b40
V = noSongMap ? 100 : currentGlobalPoint ? Max(8,point.volume) : songMap.F1000
start attr2 = ((Single(V)*G)/100f)/100f                 #M3b41 then M3ba3
replacement/settings attr2 = ((G/100f)*Single(V))/100f  #M3b1f /M3b11
```

Song-map fallback has no added minimum. Continuing gain consumes current global
timing state, not spinner sample-volume override or its final end-time point.
M3b11 retains one gain local at0018 for slider0,slider1 and then spinner
F2356 at0033/0039/003a. Targets are still reread separately. R13-C01/QA11's
correction is preserved; E041 makes no implementation change.

### Cleanup and reset boundaries

**M05fc→M3b54** caches Action(**M3ba9**) and queues it with policy2. At execution
M3ba9 reads **current F2356:0000**, calls **M313d/BASS_ChannelPause:0005**, pops the
Boolean result and returns. No handle capture, ownership/activity/validity/generation
test, field clear, Stop or SampleFree occurs in this callback.

| Reached route | Order and scope |
|---|---|
|M05f5 U==0|Queue cleanup before P accumulation; only inside reached strict update region|
|M05fa completion|Virtual cleanup at0001, optional presentation helper, evaluated=true0020, then grading/positive hook|
|M2262 pause-on|Reached current-candidate enumeration calls virtual cleanup01da before later song toggle020b|
|M0acb reached false arm|Conditional current-candidate cleanup003c; preserve its upstream F05f8/F136a gates|
|M28c5 manager cleanup|Each full-list object: virtual cleanup0050, then virtual dispose0055, later list clear|
|M05f4 /M0604 setup/end setter|Resets P/prepares N, not an established global-channel or full C/cache reset|

Ordinary spinner has no additional override of base M24ab disposal. E040's
slider-specific manager-tail native-state query M3b09 only checks2354/2355; it
must not be reused as a proven spinner ownership/stop policy. More generic cleanup
callers remain inventoried, but their editor/other-ruleset reachability is not
promoted into a universal fresh-Play lifecycle.

M28f6→M3b1d frees map-cache sample handles; M3b1b clears skin/map/embedded caches
through M3b1c/SampleFree. Reached M3b5e device teardown can also BASS_Free.
These are distinct from ChannelPause/Stop. None directly clears F2356: the only
managed writer remains M3b1f. Timing/song reset alone does not zero the channel.
Outstanding actions have no recovered map-generation cancellation. Full device,
shutdown, seek/editor/reused-manager/native validity behavior stays unresolved.

## D. Bonus-associated sound and resources

After C mutation and optional frequency request, M05f6 selects bonus only when
signed **C>unchecked(N+3)** and signed
`unchecked(C-unchecked(N+3)) %2==0` (0320/0336). After optional presentation flash
it assigns raw16384:0358/035d. It then tests global modifier bit**4194304**; if
present, skip the bonus-sample request without changing raw16384 on normal return.
No descriptive modifier name is invented from that bit.

If the bit is absent, decode **spinnerbonus**:037a/037f; obtain **M3b42()** current
global volume:0384; call **M3b2b(name,volume,7):038a**. This is a named one-shot
creator, not a write to F2356, not M3b31 and not a standard-layer request:

1. M3b2b calls **M3b14(name,false,true,flags)**:000b (nonloop/unprefixed).
2. Construct exact T02a9/M13fb(sample):0010; set player volume to
   Single(volume)/100f through M13fd:001e.
3. Register that player in F2351 under the original lock, then queue closure
   **M3b87(player)** via M2fee(action,2):0066; return the player.
4. M3b87 calls **player.M140b(true,false)**:0008. E037's concrete player obtains
   its own channel via M1406/GetChannel(false), writes numeric attributes and
   requests Play(true). This is separate from global spinner Play(false).

No custom filename, bank layer, spinner pan or song-rate adjustment is set by
M3b2b. Fresh player pan is0, factor1; M1404's pointer-derived pan arm requires
F0bbe>0, absent here. Volume uses global M3b42, including its own floor/fallback,
then ordinary player's effect/universal gain; there is no standard .8/.85 layer
attenuation. Timing/volume can change before a later callback/settings refresh.
M3b2b itself does not validate sample−1/0 before player registration/queueing.
M140b/M1407 later reject these sample values; no standard-layer fallback follows.
Native acquired channel0 is a different conditional failure boundary.

Normal raw4096/8192 and first raw0 have no dedicated one-shot producer in this
selector. The same call may still have issued frequency work, and a later update
can issue continuing work. Component amount0/100/1100 is downstream accounting,
not an audio event identifier. After the bonus request, original label/transform
helpers run before the second M05eb/cache store. Their exception can leave a queued
bonus with stale cache/no returned component. No rollback/cancellation is inferred.

### Resource keys, providers and independent-asset requirement

| Identifier | Decoded key | Source caller |
|---:|---|---|
|2064529249|spinnerspin|M3b19:015a/015f;M3b1f:00bc/00c1|
|2064529271|spinnerbonus|M3b19:016d/0172;M05f6:037a/037f|
|2064456445|SpinnerFrequencyModulate|M5353:00f7/00fc|

These strings are independently decoded from the original encrypted table,
preserving E038 caller profile/algorithm/cipher fingerprints. They were not
guessed filenames. M3b19 preloads spin(loop=true) and bonus(loop=false), both
unprefixed=true, stages7. Spin refresh and bonus event calls reach the same
M3b14 provider machinery. **Neither gets normal/soft/drum prefixes or index
suffixes**, and unlike normal table plane0, unprefixed=true permits the map stage
even when global index0. No indexed spinner-specific variants are reached.

Provider order, when its original gates permit: map/custom F234e[key], then
skin F2350[key], then embedded F234f[key]. The unsuffixed map candidates are
**key.wav→key.mp3→key.ogg**. Cache lookup precedes file/provider work. Map suppression,
current selected map, default-skin/SkinSamples and process gates remain explicit
E038 operands. Nonnull bytes terminate extension probing even if the native load
returns−1. Map/skin0 stays0; embedded native0 becomes−1. Missing keys can yield−1.
No catch guarantees fallback after every provider/null-reference exception.

Requested native flags start131072; spin adds4, bonus does not. Short map bytes
reach SampleCreate(0,44100,2,8,flags); other map bytes use memory SampleLoad max8;
skin file wrapper max8 adds0x80000000; embedded memory max5. Native outcomes and
decoded sample format/rate/length are not established by managed flag requests.
Loop flag is **not** in cache identity: a prior colliding custom/standard key may
prevent a new loop-enabled native load. Do not invent separate extension/loop caches.

Metadata-only inventory (no sample bytes distributed):

| Resource set | Key | ByteArray bytes | SHA256 |
|---|---|---:|---|
|osu!gameplay.dll|spinnerspin|36868|3a5dabea4ffa34d5f1820d94d68a09c254a41fd903dc6583dcd1860d1ad67b9f|
|osu!gameplay.dll|spinnerbonus|309536|0c54b23642a4abf3faf18c02d7a3a41cc6c3a0fb042a740dd6a9b30d26ce17e9|

Both keys are absent in osu!ui.dll's727-entry base set; present in gameplay's456-entry
base set. DLL hashes/resource-set names are in JSON. These fingerprints neither
license the assets nor prove native ChannelGetLength>8192 or audibility. Work2
needs independently supplied replacements. Mapping a supplied key to a loose WAV
in a default pack would be an engineering mapping, not the original embedded path.

## E. Completion hitsound: stored end, shared selection

M05fa queues cleanup first and stores evaluated=true before classifying.
Legacy-positive branch calls virtual M24bb:0042; modern N0 calls0080; modern
minimum-passing calls00a0. Minimum failures return−131072 without this hook.
Positive quality300/100/50 is determined after the hook or its later comparisons,
with E026's fresh C/N/compatibility reads unchanged. This hook can therefore throw
before a final positive grade returns. Later Perfect manager conversion is separate.

Slot resolution is **T067f.M3202**, not circle M24bb or slider M1466. It reads
manager.map, queries **M1be6(Double(stored end F2524)):0012, without+5**.
Null timing returns0034. Otherwise call **M24ba(point,F2526,F1537,F1538):002f**.
M1be6 retains supplied-order scan-all/last qualifying `time<=query`, first record
fallback when none qualify, null for null/empty list. It is not the global cursor.

M24ba preserves shared E037 operations: nonzero object base/index/volume override
the end-selected timing values; addition0 ultimately inherits selected base;
M44a2 always adds Normal to mask0 and conditionally adds it to nonzero mask through
LayeredHitSounds/selected-map gates. Observer M28a1 runs before either dispatch arm.
CustomF1534!=-1 bypasses standard layers, including custom0. Otherwise expand
**Normal→Finish→Whistle→Clap**, volume Max8 then Double attenuation .8/1/.85/.85
and conv.i4, E038 bank/index/table/cache lookup. Custom arm has no standard floor.
M3b31 uses its separate player/queue/rate-match path, not the continuing channel.

Virtual pan resolves base M24bd: `(fresh F252c.X/512f-.5f)*.800000011920929f`.
Fresh spinner centerX256 gives0. M05ef can later write F252c, so this is a fresh
position read, not a universal immutable zero. The cursor sampling center
F02a3.F2b6e and slider presentation-X bridge are different operands.

Missing timing/resource can suppress final playback while positive classification
continues on normal return. Null manager/map/observer/helper exceptions can instead
interrupt it after cleanup/evaluated mutation. No dedicated audible MISS is proved
by this hook. Shared result-consumer feedback sounds are outside this narrow hook.

## F. Ordering, successive objects and finite controls

Normally returning original order, keeping separate clocks/reference reads:

1. E024 current-object input samples cursor/rotation; observes old P.
2. Changed integer increments C once; optional synchronous frequency request.
3. Fresh C/N select component; conditional bonus request/label/transforms;
   reread P for cache, return raw, then ordinary ruleset accounting.
4. Refreshed candidate M05f5 smooths motion and updates presentation; U!=0 calls
   optional frequency then queues continuing start, U0 queues Pause; **then** P store.
5. Later manager strict `end<freshT`/evaluated/application gates reach completion.
6. Completion queues Pause, stores evaluated, positive branch requests end-selected
   hitsound, then returns grade for manager conversion/accounting.
7. Audio worker executes already-submitted actions under its own schedule; global
   timing/resource refresh or a later spinner can alter F2356 before execution.

At exact end no integration-loop branch or manager completion occurs under equal
clock operands; input can still observe pending P. A later after-end input can
update C before completion. No pending-integer drain, score-event-to-audio mapping,
once-per-frame rule or atomic time/reference snapshot is introduced.

Successive spinners share F2356. A later spinner may reuse native channel state or
global refresh may replace it. Prior Pause can target the replacement; a queued
start reads fresh gain/handles and can execute after cleanup. No managed owner
arbitration or generation token fixes an audible result. ChannelPause, Stop,
SampleFree, BASS_Free and field assignment remain separate events.

JSON contains **66 finite controls A01–A66**, each with exact common managed fields,
explicit overrides/native alternatives, path, ordered mutations, queued actions,
native requests, evidence sites and unresolved dependencies. All are
**RECONSTRUCTED STATIC PREDICTIONS**, not executed original playback/QA cases.

| Controls | Distinctions |
|---|---|
|A01–A08|Before/start/interior/end, positive/negative/zero U, repeated calls, interval gating|
|A09–A17|First/second/unchanged/jumped observations, bonus equality/parity/modifier gate, setting disabled|
|A18–A27|Finite frequency operands, negative ratio, zero/nonfinite/overflow symbolic conversion|
|A28–A38|Signed length8192 boundaries, invalid handle, live targets, partial exceptions, deferred gain/Play rereads|
|A39–A48|Global refresh admission/force/replacement, unsuffixed providers, sentinel distinction, bonus missing sample|
|A49–A59|300/100/50/MISS, N0, missing/end-selected timing, layers/overrides/custom0|
|A60–A66|Pause reread, manager/pause cleanup, successive objects, map/cache/device boundary, settings and exceptions|

## G. Bounded R14 handoff

The managed subset is sufficiently recovered for a future bounded implementation;
no code is implemented here. Minimum contract:

| Input/state | Required operation/output |
|---|---|
|Exact spinner identity/start/end/raw mask, base/addition/index/volume/filename|Separate final-sample metadata; retain raw spinner mask, not circle-normalized bits|
|P/cache/C/N/U with original stage order|Observation-frequency/bonus, motion-loop and final-hook events remain distinct from raw ledger labels|
|SpinnerFrequencyModulate,modifier4194304,global timing/selectors/settings|Correct nullable modulation, bonus suppression, channel refresh and current gain|
|Prepared spinnerspin/spinnerbonus and shared caches|Unprefixed unsuffixed stages7 lookup, loop flag only spin, no invented bank/index table|
|One global F2356-equivalent, no per-object ownership|Conditional Stop/replacement, Acquire(false), live-target callbacks, distinct queued Pause|
|Backend Length(handle,0),Attribute(handle,1/2/11,value),Play(false),Pause,Stop|Record exact logical/native operands; actual frequency/output support is a separate capability|
|Existing one-shot infrastructure and end-selected timing|Bonus player true/false and standard/custom final requests without changing score/HP|

Current R07 retains raw HitSound/HitSample and gameplay state but has no spinner
sound hooks. Registered override preloading retains index/filename, not a complete
spinner-associated final metadata sidecar. A future parser preparation must preserve
full per-spinner identity/overrides and a custom-handle result. Observation cache
currently follows a serial selector; audio must be placed at the recovered
pre-cache/pre-result boundary if original partial-failure behavior is modeled.
M05f5's continuing helper belongs before P accumulation, not simply after Integrate.
The positive completion hook belongs before later classification/accounting.

Reuse E037/E038 standard tables, providers and player sink where reached; reuse
E040 global timing/gain and persistent backend vocabulary while keeping the spinner
channel separate from the slider pair. R13's current Windows adapter ignores attr1,
and its resource-ID/activity/dispatcher cancellation are engineering substitutions.
It cannot silently claim original frequency modulation merely by looping a sample.
An explicit frequency-capable backend contract and supplied asset/base-rate policy
are needed for audible modulation. A backend that cannot honor that operation must
report it unsupported; a narrower milestone can support bonus/final effects and
nonmodulated persistent spin while keeping modulation explicitly unavailable.

Recommended finite domain: fresh same-map modern ordinary play, finite supported
timing/metadata, one spinner producer, explicit settings and owner-supplied independent
assets, no ambiguous shared-cache collisions. Native length/frequency can be injected
for headless tests; do not substitute encoded byte size for the length query or
manufacture exceptional conversion outputs. Test first/jumped observations, motion
without results, repeated Play(false), zero-U Pause, active1 replacement Stop,
callback target mutation, gain capture, bonus gate, stored-end timing and private
health-calibration silence independently of scoring.
Frequency support should initially require positive N and a finite in-range
conversion product, or return an explicit unsupported modulation outcome. N0
final grading remains independently recovered; unsupported modulation must not
invent a frequency fallback or change spinner judgement.

Engineering validation, safe paths, no callback I/O, effect-error containment,
map-generation cancellation and safe disposal can be appropriate but must be
labelled. They differ from original partial mutations/live global callbacks.
Overlap arbitration, editor/seek/reused-manager/device teardown, native format/error
returns/channel sharing/length units/frequency semantics, scheduling, acoustic
latency, audible pitch and universal osu!stable equivalence remain unresolved.
No implementation readiness claim depends only on knowing the resource names.

# 3. Pseudocode

**RECONSTRUCTED from CONFIRMED STATIC**. Documentation only. `fresh` denotes
separate original reads. Native/queue results remain conditional.

```text
on original M05f6 observation after angular prefix:
    if conv.i4(fresh P)==cache: return raw0
    C=unchecked(fresh C+1)
    if skin.SpinnerFrequencyModulate:
        modulate(Single(fresh C)/Single(fresh N))
    select component using fresh C/N
    if bonus predicate:
        optional presentation flash; result=16384
        if not(modifiers&4194304):
            sample=M3b14("spinnerbonus",loop=false,unprefixed=true,stages=7)
            player=new T02a9(sample);player.volume=Single(M3b42())/100f
            register(player);queue M3b87(player): player.M140b(true,false)
        retain label/transforms and exceptions
    cache=conv.i4(fresh P)
    return component for existing ruleset accounting

on reached ordinary M05f5 strict interior/positive-interval path:
    retain smoothing/clamp and presentation mutations
    if fresh U!=0:
        ratio = skin.modulate ? Nullable(Single(fresh C)/Single(fresh N)) : absent
        if ratio.HasValue: modulate(ratio.Value)
        queue QStart                            # no native activity test
    else: queue QPause
    P=original Single accumulation              # AFTER helpers

modulate(r):
    length=BASS_ChannelGetLength(fresh globalSpinnerChannel,0)
    if signed length>8192:
        target=fresh globalSpinnerChannel
        k=original conv.i4(Single(40000f*r))      # invalid/nonfinite remains symbolic
        BASS_ChannelSetAttribute(target,1,Single(Min(100000,unchecked(20000+k))))

on reached global sample refresh:
    retain force/unchanged/application gates;store global base/index
    if channel!=0 and IsActive(fresh channel)==1: Stop(fresh channel)
    sample=M3b14("spinnerspin",loop=true,unprefixed=true,stages=7)
    globalSpinnerChannel=GetChannel(sample,false)
    g=original replacement gain computed once
    Attribute(fresh channel,11,1f);Attribute(fresh channel,2,g)

QStart execution:
    target=fresh globalSpinnerChannel
    Attribute(target,2,fresh M3b41()/100f)
    Play(fresh globalSpinnerChannel,false)
QPause execution:
    Pause(fresh globalSpinnerChannel)            # not Stop,free or field clear

on reached M05fa completion after manager's strict end gate:
    queue QPause;retain presentation prefix;evaluated=true
    retain E026 fresh compatibility/C/N minimum tests
    if minimum failure: return -131072
    point=map.M1be6(Double(storedEnd))            # no +5
    if point!=null:
        select M24ba object/timing overrides;augment mask conditionally
        observer(info)
        custom!=-1 ? custom request : standard Normal/Finish/Whistle/Clap requests
    retain E026 later grade comparisons and manager/accounting

UNVERIFIED:
    actual worker/native interleavings,format/length/frequency effects,device output,
    exceptional target CLR conversion,audible continuity/pitch and original equivalence
```

Stop after E041. No R14,QA12,E042,framework migration or deferred scoring inquiry.
