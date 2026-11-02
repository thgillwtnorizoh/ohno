@echo off
setlocal
where dotnet >nul 2>nul
if errorlevel 1 (
  echo Install the .NET 8 SDK for Windows, then run this launcher again.
  pause
  exit /b 1
)
pushd "%~dp0"
dotnet run --project Ohno.CircleGame.csproj -c Release -- %*
set "ohno_exit=%ERRORLEVEL%"
popd
if not "%ohno_exit%"=="0" pause
exit /b %ohno_exit%
