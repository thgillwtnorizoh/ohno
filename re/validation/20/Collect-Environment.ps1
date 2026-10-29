# Preparation helper; NOT RUN ON WINDOWS in E20. No game method calls or networking.
param(
    [Parameter(Mandatory=$true)][string]$RunDirectory,
    [ValidatePattern('^[A-Za-z0-9_-]+$')][string]$Label = 'pre',
    [int]$GameProcessId = 0
)
$ErrorActionPreference = 'Stop'
if ([Environment]::OSVersion.Platform -ne [PlatformID]::Win32NT) {
    throw 'NOT RUN - WINDOWS HOST REQUIRED'
}
$run = (Resolve-Path -LiteralPath $RunDirectory).Path
$out = Join-Path $run ('private_artifacts\environment-' + $Label + '.json')
if (Test-Path -LiteralPath $out) { throw 'Use a fresh collection label; evidence is not overwritten.' }
function Read-Safe([scriptblock]$Body) {
    try { return (& $Body) } catch { return @{ status='UNRESOLVED'; error=$_.Exception.Message } }
}
$info = [ordered]@{
    kind = 'WINDOWS_ENVIRONMENT_INVENTORY_ONLY'
    utc = [DateTime]::UtcNow.ToString('o')
    powershell = $PSVersionTable.PSVersion.ToString()
    collector_qpc_frequency = [Diagnostics.Stopwatch]::Frequency
    collector_qpc_ticks = [Diagnostics.Stopwatch]::GetTimestamp().ToString()
    collector_process_bits = [IntPtr]::Size * 8
    windows = Read-Safe { Get-CimInstance Win32_OperatingSystem | Select-Object Caption,Version,BuildNumber,OSArchitecture }
    video_adapter_reported = Read-Safe { @(Get-CimInstance Win32_VideoController | Select-Object Name,DriverVersion,DriverDate,CurrentHorizontalResolution,CurrentVerticalResolution,CurrentRefreshRate) }
    sound_devices_not_active_endpoint = Read-Safe { @(Get-CimInstance Win32_SoundDevice | Select-Object Name,Manufacturer,Status) }
    dotnet_installed_not_process_runtime = Read-Safe { Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' | Select-Object Release,Version }
    configs = Read-Safe { @(Get-ChildItem -LiteralPath (Join-Path $run 'game') -Filter '*.cfg' -File | ForEach-Object { Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256 }) }
    reference_binaries = Read-Safe { @(Get-ChildItem -LiteralPath (Join-Path $run 'game') -File | Where-Object { $_.Extension -in '.exe','.dll' } | ForEach-Object { Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256 }) }
    process = $null
    actual_clr_modules = $null
    limits = 'Adapter reports do not prove active monitor/DPI/refresh. Sound inventory does not identify active endpoint. Installed .NET does not prove actual target CLR. Complete environment.manual.json; no gameplay operands collected here.'
}
if ($GameProcessId -gt 0) {
    $info.process = Read-Safe { Get-Process -Id $GameProcessId | Select-Object Id,ProcessName,StartTime,Path }
    $info.actual_clr_modules = Read-Safe {
        @((Get-Process -Id $GameProcessId).Modules | Where-Object { $_.ModuleName -match '^(clr|mscorwks)\.dll$' } | ForEach-Object {
            @{ path=$_.FileName; version=$_.FileVersionInfo.FileVersion; sha256=(Get-FileHash -LiteralPath $_.FileName -Algorithm SHA256).Hash }
        })
    }
}
$info | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $out -Encoding UTF8
Write-Output 'Environment inventory saved privately. Gameplay validation remains unestablished.'
