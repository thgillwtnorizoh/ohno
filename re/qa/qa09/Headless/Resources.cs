using Ohno.CircleGame.Gameplay;
namespace Ohno.QA09;
public static partial class Tests
{
    static void ResourceCases()
    {
        foreach(var bank in new[]{(1,"normal"),(2,"soft"),(3,"drum")})
            foreach(var layer in new[]{(1,"hitnormal"),(2,"hitwhistle"),(4,"hitfinish"),(8,"hitclap")})
                foreach(int index in new[]{1,2,7}) Add($"R01 key {bank.Item2}/{layer.Item2}/{index}","E038 decoded names and dynamic decimal suffix",()=>{
                    var f=new ResourcesFixture();Eq(f.R.Key(layer.Item1,bank.Item1,index),bank.Item2+"-"+layer.Item2+(index==1?"":index.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                });
        Add("R02 index0 skips map and selects UI before gameplay","E038 M3b14 map gate /embedded order",()=>{
            var f=new ResourcesFixture();f.Map.Data["normal-hitnormal.wav"]=Bytes(1);f.Defaults.U["normal-hitnormal"]=Bytes(2);f.Defaults.G["normal-hitnormal"]=Bytes(3);
            Eq(f.R.Standard(1,1,0),101);Seq(f.Trace,"ui normal-hitnormal");Eq(f.Loader.Loaded.Single().Bytes[0],(byte)2);Eq(f.Loader.Loaded[0].Mode,SampleLoadMode.EmbeddedBytes);
        });
        foreach(int index in new[]{1,2,9}) Add($"R03 index{index} map suffix and first format","E038 M3b14 map key construction",()=>{
            var f=new ResourcesFixture();string name="drum-hitfinish"+(index==1?"":index)+".wav";f.Map.Data[name]=Bytes(9);f.Map.Data[name[..^3]+"mp3"]=Bytes(5);
            Eq(f.R.Standard(4,3,index),101);Seq(f.Trace,"map read "+name);Eq(f.Loader.Loaded.Single().Name,name);
        });
        Add("R04 standard precedence wav then mp3 before ogg","E038 M3b14:0140-01e7",()=>{
            var f=new ResourcesFixture();f.Map.Data["soft-hitclap.mp3"]=Bytes(2);f.Map.Data["soft-hitclap.ogg"]=Bytes(3);
            f.R.Standard(8,2,1);Seq(f.Trace,"map read soft-hitclap.wav","map read soft-hitclap.mp3");Eq(f.Loader.Loaded.Single().Name,"soft-hitclap.mp3");
        });
        foreach(int result in new[]{-1,0,301}) Add($"R05 first nonnull map bytes loader{result}","E038 native return opaque; no extension retry; map zero retained",()=>{
            var f=new ResourcesFixture();f.Map.Data["normal-hitnormal.wav"]=Bytes(1);f.Map.Data["normal-hitnormal.mp3"]=Bytes(2);f.Loader.Result=_=>result;
            Eq(f.R.Standard(1,1,1),result);Eq(f.Loader.Loaded.Count,1);Yes(!f.Trace.Contains("map read normal-hitnormal.mp3"),"nonnull wav ends probe even load failure");Eq(f.R.Caches.Map["normal-hitnormal"],result);
        });
        foreach(int count in new[]{0,1023,1024}) Add($"R06 byte threshold {count}","E037/E038 native short-create boundary, artificial byte provider",()=>{
            var f=new ResourcesFixture();f.Map.Data["normal-hitnormal.wav"]=Bytes(count:count);f.R.Standard(1,1,1);Eq(f.Loader.Loaded.Single().Mode,count<1024?SampleLoadMode.MapShortCreate:SampleLoadMode.MapBytes);
        });
        Add("R07 version4 per-extension unprefixed fallback","E038 M3b14 legacy branch, artificial unsupported gameplay format control",()=>{
            var f=new ResourcesFixture(format:4);f.Map.Data["hitnormal.wav"]=Bytes();f.Map.Data["normal-hitnormal2.mp3"]=Bytes(2);f.R.Standard(1,1,2);Seq(f.Trace,"map read normal-hitnormal2.wav","map read hitnormal.wav");
        });
        Add("R08 version5 skips unprefixed fallback","E038 version boundary5",()=>{
            var f=new ResourcesFixture(format:5);f.Map.Data["hitnormal.wav"]=Bytes();Eq(f.R.Standard(1,1,1,4),-1);Seq(f.Trace,"map read normal-hitnormal.wav","map read normal-hitnormal.mp3","map read normal-hitnormal.ogg");
        });
        Add("R09 map missing then skin existing mp3","E038 map->skin provider order",()=>{
            var f=new ResourcesFixture(name:"independent");f.Skin.Data["soft-hitwhistle.mp3"]=Bytes(2);f.R.Standard(2,2,2);
            Seq(f.Trace,"map read soft-hitwhistle2.wav","map read soft-hitwhistle2.mp3","map read soft-hitwhistle2.ogg","skin exists soft-hitwhistle.wav","skin exists soft-hitwhistle.mp3","skin read soft-hitwhistle.mp3");Eq(f.Loader.Loaded.Single().Origin,SampleOrigin.Skin);
        });
        Add("R10 map suppressed and skin disabled use gameplay default","E038 provider gates",()=>{
            var f=new ResourcesFixture(suppressed:true,name:"custom",skinSamples:false);f.Map.Data["normal-hitfinish.wav"]=Bytes();f.Skin.Data["normal-hitfinish.wav"]=Bytes();f.Defaults.G["normal-hitfinish"]=Bytes(3);
            f.R.Standard(4,1,1);Seq(f.Trace,"ui normal-hitfinish","game normal-hitfinish");Eq(f.Loader.Loaded.Single().Origin,SampleOrigin.Default);
        });
        Add("R11 Default named skin bypassed","E038 exact Default predicate",()=>{
            var f=new ResourcesFixture();f.Skin.Data["normal-hitnormal.wav"]=Bytes();Eq(f.R.Standard(1,1,0),-1);Seq(f.Trace,"ui normal-hitnormal","game normal-hitnormal");
        });
        Add("R12 absent selected map bypasses map stage","E038 selected global map gate",()=>{
            var f=new ResourcesFixture(hasMap:false);f.Map.Data["normal-hitnormal.wav"]=Bytes();Eq(f.R.Standard(1,1,1),-1);Seq(f.Trace,"ui normal-hitnormal","game normal-hitnormal");
        });
        Add("R13 early process gate returns before resource work","E038 M3b14:0000-0016",()=>{var f=new ResourcesFixture(early:true);Eq(f.R.Standard(1,1,1),-1);Eq(f.Trace.Count,0);});
        Add("R14 missing permitted skin falls through to default","E038 M3b14 skin->embedded",()=>{
            var f=new ResourcesFixture(name:"custom");f.Defaults.G["drum-hitclap"]=Bytes(3);f.R.Standard(8,3,0);
            Seq(f.Trace,"skin exists drum-hitclap.wav","skin exists drum-hitclap.mp3","skin exists drum-hitclap.ogg","ui drum-hitclap","game drum-hitclap");
        });
        Add("R15 null permitted skin is explicit unsupported receiver","E038 original nullable-resource branch; R11 rejection substitutes exception type",()=>{var f=new ResourcesFixture(hasSkin:false,name:"x");Throws<System.IO.InvalidDataException>(()=>f.R.Standard(1,1,0));});
        foreach(int result in new[]{0,-1,700}) Add($"R16 embedded handle normalization {result}","E038 embedded-only native zero normalization",()=>{
            var f=new ResourcesFixture();f.Defaults.G["normal-hitnormal"]=Bytes();f.Loader.Result=_=>result;Eq(f.R.Standard(1,1,0),result==0?-1:result);Eq(f.R.Caches.Embedded["normal-hitnormal"],result==0?-1:result);
        });
        foreach(int result in new[]{0,-1,700}) Add($"R17 skin first existing decoder result {result}","E038 first existing path stops; skin zero retained",()=>{
            var f=new ResourcesFixture(name:"x");f.Skin.Data["normal-hitnormal.wav"]=Bytes();f.Skin.Data["normal-hitnormal.mp3"]=Bytes(2);f.Loader.Result=_=>result;
            Eq(f.R.Standard(1,1,0),result);Eq(f.Loader.Loaded.Count,1);Yes(!f.Trace.Contains("skin exists normal-hitnormal.mp3"),"no codec fallback after existing path");
        });
        Add("R18 missing resource cached without repeated lookup","E038 cached -1 skips provider work",()=>{
            var f=new ResourcesFixture();Eq(f.R.Standard(1,1,1),-1);f.Trace.Clear();f.Map.Data["normal-hitnormal.wav"]=Bytes();Eq(f.R.Standard(1,1,1),-1);Eq(f.Trace.Count,0);
        });
        Add("R19 shared fallback handles across planes","E038 table versus cache ownership",()=>{
            var f=new ResourcesFixture();f.Defaults.SupplyAll=true;f.R.Prepare([],[]);Eq(f.Loader.Loaded.Count,12);
            foreach(int b in new[]{1,2,3})foreach(int bit in new[]{1,2,4,8}){int h=f.R.Resolve(bit,b,0);Eq(f.R.Resolve(bit,b,1),h);Eq(f.R.Resolve(bit,b,2),h);}
        });
        Add("R20 dynamic preparation and dictionary-only fallback","E038 M28f5->M3b18->M3b36",()=>{
            var f=new ResourcesFixture();f.Defaults.SupplyAll=true;f.Map.Data["soft-hitclap7.wav"]=Bytes(7);f.R.Prepare([7,7],[8]);int h=f.R.Caches.Map["soft-hitclap7"];
            f.Trace.Clear();Eq(f.R.Resolve(8,2,7),h);Eq(f.R.Resolve(4,3,8),f.R.Resolve(4,3,0));Eq(f.R.Resolve(2,1,99),f.R.Resolve(2,1,0));Eq(f.Trace.Count,0);
        });
        Add("R21 timing then registered indexes are distinct preloads","E038 index collection order; cache controls",()=>{
            var f=new ResourcesFixture();f.R.Prepare([7,3,7],[3,9]);var reads=f.Trace.Where(s=>s.StartsWith("map read normal-hitnormal",StringComparison.Ordinal)).ToArray();
            Seq(reads.Take(3),"map read normal-hitnormal7.wav","map read normal-hitnormal7.mp3","map read normal-hitnormal7.ogg");
            Yes(Array.IndexOf(reads,"map read normal-hitnormal3.wav")<Array.IndexOf(reads,"map read normal-hitnormal9.wav"),"timing unique indexes before registered9");
        });
        Add("R22 cached dynamic zero not plane0 fallback","E038 found non--1 includes0",()=>{var f=new ResourcesFixture();f.PrepareDefaults();f.R.Caches.Map["normal-hitnormal7"]=0;Eq(f.R.Resolve(1,1,7),0);});
        Add("R23 custom exact file branch","E038 M3b15 exact filename before probing",()=>{
            var f=new ResourcesFixture();f.Map.Data["kick.ogg"]=Bytes(2);f.Map.Data["kick.wav"]=Bytes(1);f.R.Custom("kick.ogg");Seq(f.Trace,"map exists kick.ogg","map read kick.ogg");Eq(f.Loader.Loaded.Single().Name,"kick.ogg");
        });
        Add("R24 custom fallback wav then ogg before mp3","E038 M3b15 distinct extension order",()=>{
            var f=new ResourcesFixture();f.Map.Data["kick.ogg"]=Bytes(2);f.Map.Data["kick.mp3"]=Bytes(3);f.R.Custom("kick");Seq(f.Trace,"map read kick.wav","map read kick.ogg");
        });
        Add("R25 missing exact file strips stem and probes","E038 whole string IndexOf/LastIndexOf",()=>{
            var f=new ResourcesFixture();f.Map.Data["kick.wav"]=Bytes();f.R.Custom("kick.mp3");Seq(f.Trace,"map exists kick.mp3","map read kick.wav");Eq(f.R.Caches.Map.Count,1);Yes(f.R.Caches.Map.ContainsKey("kick"),"stem cache key");
        });
        Add("R26 exact exists but unreadable never probes extensions","E038 K22 original-static",()=>{
            var f=new ResourcesFixture();f.Map.ExistsOnly.Add("kick.ogg");f.Map.Data["kick.wav"]=Bytes();Eq(f.R.Custom("kick.ogg"),-1);Seq(f.Trace,"map exists kick.ogg","map read kick.ogg");
        });
        Add("R27 custom collisions preserve first stem cache result","E038 K24",()=>{
            var f=new ResourcesFixture();f.Map.Data["kick.wav"]=Bytes(1);f.Map.Data["kick.ogg"]=Bytes(2);int a=f.R.Custom("kick.wav");f.Trace.Clear();Eq(f.R.Custom("kick.ogg"),a);Eq(f.Trace.Count,0);Eq(f.Loader.Loaded.Count,1);
        });
        Add("R28 custom cached miss needs explicit eviction","E038 miss lifetime",()=>{
            var f=new ResourcesFixture();Eq(f.R.Custom("kick"),-1);f.Map.Data["kick.wav"]=Bytes();f.Trace.Clear();Eq(f.R.Custom("kick"),-1);Eq(f.Trace.Count,0);f.R.EvictMap("kick");Yes(f.R.Custom("kick")>0,"reload after explicit eviction");
        });
        Add("R29 custom force replaces and frees previous handle","E038 M3b15:0171-019b",()=>{
            var f=new ResourcesFixture();f.Map.Data["kick.wav"]=Bytes();int a=f.R.Custom("kick.wav");int b=f.R.Custom("kick.wav",force:true);Seq(f.Loader.Freed,a);Yes(b!=a,"new opaque ID");Eq(f.R.Caches.Map["kick"],b);
        });
        foreach(int handle in new[]{-1,0,900}) Add($"R30 custom load opaque handle {handle}","E038 -1/0 distinct; no custom normalization",()=>{
            var f=new ResourcesFixture();f.Map.Data["kick.wav"]=Bytes();f.Loader.Result=_=>handle;Eq(f.R.Custom("kick"),handle);Eq(f.R.Caches.Map["kick"],handle);Eq(f.Loader.Loaded.Count,1);
        });
        Add("R31 leading dot probes rather than exact read","E038 K23 artificial filename",()=>{var f=new ResourcesFixture();f.Map.Data[".kick"]=Bytes();Eq(f.R.Custom(".kick"),-1);Seq(f.Trace,"map read .kick.wav","map read .kick.ogg","map read .kick.mp3");});
        Add("R32 custom rejection gates perform no lookup","E037 M24b9 and E038 M3b15",()=>{
            var f=new ResourcesFixture();Eq(f.R.Custom("kick",acceptingSetter:false),-1);Eq(f.R.Custom(""),-1);Eq(f.Trace.Count,0);var g=new ResourcesFixture(suppressed:true);Eq(g.R.Custom("kick"),-1);Eq(g.Trace.Count,0);
        });
        Add("R33 shared custom/standard map cache alias","E038 K40",()=>{
            var f=new ResourcesFixture();f.Map.Data["normal-hitnormal.ogg"]=Bytes(2);f.Map.Data["normal-hitnormal.wav"]=Bytes(1);int h=f.R.Custom("normal-hitnormal.ogg");f.Trace.Clear();Eq(f.R.Standard(1,1,1),h);Eq(f.Trace.Count,0);
        });
        Add("R34 cache clear scopes and opaque frees","E038 M3b1d/M3b1e/M3b1b",()=>{
            var caches=new SampleCaches();caches.Map["m"]=-1;caches.Skin["s"]=0;caches.Embedded["d"]=99;var f=new ResourcesFixture(caches:caches);
            f.R.ClearMap();Eq(caches.Map.Count,0);Eq(caches.Skin.Count,1);Eq(caches.Embedded.Count,1);Seq(f.Loader.Freed,-1);f.R.ClearSkin();Seq(f.Loader.Freed,-1,0);Eq(caches.Embedded.Count,1);f.R.ClearAll();Seq(f.Loader.Freed,-1,0,99);Eq(caches.Embedded.Count,0);
        });
        Add("R35 map replacement new map cache retains shared skin/default","E038 cache ownership; R11 bounded model",()=>{
            var shared=new SampleCaches();var first=new ResourcesFixture(name:"x",caches:new(shared.Skin,shared.Embedded));first.Defaults.SupplyAll=true;first.Map.Data["normal-hitnormal.wav"]=Bytes(1);first.R.Prepare([],[]);int mapHandle=first.R.Resolve(1,1,1);first.R.ClearMap();Yes(first.Loader.Freed.Contains(mapHandle),"old map ID released");
            var second=new ResourcesFixture(name:"x",caches:new(shared.Skin,shared.Embedded));second.R.Prepare([],[]);Eq(second.Loader.Loaded.Count,0);Eq(second.R.Resolve(1,1,1),second.R.Resolve(1,1,0));
        });
        Add("R36 all36 prepared cells discriminate map planes/banks/rows","E037 numeric row/bank mapping and E038 index0/1/2 providers",()=>{
            var f=new ResourcesFixture();string[] banks=["normal","soft","drum"],layers=["hitnormal","hitwhistle","hitfinish","hitclap"];int[] bits=[1,2,4,8];
            for(int bank=0;bank<3;bank++)for(int row=0;row<4;row++){int marker=10*(bank+1)+row;string key=banks[bank]+"-"+layers[row];f.Defaults.G[key]=Bytes(marker);f.Map.Data[key+".wav"]=Bytes(marker+40);f.Map.Data[key+"2.wav"]=Bytes(marker+80);}
            f.Loader.Result=s=>100+s.Bytes[0];f.R.Prepare([],[]);
            for(int plane=0;plane<3;plane++)for(int bank=0;bank<3;bank++)for(int row=0;row<4;row++)Eq(f.R.Resolve(bits[row],bank+1,plane),110+bank*10+row+plane*40);
            Eq(f.R.Resolve(4,0,2),202);Eq(f.R.Resolve(4,99,1),152);Eq(f.R.Resolve(1,1,-7),110);
        });
        Add("R37 artificial All None numeric key identities","E038 enum-name spelling separate from numeric fallback bank",()=>{
            var f=new ResourcesFixture();Eq(f.R.Key(1,-1,7),"all-hitnormal7");Eq(f.R.Key(1,0,7),"none-hitnormal7");Eq(f.R.Key(1,9,7),"9-hitnormal7");
        });
        Add("R38 nonnull map load failure falls to default not next format","E038 provider boundary versus loader result",()=>{
            var f=new ResourcesFixture();f.Map.Data["normal-hitnormal.wav"]=Bytes(1);f.Map.Data["normal-hitnormal.mp3"]=Bytes(2);f.Defaults.G["normal-hitnormal"]=Bytes(3);f.Loader.Result=s=>s.Origin==SampleOrigin.Map?-1:700;
            Eq(f.R.Standard(1,1,1),700);Eq(f.Loader.Loaded.Count,2);Seq(f.Loader.Loaded.Select(s=>s.Origin),SampleOrigin.Map,SampleOrigin.Default);Yes(!f.Trace.Contains("map read normal-hitnormal.mp3"),"no next extension after nonnull bytes");
        });
    }
}
