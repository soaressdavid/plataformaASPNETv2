# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["Services/SqlExecutor/SqlExecutor.Service.csproj", "Services/SqlExecutor/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "Services/SqlExecutor/SqlExecutor.Service.csproj"

# Copy source code
COPY . .

# Build the project
WORKDIR "/src/Services/SqlExecutor"
RUN dotnet build "SqlExecutor.Service.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SqlExecutor.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SqlExecutor.Service.dll"]
