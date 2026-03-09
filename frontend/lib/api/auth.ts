import apiClient from '../api-client';
import { RegisterRequest, RegisterResponse, LoginRequest, LoginResponse } from '../types';

export const authApi = {
  /**
   * Register a new user
   */
  register: async (data: RegisterRequest): Promise<RegisterResponse> => {
    const response = await apiClient.post<RegisterResponse>('/api/auth/register', data);
    return response.data;
  },

  /**
   * Login with email and password
   */
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    console.log('authApi.login: Sending login request...');
    const response = await apiClient.post<LoginResponse>('/api/auth/login', data);
    console.log('authApi.login: Response received:', {
      hasData: !!response.data,
      hasToken: !!response.data?.token,
      tokenLength: response.data?.token?.length,
      userId: response.data?.userId,
      name: response.data?.name
    });
    return response.data;
  },

  /**
   * Logout (clear local token)
   */
  logout: () => {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('auth_token');
    }
  },

  /**
   * Get current auth token
   */
  getToken: (): string | null => {
    if (typeof window !== 'undefined') {
      // Tentar localStorage primeiro
      let token = localStorage.getItem('auth_token');
      
      // Se não encontrar, tentar sessionStorage
      if (!token) {
        token = sessionStorage.getItem('auth_token');
        console.log('authApi.getToken: Token not in localStorage, found in sessionStorage:', !!token);
        
        // Se encontrou no sessionStorage, restaurar no localStorage
        if (token) {
          localStorage.setItem('auth_token', token);
        }
      }
      
      // Se ainda não encontrar, tentar recuperar do auth_user
      if (!token) {
        const userStr = localStorage.getItem('auth_user');
        if (userStr) {
          try {
            const userData = JSON.parse(userStr);
            if (userData.token) {
              token = userData.token;
              console.log('authApi.getToken: Token recovered from auth_user');
              localStorage.setItem('auth_token', token);
              sessionStorage.setItem('auth_token', token);
            }
          } catch (e) {
            console.error('authApi.getToken: Error parsing auth_user:', e);
          }
        }
      }
      
      console.log('authApi.getToken: Retrieved token', { hasToken: !!token, tokenLength: token?.length });
      return token;
    }
    console.log('authApi.getToken: Window is undefined, returning null');
    return null;
  },

  /**
   * Set auth token
   */
  setToken: (token: string) => {
    console.log('authApi.setToken: Saving token...', { tokenLength: token?.length });
    if (typeof window !== 'undefined') {
      try {
        // Salvar o token normalmente
        localStorage.setItem('auth_token', token);
        
        // BACKUP: Também salvar no sessionStorage
        sessionStorage.setItem('auth_token', token);
        
        // BACKUP 2: Salvar junto com o user (será feito no AuthContext)
        
        console.log('authApi.setToken: Token saved to localStorage and sessionStorage');
        
        // Verificar
        const savedLocal = localStorage.getItem('auth_token');
        const savedSession = sessionStorage.getItem('auth_token');
        console.log('authApi.setToken: Verification', {
          localStorage: !!savedLocal,
          sessionStorage: !!savedSession
        });
      } catch (error) {
        console.error('authApi.setToken: Error saving token:', error);
      }
    }
  },
};
