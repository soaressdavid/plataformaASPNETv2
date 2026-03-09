'use client';

import { useAuth } from '@/lib/contexts/AuthContext';
import { useRouter } from 'next/navigation';

export default function DashboardTestPage() {
  const { isAuthenticated, isLoading, user } = useAuth();
  const router = useRouter();

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-900">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto"></div>
          <p className="mt-4 text-gray-300">Carregando...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-900 p-8">
      <div className="max-w-4xl mx-auto">
        <div className="bg-gray-800 rounded-lg shadow-lg p-8 border border-gray-700">
          <h1 className="text-3xl font-bold text-white mb-6">🎉 Dashboard de Teste</h1>
          
          <div className="space-y-4">
            <div className="bg-gray-900 p-4 rounded border border-gray-700">
              <h2 className="text-xl font-semibold text-blue-400 mb-3">Status de Autenticação</h2>
              <div className="space-y-2 text-gray-300">
                <p className="font-mono text-sm">
                  isAuthenticated: <span className={isAuthenticated ? 'text-green-400' : 'text-red-400'}>
                    {isAuthenticated ? '✓ true' : '✗ false'}
                  </span>
                </p>
                <p className="font-mono text-sm">
                  isLoading: <span className="text-gray-400">{String(isLoading)}</span>
                </p>
                <p className="font-mono text-sm">
                  hasUser: <span className={user ? 'text-green-400' : 'text-red-400'}>
                    {user ? '✓ true' : '✗ false'}
                  </span>
                </p>
              </div>
            </div>

            {user && (
              <div className="bg-gray-900 p-4 rounded border border-gray-700">
                <h2 className="text-xl font-semibold text-purple-400 mb-3">Dados do Usuário</h2>
                <div className="space-y-2 text-gray-300">
                  <p className="font-mono text-sm">ID: {user.userId}</p>
                  <p className="font-mono text-sm">Nome: {user.name}</p>
                  <p className="font-mono text-sm">Email: {user.email}</p>
                </div>
              </div>
            )}

            <div className="bg-gray-900 p-4 rounded border border-gray-700">
              <h2 className="text-xl font-semibold text-orange-400 mb-3">localStorage</h2>
              <div className="space-y-2 text-gray-300">
                <p className="font-mono text-sm">
                  auth_token: {typeof window !== 'undefined' && localStorage.getItem('auth_token') ? 
                    <span className="text-green-400">✓ Presente ({localStorage.getItem('auth_token')?.substring(0, 20)}...)</span> : 
                    <span className="text-red-400">✗ Ausente</span>
                  }
                </p>
                <p className="font-mono text-sm">
                  auth_user: {typeof window !== 'undefined' && localStorage.getItem('auth_user') ? 
                    <span className="text-green-400">✓ Presente</span> : 
                    <span className="text-red-400">✗ Ausente</span>
                  }
                </p>
              </div>
            </div>

            {!isAuthenticated && (
              <div className="bg-red-900/20 border border-red-500 p-4 rounded">
                <p className="text-red-400 font-semibold mb-2">⚠️ Você não está autenticado</p>
                <button
                  onClick={() => router.push('/login')}
                  className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700 transition-colors"
                >
                  Ir para Login
                </button>
              </div>
            )}

            {isAuthenticated && (
              <div className="bg-green-900/20 border border-green-500 p-4 rounded">
                <p className="text-green-400 font-semibold mb-4">✓ Você está autenticado!</p>
                <p className="text-gray-300 text-sm mb-4">
                  Esta é uma página de teste SEM ProtectedRoute. Se você consegue ver isso, 
                  significa que o login está funcionando perfeitamente!
                </p>
                <div className="flex gap-4">
                  <button
                    onClick={() => router.push('/dashboard')}
                    className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition-colors"
                  >
                    Tentar Dashboard Normal
                  </button>
                  <button
                    onClick={() => router.push('/login')}
                    className="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700 transition-colors"
                  >
                    Voltar para Login
                  </button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
