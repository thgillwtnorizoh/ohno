// QA01 direct component tests. No Window, input provider, GUI entry point,
// stopwatch, recorded input, OS events or application control is linked/called.
using System.Numerics;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Rendering;

namespace Ohno.QA01;

public static class CoreTests
{
    private static readonly List<(string Name, Action Body)> Cases = [];
    private static string root = "";
    private static string scratch = "";
    private static int fileId;

    private static CircleSession One(float od = 5, float cs = 5, float ar = 5) =>
        new(new Beatmap("QA01", cs, od, ar, new[] { new CircleDefinition(Vector2.Zero, 2000) }));

    private static void Equal<T>(T actual, T expected)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            throw new InvalidOperationException($"expected={expected}; actual={actual}");
    }

    private static void Check(bool value, string label)
    {
        if (!value) throw new InvalidOperationException(label);
    }

    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidDataException) { return; }
        throw new InvalidOperationException("expected InvalidDataException; actual accepted input");
    }

    private static string MapText(string rows = "256,192,2000,1,0", string difficulty =
        "CircleSize:5\nOverallDifficulty:5\nApproachRate:5") =>
        "osu file format v14\n[General]\nMode:0\n[Metadata]\nTitle:QA01\n[Difficulty]\n" +
        difficulty + "\n[HitObjects]\n" + rows + "\n";

    private static Beatmap Parse(string text)
    {
        string path = Path.Combine(scratch, $"map-{fileId++}.osu");
        File.WriteAllText(path, text);
        return Beatmap.Load(path);
    }

    private static void Add(string name, Action body) => Cases.Add((name, body));

    public static int Main(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException("Pass the repository root path, not an application/map command.");
        root = Path.GetFullPath(args[0]);
        scratch = Path.Combine(Path.GetTempPath(), "ohno-qa01-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(scratch);
        try
        {
            Register();
            int failed = 0;
            foreach (var c in Cases)
            {
                try { c.Body(); Console.WriteLine("PASS " + c.Name); }
                catch (Exception e) { failed++; Console.WriteLine($"FAIL {c.Name}: {e.GetType().Name}: {e.Message}"); }
            }
            Console.WriteLine($"HEADLESS COMPONENT TESTS: {Cases.Count - failed} passed, {failed} failed, {Cases.Count} executed");
            return failed == 0 ? 0 : 1;
        }
        finally { Directory.Delete(scratch, true); }
    }

    private static void Register()
    {
        // Independent E12 J01–J16 expectations, not another implementation of Judge.
        foreach (var (delta, expected) in new (int, Judgement)[] {
            (0, Judgement.Hit300), (-49, Judgement.Hit300), (49, Judgement.Hit300),
            (-50, Judgement.Hit100), (50, Judgement.Hit100), (-51, Judgement.Hit100), (51, Judgement.Hit100),
            (-99, Judgement.Hit100), (99, Judgement.Hit100), (-100, Judgement.Hit50), (100, Judgement.Hit50),
            (-101, Judgement.Hit50), (101, Judgement.Hit50), (-149, Judgement.Hit50), (149, Judgement.Hit50),
            (-150, Judgement.Miss), (150, Judgement.Miss), (-151, Judgement.Miss), (151, Judgement.Pending) })
            Add($"timing OD5 delta={delta}", () => {
                var s = One(); s.Press(Vector2.Zero, 2000 + delta); Equal(s.Circles[0].Result, expected);
            });
        foreach (var (od, w300, w100, w50, approach) in new (float, int, int, int, int)[] {
            (0,80,140,200,1800), (4,56,108,160,1320), (5,50,100,150,1200),
            (5.25f,48,98,147,1162), (10,20,60,100,450) })
            Add($"prepared operands OD/AR={od}", () => {
                var s = One(od, ar: od);
                Equal(s.Window300,w300); Equal(s.Window100,w100); Equal(s.Window50,w50); Equal(s.Approach,approach);
            });
        foreach (var (error, expected) in new (int, Judgement)[] {
            (55,Judgement.Hit300),(56,Judgement.Hit100),(57,Judgement.Hit100),
            (107,Judgement.Hit100),(108,Judgement.Hit50),(109,Judgement.Hit50),
            (159,Judgement.Hit50),(160,Judgement.Miss) })
            foreach (int sign in new[] {-1,1})
                Add($"R02 fixture OD4 error={error} sign={sign}",()=> {
                    var s=One(od:4);s.Press(Vector2.Zero,2000+sign*error);Equal(s.Circles[0].Result,expected);
                });
        Add("expiration equality and next millisecond", () => {
            var s = One(); s.Advance(2150); Equal(s.Circles[0].Result,Judgement.Pending);
            s.Advance(2151); Equal(s.Circles[0].Result,Judgement.Miss); Equal(s.JudgedCount,1);
            s.Advance(2500); s.Press(Vector2.Zero,2000); Equal(s.JudgedCount,1);
        });
        Add("already evaluated exclusion", () => {
            var s = One(); s.Press(Vector2.Zero,2000); s.Press(Vector2.Zero,2000); s.Advance(3000);
            Equal(s.JudgedCount,1); Equal(s.Circles[0].Result,Judgement.Hit300);
        });
        foreach (int time in new[] { 799, 800, 1600, 1601 })
            Add($"selection/admission early time={time}", () => {
                var s=One(); s.Press(Vector2.Zero,time);
                Equal(s.Circles[0].Result,time==1601?Judgement.Miss:Judgement.Pending);
                Equal(s.Feedback is not null,time>=800);
            });
        Add("ordered earlier object blocking", () => {
            var s = new CircleSession(new Beatmap("two",5,5,5,new[] {
                new CircleDefinition(Vector2.Zero,2000),new CircleDefinition(new(256,192),2100) }));
            s.Press(new(256,192),2100); Equal(s.JudgedCount,0);
            s.Advance(2151); s.Press(new(256,192),2151); Equal(s.Circles[1].Result,Judgement.Hit100);
        });
        Add("first passing object is not nearest", () => {
            var s = new CircleSession(new Beatmap("overlap",5,5,5,new[] {
                new CircleDefinition(new(5,0),2000),new CircleDefinition(Vector2.Zero,2001) }));
            s.Press(Vector2.Zero,2000); Equal(s.Circles[0].Result,Judgement.Hit300); Equal(s.Circles[1].Result,Judgement.Pending);
        });
        Add("scale-one radius scalar", () => {
            var s=One(); Equal(s.DiameterAtScaleOne,64f); Equal(s.HitRadius,32.013118743896484f);
            Check(One(cs:4).HitRadius>s.HitRadius,"CS4 radius should exceed CS5");
        });
        foreach (string location in new[] { "center", "inside", "boundary", "outside", "diagonal-inside", "diagonal-outside" })
            Add("geometry "+location, () => {
                var s=One();
                Vector2 p=location switch {
                    "center"=>Vector2.Zero, "inside"=>new(MathF.BitDecrement(s.HitRadius),0),
                    "boundary"=>new(s.HitRadius,0), "outside"=>new(MathF.BitIncrement(s.HitRadius),0),
                    "diagonal-inside"=>new(22,22), _=>new(24,24) };
                s.Press(p,2000);
                Equal(s.Circles[0].Result,location is "outside" or "diagonal-outside"?Judgement.Pending:Judgement.Hit300);
            });
        foreach (Vector2 point in new[] { new Vector2(16,0),new(-16,0),new(0,16),new(0,-16),new(-22,22),new(22,-22) })
            Add("geometry sign symmetry "+point, () => {var s=One();s.Press(point,2000);Equal(s.Circles[0].Result,Judgement.Hit300);});
        Add("QA01-D01 reject NaN X",()=> {var s=One();s.Press(new(float.NaN,0),2000);Equal(s.Circles[0].Result,Judgement.Pending);});
        Add("QA01-D01 reject NaN Y",()=> {var s=One();s.Press(new(0,float.NaN),2000);Equal(s.Circles[0].Result,Judgement.Pending);});
        Add("reject infinite pointer",()=> {var s=One();s.Press(new(float.PositiveInfinity,0),2000);Equal(s.Circles[0].Result,Judgement.Pending);});
        Add("layout controlled center scale1",()=> {
            var l=PlayfieldLayout.Fit(640,480);Equal(l.Scale,1f);Equal(l.Offset,new Vector2(64,56));
            Equal(l.ToScreen(new(256,192)),new Vector2(320,248));Equal(l.ToMap(new(320,248)),new Vector2(256,192));
        });
        Add("layout controlled doubled scale",()=> {
            var l=PlayfieldLayout.Fit(1280,960);Equal(l.Scale,2f);
            Equal(l.ToScreen(new(256,192)),new Vector2(640,496));Equal(l.ToMap(new(640,496)),new Vector2(256,192));
        });
        Add("layout letterbox translation",()=> {
            var l=PlayfieldLayout.Fit(800,480);Equal(l.StageX,80f);
            Equal(l.ToMap(l.ToScreen(new(128,288))),new Vector2(128,288));
        });
        Add("new session state isolation",()=> {
            var a=One();var b=new CircleSession(a.Map);a.Press(Vector2.Zero,2000);a.Advance(5000);
            Equal(b.JudgedCount,0);Equal(b.Combo,0);Equal(b.MaxCombo,0);Equal(b.HitValueTotal,0);
            Equal(b.Accuracy,100.0);Equal(b.Feedback,null);Check(!b.Finished,"new session finished");
            Check(!ReferenceEquals(a.Circles[0],b.Circles[0]),"circle state reused");Equal(b.Circles[0].Result,Judgement.Pending);
        });
        Add("finished session ignores later component calls",()=> {
            var s=One();s.Advance(5000);Check(s.Finished,"local result horizon not reached");
            s.Press(Vector2.Zero,2000);s.Advance(6000);Equal(s.JudgedCount,1);Equal(s.Circles[0].Result,Judgement.Miss);
        });
        Add("bundled practice parser",()=> {
            var m=Beatmap.Load(Path.Combine(root,"reconstructed/CircleGame/Fixtures/first-circles.osu"));
            Equal(m.Circles.Count,8);Equal(m.CircleSize,4f);Equal(m.OverallDifficulty,4f);Equal(m.ApproachRate,4f);
            Equal(m.Circles[0].StartTime,3000);Equal(m.Circles[^1].StartTime,13500);
        });
        Add("parser clamp and truncate both axes",()=> {
            var m=Parse(MapText("-1,999,2000,1,0\n128.9,383.9,3000,1,0"));
            Equal(m.Circles[0].Position,new Vector2(0,512));Equal(m.Circles[1].Position,new Vector2(128,383));
        });
        Add("parser sorts unsorted input",()=> {
            var m=Parse(MapText("0,0,3000,1,0\n0,0,2000,5,0"));Equal(m.Circles[0].StartTime,2000);
        });
        foreach (var (label,text) in new (string,string)[] {
            ("mode",MapText().Replace("Mode:0","Mode:1")),
            ("version",MapText().Replace("v14","v15")),
            ("missing AR",MapText(difficulty:"CircleSize:5\nOverallDifficulty:5")),
            ("NaN difficulty",MapText().Replace("CircleSize:5","CircleSize:NaN")),
            ("range difficulty",MapText().Replace("CircleSize:5","CircleSize:11")),
            ("empty",MapText("")), ("slider",MapText("0,0,2000,2,0")),
            ("spinner",MapText("0,0,2000,8,0")), ("mixed type",MapText("0,0,2000,3,0")),
            ("equal times",MapText("0,0,2000,1,0\n100,100,2000,1,0")),
            ("negative time",MapText("0,0,-1,1,0")), ("large time",MapText("0,0,3600001,1,0")),
            ("nonfinite position",MapText("NaN,0,2000,1,0")), ("short row",MapText("0,0,2000,1")) })
            Add("parser reject "+label,()=>Reject(()=>Parse(text)));
        Add("parser object count limit",()=>Reject(()=>Parse(MapText(string.Join("\n",Enumerable.Range(0,10001).Select(i=>$"0,0,{i},1,0"))))));
        Add("parser byte size limit",()=>Reject(()=>Parse(MapText()+new string(' ',2*1024*1024))));
        Add("QA01-D02 reject malformed hitSound",()=>Reject(()=>Beatmap.Load(Path.Combine(root,"re/qa/qa01/fixtures/bad-hitsound.osu"))));
        Add("QA01-D02 reject empty hitSound",()=>Reject(()=>Parse(MapText("0,0,2000,1,"))));
    }
}
