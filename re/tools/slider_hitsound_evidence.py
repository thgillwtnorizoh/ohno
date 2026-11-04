#!/usr/bin/env python3
"""E039 pinned original-static extraction. Never executes original managed/audio code.

Requires unchanged E004/E037/E038 collectors and dnfile 0.18.0, dncil 1.0.2,
pefile 2024.8.26. Output is selected sites/metadata, not redistributed audio.
"""
import hashlib
import json
from pathlib import Path
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import circle_hitsound_evidence as previous
from hitsound_resource_evidence import StaticStrings

METHODS = [0x0651,0x0653,0x0654,0x113e,0x142d,0x1431,0x143a,
 0x1449,0x144a,0x145c,0x1463,0x1464,0x1465,0x1466,0x1467,0x1469,
 0x146a,0x146b,0x146c,0x146d,0x146e,0x146f,0x1470,0x1471,0x1472,
 0x1478,0x17c2,0x1883,0x1884,0x1be6,0x0b73,0x226b,0x2295,
 0x24b3,0x24b4,0x24b9,0x24ba,0x24bb,0x24bd,0x24c4,0x249b,
 0x28a1,0x28d3,0x28e3,0x28e4,0x28f0,0x28f1,0x28f5,0x28f7,0x351a,
 0x351b,0x351d,0x45eb,0x45ec,0x48de,0x48e0,0x44a1,0x44a2,
 0x3b09,0x3b0e,0x3b14,0x3b15,0x3b18,0x3b19,0x3b20,0x3b31,
 0x3b32,0x3b33,0x3b35,0x3b36,0x3b40,0x3b41,0x3b42,0x3b53,
 0x3b68,0x3b6a,0x3b72,0x3b9b,0x3b9c,0x3ba1,0x3ba2,0x3ba8,0x3bb6,
 0x313a,0x313d,0x313e,0x3146,0x3151,0x3197,0x13fb,0x140b,0x3e4d]
FIELDS = [0x0bdf,0x0bee,0x0bf1,0x0bf2,0x0bf3,0x0bf4,0x0bf5,0x0bf6,
 0x0bf7,0x0bf8,0x0bf9,0x0bfa,0x0bfb,0x1534,0x1535,0x1536,0x1537,
 0x1538,0x1539,0x2526,0x2354,0x2355,0x2360,0x2361,0x2362,0x2363,
 0x2364,0x2368,0x2375,0x2376,0x239b]
CALLS = [0x143a,0x1469,0x146a,0x146b,0x3b33,0x3b35,0x3b53,0x24b3]
SLOTS = [0x24a9,0x24bb,0x24bd,0x24b2,0x24c3,0x24c4,0x24b3]
STRINGS = {2064529152,2064529198,2064529212,2064528831,2064528794,
 2064528790,2064529283,2064459678,2064529308,2064528980}

# Hand-derived conditional descriptions. No executable substitute slider/audio engine.
# F0bf4 has previously become -1 through a reached pre-start update unless varied.
CASES = [
 ('S01','E016 one-span S2000 E3500 times2500/3000/3464, tracking true, audio state1; reached updates2500,3000',
  'tick(0,query2502), tick(1,query3002); final schedule item2 is not scanned'),
 ('S02','Positive embedded head raw1024/512/256 in separate fresh controls',
  'one inherited circle hook each, before raw64 parent handoff; own start+5 timing'),
 ('S03','Embedded head stored raw-131072, wrapper returns-262144',
  'no M1884 positive sound hook; later tracking audio is independent'),
 ('S04','E016 gameplay success j0 at2500, tracking true, eligible M1449',
  'component raw8 first; later tick0; neither call is the other producer'),
 ('S05','E016 late entry2501 fails old deadline2500 but leaves tracking true',
  'raw-262144 can precede tick0 at query2502'),
 ('S06','Due checkpoint, follow false and pending count<N causes successful leave',
  'later M1449 tracking false suppresses discrete request and records due index'),
 ('S07','Tracking dispatch skipped; retained tracking true; M1449 reached at2500',
  'tick0 can be requested with no component consumption on that continuation'),
 ('S08','E016 jump to3100 with audio index-1, tracking true',
  'only tick1/query3002; tick0 skipped; audio index1'),
 ('S09','Repeat reached3100 without another due entry',
  'loop continuation remains reached; no new discrete request'),
 ('S10','E021 two spans times2500/3000/3500/4000/4500/4964; timely tracking at3500',
  'edge1/query3505; independent of whether component raw64 describes current item'),
 ('S11','E021 late entry3501; repeat component fails but tracking remains true',
  'edge1 can still be requested by M1449'),
 ('S12','E021 released at3500, tracking false by M1449',
  'no edge request; audio index2 consumed; reentry3501 does not replay edge1'),
 ('S13','E021 jump3500 from audio-1 while tracking true',
  'only edge1, not earlier ticks0/1'),
 ('S14','E021 jump4000 from audio-1 while tracking true',
  'only tick3/query4002, not missed repeat edge1'),
 ('S15','E021 stable4964 before parent end5000, audio index already4; final component succeeds or fails',
  'no new discrete M1449 sound; scan latest index4; gameplay128/-524288 is not an audio identity'),
 ('S16','Parent aggregation reached5000, q>0, edgeSounds length3',
  'cleanup queue then M1466(false edge2)/query5005, observer then standard layers'),
 ('S17','Parent aggregation q0 with head miss and no successes',
  'cleanup reached, positive edge hook skipped; no aggregate edge request'),
 ('S18','Parent q>0 from successful head alone, all components pending at end',
  'aggregate edge request still reached; pending components are not drained'),
 ('S19','Final component fails but aggregate q>0',
  'aggregate edge request can still occur despite final failure'),
 ('S20','Three spans, E016 finite length300/rate200/spacing100; timely tracked updates at each nonfinal deadline and completion6500',
  'times2500/3000/3500/4000/4500/5000/5500/6000/6464; tick0,tick1,edge1,tick3,tick4,edge2,tick6,tick7; aggregate edge3 at6500'),
 ('S21','R2 edgeSounds supplied[2,4,8], parent sound0, edgeSets1:2|2:3|3:1, version14',
  'head stored2/base1/add2; edge1 mask4/base2/add3; aggregate edge2 mask8/base3/add1'),
 ('S22','Missing or empty EdgeSounds; supplied EdgeSets otherwise valid',
  'simple mode true; parent sound/base/add are used; edge-set lists not selected'),
 ('S23','R2 nonempty short EdgeSounds[4] with parent sound2',
  'parser pads to[4,2,2]; short EdgeSets padded with0; excess EdgeSets trimmed'),
 ('S24','Malformed EdgeSounds element bad, otherwise nonempty R2 list',
  'TryParse result discarded; element0; constructor head normalizes bits2/4/8'),
 ('S25','EdgeSets entry lacks colon second item, or non-Int32 selector',
  'original index/conversion exception; row catch continues only when parser arg3 true; otherwise rethrows'),
 ('S26','Explicit edge mode, head edge base0, parent base3, timing base2',
  'version13 head chooses parent3; version14 chooses timing2; nonhead base0 inherits timing in both'),
 ('S27','Edge1 with timing index7 volume25, parent index9 volume90',
  'edge info index7 volume25; parent override fields are not read by M146a'),
 ('S28','Tick j0/querytimes0+2, base override0/addition3, timing base2 index7 volume0',
  'slidertick Soft index7 volume0; no layer expansion, observer or standard floor'),
 ('S29','No timing record; reached edge selector',
  'base defaults Normal1 if zero, index0 volume100; then M44a2/observer/standard'),
 ('S30','No timing record; reached tick selector',
  'unguarded timing dereference can throw; no guaranteed discrete submission'),
 ('S31','Timing change at tick deadline+2 versus edge boundary+5',
  'each query uses own prepared timestamp; full supplied-order timing rule applies; not hit timestamp'),
 ('S32','Parent custom filename accepted, resource returns valid handle or0',
  'parent custom field stored; head no filename/index/volume copy; parent discrete paths do not branch on custom handle'),
 ('S33','Same preparation with custom lookup-1/missing',
  'same direct standard/tick selectors; cache collision/provider/native effects remain conditional'),
 ('S34','Parent custom name with IndexOf(slider)>0 and IndexOf(tick)==-1',
  'M24b9 passes loop=true to M3b15; starts-at0 or tick substring fails this special loop predicate'),
 ('S35','Artificial parent ball X0/256/512; head own X128',
  'parent discrete pan-.4/0/.4; loop pan those values*.4; head pan-.2; native output unobserved'),
 ('S36','Dynamic tick key soft-slidertick7 cached-1 versus0 versus valid',
  '-1/miss falls back F2375[row0,bank1,plane0];0/valid retained'),
 ('S37','Eligible tracking update, standard/tick selected handle-1',
  'M3b31 returnsnull; no queued player; audio index was already advanced'),
 ('S38','Same with selected handle0',
  'player request can queue; later sample validity fails; do not equate0 with absent selection'),
 ('S39','Native/observer/resource/scheduler helper throws after audio index store0131',
  'index can remain advanced without a completed request; no local rollback'),
 ('S40','M1449 outside [start,end] or numeric audio state!=1',
  'audio index set-1; discrete and loop-start branch skipped; upstream pause may prevent whole update'),
 ('S41','Tracking false update consumes latest due index then reentry without new deadline',
  'no discrete replay; loop continuation can resume independently'),
 ('S42','Fresh constructor or M1431 reset, no pre-start update',
  'audio index0, not-1; direct first-index0 update does not pass latest>old gate'),
 ('S43','Rewind outside range then return into due range; update reached',
  'outside writes-1 and may allow repeated due sound; not proof of full native restart behavior'),
 ('S44','Tracking true in eligible range before next deadline',
  'M146b->M3b33 reached even with no discrete event; uses global loop channels'),
 ('S45','Version>=14 loop base/add overrides versus version<14',
  'newer forwards parent sets; older forwards zeros; current global bank/index and LayeredHitSounds still apply'),
 ('S46','Completion cleanup or inactive M1449 branch invokes M146c',
  'M3b53 queues M3ba8 native ChannelPause on global2354/2355; later eligible call can schedule starts again'),
 ('S47','Ordinary manager update, F136e false and native loop state1',
  'M0653 queues stop after candidate updates; callback/native timing unobserved'),
 ('S48','Standard edge mask14 at volume100, layering augmentation disabled/enabled with other gates satisfied',
  'disabled Finish100,Whistle85,Clap85; enabled Normal80 then same three layers; shared E037 request order'),
]

CASE_EVIDENCE = {
 'head':['M143a:0032-0059','M48e0:0055-0082','M351d:0001-0012','M1884:0076-0099','M2295:01a2-01ba'],
 'schedule':['M1449:00b8-0180,0216-022b','M146f:0262-0382','E016/E021/E022 producer controls'],
 'aggregate':['M28e4:005d-0079','M1465:0000-0052','M1466:0000-0028','M146a:00d7-01e9'],
 'metadata':['M28f7:166f-1688,17cd-1aa4,1cfa-1d03','M142d:009e-00a0,0131-0171','M48e0:001c-0082','M146a:0000-01e9'],
 'tick':['M1469:0013-0081','M3b35:0000-002b','M3b36:0004-0006,0082-008f,0133-015d,01c1-021e'],
 'resources':['M24b9:0000-006d','M3b15 (E038)','M3b31:0000-00c9','M3b36:01c1-021e','E038 provider/cache contract'],
 'loop':['M146b:0000-004e','M3b33:0000-00c0','M3b20:0000-0149','M3b9c/M3ba1/M3ba2/M3ba8','M0653:0000-0019'],
}

def case_evidence(n):
    i=int(n[1:])
    groups=(['head'] if i in [2,3] else ['aggregate'] if i in [16,17,18,19] else
            ['metadata'] if i in [21,22,23,24,25,26,27] else
            ['tick'] if i in [28,30,36] else
            ['resources'] if i in [32,33,34,37,38] else
            ['loop'] if i in [44,45,46,47] else ['schedule'])
    if i in [29,31,35,48]: groups+=['metadata','tick','head']
    sites=[site for group in groups for site in CASE_EVIDENCE[group]]
    if i==35:sites+=['M1467:0000-0022','M146b:0039-0049','M24bd (E037)']
    if i==42:sites+=['M1431:0018-001e','M142d fresh field defaults']
    if i==48:sites+=['M44a2/M3b32 (E037 exact augmentation and layer order/volumes)']
    if i==38:sites+=['M140b/M1407 (E037 sample validity)']
    return sites

def sha(data): return hashlib.sha256(data).hexdigest()

def extract(path):
    assert sha(Path(path).read_bytes()) == previous.ARCHIVE
    with zipfile.ZipFile(path) as archive: raw = archive.read('osu!.exe')
    assert sha(raw) == previous.EXE  # Must precede original instruction decoding.
    previous.METHODS = METHODS
    previous.FIELDS |= {0x4000000 + x for x in FIELDS}
    previous.CALLS |= {0x6000000 + x for x in CALLS}
    previous.SLOTS = SLOTS
    out = previous.extract(path)
    out['expedition'] = 39
    pe = dnfile.dnPE(data=raw)
    tables = pe.net.mdtables
    decoder = StaticStrings(pe)
    out['string_decoder'] = decoder.provenance
    out['strings'] = {str(n): decoder.decode(n) for n in sorted(STRINGS)}
    out['string_sites'] = []
    for rid in METHODS:
        method = tables.MethodDef.rows[rid-1]
        if not method.Rva: continue
        body = read_method_body_from_bytes(pe.get_data(method.Rva))
        target = out['methods'][hex(0x6000000+rid)]
        target['array_integer_sites'] = []
        instructions = list(body.instructions)
        for j, ins in enumerate(instructions):
            offset = ins.offset-body.header_size
            op = ins.opcode.name
            if op.startswith(('ldelem','stelem','newarr')) or op in ('ldlen','div','rem','xor'):
                target['array_integer_sites'].append([offset,op,getattr(ins.operand,'value',None)])
            if op == 'call' and getattr(ins.operand,'value',None) == 0x60049b0:
                prior = instructions[j-1]
                if prior.opcode.name == 'ldc.i4' and prior.operand in STRINGS:
                    out['string_sites'].append([hex(0x6000000+rid),offset,prior.operand])
    for token, field in out['fields'].items():
        field['name'] = str(tables.Field.rows[int(token,16)-0x4000001].Name)
    out['member_declaring_specs'] = {}
    for token in out['external_refs']:
        ref = tables.MemberRef.rows[int(token,16)-0xa000001]
        if ref.Class.table.name == 'TypeSpec':
            out['member_declaring_specs'][token] = ref.Class.row.Signature.value.hex()
    out['ancestry_method_declarations'] = {}
    for rid in [0x2af,0x339,0x4c3,0x828,0x6f6,0x94c,0x35a]:
        typ = tables.TypeDef.rows[rid-1]
        out['ancestry_method_declarations'][hex(0x2000000+rid)] = [
            {'token':hex(0x6000000+m.row_index),'name':str(m.row.Name),
             'signature':m.row.Signature.value.hex(),'virtual':m.row.Flags.mdVirtual,
             'new_slot':m.row.Flags.mdNewSlot} for m in typ.MethodList]
    out['method_impls'] = [
        {'class':hex(0x2000000+r.Class.row_index),
         'body':[r.MethodBody.table.name,r.MethodBody.row_index],
         'declaration':[r.MethodDeclaration.table.name,r.MethodDeclaration.row_index]}
        for r in tables.MethodImpl if r.Class.row_index in [0x2af,0x339,0x4c3,0x828,0x6f6,0x94c,0x35a]]
    out['embedded_slider_resources'] = []
    wanted = {f'{bank}-{kind}' for bank in ['normal','soft','drum']
              for kind in ['slidertick','sliderslide','sliderwhistle']}
    with zipfile.ZipFile(path) as archive:
        for name in ['osu!ui.dll','osu!gameplay.dll']:
            data = archive.read(name)
            dll = dnfile.dnPE(data=data)
            for resource in dll.net.resources:
                entries = {str(e.name):e for e in resource.data.entries}
                out['embedded_slider_resources'].append({
                    'assembly':name,'assembly_sha256':sha(data),'resource_set':str(resource.name),
                    'entry_count':len(entries),'keys':{key:(
                        {'type':entries[key].type_name,'bytes':len(entries[key].value),
                         'sha256':sha(entries[key].value)}
                        if key in entries and isinstance(entries[key].value,bytes) else None)
                        for key in sorted(wanted)}})
            dll.close()
    out['reference_cases'] = [{'id':n,'status':'RECONSTRUCTED STATIC PREDICTION',
        'initial_conditions':state,'conditional_request_sequence':prediction,
        'evidence':case_evidence(n),
        'unresolved':'Reached original gates; provider/native/scheduler/helper return and interleavings; no audible result'}
        for n,state,prediction in CASES]
    out['reference_case_common']={
        'domain':'Exact fresh ordinary parent/head types and valid finite E016/E021 prepared inputs; no original import measured',
        'history':'Reached pre-start M1449 has set audio index -1, except S42; no external mutation or clock change unless stated',
        'samples':'Parent raw sound0/selectors0 and absent edge arrays unless varied; supplied array entries0 unless specified; timing time0/set1/index1/volume100 unless varied',
        'runtime':'Eligible numeric audio state1; stable stated F2358 samples; nonnull skin/song; LayeredHitSounds false; song rate100 unless varied',
        'helpers':'Normal returning helpers and no observer/result mutation unless an interruption case says otherwise',
        'resources':'Symbolic supplied valid handles and noncolliding custom stems unless varied; no native loader result or audible output asserted',
        'pan':'Actual stored parent ball/head position remains a symbolic operand, except artificial X control S35',
        'artificial_controls':'S35 fixes ball X; S39 assumes interruption; S42 directly enters constructor/reset state without pre-start history; opaque handle/cache alternatives are conditional inputs, not measured native returns'}
    root=Path(__file__).resolve().parents[1]
    sources=['15/slider-entry.json','16/slider-components.json','17/slider-completion.json',
             '18/slider-break.json','21/first-repeat.json','22/terminal-checkpoint.json',
             '23/aggregate-policy.json','37/circle-hitsound.json','38/hitsound-resources.json']
    out['authority_inputs']={s:sha((root/'evidence'/s).read_bytes()) for s in sources}
    out['collector_sources']={s:sha((root/'tools'/s).read_bytes()) for s in [
        'slider_hitsound_evidence.py','circle_hitsound_evidence.py',
        'hitsound_resource_evidence.py','beatmap_init_evidence.py']}
    out['limits'] = [
        'CONFIRMED STATIC sites/metadata; conditional predictions are not original runtime measurements',
        'No original executable/CLR/native/audio method invoked; no audio bytes published',
        'Direct field/caller inventory excludes reflection, native/external mutation and aliases',
        'E037/E038 playback and provider boundaries remain authoritative; shared global loop state is not per-slider ownership',
        'Native continuation/frequency/overlap/pause and actual installed providers remain UNVERIFIED',
        'E020 all17 NOT RUN; historical862 independent checks not rerun by this static extractor']
    pe.close()
    return out

if __name__ == '__main__':
    result = json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2)+'\n'
    if len(sys.argv)>2: Path(sys.argv[2]).write_text(result,encoding='utf-8',newline='\n')
    else: print(result,end='')
