# Independently authored PCM tones. No sample inputs, decoder or playback.
param([Parameter(Mandatory=$true)][string]$PackDirectory, [switch]$Force)
$ErrorActionPreference = 'Stop'
$packRoot = [IO.Path]::GetFullPath($PackDirectory)
[IO.Directory]::CreateDirectory($packRoot) | Out-Null
$names = @('spinnerspin','spinnerbonus')
foreach ($name in $names) {
    $path = Join-Path $packRoot "$name.wav"
    if ((Test-Path $path) -and !$Force) { throw "Already exists: $path" }
}
for ($r=0; $r -lt 2; $r++) {
    $count = @(22050,13230)[$r]
    $frequency = @(160,880)[$r]
    $writer = [IO.BinaryWriter]::new([IO.File]::Create((Join-Path $packRoot "$($names[$r]).wav")))
    try {
        $writer.Write([Text.Encoding]::ASCII.GetBytes('RIFF')); $writer.Write([int](36+2*$count))
        $writer.Write([Text.Encoding]::ASCII.GetBytes('WAVEfmt ')); $writer.Write([int]16)
        $writer.Write([int16]1); $writer.Write([int16]1); $writer.Write([int]44100)
        $writer.Write([int]88200); $writer.Write([int16]2); $writer.Write([int16]16)
        $writer.Write([Text.Encoding]::ASCII.GetBytes('data')); $writer.Write([int](2*$count))
        for ($i=0; $i -lt $count; $i++) {
            $time = $i/44100.0
            $envelope = if ($r -eq 0) { 1.0 } else { [Math]::Min(1,$i/80.0)*[Math]::Exp(-$time*14)*[Math]::Min(1,($count-1-$i)/80.0) }
            $writer.Write([int16](6000*$envelope*[Math]::Sin(2*[Math]::PI*$frequency*$time)))
        }
    } finally { $writer.Dispose() }
}
Write-Output 'R14: two independent spinner PCM sounds written; no playback.'
