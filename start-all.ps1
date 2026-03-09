#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Inicia todo o projeto AspNetLearningPlatform com um único comando

.DESCRIPTION
    Este script:
    1. Carrega variáveis de ambiente do .env
    2. Inicia containers Docker (SQL Server, RabbitMQ, Redis)
    3. Aguarda os serviços ficarem prontos
    4. Compila todos os projetos
    5. Inicia todos os microserviços em paralelo
    6. Exibe URLs de acesso

.PARAMETER SkipBuild
    Pula a etapa de compilação (útil se já compilou recentemente)

.PARAMETER SkipDocker
    Pula a inicialização do Docker (útil se já está rodando)

.PARAMETER ServicesOnly
    Inicia apenas os serviços, sem Docker

.EXAMPLE
    .\start-all.ps1
    Inicia tudo do zero

.EXAMPLE
    .\start-all.ps1 -SkipBuild
    Inicia sem recompilar

.EXAMPLE
    .\start-all.ps1 -ServicesOnly
    Inicia apenas os serviços .NET
#>

param(
    [switch]$SkipBuild,
    [switch]$SkipDocker,
    [switch]$ServicesOnly
)

$ErrorActionPreference = "Stop"

# Cores para output
function Write-ColorOutput($ForegroundColor) {
    $fc = $host.UI.RawUI.ForegroundColor
    $host.UI.RawUI.ForegroundColor = $ForegroundColor
    if ($args) {
        Write-Output $args
    }
    $host.UI.RawUI.ForegroundColor = $fc
}

function Write-Step($message) {
    Write-ColorOutput Green "`n=== $message ==="
}

function Write-Info($message) {
    Write-ColorOutput Cyan "ℹ️  $message"
}

function Write-Success($message) {
    Write-ColorOutput Green "✅ $message"
}

function Write-Warning($message) {
    Write-ColorOutput Yellow "⚠️  $message"
}

function Write-Error($message) {
    Write-ColorOutput Red "❌ $message"
}

# Banner
Clear-Host
Write-ColorOutput Magenta @"
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║     AspNet Learning Platform - Startup Script             ║
║                                                           ║
║     Iniciando todos os serviços...                        ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
"@

# 1. Carregar variáveis de ambiente
Write-Step "1. Carregando variáveis de ambiente"

if (Test-Path .env) {
    Write-Info "Carregando .env..."
    Get-Content .env | ForEach-Object {
        if ($_ -match '^([^=]+)=(.+)$' -and $_ -notmatch '^#') {
            $key = $matches[1].Trim()
            $value = $matches[2].Trim()
            [Environment]::SetEnvironmentVariable($key, $value, "Process")
            Write-Info "  $key = ***"
        }
    }
    Write-Success "Variáveis de ambiente carregadas"
} else {
    Write-Warning ".env não encontrado, usando valores padrão"
}

# 2. Iniciar Docker
if (-not $SkipDocker -and -not $ServicesOnly) {
    Write-Step "2. Iniciando containers Docker"
    
    Write-Info "Verificando se Docker está rodando..."
    try {
        docker ps | Out-Null
        Write-Success "Docker está rodando"
    } catch {
        Write-Error "Docker não está rodando. Inicie o Docker Desktop e tente novamente."
        exit 1
    }

    Write-Info "Iniciando containers (SQL Server, RabbitMQ, Redis)..."
    docker-compose up -d
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Containers iniciados"
    } else {
        Write-Error "Falha ao iniciar containers"
        exit 1
    }

    # Aguardar serviços ficarem prontos
    Write-Info "Aguardando serviços ficarem prontos..."
    Start-Sleep -Seconds 10
    
    # Verificar SQL Server
    Write-Info "Verificando SQL Server..."
    $maxRetries = 60
    $retries = 0
    $sqlReady = $false
    
    while ($retries -lt $maxRetries) {
        try {
            $containerStatus = docker inspect aspnet-learning-sqlserver --format='{{.State.Running}}' 2>&1
            if ($containerStatus -eq "true") {
                # Container está rodando, agora verificar se SQL Server está aceitando conexões
                $testConnection = docker exec aspnet-learning-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "P@ssw0rd!2026#SecurePlatform" -Q "SELECT 1" -C 2>&1
                if ($LASTEXITCODE -eq 0) {
                    Write-Success "SQL Server está pronto e aceitando conexões"
                    $sqlReady = $true
                    break
                }
            }
        } catch {}
        $retries++
        if ($retries % 10 -eq 0) {
            Write-Info "  Ainda aguardando SQL Server... ($retries/$maxRetries)"
        }
        Start-Sleep -Seconds 2
    }
    
    if (-not $sqlReady) {
        Write-Warning "SQL Server pode não estar pronto ainda. Os serviços podem falhar ao iniciar."
        Write-Warning "Aguarde alguns segundos e execute '.\health-check.ps1' para verificar."
    }

    Write-Success "Docker containers prontos"
} else {
    Write-Info "Pulando inicialização do Docker"
}

# 3. Compilar projetos
if (-not $SkipBuild) {
    Write-Step "3. Compilando projetos"
    
    Write-Info "Limpando builds anteriores..."
    dotnet clean --verbosity quiet
    
    Write-Info "Compilando solução..."
    dotnet build --no-incremental --verbosity minimal
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Compilação concluída com sucesso"
    } else {
        Write-Error "Falha na compilação"
        exit 1
    }
} else {
    Write-Info "Pulando compilação"
}

# 4. Parar processos dotnet existentes
Write-Step "4. Limpando processos anteriores"

# Limpar arquivo de PIDs antigo
if (Test-Path ".pids") {
    Remove-Item ".pids" -Force
}

$dotnetProcesses = Get-Process dotnet -ErrorAction SilentlyContinue
if ($dotnetProcesses) {
    Write-Info "Parando processos dotnet anteriores..."
    $dotnetProcesses | Stop-Process -Force
    Start-Sleep -Seconds 2
    Write-Success "Processos anteriores parados"
} else {
    Write-Info "Nenhum processo dotnet anterior encontrado"
}

# 5. Iniciar serviços backend
Write-Step "5. Iniciando microserviços backend"

$services = @(
    @{Name="ApiGateway"; Path="src/ApiGateway/ApiGateway.csproj"; Port=5000; Url="http://localhost:5000"},
    @{Name="Auth"; Path="src/Services/Auth/Auth.Service.csproj"; Port=5001; Url="http://localhost:5001"},
    @{Name="Course"; Path="src/Services/Course/Course.Service.csproj"; Port=5002; Url="http://localhost:5002"},
    @{Name="Progress"; Path="src/Services/Progress/Progress.Service.csproj"; Port=5003; Url="http://localhost:5003"},
    @{Name="Challenge"; Path="src/Services/Challenge/Challenge.Service.csproj"; Port=5004; Url="http://localhost:5004"},
    @{Name="AITutor"; Path="src/Services/AITutor/AITutor.Service.csproj"; Port=5005; Url="http://localhost:5005"},
    @{Name="Execution"; Path="src/Services/Execution/Execution.Service.csproj"; Port=5006; Url="http://localhost:5006"},
    @{Name="Worker"; Path="src/Services/Worker/Worker.Service.csproj"; Port=5007; Url="http://localhost:5007"},
    @{Name="SqlExecutor"; Path="src/Services/SqlExecutor/SqlExecutor.Service.csproj"; Port=5008; Url="http://localhost:5008"},
    @{Name="Notification"; Path="src/Services/Notification/Notification.Service.csproj"; Port=5009; Url="http://localhost:5009"},
    @{Name="Analytics"; Path="src/Services/Analytics/Analytics.Service.csproj"; Port=5010; Url="http://localhost:5010"}
)

foreach ($service in $services) {
    Write-Info "Iniciando $($service.Name) na porta $($service.Port)..."
    
    # Janela fecha automaticamente quando o serviço terminar
    $process = Start-Process powershell -ArgumentList "-Command", "cd '$PWD'; dotnet run --project $($service.Path) --no-build" -WindowStyle Minimized -PassThru
    
    # Salvar PID para poder fechar depois
    Add-Content -Path ".pids" -Value "$($process.Id)"
    
    Start-Sleep -Milliseconds 500
}

Write-Success "Todos os serviços backend foram iniciados"

# 5.1. Iniciar frontend Next.js
Write-Step "5.1. Iniciando frontend Next.js"

Write-Info "Iniciando frontend Next.js na porta 3000..."
# Usar npm run dev para iniciar o frontend corretamente
$frontendProcess = Start-Process powershell -ArgumentList "-Command", "cd '$PWD/frontend'; npm run dev" -WindowStyle Minimized -PassThru

# Salvar PID do frontend
Add-Content -Path ".pids" -Value "$($frontendProcess.Id)"

Write-Success "Frontend Next.js iniciado"

# 6. Aguardar serviços ficarem prontos
Write-Step "6. Aguardando serviços ficarem prontos"
Write-Info "Aguardando 15 segundos para os serviços backend iniciarem..."
Start-Sleep -Seconds 15
Write-Info "Aguardando mais 10 segundos para o frontend Next.js compilar..."
Start-Sleep -Seconds 10

# 7. Verificar health
Write-Step "7. Verificando saúde dos serviços"

$healthyServices = 0
$totalServices = $services.Count

foreach ($service in $services) {
    try {
        $response = Invoke-WebRequest -Uri "$($service.Url)/health" -TimeoutSec 5 -UseBasicParsing -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            Write-Success "$($service.Name) está saudável"
            $healthyServices++
        } else {
            Write-Warning "$($service.Name) respondeu com status $($response.StatusCode)"
        }
    } catch {
        Write-Warning "$($service.Name) não está respondendo ainda (pode levar mais tempo)"
    }
}

# 8. Exibir resumo
Write-Step "8. Resumo"

Write-ColorOutput Cyan @"

╔═══════════════════════════════════════════════════════════╗
║                    SERVIÇOS INICIADOS                     ║
╚═══════════════════════════════════════════════════════════╝

"@

Write-ColorOutput Yellow "  🌐 Frontend (Next.js) → http://localhost:3000"
Write-ColorOutput White ""

foreach ($service in $services) {
    Write-ColorOutput White "  🚀 $($service.Name.PadRight(15)) → $($service.Url)"
}

Write-ColorOutput Cyan @"

╔═══════════════════════════════════════════════════════════╗
║                    INFRAESTRUTURA                         ║
╚═══════════════════════════════════════════════════════════╝

  🗄️ SQL Server      → localhost:1433
  🐰 RabbitMQ        → http://localhost:15672 (platform_user/SimplePass123)
  📦 Redis           → localhost:6379

╔═══════════════════════════════════════════════════════════╗
║                    COMANDOS ÚTEIS                         ║
╚═══════════════════════════════════════════════════════════╝

  Ver logs:          .\logs.ps1
  Parar tudo:        .\stop-all.ps1
  Reiniciar:         .\restart-all.ps1
  Health check:      .\health-check.ps1

╔═══════════════════════════════════════════════════════════╗
║                       STATUS                              ║
╚═══════════════════════════════════════════════════════════╝

"@

Write-ColorOutput Green "  ✅ Serviços saudáveis: $healthyServices/$totalServices"

if ($healthyServices -lt $totalServices) {
    Write-ColorOutput Yellow "  ⚠️  Alguns serviços ainda estão iniciando..."
    Write-ColorOutput Yellow "     Execute '.\health-check.ps1' em alguns segundos"
}

Write-ColorOutput Cyan @"

╔═══════════════════════════════════════════════════════════╗
║                    PRÓXIMOS PASSOS                        ║
╚═══════════════════════════════════════════════════════════╝

  1. 🌐 Acesse o Frontend: http://localhost:3000
  2. 🔧 Acesse o ApiGateway: http://localhost:5000
  3. 📚 Teste os endpoints: http://localhost:5000/swagger
  4. 📊 Monitore os logs: .\logs.ps1

"@

Write-ColorOutput Green @"
✨ Sistema iniciado com sucesso!

Os serviços estão rodando em janelas separadas (minimizadas).
Use '.\stop-all.ps1' para parar todos os serviços.
"@
