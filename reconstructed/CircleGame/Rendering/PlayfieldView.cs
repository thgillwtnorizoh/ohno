using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;

namespace Ohno.CircleGame.Rendering;

public sealed class PlayfieldView : FrameworkElement, IDisposable
{
    private readonly GameplayClock clock = new();
    private BeatmapAudio? audio;
    private static readonly Brush Back = Brush("#0C1324"), Stage = Brush("#101D32"), Accent = Brush("#58E0D3");
    private static readonly Typeface Font = new("Segoe UI");
    private bool started;
    private bool disposed;
    public CircleSession? Session { get; private set; }
    public int CurrentTime { get; private set; }
    public bool Running => clock.Running;
    public string AudioStatus => audio?.Failure is { } failure ? failure + " Reload the map or choose silent practice." :
        audio is null || audio.Silent ? "Silent practice" : audio.Ended ? "Audio ended · timeline continues" : "Local beatmap audio";
    public event Action? FrameUpdated;

    public PlayfieldView()
    {
        Focusable = true;
        Cursor = Cursors.Cross;
        ClipToBounds = true;
        CompositionTarget.Rendering += Frame;
        MouseDown += (_, _) => Focus(); // Mouse positioning only; clicks never judge.
    }

    // Ownership transfers only after the replacement is fully prepared.
    public void Load(Beatmap map, BeatmapAudio preparedAudio)
    {
        preparedAudio.RequireReady();
        var replacement = new CircleSession(map);
        clock.Pause();
        BeatmapAudio? previousAudio = audio;
        if (previousAudio is not null) previousAudio.StateChanged -= AudioChanged;
        audio = preparedAudio;
        audio.StateChanged += AudioChanged;
        Session = replacement;
        CurrentTime = 0;
        started = false;
        previousAudio?.Dispose();
        InvalidateVisual();
        FrameUpdated?.Invoke();
    }

    public void Restart()
    {
        if (Session is null) return;
        Pause();
        var replacement = new CircleSession(Session.Map);
        if (audio?.Restart() == false) { AudioChanged(); return; }
        Session = replacement;
        started = true;
        CurrentTime = 0;
        clock.Restart();
        Focus();
        InvalidateVisual();
    }

    public void TogglePause()
    {
        if (Session is null) return;
        if (!started || Session.Finished) { Restart(); return; }
        if (Running) Pause();
        else if (audio?.Resume() != false) clock.Resume();
        Focus();
        InvalidateVisual();
    }

    public void Pause()
    {
        clock.Pause();
        audio?.Pause();
        InvalidateVisual();
    }

    private void AudioChanged()
    {
        if (audio?.Failure is not null) clock.Pause();
        InvalidateVisual();
        FrameUpdated?.Invoke();
    }

    public void Press()
    {
        if (!Running || Session is null || ActualWidth <= 0 || ActualHeight <= 0) return;
        Point mouse = Mouse.GetPosition(this); // Actual OS/WPF mouse, no synthetic input.
        PlayfieldLayout layout = PlayfieldLayout.Fit(ActualWidth, ActualHeight);
        if (mouse.X < layout.StageX || mouse.Y < layout.StageY ||
            mouse.X > layout.StageX + layout.Width || mouse.Y > layout.StageY + layout.Height) return;
        // E11 uses stored gameplay time at consumption, not a key-event timestamp.
        Session.Press(layout.ToMap(new Vector2((float)mouse.X, (float)mouse.Y)), CurrentTime);
        InvalidateVisual();
        FrameUpdated?.Invoke();
    }

    private void Frame(object? sender, EventArgs args)
    {
        if (disposed || !Running || Session is null) return;
        CurrentTime = clock.Time;
        Session.Advance(CurrentTime);
        if (Session.Finished) Pause();
        InvalidateVisual();
        FrameUpdated?.Invoke();
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);
        if (ActualWidth <= 0 || ActualHeight <= 0) return;
        dc.DrawRectangle(Back, null, new Rect(0, 0, ActualWidth, ActualHeight));
        PlayfieldLayout l = PlayfieldLayout.Fit(ActualWidth, ActualHeight);
        dc.DrawRoundedRectangle(Stage, null, new Rect(l.StageX, l.StageY, l.Width, l.Height), 12, 12);
        Vector2 origin = l.ToScreen(Vector2.Zero);
        dc.DrawRectangle(null, new Pen(Brush("#293D56"), 1), new Rect(origin.X, origin.Y, 512 * l.Scale, 384 * l.Scale));
        if (Session is not { } game) { Message(dc, "Load an ordinary-circle .osu map", "Open .osu above to begin"); return; }
        double radius = game.DiameterAtScaleOne * l.Scale / 2;
        for (int i = game.Circles.Count - 1; i >= 0; i--)
        {
            CircleState circle = game.Circles[i];
            int start = circle.Definition.StartTime;
            if (circle.Result != Judgement.Pending || CurrentTime < start - game.Approach || CurrentTime > start + game.Window50) continue;
            Vector2 p = l.ToScreen(circle.Definition.Position);
            // E08 non-Hidden projection: 400ms face fade and approach scale 4 -> 1.
            double alpha = Math.Clamp((CurrentTime - (start - game.Approach)) / 400.0, 0, 1);
            if (CurrentTime > start + game.Window100)
                alpha = Math.Clamp(1.0 - (CurrentTime - start - game.Window100) / (double)(game.Window50 - game.Window100), 0, 1);
            dc.PushOpacity(alpha);
            dc.DrawEllipse(Brush("#253D58"), new Pen(Accent, Math.Max(1, 2 * l.Scale)), new Point(p.X, p.Y), radius, radius);
            Text(dc, (i + 1).ToString(), new Point(p.X, p.Y), Math.Max(12, radius * 0.8), Brushes.White, true);
            dc.Pop();
            if (CurrentTime <= start)
            {
                double remaining = Math.Clamp((start - CurrentTime) / (double)game.Approach, 0, 1);
                double approachRadius = radius * (1 + 3 * remaining);
                double approachAlpha = 0.9 * Math.Clamp((CurrentTime - (start - game.Approach)) /
                    (double)Math.Min(game.Approach, 800), 0, 1);
                dc.PushOpacity(approachAlpha);
                dc.DrawEllipse(null, new Pen(Accent, Math.Max(1, 2 * l.Scale)), new Point(p.X, p.Y), approachRadius, approachRadius);
                dc.Pop();
            }
        }
        if (game.Feedback is { } feedback && CurrentTime - feedback.Time < 650)
        {
            Vector2 p = l.ToScreen(feedback.Position);
            Brush color = feedback.Result == Judgement.Miss ? Brush("#FF8498") : Brushes.White;
            Text(dc, feedback.Text, new Point(p.X, p.Y - radius - 24), 22, color, true);
        }
        // Visible timing cue remains available with or without audio.
        CircleState? next = game.Circles.FirstOrDefault(c => c.Result == Judgement.Pending);
        if (next is not null && started && !game.Finished)
            Text(dc, $"NEXT  {Math.Max(0, next.Definition.StartTime - CurrentTime)} ms", new Point(ActualWidth / 2, 24), 15, Accent, true);

        if (game.Finished)
        {
            int n300 = game.Circles.Count(c => c.Result == Judgement.Hit300);
            int n100 = game.Circles.Count(c => c.Result == Judgement.Hit100);
            int n50 = game.Circles.Count(c => c.Result == Judgement.Hit50);
            int misses = game.Circles.Count(c => c.Result == Judgement.Miss);
            Message(dc, "Practice complete", $"300: {n300}   100: {n100}   50: {n50}   Miss: {misses}\n" +
                $"Accuracy: {game.Accuracy:F2}%    Max combo: {game.MaxCombo}\nR / Enter to play again");
        }
        else if (!started) Message(dc, "Circle practice", "Move the mouse. Press Z or X when the rings meet.\nSpace or Start to begin · " + AudioStatus);
        else if (!Running) Message(dc, audio?.Failure is null ? "Paused" : "Audio unavailable",
            audio?.Failure is null ? "Space to resume · R to restart" : "Reload the map or choose Silent fixture above.");
    }

    private void Message(DrawingContext dc, string title, string subtitle)
    {
        double w = Math.Min(560, ActualWidth - 32), h = 210;
        dc.DrawRoundedRectangle(Brush("#EE0B1324"), new Pen(Brush("#425B75"), 1),
            new Rect((ActualWidth - w) / 2, (ActualHeight - h) / 2, w, h), 16, 16);
        Text(dc, title, new Point(ActualWidth / 2, ActualHeight / 2 - 48), 30, Brushes.White, true);
        Text(dc, subtitle, new Point(ActualWidth / 2, ActualHeight / 2 + 20), 16, Brush("#C2D5E5"), true);
    }

    private void Text(DrawingContext dc, string text, Point position, double size, Brush brush, bool centered)
    {
        var formatted = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
            Font, size, brush, VisualTreeHelper.GetDpi(this).PixelsPerDip) { TextAlignment = centered ? TextAlignment.Center : TextAlignment.Left };
        dc.DrawText(formatted, new Point(position.X, position.Y - formatted.Height / 2));
    }

    private static Brush Brush(string color)
    {
        var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(color)!;
        brush.Freeze();
        return brush;
    }

    public void Dispose()
    {
        disposed = true;
        clock.Pause();
        if (audio is not null) { audio.StateChanged -= AudioChanged; audio.Dispose(); audio = null; }
        CompositionTarget.Rendering -= Frame;
    }
}
