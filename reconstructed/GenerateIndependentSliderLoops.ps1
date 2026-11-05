# Independently authored mathematical PCM loops. No original assets or playback.
param([Parameter(Mandatory=$true)][string]$PackDirectory, [switch]$Force)
$ErrorActionPreference = 'Stop'
$packRoot = [IO.Path]::GetFullPath($PackDirectory)
[IO.Directory]::CreateDirectory($packRoot) | Out-Null
$banks = @('normal','soft','drum')
$families = @('sliderslide','sliderwhistle')
foreach ($bank in $banks) { foreach ($family in $families) {
    $path = Join-Path $packRoot "$bank-$family.wav"
    if ((Test-Path $path) -and !$Force) { throw "Already exists: $path" }
} }
$count = 22050
for ($b=0; $b -lt 3; $b++) { for ($r=0; $r -lt 2; $r++) {
    $path = Join-Path $packRoot "$($banks[$b])-$($families[$r]).wav"
    $frequency = @(120,180,240)[$b] * ($r+1)
    $writer = [IO.BinaryWriter]::new([IO.File]::Create($path))
    try {
        $writer.Write([Text.Encoding]::ASCII.GetBytes('RIFF')); $writer.Write([int](36+2*$count))
        $writer.Write([Text.Encoding]::ASCII.GetBytes('WAVEfmt ')); $writer.Write([int]16)
        $writer.Write([int16]1); $writer.Write([int16]1); $writer.Write([int]44100)
        $writer.Write([int]88200); $writer.Write([int16]2); $writer.Write([int16]16)
        $writer.Write([Text.Encoding]::ASCII.GetBytes('data')); $writer.Write([int](2*$count))
        for ($i=0; $i -lt $count; $i++) {
            $phase = 2*[Math]::PI*$frequency*$i/44100.0
            $writer.Write([int16](4500*[Math]::Sin($phase)+1200*[Math]::Sin(2*$phase)))
        }
    } finally { $writer.Dispose() }
} }
Write-Output 'R13: six independent 0.5-second PCM loops generated; no playback.'
