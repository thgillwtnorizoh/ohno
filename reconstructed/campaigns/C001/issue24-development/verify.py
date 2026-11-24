#!/usr/bin/env python3
"""Verify Work2 source/evidence integrity without rerunning historical suites."""
import hashlib,json,pathlib
H=pathlib.Path(__file__).resolve().parent;R=H.parents[3]
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
for family in ['timing-state','timing-ownership']:
 p=R/'re/campaigns/C001'/family
 for name,digest in json.loads((p/'seal.json').read_text())['files'].items():assert sha(p/name)==digest,(family,name)
for name,digest in json.loads((H/'provenance.json').read_text())['source_sha256'].items():assert sha(R/name)==digest,name
j=json.loads((H/'local/results.json').read_text());assert (j['passed'],j['failed'],j['executed'])==(550,0,550)
if (H/'seal.json').exists():
 for name,digest in json.loads((H/'seal.json').read_text())['files'].items():assert sha(H/name)==digest,name
print('PASS source and unchanged Work1 seals; recorded550/550 development evidence verified, not independent QA.')
