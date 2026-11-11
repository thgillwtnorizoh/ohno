using System.Text.Json;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA13;
public static partial class Tests
{
    sealed class Probe:IRankGradeRecord
    {
        public List<string> Reads=[];public ushort C300=1,C50,Miss;public bool Good=true;public int Mods;
        public Queue<int> Totals=new([1,1]);public Queue<int> ModValues=[];
        public ushort N300{get{Reads.Add("n300");return C300;}}
        public ushort N100=>throw new Exception("Grade must not read n100 directly");
        public ushort N50{get{Reads.Add("n50");return C50;}}
        public ushort NMiss{get{Reads.Add("MISS");return Miss;}}
        public int WholeTotal{get{Reads.Add("total");return Totals.Dequeue();}}
        public bool Success{get{Reads.Add("success");return Good;}}
        public int Modifiers{get{Reads.Add("mods");return ModValues.Count>0?ModValues.Dequeue():Mods;}}
    }
    static void Grades()
    {
        using var vectors=JsonDocument.Parse(Read("re/qa/qa13/Inputs/grade-vectors.json"));
        foreach(var element in vectors.RootElement.GetProperty("cases").EnumerateArray())
        {
            var c=element.Clone();Add("G/"+c.GetProperty("id").GetString(),"E042 M2b47:0001-0113; independent rational oracle",()=>{
                var n=c.GetProperty("counts").EnumerateArray().Select(x=>(ushort)x.GetInt32()).ToArray();var e=c.GetProperty("expected");
                var r=new Record{C300=n[0],C100=n[1],C50=n[2],Miss=n[3],Success=c.GetProperty("success").GetBoolean(),Mods=c.GetProperty("mods").GetInt32()};
                Eq((int)r.Grade(),e.GetProperty("grade").GetInt32());Eq(r.WholeTotal,e.GetProperty("total").GetInt32());
                float p=(float)n[0]/r.WholeTotal,f=(float)n[2]/r.WholeTotal;
                if(r.WholeTotal==0){Yes(float.IsNaN(p)&&float.IsNaN(f),"empty proportions NaN");}
                else{Eq(unchecked((uint)BitConverter.SingleToInt32Bits(p)).ToString("x8"),e.GetProperty("p300Bits").GetString());Eq(unchecked((uint)BitConverter.SingleToInt32Bits(f)).ToString("x8"),e.GetProperty("p50Bits").GetString());}
                Yes(r.Grade()!=RankGrade.N,"N9 not calculated");observations.Add(new{p300=p.ToString("R"),p50=f.ToString("R")});
            });
        }
        Add("G/read-order-failure","M2b47:0001/0008/0011/0018/0021/0028",()=>{var p=new Probe{Good=false};Eq(OrdinaryRankGrade.Calculate(p),RankGrade.F);Seq(p.Reads,["n300","total","n50","total","success"]);});
        Add("G/two-fresh-totals","M2b47 separate callvirt M2b44",()=>{var p=new Probe{C300=99,C50=1,Totals=new([100,50])};Eq(OrdinaryRankGrade.Calculate(p),RankGrade.A);Seq(p.Reads,["n300","total","n50","total","success","MISS"]);});
        Add("G/hidden-short-circuit","M2b47:0033-0064",()=>{var p=new Probe{Mods=8};Eq(OrdinaryRankGrade.Calculate(p),RankGrade.XH);Seq(p.Reads,["n300","total","n50","total","success","mods"]);});
        Add("G/fresh-flashlight-read","M2b47:0033/0049",()=>{var p=new Probe{ModValues=new([0,1024])};Eq(OrdinaryRankGrade.Calculate(p),RankGrade.XH);Seq(p.Reads,["n300","total","n50","total","success","mods","mods"]);});
        Add("G/MISS-ordered-reads","M2b47 A then B arms",()=>{var p=new Probe{C300=9,Miss=1,Totals=new([10,10])};Eq(OrdinaryRankGrade.Calculate(p),RankGrade.B);Seq(p.Reads,["n300","total","n50","total","success","MISS","MISS"]);});
        foreach(string operand in new[]{"score","valid","perfect","maxcombo"})Add("G/independent-"+operand,"M2b47 no read of score/F1990/F197c/F1975",()=>{var r=new Record{Success=true};var before=r.Grade();switch(operand){case "score":r.Numeric=int.MaxValue;break;case "valid":r.Valid=false;break;case "perfect":r.Perfect=true;break;case "maxcombo":r.Max=int.MaxValue;break;}Eq(r.Grade(),before);});
        Add("G/uncached-counter","M2b47 no grade store",()=>{var r=new Record{Success=true};Eq(r.Grade(),RankGrade.X);r.C300=7;r.C100=3;Eq(r.Grade(),RankGrade.C);});
        Add("G/uncached-success","M2b47 fresh F1978",()=>{var r=new Record{Success=true};Eq(r.Grade(),RankGrade.X);r.Success=false;Eq(r.Grade(),RankGrade.F);});
        Add("G/R30-independent-conflict","M2b47:00be-0102; E042 R30 historical description",()=>{var r=new Record{Success=true};Eq(r.Grade(),RankGrade.X);r.C300=9;r.Miss=1;float p=(float)9/(float)10;Eq(BitConverter.SingleToInt32Bits(p),0x3f666666);Eq((double)p,0.8999999761581421d);Eq((double)p>.9,false);Eq(r.Grade(),RankGrade.B);using var e=JsonDocument.Parse(Read("re/evidence/42/rank-results.json"));var row=e.RootElement.GetProperty("reference_cases").EnumerateArray().Single(x=>x.GetProperty("id").GetString()=="R30");observations.Add(new{historical=row.Clone(),disposition="Historical descriptive conflict; binary32 instructions select B5; original x86 not executed"});});
        Add("G/N9-sentinel","E042 T0241 enum N=9; no M2b47 return9",()=>{Eq((int)RankGrade.N,9);Eq(OrdinaryRankGrade.Calculate(new Probe{Good=false}),RankGrade.F);});
    }
}
