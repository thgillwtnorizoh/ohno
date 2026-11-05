using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Gameplay;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Empty prepared-resource lifecycle only. No player, decoder, window or audio device opened.
        using var backend = new WindowsSliderLoops();
        backend.PrepareAsync(new Dictionary<int, SampleBytes>(), "", CancellationToken.None).GetAwaiter().GetResult();
        if (backend.Ready != 0 || backend.Acquire(-1, false) != 0 || backend.Acquire(0, false) != 0 || backend.Activity(1) != 0)
            throw new Exception("Empty backend admission failed.");
        backend.Attribute(0, 2, .5f); backend.Attribute(0, 3, .16f);
        backend.Play(0, true); backend.Pause(0); backend.Stop(0); backend.PauseAll();
        backend.Dispose(); backend.Dispose();
        if (backend.Acquire(1, false) != 0 || backend.Error is not null) throw new Exception("Disposed backend containment failed.");
        Console.WriteLine("R13 WINDOWS BACKEND: 1 empty-resource lifecycle group passed; no media opened or played");
    }
}
