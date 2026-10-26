# Collection helper only. NOT executed or validated on Windows in Expedition 01.
# Run in Windows PowerShell 5.1 after selecting the display/mode/settings manually.
# Output must be outside the game folder. Does not launch or change osu!.
param(
    [Parameter(Mandatory=$true)][string]$GameRoot,
    [Parameter(Mandatory=$true)][string]$ControlMap,
    [Parameter(Mandatory=$true)][string]$OutputDirectory,
    [string]$AspireMap
)
$ErrorActionPreference = 'Stop'
$root = (Resolve-Path -LiteralPath $GameRoot).Path.TrimEnd('\')
$out = [IO.Path]::GetFullPath($OutputDirectory)
if ($out.StartsWith($root + '\', [StringComparison]::OrdinalIgnoreCase) -or $out -eq $root) {
    throw 'Output must be outside the reference directory.'
}
if (Test-Path -LiteralPath $out) { throw 'Use a fresh output directory.' }
New-Item -ItemType Directory -Path $out | Out-Null
Get-CimInstance Win32_OperatingSystem |
    Select-Object Caption, Version, BuildNumber, OSArchitecture |
    ConvertTo-Json | Set-Content -Encoding UTF8 (Join-Path $out 'windows.json')
Get-CimInstance Win32_VideoController |
    Select-Object Name, DriverVersion, DriverDate, CurrentHorizontalResolution,
        CurrentVerticalResolution, CurrentRefreshRate |
    ConvertTo-Json | Set-Content -Encoding UTF8 (Join-Path $out 'gpu-adapter-reported.json')
# Adapter-reported fields do not establish active-monitor refresh or effective DPI.
$exe = Join-Path $root 'osu!.exe'
(Get-Item -LiteralPath $exe).VersionInfo |
    Select-Object FileVersion, ProductVersion, CompanyName |
    ConvertTo-Json | Set-Content -Encoding UTF8 (Join-Path $out 'version.json')
Get-AuthenticodeSignature -LiteralPath $exe |
    Select-Object Status, StatusMessage, SignerCertificate, TimeStamperCertificate |
    ConvertTo-Json -Depth 4 | Set-Content -Encoding UTF8 (Join-Path $out 'signature.json')
$files = @(Get-ChildItem -LiteralPath $root -File |
    Where-Object { $_.Extension -in '.exe', '.dll', '.cfg' })
$files += Get-Item -LiteralPath $ControlMap
if ($AspireMap) { $files += Get-Item -LiteralPath $AspireMap }
$files | ForEach-Object { Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256 } |
    Export-Csv -NoTypeInformation (Join-Path $out 'hashes.csv')
@'
UNRESOLVED: record active monitor, resolution, effective scaling/DPI, refresh,
game window/fullscreen mode, compatibility mode, limiter, skin, dim, video,
storyboards, offsets, input settings, active mods, capture method/fps/audio path,
and capture loss/duplication. Save options and Windows display screenshots.
Hash map audio, skin and enabled storyboard/video dependencies separately.
Record observed in-game version and compare all EXE/DLL hashes before/after.
Review collection output for local usernames/paths before publishing.
'@ | Set-Content -Encoding UTF8 (Join-Path $out 'manual-fields.txt')
