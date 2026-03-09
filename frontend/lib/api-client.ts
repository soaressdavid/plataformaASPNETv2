import axios, { AxiosInstance, AxiosError } from 'axios';
import toast from 'react-hot-toast';

// API base URL - can be configured via environment variable
// Usando ApiGateway (porta 5000) - CORRIGIDO!
const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

// Create axios instance with default configuration
const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 30000, // 30 seconds
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true, // Enable credentials for CORS
});

// Request interceptor to add authentication token
apiClient.interceptors.request.use(
  (config) => {
    console.log('API Request:', {
      method: config.method,
      url: config.url,
      baseURL: config.baseURL,
      fullURL: `${config.baseURL}${config.url}`
    });
    
    // Get token from localStorage (will be set after login)
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem('auth_token');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
    }
    return config;
  },
  (error) => {
    console.error('API Request Error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor to handle errors globally
apiClient.interceptors.response.use(
  (response) => {
    console.log('API Response:', {
      status: response.status,
      url: response.config.url,
      data: response.data
    });
    return response;
  },
  (error: AxiosError) => {
    console.log('API Client Interceptor - Error caught:', {
      status: error.response?.status,
      message: error.message,
      code: error.code,
      data: error.response?.data,
      url: error.config?.url
    });
    
    // Handle 401 Unauthorized - ONLY clear token, don't redirect
    // Let each page decide what to do with 401 errors
    if (error.response?.status === 401) {
      if (typeof window !== 'undefined') {
        // Only clear token if it's an auth endpoint (login/register)
        // For other endpoints, the token might just be expired
        const url = error.config?.url || '';
        if (url.includes('/auth/login') || url.includes('/auth/register')) {
          // Don't clear token on login/register failures
          console.log('API Client: Login/register failed, keeping existing token');
        } else {
          // For other endpoints, token might be invalid
          console.log('API Client: 401 on protected endpoint, token might be invalid');
          // Don't auto-redirect, let the page handle it
        }
      }
    }
    
    // Don't show toasts or redirect - let the calling code handle errors
    // This prevents duplicate error messages and unwanted redirects
    
    return Promise.reject(error);
  }
);

export default apiClient;
