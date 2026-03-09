# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

## [1.0.0] - 2026-03-08

### 🎉 Lançamento Inicial

#### ✨ Adicionado

##### Backend
- **Arquitetura de Microserviços**: 7 serviços independentes (Auth, Course, Progress, Execution, Gamification, Notification, Analytics)
- **Execução Real de Código ASP.NET Core**: 
  - Compilação dinâmica com Roslyn
  - Servidor Kestrel temporário
  - Detecção automática de endpoints
  - Testes HTTP automáticos
  - Feedback formatado com emojis
- **Sistema de Autenticação**: JWT com refresh tokens
- **Sistema de Gamificação**:
  - 5 níveis de progressão
  - Sistema de pontos e XP
  - Conquistas e badges
  - Leaderboard
  - Streaks diários
- **Gerenciamento de Cursos**: CRUD completo com lições e módulos
- **Rastreamento de Progresso**: Acompanhamento detalhado do estudante
- **Sistema de Notificações**: Eventos em tempo real via RabbitMQ
- **Analytics**: Métricas e estatísticas de uso
- **Health Checks**: Monitoramento de saúde dos serviços
- **Observabilidade**: Prometheus + Grafana
- **Logging Estruturado**: Serilog com múltiplos sinks

##### Frontend
- **Interface Moderna**: Next.js 15 com App Router
- **IDE Integrado**: Monaco Editor (VS Code)
- **Execução de Código**: Interface para executar código C# e ASP.NET Core
- **Dashboard do Estudante**: Visão geral de progresso e conquistas
- **Catálogo de Cursos**: Navegação e inscrição em cursos
- **Sistema de Lições**: Conteúdo interativo com teoria e prática
- **Desafios de Código**: Exercícios práticos
- **Leaderboard**: Ranking de estudantes
- **Perfil do Usuário**: Gerenciamento de conta e progresso
- **Tema Dark/Light**: Suporte a temas
- **Responsivo**: Design adaptável para mobile

##### DevOps
- **Docker Compose**: Orquestração de serviços
- **Scripts PowerShell**: Automação de tarefas
- **SQL Server**: Banco de dados containerizado
- **RabbitMQ**: Message broker containerizado
- **Prometheus**: Métricas containerizadas
- **Grafana**: Dashboards containerizados

##### Documentação
- README completo com instruções
- Guia de contribuição
- Documentação de APIs
- Exemplos de código
- Credenciais de teste

#### 🔧 Configuração
- Variáveis de ambiente configuráveis
- Suporte para múltiplos ambientes (Development, Production)
- Health checks em todos os serviços
- CORS configurado
- Rate limiting

#### 🎓 Conteúdo Educacional
- **Nível 1 - Iniciante**: Fundamentos de C# e .NET
- **Nível 2 - Intermediário**: ASP.NET Core básico
- **Nível 3 - Avançado**: APIs RESTful e Entity Framework
- **Nível 4 - Profissional**: Arquitetura e padrões avançados
- **Nível 5 - Expert**: Microserviços e cloud

#### 🐛 Correções
- Corrigido problema de logger no Execution Service
- Corrigido detecção de endpoints em controllers
- Corrigido substituição de parâmetros de rota em testes
- Corrigido inicialização de serviços com Docker

#### 🔒 Segurança
- Senhas hasheadas com BCrypt
- JWT com expiração configurável
- Validação de entrada em todos os endpoints
- CORS restritivo em produção
- Secrets não commitados no repositório

---

## [Unreleased]

### 🚧 Em Desenvolvimento
- Suporte para Minimal APIs
- Testes de integração automatizados
- CI/CD com GitHub Actions
- Deploy automatizado
- Suporte para múltiplos idiomas
- Sistema de certificados

---

## Tipos de Mudanças

- `✨ Adicionado` - Novas funcionalidades
- `🔧 Modificado` - Mudanças em funcionalidades existentes
- `🗑️ Removido` - Funcionalidades removidas
- `🐛 Corrigido` - Correções de bugs
- `🔒 Segurança` - Correções de vulnerabilidades
- `📚 Documentação` - Mudanças na documentação
- `⚡ Performance` - Melhorias de performance
- `♻️ Refatoração` - Mudanças de código sem alterar funcionalidade

---

[1.0.0]: https://github.com/soaressdavid/plataformaASPNET/releases/tag/v1.0.0
