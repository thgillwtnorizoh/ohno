# Independent R12 tick-only companion pack. No sample input or playback.
# Add these three files beside the unchanged twelve R11 practice WAVs.
param([Parameter(Mandatory=$true)][string]$PackDirectory, [switch]$Force)
$ErrorActionPreference = 'Stop'
$packRoot = [IO.Path]::GetFullPath($PackDirectory)
$banks = @('normal','soft','drum')
foreach ($bank in $banks) {
    $file = Join-Path $packRoot "$bank-slidertick.wav"
    if ((Test-Path $file) -and !$Force) { throw "Already exists: $file. Choose a different directory or -Force." }
}
[IO.Directory]::CreateDirectory($packRoot) | Out-Null
$count = 5292
for ($bankIndex=0; $bankIndex -lt 3; $bankIndex++) {
    $frequency = @(1000,800,400)[$bankIndex]
    $path = Join-Path $packRoot "$($banks[$bankIndex])-slidertick.wav"
    $writer = [IO.BinaryWriter]::new([IO.File]::Create($path))
    try {
        $writer.Write([Text.Encoding]::ASCII.GetBytes('RIFF')); $writer.Write([int](36+2*$count))
        $writer.Write([Text.Encoding]::ASCII.GetBytes('WAVEfmt ')); $writer.Write([int]16)
        $writer.Write([int16]1); $writer.Write([int16]1); $writer.Write([int]44100)
        $writer.Write([int]88200); $writer.Write([int16]2); $writer.Write([int16]16)
        $writer.Write([Text.Encoding]::ASCII.GetBytes('data')); $writer.Write([int](2*$count))
        for ($i=0; $i -lt $count; $i++) {
            $time = $i/44100.0
            $envelope = [Math]::Min(1,$i/40.0)*[Math]::Exp(-$time*90)*[Math]::Min(1,($count-1-$i)/80.0)
            $writer.Write([int16](8500*$envelope*[Math]::Sin(2*[Math]::PI*$frequency*$time)))
        }
    } finally { $writer.Dispose() }
}
Write-Output "R12 tick pack: 3 independent PCM WAV files written in $packRoot; no playback."
