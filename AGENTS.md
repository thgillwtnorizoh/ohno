# Project ohno — canonical work rules

Read the standing contract in `re/Instruction.md` and the relevant append-only
research/implementation journals before changing project work.

## Work ownership

- **Work 1:** original-software research, automated testing, original-behavior
  verification and QA.
- **Work 2:** implementation of the human-playable reconstruction.
- Keep research under `re/` and implementation under `reconstructed/`.
- Preserve historical reports, evidence, fixtures and journal entries. New
  findings do not silently replace earlier records.

## Canonical branch and publication

- Both workstreams use **`main`** as the canonical branch.
- Fetch current remote refs and start from the latest verified `origin/main`.
- Coordinate publication: only one workstream publishes at a time. Verify the
  expected remote head before updating it; if it changed, fetch and integrate
  that work before publishing. Never overwrite concurrent work.
- Do not create permanent expedition or milestone branches. Use a temporary
  branch only when repository protections require it, then merge it promptly.
- Do not force-push, rewrite published history or delete historical branches.
- Historical reports describing older branch arrangements are preserved records;
  these M00 publication rules supersede those arrangements for future work.

## Evidence boundary

E20 is preparation-only and incomplete. All 17 original-runtime cases remain
**NOT RUN — WINDOWS HOST REQUIRED**. Repository or implementation tests do not
establish original-game behavior. Runtime work requires the owner's authorized
environment and an appropriate assigned task; do not assume those capabilities.

Migration M00 authorizes repository consolidation only. It starts no new
expedition or implementation milestone. See `MIGRATION-M00.md` for provenance.
