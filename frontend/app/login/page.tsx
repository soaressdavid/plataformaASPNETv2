'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';
import { useAuth } from '@/lib/contexts/AuthContext';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [loginSuccess, setLoginSuccess] = useState(false);
  const { login, isAuthenticated, user } = useAuth();
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log('=== FORM SUBMIT STARTED ===');
    console.log('Email digitado:', email);
    
    setError('');
    setIsLoading(true);

    try {
      console.log('Calling login function...');
      await login({ email, password });
      console.log('Login function completed successfully');
      
      // Mostrar sucesso SEM redirecionar automaticamente
      setError('');
      setLoginSuccess(true);
      
      // NÃO redirecionar automaticamente
      // router.push('/dashboard');
    } catch (err: any) {
      console.error('=== LOGIN ERROR ===');
      console.error('Full error object:', err);
      console.error('Error response:', err.response);
      console.error('Error response data:', err.response?.data);
      console.error('Error response status:', err.response?.status);
      console.error('Error message:', err.message);
      
      let errorMessage = 'Erro ao fazer login. Verifique suas credenciais.';
      
      if (err.response?.status === 401) {
        errorMessage = 'Email ou senha incorretos. Tente: teste@teste.com / Teste123!@#';
      } else if (err.response?.data?.message) {
        errorMessage = err.response.data.message;
      } else if (err.response?.data?.error) {
        errorMessage = err.response.data.error;
      } else if (err.message) {
        errorMessage = err.message;
      }
      
      console.log('Setting error message:', errorMessage);
      setError(errorMessage);
    } finally {
      console.log('Setting isLoading to false');
      setIsLoading(false);
      console.log('=== FORM SUBMIT ENDED ===');
    }
  };

  const handleGoToDashboard = () => {
    console.log('Manual navigation to dashboard');
    // Usar window.location para forçar hard reload
    window.location.href = '/dashboard-test';
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 via-white to-purple-50 px-4">
      <div className="max-w-md w-full">
        {/* Logo */}
        <div className="text-center mb-8">
          <Link href="/" className="inline-flex items-center space-x-2">
            <div className="w-12 h-12 bg-gradient-to-br from-blue-600 to-purple-600 rounded-xl flex items-center justify-center text-white font-bold text-xl">
              A
            </div>
            <span className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
              ASP.NET Learning
            </span>
          </Link>
        </div>

        {/* Card */}
        <div className="bg-white rounded-2xl shadow-xl p-8 border border-gray-100">
          <div className="text-center mb-8">
            <h1 className="text-3xl font-bold text-gray-900">Bem-vindo de Volta</h1>
            <p className="mt-2 text-gray-600">
              Entre para continuar sua jornada de aprendizado
            </p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label htmlFor="email" className="block text-sm font-semibold text-gray-700 mb-2">
                Endereço de Email
              </label>
              <input
                id="email"
                name="email"
                type="email"
                autoComplete="email"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all"
                placeholder="voce@exemplo.com"
              />
            </div>

            <div>
              <label htmlFor="password" className="block text-sm font-semibold text-gray-700 mb-2">
                Senha
              </label>
              <input
                id="password"
                name="password"
                type="password"
                autoComplete="current-password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all"
                placeholder="••••••••"
              />
            </div>

            {error && (
              <div className="rounded-xl bg-red-50 border border-red-200 p-4">
                <p className="text-sm text-red-800">{error}</p>
              </div>
            )}

            {loginSuccess && (
              <div className="rounded-xl bg-green-50 border border-green-200 p-4">
                <p className="text-sm text-green-800 font-semibold mb-2">✓ Login bem-sucedido!</p>
                <p className="text-xs text-green-700 mb-3">
                  Autenticado: {isAuthenticated ? '✓ Sim' : '✗ Não'}<br/>
                  Usuário: {user?.name || 'N/A'}
                </p>
                <button
                  type="button"
                  onClick={handleGoToDashboard}
                  className="w-full py-2 px-4 bg-green-600 text-white rounded-lg font-semibold hover:bg-green-700 transition-colors"
                >
                  Ir para Dashboard →
                </button>
              </div>
            )}

            <button
              type="submit"
              disabled={isLoading}
              className="w-full py-3 px-4 bg-gradient-to-r from-blue-600 to-purple-600 text-white rounded-xl font-semibold hover:shadow-lg hover:scale-105 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:scale-100"
            >
              {isLoading ? 'Entrando...' : 'Entrar'}
            </button>

            <div className="text-center pt-4">
              <p className="text-sm text-gray-600">
                Não tem uma conta?{' '}
                <Link href="/register" className="font-semibold text-blue-600 hover:text-purple-600 transition-colors">
                  Cadastre-se grátis
                </Link>
              </p>
            </div>
          </form>
        </div>

        {/* Demo Credentials */}
        <div className="mt-6 text-center">
          <div className="inline-block bg-blue-50 border border-blue-200 rounded-xl px-4 py-3">
            <p className="text-xs text-blue-800 font-medium mb-1">Credenciais de Demonstração</p>
            <p className="text-xs text-blue-600">teste@teste.com / Teste123!@#</p>
          </div>
        </div>
      </div>
    </div>
  );
}
