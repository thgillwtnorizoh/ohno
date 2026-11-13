# QA15 — Independent restricted star-calculator verification

Engineering verification in progress. Gates 1 and 2 await the new exact-source
Windows run; Gate 3 is **PENDING HUMAN TEST**. No original game is executed by QA15.

Base `f1df469987ca184d28b66825718873a486a2cfc0`; tested R17 production lineage
`9ae1fbde06812adeaeac7df61de23c4d4268a51c`. Production and historical research/QA
remain unchanged. Only new QA15 files, workflow extension and an append to the
research journal are authorized.

`beatmaps/Songs/` contains 27 original comparison charts with silent local WAVs.
`manifest.json` holds exact final .osu hashes and independent expected R17 values.
Original-game and observed reconstruction results remain blank for the owner.
`verification/expected.json.gz.b64` contains independent E046 oracle traces for
27 packaged raw charts and nine additional prepared-only controls. The latter are
not positive raw comparison charts. No Work2 expected-value harness is imported.

Run on Windows with installed .NET 8 and Python:

```powershell
./re/qa/qa15/Run-QA15.ps1 -ResultPath "$env:TEMP/qa15-results.json"
```

These checks instantiate unshown WPF components only. They cannot qualify physical
input, visual rendering, original-game import, audio playback or GUI navigation.
See the human protocol and result template; submit screenshots and both identities.
