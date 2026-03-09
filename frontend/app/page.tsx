'use client';

import Link from 'next/link';
import { useAuth } from '@/lib/contexts/AuthContext';
import { Icons } from '@/lib/components/Icons';

export default function Home() {
  const { isAuthenticated } = useAuth();

  return (
    <div className="min-h-screen bg-white">
      {/* Hero Section */}
      <div className="relative">
        <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 pt-24 pb-16 text-center">
          <div className="inline-flex items-center gap-2 px-3 py-1.5 bg-gray-100 text-gray-700 rounded-full text-sm font-medium mb-8">
            <Icons.Code className="w-4 h-4" />
            <span>Plataforma de Aprendizado ASP.NET Core</span>
          </div>
          
          <h1 className="text-5xl sm:text-6xl lg:text-7xl font-bold mb-6 leading-tight text-gray-900">
            Aprenda Construindo<br />Projetos Reais
          </h1>
          
          <p className="text-xl text-gray-600 mb-10 max-w-2xl mx-auto">
            Cursos práticos, projetos reais e desafios de código para dominar o desenvolvimento com ASP.NET Core
          </p>

          <div className="flex flex-col sm:flex-row justify-center gap-3 mb-16">
            {isAuthenticated ? (
              <Link
                href="/dashboard"
                className="inline-flex items-center justify-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 transition-colors"
              >
                Continuar Aprendendo
                <Icons.ArrowRight className="w-4 h-4" />
              </Link>
            ) : (
              <>
                <Link
                  href="/register"
                  className="inline-flex items-center justify-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 transition-colors"
                >
                  Começar Grátis
                  <Icons.ArrowRight className="w-4 h-4" />
                </Link>
                <Link
                  href="/login"
                  className="px-6 py-3 bg-white text-gray-900 rounded-lg font-medium border border-gray-300 hover:border-gray-400 transition-colors"
                >
                  Entrar
                </Link>
              </>
            )}
          </div>

          {/* Stats */}
          <div className="grid grid-cols-3 gap-6 max-w-2xl mx-auto">
            <div className="text-center">
              <div className="text-4xl font-bold text-gray-900">50+</div>
              <div className="text-sm text-gray-600 mt-1">Desafios</div>
            </div>
            <div className="text-center">
              <div className="text-4xl font-bold text-gray-900">20+</div>
              <div className="text-sm text-gray-600 mt-1">Cursos</div>
            </div>
            <div className="text-center">
              <div className="text-4xl font-bold text-gray-900">10+</div>
              <div className="text-sm text-gray-600 mt-1">Projetos</div>
            </div>
          </div>
        </div>
      </div>

      {/* Features Section */}
      <div className="bg-gray-50 py-20">
        <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-16">
            <h2 className="text-3xl font-bold text-gray-900 mb-3">Como Funciona</h2>
            <p className="text-gray-600">Aprenda de forma prática e estruturada</p>
          </div>

          <div className="grid md:grid-cols-3 gap-8">
            <div className="bg-white rounded-xl p-6 border border-gray-200">
              <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center mb-4">
                <Icons.Code className="w-6 h-6 text-blue-600" />
              </div>
              <h3 className="text-lg font-semibold text-gray-900 mb-2">Desafios de Código</h3>
              <p className="text-gray-600 text-sm leading-relaxed">
                Resolva problemas reais com feedback instantâneo e testes automatizados
              </p>
            </div>

            <div className="bg-white rounded-xl p-6 border border-gray-200">
              <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center mb-4">
                <Icons.BookOpen className="w-6 h-6 text-green-600" />
              </div>
              <h3 className="text-lg font-semibold text-gray-900 mb-2">Cursos Práticos</h3>
              <p className="text-gray-600 text-sm leading-relaxed">
                Siga trilhas estruturadas do básico ao avançado com exemplos práticos
              </p>
            </div>

            <div className="bg-white rounded-xl p-6 border border-gray-200">
              <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center mb-4">
                <Icons.Rocket className="w-6 h-6 text-purple-600" />
              </div>
              <h3 className="text-lg font-semibold text-gray-900 mb-2">Projetos Completos</h3>
              <p className="text-gray-600 text-sm leading-relaxed">
                Construa aplicações reais e adicione ao seu portfólio profissional
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Features List */}
      <div className="py-20">
        <div className="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="grid md:grid-cols-2 gap-12 items-center">
            <div>
              <h2 className="text-3xl font-bold text-gray-900 mb-6">Recursos da Plataforma</h2>
              <ul className="space-y-4">
                <li className="flex items-start gap-3">
                  <div className="w-6 h-6 bg-blue-100 rounded flex items-center justify-center shrink-0 mt-0.5">
                    <Icons.Check className="w-4 h-4 text-blue-600" />
                  </div>
                  <span className="text-gray-700">Revisão de código com IA e feedback personalizado</span>
                </li>
                <li className="flex items-start gap-3">
                  <div className="w-6 h-6 bg-blue-100 rounded flex items-center justify-center shrink-0 mt-0.5">
                    <Icons.Check className="w-4 h-4 text-blue-600" />
                  </div>
                  <span className="text-gray-700">IDE no navegador com autocompletar inteligente</span>
                </li>
                <li className="flex items-start gap-3">
                  <div className="w-6 h-6 bg-blue-100 rounded flex items-center justify-center shrink-0 mt-0.5">
                    <Icons.Check className="w-4 h-4 text-blue-600" />
                  </div>
                  <span className="text-gray-700">Execução segura de código em containers isolados</span>
                </li>
                <li className="flex items-start gap-3">
                  <div className="w-6 h-6 bg-blue-100 rounded flex items-center justify-center shrink-0 mt-0.5">
                    <Icons.Check className="w-4 h-4 text-blue-600" />
                  </div>
                  <span className="text-gray-700">Sistema de gamificação com XP e conquistas</span>
                </li>
                <li className="flex items-start gap-3">
                  <div className="w-6 h-6 bg-blue-100 rounded flex items-center justify-center shrink-0 mt-0.5">
                    <Icons.Check className="w-4 h-4 text-blue-600" />
                  </div>
                  <span className="text-gray-700">Acompanhamento detalhado de progresso</span>
                </li>
              </ul>
            </div>
            <div className="bg-gray-50 rounded-xl p-8 border border-gray-200">
              <div className="mb-6">
                <div className="w-14 h-14 bg-blue-100 rounded-lg flex items-center justify-center">
                  <Icons.Rocket className="w-8 h-8 text-blue-600" />
                </div>
              </div>
              <h3 className="text-2xl font-bold text-gray-900 mb-3">Comece Agora</h3>
              <p className="text-gray-600 mb-6">
                Junte-se a milhares de desenvolvedores aprendendo ASP.NET Core na prática
              </p>
              {!isAuthenticated && (
                <Link
                  href="/register"
                  className="inline-flex items-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 transition-colors"
                >
                  Criar Conta Grátis
                  <Icons.ArrowRight className="w-4 h-4" />
                </Link>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
