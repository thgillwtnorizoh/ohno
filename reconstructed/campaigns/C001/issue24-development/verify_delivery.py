#!/usr/bin/env python3
"""Audit the CI-only checkout correction without changing pinned first evidence."""
import pathlib,json,hashlib,gzip,base64,subprocess
H=pathlib.Path(__file__).resolve().parent;R=H.parents[3]
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
for path,digest in json.loads((H/'provenance.json').read_text())['source_sha256'].items():
 if path!='.github/workflows/r04-build.yml':assert sha(R/path)==digest,path
for path,digest in json.loads((H/'ci-source-hashes.json').read_text()).items():assert sha(R/path)==digest,path
for family in ['timing-state','timing-ownership']:
 p=R/'re/campaigns/C001'/family
 for path,digest in json.loads((p/'seal.json').read_text())['files'].items():assert sha(p/path)==digest,(family,path)
for path,digest in json.loads((H/'seal.json').read_text())['files'].items():
 if path=='README.md':
  old=subprocess.check_output(['git','-C',R,'show','f76c26a05ae88587bb44440fbe078ac75b59fb41:'+str((H/path).relative_to(R))])
  assert hashlib.sha256(old).hexdigest()==digest and (H/path).read_bytes().startswith(old),path
 else:assert sha(H/path)==digest,path
failed=H/'windows85-failure'
j=json.loads(gzip.decompress(base64.b64decode((failed/'results.json.gz.b64').read_bytes())));r=json.loads(gzip.decompress(base64.b64decode((failed/'crlf-reproduction-results.json.gz.b64').read_bytes())))
assert (j['passed'],j['failed'])==(536,14)
assert [(c['id'],c['name']) for c in j['checks'] if not c['passed']]==[(c['id'],c['name']) for c in r['checks'] if not c['passed']]
print('PASS production/harness unchanged, Work1 immutable, initial evidence preserved, exact14 CRLF failure set reproduced; corrected Windows gate still requires its own results.')
