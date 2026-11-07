using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nanma.Domain.Property;

namespace Nanma.Infrastructure.Persistence.Configurations.Property;

public class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        builder.ToTable("buildings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(50);

        builder.Property(x => x.AddressLine)
            .HasMaxLength(300);

        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasMany(x => x.Apartments)
            .WithOne(x => x.Building)
            .HasForeignKey(x => x.BuildingId);
    }
}
