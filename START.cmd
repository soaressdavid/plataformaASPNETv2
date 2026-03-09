@echo off
REM ============================================================================
REM  AspNet Learning Platform - Iniciar com Duplo Clique
REM ============================================================================

echo.
echo ╔═══════════════════════════════════════════════════════════╗
echo ║                                                           ║
echo ║     AspNet Learning Platform - Startup                    ║
echo ║                                                           ║
echo ╚═══════════════════════════════════════════════════════════╝
echo.

REM Verificar se PowerShell está disponível
where pwsh >nul 2>nul
if %ERRORLEVEL% EQU 0 (
    echo Iniciando com PowerShell 7...
    pwsh -ExecutionPolicy Bypass -File "%~dp0start-all.ps1"
) else (
    where powershell >nul 2>nul
    if %ERRORLEVEL% EQU 0 (
        echo Iniciando com Windows PowerShell...
        powershell -ExecutionPolicy Bypass -File "%~dp0start-all.ps1"
    ) else (
        echo ERRO: PowerShell não encontrado!
        echo Por favor, instale o PowerShell.
        pause
        exit /b 1
    )
)

pause
