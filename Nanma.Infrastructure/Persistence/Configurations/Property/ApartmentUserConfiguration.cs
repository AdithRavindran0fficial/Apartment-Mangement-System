using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nanma.Domain.Property;

namespace Nanma.Infrastructure.Persistence.Configurations.Property;

public class ApartmentUserConfiguration : IEntityTypeConfiguration<ApartmentUser>
{
    public void Configure(EntityTypeBuilder<ApartmentUser> builder)
    {
        builder.ToTable("apartment_users");

        builder.HasKey(x => new { x.ApartmentId, x.UserId });

        builder.HasOne(x => x.Apartment)
            .WithMany(x => x.ApartmentUsers)
            .HasForeignKey(x => x.ApartmentId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ApartmentUsers)
            .HasForeignKey(x => x.UserId);
    }
}
