using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nanma.Domain.Common;

namespace Nanma.Infrastructure.Persistence.Configurations.Common;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("activities");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ActivityLog).HasMaxLength(500).IsRequired();
        builder.Property(x => x.PageValue).HasMaxLength(100);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        builder.HasIndex(x => new { x.UserId, x.ApartmentId });
    }
}
