using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Difficulty;
using Ohno.CircleGame.Rendering;
using Ohno.CircleGame.Selection;

internal static class Program
{
    static readonly List<object> checks = [];
    static readonly List<object> cases = [];
    static string id = "";
    static void Check(bool ok, string label)
    {
        checks.Add(new { id, label, pass = ok });
        if (!ok) throw new Exception($"P {id}: {label}");
    }
    static bool Near(double a, double b) => Math.Abs(a-b) <= .0001;
    static bool Near(Vector2 a, Vector2 b) => Near(a.X,b.X) && Near(a.Y,b.Y);
    static Vector2 V(JsonElement e) => new(e[0].GetSingle(),e[1].GetSingle());
    static bool Vectors(IEnumerable<Vector2> actual, JsonElement expected) => actual.Count() == expected.GetArrayLength() && actual.Zip(expected.EnumerateArray()).All(p=>Near(p.First,V(p.Second)));
    static void Reject<T>(Action action, string label) where T : Exception
    { try { action(); } catch(T) { Check(true,label); return; } throw new Exception($"P {id}: expected {typeof(T).Name}: {label}"); }
    static string Map(string rows, float stack = 0) => $"osu file format v14\n[General]\nMode:0\nStackLeniency:{stack.ToString(System.Globalization.CultureInfo.InvariantCulture)}\n[Metadata]\nTitle:P consumer control\n[Difficulty]\nHPDrainRate:5\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[TimingPoints]\n0,500,4,1,0,100,1,0\n[HitObjects]\n{rows}\n";
    static int Main(string[] args)
    {
        string repo = Path.GetFullPath(args[0]), folder = Path.Combine(repo,"re/campaigns/C001/perfect-circle");
        string output = args.Length>1 ? Path.GetFullPath(args[1]) : Path.Combine(repo,"reconstructed/campaigns/C001/perfect-development/results.json");
        string temp = Path.Combine(Path.GetTempPath(),"ohno-p-"+Guid.NewGuid().ToString("N")); Directory.CreateDirectory(temp);
        try
        {
            using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(folder,"manifest.json")));
            foreach(var fixture in manifest.RootElement.GetProperty("cases").EnumerateArray())
            {
                id = fixture.GetProperty("id").GetString()!;
                string path = Path.Combine(folder,fixture.GetProperty("path").GetString()!);
                byte[] bytes = File.ReadAllBytes(path); string text = File.ReadAllText(path);
                var expected = fixture.GetProperty("expected");
                Check(Convert.ToHexString(SHA256.HashData(bytes)).Equals(fixture.GetProperty("sha256").GetString(),StringComparison.OrdinalIgnoreCase),"unchanged Work1 fixture SHA256");
                if(id is "malformed-coordinate" or "all-coincident" or "singleton" or "vanishing-length")
                {
                    Reject<InvalidDataException>(()=>Beatmap.LoadCanonical(path),"explicit unsupported/malformed gameplay disposition");
                    if(id=="vanishing-length") Reject<InvalidDataException>(()=>SliderGeometryPreparation.Prepare(new(106,100),new Vector2[]{new(106,100),new(100,106),new(94,100)},SliderPathKind.Perfect,.00005),"empty adjusted collection remains explicit unsupported output");
                    cases.Add(new { id, disposition="REJECTED by finite engineering admission; not original-runtime equivalence", expected }); continue;
                }
                var map = Beatmap.LoadCanonical(path); var again = Beatmap.LoadCanonical(path); var slider = map.Sliders.Single(); var geometry = slider.Geometry;
                Check(map.Document!.CopySourceBytes()!.SequenceEqual(bytes) && geometry.SourceKind==SliderPathKind.Perfect && slider.RawPath==map.Document.Section("[HitObjects]").Single().Text.Split(',')[5],"source bytes/hash and raw P path retained");
                Check((int)geometry.PreparedKind==expected.GetProperty("storedPreparedKind").GetInt32(),"original count/determinant fallback kind");
                Check(geometry.SourceControls.SequenceEqual(slider.ControlPoints) && geometry.SourceControls.SequenceEqual(geometry.Controls),"normalized source controls preserved before stacking");
                if(expected.TryGetProperty("rawVertices",out var vertices))
                {
                    Check(geometry.RawSegments.Count==expected.GetProperty("rawSegmentCount").GetInt32() && Vectors(new[]{geometry.RawSegments[0].From}.Concat(geometry.RawSegments.Select(s=>s.To)),vertices),"all raw chord vertices against Work1 finite predictions");
                    var segments=expected.GetProperty("preparedSegments");
                    Check(geometry.Segments.Count==segments.GetArrayLength() && geometry.Segments.Zip(segments.EnumerateArray()).All(p=>Near(p.First.From,V(p.Second[0]))&&Near(p.First.To,V(p.Second[1]))),"all adjusted segment endpoints against Work1 predictions");
                    Check(Near(geometry.UnadjustedLength,expected.GetProperty("unadjustedChordLength").GetDouble()) && geometry.CumulativeLengths.Count==expected.GetProperty("cumulativeLengths").GetArrayLength() && geometry.CumulativeLengths.Zip(expected.GetProperty("cumulativeLengths").EnumerateArray()).All(p=>Near(p.First,p.Second.GetDouble())),"ordered Single chord/Double cumulative lengths against Work1 predictions");
                }
                else
                {
                    // Work1 v9 P4 fixture establishes dispatch only. This comparison
                    // is retained-production regression, not new independent math.
                    var inherited=SliderGeometryPreparation.Prepare(slider.Head.Position,geometry.SourceControls,true,slider.PixelLength,fixture.GetProperty("version").GetInt32());
                    Check(geometry.RawSegments.SequenceEqual(inherited.RawSegments)&&geometry.Segments.SequenceEqual(inherited.Segments)&&geometry.RawSegments.Count==199,"v9 P4 delegates unchanged version-aware Bezier (dispatch-only authority)");
                }
                if(expected.TryGetProperty("segmentFlags",out var flags))
                    Check(geometry.RawSegments.All(s=>s.IsLinear==flags.GetProperty("IsLinear").GetBoolean()&&s.IsSubcurveEnd==flags.GetProperty("IsSubcurveEnd").GetBoolean()),"original segment flags");
                if(expected.TryGetProperty("samplingInteger",out var n))
                {
                    var circle=geometry.SourcePerfectCircle!;
                    Check(circle.SamplingCount==n.GetInt32() && Near(circle.Center,V(expected.GetProperty("center"))) && Near(circle.Radius,expected.GetProperty("radius").GetDouble()) && Math.Abs(circle.StartAngle-expected.GetProperty("startAngle").GetDouble())<1e-10 && Math.Abs(circle.EndAngle-expected.GetProperty("endAngle").GetDouble())<1e-10,"circumcenter/radius/signed angles/sample count against Work1 predictions");
                    Check(geometry.RawSegments[^1].To==geometry.SourceControls[^1]&&SliderGeometryPreparation.PerfectAngleWrap==6.2831854820251465,"original endpoint and non-2pi angle wrap literal");
                }
                Check(slider.Movements.Count==geometry.Segments.Count*slider.Traversals && Near(slider.EndPosition,slider.Traversals%2==0?slider.Head.Position:slider.Tail),"shared traversal count and odd/even endpoint");
                Check(slider.Samples!.Schedule.SequenceEqual(slider.Checkpoints.Select(c=>c.Time)) && slider.Movements.Count==again.Sliders.Single().Movements.Count && slider.Movements.SequenceEqual(again.Sliders.Single().Movements),"sample schedule and stable reload (no cumulative preparation)");
                Check(map.HealthSource!.IsFreshFor(map)&&ReferenceEquals(map.CircleSounds!.RegisteredOverrides.Single().Slider,slider),"fresh ownership and resource operand identity");
                var health=HealthChart.FromMap(map,1200,HealthPreparationPolicy.E036FreshOrdinary);
                Check(health.Objects.Single().ScheduleCount==slider.Checkpoints.Count&&ReferenceEquals(health.NewComboProvenance.Single().Identity,slider),"health checkpoint contract and NewCombo registered identity");
                var selected=SliderSampleSelection.Edge(slider,slider.Samples,map.CircleSounds!.Timing,new(false,false,true,0,100,false,100,100),0);
                Check(selected.BaseSet==1&&selected.Volume==100,"existing slider sound metadata selection");
                var layout=PlayfieldLayout.Fit(1280,720); int middle=slider.Head.StartTime+(slider.EndTime-slider.Head.StartTime)/2;
                Check(Near(layout.ToMap(layout.ToScreen(slider.BallPosition(middle))),slider.BallPosition(middle)),"actual render/input layout transforms consume prepared ball coordinates");
                var session=new CircleSession(map,ScoreConfiguration.EngineeringPractice,false); var sounds=new List<SliderSoundEvent>(); session.SliderSound+=e=>sounds.Add(e);
                session.Press(slider.Head.Position,slider.Head.StartTime); for(int t=slider.Head.StartTime;t<=slider.EndTime+64;t++) session.Advance(t,slider.BallPosition(t),1);
                var referenceSlider=slider with { Geometry=geometry with { SourceKind=SliderPathKind.Linear,PreparedKind=SliderPathKind.Linear } };
                var referenceMap=map with { Sliders=new[]{referenceSlider} };
                var reference=new CircleSession(referenceMap,ScoreConfiguration.EngineeringPractice,false);
                var referenceSounds=new List<SliderSoundEvent>();reference.SliderSound+=e=>referenceSounds.Add(e);
                reference.Press(referenceSlider.Head.Position,referenceSlider.Head.StartTime);
                for(int t=referenceSlider.Head.StartTime;t<=referenceSlider.EndTime+64;t++)reference.Advance(t,referenceSlider.BallPosition(t),1);
                var state=session.Sliders[0];var baseline=reference.Sliders[0];
                Check(state.Head.Result==Judgement.Hit300&&state.ProcessedCheckpoints==baseline.ProcessedCheckpoints&&state.SuccessfulCheckpoints==baseline.SuccessfulCheckpoints&&state.Result==baseline.Result&&session.Ledger.Score==reference.Ledger.Score&&sounds.Count>0&&sounds.All(e=>ReferenceEquals(e.Parent,slider))&&sounds.Select(e=>(e.Kind,e.Index,e.Time)).SequenceEqual(referenceSounds.Select(e=>(e.Kind,e.Index,e.Time))),"actual P session preserves shared prepared-contract judgement/score/checkpoint/sound history (including nonmonotone final scheduling)");
                cases.Add(new { id, disposition="finite gameplay loaded", rawSegments=geometry.RawSegments.Count, preparedSegments=geometry.Segments.Count, preparedKind=(int)geometry.PreparedKind, slider.EndTime, successful=state.SuccessfulCheckpoints,scheduled=slider.Checkpoints.Count,checkpointTimes=slider.Checkpoints.Select(c=>c.Time).ToArray(), end=new[]{slider.EndPosition.X,slider.EndPosition.Y}, oracleScope=id=="four-controls-v9-dispatch"?"dispatch only":"declared finite coordinate/length expectations" });
            }
            id="extra-contracts"; string file=Path.Combine(temp,"map.osu");
            File.WriteAllText(file,Map("400,200,1000,2,0,P|400:200|304:296|208:200,1,300"));var large=Beatmap.LoadCanonical(file);var def=large.Sliders.Single();
            var good=new CircleSession(large,ScoreConfiguration.EngineeringPractice,false); var wrong=new CircleSession(large,ScoreConfiguration.EngineeringPractice,false);
            foreach(var s in new[]{good,wrong}){s.Press(def.Head.Position,1000);s.Advance(1000,def.Head.Position,1);}
            int mid=1000+(def.EndTime-1000)/2;good.Advance(mid,def.BallPosition(mid),1);wrong.Advance(mid,new(304,200),1);
            Check(good.Sliders[0].FollowEligible&&!wrong.Sliders[0].FollowEligible,"input follows sampled arc rather than first-last chord");
            Reject<InvalidDataException>(()=>Beatmap.Load(file),"historical string loader still rejects P");
            Reject<UnsupportedStarProfileException>(()=>StarAdmission.Parse(File.ReadAllText(file)),"R17 strict admission unchanged");
            using var cancelled=new CancellationTokenSource();cancelled.Cancel();
            Reject<OperationCanceledException>(()=>Beatmap.LoadCanonical(file,cancelled.Token),"canonical P cancellation");
            Reject<OperationCanceledException>(()=>SliderGeometryPreparation.Prepare(new(102,100),new Vector2[]{new(102,100),new(100,102),new(98,100)},SliderPathKind.Perfect,4,14,cancelled.Token),"geometry cancellation before allocation");
            Reject<InvalidDataException>(()=>SliderGeometryPreparation.Prepare(new(1000000,0),new Vector2[]{new(1000000,0),new(0,1000000),new(-1000000,0)},SliderPathKind.Perfect,100),"engineering sample allocation bound");
            var noEditorCap=SliderGeometryPreparation.Prepare(new(500,0),new Vector2[]{new(500,0),new(0,500),new(-500,0)},SliderPathKind.Perfect,1600);
            Check(noEditorCap.SourcePerfectCircle!.Radius==500&&noEditorCap.RawSegments.Count>1,"ordinary Play does not impose editor 640 bounds");
            File.WriteAllText(file,Map("102,100,1000,2,0,P|102:100|100:102|98:100,1,4\n102,100,1100,1,0",.7f));
            var stacked=Beatmap.LoadCanonical(file);var p=stacked.Sliders.Single();Vector2 shift=new(-3.2013118267059326f);
            Check(Near(p.Head.Position,new Vector2(102,100)+shift)&&p.Geometry.SourceControls[0]==new Vector2(102,100)&&p.Geometry.RawSegments[0].From==new Vector2(102,100),"existing positive stacking translates P while preserving raw geometry");
            Check(p.Geometry.Controls.Zip(p.Geometry.SourceControls).All(x=>Near(x.First,x.Second+shift))&&Near(p.Movements[0].From,p.Head.Position)&&ReferenceEquals(stacked.CircleSounds!.RegisteredOverrides[0].Slider,p)&&ReferenceEquals(HealthChart.FromMap(stacked,1200,HealthPreparationPolicy.E036FreshOrdinary).NewComboProvenance[0].Identity,p),"stacked P controls/movement/resource/health identities stay coherent");
            Check(Beatmap.LoadCanonical(file).Sliders.Single().Head.Position==p.Head.Position,"stacked P reload does not accumulate offsets");
            File.WriteAllText(file,Map("102,100,1000,2,0,P|102:100|100:102|98:100,1,4\n98,100,1100,1,0",.7f));
            Check(Near(Beatmap.LoadCanonical(file).Circles.Single().Position,new Vector2(98,100)-shift),"existing negative slider-tail stack uses prepared P tail");
            File.WriteAllText(file,Map("102,100,1000,2,0,P|102:100|100:102|98:100,1,4\n100,100,3000,2,0,L|120:100,1,20\n100,100,5000,2,0,B|101:101|102:100,1,4"));
            Check(Beatmap.LoadCanonical(file).Sliders.Select(s=>s.Geometry.SourceKind).SequenceEqual(new[]{SliderPathKind.Perfect,SliderPathKind.Linear,SliderPathKind.Bezier}),"mixed P/L/B registration in existing consumers");
            File.WriteAllText(file,Map("100,100,1000,2,0,P|100:100|broken,1,4"));Reject<InvalidDataException>(()=>Beatmap.LoadCanonical(file),"malformed replacement rejects");
            Check(ReferenceEquals(good.Map,large)&&good.Sliders.Single().Definition==def,"failed replacement leaves existing session untouched");
            File.WriteAllText(file,Map("102,100,1000,2,0,P|100:102|98:100,1,4"));
            var inserted=Beatmap.LoadCanonical(file).Sliders.Single();
            Check(inserted.Geometry.SourceControls.Count==3&&inserted.Geometry.PreparedKind==SliderPathKind.Perfect&&inserted.Geometry.SourcePerfectCircle is not null,"head insertion precedes P count dispatch");
            File.WriteAllText(file,Map("100,100,1000,2,0,P|104:100,1,12"));
            Check(Beatmap.LoadCanonical(file).Sliders.Single().Geometry.PreparedKind==SliderPathKind.Linear,"single supplied endpoint plus inserted head selects Linear fallback");
            string render=File.ReadAllText(Path.Combine(repo,"reconstructed/CircleGame/Rendering/PlayfieldView.cs"));
            Check(render.Contains("s => BuildPath(s.Geometry)")&&render.Contains("foreach (var s in source.Segments)")&&render.Contains("definition.BallPosition(CurrentTime)"),"SOURCE REVIEW render path/ball already consume shared prepared geometry; no GUI execution");
            Directory.CreateDirectory(Path.GetDirectoryName(output)!);
            File.WriteAllText(output,JsonSerializer.Serialize(new { classification="Work2 production development; Work1 fixture predictions reused, subsequent independent actual-source QA pending",originalRuntime="NOT RUN",source=Environment.GetEnvironmentVariable("GITHUB_SHA")??"LOCAL WORKING TREE - hashes below",runtime=Environment.Version.ToString(),productionSha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(repo,"reconstructed/CircleGame/Gameplay/Beatmap.cs")))),testSha256=Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(repo,"reconstructed/C001PerfectChecks/Program.cs")))),groups=checks.Count,fixtureCount=cases.Count,results=checks,cases },new JsonSerializerOptions{WriteIndented=true}));
            Console.WriteLine($"C001 PERFECT DEVELOPMENT: {checks.Count}/{checks.Count} PASS; {cases.Count} fixture dispositions; no original/GUI/audio execution");return 0;
        }
        finally{Directory.Delete(temp,true);}
    }
}
