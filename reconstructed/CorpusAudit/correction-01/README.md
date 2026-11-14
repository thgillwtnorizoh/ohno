# Issue #5 diagnostic correction — C01

Authority: re/campaigns/C001/verification/10-corpus-audit-diagnostic.md at
6e959fef9cdf62e230db79c42a7b43f4bc2b6375; defective tool8c5ba835.

CorpusAudit now classifies mutation only from observed production fingerprint
transitions or nonnull captured/prepared hash disagreement. Catalog revision
verification and LoadCanonicalGameplay remain unchanged. After a canonical failure,
a new bounded ChartFingerprint read observes the current revision; a changed revision
wins, a missing/unreadable current file is classified by its typed failure, otherwise
the original rejection is classified. If the unchanged captured document already
failed production metadata preparation, canonical InvalidDataException is
malformed-input/metadata-syntax without inspecting the offending value. Generic
Failure no longer emits changed-input from any exception message.

All earlier development assertions remain, with20 discriminating additions:
immutable numeric strings (including exact-looking catalog messages), preserved
bytes/hash/stamp/privacy/determinism/denominators, forged exception rejection,
actual timestamp-only mutation and actual mutation to malformed metadata.
Local259/259PASS; exact corrected Windows/Linux CI evidence follows publication.
Gameplay production, offset, sliders, R17 and independent probe/fixtures unchanged.

## Unchanged independent probe limitation

The mandated probe hardcodes `sourceCommit=='8c5ba835...' and sourceState=='clean'`.
Its file and fixtures are preserved byte-for-byte. An honestly stamped corrected
build cannot pass this old-SHA assertion. Actual unchanged execution yields9/10PASS:
all nine functional/read-only/privacy/determinism/denominator assertions pass;
only `exact clean source identity` fails. The previous failure (metadata phrase
impersonating mutation) now passes. This is a reproducible verification blocker,
not a claimed10/10 result. Do not forge the stamp, modify the probe or weaken it.
Work1 must explicitly parameterize/authorize a corrected-source provenance check
before that all-ten acceptance requirement can be met.

```sh
dotnet run --project reconstructed/CorpusAuditChecks/CorpusAuditChecks.csproj -c Release -- . /tmp/audit-c01
python re/campaigns/C001/corpus-audit-qa/probe.py /path/dotnet reconstructed/CorpusAudit/bin/Release/net8.0/CorpusAudit.dll /tmp/audit-c01-probe.json
```

The latter currently returns1 only because of the pinned source-identity assertion.
Transient mutation restored before observation cannot be proved from exception
text and is not invented as changed-input. Scanning still uses best-effort finite
filesystem snapshots; no hostile atomicity guarantee. Broader taxonomy remains
conservative engineering, not original-runtime evidence. Owner corpus NOT RUN.
