@echo off
:: Check for permissions
openfiles >nul 2>&1
if %errorlevel% neq 0 (
    echo Requesting administrative privileges...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)
:: Set the working directory
set "scriptDir=%~dp0"

:: Run the PowerShell script with the correct working directory
powershell -NoProfile -ExecutionPolicy Bypass -Command "Set-Location '%scriptDir%'; .\deploy-smartface.ps1"
pause
