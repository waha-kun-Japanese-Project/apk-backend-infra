using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class MaintenanceTeamConfiguration : IEntityTypeConfiguration<MaintenanceTeam>
    {
        public void Configure(EntityTypeBuilder<MaintenanceTeam> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(200);

            builder.HasMany(t => t.ResolutionActions)
                .WithOne(ra => ra.Team)
                .HasForeignKey(ra => ra.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(t => t.RepairSchedules)
                .WithOne(rs => rs.Team)
                .HasForeignKey(rs => rs.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}