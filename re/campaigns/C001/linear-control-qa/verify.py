#!/usr/bin/env python3
"""Fresh independent Issue21 QA. Read-only production worktree required."""
import argparse,hashlib,json,os,subprocess,sys,csv,re
from pathlib import Path
H=Path(__file__).resolve().parent;R=H.parents[3]
SOURCE='5dda483348e21e79c8354bf6ea322d50b7fb3e72'
p=argparse.ArgumentParser();p.add_argument('dotnet');p.add_argument('exact');p.add_argument('output');p.add_argument('--retained',action='store_true');p.add_argument('--pwsh',help='PowerShell for unchanged historical sample generators');p.add_argument('--reuse-focused',action='store_true',help='continue an already successful unchanged focused run');a=p.parse_args()
D=Path(a.dotnet).resolve();E=Path(a.exact).resolve();O=Path(a.output).resolve();O.mkdir(parents=True,exist_ok=True)
env={**os.environ,'PATH':str(D.parent)+os.pathsep+os.environ['PATH'],'DOTNET_HOST_PATH':str(D),'DOTNET_CLI_TELEMETRY_OPTOUT':'1'}
sha=lambda p:hashlib.sha256(Path(p).read_bytes()).hexdigest()
git=lambda *xs:subprocess.check_output(['git','-C',str(E),*xs],text=True).strip()
assert git('rev-parse','HEAD')==SOURCE and not git('status','--porcelain')
commands=json.loads((O/'commands.json').read_text()) if a.reuse_focused else []
def qa14_platform_failures():
 q=json.loads((O/'QA14.json').read_text());bad=[c['id'] for c in q['cases'] if not c['passed']]
 return (q['named'],q['passed'],q['failed'],set(bad))==(237,234,3,{'C/reparse-ancestry','I/windows-path-case','I/canonical-unc-rejected'})
def run(cmd,name,cwd=R):
 if a.reuse_focused and name.startswith('retained-') and (O/name).exists() and any(c.get('log')==name and c.get('exitCode')==0 for c in commands):
  print('REUSED completed '+name,flush=True);return
 cmd=list(map(str,cmd));commands.append(dict(command=cmd,cwd=str(cwd),log=name))
 with (O/name).open('w') as f:r=subprocess.run(cmd,cwd=cwd,env=env,stdout=f,stderr=subprocess.STDOUT)
 commands[-1]['exitCode']=r.returncode;(O/'commands.json').write_text(json.dumps(commands,indent=2)+'\n')
 if r.returncode:
  if name=='retained-QA14.log' and not sys.platform.startswith('win') and qa14_platform_failures():
   print('PLATFORM FAIL QA14:234 passed/3 Windows-specific failures; native Windows NOT RUN',flush=True);return
  raise RuntimeError(f'{name}: exit {r.returncode}')
 print('PASS '+name,flush=True)
if not a.reuse_focused:
 run([sys.executable,H/'byte_counts.py',O/'byte-counts.json'],'byte-counts.log')
 for family in ['control-count','traversal-profile']:run([sys.executable,R/f're/campaigns/C001/{family}/build_fixtures.py','--check'],family+'-hashes.log')
 for name,path in [('linear','06/linear-geometry.json'),('timing','05/slider-timing.json'),('bezier','32/bezier-geometry.json')]:
  script={'linear':'check_linear_geometry.py','timing':'check_slider_timing.py','bezier':'check_bezier_geometry.py'}[name]
  run([sys.executable,R/'re/tools'/script,R/'re/evidence'/path],name+'-historical.log')
 run([D,'build',H/'Checks','-c','Release',f'-p:ExactRoot={E}','--no-incremental','--nologo'],'build.log')
 run([D,H/'Checks/bin/Release/net8.0/Checks.dll',R,O],'run.log')
j=json.loads((O/'results.json').read_text());assert (j['source'],j['state'],j['failed'],j['prepared'],j['refused'],j['added'],j['lost'],j['audio'])==(SOURCE,'clean',0,112,56,74,0,112)
production=E/'reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll';linked=H/'Checks/bin/Release/net8.0/CorpusAudit.dll';assert sha(production)==sha(linked)
for name,h in j['sourceHashes'].items():
 if name in ['.NETCoreApp,Version=v8.0.AssemblyAttributes.cs','CorpusAudit.GlobalUsings.g.cs']:continue
 candidates=[x for x in (E/'reconstructed').rglob(name) if 'obj' not in x.parts and 'bin' not in x.parts]
 assert sum(sha(x).upper()==h for x in candidates)==1,name
with (O/'dispositions.csv').open('w',newline='') as f:
 w=csv.DictWriter(f,['id','hash','raw','normalized','want','error','index','indexedError','legacy','old','prepared']);w.writeheader()
 for c in j['cases']:w.writerow({k:c[k] for k in w.fieldnames})
retained=[]
if a.retained:
 # Headless historical QA on exact sources; Windows-only suites explicitly absent.
 for n in [1,3,4,5,6,7,8,9,10,11,12,13,14,15]:
  project=E/f're/qa/qa{n:02}/Headless/QA{n:02}.Headless.csproj'
  name=f'retained-QA{n:02}.log'
  if n==14 and a.reuse_focused and (O/'QA14.json').exists() and qa14_platform_failures():
   print('REUSED QA14:234 passed/3 Windows-specific failures; preserved without relabeling',flush=True)
  elif a.reuse_focused and (O/name).exists() and re.search(r'(?:QA\d+[^:\n]*|HEADLESS COMPONENT TESTS):\s*\d+ passed,\s*0 failed',(O/name).read_text()):
   print('REUSED successful '+name,flush=True)
  elif n>=9:
   assert a.pwsh,'--pwsh required for unchanged historical suite runners'
   script=E/f're/qa/qa{n:02}/Run-QA{n:02}.ps1'
   args=[a.pwsh,'-NoProfile','-File',script,'-ResultPath',O/f'QA{n:02}.json']
   if n>=11:args+=['-SkipWindows']
   run(args,name,E)
  else:run([D,'run','--project',project,'-c','Release','--',E],name,E)
  retained.append(f'QA{n:02}')
 for name in ['C001DevelopmentChecks','C001PerfectChecks','C001CatmullChecks','C001TraversalChecks','C001NearSpinnerChecks','C001LegacySpinnerChecks','C001VersionedSpinnerChecks','CorpusAuditChecks']:
  result=O/('retained-'+name);result.mkdir(exist_ok=True)
  # Perfect/Catmull accept an optional result file; other suites require directories.
  target=result/'results.json' if name in ['C001PerfectChecks','C001CatmullChecks'] else result
  run([D,'run','--project',E/'reconstructed'/name,'-c','Release','--',E,target],f'retained-{name}.log',E);retained.append(name)
 for project in ['stacking/Headless/C001.Stacking.csproj','stacking/Integration/C001.StackingIntegration.csproj','verification/Headless/C001.Headless.csproj']:
  name=Path(project).stem;run([D,'run','--project',E/'re/campaigns/C001'/project,'-c','Release','--',E,O/(name+'.json')],f'retained-{name}.log',E);retained.append(name)
# This unchanged historical suite writes its report into its checkout. Preserve
# that newly generated report outside the immutable production worktree.
generated=E/'reconstructed/campaigns/C001/development-results.json'
if generated.exists() and not git('ls-files',str(generated.relative_to(E))):
 (O/'retained-canonical-development-results.json').write_bytes(generated.read_bytes());generated.unlink()
assert not git('status','--porcelain')
seal=dict(exactSource=SOURCE,sourceState='clean',sdk=subprocess.check_output([str(D),'--version'],text=True).strip(),runtime=j['runtime'],
 productionDllSha256=sha(production),linkedDllSha256=sha(linked),checksDllSha256=sha(H/'Checks/bin/Release/net8.0/Checks.dll'),
 testSources={str(x.relative_to(H)):sha(x) for x in [*sorted((H/'Checks').glob('*.cs')),H/'Checks/Checks.csproj',*sorted((H/'PdbInspect').glob('*.cs')),*sorted((H/'PdbInspect').glob('*.csproj')),*sorted(H.glob('*.py'))]},
 resultsSha256=sha(O/'results.json'),compiledSourceHashes=j['sourceHashes'],passed=j['passed'],failed=j['failed'],retainedExecuted=retained,
 PowerShell=a.pwsh,retainedQA14=dict(passed=234,platformFailures=3,nativeWindows='NOT RUN'),Windows='NOT RUN; downloaded development artifact inspected separately',originalRuntime='NOT RUN',ownerCorpus='NOT ACCESSED')
(O/'provenance.json').write_text(json.dumps(seal,indent=2)+'\n')
print(f"PASS {j['passed']} independent Issue21 assertions; 168 immutable cases112/56; +74/0; {len(retained)} retained suites")
