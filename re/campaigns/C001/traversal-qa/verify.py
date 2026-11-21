#!/usr/bin/env python3
"""Reproduce independent QA without editing production or historical expectations."""
import argparse,hashlib,json,os,subprocess
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact');p.add_argument('old');p.add_argument('output');a=p.parse_args();H=Path(__file__).resolve().parent;R=H.parents[3];O=Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True);dotnet=str(Path(a.dotnet).resolve());env={**os.environ,'DOTNET_HOST_PATH':dotnet};sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
def run(args,log):
 with (O/log).open('w') as f:subprocess.run([str(x) for x in args],env=env,stdout=f,stderr=subprocess.STDOUT,check=True)
def git(path,*args):return subprocess.check_output(['git','-C',str(path),*args],text=True).strip()
run(['python',H/'review.py'],'review.log');run(['python',R/'re/campaigns/C001/traversal-profile/build_fixtures.py','--check'],'fixture-hashes.log');pins=[]
for label,path,source in [('baseline',Path(a.old).resolve(),'22a403505f13461477343b9c6a251980716dd90f'),('independent',Path(a.exact).resolve(),'6a152648c72773eb33d37f8ac01f18ca35ab1f41')]:
 assert git(path,'rev-parse','HEAD')==source and not git(path,'status','--porcelain')
 run([dotnet,'build',path/'reconstructed/CorpusAudit/CorpusAudit.csproj','-c','Release','--no-incremental','--nologo'],label+'-build.log')
 run([dotnet,'run','--project',H/'Checks','-c','Release',f'-p:ExactRoot={path}','--',R,O/label]+(['baseline'] if label=='baseline' else []),label+'-run.log')
 j=json.loads((O/label/'results.json').read_text());assert j['failed']==0 and j['source']==source and j['state']=='clean'
 if label=='baseline':
  assert (j['passed'],j['prepared'],j['rejected'])==(729,41,65)
  assert json.loads((O/label/'shapes.json').read_text())==json.loads((H/'baseline/shapes.json').read_text())
 else:assert (j['passed'],j['prepared'],j['rejected'],j['added'],j['lost'],j['audio'],j['errors'])==(5732,86,20,45,0,82,4)
 prod=path/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll';linked=H/'Checks/bin/Release/net8.0/CorpusAudit.dll';assert sha(prod)==sha(linked)
 pins.append(dict(label=label,source=source,sourceState='clean',productionDllSha256=sha(prod),linkedDllSha256=sha(linked),checksDllSha256=sha(H/'Checks/bin/Release/net8.0/Checks.dll'),resultSha256=sha(O/label/'results.json'),passed=j['passed'],failed=0))
 assert not git(path,'status','--porcelain')
seal=dict(sdk=subprocess.check_output([dotnet,'--version'],text=True).strip(),runtime=j['runtime'],pins=pins,qaSourceHashes={str(p.relative_to(H)):sha(p) for p in [H/'Checks/Program.cs',H/'Checks/Adversaries.cs',H/'Checks/Checks.csproj',H/'verify.py',H/'review.py',H/'expected-sources.json',H/'old-sources.json']},originalRuntime='NOT RUN',newWindowsRun='NOT RUN')
(O/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n');print('PASS5732 independent +729 old-source baseline checks;86/20,45new,0losses,82audio/4errors;exact clean source and linked DLL identities')
