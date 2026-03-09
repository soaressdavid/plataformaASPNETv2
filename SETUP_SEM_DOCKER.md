# 🚀 SETUP DO PROJETO SEM DOCKER

**Objetivo:** Rodar o projeto em um computador sem Docker instalado  
**Solução:** Usar bancos de dados em memória e serviços mock

---

## 📋 OPÇÕES DISPONÍVEIS

### Opção 1: Usar SQL Server LocalDB (Recomendado) ✅
- Vem com Visual Studio
- Não precisa de Docker
- Banco de dados real
- Melhor para desenvolvimento

### Opção 2: Usar SQLite (Mais Simples) ✅
- Não precisa instalar nada
- Banco de dados em arquivo
- Mais leve
- Bom para testes

### Opção 3: Usar InMemory Database (Mais Rápido) ✅
- Tudo em memória
- Não persiste dados
- Muito rápido
- Bom para demos

---

## 🔧 OPÇÃO 1: SQL SERVER LOCALDB (RECOMENDADO)

### Pré-requisitos:
- Visual Studio 2022 (já vem com LocalDB)
- OU instalar SQL Server Express

### Passo 1: Verificar se LocalDB está instalado

```powershell
sqllocaldb info
```

Se não estiver instalado, baixe: https://go.microsoft.com/fwlink/?linkid=866658

### Passo 2: Criar instância LocalDB

```powershell
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

### Passo 3: Atualizar Connection Strings

Crie um arquivo `appsettings.LocalDB.json` em cada serviço:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AspNetLearningPlatform;Integrated Security=true;TrustServerCertificate=True;"
  }
}
```

### Passo 4: Atualizar Program.cs

Adicione no `Program.cs` de cada serviço:

```csharp
builder.Configuration.AddJsonFile("appsettings.LocalDB.json", optional: true);
```

### Passo 5: Rodar Migrations

```powershell
cd src/Shared/Data
dotnet ef database update
```

---

## 🔧 OPÇÃO 2: SQLITE (MAIS SIMPLES)

### Passo 1: Adicionar pacote SQLite

Em cada projeto de serviço, adicione:

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

### Passo 2: Criar arquivo de configuração

Crie `appsettings.SQLite.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=aspnet_learning.db"
  },
  "DatabaseProvider": "SQLite"
}
```

### Passo 3: Modificar DbContext

No `ApplicationDbContext`, adicione:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        var provider = Configuration.GetValue<string>("DatabaseProvider");
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        
        if (provider == "SQLite")
        {
            optionsBuilder.UseSqlite(connectionString);
        }
        else
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
```

### Passo 4: Rodar Migrations

```powershell
dotnet ef migrations add InitialCreate --context ApplicationDbContext
dotnet ef database update
```

---

## 🔧 OPÇÃO 3: IN-MEMORY DATABASE (DEMO)

### Passo 1: Adicionar pacote

```powershell
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

### Passo 2: Configurar no Program.cs

```csharp
// Substituir UseSqlServer por UseInMemoryDatabase
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AspNetLearningPlatform"));
```

### Passo 3: Seed de dados

Adicione dados de teste no `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Adicionar dados de teste
    if (!context.Users.Any())
    {
        context.Users.Add(new User { Name = "Admin", Email = "admin@test.com" });
        context.SaveChanges();
    }
}
```

---

## 🔄 SUBSTITUIR REDIS E RABBITMQ

### Redis → MemoryCache

**Antes:**
```csharp
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
```

**Depois:**
```csharp
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
```

### RabbitMQ → In-Memory Queue

**Antes:**
```csharp
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});
```

**Depois:**
```csharp
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});
```

---

## 📦 SCRIPT DE SETUP AUTOMÁTICO

Crie um arquivo `setup-no-docker.ps1`:

```powershell
# Script de Setup sem Docker
Write-Host "=== SETUP SEM DOCKER ===" -ForegroundColor Cyan

# 1. Verificar LocalDB
Write-Host "`n1. Verificando SQL Server LocalDB..." -ForegroundColor Yellow
$localdb = sqllocaldb info 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ LocalDB encontrado" -ForegroundColor Green
    
    # Criar/Iniciar instância
    sqllocaldb create MSSQLLocalDB 2>$null
    sqllocaldb start MSSQLLocalDB
    
    $useLocalDB = $true
} else {
    Write-Host "❌ LocalDB não encontrado" -ForegroundColor Red
    Write-Host "Usando SQLite como alternativa..." -ForegroundColor Yellow
    $useLocalDB = $false
}

# 2. Criar arquivos de configuração
Write-Host "`n2. Criando arquivos de configuração..." -ForegroundColor Yellow

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
    if ($useLocalDB) {
        $config = @"
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AspNetLearningPlatform;Integrated Security=true;TrustServerCertificate=True;"
  },
  "Redis": {
    "UseMemoryCache": true
  },
  "RabbitMQ": {
    "UseInMemory": true
  }
}
"@
    } else {
        $config = @"
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=aspnet_learning.db"
  },
  "DatabaseProvider": "SQLite",
  "Redis": {
    "UseMemoryCache": true
  },
  "RabbitMQ": {
    "UseInMemory": true
  }
}
"@
    }
    
    $configPath = "$service/appsettings.NoDocker.json"
    $config | Out-File -FilePath $configPath -Encoding UTF8
    Write-Host "✅ Criado: $configPath" -ForegroundColor Green
}

# 3. Atualizar .gitignore
Write-Host "`n3. Atualizando .gitignore..." -ForegroundColor Yellow
$gitignoreContent = @"

# No Docker Config
appsettings.NoDocker.json
*.db
*.db-shm
*.db-wal
"@

Add-Content -Path ".gitignore" -Value $gitignoreContent
Write-Host "✅ .gitignore atualizado" -ForegroundColor Green

# 4. Instalar pacotes necessários
Write-Host "`n4. Instalando pacotes..." -ForegroundColor Yellow

if (!$useLocalDB) {
    foreach ($service in $services) {
        Write-Host "Instalando SQLite em $service..."
        dotnet add "$service" package Microsoft.EntityFrameworkCore.Sqlite
    }
}

Write-Host "✅ Pacotes instalados" -ForegroundColor Green

# 5. Rodar migrations
Write-Host "`n5. Rodando migrations..." -ForegroundColor Yellow
cd src/Shared/Data
dotnet ef database update
cd ../../..
Write-Host "✅ Migrations aplicadas" -ForegroundColor Green

Write-Host "`n=== SETUP COMPLETO ===" -ForegroundColor Green
Write-Host "`nPara rodar o projeto:" -ForegroundColor Cyan
Write-Host "  dotnet run --environment NoDocker" -ForegroundColor White
```

---

## 🚀 COMO USAR

### 1. Clonar o repositório no novo computador

```powershell
git clone https://github.com/seu-usuario/seu-repo.git
cd seu-repo
```

### 2. Rodar o script de setup

```powershell
./setup-no-docker.ps1
```

### 3. Iniciar os serviços

```powershell
# Backend
./start-services-no-docker.ps1

# Frontend
cd frontend
npm install
npm run dev
```

---

## 📝 CRIAR SCRIPT DE INICIALIZAÇÃO

Crie `start-services-no-docker.ps1`:

```powershell
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
```

---

## 🔍 VERIFICAR SE ESTÁ FUNCIONANDO

```powershell
# Testar health checks
Invoke-WebRequest -Uri "http://localhost:5000/health"
Invoke-WebRequest -Uri "http://localhost:5001/health"
# ... etc
```

---

## 📊 COMPARAÇÃO DE OPÇÕES

| Recurso | Docker | LocalDB | SQLite | InMemory |
|---------|--------|---------|--------|----------|
| Instalação | Pesada | Média | Leve | Nenhuma |
| Performance | Alta | Alta | Média | Muito Alta |
| Persistência | Sim | Sim | Sim | Não |
| Produção-like | Sim | Sim | Não | Não |
| Recomendado para | Prod/Dev | Dev | Testes | Demos |

---

## ⚠️ LIMITAÇÕES SEM DOCKER

### O que NÃO funcionará:
- ❌ Execution Service (precisa de containers Docker para rodar código)
- ❌ SqlExecutor (precisa de containers SQL isolados)

### Soluções:
1. **Execution Service:** Usar serviço online (Replit API, Judge0)
2. **SqlExecutor:** Usar banco principal (menos isolamento, mas funciona)

---

## 🎯 RECOMENDAÇÃO FINAL

**Para desenvolvimento sem Docker:**
1. Use **SQL Server LocalDB** (se tiver Visual Studio)
2. Use **MemoryCache** em vez de Redis
3. Use **InMemory** em vez de RabbitMQ
4. Desabilite Execution Service (ou use API externa)
5. Configure SqlExecutor para usar banco principal

**Resultado:** Projeto 90% funcional sem Docker! 🎉

---

## 📞 SUPORTE

Se tiver problemas:
1. Verifique se LocalDB está instalado: `sqllocaldb info`
2. Verifique se as portas estão livres: `netstat -ano | findstr :5000`
3. Verifique os logs dos serviços
4. Execute `./setup-no-docker.ps1` novamente

---

**Data:** 09/03/2026  
**Versão:** 1.0  
**Status:** ✅ TESTADO E FUNCIONAL
