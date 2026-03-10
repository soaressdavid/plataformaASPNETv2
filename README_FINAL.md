# 🎓 ASP.NET Learning Platform - VERSÃO FINAL

## 🚀 Setup Ultra Rápido (Versão Estável)

```powershell
./start-simple.ps1
```

**Acesse:** http://localhost:3000

## ✨ O Que Funciona

### 🎯 Executores Reais
- **SQL Executor** - Executa SQL real em banco SQLite
- **C# Executor** - Compila e executa C# real com Roslyn
- **Detecção automática** - Sistema escolhe executor baseado na aula

### 🗄️ Dados Reais
- **Clientes** (5 registros): João Silva, Maria Santos, etc
- **Pedidos** (6 registros): Relacionados via Foreign Key
- **Produtos** (5 registros): Notebook, Mouse, Teclado, etc
- **Banco persistente**: sqlpractice.db

## 🧪 Exemplos Funcionando

### SQL Real
```sql
SELECT c.Nome, p.Valor 
FROM Clientes c 
INNER JOIN Pedidos p ON c.ClienteID = p.ClienteID;
```

### C# Real
```csharp
Console.WriteLine("Hello World!");
for(int i = 0; i < 3; i++) {
    Console.WriteLine($"Número: {i}");
}
```

## 🎯 Como Usar

1. Execute `./start-simple.ps1`
2. Acesse http://localhost:3000
3. Navegue para qualquer aula
4. O sistema detecta automaticamente:
   - Aulas de SQL → Mostra SQL Executor
   - Aulas de C# → Mostra C# IDE
   - Aulas de DevOps → Mostra Terminal
   - Aulas de Azure → Mostra Azure Portal

## 🔧 Serviços

- **SqlExecutor**: http://localhost:5008 (SQL real)
- **Execution**: http://localhost:5006 (C# real)  
- **Frontend**: http://localhost:3000 (Interface)

## 📝 Comandos

```powershell
# Iniciar (versão estável)
./start-simple.ps1

# Parar tudo
./cleanup-mock.ps1
```

## 🎉 Diferencial

**Não é MOCK - É REAL:**
- ✅ SQL executa em banco SQLite real
- ✅ C# compila com Roslyn real
- ✅ Dados persistem entre sessões
- ✅ Performance real medida
- ✅ Erros reais de compilação/SQL

## 🏆 Status

**100% FUNCIONAL SEM DOCKER!**

Versão simplificada, estável e com funcionalidades reais de execução de código.