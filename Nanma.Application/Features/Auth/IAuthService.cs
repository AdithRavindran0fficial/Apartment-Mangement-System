using Nanma.Application.Features.Auth.Contracts;

namespace Nanma.Application.Features.Auth;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> LoginWithMpinAsync(LoginWithMpinRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken);
    Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken);
    Task<VerifyUserResponse> VerifyUserAsync(VerifyUserRequest request, CancellationToken cancellationToken);
    Task<SetMpinResponse> SetMpinAsync(SetMpinRequest request, CancellationToken cancellationToken);
}
