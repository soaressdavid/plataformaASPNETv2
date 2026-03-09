using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Configuration;

/// <summary>
/// Configuration for JWT authentication in the API Gateway.
/// </summary>
public class JwtConfiguration
{
    /// <summary>
    /// Gets or sets the RSA public key in PEM format for token validation.
    /// </summary>
    public string? PublicKeyPem { get; set; }

    /// <summary>
    /// Creates an RsaSecurityKey from the configured public key.
    /// If no public key is configured, generates a new RSA key pair (for development only).
    /// </summary>
    public RsaSecurityKey GetPublicKey()
    {
        if (!string.IsNullOrEmpty(PublicKeyPem))
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(PublicKeyPem);
            return new RsaSecurityKey(rsa);
        }

        // For development: generate a new key pair
        // In production, this should be loaded from secure configuration
        var developmentRsa = RSA.Create(2048);
        return new RsaSecurityKey(developmentRsa);
    }
}
