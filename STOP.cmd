@echo off
REM ============================================================================
REM  AspNet Learning Platform - Parar com Duplo Clique
REM ============================================================================

echo.
echo ╔═══════════════════════════════════════════════════════════╗
echo ║                                                           ║
echo ║     Parando todos os serviços...                          ║
echo ║                                                           ║
echo ╚═══════════════════════════════════════════════════════════╝
echo.

REM Verificar se PowerShell está disponível
where pwsh >nul 2>nul
if %ERRORLEVEL% EQU 0 (
    pwsh -ExecutionPolicy Bypass -File "%~dp0stop-all.ps1"
) else (
    where powershell >nul 2>nul
    if %ERRORLEVEL% EQU 0 (
        powershell -ExecutionPolicy Bypass -File "%~dp0stop-all.ps1"
    ) else (
        echo ERRO: PowerShell não encontrado!
        pause
        exit /b 1
    )
)

echo.
echo Pressione qualquer tecla para fechar...
pause >nul
