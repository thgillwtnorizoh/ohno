# QA15 independent production components only. No Window/show, OS input or media.
param([string]$ResultPath='', [switch]$SkipWindows)
$ErrorActionPreference='Stop'
$repo=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
if(!$ResultPath){$ResultPath=Join-Path ([IO.Path]::GetTempPath()) 'qa15-results.json'}
& python (Join-Path $PSScriptRoot 'build_corpus.py') --check
if($LASTEXITCODE -ne 0){exit $LASTEXITCODE}
& dotnet run --project (Join-Path $PSScriptRoot 'Headless/QA15.Headless.csproj') -c Release -- $repo $ResultPath
$headCode=$LASTEXITCODE
$winCode=0
if(!$SkipWindows){
    & dotnet run --project (Join-Path $PSScriptRoot 'Windows/QA15.Windows.csproj') -c Release -- $repo ($ResultPath+'.windows.json')
    $winCode=$LASTEXITCODE
}
if($headCode -ne 0 -or $winCode -ne 0){exit 1}
exit 0
