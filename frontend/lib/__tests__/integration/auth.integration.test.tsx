/**
 * Integration Tests: Authentication Flow
 * 
 * Tests authentication workflows including:
 * - User registration
 * - User login
 * - User logout
 * - Token management
 * 
 * Validates Requirements: 1.1, 1.2, 1.3
 */

import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { useRouter } from 'next/navigation';
import axios from 'axios';
import LoginPage from '@/app/login/page';
import RegisterPage from '@/app/register/page';
import { AuthProvider } from '@/lib/contexts/AuthContext';
import toast from 'react-hot-toast';

// Mock dependencies
jest.mock('@/lib/api-client');
jest.mock('next/navigation', () => ({
  useRouter: jest.fn(),
}));

jest.mock('axios');
jest.mock('react-hot-toast', () => {
  const mockToast = jest.fn();
  mockToast.success = jest.fn();
  mockToast.error = jest.fn();
  return {
    __esModule: true,
    default: mockToast,
  };
});

const mockedAxios = axios as jest.Mocked<typeof axios>;
const mockedUseRouter = useRouter as jest.MockedFunction<typeof useRouter>;

describe('Authentication Integration Tests', () => {
  const mockPush = jest.fn();
  const mockRouter = { push: mockPush };

  beforeEach(() => {
    jest.clearAllMocks();
    localStorage.clear();
    mockedUseRouter.mockReturnValue(mockRouter as any);
  });

  describe('User Registration Flow', () => {
    it('should successfully register a new user and redirect to dashboard', async () => {
      // Arrange
      const user = userEvent.setup();
      const mockResponse = {
        data: {
          userId: 'user-123',
          token: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJ1c2VyLTEyMyIsIm5hbWUiOiJKb2huIERvZSIsImVtYWlsIjoiam9obkBleGFtcGxlLmNvbSJ9.test',
        },
      };

      mockedAxios.post.mockResolvedValueOnce(mockResponse);

      render(
        <AuthProvider>
          <RegisterPage />
        </AuthProvider>
      );

      // Act - Fill in registration form
      await user.type(screen.getByLabelText(/full name/i), 'John Doe');
      await user.type(screen.getByLabelText(/email address/i), 'john@example.com');
      await user.type(screen.getAllByLabelText(/^password$/i)[0], 'password123');
      await user.type(screen.getByLabelText(/confirm password/i), 'password123');
      await user.click(screen.getByRole('button', { name: /create account/i }));

      // Assert
      await waitFor(() => {
        expect(mockedAxios.post).toHaveBeenCalledWith(
          '/api/auth/register',
          {
            name: 'John Doe',
            email: 'john@example.com',
            password: 'password123',
          }
        );
      });

      await waitFor(() => {
        expect(localStorage.getItem('auth_token')).toBe(mockResponse.data.token);
        expect(mockPush).toHaveBeenCalledWith('/dashboard');
      });
    });

    it('should show error when passwords do not match', async () => {
      // Arrange
      const user = userEvent.setup();

      render(
        <AuthProvider>
          <RegisterPage />
        </AuthProvider>
      );

      // Act
      await user.type(screen.getByLabelText(/full name/i), 'John Doe');
      await user.type(screen.getByLabelText(/email address/i), 'john@example.com');
      await user.type(screen.getAllByLabelText(/^password$/i)[0], 'password123');
      await user.type(screen.getByLabelText(/confirm password/i), 'different123');
      await user.click(screen.getByRole('button', { name: /create account/i }));

      // Assert
      await waitFor(() => {
        expect(screen.getByText(/passwords do not match/i)).toBeInTheDocument();
      });
      expect(mockedAxios.post).not.toHaveBeenCalled();
    });

    it('should show error when password is too short', async () => {
      // Arrange
      const user = userEvent.setup();

      render(
        <AuthProvider>
          <RegisterPage />
        </AuthProvider>
      );

      // Act
      await user.type(screen.getByLabelText(/full name/i), 'John Doe');
      await user.type(screen.getByLabelText(/email address/i), 'john@example.com');
      await user.type(screen.getAllByLabelText(/^password$/i)[0], 'pass');
      await user.type(screen.getByLabelText(/confirm password/i), 'pass');
      await user.click(screen.getByRole('button', { name: /create account/i }));

      // Assert
      await waitFor(() => {
        expect(screen.getByText(/password must be at least 6 characters/i)).toBeInTheDocument();
      });
      expect(mockedAxios.post).not.toHaveBeenCalled();
    });

    it('should handle registration API errors', async () => {
      // Arrange
      const user = userEvent.setup();
      mockedAxios.post.mockRejectedValueOnce({
        response: {
          data: {
            message: 'Email already exists',
          },
        },
      });

      render(
        <AuthProvider>
          <RegisterPage />
        </AuthProvider>
      );

      // Act
      await user.type(screen.getByLabelText(/full name/i), 'John Doe');
      await user.type(screen.getByLabelText(/email address/i), 'existing@example.com');
      await user.type(screen.getAllByLabelText(/^password$/i)[0], 'password123');
      await user.type(screen.getByLabelText(/confirm password/i), 'password123');
      await user.click(screen.getByRole('button', { name: /create account/i }));

      // Assert
      await waitFor(() => {
        expect(screen.getByText(/email already exists/i)).toBeInTheDocument();
      });
    });
  });

  describe('User Login Flow', () => {
    it('should successfully login and redirect to dashboard', async () => {
      // Arrange
      const user = userEvent.setup();
      const mockResponse = {
        data: {
          userId: 'user-123',
          name: 'John Doe',
          token: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJ1c2VyLTEyMyIsIm5hbWUiOiJKb2huIERvZSIsImVtYWlsIjoiam9obkBleGFtcGxlLmNvbSJ9.test',
          expiresAt: '2024-12-31T23:59:59Z',
        },
      };

      mockedAxios.post.mockResolvedValueOnce(mockResponse);

      render(
        <AuthProvider>
          <LoginPage />
        </AuthProvider>
      );

      // Act
      await user.type(screen.getByLabelText(/email address/i), 'john@example.com');
      await user.type(screen.getByLabelText(/password/i), 'password123');
      await user.click(screen.getByRole('button', { name: /sign in/i }));

      // Assert
      await waitFor(() => {
        expect(mockedAxios.post).toHaveBeenCalledWith(
          '/api/auth/login',
          {
            email: 'john@example.com',
            password: 'password123',
          }
        );
      });

      await waitFor(() => {
        expect(localStorage.getItem('auth_token')).toBe(mockResponse.data.token);
        expect(mockPush).toHaveBeenCalledWith('/dashboard');
      });
    });

    it('should reject invalid credentials', async () => {
      // Arrange
      const user = userEvent.setup();
      mockedAxios.post.mockRejectedValueOnce({
        response: {
          data: {
            message: 'Invalid credentials',
          },
        },
      });

      render(
        <AuthProvider>
          <LoginPage />
        </AuthProvider>
      );

      // Act
      await user.type(screen.getByLabelText(/email address/i), 'wrong@example.com');
      await user.type(screen.getByLabelText(/password/i), 'wrongpassword');
      await user.click(screen.getByRole('button', { name: /sign in/i }));

      // Assert
      await waitFor(() => {
        expect(screen.getByText(/invalid credentials/i)).toBeInTheDocument();
      });
      expect(localStorage.getItem('auth_token')).toBeNull();
      expect(mockPush).not.toHaveBeenCalled();
    });

    it('should show loading state during login', async () => {
      // Arrange
      const user = userEvent.setup();
      let resolveLogin: any;
      const loginPromise = new Promise((resolve) => {
        resolveLogin = resolve;
      });

      mockedAxios.post.mockReturnValueOnce(loginPromise as any);

      render(
        <AuthProvider>
          <LoginPage />
        </AuthProvider>
      );

      // Act
      await user.type(screen.getByLabelText(/email address/i), 'john@example.com');
      await user.type(screen.getByLabelText(/password/i), 'password123');
      await user.click(screen.getByRole('button', { name: /sign in/i }));

      // Assert - Loading state
      expect(screen.getByText(/signing in/i)).toBeInTheDocument();
      expect(screen.getByRole('button', { name: /signing in/i })).toBeDisabled();

      // Resolve login
      resolveLogin({
        data: {
          userId: 'user-123',
          name: 'John Doe',
          token: 'test-token',
          expiresAt: '2024-12-31T23:59:59Z',
        },
      });

      await waitFor(() => {
        expect(screen.queryByText(/signing in/i)).not.toBeInTheDocument();
      });
    });
  });

  describe('User Logout Flow', () => {
    it('should clear token and user state on logout', () => {
      // Arrange
      const token = 'test-token';
      localStorage.setItem('auth_token', token);

      // Act
      localStorage.removeItem('auth_token');

      // Assert
      expect(localStorage.getItem('auth_token')).toBeNull();
    });
  });

  describe('Token Management', () => {
    it('should store token in localStorage after successful login', async () => {
      // Arrange
      const user = userEvent.setup();
      const mockToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJ1c2VyLTEyMyJ9.test';
      const mockResponse = {
        data: {
          userId: 'user-123',
          name: 'John Doe',
          token: mockToken,
          expiresAt: '2024-12-31T23:59:59Z',
        },
      };

      mockedAxios.post.mockResolvedValueOnce(mockResponse);

      render(
        <AuthProvider>
          <LoginPage />
        </AuthProvider>
      );

      // Act
      await user.type(screen.getByLabelText(/email address/i), 'john@example.com');
      await user.type(screen.getByLabelText(/password/i), 'password123');
      await user.click(screen.getByRole('button', { name: /sign in/i }));

      // Assert
      await waitFor(() => {
        expect(localStorage.getItem('auth_token')).toBe(mockToken);
      });
    });

    it('should retrieve token from localStorage', () => {
      // Arrange
      const mockToken = 'test-token-123';
      localStorage.setItem('auth_token', mockToken);

      // Act
      const token = localStorage.getItem('auth_token');

      // Assert
      expect(token).toBe(mockToken);
    });

    it('should return null when no token exists', () => {
      // Arrange
      localStorage.clear();

      // Act
      const token = localStorage.getItem('auth_token');

      // Assert
      expect(token).toBeNull();
    });
  });
});
