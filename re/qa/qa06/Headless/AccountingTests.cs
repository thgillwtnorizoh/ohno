// Explicit, noninteractive calls to production classes. No replacement gameplay engine.
using System.Numerics;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Rendering;
namespace Ohno.QA06;

public static class AccountingTests
{
    private static readonly List<(string Name, Action Body)> Cases = [];
    private static string root = "", scratch = "", lastInput = "";
    private static int file;
    private static void Add(string name, Action body) => Cases.Add((name, body));
    private static void Eq<T>(T actual, T expected)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            throw new InvalidOperationException($"expected={expected}; actual={actual}");
    }
    private static void Yes(bool value, string reason) { if (!value) throw new InvalidOperationException(reason); }
    private static void Reject<T>(Action body) where T : Exception
    { try { body(); } catch (T) { return; } throw new InvalidOperationException("expected " + typeof(T).Name); }
    private static LegacyScoreMetadata Meta(string rows, string difficulty = OracleVectors.Standard, string events = "", int version = 14)
    {
        lastInput = $"[Difficulty]\n{difficulty}\n[Events]\n{events}\n[HitObjects]\n{rows}";
        return LegacyScoreMetadata.ReadFresh(lastInput.Split('\n'), version);
    }
    private static Beatmap Map(string rows, string difficulty = "HPDrainRate:0\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5", string events = "")
    {
        lastInput = $"osu file format v14\n[General]\nMode:0\n[Difficulty]\n{difficulty}\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,1000,4,1,0,100,1,0\n[Events]\n{events}\n[HitObjects]\n{rows}\n";
        string path = Path.Combine(scratch, $"map{file++}.osu"); File.WriteAllText(path,lastInput); return Beatmap.Load(path);
    }
    private static ScoreLedger Ledger(double k = 1) => new(new(k,ScorePolicy.PlainLegacy,"QA06 explicit finite control"));
    private static ScoreEvent Event(ScoreEventKind kind, int raw, SliderCheckpointKind? role = null, bool enabled = true)
    {
        ScoreObjectKind source = kind switch { ScoreEventKind.CircleWhole => ScoreObjectKind.Circle,
            ScoreEventKind.SpinnerFinal or ScoreEventKind.SpinnerProgress => ScoreObjectKind.Spinner, _ => ScoreObjectKind.Slider };
        return new(new(0,source,1000,5000),0,kind,raw,enabled,source==ScoreObjectKind.Slider,ScorePolicy.PlainLegacy,role);
    }
    private static void Whole(ScoreLedger ledger, int raw = 1024) => ledger.Apply(Event(ScoreEventKind.CircleWhole,raw));
    private static void Credit(ScoreLedger ledger, int count)
    { for(int i=0;i<count;i++) ledger.Apply(Event(ScoreEventKind.SliderInterior,8,SliderCheckpointKind.Interior)); }
    private static CircleSession Auto(Beatmap map) => new(map,ScoreConfiguration.FromBeatmap(map));
    private static Beatmap Fixture(string name) => Beatmap.Load(Path.Combine(root,"reconstructed/CircleGame/Fixtures",name+".osu"));

    public static int Main(string[] args)
    {
        if(args.Length!=1) throw new ArgumentException("Pass repository root.");
        root=Path.GetFullPath(args[0]); scratch=Path.Combine(Path.GetTempPath(),"ohno-qa06-"+Guid.NewGuid().ToString("N")); Directory.CreateDirectory(scratch);
        try
        {
            RegisterMetadata(); RegisterLedger(); RegisterIntegration(); RegisterPresentation();
            int failed=0;
            foreach(var c in Cases)
            {
                lastInput="";
                try { c.Body(); Console.WriteLine("PASS "+c.Name); }
                catch(Exception e) { failed++; Console.WriteLine($"FAIL {c.Name}: {e}"); if(lastInput.Length>0) Console.WriteLine("FAILED INPUT "+JsonSerializer.Serialize(lastInput)); }
            }
            Console.WriteLine($"QA06 ACCOUNTING TESTS: {Cases.Count-failed} passed, {failed} failed, {Cases.Count} executed");
            return failed==0?0:1;
        }
        finally { Directory.Delete(scratch,true); }
    }

    private static void RegisterMetadata()
    {
        foreach(var v in OracleVectors.Metadata)
            Add(v.Name,()=>{var m=Meta(v.Rows,v.Difficulty,v.Events);Eq(m.RowCount,v.N);Eq(m.FirstStart,v.First);Eq(m.LastStart,v.Last);Eq(m.BreakMilliseconds,v.Breaks);Eq(m.WholeSeconds,v.Seconds);Eq(m.Prepare().K,v.K);});
        Add("A20 spinner end independent from start density",()=>{
            var a=Meta("0,0,2000,8,0,5000");var b=Meta("0,0,2000,8,0,50000");Eq(a.WholeSeconds,0);Eq(b.WholeSeconds,0);Eq(a.Prepare().K,3);Eq(a.Prepare().K,b.Prepare().K);Eq(a.LastEnd,5000);Eq(b.LastEnd,50000);
        });
        Add("A21 masked new combo bits and independent recognized counts",()=>{
            var m=Meta("0,0,1000,5,0\n0,0,2000,6,0,L|100:0,1,100\n0,0,3000,12,0,6000\n0,0,4000,16,0");
            Eq(m.RowCount,4);Eq(m.Circles,(ushort)1);Eq(m.Sliders,(ushort)1);Eq(m.Spinners,(ushort)1);Eq(m.LastStart,3000);Eq(m.LastEnd,6000);
        });
        Add("A22 hold metadata separate from ordinary gameplay admission",()=>{
            var m=Meta("0,0,1000,1,0\n0,0,2000,128,0,6000:0:0:0:0:");Eq(m.FirstStart,1000);Eq(m.LastStart,6000);Eq(m.LastEnd,6000);Eq(m.WholeSeconds,5);Eq(m.Sliders,(ushort)1);
            Reject<InvalidDataException>(()=>Map("0,0,2000,128,0,6000:0:0:0:0:"));
        });
        Add("A23 modern fractional explicit difficulty",()=>{var m=Meta("0,0,1000,1,0\n0,0,5000,1,0","HPDrainRate:5.5\nOverallDifficulty:4.25\nCircleSize:3.25");Eq(m.Hp,5.5f);Eq(m.Od,4.25f);Eq(m.Cs,3.25f);Eq(m.Prepare().K,2);});
        Add("A24 fresh defaults reset independently of previous metadata",()=>{
            Meta("0,0,1000,1,0","HPDrainRate:0\nOverallDifficulty:0\nCircleSize:0");var m=Meta("0,0,1000,1,0\n0,0,5000,1,0","");Eq(m.Hp,5f);Eq(m.Od,5f);Eq(m.Cs,5f);Eq(m.Prepare().BeforeRounding,2.5f);Eq(m.Prepare().K,2);
        });
        Add("A25 modern clamp vs engineering gameplay rejection",()=>{var m=Meta("0,0,1000,1,0","HPDrainRate:-2\nOverallDifficulty:12\nCircleSize:5");Eq(m.Hp,0f);Eq(m.Od,10f);Reject<InvalidDataException>(()=>Map("0,0,1000,1,0","HPDrainRate:0\nOverallDifficulty:12\nCircleSize:5\nApproachRate:5"));});
        Add("A26 older format Byte accepted but fractional syntax rejected",()=>{
            var m=Meta("0,0,1000,1,0","HPDrainRate:3\nOverallDifficulty:4\nCircleSize:5",version:12);Eq(m.Hp,3f);Eq(m.Od,4f);Eq(m.Cs,5f);
            Reject<InvalidDataException>(()=>Meta("0,0,1000,1,0","HPDrainRate:3.5",version:12));
        });
        foreach(var pair in new[]{("4.99999",2),("5",2),("5.00001",3)})
            Add("A27 midpoint neighborhood HP"+pair.Item1,()=>Eq(Meta("0,0,1000,1,0\n0,0,5000,1,0",$"HPDrainRate:{pair.Item1}\nOverallDifficulty:5\nCircleSize:5").Prepare().K,pair.Item2));
        Add("A28 corrected binary32 vector retains every Single operation",()=>{
            var p=Meta("0,0,3000,1,0\n0,0,1000,1,0","HPDrainRate:5\nOverallDifficulty:6.4\nCircleSize:0").Prepare();
            Eq(p.DensityTerm,0f);Eq(BitConverter.SingleToInt32Bits(p.BeforeRounding),0x3fbfffff);Eq(p.K,1);
            // Independent binary32 proof in README: sum41366666 /38 ->3e999999, *5 ->3fbfffff.
            // Late Single conversion after Double division/multiplication instead gives3fc00000.
            Eq(BitConverter.SingleToInt32Bits((float)((double)(5f+6.4f)/38.0*5.0)),0x3fc00000);
        });
        Add("A29 empty zero-over-zero explicitly unsupported",()=>{var m=Meta("");Eq(m.RowCount,0);Eq(m.WholeSeconds,0);Reject<InvalidDataException>(()=>m.Prepare());});
        Add("A30 malformed nonfinite metadata explicitly rejects",()=>{
            foreach(var rows in new[]{"0,0,NaN,1,0","0,0,Infinity,1,0","0,0,2147483648,1,0","0,0,1000,notint,0"}) Reject<InvalidDataException>(()=>Meta(rows));
            Reject<InvalidDataException>(()=>Meta("0,0,1000,1,0","HPDrainRate:NaN"));
        });
        Add("A31 raw metadata order distinct from sorted admitted objects",()=>{var m=Map("0,0,5000,1,0\n0,0,1000,1,0\n0,0,3000,1,0");Eq(m.Circles[0].StartTime,1000);Eq(m.ScoreMetadata!.FirstStart,5000);Eq(m.ScoreMetadata.LastStart,3000);Eq(m.ScoreMetadata.WholeSeconds,-2);Eq(ScoreConfiguration.FromBeatmap(m).K,1.0);});
        Add("A32 equal and unknown types metadata accepted gameplay rejected",()=>{
            Eq(Meta("0,0,1000,1,0\n0,0,1000,1,0").RowCount,2);Reject<InvalidDataException>(()=>Map("0,0,1000,1,0\n0,0,1000,1,0"));
            Eq(Meta("0,0,1000,16,0").RowCount,1);Reject<InvalidDataException>(()=>Map("0,0,1000,16,0"));
        });
        Add("A33 timing metadata and spinner duration do not set seconds",()=>{
            string rows="0,0,1000,1,0\n0,0,4000,8,0,7000";var a=Map(rows);var b=Meta(rows+"\n[TimingPoints]\n0,60,4,1,0,100,1,0");Eq(a.ScoreMetadata!.WholeSeconds,3);Eq(b.WholeSeconds,3);Eq(a.ScoreMetadata.Prepare().K,b.Prepare().K);
        });
        Add("A35 binary32 4.5 tie differs from Double continuation",()=>{
            string rows=string.Join('\n',Enumerable.Range(0,21).Select(i=>$"0,0,{1000+i*2000},1,0"));
            var p=Meta(rows,"HPDrainRate:10\nOverallDifficulty:10\nCircleSize:10").Prepare();
            Eq(p.Metadata.WholeSeconds,40);Eq(BitConverter.SingleToInt32Bits(p.DensityTerm),0x40866666);
            Eq(BitConverter.SingleToInt32Bits(p.BeforeRounding),0x40900000);Eq(p.K,4);
            // 34.20000076293945 is the independently derived Single sum (bits4208cccd).
            Eq(Math.Round((double)BitConverter.Int32BitsToSingle(0x4208cccd)/38.0*5.0),5.0);
        });
        Add("A34 missing source fails automatic configuration without silent K1",()=>{
            var synthetic=new Beatmap("synthetic",5,5,5,[new(new(0,0),1000)]);Reject<InvalidDataException>(()=>ScoreConfiguration.FromBeatmap(synthetic));Eq(new CircleSession(synthetic,ScoreConfiguration.EngineeringPractice).Scoring.K,1.0);
        });
    }

    private static void RegisterLedger()
    {
        // Literal original E33 M0ad2 branch table; independent from production switch.
        var rows=new (string Name, ScoreEventKind Kind,int Raw,SliderCheckpointKind? Role,int Amount,int Combo,int Whole)[]
        {
            ("circle300",ScoreEventKind.CircleWhole,1024,null,300,1,1),("circle100",ScoreEventKind.CircleWhole,512,null,100,1,1),
            ("circle50",ScoreEventKind.CircleWhole,256,null,50,1,1),("wholeMISS",ScoreEventKind.CircleWhole,-131072,null,0,0,1),
            ("sliderHead",ScoreEventKind.SliderHead,64,null,30,1,0),("headFail",ScoreEventKind.SliderHead,-262144,null,0,0,0),
            ("interior",ScoreEventKind.SliderInterior,8,SliderCheckpointKind.Interior,10,1,0),
            ("repeat",ScoreEventKind.SliderRepeatOrTerminal,64,SliderCheckpointKind.Repeat,30,1,0),
            ("terminal",ScoreEventKind.SliderRepeatOrTerminal,128,SliderCheckpointKind.Final,30,1,0),
            ("terminalFail",ScoreEventKind.SliderRepeatOrTerminal,-524288,SliderCheckpointKind.Final,0,0,0),
            ("spinner4096",ScoreEventKind.SpinnerProgress,4096,null,0,0,0),("spinner8192",ScoreEventKind.SpinnerProgress,8192,null,100,0,0),
            ("spinner16384",ScoreEventKind.SpinnerProgress,16384,null,1100,0,0)
        };
        foreach(var r in rows) Add("B01 base table "+r.Name,()=>{var l=Ledger(7);var o=l.Apply(Event(r.Kind,r.Raw,r.Role));Eq(o.BaseAmount,r.Amount);Eq(o.CalculatedDelta,r.Amount);Eq(l.Score,r.Amount);Eq(l.LastBaseAmount,r.Amount);Eq(l.CurrentCombo,r.Combo);Eq(l.TotalWholeResults,r.Whole);});
        Add("B02 four quality counters exclusively determine Single accuracy",()=>{
            var l=Ledger();foreach(int r in new[]{1024,512,256,-131072})Whole(l,r);
            Eq(l.Score,452);Eq(l.N300,(ushort)1);Eq(l.N100,(ushort)1);Eq(l.N50,(ushort)1);Eq(l.NMiss,(ushort)1);Eq(l.AccuracyFraction,450f/1200f);Eq(l.AccuracyPercent,37.5f);Eq(l.MaximumCombo,3);Eq(l.CurrentCombo,0);
        });
        Add("B03 previous combo not incremented combo supplies bonus",()=>{var l=Ledger(3);Whole(l);Whole(l);Whole(l);Eq(l.Score,936);Eq(l.LastOutcome.ComboBefore,2);Eq(l.CurrentCombo,3);});
        Add("B04 maximum combo is not bonus operand after MISS",()=>{var l=Ledger(3);for(int i=0;i<5;i++)Whole(l);Whole(l,-131072);int old=l.Score;Whole(l,512);Eq(l.Score-old,100);Eq(l.MaximumCombo,5);Eq(l.LastOutcome.ComboBefore,0);});
        foreach(var r in new[]{(1024,300,12),(512,100,4),(256,50,2)})
            Add("B05 integer b25 quotient raw"+r.Item1,()=>{var l=Ledger(2.25);Credit(l,3);int old=l.Score;Whole(l,r.Item1);Eq(l.Score-old,r.Item2+(int)(2*r.Item3*2.25));Eq(l.LastOutcome.ComboBefore,3);});
        Add("B06 fractional bonus truncates rather than rounds",()=>{var l=Ledger(.49);Credit(l,2);int old=l.Score;Whole(l,256);Eq(l.Score-old,50);});
        Add("B07 zero K retains base scoring and combo",()=>{var l=Ledger(0);Credit(l,5);Whole(l);Eq(l.Score,350);Eq(l.CurrentCombo,6);});
        Add("B08 positive slider parent scales score without duplicate combo",()=>{var l=Ledger(3);l.Apply(Event(ScoreEventKind.SliderHead,64));l.Apply(Event(ScoreEventKind.SliderRepeatOrTerminal,128,SliderCheckpointKind.Final));l.Apply(Event(ScoreEventKind.SliderAggregate,1024));Eq(l.Score,396);Eq(l.CurrentCombo,2);Eq(l.MaximumCombo,2);Eq(l.ComboSnapshot,(ushort)2);Eq(l.N300,(ushort)1);});
        Add("B09 positive combo resets on parent whole MISS",()=>{var l=Ledger();Credit(l,4);l.Apply(Event(ScoreEventKind.SliderAggregate,-131072));Eq(l.CurrentCombo,0);Eq(l.ComboSnapshot,(ushort)0);Eq(l.MaximumCombo,4);Eq(l.NMiss,(ushort)1);Eq(l.Score,40);});
        Add("B10 terminal failure preserves combo interior failure resets",()=>{var a=Ledger();var b=Ledger();Credit(a,3);Credit(b,3);a.Apply(Event(ScoreEventKind.SliderRepeatOrTerminal,-524288,SliderCheckpointKind.Final));b.Apply(Event(ScoreEventKind.SliderInterior,-262144,SliderCheckpointKind.Interior));Eq(a.CurrentCombo,3);Eq(a.ComboSnapshot,(ushort)3);Eq(b.CurrentCombo,0);Eq(a.NMiss,(ushort)0);Eq(b.NMiss,(ushort)0);});
        Add("B11 positive parent after failed tick counts once",()=>{var l=Ledger(3);l.Apply(Event(ScoreEventKind.SliderHead,64));l.Apply(Event(ScoreEventKind.SliderInterior,-262144,SliderCheckpointKind.Interior));l.Apply(Event(ScoreEventKind.SliderRepeatOrTerminal,128,SliderCheckpointKind.Final));l.Apply(Event(ScoreEventKind.SliderAggregate,512));Eq(l.Score,160);Eq(l.CurrentCombo,1);Eq(l.TotalWholeResults,1);Eq(l.AccuracyFraction,100f/300f);});
        Add("B12 spinner amounts never scale or change existing combo",()=>{var l=Ledger(10);Credit(l,4);foreach(int raw in new[]{4096,8192,16384})l.Apply(Event(ScoreEventKind.SpinnerProgress,raw));Eq(l.Score,1240);Eq(l.CurrentCombo,4);Eq(l.TotalWholeResults,0);Eq(l.AccuracyFraction,1f);});
        Add("B13 spinner final separately scales and increments combo",()=>{var l=Ledger(2);Credit(l,3);l.Apply(Event(ScoreEventKind.SpinnerProgress,8192));l.Apply(Event(ScoreEventKind.SpinnerFinal,1024));Eq(l.Score,478);Eq(l.CurrentCombo,4);Eq(l.N300,(ushort)1);});
        Add("B14 disabled positive accounting still mutates combo",()=>{var l=Ledger(2);var o=l.Apply(Event(ScoreEventKind.CircleWhole,1024,enabled:false));Eq(o.Status,ScoreAccountingStatus.RecordSuppressed);Eq(o.CalculatedDelta,300);Eq(o.AppliedDelta,0);Eq(l.Score,0);Eq(l.N300,(ushort)0);Eq(l.CurrentCombo,1);});
        Add("B15 disabled negative admits zero write not hit counter",()=>{var l=Ledger();Credit(l,2);var o=l.Apply(Event(ScoreEventKind.CircleWhole,-131072,enabled:false));Eq(o.Status,ScoreAccountingStatus.Applied);Eq(l.Score,20);Eq(l.CurrentCombo,0);Eq(l.NMiss,(ushort)0);});
        Add("B16 raw zero is no event no mutation",()=>{var l=Ledger();Whole(l);int n=l.EventCount;var e=l.LastEvent;Eq(l.Apply(Event(ScoreEventKind.CircleWhole,0)).Status,ScoreAccountingStatus.NoEvent);Eq(l.Score,300);Eq(l.EventCount,n);Eq(l.LastEvent,e);});
        Add("B17 UInt16 hit counts wrap and empty accuracy getter returns one",()=>{var l=Ledger(0);for(int i=0;i<65536;i++)Whole(l);Eq(l.N300,(ushort)0);Eq(l.TotalWholeResults,0);Eq(l.AccuracyFraction,1f);Eq(l.Score,19660800);Eq(l.CurrentCombo,65536);Eq(l.ComboSnapshot,(ushort)0);});
        Add("B18 unchecked Int32 score addition wraps finite bonuses",()=>{var l=Ledger(10);for(int i=0;i<8000;i++)Whole(l);Eq(l.Score,unchecked((int)(300L*8000+60L*7998*7999)));Eq(l.Score,-454007176);Eq(l.IsSupported,true);});
        Add("B19 ScoreV2 unavailable preserves explicit unsupported status",()=>{var l=new ScoreLedger(new(2,ScorePolicy.ScoreV2,"QA06 unsupported"));Eq(l.Apply(Event(ScoreEventKind.CircleWhole,1024)).Status,ScoreAccountingStatus.Unsupported);Eq(l.Score,0);Eq(l.TotalWholeResults,0);});
        foreach(var e in new[]{Event(ScoreEventKind.CircleWhole,1028),Event(ScoreEventKind.CircleWhole,1024) with {ObjectHasSliderType=true},Event(ScoreEventKind.SliderInterior,8),Event(ScoreEventKind.CircleWhole,1024) with {Policy=ScorePolicy.ScoreV2}})
            Add("B20 reject unsupported event "+e.Raw+"/"+e.Kind+"/"+e.ObjectHasSliderType+"/"+e.Policy,()=>{var l=Ledger();Whole(l);Eq(l.Apply(e).Status,ScoreAccountingStatus.Unsupported);Whole(l);Eq(l.Score,300);Eq(l.N300,(ushort)1);Eq(l.IsSupported,false);});
        Add("B21 engineering K and provenance bounds explicit",()=>{foreach(double k in new[]{double.NaN,double.PositiveInfinity,-.1,10.01})Reject<ArgumentOutOfRangeException>(()=>Ledger(k));Reject<ArgumentOutOfRangeException>(()=>new ScoreLedger(new(1,ScorePolicy.PlainLegacy,"")));});
        Add("B22 overdue interior raw128 scores raw amount with producer preserved",()=>{var l=Ledger();var e=Event(ScoreEventKind.SliderInterior,128,SliderCheckpointKind.Interior);l.Apply(e);Eq(l.Score,30);Eq(l.CurrentCombo,1);Eq(l.LastEvent!.Value.ProducerRole,(SliderCheckpointKind?)SliderCheckpointKind.Interior);});
    }

    private static void RegisterIntegration()
    {
        foreach(var v in new[]{("mixed-r07",2.0),("bezier-joined",3.0)})
            Add("C01 fresh production factory "+v.Item1,()=>{var m=Fixture(v.Item1);var s=Auto(m);Eq(s.Scoring.K,v.Item2);Eq(s.Scoring.Policy,ScorePolicy.PlainLegacy);Yes(s.Scoring.MultiplierProvenance.StartsWith("AUTO E034"),"automatic config missing provenance");Eq(s.Ledger.Score,0);});
        foreach(double k in new[]{1.0,2.0}) Add("C02 explicit Mixed event contract K"+k,()=>{
            var m=Fixture("mixed-r07");var s=k==2?Auto(m):new CircleSession(m,ScoreConfiguration.EngineeringPractice);var l=s.Ledger;
            Whole(l);l.Apply(Event(ScoreEventKind.SliderHead,64));Credit(l,2);l.Apply(Event(ScoreEventKind.SliderRepeatOrTerminal,128,SliderCheckpointKind.Final));l.Apply(Event(ScoreEventKind.SliderAggregate,1024));
            // Literal E034 C1..16 event sequence: eight8192 and seven4096; C1 raw0.
            for(int i=2;i<=16;i++)l.Apply(Event(ScoreEventKind.SpinnerProgress,i%2==0?8192:4096));
            l.Apply(Event(ScoreEventKind.SpinnerFinal,1024));Whole(l);Eq(l.Score,k==2?2392:2236);Eq(l.N300,(ushort)4);Eq(l.AccuracyFraction,1f);Eq(l.MaximumCombo,7);Eq(s.JudgedCount,0); // supplied events, not gameplay lifecycle.
        });
        foreach(double k in new[]{1.0,3.0}) Add("C03 explicit Joined event contract K"+k,()=>{
            var m=Fixture("bezier-joined");var s=k==3?Auto(m):new CircleSession(m,ScoreConfiguration.EngineeringPractice);var l=s.Ledger;
            l.Apply(Event(ScoreEventKind.SliderHead,64));Credit(l,3);l.Apply(Event(ScoreEventKind.SliderRepeatOrTerminal,128,SliderCheckpointKind.Final));l.Apply(Event(ScoreEventKind.SliderAggregate,1024));
            Eq(l.Score,k==3?534:438);Eq(l.N300,(ushort)1);Eq(l.MaximumCombo,5);Eq(l.AccuracyFraction,1f);Yes(l.Score!=582,"must not fit unknown original test3");
        });
        Add("C04 Joined actual session events reach automatic ledger once",()=>{
            var s=Auto(Fixture("bezier-joined"));var d=s.Sliders[0].Definition;s.Press(d.Head.Position,d.Head.StartTime);s.Advance(d.Head.StartTime,d.Head.Position,1);
            foreach(var c in d.Checkpoints)s.Advance(c.Time,d.RequiredPosition(c.Time),1);
            Eq(s.Sliders[0].SuccessfulCheckpoints,4);Eq(s.Ledger.Score,90);Eq(s.Combo,5);Eq(s.JudgedCount,0);
            s.Advance(d.EndTime,d.RequiredPosition(d.EndTime),1);Eq(s.Ledger.Score,534);Eq(s.JudgedCount,1);Eq(s.Combo,5);Eq(s.Ledger.N300,(ushort)1);
            s.Advance(d.EndTime+1,d.EndPosition,1);Eq(s.Ledger.Score,534);Eq(s.Ledger.EventCount,6);
        });
        Add("C05 Mixed actual lifecycle with explicitly seeded spinner observations",()=>{
            var s=Auto(Fixture("mixed-r07"));s.Press(s.Circles[0].Definition.Position,3000);var d=s.Sliders[0].Definition;s.Press(d.Head.Position,5000);s.Advance(5000,d.Head.Position,1);
            foreach(var c in d.Checkpoints)s.Advance(c.Time,d.RequiredPosition(c.Time),1);s.Advance(d.EndTime,d.EndPosition,1);Eq(s.Ledger.Score,776);Eq(s.Combo,5);
            // Artificial explicit observation input, not OS gameplay, forced fields or a substitute spinner.
            for(int i=1;i<=16;i++) {int raw=s.Spinners[0].Observation.Observe(i,15);if(raw!=0)s.Ledger.Apply(Event(ScoreEventKind.SpinnerProgress,raw));}
            s.Advance(13001);s.Press(s.Circles[1].Definition.Position,15000);Eq(s.Ledger.Score,2392);Eq(s.JudgedCount,4);Eq(s.Ledger.N300,(ushort)4);Eq(s.MaxCombo,7);Eq(s.Accuracy,100.0);
        });
        Add("C06 restart and map replacement own independent K and ledgers",()=>{
            var a=Auto(Fixture("mixed-r07"));a.Press(a.Circles[0].Definition.Position,3000);var restart=new CircleSession(a.Map,a.Scoring);var b=Auto(Fixture("bezier-joined"));Eq(restart.Scoring.K,2.0);Eq(b.Scoring.K,3.0);
            foreach(var s in new[]{restart,b}){Eq(s.Ledger.Score,0);Eq(s.Combo,0);Eq(s.MaxCombo,0);Eq(s.Ledger.TotalWholeResults,0);Eq(s.JudgedCount,0);}Yes(!ReferenceEquals(a.Ledger,restart.Ledger)&&!ReferenceEquals(a.Ledger,b.Ledger),"shared ledger");Eq(a.Ledger.Score,300);
        });
        Add("C07 failed new metadata cannot mutate prior configured session",()=>{var s=Auto(Fixture("mixed-r07"));s.Press(s.Circles[0].Definition.Position,3000);Reject<InvalidDataException>(()=>ScoreConfiguration.FromBeatmap(s.Map with {ScoreMetadata=Meta("")}));Eq(s.Ledger.Score,300);Eq(s.Scoring.K,2.0);});
        Add("C08 delayed slider completion does not fabricate backlog score",()=>{
            var s=Auto(Map("0,0,2000,2,0,L|300:0,2,300"));var d=s.Sliders[0].Definition;s.Press(Vector2.Zero,2000);s.Advance(2000,Vector2.Zero,1);s.Advance(8000,d.EndPosition,1);
            Eq(s.Sliders[0].ProcessedCheckpoints,1);Eq(s.Sliders[0].Checkpoints.Count(c=>c.Succeeded is null),5);Eq(s.Ledger.EventCount,3);Eq(s.Ledger.LastEvent!.Value.Kind,ScoreEventKind.SliderAggregate);
            int score=s.Ledger.Score;s.Advance(8001,d.EndPosition,1);Eq(s.Ledger.Score,score);
        });
    }

    private static void RegisterPresentation()
    {
        foreach(var v in new[]{(0,false),(1999,false),(2000,true),(3500,true),(5000,true),(5001,true)})
            Add("D01 pending presentation at "+v.Item1,()=>Eq(SpinnerPresentation.IsVisible(new(new(2000,5000,0,null),5),v.Item1),v.Item2));
        Add("D02 completed spinner no longer presented",()=>{var s=new SpinnerState(new(2000,5000,0,null),5);Eq(s.Complete(5000),false);Eq(SpinnerPresentation.IsVisible(s,5000),true);s.Complete(5001);Eq(SpinnerPresentation.IsVisible(s,5001),false);});
        Add("D03 queries preserve motion progress input observation and scoring",()=>{
            var game=Auto(Map("0,0,2000,8,0,5000"));var s=game.Spinners[0];s.SampleAngle(0,2000,16,true);s.SampleAngle(1,2100,16,true);s.Integrate(2100,100);s.Observation.Observe(7,15);
            string before=JsonSerializer.Serialize(new {Spinner=s,Ledger=game.Ledger,game.JudgedCount,game.Feedback});
            for(int t=0;t<6000;t+=137)SpinnerPresentation.IsVisible(s,t);
            Eq(JsonSerializer.Serialize(new {Spinner=s,Ledger=game.Ledger,game.JudgedCount,game.Feedback}),before);
        });
        Add("D04 prestart spinner never blocks preceding head during old preview",()=>{
            var s=Auto(Map("100,192,9000,2,0,L|180:192,1,80\n256,192,10000,8,0,13000"));Eq(SpinnerPresentation.IsVisible(s.Spinners[0],9000),false);s.SetHeldButtons(1);s.Press(new(100,192),9000);s.Advance(9000,new(100,192),1);Eq(s.Sliders[0].Head.Result,Judgement.Hit300);Eq(s.Sliders[0].Tracking,true);Eq(s.Ledger.Score,30);
            s.Advance(9764,new(176.4f,192),1);s.Advance(9800,new(180,192),1);Eq(s.Sliders[0].Result,Judgement.Hit300);Eq(s.Ledger.Score,396);Eq(s.Combo,2);Eq(s.Spinners[0].Progress,0f);
        });
        Add("D05 held slider checkpoints survive old preview with read-only queries",()=>{
            // Slider7000..9900, Q100: ticks8000/9000, terminal9864. Old preview begins8800.
            var s=Auto(Map("100,192,7000,2,0,L|390:192,1,290\n256,192,10000,8,0,13000"));var d=s.Sliders[0].Definition;
            Eq(string.Join(',',d.Checkpoints.Select(c=>c.Time)),"8000,9000,9864");s.Press(new(100,192),7000);s.Advance(7000,new(100,192),1);
            foreach(int t in new[]{8000,8799,8800,9000,9864,9900})
            {
                var a=s.Sliders[0];var held=(a.Tracking,a.EntryTime,a.FollowEligible,a.RememberedMask,a.ProcessedCheckpoints,s.Ledger.Score,s.Ledger.EventCount);
                for(int i=0;i<20;i++)Eq(SpinnerPresentation.IsVisible(s.Spinners[0],t),false);
                Eq((a.Tracking,a.EntryTime,a.FollowEligible,a.RememberedMask,a.ProcessedCheckpoints,s.Ledger.Score,s.Ledger.EventCount),held);
                s.Advance(t,d.RequiredPosition(t),1);Eq(a.Tracking,true);Eq(a.EntryTime,7000);
            }
            Eq(s.Sliders[0].SuccessfulCheckpoints,3);Eq(s.Sliders[0].Result,Judgement.Hit300);Eq(s.Ledger.Score,452);Eq(s.Combo,4);Eq(s.Ledger.EventCount,5);Eq(s.Spinners[0].PreviousAngle,(double?)null);
        });
        Add("D06 queries cannot consume pending held-button admission",()=>{
            var map=Map("100,192,9000,2,0,L|180:192,1,80\n256,192,10000,8,0,13000");var a=Auto(map);var b=Auto(map);a.SetHeldButtons(2);b.SetHeldButtons(2);
            for(int i=0;i<50;i++)SpinnerPresentation.IsVisible(a.Spinners[0],10000);
            foreach(var s in new[]{a,b}){s.Press(new(100,192),9000,2);s.Advance(9000,new(100,192),2);}
            Eq(a.Sliders[0].RememberedMask,b.Sliders[0].RememberedMask);Eq(a.Sliders[0].Tracking,b.Sliders[0].Tracking);Eq(a.Ledger.Score,b.Ledger.Score);
        });
        Add("D07 presentation does not change strict spinner gameplay boundaries",()=>{
            var s=Auto(Map("0,0,2000,8,0,5000"));var p=s.Spinners[0];p.SampleAngle(0,2000,16,true);p.SampleAngle(1,2000,16,true);SpinnerPresentation.IsVisible(p,2000);p.Integrate(2000,100);Eq(p.Progress,0f);
            p.Integrate(5000,100);Eq(p.Progress,0f);s.Advance(5000);Eq(s.JudgedCount,0);s.Advance(5001);Eq(s.JudgedCount,1);Eq(s.Ledger.NMiss,(ushort)1);
        });
    }
}
