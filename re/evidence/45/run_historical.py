#!/usr/bin/env python3
"""Run unchanged historical static checks, writing only to a supplied output dir.

python re/evidence/45/run_historical.py /path/osubackup.zip /tmp/e045-history
Requires the same Python static-inspection dependencies as the E045 collector.
No game, CLR, audio, reconstruction or historical evidence mutation.
"""
from pathlib import Path
import contextlib,hashlib,json,runpy,sys,traceback
root=Path(__file__).resolve().parents[3]
archive=Path(sys.argv[1]).resolve();folder=Path(sys.argv[2]).resolve();folder.mkdir(parents=True,exist_ok=True)
sys.path.insert(0,str(root/'re/tools'));sys.dont_write_bytecode=True
records=[]
jobs=[('E004','check_beatmap_init.py','04/initialization.json'),
      ('E005','check_slider_timing.py','05/slider-timing.json'),
      ('E006','check_linear_geometry.py','06/linear-geometry.json'),
      ('E023','check_aggregate_policy.py','23/aggregate-policy.json'),
      ('E032','check_bezier_geometry.py','32/bezier-geometry.json'),
      ('E033','check_score_accounting.py','33/score-accounting.json'),
      ('E034','check_chart_score_multiplier.py','34/chart-score-multiplier.json')]
with (folder/'historical-checks.txt').open('w',encoding='utf-8',newline='\n') as log,contextlib.redirect_stdout(log),contextlib.redirect_stderr(log):
    for label,tool,packet in jobs:
        print(label,tool,packet,flush=True);sys.argv=[tool,str(root/'re/evidence'/packet)]
        try:runpy.run_path(str(root/'re/tools'/tool),run_name='__main__');passed=True
        except Exception:traceback.print_exc();passed=False
        records.append({'authority':label,'tool':tool,'source_sha256':hashlib.sha256((root/'re/tools'/tool).read_bytes()).hexdigest(),'packet':packet,'pass':passed})
    import check_selectplay as e43
    d=json.loads((root/'re/evidence/43/selectplay.json').read_text(encoding='utf-8'))
    try:
        literals=e43.relationships(d);controls=e43.projections(d);bodies=e43.archive_audit(d,str(archive))
        print('PASS E043 unchanged relationships',literals,'finite',controls,'archive bodies',bodies)
        boundary='Pre-R16 reconstruction-source manifest not asserted against later R16; original-static functions unchanged.'
        print(boundary)
        records.append({'authority':'E043','pass':True,'tool':'check_selectplay.py:relationships/projections/archive_audit',
            'literals':literals,'projections':controls,'original_byte_bodies':bodies,'boundary':boundary})
    except Exception:traceback.print_exc();records.append({'authority':'E043','pass':False})
(folder/'historical-checks.json').write_text(json.dumps(records,indent=2)+'\n',encoding='utf-8',newline='\n')
print('PASS historical static checks' if all(r['pass'] for r in records) else 'FAIL historical static checks')
sys.exit(0 if all(r['pass'] for r in records) else 1)
