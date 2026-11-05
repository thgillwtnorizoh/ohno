# Independent finite headless verification. Generates own PCM; never plays audio.
param([string]$ResultPath = '')
$ErrorActionPreference = 'Stop'
$repoPath = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$tempBase = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$packRoot = Join-Path $tempBase ('ohno-qa10-packs-' + [Guid]::NewGuid().ToString('N'))
[IO.Directory]::CreateDirectory($packRoot) | Out-Null
if (!$ResultPath) { $ResultPath = Join-Path $repoPath 'qa10-results.local.json' }
$exitCode = 1
try {
    foreach ($label in @('a','b')) {
        $pack = Join-Path $packRoot $label
        & (Join-Path $repoPath 'reconstructed/GenerateIndependentHitSounds.ps1') -PackDirectory $pack
        $baseline = @{}
        foreach ($file in Get-ChildItem -LiteralPath $pack -Filter '*.wav') { $baseline[$file.Name] = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant() }
        [IO.File]::WriteAllText((Join-Path $pack 'circle-baseline.json'), ($baseline | ConvertTo-Json))
        & (Join-Path $repoPath 'reconstructed/GenerateIndependentSliderTicks.ps1') -PackDirectory $pack
    }
    & dotnet run --project (Join-Path $PSScriptRoot 'Headless/QA10.Headless.csproj') -c Release -- $repoPath (Join-Path $packRoot 'a') (Join-Path $packRoot 'b') $ResultPath
    $exitCode = $LASTEXITCODE
} finally {
    $resolvedPack = [IO.Path]::GetFullPath($packRoot)
    if (!$resolvedPack.StartsWith($tempBase, [StringComparison]::OrdinalIgnoreCase) -or ![IO.Path]::GetFileName($resolvedPack).StartsWith('ohno-qa10-packs-')) { throw 'Refuse cleanup outside own temporary tree.' }
    Remove-Item -LiteralPath $resolvedPack -Recurse -Force
}
exit $exitCode
