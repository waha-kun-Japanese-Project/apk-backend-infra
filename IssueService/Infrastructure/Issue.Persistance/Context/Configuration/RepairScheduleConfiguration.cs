using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class RepairScheduleConfiguration : IEntityTypeConfiguration<RepairSchedule>
    {
        public void Configure(EntityTypeBuilder<RepairSchedule> builder)
        {
            builder.HasKey(rs => rs.Id);
            builder.Property(rs => rs.ScheduledDate).IsRequired();
            builder.Property(rs => rs.SlotStart).IsRequired();
            builder.Property(rs => rs.SlotEnd).IsRequired();
            builder.Property(rs => rs.FarmerNotified).IsRequired();
            builder.Property(rs => rs.Notes).HasMaxLength(2000);
            builder.HasIndex(rs => rs.IssueId).IsUnique();
        }
    }
}
