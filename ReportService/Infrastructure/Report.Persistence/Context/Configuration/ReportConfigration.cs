using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities.Report;

namespace Report.Persistence.Context.Configuration
{
    public class ReportConfiguration : IEntityTypeConfiguration<Domain.Entities.Report.Report>
    {
      

        public void Configure(EntityTypeBuilder<Domain.Entities.Report.Report> builder)
        {
            builder.ToTable("Reports");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Description)
                .HasMaxLength(1000);

            builder.Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.ReporterId)
                .IsRequired();

            builder.HasOne(r => r.Analysis)
                .WithOne(a => a.Report)
                .HasForeignKey<AiAnalysis>(a => a.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

        
            builder.HasOne(r => r.Location)
                .WithOne(l => l.Report)
                .HasForeignKey < Domain.Entities.Report.Report > (r => r.LocationId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Attachments)
                .WithOne(a => a.Report)
                .HasForeignKey(a => a.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
