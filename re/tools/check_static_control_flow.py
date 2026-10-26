#!/usr/bin/env python3
"""Check pinned Expedition 02 evidence, not gameplay correctness."""
import json
import sys
j = json.load(open(sys.argv[1]))
m = j['selected_methods']
assert j['archive_sha256'] == '7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert m['0x6002c49']['calls'] == [[0, 'call', '0x60023b9']]
assert j['modes']['values']['Play'] == 2 and j['modes']['values']['Unknown'] == 10
assert m['0x60046b0']['switches'][0]['targets'][2] == 0x2dd
constructors = {c[2] for c in m['0x60046b0']['calls'] if c[1] == 'newobj'}
assert {'0x60023f0','0x6000715','0x6004ab9','0x600222e'} <= constructors
assert [c[0] for c in m['0x60046b0']['calls'] if c[2] == '0xa00186e'] == [0x55a]
assert [c[0] for c in m['0x60046b0']['calls'] if c[2] == '0x6001ab1'] == [0x564]
assert {c['caller'] for c in j['commit_callers']} == {'0x600468e','0x60046ef','0x6004711'}
assert j['scan']['decoded_bodies'] == 19659 and j['scan']['zero_rva_methods'] == 1739
assert not j['scan']['decode_errors'] and not j['scan']['invalid_branch_targets']
print('PASS: pinned input, entry edge, enum, Play switch, four constructors, registration order, callers and scan coverage')
