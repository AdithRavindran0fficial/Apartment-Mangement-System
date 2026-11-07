using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nanma.Application.Common.Abstractions;

namespace Nanma.Infrastructure.Auth;

public class JwtTokenGenerator(IConfiguration configuration) : ITokenGenerator
{
    private readonly string _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
    private readonly string _audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");
    private readonly string _signingKey = configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey is missing.");

    public string GenerateAccessToken(Guid userId, string phoneNumber, IReadOnlyCollection<string> roles, DateTime expiresAtUtc)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, phoneNumber),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.MobilePhone, phoneNumber)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
