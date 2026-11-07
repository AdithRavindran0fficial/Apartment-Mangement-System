using Microsoft.EntityFrameworkCore;
using Nanma.Application.Common.Abstractions;
using Nanma.Domain.Identity;
using Nanma.Infrastructure.Persistence;

namespace Nanma.Infrastructure.Repositories;

public class RefreshTokenRepository(NanmaDbContext dbContext) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetActiveByHashAndDeviceWithUserAsync(string tokenHash, string deviceId, CancellationToken cancellationToken)
    {
        return dbContext.RefreshTokens
            .Include(x => x.User)
            .ThenInclude(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(
                x => x.TokenHash == tokenHash && x.DeviceId == deviceId && x.RevokedAtUtc == null,
                cancellationToken);
    }

    public Task<List<RefreshToken>> GetActiveByUserAndDeviceAsync(Guid userId, string deviceId, CancellationToken cancellationToken)
    {
        return dbContext.RefreshTokens
            .Where(x => x.UserId == userId && x.DeviceId == deviceId && x.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);
    }

    public void Add(RefreshToken refreshToken)
    {
        dbContext.RefreshTokens.Add(refreshToken);
    }
}
