#!/usr/bin/env python3
"""Extract only reconstruction DLL/PDB to temporary paths for metadata inspection."""
import zipfile,sys,struct
from pathlib import Path
z=zipfile.ZipFile(sys.argv[1]);o=Path(sys.argv[2]);o.mkdir(parents=True,exist_ok=True)
exe=z.read(next(n for n in z.namelist() if n.endswith('/Ohno.CircleGame.exe')))
magic=bytes.fromhex('8b1202b96a612038727b930214d7a03213f5b9e6efae3318ee3b2dce24b36aae');pos=struct.unpack_from('<q',exe,exe.index(magic)-8)[0]
def val(fmt):
 global pos
 v=struct.unpack_from(fmt,exe,pos);pos+=struct.calcsize(fmt);return v[0] if len(v)==1 else v
def string():
 global pos
 size=0;shift=0
 while True:
  b=val('<B');size|=(b&127)<<shift
  if not b&128:break
  shift+=7
 v=exe[pos:pos+size].decode();pos+=size;return v
major=val('<I');minor=val('<I');count=val('<i');string();assert major==6;val('<qqqqQ')
for _ in range(count):
 off=val('<q');size=val('<q');compressed=val('<q');kind=val('<B');name=string()
 if name=='Ohno.CircleGame.dll':
  assert compressed==0;(o/'Ohno.CircleGame.dll').write_bytes(exe[off:off+size])
(o/'Ohno.CircleGame.pdb').write_bytes(z.read(next(n for n in z.namelist() if n.endswith('/Ohno.CircleGame.pdb'))))
print('Extracted delivered reconstruction DLL/PDB for read-only metadata audit; not executed')
