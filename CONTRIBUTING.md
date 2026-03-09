# 🤝 Guia de Contribuição

Obrigado por considerar contribuir para a Plataforma de Aprendizado ASP.NET Core! Este documento fornece diretrizes para contribuir com o projeto.

## 📋 Código de Conduta

Este projeto segue um código de conduta. Ao participar, você concorda em manter um ambiente respeitoso e inclusivo.

## 🚀 Como Contribuir

### 1. Fork e Clone

```bash
# Fork o repositório no GitHub
# Clone seu fork
git clone https://github.com/SEU_USUARIO/plataformaASPNET.git
cd plataformaASPNET

# Adicione o repositório original como upstream
git remote add upstream https://github.com/soaressdavid/plataformaASPNET.git
```

### 2. Crie uma Branch

```bash
# Atualize sua main
git checkout main
git pull upstream main

# Crie uma branch para sua feature/fix
git checkout -b feature/nome-da-feature
# ou
git checkout -b fix/nome-do-bug
```

### 3. Faça suas Alterações

- Escreva código limpo e bem documentado
- Siga os padrões de código do projeto
- Adicione testes quando apropriado
- Atualize a documentação se necessário

### 4. Commit suas Mudanças

Usamos [Conventional Commits](https://www.conventionalcommits.org/):

```bash
# Exemplos de commits
git commit -m "feat: adiciona novo endpoint de autenticação"
git commit -m "fix: corrige bug no cálculo de XP"
git commit -m "docs: atualiza README com instruções de instalação"
git commit -m "refactor: melhora performance do executor de código"
git commit -m "test: adiciona testes para serviço de gamificação"
```

Tipos de commit:
- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Documentação
- `style`: Formatação (não afeta código)
- `refactor`: Refatoração de código
- `test`: Adiciona ou modifica testes
- `chore`: Tarefas de manutenção

### 5. Push e Pull Request

```bash
# Push para seu fork
git push origin feature/nome-da-feature

# Abra um Pull Request no GitHub
```

## 📝 Padrões de Código

### Backend (C#)

- Use PascalCase para classes, métodos e propriedades
- Use camelCase para variáveis locais e parâmetros
- Adicione XML comments em APIs públicas
- Siga as convenções do [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

```csharp
/// <summary>
/// Executa código ASP.NET Core dinamicamente
/// </summary>
/// <param name="code">Código fonte C#</param>
/// <returns>Resultado da execução</returns>
public async Task<ExecutionResult> ExecuteAsync(string code)
{
    // Implementação
}
```

### Frontend (TypeScript/React)

- Use PascalCase para componentes React
- Use camelCase para funções e variáveis
- Use interfaces para tipos complexos
- Prefira functional components com hooks

```typescript
interface CourseCardProps {
  title: string;
  description: string;
  level: number;
}

export function CourseCard({ title, description, level }: CourseCardProps) {
  // Implementação
}
```

## 🧪 Testes

### Backend

```bash
# Executar todos os testes
dotnet test

# Executar testes de um projeto específico
dotnet test src/Services/Auth/Auth.Service.Tests
```

### Frontend

```bash
cd frontend

# Executar testes
npm test

# Executar com coverage
npm run test:coverage
```

## 📚 Documentação

- Atualize o README.md se adicionar novas funcionalidades
- Adicione comentários em código complexo
- Crie documentação adicional em `/docs` se necessário
- Mantenha o CHANGELOG.md atualizado

## 🐛 Reportando Bugs

Ao reportar bugs, inclua:

1. **Descrição clara** do problema
2. **Passos para reproduzir**
3. **Comportamento esperado** vs **comportamento atual**
4. **Screenshots** (se aplicável)
5. **Ambiente**: SO, versão do .NET, versão do Node.js, etc.

Use o template de issue do GitHub.

## 💡 Sugerindo Funcionalidades

Ao sugerir funcionalidades:

1. **Descreva o problema** que a funcionalidade resolve
2. **Explique a solução proposta**
3. **Considere alternativas**
4. **Adicione mockups** (se aplicável)

## 🔍 Code Review

Todos os Pull Requests passam por code review:

- Seja respeitoso e construtivo
- Explique o "porquê" dos comentários
- Aceite feedback de forma positiva
- Faça perguntas se não entender algo

## 📦 Estrutura de Commits

Mantenha commits:
- **Atômicos**: Uma mudança lógica por commit
- **Descritivos**: Mensagem clara do que foi feito
- **Testados**: Código compila e testes passam

## 🎯 Áreas para Contribuir

### Backend
- Novos serviços ou endpoints
- Melhorias de performance
- Testes unitários e de integração
- Documentação de APIs

### Frontend
- Novos componentes UI
- Melhorias de UX
- Testes de componentes
- Acessibilidade

### DevOps
- Scripts de automação
- Configurações Docker
- CI/CD pipelines
- Monitoramento

### Documentação
- Tutoriais
- Guias de uso
- Exemplos de código
- Traduções

## ❓ Dúvidas?

- Abra uma [Discussion](https://github.com/soaressdavid/plataformaASPNET/discussions)
- Entre em contato via Issues
- Consulte a documentação existente

## 🙏 Agradecimentos

Toda contribuição é valiosa, seja código, documentação, testes ou feedback!

---

**Obrigado por contribuir! 🎉**
