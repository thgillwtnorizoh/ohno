#!/usr/bin/env python3
"""Independent literal relationship gates for C001 static packet; no original execution."""
import json,sys
from pathlib import Path
p=json.loads(Path(sys.argv[1]).read_bytes());m=p['methods'];checks=0

def yes(ok,note):
 global checks
 assert ok,note
 checks+=1

def site(rid,off,op,target):yes([off,op,hex(target)] in m[hex(0x6000000+rid)]['token_sites'],(hex(rid),hex(off),op,hex(target)))
def const(rid,off,op,value):yes([off,op,value] in m[hex(0x6000000+rid)]['constants'],(hex(rid),hex(off),value))
def branch(rid,off,op,*dest):yes(any(b['offset']==off and b['opcode']==op and b['targets']==list(dest) for b in m[hex(0x6000000+rid)]['branches']),(hex(rid),hex(off),op,dest))

yes(p['exe_sha256']=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d','pinned executable')
yes(p['scan']=={'decoded':19659,'zero_rva':1739,'errors':[]},'complete direct body scan')
for rid,target in [(0x9e3,0xa000612),(0x9e4,0xa000615),(0x9e5,0xa000616)]:
 site(rid,1,'ldsfld',0x4002c2b);site(rid,6,'call',target);branch(rid,0xd,'brtrue.s',0x11)
 branch(rid,0x14,'bge.s' if rid==0x9e5 else 'bge.un.s',0x26)
 branch(rid,0x28,'ble.s' if rid==0x9e5 else 'ble.un.s',0x3a)
for rid,target in [(0x9e3,0xa000613),(0x9e4,0xa0001e5)]:site(rid,0x3b,'call',target);branch(rid,0x40,'brfalse.s',0x52)
for off,field in [(6,0xb52),(0x11,0xb53),(0x1c,0xb54),(0x27,0xb55)]:
 site(0x1387,off,'stfld',0x4000000+field);const(0x1387,off-5,'ldc.r4',5.0)
const(0x1387,0x2d,'ldc.r8',1.4);const(0x1387,0x3c,'ldc.r8',1.0)
site(0x1bc1,0x1f7,'callvirt',0xa0002f6);branch(0x1bc1,0x1fc,'brtrue',0xf8f)
site(0x1bc1,0x209,'callvirt',0xa0003b9);site(0x1bc1,0x215,'call',0xa000321)
branch(0x1bc1,0x23d,'bge.s',0x247);const(0x1bc1,0x24a,'ldc.i4.s',24)
site(0x1bc1,0x28c,'callvirt',0xa00023d);site(0x1bc1,0x2e8,'call',0xa000135)
site(0x28f7,0x217,'call',0xa000135);branch(0x28f7,0x233,'ble',0x175)
for off,target in [(0x164e,0x1870),(0x1aa4,0x1871),(0x1ba6,0x1872),(0x1ce2,0x1873)]:site(0x28f7,off,'callvirt',0x6000000+target)
const(0x28f7,0x17e3,'ldc.i4',9000);branch(0x28f7,0x17e8,'ble.s',0x17fa)
branch(0x28f7,0x1cfc,'brfalse.s',0x1d03)
for off in (0xb5b,0xbae,0xc34,0xcfb):branch(0x1bc1,off,'blt.s', {0xb5b:0xb87,0xbae:0xc0d,0xc34:0xc5d,0xcfb:0xd24}[off])
site(0x1bc1,0xbca,'call',0xa000612);const(0x1bc1,0xbba,'ldc.r4',18.0)
branch(0x1bc1,0xc79,'brtrue',0xf8f);site(0x1bc1,0xc85,'stfld',0x4000b52)
branch(0x28eb,0xb,'brfalse.s',0xe);const(0x28eb,0xdb,'ldc.r4',3.0);branch(0x28eb,0xe0,'blt.s',0x105)
site(0x28ec,0x36,'callvirt',0x600144a)
yes(p['enums']['0x2000b24']['values']['Variables']==256,'Variables supported section')
yes(p['enums']['0x2000b58']['values']=={'Osu':0,'Taiko':1,'CatchTheBeat':2,'OsuMania':3},'four modes')
props={x['literal']:x['handler'] for x in p['property_dispatch'] if x['method']=='0x6001bc1'}
for key,target in {'HPDrainRate':0xb53,'CircleSize':0xba6,'OverallDifficulty':0xc2c,'ApproachRate':0xcf3,'StackLeniency':0x7ab,'Mode':0x7d5,'AudioFilename':0x754}.items():yes(props[key]==target,('literal dispatch',key))
for key in ('EditorBookmarks','EditorDistanceSpacing','StoryFireInFront','UseSkinSprites','Bookmarks','DistanceSpacing','BeatDivisor','GridSize','TimelineZoom'):
 yes(any(x.get('text')==key for x in p['strings'].values()),('decoded runtime key',key))
yes(len(p['culture_field_writers'])==1 and p['culture_field_writers'][0]['method']=='0x6004669','sole direct culture writer')
for path in ('re/evidence/04/initialization.json','re/evidence/05/slider-timing.json','re/evidence/32/bezier-geometry.json','re/evidence/36/newcombo-preparation.json','re/evidence/44/star-difficulty.json'):
 old=json.loads((Path(__file__).resolve().parents[4]/path).read_bytes())
 for key,value in old['methods'].items():
  if key in m and value.get('body_sha256'):
   yes(value['body_sha256']==m[key].get('body_sha256'),('historical method preservation',path,key))
print('PASS C001 static relationships:',checks,'checks;',len(m),'methods;',sum(len(v['branches']) for v in m.values()),'inventoried branch sites; original runtime NOT RUN')
