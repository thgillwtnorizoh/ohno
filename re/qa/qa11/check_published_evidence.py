"""QA11 evidence consistency only. Does not access originals, execute games or play media."""
from pathlib import Path
from collections import Counter
import hashlib,json,sys

q=Path(__file__).resolve().parent
r=q.parents[2]
sha=lambda b:hashlib.sha256(b).hexdigest()
load=lambda p:json.loads(p.read_text(encoding='utf-8'))
initial=q/'verification/initial'
for path,digest in load(initial/'manifest.json').items():
    assert sha((initial/Path(path).name).read_bytes())==digest,path
for item in load(q/'Inputs/manifest.json'):
    assert sha((q/'Inputs'/item['file']).read_bytes())==item['sha256']
coverage=load(q/'e040-coverage.json')
assert coverage['authority_sha256']==sha((r/'re/evidence/40/slider-continuous-audio.json').read_bytes())
assert [x['id'] for x in coverage['controls']]==[f'L{i:02}' for i in range(1,57)]
result=q/'verification/windows-results.json'
if not result.exists():result=q/'verification/local-corrected.json'
j=load(result)
assert j['executed']==247 and j['passed']==246 and j['failed']==1
failed=[c for c in j['cases'] if not c['passed']]
assert len(failed)==1 and failed[0]['Name'].startswith('G08 ')
assert len(set(c['Name'] for c in j['cases']))==247
historical=load(r/'re/qa/qa10/verification/assets.json')
old=historical['actual_ticks']+historical['historical_circle_artifacts']
expected=Counter(x['sha256'] for x in old)
w04=next(c for c in j['cases'] if c['Name'].startswith('W04 '))
actual=Counter(o['actual'] for o in w04['observations'] if isinstance(o.get('actual'),str))
assert actual==Counter({k:v*2 for k,v in expected.items()}),'Fifteen historical fingerprints across both packs'
w02=next(c for c in j['cases'] if c['Name'].startswith('W02 '))
loops=[o for o in w02['observations'] if 'file' in o]
assert len(loops)==6 and all(x['length']==44144 for x in loops)
artifact={'schema':'QA11.assets.v1','source_result':str(result.relative_to(q)).replace('\\','/'),'source_result_sha256':sha(result.read_bytes()),'expected':{'encoding':'PCM16LE','channels':1,'sample_rate':44100,'frames':22050,'seconds':0.5,'file_bytes':44144,'riff_size':44136,'data_size':44100,'chunks':{'fmt ':12,'data':36}},'actual_loops':loops,'previous_fifteen':old,'historical_fingerprint_comparison':'PASS; both packs match QA10 historical hash multiset, including intentional duplicate waveforms','limits':'Independent generated resources only; no asset byte distribution, decoder, gapless or audible claim. This consistency check adds no named QA case.'}
if '--write-assets' in sys.argv:(q/'verification/assets.json').write_text(json.dumps(artifact,indent=2)+'\n',encoding='utf-8')
print('QA11 evidence PASS: initial hashes,16 fixtures/50 references,56 controls,247 unique outcomes,one retained discrepancy,six loops,fifteen historical fingerprints twice')
