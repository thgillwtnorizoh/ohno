#!/usr/bin/env python3
"""Bounded Issue22 reproduction; use a fresh --output directory.

--audit-only verifies committed evidence without repeating execution. A full run
uses unchanged historical source and isolates only incompatible JSON envelopes
for old static checkers; original full-tree failures remain separately recorded.
"""
import argparse, hashlib, json, os, pathlib, shutil, subprocess, sys, tempfile
H=pathlib.Path(__file__).resolve().parent; R=H.parents[3]
p=argparse.ArgumentParser();p.add_argument('--output',required=True);p.add_argument('--dotnet');p.add_argument('--pwsh');p.add_argument('--audit-only',action='store_true');a=p.parse_args()
O=pathlib.Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True)
sha=lambda path:hashlib.sha256(path.read_bytes()).hexdigest()
provenance=json.loads((H/'provenance.json').read_text()); manifest=json.loads((H/'manifest.json').read_text())
for path,digest in provenance['compiled_source_candidates'].items(): assert sha(R/path)==digest,path
assert subprocess.check_output(['git','-C',R,'diff','--name-only',provenance['exact_functional_sha'],'HEAD','--','reconstructed/CircleGame','reconstructed/CorpusAudit'],text=True)==''
for c in manifest['cases']:assert sha(H/c['path'])==c['sha256'],c['id']
assert sha(H/'resources/tone.wav')==manifest['resource_sha256']
if a.audit_only:
    if (H/'seal.json').exists():
        for path,digest in json.loads((H/'seal.json').read_text())['files'].items():assert sha(H/path)==digest,path
    q=json.loads((H/'results.json').read_text());assert (q['assertions'],q['passed'],q['failed'],len(q['cases']))==(2180,2180,0,47)
    print('PASS sealed47 fixtures, immutable sources, recorded2180/2180 and packet hashes');sys.exit(0)
assert a.dotnet and a.pwsh,'Supply --dotnet and --pwsh for actual execution'
assert O!=H,'Use a fresh --output directory to preserve published evidence'
commands=[]
env={**os.environ,'PATH':str(pathlib.Path(a.dotnet).resolve().parent)+os.pathsep+os.environ['PATH'],'DOTNET_CLI_TELEMETRY_OPTOUT':'1'}
def run(cmd,name,allow_failure=False):
    with (O/name).open('w') as f: result=subprocess.run(list(map(str,cmd)),cwd=R,env=env,stdout=f,stderr=subprocess.STDOUT)
    commands.append(dict(command=list(map(str,cmd)),exit=result.returncode,log=name))
    (O/'commands.json').write_text(json.dumps(commands,indent=2)+'\n')
    if not allow_failure:assert result.returncode==0,(name,result.returncode)
    return result.returncode
run([a.dotnet,'run','--project',H/'Checks','-c','Release','--',H,O/'results.json'],'focused.log')
run([a.dotnet,'run','--project',R/'reconstructed/C001DevelopmentChecks','-c','Release','--',R],'retained-development.log')
for n in [10,11]:
    cmd=[a.pwsh,'-NoProfile','-File',R/f're/qa/qa{n}/Run-QA{n}.ps1','-ResultPath',O/f'retained-qa{n}.json']
    if n==11:cmd+=['-SkipWindows']
    run(cmd,f'retained-qa{n}.log')
static=[]
with tempfile.TemporaryDirectory(prefix='ohno-issue22-static-') as temp:
    isolated=pathlib.Path(temp)/'re';shutil.copytree(R/'re/tools',isolated/'tools');excluded=[]
    for path in (R/'re/evidence').glob('*/*.json'):
        try:j=json.loads(path.read_text())
        except (ValueError,UnicodeError):continue
        if not isinstance(j,dict) or not isinstance(j.get('methods',{}),dict):excluded.append(str(path.relative_to(R)));continue
        dest=isolated/'evidence'/path.relative_to(R/'re/evidence');dest.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(path,dest)
    for name,evidence in [('slider_timing','05/slider-timing.json'),('circle_hitsound','37/circle-hitsound.json'),('hitsound_resources','38/hitsound-resources.json'),('slider_hitsounds','39/slider-hitsounds.json'),('slider_continuous_audio','40/slider-continuous-audio.json')]:
        full=run([sys.executable,R/f're/tools/check_{name}.py',R/'re/evidence'/evidence],f'{name}-full-tree.log',True)
        code=run([sys.executable,isolated/'tools'/f'check_{name}.py',isolated/'evidence'/evidence],f'static-{name}.log')
        static.append(dict(name=name,full_tree_exit=full,isolated_exit=code,excluded_envelopes=excluded))
(O/'static-checks.json').write_text(json.dumps(static,indent=2)+'\n')
print('PASS bounded focused, C001 development, QA10/11 and five static semantic envelopes; original runtime NOT RUN')
