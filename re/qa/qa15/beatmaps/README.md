# QA15 comparison corpus

27 original synthetic osu!standard charts. All use v14, Mode0, no mods,
explicit StackLeniency0, CS5/OD5, AR5 or10, viewport512×384, time shift0 and
strictly increasing five-column circles. Local audio is newly generated PCM16
mono8000Hz silence. There is no music, skin, background, storyboard or game asset.
Built-in hitsounds come from your own installation. Do not upload these controls
as ranked maps.

## Installation into the original osu!stable

1. Extract `QA15-comparison-beatmaps.zip` into a new folder. Close osu!stable.
2. Find the `Songs` folder belonging to the legitimate installation you will test.
3. Copy the **27 child directories inside `QA15-comparison/Songs/`** directly
   into that installation's `Songs/`. Example destination:
   `osu!/Songs/01-basic-two-circle/QA15-01.osu`, alongside its `silence.wav`.
   Do not nest another `Songs` directory inside the existing one. Preserve files.
4. Launch the original normally. Open song selection and search `QA15`.
   Press F5 there to reprocess missing maps. Wait for local difficulty calculation.
5. Alternative: open each `.osz` in the ZIP's `Imports/` folder normally with the
   original client, or place those archives in its Songs directory. Do not also
   install the folder copies; avoid duplicates. Verify the imported `.osu` hash.
6. Confirm each exact title/difficulty identity, no mods, and the target build.
   Record displayed stars and screenshots using the human protocol.

These are documented-format/import candidates validated against the format and
actual R17 reader. **No human original import has been observed.** If the original
rejects a file, report ORIGINAL UNAVAILABLE or INCONCLUSIVE with the error. Do not
substitute a research value. The original may rewrite an imported chart; hash the
installed file again. Edited bytes require a separate observation and calculation.

Primary documentation consulted 2026-11-13:

- [osu! beatmap format](https://osu.ppy.sh/wiki/en/Client/File_formats/osu_%28file_format%29):
  General/audio, metadata, difficulty, timing points; circle object parameters are
  empty and absent hitSample defaults to `0:0:0:0:`. Five-column circle rows are
  deliberate and necessary for R17 admission.
- [osu! installation / adding beatmaps](https://osu.ppy.sh/wiki/en/Client/Installation#adding-beatmaps):
  opening `.osz`, placing archives in Songs, and F5 reprocessing.
- [.osz format](https://osu.ppy.sh/wiki/en/Client/File_formats/osz_%28file_format%29):
  beatmap and supporting resources archive.

## Project ohno

Use the corresponding Windows Release build and configure its `selection.json`
`BeatmapDirectory` to the **absolute extracted `QA15-comparison/Songs` path**.
Preserve any existing selection settings; restart after changing configuration.
Alternatively, use SelectPlay's Open .osu control for each file, then return to
SelectPlay. The compatibility open route can load a paused chart before returning
to the browser. Consult the application README for exact build/start commands.
No external star export is required; look for `original-derived restricted calculation`.

Verify file identity before and after both applications, e.g.:

```powershell
Get-FileHash 'C:\QA15-comparison\Songs\09-combined-repeat-visibility\QA15-09.osu' -Algorithm SHA256
```

Compare with `manifest.json`, ignoring only hex letter case. Do not compare two
same-named charts with different hashes. Keep originals pristine; missing/modified
file recovery uses disposable copies outside this corpus.

## Case identities and limits

01–03 preserve the exact numerical input of E045 two/three/four straight controls:
0.11★ /0.17★ /0.22★. 04–22 preserve all19 E046 raw numerical inputs, including
09 combined-repeat-visibility (**5.11★**), repeated/different rhythms, AR10,
angles, next-jump and history probes. Metadata/audio packaging changes only the
non-numerical envelope; SHA256 is of the final packaged file, not the old fixture.
`source_sha256_lf` identifies LF-normalized historical source transport separately.

23 is regular square movement with300ms rhythm and2.5s lead-in. 24 is a129-circle,
64s temporal-history chart with500ms intervals and2.5s lead-in; it crosses the first
minute and duration retention windows. 25–27 are independently recalculated
E045 straight patterns with every object time moved+2500ms. This modifies absolute
input timestamps and is documented as a separate derived case; time shift remains0.
None of the new expected values is guessed or taken from original-game observation.

The researched cases start at0ms. This is valid input but awkward to play manually;
use25–27 for basic physical play and23/24 for sustained gameplay. Some short dense
fixtures may be difficult to finish under ordinary failure rules: record failures
and test the easier practice cases. Do not add NoFail to a no-mod star comparison.

The exact raw history fixture names do not themselves establish boundary coverage:
capacity-below still reaches32; age-below/equal/above change the final1999/2000/2001ms
interval. Additional prepared controls and explicit independent boundary tests
identify actual reached limits. These charts are probes, not proof of all cases.

`manifest.json` separates expected Double, cached Single/bits and floor display.
`comparison-results.csv` is owner-editable and deliberately leaves **both observed
application values blank**. A predicted ohno value is not an observed GUI value.
Stars in current clients may differ from the unidentified historical build behind
E043–E046. Record actual values and identities; no general stable equivalence is claimed.
