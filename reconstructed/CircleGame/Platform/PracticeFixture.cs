using System.IO;
using System.Text;

namespace Ohno.CircleGame.Platform;

/// <summary>Reproducible original sine-tone practice audio; no third-party recording.</summary>
public sealed class PracticeFixture : IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "ohno-r03-" + Guid.NewGuid().ToString("N"));
    public string MapPath => Path.Combine(directory, "audible-circles.osu");

    public PracticeFixture()
    {
        Directory.CreateDirectory(directory);
        try
        {
            File.Copy(Path.Combine(AppContext.BaseDirectory, "Fixtures", "audible-circles.osu"), MapPath);
            GenerateWave(Path.Combine(directory, "practice-beat.wav"));
        }
        catch { Dispose(); throw; }
    }

    private static void GenerateWave(string path)
    {
        const int sampleRate = 22050, seconds = 18, frames = sampleRate * seconds;
        using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
        using var writer = new BinaryWriter(stream, Encoding.ASCII);
        writer.Write(Encoding.ASCII.GetBytes("RIFF")); writer.Write(36 + frames * 2);
        writer.Write(Encoding.ASCII.GetBytes("WAVEfmt ")); writer.Write(16);
        writer.Write((short)1); writer.Write((short)1); // PCM, mono
        writer.Write(sampleRate); writer.Write(sampleRate * 2);
        writer.Write((short)2); writer.Write((short)16);
        writer.Write(Encoding.ASCII.GetBytes("data")); writer.Write(frames * 2);
        for (int i = 0; i < frames; i++)
        {
            double time = i / (double)sampleRate;
            // Quiet half-second guide ticks; higher tones at the eight circle times.
            double tickStart = Math.Floor(time * 2) / 2;
            double elapsed = time - tickStart;
            bool target = tickStart >= 3 && tickStart <= 13.5 && ((tickStart - 3) % 1.5) == 0;
            double frequency = target ? 880 : 440;
            double envelope = elapsed < 0.09 ? Math.Sin(Math.PI * elapsed / 0.09) * Math.Exp(-elapsed * 30) : 0;
            double sample = Math.Sin(2 * Math.PI * frequency * elapsed) * envelope * (target ? 0.3 : 0.12);
            writer.Write((short)Math.Round(sample * short.MaxValue));
        }
    }

    public void Dispose()
    {
        // Media is closed before this cleanup. Some Windows decoders may release
        // their file handles later; failure leaves only this private temp folder.
        try { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
        catch (IOException error) { System.Diagnostics.Debug.WriteLine(error); }
        catch (UnauthorizedAccessException error) { System.Diagnostics.Debug.WriteLine(error); }
    }
}
