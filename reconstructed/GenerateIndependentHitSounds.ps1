# Original, independently synthesized practice tones. No osu! assets are used.
# Writes files only; never plays audio or operates the application.
param([Parameter(Mandatory=$true)][string]$PackDirectory, [switch]$Force)
$ErrorActionPreference = 'Stop'
$packRoot = [IO.Path]::GetFullPath($PackDirectory)
$banks = @('normal','soft','drum')
$layers = @('hitnormal','hitfinish','hitwhistle','hitclap')
foreach ($bank in $banks) { foreach ($layer in $layers) {
    $file = Join-Path $packRoot "$bank-$layer.wav"
    if ((Test-Path $file) -and !$Force) { throw "Already exists: $file. Use a different directory or -Force." }
} }
[IO.Directory]::CreateDirectory($packRoot) | Out-Null
$sampleRate = 44100
$count = 5292 # 120ms, ordinary PCM16 mono; comfortably above1024 bytes.
for ($bankIndex=0; $bankIndex -lt 3; $bankIndex++) {
    for ($layerIndex=0; $layerIndex -lt 4; $layerIndex++) {
        $frequency = @(720,520,160)[$bankIndex] + @(0,240,600,80)[$layerIndex]
        $path = Join-Path $packRoot "$($banks[$bankIndex])-$($layers[$layerIndex]).wav"
        $writer = [IO.BinaryWriter]::new([IO.File]::Create($path))
        try {
            $writer.Write([Text.Encoding]::ASCII.GetBytes('RIFF')); $writer.Write([int](36+2*$count))
            $writer.Write([Text.Encoding]::ASCII.GetBytes('WAVEfmt ')); $writer.Write([int]16)
            $writer.Write([int16]1); $writer.Write([int16]1); $writer.Write([int]$sampleRate)
            $writer.Write([int](2*$sampleRate)); $writer.Write([int16]2); $writer.Write([int16]16)
            $writer.Write([Text.Encoding]::ASCII.GetBytes('data')); $writer.Write([int](2*$count))
            for ($i=0; $i -lt $count; $i++) {
                $time = $i/[double]$sampleRate
                $envelope = [Math]::Min(1,$i/80.0)*[Math]::Exp(-$time*45)*[Math]::Min(1,($count-1-$i)/80.0)
                $writer.Write([int16](8500*$envelope*[Math]::Sin(2*[Math]::PI*$frequency*$time)))
            }
        } finally { $writer.Dispose() }
    }
}
Write-Output "Created 12 independent PCM WAV practice tones in $packRoot. No audio was played."
@{Enabled=$true; LayeredHitSounds=$true; SkinSamples=$true; SkinName='Default'; SkinDirectory=$null;
  DefaultPackDirectory=$packRoot; VolumeEffect=80; VolumeUniversal=100} | ConvertTo-Json
