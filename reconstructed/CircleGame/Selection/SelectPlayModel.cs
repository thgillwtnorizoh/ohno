namespace Ohno.CircleGame.Selection;

public sealed class SelectedChartState
{
    public CatalogChart? Current { get; private set; }
    public int Revision { get; private set; }
    public bool Set(CatalogChart? candidate, Action<CatalogChart>? beforeStore = null)
    {
        if (candidate is null || ReferenceEquals(Current, candidate)) return false;
        if (Current is { } old) beforeStore?.Invoke(old); // Exception retains old selection.
        Current = candidate; Revision++; return true;
    }
}
public sealed record DifficultyRow(CatalogChart Chart, int Index, int Run, int FirstInRun);
public sealed record SongRun(int Id, string Folder, IReadOnlyList<DifficultyRow> Rows)
{
    public string Label => Id < 0 ? $"Unavailable charts · {Rows.Count}" : (Rows.FirstOrDefault()?.Chart.Metadata?.Title ?? System.IO.Path.GetFileName(Folder)) + $" · {Rows.Count} difficulties";
}
public sealed class SelectPlayModel
{
    public SelectedChartState Selection { get; }
    public IReadOnlyList<DifficultyRow> Rows { get; private set; } = [];
    public IReadOnlyList<SongRun> Groups { get; private set; } = [];
    public CatalogChart? Highlighted { get; private set; }
    public CatalogChart? Committed { get; private set; }
    public Action<CatalogChart?>? SelectionCallback { get; set; }
    public event Action? Changed;
    public event Action? StartRequested;
    public bool SpecialTemporarySelection { get; set; }
    public string? Recovery { get; private set; }
    public string? SelectionError { get; private set; }
    public long CatalogGeneration { get; private set; } = -1;
    public SelectPlayModel(SelectedChartState selection)
    { Selection = selection; SelectionCallback = candidate => Selection.Set(candidate); }
    public bool Rebuild(CatalogSnapshot snapshot)
    {
        if (snapshot.Generation < CatalogGeneration) return false;
        CatalogGeneration = snapshot.Generation;
        var rows = new List<DifficultyRow>(); var runs = new List<SongRun>(); List<DifficultyRow>? current = null; string? folder = null; int run = -1, first = -1;
        foreach (var chart in snapshot.Charts) // Supplied order, not online set-ID GroupBy.
        {
            if (!chart.Available) { rows.Add(new(chart, rows.Count, -1, -1)); folder = null; current = null; continue; }
            if (folder is null || !CatalogPaths.Comparer.Equals(folder, chart.Folder))
            { folder = chart.Folder; run++; first = rows.Count; current = new(); runs.Add(new(run, folder, current)); }
            var row = new DifficultyRow(chart, rows.Count, run, first); rows.Add(row); current!.Add(row);
        }
        Rows = rows; Groups = runs; Restore(); return true;
    }
    public bool Restore()
    {
        var current = Selection.Current;
        var row = Rows.FirstOrDefault(r => ReferenceEquals(r.Chart, current) && r.Chart.Available);
        if (row is not null)
        { Highlighted = row.Chart; Committed = row.Chart; Recovery = null; Changed?.Invoke(); return true; }
        // Null callback does not erase authoritative selected chart.
        Highlighted = current is null ? Rows.FirstOrDefault(r => r.Chart.Available)?.Chart : null;
        Committed = null;
        Recovery = current is null ? null : "Selected chart is missing, changed or unavailable. Refresh and commit an available difficulty.";
        Changed?.Invoke(); return false;
    }
    public void Highlight(CatalogChart chart)
    {
        var row = Rows.FirstOrDefault(r => ReferenceEquals(r.Chart, chart)); if (row is null) return;
        Highlighted = chart;
        var committed = Rows.FirstOrDefault(r => ReferenceEquals(r.Chart, Committed));
        if (chart.Available && committed is { Run: >= 0 } && row.Run == committed.Run) Commit();
        else Changed?.Invoke(); // Different run: highlight only.
    }
    public bool Commit()
    {
        if (Highlighted is not { Available: true } chart || !Rows.Any(r => ReferenceEquals(r.Chart, chart))) return false;
        Committed = chart; // Original row field update precedes callback/global write.
        try
        {
            if (SelectionCallback is null) { SelectionError = "Selection callback unavailable; authoritative selection retained."; Changed?.Invoke(); return false; }
            SelectionCallback(chart); SelectionError = null; Recovery = null; Changed?.Invoke(); return true;
        }
        catch (Exception e) { SelectionError = "Selection callback failed: " + e.Message; Changed?.Invoke(); return false; } // ENGINEERING containment; partial writes retained.
    }
    public void Confirm()
    {
        if (Highlighted is not { Available: true }) return;
        if (!ReferenceEquals(Highlighted, Committed) || !ReferenceEquals(Selection.Current, Committed)) Commit();
        else StartRequested?.Invoke();
    }
    public void MouseSelect(CatalogChart chart)
    {
        bool repeated = ReferenceEquals(chart, Committed) && ReferenceEquals(chart, Selection.Current);
        Highlight(chart); if (!chart.Available) return;
        if (repeated) StartRequested?.Invoke(); else Commit();
    }
    public void Move(int delta)
    {
        var choices = Rows.Where(r => r.Chart.Available).Select(r => r.Chart).ToArray(); if (choices.Length == 0) return;
        int i = Array.FindIndex(choices, r => ReferenceEquals(r, Highlighted));
        Highlight(choices[Math.Clamp(i < 0 ? 0 : i + delta, 0, choices.Length - 1)]); // ENGINEERING clamp, not native wrap/filter completeness.
    }
    public void MoveGroup(int delta)
    {
        if (Highlighted is not null && !ReferenceEquals(Highlighted, Committed)) { Commit(); return; }
        if (Groups.Count == 0) return;
        int i = Groups.ToList().FindIndex(g => g.Rows.Any(r => ReferenceEquals(r.Chart, Highlighted)));
        Highlight(Groups[Math.Clamp(i + delta, 0, Groups.Count - 1)].Rows[0].Chart);
    }
}
public enum PlayEntryStage { Selected, StartRequested, Accepted, PendingPlay, PlayCommitted, Captured, Loading, Ready, Failed, Cancelled, Disposed }
public enum StartDecision { Accepted, Rejected, Duplicate }
// ENGINEERING controlled queue/generation; scalar1000 retained as evidence, not a1000ms delay.
public sealed class PlayEntryFlow(SelectedChartState selection) : IDisposable
{
    public PlayEntryStage Stage { get; private set; }
    public CatalogChart? Captured { get; private set; }
    public string? Error { get; private set; }
    public const int TransitionScalar = 1000;
    public long Generation { get; private set; }
    public StartDecision Request(bool specialTemporary = false, int phase = 2)
    {
        if (Stage == PlayEntryStage.Disposed) return StartDecision.Rejected;
        if (Stage is PlayEntryStage.StartRequested or PlayEntryStage.Accepted or PlayEntryStage.PendingPlay or PlayEntryStage.PlayCommitted or PlayEntryStage.Captured or PlayEntryStage.Loading) return StartDecision.Duplicate;
        if (selection.Current is null || specialTemporary || phase != 2)
        { Error = "Start requires a selected chart, ordinary selection and ready transition phase2."; return StartDecision.Rejected; }
        Generation++; Error = null; Captured = null; Stage = PlayEntryStage.StartRequested; return StartDecision.Accepted;
    }
    public bool Accept() { if (Stage != PlayEntryStage.StartRequested) return false; Stage = PlayEntryStage.Accepted; return true; }
    public bool Queue() { if (Stage != PlayEntryStage.Accepted) return false; Stage = PlayEntryStage.PendingPlay; return true; }
    public bool Commit() { if (Stage != PlayEntryStage.PendingPlay) return false; Stage = PlayEntryStage.PlayCommitted; return true; }
    public bool Capture()
    {
        if (Stage != PlayEntryStage.PlayCommitted) return false;
        Captured = selection.Current; // Fresh authoritative reference after commitment, no request-time chart payload.
        if (Captured is null) { Error = "Selected chart unavailable at Play capture."; Stage = PlayEntryStage.Failed; return false; }
        Stage = PlayEntryStage.Captured; return true;
    }
    public long BeginLoading()
    { if (Stage != PlayEntryStage.Captured) throw new InvalidOperationException("Play chart must be captured before loading."); Stage = PlayEntryStage.Loading; return Generation; }
    public bool CanInstall(long ticket) => ticket == Generation && Stage == PlayEntryStage.Loading;
    public bool Ready(long ticket) { if (!CanInstall(ticket)) return false; Stage = PlayEntryStage.Ready; return true; }
    public void Fail(long ticket, string message)
    { if (!CanInstall(ticket)) return; Error = message; Stage = PlayEntryStage.Failed; }
    public void Cancel()
    { if (Stage == PlayEntryStage.Disposed) return; Generation++; Stage = PlayEntryStage.Cancelled; Error = "Play entry cancelled; no stale session installed."; }
    public void Dispose() { Cancel(); Stage = PlayEntryStage.Disposed; }
}
