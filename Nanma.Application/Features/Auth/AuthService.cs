using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Nanma.Application.Common.Abstractions;
using Nanma.Application.Features.Auth.Contracts;
using Nanma.Domain.Exceptions;
using Nanma.Domain.Identity;

namespace Nanma.Application.Features.Auth;

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenGenerator tokenGenerator,
    IClock clock,
    IValidator<LoginRequest> loginValidator,
    IValidator<RefreshRequest> refreshValidator,
    IValidator<LogoutRequest> logoutValidator,
    IValidator<LoginWithMpinRequest> loginWithMpinValidator,
    IValidator<VerifyUserRequest> verifyUserValidator,
    IValidator<SetMpinRequest> setMpinValidator) : IAuthService
{
    private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        await loginValidator.ValidateAndThrowAsync(request, cancellationToken);

        var phone = request.PhoneNumber.Trim();

        var user = await userRepository.GetActiveByPhoneNumberWithRolesAsync(phone, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid phone number or password.", "invalid_credentials");
        }

        return await CreateSessionAsync(user, request.DeviceId, request.IpAddress, request.UserAgent, cancellationToken);
    }

    public async Task<AuthResponse> LoginWithMpinAsync(LoginWithMpinRequest request, CancellationToken cancellationToken)
    {
        await loginWithMpinValidator.ValidateAndThrowAsync(request, cancellationToken);

        var phone = request.PhoneNumber.Trim();

        var user = await userRepository.GetActiveByPhoneNumberWithRolesAsync(phone, cancellationToken);

        if (user is null || string.IsNullOrEmpty(user.MpinHash) || !passwordHasher.Verify(request.Mpin, user.MpinHash))
        {
            throw new UnauthorizedException("Invalid phone number or MPIN.", "invalid_mpin");
        }

        return await CreateSessionAsync(user, request.DeviceId, request.IpAddress, request.UserAgent, cancellationToken);
    }

    public async Task<VerifyUserResponse> VerifyUserAsync(VerifyUserRequest request, CancellationToken cancellationToken)
    {
        await verifyUserValidator.ValidateAndThrowAsync(request, cancellationToken);

        var phone = request.PhoneNumber.Trim();

        var user = await userRepository.GetByPhoneNumberWithRolesAsync(phone, cancellationToken);

        if (user is null)
        {
            return new VerifyUserResponse
            {
                UserExists = false,
                Message = "User not existing"
            };
        }

        var mpinExists = !string.IsNullOrEmpty(user.MpinHash);
        var nameExists = !string.IsNullOrWhiteSpace(user.FullName);

        var message = (mpinExists, nameExists) switch
        {
            (true, true) => "User is existing and mpin is set and name exists",
            (true, false) => "User is existing and mpin is set and name not exists",
            (false, true) => "User and name is existing but mpin is not set",
            (false, false) => "User is existing but mpin and name is not set"
        };

        return new VerifyUserResponse
        {
            UserExists = true,
            MpinExists = mpinExists,
            NameExists = nameExists,
            Message = message
        };
    }

    public async Task<SetMpinResponse> SetMpinAsync(SetMpinRequest request, CancellationToken cancellationToken)
    {
        await setMpinValidator.ValidateAndThrowAsync(request, cancellationToken);

        if (request.Mpin != request.ConfirmMpin)
        {
            return new SetMpinResponse { Success = false, Message = "MPIN Mismatch" };
        }

        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return new SetMpinResponse { Success = false, Message = "User not found" };
        }

        user.MpinHash = passwordHasher.Hash(request.Mpin);
        user.UpdatedAtUtc = clock.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SetMpinResponse { Success = true, Message = "MPIN updated successfully" };
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken)
    {
        await refreshValidator.ValidateAndThrowAsync(request, cancellationToken);

        var refreshTokenHash = ComputeHash(request.RefreshToken);

        var storedToken = await refreshTokenRepository.GetActiveByHashAndDeviceWithUserAsync(refreshTokenHash, request.DeviceId, cancellationToken);

        if (storedToken is null)
        {
            throw new UnauthorizedException("Refresh token is invalid.", "invalid_refresh_token");
        }

        if (storedToken.ExpiresAtUtc <= clock.UtcNow)
        {
            storedToken.RevokedAtUtc = clock.UtcNow;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedException("Refresh token is expired.", "expired_refresh_token");
        }

        storedToken.RevokedAtUtc = clock.UtcNow;

        return await CreateSessionAsync(storedToken.User, request.DeviceId, request.IpAddress, request.UserAgent, cancellationToken);
    }

    public async Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
    {
        await logoutValidator.ValidateAndThrowAsync(request, cancellationToken);

        var activeTokens = await refreshTokenRepository.GetActiveByUserAndDeviceAsync(request.UserId, request.DeviceId, cancellationToken);

        if (activeTokens.Count == 0)
        {
            return;
        }

        var now = clock.UtcNow;
        foreach (var token in activeTokens)
        {
            token.RevokedAtUtc = now;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<AuthResponse> CreateSessionAsync(User user, string deviceId, string? ipAddress, string? userAgent, CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;

        var existingActiveTokens = await refreshTokenRepository.GetActiveByUserAndDeviceAsync(user.Id, deviceId, cancellationToken);
        foreach (var token in existingActiveTokens)
        {
            token.RevokedAtUtc = now;
        }

        var refreshToken = tokenGenerator.GenerateRefreshToken();
        var refreshTokenHash = ComputeHash(refreshToken);

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            DeviceId = deviceId,
            TokenHash = refreshTokenHash,
            ExpiresAtUtc = now.Add(RefreshTokenLifetime),
            CreatedAtUtc = now,
            CreatedIpAddress = ipAddress,
            CreatedUserAgent = userAgent
        };

        refreshTokenRepository.Add(refreshTokenEntity);

        var roleNames = user.UserRoles
            .Select(x => x.Role.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var accessTokenExpiresAtUtc = now.Add(AccessTokenLifetime);
        var accessToken = tokenGenerator.GenerateAccessToken(user.Id, user.PhoneNumber, roleNames, accessTokenExpiresAtUtc);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAtUtc = accessTokenExpiresAtUtc,
            TokenType = "Bearer"
        };
    }

    private static string ComputeHash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}
