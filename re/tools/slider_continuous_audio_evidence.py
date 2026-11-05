#!/usr/bin/env python3
"""E040 deterministic original-static evidence, never original CLR/native execution.

Uses pinned dnfile0.18.0/dncil1.0.2/pefile2024.8.26 and unchanged E004/E037-E039
collectors. Publishes selected instruction relationships/metadata, not audio bytes.
"""
import hashlib
import json
from pathlib import Path
import sys
import zipfile
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import slider_hitsound_evidence as prior

METHODS = [0x0651,0x0653,0x0b73,0x0b74,0x0b75,0x0b7e,0x113e,0x142d,0x1431,
 0x1449,0x1465,0x1466,0x1467,0x1469,0x146b,0x146c,0x146d,0x146e,0x146f,
 0x1884,0x1be6,0x1bc1,0x2237,0x2246,0x2248,0x2249,0x224d,0x2262,0x226b,
 0x2275,0x22c5,0x24bb,0x24bd,0x24c4,0x28be,0x28e3,0x28e4,0x28f5,0x28f6,
 0x2914,0x3b08,0x3b09,0x3b0e,0x3b0f,0x3b11,0x3b14,0x3b18,0x3b19,
 0x3b1b,0x3b1c,0x3b1d,0x3b1e,0x3b1f,0x3b20,0x3b21,0x3b25,0x3b33,0x3b36,
 0x3b3c,0x3b3d,0x3b40,0x3b41,0x3b42,0x3b44,0x3b45,0x3b47,0x3b49,0x3b50,
 0x3b53,0x3b56,0x3b57,0x3b5e,0x3b61,0x3b65,0x3b66,0x3b67,0x3b68,0x3b69,
 0x3b6a,0x3b72,0x3b73,0x3b81,0x3b82,0x3b83,0x3b84,0x3b89,0x3b8a,0x3b9b,
 0x3b9c,0x3b9d,0x3b9e,0x3ba1,0x3ba2,0x3ba8,0x3bac,0x3bad,0x3bae,0x3baf,
 0x2fee,0x2ffd,0x30b1,0x30bf,0x30c6,0x36b3,0x313a,0x313d,0x313e,0x3146,
 0x3151,0x3154,0x3196,0x3197,0x47ca,0x47cb,0x38e1,0x38e2,0x3df2,0x48de,0x48e0]
FIELDS = [0x2354,0x2355,0x2360,0x2361,0x2362,0x2363,0x2364,0x2367,0x2368,
 0x236b,0x236c,0x236e,0x238e,0x238f,0x2391,0x2392,0x239b,0x234b,0x2376,
 0x136e,0x100f,0x1000,0x0661,0x0662,0x0663,0x0665,0x0666]
CALLS = [0x3b66,0x3b65,0x3b61,0x3b1f,0x3b20,0x3b33,0x3b53,0x3b09,0x3b11,
 0x3b3d,0x3b73,0x3b82,0x3b83,0x3b19,0x3b18,0x3b1b,0x3b1d,0x3b1e,0x146b,
 0x146c,0x3ba1,0x3ba2,0x3ba8,0x3b9c,0x3151,0x313d,0x3196,0x3154]

# Hand-derived conditional controls, not an executable replacement state machine.
# Every row inherits COMMON; replacements are explicit artificial operands.
COMMON = {
 'managed':'Fresh exact ordinary T02af path, map format14, current song-map == selected map; valid finite lists and presentation; application F2c6d=2/Osu0; audio state F2368=1; stored tracking F0bf3=true; parent start2000/end3500; time2400; mask0/base0/addition0; LayeredHitSounds=false; parent X256.',
 'global':'Timing list [time0,base1,index1,volume100,uninherited=true]; F238e=F238f=0; F2360=F2362=1; F2361=F2363=F2364=1; channels F2354=101,F2355=202; effect/universal settings100/100; valid prepared loop table samples11/22; empty action submission history.',
 'slider_fields':'Head.F1527=true, head.F152a=1024; parent.F1527=false; F0bf1=F0bf5=0; F0bf4=-1 after reached pre-start update; F0bf8=[2500,3000,3464], F0bf7=[], F2527=1; F0bf9 absent/simple mode; parent F1539=F1536=0,F1534=-1; F136e=true. Pure helper controls leave unrelated fields unconsumed. Overrides replace these values.',
 'prepared_resources':'Every reached F2376 row0/bank/plane cell=11 and row1/bank/plane cell=22 unless overridden; F234e has no dynamic entries unless overridden. Source/provider controls specify alternatives; no installed resource/native-load result is presumed. Other discrete tables are unresolved unless their separate E039 prerequisites are supplied.',
 'native':'ChannelIsActive(101/202)=1 unless varied. Native returned handles/results are conditional supplied operands, not measured or decoded outcomes. No concurrent field/settings/clock changes unless varied.',
 'helpers':'Reached gates, nonnull settings/skin/scheduler and normally returning callbacks/helpers unless varied; table/provider preparation already completed unless a preparation case.',
 'actions':'Q0=M3ba1, Q1=M3ba2, QPan=M3b9c(captured Nullable<Single>), QPause=M3ba8. Submission order does not assert execution order. At execution callbacks read actual current global handles and gain state separately.',
 'status':'RECONSTRUCTED STATIC PREDICTION; no original/runtime/native/audio observation'}

CASES = []
def C(n,state,native,path,mutations,actions,calls,evidence):
    CASES.append({'id':n,'status':'RECONSTRUCTED STATIC PREDICTION',
      'initial_managed_overrides':state,'conditional_native_operands':native,'reached_path':path,
      'ordered_managed_mutations':mutations,'enqueued_actions':actions,
      'conditional_native_calls':calls,'evidence':evidence,
      'unresolved':'Original scheduler/native results/interleavings, installed resources and audible outcome. COMMON prerequisites apply; artificial changes are not natural observations.'})

TIMING=['M3b65:0000-000c','M3b66:0006-0134','M3b89/M3b8a','M3b61:0000-002f','M3b1f:0000-0123','M3b3c','M47ca/M47cb']
CHANNEL=['M3b20:0000-0149','M3b33:0000-00c0','M3ba1/M3ba2/M3b9c','M146b:0000-004e']
RESOURCE=['M3b18:0067-008c','M3b19:00dd-0116,0281-02a4','M3b36:0034-009d,015f-021e','M3b14/E038']
LIFE=['M146c/M1431/M1465','M3b53/M3ba8','M0653/M2275','M2262:00bf-00ec,01bf-0211','M3b49']

C('L01','tracking true before first due tick','101/202 state1','M1449->M146b->M3b33',
 'No bank/index/channel change; audio index can remain unchanged.','QPan(0) only',
 'No new Play; deferred attribute3 on both execution-time handles.',CHANNEL+['M1449:0113-0131'])
C('L02','reached first due tick2500, audio index-1','101/202 state1','M1449->M146b; store0; M1469(true,0)->M146b; dedicated tick',
 'First continuing call precedes audio index store; second follows it.','QPan(0),QPan(0), then separate discrete tick action if sample branch succeeds',
 'Continuing pan twice; discrete tick remains E039 mode1.',CHANNEL+['M1449:011e-015c','M1469:0000-0013'])
C('L03','repeat reached2500, F0bf4=0 (no new due index)','101 state0;202 state1; prior Q0 not executed','M1449->M146b',
 'No channel replacement; no pending-start flag is stored.','Q0,QPan on each reached call',
 'Each Q0 if executed sets attribute2 then Play(actual F2354,true).',CHANNEL)
C('L04','tracking false with latest due0','both state1','M1449 false arm->M146c',
 'Queue cleanup before store audio index0; no channel fields cleared.','QPause',
 'At execution Pause(F2354), then separately Pause(F2355).',LIFE+['M1449:0216-021e'])
C('L05','tracking false; channels0/0','native returns opaque','M1449->M146c',
 'No validity or activity guard in cleanup.','QPause',
 'Pause(0) then Pause(0), results discarded; not evidence of success.',LIFE)
C('L06','reentry after QPause executed; audio index unchanged','101/202 state!=1','M1449->M146b',
 'No discrete replay required for continuing start.','Q0,QPan (mask0); Q1 only if Whistle bit set',
 'Play(global0,true) after live gain computation.',CHANNEL+LIFE)
C('L07','mask2, layeredfalse','both state0','M3b33 after unchanged M3b20',
 'No row0 request, no row0 stop or pause.','Q1,QPan',
 'Play global2355; pan writes both channels.',CHANNEL)
C('L08','mask2, layeredtrue','both state0','M3b33',
 'Both rows eligible; neither channels owner nor pending flag stored.','Q0,Q1,QPan',
 'Two independent Play(channel,true) actions, not native-overlap observation.',CHANNEL)
C('L09','mask4 or8, layeredfalse','both state0','M3b33',
 'Lack of Whistle2 selects row0 even without Normal1 bit.','Q0,QPan',
 'No Finish/Clap continuing resource is selected.',CHANNEL)
C('L10','mask changes0->2 while unchanged channels both active1','both state1','M3b33',
 'No replacement and no per-row off operation.','QPan only',
 'Previous row0 is not explicitly stopped by the new mask.',CHANNEL)
C('L11','parent format13, base3/addition2, globalbank1','old channels state1;new GetChannel returns301/402 if needed','M146b',
 'Passes base0/addition0, resolving to global1/base1; parent sets ignored.','QPan if unchanged; eligibility still mask dependent',
 'No forced replacement just because ignored parent overrides differ.',CHANNEL)
C('L12','parent format14, base3/addition0','old101/202 state1; GetChannel returns301/402','M146b->M3b20',
 'base3,addition3; changed both; set F2362=1; Stop101; bank3;channel301;Stop202;addition3;channel402.','QPan only if new channels report1; otherwise eligible starts precede it',
 'Resolve Normal1/Drum/index1/mode2 then Whistle2/Drum/index1/mode2; GetChannel(sample,false).',CHANNEL)
C('L13','base0,addition3; globalbank2; old selected banks1/1','old state1; GetChannel301/402','M3b20(0,3,false)',
 'Resolve base2/addition3; retain separate changed flags, replace both.','None in M3b20 itself',
 'Stop each nonzero old handle only if its fresh active result==1.',CHANNEL)
C('L14','F2360=7,F2362=1; bank values unchanged','GetChannel301/402','M3b20',
 'Both changed flags true from index; F2362=7 before row replacement.','None until caller resumes',
 'Resolve two mode2 dynamic keys; replace both regardless eventual mask.',CHANNEL+RESOURCE)
C('L15','only base bank differs; addition fixed1; index unchanged','row0 old state2; GetChannel301','M3b20(3,1,false)',
 'Replace row0, preserve row1; old non-state1 row0 not explicitly stopped.','None in helper',
 'GetChannel(newSample,false); attributes11/2 still applied to both channels after gate.',CHANNEL)
C('L16','changed bank; old channel0','native Active0 not requested in replacement guard; GetChannel301','M3b20',
 'Skip old nonzero guard then assign new bank/channel.','None',
 'No Stop(0) in replacement; later unconditional attribute writes remain.',CHANNEL)
C('L17','identical selectors, forcefalse; application2','both state1','M3b20',
 'Return before F2362 store, replacement or attribute update.','None from helper; caller may still QPan',
 'No native gain refresh from helper in unchanged case.',CHANNEL)
C('L18','changed selectors; application0; forcefalse','native unused','M3b20',
 'Return without storing new selectors/channels.','M3b33 can still queue eligible starts/pan on old channels',
 'Do not treat helper return as caller return.',CHANNEL)
C('L19','identical selectors; forcetrue (artificial direct helper)','native opaque','M3b20',
 'Store F2362 and write attributes despite no bank change; do not GetChannel when both changed flags false.','None',
 'SetAttribute pair11=1 and2=current gain.',CHANNEL)
C('L20','mode2 set2/index7; map cache soft-sliderslide7=707','native no lookup required','M3b36(Normal1,Soft2,7,2)',
 'No mutation or file load.','None',
 'Return707; whistle key is soft-sliderwhistle7, not hitwhistle7.',RESOURCE)
C('L21','mode2 dynamic key missing or cached-1; plane0 handle17','native unused','M3b36',
 'No dynamic cache insertion at request time.','None',
 'Return corresponding F2376[row,bank,0]=17.',RESOURCE)
C('L22','mode2 dynamic key cached0; plane0 handle17','GetChannel(0,false) supplied return0','M3b36->M3b20',
 'Keep0, assign channel0; no M3b31 validity gate.','M3b33 may Q0 if Active0!=1',
 'SetAttribute/Play0 can be reached; opaque native success.',RESOURCE+CHANNEL)
C('L23','mode2 selected sample-1 (plane0 or fallback)','GetChannel(-1,false) supplied return0','M3b20',
 'Store returned0, not an absent-player managed branch.','Conditional eligible start/pan',
 'Native GetChannel(-1,false) reached; unlike M3b31 sample-1 early return.',CHANNEL+RESOURCE)
C('L24','prepare planes0/1/2 and banks1/2/3','native loader returns symbolic handles','M3b19',
 'Store F2376[0,bank,plane] then[1,bank,plane]; loop true; reset selected index/last banks at tail; do not clear channel pair.','None specific to loops',
 'M3b14 flags131072|4; standard hierarchy; index0 map suppression;1 unsuffixed;2 suffix2.',RESOURCE)
C('L25','preload dynamic index7','native load opaque','M3b18(7)',
 'F2360=7; cycle F2361=1,2,3; calls for sliderslide/sliderwhistle with looptrue.','None',
 'Map WAV->MP3->OGG; caches may stop lookup before load; loop flag not part of cache key.',RESOURCE)
C('L26','global current point volume0,7,8,9 (separate finite controls)','all helpers normal','M3b42->M3b41; start callback',
 'Selected V respectively8,8,8,9; settings100/100.','As pre-existing Q0 or Q1 executes',
 'Start attribute2 respectively approximately.08,.08,.08,.09 with original Single order; no .8/.85 attenuation.',CHANNEL+['M3b40/M3b41/M3b42'])
C('L27','no song receiver/map','native opaque','M3b42',
 'Return100 immediately; no timing read.','None by getter',
 'Gain consumers can use100; does not establish the whole slider path is reachable with null map.', ['M3b0e','M3b42:0000-0009'])
C('L28','song map exists; F238f=-1; map.F1000=0 artificial','native opaque','M3b68->M47cb then M3b42',
 'Current point null; return map fallback0 with no Max8 on fallback.','None by getter',
 'Gain can be0, not repaired to floor8.',TIMING+['M3b42:0010-002a'])
C('L29','effect80,universal50,current volume25','native setters normal','M3b40/M3b41->Q0',
 'Integer product4000; G40; M3b41=10; final attribute2=.1 under stated finite operands.','Existing Q0 only',
 'SetAttribute(F2354,2,.1), rereadF2354,Play(handle,true).',CHANNEL)
C('L30','only current timing volume changes40->80; same banks/index; channelsstate1','native state1','M3b61->M3b1f;M146b->M3b33',
 'Current point advances; no selector change; both refresh helpers return; no new start.','QPan only from reached slider',
 'No fresh gain write through these calls; later start or settings event reads new volume.',TIMING+CHANNEL)
C('L31','pan parentX0/256/512; format14','native normal','M1467->M146b->M3b33',
 'Parent pan approximately-.4/0/.4; captured continuing operand approximately-.16/0/.16.','QPan(captured operand)',
 'Attribute3 on both execution-time globals; no clamp in this continuing path.',CHANNEL)
C('L32','nullable pan absent, artificial direct M3b33','both state1','M3b33',
 'No channel change.','No pan action; eligible starts still independent',
 'No attribute3 from this call.',CHANNEL)
C('L33','queuedQ0, then F2354 changes101->301 and timingV40->80 before execution','native normal','M3ba1',
 'Callback carries no parent/channel/volume snapshot.','Previously queued Q0',
 'Attribute2 target301/gain.8; separately reread301 for Play(true).',CHANNEL)
C('L34','queuedQPause when101/202, then globals301/402','native normal','M3ba8',
 'No captured target handles; no fields cleared.','Previously queued QPause',
 'Pause301 then402; not the submitted-time pair.',LIFE)
C('L35','trackingfalse cleanup submitted, then reentry; native still!=1 at start check','native normal','M146c;M146b',
 'Original submission order pause then eligible start; no cancellation token.','QPause,Q0,QPan; row1 additionally ifmask2',
 'If callbacks execute in that order on stable pair: Pause both then Play row0(true). Actual execution order remains external.',LIFE+CHANNEL)
C('L36','same submissions but native state1 at reentry eligibility test','native normal','M146c;M146b',
 'Pending pause is not consulted by start policy.','QPause,QPan; no Q0 while native==1',
 'Later pause can take effect without a matching queued restart; next reached eligible update may retry.',LIFE+CHANNEL)
C('L37','positive aggregate at exactend with pending components','native states and queue execution supplied','M1465 cleanup->M1466->M1469(false,edge)',
 'Queue cleanup; ratio positive; fresh time<=end reaches continuing helper again before discrete edge.','QPause, then conditional Q0/Q1/QPan, then separate edge-layer requests',
 'No pending component drain; later helper can interrupt edge submission.',LIFE+CHANNEL+['M1465:0000-0052','M1469:0000-0013'])
C('L38','same aggregate afterend; fresh T=3501','native normal','M1469 fresh T>end',
 'Skip its continuing helper; edge selector still reached.','QPause then discrete edge requests; no new continuing action from M1469',
 'No prediction of audible terminal result.',LIFE+['M1469:0000-0013'])
C('L39','two slider producers A/B, unchanged banks/index, panA=.1/panB=-.1','both active1','A.M146b;B.M146b',
 'Neither stores an object owner.','QPan(.1),QPan(-.1)',
 'Both write same current pair; outcome depends on execution order.',CHANNEL)
C('L40','A usesbank1; B usesbank3 with sameindex','GetChannel301/402;old active1','A.M146b;B.M146b',
 'B replaces pair globally; A cleanup or queued callback has no ownership check.','Conditional A/B starts/pans; later A QPause targets current pair',
 'Stop old active pair at replacement; callbacks may address B-created pair.',CHANNEL+LIFE)
C('L41','manager F136e=false after candidate updates','M3b09 sees either global native state1','M0653',
 'Base M28e3 completes first; no owner comparison.','QPause',
 'Cleanup callback uses then-current channels.',LIFE)
C('L42','manager F136e=true, or both native states!=1','native as stated','M0653',
 'No guard-induced cleanup; false guard queries2355 first and short-circuits on1.','None from manager tail',
 'No universal post-update Stop; E039 S47 producer is queued Pause.',LIFE+['M3b09'])
C('L43','ordinary M2275 selects first passing slider','native irrelevant','M2275->Find(M22c5)',
 'F1380 selected from prior candidates; F136e=(selected!=null&&typecontainsSlider2); F136atrue substitutesnull.','None directly',
 'No channel ownership assignment; candidate order/other gameplay gates remain external.',LIFE+['M2275:0000-0056','M22c5'])
C('L44','fresh type initialization versus slider reset','native opaque','M3b08; separately M1431',
 'Type init: channels CLR0, timing indexes-1, selected banks0. Slider reset queuescleanup before head reset/audioIndex0; does not zero globals.','Reset QPause',
 'Reset callback may target later replacement handles.',LIFE+['M3b08'])
C('L45','F238e<0, q1, list[time0 U, time2505 inherited], T2500','native irrelevant','M3b66(false) full rescan',
 'query2505; strict predicates exclude equality; F238e=0,F238f=0; changed events only.','None itself (subscriber may act)',
 'M3b61 then reads point0 sample bank/index.',TIMING)
C('L46','same list/T but initialized indexes0/0,q1','native irrelevant','M3b66(false) forward arm',
 'entry2505 not greater than query: F238f=1; F238e stays0 for inherited point.','Timing delegate event can run; no automatic sound queue in timing method',
 'Later M3b61 consumes inherited record bank/index; volume getter sees its volume.',TIMING)
C('L47','list suppliedunsorted [time0,time3000,time2000], current indexes0/0,T2500','native irrelevant','M3b66(false)',
 'Forward arm stops at first future3000; never inspects later2000.','None unless earlier delegate',
 'Not per-request scan-all selection.',TIMING)
C('L48','M3b66(true), suppliedunsorted times or equal timestamps','native irrelevant','M3b66',
 'Calls original Sort before full FindLastIndex; CompareTo orders by Double time then timing flag. Equal same-flag ordering not invented.','Changed-index delegate events as reached',
 'Framework sort behavior not replaced with stable sort.',TIMING+['M0b7e'])
C('L49','no timing list, empty list, or subscriber/helper throws inside timing try','native irrelevant','M3b66',
 'Reset both timing indexes-1 and F238b=-999. Earlier sort/field writes/subscriber mutations not rolled back.','Any earlier actions remain queued',
 'Sample index/bank/channel fields are not reset by M3b65.',TIMING)
C('L50','separate fresh timing reads F238f=-1 or1 for listCount1, or F2367=null','native irrelevant','M3b68->M3b6a->M47cb/M47ca',
 'Null list returnsnull; negative/outofrange index returns defaultnull. No last/first fallback in getter.','None',
 'M3b69 separately falls back to index0; M3b3c falls backF236b.',TIMING)
C('L51','format<=3 or current point absent; F236b=1 supplied','native normal','M3b3c;M3b61',
 'Sample index getter returnsF236b; modern selected-map gate can skip bank refresh entirely.','Conditional later slider actions use existing globalstate',
 'No guessed modern timing defaults.',TIMING+['M1bc1:0015-001b,021f-0230,0702-0718'])
C('L52','null manager/map/skin/presentation or scheduler; reached dereference','native not guaranteed reached','M146b/M3b20/M3b33',
 'No local catch/rollback in continuing helpers; already stored index/bank can persist.','Earlier submitted actions remain possible; later discrete call may not be reached',
 'Native failures returningfalse are popped; managed exceptions remain distinct.',CHANNEL+['M1449:011e,0131','M1469:000e'])
C('L53','actual pause-on branch reached after M2262 gates','native opaque','M2262 togglesF1373 true; enumerates current candidates',
 'Each candidate virtualcleanup; ordinary slider queuespause. Then optional song toggle; M3b56 concerns scheduled-player collection, not direct channel pair.','OneQPause per reached slider plus distinct song/player actions',
 'No synchronous loop Pause at M2262 enqueue site; resume does not directly Play loop pair.',LIFE)
C('L54','map resource reload or device reset branch reached','native SampleFree/BASS_Free results opaque','M28f6->M3b1d; or M3b5e:F238a arm',
 'Free dictionary sample handles and clear caches; device arm also BASS_Free; no zero assignment to F2354/F2355. M3b19 resets selectors but not channels.','Existing queued actions carry no map generation',
 'Later callbacks can reread stale/replaced native handles; full shutdown/device lifecycle remains external.',RESOURCE+['M3b1b/M3b1c/M3b1d','M3b5e:00c1-00db,0263-0297'])
C('L55','settings event while channelsactive1','native normal','M3b11',
 'Recompute gain using current timing and live settings without bank/index replacement.','No queue in this handler',
 'Synchronous attribute2 on2354 then2355 (and separate2356/player updates).',CHANNEL+['M3b11:0000-003f','M3b21:003b-008c'])
C('L56','channel replacement helper throws after F2362 store or bank store','native/helper throws before GetChannel returns','M3b20',
 'F2362 and possibly F2363 updated first; old handle may already be stopped; no rollback/clear guaranteed.','No later starts/pan from interruptedM3b33',
 'Next retry depends on surviving comparison fields, not assumed transactional repair.',CHANNEL)

def sha(b): return hashlib.sha256(b).hexdigest()

def extract(path):
    assert sha(Path(path).read_bytes()) == prior.previous.ARCHIVE
    with zipfile.ZipFile(path) as z: raw=z.read('osu!.exe')
    assert sha(raw) == prior.previous.EXE
    prior.METHODS = sorted(set(METHODS))
    prior.FIELDS = sorted(set(prior.FIELDS+FIELDS))
    prior.CALLS = sorted(set(prior.CALLS+CALLS))
    out = prior.extract(path)
    out['expedition']=40
    pe=dnfile.dnPE(data=raw);ts=pe.net.mdtables
    for rid in METHODS:
        method=ts.MethodDef.rows[rid-1]
        out['methods'][hex(0x6000000+rid)]['comparison_sites']=[] if not method.Rva else [
          [i.offset-b.header_size,i.opcode.name]
          for b in [read_method_body_from_bytes(pe.get_data(method.Rva))]
          for i in b.instructions if i.opcode.name in ['clt','clt.un','cgt','cgt.un','ceq']]
    out['method_spec_resolutions']={}
    for rid,definition in [(0x18d,0x47cb),(0x1b0,0x47ca)]:
        r=ts.MethodSpec.rows[rid-1]
        out['method_spec_resolutions'][hex(0x2b000000+rid)]={
          'base_table':r.Method.table.name,'base_rid':r.Method.row_index,
          'instantiation':r.Instantiation.value.hex(),'local_definition':hex(0x6000000+definition),
          'base_signature':r.Method.row.Signature.value.hex()}
        assert r.Method.row.Signature.value==ts.MethodDef.rows[definition-1].Signature.value
        assert str(r.Method.row.Name)==str(ts.MethodDef.rows[definition-1].Name)
    for token,f in out['fields'].items():
        row=ts.Field.rows[(int(token,16)&0xffffff)-1]
        f['static']=row.Flags.fdStatic
    out['channel_access_inventory']=[x for x in out['field_sites'] if x[3] in ['0x4002354','0x4002355']]
    out['reference_case_common']=COMMON
    out['reference_cases']=CASES
    root=Path(__file__).resolve().parents[1]
    for name in ['05/slider-timing.json','10/gameplay-clock.json','39/slider-hitsounds.json']:
        out['authority_inputs'][name]=sha((root/'evidence'/name).read_bytes())
    out['collector_sources']['slider_continuous_audio_evidence.py']=sha(Path(__file__).read_bytes())
    out['limits']=[
      'CONFIRMED STATIC metadata/instructions; hand-derived cases are RECONSTRUCTED STATIC PREDICTIONS, not original execution',
      'Pinned source only; archive and executable hashes checked before decoding; no original CLR/native/game/audio calls',
      'Complete direct managed EXE field inventory excludes reflection, external/native mutation and indirect aliases',
      'Queue submission order is recovered; callback interleaving/native outcomes/output not observed',
      'Global song timing state differs from M1be6 per-object query; no invented per-slider channel owner or rollback',
      'No audio byte contents exported; original embedded metadata only; owner-supplied independent loop assets required',
      'Full device/shutdown/seek/alternate-manager reachability and original runtime remain UNVERIFIED',
      'Historical1091 checkpoint includes four source audits; no QA rerun claimed; E02017 remain NOT RUN']
    pe.close()
    return out

if __name__=='__main__':
    result=json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2)+'\n'
    Path(sys.argv[2]).write_text(result,encoding='utf-8',newline='\n')
