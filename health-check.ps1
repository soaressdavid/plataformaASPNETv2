#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Verifica a saúde de todos os serviços

.DESCRIPTION
    Testa os endpoints /health de todos os microserviços
#>

$ErrorActionPreference = "Continue"

Write-Host "`n╔═══════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║              HEALTH CHECK - Todos os Serviços             ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

$services = @(
    @{Name="Frontend (Next.js)"; Url="http://localhost:3000"},
    @{Name="ApiGateway"; Url="http://localhost:5000/health"},
    @{Name="Auth Service"; Url="http://localhost:5001/health"},
    @{Name="Course Service"; Url="http://localhost:5002/health"},
    @{Name="Progress Service"; Url="http://localhost:5003/health"},
    @{Name="Challenge Service"; Url="http://localhost:5004/health"},
    @{Name="AITutor Service"; Url="http://localhost:5005/health"},
    @{Name="Execution Service"; Url="http://localhost:5006/health"}
)

$healthyCount = 0
$unhealthyCount = 0

foreach ($service in $services) {
    Write-Host "Verificando $($service.Name.PadRight(25))... " -NoNewline
    
    try {
        $response = Invoke-WebRequest -Uri $service.Url -TimeoutSec 5 -UseBasicParsing -ErrorAction Stop
        
        if ($response.StatusCode -eq 200) {
            Write-Host "✅ Saudável" -ForegroundColor Green
            $healthyCount++
        } else {
            Write-Host "⚠️  Status: $($response.StatusCode)" -ForegroundColor Yellow
            $unhealthyCount++
        }
    } catch {
        Write-Host "❌ Não disponível" -ForegroundColor Red
        $unhealthyCount++
    }
}

# Verificar Docker
Write-Host "`n--- Infraestrutura Docker ---`n" -ForegroundColor Cyan

Write-Host "SQL Server                   ... " -NoNewline
try {
    $containerStatus = docker inspect aspnet-learning-sqlserver --format='{{.State.Health.Status}}' 2>&1
    if ($containerStatus -eq "healthy") {
        Write-Host "✅ Saudável" -ForegroundColor Green
    } elseif ($containerStatus -eq "starting") {
        Write-Host "⚠️  Iniciando..." -ForegroundColor Yellow
    } else {
        Write-Host "❌ Não saudável" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Não disponível" -ForegroundColor Red
}

Write-Host "RabbitMQ                     ... " -NoNewline
try {
    $containerStatus = docker inspect aspnet-learning-rabbitmq --format='{{.State.Health.Status}}' 2>&1
    if ($containerStatus -eq "healthy") {
        Write-Host "✅ Saudável" -ForegroundColor Green
    } elseif ($containerStatus -eq "starting") {
        Write-Host "⚠️  Iniciando..." -ForegroundColor Yellow
    } else {
        Write-Host "❌ Não saudável" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Não disponível" -ForegroundColor Red
}

Write-Host "Redis                        ... " -NoNewline
try {
    $containerStatus = docker inspect aspnet-learning-redis --format='{{.State.Health.Status}}' 2>&1
    if ($containerStatus -eq "healthy") {
        Write-Host "✅ Saudável" -ForegroundColor Green
    } elseif ($containerStatus -eq "starting") {
        Write-Host "⚠️  Iniciando..." -ForegroundColor Yellow
    } else {
        Write-Host "❌ Não saudável" -ForegroundColor Red
    }
} catch {
    Write-Host "❌ Não disponível" -ForegroundColor Red
}

# Verificar Worker Service (background service)
Write-Host "`n--- Background Services ---`n" -ForegroundColor Cyan

Write-Host "Worker Service               ... " -NoNewline
$workerProcess = Get-Process -Name "Worker.Service" -ErrorAction SilentlyContinue
if ($workerProcess) {
    Write-Host "✅ Rodando (PID: $($workerProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "❌ Não disponível" -ForegroundColor Red
}

# Resumo
Write-Host "`n╔═══════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                         RESUMO                            ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

$totalServices = $services.Count
Write-Host "  Serviços saudáveis:   $healthyCount/$totalServices" -ForegroundColor $(if ($healthyCount -eq $totalServices) { "Green" } else { "Yellow" })
Write-Host "  Serviços com problema: $unhealthyCount/$totalServices" -ForegroundColor $(if ($unhealthyCount -eq 0) { "Green" } else { "Red" })

if ($healthyCount -eq $totalServices) {
    Write-Host "`n✨ Todos os serviços estão saudáveis!`n" -ForegroundColor Green
} else {
    Write-Host "`n⚠️  Alguns serviços precisam de atenção`n" -ForegroundColor Yellow
}
