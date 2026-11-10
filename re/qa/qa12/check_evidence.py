"""Published QA12 consistency checks. No binaries, media or original runtime."""
import base64, collections, gzip, hashlib, json, pathlib, subprocess
ROOT = pathlib.Path(__file__).resolve().parents[3]
Q = ROOT / 're/qa/qa12'
def load(path):
    b = pathlib.Path(path).read_bytes()
    if str(path).endswith('.gz.b64'):
        b = gzip.decompress(base64.b64decode(b))
    return json.loads(b)
def digest(b):
    return hashlib.sha256(b).hexdigest()
def main():
    matrix = load(Q / 'e041-coverage.json')
    assert [r['id'] for r in matrix['rows']] == [f'A{i:02}' for i in range(1,67)]
    assert dict(collections.Counter(r['execution_classification'] for r in matrix['rows'])) == matrix['counts']
    assert matrix['authority_sha256'] == digest((ROOT/'re/evidence/41/spinner-audio.json').read_bytes())
    initial = load(Q/'verification/initial-runtime/results.json.gz.b64')
    assert (initial['executed'],initial['passed'],initial['failed']) == (267,260,7)
    for p in (Q/'verification/initial-runtime').glob('*.cs'):
        assert digest(p.read_bytes()) == initial['manifest']['re/qa/qa12/Headless/'+p.name], p
    added = load(Q/'verification/added-sentinel-initial.json.gz.b64')
    assert (added['executed'],added['passed'],added['failed']) == (270,268,2)
    local = load(Q/'verification/local-results.json.gz.b64')
    assert (local['executed'],local['passed'],local['failed']) == (270,270,0)
    source = Q/'verification/windows-results.json.gz.b64'
    if source.exists():
        windows = load(source)
        assert (windows['executed'],windows['passed'],windows['failed']) == (274,274,0)
        assert len({c['id'] for c in windows['cases']}) == 274
        assert collections.Counter(c['classification'] for c in windows['cases']) == {'Executed finite component':262,'Source-only inspection':8,'Independent asset control':4}
        # The finite session trace observes every ledger event: no count gap >1.
        for case in windows['cases']:
            if not case['id'].startswith('S01 '):
                continue
            previous = 0
            events = []
            for observation in case['observations']:
                value = observation.get('actual')
                if isinstance(value,str) and value.startswith('{"Scoring"'):
                    snapshot = json.loads(value)
                    count = snapshot['EventCount']
                    assert 0 <= count-previous <= 1, (case['id'],previous,count)
                    if count != previous:
                        events.append(snapshot['LastEvent'])
                    previous = count
            assert previous == len(events) == 19, case['id']
        callback = load(Q/'verification/windows-callback-results.json.gz.b64')
        assert (callback['executed'],callback['passed'],callback['failed']) == (12,12,0)
        assert callback['source'] == windows['source']
        for row in matrix['rows']:
            for linked in row['observed_managed_result']['named_outcomes']:
                assert any(c['id']==linked['id'] and c['passed'] for c in windows['cases']), linked
        # Hosted Git checkout CRLF is retained in raw hashes; identify actual bytes.
        checkout = {}
        for name, h in windows['manifest'].items():
            p = ROOT/name
            b = p.read_bytes()
            if digest(b)==h: checkout[name]='canonical'
            elif digest(b.replace(b'\r\n',b'\n').replace(b'\n',b'\r\n'))==h: checkout[name]='CRLF checkout'
            else: raise AssertionError(('unexpected hosted source bytes',name))
        print('Hosted byte identities:',dict(collections.Counter(checkout.values())))
    before = subprocess.check_output(['git','show','e785774893d45fe4d2a45617212ce597ebe1bbba:re/contexts.md'],cwd=ROOT)
    assert (ROOT/'re/contexts.md').read_bytes().startswith(before)
    changed = subprocess.check_output(['git','diff','--name-only','e785774893d45fe4d2a45617212ce597ebe1bbba','HEAD'],cwd=ROOT,text=True).splitlines()
    assert all(p.startswith('re/qa/qa12/') or p in ['re/contexts.md','.github/workflows/r04-build.yml'] for p in changed)
    print('QA12 evidence consistency PASS; matrix66, preserved failed attempts and byte prefix/scope. This is not an additional QA case.')
if __name__ == '__main__':
    main()
