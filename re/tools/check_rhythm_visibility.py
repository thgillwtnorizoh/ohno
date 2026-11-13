#!/usr/bin/env python3
"""Independent E046 byte auditor + expected/actual history and chart controls."""
import ast,hashlib,json,math,struct,sys,traceback
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
sys.path.insert(0,str(ROOT/'re/evidence/46'))
import independent_oracle as oracle
import finite_inputs
import rhythm_visibility_projection as observed
import check_circle_star_history as oldcheck

def sha(b):return hashlib.sha256(b).hexdigest()
def equal(a,b):
    if isinstance(a,bool) or isinstance(b,bool):return type(a)==type(b) and a==b
    if isinstance(a,(int,float)) and isinstance(b,(int,float)):
        return a==b or math.isclose(a,b,rel_tol=5e-12,abs_tol=5e-13)
    if isinstance(a,dict) and isinstance(b,dict):return a.keys()==b.keys() and all(equal(a[k],b[k]) for k in a)
    if isinstance(a,(list,tuple)) and isinstance(b,(list,tuple)):return len(a)==len(b) and all(equal(x,y) for x,y in zip(a,b))
    return a==b

def audit(d,archive):
    stats=oldcheck.byte_audit(d,archive);m=d['methods'];count=0
    for method,off,op,token in [
        (0x1746,2,'stfld',0x4000db0),(0x1746,9,'stfld',0x4000db1),
        (0x1748,2,'stfld',0x4000daf),(0x174c,2,'stfld',0x4000db1),
        (0x1744,0x308,'callvirt',0x600174f),(0x1744,0x313,'callvirt',0x600174f),
        (0x1744,0x31d,'callvirt',0x600174b),(0x1744,0x328,'callvirt',0x600174c),
        (0x1744,0x3ba,'callvirt',0xa000c4f),(0x1744,0x41e,'newobj',0x6001746),
        (0x1e57,0x28,'callvirt',0x6002cb6),(0x1e57,0x69,'call',0x6001e58)]:
        assert [off,op,hex(token)] in m[hex(0x6000000+method)]['token_sites'];count+=1
    for off,op,value in [(0x3b7,'ldloc.s',16),(0x3b9,'ldloc.1',None),
                         (0x419,'ldloc.1',None),(0x41a,'stloc.s',10),(0x423,'stloc.1',None)]:
        assert [off,op,value] in m['0x6001744']['argument_local_sites'];count+=1
    for off,op,target in [(0x30d,'brfalse.s',0x38e),(0x318,'brfalse.s',0x32d),
        (0x3ac,'brtrue.s',0x3bf),(0x3b5,'ble.s',0x3bf),(0x3e6,'ble.s',0x3fb),
        (0x414,'bge.un.s',0x419),(0x42b,'ble.un.s',0x46f)]:
        assert [off,op,[target]] in m['0x6001744']['branches'];count+=1
    for token in [0x1746,0x1747,0x1748,0x1749,0x174a,0x174b,0x174c,0x174d,0x174e,0x174f]:
        assert m[hex(0x6000000+token)]['owner']=='0x2000326'
    assert d['fields']['0x4000db1']['signature']=='0608'
    for name,h in d['e045_unchanged_authority'].items():assert sha((ROOT/name).read_bytes())==h,name
    for name,h in d['e046_sources'].items():assert sha((ROOT/name).read_bytes())==h,name
    tree=ast.parse((ROOT/'re/evidence/46/independent_oracle.py').read_text(encoding='utf-8'))
    imports=[n.module if isinstance(n,ast.ImportFrom) else x.name for n in ast.walk(tree)
        if isinstance(n,(ast.Import,ast.ImportFrom)) for x in (n.names if isinstance(n,ast.Import) else [None])]
    assert set(imports)<=set(['math','struct','collections']),imports
    assert not any(isinstance(n,ast.Call) and isinstance(n.func,ast.Name) and n.func.id in ['eval','exec','__import__'] for n in ast.walk(tree))
    stats['e046_literal_relationships']=count;stats['oracle_import_independence']=True
    return stats

def finite():
    rows=[]
    def add(name,inp,expected,actual,kind='INDEPENDENT FINITE MATHEMATICS'):
        row=dict(id=f'V{len(rows)+1:03}',name=name,input=inp,expected=expected,actual=actual,
            passed=equal(expected,actual),classification=kind,original_runtime='NOT RUN')
        rows.append(row)
        if not row['passed']:raise AssertionError(row)
    def integrate(label,circles,params,kind):
        expected=oracle.chart(circles,*params);actual=observed.evaluate(circles,*params)
        # Compare every supplied intermediate. Assertion coordinates retained on failure.
        failures=[]
        def compare(path,e,a):
            if not equal(e,a):failures.append(dict(path=path,expected=e,actual=a))
        compare('pairs',expected['pairs'],actual['pairs'])
        for i,(e,a,detail) in enumerate(zip(expected['trace'],actual['trace'],actual['e046_observed_details'])):
            compare(f'{i}/rhythm',e['rhythm_multiplier'],detail['rhythm'])
            for j,(step,got) in enumerate(zip(e['rhythm']['events'],detail['groups'])):
                compare(f'{i}/group{j}',{k:step[k] for k in got},got)
            compare(f'{i}/event-count',len(e['rhythm']['events']),len(detail['groups']))
            vis=e['visibility']
            for key in ['backward','forward','angle','next_jump','movement','raw']:compare(f'{i}/{key}',vis[key],detail[key])
            compare(f'{i}/past',vis['past'],[r[1:] for r in detail['past']])
            compare(f'{i}/future',vis['future'],[r[1:] for r in detail['future']])
            compare(f'{i}/angles',[[r[0],r[1]] for r in vis['angles']],[r[1:] for r in detail['angles']])
            compare(f'{i}/raw0',e['raw0'],a['composite']);compare(f'{i}/raw1',e['raw1'],a['interval_raw'])
            compare(f'{i}/state',e['states'],a['states']);compare(f'{i}/output',e['outputs'],a['outputs'])
        compare('aggregates',expected['aggregates'],actual['aggregates'])
        compare('segments',expected['processed_segments'],actual['envelope']['processed'])
        compare('component2-prefix',expected['component2_transformed'],actual['component2_transformed'])
        for key in expected['final']:compare('final/'+key,expected['final'][key],actual['final'][key])
        add(label,dict(circles=circles,params=params),[],failures,kind)
        return dict(id=label,expected=expected,actual=actual,kind=kind)
    integrated=[]
    # Closed-form expression independently anchors the heap oracle and observed evaluator.
    circles=finite_inputs.from_intervals([300]+[100,100,200]*6)
    q=oracle.geometry(circles,50,1200,99);e=2.75/(1+math.exp(.24*(58.33-100)))
    terms=[1/3]+[math.sqrt(.25*r**(-e))*w for r,w in [(2,.5),(3,2/3),(4,5/6)]]
    scalar=math.sqrt(4+.95*sum(terms))/2
    h,r=oracle.rhythm(q,len(q)-1);actual_pairs=observed.subject.prepare(circles,50,1200,99)
    a,events=observed.observed_rhythm(actual_pairs[-1])
    add('closed repeated-group scalar independent formula',circles,scalar,h)
    add('observed repeated-group scalar',circles,scalar,a)
    add('four completed transition contributions',None,sum(terms),r['unattenuated'])
    add('repeat counters change through4',None,[1,2,3,4],[max(g[2] for g in t['saved']) for t in r['events'] if 'saved-insert' in t['flags'] or 'repeat-increment' in t['flags']])
    insert=next(t for t in events if t['saved'])
    add('saved insertion retains same identity as previous-group alias',None,insert['previous_group'][3],insert['saved'][0][3],'ARTIFICIAL IDENTITY OBSERVATION OF REACHED PATH')
    copied,copy_graph=oracle.rhythm(q,len(q)-1,True)
    copied_insert=next(t for t in copy_graph['events'] if t['saved'])
    add('deep-copy graph fails identity equivalence',None,False,copied_insert['previous_group'][3]==copied_insert['saved'][0][3],'ENGINEERING MUTANT CONTROL')
    add('deep-copy scalar alone cannot establish reference identity',None,h,copied,'ENGINEERING MUTANT CONTROL')
    no_repetition=math.sqrt(4+.95*(1/3+.5*(.5+2/3+5/6)))/2
    add('repeat increment materially changes scalar',None,True,abs(a-no_repetition)>.01,'ENGINEERING MUTANT CONTROL')
    add('reached saved identity mutates after insertion',None,[1,2,3,4],
        [t['saved'][0][2] for t in events if t['saved'] and (not t['previous_group'][3]==t['group'][3]) and
         (len(t['saved'])>len(events[events.index(t)-1]['saved']) or t['saved'][0][2]!=(events[events.index(t)-1]['saved'][0][2] if events[events.index(t)-1]['saved'] else 0))],
        'ARTIFICIAL IDENTITY OBSERVATION OF REACHED PATH')
    # Independently closed visibility expressions with six nonnull predecessor angles.
    for pre,jump in [(1200.,200),(450.,400)]:
        square=[(0,0),(jump,0),(jump,jump),(0,jump)];circles=[(i*100,*square[i%4]) for i in range(13)]
        q=oracle.geometry(circles,50,pre,99);idx=8;value,v=oracle.visibility(q,idx)
        backward=8. if pre==1200 else 2.;forward=3. if pre==1200 else 1.875;angle=1/3;velocity=jump/100
        movement=max(0.,(backward+math.sqrt(forward))**1.7*.4*angle*velocity-2.5)**.45*2.4
        short=max(500-pre,0.)**2.5/140000*angle*velocity
        raw=(movement**1.5+short**1.5)**(1/1.5)/(1-.8**.1)
        a=observed.subject.prepare(circles,50,pre,99)[idx]
        add('six-angle closed visibility scalar '+str(pre),circles,angle,observed.subject.visibility_angles(a))
        add('forward and backward independent sums '+str(pre),None,[backward,forward],[v['backward'],v['forward']])
        add('complete independent visibility expression '+str(pre),circles,raw,value)
        add('observed complete visibility expression '+str(pre),circles,raw,observed.subject.visibility_raw(a))
    # Post-update elapsed semantics: the last included entry crosses the window.
    for dt,count in [(499,5),(500,4),(501,4),(600,4)]:
        square=[(0,0),(200,0),(200,200),(0,200)];circles=[(i*dt,*square[i%4]) for i in range(13)]
        q=observed.subject.prepare(circles,50,1200,99);trace=[];got=observed.subject.visibility_angles(q[8],trace)
        want=min(1.,max(.2,2/(count*(1-(dt-200)/1800))))
        add('independent2000 crossing angle scalar/'+str(dt),circles,want,got)
        add('independent2000 crossing entry count/'+str(dt),None,count,len(trace))
    # Artificial nullable-angle inputs isolate cap/smoothing arithmetic.
    q=observed.subject.prepare([(i*100,i*40,0) for i in range(10)],50,1200,99)
    for x in q[2:]:x.angle=math.pi
    q[7].angle=0.;tr=[];got=observed.subject.visibility_angles(q[8],tr)
    add('cosine cap complete six-angle sum',None,.4,got,'ARTIFICIAL ANGLE CONTROL')
    add('cosine cap contribution is cosPiOver2',None,math.cos(math.pi/2),tr[0][2],'ARTIFICIAL ANGLE CONTROL')
    q=observed.subject.prepare([(i*100,i*40,0) for i in range(7)],50,1200,99)
    for x in q[2:]:x.angle=0.;x.jump=25.
    q[5].angle=math.pi/6;tr=[];got=observed.subject.visibility_angles(q[5],tr)
    add('quintic spacing gate complete three-angle sum',None,2/(3*math.sqrt(.5)),got,'ARTIFICIAL ANGLE CONTROL')
    for last,count in [(999,5),(1000,4),(1001,4)]:
        ts=[i*1000 for i in range(11)]+[10000+last];circles=[(t,256,192) for t in ts]
        q=observed.subject.prepare(circles,50,1200,99);value,tr=observed.observed_rhythm(q[-1])
        add('independent5000 history equality/'+str(last),None,[1.,count],[value,len(tr)])
    for n,count in [(32,28),(33,29),(34,30),(35,30)]:
        q=observed.subject.prepare([(i,256,192) for i in range(n)],50,1200,99);value,tr=observed.observed_rhythm(q[-1])
        add('independent32 capacity seed/'+str(n),None,[1.,count],[value,len(tr)])
    for dt in [129,130,131]:
        circles=finite_inputs.from_intervals([300,300]+[100,dt,250]*5)
        integrated.append(integrate('artificial-exact-tolerance/'+str(dt),circles,(50.,1200.,100.),'ARTIFICIAL PREPARED WINDOW100'))
    # New raw fixtures and numerical outcomes, including both histories active together.
    for row in finite_inputs.cases():
        params=oracle.normalize(row['cs'],row['ar'],row['od'],*row['viewport'])
        integrated.append(integrate(row['id'],row['circles'],params,row['classification']))
        # The unchanged subject raw admission is also exercised using the fixture text.
        raw=observed.subject.load_bounded_osu(finite_inputs.osu(row),*row['viewport'],row['time_shift'])
        add('raw admission final/'+row['id'],row['id'],integrated[-1]['expected']['final'],{k:raw['final'][k] for k in integrated[-1]['expected']['final']},'BOUNDED RAW-CHART CONTROL')
    # Historical traces retain their old classification; only new comparisons close them.
    historical=json.loads((ROOT/'re/evidence/45/controls.json').read_text(encoding='utf-8'))
    for old in historical['traces']:
        integrated.append(integrate('E045/'+old['id'],old['input'],(50.,1200.,99.),'PREPARED INPUT / NEW INDEPENDENT REASSESSMENT'))
        add('unchanged historical scalar/'+old['id'],old['id'],old['actual']['final']['double'],integrated[-1]['actual']['final']['double'])
    return rows,integrated

def preservation():
    d=json.loads((ROOT/'re/evidence/46/preservation.json').read_text(encoding='utf-8'))
    for name,h in d['protected'].items():assert sha((ROOT/name).read_bytes())==h,name
    assert sha((ROOT/'re/contexts.md').read_bytes()[:d['journal_bytes']])==d['journal_sha256']
    return len(d['protected'])

if __name__=='__main__':
    packet=Path(sys.argv[1]);folder=ROOT/'re/evidence/46'
    try:
        d=json.loads(packet.read_text(encoding='utf-8'));stats=audit(d,sys.argv[2]);rows,charts=finite()
        stats.update(independent_controls=len(rows),integrated_charts=len(charts),preserved_files=preservation())
        out=dict(research_source=d['research_source'],packet_sha256=sha(packet.read_bytes()),stats=stats,cases=rows,charts=charts,
            sources={str(p.relative_to(ROOT)):sha(p.read_bytes()) for p in [Path(__file__).resolve(),ROOT/'re/tools/rhythm_visibility_projection.py',folder/'independent_oracle.py',folder/'finite_inputs.py']})
        Path(sys.argv[3]).write_text(json.dumps(out,indent=2,allow_nan=False)+'\n',encoding='utf-8',newline='\n')
        print('PASS E046',json.dumps(stats,sort_keys=True))
    except Exception:
        f=folder/'failures';f.mkdir(exist_ok=True);n=len(list(f.glob('failure-*.json')))+1
        (f/f'failure-{n:02}.json').write_text(json.dumps(dict(traceback=traceback.format_exc(),
            sources={str(p.relative_to(ROOT)):p.read_text(encoding='utf-8') for p in [Path(__file__).resolve(),ROOT/'re/tools/rhythm_visibility_projection.py',folder/'independent_oracle.py',folder/'finite_inputs.py']}),indent=2)+'\n',encoding='utf-8',newline='\n')
        raise
