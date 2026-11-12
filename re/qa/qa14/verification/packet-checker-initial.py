#!/usr/bin/env python3
"""Independent QA14 packet, preservation, numeric-operand and exact-gate checks.
No original executable/CLR loading. Run after packet assembly from repository root.
"""
import base64
from collections import Counter
import gzip
import hashlib
import json
import math
from pathlib import Path
import struct
import subprocess
import sys

QA=Path(__file__).resolve().parent
ROOT=QA.parents[2]
V=QA/'verification'
BASE='abd9dbf763b27556fe73e64c41c1042443a2d2d5'
def sha(b):return hashlib.sha256(b).hexdigest()
def raw(p):
    p=Path(p)
    return p.read_bytes() if p.exists() else gzip.decompress(base64.b64decode(Path(str(p)+'.gz.b64').read_bytes()))
def load(p):return json.loads(raw(p))
def git(*args):return subprocess.check_output(['rtk','proxy','git',*args],cwd=ROOT)

def authority():
    b=raw(ROOT/'re/evidence/43/selectplay.json')
    assert sha(b)=='9ac182e9b6af5faaca3e517c4500bbeef574b715af01350f5d223b8fc88324a4'
    e=json.loads(b)
    assert len(e['reference_cases'])==66
    assert [0x21,'stsfld','0x4001a0f'] in e['methods']['0x6002c64']['token_sites']
    assert [0x31d,'call','0x60046ae'] in e['methods']['0x6002578']['token_sites']
    assert [6,'stfld','0x40013f8'] in e['methods']['0x6002237']['token_sites']
    assert [0x1b4,'call','0x6002d7f'] in e['methods']['0x6001bdb']['token_sites']
    assert [0,'ldc.i4',1370] in e['methods']['0x6000803']['constants']
    for source,expected in [(4.56789,4.56),(1.15,1.14),(1.01,1.0),(2.675,2.67),(-4.5,4.5)]:
        f=struct.unpack('<f',struct.pack('<f',source))[0]
        assert math.floor(abs(float(f))*100)/100==expected
    return e

def preserved():
    before=load(V/'baseline.json')
    b=(ROOT/'re/contexts.md').read_bytes()
    assert sha(b[:before['journal_bytes']])==before['journal_sha256']
    entries={line.split('\t',1)[1]:line.split()[2] for line in git('ls-tree','-r',BASE).decode().splitlines()}
    names=[n for n in entries if n not in ['re/contexts.md','.github/workflows/r04-build.yml']]
    assert set(names)==set(before['protected'])
    output=subprocess.check_output(['rtk','proxy','git','hash-object','--stdin-paths'],input=('\n'.join(names)+'\n').encode(),cwd=ROOT).decode().splitlines()
    assert len(output)==len(names)
    for n,digest in zip(names,output):assert digest==entries[n],n
    # Raw local bytes, including fixtures and previous logs, are separately pinned.
    for n,digest in before['protected'].items():assert sha((ROOT/n).read_bytes())==digest,n
    return len(names)

def packed():
    for name,item in load(V/'packed-files.json').items():
        b=raw(QA/name)
        assert len(b)==item['bytes'] and sha(b)==item['sha256'],name
    return len(load(V/'packed-files.json'))

def failures():
    assert 'NU1301' in raw(V/'initial-headless-local/execution.log').decode('utf-8-sig')
    assert 'CS0119' in raw(V/'initial-windows-local/execution.log').decode('utf-8-sig')
    partial=raw(V/'failure-headless-local-2/execution.log').decode('utf-8-sig')
    for text in ['C/external-path','System.TimeoutException','Access is denied','T_wrong-compatibility-osu-stable:E043:cache-v2']:assert text in partial,text
    failed=load(V/'failure-headless-local-3/results.json')
    assert (failed['named'],failed['passed'],failed['failed'])==(226,225,1)
    assert [c['id'] for c in failed['cases'] if not c['passed']]==['A/browser-rank-key-isolation']
    first=load(V/'first-complete-local/local-headless-results.json')
    assert (first['named'],first['passed'],first['failed'])==(231,231,0)
    assert sha(raw(V/'first-complete-local/local-headless-results.json'))=='d7de6ec2c68bca1e7e72583d2e93f6b32e2aca44c19e5a7423923f8b496532e1'
    for p in [V/'initial-headless-local',V/'failure-headless-local-2',V/'failure-headless-local-3',V/'initial-windows-local']:
        d=load(p/'provenance.json');assert d['exit']!=0 and sha(raw(p/'execution.log'))==d['log_sha256']

def final_results():
    head=load(V/'windows-results.json');win=load(V/'windows-widget-results.json')
    assert head.get('filter') is None and head['named']==234 and win['named']==30
    assert head['failed']==win['failed']==0
    assert head['classifications']=={'EXECUTED COMPONENT':226,'SOURCE INSPECTION':8}
    for packet in [head,win]:
        assert len(packet['cases'])==packet['named']==packet['passed']
        assert len({c['id'] for c in packet['cases']})==packet['named']
        assert all(c['passed'] and c['observations'] for c in packet['cases'])
        for name,digest in packet['manifest'].items():
            p=ROOT/name;b=p.read_bytes()
            # CI's Windows checkout may convert text LF/CRLF. No arbitrary edits allowed.
            assert digest in {sha(b),sha(b.replace(b'\r\n',b'\n')),sha(b.replace(b'\r\n',b'\n').replace(b'\n',b'\r\n'))},name
    return head,win

def coverage(e,packets):
    matrix=load(QA/'e043-coverage.json')
    assert len(matrix['rows'])==66
    cases={c['id']:c for p in packets for c in p['cases']}
    for original,row in zip(e['reference_cases'],matrix['rows']):
        assert row['id']==original['id'] and row['originalStaticPrediction']==original['expected']
        assert row['originalEvidence']==original['evidence']
        assert row['originalCaseExecuted'] is False and row['originalRuntime']=='UNVERIFIED ORIGINAL-RUNTIME'
        for linked in row['inputsAndOutcomes']:
            c=cases[linked['control']];assert c['passed']==linked['passed']
            assert linked['caseSha256']==sha(json.dumps(c,sort_keys=True,ensure_ascii=False).encode())
        if row['classification']=='FINITE':assert row['inputsAndOutcomes']
    assert matrix['counts']==dict(Counter(r['classification'] for r in matrix['rows']))
    return matrix['counts']

def gate(packets):
    run=load(V/'windows-run.json');p=load(V/'windows-provenance.json');b=raw(V/'windows-job-original.log');text=b.decode('utf-8-sig')
    assert run['conclusion']=='success' and run['headSha']==p['source']
    assert sha(b)==p['returnedLog']['sha256'] and len(b)==p['returnedLog']['bytes']
    assert all(x['source']==p['source'] for x in packets)
    for label,n in [('HEADLESS COMPONENT TESTS',90),('QA03 SLIDER TESTS',56),('QA04 GEOMETRY TESTS',43),('QA05 SPINNER TESTS',66),('QA06 ACCOUNTING TESTS',99),('QA07 HEALTH TESTS',159),('QA08 NEWCOMBO TESTS',115),('QA09 HITSOUND TESTS',234),('QA10 SLIDER SOUND TESTS',229),('QA11 CONTINUOUS TESTS',247),('QA12',274),('QA13 HEADLESS',257),('QA14 HEADLESS',234)]:
        assert f'{label}: {n} passed, 0 failed' in text,label
    for s in ['0 Warning(s)','0 Error(s)','QA11 WINDOWS EMPTY: 6 passed, 0 failed','QA12 WINDOWS: 12 passed, 0 failed','QA13 WINDOWS: 24 passed, 0 failed','QA14 WINDOWS: 30 passed, 0 failed',
        'R11 HITSOUND CHECKS: 32 passed','R12 SLIDER SOUND CHECKS: 40 passed','R13 CONTINUING CHECKS: 54 passed','R13-C01 DEVELOPMENT CHECKS: 6 passed','R14 SPINNER AUDIO CHECKS: 58 passed','R14-Q01 DEVELOPMENT: 23/23','R15 DEVELOPMENT: 79 groups passed','R15 WINDOWS: 8/8','R16 DEVELOPMENT: 52/52','R16 WINDOWS: 12/12']:
        assert s in text,s
    for name,label in [('windows-results.json','QA14'),('windows-widget-results.json','QA14 WINDOWS')]:assert sha(raw(V/name))==p['packets'][label]['sha256']
    return p['source'],p['url']

if __name__=='__main__':
    e=authority();print('PASS QA14 published E043 relationships and independent binary32 floor operands')
    if '--preservation' in sys.argv:print('PASS protected historical files',preserved())
    print('PASS lossless retained packets',packed());failures();print('PASS preserved QA14 local setup/oracle failures and first complete pass')
    if (V/'windows-provenance.json').exists():
        p=final_results();print('PASS independent named controls264:226 headless component +8 source +30 Windows')
        print('PASS independent E043 coverage',coverage(e,p));print('PASS exact Windows gate, historical1911 and retained development',gate(p))
