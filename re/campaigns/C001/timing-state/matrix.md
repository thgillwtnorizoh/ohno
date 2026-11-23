# Issue22 fixture matrix

RECONSTRUCTED original-static scalar oracles; VERIFIED COMPONENT execution. All original-runtime cases NOT RUN.

| Fixture | Version | Production disposition | Rate / end | Confidence or boundary |
|---|---:|---|---|---|
| base | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| before-first-point-fallback | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| inherited-only-no-base | 14 | ADMITTED | 100 / 5000 | M1be3 index0 fallback is inherited; effective beat is negative, so ordinary travel fallback applies. No invented positive base. |
| finish-plus-five-boundary | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| equal-base-inherited | 14 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| equal-inherited-base | 14 | ADMITTED | 166.666666667 / 3399 | Finite staged original-derived selectors |
| equal-two-bases | 14 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| equal-two-inherited | 14 | ADMITTED | 100 / 5000 | Finite staged original-derived selectors |
| reverse-base-times | 14 | ADMITTED | 100 / 5000 | Finite staged original-derived selectors |
| reverse-inherited-times | 14 | ADMITTED | 100 / 5000 | Finite staged original-derived selectors |
| future-does-not-stop-scan | 14 | ADMITTED | 100 / 5000 | Finite staged original-derived selectors |
| base-index-resets-factor | 14 | ADMITTED | 166.666666667 / 3399 | Finite staged original-derived selectors |
| fraction-after-start | 14 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| exact-start | 14 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| fraction-before-start | 14 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| negative-fraction-before-zero | 14 | ADMITTED | 400 / 1000 | Finite staged original-derived selectors |
| positive-fraction-after-zero | 14 | ADMITTED | 400 / 1000 | Finite staged original-derived selectors |
| inherited-positive-normalizes | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| inherited-zero-normalizes | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| factor-lower-clamp | 14 | ADMITTED | 1999.9999702 / 1200 | Finite staged original-derived selectors |
| factor-upper-clamp | 14 | ADMITTED | 20 / 21000 | Finite staged original-derived selectors |
| factor-single-conversion | 14 | ADMITTED | 600.000035763 / 1666 | Finite staged original-derived selectors |
| later-point-changes-samples-only | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| head-plus-five-boundary | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| tick-plus-two-boundary | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| empty-timing | 14 | ADMITTED/SOUND_UNSUPPORTED | 100 / 5000 | Original discrete query returns null; tick request requires a point. No invented sample defaults. |
| short-two-columns | 14 | ADMITTED/SOUND_UNSUPPORTED | 200 / 3000 | Original two-column bank/index depend on unreconstructed global state; production timing succeeds, sound error expected. |
| short-four-columns | 14 | ADMITTED/SOUND_UNSUPPORTED | 200 / 3000 | Original omitted volume uses caller operand; production sound error expected. |
| invalid-sample-bank | 14 | ADMITTED/SOUND_UNSUPPORTED | 200 / 3000 | Original metadata reader drops malformed row; production retains travel row and reports sound error. Original full-load consequence unresolved. |
| zero-bank-volume-clamp | 14 | ADMITTED | 200 / 3000 | Finite staged original-derived selectors |
| unusual-sample-fields | 14 | ADMITTED | 200 / 3000 | Discrete enum fallback and negative index clamping; continuing admission explicitly unsupported. |
| zero-base-rejected | 14 | ENGINEERING REJECT | excluded | ENGINEERING exclusion; no original nonpositive base runtime oracle. |
| negative-base-rejected | 14 | ENGINEERING REJECT | excluded | ENGINEERING exclusion; no original nonpositive base runtime oracle. |
| nonfinite-base-rejected | 14 | ENGINEERING REJECT | excluded | ENGINEERING finite guard; original CLR exceptional conversions NOT VERIFIED. |
| invalid-flag-rejected | 14 | ENGINEERING REJECT | excluded | ENGINEERING strict flag; original first-character rule differs in metadata stage. |
| missing-beat-rejected | 14 | ENGINEERING REJECT | excluded | Malformed original metadata row returns null; whole original load NOT VERIFIED. |
| two-traversal-control | 14 | ADMITTED | 200 / 5000 | Finite staged original-derived selectors |
| multi-parent-registration | 14 | ADMITTED | 200 / 3000; 400 / 6000 | Finite staged original-derived selectors |
| version-5 | 5 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-6 | 6 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-7 | 7 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-8 | 8 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-9 | 9 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-10 | 10 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-11 | 11 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-12 | 12 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
| version-13 | 13 | ADMITTED | 400 / 2000 | Finite staged original-derived selectors |
