# 1. Result

The first local run executed 236 controls: 228 passed, eight failed. Exact JSON,
log, harness files and hashes are retained in `verification/initial/`. Seven failures
were QA11 expectation/setup errors. G08 is retained unchanged as QA11-D01. No
production or historical assertion was edited. Two private Python edit helpers
also failed before writing files (quoting, then an incorrect working directory);
they executed no assertions and were corrected before the second run.

# 2. What they found

| Initial case | Incorrect expectation/setup | Independent correction authority |
|---|---|---|
| A01 overlap / touching | Expected the fresh parser to reach continuous admission | Existing R05 parser rejects intersecting or touching slider intervals first. The inputs now verify that rejection. P07 separately uses an explicitly artificial combined definition to exercise continuous admission; it does not claim fresh overlapping gameplay support. |
| I01 spans2 / spans3 | Restarted interior tick spacing at each reversal | E021 M144a:11f1–1200 carries phase and distance between traversals. With Q100, length240 and V1000/6: first traversal phase240/carry40 becomes phase180/carry60. Adding the next segment240 gives carry300: successive phase280/380 are accepted, phase480 is suppressed at remaining−60, boundary480 retained, carry reset0. Thus reverse ticks occur at7680/8280, then the next forward ticks9480/10080. Final timestamp is reduced36 only once. |
| I06 | Expected a failed overdue component after deliberately establishing tracking at6000 | E018 entry-time predicate retains EntryTime6000, which precedes the deadline. Corrected the input to omit that tracking update; first valid entry7441 is late, so the component fails while repeat audio remains eligible. The expected failed result was not weakened. |
| I08 false / true | Expected tracking false after the last component | E018 sectionE: leave is invoked only while processedCount&lt;total. After all components, stored Tracking can remain true even when current FollowEligible is false. At7440 the first continuing call remains reached, then aggregate cleanup and the positive hook's additional continuing call. Both initial outputs are preserved. |

These corrections follow the existing original-static reports and reached
production interfaces. Development-test expected values were not used as an oracle.
The corrected run adds eleven separately named controls, including evidence-local
validation and exact active-channel cases: 247 checks, 246 pass, G08 fails.

G08's input is finite: point volume25, effect80, universal50, channels101/202.
The independent backend changes UniversalVolume to100 when the first attribute2
write is entered. E040 M3b11 computes its Single gain once into local0 at0018,
then loads that local at001f and002c for the two channel writes. Therefore both
values must retain0.1. Actual R13 writes0.1 then0.2, recomputing its property.
P01 separately validates these already-published instruction relationships. This
is a conditional model discrepancy under artificial synchronous interleaving;
ordinary Windows reachability and audible consequences remain unverified.

# 3. Pseudocode

```text
preserve first inputs + output + exact harness + hashes
prove each correction from existing evidence
change only the new QA11 oracle/setup
rerun every new control
retain G08 unchanged and failing
publish QA11-D01 for a separate Work2 correction
```
