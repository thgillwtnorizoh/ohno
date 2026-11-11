#!/usr/bin/env python3
"""Independent E042 literal, original-byte and finite numeric projection checks.

Does not import the E042 collector, load original CLR code or deserialize images.
An optional Python CIL arithmetic walk checks the stated binary32 projection.
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


def single(value):
    return struct.unpack("<f", struct.pack("<f", value))[0]


def grade_instruction_walk(body, state):
    """Walk original decoded branch operands with explicit stable injected fields.

    This implements only M2b47's scalar subset in Python. It never invokes any
    original method. M2b44 is substituted with its independently checked four-count
    sum. Generic op_Implicit is a supplied stable integer operand in this control.
    """
    instructions = list(body.instructions)
    positions = {i.offset: n for n, i in enumerate(instructions)}
    counts = state["counts"]
    record = dict(zip([0x196a, 0x1969, 0x196b, 0x196e], counts))
    record.update({0x1978: state["F1978"], 0x1970: state["mods"]})
    stack = []
    locals_ = {}
    cursor = 0
    calls = 0
    for _ in range(300):
        ins = instructions[cursor]
        op = ins.opcode.name
        arg = ins.operand
        target = getattr(arg, "value", None)
        cursor += 1
        if op == "ldarg.0":
            stack.append(record)
        elif op == "ldfld":
            stack.append(stack.pop()[target & 0xffffff])
        elif op.startswith("ldc.i4."):
            suffix = op.rsplit(".", 1)[1]
            stack.append(-1 if suffix == "m1" else int(suffix))
        elif op in ["ldc.i4", "ldc.i4.s", "ldc.r4", "ldc.r8"]:
            stack.append(arg)
        elif op.startswith("stloc."):
            locals_[int(op.rsplit(".", 1)[1])] = stack.pop()
        elif op.startswith("ldloc."):
            stack.append(locals_[int(op.rsplit(".", 1)[1])])
        elif op == "conv.r4":
            stack.append(single(stack.pop()))
        elif op == "conv.r8":
            stack.append(float(stack.pop()))
        elif op == "callvirt":
            assert target == 0x06002b44
            operand_record = stack.pop()
            stack.append(sum(operand_record[n] for n in [0x196b, 0x1969, 0x196a, 0x196e]))
            calls += 1
        elif op == "call":
            assert target == 0x0a0002ca  # supplied original generic conversion operand
        elif op == "div":
            denominator = stack.pop()
            numerator = stack.pop()
            assert denominator or numerator == 0
            stack.append(single(numerator / denominator) if denominator else math.nan)
        elif op in ["and", "cgt"]:
            right = stack.pop()
            left = stack.pop()
            stack.append(left & right if op == "and" else int(left > right))
        elif op in ["brtrue.s", "brfalse.s"]:
            truth = bool(stack.pop())
            if truth == (op == "brtrue.s"):
                cursor = positions[arg]
        elif op in ["bne.un.s", "ble.un.s", "bgt.un.s"]:
            right = stack.pop()
            left = stack.pop()
            unordered = any(isinstance(v, float) and math.isnan(v) for v in [left, right])
            comparison = {"bne.un.s": left != right, "ble.un.s": left <= right,
                          "bgt.un.s": left > right}[op]
            if unordered or comparison:
                cursor = positions[arg]
        elif op == "ret":
            assert len(stack) == 1 and calls == 2
            return stack[0]
        else:
            raise AssertionError((hex(ins.offset - body.header_size), op))
    raise AssertionError("finite grade walk did not return")


def check_relationships(data):
    assert data["expedition"] == 42
    assert (data["archive_sha256"], data["exe_sha256"]) == (ARCHIVE, EXE)
    assert data["research_source"] == "47277a8da8603af93b0c515f86aac2a1463ef51c"
    assert data["scan"] == {"decoded": 19659, "zero_rva": 1739, "errors": []}

    def method(rid):
        return data["methods"][hex(0x06000000 + rid)]

    def token(rid, offset, opcode, operand):
        assert [offset, opcode, hex(operand)] in method(rid)["token_sites"], (hex(rid), hex(offset), opcode, hex(operand))

    def field(rid, offset, opcode, operand):
        token(rid, offset, opcode, 0x04000000 + operand)

    def constant(rid, offset, opcode, value=None):
        assert [offset, opcode, value] in method(rid)["constants"], (hex(rid), hex(offset), opcode, value)

    def branch(rid, offset, opcode, destination):
        assert [offset, opcode, [destination]] in method(rid)["branches"], (hex(rid), hex(offset), opcode, hex(destination))

    def operation(rid, offset, opcode):
        assert [offset, opcode] in method(rid)["operations"], (hex(rid), hex(offset), opcode)

    assert [r["token"] for r in data["rank_ancestry"]] == ["0x20006a8", "0x20005b4", "0x20008fb", "0x2000397"]
    assert method(0x32d0)["signature"] == "20010112a304"
    token(0x32d0, 0x25, "call", 0x06002bf2)
    token(0x2bf2, 0x18, "call", 0x06004415)
    token(0x4415, 2, "call", 0x06001aae)
    assert not any(t == "0x40013c3" for _, _, t in method(0x32d0)["token_sites"])
    # Request / commit / construct / virtual activation are distinct operations.
    field(0x46ad, 0x22a, "stsfld", 0x2c6c)
    field(0x46b0, 0x68, "stsfld", 0x2c70)
    field(0x46b0, 0x72, "stsfld", 0x2c6d)
    token(0x46b0, 0xca, "callvirt", 0x06001ab0)
    token(0x46b0, 0x99, "ldftn", 0x060046df)
    token(0x46b0, 0x4ba, "newobj", 0x060032d0)
    field(0x46b0, 0x4bf, "stsfld", 0x2c6e)
    token(0x46b0, 0x564, "callvirt", 0x06001ab1)
    branch(0x46b0, 0x573, "bne.un.s", 0x57c)
    assert any(r["method"] == "0x60032db" and not r["new_slot"] for r in data["slots"]["0x6001ab1"])
    # Reference alias and post-selector cleanup mutations, without copying a record.
    field(0x0ace, 1, "ldsfld", 0x13c3)
    field(0x0ace, 6, "stfld", 0x05f3)
    token(0x0ace, 0x12, "callvirt", 0x06002b62)
    field(0x0acd, 7, "stfld", 0x1397)
    field(0x0acd, 0x1a, "stfld", 0x1978)
    field(0x0acd, 0x3d, "stfld", 0x197c)
    operation(0x0aa5, 0x3a, "ceq")
    token(0x2276, 0xfa, "callvirt", 0x06002b5a)
    token(0x2276, 0x10f, "callvirt", 0x06002277)
    required_combo_writes = [site for site in data["field_sites"]
                             if site[0] == "0x6001150" and site[2] == "stfld" and site[3] == "0x40005fd"]
    assert required_combo_writes == [["0x6001150", offset, "stfld", "0x40005fd"]
                                     for offset in [0xc1, 0x2a8, 0x3db]]
    assert data["writer_bodies"]["0x6001150"]["body_sha256"] == "389f0e15e00771308e8191e65444c89c55c571137ede52c5d60bcd25a7671039"
    token(0x2266, 0xcc, "callvirt", 0x06002274)
    field(0x2266, 0xd1, "stsfld", 0x13c3)
    token(0x2266, 0xe3, "call", 0x06002267)
    field(0x2267, 0x6a, "stfld", 0x196f)
    branch(0x2267, 0x2f3, "ble.s", 0x300)
    field(0x2267, 0x2fb, "stfld", 0x1978)
    field(0x32de, 1, "ldsfld", 0x13c3)
    field(0x32de, 6, "stfld", 0x1da9)
    field(0x32de, 0x11, "ldfld", 0x1978)
    token(0x32dd, 0x124, "call", 0x060032de)
    assert not any(t == "0x4001990" for n in range(0x32d0, 0x32ef) for _, _, t in method(n)["token_sites"])
    assert not any(op == "newobj" and t in ["0x6002b3d", "0x6002b3e", "0x6002b3f", "0x6002b40"]
                   for n in range(0x32d0, 0x32ef) for _, op, t in method(n)["token_sites"])
    # Complete base-grade branch discriminators, including unordered comparisons.
    assert method(0x2b47)["body_sha256"] == "612f291e51c2668a73fbc0a9425e28f9b00d2bc4475322c25bd18ec2f103b267"
    for offset in [8, 0x18]:
        token(0x2b47, offset, "callvirt", 0x06002b44)
    for offset in [0xe, 0x1e]:
        operation(0x2b47, offset, "div")
    field(0x2b47, 0x21, "ldfld", 0x1978)
    branch(0x2b47, 0x26, "brtrue.s", 0x2a)
    constant(0x2b47, 0x28, "ldc.i4.8")
    for offset, value in [(0x68, .9), (0x75, .01), (0xbe, .8), (0xd3, .9),
                          (0xe2, .7), (0xf7, .8), (0x106, .6)]:
        constant(0x2b47, offset, "ldc.r8", value)
    for offset, dest in [(0x71, 0xbc), (0xc7, 0xd1), (0xdc, 0xe0),
                         (0xeb, 0xf5), (0x100, 0x104), (0x10f, 0x113)]:
        branch(0x2b47, offset, "ble.un.s", dest)
    branch(0x2b47, 0x7e, "bgt.un.s", 0xbc)
    for offset, opcode in [(0x62, "ldc.i4.2"), (0x64, "ldc.i4.0"), (0xb8, "ldc.i4.3"),
                           (0xba, "ldc.i4.1"), (0xde, "ldc.i4.4"), (0x102, "ldc.i4.5"),
                           (0x111, "ldc.i4.6"), (0x113, "ldc.i4.7")]:
        constant(0x2b47, offset, opcode)
    assert not any(t in ["0x4001990", "0x400197c", "0x4001975", "0x4001985"]
                   for _, _, t in method(0x2b47)["token_sites"])
    assert not any(op.startswith("stfld") for _, op, _ in method(0x2b47)["token_sites"])
    enum = {k: v["value"] for k, v in data["enums"]["0x2000241"]["values"].items()}
    assert enum == dict(zip(["XH", "SH", "X", "S", "A", "B", "C", "D", "F", "N"], range(10)))
    assert data["enums"]["0x2000b57"]["values"]["Hidden"]["value"] == 8
    assert data["enums"]["0x2000b57"]["values"]["Flashlight"]["value"] == 1024
    # Actual selected record, getter and direct field consumers.
    branch(0x32db, 5, "brtrue.s", 0x17)
    token(0x32db, 0x24, "callvirt", 0x060032dd)
    token(0x32db, 0x352, "callvirt", 0x060032d7)
    token(0x32db, 0x159, "call", 0x06004391)
    token(0x32db, 0x358, "call", 0x06002bf3)
    field(0x4391, 1, "stsfld", 0x2a7c)
    field(0x4391, 0xe, "stsfld", 0x2a7f)
    token(0x2bf3, 6, "call", 0x060046b1)
    token(0x46b1, 7, "call", 0x060046b2)
    field(0x46b2, 0x17, "stsfld", 0x2c6b)
    field(0x32db, 0x183, "stfld", 0x1dae)
    field(0x32d7, 0x7d, "stfld", 0x1da9)
    token(0x32d7, 0x13, "callvirt", 0x06002b4b)
    for offset, rid in [(0x1c0, 0x196a), (0x206, 0x1969), (0x24c, 0x196b),
                        (0x292, 0x196c), (0x2d8, 0x196d), (0x31e, 0x196e), (0x92a, 0x1975)]:
        field(0x32d7, offset, "ldfld", rid)
    token(0x32d7, 0x9bc, "callvirt", 0x06002b42)
    constant(0x32d7, 0x9c1, "ldc.r4", 100.0)
    operation(0x32d7, 0x9c6, "mul")
    field(0x32d7, 0xb29, "ldfld", 0x197c)
    token(0x32d7, 0xb9d, "callvirt", 0x060032da)
    for offset in [0x11, 0xfe, 0x119]:
        token(0x32da, offset, "callvirt", 0x06002b47)
    branch(0x32da, 0x104, "bge", 0x200)
    field(0x32df, 5, "ldfld", 0x0fe2)
    field(0x32df, 0x57, "ldfld", 0x0fdc)
    field(0x32df, 0xaf, "ldfld", 0x197d)
    field(0x32df, 0xba, "ldflda", 0x196f)
    token(0x32d5, 0x15, "callvirt", 0x06002cf0)
    token(0x32d5, 0x21, "callvirt", 0x06002215)
    token(0x32d5, 0x2c, "callvirt", 0x06002cf0)
    token(0x2cf0, 0x4e1, "callvirt", 0x06004737)
    assert any(r["method"] == "0x600451a" and not r["new_slot"] for r in data["slots"]["0x6004737"])
    assert any(r["method"] == "0x6001545" and not r["new_slot"] for r in data["slots"]["0x6004737"])
    assert any(r["method"] == "0x6001544" and not r["new_slot"] for r in data["slots"]["0x600094a"])
    token(0x1541, 0x15, "callvirt", 0x0600094a)
    field(0x1544, 2, "stfld", 0x051e)
    token(0x1545, 1, "call", 0x0600451a)
    token(0x1545, 0xbe, "callvirt", 0x060031f5)
    assert method(0x31f5)["abstract"] and method(0x31f5)["rva"] == "0x0"
    # Departure controls and record retention use pending mode at disposal.
    token(0x32e1, 0x28, "call", 0x060046ad)
    token(0x32e8, 0x18, "call", 0x060046ad)
    constant(0x32e8, 0x16, "ldc.i4.5")
    constant(0x32e1, 0x26, "ldc.i4.2")
    field(0x32d3, 0xc2, "ldsfld", 0x2c6c)
    field(0x32d3, 0xd9, "stsfld", 0x13c3)
    token(0x32d3, 0x129, "call", 0x06002bf6)
    branch(0x32e7, 0x11, "bge", 0xf7)
    field(0x32e7, 0x23, "stfld", 0x1da2)
    field(0x32e7, 0x34, "stfld", 0x1dae)
    expected_strings = {
        2064573791: "{0:00000000}", 2064570052: "{0:0000000}",
        2064560137: "{0}x", 2064569622: "{0:0.00}%", 2064572537: "ranking-",
        2064547909: "hit300", 2064547912: "hit100", 2064547964: "hit50",
        2064551871: "hit300g", 2064551853: "hit100k", 2064547926: "hit0",
        2064569636: "ranking-maxcombo", 2064569615: "ranking-accuracy",
        2064569590: "ranking-perfect", 2064569445: "Beatmap by ",
        2064569419: "Played by {0} on {1}.", 2064528945: "applause",
    }
    for identifier, value in expected_strings.items():
        assert data["strings"][str(identifier)]["text"] == value
    cases = data["reference_cases"]
    assert [c["id"] for c in cases] == [f"R{n:02}" for n in range(1, 79)]
    assert all(c["status"] == "RECONSTRUCTED STATIC PREDICTION" and c["evidence"] for c in cases)
    grades = [c for c in cases if c["kind"] == "grade"]
    assert [c["expected"]["grade"] for c in grades] == [2, 3, 3, 4, 5, 4, 6, 6, 7, 4, 7, 8, 8, 0, 0, 1, 1, 3, 7, 2, 4, 5, 5, 6, 5, 2]
    return grades


def audit_archive(data, path, grade_cases):
    assert sha(Path(path).read_bytes()) == ARCHIVE
    with zipfile.ZipFile(path) as archive:
        raw = archive.read("osu!.exe")
    assert sha(raw) == EXE
    import dnfile
    from dncil.cil.body.reader import read_method_body_from_bytes
    pe = dnfile.dnPE(data=raw)
    tables = pe.net.mdtables
    owners = {m.row_index: n for n, t in enumerate(tables.TypeDef, 1) for m in t.MethodList}
    fowners = {f.row_index: n for n, t in enumerate(tables.TypeDef, 1) for f in t.FieldList}
    for token, value in data["methods"].items():
        rid = int(token, 16) & 0xffffff
        row = tables.MethodDef.rows[rid - 1]
        assert value["signature"] == row.Signature.value.hex()
        assert value["owner"] == hex(0x02000000 + owners[rid])
        assert value["rva"] == hex(row.Rva)
        assert (value["virtual"], value["new_slot"]) == (row.Flags.mdVirtual, row.Flags.mdNewSlot)
    for token, value in data["fields"].items():
        rid = int(token, 16) & 0xffffff
        row = tables.Field.rows[rid - 1]
        assert value["signature"] == row.Signature.value.hex()
        assert value["owner"] == hex(0x02000000 + fowners[rid])
        if "name" in value:
            assert value["name"] == str(row.Name) and value["static"] == row.Flags.fdStatic
        if "has_default" in value:
            assert value["has_default"] == row.Flags.fdHasDefault
            assert value["has_rva"] == row.Flags.fdHasFieldRVA
    for item in data["rank_ancestry"]:
        row = tables.TypeDef.rows[(int(item["token"], 16) & 0xffffff) - 1]
        assert item["name"] == str(row.TypeName)
        assert item["methods"] == [
            {"token": hex(0x06000000 + m.row_index), "name": str(m.row.Name),
             "signature": m.row.Signature.value.hex(), "virtual": m.row.Flags.mdVirtual,
             "new_slot": m.row.Flags.mdNewSlot} for m in row.MethodList]
    classes = {int(item["token"], 16) & 0xffffff for item in data["rank_ancestry"]} | {0x059e}
    assert data["method_impls"] == [
        {"class": hex(0x02000000 + m.Class.row_index),
         "body": [m.MethodBody.table.name, m.MethodBody.row_index],
         "declaration": [m.MethodDeclaration.table.name, m.MethodDeclaration.row_index]}
        for m in tables.MethodImpl if m.Class.row_index in classes]
    watched = {int(x[3], 16) for x in data["field_sites"]}
    call_targets = {int(x[3], 16) for x in data["direct_callers"]}
    fields = []
    callers = []
    count = 0
    for rid, row in enumerate(tables.MethodDef, 1):
        if not row.Rva:
            continue
        body = read_method_body_from_bytes(pe.get_data(row.Rva))
        instructions = list(body.instructions)
        method_token = hex(0x06000000 + rid)
        for ins in instructions:
            value = getattr(ins.operand, "value", None)
            site = [method_token, ins.offset - body.header_size, ins.opcode.name, hex(value)] if value is not None else None
            if value in watched:
                fields.append(site)
            if value in call_targets:
                callers.append(site)
        if method_token not in data["methods"]:
            continue
        recorded = data["methods"][method_token]
        assert sha(pe.get_data(row.Rva, body.size)) == recorded["body_sha256"]
        assert recorded["token_sites"] == [[i.offset - body.header_size, i.opcode.name, hex(i.operand.value)]
                                           for i in instructions if hasattr(i.operand, "value")]
        assert recorded["constants"] == [[i.offset - body.header_size, i.opcode.name, i.operand]
                                         for i in instructions if i.opcode.name.startswith("ldc.")]
        branches = []
        starts = {i.offset - body.header_size for i in instructions}
        for ins in instructions:
            op = ins.opcode.name
            if op == "switch" or op.startswith(("br", "beq", "bne", "ble", "blt", "bgt", "bge", "leave")):
                targets = ins.operand if op == "switch" else [ins.operand]
                targets = [target - body.header_size for target in targets]
                assert all(target in starts for target in targets)
                branches.append([ins.offset - body.header_size, op, targets])
        assert branches == recorded["branches"]
        assert recorded["operations"] == [
            [i.offset - body.header_size, i.opcode.name] for i in instructions
            if i.opcode.name in ["add", "sub", "mul", "div", "and", "or", "ceq", "cgt",
                                 "cgt.un", "conv.r4", "conv.r8", "conv.i4", "ldnull"]]
        count += 1
    assert fields == data["field_sites"] and callers == data["direct_callers"]
    for token, candidates in data["slots"].items():
        row = tables.MethodDef.rows[(int(token, 16) & 0xffffff) - 1]
        actual = [{"method": hex(0x06000000 + n), "owner": hex(0x02000000 + owners[n]),
                   "virtual": r.Flags.mdVirtual, "new_slot": r.Flags.mdNewSlot}
                  for n, r in enumerate(tables.MethodDef, 1)
                  if str(r.Name) == str(row.Name) and r.Signature.value == row.Signature.value]
        assert actual == candidates
    for token, enum in data["enums"].items():
        row = tables.TypeDef.rows[(int(token, 16) & 0xffffff) - 1]
        ids = {f.row_index for f in row.FieldList}
        actual = {str(c.Parent.row.Name): {"field": hex(0x04000000 + c.Parent.row_index),
                  "value": int.from_bytes(c.Value.value, "little", signed=True)}
                  for c in tables.Constant if c.Parent.table.name == "Field" and c.Parent.row_index in ids}
        assert actual == enum["values"]
    grade_body = read_method_body_from_bytes(pe.get_data(tables.MethodDef.rows[0x2b46].Rva))
    for case in grade_cases:
        assert grade_instruction_walk(grade_body, case["initial_state"]) == case["expected"]["grade"], case["id"]
    # Independently reduced E038 key and byte transform, without collector decoder.
    from check_hitsound_resources import independent_key, i32
    key = independent_key()
    assert key == data["string_decoder"]["final_key"]
    table = next(r.data for r in pe.net.resources if str(r.name) == data["string_decoder"]["resource"]["name"])
    for identifier, record in data["strings"].items():
        offset = i32(int(identifier) ^ -351960377 ^ key ^ 1516081623)
        assert offset == record["offset"]
        keys = bytes(table[offset + i] ^ ((key >> (8 * i)) & 255) for i in range(3))
        length = i32(struct.unpack_from("<I", table, offset + 3)[0] ^ offset ^ -973073496 ^ key)
        assert not length & 0x80000000  # selected entries are uncompressed
        assert bool(length & 0x40000000) == record["byte_characters"]
        cipher = table[offset + 7:offset + 7 + (length & 0xfffffff)]
        assert sha(cipher) == record["cipher_sha256"]
        last = ((len(cipher) + 11) ^ (keys[1] + 7)) & 255
        state = keys[0] + (keys[2] << 8) + (last << 3)
        decoded = []
        word = 0
        for index, byte in enumerate(cipher):
            if index % 2 == 0:
                state = (state * 214013 + 2531011) & 0xffffffff
                word = (state >> 16) & 65535
            decoded.append((keys[1] ^ byte ^ (last + 3) ^ (word & 255)) & 255)
            word >>= 8
            last = byte
        text = "".join(map(chr, decoded)) if length & 0x40000000 else bytes(decoded).decode("utf-16le")
        assert text == record["text"]
    assert data["strings"]["2064569235"]["byte_characters"] is False
    assert data["strings"]["2064569235"]["text"] == "▼ Online Ranking ▼"
    pe.close()
    with zipfile.ZipFile(path) as archive:
        for recorded in data["presentation_resource_metadata"]:
            raw = archive.read(recorded["assembly"])
            assert sha(raw) == recorded["assembly_sha256"]
            dll = dnfile.dnPE(data=raw)
            resource = next(r for r in dll.net.resources if str(r.name) == recorded["resource_set"])
            entries = {str(e.name): e for e in resource.data.entries}
            assert len(entries) == recorded["entry_count"]
            start = resource.data.struct.DataSectionOffset
            assert start == recorded["data_section_offset"]
            for name, value in recorded["keys"].items():
                if value is None:
                    assert name not in entries
                    continue
                entry = entries[name]
                at = entry.struct.DataOffset
                end = min([e.struct.DataOffset for e in entries.values() if e.struct.DataOffset > at]
                          + [len(resource.data._data) - start])
                assert value["type"] == entry.type_name
                assert value["data_offset"] == at and value["serialized_span_bytes"] == end - at
                assert value["serialized_span_sha256"] == sha(resource.data._data[start + at:start + end])
            dll.close()
    return count


def check(data, archive=None):
    grades = check_relationships(data)
    root = Path(__file__).resolve().parents[1]
    for name, expected in data["authority_inputs"].items():
        assert sha((root / "evidence" / name).read_bytes()) == expected
    for name, expected in data["collector_sources"].items():
        assert sha((root / "tools" / name).read_bytes()) == expected
    for name, expected in data["reconstruction_read_only"].items():
        assert sha((root.parent / name).read_bytes()) == expected
    shared = 0
    for path in sorted((root / "evidence").glob("*/*.json")):
        if path.parent.name == "42":
            continue
        old = json.loads(path.read_text(encoding="utf-8"))
        if not isinstance(old, dict):
            continue
        for token, method in old.get("methods", {}).items():
            if token in data["methods"] and "body_sha256" in method:
                assert method["body_sha256"] == data["methods"][token]["body_sha256"]
                shared += 1
    audited = audit_archive(data, archive, grades) if archive else 0
    print(f'PASS E042 {len(data["methods"])} methods, {len(data["fields"])} field declarations, '
          f'{len(data["reference_cases"])} static predictions, {len(grades)} binary32 grade projections, '
          f'{len(data["strings"])} decoded strings, {shared} historical body comparisons, '
          f'{audited} independently byte-audited bodies. No original CLR/game/rendering execution.')


if __name__ == "__main__":
    check(json.loads(Path(sys.argv[1]).read_text(encoding="utf-8")), sys.argv[2] if len(sys.argv) > 2 else None)
