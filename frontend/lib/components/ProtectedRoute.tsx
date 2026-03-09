'use client';

import { useRouter } from 'next/navigation';
import { useAuth } from '../contexts/AuthContext';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

export const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
  const { isAuthenticated, isLoading, user } = useAuth();
  const router = useRouter();

  // Show loading state while checking authentication
  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-900">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto"></div>
          <p className="mt-4 text-gray-300">Verificando autenticação...</p>
        </div>
      </div>
    );
  }

  // TEMPORÁRIO: Mostrar status ao invés de redirecionar
  if (!isAuthenticated) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-900 p-4">
        <div className="max-w-md w-full bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-700">
          <h2 className="text-2xl font-bold text-red-400 mb-4">⚠️ Não Autenticado</h2>
          <p className="text-gray-300 mb-4">Você não está autenticado.</p>
          <div className="bg-gray-900 p-4 rounded mb-4 border border-gray-700">
            <p className="text-sm font-mono text-gray-400 mb-2">Status de Autenticação:</p>
            <p className="text-xs font-mono text-gray-300">isAuthenticated: {String(isAuthenticated)}</p>
            <p className="text-xs font-mono text-gray-300">isLoading: {String(isLoading)}</p>
            <p className="text-xs font-mono text-gray-300">hasUser: {String(!!user)}</p>
            <p className="text-xs font-mono text-gray-300 mt-2">
              Token: {typeof window !== 'undefined' && localStorage.getItem('auth_token') ? '✓ Presente' : '✗ Ausente'}
            </p>
            <p className="text-xs font-mono text-gray-300">
              User: {typeof window !== 'undefined' && localStorage.getItem('auth_user') ? '✓ Presente' : '✗ Ausente'}
            </p>
          </div>
          <button 
            onClick={() => {
              console.log('Redirecting to login...');
              router.push('/login');
            }}
            className="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 transition-colors"
          >
            Ir para Login
          </button>
        </div>
      </div>
    );
  }

  // Renderizar conteúdo protegido
  return <>{children}</>;
};
