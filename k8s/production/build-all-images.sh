#!/bin/bash

# Build All Docker Images for Microservices
# This script builds Docker images for all microservices

set -e

echo "=========================================="
echo "Building All Microservice Docker Images"
echo "=========================================="

# Color codes
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

# Configuration
REGISTRY="${REGISTRY:-aspnet-learning-platform}"
TAG="${TAG:-latest}"

# Navigate to project root
cd ../..

echo ""
echo "Registry: ${REGISTRY}"
echo "Tag: ${TAG}"
echo ""

# Array of services to build
declare -A SERVICES=(
    ["api-gateway"]="src/ApiGateway/Dockerfile"
    ["auth-service"]="src/Services/Auth/Dockerfile"
    ["course-service"]="src/Services/Course/Dockerfile"
    ["challenge-service"]="src/Services/Challenge/Dockerfile"
    ["execution-service"]="src/Services/Execution/Dockerfile"
    ["sqlexecutor-service"]="src/Services/SqlExecutor/Dockerfile"
    ["progress-service"]="src/Services/Progress/Dockerfile"
    ["aitutor-service"]="src/Services/AITutor/Dockerfile"
    ["notification-service"]="src/Services/Notification/Dockerfile"
    ["analytics-service"]="src/Services/Analytics/Dockerfile"
    ["worker-service"]="src/Services/Worker/Dockerfile"
)

# Build each service
for SERVICE in "${!SERVICES[@]}"; do
    DOCKERFILE="${SERVICES[$SERVICE]}"
    IMAGE_NAME="${REGISTRY}/${SERVICE}:${TAG}"
    
    echo "=========================================="
    echo "Building ${SERVICE}..."
    echo "Dockerfile: ${DOCKERFILE}"
    echo "Image: ${IMAGE_NAME}"
    echo "=========================================="
    
    if [ ! -f "${DOCKERFILE}" ]; then
        echo -e "${RED}Error: Dockerfile not found: ${DOCKERFILE}${NC}"
        continue
    fi
    
    # Build the image
    docker build -t "${IMAGE_NAME}" -f "${DOCKERFILE}" .
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✓ ${SERVICE} built successfully${NC}"
    else
        echo -e "${RED}✗ Failed to build ${SERVICE}${NC}"
        exit 1
    fi
    
    echo ""
done

echo "=========================================="
echo "Build Summary"
echo "=========================================="
echo ""
echo "Built Images:"
docker images | grep "${REGISTRY}"

echo ""
echo "=========================================="
echo "All images built successfully!"
echo "=========================================="
echo ""
echo "Next steps:"
echo "1. Test images locally: docker run -p 8080:8080 ${REGISTRY}/<service>:${TAG}"
echo "2. Push to registry: docker push ${REGISTRY}/<service>:${TAG}"
echo "3. Deploy to Kubernetes: ./k8s/production/deploy-all-microservices.sh"
echo ""
echo -e "${GREEN}Build complete!${NC}"
