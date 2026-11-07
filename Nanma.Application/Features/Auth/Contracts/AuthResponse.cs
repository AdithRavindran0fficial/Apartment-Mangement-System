namespace Nanma.Application.Features.Auth.Contracts;

public sealed class AuthResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime AccessTokenExpiresAtUtc { get; init; }
    public required string TokenType { get; init; }
}
