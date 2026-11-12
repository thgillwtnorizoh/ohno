# Independent finite QA14. No gameplay Window/show, OS input or nonempty media.
param([string]$ResultPath='', [switch]$SkipWindows)
$ErrorActionPreference='Stop'
$repo=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
if(!$ResultPath){$ResultPath=Join-Path ([IO.Path]::GetTempPath()) 'qa14-results.json'}
& dotnet run --project (Join-Path $PSScriptRoot 'Headless/QA14.Headless.csproj') -c Release -- $repo $ResultPath
$headCode=$LASTEXITCODE
$winCode=0
if(!$SkipWindows){
    & dotnet run --project (Join-Path $PSScriptRoot 'Windows/QA14.Windows.csproj') -c Release -- $repo ($ResultPath+'.windows.json')
    $winCode=$LASTEXITCODE
}
if($headCode -ne 0 -or $winCode -ne 0){exit 1}
exit 0
