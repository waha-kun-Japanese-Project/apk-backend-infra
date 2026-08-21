using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class StatusHistoryConfiguration : IEntityTypeConfiguration<StatusHistory>
    {
        public void Configure(EntityTypeBuilder<StatusHistory> builder)
        {
            builder.HasKey(sh => sh.Id);
            builder.Property(sh => sh.Status).IsRequired();
            builder.Property(sh => sh.Note).HasMaxLength(2000);
            builder.Property(sh => sh.ChangedAt).IsRequired();
            builder.Property(sh => sh.ChangedById).IsRequired();
        }
    }
}