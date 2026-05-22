using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TokoApi.Tests.Infrastructure;

/// <summary>
/// Generates valid JWT tokens for integration tests using the same key as appsettings.json.
/// This avoids the need to call the /api/auth/login endpoint before every test.
/// </summary>
public static class JwtTestHelper
{
    // Must match the value in appsettings.json
    private const string TestJwtKey = "BelajarNetSuperSecretKey_ChangeMeInProduction_2026!";
    private const string Issuer = "TokoApi";
    private const string Audience = "TokoFrontend";

    public static string GenerateTestToken(string role = "Admin", string username = "test-user")
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TestJwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
