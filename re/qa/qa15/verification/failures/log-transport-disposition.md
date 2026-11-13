The intermediate publication116b140 included a truncated base64 representation of
the first failed job log:1,048,606 bytes, with a literal665161-bytes-omitted marker.
The source-return log had already been retained locally and its expected hash was
recorded in initial-windows-provenance.json. The final QA15 publication restores the
complete lossless representation from that original return and verifies its decoded
byte count/hash. The corrupted intermediate Git blob remains in repository history.
No C# code, oracle expectation, chart input or passing CI result was affected.
Publication transport now chunks large text and rejects omitted-byte markers and
length mismatches; the final tree must match the local Git tree before ref update.
