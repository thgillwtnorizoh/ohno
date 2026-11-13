"""E046 independent equation/state-graph oracle, never imports E044/E045 models.

Circle-only prepared operands. Single vector stores are explicit. A group heap
with integer identities replaces the projection's mutable Python object graph.
History geometry uses indexed equations. Duration accumulation is a separate
offline sweep over already calculated state values, then a dequeue transform.
Original instructions are the authority; no original CLR methods are invoked.
"""
import math,struct
from collections import deque

def s(x):return struct.unpack('<f',struct.pack('<f',x))[0]
def i32(x):return (int(x)+2**31)%2**32-2**31
def limit(x):return min(1.,max(0.,x))
def ramp(x,a,b):return limit((x-a)/(b-a))
def cubic(x,a,b):
    z=ramp(x,a,b);return z*z*(3-2*z)
def quintic(x,a,b):
    z=ramp(x,a,b);return ((z*z)*z)*(z*(6*z-15)+10)
def length(a,b):
    x=s(a[0]-b[0]);y=s(a[1]-b[1]);return s(math.sqrt(s(s(x*x)+s(y*y))))
def normalize(cs,ar,od,w,h):
    cs,ar,od,w,h=map(s,[cs,ar,od,w,h])
    diameter=s(s(w/s(8))*(1-.699999988079071*((cs-5)/5)))
    radius=s(s(s(diameter/s(2))/s(h/s(384)))*s(1.0004099607467651))
    def interpolate(x,a,b,c):return b+(c-b)*(x-5)/5 if x>5 else b-(b-a)*(5-x)/5
    return radius,float(math.trunc(interpolate(ar,1800,1200,450))),2*(math.floor(interpolate(od,80,50,20))-.5)

def geometry(circles,radius,preempt,window):
    result=[]
    for j in range(1,len(circles)):
        t,x,y=circles[j];ot,ox,oy=circles[j-1];pos=(s(x),s(y));old=(s(ox),s(oy))
        delta=float(i32(t-ot));tau=max(25.,delta);angle=None;axis=None
        if j>=3:
            a=circles[j-2];ax=s(s(a[1])-old[0]);ay=s(s(a[2])-old[1])
            bx=s(pos[0]-old[0]);by=s(pos[1]-old[1])
            cross=s(s(ax*by)-s(ay*bx));dot=s(s(ax*bx)+s(ay*by))
            angle=abs(math.atan2(cross,dot));axis=math.atan2(abs(by),abs(bx))
        result.append(dict(index=j-1,time=float(t),delta=delta,strain=tau,position=pos,
            previous_position=old,radius=s(radius),preempt=float(preempt),window=float(window),
            gap=tau,jump=float(s(length(pos,old)*s(s(50)/s(radius)))),angle=angle,axis=axis))
    return result

def tap(p,n):
    if n is None:return 0.
    a=max(1.,p['delta']);b=abs(max(1.,n['delta'])-a)
    return 1-(a/max(a,b))**(ramp(p['jump'],100.,50.)**2*(1-min(1.,a/p['window'])**5))

def rhythm(q,index,copy_saved=False):
    """Explicit identity heap and immutable event snapshots, M1744/1746-174f."""
    heap={};events=[];alloc=0
    def new(interval=2147483647):
        nonlocal alloc
        alloc+=1;heap[alloc]=[interval,1,1];return alloc
    def record(g):return list(heap[g])+[g]
    def same(a,b,tol,parity=False):
        x,y=heap[a],heap[b]
        ok=abs(i32(x[0]-y[0]))<tol
        return ok and (x[1]>1 and y[1]>1 and x[1]%2==y[1]%2 if parity else x[1]==y[1])
    current=new();last=new();saved=[];active=False;previous_value=0.;total=0.
    p=q[index];tol=p['window']*.3;capacity=min(index,32);back=0
    while back<capacity-2 and p['time']-q[index-1-back]['time']<5000:back+=1
    seed=back
    for k in range(seed,0,-1):
        j=index-k;now=q[j];old=q[j-1];oldold=q[j-2] if j>=2 else None
        dt=max(1e-7,now['delta']);prior=max(1e-7,old['delta']);difference=abs(prior-dt)
        weight=min((capacity-k)/capacity,1-(p['time']-now['time'])/5000)
        if heap[current][0]==2147483647:current=new(math.trunc(dt))
        ratio=max(prior,dt)/min(prior,dt);fraction=ratio-math.trunc(ratio)
        triangular=limit((.5-abs(fraction-.5))*2);shape=(triangular*triangular)*(3-2*triangular)
        value=(1+26*min(.5,shape))*limit((difference-tol)/tol)*limit(2-ratio/8)
        flags=[];before_saved=[record(g) for g in saved]
        if difference<tol:
            if heap[current][0]==2147483647:heap[current][0]=max(math.trunc(dt),25)
            heap[current][1]=i32(heap[current][1]+1);flags.append('append')
        if active and difference>tol:
            if same(current,last,tol,True):value*=.5;flags.append('parity')
            if max(oldold['delta'],1e-7)>prior+tol and prior>dt+tol:value*=.125;flags.append('descending')
            if heap[last][1]==heap[current][1]:value*=.5;flags.append('equal-count')
            if prior>dt+tol:value*=.65;flags.append('accelerating')
            match=next((g for g in saved if same(g,current,tol)),None)
            if match is not None:
                flags.append('saved-match')
                if same(last,current,tol):
                    heap[match][2]=i32(heap[match][2]+1);flags.append('repeat-increment')
                repeats=heap[match][2]
                exponent=2.75/(1+math.exp(.24*(58.33-heap[current][0])))
                value*=min(3/repeats,(1/repeats)**exponent)
            elif heap[current][1]>0:
                if copy_saved:
                    alias=new();heap[alias]=list(heap[current]);saved.append(alias)
                else:saved.append(current)
                flags.append('saved-insert')
            value*=1-tap(old,now)*.75
            total+=(math.sqrt(value*previous_value) if heap[current][1]>1 else .7)*weight
            previous_value=value
            if prior+tol<dt:active=False
            last=current;current=new(math.trunc(dt))
        elif not active and prior>dt+tol:
            active=True;previous_value=value;current=new(math.trunc(dt));flags.append('activate')
        events.append(dict(k=k,pair=j,dt=dt,prior=prior,difference=difference,history_weight=weight,
            group=record(current),previous_group=record(last),saved=[record(g) for g in saved],
            active=active,value=value,sum=total,flags=flags,before_saved=before_saved))
    attenuation=ramp(float(heap[current][1]),22.,3.)
    return math.sqrt(4+total*attenuation*.95)/2,dict(seed=seed,capacity=capacity,tolerance=tol,
        heap=heap,events=events,unattenuated=total,attenuation=attenuation,current=current,last=last,saved=saved)

def fade(p,at):
    if at>p['time']:return 0.
    return limit((at-(p['time']-p['preempt']))/(400*min(1.,p['preempt']/450)))

def visibility(q,i):
    p=q[i];past=[];future=[];angular=[];angular_total=0.
    for j in range(i-1,-1,-1):
        h=q[j];age=p['time']-h['time']
        if age>3000 or h['time']<p['time']-p['preempt']:break
        past.append([j,fade(p,h['time'])*quintic(h['jump'],15,150)*limit(2-age/1500)])
    for j in range(i+1,len(q)):
        h=q[j];age=h['time']-p['time']
        if age>3000 or p['time']<h['time']-h['preempt']:break
        future.append([j,fade(h,p['time'])*limit(2-age/1500)])
    for k,j in enumerate(range(i-1,-1,-1)):
        if k and p['time']-q[j+1]['time']>=2000:break
        h=q[j]
        if h['angle'] is None or p['angle'] is None:continue
        cap=math.pi
        if k>=2 and all(q[n]['angle'] is not None for n in [i-k,i-k+1,i-k+2]):
            recent,previous,earlier=[q[n]['angle'] for n in [i-k,i-k+1,i-k+2]]
            contrast=abs(previous-h['angle'])+abs(earlier-recent)
            blend=ramp(min(h['angle'],recent)*180/math.pi,20,5)*ramp(max(h['angle'],recent)*180/math.pi,60,120)
            cap=math.pi+(.1*contrast-math.pi)*blend
        operand=min(abs(p['angle']-h['angle']),cap)*quintic(h['jump'],0,50)
        term=math.cos(3*min(math.pi/6,operand))*(1-ramp(h['strain'],200,2000))
        angular_total+=term;angular.append([j,term,cap,operand])
    angle=min(1.,max(.2,2/angular_total)) if angular_total else 1.
    backward=sum(v for _,v in past);forward=sum(v for _,v in future)
    next_jump=q[i+1]['jump'] if i+1<len(q) else None
    forward_root=math.sqrt(forward)*(quintic(next_jump,15,150) if next_jump is not None else 1.)
    velocity=max(1.,p['jump']/p['strain'])
    movement=max(0.,(backward+forward_root)**1.7*.4*angle*velocity-2.5)**.45*2.4
    short=((500-p['preempt']+abs(p['preempt']-500))/2)**2.5/140000*angle*velocity
    combined=(short**1.5+movement**1.5)**(1/1.5)
    raw=combined/(1-.8**(p['strain']/1000)) if i else 0.
    return raw,dict(past=past,future=future,angles=angular,angle=angle,backward=backward,forward=forward,
        next_jump=next_jump,forward_root=forward_root,velocity=velocity,movement=movement,short=short,raw=raw)

def movement_terms(q,i):
    if i<=1:return 0.,0.,0.
    p=q[i];h=q[i-1];a=p['angle'];b=h['angle'];t=p['strain'];u=h['strain'];d=p['jump'];e=h['jump']
    v=d/t;w=e/u;radius=max(1.,1+float(s(s(s(30)-p['radius'])/s(70))))
    wide=lambda x:cubic(x,math.radians(140),math.radians(40))
    narrow=lambda x:cubic(x,math.radians(40),math.radians(140))
    factor=1.
    if a is not None and b is not None:
        total=0.
        for n in range(i-1,max(-1,i-7),-1):
            old=q[n]
            if max(t,old['strain'])>1.1*min(t,old['strain']):break
            if old['axis'] is not None and p['axis'] is not None:total+=math.cos(8*min(math.pi/16,abs(p['axis']-old['axis'])))
        weight=min(.5/total,1.)**2 if total else 1.
        spacing=quintic(d,0,100);z=math.cos(2*min(math.pi/4,abs(a-b)*spacing))
        base=1-.15*wide(b)*z;factor=(base+(1-base)*weight*.5*spacing)**2
    m=v*factor
    if a is not None and b is not None:
        lo=0.
        if max(t,u)<1.25*min(t,u):
            k=wide(a);lo=k*(.08+.92*(1-min(k,wide(b)**3)))*min(v,w)*quintic(60000/(2*t),300,400)*quintic(d,0,200)
        k=narrow(a);hi=k*(.25+.75*(1-min(k,narrow(b)**3)))*min(d/t**1.45,e/u**1.45)
        if i>=3:
            close=length(q[i-3]['position'],h['position'])
            if close<s(1):hi*=1-.55*float(s(s(1)-close))
        extra=min(v,w)*quintic(d,50,100)*ramp(d,300,100)**1.8*quintic(a,math.radians(110),math.radians(60))
        extra*=quintic(e,50,100)*ramp(e,300,100)**1.8*quintic(b,math.radians(110),math.radians(60))
        m+=max(lo*2.41,hi*9.67);m+=extra*1.02
    contrast=cubic(abs(v-w)/max(v,w),0,1) if max(v,w) else 0.
    boost=min(125/min(t,u),abs(v-w))*contrast
    m=(m+boost*(min(t,u)/max(t,u))**2*.9)*radius/(1-.03**((t/1000)**.65))
    z=v*math.sqrt(radius)*(1+min(.25,((max(t,u)-min(t,u))/50)**4))
    if a is not None and b is not None:z*=.8+math.sqrt(math.sin(abs(a-b)/2)*180/(t*.1)/270)
    def overlap(x,y):return limit(1-(max(length(x['position'],y['position'])-x['radius'],0)/x['radius'])**2)
    repeat=1-overlap(p,h)*overlap(p,q[i-2])*overlap(h,q[i-2]) if i>2 else 1.
    if a is not None:z+=v*wide(a)*repeat
    z=(z+boost*repeat*.52)**1.45*quintic(d,0,50)
    j=(min(d,120)/120*1000/t)*radius**1.5/(1-.2**(t/1000))
    return m,j,z

def composite(q,i):
    m,j,z=movement_terms(q,i)
    # Early M11c1/M408c returns do not remove the separate jump operand.
    if i<=1:
        p=q[i];r=max(1.,1+float(s(s(s(30)-p['radius'])/s(70))))
        j=min(p['jump'],120)/120*1000/p['strain']*r**1.5/(1-.2**(p['strain']/1000))
    a=m*70.9;b=j*2.35;c=z*242;norm=(a**1.2+b**1.2)**(1/1.2)
    mix=0. if c==0 and norm else 1. if not norm else 1/(1+math.exp(-7.27*math.log(c/norm)))
    od=(79.5-q[i]['window']/2)/6
    return (norm*mix+c*(1-mix))*1.12*(.985+max(0.,od)**2/4000)

def sorted_sum(values,shape):
    return sum(v*(1+shape/(i+1))/(i**.9+1+shape/(i+1)) for i,v in enumerate(sorted((v for v in values if v>0),reverse=True)))

def duration_sum(pairs,states):
    if not pairs:return 0.,[]
    segments=[];retained=0.;origin=pairs[0]['time'];deadline=origin+400;peak=states[0];candidates=deque()
    def write(span):
        nonlocal retained
        item=[peak,float(round(span))];low=0;high=len(segments)-1
        # The declared framework BinarySearch tie policy is preserved.
        while low<=high:
            mid=(low+high)//2
            if segments[mid][0]==peak:low=mid;break
            if segments[mid][0]>peak:low=mid+1
            else:high=mid-1
        segments.insert(low,item);retained+=span
        while retained>11/(1-.9)*400:retained-=segments.pop()[1]
    for i in range(1,len(pairs)):
        t=pairs[i]['time'];value=states[i]
        while deadline<t:
            write(deadline-origin);origin=deadline
            peak=states[i-1]*.2**((origin-pairs[i-1]['time'])/1000)
            if candidates:
                previous,stamp=candidates.popleft();deadline=stamp+400;peak=max(peak,previous)
            else:deadline=origin+400
        if value>peak:
            candidates.clear();write(t-origin);origin=t;deadline=t+400;peak=value
        else:
            while candidates and candidates[-1][0]<value:candidates.pop()
            candidates.append((value,t))
    write(deadline-origin)
    pending=deque((v,dt) for v,dt in segments if v>0);elapsed=0.
    while pending and elapsed<4000:
        v,span=pending.popleft()
        for n in range(math.ceil(span/20) if span>0 else 0):
            scale=.727+.273*math.log10(1+9*limit((elapsed+20*n)/4000))
            pending.append((v*scale,float(round(min(20,span-20*n)))))
        elapsed+=span
    processed=sorted(pending,key=lambda x:x[0],reverse=True);position=0.;total=0.
    for v,span in processed:
        next_position=position+span/400;total+=v*(.9**position-.9**next_position);position=next_position
    return total/(1-.9),processed

def chart(circles,radius,preempt,window):
    q=geometry(circles,radius,preempt,window);states=[0.,0.,0.];trace=[];out=[[],[],[]]
    for i,p in enumerate(q):
        tau=p['strain'];m=composite(q,i);rh,rhtrace=rhythm(q,i);vis,vtrace=visibility(q,i)
        dt=tau/limit(tau/window/.93) if tau/window/.93>=.92 else tau/.92
        bonus=.75*((75-dt)/40)**2 if dt<75 else 0.
        raw1=(1+bonus)*1000/dt/(1-.3**(tau/1000))*(1-tap(p,q[i+1] if i+1<len(q) else None))
        decay=[.2**(tau/1000),.3**(tau/1000),.8**(p['delta']/1000)]
        inputs=[m,raw1,vis];scales=[1.,1.16,2.5]
        for k in range(3):states[k]=states[k]*decay[k]+(inputs[k]*(1-decay[k]))*scales[k]
        values=[states[0],states[1]*rh,states[2]]
        for k in range(3):out[k].append(values[k])
        trace.append(dict(pair=p,raw0=m,rhythm_multiplier=rh,rhythm=rhtrace,visibility=vtrace,raw1=raw1,states=list(states),outputs=values))
    q0,segments=duration_sum(q,out[0]);q1=sorted_sum(out[1],20.)
    prefix=sum(1 for p in q if p['time']<=q[0]['time']+60000) if q else 0
    v=[x for x in out[2] if x>0]
    for i in range(min(len(v),prefix)):v[i]*=math.log10(1+9*limit(i/prefix))
    q2=sorted_sum(v,1.)
    scaled=[q0**.63*.02275,math.sqrt(q1)*.0675,math.sqrt(q2)*.0675]
    energies=[4*x**3 for x in scaled];star=(1.12*sum(x**1.1 for x in energies)**(1/1.1))**(1/3)
    cached=s(star)
    return dict(pairs=q,trace=trace,outputs=out,aggregates=[q0,q1,q2],processed_segments=segments,
        component2_transformed=v,final=dict(double=star,single=cached,single_bits_le=struct.pack('<f',cached).hex(),display=math.floor(abs(cached)*100)/100))
