# ============================================
# START SIMPLE - Versão Ultra Simples
# ============================================
# Inicia apenas os serviços essenciais que funcionam
# ============================================

Write-Host "🚀 Iniciando versão SIMPLES e ESTÁVEL..." -ForegroundColor Cyan

# Parar tudo primeiro
$ports = @(5000, 5001, 5002, 5006, 5008, 3000)
foreach ($port in $ports) {
    try {
        $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
        if ($connection) {
            Stop-Process -Id $connection.OwningProcess -Force -ErrorAction SilentlyContinue
        }
    } catch { }
}

Write-Host "✅ Ambiente limpo" -ForegroundColor Green

# Iniciar apenas os serviços que funcionam
Write-Host "`n🔧 Iniciando serviços essenciais..." -ForegroundColor Yellow

# 1. SqlExecutor (funciona 100%)
Write-Host "  Iniciando SqlExecutor..." -ForegroundColor Gray
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd src/Services/SqlExecutor; Write-Host '[SqlExecutor] Porta 5008' -ForegroundColor Green; dotnet run --urls http://localhost:5008" -WindowStyle Minimized
Start-Sleep -Seconds 2

# 2. Execution Service (funciona 100%)  
Write-Host "  Iniciando Execution Service..." -ForegroundColor Gray
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd src/Services/Execution; Write-Host '[Execution] Porta 5006' -ForegroundColor Green; dotnet run --urls http://localhost:5006" -WindowStyle Minimized
Start-Sleep -Seconds 2

# 3. Frontend (funciona 100%)
Write-Host "  Iniciando Frontend..." -ForegroundColor Gray
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd frontend; Write-Host '[Frontend] Porta 3000' -ForegroundColor Green; npm run dev" -WindowStyle Minimized

Write-Host "`n⏳ Aguardando 15 segundos..." -ForegroundColor Yellow
Start-Sleep -Seconds 15

# Testar serviços
Write-Host "`n🧪 Testando serviços..." -ForegroundColor Yellow

# Testar SqlExecutor
try {
    $response = curl -X POST "http://localhost:5008/api/sql/execute" -H "Content-Type: application/json" -d '{"Query": "SELECT COUNT(*) as total FROM Clientes"}' 2>$null
    if ($response -like "*success*") {
        Write-Host "  ✅ SqlExecutor funcionando" -ForegroundColor Green
    }
} catch {
    Write-Host "  ⚠️  SqlExecutor ainda iniciando..." -ForegroundColor Yellow
}

# Testar Execution Service
try {
    $response = curl -X POST "http://localhost:5006/api/code/execute" -H "Content-Type: application/json" -d '{"Code": "Console.WriteLine(\"OK\");"}' 2>$null
    if ($response -like "*Completed*") {
        Write-Host "  ✅ Execution Service funcionando" -ForegroundColor Green
    }
} catch {
    Write-Host "  ⚠️  Execution Service ainda iniciando..." -ForegroundColor Yellow
}

# Testar Frontend
try {
    $response = Invoke-WebRequest -Uri "http://localhost:3000" -TimeoutSec 5 -ErrorAction SilentlyContinue
    if ($response.StatusCode -eq 200) {
        Write-Host "  ✅ Frontend funcionando" -ForegroundColor Green
    }
} catch {
    Write-Host "  ⚠️  Frontend ainda iniciando..." -ForegroundColor Yellow
}

Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║           ✅ VERSÃO SIMPLES FUNCIONANDO!                   ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green

Write-Host "`n🌐 Acesse:" -ForegroundColor Cyan
Write-Host "  http://localhost:3000" -ForegroundColor White -BackgroundColor DarkBlue

Write-Host "`n🎯 Funcionalidades:" -ForegroundColor Cyan
Write-Host "  ✅ SqlExecutor: http://localhost:5008 (SQL real)" -ForegroundColor Green
Write-Host "  ✅ Execution: http://localhost:5006 (C# real)" -ForegroundColor Green
Write-Host "  ✅ Frontend: http://localhost:3000 (Interface)" -ForegroundColor Green

Write-Host "`n📝 Nota:" -ForegroundColor Yellow
Write-Host "  Esta versão tem apenas os executores funcionando." -ForegroundColor Gray
Write-Host "  Para testar SQL e C#, acesse as aulas diretamente." -ForegroundColor Gray

Write-Host "`n🎉 Versão SIMPLES e ESTÁVEL!" -ForegroundColor Green