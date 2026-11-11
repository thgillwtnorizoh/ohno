# Preserved E042 R30 conflict

**CONFIRMED STATIC authority:** published E042 MethodDef `0x06002b47`,
owner `0x0200059e`, signature `2000118904`, original body SHA256
`612f291e51c2668a73fbc0a9425e28f9b00d2bc4475322c25bd18ec2f103b267`.
No original executable, CLR method or native call was executed for QA13.

The unchanged historical R30 prediction is:

> First X2; second A4 for n300=9,nMiss=1 under binary32 projection.

Its initial state replaces one of ten 300s with a MISS between two grade reads.
The decoded instruction evidence, also unchanged, establishes:

| CIL-relative site | Operation |
|---|---|
|0001/0006/0008/000d/000e/000f|UInt16 n300, Single conversion, fresh total, Single conversion, division, Single store|
|0011–001f|Separate n50/another total quotient and Single store|
|0021–0029|Fresh success gate; false returns F8|
|00bd/00be/00c7|Promote p300 to Double; compare with .8; unordered/<= jumps to00d1|
|00ca/00cf|Fresh MISS; zero branches to A return00de|
|00d2/00d3/00dc|Promote retained p300; strict > Double .9 required for A alternate|
|00de/00df|A4 return|
|00e1/00e2/00eb/00ee/00f3|B first alternative >.7 and fresh MISS0|
|00f6/00f7/0100|B second alternative strict > Double .8|
|0102/0103|B5 return|

**RECONSTRUCTED independent arithmetic:** `grade_oracle.py` uses exact rational
integer division, normalizes the significand, and rounds ties-to-even to 24 bits.
It does not call production or import R15 development expectations.

```text
Single(9) / Single(10)
  -> bits 0x3f666666
  -> exact stored value 0.89999997615814208984375
Double(stored value) > Double(.9) -> false
MISS == 0                         -> false
Double(stored value) > Double(.8) -> true
first call all300 -> X2; mutated second call -> B5
```

The .9 literal is binary64 `0x3feccccccccccccd`, approximately
0.9000000000000000222. Promoting the already stored Single does not round it
up to that threshold. The MISS defeats the first A alternative independently.
This agrees with E042's own numeric R05 (90/100 with MISS10), which has the same
proportion and predicates, despite the conflicting descriptive R30 sentence.

**Disposition:** a historical research-description error under its declared
binary32 projection, independently tested against production in
`G/R30-independent-conflict`. It is not a demonstrated R15 production defect.
E042 report/evidence/checker and R15's initial/corrected development assertion
remain byte-preserved. The original R15 run43001655771 at27c89de failed that new
development oracle; run43002288864 at3f9a8f5 passed its corrected B expectation.
Those historical records are not used as the QA13 numerical oracle.

**UNVERIFIED ORIGINAL-RUNTIME:** original x86 CLR/JIT extended-intermediate
behavior was not measured. QA13 verifies the explicitly stored binary32 model,
not original runtime precision at a marginal boundary.
