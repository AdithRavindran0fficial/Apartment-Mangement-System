using Microsoft.EntityFrameworkCore;
using Nanma.Domain.Identity;
using Nanma.Domain.Property;

namespace Nanma.Infrastructure.Persistence;

public class NanmaDbContext(DbContextOptions<NanmaDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Building> Buildings => Set<Building>();
    public DbSet<Apartment> Apartments => Set<Apartment>();
    public DbSet<ApartmentUser> ApartmentUsers => Set<ApartmentUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NanmaDbContext).Assembly);
    }
}
