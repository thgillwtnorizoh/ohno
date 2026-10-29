#!/usr/bin/env python3
"""Host-independent material checks; schema selftests NEVER original-game tests."""
import argparse
import copy
import hashlib
import json
from pathlib import Path
import tempfile
import zipfile
import build_probe_catalog
import pilot

def rejected(function):
    try:function()
    except (ValueError,KeyError,TypeError):return
    raise AssertionError('invalid material unexpectedly accepted')

def main():
    a=argparse.ArgumentParser();a.add_argument('--archive',type=Path,required=True);args=a.parse_args()
    pkg=Path(__file__).resolve().parent;pins=pilot.load(pkg/'inputs.json')
    assert (pkg/'probes.json').read_bytes()==build_probe_catalog.encoded(build_probe_catalog.build())
    assert pilot.sha(args.archive)==pins['archive_sha256']
    with zipfile.ZipFile(args.archive)as z:
        assert z.testzip()is None
        raw=z.read('osu!.exe');assert hashlib.sha256(raw).hexdigest()==pins['exe_sha256']
    # Metadata fingerprint only, not a new gameplay excavation or original execution.
    import dnfile
    pe=dnfile.dnPE(data=raw);assert str(pe.net.mdtables.Module.rows[0].Mvid)==pins['exe_module_mvid'];pe.close()
    for spec in pins['files'].values():assert pilot.sha(pilot.REPO/spec['repo_path'])==spec['sha256']
    probes=pilot.load(pkg/'probes.json')['probes'];assert len(probes)==34
    matrix=pilot.load(pkg/'test-matrix.json');assert len(matrix['cases'])==17
    assert not matrix['runtime_evidence_cases']and matrix['gameplay_validation_completed']is False
    for c in matrix['cases']:assert c['execution_status']==pilot.NOT_RUN and c['actual_observations']is None and c['comparison']=='NOT_RUN'and c['overhead']is None
    for f in pkg.glob('*.json'):pilot.load(f)
    for f in ['Collect-Environment.ps1','Start-Reference.ps1']:
        s=(pkg/f).read_text();assert 'Win32NT'in s and 'NOT RUN'in s
    # These are text/guard checks, NOT a PowerShell syntax/Windows execution test.
    with tempfile.TemporaryDirectory(prefix='ohno-e20-format-check-')as temp:
        root=Path(temp);(root/'private_artifacts').mkdir()
        m=pilot.load(pkg/'manifest.template.json');m.update(run_id='schema-selftest',kind='SCHEMA_SELFTEST',reference={'exe_sha256':pins['exe_sha256'],'archive_sha256':pins['archive_sha256']})
        pilot.write(root/'manifest.json',m);r=pilot.validate(root);assert r['runtime_acceptance']=='NOT_RUN'and not r['gameplay_validation_completed']
        known={p['probe_id']:p for p in probes};p=probes[0]
        e={'run_id':'schema-selftest','case_id':'P2-HIT','seq':0,'thread_id':1,'thread_seq':0,'qpc_ticks':'0',
           'probe_id':p['probe_id'],'method_token':p['method_token'],'cil_offset':'0x0','binding_id':'selftest-only',
           'stage':'entry','observation_mode':'collector_selftest','values':[{'name':'FORMAT_TEST_ONLY','value_type':'Double','capture_kind':'field_snapshot','text':'1','bits_hex':'000000000000f03f','reference_id':None,'unavailable_reason':None}],'branch':None,'exception':None}
        pilot.check_event(e,m['run_id'],known)
        bad=copy.deepcopy(e);bad['values'][0]['bits_hex']=None;rejected(lambda:pilot.check_event(bad,m['run_id'],known))
        bad=copy.deepcopy(e);bad['method_token']='0x00000000';rejected(lambda:pilot.check_event(bad,m['run_id'],known))
        bad=copy.deepcopy(m);bad['gameplay_validation_completed']=True;pilot.write(root/'manifest.json',bad);rejected(lambda:pilot.validate(root))
        bad=copy.deepcopy(m);bad['case_results'][0].update(execution_status='OBSERVED_PENDING_REVIEW',comparison='MATCH',actual_observations='FORMAT_TEST_ONLY');pilot.write(root/'manifest.json',bad);rejected(lambda:pilot.validate(root))
        pilot.write(root/'manifest.json',m)
        (root/'private_artifacts'/'format-test.txt').write_text('FORMAT TEST ONLY; no runtime observation\n')
        pilot.seal(root);assert pilot.validate(root)['runtime_acceptance']=='NOT_RUN'
        (root/'private_artifacts'/'format-test.txt').write_text('changed\n');rejected(lambda:pilot.validate(root))
        for path in ['../outside','C:/outside','x:stream','a\\b']:rejected(lambda path=path:pilot.contained(root,path))
        rejected(lambda:pilot.prepare(args.archive,root,'already-existing'))
        fake=root/'wrong.zip';fake.write_bytes(b'not the reference');out=root/'must-not-exist'
        rejected(lambda:pilot.prepare(fake,out,'wrong-archive'));assert not out.exists()
    print('PASS: 34 historical method entries, 17 NOT RUN cases, pinned archive/EXE/MVID/fixtures, JSON syntax, byte-reproducible catalog, typed-record/overclaim/hash/path/overwrite rejection selftests. No Windows/PowerShell or original gameplay execution; runtime acceptance NOT RUN.')
if __name__=='__main__':main()
