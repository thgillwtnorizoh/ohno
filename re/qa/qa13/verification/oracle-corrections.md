# QA13 initial local evidence and independently justified corrections

Production and historical assertions were never changed.

1. Initial headless run:229 named controls,228 passed/1 failed. The exact
   145581092-byte result is retained reversibly as
   `initial-local/results.json.gz.b64`, with source snapshots and SHA256.
   `C/maximum-end-not-last-start` supplied spinner1000–6000 and circle5000.
   Existing R07 Beatmap.Load rejects overlapping spinner intervals before any
   Rank constructor. The fixture exceeded the declared parser domain. The final
   suite retains that exact parser rejection and adds a distinctly named
   artificial manually assembled component map to test maximum-end selection.
   The artificial map is explicitly rejected for fresh Rank admission.

2. Initial Windows local run:24 named controls,18 passed/6 failed (W08–W13).
   Exact JSON, UTF16LE console log and original Program.cs are retained under
   `initial-windows-local/`. The harness traversed VisualTreeHelper beneath an
   unshown ScrollViewer. Its template is not materialized by constructing/binding
   the component; the expected logical TextBlocks/Buttons do exist. The final
   walker uses LogicalTreeHelper for the constructed logical child hierarchy.
   No Window.Show, ApplyTemplate, screenshot or physical input was added.
   The unchanged field/text/callback assertions then pass, as do host cleanup,
   Back, Retry and empty-effects controls. This is a harness inspection-boundary
   correction, not a production view repair or visual fidelity finding.

Local SDK10.0.402 targeted net8 and executed installed .NET8.0.17, including
Windows components. The first Windows restore emitted NU1900 because the local
network could not fetch vulnerability metadata. A later local command supplied
`-p:NuGetAudit=false` without modifying the production project. The established
Windows SDK8 gate remains authoritative for the clean Release build.

R30 is a separate preserved historical-description conflict; see
`../R30-analysis.md`. It did not fail QA13's independently derived B expectation.
