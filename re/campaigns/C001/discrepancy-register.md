# C001 discrepancy register

Status: OPEN. None of these records authorizes star fitting or historical expectation changes.

| ID | Finding | Classification / evidence | Disposition |
|---|---|---|---|
| C001-D01 | QA15 owner reports 8 matching / 19 mismatching displays, no ohno lower; nearly-equal-angle/opposing-direction especially divergent | OWNER-REPORTED in campaign authorization; exact chart hashes, original executable and cache state uncorroborated | OPEN. Preserve QA15 CSV/manifest as historical. Obtain matched provenance; investigate preparation without assuming causality |
| C001-D02 | Catalog chooses spinner then slider then circle for combined bits; original M28f7 chooses circle then slider then spinner then hold | CONFIRMED STATIC source/original relationship; production execution pending | OPEN. Canonical classification must serve catalog/gameplay; historical gameplay currently rejects combinations |
| C001-D03 | Catalog requires exact first-line header; gameplay accepts leading blanks/header whitespace; star admission requires exact v14 | SOURCE INSPECTION, ENGINEERING profiles | OPEN. Distinguish numeric admission from shared document parsing |
| C001-D04 | Metadata parser M1bc1 preserves colons after first key separator; object loader M28f7 uses only first two colon-split cells | CONFIRMED STATIC; original-behavior/01-parser-boundaries.md | OPEN. Canonical representation must preserve stage differences where applicable |
| C001-D05 | Unknown sections retain the previous recognized section on inspected original readers | CONFIRMED STATIC Enum.Parse catch-before-assignment | OPEN. Current independent readers replace section with unknown text |
| C001-D06 | HP/CS/OD/AR defaults/version13 parsing/mode3 CS and order-sensitive AR inheritance exceed prototype coverage | CONFIRMED STATIC M1387/M1bc1 | OPEN. Work 2 must preserve field history and mode at parse time |
| C001-D07 | Original slider repeats reject >9000; prototype limits 16; original missing length starts0 and effective traversal max1 | CONFIRMED STATIC M28f7/M142d; current implementation limits are ENGINEERING | OPEN; do not merely raise limits without resource controls and preparation fidelity |
| C001-D08 | Retained original-runtime E020 cases never run | UNVERIFIED ORIGINAL-RUNTIME | OPEN owner gate; component checks do not close it |
