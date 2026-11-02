using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Ohno.CircleGame.Platform;

/// <summary>UI-thread-owned Windows media, independent of the judgement clock.</summary>
public sealed class BeatmapAudio : IDisposable
{
    private readonly MediaPlayer? player;
    private readonly Uri? source;
    private readonly TaskCompletionSource<bool> opened = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool disposed;
    private bool prepared;
    public bool Silent => player is null;
    public bool Ended { get; private set; }
    public string? Failure { get; private set; }
    public event Action? StateChanged;

    public BeatmapAudio(string? path)
    {
        if (path is null) { prepared = true; return; }
        source = new Uri(Path.GetFullPath(path), UriKind.Absolute);
        if (!source.IsFile || source.IsUnc)
            throw new InvalidDataException("Only local audio files are supported.");
        string extension = Path.GetExtension(path).ToLowerInvariant();
        if (extension is not (".wav" or ".mp3" or ".wma" or ".m4a" or ".aac" or ".ogg" or ".flac" or ".opus"))
            throw new InvalidDataException("Choose a local audio file (WAV, MP3, WMA, M4A, AAC, OGG, FLAC or OPUS), not a playlist or media URL.");
        if (!File.Exists(path)) throw new FileNotFoundException("Beatmap audio was not found.", path);
        player = new MediaPlayer { Volume = 0.7 };
        player.MediaOpened += OnOpened;
        player.MediaFailed += OnFailed;
        player.MediaEnded += OnEnded;
    }

    // Called/awaited on the WPF dispatcher. Opening never starts playback.
    public async Task PrepareAsync(CancellationToken cancellationToken)
    {
        if (disposed) throw new ObjectDisposedException(nameof(BeatmapAudio));
        cancellationToken.ThrowIfCancellationRequested();
        if (Silent) return;
        try
        {
            player!.Open(source!);
            await opened.Task.WaitAsync(TimeSpan.FromSeconds(15), cancellationToken);
            RequireReady();
        }
        catch (TimeoutException)
        {
            Dispose();
            throw new InvalidDataException("Audio loading timed out after 15 seconds. The previous map is still available.");
        }
        catch { Dispose(); throw; }
    }

    public void RequireReady()
    {
        if (disposed || !prepared || Failure is not null)
            throw new InvalidOperationException(Failure ?? "Audio is not ready.");
    }

    public bool Restart()
    {
        if (Silent) return !disposed;
        bool ok = Control(() => { player!.Position = TimeSpan.Zero; player.Play(); });
        if (ok) { Ended = false; StateChanged?.Invoke(); }
        return ok;
    }

    public bool Resume()
    {
        if (disposed || Failure is not null) return false;
        // A short track stays ended on resume; only an explicit restart rewinds.
        return Silent || Ended || Control(() => player!.Play());
    }

    public void Pause()
    {
        if (!disposed && !Silent && Failure is null && !Ended)
            Control(() => player!.Pause());
    }

    private bool Control(Action action)
    {
        if (disposed || Failure is not null || !prepared) return false;
        try { action(); return Failure is null; }
        catch (Exception error) { Fail(error); return false; }
    }

    private void OnOpened(object? sender, EventArgs args)
    {
        if (disposed || Failure is not null) return;
        try
        {
            if (!player!.HasAudio) { Fail(new InvalidDataException("The selected media has no playable audio stream.")); return; }
            prepared = true;
            opened.TrySetResult(true);
        }
        catch (Exception error) { Fail(error); }
    }

    private void OnFailed(object? sender, ExceptionEventArgs args) => Fail(args.ErrorException);

    private void Fail(Exception error)
    {
        if (disposed || Failure is not null) return;
        Failure = "Audio could not be opened or played: " + error.Message;
        opened.TrySetException(new InvalidDataException(Failure, error));
        // No silent fallback on a declared-but-broken audio reference.
        CloseNativeMedia();
        StateChanged?.Invoke();
    }

    private void OnEnded(object? sender, EventArgs args)
    {
        if (disposed || Failure is not null) return;
        Ended = true;
        StateChanged?.Invoke();
    }

    private void CloseNativeMedia()
    {
        if (player is null) return;
        // Best-effort shutdown: a decoder error must not tear down the UI.
        try { player.Stop(); }
        catch (Exception error) { System.Diagnostics.Debug.WriteLine(error); }
        try { player.Close(); }
        catch (Exception error) { System.Diagnostics.Debug.WriteLine(error); }
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        opened.TrySetCanceled();
        if (player is null) return;
        player.MediaOpened -= OnOpened;
        player.MediaFailed -= OnFailed;
        player.MediaEnded -= OnEnded;
        CloseNativeMedia();
        StateChanged = null;
    }
}
