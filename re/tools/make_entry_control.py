#!/usr/bin/env python3
"""Generate original, unvalidated inputs; no game behaviour is implemented.

Writes a three-circle map and six seconds of deterministic PCM silence.
Run in a fresh directory; never overwrites files.
"""
import hashlib
import io
import json
from pathlib import Path
import sys
import wave

MAP = """osu file format v14

[General]
AudioFilename: silence.wav
AudioLeadIn: 0
PreviewTime: -1
Countdown: 0
SampleSet: Normal
StackLeniency: 0.7
Mode: 0
LetterboxInBreaks: 0
WidescreenStoryboard: 0

[Editor]
DistanceSpacing: 1
BeatDivisor: 4
GridSize: 4
TimelineZoom: 1

[Metadata]
Title:Entry Baseline Control
Artist:Project Original Fixture
Creator:Work 1
Version:Three Circles - Unvalidated
Source:
Tags:baseline original fixture
BeatmapID:0
BeatmapSetID:-1

[Difficulty]
HPDrainRate:0
CircleSize:4
OverallDifficulty:5
ApproachRate:5
SliderMultiplier:1.4
SliderTickRate:1

[Events]

[TimingPoints]
0,500,4,1,0,100,1,0

[HitObjects]
256,192,2000,1,0,0:0:0:0:
128,192,3000,1,0,0:0:0:0:
384,192,4000,1,0,0:0:0:0:
"""


def generate(directory):
    directory.mkdir(parents=True, exist_ok=True)
    names = ["entry-control.osu", "silence.wav", "input-hashes.json"]
    if any((directory / n).exists() for n in names):
        raise FileExistsError("Use a fresh output directory")
    buffer = io.BytesIO()
    with wave.open(buffer, "wb") as audio:
        audio.setnchannels(1)
        audio.setsampwidth(2)
        audio.setframerate(44100)
        audio.writeframes(b"\0\0" * (44100 * 6))
    hashes = {}
    for name, data in [(names[0], MAP.encode()), (names[1], buffer.getvalue())]:
        with (directory / name).open("xb") as output:
            output.write(data)
        hashes[name] = {"bytes": len(data), "sha256": hashlib.sha256(data).hexdigest()}
    with (directory / names[2]).open("x") as output:
        json.dump(hashes, output, indent=2, sort_keys=True)
        output.write("\n")
    return hashes


if __name__ == "__main__":
    print(json.dumps(generate(Path(sys.argv[1])), indent=2, sort_keys=True))
