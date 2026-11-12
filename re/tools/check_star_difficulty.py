#!/usr/bin/env python3
"""Independent E044 original-byte relationships and finite math controls.

Does not import the E044 extractor. Original bytes are parsed as data only.
Reference arithmetic here uses independent closed forms/Decimal where useful;
the separate research projection is not an end-to-end chart calculator.
"""
import hashlib
import json
import math
from pathlib import Path
import struct
import sys
import zipfile
from decimal import Decimal, localcontext

ARCHIVE='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
EXE='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
def sha(x):return hashlib.sha256(x).hexdigest()
def canon(v):
    if isinstance(v,float) and not math.isfinite(v):return 'NaN' if math.isnan(v) else '+Infinity' if v>0 else '-Infinity'
    if isinstance(v,list):return [canon(x) for x in v]
    if isinstance(v,tuple):return [canon(x) for x in v]
    if isinstance(v,dict):return {str(k):canon(x) for k,x in v.items()}
    return v

def audit(d,path):
    import dnfile
    from dncil.cil.body.reader import read_method_body_from_bytes
    ab=Path(path).read_bytes();assert sha(ab)==d['archive_sha256']==ARCHIVE
    with zipfile.ZipFile(path) as z:raw=z.read('osu!.exe')
    assert sha(raw)==d['exe_sha256']==EXE
    pe=dnfile.dnPE(data=raw);ts=pe.net.mdtables
    own={m.row_index:n for n,t in enumerate(ts.TypeDef,1) for m in t.MethodList}
    fo={f.row_index:n for n,t in enumerate(ts.TypeDef,1) for f in t.FieldList}
    instructions={};count=0;scan=0;fields=[];calls=[];edges=[]
    watched={int(x,16) for x in d['watched_fields']};chosen={int(x,16) for x in d['methods']}
    mathops={'add','sub','mul','div','div.un','rem','neg','and','or','xor','ceq','cgt','clt','cgt.un','clt.un','shl','shr','shr.un'}
    for n,m in enumerate(ts.MethodDef,1):
        if not m.Rva:continue
        b=read_method_body_from_bytes(pe.get_data(m.Rva));scan+=1;tok=0x6000000+n
        tokens=[];constants=[];branches=[];numeric=[];ops={}
        boundaries={i.offset-b.header_size for i in b.instructions}
        for i in b.instructions:
            o=i.offset-b.header_size;op=i.opcode.name;v=getattr(i.operand,'value',None);ops[o]=(op,v if v is not None else i.operand)
            if v is not None:
                tokens.append([o,op,hex(v)])
                if v in watched:fields.append([hex(tok),o,op,hex(v)])
                if v in chosen:calls.append([hex(tok),o,op,hex(v)])
            if op.startswith('ldc.'):constants.append([o,op,canon(i.operand)])
            if op=='switch' or op.startswith(('brtrue','brfalse','beq','bne','bge','bgt','ble','blt','leave')) or op in ['br','br.s']:
                dest=[x-b.header_size for x in i.operand] if op=='switch' else [i.operand-b.header_size]
                assert all(x in boundaries for x in dest)
                branches.append([o,op,dest])
            if op in mathops or op.startswith('conv.'):numeric.append([o,op])
        if tok not in chosen:continue
        r=d['methods'][hex(tok)];assert sha(pe.get_data(m.Rva,b.size))==r['body_sha256']
        assert (r['virtual'],r['new_slot'],r['abstract'])==(m.Flags.mdVirtual,m.Flags.mdNewSlot,m.Flags.mdAbstract)
        assert r['signature']==m.Signature.value.hex() and r['owner']==hex(0x2000000+own[n])
        assert (tokens,constants,branches,numeric)==(r['token_sites'],r['constants'],r['branches'],r['numeric_sites']),hex(tok)
        instructions[n]=ops;count+=1
        for i in b.instructions:
            v=getattr(i.operand,'value',None);op=i.opcode.name
            if not v or op not in ['call','callvirt','newobj','ldftn','ldvirtftn']:continue
            original=v
            if v>>24==0x2b:
                s=ts.MethodSpec.rows[(v&0xffffff)-1]
                v={'MethodDef':0x6000000,'MemberRef':0xa000000}[s.Method.table.name]+s.Method.row_index
            edges.append([hex(tok),i.offset-b.header_size,op,hex(original),hex(v)])
    assert scan==19659 and d['scan']=={'decoded':19659,'zero_rva':1739,'errors':[]}
    assert fields==d['field_sites'] and calls==d['direct_callers']
    assert edges==d['call_graph']
    for t,r in d['type_inventory'].items():
        n=int(t,16)&0xffffff;q=ts.TypeDef.rows[n-1]
        assert r['fields']==[hex(0x4000000+x.row_index) for x in q.FieldList]
        assert r['name']==str(q.TypeNamespace)+'.'+str(q.TypeName)
        if q.Extends and q.Extends.row:
            assert r['base']==hex({'TypeDef':0x2000000,'TypeRef':0x1000000,'TypeSpec':0x1b000000}[q.Extends.table.name]+q.Extends.row_index)
        for a in r['methods']:
            q=ts.MethodDef.rows[(int(a['token'],16)&0xffffff)-1]
            assert [a['name'],a['signature'],a['virtual'],a['new_slot'],a['abstract']]==[str(q.Name),q.Signature.value.hex(),q.Flags.mdVirtual,q.Flags.mdNewSlot,q.Flags.mdAbstract]
    for t,r in d['fields'].items():
        n=int(t,16)&0xffffff;f=ts.Field.rows[n-1]
        assert r['signature']==f.Signature.value.hex() and r['owner']==hex(0x2000000+fo[n])
        assert r['name']==str(f.Name) and r['static']==f.Flags.fdStatic
    for r in d['dispatch']:
        x=int(r['slot'],16)&0xffffff;y=int(r['implementation'],16)&0xffffff
        a=ts.MethodDef.rows[x-1];b=ts.MethodDef.rows[y-1]
        assert str(a.Name)==str(b.Name) and a.Signature.value==b.Signature.value
        assert a.Flags.mdVirtual and b.Flags.mdVirtual and not b.Flags.mdNewSlot
        n=own[y];chain=[]
        while n:
            chain.append(n);q=ts.TypeDef.rows[n-1];n=q.Extends.row_index if q.Extends and q.Extends.table.name=='TypeDef' else 0
        assert own[x] in chain and r['receiver']==hex(0x2000000+own[y])
    for t,r in d['method_specs'].items():
        q=ts.MethodSpec.rows[(int(t,16)&0xffffff)-1]
        assert r=={'method':hex({'MethodDef':0x6000000,'MemberRef':0xa000000}[q.Method.table.name]+q.Method.row_index),'instantiation':q.Instantiation.value.hex()}
    r=next(x for x in ts.FieldRva if x.Field.row_index==0xc9a);blob=pe.get_data(r.Rva,16)
    assert d['cache_cutoff_data']=={'field':'0x4000c9a','rva':hex(r.Rva),'bytes':16,'sha256':sha(blob),'int32_values':[20260630,20260630,20260630,20231214]}
    assert list(struct.unpack('<4i',blob))==d['cache_cutoff_data']['int32_values']
    # E038-independent reducer + separate bounded byte decoder (no assets emitted).
    from check_hitsound_resources import independent_key,i32
    key=independent_key();assert key==d['string_decoder']['final_key']
    source=next(r.data for r in pe.net.resources if str(r.name)==d['string_decoder']['resource']['name'])
    assert sha(source)==d['string_decoder']['resource']['sha256']
    for identifier,r in d['strings'].items():
        pos=i32(int(identifier)^-351960377^key^1516081623);assert pos==r['offset']
        keys=bytes(source[pos+j]^((key>>(j*8))&255) for j in range(3))
        size=i32(struct.unpack_from('<I',source,pos+3)[0]^pos^-973073496^key)
        assert not size&0x80000000 # All four E044 strings use uncompressed arm.
        cipher=source[pos+7:pos+7+(size&0xfffffff)];assert sha(cipher)==r['cipher_sha256']
        last=((len(cipher)+11)^(keys[1]+7))&255;state=keys[0]+(keys[2]<<8)+(last<<3);word=0;plain=[]
        for j,byte in enumerate(cipher):
            if j%2==0:state=(state*214013+2531011)&0xffffffff;word=(state>>16)&65535
            plain.append((keys[1]^byte^(last+3)^(word&255))&255);word>>=8;last=byte
        text=''.join(map(chr,plain)) if size&0x40000000 else bytes(plain).decode('utf-16le')
        assert text==r['text']
    literals=0
    def at(m,o,op,v=None):
        nonlocal literals
        actual=instructions[m][o];assert actual[0]==op and (v is None or actual[1]==v),(hex(m),hex(o),actual,op,v)
        literals+=1
    for m in [0x8d9,0x8c8]:assert not ts.MethodDef.rows[m-1].Flags.mdVirtual
    for m,o,op,v in [
        (0x1bc6,0x2c,'newobj',0x6000801),(0x801,2,'call',0x6003696),
        (0x3696,2,'call',0x60008c4),(0x8c4,0x17,'callvirt',0x6001bc0),
        (0x1bc0,0x2e,'newobj',0x6001bb3),(0x8d9,0x84,'callvirt',0x60008d3),
        (0x1bd0,0x26,'callvirt',0x60008d9),(0x1bd0,0x2b,'conv.r4',None),
        (0x8d7,0x76,'callvirt',0x60028be),(0x8d7,0x93,'callvirt',0x60028f7),
        (0x8d7,0xaf,'callvirt',0x60008d2),(0x808,0x22,'callvirt',0x600144a),
        (0x808,0x2d,'callvirt',0x6001441),(0x810,0x53,'ldc.i4.1',None),
        (0x810,0x96,'newobj',0x600366d),(0x2cb4,0x29,'sub',None),
        (0x2cb4,0x2a,'conv.r8',None),(0x366d,0x1c,'ldc.r8',25.0),
        (0x3697,0,'ldc.i4',400),(0x3f34,0xb2,'bgt',None),
        (0x1d08,0xb2,'ldloc.2',None),(0x1d08,0xb3,'conv.r8',None),
        (0x1d08,0xb5,'callvirt',0x6001d04),(0x1d08,0xba,'call',0x60019df),
        (0x19e4,2,'ldc.r8',.9),(0x19e4,0xb,'ldc.i4',400),
        (0x19e8,7,'call',0x60011c1),(0x19e8,0x18,'call',0x60029f7),
        (0x19e8,0x2f,'call',0x600408c),(0x3539,0x64,'call',0x6001744),
        (0x325e,7,'call',0x6001e57),(0x80d,1,'ldc.r8',.63),
        (0x80d,0xf,'ldc.r8',.02275),(0x80e,6,'ldc.r8',.0675),
        (0x80a,0x10a,'call',0x60019d8),(0x80a,0x10f,'call',0x600080f),
        (0x80f,1,'ldc.r8',1.12),(0x19e2,1,'ldc.r8',.3333333333333333),
        (0x1bf2,0x108,'ldc.i4',20250107),(0x1c23,0x1c,'call',0x6001bc9)]:
        at(m,o,op,v)
    pe.close();return {'audited_bodies':count,'scanned_bodies':scan,'field_sites':len(fields),'call_sites':len(calls),'literal_controls':literals}

def finite():
    import star_difficulty_projection as p
    rows=[]
    def add(name,inp,expected,actual,sites):
        if isinstance(expected,float) and isinstance(actual,(float,int)):
            ok=math.isnan(actual) if math.isnan(expected) else actual==expected if not math.isfinite(expected) else math.isclose(actual,expected,rel_tol=3e-13,abs_tol=2e-14)
        else:ok=canon(expected)==canon(actual)
        r={'id':f'N{len(rows)+1:02}','name':name,'inputs':canon(inp),'expected':canon(expected),'actual':canon(actual),'pass':ok,'authority':sites,
           'classification':'VERIFIED COMPONENT — research mathematical projection only','original_runtime':'NOT RUN'}
        rows.append(r);assert ok,r
    with localcontext() as ctx:
        ctx.prec=65
        dec=lambda x:Decimal(str(x))
        for dt in [0,25,100,400,1000,2000]:
            for base in [.2,.3,.8]:
                expected=float(dec(base)**(dec(dt)/1000))
                add('decay explicit elapsed/base',[dt,base],expected,p.recurrence(1,0,dt,base),['M19e5','M3538','M325c'])
        for radius in [20,30,36,50]:
            # Exact 3-4-5 length plus independently stored Single quotient/product.
            sf=lambda x:struct.unpack('<f',struct.pack('<f',x))[0]
            expected=float(sf(sf(5.0)*sf(sf(50.0)/sf(radius))))
            add('Single vector normalization radius',[3,4,radius],expected,p.normalized_distance(3,4,radius),['M3691:008a-0114','M4ec9'])
        for starts in [(1000,1000,1000,1.0),(1000,1100,1200,1.0),(1000,0,1400,1.5),(2147483647,-1,2147483647,1.0)]:
            a,b,e,rate=starts;delta=((a-b+2147483648)%4294967296-2147483648)/rate
            add('pair signed subtraction before Double',starts,{'delta':delta,'start':a/rate,'end':e/rate,'strain_time':max(25.,delta)},p.pair(*starts),['M2cb4:001d-004b','M366d:0016-002a'])
        for strength in [1.,20.]:
            expected=10+5*((1+strength/2)/(1+1+strength/2))
            add('per-object weight uses i to exponent; positives sorted',[5,10,0,-1],expected,p.ordered_aggregate([5,10,0,-1],strength)[0],['M1d08:0030-00df','M3536'])
            add('aggregation empty',strength,0.,p.ordered_aggregate([],strength)[0],['M1d08:0000-0016'])
            add('aggregation equal ties',strength,2+2*((1+strength/2)/(2+strength/2)),p.ordered_aggregate([2,2],strength)[0],['M1d08:0064,009d-00df'])
        for v,duration in [(10,400),(10,800),(10,200),(0,400)]:
            expected=float(dec(v)*(1-dec('.9')**(dec(duration)/400))/(1-dec('.9')))
            add('already ordered duration segment', [v,duration],expected,p.duration_aggregate([(v,duration)]),['M19e9:002a-0090'])
        add('duration order discriminates sorting',[10,400,5,400],14.5,p.duration_aggregate([(10,400),(5,400)]),['M19e9','M19ec'])
        for dt in [100.,200.,500.,1000.,2000.]:
            # Two-circle branch cancels only in this independent oracle, not model.
            q=dec('1.16')*1000/dec(dt)
            expected=float((dec('1.12')*4)**(Decimal(1)/3)*dec('.0675')*q.sqrt())
            add('conditional two prepared circles',{'dt':dt,'full_window':99,'count':2},expected,p.two_circle(dt)['double'],['M11c1:0000-0032','M408c:0000-0032','M1e57:0000-001e','M0d6c','M3539','M080a'])
        add('all-zero final accumulators',[0,0,0],0.,p.final(0,0,0)['double'],['M080a','M080d-M080f'])
        for channel in range(3):
            raw=[0.,0.,0.];raw[channel]=1.
            scale=dec('.02275') if channel==0 else dec('.0675')
            expected=float(scale*(4*dec('1.12'))**(Decimal(1)/3))
            add('single nonzero accumulator final',[raw],expected,p.final(*raw)['double'],['M080d','M080e','M080b','M1d09','M080f'])
        expected=float((Decimal(1)+Decimal(1))**(Decimal(1)/dec('1.1')))
        add('combined final channels equal',[1,1],expected,p.combine(1,1),['M080c:001c-0053'])
        add('combine bypass left zero',[0,2],2.,p.combine(0,2),['M080c:0000-000d'])
        add('combine bypass right zero',[2,0],2.,p.combine(2,0),['M080c:000e-001b'])
        add('NaN left not finite shortcut',['NaN',1],math.nan,p.combine(math.nan,1),['M080c:000a bgt.un'])
        for value,expected_bits,expected_display in [(1.15,'3333933f',1.14),(1.01,'ae47813f',1.),(2.675,'33332b40',2.67),(0.,'00000000',0.),(-1.15,'333393bf',1.14)]:
            add('cached Single bits',[value],expected_bits,p.bits(value),['M1bd0:002b'])
            add('display after Single not raw Double',[value],expected_display,p.display(value),['M1bc7:003a','M1bd5:0011','M2d7f'])
        for v in [math.nan,math.inf,-math.inf]:
            add('cache exceptional display category',canon(v),'NaN' if math.isnan(v) else '+Infinity',p.display(v),['M1bd5','M2d7f; no text/rendering guarantee'])
        for mods,expected in [(0,0),(1,0),(4,0),(64,64),(512,0),(64|512,64),(8192,0),(1370,1370),(536870912,0)]:
            add('ordinary cache key mask',mods,expected,p.cache_key(mods),['M0803','M0805','M08cf'])
        for value,busy,expected in [(None,False,True),(None,True,False),(-1.,False,True),(-1.,True,False),(0.,False,False),(1.,False,False),(math.nan,False,False)]:
            add('missing/stale/in-progress scheduling',[canon(value),busy],expected,p.requested(value,busy),['M1bd7:0000-0042'])
        add('old DB shape filters and converts',{0:1.15,4:9.,64:2.675},{'0':float(struct.unpack('<f',bytes.fromhex('3333933f'))[0]),'64':float(struct.unpack('<f',bytes.fromhex('33332b40'))[0])},p.legacy_cache({0:1.15,4:9.,64:2.675},20250106,20250106),['M1bf2:0103-0167'])
        add('new DB shape keeps supplied keys',{0:1.25,4:2.},{'0':1.25,'4':2.},p.legacy_cache({0:1.25,4:2.},20250107,20250106),['M1bf2:0169-0174'])
        add('older cutoff makes negative magnitude',1.25,{'0':-1.25},p.legacy_cache({0:1.25},20250107,20260101),['M1bf2:0175-01d6'])
    return rows

def preservation(root):
    d=json.loads((root/'re/evidence/44/preservation.json').read_text(encoding='utf-8'))
    for name,h in d['protected'].items():assert sha((root/name).read_bytes())==h,name
    assert sha((root/'re/contexts.md').read_bytes()[:d['journal_bytes']])==d['journal_sha256']
    return len(d['protected'])

if __name__=='__main__':
    packet=Path(sys.argv[1]);d=json.loads(packet.read_text(encoding='utf-8'))
    assert d['expedition']==44 and d['research_source']=='00ad4e790e5a0c23bc6bb4476a2313951dcb996f'
    stats=audit(d,sys.argv[2]);rows=finite();root=Path(__file__).resolve().parents[2]
    for name,h in d['collector_sources'].items():assert sha((root/'re/tools'/name).read_bytes())==h,name
    for name,h in d['preparation_inputs'].items():assert sha((root/name).read_bytes())==h,name
    assert len(d['reference_cases'])==58 and all(r['original_runtime']=='NOT RUN' for r in d['reference_cases'])
    shared=0
    for path in (root/'re/evidence').glob('*/*.json'):
        if not path.parent.name.isdigit() or int(path.parent.name)>=44:continue
        old=json.loads(path.read_text(encoding='utf-8'))
        if not isinstance(old,dict):continue
        for t,r in old.get('methods',{}).items():
            if t in d['methods'] and r.get('body_sha256'):
                assert r['body_sha256']==d['methods'][t].get('body_sha256'),(str(path),t)
                shared+=1
    stats['historical_body_comparisons']=shared
    stats.update(protected_prior_files=preservation(root),finite_mathematical_controls=len(rows))
    out={'research_source':d['research_source'],'archive_sha256':ARCHIVE,'exe_sha256':EXE,
        'packet_sha256':sha(packet.read_bytes()),'stats':stats,'finite_controls':rows,
        'source_sha256':{n:sha((root/'re/tools'/n).read_bytes()) for n in ['check_star_difficulty.py','star_difficulty_projection.py']},
        'limits':['NOT a full chart-star checker','Prepared/artificial operands only','No original CLR/native execution; no original-x86 marginal precision measurement']}
    if len(sys.argv)>3:Path(sys.argv[3]).write_text(json.dumps(out,indent=2,allow_nan=False)+'\n',encoding='utf-8',newline='\n')
    print('PASS E044',json.dumps(stats,sort_keys=True))
