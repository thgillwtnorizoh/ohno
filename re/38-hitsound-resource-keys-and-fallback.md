# 1. Result

Expedition 038 — Original HitSound Resource Keys, Lookup & Fallback.
Work 1, 2026-11-04 Asia/Bangkok. Base verified canonical main
`4da509a5e63498b16c1ce6d7d555a9bfc4553ab9`.

**CONFIRMED STATIC:** E037's resource identifiers can be decoded independently
from the supplied executable's metadata and encrypted resource bytes. The
ordinary standard keys are `normal|soft|drum` + `-` +
`hitnormal|hitwhistle|hitfinish|hitclap`. Standard map preparation probes
**.wav → .mp3 → .ogg**; custom-name fallback probes **.wav → .ogg → .mp3**.
These are different original algorithms. Map, skin and embedded caches also have
different keys, admission conditions and zero-handle treatment.

The twelve standard circle byte-array resources **are present** in the archive's
`osu!gameplay.dll`; they are absent under those keys in `osu!ui.dll`. No audio
contents were exported, played or published. A supplied resource pack remains
necessary for a distributable independent implementation; this report grants no
asset redistribution rights and creates no substitute assets.

**RECONSTRUCTED assessment:** E037+E038 supports a bounded ordinary-circle
resource-to-request implementation with explicit chart, loose-file provider,
skin/settings and asset-pack inputs. Complete `.osz2` provider internals,
arbitrary process state, native sample-loader outcomes and acoustic equivalence
remain unsupported or **UNVERIFIED**. No R11 work was begun.

Created the report, `evidence/38/hitsound-resources.json`,
`tools/hitsound_resource_evidence.py`, `tools/check_hitsound_resources.py`, and
appended `contexts.md`. No other tracked file changed. No authorised exception.
Publication SHA and verified remote head are supplied in the handoff.

## Preparation, identity and verification

Read actual `AGENTS.md`, directed RTK.md and complete `re/Instruction.md`;
consulted the cumulative research/reconstruction records, E004/E005/E010/E012/
E026/E037 and E037 evidence, current CircleGame parser, BeatmapAudio and Windows
practice README. The canonical contract assigns original research to Work 1
and publication to current `main`. E005's ordinary/Taiko correction is preserved:
ordinary M48df forwards metadata; E037's M24bb performs the runtime start+5
lookup. No old E004 wording, E037 claim or historical checker is rewritten.

Actual archive and EXE hashes verified **before** E038 instruction inspection:

| Input | SHA256 |
|---|---|
|osubackup.zip|7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738|
|ZIP member osu!.exe|6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d|

Same self-described Stable40/file-version1.3.3.8; official dated authenticity and
modification status remain unresolved. Windows/Python3.12,
dnfile0.18.0/pefile2024.8.26/dncil1.0.2. Only static metadata/CIL/resource bytes
were read. No original executable, CLR method, resource-manager getter, native
loader, game, audio device, physical input, debugger, OBS or listening experiment
was executed. E020's17 cases remain NOT RUN. The628 independent QA checkpoint
is preserved, not rerun by this research.

Competing explanations tested with original instruction relationships: literal
filenames versus decoded keys; one universal extension order versus separate
standard/custom loops; map-only versus map/skin/embedded fallback; index0 map
lookup versus a skipped map stage; null/missing/zero equivalence versus distinct
sentinels; judgement-time disk lookup versus earlier preparation and cache lookup.
Private normalized instruction inspection, independent arithmetic reduction and
two independently expressed string decoders distinguish these possibilities.

Reproduction from repository root with the pinned Python dependencies available:

```text
python re/tools/hitsound_resource_evidence.py /path/osubackup.zip /tmp/e038-a.json
python re/tools/hitsound_resource_evidence.py /path/osubackup.zip /tmp/e038-b.json
python re/tools/check_hitsound_resources.py /tmp/e038-a.json /path/osubackup.zip
```

The optional archive argument makes the checker independently rederive the key,
decode every selected string from original bytes and recheck the twelve embedded
entry fingerprints. Without it the checker checks JSON instruction relationships
and the independent key equations but does not claim a fresh byte decode.

Actual outcomes: two final extractions byte-identical, **1,246,588 bytes**,
SHA256 `f61b5e16baa403731488953301085e1a456b3bceceb8db7d86bd9c9f3b1fd659`.
New checker PASS: **112 methods /56 strings /56 independent original-byte decodes /
12 embedded keys /101 historical body-hash comparisons**. Full inventory:
19659 decoded bodies,1739 zero-RVA declarations,0 decode errors. Six unchanged
E004/E005/E010/E012/E026/E037 checkers PASS with their matching evidence files.
New tools compile in memory; journal-prefix, scope and whitespace checks are
required before publication. No QA/gameplay/listening pass is claimed.

Exploratory verification initially named a nonexistent `check_slider_duration.py`;
the correct unchanged `check_slider_timing.py`/E005 evidence pairing passed.
A new checker draft incorrectly placed M3b14's loop-initializer constant at0139
(the store). Inspection showed `ldc.i4.0` at0138 and `stloc` at0139; correcting
that new assertion produced PASS. No historical assertion or original conclusion
was weakened. Private arithmetic drafts were corrected against instruction
boundaries before the final byte-identical extraction.

Offsets below are **hexadecimal CIL-relative, headers excluded**. JSON uses
decimal offsets. M/F/T mean MethodDef0600xxxx/FieldDef0400xxxx/TypeDef0200xxxx.
Original-static facts are labelled **CONFIRMED STATIC**; independently reduced
equations/cases are **RECONSTRUCTED**; future sink/policy substitutions are
**ENGINEERING**. Original-runtime and audible outcomes are **UNVERIFIED**.

| Method | Owner / signature | RVA | Body SHA256 |
|---|---|---|---|
|M49af|T09f4 /000001|1d2284|fa16ad95d834b4c7e7ca083196cc642c7076bb94758560d06943763575f722ae|
|M49b0|T09f4 /00010e08|1d2404|9446c1df78215e57b6e2502757da0f2ce17673dd92322dbf4d5a680a303014a9|
|M49b1|T09f4 /00020e0802|1d242c|4b3710974ae6c882f667c61f5899c36dede9b8270a700a4acb12930ba0138534|
|M0bbf|T01a7 /00000a|41584|7de1a9849b3ccb8f45474fe3ba9753440ac27e0e91d6174344c03c8a0c202348|
|M0bd5|T01af /000008|41d18|9a7c6e7d2ea7ddf5d9df3bcfb846a18d4f4171e0976413925c4cebb957a56b0d|
|M3b14|T07b9 /0004080e0202119be4|1801d8|902a5828a359274dbb122534d659842802871c1de095fdad99a0964f4c94f725|
|M3b15|T07b9 /0004080e020202|1805b8|7226022009f57a885178efb81cfe930fb5cf9dcbfe1f78d458b5478d7f6b89c2|
|M3b19|T07b9 /000001|1808b8|057805022de1394d9728ebb2ee1fa78a07ed1f4a738aa75ccfa0f50a1cedf9b1|
|M3b36|T07b9 /00040811a61c11ac3c118d7c1184dc|181d28|a756af963988f867cf6feb7108764cce304a461d9ebe57a7ea2b9d79af7e0be0|
|M1c05|T03ad /20011286d10e|ad7c8|e64c88ae1dcd1a8e042a1a62db450aab52d533589f402e80684c1561f60d652d|

Complete identities, relevant field names/signatures, callers, writers, native
imports and body hashes are in JSON. M3b14/M3b15/M3b36 are static. Map M1c05/
M1c06/M1c0d, SkinOsu.M5351 and provider.M1dac are **nonvirtual**, even where the
caller uses `callvirt`. These are concrete calls, not unresolved interface slots.
E037 remains authoritative for the ordinary virtual sound-hook resolution.

# 2. What they found

## A. Independent string resolution

### Source and caller profile

**CONFIRMED STATIC:** M49b0 checks static dictionary F2e6e at0000–0010, otherwise
calls M49b1(identifier,true):0013. M49af creates that dictionary and prepares
F2e72/F2e74 using StackTrace/caller-declaring-type checks. M49b1 locks the same
dictionary, initializes a reader in F2e73 from the executing assembly's
GetManifestResourceStream:011e, and reads the record at a derived position.

The resource name is eleven Unicode characters, constructed by arithmetic and
StringBuilder.Append(char) at006d–0117, **not a filesystem path**:
`\u2006\u200a\u2002\u2004\u2002\u2002\u2002\u200a\u2006\u2006\u200b`.
Its175620 bytes have SHA256
`295345a25f546192080cfa5c3ce89865458560694735c35a89076e9865d690f1`.

The independently reproduced profile is an ordinary same-assembly managed caller
with a nonnull declaring type other than System.RuntimeMethodHandle. This gives
F2e74=16 and initial F2e72=−1876543868. Reflection/null-declaring-type/foreign
caller branches are retained in instruction evidence, not assumed equivalent.
No original StackTrace, original static constructor or original decoder ran.

M0bbf's guarded identity-key path reads assembly name/public-key metadata and
metadata tokens. M0bd5→M0bc8/M0bcf/M0bcd/M0bd3/M0bd1 combines tokens
T01a9/T01ab/T01ac/T01ad/T01ae/T01af using pure arithmetic M0bc4/5/6;
M0bc3 is MemberInfo.MetadataToken. The reduced token seed is `c06b3b32`.
For this unsigned assembly, identity input is UTF-16LE `osu!` followed by eight
bytes taken in M0bbf:00b4–0107's original interleaved token/seed order. Its
unchecked64-bit mixing at011c–016c gives `385151ffc1e8785e`.

M0bbe/M0bc9 initialize an identity cache through M0bcb(0). M0bca/M0bcb are the
guarded32-round encrypted cache get/set. E038 rederives the uncached identity
equation; it does not presume arbitrary corrupted/externally written cache state.
M0bc1/M0bc2 and GetCallingAssembly gates remain prerequisites of this profile.

**RECONSTRUCTED numeric reduction:** M49b1:02d7–03df consumes/resets F2e72,
xors low32(identity key) and constants, then uses nested iterator methods
M3414/M3425/M341c. Pre-generator key=135435612; first seven outer yields:

```text
135435613, 1400855005, 955100510, 903736794,
1535066414, 2045568693, -1777135343
```

The original signed32-bit arithmetic fold produces −325906065, signed remainder
2517035, and final **F2e6f=−900258630** (`ca5724ba`). F2e70=1607814 on this
profile. The extractor folds only whitelisted straight-line numeric regions and
acyclic metadata-token helpers; the checker independently uses reduced equations.
It is not a general CLR interpreter. All selected strings independently agree.

### Record decoding and cache

At M49b1:047f–04a9, position is the unchecked Int32 equation:

`offset = identifier XOR -351960377 XOR F2e6f XOR 1516081623`.

Initial shorts at025e–02b9 select a global key byte-array or a per-record key
length. Actual pinned header decodes to **no global key array, fixed key length3**.
Per-record key bytes are XORed with successive low bytes of F2e6f at0500–052f.
At0531–05de:

```text
encodedSize = ReadInt32LE() XOR offset XOR -973073496 XOR F2e6f
payloadLength = encodedSize & 0x0fffffff
```

An encodedSize−2 branch at0557–05a2 redirects through a four-byte identifier
permutation; M49b1 rechecks the string cache. The selected56 records do not use
that branch. Flags are byte-character0x40000000, compression0x80000000 and
no-cache0x20000000 (05f5–0626).

The payload transform at0628–06c1 uses its three decoded key bytes K:

```text
previous = byte((11+payloadLength) XOR (K[1]+7))
state = (K[0] | (K[2]<<8)) + (previous<<3)
for each ciphertext byte c in original order:
    on even index: state = u32(state*214013+2531011); word = u16(state>>16)
    r = low_byte(word); word >>=8
    plain = byte(K[1] XOR c XOR (3+previous) XOR r)
    previous = c
```

M49b1:06c3–0705 has an additional rotated-public-key-token XOR arm. The pinned
unsigned same-assembly profile skips it; no synthetic public key is substituted.
Compression, when flagged, obtains an output size from permuted first four bytes
and calls M49b5:0740 (literal/back-reference expansion). Byte-character records
map each byte directly to a Char:0752–077d; the other arm calls
Encoding.Unicode/GetString:0781–078d. This is **not UTF-8**.
All56 selected records are uncompressed byte-character records; the full other
branch relationships are recorded, but their general malformed-data/runtime
behavior is not claimed tested by these56 decodes. The extractor's eight-hop
redirect bound is an engineering extraction guard, not an original limit.

When allowed by no-cache flag and caller Boolean, String.Intern:0819 precedes
dictionary.Add:0828. At count6323 the decoder disposes its reader and clears
F2e73/F2e71/F2e6d, while retaining the string dictionary (0832–086c).
These fields are distinct from every audio-handle cache below. The original
caller-check anti-tamper branches can yield different results; this is not a
universal decoder for modified binaries or arbitrary reflective entry.

### Hitsound string provenance

Every selected identifier has offset, ciphertext length/hash, decoded-key hash,
flags and resulting string in JSON. Representative source callsites:

| Identifier | Result | Original caller / CIL | String-table offset |
|---:|---|---|---:|
|2064529391|hitnormal|M3b19:004f|10045|
|2064529407|hitfinish|M3b19:006e|10055|
|2064529359|hitwhistle|M3b19:008d|10065|
|2064529372|hitclap|M3b19:00ac|10076|
|2064615002|-|M3b14:007a|b1f0|
|2064528980|{0}{1}-hit{2}{3}|M3b36:00e5|101fe|
|2064631985|.wav|M3b14:0149 /M3b15:0094|ff1b|
|2064631948|.mp3|M3b14:015c /M3b15:00bd|ff26|
|2064631963|.ogg|M3b14:016a /M3b15:00aa|ff31|
|2064631958/2044/2058|wav /mp3 /ogg|M3b14:0243/0256/0264|ff3c /ff46 /ff50|
|2064593714|.|M3b14:027b|498|
|2064631972|taiko-|M3b14:009b /M3b36:00c2|ff0e|
|2064529283/9308|slider /$slider$|M3b15:0044/005a|10029 /10036|
|2064529892|Default|M4b35:0016|10e4e|
|2064453750|Skins|M4b21:000a|227dc|
|2064456412|LayeredHitSounds|M5353:0113|22d76|
|2064529910|SkinSamples|M0759:0bbd|10e5c|
|2064528048/8026|VolumeUniversal /VolumeEffect|M0759:00ed/0106|1051a /10530|

Offsets in this table are hexadecimal. No sample names were filled from enum
memory or community documentation. The decoder resolves the Taiko drum fallback
remappings too; ordinary Osu0 does not enter that branch.

## B. Standard sample preparation and cache layers

### Preparation and dynamic preloading

**CONFIRMED STATIC:** M3b08 creates F234e/F234f/F2350 dictionaries and
F2374[4,3,3], and initializes F2379 to original SampleSet values1/2/3 at00ab–00bd.
Before resource preparation those Int32 table cells have default0; this is not
the same thing as a completed missing-resource result−1.

Ordinary loading M28f5 first invokes virtual cleanup M28f6:0001; its base calls
M3b1d:0005 to free/clear the **map/custom** dictionary F234e. After object parsing,
M28f5 collects distinct timing.F0661 indexes>2 (017b–01a6), then registered
object.F1539 indexes>2 (01ce–01fd), in supplied iteration order. Its List.ForEach
uses M2914:0001→M3b18(index). No unproved sorting or fresh disk lookup at
judgement is inserted.

M3b18 writes F2360=index:0001, iterates F2379 while assigning F2361:0016 and
calls M3b14 for hitnormal/finish/whistle/clap plus the distinct slider families.
It prepares dictionary entries, **not extra F2374 planes**. Thus indexes>2 can be
available before M3b36's later dictionary-only lookup.

Next M28f5:023b calls M3b19. This iterates planes0/1/2, writes current selector
F2360:0024, iterates SampleSets1/2/3 assigning F2361:003d, and calls
M3b14(baseName,false,false,7). Store order is rows **0/2/1/3**, at005c/007b/009a/
00b9: Normal/Finish/Whistle/Clap. This preparation order happens to equal E037's
eventual request order, while numeric table row order remains0/1/2/3.
Further slider/misc preparation is not a claim about their sound triggers.

M3b19 changes the shared selectors for miscellaneous loads, resets F2360 and
related selector state at0281–0298, calls M3b1a, then restores its saved F2361
at02a4. M28f5 separately sets F2361=0 at0241 and has its version/status-dependent
M3b1f refresh. These are shared mutable preparation inputs, not object fields.

### Names and numeric mapping

For the ordinary `M3b14(name,false,false,7)` projection:

`B = parameterless_ToLower(F2361.ToString()) + "-" + name` (0060–00cb).

This gives `normal-hitnormal`, `soft-hitfinish`, etc. Parameterless String.ToLower
and String.Format are original calls; culture/provider is not explicitly passed.
Do not silently substitute an invariant-culture universal claim. For the normal
three enum names these ASCII results are also verified by the resource names.
The separate Play/Taiko prefix gate is F2c6d==2 and M2232()==1; it is not ordinary
Osu0. M3b14 can remove flag4 for that Taiko conversion based on selected-map mode.

E037 numeric map is preserved:

| Operand | Table coordinate |
|---|---|
|Normal1 /Whistle2 /Finish4 /Clap8|row0 /1 /2 /3|
|SampleSet Normal1 /Soft2 /Drum3|bank0 /1 /2|
|SampleSet None0|bank1 in M3b36; not an extra prepared bank|
|Other sample-set values|default bank0 in M3b36; dynamic key still uses actual enum ToString|
|Index clamped to0 /1 /2|plane0 /1 /2|

Original metadata calls planes Default0/Custom1/Custom2. Those labels **do not**
mean that every plane independently loads a correspondingly suffixed disk file.

### M3b14's ordered stages

At0000–0016 an external process-state gate
`F2c48 && !F2c49 && F2c4b` returns−1 before resource work. Its wider meaning is
not renamed. Flag operands select map4, skin2, embedded1 stages in that order.
The circle preparation call supplies7. Native load flags start131072 and add4
only for true loop argument; these circle loads pass false.

**Map stage (00ce–01fe):** require flag4, !F234a, nonnull selected global map,
and either unprefixed-name argument=true or F2360!=0. With ordinary argument=false:

| Plane/selector | Map stage | Map/custom cache key C |
|---|---|---|
|0|skipped|no map lookup, even if normal-hitnormal.wav exists|
|1|eligible|B (no suffix1)|
|2|eligible|B+"2"|
|n>2 via M3b18|eligible|B+original Int32.ToString(n)|

M3b14:0123–0131 checks **F234e[C]** first. A hit skips probing, including cached−1.
A miss tries **C.wav, C.mp3, C.ogg** (0140–01e7). For each extension:

1. Call currently selected map.M1c06(C+extension):017f.
2. If null and selected map's format F0fd1<5, also try **original unprefixed
   name+same extension**:0198–01a5 before moving to the next extension.
3. Nonnull bytes terminate probing: short bytes use SampleCreate; otherwise
   SampleLoad. The native return is opaque, including0 or−1.
4. Cache the resulting Int32, including−1, in F234e[C]:01f4.

A cached/loaded value other than−1 returns immediately:01f9–01fe, including0.
If−1, fall through to skin. No nearest-index search, generic path search or
automatically appended `1` exists here. F234a=true suppresses map lookup; later
skin/embedded stages can still execute. M2237:013a writes it false on the reached
ordinary setup arm, but other writers exist; it is not an immutable universalfalse.

**Skin stage (01ff–02c2):** require flag2, !M4b35() and
M177c(F03be)==true. M4b35 tests nonnull F2f16 and its F36a7 equals exact
decoded `Default`; M177c returnsfalse for a null setting receiver, otherwise its
stored Boolean. The decoded configuration key is **SkinSamples** (default true
at M0759:0bc2–0bc8, not a guarantee about the current setting).

Check **F2350[B]**, with no index suffix. On a miss, test File.Exists in order:

`Path.Combine(F2f16.M5351(), B+".wav")`, then `.mp3`, then `.ogg`
(023a–02af). M5351 returns
`Path.Combine(M4b21(), F36a7)`; M4b21 returns
`Path.Combine(M23b8(), "Skins")`. This establishes a loose skin filesystem path
under the supplied root/name, unlike an embedded ResourceManager key.
Load the first existing path through M31a4(offset0,length0,max8,flags):029e.
No byte-length threshold is applied to this file-loader branch.
Cache−1 or the returned handle in F2350[B]:02b8; return any non−1 result, including0.

A missing **file** falls through; a null skin object is different. If the setting
permits entry and F2f16 is null, M4b35 returnsfalse and the later M5351 dereference
can throw. There is no local catch granting an automatic embedded fallback for
every resource exception. Default-named skin skips this entire stage.

**Embedded stage (02c3–03d0):** if flag1 absent, return−1. Otherwise perform the
separate Taiko drum-key remapping when applicable; ordinary B is unchanged.
Check **F234f[B]**. On a miss call osu_ui.ResourcesStore.ResourceManager.
GetObject(B):036e–0379 and cast to Byte[]. If null/not Byte[], call
osu_gameplay.ResourcesStore.ResourceManager.GetObject(B):0382–0392.
No extension or index suffix is appended. These are managed resource keys,
not File.Exists paths. Culture/resource-manager/satellite behavior is external;
only the supplied base resource sets were inventoried.

If neither produces Byte[], cache/return−1:0396–03a7. Otherwise load bytes with
M31a2(offset0,length,max5,flags):03b0; explicitly normalize native0 to **−1**
at03bf–03c6, then cache/return via F234f. This normalization is **absent** from
the map and skin stages. The embedded branch also has no short-byte SampleCreate
substitution. M3b14 contains no local catch for resource-manager/native exceptions.

### Native loading and sentinel boundary

E037 already identifies the managed/native loaders. E038 preserves their exact
reached differences; it does not interpret native error values by remembered
BASS conventions:

| Source | Request | Return handling |
|---|---|---|
|Map/custom bytes length<1024|M3195 SampleCreate(0,44100,2,8,flags)|store opaque result|
|Map/custom bytes length>=1024|M31a2→M31a5 SampleLoad(memory=true,bytes,offset0,length,max8,flags)|store opaque result|
|Skin existing file|M31a4 SampleLoad-file wrapper(offset0,length0,max8,flags)|store opaque result|
|Embedded Byte[]|M31a2→M31a5(offset0,length,max5,flags)|0 becomes−1; other Int32 retained|

The file wrapper M31a4 explicitly ORs **0x80000000** into its flags at0002–0008,
then calls native M31a7(memory=false,path,offset,length,max,flags):0011,
also imported as bass.dll!BASS_SampleLoad. The memory wrapper M31a2 instead
passes memory=true to M31a5. Preserve that extra file-wrapper bit; its middleware
label is not needed to establish the original request arguments.

Nonnull bytes select a loader and terminate the extension loop even if that
loader returns−1. This is not “retry next codec after a native failure.” The map
or skin stage can then fall through to its next provider stage.

### Cache lifetime

F234e is map/custom; F2350 is skin; F234f is embedded. Each starts as a
Dictionary<String,Int32> using its parameterless constructor (M3b08:0006–001f).
No custom cache-key case comparer is supplied. Custom stems preserve original
case; filesystem/provider matching has its own semantics.

M3b16 evicts one supplied F234e key, freeing a non−1 previous value then removing
the key. M3b1c iterates a dictionary's values, passes each to SampleFree:0015
and clears the dictionary:0035. It does not filter−1/0 in that bulk loop.
M3b1d applies it only to F234e; M3b1e only F2350; M3b1b applies it to all three.
M28f6 uses map cleanup at fresh load; M4b2b:011f clears skin cache and, when
application mode2, reloads through M3b19:012c. Audio reinitialization M3b5e
contains all-cache cleanup:00d1 and later table reprepare:026f under its own gates.
Do not infer that every config mutation immediately clears every cache.

The dictionaries and F2374 are global mutable state. On cached−1, newly added
files are not automatically rediscovered by a normal cache hit. M3b15's force
operand and explicit eviction/reload are separate mechanisms. Cache aliases
between a custom filename stem and a standard map key can affect which candidate
is selected first; they are not two independent per-extension maps.
Multiple table planes can reuse the same skin/embedded cached handle. The36
cells therefore do not imply36 independent asset loads or36 separately owned
samples; the recovered cleanup iterates cache values rather than table cells.

## C. Explicit filenames and dynamic indexes

### Custom filename path

**CONFIRMED STATIC:** E037 M48df calls M24b9 after constructing/storing the circle
sample selectors. M24b9 accepts a nonempty filename only with nonnull
manager.F1800 and !F234a; otherwise F1534=−1. It does not establish a general
extension allowlist or safe-relative-path policy. Circle calls
`M3b15(name,false,false,false)`; slider-specific `slider`/`tick` tests are separate.

M3b15:001b–0039 tests **String.IndexOf('.')>0**, then strips using
Substring(0,LastIndexOf('.')). This operates on the whole supplied string,
not Path.GetFileNameWithoutExtension. A first dot at index0 is treated as the
no-extension arm. It performs no universal lowercase conversion.

Let original input be N, resulting stem S. The special third-argument slider
branch can prefix `$slider$` and remove native flag4; the ordinary circle's false
argument bypasses it. Thus its cache key is exactly **S**.

At0061–007b an existing F234e[S] returns immediately unless force argument4 is
true. Cached0 and−1 both return. On a miss/force:

* If the no-extension flag is true, or selectedMap.M1c0d(N) is false,
  probe **S.wav → S.ogg → S.mp3** at008f–0109. Each candidate uses M1c06;
  version<5 may additionally try **N+extension** after a null candidate.
* Otherwise read the **exact supplied N** via M1c06:0113. If null and version<5,
  the original code repeats the same N read:0132. It does not then enter the
  extension-probing loop. An existing but unreadable exact file is not proof
  that alternate extensions are tried.
* Nonnull bytes choose the same <1024/otherwise map-byte loaders at013b–0163.
  Null bytes leave result−1. No skin or embedded provider is consulted here.
* Recheck F234e[S]. A prior value!=−1 is SampleFree'd:0188, then cache the new
  result:0196. No0→−1 normalization is present.

Therefore custom `kick.wav` and `kick.ogg` share cache key `kick`. A cache hit can
precede checking whether the second exact filename exists. Custom lookup−1 makes
M24ba choose standard layers; custom lookup0 makes it choose the custom arm and
E037's player later rejects sample0. These are different outcomes. A valid custom
handle bypasses standard sound-bit layering and its standard-only volume floor.

### Dynamic standard keys

For clamped index>2, M3b36:00e0–012d uses decoded format
**`{0}{1}-hit{2}{3}`** with mode prefix, parameterless-lowercase sample-set enum
name, parameterless-lowercase sound enum name, boxed Int32 index. Ordinary prefix
is empty: examples `normal-hitnormal3`, `soft-hitfinish7`, `drum-hitclap10`.

M3b36's F234e.TryGetValue:01cc returns a found non−1 handle, including0. Missing
or−1 falls back to **F2374[same row,same bank,0]** at01db–0200. It does not call
M3b14/M3b15, probe files, find a neighboring index or add new planes at judgement.
M3b18 preloading supplies the map cache entries for known selectors>2 as above.
An artificially supplied None0 dynamic set formats `none-...`, while its table
fallback bank remains1; those are distinct operations, not a single enum coercion.

## D. Map/provider, skin/settings and available assets

### Selected-map bytes and paths

M3b14/M3b15 consult **global selected map M2c63/F1a0f**, with repeated reads.
E037's timing lookup uses **current object's manager.map F17ee**. Their equality
is a required fresh-chart integration assumption, not an immutable identity
established by these helpers. A retained/changed selection requires explicit state
or an unsupported outcome in a bounded reconstruction.

For loose maps, M1c05 sees no provider from M1bee and uses
**Path.Combine(map.M1bb8(), requestedName)** at0008–0014, File.Exists:0017 and
File.OpenRead:0023. M1bb8 returns Path.Combine(F19ff,map.F0fd6):0000–0010.
The configured map-root and map directory are external inputs. The original
helper does not impose the reconstruction's local-only media policy; a future
bounded local path policy is an explicit **ENGINEERING** admission choice.

With a provider, M1c05 first tests editor/alternate-Play local override M1c01;
if present opens Path.Combine(M1bbd(),name), otherwise provider.M1dac(name):004f.
M1c0d similarly chooses File.Exists for loose files, or provider.M1da7(name) OR
the editor override. M1bee includes `.osz2` acquisition/cache behavior, not a
universal assertion that each map is an ordinary directory. Provider.M1dac calls
M1dad(name,false); its stored-entry/stream/decryption internals remain a boundary.
E038 does not claim a full archive-provider implementation.

M1c05 catches its file/provider-opening exceptions and returns null after its
original reporting call. M1c06 acquires an existing provider with M1dbf(1,false),
returns null if acquisition fails, obtains the stream through M1c05, allocates
stream.Length bytes, and performs **one Stream.Read** at003a, discarding its
returned count. It closes/disposes the stream and releases provider M1dc0 in
finally paths. It is not a proven general read-until-EOF implementation; a partial
provider read can leave zero-filled remainder. Length/allocation/read/release
exceptions outside M1c05's catch are not turned universally into missing bytes.

### Layering and volume are process inputs

F36b0's original metadata name is **SkinOsu.LayeredHitSounds**. M534e initializes
it true:009b; M5353 selects decoded `General`:0006 and reads decoded
`LayeredHitSounds`:0113 through generic M531e<Boolean> using its address:0119.
M531e writes the supplied field only on successful section lookup/conversion;
the default is not proof of the current selected skin value. Section/key source
and complete skin loading remain external to the sample-file resolver.

Preserve E037 M44a2: initial mask0 always gains Normal1; a nonzero mask gains it
only if F2f16 exists, LayeredHitSounds is true, global selected map exists and its
M1bba mode!=3. No blanket `Normal always plays` rule is supported.

M0759 identifies F236e as **VolumeUniversal** (configured/default100,0–100) and
F236c as **VolumeEffect** (configured/default80,0–100), through M0767/M0768.
Current stored configuration can override the defaults. M3b40 still multiplies
the two integer reads before conversion/division; E037's player then combines
this with its per-request volume. Neither native gain nor the current user's
installed settings is measured by decoding those names.

### Reference archive resources

Only18 archive members exist; no loose map, skin directory or individual audio
sample files are supplied. That does **not** mean the standard sounds are absent:

| Store | Member SHA256 | Base resource entries | These12 keys |
|---|---|---:|---|
|osu!ui.dll|22e041674931161e2c2c15241fb6c5f3125f8d57e4660ef7c0b7fb64449253c7|727|absent|
|osu!gameplay.dll|ba3467a8db908d81a0729f78fdc5c8f1d1595d3da4e5a9a34be9a16e06da9f87|456|all present as System.ByteArray|

| Bank | hitnormal bytes | hitwhistle bytes | hitfinish bytes | hitclap bytes |
|---|---:|---:|---:|---:|
|normal|201380|213964|300336|18368|
|soft|145508|145548|402392|19476|
|drum|145508|70492|115100|115528|

JSON fingerprints each entry without copying bytes. No audio format/quality,
filename extension, native loadability or audibility is inferred merely from
these Byte[] names/sizes. Original embedded resource absence for a particular
custom/indexed key is not generalized into a missing standard fallback.
Map-local custom assets and a non-default external skin are not supplied by this
reference archive; the actual owner's installed resource state is unverified.

## E. Conditional reference controls

All K01–K42 are **RECONSTRUCTED STATIC PREDICTION — original runtime NOT RUN**.
Unless varied: fresh ordinary Osu0 context, exact matching manager/global map,
positive admitted circle, stable normally returning helpers/observer, F234a=false,
early process gate false, resource-preparation flag7 and named native return
operands. Caches initially empty; native H means an explicitly supplied nonzero,
non−1 handle. Predicting a managed resource/request choice never predicts sound.

| ID | Initial state / resource operands | Lookup sequence and conditional result / evidence |
|---|---|---|
|K01|Stored mask0; bankNormal1,index0; default-named skin|Normal added; map skipped; UI key normal-hitnormal then gameplay key; embedded loader result normalized if0 /A,B,E037|
|K02|Whistle2,augmentationfalse,bankSoft2,index1|Map soft-hitwhistle.wav→.mp3→.ogg until nonnull; standard Whistle request only /B,E037|
|K03|Finish4,augmentationfalse,bankDrum3,index1|Map drum-hitfinish candidates; Finish only /B,E037|
|K04|Clap8,augmentationfalse,bankNormal1,index1|Map normal-hitclap candidates; Clap only /B,E037|
|K05|Mask14,augmentationtrue|Normal→Finish→Whistle→Clap; four independently selected handles /B,D,E037|
|K06|Same14,augmentationfalse|Finish→Whistle→Clap; no Normal request /D,E037|
|K07|Any layer; bankNormal1|row for bit,bank0; key prefix normal- /B|
|K08|Same; bankSoft2|bank1; key prefix soft- /B|
|K09|Same; bankDrum3|bank2; key prefix drum- /B|
|K10|Index0; map has normal-hitnormal.wav|Map is still skipped; skin/embedded supplies plane0 /M3b14:00e1–00ef|
|K11|Index1|Map cache/candidates use unsuffixed B, never B1 /M3b14:0105–011c|
|K12|Index2|Map uses B2; skin/embedded still unsuffixed B /same|
|K13|Index7 present on timing/object; map B7.wav returns bytes/loadH|M28f5→M3b18 preloads F234e[B7]=H; dynamic M3b36 returnsH /B,C|
|K14|Dynamic B7 missing or cached−1|M3b36 returns same row/bank plane0, with no new file probe /C|
|K15|Dynamic B7 cached0|M3b36 returns0; E037 request can be queued but player rejects sample0 /C,E037|
|K16|Custom kick.wav, accepting setter,exact file exists,bytes/loadH|Cache stemkick checked first; exact kick.wav read; F1534=H; custom arm only /C|
|K17|Filename empty OR manager.F1800null OR F234atrue|Setter stores−1; no M3b15 call; standard layers remain /E037,M24b9|
|K18|Custom stem miss; all candidate readsnull|wav→ogg→mp3, cache−1; M24ba standard layers /C|
|K19|Custom load returns0|Store0; bypass standard; E037 worker rejects sample0 /C|
|K20|Custom no-extension kick; map has all three formats|kick.wav first; no ogg/mp3 probe after nonnull bytes /M3b15:008f–0109|
|K21|Exact custom kick.ogg absent; stem candidateswavnull,oggbytes|kick.wav then kick.ogg; H selected before mp3 /C|
|K22|Exact kick.ogg exists but readnull; version>=5; kick.wav available|Exact branch does not enter probing; cache−1→standard arm /M3b15:010d–0138|
|K23|Artificial custom .kick; a same-named file exists|First dot index0 chooses probing .kick.wav→.ogg→.mp3, not exact .kick /001b–0039|
|K24|Custom kick.wav cachedH; next inputkick.ogg|Same stemkick hit returnsH before exact-existence check /0061–007b|
|K25|Custom cached−1; file later supplied; forcefalse|Return cached−1 without new I/O; explicit force/eviction needed /same|
|K26|Artificial direct M3b15 force=true; old valueH, new lookupH2|Lookup continues; oldH freed; stem storesH2 /0171–019b|
|K27|Standard B.wav and B.mp3 both nonnull|B.wav wins; ifnull,mp3 wins beforeogg /M3b14:0140–01e7|
|K28|Version4; C.wavnull,unprefixed hitnormal.wavbytes|Unprefixed .wav wins before C.mp3; version5 skips that fallback /0188–01aa|
|K29|Map stage missing/cached−1; nondefault skin,settingtrue, B.mp3 exists|Skin B.wav File.Existsfalse then B.mp3 true; file loader selected /01ff–02c2|
|K30|Same but SkinSamplesfalse or nameexactDefault|Skin skipped; embedded stage follows /same|
|K31|Nonnull skin; all three filesmissing; UI keynull,gameplay Byte[]present|Embedded gameplay bytes loaded max5 /036e–03d0|
|K32|Artificial both embedded stores returnnull/non-Byte[]|F234f[B]=−1; no prepared native sample from that stage /0393–03a7|
|K33|Artificial embedded loader returns0|Normalize−1 and cache it /03bf–03c9|
|K34|Map or skin loader returns0|Store/return0, do not continue to next provider; later playback may reject /01f9,02bd|
|K35|Nonnull map bytes but native loader−1; later extensionsavailable|No next-extension retry; cache−1 then skin/embedded /01ac–01fe|
|K36|Standard F234e[C]=−1 or F2350[B]=−1|Skip that provider's probing; fall through to next admitted stage /cache-hit branches|
|K37|Artificial F2f16null, SkinSamplestrue, mapstagefails|Skin guard does not reject null; M5351 dereference can throw before embedded /M4b35,M3b14:0270|
|K38|Provider.acquirefalse or map.M1c05returnsnull|M1c06 returnsnull; caller may continue candidates/providers under its own guards /D|
|K39|Loose map rootR, directoryD, candidateN; no provider|File request Path.Combine(Path.Combine(R,D),N); no embedded-key extension is assumed /D|
|K40|Custom standard-looking filename populated F234e[B] before tableprepare|Plane1 standard map cache can reuse that stem's handle before extension loop /B,C|
|K41|LayeredHitSounds/currentroot/provider/currentsetting not supplied|Retain conditional branches/explicit unsupported result; no universal defaults /D|
|K42|Fresh load versus skin refresh versus audio reinit|Map-only /skin-only /all-cache cleanup at stated calls; no inferred per-hit invalidation /B|

Artificial invalid/null/handle controls are not natural gameplay measurements.
K01's Normal names the sound layer, not a universal default sample set; zero
object selectors inherit timing, and E037's long-row timing set0 becomes Soft2.
The JSON also contains36 finite naming controls for four rows×three banks×three
planes; they are deterministic key equations, not36 audio or gameplay passes.

## F. Bounded implementation-readiness handoff

**Evidence-derived operations available for a future bounded R11:**

1. Preserve raw hitSound column5 plus circle payload
   normalSet:additionSet:index:volume:filename. E037 records conversion/default/
   clamping branches; retain parsed versus constructed sound bits2/4/8 and the
   separate custom-handle sentinel. Current CircleDefinition discards these
   fields after syntax validation, so it cannot yet supply this interface.
2. Preserve ordered timing time/sample-set/index/volume, including inherited
   entries, shifted time/version and E037 M1be6's last-qualifying scan/first fallback.
   Current TimingPoint(Time,BeatLength,IsBase) alone is insufficient.
3. Supply explicit global selected-map identity, loose map root/directory or
   provider adapter, format version, map-sample suppression, process gates,
   skin identity/name/root, SkinSamples, LayeredHitSounds, culture and current
   VolumeEffect/VolumeUniversal. A fresh chart does not contain all these values.
4. At preparation, apply accepting custom setters, clear the map cache at the
   established loading boundary, preload unique indexes>2, then fill the36 table
   cells in original order through the three admitted provider stages. Preserve
   shared cache keys and−1/0/native-handle distinctions. Do not load only those
   layers encountered by the first judgement and assume equivalent cache history.
5. At a positive ordinary circle hook, select E037 start+5 timing operands and
   mask, invoke its observer, then choose custom or standard handles. Standard
   indexes>2 consult prepared dictionary then plane0; no judgement-time probing.
   Request order/volumes/pan/rate remain E037, with no scoring/HP events introduced.
6. Resource resolution ends at returned Int32 **prepared sample handle**.
   M3b31(handle,requestVolume,pan,true,1f) is the previously recovered dispatch
   boundary. E038 adds no scheduler/channel-playback reconstruction.

**ENGINEERING interface:** an independent asset provider may return immutable
owned sample data/resource IDs, and a recording/playback sink may map those to
its own handles. Declare this substitution and its failure semantics explicitly;
do not pretend its handles/error codes/threads/codecs are BASS. A portable local
file admission rule and explicit unsupported archive/skin branches are legitimate
bounded policies, not recovered original rejections. The existing WPF song
MediaPlayer/Stopwatch is unchanged and is not proposed for replacement.

Headless checks can exercise decoded naming, candidate order, version4/5
fallbacks, map/skin/embedded gates, cache aliases/invalidation, table/index
selection and request fields using independent injected providers and opaque
loader results. They cannot verify native loadability, overlap, latency or audio
output. No such reconstruction tests were implemented here.

Largest supported subset: fresh ordinary circles, known sample sets1/2/3,
well-formed E037 sample metadata, explicit matching map identities, local loose
files, supplied nondefault skin files or independently provided default resources,
and explicit settings/culture/cache lifecycle. Retain unsupported outcomes for
unrecovered archive-provider internals, selection changes, opaque global gates,
malformed payload/runtime conversion edges, unavailable external assets or
unrepresented native loader behavior. Do not manufacture coefficients, samples
or success defaults to fill these gaps.

Remaining **UNVERIFIED**: full `.osz2` entry decryption/partial-stream behavior,
complete skin configuration/import/culture/provider selection, external mutation
and concurrency, exact current settings/assets, native sample load/free outcomes,
driver/sink errors, queued timing and audible output. Original-runtime tests are
not authorised by E038. The bounded resource contract is defensible; universal
skin/sample-pack or gameplay equivalence is not established.

# 3. Pseudocode

**RECONSTRUCTED from CONFIRMED STATIC instructions.** Original opaque gates,
repeated map reads, load returns, exceptions and cache lifetime remain inputs.
This is explanatory pseudocode, not an executable reconstruction.

```text
prepare fresh ordinary chart:
    original loading cleanup -> free/clear map/custom cache F234e
    parse objects; accepting filename setters may already populate F234e
    collect unique timing.index>2, then registered object.index>2
    for each: M3b18(index) -> prepare standard map keys for sample sets1/2/3
    for plane in [0,1,2]:
        F2360=plane
        for set in [Normal1,Soft2,Drum3]:
            F2361=set
            for (row,name) in [(0,hitnormal),(2,hitfinish),(1,hitwhistle),(3,hitclap)]:
                F2374[row,bank,plane] = M3b14(name,false,false,7)
    retain original miscellaneous loads, selector resets/restoration

M3b14(name,loop,unprefixed,flags):
    if original early process gate: return -1
    B = original optional sample-set/mode prefix + name
    if original map-stage gates pass:
        C = B + original index suffix (none for1; map stage skipped for0)
        if F234e has C: h = cached value
        else:
            h = -1
            for extension in [.wav,.mp3,.ogg]:
                bytes = freshly selected map.M1c06(C+extension)
                if bytes is null and freshly selected map.version<5:
                    bytes = freshly selected map.M1c06(name+extension)
                if bytes is not null:
                    h = original short-byte/create OR memory-load result
                    break                           // even when native result -1
            F234e[C] = h
        if h!=-1: return h                          // zero included
    if original skin-stage gates pass:
        if F2350 has B: h = cached value
        else:
            h = first existing skin B.wav /B.mp3 /B.ogg -> file-load result, else -1
            F2350[B] = h
        if h!=-1: return h                          // zero included
    if embedded flag absent: return -1
    apply separate original Taiko remapping if reached
    if F234f has B: return cached value
    bytes = UI.GetObject(B) as Byte[]
    if bytes is null: bytes = Gameplay.GetObject(B) as Byte[]
    if bytes is null: h = -1
    else:
        h = original memory-load result(max5)
        if h==0: h=-1                              // embedded-specific
    F234f[B]=h; return h

M3b15(customName,false,false,false) [ordinary projection]:
    S = substring before last dot only if first dot index>0; otherwise customName
    if F234e has S: return cached value             // even -1 or0
    if no-extension arm OR !selectedMap.M1c0d(customName):
        bytes = first nonnull map read S.wav /S.ogg /S.mp3
        preserve each version<5 customName+extension retry
    else:
        bytes = map read exact customName
        preserve version<5 same-name retry, without extension-loop fallback
    h = bytes is null ? -1 : original byte-loader result
    free existing cached S if present and value != -1
    F234e[S]=h; return h

on original positive ordinary circle sound hook:
    E037 timing at stored start+5 -> selectors/mask -> observer
    if object.customHandle!=-1:
        E037 M3b31(reloaded customHandle,effectiveVolume,pan,true,1f)
    else:
        for selected layer in [Normal,Finish,Whistle,Clap]:
            if index<=2: h = prepared F2374[row,bank,index]
            else:
                key = original format "{0}{1}-hit{2}{3}"
                h = F234e[key] if found and value!=-1 else F2374[row,bank,0]
            E037 M3b31(h,original layer volume,pan,true,1f)
    // Original dispatch/player/queue/native outcome remains E037 + runtime inputs.
```

No audio assets, original runtime observations or reconstruction changes are
delivered. Stop after E038; no E039, R11 or other milestone begins.
