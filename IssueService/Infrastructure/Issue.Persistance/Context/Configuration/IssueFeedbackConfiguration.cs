using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class IssueFeedbackConfiguration : IEntityTypeConfiguration<IssueFeedback>
    {
        public void Configure(EntityTypeBuilder<IssueFeedback> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Rating).IsRequired();
            builder.Property(f => f.Comment).HasMaxLength(2000);
            builder.HasIndex(f => f.IssueId).IsUnique();
        }
    }
}
