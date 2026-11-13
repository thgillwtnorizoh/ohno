"""Read-only E046 expected-data adapter; never imports production or observed model.
Not a new research/QA gate. Writes only R17 input/supplement records.
"""
import hashlib,importlib.util,json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
def load(name,path):
    spec=importlib.util.spec_from_file_location(name,ROOT/path);module=importlib.util.module_from_spec(spec);spec.loader.exec_module(module);return module
oracle=load('e046_independent','re/evidence/46/independent_oracle.py')
r=json.loads((ROOT/'re/evidence/46/results.json').read_bytes());cases={x['name']:x for x in r['cases']};inputs=[]
for c in r['charts']:
    inp=cases[c['id']]['input'];params=inp['params'];q=oracle.geometry(inp['circles'],*params)
    inputs.append(dict(id=c['id'],kind=c['kind'],circles=inp['circles'],params=params,
        movement_terms=[oracle.movement_terms(q,i) for i in range(len(q))]))
# Oracle movement_terms returns early all0 at i<=1; the separate jump term remains
# reached in composite. Those early jump entries are intentionally not compared.
out=dict(authority='re/evidence/46/results.json',authoritySha256=hashlib.sha256((ROOT/'re/evidence/46/results.json').read_bytes()).hexdigest(),
    oracleSha256=hashlib.sha256((ROOT/'re/evidence/46/independent_oracle.py').read_bytes()).hexdigest(),rows=inputs)
(ROOT/'reconstructed/R17DevelopmentChecks/reference-inputs.json').write_text(json.dumps(out,indent=2)+'\n')
print('R17 input/supplement records',len(inputs),'independent expected charts remain in original E046 results.json')
