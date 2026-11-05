# E040 coverage reassessment

No original runtime was executed. This matrix covers finite managed variants; exact original native operands and unsupported state remain conditional. Full original prerequisites and expected/actual locators are in `e040-coverage.json`. Cases are counted once in the harness, never once per matrix row.

| E040 | Assessment | Independent cases |
|---|---|---|
|L01|FINITE_MANAGED_VARIANT|P02 active session before tick only pan submitted|
|L02|FINITE_MANAGED_VARIANT|I03 timely tick has two continuing calls, P03 active session due tick submits pan twice|
|L03|FINITE_MANAGED_VARIANT|C15 repeated submissions do not invent pending-active state, I04 same-time update continues without discrete duplicate|
|L04|FINITE_MANAGED_VARIANT|I05 tracking loss consumes audio before reentry|
|L05|FINITE_MANAGED_VARIANT|P04 tracking false zero pair still queues Pause|
|L06|FINITE_MANAGED_VARIANT|I05 tracking loss consumes audio before reentry, P05 completed cleanup then reentry can start|
|L07|FINITE_MANAGED_VARIANT|C14 mask2 layeredFalse|
|L08|FINITE_MANAGED_VARIANT|C14 mask2 layeredTrue|
|L09|FINITE_MANAGED_VARIANT|C14 mask4 layeredFalse, C14 mask4 layeredTrue, C14 mask8 layeredFalse, C14 mask8 layeredTrue|
|L10|FINITE_MANAGED_VARIANT|C16 active mask exclusion does not Stop old row|
|L11|UNSUPPORTED_ORIGINAL_PATH|A01 bounded admission format13|
|L12|PARTIAL_EXECUTED|C03 row0-only change preserves row1, C10 base0/addition0 inherit global bank|
|L13|PARTIAL_EXECUTED|C04 row1-only change preserves row0, C10 base0/addition0 inherit global bank|
|L14|FINITE_MANAGED_VARIANT|C05 sample-index change replaces both rows|
|L15|FINITE_MANAGED_VARIANT|C03 row0-only change preserves row1|
|L16|FINITE_MANAGED_VARIANT|C06 zero old handle skips activity and Stop|
|L17|FINITE_MANAGED_VARIANT|C08 unchanged pair early return|
|L18|FINITE_MANAGED_VARIANT|C07 pair application gate0, C07 pair application gate9|
|L19|FINITE_MANAGED_VARIANT|C09 force unchanged pair attributes without Acquire|
|L20|FINITE_MANAGED_VARIANT|R02 dynamic row0 bank1, R02 dynamic row0 bank2, R02 dynamic row0 bank3, R02 dynamic row1 bank1, R02 dynamic row1 bank2, R02 dynamic row1 bank3|
|L21|FINITE_MANAGED_VARIANT|R03 dynamic cache-1, R04 missing dynamic entry cache-only fallback|
|L22|FINITE_MANAGED_VARIANT|R03 dynamic cache0|
|L23|FINITE_MANAGED_VARIANT|C12 unresolved resource sample-1 still acquired, R16 no resources prepared minus1|
|L24|FINITE_MANAGED_VARIANT|R01 mode2 row0 bank1 plane0, R01 mode2 row0 bank1 plane1, R01 mode2 row0 bank1 plane2, R01 mode2 row0 bank2 plane0, R01 mode2 row0 bank2 plane1, R01 mode2 row0 bank2 plane2, R01 mode2 row0 bank3 plane0, R01 mode2 row0 bank3 plane1, R01 mode2 row0 bank3 plane2, R01 mode2 row1 bank1 plane0, R01 mode2 row1 bank1 plane1, R01 mode2 row1 bank1 plane2, R01 mode2 row1 bank2 plane0, R01 mode2 row1 bank2 plane1, R01 mode2 row1 bank2 plane2, R01 mode2 row1 bank3 plane0, R01 mode2 row1 bank3 plane1, R01 mode2 row1 bank3 plane2|
|L25|FINITE_MANAGED_VARIANT|R02 dynamic row0 bank1, R02 dynamic row0 bank2, R02 dynamic row0 bank3, R02 dynamic row1 bank1, R02 dynamic row1 bank2, R02 dynamic row1 bank3|
|L26|FINITE_MANAGED_VARIANT|G01 point minimum volume0, G01 point minimum volume1, G01 point minimum volume7, G01 point minimum volume8, G01 point minimum volume9, G01 point minimum volume17, G01 point minimum volume23, G01 point minimum volume100|
|L27|FINITE_MANAGED_VARIANT|G04 no song volume100|
|L28|FINITE_MANAGED_VARIANT|G03 absent current point fallback0, G03 absent current point fallback1, G03 absent current point fallback7, G03 absent current point fallback23|
|L29|FINITE_MANAGED_VARIANT|G02 distinct Single operation orders literal ULP, G07 settings refresh both current targets|
|L30|FINITE_MANAGED_VARIANT|G06 volume-only change leaves active gain untouched|
|L31|FINITE_MANAGED_VARIANT|D11 stored presentation pan0, D11 stored presentation pan64, D11 stored presentation pan256, D11 stored presentation pan448, D11 stored presentation pan512, D11 stored presentation pan768, I13 stored presentation operand is consumed|
|L32|FINITE_MANAGED_VARIANT|D12 no pan action when absent|
|L33|FINITE_MANAGED_VARIANT|D01 start targets fresh channel and gain, D02 second channel read after Attribute|
|L34|FINITE_MANAGED_VARIANT|D04 Pause submitted101/202 executes301/402, D05 Pause second target reread|
|L35|FINITE_MANAGED_VARIANT|D08 pause then start controlled order|
|L36|FINITE_MANAGED_VARIANT|D09 pending pause with active1 suppresses new start|
|L37|FINITE_MANAGED_VARIANT|I07 positive pending aggregate at7440, I08 final componentFalse retains positive aggregate cleanup, I08 final componentTrue retains positive aggregate cleanup|
|L38|FINITE_MANAGED_VARIANT|I07 positive pending aggregate at7441|
|L39|FINITE_MANAGED_VARIANT|P06 two artificial same-bank producers have no owner|
|L40|FINITE_MANAGED_VARIANT|D15 artificial producers share one pair|
|L41|FINITE_MANAGED_VARIANT|D13 manager currentFalse active0, D13 manager currentFalse active1, D13 manager currentFalse active3, D14 manager activity short-circuits row1|
|L42|FINITE_MANAGED_VARIANT|D13 manager currentFalse active0, D13 manager currentFalse active1, D13 manager currentFalse active3, D13 manager currentTrue active0, D13 manager currentTrue active1, D13 manager currentTrue active3|
|L43|UNSUPPORTED_ORIGINAL_PATH|B08 manager proxy and effect exception containment labelled|
|L44|PARTIAL_EXECUTED|T01 supported post-song-load initial state, T13 reset returns minus1 not raw CLRzero, I14 restart model and session isolation|
|L45|FINITE_MANAGED_VARIANT|T04 strict rescan at start+4, T04 strict rescan at start+5, T04 strict rescan at start+6, T05 same timestamp changes on forward equality|
|L46|FINITE_MANAGED_VARIANT|T05 same timestamp changes on forward equality|
|L47|FINITE_MANAGED_VARIANT|T08 first future blocks later expired entry|
|L48|FINITE_MANAGED_VARIANT|T09 explicit rescan sorts before strict search, T10 equal-time base before inherited|
|L49|PARTIAL_EXECUTED|T02 empty list resets, T03 null enumerable explicit API rejection|
|L50|PARTIAL_EXECUTED|T01 supported post-song-load initial state, T22 cursor reset retains selected fields and channels|
|L51|PARTIAL_EXECUTED|T21 external gate no selected, T21 external gate format3, T21 external gate format4, T21 external gate no song, T21 external gate identity, T23 song format3 sample-index gate, T23 song format4 sample-index gate|
|L52|UNSUPPORTED_ORIGINAL_PATH||
|L53|SOURCE_INSPECTION_ONLY|B02 Windows cancellation and pause policy, B05 native substitutions remain explicit logical operations|
|L54|PARTIAL_EXECUTED|R18 ClearMap removes dynamic cache but retains prepared cells, R19 ClearAll clears three tables, A04 prepared map replacement dynamic cache isolation, B02 Windows cancellation and pause policy|
|L55|CONFIRMED_CONDITIONAL_DISCREPANCY|G07 settings refresh both current targets, G08 settings gain captured once across backend callback, P01 published M3b11 stores one gain before both writes|
|L56|FINITE_MANAGED_VARIANT|C13 partial mutation at Activity, C13 partial mutation at Stop, C13 partial mutation at resolve0, C13 partial mutation at Acquire, C13 partial mutation at Attribute, D16 callback failure Play, D16 callback failure Attribute, D16 callback failure Pause|
