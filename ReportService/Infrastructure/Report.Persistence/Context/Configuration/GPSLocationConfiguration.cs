using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities.Report;

namespace Report.Infrastructure.Persistence.Configurations;

public class GPSLocationConfiguration : IEntityTypeConfiguration<GPSLocation>
{
    public void Configure(EntityTypeBuilder<GPSLocation> builder)
    {
        builder.ToTable("GPSLocations");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Latitude)
               .IsRequired();

        builder.Property(g => g.Longitude)
               .IsRequired();
    }
}