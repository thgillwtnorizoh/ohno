#!/usr/bin/env python3
"""Unchanged historical functions; all new output is confined to supplied dir."""
from pathlib import Path
import contextlib,json,runpy,sys,traceback
root=Path(__file__).resolve().parents[3];archive=Path(sys.argv[1]).resolve();out=Path(sys.argv[2]).resolve();out.mkdir(parents=True,exist_ok=True)
sys.path.insert(0,str(root/'re/tools'));sys.dont_write_bytecode=True
records=[]
with (out/'historical-closure.txt').open('w',encoding='utf-8',newline='\n') as f,contextlib.redirect_stdout(f),contextlib.redirect_stderr(f):
    try:
        sys.argv=['run_historical.py',str(archive),str(out/'relationships')]
        runpy.run_path(str(root/'re/evidence/45/run_historical.py'),run_name='__main__')
    except SystemExit as e:
        records.append({'suite':'E004/E005/E006/E023/E032/E033/E034/E043','pass':e.code==0})
    except Exception:traceback.print_exc();records.append({'suite':'historical relationships','pass':False})
    try:
        sys.argv=['check_star_difficulty.py',str(root/'re/evidence/44/star-difficulty.json'),str(archive),str(out/'e044-controls.json')]
        runpy.run_path(str(root/'re/tools/check_star_difficulty.py'),run_name='__main__')
        records.append({'suite':'E044 unchanged full CLI','pass':True})
    except Exception:traceback.print_exc();records.append({'suite':'E044','pass':False})
    try:
        import check_circle_star_history as e45
        packet=root/'re/evidence/45/circle-star-history.json';d=json.loads(packet.read_text(encoding='utf-8'))
        stats=e45.byte_audit(d,str(archive));rows,traces=e45.finite();preserved=e45.preservation(root)
        stats.update(finite_controls=len(rows),non_golden_traces=len(traces),preserved_files=preserved)
        (out/'e045-controls.json').write_text(json.dumps(dict(stats=stats,cases=rows),indent=2,allow_nan=False)+'\n',encoding='utf-8',newline='\n')
        print('PASS E045 unchanged byte_audit/finite/preservation',json.dumps(stats,sort_keys=True))
        records.append({'suite':'E045 unchanged byte_audit/finite/preservation','pass':True,'stats':stats,
            'boundary':'Call unchanged functions directly to avoid historical CLI failure-capture writing inside evidence45; outputs only under new directory.'})
    except Exception:traceback.print_exc();records.append({'suite':'E045','pass':False})
(out/'historical-summary.json').write_text(json.dumps(records,indent=2)+'\n',encoding='utf-8',newline='\n')
print('PASS historical E046 authorities' if all(r['pass'] for r in records) else 'FAIL historical E046 authorities')
sys.exit(0 if all(r['pass'] for r in records) else 1)
