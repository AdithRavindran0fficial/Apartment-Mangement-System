namespace Nanma.Application.Common.Abstractions;

public interface ITokenGenerator
{
    string GenerateAccessToken(Guid userId, string phoneNumber, IReadOnlyCollection<string> roles, DateTime expiresAtUtc);
    string GenerateRefreshToken();
}
