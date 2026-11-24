# Work2 handoff — C001-I23-ORDER-01

**Source-supported connected incompatibility; production correction pending.**
Work1 did not modify production. Tested functional source:
`5dda483348e21e79c8354bf6ea322d50b7fb3e72`.

The decisive missing edge is **M28f5:007f -> virtual M28be(false,true,true) ->
M28be:01b3 -> M3b66(true)**. It sorts the selected map's timing list before the
runtime object parser and slider postpass. M224d's later forced sort is not the
first initialization sort. Graph/field identity and ordinary-slot evidence are in
graph.md and static-evidence.json. Head sample banks are constructed after the
early sort; they are not encounter-order snapshots on this successful path.

Minimal unchanged Issue22 fixture: `timing-state/fixtures/equal-inherited-base/control.osu`.
Its raw timing rows are base(time0,beat500,bank1,index1,volume40), then inherited
(time1000,beat-50,bank2,index2,volume70), then base(time1000,beat600,bank3,index1,
volume60). Comparator order is source positions **[0,2,1]**, with no equal-key
stability ambiguity. Start1000, length400, multiplier1, tickrate1, one traversal.

| Observation | Actual production | Original-static initialized prediction |
|---|---|---|
| Prepared speed |166.66666666666669|333.33333333333337|
| Stored end |3399|2200|
| Checkpoint dates |1600,2200,2800,3364|1600,2164|
| Head bank/index/volume |3/1/60|2/2/70|
| First tick bank/index/volume |3/1/60|2/2/70|
| Finish bank/index/volume |3/1/60|2/2/70|
| Recorded head resource / effective volume |drum-hitnormal /48|soft-hitnormal2 /56|
| Recorded tick resource / volume |drum-slidertick /60|soft-slidertick2 /70|
| Recorded finish resource / effective volume |drum-hitnormal /48|soft-hitnormal2 /56|

All three actual requests use real production handles loaded from generated PCM;
recording is not audible/native execution. The fixture is already admitted by
production's monotone v14 continuing profile. A scratch transformed-order copy
loaded through the unchanged production loader agrees with independent early-sort
rate/end/date/sample predictions. It is an explicit operand control, not a fix.

Required correction semantics:

1. Preserve source bytes, OsuDocument entries, raw encounter order and SourceOrder
   provenance. Original initialization does not authorize rewriting canonical raw
   input, charts, history or the sealed fixtures.
2. Establish the ordinary initialized timing view **before** object sample-bank
   construction and geometry/rate/tick preparation. Use original raw Double-time
   ascending comparison, then base-before-inherited at equal time. Do not replace
   M1be3's own truncated-time/index selection or M1be6's raw-time lookup.
3. Feed the initialized state consistently to slider preparation, head factory,
   discrete sample consumers and global continuing initialization. A copy is an
   implementation option only if its stage/visibility matches the supported
   original behavior; sorting only the continuing copy after preparation is not
   sufficient. The later rescan must not silently recompute geometry.
4. Retain current public/legacy/profile/resource protections. In particular, an
   ordered prepared view must not silently bypass raw out-of-order continuing
   admission limits. No loading/profile expansion, B/P/C geometry change,
   score/HP/R17 adjustment or short-column default work is authorized here.
5. Do not add SourceOrder as a claimed original tie-breaker. Equal time/equal
   base flag compares equal; original List.Sort stability is not established.
   Both ambiguous controls remain explicitly conditional/excluded from exact
   ordering equivalence until separately supported.

Separate gates for an authorized Work2 implementation:

- Development: raw/prepared/state ownership and serialization; the minimal exact
  outputs above; all12 unambiguous reused controls and the two ambiguous domains;
  geometry, first sample bank, head/tick/edge/finish and multi-parent integration.
- Regression: unchanged semantic selectors/fractional conversion, old-version and
  profile fences, retained sample families, L96/1–32 and B/P/C paths, score/health
  and R17 behavior. Preserve Issue22's supplied-list component evidence; new
  initialization expectations must be separate, with justified supersession.
- Windows: exact-source build and real application/input/audio lifecycle gates at
  the implementation milestone; disclose absent native original qualification.
- Work1 independent QA: fresh correction source, original-derived outputs and
  connected ownership/sample evidence. Development tests are not independent QA.
- Owner: acceptance and original runtime Issue4 remain separate and OPEN.

The narrow next actor is Work2 under Headquarters assignment, followed by Work1
verification. This handoff is neither an implementation authorization nor acceptance.
