#!/usr/bin/env python3
"""Checks original-binary evidence edges, never constructs gameplay objects."""
import json
import sys
j=json.load(open(sys.argv[1]));m=j['methods']
assert j['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
assert len(m)==66

def site(method,offset,op,target):
    assert [offset,op,target] in m[method]['token_sites'],(method,hex(offset),op,target)
site('0x6002237',1,'call','0x6002c63')
site('0x6002c63',0,'ldsfld','0x4001a0f')
site('0x6002237',6,'stfld','0x40013f8')
for off,target in [(0x656,'0x600223b'),(0x663,'0x600223c')]:site('0x6002237',off,'ldvirtftn',target)
for off,target in [(0xff,'0x6002258'),(0x182,'0x60028a2'),(0x18f,'0x60028f5')]:site('0x600223b',off,'call' if off==0xff else 'callvirt',target)
site('0x6001ee1',1,'newobj','0x60020cd');site('0x60020d4',1,'newobj','0x600426d')
site('0x60028f5',0xd4,'call','0x60028f7')
site('0x60028f7',0x161,'callvirt','0x6001c05')
site('0x60028f7',0x1631,'call','0x6002902');site('0x60028f7',0x1a79,'call','0x6002902')
for off,target in [(0x164e,'0x6001870'),(0x1aa4,'0x6001871'),(0x1ba6,'0x6001872')]:site('0x60028f7',off,'callvirt',target)
for method,target in [('0x600426e','0x60020dc'),('0x600426f','0x600424e'),('0x6004270','0x6001d6d')]:
    assert any(op=='newobj' and t==target for _,op,t in m[method]['token_sites'])
for tok in ['0x60028bc','0x6001870','0x6001871','0x6001872']:assert m[tok]['abstract'] and m[tok]['rva']=='0x0'
for base,override in [('0x60028bc','0x60020d4'),('0x6001870','0x600426e'),('0x6001871','0x600426f'),('0x6001872','0x6004270')]:
    assert m[base]['name']==m[override]['name'] and m[base]['signature']==m[override]['signature']
    assert m[override]['virtual'] and not m[override]['new_slot']
site('0x60028d2',0xcb,'callvirt','0xa0010b2')
for tok in ['0x60028ef','0x60028f0','0x60028f1']:site(tok,4,'callvirt','0x60028d2')
assert j['enums']['0x2000b58']['values']['Osu']==0
assert j['enums']['0x2000b24']['values']['HitObjects']==64
assert {k:j['enums']['0x2000746']['values'][k] for k in ['Normal','Slider','Spinner']}=={'Normal':1,'Slider':2,'Spinner':8}
site('0x60045ec',0x17,'stfld','0x4002523');site('0x60045ec',0x1e,'stfld','0x4002524')
site('0x6003201',0x17,'stfld','0x400252c');site('0x6003201',0x31,'stfld','0x4002524')
site('0x60028ea',0x2e,'callvirt','0x600144a')
print('PASS: pinned input, 66 methods, selected-map flow, callback wiring, factory slots, object constructors, registration and initial fields')
