"""Recheck retained evidence and exact-source identity; no original execution."""
from pathlib import Path
import base64,gzip,hashlib,json,subprocess
root=Path(__file__).resolve().parents[5]
def git_bytes(ref,path):return subprocess.check_output(['git','show',f'{ref}:{path}'],cwd=root)
r=root/'re/campaigns/C001/catmull'
p=json.loads((r/'FreshReview/provenance.json').read_bytes())
old=gzip.decompress(base64.b64decode((r/'qa-2d776dac/results.json.gz.b64').read_bytes()))
assert hashlib.sha256(old).hexdigest()==p['reusedPrior5782ResultSha256']
assert json.loads(old)['checks']==5782 and json.loads(old)['failed']==0
for file in [r/'manifest.json',*sorted((r/'fixtures').glob('*.osu'))]:
 assert file.read_bytes()==git_bytes('c353b83e2dfb5e6da7cfe20053f15a6afdc3626f',str(file.relative_to(root)))
for path,sha in p['sourceSha256'].items():
 data=(root/path).read_bytes();assert hashlib.sha256(data).hexdigest()==sha,path
 if path.startswith('reconstructed/'):
  assert data==git_bytes(p['exactTestedProduction'],path),path
new=json.loads((r/'FreshReview/results.json').read_bytes())
assert new['checks']==22 and new['failed']==0
print('PASS retained5782 result seal; unchanged19fixture packet; exact production source hashes;22/22 fresh checks')
