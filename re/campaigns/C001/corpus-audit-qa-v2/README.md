# CorpusAudit independent verification version 2

See ../verification/11-corpus-audit-correction.md for scope, exact source,
provenance, historical comparison and limits. Run:

```sh
python re/campaigns/C001/corpus-audit-qa-v2/verify.py /path/to/dotnet /path/to/clean-2369b406-checkout /tmp/audit-v2-results
```

Requires Python 3, Git and .NET 8. Enforces exact corrected source and all eight
linked hashes, clean state, actual binary linkage, 10 profile assertions and 24
scan assertions. No arbitrary source overrides. Historical corpus-audit-qa probe
and fixtures are reused unchanged. Only temporary synthetic files are changed by
mutation tests. No assets or owner charts are required.

scan-initial-results.json retains the 22/23 initial new-harness run, explained in
the report. Final scan-results.json is 24/24. Historical evidence remains intact.
