# Script de teste para verificar setup sem Docker
Write-Host "=== TESTE DE SETUP SEM DOCKER ===" -ForegroundColor Cyan

# 1. Verificar LocalDB
Write-Host "`n1. Testando SQL Server LocalDB..." -ForegroundColor Yellow
try {
    $localdb = sqllocaldb info 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ LocalDB disponível" -ForegroundColor Green
        
        # Testar conexão
        $instances = sqllocaldb info
        Write-Host "Instâncias encontradas: $instances" -ForegroundColor Gray
    } else {
        Write-Host "❌ LocalDB não encontrado" -ForegroundColor Red
        Write-Host "Instale SQL Server Express ou use SQLite" -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ Erro ao verificar LocalDB: $_" -ForegroundColor Red
}

# 2. Verificar .NET SDK
Write-Host "`n2. Testando .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET SDK $dotnetVersion instalado" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET SDK não encontrado" -ForegroundColor Red
    Write-Host "Instale .NET 9 SDK: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
}

# 3. Verificar Node.js
Write-Host "`n3. Testando Node.js..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "✅ Node.js $nodeVersion instalado" -ForegroundColor Green
} catch {
    Write-Host "❌ Node.js não encontrado" -ForegroundColor Red
    Write-Host "Instale Node.js: https://nodejs.org/" -ForegroundColor Yellow
}

# 4. Verificar portas disponíveis
Write-Host "`n4. Verificando portas..." -ForegroundColor Yellow
$ports = @(5000, 5001, 5002, 5003, 5004, 5005, 5006, 5008, 5009, 5010, 3000)
$portsInUse = @()

foreach ($port in $ports) {
    $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    if ($connection) {
        $portsInUse += $port
        Write-Host "⚠️  Porta $port em uso" -ForegroundColor Yellow
    } else {
        Write-Host "✅ Porta $port disponível" -ForegroundColor Green
    }
}

if ($portsInUse.Count -gt 0) {
    Write-Host "`n⚠️  Portas em uso: $($portsInUse -join ', ')" -ForegroundColor Yellow
    Write-Host "Execute: Get-Process -Id (Get-NetTCPConnection -LocalPort PORTA).OwningProcess | Stop-Process" -ForegroundColor Gray
}

# 5. Verificar estrutura do projeto
Write-Host "`n5. Verificando estrutura do projeto..." -ForegroundColor Yellow
$requiredPaths = @(
    "src/Services/Auth",
    "src/Services/Course",
    "src/Services/Progress",
    "src/Services/Challenge",
    "src/Services/AITutor",
    "src/Services/Execution",
    "src/Services/SqlExecutor",
    "src/Services/Notification",
    "src/Services/Analytics",
    "src/ApiGateway",
    "frontend"
)

$missingPaths = @()
foreach ($path in $requiredPaths) {
    if (Test-Path $path) {
        Write-Host "✅ $path" -ForegroundColor Green
    } else {
        $missingPaths += $path
        Write-Host "❌ $path não encontrado" -ForegroundColor Red
    }
}

if ($missingPaths.Count -gt 0) {
    Write-Host "`n❌ Estrutura do projeto incompleta" -ForegroundColor Red
    Write-Host "Caminhos faltando: $($missingPaths -join ', ')" -ForegroundColor Yellow
}

# 6. Resumo
Write-Host "`n=== RESUMO ===" -ForegroundColor Cyan
Write-Host "LocalDB: $(if ($LASTEXITCODE -eq 0) { '✅' } else { '❌' })" -ForegroundColor $(if ($LASTEXITCODE -eq 0) { 'Green' } else { 'Red' })
Write-Host ".NET SDK: ✅" -ForegroundColor Green
Write-Host "Node.js: $(if ($nodeVersion) { '✅' } else { '❌' })" -ForegroundColor $(if ($nodeVersion) { 'Green' } else { 'Red' })
Write-Host "Portas disponíveis: $(if ($portsInUse.Count -eq 0) { '✅' } else { "⚠️  $($portsInUse.Count) em uso" })" -ForegroundColor $(if ($portsInUse.Count -eq 0) { 'Green' } else { 'Yellow' })
Write-Host "Estrutura: $(if ($missingPaths.Count -eq 0) { '✅' } else { '❌' })" -ForegroundColor $(if ($missingPaths.Count -eq 0) { 'Green' } else { 'Red' })

Write-Host "`n=== PRÓXIMOS PASSOS ===" -ForegroundColor Cyan
Write-Host "1. Execute: ./setup-no-docker.ps1" -ForegroundColor White
Write-Host "2. Execute: ./start-services-no-docker.ps1" -ForegroundColor White
Write-Host "3. Em outro terminal: cd frontend && npm run dev" -ForegroundColor White
Write-Host "4. Acesse: http://localhost:3000" -ForegroundColor White
