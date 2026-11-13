"""Deterministic research inputs; no expected values or subject-model imports."""
def from_intervals(intervals,positions=None):
    times=[0]
    for dt in intervals:times.append(times[-1]+dt)
    square=[(64,64),(448,64),(448,320),(64,320)]
    return [(t,*(positions[i] if positions else square[i%4])) for i,t in enumerate(times)]

def cases():
    rows=[
        ('repeating-count2',[300]+[100,100,200]*6),
        ('distinct-count2',[300,300]+[100,100,250,160,160,250,100,100,250,200,200,300,100,100,250]),
        ('different-counts',[300,300]+[100,100,250,100,100,100,250,150,150,250,100,100,250]),
        ('repeat-after-nonmatch',[300,300]+[100,100,250,160,160,250,100,100,250,100,100,250]),
        ('inside-tolerance',[300,300]+[100,129,250]*6),
        ('near-tolerance-outside',[300,300]+[100,130,250]*6),
        ('capacity-below',[100,100,200]*11),
        ('capacity-above',[100,100,200]*14),
        ('age-below',[300]+[100,100,200]*4+[100,100,1999]),
        ('age-equal',[300]+[100,100,200]*4+[100,100,2000]),
        ('age-above',[300]+[100,100,200]*4+[100,100,2001]),
        ('dense-right-angles',[100]*12),
        ('dense-short-preempt',[100]*18),
        ('combined-repeat-visibility',[300]+[100,100,200]*6)]
    result=[]
    for name,intervals in rows:
        result.append({'id':name,'circles':from_intervals(intervals),
            'cs':5.,'ar':10. if name in ['dense-short-preempt','combined-repeat-visibility'] else 5.,'od':5.,
            'viewport':[512.,384.],'time_shift':0,'classification':'BOUNDED RAW V14 / StackLeniency0'})
    # Distinct spatial histories retain the same admitted geometry limits.
    positions=[(64+(i*37)%380,64+(i*61)%250) for i in range(25)]
    result.append(dict(id='nearly-equal-angles',circles=from_intervals([100]*24,positions),cs=5.,ar=5.,od=5.,viewport=[512.,384.],time_shift=0,classification='BOUNDED RAW V14 / StackLeniency0'))
    positions=[(64 if i%2==0 else 448,192+(i%3)) for i in range(25)]
    result.append(dict(id='opposing-directions',circles=from_intervals([100]*24,positions),cs=5.,ar=5.,od=5.,viewport=[512.,384.],time_shift=0,classification='BOUNDED RAW V14 / StackLeniency0'))
    for tag,step in [('next-jump-zero',0),('next-jump-small',1)]:
        circles=from_intervals([100]*12);t,_,_=circles[10];_,x,y=circles[9];circles[10]=(t,x+step,y)
        result.append(dict(id=tag,circles=circles,cs=5.,ar=10.,od=5.,viewport=[512.,384.],time_shift=0,classification='BOUNDED RAW V14 / StackLeniency0'))
    result.append(dict(id='following-outside-window',circles=from_intervals([100]*11+[3001]),cs=5.,ar=10.,od=5.,viewport=[512.,384.],time_shift=0,classification='BOUNDED RAW V14 / StackLeniency0'))
    return result

def osu(row):
    return ('osu file format v14\n\n[General]\nMode:0\nStackLeniency:0\n\n[Metadata]\n'
        'Title:E046 independent history\nArtist:Work1 research\nCreator:Work1\nVersion:'+row['id']+'\n\n'
        '[Difficulty]\nHPDrainRate:5\nCircleSize:'+str(row['cs'])+'\nApproachRate:'+str(row['ar'])+
        '\nOverallDifficulty:'+str(row['od'])+'\nSliderMultiplier:1.4\nSliderTickRate:1\n\n'
        '[TimingPoints]\n0,500,4,1,0,100,1,0\n\n[HitObjects]\n'+
        ''.join(f'{x},{y},{t},1,0\n' for t,x,y in row['circles']))
