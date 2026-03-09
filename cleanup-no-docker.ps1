# Script para limpar ambiente sem Docker
Write-Host "=== LIMPEZA DE AMBIENTE SEM DOCKER ===" -ForegroundColor Cyan

Write-Host "`nEste script irá:" -ForegroundColor Yellow
Write-Host "  - Remover arquivos de configuração NoDocker" -ForegroundColor Gray
Write-Host "  - Remover banco de dados SQLite" -ForegroundColor Gray
Write-Host "  - Parar processos dos serviços" -ForegroundColor Gray

$confirm = Read-Host "`nDeseja continuar? (S/N)"
if ($confirm -ne "S" -and $confirm -ne "s") {
    Write-Host "Operação cancelada." -ForegroundColor Yellow
    exit
}

# 1. Parar processos
Write-Host "`n1. Parando processos dos serviços..." -ForegroundColor Yellow
$ports = @(5000, 5001, 5002, 5003, 5004, 5005, 5006, 5008, 5009, 5010)

foreach ($port in $ports) {
    try {
        $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
        if ($connection) {
            $processId = $connection.OwningProcess
            $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
            if ($process) {
                Stop-Process -Id $processId -Force
                Write-Host "✅ Processo na porta $port parado" -ForegroundColor Green
            }
        }
    } catch {
        # Porta não estava em uso
    }
}

# 2. Remover arquivos de configuração
Write-Host "`n2. Removendo arquivos de configuração..." -ForegroundColor Yellow
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
    $configPath = "$service/appsettings.NoDocker.json"
    if (Test-Path $configPath) {
        Remove-Item $configPath -Force
        Write-Host "✅ Removido: $configPath" -ForegroundColor Green
    }
}

# 3. Remover banco SQLite
Write-Host "`n3. Removendo banco de dados SQLite..." -ForegroundColor Yellow
$dbFiles = Get-ChildItem -Path . -Filter "*.db" -Recurse -ErrorAction SilentlyContinue
foreach ($file in $dbFiles) {
    Remove-Item $file.FullName -Force
    Write-Host "✅ Removido: $($file.FullName)" -ForegroundColor Green
}

# Remover arquivos auxiliares do SQLite
$dbAuxFiles = Get-ChildItem -Path . -Filter "*.db-*" -Recurse -ErrorAction SilentlyContinue
foreach ($file in $dbAuxFiles) {
    Remove-Item $file.FullName -Force
    Write-Host "✅ Removido: $($file.FullName)" -ForegroundColor Green
}

# 4. Limpar LocalDB (opcional)
Write-Host "`n4. Deseja limpar o banco LocalDB? (S/N)" -ForegroundColor Yellow
$cleanLocalDB = Read-Host
if ($cleanLocalDB -eq "S" -or $cleanLocalDB -eq "s") {
    try {
        sqllocaldb stop MSSQLLocalDB 2>$null
        sqllocaldb delete MSSQLLocalDB 2>$null
        Write-Host "✅ LocalDB limpo" -ForegroundColor Green
    } catch {
        Write-Host "⚠️  Erro ao limpar LocalDB: $_" -ForegroundColor Yellow
    }
}

Write-Host "`n=== LIMPEZA COMPLETA ===" -ForegroundColor Green
Write-Host "`nPara reconfigurar o ambiente:" -ForegroundColor Cyan
Write-Host "  ./setup-no-docker.ps1" -ForegroundColor White
