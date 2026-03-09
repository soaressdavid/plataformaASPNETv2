# 🎓 Plataforma de Aprendizado ASP.NET Core

Uma plataforma completa de ensino e aprendizado de ASP.NET Core com execução real de código, sistema de gamificação, e ambiente IDE integrado.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-512BD4?logo=dotnet)
![Next.js](https://img.shields.io/badge/Next.js-15-000000?logo=next.js)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)
![License](https://img.shields.io/badge/License-MIT-green)

## 🚀 Características Principais

### 💻 Execução Real de Código
- **IDE Integrado**: Editor de código com syntax highlighting e autocomplete
- **Execução ASP.NET Core Real**: Cria servidor Kestrel temporário e executa código de verdade
- **Testes Automáticos**: Detecta e testa endpoints HTTP automaticamente
- **Feedback Instantâneo**: Resultados em tempo real com status codes e responses

### 🎮 Sistema de Gamificação
- **Sistema de Níveis**: 5 níveis de progressão (Iniciante → Expert)
- **Pontos e XP**: Ganhe pontos completando lições e desafios
- **Conquistas**: Desbloqueie badges e conquistas
- **Leaderboard**: Compete com outros estudantes
- **Streaks**: Mantenha sequências de estudo diárias

### 📚 Conteúdo Educacional
- **Cursos Estruturados**: Do básico ao avançado
- **Lições Interativas**: Teoria + prática integradas
- **Desafios de Código**: Exercícios práticos
- **Projetos Reais**: Construa aplicações completas
- **Exemplos de Código**: Biblioteca de snippets prontos

### 🏗️ Arquitetura Microserviços
- **7 Serviços Independentes**: Auth, Course, Progress, Execution, Gamification, Notification, Analytics
- **Comunicação Assíncrona**: RabbitMQ para eventos
- **Observabilidade**: Prometheus + Grafana
- **Health Checks**: Monitoramento de saúde dos serviços
- **API Gateway**: Roteamento centralizado

## 🛠️ Tecnologias

### Backend
- **ASP.NET Core 10.0**: Framework principal
- **Entity Framework Core**: ORM para banco de dados
- **SQL Server**: Banco de dados relacional
- **RabbitMQ**: Message broker para eventos
- **Roslyn**: Compilação dinâmica de código C#
- **Prometheus**: Métricas e monitoramento
- **Serilog**: Logging estruturado

### Frontend
- **Next.js 15**: Framework React com App Router
- **TypeScript**: Tipagem estática
- **Tailwind CSS**: Estilização
- **Monaco Editor**: Editor de código (VS Code)
- **Recharts**: Gráficos e visualizações
- **Lucide Icons**: Ícones modernos

### DevOps
- **Docker**: Containerização
- **Docker Compose**: Orquestração local
- **PowerShell**: Scripts de automação
- **Git**: Controle de versão

## 📋 Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [SQL Server](https://www.microsoft.com/sql-server) ou Docker
- [Git](https://git-scm.com/)

## 🚀 Instalação e Execução

### 1. Clone o Repositório
```bash
git clone https://github.com/soaressdavid/plataformaASPNET.git
cd plataformaASPNET
```

### 2. Configure as Variáveis de Ambiente

Copie os arquivos de exemplo:
```bash
cp .env.example .env
cp frontend/.env.local.example frontend/.env.local
```

### 3. Inicie os Serviços com Docker

```bash
docker-compose up -d
```

Isso iniciará:
- SQL Server (porta 1433)
- RabbitMQ (porta 5672, Management UI: 15672)
- Prometheus (porta 9090)
- Grafana (porta 3001)

### 4. Inicie os Serviços Backend e Frontend

**Windows (PowerShell):**
```powershell
.\start-all.ps1
```

**Ou manualmente:**
```bash
# Backend (7 serviços)
dotnet run --project src/Services/Auth
dotnet run --project src/Services/Course
dotnet run --project src/Services/Progress
dotnet run --project src/Services/Execution
dotnet run --project src/Services/Gamification
dotnet run --project src/Services/Notification
dotnet run --project src/Services/Analytics

# Frontend
cd frontend
npm install
npm run dev
```

### 5. Acesse a Plataforma

- **Frontend**: http://localhost:3000
- **API Gateway**: http://localhost:5000
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **Prometheus**: http://localhost:9090
- **Grafana**: http://localhost:3001 (admin/admin)

## 👤 Credenciais de Teste

Usuários pré-configurados para teste:

| Email | Senha | Descrição |
|-------|-------|-----------|
| test@test.com | Test123! | Usuário de teste principal |
| alice@example.com | password123 | Usuário exemplo |
| bob@example.com | securepass456 | Usuário exemplo |

## � Documentação

- [Como Rodar o Projeto](COMO_RODAR_O_PROJETO.md)
- [Comandos Rápidos](COMANDOS_RAPIDOS.md)
- [Configuração](CONFIGURATION.md)
- [Execução ASP.NET Core](ASPNET_CORE_EXECUTION_COMPLETO.md)
- [Credenciais de Acesso](CREDENCIAIS_ACESSO.md)

## 🏗️ Estrutura do Projeto

```
plataformaASPNET/
├── src/
│   ├── Services/           # Microserviços
│   │   ├── Auth/          # Autenticação e autorização
│   │   ├── Course/        # Gerenciamento de cursos
│   │   ├── Progress/      # Progresso do estudante
│   │   ├── Execution/     # Execução de código
│   │   ├── Gamification/  # Sistema de gamificação
│   │   ├── Notification/  # Notificações
│   │   └── Analytics/     # Análise de dados
│   ├── Shared/            # Código compartilhado
│   └── Gateway/           # API Gateway
├── frontend/              # Aplicação Next.js
│   ├── app/              # App Router (Next.js 15)
│   ├── components/       # Componentes React
│   └── lib/              # Utilitários
├── docker-compose.yml    # Orquestração Docker
└── docs/                 # Documentação adicional
```

## 🎯 Funcionalidades Detalhadas

### Execução de Código ASP.NET Core

O sistema possui um executor especializado que:

1. **Detecta código ASP.NET Core** automaticamente
2. **Compila dinamicamente** usando Roslyn
3. **Cria servidor Kestrel temporário** na porta 5555
4. **Registra controllers** do código compilado
5. **Detecta endpoints** ([HttpGet], [HttpPost], etc.)
6. **Executa requisições HTTP reais** para cada endpoint
7. **Retorna resultados formatados** com status codes e responses
8. **Encerra servidor automaticamente** após testes

Exemplo de output:
```
🔨 Compilando código ASP.NET Core...
✅ Compilação bem-sucedida!

🚀 Iniciando servidor temporário na porta 5555...
✅ Servidor iniciado!

📡 Testando endpoints detectados:

🔹 GET /api/products
   Status: 200 OK
   Response: ["Product1","Product2"]

🔹 POST /api/products
   Status: 201 Created
   Response: test data

🛑 Encerrando servidor temporário...
✅ Servidor encerrado com sucesso!
```

### Sistema de Níveis

| Nível | Nome | XP Necessário | Descrição |
|-------|------|---------------|-----------|
| 1 | Iniciante | 0 | Fundamentos de C# e .NET |
| 2 | Intermediário | 1000 | ASP.NET Core básico |
| 3 | Avançado | 3000 | APIs RESTful e Entity Framework |
| 4 | Profissional | 6000 | Arquitetura e padrões avançados |
| 5 | Expert | 10000 | Microserviços e cloud |

## 🧪 Testes

```bash
# Backend
dotnet test

# Frontend
cd frontend
npm test
```

## 📊 Monitoramento

### Prometheus Metrics
- Requisições HTTP por serviço
- Tempo de resposta
- Taxa de erro
- Execuções de código
- Eventos RabbitMQ

### Health Checks
Todos os serviços expõem `/health` endpoint:
- SQL Server connectivity
- RabbitMQ connectivity
- Service status

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👨‍💻 Autor

**David Soares**
- GitHub: [@soaressdavid](https://github.com/soaressdavid)

## 🙏 Agradecimentos

- Comunidade ASP.NET Core
- Next.js Team
- Todos os contribuidores open-source

## 📞 Suporte

Se você encontrar algum problema ou tiver dúvidas:

1. Verifique a [documentação](COMO_RODAR_O_PROJETO.md)
2. Procure em [Issues](https://github.com/soaressdavid/plataformaASPNET/issues)
3. Abra uma nova issue se necessário

---

⭐ Se este projeto foi útil para você, considere dar uma estrela!
