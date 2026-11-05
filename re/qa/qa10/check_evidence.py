"""Independent packet integrity; never opens the original archive or audio device."""
import hashlib
import json
import base64
import gzip
import re
from pathlib import Path

HERE = Path(__file__).resolve().parent
ROOT = HERE.parents[2]


def sha(data):
    return hashlib.sha256(data).hexdigest()


initial_bytes = (HERE / 'verification/local-third.json').read_bytes()
assert sha(initial_bytes) == '2f136419b036d804d0ab8c332602ccf59b19e516253140cb2fca03a24a85defc'
initial = json.loads(initial_bytes)
assert (initial['executed'], initial['passed'], initial['failed']) == (227, 224, 3)
for path, expected in initial['sourceHashes'].items():
    if path.startswith('re/qa/qa10/Headless/'):
        snapshot = HERE / 'verification/initial-harness' / (Path(path).name + '.txt')
        assert sha(snapshot.read_bytes()) == expected, snapshot
manifest = json.loads((HERE / 'Inputs/manifest.json').read_text())
for item in manifest:
    assert sha((HERE / 'Inputs' / item['path']).read_bytes()) == item['sha256']
coverage = json.loads((HERE / 'verification/e039-coverage.json').read_text())
assert coverage['authority_sha256'] == sha((ROOT / 're/evidence/39/slider-hitsounds.json').read_bytes())
assert [c['original']['id'] for c in coverage['cases']] == [f'S{i:02}' for i in range(1, 49)]
for c in coverage['cases'][43:47]:
    assert c['qa10_cases'] == [] and c['coverage'] == 'UNSUPPORTED CONTINUOUS SUBSYSTEM'

windows = HERE / 'verification/windows-results.json'
if windows.exists():
    results = json.loads(windows.read_bytes())
    assert (results['executed'], results['passed'], results['failed'], results['sourceInspected']) == (229, 229, 0, 4)
    assert results['runtime'].startswith('.NET 8.')
    assert len({c['Name'] for c in results['cases']}) == 229
    assert all(c['passed'] for c in results['cases'])
    for path, expected in results['sourceHashes'].items():
        data = (ROOT / path).read_bytes()
        # Windows checkout line endings are recorded rather than hidden.
        lf = data.replace(b'\r\n', b'\n')
        assert expected in {sha(data), sha(lf), sha(lf.replace(b'\n', b'\r\n'))}, path
    provenance = json.loads((HERE / 'verification/windows-provenance.json').read_text())
    assert provenance['results_sha256'] == sha(windows.read_bytes())
    assert provenance['tested_source'] == results['sourceSha']
    assert provenance['historical_total'] == 862
    log_bytes = (HERE / 'verification/windows-job.log').read_bytes()
    assert sha(log_bytes) == provenance['job_log_sha256']
    log = log_bytes.decode('utf-8')
    parts = re.findall(r'QA10 EVIDENCE (\d+)/(\d+) ([A-Za-z0-9+/=]+)', log)
    assert len(parts) == int(parts[0][1])
    assert [int(p[0]) for p in parts] == list(range(1, len(parts)+1))
    assert gzip.decompress(base64.b64decode(''.join(p[2] for p in parts))) == windows.read_bytes()
    for suite, count in provenance['historical'].items():
        pattern = ('HEADLESS COMPONENT' if suite == 'QA01' else suite + r' [A-Z ]+')
        assert re.search(pattern + rf' TESTS: {count} passed, 0 failed, {count} executed', log), suite
    assert 'QA10 SLIDER SOUND TESTS: 229 passed, 0 failed, 229 executed' in log
    assert '0 Warning(s)' in log and '0 Error(s)' in log
    assert all(s['conclusion'] == 'success' for s in provenance['steps'])
    print('PASS Windows .NET8: 225 component +4 source audits; historical862 recorded')
else:
    print('Windows packet not present; no final-gate claim')
print(f'PASS preserved initial failure, five source snapshots, {len(manifest)} finite fixtures, 48-case bounded coverage map')
