namespace Nanma.Application.Features.Auth.Contracts;

public sealed class VerifyUserRequest
{
    public required string PhoneNumber { get; init; }
}
