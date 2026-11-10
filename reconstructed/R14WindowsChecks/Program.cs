using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        using var backend = new WindowsSliderLoops();
        backend.PrepareAsync(new Dictionary<int,SampleBytes>(), "", CancellationToken.None).GetAwaiter().GetResult();
        if (backend.FrequencyCapability != FrequencyCapability.Unsupported || backend.Length(0,0) is not null)
            throw new Exception("Frequency/native length capability must be explicitly unsupported.");
        backend.Attribute(0,1,60000); backend.Play(0,false); backend.Pause(0); backend.Stop(0);
        if (backend.Ready != 0 || backend.Activity(0) != 0 || backend.Acquire(-1,false) != 0 || backend.Error is not null)
            throw new Exception("Empty spinner backend lifecycle failed.");
        backend.Dispose(); backend.Dispose();
        Console.WriteLine("R14 WINDOWS EMPTY: 1 capability/lifecycle group passed; no media constructed/opened/played");
    }
}
