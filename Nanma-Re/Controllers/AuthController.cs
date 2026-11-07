using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nanma.Application.Features.Auth;
using Nanma.Application.Features.Auth.Contracts;
using Nanma.Domain.Exceptions;

namespace Nanma_Re.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("login-mpin")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> LoginWithMpin([FromBody] LoginWithMpinRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginWithMpinAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("verify-user")]
    [AllowAnonymous]
    public async Task<ActionResult<VerifyUserResponse>> VerifyUser([FromBody] VerifyUserRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.VerifyUserAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("mpin")]
    [Authorize]
    public async Task<ActionResult<SetMpinResponse>> SetMpin([FromBody] SetMpinRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.SetMpinAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.RefreshAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedException("User identifier is missing in token.", "invalid_user_token");
        }

        var normalizedRequest = new LogoutRequest
        {
            UserId = userId,
            DeviceId = request.DeviceId
        };

        await authService.LogoutAsync(normalizedRequest, cancellationToken);
        return NoContent();
    }
}
