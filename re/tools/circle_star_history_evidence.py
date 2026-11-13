#!/usr/bin/env python3
"""E045 deterministic original-static metadata extraction, never CLR execution."""
import hashlib,json,sys,zipfile
from pathlib import Path
import star_difficulty_evidence as e44

SOURCE='e44e63b2d2bf18c3ccb733b490d717dd8561d587'
def sha(b):return hashlib.sha256(b).hexdigest()
def extract(archive):
    # Extend in-memory collector scope only. Historical files remain unchanged.
    e44.SUPPORT=sorted(set(e44.SUPPORT)|{0x4ecb,0x24b0})
    d=e44.extract(archive)
    # Decode the actual parser key using E038's proven static decoder. The
    # independent checker uses its separate byte reducer, not this decoder.
    with zipfile.ZipFile(archive) as z:
        executable=z.read(next(n for n in z.namelist() if n.endswith('osu!.exe')))
    p=e44.dnfile.dnPE(data=executable)
    d['strings']['2064567436']=e44.StaticStrings(p).decode(2064567436)
    d['string_sites'].append(['0x6001bc1',0x5b8,2064567436]);p.close()
    d['expedition']=45;d['research_source']=SOURCE
    d['e044_reference_cases']=d.pop('reference_cases')
    d['e044_mathematical_readiness']=d.pop('mathematical_readiness')
    d['e044_authority_source']='00ad4e790e5a0c23bc6bb4476a2313951dcb996f'
    root=Path(__file__).resolve().parents[1];repo=root.parent
    d['authority_inputs']={name:sha((repo/name).read_bytes()) for name in [
        're/44-original-local-star-difficulty-calculation.md','re/evidence/44/star-difficulty.json',
        're/evidence/44/projection-checks.json','re/tools/star_difficulty_projection.py',
        're/tools/star_difficulty_evidence.py','re/tools/check_star_difficulty.py',
        're/43-original-selectplay-beatmap-selection-and-gameplay-entry.md',
        'reconstructed/R16-original-derived-selectplay.md','re/qa/qa14/README.md']}
    d['collector_sources']['circle_star_history_evidence.py']=sha(Path(__file__).read_bytes())
    d['corrections']=[{'authority':'E044 report','preserved_passage':'T0339 is a shared ancestor in circle/slider paths, not a sufficient test for a slider',
        'finding':'Ordinary T094c -> T035a -> T04c3, while slider T02af -> T0339 -> T04c3. Circle is not assignable to T0339.',
        'effect':'Ordinary circle-only M11c1/M408c/M1744 T0339 arms are excluded by ancestry, not by guessed travel zero.',
        'historical_action':'No E044 file modified; correction recorded only in E045.'}]
    d['dependency_graph']={
        'movement':['M11c1','M11c3','M11c2','M11c4','M11c5','M29f7','M29f8','M19ea','M19eb'],
        'velocity_angle':['M408c','M408d','M3694','M4ecb'],
        'rhythm':['M1744','M1745','M1746','M1747','M1748','M1749','M174a','M174b','M174c','M174d','M174e','M174f','M0d6c','M0d6d','M3690'],
        'visibility':['M1e57','M1e58','M1e59','M1e5b','M1e5c','M1e5d','M1e5e','M1e5f','M1e60','M1e63','M368f'],
        'aggregation':['M3f33','M3f34','M3f35','M3f38','M3f39','M3f3c','M19ec','M19e9','M325f','M3260','M1d08'],
        'inputs':['M28be','M28b2','M28b3','M28eb','M24b0','M0810','M366d','M3691','M3694'],
        'excluded':['M1e5a Hidden arm','M3692 slider lazy travel','T067f spinner arms','T0339 noncircle arms','T084a Flashlight component']}
    d['stack_preparation']={'parser':['M1bc1:05b3-05c2','M1bc1:07ab-07cb'],
        'field':'T03ad.F1004 Single clamped0..1',
        'position_mutation':['M28eb:003c-0042','M28eb:00c6->0124','M28eb:01b0->02e7','M28eb:0324','M24b0:0002'],
        'admission':'Explicit StackLeniency0 and strictly increasing circle start=end Int32 times make the threshold0 and time-gap exits precede any stack increment. F252d stays0 and F252c equals raw F152e.',
        'excluded':'Nonzero or absent leniency, equal-time registration, general stacking, arbitrary provider/configuration state'}
    specs=[
        ('N ordered circles','N-1 pairs; source registration retained before current-start sorting',['M0810','M3699'],'N objects'),
        ('No two prior difficulty pairs','First two angles null, M11c1/M408c index<=1 return0',['M3694','M11c1:0014','M408c:0014'],'early returns'),
        ('Constant spacing, dt24/25/26/99/100/101/1000','Raw dt retained; strain floor25; separate movement and velocity outputs',['M11c1','M408c'],'constant'),
        ('More than six angle predecessors','M11c3 inspects no more than six and applies repeated-angle weighting',['M11c3'],'six-predecessor'),
        ('Straight or right-angle history','Distinct movement and velocity-angle channels remain independently evaluated',['M11c1','M408c'],'angle branch'),
        ('Equal rhythm intervals','No rhythm transition sum; multiplier1',['M1744'],'equal intervals'),
        ('Alternating rhythm intervals','Mutable group transition produces finite positive rhythm sum',['M1744','M1746-M174f'],'alternating'),
        ('History length beyond32','Seed traversal yields at most30 processed comparisons for dense normal inputs',['M1744:0064-00c9'],'capacity32'),
        ('History age at5000','Strict traversal predicate distinguishes just-before and equality',['M1744:009a-00a3'],'5000ms'),
        ('Pattern interval difference equals tolerance','Strict parity/equality comparisons reject equality',['M174e','M174f'],'strict tolerance'),
        ('Visibility fade-in and start endpoints','No-mod visibility has explicit fade/preempt and after-start guard',['M368f'],'visibility interval'),
        ('Past/future history age3000','Equality enters processing; later item terminates scan',['M1e63','M1e5d'],'3000ms'),
        ('Angle-history elapsed2000','The first outside entry can be processed before updated elapsed stops next iteration',['M1e5e'],'2000ms'),
        ('Following pair jump differs from current','M1e58 uses next pair retained from M2cb6, not current pair',['M1e57:0028-0069','M1e58'],'following'),
        ('Envelope exact400','Strict crossing; no interval close at equality',['M3f34:00b2'],'strict400'),
        ('Mutable4000-duration prefix','Appended20-unit copies may themselves be consumed; Skip then descending sort',['M19ec'],'duration prefix'),
        ('First60000ms boundary','Count equality; transform positive prefix before descending sort',['M3260','M325f','M1d08'],'first-minute'),
        ('Two circle complete finite result','Interval-only closed scalar with instruction-derived adjusted timing',['M0d6c','M3539','M080a'],'two-circle'),
        ('Three circle complete finite result','Positive temporal sum, zero reached movement/visibility in finite dt1000 controls',['M11c1','M408c','M1e57','M1d08'],'small complete'),
        ('Four straight circles dt1000','First nonzero movement composite plus independent mutable duration oracle and temporal sum',['M19ea','M19ec','M080a'],'four-circle'),
        ('Coincident circle positions','No universal zero stars; interval component survives',['M0d6c'],'coincident'),
        ('Explicit CS5 AR5 OD5 viewport512x384','Prepared radius32.013118743896484, preempt1200, full window99',['M28be','M28b2','M28b3'],'manager'),
        ('Strict admitted v14 source with StackLeniency0','No-stack position proof; independent two/three/four-circle star scalar',['M1bc1','M28eb','M24b0'],'bounded osu-to-star'),
        ('Missing/nonzero leniency or equal timestamps','Research raw reader rejects; no guessed stack displacement',['M28eb'],'bounded reader rejects'),
        ('Signed zero/negative/overflow pair delta artificial control','Original Int32 subtraction occurs before Double; only strain clamped',['M2cb4','M366d'],'registered order'),
        ('Final Double near cache boundary','Convert to Single before absolute/floor display',['M1bd0','M1bd5','M2d7f'],'cache'),
        ('General dense circle history','All translated components execute, but six large traces have no independent golden scalar',['M11c1','M408c','M1744','M1e57'],'TRACE ONLY')]
    d['reference_cases']=[{'id':f'C{i:02}','initial_state':state,'prediction':pred,'sites':sites,
        'control_name_filter':test,'classification':'RECONSTRUCTED STATIC PREDICTION','original_runtime':'NOT RUN'}
        for i,(state,pred,sites,test) in enumerate(specs,1)]
    finite_ids={5,6,7,8,9,10,11,12,13,25,27,28,29,30,32,33,34,35,36,37,38,39,40,41,42,51}
    deferred={14,16,17,18,19,20,21,22,23,24,26,31,43,44,45,46,47,48,49,50,52,53,54,55,56,57,58}
    d['e044_boundary_closure']=[{'id':r['id'],'sites':r['sites'],'original_prediction':r['prediction'],
        'e045_status':('FINITE MATHEMATICAL SUBSET' if int(r['id'][1:]) in finite_ids else
                       'DEFERRED / UNCHANGED STATIC' if int(r['id'][1:]) in deferred else 'RETAINED STATIC'),
        'boundary':'Circle-only prepared finite controls do not execute original runtime, manager/provider, sliders/spinners, mods or worker lifecycle.'}
        for r in d['e044_reference_cases']]
    d['mathematical_readiness']={
        'e044_final_formula':'RETAINED; independent Decimal finite checks',
        'movement_history':'READY for declared prepared circle profile; translated all reached arms with finite angle/spacing controls',
        'velocity_angle':'READY for declared prepared circle profile; independent early/full angle controls',
        'rhythm_history':'PARTIAL independent validation: full circle translation, equal/alternating/group/window controls; arbitrary repeated-group histories lack independent chart golden',
        'visibility_history':'PARTIAL independent validation: full no-mod translation, following-pair and window controls; complex whole-chart golden remains absent',
        'circle_input':'CONDITIONAL explicit viewport/time-shift, v14 no-stack strict source class; prepared final positions otherwise supplied',
        'aggregation':'READY translated finite envelope/queue-prefix/first-minute controls; equal-value library tie/JIT precision conditional',
        'two_circle':'READY within admitted input and numerical profile',
        'three_four_circle':'READY only for independently closed finite cases, not all spatial/timing combinations',
        'general_circle':'PARTIAL: integrated evaluator exists, six non-golden traces are not full independent semantic closure',
        'sliders_spinners_modifiers':'DEFERRED',
        'general_R17':'NOT READY. Recommend only a separately authorized tiny circle subset, or further independent repeated-rhythm/visibility scalar closure.'}
    d['limits']=['No original CLR/game/native execution; metadata and math projections only',
        'No-mod ordinary circles only; explicit viewport/time-shift and admitted metadata; raw reader requires StackLeniency0 and strictly increasing circle times',
        'Original provider/parser/global state not emulated universally; strict research reader is ENGINEERING admission',
        'Original x86/JIT/library last-bit precision and equal-comparison native library interleaving unmeasured',
        'Historical2175 QA not rerun; all E02017 cases NOT RUN; no R17/E046/QA15']
    return d

if __name__=='__main__':
    Path(sys.argv[2]).write_text(json.dumps(extract(sys.argv[1]),indent=2,sort_keys=True,allow_nan=False)+'\n',encoding='utf-8',newline='\n')
