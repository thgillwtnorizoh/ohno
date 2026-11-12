#!/usr/bin/env python3
"""Independent E043 archive/metadata audit and literal relationship checks.

Does not import the E043 extractor or execute original managed code. The finite
projections below are separately written Python controls, not original behavior
measurements. The unchanged E038 independent key reducer checks selected strings.
"""
import hashlib
import json
import math
from pathlib import Path
import struct
import sys
import zipfile

ARCHIVE = "7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738"
EXE = "6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d"


def sha(data):
    return hashlib.sha256(data).hexdigest()


def relationships(data):
    assert data["expedition"] == 43
    assert data["research_source"] == "b440e4e601798be9157fbaa1274a25fcac130232"
    assert (data["archive_sha256"],data["exe_sha256"]) == (ARCHIVE,EXE)
    assert data["scan"] == {"decoded":19659,"zero_rva":1739,"errors":[]}
    assert data["input_bytes"] == {"archive":78546961,"exe":4541728,"zip_members":18}
    count = 0

    def m(rid):
        return data["methods"][hex(0x06000000+rid)]

    def token(rid,offset,opcode,operand):
        nonlocal count
        assert [offset,opcode,hex(operand)] in m(rid)["token_sites"], (hex(rid),hex(offset),opcode,hex(operand))
        count += 1

    def constant(rid,offset,opcode,value=None):
        nonlocal count
        assert [offset,opcode,value] in m(rid)["constants"], (hex(rid),hex(offset),opcode,value)
        count += 1

    def branch(rid,offset,opcode,target):
        nonlocal count
        assert [offset,opcode,[target]] in m(rid)["branches"], (hex(rid),hex(offset),opcode,hex(target))
        count += 1

    assert [x["token"] for x in data["ancestry"]["0x20004d6"]] == ["0x20004d6","0x20005b4","0x20008fb","0x2000397"]
    assert m(0x2571)["signature"] == "20010112a304"
    token(0x2571,0x3f,"call",0x06002bf2)
    token(0x46b0,0x361,"call",0x060019b8)
    token(0x46b0,0x38c,"newobj",0x060019b6)
    token(0x46b0,0x3a7,"newobj",0x06002571)
    token(0x46b0,0x564,"callvirt",0x06001ab1)
    assert any(x["method"]=="0x60025c0" and not x["new_slot"] for x in data["slots"]["0x6001ab1"])
    assert any(x["method"]=="0x60025a2" and not x["new_slot"] for x in data["slots"]["0x6001ab2"])
    assert any(x["method"]=="0x60025bf" and not x["new_slot"] for x in data["slots"]["0x6002bf4"])
    token(0x2571,0x95,"stfld",0x0400159a)
    token(0x2571,0x272,"newobj",0x06003e6a)
    token(0x2571,0x1df3,"call",0x06002c64)
    token(0x2571,0x1e21,"call",0x060025af)
    token(0x2571,0x1e50,"stsfld",0x040015ae)
    token(0x25bf,0x326,"callvirt",0x06002cf0)
    token(0x25bf,0x332,"callvirt",0x06004416)
    token(0x25a2,0x1b5,"call",0x060025c2)
    token(0x25a2,0x1bb,"call",0x06002588)
    # The only direct global-map stores are in the original setter.
    stores = [x for x in data["field_sites"] if x[2]=="stsfld" and x[3]=="0x4001a0f"]
    assert stores == [["0x6002c64",0x21,"stsfld","0x4001a0f"]]
    token(0x2c63,0,"ldsfld",0x04001a0f)
    token(0x2c64,0x15,"callvirt",0x06001c12)
    token(0x2c64,0x1b,"call",0x060038b9)
    token(0x2c64,0x21,"stsfld",0x04001a0f)
    token(0x1c12,0xd,"stfld",0x04000fc0)
    token(0x2581,0x11,"call",0x06002c64)
    token(0x2581,0x6e,"call",0x060025b2)
    token(0x2581,0x156,"callvirt",0x06001bc1)
    token(0x2581,0x1ec,"call",0x060025c1)
    token(0x3ebc,0x3e,"stfld",0x0400254b)
    token(0x3ec7,0x40,"stfld",0x0400254d)
    token(0x3ea7,0x68,"ldfld",0x04000fd6)
    token(0x3ea7,0x73,"ldfld",0x04000fd6)
    token(0x3ea7,0xb5,"stfld",0x04001942)
    branch(0x3eba,0xa8,"bge.un.s",0xaf)  # strict improvement; ties retain earlier
    token(0x3eba,0x38,"callvirt",0x060013d6)
    token(0x3eba,0x98,"callvirt",0x06001bd9)
    token(0x25f3,0x48,"call",0x06002c63)
    token(0x25f3,0x5f,"callvirt",0x06003ebb)
    token(0x3ed1,0x2f,"stfld",0x0400253a)
    token(0x3ed1,0x55,"callvirt",0x06002fee)
    token(0x3ee5,0x51,"stfld",0x0400253b)
    # UI start has a different helper from immediate M46ad admission.
    token(0x2578,0x311,"call",0x06002577)
    constant(0x2578,0x318,"ldc.i4",1000)
    token(0x2578,0x31d,"call",0x060046ae)
    # Earlier alternate-mode branches call M46ad; only the ordinary terminal
    # branch uses this queued overload. Do not assert absence across all modes.
    assert not any(t=="0x60046ad" for off,_,t in m(0x2578)["token_sites"] if off>=0x311)
    for off,field in [(8,0x2cd6),(0xf,0x2cd7),(0x16,0x2cd8)]:
        token(0x46ae,off,"stfld",0x04000000+field)
    assert not any(t=="0x6002c63" for _,_,t in m(0x46ae)["token_sites"])
    token(0x4711,0x94,"stsfld",0x04002c6c)
    token(0x4711,0x9f,"call",0x060046b0)
    token(0x2237,1,"call",0x06002c63)
    token(0x2237,6,"stfld",0x040013f8)
    token(0x32e8,0x18,"call",0x060046ad)
    assert not any(t=="0x6002c64" for _,_,t in m(0x32d3)["token_sites"])
    # Collection / path / database provenance and filename-only comparison.
    token(0x0759,0xe3,"stsfld",0x040019ff)
    token(0x2c85,0x42,"call",0x06002c6d)
    token(0x2c85,0x47,"call",0x06002c86)
    token(0x2ca5,0x77,"stsfld",0x04001a01)
    token(0x2c8c,0x1b4,"stfld",0x04000feb)
    token(0x2c8c,0x219,"callvirt",0x06001bc1)
    token(0x2c8c,0x25e,"call",0x06002c6f)
    token(0x2c8c,0x265,"stsfld",0x04001a0b)
    assert [x for x in m(0x1bf4)["token_sites"] if x[2].startswith("0x400")] == [[1,"ldfld","0x4000feb"],[7,"ldfld","0x4000feb"]]
    token(0x2c88,0x240,"call",0x06002c65)
    # Preview uses live selector, while the receiver stores its own map reference.
    token(0x25c1,0x2a,"call",0x06003b47)
    token(0x3b45,0xb7,"stfld",0x040024ad)
    token(0x3b3e,0xb,"ldfld",0x04000fff)
    token(0x3b0e,0xb,"ldfld",0x040024ad)
    assert not any(x[2].startswith("st") for x in data["field_sites"] if x[3] in ["0x40015e4","0x40015e5"])
    # Star provenance, stopping at the factory and virtual calculator entry.
    assert data["fields"]["0x4000fea"]["signature"] == "061d1512826902080c"
    token(0x1bd5,2,"call",0x06001bda)
    token(0x1bd5,0xc,"call",0x06001bc7)
    token(0x1bdb,0xbd,"call",0x06001bd5)
    token(0x1bdb,0x1b4,"call",0x06002d7f)
    branch(0x1bd7,0x35,"bge.un.s",0x41)
    token(0x1bd0,2,"call",0x06001bc6)
    token(0x1bd0,0x26,"callvirt",0x060008d9)
    token(0x1bc6,0x2c,"newobj",0x06000801)
    token(0x0801,2,"call",0x06003696)
    token(0x1c1e,0xa4,"call",0x06001bd0)
    token(0x1c23,0x1c,"call",0x06001bc9)
    token(0x25d1,0x5c,"call",0x060025b2)
    constant(0x0803,0,"ldc.i4",1370)
    expected_strings = {2064463696:"osu!.db",2064528221:"BeatmapDirectory",2064528036:"Songs",
        2064566862:"Star Rating: {4:0.##}★",2064566717:"{0:0}-{1:0} ({2:0})"}
    for identifier,value in expected_strings.items():
        assert data["strings"][str(identifier)]["text"] == value
    for row in data["reference_cases"]:
        assert row["status"] == "RECONSTRUCTED STATIC PREDICTION" and row["evidence"]
    assert [c["id"] for c in data["reference_cases"]] == [f"P{i+1:02}" for i in range(len(data["reference_cases"]))]
    return count


def projections(data):
    """Independent finite dataflow controls; no original game code invocation."""
    count = 0
    for case in data["reference_cases"]:
        p = case["projection"]
        if p is None:
            continue
        kind = p["kind"]
        if kind == "construction":
            actual = "loading19" if p["loading"] or (p["previous"] != 19 and p["count"]==0) else "T04d6"
        elif kind == "runs":
            actual = []
            run = 0
            last = None
            for folder in p["folders"]:
                if folder != last:
                    run += 1
                actual.append(run)
                last = folder
        elif kind == "setter":
            clean = p["old"] is not None and p["old"] != p["new"]
            actual = {"global":p["old"] if p["throws"] else p["new"],"clean":clean}
        elif kind == "selection_callback":
            actual = p["old"] if p["candidate"] is None else p["candidate"]
        elif kind == "start_guard":
            actual = p["selected"] and not p["random"] and p["phase"]==2
        elif kind == "capture":
            actual = p["at_init"]
        elif kind == "display_star":
            value = struct.unpack("<f",struct.pack("<f",p["single"]))[0]
            actual = math.floor(abs(value)*100.0)/100.0
        elif kind == "nearest":
            actual = min(range(len(p["values"])),key=lambda i:abs(p["values"][i]-p["preference"]))
        else:
            raise AssertionError(kind)
        assert actual == p["result"], (case["id"],actual,p["result"])
        count += 1
    return count


def archive_audit(data,path):
    import dnfile
    from dncil.cil.body.reader import read_method_body_from_bytes
    assert sha(Path(path).read_bytes()) == ARCHIVE
    with zipfile.ZipFile(path) as z:
        raw = z.read("osu!.exe")
    assert sha(raw) == EXE
    pe = dnfile.dnPE(data=raw)
    ts = pe.net.mdtables
    owners = {m.row_index:n for n,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    fowners = {f.row_index:n for n,t in enumerate(ts.TypeDef,1) for f in t.FieldList}
    for token,row in data["fields"].items():
        rid = int(token,16)&0xffffff
        original = ts.Field.rows[rid-1]
        assert row["owner"]==hex(0x02000000+fowners[rid])
        assert row["signature"]==original.Signature.value.hex()
        assert row["name"]==str(original.Name) and row["static"]==original.Flags.fdStatic
        assert row["has_default"]==original.Flags.fdHasDefault
        assert row["has_rva"]==original.Flags.fdHasFieldRVA
    for key,chain in data["ancestry"].items():
        rid = int(key,16)&0xffffff
        for item in chain:
            assert item["token"] == hex(0x02000000+rid)
            original = ts.TypeDef.rows[rid-1]
            assert item["name"]==str(original.TypeName)
            assert item["methods"] == [{"token":hex(0x06000000+m.row_index),"name":str(m.row.Name),
                "signature":m.row.Signature.value.hex(),"virtual":m.row.Flags.mdVirtual,
                "new_slot":m.row.Flags.mdNewSlot} for m in original.MethodList]
            rid = original.Extends.row_index if original.Extends and original.Extends.table.name=="TypeDef" else 0
        assert rid==0
    watched = {int(t,16) for t in data["watched_fields"]}
    called = {int(t,16) for t in data["watched_calls"]}
    fields,callers,errors = [],[],[]
    audited = decoded = 0
    for rid,row in enumerate(ts.MethodDef,1):
        key = hex(0x06000000+rid)
        if not row.Rva:
            if key in data["methods"]:
                assert data["methods"][key]["rva"]=="0x0"
            continue
        try:
            body = read_method_body_from_bytes(pe.get_data(row.Rva))
        except Exception as error:
            errors.append([rid,str(error)])
            continue
        decoded += 1
        ins = list(body.instructions)
        tokens = [[i.offset-body.header_size,i.opcode.name,hex(i.operand.value)] for i in ins if hasattr(i.operand,"value")]
        fields.extend([[key,*site] for site in tokens if int(site[2],16) in watched])
        callers.extend([[key,*site] for site in tokens if int(site[2],16) in called])
        if key not in data["methods"]:
            continue
        record = data["methods"][key]
        assert record["owner"]==hex(0x02000000+owners[rid])
        assert record["signature"]==row.Signature.value.hex() and record["rva"]==hex(row.Rva)
        assert record["body_sha256"]==sha(pe.get_data(row.Rva,body.size))
        assert (record["virtual"],record["new_slot"])==(row.Flags.mdVirtual,row.Flags.mdNewSlot)
        assert record["token_sites"]==tokens
        constants = [[i.offset-body.header_size,i.opcode.name,i.operand] for i in ins if i.opcode.name.startswith("ldc.")]
        # NaN is unequal to itself. Compare its category explicitly; the raw body
        # hash above independently pins its exact payload bytes as well.
        def comparable(rows):
            def finite(v):
                if isinstance(v,float) and not math.isfinite(v):
                    return "NaN" if math.isnan(v) else "+Infinity" if v>0 else "-Infinity"
                return v
            return [[off,op,finite(v)] for off,op,v in rows]
        assert comparable(record["constants"])==comparable(constants), key
        branches = []
        starts = {i.offset-body.header_size for i in ins}
        for i in ins:
            op = i.opcode.name
            if op=="switch" or op.startswith(("br","beq","bne","ble","blt","bgt","bge","leave")):
                targets = i.operand if op=="switch" else [i.operand]
                targets = [n-body.header_size for n in targets]
                assert all(n in starts for n in targets)
                branches.append([i.offset-body.header_size,op,targets])
        assert record["branches"]==branches
        assert record["operations"]==[[i.offset-body.header_size,i.opcode.name] for i in ins if i.opcode.name in
            ["add","sub","mul","div","and","or","ceq","cgt","cgt.un","conv.r4","conv.r8","conv.i4","ldnull"]]
        indexed = {i.offset-body.header_size:j for j,i in enumerate(ins)}
        for method,offset,identifier in data["string_sites"]:
            if method!=key:
                continue
            j = indexed[offset]
            assert ins[j].opcode.name=="call" and ins[j].operand.value==0x060049b0
            assert j>0 and ins[j-1].opcode.name=="ldc.i4" and ins[j-1].operand==identifier
        audited += 1
    assert data["scan"]=={"decoded":decoded,"zero_rva":sum(not x.Rva for x in ts.MethodDef),"errors":errors}
    assert fields==data["field_sites"] and callers==data["direct_callers"]
    for token,candidates in data["slots"].items():
        original = ts.MethodDef.rows[(int(token,16)&0xffffff)-1]
        assert candidates==[{"method":hex(0x06000000+n),"owner":hex(0x02000000+owners[n]),
            "virtual":m.Flags.mdVirtual,"new_slot":m.Flags.mdNewSlot} for n,m in enumerate(ts.MethodDef,1)
            if str(m.Name)==str(original.Name) and m.Signature.value==original.Signature.value]
    for token,enum in data["enums"].items():
        ids = {f.row_index for f in ts.TypeDef.rows[(int(token,16)&0xffffff)-1].FieldList}
        assert enum["values"]=={str(c.Parent.row.Name):{"field":hex(0x04000000+c.Parent.row_index),
            "value":int.from_bytes(c.Value.value,"little",signed=True)}
            for c in ts.Constant if c.Parent.table.name=="Field" and c.Parent.row_index in ids}
    for token,spec in data["method_specs"].items():
        original = ts.MethodSpec.rows[(int(token,16)&0xffffff)-1]
        assert spec=={"method_table":original.Method.table.name,"method_rid":original.Method.row_index,
                      "instantiation":original.Instantiation.value.hex()}
    # Independent encrypted-string transform, including proven Unicode branch.
    from check_hitsound_resources import independent_key,i32
    key = independent_key()
    assert key==data["string_decoder"]["final_key"]
    source = next(r.data for r in pe.net.resources if str(r.name)==data["string_decoder"]["resource"]["name"])
    assert sha(source)==data["string_decoder"]["resource"]["sha256"]
    for identifier,record in data["strings"].items():
        at = i32(int(identifier)^-351960377^key^1516081623)
        assert at==record["offset"]
        keys = bytes(source[at+j]^((key>>(j*8))&255) for j in range(3))
        size = i32(struct.unpack_from("<I",source,at+3)[0]^at^-973073496^key)
        assert bool(size&0x80000000)==record["compressed"]
        assert bool(size&0x40000000)==record["byte_characters"]
        cipher = source[at+7:at+7+(size&0xfffffff)]
        assert sha(cipher)==record["cipher_sha256"]
        last = ((len(cipher)+11)^(keys[1]+7))&255
        state = keys[0]+(keys[2]<<8)+(last<<3)
        plain = []
        word = 0
        for j,byte in enumerate(cipher):
            if not j%2:
                state = (state*214013+2531011)&0xffffffff
                word = (state>>16)&65535
            plain.append((keys[1]^byte^(last+3)^(word&255))&255)
            word >>= 8
            last = byte
        if size&0x80000000:
            # E038 M49b5: length header byte order2,3,0,1; flag bits LSB-first.
            # Separate flag-group traversal, independent of collector's bit loop.
            length = int.from_bytes(bytes([plain[2],plain[3],plain[0],plain[1]]),"little")
            assert 0<=length<1_000_000
            expanded = bytearray(length)
            read,write = 4,0
            stopped = False
            while write<length and not stopped:
                flags = plain[read]
                read += 1
                for bit in range(8):
                    if write>=length:
                        break
                    if not flags&(1<<bit):
                        expanded[write] = plain[read]
                        write += 1
                        read += 1
                        continue
                    first,second = plain[read:read+2]
                    read += 2
                    amount = (first>>2)+3
                    distance = ((first<<8)|second)&1023
                    origin = write-distance
                    if origin<0:
                        stopped = True
                        break
                    for index in range(min(amount,length-write)):
                        expanded[write] = expanded[origin+index]
                        write += 1
            plain = expanded
        text = "".join(map(chr,plain)) if size&0x40000000 else bytes(plain).decode("utf-16le")
        assert text==record["text"]
    pe.close()
    return audited


def check(data,archive=None):
    literals = relationships(data)
    finite = projections(data)
    root = Path(__file__).resolve().parents[1]
    for name,digest in data["authority_inputs"].items():
        assert sha((root/"evidence"/name).read_bytes())==digest
    for name,digest in data["collector_sources"].items():
        assert sha((root/"tools"/name).read_bytes())==digest
    for name,digest in data["read_only_sources"].items():
        assert sha((root.parent/name).read_bytes())==digest
    shared = 0
    for path in sorted((root/"evidence").glob("*/*.json")):
        if path.parent.name=="43":
            continue
        old = json.loads(path.read_text(encoding="utf-8"))
        if not isinstance(old,dict):
            continue
        for token,method in old.get("methods",{}).items():
            if token in data["methods"] and "body_sha256" in method:
                assert method["body_sha256"]==data["methods"][token]["body_sha256"]
                shared += 1
    audited = archive_audit(data,archive) if archive else 0
    print(f'PASS E043: {len(data["methods"])} methods, {len(data["fields"])} fields, '
          f'{literals} literal relationships, {len(data["reference_cases"])} static predictions, '
          f'{finite} independent finite projections, {len(data["strings"])} decoded strings, '
          f'{shared} historical body comparisons, {audited} original-byte-audited bodies. '
          'No original CLR/game/UI/audio execution; historical1911 QA not rerun.')


if __name__ == "__main__":
    check(json.loads(Path(sys.argv[1]).read_text(encoding="utf-8")),sys.argv[2] if len(sys.argv)>2 else None)
