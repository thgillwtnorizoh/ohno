"""Independent QA13 packet audit. Reads published evidence/source only; no original code loading."""
import base64,gzip,hashlib,importlib.util,json,subprocess,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[3]
QA=Path(__file__).resolve().parent
BASE='74c47dd452befb741d659e71b2f5267ceb7171a2'
def git(*args):return subprocess.check_output(['rtk','proxy','git',*args],cwd=ROOT)
def sha(b):return hashlib.sha256(b).hexdigest()
def load(p):return json.loads(p.read_text(encoding='utf-8-sig'))
def unpack(p):return gzip.decompress(base64.b64decode(p.read_bytes()))
def authority():
    e=load(ROOT/'re/evidence/42/rank-results.json');m=e['methods']['0x6002b47']
    assert sha((ROOT/'re/evidence/42/rank-results.json').read_bytes())=='484881b5ce25f251208c78f0120810d72a326bca657cf6873bd972f12b4ff911'
    assert m['body_sha256']=='612f291e51c2668a73fbc0a9425e28f9b00d2bc4475322c25bd18ec2f103b267'
    assert [s for s in m['token_sites'] if s[1]=='callvirt']==[[8,'callvirt','0x6002b44'],[24,'callvirt','0x6002b44']]
    branches={a:(b,c) for a,b,c in m['branches']}
    assert branches[0xc7]==('ble.un.s',[0xd1]) and branches[0xcf]==('brfalse.s',[0xde])
    assert branches[0xdc]==('ble.un.s',[0xe0]) and branches[0x100]==('ble.un.s',[0x104])
    assert [0xd3,'ldc.r8',.9] in m['constants'] and [0xf7,'ldc.r8',.8] in m['constants']
    assert m['token_sites'][-2:]==[[0xca,'ldfld','0x400196e'],[0xee,'ldfld','0x400196e']]
    spec=importlib.util.spec_from_file_location('qa13oracle',QA/'grade_oracle.py');oracle=importlib.util.module_from_spec(spec);spec.loader.exec_module(oracle)
    vectors=load(QA/'Inputs/grade-vectors.json')['cases']
    for v in vectors:assert v['expected']==oracle.decide(v['counts'],v['success'],v['mods'])
    assert oracle.decide([9,0,0,1],True,0)['grade']==5
    assert oracle.binary32(9,10)[0]==0x3f666666
    assert 'second A4' in e['reference_cases'][29]['expected']
    assert len(vectors)==91
    return e
def preservation():
    # Compare every previously tracked path in the working tree. Only authorized CI and journal append may differ.
    entries={line.split('\t',1)[1]:line.split()[2] for line in git('ls-tree','-r',BASE).decode().splitlines()}
    before=git('show',f'{BASE}:re/contexts.md')
    assert (ROOT/'re/contexts.md').read_bytes().replace(b'\r\n',b'\n').startswith(before.replace(b'\r\n',b'\n'))
    files=[f for f in entries if f not in ['re/contexts.md','.github/workflows/r04-build.yml']]
    process=subprocess.run(['rtk','proxy','git','hash-object','--stdin-paths'],input=('\n'.join(files)+'\n').encode(),stdout=subprocess.PIPE,check=True,cwd=ROOT)
    actual=process.stdout.decode().splitlines();assert len(actual)==len(files)
    for f,current in zip(files,actual):assert current==entries[f],f
    return len(files)
def results():
    out=[]
    for name in ['windows-results.json.gz.b64','windows-widget-results.json.gz.b64']:
        p=QA/'verification'/name
        if p.exists():
            raw=unpack(p);d=json.loads(raw);assert d['failed']==0
            cases=d['cases'];assert len(cases)==d['named'] and len({c['id'] for c in cases})==len(cases)
            assert all(c['passed'] for c in cases);out.append((name,d['named'],sha(raw)))
    return out
if __name__=='__main__':
    authority();print('QA13 published original relationships and 91 independent rational vectors PASS')
    if '--preservation' in sys.argv:print('QA13 protected historical files PASS',preservation())
    for row in results():print('QA13 exact result packet PASS',*row)
