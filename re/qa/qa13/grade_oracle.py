"""QA13 finite oracle: exact rational -> IEEE754 binary32, then Double thresholds.
No production C# is imported or executed. Expected values come from E042 M2b47.
Original x86/JIT execution is explicitly not measured by this projection.
"""
from fractions import Fraction as Q
from pathlib import Path
import json, struct

def binary32(n, d):
    if not d:
        return 0x7fc00000, None
    if not n:
        return 0, Q(0)
    x=Q(n,d); exponent=0
    while x < 1: x*=2; exponent-=1
    while x >= 2: x/=2; exponent+=1
    significand=x*2**23
    integer,rem=divmod(significand.numerator,significand.denominator)
    twice=rem*2
    if twice>significand.denominator or (twice==significand.denominator and integer%2): integer+=1
    if integer==2**24: integer//=2;exponent+=1
    bits=((exponent+127)<<23)|(integer-2**23)
    value=Q(integer,2**23)*(Q(2)**exponent)
    return bits,value

def decide(counts,success,mods):
    n300,n100,n50,miss=counts;total=sum(counts)
    b300,p=binary32(n300,total);b50,f=binary32(n50,total)
    gt=lambda a,b:a is not None and a>Q.from_float(b)
    le=lambda a,b:a is not None and a<=Q.from_float(b)
    silver=bool(mods&8) or bool(mods&1024)
    # Ordered return sites 0028,0062/64,00b8/ba,00de,0102,0111,0113.
    decisions=[(not success,8),(p==1,0 if silver else 2),
        (gt(p,.9) and le(f,.01) and miss==0,1 if silver else 3),
        ((gt(p,.8) and miss==0) or gt(p,.9),4),
        ((gt(p,.7) and miss==0) or gt(p,.8),5),(gt(p,.6),6),(True,7)]
    grade=next(g for accepted,g in decisions if accepted)
    return dict(grade=grade,total=total,p300Bits=f'{b300:08x}',p50Bits=f'{b50:08x}',
        p300=float(p) if p is not None else 'NaN',p50=float(f) if f is not None else 'NaN')

def generate():
    cases=[]
    def add(name,counts,success=True,mods=0):
        cases.append(dict(id=name,counts=counts,success=success,mods=mods,expected=decide(counts,success,mods)))
    for mods in [0,8,1024,1032,16,2048]:
        add(f'all300-mod{mods}',[17,0,0,0],mods=mods)
        add(f'S-mod{mods}',[99,1,0,0],mods=mods)
    for threshold in [90,80,70,60]:
        for change in [-1,0,1]:
            n=threshold+change
            add(f'p300-{n}-noMISS',[n,100-n,0,0])
            add(f'p300-{n}-MISS',[n,0,0,100-n])
    for n in [98,99,100,101,102]:
        add(f'p50-{n}-of10000',[9900-n,100,n,0])
    for n in [0,1,2,3]:add(f'50-{n}-of100',[100-n,0,n,0])
    for counts in [[9,0,0,1],[1,0,0,1],[0,0,0,0],[0,0,0,1],[65535,65535,65535,65535],
                   [65535,0,0,0],[65534,1,0,0],[1,1,1,1],[0,10,0,0],[0,0,10,0]]:
        for success in [True,False]:add('counts-'+','.join(map(str,counts))+'-success'+str(success),counts,success)
    # Literal expectations independently anchor the predicate table, including R30 conflict.
    for c,g in [([80,20,0,0],4),([70,30,0,0],6),([60,40,0,0],6),([9,0,0,1],5),([99,0,1,0],3)]:
        assert decide(c,True,0)['grade']==g
    assert binary32(9,10)[0]==0x3f666666
    # Use the published original inputs, never its expected grade or R15 development output.
    evidence=json.loads((Path(__file__).parents[2]/'evidence/42/rank-results.json').read_text(encoding='utf-8'))
    for row in evidence['reference_cases'][:26]:
        state=row['initial_state']
        add('E042-'+row['id'],state['counts'],state['F1978'],state['mods'])
    target=Path(__file__).parent/'Inputs'/'grade-vectors.json'
    target.parent.mkdir(exist_ok=True)
    target.write_text(json.dumps(dict(schema='QA13.exact-rational.v1',authority='E042 T059e.M2b47 0001-0113; M2b44',
        numeric_profile='Exact integer quotient rounded once ties-to-even binary32; exact comparison with binary64 literals',cases=cases),indent=2)+'\n',encoding='utf-8')
    print(f'QA13 independent grade oracle: {len(cases)} vectors; R30 p300 bits 3f666666 -> B5')

if __name__=='__main__':generate()
