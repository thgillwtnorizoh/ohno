#!/usr/bin/env python3
"""Research-only finite mathematical projections, NOT a .osu star calculator.

All inputs are explicit prepared/artificial operands. No original CLR method,
gameplay parser, proprietary asset or reconstruction component is executed.
General M11c1/M408c/M1744/M1e57 and slider nested preparation are NOT emulated.
"""
import math
import struct

def single(x):
    try:return struct.unpack('<f',struct.pack('<f',x))[0]
    except OverflowError:return math.copysign(math.inf,x)

def bits(x):return struct.pack('<f',single(x)).hex()
def i32(x):return (x+2**31)%2**32-2**31
def divide(a,b):
    if b==0:
        return math.nan if a==0 or math.isnan(a) else math.copysign(math.inf,a*math.copysign(1,b))
    return a/b
def power(a,b):
    try:return math.pow(a,b)
    except ValueError:return math.nan
    except OverflowError:return math.inf
def clamp(x,lo,hi):
    if math.isnan(lo) or math.isnan(hi):return math.nan
    if x>hi:return hi
    if x<lo:return lo
    return x

def pair(start,previous,end,rate):
    delta=divide(float(i32(start-previous)),rate)
    return {'delta':delta,'start':divide(float(start),rate),
        'end':divide(float(end),rate),'strain_time':max(delta,25.0)}

def normalized_distance(dx,dy,radius):
    # M3691 uses Single Vector2 length and Single(50/radius) before conv.r8.
    xx=single(single(dx)*single(dx)); yy=single(single(dy)*single(dy))
    length=single(math.sqrt(single(xx+yy)))
    return float(single(length*single(single(50.0)/single(radius))))

def recurrence(old,raw,dt,base,multiplier=1.0):
    d=power(base,dt/1000.0)
    state=old*d
    state=state+((raw*(1.0-d))*multiplier)
    return state

def ordered_aggregate(values,strength=1.0,exponent=0.9):
    total=0.0; terms=[]
    for i,x in enumerate(sorted((x for x in values if x>0),reverse=True)):
        top=1.0+strength/(1+i)
        bottom=power(float(i),exponent)+1.0+strength/(1+i)
        weight=top/bottom
        total=total+x*weight;terms.append([x,weight])
    return total,terms

def duration_aggregate(segments,decay=.9):
    """M19e9 only: input is ALREADY the reached M19ec ordered segment output."""
    total=0.0;t=0.0
    for value,duration in segments:
        t2=t+duration/400.0
        w=power(decay,t)-power(decay,t2)
        total=total+value*w;t=t2
    return total/(1.0-decay)

def pnorm(p,values):
    total=0.0
    for v in values:total=total+power(v,p)
    return power(total,1.0/p)

def cube(v):return (v*v)*v
def combine(a,b):
    # bgt.un: NaN goes to the combine arm, not the finite <=0 shortcut.
    if a<=0.0:return b
    if b<=0.0:return a
    return pnorm(1.1,[a,b*clamp(b/a,.25,1.0)])

def final(raw0,raw1,raw2,raw3=None):
    scaled0=power(raw0,.63)*.02275
    scaled1=math.sqrt(raw1)*.0675 if raw1>=0 else math.nan
    scaled2=math.sqrt(raw2)*.0675 if raw2>=0 else math.nan
    scaled3=0.0 if raw3 is None else math.sqrt(raw3)*.0675 if raw3>=0 else math.nan
    a=4.0*cube(scaled0);b=4.0*cube(scaled1)
    c=4.0*cube(scaled2);d=25.0*(scaled3*scaled3)
    third=combine(c,d)
    joined=pnorm(1.1,[a,b,third])
    output=power(joined*1.12,0.3333333333333333)
    return {'scaled':[scaled0,scaled1,scaled2,scaled3],
        'energies':[a,b,c,d],'combined_third':third,'joined':joined,
        'double':output,'single':single(output),'single_bits_le':bits(output),
        'display':display(output)}

def display(v):
    cached=single(v);mag=abs(float(cached))
    return math.floor(mag*100.0)/100.0 if math.isfinite(mag) else 'NaN' if math.isnan(mag) else '+Infinity'

def speed_raw(dt,full_window,next_doubletap=0.0):
    """M0d6c finite ordinary nonspinner arm with supplied M3690 result."""
    adjusted=dt/clamp(dt/full_window/.93,.92,1.0)
    bonus=0.0
    if 60000.0/(adjusted*4)>200.0:
        bonus=.75*power((60000.0/4.0/200.0-adjusted)/40.0,2.0)
    value=(1.0+bonus)*1000.0/adjusted
    value=value*divide(1.0,1.0-power(.3,dt/1000.0))
    return value*(1.0-next_doubletap)

def two_circle(dt,full_window=99.0):
    """Conditional prepared two-circle control, dt>=100 (no bonus/short clamp).

    Index0 movement/visibility helpers return0; speed history multiplier1.
    Requires exactly two prepared circle objects, positive finite timing/radius,
    no mods, normal helper returns and no extra original objects.
    """
    if dt<100:raise ValueError('bounded control requires dt>=100')
    raw=speed_raw(dt,full_window)
    speed=recurrence(0.0,raw,dt,.3,1.16)
    return final(0.0,speed,0.0)

def cache_key(mods):return mods&1370
def requested(cached,in_progress=False):
    return not in_progress and (cached is None or cached<0.0)
def legacy_cache(values,version,cutoff):
    d={k:single(v) for k,v in values.items() if version>=20250107 or k==(k&1370)}
    if version<cutoff:d={k:single(-abs(v)) for k,v in d.items()}
    return d
