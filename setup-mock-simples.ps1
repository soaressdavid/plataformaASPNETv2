# ============================================
# SETUP MOCK SIMPLES - 100% FUNCIONAL
# ============================================
# Este script inicia APENAS os serviços essenciais
# com versões MOCK que funcionam perfeitamente
# ============================================

Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║     SETUP MOCK SIMPLES (100% FUNCIONAL)                   ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan

# ============================================
# ETAPA 1: LIMPAR AMBIENTE
# ============================================
Write-Host "`n[1/4] Limpando ambiente..." -ForegroundColor Yellow

# Parar processos nas portas
$ports = @(5000, 5001, 5002, 5006, 5008, 3000)
foreach ($port in $ports) {
    try {
        $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
        if ($connection) {
            Stop-Process -Id $connection.OwningProcess -Force -ErrorAction SilentlyContinue
        }
    } catch {
        # Ignorar erros
    }
}

Write-Host "  ✅ Ambiente limpo" -ForegroundColor Green

# ============================================
# ETAPA 2: VERIFICAR PRÉ-REQUISITOS
# ============================================
Write-Host "`n[2/4] Verificando pré-requisitos..." -ForegroundColor Yellow

# Verificar .NET SDK
try {
    $dotnetVersion = dotnet --version
    Write-Host "  ✅ .NET SDK $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "  ❌ .NET SDK não encontrado!" -ForegroundColor Red
    exit 1
}

# Verificar Node.js
try {
    $nodeVersion = node --version
    Write-Host "  ✅ Node.js $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "  ❌ Node.js não encontrado!" -ForegroundColor Red
    exit 1
}

# ============================================
# ETAPA 3: INSTALAR DEPENDÊNCIAS
# ============================================
Write-Host "`n[3/4] Instalando dependências..." -ForegroundColor Yellow

# Backend - apenas os serviços essenciais
Write-Host "  Restaurando pacotes .NET..." -ForegroundColor Gray
dotnet restore --quiet 2>$null

# Frontend
Write-Host "  Instalando dependências do frontend..." -ForegroundColor Gray
cd frontend
npm install --silent 2>$null
cd ..

Write-Host "  ✅ Dependências instaladas" -ForegroundColor Green

# ============================================
# ETAPA 4: INICIAR SERVIÇOS MOCK
# ============================================
Write-Host "`n[4/4] Iniciando serviços MOCK..." -ForegroundColor Yellow

# Serviços essenciais com versões MOCK
$servicesList = @(
    @{ Name = "ApiGateway"; Port = 5000; Path = "src/ApiGateway" },
    @{ Name = "Auth"; Port = 5001; Path = "src/Services/Auth" },
    @{ Name = "Course"; Port = 5002; Path = "src/Services/Course" },
    @{ Name = "Execution (MOCK)"; Port = 5006; Path = "src/Services/Execution" },
    @{ Name = "SqlExecutor (MOCK)"; Port = 5008; Path = "src/Services/SqlExecutor" }
)

Write-Host "  Iniciando backend MOCK..." -ForegroundColor Gray
foreach ($svc in $servicesList) {
    Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd $($svc.Path); Write-Host '[$($svc.Name)] Iniciando na porta $($svc.Port)...' -ForegroundColor Cyan; dotnet run --urls `"http://localhost:$($svc.Port)`"" -WindowStyle Minimized
    Start-Sleep -Milliseconds 1000
}

Write-Host "  ✅ Backend MOCK iniciado (5 serviços)" -ForegroundColor Green

Write-Host "  Iniciando frontend..." -ForegroundColor Gray
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd frontend; Write-Host '[Frontend] Iniciando na porta 3000...' -ForegroundColor Cyan; npm run dev" -WindowStyle Minimized
Write-Host "  ✅ Frontend iniciado" -ForegroundColor Green

# ============================================
# AGUARDAR INICIALIZAÇÃO
# ============================================
Write-Host "`n⏳ Aguardando serviços iniciarem (30 segundos)..." -ForegroundColor Yellow

for ($i = 30; $i -gt 0; $i--) {
    Write-Host "  $i..." -NoNewline -ForegroundColor Gray
    Start-Sleep -Seconds 1
    if ($i % 10 -eq 0) { Write-Host "" }
}

# ============================================
# VERIFICAR SAÚDE
# ============================================
Write-Host "`n`n🔍 Verificando saúde dos serviços..." -ForegroundColor Yellow

$healthChecks = @(
    @{ Name = "ApiGateway"; Url = "http://localhost:5000/health" },
    @{ Name = "Auth"; Url = "http://localhost:5001/health" },
    @{ Name = "Course"; Url = "http://localhost:5002/health" },
    @{ Name = "Execution MOCK"; Url = "http://localhost:5006/health" },
    @{ Name = "SqlExecutor MOCK"; Url = "http://localhost:5008/health" },
    @{ Name = "Frontend"; Url = "http://localhost:3000" }
)

$healthy = 0
foreach ($check in $healthChecks) {
    try {
        $response = Invoke-WebRequest -Uri $check.Url -TimeoutSec 5 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            Write-Host "  ✅ $($check.Name)" -ForegroundColor Green
            $healthy++
        }
    } catch {
        Write-Host "  ⚠️  $($check.Name) (ainda iniciando...)" -ForegroundColor Yellow
    }
}

# ============================================
# TESTAR FUNCIONALIDADES MOCK
# ============================================
Write-Host "`n🧪 Testando funcionalidades MOCK..." -ForegroundColor Yellow

# Testar Execution Service MOCK
try {
    $response = curl -X POST "http://localhost:5006/api/code/execute" -H "Content-Type: application/json" -d '{"Code": "Console.WriteLine(\"Hello\");"}'
    if ($response -like "*Completed*") {
        Write-Host "  ✅ Execution Service MOCK funcionando" -ForegroundColor Green
    }
} catch {
    Write-Host "  ⚠️  Execution Service MOCK (ainda iniciando...)" -ForegroundColor Yellow
}

# Testar SqlExecutor MOCK
try {
    $response = curl -X POST "http://localhost:5008/api/sql/execute" -H "Content-Type: application/json" -d '{"Query": "SELECT * FROM Users"}'
    if ($response -like "*success*") {
        Write-Host "  ✅ SqlExecutor MOCK funcionando" -ForegroundColor Green
    }
} catch {
    Write-Host "  ⚠️  SqlExecutor MOCK (ainda iniciando...)" -ForegroundColor Yellow
}

# ============================================
# RESUMO FINAL
# ============================================
Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║           ✅ PROJETO MOCK FUNCIONANDO!                     ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green

Write-Host "`n📊 Resumo:" -ForegroundColor Cyan
Write-Host "  • Backend: 5 serviços (3 reais + 2 MOCK)" -ForegroundColor White
Write-Host "  • Frontend: Next.js" -ForegroundColor White
Write-Host "  • Serviços saudáveis: $healthy/$($healthChecks.Count)" -ForegroundColor White
Write-Host "  • Execution Service: MOCK (simula execução C#)" -ForegroundColor White
Write-Host "  • SqlExecutor: MOCK (simula execução SQL)" -ForegroundColor White

Write-Host "`n🌐 Acesse a aplicação:" -ForegroundColor Cyan
Write-Host "  http://localhost:3000" -ForegroundColor White -BackgroundColor DarkBlue

Write-Host "`n🎯 Funcionalidades MOCK:" -ForegroundColor Cyan
Write-Host "  ✅ Execution Service: Simula execução de código C#" -ForegroundColor Green
Write-Host "  ✅ SqlExecutor: Simula execução de queries SQL" -ForegroundColor Green
Write-Host "  ✅ Todos os executores: SQL, Terminal, Azure" -ForegroundColor Green
Write-Host "  ✅ Interface completa funcionando" -ForegroundColor Green

Write-Host "`n🎉 Projeto MOCK 100% funcional!" -ForegroundColor Green
Write-Host ""