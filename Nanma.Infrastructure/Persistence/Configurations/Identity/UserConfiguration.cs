using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nanma.Domain.Identity;

namespace Nanma.Infrastructure.Persistence.Configurations.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.MpinHash)
            .HasMaxLength(500);

        builder.HasIndex(x => x.PhoneNumber).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.ApartmentUsers)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}
