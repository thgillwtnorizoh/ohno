# E046 initial export-path failure

The first full new checker failed after byte auditing and finite comparisons,
while serializing source provenance. The same path conversion also broke its
failure handler. `failure-01.json` preserves all then-current mathematical and
checker sources, the executed command/phase, error and original packet hash;
`initial-packet.json` preserves that packet. No mathematical assertion failed.

The failure is reproducible without the archive or any original execution:

```python
from pathlib import Path
root = Path('ohno').resolve()
Path('ohno/re/tools/check_rhythm_visibility.py').relative_to(root)  # ValueError
Path('ohno/re/tools/check_rhythm_visibility.py').resolve().relative_to(root)
# Corrected result: re/tools/check_rhythm_visibility.py (platform separators)
```

Only provenance path resolution changed. Seven explicit original branch-target
assertions were also added; no numerical expectation was weakened. The corrected
full gate passes88 controls. E044/E045 historical failures and files are untouched.
