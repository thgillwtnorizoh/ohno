using System.Diagnostics;

namespace Ohno.CircleGame.Platform;

// Independent silent-fixture clock. Does NOT reconstruct E10's BASS clock.
public sealed class GameplayClock
{
    private readonly Stopwatch stopwatch = new();
    public bool Running => stopwatch.IsRunning;
    public int Time => checked((int)Math.Round(stopwatch.Elapsed.TotalMilliseconds));
    public void Restart() => stopwatch.Restart();
    public void Pause() => stopwatch.Stop();
    public void Resume() => stopwatch.Start();
}
