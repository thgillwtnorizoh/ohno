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
    private Dictionary<SliderDefinition, StreamGeometry> paths = new();
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
        var preparedPaths = map.Sliders.ToDictionary(s => s, s => BuildPath(s.Geometry));
        clock.Pause();
        BeatmapAudio? previousAudio = audio;
        if (previousAudio is not null) previousAudio.StateChanged -= AudioChanged;
        audio = preparedAudio;
        audio.StateChanged += AudioChanged;
        Session = replacement;
        paths = preparedPaths;
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

    public void SetHeldButtons(int buttons) => Session?.SetHeldButtons(buttons);

    public void Press(int side = 1)
    {
        if (!Running || Session is null || ActualWidth <= 0 || ActualHeight <= 0) return;
        Point mouse = Mouse.GetPosition(this); // Actual OS/WPF mouse, no synthetic input.
        PlayfieldLayout layout = PlayfieldLayout.Fit(ActualWidth, ActualHeight);
        if (mouse.X < layout.StageX || mouse.Y < layout.StageY ||
            mouse.X > layout.StageX + layout.Width || mouse.Y > layout.StageY + layout.Height) return;
        // E11 uses stored gameplay time at consumption, not a key-event timestamp.
        Session.Press(layout.ToMap(new Vector2((float)mouse.X, (float)mouse.Y)), CurrentTime, side);
        InvalidateVisual();
        FrameUpdated?.Invoke();
    }

    private void Frame(object? sender, EventArgs args)
    {
        if (disposed || !Running || Session is null) return;
        CurrentTime = clock.Time;
        // Real held-key and mouse samples; no synthetic gameplay events.
        int buttons = (Keyboard.IsKeyDown(Key.Z) ? 1 : 0) | (Keyboard.IsKeyDown(Key.X) ? 2 : 0);
        Point mouse = Mouse.GetPosition(this);
        PlayfieldLayout layout = PlayfieldLayout.Fit(ActualWidth, ActualHeight);
        bool inside = mouse.X >= layout.StageX && mouse.Y >= layout.StageY &&
            mouse.X <= layout.StageX + layout.Width && mouse.Y <= layout.StageY + layout.Height;
        Vector2 pointer = inside ? layout.ToMap(new((float)mouse.X, (float)mouse.Y)) : new(float.NaN, float.NaN);
        Session.Advance(CurrentTime, pointer, buttons);
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
        if (Session is not { } game) { Message(dc, "Load a supported .osu map", "Open .osu above to begin"); return; }
        double radius = game.DiameterAtScaleOne * l.Scale / 2;
        foreach (SliderState slider in game.Sliders)
        {
            SliderDefinition definition = slider.Definition;
            if (slider.Result != Judgement.Pending || CurrentTime < definition.Head.StartTime - game.Approach) continue;
            double alpha = Math.Clamp((CurrentTime - (definition.Head.StartTime - game.Approach)) / 400.0, 0, 1);
            dc.PushOpacity(alpha);
            // Immutable map-space path built once on Load; only layout changes per frame.
            dc.PushTransform(new MatrixTransform(l.Scale, 0, 0, l.Scale, origin.X, origin.Y));
            var outline = new Pen(Accent, (radius * 2 + 4) / l.Scale)
                { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round, LineJoin = PenLineJoin.Round };
            var body = new Pen(Brush("#253D58"), radius * 2 / l.Scale)
                { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round, LineJoin = PenLineJoin.Round };
            dc.DrawGeometry(null, outline, paths[definition]);
            dc.DrawGeometry(null, body, paths[definition]);
            dc.Pop();
            int traversal = definition.TraversalIndex(CurrentTime);
            SliderMovement movement = definition.TraversalEndMovement(traversal);
            Vector2 endpoint = l.ToScreen(movement.To);
            dc.DrawEllipse(null, new Pen(Accent, 2), new(endpoint.X, endpoint.Y), radius * 0.45, radius * 0.45);
            // Producer metadata drives independent markers, never judgement codes.
            foreach (var checkpoint in slider.Checkpoints)
            {
                if (checkpoint.Definition.Kind != SliderCheckpointKind.Interior || checkpoint.Definition.Traversal != traversal) continue;
                Vector2 tick = l.ToScreen(checkpoint.Definition.Position);
                Brush color = checkpoint.Succeeded switch { true => Accent, false => Brush("#FF8498"), _ => Brush("#FFDB83") };
                double tickRadius = Math.Max(3, 4 * l.Scale);
                dc.DrawEllipse(color, new Pen(Brushes.White, 1), new(tick.X, tick.Y), tickRadius, tickRadius);
            }
            if (traversal < definition.Traversals - 1)
            {
                Vector2 difference = movement.From - movement.To;
                // A researched zero final direction may remain. The TURN label still
                // identifies the endpoint; omit its chevron rather than normalize zero.
                Vector2 direction = difference == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(difference);
                Vector2 normal = new(-direction.Y, direction.X);
                float size = (float)Math.Max(8, radius * 0.45);
                Vector2 tip = endpoint + direction * size * 0.5f;
                Vector2 a = endpoint - direction * size * 0.5f + normal * size;
                Vector2 b = endpoint - direction * size * 0.5f - normal * size;
                var arrow = new Pen(Brush("#FFDB83"), 3);
                dc.DrawLine(arrow, new(a.X, a.Y), new(tip.X, tip.Y));
                dc.DrawLine(arrow, new(b.X, b.Y), new(tip.X, tip.Y));
                Text(dc, "TURN", new(endpoint.X, endpoint.Y - radius - 16), 13, Brushes.White, true);
            }
            if (CurrentTime >= definition.Head.StartTime)
            {
                Vector2 ball = l.ToScreen(definition.BallPosition(CurrentTime));
                Brush followColor = slider.FollowEligible ? Accent : Brush("#FFB37F");
                double followRadius = game.HitRadius * l.Scale * (slider.Tracking ? 2.4000000953674316f : 1f);
                dc.DrawEllipse(null, new Pen(followColor, 2), new(ball.X, ball.Y), followRadius, followRadius);
                dc.DrawEllipse(followColor, new Pen(Brushes.White, 2), new(ball.X, ball.Y), radius * 0.65, radius * 0.65);
                Text(dc, $"SPAN {traversal + 1}/{definition.Traversals} · " +
                    (slider.ProcessedCheckpoints < slider.Checkpoints.Count ? "HOLD Z / X" : "TAIL CHECKED"),
                    new(ball.X, ball.Y + followRadius + 18), 14, Brushes.White, true);
                Text(dc, $"CHECKPOINTS {slider.ProcessedCheckpoints}/{slider.Checkpoints.Count} · HIT {slider.SuccessfulCheckpoints}",
                    new Point(ActualWidth / 2, ActualHeight - 22), 14, Accent, true);
            }
            dc.Pop();
        }
        for (int i = game.Heads.Count - 1; i >= 0; i--)
        {
            CircleState circle = game.Heads[i];
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
        CircleState? next = game.Heads.FirstOrDefault(c => c.Result == Judgement.Pending);
        if (next is not null && started && !game.Finished)
            Text(dc, $"NEXT  {Math.Max(0, next.Definition.StartTime - CurrentTime)} ms", new Point(ActualWidth / 2, 24), 15, Accent, true);

        if (game.Finished)
        {
            int n300 = game.ObjectResults.Count(r => r == Judgement.Hit300);
            int n100 = game.ObjectResults.Count(r => r == Judgement.Hit100);
            int n50 = game.ObjectResults.Count(r => r == Judgement.Hit50);
            int misses = game.ObjectResults.Count(r => r == Judgement.Miss);
            Message(dc, "Practice complete", $"300: {n300}   100: {n100}   50: {n50}   Miss: {misses}\n" +
                $"Accuracy: {game.Accuracy:F2}%    Max combo: {game.MaxCombo}\nR / Enter to play again");
        }
        else if (!started) Message(dc, "Circle / slider practice", "Aim and press Z / X; hold and follow slider balls.\nSpace or Start to begin · " + AudioStatus);
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

    private static StreamGeometry BuildPath(SliderGeometry source)
    {
        var geometry = new StreamGeometry();
        using (StreamGeometryContext context = geometry.Open())
        {
            Vector2? previous = null;
            foreach (var s in source.Segments)
            {
                if (previous is null || previous.Value != s.From) context.BeginFigure(new(s.From.X, s.From.Y), false, false);
                context.LineTo(new(s.To.X, s.To.Y), true, false);
                previous = s.To;
            }
        }
        geometry.Freeze();
        return geometry;
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
        paths.Clear();
    }
}
