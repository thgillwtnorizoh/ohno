using System.IO;
using System.Windows;
using System.Windows.Media;
using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Platform;

// ENGINEERING persistent MediaPlayers, distinct from the one-shot voice pool.
// MediaEnded restarts the prepared stream: no guaranteed gapless/device/native parity.
public sealed class WindowsSliderLoops : IPersistentSampleChannels
{
    private sealed class Channel(MediaPlayer player)
    {
        public MediaPlayer Player { get; } = player;
        public int State { get; set; }
    }
    private readonly Dictionary<int, Channel> channels = new();
    private bool disposed;
    public string? Error { get; private set; }
    public int Ready => channels.Count;
    public async Task PrepareAsync(IReadOnlyDictionary<int, SampleBytes> samples, string cache, CancellationToken token)
    {
        await Task.WhenAll(samples.Where(p => p.Value.Loop).Select(async pair =>
        {
            string ext = pair.Value.Origin == SampleOrigin.Default ? ".wav" : Path.GetExtension(pair.Value.Name).ToLowerInvariant();
            var channel = new Channel(new MediaPlayer());
            channels.Add(pair.Key, channel);
            var ready = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            EventHandler opened = (_, _) => { if (channel.Player.HasAudio) ready.TrySetResult(true); else ready.TrySetException(new InvalidDataException("Loop has no audio stream.")); };
            EventHandler<ExceptionEventArgs> failed = (_, e) => ready.TrySetException(e.ErrorException);
            channel.Player.MediaOpened += opened; channel.Player.MediaFailed += failed;
            try
            {
                channel.Player.Open(new Uri(Path.Combine(cache, pair.Key + ext)));
                await ready.Task.WaitAsync(TimeSpan.FromSeconds(15), token);
                channel.Player.MediaEnded += (_, _) => Safe(() =>
                { if (channel.State == 1) { channel.Player.Position = TimeSpan.Zero; channel.Player.Play(); } });
                channel.Player.MediaFailed += (_, e) => { channel.State = 0; Error = e.ErrorException.Message; };
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception e) { Error = "Loop decoder unavailable: " + pair.Value.Name + " — " + e.Message; channel.Player.Close(); channels.Remove(pair.Key); }
            finally { channel.Player.MediaOpened -= opened; channel.Player.MediaFailed -= failed; }
        }));
    }
    private void Safe(Action action) { try { if (!disposed) action(); } catch (Exception e) { Error = e.Message; } }
    // Invalid/missing sample IDs become managed unavailable0; these are not native return values.
    public int Acquire(int sample, bool onlyNew) => !disposed && channels.ContainsKey(sample) ? sample : 0;
    public int Activity(int channel) => channels.TryGetValue(channel, out var c) ? c.State : 0;
    public void Attribute(int channel, int attribute, float value) => Safe(() =>
    {
        if (!channels.TryGetValue(channel, out var c)) return;
        if (attribute == 2) c.Player.Volume = Math.Clamp(value, 0f, 1f);
        else if (attribute == 3) c.Player.Balance = Math.Clamp(value, -1f, 1f);
        // attr11=1 retained by model; no unsupported rate/pitch mapping invented.
    });
    public void Play(int channel, bool restart) => Safe(() =>
    { if (channels.TryGetValue(channel, out var c)) { if (restart) c.Player.Position = TimeSpan.Zero; c.State = 1; c.Player.Play(); } });
    public void Pause(int channel) => Safe(() =>
    { if (channels.TryGetValue(channel, out var c)) { c.State = 3; c.Player.Pause(); } });
    public void Stop(int channel) => Safe(() =>
    { if (channels.TryGetValue(channel, out var c)) { c.State = 0; c.Player.Stop(); } });
    public void PauseAll() { foreach (int id in channels.Keys) Pause(id); }
    public void Dispose()
    {
        if (disposed) return;
        foreach (var c in channels.Values) Safe(() => { c.State = 0; c.Player.Stop(); c.Player.Close(); });
        disposed = true; channels.Clear();
    }
}
