#!/usr/bin/env python3
"""E045 research mathematics only; no original CLR or reconstruction execution.

Circle-only, no mods. Explicit prepared operands and a declared binary32/64
profile. Every excluded type arm is justified by ancestry in the evidence.
No generic neutral-factor substitution for an unresolved component is permitted.
"""
from dataclasses import dataclass,field
import math
from pathlib import Path
import re
import star_difficulty_projection as e44

f32=e44.single;div=e44.divide;pw=e44.power;clip=e44.clamp
def mx(a,b):return math.nan if math.isnan(a) or math.isnan(b) else max(a,b)
def mn(a,b):return math.nan if math.isnan(a) or math.isnan(b) else min(a,b)
def cube(x):return (x*x)*x
def square(x):return x*x
def root(x):return math.sqrt(x) if x>=0 else math.nan
def trig(fn,x):
    try:return fn(x)
    except ValueError:return math.nan
def deg(x):return (x*math.pi)/180.0
def linear(x,a,b):return clip(div(x-a,b-a),0.,1.)
def smooth(x,a,b):
    t=linear(x,a,b);return (t*t)*(3.-2.*t)
def smoother(x,a,b):
    t=linear(x,a,b);return ((t*t)*t)*(t*(6.*t-15.)+10.)
def wide(x):return smooth(x,deg(140),deg(40)) # M11c5
def narrow(x):return smooth(x,deg(40),deg(140)) # M11c4
def vsub(a,b):return (f32(a[0]-b[0]),f32(a[1]-b[1]))
def vlength(v):return f32(root(f32(f32(v[0]*v[0])+f32(v[1]*v[1]))))
def dist(a,b):return vlength(vsub(a,b))
def i4(x):
    if not math.isfinite(x) or not -2147483648<=x<2147483648:raise ValueError('UNSUPPORTED original conv.i4 boundary')
    return math.trunc(x)

@dataclass
class Pair:
    index:int
    time:float
    delta:float
    strain:float
    position:tuple
    previous_position:tuple
    radius:float
    preempt:float
    window:float
    gap:float
    jump:float
    angle:float|None=None
    axis:float|None=None
    history:list=field(default_factory=list,repr=False)
    def prev(self,k=0):
        n=self.index-(k+1)
        return self.history[n] if 0<=n<len(self.history) else None
    def following(self,k=0):
        n=self.index+k+1
        return self.history[n] if 0<=n<len(self.history) else None
    def snapshot(self):
        return {k:getattr(self,k) for k in ['index','time','delta','strain','position','previous_position','radius','preempt','window','gap','jump','angle','axis']}

def prepare(circles,radius,preempt,full_window):
    """circles is ORIGINAL registered order of (start,x,y); no presort."""
    if not all(math.isfinite(x) and x>0 for x in [radius,preempt,full_window]):raise ValueError('finite positive prepared operands required')
    circles=[(int(t),f32(x),f32(y)) for t,x,y in circles];pairs=[]
    for j in range(1,len(circles)):
        t,x,y=circles[j];last,lx,ly=circles[j-1];dt=float(e44.i32(t-last));st=mx(dt,25.)
        p=Pair(j-1,float(t),dt,st,(x,y),(lx,ly),f32(radius),float(preempt),float(full_window),st,0.,history=pairs)
        p.jump=float(f32(dist(p.position,p.previous_position)*f32(f32(50.)/f32(radius))))
        earlier=p.prev(1)
        if earlier is not None:
            a=vsub(earlier.position,p.previous_position);b=vsub(p.position,p.previous_position)
            dot=f32(f32(a[0]*b[0])+f32(a[1]*b[1]))
            cross=f32(f32(a[0]*b[1])-f32(a[1]*b[0]))
            p.angle=abs(math.atan2(float(cross),float(dot)))
            d=vsub(p.position,p.previous_position)
            p.axis=math.atan2(float(abs(d[1])),float(abs(d[0])))
        pairs.append(p)
    return pairs

def radius_scale(p):return mx(1.,1.+float(f32(f32(f32(30.)-p.radius)/f32(70.))))

def repeated_angle(p,previous):
    """M11c3, all nullable and six-predecessor gates retained."""
    if p.angle is None or previous.angle is None:return 1.
    total=0.;visited=[]
    for k in range(6):
        h=p.prev(k)
        if h is None or mx(p.strain,h.strain)>1.1*mn(p.strain,h.strain):break
        if h.axis is not None and p.axis is not None:
            diff=abs(p.axis-h.axis)
            total=total+math.cos(8.*mn(deg(11.25),diff))
        visited.append(h.index)
    factor=square(mn(div(.5,total),1.))
    spacing=smoother(p.jump,0.,100.)
    cosine=math.cos(2.*mn(deg(45.),abs(p.angle-previous.angle)*spacing))
    b=1.-(.15*wide(previous.angle))*cosine
    return square(b+(((1.-b)*factor)*.5)*spacing)

def movement(p):
    """M11c1(circle,true). T0339 arms excluded by actual circle ancestry."""
    if p.index<=1:return 0.
    prev=p.prev();older=p.prev(2);distance=p.jump;prior_distance=prev.jump
    v=div(distance,p.strain);pv=div(prior_distance,prev.strain)
    value=v*repeated_angle(p,prev)
    if p.angle is not None and prev.angle is not None:
        aa=p.angle;bb=prev.angle;minimum=mn(v,pv);small=0.
        if mx(p.strain,prev.strain)<1.25*mn(p.strain,prev.strain):
            small=wide(aa)
            small=small*(.08+.92*(1.-mn(small,cube(wide(bb)))))
            small=small*((minimum*smoother(60000./(p.strain*2),300.,400.))*smoother(distance,0.,200.))
        large=narrow(aa)
        large=large*(.25+.75*(1.-mn(large,cube(narrow(bb)))))
        cur=div(distance,pw(p.strain,1.45));old=div(prior_distance,pw(prev.strain,1.45))
        large=large*mn(cur,old)
        if older is not None:
            d=dist(older.position,prev.position)
            if d<f32(1.):large=large*(1.-.55*float(f32(f32(1.)-d)))
        value=value+mx(small*2.41,large*9.67)
        more=((((minimum*smoother(distance,50.,100.))*pw(linear(distance,300.,100.),1.8))*smoother(aa,deg(110.),deg(60.)))*smoother(prior_distance,50.,100.))
        more=(more*pw(linear(prior_distance,300.,100.),1.8))*smoother(bb,deg(110.),deg(60.))
        value=value+more*1.02
    if mx(pv,v)!=0.:
        contrast=smooth(div(abs(pv-v),mx(pv,v)),0.,1.)
        boost=mn(125./mn(p.strain,prev.strain),abs(pv-v))*contrast
        boost=boost*square(div(mn(p.strain,prev.strain),mx(p.strain,prev.strain)))
        value=value+boost*.9
    value=value*radius_scale(p)
    return value*div(1.,1.-pw(.03,pw(p.strain/1000.,.65)))

def overlap(a,b):
    """M408d: raw positions, Single distance, then Double radius expression."""
    d=float(dist(a.position,b.position));r=float(a.radius)
    return clip(1.-square(div(mx(d-r,0.),r)),0.,1.)

def velocity_angle(p):
    """M408c(circle,true), distinct from M11c1."""
    if p.index<=1:return 0.
    prev=p.prev();old=p.prev(1);distance=p.jump
    v=distance/p.strain;pv=prev.jump/prev.strain
    value=v*root(radius_scale(p))
    value=value*(1.+mn(.25,pw((mx(p.strain,prev.strain)-mn(p.strain,prev.strain))/50.,4.)))
    if p.angle is not None and prev.angle is not None:
        x=(math.sin(abs(p.angle-prev.angle)/2.)*180.)/(p.strain*.1)
        value=value*(.8+root(x/270.))
    repetition=1.
    if p.index>2:repetition=1.-((overlap(p,prev)*overlap(p,old))*overlap(prev,old))
    if p.angle is not None:value=value+(v*wide(p.angle))*repetition
    if mx(pv,v)!=0.:
        contrast=smooth(div(abs(pv-v),mx(pv,v)),0.,1.)
        boost=mn(125./mn(p.strain,prev.strain),abs(pv-v))
        value=value+((boost*contrast)*repetition)*.52
    value=pw(value,1.45)
    return value*smoother(distance,0.,50.)

def jump_term(p): # M29f7 ordinary circles, travel distance proven zero
    value=((mn(p.jump,120.)/120.)*1000.)/p.strain
    value=value*pw(radius_scale(p),1.5)
    return value*div(1.,1.-pw(.2,p.strain/1000.))

def composite(p):
    m=movement(p);j=jump_term(p);v=velocity_angle(p)
    x=m*70.9;y=j*2.35;z=v*242.
    norm=e44.pnorm(1.2,[x,y]);ratio=div(z,norm)
    if ratio==0.:blend=0.
    elif math.isnan(ratio):blend=1.
    else:
        try:blend=1./(1.+math.exp(-7.27*math.log(ratio)))
        except OverflowError:blend=0.
    value=((norm*blend)+(z*(1.-blend)))*1.12
    od=(79.5-p.window/2.)/6.
    value=value*(.985+square(mx(0.,od))/4000.)
    return {'movement':m,'jump':j,'velocity_angle':v,'composite':value}

@dataclass
class Pattern:
    interval:int=2147483647
    count:int=1
    repeats:int=1
    def append(self,dt):
        if self.interval==2147483647:self.interval=max(dt,25)
        self.count=e44.i32(self.count+1)
    def parity(self,other,tolerance):
        return self.count>1 and other.count>1 and abs(e44.i32(self.interval-other.interval))<tolerance and self.count%2==other.count%2
    def same(self,other,tolerance):
        return abs(e44.i32(self.interval-other.interval))<tolerance and self.count==other.count
    def snapshot(self):return [self.interval,self.count,self.repeats]

def fractional_ratio(r): # M1745 + M19db
    f=r-math.trunc(r);tri=clip((.5-abs(f-.5))*2.,0.,1.)
    wave=(tri*tri)*(3.-2.*tri)
    return 1.+26.*mn(.5,wave)

def rhythm(p,trace=None):
    """M1744 with T067f/T0339 arms proved false for ordinary circles."""
    total=0.;tolerance=p.window*.3;group=Pattern();lastgroup=Pattern();seen=[]
    lastvalue=0.;active=False;capacity=min(p.index,32);offset=0
    while offset<capacity-2 and p.time-p.prev(offset).time<5000.:offset+=1
    older=p.prev(offset);oldest=p.prev(offset+1)
    for k in range(offset,0,-1):
        h=p.prev(k-1)
        timeweight=(5000.-(p.time-h.time))/5000.
        historyweight=mn((capacity-k)/capacity,timeweight)
        dt=mx(h.delta,1e-7);prior=mx(older.delta,1e-7);difference=abs(prior-dt)
        if group.interval==2147483647:group=Pattern(i4(dt))
        ratio=mx(prior,dt)/mn(prior,dt)
        highratio=clip(2.-ratio/8.,0.,1.)
        contrast=clip(div(difference-tolerance,tolerance),0.,1.)
        value=(fractional_ratio(ratio)*contrast)*highratio
        if difference<tolerance:group.append(i4(dt))
        if active:
            if difference>tolerance:
                if group.parity(lastgroup,tolerance):value=value*.5
                if mx(oldest.delta,1e-7)>prior+tolerance and prior>dt+tolerance:value=value*.125
                if lastgroup.count==group.count:value=value*.5
                if prior>dt+tolerance:value=value*.65
                found=False
                for saved in seen:
                    if not saved.same(group,tolerance):continue
                    if lastgroup.same(group,tolerance):saved.repeats=e44.i32(saved.repeats+1)
                    exponent=2.75/(1.+math.exp(.24*(58.33-float(group.interval))))
                    value=value*mn(3./saved.repeats,pw(1./saved.repeats,exponent))
                    found=True;break
                if not found and group.count>0:seen.append(group)
                value=value*(1.-doubletap(older,h)*.75)
                if group.count>1:total=total+root(value*lastvalue)*historyweight
                else:total=total+.7*historyweight
                lastvalue=value
                if prior+tolerance<dt:active=False
                lastgroup=group;group=Pattern(i4(dt))
        elif prior>dt+tolerance:
            active=True;lastvalue=value;group=Pattern(i4(dt))
        if trace is not None:trace.append({'k':k,'pair':h.index,'dt':dt,'prior':prior,'difference':difference,
            'history_weight':historyweight,'group':group.snapshot(),'previous_group':lastgroup.snapshot(),
            'saved':[g.snapshot() for g in seen],'active':active,'value':value,'sum':total})
        oldest=older;older=h
    total=total*linear(float(group.count),22.,3.)
    return root(4.+total*.95)/2.

def doubletap(p,following): # M3690, no window substitution
    if following is None:return 0.
    dt=mx(1.,p.delta);difference=abs(mx(1.,following.delta)-dt)
    timing=div(dt,mx(dt,difference))
    window=pw(mn(1.,div(dt,p.window)),5.)
    spacing=square(linear(p.jump,100.,50.))
    return 1.-pw(timing,spacing*(1.-window))

def interval_raw(p):
    adjusted=p.strain/clip(p.strain/p.window/.93,.92,1.)
    bonus=0.
    if 60000./(adjusted*4)>200.:bonus=.75*square((60000./4./200.-adjusted)/40.)
    value=((1.+bonus)*1000.)/adjusted
    value=value*div(1.,1.-pw(.3,p.strain/1000.))
    return value*(1.-doubletap(p,p.following()))

def visible(p,at,hidden=False): # M368f; no-mod caller always false
    if at>p.time:return 0.
    begin=p.time-p.preempt;fade=400.*mn(1.,p.preempt/450.)
    first=clip(div(at-begin,fade),0.,1.)
    if not hidden:return first
    return mn(first,1.-clip(div(at-(begin+fade),p.preempt*.3),0.,1.))

def visibility_time_weight(dt):return clip(2.-dt/1500.,0.,1.)

def past_visible(p,trace=None): # M1e5c + M1e5b
    total=0.
    for k in range(p.index):
        h=p.prev(k)
        if p.time-h.time>3000. or h.time<p.time-p.preempt:break
        v=visible(p,h.time)*smoother(h.jump,15.,150.)
        v=v*visibility_time_weight(p.time-h.time);total=total+v
        if trace is not None:trace.append(['past',h.index,v])
    return total

def future_visible(p,trace=None): # M1e5d, stop on first failed temporal condition
    h=p.following();total=0.
    while h is not None:
        if h.time-p.time>3000. or p.time<h.time-h.preempt:break
        w=visibility_time_weight(h.time-p.time)
        value=visible(h,p.time)*w;total=total+value
        if trace is not None:trace.append(['future',h.index,value])
        h=h.following()
    return total

def visibility_angles(p,trace=None): # M1e5e
    total=0.;k=0;elapsed=0.;recent=p;previous=None;earlier=None
    while elapsed<2000.:
        h=p.prev(k)
        if h is None:break
        temporal=1.-linear(h.strain,200.,2000.)
        if h.angle is not None and p.angle is not None:
            difference=abs(p.angle-h.angle);angle=math.pi
            if recent.angle is not None and previous is not None and previous.angle is not None and earlier is not None and earlier.angle is not None:
                angle=abs(previous.angle-h.angle)+abs(earlier.angle-recent.angle)
                blend=1.
                blend=blend*linear((mn(h.angle,recent.angle)*180.)/math.pi,20.,5.)
                blend=blend*linear((mx(h.angle,recent.angle)*180.)/math.pi,60.,120.)
                angle=math.pi+(.1*angle-math.pi)*blend
            spacing=smoother(h.jump,0.,50.)
            contribution=math.cos(3.*mn(deg(30.),mn(difference,angle)*spacing))*temporal
            total=total+contribution
            if trace is not None:trace.append(['angle',h.index,contribution])
        elapsed=p.time-h.time;k+=1;earlier=previous;previous=recent;recent=h
    return clip(div(2.,total),.2,1.)

def visibility_raw(p,trace=None): # M1e57, no Hidden: M1e5a unreachable
    if p.index==0:return 0.
    velocity=mx(1.,div(p.jump,p.strain))
    forward=future_visible(p,trace);backward=past_visible(p,trace);angles=visibility_angles(p,trace)
    movement=visibility_movement(p.following(),velocity,angles,backward,forward)
    temporal=pw(((500.-p.preempt)+abs(p.preempt-500.))/2.,2.5)/140000.
    temporal=temporal*(angles*velocity)
    value=e44.pnorm(1.5,[temporal,0.,movement])
    return value*div(1.,1.-pw(.8,p.strain/1000.))

def visibility_movement(following,velocity,angles,backward,forward):
    # M1e58 first argument is the following pair retained on evaluation stack.
    f=root(forward)
    if following is not None:f=f*smoother(following.jump,15.,150.)
    value=(((pw(backward+f,1.7)*.4)*angles)*velocity)
    value=mx(0.,value-2.5)
    return pw(value,.45)*2.4

class Envelope:
    """T0845 state, feeding actual M19ec mutable-prefix aggregation."""
    def __init__(self):
        self.start=0.;self.next=0.;self.peak=0.;self.candidates=[]
        self.segments=[];self.retained=0.;self.finalized=False;self.events=[]
    def record(self,duration):
        segment=[self.peak,float(round(duration))] # M3f39 Round(Double), ties-even
        lo=0;hi=len(self.segments)-1;found=None
        while lo<=hi:
            mid=lo+((hi-lo)>>1)
            value=self.segments[mid][0]
            if value==segment[0]:found=mid;break
            if value>segment[0]:lo=mid+1
            else:hi=mid-1
        at=lo if found is None else found
        self.segments.insert(at,segment);self.retained=self.retained+duration
        self.events.append(['record',*segment,duration])
        while self.retained>(11./(1.-.9))*400.:
            dropped=self.segments.pop();self.retained=self.retained-dropped[1]
            self.events.append(['trim',*dropped])
    def process(self,p,oldstate,raw):
        if p.index==0:
            self.start=p.time;self.next=self.start+400.;self.peak=raw();return self.peak
        while p.time>self.next:
            self.record(self.next-self.start);self.start=self.next
            if self.candidates:
                value,time=self.candidates.pop(0);self.next=time+400.
                self.peak=oldstate*pw(.2,(self.start-p.prev().time)/1000.)
                self.peak=mx(self.peak,value)
            else:
                self.next=self.start+400.
                self.peak=oldstate*pw(.2,(self.start-p.prev().time)/1000.)
        value=raw()
        if value>self.peak:
            self.candidates.clear();self.record(p.time-self.start)
            self.start=p.time;self.next=self.start+400.;self.peak=value
        else:
            while self.candidates and self.candidates[-1][0]<value:self.candidates.pop()
            self.candidates.append([value,p.time])
        return value
    def finish(self):
        if not self.finalized:self.record(self.next-self.start);self.finalized=True
        data=[list(s) for s in self.segments if s[0]>0.];i=0;elapsed=0.;prefix=[]
        while i<len(data) and elapsed<4000.:
            value,duration=data[i];part=0.
            while part<duration:
                weight=math.log10(1.+9.*clip((elapsed+part)/4000.,0.,1.))
                amount=float(round(mn(20.,duration-part)))
                item=[value*(.727+(1.-.727)*weight),amount]
                data.append(item);prefix.append([i,elapsed,part,*item]);part=part+20.
            elapsed=elapsed+duration;i+=1
        processed=sorted(data[i:],key=lambda s:s[0],reverse=True)
        total=e44.duration_aggregate(processed)
        return total,{'segments':self.segments,'processed':processed,'prefix_steps':prefix,'consumed':i,'retained':self.retained}

def first_minute(values,pairs):
    positive=[v for v in values if v>0.];count=0
    if pairs:
        limit=pairs[0].time+60000.
        for p in pairs:
            if p.time>limit:break
            count+=1
    for i in range(min(len(positive),count)):
        w=math.log10(1.+9.*clip(float(i)/float(count),0.,1.))
        positive[i]=positive[i]*(0.+(1.-0.)*w)
    return positive,count

def calculate_prepared(circles,radius,preempt,full_window):
    pairs=prepare(circles,radius,preempt,full_window)
    order=sorted(pairs,key=lambda p:p.time) # Pair history already built.
    states=[0.,0.,0.];outputs=[[],[],[]];trace=[];envelope=Envelope()
    for p in order:
        r=composite(p);history=[];vis=[]
        def component0():
            d=pw(.2,p.strain/1000.);states[0]=states[0]*d
            states[0]=states[0]+r['composite']*(1.-d)
            return states[0]
        output0=envelope.process(p,states[0],component0)
        raw1=interval_raw(p);d=pw(.3,p.strain/1000.)
        states[1]=states[1]*d;states[1]=states[1]+((raw1*(1.-d))*1.16)
        mult=rhythm(p,history);output1=states[1]*mult
        raw2=visibility_raw(p,vis);d=pw(.8,p.delta/1000.)
        states[2]=states[2]*d;states[2]=states[2]+((raw2*(1.-d))*2.5)
        output2=states[2]
        for a,x in zip(outputs,[output0,output1,output2]):a.append(x)
        trace.append({'pair':p.snapshot(),**r,'interval_raw':raw1,'rhythm_multiplier':mult,'rhythm_walk':history,
            'visibility_raw':raw2,'visibility_walk':vis,'states':list(states),'outputs':[output0,output1,output2]})
    if not circles:q0=0.;env={'segments':[],'processed':[],'prefix_steps':[],'consumed':0,'retained':0.}
    else:q0,env=envelope.finish()
    q1,weights1=e44.ordered_aggregate(outputs[1],20.,.9)
    positive,count=first_minute(outputs[2],order);q2,weights2=e44.ordered_aggregate(positive,1.,.9)
    result=e44.final(q0,q1,q2)
    return {'pairs':[p.snapshot() for p in pairs],'trace':trace,'envelope':env,'component_outputs':outputs,
        'component2_transformed':positive,'first_minute_count':count,'weights1':weights1,'weights2':weights2,
        'aggregates':[q0,q1,q2],'final':result,'profile':'explicit binary32 vector storage / binary64 math; no original runtime'}

def manager_operands(cs,ar,od,viewport_width,viewport_height):
    if not all(math.isfinite(x) for x in [cs,ar,od,viewport_width,viewport_height]) or min(viewport_width,viewport_height)<=0:raise ValueError('explicit valid viewport required')
    if not all(0.<=x<=10. for x in [cs,ar,od]):raise ValueError('bounded difficulty scalars0..10 required')
    cs=f32(cs);ar=f32(ar);od=f32(od);width=f32(viewport_width);height=f32(viewport_height)
    scalar=(float(cs)-5.)/5.
    diameter=f32(float(f32(width/f32(8.)))*(1.-.699999988079071*scalar))
    radius=f32(f32(f32(diameter/f32(2.))/f32(height/f32(384.)))*f32(1.0004099607467651))
    def interpolate(d,low,middle,high):
        if d>5.:return middle+((high-middle)*(d-5.))/5.
        if d<5.:return middle-((middle-low)*(5.-d))/5.
        return middle
    preempt=i4(interpolate(float(ar),1800.,1200.,450.))
    window=2.*(math.floor(interpolate(float(od),80.,50.,20.))-.5)
    return {'radius':radius,'preempt':float(preempt),'full_window':window}

def load_bounded_osu(text,viewport_width,viewport_height,time_shift):
    """Research admission/projection, NOT execution/equivalence of original parser.

    Format14, explicit no-mod mode0 CS/AR/OD/StackLeniency0, simple circles,
    unique increasing nonnegative times, coords0..512. Explicit viewport/shift.
    Other original loader/provider/registration policies remain conditional.
    """
    lines=text.splitlines()
    if not lines or lines[0]!='osu file format v14':raise ValueError('bounded format14 required')
    section='';values={};circles=[]
    for line in lines[1:]:
        line=line.strip()
        if not line or line.startswith('//'):continue
        if line.startswith('[') and line.endswith(']'):section=line[1:-1];continue
        if section in ['General','Difficulty'] and ':' in line:
            k,v=line.split(':',1);values[k.strip()]=v.strip()
        elif section=='HitObjects':
            fields=line.split(',')
            if len(fields)!=5:raise ValueError('bounded five-column circle rows required')
            x,y,t,kind,sound=map(int,fields)
            if kind not in [1,5] or sound!=0 or not (0<=x<=512 and 0<=y<=512):raise ValueError('unsupported bounded circle metadata')
            if not 0<=t<=2147483647 or not -2147483648<=int(time_shift)<=2147483647 or not 0<=t+int(time_shift)<=2147483647:
                raise ValueError('unsupported source conversion/shift boundary')
            t=e44.i32(t+int(time_shift))
            if t<0 or circles and t<=circles[-1][0]:raise ValueError('strict registered time order required; no guessed equal-time insertion')
            circles.append((t,x,y))
    if values.get('Mode')!='0':raise ValueError('explicit ordinary mode0 required')
    # M28eb initializes stack counts to zero. With threshold preempt*0 and
    # strictly increasing circle start=end times, both stacking scans terminate
    # at each positive time gap before a position-dependent stack can be added.
    # This is a bounded proof of unchanged F252c, not a stacking implementation.
    if 'StackLeniency' not in values or float(values['StackLeniency'])!=0.:
        raise ValueError('explicit StackLeniency0 required; stacking is not projected')
    operands=manager_operands(float(values['CircleSize']),float(values['ApproachRate']),float(values['OverallDifficulty']),viewport_width,viewport_height)
    return calculate_prepared(circles,**operands)

if __name__=='__main__':
    import argparse,json
    cli=argparse.ArgumentParser(description='E045 RESEARCH ONLY strict no-mod circle mathematics, not an original loader or R17')
    cli.add_argument('chart');cli.add_argument('output')
    cli.add_argument('--width',type=float,required=True);cli.add_argument('--height',type=float,required=True)
    cli.add_argument('--time-shift',type=int,required=True)
    a=cli.parse_args()
    result=load_bounded_osu(Path(a.chart).read_text(encoding='utf-8-sig'),a.width,a.height,a.time_shift)
    Path(a.output).write_text(json.dumps(result,indent=2,allow_nan=False)+'\n',encoding='utf-8',newline='\n')
