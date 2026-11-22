Issue21 independent QA attempts, 2026-11-22. No production, original fixture,
historical expectation or old report was changed.

1. First focused run: **5316 passed /233 failed /5549 total**. `first/` preserves
   every QA source and the complete results/log. Incorrect new QA expectations:
   audit status was named no-audio-declared rather than audio-not-declared;
   Rank uses an eight-digit score and SessionIdentity rather than record reference;
   sound event count cannot be lower-bounded by traversals (E039 starts at index0);
   source P with >3 controls falls back to Bezier, not Linear;
   a 300-byte custom WAV uses unsupported native short-sample creation;
   original CS5 step is3.2013118267059326, not3.2;
   the nonmonotone206 probe initially extended raw G190 rather than using raw G206.
   Corrected test data/expectations use the existing original reports and accepted
   component contracts. The source-P fence now also tests a real two-control
   Linear fallback and a separate normalized65 refusal. A valid2048-byte PCM
   file exercises actual map-byte ownership/cancellation.
2. Second focused run: **5665 passed /2 failed /5667 total**. `second/` preserves
   source and results/log. The new exact E039 callback oracle initialized every
   parent at0. Later parents have already been visited before their head time,
   which resets their index to-1. The two later parents of the19/27/31 map exposed
   this distinction. The final test predicts the full callback sequence with the
   correct fresh/previously-visited state. Final **5667/5667**; no production edit.
3. Provenance runner first selected the last matching Program.cs anywhere under
   reconstructed/, rather than the uniquely matching compiled checksum. The
   preserved source-hash-verifier.log records the failure after the focused suite
   passed. Corrected code requires exactly one matching source hash per metadata
   entry, excludes only two generated compiler documents, and seals the linked
   production DLL. No focused C# rerun was needed for this Python-only correction.
4. Early review mistakenly rejected new intervening campaign research as an old
   source edit; review-scope.log preserved. Review now checks unchanged historical
   source directories and all1876 pre-existing research file bytes/append-only
   prefixes individually. A later check mistakenly expected a cases array in the
   WPF packet; its actual fields are prepared/added/checks. The corrected review
   validates112 prepared owners,564 assertions and13350 segments. Another interim
   clean-tree assertion encountered only compiler bin/obj output; these generated
   directories are locally excluded. No tracked production files changed.
5. Initial QA09 invocation omitted the historical PowerShell-generated pack
   arguments and exited134 before assertions; QA09-missing-args.log and
   retained-invocation.log preserved. Downloaded PowerShell7.4.6 from Microsoft's
   Debian package endpoint; original unmodified Run-QA09–15 scripts then executed
   with their proper contracts. Successful earlier suites were reused, not rerun.
6. Actual unmodified QA14 on Linux: **234 passed /3 failed /237 total**. These
   failures are preserved in results/QA14.json.gz.b64 and retained-QA14.log.gz.b64:
   C/reparse-ancestry needs cmd.exe; I/windows-path-case expects Windows casing;
   I/canonical-unc-rejected expects Windows UNC semantics. Native Windows execution
   here is NOT RUN. Existing exact-source run84 independently audited2312/2312
   Windows historical results includes QA14 237/237. The Linux result is never
   relabeled PASS and no historical test was skipped, weakened or modified.
7. The unchanged canonical development suite writes a new untracked
   development-results.json in its checkout. Its generated report was preserved
   as results/retained-canonical-development-results.json (possibly packed), then
   moved out of the immutable worktree. Tracked source remained unchanged.
8. CLI Git write credentials were unavailable (dry-run failed to obtain username).
   Publication uses the authorized GitHub connector with current-head lease and
   nonforced ref update; remote readback is mandatory. No permission bypass.

Large traces are losslessly gzip/base64 packed; packed-evidence.json carries the
original-byte hashes. Do not sum intermediate focused runs into final pass totals.
