using System.Diagnostics;

namespace Ohno.CircleGame.Platform;

// Practice clock authority for both silent and audible maps, not E10's BASS clock.
public sealed class GameplayClock
{
    private readonly Stopwatch stopwatch = new();
    public bool Running => stopwatch.IsRunning;
    public int Time => checked((int)Math.Round(stopwatch.Elapsed.TotalMilliseconds));
    public void Restart() => stopwatch.Restart();
    public void Pause() => stopwatch.Stop();
    public void Resume() => stopwatch.Start();
}
