#!/usr/bin/env python3
"""Read-only archive audit. Does not execute or extract the reference game.

Usage: python audit_reference.py /path/osubackup.zip > audit.json
Requires pefile==2024.8.26 and dnfile==0.18.0.
"""
import hashlib
import json
import re
import sys
import zipfile

import dnfile


def digest(data, algorithm="sha256"):
    return hashlib.new(algorithm, data).hexdigest()


def audit(path):
    raw = open(path, "rb").read()
    with zipfile.ZipFile(path) as archive:
        bad = archive.testzip()
        if bad is not None:
            raise ValueError("CRC failure: " + bad)
        names = archive.namelist()
        if len(names) != len(set(names)):
            raise ValueError("Duplicate archive paths require manual review")
        config = archive.read("osu!.cfg").decode("utf-8-sig")
        expected = dict(re.findall(r"^h_(.*?) = ([0-9a-f]{32})\s*$", config, re.M))
        files = []
        for info in archive.infolist():
            if info.is_dir():
                continue
            data = archive.read(info)
            row = {"path": info.filename, "bytes": len(data), "sha256": digest(data),
                   "md5": digest(data, "md5")}
            if info.filename in expected:
                row["config_md5"] = expected[info.filename]
                row["config_md5_matches"] = row["md5"] == expected[info.filename]
            files.append(row)
        pe = dnfile.dnPE(data=archive.read("osu!.exe"))
        versions = {}
        for group in pe.FileInfo:
            for node in group:
                for table in getattr(node, "StringTable", []):
                    versions.update({k.decode(): v.decode() for k, v in table.entries.items()})
        assemblies = []
        for row in pe.net.mdtables.Assembly:
            assemblies.append({"name": str(row.Name), "version":
                [row.MajorVersion, row.MinorVersion, row.BuildNumber, row.RevisionNumber]})
        cert = pe.OPTIONAL_HEADER.DATA_DIRECTORY[4]
        result = {
            "archive_sha256": digest(raw), "archive_bytes": len(raw),
            "entry_count": len(names), "crc_test": "PASS", "files": files,
            "config": dict(line.split(" = ", 1) for line in config.splitlines() if " = " in line),
            "config_hash_count": len(expected),
            "config_matches": sum(r.get("config_md5_matches", False) for r in files),
            "beatmap_paths": [n for n in names if n.lower().endswith((".osu", ".osz"))],
            "replay_paths": [n for n in names if n.lower().endswith(".osr")],
            "exe_metadata": {
                "machine": hex(pe.FILE_HEADER.Machine),
                "optional_header_magic": hex(pe.OPTIONAL_HEADER.Magic),
                "clr_flags": pe.net.struct.Flags,
                "clr_metadata_version": pe.net.metadata.struct.Version.rstrip(b"\0").decode(),
                "assemblies": assemblies, "version_resource": versions,
                "certificate_table_file_offset": cert.VirtualAddress,
                "certificate_table_bytes": cert.Size,
                "authenticode_verification": "NOT RUN; directory presence does not prove validity",
            },
        }
        pe.close()
        return result


if __name__ == "__main__":
    print(json.dumps(audit(sys.argv[1]), indent=2, sort_keys=True))
