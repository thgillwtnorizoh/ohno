QA08 preliminary local oracle correction (before required net8 Windows gate).

Retained old harness SHA256 is in `initial-local-harness.sha256`; failed112/113
output and complete per-case inputs are in `initial-local-output.txt` and
`local-preflight-results.json`. Runtime: preinstalled net10.0/SDK10.0.402.

Original I07 suffix:

```csharp
for(int t=0;t<=6500;t+=100){a.Advance(t);b.Advance(t);Same(a,b);}Eq(a.JudgedCount,3);Eq(a.Ledger.NMiss,(ushort)3);
```

Corrected suffix keeps the same input/step comparison and asserts:

```csharp
Eq(a.JudgedCount,3);Eq(a.Ledger.NMiss,(ushort)2);Eq(a.Ledger.N50,(ushort)1);
Eq(a.Spinners[0].Required,2);Eq(a.Spinners[0].Result,Judgement.Hit50);
```

Proof: E026 M05fa uses modern count thresholds and integer division by4. OD5,
500ms gives preparedN2. Count0 < Max(0,2/4) is false; count0>2 and count0>0
are false, so raw256. Two other missed objects give NMiss2/N501. Added independent
C0/N2 and C0/N4 classifier controls distinguish the integer boundary.
No production change, no historical assertion change, no original-binary analysis.
