using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Platform;
using Ohno.CircleGame.Rendering;
using Ohno.CircleGame.Selection;
using Ohno.CircleGame.Difficulty;

namespace Ohno.CircleGame;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = new Application();
        app.Run(new GameWindow(args.FirstOrDefault()));
    }
}

public sealed class GameWindow : Window
{
    private readonly PlayfieldView surface = new();
    private readonly RankResultsView rankView = new();
    private readonly SelectedChartState selectedChart = new();
    private readonly LocalChartCatalog catalog;
    private readonly SelectPlayModel browser;
    private readonly SelectPlayView browserView;
    private readonly PlayEntryFlow entry;
    private readonly CalculatedStarCache starCache = new();
    private readonly SelectPlayStarController stars;
    private readonly SelectionSettings selectionSettings;
    private readonly TextBlock home = new() { Text = "LOCAL PRACTICE HOME\nOpen SelectPlay, a fixture, or an .osu chart using the controls above.", Foreground = Brushes.White, FontSize = 24, TextWrapping = TextWrapping.Wrap, Margin = new(30), Background = Brushes.Black, Visibility = Visibility.Collapsed };
    private bool selectionVisible = true, homeVisible;
    private RankSession? restoredRank;
    private string catalogStatus = "Preparing local metadata index…";
    private string? selectionConfigurationError;
    private readonly TextBlock hud = new() { Foreground = Brushes.White, FontSize = 16, VerticalAlignment = VerticalAlignment.Center };
    private readonly TextBlock mapLabel = new() { Foreground = Brushes.LightSteelBlue, FontSize = 13, Margin = new Thickness(16, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
    private readonly Button playButton;
    private readonly Button restartButton;
    private readonly TextBlock mediaStatus = new() { Foreground = Brushes.LightSteelBlue, FontSize = 13, TextWrapping = TextWrapping.Wrap };
    private readonly List<Button> buttons = [];
    private readonly HashSet<Key> heldKeys = [];
    private readonly TextBlock scoringStatus = new() { Foreground = Brushes.LightSteelBlue, FontSize = 13, TextWrapping = TextWrapping.Wrap };
    private readonly TextBlock healthStatus = new() { Foreground = Brushes.LightSteelBlue, FontSize = 12, TextWrapping = TextWrapping.Wrap };
    private CancellationTokenSource? loadCancellation;
    private BeatmapAudio? pendingAudio;
    private PracticeFixture? audibleFixture;
    private bool loading;
    private bool closing;
    private readonly HitSoundLibrary effectsLibrary;
    private WindowsCircleEffects? pendingEffects;
    private readonly TextBlock effectsStatus = new() { Foreground = Brushes.LightSteelBlue, FontSize = 12, TextWrapping = TextWrapping.Wrap };
    private readonly CheckBox effectsEnabled = new() { Content = "HitSounds (including loops)", Foreground = Brushes.White, Margin = new Thickness(8, 6, 8, 6) };
    private string? effectsConfigurationError;
    private static string EffectsConfigurationPath => Path.Combine(AppContext.BaseDirectory, "hitsounds.json");

    public GameWindow(string? mapPath)
    {
        Title = "ohno · R17 Restricted Circle Stars";
        try { selectionSettings = SelectionSettings.Read(SelectionSettings.ConfigurationPath()); }
        catch (Exception e) { selectionSettings = new(); selectionConfigurationError = "Selection configuration unavailable: " + e.Message; }
        try { catalog = new([selectionSettings.SongsRoot(AppContext.BaseDirectory), Path.Combine(AppContext.BaseDirectory, "Fixtures")], selectionSettings.PreferUnicode); }
        catch (Exception e) { catalog = new([Path.Combine(AppContext.BaseDirectory, "Fixtures")]); selectionConfigurationError = "Songs root unavailable: " + e.Message; }
        browser = new(selectedChart); browserView = new(browser); entry = new(selectedChart);
        stars = new(starCache);
        browserView.CalculatedStars = chart => stars.StateFor(chart).Text;
        browser.Changed += () => { if (selectionVisible) stars.Highlight(browser.Highlighted, browser.CatalogGeneration); };
        stars.Changed += () =>
        {
            if (closing || Dispatcher.HasShutdownStarted) return;
            Dispatcher.BeginInvoke(new Action(() => { if (!closing && selectionVisible) browserView.Bind(); }));
        };
        browser.StartRequested += StartSelected;
        browserView.RefreshRequested += () => _ = RefreshCatalogAsync();
        browserView.OpenRequested += OpenMap;
        browserView.BackRequested += () => { entry.Cancel(); loadCancellation?.Cancel(); selectionVisible = false; homeVisible = true; surface.Pause(); UpdateHud(); };
        try { effectsLibrary = new(HitSoundSettings.Read(EffectsConfigurationPath)); }
        catch (Exception e)
        {
            effectsConfigurationError = "Effects settings unavailable: " + e.Message;
            effectsLibrary = new(new HitSoundSettings { Enabled = false });
        }
        effectsEnabled.IsChecked = effectsLibrary.Settings.Enabled;
        surface.EffectsEnabled = effectsLibrary.Settings.Enabled;
        effectsEnabled.Checked += (_, _) => { surface.EffectsEnabled = true; UpdateHud(); };
        effectsEnabled.Unchecked += (_, _) => { surface.EffectsEnabled = false; UpdateHud(); };
        Width = 1120; Height = 860; MinWidth = 800; MinHeight = 640;
        Background = new SolidColorBrush(Color.FromRgb(12, 19, 36));
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        var root = new Grid();
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition());
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        var tools = new WrapPanel { Margin = new Thickness(16, 12, 16, 12) };
        tools.Children.Add(Button("SelectPlay", ShowBrowser));
        tools.Children.Add(Button("Refresh Songs", () => _ = RefreshCatalogAsync()));
        tools.Children.Add(Button("Resume loaded chart", () => { if (surface.Session is not null) { selectionVisible = false; homeVisible = false; surface.TogglePause(); } }));
        tools.Children.Add(Button("Open .osu", OpenMap));
        playButton = Button("Start / Pause", () => { if (selectionVisible) browser.Confirm(); else if (!homeVisible) surface.TogglePause(); });
        tools.Children.Add(playButton);
        restartButton = Button("Restart / Retry (R)", () => { if (surface.HasRankControls) surface.RetryResults(); else surface.Restart(); });
        tools.Children.Add(restartButton);
        tools.Children.Add(Button("Audible fixture", OpenAudibleFixture));
        tools.Children.Add(Button("Silent fixture", () => _ = LoadMapAsync(BundledSilentMap)));
        tools.Children.Add(Button("Slider fixture", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "first-slider.osu"))));
        tools.Children.Add(Button("Tick fixture", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "slider-ticks.osu"))));
        tools.Children.Add(Button("Repeat fixture", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "slider-repeat.osu"))));
        tools.Children.Add(Button("Mixed R05", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "mixed-sliders.osu"))));
        tools.Children.Add(Button("Quadratic", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "bezier-quadratic.osu"))));
        tools.Children.Add(Button("Cubic", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "bezier-cubic.osu"))));
        tools.Children.Add(Button("Joined Bézier", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "bezier-joined.osu"))));
        tools.Children.Add(Button("Mixed R06", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "bezier-mixed.osu"))));
        tools.Children.Add(Button("Spinner Practice", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "spinner-practice.osu"))));
        tools.Children.Add(Button("Circle + Spinner", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "circle-spinner.osu"))));
        tools.Children.Add(Button("Mixed R07", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "mixed-r07.osu"))));
        tools.Children.Add(Button("Health R09", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "health-r09.osu"))));
        tools.Children.Add(Button("Discrete R12", () => _ = LoadMapAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "discrete-slider-r12.osu"))));
        tools.Children.Add(effectsEnabled);
        tools.Children.Add(Button("Effects setup", () => MessageBox.Show(this,
            "Configure hitsounds.json beside the built executable, then relaunch.\n" + EffectsConfigurationPath +
            "\n\nOptional SkinDirectory and DefaultPackDirectory admit local roots. The full independent pack uses 23 exact key.wav files. " +
            "Spinner sound needs spinnerspin.wav and spinnerbonus.wav in the configured pack. Restart the app after changing settings or missing pack files. See CircleGame/README.md for the JSON schema and independent sample generators. No original samples are supplied.", "HitSounds setup")));
        tools.Children.Add(Button("Save audio diagnostics", SaveAudioDiagnostics));
        tools.Children.Add(mapLabel);
        root.Children.Add(tools);
        var playHost = new Grid(); playHost.Children.Add(surface); playHost.Children.Add(rankView); playHost.Children.Add(home); playHost.Children.Add(browserView);
        Grid.SetRow(playHost, 1); root.Children.Add(playHost);
        rankView.BackRequested += () => { heldKeys.Clear(); surface.BackFromResults(); };
        rankView.RetryRequested += () => { heldKeys.Clear(); surface.RetryResults(); };
        rankView.SkipRequested += surface.SkipResultsReveal;
        surface.ResultsCommitted += () => { heldKeys.Clear(); PublishHeldKeys(); };
        var footer = new StackPanel { Margin = new Thickness(20, 12, 20, 14) };
        footer.Children.Add(hud);
        footer.Children.Add(scoringStatus);
        footer.Children.Add(healthStatus);
        footer.Children.Add(new TextBlock { Text = "Aim: mouse   Hit / hold: Z / X   Pause: Space   Restart: R / Enter   •   Spinner: hold and rotate around center",
            Foreground = Brushes.LightSteelBlue, FontSize = 13, Margin = new Thickness(0, 7, 0, 0) });
        footer.Children.Add(mediaStatus);
        footer.Children.Add(effectsStatus);
        Grid.SetRow(footer, 2); root.Children.Add(footer);
        Content = root;
        surface.FrameUpdated += UpdateHud;
        PreviewKeyDown += KeyPressed;
        PreviewKeyUp += (_, e) => { heldKeys.Remove(e.Key); PublishHeldKeys(); };
        Deactivated += (_, _) => { heldKeys.Clear(); PublishHeldKeys(); surface.Pause(); UpdateHud(); };
        Closed += (_, _) =>
        {
            closing = true;
            entry.Dispose(); catalog.Dispose(); browserView.Dispose();
            stars.Dispose(); starCache.Dispose();
            loadCancellation?.Cancel();
            pendingAudio?.Dispose(); // Synchronous release, even if an await cannot resume at shutdown.
            pendingEffects?.Dispose();
            surface.Dispose();
            effectsLibrary.Dispose();
            audibleFixture?.Dispose();
        };
        Loaded += async (_, _) =>
        {
            await RefreshCatalogAsync();
            if (mapPath is not null) await LoadMapAsync(mapPath);
            else ShowBrowser();
            if (!closing) surface.Focus();
        };
    }

    private void ShowBrowser()
    {
        entry.Cancel(); loadCancellation?.Cancel();
        surface.Pause(); heldKeys.Clear(); PublishHeldKeys(); selectionVisible = true; homeVisible = false;
        browser.Restore(); UpdateHud();
    }
    private async Task RefreshCatalogAsync(string? external = null)
    {
        if (closing) return;
        entry.Cancel(); loadCancellation?.Cancel();
        stars.Invalidate();
        catalogStatus = "Indexing local .osu metadata…"; UpdateHud();
        try
        {
            var snapshot = await catalog.RefreshAsync(addExternal: external);
            if (closing || snapshot is null) return;
            browser.Rebuild(snapshot);
            catalogStatus = $"{snapshot.Charts.Count} local charts · " + string.Join(" · ", snapshot.Notices.Take(3));
        }
        catch (Exception e) { if (!closing) catalogStatus = "Catalog unavailable: " + e.Message; }
        if (!closing) UpdateHud();
    }
    private void StartSelected()
    {
        if (closing || entry.Request(browser.SpecialTemporarySelection) != StartDecision.Accepted) { UpdateHud(); return; }
        entry.Accept(); entry.Queue();
        long queuedGeneration = entry.Generation;
        stars.Invalidate();
        // ENGINEERING next dispatcher turn, not a measured1000ms original delay.
        Dispatcher.BeginInvoke(new Action(async () =>
        {
            if (closing || queuedGeneration != entry.Generation || !entry.Commit() || !entry.Capture()) { UpdateHud(); return; }
            long ticket = entry.BeginLoading();
            await LoadMapAsync(entry.Captured!.Path, entry.Captured, ticket);
        }));
        UpdateHud();
    }

    private void SaveAudioDiagnostics()
    {
        // Owner action; serialization/file writing never occurs in gameplay callbacks.
        surface.Pause();
        var dialog = new SaveFileDialog { Filter = "Audio diagnosis (*.json)|*.json", FileName = "ohno-audio-diagnostics.json" };
        if (dialog.ShowDialog(this) != true) return;
        try
        {
            var fallback = new EffectAudioDiagnostics(effectsLibrary.Settings, EffectsConfigurationPath);
            fallback.Set("preparationOrConfigurationError", effectsConfigurationError ?? surface.EffectsNote);
            fallback.Set("effectsInstance", "UNAVAILABLE");
            File.WriteAllText(dialog.FileName, surface.EffectsDiagnosticsJson ?? fallback.Json());
        }
        catch (Exception e) { MessageBox.Show(this, e.Message, "Could not save audio diagnostics"); }
    }

    private static string BundledSilentMap => Path.Combine(AppContext.BaseDirectory, "Fixtures", "first-circles.osu");
    private void PublishHeldKeys() => surface.SetHeldButtons((heldKeys.Contains(Key.Z) ? 1 : 0) | (heldKeys.Contains(Key.X) ? 2 : 0));

    private Button Button(string text, Action click)
    {
        var button = new Button { Content = text, Padding = new Thickness(14, 7, 14, 7),
            Margin = new Thickness(0, 0, 8, 6), FontSize = 14 };
        button.Click += (_, _) => { click(); surface.Focus(); UpdateHud(); };
        buttons.Add(button);
        return button;
    }

    private async void OpenMap()
    {
        surface.Pause();
        var dialog = new OpenFileDialog { Filter = "osu! beatmap (*.osu)|*.osu", Title = "Open a supported osu!standard beatmap" };
        if (dialog.ShowDialog(this) == true) await LoadMapAsync(dialog.FileName);
    }

    private async void OpenAudibleFixture()
    {
        surface.Pause();
        try
        {
            audibleFixture ??= new PracticeFixture();
            await LoadMapAsync(audibleFixture.MapPath);
        }
        catch (Exception error)
        {
            if (!closing) MessageBox.Show(this, error.Message, "Could not create audible fixture", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        if (!closing) UpdateHud();
    }

    private async Task LoadMapAsync(string path, CatalogChart? captured = null, long ticket = -1)
    {
        if (closing) return;
        if (captured is null) entry.Cancel();
        loadCancellation?.Cancel();
        pendingAudio?.Dispose();
        pendingEffects?.Dispose();
        var request = new CancellationTokenSource();
        loadCancellation = request;
        BeatmapAudio? candidateAudio = null;
        WindowsCircleEffects? candidateEffects = null;
        loading = true;
        heldKeys.Clear();
        PublishHeldKeys();
        surface.Pause();
        UpdateHud();
        try
        {
            captured?.Verify(request.Token);
            Beatmap map = Beatmap.Load(path); // Parse before replacing any active state.
            ScoreConfiguration scoring = ScoreConfiguration.FromBeatmap(map); // No fallback preset on error.
            candidateAudio = new BeatmapAudio(map.AudioPath);
            pendingAudio = candidateAudio;
            await candidateAudio.PrepareAsync(request.Token);
            if (closing || request.IsCancellationRequested || !ReferenceEquals(loadCancellation, request)) return;
            surface.EffectsNote = effectsConfigurationError ?? "preparing optional assets";
            if (effectsConfigurationError is null)
            {
                try
                {
                    candidateEffects = await WindowsCircleEffects.PrepareAsync(map, effectsLibrary, Dispatcher, request.Token,
                        effect => pendingEffects = effect);
                }
                catch (OperationCanceledException) when (request.IsCancellationRequested || closing) { throw; }
                catch (Exception e) { surface.EffectsNote = "Resource preparation failed: " + e.Message; }
            }
            if (closing || request.IsCancellationRequested || !ReferenceEquals(loadCancellation, request)) return;
            captured?.Verify(request.Token);
            if (captured is not null && !entry.CanInstall(ticket)) return;
            surface.Load(map, candidateAudio, scoring, candidateEffects);
            candidateEffects = null; pendingEffects = null;
            candidateAudio = null; // Surface now owns this prepared player.
            pendingAudio = null;
            mapLabel.Text = map.Title.Length <= 38 ? map.Title : map.Title[..35] + "…";
            mapLabel.ToolTip = $"{map.Title}\n{path}";
            selectionVisible = false; homeVisible = false; restoredRank = null;
            if (captured is not null) { captured.ReportGameplay(null); entry.Ready(ticket); surface.TogglePause(); }
            else
            {
                // Compatibility picker/fixtures still load paused; register their selection without replacing loaded gameplay.
                try
                {
                    var snapshot = await catalog.RefreshAsync(addExternal: path);
                    if (!closing && !request.IsCancellationRequested && ReferenceEquals(loadCancellation, request) && snapshot is not null)
                    { browser.Rebuild(snapshot); var row = snapshot.Charts.FirstOrDefault(c => CatalogPaths.Comparer.Equals(c.Path, Path.GetFullPath(path))); if (row is not null) { row.ReportGameplay(null); browser.Highlight(row); browser.Commit(); } }
                }
                catch (Exception e) { if (!closing && ReferenceEquals(loadCancellation, request)) catalogStatus = "Loaded through compatibility route; catalog unavailable: " + e.Message; }
            }
        }
        catch (OperationCanceledException) when (request.IsCancellationRequested || closing)
        { if (captured is not null && entry.CanInstall(ticket)) entry.Cancel(); }
        catch (Exception error)
        {
            if (captured is not null && entry.CanInstall(ticket) && ReferenceEquals(loadCancellation, request) && !closing) { captured.ReportGameplay(error.Message); entry.Fail(ticket, error.Message); selectionVisible = true; homeVisible = false; catalogStatus = error.Message; browser.Rebuild(catalog.Snapshot); }
            // Includes native decoder/format/IO failures; keep the old session paused.
            if (!closing && ReferenceEquals(loadCancellation, request))
                if (captured is null) MessageBox.Show(this, error.Message + (surface.Session is null ?
                    "\n\nChoose another map or one of the practice fixtures." :
                    "\n\nThe previous map and progress have been retained. Space resumes it."),
                    "Could not load beatmap/audio", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        finally
        {
            candidateAudio?.Dispose();
            candidateEffects?.Dispose();
            if (ReferenceEquals(loadCancellation, request))
            {
                pendingAudio = null;
                pendingEffects = null;
                loadCancellation = null;
                loading = false;
                if (!closing) UpdateHud();
            }
            request.Dispose();
        }
    }

    private void KeyPressed(object sender, KeyEventArgs e)
    {
        if (selectionVisible)
        { if (!closing && HandleBrowserKey(e.Key, e.IsRepeat)) { e.Handled = true; UpdateHud(); } return; }
        if (homeVisible) return;
        if (e.Key is not (Key.Z or Key.X or Key.Space or Key.R or Key.Enter or Key.Escape)) return;
        e.Handled = true;
        if (loading || closing) return;
        if (e.IsRepeat || !heldKeys.Add(e.Key)) return;
        if (surface.HasRankControls)
        {
            heldKeys.Clear(); PublishHeldKeys();
            if (e.Key is Key.Space or Key.Enter) surface.SkipResultsReveal();
            else if (e.Key == Key.Escape) surface.BackFromResults();
            else if (e.Key == Key.R) surface.RetryResults();
            UpdateHud(); return;
        }
        PublishHeldKeys();
        switch (e.Key)
        {
            case Key.Z: case Key.X: surface.Press(e.Key == Key.Z ? 1 : 2); break;
            case Key.Space: surface.TogglePause(); break;
            case Key.R: case Key.Enter: surface.Restart(); break;
        }
        UpdateHud();
    }

    private bool HandleBrowserKey(Key key, bool repeated)
    {
        switch (key)
        {
            case Key.Up: browser.Move(-1); break;
            case Key.Down: browser.Move(1); break;
            case Key.PageUp: browser.Move(-10); break;
            case Key.PageDown: browser.Move(10); break;
            case Key.Left: browser.MoveGroup(-1); break;
            case Key.Right: browser.MoveGroup(1); break;
            case Key.Enter: if (!repeated) browser.Confirm(); break;
            case Key.Escape: entry.Cancel(); loadCancellation?.Cancel(); selectionVisible = false; homeVisible = true; surface.Pause(); break;
            default: return false;
        }
        return true;
    }

    private void UpdateHud()
    {
        if (surface.Rank is { Stage: RankStage.ChartSelection } returning && !ReferenceEquals(returning, restoredRank))
        { restoredRank = returning; selectionVisible = true; homeVisible = false; browser.Restore(); _ = RefreshCatalogAsync(); }
        browserView.Visibility = selectionVisible ? Visibility.Visible : Visibility.Collapsed;
        home.Visibility = homeVisible ? Visibility.Visible : Visibility.Collapsed;
        browserView.SetStatus((selectionConfigurationError is null ? "" : selectionConfigurationError + " · ") + catalogStatus + $" · Play entry: {entry.Stage}" + (entry.Captured is null ? "" : " · Captured: " + entry.Captured.Path) + (entry.Error is null ? "" : " · " + entry.Error));
        if (surface.Rank is { } rank && rank.Stage is RankStage.ResultsPresented or RankStage.Unavailable)
            rankView.Bind(rank.Presentation, rank.Limitation, rank.Reveal?.Text(surface.RankHostTime) ?? "");
        else rankView.Hide();
        foreach (Button button in buttons) button.IsEnabled = !loading && !closing;
        playButton.IsEnabled &= selectionVisible ? browser.Highlighted?.Available == true : !homeVisible && surface.Session is not null;
        restartButton.IsEnabled &= !selectionVisible && !homeVisible && surface.Session is not null;
        effectsEnabled.IsEnabled = !loading && !closing;
        effectsStatus.Text = loading ? "Preparing optional effects off the judgement path…" : surface.EffectsStatus;
        mediaStatus.Text = loading ? "Loading local beatmap audio… previous session paused (15-second timeout)." : surface.AudioStatus;
        if (surface.Session is not { } game) { hud.Text = "No map loaded"; return; }
        ScoreLedger ledger = game.Ledger;
        hud.Text = ledger.IsSupported ? $"Score {ledger.Score:N0}   Accuracy {ledger.AccuracyPercent:F2}%   " +
            $"Combo {ledger.CurrentCombo}×   Best {ledger.MaximumCombo}×   {game.JudgedCount}/{game.ObjectCount} objects   {surface.CurrentTime / 1000.0:F2}s" :
            $"Accounting unsupported · {game.JudgedCount}/{game.ObjectCount} objects";
        scoringStatus.Text = ledger.PolicyLabel + (ledger.IsSupported ? "" : " · " + ledger.UnsupportedReason);
        healthStatus.Text = game.Health is { } hp ?
            $"{game.HealthStatus} · H {hp.State.H:F2} / V {hp.State.V:F2} / A {hp.State.A:F2} · " +
            $"r {hp.State.Rate:G6}, B {hp.State.B:G5}, G {hp.State.G:G5}, trials {hp.Calibration.Trials}" +
            (hp.Failed ? " · failure latched; departure unimplemented, practice continues" : "") : game.HealthStatus;
        playButton.Content = surface.HasRankControls ? "Reveal score" : surface.Running ? "Pause (Space)" : "Start / Resume";
    }
}
