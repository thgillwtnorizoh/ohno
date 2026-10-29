# Explicit Windows-only launch of the disposable copy. Does not import/judge/capture.
# NOT RUN ON WINDOWS in E20. Use a fresh bundle/snapshot for matched runs.
param(
    [Parameter(Mandatory=$true)][string]$RunDirectory,
    [Parameter(Mandatory=$true)][switch]$AuthorizedCopy,
    [Parameter(Mandatory=$true)][switch]$DisposableOffline
)
$ErrorActionPreference = 'Stop'
if ([Environment]::OSVersion.Platform -ne [PlatformID]::Win32NT) { throw 'NOT RUN - WINDOWS HOST REQUIRED' }
if (-not $AuthorizedCopy -or -not $DisposableOffline) { throw 'Authorized disposable offline Windows environment must be established first.' }
$run = (Resolve-Path -LiteralPath $RunDirectory).Path
$game = Join-Path $run 'game'
$exe = Join-Path $game 'osu!.exe'
$expected = '6e182c10d1813209d12753dbc70b3a5bba00fef4ecf64bc42051870e6dfe4b7d'
if ((Get-FileHash -LiteralPath $exe -Algorithm SHA256).Hash.ToLowerInvariant() -ne $expected) { throw 'Pinned executable mismatch.' }
$baseline = Get-Content -LiteralPath (Join-Path $run 'baseline-files.json') -Raw | ConvertFrom-Json
foreach ($item in $baseline) {
    if ($item.relative_path -match '\.(exe|dll)$') {
        $p = Join-Path $run $item.relative_path
        if ((Get-FileHash -LiteralPath $p -Algorithm SHA256).Hash.ToLowerInvariant() -ne $item.sha256) { throw 'Original executable/library changed.' }
    }
}
$record = Join-Path $run 'private_artifacts\launch-attempt.json'
if (Test-Path -LiteralPath $record) { throw 'Launch attempt already recorded; use a fresh matched run bundle.' }
$attempt = [ordered]@{ status='LAUNCH_ATTEMPT_ONLY'; utc=[DateTime]::UtcNow.ToString('o'); process_id=$null; error=$null; gameplay_entry='UNRESOLVED' }
$attempt | ConvertTo-Json | Set-Content -LiteralPath $record -Encoding UTF8
try {
    $process = Start-Process -FilePath $exe -WorkingDirectory $game -PassThru
    $attempt.process_id = $process.Id
    $attempt.status = 'PROCESS_START_RETURNED_NOT_GAMEPLAY_PROOF'
} catch {
    $attempt.status = 'LAUNCH_ERROR'; $attempt.error = $_.Exception.Message
    $attempt | ConvertTo-Json | Set-Content -LiteralPath $record -Encoding UTF8
    throw
}
$attempt | ConvertTo-Json | Set-Content -LiteralPath $record -Encoding UTF8
Write-Output ('Process ID: ' + $process.Id + '. Import, readiness and original observations still require operator evidence.')
