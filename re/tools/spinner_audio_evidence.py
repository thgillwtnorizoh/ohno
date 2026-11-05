#!/usr/bin/env python3
"""E041 pinned original-static extraction. Never load original CLR/native/audio code.

Selected metadata/instruction relationships only; no original sample bytes exported.
Uses unchanged E004/E037 structural collector and E038 mathematical string decoder.
"""
import hashlib,json,sys,zipfile
from pathlib import Path
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
import circle_hitsound_evidence as prior
from hitsound_resource_evidence import StaticStrings

METHODS=sorted(set(prior.METHODS+[0x05ea,0x05eb,0x05ee,0x05ef,0x05f4,0x05f5,0x05f6,0x05f7,
 0x05f8,0x05f9,0x05fa,0x05fb,0x05fc,0x05fd,0x0604,0x0653,0x0ac5,0x0acb,
 0x114a,0x1872,0x1d75,0x2262,0x2275,0x24c3,0x24b2,0x24a9,0x24c4,0x24aa,0x24ab,
 0x28c5,0x28d3,0x28e3,0x28e4,0x28f5,0x28f6,0x3b0e,0x3b3c,0x3b1a,0x3b1b,0x3b1c,
 0x3b1d,0x3b1e,0x3b1f,0x3b11,0x3b28,0x3b2b,0x3b34,0x3b40,0x3b41,
 0x3b42,0x3b49,0x3b54,0x3b56,0x3b57,0x3b5e,0x3b61,0x3b65,0x3b66,
 0x3b68,0x3b6a,0x3b87,0x3ba3,0x3ba9,0x3bac,0x3bae,0x3121,0x3122,0x313a,
 0x313d,0x3151,0x3154,0x3196,0x48e1,0x534e,0x5353]))
FIELDS=[0x2356,0x2354,0x2355,0x2360,0x2361,0x2367,0x2368,0x36af,0x36b0,
 0x02b4,0x029e,0x02a2,0x02a0,0x02af,0x02b0,0x02b3,0x2358,0x2a94,0x2a90,
 0x0cc6,0x1527,0x1534,0x1536,0x1537,0x1538,0x1539,0x2523,0x2524,0x2526,
 0x234b,0x234e,0x234f,0x2350,0x2351,0x23a1,0x23a7,0x2393]
CALLS=[0x05fb,0x05fc,0x3b28,0x3b34,0x3b54,0x3ba3,0x3ba9,0x3b2b,0x3b87,
 0x24c4,0x3202,0x3b1f,0x3b11]
STRINGS=[2064529249,2064529271,2064456445,2064631985,2064631948,2064631963,
 2064456412,2064529892,2064528980]

COMMON={
 'domain':'Exact ordinary T00d8->T067f->T04c3->T0828, not Taiko. Fresh modern source policy unless varied; S2000/E5000/N15/C0/cache0/P0; valid original presentation/manager/map/candidates; reached application Play2/Osu0 gates.',
 'global':'F2356=101, separate from slider pair201/202; global timing current point set1/index1/volume100; effect/universal100/100; normally returning nonnull skin/scheduler/song; SpinnerFrequencyModulate=true; plain modifier bits0.',
 'resources':'Symbolic spinnerspin sample11, spinnerbonus sample22, standard final table handles31/32/34/38, custom-1; no native success assumed. Provider/preparation cases override prerequisites explicitly.',
 'native':'Length query returns signed Int64 8193 unless varied. Acquisition returns supplied channel101; start action and cleanup execute only when explicitly stated. Native activity/results/validity are conditional inputs, never measured.',
 'ordering':'Input M05f6 observes old P and dispatches before later M05f5 integration/loop request; manager completion follows. Every fresh field/helper/clock read remains separate.',
 'status':'RECONSTRUCTED STATIC PREDICTION; no original/runtime/native/audio observation'}
CASES=[]
def C(state,path,mutation,actions,native,evidence):
 CASES.append({'id':f'A{len(CASES)+1:02}','status':'RECONSTRUCTED STATIC PREDICTION','initial_overrides':state,'reached_path':path,'ordered_managed_transitions':mutation,'queued_actions':actions,'conditional_native_operations':native,'evidence':evidence,'unresolved':'COMMON prerequisites; actual scheduler/interleaving, installed providers, native return values, CLR exceptional conversion and audible output. Artificial inputs are not natural observations.'})
F=['M05f6:02d4-04cd','M05fb:0000-0030','M3b28:0000-0037']
L=['M05f5:0143-0173,0352-0395','M3b34','M3ba3','M05fc/M3b54/M3ba9']
R=['M3b19:015a-017a','M3b1f:009d-0110','M3b14/E038']
E=['M05fa','M3202:000b-002f','M24ba/M44a2/M3b32/M3b31']

C('T1999 before start; normally selected ordinary current predicate','M22c5;M05f5','No ordinary current input; update returns before loop operations','none','none from these routes',L)
C('Every relevant T2000 exactly start; current input reached','M05f6 then M05f5','Input may observe old P; strict integration/loop region skipped','none from update; input bonus separate','Frequency only if input detects a changed integer',F+L)
C('S<T<E,positive interval,post-smoothing U0','M05f5:0361 -> cleanup','Presentation rotation can precede cleanup; P store follows','QPause=M3ba9','On callback Pause(fresh2356); no Stop/free',L)
for sign in ['positive','negative']:
 C(f'S<T<E,positive interval,post-smoothing U {sign} nonzero','M05f5->M05fb->M3b34','Loop helper reached before P accumulation; direction does not select a different resource','QStart=M3ba3 after optional synchronous frequency request','With COMMON C0/N15 and modulation true:Length query and attr1=20000 when>8192 before enqueue;on callback attr2(fresh timing gain),Play(fresh2356,false)',L)
C('Two reached interior updates at unchanged T,positive supplied intervals,U nonzero','M05f5 twice','No native-activity/pending-start suppression in M3b34','QStart,QStart','Conditional callback Play(false) twice; not two audible restarts',L)
C('Interior update interval0 or negative','M05f5:0173','Skip smoothing/loop/zero-motion cleanup region; later display processing separate','none here','No loop request from skipped region',L)
C('T=end exactly with U nonzero','M05f5:014e;M28e4:008f','Loop region and whole completion both skipped','none from these paths','Prior queued operations remain independent',L+E)
for label,p,c,cache in [('first',1,0,0),('second',2,1,1),('jump',7,0,0)]:
 C(f'{label} converted P{p},old C{c},cache{cache};ordinary sample prefix passes','M05f6 inequality','C increases once; modulate fresh C/N before result; later cache reread','none from frequency; no bonus in stated controls',f'Length(2356,mode0); if>8192 set attr1 from ratio; returned component {8192 if c==1 else 0}',F)
C('P integer equals cached integer,even if motion/target changed','M05f6:02e0 equality','Earlier angular mutations retained; C unchanged','none from this observation','No M3b28 or bonus here; later update may queue loop',F+L)
C('C becomes N+3=18','M05f6:0320','Bonus equality rejected; ordinary parity8192','none from bonus','Optional frequency remains reached first',F)
C('C becomes N+4=19 (odd offset1)','M05f6:0336','Bonus rejected; ordinary4096','none from bonus','Optional frequency first',F)
C('C becomes N+5=20;plain mods','M05f6 bonus branch','Optional presentation flash; raw16384 chosen; bonus request before later label/transforms/cache','bonus player action M3b87','Load spinnerbonus; volumeM3b42/100f; callback Play(true)',F+['M3b2b/M3b87/M140b'])
C('Same qualifying bonus with F0cc6 bit4194304 set','M05f6:0378','Raw16384 retained; ancillary sample request skipped; presentation/cache continue','no bonus action','No bonus sample load here; frequency independent',F)
C('SpinnerFrequencyModulate=false,changed integer then interior U nonzero','M05f6;M05fb','No observation modulation; M05fb passes Nullable absent','QStart still submitted','No M3b28 from either gate; callback attr2/Play(false)',F+L)
for ratio,freq in [('0','20000'),('0.5','40000'),('1','60000'),('2','100000'),('3','100000'),('-1','-20000')]:
 C('Artificial direct M3b28 ratio'+ratio+';length8193','M3b28','No channel validity/activity/lower-bound test','none','SetAttribute(current2356,1,'+freq+'); native result discarded',F)
for ratio in ['0/0 NaN','positive/0 +Infinity','negative/0 -Infinity','out-of-Int32 product']:
 C('Artificial ratio '+ratio+';length8193','M3b28','Single multiplication then original unchecked conv.i4; no finite guard','none','Keep k=original conv.i4 output symbolic; attr1=Single(Min(100000,unchecked(20000+k))); no measured CLR output',F)
for length in [-1,0,8191,8192,8193]:
 C(f'Native signed length result{length};ratio1','M3121->M3122;M3b28','Signed <=8192 skips; otherwise branch reaches update','none','Length(fresh2356,0); '+('no attr1' if length<=8192 else 'attr1=60000'),F)
C('F2356=0 or-1 artificial;length return supplied','M3b28','No managed nonzero/activity guard','none','Query invalid handle; branch solely on supplied signed length; no guaranteed native success',F)
C('Channel101 changes301 during length query','M3b28:0000 versus0012','Length target captured before query; attribute target reread afterward','none','Length101 then qualifying attr1 on301',F)
C('Changed integer increments C;native/helper throws before cache','M05f6:02ed->M3b28','C increment retained; cache stale; no normal result dispatch','no new bonus action reached','Later same P can be counted again if method retried',F)
C('U already stored;M05fb frequency helper throws','M05f5:0364','Earlier U/presentation mutations survive; P store0395 not reached','no QStart if exception before M3b34 enqueue','No rollback',L)
C('Start queued at101/V40;channel301/V80 before execution','M3ba3','No parent/channel/gain snapshot; target read before gain; Play target fresh again','existing QStart','Attribute301,2,.8 then Play(current2356,false)',L)
C('Channel changes301->401 during start attribute write','M3ba3:0011/0017','Attribute and Play use different fresh reads','existing QStart','Attribute301 then Play401,false',L)
C('Changed global sample bank or index;application1/2','M3b1f','Update selectors; Stop old nonzero active1; acquire spinnerspin(false);store2356;attributes11 then2','none in replacement','Loader(loop=true,unprefixed=true,stages7);GetChannel(sample,false)',R)
C('Unchanged selectors,forcefalse','M3b1f:0015','Return before spinner replacement','none','No spinner Activity/Stop/Acquire/attribute from refresh',R)
C('Changed selectors,application0,forcefalse','M3b1f:0017-0027','Return before selector/channel writes','none','No replacement here',R)
C('Forced refresh unchanged selectors/application0','M3b1f:0001','Force bypasses early gates; spinner replacement still performed','none','Active1 old gets Stop; Acquire(false); no per-spinner owner',R)
C('Previous spinner channel nonzero but native active0/2/3','M3b1f:00af','No Stop; acquire/store replacement independently','none','No equivalence between inactive/paused/stopped assumed',R)
C('Map spinnerspin.wav present;global index0 or17/base3','M3b14 unprefixed true','No bank prefix/index suffix;same keyspinnerspin;shared cache consulted first','none from load','WAV->MP3->OGG;loop flag131072|4',R)
C('Map misses;skin admitted/default resource supplied','M3b14','Mapcache then unsuffixed skin cache then UI/gameplay key;sample bytes select loader','none','Keep source-specific0 vs-1 behavior',R)
C('spinnerspin unresolved-1 or map/skin0;Acquire returns0','M3b1f','No one-shot sample-validity guard; store acquired0;attributes still attempted','later QStart may still submit','Acquire(-1 or0,false);attr11/2;QStart may Play0,false',R+L)
C('spinnerbonus resource unresolved-1 or0','M3b2b->M13fb','Player still created/registered/queued;sample validity tested by M140b','M3b87','Invalid sample sets player flag,skips native Play;no standard fallback',F+['M3b2b/M3b87/M1407/M140b'])
C('First map bytes nonnull but backend returns-1','M3b14','Stop extension probing;fall through provider stages;do not retry codec after load','none','Source native load result remains opaque',R)
for count,raw in [(16,1024),(14,512),(3,256),(2,-131072)]:
 C(f'Modern N15/C{count};strict after-end manager gate passes','M05fa','Cleanup first; evaluated true before grade;'+('positive hook before later comparisons' if raw>0 else 'minimum failure skips hook'),'QPause then '+('ordered completion sample actions if resources' if raw>0 else 'no final hitsound action'),f'Return raw{raw};queued Pause and playback are distinct',E+L)
C('Modern N0/C0 artificial prepared control','M05fa:007d','Cleanup/evaluated;positive hook;return1024 independent of C','QPause then final sample actions','Final hook timing stored end',E)
C('Final timing list null/empty','M3202:0019','Positive hook returns without sample selection;grade can continue','cleanup only','No final playback request from this hook',E)
C('Timing change at E,another at E+1 or E+5','M3202/M1be6','Query Double(storedE),inclusive scan-all;later qualifying supplied entry atE wins','final requests selected fromE point','No start+5/end+5/hit-time substitution',E)
C('Final rawmask14/layeringfalse/custom-1/volume100','M24ba/M3b32','Observer then Finish100->Whistle85->Clap85;no unconditional Normal','three one-shot actions','Inherited pan from centerX256 gives0',E)
C('Final rawmask0 or qualifying layered14','M44a2/M3b32','Normal added;standard order1/4/2/8;floor8 precedes attenuation','ordered standard actions','Normal*.8;Finish1;Whistle/Clap*.85',E)
C('Object base3/addition1/index2/volume25/custom7317','M3202/M24ba','Object nonzero selectors win;observer still runs;custom!= -1 bypasses standard','one custom M3bb6 action','Volume25 no standard floor;pan0;matchRate true/factor initial1',E)
C('Object custom0 versus-1','M24ba','0 selects custom arm; -1 selects standard;validity later','custom0 action or standard layers','Do not convert0 into standard fallback',E)
C('Completion QPause submitted101,then global channel301','M05fc->M3b54->M3ba9','No capture/clear/ownership check','QPause','At execution Pause301;not submitted101',L)
C('Manager cleanup/reset or reached pause-on candidates include spinner','M28c5/M0acb/M2262->virtualM24c4','Queue cleanup before later dispose/song toggle;cursor/count resets independent','QPause for reached spinner','Native Pause only when callback executes;no implicit Stop/free',L+['M28c5:0050','M2262:01da','M0acb:003c'])
C('Later spinner starts before prior cleanup executes','M3b1f or reused channel;M3b34','Same global2356;prior queued cleanup can affect later channel;no owner token','QPause/QStart in reached submission order','Conditional execution may Pause current later channel;native outcome unresolved',L+R)
C('Mapcache invalidation or device reinitialization branch reached','M28f6/M3b1d/M3b1b/M3b5e','SampleFree/cacheclear distinct from global2356;no direct zero store','Old actions not proven cancelled','Native frees do not prove handle-field reset',R)
C('Settings event with spinner channel active','M3b11','Compute one gain local;write slider0/slider1/spinner2356 using it','none from this handler','Spinner attr2 uses same captured gain;target2356 read after earlier writes',R+['M3b11:0018-003f'])
C('Observer in final M24ba throws','M28a1 before custom/standard','Evaluated remains true;cleanup already submitted;classification/handoff interrupted','earlier QPause retained','No guaranteed final sample request or grade return',E)
C('Bonus queued successfully,then later label/transform helper throws','M05f6:0390-04c7','C advanced;cache may stay stale;one-shot already registered/queued;no normal component return','bonus action survives logically','No transactional cancellation;retry could request another bonus',F)

def sha(b):return hashlib.sha256(b).hexdigest()
def extract(path):
 assert sha(Path(path).read_bytes())==prior.ARCHIVE
 with zipfile.ZipFile(path)as z:raw=z.read('osu!.exe')
 assert sha(raw)==prior.EXE
 prior.METHODS=METHODS;prior.FIELDS|={0x4000000+f for f in FIELDS};prior.CALLS|={0x6000000+m for m in CALLS}
 prior.SLOTS=sorted(set(prior.SLOTS+[0x24c3,0x24b2,0x24a9,0x24c4]))
 out=prior.extract(path);out['expedition']=41
 pe=dnfile.dnPE(data=raw);ts=pe.net.mdtables;decoder=StaticStrings(pe)
 out['strings']={str(n):decoder.decode(n) for n in STRINGS};out['string_decoder']=decoder.provenance;out['string_sites']=[]
 for token,field in out['fields'].items():
  row=ts.Field.rows[(int(token,16)&0xffffff)-1];field.update(name=str(row.Name),static=row.Flags.fdStatic,has_default=row.Flags.fdHasDefault,has_rva=row.Flags.fdHasFieldRVA)
 for rid in METHODS:
  m=ts.MethodDef.rows[rid-1]
  if not m.Rva:continue
  b=read_method_body_from_bytes(pe.get_data(m.Rva));ins=list(b.instructions);row=out['methods'][hex(0x6000000+rid)]
  row['integer_sites']=[[i.offset-b.header_size,i.opcode.name] for i in ins if i.opcode.name in ['conv.i8','rem','div','add','mul','conv.i4','conv.r4','conv.r8']]
  for j,i in enumerate(ins):
   if i.opcode.name=='call' and getattr(i.operand,'value',0)==0x60049b0 and ins[j-1].opcode.name=='ldc.i4' and ins[j-1].operand in STRINGS:out['string_sites'].append([hex(0x6000000+rid),i.offset-b.header_size,ins[j-1].operand])
 out['channel_access_inventory']=[x for x in out['field_sites'] if x[3]=='0x4002356']
 out['spinner_type_declarations']={hex(0x2000000+rid):[{'token':hex(0x6000000+m.row_index),'name':str(m.row.Name),'signature':m.row.Signature.value.hex(),'virtual':m.row.Flags.mdVirtual,'new_slot':m.row.Flags.mdNewSlot} for m in ts.TypeDef.rows[rid-1].MethodList] for rid in [0xd8,0x67f,0x4c3,0x828]}
 out['method_impls']=[{'class':hex(0x2000000+x.Class.row_index),'body':[x.MethodBody.table.name,x.MethodBody.row_index],'declaration':[x.MethodDeclaration.table.name,x.MethodDeclaration.row_index]} for x in ts.MethodImpl if x.Class.row_index in [0xd8,0x67f,0x4c3,0x828]]
 out['embedded_spinner_resources']=[]
 with zipfile.ZipFile(path)as z:
  for name in ['osu!ui.dll','osu!gameplay.dll']:
   data=z.read(name);dll=dnfile.dnPE(data=data)
   for resource in dll.net.resources:
    entries={str(e.name):e for e in resource.data.entries}
    out['embedded_spinner_resources'].append({'assembly':name,'assembly_sha256':sha(data),'resource_set':str(resource.name),'entry_count':len(entries),'keys':{key:({'type':entries[key].type_name,'bytes':len(entries[key].value),'sha256':sha(entries[key].value)} if key in entries and isinstance(entries[key].value,bytes) else None)for key in ['spinnerspin','spinnerbonus']}})
   dll.close()
 root=Path(__file__).resolve().parents[1]
 out['authority_inputs']={s:sha((root/'evidence'/s).read_bytes())for s in ['24/spinner-entry.json','25/spinner-result.json','26/spinner-completion.json','37/circle-hitsound.json','38/hitsound-resources.json','39/slider-hitsounds.json','40/slider-continuous-audio.json']}
 out['collector_sources']={s:sha((root/'tools'/s).read_bytes())for s in ['spinner_audio_evidence.py','circle_hitsound_evidence.py','hitsound_resource_evidence.py','beatmap_init_evidence.py']}
 out['reference_case_common']=COMMON;out['reference_cases']=CASES
 out['limits']=['CONFIRMED STATIC instructions/metadata; cases RECONSTRUCTED STATIC PREDICTION, never original runtime','No original CLR/native/game/audio execution; no proprietary sample bytes exported','Direct managed inventory excludes reflection/native/external mutation/aliases','CIL floating exceptional conversion remains symbolic; no measured JIT/native frequency or audible pitch','Global timing/resources/channel and queued callbacks are shared; no invented per-spinner ownership or cancellation','Historical1344 independent QA checks are not rerun by static tools;E02017 remain NOT RUN']
 pe.close();return out
if __name__=='__main__':Path(sys.argv[2]).write_text(json.dumps(extract(sys.argv[1]),sort_keys=True,indent=2)+'\n',encoding='utf-8',newline='\n')
