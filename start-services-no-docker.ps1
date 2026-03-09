# Iniciar todos os serviços sem Docker
Write-Host "=== INICIANDO SERVIÇOS (SEM DOCKER) ===" -ForegroundColor Cyan

$services = @(
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

foreach ($service in $services) {
    Write-Host "Iniciando $($service.Name) na porta $($service.Port)..." -ForegroundColor Yellow
    
    Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd $($service.Path); dotnet run --urls `"http://localhost:$($service.Port)`" --environment NoDocker"
    
    Start-Sleep -Seconds 2
}

Write-Host "`n✅ Todos os serviços iniciados!" -ForegroundColor Green
Write-Host "Pressione qualquer tecla para sair..." -ForegroundColor Cyan
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
