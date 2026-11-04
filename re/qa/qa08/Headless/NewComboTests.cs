// QA08 independent E036/E035 finite assertions. Executes linked production only.
using System.Numerics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ohno.CircleGame.Gameplay;
namespace Ohno.QA08;

public static class NewComboTests
{
    static readonly List<(string Name, Action Body)> cases = [];
    static readonly List<object> results = [];
    static readonly List<object> inputs = [];
    static readonly List<object> observations = [];
    static string root = "", scratch = "";
    static void Add(string name, Action body) => cases.Add((name, body));
    static void Eq<T>(T actual, T expected)
    { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"expected={expected}; actual={actual}"); }
    static void Yes(bool value, string why) { if (!value) throw new Exception(why); }
    static void Near(double actual, double expected, double tolerance = 1e-10)
    { if (!double.IsFinite(actual) || Math.Abs(actual - expected) > tolerance) throw new Exception($"expected={expected:R}; actual={actual:R}; tolerance={tolerance}"); }
    static void Reject(Action body, string contains = "")
    {
        try { body(); } catch (InvalidDataException e) { Yes(e.Message.Contains(contains), e.Message); return; }
        throw new Exception("expected InvalidDataException: " + contains);
    }
    static void Seq<T>(IEnumerable<T> actual, params T[] expected)
    { Yes(actual.SequenceEqual(expected), $"expected=[{string.Join(",",expected)}]; actual=[{string.Join(",",actual)}]"); }
    static NewComboInput I(int raw, int time, object? id = null) => new(id ?? new object(),time,raw,
        (raw & 8) != 0 ? OriginalRegisteredType.SpinnerBase : (raw & 2) != 0 ? OriginalRegisteredType.Slider : OriginalRegisteredType.Circle);
    static IReadOnlyList<PreparedNewCombo> Prep(int[] raw, HealthBreak[]? breaks = null, int[]? starts = null, int version = 14) =>
        NewComboPreparation.Prepare(raw.Select((r,i)=>I(r,starts?[i] ?? 2000+i*1000)).ToArray(),breaks ?? [],version,NewComboContext.FreshZeroShift);
    static PreparedHealthObject C(int time, bool flag) => new(time,time,ScoreObjectKind.Circle,flag);
    static HealthChart Chart(bool first, bool second) => new(10,0,14,[C(2000,first),C(2100,second)],[]);
    static CircleSession Fresh(Beatmap map, bool hp = true) => CircleSession.CreateFreshOrdinary(map,ScoreConfiguration.FromBeatmap(map),hp);
    static HealthChart FreshChart(Beatmap map, int approach = 0) => HealthChart.FromMap(map,approach,HealthPreparationPolicy.E036FreshOrdinary);
    static string Row(int t, int raw) => $"128,192,{t},{raw},0" + ((raw&8)!=0 ? $",{t+500}" : (raw&2)!=0 ? ",L|228:192,1,100" : "");
    static Beatmap Map(string rows, string events = "", int hp = 0, int ar = 5, int version = 14)
    {
        string text=$"osu file format v{version}\n[General]\nMode:0\n[Metadata]\nTitle:QA08 finite control\n[Difficulty]\nHPDrainRate:{hp}\nCircleSize:5\nOverallDifficulty:5\nApproachRate:{ar}\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[Events]\n{events}\n[HitObjects]\n{rows}\n";
        string name=$"input-{inputs.Count:D3}.osu";
        inputs.Add(new { name,text });
        string path=Path.Combine(scratch,name);File.WriteAllText(path,text);return Beatmap.Load(path);
    }
    static Beatmap Map(int[] raw, string events = "", int hp = 0, int ar = 5) => Map(string.Join('\n',raw.Select((r,i)=>Row(2000+i*2000,r))),events,hp,ar);
    public static int Main(string[] args)
    {
        root=Path.GetFullPath(args.Single());scratch=Path.Combine(Path.GetTempPath(),"ohno-qa08-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(scratch);
        try
        {
            PredicateAndReferences(); Breaks(); GuardsAndProvenance(); Compatibility(); Calibration(); Integration(); Regressions();
            int failed=0;
            foreach(var c in cases)
            {
                inputs.Clear(); observations.Clear();
                try { c.Body(); Console.WriteLine("PASS "+c.Name);results.Add(new { c.Name,passed=true,inputs=inputs.ToArray(),observations=observations.ToArray() }); }
                catch(Exception e) { failed++;Console.WriteLine($"FAIL {c.Name}: {e}");results.Add(new { c.Name,passed=false,error=e.ToString(),inputs=inputs.ToArray(),observations=observations.ToArray() }); }
            }
            Console.WriteLine($"QA08 NEWCOMBO TESTS: {cases.Count-failed} passed, {failed} failed, {cases.Count} executed");
            Console.WriteLine("QA08 RESULTS "+JsonSerializer.Serialize(new { passed=cases.Count-failed,failed,executed=cases.Count,cases=results }));
            return failed==0 ? 0 : 1;
        }
        finally { Directory.Delete(scratch,true); }
    }
    static void PredicateAndReferences()
    {
        foreach(var x in new[]{(OriginalRegisteredType.Null,true),(OriginalRegisteredType.Circle,false),(OriginalRegisteredType.Slider,false),
            (OriginalRegisteredType.SpinnerBase,true),(OriginalRegisteredType.AssignableT081f,true)})
            Add($"P01 M2902 operand {x.Item1} (T081f artificial)",()=>Eq(NewComboPreparation.Infer(x.Item1),x.Item2));
        foreach(var x in new[]{OriginalRegisteredType.Unknown,(OriginalRegisteredType)999})
            Add($"P02 unknown runtime identity {x}",()=>Reject(()=>NewComboPreparation.Infer(x)));
        // Literal flags from E036 N01-N13, independent of production Infer/Prepare.
        var controls=new (string Name,int[] Raw,bool[] Flags)[]{
            ("N01 first circle",[1],[true]),("first slider",[2],[true]),
            ("N02 explicit following circle",[1,5],[true,true]),("N03 circles",[1,1],[true,false]),
            ("N04 circle-slider",[1,2],[true,false]),("N05 explicit slider",[1,6],[true,true]),
            ("N06 slider-circle",[2,1],[true,false]),("N07 sliders",[2,2],[true,false]),
            ("N08 alternating",[1,2,1,2],[true,false,false,false]),
            ("N09 unmarked spinner adjacency",[1,8,1],[true,false,true]),
            ("N10 marked spinner adjacency",[2,12,2],[true,true,true]),
            ("N11 consecutive spinners",[8,8,2],[false,false,true]),
            ("spinner then circle",[8,1],[false,true]),("spinner then slider",[8,2],[false,true]),
            ("marked spinner then circle",[12,1],[true,true]),("marked spinner then slider",[12,2],[true,true])};
        foreach(var x in controls) Add("P03 "+x.Name,()=>Seq(Prep(x.Raw).Select(p=>p.FinalFlag),x.Flags));
        foreach(int version in new[]{5,8,9,14}) foreach(int raw in new[]{8,12})
            Add($"P04 constructor/parser spinner v{version} type{raw} pure diagnostic",()=>{
                var p=Prep([raw,1],version:version);Eq(p[0].Inferred,(bool?)null);Eq(p[0].FactoryArgument,(bool?)null);
                Eq(p[0].ConstructorFlag,version<=8);Eq(p[0].ParserFlag,raw==12);Eq(p[0].FinalFlag,raw==12);
                Eq(p[1].Inferred,(bool?)true);Eq(p[1].LastRegisteredBefore,OriginalRegisteredType.SpinnerBase);
            });
        foreach(int raw in new[]{5,6}) Add($"P05 explicit bit still records helper result raw{raw}",()=>{
            var p=Prep([1,raw]);Eq(p[1].FileBit4,true);Eq(p[1].Inferred,(bool?)false);Eq(p[1].FactoryArgument,(bool?)true);
            Eq(p[1].ConstructorFlag,true);Eq(p[1].ParserFlag,true);
        });
        Add("P06 first marked row still evaluates null helper",()=>{var p=Prep([5]);Eq(p[0].Inferred,(bool?)true);Eq(p[0].FileBit4,true);});
    }
    static void Breaks()
    {
        foreach(int end in new[]{2999,3000,3001}) Add($"B01 strict break end {end}",()=>{
            var p=Prep([1,1,1],[new(2500,end)],[2000,3000,4000]);
            Eq(p[1].ExpiredBreaksConsumed,end<3000?1:0);Eq(p[1].FinalFlag,end<3000);
            Eq(p[2].ExpiredBreaksConsumed,end<3000?0:1);Eq(p[2].FinalFlag,end>=3000);
        });
        Add("B02 N17 multiple expired breaks force one object",()=>{
            var p=Prep([1,1],[new(2100,2200),new(2300,2400)],[2000,3000]);
            Eq(p[1].ExpiredBreaksConsumed,2);Eq(p[1].ParserFlag,false);Eq(p[1].FinalFlag,true);
        });
        Add("B03 preserve explicit true without break",()=>{var p=Prep([1,5]);Eq(p[1].FinalFlag,true);Eq(p[1].ExpiredBreaksConsumed,0);});
        Add("B04 supplied break order unexpired head blocks later expired entry",()=>{
            var p=Prep([1,1,1],[new(2100,4500),new(2200,2500)],[2000,3000,5000]);
            Eq(p[1].ExpiredBreaksConsumed,0);Eq(p[1].FinalFlag,false);Eq(p[2].ExpiredBreaksConsumed,2);Eq(p[2].FinalFlag,true);
        });
        Add("B05 reversed supplied break order changes forcing",()=>{
            var p=Prep([1,1,1],[new(2200,2500),new(2100,4500)],[2000,3000,5000]);
            Eq(p[1].ExpiredBreaksConsumed,1);Eq(p[1].FinalFlag,true);Eq(p[2].ExpiredBreaksConsumed,1);
        });
        Add("B06 N16 spinner flag can be forced in postpass",()=>{
            var p=Prep([1,8,1],[new(2100,2500)],[2000,3000,5000]);Eq(p[1].ParserFlag,false);Eq(p[1].FinalFlag,true);Eq(p[2].Inferred,(bool?)true);
        });
        Add("B07 every expired break forces independently",()=>{
            foreach(var b in new[]{new HealthBreak(2100,2200),new HealthBreak(2300,2400)})
            {var p=Prep([1,1],[b],[2000,3000]);Eq(p[1].ExpiredBreaksConsumed,1);Eq(p[1].FinalFlag,true);}
        });
        Add("B08 E036 while differs from E035 one-break gap exclusion",()=>{
            var m=Map(Row(2000,1)+"\n"+Row(3000,1),"2,2100,2200\n2,2300,2400",hp:10);var chart=FreshChart(m);
            Eq(chart.NewComboProvenance[1].ExpiredBreaksConsumed,2);Eq(chart.Objects[1].NewCombo,(bool?)true);
            var c=new HealthCalibration();Eq(c.Trial(chart),HealthRetry.None);Eq(c.LastBreaksConsumed,1);Eq(c.LastExcludedGap,200);Near(c.LastGapLoss,40);
            // first +20; second gap loss40 then +20: H180,A200,mean0.
            Near(c.Snapshots[0],200);Near(c.Snapshots[1],180);Near(c.LastA,200);
        });
    }
    static void GuardsAndProvenance()
    {
        foreach(var origin in new[]{NewComboOrigin.RetainedManager,NewComboOrigin.CloneOrEditor,NewComboOrigin.AlternateFactory,NewComboOrigin.Unknown})
            Add($"G01 unsupported context {origin}",()=>Reject(()=>NewComboPreparation.Prepare([I(1,2000)],[],14,new(origin,OriginalRegisteredType.Null,true))));
        foreach(var last in new[]{OriginalRegisteredType.Circle,OriginalRegisteredType.Slider,OriginalRegisteredType.SpinnerBase,OriginalRegisteredType.AssignableT081f,OriginalRegisteredType.Unknown})
            Add($"G02 retained initial reference {last}",()=>Reject(()=>NewComboPreparation.Prepare([I(5,2000)],[],14,new(NewComboOrigin.FreshOrdinaryText,last,true))));
        Add("G03 timestamp domain unknown",()=>Reject(()=>NewComboPreparation.Prepare([I(1,2000)],[],14,new(NewComboOrigin.FreshOrdinaryText,OriginalRegisteredType.Null,false))));
        foreach(int second in new[]{2000,1999}) Add($"G04 source order rejects second {second}",()=>Reject(()=>Prep([1,1],starts:[2000,second]),"increasing"));
        Add("G05 duplicate exact identity rejected",()=>{object id=new();Reject(()=>NewComboPreparation.Prepare([I(1,2000,id),I(1,3000,id)],[],14,NewComboContext.FreshZeroShift));});
        Add("G06 missing identity rejected",()=>Reject(()=>NewComboPreparation.Prepare([new(null!,2000,1,OriginalRegisteredType.Circle)],[],14,NewComboContext.FreshZeroShift)));
        foreach(var type in new[]{OriginalRegisteredType.AssignableT081f,OriginalRegisteredType.Unknown,OriginalRegisteredType.SpinnerBase})
            Add($"G07 circle raw with alternate runtime {type}",()=>Reject(()=>NewComboPreparation.Prepare([new(new object(),2000,1,type)],[],14,NewComboContext.FreshZeroShift)));
        foreach(int raw in new[]{3,9,128,0}) Add($"G08 unsupported mixed/hold raw {raw}",()=>Reject(()=>Prep([raw])));
        Add("G09 all provenance operands and exact identity",()=>{
            var a=I(1|112,2000);var b=I(6|48,3000);var c=I(8,4000);var d=I(1,5000);
            var p=NewComboPreparation.Prepare([a,b,c,d],[new(3100,3500)],14,NewComboContext.FreshZeroShift);
            var src=new[]{a,b,c,d};
            for(int i=0;i<4;i++){Eq(p[i].SourceIndex,i);Yes(ReferenceEquals(p[i].Identity,src[i].Identity),"exact definition identity");Eq(p[i].Start,src[i].Start);Eq(p[i].RawType,src[i].RawType);Eq(p[i].RuntimeType,src[i].RuntimeType);}
            Seq(p.Select(x=>x.FileColourSkip),7,3,0,0);Seq(p.Select(x=>x.FileBit4),false,true,false,false);
            Seq(p.Select(x=>x.LastRegisteredBefore),OriginalRegisteredType.Null,OriginalRegisteredType.Circle,OriginalRegisteredType.Slider,OriginalRegisteredType.SpinnerBase);
            Seq(p.Select(x=>x.Inferred),(bool?)true,false,null,true);Seq(p.Select(x=>x.FactoryArgument),(bool?)true,true,null,true);
            Seq(p.Select(x=>x.ConstructorFlag),true,true,false,true);Seq(p.Select(x=>x.ParserFlag),true,true,false,true);
            Seq(p.Select(x=>x.ExpiredBreaksConsumed),0,0,1,0);Seq(p.Select(x=>x.FinalFlag),true,true,true,true);
        });
        Add("G10 structurally equal distinct identities stay distinct",()=>{
            object a=new CircleDefinition(new(1,1),2000),b=new CircleDefinition(new(1,1),2000);
            var p=NewComboPreparation.Prepare([I(1,2000,a),I(1,3000,b)],[],14,NewComboContext.FreshZeroShift);Eq(p.Count,2);Yes(!ReferenceEquals(p[0].Identity,p[1].Identity),"reference identity");
        });
        Add("G11 parser chart identities correspond C/L/P/C",()=>{
            var m=Map([1,2,8,1]);var c=FreshChart(m);var p=c.NewComboProvenance;
            Yes(ReferenceEquals(p[0].Identity,m.Circles[0]),"circle");Yes(ReferenceEquals(p[1].Identity,m.Sliders[0]),"slider parent");
            Yes(ReferenceEquals(p[2].Identity,m.Spinners[0]),"spinner");Yes(ReferenceEquals(p[3].Identity,m.Circles[1]),"last circle");
            Seq(c.Objects.Select(o=>o.NewCombo),(bool?)true,false,false,true);
            Seq(p.Select(o=>o.SourceIndex),0,1,2,3);
        });
        Add("G12 clone map rejected even same property references",()=>{var m=Map([1,1]);var clone=m with { Title="clone" };Yes(m.HealthSource!.IsFreshFor(m),"fresh owner");Eq(m.HealthSource.IsFreshFor(clone),false);Reject(()=>FreshChart(clone),"ownership");});
        Add("G13 manually assembled with captured metadata rejected",()=>{
            var m=Map([1]);var synthetic=new Beatmap(m.Title,5,5,5,m.Circles){ScoreMetadata=m.ScoreMetadata,HealthSource=m.HealthSource};Reject(()=>FreshChart(synthetic),"ownership");
        });
        Add("G14 manual map without provenance remains playable",()=>{
            var m=new Beatmap("synthetic",5,5,5,[new(new(128,192),2000)]);var s=CircleSession.CreateFreshOrdinary(m,ScoreConfiguration.EngineeringPractice);
            Eq(s.Health,(OrdinaryHealth?)null);Yes(s.HealthStatus.Contains("Missing"),s.HealthStatus);s.Press(new(128,192),2000);Eq(s.Combo,1);
        });
        Add("G15 out of order parser cannot silently repair fresh HP",()=>{
            var m=Map(Row(4000,1)+"\n"+Row(2000,1));Eq(m.Circles[0].StartTime,2000);Seq(m.HealthSource!.Rows.Select(x=>x.Start),4000,2000);
            var s=Fresh(m);Eq(s.Health,(OrdinaryHealth?)null);Yes(s.HealthStatus.Contains("increasing"),s.HealthStatus);s.Press(new(128,192),2000);Eq(s.Combo,1);
        });
        Add("G16 equal-start parser rejects without stable sort",()=>Reject(()=>Map(Row(2000,1)+"\n"+Row(2000,2)),"equal-time"));
        Add("G17 unsorted break pure control does not grant app admission",()=>{
            var m=Map(Row(2000,1)+"\n"+Row(6000,1),"2,4000,5000\n2,2500,3000");Eq(Fresh(m).Health,(OrdinaryHealth?)null);
        });
        Add("G18 older spinner pure preparation does not grant gameplay",()=>Reject(()=>Map(Row(2000,8),version:8),"format-v14"));
    }
    static void Compatibility()
    {
        Add("A01 historical constructors retain old admission",()=>{
            var m=Map([1,1]);var scoring=ScoreConfiguration.FromBeatmap(m);
            foreach(var s in new[]{new CircleSession(m),new CircleSession(m,scoring),new CircleSession(m,scoring,true)})
            {Eq(s.HealthPolicy,HealthPreparationPolicy.HistoricalR09);Eq(s.Health,(OrdinaryHealth?)null);Yes(s.HealthStatus.Contains("M2902"),s.HealthStatus);}
        });
        Add("A02 two-argument FromMap retains historical unknown group rejection",()=>{var m=Map([1,1]);Reject(()=>HealthChart.FromMap(m,1200),"M2902");});
        Add("A03 historical explicit-group map remains supported",()=>{var m=Map([1,5]);var c=HealthChart.FromMap(m,1200);Eq(c.PreparationPolicy,HealthPreparationPolicy.HistoricalR09);Eq(c.NewComboProvenance.Count,0);Eq(new CircleSession(m).Health is not null,true);});
        Add("A04 fresh factory selects E036 and preserves metadata into HP",()=>{
            var m=Map([1,1]);var s=Fresh(m);Yes(s.Health is not null,s.HealthStatus);Eq(s.HealthPolicy,HealthPreparationPolicy.E036FreshOrdinary);
            Eq(s.Health!.Chart.PreparationPolicy,HealthPreparationPolicy.E036FreshOrdinary);Seq(s.Health.Chart.Objects.Select(x=>x.NewCombo),(bool?)true,false);Eq(s.Health.Chart.NewComboProvenance.Count,2);
        });
        Add("A05 unsupported fresh ownership never falls back to historical",()=>{
            var m=Map([1,5]);var clone=m with { Title="clone" };Yes(new CircleSession(clone).Health is not null,"historical contract remains");
            var s=Fresh(clone);Eq(s.Health,(OrdinaryHealth?)null);Eq(s.HealthPolicy,HealthPreparationPolicy.E036FreshOrdinary);Yes(s.HealthStatus.Contains("ownership"),s.HealthStatus);
        });
        Add("A06 production application Load calls fresh factory",()=>CheckAppMethod("Load","CircleSession.CreateFreshOrdinary(map, scoring)"));
        Add("A07 production application Restart calls fresh factory",()=>CheckAppMethod("Restart","CircleSession.CreateFreshOrdinary(Session.Map, Session.Scoring)"));
    }
    static void CheckAppMethod(string method,string call)
    {
        string source=File.ReadAllText(Path.Combine(root,"reconstructed/CircleGame/Rendering/PlayfieldView.cs"));
        var match=Regex.Match(source,@"public void "+method+@"\([^)]*\)\s*\{");Yes(match.Success,"entry exists");
        int start=match.Index+match.Length,depth=1,end=start;
        while(depth>0 && end<source.Length){if(source[end]=='{')depth++;if(source[end]=='}')depth--;end++;}
        string body=source[start..(end-1)];Yes(body.Contains(call),method+" factory binding");Yes(!body.Contains("new CircleSession"),"no historical construction");Yes(body.Contains("Session = replacement"),"factory result assigned");
        observations.Add(new { method,call,classification="SOURCE BINDING; WPF entry not executed" });
    }
    static void Calibration()
    {
        foreach(bool first in new[]{false,true}) foreach(bool next in new[]{false,true}) Add($"C01 next-property first{first} next{next}",()=>{
            var c=new HealthCalibration();Eq(c.Trial(Chart(first,next)),HealthRetry.None);Near(c.LastA,next?235:221);Near(c.LastH,200);Near(c.Snapshots[0],200);Near(c.Snapshots[1],200);
        });
        foreach(bool first in new[]{false,true}) Add($"C02 last object always group first{first}",()=>{
            var c=new HealthCalibration();Eq(c.Trial(new(10,0,14,[C(2000,first)],[])),HealthRetry.None);Near(c.LastA,220);
        });
        Add("C03 explicit bit affects preceding object balance",()=>{
            foreach(int raw in new[]{1,5}){var m=Map(Row(2000,1)+"\n"+Row(2100,raw),hp:10);var c=new HealthCalibration();Eq(c.Trial(FreshChart(m)),HealthRetry.None);Near(c.LastA,raw==5?235:221);}
        });
        Add("C04 break forcing affects preceding object before calibration",()=>{
            var m=Map(Row(2000,1)+"\n"+Row(2100,1),"2,2010,2020",hp:10);var chart=FreshChart(m);Eq(chart.NewComboProvenance[1].ParserFlag,false);Eq(chart.Objects[1].NewCombo,(bool?)true);
            var c=new HealthCalibration();Eq(c.Trial(chart),HealthRetry.None);Near(c.LastA,236); // 200+20 - .05*(100-20) +20
        });
        Add("C05 break-end equality excludes gap but does not force NewCombo",()=>{
            var m=Map(Row(2000,1)+"\n"+Row(2100,1),"2,2010,2100",hp:10);var chart=FreshChart(m);Eq(chart.Objects[1].NewCombo,(bool?)false);
            var c=new HealthCalibration();Eq(c.Trial(chart),HealthRetry.None);Eq(c.LastExcludedGap,100);Near(c.LastA,226);
        });
        Add("C06 five-circle unmarked closed balance oracle",()=>Five(false));
        Add("C07 five-circle explicit groups closed balance oracle",()=>Five(true));
    }
    static void Five(bool marked)
    {
        var m=Map(string.Join('\n',Enumerable.Range(0,5).Select(i=>Row(2000+100*i,marked?5:1))));var s=Fresh(m);Yes(s.Health is not null,s.HealthStatus);
        var chart=s.Health!.Chart;var c=new HealthCalibration();double rate=.05;
        for(int k=0;k<61;k++){Eq(c.Trial(chart),HealthRetry.PreObject);rate*=.96;Near(c.Rate,rate,1e-17);Eq(c.B,1.0);Eq(c.G,1.0);}
        double b=1,g=1;
        for(int k=0;k<=(marked?0:4);k++)
        {
            // Independent closed sum: 1600ms loss, four ordinary6G + final(14B+6G),
            // or five(14B+6G). All snapshots cap200, so no low-group/end retries.
            double expectedA=200-1600*rate+(marked?70*b+30*g:14*b+30*g);
            var retry=c.Trial(chart);Near(c.LastA,expectedA);Near(c.LastMean,(expectedA-200)/5);Near(c.LastH,200);Seq(c.Snapshots,200d,200d,200d,200d,200d);
            observations.Add(new { trial=62+k,rate,b,g,expectedA,actualA=c.LastA,retry=retry.ToString() });
            bool accept=marked || k==4;Eq(retry,accept?HealthRetry.None:HealthRetry.Balance);
            if(!accept){Yes((expectedA-200)/5<8,"below W8");rate*=.96;b*=1.02;g*=1.01;}
            else Yes((expectedA-200)/5>=8,"accept W8");
        }
        Eq(c.Trials,marked?62:66);Near(c.Rate,rate,1e-17);Near(c.B,b);Near(c.G,g);
        Near(s.Health.Calibration.Rate,rate,1e-17);Near(s.Health.Calibration.B,b);Near(s.Health.Calibration.G,g);Eq(s.Health.Calibration.Trials,c.Trials);
        Eq(s.Ledger.EventCount,0);Eq(s.Ledger.Score,0);Eq(s.Ledger.TotalWholeResults,0);Eq(s.Combo,0);Eq(s.Health.ResultEvents,0);
        Near(s.Health.State.H,200);Near(s.Health.State.A,200);Near(s.Health.State.V,0);
    }
    static void Integration()
    {
        foreach(var raw in new[]{new[]{1,1,1},new[]{1,2,1},new[]{2,1,2},new[]{1,8,1},new[]{2,12,2}})
            Add("I01 fresh parsed kinds "+string.Join("-",raw),()=>{
                var m=Map(raw);var s=Fresh(m);Yes(s.Health is not null,s.HealthStatus);Eq(s.Health!.Chart.NewComboProvenance.Count,raw.Length);Eq(s.Health.ResultEvents,0);Eq(s.Ledger.EventCount,0);
            });
        Add("I02 unchanged bundled eight-circle fixture health eligible",()=>{
            var m=Beatmap.Load(Path.Combine(root,"reconstructed/CircleGame/Fixtures/first-circles.osu"));Eq(m.Circles.Count,8);var s=Fresh(m);Yes(s.Health is not null,s.HealthStatus);
            Seq(s.Health!.Chart.Objects.Select(o=>o.NewCombo),(bool?)true,false,false,false,false,true,false,false);
        });
        Add("I03 separately tracked three-circle input eligible",()=>{
            var m=Beatmap.Load(Path.Combine(root,"re/qa/qa08/Inputs/three-unmarked.osu"));Eq(m.Circles.Count,3);Yes(Fresh(m).Health is not null,"new unmarked chart");
        });
        Add("I04 supported break chart final group propagation",()=>{
            var m=Map([1,1,2],"2,2500,3000");var s=Fresh(m);Yes(s.Health is not null,s.HealthStatus);Seq(s.Health!.Chart.Objects.Select(o=>o.NewCombo),(bool?)true,true,false);
        });
        Add("I05 restart creates fresh projection after final slider",()=>{
            var m=Map([1,2]);var a=Fresh(m);a.Health!.State.Assign(0);a.Health.CheckFailure(12);a.Press(new(128,192),2000);
            var b=CircleSession.CreateFreshOrdinary(a.Map,a.Scoring);Yes(!ReferenceEquals(a.Health,b.Health),"new health");Eq(b.Health!.Failed,false);Eq(b.Health.ResultEvents,0);Eq(b.Ledger.EventCount,0);Eq(b.Heads[0].Result,Judgement.Pending);
            Eq(b.Health.Chart.NewComboProvenance[0].LastRegisteredBefore,OriginalRegisteredType.Null);Near(b.Health.State.H,200);Near(b.Health.Calibration.Rate,a.Health.Calibration.Rate);
        });
        Add("I06 replacement supported-unavailable-supported isolates state",()=>{
            var good=Map([1,1]);var a=Fresh(good);a.Health!.State.Assign(0);a.Health.CheckFailure(1);var bad=Fresh(good with {Title="clone"});Eq(bad.Health,(OrdinaryHealth?)null);
            var c=Fresh(Map([2,1]));Eq(c.Health!.Failed,false);Eq(c.Health.ResultEvents,0);Eq(c.Ledger.Score,0);Near(c.Health.State.H,200);Eq(c.Health.Chart.NewComboProvenance[0].LastRegisteredBefore,OriginalRegisteredType.Null);
        });
        Add("I07 unsupported fresh clone keeps slider and spinner scoring",()=>{
            var m=Map([2,8,1]);var clone=m with {Title="no ownership"};var a=Fresh(clone);var b=Fresh(clone,false);Eq(a.Health,(OrdinaryHealth?)null);
            for(int t=0;t<=6500;t+=100){a.Advance(t);b.Advance(t);Same(a,b);}
            Eq(a.JudgedCount,3);Eq(a.Ledger.NMiss,(ushort)2);Eq(a.Ledger.N50,(ushort)1);
            Eq(a.Spinners[0].Required,2);Eq(a.Spinners[0].Result,Judgement.Hit50); // E026 floor(2/4)=0; 0>2-2 is false.
        });
    }
    static void Regressions()
    {
        Add("R00 E026 short spinner zero observations N2 is50",()=>Eq(SpinnerState.ClassifyModern(0,2),256));
        Add("R00 E026 zero observations N4 ismiss",()=>Eq(SpinnerState.ClassifyModern(0,4),-131072));
        foreach(int[] raw in new[]{new[]{1,1,1},new[]{1,2,8,1},new[]{2,12,2}}) Add("R01 enabled-disabled component sequence "+string.Join("-",raw),()=>{
            var m=Map(raw);var a=Fresh(m);var b=Fresh(m,false);Yes(a.Health is not null,a.HealthStatus);
            for(int t=0;t<=10000;t+=20)
            {
                Vector2 pointer=new(128,192);int held=0;var slider=m.Sliders.FirstOrDefault(x=>t>=x.Head.StartTime&&t<=x.EndTime);
                if(slider is not null){pointer=slider.RequiredPosition(t);held=1;}
                foreach(var c in m.Circles.Where(c=>c.StartTime==t)){a.Press(c.Position,t);b.Press(c.Position,t);}
                if(slider?.Head.StartTime==t){a.Press(pointer,t);b.Press(pointer,t);}
                // Explicit component operands, no OS input/window. Spinner receives held angular samples.
                Vector2 spin=new(256+100*(float)Math.Cos(t*.02),192+100*(float)Math.Sin(t*.02));
                if(m.Spinners.Any(s=>t>=s.StartTime&&t<=s.EndTime))held=1;
                a.Advance(t,pointer,held,spin);b.Advance(t,pointer,held,spin);Same(a,b);
            }
            Eq(a.Health!.ResultEvents,a.Ledger.EventCount);Eq(a.JudgedCount,raw.Length);
            if(raw.All(x=>(x&~4)==1))Eq(a.Ledger.EventCount,raw.Length);else Yes(a.Ledger.EventCount>raw.Length,"actual component events");
        });
        Add("R02 explicit groups change HP coefficients without changing E034 K",()=>{
            var a=Fresh(Map(string.Join('\n',Enumerable.Range(0,5).Select(i=>Row(2000+100*i,1)))));
            var b=Fresh(Map(string.Join('\n',Enumerable.Range(0,5).Select(i=>Row(2000+100*i,5)))));
            Eq(a.Scoring.K,b.Scoring.K);Yes(a.Health!.State.B!=b.Health!.State.B,"grouping changes B");
            foreach(int t in Enumerable.Range(0,5).Select(i=>2000+100*i)){a.Press(new(128,192),t);b.Press(new(128,192),t);Same(a,b);}
        });
        foreach(double h in new[]{159.999,160,200}) Add($"R03 R09-C01 recovery threshold H{h}",()=>{
            var s=Fresh(Map([1]));var health=s.Health!;health.State.Assign(h);health.State.ConfigureFill(123,.03,false);health.SupplyRecoveryCount(1);health.BeginRecovery();
            Near(health.State.H,h<160?200:h);Near(health.State.A,h<160?h+160:h);Eq(health.State.Fill,h<160);Near(health.State.FillRate,h<160?.07999999821186066:.03);Eq(health.State.FillFrom,123);Eq(health.RecoveryCount,0);Eq(s.Ledger.EventCount,0);
        });
        Add("R04 E035 fresh HP delta unchanged",()=>{
            var h=Fresh(Map([1])).Health!;h.State.Assign(100);h.ApplyResultHp(1024,true);Near(h.State.H,100+6*h.State.G);
            h.State.Assign(100);h.ApplyResultHp(-262144,true);Near(h.State.H,96);Eq(h.ResultEvents,2);
        });
        Add("R05 E035 prior eligibility drain and smoothing",()=>{
            var h=Fresh(Map([1,1])).Health!;var e=new HealthEligibility(2000,2000,2000,4000,1200,-1,1,14);
            h.State.Assign(100);h.State.SetPresentation(100);h.State.ConfigureFill(0,.02,false);
            h.Frame(new(2000,1990,0,0,0,1,false,false,false,e));Near(h.State.H,100); // old eligible false
            h.Frame(new(2010,2000,0,0,1,1,false,false,false,e with {Time=2010}));
            double expected=100-h.State.Rate*10;Near(h.State.H,expected);Near(h.State.V,100-(100-expected)/6);
        });
    }
    static void Same(CircleSession a,CircleSession b)
    {
        Eq(a.Ledger.Score,b.Ledger.Score);Eq(a.Scoring.K,b.Scoring.K);Eq(a.Combo,b.Combo);Eq(a.MaxCombo,b.MaxCombo);Eq(a.Ledger.ComboSnapshot,b.Ledger.ComboSnapshot);
        Eq(a.Ledger.N300,b.Ledger.N300);Eq(a.Ledger.N100,b.Ledger.N100);Eq(a.Ledger.N50,b.Ledger.N50);Eq(a.Ledger.NMiss,b.Ledger.NMiss);
        Eq(a.Accuracy,b.Accuracy);Eq(a.JudgedCount,b.JudgedCount);Eq(a.Ledger.EventCount,b.Ledger.EventCount);Eq(a.Ledger.LastEvent,b.Ledger.LastEvent);Eq(a.Ledger.LastOutcome,b.Ledger.LastOutcome);
        Yes(a.ObjectResults.SequenceEqual(b.ObjectResults),"judgement equality");
        for(int i=0;i<a.Sliders.Count;i++){Eq(a.Sliders[i].ProcessedCheckpoints,b.Sliders[i].ProcessedCheckpoints);Seq(a.Sliders[i].Checkpoints.Select(x=>x.RawResult),b.Sliders[i].Checkpoints.Select(x=>x.RawResult).ToArray());}
        for(int i=0;i<a.Spinners.Count;i++){Eq(a.Spinners[i].Progress,b.Spinners[i].Progress);Eq(a.Spinners[i].Observation.Count,b.Spinners[i].Observation.Count);}
    }
}
