#!/usr/bin/env python3
"""Retain ordinary Actions log lines; hash repeated historical QA evidence packets."""
import pathlib,json,sys,base64,gzip,hashlib
transport=pathlib.Path(sys.argv[1]);prefix=pathlib.Path(sys.argv[2]);source=sys.argv[3];run=int(sys.argv[4]);job=int(sys.argv[5])
raw=json.loads(transport.read_text()).encode('utf-8');omitted=[];lines=[]
for n,line in enumerate(raw.decode().splitlines(keepends=True),1):
 if len(line)>1000 and ('EVIDENCE ' in line or 'DIAGNOSTICS ' in line):
  omitted.append(dict(line=n,bytes=len(line.encode()),sha256=hashlib.sha256(line.encode()).hexdigest(),prefix=line[:100]));lines.append(f'[REPEATED QA PAYLOAD OMITTED: original line {n}; hash in gate JSON]\n')
 else:lines.append(line)
summary=''.join(lines).encode()
prefix.with_suffix('.log.gz.b64').write_bytes(base64.b64encode(gzip.compress(summary,mtime=0))+b'\n')
info=dict(source=source,run=run,job=job,raw_log_bytes=len(raw),raw_log_sha256=hashlib.sha256(raw).hexdigest(),summary_sha256=hashlib.sha256(summary).hexdigest(),retention='All ordinary log lines retained; repeated historical result packets omitted with exact line hashes. Full log via Actions job ID.',omitted_payloads=omitted,notable_lines=[l.strip() for l in raw.decode().splitlines() if any(s in l for s in ['FAIL ', 'error CS', 'Unhandled exception', 'C001 DEVELOPMENT:', 'Warning(s)', 'Error(s)'])])
prefix.with_suffix('.json').write_text(json.dumps(info,indent=2)+'\n');print('Retained gate',prefix,'raw bytes',len(raw),'sha256',info['raw_log_sha256']);transport.unlink()
