# QA12 independent finite verification. No game, decoder or audio device is opened.
param([string]$ResultPath='', [switch]$SkipWindows)
$ErrorActionPreference='Stop'
$repo=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$packRoot=Join-Path ([IO.Path]::GetTempPath()) ('ohno-qa12-'+[Guid]::NewGuid().ToString('N'))
if(!$ResultPath){$ResultPath=Join-Path ([IO.Path]::GetTempPath()) 'qa12-results.json'}
[IO.Directory]::CreateDirectory($packRoot)|Out-Null
$headCode=1;$winCode=0
try {
    foreach($label in @('a','b')){
        $pack=Join-Path $packRoot $label
        & (Join-Path $repo 'reconstructed/GenerateIndependentHitSounds.ps1') -PackDirectory $pack
        & (Join-Path $repo 'reconstructed/GenerateIndependentSliderTicks.ps1') -PackDirectory $pack
        & (Join-Path $repo 'reconstructed/GenerateIndependentSliderLoops.ps1') -PackDirectory $pack
        $baseline=@{}
        foreach($file in Get-ChildItem -LiteralPath $pack -Filter '*.wav'){$baseline[$file.Name]=(Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()}
        if($baseline.Count -ne 21){throw 'Expected21 prior files.'}
        [IO.File]::WriteAllText((Join-Path $pack 'baseline21.json'),($baseline|ConvertTo-Json))
        & (Join-Path $repo 'reconstructed/GenerateIndependentSpinnerSounds.ps1') -PackDirectory $pack
    }
    & dotnet run --project (Join-Path $PSScriptRoot 'Headless/QA12.Headless.csproj') -c Release -- $repo $ResultPath (Join-Path $packRoot 'a') (Join-Path $packRoot 'b')
    $headCode=$LASTEXITCODE
    if(!$SkipWindows){
        & dotnet run --project (Join-Path $PSScriptRoot 'Windows/QA12.Windows.csproj') -c Release -- ($ResultPath+'.windows.json')
        $winCode=$LASTEXITCODE
    }
}finally{
    if(![IO.Path]::GetFileName($packRoot).StartsWith('ohno-qa12-')){throw 'Unsafe cleanup target.'}
    Remove-Item -LiteralPath $packRoot -Recurse -Force
}
if($headCode -ne 0 -or $winCode -ne 0){exit 1}
exit 0
