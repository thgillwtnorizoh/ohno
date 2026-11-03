#!/usr/bin/env python3
"""Independent E036 literal instruction assertions, not a replacement parser."""
import json
import pathlib
import sys

def check(d):
    assert d['archive_sha256']=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
    assert d['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
    assert d['scan']=={'decoded':19659,'zero_rva':1739,'errors':[]}
    def row(m):return d['methods'][hex(0x6000000+m)]
    def site(m,o,op,t):assert [o,op,hex(t)] in row(m)['token_sites'],(hex(m),hex(o),op,hex(t))
    def branch(m,o,op,t):assert [o,op,[t]] in row(m)['branches'],(hex(m),hex(o))
    def operation(m,o,op):assert [o,op,None] in row(m)['operations']
    r=row(0x2902)
    assert (r['owner'],r['signature'],r['virtual'],r['code_bytes'])==('0x2000556','200002',False,38)
    assert r['body_sha256']=='3d34c695021f9f3e85f3614df524072bde01394675edb95e030d44de9b0df46b'
    assert r['token_sites']==[[1,'ldfld','0x4001805'],[9,'ldfld','0x4001805'],
        [14,'isinst','0x200067f'],[22,'ldfld','0x4001805'],[27,'isinst','0x200081f']]
    branch(0x2902,6,'brfalse.s',0x24);branch(0x2902,0x13,'brtrue.s',0x24)
    operation(0x2902,0x21,'cgt.un');assert r['returns']==[0x23,0x25]
    assert r['constants']==[[0x24,'ldc.i4.1',None]]
    assert not any(op.startswith(('call','st')) for o,op,t in r['token_sites'])
    calls=[x for x in d['direct_callers'] if x[3]=='0x6002902']
    assert calls==[['0x60028f7',o,'call','0x6002902'] for o in [0x1631,0x1a79,0x1cc5]]
    for o in [0x1638,0x1a80]:operation(0x28f7,o,'or')
    assert [0x1496,'ldc.i4.s',-113] in row(0x28f7)['constants']
    assert [0x155a,'ldc.i4.4',None] in row(0x28f7)['constants']
    for o,t in [(0x164e,0x1870),(0x1658,0x28ef),(0x1aa4,0x1871),(0x1aae,0x28f1),
                (0x1ba6,0x1872),(0x1bb7,0x3e57),(0x1bbf,0x28f0)]:site(0x28f7,o,'callvirt',0x6000000+t)
    for m,o,t in [(0x48df,0xd,0x45eb),(0x45eb,0x20,0x45ec),
                  (0x48e0,0x17,0x142d),(0x48e1,9,0x5ea)]:site(m,o,'newobj' if m in [0x48df,0x48e0,0x48e1] else 'call',0x6000000+t)
    for m,o in [(0x45ec,0x25),(0x45ec,0x4c),(0x142d,0x192),(0x142d,0x1a4),(0x3201,0x38),(0x3201,0x59),(0x3e57,0xc),(0x3e57,0x1c)]:site(m,o,'stfld',0x4002525)
    site(0x3e56,2,'call',0x6003e4e);site(0x3e4e,1,'ldfld',0x4002525)
    site(0x28d2,0xcb,'callvirt',0xa0010b2);site(0x28d2,0xd7,'stfld',0x4001805)
    assert [x for x in d['field_sites'] if x[3]=='0x4001805' and x[2].startswith('st')]==[['0x60028d2',0xd7,'stfld','0x4001805']]
    site(0x28f5,0xb4,'call',0x60028c5);site(0x28f5,0xd4,'call',0x60028f7)
    site(0x28f5,0x15c,'call',0x60028db);site(0x28f5,0x269,'callvirt',0x60028e9)
    site(0x28db,0x90,'callvirt',0x6003e57)
    assert [0x50,'ldc.i4.m1',None] in row(0x28db)['constants']
    assert [0x8f,'ldc.i4.1',None] in row(0x28db)['constants']
    branch(0x28db,0x86,'br.s',0x95);branch(0x28db,0x9b,'bge.s',0xbe)
    site(0x28db,0xb1,'ldfld',0x400086e);site(0x28db,0xb7,'ldfld',0x4002523)
    branch(0x28db,0xbc,'blt.s',0x88)
    site(0x1150,0x36a,'callvirt',0x6003e56);branch(0x1150,0x355,'beq.s',0x371)
    branch(0x1150,0x36f,'brfalse.s',0x3c6)
    for o in [0x378,0x3cd]:site(0x1150,o,'callvirt',0x6000ad2)
    # No ordinary type replaces these virtual property slots.
    for slot in ['0x6003e56','0x6003e57']:
        assert {x['owner'] for x in d['slots'][slot]}=={'0x2000828','0x200052c','0x2000553','0x2000747'}
    shared=0
    for path in sorted((pathlib.Path(__file__).resolve().parents[1]/'evidence').glob('*/*.json')):
        if path.parent.name=='36':continue
        try:old=json.loads(path.read_text(encoding='utf-8'))
        except (ValueError,UnicodeError):continue
        for tok,v in old.get('methods',{}).items():
            if tok in d['methods'] and 'body_sha256' in v:
                assert v['body_sha256']==d['methods'][tok]['body_sha256'],(path,tok)
                shared+=1
    print(f"PASS E036: {len(d['methods'])} methods; 19659 bodies; literal helper/parser/constructor/break/consumer relationships; {shared} historical body hashes")

if __name__=='__main__':check(json.loads(pathlib.Path(sys.argv[1]).read_text(encoding='utf-8')))
