# Cumulative research record

Append-only. Historical entries and completed marks are preserved under `Instruction.md`.

## Expedition 01 — Reference Environment & Gameplay Entry Baseline

Date: 2026-10-26 (Asia/Bangkok). Owner: Work 1. Layer A.

**Objective:** Determine which build/environment/entry claims can be established from the supplied archive and available execution environment. Establish a reproducible baseline or explicitly document why original execution cannot be reproduced.

**Scope/non-goals:** Static input identity, environment inventory, proposed control/stress cases and capture checkpoints. No gameplay mechanics, UI reconstruction, skin/rendering work, proprietary code copying or reference-asset publication.

**Prior record/repository:** `https://github.com/thgillwtnorizoh/ohno`; GitHub get_repo returned ID 1431318446, default `main`, size 0. Contents API returned 404 with “This repository is empty.” `git ls-remote` returned no refs; local clone showed no commits. No existing `re`, contexts, expedition marks, AGENTS.md or Work 2 components were available. Supplied contract was read completely and copied byte-for-byte to `re/Instruction.md`. No history was edited. Dedicated research publication is intended; final handoff identifies actual verified remote revision. No remote outcome is assumed here.

**Inputs inspected:** all 18 ZIP entries and `osu!.cfg`; original exe and managed metadata; full supplied Instruction.md. ZIP SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`, 78,546,961 bytes. Contract SHA-256 `bf9952eec9d375a8e2fb661142390c2feff6570214f4ce09aa8063d6f34274f9`. All member fingerprints in `evidence/01/archive-audit.json`.

**Reference/build:** CONFIRMED static metadata: stream text Stable40; EXE file/product version 1.3.3.8; assembly 0.0.0.0; I386/PE32, CLR flags 3, metadata v4.0.30319. All 16 self-declared MD5s agree; certificate directory exists. UNRESOLVED dated official build and clean/modified status. No signature trust or independent official hash comparison was performed. Filename/module role assumptions were not used as runtime evidence.

**Environment:** Available host is Linux x86_64, kernel 6.18.55, no Wine/dotnet/PowerShell/display/GPU device. ffmpeg available but no reference capture source established. See `host-capabilities.json`. Reference Windows version/GPU/driver/resolution/DPI/refresh/window mode/settings are UNRESOLVED. Config in archive contains updater values only. No runtime execution/capture occurred.

**Competing explanations/experiment:** H1 fixed visible checkpoint sequence/durations; H2 same sequence with first-load timing difference; H3 setup/dependency blocker. Proposed smallest experiment: offline disposable Windows copy, actual version/settings/hash evidence, one original fixture run captured from Play through exit; if successful, three fresh-process repetitions; isolate a warm repeat if needed. Record frame PTS/input times, blockers and pre/post hashes. Full procedure and C0–C8 observation table in `reference-environment.md`. All runtime checkpoints currently UNRESOLVED.

**Control/stress:** No `.osu`, `.osz` or `.osr` in archive. Wrote independent deterministic generator for three circles at 2000/3000/4000 ms plus six-second PCM silence; output is a proposed control, not stable-validated. Expected SHA-256 map `dc4112acfcb0e7f345c6350a0cf1640d74f7acdc6eb2ae77047023b09e330541`; WAV `c263760de082d12fd4a983296b361d1c29c0567f354b01a44149fe24e9609977`. Aspire selection/identity/hash UNRESOLVED because none supplied. No third-party assets imported or published.

**Changes:** Added `re/Instruction.md`, `re/reference-environment.md`, `re/01-reference-environment-and-gameplay-entry.md`, this new `re/contexts.md`, `re/tools/audit_reference.py`, `re/tools/make_entry_control.py`, `re/tools/collect_windows_baseline.ps1`, and `re/evidence/01/{archive-audit,host-capabilities,control-input-hashes}.json`. Original archive and attachment untouched. Reference binaries only inspected privately outside repo. No gameplay implementation or Work 2 changes.

**Tests actually run:**

- `python re/tools/audit_reference.py ../project_sources/01-osubackup.zip > re/evidence/01/archive-audit.json`: PASS; all ZIP CRCs, 18 member fingerprints; 16/16 config MD5 matches. pefile 2024.8.26 and dnfile 0.18.0.
- `python re/tools/make_entry_control.py ../entry-control-a` and same with `../entry-control-b`: PASS, identical map/WAV bytes and hash JSON. Wave reader verified mono/16-bit/44100 Hz/264600 frames of silence; source text contains three declared circles. This is fixture validation only, not beatmap parser equivalence.
- Re-running generator in existing output directory: correctly refused overwrite; original output hashes unchanged.
- `python -m py_compile re/tools/audit_reference.py re/tools/make_entry_control.py`: PASS (host Python).
- Repeated full audit JSON generation/byte comparison: PASS, deterministic result.
- Host tool/path/environment probe: completed, missing capabilities recorded in evidence JSON.
- `git diff --cached --check` and staged diff review: PASS before publication. Only research, original tooling, contract and non-asset metadata intended for GitHub.

**Not run/limitations:** Windows collector (no PowerShell), Authenticode validity/trust, native original launch, fixture import, input injection, gameplay capture, transition timing, Aspire comparison, rendering/behavioural equivalence tests. The PowerShell helper is unvalidated and may need adaptation on target Windows. Silence prevents audible synchronization measurement. Capture alone cannot establish internal states or sub-frame timing. Self-consistent config cannot prove provenance. Environment/config must be measured rather than inferred from defaults.

**Findings:** CONFIRMED static input identity and local execution limitation. RECONSTRUCTED experiment design only. HYPOTHESIS timing/entry alternatives untested. UNRESOLVED all original gameplay-entry/exit state transitions and runtime algorithms. No numerical gameplay rules were inferred.

**Next useful experiment:** Obtain actual version/environment/settings evidence and one original control entry-to-exit recording on Windows, preserve hashes/capture/logs, then repeat before inferring a state machine. Select a concrete Aspire input only when legitimately available. Any later findings belong in a new appended entry/mark; do not silently revise this baseline.

**Authorised exceptions:** None.
