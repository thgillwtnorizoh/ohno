"""Contract tests against static predictions, never original-game observations."""

import json
import math
from pathlib import Path
import unittest

from reconstructed.transition_progress import Completion, StepResult, step


class TransitionProgressTests(unittest.TestCase):
    def call(self, phase, initial_progress, **changes):
        inputs = dict(phase=phase, progress=initial_progress, outgoing_rate=10,
                      incoming_rate=10, update_scale=1)
        inputs.update(changes)
        return step(**inputs)

    def test_static_contract_cases(self):
        fixture = json.loads((Path(__file__).parents[1] / 'fixtures' /
                              'r01-finite-cases.json').read_text())
        self.assertEqual(fixture['research_commit'],
                         '586ed1d668d535533cd5822cf0fdd6790b61d5d0')
        for case in fixture['cases']:
            with self.subTest(case=case['id']):
                calls = []
                result = step(**case['input'], on_complete=calls.append)
                expected = case['expected']
                self.assertEqual(result.phase, expected['phase'])
                self.assertEqual(result.progress, expected['progress'])
                self.assertEqual(result.completion.value if result.completion
                                 else None, expected['completion'])
                self.assertEqual(calls, [result] if result.completion else [])

    def test_two_invocation_sequences(self):
        for phase, initial, crossing, terminal, event in (
            (0, 99, 100, 3, Completion.OUTGOING),
            (1, 1, -1, 2, Completion.INCOMING),
        ):
            with self.subTest(phase=phase):
                calls = []
                first = self.call(phase, initial, on_complete=calls.append)
                self.assertEqual(first, StepResult(phase, crossing, None))
                self.assertEqual(calls, [])
                second = self.call(first.phase, first.progress,
                                   on_complete=calls.append)
                self.assertEqual(second, StepResult(terminal, crossing, event))
                self.assertEqual(calls, [second])
                # Terminal-phase processing is deliberately outside R01.
                with self.assertRaises(ValueError):
                    self.call(second.phase, second.progress,
                              on_complete=calls.append)
                self.assertEqual(calls, [second])

    def test_thresholds_precede_arithmetic(self):
        # Finite operands whose product would overflow must not affect a
        # threshold completion (including a negative/zero update scale).
        for phase, p, terminal in ((0, 100, 3), (1, 0, 2)):
            for scale in (1e308, -1e308, 0):
                with self.subTest(phase=phase, scale=scale):
                    result = self.call(phase, p, outgoing_rate=1e308,
                                       incoming_rate=1e308, update_scale=scale)
                    self.assertEqual((result.phase, result.progress),
                                     (terminal, p))

    def test_adjacent_float_thresholds(self):
        below = math.nextafter(100.0, -math.inf)
        above = math.nextafter(0.0, math.inf)
        self.assertEqual(self.call(0, below, update_scale=0),
                         StepResult(0, below, None))
        self.assertEqual(self.call(1, above, update_scale=0),
                         StepResult(1, above, None))

    def test_unused_rate_does_not_change_arithmetic(self):
        self.assertEqual(self.call(0, 10, incoming_rate=-1e308),
                         self.call(0, 10, incoming_rate=1e308))
        self.assertEqual(self.call(1, 10, outgoing_rate=-1e308),
                         self.call(1, 10, outgoing_rate=1e308))

    def test_no_callback_is_required(self):
        self.assertEqual(self.call(0, 100).completion, Completion.OUTGOING)
        self.assertEqual(self.call(1, 0).completion, Completion.INCOMING)

    def test_callback_receives_final_result_once(self):
        calls = []
        result = self.call(0, 101, on_complete=calls.append)
        self.assertEqual(calls, [StepResult(3, 101, Completion.OUTGOING)])
        self.assertIs(calls[0], result)

    def test_callback_error_propagation_is_api_policy(self):
        calls = []

        def fail(result):
            calls.append(result)
            raise RuntimeError('caller failure')

        with self.assertRaisesRegex(RuntimeError, 'caller failure'):
            self.call(1, 0, on_complete=fail)
        self.assertEqual(calls, [StepResult(2, 0, Completion.INCOMING)])

    def test_unsupported_phases_are_rejected_without_callback(self):
        calls = []
        for phase in (-1, 2, 3, 4, True, 0.0, '0'):
            with self.subTest(phase=phase), self.assertRaises(ValueError):
                self.call(phase, 0, on_complete=calls.append)
        self.assertEqual(calls, [])

    def test_nonfinite_inputs_are_rejected_without_callback(self):
        for field in ('progress', 'outgoing_rate', 'incoming_rate', 'update_scale'):
            for value in (math.nan, math.inf, -math.inf):
                calls = []
                with self.subTest(field=field, value=value):
                    with self.assertRaises(ValueError):
                        self.call(0, 100, **{field: value}, on_complete=calls.append)
                    self.assertEqual(calls, [])

    def test_input_types_are_api_policy(self):
        for value in (True, '1', None, object()):
            with self.subTest(value=value), self.assertRaises(TypeError):
                self.call(0, value)
        with self.assertRaises(ValueError):
            self.call(0, 10 ** 10000)
        with self.assertRaises(TypeError):
            self.call(0, 100, on_complete=7)

    def test_overflow_policy(self):
        # Bounded overflow still clamps; unbounded overflow is outside the API.
        self.assertEqual(self.call(0, 99, outgoing_rate=1e308, update_scale=1e308),
                         StepResult(0, 100, None))
        self.assertEqual(self.call(1, 1, incoming_rate=1e308, update_scale=1e308),
                         StepResult(1, -1, None))
        for phase in (0, 1):
            calls = []
            with self.subTest(phase=phase), self.assertRaises(OverflowError):
                self.call(phase, 50, outgoing_rate=-1e308, incoming_rate=-1e308,
                          update_scale=1e308, on_complete=calls.append)
            self.assertEqual(calls, [])


if __name__ == '__main__':
    unittest.main()
