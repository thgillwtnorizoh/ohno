#!/usr/bin/env python3
"""Deterministic owner ZIP plus per-map osz import archives; no proprietary assets."""
import hashlib,io,json,sys,wave,zipfile
from pathlib import Path
QA=Path(__file__).resolve().parent
B=QA/'beatmaps'
def sha(b):return hashlib.sha256(b).hexdigest()
def entry(z,name,b):
    info=zipfile.ZipInfo(name,(2026,11,13,0,0,0));info.compress_type=zipfile.ZIP_DEFLATED
    info.external_attr=0o100644<<16;z.writestr(info,b,compresslevel=9)
def check_folder():
    manifest=json.loads((B/'manifest.json').read_bytes());rows=manifest['files'];assert len(rows)==27
    assert len(list((B/'Songs').rglob('*.osu')))==len(rows)
    for r in rows:
        p=B/r['path'];b=p.read_bytes();assert sha(b)==r['sha256']
        text=b.decode();assert text.splitlines()[0]=='osu file format v14'
        for key,value in [('Mode','0'),('StackLeniency','0'),('CircleSize','5'),('OverallDifficulty','5'),('AudioFilename','silence.wav')]:assert f'{key}:{value}\n' in text
        assert f"Version:{r['id']} {r['name']}\n" in text
        assert 'Title:QA15 ' in text and 'Artist:' in text and 'Creator:Work1\n' in text
        assert text.split('[Events]\n')[1].split('[TimingPoints]')[0]=='// No background video storyboard or external assets\n\n'
        assert text.split('[TimingPoints]\n')[1].split('[HitObjects]')[0]=='0,500,4,1,0,100,1,0\n\n'
        cells=[list(map(int,l.split(','))) for l in text.split('[HitObjects]\n')[1].splitlines() if l]
        assert len(cells)==r['circle_count'] and all(len(c)==5 and c[3:]==[1,0] for c in cells)
        assert all(a[2]<b[2] for a,b in zip(cells,cells[1:]));assert cells[-1][2]-cells[0][2]==r['chart_duration_ms']
        audio=B/r['audio']['path'];raw=audio.read_bytes();assert sha(raw)==r['audio']['sha256']
        assert not any(raw[44:])
        with wave.open(str(audio),'rb') as w:
            assert (w.getnchannels(),w.getsampwidth(),w.getframerate(),w.getcomptype())==(1,2,8000,'NONE')
            assert w.getnframes()/8==r['audio_duration_ms']>r['last_object_ms']
        assert r['original_game_displayed_stars']=='' and r['comparison_status']=='PENDING HUMAN TEST'
    return manifest

def build():
    m=check_folder();out=io.BytesIO();archives={}
    with zipfile.ZipFile(out,'w') as z:
        for p in sorted(B.rglob('*')):
            if p.is_file():entry(z,'QA15-comparison/'+p.relative_to(B).as_posix(),p.read_bytes())
        for name in ['human-test-protocol.md','human-results-template.md']:
            entry(z,'QA15-comparison/'+name,(QA/name).read_bytes())
        for r in m['files']:
            archive=io.BytesIO();p=B/r['path']
            with zipfile.ZipFile(archive,'w') as inner:
                entry(inner,p.name,p.read_bytes());entry(inner,'silence.wav',(p.parent/'silence.wav').read_bytes())
            b=archive.getvalue();archives[r['id']]=dict(sha256=sha(b),bytes=len(b),osu_sha256=r['sha256'])
            entry(z,'QA15-comparison/Imports/'+r['id']+'.osz',b)
    b=out.getvalue();p=QA/'QA15-comparison-beatmaps.zip'
    if '--check' in sys.argv:assert p.read_bytes()==b
    else:p.write_bytes(b)
    with zipfile.ZipFile(io.BytesIO(b)) as z:
        for r in m['files']:
            assert sha(z.read('QA15-comparison/'+r['path']))==r['sha256']
            with zipfile.ZipFile(io.BytesIO(z.read('QA15-comparison/Imports/'+r['id']+'.osz'))) as inner:
                assert set(inner.namelist())=={Path(r['path']).name,'silence.wav'}
                assert sha(inner.read(Path(r['path']).name))==r['sha256']
    packet=dict(archive=p.name,sha256=sha(b),bytes=len(b),charts=len(m['files']),osz=archives,
        original_import='NOT OBSERVED; documented-format/package validation only')
    if '--check' in sys.argv:assert json.loads((QA/'verification/package.json').read_bytes())==packet
    else:(QA/'verification/package.json').write_text(json.dumps(packet,indent=2)+'\n')
    print('PASS QA15 folder, PCM silence, ZIP and osz byte identity:',len(m['files']),'charts;',len(b),'bytes;',sha(b))
if __name__=='__main__':build()
