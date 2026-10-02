using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities.Issue;

namespace Report.Infrastructure.Persistence.Configurations;

// NEW: Report.Domain.Entities.Issue.AiAnalysis previously had no explicit
// configuration at all - it only existed as IssueDbContext's DbSet<AiAnalysis>,
// which defaulted (by convention) to a table literally named "AiAnalyses",
// the exact same table name Report.Domain.Entities.Report.AiAnalysis's
// configuration also used. Distinct table name here fixes that collision.
public class IssueAiAnalysisConfiguration : IEntityTypeConfiguration<AiAnalysis>
{
    public void Configure(EntityTypeBuilder<AiAnalysis> builder)
    {
        builder.ToTable("IssueAiAnalyses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Confidence)
               .IsRequired();

        builder.Property(a => a.Severity)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(a => a.ProblemName)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(a => a.Recommendation)
               .HasMaxLength(1000);

        builder.Property(a => a.Explanation)
               .HasMaxLength(2000);

        builder.Property(a => a.ModelVersion)
               .HasMaxLength(100);

        // Issue.AiAnalyses is an ICollection<AiAnalysis> with no FK property
        // declared on AiAnalysis itself - this creates the shadow "IssueId"
        // FK column EF needs to actually link the two tables, which is the
        // "linking relationship" the original error said was missing.
        builder.HasOne<Issue>()
               .WithMany(i => i.AiAnalyses)
               .HasForeignKey("IssueId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}