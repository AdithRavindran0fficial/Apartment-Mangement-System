using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nanma.Domain.Identity;

namespace Nanma.Infrastructure.Persistence.Configurations.Identity;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TokenHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.DeviceId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedIpAddress)
            .HasMaxLength(64);

        builder.Property(x => x.CreatedUserAgent)
            .HasMaxLength(500);

        builder.HasIndex(x => x.TokenHash)
            .IsUnique();

        builder.HasIndex(x => new { x.UserId, x.DeviceId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId);
    }
}
