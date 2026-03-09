'use client';

import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { authApi } from '../api/auth';
import { LoginRequest, RegisterRequest, LoginResponse } from '../types';
import toast from 'react-hot-toast';

interface User {
  userId: string;
  name: string;
  email: string;
  token?: string; // Token como backup
}

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (credentials: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Log state changes
  useEffect(() => {
    console.log('AuthContext: State changed - user:', user, 'isAuthenticated:', !!user, 'isLoading:', isLoading);
  }, [user, isLoading]);

  // Check for existing token on mount (client-side only)
  useEffect(() => {
    console.log('AuthContext: Initializing...');
    
    // Ensure we're on the client side
    if (typeof window === 'undefined') {
      console.log('AuthContext: Server side, skipping initialization');
      setIsLoading(false);
      return;
    }

    // NOVA ESTRATÉGIA: Recuperar APENAS do auth_user que contém o token
    const storedUser = localStorage.getItem('auth_user');
    
    console.log('AuthContext: Checking stored user', { hasStoredUser: !!storedUser });
    
    if (storedUser) {
      try {
        const userData = JSON.parse(storedUser);
        console.log('AuthContext: User data found:', { userId: userData.userId, name: userData.name, hasToken: !!userData.token });
        
        // Verificar se tem token no userData
        if (userData.token) {
          console.log('AuthContext: Restoring session with token from user data');
          
          // Restaurar token no localStorage e sessionStorage
          localStorage.setItem('auth_token', userData.token);
          sessionStorage.setItem('auth_token', userData.token);
          
          // Setar usuário
          setUser(userData);
          console.log('AuthContext: Session restored successfully');
        } else {
          console.log('AuthContext: User data found but no token, clearing session');
          localStorage.removeItem('auth_user');
          sessionStorage.removeItem('auth_user');
        }
      } catch (error) {
        console.error('Failed to restore user session:', error);
        localStorage.removeItem('auth_user');
        sessionStorage.removeItem('auth_user');
      }
    } else {
      console.log('AuthContext: No stored user found');
    }
    
    setIsLoading(false);
    console.log('AuthContext: Initialization complete');
  }, []);

  const login = async (credentials: LoginRequest) => {
    try {
      console.log('AuthContext: Starting login...');
      const response: LoginResponse = await authApi.login(credentials);
      console.log('AuthContext: Login API response received:', { userId: response.userId, name: response.name });
      
      console.log('AuthContext: Saving token to localStorage...');
      authApi.setToken(response.token);
      
      const userData = {
        userId: response.userId,
        name: response.name,
        email: credentials.email,
        token: response.token, // BACKUP: Salvar token junto com user
      };
      
      console.log('AuthContext: Setting user state:', userData);
      setUser(userData);
      
      // Store user data in localStorage for persistence
      if (typeof window !== 'undefined') {
        localStorage.setItem('auth_user', JSON.stringify(userData));
        sessionStorage.setItem('auth_user', JSON.stringify(userData)); // BACKUP
        console.log('AuthContext: User data saved to localStorage and sessionStorage');
        
        // Verify it was saved
        const saved = localStorage.getItem('auth_user');
        const savedToken = localStorage.getItem('auth_token');
        console.log('AuthContext: Verification', {
          hasUser: !!saved,
          hasToken: !!savedToken
        });
      }
      
      console.log('AuthContext: Login successful, user set:', userData);
      console.log('AuthContext: isAuthenticated should now be:', !!userData);
      // Don't show toast here - let the page handle success message
    } catch (error: any) {
      console.error('AuthContext: Login failed:', error);
      console.error('AuthContext: Error details:', {
        message: error.message,
        response: error.response?.data,
        status: error.response?.status
      });
      // Don't show toast here - let the page handle error display
      throw error;
    }
  };

  const register = async (data: RegisterRequest) => {
    try {
      console.log('AuthContext: Starting registration...');
      const response = await authApi.register(data);
      console.log('AuthContext: Registration API response received');
      
      authApi.setToken(response.token);
      const userData = {
        userId: response.userId,
        name: data.name,
        email: data.email,
      };
      setUser(userData);
      
      // Store user data in localStorage for persistence
      if (typeof window !== 'undefined') {
        localStorage.setItem('auth_user', JSON.stringify(userData));
      }
      
      console.log('AuthContext: Registration successful, user set:', userData);
      // Don't show toast here - let the page handle success message
    } catch (error: any) {
      console.error('AuthContext: Registration failed:', error);
      console.error('AuthContext: Error details:', {
        message: error.message,
        response: error.response?.data,
        status: error.response?.status
      });
      // Don't show toast here - let the page handle error display
      throw error;
    }
  };

  const logout = () => {
    authApi.logout();
    setUser(null);
    
    // Remove user data from localStorage and sessionStorage
    if (typeof window !== 'undefined') {
      localStorage.removeItem('auth_user');
      localStorage.removeItem('auth_token');
      sessionStorage.removeItem('auth_user');
      sessionStorage.removeItem('auth_token');
    }
    
    toast('You have been logged out.', {
      icon: 'ℹ️',
    });
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        isLoading,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
