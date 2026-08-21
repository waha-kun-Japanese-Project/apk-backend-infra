using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Issue.Domain.Entities.Issue;

namespace Issue.Persistence.Context.Configuration
{
    public class ExpertReviewsConfiguration : IEntityTypeConfiguration<ExpertReviews>
    {
        public void Configure(EntityTypeBuilder<ExpertReviews> builder)
        {
            builder.HasKey(er => er.Id);
            builder.Property(er => er.Decision).IsRequired();
            builder.Property(er => er.Notes).HasMaxLength(2000);
            builder.Property(er => er.ReviewedAt).IsRequired();
            builder.Property(er => er.ExpertId).IsRequired();
        }
    }
}