'use client';

import React, { useState } from 'react';
import Link from 'next/link';
import { usePathname, useRouter } from 'next/navigation';
import { useAuth } from '@/lib/contexts/AuthContext';
import { Icons } from './Icons';

export function Navigation() {
  const pathname = usePathname();
  const router = useRouter();
  const { user, logout } = useAuth();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    router.push('/login');
    setMobileMenuOpen(false);
  };

  const navLinks = [
    { href: '/dashboard', label: 'Painel', icon: <Icons.ChartBar className="w-4 h-4" /> },
    { href: '/courses', label: 'Cursos', icon: <Icons.BookOpen className="w-4 h-4" /> },
    { href: '/challenges', label: 'Desafios', icon: <Icons.Code className="w-4 h-4" /> },
    { href: '/projects', label: 'Projetos', icon: <Icons.Rocket className="w-4 h-4" /> },
    { href: '/ide', label: 'IDE', icon: <Icons.Sparkles className="w-4 h-4" /> },
    { href: '/leaderboard', label: 'Ranking', icon: <Icons.Trophy className="w-4 h-4" /> },
  ];

  const isActive = (href: string) => {
    return pathname === href || pathname?.startsWith(href + '/');
  };

  const handleLinkClick = () => {
    setMobileMenuOpen(false);
  };

  return (
    <nav className="bg-[#161b22] border-b border-[#30363d] sticky top-0 z-50">
      <div className="px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-14">
          {/* Logo */}
          <div className="flex items-center">
            <Link href="/dashboard" className="flex items-center space-x-2">
              <div className="w-8 h-8 bg-blue-600 rounded-md flex items-center justify-center text-white font-bold text-sm">
                A
              </div>
              <span className="text-base font-semibold text-gray-100 hidden sm:block">
                ASP.NET Learning
              </span>
            </Link>
          </div>

          {/* Desktop Navigation - Centered */}
          <div className="hidden lg:flex items-center space-x-6">
            {navLinks.map((link) => (
              <Link
                key={link.href}
                href={link.href}
                className={`px-4 py-1.5 rounded-md text-sm font-medium transition-colors flex items-center space-x-2 ${
                  isActive(link.href)
                    ? 'bg-[#30363d] text-white'
                    : 'text-gray-400 hover:text-gray-200 hover:bg-[#21262d]'
                }`}
              >
                {link.icon}
                <span>{link.label}</span>
              </Link>
            ))}
          </div>

          {/* Desktop User Menu */}
          <div className="hidden lg:flex items-center space-x-3">
            <div className="flex items-center space-x-2 px-3 py-1.5 bg-[#21262d] rounded-md">
              <div className="w-6 h-6 bg-blue-600 rounded-full flex items-center justify-center text-white font-medium text-xs">
                {user?.name?.charAt(0).toUpperCase()}
              </div>
              <span className="text-gray-300 text-sm font-medium">{user?.name}</span>
            </div>
            <button
              onClick={handleLogout}
              className="px-3 py-1.5 text-sm font-medium text-gray-400 hover:text-gray-200 hover:bg-[#21262d] rounded-md transition-colors"
            >
              Sair
            </button>
          </div>

          {/* Mobile Menu Button */}
          <div className="flex items-center lg:hidden">
            <button
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
              className="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-gray-200 hover:bg-[#21262d] transition-colors"
              aria-expanded={mobileMenuOpen}
              aria-label="Toggle navigation menu"
            >
              {!mobileMenuOpen ? (
                <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
                </svg>
              ) : (
                <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                </svg>
              )}
            </button>
          </div>
        </div>
      </div>

      {/* Mobile Menu */}
      {mobileMenuOpen && (
        <div className="lg:hidden border-t border-[#30363d] bg-[#161b22]">
          <div className="px-4 pt-2 pb-3 space-y-1">
            {navLinks.map((link) => (
              <Link
                key={link.href}
                href={link.href}
                onClick={handleLinkClick}
                className={`flex items-center space-x-3 px-3 py-2 rounded-md text-base font-medium transition-colors ${
                  isActive(link.href)
                    ? 'bg-[#30363d] text-white'
                    : 'text-gray-400 hover:text-gray-200 hover:bg-[#21262d]'
                }`}
              >
                {link.icon}
                <span>{link.label}</span>
              </Link>
            ))}
          </div>
          <div className="pt-4 pb-3 border-t border-[#30363d] px-4">
            <div className="flex items-center space-x-3 mb-3">
              <div className="w-10 h-10 bg-blue-600 rounded-full flex items-center justify-center text-white font-medium">
                {user?.name?.charAt(0).toUpperCase()}
              </div>
              <div>
                <div className="text-base font-medium text-gray-100">{user?.name}</div>
                <div className="text-sm text-gray-400">{user?.email}</div>
              </div>
            </div>
            <button
              onClick={handleLogout}
              className="w-full text-left px-3 py-2 rounded-md text-base font-medium text-gray-400 hover:text-gray-200 hover:bg-[#21262d] transition-colors"
            >
              Sair
            </button>
          </div>
        </div>
      )}
    </nav>
  );
}
