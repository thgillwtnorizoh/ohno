#!/usr/bin/env python3
"""Packet consistency and independently fixed original identities; not runtime."""
from pathlib import Path
import json,hashlib,subprocess,sys
H=Path(__file__).resolve().parent;ROOT=H.parents[3]
j=json.loads((H/'static-evidence.json').read_text())
assert j['assertionCount']==len(j['assertions'])==55 and len(j['methods'])==13 and j['historicalIdentities']==10
assert j['methods']['M0603']['body_sha256']=='538b6f46d0e72d0d0c4f89319ee94e4e14f4589ad0526a750ba1f42517008b16'
assert j['methods']['M28ec']['body_sha256']=='dca567b47a012156e0dea0f16c104cdc01f54679fb2d62d4a1f3c84e8b902364'
assert j['spinnerSetter']['implementation']=='M0603' and j['earlyPassHasNoSpinnerTypeTest'] and j['originalExecuted'] is False
old=json.loads((ROOT/'re/campaigns/C001/original-behavior/static-inventory.json').read_text())['methods']
for k,v in j['methods'].items():
 key=hex(0x6000000+int(k[1:],16))
 if key in old:assert v['body_sha256']==old[key]['body_sha256'],k
m=json.loads((H/'manifest.json').read_text());assert len(m['cases'])==81
for c in m['cases']:assert hashlib.sha256((H/c['path']).read_bytes()).hexdigest()==c['sha256'],c['id']
assert next(c for c in m['cases'] if c['id']=='v05-spinner-after')['original']['heights']==[1,0]
assert next(c for c in m['cases'] if c['id']=='v06-spinner-after')['original']['heights']==[0,0]
assert next(c for c in m['cases'] if c['id']=='v14-tail-center')['original']['preparedHeads'][1]==[256,192]
# Preserve original Issue7 fixtures and sealed independently authored verifier/results.
prior=ROOT/'re/campaigns/C001/versioned-spinner-qa';seal=json.loads((prior/'provenance.json').read_text())
assert hashlib.sha256((prior/'results.json').read_bytes()).hexdigest()==seal['resultsSha256']
for f,h in seal['verifierHashes'].items():assert hashlib.sha256((prior/f).read_bytes()).hexdigest()==h
for f,h in seal['fixtureHashes'].items():assert hashlib.sha256((ROOT/'re/campaigns/C001/versioned-spinners'/f).read_bytes()).hexdigest()==h
print('PASS 55 static sites / 13 selected bodies / 10 inherited identities / 81 fixture hashes; prior33/548 seals unchanged')
