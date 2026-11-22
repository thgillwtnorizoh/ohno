#!/usr/bin/env python3
"""Work2 development reproduction; original research and fixtures remain read-only."""
import argparse,csv,hashlib,json,os,pathlib,subprocess
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('output');a=p.parse_args()
R=pathlib.Path(__file__).resolve().parents[4];O=pathlib.Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True)
dot=str(pathlib.Path(a.dotnet).resolve());env={**os.environ,'PATH':str(pathlib.Path(dot).parent)+os.pathsep+os.environ['PATH'],'DOTNET_HOST_PATH':dot,'DOTNET_CLI_TELEMETRY_OPTOUT':'1'}
def run(cmd,name):
 with (O/name).open('w') as f:subprocess.run(list(map(str,cmd)),cwd=R,env=env,stdout=f,stderr=subprocess.STDOUT,check=True)
sha=lambda p:hashlib.sha256(pathlib.Path(p).read_bytes()).hexdigest()
for name in ['control-count','traversal-profile']:run(['python',R/f're/campaigns/C001/{name}/build_fixtures.py','--check'],name+'-hashes.log')
run([dot,'build','reconstructed/C001LinearControlChecks','-c','Release','--no-incremental','--nologo'],'build.log')
run([dot,'run','--project','reconstructed/C001LinearControlChecks','-c','Release','--no-build','--',R,O],'run.log')
j=json.loads((O/'results.json').read_text());assert (j['prepared'],j['refused'],j['added'],j['lost'],j['failed'])==(112,56,74,0,0)
with (O/'dispositions.csv').open('w',newline='') as f:
 keys=['id','sha256','raw','normalized','previouslyPrepared','prepared','added','error','index','indexedError','legacyError','sampleError'];w=csv.DictWriter(f,keys+['auditCode','exceptionType']);w.writeheader()
 for c in j['cases']:w.writerow({**{k:c[k] for k in keys},'auditCode':c['audit']['Code'],'exceptionType':c['audit']['ExceptionType']})
dll=R/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll';linked=R/'reconstructed/C001LinearControlChecks/bin/Release/net8.0/CorpusAudit.dll';assert sha(dll)==sha(linked)
sources={str(p.relative_to(R)):sha(p) for p in [R/'reconstructed/CircleGame/Gameplay/Beatmap.cs',R/'.github/workflows/r04-build.yml',*sorted((R/'reconstructed/C001LinearControlChecks').glob('*.cs')),*sorted((R/'reconstructed/C001LinearControlWindowsChecks').glob('*.cs'))]}
(O/'provenance.json').write_text(json.dumps(dict(source=j['source'],state=j['state'],sourceHashes=j['sourceHashes'],assetHash=j['assetHash'],sdk=subprocess.check_output([dot,'--version'],text=True).strip(),runtime=j['runtime'],productionDllSha256=sha(dll),linkedDllSha256=sha(linked),checksDllSha256=sha(R/'reconstructed/C001LinearControlChecks/bin/Release/net8.0/C001LinearControlChecks.dll'),sources=sources,resultsSha256=sha(O/'results.json'),passed=j['passed'],failed=0,originalRuntime='NOT RUN'),indent=2)+'\n')
print(f"PASS {j['passed']} development assertions;112/56;+74/0 losses; linked production/source/results seals")
