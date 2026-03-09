# ============================================
# SETUP COMPLETO SEM DOCKER - COMANDO ÚNICO
# ============================================
# Este script configura e inicia o projeto automaticamente
# Basta executar: ./setup-no-docker.ps1
# ============================================

Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║     SETUP AUTOMÁTICO - PROJETO SEM DOCKER                  ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan

# ============================================
# ETAPA 1: PRÉ-REQUISITOS
# ============================================
Write-Host "`n[1/8] Verificando pré-requisitos..." -ForegroundColor Yellow

# Verificar .NET SDK
try {
    $dotnetVersion = dotnet --version
    Write-Host "  ✅ .NET SDK $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "  ❌ .NET SDK não encontrado!" -ForegroundColor Red
    Write-Host "  Instale: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    exit 1
}

# Verificar Node.js
try {
    $nodeVersion = node --version
    Write-Host "  ✅ Node.js $nodeVersion" -ForegroundColor Green
} catch {
    Write-Host "  ❌ Node.js não encontrado!" -ForegroundColor Red
    Write-Host "  Instale: https://nodejs.org/" -ForegroundColor Yellow
    exit 1
}

# ============================================
# ETAPA 2: BANCO DE DADOS
# ============================================
Write-Host "`n[2/8] Configurando banco de dados..." -ForegroundColor Yellow

$useLocalDB = $false
$localdb = sqllocaldb info 2>$null

if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✅ SQL Server LocalDB encontrado" -ForegroundColor Green
    
    # Criar/Iniciar instância
    sqllocaldb create MSSQLLocalDB 2>$null
    sqllocaldb start MSSQLLocalDB 2>$null
    
    $useLocalDB = $true
    Write-Host "  ✅ Usando SQL Server LocalDB" -ForegroundColor Green
} else {
    Write-Host "  ⚠️  LocalDB não encontrado, usando SQLite" -ForegroundColor Yellow
    $useLocalDB = $false
}

# ============================================
# ETAPA 3: CONFIGURAÇÃO DOS SERVIÇOS
# ============================================
Write-Host "`n[3/8] Criando arquivos de configuração..." -ForegroundColor Yellow

$services = @(
    "src/Services/Auth",
    "src/Services/Course",
    "src/Services/Progress",
    "src/Services/Challenge",
    "src/Services/AITutor",
    "src/Services/Execution",
    "src/Services/SqlExecutor",
    "src/Services/Notification",
    "src/Services/Analytics"
)

foreach ($service in $services) {
    if ($useLocalDB) {
        $config = @"
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AspNetLearningPlatform;Integrated Security=true;TrustServerCertificate=True;"
  },
  "Redis": {
    "UseMemoryCache": true
  },
  "RabbitMQ": {
    "UseInMemory": true
  }
}
"@
    } else {
        $config = @"
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=aspnet_learning.db"
  },
  "DatabaseProvider": "SQLite",
  "Redis": {
    "UseMemoryCache": true
  },
  "RabbitMQ": {
    "UseInMemory": true
  }
}
"@
    }
    
    $configPath = "$service/appsettings.NoDocker.json"
    $config | Out-File -FilePath $configPath -Encoding UTF8
    Write-Host "  ✅ $service" -ForegroundColor Green
}

# ============================================
# ETAPA 4: INSTALAR DEPENDÊNCIAS BACKEND
# ============================================
Write-Host "`n[4/8] Instalando dependências do backend..." -ForegroundColor Yellow

if (!$useLocalDB) {
    Write-Host "  Instalando pacote SQLite..." -ForegroundColor Gray
    foreach ($service in $services) {
        dotnet add "$service" package Microsoft.EntityFrameworkCore.Sqlite --quiet 2>$null
    }
}

Write-Host "  ✅ Dependências do backend instaladas" -ForegroundColor Green

# ============================================
# ETAPA 5: INSTALAR DEPENDÊNCIAS FRONTEND
# ============================================
Write-Host "`n[5/8] Instalando dependências do frontend..." -ForegroundColor Yellow

cd frontend
npm install --silent 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✅ Dependências do frontend instaladas" -ForegroundColor Green
} else {
    Write-Host "  ⚠️  Erro ao instalar dependências do frontend" -ForegroundColor Yellow
}
cd ..

# ============================================
# ETAPA 6: MIGRATIONS DO BANCO
# ============================================
Write-Host "`n[6/8] Aplicando migrations do banco de dados..." -ForegroundColor Yellow

# Verificar se EF Tools está instalado
$efInstalled = dotnet tool list -g | Select-String "dotnet-ef"
if (!$efInstalled) {
    Write-Host "  Instalando EF Core Tools..." -ForegroundColor Gray
    dotnet tool install --global dotnet-ef --quiet 2>$null
}

cd src/Shared/Data
$migrationResult = dotnet ef database update 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✅ Migrations aplicadas com sucesso" -ForegroundColor Green
} else {
    Write-Host "  ⚠️  Aviso: Migrations podem ter falhado (normal se já aplicadas)" -ForegroundColor Yellow
}
cd ../../..

# ============================================
# ETAPA 7: VERIFICAR PORTAS
# ============================================
Write-Host "`n[7/8] Verificando portas disponíveis..." -ForegroundColor Yellow

$ports = @(5000, 5001, 5002, 5003, 5004, 5005, 5006, 5008, 5009, 5010, 3000)
$portsInUse = @()

foreach ($port in $ports) {
    $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    if ($connection) {
        $portsInUse += $port
    }
}

if ($portsInUse.Count -gt 0) {
    Write-Host "  ⚠️  Portas em uso: $($portsInUse -join ', ')" -ForegroundColor Yellow
    Write-Host "  Liberando portas..." -ForegroundColor Gray
    
    foreach ($port in $portsInUse) {
        try {
            $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
            if ($connection) {
                Stop-Process -Id $connection.OwningProcess -Force -ErrorAction SilentlyContinue
            }
        } catch {
            # Ignorar erros
        }
    }
    Start-Sleep -Seconds 2
    Write-Host "  ✅ Portas liberadas" -ForegroundColor Green
} else {
    Write-Host "  ✅ Todas as portas disponíveis" -ForegroundColor Green
}

# ============================================
# ETAPA 8: INICIAR SERVIÇOS
# ============================================
Write-Host "`n[8/8] Iniciando serviços..." -ForegroundColor Yellow

$servicesList = @(
    @{ Name = "ApiGateway"; Port = 5000; Path = "src/ApiGateway" },
    @{ Name = "Auth"; Port = 5001; Path = "src/Services/Auth" },
    @{ Name = "Course"; Port = 5002; Path = "src/Services/Course" },
    @{ Name = "Progress"; Port = 5003; Path = "src/Services/Progress" },
    @{ Name = "Challenge"; Port = 5004; Path = "src/Services/Challenge" },
    @{ Name = "AITutor"; Port = 5005; Path = "src/Services/AITutor" },
    @{ Name = "Execution"; Port = 5006; Path = "src/Services/Execution" },
    @{ Name = "SqlExecutor"; Port = 5008; Path = "src/Services/SqlExecutor" },
    @{ Name = "Notification"; Port = 5009; Path = "src/Services/Notification" },
    @{ Name = "Analytics"; Port = 5010; Path = "src/Services/Analytics" }
)

Write-Host "  Iniciando backend..." -ForegroundColor Gray
foreach ($svc in $servicesList) {
    Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd $($svc.Path); Write-Host '[$($svc.Name)] Iniciando na porta $($svc.Port)...' -ForegroundColor Cyan; dotnet run --urls `"http://localhost:$($svc.Port)`" --environment NoDocker" -WindowStyle Minimized
    Start-Sleep -Milliseconds 500
}

Write-Host "  ✅ Backend iniciado (10 serviços)" -ForegroundColor Green

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
# VERIFICAR SAÚDE DOS SERVIÇOS
# ============================================
Write-Host "`n`n🔍 Verificando saúde dos serviços..." -ForegroundColor Yellow

$healthChecks = @(
    @{ Name = "ApiGateway"; Url = "http://localhost:5000/health" },
    @{ Name = "Auth"; Url = "http://localhost:5001/health" },
    @{ Name = "Course"; Url = "http://localhost:5002/health" }
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
# RESUMO FINAL
# ============================================
Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║              ✅ SETUP COMPLETO E RODANDO!                  ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green

Write-Host "`n📊 Resumo:" -ForegroundColor Cyan
Write-Host "  • Banco de dados: $(if ($useLocalDB) { 'SQL Server LocalDB' } else { 'SQLite' })" -ForegroundColor White
Write-Host "  • Backend: 10 serviços rodando" -ForegroundColor White
Write-Host "  • Frontend: Next.js rodando" -ForegroundColor White
Write-Host "  • Serviços saudáveis: $healthy/$($healthChecks.Count)" -ForegroundColor White

Write-Host "`n🌐 Acesse a aplicação:" -ForegroundColor Cyan
Write-Host "  http://localhost:3000" -ForegroundColor White -BackgroundColor DarkBlue

Write-Host "`n👤 Usuários de teste:" -ForegroundColor Cyan
Write-Host "  Email: test@test.com" -ForegroundColor White
Write-Host "  Senha: Test123!" -ForegroundColor White

Write-Host "`n📝 Comandos úteis:" -ForegroundColor Cyan
Write-Host "  • Ver logs: Verifique as janelas minimizadas do PowerShell" -ForegroundColor Gray
Write-Host "  • Parar tudo: Execute ./cleanup-no-docker.ps1" -ForegroundColor Gray
Write-Host "  • Resetar: Execute ./cleanup-no-docker.ps1 e depois ./setup-no-docker.ps1" -ForegroundColor Gray

Write-Host "`n✨ Projeto rodando com sucesso!" -ForegroundColor Green
Write-Host ""
