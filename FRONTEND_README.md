# Frontend Next.js - Guia Rápido

## 🚀 Como Iniciar

### Opção 1: Com o script completo (recomendado)
```powershell
.\start-all.ps1
```
Este script inicia o backend + frontend automaticamente.

### Opção 2: Apenas o frontend
```powershell
.\start-frontend.ps1
```
Ou manualmente:
```powershell
cd frontend
npx next dev
```

## 📝 Nota Importante

Se você encontrar o erro `Cannot find module 'next/dist/bin/next'`, execute:

```powershell
cd frontend
Remove-Item -Recurse -Force node_modules, package-lock.json
npm install
```

Isso reinstalará todas as dependências corretamente.

## 🌐 Acessar o Frontend

Após iniciar, acesse: http://localhost:3000

## 🔧 Configuração

O frontend está configurado para se conectar ao backend em:
- API Gateway: http://localhost:5000

Veja o arquivo `frontend/.env.local` para mais configurações.

## ⚠️ Requisitos

- Node.js v20.15.1 ou superior
- npm 10.7.0 ou superior

## 📚 Estrutura

```
frontend/
├── app/              # Páginas Next.js (App Router)
├── lib/              # Bibliotecas e utilitários
│   ├── api/          # Clientes API
│   ├── components/   # Componentes React
│   ├── contexts/     # Contextos React
│   └── hooks/        # Hooks customizados
├── public/           # Arquivos estáticos
└── .env.local        # Configurações locais
```

## 🐛 Troubleshooting

### Frontend não inicia
1. Verifique se o Node.js está instalado: `node --version`
2. Verifique se a porta 3000 está livre
3. Reinstale as dependências (veja acima)

### Erro de conexão com backend
1. Verifique se o backend está rodando: `.\health-check.ps1`
2. Verifique o arquivo `.env.local` no frontend

### Página em branco
1. Abra o console do navegador (F12)
2. Verifique se há erros JavaScript
3. Verifique se o backend está respondendo
