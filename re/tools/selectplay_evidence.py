#!/usr/bin/env python3
"""E043 pinned original-static SelectPlay evidence. No CLR or game execution.

Uses the unchanged E004/E037 metadata collector and E038 mathematical string
decoder. Publishes identities, relationships and hashes, never original assets.
"""
import hashlib
import json
import math
from pathlib import Path
import sys
import zipfile

import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import circle_hitsound_evidence as prior
from hitsound_resource_evidence import StaticStrings

SOURCE = "b440e4e601798be9157fbaa1274a25fcac130232"
METHODS = sorted(set([
    0x0759, 0x0801, 0x0803, 0x0805, 0x08cf, 0x08c8, 0x08d9,
    0x0b23, 0x13d6, 0x19b6, 0x19b8, 0x19b9, 0x19bb, 0x19bc, 0x19bd, 0x19c0, 0x19c5,
    0x1ab0, 0x1ab1, 0x1ab2, 0x1bb2, 0x1bb5, 0x1bb6, 0x1bb8, 0x1bb9,
    0x1bba, 0x1bbb, 0x1bc1, 0x1bc6, 0x1bc7, 0x1bc8, 0x1bc9,
    0x1bce, 0x1bcf, 0x1bd0, 0x1bd5, 0x1bd6, 0x1bd7, 0x1bd8, 0x1bd9,
    0x1bda, 0x1bdb, 0x1bdc, 0x1bdd, 0x1bde, 0x1be0, 0x1bf1, 0x1bf2,
    0x1bf3, 0x1bf4, 0x1bfa, 0x1bfb, 0x1bff, 0x1c02, 0x1c05, 0x1c12,
    0x1c13, 0x1c14, 0x1c16, 0x1c1e, 0x1c1f, 0x1c23,
    0x2232, 0x2233, 0x2237, 0x223b, 0x28af, 0x28b1,
    0x2b08, 0x2b0e, 0x2b12, 0x2b13, 0x2b1f, 0x2b25,
    0x2bf2, 0x2bf3, 0x2bf4, 0x2bf5, 0x2bf6,
    0x2c5e, 0x2c60, 0x2c61, 0x2c62, 0x2c63, 0x2c64, 0x2c65, 0x2c66,
    0x2c67, 0x2c68, 0x2c69, 0x2c6a, 0x2c6d, 0x2c6e, 0x2c6f, 0x2c70, 0x2c71,
    0x2c77, 0x2c85, 0x2c86, 0x2c87, 0x2c88, 0x2c89, 0x2c8a, 0x2c8b, 0x2c8c,
    0x2c97, 0x2ca4, 0x2ca5, 0x2ca6, 0x2ca7, 0x2ca8, 0x2ca9, 0x2cae, 0x2caf,
    0x2cf0, 0x2d7f, 0x32d3, 0x32db, 0x32e8, 0x3696,
    0x3b0e, 0x3b3e, 0x3b45, 0x3b47, 0x3b48, 0x3b50, 0x3d51, 0x3d52,
    0x3e6a, 0x3e6b, 0x3e6f, 0x3e70, 0x3e71, 0x3e72, 0x3e73, 0x3e74,
    0x3e7e, 0x3e80, 0x3e82, 0x3e84, 0x3e86, 0x3e88, 0x3e8a, 0x3e8c,
    0x3e90, 0x3e91, 0x3e93, 0x3e94, 0x3e95, 0x3e96, 0x3e97, 0x3e9c, 0x3e9d,
    0x3ea7, 0x3eba, 0x3ebb, 0x3ebc, 0x3ec6, 0x3ec7, 0x3ecd, 0x3ece,
    0x3ed0, 0x3ed1, 0x3ed2, 0x3ed3, 0x3ed5, 0x3ed6, 0x3ed7, 0x3ed8,
    0x3ee5, 0x3eef, 0x3f1c, 0x4416, 0x46ad, 0x46ae, 0x46af, 0x46b0,
    0x46df, 0x46e6, 0x4710, 0x4711, 0x496f, 0x4970,
] + list(range(0x2571, 0x25cd)) + [0x25d1, 0x25f3]))
# Complete direct inventories for deliberately bounded state, not every UI field.
FIELDS = sorted(set([
    0x0b52, 0x0b53, 0x0b54, 0x0b55, 0x0b58, 0x0b5b, 0x0cc6, 0x0ec1,
    0x0fc3, 0x0fc5, 0x0fc7, 0x0fc8, 0x0fca, 0x0fcd, 0x0fce, 0x0fd6,
    0x0fdc, 0x0fde, 0x0fea, 0x0feb, 0x0fec, 0x0fff, 0x1008, 0x100a,
    0x1014, 0x13f8, 0x158e, 0x159a, 0x15ad, 0x15ae, 0x15af,
    0x15c4, 0x15c5, 0x15ce, 0x15db, 0x15de, 0x15e3, 0x15e4, 0x15e5,
    0x1941, 0x1942, 0x1948, 0x1949, 0x19ff, 0x1a01, 0x1a02, 0x1a03,
    0x1a0b, 0x1a0d, 0x1a0f, 0x2367, 0x24ad, 0x2539, 0x253a, 0x253b,
    0x2540, 0x2549, 0x254a, 0x254b, 0x254d, 0x2557, 0x255a,
    0x2c48, 0x2c6b, 0x2c6c, 0x2c6d, 0x2c6e, 0x2c70,
]))
CALLS = [0x1bc1, 0x1bc6, 0x1bd0, 0x2571, 0x2578, 0x2581, 0x25c0,
         0x2c64, 0x2c85, 0x2c87, 0x3ebb, 0x3ebc, 0x3b47, 0x46ae]
SLOTS = [0x1ab0, 0x1ab1, 0x1ab2, 0x2bf4, 0x2bf6, 0x4416, 0x08c8, 0x08d9]


def sha(data):
    return hashlib.sha256(data).hexdigest()


def json_safe(value):
    if isinstance(value,float) and not math.isfinite(value):
        return "NaN" if math.isnan(value) else "+Infinity" if value>0 else "-Infinity"
    if isinstance(value,dict):
        return {k:json_safe(v) for k,v in value.items()}
    if isinstance(value,list):
        return [json_safe(v) for v in value]
    return value


def reference_cases():
    cases = []

    def add(initial, expected, evidence, projection=None):
        cases.append({"id": f"P{len(cases)+1:02}",
            "status": "RECONSTRUCTED STATIC PREDICTION", "initial_state": initial,
            "expected": expected, "evidence": evidence, "projection": projection,
            "prerequisites": "Ordinary mode5/ruleset0, fresh no replay, normally returning reached helpers, valid host/collections unless explicitly varied.",
            "unresolved": "Original scheduler/interleaving, live references between reads, culture, provider/resource availability, native preview and visible UI are unmeasured."})

    add("M46ad target5, F2c48=true, other guards admitted", "Target redirects Menu0; no guaranteed SelectPlay controller.", ["M46ad", "E003"])
    add("M46ad target5, closed E003 time gate", "Queue M46e6; callback reruns guards; pending/current and construction are not established by submission.", ["M46ad:0030-006d", "M46e6:000c"])
    add("pending5/current7, commit not reached", "Current remains Rank7; no controller construction yet.", ["M46b0:0063-0072"])
    add("M46b0 switch5, M19b8=true", "Construct loading T0373 and set current19.", ["M46b0:0361-0398"], {"kind":"construction","loading":True,"previous":7,"count":1,"result":"loading19"})
    add("M19b8=false, previous7, catalog empty", "Loading19, not empty visible song list by assumption.", ["M46b0:0368-0398"], {"kind":"construction","loading":False,"previous":7,"count":0,"result":"loading19"})
    add("M19b8=false, previous19, catalog empty", "Construct T04d6; M258f can reject ordinary availability during constructor/init.", ["M46b0:0368-03ac", "M258f"], {"kind":"construction","loading":False,"previous":19,"count":0,"result":"T04d6"})
    add("M19b8=false, previous7, catalog1 valid map", "Construct T04d6, register then virtual M25c0. Drawing requires later draw path.", ["M46b0:03a7,055a-0564", "M25bf:0326-0360"], {"kind":"construction","loading":False,"previous":7,"count":1,"result":"T04d6"})
    add("M258f catalog2, F1a0d=2, ordinary flagtrue", "Set F15c4; returnfalse, with settings-dependent mode15 or notification actions.", ["M258f:0000-00be"])
    add("One supplied catalog map A, available, normal grouping12", "One T059a row retains A reference; browser can commit it through F2557/M2581.", ["M3ed2:0506", "M3ed5", "M3ebc:0092-00ac"])
    add("Prepared rows A/B same folder, C another folder, all available", "A/B share contiguous run F1942; C begins new run, independent of chart/set IDs.", ["M3ea7:005f-00b5"], {"kind":"runs","folders":["s","s","t"],"result":[1,1,2]})
    add("Prepared rows A/B/A folders s/t/s, all available", "Three runs in supplied order; not a global folder GroupBy after sorting.", ["M3ea7:005f-00b5"], {"kind":"runs","folders":["s","t","s"],"result":[1,2,3]})
    add("GlobalA, commit difficultyB, both same folder", "F254b first changes; event stores globalB, clears old cached resource, metadata refreshes; same-folder preview can reuse song.", ["M3ebc:003e,0092-00ac", "M2581:0011,00df-01ec", "M2c64"])
    add("GlobalA, commit songC with different folder", "GlobalC; same-folder preview shortcut false; selection-expand resource request conditional on refresh.", ["M2581:00df-0134"])
    add("GlobalA, setter(A)", "Return without cleanup/store.", ["M2c64:0000-0008"], {"kind":"setter","old":"A","new":"A","throws":False,"result":{"global":"A","clean":False}})
    add("GlobalA, setter(B), old cleanup returns", "Old M1c12 then M38b9(true), then storeB.", ["M2c64:0009-0021"], {"kind":"setter","old":"A","new":"B","throws":False,"result":{"global":"B","clean":True}})
    add("GlobalA, setter(B), old M1c12 throws", "Store not reached; globalA retained, preceding helper may be partially mutated.", ["M2c64:0015-0021"], {"kind":"setter","old":"A","new":"B","throws":True,"result":{"global":"A","clean":True}})
    add("GlobalA, browser selection callback map=null", "No M2c64(null) in this callback; globalA remains.", ["M2581:000c-0011"], {"kind":"selection_callback","old":"A","candidate":None,"result":"A"})
    add("Globalnull, browser callbackA", "SetglobalA before metadata/preview preparation.", ["M2581:0011-006e"], {"kind":"selection_callback","old":None,"candidate":"A","result":"A"})
    add("GlobalA, browser highlightB in different run via M3ec7", "F254d=B index; no committed F254b/F2557 map change on that branch.", ["M3ec7:0017-0040"])
    add("GlobalA, unavailable selectedA absent from rebuilt rows", "Reference search returns-1; callbacknull leaves globalA. No unconditional first-map recovery.", ["M3ebb", "M3f1c", "M3ebc:0187-019c", "M2581:000c"])
    add("Indexed A.F0fde=true during scan removal", "M2caf true removes A; watcher/refresh callbacks and selected-map relookup remain separate.", ["M2c88:0098,0229-0245", "M2caf"])
    add("Catalog entry existing filename x, incoming same x, equal F0fee", "M1bf4 compares filename only; registration returns existing index before hash/ID dictionary assignment.", ["M1bf4:0001-000c", "M2c6f:0006-0029"])
    add("Two catalog records with same positive chartID, rebuild in A,B order", "Dictionary indexer laterB wins; no universal duplicate-ID rejection.", ["M2c6a"])
    add("M2c85 catalognonempty, forcefalse", "Returnfalse; skip DB reload/watcher recreation.", ["M2c85:0000-0017"])
    add("DB supplied, M2ca5 read succeeds", "List<T03ad> restored, dictionaries rebuilt; UI selection need not parse every file.", ["M2ca5:006c-009a", "M1bf1", "M1bf2"])
    add("New local *.osu discovered, metadata helper normal, valid/loadable predicates true", "Construct map, store filename/folder, parse M1bc1, register, enqueue F1a0b; selection later in constructor.", ["M2c8c:018b-0265", "M2571:1de0-1df9"])
    add("SelectedA, M2581 changed toB; B not prepared and locally loadable, M1bc1 false", "GlobalB already assigned; repair/error branch can return before preview. No selection rollback proven.", ["M2581:0011,013a-01bb"])
    add("Async row build captured controllerA; active controllerB at M25f3", "Return before enabling browser/selection callback.", ["M25f3:0000-0020"])
    add("Async rows built; global changes A->B before M25f3, B present byref", "Fresh globalB used for browser reference search and selection restoration.", ["M25f3:0048-005f", "M3f1c"])
    add("Ordinary start, selectednonnull, randomfalse, phase2", "Cleanup M2577(true) precedes queued M46ae(2,false,1000); no map captured by transition closure.", ["M2578:0000-0024,0311-031d", "M46ae:0000-0016"])
    for selected, random, phase in [(False,False,2),(True,True,2),(True,False,1)]:
        add({"selected_nonnull":selected,"F2540":random,"F2c6b":phase}, "M2578 returns before Play request.", ["M2578:0000-0024"], {"kind":"start_guard","selected":selected,"random":random,"phase":phase,"result":False})
    add("Ordinary eligible start with selectedA but missing chart file", "M2578 has no direct file-exists guard; downstream map preparation/loader can fail. A request is not successful loading.", ["M2578", "M223b:008e", "E004 M223c failure"])
    add("Queued M46ae target2, current!=2, action not executed", "No committed Play yet; capture contains target/repeat/cleanup scalar only.", ["M46ae:0008-0016,0059-006f"])
    add("Queued start forA; global replacedB before M2237", "Play.F13f8 capturesB; request does not freezeA.", ["M4711:0094-009f", "M2237:0001-0006"], {"kind":"capture","request":"A","at_init":"B","after_init":"C","result":"B"})
    add("Play initializedcapturedA; global changesB afterwards", "F13f8 remainsA absent another writer; loading retains its own capture and additional fresh reads.", ["M2237:0006", "M223b", "E004"], {"kind":"capture","request":"A","at_init":"A","after_init":"B","result":"A"})
    add("Rank Back with ordinary localrecord", "M32e8 requests5,false via M46ad; no map or scroll payload in request.", ["M32e8:0018", "M46ad"])
    add("Rank Back deferred/rejected/redirected", "No guaranteed SelectPlay construction; E003 live admission gates apply.", ["M32e8", "M46ad", "M46e6"])
    add("Rank return globalA still in catalog, saved group/search present", "Ctor captures liveglobal, restores group/search, async M25f3 locatesA byref. PreviousRank does not set returnFromPlay flag.", ["M2571:0049-0052,0095,1e21-1e50", "M25f3:0048-005f"])
    add("Rank return globalA gone, queued newmapnull", "M3ebb may fail refsearch. Later state depends constructor validation and preview fallback; no unconditional same-scroll guarantee.", ["M2571:0130-01c3", "M3ebb", "M25c2:008e-009d"])
    add("MetadataA available, F2540=false", "M25b2 reads A title/BPM/end/counts/creator/difficulty text; F1008 is not E034 score drain-duration operand.", ["M25b2", "M1c16", "M1bde", "M1bdb"])
    add("Selected difficulty changes A->B with different version/CS and starcache", "Changed selection triggers M25b2 for B; completed background calc refreshes only when same reference as current selection.", ["M2581:006e", "M25d1:0049-005c"])
    add("Ordinary selected map has optional Unicode disabled", "M1c16 returns cached F0fe3; normal fallback string prepared by M1bfb.", ["M1c16", "M1bfb"])
    add("M25c1(argmapA,true), globalB", "Calls M3b47 with freshglobalB; supplied map argument is not the audio operand.", ["M25c1:0022-002a"])
    add("Two maps same folder and audio filename; existing receiver valid and eligible", "M3b45 rebinds F2367.F24ad to newmap and returnsfalse; stream reuse branch.", ["M3b45:0064-00bc"])
    add("Missing selected audio resource", "M1c02 may returnnull; M3d52 can use M3d51 synthetic timeline receiver. No guaranteed audible preview.", ["M1c02", "M3d52:0041-0068", "M3d51"])
    add("M25c1(false)", "Set F15e3=0; later M25c2 conditionally fades/reloads; not immediate universal cancellation.", ["M25c1", "M25c2:000c-0055"])
    add("Preview helper throws after global selection changed", "F15c0 error can suppress upkeep; selected reference can disagree with receiver map.", ["M2581:01ec-0205", "M25c2:0001"])
    add("Current map sample/score metadata changes", "Selection metadata cache is distinct from Play refresh and E034 legacy multiplier. No new score transformation established.", ["M25b2", "M223b:008e", "E034"])
    add("Star cache ruleset0 normalizedmods0 holds Single4.56789", "M1bd5 yields Abs(Double(Single)); display floors to two decimals then formats0.##★.", ["M1bc7:0035-003a", "M1bd5:0011", "M1bdb:01b4", "M2d7f"], {"kind":"display_star","single":4.56789,"result":4.56})
    add("Star cache normalized key absent, F1014=false", "M1bc7 returns0; M1bd7 can request calculation. Missing cache does not universally hide star line.", ["M1bc7:001e-0027", "M1bd7", "M1bdb:0148"])
    add("Star cache key0 Single-4.5, F1014=false", "DisplayAbs4.5 while M1bd7 treats key0 negative as needing refresh.", ["M1bd5", "M1bd7"], {"kind":"display_star","single":-4.5,"result":4.5})
    add("Star cache key0NaN", "M1bd5 AbsNaN; display blt.un skips star line; M1bd7 unordered branch is not negative.", ["M1bd7", "M1bdb:0148"])
    add("Cache version older than F0fbd[mode] cutoff", "M1bf2 stores -Abs(each cachedSingle), preserving value magnitude as stale marker.", ["M1bf2:0181-01d6"])
    add("Cache version20250106 vs20250107", "First reads Double dictionary then convertsSingle with normalization filter; second reads Int32/Single dictionary.", ["M1bf2:0100-0179"])
    add("Selected mods include only bits outside ordinary mask1370", "M08cf/M0805 normalize cachekey by AND; not every distinct rawmodinteger has separate starentry.", ["M08cf", "M0805", "M0803:0000"])
    add("Missing cache, scheduler available, not Play at worker execution", "M1c1e calls local M1bd0; ordinary factory M1bc6 constructs T011b overT072f; dictionary writeback queued, algorithm not recovered here.", ["M1bcf", "M1c1e:00a4,0115", "M1bd0:0002-003c", "M1bc6:002c"])
    add("Difficulty worker executes while currentmode2", "Waits with Thread.Sleep1000 until currentmode changes; no measured duration/cancellation promise.", ["M1c1e:0000-0018"])
    add("Background callback forA after selectedmap becomesB", "CacheA updated, pendingchartID removed, but no selected metadata refresh forA.", ["M1c23", "M25d1:0049-005c"])
    add("Preferred-difficulty candidates4.0/6.0, preference5.0, same run/ruleset", "Strict distance< retains first equidistant candidate; preference source remains profile getter M13d6.", ["M3eba:0054-00e4"], {"kind":"nearest","values":[4.0,6.0],"preference":5.0,"result":0})
    add("Browser Enter13, accepted initial-press operand, currentdifficultyvalid, no highlight/modal/specialmod branch", "F255a invokes M2584 then M2578; start guards still apply.", ["M3e97", "M3e84", "M2584"])
    add("Mouse release over same selected difficulty, inputenabled, no specialflag", "F255a triggersstart; different difficulty follows select/preferred-select path.", ["M3e94:0121-0131"])
    add("Ordinary Back callback reached", "M25c5 requests Menu0,false, not Rank and not universal selection deletion.", ["M25c5:003f-0041"])
    add("First disposal towardPlay", "M2577(true) runs once, saves group/ruleset, unregisters inputs; gameplay container optionally retained. Secondcleanup returns immediately.", ["M2577:0001-0008,0195,0234", "M2576"])
    add("F15e4/F15e5 defaultnull under direct managed inventory", "No direct writer to these fields found; retain conditional preview-thread branches without inventing an active producer.", ["field_sites F15e4/F15e5", "M25c1", "M2577:0215"])
    return cases


def extract(path):
    archive_bytes = Path(path).read_bytes()
    assert sha(archive_bytes) == prior.ARCHIVE
    with zipfile.ZipFile(path) as archive:
        raw = archive.read("osu!.exe")
        members = len(archive.infolist())
    assert sha(raw) == prior.EXE
    prior.METHODS = METHODS
    prior.FIELDS = {0x04000000+n for n in FIELDS}
    prior.CALLS = {0x06000000+n for n in CALLS}
    prior.SLOTS = SLOTS
    out = prior.extract(path)
    out.update(expedition=43, research_source=SOURCE,
               input_bytes={"archive":len(archive_bytes),"exe":len(raw),"zip_members":members},
               watched_fields=[hex(0x04000000+n) for n in FIELDS],
               watched_calls=[hex(0x06000000+n) for n in CALLS])
    pe = dnfile.dnPE(data=raw)
    tables = pe.net.mdtables
    fowners = {f.row_index:n for n,t in enumerate(tables.TypeDef,1) for f in t.FieldList}
    referenced = {int(t,16)&0xffffff for m in out["methods"].values()
                  for _,_,t in m.get("token_sites",[]) if int(t,16)>>24 == 4} | set(FIELDS)
    for rid in referenced:
        row = tables.Field.rows[rid-1]
        out["fields"][hex(0x04000000+rid)] = {
            "owner":hex(0x02000000+fowners[rid]),"name":str(row.Name),
            "signature":row.Signature.value.hex(),"static":row.Flags.fdStatic,
            "has_default":row.Flags.fdHasDefault,"has_rva":row.Flags.fdHasFieldRVA}
    out["ancestry"] = {}
    for start in [0x04d6,0x082c,0x03ad,0x059a,0x0258,0x011b]:
        rid = start
        chain = []
        while rid:
            row = tables.TypeDef.rows[rid-1]
            chain.append({"token":hex(0x02000000+rid),"name":str(row.TypeName),
                "methods":[{"token":hex(0x06000000+m.row_index),"name":str(m.row.Name),
                    "signature":m.row.Signature.value.hex(),"virtual":m.row.Flags.mdVirtual,
                    "new_slot":m.row.Flags.mdNewSlot} for m in row.MethodList]})
            rid = row.Extends.row_index if row.Extends and row.Extends.table.name == "TypeDef" else 0
        out["ancestry"][hex(0x02000000+start)] = chain
    out["enums"] = {}
    for rid in [0x06e0,0x0a0a,0x0b4b,0x0b57,0x0b58]:
        row = tables.TypeDef.rows[rid-1]
        ids = {f.row_index for f in row.FieldList}
        out["enums"][hex(0x02000000+rid)] = {
            "name":str(row.TypeNamespace)+"."+str(row.TypeName),
            "values":{str(c.Parent.row.Name):{"field":hex(0x04000000+c.Parent.row_index),
                "value":int.from_bytes(c.Value.value,"little",signed=True)}
                for c in tables.Constant if c.Parent.table.name=="Field" and c.Parent.row_index in ids}}
    out["method_specs"] = {hex(0x2b000000+rid):{
        "method_table":tables.MethodSpec.rows[rid-1].Method.table.name,
        "method_rid":tables.MethodSpec.rows[rid-1].Method.row_index,
        "instantiation":tables.MethodSpec.rows[rid-1].Instantiation.value.hex()}
        for rid in [0xce,0xcf,0xd0,0x124]}
    decoder = StaticStrings(pe)
    out["string_decoder"] = decoder.provenance
    out["strings"] = {}
    out["string_sites"] = []
    # Decode only the bounded selection/metadata/cache methods, not broad global
    # initialization, gameplay loading, alternate-mode details or calculator code.
    string_methods = set(range(0x2571,0x25cd)) | {
        0x0759,0x19c0,0x1bc1,0x1bdb,0x1be0,0x1bfb,0x1c16,0x2c62,0x2c86,0x2c89,
        0x2c8c,0x2ca6,0x2ca7,0x3ed2}
    for rid in sorted(string_methods):
        body = read_method_body_from_bytes(pe.get_data(tables.MethodDef.rows[rid-1].Rva))
        ins = list(body.instructions)
        for j,i in enumerate(ins):
            if getattr(i.operand,"value",None)==0x060049b0 and j and ins[j-1].opcode.name=="ldc.i4":
                identifier = ins[j-1].operand
                decoded = decoder.decode(identifier)
                if rid==0x1bc1 and decoded["text"] not in ["AudioFilename","PreviewTime","Title","Artist",
                    "Creator","Version","BeatmapID","BeatmapSetID","HPDrainRate","CircleSize",
                    "OverallDifficulty","ApproachRate","Mode"]:
                    continue
                out["strings"][str(identifier)] = decoded
                out["string_sites"].append([hex(0x06000000+rid),i.offset-body.header_size,identifier])
    root = Path(__file__).resolve().parents[1]
    authority = ["02/static-control-flow.json","03/request-guards.json","04/initialization.json",
        "05/slider-timing.json","23/aggregate-policy.json","27/gameplay-completion.json",
        "33/score-accounting.json","34/chart-score-multiplier.json","42/rank-results.json"]
    # Historical names are resolved explicitly; a missing authority is an error.
    out["authority_inputs"] = {name:sha((root/"evidence"/name).read_bytes()) for name in authority}
    out["collector_sources"] = {name:sha((root/"tools"/name).read_bytes()) for name in [
        "selectplay_evidence.py","circle_hitsound_evidence.py","beatmap_init_evidence.py","hitsound_resource_evidence.py"]}
    repo = root.parent
    names = ["reconstructed/CircleGame/Program.cs","reconstructed/CircleGame/Gameplay/Beatmap.cs",
             "re/qa/qa13/README.md","re/qa/qa13/e042-coverage.json"]
    out["read_only_sources"] = {name:sha((repo/name).read_bytes()) for name in names}
    out["reference_cases"] = reference_cases()
    out["verification_development"] = {
        "setup": "First extraction stopped at a misspelled historical E005 evidence path after static decoding; corrected to 05/slider-timing.json. A misnamed optional historical checker was subsequently replaced by its actual unchanged check_slider_timing.py invocation.",
        "literal_check": "Initial new-checker assertion incorrectly excluded M46ad from all of M2578. Original alternate-mode branches do call it; only the ordinary terminal branch uses M46ae. Narrowed assertion to offsets>=0311; retained all alternate branches in evidence. Five preliminary opcode/offset transcription errors were corrected against literal CIL before passing.",
        "archive_audit": "First full audit stopped comparing constants in M25ad:0008/0041: both are original ldc.r8 NaN, which is unequal to itself in Python. Compare nonfinite categories explicitly while independently checking the entire original body hash. Serialize nonfinite constants as strings for strict JSON.",
        "classification": "Research-tool development failures only; no original execution, reconstruction behavior failure or historical evidence correction."}
    out["limits"] = [
        "CONFIRMED STATIC original instructions; all original reference cases unexecuted",
        "Direct inventories exclude reflection/native/external mutation and arbitrary aliasing",
        "Original build metadata remains historically unauthenticated; scheduler/UI/native preview unmeasured",
        "Star cache/display/local producer entry recovered; full difficulty algorithm deliberately not reconstructed",
        "No proprietary asset bytes published; no CLR/game/GPU/audio execution",
        "Historical1911 QA checks not rerun; E02017 original-runtime cases remain NOT RUN",
        "No reconstruction changes, R16, E044 or QA14"]
    pe.close()
    return json_safe(out)


if __name__ == "__main__":
    text = json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2,allow_nan=False)+"\n"
    Path(sys.argv[2]).parent.mkdir(parents=True,exist_ok=True)
    Path(sys.argv[2]).write_text(text,encoding="utf-8",newline="\n")
