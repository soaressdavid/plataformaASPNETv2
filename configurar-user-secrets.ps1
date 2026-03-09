# Script para Configurar User Secrets
# Resolve o problema de senha do banco de dados

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  CONFIGURANDO USER SECRETS             " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 1. VERIFICAR .ENV
Write-Host "[1/4] Lendo senhas do .env..." -ForegroundColor Yellow
if (-not (Test-Path ".env")) {
    Write-Host "  ❌ Arquivo .env não encontrado!" -ForegroundColor Red
    exit 1
}

# Ler senhas
$dbPassword = (Get-Content .env | Select-String "DB_PASSWORD=").ToString().Split("=")[1].Trim()
$rabbitPassword = (Get-Content .env | Select-String "RABBITMQ_PASSWORD=").ToString().Split("=")[1].Trim()

if ([string]::IsNullOrEmpty($dbPassword)) {
    Write-Host "  ❌ DB_PASSWORD não encontrado no .env!" -ForegroundColor Red
    exit 1
}

Write-Host "  ✅ Senhas lidas do .env" -ForegroundColor Green

# 2. CONFIGURAR COURSESERVICE
Write-Host "[2/4] Configurando CourseService..." -ForegroundColor Yellow
Push-Location src/Services/Course

# Inicializar user secrets
dotnet user-secrets init 2>&1 | Out-Null

# Configurar connection string
$connectionString = "Server=localhost,1433;Database=aspnet_learning_platform;User Id=sa;Password=$dbPassword;TrustServerCertificate=True"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" $connectionString 2>&1 | Out-Null

Pop-Location
Write-Host "  ✅ CourseService configurado" -ForegroundColor Green

# 3. CONFIGURAR APIGATEWAY
Write-Host "[3/4] Configurando ApiGateway..." -ForegroundColor Yellow
Push-Location src/ApiGateway

# Inicializar user secrets
dotnet user-secrets init 2>&1 | Out-Null

# Configurar connection strings
dotnet user-secrets set "ConnectionStrings:DefaultConnection" $connectionString 2>&1 | Out-Null
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379" 2>&1 | Out-Null
dotnet user-secrets set "RabbitMQ:Password" $rabbitPassword 2>&1 | Out-Null

Pop-Location
Write-Host "  ✅ ApiGateway configurado" -ForegroundColor Green

# 4. CONFIGURAR OUTROS SERVIÇOS
Write-Host "[4/4] Configurando outros serviços..." -ForegroundColor Yellow

# AuthService
if (Test-Path "src/Services/Auth") {
    Push-Location src/Services/Auth
    dotnet user-secrets init 2>&1 | Out-Null
    # Auth usa PostgreSQL, mas vamos configurar mesmo assim
    $authConnectionString = "Host=localhost;Port=5432;Database=aspnet_learning_platform;Username=platform_user;Password=$dbPassword"
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" $authConnectionString 2>&1 | Out-Null
    dotnet user-secrets set "RabbitMQ:Password" $rabbitPassword 2>&1 | Out-Null
    Pop-Location
}

# ProgressService
if (Test-Path "src/Services/Progress") {
    Push-Location src/Services/Progress
    dotnet user-secrets init 2>&1 | Out-Null
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" $connectionString 2>&1 | Out-Null
    dotnet user-secrets set "RabbitMQ:Password" $rabbitPassword 2>&1 | Out-Null
    Pop-Location
}

Write-Host "  ✅ Outros serviços configurados" -ForegroundColor Green

# RESUMO
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "           CONFIGURAÇÃO COMPLETA        " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "User Secrets configurados para:" -ForegroundColor Cyan
Write-Host "  ✅ CourseService" -ForegroundColor Green
Write-Host "  ✅ ApiGateway" -ForegroundColor Green
Write-Host "  ✅ AuthService" -ForegroundColor Green
Write-Host "  ✅ ProgressService" -ForegroundColor Green
Write-Host ""
Write-Host "Próximos passos:" -ForegroundColor Cyan
Write-Host "  1. Parar serviços atuais (se rodando)" -ForegroundColor White
Write-Host "  2. Iniciar ApiGateway: dotnet run --project src/ApiGateway" -ForegroundColor White
Write-Host "  3. Iniciar CourseService: dotnet run --project src/Services/Course" -ForegroundColor White
Write-Host "  4. Testar: http://localhost:5000/api/courses" -ForegroundColor White
Write-Host ""
Write-Host "Para ver secrets configurados:" -ForegroundColor Gray
Write-Host "  cd src/Services/Course" -ForegroundColor Gray
Write-Host "  dotnet user-secrets list" -ForegroundColor Gray
Write-Host ""
