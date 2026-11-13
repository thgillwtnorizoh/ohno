#!/usr/bin/env python3
"""Assemble coverage/summary from executed results; never manufactures observations."""
import base64,gzip,hashlib,json
from collections import Counter
from pathlib import Path
QA=Path(__file__).resolve().parent
V=QA/'verification'
def read(name):return json.loads(gzip.decompress(base64.b64decode((V/name).read_bytes())))
def write(path,data):path.write_text(json.dumps(data,ensure_ascii=False,indent=2)+'\n',encoding='utf-8',newline='\n')
head=read('windows-headless-results.json.gz.b64');win=read('windows-widget-results.json.gz.b64');expected=read('expected.json.gz.b64');byid={x['id']:x for x in head['charts']}
rows=[]
for x in expected['rows']:
    a=byid[x['id']]['actual'];e=x['expected'];rows.append(dict(id=x['id'],kind=x['kind'],path=x.get('path'),expected=e['final'],actual=a['final'],double_absolute_difference=abs(e['final']['double']-a['final']['double']),single_bits_match=e['final']['single_bits_le']==a['final']['single_bits_le'],display_match=e['final']['display']==a['final']['display'],expected_aggregates=e['aggregates'],actual_aggregates=a['aggregates'],independent_control='numerical/'+x['id']))
write(V/'chart-comparisons.json',dict(authority='E046 independent oracle; expected.json.gz.b64',production='Actual C# source linked from reconstructed/CircleGame, no Work2 harness imported',rows=rows))
counts=dict(Counter(c['classification'] for p in [head,win] for c in p['cases']))
coverage=[]
def link(label,controls,evidence,limits='Finite component coverage only; no original runtime execution'):
    allcases={c['id']:c for p in [head,win] for c in p['cases']}
    assert controls and all(c in allcases and allcases[c]['passed'] for c in controls)
    coverage.append(dict(category=label,controls=controls,authority=evidence,classification='VERIFIED COMPONENT',limits=limits))
nums=[c['id'] for c in head['cases'] if c['id'].startswith('numerical/')]
for label,evidence in [
 ('Strict original-derived preparation and pair construction','E045/E046 Single geometry and admitted profiles'),
 ('Movement, jump and velocity-angle terms','E045 M11c1/M29f7/M408c; E046 movement_terms/composite'),
 ('Mutable rhythm groups, aliases, repetitions and history weights','E046 independent integer identity heap/events; M1744'),
 ('Dense backward/forward/angular visibility and following-pair contribution','E046 indexed visibility equations; M1e5e/M1e57/M1e58'),
 ('All three component recurrences and duration aggregation','E044/E046 independent offline duration sweep/dequeue; three state recurrences'),
 ('Final Double, exact cached Single bits, display flooring','E044/E043/E046 final/cache/display')]:link(label,nums,evidence)
for label,prefix,evidence in [
 ('Strict admission including unsupported object rows and profiles','admission/','R17 engineering admission vs E045/E046 finite raw profile'),
 ('Cache identity/reuse, cancellation and changed-file invalidation','cache/','R17 engineering cache lifecycle; actual production jobs'),
 ('Stale asynchronous results and generation isolation','selection/','R17 engineering controller; held actual jobs'),
 ('Unsupported charts remain playable but unrated','unsupported/','Actual gameplay parser vs restricted calculator'),
 ('Gameplay score/health/Rank state preservation','gameplay/','Matched production session snapshots and fresh-session creation')]:link(label,[c['id'] for c in head['cases'] if c['id'].startswith(prefix)],evidence)
link('Cooperative cancellation and bounded calculation work',['calculator/cooperative-cancel'],'Actual R17 cancellation token/work budget')
link('Unshown SelectPlay display binding, correct chart identity and restoration',[c['id'] for c in win['cases'] if c['id'].startswith('binding/')],'Actual WPF SelectPlayView/cache/controller; independent manifest expectations','Unshown WPF only; physical GUI/rendering/audio remain PENDING HUMAN TEST')
link('Actual unshown Rank Back and fresh Retry',['rank/back-to-rated-selection','rank/fresh-retry'],'Actual PlayfieldView host methods at Rank only','No active gameplay Frame/OS-input sampling; synthetic Rank control only')
coverage.append(dict(category='GameWindow dispatcher, selection, Play capture and disposal wiring',controls=[c['id'] for c in win['cases'] if c['classification']=='SOURCE INSPECTION'],classification='SOURCE INSPECTION',limits='No GameWindow constructed/launched; not human runtime evidence'))
manifest=json.loads((QA/'beatmaps/manifest.json').read_bytes())
write(QA/'coverage.json',dict(schema='QA15.coverage.v1',source=head['source'],historical_independent=2175,qa15_named=head['named']+win['named'],qa15_classifications=counts,combined_named_checkpoint=2175+head['named']+win['named'],counts_not_additional_tests=dict(raw_corpus_files=27,whole_numerical_traces=47,prepared_only=20,scalar_assertions=279369),rows=coverage,
    chart_categories=dict(two_circle=['QA15-01','QA15-25'],three_circle=['QA15-02','QA15-26'],four_circle_first_angle=['QA15-03','QA15-27'],regular=['QA15-23','QA15-24'],repeated=['QA15-22'],irregular=['QA15-12','QA15-13','QA15-21'],dense_visibility=['QA15-10'],short_preempt=['QA15-11'],mixed_angles=['QA15-17','QA15-20'],combined=['QA15-09'],temporal_history=['QA15-04','QA15-05','QA15-06','QA15-07','QA15-08','QA15-24']),
    exact_boundary_controls=[x['id'] for x in expected['rows'] if x['id'].startswith('boundary/')],
    limitations=['All original runtime, import, x86/JIT/library precision, real GUI/input/audio and owner acceptance remain unobserved','No general manager/provider/stacking or slider/spinner/mod difficulty qualification','Corpus fixture names alone do not establish exact boundary reachability','MaximumPending queue saturation and full 4096-circle performance are not newly independently qualified'],
    gates={'1_corpus':'PASS engineering/documented-format validation; original import unobserved','2_independent':'PASS finite components and exact-source Windows historical regression','3_human':'PENDING HUMAN TEST'}))
print('Assembled QA15 coverage',counts,'comparison rows',len(rows))
