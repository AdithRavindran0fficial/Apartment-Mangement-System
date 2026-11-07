namespace Nanma.Application.Features.Auth.Contracts;

public sealed class RefreshRequest
{
    public required string RefreshToken { get; init; }
    public required string DeviceId { get; init; }
    public string? UserAgent { get; init; }
    public string? IpAddress { get; init; }
}
