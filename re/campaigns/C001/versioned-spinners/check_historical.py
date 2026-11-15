#!/usr/bin/env python3
"""Run unchanged legacy checkers in their dictionary-evidence schema envelope.
Later list-valued execution summaries contain no method table and crash those old
checkers before completion. Preserve and disclose that full-tree failure; do not
edit scripts or historical evidence. Copy every dictionary evidence file and every
tool byte-identically, excluding only the enumerated non-dictionary summary files.
"""
from pathlib import Path
import json,shutil,subprocess,sys,tempfile
R=Path(__file__).resolve().parents[4]
with tempfile.TemporaryDirectory(prefix='spinner-historical-schema-') as temp:
 root=Path(temp)/'re';(root/'evidence').mkdir(parents=True);shutil.copytree(R/'re/tools',root/'tools',ignore=shutil.ignore_patterns('__pycache__'))
 omitted=[]
 for p in sorted((R/'re/evidence').glob('*/*.json')):
  if not isinstance(json.loads(p.read_bytes()),dict):omitted.append(str(p.relative_to(R)));continue
  out=root/'evidence'/p.relative_to(R/'re/evidence');out.parent.mkdir(exist_ok=True);shutil.copyfile(p,out);assert out.read_bytes()==p.read_bytes()
 assert len(omitted)==5,omitted
 for tool,evidence in [('check_newcombo_preparation.py','36/newcombo-preparation.json'),('check_spinner_audio.py','41/spinner-audio.json')]:
  assert (root/'tools'/tool).read_bytes()==(R/'re/tools'/tool).read_bytes()
  subprocess.run([sys.executable,str(root/'tools'/tool),str(root/'evidence'/evidence)],check=True)
 print('Schema-only exclusions (no methods; original files unchanged):',','.join(omitted))
