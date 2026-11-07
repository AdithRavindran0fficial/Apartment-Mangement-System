using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nanma.Domain.Property;

namespace Nanma.Infrastructure.Persistence.Configurations.Property;

public class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.ToTable("apartments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApartmentNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => new { x.BuildingId, x.ApartmentNumber }).IsUnique();

        builder.HasOne(x => x.Building)
            .WithMany(x => x.Apartments)
            .HasForeignKey(x => x.BuildingId);

        builder.HasMany(x => x.ApartmentUsers)
            .WithOne(x => x.Apartment)
            .HasForeignKey(x => x.ApartmentId);
    }
}
