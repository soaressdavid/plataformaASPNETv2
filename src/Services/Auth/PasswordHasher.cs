namespace Auth.Service;

/// <summary>
/// Provides password hashing and verification using BCrypt with a work factor of 12.
/// </summary>
public class PasswordHasher
{
    private const int WorkFactor = 12;

    /// <summary>
    /// Hashes a password using BCrypt with salt (work factor: 12).
    /// Each call produces a different hash due to unique salt generation.
    /// </summary>
    /// <param name="password">The plaintext password to hash</param>
    /// <returns>The BCrypt hash of the password</returns>
    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
        }

        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    /// <summary>
    /// Verifies a password against a BCrypt hash.
    /// </summary>
    /// <param name="password">The plaintext password to verify</param>
    /// <param name="hash">The BCrypt hash to verify against</param>
    /// <returns>True if the password matches the hash, false otherwise</returns>
    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
        }

        if (string.IsNullOrEmpty(hash))
        {
            throw new ArgumentException("Hash cannot be null or empty", nameof(hash));
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            // Invalid hash format
            return false;
        }
    }
}
