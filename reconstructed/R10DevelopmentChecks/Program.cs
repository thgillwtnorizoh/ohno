// Bounded E036 N01–N22 controls, direct production calls, not independent QA08.
using System.Numerics;
using Ohno.CircleGame.Gameplay;

if (args.Length != 1) throw new ArgumentException("Pass repository root.");
string fixtures = Path.Combine(args[0], "reconstructed", "CircleGame", "Fixtures");
string scratch = Path.Combine(Path.GetTempPath(), "ohno-r10-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(scratch);
int groups = 0, file = 0;
try
{
    foreach (string kind in new[] { "C0", "L0" })
    {
        var facts = Pure([kind]); Flags(facts, "T"); Eq(facts[0].LastRegisteredBefore, OriginalRegisteredType.Null);
        Eq(facts[0].Inferred, (bool?)true); Eq(facts[0].FactoryArgument, (bool?)true); Eq(facts[0].FileBit4, false);
    }
    Pass("N01 fresh first unmarked circle/slider infers true before file OR");
    var explicitCircle = Pure(["C0", "C1"]); Flags(explicitCircle, "TT");
    Eq(explicitCircle[1].Inferred, (bool?)false); Eq(explicitCircle[1].FactoryArgument, (bool?)true);
    Check(explicitCircle[1].FileBit4); // Helper still evaluated even when bit4 is true.
    Pass("N02 explicit file bit preserves helper evaluation and precedence");
    Flags(Pure(["C0", "C0"]), "TF"); Pass("N03 consecutive unmarked circles");
    Flags(Pure(["C0", "L0"]), "TF"); Flags(Pure(["C0", "L1"]), "TT");
    Pass("N04/N05 circle-to-slider, explicit slider bit and no type-change default");
    Flags(Pure(["L0", "C0"]), "TF"); Pass("N06 slider-to-circle");
    Flags(Pure(["L0", "L0"]), "TF"); Pass("N07 consecutive unmarked sliders");
    var mixedFlags = Pure(["C0", "L0", "C0", "L0"]); Flags(mixedFlags, "TFFF");
    Eq(mixedFlags[1].LastRegisteredBefore, OriginalRegisteredType.Circle);
    Eq(mixedFlags[2].LastRegisteredBefore, OriginalRegisteredType.Slider);
    Pass("N08 multiple registrations retain last registered type, not judgement or last flag");
    var spinner = Pure(["C0", "P0", "C0"]); Flags(spinner, "TFT");
    Eq(spinner[1].Inferred, (bool?)null); Eq(spinner[1].FactoryArgument, (bool?)null);
    Eq(spinner[1].ParserFlag, false); Eq(spinner[2].Inferred, (bool?)true);
    Pass("N09 spinner bypasses helper; following circle infers true independently");
    Flags(Pure(["L0", "P1", "L0"]), "TTT"); Flags(Pure(["P0", "P0", "L0"]), "FFT");
    Pass("N10/N11 explicit spinner, consecutive spinners and following slider");
    foreach (int version in new[] { 8, 9, 14 })
    {
        var old = Pure(["P0", "C0"], format: version); Flags(old, "FT");
        Eq(old[0].ConstructorFlag, version <= 8); Eq(old[0].ParserFlag, false); Eq(old[0].Inferred, (bool?)null);
    }
    Reject(() => Load(Rows(["P0", "C0"]), format: 8)); // Actual R07 admission still v14 only.
    Pass("N12/N13 parser overwrites old spinner constructor flag, without extending gameplay format support");
    var after = Pure(["C0", "C0"], starts: [1000, 3000], breaks: [new(1500, 2500)]);
    Flags(after, "TT"); Eq(after[1].ParserFlag, false); Eq(after[1].ExpiredBreaksConsumed, 1);
    Pass("N14 strict expired-break forcing preserves factory and final flag provenance");
    var equality = Pure(["C0", "C0", "C0"], starts: [1000, 3000, 3001], breaks: [new(2000, 3000)]);
    Flags(equality, "TFT"); Eq(equality[1].ExpiredBreaksConsumed, 0); Eq(equality[2].ExpiredBreaksConsumed, 1);
    Flags(Pure(["C0", "P0", "C0"], starts: [1000, 3000, 5000], breaks: [new(1500, 2500)]), "TTT");
    Pass("N15/N16 equality stays pending and break forcing applies to spinner");
    var multi = Pure(["C0", "C0"], starts: [1000, 5000], breaks: [new(1500, 2000), new(3500, 4000)]);
    Flags(multi, "TT"); Eq(multi[1].ExpiredBreaksConsumed, 2);
    var blocked = Pure(["C0", "C0"], starts: [1000, 5000], breaks: [new(8000, 9000), new(1500, 2000)]);
    Flags(blocked, "TF"); Eq(blocked[1].ExpiredBreaksConsumed, 0); // Explicit supplied-order probe, not accepted map ordering.
    Pass("N17 multiple expired breaks consumed; pending head blocks later entries, no sorting");
    Eq(NewComboPreparation.Infer(OriginalRegisteredType.Circle), false);
    Eq(NewComboPreparation.Infer(OriginalRegisteredType.Slider), false);
    Eq(NewComboPreparation.Infer(OriginalRegisteredType.SpinnerBase), true);
    Eq(NewComboPreparation.Infer(OriginalRegisteredType.AssignableT081f), true);
    Reject(() => NewComboPreparation.Infer(OriginalRegisteredType.Unknown));
    foreach (var last in new[] { OriginalRegisteredType.Circle, OriginalRegisteredType.SpinnerBase, OriginalRegisteredType.AssignableT081f, OriginalRegisteredType.Null })
        Reject(() => NewComboPreparation.Prepare([], [], 14, new(NewComboOrigin.RetainedManager, last, true)));
    var ordinary = new[] { new NewComboInput(new object(), 2000, 2, OriginalRegisteredType.Slider) };
    foreach (NewComboOrigin origin in new[] { NewComboOrigin.CloneOrEditor, NewComboOrigin.AlternateFactory, NewComboOrigin.Unknown })
        Reject(() => NewComboPreparation.Prepare(ordinary, [], 14, new(origin, OriginalRegisteredType.Null, true)));
    Reject(() => NewComboPreparation.Prepare([ordinary[0] with { RuntimeType = OriginalRegisteredType.AssignableT081f }], [], 14, NewComboContext.FreshZeroShift));
    Pass("N18–N20/N22 direct stable type predicate versus unsupported retained/alternate lifecycle; empty list proves no reset");
    Reject(() => Pure(["C0", "L0"], starts: [4000, 2000])); Reject(() => Pure(["C0", "C0"], starts: [2000, 2000]));
    Reject(() => NewComboPreparation.Prepare(ordinary, [], 14, NewComboContext.FreshZeroShift with { SharedTimeDomain = false }));
    Reject(() => NewComboPreparation.Prepare([ordinary[0], ordinary[0] with { Start = 4000 }], [], 14, NewComboContext.FreshZeroShift));
    var unorderedMap = Load("128,192,4000,1,0\n256,192,2000,1,0");
    var unavailable = Fresh(unorderedMap); Eq(unavailable.Health, (OrdinaryHealth?)null); Check(unavailable.HealthStatus.Contains("increasing"));
    Reject(() => Load("128,192,2000,1,0\n256,192,2000,1,0"));
    Pass("N21 source/registration order, equal-time/identity/domain guards retained without chronological repair");
    var normal = Load(Rows(["C0", "C0"]));
    Eq(normal.HealthSource!.IsFreshFor(normal), true);
    var clone = normal with { Title = "clone" }; Eq(clone.HealthSource!.IsFreshFor(clone), false);
    Eq(Fresh(clone).Health, (OrdinaryHealth?)null);
    var proof = HealthChart.FromMap(normal, 1200, HealthPreparationPolicy.E036FreshOrdinary).NewComboProvenance;
    Check(ReferenceEquals(proof[0].Identity, normal.Circles[0])); Eq(proof[0].SourceIndex, 0); Eq(proof[1].SourceIndex, 1);
    var colour = NewComboPreparation.Prepare([new(new object(), 1000, 1, OriginalRegisteredType.Circle),
        new(new object(), 2000, 37, OriginalRegisteredType.Circle)], [], 14, NewComboContext.FreshZeroShift);
    Eq(colour[1].RawType, 37); Eq(colour[1].FileColourSkip, 2); Eq(colour[1].Inferred, (bool?)false); Eq(colour[1].FactoryArgument, (bool?)true);
    Pass("fresh parser ownership, exact registered definition identity and raw/file/colour/factory provenance");

    var ordinaryRewards = Load("128,192,2000,1,0\n256,192,2100,1,0", hp: 10);
    var explicitRewards = Load("128,192,2000,1,0\n256,192,2100,5,0", hp: 10);
    var a = new HealthCalibration(); var b = new HealthCalibration();
    var aChart = HealthChart.FromMap(ordinaryRewards, 0, HealthPreparationPolicy.E036FreshOrdinary);
    var bChart = HealthChart.FromMap(explicitRewards, 0, HealthPreparationPolicy.E036FreshOrdinary);
    Eq(a.Trial(aChart), HealthRetry.None); Eq(b.Trial(bChart), HealthRetry.None);
    Eq(a.LastA, 221.0); Eq(b.LastA, 235.0); // Literal E035:6-5+20 vs20-5+20, plus200.
    var unusedFirst = aChart with { Objects = [aChart.Objects[0] with { NewCombo = false }, aChart.Objects[1]] };
    var first = new HealthCalibration(); Eq(first.Trial(unusedFirst), HealthRetry.None); Eq(first.LastA, 221.0);
    Pass("final next flag selects private1024/1028; first flag is unused, literal balance221/235");
    var twoBreakMap = Load("128,192,1000,1,0\n256,192,5000,1,0", "2,1500,2000\n2,3500,4000", 10);
    var twoBreakChart = HealthChart.FromMap(twoBreakMap, 0, HealthPreparationPolicy.E036FreshOrdinary);
    Eq(twoBreakChart.NewComboProvenance[1].ExpiredBreaksConsumed, 2);
    var breakTrial = new HealthCalibration(); Eq(breakTrial.Trial(twoBreakChart), HealthRetry.PreObject);
    Eq(breakTrial.LastBreaksConsumed, 1); Eq(breakTrial.LastExcludedGap, 1000); Eq(breakTrial.LastGapLoss, 150.0);
    var equalBreakMap = Load("128,192,1000,1,0\n256,192,3000,1,0\n384,192,5000,1,0", "2,2000,3000", 10);
    var equalChart = HealthChart.FromMap(equalBreakMap, 0, HealthPreparationPolicy.E036FreshOrdinary);
    Eq(equalChart.Objects[1].NewCombo, (bool?)false); Eq(equalChart.Objects[2].NewCombo, (bool?)true);
    var equalTrial = new HealthCalibration(); equalTrial.Trial(equalChart); Eq(equalTrial.LastBreaksConsumed, 1);
    Pass("break grouping while-loop remains distinct from calibration one-break gap exclusion and equality");
    string denseRows = string.Join('\n', Enumerable.Range(0, 5).Select(i => $"128,192,{2000 + i * 100},1,0"));
    var dense = Fresh(Load(denseRows)); var allGroups = Fresh(Load(denseRows.Replace(",1,0", ",5,0")));
    Check(dense.Health is not null && allGroups.Health is not null);
    var d = dense.Health!.Calibration; var marked = allGroups.Health!.Calibration;
    Eq(marked.Trials, 62); Eq(marked.B, 1.0); Eq(marked.G, 1.0); Eq(marked.BalanceRetries, 0);
    Check(d.BalanceRetries > 0 && d.Rate < marked.Rate && d.B > marked.B && d.G > marked.G);
    Eq(dense.Ledger.Score, 0); Eq(dense.Ledger.EventCount, 0); Eq(dense.JudgedCount, 0); Eq(dense.Health.ResultEvents, 0);
    // Same textual starts/difficulty -> same E034 K. Grouping modifies only private HP preparation.
    Eq(dense.Scoring.K, allGroups.Scoring.K);
    Console.WriteLine($"R10 CALIBRATION control unmarked r={d.Rate:R},B={d.B:R},G={d.G:R},trials={d.Trials}; marked r={marked.Rate:R},B={marked.B:R},G={marked.G:R},trials={marked.Trials}");
    Pass("group boundaries materially change calibrated r/B/G while trial score/count/combo stay zero");
    var three = Fresh(Load(Rows(["C0", "C0", "C0"])));
    Check(three.Health is not null); Eq(three.HealthPolicy, HealthPreparationPolicy.E036FreshOrdinary);
    Eq(three.Health!.Chart.Objects[0].NewCombo, (bool?)true); Eq(three.Health.Chart.Objects[1].NewCombo, (bool?)false);
    var bundled = Fresh(Beatmap.Load(Path.Combine(fixtures, "first-circles.osu"))); Check(bundled.Health is not null);
    foreach (string name in new[] { "first-slider", "slider-repeat", "slider-ticks", "mixed-r07", "health-r09" })
        Check(Fresh(Beatmap.Load(Path.Combine(fixtures, name + ".osu"))).Health is not null);
    Pass("newly eligible three-circle control, unchanged bundled circles and supported mixed/sliders prepare HP");
    var legacy = new CircleSession(normal, ScoreConfiguration.FromBeatmap(normal));
    Eq(legacy.HealthPolicy, HealthPreparationPolicy.HistoricalR09); Eq(legacy.Health, (OrdinaryHealth?)null);
    Check(legacy.HealthStatus.Contains("M2902")); Check(Fresh(normal).Health is not null);
    Eq(HealthChart.FromMap(normal, 1200, HealthPreparationPolicy.E036FreshOrdinary).PreparationPolicy, HealthPreparationPolicy.E036FreshOrdinary);
    Pass("owner-selected versioned R09 compatibility remains separate from explicit R10 app contract");
    three.Health.State.Assign(0); three.Health.CheckFailure(77);
    var restarted = Fresh(three.Map); Check(!ReferenceEquals(restarted.Health, three.Health));
    Eq(restarted.Health!.State.H, 200.0); Eq(restarted.Health.State.A, 200.0); Eq(restarted.Health.State.V, 0.0); Eq(restarted.Health.Failed, false);
    Eq(restarted.Health.Chart.NewComboProvenance[0].LastRegisteredBefore, OriginalRegisteredType.Null);
    var replaced = Fresh(Load(Rows(["P0", "C0"]))); Eq(replaced.Health!.Chart.Objects[0].NewCombo, (bool?)false); Eq(replaced.Health.Chart.Objects[1].NewCombo, (bool?)true);
    Eq(replaced.Ledger.EventCount, 0); Eq(restarted.Ledger.Score, 0);
    Pass("restart/new map gets fresh null manager model and isolated calibration/failure/ledger");
    var mixed = Beatmap.Load(Path.Combine(fixtures, "mixed-r07.osu")); var enabled = Fresh(mixed); var disabled = Fresh(mixed, false);
    var slider = mixed.Sliders.Single();
    for (int time = 0; time <= 18000; time += 10)
    {
        Vector2 pointer = new(256, 192); int held = 0;
        if (time >= slider.Head.StartTime && time <= slider.EndTime) { pointer = slider.BallPosition(time); held = 1; }
        foreach (var c in mixed.Circles.Where(c => c.StartTime == time)) { enabled.Press(c.Position, time); disabled.Press(c.Position, time); }
        if (time == slider.Head.StartTime) { enabled.Press(pointer, time); disabled.Press(pointer, time); }
        enabled.Advance(time, pointer, held); disabled.Advance(time, pointer, held);
        Eq(enabled.Ledger.Score, disabled.Ledger.Score); Eq(enabled.Combo, disabled.Combo); Eq(enabled.MaxCombo, disabled.MaxCombo);
        Eq(enabled.Accuracy, disabled.Accuracy); Eq(enabled.Ledger.TotalWholeResults, disabled.Ledger.TotalWholeResults);
        Check(enabled.ObjectResults.SequenceEqual(disabled.ObjectResults));
    }
    Eq(enabled.Health!.ResultEvents, enabled.Ledger.EventCount);
    Pass("matched explicit component lifecycle preserves score/accuracy/combo/judgements under fresh grouping");
    Console.WriteLine($"R10 NEWCOMBO CHECKS: {groups} passed (E036 finite controls, separate from independent QA; no game window or OS input)");
}
finally { Directory.Delete(scratch, true); }

CircleSession Fresh(Beatmap map, bool hp = true) => CircleSession.CreateFreshOrdinary(map, ScoreConfiguration.FromBeatmap(map), hp);
Beatmap Load(string rows, string events = "", int hp = 0, int format = 14)
{
    string path = Path.Combine(scratch, $"map{file++}.osu");
    File.WriteAllText(path, $"osu file format v{format}\n[General]\nMode:0\n[Difficulty]\nHPDrainRate:{hp}\nCircleSize:5\nOverallDifficulty:5\nApproachRate:5\nSliderMultiplier:1\nSliderTickRate:1\n[Events]\n{events}\n[TimingPoints]\n0,1000,4,1,0,100,1,0\n[HitObjects]\n{rows}\n");
    return Beatmap.Load(path);
}
static string Rows(string[] types) => string.Join('\n', types.Select((v, i) =>
{
    int time = 2000 + 4000 * i, bit = v[1] == '1' ? 4 : 0;
    return v[0] switch { 'C' => $"128,192,{time},{1 | bit},0", 'L' => $"128,192,{time},{2 | bit},0,L|228:192,1,100",
        _ => $"256,192,{time},{8 | bit},0,{time + 2000}" };
}));
static IReadOnlyList<PreparedNewCombo> Pure(string[] types, int[]? starts = null, HealthBreak[]? breaks = null, int format = 14)
{
    var inputs = types.Select((v, i) => new NewComboInput(new object(), starts?[i] ?? 2000 + 4000 * i,
        (v[0] == 'C' ? 1 : v[0] == 'L' ? 2 : 8) | (v[1] == '1' ? 4 : 0),
        v[0] == 'C' ? OriginalRegisteredType.Circle : v[0] == 'L' ? OriginalRegisteredType.Slider : OriginalRegisteredType.SpinnerBase)).ToArray();
    return NewComboPreparation.Prepare(inputs, breaks ?? [], format, NewComboContext.FreshZeroShift);
}
static void Flags(IReadOnlyList<PreparedNewCombo> p, string expected) => Eq(string.Concat(p.Select(x => x.FinalFlag ? "T" : "F")), expected);
void Pass(string name) { groups++; Console.WriteLine("PASS R10 " + name); }
static void Check(bool condition) { if (!condition) throw new Exception("Bounded NewCombo assertion failed."); }
static void Reject(Action action) { try { action(); } catch (InvalidDataException) { return; } throw new Exception("Unsupported NewCombo state accepted."); }
static void Eq<T>(T actual, T expected) { if (!EqualityComparer<T>.Default.Equals(actual, expected)) throw new Exception($"Expected {expected}; got {actual}"); }
