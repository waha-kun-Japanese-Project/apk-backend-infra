using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities.Report;

namespace Report.Infrastructure.Persistence.Configurations;

public class AiAnalysisConfiguration : IEntityTypeConfiguration<AiAnalysis>
{
    public void Configure(EntityTypeBuilder<AiAnalysis> builder)
    {
        builder.ToTable("AiAnalyses");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Confidence)
               .IsRequired();

        builder.Property(a => a.Severity)
               .HasConversion<string>();

     builder.Property(a => a.ProblemName)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(a => a.Recommendation)
               .HasMaxLength(1000);

        builder.Property(a => a.Explanation)
               .HasMaxLength(2000);

        builder.Property(a => a.ModelVersion)
               .HasMaxLength(100);
    }
}