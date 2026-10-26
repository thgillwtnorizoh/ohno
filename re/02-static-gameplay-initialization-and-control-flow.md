# 1. Result

Expedition 02 — Static Gameplay Initialization & Runtime Call-Graph Recovery. 2026-10-26, Asia/Bangkok. Work 1 is now the exclusive research agent; no reconstructed game implementation is authorised.

## Objective and scope

Narrow question: which binary-verifiable bootstrap, mode-request, transition-commit and Play-initialization relationships can be recovered without running the supplied executable? Scope is managed metadata and CIL cross-references, with selected control-flow interpretation. No runtime, renderer or temporary replacement component was built. `reconstructed/` was not created or modified.

Baseline: explicitly requested commit `f0ddc9ec116acf67b4f3ed2436f0d0c6503fd31f`, tree `23c307de9f2737453b69af4918566b24852bc4ab`. Read its contract, contexts, Expedition 01 mark, environment report and archive evidence. The old branch head contains the subsequently requested deletions. This expedition uses a separate branch from the specified historical baseline; it does not reverse those deletions on the old branch or rewrite history. Earlier records on the new branch are unchanged.

## Evidence used and tests

- Supplied ZIP SHA-256 `7089d49fe3e91be66e026b1d1a3245503f65580879b20bb81e7d97dd8534b738`; executable SHA-256 `6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d`. Exact official dated release and clean/modified status remain UNRESOLVED.
- `evidence/02/static-control-flow.json`: assembly inventory, metadata enum constants, selected method RVA/file offset/body hashes, calls, field accesses and switch destinations. Site arrays mean `[code_offset, opcode, target_token]`; switch targets use the same offset basis. Tokens are scoped to this exact executable, never universal names.
- `tools/static_control_flow.py`: read-only reproducible analyser. Dependencies dnfile 0.18.0, pefile 2024.8.26, dncil 1.0.2. It neither loads executable code into a CLR nor extracts distributable assets.
- CIL inspected privately for the selected methods. Public evidence contains metadata/cross-references and independently written explanations, not decompiled proprietary source or reference assets.

Commands from repository root:

```sh
python -m pip install dnfile==0.18.0 pefile==2024.8.26 dncil==1.0.2
python re/tools/static_control_flow.py /path/osubackup.zip > /tmp/static-control-flow.json
python re/tools/check_static_control_flow.py /tmp/static-control-flow.json
python -m py_compile re/tools/static_control_flow.py re/tools/check_static_control_flow.py
```

Executed whole-executable scan: 21,398 MethodDefs, 19,659 decoded nonzero-RVA bodies, 1,739 zero-RVA methods, zero decoding errors, zero decoded branch/switch targets outside instruction boundaries, 119,386 call-like sites. Zero-RVA methods are not called failures or assumed unused. Repeated output matched byte-for-byte; selected edges, enum values and branch mapping passed evidence assertions. Host remains Linux without reference execution/capture capability. No original gameplay, signature validation, JIT execution, import/play test or timing measurement was performed. Parser success is not reachability or semantic correctness proof.

Changed only this new mark, new analyser, evidence checker and evidence, and an append to `contexts.md`. Publication branch/revision and verification are in the final handoff. No contract exception was used; the latest user instruction narrows this agent to research only.

# 2. What they found

To recheck branch arithmetic locally, run the analyser with `--inspect-token 060046a3`; similarly inspect `060046ad` and `060046b0` for request/dispatch guards. Printed instruction and branch offsets exclude method headers. Do not commit the resulting original disassembly.

## Mechanism and confidence

**CONFIRMED (metadata):** `osu!gameplay.dll`, `osu!ui.dll`, and `osu!seasonal.dll` each have two TypeDefs and four methods: constructor plus resource-manager/culture accessors. Their resource payload sizes are 31,831,830 / 25,998,273 / 3,693,277 bytes respectively. They are resource-store assemblies, not evidence of separate gameplay logic modules. `osu!auth.dll` has no CLR header recognised by dnfile; native internals were not investigated. `osu!.exe` has 2,907 TypeDefs and carries the inspected managed logic. No claim about hidden code inside resources is made.

**CONFIRMED (static callsites):** CLR entry `0x06002c49`, RVA `0x124288`, calls `0x060023b9` and returns. Startup contains command-line/current-directory handling, assembly-resolve handler registration and conditional failure/exit paths. A later site calls `0x060023bc`; that method conditionally calls `0x060023be`. The latter contains four construction sites for host constructor `0x06004668`, each followed by base-host method `0x06004247`. These are alternative paths, not four hosts always created. `0x06004247` invokes virtual initialization and loop-entry slots, then an underlying host method. We did not prove the underlying OS event pump or every intervening error condition.

**RECONSTRUCTED (semantic aliases):** host type `0x0200095e`, request `0x060046ad`, commit `0x060046b0`, progress-update `0x060046a3`. These names describe inspected dataflow and are not original unobfuscated identifiers. CONFIRMED enum `osu.OsuModes` at `0x02000b4b` has Menu=0, Edit=1, Play=2, Exit=3, SelectPlay=5, Rank=7, Update=8, Busy=9, Unknown=10 and other values through Charts=23. Full mapping is in JSON.

| Semantic alias (RECONSTRUCTED) | Original field token | Evidence |
|---|---|---|
| pending mode | `0x04002c6c` | written by request; read by commit; cleared to Unknown on matching current mode |
| current mode | `0x04002c6d` | assigned pending mode before constructor dispatch |
| previous mode | `0x04002c70` | assigned old current mode before replacement |
| active mode object | `0x04002c6e` | disposed/cleared, replaced by constructor, registered and initialized |
| numeric transition phase | `0x04002c6b` | request writes 0; progress routine handles 0/1/2; phase-completion writes 3/2 |
| transition progress `p` | `0x04002c6a` | double, threshold tests at 0/100, bounded addition/subtraction |
| normalized update scale `s` | `0x04002c22` | double; timing routine divides elapsed value by 16.666666666666668 |
| outgoing/incoming rates | `0x04002c7e` / `0x04002c7f` | multiplied by update scale in progress routine |
| transition callbacks | `0x04002c77` / `0x04002c76` | EventHandler invocation on outgoing/incoming completion |

**CONFIRMED (static request projection):** `0x060046ad(OsuModes, bool)` normalizes Unknown to current mode and sets the bool true. It contains early-return/redirect guards; after those it suppresses requests equal to current or pending mode unless the bool is true, and suppresses requests while pending is Exit. If accepted, it writes pending mode and phase 0. `force` is only an explanatory alias for that bool, not permission to bypass every guard. A separate overload `0x060046ae(OsuModes,bool,int)` queues a closure; callback `0x06004711` writes pending mode and calls commit with its stored integer. Scheduler threading, units and queue policy are UNRESOLVED; 170 is a literal in a deferral expression, not a measured delay.

**CONFIRMED (static callback chain):** host constructor references handler `0x060046ef` and passes its EventHandler to `0x060046b5`; that adder combines it into `0x04002c77`. The handler calls commit with integer zero. Host update method `0x0600469e` directly calls progress-update at code offset `0xd7`. This establishes syntactic wiring, not proof that a particular input reaches it.

**CONFIRMED (commit projection):** commit returns immediately when pending=Unknown. On its normal path, previous=current and current=pending are assigned before replacement construction. An existing active object is removed/disposed immediately for a nonpositive integer parameter, or a cleanup callback is queued for a positive parameter; the active field is then cleared. Positive values do not imply synchronous disposal. Exit has a separate host-exit path. A 24-entry switch dispatches on current mode at code offset `0x255`.

**CONFIRMED (Play branch, including an important edge case):** switch entry Play=2 targets code offset `0x2dd`. It has FOUR constructor alternatives, in this priority order:

1. If previous=MatchSetup(12) OR reference field `0x040014a8` is non-null: constructor `0x060023f0`, with value from `0x04001aad`.
2. Else if bool `0x04002c73`: constructor `0x06000715`.
3. Else if predicate `0x06004390()` OR bool `0x04002a80` OR bit 2048 in field `0x04000cc6`: constructor `0x06004ab9`.
4. Else: constructor `0x0600222e`.

The last type is `0x0200047b`; the other three are subclasses of it. Names such as multiplayer, replay, autoplay, benchmark or standard-ruleset are NOT assigned to these alternatives without further evidence. The `Play` application mode does not itself identify the gameplay ruleset. `0x06000715` sets one field to -1 then invokes `0x0600222e`; `0x06004ab9` invokes that base constructor. These are distinct paths, not a single unconditional gameplay constructor.

**CONFIRMED (initialization order on common non-exit path):** active object is added to collection `host.0x04002c9f` at code offset `0x55a`, then virtual slot `0x06001ab1` is called at `0x564`. Only afterward does commit clear pending to Unknown, and only if pending still equals current. Reentrant mode requests therefore must not be overwritten by unconditional clearing. Base Play type overrides initialization at `0x06002237` and update at `0x0600226b` (virtual, not new-slot metadata); its initialization references additional state and subscribes the same handler `0x06002239` via two host methods. Subclass overrides mean the Play base initializer is not automatically the first or only virtual target. Exact map parser/object-factory/audio startup boundaries remain UNRESOLVED.

**CONFIRMED (finite-value progress arithmetic):** phase 0 tests `p>=100` BEFORE adding; only that threshold branch writes phase 3 and invokes the outgoing callback. Otherwise it assigns `min(100,p + outgoing_rate*s)`. Phase 1 tests `p<=0` BEFORE subtracting; only that threshold branch writes phase 2 and invokes incoming callback. Otherwise it assigns `max(-1,p - incoming_rate*s)`. Reaching a threshold by arithmetic does not itself take the threshold branch in that same invocation. IEEE NaN/unordered branch handling is not covered by this simplified finite-value description. The routine also updates other fields and calls other systems; it is not wholly represented here. Presentation meaning as a fade is RECONSTRUCTED, not visually observed. Millisecond units for the source clock are HYPOTHESIS; the normalization literal and arithmetic are static facts.

## Interface, dependencies and edges

Research interface for Work 2: mode enum, pending/current/previous, active object identity, numeric phase/progress, rate and update-scale values, callback invocation, constructor identity, collection registration and virtual initialization boundary. Outputs are state changes and potential callback/call edges, not pixels or gameplay judgments. Inputs include request enum/bool, commit integer, global predicates/references/flags and host timing state. Initial state, thread ownership, full guard meanings and exception outcomes remain UNRESOLVED. Always qualify these interfaces with build hash and confidence; this expedition supplies no production API.

Competing explanations tested: DLL-separated gameplay logic versus EXE-hosted inspected logic (metadata distinguishes these); immediate request-as-construction versus request/commit split (separate CIL methods distinguish these); one Play constructor versus conditional variants (switch paths distinguish these); threshold callback on crossing versus on later threshold test (branch order distinguishes these). Dynamic execution could still expose untraversed branches, delegate rebinding, native/reflection paths or build changes.

## Validation for Work 2 and next observations

Static reference assertions actually checked: entry-to-startup edge; enum Play=2/Unknown=10; Play switch destination; presence of all four constructor targets; Add before virtual Initialize; three direct commit callers; zero branch-target decoding failures. Repeating the analyser should reproduce JSON and selected body hashes exactly.

Expected reference cases below are derived from inspected branches, NOT executed gameplay tests:

| Case / preconditions | Expected result to verify on original |
|---|---|
| commit with pending Unknown | immediate return; no replacement |
| accepted request equals current, bool false, all earlier guards permit | return before pending write |
| Play with all special conditions false | `0x0600222e`; common registration before virtual initialization |
| Play with previous MatchSetup | `0x060023f0`, regardless of later flags |
| outgoing phase 0, p=99, rate=10, s=1, finite | first call p=100 without threshold callback; next call phase 3 and callback |
| incoming phase 1, p=1, rate=10, s=1, finite | first call p=-1; next call phase 2 and callback |
| initializer changes pending to another mode | final pending/current check must preserve different pending value |

Smallest next experiment: on the pinned Windows build, break/log request, progress callback, commit, selected constructor and virtual initialization, recording the fields above, managed thread IDs and input timestamps during one control-map entry. Capture pending/current before and after initialization to test reentrancy. First identify the semantic origins of the four Play selectors and trace from `0x06002237` to the map-loading boundary. Do not substitute a guessed map parser or runtime. Windows execution and native/display evidence remain unavailable here.

# 3. Pseudocode

The following is an implementation-independent, partial projection for finite inputs. Opaque operations deliberately preserve unresolved dependencies; it is not replacement runtime code.

```text
CONFIRMED [static structure; not observed execution]:
    CLR entry -> startup -> conditional launch preparation
    launch path may construct host and invoke virtual host initialization

RECONSTRUCTED [semantic aliases over confirmed field accesses]:
    Request(target, flag):
        if target == Unknown: target = current; flag = true
        apply ORIGINAL_UNRESOLVED_GUARDS_AND_REDIRECTS_OR_RETURN
        if not flag and target in {current, pending}: return
        if pending == Exit: return
        ORIGINAL_ACCEPTED_REQUEST_SIDE_EFFECTS
        pending = target
        phase = 0

CONFIRMED [partial finite-value progress projection]:
    if phase == 1:
        if p <= 0: phase = 2; invoke incoming callback if present
        else: p = max(-1, p - incoming_rate * update_scale)
    else if phase == 0:
        if p >= 100: phase = 3; invoke outgoing callback if present
        else: p = min(100, p + outgoing_rate * update_scale)
    OTHER_ORIGINAL_PROGRESS_SIDE_EFFECTS
    # outgoing handler includes Commit(0)

RECONSTRUCTED [commit projection; aliases, not a complete implementation]:
    Commit(cleanup_parameter):
        if pending == Unknown: return
        ORIGINAL_PRE_COMMIT_SIDE_EFFECTS
        previous = current
        current = pending
        remove/dispose old object now OR queue cleanup, depending on parameter
        active = null
        ORIGINAL_CALLBACKS_AND_MODE_SPECIFIC_SIDE_EFFECTS
        if current == Exit: follow original host-exit branch
        else:
            if current == Play:
                choose constructor by ordered four-way branch described above
            else: follow original 24-way mode dispatch
            ORIGINAL_COMMON_SETUP
            add active to host collection
            invoke active virtual initialization slot
        if pending == current: pending = Unknown

UNRESOLVED:
    full request guard semantics; scheduler units/thread policy
    actual UI-to-request path; map/audio/object initialization boundaries
    runtime reachability, original capture timings and gameplay equivalence
```
