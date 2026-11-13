# QA15 human runtime qualification — PENDING HUMAN TEST

The owner must physically launch and operate both Windows applications. An agent's
unshown WPF tests, mathematical outputs, callbacks and source audits cannot pass this
gate. Do not execute/instrument the original within Work1. Use your legitimate
installation without patches or server-verification changes.

## Record the environment before testing

Record observer/date/time/timezone; Windows version; GPU/driver; display resolution,
DPI/scaling, refresh rate and window mode; frame limiter/compatibility mode; skin,
audio device, music/effects volumes and gameplay keys. Record original executable
path, SHA256, file/product version and visible client build string. Record Project
ohno exact **build source SHA**, executable SHA256, build configuration/runtime and
selection.json location/BeatmapDirectory. Screenshots must show the relevant chart
identity and the star value. Keep personal information out of captures if desired.

Research target: archived `osu!.exe` SHA256
`6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`, assembly
40/1.3.3.8; official dated release identity remains unauthenticated. Record your
actual build even if different. Build mismatch is a comparison condition, not a
reason to fabricate or discard a result.

PowerShell identity commands (substitute your paths):

```powershell
(Get-Item 'C:\osu!\osu!.exe').VersionInfo | Format-List FileVersion,ProductVersion
Get-FileHash 'C:\osu!\osu!.exe' -Algorithm SHA256
Get-FileHash 'C:\ohno\Ohno.CircleGame.exe' -Algorithm SHA256
```

## A. Original osu!stable: all27 positive comparison charts

1. Follow `beatmaps/README.md` to copy folder children or import the individual osz
   archives. Confirm all27 intended titles and Version strings appear. Record any
   absent/rejected chart and its import error; do not replace it with another map.
2. For each chart verify the installed `.osu` SHA256 against the manifest. Record
   the source and installed paths. If the game has rewritten bytes, stop that
   comparison; retain both files and report INCONCLUSIVE for the prescribed hash.
3. Select the exact difficulty; select osu!standard and disable all modifiers.
   Record any offsets/speed/difficulty overrides; use zero adjustment/rate1.
4. Allow local difficulty calculation to finish. If stale, use F5 reprocessing and
   reselect, recording what you did. Do not manually edit its cached star value.
5. Copy the star text **exactly as displayed**, including all shown decimal places.
   Capture `QA15-XX-original.png` showing chart identity and that value.
6. Record unavailable/pending results honestly. Leave the original value blank if
   no value was observed. Research predictions must never fill this column.

## B. Project ohno: same27 `.osu` bytes

1. Use the specified R17 Windows build. Set BeatmapDirectory to the extracted Songs
   path or use Open .osu and return to SelectPlay. Record the build source SHA.
2. Verify `.osu` SHA256. Highlight the intended chart in SelectPlay. Observe
   Pending → Calculated and `original-derived restricted calculation`. Record
   the actual numerical text and capture `QA15-XX-ohno.png`. Very short jobs may
   finish before Pending is visually captured; record that, do not invent a frame.
3. Navigate to a different compatible difficulty and confirm its own value appears.
   Return to the first; confirm restoration and immediate reuse if observable.
   UI speed alone cannot prove an internal cache hit: record its visible behavior.
4. Refresh catalog; confirm the selected chart/rating returns. If explicit
   recommit is requested, record it. Record wrong-chart/stale ratings as failures.
5. Repeat for all27. Preserve exact display precision in the result CSV.

## C. Side-by-side comparison

For each row record both actual displays and matching chart SHA256. Keep the expected
R17 columns unchanged. In `displayed_difference_ohno_minus_original`, subtract the
actual original displayed number from the actual ohno displayed number. Leave it
blank if either observation is absent. Do not round the applications to a common
number or pretend that 1.2 and1.23 agree. `1.20` and1.2 represent the same numeric
value, but preserve their original text in separate observation fields.

Allowed final classifications:

| Status | Meaning |
|---|---|
| MATCH | Both observed numerical displays agree for identical bytes/profile. |
| MISMATCH | Both observed numerical displays differ. Preserve signed difference. |
| ORIGINAL UNAVAILABLE | Original cannot import/display/calculate the chart. |
| RECONSTRUCTION UNSUPPORTED | R17 rejects the provided chart/profile. |
| INCONCLUSIVE | Hash/build/cache/mod/runtime conditions prevent reliable comparison. |

Before observations, status stays PENDING HUMAN TEST. A mismatch calls for evidence
review: original manager preparation, cache freshness, build version, precision and
unsupported original runtime behavior can differ. Do not fit R17 mathematics to an
unexplained display. A confirmed production discrepancy requires a separately
authorized Work2 correction; missing original knowledge may require new research.

## D. Gameplay/navigation: physical mouse and keyboard

Use25–27 first (2.5s lead-in), then23/24 for sustained ordinary play. Record chosen
IDs, hashes and input bindings. Use the same no-mod profile. In both applications,
where applicable, physically play and record score, judgements, health, audible
hitsounds/effects and completion. Silent music is intentional; it does not silence
built-in hitsounds. In ohno, verify effects settings/resources from its README;
optional independent sound-pack scripts generate original tones. Record absent
resources or inaudible effects as limitations/failures; do not claim audio passed.

In ohno verify and capture:

1. Supported selection and correct stars, then start the committed chart.
2. Physical Z/X (or configured keys) and mouse targeting; score/judgement/HP/effects.
3. Completion → Rank with the completed chart's results.
4. Back → SelectPlay restores that chart and rating.
5. Fresh Retry starts the captured chart with clean score/judgement/HP/time state.
6. In a **disposable copy**, rename/remove the selected .osu; return/refresh/start.
   Record unavailable recovery without a stale numeric result/session overwrite.
7. Restore or modify the disposable file; refresh, explicitly commit its new row,
   and confirm the correct recalculation. Never edit canonical corpus files.
8. Rapidly alternate compatible charts and a slider/spinner; no late old value may
   replace the newly highlighted chart. Capture incorrect identity/value pairs.
9. Use bundled `first-slider.osu` and `spinner-practice.osu`: they remain playable,
   with explicitly unsupported/nonnumeric stars. They are negative controls outside
   the positive comparison corpus; do not assign them research star values.

Log exact steps, expected/observed behavior, screenshot filenames and repeatability
for every failure. Do not skip a failure silently or change mods to manufacture a pass.

## Submission and gate decision

Return the edited comparison-results.csv, filled human-results-template.md and
screenshots; include executable/version/build and installed-chart hash evidence.
All27 positive cases require an observation or an explicit unavailable/inconclusive
status. Keep gameplay/navigation evidence separately scoped by tested case IDs.

Gate1 is corpus engineering. Gate2 is independent component/Windows regression.
Gate3 remains **PENDING HUMAN TEST** until actual owner observations are reviewed.
Numerical agreement qualifies only the tested chart/profile/build cases. No blanket
original-game equivalence or R17 owner acceptance follows from automated passes.
