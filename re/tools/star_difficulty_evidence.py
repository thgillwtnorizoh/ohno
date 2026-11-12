#!/usr/bin/env python3
"""E044 pinned, read-only CIL/metadata extraction; never loads the original CLR.

Requires dnfile 0.18.0, dncil 1.0.2, pefile 2024.8.26. Only instruction
relationships, metadata and hashes are emitted; no original code/assets.
"""
import hashlib
import json
import math
import struct
from pathlib import Path
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import circle_hitsound_evidence as collector
from hitsound_resource_evidence import StaticStrings

SOURCE = '00ad4e790e5a0c23bc6bb4476a2313951dcb996f'
CLUSTERS = [0x11b,0x11c,0x138,0x139,0x13a,0x72f,0x730,0x72e,0x5d3,
    0x8f2,0x8f1,0x3da,0x845,0x846,0x847,0x379,0x37a,0x3db,0x3dc,
    0x6fe,0x698,0x699,0x378,0x262,0x57c,0x87c,0x1da,0x325,0x326,
    0x40e,0x40f,0x84a,0x879,0x87a,0x87b,0x13b,0x13c,0x9b3,0x40c,0x952]
SUPPORT = [0x651,0x652,0x28a2,0x28a3,0x28ab,0x28be,0x28c6,0x28ea,
    0x28eb,0x28ec,0x28f7,0x28d2,0x2902,0x48de,0x48df,0x48e0,0x48e1,
    0x142d,0x1432,0x1433,0x1434,0x1440,0x1441,0x144a,0x144b,0x1259,
    0x125a,0x28b0,0x28b2,0x28b3,0x28bc,0x0fe2,0x0fe4,0x0fea,0x1bb2,0x1bb3,0x1bb4,0x1bb8,0x1bc0,0x1bc1,
    0x1bc6,0x1bc7,0x1bc9,0x1bce,0x1bcf,0x1bd0,0x1bd5,0x1bd7,0x1bf2,
    0x1bf3,0x1c1e,0x1c1f,0x1c23,0x25d1,0x2d7b,0x2d7d,0x2d7e,0x2d7f,
    0x47c8,0x432d,0x4ec9,0x4ecf,0x4eee,0x4ef0,0x4ef4,0x4efd,0x4efe,
    0x45eb,0x45ec,0x1883,0x17c2,0x05ea,0x3201,0x09e3,0x09e4,0x09e5]
# Declared finite dispatch, independently audited against ancestry/name/signature.
RESOLUTIONS = [(0x8c5,0x802),(0x8c6,0x804),(0x8d0,0x806),(0x8d1,0x807),
    (0x8d2,0x808),(0x8d3,0x809),(0x369a,0x80a),(0x369b,0x810),
    (0x369c,0x811),(0x43e2,0x3f33),(0x43e3,0x19e9),
    (0x3f32,0x19e7),(0x3f37,0x19e6),(0x43e2,0x1d06),
    (0x43e3,0x1d08),(0x1d05,0x3539),(0x1d05,0x325d),
    (0x1d07,0x325f),(0x1d03,0x3536),(0x1d04,0x3537),
    (0x1259,0x28b2),(0x125a,0x28b3),(0x28bc,0x652)]

def sha(b): return hashlib.sha256(b).hexdigest()
def reference_cases():
    rows=[]
    def add(initial,prediction,sites,boundary='Unmeasured original x86 evaluation, helper failures and filesystem/worker scheduling remain conditional.'):
        rows.append({'id':f'D{len(rows)+1:02}','status':'RECONSTRUCTED STATIC PREDICTION',
            'initial_state':initial,'prediction':prediction,'sites':sites,'unresolved':boundary,'original_runtime':'NOT RUN'})
    add('Ordinary mode0 factory','M1bc6 constructs T011b -> T072f -> T0138. M08d9 itself is nonvirtual despite callvirt.', ['M1bc6:002c','M0801:0002','M3696:0002','M08d9'])
    add('Empty source filename/folder at M1bc0','Constructor can retain null F04d6; M08d9 returns -1 before its catch region.', ['M1bc0:0000-001b','M08d9:0000-0011'])
    add('Non-null map clone, preparation cannot establish object list','M08d9 null-list guard returns0, not proof of a valid empty chart.', ['M08d7','M08d9:0049-005b'])
    add('Reached helper throws inside M08d9 try','Caught Exception returns0; prior mutations remain. Constructor exceptions outside the try are different.', ['M08d9:0013-009a'])
    add('Valid prepared empty list','M080a returns a new result with default Double F1094=0; no skill aggregation required.', ['M3699:0017-002b','M080a:0000-0028'])
    add('Exactly one valid prepared circle, no mods','No pair is built; normally returning fresh components aggregate0.', ['M0810:0053-00b0','M1d08','M19e9','M080a'])
    add('Two prepared circles, dt1000, positive radius, window99, no mods','Conditional projection in N-controls; movement and visibility early guards yield0, interval component yields1.16 before final conversion.', ['M11c1:0000-0032','M408c:0000-0032','M1e57:0000-001e','M3539','M080a'])
    add('Same two circles at same versus distinct locations','The bounded index0 early guards make the computed star equal for the stated two-circle preparation; not a claim for three or more objects.', ['M11c1','M408c','M1e57','M0810'])
    add('Two prepared circles at dt100/200/500/2000, window99','Finite two-circle projections vary interval operand with dt; no supplied .osu is executed.', ['M0d6c','M3539','M080a'])
    add('Identical raw start timestamps','Unchecked Int32 delta0 becomes Double0 then strain_time Max(0,25)=25; other fields still retain0.', ['M2cb4:001d-002d','M366d:0016-002a'])
    add('Raw starts out of order','Pair construction follows F04d8 order before OrderBy; negative delta clamps only the strain-time field. No universal successful import prediction.', ['M0810','M3699:002c-0056','M366d'])
    add('Int32 subtraction overflows','Sub occurs before conv.r8; signed wrap can create negative delta before Max25.', ['M2cb4:001d-002d'])
    add('No-mod clock rate','F04d9 becomes Double100/100=1; pair timestamps divided by it.', ['M08d6:0048-0058','M2cb4'])
    add('DoubleTime64 versus HalfTime256','Rate1.5 versus.75; if both artificial bits set,64 has priority. Normal mod enumeration excludes both.', ['M08d6','M08e8:0043-004f'])
    add('Repeated invocation with same CS-affecting bits','Manager may be reused after setting mods/configuration; components are freshly allocated in each M3699.', ['M0807','M08d9:005d-0084','M0811'])
    add('Easy2 or HardRock16 changes between invocations','XOR&18 triggers object-manager reconstruction and new preparation.', ['M0807:0000-0018','M08d9:0039-0044'])
    add('Scalar5, Easy2 outside Edit1','Difficulty helper divides by2 before interpolation; HardRock separately multiplies/clamps if present.', ['M28b0:0000-005c','M28b3'])
    add('Scalar5, HardRock16 outside Edit1','CS normalization uses1.3; hit-window/approach interpolation uses1.4.', ['M28b2','M28b3','M28b0'])
    add('Active application mode Edit1','M28b0 skips Easy/HardRock scalar changes; this is an external state boundary.', ['M28b0:0000-005b'])
    add('Fresh valid slider parsed in calculator','M0808 invokes M144a(false) then M1441; nested schedule and geometry feed T072e. General slider numerical result remains partial.', ['M0808:0015-002d','M1441','M3692'])
    add('Slider formats7 versus8 and above','M1441 changes tick-spacing inheritance at8; E005/E032 geometry/version branches still apply.', ['M1441:0000-0044','M144a'])
    add('Slider with repeated traversals','M3692 consumes duration/effective traversals and mixed nested entries; do not replace with endpoint distance alone.', ['M3692:00b8-0124,0148-0234'])
    add('Current spinner in T072e','Hit-window-derived parameter is set0; movement geometry early-return paths and skill evaluators distinguish spinner.', ['M366d:006c-0088','M3691:0070-0089','M0d6c:0000-0016'])
    add('All prepared objects spinners, finite ordered times','Under normal nonexceptional preparation, no-mod per-object contributions remain0. No bonus/rotation gameplay inputs enter this calculator.', ['M11c1','M29f7','M408c','M0d6c','M1e57'])
    add('Three-plus circles, alternating angles/timing','General M11c1/M408c/M1744/M1e57 chain required; extracted but not a complete independently checked star projection.', ['M19e8','M3539','M325e'],'Semantic/finite validation blocker: original evaluators and history helpers listed in readiness matrix.')
    add('Circle after slider','Lazy slider position/travel and penultimate nested endpoint affect spacing/angle; retain Single vector operations.', ['M3691:0125-01d9','M3693','M3695'])
    add('Identical coordinates','Vector distance may be0; no universal difficulty0 because independent interval/history component exists.', ['M3691','M0d6c','M3539'])
    add('Raw mathematical channel0 positive finite','M080d Pow(raw,.63)*.02275 then M080b left-associated cube*4.', ['M080d:0000-0019','M080b:0000-0011'])
    add('Raw mathematical channels1/2 positive finite','Sqrt(raw)*.0675 then cube*4.', ['M080e','M1d09'])
    add('No Flashlight bit','M0811 does not construct T084a; optional energy is0.', ['M0811:002b-004c','M080a:00aa-00c4'])
    add('Flashlight1024 bit','T084a added; transformed optional value is squared*25 and combined asymmetrically with transformed channel2.', ['M0811:002b-0047','M3f4e','M080c'])
    add('Combination with first or second operand nonpositive','M080c finite shortcut returns the other operand; NaN follows unordered combine arm.', ['M080c:0000-001b'])
    add('All final energies0','Generalized norm and final cube-root return0 in declared projection profile.', ['M080a:00ec-0114','M19d8','M080f'])
    add('Final transformed energies positive','Norm exponent1.1; multiply1.12 then Math.Pow(x, Double1/3). No rounding here.', ['M080a:00ec-0114','M080f','M19e2'])
    add('Component0 initial pair at time1000','Envelope begins at pair start1000 and next1400; not a universal global multiple400.', ['M3f33:0008-0023'])
    add('Envelope next boundary equals current object time','Strict bgt in M3f34 does not close interval at equality; immediately above can close one or more.', ['M3f34:00a6-00b2'])
    add('Component0 retained duration exceeds threshold','M3f35 trims whole trailing segments while duration>11/(1-.9)*400; preceding mutations remain.', ['M3f31:002b-0046','M3f35:006d-0081'])
    add('Component0 sorted processed segments','M19ec filters value>0, applies initial4000 duration transform in20 slices, then descending sorting; M19e9 duration weights.', ['M19ec','M19e9'])
    add('Component1/2 aggregation','M1d08 sorts positive object values; weight numerator1+s/(i+1), denominator Pow(i,.9)+1+s/(i+1); s20 for component1,1 for component2.', ['M1d08:0044-00df','M3536'])
    add('Equal positive aggregation values','Retain both entries with successive weights; no deduplication.', ['M1d08:0095-00e3'])
    add('Nonpositive/NaN aggregation entries','Ordered positive predicates exclude them; full-chart NaN propagation remains helper-specific.', ['M1d0d','M3263','M19ef'])
    add('Component2 first60000ms density prefix','M3260 counts through equality and stops on first later item; M325f scales earliest positive values with log interpolation.', ['M3260:000f-0069','M325f'])
    add('Background mode0 modifier generation','Six binary choices256,64,2,16,1024,8 with pair exclusions ->36 compatible masks including0, conditional on hash-based overlap filtering not colliding.', ['M0804','M08df','M08e8','M08f2'],'GetHashCode-based overlap tests are external runtime operands; no measured enumeration order/count.')
    add('NoFail1, TouchDevice4, Nightcore512 or Relax8192 raw lookup bits','Ordinary cache mask1370 removes these. Direct artificial calculator calls can contain additional bits tested by evaluators.', ['M0803','M0805','M08cf','M19e7','M325e'])
    add('Same normalized key from distinct raw mods','Dictionary lookup shares that cache entry; not proof of identical unrestricted direct calculation.', ['M1bc7:0000-003b'])
    add('Missing key0 and not in progress','M1bd7 returns true; missing cached getter separately returns Double0.', ['M1bd7:0000-001d','M1bc7:001e-0027'])
    add('Negative key0 with/without F1014','Not-busy allows scheduling; busy suppresses it; display takes positive magnitude.', ['M1bd7:001e-0042','M1bd5'])
    add('NaN key0','bge.un branches to false scheduling result; Abs remains NaN and ordered display gate differs.', ['M1bd7:0030-0042','M1bd5','E043 M1bdb'])
    add('DB version20250106 versus20250107','Representation conversion Double->Single and old-key filtering versus direct Single dictionaries; no formula switch here.', ['M1bf2:0103-0174'])
    add('DB version below per-mode F0fbd cutoff','Store -Abs(Single) stale marker; magnitude is not recalculated by this branch.', ['M1bf2:0175-01d6'])
    add('Calculated Double at Single/display boundary','M1bd0 conv.r4 can move value across floor boundary; display is Abs(promoted cachedSingle) then Floor(*100)/100.', ['M1bd0:0026-002c','M1bc7:003a','M1bd5','M2d7f'])
    add('Worker runs during Play2','M1c1e sleeps1000 then rereads mode; no cancellation/fresh chart snapshot guaranteed.', ['M1c1e:000d-001f'])
    add('Worker normally completes','Clear in-progress before scheduling M1c23; merge key results into retained map then invoke completion.', ['M1c1e:00d4-0115','M1c23:0000-0031','M1bc9'])
    add('Map replaced while worker pending','M1c23 writes retained map reference, not a fresh global selection; no hash/generation check in this callback.', ['M1c23:0000-001c','M25d1:0049-005c'])
    add('Exception escapes M1bd0/mod enumeration','Worker does not wrap whole computation in a flag-clearing finally; F1014 may remain true, timeout callback separate.', ['M1c1e:0097-00ec','M1bd0'])
    add('Cleanup after ordinary successful dictionary production','M08cd disposes manager then clears field; no encompassing finally in M1bd0.', ['M1bd0:003b-0042','M08cd'])
    add('Timeout callback reached','M1c1f clears F1014 and invokes completion before conditional task/scheduler actions. It does not itself prove an old worker cannot write later.', ['M1c1f:0000-00d4'])
    add('Default freshness cutoffs','M1bb4 initializes mode cutoffs20260630/20260630/20260630/20231214 from F0c9a. These are cache policy constants, not authenticated release dates.', ['M1bb4:0000-0011','F0c9a RVA data'])
    return rows
def safe(v):
    if isinstance(v,float) and not math.isfinite(v):
        return 'NaN' if math.isnan(v) else '+Infinity' if v>0 else '-Infinity'
    if isinstance(v,list): return [safe(x) for x in v]
    if isinstance(v,dict): return {k:safe(x) for k,x in v.items()}
    return v

def extract(path):
    ab=Path(path).read_bytes();assert sha(ab)==collector.ARCHIVE
    with zipfile.ZipFile(path) as z: raw=z.read('osu!.exe'); members=len(z.infolist())
    assert sha(raw)==collector.EXE
    p=dnfile.dnPE(data=raw);ts=p.net.mdtables
    owners={m.row_index:n for n,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    fowners={f.row_index:n for n,t in enumerate(ts.TypeDef,1) for f in t.FieldList}
    mids=sorted(set(SUPPORT)|{m.row_index for n in CLUSTERS for m in ts.TypeDef.rows[n-1].MethodList})
    fs={f.row_index for n in CLUSTERS for f in ts.TypeDef.rows[n-1].FieldList}
    fs|={0xfea,0x1014,0xfbd,0xfca,0xfec,0xfd1,0x100b,0x17ee,0x17ef,0x17fb,
         0xada,0xadb,0xade,0xb52,0xb53,0xb54,0xb55,0xb56,0xb57,0x2523,0x2524,0x252c}
    collector.METHODS=mids;collector.FIELDS={0x4000000+x for x in fs}
    collector.CALLS={0x6000000+x for x in mids}
    collector.SLOTS=sorted(set(a for a,b in RESOLUTIONS))
    out=collector.extract(path)
    out.update(expedition=44,research_source=SOURCE,input_bytes={'archive':len(ab),'exe':len(raw),'zip_members':members},
        cluster_types=[hex(0x2000000+n) for n in CLUSTERS],
        method_scope='Complete declared mathematical type clusters plus bounded preparation/cache methods. Membership alone is NOT reachability or a completed mathematical recovery.',
        watched_fields=[hex(0x4000000+n) for n in sorted(fs)])
    out['type_inventory']={}
    todo=set(CLUSTERS)|{owners[n] for n in mids}
    while todo:
        n=todo.pop();t=ts.TypeDef.rows[n-1];key=hex(0x2000000+n)
        if key in out['type_inventory']:continue
        base=None
        if t.Extends and t.Extends.row:
            base=hex({'TypeDef':0x2000000,'TypeRef':0x1000000,'TypeSpec':0x1b000000}[t.Extends.table.name]+t.Extends.row_index)
            if t.Extends.table.name=='TypeDef':todo.add(t.Extends.row_index)
        out['type_inventory'][key]={'name':str(t.TypeNamespace)+'.'+str(t.TypeName),'base':base,
            'methods':[{'token':hex(0x6000000+m.row_index),'name':str(m.row.Name),'signature':m.row.Signature.value.hex(),
                'virtual':m.row.Flags.mdVirtual,'new_slot':m.row.Flags.mdNewSlot,'abstract':m.row.Flags.mdAbstract} for m in t.MethodList],
            'fields':[hex(0x4000000+f.row_index) for f in t.FieldList]}
    out['dispatch']=[{'slot':hex(0x6000000+a),'implementation':hex(0x6000000+b),
        'receiver':hex(0x2000000+owners[b])} for a,b in RESOLUTIONS]
    out['method_specs']={};out['call_graph']=[];out['referenced_methods']={}
    touched_fields=set(fs)
    mathops={'add','sub','mul','div','div.un','rem','neg','and','or','xor','ceq','cgt','clt','cgt.un','clt.un','shl','shr','shr.un'}
    for n in mids:
        r=out['methods'][hex(0x6000000+n)];m=ts.MethodDef.rows[n-1]
        if not m.Rva:continue
        body=read_method_body_from_bytes(p.get_data(m.Rva));r['numeric_sites']=[]
        for i in body.instructions:
            op=i.opcode.name;off=i.offset-body.header_size;v=getattr(i.operand,'value',None)
            if op in mathops or op.startswith('conv.'):
                r['numeric_sites'].append([off,op])
            if v and v>>24==4:touched_fields.add(v&0xffffff)
            if not v or op not in ['call','callvirt','newobj','ldftn','ldvirtftn']:continue
            original=v
            if v>>24==0x2b:
                s=ts.MethodSpec.rows[(v&0xffffff)-1]
                v={'MethodDef':0x6000000,'MemberRef':0xa000000}[s.Method.table.name]+s.Method.row_index
                out['method_specs'][hex(original)]={'method':hex(v),'instantiation':s.Instantiation.value.hex()}
            out['call_graph'].append([hex(0x6000000+n),off,op,hex(original),hex(v)])
            if v>>24==6:
                q=ts.MethodDef.rows[(v&0xffffff)-1]
                out['referenced_methods'][hex(v)]={'owner':hex(0x2000000+owners[v&0xffffff]),'name':str(q.Name),
                    'signature':q.Signature.value.hex(),'virtual':q.Flags.mdVirtual,'new_slot':q.Flags.mdNewSlot,'extracted':(v&0xffffff) in mids}
    for n in touched_fields:
        f=ts.Field.rows[n-1];out['fields'][hex(0x4000000+n)]={'owner':hex(0x2000000+fowners[n]),
            'name':str(f.Name),'signature':f.Signature.value.hex(),'static':f.Flags.fdStatic}
    # Explicitly include the mod enum values; no familiar naming assumed.
    t=ts.TypeDef.rows[0xb56];ids={f.row_index for f in t.FieldList}
    out['mod_enum']={str(c.Parent.row.Name):int.from_bytes(c.Value.value,'little',signed=True)
        for c in ts.Constant if c.Parent.table.name=='Field' and c.Parent.row_index in ids}
    r=next(x for x in ts.FieldRva if x.Field.row_index==0xc9a);array=p.get_data(r.Rva,16)
    out['cache_cutoff_data']={'field':'0x4000c9a','rva':hex(r.Rva),'bytes':16,'sha256':sha(array),'int32_values':list(struct.unpack('<4i',array))}
    dec=StaticStrings(p);out['string_decoder']=dec.provenance;out['strings']={};out['string_sites']=[]
    for n in [0x809,0x1bc0]:
        b=read_method_body_from_bytes(p.get_data(ts.MethodDef.rows[n-1].Rva));ii=list(b.instructions)
        for j,i in enumerate(ii):
            if getattr(i.operand,'value',None)==0x60049b0 and j and ii[j-1].opcode.name=='ldc.i4':
                k=ii[j-1].operand;out['strings'][str(k)]=dec.decode(k);out['string_sites'].append([hex(0x6000000+n),i.offset-b.header_size,k])
    root=Path(__file__).resolve().parents[1];repo=root.parent
    paths=['re/contexts.md','reconstructed/contexts.md','re/43-original-selectplay-beatmap-selection-and-gameplay-entry.md',
        're/evidence/43/selectplay.json','re/32-bezier-slider-geometry.md','reconstructed/R16-original-derived-selectplay.md','re/qa/qa14/README.md']
    out['preparation_inputs']={name:sha((repo/name).read_bytes()) for name in paths if name!='re/contexts.md'}
    out['collector_sources']={name:sha((root/'tools'/name).read_bytes()) for name in ['star_difficulty_evidence.py','circle_hitsound_evidence.py','beatmap_init_evidence.py','hitsound_resource_evidence.py']}
    out['limits']=['Static original-byte extraction only; no original CLR/native/game execution',
        'Direct inventories exclude reflection/external/native mutation',
        'Extracted subroutine is not a verified independent implementation',
        'No full-chart star oracle: general per-object evaluators/slider nested preparation remain partial',
        'No original-x86 floating/runtime, scheduler, provider or persistent-cache freshness measurement',
        '2175 historical QA checks not rerun; E02017 cases NOT RUN; no R17/E045/QA15']
    out['reference_cases']=reference_cases()
    out['mathematical_readiness']={'final_formula':'Recovered for explicit component accumulators',
        'general_no_mod_chart':'PARTIAL — not ready for faithful R17 chart calculation',
        'blocking_math_methods':['M11c1/M11c3','M408c/M408d','M1744/M1746-M174f','M1e57-M1e65'],
        'blocking_preparation':['M1441/M432d nested event identity','M3691/M3692 slider lazy travel and history','original nonordinary/global provider preparation'],
        'reason':'Instructions/body hashes are available. Full independent semantic and finite validation of history/angle/visibility and nested slider paths is not complete. No alternate-version algorithm is substituted.'}
    p.close();return safe(out)

if __name__=='__main__':
    result=json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2,allow_nan=False)+'\n'
    Path(sys.argv[2]).write_text(result,encoding='utf-8',newline='\n')
