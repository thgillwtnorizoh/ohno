# C001 functional/dependency inventory — checkpoint 01

Original-supported domain inventory is OPEN; this is the complete identified
production entry-point inventory at baseline, not an assertion that all original
branches have already been discovered.

| Entry/consumer | Original authority | Current contract / remaining dependency |
|---|---|---|
| Beatmap.Load | E004–006, E032 | OsuDocument → bounded standard circles/L/B/spinner projection; attached raw document |
| LegacyScoreMetadata.ReadFresh | E034 | document entries in original source order; public headerless compatibility adapter retained |
| LocalChartCatalog.Index | E043, engineering index | byte fingerprint → document → metadata projection; index admits more than gameplay |
| StarAdmission.Read/Parse | E044–046 | same syntax; strict v14 circle-only no-stack profile retained; mathematics unchanged |
| CalculatedStarCache | E044–046 + engineering | fingerprint/profile key; stale publication guards retained |
| NewComboPreparation | E036 | raw registration order/types + breaks; ordinary fresh ownership provenance |
| SliderGeometryPreparation/PrepareSlider | E005/006/016/021/022/032 | typed segments, lengths, movements/checkpoints; L and version-aware B; P/C pending |
| CircleSession / ledger | E011–019/033–036 | consumes prepared definitions, K, health source; regression required |
| CircleHitSounds | E037–041 | circle/slider/spinner sidecars + source timing order; short-row defaults unresolved |
| Rendering/PlayfieldLayout | E008/009/014 + engineering | prepared object positions/movements; full translated original stacks pending |
| Program/GameWindow | R02–17 + E043 | selected fingerprint, async loading, session/audio/rank entry; owner physical qualification pending |
| BeatmapAudio / resource library | E010/037–041 + engineering | local contained references, optional independently supplied media; original resource semantics incomplete |
| SelectPlay/Rank flow | E027/042/043 + engineering | identity, captured chart, Back/Retry; no extra independent parser |

Additional non-production parsers are evidence tools, fixture generators and QA
oracles; keep them independent. Do not rewrite historical original fixtures to
match production. All independent and development csproj files directly linking
Beatmap.cs remain supported; shared syntax is deliberately kept in that file.

Original method dependencies: M1bc1 metadata sections, M28f5 loading, M28f7 object
parse, M28ff timing, M28ea postprocess, M28eb/M28ec stacks, M144a geometry/timing,
M2d62/M2d64 samplers, M2902 NewCombo, M48de/M48e0 ordinary factory. Their general
mode/default/malformed branches still require Work 1 inventory. E005 corrects
E004's historical Taiko/standard dispatch attribution; do not use E004 alone.
