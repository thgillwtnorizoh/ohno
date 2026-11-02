using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Ohno.CircleGame.Gameplay;
using Ohno.CircleGame.Rendering;

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
    private readonly TextBlock hud = new() { Foreground = Brushes.White, FontSize = 16, VerticalAlignment = VerticalAlignment.Center };
    private readonly TextBlock mapLabel = new() { Foreground = Brushes.LightSteelBlue, FontSize = 13, Margin = new Thickness(16, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
    private readonly Button playButton;
    private readonly HashSet<Key> heldKeys = [];

    public GameWindow(string? mapPath)
    {
        Title = "ohno · R02 Circle Practice";
        Width = 1120; Height = 860; MinWidth = 800; MinHeight = 640;
        Background = new SolidColorBrush(Color.FromRgb(12, 19, 36));
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        var root = new Grid();
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition());
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        var tools = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(16, 12, 16, 12) };
        tools.Children.Add(Button("Open .osu", OpenMap));
        playButton = Button("Start / Pause", () => surface.TogglePause());
        tools.Children.Add(playButton);
        tools.Children.Add(Button("Restart (R)", () => surface.Restart()));
        tools.Children.Add(mapLabel);
        root.Children.Add(tools);
        Grid.SetRow(surface, 1); root.Children.Add(surface);
        var footer = new StackPanel { Margin = new Thickness(20, 12, 20, 14) };
        footer.Children.Add(hud);
        footer.Children.Add(new TextBlock { Text = "Aim: mouse   Hit: Z / X   Pause: Space   Restart: R / Enter   •   Silent, no mods, circles only",
            Foreground = Brushes.LightSteelBlue, FontSize = 13, Margin = new Thickness(0, 7, 0, 0) });
        Grid.SetRow(footer, 2); root.Children.Add(footer);
        Content = root;
        surface.FrameUpdated += UpdateHud;
        PreviewKeyDown += KeyPressed;
        PreviewKeyUp += (_, e) => heldKeys.Remove(e.Key);
        Deactivated += (_, _) => { heldKeys.Clear(); surface.Pause(); UpdateHud(); };
        Closed += (_, _) => surface.Dispose();
        Loaded += (_, _) =>
        {
            string path = mapPath ?? Path.Combine(AppContext.BaseDirectory, "Fixtures", "first-circles.osu");
            LoadMap(path);
            surface.Focus();
        };
    }

    private Button Button(string text, Action click)
    {
        var button = new Button { Content = text, Padding = new Thickness(14, 7, 14, 7),
            Margin = new Thickness(0, 0, 8, 0), FontSize = 14 };
        button.Click += (_, _) => { click(); surface.Focus(); UpdateHud(); };
        return button;
    }

    private void OpenMap()
    {
        surface.Pause();
        var dialog = new OpenFileDialog { Filter = "osu! beatmap (*.osu)|*.osu", Title = "Open a circle-only osu!standard beatmap" };
        if (dialog.ShowDialog(this) == true) LoadMap(dialog.FileName);
    }

    private void LoadMap(string path)
    {
        try
        {
            Beatmap map = Beatmap.Load(path); // Parse fully before replacing a session.
            surface.Load(map);
            mapLabel.Text = map.Title.Length <= 38 ? map.Title : map.Title[..35] + "…";
            mapLabel.ToolTip = $"{map.Title}\n{path}";
        }
        catch (Exception error) when (error is IOException or InvalidDataException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            MessageBox.Show(this, error.Message, "Could not load beatmap", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        UpdateHud();
    }

    private void KeyPressed(object sender, KeyEventArgs e)
    {
        if (e.Key is not (Key.Z or Key.X or Key.Space or Key.R or Key.Enter)) return;
        e.Handled = true;
        if (e.IsRepeat || !heldKeys.Add(e.Key)) return;
        switch (e.Key)
        {
            case Key.Z: case Key.X: surface.Press(); break;
            case Key.Space: surface.TogglePause(); break;
            case Key.R: case Key.Enter: surface.Restart(); break;
        }
        UpdateHud();
    }

    private void UpdateHud()
    {
        if (surface.Session is not { } game) { hud.Text = "No map loaded"; return; }
        hud.Text = $"{game.JudgedCount}/{game.Circles.Count} circles     Combo {game.Combo}×     Best {game.MaxCombo}×     " +
            $"Accuracy {game.Accuracy:F2}%     {surface.CurrentTime / 1000.0:F2}s";
        playButton.Content = surface.Running ? "Pause (Space)" : game.Finished ? "Play again" : "Start / Resume";
    }
}
