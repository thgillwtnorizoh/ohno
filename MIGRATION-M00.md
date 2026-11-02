# Migration M00 — Establish main

Date: 2026-11-02 (Asia/Ho_Chi_Minh). Assigned to Work 1. This is a Git migration,
not an original-software expedition or an implementation milestone.

## Inputs and ancestry audit

Fresh GitHub branch/repository reads and `git fetch origin` established:

| Source | Verified tip |
|---|---|
| E31 research | `92a1548808f75daf21bd3a10d988ef26f7562f0f` |
| R01 reconstruction | `da911d03820368b9871ab49f3fba04ee7476162c` |
| Existing default `work1/expedition-01-baseline` | `60346f73b28609456c8d69ae603b7b15edcae37e` |

There were 32 remote branches and no `main`. E02 is the common ancestor of E31
and R01. R01 has one unique commit, adding five paths under `reconstructed/`.
`git merge-tree --write-tree E31 R01` produced the conflict-free union tree
`b2d95b257a7733b2b10fee626a2937127ed0468d`.

Only the current default branch had commits outside the E31/R01 ancestry union:
the following ten deletion-only commits, each titled “Remove cancelled assignment
output at user request.” They deleted the original E01 files, leaving an empty
default-branch tree. They introduced no unique file additions or modifications.

| Commit | Deleted path |
|---|---|
| `f17a3d02acbf219c3d52861c5714e3986f7fd7da` | `re/contexts.md` |
| `8edc42e80ccd5a14c2deb143c23945518c031f69` | `re/01-reference-environment-and-gameplay-entry.md` |
| `bc4fb0c6bf3fb5a062cb570166d4a4072e4f527b` | `re/Instruction.md` |
| `c9ae8a9ffc5eae907fbffd11e45a1c75d929f9b3` | `re/evidence/01/archive-audit.json` |
| `63d4f0adf19b336ef338881b1cb672be4a583daf` | `re/evidence/01/control-input-hashes.json` |
| `d5e3c1e8dba821ed42e99bd4b642c7c483c00029` | `re/evidence/01/host-capabilities.json` |
| `5001cf7b923268f9b9b0da4747f847a295617318` | `re/reference-environment.md` |
| `63df2bc1a659dd8defcd9e6316ea5c8646a2449a` | `re/tools/audit_reference.py` |
| `1198cbb74a4bcbc352c255eddbf0b1a665f4f789` | `re/tools/collect_windows_baseline.ps1` |
| `60346f73b28609456c8d69ae603b7b15edcae37e` | `re/tools/make_entry_control.py` |

## Integration and preservation policy

`main` starts at verified E31. The integration commit has ordered parents E31,
R01 and the old default tip. Its tree retains E31 research and R01 reconstruction;
the old default's deletion effects are deliberately not applied, following M00's
explicit requirement to preserve E01–E31. The deletion commits themselves remain
ancestors, visible and unchanged. This is an explicit content resolution, not a
claim that those commits had no effects or that the old default was already an
ancestor of E31. All 32 historical branch tips become ancestors of `main`.

Preservation checks compare original Git blob IDs and modes, not just filenames:

- All 145 E31 files remain byte-identical except the permitted append to
  `re/contexts.md`; its complete 327,768-byte E31 prefix remains unchanged.
- All five R01 `reconstructed/` files, including its separate journal and fixtures,
  remain byte-identical with their original modes. R01 adds no `re/` changes.
- Earlier expedition reports, evidence, fixtures, tools, the standing contract
  and the complete E20 packet remain unchanged.
- `AGENTS.md` records the owner's permanent workstream, `main` and coordinated
  publication rules. This document records the audit. No historical report or
  implementation artifact is rewritten.
- Historical branches are retained. No force-push, rebase, squash or history
  rewriting is used. No unique published commit is left unresolved.

## Repository checks

Relevant checks are run on the combined tree before publication:

```text
PYTHONDONTWRITEBYTECODE=1 python -m unittest discover -s reconstructed/tests -v
PYTHONDONTWRITEBYTECODE=1 python re/tools/check_<name>.py re/evidence/<number>/<file>.json
PYTHONDONTWRITEBYTECODE=1 python re/validation/20/check_package.py --archive /private/osubackup.zip
git diff --cached --check
git fsck --full --no-reflogs
```

The second command denotes each of the 29 existing research checkers E02–19 and
E21–31: all passed. R01's 12 tests, including 22 static-contract fixture cases,
passed. In-memory Python compilation passed for 65 files. Git integrity checking
passed; it reports one existing dangling local E31 preparation commit, not
corruption or missing published history. Whitespace and preservation checks
passed. These are repository/implementation checks, not original-game results.

The additional E20 package-check attempt stopped at
`ModuleNotFoundError: No module named 'dnfile'` before full verification. No
environment dependency installation was performed for this repository-only
migration. Its entire tracked packet and fixtures are instead confirmed unchanged
by blob/mode comparison with E31. This attempt is not reported as a passing E20
package check. All 17 original-runtime cases remain NOT RUN.

No osu!stable, OBS, debugger, Windows desktop or manual gameplay is used. No new
research extraction, gameplay implementation or unrelated infrastructure work
is performed. E20 remains incomplete.

## Default branch and resumption

Remote commit parents, complete tree/blob modes and historical branch heads are
verified after publication. The publication response gives the resulting `main`
SHA; it is not inserted into its own commit.

The connected GitHub interface provides branch/tree/commit operations and GET
repository metadata, but exposes no repository-settings update operation. The
initial metadata reports repository admin permissions; that is not proof this
interface can perform an administration PATCH. No permission denial is invented.
If the final verification still reports the old default, the owner must execute:

```sh
gh api --method PATCH repos/thgillwtnorizoh/ohno -f default_branch=main
gh repo view thgillwtnorizoh/ohno --json defaultBranchRef
```

Both workstreams can use verified `main` explicitly once published, coordinating
one publisher at a time. The remaining default-setting action affects implicit
GitHub/clone branch selection. Future work follows `AGENTS.md`; no new expedition
or milestone starts as part of this migration.
