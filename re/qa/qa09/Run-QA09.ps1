# Independent headless verification only. Generates own tones; never plays audio.
param([string]$ResultPath = '')
$ErrorActionPreference = 'Stop'
$repoPath = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$tempBase = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$packRoot = Join-Path $tempBase ('ohno-qa09-packs-' + [Guid]::NewGuid().ToString('N'))
[IO.Directory]::CreateDirectory($packRoot) | Out-Null
if (!$ResultPath) { $ResultPath = Join-Path $packRoot 'qa09-results.json' }
$env:QA09_POWERSHELL = (Get-Process -Id $PID).Path
$exitCode = 1
try {
    & (Join-Path $repoPath 'reconstructed/GenerateIndependentHitSounds.ps1') -PackDirectory (Join-Path $packRoot 'a')
    & (Join-Path $repoPath 'reconstructed/GenerateIndependentHitSounds.ps1') -PackDirectory (Join-Path $packRoot 'b')
    & dotnet run --project (Join-Path $PSScriptRoot 'Headless/QA09.Headless.csproj') -c Release -- $repoPath (Join-Path $packRoot 'a') (Join-Path $packRoot 'b') $ResultPath
    $exitCode = $LASTEXITCODE
} finally {
    # Never delete a user-supplied path. Only this run's checked temporary tree.
    $resolvedPack = [IO.Path]::GetFullPath($packRoot)
    if (!$resolvedPack.StartsWith($tempBase, [StringComparison]::OrdinalIgnoreCase) -or
        ![IO.Path]::GetFileName($resolvedPack).StartsWith('ohno-qa09-packs-')) {
        throw 'Refuse QA09 cleanup outside its own temporary directory.'
    }
    Remove-Item -LiteralPath $resolvedPack -Recurse -Force
}
exit $exitCode
