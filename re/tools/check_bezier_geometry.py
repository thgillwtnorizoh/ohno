#!/usr/bin/env python3
"""E32 evidence assertions, not a curve implementation or original execution."""
import json
from pathlib import Path
import sys
j=json.loads(Path(sys.argv[1]).read_text()); m=j['methods']
assert j['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert len(m)==22
assert j['fields']['0x4001aa6']['signature']=='0608'
for f in ['0x4001aa8','0x4001aa9']: assert j['fields'][f]['signature']=='061d11aa08'
assert m['0x6002d93']['body_sha256']=='930601257e3623324cad3c4734da09d782fcf699f373ee6b23f7f9e9e8e6c6cc'
assert m['0x6002d92']['body_sha256']=='823031c3e49cd72be2028b53370cbbb2885feade4bb82081c000db1252935597'


def row(r): return m[hex(0x6000000+r)]
def site(r,o,op,t): assert [o,op,hex(t)] in row(r)['token_sites'],(hex(r),hex(o),op)
def branch(r,o,op,ds): assert [o,op,ds] in row(r)['branches'],(hex(r),hex(o),op)
def const(r,o,op,v): assert [o,op,v] in row(r)['constants'],(hex(r),hex(o),op)
branch(0x144a,0xb4,'switch',[0xce,0x1f8,0x7a2,0x56a])
# Version priorities, forward-adjacent separators, two-point bypass and samplers.
branch(0x144a,0x213,'ble',[0x3da]); branch(0x144a,0x3eb,'ble',[0x4ae])
site(0x144a,0x242,'call',0x6004efb); branch(0x144a,0x226,'bge.s',[0x249])
branch(0x144a,0x25e,'bne.un',[0x549]); site(0x144a,0x271,'callvirt',0xa000ac9)
branch(0x144a,0x280,'bne.un',[0x324]); site(0x144a,0x295,'newobj',0x600110c)
branch(0x144a,0x336,'bge.s',[0x378])
for o,t in [(0x33a,0x2d64),(0x37a,0x2d62),(0x44e,0x2d62),(0x4f6,0x2d62)]: site(0x144a,o,'call',0x6000000+t)
for o in [0x3c5,0x499,0x541]: site(0x144a,o,'stfld',0x4000a34)
site(0x144a,0x306,'stfld',0x4000a35)
# Uniform de Casteljau N*50 excludes the final t==1 sample.
const(0x2d64,0x15,'ldc.i4.s',50); branch(0x2d64,0x96,'blt.s',[0x20])
site(0x2d64,0x61,'call',0x6004edb)
# Adaptive second differences use squared length; unordered also passes ble.un.
site(0x2d62,1,'newobj',0x6002d8f); site(0x2d62,6,'callvirt',0x6002d93)
site(0x2d90,0x34,'call',0x6004eca); const(0x2d90,0x39,'ldc.r4',0.25)
branch(0x2d90,0x3e,'ble.un.s',[0x42])
for o,f in [(8,0x1aa7),(0x14,0x1aa6),(0x25,0x1aa8),(0x3a,0x1aa9)]:site(0x2d8f,o,'stfld',0x4000000+f)
site(0x2d91,0x6e,'call',0x6004efd); site(0x2d91,0x78,'call',0x6004f03)
site(0x2d92,0x14,'call',0x6002d91);const(0x2d92,0x59,'ldc.r4',0.25)
site(0x2d92,0x78,'call',0x6004efd);site(0x2d92,0x86,'call',0x6004efd)
site(0x2d93,0x40,'call',0x6002d90);site(0x2d93,0x4a,'call',0x6002d92)
for o in [0xa7,0xae]:site(0x2d93,o,'callvirt',0xa00122d)
site(0x2d93,0xce,'callvirt',0xa0007d8);site(0x2d93,0xd3,'callvirt',0xa000392)
# Shared adjustment, storage, downstream duration: no separate Bezier timing model.
for o,f in [(0x8d0,0xbdd),(0xae1,0xbdc),(0xaf6,0xbde),(0xa69,0xa33),(0x1378,0x2524)]:site(0x144a,o,'stfld',0x4000000+f)
const(0x144a,0xa12,'ldc.r8',0.0001);branch(0x144a,0xa1c,'ble.un.s',[0xa70])
# Pin all shared bodies to already-published E06, without re-running its expedition.
old=json.loads((Path(__file__).parents[1]/'evidence/06/linear-geometry.json').read_text())['methods']
shared=0
for token,r in m.items():
    if token in old and 'body_sha256' in old[token]:
        assert r['body_sha256']==old[token]['body_sha256'],token
        shared+=1
assert shared>0
assert row(0x2d93)['owner']=='0x20005e7'
print(f'PASS E32: 22 method bodies, version/separator/sampler/flatness/subdivision/output/tail sites; {shared} E06 body hashes agree; no game execution')
