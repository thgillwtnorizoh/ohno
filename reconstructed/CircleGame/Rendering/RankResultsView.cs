using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Ohno.CircleGame.Gameplay;

namespace Ohno.CircleGame.Rendering;

// ENGINEERING WPF layout/typography. No original texture extraction, skin or pixel-fidelity claim.
public sealed class RankResultsView : Grid
{
    private readonly TextBlock score = Text("", 42, Brushes.White);
    private RankPresentation? bound;
    public event Action? BackRequested;
    public event Action? RetryRequested;
    public event Action? SkipRequested;
    public RankResultsView() { Background = Brush("#0C1324"); Visibility = Visibility.Collapsed; }
    public void Bind(RankPresentation? presentation, string? unavailable, string revealedScore)
    {
        Visibility = Visibility.Visible;
        if (ReferenceEquals(bound, presentation) && Children.Count > 0) { score.Text = revealedScore; return; }
        bound = presentation; if (score.Parent is Panel old) old.Children.Remove(score); Children.Clear();
        var card = new Border { Background = Brush("#15233B"), BorderBrush = Brush("#35506A"), BorderThickness = new(1),
            CornerRadius = new(18), Padding = new(28), Margin = new(24), MaxWidth = 860, VerticalAlignment = VerticalAlignment.Center };
        var stack = new StackPanel(); card.Child = stack; Children.Add(new ScrollViewer { Content = card, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled });
        stack.Children.Add(Text(presentation is null ? "RESULTS UNAVAILABLE" : "RANK  /  SESSION RESULTS", 17, Brush("#58E0D3")));
        if (presentation is null)
            stack.Children.Add(Text(unavailable ?? "Results preparation unsupported.", 19, Brushes.White));
        else
        {
            stack.Children.Add(Text(presentation.Map.Title, 24, Brushes.White));
            stack.Children.Add(Text(presentation.Map.Difficulty ?? "Difficulty metadata unavailable", 14, Brush("#B3C8DE")));
            if (!string.IsNullOrWhiteSpace(presentation.Map.Artist)) stack.Children.Add(Text(presentation.Map.Artist, 13, Brush("#B3C8DE")));
            var summary = new Grid { Margin = new(0, 14, 0, 14) };
            summary.ColumnDefinitions.Add(new() { Width = new(160) }); summary.ColumnDefinitions.Add(new());
            summary.Children.Add(Text(presentation.Grade.First.ToString(), 88, Brush("#58E0D3")));
            var numbers = new StackPanel { VerticalAlignment = VerticalAlignment.Center }; Grid.SetColumn(numbers, 1);
            numbers.Children.Add(Text("SCORE", 12, Brush("#B3C8DE"))); score.Text = revealedScore; numbers.Children.Add(score);
            numbers.Children.Add(Text("Accuracy  " + presentation.Accuracy, 23, Brushes.White)); summary.Children.Add(numbers); stack.Children.Add(summary);
            var counters = new UniformGrid { Columns = 3 };
            foreach (var (label, value) in new[] { ("300", presentation.N300.ToString(CultureInfo.InvariantCulture)),
                ("100", presentation.N100.ToString(CultureInfo.InvariantCulture)), ("50", presentation.N50.ToString(CultureInfo.InvariantCulture)),
                ("MISS", presentation.NMiss.ToString(CultureInfo.InvariantCulture)), ("MAX COMBO", presentation.MaximumCombo.ToString(CultureInfo.InvariantCulture) + "x"),
                ("RECORD", presentation.Valid ? "VALID" : "INVALID") })
            {
                var cell = new StackPanel { Margin = new(8) }; cell.Children.Add(Text(label, 12, Brush("#B3C8DE")));
                cell.Children.Add(Text(value, 24, Brushes.White)); counters.Children.Add(cell);
            }
            stack.Children.Add(counters);
            stack.Children.Add(Text(presentation.Perfect switch { true => "PERFECT · prepared combo condition satisfied", false => "Prepared perfect condition not satisfied", null => "Perfect decoration unavailable · preparation provenance unsupported" }, 13, Brush("#B3C8DE")));
            stack.Children.Add(Text("Base legacy record · independent WPF graphics · silent results", 12, Brush("#B3C8DE")));
        }
        var controls = new WrapPanel { Margin = new(0, 20, 0, 0) };
        controls.Children.Add(Button("Back · Esc", () => BackRequested?.Invoke()));
        controls.Children.Add(Button("Retry · R", () => RetryRequested?.Invoke()));
        if (presentation is not null) controls.Children.Add(Button("Reveal score · Space / Enter", () => SkipRequested?.Invoke()));
        stack.Children.Add(controls);
    }
    public void Hide() { Visibility = Visibility.Collapsed; bound = null; Children.Clear(); }
    private static Button Button(string label, Action click)
    {
        var b = new Button { Content = label, Padding = new(14, 9, 14, 9), Margin = new(0, 0, 10, 4), FontSize = 14 };
        b.Click += (_, _) => click(); return b;
    }
    private static TextBlock Text(string value, double size, Brush color) => new()
    { Text = value, FontSize = size, Foreground = color, FontFamily = new("Segoe UI"), TextWrapping = TextWrapping.Wrap, Margin = new(0, 3, 0, 3) };
    private static SolidColorBrush Brush(string value) => new((Color)ColorConverter.ConvertFromString(value));
}
