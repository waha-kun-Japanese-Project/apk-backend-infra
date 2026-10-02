using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities.Issue;

namespace Report.Infrastructure.Persistence.Configurations;

// NEW: same bug as IssueAiAnalysisConfiguration, just not reached yet because
// EF throws on the first colliding table it validates. Report.Domain.Entities.Report.GPSLocation
// already has GPSLocationConfiguration -> ToTable("GPSLocations"); this
// Issue-namespace GPSLocation was defaulting to the same table name by
// convention. Fixed preemptively so this doesn't become the next identical
// error immediately after the AiAnalysis fix deploys.
public class IssueGPSLocationConfiguration : IEntityTypeConfiguration<GPSLocation>
{
    public void Configure(EntityTypeBuilder<GPSLocation> builder)
    {
        builder.ToTable("IssueGPSLocations");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Latitude)
               .IsRequired();

        builder.Property(g => g.Longitude)
               .IsRequired();

        // Issue.GPSLocationId is the FK property already declared on Issue
        // itself, pointing at this entity's Id - EF's convention already
        // picks this up correctly by name, no explicit HasForeignKey needed.
    }
}