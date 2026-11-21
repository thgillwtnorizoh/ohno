# Issue15 independent Work1 verification

Exact production22a403505f13461477343b9c6a251980716dd90f; starting main1638dfd9.
This new versioned suite adapts the frozen Work1 Issue13 consumer verifier and adds
separately authored adversaries in Checks/Extra.cs. Work2's development harness is
not the oracle. Historical source pins, manifests and runners remain unchanged.
The independent literal12-ID set plus original old dispositions determines each
new disposition, separately cross-checked against the accepted handoff.

```sh
git worktree add --detach /tmp/i15-exact 22a403505f13461477343b9c6a251980716dd90f
python re/campaigns/C001/near-spinner-qa/verify.py /path/dotnet /tmp/i15-exact /tmp/independent-i15
python re/campaigns/C001/near-spinner-qa/check_artifact.py /path/Issue15-Windows.zip
```

Expected1833/1833 independent component checks,59prepared/41refused,12exact gains,
0losses;81Issue9 +33Issue7 per-case contracts and8Issue11 message/type controls are
included in that count. No old1344/89/10/20 counts relabeled. Source/binary seals
verify actual linked source; original report13 static/input proof and original
heights remain theoretical. No production/internal-height reconstruction or zero claim.

Live CI extraction is in ci-live.json; actual downloaded ZIP/executable/source/
Windows1503packet/fixture/compiled-CRLF checks are in artifact-seal.json. These are
corroborated development/historical CI, not new independent assertion counts.
Application EXE was never executed. Synthetic WAV assets/recording channels are
component probes, not audible qualification. No owner charts or manual actions.

Initial review mistakenly demanded no new re files, encountering the approved
byte-preserving .gitattributes addition. initial-review-failure.log preserves it;
corrected check allows only that specific new file, no historical blob changes.
Initial packaged-manifest LF-byte expectation was corrected to newline-normalized
manifest identity; all actual .osu fixtures still have exact100 byte/hash checks.
No production/fixture expectation/source pin changed. No focused QA assertion failed.
