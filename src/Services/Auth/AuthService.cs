using Shared.Entities;
using Shared.Interfaces;

namespace Auth.Service;

/// <summary>
/// Provides authentication services including user registration and login.
/// </summary>
public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtTokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        PasswordHasher passwordHasher,
        JwtTokenService tokenService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    /// <summary>
    /// Registers a new user with the provided information.
    /// </summary>
    /// <param name="name">The user's name</param>
    /// <param name="email">The user's email address</param>
    /// <param name="password">The user's password (will be hashed)</param>
    /// <returns>A result containing the user ID and authentication token if successful</returns>
    public async Task<RegisterResult> RegisterAsync(string name, string email, string password)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            return RegisterResult.Failed("Name is required");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return RegisterResult.Failed("Email is required");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return RegisterResult.Failed("Password is required");
        }

        if (password.Length < 8)
        {
            return RegisterResult.Failed("Password must be at least 8 characters long");
        }

        // Check if email already exists
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
        {
            return RegisterResult.Failed("Email is already registered");
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Save user to database
        await _userRepository.CreateAsync(user);

        // Generate token
        var token = _tokenService.GenerateToken(user.Id, user.Email, user.Name);
        var expiresAt = DateTime.UtcNow.Add(_tokenService.TokenExpiration);

        return RegisterResult.Successful(user.Id, token, expiresAt);
    }

    /// <summary>
    /// Authenticates a user with email and password.
    /// Implements account lockout after 5 failed attempts (30 minutes lockout).
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <param name="password">The user's password</param>
    /// <returns>A result containing user information and authentication token if successful</returns>
    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(email))
        {
            return LoginResult.Failed("Email is required");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return LoginResult.Failed("Password is required");
        }

        // Find user by email
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return LoginResult.Failed("Invalid email or password");
        }

        // Check if account is locked out
        if (user.IsLockedOut)
        {
            var remainingMinutes = (int)(user.LockoutEnd!.Value - DateTime.UtcNow).TotalMinutes;
            return LoginResult.LockedOut($"Account is locked due to too many failed login attempts. Try again in {remainingMinutes} minutes.");
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            // Increment failed login attempts
            user.FailedLoginAttempts++;
            user.UpdatedAt = DateTime.UtcNow;

            // Lock account after 5 failed attempts
            if (user.FailedLoginAttempts >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(30);
                await _userRepository.UpdateAsync(user);
                return LoginResult.LockedOut("Account has been locked due to too many failed login attempts. Try again in 30 minutes.");
            }

            await _userRepository.UpdateAsync(user);
            return LoginResult.Failed("Invalid email or password");
        }

        // Successful login - reset failed attempts
        if (user.FailedLoginAttempts > 0 || user.LockoutEnd.HasValue)
        {
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
        }

        // Generate token
        var token = _tokenService.GenerateToken(user.Id, user.Email, user.Name);
        var expiresAt = DateTime.UtcNow.Add(_tokenService.TokenExpiration);

        return LoginResult.Successful(user.Id, user.Name, user.Email, token, expiresAt);
    }
}

/// <summary>
/// Result of a user registration operation.
/// </summary>
public record RegisterResult
{
    public bool Success { get; init; }
    public Guid UserId { get; init; }
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;

    private RegisterResult() { }

    public static RegisterResult Successful(Guid userId, string token, DateTime expiresAt) =>
        new() { Success = true, UserId = userId, Token = token, ExpiresAt = expiresAt };

    public static RegisterResult Failed(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };
}

/// <summary>
/// Result of a user login operation.
/// </summary>
public record LoginResult
{
    public bool Success { get; init; }
    public bool IsLockedOut { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;

    private LoginResult() { }

    public static LoginResult Successful(Guid userId, string name, string email, string token, DateTime expiresAt) =>
        new() { Success = true, UserId = userId, Name = name, Email = email, Token = token, ExpiresAt = expiresAt };

    public static LoginResult Failed(string errorMessage) =>
        new() { Success = false, ErrorMessage = errorMessage };

    public static LoginResult LockedOut(string errorMessage) =>
        new() { Success = false, IsLockedOut = true, ErrorMessage = errorMessage };
}
