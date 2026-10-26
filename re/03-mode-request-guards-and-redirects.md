# 1. Result

Expedition 03 — Mode-Request Guards and Redirect Semantics. 2026-10-26 (Asia/Bangkok). Work 1, research only.

## Objective, scope and provenance

Recover every explicit guard, redirect, short-circuit and direct side-effect site in MethodDef `0x060046ad`, without guessing its callees' meanings. Parent commit: `586ed1d668d535533cd5822cf0fdd6790b61d5d0`, tree `f73cca9ee128d7a47d5ce88f3f1090b062882f17`, verified through GitHub. Read complete Instruction.md, cumulative contexts and Expedition 02. Dedicated branch: `work1/expedition-03-guards`. Earlier records remain unchanged; contexts receives only an append. No reconstructed gameplay components or changes under `reconstructed/`.

Reference ZIP SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; EXE SHA-256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`. Same baseline metadata Stable40 / file version 1.3.3.8, not a verified dated official release. Exact provenance remains UNRESOLVED. Original-game execution was NOT performed; Windows environment/settings/capture remain unavailable. Static original CIL inspection on Linux is not a gameplay observation.

## Evidence and actual validation

- `evidence/03/request-guards.json`: method identities, 63 basic blocks, 36 conditional/switch sites, all eight explicit return offsets, ordered token sites/constants, field/callee signatures, two finally regions and deferred callback cross-references.
- `tools/request_guard_evidence.py`: read-only extraction directly from ZIP using dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2. It does not execute or simulate mode requests.
- `tools/check_request_guards.py`: checks the pinned binary/body identity, branch destinations, switch mapping, side-effect locations and deferred callback. No replacement request function exists in this tooling.
- Existing Expedition 02 local inspection tool rechecked `060046ad`, `060046e6` and closure constructor `060046e5`. Original disassembly was inspected privately, not committed. The closure constructor only calls an Object constructor.

Method `060046ad`: RVA `0x1c3098`, file offset 1,839,768; 12-byte header; 750 code bytes; 792 bytes including header/extra sections. Body SHA-256 `8e25430f79dd7f0cff8e522b79c12823a0e9b7371e3f1efd410017b94dce7c14`. All offsets below are hex and relative to the first opcode, excluding the header. EH offsets from dncil already use that basis. CFG branches show instruction-level transfers; `leave` targets omit finally unwinding, recorded separately in exception regions. This graph is not an exception-aware interpreter or proof of path feasibility.

Reproduction commands, repository root:

```sh
python re/tools/request_guard_evidence.py /path/osubackup.zip > /tmp/guards.json
python re/tools/check_request_guards.py /tmp/guards.json
python re/tools/static_control_flow.py /path/osubackup.zip --inspect-token 060046ad
python re/tools/static_control_flow.py /path/osubackup.zip --inspect-token 060046e6
python -m py_compile re/tools/request_guard_evidence.py re/tools/check_request_guards.py
```

Executed: extractor twice with byte-identical output; evidence checker PASS; compilation PASS; all extracted branch destinations verified on instruction boundaries; baseline/prefix preservation and staged diff checks PASS. The checker checks 34 conditional destinations plus the switch and identity of the remaining normalization branch through the pinned body hash. No dynamic path, original-input case, scheduler test, UI capture, or gameplay equivalence test ran. Branch predictions below are not test passes on original gameplay. No authorised exceptions.

# 2. What they found

## Interface and confidence convention

**CONFIRMED** below means direct static evidence in this exact binary. Semantic aliases are **RECONSTRUCTED** from dataflow. Callee purpose, units, runtime reachability and atomicity remain **UNRESOLVED** unless stated otherwise. `Fxxxx` abbreviates field token `0x0400xxxx`; `Mxxxx` abbreviates method token `0x0600xxxx`. Calls must retain their argument order, receiver, short-circuit order and ability to throw/mutate state. Fields are live reads at their original locations, not one immutable input snapshot.

Inputs: static void method `(osu.OsuModes target, bool flag)`; private closure holds target in `F2cae` and flag in `F2caf`. Outputs: no return value; possible queued callback, early return, direct state writes and opaque external calls. Enum constants: Menu=0, Edit=1, Play=2, Exit=3, SelectPlay=5, Rank=7, Update=8, Busy=9, Unknown=10. No explicit enum-range validation is present.

| Alias / dependency | Token | Static type / role |
|---|---|---|
| pending/current mode | F2c6c / F2c6d | OsuModes; independently read live |
| gate operands | F2c3a / F247e | signed int32; units unresolved |
| active object / Edit flag | F2c6e / F2910 | reference cast to TypeDef 020008b6, then bool field |
| conditional guard group | F2c48 | bool; not given a product setting name |
| Play-exit blockers | F2a82; F136d.F13bb | bool; optional reference with enum/int comparison to 4 |
| Rank-to-Play threshold operand | F2c51 | double; units unresolved |
| optional pre-suppression call | F1f51 | bool controlling M4060 call |
| collection / selection field | F2c1f / item.F157f | collection; per-item bool sampled before callbacks |
| helper receivers | F2c2e / F2c37 | references; semantics unresolved |
| outgoing rate / phase | F2c7e / F2c6b | double / enum numeric phase |
| Exit callback | F2c56 | nullable delegate |
| Exit-rate selectors | F040e via M177c; F2ca1; F1483; F2c48 | conversion result; enum compared to 4; bool; bool |
| final reset | F2a79 | bool written false only on Exit path |

## Complete guard ordering (CONFIRMED static structure)

| Order and offsets | Condition and result |
|---|---|
| 1: `000–02b` | Allocate closure, capture arguments. If target==Unknown, replace it with the current mode and set captured flag=true. Normalization occurs once per invocation. |
| 2: `030–06d` | If effective target is nonzero AND signed F2c3a < F247e, enqueue callback M46e6 on F2c78 via M09af, with delay expression F247e - F2c3a + 170 and final argument false; discard result and return at 06d. Menu bypasses this gate. Values are re-read for the expression; arithmetic uses unchecked int32 add/sub. No unit or scheduler contract is established. |
| 3: `06e–098` | If current==Edit, cast active object to 020008b6. If its F2910 is false, invoke M41c7 on the cast object. If that result is also false, return at 098. True F2910 skips M41c7. Even Exit/Update and flag=true pass through this gate. Null/wrong-type active object can throw rather than reject cleanly. |
| 4: `099–0c3` | Exit or Update bypass both predicates. For every other effective target, call M477a(false). Only if true call M3b5b(). If either required result is false, redirect captured target to Menu AND set flag=true. Do not name these predicates as login/license/network checks without evidence. |
| 5: `0c8–154` | Only when F2c48 is true, execute the three subrules below, using the already redirected target. False skips directly to 155. |
| 6: `155–176` | Call M405e() unconditionally once this stage is reached. If F1f51, call M4060(M49b0(2064525784), M4e03(), 60000, null). M4e03 resolves to Color.get_Red. Both callsites precede all duplicate and pending-Exit checks. Purposes/string text/units remain unresolved. |
| 7: `17b–190` | If target==current AND flag=false, return at 190. |
| 8: `191–1a6` | If target==pending AND flag=false, return at 1a6. |
| 9: `1a7–1af` | If pending==Exit, return at 1af, irrespective of flag. This is not an entry-time lock: deferral and earlier helper calls can occur first. |
| 10: `1b0–2ed` | Snapshot selected collection items, invoke callbacks, write pending, perform common and optional Exit effects, then set phase=0 and return. No constructor/commit invocation occurs directly in this method. |

The F2c48 group has precise target-dependent routing:

- **SelectPlay (5):** redirect to Menu at `0ff–101`, preserve flag, then jump to `133`. It bypasses the Play-exit blocker. This redirect differs from predicate failure, which sets flag=true.
- **Exit (3), Rank (7), Busy (9):** jump directly to `133`, bypassing that blocker.
- **All other target values**, including Menu, Edit, Play, Update, SelectEdit, SelectDrawings and out-of-range integers: enter `108`. If current==Play AND M3b0d()==false, return at `132` when F2a82 is true OR when F136d is non-null and its F13bb==4. The second alternative is not read if F2a82 is true. A null F136d skips its field read. Predicate true bypasses both blockers.
- At `133`, if current==Rank AND effective target==Play AND F2c51 is ordered-less-than 10000, return at `154`. `bge.un.s` at `152` permits both >=10000 and unordered/NaN to proceed. Exactly 10000 passes; negative infinity blocks, positive infinity and NaN pass. This is a comparison fact, not evidence of ten seconds of elapsed time.

The switch at `0dc` uses target-3 with destinations `[133,108,0ff,108,133,108,133]` for values 3–9 and default `108`. All guard evaluations occur before duplicate suppression, except the two duplicate checks themselves and the pending-Exit check.

## Boolean influence and deferred replay

**CONFIRMED:** The original boolean is stored at `00f` and only read at `189`/`19f` in M46ad. It changes only the two duplicate checks. It does not directly bypass time deferral, Edit rejection, opaque predicate redirects, F2c48 blockers, pending Exit, callbacks or exceptions. The method itself can set it true through Unknown normalization or predicate-failure redirection. SelectPlay redirection does not set it true. No caller-visible boolean is returned or modified.

Deferred callback M46e6, RVA `0x1c4d4c`, body hash `733f7e46430aa1895c2c3d71d9f28d75f4d8a4041723f4061175211ba25b95ac`, reads captured target/flag and calls M46ad at code offset `00c`. Thus it reruns the complete method with live global guards; it does not resume after the gate. If Unknown was normalized before deferral, its captured target is the old current value, not an instruction to fetch a later current value again (unless that captured value itself is Unknown). Repeated deferral is statically possible. Cancellation, timing/thread policy and callback reachability are UNRESOLVED.

## Complete direct side-effect sequence

Assuming normal completion of all invoked operations:

1. Pre-suppression helper calls at `155` and optional `176` have already happened even if stages 7–9 return. Therefore duplicate suppression does not mean the invocation was side-effect-free.
2. Allocate a temporary list at `1b0`. Enumerate F2c1f; append items whose F157f is true. Finish/dispose that enumerator. Enumerate this snapshot in original order; invoke each item's virtual M2560(false). Finish/dispose the second enumerator. No matching items means no M2560 calls. Selection is sampled before the callbacks, not re-tested for each callback. Both loops have finally disposal regions (`1c2–1e6` / handler `1e6–1f4`, and `1fc–216` / handler `216–224`). They are finally regions, not catches.
3. Write captured target to pending F2c6c at `22a`, AFTER every snapshot-item callback returns.
4. Invoke F2c2e.M4735(false) at `235`; write F2c7e=10.0 at `243`; invoke F2c37.M2cea(true) at `24e`; invoke F2c37.M2ce6(F2c2e) at `25d`.
5. Only when captured target==Exit: invoke F2c56 if non-null at `277`. Evaluate M177c(F040e). Use rate=5.0 if that result is false, OR (F2ca1==4 AND F1483), OR F2c48. Otherwise use rate=0.6 and invoke M3b2b(M49b0(2064525556),100,choice), discard its return. Here choice is 3 when `(MemberRef 0a0004cc(F2d26) & 4) > 0` (signed comparison), else 1. No semantic name for the converted enum or audio/UI action is assumed. Finally write F2a79=false at `2e2`. The conversion/selector order is short-circuited exactly as pseudocode below.
6. Write F2c6b=0 at `2e8`, return at `2ed`.

The method does not catch exceptions. Allocation/casts/null dereferences, enumerators, predicates, helpers and callbacks can throw; only enumerator cleanup is locally guaranteed through finally, subject to exceptions within disposal itself. The method is not transactional. A callback can modify pending or other globals between reads; no final recheck of pending Exit occurs after the snapshot callbacks. The later write at `22a` can replace a pending value written by one of those callbacks. Conversely, a helper called after `22a` can change pending again without this method restoring it. Concurrency/reentrancy in real runs is UNRESOLVED, but no atomicity can be inferred from the method body.

## Branch-derived reference cases (NOT original-game test results)

Unless overridden, earlier gates allow continuation, references/casts are valid, predicates return true, F2c48=false, target differs from current/pending, pending is not Exit, and callees complete normally. These are controlled assumptions, not measured defaults.

| ID | Variation / precondition | Static prediction / key offsets |
|---|---|---|
| G01 | target=Unknown | target=current, flag=true; normalization 01c–02b |
| G02 | target=Unknown and current=Unknown | remains Unknown; no range/sentinel rejection here |
| G03 | target=Menu and F2c3a<F247e | no deferral; 036 jumps 06e |
| G04 | target=Exit, gate closed, flag=true, pending=Exit | queue then return 06d BEFORE pending-Exit check |
| G05 | nonzero target, gate operands equal | signed bge proceeds to 06e |
| G06 | current=Edit, F2910=false, M41c7=false | return 098 even for Exit/flag=true |
| G07 | current=Edit, F2910=true | skip M41c7 |
| G08 | target=Exit or Update | skip M477a and M3b5b |
| G09 | other target, M477a(false)=false | skip M3b5b; target=Menu, flag=true |
| G10 | other target, first predicate true, M3b5b=false | target=Menu, flag=true |
| G11 | F2c48=true, target=SelectPlay, flag=false, current=Menu | redirect Menu without forcing; duplicate-current return 190 |
| G12 | F2c48=true, current=Play, target=SelectPlay, F2a82=true | redirect Menu and skip Play blocker |
| G13 | F2c48=true, current=Play, target=Rank/Exit/Busy, blocker=true | skip Play blocker; do not infer general forced acceptance |
| G14 | F2c48=true, current=Play, target=Update, M3b0d=false, F2a82=true | return 132 despite Update bypassing earlier predicates |
| G15 | same, other eligible target, M3b0d=true | skip both Play blockers |
| G16 | eligible Play blocker path, F2a82=false, F136d=null | skip F13bb read and continue |
| G17 | eligible Play blocker path, F2a82=false, F136d.F13bb=4 | return 132 |
| G18 | F2c48=false, current=Rank, target=Play, operand=0 | skip threshold gate entirely |
| G19 | F2c48=true, current=Rank, target=Play, F2c51=9999 | return 154 regardless of flag |
| G20 | same, operand=10000 or NaN | proceed to 155 |
| G21 | target=current, flag=false | M405e/optional M4060 first; return 190 |
| G22 | target=pending but not current, flag=false | pre-suppression helpers first; return 1a6 |
| G23 | target=current or pending, flag=true, pending not Exit | pass duplicate checks, other guards still apply |
| G24 | pending=Exit, target differs, flag=true | pre-suppression helpers first; return 1af |
| G25 | target=current=pending=Exit, flag=false | return 190, not 1af; check order matters |
| G26 | M2560 callback writes pending=Exit, original check passed | later pending write at 22a overwrites it if callback returns |
| G27 | M2560 callback throws | no 22a write from this invocation; finally attempts enumerator Dispose |
| G28 | accepted non-Exit target | common rate 10; no Exit callback/reset; phase 0 last |
| G29 | accepted Exit, M177c=false | rate 5; skip F2ca1/F1483/F2c48 rate decisions and M3b2b |
| G30 | Exit, M177c=true, F2ca1=4, F1483=true | rate 5 without reading F2c48 for rate selection |
| G31 | Exit, M177c=true, F2ca1!=4, F2c48=true | skip F1483; rate 5 |
| G32 | Exit, M177c=true, (F2ca1!=4 OR F1483=false), F2c48=false | rate .6; M3b2b choice 1/3 from converted bit 4; F2a79=false, phase=0 |
| G33 | earlier gate permits target=24, predicates true, no duplicates | no range rejection; can write 24 to pending |
| G34 | deferred Unknown normalized to Play, global current later changes | replay argument remains Play; global guards re-evaluated |

Competing explanations resolved statically: boolean-as-global-bypass versus duplicate-only; pending Exit as first guard versus late suppression; identical Menu redirects versus distinct flag handling; duplicate no-op versus pre-suppression helper effects; live iteration during callbacks versus snapshot-then-callbacks. UNRESOLVED: meanings/side effects of opaque callees and flags, which combinations are naturally reachable, runtime exception frequencies and actual time units. Next discriminating experiment: instrument M46ad/M46e6 and opaque predicates on the pinned Windows original, log argument transformations, each guard's values, queued callbacks, ordered helper invocations and writes. Use G04/G11/G14/G20/G21/G26 as priority probes; do not treat them as observed behavior before that run.

# 3. Pseudocode

**CONFIRMED static control structure; RECONSTRUCTED aliases; UNRESOLVED callees stay token-labelled.** This is documentation, not executable replacement gameplay code. Read fields live and preserve short-circuit/evaluation order. `invoke` may throw; there is no catch/rollback. Simplified notation for loops retains their finally cleanup.

```text
Request(target, flag):
    capture target and flag in a new closure
    if target == Unknown:
        target = Read(F2c6d)
        flag = true

    if target != Menu and signed(Read(F2c3a)) < signed(Read(F247e)):
        F2c78.M09af(delegate M46e6(captured target, flag),
                   int32_wrap(Read(F247e) - Read(F2c3a) + 170), false)
        return

    if Read(F2c6d) == Edit:
        if not cast020008b6(Read(F2c6e)).F2910:
            if not cast020008b6(Read(F2c6e)).M41c7(): return

    if target != Exit and target != Update:
        if not M477a(false) or not M3b5b():
            target = Menu
            flag = true

    if Read(F2c48):
        if target == SelectPlay:
            target = Menu                 # flag unchanged; skip Play blocker
        else if target not in {Exit, Rank, Busy}:
            if Read(F2c6d) == Play and not M3b0d():
                if Read(F2a82): return
                if Read(F136d) != null and Read(F136d).F13bb == 4: return
        if Read(F2c6d) == Rank and target == Play:
            if ordered_less(Read(F2c51), 10000.0): return

    M405e()
    if Read(F1f51):
        M4060(M49b0(2064525784), M4e03(), 60000, null)

    if target == Read(F2c6d) and not flag: return
    if target == Read(F2c6c) and not flag: return
    if Read(F2c6c) == Exit: return

    snapshot = new list
    using enumerator over Read(F2c1f):
        append each item with item.F157f == true
    using enumerator over snapshot:
        invoke each item.M2560(false)

    Write(F2c6c, target)
    Read(F2c2e).M4735(false)
    Write(F2c7e, 10.0)
    Read(F2c37).M2cea(true)
    Read(F2c37).M2ce6(Read(F2c2e))

    if target == Exit:
        if Read(F2c56) != null: invoke Read(F2c56)()
        if not M177c(Read(F040e)):
            Write(F2c7e, 5.0)
        else if Read(F2ca1) == 4 and Read(F1483):
            Write(F2c7e, 5.0)
        else if Read(F2c48):
            Write(F2c7e, 5.0)
        else:
            Write(F2c7e, 0.6)
            # preserve argument-evaluation order: decode string before conversion
            text = M49b0(2064525556)
            value = MemberRef0a0004cc(Read(F2d26))
            choice = 3 if signed(value & 4) > 0 else 1
            discard M3b2b(text, 100, choice)
        Write(F2a79, false)
    Write(F2c6b, 0)
    return

DeferredClosure.M46e6():
    M46ad(captured_target, captured_flag)   # full new invocation, not continuation
```
