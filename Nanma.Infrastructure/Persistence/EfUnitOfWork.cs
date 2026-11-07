using Nanma.Application.Common.Abstractions;
using Nanma.Infrastructure.Persistence;

namespace Nanma.Infrastructure.Persistence;

public class EfUnitOfWork(NanmaDbContext dbContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
