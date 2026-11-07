using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nanma.Domain.Identity;

namespace Nanma.Infrastructure.Persistence.Configurations.Identity;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("profiles");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasMaxLength(100);

        builder.Property(x => x.AlternatePhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Gender)
            .HasMaxLength(30);

        builder.Property(x => x.AddressLine)
            .HasMaxLength(300);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Profile)
            .HasForeignKey<Profile>(x => x.UserId);
    }
}
