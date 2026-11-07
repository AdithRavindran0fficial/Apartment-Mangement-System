using Nanma.Domain.Identity;

namespace Nanma.Application.Common.Abstractions;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetActiveByHashAndDeviceWithUserAsync(string tokenHash, string deviceId, CancellationToken cancellationToken);
    Task<List<RefreshToken>> GetActiveByUserAndDeviceAsync(Guid userId, string deviceId, CancellationToken cancellationToken);
    void Add(RefreshToken refreshToken);
}
