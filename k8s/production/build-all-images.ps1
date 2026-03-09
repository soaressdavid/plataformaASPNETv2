# Build All Docker Images for Microservices
# This script builds Docker images for all microservices

$ErrorActionPreference = "Stop"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Building All Microservice Docker Images" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan

# Configuration
$REGISTRY = if ($env:REGISTRY) { $env:REGISTRY } else { "aspnet-learning-platform" }
$TAG = if ($env:TAG) { $env:TAG } else { "latest" }

# Navigate to project root
Set-Location ..\..

Write-Host ""
Write-Host "Registry: $REGISTRY"
Write-Host "Tag: $TAG"
Write-Host ""

# Services to build
$services = @{
    "api-gateway" = "src\ApiGateway\Dockerfile"
    "auth-service" = "src\Services\Auth\Dockerfile"
    "course-service" = "src\Services\Course\Dockerfile"
    "challenge-service" = "src\Services\Challenge\Dockerfile"
    "execution-service" = "src\Services\Execution\Dockerfile"
    "sqlexecutor-service" = "src\Services\SqlExecutor\Dockerfile"
    "progress-service" = "src\Services\Progress\Dockerfile"
    "aitutor-service" = "src\Services\AITutor\Dockerfile"
    "notification-service" = "src\Services\Notification\Dockerfile"
    "analytics-service" = "src\Services\Analytics\Dockerfile"
    "worker-service" = "src\Services\Worker\Dockerfile"
}

# Build each service
foreach ($service in $services.Keys) {
    $dockerfile = $services[$service]
    $imageName = "$REGISTRY/${service}:$TAG"
    
    Write-Host "==========================================" -ForegroundColor Cyan
    Write-Host "Building $service..." -ForegroundColor Cyan
    Write-Host "Dockerfile: $dockerfile"
    Write-Host "Image: $imageName"
    Write-Host "==========================================" -ForegroundColor Cyan
    
    if (-not (Test-Path $dockerfile)) {
        Write-Host "Error: Dockerfile not found: $dockerfile" -ForegroundColor Red
        continue
    }
    
    # Build the image
    docker build -t $imageName -f $dockerfile .
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ $service built successfully" -ForegroundColor Green
    } else {
        Write-Host "✗ Failed to build $service" -ForegroundColor Red
        exit 1
    }
    
    Write-Host ""
}

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Build Summary" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Built Images:"
docker images | Select-String $REGISTRY

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "All images built successfully!" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:"
Write-Host "1. Test images locally: docker run -p 8080:8080 $REGISTRY/<service>:$TAG"
Write-Host "2. Push to registry: docker push $REGISTRY/<service>:$TAG"
Write-Host "3. Deploy to Kubernetes: .\k8s\production\deploy-all-microservices.ps1"
Write-Host ""
Write-Host "Build complete!" -ForegroundColor Green
