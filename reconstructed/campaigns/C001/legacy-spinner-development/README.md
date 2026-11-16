# Issue9 blocked candidate evidence

See ../13-issue9-integration-blocker.md for mandatory checkpoint, scope and commands.
Main production is unchanged. candidate.bundle.b64 is a base64 thin Git bundle of
exact tested candidate2c212abd963cdc5473d72d06bfc4207f74b56ca7 based onf9cce9e.
It contains both production changes and separately versioned new tests; candidate.patch
provides readable equivalent changes. Candidate is NOT integrated or Windows-qualified.
Use a disposable detached worktree; do not apply the patch to main as an accepted fix.
seal.json distinguishes source/binary/artifact hashes and NOT RUN gates.
exact-results.json: new801PASS/66admissions/15exclusions, not original775/548 QA.
exact-qa14.json/log: unchanged filtered control0PASS/1FAIL; not full2309 regression.
retained-issue7-failure.log: runner aborts at first expanded-profile assertion.
Initial build/invocation failures and preliminary run packets are retained separately.
No original fixture or historical checker was edited. Work2 stops at reproducible
acceptance dependency; Headquarters disposition needed, no owner manual action.
