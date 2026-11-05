# Independent controls only. No media resources are opened by the Windows suite.
param([string]$ResultPath='', [switch]$SkipWindows)
$ErrorActionPreference='Stop'
$repoPath=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$tempBase=[IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$packRoot=Join-Path $tempBase ('ohno-qa11-packs-'+[Guid]::NewGuid().ToString('N'))
[IO.Directory]::CreateDirectory($packRoot)|Out-Null
if(!$ResultPath){$ResultPath=Join-Path $repoPath 'qa11-results.local.json'}
$headCode=1; $windowsCode=0
try {
    foreach($label in @('a','b')){
        $pack=Join-Path $packRoot $label
        & (Join-Path $repoPath 'reconstructed/GenerateIndependentHitSounds.ps1') -PackDirectory $pack
        & (Join-Path $repoPath 'reconstructed/GenerateIndependentSliderTicks.ps1') -PackDirectory $pack
        $baseline=@{}
        foreach($file in Get-ChildItem -LiteralPath $pack -Filter '*.wav'){$baseline[$file.Name]=(Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()}
        [IO.File]::WriteAllText((Join-Path $pack 'baseline15.json'),($baseline|ConvertTo-Json))
        & (Join-Path $repoPath 'reconstructed/GenerateIndependentSliderLoops.ps1') -PackDirectory $pack
    }
    & dotnet run --project (Join-Path $PSScriptRoot 'Headless/QA11.Headless.csproj') -c Release -- $repoPath (Join-Path $packRoot 'a') (Join-Path $packRoot 'b') $ResultPath
    $headCode=$LASTEXITCODE
    if(!$SkipWindows){
        & dotnet run --project (Join-Path $PSScriptRoot 'Windows/QA11.Windows.csproj') -c Release -- ($ResultPath+'.windows.json')
        $windowsCode=$LASTEXITCODE
    }
} finally {
    $target=[IO.Path]::GetFullPath($packRoot)
    if(!$target.StartsWith($tempBase,[StringComparison]::OrdinalIgnoreCase) -or ![IO.Path]::GetFileName($target).StartsWith('ohno-qa11-packs-')){throw 'Refuse cleanup outside own temporary directory.'}
    Remove-Item -LiteralPath $target -Recurse -Force
}
if($headCode -ne 0 -or $windowsCode -ne 0){exit 1}
exit 0
