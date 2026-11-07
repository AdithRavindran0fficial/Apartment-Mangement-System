namespace Nanma.Application.Features.Auth.Contracts;

public sealed class VerifyUserResponse
{
    public required bool UserExists { get; init; }
    public bool MpinExists { get; init; }
    public bool NameExists { get; init; }
    public required string Message { get; init; }
}
