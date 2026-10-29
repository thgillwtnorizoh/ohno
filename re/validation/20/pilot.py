#!/usr/bin/env python3
"""Prepare private reference copies and check evidence structure; never runs gameplay.
Python3.10+ standard library. No HP/judgement/timing model or game instrumentation.
"""
import argparse
import hashlib
import importlib.util
import json
import os
from pathlib import Path, PurePosixPath, PureWindowsPath
import re
import shutil
import tempfile
import wave
import zipfile

PACKAGE=Path(__file__).resolve().parent
REPO=PACKAGE.parents[2]
NOT_RUN='NOT RUN — WINDOWS HOST REQUIRED'
def load(p):return json.loads(Path(p).read_text(encoding='utf-8-sig'))
def write(p,x):Path(p).write_text(json.dumps(x,sort_keys=True,indent=2,allow_nan=False)+'\n',encoding='utf-8')
def sha(p):
    h=hashlib.sha256()
    with Path(p).open('rb')as f:
        for b in iter(lambda:f.read(1024*1024),b''):h.update(b)
    return h.hexdigest()
def require(ok,message):
    if not ok:raise ValueError(message)
def contained(root,relative):
    rel=PurePosixPath(relative)
    require(not rel.is_absolute()and '..'not in rel.parts and '\\'not in relative
      and ':'not in relative and not PureWindowsPath(relative).drive,'unsafe relative path')
    p=(root/relative).resolve();require(p.is_relative_to(root.resolve()),'path escapes root')
    return p
def prepare(archive,output,run_id):
    pins=load(PACKAGE/'inputs.json');archive=archive.resolve();output=output.resolve()
    require(re.fullmatch(r'[A-Za-z0-9_-]+',run_id)is not None,'invalid run ID')
    require(not output.is_relative_to(REPO)and not archive.is_relative_to(output),'private output must be outside checkout and must not contain source archive')
    require(not output.exists(),'fresh output directory required')
    require(sha(archive)==pins['archive_sha256'],'wrong reference archive hash')
    for spec in pins['files'].values():require(sha(REPO/spec['repo_path'])==spec['sha256'],'changed pinned input')
    with zipfile.ZipFile(archive)as z:
        require(z.testzip()is None,'ZIP CRC failure')
        require(hashlib.sha256(z.read('osu!.exe')).hexdigest()==pins['exe_sha256'],'wrong executable')
        for item in z.infolist():
            contained(output/'game',item.filename)
            require((item.external_attr>>16)&0o170000 !=0o120000,'ZIP symlink refused')
        output.mkdir(parents=True);game=output/'game';game.mkdir()
        baseline=[]
        for item in z.infolist():
            dest=contained(game,item.filename)
            if item.is_dir():dest.mkdir(parents=True,exist_ok=True);continue
            dest.parent.mkdir(parents=True,exist_ok=True)
            with z.open(item)as src,dest.open('xb')as dst:shutil.copyfileobj(src,dst)
            baseline.append({'relative_path':dest.relative_to(output).as_posix(),'sha256':sha(dest),'role':'original_archive_member'})
    generator=REPO/pins['files']['audio_generator']['repo_path']
    spec=importlib.util.spec_from_file_location('e01_fixture_generator',generator)
    mod=importlib.util.module_from_spec(spec);spec.loader.exec_module(mod)
    with tempfile.TemporaryDirectory(prefix='ohno-e20-audio-')as temp:
        mod.generate(Path(temp));audio=Path(temp)/'silence.wav'
        require(sha(audio)==pins['audio']['sha256'],'audio generator hash mismatch')
        with wave.open(str(audio),'rb')as w:
            require((w.getnchannels(),w.getsampwidth(),w.getframerate(),w.getnframes())==(1,2,44100,264600),'unexpected WAV header')
            require(not any(w.readframes(w.getnframes())),'control WAV is not silence')
        inputs={}
        for key in ['slider','circle']:
            source=REPO/pins['files'][key]['repo_path']
            require(re.search(r'^AudioFilename: silence\.wav$',source.read_text(),re.M)is not None,'AudioFilename mismatch')
            song=game/'Songs'/('E20-'+key);song.mkdir(parents=True)
            dest=song/source.name;shutil.copyfile(source,dest);shutil.copyfile(audio,song/'silence.wav')
            inputs[key]={'map':dest.relative_to(output).as_posix(),'map_sha256':sha(dest),
              'audio':(song/'silence.wav').relative_to(output).as_posix(),'audio_sha256':sha(song/'silence.wav')}
    (output/'private_artifacts').mkdir()
    write(output/'baseline-files.json',baseline)
    manifest=load(PACKAGE/'manifest.template.json');manifest['run_id']=run_id
    manifest['reference']={'archive_sha256':pins['archive_sha256'],'exe_sha256':pins['exe_sha256'],'identity':pins['identity']}
    manifest['input_hashes']=inputs
    write(output/'manifest.json',manifest)
    shutil.copyfile(PACKAGE/'environment.manual.template.json',output/'environment.manual.json')
    probes=load(PACKAGE/'probes.json')
    write(output/'bindings.template.json',{'status':'UNBOUND — WINDOWS DEBUGGER REQUIRED','bindings':[
      {'binding_id':x['probe_id'],'probe_id':x['probe_id'],'method_token':x['method_token'],
       'expected_body_sha256':x['body_sha256'],'module_path':None,'module_sha256':None,
       'module_mvid':None,'process_id':None,'runtime_type':None,'jit_code_version':None,
       'native_address_and_il_map':None,'actual_binding_verified':False}for x in probes['probes']]})
    return {'preparation':'COPIES_PREPARED','runtime_status':NOT_RUN,'gameplay_validation_completed':False,'original_execution_performed':False}

def check_event(e,run_id,known_probes):
    schema=load(PACKAGE/'trace.schema.json');require(set(e)==set(schema['required']),'event keys do not match schema')
    require(e['run_id']==run_id,'event/run mismatch')
    for key in ['seq','thread_id','thread_seq']:require(type(e[key])is int and e[key]>=(1 if key=='thread_id'else 0),'invalid integer '+key)
    for key in ['run_id','case_id','probe_id','binding_id']:require(isinstance(e[key],str)and bool(e[key]),'empty '+key)
    require(re.fullmatch(r'[0-9]+',e['qpc_ticks']or '')is not None,'bad QPC text')
    require(re.fullmatch(r'0x060[0-9a-fA-F]{5}',e['method_token']or '')is not None,'bad method token')
    require(re.fullmatch(r'0x[0-9a-fA-F]+',e['cil_offset']or '')is not None,'bad CIL offset')
    for key in ['stage','observation_mode']:require(e[key]in schema['properties'][key]['enum'],'bad '+key)
    p=known_probes.get(e['probe_id']);require(p is not None and p['method_token']==e['method_token'],'unknown/mismatched probe')
    require(int(e['cil_offset'],16)<p['code_bytes'],'offset outside method')
    for key in ['branch','exception']:require(e[key]is None or isinstance(e[key],str),'bad '+key)
    require(isinstance(e['values'],list),'values must be a list')
    value_schema=schema['properties']['values']['items']
    for v in e['values']:
        require(set(v)==set(value_schema['required']),'operand keys do not match schema')
        require(isinstance(v['name'],str)and bool(v['name']),'bad operand name')
        for key in ['value_type','capture_kind']:require(v[key]in value_schema['properties'][key]['enum'],'bad operand '+key)
        for key in ['text','bits_hex','reference_id','unavailable_reason']:require(v[key]is None or isinstance(v[key],str),'bad operand '+key)
        if v['capture_kind']=='unavailable' or v['value_type']=='Unknown':
            require(bool(v['unavailable_reason']),'unavailable operand needs reason');continue
        t=v['value_type'];text=v['text'];require(text is not None,'captured operand needs original text')
        if t in ['Int32','UInt16','EnumInt32']:
            require(re.fullmatch(r'-?[0-9]+',text)is not None,'invalid integer text');n=int(text)
            require((0<=n<=65535)if t=='UInt16'else(-2147483648<=n<=2147483647),'integer outside declared type')
        if t in ['Single','Double']:require(re.fullmatch(r'[0-9a-fA-F]{'+('8'if t=='Single'else'16')+'}',v['bits_hex']or '')is not None,'exact float bits required')
        if t=='Boolean':require(text in ['true','false'],'invalid Boolean')
        if t=='Reference':require(text=='null'or bool(v['reference_id']),'nonnull reference needs process-local identity')
        if t in ['Int32List','BooleanArray']:
            a=json.loads(text);require(isinstance(a,list),'list operand required')
            require(all(type(x)is bool for x in a)if t=='BooleanArray'else all(type(x)is int and -2147483648<=x<=2147483647 for x in a),'list element types')

def seal(run):
    m=load(run/'manifest.json');old={a['relative_path']:a for a in m['artifacts']};artifacts=[]
    for p in sorted((run/'private_artifacts').rglob('*')):
        require(not p.is_symlink(),'artifact symlinks refused')
        if not p.is_file():continue
        rel=p.relative_to(run).as_posix();a=old.get(rel,{})
        artifacts.append({'artifact_id':a.get('artifact_id','a-'+hashlib.sha256(rel.encode()).hexdigest()[:16]),
          'relative_path':rel,'sha256':sha(p),'bytes':p.stat().st_size,
          'kind':a.get('kind','trace'if p.suffix=='.jsonl'else'UNCLASSIFIED')})
    m['artifacts']=artifacts;write(run/'manifest.json',m)
    return {'artifact_hashes_recorded':len(artifacts),'runtime_acceptance':'NOT_EVALUATED'}

def validate(run):
    m=load(run/'manifest.json');require(m['gameplay_validation_completed']is False,'this structural tool cannot certify gameplay completion')
    require(m['kind']in ['ORIGINAL_REFERENCE_PILOT','SCHEMA_SELFTEST'],'unknown manifest kind')
    pins=load(PACKAGE/'inputs.json');require(m['reference']['exe_sha256']==pins['exe_sha256']and m['reference']['archive_sha256']==pins['archive_sha256'],'reference mismatch')
    known={p['probe_id']:p for p in load(PACKAGE/'probes.json')['probes']};ids=set();artifacts_by_id={};events={};threads={};prev=-1
    for a in m['artifacts']:
        require(a['artifact_id']not in ids,'duplicate artifact ID');ids.add(a['artifact_id']);artifacts_by_id[a['artifact_id']]=a
        p=contained(run,a['relative_path']);require(p.is_file()and sha(p)==a['sha256']and p.stat().st_size==a['bytes'],'artifact hash/size mismatch')
        if a['kind']!='trace':continue
        for line in p.read_text(encoding='utf-8-sig').splitlines():
            require(bool(line.strip()),'empty trace record');e=json.loads(line);check_event(e,m['run_id'],known)
            require(e['seq']>prev,'collector sequence must increase across trace files');prev=e['seq']
            require(e['thread_seq']>threads.get(e['thread_id'],-1),'per-thread sequence must increase');threads[e['thread_id']]=e['thread_seq'];events[e['seq']]=e
    case_ids=[x['case_id']for x in load(PACKAGE/'test-matrix.json')['cases']]
    require([x['case_id']for x in m['case_results']]==case_ids,'case matrix mismatch')
    require(all(e['case_id']in case_ids for e in events.values()),'unknown event case ID')
    observed=0
    for c in m['case_results']:
        require(all(a in ids for a in c['evidence_ids']),'missing case artifact')
        if c['execution_status']==NOT_RUN:
            require(c['actual_observations']is None and c['comparison']=='NOT_RUN'and not c['evidence_ids'],'unrun case must not claim observations');continue
        require(c['execution_status']in ['OBSERVED_PENDING_REVIEW','BLOCKED_DURING_WINDOWS_RUN'],'unknown execution status')
        require(c['comparison']in ['MATCH','MISMATCH','INCONCLUSIVE'],'comparison required')
        require(isinstance(c['actual_observations'],dict)and bool(c['evidence_ids']),'observed/blocker case needs structured linked evidence')
        obs=c['actual_observations'];require(bool(obs.get('summary'))and isinstance(obs.get('event_seqs'),list),'summary/event_seqs required')
        for seq in obs['event_seqs']:
            require(type(seq)is int and seq in events and events[seq]['case_id']==c['case_id'],'missing/mismatched observed event')
            require(events[seq]['observation_mode']==c['observation_mode'],'case/event observation-mode mismatch')
        observed+=1
    if m['execution_status']==NOT_RUN:
        require(not observed and m['windows_original_execution_attested']is False and not events,'unrun manifest cannot contain runtime records')
    else:
        require(m['execution_status']in ['OBSERVED_PENDING_REVIEW','BLOCKED_DURING_WINDOWS_RUN'],'unknown run status')
        require('Windows'in (m['environment']['os']or ''),'actual Windows environment required')
        require(m['kind']!='SCHEMA_SELFTEST','schema selftest can never attest original execution')
        if m['execution_status']=='OBSERVED_PENDING_REVIEW':
            require(m['windows_original_execution_attested']is True and bool(events),'original observations require attestation and trace artifacts')
            require(type(m['environment']['qpc_frequency'])is int and m['environment']['qpc_frequency']>0,'QPC frequency required')
            require(bool(m['environment']['clr_actual'])and bool(m['environment']['process_architecture']),'actual process CLR/architecture required')
            require(not any(e['observation_mode']=='collector_selftest'for e in events.values()),'selftest records are not runtime evidence')
            aid=m['instrumentation']['binding_artifact_id'];require(aid in artifacts_by_id,'verified binding artifact required')
            bindings=load(contained(run,artifacts_by_id[aid]['relative_path']))['bindings'];byid={b['binding_id']:b for b in bindings}
            for event in events.values():
                b=byid.get(event['binding_id']);require(b is not None,'missing runtime binding')
                require(b['actual_binding_verified']is True and b['module_sha256']==pins['exe_sha256']and b['module_mvid']==pins['exe_module_mvid'],'unverified/mismatched module binding')
                require(b['method_token']==event['method_token']and b['probe_id']==event['probe_id']and bool(b['native_address_and_il_map'])and bool(b['jit_code_version']),'incomplete runtime binding')
                require(type(b['process_id'])is int and b['process_id']>0,'binding process ID required')
    return {'structure':'VALID','runtime_acceptance':'NOT_RUN'if m['execution_status']==NOT_RUN else'MANUAL_REVIEW_REQUIRED',
      'gameplay_validation_completed':False,'trace_records':len(events),'declared_observed_or_blocked_cases':observed,
      'warning':'Hashes and declared provenance cannot authenticate original execution or prove a static prediction.'}

def main():
    p=argparse.ArgumentParser();s=p.add_subparsers(dest='command',required=True)
    a=s.add_parser('prepare');a.add_argument('--archive',type=Path,required=True);a.add_argument('--output',type=Path,required=True);a.add_argument('--run-id',required=True)
    for name in ['seal','validate']:
        a=s.add_parser(name);a.add_argument('--run',type=Path,required=True)
    a=p.parse_args()
    if a.command=='prepare':r=prepare(a.archive,a.output,a.run_id)
    elif a.command=='seal':r=seal(a.run.resolve())
    else:r=validate(a.run.resolve())
    print(json.dumps(r,sort_keys=True,allow_nan=False))
if __name__=='__main__':main()
