#!/usr/bin/env python3
"""Losslessly pack large new QA traces; never touches historical records."""
import base64,gzip,hashlib,json
from pathlib import Path
H=Path(__file__).resolve().parent;records=[]
for folder in ['results','attempts']:
 for p in sorted((H/folder).rglob('*')):
  if not p.is_file() or p.suffix not in ['.json','.log'] or p.stat().st_size<100000:continue
  if p.relative_to(H).as_posix() in ['results/results.json','results/source-and-artifact-review.json']:continue
  raw=p.read_bytes();q=p.with_name(p.name+'.gz.b64');q.write_bytes(base64.b64encode(gzip.compress(raw,mtime=0))+b'\n')
  assert gzip.decompress(base64.b64decode(q.read_bytes()))==raw
  records.append(dict(path=str(p.relative_to(H)),packedPath=str(q.relative_to(H)),bytes=len(raw),sha256=hashlib.sha256(raw).hexdigest(),packedSha256=hashlib.sha256(q.read_bytes()).hexdigest()));p.unlink()
(H/'packed-evidence.json').write_text(json.dumps(records,indent=2)+'\n')
print(f'PASS {len(records)} large new evidence files compressed losslessly; originals reconstructed by base64 then gzip')
