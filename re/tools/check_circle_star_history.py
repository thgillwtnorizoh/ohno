#!/usr/bin/env python3
"""E045 independent byte relationships + separately derived finite math oracles.

No import of E045 collector. Never executes original CLR methods. Generic
projection traces without independent scalar expectations are reported separately.
"""
from collections import deque
from decimal import Decimal,localcontext
import hashlib,json,math,struct,sys,traceback
from pathlib import Path
import check_star_difficulty as e44check
import circle_star_history_projection as p

def sha(b):return hashlib.sha256(b).hexdigest()
def norm(v):return e44check.canon(v)
def equal(a,b):
    if isinstance(a,(int,float)) and isinstance(b,(int,float)):
        if math.isnan(a):return math.isnan(b)
        return a==b or math.isclose(a,b,rel_tol=5e-12,abs_tol=5e-13)
    if isinstance(a,dict) and isinstance(b,dict):return a.keys()==b.keys() and all(equal(a[k],b[k]) for k in a)
    if isinstance(a,(list,tuple)) and isinstance(b,(list,tuple)):return len(a)==len(b) and all(equal(x,y) for x,y in zip(a,b))
    return a==b

def byte_audit(d,archive):
    stats=e44check.audit(d,archive)
    methods=d['methods'];types=d['type_inventory'];count=0
    def site(m,off,op,tok):
        nonlocal count
        assert [off,op,hex(tok)] in methods[hex(0x6000000+m)]['token_sites'],(hex(m),hex(off));count+=1
    def branch(m,off,op,dest):
        nonlocal count
        assert [off,op,[dest]] in methods[hex(0x6000000+m)]['branches'];count+=1
    def const(m,off,op,v):
        nonlocal count
        assert [off,op,v] in methods[hex(0x6000000+m)]['constants'];count+=1
    def ancestry(n):
        result=[]
        while n:
            result.append(n);q=types[hex(0x2000000+n)]['base'];n=int(q,16)&0xffffff if q and int(q,16)>>24==2 else 0
        return result
    assert ancestry(0x94c)[:3]==[0x94c,0x35a,0x4c3]
    assert ancestry(0x2af)[:3]==[0x2af,0x339,0x4c3] and 0x339 not in ancestry(0x94c)
    for m,off,op,tok in [(0x11c1,6,'isinst',0x200067f),(0x11c1,0x78,'isinst',0x2000339),
        (0x11c1,0xc6,'call',0x60011c3),(0x408c,0x1c1,'call',0x600408d),
        (0x1744,0x1ea,'isinst',0x2000339),(0x1744,0x279,'callvirt',0x600174e),
        (0x1744,0x308,'callvirt',0x600174f),(0x1e57,0x28,'callvirt',0x6002cb6),
        (0x1e57,0x69,'call',0x6001e58),(0x1e58,0xd,'callvirt',0x6003675),
        (0x3f39,9,'call',0xa000398),(0x3f3c,0x10,'call',0xa000693),
        (0x28eb,0x3c,'ldfld',0x4001004),(0x28eb,0x57,'stfld',0x400252d),
        (0x28eb,0x324,'callvirt',0x60024b0),(0x24b0,2,'stfld',0x400252c)]:
        site(m,off,op,tok)
    for m,off,op,v in [(0x11c1,0x13,'ldc.i4.1',None),(0x11c3,0xed,'ldc.r8',6.),
        (0x1744,0x6a,'ldc.i4.s',32),(0x1744,0x9a,'ldc.r8',5000.),
        (0x1746,1,'ldc.i4.1',None),(0x1746,8,'ldc.i4.1',None),
        (0x1e63,0x4c,'ldc.r8',3000.),(0x1e5d,0x26,'ldc.r8',3000.),
        (0x1e5e,0x287,'ldc.r8',2000.),(0x19ec,0xdf,'ldc.r8',20.),
        (0x19ec,0x10f,'ldc.r8',4000.),(0x3260,0x1f,'ldc.r8',60000.),
        (0x1bc1,0x5b3,'ldc.i4',2064567436)]:const(m,off,op,v)
    for m,off,op,dest in [(0x11c1,0x14,'ble.s',0x29),(0x408c,0x14,'ble.s',0x29),
        (0x174e,0x28,'bge.un.s',0x3d),(0x174f,0x14,'bge.un.s',0x25),
        (0x1744,0xa3,'blt.s',0x78),(0x1e5e,0x290,'blt',0x26),
        (0x1e5d,0x2f,'bgt.s',0x81),(0x3f34,0xb2,'bgt',5),
        (0x3260,0x47,'ble.un.s',0x4b),
        (0x28eb,0xc6,'bgt.s',0x124),(0x28eb,0x1b0,'bgt',0x2e7)]:branch(m,off,op,dest)
    # Pinned generic call order: following-pair value remains below numeric args.
    seq=methods['0x6001e57']['argument_local_sites']
    assert not any(op.startswith('stloc') for off,op,*_ in seq if 0x2d<off<0x32)
    assert d['strings']['2064567436']['text']=='StackLeniency'
    assert len(d['reference_cases'])==27 and len(d['e044_boundary_closure'])==58
    stats['e045_literal_relationships']=count
    stats['circle_type_exclusion']=True
    return stats

def prefix_oracle(segments):
    """Independent queue formulation of M19ec, not projection's append/Skip loop."""
    pending=deque((v,float(round(dt))) for v,dt in segments if v>0.)
    elapsed=0.;consumed=0;appended=0
    while pending and elapsed<4000.:
        value,span=pending.popleft();count=math.ceil(span/20.) if span>0 else 0
        for n in range(count):
            t=elapsed+n*20.;fraction=min(1.,max(0.,t/4000.))
            scale=.727+.273*math.log10(1.+9.*fraction)
            pending.append((value*scale,float(round(min(20.,span-n*20.)))));appended+=1
        elapsed+=span;consumed+=1
    ordered=sorted(pending,reverse=True,key=lambda x:x[0]);t=0.;s=0.
    for value,duration in ordered:
        t2=t+duration/400.;s+=value*(math.pow(.9,t)-math.pow(.9,t2));t=t2
    return s/(1.-.9),ordered,consumed,appended

def scalar_final(q0,q1,q2):
    # Independent direct formula, no E044/E045 functions as expected oracle.
    with localcontext() as c:
        c.prec=65;D=lambda x:Decimal(str(x))
        a=4*(D(q0)**D('.63')*D('.02275'))**3 if q0 else D(0)
        b=4*(D(q1).sqrt()*D('.0675'))**3 if q1 else D(0)
        z=4*(D(q2).sqrt()*D('.0675'))**3 if q2 else D(0)
        joined=(a**D('1.1')+b**D('1.1')+z**D('1.1'))**(1/D('1.1'))
        return float((joined*D('1.12'))**(1/D(3)))

def finite():
    cases=[];explorations=[]
    def add(name,inp,want,got,sites,kind='EXECUTED MATHEMATICAL CONTROL'):
        r={'id':f'H{len(cases)+1:03}','name':name,'input':norm(inp),'expected':norm(want),'actual':norm(got),
           'pass':equal(want,got),'authority':sites,'classification':kind,'original_runtime':'NOT RUN'}
        cases.append(r)
        if not r['pass']:raise AssertionError(r)
    def circles(n,dt=1000,step=100):return [(i*dt,(i%5)*step,192) for i in range(n)]
    def pairs(n,dt=1000,step=100,r=50,pre=1200,window=99):return p.prepare(circles(n,dt,step),r,pre,window)
    sites=['M0810','M366d','M3691','M3694']
    for n in range(2,8):
        ps=p.prepare([(i*1000,i*40,192) for i in range(n)],50,1200,99)
        add('N objects produce N-1 pairs',n,n-1,len(ps),sites)
        add('nullable angle starts at pair2',n,([None,None]+[math.pi]*max(0,len(ps)-2))[:len(ps)], [x.angle for x in ps],sites)
    for dt in [24,25,26,99,100,101,1000]:
        ps=pairs(4,dt);x=ps[-1];t=max(dt,25.);v=100./t
        expected=v/(1.-math.pow(.03,math.pow(t/1000.,.65)))
        add('movement first reached nonzero pair constant spacing',dt,expected,p.movement(x),['M11c1:000d-0032,00c1-00cc,0504-051c'])
        add('distinct velocity-angle pair2 constant timing',dt,math.pow(v,1.45),p.velocity_angle(x),['M408c:0014,00bd-011a,02ca-02f5'])
        add('strain floor retains raw delta',dt,[float(dt),float(max(dt,25))],[x.delta,x.strain],sites)
    for n in [2,3]:
        ps=pairs(n)
        add('movement and contrast early returns',n,[[0.,0.] for _ in ps],[[p.movement(x),p.velocity_angle(x)] for x in ps],['M11c1:0014','M408c:0014'])
    for distance,expected in [(0,1.),(49,1.),(50,1.),(75,.75),(100,0.),(101,0.)]:
        ps=pairs(4);a,b=ps[-1],ps[-2];a.position=(float(distance),0.);b.position=(0.,0.)
        add('raw radius overlap ramp',[distance,50],expected,p.overlap(a,b),['M408d:0000-0065'],'ARTIFICIAL HELPER CONTROL')
    for history,expected_count in [(1,1),(2,2),(6,6),(9,6)]:
        ps=pairs(history+2);x=ps[-1]
        for q in ps:q.angle=math.pi/2;q.axis=0.
        b=.925;weight=(.5/expected_count)**2
        expected=(b+(1.-b)*weight*.5)**2
        add('six-predecessor repeated angle weighting',history,expected,p.repeated_angle(x,x.prev()),['M11c3:0034-011a,018c-01d1'],'ARTIFICIAL HELPER CONTROL')
    ps=pairs(5);x=ps[-1]
    expected=(.1+.25*(100./math.pow(1000.,1.45))*9.67)/.97
    add('movement full straight angle branch',x.snapshot(),expected,p.movement(x),['M11c1:00cd-03fc','M11c3'])
    turn=[(0,0,0),(1000,100,0),(2000,100,100),(3000,0,100),(4000,0,0)]
    ps=p.prepare(turn,50,1200,99);x=ps[-1]
    s=.4**3*(.4*(6*.4-15)+10)
    expected=(.1*.9625**2 + .453125*(100/1000**1.45)*9.67 + (.1*s*s)*1.02)/.97
    add('movement two right angles independently expanded',turn,expected,p.movement(x),['M11c1:00cd-03fc','M11c3'])
    add('distinct velocity full right angle branch',turn,.13**1.45,p.velocity_angle(x),['M408c:011b-021c,02ca-02f5'])
    for dt in [24,25,26,99,100,101,1000]:
        ps=pairs(40,dt);tr=[]
        add('equal intervals rhythm multiplier',dt,1.,p.rhythm(ps[-1],tr),['M1744:023f-046f,0485-04cd'])
    ts=[0]
    for dt in [200,100,200,100,200,100]:ts.append(ts[-1]+dt)
    ps=p.prepare([(t,200*(i%2),192) for i,t in enumerate(ts)],50,1200,99);tr=[];v=p.rhythm(ps[-1],tr)
    add('alternating intervals one completed group',ts,math.sqrt(4+.56*.95)/2,v,['M1744:03df-040a,0485-04cd'])
    add('alternating group intermediate sum',ts,.56,tr[-1]['sum'],['M1744:03fb-040a'])
    ps=pairs(40,1);tr=[];p.rhythm(ps[-1],tr)
    add('capacity32 retains30 processed comparisons',40,30,len(tr),['M1744:0064-00c9'])
    for last,expected in [(999,5),(1000,4),(1001,4)]:
        ts=[i*1000 for i in range(11)]+[10000+last];ps=p.prepare([(t,i%2*200,0) for i,t in enumerate(ts)],50,1200,99);tr=[];p.rhythm(ps[-1],tr)
        add('5000ms backward equality boundary',last,expected,len(tr),['M1744:007e-00a5'])
    for a,b,tol,want in [(100,109,10,True),(100,110,10,False),(100,111,10,False)]:
        g=p.Pattern(a,2);h=p.Pattern(b,2)
        add('pattern equality strict tolerance',[a,b,tol],want,g.same(h,tol),['M174f'],'ARTIFICIAL HELPER CONTROL')
        add('pattern parity strict tolerance',[a,b,tol],want,g.parity(h,tol),['M174e'],'ARTIFICIAL HELPER CONTROL')
    g=p.Pattern();add('pattern constructor counters',None,[2147483647,1,1],g.snapshot(),['M1746'])
    g.append(24);add('sentinel append clamps interval only once',24,[25,2,1],g.snapshot(),['M174d'])
    g.append(100);add('subsequent append retains interval',100,[25,3,1],g.snapshot(),['M174d'])
    for ratio,want in [(1,1.),(1.5,14.),(2,1.),(2.25,14.)]:
        add('fractional interval ratio triangular smooth',ratio,want,p.fractional_ratio(ratio),['M1745','M19db'])
    ps=pairs(6);x=ps[-1];x.time=10000.;x.preempt=1200.
    for at,want in [(8799,0.),(8800,0.),(9000,.5),(9200,1.),(10000,1.),(10001,0.)]:
        add('visibility interval endpoints',at,want,p.visible(x,at),['M368f:0000-0055,00c8-00e4'])
    for dt,want in [(1499,1.),(1500,1.),(2250,.5),(3000,0.),(3001,0.)]:
        add('visibility history time attenuation',dt,want,p.visibility_time_weight(dt),['M1e5f'])
    for dt,want in [(100,20),(1001,2)]:
        ps=pairs(35,dt)
        for q in ps:q.angle=math.pi;q.strain=100.
        tr=[];p.visibility_angles(ps[-1],tr)
        add('2000ms angle walk processes first outside entry',dt,want,len(tr),['M1e5e:0267-0290'],'ARTIFICIAL HELPER CONTROL')
    ps=pairs(5);n=ps[-1];n.jump=150.
    for following,label,total in [(n,'following large spacing',7.),(None,'nullable following bypass',7.)]:
        expected=max(0.,total**1.7*.4*2.-2.5)**.45*2.4
        add(label,None,expected,p.visibility_movement(following,2.,1.,4.,9.),['M1e57:0026-0069','M1e58'],'ARTIFICIAL HELPER CONTROL')
    n.jump=0.;expected=max(0.,4.**1.7*.4*2.-2.5)**.45*2.4
    add('following spacing zero suppresses sqrt-forward term',0,expected,p.visibility_movement(n,2.,1.,4.,9.),['M1e58:0000-007f'],'ARTIFICIAL HELPER CONTROL')
    vals=[4.,1.,9.];ps=pairs(4)
    add('first-minute transformation precedes descending sort',vals,[0.,math.log10(4.),9*math.log10(7.)],p.first_minute(vals,ps)[0],['M325f','M1d08'])
    ps[0].time=0.;ps[1].time=60000.;ps[2].time=60001.
    add('first-minute equality included then future stops',None,2,p.first_minute(vals,ps)[1],['M3260'])
    for dt,want in [(.5,0.),(1.5,2.),(2.5,2.),(3.5,4.)]:
        e=p.Envelope();e.peak=1.;e.record(dt)
        add('duration constructor ties-even rounding',dt,want,e.segments[0][1],['M3f39'])
    for source in [[[1.,400.]],[[3.,400.],[2.,800.],[1.,200.]],[[2.,2000.],[1.,2000.]],[[2.,5000.]]]:
        expected,ordered,consumed,appended=prefix_oracle(source)
        e=p.Envelope();e.finalized=True;e.segments=source;actual,details=e.finish()
        add('mutable duration prefix independently queued',source,expected,actual,['M19ec','M19e9'])
        add('duration prefix consumed/grown list',source,[consumed,appended,len(ordered)],[details['consumed'],len(details['prefix_steps']),len(details['processed'])],['M19ec'])
    for dt in [24,25,26,99,100,101,1000]:
        ts=[(0,0,192),(dt,100,192)];result=p.calculate_prepared(ts,50,1200,99)
        t=float(max(25,dt));adjust=t/min(1.,max(.92,t/99/.93));bonus=.75*((75-adjust)/40)**2 if adjust<75 else 0.
        q=1.16*(1+bonus)*1000/adjust
        add('two-circle complete calculated scalar',dt,scalar_final(0.,q,0.),result['final']['double'],['M0d6c','M3539','M080a'])
    for n in [0,1,2,3]:
        result=p.calculate_prepared(circles(n),50,1200,99)
        q=[0.,0.,1.16,1.508+1.16*11/12][n]
        add('small complete chart scalar',n,scalar_final(0.,q,0.),result['final']['double'],['M0810','M11c1','M408c','M1e57','M080a'])
    for step in [0,1,25,50,100,170]:
        source=[(i*1000,i*step,192) for i in range(4)];x=p.prepare(source,50,1200,99)[-1]
        v=step/1000.;t=min(1.,step/50.);gate=t**3*(6*t*t-15*t+10)
        add('spacing movement no-angle branch',step,v/.97,p.movement(x),['M11c1:00c1-00dc,0504-051c'])
        add('spacing velocity exponent and distance gate',step,v**1.45*gate,p.velocity_angle(x),['M408c:02ca-02f5'])
    q1=1.6124+1.508*(11/12)+1.16*((1+20/3)/(2**.9+1+20/3))
    coincident=[(i*1000,256,192) for i in range(4)];result=p.calculate_prepared(coincident,50,1200,99)
    add('four coincident circles have positive temporal stars',coincident,scalar_final(0.,q1,0.),result['final']['double'],['M0d6c','M3539','M1d08','M080a'])
    def four_oracle(jump):
        movement=(jump/1000.)/.97
        auxiliary=(min(jump,120.)/120.)/.8
        contrast=(jump/1000.)**1.45
        a=movement*70.9;b=auxiliary*2.35;z=contrast*242.
        combined=(a**1.2+b**1.2)**(1/1.2)
        w=1/(1+math.exp(-7.27*math.log(z/combined)))
        raw=(combined*w+z*(1-w))*1.12*(.985+25/4000.)
        q0=prefix_oracle([[raw*.8,400.]])[0]
        return q0,q1,scalar_final(q0,q1,0.)
    source=[(i*1000,i*100,192) for i in range(4)]
    expected0,expected1,expectedstar=four_oracle(100.)
    actual=p.calculate_prepared(source,50,1200,99)
    add('four-circle independently closed aggregate operands',source,[expected0,expected1,0.],actual['aggregates'],['M11c1','M408c','M19ea','M19ec','M1d08'])
    add('four-circle independently closed final scalar',source,expectedstar,actual['final']['double'],['M080a'])
    # Realistic bounded metadata profile, with viewport supplied explicitly.
    sf=lambda x:struct.unpack('<f',struct.pack('<f',x))[0]
    radius=sf(sf(sf(64./2.)/sf(384./384.))*sf(1.0004099607467651))
    add('manager no-mod CS5 AR5 OD5 prepared operands',[512,384],{'radius':radius,'preempt':1200.,'full_window':99.},p.manager_operands(5,5,5,512,384),['M28be','M28b2','M28b3','M0810'])
    for count in [2,3,4]:
        text='osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Difficulty]\nCircleSize:5\nApproachRate:5\nOverallDifficulty:5\n[HitObjects]\n'+''.join(f'{i*100},192,{i*1000},1,0\n' for i in range(count))
        result=p.load_bounded_osu(text,512,384,0)
        if count<4:want=scalar_final(0.,[0.,0.,1.16,1.508+1.16*11/12][count],0.)
        else:want=four_oracle(float(sf(sf(100.)*sf(sf(50.)/radius))))[2]
        add('bounded osu-to-star independent finite scalar',{'text':text,'viewport':[512,384],'time_shift':0},want,result['final']['double'],['M28be','M0810','M11c1','M408c','M1d08','M080a'])
        bits=struct.pack('<f',want).hex()
        add('calculated result caches Single bits',count,bits,result['final']['single_bits_le'],['M1bd0:002b'])
        mag=abs(struct.unpack('<f',bytes.fromhex(bits))[0]);display=math.floor(mag*100)/100
        add('calculated result displayed after cache rounding',count,display,result['final']['display'],['M1bc7','M1bd5','M2d7f'])
    for shift in [0,1000,123456]:
        text='osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Difficulty]\nCircleSize:5\nApproachRate:5\nOverallDifficulty:5\n[HitObjects]\n0,192,0,1,0\n100,192,1000,1,0\n200,192,2000,1,0\n'
        result=p.load_bounded_osu(text,512,384,shift)
        add('explicit constant chart shift preserves finite scalar',shift,scalar_final(0.,1.508+1.16*11/12,0.),result['final']['double'],['M2cb4','M3691','M19e9'])
    # Raw-chart admission excludes unresolved stack displacement instead of
    # silently treating all F252c positions as original raw F152e coordinates.
    source='osu file format v14\n[General]\nMode:0\nStackLeniency:0\n[Difficulty]\nCircleSize:5\nApproachRate:5\nOverallDifficulty:5\n[HitObjects]\n256,192,0,1,0\n256,192,1000,1,0\n'
    for label,text in [('missing stack input',source.replace('StackLeniency:0\n','')),
        ('positive stack input',source.replace('StackLeniency:0','StackLeniency:0.7')),
        ('equal time stack ambiguity',source.replace('192,1000','192,0')),
        ('unsupported source order',source.replace('192,0,1','192,1001,1')),
        ('unsupported slider',source.replace('1000,1,0','1000,2,0')),
        ('unsupported modifier domain mode',source.replace('Mode:0','Mode:1')),
        ('nonfinite stack input',source.replace('StackLeniency:0','StackLeniency:NaN'))]:
        actual='accepted'
        try:p.load_bounded_osu(text,512,384,0)
        except ValueError:actual='rejected'
        add('bounded reader rejects '+label,text,'rejected',actual,['M28eb:003c-0042,00c6,01b0,0324','M24b0:0002'],'ENGINEERING ADMISSION CONTROL')
    add('zero stack threshold keeps coincident circle coordinates',source,scalar_final(0.,1.16,0.),p.load_bounded_osu(source,512,384,0)['final']['double'],['M28eb','M24b0','M0d6c','M080a'])
    # Strict 400-unit crossing is different from equality; observe stored state.
    for elapsed,want in [(399,0),(400,0),(401,1)]:
        ps=p.prepare([(0,0,0),(1000,0,0),(1000+elapsed,0,0)],50,1200,99)
        e=p.Envelope();e.process(ps[0],0.,lambda:1.)
        e.process(ps[1],1.,lambda:0.)
        add('duration envelope strict400 boundary',elapsed,want,len(e.segments),['M3f34:00b2'])
    for elapsed,want in [(59999,2),(60000,2),(60001,1)]:
        ps=p.prepare([(0,0,0),(1000,0,0),(1000+elapsed,0,0)],50,1200,99)
        add('first-minute strict future boundary',elapsed,want,p.first_minute([1.,1.],ps)[1],['M3260:001f,0047'])
    for elapsed,want in [(2999,1),(3000,1),(3001,0)]:
        ps=p.prepare([(0,0,0),(1000,100,0),(1000+elapsed,200,0)],50,5000,99)
        a=[];b=[];p.future_visible(ps[0],a);p.past_visible(ps[1],b)
        add('3000ms history comparison includes equality',elapsed,[want,want],[len(a),len(b)],['M1e63:004c','M1e5d:0026,002f'],'ARTIFICIAL PREEMPT CONTROL')
    for source,delta in [([(0,0,0),(0,100,0)],[0.]),
        ([(100,0,0),(0,100,0),(50,200,0)],[-100.,50.]),
        ([(-2147483648,0,0),(2147483647,100,0)],[-1.])]:
        ps=p.prepare(source,50,1200,99)
        add('registered order preserved before any pair sort',source,delta,[x.delta for x in ps],['M0810','M2cb4:001d-002d'],'ARTIFICIAL PAIR INPUT')
        add('strain floor alone clamps signed differences',source,[max(x,25.) for x in delta],[x.strain for x in ps],['M366d:0016-002a'],'ARTIFICIAL PAIR INPUT')
    for radius in [0.,math.nan,math.inf]:
        actual='accepted'
        try:p.prepare([(0,0,0),(1000,100,0)],radius,1200,99)
        except ValueError:actual='rejected'
        add('invalid prepared radius explicit outcome',radius,'rejected',actual,['M0810 required prepared radius'],'ENGINEERING ADMISSION CONTROL')
    for span,want in [(43999.,1),(44000.,1),(44001.,0)]:
        e=p.Envelope();e.peak=1.;e.record(span)
        add('retained duration trims whole segment strictly above threshold',span,want,len(e.segments),['M3f35'])
    e=p.Envelope();e.peak=2.;e.record(22000.);e.peak=1.;e.record(22001.)
    add('trimming drops lower segment retaining prior mutations',None,[[2.,22000.]],e.segments,['M3f35'])
    add('equal positive object values retain successive weights',[2.,2.],2.+2.*11/12,p.e44.ordered_aggregate([2.,2.],20.,.9)[0],['M1d08'])
    add('nonpositive and NaN entries excluded from positive object aggregation',[0.,-1.,math.nan,2.],2.,p.e44.ordered_aggregate([0.,-1.,math.nan,2.],20.,.9)[0],['M1d08','M1d0d'])
    # Larger matrices execute the whole translated pipeline but are NOT golden scalars.
    for tag,ds,coords in [
        ('regular',[100]*40,[(100*(i%4),192) for i in range(41)]),
        ('alternating',[100,200]*20,[(200*(i%2),100) for i in range(41)]),
        ('perturbed',[99,100,101]*12,[(i%5*70,(i%3)*90) for i in range(37)]),
        ('long-pauses',[100,5000,100,5001,400,401]*5,[(i%4*100,192) for i in range(31)]),
        ('same-position',[25]*35,[(256,192)]*36),
        ('right-angles',[1000]*20,[(100*(i%2),100*((i//2)%2)) for i in range(21)])]:
        ts=[0]
        for dt in ds:ts.append(ts[-1]+dt)
        source=[(t,*xy) for t,xy in zip(ts,coords)];r=p.calculate_prepared(source,50,1200,99)
        explorations.append({'id':tag,'input':source,'actual':norm(r),'classification':'EXECUTED PROJECTION TRACE — no independent golden chart scalar','original_runtime':'NOT RUN'})
    return cases,explorations

def preservation(root):
    d=json.loads((root/'re/evidence/45/preservation.json').read_text(encoding='utf-8'))
    for name,h in d['protected'].items():assert sha((root/name).read_bytes())==h,name
    assert sha((root/'re/contexts.md').read_bytes()[:d['journal_bytes']])==d['journal_sha256']
    return len(d['protected'])

if __name__=='__main__':
    root=Path(__file__).resolve().parents[2];packet=Path(sys.argv[1]);d=json.loads(packet.read_text(encoding='utf-8'))
    try:
        stats=byte_audit(d,sys.argv[2]);cases,traces=finite();stats.update(finite_controls=len(cases),non_golden_traces=len(traces),preserved_files=preservation(root))
        out={'source':d['research_source'],'packet_sha256':sha(packet.read_bytes()),'stats':stats,'cases':cases,'traces':traces,
            'source_sha256':{n:sha((root/'re/tools'/n).read_bytes()) for n in ['check_circle_star_history.py','circle_star_history_projection.py']}}
        Path(sys.argv[3]).write_text(json.dumps(norm(out),indent=2,allow_nan=False)+'\n',encoding='utf-8',newline='\n')
        print('PASS E045',json.dumps(stats,sort_keys=True))
    except Exception:
        folder=root/'re/evidence/45/failures';folder.mkdir(exist_ok=True)
        idx=len(list(folder.glob('failure-*.json')))+1
        original={n:(root/'re/tools'/n).read_text(encoding='utf-8') for n in ['check_circle_star_history.py','circle_star_history_projection.py']}
        record={'traceback':traceback.format_exc(),'source':original,'packet_sha256':sha(packet.read_bytes())}
        (folder/f'failure-{idx:02}.json').write_text(json.dumps(record,indent=2)+'\n',encoding='utf-8',newline='\n')
        raise
