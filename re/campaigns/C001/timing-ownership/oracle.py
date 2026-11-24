#!/usr/bin/env python3
"""Conditional before/after original-static predictions; never imports production.

Reuses Issue22 raw/hash-sealed charts. Original M0b7e comparator establishes time,
then base-before-inherited; equal-time/equal-flag permutations remain a domain,
not a claimed stable original List.Sort outcome.
"""
import hashlib,itertools,json,pathlib,struct,math,sys
H=pathlib.Path(__file__).resolve().parent;P=H.parent/'timing-state'
manifest=json.loads((P/'manifest.json').read_text())
names=['base','equal-base-inherited','equal-inherited-base','reverse-base-times',
 'reverse-inherited-times','base-index-resets-factor','future-does-not-stop-scan',
 'fraction-after-start','fraction-before-start','positive-fraction-after-zero',
 'negative-fraction-before-zero','multi-parent-registration','equal-two-bases','equal-two-inherited']
f32=lambda x:struct.unpack('<f',struct.pack('<f',x))[0]
def inv(beat):return f32(f32(max(10.,min(1000.,f32(-beat))))/100.) if beat<0 else 1.
def at(points,order,t):
 indices=[i for i in order if points[i]['time']<=t]
 return indices[-1] if indices else order[0]
def speed(points,order,t):
 bases=[k for k,i in enumerate(order) if points[i]['base'] and math.trunc(points[i]['time'])<=t]
 inherited=[k for k,i in enumerate(order) if not points[i]['base'] and math.trunc(points[i]['time'])<=t]
 b=bases[-1] if bases else 0;h=inherited[-1] if inherited else 0
 e=points[order[b]]['beat']*(inv(points[order[h]]['beat']) if h>b else 1.)
 return 100.*(1000./e) if e>0 else 100.
def prepare(points,order,start,spans,version):
 rate=speed(points,order,start);raw=at(points,order,start)
 q=min(400.,100. if version<8 else 100./inv(points[raw]['beat']))
 assert spans==1 # These14 reused controls have only single-traversal parents.
 end=math.trunc(start+400000./rate);dates=[];k=1
 while k*q<=400.:
  distance=k*q
  if 400.-distance<=.01*rate:break
  dates.append(math.trunc(start+(f32(distance)/rate)*1000.));k+=1
 dates.append(max(start+(end-start)//2,math.trunc(start+(f32(400.)/rate)*1000.)-36))
 return dict(prepared_rate=rate,prepared_end=end,dates=dates)
cases=[]
for name in names:
 c=next(c for c in manifest['cases'] if c['id']==name)
 assert hashlib.sha256((P/c['path']).read_bytes()).hexdigest()==c['sha256']
 points=c['points'];before=list(range(len(points)))
 groups=[]
 for key,g in itertools.groupby(sorted(before,key=lambda i:(points[i]['time'],not points[i]['base'])),key=lambda i:(points[i]['time'],points[i]['base'])):groups.append(list(g))
 orders=[sum((list(x) for x in choice),[]) for choice in itertools.product(*(list(itertools.permutations(g)) for g in groups))]
 parents=[]
 for e in c['expected']:
  queries=[e['start']+5,e['dates'][0]+2,e['end']+5]
  head=at(points,before,queries[0]);variants=[]
  for order in orders:
   prepared=prepare(points,order,e['start'],c['spans'],c['version'])
   original_queries=[e['start']+5,prepared['dates'][0]+2,prepared['prepared_end']+5]
   original_head=at(points,order,original_queries[0])
   rows=[]
   for kind,t in zip(['Head','Tick','Finish'],original_queries):
    i=at(points,order,t);bank,index,volume=points[i]['sample']
    if kind=='Head':bank=points[original_head]['sample'][0] # E039 snapshot occurs AFTER early loader sort.
    rows.append(dict(kind=kind,query=t,source_order=i,bank=bank,index=index,volume=volume,
      request_volume=volume if kind=='Tick' else int(max(8,volume)*.8)))
   variants.append(dict(order=order,**prepared,prepared_head_bank=points[original_head]['sample'][0],samples=rows))
  parents.append(dict(start=e['start'],prepared_rate=e['speed'],prepared_end=e['end'],prepared_head_bank=points[head]['sample'][0],queries=queries,
    before_samples=[dict(kind=kind,source_order=at(points,before,t),values=points[at(points,before,t)]['sample']) for kind,t in zip(['Head','Tick','Finish'],queries)],after_sort_variants=variants))
 cases.append(dict(id=name,path=c['path'],sha256=c['sha256'],format=c['version'],points=points,raw_timing_rows=c['rows'],
   source_order=before,after_order_domain=orders,equal_key_order_unresolved=len(orders)>1,parents=parents))
out=dict(schema='issue23-conditional-oracle-v1',classification='RECONSTRUCTED from CONFIRMED ORIGINAL static ownership/chronology; original runtime NOT RUN',
 parent_manifest_sha256=hashlib.sha256((P/'manifest.json').read_bytes()).hexdigest(),
 chronology='Audio binds map; M28f5->M28be(false,true,true) sorts before objects/preparation; M224d sorts again afterward. Ambiguous equal-key cases are conditional same-order variants only: early/late sorts may choose different domain members.',cases=cases)
target=pathlib.Path(sys.argv[1]) if len(sys.argv)>1 else H/'conditional-oracle.json'
target.write_text(json.dumps(out,indent=2)+'\n');print(f'PASS14 reused fixture hashes;12 exact sort orders and2 explicitly ambiguous equal-key order domains')
