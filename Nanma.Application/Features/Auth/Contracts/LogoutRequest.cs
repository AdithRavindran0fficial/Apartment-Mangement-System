namespace Nanma.Application.Features.Auth.Contracts;

public sealed class LogoutRequest
{
    public required Guid UserId { get; init; }
    public required string DeviceId { get; init; }
}
