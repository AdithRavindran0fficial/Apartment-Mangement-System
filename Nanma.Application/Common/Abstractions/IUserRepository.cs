using Nanma.Domain.Identity;

namespace Nanma.Application.Common.Abstractions;

public interface IUserRepository
{
    Task<User?> GetActiveByPhoneNumberWithRolesAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByPhoneNumberWithRolesAsync(string phoneNumber, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}
