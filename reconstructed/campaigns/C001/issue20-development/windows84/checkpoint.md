# 1. Result

Project / Campaign / Actor / Assignment: **ohno / C001 / Work 2 / Issue #20 — Final Evidence Reconciliation**.
Status: **VERIFIED CHECKPOINT — final development checkpoint**. Issue #20, C001, subsystem and original-runtime Issue #4 remain **OPEN**. This is not owner acceptance or new-source independent QA.

| Revision / authority | Exact record |
|---|---|
| Reconciliation starting `main` | `5dda483348e21e79c8354bf6ea322d50b7fb3e72`, verified against remote before recovery and again before publication |
| Original Issue #20 starting `main` | `131c78835594b5814cd3d6ee3d9435fb9ae26ee1` |
| Accepted prior gameplay production | `6a152648c72773eb33d37f8ac01f18ca35ab1f41` |
| Introducing implementation | `585c624b2675e534dfaacf9c497120099f61dc6d` |
| Exact Windows-tested and packaged source | `5dda483348e21e79c8354bf6ea322d50b7fb3e72` |
| Published reconciliation SHA | Evidence-only commit containing this packet; its exact SHA is recorded in the final Issue #20 comment. It is a descendant of the tested source, not a new compiled-source claim. |
| Authority | Owner's evidence-only reconciliation assignment; Issue #20; UEP-001 Revision 2.1; unchanged Issue #19 handoff/static authority |

Objective: recover the existing implementation checkpoint, verify its successful Windows delivery, reconcile persistent evidence, and publish the complete development checkpoint. **No implementation, test, workflow, original research or fixture changes; no corpus scan; no history rewrite; no authorization exception.**

Existing supported profile: canonical ordinary Mode 0, source marker `L`, formats v5–v14, at most 96 normalized controls. The explicit internal source capability leaves public/default geometry, historical string loading and source B/P/C at 64 controls; canonical 1–32 and historical/public 1–16 traversal profiles remain unchanged. Existing geometry, timing, stacking, scheduling, score, fresh health/NewCombo, input/session and real sample/resource ownership integration are retained. The 96 ceiling is ENGINEERING, not an original-game maximum.

## Windows delivery and artifact verification

- [Windows run #84](https://github.com/thgillwtnorizoh/ohno/actions/runs/45002466140), run ID `45002466140`, attempt 1, completed **SUCCESS** on 2026-11-22; [job `137146777470`](https://github.com/thgillwtnorizoh/ohno/actions/runs/45002466140/job/137146777470), **54/54 recorded steps successful**. Step numbers are 1–52, 104 and 105; the highest ordinal is not the step count.
- Windows Server 2022, 10.0.20348, windows-2022 image 20261116.370.1; .NET SDK **8.0.428**, runtime **8.0.34**. Release compilation and self-contained win-x64 single-file publication succeeded, **0 compiler warnings / 0 errors**. Upload emitted two Node deprecation notices (`DEP0040`, `DEP0169`), retained separately from compiler warnings.
- [Artifact `14746253714`](https://github.com/thgillwtnorizoh/ohno/actions/runs/45002466140/artifacts/14746253714), `ohno-C001-5dda483348e21e79c8354bf6ea322d50b7fb3e72`, **68,714,855 bytes**, downloaded and verified; expiration recorded as 2027-02-20. ZIP SHA-256: `91c41682aaeab1ba18e311bbebbeeb4307752098dd33bf1059c71a11205348cb`.
- `Ohno.CircleGame.exe`: **154,045,712 bytes**, PE x64, SHA-256 `409e7fc13a92047e1eff0a56bf681c351f5616eca9e4b8d3be029f5e866238d7`. `SOURCE-COMMIT.txt` and embedded commit match the exact tested source. The 443-entry bundle includes .NETCore and WindowsDesktop 8.0.34 frameworks and WPF dependencies.
- All **274 packaged fixture hashes** match unchanged manifests (168 control-count + 106 traversal). All **168 individual dispositions** match original fixture identities and expected admissions: **112 prepared / 56 excluded / +74 newly admitted / zero losses of the prior 38 / 112 audio consumers**. Catalog, direct-load, legacy errors, raw/normalized counts, sample errors and CorpusAudit classification remain individually recorded.
- Actual WPF-linked checks: **564/564**, **112 owners**, **13,350 actual BuildPath segments**. Frozen path construction, finite endpoints, unfilled/open/stroked constructor data, registered session/health identity and actual compiled Beatmap portable-PDB checksum passed. This is managed construction without a shown Window or native rasterization claim.
- Download verification matched source hashes (including Windows CRLF compilation), the linked assembly's SHA-256 and PDB identity, and **six method bodies** between the tested WPF assembly and the delivered executable's bundled managed assembly, including checked movement multiplication. Whole assembly hashes differ between build and publish; six-method equality is the verified comparison, not whole-DLL identity.

## Applicable test counts

These are recovered results from run #84, not tests rerun during reconciliation. Keep suite denominators separate.

| Current suite | Passed / total |
|---|---:|
| Issue #20 headless development | 5430 / 5430 |
| Issue #20 WPF-linked development | 564 / 564 |
| C001 canonical development | 264 / 264 |
| Perfect-circle development | 299 / 299 |
| Catmull development | 4375 / 4375 |
| Retained finite stacking heads / integration | 43 / 43; 394 / 394 |
| Issue #18 traversal development | 3974 / 3974 |
| Issue #15 near-spinner development | 1503 / 1503 |
| Issue #9 legacy stacking/timing development | 801 / 801 |
| Issue #7 versioned spinner development | 656 / 656 |
| Issue #11 / CorpusAudit development | 295 / 295; 0 platform controls NOT RUN |
| Retained finite original-contract predictions | 106 / 106 fields; 4 / 4 engineering controls; 0 gaps; 10 diagnostic-only probes excluded from pass counts |

| Historical QA summary | Passed / total |
|---|---:|
| QA15 headless / Windows | 95 / 95; 39 / 39 |
| QA01 headless components | 90 / 90 |
| QA03 sliders | 56 / 56 |
| QA04 geometry | 43 / 43 |
| QA05 spinner | 66 / 66 |
| QA06 accounting | 99 / 99 |
| QA07 health | 159 / 159 |
| QA08 NewCombo | 115 / 115 |
| QA09 hitsounds | 234 / 234 |
| QA10 slider sounds | 229 / 229 |
| QA11 continuous / Windows empty | 247 / 247; 6 / 6 |
| QA12 headless / Windows | 274 / 274; 12 / 12 |
| QA13 headless / Windows | 257 / 257; 24 / 24 |
| QA14 headless / Windows | 237 / 237; 30 / 30 |
| **19 historical summaries total** | **2312 / 2312; 0 failed** |

All earlier development/smoke stages also succeeded: R06 4; R07 8; R08 13, C01 4, C02 14; R09 15; R10 23; R11 32; R12 40 (including retained S25/QA10 M16 refusal controls); R13 54 plus Windows empty-resource 1 and C01 6; R14 58 plus Windows capability 1 and Q01 23; R15 79 plus Windows 8; R16 52 plus Windows 12; R17 82 plus Windows 12 and 89,878 finite scalar comparisons. These historical group counts include their explicitly labeled source-inspection groups and are not added to the 2,312 independent historical QA denominator. Exact log wording is preserved in `all-test-summaries.log`.

Prior Linux development evidence remains **5429/5429**, SDK 8.0.427/runtime 8.0.33; retained Linux 264/299/4375/3973/1502/801/656/295 separately passed. The Windows headless denominator includes the additional clean compiled-source stamp; WPF 564 is a separate suite. Existing E005/E006/E032 static-checker passes and sealed fixture checks are retained, not rerun or relabeled as original runtime.

Reconciliation verification actually run on Linux: unchanged `check_artifact.py`, with `dnfile` and `dncil`, against the downloaded ZIP, decoded job log and live CI metadata. Result: **PASS exact Windows ZIP/exe/bundle/PDB/6 methods/274 fixture hashes/168 dispositions; 54 steps; 5430 development; 564 WPF; 2312 historical** (`verification.log`). Additional evidence integrity checks confirm the compressed log round-trip, result packet, manifests, append-only changes and documentation-only path scope. No repeated build, regression execution or corpus scan was needed.

## Records and reproduction

Reconciled by append-only updates: `18-issue20-linear-controls.md`, `status.md`, `coverage.md`, `discrepancies.md`, `reconstructed/contexts.md`, and this development packet's README, attempts and dependency record. Preserved original pending statements as dated historical observations and the first exact Windows failure.

New evidence: `windows84-ci.json`, `windows84.log.gz.b64` (decoded UTF-8 log with LF line endings and BOM removed), `windows84/{artifact-seal.json,windows-results.json,windows-linked.json,windows-dispositions.csv,ci-summary.log,all-test-summaries.log,verification.log,checkpoint.md}` plus retained stacking/finite-contract result packets. The seal records packet/log/executable/source/assembly/method and all fixture hashes. Decoded normalized-log SHA-256: `92dd1d1b849b5af3674db3c1534bafb6ea3fa9fa4260954032873561fa5c12e0`.

From repository root, download the linked artifact as `/tmp/windows84.zip`, decode `windows84.log.gz.b64` using base64 then gzip into `/tmp/windows84.log`, install `dnfile` and `dncil`, then run:

```text
python reconstructed/campaigns/C001/issue20-development/check_artifact.py /tmp/windows84.zip /tmp/windows84.log reconstructed/campaigns/C001/issue20-development/windows84-ci.json /tmp/issue20-verification
```

The existing `verify.py` and `.github/workflows/r04-build.yml` preserve exact build/test commands for future reproduction; they were not executed again in this evidence-only task.

# 2. What they found

**VERIFIED COMPONENT:** successful exact-source Windows build, retained regressions, connected controls/owners, WPF managed construction and verified portable delivery close the previously pending Work 2 Windows development gate. Exact multi-parent 250,000-movement / 100,000-prepared-segment / 100,000-checkpoint maps, per-parent 4,096 checkpoint/overflow refusals, one-over cases, cooperative cancellation and allocated custom resource cleanup remain covered by the sealed headless results. Final prepared-segment budgeting retains the raw95-to-prepared60 trimmed final parent case. Position-query work bounds and measured WPF construction are evidence, not frame-time guarantees.

**CONFIRMED ORIGINAL:** reused authenticated Issue #19 static evidence (18 bodies, 39 historical matches, 748 sites, 15 anchors, 3 branches). **RECONSTRUCTED:** original-derived conversion/arithmetic/geometry chronology. **ENGINEERING:** canonical source-L96 admission, resource ceilings, diagnostic boundaries and cancellation observations. Source B/P/C and legacy/public profiles remain bounded at 64; P fallback does not acquire source-L privileges. No broader compatibility or owner-corpus recovery estimate follows.

**Preserved failure:** first Windows run `45001890246` at `585c624` passed 5430 headless and 2312 historical QA but failed the new WPF verifier: native flattening returned no figures for an unfilled StreamGeometry, followed by verifier indexing an empty array. Publication was skipped. Commit `5dda483` changed verifier readback to `PathGeometry.CreateFromGeometry`, checking retained managed constructor data with an explicit empty check; production renderer and gameplay stayed unchanged. First failed logs, original verifier, early compiler/test-data assumptions and CorpusAudit host-path failure remain in `attempts.md` and `first-*` files. Run #84's success does not erase or retroactively pass them.

**UNVERIFIED RUNTIME / NOT RUN:** original executable/CLR, original 17 E20 runtime cases, shown application/GUI/rasterized comparison, audible speakers/media playback, physical input and private owner corpus. No arbitrary thread-preemption guarantee. v5/v6 older continuing limitations remain; no B/P/C/legacy/version/mode/replay/mod/editor expansion. Historical star discrepancies and other campaign requirements remain open. No new original-behavior claim or independent Issue #20 verification is asserted.

Next bounded task / Actor / Dependencies: **Headquarters review of this exact-source development checkpoint, then separately authorized Work 1 independent new-source QA** against unchanged Issue #19 handoff/manifests and actual connected production. Any discrepancy returns to Work 2. No owner rescan or new manual action is requested by this reconciliation. **Subsystem: OPEN; Issue #20: OPEN; C001: OPEN; Issue #4: OPEN.** Owner acceptance is not asserted. Stop at publication of the final development checkpoint.

# 3. Pseudocode

Existing implementation chronology, unchanged by this reconciliation; RECONSTRUCTED arithmetic, ENGINEERING capability, VERIFIED COMPONENT development evidence:

```text
select96 only when canonical and literal source L; otherwise64
bound raw coordinate cells before conversion
convert Double -> Int32 -> Single; insert head on exact inequality
bound normalized controls with same internal capability
emit and adjust existing geometry unchanged
check final prepared map segments, checked movements and exact checkpoint plan
observe cancellation during parse, geometry, schedule and publication
publish actual canonical owners and use existing gameplay/sample consumers
```

Original-runtime equivalence remains UNVERIFIED; this task adds evidence only.
