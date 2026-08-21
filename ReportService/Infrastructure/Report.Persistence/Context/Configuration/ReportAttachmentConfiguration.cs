using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities.Report;

namespace Report.Persistence.Context.Configration
{
    public class ReportAttachmentConfiguration : IEntityTypeConfiguration<ReportAttachment>
    {
        public void Configure(EntityTypeBuilder<ReportAttachment> builder)
        {
            builder.ToTable("ReportAttachments");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Type)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.Url)
                  .HasMaxLength(500)
                  .IsRequired();

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.ReportId)
                .IsRequired();
        }
    }
}