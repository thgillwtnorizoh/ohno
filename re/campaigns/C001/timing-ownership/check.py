#!/usr/bin/env python3
"""Validate targeted original relationships/chronology against reused evidence."""
import json,pathlib,sys
H=pathlib.Path(__file__).resolve().parent;R=H.parents[3]
j=json.loads(pathlib.Path(sys.argv[1]).read_text());m=j['methods'];assert len(m)==40
checks=[]
def site(rid,off,op,token):
 assert [off,op,hex(token)] in m[hex(0x6000000+rid)]['token_sites'],(hex(rid),hex(off));checks.append((hex(rid),hex(off)))
for rid,off,op,t in [
 (0x2240,0xa3,'ldfld',0x40013f8),(0x2240,0xbb,'call',0x6003b45),
 (0x3b45,0xc8,'call',0x6003d52),(0x3b45,0xd5,'stsfld',0x4002367),
 (0x223b,0x8e,'callvirt',0x6001bc1),(0x223b,0x15b,'call',0x6002240),
 (0x223b,0x16e,'ldfld',0x40013f8),(0x223b,0x182,'callvirt',0x60028a2),
 (0x223b,0x18f,'callvirt',0x60028f5),(0x223b,0x24d,'call',0x600224d),
 (0x28f5,0xd4,'call',0x60028f7),(0x28f5,0x269,'callvirt',0x60028e9),
 (0x28e9,0x1b,'call',0x60028ea),(0x28ea,0x2e,'callvirt',0x600144a),
 (0x28ab,9,'callvirt',0x6001be3),
 (0x1bc1,0x366,'call',0x60028ff),(0x1bc1,0x10ce,'stfld',0x400100f)]:site(rid,off,op,t)
assert j['external_refs']['0xa000877']['name'].endswith('::Sort')
assert [0x1b0,'brfalse.s',[0x1b8]] in m['0x60028be']['branches']
assert [0x2c,'brfalse.s',[0x34]] in m['0x6003b66']['branches']
assert len(j['ownership_relations'])==12 and all(x['passed'] for x in j['ownership_relations'])
assert j['configuration_slot']['ordinary_extends']=='0x2000556'
assert not any(d['owner']=='0x20000e8' for d in j['configuration_slot']['declarations'])
overlaps=[]
for f in ['04/initialization.json','05/slider-timing.json','37/circle-hitsound.json','39/slider-hitsounds.json','40/slider-continuous-audio.json']:
 old=json.loads((R/'re/evidence'/f).read_text())
 for token,v in m.items():
  if token in old['methods']:
   assert v['body_sha256']==old['methods'][token]['body_sha256'],(f,token);overlaps.append([f,token])
print(json.dumps(dict(status='PASS',selected_methods=40,sites=len(checks),ownership_relations=12,historical_hash_comparisons=len(overlaps),limits='Original-static only; no original execution'),sort_keys=True))
