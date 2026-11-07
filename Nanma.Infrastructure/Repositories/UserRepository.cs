using Microsoft.EntityFrameworkCore;
using Nanma.Application.Common.Abstractions;
using Nanma.Domain.Identity;
using Nanma.Infrastructure.Persistence;

namespace Nanma.Infrastructure.Repositories;

public class UserRepository(NanmaDbContext dbContext) : IUserRepository
{
    public Task<User?> GetActiveByPhoneNumberWithRolesAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return dbContext.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.IsActive, cancellationToken);
    }

    public Task<User?> GetByPhoneNumberWithRolesAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return dbContext.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return dbContext.Users
            .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}
