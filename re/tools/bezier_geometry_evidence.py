#!/usr/bin/env python3
"""E32 read-only focused original-CIL extraction; no curve/game execution.
PYTHONPATH=<dependencies> python re/tools/bezier_geometry_evidence.py archive.zip
Dependencies: dnfile0.18.0, pefile2024.8.26, dncil1.0.2 (same as E06).
"""
import json
import sys
import slider_timing_evidence as prior

METHODS = [0x142d,0x144a,0x110c,0x110d,0x2d62,0x2d64,
           0x2d8f,0x2d90,0x2d91,0x2d92,0x2d93,
           0x4ec9,0x4eca,0x4ecb,0x4ed2,0x4edb,0x4efb,0x4efd,
           0x4efe,0x4f00,0x4f01,0x4f03]

def extract(path):
    prior.METHODS = METHODS
    j = prior.extract(path)
    j['expedition'] = 32
    j['scoped_field_accesses'] = {}
    for token, method in j['methods'].items():
        for offset, op, field in method.get('token_sites', []):
            if field.startswith('0x4'):
                j['scoped_field_accesses'].setdefault(field, []).append([token, offset, op])
    j['limits'] += ['Field inventory covers selected direct helper bodies only; not whole-assembly exhaustive',
                    'No numerical curve emulator, original CLR execution or runtime validation',
                    'Method tokens/branches/constants are evidence sites, not a full disassembly']
    return j

if __name__ == '__main__':
    print(json.dumps(extract(sys.argv[1]), sort_keys=True, allow_nan=False))
