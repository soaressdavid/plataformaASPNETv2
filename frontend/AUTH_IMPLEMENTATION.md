# Authentication Implementation

This document describes the authentication system implemented for the ASP.NET Core Learning Platform frontend.

## Components

### 1. AuthContext (`lib/contexts/AuthContext.tsx`)

A React Context that manages authentication state across the application.

**Features:**
- User session management
- Token storage in localStorage
- Login/Register/Logout functionality
- Automatic token validation on mount
- JWT token decoding for user information

**Usage:**
```tsx
import { useAuth } from '@/lib/contexts/AuthContext';

function MyComponent() {
  const { user, isAuthenticated, login, logout } = useAuth();
  // ...
}
```

### 2. ProtectedRoute Component (`lib/components/ProtectedRoute.tsx`)

A wrapper component that protects routes requiring authentication.

**Features:**
- Redirects unauthenticated users to login page
- Shows loading state while checking authentication
- Prevents rendering of protected content until authenticated

**Usage:**
```tsx
import { ProtectedRoute } from '@/lib/components/ProtectedRoute';

export default function DashboardPage() {
  return (
    <ProtectedRoute>
      <div>Protected content here</div>
    </ProtectedRoute>
  );
}
```

### 3. Login Page (`app/login/page.tsx`)

User login interface with email and password fields.

**Features:**
- Form validation
- Error handling
- Loading states
- Link to registration page
- Redirects to dashboard on successful login

**Route:** `/login`

### 4. Registration Page (`app/register/page.tsx`)

User registration interface with name, email, and password fields.

**Features:**
- Form validation (password matching, minimum length)
- Error handling
- Loading states
- Link to login page
- Redirects to dashboard on successful registration

**Route:** `/register`

## Authentication Flow

### Registration Flow
1. User fills out registration form (name, email, password)
2. Frontend validates password match and length
3. API call to `/api/auth/register`
4. On success: token stored in localStorage, user redirected to dashboard
5. On error: error message displayed

### Login Flow
1. User enters email and password
2. API call to `/api/auth/login`
3. On success: token stored in localStorage, user redirected to dashboard
4. On error: error message displayed

### Protected Route Access
1. User navigates to protected route
2. ProtectedRoute checks authentication status
3. If authenticated: content rendered
4. If not authenticated: redirect to login page
5. Loading state shown during check

### Logout Flow
1. User clicks logout button
2. Token removed from localStorage
3. User state cleared
4. Redirect to login page

## Token Management

Tokens are stored in localStorage with the key `auth_token`. The API client automatically includes the token in the Authorization header for all requests.

**Token Structure:**
```
Authorization: Bearer <token>
```

**Token Validation:**
- Tokens are validated on application mount
- Invalid tokens are automatically cleared
- 401 responses trigger automatic logout and redirect

## Security Considerations

1. **Token Storage:** Tokens are stored in localStorage (consider httpOnly cookies for production)
2. **Token Expiration:** Tokens expire after 24 hours (configured in backend)
3. **HTTPS:** Always use HTTPS in production to protect tokens in transit
4. **XSS Protection:** Sanitize all user inputs to prevent XSS attacks
5. **CSRF Protection:** Consider implementing CSRF tokens for state-changing operations

## API Integration

The authentication system integrates with the backend API through the `authApi` service (`lib/api/auth.ts`):

- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login

## Environment Variables

Configure the API base URL in `.env.local`:

```
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## Testing

To test the authentication system:

1. Start the backend API server
2. Start the frontend development server: `npm run dev`
3. Navigate to `/register` to create an account
4. Navigate to `/login` to sign in
5. Access `/dashboard` to verify protected route functionality

## Future Enhancements

- [ ] Implement refresh token mechanism
- [ ] Add "Remember Me" functionality
- [ ] Implement password reset flow
- [ ] Add social authentication (Google, GitHub)
- [ ] Implement two-factor authentication
- [ ] Add session timeout warnings
- [ ] Implement account verification via email
