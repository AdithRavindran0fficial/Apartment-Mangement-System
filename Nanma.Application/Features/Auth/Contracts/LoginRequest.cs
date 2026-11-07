namespace Nanma.Application.Features.Auth.Contracts;

public sealed class LoginRequest
{
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public required string DeviceId { get; init; }
    public string? UserAgent { get; init; }
    public string? IpAddress { get; init; }
}
