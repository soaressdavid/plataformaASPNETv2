# 🎓 ASP.NET Learning Platform - Versão MOCK

Uma plataforma completa de aprendizado de programação com **executores contextuais** e **versões MOCK** que funcionam 100% sem Docker.

## 🚀 Setup Ultra Rápido (1 Comando)

```powershell
./setup-mock-simples.ps1
```

**Pronto!** Em 2 minutos você tem a plataforma rodando em http://localhost:3000

## ✨ Funcionalidades

### 🎯 Executores Contextuais
- **SQL Executor** - Para aulas de banco de dados
- **Terminal Executor** - Para aulas de DevOps, Docker, Git
- **Azure Simulator** - Para aulas de cloud computing
- **C# IDE** - Para aulas de programação (padrão)

### 🔧 Detecção Automática
O sistema detecta automaticamente o tipo de aula e mostra o executor apropriado:
- Palavras-chave SQL → SQL Executor
- Palavras-chave DevOps → Terminal Executor  
- Palavras-chave Azure → Azure Simulator
- Padrão → C# IDE

### 🎮 Versões MOCK
- **Execution Service MOCK** - Simula execução de código C# com outputs realistas
- **SqlExecutor MOCK** - Simula execução de queries SQL com dados mockados
- **Interface idêntica** aos serviços reais
- **Funciona instantaneamente** sem dependências complexas

## 🛠️ Pré-requisitos

- Windows 10/11
- .NET SDK 8.0+
- Node.js 18+

## 📊 Arquitetura

```
Frontend (Next.js) → ApiGateway → Microserviços
├── Auth Service (Real)
├── Course Service (Real)  
├── Execution Service (MOCK)
└── SqlExecutor Service (MOCK)
```

## 🎯 Comandos Úteis

```powershell
# Iniciar projeto
./setup-mock-simples.ps1

# Parar tudo
./cleanup-mock.ps1

# Verificar status
curl http://localhost:3000
```

## 🌟 Destaques

- ✅ **Setup em 1 comando** - Funciona em qualquer PC Windows
- ✅ **Sem Docker** necessário - Versões MOCK ultra simples
- ✅ **Executores contextuais** - SQL, Terminal, Azure, C#
- ✅ **Interface completa** - Cursos, desafios, projetos, analytics
- ✅ **Experiência realista** - Outputs mockados convincentes
- ✅ **Perfeito para demos** - Impressiona com funcionalidade

## 🎓 Como Usar

1. Clone o repositório
2. Execute `./setup-mock-simples.ps1`
3. Acesse http://localhost:3000
4. Explore os cursos e veja os executores em ação!

## 📝 Licença

MIT License - Sinta-se livre para usar e modificar.

---

**🎉 Versão MOCK 100% funcional - Melhor simples que funciona do que complexo que não funciona!**