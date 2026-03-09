#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Para todos os serviços do AspNetLearningPlatform

.DESCRIPTION
    Para todos os processos dotnet e containers Docker
#>

$ErrorActionPreference = "Continue"

Write-Host "`n=== Parando todos os serviços ===" -ForegroundColor Green

# Parar processos dotnet
Write-Host "Parando processos .NET..." -ForegroundColor Cyan
$dotnetProcesses = Get-Process dotnet -ErrorAction SilentlyContinue
if ($dotnetProcesses) {
    $dotnetProcesses | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
    Write-Host "✅ Processos .NET parados" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Nenhum processo .NET rodando" -ForegroundColor Yellow
}

# Parar processos Node.js (frontend)
Write-Host "`nParando frontend Next.js..." -ForegroundColor Cyan
$nodeProcesses = Get-Process node -ErrorAction SilentlyContinue
if ($nodeProcesses) {
    $nodeProcesses | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
    Write-Host "✅ Frontend Next.js parado" -ForegroundColor Green
} else {
    Write-Host "ℹ️  Frontend não está rodando" -ForegroundColor Yellow
}

# Fechar janelas do PowerShell que foram abertas pelo start-all.ps1
Write-Host "`nFechando janelas do PowerShell..." -ForegroundColor Cyan

if (Test-Path ".pids") {
    $pids = Get-Content ".pids" -ErrorAction SilentlyContinue
    $closedCount = 0
    
    foreach ($processId in $pids) {
        try {
            $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
            if ($process) {
                Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
                $closedCount++
            }
        } catch {
            # Ignora erros
        }
    }
    
    # Remover arquivo de PIDs
    Remove-Item ".pids" -Force -ErrorAction SilentlyContinue
    
    if ($closedCount -gt 0) {
        Write-Host "✅ $closedCount janela(s) do PowerShell fechada(s)" -ForegroundColor Green
    } else {
        Write-Host "ℹ️  Nenhuma janela do PowerShell para fechar" -ForegroundColor Yellow
    }
} else {
    Write-Host "ℹ️  Nenhuma janela do PowerShell para fechar" -ForegroundColor Yellow
}

# Parar containers Docker
Write-Host "`nParando containers Docker..." -ForegroundColor Cyan
try {
    docker-compose down 2>&1 | Out-Null
    Write-Host "✅ Containers Docker parados" -ForegroundColor Green
} catch {
    Write-Host "⚠️  Erro ao parar containers Docker" -ForegroundColor Yellow
}

Write-Host "`n✨ Todos os serviços foram parados!`n" -ForegroundColor Green
