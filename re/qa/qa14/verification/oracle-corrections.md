# QA14 local setup, fixture and oracle history

These records concern **new Work1 QA14 tooling**. They do not alter R16's history:
its source19bb644e/run43153555528 and publicationabd9dbf/run43154351392 both
passed their first development/publication gates. Original E043, R16 development
assertions and every historical QA remain unchanged.

The lossless packet registry records exact byte lengths and SHA256 for compressed
logs/results. Every initial failed source snapshot is retained in its own folder.

| Local execution | Exact retained evidence | Disposition |
|---|---|---|
| Initial headless dependency setup | `initial-headless-local/` | NU1301 while the isolated CLI home could not reach NuGet. Zero assertions. Reused already installed package cache; no SDK/runtime installation. |
| First executed headless, partial | `failure-headless-local-2/` | Mixed-separator expected path, local file-isolation timeout/junction denial, then a Windows-invalid colon in a case-directory name. Initial log/source/exit retained; no complete result packet was emitted. |
| Initial Windows compilation | `initial-windows-local/` | CS0119: the test helper named Button shadowed the WPF type in Button.ClickEvent. Zero Windows assertions. Qualified the WPF event token; production unchanged. |
| Next complete headless | `failure-headless-local-3/` |225/226. Only the new keyboard source audit failed because it assumed LF immediately after `if (selectionVisible)` in a CRLF checkout. |
| First complete corrected headless | `first-complete-local/` |231/231, exact result SHA256`d7de6ec2c68bca1e7e72583d2e93f6b32e2aca44c19e5a7423923f8b496532e1`. Preserved before additional async controls. |
| Corrected local Windows | `local-windows-results.json` |30/30 actual unshown components on.NET8.0.17. No Window/show/media opening. |
| Strengthened final subsets | `local-selection-results.json`, `local-headless-results.json` |26 selection controls and15 race controls pass; final exact-source full count belongs to Windows CI. Repeated/subset runs add no independent cases. |

## Path expectation proof

The input to C/external-path was constructed with `Path.Combine(work,"external/a.osu")`.
Its string retained a forward slash in the final component. R16's declared policy
uses `Path.GetFullPath`, yielding the same actual file with Windows backslashes.
The initial expected string was the unnormalized input. The corrected expectation
is independently `Path.GetFullPath(input)`, while exact fingerprint/path/reference
assertions remain. This fixes a wrong oracle, not the canonical-path implementation.

## Case-directory and record hardening

One test ID contains the literal supplied compatibility tag
`osu-stable:E043:cache-v2`. That is a valid operand but an invalid Windows directory
component. It crashed the initial driver outside the per-case catch. The final
driver hashes IDs for owned scratch names. An initial duplicate C/empty ID was
also split into C/empty and C/invalid-empty; the final driver explicitly rejects
duplicate IDs. Initial partial logs are not represented as a complete named gate.

Large count tests still retain their expected10000/4096/32/50000 engineering
limits. The local file-isolated process timed out on the10001-chart fixture and
denied junction creation. Normal Windows filesystem execution passed these same
assertions. The timeout allowance is five minutes per control; no admission
predicate or expected catalog count was weakened. None of these limits is claimed
as an original osu!stable limit.

## Source-audit proof

The initial audit searched the entire host text for the LF-specific sequence
`if (selectionVisible)\n`. The unchanged host's CRLF checkout does not contain
that sequence. The corrected audit isolates KeyPressed through HandleBrowserKey,
then checks that the browser branch precedes the Rank/gameplay branch and retains
every required command/skip/back/retry assertion. It does not normalize or edit
production bytes, and it is still classified SOURCE INSPECTION.

## Additional finite coverage and unchanged production

The231-case complete pass was reviewed before CI. An empty-return test's overly
broad cancelled-refresh name was corrected to S/empty-return-no-replacement.
Three new cases actually exercise live selection after a blocked row rebuild,
restoration after cancellation of a blocked refresh, and an awaited preparation
throwing after cancellation. The first successful packet remains intact; the
final gate contains234 headless controls, not231 plus repeated runs.

Matched-session history assertions were also made compact before that complete
pass: every action compares digests of complete ordered histories, while full
history and meaningful snapshots are retained once per case. This avoids
quadratic evidence duplication without weakening equality.

The unpushed local commitcc82381 accidentally included generated QA14 bin/obj
outputs. Commitb795dcb removed them from the publication tree using index-only
removal, added QA14-local ignore rules and exact-byte fixture attributes, and
preserved the earlier commit. No force update or history rewrite occurred; no
compiled output is present in the final tree. This is a staging correction,
not an executed R16 behavior failure.

## Final packet-checker correction

The first post-CI packet check passed original relationships, preservation, all
264 independent outcomes and the66-row matrix, then failed a log-template
assumption: it expected the text `R16 DEVELOPMENT: 52/52`. The unchanged R16
runner actually prints `R16 DEVELOPMENT: 52 groups passed`; its Windows runner
uses12/12. `packet-checker-initial.py` and the original failed checker log are
retained. The corrected packet checker requires the actual literal and separately
requires the52 ordered individual `PASS R16 NN` lines. No development assertion,
production code, independent case or executed gate changed; this is an evidence
reader correction after an already successful Windows gate.
