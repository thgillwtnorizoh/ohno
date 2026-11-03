#!/usr/bin/env python3
"""Assert E035 original instruction facts; does not execute or simulate HP."""
import json
import sys
from pathlib import Path
j = json.loads(Path(sys.argv[1]).read_text())
m = j['methods']
assert j['archive_sha256'] == '7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
assert j['exe_sha256'] == '6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert len(m) == 61
assert j['scan'] == {'decoded':19659,'zero_rva':1739,'errors':0,'watched_fields':15,'helper_tokens':17}
def row(r): return m[hex(0x6000000+r)]
def site(r,o,op,t): assert [o,op,hex(t)] in row(r)['token_sites'], (hex(r),hex(o),op,hex(t))
def c(r,o,op,v): assert [o,op,v] in row(r)['constants'], (hex(r),hex(o),op,v)
def ar(r,o,op): assert [o,op] in row(r)['arithmetic_sites'], (hex(r),hex(o),op)
def br(r,o,op,v): assert [o,op,[v]] in row(r)['branches'], (hex(r),hex(o),op,hex(v))
# Fresh ordinary factory, slot resolution, three distinct Double fields/accessors.
site(0xab4,7,'newobj',0x6002963);site(0xab4,12,'stfld',0x40005f5)
for get,setter,f in [(0x2964,0x2965,0x1856),(0x2966,0x2967,0x1857),(0x2968,0x2969,0x1858)]:
    site(get,1,'ldfld',0x4000000+f);site(setter,2,'stfld',0x4000000+f)
    assert j['fields'][hex(0x4000000+f)]['signature']=='060d'
    writers=[(x[0],x[1]) for x in j['direct_field_accesses'] if x[3]==hex(0x4000000+f) and x[2] in ['stfld','stsfld']]
    assert writers==[(hex(0x6000000+setter),2)]
for tid,slot,target in [(0x253,0xab4,0xab4),(0x253,0xab6,0x1142),(0x253,0xac7,0x1146),
 (0x253,0xac6,0xac6),(0x253,0xac8,0xac8),(0xe8,0x125a,0x28b3),(0x47b,0x227d,0x227d)]:
    assert j['ordinary_resolution'][f'{hex(0x2000000+tid)}:{hex(0x6000000+slot)}']['method']==hex(0x6000000+target)
# Reset and absolute/add/sub state mutation order, floor/cap asymmetry.
for o,t in [(10,0x2967),(25,0x2969),(40,0x2965)]: site(0x297d,o,'call',0x6000000+t)
for o,v in [(1,200.0),(16,200.0),(31,0.0)]:c(0x297d,o,'ldc.r8',v)
site(0x2979,0x14,'call',0xa0002d4);site(0x2979,0x19,'call',0xa0002d1)
site(0x2979,0x1e,'call',0x6002967);site(0x2979,0x25,'call',0x6002969)
site(0x297a,0x2d,'call',0x6002969);site(0x297a,0x49,'call',0x6002967)
site(0x297b,0x1f,'call',0x6002969);site(0x297b,0x49,'call',0x6002967)
assert not any(x[2]=='0xa0002d4' for x in row(0x297a)['token_sites'])
# All direct rate writers, and late eligibility writer, are independently pinned.
rate={(x[0],x[1])for x in j['direct_field_accesses']if x[3]=='0x4001855' and x[2]=='stfld'}
assert rate=={('0x6000c62',0x71),('0x6000c62',0xaa),('0x6001142',0x15),('0x6001142',0x4e),('0x6004607',0x16),('0x6004607',0x59)}
assert [(x[0],x[1])for x in j['direct_field_accesses']if x[3]=='0x40013c4' and x[2]=='stfld']==[('0x600226b',0xf0b)]
assert all(v['body_sha256']for v in j['unselected_writer_methods'].values())
# Setup and complete calibration retry graph; no original loop executed here.
site(0xaaf,0xd,'callvirt',0x6000ab4);site(0xaaf,0x25,'callvirt',0x6000ab6)
site(0x1142,0x10,'callvirt',0x6001150);site(0x1142,0x15,'stfld',0x4001855)
c(0x1142,0x44,'ldc.r8',.75);site(0x1142,0x4e,'stfld',0x4001855)
c(0x1150,0,'ldc.r8',.05);site(0x1150,0xcc,'callvirt',0x600297d)
for o in [0x3a,0x64,0x8e,0xb8]:site(0x1150,o,'callvirt',0x600125a)
for o in [0x1d3,0x256]:site(0x1150,o,'callvirt',0x600297a)
for o,v in [(0x2b3,64),(0x2cd,None),(0x2f9,8192),(0x372,1028),(0x3c7,1024)]:
    c(0x1150,o,'ldc.i4.8' if v is None else ('ldc.i4.s' if v==64 else 'ldc.i4'),v)
for o in [0x2b6,0x2cf,0x2ff,0x378,0x3cd]:site(0x1150,o,'callvirt',0x6000ad2)
for o,v in [(0x20e,.96),(0x337,.96),(0x39d,1.07),(0x3b3,1.03),(0x41b,.94),(0x42d,1.01),(0x443,1.01),(0x482,.96),(0x494,1.02),(0x4aa,1.01)]:c(0x1150,o,'ldc.r8',v)
br(0x1150,0x4ba,'brtrue',0xbf);br(0x1150,0x209,'bgt.un.s',0x21e)
site(0x1150,0x3ec,'stfld',0x4001529);site(0x1150,0x458,'callvirt',0x6002968)
# Passive drain uses signed Int32 subtraction before r8, old eligibility/current-spinner flag.
site(0xac4,0x10,'callvirt',0x6000ac7);site(0xac4,0x23,'callvirt',0x6002972)
site(0x1146,0xe,'ldfld',0x40013c4);site(0x1146,0x21,'ldfld',0x4001855)
site(0x1146,0x30,'ldsfld',0x4002358);site(0x1146,0x35,'ldsfld',0x400235a)
ar(0x1146,0x3a,'sub');ar(0x1146,0x3b,'conv.r8');c(0x1146,0x4b,'ldc.r4',.25)
site(0x1146,0x52,'callvirt',0x600297a);site(0x1146,0x58,'call',0x6000ac7)
site(0x2275,0x72,'stsfld',0x400136f);c(0x2275,0x69,'ldc.i4.8',None)
site(0x226b,0x9cf,'callvirt',0x60028e3);site(0x226b,0x9da,'callvirt',0x6000ac4)
site(0x226b,0xf0b,'stfld',0x40013c4);site(0x3b25,0x43,'stsfld',0x400235a)
br(0x226b,0xec7,'blt.s',0xf0a);br(0x226b,0xef0,'bgt.s',0xf0a)
br(0x226b,0xef9,'bge.s',0xf0a);br(0x226b,0xf00,'brfalse.s',0xf0a)
# Break selection, equality, queue provenance and default end-time stub comparison.
br(0x2f05,0x1ef,'blt.s',0x20a);br(0x2f05,0x1fd,'bgt.s',0x20a)
site(0x2f05,0x3e6,'stsfld',0x4001b97);br(0x2271,0x7c,'blt.s',0x53)
br(0x226f,0x1f,'ble.s',0x30);c(0x226f,0x2c,'ldc.i4.8',None)
site(0x2243,0x170,'callvirt',0x60028de);site(0x2243,0x17a,'ldfld',0x4002524)
site(0x28de,2,'call',0x60028dc);site(0x28dc,0x18,'newobj',0x600396e)
site(0x24c2,0x11,'ldflda',0x4002524);site(0x24c2,0x17,'ldfld',0x4002524)
# Result-specific HP deltas. Original score clauses are not reinterpreted.
for o,v in [(0x3fe,-4.0),(0x407,-15.0),(0x410,-28.0),(0x46e,-6.0),(0x477,-25.0),(0x480,-40.0),
 (0x582,3.0),(0x652,4.0),(0x722,4.0),(0x75a,3.2),(0x763,.4),(0x76c,.4),
 (0x800,17.6),(0x809,2.2),(0x812,2.2),(0x889,6.0),(0xb13,14.0),(0xb23,6.0),
 (0xb6b,1.7),(0xb86,1.7),(0xba4,2.0)]:c(0xad2,o,'ldc.r8',v)
br(0xad2,0x12e9,'ble.un.s',0x12f9);site(0xad2,0x12f2,'callvirt',0x600297b)
site(0xad2,0x1301,'callvirt',0x600297a);site(0xad2,0x1404,'callvirt',0x600226c)
site(0xad2,0x12c3,'callvirt',0x6002979);br(0xad2,0x119b,'brfalse',0x12c8)
# Smoothed state, not failure-tested target: preserve distinct time/scale sources.
site(0x2972,0x1ee,'call',0x6002964);site(0x2972,0x1f4,'call',0x6002966)
site(0x2972,0x248,'ldsfld',0x400235d);site(0x2972,0x24f,'ldsfld',0x4002c71)
c(0x2972,0x2cd,'ldc.r8',4.0);c(0x2972,0x321,'ldc.r8',6.0)
# Original failure/recovery continuation and zero-inclusive test.
br(0x226c,0x29,'bgt.un.s',0x93);site(0x226c,0x8e,'callvirt',0x600227d)
site(0xacc,0x25,'callvirt',0x600297b);c(0xacc,0xb,'ldc.r8',160.0)
site(0xacc,0x6e,'stsfld',0x4001376);site(0xacb,0x60,'stsfld',0x400136a)
br(0x226b,0xb02,'ble.un.s',0xb34)
assert not any(x[2]=='0x600226c' for x in row(0x1146)['token_sites'])
shared=0
for n in ['05','10','15','17','18','19','24','25','26','27','28','29']:
    for p in (Path(__file__).parents[1]/'evidence'/n).glob('*.json'):
        old=json.loads(p.read_text()).get('methods',{})
        for token,r in m.items():
            if token in old and old[token].get('body_sha256'):
                assert r['body_sha256']==old[token]['body_sha256'],(str(p),token)
                shared+=1
print(f'PASS E035:61 methods; three exact state writers; calibration/drain/break/delta/recovery sites; {shared} historical body hashes; no original runtime')
