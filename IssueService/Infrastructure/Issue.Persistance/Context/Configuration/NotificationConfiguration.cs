using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
            builder.Property(n => n.Message).HasMaxLength(2000);
            builder.Property(n => n.Type).IsRequired();
            builder.Property(n => n.Read).IsRequired();
            builder.Property(n => n.UserId).IsRequired();
        }
    }
}