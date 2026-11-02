#!/usr/bin/env python3
"""QA01 static test runner. Never launches either game or generates OS input."""
import argparse
import json
import os
from pathlib import Path
import shutil
import sys
import unittest


class RecordedResult(unittest.TestResult):
    def __init__(self):
        super().__init__()
        self.records = []

    def addSuccess(self, test):
        super().addSuccess(test)
        self.records.append({'test': test.id(), 'status': 'PASS'})

    def addFailure(self, test, err):
        super().addFailure(test, err)
        self.records.append({'test': test.id(), 'status': 'FAIL', 'detail': str(err[1])})

    def addError(self, test, err):
        super().addError(test, err)
        self.records.append({'test': test.id(), 'status': 'ERROR', 'detail': str(err[1])})

    def addSkip(self, test, reason):
        super().addSkip(test, reason)
        self.records.append({'test': test.id(), 'status': 'SKIP', 'detail': reason})


def main():
    a = argparse.ArgumentParser(description=__doc__)
    a.add_argument('--archive', type=Path, required=True)
    a.add_argument('--output', type=Path, required=True)
    args = a.parse_args()
    os.environ['QA01_ARCHIVE'] = str(args.archive.resolve())
    here = Path(__file__).resolve().parent
    result = RecordedResult()
    unittest.defaultTestLoader.discover(str(here), pattern='test_static.py').run(result)
    out = {
        'qa': 'QA01', 'target_commit': '140f2db00a513221dde4d00cd9897663635375c1',
        'category': 'STATIC_SOURCE_AND_ORIGINAL_BYTES', 'tests': result.records,
        'summary': {'run': result.testsRun, 'passed': sum(x['status'] == 'PASS' for x in result.records),
                    'failed': len(result.failures), 'errors': len(result.errors), 'skipped': len(result.skipped)},
        'headless_csharp_execution': {'status': 'NOT RUN', 'reason':
                                     '.NET SDK unavailable in this QA host' if shutil.which('dotnet') is None
                                     else 'Separate headless suite is not executed by this static runner',
                                     'tests_executed': 0, 'dotnet_on_path': shutil.which('dotnet') is not None},
        'windows_launch': 'UNVERIFIED — owner operation only',
        'visual_presentation': 'UNVERIFIED — no owner capture supplied',
        'physical_input_gameplay': 'UNVERIFIED — owner operation only',
        'original_runtime': 'NOT RUN; E20 remains incomplete',
        'limitations': ['No R02 C# method executed by these tests',
                        'Bounded source checks are not a general C# compiler or behavioral test substitute',
                        'Headless C# behavioral suite provided separately; do not infer it passed',
                        'No desktop, OS input, GUI application, capture or original executable operation']}
    args.output.write_text(json.dumps(out, indent=2, sort_keys=True) + '\n')
    print(json.dumps(out['summary'], sort_keys=True))
    for t in result.records:
        if t['status'] != 'PASS':
            print(t['status'], t['test'], t.get('detail', ''))
    return 0 if result.wasSuccessful() else 1


if __name__ == '__main__':
    sys.exit(main())
