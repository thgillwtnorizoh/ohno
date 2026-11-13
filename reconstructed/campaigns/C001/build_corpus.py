#!/usr/bin/env python3
"""C001 independent synthetic inputs. Expected states are Work 2 controls, not original-runtime observations."""
import hashlib, json, pathlib, sys
root = pathlib.Path(__file__).resolve().parent
cases = {}
def chart(version, objects, mode=0, extra=''):
    return f'''osu file format v{version}
[General]
Mode:{mode}
StackLeniency:0
[Metadata]
Title:C001 Independent Controls
Artist:Project ohno
Creator:Work 2
Version:v{version} mode{mode}
[Difficulty]
HPDrainRate:5
CircleSize:5
OverallDifficulty:5
ApproachRate:5
SliderMultiplier:1
SliderTickRate:1
[TimingPoints]
0,500,4,1,0,100,1,0
{extra}[HitObjects]
{objects}
'''
def add(name, text, features, gameplay, expected):
    data=text.encode('utf-8');cases[name]=(data,dict(file='corpus/'+name,sha256=hashlib.sha256(data).hexdigest(),source='build_corpus.py',features=features,expected_syntax='accepted',expected_gameplay=gameplay,expected_prepared=expected,original_runtime='NOT RUN',confidence='ENGINEERING synthetic fixture; version geometry predictions derived from E006/E032'))
for v in range(5,15):
    add(f'v{v:02}-bezier.osu',chart(v,'128,192,3000,2,0,B|256:64|384:192,1,300'),['bezier',f'v{v}','timing','sample-sidecar'],True,dict(objects=1,sliders=1,format=v))
    add(f'v{v:02}-polyline.osu',chart(v,'128,192,3000,2,0,L|228:192|228:292,1,250'),['linear','multi-control',f'v{v}','tail-extension'],True,dict(objects=1,sliders=1,end=4250,tail=[228,342]))
for mode in range(4):
    obj='64,64,3000,128,0,4000:1:2:3:40:custom.wav' if mode==3 else '64,64,3000,1,0'
    add(f'mode{mode}-raw.osu',chart(14,obj,mode,'[Editor]\nBookmarks:1000,2000\n[Events]\n0,0,"background,one.jpg",0,0\nSample,2500,0,"sample.wav",60\n[Colours]\nCombo1:10,20,30\n[Future]\nUnknown:value:with:colons\n'),['mode-identity','resources','unknown-data','quoted-comma','hold' if mode==3 else 'circle'],mode==0,dict(raw_hitobjects=1,mode=mode))
manifest=dict(classification='Work 2 engineering compatibility corpus; original runtime and independent verification pending',cases=[v[1] for v in cases.values()])
outputs={root/'corpus'/k:v[0] for k,v in cases.items()}
outputs[root/'corpus-manifest.json']=(json.dumps(manifest,indent=2)+'\n').encode()
for p,b in outputs.items():
    if '--check' in sys.argv:
        assert p.read_bytes()==b, str(p)
    else:
        p.parent.mkdir(parents=True,exist_ok=True);p.write_bytes(b)
print(f'PASS C001 reproducible corpus: {len(cases)} exact-byte fixtures; no original execution')
