@echo off
:: Check for permissions
openfiles >nul 2>&1
if %errorlevel% neq 0 (
    echo Requesting administrative privileges...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)
:: Run the PowerShell script
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0deploy-smartface.ps1"
pause