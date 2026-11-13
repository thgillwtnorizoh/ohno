using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ohno.CircleGame.Selection;

namespace Ohno.CircleGame.Rendering;

// Independent ENGINEERING WPF layout; metadata-only and silent. Never a native skin claim.
public sealed class SelectPlayView : Grid, IDisposable
{
    private readonly SelectPlayModel model;
    private readonly ListBox songs = new(), difficulties = new();
    private readonly TextBlock detail = new() { TextWrapping = TextWrapping.Wrap, Foreground = Brushes.White, FontSize = 15 };
    private readonly TextBlock state = new() { TextWrapping = TextWrapping.Wrap, Foreground = Brushes.LightSteelBlue, FontSize = 13 };
    private readonly TextBlock status = new() { TextWrapping = TextWrapping.Wrap, Foreground = Brushes.LightSteelBlue, FontSize = 13 };
    private readonly Button play;
    private bool binding, disposed;
    public Func<CatalogChart,string>? CalculatedStars { get; set; }
    public event Action? RefreshRequested;
    public event Action? OpenRequested;
    public event Action? BackRequested;
    public SelectPlayView(SelectPlayModel model)
    {
        this.model = model; Background = new SolidColorBrush(Color.FromRgb(12, 19, 36)); Margin = new(0);
        RowDefinitions.Add(new() { Height = GridLength.Auto }); RowDefinitions.Add(new()); RowDefinitions.Add(new() { Height = GridLength.Auto });
        var header = new StackPanel { Margin = new(20, 12, 20, 8) };
        header.Children.Add(new TextBlock { Text = "SELECT PLAY  /  LOCAL BEATMAPS", FontSize = 24, Foreground = Brushes.White });
        header.Children.Add(state); Children.Add(header);
        var body = new Grid { Margin = new(16, 0, 16, 8) }; body.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) }); body.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) }); body.ColumnDefinitions.Add(new() { Width = new(1.25, GridUnitType.Star) });
        songs.DisplayMemberPath = "Label"; difficulties.DisplayMemberPath = "Label";
        songs.Margin = new(0, 0, 8, 0); difficulties.Margin = new(0, 0, 10, 0);
        body.Children.Add(songs); Grid.SetColumn(difficulties, 1); body.Children.Add(difficulties);
        var details = new ScrollViewer { Content = detail, VerticalScrollBarVisibility = ScrollBarVisibility.Auto }; Grid.SetColumn(details, 2); body.Children.Add(details); Grid.SetRow(body, 1); Children.Add(body);
        var footer = new StackPanel { Margin = new(20, 8, 20, 12) }; footer.Children.Add(status);
        var buttons = new WrapPanel(); play = Button("Commit difficulty", () => model.Confirm()); buttons.Children.Add(play);
        buttons.Children.Add(Button("Refresh catalog", () => RefreshRequested?.Invoke()));
        buttons.Children.Add(Button("Open .osu", () => OpenRequested?.Invoke())); buttons.Children.Add(Button("Back · Esc", () => BackRequested?.Invoke()));
        footer.Children.Add(buttons); footer.Children.Add(new TextBlock { Text = "↑/↓ nearby · PgUp/PgDn ±10 · ←/→ group/commit · Enter commit/start · click again to start · wheel scroll · silent selection", Foreground = Brushes.LightSteelBlue, FontSize = 12, TextWrapping = TextWrapping.Wrap });
        Grid.SetRow(footer, 2); Children.Add(footer);
        songs.SelectionChanged += (_, _) => { if (!binding && songs.SelectedItem is SongRun run && run.Rows.FirstOrDefault() is { } first) model.Highlight(first.Chart); };
        difficulties.SelectionChanged += (_, _) => { if (!binding && difficulties.SelectedItem is CatalogChart chart) model.Highlight(chart); };
        difficulties.PreviewMouseLeftButtonDown += (_, e) =>
        {
            if (disposed) return;
            if (ItemsControl.ContainerFromElement(difficulties, e.OriginalSource as DependencyObject) is ListBoxItem { Content: CatalogChart chart })
            { e.Handled = true; SelectRow(chart); } // Direct component callback, original geometry/release timing not claimed.
        };
        model.Changed += ModelChanged; Bind();
    }
    public void SetStatus(string text) { if (!disposed) status.Text = text; }
    private void ModelChanged() { if (!disposed) Bind(); }
    public void SelectRow(CatalogChart chart) { if (!disposed) model.MouseSelect(chart); }
    public void Confirm() { if (!disposed) model.Confirm(); }
    public void Bind()
    {
        if (disposed || binding) return;
        binding = true;
        try
        {
            var groups = model.Groups.ToList(); var unavailable = model.Rows.Where(r => !r.Chart.Available).ToArray();
            if (unavailable.Length > 0) groups.Add(new(-1, "Unavailable charts", unavailable));
            songs.ItemsSource = groups;
            var run = groups.FirstOrDefault(g => g.Rows.Any(r => ReferenceEquals(r.Chart, model.Highlighted))) ?? groups.FirstOrDefault();
            songs.SelectedItem = run; difficulties.ItemsSource = run?.Rows.Select(r => r.Chart).ToArray(); difficulties.SelectedItem = model.Highlighted;
            var chart = model.Highlighted;
            detail.Text = chart is null ? "No highlighted difficulty. Configure BeatmapDirectory or open a local .osu file.\n" + model.Recovery :
                (chart.Metadata?.Details ?? chart.Filename) + "\n\n" + (CalculatedStars?.Invoke(chart) ?? StarDisplay.Prepare(chart, chart.Metadata?.Ruleset ?? 0, 0).Text) + "\n\n" +
                (chart.Available ? chart.GameplayValidated ? "Gameplay parser validated this revision." : "Metadata indexed; full gameplay validation occurs on load." : chart.GameplayError ?? chart.Error) + "\n\n" + chart.Path;
            state.Text = "Highlight: " + (chart?.Metadata?.Version ?? chart?.Filename ?? "none") + "  |  Committed: " + (model.Committed?.Filename ?? "none") + "  |  Selected: " + (model.Selection.Current?.Filename ?? "none") +
                (model.Recovery is null ? "" : "\n" + model.Recovery) + (model.SelectionError is null ? "" : "\n" + model.SelectionError);
            play.IsEnabled = chart?.Available == true;
            play.Content = ReferenceEquals(model.Highlighted, model.Committed) && ReferenceEquals(model.Selection.Current, model.Committed) ? "Play selected" : "Commit difficulty";
        }
        finally { binding = false; }
    }
    private static Button Button(string label, Action callback)
    { var b = new Button { Content = label, Padding = new(12, 7, 12, 7), Margin = new(0, 6, 8, 0) }; b.Click += (_, _) => callback(); return b; }
    public void Dispose() { if (disposed) return; disposed = true; model.Changed -= ModelChanged; Children.Clear(); }
}
