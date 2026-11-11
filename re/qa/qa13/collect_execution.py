"""Recover exact QA13 payloads from the returned GitHub job-log text.
Input is a private JSON transport containing {'log': <exact returned string>}.
The public packet retains exact UTF8 log bytes in reversible gzip/base64.
"""
import base64,gzip,hashlib,json,re,sys
from pathlib import Path
QA=Path(__file__).resolve().parent;ROOT=QA.parents[2]
def sha(b):return hashlib.sha256(b).hexdigest()
def save(name,data):(QA/'verification'/name).write_bytes(data)
def packed(name,data):save(name,(base64.b64encode(gzip.compress(data,mtime=0)).decode()+'\n').encode())
def main(path):
    log=json.loads(Path(path).read_text(encoding='utf-8'))['log'];raw=log.encode('utf-8')
    packed('windows-job-original.log.gz.b64',raw)
    save('windows-job.log',raw.replace(b'\r\n',b'\n'))
    packets={}
    for label,name in [('QA13','windows-results.json.gz.b64'),('QA13 WINDOWS','windows-widget-results.json.gz.b64')]:
        found=re.findall(re.escape(label)+r' EVIDENCE ([A-Za-z0-9+/=]+)',log);assert len(found)==1
        data=gzip.decompress(base64.b64decode(found[0]));expected=re.findall(re.escape(label)+r' SHA256 ([0-9a-f]+)',log);assert expected==[sha(data)]
        packed(name,data);d=json.loads(data);assert d['failed']==0
        packets[label]=dict(named=d['named'],passed=d['passed'],failed=d['failed'],bytes=len(data),sha256=sha(data),source=d['source'],classifications=d.get('classifications',{'EXECUTED WINDOWS COMPONENT':d['named']}))
        if label=='QA13':
            correspondence={}
            for f,h in d['manifest'].items():
                current=(ROOT/f).read_bytes();normalized=current.replace(b'\r\n',b'\n')
                choices={'raw':sha(current),'LF':sha(normalized),'CRLF':sha(normalized.replace(b'\n',b'\r\n'))}
                assert h in choices.values(),f
                correspondence[f]=dict(runner=h,canonicalLF=choices['LF'],profile=next(k for k,v in choices.items() if v==h))
            save('source-correspondence.json',(json.dumps(correspondence,indent=2)+'\n').encode())
    summary='\n'.join(line for line in log.splitlines() if re.search(r'CHECKS:|TARGETS:|FIXTURE|TESTS:|BACKEND:|WINDOWS:|WINDOWS EMPTY:|QA12:|QA13 HEADLESS:|Warning.s.|Error.s.|DEVELOPMENT:|8.0.427',line) and 'EVIDENCE' not in line and len(line)<600)+'\n'
    save('gate-summary.log',summary.encode())
    run=json.loads((QA/'verification/windows-run.json').read_text());job=json.loads((QA/'verification/windows-job.json').read_text())['jobs'][0]
    assert run['head_sha']==packets['QA13']['source']==packets['QA13 WINDOWS']['source']
    p=dict(schema='QA13.execution.v1',source=run['head_sha'],production='3f9a8f58d29480729d1d305f075791769163f46d',
        baseline='74c47dd452befb741d659e71b2f5267ceb7171a2',run=run['id'],job=job['id'],url=run['html_url'],
        result='success',returnedLog=dict(bytes=len(raw),sha256=sha(raw)),readableLog=dict(bytes=len(raw.replace(b'\r\n',b'\n')),sha256=sha(raw.replace(b'\r\n',b'\n'))),packets=packets,
        note='Exact returned UTF8 BOM/CRLF log retained; readable log LF; runner source hashes match current source with only checkout LF/CRLF conversion.')
    save('windows-provenance.json',(json.dumps(p,indent=2)+'\n').encode())
    print(json.dumps(p,indent=2))
if __name__=='__main__':main(sys.argv[1])
