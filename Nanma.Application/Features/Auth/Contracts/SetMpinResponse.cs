namespace Nanma.Application.Features.Auth.Contracts;

public sealed class SetMpinResponse
{
    public required bool Success { get; init; }
    public required string Message { get; init; }
}
