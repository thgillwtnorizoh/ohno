using Ohno.CircleGame.Gameplay;

int groups = 0;
void Yes(bool value) { if (!value) throw new Exception("R13-C01 assertion failed in group " + (groups + 1)); }
void Bits(float value, uint expected) => Yes(BitConverter.SingleToUInt32Bits(value) == expected);
void Pass(string name) => Console.WriteLine($"PASS C{++groups:00} {name}");
(SharedSliderChannels Model, RecordingSampleChannels Backend) Pair(int volume = 25)
{
    var backend = new RecordingSampleChannels();
    var model = new SharedSliderChannels(new GlobalSampleTiming([new SampleTiming(0, 1, 1, volume, true, 0)]), backend,
        (row, set, index) => row == 0 ? 101 : 202);
    model.Clock(0);
    backend.Operations.Clear();
    return (model, backend);
}

var ordinary = Pair();
ordinary.Model.SettingsChanged(80, 50);
Yes(ordinary.Backend.Operations.Select(o => (o.Name, o.Channel, o.Attribute)).SequenceEqual(new[] { ("Attribute", 101, 2), ("Attribute", 202, 2) }));
foreach (var op in ordinary.Backend.Operations) Bits(op.Value, 0x3dcccccd);
Yes(ordinary.Model.EffectVolume == 80 && ordinary.Model.UniversalVolume == 50 && ordinary.Model.Pending == 0);
Pass("ordinary settings refresh: both gains0.1, immediate ordered writes");

var mutation = Pair();
mutation.Backend.OnOperation = op => { if (op.Channel == 101) mutation.Model.UniversalVolume = 100; };
mutation.Model.SettingsChanged(80, 50);
Yes(mutation.Backend.Operations.Count == 2);
foreach (var op in mutation.Backend.Operations) Bits(op.Value, 0x3dcccccd);
Yes(mutation.Model.UniversalVolume == 100);
Console.WriteLine("R13-C01 G08 control: expected0.1/0.1 actual=" + string.Join("/", mutation.Backend.Operations.Select(o => o.Value.ToString("R", System.Globalization.CultureInfo.InvariantCulture))));
Pass("controlled settings mutation retains captured gain, not rolled-back settings");

var target = Pair();
target.Backend.OnOperation = op => { if (op.Channel == 101) { target.Model.Channel1 = 402; target.Model.UniversalVolume = 100; } };
target.Model.SettingsChanged(80, 50);
Yes(target.Backend.Operations.Select(o => o.Channel).SequenceEqual(new[] { 101, 402 }));
foreach (var op in target.Backend.Operations) Bits(op.Value, 0x3dcccccd);
Pass("fresh second channel target402 with original captured gain0.1");

// Literal IEEE32 expectations independently published by QA11 G02/G05/E040;
// these controls compare production output bits, not a copied gain implementation.
var rounding = Pair(17);
rounding.Model.SettingsChanged(1, 1);
foreach (var op in rounding.Backend.Operations) Bits(op.Value, 0x378e9b38);
var overflow = Pair(100);
overflow.Model.SettingsChanged(46341, 46341);
foreach (var op in overflow.Backend.Operations) Bits(op.Value, 0xc851b6fa);
Pass("Single operation order and unchecked Int32 multiplication preserved");

foreach (int failingChannel in new[] { 101, 202 })
{
    var failure = Pair();
    failure.Backend.OnOperation = op =>
    {
        if (op.Channel == 101) { failure.Model.UniversalVolume = 100; failure.Model.Channel1 = 402; }
        if (op.Channel == (failingChannel == 101 ? 101 : 402)) throw new IOException("controlled settings failure");
    };
    bool threw = false;
    try { failure.Model.SettingsChanged(80, 50); } catch (IOException) { threw = true; }
    Yes(threw && failure.Model.EffectVolume == 80 && failure.Model.UniversalVolume == 100 && failure.Model.Channel1 == 402);
    Yes(failure.Backend.Operations.Count == (failingChannel == 101 ? 1 : 2));
    foreach (var op in failure.Backend.Operations) Bits(op.Value, 0x3dcccccd);
    Yes(failure.Model.Pending == 0 && failure.Model.LastIndex == 1 && failure.Model.LastBase == 1);
}
Pass("first/second write exceptions propagate; partial mutations survive; no extra write or changed gain");

var paths = Pair(17);
paths.Model.EffectVolume = paths.Model.UniversalVolume = 1;
paths.Backend.States[101] = paths.Backend.States[202] = 1;
paths.Model.PreparePair(3, 2);
Yes(paths.Backend.Operations.Where(o => o.Name == "Stop").Select(o => o.Channel).SequenceEqual(new[] { 101, 202 }));
foreach (var op in paths.Backend.Operations.Where(o => o.Name == "Attribute" && o.Attribute == 2)) Bits(op.Value, 0x378e9b38);
paths.Backend.States.Clear(); paths.Backend.Operations.Clear();
paths.Model.Submit(0, 3, 2, false, 0);
// Deferred start must still read fresh timing and settings rather than the settings-refresh local.
paths.Model.EffectVolume = paths.Model.UniversalVolume = 100;
paths.Model.Channel0 = 301; paths.Model.Channel1 = 402;
paths.Model.Drain();
Bits(paths.Backend.Operations.First(o => o.Name == "Attribute" && o.Attribute == 2).Value, BitConverter.SingleToUInt32Bits(.17f));
Yes(paths.Backend.Operations.Single(o => o.Name == "Play").Channel == 301);
foreach (var op in paths.Backend.Operations.Where(o => o.Attribute == 3)) Yes(Math.Abs(op.Value - (-.16f)) < .000001f);
paths.Model.EffectVolume = paths.Model.UniversalVolume = 1;
paths.Backend.Operations.Clear(); paths.Model.Submit(0, 3, 2, false, null); paths.Model.Drain();
Bits(paths.Backend.Operations.First(o => o.Attribute == 2).Value, 0x378e9b39);
paths.Backend.Operations.Clear(); paths.Model.QueuePause(); paths.Model.Drain();
Yes(paths.Backend.Operations.Select(o => o.Name).SequenceEqual(new[] { "Pause", "Pause" }));
Yes(paths.Model.Channel0 == 301 && paths.Model.Channel1 == 402 && paths.Model.LastBase == 3 && paths.Model.LastAddition == 2 && paths.Backend.Activity(301) == 0);
Pass("replacement/start gain paths, captured pan, Pause versus Stop and shared logical state unchanged");
Console.WriteLine($"R13-C01 DEVELOPMENT CHECKS: {groups} passed; actual production model; no media, window or original runtime");
