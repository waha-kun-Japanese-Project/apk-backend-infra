using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class AiAnalysisConfiguration : IEntityTypeConfiguration<AiAnalysis>
    {
        public void Configure(EntityTypeBuilder<AiAnalysis> builder)
        {
            builder.HasKey(ai => ai.Id);

            builder.Property(ai => ai.ProblemName).IsRequired().HasMaxLength(200);
            builder.Property(ai => ai.ProblemArabic).HasMaxLength(200);
            builder.Property(ai => ai.Confidence).IsRequired();
            builder.Property(ai => ai.Severity).IsRequired().HasMaxLength(50);
            builder.Property(ai => ai.Recommendation).HasMaxLength(2000);
            builder.Property(ai => ai.Explanation).HasMaxLength(2000);
            builder.Property(ai => ai.ModelVersion).HasMaxLength(50);

            builder.HasIndex(ai => ai.IssueAttachmentId).IsUnique();
        }
    }
}