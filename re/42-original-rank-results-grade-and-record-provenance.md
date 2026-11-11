# 1. Result

Expedition 042, Work 1, 2026-11-11 Asia/Bangkok. Original-static research on
canonical main `47277a8da8603af93b0c515f86aac2a1463ef51c`, the R14-Q01 publication.
Recovered the ordinary Rank construction and record-selection path, the base
record's complete final-grade decision, result-field presentation inputs,
resource identifiers, reveal controls and bounded departure/disposal behavior.
This supports a narrow original-derived R15 results model with an engineering
WPF presentation. The complete native frontend remains outside that assessment.

Verified the actual inputs before decoding original instructions.

| Input | Bytes | SHA256 |
|---|---:|---|
| `osubackup.zip` |78546961|`7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`|
| ZIP member `osu!.exe` |4541728|`6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`|

Stable40/file-version1.3.3.8 remain the executable's self-description. Its
official release date, authenticity and modification history are unauthenticated.
Windows host, Python3.12 and the existing dnfile0.18.0/dncil1.0.2/pefile2024.8.26
readers were used solely for metadata, CIL and opaque resource metadata.

The five deliverables are this report, `evidence/42/rank-results.json`,
`tools/rank_results_evidence.py`, `tools/check_rank_results.py` and the append-only
E042 journal entry. Exact final publication identity is supplied in the verified
handoff. It is also obtainable from the last canonical commit affecting this
report. The tested research baseline is the full SHA above. No original process
or reconstruction production component was executed during this expedition.

## Procedure and verification

Preparation used the standing contract and directed RTK instructions, both project
journals, E002/E003/E019/E023/E027/E033/E034, relevant spinner and health/NewCombo
findings, R08/C02/R09/C01/R10/R14/Q01, QA06–QA08/QA12 and current ledger/session/
Windows host/overlay interfaces. E005's ordinary osu!standard versus Taiko
correction remains authoritative. Historical records and fixture bytes are retained.

The evidence selects 112 MethodDefs, including abstract M3c55/M31f5, with exact original
names, signatures, owners, flags, RVAs, code/header/body sizes and body SHA256.
It records 430 referenced or explicitly watched FieldDefs, 3064 direct accesses
to 69 watched fields and 126 direct references to the selected target methods.
The full direct inventory decodes 19659 managed bodies, encounters 1739 zero-RVA
entries and reports zero decoding errors. Inventory coverage excludes reflection,
native/external writes and unprovided code. A method inventory is separate from
proof that a particular original play reaches it.

The independent checker pins literal branch/call/field relationships, rereads
original bytes separately, reconstructs token/constant/branch inventories,
checks dispatch candidates and enum constants, and verifies source/authority
hashes. It independently decodes 58 string identifiers with the reduced E038
key/byte transform. It checks opaque serialized-entry spans for relevant resource
keys without deserializing or publishing an image. A small Python instruction
walk checks 26 grade controls under the explicitly stated binary32 projection.
It substitutes stable wrapper/count operands and never invokes original CLR code.

The first archive audit stopped at the new checker's assumption that every selected
string used the byte-character flag. Identifier2064569235 is an uncompressed
36-byte UTF16LE entry for the optional online-child label. E038's original Unicode
branch independently justifies the corrected decoding. The packet retains the
initial checker SHA256, incorrect expectation, actual flag/offset/cipher hash and
correction. No historical assertion or original instruction finding changed.

Reproduce from the repository root with the established reader dependencies on
PYTHONPATH. Output destinations should be separate private files for comparison.

```text
python re/tools/rank_results_evidence.py /private/osubackup.zip /private/rank-a.json
python re/tools/rank_results_evidence.py /private/osubackup.zip /private/rank-b.json
python re/tools/check_rank_results.py /private/rank-a.json /private/osubackup.zip
```

Final deterministic extraction, independent archive audit, historical checker
outcomes, preservation review and publication verification are recorded in the
append-only E042 journal entry. Static checks establish byte/relationship
consistency. All 78 original reference cases remain **RECONSTRUCTED STATIC
PREDICTIONS**. The 1630 historical independent QA checks were not rerun here.
E020's 17 original-runtime cases remain NOT RUN. No contract exception was used.

Actual final execution passed the independent archive audit for112 method
identities/110 managed bodies,430 field declarations, all watched field/caller
inventories,26 grade projections,58 independent string decodes and116 shared
historical body comparisons. Two fresh extraction processes produced identical
1220453-byte JSON, SHA256
`484881b5ce25f251208c78f0120810d72a326bca657cf6873bd972f12b4ff911`.
All12 unchanged historical checkers passed for E002/E003/E019/E023/E025/E026/
E027/E033/E034/E035/E036/E038. E038 additionally checked its56 strings against
the archive. Both new Python tools compiled in memory. These outcomes are static
verification only. Original gameplay, rendering and reconstruction QA were unexecuted.

The final collector SHA256 is
`6fefa97be85ec05b215d72ec49f632b2894b30f55b8abc4863cc9fd77157af90`.
The final independent checker SHA256 is
`d2c97787c0f30cf6cbc0e5a1257312ddedcbe62132015853bb1a99bece16ec2f`.

# 2. What they found

## A. Confidence, identities and stage boundaries

**CONFIRMED STATIC** denotes decoded instructions or original metadata.
**RECONSTRUCTED** denotes the conditional interpretation below. **ENGINEERING**
identifies future reconstruction choices. **UNVERIFIED ORIGINAL-RUNTIME** covers
actual original reachability, JIT precision, callbacks, installed resources,
pixels, GPU/layout/animation and physical input. E042 supplies no new gameplay
**VERIFIED COMPONENT** claim.

Mxxxx/Fxxxx/Txxxx abbreviate MethodDef0600xxxx, FieldDef0400xxxx and TypeDef0200xxxx
in this exact executable. All report offsets are hexadecimal from the first CIL
opcode, excluding headers. JSON offsets are decimal. Original obfuscated names
and raw signature blobs remain in JSON alongside these stable token aliases.

| Method | Declaring type / signature | Relevant boundary |
|---|---|---|
| M32d0 |T06a8 / `20010112a304`|Rank constructor, instance void(host T08c1)|
| M32db |T06a8 / `200001`|Virtual initialization override of M1ab1|
| M32dd / M32de |T06a8 / `200001` / `20010102`|Record selection / ordinary record preparation|
| M32d7 |T06a8 / `200201129678151188010108`|Prepare widgets from T059e and nullable Int32 score override|
| M32da |T06a8 / `20010108`|Fresh grade reads and grade sprites|
| M32d4 / M32d5 |T06a8 / `200001`|Update/reveal / draw override|
| M32d3 |T06a8 / `20010102`|Virtual disposal override|
| M2b47 |T059e / `2000118904`|Virtual grade getter returning T0241|
| M2b42 / M2b44 / M2b4b |T059e / `20000c` / `200008` / `200008`|Accuracy / whole-result total / numeric score|

T06a8's exact inheritance is T06a8 → T05b4 → T08fb → T0397 → System.Object.
Its constructor initializes two lists and a ten-element T092e array, then calls
M2bf2 at0025. That base constructs F19d6's container, selects the supplied host
or global host when null, then calls M4415 → M1aae, which stores F0f49. The Rank
constructor has no completed-record read or results formatting. Its single
declared constructor differs from later virtual activation.

E003's M46ad request guards remain intact. Accepted Rank7,false writes pending
F2c6c at022a and phase0 later. It carries no record argument. A deferred M46e6
invocation replays the whole guarded request using live globals. Admission does
not itself commit current mode or construct Rank.

On a reached M46b0 commit, pendingUnknown10 returns0009. Otherwise the relevant
ordering is previous F2c70 at0068, current F2c6d at0072, then old-object cleanup.
For cleanup argument<=0, the old object is removed and Dispose/M1ab0 is invoked
at00ca. For argument>0, M46df is queued using a captured old-object reference.
The active field is cleared00d0 in either path. Rank case7 constructs M32d0 at
04ba and assigns active at04bf. Common registration
Add occurs055a, followed by virtual M1ab1 at0564, resolving T06a8.M32db through
the recorded name/signature/ancestry/reuse-slot chain. Pending is cleared0577
only if it still equals current. A new request made during initialization survives
the mismatch branch0573.

Thus Rank requested, accepted pending, committed current, constructed, initialized
and drawn are six distinct states. Exceptions in prefix/disposal/registration/
initialization preserve preceding mutations. M46b0 has no transaction covering them.
Queued old cleanup permits later execution relative to Rank initialization.

## B. Completed-record provenance and old Play disposal

Global Play.F13c3 and handler T0186.F05f3 both have signature `06129678`, a T059e
reference. M0ace reads the global at0001, stores the same reference into handler
at0006, then invokes virtual reset M2b62 at0012. E023/E033 establish fresh ordinary
selection of base T059e through M2e1c:0047 → M2b3e, subject to the original mode,
source/modifier and optional-accumulator gates. Alternative T0934/other rulesets
retain different getters and remain outside the grade contract below.

M0acd first sets Play.F1397 at0007. It writes handlerRecord.F1978 from NOT global
failed F136a at001a, then F197c from the original signed difference/comparison
against handler.F05fd and record.F1975 at003d. Preserve the unchecked subtraction
and original fresh reference reads. Count-validation mismatch in E027 calls
M2b5a, whose0002 store sets F1990=false. That invalidation does not veto
finalization or replace the handler record.

The watched-field inventory also connects F05fd to E035's unchanged M1150.
It resets the scalar at00c1 for a trial, adds `a+b−1` for a slider at02a8, where
a=subset.Count+2 and b=mixedSchedule.Count−subset.Count−1, then adds1 per completed
object at03db. A fully completed admitted ordinary trial therefore supplies the
finite object-count-plus-slider-schedule-count scalar, with the original Int32
operation order retained. Partial retries can leave partial counts until reset.
F197c's consumer compares this prepared scalar with record maximum combo.
This provenance supports the conditional ranking-perfect decoration. It supplies
no new HP arithmetic, numeric-score multiplier or universal full-combo formula.

E027's exit selector M2285 reads the global record's virtual grade. F8 in the
ordinary non-special path requests SelectPlay5,false after invalidation. A non-F
read can reach ordinary M2286 → M46ad(Rank7,false). Handler completion and this
global read can use different records after restoration/replacement or under an
explicit artificial mismatch. No reference reconciliation is added here.

Old ordinary Play disposal M2266 matters between the selector and Rank activation.
At00cc it invokes virtual M2274, whose ordinary implementation freshly returns
global F13c3, then reassigns global00d1. This is normally a self-assignment. At00e3
it invokes M2267. The relevant normally returning prefix/suffix writes include
global record.F1991 from manager state0045, F198d from F137a005b and UTC date
F196f006a when M4390 is false. A special F14b3/F14af branch can clear F1978 at0030.
Later, if all intervening original eligibility branches are reached and F136b>0,
M2267 clears F1978 at02fb. F1402=false instead reaches the original M22aa and
Environment.Exit(-1) prefix, so return into Rank is not guaranteed.

M22a1 and M22a2 gate parts of that suffix with record validity and other original
live state. Their F1990 reads are0043 and0041 respectively. F1990=false can divert
M2267 to its invalidation suffix0324. E042 does not import that eligibility into
the grade formula or pretend the whole cleanup always follows its successful arm.
Persistence/background operations reached by M2267/M2b5c are recorded boundaries.
No online-submission behavior is reconstructed.

**RECONSTRUCTED conditional consequence** follows from the ordering. An earlier
non-F selector result need not equal Rank's later grade. A reached cleanup can
change F1978 or other external state before the later getter. This is a managed
state control with explicit prerequisites, not an observed normal-play F screen.
Positive cleanup arguments additionally leave scheduling-dependent ordering.

### Which reference Rank selects

M32db checks global F13c3 then source F2a7f. Both null cause
M46ae(Menu0,true,0) at0011 and return0016. It allocates F1daa's container at001e,
then invokes virtual M32dd at0024.

If that virtual selection returns normally with F1da9 still null, the002f branch
reaches another queued Menu request0034 and returns0039. This guard occurs after
the selection helper, so it does not protect a prior M32de null dereference.

M32dd captures M4390/F2a7c into F1da5. For the scoped fresh ordinary false branch,
it invokes M32de(true) at0124. M32de copies the then-current global reference into
Rank.F1da9 at0006 and reads its F1978 at0011. No new completed record or value copy
is constructed. Replacing global between request and this read changes the
selected record. The source-only alternative can pass M32db's initial check with
global null and still throw at M32de's dereference. There is no universal safe
empty-record fallback.

For the source/recorded true branch, F2a80 plus a nonnull global selects global
at002a. Otherwise F2a7f is selected0060, optional result-history data is copied,
a matching saved record may replace F1da9 at00ee, and M2b64 can restore replay/
graph payloads. These branches are retained in the evidence and excluded from
fresh ordinary R15 support. A null player name can throw at M32dd:003a rather
than behaving as an empty name.

M32de's successful non-recorded branch excludes record mod bits128/8192, compares
record score with the selected map's prior record, and may call its grade setter
M1beb. It eventually constructs optional T02bc/M14b1 at0162 when its Boolean is
true, or calls M1762(record,false,true) at0175 when false. M14b1 retains the same
record in F0c30 and reaches persistence/optional external work. This child is a
separate results extension. A minimal display can leave it unsupported explicitly.

After selection M32db checks the current selected map and record hash F1972.
On absence/hash mismatch it calls M2c67(recordHash) at007a, conditionally selects
the resolved map via M2c64 at0084, then rereads M2c63. A still-null map requests
Menu0,true,0 at0093 and returns0098. Record preparation precedes these map checks,
so an earlier map dereference/helper can already throw. The method does not roll
back preceding record/container effects.

Complete direct F1da9 stores are M1f3b:0006, M32d7:007d, M32dd:002a/0060/00ee and
M32de:0006. M1f3b belongs to a Rank-derived alternative, outside the ordinary
M32d0 construction. M32d7 can rebind F1da9 to its passed record after clearing
its previous widget lists. Passing null returns before that cleanup.

## C. Exact ordinary final grade

T0241's original enum names and values are XH0, SH1, X2, S3, A4, B5, C6, D7,
F8 and N9. These are metadata names. Calling X “SS” in a future interface would
be a presentation choice. The base M2b47 returns0–8 through its recovered branches.
N9 is an enum sentinel used elsewhere, including selected-map stored-grade logic.

M2b47 body SHA256 is
`612f291e51c2668a73fbc0a9425e28f9b00d2bc4475322c25bd18ec2f103b267`.
Its input proportions are computed before the success flag is tested.

| Original sites | Operation |
|---|---|
|0001/0006/0008/000d/000e/000f|UInt16 n300 F196a, conv.r4, virtual M2b44 total, conv.r4, divide, store Single local0|
|0011/0016/0018/001d/001e/001f|UInt16 n50 F196b, independent total call, analogous Single local1|
|0021/0026/0028|False F1978 returns F8 after those two divisions|
|002b/0030|p300==Single1 enters X/XH branch|
|0068/0071,0075/007e,0081/0086|p300>.9 Double threshold, p50<=.01 Double threshold, MISS==0 enters S/SH|
|00be–00de|A if `(p300>.8 && MISS==0) || p300>.9`|
|00e2–0102|B if `(p300>.7 && MISS==0) || p300>.8`|
|0106–0113|C if p300>.6, otherwise D|

M2b44 adds F196b+F1969+F196a+F196e with Int32 adds. The two calls are separate
virtual reads. With stable UInt16 fields the total is finite0..262140. Whole
MISS affects the denominator as well as the explicit MISS predicates. Decorated
F196c/F196d are excluded from this base total.

In the X and S branches, original generic conversion of F1970 is followed by
signed `(bits &8)>0`, then conditionally a fresh conversion for `(bits &1024)>0`.
Either selects XH0 or SH1. Metadata names those bits Hidden and Flashlight.
Other modifiers do not directly select silver letters in this getter. Their
effects on record production, success, policy or alternate record type remain
separate and mostly outside the no-mod scope.

The threshold constants are Double .9/.01/.8/.7/.6 after each stored Single
proportion is promoted with conv.r8. There is no percentage rounding before
grading. Under the declared binary32 projection, 90/100 stores approximately
.899999976, 80/100 stores .8000000119, 70/100 stores .699999988 and 60/100 stores
.6000000238. Consequently 80 300s/20 100s reaches A, 70/30 reaches C and 60/40
also reaches C. These are explicit precision discriminators. The original x86
JIT's extended-intermediate behavior at marginal boundaries remains unmeasured.
A future .NET implementation should document its numeric projection and keep
those original-runtime limits visible.

The `ble.un` and `bgt.un` branches preserve unordered behavior. With an empty,
stable base record the divisions are floating0/0. When F1978 is true, NaN fails
the positive grade thresholds and reaches D7 under the projection. When F1978
is false, F8 returns earlier. M2b42 independently returns1f for total<=0, so an
empty record can have 100% accuracy and D or F. This does not establish an
accepted empty-chart play.

There is no numeric-score, accuracy-getter, current/max combo, F197c or F1990
read in this grade body. It writes no grade cache. Calls made twice can return
different grades after mutation. Concrete alternative record getters are listed
by matching original names/signatures and remain excluded from base ordinary
interpretation.

## D. Rank statistics, formatting and capture boundaries

M32db prepares the viewport and containers, sets F1dae to a fresh host/UI clock
F2c3a+300 at0183, registers its keyboard handler, constructs background/title,
invokes map/player header preparation M32df, then passes F1da9 and an absent
nullable score to M32d7 at0352. A preceding score-format write02ce is superseded
by the reached M32d7 branch. Its ordinary mode0 path is preserved separately
from Taiko/Catch/Mania switch arms.

Record selection occurs before M32db's Monitor-protected M4391(false) call0159.
M4391 stores F2a7c=false and clears source F2a7f and F2a9f when false. M32db then
clears F2a80/F2a81 at015f/0165. Its local F1da9 and captured F1da5 still retain
the earlier selection/provenance. After preparing the widgets it calls base
M2bf3 at0358, which reaches M46b1 → M46b2. M46b2 sets transition phase1 at0017,
with a preceding progress140 store when the prior phase differs from2. This
establishes an incoming transition request in the reached initialization chain.
It supplies no observed fade timing or guarantee that a first draw occurs.

| Result field | Original Rank source | Presentation preparation |
|---|---|---|
|Numeric score|Passed record virtual M2b4b at M32d7:0013 unless nullable override supplied|Format at00c1, captured as F1daf at00c6|
|Accuracy|Passed record virtual M2b42 at09bc|Multiply100f at09c6, format09cc, pass to M0fb7 at09e0|
|300 /100 /50|Passed record F196a/F1969/F196b at01c0/0206/024c|Format `{0}x`, ordinary count widgets|
|Additional decorated counts|F196c/F196d at0292/02d8|Separate hit300g/hit100k widgets|
|MISS|F196e at031e|hit0 widget, also `{0}x`|
|Maximum combo|F1975 at092a|`{0}x` passed to M0fb7 at0948|
|Current combo / snapshot|No corresponding ordinary Rank statistic read|Maximum field is the established displayed combo operand|
|Grade|F1da9 virtual M2b47 in M32da:0011/00fe/0119|Fresh enum.ToString for resource key|
|Completion/perfect decoration|Passed record F197c at0b29|Conditional ranking-perfect widget|
|Validity|No F1990 read in T06a8's direct methods|Upstream eligibility/record validity remains separate|
|Modifiers|F1da9.F1970 in M32d9|Original enum scan, conditional mod icons|
|Map header|Fresh M2c63().F0fe2 in M32df:0005|Text constructor M0926|
|Map author|Fresh selected map.F0fdc at0057|Prefix “Beatmap by ”, then text constructor|
|Player/date|F1da9.F197d and F196f at00af/00ba|DateTime.ToLocalTime then “Played by {0} on {1}.”|

The direct Rank methods also contain no F1397 read. Finalization latch, success,
validity and the selected Rank identity therefore retain their separate producer
and consumer boundaries. Reaching Rank construction alone proves none of them.

M2b4b reads F198e. Null returns legacy F1985 at000b. Nonnull calls abstract
policy getter M3c55 at0011. That alternate policy was already E023/E033's boundary.
E042 finds formatting and getter consumption, with no additional ordinary
legacy score scaling in the covered Rank methods. The deferred Joined Bézier
numerical mismatch remains open. No original chart/settings/event provenance
has been supplied to resolve it.

M32d7 tests Nullable.HasValue before widget cleanup. A supplied value bypasses
M2b4b and only supplies formatted score. The record reference is still retained.
For ordinary mode0/null F198e the decoded format is `{0:00000000}`. Zero becomes
`00000000` under the specified digit culture. A finite123456789 remains nine
digits. The format specifies minimum width. The alternate F198e-nonnull/mode3
arm uses `{0:0000000}`. Neither arm changes the record's numeric score.

Accuracy formatting is `{0:0.00}%` applied to Single(getter*100f). String.Format
has no explicit IFormatProvider at this callsite. Decimal separators, date text
and later glyph coverage depend on culture/resources. E033's getter formula
remains the authority. Rank does not derive accuracy from its widgets or from
numeric score. Dot-decimal examples in R53/R54 explicitly supply that culture.

Score/count/accuracy strings and header text are prepared values. The score
animation subsequently reads captured F1daf. Grade construction makes further
live calls. M32d7 can be invoked again with another record or score, so a universal
immutable snapshot is unsupported. M490a's graph child retains the record in
F2e06, and creates presentation data from its optional history. Optional F1981/
F1982 lists feed M32d8 statistical text, including error/speed-related values.
Those auxiliary statistics, graph resampling and asynchronous external data are
outside the smallest R15 display contract. No duration default is invented.

F0fe2 is a prepared map string. M1bfb constructs it from base metadata strings,
including base map-string combinations, a difficulty annotation and optional F0fd3
suffix. Its original keys/branches are retained. Empty metadata can reach a
filename-derived fallback at0110–0128. Rank consumes that existing field. It
does not independently rebuild a map title from gameplay objects.

## E. UI resources and draw boundary

The E038 pure decoder recovers these exact resource keys and format strings.
Each JSON string entry retains the encrypted integer identifier, table offset,
key/input provenance and cipher hash. No conventional osu! appearance was used
to guess the strings.

| Consumer | Decoded key / construction |
|---|---|
|M32da:0006/010e|`ranking-` + fresh T0241 enum.ToString, preserving case|
|M32d7 ordinary count arms|`hit300`, `hit100`, `hit50`, `hit300g`, `hit100k`, `hit0`|
|M32d7 common widgets|`ranking-maxcombo`, `ranking-accuracy`, conditional `ranking-perfect`|
|M32db /M32dc|`ranking-background-overlay`, `ranking-title`, `ranking-graph`, `ranking-panel`|
|M32df retry|`ranking-retry`, with reached `pause-retry` fallback|
|M32df replay|`ranking-replay`, with reached `pause-replay` fallback|
|M32d9|`selection-mod-` + modifier.ToString().ToLower()|

Grade uses M38b5(key,7,null) atM32da:002b, then M4510 creates the first grade
sprite and stores F1db1 at0062. A fresh grade read00fe followed by signed >=6
skips the additional pulse sprite. Otherwise a third getter0119 supplies its
resource key. A mutation can therefore change the later resource or eligibility.
The first sprite has scale2→1 and alpha0→1 transformations over the supplied
F1dae+delay through +1000 interval. The extra sprite has later growth/fade
transformations. These are prepared operands, without a measured animation claim.

M0fb6 constructs a resource sprite and optional M0fb7 digit text. M0fb7 forwards
the skin's named FontScore/F36a9 prefix and FontScoreOverlap/F36aa spacing into
T02df/M153d. That constructor
prepares digit0..9 and dot/comma/percent keys using prefix+`-`+suffix. The prefix
and spacing are runtime skin/configuration operands. M093a stores text into
F051d and marks F051e dirty only when the string differs. Container Add/queue
M2ce6 and later update/draw work remain separate operations.

Metadata-only inspection of `osu!ui.dll` finds727 resource entries. Seventeen
of the21 requested result/control keys are present as System.Drawing.Bitmap
serialized entries. `osu!gameplay.dll` has456 entries and none of those keys.
The packet hashes their opaque serialized spans and assembly bytes. It does
not deserialize Bitmap objects, export images, infer image dimensions from
serialized lengths or claim the span hash is a decoded-pixel hash.
The inspected key set includes ranking-XH/SH/X/S/A/B/C/D and the main panels.
ranking-F and ranking-N are absent from those exact two embedded stores, as are
ranking-retry and ranking-replay. Original selected-skin/provider fallback may
still supply a requested resource. Absence from those stores proves no universal
visible fallback behavior. M38b5/provider/HD selection and missing texture policy
remain a presentation dependency at this expedition's boundary.

Rank draw override M32d5 returns if viewport F1db2 is null. Otherwise it calls
base draw M2bf4 at000a, F1daa.M2cf0 at0015, viewport.M2215 at0021 and
F1dac.M2cf0 at002c. The viewport's content container is the stored F1dab used
for results widgets. M2cf0 iterates prepared children and invokes virtual draw
M4737 at04e1 when its state/clip/skip gates permit. For T092e, this resolves
M451a. M451a first calls base eligibility, obtains virtual texture M451d at001b,
and returns without a texture draw when null or M27d7 reports its unavailable
condition. The managed resource request/sprite/container/draw prefix is recovered.
For digit text, T02df overrides that draw slot with M1545. It calls M451a first,
then iterates its glyph textures and reaches the abstract renderer M31f5 at00be
when F1764 is nonnull. Its texture getter M1542 returns null intentionally, so
the base sprite prefix can return before this later glyph loop continues. The
concrete texture renderer remains a runtime dependency. M1541's update checks
the dirty flag and calls virtual M094a → M1544, which clears and rebuilds glyph
lists from stored text. Widget construction and digit preloading are eager in
initialization while this glyph-list rebuilding is deferred to a reached update.
Actual GPU calls, visible pixels and complete text rasterization remain
UNVERIFIED ORIGINAL-RUNTIME.

## F. Reveal, input and departure lifecycle

M32d4 returns immediately when F1db2 is null. Otherwise it updates the viewport,
optional online-child visibility and its score text. `elapsed=unchecked(F2c3a-
F1dae)` and signed elapsed/500 determine the revealed prefix length. Characters
before that index come from captured F1daf. Others use M2333(0,9), ToString and
its first character. This modifies F1da7 text through M093a at00d4. Numeric score
is untouched. Host/UI clock F2c3a is distinct from gameplay clock F2358.
Elapsed>4400 and !F1da5 can reach M32ea's optional external child initialization.
The base update is reached afterward. No independently measured wall-clock
duration follows from the constants alone.

M32e7 manages reveal skipping. If signed elapsed<5000 and F1da2 is false, it sets
that flag0023, stores F1dae=freshF2c3a−7000 at0034, retimes nonloop transforms,
removes the F1da8 overlay and returnsfalse00f6. Otherwise it optionally invokes
M32e8 when its argument is true, then returnstrue. The returned Boolean is
interpreted differently by its callers.

| Control / caller | Recovered reached behavior |
|---|---|
|M32d1 key27|Immediately virtual M32e8, then true|
|M32d1 keys32/13|M32e7(false), discard Boolean, returntrue|
|M32eb back-button event|Virtual M32e8|
|M32e1 retry event|M32e7(false), discard result, then if !F1da3 request sound and Play2,false|
|M32e0 replay event|Requires !F1da3 and M32e7(false)==true before M32e3|
|M32e2|M32e7(true), discard result|
|M32e8|Request `menuback` through M3b30(1f,3), then M46ad(SelectPlay5,false)|

Key identities are the original numeric operands. Escape/Space/Enter aliases
agree with the reached Windows key boundary. No physical input was generated.
The direct key13/32 handler performs reveal work without an unconditional retry
request. The retry event requests `menuhit` then M46ad(Play2,false), subject to
E003's guards. Its reveal Boolean does not gate that request. Replay/download
controls and saved-record selection are retained dependencies, outside the
fresh no-mod implementation recommendation.

M32db's final F1983 predicates M32f1/M32f2 count true versus false history flags.
If trueCount>falseCount it requests `applause` with M3b2b(key,100,7) at03f8,
storing player F1d9e. A positive grade alone is insufficient to infer that request.
Sample availability and native playback retain E037/E038 limitations.

Rank disposal M1ab0 dispatches virtual M1ab5 to M32d3. It stops/removes the optional
F1d9e player via M3b2c, reaches global audio cleanup M3b55, disposes viewport,
unregisters M32d1 and clears an optional selected-map auxiliary. It conditionally
closes the external child. With a local record it calls M2b61 for a nonnull graph
history, removes an optional callback, and calls M2b63 when !M4390. M2b61 can
serialize then clear F1973. M2b63 nulls F1980/F197f/F197e. These operations mutate
the retained record's auxiliary state, without replacing score/hit counters.

At00c2 M32d3 reads **pending** F2c6c. Pending7/14/17/18 skips clearing global.
Other values reach ldnull/stsfldF13c3 at00d9. This is separate from clearing a
local F1da9 or every other alias. The method removes overlays, disposes its three
containers and calls base M2bf6 at0129. Fresh Rank allocation receives default
flags and new containers. Reentering initialization on an existing object has
no established idempotence guarantee. Optional child disposal and audio/helper
exceptions can interrupt cleanup. Full scene/editor/replay lifetimes are bounded
dependencies, beyond the ordinary route.

## G. Conditional reference controls and independent checking

The evidence packet contains78 explicitly numbered controls R01–R78. Each row
records initial operands/references, ordered expected behavior, original method
sites, exception prerequisites and runtime limits. Stable ordinary defaults and
numeric/culture profiles are stated once in `reference_common`. Artificial record
swaps, inconsistent flags, missing subobjects and helper return operands remain
conditional controls. None is an original-runtime observation.

| Controls | Discriminators |
|---|---|
|R01–R26|Literal grade/count/flag/modifier vectors, empty totals, UInt16 maximum and binary32 thresholds|
|R27–R32|Validity versus grade, perfect decoration, repeated getter mutation, handler/global split and reset alias|
|R33–R40|Deferral, admission, pending/current/commit, synchronous/deferred old disposal, reentrant request and record replacement|
|R41–R48|Fresh/reference selection, invalid/null/source-only records, missing/mismatched map, null builder and optional score|
|R49–R56|Score zero/large/override/policy,100%/fractional/empty accuracy, all ordinary counts and maximum combo|
|R57–R62|Grade key/repeated reads, missing texture, initialization before draw, score reveal|
|R63–R70|Reveal skip, back/retry, pending-sensitive disposal, old Play flag mutation and nonreturning cleanup prefix|
|R71–R75|Recorded/source alternatives, optional histories, repeated initialization and applause predicate|
|R76–R78|Validation-count mismatch followed by finalization, failed-latch caller bypass and partially initialized viewport|

The checker derives its arithmetic walk directly from decoded M2b47 instructions,
with separate literal expected outputs. Its stable base-total substitution is
independently checked against M2b44's fields and operations. This validates the
declared finite projection, without certifying original CPU floating execution.
Lifecycle controls are supported by original branch/token/literal relationships
and body hashes. They are not a dynamically executed replacement Rank engine.

## H. Current reconstruction provenance and smallest R15 scope

Read-only inspected baseline files are fingerprinted in JSON. Current
ScoreLedger lives in Gameplay/CircleSession.cs. Program prepares automatic E034
configuration, and PlayfieldView constructs fresh E036 sessions on load/restart.
Its `Practice complete` drawing remains an engineering overlay reading the live
ledger when CircleSession.Finished is true. It formats score N0, accuracy F2,
four counts, maximum combo and extra practice/provenance text. It has no original
Rank type, grade enum/getter, original transition manager or results-record binding.

| Feature | Existing reconstruction | Original-static authority | Readiness |
|---|---|---|---|
|Score|R08 ledger, automatic C02 K|E033/E034, M2b4b → M32d7|Reuse bounded ledger value and getter boundary|
|Accuracy|R08 Single fraction/percentage|E033 M2b42 → M32d7:09bc|Reuse arithmetic, choose/document formatting culture|
|Hit counters|R08 four UInt16 counters|E033 writes, M32d7 six original fields|Four supported whole counters reusable, extra decorated fields unavailable|
|Maximum combo|R08 MaximumCombo|E033 F1975 → M32d7:092a|Reuse supported field, keep current combo separate|
|Grade|Absent as original Rank model|M2b47/T0241 and E042|Bounded base ordinary calculation is established|
|Record validity / success / perfect|HP failure and session state remain separate|F1990/F1978/F197c, M0acd/M2267|Needs explicit results operands and validation provenance|
|Entry timing|Finished/overlay on session clock|E027 + E003 + M46b0/M32db|Separate completion, request, commit and initialization|
|Results UI|WPF practice overlay|M32d7/M32da/M32df/container path|Basic original-derived field model, engineering layout/resources|
|Departure/restart|R/Enter restarts current session|M32e8 SelectPlay request, M32e1 Play request|Bounded host routes with disclosed mode-manager substitution|

**RECONSTRUCTED readiness** is sufficient for a small fresh no-mod/base-legacy
results model. Preserve existing ledger/calibration/scoring calculations. Required
inputs are the completed session/ledger identity, counters, numeric score, maximum
combo, success F1978 equivalent, validity F1990 equivalent, F197c completion
operand/provenance, ruleset/mod policy and supported map/player metadata. Success
cannot be inferred from accuracy, validity or object count alone. F197c needs the
established preparation-required combo operand and the original comparison.
Fresh legacy-policy admission also retains E023's actual factory conditions,
including the7000-row automatic ScoreV2 boundary. A no-mod label alone does not
prove null F198e. Where required-combo preparation is unavailable, keep the
perfect decoration unsupported while presenting the established score/count/grade
subset with its provenance.

An R15 could implement distinct active/completed/results state, original-derived
grade getter and record-bound display, basic reveal/skip and explicit back/retry
actions. Bind the results record after the supported completion/cleanup stage.
A frozen DTO would be an ENGINEERING lifetime decision. The original retains a
mutable reference and makes repeated getters. If a DTO is chosen, document its
capture point and exclude record replacement/replay/reentrant cases from claimed
equivalence. Current practice failure continuation likewise needs an explicit
policy before success/grade can claim the original no-failure route.

WPF text and independently authored grade labels can present those inputs with
no proprietary assets. Original key names can serve as metadata. Full original
skin loading, glyphs, grade pulses, graph/statistics, asynchronous child panels,
replay/local persistence, online requests and native rendering are unsupported
for the smallest milestone. Original Rank-to-selection controls require a host
destination or an explicitly identified engineering substitute for the absent
song-selection frontend. Request/commit semantics should remain distinguishable.

Headless controls can verify the26 numeric vectors, repeated reads versus captured
fields, identity, success/validity/perfect distinctions, entry/exit ordering,
missing metadata/resource handling and session replacement. They can compare all
ledger/health/discrete/continuous audio state before and after results construction.
They can test requests and engineering rendering data without a gameplay window.
Human-visible fidelity, target CLR threshold behavior, original helper/scheduler
interleavings and installed-resource fallback still need separately authorized
qualification. E042 commissions no implementation or runtime campaign.

# 3. Pseudocode

**RECONSTRUCTED finite ordinary projection of CONFIRMED STATIC instructions.**
Names describe evidence relationships. Original calls retain independent reads,
short-circuit behavior, nullable branches and ability to throw or mutate state.

```text
ordinary finalization / transition
    handler aliases a configured global record through M0ace
    reached M0acd
        Play.finalized = true
        fresh handlerRecord.success = NOT fresh failedLatch
        fresh handlerRecord.perfectFlag = original signed difference test
    exit selector reads fresh globalRecord.virtual Grade()
    if grade F8 on ordinary path
        original invalidation and SelectPlay request
    otherwise request Rank7,false through E003 guards
    accepted pending request waits for commit

commit(cleanupParameter)
    previous = current
    current = fresh pending
    dispose old mode now or queue its captured cleanup
    old Play cleanup can update the global record before Rank reads it
    active = new original Rank(host)
    register active
    active.virtual Initialize()                 // M32db
    clear pending only if it still equals current

fresh ordinary Rank initialization
    if globalRecord and sourceRecord are both null
        original queued Menu request
        return
    allocate initial container
    F1da5 = original recorded-source predicate
    ordinary false arm captures fresh globalRecord into F1da9
    retain original eligibility/map/persistence helper branches
    resolve selected map from record hash when required
    if map still absent
        original queued Menu request
        return
    construct viewport, containers, header and controls
    F1dae = fresh hostClock + 300
    PrepareRecordWidgets(F1da9, nullableScore absent)
    retain optional child, input and history-dependent applause operations

base T059e Grade()
    p300 = Single(n300) / Single(fresh virtual WholeTotal())
    p50  = Single(n50)  / Single(another virtual WholeTotal())
    if not fresh successFlag
        return F8
    if p300 == 1f
        return XH0 if original fresh mod8 / mod1024 tests pass else X2
    if Double(p300) > .9 and Double(p50) <= .01 and fresh MISS == 0
        return SH1 if original fresh mod8 / mod1024 tests pass else S3
    if (Double(p300) > .8 and fresh MISS == 0) or Double(p300) > .9
        return A4
    if (Double(p300) > .7 and fresh MISS == 0) or Double(p300) > .8
        return B5
    if Double(p300) > .6
        return C6
    return D7
    // Preserve actual .un branch behavior for NaN and original precision limits.

PrepareRecordWidgets(record, optionalScore)
    if record is null
        return
    score = optionalScore when supplied, otherwise record.virtual NumericScore()
    remove prior widget lists through original helpers
    F1da9 = record
    F1daf = original width7/width8 format(score)
    build ordinary six count widgets from original record fields
    build maximum combo from F1975 and accuracy from virtual getter * 100f
    if record.F197c
        request ranking-perfect widget
    request ranking- + fresh F1da9.virtual Grade().ToString()
    if another fresh grade is signed less than 6
        request additional sprite from another fresh grade
    retain graph/modifier/overlay and container submissions

later Update / Draw
    if viewport is null
        return
    reveal score prefix from F1daf using signed elapsed / 500
    leave numeric record unchanged by reveal
    update original children
    draw eligible prepared containers/viewport through original virtual slots
    missing/unavailable texture can terminate a sprite's draw prefix

reveal / departure
    early unskipped reveal action retimes transforms and returns false
    later reveal action optionally invokes back, then returns true
    back requests menuback and SelectPlay5,false
    retry event requests menuhit and Play2,false when its original busy flag permits
    every mode request retains E003 admission/deferral/commit distinction

Rank Dispose
    original audio, viewport, input, optional child and record-auxiliary cleanup
    if fresh pending mode is outside 7/14/17/18
        globalRecord = null
    dispose containers and base state
```

**ENGINEERING R15 boundary** can use WPF and a bounded results-session adapter
around existing ledger values. **UNVERIFIED ORIGINAL-RUNTIME** still covers
original rendering, installed skin/resources, native timing and scheduler effects.
The deferred score mismatch remains open. Stop after E042.
