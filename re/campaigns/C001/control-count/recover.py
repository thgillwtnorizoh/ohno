#!/usr/bin/env python3
"""Hash original bodies and check selected historical CIL sites; no execution."""
import hashlib,json,sys,zipfile
from pathlib import Path
import dnfile
from dncil.cil.body.reader import read_method_body_from_bytes
H=Path(__file__).resolve().parent;R=H.parents[3]
sha=lambda b:hashlib.sha256(b).hexdigest()
REGIONS={0x28f7:[(0x1484,0x1558),(0x1662,0x17cd),(0x1a6e,0x1ab8),(0x1cfa,0x1d10)],
 0x142d:[(0xad,0x123)],0x9e4:[(0,0xffff)],0x144a:[(0xaa,0xce),(0x7a2,0x8d6),(0x9d9,0xb55),(0xd47,0xf45),(0x1332,0x1345),(0x1377,0x137d)],
 0x110c:[(0,0xffff)],0x110d:[(0,0xffff)],0x4ec9:[(0,0xffff)],0x4ecb:[(0,0xffff)],0x4ed2:[(0,0xffff)],
 0x4efd:[(0,0xffff)],0x4efe:[(0,0xffff)],0x4f00:[(0,0xffff)],0x1457:[(0,0xffff)],
 0x144b:[(0x35,0x50),(0x120,0x216)],0x28f5:[(0xc0,0x160)],0x28ea:[(0,0xffff)],0x28ec:[(0,0xffff)],0x145f:[(0,0xb0)]}
PACKETS=['05/slider-timing.json','06/linear-geometry.json','32/bezier-geometry.json','16/slider-components.json']
def recover(path):
    archive=Path(path).read_bytes();assert sha(archive)=='7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738'
    raw=zipfile.ZipFile(path).read('osu!.exe');assert sha(raw)=='6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
    pe=dnfile.dnPE(data=raw);packets={p:json.loads((R/'re/evidence'/p).read_text()) for p in PACKETS}
    methods={};sites=[];matches=[];bodies={};new=[]
    for rid,regions in REGIONS.items():
        md=pe.net.mdtables.MethodDef.rows[rid-1];b=read_method_body_from_bytes(pe.get_data(md.Rva));bodies[rid]=b
        key=hex(0x6000000+rid);digest=sha(pe.get_data(md.Rva,b.size));old=None
        for p,j in packets.items():
            if key in j['methods']:
                assert digest==j['methods'][key]['body_sha256'];matches.append([p,key]);old=j['methods'][key]
        assert old is not None,key
        methods[key]=dict(rva=md.Rva,codeBytes=b.code_size,signature=md.Signature.value.hex(),bodySha256=digest,
            exceptionRegions=[{**{k:v for k,v in vars(e).items() if k!='catch_type'},'catchToken':getattr(e.catch_type,'value',None)} for e in b.exception_handlers])
        by={i.offset-b.header_size:i for i in b.instructions}
        selected={}
        for field in ('token_sites','constants','arithmetic_sites','operations'):
            for item in old.get(field,[]):
                off,op=item[:2]
                if not any(lo<=off<hi for lo,hi in regions):continue
                want=item[2] if len(item)>2 else None
                if isinstance(want,str) and want.startswith('0x'):want=int(want,16)
                selected[off,op]=want
        for (off,op),want in sorted(selected.items()):
            i=by[off];got=getattr(i.operand,'value',i.operand)
            assert (i.opcode.name,got)==(op,want),(key,off)
            sites.append([key,off,op,want])
    def anchor(rid,off,op,value=None):
        b=bodies[rid];i=next(i for i in b.instructions if i.offset-b.header_size==off)
        assert (i.opcode.name,getattr(i.operand,'value',i.operand))==(op,value)
        new.append([hex(0x6000000+rid),off,op,value])
    for off in (0x176c,0x17ac):anchor(0x28f7,off,'conv.i4');anchor(0x28f7,off+1,'conv.r4')
    anchor(0x28f7,0x1699,'ldc.i4.s',124);anchor(0x28f7,0x1749,'ldc.i4.s',58)
    anchor(0x28f7,0x1728,'ldc.i4.2');anchor(0x144a,0x7a2,'ldc.i4.1');anchor(0x144a,0x7cd,'ldc.i4.1')
    anchor(0x28f7,0x1cfb,'ldarg.3');anchor(0x28f7,0x1d03,'rethrow')
    anchor(0x28f5,0xd3,'ldc.i4.0');anchor(0x28f5,0xd4,'call',0x60028f7)
    anchor(0x28f5,0x156,'ldc.i4.0');anchor(0x28f5,0x157,'stloc.1')
    assert str(pe.net.mdtables.TypeRef.rows[0x202-1].TypeName)=='Object'
    assert str(pe.net.mdtables.TypeRef.rows[0x16d-1].TypeName)=='Exception'
    # Head insertion and one-pair Linear loop compare against list Count, not64.
    branchChecks=[]
    for rid,off,op,targets in [(0x144a,0xb4,'switch',[0xce,0x1f8,0x7a2,0x56a]),(0x144a,0x877,'blt',[0x7aa]),(0x28f7,0x17c8,'blt',[0x16ab])]:
        b=bodies[rid];i=next(i for i in b.instructions if i.offset-b.header_size==off)
        operand=i.operand if isinstance(i.operand,list) else [i.operand]
        assert i.opcode.name==op and [x-b.header_size for x in operand]==targets
        branchChecks.append([hex(0x6000000+rid),off,op,targets])
    return dict(archiveSha256=sha(archive),exeSha256=sha(raw),methods=methods,methodCount=len(methods),
        exceptionTypes={'0x1000202':'Object','0x100016d':'Exception'},
        historicalBodyMatches=matches,siteCount=len(sites),sites=sites,independentAnchors=new,branches=branchChecks,
        authorityHashes={p:sha((R/'re/evidence'/p).read_bytes()) for p in PACKETS},originalExecuted=False,
        limitation='selected ordinary branch sites; no universal original maximum or CLR allocation/performance theorem')
if __name__=='__main__':
    j=recover(sys.argv[1]);Path(sys.argv[2]).write_text(json.dumps(j,indent=2,sort_keys=True)+'\n')
    print('PASS',j['siteCount'],'historical sites;',len(j['independentAnchors']),'independent anchors;',len(j['branches']),'branches;',j['methodCount'],'bodies;',len(j['historicalBodyMatches']),'body matches')
