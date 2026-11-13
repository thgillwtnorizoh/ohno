#!/usr/bin/env python3
"""E046 deterministic static extraction. No original CLR/game methods invoked."""
import hashlib,json,sys
from pathlib import Path
import circle_star_history_evidence as previous

def extract(archive):
    d=previous.extract(archive);d['expedition']=46
    d['research_source']='fadeca62227b94b25fa3d940e5ecf85453482bfc'
    root=Path(__file__).resolve().parents[2]
    paths=['re/45-original-circle-star-history-mathematics.md','re/evidence/45/circle-star-history.json',
           're/evidence/45/controls.json','re/evidence/45/coverage.json','re/evidence/45/failures/failure-01.json',
           're/tools/circle_star_history_projection.py','re/tools/check_circle_star_history.py']
    d['e045_unchanged_authority']={p:hashlib.sha256((root/p).read_bytes()).hexdigest() for p in paths}
    d['collector_sources']['rhythm_visibility_evidence.py']=hashlib.sha256(Path(__file__).read_bytes()).hexdigest()
    d['e046_sources']={p:hashlib.sha256((root/p).read_bytes()).hexdigest() for p in [
        're/tools/rhythm_visibility_projection.py','re/tools/check_rhythm_visibility.py',
        're/evidence/46/independent_oracle.py','re/evidence/46/finite_inputs.py']}
    d['e046_dependencies']={
        'group_heap':{'type':'T0326','fields':['F0daf interval','F0db0 count','F0db1 repeats'],
            'sites':['M1744:02ef-03ba','M1744:031a-0328','M1744:0419-0423','M1746-M174f']},
        'visibility':['M1e57:0028-0069','M1e58','M1e5b-M1e65','M368f','M325f-M3260'],
        'independence':'Expected oracle imports only math/struct/collections; explicit identity heap, indexed geometry and offline duration sweep. Subject is unchanged E045 projection with tracing-only identities.',
        'admission':'Same E045 v14 StackLeniency0 / strictly increasing circles / explicit viewport+shift; no broadening',
        'limits':['Original runtime/JIT/sort/library precision NOT RUN','No sliders/spinners/mods/nonzero stacking','Historical2175 QA NOT RERUN']}
    return d

if __name__=='__main__':
    Path(sys.argv[2]).write_text(json.dumps(extract(sys.argv[1]),indent=2,sort_keys=True,allow_nan=False)+'\n',encoding='utf-8',newline='\n')
