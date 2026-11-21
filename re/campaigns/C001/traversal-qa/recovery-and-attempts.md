# Recovery and prior attempts

This task crossed a container restart. The first QA implementation and its raw
outputs were in `/tmp/i18-*`; the restart cleared those files before publication.
The following is a transcript-derived recovery record, not replacement raw logs
and not a claim that those lost binaries can be reproduced byte-for-byte.

- First compile: cancellation-aware sample provider did not implement the actual
  IDefaultSamples.Ui/Gameplay interface; corrected in QA, no production edit.
- Initial old-source characterization:719/720 PASS; checkpoint-limit family was
  incorrectly assumed to be slider-repeat-profile. A second incorrect proposed
  family slider-work-limit also failed. Actual existing family checkpoint-profile
  established from actual audit packet; corrected old-source run720/720.
- First new-source attempt asserted complete edge arrays even after malformed-set
  errors (two failures), then aborted on an overlapping generated checkpoint stress
  map. Stored malformed metadata can be partial by design; stress times corrected.
- Second:53608/53609 PASS. A generated fast-velocity control suppressed an extra
  checkpoint (97536 instead of assumed98304). Timing was changed so suppression
  distance is below tick spacing; a precise4096-per-parent control was established.
- Added assertion that historical fixtures named nonmonotone-final MUST be inverted
  failed twice:53615/53617 PASS. Existing original results already show their final
  two timestamps ordered. The names were not treated as expected behavior. A new
  genuine17-traversal control with raw/prepared206pixels has end19510, prior19480,
  adjusted final19474. Historical fixtures/expectations remain untouched.
- Final pre-restart run53704/53704 PASS;106 fixtures86/20,45new/0losses,82 audio/4stored
  errors. Also corroborated Windows ZIP/exe/bundle and reran retained suites.
- First retained CorpusAudit run could not launch its subprocess because dotnet
  was absent from PATH; setting DOTNET_HOST_PATH resolved it,295/295 PASS.

After restart, temporary files were absent. Remote main still727958c. Worktrees,
SDK, artifact, downloaded logs and QA now live in the persistent workspace.
The recovered independent suite groups all movement timestamp comparisons for a
parent into one check and groups discrete callback identity checks per consumer.
It therefore has a DIFFERENT assertion denominator. Its code and fresh results
are the published authoritative reproduction, not the lost53704-check binary.
All known failed attempts are disclosed here; lost raw data is explicitly NOT
RECOVERED. No counts from the first epoch are added to fresh counts, and no new
production behavior or historical expectation was changed to make checks pass.
