using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class IssueAttachmentConfiguration : IEntityTypeConfiguration<IssueAttachment>
    {
        public void Configure(EntityTypeBuilder<IssueAttachment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Type).IsRequired();
            builder.Property(a => a.Purpose).IsRequired();
            builder.Property(a => a.Url).IsRequired().HasMaxLength(500);

            builder.HasOne(a => a.AiAnalysis)
                .WithOne(ai => ai.IssueAttachment)
                .HasForeignKey<AiAnalysis>(ai => ai.IssueAttachmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}