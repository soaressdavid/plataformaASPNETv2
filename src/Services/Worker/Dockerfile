# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["Services/Worker/Worker.Service.csproj", "Services/Worker/"]
COPY ["Services/Execution/Execution.Service.csproj", "Services/Execution/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "Services/Worker/Worker.Service.csproj"

# Copy source code
COPY . .

# Build the project
WORKDIR "/src/Services/Worker"
RUN dotnet build "Worker.Service.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Worker.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# The worker needs access to Docker socket
# Mount the Docker socket when running: -v /var/run/docker.sock:/var/run/docker.sock

ENTRYPOINT ["dotnet", "Worker.Service.dll"]
