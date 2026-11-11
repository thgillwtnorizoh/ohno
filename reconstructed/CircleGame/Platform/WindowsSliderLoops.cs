using System.IO;
using System.Windows;
using System.Windows.Media;
using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Platform;

// ENGINEERING persistent MediaPlayers, distinct from the one-shot voice pool.
// MediaEnded restarts the prepared stream: no guaranteed gapless/device/native parity.
public sealed class WindowsSliderLoops : ISpinnerSampleChannels
{
    private sealed class Channel(MediaPlayer player)
    {
        public MediaPlayer Player { get; } = player;
        public int State { get; set; }
        public bool DecoderReady { get; set; }
    }
    private readonly Dictionary<int, Channel> channels = new();
    private bool disposed;
    public string? Error { get; private set; }
    public EffectAudioDiagnostics? Diagnostics { get; set; }
    public bool IsReady(int handle) => channels.TryGetValue(handle, out var c) && c.DecoderReady;
    public FrequencyCapability FrequencyCapability => FrequencyCapability.Unsupported;
    public string FrequencyStatus => "Unsupported frequency modulation: MediaPlayer has no verified native length/attribute1 mapping.";
    public long? Length(int channel, int mode) => null; // Never substitute encoded WAV byte length.
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
                Diagnostics?.Count("loop.decoder.open", pair.Key + ":" + pair.Value.Name);
                channel.Player.Open(new Uri(Path.Combine(cache, pair.Key + ext)));
                await ready.Task.WaitAsync(TimeSpan.FromSeconds(15), token);
                channel.DecoderReady = true; Diagnostics?.Count("loop.decoder.ready", pair.Key);
                channel.Player.MediaEnded += (_, _) => Safe(() =>
                { if (channel.State == 1) { Diagnostics?.Count("loop.rewind", pair.Key); channel.Player.Position = TimeSpan.Zero; channel.Player.Play(); } });
                channel.Player.MediaFailed += (_, e) => { channel.State = 0; channel.DecoderReady = false; Error = e.ErrorException.Message; Diagnostics?.Count("loop.media.failed", pair.Key + ":" + Error); };
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception e) { Error = "Loop decoder unavailable: " + pair.Value.Name + " — " + e.Message; Diagnostics?.Count("loop.decoder.failed", Error); channel.Player.Close(); channels.Remove(pair.Key); }
            finally { channel.Player.MediaOpened -= opened; channel.Player.MediaFailed -= failed; }
        }));
    }
    private void Safe(Action action) { try { if (!disposed) action(); } catch (Exception e) { Error = e.Message; Diagnostics?.Count("loop.exception", Error); } }
    // Invalid/missing sample IDs become managed unavailable0; these are not native return values.
    public int Acquire(int sample, bool onlyNew)
    {
        int channel = !disposed && channels.ContainsKey(sample) ? sample : 0;
        Diagnostics?.Count("loop.acquire", $"sample={sample}; channel={channel}; ready={IsReady(sample)}; onlyNew={onlyNew}");
        return channel;
    }
    public int Activity(int channel) => channels.TryGetValue(channel, out var c) ? c.State : 0;
    public void Attribute(int channel, int attribute, float value) => Safe(() =>
    {
        if (!channels.TryGetValue(channel, out var c)) { Diagnostics?.Count("loop.attribute.missing", channel); return; }
        if (attribute == 2) Diagnostics?.Set("loop.gain." + channel, value);
        if (attribute == 2) c.Player.Volume = Math.Clamp(value, 0f, 1f);
        else if (attribute == 3) c.Player.Balance = Math.Clamp(value, -1f, 1f);
        // attr11=1 retained by model; no unsupported rate/pitch mapping invented.
    });
    public void Play(int channel, bool restart) => Safe(() =>
    { Diagnostics?.Count("loop.start.request", channel); if (channels.TryGetValue(channel, out var c)) { if (restart) c.Player.Position = TimeSpan.Zero; c.State = 1; c.Player.Play(); Diagnostics?.Transition(channel, "Play(false/true=" + restart + ")"); Diagnostics?.Count("loop.Play." + channel, $"restart={restart}; ready={c.DecoderReady}"); } else Diagnostics?.Count("loop.start.missing", channel); });
    public void Pause(int channel) => Safe(() =>
    { Diagnostics?.Count("loop.pause.request", channel); if (channels.TryGetValue(channel, out var c)) { c.State = 3; c.Player.Pause(); Diagnostics?.Transition(channel, "Pause"); Diagnostics?.Count("loop.Pause." + channel); } });
    public void Stop(int channel) => Safe(() =>
    { Diagnostics?.Count("loop.stop.request", channel); if (channels.TryGetValue(channel, out var c)) { c.State = 0; c.Player.Stop(); Diagnostics?.Transition(channel, "Stop"); Diagnostics?.Count("loop.Stop." + channel); } });
    public void PauseAll() { foreach (int id in channels.Keys) Pause(id); }
    public void Dispose()
    {
        if (disposed) return;
        foreach (var c in channels.Values) Safe(() => { c.State = 0; c.Player.Stop(); c.Player.Close(); });
        disposed = true; channels.Clear();
    }
}
