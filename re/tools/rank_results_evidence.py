#!/usr/bin/env python3
"""E042 pinned original-static metadata and CIL relationship extraction.

Never imports original CLR code or exports original presentation resource bytes.
The E004/E037 collector and E038 mathematical string decoder remain unchanged.
"""
import hashlib
import json
from pathlib import Path
import struct
import sys
import zipfile

import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import circle_hitsound_evidence as prior
from hitsound_resource_evidence import StaticStrings

SOURCE = "47277a8da8603af93b0c515f86aac2a1463ef51c"
METHODS = sorted(set([
    0x0926, 0x093a, 0x094a, 0x0aad, 0x0aa5, 0x0acd, 0x0ace, 0x0fb6, 0x0fb7,
    0x14b1, 0x153d, 0x1541, 0x1542, 0x1544, 0x1545, 0x1762, 0x1767, 0x1aae, 0x1ab0, 0x1ab1,
    0x1ab2, 0x1ab5, 0x1bfb, 0x1f3b, 0x2258, 0x2266, 0x2267, 0x226e, 0x2274,
    0x2276, 0x2277, 0x2285, 0x2286, 0x22a1, 0x22a2, 0x2b3e, 0x2b42,
    0x2b44, 0x2b47, 0x2b4b, 0x2b4c, 0x2b5a, 0x2b5b, 0x2b5c,
    0x2b61, 0x2b62, 0x2b63, 0x2b64, 0x2bf2, 0x2bf3, 0x2bf4, 0x2bf5,
    0x2bf6, 0x2c63, 0x2c64, 0x2ce6, 0x2cf0, 0x2e1c, 0x38b5, 0x38b8,
    0x31f5, 0x3c55, 0x4390, 0x4391, 0x4415, 0x4510, 0x451a, 0x451d,
    0x46ad, 0x46ae, 0x46b0, 0x46b1, 0x46b2,
    0x46df, 0x46e6, 0x46ef, 0x4711, 0x4737, 0x490a, 0x32f1, 0x32f2,
] + list(range(0x32d0, 0x32ef))))
FIELDS = [
    0x05f3, 0x05fd, 0x136a, 0x136b, 0x1397, 0x13c3, 0x1402,
    0x1969, 0x196a, 0x196b, 0x196c, 0x196d, 0x196e, 0x196f,
    0x1970, 0x1971, 0x1972, 0x1973, 0x1974, 0x1975, 0x1978, 0x197c,
    0x197d, 0x1980, 0x1981, 0x1982, 0x1983, 0x1985, 0x198a, 0x198e,
    0x1990, 0x1991, 0x0fe2, 0x0fdc, 0x0fc8, 0x2a7c, 0x2a7f, 0x2a80,
    0x2c3a, 0x2c6c, 0x2c6d, 0x2c6e, 0x2c70, 0x36a9, 0x36aa,
] + list(range(0x1d9c, 0x1db5))
CALLS = [0x2b47, 0x2b42, 0x2b4b, 0x32d0, 0x32db, 0x32d7, 0x32da,
         0x32dd, 0x32de, 0x32e7, 0x32e8, 0x46b0, 0x2285, 0x2286]
SLOTS = [0x1ab1, 0x1ab2, 0x1ab5, 0x2bf4, 0x2274, 0x2b42, 0x2b44,
         0x2b47, 0x2b4b, 0x32dd, 0x32d7, 0x4737, 0x451d, 0x094a]


def sha(data):
    return hashlib.sha256(data).hexdigest()


def single(value):
    return struct.unpack("<f", struct.pack("<f", value))[0]


def reference_cases():
    """Independently derived finite predictions, never original runtime results."""
    cases = []

    def add(state, expected, sites, kind="lifecycle"):
        cases.append({
            "id": f"R{len(cases) + 1:02}",
            "status": "RECONSTRUCTED STATIC PREDICTION",
            "kind": kind,
            "initial_state": state,
            "expected": expected,
            "evidence": sites,
            "exceptional_dependencies": "Reached getters, wrappers, allocations, optional helpers and resources must return normally. Live references can change between reads.",
            "runtime_boundary": "Original reachability, scheduler/interleaving, target JIT precision, resource availability, rendering and visible pixels unmeasured.",
        })

    # Tuple order is n300, n100, n50, nMiss, success, modifiers, expected enum.
    grades = [
        (10, 0, 0, 0, True, 0, 2),
        (99, 1, 0, 0, True, 0, 3),
        (99, 0, 1, 0, True, 0, 3),
        (99, 0, 0, 1, True, 0, 4),
        (90, 0, 0, 10, True, 0, 5),
        (80, 20, 0, 0, True, 0, 4),
        (70, 30, 0, 0, True, 0, 6),
        (60, 40, 0, 0, True, 0, 6),
        (59, 41, 0, 0, True, 0, 7),
        (95, 0, 5, 0, True, 0, 4),
        (0, 0, 0, 0, True, 0, 7),
        (0, 0, 0, 0, False, 0, 8),
        (10, 0, 0, 0, False, 0, 8),
        (10, 0, 0, 0, True, 8, 0),
        (10, 0, 0, 0, True, 1024, 0),
        (99, 1, 0, 0, True, 8, 1),
        (99, 1, 0, 0, True, 1024, 1),
        (99, 1, 0, 0, True, 16, 3),
        (0, 100, 0, 0, True, 0, 7),
        (100, 0, 0, 0, True, 0, 2),
        (90, 10, 0, 0, True, 0, 4),
        (81, 0, 0, 19, True, 0, 5),
        (71, 29, 0, 0, True, 0, 5),
        (61, 39, 0, 0, True, 0, 6),
        (80, 0, 0, 20, True, 0, 5),
        (65535, 0, 0, 0, True, 0, 2),
    ]
    for a, b, c, miss, success, mods, grade in grades:
        total = a + b + c + miss
        ratios = [single(a / total), single(c / total)] if total else ["NaN", "NaN"]
        add({"counts": [a, b, c, miss], "F1978": success, "mods": mods,
             "F1990": True, "F197c": False, "score": 0, "max_combo": 0,
             "profile": "Base T059e, stable fields, binary32 stored quotient projection"},
            {"grade": grade, "total": total, "p300_p50": ratios},
            ["M2b47:0000-0114", "M2b44:0000-001b"], "grade")

    add("R01 fields except F1990=false", "Grade X2 remains; no F1990 read in M2b47 or direct Rank renderer. Persistence/eligibility consumers are separate.", ["M2b5a:0002", "M2b47", "M32d7", "M32de"])
    add("R02 fields, change only F197c=true and max/current combo", "Grade S3 unchanged; ranking-perfect element is separately requested when F197c=true.", ["M2b47", "M32d7:0b28-0b92"])
    add("R01 getter then mutate F1978=false before second getter", "First X2, second F8; no grade cache store.", ["M2b47:0021-0029", "M32da:0011,00fe,0119"])
    add("R01 getter then replace one 300 by MISS before second getter", "First X2; second A4 for n300=9,nMiss=1 under binary32 projection.", ["M2b47:0066-00df"])
    add("Global=A, handler=B, both nonnull", "M0acd mutates B after Play latch. M32de later retains current global A. B's completion does not establish A's grade.", ["M0acd:0001-003d", "M32de:0001-0011"])
    add("M0ace reached with global=A", "handler.F05f3=A before virtual M2b62 reset; reference alias, no record copy.", ["M0ace:0001-0012"])
    add("Rank request7,false with closed E003 time gate", "Queue M46e6 then return before pending write or construction; callback reruns live guards.", ["M46ad:0030-006d", "M46e6:000c"])
    add("Deferred callback later passes all guards, pending differs", "Accepted request writes pending7/phase0; no M32d0 construction within M46ad.", ["M46ad:022a-02ed"])
    add("pendingRank7/currentPlay2, outgoing callback has not committed", "Current Play remains; pending acceptance does not initialize Rank.", ["M46ad:022a", "M46b0:0063-0072"])
    add("pendingUnknown10", "M46b0 returns at0009 before disposal/construction.", ["M46b0:0000-0009"])
    add("pendingRank7/currentPlay2, cleanup_parameter=0, helpers normal", "previous=Play;current=Rank; remove/dispose old mode, clear active, construct Rank, register, initialize, clear matching pending.", ["M46b0:0063-00d0,04b5-04bf,054b-0577"])
    add("Same, cleanup_parameter>0", "Queue captured old-object cleanup M46df; Rank initialization may precede its execution. Scheduler outcome remains unresolved.", ["M46b0:0084-00ac", "M46df:0000-0021"])
    add("Rank initializer requests another pending mode", "Final pending/current mismatch preserves the new request.", ["M46b0:0569-0577"])
    add("Global changes A->B between request and ordinary M32de", "Rank captures B at initialization. Request carries a mode/Boolean, no completed-record payload.", ["M2286", "M46ad", "M32de:0001-0006"])
    add("Ordinary init F2a7c=false, global valid A, map resolves", "F1da5=false; M32dd calls M32de(true); F1da9=A; no new completed record.", ["M4390", "M32dd:0000-0011,0122-0129", "M32de:0001-0006"])
    add("Same with A.F1990=false, F1978=true", "Direct Rank record/grade/display path has no validity veto. Conditional persistence helper remains external to supported display model.", ["M32de", "M32db:002a-0098", "M2b47"])
    add("Both global F13c3 and source F2a7f null", "M32db queues M46ae(Menu0,true,0), then returns before containers/statistics.", ["M32db:0000-0016"])
    add("Global null, source nonnull, ordinary F2a7c=false", "Initial null check passes, M32de stores null then dereferences F1978 and can throw. No universal safe empty-results screen.", ["M32db:0000-0024", "M32de:0001-0011"])
    add("Selected map absent, record hash cannot resolve", "After record selection, M32db requests Menu0,true,0 and returns before result widgets.", ["M32db:003a-0098"])
    add("Map hash differs; record hash resolves another map", "M2c67(record.F1972), conditional M2c64, then fresh M2c63 check. UI reads newly selected map metadata.", ["M32db:0053-0098", "M32df:0000-0057"])
    add("M32d7 receives null while prior UI exists", "Immediate return, no old-widget removal, record replacement or new grade call.", ["M32d7:0000-0003"])
    add("M32d7 receives record and absent nullable score", "Invoke virtual M2b4b once before list cleanup; keep nullable override local for score formatting.", ["M32d7:0004-0021"])
    add("Legacy record score0, supplied score absent", "F1daf='00000000' under specified digit culture; numeric score remains0.", ["M2b4b:0001-0010", "M32d7:00b0-00c6"])
    add("Legacy score123456789, supplied score absent", "F1daf='123456789'; eight placeholders are a minimum width, no score cap or truncation.", ["M32d7:00b0-00c6"])
    add("Explicit nullable score123, underlying score999", "F1daf uses123 ('00000123'); underlying record is retained and score field remains999.", ["M32d7:000b-0021,00b0-00c6"])
    add("Alternate F198e nonnull or record.mode3 (outside ordinary legacy)", "Seven-placeholder formatting branch; M2b4b may use abstract policy M3c55. No new legacy transformation inferred.", ["M32d7:0082-00ae", "M2b4b:0000-0016"])
    add("n300=1,other whole counts0", "Accuracy getter1f; multiply100f then format '{0:0.00}%'. For dot-decimal culture request '100.00%'.", ["M2b42", "M32d7:09b1-09cc"])
    add("n300=1,n100=1,other whole counts0", "Accuracy fraction Single400/600; formatted percent about66.67 under dot-decimal culture; score independent.", ["M2b42:000f-003d", "M32d7:09b1-09cc"])
    add("All whole counts0,F1978=true", "Accuracy100% getter policy while grade D7 under binary32 NaN control. No successful play inferred.", ["M2b42:0001-000e", "M2b47:0000-0026,0066-0114"])
    add("Ordinary n300=9,n100=2,n50=1,nMiss=3,extra counts4/5,max123", "Six count widgets read F196a/F1969/F196b/F196c/F196d/F196e. Max widget reads F1975=123. Current combo and snapshot do not replace it.", ["M32d7:0196-033a,091e-0948"])
    add("Grade X2, resources supplied", "M32da requests key 'ranking-X', stages7,null provider; constructs F1db1 and, if later grade<6, a second grade sprite from another fresh getter.", ["M32da:0001-0062,00f8-016a"])
    add("Grade D7 or F8", "First grade sprite request still occurs; signed later >=6 skips extra pulse sprite.", ["M32da:0001-0062,00f8-0104"])
    add("Grade mutates X2->F8 after first M32da getter", "First resource key ranking-X remains; later >=6 skips second sprite. Three potential grade calls are not one snapshot.", ["M32da:0011,00fe,0119"])
    add("Texture lookup yields null or texture.M27d7()==true", "Sprite can exist without drawing a valid texture. M451a returns true at002d; visible pixels unproved.", ["M451a:001b-002d"])
    add("All initializers return before first draw", "Widgets queued into containers before M32db returns. M32d5 later reaches base/draw containers and viewport, only if F1db2 nonnull.", ["M32db:0341-0357", "M32d5:0001-0032", "M2cf0:04e1"])
    add("UI elapsed=0,500,4000,5000; score string eight chars", "M32d4 exposes prefix lengths0,1,8,10 through signed elapsed/500; unrevealed digits use random helper; underlying score unchanged.", ["M32d4:004b-00d4"])
    add("First M32e7(false), elapsed<5000 and F1da2=false", "Set skip flag;F1dae=currentClock-7000;retime nonloop transforms; remove F1da8;returnfalse, no departure from this call.", ["M32e7:0000-00f6"])
    add("M32e7(false) after elapsed>=5000 or skip flagtrue", "Returntrue without requesting exit; argumenttrue additionally invokes M32e8.", ["M32e7:00f7-0101"])
    add("Escape key27 or back-button callback reached", "Virtual M32e8 requests menuback sound then SelectPlay5,false. E003 guards still apply.", ["M32d1:0000-000b", "M32eb", "M32e8:0000-001d"])
    add("Retry click, F1da3=false", "M32e1 calls reveal helper, then menuhit and requests Play2,false even if reveal helper returnedfalse. Entry is subject to E003 Rank->Play gate.", ["M32e1:0000-002d", "M46ad:0133-0154"])
    add("Rank disposal pendingSelectPlay5, local record nonnull, non-recorded", "Serialize/clear graph and replay auxiliaries, clear globalF13c3 at00d9; retained external references do not become null automatically; containers disposed.", ["M32d3:0072-0129", "M2b61", "M2b63"])
    add("Rank disposal pending7,14,17 or18", "Skip global null store; same optional local cleanup still precedes pending test.", ["M32d3:00c2-00de"])
    add("Exit selector sees F1978=true; old Play cleanup F1402=true, non-recorded, normal eligibility, F136b>0", "Reached M2267:02fb clears F1978 before Rank grade initialization. Earlier non-F selection can lead to displayed F8 request; helpers/gates retain full prerequisites.", ["M2285", "M2266:00e3", "M2267:02ed-0305", "M32da:0011"])
    add("Old Play cleanup F1402=false", "M2267 reaches M22aa and Environment.Exit(-1) prefix. No guaranteed Rank initialization return.", ["M2267:0001-0014"])
    add("Playback-source branch F2a7c=true,F2a80=true,global nonnull", "M32dd retains global; empty nonnull player name filled from F1118.M13d4. A null name can throw at get_Length.", ["M32dd:0016-0059"])
    add("Playback-source branch chooses F2a7f and matching historical saved record", "Optional record/list replacement and M2b64 restore path remain beyond fresh ordinary R15 support.", ["M32dd:005a-0121"])
    add("F1973/F1981/F1982 optional histories null or empty", "Graph/statistical helper branches can be skipped while score/count/grade widgets remain. No duration or unstable-rate default invented.", ["M32d7:0a01-0b28", "M490a:0045-005c"])
    add("Reentrant M32db on same object, helpers return", "No one-time initialization latch in this body; containers/listeners may be replaced or added again. Safe idempotence is unsupported.", ["M32db", "M32d7:0026-0076"])
    add("F1983 contains more true than false flags and initialization reaches suffix", "M32db requests applause through M3b2b(key,100,7), stores optional playerF1d9e; disposal passes it to M3b2c. Availability/audibility remains conditional.", ["M32db:0380-0402", "M32f1", "M32f2", "M32d3:0001-000e"])
    add("Artificial matched global/handler record: n300=9,other whole counts0;managerF17fc=10;F136a=false;D/K/F1376=false;T>B+200;live validation reached", "M0aa5 count equality is false. M2276 invalidates F1990 then still invokes M2277/M0acd. Success becomestrue; base grade can return X2 despite mismatched count and invalidity. Later request guards and cleanup remain separate.", ["M0aa5:003a", "M2276:00f5-010f", "M0acd:0007-003d", "M2b47"])
    add("Failed gameplay latchF136a=true at E027 outer Play update0abd", "Ordinary late finalization/automatic Rank block is bypassed by the reached failure/control return. Artificial direct M0acd would store F1978=false, and its base grade F8, without proving that ordinary Rank was reached.", ["E027 M226b:0abd-0ad0", "M0acd:000c-001a", "M2b47:0021-0029"])
    add("Artificial partial Rank initialization: F1db2=null; optionalF1da4/F1d9e=null", "Update M32d4 and Draw M32d5 return before score reveal/container draw. Disposal skips optional player/child work while retaining other original cleanup. Constructed Rank does not establish a presented result.", ["M32d4:0001-0008", "M32d5:0001-0008", "M32d3:0001-0013,0053-0071"])
    return cases


def extract(path):
    assert sha(Path(path).read_bytes()) == prior.ARCHIVE
    with zipfile.ZipFile(path) as archive:
        raw = archive.read("osu!.exe")
    assert sha(raw) == prior.EXE
    prior.METHODS = METHODS
    prior.FIELDS = {0x04000000 + n for n in FIELDS}
    prior.CALLS = {0x06000000 + n for n in CALLS}
    prior.SLOTS = SLOTS
    out = prior.extract(path)
    out["expedition"] = 42
    out["research_source"] = SOURCE
    pe = dnfile.dnPE(data=raw)
    tables = pe.net.mdtables
    decoder = StaticStrings(pe)
    owners = {f.row_index: n for n, t in enumerate(tables.TypeDef, 1) for f in t.FieldList}
    for rid in FIELDS:
        field = tables.Field.rows[rid - 1]
        out["fields"][hex(0x04000000 + rid)] = {
            "owner": hex(0x02000000 + owners[rid]), "name": str(field.Name),
            "signature": field.Signature.value.hex(), "static": field.Flags.fdStatic,
            "has_default": field.Flags.fdHasDefault, "has_rva": field.Flags.fdHasFieldRVA,
        }
    out["strings"] = {}
    out["string_sites"] = []
    for rid in METHODS:
        row = tables.MethodDef.rows[rid - 1]
        if not row.Rva:
            continue
        body = read_method_body_from_bytes(pe.get_data(row.Rva))
        instructions = list(body.instructions)
        for j, ins in enumerate(instructions):
            if getattr(ins.operand, "value", None) != 0x060049b0:
                continue
            previous = instructions[j - 1]
            if previous.opcode.name != "ldc.i4":
                continue
            identifier = previous.operand
            # Results/presentation metadata strings only. Large parser/global helpers
            # are retained as relationship evidence without broad string extraction.
            if 0x32d0 <= rid <= 0x32ee or rid in [0x153d, 0x1bfb]:
                out["strings"][str(identifier)] = decoder.decode(identifier)
                out["string_sites"].append([hex(0x06000000 + rid), ins.offset - body.header_size, identifier])
    out["string_decoder"] = decoder.provenance
    out["enums"] = {}
    for rid in [0x0241, 0x0b4b, 0x0b57, 0x0b58]:
        row = tables.TypeDef.rows[rid - 1]
        ids = {f.row_index for f in row.FieldList}
        out["enums"][hex(0x02000000 + rid)] = {
            "name": str(row.TypeNamespace) + "." + str(row.TypeName),
            "values": {str(c.Parent.row.Name): {"field": hex(0x04000000 + c.Parent.row_index),
                       "value": int.from_bytes(c.Value.value, "little", signed=True)}
                       for c in tables.Constant if c.Parent.table.name == "Field" and c.Parent.row_index in ids},
        }
    out["rank_ancestry"] = []
    rid = 0x06a8
    while rid:
        row = tables.TypeDef.rows[rid - 1]
        out["rank_ancestry"].append({
            "token": hex(0x02000000 + rid), "name": str(row.TypeName),
            "methods": [{"token": hex(0x06000000 + m.row_index), "name": str(m.row.Name),
                         "signature": m.row.Signature.value.hex(), "virtual": m.row.Flags.mdVirtual,
                         "new_slot": m.row.Flags.mdNewSlot} for m in row.MethodList],
        })
        rid = row.Extends.row_index if row.Extends and row.Extends.table.name == "TypeDef" else 0
    ancestry = {int(r["token"], 16) & 0xffffff for r in out["rank_ancestry"]}
    out["method_impls"] = [{"class": hex(0x02000000 + m.Class.row_index),
                           "body": [m.MethodBody.table.name, m.MethodBody.row_index],
                           "declaration": [m.MethodDeclaration.table.name, m.MethodDeclaration.row_index]}
                          for m in tables.MethodImpl if m.Class.row_index in ancestry | {0x059e}]
    # Hash opaque serialized resource-entry spans. Do not deserialize Bitmaps or
    # write image contents. DataOffset is relative to the ResourceSet data section.
    keys = ["ranking-" + name for name in ["XH", "SH", "X", "S", "A", "B", "C", "D", "F", "N",
            "title", "panel", "graph", "accuracy", "maxcombo", "perfect", "background-overlay",
            "retry", "replay"]] + ["pause-retry", "pause-replay"]
    out["presentation_resource_metadata"] = []
    with zipfile.ZipFile(path) as archive:
        for assembly in ["osu!ui.dll", "osu!gameplay.dll"]:
            data = archive.read(assembly)
            dll = dnfile.dnPE(data=data)
            for resource in dll.net.resources:
                store = resource.data
                entries = {str(entry.name): entry for entry in store.entries}
                offsets = sorted({entry.struct.DataOffset for entry in store.entries})
                start = store.struct.DataSectionOffset
                records = {}
                for key in keys:
                    if key not in entries:
                        records[key] = None
                        continue
                    entry = entries[key]
                    offset = entry.struct.DataOffset
                    following = next((n for n in offsets if n > offset), len(store._data) - start)
                    span = store._data[start + offset:start + following]
                    records[key] = {"type": entry.type_name, "data_offset": offset,
                                    "serialized_span_bytes": len(span), "serialized_span_sha256": sha(span)}
                out["presentation_resource_metadata"].append({
                    "assembly": assembly, "assembly_sha256": sha(data), "resource_set": str(resource.name),
                    "entry_count": len(entries), "data_section_offset": start, "keys": records,
                })
            dll.close()
    root = Path(__file__).resolve().parents[1]
    authority = ["02/static-control-flow.json", "03/request-guards.json", "19/play-failure.json",
                 "23/aggregate-policy.json", "27/gameplay-completion.json", "33/score-accounting.json",
                 "34/chart-score-multiplier.json", "35/health-state.json", "36/newcombo-preparation.json",
                 "38/hitsound-resources.json"]
    out["authority_inputs"] = {name: sha((root / "evidence" / name).read_bytes()) for name in authority}
    out["collector_sources"] = {name: sha((root / "tools" / name).read_bytes()) for name in [
        "rank_results_evidence.py", "circle_hitsound_evidence.py", "beatmap_init_evidence.py",
        "hitsound_resource_evidence.py"]}
    repo = root.parent
    sources = ["reconstructed/CircleGame/Gameplay/CircleSession.cs",
               "reconstructed/CircleGame/Rendering/PlayfieldView.cs", "reconstructed/CircleGame/Program.cs"]
    out["reconstruction_read_only"] = {name: sha((repo / name).read_bytes()) for name in sources}
    out["reference_common"] = {
        "record": "Fresh no-mod ordinary base T059e, null F198e, stable references unless explicitly varied.",
        "rank": "Ordinary F2a7c=false, global nonnull, same resolvable map, valid host/resources and normally returning helpers unless varied.",
        "numeric": "Binary32 stored-quotient finite projection. Original x86 extended precision and exceptional conversion remain runtime boundaries. Explicit dot-decimal culture only in literal text examples.",
        "execution": "All original reference cases unexecuted. Python numeric checks validate a static projection, never original CLR/game methods.",
    }
    out["reference_cases"] = reference_cases()
    out["verification_development"] = {
        "initial_checker_sha256": "062a10d46fa60127cd9428d39fb48131b62836b2c1f365f57cbcb458afdc5cf9",
        "initial_failure": "New checker line337 assumed every selected string had the uncompressed byte-character flag. Original identifier2064569235 has encoded_size36, UTF16LE text, byte flagfalse. Archive audit stopped at that assertion after method/field/grade checks.",
        "expected_actual": {"incorrect_expected_high_nibble": "0x40000000", "actual_high_nibble": "0x00000000",
                            "identifier": 2064569235, "offset": 123961, "cipher_bytes": 36},
        "correction": "Independently follow E038 M49b1 byte-character versus Unicode branch. Decode decrypted bytes as UTF16LE when flag absent. Assert this exact nonbyte control explicitly. No original finding or historical checker changed.",
        "setup_probes": "A runpy invocation initially omitted re/tools from sys.path and decoded no original instructions. A preliminary relationship check read the prior74-case JSON while the75-case extraction was pending. Final checks wait for completed extraction.",
    }
    out["limits"] = [
        "CONFIRMED STATIC metadata/instructions; RECONSTRUCTED conditional cases; no original-runtime observation",
        "Direct managed inventories exclude reflection/native/external mutation and arbitrary aliasing",
        "Texture key request/container submission/draw reachability do not establish visible pixels",
        "Persistence, replay, online child, native renderer, installed resources, scheduler and arbitrary mods remain bounded dependencies",
        "Historical1630 independent QA checks were not rerun; E02017 original-runtime cases remain NOT RUN",
        "No reconstruction changes, multiplier correction, R15, E043 or QA13",
    ]
    pe.close()
    return out


if __name__ == "__main__":
    result = json.dumps(extract(sys.argv[1]), sort_keys=True, indent=2) + "\n"
    Path(sys.argv[2]).write_text(result, encoding="utf-8", newline="\n")
