#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Inicia apenas o frontend Next.js

.DESCRIPTION
    Script para iniciar o frontend Next.js em modo desenvolvimento
#>

$ErrorActionPreference = "Stop"

Write-Host "`n╔═══════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║          Iniciando Frontend Next.js                      ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

# Verificar se node_modules existe
if (-not (Test-Path "frontend/node_modules")) {
    Write-Host "📦 Instalando dependências (primeira vez)..." -ForegroundColor Yellow
    Push-Location frontend
    npm install
    Pop-Location
    Write-Host "✅ Dependências instaladas`n" -ForegroundColor Green
}

# Verificar se o backend está rodando
Write-Host "🔍 Verificando backend..." -ForegroundColor Cyan
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5000/health" -TimeoutSec 3 -UseBasicParsing -ErrorAction SilentlyContinue
    Write-Host "✅ Backend está rodando`n" -ForegroundColor Green
} catch {
    Write-Host "⚠️  Backend não está rodando!" -ForegroundColor Yellow
    Write-Host "   Execute '.\start-all.ps1' para iniciar o backend primeiro`n" -ForegroundColor Yellow
}

# Iniciar frontend
Write-Host "🚀 Iniciando frontend em http://localhost:3000..." -ForegroundColor Green
Write-Host "   Pressione Ctrl+C para parar`n" -ForegroundColor Gray

# Usar node diretamente para evitar problemas com npm
node node_modules/next/dist/bin/next dev
