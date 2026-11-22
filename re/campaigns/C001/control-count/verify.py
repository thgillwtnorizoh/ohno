#!/usr/bin/env python3
"""Reproduce Issue19 research using unchanged exact production binaries."""
import argparse,hashlib,json,os,subprocess
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3]
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact');p.add_argument('archive');p.add_argument('output');a=p.parse_args()
E=Path(a.exact).resolve();O=Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True)
dotnet=str(Path(a.dotnet).resolve());env={**os.environ,'DOTNET_HOST_PATH':dotnet,'DOTNET_CLI_TELEMETRY_OPTOUT':'1'}
sha=lambda p:hashlib.sha256(Path(p).read_bytes()).hexdigest()
def run(args,name):
    with (O/name).open('w') as f:subprocess.run([str(x) for x in args],stdout=f,stderr=subprocess.STDOUT,check=True,env=env)
run(['python',H/'build_fixtures.py','--check'],'fixtures.log')
run(['python',H/'review.py',E,O/'source-review.json'],'review.log')
run(['python',H/'recover.py',a.archive,O/'static-repeat.json'],'static.log')
assert sha(O/'static-repeat.json')==sha(H/'static-evidence.json')
for name,path in [('linear','06/linear-geometry.json'),('timing','05/slider-timing.json'),('bezier','32/bezier-geometry.json')]:
    script={'linear':'check_linear_geometry.py','timing':'check_slider_timing.py','bezier':'check_bezier_geometry.py'}[name]
    run(['python',R/'re/tools'/script,R/'re/evidence'/path],name+'-historical.log')
run([dotnet,'build',E/'reconstructed/CorpusAudit/CorpusAudit.csproj','-c','Release','--no-incremental','--nologo'],'build.log')
run([dotnet,'run','--project',H/'Checks','-c','Release',f'-p:ExactRoot={E}','--',R,O],'production.log')
j=json.loads((O/'results.json').read_text());assert (j['passed'],j['failed'],j['prepared'],j['refused'],len(j['probes']),j['audio'])==(3324,0,38,130,110,38)
assert (j['source'],j['state'])==('6a152648c72773eb33d37f8ac01f18ca35ab1f41','clean')
dll=E/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll';linked=H/'Checks/bin/Release/net8.0/CorpusAudit.dll';assert sha(dll)==sha(linked)
assert not subprocess.check_output(['git','-C',str(E),'status','--porcelain'],text=True).strip()
seal=dict(startingMain='77b4d582f5d54e3f52018a6b9b6fc593c9742045',exactSource=j['source'],sourceState=j['state'],sdk=subprocess.check_output([dotnet,'--version'],text=True).strip(),runtime=j['runtime'],
    productionDllSha256=sha(dll),linkedProductionDllSha256=sha(linked),checksDllSha256=sha(H/'Checks/bin/Release/net8.0/Checks.dll'),
    packetHashes={str(x.relative_to(H)):sha(x) for x in [H/'verify.py',H/'recover.py',H/'review.py',H/'build_fixtures.py',H/'expected-sources.json',H/'manifest.json',*sorted((H/'Checks').glob('*.cs')),H/'Checks/Checks.csproj']},
    resultsSha256=sha(O/'results.json'),staticSha256=sha(H/'static-evidence.json'),passed=j['passed'],failed=0,
    originalRuntime='NOT RUN',Windows='NOT RUN',ownerCorpus='NOT ACCESSED',fullRegression='NOT RUN; production unchanged; historical evidence retained')
(O/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n')
print('PASS3324 independent checks;168fixtures38 prepared130 refused;110 isolated schedules;exact clean source and linked DLL')
