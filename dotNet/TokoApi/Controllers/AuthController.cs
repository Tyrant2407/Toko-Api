using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokoApi.DTOs;

namespace TokoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Login and receive a JWT token.
    /// Demo credentials: rizky/admin123 (Admin) | putra/user123 (Customer)
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        string? role = null;

        // Demo user store — replace with a real User table + password hash in production
        if (request.Username.ToLower() == "rizky" && request.Password == "admin123")
            role = "Admin";
        else if (request.Username.ToLower() == "siti" && request.Password == "kasir123")
            role = "Kasir";
        else if (request.Username.ToLower() == "putra" && request.Password == "user123")
            role = "Customer";

        if (role == null)
            return Unauthorized(new { Message = "Username atau password salah!" });

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key tidak dikonfigurasi di appsettings.");
        var issuer = _configuration["Jwt:Issuer"] ?? "TokoApi";
        var audience = _configuration["Jwt:Audience"] ?? "TokoFrontend";

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = jwt,
            username = request.Username,
            role = role,
            expiresAt = DateTime.UtcNow.AddHours(8)
        });
    }
}