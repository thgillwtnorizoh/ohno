#!/usr/bin/env python3
"""E046 observed side: unchanged E045 research projection, with identity logging.

Not an independent expected-value oracle. This adapter never executes original
methods. Snapshot decoration changes tracing only, never group arithmetic.
"""
import circle_star_history_projection as subject

def observed_rhythm(pair):
    original=subject.Pattern;serial=0;identities={}
    def identity(obj):
        nonlocal serial
        key=id(obj)
        if key not in identities:
            serial+=1
            identities[key]=(obj,serial) # Retain object: prevent address reuse.
        assert identities[key][0] is obj
        return identities[key][1]
    class Identified(original):
        def __init__(self,*a,**kw):
            super().__init__(*a,**kw);identity(self)
        def snapshot(self):return super().snapshot()+[identity(self)]
    subject.Pattern=Identified
    try:
        events=[];value=subject.rhythm(pair,events)
        return value,events
    finally:subject.Pattern=original

def evaluate(circles,radius,preempt,window):
    actual=subject.calculate_prepared(circles,radius,preempt,window)
    q=subject.prepare(circles,radius,preempt,window);details=[]
    for p in q:
        rhythm,groups=observed_rhythm(p);past=[];future=[];angles=[]
        backward=subject.past_visible(p,past);forward=subject.future_visible(p,future)
        angular=subject.visibility_angles(p,angles);next_pair=p.following()
        velocity=max(1.,p.jump/p.strain)
        movement=subject.visibility_movement(next_pair,velocity,angular,backward,forward)
        details.append(dict(rhythm=rhythm,groups=groups,past=past,future=future,angles=angles,
            backward=backward,forward=forward,angle=angular,next_jump=next_pair.jump if next_pair else None,
            movement=movement,raw=subject.visibility_raw(p)))
    actual['e046_observed_details']=details
    return actual
