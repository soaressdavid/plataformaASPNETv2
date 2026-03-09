# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["Services/Execution/Execution.Service.csproj", "Services/Execution/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "Services/Execution/Execution.Service.csproj"

# Copy source code
COPY . .

# Build the project
WORKDIR "/src/Services/Execution"
RUN dotnet build "Execution.Service.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Execution.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .

# The execution service needs access to Docker socket to create containers
# Mount the Docker socket when running: -v /var/run/docker.sock:/var/run/docker.sock

ENTRYPOINT ["dotnet", "Execution.Service.dll"]
