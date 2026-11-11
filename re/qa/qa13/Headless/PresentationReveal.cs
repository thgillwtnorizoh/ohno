using System.Globalization;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA13;
public static partial class Tests
{
    static void Presentation()
    {
        foreach(var (score,text) in new[]{(0,"00000000"),(123,"00000123"),(123456789,"123456789"),(int.MaxValue,"2147483647")})Add("V/score-format-"+score,"E042 M32d7 minimum eight digits",()=>{var r=new Record{Numeric=score,Success=true};Eq(RankPresentation.Capture(r).Score,text);Eq(r.Score,score);});
        Add("V/display-override-only","E042 M32d7 nullable score branch",()=>{var r=new Record{Numeric=123456789,Success=true};var p=RankPresentation.Capture(r,scoreOverride:123);Eq(p.Score,"00000123");Eq(r.Score,123456789);Eq(r.Grade(),RankGrade.X);});
        foreach(var (n300,n100,n50,miss,text) in new[]{(1,0,0,0,"100.00%"),(1,1,0,0,"66.67%"),(1,0,1,0,"58.33%"),(9,0,0,1,"90.00%"),(0,0,0,0,"100.00%")})
            Add($"V/accuracy-{n300}-{n100}-{n50}-{miss}","E033 M2b42; E042 M32d7:09bc *100f",()=>{var r=new Record{C300=(ushort)n300,C100=(ushort)n100,C50=(ushort)n50,Miss=(ushort)miss,Success=true};var p=RankPresentation.Capture(r);Eq(p.Accuracy,text);Eq(r.WholeTotal,n300+n100+n50+miss);});
        foreach(string culture in new[]{"fr-FR","th-TH","en-US"})Add("V/invariant-engineering-"+culture,"Original CurrentCulture vs explicit ENGINEERING invariant policy",()=>{var previous=CultureInfo.CurrentCulture;try{CultureInfo.CurrentCulture=CultureInfo.GetCultureInfo(culture);var p=RankPresentation.Capture(new Record{C300=1,C100=1,Numeric=123,Success=true});Eq(p.Accuracy,"66.67%");Eq(p.Score,"00000123");}finally{CultureInfo.CurrentCulture=previous;}});
        Add("V/counters-maxcombo","M32d7 whole counters and F1975 maximum",()=>{var r=new Record{C300=31,C100=7,C50=2,Miss=4,Max=123,Success=true};var p=RankPresentation.Capture(r);Eq(p.N300,(ushort)31);Eq(p.N100,(ushort)7);Eq(p.N50,(ushort)2);Eq(p.NMiss,(ushort)4);Eq(p.MaximumCombo,123);Yes(ReferenceEquals(p.RecordIdentity,r.SessionIdentity),"record identity retained");});
        Add("V/map-fields","E042 header sources; parser sidecar engineering",()=>{var r=new Record{Data=new("Distinct title","Distinct version","Distinct artist"),Success=true};var p=RankPresentation.Capture(r);Eq(p.Map.Title,"Distinct title");Eq(p.Map.Difficulty,"Distinct version");Eq(p.Map.Artist,"Distinct artist");});
        Add("V/missing-map","E042 null metadata dependency; R15 bounded rejection",()=>{var r=new Record{Data=null,Success=true};Throws<InvalidOperationException>(()=>RankPresentation.Capture(r));});
        Add("V/capture-freezes-fields","E042 captured F1daf vs retained F1da9; ENGINEERING DTO",()=>{var r=new Record{Numeric=99,Max=17,Success=true};var p=RankPresentation.Capture(r);r.Numeric=44;r.C300=0;r.C50=10;r.Max=99;r.Success=false;r.Valid=false;r.Perfect=true;Eq(p.Score,"00000099");Eq(p.N300,(ushort)10);Eq(p.MaximumCombo,17);Eq(p.Success,true);Eq(p.Valid,true);Eq(p.Perfect,null);Eq(p.Grade.First,RankGrade.X);Eq(r.Grade(),RankGrade.F);});
        foreach(var grade in new[]{RankGrade.X,RankGrade.XH,RankGrade.S,RankGrade.SH,RankGrade.A,RankGrade.B,RankGrade.C,RankGrade.D,RankGrade.F})
            Add("V/resource-"+grade,"M32da ranking- + enum.ToString; repeated grade reads",()=>{var r=ForGrade(grade);var k=RankGradeResources.Read(()=>r);Eq(k.First,grade);Eq(k.FirstKey,"ranking-"+grade);Eq(k.Eligibility,grade);Eq(k.SecondaryKey,(int)grade<6?"ranking-"+grade:null);});
        Add("V/second-grade-read","M32da:0011/00fe independent reads",()=>{var r=ForGrade(RankGrade.X);var reads=new List<int>();var k=RankGradeResources.Read(()=>r,i=>{reads.Add(i);if(i==1)r.Success=false;});Eq(k.FirstKey,"ranking-X");Eq(k.Eligibility,RankGrade.F);Eq(k.SecondaryKey,null);Seq(reads,[1,2]);});
        Add("V/third-grade-read","M32da:0119 fresh getter after eligible second",()=>{var r=ForGrade(RankGrade.X);var reads=new List<int>();var k=RankGradeResources.Read(()=>r,i=>{reads.Add(i);if(i==2){r.C300=0;r.C100=10;}});Eq(k.FirstKey,"ranking-X");Eq(k.Eligibility,RankGrade.X);Eq(k.SecondaryKey,"ranking-D");Seq(reads,[1,2,3]);});
        Add("V/resource-selector-reference-reread","M32da fresh F1da9 per getter; artificial alias control",()=>{var r=ForGrade(RankGrade.X);var k=RankGradeResources.Read(()=>r,i=>{if(i==1)r=ForGrade(RankGrade.C);});Eq(k.First,RankGrade.X);Eq(k.Eligibility,RankGrade.C);Eq(k.SecondaryKey,null);});
        Add("V/accuracy-not-grade-input","M2b42 formatting does not select M2b47",()=>{var r=new Record{C300=9,Miss=1,Success=true};var p=RankPresentation.Capture(r);Eq(p.Accuracy,"90.00%");Eq(p.Grade.First,RankGrade.B);});
    }
    static Record ForGrade(RankGrade g)=>g switch
    {
        RankGrade.X=>new(){Success=true},RankGrade.XH=>new(){Success=true,Mods=8},
        RankGrade.S=>new(){C300=99,C100=1,Success=true},RankGrade.SH=>new(){C300=99,C100=1,Success=true,Mods=1024},
        RankGrade.A=>new(){C300=80,C100=20,Success=true},RankGrade.B=>new(){C300=75,C100=25,Success=true},
        RankGrade.C=>new(){C300=70,C100=30,Success=true},RankGrade.D=>new(){C300=1,C100=9,Success=true},
        _=>new(){Success=false}
    };
    static void Reveal()
    {
        foreach(var (time,text) in new[]{(799,"········"),(800,"········"),(1299,"········"),(1300,"········"),(1799,"········"),(1800,"1·······"),(2300,"12······"),(5299,"1234567·"),(5300,"12345678"),(6300,"12345678")})
            Add("R/prefix-"+time,"E042 M32d4 signed elapsed/500; dots ENGINEERING",()=>{var r=new RankScoreReveal("12345678",1000);Eq(r.Start,1300);Eq(r.Text(time),text);Eq(r.CapturedScore,"12345678");Eq(r.Skipped,false);});
        foreach(int now in new[]{1300,6299,6300,6301})Add("R/skip-"+now,"M32e7 elapsed<5000 versus equality",()=>{var r=new RankScoreReveal("12345678",1000);Eq(r.Skip(now),now>=6300);Eq(r.Skipped,now<6300);Eq(r.Start,now<6300?now-7000:1300);if(now<6300){Eq(r.Text(now),"12345678");Eq(r.Skip(now),true);}});
        Add("R/overflow-start","M32db +300 unchecked; M32d4 signed difference",()=>{var r=new RankScoreReveal("98765432",int.MaxValue-10);Eq(r.Start,unchecked(int.MaxValue-10+300));Eq(r.Text(unchecked(r.Start+500)),"9·······");});
        Add("R/separate-host-clock","E042 host F2c3a distinct gameplay F2358",()=>{using var f=Flow();Enter(f);Eq(f.Reveal!.Start,1300);f.Observe(999999);Eq(f.Reveal.Text(1300),"········");Eq(f.HandlerRecord.Score,0);});
        Add("R/skip-does-not-depart","M32d1 Space/Enter discard Boolean",()=>{using var f=Flow();Enter(f);var record=f.SelectedRecord;f.Skip(1300);f.Skip(1300);Eq(f.Transitions.Pending,null);Eq(f.Stage,RankStage.ResultsPresented);Yes(ReferenceEquals(record,f.SelectedRecord),"no replacement");});
    }
}
