using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Platform;

// ENGINEERING WPF effect voices, separate from song MediaPlayer/Stopwatch.
// MediaOpened is awaited before play; decoding is never initiated by judgement.
public sealed class WindowsCircleEffects : IDisposable
{
    private sealed class Voice(MediaPlayer player)
    {
        public MediaPlayer Player { get; } = player;
        public bool Busy { get; set; }
    }
    private readonly PreparedCircleEffects prepared;
    private readonly HitSoundSettings settings;
    private readonly Dictionary<int, Voice[]> voices = new();
    private readonly CircleSoundPlayback playback;
    private readonly string cache = Path.Combine(Path.GetTempPath(), "ohno-effects-" + Guid.NewGuid().ToString("N"));
    private CircleSession? session;
    private readonly WindowsSliderLoops loops = new();
    private SharedSliderChannels? continuing;
    private readonly Dispatcher dispatcher;
    private bool loopsPaused = true;
    private int loopGeneration;
    private string? failure;
    private readonly List<Voice> opening = new();
    private string? Failure => failure ?? prepared.Error ?? playback.Error;
    private bool disposed;
    public bool Enabled { get => playback.Enabled; set { if (!value) playback.Pause(); if (!value) PauseLoops(); playback.Enabled = value; } }
    public string Status => $"HitSounds {(Enabled ? "ON" : "OFF")} · {voices.Count}/{prepared.Samples.Count} resources decoded · {prepared.MissingLayers} unresolved circle layers · {prepared.Sliders.Count} discrete sliders · {loops.Ready} loop resources · " +
        (settings.DefaultPackDirectory is null ? "no independent default pack" : "independent default pack configured") +
        $" · effect {settings.VolumeEffect}% / global {settings.VolumeUniversal}% · layered {settings.LayeredHitSounds} (explicit policy)" +
        (prepared.ContinuousLimitation is { } limitation ? " · continuous unavailable: " + limitation : "") +
        (prepared.Sliders.Count > 0 && loops.Ready == 0 ? " · continuous unavailable: no decoded loop resources" : "") +
        (loops.Error is { } loopError ? " · " + loopError : "") +
        (Failure is { } error ? " · " + error : "") +
        (voices.Count == 0 ? " · no playable effects; supply local map/skin samples or a default pack" : "");
    private WindowsCircleEffects(PreparedCircleEffects prepared, HitSoundSettings settings, Dispatcher dispatcher)
    {
        this.prepared = prepared; this.settings = settings; this.dispatcher = dispatcher;
        playback = new(Play, action => dispatcher.BeginInvoke(action, DispatcherPriority.Normal), StopVoices) { Enabled = settings.Enabled };
    }
    public static async Task<WindowsCircleEffects> PrepareAsync(Beatmap map, HitSoundLibrary library,
        Dispatcher dispatcher, CancellationToken token, Action<WindowsCircleEffects>? acquired = null)
    {
        PreparedCircleEffects data = await library.PrepareAsync(map, token);
        var result = new WindowsCircleEffects(data, library.Settings, dispatcher);
        acquired?.Invoke(result);
        try
        {
            await Task.Run(() =>
            {
                Directory.CreateDirectory(result.cache);
                foreach (var pair in data.Samples)
                {
                    token.ThrowIfCancellationRequested();
                    string ext = pair.Value.Origin == SampleOrigin.Default ? ".wav" : Path.GetExtension(pair.Value.Name).ToLowerInvariant();
                    File.WriteAllBytes(Path.Combine(result.cache, pair.Key + ext), pair.Value.Bytes);
                }
            }, token);
            // Independent two-voice pool per resource, all opens asynchronous.
            // ENGINEERING cap comes from the64-resource catalog, not BASS max8.
            await Task.WhenAll(data.Samples.Where(p => !p.Value.Loop).Select(async pair =>
            {
                string ext = pair.Value.Origin == SampleOrigin.Default ? ".wav" : Path.GetExtension(pair.Value.Name).ToLowerInvariant();
                string file = Path.Combine(result.cache, pair.Key + ext);
                var pool = new List<Voice>();
                try
                {
                    for (int i = 0; i < 2; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        var voice = new Voice(new MediaPlayer()); pool.Add(voice); result.opening.Add(voice);
                        await result.OpenAsync(voice, file, token);
                    }
                    token.ThrowIfCancellationRequested();
                    result.voices.Add(pair.Key, pool.ToArray());
                    foreach (var v in pool) result.opening.Remove(v);
                }
                catch (OperationCanceledException) { foreach (var v in pool) Close(v); throw; }
                catch (Exception e)
                { foreach (var v in pool) Close(v); result.failure = "Decoder unavailable: " + pair.Value.Name + " — " + e.Message; }
            }));
            await result.loops.PrepareAsync(data.Samples, result.cache, token);
            if (data.ContinuousLimitation is null)
                result.continuing = new(new GlobalSampleTiming(data.Source.Timing), result.loops, data.Resources.ResolveContinuing)
                { SelectedFormat = data.Source.Format, SongFormat = data.Source.Format,
                  EffectVolume = library.Settings.VolumeEffect, UniversalVolume = library.Settings.VolumeUniversal };
            token.ThrowIfCancellationRequested();
            return result;
        }
        catch { result.Dispose(); throw; }
    }
    private async Task OpenAsync(Voice voice, string path, CancellationToken token)
    {
        var opened = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        EventHandler onOpen = (_, _) =>
        {
            try { if (voice.Player.HasAudio) opened.TrySetResult(true); else opened.TrySetException(new InvalidDataException("No audio stream.")); }
            catch (Exception e) { opened.TrySetException(e); }
        };
        EventHandler<ExceptionEventArgs> onFail = (_, args) => opened.TrySetException(args.ErrorException);
        voice.Player.MediaOpened += onOpen; voice.Player.MediaFailed += onFail;
        try
        {
            voice.Player.Open(new Uri(path)); // Never Play during preparation.
            await opened.Task.WaitAsync(TimeSpan.FromSeconds(15), token);
        }
        finally { voice.Player.MediaOpened -= onOpen; voice.Player.MediaFailed -= onFail; }
        voice.Player.MediaEnded += (_, _) => { voice.Busy = false; try { voice.Player.Stop(); } catch (Exception e) { failure = e.Message; } };
        voice.Player.MediaFailed += (_, args) => { voice.Busy = false; failure = "Effect playback failed: " + args.ErrorException.Message; };
    }
    public void Attach(CircleSession replacement)
    {
        if (session is not null) { session.PositiveCircleSound -= OnCircle; session.SliderSound -= OnSlider; DetachLoops(session); }
        if (continuing is not null) continuing.SameMapIdentity = ReferenceEquals(replacement.Map, prepared.PreparedMap);
        session = replacement; session.PositiveCircleSound += OnCircle; session.SliderSound += OnSlider;
        session.GlobalSoundTiming += OnGlobalTiming; session.ContinuingSound += OnContinuing; session.ContinuingManagerTail += OnManager;
    }
    private void DetachLoops(CircleSession old)
    { old.GlobalSoundTiming -= OnGlobalTiming; old.ContinuingSound -= OnContinuing; old.ContinuingManagerTail -= OnManager; }
    private void OnGlobalTiming(int time)
    { if (LoopsEligible) GuardLoops(() => continuing!.Clock(time)); }
    private bool LoopsEligible => !disposed && !loopsPaused && Enabled && continuing is { SameMapIdentity: true };
    private void GuardLoops(Action action)
    {
        try { action(); }
        catch (Exception e) { failure = "Continuous effect unavailable: " + e.Message; continuing?.Cancel(); loops.PauseAll(); }
    }
    private void FlushLoops()
    {
        int epoch = loopGeneration;
        dispatcher.BeginInvoke(new Action(() =>
        { if (epoch == loopGeneration && LoopsEligible) GuardLoops(() => continuing!.Drain()); }), DispatcherPriority.Normal);
    }
    private void OnContinuing(SliderState slider, int time, bool active)
    {
        if (!LoopsEligible) return;
        GuardLoops(() =>
        {
            if (!active) continuing!.QueuePause();
            else if (slider.Definition.Samples is { } data)
                continuing!.Submit(data.RawSound, data.BaseSet, data.AdditionSet, settings.LayeredHitSounds, slider.PresentationBallX);
            FlushLoops();
        });
    }
    private void OnManager(bool current)
    { if (LoopsEligible) GuardLoops(() => { continuing!.ManagerTail(current); FlushLoops(); }); }
    private void OnCircle(CircleDefinition circle)
    {
        if (disposed || prepared.Source.TimingError is not null) return;
        if (!prepared.Circles.TryGetValue(circle, out var sample)) { failure = "Unprepared circle identity."; return; }
        if (sample.Metadata.Error is { } error) { failure = error; return; }
        CircleSampleSelection.Request(sample.Metadata, prepared.Source.Timing, sample.Custom, prepared.Resources,
            new(true, settings.LayeredHitSounds, true, 0, 100, false, settings.VolumeEffect, settings.VolumeUniversal), playback);
    }
    private void Play(SamplePlaybackRequest request)
    {
        if (disposed || !voices.TryGetValue(request.Handle, out var pool)) return;
        Voice? voice = pool.FirstOrDefault(v => !v.Busy);
        if (voice is null) { failure = "Two effect voices busy; new voice dropped (engineering limit)."; return; }
        voice.Player.Stop(); voice.Player.Position = TimeSpan.Zero;
        voice.Player.Volume = Math.Clamp(request.Gain, 0, 1);
        voice.Player.Balance = Math.Clamp(request.Pan, -1, 1);
        voice.Player.SpeedRatio = request.Factor; // Ordinary no-mod practice factor1; not native frequency equivalence.
        voice.Busy = true; voice.Player.Play();
    }
    private void OnSlider(SliderSoundEvent sound)
    {
        if (disposed || prepared.Source.TimingError is not null) return;
        if (!prepared.Sliders.TryGetValue(sound.Parent, out var data)) { failure = "Unprepared slider sound identity."; return; }
        try
        {
            var sink = new SliderSink(playback, message => failure = message);
            SliderSampleSelection.Request(sound, data, prepared.Source.Timing, prepared.Resources,
                new(true, settings.LayeredHitSounds, true, 0, 100, false, settings.VolumeEffect, settings.VolumeUniversal), sink);
            if (sink.Requests == 0) failure = "Slider " + sound.Kind + " effect unavailable: missing resource or timing sample.";
        }
        catch (Exception e) { failure = "Slider effect unavailable: " + e.Message; }
    }
    private sealed class SliderSink(ISamplePlaybackSink next, Action<string> report) : ISamplePlaybackSink
    {
        public int Requests { get; private set; }
        public void Enqueue(SamplePlaybackRequest request)
        {
            Requests++;
            if (request.Handle == 0) report("Slider effect handle0 is invalid; no playback.");
            next.Enqueue(request);
        }
    }
    private void StopVoices()
    {
        foreach (var pool in voices.Values) foreach (var voice in pool)
        { try { voice.Player.Stop(); } catch (Exception e) { failure = e.Message; } voice.Busy = false; }
    }
    private void PauseLoops() { loopsPaused = true; loopGeneration++; continuing?.Cancel(); loops.PauseAll(); }
    public void Pause()
    { playback.Pause(); PauseLoops(); }
    public void Resume() { playback.Resume(); loopsPaused = false; }
    public void Restart(CircleSession replacement)
    { Pause(); continuing?.Timing.Reset(); playback.Restart(); Attach(replacement); Resume(); }
    private static void Close(Voice voice) { try { voice.Player.Close(); } catch (Exception e) { System.Diagnostics.Debug.WriteLine(e); } }
    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        if (session is not null) { session.PositiveCircleSound -= OnCircle; session.SliderSound -= OnSlider; DetachLoops(session); }
        loopGeneration++; continuing?.Cancel(); loops.Dispose();
        session = null; playback.Dispose();
        foreach (var pool in voices.Values) foreach (var voice in pool) Close(voice);
        voices.Clear();
        foreach (var voice in opening) Close(voice); opening.Clear();
        prepared.Dispose();
        try { if (Directory.Exists(cache)) Directory.Delete(cache, true); }
        catch (Exception e) { System.Diagnostics.Debug.WriteLine(e); }
    }
}
