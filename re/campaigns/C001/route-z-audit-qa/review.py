#!/usr/bin/env python3
"""Source boundary, historical preservation and exact gameplay-tree review."""
from pathlib import Path
import subprocess,hashlib,json
H=Path(__file__).resolve().parent;ROOT=H.parents[3]
def git(*args):return subprocess.check_output(['git','-C',str(ROOT),*args])
base='9ed9eb5c58f631f89fb66fc97802cfbf9ca86e44';new='f6d0411abc865134a1dafb7fb7ef1473b621a84f'
a=git('rev-parse',base+':reconstructed/CircleGame').decode().strip();b=git('rev-parse',new+':reconstructed/CircleGame').decode().strip();assert a==b=='2075c2a7dd9816e6e330db44da2682bf8b4fe9a8'
assert not git('diff',base,new,'--raw','--','reconstructed/CircleGame').strip()
files=git('ls-tree','-r','--name-only',new,'--','reconstructed/CircleGame').decode().splitlines()
path='reconstructed/CorpusAudit/AuditRunner.cs';old=git('show',base+':'+path).decode();now=git('show',new+':'+path).decode()
insert='''            // Issue11: fixed production Route Z guards only. Match original text
            // ordinally; chart-controlled fragments/case variants are not evidence.
            if(e.Message == "Legacy spinner stack profile lacks a zero-displacement certificate.")
                return new("unsupported-admission","stack-certificate-unproven",name);
            if(e.Message == "Legacy spinner zero-displacement certificate exceeds pair work limit.")
                return new("unsupported-admission","stack-certificate-work-limit",name);
'''
assert now.count(insert)==1 and now.replace(insert,'')==old,'more than two diagnostic mappings changed'
for f in ['Program.cs','CorpusAudit.csproj']:assert git('show',base+':reconstructed/CorpusAudit/'+f)==git('show',new+':reconstructed/CorpusAudit/'+f)
original={}
for folder in ['corpus-audit-qa','corpus-audit-qa-v2']:
 prefix='re/campaigns/C001/'+folder
 assert not git('diff','c6447af',new,'--name-only','--',prefix).strip()
 for f in git('ls-tree','-r','--name-only','c6447af','--',prefix).decode().splitlines():
  raw=(ROOT/f).read_bytes();assert raw==git('show','c6447af:'+f),f;original[f]=hashlib.sha256(raw).hexdigest()
assert "8c5ba835f260401a56dea27367c6a6e9c55e9d84" in (ROOT/'re/campaigns/C001/corpus-audit-qa/probe.py').read_text()
(H/'source-review.json').write_text(json.dumps(dict(baseGameplay=base,newSource=new,entireGameplayTree=a,entireGameplayFileCount=len(files),gameplayRawDiffEmpty=True,onlyTwoFixedMappings=True,restOfAuditRunnerByteIdentical=True,toolEntryProjectUnchanged=True,historicalSourceAndFixturesUnchanged=original),indent=2)+'\n')
print('PASS entire gameplay tree unchanged:',len(files),'files; only two tool mappings; old probes/fixtures preserved')
