#!/usr/bin/env python3
"""Issue23 immutable evidence audit, not a rerun of original/production tests."""
import hashlib,json,pathlib,subprocess,sys
H=pathlib.Path(__file__).resolve().parent;R=H.parents[3]
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
parent=H.parent/'timing-state'
for path,digest in json.loads((parent/'seal.json').read_text())['files'].items():assert sha(parent/path)==digest,path
for path,digest in json.loads((H/'provenance.json').read_text())['production_source_sha256'].items():assert sha(R/path)==digest,path
assert subprocess.check_output(['git','-C',R,'diff','--name-only','5dda483348e21e79c8354bf6ea322d50b7fb3e72','HEAD','--','reconstructed/CircleGame','reconstructed/CorpusAudit'],text=True)==''
if (H/'seal.json').exists():
 for path,digest in json.loads((H/'seal.json').read_text())['files'].items():assert sha(H/path)==digest,path
j=json.loads((H/'production-results.json').read_text())
assert (j['characterizationChecks'],j['passed'],j['failed'],j['originalDerivedComparisons'],j['matching'],j['mismatches'])==(233,233,0,52,35,17)
assert len(j['recordedRequests'])==3 and all(not x['matches'] for x in j['recordedRequests'])
subprocess.run([sys.executable,H/'check.py',H/'static-evidence.json'],check=True)
print('PASS immutable parent47/source/new packet seals; recorded233 characterization and35/17 comparisons audited. No execution rerun.')
