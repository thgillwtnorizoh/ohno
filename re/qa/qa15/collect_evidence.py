#!/usr/bin/env python3
"""Collect actual returned CI logs/metadata without executing or changing production."""
import base64,gzip,hashlib,json,sys
from pathlib import Path
QA=Path(__file__).resolve().parent
V=QA/'verification'
def sha(b):return hashlib.sha256(b).hexdigest()
def write(p,x):p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+'\n',encoding='utf-8',newline='\n')
def main():
    log=Path(sys.argv[1]).read_bytes();run=json.loads(Path(sys.argv[2]).read_bytes());jobs=json.loads(Path(sys.argv[3]).read_bytes())
    text=log.decode('utf-8');packets={}
    for label,name in [('QA15 HEADLESS','windows-headless-results.json'),('QA15 WINDOWS','windows-widget-results.json')]:
        prefix=label+' EVIDENCE '
        found=[l.split(prefix,1)[1].strip() for l in text.splitlines() if prefix in l]
        assert len(found)==1,(label,len(found))
        b=gzip.decompress(base64.b64decode(found[0]));data=json.loads(b)
        assert data['source']==run['head_sha']
        (V/(name+'.gz.b64')).write_bytes(base64.b64encode(gzip.compress(b,mtime=0))+b'\n')
        packets[label]=dict(path=name+'.gz.b64',bytes=len(b),sha256=sha(b),source=data['source'],named=data['named'],passed=data['passed'],failed=data['failed'])
    (V/'windows-job-original.log.gz.b64').write_bytes(base64.b64encode(gzip.compress(log,mtime=0))+b'\n')
    write(V/'windows-run.json',run);write(V/'windows-jobs.json',jobs)
    write(V/'windows-provenance.json',dict(source=run['head_sha'],run=run['id'],url=run['html_url'],conclusion=run['conclusion'],returned_log=dict(bytes=len(log),sha256=sha(log)),packets=packets))
    print(json.dumps(packets,indent=2))
if __name__=='__main__':main()
