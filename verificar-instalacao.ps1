# Script de verificação pós-instalação
Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║           VERIFICAÇÃO DE INSTALAÇÃO                       ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan

Write-Host "`n🔍 Verificando serviços..." -ForegroundColor Yellow

$services = @(
    @{ Name = "ApiGateway"; Url = "http://localhost:5000/health"; Port = 5000 },
    @{ Name = "Auth"; Url = "http://localhost:5001/health"; Port = 5001 },
    @{ Name = "Course"; Url = "http://localhost:5002/health"; Port = 5002 },
    @{ Name = "Progress"; Url = "http://localhost:5003/health"; Port = 5003 },
    @{ Name = "Challenge"; Url = "http://localhost:5004/health"; Port = 5004 },
    @{ Name = "AITutor"; Url = "http://localhost:5005/health"; Port = 5005 },
    @{ Name = "Execution"; Url = "http://localhost:5006/health"; Port = 5006 },
    @{ Name = "SqlExecutor"; Url = "http://localhost:5008/health"; Port = 5008 },
    @{ Name = "Notification"; Url = "http://localhost:5009/health"; Port = 5009 },
    @{ Name = "Analytics"; Url = "http://localhost:5010/health"; Port = 5010 },
    @{ Name = "Frontend"; Url = "http://localhost:3000"; Port = 3000 }
)

$healthy = 0
$unhealthy = 0
$notRunning = 0

foreach ($service in $services) {
    Write-Host "`n[$($service.Name)]" -ForegroundColor Cyan
    
    # Verificar se a porta está em uso
    $connection = Get-NetTCPConnection -LocalPort $service.Port -ErrorAction SilentlyContinue
    
    if (!$connection) {
        Write-Host "  ❌ Não está rodando (porta $($service.Port) livre)" -ForegroundColor Red
        $notRunning++
        continue
    }
    
    # Verificar health check
    try {
        $response = Invoke-WebRequest -Uri $service.Url -TimeoutSec 5 -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            Write-Host "  ✅ Saudável (porta $($service.Port))" -ForegroundColor Green
            $healthy++
        } else {
            Write-Host "  ⚠️  Rodando mas não saudável (status $($response.StatusCode))" -ForegroundColor Yellow
            $unhealthy++
        }
    } catch {
        Write-Host "  ⚠️  Rodando mas health check falhou" -ForegroundColor Yellow
        $unhealthy++
    }
}

# Resumo
Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                    RESUMO                                  ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan

$total = $services.Count
Write-Host "`n📊 Status dos Serviços:" -ForegroundColor Cyan
Write-Host "  ✅ Saudáveis: $healthy/$total" -ForegroundColor Green
Write-Host "  ⚠️  Com problemas: $unhealthy/$total" -ForegroundColor Yellow
Write-Host "  ❌ Não rodando: $notRunning/$total" -ForegroundColor Red

if ($healthy -eq $total) {
    Write-Host "`n🎉 PERFEITO! Todos os serviços estão rodando!" -ForegroundColor Green
    Write-Host "`n🌐 Acesse: http://localhost:3000" -ForegroundColor Cyan
    Write-Host "👤 Login: test@test.com / Test123!" -ForegroundColor Cyan
} elseif ($healthy -ge ($total * 0.8)) {
    Write-Host "`n✅ BOM! Maioria dos serviços rodando." -ForegroundColor Green
    Write-Host "⚠️  Alguns serviços podem estar iniciando ainda." -ForegroundColor Yellow
    Write-Host "Aguarde mais alguns segundos e execute novamente." -ForegroundColor Gray
} else {
    Write-Host "`n❌ PROBLEMA! Muitos serviços não estão rodando." -ForegroundColor Red
    Write-Host "`n🔧 Soluções:" -ForegroundColor Yellow
    Write-Host "  1. Aguarde mais tempo (serviços podem estar iniciando)" -ForegroundColor Gray
    Write-Host "  2. Execute: ./cleanup-no-docker.ps1" -ForegroundColor Gray
    Write-Host "  3. Execute: ./setup-no-docker.ps1" -ForegroundColor Gray
    Write-Host "  4. Verifique os logs nas janelas do PowerShell" -ForegroundColor Gray
}

Write-Host ""
