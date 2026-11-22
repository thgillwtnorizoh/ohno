#!/usr/bin/env python3
"""Derive count/equality even for inputs refused before conversion in production."""
import hashlib,json,math,struct,sys
from pathlib import Path
H=Path(__file__).resolve().parent;root=H.parents[3];home=root/'re/campaigns/C001/control-count'
f32=lambda x:struct.unpack('<f',struct.pack('<f',x))[0]
rows=[]
for c in json.loads((home/'manifest.json').read_text())['cases']:
 raw=(home/c['path']).read_bytes();assert hashlib.sha256(raw).hexdigest().lower()==c['sha256'].lower()
 line=next(x for x in raw.decode().splitlines() if x.startswith('128,192,2000,'));r=line.split(',');cells=r[5].split('|')
 head=tuple(f32(int(min(512,max(0,float(x))))) for x in r[:2]);points=[];invalid=None
 for cell in cells[1:]:
  parts=cell.split(':')
  if len(parts)!=2:invalid='arity-or-extra-marker';break
  try:coords=tuple(float(x) for x in parts)
  except ValueError:invalid='number';break
  if not all(math.isfinite(x) for x in coords):invalid='nonfinite';break
  if not all(-2147483648<=x<=2147483647 for x in coords):invalid='int32-range';break
  points.append(tuple(f32(int(x)) for x in coords))
 normalized=None;equal=None
 if invalid is None:
  equal=bool(points and points[0]==head);normalized=len(points)+(not equal)
  assert (len(points),normalized,equal)==(c['rawCoordinates'],c['normalized'],c['firstEqualsHead']),c['id']
 assert len(cells)-1==c['rawCells'],c['id']
 rows.append(dict(id=c['id'],sha256=c['sha256'],sourceMarker=cells[0],rawCells=len(cells)-1,convertedNormalized=normalized,firstEqualsHead=equal,invalid=invalid))
out=Path(sys.argv[1]);out.write_text(json.dumps(dict(cases=rows,allHashes=168,allRawCellCounts=168,validConvertedCounts=sum(x['invalid'] is None for x in rows),invalidCoordinateCases=sum(x['invalid'] is not None for x in rows)),indent=2)+'\n')
print(f"PASS168 immutable hashes/raw-cell counts;{sum(x['invalid'] is None for x in rows)} byte-derived converted normalized/equality counts (including excluded inputs);{sum(x['invalid'] is not None for x in rows)} intentionally invalid coordinate cases")
